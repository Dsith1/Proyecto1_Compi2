Imports System.IO
Imports PRoyecto1_Compi2_VB.MyParser


Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (TextBox1.Text.Equals("")) Then
            MessageBox.Show("No hay nada que analizar", "Error")
        Else
            MyParser.Setup()

            If (MyParser.Parse(New StringReader(TextBox1.Text))) Then
                MessageBox.Show("Exito")

            Else
                MessageBox.Show("Ha habido un Error" + SError, "Error")
            End If

        End If
    End Sub
End Class
