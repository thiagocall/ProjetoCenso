
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using System.Net.Http;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Censo.API.Model.dados;


namespace Censo.API.Controllers.Enade
{
    [AllowAnonymous]
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class EnadeController: ControllerBase
    {

        public EnadeContext Econtext;
        public RegimeContext regContext;
        public EnadeController(EnadeContext EContext, RegimeContext RegContext)
        {
            this.Econtext = EContext;
            this.regContext = RegContext;
        }


        /*
        //Get api/Professores
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            

            try
            {
                var results = await Professores.getProfessoresIES(context).ToListAsync(); //Where(p => p.titulacao != "NÃO IDENTIFICADA" & p.titulacao != "GRADUADO")
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
                        }

                    });
                    
                
                return Ok(results);
                
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
            }

        }

        */

        [AllowAnonymous]
        [HttpGet("ObterCicloporId/{_id}")]
        public ActionResult obterCicloporId(long _id)
        {
            var resultado = this.Econtext.Ciclo.Select(x => new {x.IdCiclo, x.DescricaoCiclo, x.DescArea}).FirstOrDefault(x => x.IdCiclo == _id);
            
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("ObterCiclos")]
        public async Task<IActionResult> obterCiclos()
        {
            var query = await this.Econtext.Ciclo
                             .Select(x => new {x.IdCiclo, x.DescricaoCiclo, x.DescArea})
                             //.OrderByDescending(x => x.IdCiclo)
                             .OrderBy(x => x.IdCiclo)
                             .ToArrayAsync();
    
            return Ok(query);
        }
        
        [AllowAnonymous]
        [HttpGet("ObterDescCiclo/{campo}")]
        public ActionResult ObterDescCiclo(string campo)
        {
            var resultado = this.Econtext.Ciclo.Select(x => new {x.DescricaoCiclo}).FirstOrDefault(x => x.DescricaoArea == campo);
            
            return Ok(resultado);
        }

        /*
        [AllowAnonymous]
        [HttpGet("Busca/{campo}")]
        public async Task<IActionResult> BuscaProfessores(string campo)
        {
                
                Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            
                try
                {

                 Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                        // erro
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
        */


    }
}