using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Censo.API.ADODB
{
    public static class Connection
    {

        public static IConfiguration Configuration { get; }

        private static SqlConnection conn;

        public static SqlConnection Get()
        {
            string config = Configuration.GetConnectionString("DefaultConnection");
            string strConn = config;
            conn = new SqlConnection(strConn);
            conn.Open();
            return conn;

        }

        public static void Close(){

            conn.Close();

        }

        



        
    }
}