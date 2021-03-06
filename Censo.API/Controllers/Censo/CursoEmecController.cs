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
using System.Data.SqlClient;
using Censo.API.Services.Redis.Services;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using st = ServiceStack;
using System.Reflection;

namespace Censo.API.Controllers.Censo
{
    [Route("api/v1/Censo/[controller]")]
    [ApiController]
    public class CursoEmecController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public ProfessorIESContext ProfContext { get; }
        public CensoContext Context {get; }

        public TempProducaoContext ProducaoContext { get; set; }

        public CursoEnquadramentoContext CursoEnquadramentoContext;

        public IOtimizacao Otm { get; }

        public Dictionary<long?, PrevisaoSKU> ListaPrevisaoSKU;

        public ParametrosCenso Formulario { get; set; }

        // Teste Redis

        public RedisService Redis { get; set; }



        public CursoEmecController( CensoContext _context,
                                    ProfessorIESContext _profcontext,
                                    IConfiguration _configuration,
                                    IOtimizacao _otm, 
                                    CursoEnquadramentoContext _cursoEnquadContext, 
                                    TempProducaoContext _producaoContext,
                                    RedisService _redisService)
        {
            this.Redis = _redisService;
            this.Context = _context;
            this.ProfContext = _profcontext;
            this.Configuration = _configuration;
            this.CursoEnquadramentoContext = _cursoEnquadContext;
            this.Otm = _otm;
            this.ProducaoContext = _producaoContext;
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ProfContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.CursoEnquadramentoContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ProducaoContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet("geraPrevisao/{id}/{tipo}")]
        public async Task<IActionResult> Get([FromQuery]long id, string tipo)
        {
            var query = await Context.ProfessorCursoEmec.ToListAsync();


            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();
            var ListaCursoArea = await this.CursoEnquadramentoContext.CursoEnquadramento.ToListAsync();


            // ########## Monta a lista de cursos por professores ##########
            cursoProfessor = MontaCursoProfessor(query, ListaCursoArea);

            var results = cursoProfessor.ToList();

            return Ok(results);

        }
        
        /// ############## Exemplo Implementação Redis ############### ///

        // [AllowAnonymous]
        // [HttpGet("setRedis")]
        // public async Task<IActionResult> setRedis(){

        //     Teste2 dado;

        //     using(RedisClient redis = new RedisClient("10.200.0.9", 6379)) // var redis = this.Redis.GetRedisClient())
        //             {
                        
                         
        //                  IRedisTypedClient<Teste2> dads = redis.As<Teste2>() as IRedisTypedClient<Teste2>;
        //                 Teste2 teste2 = new Teste2();

        //                 teste2.codEmec = 1;
        //                 teste2.Lista = new List<Teste>(){
        //                     new Teste(){Nome = "Thiago", Idade = 35},
        //                     new Teste(){Nome = "Lucas", Idade = 39}
        //                 };

        //                 // var dados = redis.GetTypedClient<Teste2>();

        //                 st.ModelConfig<Teste2>.Id(x => x.codEmec);
                        
        //                 dads.Store(teste2);

        //                 dado = (Teste2)dads.GetById(1);
                        

        //             }

        //     return Ok(dado);

        // }


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

        
        [AllowAnonymous]
        [HttpGet("ObterResultados")]
        public async Task<IActionResult> obterResultados()
        {
            var query = await this.ProducaoContext.TbResultado
                             .Select(x => new {x.Id, x.Resumo, x.TempoExecucao, x.indOficial, x.Observacao})
                             .OrderByDescending(x => x.Id)
                             .ToArrayAsync();
    
            return Ok(query);
        }
        
