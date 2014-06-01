


Public Class frmBackup

    'Oggetto manager backup
    Dim WithEvents bkObject As DeviceBackup
    Dim bkRunning As Boolean
    Dim internalList As DeviceCollection
    Dim logFile As TextFormatters.TXTFormatter
    Dim currDevNode As TreeNode
    Dim elapsedTime As Stopwatch

    Public WriteOnly Property DeviceList() As DeviceCollection
        Set(ByVal value As DeviceCollection)
            'Scarta l'assegnazione se il backup e in esecuzione
            If bkRunning Then Return
            internalList = value
        End Set
    End Property

    Private Sub cmdBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBackup.Click
        Try

            If bkRunning Then Exit Sub 'Esce se l'operazione è in corso
            'Esce se non ci sono devices selezionati
            If internalList.Count <= 0 Then
                MsgBox(GetLangStr("BRE_NoDevices"), MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Modalità normale
            If Not Directory.Exists(txtPath.Text) Then
                MsgBox(GetLangStr("ERROR:FLD"), MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            bkObject = New DeviceBackup(internalList, txtPath.Text, txtDateFormat.Text)

            bkObject.BackupPathFormat = txtFormat.Text
            bkObject.FileManager = New BRStdFileManager(txtPath.Text)

            bkObject.DevicePathFormat = txtDevFormat.Text
            bkObject.CanOverwrite = chkOverwrite.Checked
            bkObject.Description = txtDesc.Text
            bkObject.BackupInfoFile = txtBackupFile.Text

            If bkObject.AsyncBackup = False Then
                MsgBox(GetLangStr("ERROR:BERR"), MsgBoxStyle.Critical)
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(GetLangStr("ERROR:BERR"), MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub frmBackup_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If bkRunning Then
            e.Cancel = True 'Non chiude il form se il backup è in esecuzione
            Exit Sub
        End If
    End Sub


    Public Function GeneratePathDemo() As Boolean
        Try
            Dim relativePath As New StringBuilder(Path.Combine(txtPath.Text, txtFormat.Text))
            Dim dt As DateTime = Date.Now

            relativePath.Replace("%COMPUTERNAME%", Utils.PathStringFilter(Utils.ComputerName))

            relativePath.Replace("%NOW%", Utils.PathStringFilter(Date.Today.ToString(txtDateFormat.Text).Replace("/", "-")))

            lblOutputPath.Text = relativePath.ToString

            Return True
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "DeviceBackup::FormatBackupPath", ex.Message)
            Return False
        End Try
    End Function

    Public Sub ShowDevices()
        'Implementazione del metodo ShowDevices
        Dim currNode As TreeNode
        Dim devFnd As Integer

        Try
            devTree.Visible = False
            devTree.Nodes.Clear()
            'Lancia un'eccezione se la collezione non contiene devices
            If Me.internalList Is Nothing OrElse Me.internalList.Count <= 0 Then Throw New ArgumentException

            For Each dv As Device In Me.internalList
                currNode = devTree.Nodes.Add(dv.Description)
                currNode.Tag = dv
                currNode.ImageKey = "icoSave"
                currNode.SelectedImageKey = currNode.ImageKey
                devFnd += 1
            Next

            lblDevFound.Text = [String].Format(GetLangStr("FRMBACK:DEVFOUND"), devFnd, CInt(Me.internalList.TotalDeviceFilesSize / 1000000))

        Catch ex As Exception
            devTree.Nodes.Add("NODEV", GetLangStr("FRMMAIN:NODEVICES"), "icoError")
            lblDevFound.Text = [String].Format(GetLangStr("FRMBACK:DEVFOUND"), 0, 0)
        Finally
            devTree.Visible = True
        End Try
    End Sub


    Public Sub LoadControls(ByVal imgList As ImageList)

        'Carica le risorse di linguaggio e collega l'imagelist
        Try
            devTree.Nodes.Clear()
            devTree.ImageList = imgList
            ShowDevices()
            cmdCancel.Visible = False
            cmdContinue.Visible = False
            cmdBackup.Visible = True
            cmdOk.Visible = True
            cmdCancel.Location = cmdBackup.Location
            cmdContinue.Location = cmdBackup.Location
            infoBox.Location = optBox.Location
            infoBox.Visible = False
            txtFormat.Text = My.Settings.StdBackupPathFormat
            txtDevFormat.Text = My.Settings.StdDevicePathFormat
            txtBackupFile.Text = My.Settings.StdBackupInfoFile
            txtDateFormat.Text = My.Settings.DateTimePattern
            Me.Icon = frmMain.Icon
            Me.MaximumSize = Me.Size

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::LoadControls", ex.Message)
        End Try

    End Sub

    Private Sub BackupBeginDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs)
        Try
            currDevNode = AddTreeNode(Nothing, e.Source.Description, "icoInfo")

            logFile.AddDevice(e.Source)
            deviceBar.Maximum = e.Source.DriverFiles.Count
            deviceBar.Value = 0
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupBeginDevice", ex.Message)
        End Try
    End Sub

    Private Sub BackupDeviceError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs)
        Try
            Dim currNode As TreeNode
            Dim errNotify As Boolean = False

            If currDevNode Is Nothing Then Exit Sub
            currNode = currDevNode.Nodes.Add(GetLangStr(e.Code.ToString))

            If e.Code = BackupRestoreErrorCodes.BRE_FileOverwiting Then
                'Aggiunge il nome del file che ha causato l'errore
                currNode.Text = currNode.Text & ": " & e.Data("Filename")
                logFile.AddMsgError(currNode.Text, True)
                errNotify = True
            End If

            If e.Code = BackupRestoreErrorCodes.BRE_FileIOError Then
                'Aggiunge maggiori informazioni
                currNode.Text = e.Data("Msg")
                logFile.AddMsgError(currNode.Text, True)
                errNotify = True
            End If

            currNode.ImageKey = "icoError"
            currNode.SelectedImageKey = currNode.ImageKey

            If Not errNotify Then logFile.AddError(e.Code, True)

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupDeviceError", ex.Message)
        End Try
    End Sub

    Private Sub BackupEndDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs)
        Try
            If currDevNode Is Nothing Then Exit Sub
            Dim currNode As TreeNode

            currNode = currDevNode.Nodes.Add([String].Format(GetLangStr("FRMBACK:ENDDEVICE"), e.FilesCopied, e.Source.DriverFiles.Count))

            If e.HasErrors Then
                currDevNode.ImageKey = "icoWarning"
            Else
                currDevNode.ImageKey = "icoInfo"
            End If

            currDevNode.SelectedImageKey = currDevNode.ImageKey
            currNode.ImageKey = "icoInfo"
            currNode.SelectedImageKey = currNode.ImageKey

            backupBar.Value = backupBar.Value + 1
            logFile.EndDevice(e.HasErrors)
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupEndDevice", ex.Message)
        End Try
    End Sub

    Private Sub BackupError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs)
        Try

            AddTreeNode(Nothing, GetLangStr(e.Code.ToString), "icoError")

            logFile.AddError(e.Code, False)
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupError", ex.Message)
        End Try
    End Sub

    Private Sub BackupFile(ByVal sender As Object, ByVal e As DeviceBackupRestore.FileEventArgs)
        Try
            Dim currNode As TreeNode
            If currDevNode Is Nothing Then Exit Sub

            currNode = currDevNode.Nodes.Add([String].Format(GetLangStr("FRMBACK:FILECOPIED"), e.FileName))

            If e.Errors Then
                currNode.ImageKey = "icoError"
                currNode.SelectedImageKey = currNode.ImageKey
            Else
                currNode.ImageKey = "icoInfo"
                currNode.SelectedImageKey = currNode.ImageKey
            End If

            lblFilename.Text = [String].Format("{0}  {1} bytes", e.FileName, e.Size)
            logFile.AddFile(e.FileName)
            deviceBar.Value = deviceBar.Value + 1
            Application.DoEvents()
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupFile", ex.Message)
        End Try
    End Sub

    Private Sub BackupStarted(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs)
        'Mostra solo il pulsante annulla
        Try
            'Pulisce il treeview
            devTree.Nodes.Clear()
            'Inizializza un nuovo TXTformatter
            logFile = New TextFormatters.TXTFormatter
            cmdBackup.Visible = False
            cmdContinue.Visible = False
            cmdCancel.Visible = True
            cmdOk.Visible = False
            cmdLog.Visible = False
            optBox.Visible = False
            infoBox.Visible = True
            bkRunning = True
            'Inizializza il timer
            elapsedTime = New Stopwatch
            elapsedTime.Start()
            'Inizializza le progressbar
            backupBar.Maximum = e.TotalDevices
            backupBar.Value = 0
            'Visualizza le informazioni sul treeview
            AddTreeNode(Nothing, [String].Format(GetLangStr("FRMBACK:BEGINBACKUP"), e.TotalDevices), "icoInfo")
            'Scrive l'evento sul file log
            logFile.BeginOperation([String].Format(GetLangStr("FRMBACK:BEGINBACKUP"), e.TotalDevices))

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupStarted", ex.Message)
        End Try

    End Sub

    Private Sub BackupEnded(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs)
        'Mostra il pulsante continua
        Try
            cmdCancel.Visible = False
            cmdBackup.Visible = False
            cmdContinue.Visible = True
            cmdOk.Visible = True
            cmdLog.Visible = True
            elapsedTime.Stop()
                'Tempo impegato
            logFile.AddMsg(AddTreeNode(Nothing, [String].Format(GetLangStr("FRMBACK_BACKUPTIME"), elapsedTime.ElapsedMilliseconds \ 1000L), "icoInfo").Text, False)

            'Termine operazioni
            bkRunning = False
            'Scrive l'evento sul file log
            logFile.EndOperation(AddTreeNode(Nothing, [String].Format(GetLangStr("FRMBACK:ENDBACKUP"), e.TotalDevices, Me.internalList.Count), "icoInfo").Text)

        Catch ex As Exception
            bkRunning = False
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BackupEnded", ex.Message)
        End Try

    End Sub


