using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using Censo.API.Data.Censo;
using Microsoft.Extensions.Configuration;
using Censo.API.blacklist;

namespace Censo.API.Resultados
{
    public class Otimizacao: IOtimizacao
    {

        TempProducaoContext ResultContext;
        CensoContext Context;

        IConfiguration Configuration;

        CargaContext CargaContext;
        ProfessorAddContext ProfessorAddContext;

        public Otimizacao ( TempProducaoContext _resultContext,
                            CensoContext _context,
                            CargaContext _cargaContext,
                            ProfessorAddContext _ProfessorAddContext,
                            IConfiguration _configuration)
        {
            this.ResultContext = _resultContext;
            this.Context = _context;
            this.CargaContext = _cargaContext;
            this.ProfessorAddContext = _ProfessorAddContext;
            this.Configuration = _configuration;
        }

        private ProfessorCurso professorCurso;
        // private CursoCenso curso;
        private List<ProfessorCurso> ListaprofessorCurso;
        public List<Resultado> OtimizaCurso(Dictionary<long?, PrevisaoSKU> _dicPrevisao,
                                        List<ProfessorCursoEmec> _ListaProfessorEmec,
                                        List<CursoProfessor> _listaProfessor,
                                        List<CursoEnquadramento> _listaCursoEnquadramento,
                                        ParametrosCenso _parametros)
        {

            var cargaDS = this.CargaContext.CargaDS
                                .ToDictionary(x => x.CpfProfessor);
            var cargaFS = this.CargaContext.CargaFS
                                .ToDictionary(x => x.CpfProfessor);

            ListaprofessorCurso = new List<ProfessorCurso>();

            var TaskEnade = Task.Run( () => {

                return this.Context.CursoCenso.ToList();

            });

                var ListaProfessorAdd = this.ProfessorAddContext.Professor.ToList();

                

                    // ############## Alavanca Curso não Enade ############
                    // ####################################################

                    Task.WaitAll(TaskEnade);
                    
                    var CursoEnade = TaskEnade.Result;
                    var CursoNaoEnade = CursoEnade.Where(x => x.IndEnade.Contains('N')).Select(c => c.CodEmec.ToString()).Distinct().ToList();
                    var CursoSimEnade = CursoEnade.Where(x => x.IndEnade.Contains('S')).Select(c => c.CodEmec.ToString()).Distinct().ToList();


                      foreach(var item in _listaProfessor)
                      {
                            
                           item.Professores.RemoveAll(pe =>
                                   RemoveProfessor(_listaProfessor, item, _dicPrevisao, pe, "S") && 
                                                    _listaProfessor.Where(cp => cp.Professores
                                                                            .Where(p => p.cpfProfessor == pe.cpfProfessor).Count() > 0)
                                                                            .Select(cp => cp.CodEmec)
                                                                            .ToList()
                                                                            .Exists(x => CursoNaoEnade.Contains(x.ToString()) &&
                                                                        CursoSimEnade.Contains(item.CodEmec.ToString()))
                                                    );
                                                    
                                };
                    

                    // ######################## Alavanca Colaborador ######################## //

                           foreach(var item in _listaProfessor)
                      {
                            
                                item.Professores.RemoveAll(pe =>  (RemoveProfessor(_listaProfessor, item, _dicPrevisao, pe) &&
                                                        pe.Regime == "HORISTA" &&
                                                        ((cargaDS.TryGetValue(pe.cpfProfessor.ToString(), out var ds) ? ds.QtdHoras : 0) +
                                                         (cargaFS.TryGetValue(pe.cpfProfessor.ToString(), out var fs) ? fs.QtdHoras : 0)) < 8 
                                             ));
                            
                        };

                         // ######################## Alavanca Excluído Ofensor ######################## //


                        foreach(var item in _listaProfessor)
                            {
                                    
                                        item.Professores.RemoveAll(pe =>  (RemoveProfessorExcluido(_listaProfessor, item, _dicPrevisao, pe) &&
                                                                pe.Ativo == "NÃO" 
                                                    ));
                                    
                                };

                        


                        // ######################## Limpeza força bruta ######################## //

                         foreach(var item in _listaProfessor)
                        {
                                 
                                item.Professores.RemoveAll(pe =>  (RemoveProfessor(_listaProfessor, item, _dicPrevisao, pe, "N") &&
                                                                (CalculaNota(item, _dicPrevisao, pe.Regime, pe.Titulacao) ?? 0) < 1.945
                                             ));
                            
                         };


                         // ##################### Black List ############################### //

                        // ProfessorBlaklist

                        foreach(var item in _listaProfessor)
                        {
                            
                                item.Professores.RemoveAll(pe => 
                                                    (RemoveProfessorBlackList(_listaProfessor, item, _dicPrevisao, pe) 
                                             ));
                            
                         };

                        // #####################   Alavancas  ########################## //
                        // Remove professor Cenário 2 - Hoper

                        /*

                            HashSet<long> CursoExcluir =  new HashSet<long>()
                                {
                                 

                            };

                            foreach (var item in _listaProfessor)
                            {

                                if (CursoExcluir.Contains(item.CodEmec))
                                {
                                    int removido= 0;
                                    double meta = Math.Floor(item.Professores.Count() * 0.3);
                                    int qtdAnterior = item.Professores.Count();
                                    
                                    
                                    // item.Professores.RemoveAll(pe => 
                                    
                                    //     RemoveProfessor30Perc(_listaProfessor, item, _dicPrevisao, pe, meta, qtdAnterior)     
                                    //                 );


                                     foreach (var pe in item.Professores)
                                    {   
                                           if (RemoveProfessor30Perc(_listaProfessor, item, _dicPrevisao, pe, meta, qtdAnterior) &&
                                            meta > removido)
                                           {
                                               pe.ind_remover = "S";
                                               removido++;
                                           } 
                                    }

                                    item.Professores.RemoveAll(pe => pe.ind_remover == "S");

                                }

                            }
                            */


                            // ##################### Professor Adicionado ############################### //


                        try
                        {

                        ProfessorEmec professorEmec;
                        foreach(var item in ListaProfessorAdd)
                        {

                                CursoProfessor curso; 

                                if (_listaProfessor.Where(x => x.CodEmec == item.CodEmec).Count() < 1 )
                                {
                                    CursoProfessor Cr = new CursoProfessor();
                                    Cr.CodEmec = item.CodEmec;
                                    Cr.CodArea = 9999;
                                    curso = Cr;
                                    curso.Professores = new List<ProfessorEmec>();
                                    _listaProfessor.Add(curso);

                                }

                                    curso =  _listaProfessor.First(x => x.CodEmec == item.CodEmec);
                                    var prof =  _ListaProfessorEmec.Where(x => x.CpfProfessor == item.CpfProfessor);
                                    if(prof.Count() != 0) {
                                        
                                        var profi = prof.First();
                                        professorEmec = new ProfessorEmec();
                                        professorEmec.cpfProfessor = profi.CpfProfessor;
                                        professorEmec.Regime = profi.Regime;
                                        professorEmec.Titulacao = profi.Titulacao;
                                        professorEmec.Ativo = profi.IndAtivo;

                                        curso.Professores.Add(professorEmec);
                                    }

                                    else {
                                        // Criando professor que não está na curso professor
                                        
                                        professorEmec = new ProfessorEmec();
                                        professorEmec.cpfProfessor = item.CpfProfessor;
                                        professorEmec.Regime = "TEMPO INTEGRAL";
                                        professorEmec.Titulacao = "DOUTOR";
                                        professorEmec.Ativo = "SIM";
                                        curso.Professores.Add(professorEmec);

                                    } 

                                    prof = null;                                   

                            curso = null;

                         };
                            
                        }
                        catch (System.Exception ex)
                        {
                                      
                            
                        }
                        


                        var final = CalculaNotaCursos(_dicPrevisao, _listaProfessor, CursoSimEnade);

                        return final;

        }

