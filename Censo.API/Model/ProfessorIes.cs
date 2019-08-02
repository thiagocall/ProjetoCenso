using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Censo.API.CargaHoraria;

namespace Censo.API.Model
{
    public partial class ProfessorIes
    {

        public ProfessorIes(long? cpfProfessor)
        {
            //this.NumMatricula = numMatricula;
            this.CpfProfessor = cpfProfessor;
            cargaDS = this._getCargaDs();
            cargaFS = this._getCargaFs();

        }
        public string CodProfessor { get; set; }
        public string NumMatricula { get; set; }
        public long? CpfProfessor { get; set; }
        public string NomRegiao { get; set; }
        public long CodInstituicao { get; set; }
        public string NomInstituicao { get; set; }
        public string NomProfessor {get;set;}
        public string ativo { get; set; }
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
