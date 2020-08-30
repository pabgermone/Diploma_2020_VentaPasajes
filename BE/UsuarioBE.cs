using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class UsuarioBE
    {
        private int _id;
        private string _usuario;
        private string _password;
        private RolBE _rol;
        private string _nombre;
        private string _apellido;
        private string _mail;
        private int _dv;

        public int ID { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public RolBE Rol { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public int DV { get; set; }
    }
}
