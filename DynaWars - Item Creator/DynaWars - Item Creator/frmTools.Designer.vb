<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTools
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
        Me.btnOrder = New System.Windows.Forms.Button()
        Me.btnBlockGenerator = New System.Windows.Forms.Button()
        Me.btnTint = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnOrder
        '
        Me.btnOrder.Location = New System.Drawing.Point(12, 12)
        Me.btnOrder.Name = "btnOrder"
        Me.btnOrder.Size = New System.Drawing.Size(160, 60)
        Me.btnOrder.TabIndex = 0
        Me.btnOrder.Text = "Ordenar Items"
        Me.btnOrder.UseVisualStyleBackColor = True
        '
        'btnBlockGenerator
        '
        Me.btnBlockGenerator.Location = New System.Drawing.Point(12, 78)
        Me.btnBlockGenerator.Name = "btnBlockGenerator"
        Me.btnBlockGenerator.Size = New System.Drawing.Size(160, 60)
        Me.btnBlockGenerator.TabIndex = 1
        Me.btnBlockGenerator.Text = "Generar miniatura de bloque"
        Me.btnBlockGenerator.UseVisualStyleBackColor = True
        '
        'btnTint
        '
        Me.btnTint.Location = New System.Drawing.Point(12, 144)
        Me.btnTint.Name = "btnTint"
        Me.btnTint.Size = New System.Drawing.Size(160, 60)
        Me.btnTint.TabIndex = 2
        Me.btnTint.Text = "Tintar imagen"
        Me.btnTint.UseVisualStyleBackColor = True
        '
        'frmTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(184, 466)
        Me.Controls.Add(Me.btnTint)
        Me.Controls.Add(Me.btnBlockGenerator)
        Me.Controls.Add(Me.btnOrder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmTools"
        Me.Text = "Herramientas"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOrder As System.Windows.Forms.Button
    Friend WithEvents btnBlockGenerator As System.Windows.Forms.Button
    Friend WithEvents btnTint As System.Windows.Forms.Button
End Class
