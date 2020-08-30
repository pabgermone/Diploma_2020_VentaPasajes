using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Servicios
{
    public class DBManager
    {
        private static SqlConnection Connection;

        private static void Conectar()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.DataSource = "PABLO-PC\\SQLEXPRESS";
            connectionStringBuilder.InitialCatalog = "Diploma_2020_VentaPasajes";

            Connection = new SqlConnection(connectionStringBuilder.ConnectionString);
            Connection.Open();
        }

        public static SqlDataReader Obtener(string pQuery)
        {
            Conectar();

            SqlCommand command = new SqlCommand(pQuery, Connection);
            SqlDataReader sqlReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            return sqlReader;
        }

        public static void GuardarCambios(string pQuery)
        {
            Conectar();

            SqlCommand command = new SqlCommand(pQuery, Connection);
            command.ExecuteNonQuery();

            Connection.Close();
        }
    }
}
