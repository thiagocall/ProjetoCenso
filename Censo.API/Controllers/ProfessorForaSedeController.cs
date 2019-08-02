using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Campus;
using Censo.API.Model;
using System.Net.Http;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Censo.API.Controllers
{
    [Route("api/ForaDeSede")]
    public class ProfessorForaSedeController: ControllerBase
    {
        public Dictionary<string, List<string>> dicProfessorCampus;
        public ProfessorForaSedeController(ProfessorContext _professorContext)
        {
            if (dicProfessorCampus == null)
            {
                  dicProfessorCampus = CampusProfessor.getCampusProfessor();
            }
        }

        [HttpGet]
        public ActionResult Get()
        {
            //var results = new string[] {"value 1", "value 2", "value 3"};
            var results = dicProfessorCampus.Where(x => x.Key == "3566706").Select(x => x.Key).ToArray();

            return Ok(results);

        }


        
    }
}