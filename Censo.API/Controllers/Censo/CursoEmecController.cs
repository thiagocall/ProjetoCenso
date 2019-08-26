using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers.Censo
{
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class CursoEmecController : ControllerBase
    {

        public ProfessorCursoEmecContext Context { get; }

        public CursoEmecController(ProfessorCursoEmecContext context)
        {
            this.Context = context;
        }
        

         [HttpGet]
        public ActionResult Get()
        {
            var results = Context.ProfessorCursoEmec.ToList();
            
            return Ok(results);
            
        }

        [HttpGet("EmecProf")]
        public ActionResult Get(string str = "s")
        {
            var query = Context.ProfessorCursoEmec.ToList();

            Dictionary<long, CursoProfessor> cursoProfessor = new Dictionary<long, CursoProfessor>();

            foreach (var res in query)
            {
                if(cursoProfessor.ContainsKey(res.CodEmec)){
                    CursoProfessor prof = cursoProfessor[res.CodEmec];

                    if (!prof.Professores.Contains(res.CpfProfessor))
                    {
                         prof.Professores.Add(res.CpfProfessor);
                    }

                }
                else
                {
                    CursoProfessor prof = new CursoProfessor();
                    prof.CodEmec = res.CodEmec;
                    prof.Professores  = new List<long>();
                    prof.Professores.Add(res.CpfProfessor);
                    cursoProfessor.Add(prof.CodEmec, prof);

                }
            }


            var resu = cursoProfessor.Select(x => new {codEmec =  x.Key, Professores = x.Value});
            var results = resu.ToList();

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
