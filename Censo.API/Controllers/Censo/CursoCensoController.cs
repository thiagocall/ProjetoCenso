using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data.Censo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers.Censo
{
    [Route("api/v1/Censo/[controller]")]
    [ApiController]
    public class CursoCensoController : ControllerBase
    {

        public CensoContext Context { get; }

        public CursoCensoController(CensoContext context)
        {
            this.Context = context;
            this.Context.ChangeTracker.QueryTrackingBehavior= QueryTrackingBehavior.NoTracking;
        }
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await this.Context.ProfessorCursoCenso.ToListAsync();

                var results = query
                    .Select(x => new 
                                    {   CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao
                                        }).ToList();

            return Ok(results);
            
        }


         [HttpGet("Cursos")]
        public async Task<IActionResult> GetCursos()
        {
            var results = await Context.CursoCenso.ToListAsync();

            return Ok(results);
            
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var query = await this.Context.ProfessorCursoCenso.ToListAsync();

                var results = query.Select(x => new
                                    {   CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao
                                        }).Where(c => c.CodCampus == id).ToList();


            return Ok(results);
        
        }
        

        //Curso Professor Emec

         [HttpGet("Emec")]
        public ActionResult GetEmec()
        {
            var query = this.Context.ProfessorCursoCenso.ToList();

                var results = query.Select(x => new 
                                    {   
                                        CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao
                                        }).ToList();

            return Ok(results);
            
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
