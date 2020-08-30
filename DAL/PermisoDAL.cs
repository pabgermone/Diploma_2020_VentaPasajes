using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Microsoft.SqlServer.Server;
using Servicios;

namespace DAL
{
    public static class PermisoDAL
    {
        private static List<PermisoAbstractoBE> CargarBE(SqlDataReader pReader, bool pCompuesto, PermisoCompuestoBE pPermiso = null)
        {
            List<PermisoAbstractoBE> listaPermisos = new List<PermisoAbstractoBE>();

            while (pReader.Read())
            {
                if (pCompuesto)
                {
                    if(!BuscarPermiso(listaPermisos, pReader.GetInt32(pReader.GetOrdinal("permiso_id"))))
                    {
                        PermisoCompuestoBE permisoCompuesto = new PermisoCompuestoBE();

                        permisoCompuesto.ID = pReader.GetInt32(pReader.GetOrdinal("permiso_id"));
                        permisoCompuesto.Nombre = pReader.GetString(pReader.GetOrdinal("nombre"));
                        permisoCompuesto.PadreID = pReader.GetInt32(pReader.GetOrdinal("compuesto_padre"));

                        string queryHijosComp = "select * from permisoCompuesto where compuesto_padre = " + permisoCompuesto.ID;
                        SqlDataReader readerHijosComp = DBManager.Obtener(queryHijosComp);

                        permisoCompuesto.ListaPermisos.AddRange(CargarBE(readerHijosComp, true, permisoCompuesto));

                        string queryHijos = "select * from hijosCompuesto where compuesto_id = " + permisoCompuesto.ID;
                        SqlDataReader readerHijos = DBManager.Obtener(queryHijos);

                        permisoCompuesto.ListaPermisos.AddRange(CargarBE(readerHijos, false, permisoCompuesto));

                        if (pPermiso == null)
                        {
                            listaPermisos.Add(permisoCompuesto);
                        }
                        else
                        {
                            pPermiso.ListaPermisos.Add(permisoCompuesto);
                        }
                    }
                }
                else
                {
                    PermisoSimpleBE permisoSimple = new PermisoSimpleBE();

                    permisoSimple.ID = pReader.GetInt32(pReader.GetOrdinal("permiso_id"));
                    permisoSimple.Nombre = pReader.GetString(pReader.GetOrdinal("nombre"));

                    if (pPermiso == null)
                    {
                        listaPermisos.Add(permisoSimple);
                    }
                    else
                    {
                        pPermiso.ListaPermisos.Add(permisoSimple);
                    }
                }
            }

            pReader.Close();

            return listaPermisos;
        }

        private static PermisoAbstractoBE InsertarID(PermisoAbstractoBE pPermiso)
        {
            string query;

            if(pPermiso.GetType() == typeof(PermisoCompuestoBE))
            {
                query = "select top 1 * from permisoCompuesto order by permiso_id desc;";
            }
            else
            {
                query = "select top 1 * from permisoSimple order by permiso_id desc;";
            }

            SqlDataReader sqlReader = DBManager.Obtener(query);

            while (sqlReader.Read())
            {
                pPermiso.ID = sqlReader.GetInt32(sqlReader.GetOrdinal("permiso_id"));
            }

            sqlReader.Close();

            return pPermiso;
        }

        private static bool BuscarPermiso(List<PermisoAbstractoBE> pLista, int pID)
        {
            foreach(PermisoAbstractoBE permiso in pLista)
            {
                if(permiso.GetType() == typeof(PermisoCompuestoBE))
                {
                    if(permiso.ID == pID)
                    {
                        return true;
                    }
                    else if(((PermisoCompuestoBE)permiso).ListaPermisos.Count > 0)
                    {
                        BuscarPermiso(((PermisoCompuestoBE)permiso).ListaPermisos, pID);
                    }
                }
            }

            return false;
        }

