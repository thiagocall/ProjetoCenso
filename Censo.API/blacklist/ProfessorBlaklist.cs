using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Censo.API.ADODB;
using Microsoft.Extensions.Configuration;

namespace Censo.API.blacklist
{
    public class ProfessorBlaklist
    {
        private static Dictionary<string, ProfessorBlackList> DicProfessorBlackList;
        public static Dictionary<string, ProfessorBlackList> GetBlackList(IConfiguration _configuration)
        {
            if (DicProfessorBlackList == null)
            {
                SqlConnection Conexao = Connection.Get(_configuration);
                SqlCommand cmd = new SqlCommand("Select * from Rel_Professor_BlackList", Conexao);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                ProfessorBlackList prof;
                DicProfessorBlackList = new Dictionary<string, ProfessorBlackList>();
                while (reader.Read())
                {

                    if (!DicProfessorBlackList.ContainsKey(reader["CPF_PROFESSOR"].ToString()))
                    {
                        prof = new ProfessorBlackList();
                        prof.ListaCodEmec = new List<long>();
                        prof.CpfProfessor = (long)reader["CPF_PROFESSOR"];
                        prof.ListaCodEmec.Add((long)reader["COD_EMEC"]);
                        DicProfessorBlackList.Add(prof.CpfProfessor.ToString(), prof);
                    }
                    else
                    {
                        prof = DicProfessorBlackList[reader["CPF_PROFESSOR"].ToString()];
                        prof.ListaCodEmec.Add((long)reader["COD_EMEC"]);

                    }

                }

                return DicProfessorBlackList;

            }
            return DicProfessorBlackList;

        }
    }
    
    public class ProfessorBlackList{

        public long CpfProfessor { get; set; }
        public List<long> ListaCodEmec { get; set; }
    }
}