
Imports DriverBackup__2.DeviceManagement
Imports DriverBackup__2.CommonVariables
Imports System.IO.Compression

Public Class frmMain

    Private m_currDevices As New DeviceCollection 'Oggetto contenente i devices da visualizzare
    Private m_RestoreDevs As New DeviceCollection
    Public OfflineBackupObj As DeviceBackupOffline
    Private devFilter As New DeviceFilter 'Crea il filtro dei devices
    Private devRestorer As DeviceRestore
    Private modeBackup As Boolean = True
    Private cons As Integer
    Private fixedSize As Size = New Size(763, 660)
    Private commandMode As Boolean


    Public Sub LoadBackupDevices()
        lblBarInfo.Visible = True
        devScanBar.Visible = True

        m_currDevices = DeviceCollection.Create(AddressOf DevScanning)

        lblBarInfo.Visible = False
        devScanBar.Visible = False

        RefreshDevices()

    End Sub

    Private Sub ShowUnknownDevices()
        Dim fs As FileStream = Nothing

        Try
            Dim pnpDev As Dictionary(Of String, Boolean) = DeviceRestore.EnumPnPDevices
            Dim pciFile As String = Path.Combine(My.Application.Info.DirectoryPath, My.Settings.PCIDatabase)
            Dim currNode As TreeNode
            Dim currObj As Utils.HWOBJ = Nothing

            If pnpDev Is Nothing Then Return

            If File.Exists(pciFile) = False Then
                'Database non trovato
                MsgBox(GetLangStr("FRMMAIN_NOPCIDTB"), MsgBoxStyle.Critical)
                Return
            End If

            treeDevices.Nodes.Clear()
            'Apre il filestream
            fs = New FileStream(Path.Combine(My.Application.Info.DirectoryPath, My.Settings.PCIDatabase), FileMode.Open)

            For Each k As KeyValuePair(Of String, Boolean) In pnpDev
                If k.Key.ToLower.StartsWith("pci") Then
                    'Processa solo i device connessi al bus PCI
                    currObj = Utils.LocateUnknownDevice(k.Key.ToLower, fs)

                    If currObj Is Nothing OrElse [String].IsNullOrEmpty(currObj.Vendor) Then Continue For

                    If Not treeDevices.Nodes.ContainsKey(currObj.Vendor) Then
                        'Aggiunge un nuovo nodo radice corrispondente al produttore hardware

                    End If

                    If k.Value = True Then
                        'Device con problemi
                        currNode.ImageKey = "icoWarning"
                    Else
                        currNode.ImageKey = "cmdPort"
                    End If
                    currNode.SelectedImageKey = currNode.ImageKey
                End If
            Next


        Catch ex As Exception
            treeDevices.Nodes.Add("NODEV", GetLangStr("FRMMAIN:NODEVICES"), "icoError", "icoError")
        Finally
            If fs IsNot Nothing Then fs.Close()
        End Try

    End Sub


    Private Sub ShowPckInfo()
        Try
            If devRestorer Is Nothing OrElse devRestorer.Info Is Nothing Then Exit Sub

            pckInfoBox.Visible = True
            devInfoBox.Visible = False
            With devRestorer.Info
                Me.lblPckComputerName.Text = .ComputerName
                Me.lblPckDate.Text = .CreationDate
                Me.lblPckDesc.Text = .Description
                Me.lblPckSystem.Text = .SystemDescription
                Me.lblPckSysVersion.Text = .SystemVersion.ToString
                Me.lblPckDrvVersion.Text = .DrvVersion.ToString

                If .IsDifferentSystem Then
                    'Versione differente di sistema operativo
                    lblPckInfo.Text = GetLangStr("FRMMAIN:DIFFERENTSYSTEMS")
                    pictPck.Visible = True
                Else
                    lblPckInfo.Text = ""
                    pictPck.Visible = False
                End If

            End With

        Catch ex As Exception
            pckInfoBox.Visible = False

        End Try
    End Sub

    Private Sub ShowDevInfo(ByVal dev As Device)
        Try

            If dev Is Nothing Then
                devInfoBox.Visible = False
                Return
            End If

            devInfoBox.Visible = True
            pckInfoBox.Visible = False

            lblProduttore.Text = dev.ProviderName
            lblVersione.Text = dev.ReleaseVersion.ToString
            lblData.Text = dev.ReleaseDate.ToString
            lblFileINF.Text = dev.InstallationFile
            lblID.Text = dev.MatchingID
            lblDevTipo.Text = dev.ClassInfo.ClassName

            If dev.IsDigitalSigned Then
                lblFirma.Text = GetLangStr("YES")
                lblFirma.ForeColor = Color.Green
            Else
                lblFirma.Text = GetLangStr("NO")
                lblFirma.ForeColor = Color.Red
            End If
            If modeBackup Then
                Select Case dev.PortabilityLevel
                    Case Is = DevicePortability.DCmp_Full
                        lblDevPort.ForeColor = Color.Green
                        lblOpSystem.Text = GetLangStr("FRMMAIN:COMPFULL")
                    Case Is = DevicePortability.DCmp_None
                        lblDevPort.ForeColor = Color.Red
                        lblOpSystem.Text = GetLangStr("FRMMAIN:COMPNONE")
                    Case Is = DevicePortability.DCmp_Partial
                        lblDevPort.ForeColor = Color.Orange
                        lblOpSystem.Text = GetLangStr("FRMMAIN:COMPPARTIAL")
                End Select
            Else
                Select Case dev.PortabilityLevel
                    Case Is = DevicePortability.DCmp_Full
                        lblDevPort.ForeColor = Color.Green
                        Select Case DirectCast(dev.ExtendedInfo.Item("RestoreMode"), Int32)
                            Case Is = 1
                                lblOpSystem.Text = GetLangStr("FRMMAIN:DRIVERREQUIRED")
                            Case Is = 0
                                If dev.ExtendedInfo.ContainsKey("IsNewer") Then
                                    lblOpSystem.Text = GetLangStr("FRMMAIN:DRIVERUPDATE")
                                Else
                                    lblOpSystem.Text = GetLangStr("FRMMAIN:DRIVERNOTREQUIRED")
                                End If
                        End Select

                    Case Is = DevicePortability.DCmp_None
                        lblDevPort.ForeColor = Color.Red
                        lblOpSystem.Text = GetLangStr("FRMMAIN:RCOMPNONE")
                    Case Is = DevicePortability.DCmp_Partial
                        lblDevPort.ForeColor = Color.Orange
                        lblOpSystem.Text = GetLangStr("FRMMAIN:RCOMPPARTIAL")
                End Select
            End If

            If dev.HasAllFiles Then
                lblFiles.Text = GetLangStr("FRMMAIN:FILES")
                lblFiles.ForeColor = Color.Green
            Else
                lblFiles.Text = GetLangStr("FRMMAIN:NOFILES")
                lblFiles.ForeColor = Color.Red
            End If

            lblDevPort.Text = GetLangStr(dev.PortabilityLevel.ToString)

        Catch ex As Exception
            devInfoBox.Visible = False
        End Try
    End Sub

    Private Sub ShowDevices(ByVal lst As DeviceCollection)
        Dim currClass As DeviceClass = Nothing
        Dim currClassNode As TreeNode = Nothing
        Dim currNode As TreeNode = Nothing
        Dim currFileNode As TreeNode
        Dim totalDevs As Integer

        Try
            treeDevices.Visible = False 'Evita sfarfallii
            treeDevices.Nodes.Clear()

            totalDevs = lst.SetDevicesProperties(devFilter, "Visible", GetType(Boolean), True, False)
            'Label conteggio devices
            lblDispTrovati.Text = [String].Format(GetLangStr("FRMMAIN:DEVFOUND"), totalDevs, lst.Count)

            If totalDevs <= 0 Then Throw New ArgumentException

            For Each dv As Device In lst
                'Determina sel il device deve essere visualizzato
                If Not CBool(dv.ExtendedInfo("Visible")) Then Continue For

                If currClass Is Nothing OrElse dv.ClassInfo <> currClass Then
                    currClass = dv.ClassInfo

                    'Mostra il nodo informazioni classe
                    If modeBackup Then
                        currClassNode = treeDevices.Nodes.Add([String].Format(GetLangStr("FRMMAIN:TREENODEDEV"), dv.ClassInfo.ClassDescription, dv.ClassInfo.Devices.Count))
                    Else
                        currClassNode = treeDevices.Nodes.Add(dv.ClassInfo.ClassDescription)
                    End If

                    currClassNode.ForeColor = Color.Blue
                    currClassNode.Tag = "CLASSNODE" 'Contrassegna il nodo come nodo chiave
                    currClassNode.Checked = True
                    'Aggiunge l'icona della classe all'imagelist
                    If modeBackup Then
                        Try
                            If ImageList1.Images.ContainsKey(currClass.ClassName) = False Then
                                'Aggiunge l'immagine solo se non è già stata aggiunta
                                ImageList1.Images.Add(currClass.ClassName, currClass.ClassIcon)
                            End If
                            currClassNode.ImageKey = currClass.ClassName
                            currClassNode.SelectedImageKey = currClassNode.ImageKey
                        Catch ex As Exception
                            'Impossibile caricare l'icona
                            currClassNode.ImageKey = ""
                            currClassNode.SelectedImageKey = ""
                        End Try
                    Else
                        currClassNode.ImageKey = "icoRestoreClass"
                        currClassNode.SelectedImageKey = currClassNode.ImageKey
                    End If

                End If
                'Mostra le informazioni del device
                If currClassNode IsNot Nothing Then
                    currNode = currClassNode.Nodes.Add(dv.Description)
                    currNode.Tag = dv 'Assegna al nodo l'oggetto device corrispondente
                    currNode.ImageKey = currClassNode.ImageKey
                    currNode.SelectedImageKey = currNode.ImageKey
                    currNode.Checked = CBool(dv.ExtendedInfo.Item("Selected"))

                    If Not modeBackup Then
                        'Variazione del codice per il modo restore
                        currNode.ImageKey = "icoRestore"
                        currNode.SelectedImageKey = currNode.ImageKey
                        If dv.ExtendedInfo.ContainsKey("IsNewer") Or DirectCast(dv.ExtendedInfo.Item("RestoreMode"), Int32) = 1 Then
                            currNode.ForeColor = Color.Green
                            currNode.NodeFont = New Font(Me.Font, FontStyle.Bold)
                        End If

                        If Not dv.HasAllFiles Then
                            'Alcuni files sono mancanti
                            currNode.ForeColor = Color.Red
                            currNode.ImageKey = "icoWarning"
                            currNode.SelectedImageKey = currNode.ImageKey
                        End If
                    End If

                    'Visualizza i files
                    For Each dvFile As DeviceFile In dv.DriverFiles
                        currFileNode = currNode.Nodes.Add(dvFile.FullName)
                        If dvFile.Exist Then
                            currFileNode.ImageKey = "fileOK" 'ImageIndex= 2
                        Else
                            currFileNode.ImageKey = "fileNO" 'ImageIndex = 1
                        End If
                        currFileNode.SelectedImageKey = currFileNode.ImageKey
                        currFileNode.Tag = "FILENODE"
                    Next
                    'currNode.Collapse()
                    currClassNode.Expand()
                End If
            Next


        Catch ex As Exception
            If modeBackup Then
                treeDevices.Nodes.Add("NODEV", GetLangStr("FRMMAIN:NODEVICES"), "icoError", "icoError")
            Else
                treeDevices.Nodes.Add("NODEV", GetLangStr("FRMMAIN:NORESTDEVICES"), "icoWarning", "icoWarning")
            End If
        Finally
            'treeDevices.ExpandAll()
            treeDevices.Visible = True
        End Try
    End Sub

    Public Sub RefreshDevices()
        Try
            If modeBackup Then
                m_currDevices.SetDevicesProperties(devFilter, "Selected", GetType(Boolean), True, False)
                ShowDevices(m_currDevices)
            Else
                ShowDevices(m_RestoreDevs)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DevScanning(ByVal sender As Object, ByVal currClass As DeviceClass, ByVal currClassN As Integer, ByVal totalClasses As Integer)
        'Mostra la barra di avanzamento
        devScanBar.Maximum = totalClasses
        devScanBar.Value = currClassN
        lblBarInfo.Text = currClass.ClassDescription
        Application.DoEvents()
    End Sub

    Private Sub InitializeControls()
        'Inizializza i controlli del form
        Try

            panelRestore.Location = panelBackup.Location
            pckInfoBox.Visible = False
            pckInfoBox.Location = devInfoBox.Location
            With ImageList1.Images
                pictPck.Image = Me.ImageList1.Images.Item("icoWarning")
                pictDev.Image = Me.ImageList1.Images.Item("icoInfo")
                cmdBackup.Image = .Item("icoSaveAll")
                cmdUpdate.Image = .Item("icoRefresh")
                cmdRestore.Image = .Item("icoRedo")
                cmdOpenBkFile.Image = .Item("icoOpen")
                cmdAll.Image = .Item("provAll")
                cmdOthers.Image = .Item("provOthers")
                cmdOem.Image = .Item("provOem")
                cmdFullPort.Image = .Item("cmdPort")
                cmdSignature.Image = .Item("cmdSignature")
                'cmdRemoveOEM.Image = .Item("icoDelete")
                cmdBackupMode.Image = .Item("icoSave")
                cmdRestoreMode.Image = .Item("icoFrmRestore")
            End With

            popupAll.Checked = True
            popupOem.Checked = False
            popupThird.Checked = False
            popupSignature.Checked = False
            popupPortability.Checked = False
            'Handler d'evento
            AddHandler popupAll.Click, AddressOf Me.cmdAll_Click
            AddHandler popupOem.Click, AddressOf Me.cmdOem_Click
            AddHandler popupThird.Click, AddressOf Me.cmdTerzeParti_Click
            AddHandler popupSignature.Click, AddressOf Me.cmdFirma_Click
            AddHandler popupPortability.Click, AddressOf Me.cmdPienaP_Click
            'Ridimensiona se serve i controlli sul form
            Me.MaximumSize = Me.fixedSize

            Dim scr As Size = New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)

            If (scr.Width * scr.Height) < (Me.Size.Height * Me.Size.Width) Then
                Dim scale As Double = (scr.Width * scr.Height) / ((Me.Size.Height * Me.Size.Width) * 1.2)

                'Ridimensiona i forms
                Me.Size = New Size(Me.Size.Width * scale, Me.Size.Height * scale)
                frmBackup.Size = New Size(frmBackup.Size.Width * scale, frmBackup.Size.Height * scale)
                frmRestore.Size = New Size(frmRestore.Size.Width * scale, frmRestore.Size.Height * scale)
                frmCmdBuilder.Size = New Size(frmCmdBuilder.Size.Width * scale, frmCmdBuilder.Size.Height * scale)
                frmOffline.Size = New Size(frmOffline.Size.Width * scale, frmOffline.Size.Height * scale)
            End If

        Catch ex As Exception
            Debug.WriteLine("Initializing error in GUI.")
        End Try
    End Sub

    Private Sub LanguageChange(ByVal sender As Object, ByVal e As EventArgs)
        'Cambia la lingua
        Dim item As ToolStripItem

        Try
            item = DirectCast(sender, ToolStripItem)

            CommonVariables.ChangeLanguage(item.Tag)

            RefreshDevices()

            devInfoBox.Visible = False

        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Mostra il messaggio di donazione
        If Me.OfflineBackupObj IsNot Nothing Then
            Me.OfflineBackupObj.Dispose()
            Me.OfflineBackupObj = Nothing
        End If

        If My.Settings.CheckDonate = False And Not commandMode Then
            frmDonate.ShowDialog()
        End If
    End Sub

    Public Sub CustomScaleControl(ByVal ctrl As Control, ByVal scale As Double)
        'Funzione ricorsiva
        Try
            ctrl.Top = ctrl.Top * scale
            ctrl.Left = ctrl.Left * scale
            ctrl.Size = New Size(ctrl.Size.Width * scale, ctrl.Size.Height * scale)
            'Ridimensiona il carattere e imposta lo stile come regolare
            ctrl.Font = New Font(ctrl.Font.FontFamily, ctrl.Font.Size * 0.95, FontStyle.Regular, ctrl.Font.Unit)

            'Scansiona l'albero dei controlli ridimensionando gli oggetti incontrati

            If ctrl.Controls.Count = 0 Then Return 'Nessun controllo figlio

            For Each children As Control In ctrl.Controls
                CustomScaleControl(children, scale)
            Next
        Catch ex As Exception
            Return
        End Try

    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Inizializza i controlli e il linguaggio
        Dim cmdParser As New CommandLineManager.CommandLine(Environment.CommandLine)
        CommonVariables.InitializeLanguage()
        InitializeControls()
        'Imposta la chiave di registro predefinita
        DeviceClassCollection.RegKey = My.Settings.MainRegKey

        If Utils.WindowsVersion < Utils.EnWinVersion.WXP Then
            'Sistema operativo non supportato
            MsgBox(GetLangStr("ERROR_BadSystem"), MsgBoxStyle.Critical)
            commandMode = True 'Esce dal programma
            Me.Close()
            Application.Exit()
            Return
        End If

        If cmdParser.Execute = False Then
            'Avvia l'interfaccia grafica
            'Nessun comando testuale
            commandMode = False

            RefreshDevices()

            For Each k As KeyValuePair(Of String, String) In CommonVariables.GetLanguageFiles
                Dim item As ToolStripItem = menuLang.DropDownItems.Add(k.Key & "  (" & k.Value & ")")
                item.Tag = k.Key
                AddHandler item.Click, AddressOf LanguageChange
            Next
        Else
            'Processa i comandi testuali e chiude il programma
            commandMode = True
            Me.Close()
            Application.Exit()
        End If

        'CommonVariables.GenerateLanguageFile("R:\Italiano.xml")

    End Sub


    Private Sub cmdPienaP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFullPort.Click
        If devFilter.Portability = DevicePortability.DCmp_Full Then
            'Proprietà già attivata
            cmdFullPort.FlatStyle = FlatStyle.Standard
            cmdFullPort.ForeColor = Color.Black
            devFilter.Portability = -1
            popupPortability.Checked = False
        Else
            cmdFullPort.FlatStyle = FlatStyle.Popup
            devFilter.Portability = DevicePortability.DCmp_Full
            cmdFullPort.ForeColor = Color.Blue
            popupPortability.Checked = True
        End If
        RefreshDevices()
    End Sub

    Private Sub cmdFirma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSignature.Click
        If devFilter.MustSigned Then
            'Proprietà già attivata
            cmdSignature.FlatStyle = FlatStyle.Standard
            cmdSignature.ForeColor = Color.Black
            devFilter.MustSigned = False
            popupSignature.Checked = False
        Else
            cmdSignature.FlatStyle = FlatStyle.Popup
            cmdSignature.ForeColor = Color.Blue
            devFilter.MustSigned = True
            popupSignature.Checked = True
        End If
        RefreshDevices()
    End Sub

    Private Sub cmdTerzeParti_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOthers.Click

        If devFilter.ProviderType <> DeviceFilter.DeviceFilterProviders.Prov_Others Then
            cmdOem.ForeColor = Color.Black
            cmdAll.ForeColor = Color.Black
            cmdOthers.ForeColor = Color.Blue
            cmdOem.FlatStyle = FlatStyle.Standard
            cmdAll.FlatStyle = FlatStyle.Standard
            cmdOthers.FlatStyle = FlatStyle.Popup
            popupAll.Checked = False
            popupOem.Checked = False
            popupThird.Checked = True
            devFilter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Others
        End If
        RefreshDevices()
    End Sub

    Private Sub cmdOem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOem.Click
        If devFilter.ProviderType <> DeviceFilter.DeviceFilterProviders.Prov_Oem Then
            cmdOem.ForeColor = Color.Blue
            cmdAll.ForeColor = Color.Black
            cmdOthers.ForeColor = Color.Black
            cmdOem.FlatStyle = FlatStyle.Popup
            cmdAll.FlatStyle = FlatStyle.Standard
            cmdOthers.FlatStyle = FlatStyle.Standard
            popupAll.Checked = False
            popupOem.Checked = True
            popupThird.Checked = False
            devFilter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_Oem
        End If
        RefreshDevices()
    End Sub

    Private Sub cmdAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAll.Click
        If devFilter.ProviderType <> DeviceFilter.DeviceFilterProviders.Prov_All Then
            cmdOem.ForeColor = Color.Black
            cmdAll.ForeColor = Color.Blue
            cmdOthers.ForeColor = Color.Black
            cmdOem.FlatStyle = FlatStyle.Standard
            cmdAll.FlatStyle = FlatStyle.Popup
            cmdOthers.FlatStyle = FlatStyle.Standard
            popupAll.Checked = True
            popupOem.Checked = False
            popupThird.Checked = False
            devFilter.ProviderType = DeviceFilter.DeviceFilterProviders.Prov_All
        End If
        RefreshDevices()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBackupMode.Click
        modeBackup = True
        cmdBackupMode.BackColor = Color.DarkOrange
        cmdRestoreMode.BackColor = Color.FromName("Control")
        cmdOfflineMode.BackColor = Color.FromName("Control")
        If Me.OfflineBackupObj IsNot Nothing Then
            'Torna al modo backup standard
            Me.OfflineBackupObj.Dispose()
            Me.OfflineBackupObj = Nothing
        End If
        panelBackup.Visible = True
        panelRestore.Visible = False
        lblBarInfo.Visible = False
        LoadBackupDevices()
        RefreshDevices()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestoreMode.Click
        modeBackup = False
        cmdBackupMode.BackColor = Color.FromName("Control")
        cmdRestoreMode.BackColor = Color.DarkOrange
        cmdOfflineMode.BackColor = Color.FromName("Control")
        panelBackup.Visible = False
        panelRestore.Visible = True
        lblBarInfo.Visible = True
        RefreshDevices()
    End Sub

    Private Sub cmdAggiorna_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        'ShowUnknownDevices()
        RefreshDevices()
        'LoadBackupDevices()
        'Dim f As String = InputBox("Nome del file")

        'GenerateLanguageFile(f, True)
        'Dim bo As DeviceBackupOffline = DeviceBackupOffline.Create("Z:\WINDOWS")
        'bo.LoadRegistryFile("D:\")
        'If bo Is Nothing Then
        ' MsgBox("Errore")
        ' Return
        ' End If

        '       LoadBackupDevices()

        'Dim pnp As Dictionary(Of String, Boolean)

        'pnp = DeviceRestore.EnumPnPDevices

        'For Each k As KeyValuePair(Of String, Boolean) In pnp
        'If k.Value = True Then
        'treeDevices.Nodes.Add(k.Key & "   " & k.Value.ToString)
        'End If
        'Next

        'Dim inff As DeviceInfFile
        'Dim lst As New List(Of DeviceInfFile.SupportedDevice)

        'inff = DeviceInfFile.OpenInfFile("D:\oem7.inf")

        '       inff.GetFilesFromSection("dc3000.Install")
        '
        'MsgBox(inff.GetSupportedDevices(lst))

        'For Each sp As DeviceInfFile.SupportedDevice In lst
        'treeDevices.Nodes.Add(sp.HardwareID)
        'Next

        '      inff.Dispose()

    End Sub


    Private Sub frmMain_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
        'Return
    End Sub

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Thread.Sleep(500)


        LoadBackupDevices()
    End Sub

    Private Sub treeDevices_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeDevices.AfterCheck

        If e.Node.Level = 0 Then
            'Il nodo di classe seleziona o deseleziona tutti i devices figli

            For Each thNode As TreeNode In e.Node.Nodes
                Dim dv As Device = TryCast(thNode.Tag, Device)
                If dv IsNot Nothing Then
                    dv.ExtendedInfo("Selected") = e.Node.Checked
                End If
                thNode.Checked = e.Node.Checked
            Next
            Exit Sub
        End If

        If e.Node.Level = 1 AndAlso e.Node.Tag IsNot Nothing Then
            Dim dvc As Device = TryCast(e.Node.Tag, Device)

            If dvc IsNot Nothing Then
                dvc.ExtendedInfo("Selected") = e.Node.Checked
            End If
        End If

    End Sub

    Private Sub treeDevices_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeDevices.AfterSelect

        If e.Node.Level = 1 Then
            Dim dev As Device = TryCast(e.Node.Tag, Device)
            If dev IsNot Nothing Then
                ShowDevInfo(dev)
            End If
        End If

    End Sub


    Private Sub treeDevices_BeforeCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles treeDevices.BeforeCheck
        If e.Node.Level > 1 Then e.Cancel = True 'Non è possibile selezionare i files

    End Sub

    Private Sub cmdBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBackup.Click

        frmBackup.DeviceList = DeviceCollection.Create(Me.m_currDevices, "Selected", True)
        frmBackup.LoadControls(Me.ImageList1)
        frmBackup.ShowDialog()


    End Sub


    Private Sub cmdApriBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpenBkFile.Click

        Dim fOpen As OpenFileDialog
        Try

            fOpen = New OpenFileDialog
            fOpen.CheckFileExists = True
            fOpen.Filter = [String].Format("Backup files (*{0})|*{0}|All Files (*.*)|*.*", My.Settings.StdBackupInfoExt)
            If fOpen.ShowDialog = Windows.Forms.DialogResult.OK Then
                devRestorer = DeviceRestore.Create(fOpen.FileName)
                If devRestorer Is Nothing Then
                    'Errore nell'apertura del file
                    MsgBox(GetLangStr("ERROR:FileOpen"), MsgBoxStyle.Exclamation)
                    lblBarInfo.Text = GetLangStr("ERROR:FileOpen")
                Else
                    Me.m_RestoreDevs = devRestorer.DeviceList
                    lblBarInfo.Text = fOpen.FileName
                    RefreshDevices()
                End If
            End If
            fOpen.Dispose()
        Catch ex As Exception

        End Try


    End Sub

    Private Sub cmdBkExtInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBkExtInfo.Click
        ShowPckInfo()
    End Sub

    Private Sub frmMain_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        'Me.Size = Me.fixedSize
    End Sub

    Private Sub cmdRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestore.Click

        If devRestorer Is Nothing Then Return
        frmRestore.RestorerObj = devRestorer
        frmRestore.InitializeControls(Me.ImageList1)
        frmRestore.Show()
        frmRestore.ShowDevices()

    End Sub



    Private Sub EsciDaDriverBackupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EsciDaDriverBackupToolStripMenuItem.Click
        Me.Close()
    End Sub


    Private Sub treeDevices_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles treeDevices.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            'Mostra il menu
            popupMenu.Show(treeDevices, e.Location)

        End If
    End Sub

    Private Sub SelezionaTuttoToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popupSelectAll.Click
        If treeDevices Is Nothing Then Return

        For Each nd As TreeNode In treeDevices.Nodes
            nd.Checked = True
        Next
    End Sub

    Private Sub AnnullaSelezioneToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popupCancelSelect.Click
        If treeDevices Is Nothing Then Return

        For Each nd As TreeNode In treeDevices.Nodes
            nd.Checked = False
        Next

    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        treeDevices.ExpandAll()
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        treeDevices.CollapseAll()
    End Sub

    Private Sub BuilderRigaDiComandoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuilderRigaDiComandoToolStripMenuItem.Click
        frmCmdBuilder.Show()
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        CommonVariables.OpenHelpGuide(HelpGuideSection.index)
    End Sub

    Private Sub GuidaAlBackupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuidaAlBackupToolStripMenuItem.Click
        CommonVariables.OpenHelpGuide(HelpGuideSection.Backup)
    End Sub

    Private Sub GuidaAlRipristinoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuidaAlRipristinoToolStripMenuItem.Click
        CommonVariables.OpenHelpGuide(HelpGuideSection.Restore)
    End Sub

    Private Sub GuidaAllaRigaDiComandoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuidaAllaRigaDiComandoToolStripMenuItem.Click
        CommonVariables.OpenHelpGuide(HelpGuideSection.CommandLine)
    End Sub

    Private Sub InfoSuDriverBackupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InfoSuDriverBackupToolStripMenuItem.Click
        CommonVariables.OpenHelpGuide(HelpGuideSection.Info)
    End Sub

    Private Sub MostraDispositiviPCIToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MostraDispositiviPCIToolStripMenuItem.Click
        ShowUnknownDevices()
    End Sub

    Private Sub cmdOfflineMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOfflineMode.Click
        modeBackup = True
        cmdBackupMode.BackColor = Color.FromName("Control")
        cmdRestoreMode.BackColor = Color.FromName("Control")
        cmdOfflineMode.BackColor = Color.DarkOrange
        panelBackup.Visible = True
        panelRestore.Visible = False
        lblBarInfo.Visible = False

        frmOffline.ShowDialog()
    End Sub
End Class


