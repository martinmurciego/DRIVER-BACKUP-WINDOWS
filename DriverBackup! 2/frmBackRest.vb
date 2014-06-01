Imports DriverBackup__2.DeviceManagement
Imports DriverBackup__2.DeviceBackupRestore

Public Class frmBackRest

    Public Enum FormBackupRestoreEventEnum
        Ev_Information
        Ev_Warning
        Ev_Error
    End Enum

    Dim m_dvc As DeviceCollection 'Collezione interna di oggetti
    Dim imgLst As ImageList
    Dim m_modeBackup As Boolean = True
    Dim m_opStarted As Boolean
    Dim WithEvents bk As DeviceBackup


    'Visualizzazione eventi
    Dim currEventNode As TreeNode
    Dim currError As Boolean

    'Variabili principali
    Dim br_Path As String
    Dim br_Format As String


    Public Property ModeBackup() As Boolean
        Get
            Return m_modeBackup
        End Get
        Set(ByVal value As Boolean)
            m_modeBackup = value
            If value Then
                cmdAction.Text = GetLangStr("FRMBACK:BTNBEGINBACKUP")
                groupBack.Visible = True
                Me.Text = GetLangStr("FRMBACK:TEXTBACK")

            Else
                cmdAction.Text = GetLangStr("FRMBACK:BTNBEGINRESTORE")
                groupBack.Visible = False
                Me.Text = GetLangStr("FRMBACK:TEXTRESTORE")
            End If
        End Set
    End Property

    Public Property CtrlImageList() As ImageList
        Get
            Return imgLst
        End Get
        Set(ByVal value As ImageList)
            Me.imgLst = value
        End Set
    End Property

    Public Property InternalCollection() As DeviceCollection
        Get
            Return m_dvc
        End Get
        Set(ByVal value As DeviceCollection)
            m_dvc = value
        End Set
    End Property


    Public Function ShowEvent(ByVal parentNode As TreeNode, ByVal eventDesc As String, ByVal eventTp As FormBackupRestoreEventEnum) As TreeNode
        'Garantisce la visualizzazione dei messaggi provenienti da thread differenti
        Try
            Dim icoName As String = Nothing
            Dim currNode As TreeNode = Nothing

            Select Case eventTp
                Case Is = FormBackupRestoreEventEnum.Ev_Error
                    icoName = "icoError"
                Case Is = FormBackupRestoreEventEnum.Ev_Information
                    icoName = "icoInfo"
                Case Is = FormBackupRestoreEventEnum.Ev_Warning
                    icoName = "IcoWarning"
            End Select

            If parentNode IsNot Nothing Then
                currNode = TryCast(devTree.Invoke(ThreadingUtilities.MethodInvoke, parentNode.Nodes, "Add", New Object() {eventDesc}), TreeNode)
            Else
                currNode = TryCast(devTree.Invoke(ThreadingUtilities.MethodInvoke, devTree.Nodes, "Add", New Object() {eventDesc}), TreeNode)
            End If

            If currNode IsNot Nothing Then
                devTree.Invoke(ThreadingUtilities.PropertyInvoke, currNode, "ImageKey", icoName, Nothing)
                devTree.Invoke(ThreadingUtilities.PropertyInvoke, currNode, "SelectedImageKey", icoName, Nothing)
            End If
            Application.DoEvents()

            Return currNode

        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Private Sub LoadControls()
        'Funzione che inizializza i controlli presenti sul form
        If imgLst IsNot Nothing Then
            devTree.ImageList = Me.imgLst
        End If

        txtFormat.Text = My.Settings.StdBackupPathFormat
        txtDevFormat.Text = My.Settings.StdDevicePathFormat
    End Sub

    Public Sub ShowDevices()
        'Implementazione del metodo ShowDevices
        Dim currNode As TreeNode
        Dim devFnd As Integer

        Try
            devTree.Visible = False
            devTree.Nodes.Clear()
            'Lancia un'eccezione se la collezione non contiene devices
            If m_dvc Is Nothing OrElse m_dvc.Count <= 0 Then Throw New ArgumentException

            For Each dv As Device In m_dvc

                If ModeBackup Then
                    'Controllo per la modalità backup
                    If Not CBool(dv.ExtendedInfo("Selected")) Then Continue For
                Else
                    'Controllo per la modalità Restore
                End If

                currNode = devTree.Nodes.Add(dv.Description)
                currNode.Tag = dv
                currNode.ImageKey = "icoSave"
                currNode.StateImageKey = currNode.ImageKey
                devFnd += 1
            Next

            lblDevFound.Text = [String].Format(GetLangStr("FRMBACK:DEVFOUND"), devFnd, CInt(m_dvc.TotalDeviceFilesSize / 1000000))

            If devFnd = 0 Then
                groupBack.Enabled = False
                cmdAction.Enabled = False
            End If

        Catch ex As Exception
            devTree.Nodes.Add("NODEV", GetLangStr("FRMMAIN:NODEVICES"), 0)
            lblDevFound.Text = [String].Format(GetLangStr("FRMBACK:DEVFOUND"), 0, 0)
        Finally
            devTree.Visible = True
        End Try
    End Sub


    Private Sub frmBackRest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadControls()

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()

    End Sub


    Private Function StartBackup() As Boolean
        Try

            If m_opStarted Then Return True

            If Directory.Exists(Me.br_Path) = False Then
                MsgBox(GetLangStr("ERROR:FLD"), MsgBoxStyle.Exclamation)
                Return False
            End If

            devTree.Nodes.Clear()

            'Inizializzazione dell'oggetto
            bk = New DeviceBackup(InternalCollection, br_Path)

            bk.BackupPathFormat = txtFormat.Text
            bk.DevicePathFormat = txtDevFormat.Text

            bk.CanOverwrite = chkOverwrite.Checked

            'Visualizza le informazioni
            groupBack.Visible = False
            groupInfo.Visible = True
            'Inizializza un nuovo thread

            If bk.AsyncBackup = True Then
                m_opStarted = True
                Return True
            Else
                m_opStarted = False
                Throw New Exception
                Return False
            End If

        Catch ex As Exception
            MsgBox(GetLangStr("ERROR:BERR"), MsgBoxStyle.Critical)
            StopBackup()
            Return False
        End Try

    End Function

    Public Sub StopBackup()
        'Ferma il thread backup
        'Elimina gli eventi dell'oggetto BACKUP
        If bk IsNot Nothing Then
            bk.BackupCanceled = True
           
        End If
        m_opStarted = False

        cmdAction.Visible = False
        cmdCancel.Visible = False
        cmdContinue.Visible = True
    End Sub

    Private Sub cmdAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAction.Click
        'Avvia o ferma le operazioni di backup 
        If m_modeBackup Then
            If Not StartBackup() Then Exit Sub
            cmdAction.Visible = False
            cmdContinue.Visible = False
            cmdCancel.Visible = True
        Else


        End If
        
    End Sub


    'routine di gestione degli eventi
    Private Sub OperationBegin(ByVal sender As Object, ByVal totalDevices As Integer)
        'Intestazioni di inizio operazioni backup
        currEventNode = Nothing
        If m_modeBackup Then
            'Modalità backup
            ShowEvent(Nothing, [String].Format(GetLangStr("FRMBACK:BEGINBACKUP"), My.Computer.Name, totalDevices), FormBackupRestoreEventEnum.Ev_Information)
        Else
            'Modalità restore
        End If

    End Sub

    Private Sub OperationEnd(ByVal sender As Object, ByVal totalDevices As Integer)
        'Intestazioni di inizio operazioni backup
        currEventNode = Nothing

        If m_modeBackup Then
            'Modalità backup
            ShowEvent(Nothing, [String].Format(GetLangStr("FRMBACK:ENDBACKUP"), totalDevices), FormBackupRestoreEventEnum.Ev_Information)
            Me.Invoke(ThreadingUtilities.MethodInvoke, Me, "StopBackup", Nothing)
        Else
            'Modalità restore

        End If
    End Sub

    Private Sub OperationBeginDevice(ByVal sender As Object, ByVal dev As Device, ByVal totalFiles As Integer)
        currEventNode = Nothing
        currError = False

        If m_modeBackup Then
            'Modalità backup
            currEventNode = ShowEvent(Nothing, dev.Description, FormBackupRestoreEventEnum.Ev_Information)
        Else
            'Modalità restore

        End If
    End Sub

    Private Sub OperationEndDevice(ByVal sender As Object, ByVal dev As Device, ByVal totalFiles As Integer)
        If m_modeBackup Then
            'Modalità backup
            ShowEvent(currEventNode, [String].Format(GetLangStr("FRMBACK:ENDDEVICE"), totalFiles, dev.DriverFiles.Count), FormBackupRestoreEventEnum.Ev_Information)
            If currError Then
                'Modifica l'icona in caso di errore
                devTree.Invoke(ThreadingUtilities.PropertyInvoke, currEventNode, "ImageKey", "icoWarning", Nothing)
            End If
        Else
            'Modalità restore

        End If
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim flsBw As New FolderBrowserDialog


        flsBw.ShowNewFolderButton = True

        If flsBw.ShowDialog() = Windows.Forms.DialogResult.OK Then
            br_Path = flsBw.SelectedPath
            txtPath.Text = br_Path

        End If


    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If m_modeBackup Then
            StopBackup()
        End If

    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        If m_modeBackup Then
            'Resetta la finestra per un nuovo backup

            groupBack.Visible = True
            groupInfo.Visible = False
            ShowDevices()
            cmdContinue.Visible = False
            cmdCancel.Visible = False
            cmdAction.Visible = True
        End If
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub chkAutoRestore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoRestore.CheckedChanged

    End Sub

    Private Sub chkOverwrite_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwrite.CheckedChanged

    End Sub

    Private Sub txtDevFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDevFormat.TextChanged

    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub txtFormat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFormat.TextChanged

    End Sub

    Private Sub chkAutorun_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutorun.CheckedChanged

    End Sub

    Private Sub txtPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPath.TextChanged

    End Sub
End Class