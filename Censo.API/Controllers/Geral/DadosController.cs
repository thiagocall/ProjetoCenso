using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data.Censo;
using Censo.API.Model.Censo;
using Censo.API.Model.dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Censo.API.Controllers.Censo;
using Microsoft.AspNetCore.Authorization;
using Censo.API.Model;

namespace Censo.API.Controllers.Geral
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DadosController: ControllerBase
    {
    private CensoContext CensoContex { get; set; }
    public dadosContext DadosContext { get; set; }
    public CampusContext CampusContext { get; set; }

    public RegionalSiaContext RegionalContext {get;set;}
        public DadosController(CensoContext _censoContex, dadosContext _dadosContext, RegionalSiaContext _regContext, CampusContext _campusContex)
        {   
            this.CensoContex = _censoContex;
            this.DadosContext = _dadosContext;
            this.RegionalContext = _regContext;
            this.CampusContext = _campusContex;

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

            return Ok(new {campus, cursos});


        }


        [HttpGet("getIES")]
        public async Task<IActionResult> getIes () {

            var result = await this.RegionalContext.RegionalSia.ToListAsync();

            var ies = Task.Factory.StartNew( () =>
            {    
                return      result.Distinct<RegionalSia>(new CodIesComparer())
                            .Select(x => new { CodIes = (int)x.CodIes, x.NomIes})
                            .ToList();
            }
            );


            var campus = Task.Factory.StartNew( () =>
            {    
                return      result.Distinct<RegionalSia>(new CodCampusComparer())
                            .Select(x => new { CodCampus = (int)x.CodCampus, x.NomCampus, CodIes = (int)x.CodIes})
                            .ToList();
            }
            );

            Task.WaitAll(ies, campus);

            var resultFinal = new {
                ies = ies.Result,
                campus = campus.Result
            };
            

            return Ok(resultFinal);

        }


        [HttpGet("getCampus")]
        public async Task<IActionResult> getCampus () {

            var curso = this.CensoContex.CursoCenso.ToListAsync();
            var campus = this.CampusContext.TbSiaCampus.Select(x => new {codCurso = (int)x.CodCampus, nomCurso = x.NomCampus}).ToListAsync();

            var resultado = new {Cursos = await curso, Campi = await campus};

            return Ok(resultado);

        }

    }


    public class CodIesComparer : IEqualityComparer<RegionalSia>
    {
        public bool Equals(RegionalSia x, RegionalSia y)
        {
            if ( x.CodIes  == y.CodIes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(RegionalSia obj)
        {
            return obj.CodIes.GetHashCode();
        }
    }

    public class CodCampusComparer : IEqualityComparer<RegionalSia>
    {
        public bool Equals(RegionalSia x, RegionalSia y)
        {
            if ( x.CodCampus  == y.CodCampus)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(RegionalSia obj)
        {
            return obj.CodCampus.GetHashCode();
        }
    }



    public class CampusComparer : IEqualityComparer<CampusSia>
    {
        public bool Equals(CampusSia x, CampusSia y)
        {
            if ( x.CodCampus  == y.CodCampus)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(CampusSia obj)
        {
            return obj.CodCampus.GetHashCode();
        }
    }
}

