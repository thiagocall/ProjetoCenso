using System;
using System.Collections.Generic;

namespace Censo.API.Model.Censo
{
    public partial class ProfessorCursoCenso
    {
        public long CpfProfessor { get; set; }
        public long? CodIes { get; set; }
        public long NumHabilitacao { get; set; }
        public long CodCampus { get; set; }
        public long CodCurso { get; set; }
        public long? QtdAlunos { get; set; }
    }
}
