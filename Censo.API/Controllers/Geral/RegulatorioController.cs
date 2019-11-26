using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                 
        public RegulatorioController(ProfessorIESContext Context, ProfessorContext ProfContext
                                    , RegimeContext regimeContext
                                    ,CensoContext CContext
                                    , CargaContext cargaContext)
        {
            this.Context = Context;
            this.Profcontext = ProfContext;
            this.RegContext = regimeContext;
            this.CContext = CContext;
            this.CgContext = cargaContext;

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
            
            Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      dic = CgContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());                      
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
                                        }).ToList();

            return Ok(results);
            
        }
       
        
    }
}
