using System;
using System.Collections.Generic;

namespace Censo.API.Model
{
    public partial class ProfessorRegime
    {
        public string CpfProfessor { get; set; }
        public double? QtdHorasDs { get; set; }
        public double? QtdHorasFs { get; set; }
        public double? CargaTotal { get; set; }
        public string Regime { get; set; }
    }
}
