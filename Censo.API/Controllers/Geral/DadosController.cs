using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data.Censo;
using Censo.API.Model.Censo;
using Censo.API.Model.dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Censo.API.Controllers.Geral
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DadosController: ControllerBase
    {
        private CensoContext CensoContex { get; set; }
    public dadosContext DadosContext { get; set; }
        public DadosController(CensoContext _censoContex, dadosContext _dadosContext)
        {   
            this.CensoContex = _censoContex;
            this.DadosContext = _dadosContext;
        }

        [HttpGet]
        public async Task<IActionResult> getDados() {


            //List<CursoCenso> cursoCenso;

            Task[] tasks = new Task[2];


            Task<List<CursoCenso>> task1 = Task.Run(

                () => {
                        //cursoCenso = this.CensoContex.CursoCenso.ToList();
                        return this.CensoContex.CursoCenso.Where(c => c.CodIes != null).ToList();     
                    }
            );

            Task<List<CampusSia>> task2 = Task.Run(
                () => {
                     return this.DadosContext.CampusSia.ToList();
                    }
            );


            tasks[0] = task1;
            tasks[1] = task2;

            await task1;
            await task2;


            var curso1 = task1.Result.Select(x => x.CodCampus).Distinct().ToList();

            var campus = task2.Result.Where(c => curso1.Contains((long)c.CodCampus))
                                     .OrderBy(x => x.NomCampus)
                                     .ToList();
            
            var cursos = task1.Result.Where(x => x.CodIes != null).ToList();

            //var cursos = task1.Result;
            //var campus = task2.Result.OrderBy(x => x.NomCampus);

            // List<object> resultados = new List<object>();
            // resultados.Add(cursos);
            // resultados.Add(campus);

            return Ok(new {campus, cursos});


        }



    }
}