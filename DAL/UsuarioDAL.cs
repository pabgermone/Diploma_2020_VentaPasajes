using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Servicios;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;

namespace DAL
{
    public static class UsuarioDAL
    {
        public static UsuarioBE UsuarioBE { get; set; }

        private static List<UsuarioBE> CargarUsuarios(SqlDataReader pReader)
        {
            List<UsuarioBE> listaClientes = new List<UsuarioBE>();

            while (pReader.Read())
            {
                UsuarioBE usuarioBE = new UsuarioBE();

                usuarioBE.ID = pReader.GetInt32(pReader.GetOrdinal("usuario_id"));
                usuarioBE.Usuario = pReader.GetString(pReader.GetOrdinal("usuario"));
                usuarioBE.Password = pReader.GetString(pReader.GetOrdinal("password"));
                usuarioBE.Nombre = pReader.GetString(pReader.GetOrdinal("nombre"));
                usuarioBE.Apellido = pReader.GetString(pReader.GetOrdinal("apellido"));
                usuarioBE.Mail = pReader.GetString(pReader.GetOrdinal("mail"));
                usuarioBE.Rol = RolDAL.ObtenerRol(usuarioBE.ID);
                usuarioBE.DV = pReader.GetInt32(pReader.GetOrdinal("dv"));

                listaClientes.Add(usuarioBE);
            }

            pReader.Close();

            return listaClientes;
        }

        private static UsuarioBE InsertarID(UsuarioBE pUsuario)
        {
            string query = "select top 1 * from usuario order by usuario_id desc;";
            SqlDataReader sqlReader = DBManager.Obtener(query);

            while (sqlReader.Read())
            {
                pUsuario.ID = sqlReader.GetInt32(sqlReader.GetOrdinal("usurio_id"));
            }

            sqlReader.Close();

            return pUsuario;
        }

        public static List<UsuarioBE> ListarUsuarios()
        {
            string query = "select * from usuario;";

            SqlDataReader readerUsuario = DBManager.Obtener(query);

            return CargarUsuarios(readerUsuario);
        }

        public static UsuarioBE ObtenerUsuario(string pUsuario, string pPassword)
        {
            string query = "select * from usuario where usuario = '" + pUsuario + "' and password = '" + pPassword + "';";

            SqlDataReader readerUsuario = DBManager.Obtener(query);

            List<UsuarioBE> usuarios = CargarUsuarios(readerUsuario);

            if(usuarios.Count > 0)
            {
                return CargarUsuarios(readerUsuario)[0];
            }
            else
            {
                return null;
            }
        }

        public static void AltaUsuario(UsuarioBE pUsuario)
        {
            string query = "insert into usuario (usuario_id, usuario, password, nombre, apellido, mail, rol_id, dv) " +
                "values(" + pUsuario.ID + ", '" + pUsuario.Usuario + "', '" + pUsuario.Password + "', '" + pUsuario.Nombre + "', '" + pUsuario.Apellido + "', '" + pUsuario.Mail + "', " + pUsuario.Rol.ID + ", " + pUsuario.DV + ");";

            DBManager.GuardarCambios(query);
        }

        public static void ModificarUsuario(UsuarioBE pUsuario)
        {
            string query = "update usuario " +
                "set usuario = '" + pUsuario.Usuario +
                "', password = '" + pUsuario.Password +
                "', nombre = '" + pUsuario.Nombre +
                "', apellido = '" + pUsuario.Apellido +
                "', mail = '" + pUsuario.Mail +
                "', rol_id = " + pUsuario.Rol.ID +
                ", dv = " + pUsuario.DV +
                " where usuario_id = " + pUsuario.ID + ";";

            DBManager.GuardarCambios(query);
        }
    }
}
