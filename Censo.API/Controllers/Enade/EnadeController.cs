
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
    [Authorize(Roles="Master, User")]
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class EnadeController: ControllerBase
    {

        public EnadeContext Econtext { get; set; }
        public CampusContext CampContext { get; set; }
        private CensoContext CensoContex { get; set; }
        public dadosContext DadosContext { get; set; }
        public CursoEnquadramentoContext CeContext { get;set;}


        public EnadeController(EnadeContext EContext, CampusContext CampContext, 
                                CensoContext CContext, dadosContext DContext,
                                CursoEnquadramentoContext CursoEnqContext)
        {
            this.Econtext = EContext;
            this.CampContext = CampContext;
            this.CensoContex = CContext;
            this.DadosContext = DContext;
            this.CeContext = CursoEnqContext;
            
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

        

        
        [HttpGet("ObterCicloporId/{_id}")]
        public ActionResult obterCicloporId(long _id)
        {
            var resultado = this.Econtext.Ciclo.Select(x => new {x.IdCiclo, x.DescricaoCiclo, x.DescArea}).FirstOrDefault(x => x.IdCiclo == _id);
            
            return Ok(resultado);
        }

        
        
        [HttpGet("ObterCiclos")]
        public async Task<IActionResult> obterCiclos()
        {
            var query = await this.Econtext.Ciclo
                             .Select(x => new {x.IdCiclo, x.DescArea, x.DescricaoCiclo, x.Obs, x.AnoAtual})
                             .OrderByDescending(x => x.IdCiclo)
                             .OrderBy(x => x.IdCiclo)
                             .ToArrayAsync();

            return Ok(query);
        }

        // Não está sendo usado no Front-End
      
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
        

        
        [HttpGet("ObterDescCiclo/{campo}")]
        public ActionResult ObterDescCiclo(string campo)
        {
            // Analisar porque nao mostra o x.Descrocapgit
            var resultado = this.Econtext.Ciclo.Select(x => new {x.DescricaoCiclo}).FirstOrDefault(x => x.DescricaoCiclo == campo);
            //var resultado = this.Econtext.Ciclo.Select(x => new {x.Obs, x.DescricaoCiclo}).FirstOrDefault(x => x.DescricaoCiclo == campo);
            
            return Ok(resultado);
        }


        
        [HttpGet("ElegerAreasCiclos/{_id}")]
        public ActionResult ElegerAreasCiclos(long _id)
        {
            //var resultado = this.EContext..Select(x => new {x.Id, x.Resumo, x.indOficial}).FirstOrDefault(x => x.Id == _id);
            var resultado = this.Econtext.EmecCiclo.Select(x => new {x.IdCiclo, x.CodAreaEmec}).Select(x => x.IdCiclo != _id);

            return Ok(resultado);
        }

        
        [HttpGet("MostraAreas/{_id}")]
        public async Task<IActionResult> MostraAreas(int _id)
        {
            var query = await this.Econtext.EmecCiclo.ToListAsync();
            

                var results = query.Select(x => new
                                    {   
                                        cod_area_emec = x.CodAreaEmec,
                                        id_ciclo = x.IdCiclo
                                        }).Where(c => c.id_ciclo != _id).ToList();


            return Ok(results);
        
        }


       
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

      
        [HttpGet("TodosCampus")]
        public async Task<IActionResult> TodosCampus () {

            var campus = this.CampContext.TbSiaCampus.Where(x => x.IndSituacao == "A").Select(x => new {codCampus = (int)x.CodCampus, nomCampus = x.NomCampus, indsituacao = x.IndSituacao})
                                                    .OrderBy(x => x.nomCampus)
                                                    .ToListAsync();

            var resultado = new {Campi = await campus};

            return Ok(resultado);

        }

        // Parametro codigo-curso
       
        [HttpGet("ObtemDadosEnade/{_id}")]
        //public async Task<IActionResult> ObtemDadosEnade (int _id) {
        public ActionResult ObtemDadosEnade(long _id) 
        {
            try
            {

            
            //var campus = this.CampContext.TbSiaCampus.Select(x => new {codCampus = (int)x.CodCampus, nomCampus = x.NomCampus}).ToListAsync();
            
            // Buscar os todos os cursos com o id fornecido
            //var cursos = this.CensoContex.CursoCenso.Where(x => x.CodCampus == _id).ToListAsync();

            var cursos = this.CensoContex.CursoCenso.Where(x => x.CodCampus == _id && x.CodIes != null).ToList();

            // Lista com os resultados do enade
            List<resultadoenade> Listaresultado = new List<resultadoenade>();

            
            //var novoDicEnq = new List<CursoEnquadramentoContext>();

            //----- LEVANTAR OS CONTEXTOS

            // dicionario com nome do curso
            //Dictionary<long, string> DicNomecurso = new Dictionary<long, string>();
            
            // dic rel_enquadramento_emec    EMEC // AREA
            Dictionary<int, CursoEnquadramento> DicEmecArea = new Dictionary<int, CursoEnquadramento>();
            //Dictionary<long, int> DicEmecArea = new Dictionary<long, int>();
            // cod-area -- cod-ciclo
            DicEmecArea = this.CeContext.CursoEnquadramento.ToDictionary(x => x.CodEmec);
                        
            // dic descricao do enquadramento
            //Dictionary<long, int> DicDescEnq = new Dictionary<long, int>();
            // DicDescEnq = this.CursoEnquadramento.ToDictionary(x => x.codarea)

            // cod_area_emec -- id_ciclo
            Dictionary<long, EmecCiclo> DicEmecCiclo = new Dictionary<long, EmecCiclo>();
            //DicEmecCiclo = this.CeContext.CursoEnquadramento.ToDictionary(x => x.CodEmec, x => x.CodArea);
            DicEmecCiclo = this.Econtext.EmecCiclo.ToDictionary(x => x.CodAreaEmec);

            // id_ciclo -- desc_ciclo -- desc_area -- obs -- ano_atual 
            Dictionary<long, Ciclo> DicCiclo = new Dictionary<long, Ciclo>(); 
            DicCiclo = this.Econtext.Ciclo.ToDictionary(x => x.IdCiclo); 

            // DicEmecArea = await TDicEmecArea;

            foreach (var item in cursos)
            //foreach (var item in await cursos)
                {
                    var resultadoenade = new resultadoenade();
                    resultadoenade.Nomecurso = item.NomCursoCenso;
                    resultadoenade.Codarea = DicEmecArea.TryGetValue((int)(item.CodEmec ?? 0), out var ar) ? ar.CodArea: 9999;
                    resultadoenade.Idciclo = DicEmecCiclo[resultadoenade.Codarea].IdCiclo;
                    //var indice = 1;
                    if (resultadoenade.Idciclo != -1)
                    {
                        //resultadoenade.Ciclo = DicCiclo[resultadoenade.Idciclo];
                        //resultadoenade.AnoAtual = resultadoenade.Ciclo.AnoAtual;
                        resultadoenade.AnoAtual = DicCiclo[resultadoenade.Idciclo].AnoAtual;
                    }
                    else
                    {
                        //resultadoenade.Ciclo = "Não determinado";
                        resultadoenade.AnoAtual = "99/99";
                    }

                    Listaresultado.Add(resultadoenade);
                    
                }
             
                //var area = this.Econtext.EmecCiclo.Select(x => new {cod_area_emec = x.IdCiclo, id_ciclo = x.CodCursoEmec}).Where(c => campus.Id.Contains((long)c.cod_area_emec)).ToList();


                return Ok(Listaresultado);

        }
               catch (System.Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }
            finally{
            }
        }
            
            
        // INICIO

        // [AllowAnonymous]
        //[HttpGet("SelecionaCiclos/{_id}")]
        [HttpGet("SelecionaCiclos")]
        public ActionResult SelecionaCiclos() 
        {
            try
            {
            
            // TODOS OS CICLOS E SUAS AREAS
            //var descarea = this.Econtext.EmecCiclo.Where(x => x.IdCiclo == _id).OrderBy(x => x.IdCiclo).ToList();
            var descarea = this.Econtext.EmecCiclo.OrderBy(x => x.IdCiclo).ToList();
            
            Dictionary <long, EmecCiclo> areasciclos = new Dictionary<long, EmecCiclo>();
            areasciclos = this.Econtext.EmecCiclo.ToDictionary(x => x.CodAreaEmec);

            // ERRO AQUI
            Dictionary<double, Enquadramento> descenquadra = new Dictionary<double, Enquadramento>();
            descenquadra = this.Econtext.Enquadramento.ToDictionary(x => x.CodEnq);

            // Lista com os resultados do enquadramento
            List<resultadoenquadramento> Listaresultado = new List<resultadoenquadramento>();
            

            //----- LEVANTAR outros CONTEXTOS
           
 
            // id_ciclo -- desc_ciclo -- desc_area -- obs -- ano_atual 
            Dictionary<long, Ciclo> DicCiclo = new Dictionary<long, Ciclo>(); 
            DicCiclo = this.Econtext.Ciclo.ToDictionary(x => x.IdCiclo); 

            // DicEmecArea = await TDicEmecArea;

            foreach (var item in descarea)
            //foreach (var item in await cursos)
                {
                    var resultadoenquadramento = new resultadoenquadramento();
                    //resultadoenquadramento.Idciclo = item.IdCiclo;

                    resultadoenquadramento.Idciclo = (item.IdCiclo != -1 ? item.IdCiclo : 99);
                    resultadoenquadramento.Codareaemec = item.CodAreaEmec;
                    resultadoenquadramento.descricaoarea = descenquadra[(int)item.CodAreaEmec].NomEnq;
                    

                    Listaresultado.Add(resultadoenquadramento);
                    
                }
             
                //var area = this.Econtext.EmecCiclo.Select(x => new {cod_area_emec = x.IdCiclo, id_ciclo = x.CodCursoEmec}).Where(c => campus.Id.Contains((long)c.cod_area_emec)).ToList();


                return Ok(Listaresultado);

        }
               catch (System.Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }
            finally{
            }
        }



        public class resultadoenade 
            { 
                        
                        public string Nomecurso { get; set; }
                        public int Codarea { get; set; }    
                        public long Idciclo { get; set; }
                        public string AnoAtual { get; set; }
                        //public Ciclo Ciclo { get; set; }
            }

        public class resultadoenquadramento 
            { 
                        public long Idciclo { get; set; }
                        public long Codareaemec { get; set; }    
                        public string descricaoarea { get; set; }
                        
    
            }



    }
}