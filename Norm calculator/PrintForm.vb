
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
            GenerateIT1RtfFile()
        ElseIf IT3RB.Checked Then
            GenerateIT3RtfFile()
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GenerateIT3RtfFile()
        Dim template As String = LoadIT3Template()
        Dim result As New Text.StringBuilder()

        result.AppendLine("{\rtf1\ansi\deff3\adeflang1025")

        Dim roundsPerDay As New Text.StringBuilder()

        Dim roundsInDay As Byte = 0
        For i As Integer = 0 To CalculatorForm.rounds.Count - 1
            If i = 0 Then
                roundsInDay = 1
            ElseIf CalculatorForm.rounds(i) = CalculatorForm.rounds(i - 1) Then
                roundsInDay += 1
            Else
                roundsPerDay.Append(roundsInDay & " ")
                roundsInDay = 1
            End If
        Next
        roundsPerDay.Append(roundsInDay)

        Dim modified As String = template _
        .Replace("[$fed$]", EscapeRtf(CalculatorForm.federation)) _
        .Replace("[$name$]", EscapeRtf(CalculatorForm.eventName)) _
        .Replace("[$city$]", EscapeRtf(CalculatorForm.city)) _
        .Replace("[$start$]", EscapeRtf(CalculatorForm.startDate)) _
        .Replace("[$date$]", EscapeRtf(CalculatorForm.endDate)) _
        .Replace("[$organizer$]", "___________________________________") _
        .Replace("[$organiz$]", "___________________________________") _
        .Replace("[$ro$]", CalculatorForm.rounds.Count) _
        .Replace("[$ro_p_d$]", roundsPerDay.ToString()) _
        .Replace("[$rate$]", EscapeRtf(CalculatorForm.rate)) _
        .Replace("[$system$]", EscapeRtf(CalculatorForm.system)) _
        .Replace("[$chief_arb$]", EscapeRtf(CalculatorForm.chief))

        Dim ratedHost As UShort = 0
        Dim ratedNoHost As UShort = 0
        Dim unratedHost As UShort = 0
        Dim unratedNoHost As UShort = 0

        Dim hostGMs As UShort = 0
        Dim noHostGMs As UShort = 0
        Dim hostIMs As UShort = 0
        Dim noHostIMs As UShort = 0
        Dim hostFMs As UShort = 0
        Dim noHostFMs As UShort = 0
        Dim hostWGMs As UShort = 0
        Dim noHostWGMs As UShort = 0
        Dim hostWIMs As UShort = 0
        Dim noHostWIMs As UShort = 0
        Dim hostWFMs As UShort = 0
        Dim noHostWFMs As UShort = 0


        Dim ratedCountries As New HashSet(Of String)
        Dim gmCountries As New HashSet(Of String)
        Dim imCountries As New HashSet(Of String)
        Dim fmCountries As New HashSet(Of String)
        Dim unratedCountries As New HashSet(Of String)
        Dim wgmCountries As New HashSet(Of String)
        Dim wimCountries As New HashSet(Of String)
        Dim wfmCountries As New HashSet(Of String)

        For Each kvp As KeyValuePair(Of UShort, Player) In CalculatorForm.playersDict
            Dim player As Player = kvp.Value

            If player.rating >= 1400 Then
                ratedCountries.Add(player.federation)
                If player.federation = CalculatorForm.federation Then
                    ratedHost += 1
                Else
                    ratedNoHost += 1
                End If
            Else
                unratedCountries.Add(player.federation)
                If player.federation = CalculatorForm.federation Then
                    unratedHost += 1
                Else
                    unratedNoHost += 1
                End If
            End If

            Select Case player.playerTitle
                Case Player.Title.GM
                    gmCountries.Add(player.federation)
                    If player.federation = CalculatorForm.federation Then
                        hostGMs += 1
                    Else
                        noHostGMs += 1
                    End If

                Case Player.Title.IM
                    imCountries.Add(player.federation)
                    If player.federation = CalculatorForm.federation Then
                        hostIMs += 1
                    Else
                        noHostIMs += 1
                    End If

                Case Player.Title.FM
                    fmCountries.Add(player.federation)
                    If player.federation = CalculatorForm.federation Then
                        hostFMs += 1
                    Else
                        noHostFMs += 1
                    End If

                Case Player.Title.WGM
                    wgmCountries.Add(player.federation)
                    If player.federation = CalculatorForm.federation Then
                        hostWGMs += 1
                    Else
                        noHostWGMs += 1
                    End If

                Case Player.Title.WIM
                    wimCountries.Add(player.federation)
                    If player.federation = CalculatorForm.federation Then
                        hostWIMs += 1
                    Else
                        noHostWIMs += 1
                    End If

                Case Player.Title.WFM
                    wfmCountries.Add(player.federation)
                    If player.federation = CalculatorForm.federation Then
                        hostWFMs += 1
                    Else
                        noHostWFMs += 1
                    End If

            End Select

        Next

        modified = modified _
        .Replace("[$n_ra$]", ratedHost + ratedNoHost) _
        .Replace("[$n_f$]", ratedCountries.Count) _
        .Replace("[$n_hf$]", ratedHost) _
        .Replace("[$n_ofp$]", ratedNoHost) _
        .Replace("[$n_ura$]", unratedHost + unratedNoHost) _
        .Replace("[$n_urh$]", unratedCountries.Count) _
        .Replace("[$n_uhf$]", unratedHost) _
        .Replace("[$n_uofp$]", unratedNoHost) _
        .Replace("[$n_gm$]", hostGMs + noHostGMs) _
        .Replace("[$n_fgm$]", gmCountries.Count) _
        .Replace("[$n_hgm$]", hostGMs) _
        .Replace("[$n_ogm$]", noHostGMs) _
        .Replace("[$n_wgm$]", hostWGMs + noHostWGMs) _
        .Replace("[$n_fwgm$]", wgmCountries.Count) _
        .Replace("[$n_hwgm$]", hostWGMs) _
        .Replace("[$n_owgm$]", noHostWGMs) _
        .Replace("[$n_im$]", hostIMs + noHostIMs) _
        .Replace("[$n_fim$]", imCountries.Count) _
        .Replace("[$n_him$]", hostIMs) _
        .Replace("[$n_oim$]", noHostIMs) _
        .Replace("[$n_wim$]", hostWIMs + noHostWIMs) _
        .Replace("[$n_fwim$]", wimCountries.Count) _
        .Replace("[$n_hwim$]", hostWIMs) _
        .Replace("[$n_owim$]", noHostWIMs) _
        .Replace("[$n_fm$]", hostFMs + noHostFMs) _
        .Replace("[$n_ffm$]", fmCountries.Count) _
        .Replace("[$n_hfm$]", hostFMs) _
        .Replace("[$n_ofm$]", noHostFMs) _
        .Replace("[$n_wfm$]", hostWFMs + noHostWFMs) _
        .Replace("[$n_fwfm$]", wfmCountries.Count) _
        .Replace("[$n_hwfm$]", hostWFMs) _
        .Replace("[$n_owfm$]", noHostWFMs)

        result.Append(modified)

        If result.Length = 0 Then
            MessageBox.Show("No data found for IT3 report.")
            Return
        End If

        result.AppendLine("}")

        Dim filePath As String = SaveRtfFile(result.ToString(), "IT3_")
        MessageBox.Show($"IT3 Report created:{Environment.NewLine}{filePath}")
    End Sub

    Private Function LoadIT3Template() As String
        Dim asm = Reflection.Assembly.GetExecutingAssembly()
        Dim resName = asm.GetManifestResourceNames().First(Function(n) n.EndsWith("IT3_template.rtf"))

        Using sr As New IO.StreamReader(asm.GetManifestResourceStream(resName))
            Return sr.ReadToEnd()
        End Using
    End Function

    Private Function SaveRtfFile(content As String, Optional prefix As String = "NormCertificates_") As String
        Dim path As String = IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
        $"{prefix}{DateTime.Now:yyyyMMdd_HHmm}.rtf")

        IO.File.WriteAllText(path, content)
        Return path
    End Function


    Private Sub GenerateIT1RtfFile()
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