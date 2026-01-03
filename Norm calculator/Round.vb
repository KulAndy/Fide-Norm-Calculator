Imports System.Security.Policy
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Windows.Win32.System

Public Class Round
    Public Enum Color
        W
        B
    End Enum

    Public Enum Result
        Minus
        Plus
        WIN_NO1
        DRAW_NO1
        LOSE_NO1
        WIN
        DRAW
        LOSE
        HALFBYE
        FULLBYE
        UNPAIRED
        None
    End Enum
    Public OpponentID As UShort
    Public GameColor As Color
    Public GameResult As Result

    Public Sub New(span As String)
        UShort.TryParse(Mid(span, 1, 4), Me.OpponentID)
        Select Case Mid(span, 6, 1)
            Case "w"
                Me.GameColor = Color.W
            Case "b"
                Me.GameColor = Color.B
            Case Else
                Me.GameColor = Nothing
        End Select
        Select Case Mid(span, 8, 1)
            Case "-"
                Me.GameResult = Result.Minus
            Case "+"
                Me.GameResult = Result.Plus
            Case "W"
                Me.GameResult = Result.WIN_NO1
            Case "D"
                Me.GameResult = Result.DRAW_NO1
            Case "L"
                Me.GameResult = Result.LOSE_NO1
            Case "1"
                Me.GameResult = Result.WIN
            Case "="
                Me.GameResult = Result.DRAW
            Case "0"
                Me.GameResult = Result.LOSE
            Case "H"
                Me.GameResult = Result.HALFBYE
            Case "F"
                Me.GameResult = Result.FULLBYE
            Case "U"
                Me.GameResult = Result.UNPAIRED
            Case Else
                Me.GameResult = Result.None
        End Select

    End Sub

    Public Overrides Function ToString() As String
        Dim oppIdStr As String = Me.OpponentID.ToString().PadLeft(4)
        Dim colorStr As String = GameColor.ToString().Substring(0, 1)
        Dim resultStr As String
        Select Case Me.GameResult
            Case Result.Minus
                resultStr = "-"
            Case Result.Plus
                resultStr = "+"
            Case Result.WIN_NO1
                resultStr = "W"
            Case Result.DRAW_NO1
                resultStr = "D"
            Case Result.LOSE_NO1
                resultStr = "L"
            Case Result.WIN
                resultStr = "1"
            Case Result.DRAW
                resultStr = "="
            Case Result.LOSE
                resultStr = "0"
            Case Result.HALFBYE
                resultStr = "H"
            Case Result.FULLBYE
                resultStr = "F"
            Case Result.UNPAIRED
                resultStr = "U"
            Case Else
                resultStr = " "

        End Select
        Return $"{oppIdStr} {colorStr} {resultStr}"
    End Function

End Class
