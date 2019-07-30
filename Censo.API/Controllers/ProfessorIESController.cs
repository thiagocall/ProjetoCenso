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

        public ProfessorIESController(ProfessorIESContext Context)
        {
            this.context = Context;
        }

        
        //Get api/Professores
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            try
            {
                var results = await Professores.getProfessoresIES(context).ToListAsync();
                return Ok(results);
                
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
            }

        }

        [HttpGet("{id}")]
        public ActionResult<List<ProfessorIes>> Get(long? id)
        {
                var results = Professores.getProfessoresIES(context).Where(x => x.CpfProfessor == id).ToList();
                return results;  

        }




    }
}