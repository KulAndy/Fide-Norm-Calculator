<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsForm
    Inherits System.Windows.Forms.Form

    'Formularz przesłania metodę dispose, aby wyczyścić listę składników.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wymagane przez Projektanta formularzy systemu Windows
    Private components As System.ComponentModel.IContainer

    'UWAGA: następująca procedura jest wymagana przez Projektanta formularzy systemu Windows
    'Możesz to modyfikować, używając Projektanta formularzy systemu Windows. 
    'Nie należy modyfikować za pomocą edytora kodu.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        twoFederationCB = New CheckBox()
        RRUnratedCB = New CheckBox()
        OKButton = New Button()
        CancelButton = New Button()
        SuspendLayout()
        ' 
        ' twoFederationCB
        ' 
        twoFederationCB.AutoSize = True
        twoFederationCB.Checked = True
        twoFederationCB.CheckState = CheckState.Checked
        twoFederationCB.Location = New Point(12, 12)
        twoFederationCB.Name = "twoFederationCB"
        twoFederationCB.Size = New Size(106, 21)
        twoFederationCB.TabIndex = 0
        twoFederationCB.Text = "2 Federations"
        twoFederationCB.UseVisualStyleBackColor = True
        ' 
        ' RRUnratedCB
        ' 
        RRUnratedCB.AutoSize = True
        RRUnratedCB.Location = New Point(12, 39)
        RRUnratedCB.Name = "RRUnratedCB"
        RRUnratedCB.Size = New Size(146, 21)
        RRUnratedCB.TabIndex = 1
        RRUnratedCB.Text = "round robin unrated"
        RRUnratedCB.UseVisualStyleBackColor = True
        ' 
        ' OKButton
        ' 
        OKButton.Location = New Point(219, 128)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(75, 23)
        OKButton.TabIndex = 2
        OKButton.Text = "OK"
        OKButton.UseVisualStyleBackColor = True
        ' 
        ' CancelButton
        ' 
        CancelButton.Location = New Point(315, 128)
        CancelButton.Name = "CancelButton"
        CancelButton.Size = New Size(75, 23)
        CancelButton.TabIndex = 3
        CancelButton.Text = "Cancel"
        CancelButton.UseVisualStyleBackColor = True
        ' 
        ' OptionsForm
        ' 
        AcceptButton = OKButton
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(402, 163)
        Controls.Add(CancelButton)
        Controls.Add(OKButton)
        Controls.Add(RRUnratedCB)
        Controls.Add(twoFederationCB)
        KeyPreview = True
        Name = "OptionsForm"
        Text = "Options"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents twoFederationCB As CheckBox
    Friend WithEvents RRUnratedCB As CheckBox
    Friend WithEvents OKButton As Button
    Friend WithEvents CancelButton As Button
End Class
