using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Objetos
    {

        public Objeto cabeza;
        public Objeto ultimo;
        public Objeto aux;

        public string ruta;

        public Objetos(string r)
        {
            ruta = r;

            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Objeto nuevo)
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

        public bool existe(string nombre)
        {
            if (cabeza == null)
            {
                return false;
            }
            else
            {
                aux = cabeza;

                bool seguir = true;
                bool respuesta = false;

                while (seguir)
                {
                    if (aux.nombre.Equals(nombre))
                    {
                        respuesta = true;
                        seguir = false;
                    }
                    else
                    {
                        if (aux.siguiente != null)
                        {
                            aux = aux.siguiente;
                        }
                        else
                        {
                            seguir = false;
                            respuesta = false;
                        }
                    }
                }

                return respuesta;
            }

        }
    }
}
