Imports System.Globalization

Public Class CalculatorForm
    Private eventName As String = ""
    Dim city As String = ""
    Dim federation As String = ""
    Dim startDate As Date = Nothing
    Dim endDate As Date = Nothing
    Dim playersNo As UShort = 0
    Dim ratedPlayersNo As UShort = 0
    Dim system As String = ""
    Dim chief As String = ""
    Dim deputies As New List(Of String)()
    Dim rate As String = ""
    Dim rounds As New List(Of Date)()
    Dim playersDict As New Dictionary(Of UShort, Player)

    Dim twoFederations As Boolean = False
    Dim roundRobinUnrated As Boolean = False

    Private Sub OpenFile_Click(sender As Object, e As EventArgs) Handles OpenFile.Click
        If SelectReportDialog.ShowDialog = DialogResult.OK Then
            pathStatus.Text = SelectReportDialog.FileName
            playersDict.Clear()
            Dim objFile As New IO.StreamReader(SelectReportDialog.FileName)
            Dim line As String = objFile.ReadLine()
            Do Until line Is Nothing
                Dim prefix As String = Microsoft.VisualBasic.Left(line, 3)
                Dim value As String = Mid(line, 5)

                Select Case prefix
                    Case "001"
                        Dim playerId As UShort
                        If UShort.TryParse(Mid(line, 5, 4), playerId) Then
                            playersDict.Add(playerId, New Player(line))
                        End If
                    Case "012"
                        eventName = value
                    Case "022"
                        city = value
                    Case "032"
                        federation = value
                    Case "042"
                        Try
                            startDate = DateTime.ParseExact(value, "yyyy/MM/dd", CultureInfo.InvariantCulture)
                        Catch ex As Exception
                            MessageBox.Show("Invalid start date: " & value & ".Expected yyyy/MM/dd", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    Case "052"
                        Try
                            endDate = DateTime.ParseExact(value, "yyyy/MM/dd", CultureInfo.InvariantCulture)
                        Catch ex As Exception
                            MessageBox.Show("Invalid end date: " & value & ".Expected yyyy/MM/dd", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    Case "062"
                        If Not UShort.TryParse(value, playersNo) Then
                            MessageBox.Show("Invalid players number")
                        End If
                    Case "072"
                        If Not UShort.TryParse(value, ratedPlayersNo) Then
                            MessageBox.Show("Invalid rated players number")
                        End If
                    Case "092"
                        system = value
                    Case "102"
                        chief = value
                    Case "112"
                        deputies.Add(value)
                    Case "122"
                        rate = value
                    Case "132"
                        For i = 92 To line.Length Step 10
                            Dim span As String = Mid(line, i, 8)
                            rounds.Add(DateTime.ParseExact(span, "yy/MM/dd", Nothing))
                        Next i
                End Select

                line = objFile.ReadLine()
            Loop
            objFile.Close()
            RefreshData()
            EventLabel.Text = eventName
            CityLabel.Text = city & ", " & federation
            RateLabel.Text = rate
            ChiefLabel.Text = chief

            Dim rated As Byte = 0
            Dim GMs As Byte = 0
            Dim IMs As Byte = 0
            Dim FMs As Byte = 0
            Dim unrated As Byte = 0
            Dim WGMs As Byte = 0
            Dim WIMs As Byte = 0
            Dim WFMs As Byte = 0

            Dim ratedFeds As New HashSet(Of String)()
            Dim GMsFeds As New HashSet(Of String)()
            Dim IMsFeds As New HashSet(Of String)()
            Dim FMsFeds As New HashSet(Of String)()
            Dim unratedFeds As New HashSet(Of String)()
            Dim WGMsFeds As New HashSet(Of String)()
            Dim WIMsFeds As New HashSet(Of String)()
            Dim WFMsFeds As New HashSet(Of String)()

            Dim ratedHost As Byte = 0
            Dim GMsHost As Byte = 0
            Dim IMsHost As Byte = 0
            Dim FMsHost As Byte = 0
            Dim unratedHost As Byte = 0
            Dim WGMsHost As Byte = 0
            Dim WIMsHost As Byte = 0
            Dim WFMsHost As Byte = 0

            Dim ratedOther As Byte = 0
            Dim GMsOther As Byte = 0
            Dim IMsOther As Byte = 0
            Dim FMsOther As Byte = 0
            Dim unratedOther As Byte = 0
            Dim WGMsOther As Byte = 0
            Dim WIMsOther As Byte = 0
            Dim WFMsOther As Byte = 0

            For Each kvp As KeyValuePair(Of UShort, Player) In playersDict
                Dim player As Player = kvp.Value
                If player.rating > 1400 Then
                    rated += 1
                    ratedFeds.Add(player.federation)
                    If player.federation = federation Then ratedHost += 1 Else ratedOther += 1
                Else
                    unrated += 1
                    unratedFeds.Add(player.federation)
                    If player.federation = federation Then unratedHost += 1 Else unratedOther += 1
                End If
                Select Case player.playerTitle
                    Case Player.Title.GM
                        GMs += 1
                        GMsFeds.Add(player.federation)
                        If player.federation = federation Then GMsHost += 1 Else GMsOther += 1
                    Case Player.Title.IM
                        IMs += 1
                        IMsFeds.Add(player.federation)
                        If player.federation = federation Then IMsHost += 1 Else IMsOther += 1
                    Case Player.Title.FM
                        FMs += 1
                        FMsFeds.Add(player.federation)
                        If player.federation = federation Then FMsHost += 1 Else FMsOther += 1
                    Case Player.Title.WGM
                        WGMs += 1
                        WGMsFeds.Add(player.federation)
                        If player.federation = federation Then WGMsHost += 1 Else WGMsOther += 1
                    Case Player.Title.WIM
                        WIMs += 1
                        WIMsFeds.Add(player.federation)
                        If player.federation = federation Then WIMsHost += 1 Else WIMsOther += 1
                    Case Player.Title.WFM
                        WFMs += 1
                        WFMsFeds.Add(player.federation)
                        If player.federation = federation Then WFMsHost += 1 Else WFMsOther += 1
                End Select
            Next

            Debug.WriteLine("")
            Debug.WriteLine("====================================================================")
            Debug.WriteLine(rated)
            Debug.WriteLine(GMs)
            Debug.WriteLine(IMs)
            Debug.WriteLine(FMs)
            Debug.WriteLine(unrated)
            Debug.WriteLine(WGMs)
            Debug.WriteLine(WIMs)
            Debug.WriteLine(WFMs)
            Debug.WriteLine("====================================================================")
            Debug.WriteLine(ratedFeds.Count)
            Debug.WriteLine(GMsFeds.Count)
            Debug.WriteLine(IMsFeds.Count)
            Debug.WriteLine(FMsFeds.Count)
            Debug.WriteLine(unratedFeds.Count)
            Debug.WriteLine(WGMsFeds.Count)
            Debug.WriteLine(WIMsFeds.Count)
            Debug.WriteLine(WFMsFeds.Count)
            Debug.WriteLine("====================================================================")
            Debug.WriteLine(ratedHost)
            Debug.WriteLine(GMsHost)
            Debug.WriteLine(IMsHost)
            Debug.WriteLine(FMsHost)
            Debug.WriteLine(unratedHost)
            Debug.WriteLine(WGMsHost)
            Debug.WriteLine(WIMsHost)
            Debug.WriteLine(WFMsHost)
            Debug.WriteLine("====================================================================")
            Debug.WriteLine(ratedOther)
            Debug.WriteLine(GMsOther)
            Debug.WriteLine(IMsOther)
            Debug.WriteLine(FMsOther)
            Debug.WriteLine(unratedOther)
            Debug.WriteLine(WGMsOther)
            Debug.WriteLine(WIMsOther)
            Debug.WriteLine(WFMsOther)
            Debug.WriteLine("====================================================================")
        End If
    End Sub

    Private Sub RefreshData()
        PlayersDataGridView.DataSource = Nothing
        PlayersDataGridView.Columns.Clear()

        Dim dt As New DataTable()

        dt.Columns.Add("Start", GetType(UShort))
        dt.Columns.Add("Title", GetType(String))
        dt.Columns.Add("Name", GetType(String))
        dt.Columns.Add("Sex", GetType(String))
        dt.Columns.Add("Rating", GetType(String))
        dt.Columns.Add("Federation", GetType(String))
        dt.Columns.Add("FideID", GetType(String))
        dt.Columns.Add("Birthday", GetType(String))
        dt.Columns.Add("Norm", GetType(String))
        dt.Columns.Add("Points", GetType(String))
        dt.Columns.Add("GMs", GetType(String))
        dt.Columns.Add("IMs", GetType(String))
        dt.Columns.Add("FMs", GetType(String))
        dt.Columns.Add("WGMs", GetType(String))
        dt.Columns.Add("WIMs", GetType(String))
        dt.Columns.Add("WFMs", GetType(String))
        dt.Columns.Add("Delta", GetType(String))
        dt.Columns.Add("Perf", GetType(String))
        dt.Columns.Add("ARO", GetType(String))
        dt.Columns.Add("GM raised", GetType(String))
        dt.Columns.Add("IM raised", GetType(String))
        dt.Columns.Add("WGM raised", GetType(String))
        dt.Columns.Add("WIM raised", GetType(String))

        For i = 1 To rounds.Count
            dt.Columns.Add($"Round{i}", GetType(String))
        Next

        For Each playerId As UShort In playersDict.Keys
            Dim player As Player = playersDict(playerId)

            Dim newRow As DataRow = dt.NewRow()
            newRow("Start") = playerId
            newRow("Sex") = player.playerSex.ToString()
            Select Case player.playerTitle
                Case Player.Title.GM
                    newRow("Title") = "GM"
                Case Player.Title.IM
                    newRow("Title") = "IM"
                Case Player.Title.WGM
                    newRow("Title") = "WGM"
                Case Player.Title.FM
                    newRow("Title") = "FM"
                Case Player.Title.WIM
                    newRow("Title") = "WIM"
                Case Player.Title.CM
                    newRow("Title") = "CM"
                Case Player.Title.WFM
                    newRow("Title") = "WFM"
                Case Player.Title.WCM
                    newRow("Title") = "WCM"
                Case Else
                    newRow("Title") = ""
            End Select

            newRow("Name") = player.name
            newRow("Rating") = player.rating
            newRow("Federation") = player.federation
            newRow("FideID") = player.fideId
            newRow("Birthday") = If(player.birthday = "0000-00-00", "", player.birthday)
            newRow("Norm") = GetNorm(player)
            newRow("Points") = player.GetPoints()
            newRow("GMs") = CountGMOpponents(player)
            newRow("IMs") = CountIMOpponents(player)
            newRow("FMs") = CountFMOpponents(player)
            newRow("WGMs") = CountWGMOpponents(player)
            newRow("WIMs") = CountWIMOpponents(player)
            newRow("WFMs") = CountWFMOpponents(player)
            newRow("Delta") = player.GetDelta()
            newRow("Perf") = GetAverageRating(player) + player.GetDelta()
            newRow("ARO") = GetAverageRating(player)
            newRow("GM raised") = RaiseGMAverageRating(player)
            newRow("IM raised") = RaiseIMAverageRating(player)
            newRow("WGM raised") = RaiseWGMAverageRating(player)
            newRow("WIM raised") = RaiseWIMAverageRating(player)

            For i = 0 To player.rounds.Count - 1
                newRow($"Round{i + 1}") = If(player.rounds(i) IsNot Nothing, player.rounds(i).ToString(), "")
            Next

            dt.Rows.Add(newRow)
        Next

        PlayersDataGridView.DataSource = dt

        For Each col As DataGridViewColumn In PlayersDataGridView.Columns
            col.HeaderText = col.Name
        Next
    End Sub

    Private Sub PlayersDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles PlayersDataGridView.CellFormatting
        If PlayersDataGridView.Columns("Norm") IsNot Nothing AndAlso e.ColumnIndex = PlayersDataGridView.Columns("Norm").Index Then
            If e.Value <> "" Then
                For Each cell As DataGridViewCell In PlayersDataGridView.Rows(e.RowIndex).Cells
                    cell.Style.BackColor = Color.Green
                Next
            Else
                For Each cell As DataGridViewCell In PlayersDataGridView.Rows(e.RowIndex).Cells
                    cell.Style.BackColor = Color.White
                Next
            End If
        End If

    End Sub

    Function GetNorm(ByRef player As Player) As String
        If Not (twoFederations Or CheckFederation(player)) Or player.playerTitle = Player.Title.GM Then
            Return ""
        End If

        Dim title As String = ""
        Dim delta As Single = player.GetDelta()
        Dim WFMs = CountWFMOpponents(player)
        Dim WIMs = CountWIMOpponents(player)
        Dim WGMs = CountWGMOpponents(player)
        Dim FMs = CountFMOpponents(player)
        Dim IMs = CountIMOpponents(player)
        Dim GMs = CountGMOpponents(player)
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

        If points / count < 0.35 Then
            Return ""
        End If


        If player.playerTitle < Player.Title.WIM And
            player.playerSex = Player.Sex.W And
            (WFMs + WIMs + WGMs + FMs + IMs + GMs) / count > 1 / 2 And
            (WIMs + WGMs + IMs + GMs) / count > 1 / 3 And
            (WIMs + WGMs + IMs + GMs) >= 3 And
            RaiseWIMAverageRating(player) + delta >= 2250 Then
            title = "WIM"
        End If
        If player.playerTitle < Player.Title.WGM And
            player.playerSex = Player.Sex.W And
            (WFMs + WIMs + WGMs + FMs + IMs + GMs) / count > 1 / 2 And
            (WGMs + IMs + GMs) / count > 1 / 3 And
            (WGMs + IMs + GMs) >= 3 And
            RaiseWGMAverageRating(player) + delta >= 2400 Then
            title = "WGM"
        End If

        If player.playerTitle < Player.Title.IM And
            (WFMs + WIMs + WGMs + FMs + IMs + GMs) / count > 1 / 2 And
            (IMs + GMs) / count > 1 / 3 And
            (IMs + GMs) >= 3 And
            RaiseIMAverageRating(player) + delta >= 2450 Then
            title = "IM"
        End If

        If player.playerTitle < Player.Title.GM And
            (WFMs + WIMs + WGMs + FMs + IMs + GMs) / count > 1 / 2 And
            GMs / count > 1 / 3 And
            GMs >= 3 And
            RaiseGMAverageRating(player) + delta >= 2600 Then
            title = "GM"
        End If

        Return title
    End Function

    Function CheckFederation(ByRef player As Player) As Boolean
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

    Public Function GetAverageRating(ByRef player As Player) As Single
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
            Return sum / count
        End If
    End Function

    Public Function RaiseTitleAverageRating(ByRef player As Player, floor As UShort) As Single
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

        Return sum / count
    End Function

    Public Function RaiseGMAverageRating(ByRef player As Player) As Single
        Return RaiseTitleAverageRating(player, 2200)
    End Function

    Public Function RaiseIMAverageRating(ByRef player As Player) As Single
        Return RaiseTitleAverageRating(player, 2050)
    End Function

    Public Function RaiseWGMAverageRating(ByRef player As Player) As Single
        Return RaiseTitleAverageRating(player, 2000)
    End Function

    Public Function RaiseWIMAverageRating(ByRef player As Player) As Single
        Return RaiseTitleAverageRating(player, 1850)
    End Function


    Public Function CountTitleOpponents(ByRef player As Player, title As Player.Title) As Byte
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


    Public Function CountGMOpponents(ByRef player As Player) As Byte
        Return CountTitleOpponents(player, Player.Title.GM)
    End Function
    Public Function CountIMOpponents(ByRef player As Player) As Byte
        Return CountTitleOpponents(player, Player.Title.IM)
    End Function
    Public Function CountFMOpponents(ByRef player As Player) As Byte
        Return CountTitleOpponents(player, Player.Title.FM)
    End Function
    Public Function CountWGMOpponents(ByRef player As Player) As Byte
        Return CountTitleOpponents(player, Player.Title.WGM)
    End Function
    Public Function CountWIMOpponents(ByRef player As Player) As Byte
        Return CountTitleOpponents(player, Player.Title.WIM)
    End Function

    Public Function CountWFMOpponents(ByRef player As Player) As Byte
        Return CountTitleOpponents(player, Player.Title.WFM)
    End Function
    Private Sub Options_Click(sender As Object, e As EventArgs) Handles Options.Click
        If OptionsForm.ShowDialog() = DialogResult.OK Then
            twoFederations = OptionsForm.twoFederationCB.Checked
            roundRobinUnrated = OptionsForm.RRUnratedCB.Checked
            RefreshData()
        End If
    End Sub
End Class
