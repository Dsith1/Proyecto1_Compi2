using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Registro
    {
        public string valor;

        public Registro siguiente;
        public Registro anterior;

        public Registro (string n)
        {
            n = valor;

            siguiente = null;
            anterior = null;
        }
    }
}
