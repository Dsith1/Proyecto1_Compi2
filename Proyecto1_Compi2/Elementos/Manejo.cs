using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Xml;

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

        public string Crear_Base(string nombre)
        {

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, nombre+".usac");

            string PathM = System.IO.Path.Combine(activeDir, "Maestro.usac");

            if (File.Exists(newPath)==false)
            {

                System.IO.FileStream fs = System.IO.File.Create(newPath);
                fs.Close();

                XmlDocument doc = new XmlDocument();

                XmlElement root = doc.DocumentElement;

                XmlElement element1 = doc.CreateElement(string.Empty, "DB", string.Empty);
                doc.AppendChild(element1);

                XmlElement element2 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                XmlText text1 = doc.CreateTextNode(nombre);
                element2.AppendChild(text1);
                element1.AppendChild(element2);

                XmlElement element3 = doc.CreateElement(string.Empty, "path", string.Empty);
                XmlText text2 = doc.CreateTextNode(newPath);
                element3.AppendChild(text2);
                element1.AppendChild(element3);

                doc.Save(PathM);


                return "Base De Datos " + nombre + " Creada";
            }
            else
            {
                return "Ya Existe la Base De Datos " + nombre;
            }

            

        }


        public string Crear_Tabla(string nombre,string Base)
        {
            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, Base+"_"+nombre + ".usac");

            if (File.Exists(newPath) == false)
            {
                System.IO.FileStream fs = System.IO.File.Create(newPath);
                return "Tabla " + nombre + " Creada en la Base "+Base;
            }
            else
            {
                return "Ya Existe la Tabla " + nombre+" En la Base "+ Base;
            }
        }
    }
    
}
