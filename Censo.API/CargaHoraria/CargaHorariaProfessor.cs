using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
//using Microsoft.Extensions.Configuration;

namespace Censo.API.CargaHoraria
{
    public static class CargaProfessor
    {
        public static Dictionary<string, double> dicCargaDs; 
        public static Dictionary<string, double> dicCargaFs;


        public static Dictionary<string, double> getCargaDS()
        {
            if (dicCargaDs != null)
            {
                return dicCargaDs;
            }

            //Dictionary<string, double> dicCargaDs = new Dictionary<string, double>();

            string strConn = "Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;";

            using (SqlConnection conn = new SqlConnection(strConn))
            {

                SqlCommand cmd = new SqlCommand("select * from Rel_MATRICULA_CARGA_DS",conn);
                cmd.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dicCargaDs.Add(reader["NUM_MATRICULA"].ToString(), Convert.ToDouble(reader["Qtd_Horas"]));
                }

                return dicCargaDs;

            }


        }


         public static Dictionary<string, double> getCargaFS()
        {
            if (dicCargaFs != null)
            {
                return dicCargaFs;
            }

            string strConn = "Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;";

            using (SqlConnection conn = new SqlConnection(strConn))
            {

                SqlCommand cmd = new SqlCommand("select * from Rel_MATRICULA_CARGA_EX_DS",conn);
                cmd.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dicCargaFs.Add(reader["NUM_MATRICULA"].ToString(), Convert.ToDouble(reader["Qtd_Horas"]));
                }

                return dicCargaFs;

            }

           

        }


    }


    

}