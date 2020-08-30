using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BE;
using Servicios;

namespace DAL
{
    public static class RolDAL
    {
        private static List<RolBE> CargarBE(SqlDataReader pReader)
        {
            List<RolBE> listaRoles = new List<RolBE>();

            while (pReader.Read())
            {
                RolBE rol = new RolBE();

                rol.ID = pReader.GetInt32(pReader.GetOrdinal("rol_id"));
                rol.Nombre = pReader.GetString(pReader.GetOrdinal("nombre"));
                rol.DV = pReader.GetInt32(pReader.GetOrdinal("dv"));
                rol.ListaPermisos.AddRange(PermisoDAL.ListarPermisos(rol.ID));

                listaRoles.Add(rol);
            }

            pReader.Close();

            return listaRoles;
        }

        private static RolBE InsertarID(RolBE pRol)
        {
            string query = "select top 1 * from rol order by rol_id desc;";
            SqlDataReader sqlReader = DBManager.Obtener(query);

            while (sqlReader.Read())
            {
                pRol.ID = sqlReader.GetInt32(sqlReader.GetOrdinal("rol_id")) + 1;
            }

            sqlReader.Close();

            return pRol;
        }

        public static List<RolBE> ListarRoles()
        {
            string query = "select * from rol;";
            SqlDataReader readerRoles = DBManager.Obtener(query);

            return CargarBE(readerRoles);
        }

        public static RolBE ObtenerRol(string pNombre)
        {
            string query = "select * from rol where nombre = '" + pNombre + "';";
            SqlDataReader readerRol = DBManager.Obtener(query);

            return CargarBE(readerRol)[0];
        }

        public static RolBE ObtenerRol(int pID)
        {
            string query = "select * from rol where rol_id = '" + pID + "';";
            SqlDataReader readerRol = DBManager.Obtener(query);

            List<RolBE> listaRoles = CargarBE(readerRol);

            if(listaRoles.Count > 0)
            {
                return listaRoles[0];
            }
            else
            {
                return null;
            }
        }

        public static void CrearRol(RolBE pRol)
        {
            InsertarID(pRol);

            string query = "insert into rol(rol_id, nombre, dv) values(" + pRol.ID + ", '" + pRol.Nombre + "', " + pRol.DV + ");";
            DBManager.GuardarCambios(query);

            ModificarPermisos(pRol);
        }

        public static void ModificarPermisos(RolBE pRol)
        {
            string query;
            string queryQuitarCompuestos = "delete from rolCompuesto where rol_id = " + pRol.ID + ";";
            string queryQuitarSimples = "delete from rolSimple where rol_id = " + pRol.ID + ";";

            DBManager.GuardarCambios(queryQuitarCompuestos);
            DBManager.GuardarCambios(queryQuitarSimples);

            foreach(PermisoAbstractoBE permiso in pRol.ListaPermisos)
            {
                if(permiso.GetType() == typeof(PermisoCompuestoBE))
                {
                    query = "insert into rolCompuesto (rol_id, compuesto_id) values(" + pRol.ID + ", " + permiso.ID + ");";
                }
                else
                {
                    query = "insert into rolSimple (rol_id, simple_id) values(" + pRol.ID + ", " + permiso.ID + ");";
                }

                DBManager.GuardarCambios(query);
            }
        }

        public static void EliminarRol(int pID)
        {
            string query = "delete from rol where rol_id = " + pID + ";";
            DBManager.GuardarCambios(query);
        }
    }
}