Public MustInherit Class ITTemplate
    Protected Const emptyCircleRTF As String = "\u9675?"
    Protected Const filledCircleRTF As String = "\u9679?"

    Protected notHostTitled As Byte = 0
    Protected countryNo As Byte = 0
    Protected countryNo143d As Byte = 0
    Protected ratedNoHost As Byte = 0

    Protected Sub Count143()
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
                countryNo143d += 1
            End If
        Next
    End Sub
    Public MustOverride Sub GenerateRtfFile()

    Protected Function LoadTemplate(templateName As String) As String
        Dim asm = Reflection.Assembly.GetExecutingAssembly()
        Dim resName = asm.GetManifestResourceNames().First(Function(n) n.EndsWith(templateName))

        Using sr As New IO.StreamReader(asm.GetManifestResourceStream(resName))
            Return sr.ReadToEnd()
        End Using
    End Function

    Protected Function SaveRtfFile(content As String, Optional filename As String = "") As String
        Dim path As String = IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            If(filename = "", $"NormCertificates_{Date.Now:yyyyMMdd_HHmm}.rtf", filename)
        )

        IO.File.WriteAllText(path, content)
        Return path
    End Function

    Protected Function EscapeRtf(text As String) As String
        If String.IsNullOrEmpty(text) Then Return ""
        Return text.Replace("\", "\\").Replace("{", "\{").Replace("}", "\}")
    End Function
End Class
