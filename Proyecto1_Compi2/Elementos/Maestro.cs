using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Maestro
    {
        public Usuarios usuarios;
        public Bases bases;
        string ruta;

        public Maestro()
        {
            usuarios = new Usuarios();
            bases = new Bases();
            ruta= System.IO.Path.Combine(@"c:\DBMS", "Maestro.usac");
        }
    }
}
