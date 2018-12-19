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

                Salida = "[ ""paquete"":""usql"", ""instrucción"":'" + Cadena + "',]"




                'Cadena += 
                'bytes = Nothing
                'Dim x As Integer
                'x = Cadena.Length
                'Dim j As Integer = x / 256
                'If (x Mod 256 <> 0) Then
                '    j = j + 1
                'End If
                'Dim codigo As String = TextBox1.Text
                'For cont As Integer = 0 To j - 1
                '    Dim salida As String
                '    Dim inicio As String = 256 * cont
                '    If (cont = j - 1) Then
                '        Dim final As Integer = x Mod 256
                '        salida = codigo.Substring(inicio, final)
                '    Else
                '        salida = codigo.Substring(inicio, 256)
                '    End If
                '    cliente = New TcpClient
                '    Try
                '        cliente.Connect(IPAddress.Parse("192.168.0.17"), 6000)
                '    Catch ex As Exception
                '        MessageBox.Show("IMPOSIBLE CONECTAR CON SERVIDOR", "ERROR")
                '    End Try
                '    If cliente.Connected = True Then
                '        leer_escribir = cliente.GetStream
                '        bw = New BinaryWriter(leer_escribir)
                '        bw.Write(TextBox1.Text)
                '    End If
                'Next cont

            Else
                MessageBox.Show("Ha habido un Error" + SError, "Error")
            End If

        End If
    End Sub


End Class
