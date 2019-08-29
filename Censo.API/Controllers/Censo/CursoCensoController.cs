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
        public ProfessorCursoCensoContext ProfCurCensoCtx { get; }

        public CursoCensoController(CursoCensoContext context, ProfessorCursoCensoContext _profCurCensoCtx)
        {
            this.Context = context;
            this.ProfCurCensoCtx = _profCurCensoCtx;
        }
        
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await ProfCurCensoCtx.ProfessorCursoCenso.ToListAsync();

                var results = query.Select(x => new 
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

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var query = await ProfCurCensoCtx.ProfessorCursoCenso.ToListAsync();

                var results = query.Select(x => new
                                    {   CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao,
                                        
                                        
                                        }).Where(c => c.CodCampus == id).ToList();

            return Ok(results);
        
        }


        //Curso Professor Emec

         [HttpGet("Emec")]
        public ActionResult Get(string emec = "sim")
        {
            var query = ProfCurCensoCtx.ProfessorCursoCenso.ToList();

                var results = query.Select(x => new 
                                    {   CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = x.NumHabilitacao
                                        }).ToList();

            return Ok("ok");
            
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
