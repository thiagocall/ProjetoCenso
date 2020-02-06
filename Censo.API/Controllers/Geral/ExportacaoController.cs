
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API;
using Censo.API.Model;
using Censo.API.Data;
using Censo.API.Data.Censo;
using Censo.API.Model.dados;
using Censo.API.Resultados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using OfficeOpenXml;

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

        public CensoContext CContext { get; }

        public TempProducaoContext ProducaoContext { get; set; }

        public ExportacaoController(ProfessorContext _context, ExportacaoContext  _expContext, ProfessorContext _profContext
                                    , RegimeContext _regContext, CensoContext _ccontext
                                    , TempProducaoContext _producaoContext)
        {
            this.Context = _context;
            this.ExpContext = _expContext;
            this.ExpContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;;
            this.Profcontext = _profContext;
            this.RegContext = _regContext;
            this.CContext = _ccontext;
            this.ProducaoContext = _producaoContext;
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

        [AllowAnonymous]
        //[HttpGet("Buscaies/{id}")]
        [HttpGet("Buscaies")]
        //public async Task<IActionResult> Get(long id)
        public async Task<IActionResult> Buscaies()
        {
            // pegar os contextos professor e regime
            var ListaProfessores = Professores.getProfessores(Profcontext).ToListAsync();
  
            
            // var query = await this.Context.CCenso.ToListAsync();
            var query = await this.CContext.ProfessorCursoCenso.ToListAsync();
            
            
                var results = query.Select(x => new
                                    {   CodIes = x.CodIes,
                                        CpfProfessor = x.CpfProfessor, 
                                        CodCampus = x.CodCampus,
                                        CodCurso = x.CodCurso,
                                        }).Where(c => c.CodCampus == c.CodCampus).ToList();


                return Ok(results);
            

            
            List<ProfessorDetalhe> ListaProfessorDetalhe = new List<ProfessorDetalhe>();
                        
                        foreach (var professor in await ListaProfessores)
                        {
                                ProfessorDetalhe professordetalhe = new ProfessorDetalhe();

                                //cpf/nomeprofessor//titulacao//Nome
                                professordetalhe.CpfProfessor = professor.CpfProfessor.ToString();
                                professordetalhe.NomProfessor = professor.NomProfessor;
                                professordetalhe.titulacao = professor.Titulacao;
                                professordetalhe.dtAdmissao = Convert.ToDateTime(professor.dtAdmissao);
                                professordetalhe.codSexo = professor.CodSexo;
                                professordetalhe.Nomraca = professor.NomRaca;
                                professordetalhe.NomMae = professor.NomMae;
                                professordetalhe.NacioProfessor = professor.NacionalidadeProfessor;

                                ListaProfessorDetalhe.Add(professordetalhe);
                        }

                       return Ok(ListaProfessorDetalhe.Select(x=> new {x.CpfProfessor
                                                                       , x.NomProfessor
                                                                      , x.titulacao
                                                                      , x.dtAdmissao
                                                                      , x.codSexo
                                                                      , x.Nomraca
                                                                      , x.NomMae
                                                                      , x.NacioProfessor
                                                                      //, x.Pais
                                                                      //, x.UF
                                                                      //, x.Municipio
                                                                      }));
    

        }

        // INICIO 
       [AllowAnonymous]
        [HttpGet("Geracao/Excel/{id}")]
        public async Task<IActionResult> Geracao(long id) {

             try
            {
                var resultadoOTM = await this.ProducaoContext.TbResultado
                        .FirstOrDefaultAsync(r => r.Id == id);
                
                // Tratando dados para Excel

                var parametros =  new List<ParametrosCenso>();
                var resultados = new List<Resultado>();

                var ListaEmecIES = await this.ExpContext.CursoEmecIes.ToListAsync();
                var ListaCurso = await this.CContext.CursoCenso.ToListAsync();
                
                //Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
                Dictionary<string, Professor> dic = new Dictionary<string, Professor>();

                var professores = JsonConvert.DeserializeObject<List<CursoProfessor>>(resultadoOTM.Professores);

                //inicio
                var XCursoEmecIes = ListaEmecIES;

                Dictionary<long?, string> DicEmecIes = new Dictionary<long?, string>();

                XCursoEmecIes.ForEach( p =>
                {
                    if (!DicEmecIes.TryGetValue(p.CodCursoEmec, out string cr)) {
                        
                        DicEmecIes.Add(p.CodCursoEmec, Convert.ToString(p.CodIes));
                    }
                     
                });
                //fim

                var cursoCenso = ListaCurso;

                Dictionary<long?, string> EmecIes = new Dictionary<long?, string>();

                cursoCenso.ForEach( p =>
                {
                    if (!EmecIes.TryGetValue(p.CodEmec, out string cr)) {
                        
                        EmecIes.Add(p.CodEmec, Convert.ToString(p.CodIes));
                    }
                     
                });

                // Dicionario CursoEmec
                Dictionary<long?, string> CursoEmec = new Dictionary<long?, string>();

                cursoCenso.ForEach( p =>
                {
                    if (!CursoEmec.TryGetValue(p.CodEmec, out string cr)) {
                        
                        CursoEmec.Add(p.CodEmec, p.NomCursoCenso);
                    }
                     
                });

                List<ProfessorGeracao> listaProfessor = new List<ProfessorGeracao>();
                //String.Join(";", listaProfessor);

                var dicProfessor = Profcontext.Professores.ToDictionary(x => x.CpfProfessor.ToString());


                foreach (var item in professores)
                {
                    item.Professores.ForEach( (p) =>
                            {
                                      
                               if (dicProfessor.ContainsKey(p.cpfProfessor.ToString()))
                                {
                                    var prof =  dicProfessor[p.cpfProfessor.ToString()];
                                   // NomeCompleto = dic[p.cpfProfessor].CpfProfessor

                               listaProfessor.Add( new ProfessorGeracao { 
                                   cpfProfessor = p.cpfProfessor,
                                   Codies = EmecIes[item.CodEmec],             
                                   CodEmec = item.CodEmec,    
                                   NomeCompleto =  prof.NomProfessor,
                                   Dtnascimento = prof.DtNascimentoProfessor.ToString(),
                                   NomRaca = prof.NomRaca,
                                   NomMae = prof.NomMae,
                                   NacioProfessor = prof.NacionalidadeProfessor,
                                   Pais = prof.NomPais,
                                   UF = prof.UfNascimento,
                                   Municipio = prof.MunicipioNascimento,
                                   Escolaridade = prof.Escolaridade,
                                   Titulacao = p.Titulacao,
                                   NomeCurso = CursoEmec[item.CodEmec],
                                   
                               });
                               }  ;
                            });
                }

                // inicio FOR EACH PARA SEPARA POR IES
                
                Dictionary<string, ProfessorGeracao> DicProfessor2 = new Dictionary<string, ProfessorGeracao>();

                  foreach (var item in professores)
                {
                    item.Professores.ForEach( (p) =>
                            {
                                var varcpf = p.cpfProfessor;
                                var varcpf2 = p.cpfProfessor;
                                if (p.cpfProfessor.ToString() == "4243979782")
                                {
                                    varcpf2 = p.cpfProfessor;
                                }
                                    
                                // Se não existe o professor , Criacao de um dicionario novo para incluir os professores
                               if (!DicProfessor2.ContainsKey(p.cpfProfessor.ToString()))   
                                {
                                   //ProfessorGeracao prof = new ProfessorGeracao();   // instanciei um classe professorgeracao
                                   ProfessorGeracao prof = listaProfessor.Find(x => x.cpfProfessor == p.cpfProfessor);   // instanciei um classe professorgeracao
                                   prof.cpfProfessor = p.cpfProfessor;  // adicionei um cpf que nao existia dentro da classe
                                   IES ies = new IES();                 // instanciei uma IES dentro professorgeracao
                                   ies.codies = EmecIes[item.CodEmec];  // adicinei uma IES nova
                                   CursoProf curso = new CursoProf();   // instanciei um novo curso
                                   curso.codcursoEmec = item.CodEmec;   // adicinei um codemec no curso
                                   curso.nomcursoEmec = cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso; // procurei o curso
                                   ies.Cursos.Add(curso);               // adicionei o curso seguindo HIERARQUIA CURSO / IES / PROF - OBJETO
                                   prof.Listaies.Add(ies);              // adicinei uma IES 
                                   DicProfessor2.Add(prof.cpfProfessor.ToString(), prof);  // CRIEI UM NOVO ITEM NO OBJETO 
                                }
                               else  // else(1) caso exista o professor
                                {
                                    //var prof =  DicProfessor2[p.cpfProfessor.ToString()];
                                    var prof = DicProfessor2[p.cpfProfessor.ToString()];
                                    //if (String.IsNullOrEmpty(EmecIes[item.CodEmec]))
                                    //if (!(prof.Listaies.Find(x => x.codies == EmecIes[item.CodEmec].ToString()) == null))
                                    if (!(prof.Listaies.Find(x => x.codies == DicEmecIes[item.CodEmec].ToString()) == null))
                                    //nova verificacao
                                    // if (EmecIes[item.CodEmec] != null)
                                    {
                                        //IES ies = prof.Listaies.Find(x => x.codies == EmecIes[item.CodEmec]); 
                                        //ies.codies = EmecIes[item.CodEmec];  //erro aqui
                                        IES ies = prof.Listaies.Find(x => x.codies == DicEmecIes[item.CodEmec]); 
                                        ies.codies = DicEmecIes[item.CodEmec]; //erro verificar
                                        CursoProf curso = new CursoProf();
                                        curso.codcursoEmec = item.CodEmec;
                                        curso.nomcursoEmec = cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso;
                                        ies.Cursos.Add(curso);
                                        //String.Join(";", cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso);
                                        //DicProfessor2.Add(p.cpfProfessor.ToString(), prof);  // CRIEI2 UM NOVO ITEM NO OBJETO 
                                        
                                    }
                                    else // else(2)
                                    {
                                        IES ies = new IES();
                                        ies.codies = EmecIes[item.CodEmec];
                                        //if (String.IsNullOrEmpty(item.CodEmec))  
                                        CursoProf curso = new CursoProf();
                                        curso.nomcursoEmec = cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso; // procurei o curso
                                        //if (cursoCenso.nomcursoEmec != null)
                                        if (String.IsNullOrEmpty(curso.nomcursoEmec))
                                        {
                                            ies.Cursos.Add(curso);
                                            prof.Listaies.Add(ies);
                                            CursoProf cursoprof = new CursoProf();
                                            cursoprof.nomcursoEmec = curso.nomcursoEmec;
                                        }
                                    }  // fim-else(2)

                                } // fim-else(1)
                                

                            }  // final do for each  
      
                    );   // final(p)
                }
                // FINAL DO FOR EACH

                // inicio da pesquisa e ordenação
                Dictionary<string, ProfessorGeracao> DicProfessor3 = new Dictionary<string, ProfessorGeracao>();
                foreach (var item in professores)
                {
                    item.Professores.ForEach( (p) =>
                    {
                    //ProfessorGeracao profi = listaProfessor.Find(x => x.cpfProfessor == p.cpfProfessor);   
                    var prof = DicProfessor3[p.cpfProfessor.ToString()];
                    IES ies = prof.Listaies.Find(x => x.codies == DicEmecIes[item.CodEmec]); 
                    if (ies.codies == EmecIes[item.CodEmec] && prof.cpfProfessor == p.cpfProfessor);
                        {
                            prof.NomeCurso  = String.Join(";", cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso);
                        }
                    
                    }
                    );
                }
                // termino da pesquisa e ordenacao

                //  Monta arquivo para Download em Excel

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream)) {                
                    var shProfessores = package.Workbook.Worksheets.Add("Professores");
                    shProfessores.Cells.LoadFromCollection(listaProfessor, true);
                    package.Save();            
                };  

                    stream.Position = 0;
                    var contentType = "application/octet-stream";
                    //var fileName = "Geracao-" + id + "-" + (DateTime.Now.ToShortDateString()) + ".xlsx";
                    var fileName = "Geracao-" + id + "-" + (DateTime.Now.ToString("dd-MM-yyyy")) + ".xlsx";

                return File(stream, contentType, fileName);
            }
            catch (System.Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }        

        }


        // TERMINO
    }
}
