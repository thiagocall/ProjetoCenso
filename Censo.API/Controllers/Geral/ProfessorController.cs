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
    public class ProfessorController: ControllerBase
    {

        public ProfessorContext context;
        public RegimeContext regContext;

        public ProfessorController(ProfessorContext Context,RegimeContext RegContext)
        {
            this.context = Context;
            this.regContext = RegContext;

        }
        
        //Get api/Professores
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
                try
                {
                    var results =  await Professores.getProfessores(context).ToListAsync();


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