        public List<Resultado> CalculaNotaCursos( Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, List<CursoProfessor> _listaCursoProfessor, List<string> _listaEnade =  null) 
            {

                
                var ListaPrevisaoSKU = _listaPrevisaoSKU;

                // ######## Calcula Nota Prévia dos Cursos ###########

                foreach (var item in _listaCursoProfessor)
                {   
                    double qtdProf = item.Professores
                            .Where(x => x.ind_remover == null)
                            .Count();
                    double qtdD = item.Professores.Where(x => x.Titulacao == "DOUTOR" && x.ind_remover == null)
                            .Count();
                    double qtdM = item.Professores.Where(x => (x.Titulacao == "MESTRE" | x.Titulacao == "DOUTOR") && x.ind_remover == null)
                            .Count();
                    double qtdR = item.Professores.Where(x => (x.Regime == "TEMPO INTEGRAL" | x.Regime == "TEMPO PARCIAL")  && x.ind_remover == null)
                            .Count();

                    double perc_D = qtdD / qtdProf;
                    double perc_M = qtdM / qtdProf;
                    double perc_R = qtdR / qtdProf;
                   

                    if(0 == 0)
                    {
                        
                        var area = item.CodArea;
                        //##### Previsão Doutor
                        
                        if (ListaPrevisaoSKU.ContainsKey(area))
                        {
                            var prev = ListaPrevisaoSKU[area];

                            var prev_minM = prev.P_Min_Mestre == 1 ? 0.90 : prev.P_Min_Mestre;
                            var prev_maxM= prev.P_Max_Mestre;

                            var prev_minD = prev.P_Min_Doutor == 1 ? 0.90 : prev.P_Min_Doutor;
                            var prev_maxD= prev.P_Max_Doutor;

                            var prev_minR = prev.P_Min_Regime == 1 ? 0.90 : prev.P_Min_Regime;
                            var prev_maxR= prev.P_Max_Regime;

                            double notaM =  (N_Escala(prev_minM, prev_maxM, perc_M)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minM, prev_maxM, perc_M));
                            double notaD =  (N_Escala(prev_minD, prev_maxD, perc_D)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minD, prev_maxD, perc_D));
                            double notaR =  (N_Escala(prev_minR, prev_maxR, perc_R)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minR, prev_maxR, perc_R));
                            
                            item.Nota_Mestre = notaM;
                            item.Nota_Doutor = notaD;
                            item.Nota_Regime = notaR;
                            
                        }

                    }
                
                }

                    
                    var result = _listaCursoProfessor
                                .Select( x => new Resultado {
                                            CodEmec = x.CodEmec, 
                                            Nota_Mestre =  x.Nota_Mestre,
                                            Nota_Doutor = x.Nota_Doutor,
                                            Nota_Regime = x.Nota_Regime,
                                            Mestres = x.Professores
                                                    .Where(p => p.Titulacao == "MESTRE" || p.Titulacao == "DOUTOR" )
                                                    .Count(),
                                            QtdProfessores = x.Professores.Count(),
                                            Doutores = x.Professores
                                                    .Where(p => p.Titulacao == "DOUTOR").Count(),
                                            CodArea = x.CodArea,

                                                    indEnade = (_listaEnade != null) ? _listaEnade.Contains(x.CodEmec.ToString()) ? "S" : "N" : "N"
                                            
                                            
                                            })
                                        .ToList();

                return result;
                

            }


            public dynamic MontaResultadoFinal(List<Resultado> _resultado) {

                int QtdCursos_E = 0;
                int Nota1a2_E = 0;
                int Nota3_E = 0;
                int Nota4a5_E = 0;
                int qtdD_1a2_E = 0;
                int qtdD_3_E = 0;
                int qtdD_4a5_E = 0;
                int qtdM_1a2_E = 0;
                int qtdM_3_E = 0;
                int qtdM_4a5_E = 0;
                int qtdR_1a2_E = 0;
                int qtdR_3_E = 0;
                int qtdR_4a5_E = 0;


                int QtdCursos = 0;
                int Nota1a2 = 0;
                int Nota3 = 0;
                int Nota4a5 = 0;
                int qtdD_1a2 = 0;
                int qtdD_3 = 0;
                int qtdD_4a5 = 0;
                int qtdM_1a2 = 0;
                int qtdM_3 = 0;
                int qtdM_4a5 = 0;
                int qtdR_1a2 = 0;
                int qtdR_3 = 0;
                int qtdR_4a5 = 0;


                // string indEnade = "";

                List<Resultado> Resultado_Enade = _resultado.Where(r => r.indEnade == "S").ToList();
                List<Resultado> Resultado_NaoEnade = _resultado;


                QtdCursos_E = Resultado_Enade.Count();
                // Índices de Nota Geral
                Nota1a2_E = Resultado_Enade.Where(x => x.Nota_CorpoDocente <= 2).Count(); // Insatisfatório
                Nota3_E = Resultado_Enade.Where(x => x.Nota_CorpoDocente == 3).Count(); // Satisfatório
                Nota4a5_E = Resultado_Enade.Where(x => x.Nota_CorpoDocente >=4).Count(); // Excelência

                // Índices de Titulação
                qtdD_1a2_E = Resultado_Enade.Where(x => x.Nota_Doutor < 1.945).Count();
                qtdD_3_E = Resultado_Enade.Where(x => x.Nota_Doutor >= 1.945 && x.Nota_Doutor < 2.945).Count();
                qtdD_4a5_E = Resultado_Enade.Where(x => x.Nota_Doutor >= 2.945).Count();
                qtdM_1a2_E = Resultado_Enade.Where(x => x.Nota_Mestre < 1.945).Count();
                qtdM_3_E = Resultado_Enade.Where(x => x.Nota_Mestre >= 1.945 && x.Nota_Mestre < 2.945).Count();
                qtdM_4a5_E = Resultado_Enade.Where(x => x.Nota_Mestre >= 2.945).Count();
                qtdR_1a2_E = Resultado_Enade.Where(x => x.Nota_Regime < 1.945).Count();
                qtdR_3_E = Resultado_Enade.Where(x => x.Nota_Regime >= 1.945 && x.Nota_Regime < 2.945).Count();
                qtdR_4a5_E = Resultado_Enade.Where(x => x.Nota_Regime >= 2.945).Count();


                QtdCursos = Resultado_NaoEnade.Count();
                // Índices de Nota Geral
                Nota1a2 = Resultado_NaoEnade.Where(x => x.Nota_CorpoDocente <= 2).Count(); // Insatisfatório
                Nota3 = Resultado_NaoEnade.Where(x => x.Nota_CorpoDocente == 3).Count(); // Satisfatório
                Nota4a5 = Resultado_NaoEnade.Where(x => x.Nota_CorpoDocente >=4).Count(); // Excelência

                // Índices de Titulação
                qtdD_1a2 = Resultado_NaoEnade.Where(x => x.Nota_Doutor < 1.945).Count();
                qtdD_3 = Resultado_NaoEnade.Where(x => x.Nota_Doutor >= 1.945 && x.Nota_Doutor < 2.945).Count();
                qtdD_4a5 = Resultado_NaoEnade.Where(x => x.Nota_Doutor >= 2.945).Count();
                qtdM_1a2 = Resultado_NaoEnade.Where(x => x.Nota_Mestre < 1.945).Count();
                qtdM_3 = Resultado_NaoEnade.Where(x => x.Nota_Mestre >= 1.945 && x.Nota_Mestre < 2.945).Count();
                qtdM_4a5 = Resultado_NaoEnade.Where(x => x.Nota_Mestre >= 2.945).Count();
                qtdR_1a2 = Resultado_NaoEnade.Where(x => x.Nota_Regime < 1.945).Count();
                qtdR_3 = Resultado_NaoEnade.Where(x => x.Nota_Regime >= 1.945 && x.Nota_Regime < 2.945).Count();
                qtdR_4a5 = Resultado_NaoEnade.Where(x => x.Nota_Regime >= 2.945).Count();


                return new {
                    QtdCursos_E,
                    Nota1a2_E,
                    Nota3_E,
                    Nota4a5_E,
                    qtdD_1a2_E,
                    qtdD_3_E,
                    qtdD_4a5_E,
                    qtdM_1a2_E,
                    qtdM_3_E,
                    qtdM_4a5_E,
                    qtdR_1a2_E,
                    qtdR_3_E,
                    qtdR_4a5_E,

                    QtdCursos,
                    Nota1a2,
                    Nota3,
                    Nota4a5,
                    qtdD_1a2,
                    qtdD_3,
                    qtdD_4a5,
                    qtdM_1a2,
                    qtdM_3,
                    qtdM_4a5,
                    qtdR_1a2,
                    qtdR_3,
                    qtdR_4a5
                        };
            }


        public double? N_Escala(double? lim_min, double? lim_max, double? percent)
        {

            double? n;

            try
            {

                n = (percent - lim_min) /  (lim_max - lim_min)   * 5;
                if (n < 0)
                {
                    return 0;
                }
                else if(n > 5)
                {
                    return 5;

                }

                else
                {
                    double? n1 = (n == null) ? (double?)0 : (double?)Math.Round((decimal)n, 4);
                    return  n1;

                }

            }
            catch (System.Exception)
            {
                
                  if(lim_max == lim_min){
                    return 5;
                } else {
                    return 0;
                }
            }

        }

        public double? CalculaNota(CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, string _regime, string _titulacao, int _indMovimento = 0)
        {
            
                double qtdProf = _cursoProfessor.Professores
                        .Where(x => x.ind_remover == null)
                        .Count();
                double qtdD = _cursoProfessor.Professores
                        .Where(x => x.Titulacao == "DOUTOR" && x.ind_remover == null)
                        .Count();
                double qtdM = _cursoProfessor.Professores
                        .Where(x => (x.Titulacao == "MESTRE" | x.Titulacao == "DOUTOR" ) && x.ind_remover == null)
                        .Count();
                double qtdR = _cursoProfessor.Professores
                        .Where(x => (x.Regime == "TEMPO INTEGRAL" | x.Regime == "TEMPO PARCIAL") && x.ind_remover == null)
                        .Count();
                
                qtdProf = qtdProf + _indMovimento;

                qtdD = (_titulacao == "DOUTOR") ? qtdD + _indMovimento : qtdD;
                qtdM = (_titulacao == "DOUTOR" | _titulacao == "MESTRE" ) ? qtdM + _indMovimento : qtdM;
                qtdR = (_regime == "TEMPO PARCIAL" | _regime == "TEMPO INTEGRAL") ? qtdR + _indMovimento : qtdR;


                double notaM = 0;
                double notaD = 0;
                double notaR = 0;

                double perc_D = qtdD / qtdProf;
                double perc_M = qtdM / qtdProf;
                double perc_R = qtdR / qtdProf;

                var area = _cursoProfessor.CodArea;
                    
                    if (_listaPrevisaoSKU.ContainsKey(area))
                    {
                        var prev = _listaPrevisaoSKU[area];

                        var prev_minM = prev.P_Min_Mestre;
                        var prev_maxM= prev.P_Max_Mestre;

                        var prev_minD = prev.P_Min_Doutor;
                        var prev_maxD= prev.P_Max_Doutor;

                        var prev_minR = prev.P_Min_Regime;
                        var prev_maxR= prev.P_Max_Regime;

                        var M = N_Escala(prev_minM, prev_maxM, perc_M);
                        var D = N_Escala(prev_minD, prev_maxD, perc_D);
                        var R = N_Escala(prev_minR, prev_maxR, perc_R);

                        notaM = (M == null) ? 0 : Convert.ToDouble(M);
                        notaD = (D == null) ? 0 : Convert.ToDouble(D);
                        notaR = (R == null) ? 0 : Convert.ToDouble(R);
                    }

            double resultado = notaM * 0.25 + notaR * 0.25 + notaD * 0.5;

            return (resultado);

        }

        public bool RemoveProfessor(List<CursoProfessor> _ListaCursoProfessor, CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, ProfessorEmec _prof, string _indNaoEnade = null)
        {

            var qtdCursos = _ListaCursoProfessor.Where(
                                            x => x.Professores.Where(
                                                        c => c.cpfProfessor == _prof.cpfProfessor).Count() > 0).Count();

            var notaAnt = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao);

            var notaNova = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao, -1);

            var qtdProf =  _cursoProfessor.Professores.Count();

            if (notaNova > notaAnt & (qtdCursos > 1 || _indNaoEnade == "S") & qtdProf > 2 )
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        public bool RemoveProfessorExcluido(List<CursoProfessor> _ListaCursoProfessor, CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, ProfessorEmec _prof)
        {

            var qtdCursos = _ListaCursoProfessor.Where(
                                            x => x.Professores.Where(
                                                        c => c.cpfProfessor == _prof.cpfProfessor).Count() > 0).Count();

            var notaAnt = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao);

            var notaNova = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao, -1);

            var qtdProf =  _cursoProfessor.Professores.Count();

            if (notaNova > notaAnt )
            {
                return true;
            }

            else
            {
                return false;
            }
 
        }


        public bool RemoveProfessorBlackList(List<CursoProfessor> _ListaCursoProfessor, CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, ProfessorEmec _prof)
        {
            
                var dic = ProfessorBlaklist.GetBlackList(this.Configuration);

                if (dic.ContainsKey(_prof.cpfProfessor.ToString()))
                {
                    if (dic[_prof.cpfProfessor.ToString()].ListaCodEmec.Contains(_cursoProfessor.CodEmec))
                    {
                        return true;

                    }
                }

                return false;

        } 

              public bool RemoveProfessor30Perc(List<CursoProfessor> _ListaCursoProfessor,
                                    CursoProfessor _cursoProfessor,
                                    Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU,
                                    ProfessorEmec _prof,
                                    double _meta,
                                    int _qtd_anterior)
        {


            var notaAnt = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao);

            var notaNova = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao, -1);

            var qtdProf =  _cursoProfessor.Professores.Count();

            if (notaNova > notaAnt && (_meta > _qtd_anterior - qtdProf))
            {
                return true;
            }

            else
            {
                return false;
            }

        }   






       public bool AddProfessor(List<CursoProfessor> _ListaCursoProfessor,
                                CursoProfessor _cursoProfessor,
                                Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU,
                                ProfessorEmec _prof,
                                string _indNaoEnade = null)
        {

            var qtdCursos = _ListaCursoProfessor.Where(
                                            x => x.Professores.Where(
                                                        c => c.cpfProfessor == _prof.cpfProfessor).Count() > 0).Count();

            var notaAnt = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao);

            var notaNova = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao, 1);

            // var qtdProf =  _cursoProfessor.Professores.Count();

            if (notaNova >= notaAnt)
            {
                return true;
            }

            else
            {
                return false;
            }

        }


        public void AddProfessor20p(List<CursoProfessor> _cursoProfessor, List<ProfessorCursoEmec20p> _listaProfessor20p, Dictionary<long?, PrevisaoSKU> _dicPrevisao, ParametrosCenso _param, List<string> _listaEnade)
        {

            // Controle de Uso
            Dictionary<long, int> listaUsoProfessor =  new Dictionary<long, int>();
            // Adiciona os professores 20% nos cursos

            Dictionary<string, string> dicEnade = new Dictionary<string, string>();

            dicEnade = _listaEnade.ToDictionary(x => x);
                
            foreach (var emec in _listaProfessor20p)
            {

                try
                {

                if(dicEnade.ContainsKey(emec.CodEmec.ToString())) 
                    {

                            var codEmec = emec.CodEmec;
                            var qtd = (_cursoProfessor.Find(x => x.CodEmec == codEmec) != null) ?
                                            _cursoProfessor
                                            .Find(x => x.CodEmec == codEmec)
                                                .Professores
                                                .Where(x => x.cpfProfessor == emec.CpfProfessor).Count() : 0;
                            
                            if (qtd < 1) {
                                
                                var curso = _cursoProfessor.Find(x => x.CodEmec == emec.CodEmec);
                                
                                if (curso != null)
                                {

                                    var _prof=  new ProfessorEmec {
                                            Ativo = emec.IndAtivo,
                                            cpfProfessor = emec.CpfProfessor,
                                            Regime = emec.Regime,
                                            Titulacao = emec.Titulacao
                                            };


                                    if (curso.Professores.Where(x => x.cpfProfessor == emec.CpfProfessor).Count() < 1  &
                                        AddProfessor(_cursoProfessor, curso, _dicPrevisao, _prof)
                                            & (listaUsoProfessor.TryGetValue(emec.CpfProfessor, out int dic) ? dic : 0) < _param.usoProfessor 
                                            & dic < _param.usoProfessorGeral)
                                    {

                                        curso.Professores.Add(_prof);

                                            // Adciona controle de adição de professores
                                            if (listaUsoProfessor.ContainsKey(emec.CpfProfessor)) {

                                                listaUsoProfessor[emec.CpfProfessor] += 1;


                                            } else {
                                                listaUsoProfessor.Add(emec.CpfProfessor, 1);
                                            }

                                    }
                                    
                                }
                            }
                        }
                }

            catch (System.Exception ex)
            {
                
            }
        }


        }



        
    }




}