Public Class IT3Form
    Inherits ITTemplate

    Public Overrides Sub GenerateRtfFile()
        Count143()
        Dim template As String = LoadTemplate("IT3_template.rtf")
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

        Dim filePath As String = SaveRtfFile(result.ToString(), "IT3 " & CalculatorForm.eventName & ".rtf")
        MessageBox.Show($"IT3 Report created:{Environment.NewLine}{filePath}")
    End Sub
End Class
