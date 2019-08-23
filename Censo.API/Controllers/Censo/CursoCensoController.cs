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
        public async Task<IActionResult> Get()
        {
            var query = await ProfCurCensoctx.ProfessorCursoCenso.ToListAsync();

                var results = query.Select(x => new 
                                    {   CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao
                                        }).ToList();

            return Ok(results);
            
        }

        // GET api/Censo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var query = await ProfCurCensoctx.ProfessorCursoCenso.ToListAsync();

                var results = query.Select(x => new
                                    {   CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao
                                        }).Where(c => c.CodCampus == id).ToList();

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
