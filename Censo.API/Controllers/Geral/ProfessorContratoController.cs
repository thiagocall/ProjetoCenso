using System.Collections.Generic;
using System.Threading.Tasks;
using Censo.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers

{
    [Route("api/ProfessorContrato")]
    [ApiController]
    public class ProfessorContratoController: ControllerBase
    {   

        ProfessorContratoContext ProfessorContratoContext;

            public ProfessorContratoController(ProfessorContratoContext _profesorContratoCtx)
            {
                this.ProfessorContratoContext = _profesorContratoCtx;
            }


        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var results = await this.ProfessorContratoContext.ProfessorContrato.ToArrayAsync();
        
            return Ok(results);

        }

        
    }
}