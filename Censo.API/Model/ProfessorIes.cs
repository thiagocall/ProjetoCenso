using System;
using System.Collections.Generic;

namespace Censo.API.Model
{
    public partial class ProfessorIes
    {
        public string CodProfessor { get; set; }
        public string NumMatricula { get; set; }
        public long? CpfProfessor { get; set; }
        public string NomRegiao { get; set; }
        public long CodInstituicao { get; set; }
        public string NomInstituicao { get; set; }
    }
}
