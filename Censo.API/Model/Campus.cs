using System;
using System.Collections.Generic;

namespace Censo.API.Model
{
    public partial class Campus
    {
        public decimal CodCampus { get; set; }
        public string NomCampus { get; set; }
        public string CodCampusSap { get; set; }
        public decimal? CodMunicipio { get; set; }
        public string EndCampus { get; set; }
        public string TxtComplEndereco { get; set; }
        public string CepCampus { get; set; }
    }
}
