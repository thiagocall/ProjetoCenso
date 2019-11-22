using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Censo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegulatorioController : ControllerBase
    {
        public ProfessorIESContext context;
        public ProfessorContext Profcontext;
                
        public RegimeContext RegContext;
        
        public RegulatorioController(ProfessorIESContext Context, ProfessorContext ProfContext, RegimeContext regimeContext)
        {
            this.context = Context;
            this.Profcontext = ProfContext;
            this.RegContext = regimeContext;

        }
        
     
        // POST api/values
     
        // EXPORTACAO CORPO DOCENTE
        [AllowAnonymous]
        [HttpGet("BuscaIes/Excel")]
        public async Task<IActionResult> BuscaIesDownload()
        {

            try
            {


                 Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
            

                 Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                      dic = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                    }
                    );

                
                Task.WaitAll(task1);

                var results =  await Professores.getProfessores(this.Profcontext).ToListAsync();

                                foreach (var item in results)
                                {
                                    if (dic.ContainsKey(item.CpfProfessor.ToString()))
                                    {
                                        item.regime = dic[item.CpfProfessor.ToString()].Regime;
                                    }

                                    else
                                    {
                                        item.regime = "CHZ/AFASTADO";
                                    }
                                }


            //  Monta arquivo para Download em Excel

             var stream = new MemoryStream();

             using (var package = new ExcelPackage(stream)) {                
                var workSheet = package.Workbook.Worksheets.Add("ProfCursoCenso");
                workSheet.Cells.LoadFromCollection(results
                                                .Select(x => 
                                                        new {CPF = x.CpfProfessor,
                                                             NOME = x.NomProfessor,
                                                             NASCIMENTO = x.DtNascimentoProfessor.Value.ToString("dd/MM/yyyy"),
                                                             REGIME = x.regime,
                                                             TITULACAO = x.Titulacao
                                                       }), true);
                // workSheet.Column(3).Style.Numberformat.Format = "dd/MM/yyyy";
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

            var results = Professores.getProfessoresIES(context).Where(p => p.CodInstituicao == id).ToList();
            
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

    }
}
