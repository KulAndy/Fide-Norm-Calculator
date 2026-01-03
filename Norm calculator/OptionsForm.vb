Public Class OptionsForm
    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class