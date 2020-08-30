using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IObservado
    {
        List<IObservador> Observadores { get; }
        void RegistrarObservador(IObservador pObservador);
        void DesregistrarObservador(IObservador pObservador);
        void ActualizarObservadores();
    }
}