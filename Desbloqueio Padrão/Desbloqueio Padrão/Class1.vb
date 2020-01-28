Public Class ClasseDePonto

    Dim Ponto1 As Point
    Dim Ponto2 As Point


    Public Sub New(Ponto1 As Point, Ponto2 As Point)

        Me.Ponto1 = Ponto1
        Me.Ponto2 = Ponto2

    End Sub

    Public Property GSPonto1() As Point
        Get
            Return Ponto1
        End Get
        Set(Ponto1_ As Point)
            Ponto1 = Ponto1_
        End Set
    End Property

    Public Property GSPonto2() As Point
        Get
            Return Ponto2
        End Get
        Set(Ponto2_ As Point)
            Ponto2 = Ponto2_
        End Set
    End Property

End Class
