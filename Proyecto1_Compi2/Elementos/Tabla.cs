using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Tabla
    {
        public string Nombre;
        
        public string tipos;
        public string campos;

        public string ruta;

        public Tabla siguiente;
        public Tabla anterior;
        
        public Registro cabeza;
        public Registro ultimo;
        public Registro aux;

        public Tabla(string n,string t, string c)
        {
            Nombre = n;
            tipos = t;
            campos = c;

            siguiente = null;
            anterior = null;

            cabeza = null;
            ultimo = null;
            aux = null;

        }

        public void SetRuta(string r)
        {
            ruta = r;
        }

        public void Insertar(Registro nuevo)
        {
            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else if (ultimo == null)
            {
                ultimo = nuevo;

                ultimo.anterior = cabeza;
                cabeza.siguiente = ultimo;
            }
            else
            {
                aux = nuevo;

                aux.anterior = ultimo;
                ultimo.siguiente = aux;

                ultimo = aux;

            }
        }


    }
}
