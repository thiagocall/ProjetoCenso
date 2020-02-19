using System;

namespace Censo.API.Model.Censo
{
    public class ProfessorEmec : ICloneable
    {
        public long cpfProfessor { get; set; }
        public string Regime { get; set; }
        public string Titulacao { get; set; }
        public string Ativo { get; set; }
        public string Pais { get; set; }
        public string UF { get; set; }
        public string Municipio { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}