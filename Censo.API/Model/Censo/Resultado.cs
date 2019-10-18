namespace Censo.API.Model.Censo
{
    public class Resultado
    {
        public long CodEmec { get; set; }
        public double Nota_Mestre { get; set; }
        public double Nota_Doutor { get; set; }
        public double Nota_Regime { get; set; }
        public double? Nota_CorpoDocente {
            get {
                return this.Nota_Doutor * 0.5 + this.Nota_Mestre * 0.25 + this.Nota_Regime * 0.25;
            }
            }
        public int Mestres { get; set; }
        public int QtdProfessores { get; set; }
        public int Doutores { get; set; }
        public int CodArea { get; set; }


    }
}