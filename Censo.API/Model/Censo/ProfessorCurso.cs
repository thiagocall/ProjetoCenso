using System.Collections.Generic;

namespace Censo.API.Model.Censo
{

    public class ProfessorCurso
    {

        public ProfessorCurso()
        {
            this.strCursos = new List<string>();
            this.Cursos = new List<CursoCenso>();
        }
        public string cpfProfessor;

        public List<CursoCenso> Cursos;

        public List<string> strCursos;


    }
}