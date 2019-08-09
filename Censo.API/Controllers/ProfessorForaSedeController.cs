using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Campus;
using Censo.API.Model;
using Censo.API.ForaDeSede;
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

        CampusContext CampusContext;

        ProfessorContext ProfessorContext;
        public Dictionary<string, List<string>> dicProfessorCampus;


        public ProfessorForaSedeController(ProfessorContext _professorContext, CampusContext _campusContext)
        {
            if (dicProfessorCampus == null)
            {
                  dicProfessorCampus = CampusProfessor.getCampusProfessor();
            }

            this.CampusContext = _campusContext;

            this.ProfessorContext = _professorContext;
        }

        [HttpGet]
        public ActionResult Get()
        {
            // ########### Método para criação dos professores fora de SEDE

            List<string> listaForaSede =new List<string>(){
                    "4"
                    ,"5"
                    ,"7"
                    ,"8"
                    ,"33"
                    ,"42"
                    ,"43"
                    ,"44"
                    ,"49"
                    ,"51"
                    ,"52"
                    ,"61"
                    ,"67"
                    ,"297"
                    ,"301"
                    ,"564"
                    ,"720"
                    ,"721"
                    ,"1002"

            };


            //var campProfessor = CampusProfessor.getCampusProfessor();

            //var results = ForaDeSedePr.OtimizaProfessorForaDeSede(ProfessorContext.Professores, dicProfessorCampus).FirstOrDefault(c => c.CpfProfessor.ToString() == "457721774" );
            var results =  dicProfessorCampus["545258707"];
            //campProfessor.Where(x => listaForaSede.Contains(x.Value));
            
            //var results = dicProfessorCampus.Where(x => x.Key == "3566706").Select(x => x.Key).ToArray();
            //var results = CampusContext.TbSiaCampus.Where(x => x.CodCampus == 327);

            //return Ok("{'value1', 'value2'}");
            return Ok(results);

        }


        
    }
}