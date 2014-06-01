Public Class frmOffline

    Dim userOk As Boolean = False


    Private Sub LoadControls()
        Try
            Me.Icon = frmMain.Icon

            Me.MaximumSize = Me.Size
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmOffline::LoadControls", ex.Message)
        End Try
    End Sub

    Private Sub frmOffline_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not userOk Then
            If frmMain.OfflineBackupObj IsNot Nothing Then frmMain.OfflineBackupObj.Dispose()
            frmMain.OfflineBackupObj = Nothing
        Else
            frmMain.LoadBackupDevices()
        End If
    End Sub

    Private Sub frmOffline_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadControls()
    End Sub


    Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
        Try
            Dim flsBw As New FolderBrowserDialog

            flsBw.ShowNewFolderButton = False

            If flsBw.ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtsysPath.Text = flsBw.SelectedPath
            End If
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmOffline::BrowseFolder", ex.Message)
        End Try
    End Sub

    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Try
            If Not Directory.Exists(txtsysPath.Text) Then
                'Percorso non esistente
                MsgBox(GetLangStr("ERROR_FLD"), MsgBoxStyle.Exclamation)
                Return
            End If

            If frmMain.OfflineBackupObj IsNot Nothing Then frmMain.OfflineBackupObj.Dispose()

            frmMain.OfflineBackupObj = DeviceBackupOffline.Create(txtsysPath.Text)

            If frmMain.OfflineBackupObj Is Nothing Then
                MsgBox(GetLangStr("FRMOFFLINE_GENERIC"), MsgBoxStyle.Critical)
                Return
            End If

            If frmMain.OfflineBackupObj.HasPathError Then
                MsgBox(GetLangStr("FRMOFFLINE_PATH"), MsgBoxStyle.Exclamation)
                frmMain.OfflineBackupObj.Dispose()
                frmMain.OfflineBackupObj = Nothing
                Return
            End If

            If frmMain.OfflineBackupObj.HasPrivilegeError Then
                MsgBox(GetLangStr("FRMOFFLINE_PRIVILEGE"), MsgBoxStyle.Exclamation)
                frmMain.OfflineBackupObj.Dispose()
                frmMain.OfflineBackupObj = Nothing
                Return
            End If

            lblPcName.Text = frmMain.OfflineBackupObj.ComputerName

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        'Conferma la scelta dell'utente
        If frmMain.OfflineBackupObj IsNot Nothing Then
            frmMain.OfflineBackupObj.UseOfflinePCName = chkUseOfflineName.Checked
            userOk = True
        End If
        Me.Close()
    End Sub
End Class