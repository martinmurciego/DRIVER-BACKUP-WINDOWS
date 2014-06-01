Public Class frmCmdBuilder

    Dim cmdBuilder As New CommandLineManager.CommandLineBuilder


    Private Sub LoadControls()
        Try
            txtFormat.Text = My.Settings.StdBackupPathFormat
            txtDevFormat.Text = My.Settings.StdDevicePathFormat
            txtBackupFile.Text = My.Settings.StdBackupInfoFile
            txtDateFormat.Text = My.Settings.DateTimePattern


            cmdBuilder.Mode = 0
            groupRestore.Visible = False
            groupRestore.Location = groupBackup.Location
            Me.Icon = frmMain.Icon
            Me.MaximumSize = Me.Size

        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmCmdBuilder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadControls()

    End Sub

    Private Sub txtPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath.TextChanged
        cmdBuilder.BackupPath = txtPath.Text

    End Sub

    Private Sub txtDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesc.TextChanged
        cmdBuilder.BackupDescription = txtDesc.Text

    End Sub

    Private Sub txtBackupFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBackupFile.TextChanged
        cmdBuilder.BackupFileName = txtBackupFile.Text

    End Sub

    Private Sub txtFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFormat.TextChanged
        cmdBuilder.BackupPathFormat = txtFormat.Text

    End Sub

    Private Sub txtDevFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDevFormat.TextChanged
        cmdBuilder.BackupDevFormat = txtDevFormat.Text

    End Sub

    Private Sub chkOverwrite_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwrite.CheckedChanged
        cmdBuilder.OverwriteFile = chkOverwrite.Checked

    End Sub

    Private Sub chkAutoRestore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoRestore.CheckedChanged
        cmdBuilder.GenerateAutorun = chkAutoRestore.Checked

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim result As String = cmdBuilder.Build

        If [String].IsNullOrEmpty(result) Then
            MsgBox(GetLangStr("FRMBUILDER:BADSETTINGS"), MsgBoxStyle.Exclamation)
            txtOutput.Text = ""
        Else
            txtOutput.Text = result
        End If


    End Sub

    Private Sub optBackup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optBackup.CheckedChanged
        If optBackup.Checked = True Then
            cmdBuilder.Mode = 0
            groupBackup.Visible = True
            groupRestore.Visible = False
            optRecommended.Visible = False
            optAll.Checked = True
        End If

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            Dim flsBw As New FolderBrowserDialog

            flsBw.ShowNewFolderButton = True

            If flsBw.ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtPath.Text = flsBw.SelectedPath
            End If
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmCmdBuilder::BrowseFolder", ex.Message)
        End Try
    End Sub

    Private Sub optRestore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optRestore.CheckedChanged
        If optRestore.Checked = True Then
            cmdBuilder.Mode = 1
            groupBackup.Visible = False
            groupRestore.Visible = True
            optRecommended.Visible = True
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Copia la riga di comando sulla clipboard
        My.Computer.Clipboard.SetText(txtOutput.Text)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim fOpen As OpenFileDialog
        Try

            fOpen = New OpenFileDialog
            fOpen.CheckFileExists = True
            fOpen.Filter = [String].Format("Backup files (*{0})|*{0}|All Files (*.*)|*.*", My.Settings.StdBackupInfoExt)
            If fOpen.ShowDialog = Windows.Forms.DialogResult.OK Then
                txtRestoreFile.Text = Path.GetFileName(fOpen.FileName)
            End If
            fOpen.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtRestoreFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRestoreFile.TextChanged
        cmdBuilder.RestoreFileName = txtRestoreFile.Text

    End Sub

    Private Sub chkPnPUpdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPnPUpdate.CheckedChanged
        cmdBuilder.EnabledPnPRescan = chkPnPUpdate.Checked

    End Sub

    Private Sub chkForceUpdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkForceUpdate.CheckedChanged
        cmdBuilder.UpdateOEMInf = chkForceUpdate.Checked
    End Sub

    Private Sub optAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAll.CheckedChanged
        If optAll.Checked = True Then
            cmdBuilder.Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_All
        End If
    End Sub

    Private Sub optOEM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optOEM.CheckedChanged
        If optOEM.Checked = True Then
            cmdBuilder.Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Oem
        End If
    End Sub

    Private Sub optThird_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optThird.CheckedChanged
        If optThird.Checked = True Then
            cmdBuilder.Filter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Others
        End If
    End Sub

    Private Sub chkSignature_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSignature.CheckedChanged
        cmdBuilder.Filter.MustSigned = chkSignature.Checked
    End Sub

    Private Sub chkPort_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPort.CheckedChanged
        If chkPort.Checked = True Then
            cmdBuilder.Filter.Portability = DevicePortability.DCmp_Full
        Else
            cmdBuilder.Filter.Portability = -1
        End If

    End Sub

   
    Private Sub chkLog_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLog.CheckedChanged
        cmdBuilder.Logging = chkLog.Checked

    End Sub

    Private Sub txtDateFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDateFormat.TextChanged
        cmdBuilder.BackupDateFormat = txtDateFormat.Text

    End Sub

    Private Sub optRecommended_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optRecommended.CheckedChanged
        If optRecommended.Checked = True Then
            cmdBuilder.Filter.ProviderType = -1
        End If
    End Sub

    Private Sub chkVerbose_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVerbose.CheckedChanged
        cmdBuilder.DisableInteraction = chkVerbose.Checked

    End Sub

    Private Sub chkOffline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOffline.CheckedChanged
        If chkOffline.Checked Then
            'Modalità backup offline
            cmdBuilder.SystemDirectory = txtSysPath.Text
        Else
            cmdBuilder.SystemDirectory = ""
        End If
        chkUseOfflineName.Visible = chkOffline.Checked

    End Sub

    Private Sub chkUseOfflineName_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseOfflineName.CheckedChanged
        cmdBuilder.UseOfflineComputerName = chkUseOfflineName.Checked
    End Sub
End Class