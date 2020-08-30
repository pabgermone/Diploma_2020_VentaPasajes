using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        private UsuarioBE _be;

        public UsuarioBE BE { get; set; }

        public bool Login(string pUsuario, string pPassword)
        {
            this.BE = UsuarioDAL.ObtenerUsuario(pUsuario, pPassword);

            if(this.BE != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Registro(string pNombre, string pApellido, string pMail, string pUsuario, string pPassword, RolBLL pRol)
        {
            this.BE = new UsuarioBE();

            this.BE.Nombre = pNombre;
            this.BE.Apellido = pApellido;
            this.BE.Mail = pMail;
            this.BE.Usuario = pUsuario;
            this.BE.Password = pPassword;
            this.BE.Rol = pRol.BE;

            UsuarioDAL.AltaUsuario(this.BE);
        }
    }
}
