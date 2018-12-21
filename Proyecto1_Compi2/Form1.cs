using Irony.Parsing;
using Proyecto1_Compi2.Analizadores;
using Proyecto1_Compi2.Ejecucion;
using Proyecto1_Compi2.Elementos;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;


namespace Proyecto1_Compi2
{
    public partial class Form1 : Form
    {

        string graph = "";
        string cadena;
        bool escuchar = true;
        string BaseActual;
        string UserActaul= "Admin";
        string alteraraux;
        string TablaAux;
        int tipoin;
        Manejo manejo;
        Entorno Global;
        Entorno Eactual;
        int contadorN=0;        
        string codigo;
        Thread hilo;
        string recibo = "";

        public Form1()
        {
            InitializeComponent();
            Global = new Entorno(1);
            Global.nombre = "Global";
            Eactual = Global;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            BaseActual = "";
            manejo = new Manejo();
            manejo.usuario = UserActaul;
            string Consola = txtConsola.Text;

            string activeDir = @"c:\DBMS";

            string newPath = System.IO.Path.Combine(activeDir, "Maestro.usac");

            if (File.Exists(newPath) == false)
            {
                Consola += manejo.Crear_Maestro();

                Consola += "\r\n" + manejo.Crear_Usuario("Admin", "1234");

                txtConsola.Text = Consola;

            }
            else
            {
                manejo.LeerInfoTxt(newPath);
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

            codigo = cadenaEntrada;
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

        public string esCadenaValidSQL2(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);

            codigo = cadenaEntrada;
            string a = "";

            a = ActuarSQL(arbol.Root);


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
            //Creamos el delegado 
            ThreadStart delegado = new ThreadStart(CorrerProceso);
            //Creamos la instancia del hilo 
            hilo = new Thread(delegado);
            //Iniciamos el hilo 
            hilo.Start();



            cadena = textBox1.Text;
          
            
            //AnalizarPaquete(cadena);


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

                        string p = nodo.ChildNodes[0].Span.Location.ToString();

                        int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                        string instrucciones =codigo.Substring(inicio - 1, nodo.ChildNodes[0].Span.Length);

                        if (instrucciones.Contains("BACKUP")|| instrucciones.Contains("RESTAURAR"))
                        {

                        }
                        else
                        {
                            manejo.Add_historial(instrucciones, BaseActual);
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

                            resultado = "\r\n"+manejo.Crear_Tabla(TablaAux, BaseActual, campos);

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
                                resultado = " " + aux;
                            }
                            else if (resultado.Equals("NO"))
                            {
                                resultado += ActuarSQL(nodo.ChildNodes[1]);
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
                            resultado += nodo.ChildNodes[1].Token.Text;
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
                            contadorN++;
                            string nentorno = "procedimiento_" + contadorN;

                            Entorno nuevo = new Entorno(1);
                            nuevo.nombre = nentorno;

                            Eactual.Hijo = nuevo;
                            nuevo.Padre = Eactual;

                            Eactual = nuevo;

                            if (nodo.ChildNodes.Count == 8)
                            {
                                TablaAux = nodo.ChildNodes[1].Token.Text+"_";

                                string campos = ActuarSQL(nodo.ChildNodes[3]);

                                string[] campo = campos.Split(';');

                                for(int x = 0; x < campo.Length; x++)
                                {
                                    TablaAux += campo[x].Split(',')[0] + "_";
                                }

                                TablaAux = TablaAux.Trim('_');

                                string p = nodo.ChildNodes[6].Span.Location.ToString();

                                int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                                string instrucciones = codigo.Substring(inicio-1, nodo.ChildNodes[6].Span.Length+1);

                                resultado = manejo.Crear_Procedimiento(TablaAux, BaseActual, campos, instrucciones);
                            }
                            else
                            {
                                TablaAux = nodo.ChildNodes[1].Token.Text + "_";

                                string p = nodo.ChildNodes[5].Span.Location.ToString();

                                int inicio =Convert.ToInt32( p.Split(':')[1].Trim(')'));

                                string instrucciones = codigo.Substring(inicio-1, nodo.ChildNodes[5].Span.Length+1);

                                resultado = manejo.Crear_Procedimiento(TablaAux, BaseActual, "", instrucciones);

                            }

                            Eactual = Eactual.Padre;
                            Eactual.Hijo = null;

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

                case "instruccion":
                    {

                        if (nodo.ChildNodes[0].Term.Name.ToString().Equals("RDETENER"))
                        {
                            int validarD = DetenerValido();

                            if (validarD == -1)
                            {
                                resultado="Break Erroneo en "+nodo.ChildNodes[0].Token.Location;
                            }
                            else
                            {
                                setDetener(validarD);
                            }
                            
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

                        resultado =ActuarSQL(nodo.ChildNodes[2]);

                        if (resultado.Contains("Error")){
                            if (resultado.Contains(";"))
                            {
                                string[] val = resultado.Split(';');

                                resultado = "\r\nError al Imprimir " + val[0];
                            }
                        }
                        else
                        {
                            if (resultado.Contains(";"))
                            {
                                string[] val = resultado.Split(';');

                                resultado = val[1];
                            }
                            else if (resultado.Contains("@"))
                            {
                                resultado=Get_VariableV(resultado);
                                string[] val = resultado.Split(';');

                                resultado = val[1];
                            }
                        }

                        resultado = "\r\n" + resultado;


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

                            param = Remplazo_tipos(param);

                            string[] parametros = param.Split(',');
                            string val = "";

                            for(int x = 0; x < parametros.Length; x++)
                            {
                                string[] dato = parametros[x].Split(';');

                                proc += "_" + dato[0];
                                val += dato[1] + ",";
                            }


                            val = val.Trim(',');

                            string codigo = manejo.EjecutarProcedimeinto(proc, BaseActual);
                            string detalles = manejo.ProcedimientoParam(proc, BaseActual);

                            if (manejo.EsFuncion(proc, BaseActual))
                            {

                                Entorno nuevo = new Entorno(1);
                                nuevo.nombre = "funcion_";

                                Eactual.Hijo = nuevo;
                                nuevo.Padre = Eactual;

                                Eactual = nuevo;

                                string tipo = manejo.getTipoF(proc, BaseActual);

                                Variable variable = new Variable(tipo, "Retorno");

                                Eactual.variables.Insertar(variable);

                                string[] detalle = detalles.Split(';');

                                string[] var = detalle[0].Split(',');
                                string[] tipos = detalle[1].Split(',');

                                string[] valor = val.Split(',');

                                for (int y = 0; y < var.Length; y++)
                                {
                                    Variable variablep = new Variable(tipos[y], var[0]);
                                    variablep.SetValor(valor[y]);

                                    nuevo.variables.Insertar(variablep);

                                }


                                Analizador_Procedimientos gramatica = new Analizador_Procedimientos();

                                esCadenaValidSQL2(codigo, gramatica);

                                Eactual.variables.existe("Retorno");

                                resultado = Eactual.variables.aux.GetValor();

                                Eactual = Eactual.Padre;
                                Eactual.Hijo = null;
                            }
                            else
                            {
                                Entorno nuevo = new Entorno(1);
                                nuevo.nombre = "procedimiento_";

                                Eactual.Hijo = nuevo;
                                nuevo.Padre = Eactual;

                                Eactual = nuevo;

                                string[] detalle = detalles.Split(';');

                                string[] var = detalle[0].Split(',');
                                string[] tipos= detalle[1].Split(',');

                                string[] valor = val.Split(',');

                                for(int y = 0; y < var.Length; y++)
                                {
                                    Variable variable = new Variable(tipos[y], var[0]);
                                    variable.SetValor(valor[y]);

                                    nuevo.variables.Insertar(variable);

                                }




                                Analizador_Procedimientos gramatica = new Analizador_Procedimientos();

                                resultado = esCadenaValidSQL2(codigo, gramatica);

                                Eactual = Eactual.Padre;
                                Eactual.Hijo = null;
                            }


                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("rutaB"))
                            {
                                
                                string proc = ActuarSQL(nodo.ChildNodes[0])+"_";

                                string codigo=manejo.EjecutarProcedimeinto(proc, BaseActual);


                                if (manejo.EsFuncion(proc, BaseActual))
                                {

                                    Entorno nuevo = new Entorno(1);
                                    nuevo.nombre = "funcion_";

                                    Eactual.Hijo = nuevo;
                                    nuevo.Padre = Eactual;

                                    Eactual = nuevo;

                                    string tipo = manejo.getTipoF(proc, BaseActual);

                                    Variable variable = new Variable(tipo, "Retorno");

                                    Eactual.variables.Insertar(variable);

                                    Analizador_Procedimientos gramatica = new Analizador_Procedimientos();

                                    esCadenaValidSQL2(codigo, gramatica);

                                    Eactual.variables.existe("Retorno");

                                    resultado = Eactual.variables.aux.GetValor();

                                    Eactual = Eactual.Padre;
                                    Eactual.Hijo = null;
                                }
                                else
                                {
                                    Entorno nuevo = new Entorno(1);
                                    nuevo.nombre = "procedimiento_";

                                    Eactual.Hijo = nuevo;
                                    nuevo.Padre = Eactual;

                                    Eactual = nuevo;

                                    Analizador_Procedimientos gramatica = new Analizador_Procedimientos();

                                    resultado = esCadenaValidSQL2(codigo, gramatica);

                                    Eactual = Eactual.Padre;
                                    Eactual.Hijo = null;
                                }

                                
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

                            
                            resultado = nodo.ChildNodes[0].Token.Text;

                            if (resultado.Contains("@"))
                            {
                                resultado = Get_VariableV(resultado);
                            }
                        }


                        break;
                    }

                case "insertar":
                    {

                        TablaAux = nodo.ChildNodes[3].Token.Text;

                        string Datos = ActuarSQL(nodo.ChildNodes[4]);

                        if (tipoin == 0)
                        {
                           resultado="\r\n"+ manejo.Insertar_Tabla(Datos, BaseActual, TablaAux);
                        }
                        else
                        {
                            string[] aux = Datos.Split('$');

                            string valor = aux[0];
                            string datos = aux[1];

                            resultado = "\r\n" + manejo.Insertar_Tabla_v(datos,valor, BaseActual, TablaAux);
                        }

                        

                        break;
                    }

                case "tipoins":
                    {

                        if (nodo.ChildNodes.Count == 8)
                        {
                            tipoin = 1;

                            resultado = ActuarSQL(nodo.ChildNodes[1]) + "$";
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
                            resultado = ActuarSQL(nodo.ChildNodes[0]) + ",";
                            resultado += ActuarSQL(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "valores":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0])+ ",";                           
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

                            string p = nodo.ChildNodes[10].Span.Location.ToString();

                            int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                            string instrucciones = codigo.Substring(inicio + 5, nodo.ChildNodes[10].Span.Length - 5);

                            instrucciones = instrucciones.Trim(';');
                            instrucciones = instrucciones.Trim();

                            string[] instruccion = instrucciones.Split(new string[] { "||", "&&" }, StringSplitOptions.None);

                            for (int x = 0; x < instruccion.Length; x++)
                            {
                                string temporal = instruccion[x];
                                instruccion[x] = CambiarVariablesVal(instruccion[x]);

                                instrucciones = instrucciones.Replace(temporal, instruccion[x]);
                            }



                            resultado = manejo.Actualizar_Tabla(BaseActual, TablaAux, camposA, valorA, instrucciones);
                        }
                        else
                        {
                            resultado = manejo.Actualizar_Tabla(BaseActual, TablaAux, camposA, valorA, "");
                        }


                        break;
                    }

                case "borrar":
                    {

                        TablaAux = nodo.ChildNodes[3].Token.Text;


                        if (nodo.ChildNodes.Count == 6)
                        {
                            string p = nodo.ChildNodes[4].Span.Location.ToString();

                            int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                            string instrucciones = codigo.Substring(inicio + 5, nodo.ChildNodes[4].Span.Length - 5);

                            instrucciones = instrucciones.Trim(';');
                            instrucciones = instrucciones.Trim();

                            string[] instruccion = instrucciones.Split(new string[] { "||", "&&" }, StringSplitOptions.None);

                            for (int x = 0; x < instruccion.Length; x++)
                            {
                                string temporal = instruccion[x];
                                instruccion[x] = CambiarVariablesVal(instruccion[x]);

                                instrucciones = instrucciones.Replace(temporal, instruccion[x]);
                            }

                            resultado = manejo.Borrar_Tabla(BaseActual, TablaAux, instrucciones);
                        }
                        else
                        {
                            resultado = manejo.Borrar_Tabla(BaseActual, TablaAux, "");
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

                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }
                               

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }
                               

                                term2 = Remplazo_operaciones(term2);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo="";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

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
                                        re = "Error tipos Incompatibles ";
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
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo="";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

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
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();


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
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

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
                                    else if (OP2[0].Equals("Cadena"))
                                    {
                                        tipo = "Error tipos Incompatibles  (entero * Cadena)";
                                        re = "Error";
                                    }
                                    else
                                    {
                                       
                                    }

                                }
                                else if (OP1[0].Equals("doble"))
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
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

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
                            string term1 = ActuarSQL(nodo.ChildNodes[1]);
                            if (term1.Contains("@"))
                            {
                                term1 = Get_VariableV(term1);
                            }

                            term1 = Remplazo_operaciones(term1);

                            
                            string[] OP1 = term1.Split(';');

                            if (OP1[0].Equals("entero") || OP1[0].Equals("doble"))
                            {
                                resultado = OP1[0] + ";-" + OP1[1];
                            }
                            else
                            {
                                resultado = "ERROR";
                                
                            }

                            

                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "relacional":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[1].Term.Name.ToString().Equals("IGUAL"))
                            {

                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                if (OP1[0].Equals("entero")|| OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero")|| OP2[0].Equals("doble"))
                                    {

                                        tipo = "bool";
                                        if (OP1[1].Equals(OP2[1]))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }
                                        
                                        
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles ";
                                    }
                                }
                                else if (OP1[0].Equals("Cadena"))
                                {
                                    if (OP2[0].Equals("Cadena"))
                                    {
                                        tipo = "bool";
                                        if (OP1[1].Equals(OP2[1]))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }
                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles";
                                    }
                                }


