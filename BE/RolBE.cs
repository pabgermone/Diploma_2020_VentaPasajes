using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class RolBE
    {
        private int _id;
        private string _nombre;
        private List<PermisoAbstractoBE> _listaPermisos;
        private int _dv;

        public int ID { get; set; }
        public string Nombre { get; set; }
        public List<PermisoAbstractoBE> ListaPermisos { get; set; }
        public int DV { get; set; }

        public RolBE()
        {
            this.ListaPermisos = new List<PermisoAbstractoBE>();
        }
    }
}
