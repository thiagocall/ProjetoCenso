
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using System.Net.Http;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Censo.API.Model.dados;
using Censo.API.Data.Censo;
using Censo.API.Model.Censo;

namespace Censo.API.Controllers.Enade
{
    [AllowAnonymous]
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class EnadeController: ControllerBase
    {

        public EnadeContext Econtext;
        public CampusContext CampContext;
        private CensoContext CensoContex { get; set; }
        public dadosContext DadosContext { get; set; }

        public EnadeController(EnadeContext EContext, CampusContext CampContext, CensoContext CContext, dadosContext DContext)
        {
            this.Econtext = EContext;
            this.CampContext = CampContext;
            this.CensoContex = CContext;
            this.DadosContext = DContext;
            
        }

        /*
        //Get api/Enade/Todosciclos
        [AllowAnonymous]
        [HttpGet("Todosciclos")]
        public async Task<IActionResult> Todosciclos()
        //Todosciclos(long _id)
        
        //public async Task<IActionResult> Get()
        {
            

            try
            {
                var results = await Professores.getProfessoresIES(context).ToListAsync(); //Where(p => p.titulacao != "NÃO IDENTIFICADA" & p.titulacao != "GRADUADO")
                var dic = regContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

                await Task.Run (
                    () => 
                    {
                        foreach (var item in results)
                        {
                            if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }
                        }

                    });
                    
                
                return Ok(results);
                
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
            }

        }
        */

        

        [AllowAnonymous]
        [HttpGet("ObterCicloporId/{_id}")]
        public ActionResult obterCicloporId(long _id)
        {
            var resultado = this.Econtext.Ciclo.Select(x => new {x.IdCiclo, x.DescricaoCiclo, x.DescArea}).FirstOrDefault(x => x.IdCiclo == _id);
            
            return Ok(resultado);
        }

        
        [AllowAnonymous]
        [HttpGet("ObterCiclos")]
        public async Task<IActionResult> obterCiclos()
        {
            var query = await this.Econtext.Ciclo
                             .Select(x => new {x.IdCiclo, x.DescArea, x.DescricaoCiclo})
                             .OrderByDescending(x => x.IdCiclo)
                             .OrderBy(x => x.IdCiclo)
                             .ToArrayAsync();

            return Ok(query);
        }

        // Não está sendo usado no Front-End
        [AllowAnonymous]
        [HttpGet("DescricaoCiclos")]
        public async Task<IActionResult> DescricaoCiclos()
        {
            var query = await this.Econtext.Ciclo
                             .Select(x => new {x.IdCiclo,x.DescArea})
                             //.OrderByDescending(x => x.IdCiclo)
                             //.OrderBy(x => x.IdCiclo)
                             .ToArrayAsync();
    
                        string juntaarea = "";
                        //Int64 item = 1;
            
            foreach (var item in query)
            {
                
                if (item.DescArea != null)
                {
                    juntaarea = juntaarea + item.DescArea;
                }
                

            }
            
            /*
            for (int i = 0;i <3; i++)
                {
                    juntaarea = juntaarea + query[i].DescArea;
                }
            */
            return Ok(juntaarea);
        }
        

        [AllowAnonymous]
        [HttpGet("ObterDescCiclo/{campo}")]
        public ActionResult ObterDescCiclo(string campo)
        {
            // Analisar porque nao mostra o x.Descrocapgit
            var resultado = this.Econtext.Ciclo.Select(x => new {x.DescricaoCiclo}).FirstOrDefault(x => x.DescricaoCiclo == campo);
            //var resultado = this.Econtext.Ciclo.Select(x => new {x.Obs, x.DescricaoCiclo}).FirstOrDefault(x => x.DescricaoCiclo == campo);
            
            return Ok(resultado);
        }


        [AllowAnonymous]
        [HttpGet("ElegerAreasCiclos/{_id}")]
        public ActionResult ElegerAreasCiclos(long _id)
        {
            //var resultado = this.EContext..Select(x => new {x.Id, x.Resumo, x.indOficial}).FirstOrDefault(x => x.Id == _id);
            var resultado = this.Econtext.EmecCiclo.Select(x => new {x.IdCiclo, x.CodCursoEmec}).Select(x => x.IdCiclo != _id);
            //var resultadoAtual = this.ProducaoContext.TbResultadoAtual.Select(x => new {x.Id, x.Resumo}).FirstOrDefault(x => x.Id == _id);

            //var resultadoCompleto = new {resultado, resultadoAtual};
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("MostraAreas/{_id}")]
        public async Task<IActionResult> MostraAreas(int _id)
        {
            var query = await this.Econtext.EmecCiclo.ToListAsync();
            

                var results = query.Select(x => new
                                    {   
                                        cod_area_emec = x.CodCursoEmec,
                                        id_ciclo = x.IdCiclo
                                        }).Where(c => c.id_ciclo != _id).ToList();


            return Ok(results);
        
        }


        [AllowAnonymous]
        [HttpGet("BuscaAreasCiclos")]
        public async Task<IActionResult> BuscaAreasCiclos() {

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

            // Pega o codigo do curso
            var curso1 = task1.Result.Select(x => x.CodCampus).Distinct().ToList();

            var campus = task2.Result.Where(c => curso1.Contains((long)c.CodCampus))
                                     .OrderBy(x => x.NomCampus)
                                     .ToList();
             
            var cursos = task1.Result.Where(x => x.CodIes != null).ToList();

            return Ok(new {campus});


        }

        [AllowAnonymous]
        [HttpGet("TodosCampus")]
        public async Task<IActionResult> TodosCampus () {

            var campus = this.CampContext.TbSiaCampus.Select(x => new {codCampus = (int)x.CodCampus, nomCampus = x.NomCampus}).ToListAsync();

            var resultado = new {Campi = await campus};

            return Ok(resultado);

        }


        [AllowAnonymous]
        [HttpGet("ObtemDadosEnade")]
        public async Task<IActionResult> ObtemDadosEnade () {

            var curso = this.CensoContex.CursoCenso.ToListAsync();
            var campus = this.CampContext.TbSiaCampus.Select(x => new {codCampus = (int)x.CodCampus, nomCampus = x.NomCampus}).ToListAsync();
            var area = this.Econtext.EmecCiclo.Select(x => new {cod_area_emec = x.IdCiclo, id_ciclo = x.CodCursoEmec})

            var resultado = new {Cursos = await curso, Campi = await campus};

            return Ok(resultado);

        }



    }
}