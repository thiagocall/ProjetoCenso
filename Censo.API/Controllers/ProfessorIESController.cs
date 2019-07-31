using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers
{
    [Route ("api/[controller]")]
    public class ProfessorIESController: ControllerBase
    {

        public ProfessorIESContext context;
        public RegimeContext regContext;

        public ProfessorIESController(ProfessorIESContext Context, RegimeContext RegContext)
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
                var results = await Professores.getProfessoresIES(context).ToListAsync();
                await Task.Run ( ()=> {
                    Parallel.ForEach(results, (item)=>{
                        item.regime = regContext.ProfessorRegime.FirstOrDefault(x => x.NumMatricula == item.NumMatricula).Regime;
                    });
                    });

                
                return Ok(regContext.ProfessorRegime.Count());
                
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
            }

        }

        [HttpGet("{id}")]
        public ActionResult<List<ProfessorIes>> Get(long? id)
        {

            var results = Professores.getProfessoresIES(context).Where(p => p.CpfProfessor == id).ToList();
            
            //var results = regContext.ProfessorRegime.Where(p => p.NumMatricula == id).ToList();

                var dic = regContext.ProfessorRegime.ToDictionary(x => x.NumMatricula);

                 foreach (var item in results)
                {
                    if (dic.ContainsKey(item.NumMatricula))
                    {
                        item.regime = dic[item.NumMatricula].Regime;
                    }
                }
                
        
                return Ok(results);
                
                // var results = Professores.getProfessoresIES(context).Where(x => x.CpfProfessor == id).ToList();
                // return results;  

        }

    }
}