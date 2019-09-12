using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Censo.API.ADODB;
using Microsoft.Extensions.Configuration;

namespace Censo.API.Model.Censo
{
    public static class PrevisaoEmec
    {

        private static List<CursoPrevisao> listaPrevisao;

        public static List<CursoPrevisao> getPrevisao(IConfiguration _configuration)
        {
            CursoPrevisao cp;

            if (listaPrevisao != null)
            {
                return listaPrevisao;
            }

            var conn = Connection.Get(_configuration);

            SqlCommand cmd = new SqlCommand("select * from Rel_Evolucao_CPC",conn);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            listaPrevisao = new List<CursoPrevisao>();

            while (reader.Read())
            {
                
                cp = new CursoPrevisao();
                cp.Ano = (long?)reader["ANO_ENADE"];
                cp.CodArea = Convert.IsDBNull(reader["COD_AREA"]) ? null : (long?)reader["COD_AREA"];
                cp.Min_Mestre = (double?)reader["MIN_PERC_MESTRE"];
                cp.Max_Mestre = (double?)reader["MAX_PERC_MESTRE"];

                cp.Min_Doutor = Convert.IsDBNull(reader["MIN_PERC_DOUTOR"]) ? null : (double?)reader["MIN_PERC_DOUTOR"];
                cp.Max_Doutor = Convert.IsDBNull(reader["MAX_PERC_DOUTOR"]) ? null : (double?)reader["MAX_PERC_DOUTOR"];

                cp.Min_Regime = Convert.IsDBNull(reader["MIN_PERC_REGIME"]) ? null : (double?)reader["MIN_PERC_REGIME"];
                cp.Max_Regime = Convert.IsDBNull(reader["MAX_PERC_REGIME"]) ? null : (double?)reader["MAX_PERC_REGIME"];

                cp.Avg_Infra = Convert.IsDBNull(reader["AVG_INFRA"]) ? null : (double?)reader["AVG_INFRA"];
                cp.Avg_OP =  Convert.IsDBNull(reader["AVG_NOTA_OP"]) ? null : (double?)reader["AVG_NOTA_OP"];
                cp.Avg_CE =  Convert.IsDBNull(reader["AVG_CE"]) ? null : (double?)reader["AVG_CE"];
                cp.Avg_AF = Convert.IsDBNull(reader["AVG_NOTA_AF"]) ? null : (double?)reader["AVG_NOTA_AF"];

                cp.Min_CE = Convert.IsDBNull(reader["MIN_NOTA_CE"]) ? null : (double?)reader["MIN_NOTA_CE"];
                cp.Max_CE = Convert.IsDBNull(reader["MAX_NOTA_CE"]) ? null : (double?)reader["MAX_NOTA_CE"];

                cp.Min_FG = Convert.IsDBNull(reader["MIN_NOTA_FG"]) ? null : (double?)reader["MIN_NOTA_FG"];
                cp.Max_FG = Convert.IsDBNull(reader["MAX_NOTA_FG"]) ? null : (double?)reader["MAX_NOTA_FG"];   

                listaPrevisao.Add(cp);

            }

            Connection.Close();

            return listaPrevisao;

        }
    }
    public class CursoPrevisao
    {
        public long? CodArea { get; set; }
        public long? Ano { get; set; }
        public double? Min_Mestre { get; set; }
        public double? Max_Mestre { get; set; }
        public double? Min_Doutor { get; set; }
        public double? Max_Doutor { get; set; }
        public double? Min_Regime { get; set; }
        public double? Max_Regime { get; set; }
        public double? Avg_Infra { get; set; }
        public double? Avg_CE { get; set; }
        public double? Avg_OP { get; set; }
        public double? Avg_AF { get; set; }
        public double? Min_CE { get; set; }
        public double? Max_CE { get; set; }
        public double? Min_FG { get; set; }
        public double? Max_FG { get; set; }
        //public double? CE_FG_w { get; set; }

    }




}