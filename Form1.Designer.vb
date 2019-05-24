<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenEncryptedSkylanderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenDecryptedSkylanderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsEncryptedSkylanderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsDecryptedSkylanderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PortalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReadSkylanderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WriteSkylanderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReadSwapperOtherHalfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WriteSwapperOtherHalfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RawDumpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectToPortalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetFigureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetSwapperOtherHalfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowMetadataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FixCRCsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PortalToDecryptedFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(103, 190)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(151, 21)
        Me.ComboBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Figure Name:"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.PortalToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(410, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenEncryptedSkylanderToolStripMenuItem, Me.OpenDecryptedSkylanderToolStripMenuItem, Me.SaveAsEncryptedSkylanderToolStripMenuItem, Me.SaveAsDecryptedSkylanderToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenEncryptedSkylanderToolStripMenuItem
        '
        Me.OpenEncryptedSkylanderToolStripMenuItem.Name = "OpenEncryptedSkylanderToolStripMenuItem"
        Me.OpenEncryptedSkylanderToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.OpenEncryptedSkylanderToolStripMenuItem.Text = "Open Encrypted Skylander"
        '
        'OpenDecryptedSkylanderToolStripMenuItem
        '
        Me.OpenDecryptedSkylanderToolStripMenuItem.Name = "OpenDecryptedSkylanderToolStripMenuItem"
        Me.OpenDecryptedSkylanderToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.OpenDecryptedSkylanderToolStripMenuItem.Text = "Open Decrypted Skylander"
        '
        'SaveAsEncryptedSkylanderToolStripMenuItem
        '
        Me.SaveAsEncryptedSkylanderToolStripMenuItem.Name = "SaveAsEncryptedSkylanderToolStripMenuItem"
        Me.SaveAsEncryptedSkylanderToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.SaveAsEncryptedSkylanderToolStripMenuItem.Text = "Save as Encrypted Skylander"
        '
        'SaveAsDecryptedSkylanderToolStripMenuItem
        '
        Me.SaveAsDecryptedSkylanderToolStripMenuItem.Name = "SaveAsDecryptedSkylanderToolStripMenuItem"
        Me.SaveAsDecryptedSkylanderToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.SaveAsDecryptedSkylanderToolStripMenuItem.Text = "Save as Decrypted Skylander"
        '
        'PortalToolStripMenuItem
        '
        Me.PortalToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReadSkylanderToolStripMenuItem, Me.WriteSkylanderToolStripMenuItem, Me.ReadSwapperOtherHalfToolStripMenuItem, Me.WriteSwapperOtherHalfToolStripMenuItem, Me.RawDumpToolStripMenuItem, Me.PortalToDecryptedFileToolStripMenuItem, Me.ConnectToPortalToolStripMenuItem})
        Me.PortalToolStripMenuItem.Name = "PortalToolStripMenuItem"
        Me.PortalToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.PortalToolStripMenuItem.Text = "Portal"
        '
        'ReadSkylanderToolStripMenuItem
        '
        Me.ReadSkylanderToolStripMenuItem.Name = "ReadSkylanderToolStripMenuItem"
        Me.ReadSkylanderToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ReadSkylanderToolStripMenuItem.Text = "Read Skylander"
        '
        'WriteSkylanderToolStripMenuItem
        '
        Me.WriteSkylanderToolStripMenuItem.Name = "WriteSkylanderToolStripMenuItem"
        Me.WriteSkylanderToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.WriteSkylanderToolStripMenuItem.Text = "Write Skylander"
        '
        'ReadSwapperOtherHalfToolStripMenuItem
        '
        Me.ReadSwapperOtherHalfToolStripMenuItem.Name = "ReadSwapperOtherHalfToolStripMenuItem"
        Me.ReadSwapperOtherHalfToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ReadSwapperOtherHalfToolStripMenuItem.Text = "Read Swapper Other Half"
        '
        'WriteSwapperOtherHalfToolStripMenuItem
        '
        Me.WriteSwapperOtherHalfToolStripMenuItem.Name = "WriteSwapperOtherHalfToolStripMenuItem"
        Me.WriteSwapperOtherHalfToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.WriteSwapperOtherHalfToolStripMenuItem.Text = "Write Swapper Other Half"
        '
        'RawDumpToolStripMenuItem
        '
        Me.RawDumpToolStripMenuItem.Name = "RawDumpToolStripMenuItem"
        Me.RawDumpToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.RawDumpToolStripMenuItem.Text = "Portal to Encrypted File"
        '
        'ConnectToPortalToolStripMenuItem
        '
        Me.ConnectToPortalToolStripMenuItem.Name = "ConnectToPortalToolStripMenuItem"
        Me.ConnectToPortalToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ConnectToPortalToolStripMenuItem.Text = "Connect to Portal"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResetFigureToolStripMenuItem, Me.ResetSwapperOtherHalfToolStripMenuItem, Me.ShowMetadataToolStripMenuItem, Me.FixCRCsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'ResetFigureToolStripMenuItem
        '
        Me.ResetFigureToolStripMenuItem.Name = "ResetFigureToolStripMenuItem"
        Me.ResetFigureToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ResetFigureToolStripMenuItem.Text = "Reset Figure"
        '
        'ResetSwapperOtherHalfToolStripMenuItem
        '
        Me.ResetSwapperOtherHalfToolStripMenuItem.Name = "ResetSwapperOtherHalfToolStripMenuItem"
        Me.ResetSwapperOtherHalfToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ResetSwapperOtherHalfToolStripMenuItem.Text = "Reset Swapper Other Half"
        '
        'ShowMetadataToolStripMenuItem
        '
        Me.ShowMetadataToolStripMenuItem.Name = "ShowMetadataToolStripMenuItem"
        Me.ShowMetadataToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ShowMetadataToolStripMenuItem.Text = "Show Metadata"
        '
        'FixCRCsToolStripMenuItem
        '
        Me.FixCRCsToolStripMenuItem.Name = "FixCRCsToolStripMenuItem"
        Me.FixCRCsToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.FixCRCsToolStripMenuItem.Text = "Fix CRCs"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Element:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Skills:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 193)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Hat:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 222)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Exp. Points:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 250)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Gold:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(103, 219)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(109, 20)
        Me.TextBox1.TabIndex = 8
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(103, 247)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(109, 20)
        Me.TextBox2.TabIndex = 9
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(248, 27)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(150, 150)
        Me.PictureBox1.TabIndex = 10
        Me.PictureBox1.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(100, 41)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(16, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "- -"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(100, 68)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "- -"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(218, 222)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Level:"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(72, 120)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(57, 17)
        Me.RadioButton1.TabIndex = 14
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Path B"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(72, 97)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(57, 17)
        Me.RadioButton2.TabIndex = 15
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Path A"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 280)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(410, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 16
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(59, 17)
        Me.ToolStripStatusLabel1.Text = "Standby..."
        '
        'PortalToDecryptedFileToolStripMenuItem
        '
        Me.PortalToDecryptedFileToolStripMenuItem.Name = "PortalToDecryptedFileToolStripMenuItem"
        Me.PortalToDecryptedFileToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PortalToDecryptedFileToolStripMenuItem.Text = "Portal to Decrypted File"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(410, 302)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Skylander Editor"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PortalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenEncryptedSkylanderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenDecryptedSkylanderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsEncryptedSkylanderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAsDecryptedSkylanderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReadSkylanderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WriteSkylanderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReadSwapperOtherHalfToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WriteSwapperOtherHalfToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResetFigureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResetSwapperOtherHalfToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowMetadataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FixCRCsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConnectToPortalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RawDumpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents PortalToDecryptedFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
