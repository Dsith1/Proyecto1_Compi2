﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Xml;
using System.Xml.Linq;

namespace Proyecto1_Compi2.Elementos
{
    class Manejo
    {
        Maestro maestro;

        public Manejo()
        {
            string activeDir = @"c:\";
            string newPath = System.IO.Path.Combine(activeDir, "DBMS");

            if (Directory.Exists(newPath) == false) { }
            {
                System.IO.Directory.CreateDirectory(newPath);
            }

        }

        public string Crear_Maestro()
        {
            maestro = new Maestro();
            

            return "Archivo Maestro Creado";

            //string activeDir = @"c:\DBMS";

            //string newPath = System.IO.Path.Combine(activeDir, "Maestro.usac");

            //if (File.Exists(newPath) == false)
            //{
            //    System.IO.FileStream fs = System.IO.File.Create(newPath);

            //    fs.Close();
            //    return "Archivo Maestro Creado" ;

            //    //Crear_Base("BMaestra") + "\r\n" + Crear_Tabla("Usuario", "BMaestra")


            //}
            //else
            //{
            //    return "";
            //}


        }
        
        public string Crear_Usuario(string nombre,string contra)
        {

            if (maestro.usuarios.existe(nombre))
            {
                return "El Usuario " + nombre + " Ya Existe";
            }
            else
            {
                Usuario nuevo = new Usuario(nombre, contra);

                maestro.usuarios.Insertar(nuevo);

                return "Se Ha Creado el Usuario " + nombre;
            }

            
        }
        
        public string Crear_Base(string nombre)
        {

            if (maestro.bases.existe(nombre))
            {
                return "La Base de Datos " + nombre + " Ya Existe";
            }
            else
            {
                Base nuevo = new Base(nombre);

                maestro.bases.Insertar(nuevo);

                return "Se Ha Creado La Base de Datos " + nombre;
            }            
        }
        
        public string Crear_Tabla(string nombre,string Base,string campos)
        {

            maestro.bases.existe(Base);

            if (maestro.bases.aux.tablas.existe(nombre))
            {
                return "Ya Existe la Tabla "+nombre +" En la Base "+Base;
            }
            else
            {

                string tipos = "";
                string fields = "";

                string[] campo = campos.Split(';');

                for( int x = 0; x < campo.Length; x++)
                {
                    string[] dato = campo[x].Split(',');

                    tipos += dato[0]+",";
                    fields += dato[1] + ",";
                }

                tipos = tipos.Trim(',');
                fields = fields.Trim(',');

                Tabla nuevo = new Tabla(nombre , tipos, fields);

                nuevo.SetRuta(System.IO.Path.Combine(@"c:\DBMS", Base+"_"+nombre+".usac"));

                maestro.bases.aux.tablas.Insertar(nuevo);

                return "Se Ha Creado La Tabla "+nombre+" En la Base de Datos " + Base;
            }


        }

