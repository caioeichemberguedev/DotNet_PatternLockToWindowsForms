Imports System.Threading
Imports System.Drawing.Drawing2D
Public Class Form3

    Private Centro As Point
    Private Tentativa As Boolean = False
    'Private AchadoPrimeiroPonto As Boolean = False
    Private LocalizacaoAtualDoMouse As Point
    Private CirculoAchado As Integer = -1
    Private PontoA As Point
    Private PontoB As Point
    Private TentativasErradas As Integer = 0
    Private TempoDeEspera As Integer = 10
    Private Nivel As Integer = 0
    Private SalvandoUltimoAviso As String
    Private SistemaBloqueado As Boolean = False
    Public Login As Boolean = False

    Dim MyPen1 As Pen = New Pen(Color.DeepSkyBlue, 6)
    Dim Preenchimento As Brush = Brushes.Black

    Private ListaDePontos As New List(Of Point)
    Private TentativaSenha As New List(Of Point)
    Private SenhaCadastrada1 As New List(Of Point)
    Public SenhaCadastrada2 As New List(Of Point)
    Private SequenciaDaTentativa As ArrayList = New ArrayList



    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Centro = New Point(Me.Width / 2, Me.Height / 2)
        PontoA = New Point(-1, -1)
        CriandoPadrao()
        If SenhaCadastrada2.Count = 0 Then
            Label1.Text = "Cadastre uma senha"
            Label1.ForeColor = Color.DeepSkyBlue
        Else
            Label1.Text = "Desenhe sua senha para logar"
            Label1.ForeColor = Color.DeepSkyBlue
        End If

    End Sub

    'Private Sub Atualiza()
    '    If ListaDePontos.Count > 0 And SenhaCadastrada1.Count = 0 And SenhaCadastrada2.Count = 0 And TentativaSenha.Count = 0 And TentativasErradas = 0 Then
    '        Label1.Text = "Cadastre sua senha"
    '        Label1.ForeColor = Color.DeepSkyBlue
    '    ElseIf ListaDePontos.Count > 0 And SenhaCadastrada1.Count > 0 And SenhaCadastrada2.Count = 0 And TentativaSenha.Count = 0 And TentativasErradas = 0 Then
    '        Label1.Text = "Confirme a senha"
    '        Label1.ForeColor = Color.DeepSkyBlue
    '    ElseIf ListaDePontos.Count > 0 And SenhaCadastrada1.Count > 0 And SenhaCadastrada2.Count > 0 And TentativaSenha.Count = 0 And TentativasErradas = 0 Then
    '        Label1.Text = "Senha Cadastrada com sucesso"
    '        Label1.ForeColor = Color.Green
    '    ElseIf ListaDePontos.Count > 0 And SenhaCadastrada1.Count > 0 And SenhaCadastrada2.Count > 0 And TentativaSenha.Count > 0 And TentativasErradas = 0 Then
    '        Label1.Text = "Login Realizado"
    '        Label1.ForeColor = Color.Black
    '    ElseIf ListaDePontos.Count > 0 And SenhaCadastrada1.Count > 0 And SenhaCadastrada2.Count > 0 And TentativaSenha.Count > 0 And TentativasErradas > 0 And TentativasErradas <= 5 Then
    '        Label1.Text = "Senha errada, você tem mais: " & 5 - TentativasErradas & " tentativas"
    '        Label1.ForeColor = Color.Orange
    '    Else
    '        Label1.Text = "Bloqueado, aguarde: " & TempoDeEspera & " segundos"
    '        Label1.ForeColor = Color.Red
    '    End If
    '    Refresh()
    'End Sub

    Private Sub CriandoPadrao()
        For Linha As Integer = 0 To 2
            For Coluna As Integer = 0 To 2
                ListaDePontos.Add(New Point(Centro.X - 150 + Coluna * 100, Centro.Y - 150 + Linha * 100))
                Refresh()
                Thread.Sleep(100)
            Next
        Next
    End Sub

    Private Sub Desenho(sender As Object, e As PaintEventArgs) Handles Me.Paint

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias


        For Draw As Integer = 0 To ListaDePontos.Count - 1
            e.Graphics.FillEllipse(Preenchimento, ListaDePontos(Draw).X, ListaDePontos(Draw).Y, 50, 50)
        Next

        If SistemaBloqueado = False And Tentativa = True Then

            For k As Integer = 0 To SequenciaDaTentativa.Count - 1
                Dim PtA As Point = New Point(SequenciaDaTentativa(k).GSPonto1.X + 25, SequenciaDaTentativa(k).GSPonto1.Y + 25)
                Dim PtB As Point = New Point(SequenciaDaTentativa(k).GSPonto2.X + 25, SequenciaDaTentativa(k).GSPonto2.Y + 25)
                MyPen1.StartCap = LineCap.RoundAnchor
                MyPen1.EndCap = LineCap.RoundAnchor
                e.Graphics.DrawLine(MyPen1, PtA, PtB)
            Next

            MyPen1 = New Pen(Color.DeepSkyBlue, 6)
            MyPen1.StartCap = LineCap.RoundAnchor
            MyPen1.EndCap = LineCap.RoundAnchor
            If SequenciaDaTentativa.Count > 0 And SequenciaDaTentativa.Count <= 8 Then
                Dim Ultimo As Integer = SequenciaDaTentativa.Count - 1
                Dim UltimoCirculoCriado As Point = New Point(SequenciaDaTentativa(Ultimo).GSPonto2.X + 25, SequenciaDaTentativa(Ultimo).GSPonto2.Y + 25)
                e.Graphics.DrawLine(MyPen1, UltimoCirculoCriado, LocalizacaoAtualDoMouse)
            ElseIf SequenciaDaTentativa.Count = 0 And PontoA <> New Point(-1, -1) Then
                e.Graphics.DrawLine(MyPen1, New Point(PontoA.X + 25, PontoA.Y + 25), LocalizacaoAtualDoMouse)
            End If

        End If

        If CheckBox1.CheckState = CheckState.Checked And Nivel >= 1 Then
            For k As Integer = 0 To SenhaCadastrada1.Count - 2
                Dim PtA As Point = New Point(SenhaCadastrada1(k).X + 25, SenhaCadastrada1(k).Y + 25)
                Dim PtB As Point = New Point(SenhaCadastrada1(k + 1).X + 25, SenhaCadastrada1(k + 1).Y + 25)
                Dim MyPen2 As Pen = New Pen(Color.LightBlue, 4)
                MyPen2.StartCap = LineCap.RoundAnchor
                MyPen2.EndCap = LineCap.RoundAnchor
                e.Graphics.DrawLine(MyPen2, PtA, PtB)
                Dim MyFont As Font = New Font("Verdana", 10, FontStyle.Bold)
                e.Graphics.DrawString(k + 1.ToString & "º", MyFont, Brushes.Black, New Point(PtA.X - 35, PtA.Y - 35))
            Next
        End If

    End Sub

    Private Sub Tela_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If SistemaBloqueado = False Then
            If Tentativa = False Then
                Tentativa = True
            Else
                Tentativa = False
            End If
        End If
    End Sub

    Private Sub Tela_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Refresh()
        If e.Button = MouseButtons.Left And SistemaBloqueado = False Then
            LocalizacaoAtualDoMouse = New Point(e.X, e.Y)
            If VerificaSeEstaNoCirculo(LocalizacaoAtualDoMouse) = True Then
                If PontoA = New Point(-1, -1) Then
                    PontoA = New Point(ListaDePontos(CirculoAchado))
                Else
                    PontoB = New Point(ListaDePontos(CirculoAchado))
                    SequenciaDaTentativa.Add(New ClasseDePonto(PontoA, PontoB))
                    PontoA = PontoB
                End If
            End If
        End If
    End Sub

    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If SistemaBloqueado = False Then
            Dim Soneca As Integer = 500
            If Tentativa = True And Nivel = 0 And SenhaCadastrada1.Count > 0 And SenhaCadastrada1.Count >= 5 Then
                MyPen1 = New Pen(Color.Green, 6)
                Nivel += 1
                MyPen1 = New Pen(Color.Green, 6)
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                Label1.Text = "Ok, senha de " & SenhaCadastrada1.Count & " pontos"
                Label1.ForeColor = Color.LimeGreen
                Refresh()
                Thread.Sleep(Soneca)
                SequenciaDaTentativa.Clear()
                Label1.Text = "Confirme sua senha"
            ElseIf Tentativa = True And Nivel = 0 And SenhaCadastrada1.Count > 0 And SenhaCadastrada1.Count < 5 Then
                MyPen1 = New Pen(Color.Red, 6)
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                Label1.Text = SenhaCadastrada1.Count & " Pontos insulficientes, tente ao menos 5..."
                Label1.ForeColor = Color.Red
                Refresh()
                Thread.Sleep(Soneca)
                SequenciaDaTentativa.Clear()
                SenhaCadastrada1.Clear()
                Label1.Text = "Cadastre uma senha"
            ElseIf Tentativa = True And Nivel = 1 And SenhaCadastrada2.Count > 0 And SenhaCadastrada1.SequenceEqual(SenhaCadastrada2) = True Then
                MyPen1 = New Pen(Color.Green, 6)
                Nivel += 1
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                Label1.Text = "Senha de " & SenhaCadastrada2.Count & " pontos cadastrada"
                Label1.ForeColor = Color.LimeGreen
                Refresh()
                Thread.Sleep(Soneca)
                SequenciaDaTentativa.Clear()
                Label1.Text = "Desenhe sua senha para logar"
            ElseIf Tentativa = True And Nivel = 1 And SenhaCadastrada2.Count > 0 And SenhaCadastrada1.SequenceEqual(SenhaCadastrada2) = False Then
                MyPen1 = New Pen(Color.Red, 6)
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                TentativasErradas -= 1
                Label1.Text = "Senhas divergentes"
                Label1.ForeColor = Color.Red
                Refresh()
                Thread.Sleep(Soneca)
                SequenciaDaTentativa.Clear()
                SenhaCadastrada1.Clear()
                SenhaCadastrada2.Clear()
                Nivel -= 1
                Label1.Text = "Cadastre uma senha"
            ElseIf Tentativa = True And Nivel = 2 And TentativaSenha.Count > 0 And SenhaCadastrada2.SequenceEqual(TentativaSenha) = True Then
                MyPen1 = New Pen(Color.Green, 6)
                Nivel += 1
                SenhaCadastrada1.Clear()
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                Label1.Text = "Senha de " & SenhaCadastrada2.Count & " pontos correta, realizando login..."
                Label1.ForeColor = Color.LimeGreen
                Preenchimento = Brushes.Green
                Refresh()
                Thread.Sleep(Soneca)
                Form2.Show()
                Preenchimento = Brushes.Black
                SequenciaDaTentativa.Clear()
                TentativaSenha.Clear()
                Tentativa = False
                Label1.Text = "Desenhe sua senha para poder alterar"
                Me.Hide()
            ElseIf Tentativa = True And Nivel = 2 And TentativaSenha.Count > 0 And SenhaCadastrada2.SequenceEqual(TentativaSenha) = False Then
                MyPen1 = New Pen(Color.Red, 6)
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                TentativasErradas += 1
                Label1.ForeColor = Color.Red
                If TentativasErradas < 5 Then
                    Dim tem As Integer = 5 - TentativasErradas
                    Label1.Text = "Senha incorreta, mais " & tem & " tentativas"
                    Refresh()
                    Thread.Sleep(Soneca)
                    PontoA = New Point(-1, -1)
                    SequenciaDaTentativa.Clear()
                    TentativaSenha.Clear()
                    Label1.Text = "Desenhe sua senha para logar"
                Else
                    Bloqueio()
                End If
            ElseIf Tentativa = True And Nivel = 3 And SenhaCadastrada2.Count > 0 And TentativaSenha.SequenceEqual(SenhaCadastrada2) = True Then
                MyPen1 = New Pen(Color.Green, 6)
                Nivel = 0
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                Label1.Text = "Senha antiga apagada"
                Label1.ForeColor = Color.Orange
                Refresh()
                Thread.Sleep(Soneca)
                SequenciaDaTentativa.Clear()
                Label1.Text = "Desenhe sua senha para poder alterar"
            ElseIf Tentativa = True And Nivel = 3 And SenhaCadastrada2.Count > 0 And TentativaSenha.SequenceEqual(SenhaCadastrada2) = False Then
                MyPen1 = New Pen(Color.Red, 6)
                LocalizacaoAtualDoMouse = New Point(PontoA.X + 25, PontoA.Y + 25)
                TentativasErradas += 1
                Label1.ForeColor = Color.Red
                If TentativasErradas < 5 Then
                    Dim tem As Integer = 5 - TentativasErradas
                    Label1.Text = "Senha incorreta, mais " & tem & " tentativas"
                    Refresh()
                    Thread.Sleep(Soneca)
                    PontoA = New Point(-1, -1)
                    SequenciaDaTentativa.Clear()
                    TentativaSenha.Clear()
                    Label1.Text = "Desenhe sua senha para logar"
                Else
                    Bloqueio()
                End If
            Else
                SalvandoUltimoAviso = Label1.Text
                Label1.Text = "Pressione e arraste o mouse sobre os pontos"
                Label1.ForeColor = Color.Yellow
                Refresh()
                Thread.Sleep(Soneca)
                Label1.Text = SalvandoUltimoAviso
            End If
            PontoA = New Point(-1, -1)
            MyPen1 = New Pen(Color.DeepSkyBlue, 6)
            Label1.ForeColor = Color.DeepSkyBlue
            Tentativa = False
            Refresh()
        End If
    End Sub

    Private Sub Bloqueio()
        PontoA = New Point(-1, -1)
        SequenciaDaTentativa.Clear()
        TentativaSenha.Clear()
        SistemaBloqueado = True
        For Tempo As Integer = 0 To 9
            TempoDeEspera -= 1
            Label1.Text = "Sistema bloqueado, aguarde " & TempoDeEspera & " segundos..."
            Refresh()
            Thread.Sleep(1000)
        Next
        TempoDeEspera = 10
        TentativasErradas = 0
        Label1.Text = "Desenhe sua senha para logar"
        Label1.ForeColor = Color.DeepSkyBlue
        SistemaBloqueado = False
        Refresh()
    End Sub

    Private Function VerificaSeEstaNoCirculo(ByVal P As Point) As Boolean
        If SistemaBloqueado = False Then
            For k As Integer = 0 To ListaDePontos.Count - 1
                If P.X >= ListaDePontos(k).X And P.X <= ListaDePontos(k).X + 50 And P.Y >= ListaDePontos(k).Y And P.Y <= ListaDePontos(k).Y + 50 Then
                    If Nivel = 0 Then
                        If SenhaCadastrada1.Contains(ListaDePontos(k)) = False Then
                            SenhaCadastrada1.Add(New Point(ListaDePontos(k)))
                            CirculoAchado = k
                            Return True
                        End If
                    ElseIf Nivel = 1 Then
                        If SenhaCadastrada2.Contains(ListaDePontos(k)) = False Then
                            SenhaCadastrada2.Add(New Point(ListaDePontos(k)))
                            CirculoAchado = k
                            Return True
                        End If
                    ElseIf Nivel = 2 Then
                        If TentativaSenha.Contains(ListaDePontos(k)) = False Then
                            TentativaSenha.Add(New Point(ListaDePontos(k)))
                            CirculoAchado = k
                            Return True
                        End If
                    ElseIf Nivel = 3 Then
                        If TentativaSenha.Contains(ListaDePontos(k)) = False Then
                            TentativaSenha.Add(New Point(ListaDePontos(k)))
                            CirculoAchado = k
                            Return True
                        End If
                    End If
                End If
            Next
        End If
        Return False
    End Function

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Refresh()
    End Sub
End Class