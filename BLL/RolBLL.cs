using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RolBLL
    {
        private RolBE _be;

        public RolBE BE { get; set; }

        public static List<RolBLL> ListarRoles()
        {
            List<RolBLL> roles = new List<RolBLL>();

            foreach(RolBE rolBE in RolDAL.ListarRoles())
            {
                RolBLL rol = new RolBLL();
                rol.BE = rolBE;
                roles.Add(rol);
            }

            return roles;
        }

        public override string ToString()
        {
            return this.BE.Nombre;
        }
    }
}
