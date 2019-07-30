using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Censo.API.CargaHoraria
{
    public class CargaDS
    {
        

        public Dictionary<string, double> getCargaDS()
        {

            Dictionary<string, double> kvCargaDs = new Dictionary<string, double>();

            string strConn = "Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;";

            using (SqlConnection con = new SqlConnection(strConn))
            {

                SqlCommand cmd = new SqlCommand("select * from Rel_MATRICULA_CARGA_DS",con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    kvCargaDs.Add(reader["NUM_MATRICULA"].ToString(), Convert.ToDouble(reader["Qtd_Horas"]));
                }

                return kvCargaDs;



            }


           

        }


    }


    public class CargaFS
    {
        
    }

}