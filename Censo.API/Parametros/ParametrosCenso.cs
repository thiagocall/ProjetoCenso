using System.Collections.Generic;

namespace Censo.API.Parametros
{
    public class ParametrosCenso
    {

            private List<string> ListaProfessores = new List<string>();
            public  bool DTP {get; set;}
            public  bool DTI {get; set;}
            public  bool DH {get; set;}
            public  bool MTI {get; set;}
            public  bool MTP {get; set;}
            public  bool MH {get; set;}
            public  bool ETI {get; set;}
            public  bool ETP {get; set;}
            public  bool EH {get; set;}
            public  double Perclimite {get; set;}
            public  bool otimiza20p {get; set;}
            public  double usoProfessor {get; set;}
            public  double usoProfessorGeral {get; set;}
            public  double PercReduProf {get; set;}
            public int Metodo { get; set; }
            public string Obs {get; set;}

            public List<string> MontaLista(){
                
                
                if (this.DTI) this.ListaProfessores.Add("DOUTOR_TEMPO INTEGRAL");
                if (this.DTP) this.ListaProfessores.Add("DOUTOR_TEMPO PARCIAL");
                if (this.DH) this.ListaProfessores.Add("DOUTOR_HORISTA");
                if (this.MTI) this.ListaProfessores.Add("MESTRE_TEMPO INTEGRAL");
                if (this.MTP) this.ListaProfessores.Add("MESTRE_TEMPO PARCIAL");
                if (this.MH) this.ListaProfessores.Add("MESTRE_HORISTA");
                if (this.ETI) this.ListaProfessores.Add("ESPECIALISTA_TEMPO INTEGRAL");
                if (this.ETP) this.ListaProfessores.Add("ESPECIALISTA_TEMPO PARCIAL");
                if (this.EH) this.ListaProfessores.Add("ESPECIALISTA_HORISTA");

                return this.ListaProfessores;

            }

    }
}