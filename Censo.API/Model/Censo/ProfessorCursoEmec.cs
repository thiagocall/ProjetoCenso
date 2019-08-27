using System;
using System.Collections.Generic;

namespace Censo.API.Model.Censo
{
    public partial class ProfessorCursoEmec
    {
        public long CodCampus { get; set; }
        public long CodCurso { get; set; }
        public long? CodIes { get; set; }
        public long NumHabilitacao { get; set; }
        public long CodEmec { get; set; }
        public string NomCursoCenso { get; set; }
        public long CpfProfessor { get; set; }
        public long? QtdAlunos { get; set; }
        public string Titulacao { get; set; }
        public string IndAtivo { get; set; }
        public string Regime { get; set; }

    }
}
