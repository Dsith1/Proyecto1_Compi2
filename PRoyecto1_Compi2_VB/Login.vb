Imports System.IO
Imports System.Net.Sockets
Imports System.Text

Public Class Login
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim usuario, contra, salida As String

        usuario = TextBox1.Text
        contra = TextBox2.Text

        salida = "[ ""validar"": 1500, ""login"":[ ""comando"" => 'seleccionar * de usuarios donde usuario = """ + usuario + """"
        salida = salida + " && password = """ + contra + """']]$"
        UserActual = usuario

        checkNo(salida)

    End Sub

    Private Sub checkNo(cadena As String)



        Dim bufferEscritura As Byte()
        Dim Stm As Stream
        Dim tcpClnt As TcpClient = New TcpClient()

        Dim login As Boolean

        tcpClnt.Connect("192.168.0.17", 8000)
        Stm = tcpClnt.GetStream()

        If (Stm.CanWrite) Then
            bufferEscritura = Encoding.UTF8.GetBytes(cadena)
            If Not Stm Is Nothing Then
                Stm.Write(bufferEscritura, 0, bufferEscritura.Length)

                Dim data(0 To 256 - 1) As Byte

                ''String to store the response ASCII representation.
                Dim responseData As String = String.Empty

                ''Read the first batch of the TcpServer response bytes.
                Dim bytes As Int32 = Stm.Read(data, 0, data.Length)

                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes)
                Console.WriteLine("Received: {0}", responseData)

                If (responseData.Contains("true")) Then

                    login = True


                Else
                    login = False
                    UserActual = ""
                End If
            End If

        End If

        tcpClnt.Close()
        tcpClnt = Nothing

        If (login) Then

            Dim principal As New Form1
            principal.Show()
            Me.Hide()

        Else

            MessageBox.Show("Usuario/Contraseña Erroneos")



        End If



    End Sub


End Class