        public string Insertar_Tabla(string valores,string Base, string Tabla)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.tablas.existe(Tabla))
            {

                valores = valores.Replace("entero", "INTEGER");
                valores = valores.Replace("Cadena", "TEXT");

                string[] val = valores.Split(',');

                string subt="";
                string subv = "";

                for (int x = 0; x < val.Length; x++)
                {
                    string[] aux = val[x].Split(';');

                    subt += aux[0] + ",";
                    subv += aux[1] + ",";
                }

                subt = subt.Trim(',');
                subv = subv.Trim(',');

                string tipos = maestro.bases.aux.tablas.aux.tipos;

                if (tipos.Equals(subt))
                {
                    Registro nuevo = new Registro(subv);

                    maestro.bases.aux.tablas.aux.Insertar(nuevo);

                    return "1 Fila Insertada";
                }
                else
                {
                    return "Error de Tipos se Esperaba(" + tipos + ") Se Encontro(" + subt + ")";
                }

                

                
            }
            else
            {

                return "No Existe la Tabla " + Tabla + " en La Base " + Base;
            }

            
        }

        public string Insertar_Tabla_v(string valores,string parametros, string Base, string Tabla)
        {

            maestro.bases.existe(Base);

            if (maestro.bases.aux.tablas.existe(Tabla))
            {

                string[] parametro = parametros.Split(',');

                string campos= maestro.bases.aux.tablas.aux.campos;

                bool encontrado = false;

                for (int x = 0; x < parametro.Length; x++)
                {

                    if (campos.Contains(parametro[x]))
                    {
                        encontrado = true;
                    }
                    else
                    {
                        encontrado = false;
                        break;
                    }
                }

                if (encontrado)
                {
                    string[] campo = campos.Split(',');

                    string[] val = valores.Split(',');

                    string tipos = maestro.bases.aux.tablas.aux.tipos;

                    string[] tipo = tipos.Split(',');

                    string subval = "";

                    int auxF = 0;

                    for (int x = 0; x < parametro.Length; x++)
                    {
                        for(int y = auxF; y < campo.Length; y++)
                        {
                            if (parametro[x].Equals(campo[y])){
                                subval += val[x]+",";

                                auxF = y+1;
                                break;
                            }
                        }
                    }


                    valores = subval.Trim(',');

                    valores = valores.Replace("entero", "INTEGER");
                    valores = valores.Replace("Cadena", "TEXT");

                    

                    if (auxF < campo.Length)
                    {
                        bool seguir = true;

                        while (seguir)
                        {

                            if (auxF < campo.Length)
                            {
                                valores += "," + tipo[auxF] + ";null";
                                auxF++;
                            }
                            else
                            {
                                seguir = false;
                            }
                        }
                    }

                    val = valores.Split(',');

                    string subt = "";
                    string subv = "";

                    for (int x = 0; x < val.Length; x++)
                    {
                        string[] aux = val[x].Split(';');

                        subt += aux[0] + ",";
                        subv += aux[1] + ",";
                    }

                    subt = subt.Trim(',');
                    subv = subv.Trim(',');

                    

                    if (tipos.Equals(subt))
                    {
                        Registro nuevo = new Registro(subv);

                        maestro.bases.aux.tablas.aux.Insertar(nuevo);

                        return "1 Fila Insertada";
                    }
                    else
                    {
                        return "Error de Tipos se Esperaba(" + tipos + ") Se Encontro(" + subt + ")";
                    }
                }
                else
                {
                    return "Error Campos inexistentes en la Tabla " + Tabla;
                }


                




            }
            else
            {

                return "No Existe la Tabla " + Tabla + " en La Base " + Base;
            }


        }

        public string Crear_Objeto(string nombre, string Base, string campos)
        {
            maestro.bases.existe(Base);

            if (maestro.bases.aux.objetos.existe(nombre))
            {
                return "Ya Existe el Objeto " + nombre + " En la Base " + Base;
            }
            else
            {

                string tipos = "";
                string fields = "";

                string[] campo = campos.Split(';');

                for (int x = 0; x < campo.Length; x++)
                {
                    string[] dato = campo[x].Split(',');

                    tipos += dato[0] + ",";
                    fields += dato[1] + ",";
                }

                tipos = tipos.Trim(',');
                fields = fields.Trim(',');

                Objeto nuevo = new Objeto(nombre, tipos, fields);

                maestro.bases.aux.objetos.Insertar(nuevo);

                return "Se Ha Creado La Tabla " + nombre + " En la Base de Datos " + Base;
            }




        }

        public bool Buscar_Objeto(string nombre,string Base)
        {
            maestro.bases.existe(nombre);

            return maestro.bases.aux.objetos.existe(nombre);
        }

        public string Crear_Procedimiento(string nombre, string Base, string campos,string instrucciones)
        {


            return "";

        }
    }
    
}
