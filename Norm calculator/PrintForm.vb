
Public Class PrintForm
    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        If IT1RB.Checked Then
            Dim it1 As New IT1Form()
            it1.GenerateRtfFile()
        ElseIf IT3RB.Checked Then
            Dim it3 As New IT3Form()
            it3.GenerateRtfFile()
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class