#Region "EVNMANAGER"
    'Funzioni di gestione eventi nascoste
    'Invocano funzioni private non nascoste utilizzando il thread del form
    Dim beginDeviceHandler As New BRDeviceProcessingHandler(AddressOf BackupBeginDevice)
    Dim errorDevice As New BRExceptionHandler(AddressOf BackupDeviceError)
    Dim endDeviceHandler As New BRDeviceProcessingHandler(AddressOf BackupEndDevice)
    Dim beginOpHandler As New BRBeginEndHandler(AddressOf BackupStarted)
    Dim endOpHandler As New BRBeginEndHandler(AddressOf BackupEnded)
    Dim fileHandler As New BRFileProcessingHandler(AddressOf BackupFile)
    Dim errorHandler As New BRExceptionHandler(AddressOf BackupError)

    Private Sub bkObject_BackupBeginDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs) Handles bkObject.BackupBeginDevice
        Me.Invoke(beginDeviceHandler, sender, e)
    End Sub

    Private Sub bkObject_BackupDeviceError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs) Handles bkObject.BackupDeviceError
        Me.Invoke(errorDevice, sender, e)
    End Sub

    Private Sub bkObject_BackupEndDevice(ByVal sender As Object, ByVal e As DeviceBackupRestore.DeviceEventArgs) Handles bkObject.BackupEndDevice
        Me.Invoke(endDeviceHandler, sender, e)
    End Sub

    Private Sub bkObject_BackupEnded(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs) Handles bkObject.BackupEnded
        Me.Invoke(endOpHandler, sender, e)
    End Sub

    Private Sub bkObject_BackupError(ByVal sender As Object, ByVal e As DeviceBackupRestore.ExceptionEventArgs) Handles bkObject.BackupError
        Me.Invoke(errorHandler, sender, e)
    End Sub

    Private Sub bkObject_BackupFile(ByVal sender As Object, ByVal e As DeviceBackupRestore.FileEventArgs) Handles bkObject.BackupFile
        Me.Invoke(fileHandler, sender, e)
    End Sub

    Private Sub bkObject_BackupStarted(ByVal sender As Object, ByVal e As DeviceBackupRestore.OperationEventArgs) Handles bkObject.BackupStarted
        Me.Invoke(beginOpHandler, sender, e)
    End Sub
