using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Usuario
    {
        public string Nombre;
        public string Contraseña;

        public Usuario siguiente;
        public Usuario anterior;

        public string permiso;


        public Usuario(string n,string c)
        {
            Nombre = n;
            Contraseña = c;
            siguiente = null;
            anterior = null;

            permiso = "";
        }
    }
}
