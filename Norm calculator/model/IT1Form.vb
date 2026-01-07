Public Class IT1Form
    Inherits ITTemplate

    Public Overrides Sub GenerateRtfFile()
        Count143()
        Dim template As String = LoadTemplate("IT1_template.rtf")
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

        Dim filePath As String = SaveRtfFile(result.ToString(), "IT1 " & CalculatorForm.eventName & ".rtf")
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

        Dim gmCirle = emptyCircleRTF
        Dim imCirle = emptyCircleRTF
        Dim wgmCirle = emptyCircleRTF
        Dim wimCirle = emptyCircleRTF

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
                modified = modified.Replace("[$Ra$]", RaiseGMAverageRating(player, CalculatorForm.playersDict))
                gmCirle = filledCircleRTF
                requiredPoints = GetRequiredNormPoints(player, Player.Title.GM, CalculatorForm.playersDict)
                floor = 2200
            Case "IM"
                modified = modified.Replace("[$Ra$]", RaiseIMAverageRating(player, CalculatorForm.playersDict))
                imCirle = filledCircleRTF
                requiredPoints = GetRequiredNormPoints(player, Player.Title.IM, CalculatorForm.playersDict)
                floor = 2050
            Case "WGM"
                modified = modified.Replace("[$Ra$]", RaiseWGMAverageRating(player, CalculatorForm.playersDict))
                wgmCirle = filledCircleRTF
                requiredPoints = GetRequiredNormPoints(player, Player.Title.WGM, CalculatorForm.playersDict)
                floor = 2000
            Case "WIM"
                modified = modified.Replace("[$Ra$]", RaiseWIMAverageRating(player, CalculatorForm.playersDict))
                wimCirle = filledCircleRTF
                requiredPoints = GetRequiredNormPoints(player, Player.Title.WIM, CalculatorForm.playersDict)
                floor = 1850
            Case Else
                modified = modified.Replace("[$Ra$]", GetAverageRating(player, CalculatorForm.playersDict))
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

        Dim WFMs = CountWFMOpponents(player, CalculatorForm.playersDict)
        Dim WIMs = CountWIMOpponents(player, CalculatorForm.playersDict)
        Dim WGMs = CountWGMOpponents(player, CalculatorForm.playersDict)
        Dim FMs = CountFMOpponents(player, CalculatorForm.playersDict)
        Dim IMs = CountIMOpponents(player, CalculatorForm.playersDict)
        Dim GMs = CountGMOpponents(player, CalculatorForm.playersDict)

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
End Class
