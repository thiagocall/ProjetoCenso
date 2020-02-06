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
    public class PostarController : ControllerBase
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

        public PostarController(ProfessorIESContext Context
                                    , ProfessorContext ProfContext
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
        
        // POST api/values
     
        // EXPORTACAO CORPO DOCENTE EM PLANILHA EXCEL
        // AJUSTE PARA CRIAÇÃO DE DOWNLOAD EXCEL POR IES PASSANDO ID THIAGO CALDAS

                
        // INICIO PROFESSOR IES
       
       
        //Curso Professor Emec

        

        //Exporta em Excel

        //// regulatorio/Emec/ExcelCampus/
      
        /* fora de sede pelo cod_campus */
        
        /* busca professor dentro dos campus */
 
        [AllowAnonymous]
        [HttpPost("TestaGapProf")]
        public async Task<IActionResult> getCalculaGapProf(ProfessorGap[] ListaProfessorGap) 
        {


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
 
        /* termino da busca dos professores */
        
        /* busca todos os professores  */
        
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
