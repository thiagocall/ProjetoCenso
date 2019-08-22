using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model.Censo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers.Censo
{
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class CursoCensoController : ControllerBase
    {

        public CursoCensoContext Context { get; }
        public ProfessorCursoCensoContext ProfCurCensoctx { get; }

        public CursoCensoController(CursoCensoContext context, ProfessorCursoCensoContext profCurCensoctx)
        {
            this.Context = context;
            this.ProfCurCensoctx = profCurCensoctx;
        }
        
        // GET api/values
        
        [HttpGet]
        public  ActionResult Get()
        {
            var results = ProfCurCensoctx.ProfessorCursoCenso.Count();

            return Ok(results);
            
        }

        // GET api/Censo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var results = await Context.CursoCenso.Where(x => x.CodCampus == id).ToListAsync();
            
                return Ok(results);
                
            }
            catch (System.Exception)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados");
            }
        
        }

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
    }
}
