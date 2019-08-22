using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Censo.API.Model.Censo
{
    public partial class CursoCenso
    {
        public long? CodIes { get; set; }
        public long CodCampus { get; set; }
        public long CodCurso { get; set; }
        public string NumHabilitacao { get; set; }
        public long? CodEmec { get; set; }
        public string NomCursoCenso { get; set; }
        public long? QtdAlunos { get; set; }
        [NotMapped]
        public List<ProfessorCurso> ListaProfessores { get; set; }
    }
}
