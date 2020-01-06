using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ProfessorMatriculaContext MatriculaContext;
        IConfiguration Configuration;

        List<string> listaForaSede;
        
        public Dictionary<string, List<string>> dicProfessorCampus;

        public RegulatorioController(ProfessorIESContext Context, ProfessorContext ProfContext
                                    , RegimeContext regimeContext
                                    , CensoContext CContext
                                    , CargaContext cargaContext
                                    , CampusContext _campusContext
                                    ,ProfessorMatriculaContext _matContext
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
            this.MatriculaContext = _matContext;

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
        
        [HttpGet("BuscaIesID/Excel/{id}")]
        public async Task<IActionResult> BuscaIesDownload(long id)
        {


            try
            {

                 List<ProfessorIes> results;

            if (id != 0) {
                results = await Professores.getProfessoresIES(Context).Where(p => p.CodInstituicao == id).ToListAsync();
            } else {
                results = await Professores.getProfessoresIES(Context).ToListAsync();
            }

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
                var workSheet = package.Workbook.Worksheets.Add("Professores");
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
       
       [HttpGet("BuscaIes/{id}")]
        public async Task<IActionResult> BuscaIes(long? id)
        {

            List<ProfessorIes> results;

            if (id != 0) {
                results = await Professores.getProfessoresIES(this.Context).Where(p => p.CodInstituicao == id).ToListAsync();
            } else {
                results = await Professores.getProfessoresIES(this.Context).ToListAsync();
            }

            //var results = regContext.ProfessorRegime.Where(p => p.NumMatricula == id).ToList();

                var dic = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());

                 foreach (var item in results)
                {
                    if (dic.ContainsKey(item.CpfProfessor.ToString()))
                            {
                                item.regime = item.regime = dic.TryGetValue(item.CpfProfessor.ToString(), out var db) ? db.Regime : "";;
                            }
                }
        
                return Ok(results);
                
                // var results = Professores.getProfessoresIES(context).Where(x => x.CpfProfessor == id).ToList();
                // return results;  

        }


        //Curso Professor Emec

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
       
        /* fora de sede pelo cod_campus */
        
        [HttpGet("foradesede/{id}")]
        public ActionResult foradesede(long id)
        //public async Task<IActionResult> Get(long? id)
        {
            var professores = getforadesede(id);
            return Ok(professores);


        }
        
        [HttpGet("foradesede/excel/{id}")]
        public ActionResult DownloadExcel(long id)
        //public async Task<IActionResult> Get(long? id)
        {
            var professores = getforadesede(id);
            
            
            //  Monta arquivo para Download em Excel

             var stream = new MemoryStream();

             using (var package = new ExcelPackage(stream)) {                
                var workSheet = package.Workbook.Worksheets.Add("ProfessorForadeSede");
                workSheet.Cells.LoadFromCollection(professores, true);
                package.Save();            
            };  

                stream.Position = 0;
                var contentType = "application/octet-stream";
                var fileName = "file.xlsx";

                return File(stream, contentType, fileName);

        }

        /* Traz todos os campi fora de sede */
        
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

        public dynamic getforadesede(long id) 
        {
            var dicRegime = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
            var diccenso = CContext.ProfessorCursoEmec.Where(x =>x.CodCampus == id)
                                                      .Select(x => x.CpfProfessor)
                                                      .Distinct().ToDictionary(x => x);

            //Trazendo o nome do campus
            var diccampus = CampusContext.TbSiaCampus.Find((decimal)id);
                                   

            var professores = Profcontext.Professores.Where(x => diccenso.ContainsKey(Convert.ToInt64(x.CpfProfessor)) && x.Ativo == "SIM")
                        


             .Select(p => new 
                                           {
                                               CpfProfessor = p.CpfProfessor,
                                               codCampus = id,
                                               NomProfessor = p.NomProfessor,
                                               ativo = p.Ativo,
                                               regime = dicRegime.ContainsKey(p.CpfProfessor.ToString()) ? dicRegime[p.CpfProfessor.ToString()].Regime : null ,
                                               titulacao = p.Titulacao,
                                               nomcampus = diccampus.NomCampus

                                            }
                                            ).ToList();

                 return professores;
        }

        [AllowAnonymous]
        [HttpPost("CalculaGapProf")]
        public async Task<IActionResult> getCalculaGapProf(ProfessorGap[] ListaProfessorGap) 
        {
               // x.CpfProfessor
            // , x.NomProfessor
            // ,x.titulacao
            // ,x.dtAdmissao
            // ,x.regime

            var admissao = await this.BuscaDataAdmissao(ListaProfessorGap.Select(x => x.Cpf.ToString()).ToList());

            // Erro no dicionario dicDemissao
         


            try 
            {
                var dicDemissao = admissao.ToDictionary(x => x.CpfProfessor);
                
                foreach (var item in ListaProfessorGap)
                {
                     item.Complemento = ComplementoCargaHoraria.CalculaGap(item.Target, item.Ds, item.Fs);  
                }

                var ListaFinal = ListaProfessorGap.Select(x => new {x.Cpf,
                                                                    x.Target,
                                                                    x.Ds, x.Fs,
                                                                    x.Complemento,
                                                                    dtAdmissao = dicDemissao[x.Cpf].dtAdmissao}).ToList();

                return Ok(ListaFinal);
            }
            catch (Exception e) {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no processamento." + e.Message);
            }

        }


        /* busca todos os professores  */
        [AllowAnonymous]
        [HttpGet("BuscaProfessor")]
        public async Task<IActionResult> BuscaProfessor()
        {
            
                try
                {
                    
                      // pegar os contextos professor e regime
                      var ListaProfessores = Professores.getProfessores(Profcontext).ToListAsync();

                      var regime  = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor);
                      //var ListaRegime = regime.Keys.ToList();
                      List<ProfessorDetalhe> ListaProfessorDetalhe = new List<ProfessorDetalhe>();
                        
                        foreach (var professor in await ListaProfessores)
                        {
                                ProfessorDetalhe professordetalhe = new ProfessorDetalhe();

                                //cpf/nomeprofessor//titulacao//regime
                                professordetalhe.CpfProfessor = professor.CpfProfessor.ToString();
                                professordetalhe.NomProfessor = professor.NomProfessor;
                                professordetalhe.titulacao = professor.Titulacao;
                                
                                if (regime.ContainsKey(professordetalhe.CpfProfessor))
                                {
                                professordetalhe.regime = regime[professordetalhe.CpfProfessor.ToString()].Regime;
                                professordetalhe.CargaTotal = (double)Math.Round((decimal)((regime[professordetalhe.CpfProfessor.ToString()].CargaTotal == null) ? 0.0 : regime[professordetalhe.CpfProfessor.ToString()].CargaTotal) ,2);
                                professordetalhe.QtdHorasDs = (double)Math.Round((decimal)((regime[professordetalhe.CpfProfessor.ToString()].QtdHorasDs == null) ? 0.00 : regime[professordetalhe.CpfProfessor.ToString()].QtdHorasDs) ,2);
                                professordetalhe.QtdHorasFs = (double)Math.Round((decimal)((regime[professordetalhe.CpfProfessor.ToString()].QtdHorasFs == null) ? 0.00 : regime[professordetalhe.CpfProfessor.ToString()].QtdHorasFs) ,2);                                
                                }
                                else
                                {
                                    professordetalhe.regime = "CHZ/AFASTADO";
                                    professordetalhe.CargaTotal = 0;
                                    professordetalhe.QtdHorasDs = 0;
                                    professordetalhe.QtdHorasFs = 0;
                                 }


                                ListaProfessorDetalhe.Add(professordetalhe);
                        }

                       
                        
                        return Ok(ListaProfessorDetalhe.Select(x=> new {x.CpfProfessor
                                                                       , x.NomProfessor
                                                                      ,x.titulacao
                                                                      , x.regime
                                                                      , x.QtdHorasDs
                                                                      , x.QtdHorasFs}));
                        
                }
                catch (System.Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }
                // Termino da pesquisa detalhe professor
                      
        }

        /* termino da busca dos professores */

        
        /* busca todos os professores  */
        
        [AllowAnonymous]
        [HttpGet("PesquisaProfessor")]
        public async Task<IActionResult> PesquisaProfessor()
        {
                 List<ProfessorMatricula> matricula;
                 Dictionary<string, DateTime> ListaAdmissao = new Dictionary<string, DateTime>();
                 //Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
                 var mat =  MatriculaContext.ProfessorMatricula.ToListAsync();

                try
                {
                    
                      // pegar os contextos professor e regime
                      var ListaProfessores = Professores.getProfessores(Profcontext).ToListAsync();

                      var regime  = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor);
                      //var ListaRegime = regime.Keys.ToList();
                      List<ProfessorDetalhe> ListaProfessorDetalhe = new List<ProfessorDetalhe>();

                      // buscar admissao no contexto matricula
 
                    Task task1 = Task.Factory.StartNew (
                    () => 
                    {
                       Dictionary<string, DateTime> ListaAdmissao1 = new Dictionary<string, DateTime>();
                      //dic = RegContext.ProfessorRegime.ToDictionary(x => x.CpfProfessor.ToString());
                    }
                    );

                    Task.WaitAll(task1);

                    matricula = await mat;

                      
                     // var matricula = MatriculaContext.ProfessorMatricula.ToDictionary(x => Convert.ToInt64(x.cpfProfessor));
                      
                      //List<ProfessorMatricula> matricula = new List<ProfessorMatricula>();
                        
                        foreach (var professor in await ListaProfessores)
                        {
                                ProfessorDetalhe professordetalhe = new ProfessorDetalhe();

                                //cpf/nomeprofessor//titulacao//regime
                                professordetalhe.CpfProfessor = professor.CpfProfessor.ToString();
                                professordetalhe.NomProfessor = professor.NomProfessor;
                                professordetalhe.titulacao = professor.Titulacao;
                                
                                if (regime.ContainsKey(professordetalhe.CpfProfessor))
                                {
                                professordetalhe.regime = regime[professordetalhe.CpfProfessor.ToString()].Regime;
                                }
                                else
                                {
                                    professordetalhe.regime = "CHZ/AFASTADO";
                                }
                                //inicio
                                
                                //if (matricula.Where(x => x.cpfProfessor.ToString() == professor.CpfProfessor).Count() > 0)
                                
                                if (matricula.Where(x => x.cpfProfessor.ToString() == professor.CpfProfessor).Count() > 0)
                                {
                                    //professordetalhe.dtAdmissao = matricula[professordetalhe.CpfProfessor].dtAdmissao;
                                    
                                    DateTime? _data = matricula.Where(p => p.cpfProfessor.ToString() == professor.CpfProfessor).Min(d => d.dtAdmissao);
                                    
                                    professor.dtAdmissao = (_data != null) ? _data.Value.ToString("MM/dd/yyyy") : null;

                                }
                                
                                
                                // termino

                                ListaProfessorDetalhe.Add(professordetalhe);
                        }
                        
                        return Ok(ListaProfessorDetalhe.Select(x=> new {x.CpfProfessor
                                                                       , x.NomProfessor
                                                                      , x.dtAdmissao
                                                                       , x.titulacao
                                                                      , x.regime}));
                        
                }
                catch (System.Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }
                // Termino da pesquisa detalhe professor
                      
        }
        /* termino da busca dos professores */

        /* busca todos os professores no detalhe */
        //[AllowAnonymous]
        //[HttpGet("MostraProfessor")]
        public async Task<IEnumerable<dynamic>> BuscaDataAdmissao(List<string> _listaProf)
        {
                try
                {
                    
                      // pegar os contextos professor e regime TRAZER POR CPF
                      var ListaProfessores = Professores.getProfessores(Profcontext)
                                                           .Where(x => _listaProf
                                                                           .Contains(x.CpfProfessor))
                                                            .ToListAsync();

                      var regime  = RegContext.ProfessorRegime
                                                                 .Where(x => _listaProf
                                                                           .Contains(x.CpfProfessor))
                                                              .ToDictionary(x => x.CpfProfessor);
                      //var ListaRegime = regime.Keys.ToList();
                     var admissao = MatriculaContext.ProfessorMatricula
                                                                        .Where(x => _listaProf
                                                                                   .Contains(x.cpfProfessor.ToString()))
                                                                       .ToList();

                      List<ProfessorDetalhe> ListaProfessorDetalhe = new List<ProfessorDetalhe>();
                        
                        foreach (var professor in await ListaProfessores)
                        {
                                ProfessorDetalhe profdet = new ProfessorDetalhe();

                                //cpf/nomeprofessor//titulacao//regime
                                profdet.CpfProfessor = professor.CpfProfessor.ToString();
                                profdet.NomProfessor = professor.NomProfessor;
                                profdet.titulacao = professor.Titulacao;
                                
                                if (regime.ContainsKey(profdet.CpfProfessor))
                                {
                                profdet.regime = regime[profdet.CpfProfessor.ToString()].Regime;
                                }
                                else
                                {
                                    profdet.regime = "CHZ/AFASTADO";
                                 }
                                
                                
                                profdet.dtAdmissao = (admissao.Find(x => x.cpfProfessor.ToString() == profdet.CpfProfessor) == null) ?
                                                        Convert.ToDateTime("1900/01/01") :
                                                        admissao.Where(x => x.cpfProfessor.ToString() == profdet.CpfProfessor)
                                                        .Min(x => x.dtAdmissao);
                                


                                ListaProfessorDetalhe.Add(profdet);
                        }

                       
                        
                        return ListaProfessorDetalhe.Select(x=> new {x.CpfProfessor
                                                                      , x.NomProfessor
                                                                      ,x.titulacao
                                                                      ,x.dtAdmissao
                                                                      ,x.regime}).ToList();
                        
                }
                catch (System.Exception ex)
                {
                    return null;
                }
        // Termino da pesquisa detalhe professor
                      
        }


    public class ProfessorGap
            {
                public string Cpf { get; set; }
                public double Ds { get; set; }
                public double Fs { get; set; }
                public string Target { get; set; }
                
                public double Complemento { get; set; }

            }
            


    }
}
