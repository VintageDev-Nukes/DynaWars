Imports System.IO
Imports System.Globalization
Public Class frmMain

    Dim tools As frmTools
    Dim WithEvents frmProfiler As New frmItemProfile
    Dim WithEvents panel As Panel
    Dim itemGroup As GroupBox
    Dim WithEvents itemPic As PictureBox
    Dim WithEvents itemButtonEdit As Button
    Dim WithEvents itemButtonDelete As Button
    Shadows WithEvents scroll As VScrollBar
    Const ScrollChange As Integer = -5

    Dim WithEvents mApp As mainApp

    Dim needClose, safeClosing, creatingNew, saved As Boolean


    Private Shadows Sub Load() Handles MyBase.Load
        mApp = New mainApp
        tools = New frmTools
        tools.mainForm = Me
        tools.Show()
        ToolsPosition()
    End Sub

    Public Sub VirtualizeItems()
        If mainApp.loadedItemList Is Nothing Or mainApp.loadedItemList.myBase Is Nothing Then
            Exit Sub
        End If
        DisposeMainGUI()
        Dim num = mainApp.loadedItemList.myBase.Count, scrollValue As Integer
        If scroll IsNot Nothing Then
            scrollValue = scroll.Value
        End If
        Dim gamePath As String = mainApp.gamePath
        scroll = New VScrollBar() With {.Top = 33, .Left = 552, .Width = 23, .Height = 420, .Maximum = If(num >= 12, 103 * (Math.Ceiling((num + 1) / 3) - 4) + 24, 1), .Visible = num >= 12, .Value = scrollValue, .Dock = DockStyle.Right}
        Me.Controls.Add(scroll)
        AddHandler scroll.ValueChanged, AddressOf ChangeScroll
        panel = New Panel() With {.Top = If(scrollValue = 0, 24, -scrollValue), .Left = 12, .Width = If(num >= 12, 537, 560), .Height = If(num >= 12, 424 + (106 * (Math.Ceiling((num + 1) / 3) - 4)) + 9, 460)}
        Me.Controls.Add(panel)
        AddHandler panel.MouseWheel, AddressOf Panel_MouseWheel
        For i As Integer = 0 To num
            If i < num Then
                itemGroup = New GroupBox() With {.Name = "gbItem" & i, .Text = "ID " & mainApp.loadedItemList.myBase(i).id & " - " & mainApp.loadedItemList.myBase(i).DisplayName, .Width = 170, .Height = 100, .Top = (12 + 100 * Math.Round((i - 1) / 3) + 6 * Math.Round((i - 1) / 3)), .Left = (If(num >= 12, 3, 19) + 170 * (i Mod 3) + 6 * (i Mod 3))}
                panel.Controls.Add(itemGroup)
                itemButtonEdit = New Button() With {.Name = i, .Top = 69, .Left = 10, .Height = 25, .Width = 70, .Text = "Editar Item"}
                itemGroup.Controls.Add(itemButtonEdit)
                AddHandler itemButtonEdit.Click, AddressOf EditItem
                itemButtonDelete = New Button() With {.Name = i, .Top = 69, .Left = 90, .Height = 25, .Width = 70, .Text = "Borrar Item"}
                itemGroup.Controls.Add(itemButtonDelete)
                AddHandler itemButtonDelete.Click, AddressOf DeleteItem
                itemPic = New PictureBox() With {.ImageLocation = Path.Combine(New String() {gamePath, mainApp.loadedItemList.myBase(i).itemtex.Replace("/", "\")}), .Width = 32, .Height = 32, .Top = 25, .Left = 69, .SizeMode = PictureBoxSizeMode.StretchImage}
                itemGroup.Controls.Add(itemPic)
            Else
                itemGroup = New GroupBox() With {.Name = "gbItem" & i, .Width = 170, .Height = 100, .Top = (12 + 100 * Math.Round((i - 1) / 3) + 6 * Math.Round((i - 1) / 3)), .Left = (If(num >= 12, 3, 19) + 170 * (i Mod 3) + 6 * (i Mod 3))}
                panel.Controls.Add(itemGroup)
                Dim newItemButton As Button = New Button() With {.Width = 80, .Height = 40, .Top = 30, .Left = 45, .Text = "Nuevo Item"}
                itemGroup.Controls.Add(newItemButton)
                AddHandler newItemButton.Click, AddressOf NewItem
            End If
        Next
        panel.SendToBack()
    End Sub

    Private Sub CreateAloneButton()
        DisposeMainGUI()
        Dim gamePath As String = mainApp.gamePath
        panel = New Panel() With {.Top = 20, .Left = 20, .Width = 560, .Height = 460}
        Me.Controls.Add(panel)
        AddHandler panel.MouseWheel, AddressOf Panel_MouseWheel
        itemGroup = New GroupBox() With {.Name = "gbNewItem", .Width = 170, .Height = 100, .Top = 0, .Left = 0}
        panel.Controls.Add(itemGroup)
        Dim newItemButton As Button = New Button() With {.Width = 80, .Height = 40, .Top = 30, .Left = 45, .Text = "Nuevo Item"}
        itemGroup.Controls.Add(newItemButton)
        AddHandler newItemButton.Click, AddressOf NewItem
    End Sub

    Private Sub ChangeScroll()
        panel.Top = -scroll.Value + 24
    End Sub

    Private Sub Panel_MouseWheel(ByVal sender As Object, ByVal e As MouseEventArgs)
        Try
            Select Case Math.Sign(e.Delta)
                Case Is > 0 : scroll.Value += ScrollChange
                Case Is < 0 : scroll.Value -= ScrollChange
            End Select
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadItems()
        mainApp.loadedItemList = New ItemBase
        mainApp.loadedItemList = XMLTools.DeserializeFromFile(Of ItemBase)(ofdInventory.FileName)
    End Sub

    Private Sub EditItem(sender As Object, e As EventArgs)
        Dim myButton As Button = DirectCast(sender, Button)
        Dim index As Integer = Convert.ToInt32(myButton.Name)
        frmProfiler = New frmItemProfile 'Reset everything
        frmProfiler.selectedItem = index
        frmProfiler.ShowDialog()
    End Sub

    Private Sub DeleteItem(sender As Object, e As EventArgs)
        If MsgBox("¿Estás seguro de borrar el item seleccionado?", MsgBoxStyle.YesNo, "Atención") = MsgBoxResult.Yes Then
            Dim myButton As Button = DirectCast(sender, Button)
            Dim index As Integer = Convert.ToInt32(myButton.Name)
            mainApp.loadedItemList.myBase.RemoveAt(index)
            mainApp.iChanged(Nothing)
        End If
    End Sub

    Private Sub SortedItems() Handles mApp.itemsChanged
        VirtualizeItems()
    End Sub

    Private Sub NewItem()
        frmProfiler = New frmItemProfile 'Reset everything
        frmProfiler.ShowDialog()
    End Sub

    Private Sub CreateNew()
        mainApp.loadedItemList = New ItemBase() With {.myBase = New List(Of Item)}
        XMLTools.SerializeToFile(Of ItemBase)(mainApp.loadedItemList, svdInventory.FileName)
        CreateAloneButton()
    End Sub

    Private Sub ProfilerUsed() Handles frmProfiler.itemProfileUsed
        mApp.SortItems()
    End Sub

    Private Sub Moving() Handles Me.Move
        If tools IsNot Nothing Then
            ToolsPosition()
        End If
    End Sub

    Private Sub ToolsPosition()
        tools.Location = New Point(Me.Location.X - tools.Width - 5, Me.Location.Y)
    End Sub

    Private Sub DisposeMainGUI()
        If scroll IsNot Nothing Then
            scroll.Dispose()
        End If
        If panel IsNot Nothing Then
            panel.Dispose()
        End If
    End Sub

    Private Sub SafeExit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
        If Not saved AndAlso Not String.IsNullOrEmpty(ofdInventory.FileName) Then
            Dim alert As MsgBoxResult = MsgBox("No has guardado los cambios. ¿Deseas hacerlo?", MsgBoxStyle.YesNoCancel, "¡Atención!")
            Select Case alert
                Case MsgBoxResult.Yes
                    needClose = True
                    svdInventory.ShowDialog()
                Case MsgBoxResult.No
                    safeClosing = True
                    ExitApp()
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        Else
            safeClosing = True
            ExitApp()
        End If
    End Sub

    Private Sub ExitApp()
        If System.Diagnostics.Process.GetCurrentProcess().Threads.Count > 0 Then
            For Each t As System.Diagnostics.ProcessThread In System.Diagnostics.Process.GetCurrentProcess().Threads
                t.Dispose()
            Next t
        End If
        Application.Exit()
    End Sub

    Private Sub SaveItems(ByVal path As String)
        XMLTools.SerializeToFile(Of ItemBase)(mainApp.loadedItemList, path)
        saved = True
    End Sub

    Private Sub ClearSolution()
        If Not saved AndAlso Not String.IsNullOrEmpty(ofdInventory.FileName) Then
            Select Case MsgBox("Si continuas, todo la edición se perderá. ¿Estás seguro?", MsgBoxStyle.YesNo, "Advertencia")
                Case MsgBoxResult.Yes
                    DisposeMainGUI()
                    mainApp.loadedItemList = Nothing
            End Select
        End If
        svdInventory.FileName = ""
    End Sub

    Private Sub SetGamePath()
        fbdSetGamepath.ShowDialog()
        mainApp.gamePath = fbdSetGamepath.SelectedPath
    End Sub

    Private Sub ofdInventory_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ofdInventory.FileOk
        mainApp.gamePath = New DirectoryInfo(ofdInventory.FileName).Parent.Parent.FullName
        LoadItems()
    End Sub

    Private Sub AbrirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirToolStripMenuItem.Click
        ofdInventory.ShowDialog()
    End Sub

    Private Sub NuevoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevoToolStripMenuItem.Click
        creatingNew = True
        svdInventory.ShowDialog()
    End Sub

    Private Sub GuardarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GuardarToolStripMenuItem.Click
        If String.IsNullOrEmpty(ofdInventory.FileName) Then
            svdInventory.ShowDialog()
        Else
            SaveItems(ofdInventory.FileName)
        End If
    End Sub

    Private Sub svdInventory_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles svdInventory.FileOk
        If creatingNew Then
            CreateNew()
            creatingNew = False
        Else
            SaveItems(svdInventory.FileName)
            If needClose Then
                ExitApp()
            End If
        End If
    End Sub

    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        SafeExit(sender, e)
    End Sub

    Private Shadows Sub Closing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Not safeClosing Then
            SafeExit(sender, e)
        End If
    End Sub

    Private Sub EstablecerRutaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EstablecerRutaToolStripMenuItem.Click
        SetGamepath()
    End Sub

    Private Sub LimpiarEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LimpiarEditorToolStripMenuItem.Click
        ClearSolution()
    End Sub

    Private Sub MostrarHerramientasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MostrarHerramientasToolStripMenuItem.Click
        If tools IsNot Nothing AndAlso Not tools.Visible Then
            tools = New frmTools
            tools.Show()
        End If
    End Sub

End Class
