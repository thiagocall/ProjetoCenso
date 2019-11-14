// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Censo.API.Data;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace Censo.API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class ReguularioController : ControllerBase
//     {

//         public ProfessorContext Context { get; }


//         public ReguularioController(ProfessorContext context)
//         {
//             this.Context = context;
//         }
        
//         // GET api/values
        
        

//         [HttpGet]
//         public async Task<IActionResult> Get()
//         {
// //Where(x => x.CpfProfessor == 3566706)
//              try
//             {
//                 var results = await Context.Professores.ToListAsync();
            
//                 return Ok(results);
                
//             }
//             catch (System.Exception)
//             {
                
//                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados");
//             }
            
//         }

//         // GET api/values/5
//         [HttpGet("{id}")]
//         public async Task<IActionResult> Get(string id)
//         {
//             try
//             {
//                 var results = await Context.Professores.Where(x => x.CpfProfessor == id).ToListAsync();
            
//                 return Ok(results);
                
//             }
//             catch (System.Exception)
//             {
                
//                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados");
//             }
        
//         }

//         // POST api/values
//         [HttpPost]
//         public void Post([FromBody] string value)
//         {
//         }

//         // PUT api/values/5
//         [HttpPut("{id}")]
//         public void Put(int id, [FromBody] string value)
//         {
//         }

//         // DELETE api/values/5
//         [HttpDelete("{id}")]
//         public void Delete(int id)
//         {
//         }
//     }
// }
