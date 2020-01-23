using System;
using System.Collections.Generic;

namespace Censo.API.Model.dados
{
    public partial class ProfessorAdicionado
    {
        public long CpfProfessor { get; set; }
        public string Regime { get; set; }
        public string Titulacao { get; set; }
        public double qtdHorasDs { get; set; }
        public double qtdHorasFs { get; set; }
    }
}
