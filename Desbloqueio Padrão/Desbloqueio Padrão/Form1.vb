Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Button1.Location = New Point(Me.Width / 2 - Button1.Width / 2, 10)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Form3.SenhaCadastrada2.Count = 0 Then
            Form3.Login = True
            Form2.Show()
        Else
            Form3.Text = "Login"
            Form3.Show()
        End If
    End Sub

End Class
