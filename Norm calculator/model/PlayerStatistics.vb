Module PlayerStatistics
    Public Function GetNorm(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As String
        If Not (CalculatorForm.twoFederations Or CheckFederation(player, playersDict)) Or player.playerTitle = Player.Title.GM Then
            Return ""
        End If

        Dim title As String = ""
        Dim delta As Single = player.GetDelta()
        Dim WFMs = CountWFMOpponents(player, playersDict)
        Dim WIMs = CountWIMOpponents(player, playersDict)
        Dim WGMs = CountWGMOpponents(player, playersDict)
        Dim FMs = CountFMOpponents(player, playersDict)
        Dim IMs = CountIMOpponents(player, playersDict)
        Dim GMs = CountGMOpponents(player, playersDict)
        Dim count As Byte = 0
        Dim points As Single = 0
        For Each roundItem As Round In player.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN
                        count += 1
                        points += 1
                    Case Round.Result.DRAW_NO1, Round.Result.DRAW
                        count += 1
                        points += 0.5
                    Case Round.Result.LOSE_NO1, Round.Result.LOSE
                        count += 1
                End Select
            Catch ex As Exception
            End Try
        Next

        Dim titled = WFMs + WIMs + WGMs + FMs + IMs + GMs
        If points / count < 0.35 Or titled * 1.0 / count < 1.0 / 2.0 Then
            Return ""
        End If


        If player.playerTitle < Player.Title.WIM And
            player.playerSex = Player.Sex.W And
            (WIMs + WGMs + IMs + GMs) * 1.0 / count >= 1.0 / 3.0 And
            (WIMs + WGMs + IMs + GMs) >= 3 And
            RaiseWIMAverageRating(player, playersDict) >= 2030 And
            RaiseWIMAverageRating(player, playersDict) + delta >= 2250 Then
            title = "WIM"
        End If
        If player.playerTitle < Player.Title.WGM And
            player.playerSex = Player.Sex.W And
            (WGMs + IMs + GMs) * 1.0 / count >= 1.0 / 3.0 And
            (WGMs + IMs + GMs) >= 3 And
            RaiseWGMAverageRating(player, playersDict) >= 2180 And
            RaiseWGMAverageRating(player, playersDict) + delta >= 2400 Then
            title = "WGM"
        End If

        If player.playerTitle < Player.Title.IM And
            (IMs + GMs) * 1.0 / count >= 1.0 / 3.0 And
            (IMs + GMs) >= 3 And
            RaiseIMAverageRating(player, playersDict) >= 2230 And
            RaiseIMAverageRating(player, playersDict) + delta >= 2450 Then
            title = "IM"
        End If

        If player.playerTitle < Player.Title.GM And
            GMs * 1.0 / count >= 1.0 / 3.0 And
            GMs >= 3 And
            RaiseWGMAverageRating(player, playersDict) >= 2380 And
            RaiseGMAverageRating(player, playersDict) + delta >= 2600 Then
            title = "GM"
        End If

        Return title
    End Function

    Private Function CheckFederation(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Boolean
        Dim countries As New Dictionary(Of String, Byte)
        For Each roundItem As Round In player.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN, Round.Result.DRAW_NO1, Round.Result.DRAW, Round.Result.LOSE_NO1, Round.Result.LOSE
                        Dim fed As String = playersDict(roundItem.OpponentID).federation
                        If countries.ContainsKey(fed) Then
                            countries(fed) += 1
                        Else
                            countries(fed) = 1
                        End If
                End Select
            Catch ex As Exception
            End Try
        Next

        Dim countryNo As Byte = 0
        For Each kvp As KeyValuePair(Of String, Byte) In countries
            If player.federation <> kvp.Key Then
                countryNo += 1
                If kvp.Value / player.rounds.Count > 2.0 / 3.0 Then
                    Return False
                End If
            ElseIf kvp.Value / player.rounds.Count > 3.0 / 5.0 Then
                Return False
            End If
        Next

        Return countryNo >= 2
    End Function

    Public Function GetRequiredNormPoints(ByRef player As Player, title As Player.Title, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Select Case title
            Case Player.Title.GM
                Return CalculatorForm.rounds.Count * player.GetReversedDelta(2600 - RaiseGMAverageRating(player, playersDict))
            Case Player.Title.IM
                Return CalculatorForm.rounds.Count * player.GetReversedDelta(2450 - RaiseIMAverageRating(player, playersDict))
            Case Player.Title.WGM
                Return CalculatorForm.rounds.Count * player.GetReversedDelta(2400 - RaiseWGMAverageRating(player, playersDict))
            Case Player.Title.WIM
                Return CalculatorForm.rounds.Count * player.GetReversedDelta(2250 - RaiseWIMAverageRating(player, playersDict))
            Case Else
                Return Single.NaN
        End Select
    End Function

    Public Function GetAverageRating(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Dim ratings As New List(Of UShort)
        For Each roundItem As Round In player.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN, Round.Result.DRAW_NO1, Round.Result.DRAW, Round.Result.LOSE_NO1, Round.Result.LOSE
                        If playersDict(roundItem.OpponentID).rating > 1400 Then
                            ratings.Add(playersDict(roundItem.OpponentID).rating)
                        Else
                            ratings.Add(1400)
                        End If
                End Select
            Catch ex As Exception

            End Try
        Next

        Dim sum As Single = ratings.Sum(Function(item) item)
        Dim count As Byte = ratings.Count()

        If count = 0 Then
            Return 0
        Else
            Return Math.Round(sum / count)
        End If
    End Function

    Public Function RaiseTitleAverageRating(ByRef player As Player, floor As UShort, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Dim ratings As New List(Of UShort)
        For Each roundItem As Round In player.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN, Round.Result.DRAW_NO1, Round.Result.DRAW, Round.Result.LOSE_NO1, Round.Result.LOSE
                        If playersDict(roundItem.OpponentID).rating > 1400 Then
                            ratings.Add(playersDict(roundItem.OpponentID).rating)
                        Else
                            ratings.Add(1400)
                        End If
                End Select
            Catch ex As Exception
            End Try
        Next

        If ratings.Count = 0 Then
            Return 0
        End If

        Dim sum As Single = ratings.Sum(Function(item) item)
        Dim minimum As UShort = ratings.Min()
        Dim count As Byte = ratings.Count()
        If minimum < floor Then
            sum -= minimum
            sum += floor
        End If

        Return Math.Round(sum / count)
    End Function

    Public Function RaiseGMAverageRating(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Return RaiseTitleAverageRating(player, 2200, playersDict)
    End Function

    Public Function RaiseIMAverageRating(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Return RaiseTitleAverageRating(player, 2050, playersDict)
    End Function

    Public Function RaiseWGMAverageRating(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Return RaiseTitleAverageRating(player, 2000, playersDict)
    End Function

    Public Function RaiseWIMAverageRating(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Single
        Return RaiseTitleAverageRating(player, 1850, playersDict)
    End Function


    Public Function CountTitleOpponents(ByRef player As Player, title As Player.Title, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Dim count As Byte = 0
        For Each roundItem As Round In player.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN, Round.Result.DRAW_NO1, Round.Result.DRAW, Round.Result.LOSE_NO1, Round.Result.LOSE
                        If playersDict(roundItem.OpponentID).playerTitle = title Then count += 1
                End Select
            Catch ex As Exception

            End Try
        Next

        Return count
    End Function


    Public Function CountGMOpponents(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Return CountTitleOpponents(player, Player.Title.GM, playersDict)
    End Function
    Public Function CountIMOpponents(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Return CountTitleOpponents(player, Player.Title.IM, playersDict)
    End Function
    Public Function CountFMOpponents(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Return CountTitleOpponents(player, Player.Title.FM, playersDict)
    End Function
    Public Function CountWGMOpponents(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Return CountTitleOpponents(player, Player.Title.WGM, playersDict)
    End Function
    Public Function CountWIMOpponents(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Return CountTitleOpponents(player, Player.Title.WIM, playersDict)
    End Function

    Public Function CountWFMOpponents(ByRef player As Player, ByRef playersDict As Dictionary(Of UShort, Player)) As Byte
        Return CountTitleOpponents(player, Player.Title.WFM, playersDict)
    End Function


End Module
