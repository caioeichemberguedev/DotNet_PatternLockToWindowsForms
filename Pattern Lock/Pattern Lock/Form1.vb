Imports System.Threading.Thread

Public Class Form1

    Private Lista_Tentativa As New List(Of Integer)
    Private Lista_Senha As New List(Of Integer)

    Private ListPoints As New List(Of Point)
    Const N_Tamanho As Integer = 15
    Private P_MousePoint As Point
    Private C_PatternConnectionColor As Color = Color.DeepSkyBlue
    Private N_Quantidade_Tentativas As Integer = 3
    Const N_Qtde_Pontos As Integer = 9
    Private B_PermiteDesenhoPontoMouse As Boolean = True
    Private B_AutorizaTarefasMouse As Boolean = True
    Public B_TrocaSenha As Boolean = False
    Private B_CriouSenha As Boolean = False
    Private B_TentativaDeLoginIncorreta As Boolean = False
    Private N_TempoDeBloqueio As Integer = 10

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Activated
        Label1.Text = GerenciaInformacoes(Lista_Tentativa.Count, Lista_Senha.Count, N_TempoDeBloqueio)
        Label1.ForeColor = C_PatternConnectionColor
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigTela()
        ConfigPontos()
    End Sub

    Private Sub ConfigTela()
        Me.CenterToScreen()
        Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
    End Sub


    Private Sub ConfigPontos()
        If ListPoints.Count = 0 Then
            Dim Colunas As Integer = 0
            Dim Linhas As Integer = 0
            Dim DistanciaEmX As Integer = PictureBox1.Width * 0.25
            Dim DistanciaEmY As Integer = PictureBox1.Height * 0.25

            For k = 0 To N_Qtde_Pontos - 1
                Dim Ponto As Point = New Point(DistanciaEmX + Colunas * DistanciaEmX, DistanciaEmY + Linhas * DistanciaEmY)
                ListPoints.Add(Ponto)
                If Colunas = 2 Then
                    Linhas += 1
                    Colunas = 0
                Else
                    Colunas += 1
                End If
            Next
            Label1.Text = GerenciaInformacoes(Lista_Tentativa.Count, Lista_Senha.Count, N_TempoDeBloqueio)
            Label1.ForeColor = C_PatternConnectionColor
        End If
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        P_MousePoint = New Point(e.X, e.Y)
        Me.Refresh()
        If e.Button = MouseButtons.Left And B_AutorizaTarefasMouse = True Then
            CapturaPonto(P_MousePoint)
        End If
    End Sub

    Private Sub CapturaPonto(pontodomouse As Point)
        Dim ValorLimite As Integer = N_Tamanho * 1.5
        For k = 0 To ListPoints.Count - 1
            If pontodomouse.X < ListPoints(k).X + ValorLimite And pontodomouse.X > ListPoints(k).X - ValorLimite And pontodomouse.Y < ListPoints(k).Y + ValorLimite And pontodomouse.Y > ListPoints(k).Y - ValorLimite Then
                If Lista_Tentativa.Contains(k) = False Then Lista_Tentativa.Add(k)
            End If
        Next
    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        If B_AutorizaTarefasMouse = True Then VerificaTentativa()
    End Sub

    Private Sub VerificaTentativa()
        B_AutorizaTarefasMouse = False
        B_PermiteDesenhoPontoMouse = False
        GerenciaListas(Lista_Tentativa.Count, Lista_Senha.Count)
        Label1.Text = GerenciaInformacoes(Lista_Tentativa.Count, Lista_Senha.Count, N_TempoDeBloqueio)
        Label1.ForeColor = C_PatternConnectionColor
        If Timer1.Enabled = False Then LimpaListaTentativa()
    End Sub

    Private Sub GerenciaListas(ItensTentativa As Integer, ItensSenha As Integer)
        If ItensTentativa > 4 And ItensSenha = 0 Then
            For k = 0 To ItensTentativa - 1
                Lista_Senha.Add(Lista_Tentativa(k))
            Next
            B_CriouSenha = True
        ElseIf ItensTentativa > 4 And ItensSenha <> 0 And B_TrocaSenha = False Then
            If Lista_Tentativa.SequenceEqual(Lista_Senha) = True Then
                Label1.Text = GerenciaInformacoes(Lista_Tentativa.Count, Lista_Senha.Count, N_TempoDeBloqueio)
                Label1.ForeColor = C_PatternConnectionColor
                Refresh()
                Sleep(2000)
                Form2.Show()
                Me.Hide()
            Else
                B_TentativaDeLoginIncorreta = True
                N_Quantidade_Tentativas -= 1
                If N_Quantidade_Tentativas = -1 Then
                    N_Quantidade_Tentativas = 0
                    B_AutorizaTarefasMouse = False
                    Timer1.Enabled = True
                End If
            End If
        ElseIf ItensTentativa > 4 And ItensSenha <> 0 And B_TrocaSenha = True Then
            If Lista_Tentativa.SequenceEqual(Lista_Senha) = True Then
                B_TrocaSenha = False
                Lista_Senha.Clear()
            Else
                B_TentativaDeLoginIncorreta = True
                N_Quantidade_Tentativas -= 1
                If N_Quantidade_Tentativas = -1 Then
                    N_Quantidade_Tentativas = 0
                    B_AutorizaTarefasMouse = False
                    Timer1.Enabled = True
                End If
            End If
        End If
    End Sub


    Private Function GerenciaInformacoes(ItensTentativa As Integer, ItensSenha As Integer, TempoBloqueio As Integer) As String

        If Timer1.Enabled = True Then
            C_PatternConnectionColor = Color.Red
            Return "Sistema bloqueado, aguarde " & TempoBloqueio & " segundos..."

        Else
            If ItensTentativa = 0 And ItensSenha = 0 Then
                C_PatternConnectionColor = Color.DeepSkyBlue
                Return "Clique e arraste para desenhar/criar senha"

            ElseIf ItensTentativa > 0 And ItensTentativa <= 4 And ItensSenha = 0 Then
                C_PatternConnectionColor = Color.White
                Return "Use no mínimo 5 pontos"

            ElseIf ItensTentativa > 0 And ItensTentativa <= 4 And ItensSenha > 4 Then
                C_PatternConnectionColor = Color.White
                Return "Nenhuma senha possui menos que 5 pontos"

            ElseIf ItensTentativa = 0 And ItensSenha > 0 And B_TrocaSenha = False Then
                C_PatternConnectionColor = Color.DeepSkyBlue
                Return "Desenhe sua senha para logar"

            ElseIf ItensTentativa = 0 And ItensSenha > 0 And B_TrocaSenha = True Then
                C_PatternConnectionColor = Color.Magenta
                Return "Desenhe sua senha para apaga-lá"

            ElseIf ItensTentativa > 4 And ItensSenha = 0 And B_TrocaSenha = False Then
                C_PatternConnectionColor = Color.Orange
                Return "Senha apagada com sucesso"

            ElseIf ItensTentativa > 4 And ItensSenha > 4 And B_CriouSenha = True Then
                B_CriouSenha = False
                C_PatternConnectionColor = Color.Yellow
                Return "Senha de " & Lista_Senha.Count & " pontos criada"

            ElseIf ItensTentativa > 4 And ItensSenha > 4 And B_CriouSenha = False And B_TentativaDeLoginIncorreta = False And B_TrocaSenha = False Then
                C_PatternConnectionColor = Color.Green
                Return "Senha correta, logando no sistema..."

            ElseIf ItensTentativa > 4 And ItensSenha > 4 And B_CriouSenha = False And B_TentativaDeLoginIncorreta = True Then
                B_TentativaDeLoginIncorreta = False
                C_PatternConnectionColor = Color.Red
                Return "Senha Incorreta, " & N_Quantidade_Tentativas & " tentativas restantes"

            End If

        End If

        Return "ERROR 404"
    End Function

    Private Sub LimpaListaTentativa()
        Refresh()
        If Timer1.Enabled = False Then Sleep(1500)
        Lista_Tentativa.Clear()
        B_AutorizaTarefasMouse = True
        B_PermiteDesenhoPontoMouse = True
        Label1.Text = GerenciaInformacoes(Lista_Tentativa.Count, Lista_Senha.Count, N_TempoDeBloqueio)
        Label1.ForeColor = C_PatternConnectionColor
    End Sub

    Private Sub DrawListPoints(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'DESENHA LINHAS ENTRE PONTOS SELECIONADOS E MOUSE
        Dim MyPenToLine As Pen = New Pen(Color.WhiteSmoke, N_Tamanho / 2)
        MyPenToLine.EndCap = Drawing2D.LineCap.RoundAnchor
        If Lista_Tentativa.Count = 1 And B_PermiteDesenhoPontoMouse = True Then
            e.Graphics.DrawLine(MyPenToLine, ListPoints(Lista_Tentativa(0)), P_MousePoint)
        ElseIf Lista_Tentativa.Count >= 2 Then
            For k = 0 To Lista_Tentativa.Count - 2
                e.Graphics.DrawLine(MyPenToLine, ListPoints(Lista_Tentativa(k)), ListPoints(Lista_Tentativa(k + 1)))
                If Lista_Tentativa.Count < 9 And k = Lista_Tentativa.Count - 2 And B_PermiteDesenhoPontoMouse = True Then e.Graphics.DrawLine(MyPenToLine, ListPoints(Lista_Tentativa(k + 1)), P_MousePoint)
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
            Dim MyRect As Rectangle = New Rectangle(ListPoints(k).X - (N_Tamanho / 2), ListPoints(k).Y - (N_Tamanho / 2), N_Tamanho, N_Tamanho)
            e.Graphics.FillEllipse(Brushes.White, MyRect)
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        N_TempoDeBloqueio -= 1
        If N_TempoDeBloqueio = -1 Then
            N_TempoDeBloqueio = 10
            N_Quantidade_Tentativas = 3
            LimpaListaTentativa()
            Timer1.Enabled = False
        End If
        Label1.Text = GerenciaInformacoes(Lista_Tentativa.Count, Lista_Senha.Count, N_TempoDeBloqueio)
        Label1.ForeColor = C_PatternConnectionColor
        Refresh()
    End Sub

End Class
