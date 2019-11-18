using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Censo.API.Resultados;
using Censo.API.Model;
//using Censo.API.Controllers.Censo;
using Censo.API.Data.Censo;
using Censo.API.Model.Censo;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Censo.API.Controllers
{
    [AllowAnonymous]
    [Route ("api/[controller]")]
    [ApiController]
    public class ProfessorController: ControllerBase
    {

        public ProfessorContext context;
        public RegimeContext regContext;

        public CensoContext  censocontext;

        public CampusContext CampusContext;

        public ProfessorMatriculaContext MatriculaContext;

        public ProfessorController(ProfessorContext Context,RegimeContext RegContext, CensoContext  Censocontext, 
                                                CampusContext campusContext,
                                                ProfessorMatriculaContext _matContext)
        {
            this.context = Context;
            this.regContext = RegContext;
            this.MatriculaContext = _matContext;
            this.censocontext = Censocontext;
            this.CampusContext = campusContext;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.regContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.censocontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.MatriculaContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            

        }
        
        //Get api/Professor
        [HttpGet]
        public async Task<IActionResult> Get()
        {
                
                Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            
                try
                {

                 Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                    }
                    );

                    Task.WaitAll(task1);
                    
                    
                

                    var results =  await Professores.getProfessores(context).ToListAsync();

                        foreach (var item in results)
                        {
                            if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }

                            else
                            {
                                item.regime = "CHZ/AFASTADO";
                            }
                        }

                     
                    var qtdProfessores = results.Count();
                    var qtdDoutor = results.Where(x => x.Titulacao == "DOUTOR").Count();
                    var qtdMestre = results.Where(x => x.Titulacao == "MESTRE").Count();
                    var qtdEspecialista = results.Where(x => x.Titulacao == "ESPECIALISTA").Count();
                    var qtdNTitulado = results.Where(x => x.Titulacao == "NÃO IDENTIFICADA").Count();
                    var qtdRegime = results.Where(x => x.regime == "TEMPO INTEGRAL" | x.regime == "TEMPO PARCIAL" ).Count();
                    var qtdTempoIntegral = results.Where(x => x.regime == "TEMPO INTEGRAL").Count();
                    var qtdTempoParcial = results.Where(x => x.regime == "TEMPO PARCIAL" ).Count();
                    var qtdHorista = results.Where(x => x.regime == "HORISTA" ).Count();

                    var res = new { qtdDoutor, 
                                    qtdMestre,
                                    qtdRegime,
                                    qtdTempoIntegral,
                                    qtdTempoParcial,
                                    qtdHorista,
                                    qtdProfessores,
                                    qtdNTitulado,
                                    qtdEspecialista};

                  return Ok(res);
                    
                }
                catch (System.Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }


        }

        [HttpGet("Lista")]
        public async Task<IActionResult> ListaProfessores()
        {

                List<ProfessorMatricula> matricula;
                Dictionary<string, DateTime> ListaAdmissao = new Dictionary<string, DateTime>();

                var mat =  MatriculaContext.ProfessorMatricula.ToListAsync();

                
                Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            
                try
                {

                 Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                    }
                    );

                    Task.WaitAll(task1);

                    matricula = await mat;

                    var results =  await Professores.getProfessores(context).ToListAsync();

                        foreach (var item in results)
                        {
                            if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }

                            else
                            {
                                item.regime = "CHZ/AFASTADO";
                            }

                            if (matricula.Where(x => x.cpfProfessor.ToString() == item.CpfProfessor).Count() > 0)
                            {
                                
                                DateTime? _data = matricula.Where(p => p.cpfProfessor.ToString() == item.CpfProfessor).Min(d => d.dtAdmissao);

                                item.dtAdmissao = (_data != null) ? _data.Value.ToString("MM/dd/yyyy") : null;

                            }
                        }

                  return Ok(results);
                    
                }
                catch (System.Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta."  + e.Message);
                }
    

        }

        [HttpGet("Busca/{campo}")]
        public async Task<IActionResult> BuscaProfessores(string campo)
        {
                
                Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            
                try
                {

                 Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                    }
                    );
                
                    Task.WaitAll(task1);
                    var results =  await Professores.getProfessores(context).ToListAsync();

                        foreach (var item in results)
                        {
                            if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }

                            else
                            {
                                item.regime = "CHZ/AFASTADO";
                            }
                        }

                    var results2 = results.Where(x => x.NomProfessor.ToUpper().Contains(campo.ToUpper()) || x.CpfProfessor.ToString().Contains(campo))
                                                .OrderBy(x => x.NomProfessor).ToList();

                  return Ok(results2);
                    
                }
                catch (System.Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }

                      
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            
             try
                {
                    var results =  await Professores.getProfessores(context).Where(x => x.CpfProfessor == id).ToListAsync();
                     var dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

                await Task.Run (
                    () => 
                    {
                        foreach (var item in results)
                        {
                            if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }

                              else
                            {
                                item.regime = "CHZ/AFASTADO";
                            }
                        }
                        

                    });
                    

                    return Ok(results);
                    
                }
                catch (System.Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }

        }

        // nova pesquisa detalhe professor

        [HttpGet("BuscaDetalhe/{id}")]
        public async Task<IActionResult> BuscaDetalhe(string id)
        {
            
                try
                {

                     var diccampus = this.CampusContext.TbSiaCampus.ToDictionary(x => x.CodCampus);
                     var dic = this.censocontext.ProfessorCursoCenso
                                                        .Where(x => x.CpfProfessor.ToString() == id )
                                                        .ToList();
                      var dic1 = this.censocontext.CursoCenso.Select(x => new CursoDetalhe {CodCurso = x.CodCurso, NomCurso = x.NomCursoCenso})
                                        .ToList();

                      var dicCurso = dic1.Distinct<CursoDetalhe>(new CursoComparer()).ToDictionary(x => x.CodCurso);

                      //List<ProfessorMatricula> matricula;
                      //var mat = this.MatriculaContext.ProfessorMatricula.ToDictionary(x => x.cpfProfessor.ToString());

                      // pegar nome do professor -- titulacao -- regime
                      var professor = Professores.getProfessores(context).Where(x => x.CpfProfessor == id).First();
                      var regime = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                      ProfessorDetalhe professordetalhe = new ProfessorDetalhe();

                        //cpf/nomeprofessor//titulacao//regime
                        professordetalhe.CpfProfessor = professor.CpfProfessor.ToString();
                        professordetalhe.NomProfessor = professor.NomProfessor;
                        professordetalhe.titulacao = professor.Titulacao;
                        professordetalhe.regime = regime[professordetalhe.CpfProfessor.ToString()].Regime;
                        
                        // RECEBENDO A REGIAO
                        //professordetalhe.nomeRegiao = mat[professordetalhe.CpfProfessor.ToString()].nomeRegiao;
                        //mat.Where(p => p.cpfProfessor.ToString() == item.CpfProfessor));


                        //professordetalhe.CargaTotal = double.Parse(regime[professordetalhe.CpfProfessor.ToString()].CargaTotal).ToString();
                        professordetalhe.CargaTotal = (double)Math.Round((decimal)((regime[professordetalhe.CpfProfessor.ToString()].CargaTotal == null) ? 0.0 : regime[professordetalhe.CpfProfessor.ToString()].CargaTotal) ,2);
                        professordetalhe.QtdHorasDs = (double)Math.Round((decimal)((regime[professordetalhe.CpfProfessor.ToString()].QtdHorasDs == null) ? 0.00 : regime[professordetalhe.CpfProfessor.ToString()].QtdHorasDs) ,2);
                        professordetalhe.QtdHorasFs = (double)Math.Round((decimal)((regime[professordetalhe.CpfProfessor.ToString()].QtdHorasFs == null) ? 0.00 : regime[professordetalhe.CpfProfessor.ToString()].QtdHorasFs) ,2);

                        foreach (var item in dic)
                        {

                           //if (listaprofessordetalhe.Find(x => x.CpfProfessor == item.CpfProfessor.ToString()) == null)
                           if (dic.Count > 0)
                           {
                                                                                
                                professordetalhe.Cursos.Add( new Curso{codcurso = item.CodCurso, 
                                                                        nomcampus = diccampus.TryGetValue(item.CodCampus, out var camp) ? camp.NomCampus : "NÃO ENCONTRADO",
                                                                        nomcurso = dicCurso.TryGetValue(item.CodCurso, out var curso) ? curso.NomCurso : "NÃO ENCONTRADO"
                                                                        });
                                
                               
                           }     
                          

                        }

                        //return Ok(results2);
                        return Ok(professordetalhe);
                    
                }
                catch (System.Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }
                // Termino da pesquisa detalhe professor
                      
        }



    }



     public class CursoComparer : IEqualityComparer<CursoDetalhe>
    {
        public bool Equals(CursoDetalhe x, CursoDetalhe y)
        {
            if (x.CodCurso == y.CodCurso)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(CursoDetalhe obj)
        {
            return obj.CodCurso.GetHashCode();
        }
    }


   

}