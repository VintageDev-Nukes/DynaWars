Public Enum FileToSelect
    WorldObject
    DropObject
    Texture
End Enum

Public Class frmItemProfile

    Public Event itemProfileUsed As EventHandler

    Dim WithEvents mApp As mainApp
    Dim selectionType As FileToSelect

    Dim texSelected As Boolean

    Public selectedItem As Integer = -1

    Private Shadows Sub Load() Handles MyBase.Load
        mApp = New mainApp
    End Sub

    Private Shadows Sub Shown() Handles MyBase.Shown
        If selectedItem = -1 Then
            btnGo.Text = "¡Crear nuevo Item!"
        Else
            btnGo.Text = "¡Editar Item!"
            LoadValuesFromItem(selectedItem)
        End If
    End Sub

    Private Sub LoadValuesFromItem(ByVal i As Integer)
        Dim item As Item = mainApp.loadedItemList.myBase(i)
        txtDescription.Text = item.Description
        txtDisplayName.Text = item.DisplayName
        txtDObj.Text = item.dropObject
        txtFXP.Text = item.FcustomPostion.x
        txtFXR.Text = item.FcustomRotation.x
        txtFXS.Text = item.FcustomScale.x
        txtFYP.Text = item.FcustomPostion.y
        txtFYR.Text = item.FcustomRotation.y
        txtFYS.Text = item.FcustomScale.y
        txtFZP.Text = item.FcustomPostion.z
        txtFZR.Text = item.FcustomRotation.z
        txtFZS.Text = item.FcustomScale.z
        txtID.Text = item.id
        txtItemTex.Text = item.itemtex
        txtName.Text = item.itemname
        txtTXP.Text = item.TcustomPostion.x
        txtTXR.Text = item.TcustomRotation.x
        txtTXS.Text = item.TcustomScale.x
        txtTYP.Text = item.TcustomPostion.y
        txtTYR.Text = item.TcustomRotation.y
        txtTYS.Text = item.TcustomScale.y
        txtTZP.Text = item.FcustomPostion.z
        txtTZR.Text = item.TcustomRotation.z
        txtTZS.Text = item.TcustomScale.z
        txtWeight.Text = item.weight
        txtWObj.Text = item.worldObject
        txtshopValue.Text = item.shopValue
        chkDropeable.Checked = item.droppable
        chkShowStack.Checked = item.showStack
        nudMaxItemStack.Value = item.itemstacklimit
    End Sub

    Private Function SaveItem() As Item
        Dim FXP, FYP, FZP, FXR, FYR, FZR, FXS, FYS, FZS, TXP, TYP, TZP, TXR, TYR, TZR, TXS, TYS, TZS, weight As Single
        Dim item As Item = New Item
        item.Description = txtDescription.Text
        item.DisplayName = txtDisplayName.Text
        item.dropObject = txtDObj.Text
        Single.TryParse(txtFXP.Text, FXP)
        Single.TryParse(txtFXR.Text, FXR)
        Single.TryParse(txtFXS.Text, FXS)
        Single.TryParse(txtFYP.Text, FYP)
        Single.TryParse(txtFYR.Text, FYR)
        Single.TryParse(txtFYS.Text, FYS)
        Single.TryParse(txtFZP.Text, FZP)
        Single.TryParse(txtFZR.Text, FZR)
        Single.TryParse(txtFZS.Text, FZS)
        Single.TryParse(txtTXP.Text, TXP)
        Single.TryParse(txtTXR.Text, TXR)
        Single.TryParse(txtTXS.Text, TXS)
        Single.TryParse(txtTYP.Text, TYP)
        Single.TryParse(txtTYR.Text, TYR)
        Single.TryParse(txtTYS.Text, TYS)
        Single.TryParse(txtTZP.Text, TZP)
        Single.TryParse(txtTZR.Text, TZR)
        Single.TryParse(txtTZS.Text, TZS)
        Single.TryParse(txtWeight.Text, weight)
        item.FcustomPostion = New Vector3(FXP, FYP, FZP)
        item.FcustomRotation = New Vector3(FXR, FYR, FZR)
        item.FcustomScale = New Vector3(FXS, FYS, FZS)
        item.TcustomPostion = New Vector3(TXP, TYP, TZP)
        item.TcustomRotation = New Vector3(TXR, TYR, TZR)
        item.TcustomScale = New Vector3(TXS, TYS, TZS)
        item.weight = weight
        item.id = Convert.ToInt32(txtID.Text)
        item.itemtex = txtItemTex.Text
        item.itemname = txtName.Text
        item.worldObject = txtWObj.Text
        item.droppable = chkDropeable.Checked
        item.showStack = chkShowStack.Checked
        item.shopValue = Convert.ToInt32(txtShopValue.Text)
        item.itemstacklimit = nudMaxItemStack.Value
        Return item
    End Function

    Public Sub NumericTextbox(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> ","c Then
            e.Handled = True
        End If

        ' only allow one decimal point
        'If (e.KeyChar = "."c AndAlso TryCast(sender, TextBox).Text.IndexOf("."c) > -1) Or (e.KeyChar = ","c AndAlso TryCast(sender, TextBox).Text.IndexOf(","c) > -1) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub btnWObj_Click(sender As Object, e As EventArgs) Handles btnWObj.Click
        selectionType = FileToSelect.WorldObject
        ofdFileSelector.Filter = "Prefab Files (*.prefab)|*.prefab|All Files (*.*)|*.*"
        ofdFileSelector.ShowDialog()
    End Sub

    Private Sub btnTex_Click(sender As Object, e As EventArgs) Handles btnTex.Click
        selectionType = FileToSelect.Texture
        ofdFileSelector.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp|All Files (*.*)|*.*"
        If Not String.IsNullOrEmpty(txtName.Text) AndAlso Not texSelected Then
            ofdFileSelector.FileName += txtName.Text
        End If
        ofdFileSelector.ShowDialog()
    End Sub

    Private Sub btnDObj_Click(sender As Object, e As EventArgs) Handles btnDObj.Click
        selectionType = FileToSelect.DropObject
        ofdFileSelector.Filter = "Prefab Files (*.prefab)|*.prefab|All Files (*.*)|*.*"
        ofdFileSelector.ShowDialog()
    End Sub

    Private Sub ofdFileSelector_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ofdFileSelector.FileOk
        Select Case selectionType
            Case FileToSelect.DropObject
                txtDObj.Text = ofdFileSelector.FileName.Replace(mainApp.gamePath & "\", "").Replace("\", "/")
            Case FileToSelect.Texture
                txtItemTex.Text = ofdFileSelector.FileName.Replace(mainApp.gamePath & "\", "").Replace("\", "/")
                texSelected = True
            Case FileToSelect.WorldObject
                txtWObj.Text = ofdFileSelector.FileName.Replace(mainApp.gamePath & "\", "").Replace("\", "/")
        End Select
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        If selectedItem = -1 Then 'Create
            mainApp.loadedItemList.myBase.Add(SaveItem())
        Else 'Save
            mainApp.loadedItemList.myBase(selectedItem) = SaveItem()
        End If
        RaiseEvent itemProfileUsed(Me, Nothing)
        Me.Close()
    End Sub

    Private Sub txtFZS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFZS.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFYS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFYS.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFXS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFXS.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFZR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFZR.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFYR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFYR.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFXR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFXR.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFZP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFZP.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFYP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFYP.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtFXP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFXP.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTZS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTZS.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTYS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTYS.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTXS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTXS.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTZR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTZR.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTYR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTYR.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTXR_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTXR.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTZP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTZP.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTYP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTYP.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtTXP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTXP.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtWeight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtWeight.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtID.KeyPress
        NumericTextbox(sender, e)
    End Sub

    Private Sub txtshopValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtshopValue.KeyPress
        NumericTextbox(sender, e)
    End Sub

End Class