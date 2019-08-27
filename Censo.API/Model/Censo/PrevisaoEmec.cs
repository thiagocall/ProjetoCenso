using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Censo.API.ADODB;

namespace Censo.API.Model.Censo
{
    public static class PrevisaoEmec
    {

        private static List<CursoPrevisao> listaPrevisao;

        public static List<CursoPrevisao> getPrevisao()
        {
            CursoPrevisao cp;

            if (listaPrevisao != null)
            {
                return listaPrevisao;
            }

            var conn = Connection.Get();

            SqlCommand cmd = new SqlCommand("select * from Rel_Evolucao_CPC",conn);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            listaPrevisao = new List<CursoPrevisao>();

            while (reader.Read())
            {
                cp = new CursoPrevisao();
                cp.Ano = (long?)reader["ANO_ENADE"];
                cp.NomArea = reader["NOM_AREA"].ToString();
                cp.CodArea = (long?)reader["COD_AREA"];
                cp.Min_Mestre = (double?)reader["MIN_PERC_MESTRE"];
                cp.Max_Mestre = (double?)reader["MAX_PERC_MESTRE"];

                cp.Min_Doutor = (double?)reader["MIN_PERC_DOUTOR"];
                cp.Max_Doutor = (double?)reader["MAX_PERC_DOUTOR"];

                cp.Min_Regime = (double?)reader["MIN_PERC_REGIME"];
                cp.Max_Regime = (double?)reader["MAX_PERC_REGIME"];

                cp.Avg_Infra = (double?)reader["AVG_INFRA"];
                cp.Avg_OP = (double?)reader["AVG_NOTA_OP"];
                cp.Avg_AF = (double?)reader["AVG_NOTA_AF"];

                cp.Min_CE = (double?)reader["MIN_NOTA_CE"];
                cp.Max_CE = (double?)reader["MAX_NOTA_CE"];

                cp.Min_FG = (double?)reader["MIN_NOTA_FG"];
                cp.Max_FG = (double?)reader["MAX_NOTA_FG"];   

                listaPrevisao.Add(cp);

            }

            Connection.Close();

            return listaPrevisao;

        }

    public class CursoPrevisao
    {
        public long? CodArea { get; set; }
        public string NomArea { get; set; }
        public long? Ano { get; set; }
        public double? Min_Mestre { get; set; }
        public double? Max_Mestre { get; set; }
        public double? Min_Doutor { get; set; }
        public double? Max_Doutor { get; set; }
        public double? Min_Regime { get; set; }
        public double? Max_Regime { get; set; }
        public double? Avg_Infra { get; set; }
        public double? Avg_OP { get; set; }
        public double? Avg_AF { get; set; }
        public double? Min_CE { get; set; }
        public double? Max_CE { get; set; }
        public double? Min_FG { get; set; }
        public double? Max_FG { get; set; }
        //public double? CE_FG_w { get; set; }

    }








}