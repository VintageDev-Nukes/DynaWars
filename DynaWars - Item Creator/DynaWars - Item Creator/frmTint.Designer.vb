<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTint
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
        Me.cdTintColor = New System.Windows.Forms.ColorDialog()
        Me.tbDepth = New System.Windows.Forms.TrackBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.lblColor = New System.Windows.Forms.Label()
        Me.txtSelectedFile = New System.Windows.Forms.TextBox()
        Me.btnTint = New System.Windows.Forms.Button()
        Me.lblDepth = New System.Windows.Forms.Label()
        Me.txtSavePath = New System.Windows.Forms.TextBox()
        Me.btnColorSelect = New System.Windows.Forms.Button()
        Me.ofdFileToTint = New System.Windows.Forms.OpenFileDialog()
        Me.sfdFileToSave = New System.Windows.Forms.SaveFileDialog()
        Me.btnSeeFile = New System.Windows.Forms.Button()
        Me.lblContrast = New System.Windows.Forms.Label()
        Me.tbContrast = New System.Windows.Forms.TrackBar()
        Me.tbOpacity = New System.Windows.Forms.TrackBar()
        Me.lblOpacity = New System.Windows.Forms.Label()
        CType(Me.tbDepth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbContrast, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbOpacity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbDepth
        '
        Me.tbDepth.Location = New System.Drawing.Point(12, 197)
        Me.tbDepth.Maximum = 2000
        Me.tbDepth.Minimum = -2000
        Me.tbDepth.Name = "tbDepth"
        Me.tbDepth.Size = New System.Drawing.Size(260, 45)
        Me.tbDepth.TabIndex = 0
        Me.tbDepth.TickFrequency = 200
        Me.tbDepth.Value = 500
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Imagen seleccionada"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Directorio a guardar"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Color"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(251, 25)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(21, 20)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(251, 64)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(21, 20)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'lblColor
        '
        Me.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblColor.Location = New System.Drawing.Point(12, 108)
        Me.lblColor.Name = "lblColor"
        Me.lblColor.Size = New System.Drawing.Size(64, 64)
        Me.lblColor.TabIndex = 6
        '
        'txtSelectedFile
        '
        Me.txtSelectedFile.Location = New System.Drawing.Point(12, 25)
        Me.txtSelectedFile.Name = "txtSelectedFile"
        Me.txtSelectedFile.ReadOnly = True
        Me.txtSelectedFile.Size = New System.Drawing.Size(233, 20)
        Me.txtSelectedFile.TabIndex = 7
        '
        'btnTint
        '
        Me.btnTint.Location = New System.Drawing.Point(29, 376)
        Me.btnTint.Name = "btnTint"
        Me.btnTint.Size = New System.Drawing.Size(110, 32)
        Me.btnTint.TabIndex = 8
        Me.btnTint.Text = "¡Tintar!"
        Me.btnTint.UseVisualStyleBackColor = True
        '
        'lblDepth
        '
        Me.lblDepth.AutoSize = True
        Me.lblDepth.Location = New System.Drawing.Point(12, 181)
        Me.lblDepth.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        Me.lblDepth.Name = "lblDepth"
        Me.lblDepth.Size = New System.Drawing.Size(131, 13)
        Me.lblDepth.TabIndex = 9
        Me.lblDepth.Text = "Profundidad del color (0,5)"
        '
        'txtSavePath
        '
        Me.txtSavePath.Location = New System.Drawing.Point(12, 64)
        Me.txtSavePath.Name = "txtSavePath"
        Me.txtSavePath.ReadOnly = True
        Me.txtSavePath.Size = New System.Drawing.Size(233, 20)
        Me.txtSavePath.TabIndex = 10
        '
        'btnColorSelect
        '
        Me.btnColorSelect.Location = New System.Drawing.Point(82, 127)
        Me.btnColorSelect.Name = "btnColorSelect"
        Me.btnColorSelect.Size = New System.Drawing.Size(111, 26)
        Me.btnColorSelect.TabIndex = 11
        Me.btnColorSelect.Text = "Seleccionar color"
        Me.btnColorSelect.UseVisualStyleBackColor = True
        '
        'ofdFileToTint
        '
        Me.ofdFileToTint.FileName = "OpenFileDialog1"
        Me.ofdFileToTint.Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*"
        '
        'sfdFileToSave
        '
        Me.sfdFileToSave.Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*"
        '
        'btnSeeFile
        '
        Me.btnSeeFile.Enabled = False
        Me.btnSeeFile.Location = New System.Drawing.Point(145, 376)
        Me.btnSeeFile.Name = "btnSeeFile"
        Me.btnSeeFile.Size = New System.Drawing.Size(110, 32)
        Me.btnSeeFile.TabIndex = 12
        Me.btnSeeFile.Text = "Ver archivo"
        Me.btnSeeFile.UseVisualStyleBackColor = True
        '
        'lblContrast
        '
        Me.lblContrast.AutoSize = True
        Me.lblContrast.Location = New System.Drawing.Point(12, 245)
        Me.lblContrast.Name = "lblContrast"
        Me.lblContrast.Size = New System.Drawing.Size(67, 13)
        Me.lblContrast.TabIndex = 13
        Me.lblContrast.Text = "Contraste (1)"
        '
        'tbContrast
        '
        Me.tbContrast.Location = New System.Drawing.Point(12, 261)
        Me.tbContrast.Maximum = 5000
        Me.tbContrast.Name = "tbContrast"
        Me.tbContrast.Size = New System.Drawing.Size(260, 45)
        Me.tbContrast.TabIndex = 14
        Me.tbContrast.TickFrequency = 250
        Me.tbContrast.Value = 1000
        '
        'tbOpacity
        '
        Me.tbOpacity.Location = New System.Drawing.Point(12, 325)
        Me.tbOpacity.Maximum = 1000
        Me.tbOpacity.Name = "tbOpacity"
        Me.tbOpacity.Size = New System.Drawing.Size(260, 45)
        Me.tbOpacity.TabIndex = 16
        Me.tbOpacity.TickFrequency = 50
        Me.tbOpacity.Value = 1000
        '
        'lblOpacity
        '
        Me.lblOpacity.AutoSize = True
        Me.lblOpacity.Location = New System.Drawing.Point(12, 309)
        Me.lblOpacity.Name = "lblOpacity"
        Me.lblOpacity.Size = New System.Drawing.Size(68, 13)
        Me.lblOpacity.TabIndex = 15
        Me.lblOpacity.Text = "Opacidad (1)"
        '
        'frmTint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 417)
        Me.Controls.Add(Me.tbOpacity)
        Me.Controls.Add(Me.lblOpacity)
        Me.Controls.Add(Me.tbContrast)
        Me.Controls.Add(Me.lblContrast)
        Me.Controls.Add(Me.btnSeeFile)
        Me.Controls.Add(Me.btnColorSelect)
        Me.Controls.Add(Me.txtSavePath)
        Me.Controls.Add(Me.lblDepth)
        Me.Controls.Add(Me.btnTint)
        Me.Controls.Add(Me.txtSelectedFile)
        Me.Controls.Add(Me.lblColor)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbDepth)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTint"
        Me.Text = "DynaWars - Tintar imagen"
        CType(Me.tbDepth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbContrast, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbOpacity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cdTintColor As System.Windows.Forms.ColorDialog
    Friend WithEvents tbDepth As System.Windows.Forms.TrackBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents lblColor As System.Windows.Forms.Label
    Friend WithEvents txtSelectedFile As System.Windows.Forms.TextBox
    Friend WithEvents btnTint As System.Windows.Forms.Button
    Friend WithEvents lblDepth As System.Windows.Forms.Label
    Friend WithEvents txtSavePath As System.Windows.Forms.TextBox
    Friend WithEvents btnColorSelect As System.Windows.Forms.Button
    Friend WithEvents ofdFileToTint As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdFileToSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnSeeFile As System.Windows.Forms.Button
    Friend WithEvents lblContrast As System.Windows.Forms.Label
    Friend WithEvents tbContrast As System.Windows.Forms.TrackBar
    Friend WithEvents tbOpacity As System.Windows.Forms.TrackBar
    Friend WithEvents lblOpacity As System.Windows.Forms.Label
End Class
