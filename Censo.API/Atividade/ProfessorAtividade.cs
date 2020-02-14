using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Censo.API.ADODB;
using Microsoft.Extensions.Configuration;

namespace Censo.API.Atividade
{
    public static class ProfessorAtividade
    {
        private static Dictionary<string, Professor> DicProfessor;
        public static Dictionary<string, Professor> GerarLista(IConfiguration _configuration)
        {
            if (DicProfessor == null)
            {
                SqlConnection Conexao = Connection.Get(_configuration);
                SqlCommand cmd = new SqlCommand("Select * from Rel_Professor_Atividade", Conexao);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                Professor prof;
                DicProfessor = new Dictionary<string, Professor>();
                while (reader.Read())
                {

                    if (!DicProfessor.ContainsKey(reader["CPF_PROFESSOR"].ToString()))
                    {
                        prof = new Professor();
                        prof.CpfProfessor = reader["CPF_PROFESSOR"].ToString();
                        prof.Atividades.Add(reader["ATIVIDADE"].ToString());
                        DicProfessor.Add(prof.CpfProfessor, prof);
                    }
                    else
                    {
                        prof = DicProfessor[reader["CPF_PROFESSOR"].ToString()];
                        prof.Atividades.Add(reader["ATIVIDADE"].ToString());

                    }

                }

                return DicProfessor;

            }
            return DicProfessor;

        }

    }

    public class Professor
    {
            public string CpfProfessor { get; set; }
            public List<string> Atividades = new List<string>(){
                "Curso de graduação presencial"
            };

            public List<string> getSorted() {
                
                this.Atividades.Sort();
                return this.Atividades;
            }
            
    }
}