using System.Collections.Generic;
using Censo.API.Model.Censo;

namespace Censo.API.Model
{
    public class CursoProfessor
    {
        public long CodEmec { get; set; }
        public int CodArea { get; set; }
        public List<ProfessorEmec> Professores{ get; set; }
        public double Nota_CPC_Iso { get; set; }
        public double Nota_CPC_Geral { get; set; }
        public double Nota_Doutor { get; set; }
        public double Nota_Mestre { get; set; }
        public double Nota_Regime { get; set; }
        public double Nota_Infra { get; set; }
        public double Nota_OP { get; set; }
        public double Nota_AF { get; set; }
        public double Nota_CE { get; set; }





    }
}