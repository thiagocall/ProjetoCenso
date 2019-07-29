using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Professor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers
{
    [Route ("api/[controller]")]
    public class ProfessorController: ControllerBase
    {

        public ProfessorContext Context;
        public ProfessorController(ProfessorContext _context)
        {
            this.Context = _context;
            
        }
        
        //Get api/Professores
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await this.Context.Professores.ToArrayAsync();
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
                var results = await this.Context.Professores.Where(x => x.CpfProfessor == id).ToArrayAsync();
                return Ok(results);
                
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
            }

        }




    }
}