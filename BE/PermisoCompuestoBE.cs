using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PermisoCompuestoBE : PermisoAbstractoBE
    {
        private List<PermisoAbstractoBE> _listaPermisos;

        public List<PermisoAbstractoBE> ListaPermisos { get; set; }

        public PermisoCompuestoBE()
        {
            this.ListaPermisos = new List<PermisoAbstractoBE>();
        }
    }
}
