
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API;
using Censo.API.Model;
using Censo.API.Data;
using Censo.API.Model.dados;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Censo.API.Controllers.Geral
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ExportacaoController : ControllerBase
    {

        public ProfessorContext Context { get; set; }

        public ExportacaoContext ExpContext { get; set; }

        public ProfessorContext Profcontext;

        public RegimeContext RegContext;

        public ExportacaoController(ProfessorContext _context, ExportacaoContext  _expContext, ProfessorContext _profContext, RegimeContext _regContext)
        {
            this.Context = _context;
            this.ExpContext = _expContext;
            this.ExpContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;;
            this.Profcontext = _profContext;
            this.RegContext = _regContext;
        }
        

        [HttpGet]
        public async Task<IActionResult> Get()
        {
             try
            {
                var results = await Context.Professores.ToListAsync();
            
                return Ok(results);
                
            }
            catch (System.Exception)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados");
            }
            
        }

        // inicio
        /* busca todos os professores  */
        
        [AllowAnonymous]
        [HttpGet("  ")]
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
                                
                                }
                                else
                                {
                                    professordetalhe.regime = "CHZ/AFASTADO";
                                 }


                                ListaProfessorDetalhe.Add(professordetalhe);
                        }
                        return Ok(ListaProfessorDetalhe.Select(x=> new {x.CpfProfessor
                                                                       , x.NomProfessor
                                                                      ,x.titulacao
                                                                      , x.regime}));
                        
                }
                catch (System.Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro no Banco de Dados.");
                }
                // Termino da pesquisa detalhe professor

        // fim
        }

        [AllowAnonymous]

        [HttpPost("DevolveProf")]
        public ActionResult putDevolveProf(List<ProfessorAdicionado> ListaProfessorDevolve) 
        {
   
            try 
            {
                ListaProfessorDevolve.ForEach(x => this.ExpContext.Add(x));
                this.ExpContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, "Incluído com sucesso.");
            }
            catch (Exception e) {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no processamento." + e.Message);
            }

            

        }

    }
}
