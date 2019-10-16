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

namespace Censo.API.Controllers.Censo
{
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class EmecController: ControllerBase
    {

        public EmecController()
        {
            
        }
        

        // download pdf from site
        [HttpGet]
        public ActionResult Get(){

            FileStream fs = new FileStream(@"C:\Users\thiago.caldas\Desktop\HOSTGATOR BRASIL HOSPEDAGEM DE SITES.pdf", FileMode.Open, FileAccess.Read);
            
            
            
            FileStreamResult fsr = new FileStreamResult(fs ,"application/pdf");
            return fsr;
            
        }


        
    }
}