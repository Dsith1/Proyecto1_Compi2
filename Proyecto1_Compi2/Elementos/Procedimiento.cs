using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Procedimiento
    {
        public string nombre;
        public string campos;
        public string tipos;
        public string instrucciones;
        public bool funcion = false;
        public string tipo;

        public Procedimiento siguiente;
        public Procedimiento anterior;


        public Procedimiento(string n,string codigo)
        {
            nombre = n;
            instrucciones = codigo;

            siguiente = null;
            anterior = null;

            campos = null;

        }

        public Procedimiento(string n, string codigo,string parametros)
        {
            nombre = n;
            instrucciones = codigo;

            siguiente = null;
            anterior = null;

        }
    }
}
