using System;
using System.Collections.Generic;

namespace Censo.API.Model
{
    public partial class Professor
    {
        public decimal CpfProfessor { get; set; }
        public string NomProfessor { get; set; }
        public DateTime? DtNascimentoProfessor { get; set; }
        public string CodSexo { get; set; }
        public string NomRaca { get; set; }
        public string NomMae { get; set; }
        public string NacionalidadeProfessor { get; set; }
        public string NomPais { get; set; }
        public string UfNascimento { get; set; }
        public string MunicipioNascimento { get; set; }
        public string Escolaridade { get; set; }
        public string Titulacao { get; set; }
        public string DocenteDeficiencia { get; set; }
        public string Def1 { get; set; }
        public string Def2 { get; set; }
        public string Def3 { get; set; }
        public string Perfil { get; set; }
        public string DocenteSubstituto { get; set; }
        public string Ativo { get; set; }
        public string Pesquisa { get; set; }
    }
}
