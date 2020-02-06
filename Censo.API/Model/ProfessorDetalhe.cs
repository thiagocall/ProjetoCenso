using System;
using System.Collections.Generic;
using Censo.API.Model.Censo;

namespace Censo.API.Model
{
    public partial class ProfessorDetalhe
    {
        public string CpfProfessor { get; set; }
        public string NomProfessor { get; set; }
        public string titulacao { get; set; }
        public string regime { get; set; }
        public string codRegiao { get; set; }
        public string nomeRegiao { get; set; }
        public double? QtdHorasDs { get; set; }
        public double? QtdHorasFs { get; set; }
        public double? CargaTotal { get; set; }
        public DateTime? dtAdmissao { get; set; }
        public List<Curso> Cursos = new List<Curso>();

        public List<string> Regioes = new List<string>();
        
        public string codSexo { get; set; }
        public string Nomraca { get; set; }
        public string NomMae { get; set; }
        public string NacioProfessor  { get; set; }
    }

        public class Curso{
            public long codcurso { get; set; }
            public string nomcurso { get; set; }
            public string nomcampus { get; set; }
                    
            } 
}
