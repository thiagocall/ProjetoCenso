using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Campus;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.ForaDeSede;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Censo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegulatorioController : ControllerBase
    {
        public ProfessorIESContext Context;
        public ProfessorContext Profcontext;
        public RegimeContext RegContext;
        public CensoContext CContext;
        public CargaContext CgContext;
        public CampusContext CampusContext;
        IConfiguration Configuration;

        List<string> listaForaSede;
        
        public Dictionary<string, List<string>> dicProfessorCampus;

        public RegulatorioController(ProfessorIESContext Context, ProfessorContext ProfContext
                                    , RegimeContext regimeContext
                                    , CensoContext CContext
                                    , CargaContext cargaContext
                                    , CampusContext _campusContext
                                    , IConfiguration _configuration)
        {

            if (dicProfessorCampus == null)
            {
                  dicProfessorCampus = CampusProfessor.getCampusProfessor(_configuration);
            }
    
            this.Context = Context;
            this.Profcontext = ProfContext;
            this.RegContext = regimeContext;
            this.CContext = CContext;
            this.CgContext = cargaContext;
            this.CampusContext = _campusContext;

            this.listaForaSede = new List<string>(){
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

        }
        


        // POST api/values
     
        // EXPORTACAO CORPO DOCENTE EM PLANILHA EXCEL
        // AJUSTE PARA CIRACAO DE DOWNLOAD EXCEL PASSANDO O ID - THIAGO CALDAS
        [AllowAnonymous]
        [HttpGet("BuscaIesID/Excel/{id}")]
        public async Task<IActionResult> BuscaIesDownload(long id)
        {


            try
            {

                var results = Professores.getProfessoresIES(Context).Where(p => p.CodInstituicao == id).ToList();

                    //var results = regContext.ProfessorRegime.Where(p => p.NumMatricula == id).ToList();

                var dic = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

                foreach (var item in results)
            {
                if (dic.ContainsKey(item.CpfProfessor.ToString()))
                        {
                            item.regime = dic[item.CpfProfessor.ToString()].Regime;
                        }
            }

            //  Monta arquivo para Download em Excel

             var stream = new MemoryStream();

             using (var package = new ExcelPackage(stream)) {                
                var workSheet = package.Workbook.Worksheets.Add("ProfessorIES");
                workSheet.Cells.LoadFromCollection(results, true);
                // workSheet.Column(3).Style.Numberformat.Format = "dd/MM/yyyy";

                                                    //         .Select(x =>     new {CPF = x.CpfProfessor,
                                                    //          NOME = x.NomProfessor,
                                                    //          NASCIMENTO = x.DtNascimentoProfessor.Value.ToString("dd/MM/yyyy"),
                                                    //          REGIME = x.regime,
                                                    //          TITULACAO = x.Titulacao
                                                    //    })
                package.Save();            
            };  

                stream.Position = 0;
                var contentType = "application/octet-stream";
                var fileName = "file.xlsx";

                return File(stream, contentType, fileName);
            }
            catch (System.Exception)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }                
                      

        }


        // POST api/values
     
        // EXPORTACAO CORPO DOCENTE EM PLANILHA EXCEL
        // AJUSTE PARA CRIAÇÃO DE DOWNLOAD EXCEL POR IES PASSANDO ID THIAGO CALDAS
        [AllowAnonymous]
        [HttpGet("BuscaIes/Excel")]
        public async Task<IActionResult> IesDownload()
        {


            try
            {

                var results = Professores.getProfessores(this.Profcontext).ToList();

                    //var results = regContext.ProfessorRegime.Where(p => p.NumMatricula == id).ToList();

                var dic = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

                foreach (var item in results)
            {
                if (dic.ContainsKey(item.CpfProfessor.ToString()))
                        {
                            item.regime = dic[item.CpfProfessor.ToString()].Regime;
                        } else {
                            item.regime = "CHZ/AFASTADO";
                        }
            }

            //  Monta arquivo para Download em Excel

             var stream = new MemoryStream();

             using (var package = new ExcelPackage(stream)) {                
                var workSheet = package.Workbook.Worksheets.Add("ProfessorIES");
                workSheet.Cells.LoadFromCollection(results.Select(x =>     new {CPF = x.CpfProfessor,
                                                              NOME = x.NomProfessor,
                                                              NASCIMENTO = x.DtNascimentoProfessor.Value.ToString("dd/MM/yyyy"),
                                                              REGIME = x.regime,
                                                              TITULACAO = x.Titulacao
                                                        }), true);
                package.Save();            
            };  

                stream.Position = 0;
                var contentType = "application/octet-stream";
                var fileName = "file.xlsx";

                return File(stream, contentType, fileName);
            }
            catch (System.Exception)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }                
                      

        }
                
        // INICIO PROFESSOR IES
       [AllowAnonymous]
       [HttpGet("BuscaIes/{id}")]
        public ActionResult<List<ProfessorIes>> BuscaIes(long? id)
        {

            var results = Professores.getProfessoresIES(Context).Where(p => p.CodInstituicao == id).ToList();

            //var results = regContext.ProfessorRegime.Where(p => p.NumMatricula == id).ToList();

                var dic = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

                 foreach (var item in results)
                {
                    if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = dic[item.CpfProfessor.ToString()].Regime;
                            }
                }
        
                return Ok(results);
                
                // var results = Professores.getProfessoresIES(context).Where(x => x.CpfProfessor == id).ToList();
                // return results;  

        }


        //Curso Professor Emec
        [AllowAnonymous]
        [HttpGet("Emec/{id}")]
        public async Task<IActionResult> Get(long? id)
        {
            Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            Dictionary<string, Professor> dicprof = new Dictionary<string, Professor>();
            
            Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      dic = CgContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());                      
                      dicprof = this.Profcontext.Professores.ToDictionary(x => x.CpfProfessor.ToString());
                    }
                    );
            
      
            var query = await this.CContext.ProfessorCursoEmec
                            .Where(p => p.CodEmec == id)
                                    .ToListAsync();

            Task.WaitAll(task1);
         
                var results = query.Select(x => new 
                                    {   
                                        CpfProfessor = x.CpfProfessor, 
                                        CodIes = x.CodIes,
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        NumHabilitacao = (long?)x.NumHabilitacao == -1 ? null : (long?)x.NumHabilitacao,
                                        regime = dic.TryGetValue(x.CpfProfessor.ToString(),out ProfessorRegime pp) ? pp.Regime:"CHZ/AFASTADO",
                                        Qtd_Horas_DS = dic.TryGetValue(x.CpfProfessor.ToString(), out ProfessorRegime ps ) ? Math.Round((decimal)ps.QtdHorasDs, 2) : 0,
                                        Qtd_Horas_FS = dic.TryGetValue(x.CpfProfessor.ToString(), out ProfessorRegime pf ) ? Math.Round((decimal)pf.QtdHorasFs, 2) : 0,
                                        nomprofessor = dicprof.TryGetValue(x.CpfProfessor.ToString(), out Professor pr) ? pr.NomProfessor : "",
                                        Titulacao = dicprof.TryGetValue(x.CpfProfessor.ToString(), out Professor tit) ? tit.Titulacao : ""
                                        }).ToList();

            return Ok(results);
            
        }
       
        /* fora de sede */
        [AllowAnonymous]
        [HttpGet("foradesede/{id}")]
        public ActionResult foradesede(long? id)
        //public async Task<IActionResult> Get(long? id)
        {
            var dicRegime = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

    

            var campProfessor = CampusProfessor.getCampusProfessor(this.Configuration);

            var results = ForaDeSedePr.OtimizaProfessorForaDeSede(Context.ProfessorIES, dicProfessorCampus)
             .Select(p => new
                                           {
                                               CpfProfessor = p.CpfProfessor,
                                               NomProfessor = p.NomProfessor,
                                               ativo = p.ativo,
                                               regime = dicRegime.ContainsKey(p.CpfProfessor.ToString()) ? dicRegime[p.CpfProfessor.ToString()].Regime : null ,
                                               titulacao = p.titulacao,
                                               campi = campProfessor.FirstOrDefault(c => c.Key == p.CpfProfessor.ToString()).Value.ToList()
                                               //}).Where(c => c.CodCampus == id).ToList();
                                            }
                                        )
                               .ToList();

            return Ok(results);

        }
  

        /* inicio */
        [AllowAnonymous]
        [HttpGet("BuscaCampus")]

        public ActionResult<List<CampusContext>> BuscaCampus()
        {

            //var results = CampusContext.TbSiaCampus.Where(p => p.CodCampus == id).ToList();
            var results = CampusContext.TbSiaCampus.Select(x => new { CodCampus = (int)x.CodCampus, x.NomCampus})
                                                    .Where(x => this.listaForaSede.Contains(x.CodCampus.ToString()))
                                                    .OrderBy(x => x.NomCampus)
                                                    .ToList() ;
            
            return Ok(results);
        }
        /* fim */


        /* busca professor dentro dos campus */

        [AllowAnonymous]
        [HttpGet("BuscaProfCampus")]

        public ActionResult<List<CampusContext>> BuscaProfCampus()
        {

            //var results = CampusContext.TbSiaCampus.Where(p => p.CodCampus == id).ToList();
            var results = CampusContext.TbSiaCampus.Select(x => new { CodCampus = (int)x.CodCampus, x.NomCampus})
                                                    .OrderBy(x => x.NomCampus)
                                                    .ToList() ;
            
            return Ok(results);
        }
        /* fim */

    }
}
