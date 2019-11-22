using System;


namespace Censo.API.Model
{
    public class ProfessorExcel
    {
        public long cpfProfessor { get; set; }
        public string Titulacao { get; set; }
        public string Regime { get; set; }
        public long CodEmec { get; set; }
        public string NomeCurso { get; set; }
        public double Nota_Doutor { get; set; }
        public double Nota_Mestre { get; set; }
        public double Nota_Regime { get; set; }
        public double Nota_CorpoDocente {
            get {
            
            return   this.Nota_Doutor * 0.5 +
                    this.Nota_Mestre * 0.25 +
                    this.Nota_Regime * 0.25;
             } 
            
        }
        
    }
}