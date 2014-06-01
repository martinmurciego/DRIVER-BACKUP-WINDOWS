Public Class frmRestore

    Dim rsRunning As Boolean
    Dim logFile As TextFormatters.TXTFormatter
    Dim currDevNode As TreeNode
    Dim WithEvents devRestorer As DeviceRestore
    Dim internalList As DeviceCollection
    Dim sourcePath As String

    Public Property RestorerObj() As DeviceRestore
        Get
            Return Me.devRestorer
        End Get
        Set(ByVal value As DeviceRestore)
            Me.devRestorer = value
            Me.internalList = DeviceCollection.Create(devRestorer.DeviceList, "Selected", True)
        End Set
    End Property

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
                currNode.ImageKey = "icoRestore"
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


    Public Sub InitializeControls(ByVal imgList As ImageList)
        Try
            devTree.Nodes.Clear()
            devTree.ImageList = imgList
            cmdRestore.Visible = True
            cmdOk.Visible = True
            cmdCancel.Visible = False
            cmdLog.Visible = False
            cmdContinue.Visible = False
            cmdContinue.Location = cmdRestore.Location
            cmdCancel.Location = cmdOk.Location
            Me.Icon = frmMain.Icon
            Me.MaximumSize = Me.Size
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmBackup::LoadControls", ex.Message)
        End Try
    End Sub


    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        'Chiude la finestra
        Me.Close()
    End Sub

    Private Sub cmdRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestore.Click
        Try

            If rsRunning Then Exit Sub 'Esce se l'operazione è in corso

            If internalList.Count <= 0 Then
                MsgBox(GetLangStr("BRE_NoDevices"), MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            devRestorer.UpdateDeviceInfo = chkForceUpdate.Checked

            If devRestorer.AsyncRestoreDevices = False Then
                MsgBox(GetLangStr("ERROR:BERR"), MsgBoxStyle.Critical)
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(GetLangStr("ERROR:BERR"), MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub RestoreStarted(ByVal sender As Object, ByVal e As OperationEventArgs)
        'Mostra solo il pulsante annulla
        Dim currNode As TreeNode
        Dim tempnode As TreeNode

        Try
            'Pulisce il treeview
            devTree.Nodes.Clear()
            'Inizializza un nuovo TXTformatter
            logFile = New TextFormatters.TXTFormatter
            cmdRestore.Visible = False
            cmdContinue.Visible = False
            cmdCancel.Visible = True
            cmdOk.Visible = False
            cmdLog.Visible = False
            rsRunning = True
            'Visualizza le informazioni sul treeview
            currNode = devTree.Nodes.Add([String].Format(GetLangStr("FRMRESTORE:BEGINRESTORE"), e.TotalDevices))
            currNode.ImageKey = "icoInfo"
            currNode.SelectedImageKey = currNode.ImageKey
            tempnode = devTree.Nodes.Add([String].Format(GetLangStr("LOG_OperationStarted"), [Date].Now))
            tempnode.ImageKey = "icoInfo"
            tempnode.SelectedImageKey = tempnode.ImageKey
            'Scrive l'evento sul file log
            logFile.BeginOperation([String].Format(GetLangStr("FRMRESTORE:BEGINRESTORE"), e.TotalDevices))
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::RestoreStarted", ex.Message)
        End Try

    End Sub

    Private Sub RestoreEnded(ByVal sender As Object, ByVal e As OperationEventArgs)
        Dim currNode As TreeNode
        Dim tempNode As TreeNode
        Try
            cmdCancel.Visible = False
            cmdRestore.Visible = False
            cmdContinue.Visible = True
            cmdOk.Visible = True
            cmdLog.Visible = True
            currNode = devTree.Nodes.Add([String].Format(GetLangStr("FRMRESTORE:ENDRESTORE"), e.TotalDevices, Me.internalList.Count))
            currNode.ImageKey = "icoInfo"
            currNode.SelectedImageKey = currNode.ImageKey
            rsRunning = False

            'Scrive l'evento sul file log
            logFile.EndOperation([String].Format(GetLangStr("FRMRESTORE:ENDRESTORE"), e.TotalDevices, Me.internalList.Count))

            tempNode = devTree.Nodes.Add([String].Format(GetLangStr("LOG_OperationEnded"), [Date].Now))
            tempNode.ImageKey = "icoInfo"
            tempNode.SelectedImageKey = tempNode.ImageKey

            If chkPnPUpdate.Checked = True Then
                'Aggiorna la configurazione PnP
                If DeviceRestore.PnPConfigUpdate = True Then
                    'Configurazione aggiornata
                    MsgBox(GetLangStr("FRMRESTORE:PNPRESCAN"), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
                Else
                    'Impossibile aggiornare la configurazione
                    MsgBox(GetLangStr("FRMRESTORE:PNPRESCANFAILED"), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation)
                End If
            End If

        Catch ex As Exception
            rsRunning = False
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::RestoreEnded", ex.Message)
        End Try
    End Sub

    Private Sub RestoreBeginDevice(ByVal sender As Object, ByVal e As DeviceEventArgs)
        Try
            e.Cancel = False
            currDevNode = devTree.Nodes.Add(e.Source.Description)
            currDevNode.ImageKey = "icoInfo"
            currDevNode.SelectedImageKey = currDevNode.ImageKey
            
            logFile.AddDevice(e.Source)
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::RestoreBeginDevice", ex.Message)
        End Try
    End Sub

    Private Sub RestoreEndDevice(ByVal sender As Object, ByVal e As DeviceEventArgs)
        Try
            If currDevNode Is Nothing Then Exit Sub
            Dim currNode As TreeNode

            If e.Data.ContainsKey("OEMINF") AndAlso Not [String].IsNullOrEmpty(e.Data.Item("OEMINF")) Then
                currNode = currDevNode.Nodes.Add([String].Format(GetLangStr("FRMRESTORE:OEMINF"), e.Data("OEMINF")))
                logFile.AddMsg(currNode.Text, True)
                currNode.ImageKey = "icoInfo"
                currNode.SelectedImageKey = currNode.ImageKey
            End If

            currNode = currDevNode.Nodes.Add(GetLangStr("FRMRESTORE:ENDDEVICE"))
            currNode.ImageKey = "icoInfo"
            currNode.SelectedImageKey = currNode.ImageKey

            If e.HasErrors Then
                currDevNode.ImageKey = "icoWarning"
                currDevNode.SelectedImageKey = currDevNode.ImageKey
            End If

            logFile.EndDevice(e.HasErrors)
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::RestoreEndDevice", ex.Message)
        End Try
    End Sub

    Private Sub RestoreError(ByVal sender As Object, ByVal e As ExceptionEventArgs)
        Try
            Dim currNode As TreeNode
            currNode = devTree.Nodes.Add(e.Data.Item("Msg"))

            currNode.ImageKey = "icoError"
            currNode.SelectedImageKey = currNode.ImageKey
            logFile.AddMsgError(currNode.Text, False)

        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::RestoreError", ex.Message)
        End Try
    End Sub

    Private Sub RestoreDeviceError(ByVal sender As Object, ByVal e As ExceptionEventArgs)
        Try
            Dim currNode As TreeNode

            If e.Code = BackupRestoreErrorCodes.BRE_ForceUpdate Then
                'Gestisce la forzatura
                If MsgBox([String].Format(GetLangStr(e.Code.ToString), DirectCast(e.Data("Device"), Device).Description), MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If
                Exit Sub
            End If

            If currDevNode Is Nothing Then Exit Sub
            currNode = currDevNode.Nodes.Add(GetLangStr(e.Code.ToString))
            currNode.ImageKey = "icoError"
            currNode.SelectedImageKey = currNode.ImageKey

            If e.Code = BackupRestoreErrorCodes.BRE_Generic Then
                'Aggiunge maggiori informazioni
                currNode.Text = e.Data("Msg")
                logFile.AddMsgError(currNode.Text, True)
                Return
            End If

            logFile.AddError(e.Code, True)


        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::RestoreDeviceError", ex.Message)
        End Try
    End Sub
#Region "EVMANAGER"

    'Garantisce che le routine di evento siano eseguite dal thread che ha creato il form
    Dim beginDeviceHandler As New BRDeviceProcessingHandler(AddressOf RestoreBeginDevice)
    Dim errorDevice As New BRExceptionHandler(AddressOf RestoreDeviceError)
    Dim endDeviceHandler As New BRDeviceProcessingHandler(AddressOf RestoreEndDevice)
    Dim beginOpHandler As New BRBeginEndHandler(AddressOf RestoreStarted)
    Dim endOpHandler As New BRBeginEndHandler(AddressOf RestoreEnded)
    Dim errorHandler As New BRExceptionHandler(AddressOf RestoreError)

    Private Sub rsObject_RestoreStarted(ByVal sender As Object, ByVal e As OperationEventArgs) Handles devRestorer.RestoreBegin
        Try
            Me.Invoke(beginOpHandler, sender, e)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub rsObject_RestoreEnded(ByVal sender As Object, ByVal e As OperationEventArgs) Handles devRestorer.RestoreEnd
        Try
            Me.Invoke(endOpHandler, sender, e)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub rsObject_RestoreBeginDevice(ByVal sender As Object, ByVal e As DeviceEventArgs) Handles devRestorer.RestoreBeginDevice
        Try
            Me.Invoke(beginDeviceHandler, sender, e)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub rsObject_RestoreEndDevice(ByVal sender As Object, ByVal e As DeviceEventArgs) Handles devRestorer.RestoreEndDevice
        Try
            Me.Invoke(endDeviceHandler, sender, e)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub rsObject_RestoreError(ByVal sender As Object, ByVal e As ExceptionEventArgs) Handles devRestorer.RestoreError
        Try
            Me.Invoke(errorHandler, sender, e)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub rsObject_RestoreDeviceError(ByVal sender As Object, ByVal e As ExceptionEventArgs) Handles devRestorer.RestoreDeviceError
        Try
            Me.Invoke(errorDevice, sender, e)
        Catch ex As Exception
        End Try
    End Sub

#End Region

    Private Sub cmdLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLog.Click
        Try
            If logFile IsNot Nothing Then
                Dim fileName As String
                Dim saveDlg As New SaveFileDialog
                saveDlg.CheckFileExists = False
                saveDlg.CheckPathExists = True
                'saveDlg.DefaultExt = ".txt"
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
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::FileLogSave", ex.Message)
        End Try
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        Try
            'Pulisce tutto e mostra di nuovo i devices selezionati
            cmdContinue.Visible = False
            cmdCancel.Visible = False
            cmdRestore.Visible = True
            cmdOk.Visible = True
            cmdLog.Visible = False
            ShowDevices()
            rsRunning = False
            Application.DoEvents()
        Catch ex As Exception
            Debug.Print(My.Settings.DebugStringFormat, "frmRestore::cmdContinue", ex.Message)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If devRestorer IsNot Nothing Then
            devRestorer.RestoreCanceled = True 'Annulla l'operazione in corso
        End If
    End Sub

    Private Sub devTree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles devTree.AfterSelect
        e.Node.SelectedImageKey = e.Node.ImageKey
    End Sub


End Class