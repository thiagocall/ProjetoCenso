using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Censo.API.Controllers.Censo
{
    [Route("api/v1/Censo/[controller]")]
    [ApiController]
    public class CursoEmecController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        // public ProfessorCursoEmecContext Context { get; }
        public ProfessorIESContext ProfContext { get; }
        public CensoContext Context { get; }

        public static Dictionary<long?, PrevisaoSKU> ListaPrevisaoSKU;

        // public CursoCensoContext CursoCensoContext { get; set; }
        
        public CursoEmecController(CensoContext _context, ProfessorIESContext _profcontext, IConfiguration _configuration)
        {
            this.Context = _context;
            this.ProfContext = _profcontext;
            this.Configuration = _configuration;
        }    

        [HttpGet("geraPrevisao/{id}/{tipo}")]
        public async Task<IActionResult> Get(long id, string tipo)
        {
            var query = await Context.ProfessorCursoEmec.ToListAsync();

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();
        
            // ########## Monta a lista de cursos por professores ##########
            cursoProfessor = MontaCursoProfessor(query);

            var results = cursoProfessor.ToList();

            return Ok(results);

           //var results = GeraPrevisao(id);

        }


        [HttpGet("GeraNota/{id}")]
        public ActionResult GeraNota(long? id)
        {

            List<CursoPrevisao> listaPrev = PrevisaoEmec.getPrevisao(this.Configuration);

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

            List<string> tipos = new List<string>(){"D","R","M"};

            if(tipos.Contains(_tipo.ToUpper())){

                    prev[0] = (prev[0] < 0) ? 0 : prev[0];
                    prev[0] = (prev[0] > 5) ? 5 : prev[0];

                    prev[1] = (prev[1] < 0) ? 0 : prev[1];
                    prev[1] = (prev[1] > 5) ? 5 : prev[1];

            }

            //var t = MontaPrevisao(2019, query.Select(c => c.Ano).ToList(), query.Select(c => c.Max_Mestre).ToList());

            return Ok(prev);

        }


        [HttpGet("Notas")]
        public ActionResult Notas(){

            return Ok(getNotaCursos());
            //GeraListaPrevisaoSKU();
            //var results =  this.ListaPrevisaoSKU.Select(a => new {
            //         a.Value.CodArea,
            //         a.Value.P_Min_Mestre,
            //         a.Value.P_Max_Mestre,
            //         a.Value.P_Min_Doutor,
            //         a.Value.P_Max_Doutor,
            //         a.Value.P_Min_Regime,
            //         a.Value.P_Max_Regime
            // }).ToList();
            // var results = this.ListaPrevisaoSKU[5];
            // return Ok(results);

        }

        // ################# Monta Cursos dos Professores ######################
        private List<CursoProfessor> MontaCursoProfessor(List<ProfessorCursoEmec> query)
        {

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();

            foreach (var res in query)
            {
                // Filtra parâmtetro indGraduacao
                if ((res.Titulacao != "GRADUADO" | ParametrosFiltro.indGraduado) & res.Titulacao != null ) //res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado
                {
                    
                    if (cursoProfessor.Where(c => c.CodEmec == res.CodEmec).Count() > 0)
                    {
                        CursoProfessor prof = cursoProfessor.Find(x => x.CodEmec == res.CodEmec);

                        if (!prof.Professores.ContainsKey(res.CpfProfessor))
                        {
                            ProfessorEmec pr = new ProfessorEmec{
                                cpfProfessor = res.CpfProfessor,
                                Ativo = res.IndAtivo,
                                Regime = res.Regime,
                                Titulacao = res.Titulacao
                            };
                            //prof.Professores = new Dictionary<long, ProfessorEmec>();
                            prof.Professores.Add(pr.cpfProfessor, pr);
                        }

                    }
                    else
                    {
                        CursoProfessor prof = new CursoProfessor();
                        prof.CodEmec = res.CodEmec;
                        prof.Professores = new  Dictionary<long, ProfessorEmec>();
                        ProfessorEmec pr = new ProfessorEmec{
                                cpfProfessor = res.CpfProfessor,
                                Ativo = res.IndAtivo,
                                Regime = res.Regime,
                                Titulacao = res.Titulacao
                            };
                        prof.Professores.Add(pr.cpfProfessor, pr);
                        cursoProfessor.Add(prof);

                    }
                }
            }

            return cursoProfessor;

        }

        //################## Previsão ################################


        private double?[] GeraPrevisao(long? _id, string _tipo, List<CursoPrevisao> _query){


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
                     b2 += Math.Pow((double)x_dev[i],2);
                 }

                 b = b1 / b2;
                 a = y_avg - (b * x_avg );

                res = (a + b * alvo);

                //res = (res > 1) ? 1 : res;
                //res = (res < 0) ? 0 : res;
                
                return res;

        }
        
        private void GeraListaPrevisaoSKU() {

            // if (ListaPrevisaoSKU != null)
            // {
            //     return;
            // }

            List<CursoPrevisao> listaPrev = PrevisaoEmec.getPrevisao(this.Configuration);

            ListaPrevisaoSKU = new Dictionary<long?, PrevisaoSKU>();

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

        }

        //#################### Gera notas para cursos #####################
        private object getNotaCursos() 
        {

            var query = this.Context.ProfessorCursoEmec.ToList();

            GeraListaPrevisaoSKU();
            
            //List<CursoPrevisao> listaPrev = PrevisaoEmec.getPrevisao(this.Configuration);

            List<CursoProfessor> cursoProfessor;
        
            // ########## Monta a lista de cursos por professores ##########
            cursoProfessor = MontaCursoProfessor(query);
            var cctx = this.Context.CursoCenso.ToList();
            List<double> percent = new List<double>();
            //conte

            // ######## Calcula Nota Prévia dos Cursos ###########

            foreach (var item in cursoProfessor)
            {   
                double qtdProf = item.Professores
                        .Count();
                double qtdD = item.Professores.Where(x => x.Value.Titulacao == "DOUTOR")
                        .Count();
                double qtdM = item.Professores.Where(x => x.Value.Titulacao == "MESTRE" | x.Value.Titulacao == "DOUTOR")
                        .Count();
                double qtdR = item.Professores.Where(x => x.Value.Regime == "TEMPO INTEGRAL" | x.Value.Regime == "TEMPO PARCIAL")
                        .Count();

                double perc_D = qtdD / qtdProf;
                double perc_M = qtdM / qtdProf;
                double perc_R = qtdR / qtdProf;
                ////e.CodCampus, e.CodCurso, e.NumHabilitacao
                var ii = cctx.FirstOrDefault(x => x.CodEmec == item.CodEmec);

                if(ii != null)
                {
                    percent.Add(perc_D);
                    var area = ii.CodArea;
                     //##### Previsão Doutor

                     //double?[] prev = new double?[2];

                     //var query2 = listaPrev.Where(x => x.CodArea == area).OrderBy(x => x.Ano).ToList();
                    
                    if (ListaPrevisaoSKU.ContainsKey(area))
                    {
                     var prev = ListaPrevisaoSKU[area];
                     var prev_min = prev.P_Min_Mestre;
                     var prev_max = prev.P_Max_Mestre;
                     double notaM =  (N_Escala(prev_min, prev_max, perc_M)) == null ? 0 : Convert.ToDouble(N_Escala(prev_min, prev_max, perc_M));
                     double notaD =  (N_Escala(prev_min, prev_max, perc_D)) == null ? 0 : Convert.ToDouble(N_Escala(prev_min, prev_max, perc_D));
                     double notaR =  (N_Escala(prev_min, prev_max, perc_R)) == null ? 0 : Convert.ToDouble(N_Escala(prev_min, prev_max, perc_R));
                     item.Nota_Mestre = notaM;
                     item.Nota_Doutor = notaD;
                     item.Nota_Regime = notaR;
                        
                    }

                }
               
            }

                //var result = cursoProfessor.Select(x => x.Nota_Mestre).ToList();
                var result = cursoProfessor.Select( x => new{ x.CodEmec, 
                                                              x.Nota_Mestre,
                                                              x.Nota_Doutor,
                                                              x.Nota_Regime,
                                                              Mestres = x.Professores
                                                                        .Where(p => p.Value.Titulacao == "MESTRE" || p.Value.Titulacao == "DOUTOR" )
                                                                        .Count(),
                                                              Professores = x.Professores.Count(),
                                                              doutores = x.Professores
                                                                        .Where(p => p.Value.Titulacao == "DOUTOR").Count()})
                                                            .ToList();

            return result;

        }

        private double? N_Escala(double? lim_min, double? lim_max, double? percent){

            double? n;

            try
            {

                n = (percent - lim_min) /  (lim_max - lim_min)   * 5;
                if (n < 0)
                {
                    return 0;
                }
                else if(n > 5)
                {
                    return 5;

                }

                else
                {
                    return n;

                }

            }
            catch (System.Exception)
            {
                
                return 0;
            }

        }


         #region httpVerbs
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

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
