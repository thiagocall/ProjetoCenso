
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
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using at = Censo.API.Atividade;

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
        public IConfiguration Configuration { get; set; }    

        public TempProducaoContext ProducaoContext { get; set; }

        public ExportacaoController(ProfessorContext _context, ExportacaoContext  _expContext, ProfessorContext _profContext
                                    , RegimeContext _regContext, CensoContext _ccontext
                                    , TempProducaoContext _producaoContext
                                    , IConfiguration _configuration)
        {
            this.Context = _context;
            this.ExpContext = _expContext;
            this.ExpContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;;
            this.Profcontext = _profContext;
            this.RegContext = _regContext;
            this.CContext = _ccontext;
            this.ProducaoContext = _producaoContext;
            this.Configuration = _configuration;
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

                return Ok("Incluído com sucesso.");
            }
            catch (Exception e) {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no processamento." + e.Message);
            }

            

        }

        [AllowAnonymous]
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


        //        return Ok(results);
            
        
            
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
        [HttpGet("Geracao/Excel")]
        public async Task<IActionResult> Geracao()
        {  //inicio requisicao

             try
            {   // inicio try catch
                var resultadoOTM = await this.ProducaoContext.TbResultado
                        .FirstOrDefaultAsync(r => r.indOficial == 1);
                
                // Tratando dados para Excel


                var dicProfessorAtividade = at.ProfessorAtividade.GerarLista(this.Configuration);


                var parametros =  new List<ParametrosCenso>();
                var resultados = new List<Resultado>();

                var ListaEmecIES = await this.ExpContext.CursoEmecIes.ToListAsync();
                var ListaCurso = await this.CContext.CursoCenso.ToListAsync();

                //Erro ocorre AQUI
                var ListaIesSiaEmec = await this.ExpContext.IesSiaEmec.ToListAsync();

                //List<object> Listatotalprof = new List<object>();
                List<ProfessorEscrita> Listatotalprof = new List<ProfessorEscrita>();
                
                //Dictionary<string, ProfessorRegime> dic = new Dictionary<string, ProfessorRegime>();
                Dictionary<string, Professor> dic = new Dictionary<string, Professor>();

                var professores = JsonConvert.DeserializeObject<List<CursoProfessor>>(resultadoOTM.Professores);

                //inicio
                var XCursoEmecIes = ListaEmecIES;

                Dictionary<long?, string> DicEmecIes = new Dictionary<long?, string>();

                XCursoEmecIes.ForEach( p =>
                {
                    if (!DicEmecIes.TryGetValue(p.CodCursoEmec, out string cr)) 
                    {
                        
                        DicEmecIes.Add(p.CodCursoEmec, Convert.ToString(p.CodIes));
                    }
                     
                });

                var cursoCenso = ListaCurso;

                Dictionary<long?, string> EmecIes = new Dictionary<long?, string>();

                cursoCenso.ForEach( p =>
                {
                    if (!EmecIes.TryGetValue(p.CodEmec, out string cr)) 
                    {
                        if (p.CodIes != null)
                        {
                            EmecIes.Add(p.CodEmec, Convert.ToString(p.CodIes));
                        }
                            
                    }
                     
                });

                // Dicionario CursoEmec
                Dictionary<long?, string> CursoEmec = new Dictionary<long?, string>();

                cursoCenso.ForEach( p =>
                {
                    if (!CursoEmec.TryGetValue(p.CodEmec, out string cr)) 
                    {
                        
                        CursoEmec.Add(p.CodEmec, p.NomCursoCenso);
                    }
                     
                });

                List<ProfessorGeracao> listaProfessor = new List<ProfessorGeracao>();

                // Dicionario do professor com seus dados pessoais
                var dicProfessor = Profcontext.Professores.ToDictionary(x => x.CpfProfessor.ToString());

                // Primeiro for each
                foreach (var item in professores)
                {  // inicio foreach
                    item.Professores.ForEach( (p) =>
                            {  // inicio foreach
                                      
                               if (dicProfessor.ContainsKey(p.cpfProfessor.ToString()))
                                {
                                    var prof =  dicProfessor[p.cpfProfessor.ToString()];

                                    listaProfessor.Add( new ProfessorGeracao 
                                    { 
                                        cpfProfessor = p.cpfProfessor,
                                        Codies = EmecIes[item.CodEmec],             
                                        CodEmec = item.CodEmec,    
                                        NomeCompleto =  prof.NomProfessor,
                                        Dtnascimento = prof.DtNascimentoProfessor.ToString(),
                                        NomSexo = prof.CodSexo,
                                        NomRaca = prof.NomRaca,
                                        NomMae = prof.NomMae,
                                        NacioProfessor = prof.NacionalidadeProfessor,
                                        Pais = prof.NomPais,
                                        UF = prof.UfNascimento,
                                        Municipio = prof.MunicipioNascimento,
                                        Escolaridade = prof.Escolaridade,
                                        DocentecomDeficiencia = prof.DocenteDeficiencia,
                                        def1 = prof.Def1,
                                        def2 = prof.Def2,
                                        def3 = prof.Def3,
                                        Situacaodocente = prof.DocenteDeficiencia,
                                        Perfil = prof.Perfil,
                                        Regime = p.Regime,
                                        DocenteSubstituto = prof.DocenteSubstituto,
                                        DocenteAtivo3112 = prof.Ativo,
                                        Titulacao = p.Titulacao,
                                        NomeCurso = CursoEmec[item.CodEmec],

                                    });  // termino do add listaprofessor
                               };  // termino do if
                            });   // termino do for each
                } // termino do for each

                // inicio FOR EACH PARA SEPARA POR IES = DicProfessor2 organiza por IES os professores
                Dictionary<string, ProfessorGeracao> DicProfessor2 = new Dictionary<string, ProfessorGeracao>();

                // Segundo foreach
                foreach (var item in professores)
                {
                    item.Professores.ForEach( (p) =>
                            {
                                var varcpf = p.cpfProfessor;
                                var varcpf2 = p.cpfProfessor;
                                /*
                                if (p.cpfProfessor.ToString() == "5809478409")
                                {
                                    varcpf2 = p.cpfProfessor;
                                } */

                                // Se não existe o professor , Criacao de um dicionario novo para incluir os professores
                               if (!DicProfessor2.ContainsKey(p.cpfProfessor.ToString()))   
                                {
                                   //ProfessorGeracao prof = new ProfessorGeracao();   // instanciei um classe professorgeracao
                                   ProfessorGeracao prof = listaProfessor.Find(x => x.cpfProfessor == p.cpfProfessor);   // instanciei um classe professorgeracao
                                   prof.cpfProfessor = p.cpfProfessor;  // adicionei um cpf que nao existia dentro da classe
                                   IES ies = new IES();                 // instanciei uma IES dentro professorgeracao
                                   ies.codies = EmecIes[item.CodEmec];  // adicinei uma IES nova
                                   //ies.Nomies = DicEmecIes[item.CodEmec].Nomies;
                                   ////ies.Nomies = ListaIesSiaEmec.Find(x => x.Cod_Ies.ToString() == ies.codies.ToString()).Nom_Ies;
                                   CursoProf curso = new CursoProf();   // instanciei um novo curso
                                   curso.codcursoEmec = item.CodEmec;   // adicinei um codemec no curso
                                   curso.nomcursoEmec = cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso; // procurei o curso
                                   ies.Cursos.Add(curso);               // adicionei o curso seguindo HIERARQUIA CURSO / IES / PROF - OBJETO
                                   prof.Listaies.Add(ies);              // adicinei uma IES 
                                   DicProfessor2.Add(prof.cpfProfessor.ToString(), prof);  // CRIEI UM NOVO ITEM NO OBJETO 
                                }
                               else  // else(1) caso exista o professor
                                {
                                    var prof = DicProfessor2[p.cpfProfessor.ToString()];
                                    //if (!(prof.Listaies.Find(x => x.codies == EmecIes[item.CodEmec].ToString()) == null))
                                    if (!(prof.Listaies.Find(x => x.codies == DicEmecIes[item.CodEmec]) == null))
                                    //nova verificacao
                                     {
                                        //IES ies = prof.Listaies.Find(x => x.codies == EmecIes[item.CodEmec]); 
                                        //ies.codies = EmecIes[item.CodEmec];  //erro aqui
                                        IES ies = prof.Listaies.Find(x => x.codies == DicEmecIes[item.CodEmec]); 
                                        //ies.Nomies = prof.Listaies.Find(x => x.codies == DicEmecIes[item.CodEmec]).Nomies; // novidade
                                        //ies.codies = DicEmecIes[item.CodEmec]; //erro verificar
                                        CursoProf curso = new CursoProf();
                                        curso.codcursoEmec = item.CodEmec;
                                        curso.nomcursoEmec = cursoCenso.Find(x => x.CodEmec == item.CodEmec).NomCursoCenso;
                                        ies.Cursos.Add(curso);
                                        //prof.Listaies.Add(ies);
                                        
                                    }
                                    else // else(2)
                                    {
                                        IES ies = new IES();
                                        ies.codies = EmecIes[item.CodEmec];
                                        ////ies.Nomies = ListaIesSiaEmec.Find(x => x.Cod_Ies.ToString() == ies.codies.ToString()).Nom_Ies;
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
                
                } // FINAL DO FOR EACH

                // NOVO NOVO NOVO
                // inicio da pesquisa e ordenação
                
                
                var listaprofselecionado = listaProfessor;

                ProfessorEscrita profesc;
                // teceiro foreach
                foreach (var pro in DicProfessor2.Values)
                {  // inicio foreach
                        profesc = new ProfessorEscrita();                        
                        
                        //profesc.cpfProfessor = pro.cpfProfessor;

                        /* if (pro.Listaies.Count() == 0)
                            {
                                pro.cpfProfessor = pro.cpfProfessor;
                            } */
                        foreach (var Umaies in pro.Listaies )
                            {
                            profesc.Codies = Umaies.codies;
                            var IesEmec = ListaIesSiaEmec.Find(x => x.Cod_Ies.ToString() == profesc.Codies);
                            profesc.Codies = IesEmec.Cod_Ies_Emec.ToString();
                            string Carregaies = (IesEmec.Nom_Ies) ?? "SEM IES";
                            if (Carregaies != null)
                            {
                                profesc.NomIes = Carregaies;
                            }
                            //profesc.NomIes =  ListaIesSiaEmec.Find(x => x.Cod_Ies.ToString() == profesc.Codies.ToString()).Nom_Ies;
                            profesc.NomeCompleto = pro.NomeCompleto;
                            profesc.cpfProfessor = pro.cpfProfessor;
                            profesc.NomeCompleto = pro.NomeCompleto;
                            profesc.Dtnascimento = System.DateTime.Parse(pro.Dtnascimento).ToString("dd/MM/yyyy");
                            profesc.NomSexo = pro.NomSexo; 
                            profesc.NomRaca = (pro.NomRaca == "Nao declarada" ? "Branca" : pro.NomRaca);
                            profesc.NomMae = pro.NomMae;
                            profesc.NacioProfessor = pro.NacioProfessor;
                            profesc.Pais = pro.Pais;
                            profesc.UF = pro.UF;
                            profesc.Municipio = pro.Municipio;
                            profesc.Escolaridade = pro.Escolaridade;
                            profesc.Posgraduacao = pro.Titulacao;
                            profesc.Docentecomdeficiencia = pro.DocentecomDeficiencia;
                            profesc.Def1 = pro.def1;
                            profesc.Def2 = pro.def2;
                            profesc.Def3 = pro.def3;
                            profesc.Situacaodocente = (pro.Situacaodocente == "NÃO" ? "Esteve em Exercicio" : pro.Situacaodocente);
                            profesc.Perfil = pro.Perfil;
                            profesc.Regime = pro.Regime;
                            profesc.DocenteSubstituto = pro.DocenteSubstituto;
                            profesc.DocenteAtivo3112 = pro.DocenteAtivo3112;
                            // Ajustado Thiago ///
                            profesc.primeiraatuacao = "Curso de graduação presencial";
                            if (dicProfessorAtividade.ContainsKey(profesc.cpfProfessor.ToString()))
                            {
                                var at = dicProfessorAtividade[profesc.cpfProfessor.ToString()];

                                profesc.segundaatuacao = at.Atividades.ToArray().ElementAtOrDefault(1);
                                profesc.terceiraatuacao =  at.Atividades.ToArray().ElementAtOrDefault(2);
                                profesc.quartaatuacao =  at.Atividades.ToArray().ElementAtOrDefault(3);
                            }
                            
                            profesc.Cursos1 = String.Join(";", Umaies.Cursos.Select(x => x.codcursonomecurso).ToList());

                            //Ajustado Thiago //
                            //profesc.Atividades = dicProfessorAtividade.TryGetValue(profesc.cpfProfessor.ToString(), out var p) ? String.Join(";", p.getSorted()) : "" ;
                            Listatotalprof.Add(profesc);
                            }
                            //);
                }  // termino for each
            // }  //final comentado                

                //  Monta arquivo para Download em Excel

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream)) 
                {                
                    var shProfessores = package.Workbook.Worksheets.Add("Professores");
                    
                    shProfessores.Cells.LoadFromCollection(Listatotalprof, true);


                    using (var range = shProfessores.Cells[1, 1, 1, 29]) 
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    }
                    
                    for (int i = 1;i < 29; i++)
                        {
                            //count ++;
                        shProfessores.Cells[1, i].Style.Font.Bold = true;
                        
                        }

                    
                    // int lin = 2;
                    int col = 29;
                        
                        for (int i = 2; i < 1000000; i++)
                        {

                            if (shProfessores.Cells[i, 1].Value == null)
                            {
                                break;
                            }

                                
                                  
                            //var cursos = ((string)shProfessores.Cells[i, col].Value).Split(";");

                            var cursos = new [] {"SEM CURSOS"};
                            if (shProfessores.Cells[i, col].Value != null)
                            {
                                cursos = ((string)shProfessores.Cells[i, col].Value).Split(";");
                                int vnum = 1;
                                int vcont = 1;
                                int vcol = 29;
                                foreach(string parte in cursos) 
                                        {
                                            if (vnum % 2 == 0)
                                            {
                                                shProfessores.Cells[1, vcol].Value = "CÓDIGO NO CURSO e-MEC";

                                                using (var range = shProfessores.Cells[1, 1, 1, vcol + 1]) 
                                                {
                                                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                    range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                                }

                                                for (int j = 1;j < vcol + 1; j++)
                                                    {
                                                        //count ++;
                                                    shProfessores.Cells[1, j].Style.Font.Bold = true;
                                                    
                                                    }

                                            }
                                            else
                                            {
                                                shProfessores.Cells[1, vcol].Value = (vcont) + "º" + " CURSO (NOME)";
                                                vcont = vcont + 1;
                                                using (var range = shProfessores.Cells[1, 1, 1, vcol + 1]) 
                                                {
                                                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                    range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                                }

                                                for (int j = 1;j < vcol + 1; j++)
                                                    {
                                                        //count ++;
                                                    shProfessores.Cells[1, j].Style.Font.Bold = true;
                                                    
                                                    }

                                            }
                                            vnum = vnum + 1;
                                            vcol = vcol + 1;
                                        }
                                
                                col = 29;

                                foreach (var item in cursos)
                                {
                                    shProfessores.Cells[i, col++].Value = item.ToString();
                                }
                                col = 29;
                            }
                            
                        }

                        // colocar aqui             
                      
              
                    
                    package.Save();            
                };  

                    stream.Position = 0;
                    var contentType = "application/octet-stream";
                     var fileName = "Geracao-" + resultadoOTM.Id + "-" + (DateTime.Now.ToString("dd-MM-yyyy")) + ".xlsx";

                return File(stream, contentType, fileName);
            } // termino try catch
            catch (System.Exception ex)
            {
                
                 return StatusCode(StatusCodes.Status500InternalServerError, "Erro na Consulta.");
            }        

        }    // termino da requisicao
        
        //}

                public class ProfessorEscrita 
            { 
                        
                        public string Codies { get; set; }
                        public string NomIes { get; set; }    
                        //public long CodEmec { get; set; }
                        public long cpfProfessor { get; set; }
                        public string NomeCompleto { get; set; }
                        public string Dtnascimento { get; set; }
                        public string NomSexo {get; set;}
                        public string NomRaca { get; set; }
                        public string NomMae { get; set; }
                        public string NacioProfessor { get; set; }
                        public string Pais { get; set; }
                        public string UF { get; set; }
                        public string Municipio { get; set; }
                        public string Escolaridade { get; set; }    
                        //public string NomeCurso { get; set; }    
                        public string Posgraduacao { get; set; } 
                        public string Docentecomdeficiencia { get; set; } 
                        public string Def1 {get; set;}
                        public string Def2 {get; set;}
                        public string Def3 {get; set;}
                        public string Situacaodocente {get; set;}
                        public string Perfil {get; set;}
                        public string Regime {get; set;}
                        public string DocenteSubstituto {get; set;}
                        public string DocenteAtivo3112 {get; set;}
                        public string primeiraatuacao { get; set; }
                        public string segundaatuacao { get; set; }
                        public string terceiraatuacao { get; set; }
                        public string quartaatuacao { get; set; }
                        public string Bolsapesquisa { get; set; }
                        public string Cursos1 { get; set; }
                        //public string Cursos2 { get; set; }
                        //public string Cursos3 { get; set; }
            }

        // TERMINO
    }
}