        [AllowAnonymous]
        [HttpPost("MarcaResultadoOficial")]
        public ActionResult MarcaResultadoOficial([FromBody] dynamic obj)
        {
            try 
            {
                    if (obj.valor == true)
                    {
                        
                        var commandAntigo = "UPDATE TbResultado set ind_oficial = 0 where ind_oficial = 1";
                        this.ProducaoContext.Database.ExecuteSqlCommand(commandAntigo);

                        var commandNovo = "UPDATE TbResultado set ind_oficial = 1 where num_ordem = @num_ordem";
                        var nameAntigo = new SqlParameter( "@num_ordem", (long)obj.id);
                        this.ProducaoContext.Database.ExecuteSqlCommand(commandNovo, nameAntigo);

                        
                    }
                    else
                    { 
                        var commandNovo = "UPDATE TbResultado set ind_oficial = 0 where num_ordem = @num_ordem";
                        var nameNovo = new SqlParameter( "@num_ordem", (long)obj.id);
                        this.ProducaoContext.Database.ExecuteSqlCommand(commandNovo, nameNovo);
                        
                    }

                     return Ok();
                    

            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no processamento." + ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet("ObterResultados/{_id}")]
        public ActionResult obterResultadosporId(long _id)
        {
            var resultado = this.ProducaoContext.TbResultado.Select(x => new {x.Id, x.Resumo, x.indOficial}).FirstOrDefault(x => x.Id == _id);
            var resultadoAtual = this.ProducaoContext.TbResultadoAtual.Select(x => new {x.Id, x.Resumo}).FirstOrDefault(x => x.Id == _id);

            var resultadoCompleto = new {resultado, resultadoAtual};
            return Ok(resultadoCompleto);
        }

        [HttpGet("ObterResultadosCompleto")]
        public async Task<ActionResult> obterResultadosCompleto()
        {
            var query = await this.ProducaoContext.TbResultado.ToListAsync();

            return Ok(query);
        }

        [HttpDelete("{id}")]
        // DELETE api/values/5
        public async Task<IActionResult> Delete(long id)
        {
            // var item = this.ProducaoContext.TbResultado.Find(id);
            // var item2 = this.ProducaoContext.TbResultadoAtual.Find(id);
            // this.ProducaoContext.RemoveRange(item, item2); 
            // await this.ProducaoContext.SaveChangesAsync();


            var paramId = new SqlParameter( "@num_ordem", (long)id);
            
            Task task1 = Task.Run(
                () => {
                var deletar = "DELETE FROM TbResultado WHERE num_ordem = @num_ordem";
                this.ProducaoContext.Database.ExecuteSqlCommand(deletar, paramId);
                var deletarAtual = "DELETE FROM TbResultado_Atual WHERE num_ordem = @num_ordem";
                this.ProducaoContext.Database.ExecuteSqlCommand(deletarAtual, paramId);
                }
            );

            Task.WaitAll(task1);

            return Ok();
        }

        [AllowAnonymous]
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
        

            if (emec != null)
            {
                
                var area1 = emec.CodArea;
                //##### Previsão Doutor

                var prev_minM = previsao.P_Min_Mestre;
                var prev_maxM = previsao.P_Max_Mestre;

                var prev_minD = previsao.P_Min_Doutor ;
                var prev_maxD = previsao.P_Max_Doutor;

                var prev_minR = previsao.P_Min_Regime ;
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

                var resultadoOTMAnterior = await this.ProducaoContext.TbResultadoAtual
                        .FirstOrDefaultAsync(r => r.Id == id);
                
                // Tratando dados para Excel

                var parametros =  new List<ParametrosCenso>();
               

                var ListaCurso = await this.Context.CursoCenso.ToListAsync();

                parametros.Add(JsonConvert.DeserializeObject<ParametrosCenso>(resultadoOTM.Parametro));

                var resultados = JsonConvert.DeserializeObject<List<Resultado>>(resultadoOTM.Resultado);

                var resultadosAnt = JsonConvert.DeserializeObject<List<Resultado>>(resultadoOTMAnterior.Resultado);

                var professores = JsonConvert.DeserializeObject<List<CursoProfessor>>(resultadoOTM.Professores);

                var professoresAnterior = JsonConvert.DeserializeObject<List<CursoProfessor>>(resultadoOTMAnterior.Professores);

                var cursoCenso = ListaCurso;

                Dictionary<long?, string> CursoEmec = new Dictionary<long?, string>();

                cursoCenso.ForEach( p =>
                {
                    if (!CursoEmec.TryGetValue(p.CodEmec, out string cr)) {
                        
                        CursoEmec.Add(p.CodEmec, p.NomCursoCenso);
                    }
                      
                });

                List<ProfessorExcel> listaProfessor = new List<ProfessorExcel>();
                List<ProfessorExcel> listaProfessorAnt = new List<ProfessorExcel>();

                foreach (var item in professores)
                {
                    item.Professores.ForEach( (p) =>
                            {
                               listaProfessor.Add( new ProfessorExcel { 
                                   
                                   cpfProfessor = p.cpfProfessor,
                                   Regime = p.Regime,
                                   Titulacao = p.Titulacao,
                                   NomeCurso = CursoEmec[item.CodEmec],
                                   CodEmec = item.CodEmec,
                                   Nota_Doutor = item.Nota_Doutor,
                                   Nota_Mestre = item.Nota_Mestre,
                                   Nota_Regime = item.Nota_Regime,

                               })  ;
                            });
                }

                foreach (var item in professoresAnterior)
                {
                    item.Professores.ForEach( (p) =>
                            {
                               listaProfessorAnt.Add( new ProfessorExcel { 
                                   
                                   cpfProfessor = p.cpfProfessor,
                                   Regime = p.Regime,
                                   Titulacao = p.Titulacao,
                                   NomeCurso = CursoEmec[item.CodEmec],
                                   CodEmec = item.CodEmec,
                                   Nota_Doutor = item.Nota_Doutor,
                                   Nota_Mestre = item.Nota_Mestre,
                                   Nota_Regime = item.Nota_Regime,

                               })  ;
                            });
                }

                //  Monta arquivo para Download em Excel

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream)) {                
                    var shResumo = package.Workbook.Worksheets.Add("Resultado");
                    var shResumoaAnt = package.Workbook.Worksheets.Add("ResultadoNaoOtimizado");
                    var shParam = package.Workbook.Worksheets.Add("Parametros");
                    var shProfessores = package.Workbook.Worksheets.Add("Professores");
                    //var shProfessoresAnt = package.Workbook.Worksheets.Add("ProfessoresAnt");
                    shResumo.Cells.LoadFromCollection(resultados, true);
                    shResumoaAnt.Cells.LoadFromCollection(resultadosAnt, true);
                    
                    shParam.Cells.LoadFromCollection(parametros, true);
                    shProfessores.Cells.LoadFromCollection(listaProfessor, true);
                    //shProfessoresAnt.Cells.LoadFromCollection(listaProfessorAnt, true);
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


        private List<CursoProfessor> MontaCursoProfessor([FromQuery] List<ProfessorCursoEmec> _profs, List<CursoEnquadramento> _CursoArea)
        {

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();
        

            var query = _profs;

            var CursoArea = _CursoArea.ToDictionary(x => x.CodEmec);

            try
            {

                foreach (var res in query)
                {
                    // Filtra parâmtetro indGraduacao
                    if (!(res.Titulacao == null || res.Titulacao == "GRADUADO" || (res.Regime == null && res.Titulacao == "NÃO IDENTIFICADA"))) //res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado
                    {
                        

                        if (cursoProfessor.Where(c => c.CodEmec == res.CodEmec).Count() > 0
                    )
                        {
                            CursoProfessor prof = cursoProfessor.Find(x => x.CodEmec == res.CodEmec);

                            prof.CodArea = (CursoArea.ContainsKey(Convert.ToInt32((prof.CodEmec)))) ? CursoArea[(Int32)prof.CodEmec].CodArea : 9999;

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
                            prof.CodArea = (CursoArea.ContainsKey(Convert.ToInt32((res.CodEmec)))) ? CursoArea[(Int32)res.CodEmec].CodArea : 9999;
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
            catch (System.Exception ex)
            {
                
                return null;
            }

        }

        private List<CursoProfessor> MontaCursoProfessor20P([FromQuery] List<ProfessorCursoEmec> _profs, List<CursoEnquadramento> _CursoArea)
        {

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();

            var query = _profs;

            var CursoArea = _CursoArea.ToDictionary(x => x.CodEmec);

            foreach (var res in query)
            {
                // Filtra parâmtetro indGraduacao
                if (!(res.Titulacao == null || res.Titulacao == "GRADUADO")) //res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado
                {

                    if (cursoProfessor.Where(c => c.CodEmec == res.CodEmec).Count() > 0)
                    {
                        CursoProfessor prof = cursoProfessor.Find(x => x.CodEmec == res.CodEmec);
                        prof.CodArea = (CursoArea.ContainsKey(Convert.ToInt32((prof.CodEmec)))) ? CursoArea[(Int32)prof.CodEmec].CodEmec : 9999;

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

        [AllowAnonymous]
        [HttpPost("ComparaResultado")]
        public async Task<IActionResult> getComparaResultado( string[] _listaResultado) {

            try 
            {

                    var res = await this.ProducaoContext.TbResultado
                                        .Where(r => _listaResultado.Contains(r.Id.ToString()))
                                        .OrderByDescending(r => r.Id)
                                        .Select(r => new {r.Id, r.Resumo, r.indOficial})
                                        .ToArrayAsync();

                    return Ok(res) ;

            }
            catch (Exception e) {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no processamento." + e.Message);
            }

        }

        //################## Previsão ################################

        private double?[] GeraPrevisao(long? _id, string _tipo, List<CursoPrevisao> _query)
        {


            double?[] prev = new double?[2];

            var anoAtual = _query.Max(x => x.Ano) + 3;

            int metodo =  this.Formulario == null ? -1 : this.Formulario.Metodo;

            switch (_tipo.ToUpper())
            {
                case "M":
                    prev[0] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Min_Mestre).ToList(),metodo);
                    prev[1] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Mestre).ToList(),metodo);
                    break;

                case "D":
                    prev[0] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Min_Doutor).ToList(),metodo);
                    prev[1] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Doutor).ToList(),metodo);
                    break;

                case "R":
                    prev[0] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Min_Regime).ToList(),metodo);
                    prev[1] = MontaPrevisao(anoAtual, _query.Select(c => (double?)c.Ano).ToList(), _query.Select(c => c.Max_Regime).ToList(),metodo);
                    break;

           
                default:
                    break;
            }

           
            return prev;

        }

