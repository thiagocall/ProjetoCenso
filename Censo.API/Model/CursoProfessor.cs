using System.Collections.Generic;

namespace Censo.API.Model
{
    public class CursoProfessor
    {
        public long CodEmec { get; set; }
        public List<long> Professores{ get; set; }


    }
}