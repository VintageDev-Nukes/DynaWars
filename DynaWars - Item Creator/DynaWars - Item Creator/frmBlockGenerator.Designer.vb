<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBlockGenerator
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFT = New System.Windows.Forms.TextBox()
        Me.pbResult = New System.Windows.Forms.PictureBox()
        Me.txtFL = New System.Windows.Forms.TextBox()
        Me.txtFR = New System.Windows.Forms.TextBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.sfdBlock = New System.Windows.Forms.SaveFileDialog()
        Me.btnSeeFile = New System.Windows.Forms.Button()
        CType(Me.pbResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(133, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Textura de la cara superior"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Textura de la cara izquierda"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(135, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Textura de la cara derecha"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(111, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Resultado"
        '
        'txtFT
        '
        Me.txtFT.Location = New System.Drawing.Point(156, 12)
        Me.txtFT.Name = "txtFT"
        Me.txtFT.Size = New System.Drawing.Size(116, 20)
        Me.txtFT.TabIndex = 4
        '
        'pbResult
        '
        Me.pbResult.Location = New System.Drawing.Point(12, 103)
        Me.pbResult.Name = "pbResult"
        Me.pbResult.Size = New System.Drawing.Size(260, 108)
        Me.pbResult.TabIndex = 5
        Me.pbResult.TabStop = False
        '
        'txtFL
        '
        Me.txtFL.Location = New System.Drawing.Point(156, 38)
        Me.txtFL.Name = "txtFL"
        Me.txtFL.Size = New System.Drawing.Size(116, 20)
        Me.txtFL.TabIndex = 6
        '
        'txtFR
        '
        Me.txtFR.Location = New System.Drawing.Point(156, 64)
        Me.txtFR.Name = "txtFR"
        Me.txtFR.Size = New System.Drawing.Size(116, 20)
        Me.txtFR.TabIndex = 7
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(29, 217)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(110, 33)
        Me.btnGenerate.TabIndex = 8
        Me.btnGenerate.Text = "¡Generar!"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'sfdBlock
        '
        '
        'btnSeeFile
        '
        Me.btnSeeFile.Enabled = False
        Me.btnSeeFile.Location = New System.Drawing.Point(145, 217)
        Me.btnSeeFile.Name = "btnSeeFile"
        Me.btnSeeFile.Size = New System.Drawing.Size(110, 33)
        Me.btnSeeFile.TabIndex = 9
        Me.btnSeeFile.Text = "Ver archivo"
        Me.btnSeeFile.UseVisualStyleBackColor = True
        '
        'frmBlockGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.btnSeeFile)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.txtFR)
        Me.Controls.Add(Me.txtFL)
        Me.Controls.Add(Me.pbResult)
        Me.Controls.Add(Me.txtFT)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBlockGenerator"
        Me.Text = "DynaWars - Generador de bloques"
        CType(Me.pbResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFT As System.Windows.Forms.TextBox
    Friend WithEvents pbResult As System.Windows.Forms.PictureBox
    Friend WithEvents txtFL As System.Windows.Forms.TextBox
    Friend WithEvents txtFR As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents sfdBlock As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnSeeFile As System.Windows.Forms.Button
End Class
