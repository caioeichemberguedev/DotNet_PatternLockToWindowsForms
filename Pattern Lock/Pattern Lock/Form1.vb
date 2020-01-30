Imports System.Threading.Thread

Public Class Form1

    Private Lista_Tentativa As New List(Of Integer)
    Private Lista_Senha As New List(Of Integer)

    Private ListPoints As New List(Of Point)
    Const Tamanho As Integer = 15
    Private MousePoint As Point
    Private C_PatternConnectionColor As Color = Color.DeepSkyBlue
    Private N_Quantidade_Tentativas As Integer = 5

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigTela()
        ConfigPontos()
        Tentativa()
    End Sub

    Private Sub ConfigTela()
        Me.CenterToScreen()
    End Sub

    Private Sub ConfigPontos()

        Dim Colunas As Integer = 0
        Dim Linhas As Integer = 0
        Const Distancia As Integer = 80

        For k = 0 To 8
            Dim Ponto As Point = New Point(50 + Colunas * Distancia, Distancia + Linhas * (Distancia * 1.2)) ' 20% +
            ListPoints.Add(Ponto)
            If Colunas = 2 Then
                Linhas += 1
                Colunas = 0
            Else
                Colunas += 1
            End If

            Me.Refresh()
            'Sleep(300)
        Next

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        MousePoint = New Point(e.X, e.Y)
        ' Label1.Text = MousePoint.ToString
        VerificaPosicao(MousePoint)
        PictureBox1.Refresh()
    End Sub

    Private Function VerificaPosicao(ByVal Valor As Point) As Boolean
        Dim ValorLimite As Integer = Tamanho / 1.5
        For k = 0 To ListPoints.Count - 1
            If Valor.X < ListPoints(k).X + ValorLimite And Valor.X > ListPoints(k).X - ValorLimite And Valor.Y < ListPoints(k).Y + ValorLimite And Valor.Y > ListPoints(k).Y - ValorLimite Then
                If Lista_Tentativa.Contains(k) = False Then
                    Lista_Tentativa.Add(k)
                    If Lista_Tentativa.Count = 9 Then Tentativa()
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Sub DrawListPoints(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'DESENHA LINHAS ENTRE PONTOS SELECIONADOS E MOUSE
        Dim MyPenToLine As Pen = New Pen(Color.WhiteSmoke, Tamanho / 2)
        MyPenToLine.EndCap = Drawing2D.LineCap.RoundAnchor
        If Lista_Tentativa.Count = 1 Then
            e.Graphics.DrawLine(MyPenToLine, ListPoints(Lista_Tentativa(0)), MousePoint)
        ElseIf Lista_Tentativa.Count >= 2 Then
            For k = 0 To Lista_Tentativa.Count - 2
                e.Graphics.DrawLine(MyPenToLine, ListPoints(Lista_Tentativa(k)), ListPoints(Lista_Tentativa(k + 1)))
                If Lista_Tentativa.Count < 9 And k = Lista_Tentativa.Count - 2 Then e.Graphics.DrawLine(MyPenToLine, ListPoints(Lista_Tentativa(k + 1)), MousePoint)
            Next
        End If

        'DESENHA PONTOS SELECIONADOS
        Dim MyPenToSelects As Pen = New Pen(C_PatternConnectionColor, 5)
        For k = 0 To Lista_Tentativa.Count - 1
            Dim MyRect As Rectangle = New Rectangle(ListPoints(Lista_Tentativa(k)).X - 20, ListPoints(Lista_Tentativa(k)).Y - 20, 40, 40)
            e.Graphics.DrawEllipse(MyPenToSelects, MyRect)
            e.Graphics.FillEllipse(Brushes.Black, MyRect)
        Next

        'DESENHA PONTOS SELECIONAVEIS
        For k = 0 To ListPoints.Count - 1
            Dim MyRect As Rectangle = New Rectangle(ListPoints(k).X - (Tamanho / 2), ListPoints(k).Y - (Tamanho / 2), Tamanho, Tamanho)
            e.Graphics.FillEllipse(Brushes.White, MyRect)
        Next
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Tentativa()
    End Sub

    Private Sub Tentativa()
        If Lista_Tentativa.Count > 4 And Lista_Senha.Count = 0 Then
            For k = 0 To Lista_Tentativa.Count - 1
                Lista_Senha.Add(Lista_Tentativa(k))
            Next
            Label1.Text = "Senha de " & Lista_Senha.Count & " digitos criada"
            C_PatternConnectionColor = Color.Yellow
            Label1.ForeColor = C_PatternConnectionColor
            Refresh()
            Sleep(2000)
            Lista_Tentativa.Clear()
            Label1.Text = "Desenhe sua senha para logar"
            C_PatternConnectionColor = Color.DeepSkyBlue
            Label1.ForeColor = C_PatternConnectionColor
        ElseIf Lista_Tentativa.Count > 0 And Lista_Senha.Count > 0 Then
            If Lista_Senha.SequenceEqual(Lista_Tentativa) = True Then
                C_PatternConnectionColor = Color.Green
                Label1.Text = "Senha correta, Logando..."
                Label1.ForeColor = C_PatternConnectionColor
                Refresh()
                Sleep(2000)
                Label1.Text = "Desenhe sua senha para logar"
                C_PatternConnectionColor = Color.DeepSkyBlue
                Label1.ForeColor = C_PatternConnectionColor
                Lista_Tentativa.Clear()
                Form2.Show()
                Me.Hide()
            Else
                C_PatternConnectionColor = Color.Red
                N_Quantidade_Tentativas -= 1
                Label1.Text = "Senha Incorreta, [" & N_Quantidade_Tentativas & "] tentativas restantes"
                Label1.ForeColor = C_PatternConnectionColor
                Refresh()
                Sleep(2000)
                Lista_Tentativa.Clear()
                Label1.Text = "Desenhe sua senha para logar"
                C_PatternConnectionColor = Color.DeepSkyBlue
                Label1.ForeColor = C_PatternConnectionColor
            End If
        Else
            C_PatternConnectionColor = Color.Red
            Label1.Text = "Use no mínimo 5 pontos"
            Label1.ForeColor = C_PatternConnectionColor
            Refresh()
            Sleep(2000)
            Lista_Tentativa.Clear()
            C_PatternConnectionColor = Color.DeepSkyBlue
            Label1.ForeColor = C_PatternConnectionColor
            Label1.Text = "Crie sua senha"
        End If
    End Sub
End Class
