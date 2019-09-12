using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Censo.API.ADODB;
using Microsoft.Extensions.Configuration;

namespace Censo.API.Campus
{
    public static class CampusProfessor
    {
        public static Dictionary<string, List<string>> dicCampusProfessor;

        public static Dictionary<string, List<string>> getCampusProfessor(IConfiguration _configuration)
        {
            if (dicCampusProfessor != null)
            {
                return dicCampusProfessor;
            }

                var conn = Connection.Get(_configuration);

                SqlCommand cmd = new SqlCommand("select DISTINCT CPF_PROFESSOR, COD_CAMPUS from Rel_Professor_Curso_Censo",conn);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();

                dicCampusProfessor = new Dictionary<string, List<string>>();

                while (reader.Read())
                {
                        if(dicCampusProfessor.ContainsKey(reader["CPF_PROFESSOR"].ToString()))
                        {
                            dicCampusProfessor[reader["CPF_PROFESSOR"].ToString()].Add(reader["COD_CAMPUS"].ToString());
                        }

                        else
                        {
                            var list = new List<string>();
                            list.Add(reader["COD_CAMPUS"].ToString());
                            dicCampusProfessor.Add(reader["CPF_PROFESSOR"].ToString(), list);
                        }

                }

                Connection.Close();

                return dicCampusProfessor;

            

        }



        
        
    }
}