                                resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("DISTINTO"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                tipo = "bool";
                                if (OP1[1]!=OP2[1])
                                {
                                    re = "1";
                                }
                                else
                                {
                                    re = "0";
                                }

                                resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MENOR"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";

                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                if (OP1[0].Equals("entero") || OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {
                                        
                                        tipo = "bool";
                                        if (Convert.ToDouble(OP1[1])< Convert.ToDouble(OP2[1]))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }
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
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MAYOR"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                if (OP1[0].Equals("entero") || OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {

                                        tipo = "bool";
                                        if (Convert.ToDouble(OP1[1]) > Convert.ToDouble(OP2[1]))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }
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
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MENOR_IGUAL"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                if (OP1[0].Equals("entero") || OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {

                                        tipo = "bool";
                                        if (Convert.ToDouble(OP1[1]) <= Convert.ToDouble(OP2[1]))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }
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
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MAYOR_IGUAL"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                if (term1.Contains("@"))
                                {
                                    term1 = Get_VariableV(term1);
                                }

                                term1 = Remplazo_operaciones(term1);

                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                if (term2.Contains("@"))
                                {
                                    term2 = Get_VariableV(term2);
                                }

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                if (OP1[0].Equals("entero") || OP1[0].Equals("doble"))
                                {
                                    if (OP2[0].Equals("entero") || OP2[0].Equals("doble"))
                                    {

                                        tipo = "bool";
                                        if (Convert.ToDouble(OP1[1]) >= Convert.ToDouble(OP2[1]))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }
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
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "logica":
                    {


                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[1].Term.Name.ToString().Equals("AND"))
                            {

                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";
                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();


                                if (OP1[0].Equals("bool"))
                                {
                                    if (OP2[0].Equals("bool"))
                                    {

                                        
                                        tipo = "bool";
                                        if (OP1[1].Equals("1") && OP2[1].Equals("1"))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }


                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles ";
                                    }
                                }
                                else
                                {
                                    tipo = "Error";
                                    re = "Error tipos Incompatibles";
                                }


                                resultado = tipo + ";" + re;
                            }
                            else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("OR"))
                            {
                                string term1 = ActuarSQL(nodo.ChildNodes[0]);
                                string term2 = ActuarSQL(nodo.ChildNodes[2]);

                                string[] OP1 = term1.Split(';');
                                string[] OP2 = term2.Split(';');

                                string tipo = "";
                                string re = "";

                                OP1[0] = OP1[0].Trim();
                                OP2[0] = OP2[0].Trim();

                                if (OP1[0].Equals("bool"))
                                {
                                    if (OP2[0].Equals("bool"))
                                    {


                                        tipo = "bool";
                                        if (OP1[1].Equals("1") || OP2[1].Equals("1"))
                                        {
                                            re = "1";
                                        }
                                        else
                                        {
                                            re = "0";
                                        }


                                    }
                                    else
                                    {
                                        tipo = "Error";
                                        re = "Error tipos Incompatibles ";
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
                            string term1 = ActuarSQL(nodo.ChildNodes[1]);

                            string[] OP1 = term1.Split(';');

                            string tipo = "";
                            string re = "";

                            OP1[0] = OP1[0].Trim();

                            if (OP1[0].Equals("bool"))
                            {
                                tipo = "bool";
                                if (OP1[1].Equals("1") )
                                {
                                    re = "0";
                                }
                                else
                                {
                                    re = "1";
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
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "if":
                    {
                        Entorno nuevo = new Entorno(1);
                        nuevo.nombre = "if";

                        Eactual.Hijo = nuevo;
                        nuevo.Padre = Eactual;
                        Eactual = Eactual.Hijo;

                        string precondicion= ActuarSQL(nodo.ChildNodes[2]);

                        string[] conaux = precondicion.Split(';');

                        string condicion = conaux[1];

                        if (nodo.ChildNodes.Count ==8)
                        {
                            if (condicion.Equals("1"))
                            {
                                resultado = ActuarSQL(nodo.ChildNodes[5]);
                            }
                            else
                            {
                                resultado = ActuarSQL(nodo.ChildNodes[7]);
                            }
                        }
                        else
                        {
                            if (condicion.Equals("1"))
                            {
                                resultado = ActuarSQL(nodo.ChildNodes[5]);
                            }
                            else
                            {
                                resultado = "";
                            }
                        }

                        Eactual = Eactual.Padre;
                        Eactual.Hijo = null;

                        break;
                    }

                case "sino":
                    {
                        Entorno nuevo = new Entorno(1);
                        Eactual.Hijo = nuevo;
                        nuevo.Padre = Eactual;

                        Eactual = Eactual.Hijo;

                        resultado = ActuarSQL(nodo.ChildNodes[2]);

                        Eactual = Eactual.Padre;
                        Eactual.Hijo = null;
                        break;
                    }

                case "while":
                    {
                        string condicion = ActuarSQL(nodo.ChildNodes[2]).Split(';')[1];

                        Entorno nuevo = new Entorno(1);
                        contadorN++;
                        nuevo.nombre = "while_" + contadorN;

                        Eactual.Hijo = nuevo;
                        nuevo.Padre = Eactual;

                        Eactual = nuevo;


                        while (condicion.Equals("1") && Eactual.detener==false)
                        {
                            resultado+= ActuarSQL(nodo.ChildNodes[5]);

                            condicion = ActuarSQL(nodo.ChildNodes[2]).Split(';')[1];
                        }

                        Eactual = Eactual.Padre;
                        Eactual.Hijo = null;

                        break;
                    }

                case "for":
                    {
                        contadorN++;
                        string nombren = "for_" + contadorN;
                        Entorno nuevo = new Entorno(1);
                        nuevo.nombre = nombren;

                        Eactual.Hijo = nuevo;
                        nuevo.Padre = Eactual;

                        Eactual = Eactual.Hijo;

                        string subidr = nodo.ChildNodes[3].Token.Text;
                        string subval = ActuarSQL(nodo.ChildNodes[6]);

                        string[] subsval = subval.Split(';');

                        Variable variable = new Variable(nodo.ChildNodes[4].Token.Text, subidr);

                        variable.SetValor(subsval[1]);

                        Eactual.variables.Insertar(variable);

                        string condicion = ActuarSQL(nodo.ChildNodes[8]).Split(';')[1];

                        while (condicion.Equals("1"))
                        {
                            resultado += ActuarSQL(nodo.ChildNodes[13]);

                            

                            string opcionF= ActuarSQL(nodo.ChildNodes[10]);

                            if (opcionF.Equals("++"))
                            {

                                Eactual.variables.existe(subidr);
                                string subv = Eactual.variables.aux.GetValor();
                                string[] subsv = subv.Split(';');

                                int tempF = Convert.ToInt32(subsv[1]);
                                tempF++;

                                Eactual.variables.aux.SetValor(tempF.ToString());

                            }
                            else
                            {
                                Eactual.variables.existe(subidr);
                                string subv = Eactual.variables.aux.GetValor();
                                string[] subsv = subv.Split(';');

                                int tempF = Convert.ToInt32(subsv[1]);
                                tempF--;

                                Eactual.variables.aux.SetValor(tempF.ToString());
                            }

                            condicion = ActuarSQL(nodo.ChildNodes[8]).Split(';')[1];
                        }

                        Eactual = Eactual.Padre;
                        Eactual.Hijo = null;

                        break;
                    }
                    
                case "asignacion":
                    {
                        if (nodo.ChildNodes[0].Term.Name.ToString().Equals("rutaB"))
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[0]);
                        }
                        else
                        {
                            string var = nodo.ChildNodes[0].Token.Text;
                            string valor= ActuarSQL(nodo.ChildNodes[2]);

                            bool seguirV = true;
                            bool EncontradoV = false;

                            if (valor.Contains("@"))
                            {
                                valor = Get_VariableV(valor);
                            }

                            Entorno aux = Eactual;

                            while (seguirV)
                            {
                                if (aux.variables.existe(var))
                                {
                                    seguirV = false;
                                    EncontradoV = true;
                                }
                                else
                                {
                                    if (aux.Padre != null)
                                    {
                                        aux = aux.Padre;
                                    }
                                    else
                                    {
                                        seguirV = false;
                                    }
                                }
                            }

                            if (EncontradoV)
                            {

                                bool compatible;

                                valor = Remplazo_tipos(valor);

                                string[] conaux = valor.Split(';');

                                string ntipo = conaux[0];

                                string tipo = aux.variables.aux.tipo;

                                if (tipo.Equals(ntipo))
                                {
                                    compatible = true;
                                }
                                else if (tipo.Equals("DOUBLE"))
                                {
                                    if (ntipo.Equals("INTEGER"))
                                    {
                                        compatible = true;
                                    }
                                    else
                                    {
                                        compatible = false;
                                    }
                                }
                                else
                                {
                                    compatible = false;
                                }

                                if (compatible)
                                {
                                    aux.variables.aux.SetValor(conaux[1]);
                                }
                                else
                                {
                                    resultado = "\r\nError de tipos";
                                }
                            }
                            else
                            {
                                resultado = "\n\rNo Existe la variable " + var;
                            }

                            
                        }

                        break;
                    }

                case "declarar":
                    {

                        string vars = ActuarSQL(nodo.ChildNodes[1]);
                        string[] nombre = vars.Split(',');

                        if (nodo.ChildNodes.Count == 6)
                        {

                            string valor = ActuarSQL(nodo.ChildNodes[4]);

                            valor = Remplazo_tipos(valor);

                            string[] data = valor.Split(';');

                            string tipo = ActuarSQL(nodo.ChildNodes[2]);

                            bool compatible;

                            if (tipo.Equals(data[0]))
                            {
                                compatible = true;
                            }
                            else if (tipo.Equals("DOUBLE"))
                            {
                                if (data[0].Equals("INTEGER"))
                                {
                                    compatible = true;
                                }
                                else
                                {
                                    compatible = false;
                                }
                            }
                            else
                            {
                                compatible = false;
                            }




                            if (compatible)
                            {
                                for (int x = 0; x < nombre.Length; x++)
                                {
                                    Variable nuevo = new Variable(tipo, nombre[x]);
                                    nuevo.SetValor(data[1]);

                                    if (Eactual.variables.existe(nombre[x]))
                                    {
                                        resultado += "\r\nYa Existe la Variable " + nombre[x];
                                    }
                                    else
                                    {
                                        Eactual.variables.Insertar(nuevo);
                                    }

                                }
                            }
                            else
                            {
                                resultado = "\r\nError tipos incompatibles";
                            }

                        }
                        else
                        {

                            string tipo;
                            

                            bool existeO;

                            if (nodo.ChildNodes[2].Term.Name.ToString().Equals("tipo_dato"))
                            {
                                tipo = ActuarSQL(nodo.ChildNodes[2]);
                                existeO = true;
                            }
                            else
                            {
                                tipo = nodo.ChildNodes[2].Token.Text;

                                if (BaseActual.Equals(""))
                                {
                                    existeO = false;
                                }
                                else
                                {
                                    existeO = manejo.Buscar_Objeto(tipo,BaseActual);
                                }
                                
                            }


                            if (existeO)
                            {
                                for (int x = 0; x < nombre.Length; x++)
                                {
                                    Variable nuevo = new Variable(tipo, nombre[x]);


                                    if (Eactual.variables.existe(nombre[x]))
                                    {
                                        resultado += "\r\nYa Existe la Variable " + nombre[x];
                                    }
                                    else
                                    {
                                        Eactual.variables.Insertar(nuevo);
                                    }

                                }
                            }
                            else
                            {
                                resultado = "\r\nError no Existe el Objeto " + tipo;
                            }
                            


                                
                        }

                        break;
                    }

                case "variables":
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

                case "opciones_for":
                    {
                        resultado = nodo.ChildNodes[0].Token.Text;
                        break;
                    }

                case "switch":
                    {
                        Entorno nuevo = new Entorno(1);
                        nuevo.nombre = "switch";


                        Eactual.Hijo = nuevo;
                        nuevo.Padre = Eactual;

                        Eactual = nuevo;

                        resultado = Actuarswitch(nodo.ChildNodes[5], nodo.ChildNodes[2]);

                        Eactual = Eactual.Padre;
                        Eactual.Hijo = null;

                        break;
                    }

                case "defecto":
                    {
                        resultado = ActuarSQL(nodo.ChildNodes[2]);

                        break;
                    }

                case "alterar":
                    {
                        alteraraux = nodo.ChildNodes[2].Token.Text;

                        if (nodo.ChildNodes.Count ==8)
                        {
                            string nuevo_pass= nodo.ChildNodes[6].Token.Text;
                            nuevo_pass = nuevo_pass.Trim('\"');

                            resultado = manejo.Modificar_Usuario(alteraraux, nuevo_pass);
                        }
                        else
                        {
                            resultado = ActuarSQL(nodo.ChildNodes[3]);
                        }

                        break;
                    }

                case "alterarobjeto":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string campos= ActuarSQL(nodo.ChildNodes[2]);

                            resultado = manejo.Agregar_Objeto(alteraraux, BaseActual, campos);
                        }
                        else
                        {
                            string campos= ActuarSQL(nodo.ChildNodes[1]);

                            string[] campo = campos.Split(',');

                            for(int x = 0; x < campo.Length; x++)
                            {
                               resultado= "\r\n" + manejo.Quitar_Objeto(alteraraux, BaseActual, campo[x]);
                            }
                            
                        }

                        break;
                    }

                case "alterartabla":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string campos = ActuarSQL(nodo.ChildNodes[2]);

                            resultado = manejo.Agregar_Tabla(alteraraux, BaseActual, campos);
                        }
                        else
                        {
                            string campos = ActuarSQL(nodo.ChildNodes[1]);

                            string[] campo = campos.Split(',');

                            for (int x = 0; x < campo.Length; x++)
                            {
                                resultado = "\r\n" + manejo.Quitar_Tabla(alteraraux, BaseActual, campo[x]);
                            }

                        }

                        break;
                    }

                case "c_funcion":
                    {
                        if (nodo.ChildNodes.Count == 9)
                        {
                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string tipo;

                            if (nodo.ChildNodes[5].Term.Name.ToString().Equals("tipo_dato"))
                            {
                                tipo = ActuarSQL(nodo.ChildNodes[5]);
                            }
                            else
                            {
                                tipo = nodo.ChildNodes[5].Token.Text;
                            }

                            string campos = ActuarSQL(nodo.ChildNodes[3]);

                            string[] campo = campos.Split(';');

                            for (int x = 0; x < campo.Length; x++)
                            {
                                TablaAux += campo[x].Split(',')[0] + "_";
                            }

                            TablaAux = TablaAux.Trim('_');

                            string p = nodo.ChildNodes[7].Span.Location.ToString();

                            int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                            string instrucciones = codigo.Substring(inicio - 1, nodo.ChildNodes[7].Span.Length + 1);

                            manejo.Crear_funcion(nombre, BaseActual, campos, instrucciones, tipo);

                        }
                        else
                        {
                            string nombre= nodo.ChildNodes[1].Token.Text;

                            string tipo;

                            if (nodo.ChildNodes[4].Term.Name.ToString().Equals("tipo_dato"))
                            {
                                tipo = ActuarSQL(nodo.ChildNodes[4]);
                            }
                            else
                            {
                                tipo= nodo.ChildNodes[4].Token.Text;
                            }

                            string p = nodo.ChildNodes[6].Span.Location.ToString();

                            int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                            string instrucciones = codigo.Substring(inicio - 1, nodo.ChildNodes[6].Span.Length + 1);

                            manejo.Crear_funcion(nombre, BaseActual, "", instrucciones, tipo);

                        }
                        
                        break;
                    }

                case "retorno":
                    {

                        string retornar = ActuarSQL(nodo.ChildNodes[1]);

                        bool seguirV = true;
                        bool EncontradoV = false;

                        Entorno aux = Eactual;

                        while (seguirV)
                        {
                            if (aux.variables.existe("Retorno"))
                            {
                                seguirV = false;
                                EncontradoV = true;
                            }
                            else
                            {
                                if (aux.Padre != null)
                                {
                                    aux = aux.Padre;
                                }
                                else
                                {
                                    seguirV = false;
                                }
                            }
                        }

                        if (EncontradoV)
                        {

                            bool compatible;

                            retornar = Remplazo_tipos(retornar);

                            string[] conaux = retornar.Split(';');

                            string ntipo = conaux[0];

                            string tipo = aux.variables.aux.tipo;

                            if (tipo.Equals(ntipo))
                            {
                                compatible = true;
                            }
                            else if (tipo.Equals("DOUBLE"))
                            {
                                if (ntipo.Equals("INTEGER"))
                                {
                                    compatible = true;
                                }
                                else
                                {
                                    compatible = false;
                                }
                            }
                            else
                            {
                                compatible = false;
                            }

                            if (compatible)
                            {
                                aux.variables.aux.SetValor(conaux[1]);
                            }
                            else
                            {
                                resultado = "\r\nError de tipos";
                            }

                        }

                            break;
                    }

                case "eliminar":
                    {
                        string nombre = nodo.ChildNodes[2].Token.Text;

                        if (nodo.ChildNodes[1].Term.Name.ToString().Equals("RTABLA"))
                        {
                            resultado = manejo.Eliminar_Tabla(nombre, BaseActual);
                        }
                        else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("RBASE"))
                        {
                            resultado = manejo.Eliminar_Base(nombre);
                        }
                        else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("ROBJETO"))
                        {
                            resultado = manejo.Eliminar_Objeto(nombre, BaseActual);
                        }
                        else
                        {
                            resultado = manejo.Eliminar_Usuario(nombre);
                        }


                            break;
                    }

                case "seleccionar":
                    {

                        bool todo;
                        string campos = "";

                        if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MULTI"))
                        {
                            todo = true;
                        }
                        else
                        {
                            todo = false;
                            campos= ActuarSQL(nodo.ChildNodes[1]);

                        }

                        string tablas = ActuarSQL(nodo.ChildNodes[3]);

                        if (nodo.ChildNodes.Count == 7)
                        {
                            string p = nodo.ChildNodes[4].Span.Location.ToString();

                            int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                            string instrucciones = codigo.Substring(inicio + 5, nodo.ChildNodes[4].Span.Length - 5);

                            instrucciones = instrucciones.Trim(';');
                            instrucciones = instrucciones.Trim();

                            string[] instruccion = instrucciones.Split(new string[] { "||", "&&" }, StringSplitOptions.None);

                            for (int x = 0; x < instruccion.Length; x++)
                            {
                                string temporal = instruccion[x];
                                instruccion[x] = CambiarVariablesVal(instruccion[x]);

                                instrucciones = instrucciones.Replace(temporal, instruccion[x]);
                            }



                            string orden = ActuarSQL(nodo.ChildNodes[5]);

                            if (todo)
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*", orden, instrucciones);
                            }
                            else
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, orden, instrucciones);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[4].Term.Name.ToString().Equals("orden"))
                            {
                                string orden = ActuarSQL(nodo.ChildNodes[4]);

                                if (todo)
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*",orden, "");
                                }
                                else
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, orden, "");
                                }


                            }
                            else
                            {
                                string p = nodo.ChildNodes[4].Span.Location.ToString();

                                int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                                string instrucciones = codigo.Substring(inicio +5, nodo.ChildNodes[4].Span.Length-5);

                                instrucciones = instrucciones.Trim(';');
                                instrucciones = instrucciones.Trim();

                                string[] instruccion = instrucciones.Split(new string[] { "||", "&&" }, StringSplitOptions.None);

                                for(int x = 0; x < instruccion.Length; x++)
                                {
                                    string temporal = instruccion[x];
                                    instruccion[x] = CambiarVariablesVal(instruccion[x]);

                                    instrucciones = instrucciones.Replace(temporal, instruccion[x]);
                                }

                                

                                if (todo)
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*", "",instrucciones);
                                }
                                else
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, "", instrucciones);
                                }

                            }
                        }
                        else {

                            if (todo)
                            {
                                resultado = "\r\n"+manejo.Select(BaseActual, tablas, "*", "", "");
                            }
                            else
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, "", "");
                            }

                        }


                        break;
                    }

                case "orden":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string campo = nodo.ChildNodes[1].Token.Text;
                            string orden= nodo.ChildNodes[2].Token.Text;

                            resultado = campo + " " + orden;

                        }
                        else
                        {
                            string campo= nodo.ChildNodes[1].Token.Text;

                            resultado = campo + " ASC";
                        }

                            break;
                    }

                case "contar":
                    {

                        string seleccion = ActuarSQL(nodo.ChildNodes[3]);

                        string[] pre= seleccion.Split(new string[] { ";;" }, StringSplitOptions.None);

                        int conteo = pre[1].Split(';').Length;

                        resultado = "entero;" + conteo;

                        break;
                    }

                case "contarAsig":
                    {

                        string seleccion = ActuarSQL(nodo.ChildNodes[3]);
                        string[] pre = seleccion.Split(new string[] { ";;" }, StringSplitOptions.None);

                        int conteo = pre[1].Split(';').Length;

                        resultado = "entero;" + conteo;
                        break;
                    }

                case "seleccionarf":
                    {

                        bool todo;
                        string campos = "";

                        if (nodo.ChildNodes[1].Term.Name.ToString().Equals("MULTI"))
                        {
                            todo = true;
                        }
                        else
                        {
                            todo = false;
                            campos = ActuarSQL(nodo.ChildNodes[1]);

                        }

                        string tablas = ActuarSQL(nodo.ChildNodes[3]);

                        if (nodo.ChildNodes.Count == 6)
                        {
                            string p = nodo.ChildNodes[4].Span.Location.ToString();

                            int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                            string instrucciones = codigo.Substring(inicio + 5, nodo.ChildNodes[4].Span.Length - 5);

                            instrucciones = instrucciones.Trim(';');
                            instrucciones = instrucciones.Trim();

                            string[] instruccion = instrucciones.Split(new string[] { "||", "&&" }, StringSplitOptions.None);

                            for (int x = 0; x < instruccion.Length; x++)
                            {
                                string temporal = instruccion[x];
                                instruccion[x] = CambiarVariablesVal(instruccion[x]);

                                instrucciones = instrucciones.Replace(temporal, instruccion[x]);
                            }



                            string orden = ActuarSQL(nodo.ChildNodes[5]);

                            if (todo)
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*", orden, instrucciones);
                            }
                            else
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, orden, instrucciones);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            if (nodo.ChildNodes[4].Term.Name.ToString().Equals("orden"))
                            {
                                string orden = ActuarSQL(nodo.ChildNodes[4]);

                                if (todo)
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*", orden, "");
                                }
                                else
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, orden, "");
                                }


                            }
                            else
                            {
                                string p = nodo.ChildNodes[4].Span.Location.ToString();

                                int inicio = Convert.ToInt32(p.Split(':')[1].Trim(')'));

                                string instrucciones = codigo.Substring(inicio + 5, nodo.ChildNodes[4].Span.Length - 5);

                                instrucciones = instrucciones.Trim(';');
                                instrucciones = instrucciones.Trim();

                                string[] instruccion = instrucciones.Split(new string[] { "||", "&&" }, StringSplitOptions.None);

                                for (int x = 0; x < instruccion.Length; x++)
                                {
                                    string temporal = instruccion[x];
                                    instruccion[x] = CambiarVariablesVal(instruccion[x]);

                                    instrucciones = instrucciones.Replace(temporal, instruccion[x]);
                                }



                                if (todo)
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*", "", instrucciones);
                                }
                                else
                                {
                                    resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, "", instrucciones);
                                }

                            }
                        }
                        else
                        {

                            if (todo)
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, "*", "", "");
                            }
                            else
                            {
                                resultado = "\r\n" + manejo.Select(BaseActual, tablas, campos, "", "");
                            }

                        }


                        break;
                    }

                case "back":
                    {

                        string auxbase= nodo.ChildNodes[2].Token.Text;
                        string archivof = ActuarSQL(nodo.ChildNodes[3]);

                        

                        if (nodo.ChildNodes[1].Term.Name.ToString().Equals("RUSQLDUMP"))
                        {
                            if (archivof.Contains(".USQLDUMP") == false)
                            {
                                archivof += ".USQLDUMP";
                            }


                            manejo.imprimir_historial(auxbase, archivof);
                        }
                        else
                        {
                            if (archivof.Contains(".zip") == false)
                            {
                                archivof += ".zip";
                            }

                            manejo.Comprimir(auxbase, archivof);
                        }

                        break;
                    }

                case "restaurar":
                    {
                        string archivof = nodo.ChildNodes[2].Token.Text;



                        if (nodo.ChildNodes[1].Term.Name.ToString().Equals("RUSQLDUMP"))
                        {
                            if (archivof.Contains(".USQLDUMP") == false)
                            {
                                resultado = "Error en archivo";
                            }
                            else
                            {
                                string codigoR = manejo.LeerUsql(archivof.Trim('"'));                                

                                Analizar(codigoR);

                                resultado = "\r\nLA Base "+BaseActual+" ha sido Restaurada";
                            }
                        }
                        else
                        {
                            if (archivof.Contains(".zip") == false)
                            {
                                resultado = "Error en archivo";
                            }
                            else
                            {
                                manejo.Descomprimir(archivof.Trim('"'));

                                string[] files = Directory.GetFiles(@"C:\DBMS","*_objetos*");

                                string[] filtro1 = files[0].Split('_');

                                string[] filtro2 = filtro1[0].Split('\\');

                                string Nbase = filtro2[2];

                                manejo.lectura = 1;
                                manejo.Crear_Base(Nbase);

                                manejo.LeerInfoTxt(@"C:\DBMS\"+Nbase+".usac");
                                manejo.lectura = 0;

                                manejo.commit();

                                resultado = "\r\nLA Base " + Nbase + " ha sido Restaurada";




                            }

                            
                        }
                        break;
                    }
            }

            return resultado;
        }

        public string Remplazo_tipos(string entrada)
        {
            entrada = entrada.Replace("entero", "INTEGER");
            entrada = entrada.Replace("doble", "DOUBLE");
            entrada = entrada.Replace("Cadena", "TEXT");
            entrada = entrada.Replace("bool", "BOOL");
           
            return entrada;
        }

        public string Remplazo_operaciones(string entrada)
        {
            entrada = entrada.Replace("INTEGER", "entero");
            entrada = entrada.Replace("DOUBLE", "doble");
            entrada = entrada.Replace("TEXT", "Cadena");
            entrada = entrada.Replace("BOOL", "bool");

            return entrada;
        }

        string Get_VariableV(string var)
        {
            string respuesta = "";
            Entorno aux = Eactual;

            bool seguir = true;

            while (seguir)
            {
                if (aux.variables.existe(var))
                {                    

                    respuesta = aux.variables.aux.GetValor();
                    seguir = false;
                }
                else
                {
                    if (aux.Padre != null)
                    {
                        aux = aux.Padre;
                    }
                    else
                    {
                        seguir = false;
                    }
                }
            }


            return respuesta;
        }

        string Actuarswitch(ParseTreeNode nodo, ParseTreeNode nodocondicion)
        {
            string resultado="";

            switch (nodo.Term.Name.ToString())
            {
                case "casos":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = Actuarswitch(nodo.ChildNodes[0], nodocondicion);

                            resultado += Actuarswitch(nodo.ChildNodes[1], nodocondicion);
                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("defecto"))
                            {
                                if (Eactual.defectoS)
                                {
                                    resultado = ActuarSQL(nodo.ChildNodes[0]);
                                }
                                

                            }
                            else
                            {
                                resultado = Actuarswitch(nodo.ChildNodes[0], nodocondicion);
                            }
                        }
                        break;

                    }

                case "caso":
                    {
                        
                        string caso = ActuarSQL(nodo.ChildNodes[1]);

                        string validacion= ActuarSQL(nodocondicion);

                        if (caso.Equals(validacion)||Eactual.continuarSwitch && Eactual.detener==false)
                        {
                            Eactual.continuarSwitch = true;
                            Eactual.defectoS = false;
                            resultado = ActuarSQL(nodo.ChildNodes[3]);

                        }
                        else if(Eactual.detener == true)
                        {
                            
                        }
                        else
                        {
                            Eactual.defectoS = true;
                        }


                        break;
                    }
            }


            return resultado;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        int DetenerValido()
        {
            int resultado = 0;

            bool seguir = true;

            Entorno aux = Eactual;

            while (seguir)
            {
                if ( aux.nombre.Contains("while") || aux.nombre.Contains("for")|| aux.nombre.Contains("switch"))
                {
                    resultado++;
                    seguir = false;
                }
                else
                {
                    if(aux.Padre== null)
                    {
                        seguir = false;
                        resultado= - 1;
                        
                    }
                    else
                    {
                        aux = aux.Padre;
                        resultado++;
                    }
                }
            }


            return resultado;
        }

        void setDetener(int niveles)
        {
            Entorno aux = Eactual;
            for(int x = 0; x < niveles;x++)
            {
                aux.detener = true;
                aux = aux.Padre;
            }
        }

        string CambiarVariablesVal(string entrada)
        {

            var Match = Regex.Match(entrada, "@[a-zA-Z]+([a-zA-Z0-9_])*");

            for (int x = 0; x < Match.Groups.Count; x++)
            {
                string a=Match.Groups[x].Value;

                if (a != "")
                {
                    string b = Get_VariableV(a);

                    entrada = entrada.Replace(a, b);
                }

            }

            return entrada;
        }

        private void CorrerProceso()
        {
            while (escuchar)
            {
                Thread.Sleep(100);

                Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sck.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.17"), 8000));
                sck.Listen(0);

                Socket acc = sck.Accept();


                byte[] buffer = new byte[255];
                int rec = acc.Receive(buffer, 0, buffer.Length, 0);

                Array.Resize(ref buffer, rec);

                Console.WriteLine("Recibido {0}", Encoding.UTF8.GetString(buffer));
                recibo += Encoding.UTF8.GetString(buffer);

                buffer = Encoding.Default.GetBytes("hola cliente");
                acc.Send(buffer, 0, buffer.Length, 0);

                sck.Close();
                acc.Close();

                if (recibo.Contains("]$"))
                {
                    recibo = recibo.Replace("]$", "]");

                    AnalizarPaquete(recibo);
                    cadena = recibo;

                    recibo = "";

                }

            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            escuchar = false;
        }
    }
}

