Imports System.IO
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

    End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If (TextBox1.Text.Equals("")) Then
            MessageBox.Show("No hay nada que analizar", "Error")
        Else

            MyParser.Setup()

            If (MyParser.Parse(New StringReader(TextBox1.Text))) Then
                MessageBox.Show("Exito")
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




                TextBox1.Text = Cadena


                Dim Salida As String

                Salida = "[ ""paquete"":""usql"", ""instrucción"":'" + Cadena + "',]$"


                checkNo(Salida)



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
                'Dim bytes As Int32 = Stm.Read(data, 0, data.Length)

                'responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
                'Console.WriteLine("Received: {0}", responseData)


            End If
        End If

        tcpClnt2.Close()
        tcpClnt2 = Nothing



    End Sub


End Class
