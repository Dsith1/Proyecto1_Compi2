﻿using Irony.Parsing;
using Proyecto1_Compi2.Analizadores;
using Proyecto1_Compi2.Elementos;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1_Compi2
{
    public partial class Form1 : Form
    {

        string graph = "";
        string cadena;
        bool escuchar = false;
        string BaseActual;
        string UserActaul;
        string TablaAux;
        int tipoin;
        Manejo manejo;
        bool Cproc = false;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            manejo = new Manejo();
            string Consola = txtConsola.Text;

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, "Maestro.usac");

            if (File.Exists(newPath) == false)
            {
                Consola += manejo.Crear_Maestro();

                Consola += "\r\n" + manejo.Crear_Usuario("Admin", "1234");

                txtConsola.Text = Consola;

            }



        }

        public void AnalizarPaquete(string entrada)
        {
            string Consola = txtConsola.Text;
            Consola += "\r\nAnalizando Paquete ";
            txtConsola.Text = Consola;

            AnalizadorPaquete gramatica = new AnalizadorPaquete();


            string respuesta = esCadenaValidaPaquete(entrada, gramatica);

            MessageBox.Show("Grafo Finalizado");

        }


        public void Analizar(string entrada)
        {
            string Consola = txtConsola.Text;
            Consola += "\r\nAnalizando En Lenguaje USQL ";
            txtConsola.Text = Consola;

            Analizador_USQL gramatica = new Analizador_USQL();

            MessageBox.Show("Arbol de Analisis Sintactico Constuido !!!");

            string respuesta = esCadenaValidSQL(entrada, gramatica);

            MessageBox.Show("Grafo Finalizado");

        }

        public string esCadenaValidaPaquete(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);

            string a = "";
            if (arbol.HasErrors())
            {
                MessageBox.Show("Errores en la cadena de entrada");


                if (arbol.Root != null)
                {


                }
            }
            else
            {
                if (arbol.Root != null)
                {
                    a = ActuarPaquete(arbol.Root);

                    if (a.Equals("Fin"))
                    {
                        string Consola = txtConsola.Text;
                        Consola += "\r\nFin se Sesion";
                        txtConsola.Text = Consola;

                    }
                    else
                    {
                        a = a.Substring(1, a.Length - 2);

                        Analizar(a);
                    }



                }
            }


            return a;

        }


        public string esCadenaValidSQL(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);

            string a = "";
            if (arbol.HasErrors())
            {
                MessageBox.Show("Errores en la cadena de entrada");


                if (arbol.Root != null)
                {


                }
            }
            else
            {
                if (arbol.Root != null)
                {
                    a = ActuarSQL(arbol.Root);

                    string Consola = txtConsola.Text;
                    Consola += a;
                    txtConsola.Text = Consola;

                }
            }


            return a;

        }


        public void GenarbolC(ParseTreeNode raiz)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Arboles/ArbolC.txt");
            f.Write("digraph lista{ rankdir=TB;node [shape = box, style=rounded]; ");
            graph = "";
            Generar(raiz);
            f.Write(graph);
            f.Write("}");
            f.Close();

        }

        public void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"red\", style =\"filled\", shape=\"circle\"]; \r\n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \r\n";
                }
            }
        }

        private static void GenerateGraphC(string fileName, string path)
        {
            try
            {
                //String dotPath = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
                String archivoEntrada = "C:\\Arboles\\ArbolC.txt";
                String archivoSalida = "C:\\Arboles\\ArbolC.png";


                string comando = "dot " + archivoEntrada + @" -o " + archivoSalida + " -Tpng";
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;

                cmd.Start();
                cmd.StandardInput.WriteLine(comando);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                cmd.StandardOutput.ReadToEnd();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            escuchar = true;
            cadena = textBox1.Text;


            /*
            while (escuchar)
            {
                Conectar();
            }

            */
            AnalizarPaquete(cadena);

        }

        public void Conectar()
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // paso 2 - creamos el socket
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Any, 6000);
            //paso 3 -IPAddress.Any significa que va a escuchar al cliente en toda la red 

            byte[] bytes = new Byte[1024];
            try
            {
                // paso 4
                miPrimerSocket.Bind(miDireccion); // Asociamos el socket a miDireccion
                miPrimerSocket.Listen(1); // Lo ponemos a escucha

                Console.WriteLine("Escuchando...");

                Socket Escuchar = miPrimerSocket.Accept();
                //creamos el nuevo socket, para comenzar a trabajar con él
                //La aplicación queda en reposo hasta que el socket se conecte a el cliente
                //Una vez conectado, la aplicación sigue su camino  
                Console.WriteLine("Conectado con exito");


                int bytesRec = Escuchar.Receive(bytes);



                cadena += Encoding.ASCII.GetString(bytes, 0, bytesRec);





                /*Aca ponemos todo lo que queramos hacer con el socket, osea antes de 
                cerrarlo je*/
                miPrimerSocket.Close(); //Luego lo cerramos

            }
            catch (Exception error)
            {
                Console.WriteLine("Error: {0}", error.ToString());
            }
            Console.WriteLine("Presione cualquier tecla para terminar");
            Console.ReadLine();

        }


        string ActuarPaquete(ParseTreeNode nodo)
        {
            string resultado = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {

                        resultado = ActuarPaquete(nodo.ChildNodes[0]);
                        //if (nodo.ChildNodes.Count == 2)


                        break;
                    }

                case "inicio":

                    resultado = ActuarPaquete(nodo.ChildNodes[1]);
                    break;

                case "cuerpo":

                    resultado = ActuarPaquete(nodo.ChildNodes[0]);
                    break;

                case "login":

                    resultado = ActuarPaquete(nodo.ChildNodes[4]);
                    break;

                case "sublogin":

                    resultado = nodo.ChildNodes[6].Token.Text;
                    break;

                case "paquete":

                    resultado = ActuarPaquete(nodo.ChildNodes[2]);
                    break;

                case "usql":

                    resultado = nodo.ChildNodes[4].Token.Text;
                    break;

                case "fin":

                    resultado = "Fin";
                    break;



            }

            return resultado;
        }


        string ActuarSQL(ParseTreeNode nodo)
        {
            string resultado = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {

                        resultado = ActuarSQL(nodo.ChildNodes[0]);
                        //if (nodo.ChildNodes.Count == 2)


                        break;
                    }

                case "inicio":
                    {
                        resultado = ActuarSQL(nodo.ChildNodes[0]);
                        break;
                    }

                case "sentencias":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                            resultado += ActuarSQL(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }


                        break;
                    }

                case "sentencia":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);

                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "usar":
                    {
                        BaseActual = nodo.ChildNodes[1].Token.Text;

                        resultado = "\r\nLa Base de Datos que se Usará es " + BaseActual;
                        break;
                    }

                case "crear":
                    {
                        resultado = ActuarSQL(nodo.ChildNodes[1]);
                        break;
                    }

                case "opciones_crear":
                    {
                        resultado = ActuarSQL(nodo.ChildNodes[0]);
                        break;
                    }

                case "c_base":
                    {
                        string nombreb = nodo.ChildNodes[1].Token.Text;
                        manejo.Crear_Base(nombreb);

                        resultado = "\r\nSe Ha Creado la Base " + nombreb;
                        break;
                    }

                case "c_tabla":
                    {
                        if (BaseActual != "")
                        {
                            TablaAux = nodo.ChildNodes[1].Token.Text;

                            string campos = ActuarSQL(nodo.ChildNodes[3]);

                            manejo.Crear_Tabla(TablaAux, BaseActual, campos);

                            resultado = "\r\nSe Ha Creado la Tabla " + TablaAux;
                        }
                        else
                        {
                            resultado = "\r\nNo Hay Base de DAtos Asignada Actualmente";
                        }


                        break;
                    }

                case "campos_tabla":
                    {

                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]) + ";";
                            resultado += ActuarSQL(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "campo_tabla":
                    {

                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("tipo_dato"))
                            {

                                resultado = ActuarSQL(nodo.ChildNodes[0]) + ",";
                                resultado += nodo.ChildNodes[1].Token.Text + ",";
                                resultado += ActuarSQL(nodo.ChildNodes[2]);
                            }
                            else
                            {
                                resultado = nodo.ChildNodes[0].Token.Text + ",";
                                resultado += nodo.ChildNodes[1].Token.Text + ",";
                                resultado += ActuarSQL(nodo.ChildNodes[2]); ;

                            }


                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("tipo_dato"))
                            {

                                resultado = ActuarSQL(nodo.ChildNodes[0]) + ",";
                                resultado += nodo.ChildNodes[1].Token.Text.Trim(); ;
                            }
                            else
                            {
                                resultado = nodo.ChildNodes[0].Token.Text + ",";
                                resultado += nodo.ChildNodes[1].Token.Text;

                            }
                        }

                        break;
                    }

                case "tipo_dato":
                    {


                        resultado = nodo.ChildNodes[0].Token.Text.Trim();

                        break;
                    }

                case "complementos":
                    {


                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);


                            string aux = ActuarSQL(nodo.ChildNodes[1]);

                            if (resultado.Equals("NO") && aux.Equals("NULO"))
                            {
                                resultado += " " + aux;
                            }
                            else if (resultado.Equals("NO"))
                            {
                                resultado = ActuarSQL(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                resultado += "," + aux;
                            }
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "complemento":
                    {


                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = nodo.ChildNodes[0].Token.Text + " ";
                            resultado += ActuarSQL(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = nodo.ChildNodes[0].Token.Text;
                        }

                        break;
                    }

                case "c_objeto":
                    {

                        if (BaseActual != "")
                        {

                            if (nodo.ChildNodes.Count == 6)
                            {
                                TablaAux = nodo.ChildNodes[1].Token.Text;

                                string campos = ActuarSQL(nodo.ChildNodes[3]);

                                manejo.Crear_Objeto(TablaAux, BaseActual, campos);
                            }
                            else
                            {
                                TablaAux = nodo.ChildNodes[1].Token.Text;

                                string campos = "";

                                manejo.Crear_Objeto(TablaAux, BaseActual, campos);
                            }



                            resultado = "\r\nSe Ha Creado El Objeto " + TablaAux;
                        }
                        else
                        {
                            resultado = "\r\nNo Hay Base de Datos Asignada Actualmente";
                        }


                        break;
                    }

                case "parametros":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]) + ";";
                            resultado += ActuarSQL(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "parametro":
                    {


                        if (nodo.ChildNodes[0].Term.Name.ToString().Equals("tipo_dato"))
                        {

                            resultado = ActuarSQL(nodo.ChildNodes[0]) + ",";
                            resultado += nodo.ChildNodes[1].Token.Text;
                        }
                        else
                        {
                            resultado = nodo.ChildNodes[0].Token.Text + ",";
                            resultado += nodo.ChildNodes[1].Token.Text;

                        }


                        break;
                    }

                case "c_pro":
                    {

                        if (BaseActual != "")
                        {


                            if (nodo.ChildNodes.Count == 8)
                            {
                                TablaAux = nodo.ChildNodes[1].Token.Text;

                                string campos = ActuarSQL(nodo.ChildNodes[3]);
                                Cproc = true;

                                string instrucciones = ActuarSQL(nodo.ChildNodes[6]);

                                Cproc = false;

                                manejo.Crear_Objeto(TablaAux, BaseActual, campos);
                            }
                            else
                            {
                                TablaAux = nodo.ChildNodes[1].Token.Text;

                                Cproc = true;

                                string instrucciones = ActuarSQL(nodo.ChildNodes[5]);

                                Cproc = false;

                                //manejo.Crear_Objeto(TablaAux, BaseActual, "");
                            }



                            resultado = "\r\nSe Ha Creado El Procedimiento " + TablaAux;
                        }
                        else
                        {
                            resultado = "\r\nNo Hay Base de Datos Asignada Actualmente";
                        }


                        break;
                    }

                case "instrucciones":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                            resultado += ActuarSQL(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "instruccion":
                    {

                        if (nodo.ChildNodes[0].Term.Name.ToString().Equals("RDETENER"))
                        {
                            //
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "c_usuario":
                    {

                        string Nusuario = nodo.ChildNodes[1].Token.Text;
                        string NPass = nodo.ChildNodes[5].Token.Text;

                        resultado = "\r\n" + manejo.Crear_Usuario(Nusuario, NPass);

                        break;
                    }

                case "imprimir":
                    {

                        resultado = "\r\n" + ActuarSQL(nodo.ChildNodes[2]);

                        if (resultado.Contains("Error")){
                            if (resultado.Contains(";"))
                            {
                                string[] val = resultado.Split(';');

                                resultado = "\r\nError al Imprimir " + val[1];
                            }
                        }
                        else
                        {
                            if (resultado.Contains(";"))
                            {
                                string[] val = resultado.Split(';');

                                resultado = "\r\n" + val[1];
                            }
                        }

                        

                        resultado = resultado.Replace("\"", "");

                        break;
                    }

                case "expresion":
                    {

                        if (nodo.ChildNodes[0].Term.Name.ToString().Equals("contarAsig") || nodo.ChildNodes[0].Term.Name.ToString().Equals("llamada") || nodo.ChildNodes[0].Term.Name.ToString().Equals("rutaB"))
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }
                        else
                        {

                            if (nodo.ChildNodes[0].Token.Terminal.Name.Equals("entero"))
                            {

                                string aux = nodo.ChildNodes[0].Token.Text;

                                if (aux.Contains("."))
                                {
                                    resultado = "doble;" + nodo.ChildNodes[0].Token.Text;
                                }
                                else
                                {
                                    resultado = nodo.ChildNodes[0].Token.Terminal.Name + ";" + nodo.ChildNodes[0].Token.Text;
                                }
                            }
                            else
                            {
                                resultado = nodo.ChildNodes[0].Token.Terminal.Name + ";" + nodo.ChildNodes[0].Token.Text;
                            }

                            
                        }

                        break;
                    }

                case "llamada":
                    {

                        if (nodo.ChildNodes.Count == 4)
                        {

                            string proc = ActuarSQL(nodo.ChildNodes[0]);

                            string param = ActuarSQL(nodo.ChildNodes[2]);

                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("rutaB"))
                            {
                                resultado = ActuarSQL(nodo.ChildNodes[0]);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("RFECHA_HORA"))
                            {
                                resultado = DateTime.Now.ToString("g");
                            }
                            else
                            {
                                resultado = DateTime.Now.ToString("d");
                            }
                        }

                        break;
                    }

                case "rutaB":
                    {

                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]) + ",";

                            resultado += nodo.ChildNodes[2].Token.Text;

                        }
                        else
                        {
                            resultado += nodo.ChildNodes[0].Token.Text;
                        }


                        break;
                    }

                case "insertar":
                    {

                        TablaAux = nodo.ChildNodes[3].Token.Text;

                        string Datos = ActuarSQL(nodo.ChildNodes[4]);

                        break;
                    }

                case "tipoins":
                    {

                        if (nodo.ChildNodes.Count == 8)
                        {
                            tipoin = 1;

                            resultado = ActuarSQL(nodo.ChildNodes[1]) + ";";
                            resultado += ActuarSQL(nodo.ChildNodes[5]);

                        }
                        else
                        {
                            tipoin = 0;
                            resultado += ActuarSQL(nodo.ChildNodes[1]);
                        }

                        break;
                    }

                case "campos":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = nodo.ChildNodes[0].Token.Text + ",";
                            resultado += ActuarSQL(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = nodo.ChildNodes[0].Token.Text;
                        }

                        break;
                    }

                case "valores":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);                           
                            resultado += ActuarSQL(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "actualizar":
                    {

                        TablaAux = nodo.ChildNodes[2].Token.Text;

                        string camposA = ActuarSQL(nodo.ChildNodes[4]);

                        string valorA = ActuarSQL(nodo.ChildNodes[8]);

                        if (nodo.ChildNodes.Count == 12)
                        {
                            string condicion = ActuarSQL(nodo.ChildNodes[10]);

                            //Actualiza con condicion
                        }
                        else
                        {
                            //Actualiza completo
                        }


                        break;
                    }

                case "condicion":
                    {



                        break;
                    }

                case "borrar":
                    {

                        TablaAux = nodo.ChildNodes[3].Token.Text;


                        if (nodo.ChildNodes.Count == 6)
                        {
                            string condicion = ActuarSQL(nodo.ChildNodes[10]);

                            //Borrar con condicion
                        }
                        else
                        {
                            //Borrar completo
                        }

                        break;
                    }

                case "aritemtica":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[1].Term.Name.ToString().Equals("SUMA"))
                            {

                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo="";
                                string re = "";

                                if (OP1[0].Equals("entero"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        int aux = Convert.ToInt32(OP1[1]) + Convert.ToInt32(OP2[1]);
                                        tipo = "entero";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) + Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("Cadena"))
                                    {

                                        string a = OP2[1];
                                        a = a.Replace("\"", "");
                                        tipo= "Cadena";
                                        re = "\"" + OP1[1] + OP2[1] + "\"";
                                    }                                    
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }
                                }
                                else if (OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) + Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) + Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("Cadena"))
                                    {

                                        string a = OP2[1];
                                        a = a.Replace("\"", "");
                                        tipo = "Cadena";
                                        re = "\"" + OP1[1] + OP2[1] + "\"";
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }
                                }
                                else if (OP1[0].Equals("Cadena"))
                                {
                                    if(OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {
                                        tipo = "Cadena";
                                        re = "\"" + OP1[1] + OP2[1] + "\"";
                                    }
                                    else if (OP2[0].Equals("Cadena"))
                                    {
                                        string a = OP1[1];
                                        a = a.Replace("\"", "");

                                        tipo = "Cadena";
                                        re = "\"" + OP1[1] + OP2[1] + "\"";
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";   
                                    }
                                }


                                    resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("RESTA"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo="";
                                string re = "";

                                if (OP1[0].Equals("entero"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        int aux = Convert.ToInt32(OP1[1]) - Convert.ToInt32(OP2[1]);
                                        tipo = "entero";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) - Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else if (OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) - Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) - Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else
                                {
                                    tipo = "Error";
                                    re = "Error tipos Incompatibles";
                                }
                                resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("DIV"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";

                                if (OP1[0].Equals("entero"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        int aux = Convert.ToInt32(OP1[1]) / Convert.ToInt32(OP2[1]);
                                        tipo = "entero";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) / Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else if (OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) / Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) / Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else
                                {
                                    tipo = "Error";
                                    re = "Error tipos Incompatibles";
                                }

                                resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MULTI"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";

                                if (OP1[0].Equals("entero"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        int aux = Convert.ToInt32(OP1[1]) * Convert.ToInt32(OP2[1]);
                                        tipo = "entero";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) * Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                if (OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) * Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else if (OP2[0].Equals("doble"))
                                    {
                                        double aux = Convert.ToDouble(OP1[1]) * Convert.ToDouble(OP2[1]);
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else
                                {
                                    tipo = "Error";
                                    re = "Error tipos Incompatibles";
                                }

                                resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("POTENCIA"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";

                                if (OP1[0].Equals("entero"))
                                {
                                    if (OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {
                                        double aux = Math.Pow(Convert.ToDouble(OP1[1]), Convert.ToDouble(OP2[1]));
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else if (OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {
                                        double aux = Math.Pow(Convert.ToDouble(OP1[1]), Convert.ToDouble(OP2[1]));
                                        tipo = "doble";
                                        re = aux.ToString();
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }

                                }
                                else
                                {
                                    tipo = "Error";
                                    re = "Error tipos Incompatibles";
                                }

                                resultado = tipo + ";" + re;
                            }
                            else
                            {
                                resultado = ActuarSQL(nodo.ChildNodes[1]);
                            }


                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = "-" + ActuarSQL(nodo.ChildNodes[1]);

                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

            }

            return resultado;
        }
    }
}

