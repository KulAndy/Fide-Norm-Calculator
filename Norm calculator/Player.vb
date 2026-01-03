Public Class Player
    Public Enum Sex
        M
        W
    End Enum

    Public Enum Title
        GM
        IM
        WGM
        FM
        WIM
        CM
        WFM
        WCM
        NONE
    End Enum

    Public playerSex As Sex
    Public playerTitle As Title
    Public name As String
    Public rating As UShort
    Public federation As String
    Public fideId As ULong
    Public birthday As String = "0000-00-00"
    Public rounds As List(Of Round) = New List(Of Round)

    Public Sub New(line As String)
        Select Case Mid(line, 10, 1)
            Case "m"
                playerSex = Sex.M
            Case "w"
                playerSex = Sex.W
            Case Else
                playerSex = Nothing
        End Select
        Select Case Trim(Mid(line, 11, 3))
            Case "GM"
                playerTitle = Title.GM
            Case "IM"
                playerTitle = Title.IM
            Case "FM"
                playerTitle = Title.FM
            Case "CM"
                playerTitle = Title.CM
            Case "WGM"
                playerTitle = Title.WGM
            Case "WIM"
                playerTitle = Title.WIM
            Case "WFM"
                playerTitle = Title.WFM
            Case "WCM"
                playerTitle = Title.WCM
            Case Else
                playerTitle = Title.NONE
        End Select
        birthday = Mid(line, 70, 10)

        name = Trim(Mid(line, 15, 33))
        UShort.TryParse(Trim(Mid(line, 49, 4)), rating)
        federation = Trim(Mid(line, 54, 3))
        fideId = Nothing
        ULong.TryParse(Trim(Mid(line, 58, 11)), fideId)

        For i = 92 To line.Length Step 10
            rounds.Add(
                    New Round(Mid(line, i, 8))
                )
        Next i
    End Sub

    Public Function GetPoints() As Single
        Dim points As Single = 0
        For Each roundItem As Round In rounds
            Select Case roundItem.GameResult
                Case Round.Result.Plus, Round.Result.WIN_NO1, Round.Result.WIN, Round.Result.FULLBYE
                    points += 1
                Case Round.Result.DRAW_NO1, Round.Result.DRAW, Round.Result.HALFBYE
                    points += 0.5
            End Select
        Next

        Return points
    End Function

End Class
