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

        public Objetos objetos;

        public Procedimientos procedimientos;

        public Base siguiente;
        public Base anterior;

        public Base(string n)
        {
            Nombre = n;
            Ruta = System.IO.Path.Combine(@"c:\DBMS", Nombre+".usac");

            siguiente = null;
            anterior = null;

            tablas = new Tablas();

            string or= System.IO.Path.Combine(@"c:\DBMS", Nombre + "_objetos.usac");
            objetos = new Objetos(or);

            string pr = System.IO.Path.Combine(@"c:\DBMS", Nombre + "_procedimientos.usac");
            procedimientos = new Procedimientos(pr);
        }
    }
}
