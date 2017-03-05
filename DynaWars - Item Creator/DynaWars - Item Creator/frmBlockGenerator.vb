Imports System.Threading
Public Class frmBlockGenerator

    Dim thGenerate As Thread

    Private Sub GenerateBlock()
        Dim url As String = "http://gimmeahit.x10host.com/3D/?ft=" & txtFT.Text & "&fl=" & txtFL.Text & "&fr=" & txtFR.Text
        Dim startInfo As New ProcessStartInfo(My.Application.Info.DirectoryPath & "\phantomjs.exe")
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.Arguments = "screenshot.js " & url & " 3DBlock.png"
        Dim p As Process = Process.Start(startInfo)
        p.WaitForExit()
        Dim newImage As Image = Image.FromFile(My.Application.Info.DirectoryPath & "\3DBlock.png")
        If IO.File.Exists(sfdBlock.FileName) Then
            IO.File.Delete(sfdBlock.FileName)
        End If
        newImage.Save(sfdBlock.FileName)
        newImage.Dispose()
        Dim blockImage As Image = Image.FromFile(sfdBlock.FileName)
        If pbResult.InvokeRequired Then
            pbResult.Invoke(Sub() pbResult.Width = blockImage.Width * pbResult.Height / blockImage.Height)
            pbResult.Invoke(Sub() pbResult.Left = Me.Width / 2 - pbResult.Width / 2)
            pbResult.Invoke(Sub() pbResult.SizeMode = PictureBoxSizeMode.StretchImage)
            pbResult.Invoke(Sub() pbResult.Image = blockImage)
        Else
            pbResult.Width = blockImage.Width * pbResult.Height / blockImage.Height
            pbResult.Left = Me.Width / 2 - pbResult.Width / 2
            pbResult.SizeMode = PictureBoxSizeMode.StretchImage
            pbResult.Image = blockImage
        End If
        If btnSeeFile.InvokeRequired Then btnSeeFile.Invoke(Sub() btnSeeFile.Enabled = True) Else btnSeeFile.Enabled = True
        p.Dispose()
        blockImage.Dispose()
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        sfdBlock.Filter = "PNG File (*.png)|*.png|All Files (*.*)|*.*"
        sfdBlock.ShowDialog()
    End Sub

    Private Sub sfdBlock_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles sfdBlock.FileOk
        If IO.File.Exists(My.Application.Info.DirectoryPath & "\3DBlock.png") Then
            IO.File.Delete(My.Application.Info.DirectoryPath & "\3DBlock.png")
        End If
        thGenerate = New Thread(AddressOf GenerateBlock)
        thGenerate.Start()
    End Sub

    Private Sub btnSeeFile_Click(sender As Object, e As EventArgs) Handles btnSeeFile.Click
        'Dim p As Process = Process.Start(New IO.DirectoryInfo(sfdBlock.FileName).Parent.FullName)
        Dim p As Process = Process.Start("explorer.exe", "/select," & sfdBlock.FileName)
        p.Dispose()
    End Sub

End Class