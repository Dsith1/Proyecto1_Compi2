using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Objeto
    {
        public string nombre;
        public string campos;
        public string tipos;


        public Objeto siguiente;
        public Objeto anterior;

        public Objeto(string n,string c, string t)
        {
            siguiente = null;
            anterior = null;

            nombre = n;
            campos = c;
            tipos = t;
        }


    }
}
