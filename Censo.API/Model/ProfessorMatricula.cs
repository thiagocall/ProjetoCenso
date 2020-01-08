using System;

namespace Censo.API.Model
{
    public class ProfessorMatricula
    {
        public string numMatricula { get; set; }
        public long cpfProfessor {get; set; }
        public string indTipoContrato { get; set; }
        public DateTime dtAdmissao { get; set; }
        public DateTime? dtDemissao { get; set; }
        public string codRegiao { get; set; }
        public string nomeRegiao { get; set; }
        

    }
}