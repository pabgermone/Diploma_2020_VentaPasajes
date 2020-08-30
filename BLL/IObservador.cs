using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IObservador
    {
        void Actualizar(System.Windows.Forms.Control pObservador);
    }
}