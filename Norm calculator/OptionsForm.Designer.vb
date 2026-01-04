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
        OnlyGainedCB = New CheckBox()
        SuspendLayout()
        ' 
        ' twoFederationCB
        ' 
        twoFederationCB.AutoSize = True
        twoFederationCB.Checked = True
        twoFederationCB.CheckState = CheckState.Checked
        twoFederationCB.Location = New Point(12, 11)
        twoFederationCB.Name = "twoFederationCB"
        twoFederationCB.Size = New Size(96, 19)
        twoFederationCB.TabIndex = 0
        twoFederationCB.Text = "2 Federations"
        twoFederationCB.UseVisualStyleBackColor = True
        ' 
        ' RRUnratedCB
        ' 
        RRUnratedCB.AutoSize = True
        RRUnratedCB.Location = New Point(12, 34)
        RRUnratedCB.Name = "RRUnratedCB"
        RRUnratedCB.Size = New Size(133, 19)
        RRUnratedCB.TabIndex = 1
        RRUnratedCB.Text = "round robin unrated"
        RRUnratedCB.UseVisualStyleBackColor = True
        ' 
        ' OKButton
        ' 
        OKButton.Location = New Point(219, 113)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(75, 20)
        OKButton.TabIndex = 2
        OKButton.Text = "OK"
        OKButton.UseVisualStyleBackColor = True
        ' 
        ' CancelButton
        ' 
        CancelButton.Location = New Point(315, 113)
        CancelButton.Name = "CancelButton"
        CancelButton.Size = New Size(75, 20)
        CancelButton.TabIndex = 3
        CancelButton.Text = "Cancel"
        CancelButton.UseVisualStyleBackColor = True
        ' 
        ' OnlyGainedCB
        ' 
        OnlyGainedCB.AutoSize = True
        OnlyGainedCB.Location = New Point(12, 59)
        OnlyGainedCB.Name = "OnlyGainedCB"
        OnlyGainedCB.Size = New Size(90, 19)
        OnlyGainedCB.TabIndex = 4
        OnlyGainedCB.Text = "Only gained"
        OnlyGainedCB.UseVisualStyleBackColor = True
        ' 
        ' OptionsForm
        ' 
        AcceptButton = OKButton
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(402, 144)
        Controls.Add(OnlyGainedCB)
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
    Friend WithEvents OnlyGainedCB As CheckBox
End Class
