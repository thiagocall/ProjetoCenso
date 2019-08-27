using System.Collections.Generic;
using Censo.API.Model.Censo;

namespace Censo.API.Model
{
    public class CursoProfessor
    {
        public long CodEmec { get; set; }
        public Dictionary<long, ProfessorEmec> Professores{ get; set; }


    }
}