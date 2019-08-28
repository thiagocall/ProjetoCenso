using System;
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

        [HttpGet("{id}")]
        public ActionResult Get(long? id){

            
            return null;
            

        }


        
    }
}