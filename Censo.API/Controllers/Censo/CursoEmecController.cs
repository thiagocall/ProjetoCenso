using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;

namespace Censo.API.Controllers.Censo
{
    [Route("api/v1/Censo/[controller]")]
    [ApiController]
    public class CursoEmecController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        // public ProfessorCursoEmecContext Context { get; }
        public ProfessorIESContext ProfContext { get; }
        public CensoContext Context {get; }

        public TempProducaoContext ProducaoContext { get; set; }

        public CursoEnquadramentoContext CursoEnquadramentoContext;

        public IOtimizacao Otm { get; }

        public Dictionary<long?, PrevisaoSKU> ListaPrevisaoSKU;


        // public CursoCensoContext CursoCensoContext { get; set; }

        public CursoEmecController(CensoContext _context, ProfessorIESContext _profcontext, IConfiguration _configuration, IOtimizacao _otm, CursoEnquadramentoContext _cursoEnquadContext, TempProducaoContext _producaoContext)
        {
            this.Context = _context;
            this.ProfContext = _profcontext;
            this.Configuration = _configuration;
            this.CursoEnquadramentoContext = _cursoEnquadContext;
            this.Otm = _otm;
            this.ProducaoContext = _producaoContext;
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ProfContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.CursoEnquadramentoContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet("geraPrevisao/{id}/{tipo}")]
        public async Task<IActionResult> Get([FromQuery]long id, string tipo)
        {
            var query = await Context.ProfessorCursoEmec.ToListAsync();

            

            // var cod = prof.CodCurso;

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();
            var ListaCursoArea = await this.CursoEnquadramentoContext.CursoEnquadramento.ToListAsync();


            // ########## Monta a lista de cursos por professores ##########
            cursoProfessor = MontaCursoProfessor(query, ListaCursoArea);

            var results = cursoProfessor.ToList();

            return Ok(results);

            //var results = GeraPrevisao(id);
        }


        [HttpGet("GeraNota/{id}")]
        public async Task<IActionResult> GeraNota(long? id)
        {

            List<CursoPrevisao> listaPrev = await Task.Run( () => {
                return PrevisaoEmec.getPrevisao(this.Configuration);
            });

            var query = listaPrev.Where(x => x.CodArea == id).ToList();

            return Ok(query);

        }

        [HttpGet("GeraNota/{_id}/{_tipo}")]
        public ActionResult Previsao(long? _id, string _tipo)
        {
            double?[] prev = new double?[2];

            List<CursoPrevisao> listaPrev = PrevisaoEmec.getPrevisao(this.Configuration);

            var query = listaPrev.Where(x => x.CodArea == _id).OrderBy(x => x.Ano).ToList();


            prev = GeraPrevisao(_id, _tipo, query);

            List<string> tipos = new List<string>() { "D", "R", "M" };

            if (tipos.Contains(_tipo.ToUpper()))
            {

                prev[0] = (prev[0] < 0) ? 0 : prev[0];
                prev[0] = (prev[0] > 5) ? 5 : prev[0];

                prev[1] = (prev[1] < 0) ? 0 : prev[1];
                prev[1] = (prev[1] > 5) ? 5 : prev[1];

            }


            return Ok(prev);

        }


        [HttpGet("ObterResultados")]
        public async Task<IActionResult> obterResultados()
        {
            var query = await this.ProducaoContext.TbResultado
                             .Select(x => new {x.Id, x.Resumo, x.TempoExecucao})
                             .OrderByDescending(x => x.Id)
                            .ToArrayAsync();
    
            return Ok(query);
        }


        [HttpGet("ObterResultados/{_id}")]
        public ActionResult obterResultadosporId(long _id)
        {
            var resultado = this.ProducaoContext.TbResultado.Select(x => new {x.Id, x.Resumo}).FirstOrDefault(x => x.Id == _id);
            var resultadoAtual = this.ProducaoContext.TbResultadoAtual.Select(x => new {x.Id, x.Resumo}).FirstOrDefault(x => x.Id == _id);

            var resultadoCompleto = new {resultado, resultadoAtual};
            return Ok(resultadoCompleto);
        }

