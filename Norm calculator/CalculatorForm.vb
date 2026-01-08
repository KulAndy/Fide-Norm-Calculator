Imports System.Globalization

Public Class CalculatorForm
    Public eventName As String = ""
    Public city As String = ""
    Public federation As String = ""
    Public startDate As Date = Nothing
    Public endDate As Date = Nothing
    Public playersNo As UShort = 0
    Public ratedPlayersNo As UShort = 0
    Public system As String = ""
    Public chief As String = ""
    Public deputies As New List(Of String)()
    Public rate As String = ""
    Public rounds As New List(Of Date)()
    Public playersDict As New Dictionary(Of UShort, Player)

    Public twoFederations As Boolean = True
    Dim onlyGained As Boolean = False

    Private Sub OpenFile_Click(sender As Object, e As EventArgs) Handles OpenFile.Click
        If SelectReportDialog.ShowDialog = DialogResult.OK Then
            eventName = ""
            city = ""
            federation = ""
            startDate = Nothing
            endDate = Nothing
            playersNo = 0
            ratedPlayersNo = 0
            system = ""
            chief = ""
            deputies = New List(Of String)()
            rate = ""
            rounds = New List(Of Date)()
            playersDict = New Dictionary(Of UShort, Player)
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
            Dim norm = GetNorm(player, playersDict)
            newRow("Norm") = norm
            If norm = "" And onlyGained Then
                Continue For
            End If

            newRow("Points") = player.GetPoints()
            newRow("GMs") = CountGMOpponents(player, playersDict)
            newRow("IMs") = CountIMOpponents(player, playersDict)
            newRow("FMs") = CountFMOpponents(player, playersDict)
            newRow("WGMs") = CountWGMOpponents(player, playersDict)
            newRow("WIMs") = CountWIMOpponents(player, playersDict)
            newRow("WFMs") = CountWFMOpponents(player, playersDict)
            newRow("Delta") = player.GetDelta()
            newRow("Perf") = GetAverageRating(player, playersDict) + player.GetDelta()
            newRow("ARO") = GetAverageRating(player, playersDict)
            newRow("GM raised") = RaiseGMAverageRating(player, playersDict)
            newRow("IM raised") = RaiseIMAverageRating(player, playersDict)
            newRow("WGM raised") = RaiseWGMAverageRating(player, playersDict)
            newRow("WIM raised") = RaiseWIMAverageRating(player, playersDict)

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

    Private Sub Options_Click(sender As Object, e As EventArgs) Handles Options.Click
        If OptionsForm.ShowDialog() = DialogResult.OK Then
            twoFederations = OptionsForm.twoFederationCB.Checked
            onlyGained = OptionsForm.OnlyGainedCB.Checked
            RefreshData()
        End If
    End Sub

    Private Sub PrintMenu_Click(sender As Object, e As EventArgs) Handles PrintMenu.Click
        PrintForm.ShowDialog()
    End Sub
End Class
