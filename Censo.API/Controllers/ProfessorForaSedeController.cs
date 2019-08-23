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
        //ProfessorContext ProfessorContext;

        RegimeContext RegimeContext;

        ProfessorIESContext ProfessorIESContext;
        public Dictionary<string, List<string>> dicProfessorCampus;


        public ProfessorForaSedeController(ProfessorIESContext _professorContext, CampusContext _campusContext, RegimeContext _regimeContext)
        {
            if (dicProfessorCampus == null)
            {
                  dicProfessorCampus = CampusProfessor.getCampusProfessor();
            }

            this.CampusContext = _campusContext;

            this.ProfessorIESContext = _professorContext;

            this.RegimeContext = _regimeContext;


        }

        [HttpGet]
        public ActionResult Get()
        {

            var dicRegime = RegimeContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

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

            var campProfessor = CampusProfessor.getCampusProfessor();

            var results = ForaDeSedePr.OtimizaProfessorForaDeSede(ProfessorIESContext.ProfessorIES, dicProfessorCampus)
             .Select(p => new
                                           {
                                               CpfProfessor = p.CpfProfessor,
                                               NomProfessor = p.NomProfessor,
                                               ativo = p.ativo,
                                               regime = dicRegime.ContainsKey(p.CpfProfessor.ToString()) ? dicRegime[p.CpfProfessor.ToString()].Regime : null ,
                                               titulacao = p.titulacao,
                                               campi = campProfessor.Where( x => x.Key == p.CpfProfessor.ToString()).Select(s => s.Value.ToList())
                                            }
                                        )
                               .ToList();

            return Ok(results);

        }


        
    }
}