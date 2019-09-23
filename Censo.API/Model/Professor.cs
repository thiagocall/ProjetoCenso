using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Censo.API.CargaHoraria;

namespace Censo.API.Model
{
    public partial class Professor
    {

        public string CpfProfessor { get; set; }
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

        [NotMapped]
        public double? cargaDS{get;set;}
        [NotMapped]
        public double? cargaFS{ get; set; }

        [NotMapped]
        public string regime { get; set; }

        double? _getCargaFs()
        {
            return CargaProfessor.getCargaFS().Where(c => c.Key == this.CpfProfessor.ToString()).Sum(x => x.Value);
        }  
        double? _getCargaDs()
        {
            return CargaProfessor.getCargaDS().Where(c => c.Key == this.CpfProfessor.ToString()).Sum(x => x.Value);
        } 

    }
}
