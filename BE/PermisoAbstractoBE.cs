using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class PermisoAbstractoBE
    {
        private int _id;
        private string _nombre;
        private int _padreID;
        private int _dv;

        public int ID { get; set; }
        public string Nombre { get; set; }
        public int PadreID { get; set; }
        public int DV { get; set; }
    }
}