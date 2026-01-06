
Public Class PrintForm
    Public Enum FormType
        IT1
        IT3
    End Enum

    Const emptyCirlceRTF = "\u9675?"
    Const filledCirlceRTF = "\u9679?"

    Dim notHostTitled = 0
    Dim countryNo As Byte = 0
    Dim countryNo143d As Byte = 0
    Dim ratedNoHost As Byte = 0


    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        notHostTitled = 0
        countryNo = 0
        countryNo143d = 0
        ratedNoHost = 0
        Dim countries As New Dictionary(Of String, Byte)
        For Each kvp As KeyValuePair(Of UShort, Player) In CalculatorForm.playersDict
            Dim player As Player = kvp.Value

            If player.CountReadyRounds() = 0 Then Continue For

            Dim fed As String = player.federation
            Dim playedEntireTournament As Byte = 1

            If player.federation <> CalculatorForm.federation And player.Count143d() Then
                Select Case player.playerTitle
                    Case Player.Title.GM, Player.Title.IM, Player.Title.WGM, Player.Title.WIM
                        notHostTitled += 1
                End Select

                If player.rating >= 1400 Then
                    ratedNoHost += 1
                End If

                playedEntireTournament = 0
            End If

            If countries.ContainsKey(fed) Then
                countries(fed) += playedEntireTournament
            Else
                countries(fed) = playedEntireTournament
            End If
        Next

        For Each kvp As KeyValuePair(Of String, Byte) In countries
            countryNo += 1
            If CalculatorForm.federation <> kvp.Key And "FID" <> kvp.Key And kvp.Value >= 0 Then
                Debug.WriteLine(kvp.Key)
                countryNo143d += 1
            End If
        Next


        If IT1RB.Checked Then
            GenerateNormRtfFile()
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GenerateNormRtfFile()
        Dim template As String = LoadRtfTemplate()
        Dim result As New Text.StringBuilder()

        result.AppendLine("{\rtf1\ansi\deff3\adeflang1025")

        For Each row As DataGridViewRow In CalculatorForm.PlayersDataGridView.Rows
            If row.IsNewRow Then Continue For

            Dim norm As String = row.Cells("Norm").Value?.ToString()
            If String.IsNullOrWhiteSpace(norm) Then Continue For

            Dim startNo As UShort = CUShort(row.Cells("Start").Value)
            Dim player As Player = CalculatorForm.playersDict(startNo)

            result.Append(BuildPlayerRtf(template, player, norm))
            result.Append("\page ")
        Next

        If result.Length = 0 Then
            MessageBox.Show("No norms found.")
            Return
        End If

        result.AppendLine("}")

        Dim filePath As String = SaveRtfFile(result.ToString())
        MessageBox.Show($"File created:{Environment.NewLine}{filePath}")
    End Sub

    Private Function LoadRtfTemplate() As String
        Dim asm = Reflection.Assembly.GetExecutingAssembly()
        Dim resName = asm.GetManifestResourceNames().
                  First(Function(n) n.EndsWith("IT1_template.rtf"))

        Using sr As New IO.StreamReader(asm.GetManifestResourceStream(resName))
            Return sr.ReadToEnd()
        End Using
    End Function


    Private Function BuildPlayerRtf(
    template As String,
    player As Player,
    norm As String) As String

        Dim gmCirle = emptyCirlceRTF
        Dim imCirle = emptyCirlceRTF
        Dim wgmCirle = emptyCirlceRTF
        Dim wimCirle = emptyCirlceRTF

        Dim modified = template _
        .Replace("[$last_name$]", EscapeRtf(player.GetLastName)) _
        .Replace("[$first_name$]", EscapeRtf(player.GetFirstName)) _
        .Replace("[$f_code$]", EscapeRtf(player.fideId)) _
        .Replace("[$fed$]", EscapeRtf(player.federation)) _
        .Replace("[$event$]", EscapeRtf(CalculatorForm.eventName)) _
        .Replace("[$start$]", EscapeRtf(CalculatorForm.startDate)) _
        .Replace("[$close$]", EscapeRtf(CalculatorForm.endDate)) _
        .Replace("[$chief_arbiter$]", EscapeRtf(CalculatorForm.chief)) _
        .Replace("[$games$]", CalculatorForm.rounds.Count) _
        .Replace("[$rate_of_play$]", EscapeRtf(CalculatorForm.rate)) _
        .Replace("[$nophf$]", notHostTitled) _
        .Replace("[$s_arch$]", player.GetPoints())

        Dim points = player.GetPoints()
        Dim requiredPoints = Single.NaN
        Dim floor = 0

        Select Case norm
            Case "GM"
                modified = modified.Replace("[$Ra$]", CalculatorForm.RaiseGMAverageRating(player))
                gmCirle = filledCirlceRTF
                requiredPoints = CalculatorForm.GetRequiredNormPoints(player, Player.Title.GM)
                floor = 2200
            Case "IM"
                modified = modified.Replace("[$Ra$]", CalculatorForm.RaiseIMAverageRating(player))
                imCirle = filledCirlceRTF
                requiredPoints = CalculatorForm.GetRequiredNormPoints(player, Player.Title.IM)
                floor = 2050
            Case "WGM"
                modified = modified.Replace("[$Ra$]", CalculatorForm.RaiseWGMAverageRating(player))
                wgmCirle = filledCirlceRTF
                requiredPoints = CalculatorForm.GetRequiredNormPoints(player, Player.Title.WGM)
                floor = 2000
            Case "WIM"
                modified = modified.Replace("[$Ra$]", CalculatorForm.RaiseWIMAverageRating(player))
                wimCirle = filledCirlceRTF
                requiredPoints = CalculatorForm.GetRequiredNormPoints(player, Player.Title.WIM)
                floor = 1850
            Case Else
                modified = modified.Replace("[$Ra$]", CalculatorForm.GetAverageRating(player))
        End Select


        requiredPoints = CSng(Math.Ceiling(requiredPoints * 2) / 2)
        modified = modified _
            .Replace("[$GM_CIRCLE$]", gmCirle) _
            .Replace("[$IM_CIRCLE$]", imCirle) _
            .Replace("[$WGM_CIRCLE$]", wgmCirle) _
            .Replace("[$WIM_CIRCLE$]", wimCirle) _
            .Replace("[$s_arch$]", points) _
            .Replace("[$s_req$]", requiredPoints) _
            .Replace("[$points$]", points - requiredPoints)

        Dim countryNotPlayerNo As Byte = 0
        Dim countryHostPlayerNo As Byte = 0
        Dim rated As Byte = 0
        Dim minRating = UShort.MaxValue

        For Each roundItem As Round In player.rounds
            Try
                Select Case roundItem.GameResult
                    Case Round.Result.WIN_NO1, Round.Result.WIN, Round.Result.DRAW_NO1, Round.Result.DRAW, Round.Result.LOSE_NO1, Round.Result.LOSE
                        Dim opponent = CalculatorForm.playersDict(roundItem.OpponentID)
                        Dim fed As String = opponent.federation

                        If player.federation <> opponent.federation Then
                            countryNotPlayerNo += 1
                        End If
                        If CalculatorForm.federation = opponent.federation Then
                            countryHostPlayerNo += 1
                        End If

                        If opponent.rating >= 1400 Then
                            rated += 1
                            minRating = UShort.Min(minRating, opponent.rating)
                        End If
                End Select
            Catch ex As Exception
            End Try
        Next

        modified = modified _
            .Replace("[$no_fed$]", countryNo) _
            .Replace("[$143_nf$]", countryNo143d) _
            .Replace("[$nro$]", rated) _
            .Replace("[$nrpnfhf$]", ratedNoHost) _
            .Replace("[$tfe$]", countryNotPlayerNo) _
            .Replace("[$nphf$]", countryHostPlayerNo)

        Dim WFMs = CalculatorForm.CountWFMOpponents(player)
        Dim WIMs = CalculatorForm.CountWIMOpponents(player)
        Dim WGMs = CalculatorForm.CountWGMOpponents(player)
        Dim FMs = CalculatorForm.CountFMOpponents(player)
        Dim IMs = CalculatorForm.CountIMOpponents(player)
        Dim GMs = CalculatorForm.CountGMOpponents(player)

        modified = modified _
            .Replace("[$tnto$]", WFMs + WIMs + WGMs + FMs + IMs + GMs) _
            .Replace("[$ngm$]", GMs) _
            .Replace("[$nim$]", IMs) _
            .Replace("[$fm$]", FMs) _
            .Replace("[$wgm$]", WGMs) _
            .Replace("[$wim$]", WIMs) _
            .Replace("[$wfm$]", WFMs)

        Dim tableRtf As String = GeneratePlayerTableRtf(player, minRating, floor)
        modified = modified.Replace("[$tabela$]", tableRtf)

        Return modified
    End Function


    Private Function GeneratePlayerTableRtf(player As Player, minRating As UShort, floor As UShort) As String
        Dim sb As New Text.StringBuilder()

        sb.AppendLine("\pard\plain\ql\li0\ri0\fi0\tx0\par")
        Dim cols As String =
        "\cellx0" &
        "\cellx3000" &
        "\cellx5000" &
        "\cellx6500" &
        "\cellx7500" &
        "\cellx8500" &
        "\cellx9500" &
        "\cellx10500"

        sb.AppendLine("\trowd\trleft-1000\trgaph0")
        sb.AppendLine(cols)
        sb.AppendLine("\pard\intbl\b Rd\b0\cell\b Opponent\b0\cell\b ID\b0\cell\b Fed\b0\cell\b Rating\b0\cell\b Rat. 1.46c\b0\cell\b Title\b0\cell\b Score\b0\cell\row")

        Dim usedFloor = False
        For i As Integer = 0 To player.rounds.Count - 1
            Dim roundItem As Round = player.rounds(i)

            Try
                Dim opponent As Player = CalculatorForm.playersDict(roundItem.OpponentID)
                Dim score As String = GetScoreSymbol(roundItem.GameResult)
                Dim rating146c As String = ""
                If opponent.rating = minRating And minRating < floor And Not usedFloor Then
                    usedFloor = True
                    rating146c = floor.ToString()
                End If

                sb.AppendLine("\trowd\trleft-1000\trgaph0")
                sb.AppendLine(cols)

                sb.AppendLine(
                "\pard\intbl " &
                (i + 1) & "\cell " &
                EscapeRtf(opponent.GetLastName & " " & opponent.GetFirstName) & "\cell " &
                opponent.fideId & "\cell " &
                opponent.federation & "\cell " &
                opponent.rating & "\cell " &
                rating146c & "\cell " &
                If(opponent.playerTitle <> Player.Title.NONE, opponent.playerTitle.ToString(), "") & "\cell " &
                score & "\cell\row"
            )

            Catch
                sb.AppendLine("\trowd\trleft-1000\trgaph0")
                sb.AppendLine(cols)
                sb.AppendLine(
                "\pard\intbl " &
                (i + 1) & "\cell \cell -\cell \cell \cell \cell \cell \cell\row"
            )
            End Try
        Next

        sb.AppendLine("\pard\plain\par")

        Return sb.ToString()
    End Function

    Private Function GetScoreSymbol(result As Round.Result) As String
        Select Case result
            Case Round.Result.WIN, Round.Result.WIN_NO1
                Return "1"
            Case Round.Result.DRAW, Round.Result.DRAW_NO1
                Return "\u189?"
            Case Round.Result.LOSE, Round.Result.LOSE_NO1
                Return "0"
            Case Else
                Return "-"
        End Select
    End Function

    Private Function SaveRtfFile(content As String) As String

        Dim path As String =
        IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            $"NormCertificates_{DateTime.Now:yyyyMMdd_HHmm}.rtf")

        IO.File.WriteAllText(path, content)
        Return path

    End Function

    Private Function EscapeRtf(text As String) As String
        If String.IsNullOrEmpty(text) Then Return ""
        Return text.Replace("\", "\\").Replace("{", "\{").Replace("}", "\}")
    End Function
End Class