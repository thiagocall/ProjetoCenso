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

        private ProfessorCurso professorCurso;

        private CursoCenso curso;

        private List<ProfessorCurso> ListaprofessorCurso;
        public dynamic OtimizaCurso(Dictionary<long?, PrevisaoSKU> _dicPrevisao,
                                        List<ProfessorCursoEmec> _ListaProfessorEmec,
                                        List<CursoProfessor> _listaProfessor,
                                        List<CursoEnquadramento> _listaCursoEnquadramento)
        {

            ListaprofessorCurso = new List<ProfessorCurso>();

                // Monta Relação Professor Curso ####################

                foreach (var prof in _listaProfessor) {
                    
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
                                if (!professorCurso.strCursos.Contains(prof.CodEmec.ToString())) {

                                    professorCurso.strCursos.Add(prof.CodEmec.ToString());

                                }
                                

                            }

                        }

                    }

                    // Reduzir professores com Carga zerada não doutores



                    var primanota = CalculaNotaCursos(_ListaProfessorEmec, _dicPrevisao, _listaProfessor);


                    _listaProfessor.ForEach(
                            (p) => {
                                    p.Professores.RemoveAll(x => x.Titulacao != "DOUTOR");
                            }
                    );


                    var segnota = CalculaNotaCursos(_ListaProfessorEmec, _dicPrevisao, _listaProfessor);



            return new {primanota, segnota};

        }

        public dynamic CalculaNotaCursos(List<ProfessorCursoEmec> _listaProfessorEmec,
                                         Dictionary<long?, PrevisaoSKU> _listaPrevisaoSKU,
                                         List<CursoProfessor> _listaCursoProfessor
                                            ) 
            {

                var query = _listaProfessorEmec;

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
                                    .Select( x => new{ x.CodEmec, 
                                                x.Nota_Mestre,
                                                x.Nota_Doutor,
                                                x.Nota_Regime,
                                                Mestres = x.Professores
                                                        .Where(p => p.Titulacao == "MESTRE" || p.Titulacao == "DOUTOR" )
                                                        .Count(),
                                                QtdProfessores = x.Professores.Count(),
                                                doutores = x.Professores
                                                        .Where(p => p.Titulacao == "DOUTOR").Count(),
                                                x.CodArea
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





    }


}