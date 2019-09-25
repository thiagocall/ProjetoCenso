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

namespace Censo.API.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ProfessorController: ControllerBase
    {

        public ProfessorContext context;
        public RegimeContext regContext;

        public ProfessorMatriculaContext MatriculaContext;

        public ProfessorController(ProfessorContext Context,RegimeContext RegContext, ProfessorMatriculaContext _matContext)
        {
            this.context = Context;
            this.regContext = RegContext;
            this.MatriculaContext = _matContext;

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




    }
}