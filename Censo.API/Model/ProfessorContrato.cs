using System;
using System.Collections.Generic;

namespace Censo.API.Model
{
    public partial class ProfessorContrato
    {
        public string NomInstituicao { get; set; }
        public string NomRegiao { get; set; }
        public long CpfProfessor { get; set; }
        public string NomProfessor { get; set; }
        public string NumMatricula { get; set; }
        public string SglUf { get; set; }
    }
}
