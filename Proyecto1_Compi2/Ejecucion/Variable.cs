using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Ejecucion
{
    class Variable
    {

        public string nombre;
        public string tipo;
        public string valor;

        public Variable siguiente;
        public Variable anterior;

        public Variable(string t,string id)
        {
            tipo = t;
            nombre = id;

            valor = "null";

            siguiente = null;
            anterior = null;
        }

        public void SetValor(string v)
        {
            valor = v;
        }

        public string GetValor()
        {
            return tipo + ";" + valor;
        }
    }
}
