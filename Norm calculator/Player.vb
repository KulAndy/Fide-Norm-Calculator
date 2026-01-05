Public Class Player
    Public Enum Sex
        M
        W
    End Enum

    Public Enum Title
        NONE
        WCM
        WFM
        CM
        WIM
        FM
        WGM
        IM
        GM
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

    Public Function GetFirstName() As String
        If String.IsNullOrWhiteSpace(name) Then Return ""

        Dim idx As Integer = name.IndexOf(","c)
        If idx = -1 OrElse idx = name.Length - 1 Then
            Return ""
        End If

        Return name.Substring(idx + 1).Trim()
    End Function


    Public Function GetLastName() As String
        If String.IsNullOrWhiteSpace(name) Then Return ""

        Dim idx As Integer = name.IndexOf(","c)
        If idx = -1 Then
            Return name.Trim()
        End If

        Return name.Substring(0, idx).Trim()
    End Function

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

    Function GetDelta()
        Dim points As Single = 0
        Dim count As Single = 0
        For Each roundItem As Round In Me.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN
                        points += 1
                        count += 1
                    Case Round.Result.DRAW_NO1, Round.Result.DRAW
                        points += 0.5
                        count += 1
                    Case Round.Result.LOSE_NO1, Round.Result.LOSE
                        count += 1
                End Select
            Catch ex As Exception

            End Try
        Next

        If count = 0 Then
            Return 0
        End If

        Return PercentToDelta(points, count)

    End Function

    Function PercentToDelta(points As Single, count As Single) As Single
        Select Case Math.Round(points / count, 2)
            Case 1
                Return 800
            Case 0.99 To 1
                Return 677
            Case 0.98 To 0.99
                Return 589
            Case 0.97 To 0.98
                Return 538
            Case 0.96 To 0.97
                Return 501
            Case 0.95 To 0.96
                Return 470
            Case 0.94 To 0.95
                Return 444
            Case 0.93 To 0.94
                Return 422
            Case 0.92 To 0.93
                Return 401
            Case 0.91 To 0.92
                Return 383
            Case 0.9 To 0.91
                Return 366
            Case 0.89 To 0.9
                Return 351
            Case 0.88 To 0.89
                Return 336
            Case 0.87 To 0.88
                Return 322
            Case 0.86 To 0.87
                Return 309
            Case 0.85 To 0.86
                Return 296
            Case 0.84 To 0.85
                Return 284
            Case 0.83 To 0.84
                Return 273
            Case 0.82 To 0.83
                Return 262
            Case 0.81 To 0.82
                Return 251
            Case 0.8 To 0.81
                Return 240
            Case 0.79 To 0.8
                Return 230
            Case 0.78 To 0.79
                Return 220
            Case 0.77 To 0.78
                Return 211
            Case 0.76 To 0.77
                Return 202
            Case 0.75 To 0.76
                Return 193
            Case 0.74 To 0.75
                Return 184
            Case 0.73 To 0.74
                Return 175
            Case 0.72 To 0.73
                Return 166
            Case 0.71 To 0.72
                Return 158
            Case 0.7 To 0.71
                Return 149
            Case 0.69 To 0.7
                Return 141
            Case 0.68 To 0.69
                Return 133
            Case 0.67 To 0.68
                Return 125
            Case 0.66 To 0.67
                Return 117
            Case 0.65 To 0.66
                Return 110
            Case 0.64 To 0.65
                Return 102
            Case 0.63 To 0.64
                Return 95
            Case 0.62 To 0.63
                Return 87
            Case 0.61 To 0.62
                Return 80
            Case 0.6 To 0.61
                Return 72
            Case 0.59 To 0.6
                Return 65
            Case 0.58 To 0.59
                Return 57
            Case 0.57 To 0.58
                Return 50
            Case 0.56 To 0.57
                Return 43
            Case 0.55 To 0.56
                Return 36
            Case 0.54 To 0.55
                Return 29
            Case 0.53 To 0.54
                Return 21
            Case 0.52 To 0.53
                Return 14
            Case 0.51 To 0.52
                Return 7
            Case 0.5 To 0.51
                Return 0
            Case 0.5
                Return 0
            Case Else
                Return -PercentToDelta(count - points, count)
        End Select

    End Function

    Public Function GetReversedDelta(delta As Integer) As Single
        Dim low As Single = 0.5F
        Dim high As Single = 1.0F
        Dim mid As Single = 0.0F
        Dim result As Single = 0.0F
        Dim bestPercent As Single = 0.0F

        While high - low > 0.001F
            mid = (low + high) / 2
            result = PercentToDelta(mid, 1)

            If result >= delta Then
                bestPercent = mid
                high = mid
            Else
                low = mid
            End If
        End While

        Return bestPercent
    End Function

End Class
