using System;
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

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, "Maestro.usac");

            if (File.Exists(newPath) == false)
            {
                System.IO.FileStream fs = System.IO.File.Create(newPath);

                fs.Close();
                return "Archivo Maestro Creado" ;

                //Crear_Base("BMaestra") + "\r\n" + Crear_Tabla("Usuario", "BMaestra")
                
            
            }
            else
            {
                return "";
            }

            
        }


        public string Crear_Usuario(string nombre,string contra)
        {
            string activeDir = @"c:\DBMS";

            string PathM = System.IO.Path.Combine(activeDir, "Maestro.usac");

            if (new FileInfo(PathM).Length == 0)
            {
                XmlDocument doc = new XmlDocument();

                XmlElement root = doc.DocumentElement;

                XmlElement Maestro = doc.CreateElement(string.Empty, "Maestro", string.Empty);
                doc.AppendChild(Maestro);

                XmlElement element1 = doc.CreateElement(string.Empty, "Usuario", string.Empty);
                Maestro.AppendChild(element1);

                XmlElement element2 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                XmlText text1 = doc.CreateTextNode(nombre);
                element2.AppendChild(text1);
                element1.AppendChild(element2);

                XmlElement element3 = doc.CreateElement(string.Empty, "Contra", string.Empty);
                XmlText text2 = doc.CreateTextNode(contra);
                element3.AppendChild(text2);
                element1.AppendChild(element3);

                doc.Save(PathM);

            }
            else
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(PathM);

                XmlNodeList tablas = doc.GetElementsByTagName("Maestro");


                XmlElement element1 = doc.CreateElement(string.Empty, "Usuario", string.Empty);
                tablas[0].AppendChild(element1);

                XmlElement element2 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                XmlText text1 = doc.CreateTextNode(nombre);
                element2.AppendChild(text1);
                element1.AppendChild(element2);

                XmlElement element3 = doc.CreateElement(string.Empty, "Contra", string.Empty);
                XmlText text2 = doc.CreateTextNode(contra);
                element3.AppendChild(text2);
                element1.AppendChild(element3);

                doc.Save(PathM);
            }

            return "Se Ha Creado el Usuario "+nombre;
        }


        public string Crear_Base(string nombre)
        {

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, nombre+".usac");

            string PathM = System.IO.Path.Combine(activeDir, "Maestro.usac");

            if (File.Exists(newPath)==false)
            {

                System.IO.FileStream fs = System.IO.File.Create(newPath);
                fs.Close();

                if (new FileInfo(PathM).Length == 0)
                {
                    XmlDocument doc = new XmlDocument();

                    XmlElement root = doc.DocumentElement;

                    XmlElement Maestro = doc.CreateElement(string.Empty, "Maestro", string.Empty);
                    doc.AppendChild(Maestro);

                    XmlElement element1 = doc.CreateElement(string.Empty, "DB", string.Empty);
                    Maestro.AppendChild(element1);

                    XmlElement element2 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                    XmlText text1 = doc.CreateTextNode(nombre);
                    element2.AppendChild(text1);
                    element1.AppendChild(element2);

                    XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                    XmlText text2 = doc.CreateTextNode(newPath);
                    element3.AppendChild(text2);
                    element1.AppendChild(element3);

                    doc.Save(PathM);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(PathM);

                    XmlNodeList tablas = doc.GetElementsByTagName("Maestro");


                    XmlElement element1 = doc.CreateElement(string.Empty, "DB", string.Empty);
                    tablas[0].AppendChild(element1);

                    XmlElement element2 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                    XmlText text1 = doc.CreateTextNode(nombre);
                    element2.AppendChild(text1);
                    element1.AppendChild(element2);

                    XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                    XmlText text2 = doc.CreateTextNode(newPath);
                    element3.AppendChild(text2);
                    element1.AppendChild(element3);

                    doc.Save(PathM);
                }

                

                return "Base De Datos " + nombre + " Creada";
            }
            else
            {
                return "Ya Existe la Base De Datos " + nombre;
            }

            

        }


        public string Crear_Tabla(string nombre,string Base,string campos)
        {
            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, Base+"_"+nombre + ".usac");

            string PathM = System.IO.Path.Combine(activeDir, Base+".usac");

            if (File.Exists(newPath) == false)
            {
                System.IO.FileStream fs = System.IO.File.Create(newPath);
                fs.Close();

                if (new FileInfo(PathM).Length == 0)
                {
                    XmlDocument doc = new XmlDocument();

                    XmlElement root = doc.DocumentElement;

                    XmlElement Maestro = doc.CreateElement(string.Empty, "BASE", string.Empty);
                    doc.AppendChild(Maestro);

                    XmlElement element1 = doc.CreateElement(string.Empty, "Tabla", string.Empty);
                    Maestro.AppendChild(element1);

                    XmlElement element2 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                    XmlText text1 = doc.CreateTextNode(nombre);
                    element2.AppendChild(text1);
                    element1.AppendChild(element2);

                    XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                    XmlText text2 = doc.CreateTextNode(newPath);
                    element3.AppendChild(text2);
                    element1.AppendChild(element3);



                    XmlElement filas = doc.CreateElement(string.Empty, "rows", string.Empty);
                    

                    string[] info = campos.Split(';');

                    for (int x = 0; x < (info.Length - 1); x++)
                    {
                        string[] data = info[x].Split(',');

                        string tipo = data[0];

                        string campo = data[1];

                        string atrib = "";

                        for (int j = 2; j < data.Length; j++)
                        {
                            atrib += data[j];

                            if (j != (data.Length - 1))
                            {
                                atrib += ",";
                            }

                        }

                        XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                        XmlText ncampo = doc.CreateTextNode(campo);

                        campoT.SetAttribute("atributos", atrib);

                        campoT.AppendChild(ncampo);
                        filas.AppendChild(campoT);


                    }

                    element1.AppendChild(filas);


                    doc.Save(PathM);
                }
                else
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(PathM);

                    XmlNodeList tablas = doc.GetElementsByTagName("BASE");


                    XmlElement element1 = doc.CreateElement(string.Empty, "Tabla", string.Empty);
                    tablas[0].AppendChild(element1);

                    XmlElement element2 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                    XmlText text1 = doc.CreateTextNode(nombre);
                    element2.AppendChild(text1);
                    element1.AppendChild(element2);

                    XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                    XmlText text2 = doc.CreateTextNode(newPath);
                    element3.AppendChild(text2);
                    element1.AppendChild(element3);

                    XmlElement filas = doc.CreateElement(string.Empty, "rows", string.Empty);

                    string[] info = campos.Split(';');

                    for (int x = 0; x < (info.Length - 1); x++)
                    {
                        string[] data = info[x].Split(',');

                        string tipo = data[0];

                        string campo = data[1];

                        string atrib = "";

                        for (int j = 2; j < data.Length; j++)
                        {
                            atrib += data[j];

                            if (j != (data.Length - 1))
                            {
                                atrib += ",";
                            }

                        }

                        XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                        XmlText ncampo = doc.CreateTextNode(campo);

                        campoT.SetAttribute("atributos", atrib);

                        campoT.AppendChild(ncampo);
                        filas.AppendChild(campoT);

                    }

                    element1.AppendChild(filas);

                    doc.Save(PathM);
                }



                return "Tabla " + nombre + " Creada en la Base "+Base;
            }
            else
            {
                return "Ya Existe la Tabla " + nombre+" En la Base "+ Base;
            }
        }

        public string Insertar_Tabla(string valores,string Base, string Tabla)
        {
            string activeDir = @"c:\DBMS";

            string PathT = System.IO.Path.Combine(activeDir, Base+"_"+Tabla + ".usac");

            string PathB = System.IO.Path.Combine(activeDir, Base +".usac");

            if (File.Exists(PathT))
            {
                string[] valor = valores.Split(',');

                XmlDocument BASE = new XmlDocument();
                BASE.Load(PathB);

                XmlNodeList tablas = BASE.GetElementsByTagName("Tabla");

                int Lugar=0;
                
                for(int x = 0; x < tablas.Count; x++)
                {
                    XmlNode row = tablas[x];

                    string nombreTabla = row.SelectSingleNode("//Nombre").InnerText;

                    if (nombreTabla.Equals(Tabla))
                    {
                        Lugar = x;
                        break;
                    }
                }

                XmlNode fila = tablas[Lugar].SelectSingleNode("//rows");

                XmlNodeList campos = fila.ChildNodes;


               
                if (valor.Length == campos.Count)
                {
                    string tipos = "";

                    for (int x = 0; x < campos.Count; x++)
                    {
                        tipos += campos[x].InnerText+",";

                    }

                    string[] tipo = tipos.Split(',');

                    if (new FileInfo(PathT).Length == 0)
                    {
                        XmlDocument doc = new XmlDocument();

                        XmlElement root = doc.DocumentElement;

                        XmlElement Maestro = doc.CreateElement(string.Empty, "TABLA", string.Empty);
                        doc.AppendChild(Maestro);

                        XmlElement element1 = doc.CreateElement(string.Empty, "ROW", string.Empty);
                        Maestro.AppendChild(element1);

                        for (int x = 0; x < tipo.Length-1; x++)
                        {
                            XmlElement campoT = doc.CreateElement(string.Empty, tipo[x], string.Empty);
                            XmlText ncampo = doc.CreateTextNode(valor[x]);

                            campoT.AppendChild(ncampo);
                            element1.AppendChild(campoT);

                        }

                        doc.Save(PathT);

                    }
                    else
                    {
                        XDocument doc = XDocument.Load(PathT);

                        XElement root = new XElement("ROW");

                        for (int x = 0; x < tipo.Length - 1; x++)
                        {
                            root.Add(new XElement(tipo[x], valor[x]));
                        }

                        doc.Save(PathT);
                    }




                    return "\r\nFila Insertada";
                }
                else
                {
                    return "El Numero de Campos No Coincide con la Tabla "+ Tabla;
                }

                
            }
            else
            {
                return "No Existe la Tabla " + Tabla;
            }


            
        }

        public string Crear_Objeto(string nombre, string Base, string campos)
        {

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, Base + "_Objetos.usac");

            string PathM = System.IO.Path.Combine(activeDir, Base + ".usac");


            if (File.Exists(newPath) == false)
            {
                System.IO.FileStream fs = System.IO.File.Create(newPath);
                fs.Close();

                if (new FileInfo(PathM).Length == 0)
                {
                    XmlDocument doc = new XmlDocument();

                    XmlElement root = doc.DocumentElement;

                    XmlElement Maestro = doc.CreateElement(string.Empty, "BASE", string.Empty);
                    doc.AppendChild(Maestro);

                    XmlElement element1 = doc.CreateElement(string.Empty, "Object", string.Empty);
                    Maestro.AppendChild(element1);

                    XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                    XmlText text2 = doc.CreateTextNode(newPath);
                    element3.AppendChild(text2);
                    element1.AppendChild(element3);

                    doc.Save(PathM);

                    doc= new XmlDocument();

                    root = doc.DocumentElement;

                    XmlElement MaestroO = doc.CreateElement(string.Empty, "OBJETO", string.Empty);
                    doc.AppendChild(MaestroO);


                    XmlElement element4 = doc.CreateElement(string.Empty, "Obj", string.Empty);
                    MaestroO.AppendChild(element4);

                    XmlElement element5 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                    XmlText text = doc.CreateTextNode(nombre);
                    element5.AppendChild(text);
                    element4.AppendChild(element5);

                    XmlElement atributos = doc.CreateElement(string.Empty, "attr", string.Empty);
                    

                    string[] info = campos.Split(';');

                    for (int x = 0; x < (info.Length - 1); x++)
                    {
                        string[] data = info[x].Split(',');

                        string tipo = data[0];

                        string campo = data[1];


                        XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                        XmlText ncampo = doc.CreateTextNode(campo);


                        campoT.AppendChild(ncampo);
                        atributos.AppendChild(campoT);


                    }

                    element4.AppendChild(atributos);

                    doc.Save(newPath);


                }
                else
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(PathM);

                    XmlNodeList maestro = doc.GetElementsByTagName("Object");

                    if (maestro.Count == 0)
                    {
                        XmlNodeList NBase = doc.GetElementsByTagName("BASE");

                        XmlElement element1 = doc.CreateElement(string.Empty, "Object", string.Empty);
                        NBase[0].AppendChild(element1);

                        XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                        XmlText text2 = doc.CreateTextNode(newPath);
                        element3.AppendChild(text2);
                        element1.AppendChild(element3);


                        newPath = maestro[0].ChildNodes[0].InnerText;

                        doc.Save(PathM);

                        doc = new XmlDocument();

                        doc.Load(newPath);

                        XmlNodeList objetos = doc.GetElementsByTagName("OBJETO");


                        XmlElement element4 = doc.CreateElement(string.Empty, "Obj", string.Empty);
                        objetos[0].AppendChild(element4);

                        XmlElement element5 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                        XmlText text = doc.CreateTextNode(nombre);
                        element5.AppendChild(text);
                        element4.AppendChild(element5);

                        XmlElement atributos = doc.CreateElement(string.Empty, "attr", string.Empty);


                        string[] info = campos.Split(';');

                        for (int x = 0; x < (info.Length - 1); x++)
                        {
                            string[] data = info[x].Split(',');

                            string tipo = data[0];

                            string campo = data[1];


                            XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                            XmlText ncampo = doc.CreateTextNode(campo);


                            campoT.AppendChild(ncampo);
                            atributos.AppendChild(campoT);


                        }

                        element4.AppendChild(atributos);

                        doc.Save(newPath);

                    }
                    else
                    {
                        newPath = maestro[0].ChildNodes[0].InnerText;

                        doc = new XmlDocument();

                        doc.Load(newPath);

                        XmlNodeList objetos = doc.GetElementsByTagName("OBJETO");


                        XmlElement element4 = doc.CreateElement(string.Empty, "Obj", string.Empty);
                        objetos[0].AppendChild(element4);

                        XmlElement element5 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                        XmlText text = doc.CreateTextNode(nombre);
                        element5.AppendChild(text);
                        element4.AppendChild(element5);

                        XmlElement atributos = doc.CreateElement(string.Empty, "attr", string.Empty);


                        string[] info = campos.Split(';');

                        for (int x = 0; x < (info.Length - 1); x++)
                        {
                            string[] data = info[x].Split(',');

                            string tipo = data[0];

                            string campo = data[1];


                            XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                            XmlText ncampo = doc.CreateTextNode(campo);


                            campoT.AppendChild(ncampo);
                            atributos.AppendChild(campoT);


                        }

                        element4.AppendChild(atributos);

                        doc.Save(newPath);
                    }

                    


                }



                return "Objeto " + nombre + " Creada en la Base " + Base;
            }
            else
            {
                return "Ya Existe la Tabla " + nombre + " En la Base " + Base;
            }

            
        }

        public string Crear_Procedimiento(string nombre, string Base, string campos,string instrucciones)
        {

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, Base + "_Procedimientos.usac");

            string PathM = System.IO.Path.Combine(activeDir, Base + ".usac");


            if (File.Exists(newPath) == false)
            {
                System.IO.FileStream fs = System.IO.File.Create(newPath);
                fs.Close();

                if (new FileInfo(PathM).Length == 0)
                {
                    XmlDocument doc = new XmlDocument();

                    XmlElement root = doc.DocumentElement;

                    XmlElement Maestro = doc.CreateElement(string.Empty, "BASE", string.Empty);
                    doc.AppendChild(Maestro);

                    XmlElement element1 = doc.CreateElement(string.Empty, "Procedure", string.Empty);
                    Maestro.AppendChild(element1);

                    XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                    XmlText text2 = doc.CreateTextNode(newPath);
                    element3.AppendChild(text2);
                    element1.AppendChild(element3);

                    doc.Save(PathM);

                    doc = new XmlDocument();

                    root = doc.DocumentElement;

                    XmlElement MaestroO = doc.CreateElement(string.Empty, "PROCEDIMIENTO", string.Empty);
                    doc.AppendChild(MaestroO);


                    XmlElement element4 = doc.CreateElement(string.Empty, "Proc", string.Empty);
                    MaestroO.AppendChild(element4);

                    XmlElement element5 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                    XmlText text = doc.CreateTextNode(nombre);
                    element5.AppendChild(text);
                    element4.AppendChild(element5);

                    XmlElement atributos = doc.CreateElement(string.Empty, "params", string.Empty);


                    string[] info = campos.Split(';');

                    for (int x = 0; x < (info.Length - 1); x++)
                    {
                        string[] data = info[x].Split(',');

                        string tipo = data[0];

                        string campo = data[1];


                        XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                        XmlText ncampo = doc.CreateTextNode(campo);


                        campoT.AppendChild(ncampo);
                        atributos.AppendChild(campoT);


                    }

                    element4.AppendChild(atributos);

                    XmlElement element6 = doc.CreateElement(string.Empty, "src", string.Empty);
                    XmlText text3 = doc.CreateTextNode(instrucciones);
                    element6.AppendChild(text3);
                    element4.AppendChild(element6);


                    doc.Save(newPath);


                }
                else
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(PathM);

                    XmlNodeList maestro = doc.GetElementsByTagName("Procedure");

                    if (maestro.Count == 0)
                    {
                        XmlNodeList NBase = doc.GetElementsByTagName("BASE");

                        XmlElement element1 = doc.CreateElement(string.Empty, "Procedure", string.Empty);
                        NBase[0].AppendChild(element1);

                        XmlElement element3 = doc.CreateElement(string.Empty, "Path", string.Empty);
                        XmlText text2 = doc.CreateTextNode(newPath);
                        element3.AppendChild(text2);
                        element1.AppendChild(element3);


                        newPath = maestro[0].ChildNodes[0].InnerText;

                        doc.Save(PathM);

                        doc = new XmlDocument();

                        doc.Load(newPath);

                        XmlNodeList objetos = doc.GetElementsByTagName("OBJETO");


                        XmlElement element4 = doc.CreateElement(string.Empty, "Obj", string.Empty);
                        objetos[0].AppendChild(element4);

                        XmlElement element5 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                        XmlText text = doc.CreateTextNode(nombre);
                        element5.AppendChild(text);
                        element4.AppendChild(element5);

                        XmlElement atributos = doc.CreateElement(string.Empty, "attr", string.Empty);


                        string[] info = campos.Split(';');

                        for (int x = 0; x < (info.Length - 1); x++)
                        {
                            string[] data = info[x].Split(',');

                            string tipo = data[0];

                            string campo = data[1];


                            XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                            XmlText ncampo = doc.CreateTextNode(campo);


                            campoT.AppendChild(ncampo);
                            atributos.AppendChild(campoT);


                        }

                        element4.AppendChild(atributos);

                        XmlElement element6 = doc.CreateElement(string.Empty, "src", string.Empty);
                        XmlText text3 = doc.CreateTextNode(instrucciones);
                        element6.AppendChild(text3);
                        element4.AppendChild(element6);

                        doc.Save(newPath);

                    }
                    else
                    {
                        newPath = maestro[0].ChildNodes[0].InnerText;

                        doc = new XmlDocument();

                        doc.Load(newPath);

                        XmlNodeList objetos = doc.GetElementsByTagName("PROCEDIMIENTO");


                        XmlElement element4 = doc.CreateElement(string.Empty, "Proc", string.Empty);
                        objetos[0].AppendChild(element4);

                        XmlElement element5 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                        XmlText text = doc.CreateTextNode(nombre);
                        element5.AppendChild(text);
                        element4.AppendChild(element5);

                        XmlElement atributos = doc.CreateElement(string.Empty, "params", string.Empty);


                        string[] info = campos.Split(';');

                        for (int x = 0; x < (info.Length - 1); x++)
                        {
                            string[] data = info[x].Split(',');

                            string tipo = data[0];

                            string campo = data[1];


                            XmlElement campoT = doc.CreateElement(string.Empty, tipo, string.Empty);
                            XmlText ncampo = doc.CreateTextNode(campo);


                            campoT.AppendChild(ncampo);
                            atributos.AppendChild(campoT);


                        }

                        element4.AppendChild(atributos);

                        XmlElement element6 = doc.CreateElement(string.Empty, "src", string.Empty);
                        XmlText text3 = doc.CreateTextNode(instrucciones);
                        element6.AppendChild(text3);
                        element4.AppendChild(element6);

                        doc.Save(newPath);
                    }




                }



                return "Procedimiento " + nombre + " Creado en la Base " + Base;
            }
            else
            {
                return "Ya Existe la Tabla " + nombre + " En la Base " + Base;
            }


        }
    }
    
}
