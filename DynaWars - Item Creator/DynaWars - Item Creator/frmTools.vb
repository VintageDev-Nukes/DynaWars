Public Class frmTools

    Dim WithEvents mApp As mainApp

    Public WithEvents mainForm As Form

    Private Shadows Sub Load() Handles MyBase.Load
        mApp = New mainApp
        CheckItems()
    End Sub

    Private Sub CheckItems() Handles mApp.itemsChanged
        btnOrder.Enabled = mainApp.loadedItemList IsNot Nothing AndAlso mainApp.loadedItemList.myBase IsNot Nothing AndAlso mainApp.loadedItemList.myBase.Count > 0
    End Sub

    Private Sub Moving() Handles Me.Move
        If mainForm IsNot Nothing Then
            MainFormPosition()
        End If
    End Sub

    Private Sub MainFormPosition()
        mainForm.Location = New Point(Me.Location.X + Me.Width + 5, Me.Location.Y)
    End Sub

    'Private Sub Hiding() Handles Me.LostFocus
    '    If Me.WindowState = FormWindowState.Minimized Then
    '        Me.Show()
    '    End If
    'End Sub

    Private Sub btnOrder_Click(sender As Object, e As EventArgs) Handles btnOrder.Click
        mApp.SortItems()
    End Sub

    Private Sub btnBlockGenerator_Click(sender As Object, e As EventArgs) Handles btnBlockGenerator.Click
        Dim frmGen As New frmBlockGenerator
        frmGen.ShowDialog()
    End Sub

    Private Sub btnTint_Click(sender As Object, e As EventArgs) Handles btnTint.Click
        Dim frmTinte As New frmTint
        frmTinte.ShowDialog()
    End Sub

End Class