using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers.Censo
{
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class CursoEmecController : ControllerBase
    {

        public ProfessorCursoEmecContext Context { get; }
        public ProfessorIESContext ProfContext { get; }

        public CursoEmecController(ProfessorCursoEmecContext _context, ProfessorIESContext _profcontext)
        {
            this.Context = _context;
            this.ProfContext = _profcontext;
        }
        

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await Context.ProfessorCursoEmec.ToListAsync();

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();
        
            // ########## Monta a lista de cursos por professores ##########
            cursoProfessor = MontaCursoProfessor(query);


            var results = cursoProfessor.ToList();


            return Ok(results);

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




        private List<CursoProfessor> MontaCursoProfessor(List<ProfessorCursoEmec> query)
        {

            List<CursoProfessor> cursoProfessor = new List<CursoProfessor>();

            foreach (var res in query)
            {
                // Filtra parâmtetro indGraduacao
                if (res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado) //res.Titulacao != "GRADUADO" || ParametrosFiltro.indGraduado
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




        private double MontaPrevisao(int alvo, List<double> x, List<double> y){

            // calcula previsão simples para o ano atual
            //a = avg(y) - (b * avg(x))
            //b = sum((x - avg(x))* (y - avg(y))) / sum((x - avg(y)^2))
            //alvo = a + b * x
            double? a;
            double? b;
            double? x_avg = x.Average();
            double? y_avg = y.Average();
            double?[] x_dev = new double?[x.Count - 1];
            double?[] y_dev = new double?[y.Count - 1];
            double? b1 = 0;
            double? b2 = 0;
            double res;
            
            foreach (var item in x)
            {
                x_dev.Append(item - x_avg);

            }
              foreach (var item in y)
            {
                y_dev.Append(item - y_avg);
            }

            for (int i = 0; i < x.Count(); i++)
            {   
                b1 += x_dev[i] * y_dev[i];
                b2 += Math.Pow((double)x_dev[i],2);
            }

            b = b1 / b2;
            a = y_avg - (b * x_avg );

            res = (double)(a + b * alvo);

            return res;

        }


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
