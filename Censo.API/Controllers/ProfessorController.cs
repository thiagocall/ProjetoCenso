using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Censo.API.Resultados;

namespace Censo.API.Controllers
{
    [Route ("api/[controller]")]
    public class ProfessorController: ControllerBase
    {

         public ProfessorContext context;

        public ProfessorController(ProfessorContext Context)
        {
            this.context = Context;
        }
        
        //Get api/Professores
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
                try
                {
                    var results =  await Professores.getProfessores(context).ToArrayAsync();

                    return Ok(results);
                    
                }
                catch (System.Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(decimal id)
        {
            
             try
                {
                    var results =  await Professores.getProfessores(context).Where(x => x.CpfProfessor == id).ToArrayAsync();

                    return Ok(results);
                    
                }
                catch (System.Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }

        }




    }
}