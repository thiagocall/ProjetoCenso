using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data.Censo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers.Censo
{
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class OtimizacaoController: ControllerBase
    {
        CensoContext Context;

        public OtimizacaoController(CensoContext _context)
        {
            this.Context = _context;
        }

        // public async List<string> getProfessorCurso() {

             
            
        // }
        


        
    }
}