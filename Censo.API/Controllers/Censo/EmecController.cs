using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Censo.API.Controllers.Censo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Censo.API.Controllers.Censo
{
    [AllowAnonymous]
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class EmecController: ControllerBase
    {

        public EmecController()
        {
            
        }
        

        // download pdf from site
        [HttpGet]
        public FileStreamResult  Get(){

            FileStream fs = new FileStream(@"C:\Users\thiago.caldas\Desktop\Atividades_2019-11-05.xlsx", FileMode.Open, FileAccess.Read);
            
            // File fsr = File(fs ,"application/Excel", "Alunos.xlsx");

    
            return File(fs ,"application/Excel", "Alunos.xlsx");
            
        }


        
    }
}