        public static List<PermisoAbstractoBE> ListarPermisos()
        {
            List<PermisoAbstractoBE> listaPermisos = new List<PermisoAbstractoBE>();

            string queryCompuestos = "select * from permisoCompuesto;";
            SqlDataReader readerCompuestos = DBManager.Obtener(queryCompuestos);

            listaPermisos.AddRange(CargarBE(readerCompuestos, true));

            string querySimples = "select * from permisoSimple where permiso_id not in(select distict(simple_id) from hijosCompuesto);";
            SqlDataReader readerSimples = DBManager.Obtener(querySimples);

            listaPermisos.AddRange(CargarBE(readerSimples, false));

            return listaPermisos;
        }

        public static List<PermisoAbstractoBE> ListarPermisos(int pRolID)
        {
            List<PermisoAbstractoBE> listaPermisos = new List<PermisoAbstractoBE>();

            string queryCompuestos = "select * from permisoCompuesto where permiso_id in (select compuesto_id from rolCompuesto where rol_id = " + pRolID + ");";
            SqlDataReader readerCompuestos = DBManager.Obtener(queryCompuestos);

            listaPermisos.AddRange(CargarBE(readerCompuestos, true));

            string querySimples = "select * from permisoSimple where permiso_id in (select simple_id from rolSimple where rol_id = " + pRolID + ");";
            SqlDataReader readerSimples = DBManager.Obtener(querySimples);

            listaPermisos.AddRange(CargarBE(readerSimples, false));

            return listaPermisos;
        }

        public static void AltaPermiso(PermisoCompuestoBE pPermiso)
        {
            string query;

            InsertarID(pPermiso);

            if(pPermiso.PadreID != 0)
            {
                query = "insert into permisoCompuesto(permiso_id, nombre, compuesto_padre) values(" + pPermiso.ID + ", '" + pPermiso.Nombre + "', " + pPermiso.PadreID + ");";
            }
            else
            {
                query = "insert into permisoCompuesto(permiso_id, nombre) values(" + pPermiso.ID + ", '" + pPermiso.Nombre + "');";
            }

            DBManager.GuardarCambios(query);
        }

        public static void AltaPermiso(PermisoSimpleBE pPermiso)
        {
            string query = "insert into permisoCompuesto(nombre) values(" + pPermiso.ID + ", '" + pPermiso.Nombre + "');";
            DBManager.GuardarCambios(query);
        }

        public static void RelacionarPermisos(PermisoCompuestoBE pPermiso)
        {
            if(pPermiso.ListaPermisos.Count() > 0)
            {
                string queryQuitarRelaciones = "delete from hijosCompuesto where compuesto_id = " + pPermiso.ID + ";";
                DBManager.GuardarCambios(queryQuitarRelaciones);

                foreach(PermisoAbstractoBE permiso in pPermiso.ListaPermisos)
                {
                    if(permiso.GetType() == typeof(PermisoSimpleBE))
                    {
                        string query = "insert into hijosCompuesto (compuesto_id, simple_id) values(" + pPermiso.ID + ", " + permiso.ID + ");";
                        DBManager.GuardarCambios(query);
                    }
                }
            }
        }

        public static void QuitarRelacion(int pIdCompuesto, int pIdSimple)
        {
            string query = "delete from hijosCompuesto where compuesto_id = " + pIdCompuesto + " and simple_id = " + pIdSimple + ";";
            DBManager.GuardarCambios(query);
        }

        public static void EliminarPermisoCompuesto(PermisoCompuestoBE pPermiso)
        {
            foreach(PermisoAbstractoBE permiso in pPermiso.ListaPermisos)
            {
                if(permiso.GetType() == typeof(PermisoSimpleBE))
                {
                    QuitarRelacion(pPermiso.ID, permiso.ID);
                }
                else
                {
                    EliminarPermisoCompuesto((PermisoCompuestoBE)permiso);
                }
            }

            string query = "delete from permisoCompuesto where permiso_id = " + pPermiso.ID + ";";
            DBManager.GuardarCambios(query);
        }

        public static void EliminarPermisoSimple(PermisoSimpleBE pPermiso)
        {
            string queryIntermedia = "delete from hijosCompuesto where simple_id = " + pPermiso.ID + ";";
            string queryPermiso = "delete from permisoSimple where permiso_id = " + pPermiso.ID + ";";

            DBManager.GuardarCambios(queryIntermedia);
            DBManager.GuardarCambios(queryPermiso);
        }
    }
}