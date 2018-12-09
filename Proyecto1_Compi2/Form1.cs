using Irony.Parsing;
using Proyecto1_Compi2.Analizadores;
using System;
using System.Diagnostics;
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                    GenarbolC(arbol.Root);
                    GenerateGraphC("Entrada.txt", "C:/Fuentes/");

                }
            }
            else
            {
                if (arbol.Root != null)
                {
                    GenarbolC(arbol.Root);
                    GenerateGraphC("Entrada.txt", "C:/Fuentes/");

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
            cadena = null;


            /*
            while (escuchar)
            {
                Conectar();
            }

            */
            Analizar(cadena);

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
    }
}
