<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CalculatorForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CalculatorForm))
        ToolStrip1 = New ToolStrip()
        OpenFile = New ToolStripButton()
        SelectReportDialog = New OpenFileDialog()
        PlayersDataGridView = New DataGridView()
        StatusStrip1 = New StatusStrip()
        pathStatus = New ToolStripStatusLabel()
        EventLabel = New Label()
        CityLabel = New Label()
        RateLabel = New Label()
        ChiefLabel = New Label()
        Options = New ToolStripButton()
        ToolStrip1.SuspendLayout()
        CType(PlayersDataGridView, ComponentModel.ISupportInitialize).BeginInit()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.Items.AddRange(New ToolStripItem() {OpenFile, Options})
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(911, 25)
        ToolStrip1.TabIndex = 0
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' OpenFile
        ' 
        OpenFile.DisplayStyle = ToolStripItemDisplayStyle.Image
        OpenFile.Image = CType(resources.GetObject("OpenFile.Image"), Image)
        OpenFile.ImageTransparentColor = Color.Magenta
        OpenFile.Name = "OpenFile"
        OpenFile.Size = New Size(23, 22)
        OpenFile.Text = "&Open"
        ' 
        ' SelectReportDialog
        ' 
        SelectReportDialog.FileName = "OpenFileDialog1"
        SelectReportDialog.Filter = "Pliki *.txt|*.txt"
        ' 
        ' PlayersDataGridView
        ' 
        PlayersDataGridView.AllowUserToAddRows = False
        PlayersDataGridView.AllowUserToDeleteRows = False
        PlayersDataGridView.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        PlayersDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        PlayersDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        PlayersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        PlayersDataGridView.Location = New Point(12, 189)
        PlayersDataGridView.Name = "PlayersDataGridView"
        PlayersDataGridView.ReadOnly = True
        PlayersDataGridView.Size = New Size(876, 273)
        PlayersDataGridView.TabIndex = 3
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {pathStatus})
        StatusStrip1.Location = New Point(0, 452)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(911, 22)
        StatusStrip1.TabIndex = 4
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' pathStatus
        ' 
        pathStatus.Name = "pathStatus"
        pathStatus.Size = New Size(0, 17)
        ' 
        ' EventLabel
        ' 
        EventLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        EventLabel.BorderStyle = BorderStyle.FixedSingle
        EventLabel.Location = New Point(45, 25)
        EventLabel.Name = "EventLabel"
        EventLabel.Size = New Size(791, 23)
        EventLabel.TabIndex = 5
        ' 
        ' CityLabel
        ' 
        CityLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        CityLabel.BorderStyle = BorderStyle.FixedSingle
        CityLabel.Location = New Point(45, 59)
        CityLabel.Name = "CityLabel"
        CityLabel.Size = New Size(791, 23)
        CityLabel.TabIndex = 6
        ' 
        ' RateLabel
        ' 
        RateLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        RateLabel.BorderStyle = BorderStyle.FixedSingle
        RateLabel.Location = New Point(45, 98)
        RateLabel.Name = "RateLabel"
        RateLabel.Size = New Size(791, 23)
        RateLabel.TabIndex = 11
        ' 
        ' ChiefLabel
        ' 
        ChiefLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ChiefLabel.BorderStyle = BorderStyle.FixedSingle
        ChiefLabel.Location = New Point(45, 136)
        ChiefLabel.Name = "ChiefLabel"
        ChiefLabel.Size = New Size(791, 23)
        ChiefLabel.TabIndex = 12
        ' 
        ' Options
        ' 
        Options.DisplayStyle = ToolStripItemDisplayStyle.Image
        Options.Image = CType(resources.GetObject("Options.Image"), Image)
        Options.ImageTransparentColor = Color.Magenta
        Options.Name = "Options"
        Options.Size = New Size(23, 22)
        Options.Text = "O&ptions"
        ' 
        ' CalculatorForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(911, 474)
        Controls.Add(ChiefLabel)
        Controls.Add(RateLabel)
        Controls.Add(CityLabel)
        Controls.Add(EventLabel)
        Controls.Add(StatusStrip1)
        Controls.Add(PlayersDataGridView)
        Controls.Add(ToolStrip1)
        Name = "CalculatorForm"
        Text = "Norm Calculator"
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        CType(PlayersDataGridView, ComponentModel.ISupportInitialize).EndInit()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents OpenFile As ToolStripButton
    Friend WithEvents SelectReportDialog As OpenFileDialog
    Friend WithEvents PlayersDataGridView As DataGridView
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents pathStatus As ToolStripStatusLabel
    Friend WithEvents EventLabel As Label
    Friend WithEvents CityLabel As Label
    Friend WithEvents RateLabel As Label
    Friend WithEvents ChiefLabel As Label
    Friend WithEvents Options As ToolStripButton

End Class
