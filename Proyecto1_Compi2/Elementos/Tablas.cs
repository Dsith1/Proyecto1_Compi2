﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Compi2.Elementos
{
    class Tablas
    {

        public Tabla cabeza;
        public Tabla ultimo;
        public Tabla aux;

        public Tablas()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }


        public void Insertar(Tabla nuevo)
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
                    if (aux.Nombre.Equals(nombre))
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

        public string Eliminar(string nombre)
        {


            if (cabeza == null)
            {
                return "No Existe el usuario " + nombre;

            }
            else
            {
                string respuesta = "";

                if (ultimo == null)
                {
                    if (cabeza.Nombre.Equals(nombre))
                    {
                        cabeza = null;

                        respuesta = "Usuario " + nombre + " Eliminado";

                    }
                    else
                    {
                        respuesta = "No Existe el usuario " + nombre;
                    }
                }
                else
                {

                    aux = cabeza;

                    bool seguir = true;

                    while (seguir)
                    {
                        if (aux.Nombre.Equals(nombre))
                        {

                            if (aux == cabeza)
                            {
                                cabeza = aux.siguiente;
                                cabeza.anterior = null;

                            }
                            else if (aux == ultimo)
                            {
                                ultimo = aux.anterior;
                                ultimo.siguiente = null;

                            }
                            else
                            {
                                aux.anterior.siguiente = aux.siguiente;
                                aux.siguiente.anterior = aux.anterior;
                            }
                            seguir = false;

                            respuesta = "Usuario " + nombre + " Eliminado";

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
                                respuesta = "No Existe el usuario " + nombre;
                            }
                        }
                    }

                }


                return respuesta;
            }

        }


    }
}