        private double? MontaPrevisao(long? alvo, List<double?> x, List<double?> y, int ind_metodo = -1)
        {
           

        double? res = 0;

        if (ind_metodo == -1 && (x.Count() > 1 && y.Count() > 1)) 
        {

            double? a = 0;
            double? b = 0;
            double? x_avg = x.Average();
            double? y_avg = y.Average();
            List<double?> x_dev = new List<double?>();
            List<double?> y_dev = new List<double?>();
            double? b1 = 0;
            double? b2 = 0;

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


            return res;

        // ####### adiciona 10% do último ano no resultado da previsão para método de tendência
        
        } else if(ind_metodo == 0)
        {
            
            res = y.Where(v => v != null).Last() * 1.10;

            return res;

        }else{
            
            res = y.Where(v => v != null).Last() * 1.10;

            return res;

        }


        }

        private Dictionary<long?, PrevisaoSKU> GeraListaPrevisaoSKU()
        {

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
                prev.P_Min_Mestre = resM[0] == 1 ? 0.95 : resM[0];
                prev.P_Max_Mestre = resM[1];
                prev.P_Min_Doutor = resD[0] == 1 ? 0.95 : resD[0];
                prev.P_Max_Doutor = resD[1];
                prev.P_Min_Regime = resR[0] == 1 ? 0.95 : resR[0];
                prev.P_Max_Regime = resR[1];

                ListaPrevisaoSKU.Add(prev.CodArea, prev);

            };

