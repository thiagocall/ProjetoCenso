using System.Data;
using System.Data.SqlClient;

namespace Censo.API.ADODB
{
    public static class Connection
    {

        private static SqlConnection conn;

        public static SqlConnection Get()
        {
            
            string strConn = "Data Source=db-alteryx.database.windows.net;Database=db-alteryx;User Id=db-admin;Password=8v&Kmu8b;";
            conn = new SqlConnection(strConn);
            conn.Open();
            return conn;

        }

        public static void Close(){

            conn.Close();

        }

        



        
    }
}