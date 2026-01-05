<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrintForm
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
        FormTypeGroup = New GroupBox()
        IT3RB = New RadioButton()
        IT1RB = New RadioButton()
        CancelButton = New Button()
        OKButton = New Button()
        FormTypeGroup.SuspendLayout()
        SuspendLayout()
        ' 
        ' FormTypeGroup
        ' 
        FormTypeGroup.Controls.Add(IT3RB)
        FormTypeGroup.Controls.Add(IT1RB)
        FormTypeGroup.Location = New Point(12, 12)
        FormTypeGroup.Name = "FormTypeGroup"
        FormTypeGroup.Size = New Size(200, 100)
        FormTypeGroup.TabIndex = 0
        FormTypeGroup.TabStop = False
        FormTypeGroup.Text = "Form type"
        ' 
        ' IT3RB
        ' 
        IT3RB.AutoSize = True
        IT3RB.Location = New Point(6, 47)
        IT3RB.Name = "IT3RB"
        IT3RB.Size = New Size(171, 19)
        IT3RB.TabIndex = 1
        IT3RB.Text = "IT 3 (tournament summary)"
        IT3RB.UseVisualStyleBackColor = True
        ' 
        ' IT1RB
        ' 
        IT1RB.AutoSize = True
        IT1RB.Checked = True
        IT1RB.Location = New Point(6, 22)
        IT1RB.Name = "IT1RB"
        IT1RB.Size = New Size(124, 19)
        IT1RB.TabIndex = 0
        IT1RB.TabStop = True
        IT1RB.Text = "IT 1 (player norms)"
        IT1RB.UseVisualStyleBackColor = True
        ' 
        ' CancelButton
        ' 
        CancelButton.Location = New Point(325, 172)
        CancelButton.Name = "CancelButton"
        CancelButton.Size = New Size(75, 23)
        CancelButton.TabIndex = 1
        CancelButton.Text = "Cancel"
        CancelButton.UseVisualStyleBackColor = True
        ' 
        ' OKButton
        ' 
        OKButton.Location = New Point(244, 172)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(75, 23)
        OKButton.TabIndex = 2
        OKButton.Text = "OK"
        OKButton.UseVisualStyleBackColor = True
        ' 
        ' PrintForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(412, 207)
        Controls.Add(OKButton)
        Controls.Add(CancelButton)
        Controls.Add(FormTypeGroup)
        Name = "PrintForm"
        Text = "Print"
        FormTypeGroup.ResumeLayout(False)
        FormTypeGroup.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents FormTypeGroup As GroupBox
    Friend WithEvents IT3RB As RadioButton
    Friend WithEvents IT1RB As RadioButton
    Friend WithEvents CancelButton As Button
    Friend WithEvents OKButton As Button
End Class