#End Region

    Private Sub browseFolder(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
        Try
            Dim flsBw As New FolderBrowserDialog

            flsBw.ShowNewFolderButton = True

            If flsBw.ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtPath.Text = flsBw.SelectedPath
            End If
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::BrowseFolder", ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Me.Close() 'Chiude il form
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLog.Click
        Try
            If logFile IsNot Nothing Then
                Dim fileName As String
                Dim saveDlg As New SaveFileDialog
                saveDlg.CheckFileExists = False
                saveDlg.CheckPathExists = True

                saveDlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

                If saveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                    fileName = saveDlg.FileName
                    If logFile.Write(fileName) = False Then
                        MsgBox(GetLangStr("ERROR:FileWrite"), MsgBoxStyle.Exclamation)
                    Else
                        MsgBox(GetLangStr("FRMBACK:LOGSAVED"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::FileLogSave", ex.Message)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If bkObject Is Nothing Then Exit Sub
        bkObject.BackupCanceled = True 'Annulla il backup correntre

    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        Try
            'Genera eventualmente i files di Autorun
            If chkAutoRestore.Checked Then

                Utils.GenerateAutorun(bkObject.BackupPath, [String].Format(My.Settings.StdRestoreCmdLine, bkObject.BackupInfoFile, My.Settings.StdRestorePath), Path.GetDirectoryName(Application.ExecutablePath), CommonVariables.GetLanguageFiles)

            End If
            'Pulisce tutto e mostra di nuovo i devices selezionati
            cmdContinue.Visible = False
            cmdCancel.Visible = False
            cmdBackup.Visible = True
            cmdOk.Visible = True
            cmdLog.Visible = False
            infoBox.Visible = False
            optBox.Visible = True

            bkObject = Nothing
            ShowDevices()
            bkRunning = False
            Application.DoEvents()
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::cmdContinue", ex.Message)
        End Try
    End Sub

    Private Function AddTreeNode(ByVal pnode As TreeNode, ByVal text As String, ByVal img As String) As TreeNode
        Dim currNode As TreeNode

        If pnode Is Nothing Then
            currNode = devTree.Nodes.Add(text)
            currNode.ImageKey = img
            currNode.SelectedImageKey = img
            Return currNode
        Else
            currNode = pnode.Nodes.Add(text)
            currNode.ImageKey = img
            currNode.SelectedImageKey = img
            Return currNode
        End If

    End Function

    Private Sub txtPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath.TextChanged
        GeneratePathDemo()
    End Sub

    Private Sub txtDateFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDateFormat.TextChanged
        GeneratePathDemo()
    End Sub

    Private Sub txtFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFormat.TextChanged
        GeneratePathDemo()
    End Sub

    Private Sub frmBackup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class