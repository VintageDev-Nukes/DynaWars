<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemProfile
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtWObj = New System.Windows.Forms.TextBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtDisplayName = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtItemTex = New System.Windows.Forms.TextBox()
        Me.txtWeight = New System.Windows.Forms.TextBox()
        Me.txtTYP = New System.Windows.Forms.TextBox()
        Me.btnWObj = New System.Windows.Forms.Button()
        Me.btnTex = New System.Windows.Forms.Button()
        Me.chkDropeable = New System.Windows.Forms.CheckBox()
        Me.chkShowStack = New System.Windows.Forms.CheckBox()
        Me.nudMaxItemStack = New System.Windows.Forms.NumericUpDown()
        Me.txtTZP = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtTXP = New System.Windows.Forms.TextBox()
        Me.txtTXR = New System.Windows.Forms.TextBox()
        Me.txtTYR = New System.Windows.Forms.TextBox()
        Me.txtTZR = New System.Windows.Forms.TextBox()
        Me.txtTXS = New System.Windows.Forms.TextBox()
        Me.txtTYS = New System.Windows.Forms.TextBox()
        Me.txtTZS = New System.Windows.Forms.TextBox()
        Me.txtFXS = New System.Windows.Forms.TextBox()
        Me.txtFYS = New System.Windows.Forms.TextBox()
        Me.txtFZS = New System.Windows.Forms.TextBox()
        Me.txtFXR = New System.Windows.Forms.TextBox()
        Me.txtFYR = New System.Windows.Forms.TextBox()
        Me.txtFZR = New System.Windows.Forms.TextBox()
        Me.txtFXP = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtFYP = New System.Windows.Forms.TextBox()
        Me.txtFZP = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnDObj = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.txtDObj = New System.Windows.Forms.TextBox()
        Me.ofdFileSelector = New System.Windows.Forms.OpenFileDialog()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtShopValue = New System.Windows.Forms.TextBox()
        CType(Me.nudMaxItemStack, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Prefab asociado"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(78, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "ID"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Nombre a mostrar"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(52, 61)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Nombre"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(39, 113)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Descripción"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(68, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Icono"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(31, 182)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Peso del Item"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 228)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Tamaño máximo"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 275)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "(3ª persona)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(71, 298)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 13)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Posición"
        '
        'txtWObj
        '
        Me.txtWObj.Location = New System.Drawing.Point(108, 6)
        Me.txtWObj.Name = "txtWObj"
        Me.txtWObj.ReadOnly = True
        Me.txtWObj.Size = New System.Drawing.Size(137, 20)
        Me.txtWObj.TabIndex = 10
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(108, 32)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(36, 20)
        Me.txtID.TabIndex = 11
        Me.txtID.Text = "0"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(108, 58)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(137, 20)
        Me.txtName.TabIndex = 12
        '
        'txtDisplayName
        '
        Me.txtDisplayName.Location = New System.Drawing.Point(108, 84)
        Me.txtDisplayName.Name = "txtDisplayName"
        Me.txtDisplayName.Size = New System.Drawing.Size(137, 20)
        Me.txtDisplayName.TabIndex = 13
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(108, 110)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(137, 37)
        Me.txtDescription.TabIndex = 14
        '
        'txtItemTex
        '
        Me.txtItemTex.Location = New System.Drawing.Point(108, 153)
        Me.txtItemTex.Name = "txtItemTex"
        Me.txtItemTex.ReadOnly = True
        Me.txtItemTex.Size = New System.Drawing.Size(137, 20)
        Me.txtItemTex.TabIndex = 15
        '
        'txtWeight
        '
        Me.txtWeight.Location = New System.Drawing.Point(108, 179)
        Me.txtWeight.Name = "txtWeight"
        Me.txtWeight.Size = New System.Drawing.Size(62, 20)
        Me.txtWeight.TabIndex = 16
        Me.txtWeight.Text = "0"
        '
        'txtTYP
        '
        Me.txtTYP.Location = New System.Drawing.Point(175, 295)
        Me.txtTYP.Name = "txtTYP"
        Me.txtTYP.Size = New System.Drawing.Size(45, 20)
        Me.txtTYP.TabIndex = 18
        Me.txtTYP.Text = "0"
        '
        'btnWObj
        '
        Me.btnWObj.Location = New System.Drawing.Point(251, 6)
        Me.btnWObj.Name = "btnWObj"
        Me.btnWObj.Size = New System.Drawing.Size(21, 20)
        Me.btnWObj.TabIndex = 20
        Me.btnWObj.Text = "..."
        Me.btnWObj.UseVisualStyleBackColor = True
        '
        'btnTex
        '
        Me.btnTex.Location = New System.Drawing.Point(251, 153)
        Me.btnTex.Name = "btnTex"
        Me.btnTex.Size = New System.Drawing.Size(21, 20)
        Me.btnTex.TabIndex = 21
        Me.btnTex.Text = "..."
        Me.btnTex.UseVisualStyleBackColor = True
        '
        'chkDropeable
        '
        Me.chkDropeable.AutoSize = True
        Me.chkDropeable.Checked = True
        Me.chkDropeable.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDropeable.Location = New System.Drawing.Point(108, 205)
        Me.chkDropeable.Name = "chkDropeable"
        Me.chkDropeable.Size = New System.Drawing.Size(87, 17)
        Me.chkDropeable.TabIndex = 23
        Me.chkDropeable.Text = "¿Dropeable?"
        Me.chkDropeable.UseVisualStyleBackColor = True
        '
        'chkShowStack
        '
        Me.chkShowStack.AutoSize = True
        Me.chkShowStack.Checked = True
        Me.chkShowStack.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowStack.Location = New System.Drawing.Point(108, 252)
        Me.chkShowStack.Name = "chkShowStack"
        Me.chkShowStack.Size = New System.Drawing.Size(111, 17)
        Me.chkShowStack.TabIndex = 24
        Me.chkShowStack.Text = "¿Mostrar tamaño?"
        Me.chkShowStack.UseVisualStyleBackColor = True
        '
        'nudMaxItemStack
        '
        Me.nudMaxItemStack.Location = New System.Drawing.Point(108, 226)
        Me.nudMaxItemStack.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxItemStack.Name = "nudMaxItemStack"
        Me.nudMaxItemStack.Size = New System.Drawing.Size(62, 20)
        Me.nudMaxItemStack.TabIndex = 25
        Me.nudMaxItemStack.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtTZP
        '
        Me.txtTZP.Location = New System.Drawing.Point(226, 295)
        Me.txtTZP.Name = "txtTZP"
        Me.txtTZP.Size = New System.Drawing.Size(45, 20)
        Me.txtTZP.TabIndex = 17
        Me.txtTZP.Text = "0"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(68, 324)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(50, 13)
        Me.Label11.TabIndex = 26
        Me.Label11.Text = "Rotación"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(79, 350)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(39, 13)
        Me.Label12.TabIndex = 27
        Me.Label12.Text = "Escala"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 376)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(64, 13)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "(1ª persona)"
        '
        'txtTXP
        '
        Me.txtTXP.Location = New System.Drawing.Point(124, 295)
        Me.txtTXP.Name = "txtTXP"
        Me.txtTXP.Size = New System.Drawing.Size(45, 20)
        Me.txtTXP.TabIndex = 29
        Me.txtTXP.Text = "0"
        '
        'txtTXR
        '
        Me.txtTXR.Location = New System.Drawing.Point(124, 321)
        Me.txtTXR.Name = "txtTXR"
        Me.txtTXR.Size = New System.Drawing.Size(45, 20)
        Me.txtTXR.TabIndex = 32
        Me.txtTXR.Text = "0"
        '
        'txtTYR
        '
        Me.txtTYR.Location = New System.Drawing.Point(175, 321)
        Me.txtTYR.Name = "txtTYR"
        Me.txtTYR.Size = New System.Drawing.Size(45, 20)
        Me.txtTYR.TabIndex = 31
        Me.txtTYR.Text = "0"
        '
        'txtTZR
        '
        Me.txtTZR.Location = New System.Drawing.Point(226, 321)
        Me.txtTZR.Name = "txtTZR"
        Me.txtTZR.Size = New System.Drawing.Size(45, 20)
        Me.txtTZR.TabIndex = 30
        Me.txtTZR.Text = "0"
        '
        'txtTXS
        '
        Me.txtTXS.Location = New System.Drawing.Point(124, 347)
        Me.txtTXS.Name = "txtTXS"
        Me.txtTXS.Size = New System.Drawing.Size(45, 20)
        Me.txtTXS.TabIndex = 35
        Me.txtTXS.Text = "0"
        '
        'txtTYS
        '
        Me.txtTYS.Location = New System.Drawing.Point(175, 347)
        Me.txtTYS.Name = "txtTYS"
        Me.txtTYS.Size = New System.Drawing.Size(45, 20)
        Me.txtTYS.TabIndex = 34
        Me.txtTYS.Text = "0"
        '
        'txtTZS
        '
        Me.txtTZS.Location = New System.Drawing.Point(226, 347)
        Me.txtTZS.Name = "txtTZS"
        Me.txtTZS.Size = New System.Drawing.Size(45, 20)
        Me.txtTZS.TabIndex = 33
        Me.txtTZS.Text = "0"
        '
        'txtFXS
        '
        Me.txtFXS.Location = New System.Drawing.Point(124, 450)
        Me.txtFXS.Name = "txtFXS"
        Me.txtFXS.Size = New System.Drawing.Size(45, 20)
        Me.txtFXS.TabIndex = 47
        Me.txtFXS.Text = "0"
        '
        'txtFYS
        '
        Me.txtFYS.Location = New System.Drawing.Point(175, 450)
        Me.txtFYS.Name = "txtFYS"
        Me.txtFYS.Size = New System.Drawing.Size(45, 20)
        Me.txtFYS.TabIndex = 46
        Me.txtFYS.Text = "0"
        '
        'txtFZS
        '
        Me.txtFZS.Location = New System.Drawing.Point(226, 450)
        Me.txtFZS.Name = "txtFZS"
        Me.txtFZS.Size = New System.Drawing.Size(45, 20)
        Me.txtFZS.TabIndex = 45
        Me.txtFZS.Text = "0"
        '
        'txtFXR
        '
        Me.txtFXR.Location = New System.Drawing.Point(124, 424)
        Me.txtFXR.Name = "txtFXR"
        Me.txtFXR.Size = New System.Drawing.Size(45, 20)
        Me.txtFXR.TabIndex = 44
        Me.txtFXR.Text = "0"
        '
        'txtFYR
        '
        Me.txtFYR.Location = New System.Drawing.Point(175, 424)
        Me.txtFYR.Name = "txtFYR"
        Me.txtFYR.Size = New System.Drawing.Size(45, 20)
        Me.txtFYR.TabIndex = 43
        Me.txtFYR.Text = "0"
        '
        'txtFZR
        '
        Me.txtFZR.Location = New System.Drawing.Point(226, 424)
        Me.txtFZR.Name = "txtFZR"
        Me.txtFZR.Size = New System.Drawing.Size(45, 20)
        Me.txtFZR.TabIndex = 42
        Me.txtFZR.Text = "0"
        '
        'txtFXP
        '
        Me.txtFXP.Location = New System.Drawing.Point(124, 398)
        Me.txtFXP.Name = "txtFXP"
        Me.txtFXP.Size = New System.Drawing.Size(45, 20)
        Me.txtFXP.TabIndex = 41
        Me.txtFXP.Text = "0"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(79, 453)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(39, 13)
        Me.Label14.TabIndex = 40
        Me.Label14.Text = "Escala"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(68, 427)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(50, 13)
        Me.Label15.TabIndex = 39
        Me.Label15.Text = "Rotación"
        '
        'txtFYP
        '
        Me.txtFYP.Location = New System.Drawing.Point(175, 398)
        Me.txtFYP.Name = "txtFYP"
        Me.txtFYP.Size = New System.Drawing.Size(45, 20)
        Me.txtFYP.TabIndex = 38
        Me.txtFYP.Text = "0"
        '
        'txtFZP
        '
        Me.txtFZP.Location = New System.Drawing.Point(226, 398)
        Me.txtFZP.Name = "txtFZP"
        Me.txtFZP.Size = New System.Drawing.Size(45, 20)
        Me.txtFZP.TabIndex = 37
        Me.txtFZP.Text = "0"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(71, 401)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(47, 13)
        Me.Label16.TabIndex = 36
        Me.Label16.Text = "Posición"
        '
        'btnDObj
        '
        Me.btnDObj.Location = New System.Drawing.Point(251, 476)
        Me.btnDObj.Name = "btnDObj"
        Me.btnDObj.Size = New System.Drawing.Size(21, 20)
        Me.btnDObj.TabIndex = 48
        Me.btnDObj.Text = "..."
        Me.btnDObj.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(16, 479)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(86, 13)
        Me.Label17.TabIndex = 49
        Me.Label17.Text = "Objeto dropeado"
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(69, 528)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(150, 40)
        Me.btnGo.TabIndex = 50
        Me.btnGo.Text = "¡Crear nuevo Item!"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'txtDObj
        '
        Me.txtDObj.Location = New System.Drawing.Point(108, 476)
        Me.txtDObj.Name = "txtDObj"
        Me.txtDObj.ReadOnly = True
        Me.txtDObj.Size = New System.Drawing.Size(137, 20)
        Me.txtDObj.TabIndex = 51
        '
        'ofdFileSelector
        '
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(65, 505)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(37, 13)
        Me.Label18.TabIndex = 52
        Me.Label18.Text = "Precio"
        '
        'txtShopValue
        '
        Me.txtShopValue.Location = New System.Drawing.Point(108, 502)
        Me.txtShopValue.Name = "txtShopValue"
        Me.txtShopValue.Size = New System.Drawing.Size(62, 20)
        Me.txtShopValue.TabIndex = 53
        Me.txtShopValue.Text = "0"
        '
        'frmItemProfile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 580)
        Me.Controls.Add(Me.txtShopValue)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtDObj)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.btnDObj)
        Me.Controls.Add(Me.txtFXS)
        Me.Controls.Add(Me.txtFYS)
        Me.Controls.Add(Me.txtFZS)
        Me.Controls.Add(Me.txtFXR)
        Me.Controls.Add(Me.txtFYR)
        Me.Controls.Add(Me.txtFZR)
        Me.Controls.Add(Me.txtFXP)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.txtFYP)
        Me.Controls.Add(Me.txtFZP)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtTXS)
        Me.Controls.Add(Me.txtTYS)
        Me.Controls.Add(Me.txtTZS)
        Me.Controls.Add(Me.txtTXR)
        Me.Controls.Add(Me.txtTYR)
        Me.Controls.Add(Me.txtTZR)
        Me.Controls.Add(Me.txtTXP)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.nudMaxItemStack)
        Me.Controls.Add(Me.chkShowStack)
        Me.Controls.Add(Me.chkDropeable)
        Me.Controls.Add(Me.btnTex)
        Me.Controls.Add(Me.btnWObj)
        Me.Controls.Add(Me.txtTYP)
        Me.Controls.Add(Me.txtTZP)
        Me.Controls.Add(Me.txtWeight)
        Me.Controls.Add(Me.txtItemTex)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtDisplayName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.txtWObj)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemProfile"
        Me.Text = "DynaWars - Perfil de Item"
        CType(Me.nudMaxItemStack, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtWObj As System.Windows.Forms.TextBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents txtItemTex As System.Windows.Forms.TextBox
    Friend WithEvents txtWeight As System.Windows.Forms.TextBox
    Friend WithEvents txtTYP As System.Windows.Forms.TextBox
    Friend WithEvents btnWObj As System.Windows.Forms.Button
    Friend WithEvents btnTex As System.Windows.Forms.Button
    Friend WithEvents chkDropeable As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowStack As System.Windows.Forms.CheckBox
    Friend WithEvents nudMaxItemStack As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtTZP As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtTXP As System.Windows.Forms.TextBox
    Friend WithEvents txtTXR As System.Windows.Forms.TextBox
    Friend WithEvents txtTYR As System.Windows.Forms.TextBox
    Friend WithEvents txtTZR As System.Windows.Forms.TextBox
    Friend WithEvents txtTXS As System.Windows.Forms.TextBox
    Friend WithEvents txtTYS As System.Windows.Forms.TextBox
    Friend WithEvents txtTZS As System.Windows.Forms.TextBox
    Friend WithEvents txtFXS As System.Windows.Forms.TextBox
    Friend WithEvents txtFYS As System.Windows.Forms.TextBox
    Friend WithEvents txtFZS As System.Windows.Forms.TextBox
    Friend WithEvents txtFXR As System.Windows.Forms.TextBox
    Friend WithEvents txtFYR As System.Windows.Forms.TextBox
    Friend WithEvents txtFZR As System.Windows.Forms.TextBox
    Friend WithEvents txtFXP As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtFYP As System.Windows.Forms.TextBox
    Friend WithEvents txtFZP As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnDObj As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents txtDObj As System.Windows.Forms.TextBox
    Friend WithEvents ofdFileSelector As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtShopValue As System.Windows.Forms.TextBox
End Class
