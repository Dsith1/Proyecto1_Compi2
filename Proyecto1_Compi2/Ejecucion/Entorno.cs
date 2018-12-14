using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto1_Compi2.Ejecucion
{
    class Entorno
    {
        public Entorno Padre;
        public Entorno Hijo;

        public Variables variables;

        public bool detener = false;
        public bool continuarSwitch = false;
        public bool defectoS = false;

        public string nombre;
        int tipo;

        public ParseTreeNode[] subEntornos;

        //1=global
        //2=procedimientos
        //3=if
        //4=for
        //5=while
        //6=llamada
        //7=caso

        public Entorno(int tipo)
        {
            Padre = null;
            Hijo = null;
            variables = new Variables();

        }
    }
}