            return ListaPrevisaoSKU;

        }

        //#################### Gera notas para cursos #####################
        private dynamic getNotaCursos(List<ProfessorCursoEmec> _query, List<CursoEnquadramento> _cursoProfessor)
        {


        try {

            var query = _query;

            var ListaPrevisaoSKU = GeraListaPrevisaoSKU();


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
               
                var ii = cctx.FirstOrDefault(x => x.CodEmec == item.CodEmec);

                if (ii != null)
                {
                    percent.Add(perc_D);
                    var area = ii.CodArea;
                    //##### Previsão Doutor


                    if (ListaPrevisaoSKU.ContainsKey(area))
                    {
                        var prev = ListaPrevisaoSKU[area];

                        var prev_minM = prev.P_Min_Mestre;
                        var prev_maxM = prev.P_Max_Mestre;

                        var prev_minD = prev.P_Min_Doutor;
                        var prev_maxD = prev.P_Max_Doutor;

                        var prev_minR = prev.P_Min_Regime;
                        var prev_maxR = prev.P_Max_Regime;

                        var M = Otm.N_Escala(prev_minM, prev_maxM, perc_M);
                        var D = Otm.N_Escala(prev_minD, prev_maxD, perc_D);
                        var R = Otm.N_Escala(prev_minR, prev_maxR, perc_R);

                        double notaM = (M == null) ? 0 : Convert.ToDouble(M);
                        double notaD = (D == null) ? 0 : Convert.ToDouble(D);
                        double notaR = (R == null) ? 0 : Convert.ToDouble(R);

                        item.Nota_Mestre = notaM;
                        item.Nota_Doutor = notaD;
                        item.Nota_Regime = notaR;

                    }

                }

            }
            
            
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
                                    
                                })
                                    .ToList();

            return result;

        }
        catch (Exception ex){
            return null;
        }

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

               
               //N_escala da Curso Emec

            }
            catch (System.Exception)
            {
                if(lim_max == lim_min){
                    return 5;
                } else {
                    return 0;
                }
            }

        }



        #region httpVerbs
        // POST api/values
        [HttpPost("Otimizar")]
        public async Task<IActionResult> Otimizar([FromBody] ParametrosCenso _formulario)
        {

            this.Formulario = _formulario;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var TaskEnade = await Task.Run( () => {

                return this.Context.CursoCenso.ToList();

            });

            var ResId = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            
            try
            {

                var query = await this.Context.ProfessorCursoEmec.ToListAsync();
                var query20p = await this.Context.ProfessorCursoEmec20p.ToListAsync();
                var ListaCursoArea = await this.CursoEnquadramentoContext.CursoEnquadramento.ToListAsync();
                var ListaPrevisaoSKU = GeraListaPrevisaoSKU();
               
                var Cursoprofessor = MontaCursoProfessor(query, ListaCursoArea);

                // // Obtem lista dos professores escolhidos no filtro
                var lista = _formulario.MontaLista();

                List<CursoProfessor> cursoProfessorAtual = new List<CursoProfessor>();
                Cursoprofessor.ForEach( (item) => {
                        cursoProfessorAtual.Add((CursoProfessor)item.Clone());
                        }
                    );

                var CursoNota = getNotaCursos(query, ListaCursoArea);

                var CursoEnade = TaskEnade.Where(x => x.IndEnade.Contains('S')).Select(c => c.CodEmec.ToString()).Distinct().ToList();

                List<Resultado> ResultadoAtual = Otm.CalculaNotaCursos(ListaPrevisaoSKU, cursoProfessorAtual, CursoEnade);

                   // ######################## Alavanca 20% ######################## //
                
                if (_formulario.otimiza20p)
                {
                    Otm.AddProfessor20p(Cursoprofessor, query20p, ListaPrevisaoSKU, _formulario, CursoEnade);
                }

                List<Resultado> resultado = Otm.OtimizaCurso(ListaPrevisaoSKU, query,  Cursoprofessor, ListaCursoArea, _formulario);
                
                // ############## Monta resultados a partir do cenário otimizado ################# //
               
                var Resumoresultado = Otm.MontaResultadoFinal(resultado);

                var ResumoresultadoAtual = Otm.MontaResultadoFinal(ResultadoAtual);


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
                 objRes.TempoExecucao = DateTime.Now.ToString("HH:mm:ss");
                 objRes.Observacao = _formulario.Observacao;

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
   
        #endregion
    // Censo - Resultados - Calculadora
    [HttpPost("GetDadosCalculadora")]
    public async Task<IActionResult> GetDadosCalculadora([FromBody] dados _dados) 
    {
   
        try
        {
            var Strprof = await ProducaoContext.TbResultado.FindAsync(_dados._idResultado);
            var professores = JsonConvert.DeserializeObject<IEnumerable<CursoProfessor>>(Strprof.Professores).ToList();
            var dados = professores.Find(x => x.CodEmec == _dados._idEmec);
            var area = professores.Find(x => x.CodEmec == _dados._idEmec).CodArea;
            var QtdDR = dados.Professores.Where(x => x.Titulacao == "DOUTOR" && (x.Regime == "TEMPO INTEGRAL" || x.Regime == "TEMPO PARCIAL")).Count();
            var QtdDH = dados.Professores.Where(x => x.Titulacao == "DOUTOR" && !(x.Regime == "TEMPO INTEGRAL" || x.Regime == "TEMPO PARCIAL")).Count();
            var QtdMR = dados.Professores.Where(x => x.Titulacao == "MESTRE" && (x.Regime == "TEMPO INTEGRAL" || x.Regime == "TEMPO PARCIAL")).Count();
            var QtdMH = dados.Professores.Where(x => x.Titulacao == "MESTRE" && !(x.Regime == "TEMPO INTEGRAL" || x.Regime == "TEMPO PARCIAL")).Count();
            var QtdER = dados.Professores.Where(x => x.Titulacao == "ESPECIALISTA" && (x.Regime == "TEMPO INTEGRAL" || x.Regime == "TEMPO PARCIAL")).Count();
            var QtdEH = dados.Professores.Where(x => x.Titulacao == "ESPECIALISTA" && !(x.Regime == "TEMPO INTEGRAL" || x.Regime == "TEMPO PARCIAL")).Count();
            var Qtd = dados.Professores.Count();
            var Nota_Doutor = dados.Nota_Doutor;
            var Nota_Mestre = dados.Nota_Mestre;
            var Nota_Regime = dados.Nota_Regime;

            var ListaPrevisaoSKU = GeraListaPrevisaoSKU().Where(x => x.Value.CodArea == area).First().Value;

             return Ok(new {QtdDR, QtdDH, QtdMR, QtdMH, QtdER, QtdEH, Qtd, 
                            Nota_Doutor, Nota_Mestre, Nota_Regime, ListaPrevisaoSKU} );
        }
        catch (System.Exception ex)
        {
            
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
                
            // return Ok(dados);


    }


    public class dados
    {
    
        public int _idEmec { get; set; }
        public long _idResultado { get; set; }

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


    public class Teste
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
    }

      public class Teste2
    {
        public int codEmec { get; set; }
        public List<Teste> Lista {get; set;}
    }

}