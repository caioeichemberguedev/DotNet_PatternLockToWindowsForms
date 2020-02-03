Public Class Form2

    Private Sub TrocarSenhaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TrocarSenhaToolStripMenuItem.Click
        Form1.B_TrocaSenha = True
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub VoltarParaLoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VoltarParaLoginToolStripMenuItem.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
    End Sub
End Class