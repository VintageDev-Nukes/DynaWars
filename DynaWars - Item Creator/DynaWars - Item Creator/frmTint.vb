Public Class frmTint

    Dim filePath As String
    Dim savePath As String
    Dim recall As Boolean

    Private Sub Tint()
        If String.IsNullOrEmpty(filePath) Then
            ofdFileToTint.ShowDialog()
            recall = True
            Exit Sub
        End If
        If String.IsNullOrEmpty(savePath) Then
            sfdFileToSave.ShowDialog()
            recall = True
            Exit Sub
        End If
        Dim openImage As Image = Image.FromFile(filePath)
        If IO.File.Exists(savePath) Then
            IO.File.Delete(savePath)
        End If
        Dim newImage As Image = ImageTools.Tint(openImage, cdTintColor.Color, (tbDepth.Value / 1000), (tbContrast.Value / 1000), (tbOpacity.Value / 1000))
        newImage.Save(savePath)
        newImage.Dispose()
        openImage.Dispose()
        btnSeeFile.Enabled = True
    End Sub

    Private Sub btnColorSelect_Click(sender As Object, e As EventArgs) Handles btnColorSelect.Click
        cdTintColor.ShowDialog()
        lblColor.BackColor = cdTintColor.Color
    End Sub

    Private Sub btnTint_Click(sender As Object, e As EventArgs) Handles btnTint.Click
        Tint()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ofdFileToTint.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        sfdFileToSave.ShowDialog()
    End Sub

    Private Sub ofdFileToTint_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ofdFileToTint.FileOk
        filePath = ofdFileToTint.FileName
        txtSelectedFile.Text = filePath
        If recall Then
            recall = False
            Tint()
        End If
    End Sub

    Private Sub sfdFileToSave_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles sfdFileToSave.FileOk
        savePath = sfdFileToSave.FileName
        txtSavePath.Text = savePath
        If recall Then
            recall = False
            Tint()
        End If
    End Sub

    Private Sub btnSeeFile_Click(sender As Object, e As EventArgs) Handles btnSeeFile.Click
        Dim p As Process = Process.Start("explorer.exe", "/select," & savePath)
        p.Dispose()
    End Sub

    Private Sub tbDepth_ValueChanged(sender As Object, e As EventArgs) Handles tbDepth.ValueChanged
        lblDepth.Text = "Profundidad del color (" & (tbDepth.Value / 1000).ToString("F3") & ")"
    End Sub

    Private Sub tbContrast_ValueChanged(sender As Object, e As EventArgs) Handles tbContrast.ValueChanged
        lblContrast.Text = "Contraste (" & (tbContrast.Value / 1000).ToString("F3") & ")"
    End Sub

    Private Sub tbOpacity_ValueChanged(sender As Object, e As EventArgs) Handles tbOpacity.ValueChanged
        lblOpacity.Text = "Opacidad (" & (tbOpacity.Value / 1000).ToString("F3") & ")"
    End Sub

End Class