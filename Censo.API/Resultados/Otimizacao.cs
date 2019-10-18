using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;

namespace Censo.API.Resultados
{
    public class Otimizacao: IOtimizacao
    {

        TempProducaoContext ResultContext;

        public Otimizacao (TempProducaoContext _resultContext)
        {
            this.ResultContext = _resultContext;
        }

        private ProfessorCurso professorCurso;

        private CursoCenso curso;

        private List<ProfessorCurso> ListaprofessorCurso;
        public List<Resultado> OtimizaCurso(Dictionary<long?, PrevisaoSKU> _dicPrevisao,
                                        List<ProfessorCursoEmec> _ListaProfessorEmec,
                                        List<CursoProfessor> _listaProfessor,
                                        List<CursoEnquadramento> _listaCursoEnquadramento,
                                        ParametrosCenso _parametros)
        {

            ListaprofessorCurso = new List<ProfessorCurso>();

                // Monta Relação Professor Curso ####################

                foreach (var prof in _listaProfessor)
                { 
                    foreach (var item in prof.Professores)
                    {
                        // Professor novo na lista
                        if (ListaprofessorCurso.Where(x => x.cpfProfessor == item.cpfProfessor.ToString()).Count() == 0)
                        {

                            professorCurso = new ProfessorCurso();
                            professorCurso.cpfProfessor = item.cpfProfessor.ToString();
                            professorCurso.strCursos.Add(prof.CodEmec.ToString());

                            ListaprofessorCurso.Add(professorCurso);
                            professorCurso = null;

                        }

                        //  Adicionando curso ao professor
                        else {

                            professorCurso = ListaprofessorCurso.FirstOrDefault(x => x.cpfProfessor == item.cpfProfessor.ToString());
                            if (!professorCurso.strCursos.Contains(prof.CodEmec.ToString()))
                            {
                                professorCurso.strCursos.Add(prof.CodEmec.ToString());
                            }       
                        }
                    }
                }

                    // Reduzir professores com Carga zerada não doutores


                    // var primanota = CalculaNotaCursos(_dicPrevisao, _listaProfessor);
                    

                    // Remove Professores com Carga Horária Zerada não Doutores | Alavanca Professor Ofensor

                    _listaProfessor.ForEach(
                            p => {
                                   // p.Professores.RemoveAll(x => x.Titulacao != "DOUTOR" & x.Regime == "CHZ/AFASTADO");

                                   p.Professores.RemoveAll(x => RemoveProfessor(_listaProfessor, p, _dicPrevisao, x));
                            }
                    );

                    var final = CalculaNotaCursos(_dicPrevisao, _listaProfessor);


                    return final;

        }

        public List<Resultado> CalculaNotaCursos( Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, List<CursoProfessor> _listaCursoProfessor) 
            {
                // var query = _listaProfessorEmec;

                var ListaPrevisaoSKU = _listaPrevisaoSKU;

                // int area;

                // ######## Calcula Nota Prévia dos Cursos ###########

                foreach (var item in _listaCursoProfessor)
                {   
                    double qtdProf = item.Professores
                            .Count();
                    double qtdD = item.Professores.Where(x => x.Titulacao == "DOUTOR")
                            .Count();
                    double qtdM = item.Professores.Where(x => x.Titulacao == "MESTRE" | x.Titulacao == "DOUTOR")
                            .Count();
                    double qtdR = item.Professores.Where(x => x.Regime == "TEMPO INTEGRAL" | x.Regime == "TEMPO PARCIAL")
                            .Count();

                    double perc_D = qtdD / qtdProf;
                    double perc_M = qtdM / qtdProf;
                    double perc_R = qtdR / qtdProf;
                    ////e.CodCampus, e.CodCurso, e.NumHabilitacao

                    if(0 == 0)
                    {
                        
                        var area = item.CodArea;
                        //##### Previsão Doutor
                        
                        if (ListaPrevisaoSKU.ContainsKey(area))
                        {
                        var prev = ListaPrevisaoSKU[area];

                        var prev_minM = prev.P_Min_Mestre;
                        var prev_maxM= prev.P_Max_Mestre;

                        var prev_minD = prev.P_Min_Doutor;
                        var prev_maxD= prev.P_Max_Doutor;

                        var prev_minR = prev.P_Min_Regime;
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

                    //var result = cursoProfessor.Select(x => x.Nota_Mestre).ToList();
                    var result = _listaCursoProfessor
                                    .Select( x => new Resultado { CodEmec = x.CodEmec, 
                                                Nota_Mestre =  x.Nota_Mestre,
                                                Nota_Doutor = x.Nota_Doutor,
                                                Nota_Regime = x.Nota_Regime,
                                                Mestres = x.Professores
                                                        .Where(p => p.Titulacao == "MESTRE" || p.Titulacao == "DOUTOR" )
                                                        .Count(),
                                                QtdProfessores = x.Professores.Count(),
                                                Doutores = x.Professores
                                                        .Where(p => p.Titulacao == "DOUTOR").Count(),
                                                CodArea = x.CodArea
                                                // Professores = x.Professores,
                                                })
                                            .ToList();

                return result;
                

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
                    return n;

                }

            }
            catch (System.Exception)
            {
                
                return 0;
            }

        }

        public double? CalculaNota(CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, string _regime, string _titulacao, int _indMovimento = 0)
        {
            
                double qtdProf = _cursoProfessor.Professores
                        .Count();
                double qtdD = _cursoProfessor.Professores
                        .Where(x => x.Titulacao == "DOUTOR")
                        .Count();
                double qtdM = _cursoProfessor.Professores
                        .Where(x => x.Titulacao == "MESTRE" | x.Titulacao == "DOUTOR")
                        .Count();
                double qtdR = _cursoProfessor.Professores
                        .Where(x => x.Regime == "TEMPO INTEGRAL" | x.Regime == "TEMPO PARCIAL")
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
                ////e.CodCampus, e.CodCurso, e.NumHabilitacao

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

                        notaM =  (N_Escala(prev_minM, prev_maxM, perc_M)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minM, prev_maxM, perc_M));
                        notaD =  (N_Escala(prev_minD, prev_maxD, perc_D)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minD, prev_maxD, perc_D));
                        notaR =  (N_Escala(prev_minR, prev_maxR, perc_R)) == null ? 0 : Convert.ToDouble(N_Escala(prev_minR, prev_maxR, perc_R));
                    }

            double resultado = notaM * 0.25 + notaR * 0.25 + notaD * 0.5;

            return (resultado);


        }

        public bool RemoveProfessor(List<CursoProfessor> _ListaCursoProfessor, CursoProfessor _cursoProfessor, Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU, ProfessorEmec _prof )
        {



            var qtdCursos = _ListaCursoProfessor.Where(x => x.Professores.Where(c => c.cpfProfessor == _prof.cpfProfessor).Count() > 0).Count();

            var notaAnt = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao);

            var notaNova = CalculaNota(_cursoProfessor, _listaPrevisaoSKU, _prof.Regime, _prof.Titulacao, -1);

            var qtdProf =  _cursoProfessor.Professores.Count();

            if (notaNova >= notaAnt & qtdCursos > 2 & qtdProf > 2)
            {
                return true;
            }

            else
            {
                return false;
            }

        }


        public void AddProfessor(CursoProfessor _cursoProfessor)
        {
            throw new NotImplementedException();
        }
    }


}