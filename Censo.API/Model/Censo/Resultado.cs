namespace Censo.API.Model.Censo
{
    public class Resultado
    {
        public long CodEmec { get; set; }
        public double Nota_Mestre { get; set; }
        public double Nota_Doutor { get; set; }
        public double Nota_Regime { get; set; }
        public int Mestres { get; set; }
        public int QtdProfessores { get; set; }
        public int Doutores { get; set; }
        public int CodArea { get; set; }


    }
}