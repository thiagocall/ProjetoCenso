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

                var nota = this.Nota_Doutor * 0.5 + this.Nota_Mestre * 0.25 + this.Nota_Regime * 0.25; 

                    if (nota < 0.945)
                    {
                        return 1;
                    } else if(nota < 1.945)
                    {
                        return 2;
                    } else if(nota < 2.945)
                    {
                        return 3;
                    } else if(nota < 3.945)
                    {
                        return 4;
                    } else {
                        return 5;
                    }
                
                }
            }
        public int Mestres { get; set; }
        public int QtdProfessores { get; set; }
        public int Doutores { get; set; }
        public int CodArea { get; set; }
        public string indEnade { get; set; }


    }
}