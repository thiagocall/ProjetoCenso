using System;
using System.Collections.Generic;

namespace Censo.API.Model.Censo
{
    public partial class TbResultado
    {
        public long Id { get; set; }
        public string Resultado { get; set; }
        public string Parametro { get; set; }
        public string Resumo { get; set; }
        public string Professores { get; set; }
        public string TempoExecucao { get; set; }
        public int indOficial { get; set; }
        public string Observacao {get; set;}
    }
}
