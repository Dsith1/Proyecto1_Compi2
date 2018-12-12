using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Base
    {
        public string Nombre;
        public string Ruta;

        public Tablas tablas;

        public Base siguiente;
        public Base anterior;

        public Base(string n,string r)
        {
            Nombre = n;
            Ruta = r;

            siguiente = null;
            anterior = null;

            tablas = new Tablas();
        }
    }
}