           [HttpGet("ObterResultadosCompleto")]
        public async Task<ActionResult> obterResultadosCompleto()
        {
            var query = this.ProducaoContext.TbResultado.ToListAsync();

            return Ok(await query);
        }

        [HttpDelete("{id}")]
        // DELETE api/values/5
        public async Task<IActionResult> Delete(long id)
        {
            var item = this.ProducaoContext.TbResultado.Find(id); 
            this.ProducaoContext.Remove(item); 
            await this.ProducaoContext.SaveChangesAsync(); //comitar a alteração do dado
            return Ok();
        }


        [HttpGet("obterInfoCurso/{codCurso}")]
        public async Task<IActionResult> getDadosCursoEmec(long codCurso)
        {

            double notaM = 0;
            double notaD = 0;
            double notaR = 0;
            Dictionary<long?, PrevisaoSKU> ListaPrevisaoSKU;

            var ListaCursoArea = await this.CursoEnquadramentoContext.CursoEnquadramento.ToListAsync();

            var query = await this.Context.ProfessorCursoEmec
                                .Where(x => x.CodEmec == codCurso).ToListAsync();


            Task<Dictionary<long?, PrevisaoSKU>> task1 = Task.Factory.StartNew(
                () =>
                {
                    return GeraListaPrevisaoSKU();
                }
            );

            ListaPrevisaoSKU = await task1;
            var emec = this.Context.CursoCenso.Where(c => c.CodEmec == codCurso).First();
            var previsao = ListaPrevisaoSKU[emec.CodArea];

            List<CursoProfessor> cursoProfessor;

            cursoProfessor = MontaCursoProfessor(query,ListaCursoArea);

            var cursoEmec = cursoProfessor.First();
            var Professores = cursoEmec.Professores;

            double qtdProf = Professores.Count();
            double qtdD = Professores.Where(x => x.Titulacao == "DOUTOR")
                    .Count();
            double qtdM = Professores.Where(x => x.Titulacao == "MESTRE" | x.Titulacao == "DOUTOR")
                    .Count();
            double qtdR = Professores.Where(x => x.Regime == "TEMPO INTEGRAL" | x.Regime == "TEMPO PARCIAL")
                    .Count();

            double perc_D = qtdD / qtdProf;
            double perc_M = qtdM / qtdProf;
            double perc_R = qtdR / qtdProf;
            ////e.CodCampus, e.CodCurso, e.NumHabilitacao

            if (emec != null)
            {
                // percent.Add(perc_D);
                var area1 = emec.CodArea;
                //##### Previsão Doutor

                var prev_minM = previsao.P_Min_Mestre;
                var prev_maxM = previsao.P_Max_Mestre;

                var prev_minD = previsao.P_Min_Doutor;
                var prev_maxD = previsao.P_Max_Doutor;

                var prev_minR = previsao.P_Min_Regime;
                var prev_maxR = previsao.P_Max_Regime;

                notaM = (N_Escala(prev_minM, prev_maxM, perc_M)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minM, prev_maxM, perc_M));
                notaD = (N_Escala(prev_minD, prev_maxD, perc_D)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minD, prev_maxD, perc_D));
                notaR = (N_Escala(prev_minR, prev_maxR, perc_R)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minR, prev_maxR, perc_R));

            }

            return Ok(new { previsao, cursoProfessor = cursoEmec.Professores.ToList(), perc_M, perc_D, perc_R, notaM, notaD, notaR, qtdD, qtdM, qtdR });
        }



        [AllowAnonymous]
        [HttpGet("Resultado/Excel/{id}")]
        public async Task<IActionResult> ExportaResultado(long id) {

             try
            {
                var resultadoOTM = await this.ProducaoContext.TbResultado
                        .FirstOrDefaultAsync(r => r.Id == id);
                
                // Tratando dados para Excel

                var parametros =  new List<ParametrosCenso>();
                //var resultados = new List<Resultado>();

                parametros.Add(JsonConvert.DeserializeObject<ParametrosCenso>(resultadoOTM.Parametro));

                var resultados = JsonConvert.DeserializeObject<List<Resultado>>(resultadoOTM.Resultado);

                // var resultado = resultadoOTM.Resultado;

            //  Monta arquivo para Download em Excel

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream)) {                
                    var shParam = package.Workbook.Worksheets.Add("Parametros");
                    var shResumo = package.Workbook.Worksheets.Add("Resultado");
                    shParam.Cells.LoadFromCollection(parametros, true);
                    shResumo.Cells.LoadFromCollection(resultados, true);
                    package.Save();            
                };  

                    stream.Position = 0;
                    var contentType = "application/octet-stream";
                    var fileName = "Resultado.xlsx";

                return File(stream, contentType, fileName);
            }
            catch (System.Exception)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }        

        }

        // ########## Monta a lista de cursos por professores ##########

        // ################# Monta Cursos dos Professores ######################


        public List<CursoProfessor> MontaCursoProfessor([FromQuery] List<ProfessorCursoEmec> _profs, List<CursoEnquadramento> _CursoArea)
        {

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();

            var query = _profs;

            var CursoArea = _CursoArea.ToDictionary(x => x.codEmec);

            foreach (var res in query)
            {
                // Filtra parâmtetro indGraduacao
                if (!(res.Titulacao == null || res.Titulacao == "GRADUADO")) //res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado
                {

                    if (cursoProfessor.Where(c => c.CodEmec == res.CodEmec).Count() > 0)
                    {
                        CursoProfessor prof = cursoProfessor.Find(x => x.CodEmec == res.CodEmec);
                        prof.CodArea = (CursoArea.ContainsKey(Convert.ToInt32((prof.CodEmec)))) ? CursoArea[(Int32)prof.CodEmec].codArea : 9999;

                        if (prof.Professores.Where(x => x.cpfProfessor == res.CpfProfessor).Count() == 0)
                        {
                            ProfessorEmec pr = new ProfessorEmec
                            {
                                cpfProfessor = res.CpfProfessor,
                                Ativo = res.IndAtivo,
                                Regime = res.Regime,
                                Titulacao = res.Titulacao
                            };
                            //prof.Professores = new Dictionary<long, ProfessorEmec>();
                            prof.Professores.Add(pr);
                        }
                        
                    }
                    else
                    {
                        CursoProfessor prof = new CursoProfessor();
                        prof.CodEmec = res.CodEmec;
                        prof.Professores = new List<ProfessorEmec>();
                        ProfessorEmec pr = new ProfessorEmec
                        {
                            cpfProfessor = res.CpfProfessor,
                            Ativo = res.IndAtivo,
                            Regime = res.Regime,
                            Titulacao = res.Titulacao
                        };
                        prof.Professores.Add(pr);
                        cursoProfessor.Add(prof);

                    }
                }
            }

            return cursoProfessor;

        }


        public List<CursoProfessor> MontaCursoProfessor20P([FromQuery] List<ProfessorCursoEmec> _profs, List<CursoEnquadramento> _CursoArea)
        {

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();

            var query = _profs;

            var CursoArea = _CursoArea.ToDictionary(x => x.codEmec);

            foreach (var res in query)
            {
                // Filtra parâmtetro indGraduacao
                if (!(res.Titulacao == null || res.Titulacao == "GRADUADO")) //res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado
                {

                    if (cursoProfessor.Where(c => c.CodEmec == res.CodEmec).Count() > 0)
                    {
                        CursoProfessor prof = cursoProfessor.Find(x => x.CodEmec == res.CodEmec);
                        prof.CodArea = (CursoArea.ContainsKey(Convert.ToInt32((prof.CodEmec)))) ? CursoArea[(Int32)prof.CodEmec].codArea : 9999;

                        if (prof.Professores.Where(x => x.cpfProfessor == res.CpfProfessor).Count() == 0)
                        {
                            ProfessorEmec pr = new ProfessorEmec
                            {
                                cpfProfessor = res.CpfProfessor,
                                Ativo = res.IndAtivo,
                                Regime = res.Regime,
                                Titulacao = res.Titulacao
                            };
                            //prof.Professores = new Dictionary<long, ProfessorEmec>();
                            prof.Professores.Add(pr);
                        }
                        
                    }
                    else
                    {
                        CursoProfessor prof = new CursoProfessor();
                        prof.CodEmec = res.CodEmec;
                        prof.Professores = new List<ProfessorEmec>();
                        ProfessorEmec pr = new ProfessorEmec
                        {
                            cpfProfessor = res.CpfProfessor,
                            Ativo = res.IndAtivo,
                            Regime = res.Regime,
                            Titulacao = res.Titulacao
                        };
                        prof.Professores.Add(pr);
                        cursoProfessor.Add(prof);

                    }
                }
            }

            return cursoProfessor;

        }

        //################## Previsão ################################

        private double?[] GeraPrevisao(long? _id, string _tipo, List<CursoPrevisao> _query)
        {


            double?[] prev = new double?[2];

            var anoAtual = _query.Max(x => x.Ano) + 3;

            switch (_tipo.ToUpper())
            {
                case "M":
                    prev[0] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Min_Mestre).ToList());
                    prev[1] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Mestre).ToList());
                    break;

                case "D":
                    prev[0] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Min_Doutor).ToList());
                    prev[1] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Doutor).ToList());
                    break;

                case "R":
                    prev[0] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Min_Regime).ToList());
                    prev[1] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Regime).ToList());
                    break;

                /*/ case "I":
                 prev[0] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Avg_Infra).ToList());
                 //prev[1] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Regime).ToList());
                 break;
                 case "O":
                 prev[0] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Avg_OP).ToList());
                 //prev[1] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Regime).ToList());
                 break;
                 case "C":
                 prev[0] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Avg_CE).ToList());
                 //prev[1] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Regime).ToList());
                 break;
                 case "A":
                 prev[0] = MontaPrevisao(2019, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Avg_AF).ToList());
                 //prev[1] = MontaPrevisao(2019, query.Select(c => (double?)c.Ano).ToList(), query.Select(c => c.Max_Regime).ToList());
                 break;
                 */
                default:
                    break;
            }

            //var t = MontaPrevisao(2019, query.Select(c => c.Ano).ToList(), query.Select(c => c.Max_Mestre).ToList());
            return prev;

        }

        private double? MontaPrevisao(long? alvo, List<double?> x, List<double?> y)
        {

            // calcula a regressão linear pelo ano atual
            //a = avg(y) - (b * avg(x))
            //b = sum((x - avg(x))* (y - avg(y))) / sum((x - avg(y)^2))
            //alvo = a + b * x
            double? a = 0;
            double? b = 0;
            double? x_avg = x.Average();
            double? y_avg = y.Average();
            List<double?> x_dev = new List<double?>();
            List<double?> y_dev = new List<double?>();
            double? b1 = 0;
            double? b2 = 0;
            double? res = 0;

            foreach (var item in x)
            {
                x_dev.Add((item - x_avg));

            }
            foreach (var item in y)
            {
                y_dev.Add(item - y_avg);
            }

            for (int i = 0; i < x.Count(); i++)
            {
                b1 += x_dev[i] * y_dev[i];
                b2 += Math.Pow((double)x_dev[i], 2);
            }

            b = b1 / b2;
            a = y_avg - (b * x_avg);

            res = (a + b * alvo);

            //res = (res > 1) ? 1 : res;
            //res = (res < 0) ? 0 : res;

            return res;

        }

        private Dictionary<long?, PrevisaoSKU> GeraListaPrevisaoSKU()
        {

            // if (ListaPrevisaoSKU != null)
            // {
            //     return;
            // }

            List<CursoPrevisao> listaPrev = PrevisaoEmec.getPrevisao(this.Configuration);

            Dictionary<long?, PrevisaoSKU> ListaPrevisaoSKU = new Dictionary<long?, PrevisaoSKU>();

            PrevisaoSKU prev;

            var areas = listaPrev.Select(x => x.CodArea).Distinct().ToList();

            foreach (var item in areas)
            {
                var query = listaPrev.Where(x => x.CodArea == item).ToList();
                var resM = GeraPrevisao(item, "M", query);
                var resD = GeraPrevisao(item, "D", query);
                var resR = GeraPrevisao(item, "R", query);

                resM[0] = (resM[0] < 0) ? 0 : resM[0];
                resM[0] = (resM[0] > 1) ? 1 : resM[0];

                resM[1] = (resM[1] < 0) ? 0 : resM[1];
                resM[1] = (resM[1] > 1) ? 1 : resM[1];

                resD[0] = (resD[0] < 0) ? 0 : resD[0];
                resD[0] = (resD[0] > 1) ? 1 : resD[0];

                resD[1] = (resD[1] < 0) ? 0 : resD[1];
                resD[1] = (resD[1] > 1) ? 1 : resD[1];

                resR[0] = (resR[0] < 0) ? 0 : resR[0];
                resR[0] = (resR[0] > 1) ? 1 : resR[0];

                resR[1] = (resR[1] < 0) ? 0 : resR[1];
                resR[1] = (resR[1] > 1) ? 1 : resR[1];


                prev = new PrevisaoSKU();
                prev.CodArea = item;
                prev.P_Min_Mestre = resM[0];
                prev.P_Max_Mestre = resM[1];
                prev.P_Min_Doutor = resD[0];
                prev.P_Max_Doutor = resD[1];
                prev.P_Min_Regime = resR[0];
                prev.P_Max_Regime = resR[1];

                ListaPrevisaoSKU.Add(prev.CodArea, prev);

            };

            return ListaPrevisaoSKU;

        }

        //#################### Gera notas para cursos #####################
        private dynamic getNotaCursos(List<ProfessorCursoEmec> _query, List<CursoEnquadramento> _cursoProfessor)
        {

             var query = _query;

            var ListaPrevisaoSKU = GeraListaPrevisaoSKU();

            //List<CursoPrevisao> listaPrev = PrevisaoEmec.getPrevisao(this.Configuration);

            List<CursoProfessor> cursoProfessor;

            // ########## Monta a lista de cursos por professores ##########
            cursoProfessor = MontaCursoProfessor(query, _cursoProfessor);
            var cctx = this.Context.CursoCenso.ToList();
            List<double> percent = new List<double>();
            //conte

            // ######## Calcula Nota Prévia dos Cursos ###########

            foreach (var item in cursoProfessor)
            {
                double qtdProf = item.Professores
                        .Count();
                double qtdD = item.Professores.Where(x => x.Titulacao == "DOUTOR")
                        .Count();
                double qtdM = item.Professores.Where(x => x.Titulacao == "MESTRE" | x.Titulacao == "DOUTOR")
                        .Count();
                double qtdR = item.Professores.Where(x => x.Regime == "TEMPO INTEGRAL" | x.Regime == "TEMPO PARCIAL")
                        .Count();

                double perc_D = qtdD / qtdProf;
                double perc_M = qtdM / qtdProf;
                double perc_R = qtdR / qtdProf;
                ////e.CodCampus, e.CodCurso, e.NumHabilitacao
                var ii = cctx.FirstOrDefault(x => x.CodEmec == item.CodEmec);

                if (ii != null)
                {
                    percent.Add(perc_D);
                    var area = ii.CodArea;
                    //##### Previsão Doutor

                    //double?[] prev = new double?[2];

                    //var query2 = listaPrev.Where(x => x.CodArea == area).OrderBy(x => x.Ano).ToList();

                    if (ListaPrevisaoSKU.ContainsKey(area))
                    {
                        var prev = ListaPrevisaoSKU[area];

                        var prev_minM = prev.P_Min_Mestre;
                        var prev_maxM = prev.P_Max_Mestre;

                        var prev_minD = prev.P_Min_Doutor;
                        var prev_maxD = prev.P_Max_Doutor;

                        var prev_minR = prev.P_Min_Regime;
                        var prev_maxR = prev.P_Max_Regime;

                        double notaM = (N_Escala(prev_minM, prev_maxM, perc_M)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minM, prev_maxM, perc_M));
                        double notaD = (N_Escala(prev_minD, prev_maxD, perc_D)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minD, prev_maxD, perc_D));
                        double notaR = (N_Escala(prev_minR, prev_maxR, perc_R)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minR, prev_maxR, perc_R));

                        item.Nota_Mestre = notaM;
                        item.Nota_Doutor = notaD;
                        item.Nota_Regime = notaR;

                    }

                }

            }

            //var result = cursoProfessor.Select(x => x.Nota_Mestre).ToList();
            var result = cursoProfessor
                            .Select(x => new
                            {
                                x.CodEmec,
                                x.Nota_Mestre,
                                x.Nota_Doutor,
                                x.Nota_Regime,
                                Mestres = x.Professores
                                               .Where(p => p.Titulacao == "MESTRE" || p.Titulacao == "DOUTOR")
                                               .Count(),
                                QtdProfessores = x.Professores.Count(),
                                doutores = x.Professores
                                               .Where(p => p.Titulacao == "DOUTOR").Count(),
                                cctx.FirstOrDefault(c => c.CodEmec == x.CodEmec).CodArea,
                                cctx.FirstOrDefault(c => c.CodEmec == x.CodEmec).NomCursoCenso,
                                    // Professores = x.Professores,
                                })
                                    .ToList();

            return result;

        }

        private double? N_Escala(double? lim_min, double? lim_max, double? percent)
        {

            double? n;

            try
            {

                n = (percent - lim_min) / (lim_max - lim_min) * 5;
                if (n < 0)
                {
                    return 0;
                }
                else if (n > 5)
                {
                    return 5;
                }

                else
                {
                    double? n1 = (n == null) ? (double?)0 : (double?)Math.Round((decimal)n, 4);
                    return  n1;
                }

            }
            catch (System.Exception)
            {

                return 0;
            }

        }



        #region httpVerbs
        // POST api/values
        [HttpPost("Otimizar")]
        public async Task<IActionResult> Otimizar([FromBody] ParametrosCenso _formulario)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();


            var TaskEnade = await Task.Run( () => {

                return this.Context.CursoCenso.ToList();

            });

            var ResId = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));

                // int QtdCursos = 0;
                // int Nota1a2 = 0;
                // int Nota3 = 0;
                // int Nota4a5 = 0;
                // int qtdD_1a2 = 0;
                // int qtdD_3a5 = 0;
                // int qtdM_1a2 = 0;
                // int qtdM_3a5 = 0;
                // int qtdR_1a2 = 0;
                // int qtdR_3a5 = 0;

            try
            {

                var query = this.Context.ProfessorCursoEmec.ToListAsync();
                var query20p = this.Context.ProfessorCursoEmec20p.ToListAsync();
                var ListaCursoArea = this.CursoEnquadramentoContext.CursoEnquadramento.ToListAsync();
                var ListaPrevisaoSKU = GeraListaPrevisaoSKU();
                var Cursoprofessor = MontaCursoProfessor(await query, await ListaCursoArea);
                Otm.AddProfessor20p(Cursoprofessor, await query20p);
                // var Cursoprofessor20p = MontaCursoProfessor(await query20p, await ListaCursoArea);;

                // // Obtem lista dos professores escolhidos no filtro
                var lista = _formulario.MontaLista();

                var CursoNota = getNotaCursos(await query, await ListaCursoArea);

                List<CursoProfessor> cursoProfessorAtual = new List<CursoProfessor>();
                Cursoprofessor.ForEach( (item) => {
                    cursoProfessorAtual.Add(item);
                }
                    );

                var CursoEnade = TaskEnade.Where(x => x.IndEnade.Contains('S')).Select(c => c.CodEmec.ToString()).Distinct().ToList();

                List<Resultado> ResultadoAtual = Otm.CalculaNotaCursos(ListaPrevisaoSKU, cursoProfessorAtual, CursoEnade);


                List<Resultado> resultado = Otm.OtimizaCurso(ListaPrevisaoSKU, await query, Cursoprofessor, await ListaCursoArea, _formulario);
                
                // ############## Monta resultados a partir do cenário otimizado ################# //
               
                var Resumoresultado = Otm.MontaResultadoFinal(resultado);

                var ResumoresultadoAtual = Otm.MontaResultadoFinal(ResultadoAtual);

                // string formJson;
                // string resumoJson;
                // string professorJson;

                 sw.Stop();

                // ############ Monta Objeto resultado Otimizado ############## //
                Task<string> json = Task.Run(
                    ()=> {
                        
                        return JsonConvert.SerializeObject(resultado);
                }
                );

                  Task<string> formJson = Task.Run(
                    ()=> {
                        
                        return JsonConvert.SerializeObject(_formulario);
                }
                );

                   Task<string> resumoJson = Task.Run(
                    ()=> {
                        
                        return (string)JsonConvert.SerializeObject(Resumoresultado);
                }
                );

                   Task<string> professorJson = Task.Run(
                    ()=> {
                        
                        return JsonConvert.SerializeObject(Cursoprofessor);
                }
                );


                // ############ Monta Objeto resultado Atual ############## //
                Task<string> jsonAt = Task.Run(
                    ()=> {
                        
                        return JsonConvert.SerializeObject(ResultadoAtual);
                }
                );

                  Task<string> formJsonAt = Task.Run(
                    ()=> {
                        
                        return JsonConvert.SerializeObject(_formulario);
                }
                );

                   Task<string> resumoJsonAt = Task.Run(
                    ()=> {
                        
                        return (string)JsonConvert.SerializeObject(ResumoresultadoAtual);
                }
                );

                   Task<string> professorJsonAt = Task.Run(
                    ()=> {
                        
                        return JsonConvert.SerializeObject(cursoProfessorAtual);
                }
                );

                var objRes = new TbResultado();
                objRes.Id = ResId;

                var objResAtual = new TbResultadoAtual();
                objResAtual.Id = ResId;


                Task.WaitAll(json, formJson, resumoJson, professorJson);
                Task.WaitAll(jsonAt, formJsonAt, resumoJsonAt, professorJsonAt);

                 objRes.Resultado = json.Result;
                 objRes.Parametro = formJson.Result;
                 objRes.Resumo = resumoJson.Result;
                 objRes.Professores = professorJson.Result;
                 objRes.TempoExecucao = sw.Elapsed.Duration().ToString(); // sw.Elapsed.Hours.ToString() + ":" + Math.Floor(sw.Elapsed.TotalMinutes) .ToString() + ":" + sw.Elapsed.Seconds.ToString();

                 objResAtual.Resultado = jsonAt.Result;
                 objResAtual.Parametro = formJsonAt.Result;
                 objResAtual.Resumo = resumoJsonAt.Result;
                 objResAtual.Professores = professorJsonAt.Result;

                ProducaoContext.Add(objRes);
                ProducaoContext.Add(objResAtual);

                ProducaoContext.SaveChanges();

                return Ok();

            }
            catch (System.Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no processamento." + e.Message);
            }

        }



        // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }


        #endregion

    }

    public class ProfessorComparer : IEqualityComparer<ProfessorCursoEmec>
    {
        public bool Equals(ProfessorCursoEmec x, ProfessorCursoEmec y)
        {
            if (x.CpfProfessor == y.CpfProfessor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(ProfessorCursoEmec obj)
        {
            return obj.CpfProfessor.GetHashCode();
        }
    }


    public class ProfessorCursoComparer : IEqualityComparer<CursoProfessor>
    {
        public bool Equals(CursoProfessor x, CursoProfessor y)
        {
            if (x.CodEmec == y.CodEmec)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(CursoProfessor obj)
        {
            return obj.CodEmec.GetHashCode();
        }

    }



}