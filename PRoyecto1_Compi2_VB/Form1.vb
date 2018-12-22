Imports System.IO
Imports PRoyecto1_Compi2_VB.MyParserUSQL
Imports PRoyecto1_Compi2_VB.MyParser
Imports System.Net
Imports System.Net.Sockets
Imports System.Text



Public Class Form1

    Public cliente As TcpClient
    Public bytes() As Byte = Nothing
    Public leer_escribir As NetworkStream
    Dim br As BinaryReader
    Dim bw As BinaryWriter

    Dim Cadena As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Lectura("C:\DBMS\Maestro.usac")

        TreeView1.ExpandAll()


    End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If (TextBox1.Text.Equals("")) Then
            MessageBox.Show("No hay nada que analizar", "Error")
        Else

            MyParser.Setup()
            MyParserUSQL.Setup()

            If (MyParserUSQL.Parse(New StringReader(TextBox1.Text))) Then
                Cadena = TextBox1.Text

                Cadena = Replace(Cadena, "USAR", "USAR ")
                Cadena = Replace(Cadena, "CREAR", "CREAR ")
                Cadena = Replace(Cadena, "RETORNO", "RETORNO ")
                Cadena = Replace(Cadena, "IMPRIMIR", "IMPRIMIR ")
                Cadena = Replace(Cadena, "INSERTAR", "INSERTAR ")
                Cadena = Replace(Cadena, "DECLARAR", "DECLARAR ")
                Cadena = Replace(Cadena, "IMPRIMIR", "IMPRIMIR ")
                Cadena = Replace(Cadena, "SI(", "SI (")
                Cadena = Replace(Cadena, "SINO", "SINO ")
                Cadena = Replace(Cadena, "PARA", "PARA ")
                Cadena = Replace(Cadena, "MIENTRAS", "MIENTRAS ")
                Cadena = Replace(Cadena, "INTEGER", "INTEGER ")
                Cadena = Replace(Cadena, "DOUBLE", "DOUBLE ")
                Cadena = Replace(Cadena, "BOOL", "BOOL ")
                Cadena = Replace(Cadena, "TEXT", "TEXT ")
                Cadena = Replace(Cadena, "DATE", "DATE ")
                Cadena = Replace(Cadena, "DATETIME", "DATETIME ")
                Cadena = Replace(Cadena, "DATE TIME", "DATETIME")
                Cadena = Replace(Cadena, "no ", "NO ")
                Cadena = Replace(Cadena, "nO ", "NO ")
                Cadena = Replace(Cadena, "No ", "NO ")
                Cadena = Replace(Cadena, "no m", "nom")
                Cadena = Replace(Cadena, "nO m", "nOm")
                Cadena = Replace(Cadena, "No m", "Nom")
                Cadena = Replace(Cadena, "no M", "noM")
                Cadena = Replace(Cadena, "nO M", "nOM")
                Cadena = Replace(Cadena, "No M", "NoM")
                Cadena = Replace(Cadena, "nulo", "NULO")
                Cadena = Replace(Cadena, "nulO", "NULO")
                Cadena = Replace(Cadena, "nuLo", "NULO")
                Cadena = Replace(Cadena, "nUlo", "NULO")
                Cadena = Replace(Cadena, "Nulo", "NULO")
                Cadena = Replace(Cadena, "NULo", "NULO")
                Cadena = Replace(Cadena, "NUlO", "NULO")
                Cadena = Replace(Cadena, "NuLO", "NULO")
                Cadena = Replace(Cadena, "nulo", "NULO")
                Cadena = Replace(Cadena, "SELECCIONA", "SELECCIONA ")
                Cadena = Replace(Cadena, "CASO", "CASO ")
                Cadena = Replace(Cadena, "DEFECTO", "DEFECTO ")
                Cadena = Replace(Cadena, "SELECCIONA R", "SELECCIONAR ")
                Cadena = Replace(Cadena, "DETENER", "DETENER ")
                Cadena = Replace(Cadena, "ALTERAR", "ALTERAR ")
                Cadena = Replace(Cadena, "USUARIO", "USUARIO ")
                Cadena = Replace(Cadena, "VALORES", "VALORES ")
                Cadena = Replace(Cadena, "ELIMINAR", "ELIMINAR ")
                Cadena = Replace(Cadena, "CONTAR", "CONTAR ")
                Cadena = Replace(Cadena, "ACTUALIZAR", "ACTUALIZAR ")
                Cadena = Replace(Cadena, "USQLDUMP", "USQLDUMP ")
                Cadena = Replace(Cadena, "BACKUP", "BACKUP ")
                Cadena = Replace(Cadena, "=", "= ")
                Cadena = Replace(Cadena, "= =", "== ")
                Cadena = Replace(Cadena, ">", "> ")
                Cadena = Replace(Cadena, "> =", ">= ")
                Cadena = Replace(Cadena, "> >", ">>")
                Cadena = Replace(Cadena, "<", "< ")
                Cadena = Replace(Cadena, "< =", "<= ")
                Cadena = Replace(Cadena, "< <", "<<")


                Cadena = Replace(Replace(Cadena, Chr(10), " "), Chr(13), " ")







                Dim Salida As String

                Salida = "[ ""paquete"":""usql"", ""instrucción"":'" + Cadena + "',]$"


                checkNo(Salida)

                TreeView1.Nodes.Clear()


                Lectura("C:\DBMS\Maestro.usac")

                TreeView1.ExpandAll()


            ElseIf (MyParser.Parse(New StringReader(TextBox1.Text))) Then

                Cadena = TextBox1.Text
                Dim busqueda() As String = Cadena.Split(New String() {"USQL"}, StringSplitOptions.None)

                Dim seleccion As String = busqueda(1)

                BusquedaReporte = busqueda(1)

                seleccion = seleccion.Trim("/")
                seleccion = seleccion.Trim("<")
                seleccion = seleccion.Trim(">")
                seleccion = seleccion.Trim()

                Dim Salida As String

                Salida = "[""paquete"": ""reporte"",""instrucción"": '" + seleccion + "',]$"

                checkNo(Salida)

                TreeView1.Nodes.Clear()
                Lectura("C:\DBMS\Maestro.usac")
                TreeView1.ExpandAll()

            Else
                MessageBox.Show("Ha habido un Error" + SError, "Error")
            End If

        End If
    End Sub

    Private Sub checkNo(cadena As String)


        Dim caracteres As Integer = cadena.Length

        Dim vueltas As Integer = caracteres / 255

        Dim extra As Integer = caracteres Mod 255

        If (extra <> 0) Then
            vueltas = vueltas - 1

        End If

        For x As Integer = 0 To vueltas - 1

            Dim multiplo = x * 255
            If (x <> 0) Then
                multiplo = multiplo - 1
            End If

            Dim envio = cadena.Substring(multiplo, 255)
            Dim bufferEscritura As Byte()
            Dim Stm As Stream
            Dim tcpClnt As TcpClient = New TcpClient()

            tcpClnt.Connect("192.168.0.17", 8000)
            Stm = tcpClnt.GetStream()

            If (Stm.CanWrite) Then
                bufferEscritura = Encoding.UTF8.GetBytes(envio)
                If Not Stm Is Nothing Then
                    Stm.Write(bufferEscritura, 0, bufferEscritura.Length)

                    Dim data(0 To 256 - 1) As Byte

                    ''String to store the response ASCII representation.
                    Dim responseData As String = String.Empty

                    ''Read the first batch of the TcpServer response bytes.
                    'Dim bytes As Int32 = Stm.Read(data, 0, data.Length)

                    'responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
                    'Console.WriteLine("Received: {0}", responseData)


                End If
            End If

            tcpClnt.Close()
            tcpClnt = Nothing


        Next x

        If (vueltas = -1) Then

            vueltas = 0
        End If


        Dim envio2 = cadena.Substring(vueltas * 255, extra)
        Dim bufferEscritura2 As Byte()
        Dim Stm2 As Stream
        Dim tcpClnt2 As TcpClient = New TcpClient()

        tcpClnt2.Connect("192.168.0.17", 8000)
        Stm2 = tcpClnt2.GetStream()

        If (Stm2.CanWrite) Then
            bufferEscritura2 = Encoding.UTF8.GetBytes(envio2)
            If Not Stm2 Is Nothing Then
                Stm2.Write(bufferEscritura2, 0, bufferEscritura2.Length)

                Dim data(0 To 256 - 1) As Byte

                ''String to store the response ASCII representation.
                Dim responseData As String = String.Empty

                ''Read the first batch of the TcpServer response bytes.
                Dim seguir As Boolean = True

                Dim entrada As String = ""

                While (seguir)
                    Dim bytes As Int32 = Stm2.Read(data, 0, data.Length)

                    responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes)
                    entrada = entrada + responseData
                    Console.WriteLine("Received: {0}", responseData)

                    If (entrada.Contains("]$")) Then
                        seguir = False


                        If (entrada.Contains("reporte")) Then

                            Dim resultados() As String = entrada.Split(New String() {"table"}, StringSplitOptions.None)

                            TextBox1.Text = TextBox1.Text.Replace(BusquedaReporte, resultados(1))

                            TextBox1.Text = TextBox1.Text.Replace("USQL", "table")

                            Dim ruta As String = "C:\Reportes\Reporte.html"
                            Dim escritor As StreamWriter
                            File.Delete(ruta)
                            escritor = File.AppendText(ruta)
                            escritor.Write(TextBox1.Text)
                            escritor.Flush()
                            escritor.Close()

                        End If

                        TextBox2.AppendText("Recibiendo Paquete" & Environment.NewLine)
                        TextBox2.AppendText(entrada & Environment.NewLine)

                        If (TextBox2.Text.Contains("Restaurada")) Then
                            Lectura("C:\DBMS\Maestro.usac")
                        End If

                    End If

                End While


            End If
        End If

        tcpClnt2.Close()
        tcpClnt2 = Nothing



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim bufferEscritura As Byte()
        Dim Stm As Stream
        Dim tcpClnt As TcpClient = New TcpClient()

        tcpClnt.Connect("192.168.0.17", 8000)
        Stm = tcpClnt.GetStream()

        If (Stm.CanWrite) Then
            bufferEscritura = Encoding.UTF8.GetBytes("[""paquete"": ""fin""]")
            If Not Stm Is Nothing Then
                Stm.Write(bufferEscritura, 0, bufferEscritura.Length)

                Dim data(0 To 256 - 1) As Byte

                ''String to store the response ASCII representation.
                Dim responseData As String = String.Empty

                ''Read the first batch of the TcpServer response bytes.
                'Dim bytes As Int32 = Stm.Read(data, 0, data.Length)

                'responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
                'Console.WriteLine("Received: {0}", responseData)


            End If
        End If

        tcpClnt.Close()
        tcpClnt = Nothing


        Dim Login As New Login

        Login.Show()
        Me.Hide()

    End Sub

    Private Sub Lectura(ruta As String)

        Dim objReader As New StreamReader(ruta)
        Dim sLine As String = ""
        Dim nombre, parametros, funcion, tipos As String
        Dim tipo As Integer = 0
        Dim lugar As Integer
        funcion = ""
        tipos = ""
        parametros = ""

        Do
            sLine = objReader.ReadLine()
            If Not sLine Is Nothing Then
                If sLine = "<DB>" Then

                    tipo = 1

                ElseIf sLine.Contains("<Procedure>") Then

                    lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                    TreeView1.Nodes.Item(lugar).Nodes.Add("Procedimientos_" + BaseActual, "Procedimientos")

                ElseIf sLine.Contains("<Object>") Then

                    lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                    TreeView1.Nodes.Item(lugar).Nodes.Add("Objetos_" + BaseActual, "Objetos")

                ElseIf sLine.Contains("<Proc>") Then

                    tipo = 2

                ElseIf sLine.Contains("<Obj>") Then

                    tipo = 2

                ElseIf sLine.Contains("<Tabla>") Then

                    If (tipo <> 3) Then
                        lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                        TreeView1.Nodes.Item(lugar).Nodes.Add("Tablas_" + BaseActual, "Tablas")

                        tipo = 3
                    End If



                ElseIf sLine.Contains("<nombre>") Then



                    If tipo = 1 Then
                        nombre = sLine.Substring(10, sLine.Length - 19)
                        BaseActual = nombre
                        TreeView1.Nodes.Add(nombre, nombre)

                        tipo = 0

                    ElseIf tipo = 2 Then

                        nombre = sLine.Substring(10, sLine.Length - 19)

                    ElseIf tipo = 3 Then

                        nombre = sLine.Substring(10, sLine.Length - 19)

                        lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                        Dim sublugar As Integer = TreeView1.Nodes.Item(lugar).Nodes.IndexOfKey("Tablas_" + BaseActual)
                        TreeView1.Nodes.Item(lugar).Nodes.Item(sublugar).Nodes.Add(nombre, nombre)



                    End If

                ElseIf sLine.Contains("<path>") Then

                    Dim subruta As String = sLine.Substring(10, sLine.Length - 19)

                    Lectura(subruta)

                ElseIf sLine.Contains("<src>") Then

                    lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                    Dim sublugar As Integer = TreeView1.Nodes.Item(lugar).Nodes.IndexOfKey("Procedimientos_" + BaseActual)

                    TreeView1.Nodes.Item(lugar).Nodes.Item(sublugar).Nodes.Add(nombre, nombre + "(" + parametros + ")")

                    parametros = ""
                    tipos = ""

                ElseIf sLine.Contains("</attr>") Then

                    lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                    Dim sublugar As Integer = TreeView1.Nodes.Item(lugar).Nodes.IndexOfKey("Objetos_" + BaseActual)

                    TreeView1.Nodes.Item(lugar).Nodes.Item(sublugar).Nodes.Add(nombre, nombre + "(" + parametros + ")")

                    parametros = ""
                    tipos = ""

                ElseIf sLine.Contains("<tipo>") Then

                ElseIf sLine.Contains("<params>") Then

                ElseIf sLine.Contains("<Historia>") Then

                Else

                    If tipo = 3 Then

                        If sLine.Contains("<INTEGER") Then

                            Dim fin_inicio As Integer = sLine.IndexOf(">") + 1
                            Dim fin_campo As Integer = sLine.IndexOf("</")

                            Dim campo = sLine.Substring(fin_inicio, fin_campo - fin_inicio)

                            campo = campo.Trim("@")

                            lugar = TreeView1.Nodes.IndexOfKey(BaseActual)
                            Dim sublugar As Integer = TreeView1.Nodes.Item(lugar).Nodes.IndexOfKey("Tablas_" + BaseActual)

                            'TreeView1.Nodes.Item(lugar).Nodes.Item(sublugar).Nodes.Add(campo, campo)


                        End If

                    Else

                        If sLine.Contains("<INTEGER>") Then

                            If (tipos = "") Then

                                tipos = tipos + sLine.Substring(7, 7)
                                parametros += tipos + " " + sLine.Substring(17, sLine.Length - 29)


                            Else
                                tipos = tipos + "_" + sLine.Substring(7, 7)
                                parametros += "," + sLine.Substring(7, 7) + " " + sLine.Substring(17, sLine.Length - 29)


                            End If

                        ElseIf sLine.Contains("<TEXT>") Then

                            If (tipos = "") Then

                                tipos = tipos + sLine.Substring(7, 4)
                                parametros += tipos + " " + sLine.Substring(14, sLine.Length - 23)


                            Else
                                tipos = tipos + "_" + sLine.Substring(7, 4)
                                parametros += "," + sLine.Substring(7, 4) + " " + sLine.Substring(14, sLine.Length - 23)


                            End If

                        End If


                    End If


                End If

                End If

        Loop Until sLine Is Nothing
        objReader.Close()


    End Sub
End Class
