<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.EsciDaDriverBackupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuLang = New System.Windows.Forms.ToolStripMenuItem()
        Me.UtilitàToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BuilderRigaDiComandoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MostraDispositiviPCIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GuidaAlBackupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GuidaAlRipristinoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GuidaAllaRigaDiComandoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InfoSuDriverBackupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblBarInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.devScanBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.devInfoBox = New System.Windows.Forms.GroupBox()
        Me.pictDev = New System.Windows.Forms.PictureBox()
        Me.lblFiles = New System.Windows.Forms.Label()
        Me.lblOpSystem = New System.Windows.Forms.Label()
        Me.lblDevPort = New System.Windows.Forms.Label()
        Me.lblDevTipo = New System.Windows.Forms.Label()
        Me.lblID = New System.Windows.Forms.Label()
        Me.lblData = New System.Windows.Forms.Label()
        Me.lblFirma = New System.Windows.Forms.Label()
        Me.lblFileINF = New System.Windows.Forms.Label()
        Me.lblProduttore = New System.Windows.Forms.Label()
        Me.lblVersione = New System.Windows.Forms.Label()
        Me.lblPortRest = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.treeDevices = New System.Windows.Forms.TreeView()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.cmdBackupMode = New System.Windows.Forms.ToolStripButton()
        Me.cmdRestoreMode = New System.Windows.Forms.ToolStripButton()
        Me.lblDispTrovati = New System.Windows.Forms.ToolStripLabel()
        Me.cmdOfflineMode = New System.Windows.Forms.ToolStripButton()
        Me.panelBackup = New System.Windows.Forms.Panel()
        Me.cmdUpdate = New System.Windows.Forms.Button()
        Me.cmdBackup = New System.Windows.Forms.Button()
        Me.panelRestore = New System.Windows.Forms.Panel()
        Me.cmdBkExtInfo = New System.Windows.Forms.Button()
        Me.cmdOpenBkFile = New System.Windows.Forms.Button()
        Me.cmdRestore = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmdOthers = New System.Windows.Forms.Button()
        Me.cmdOem = New System.Windows.Forms.Button()
        Me.cmdAll = New System.Windows.Forms.Button()
        Me.pckInfoBox = New System.Windows.Forms.GroupBox()
        Me.lblPckInfo = New System.Windows.Forms.Label()
        Me.pictPck = New System.Windows.Forms.PictureBox()
        Me.lblPckSysVersion = New System.Windows.Forms.Label()
        Me.lblPckDrvVersion = New System.Windows.Forms.Label()
        Me.lblPckDate = New System.Windows.Forms.Label()
        Me.lblPckSystem = New System.Windows.Forms.Label()
        Me.lblPckComputerName = New System.Windows.Forms.Label()
        Me.lblPckDesc = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmdFullPort = New System.Windows.Forms.Button()
        Me.cmdSignature = New System.Windows.Forms.Button()
        Me.popupMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.popupSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.popupCancelSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.popupAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.popupOem = New System.Windows.Forms.ToolStripMenuItem()
        Me.popupThird = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.popupSignature = New System.Windows.Forms.ToolStripMenuItem()
        Me.popupPortability = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.devInfoBox.SuspendLayout()
        CType(Me.pictDev, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.panelBackup.SuspendLayout()
        Me.panelRestore.SuspendLayout()
        Me.pckInfoBox.SuspendLayout()
        CType(Me.pictPck, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.popupMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "empty.ico")
        Me.ImageList1.Images.SetKeyName(1, "Icon_110.ico")
        Me.ImageList1.Images.SetKeyName(2, "icoWarning")
        Me.ImageList1.Images.SetKeyName(3, "icoError2")
        Me.ImageList1.Images.SetKeyName(4, "icoRestore2")
        Me.ImageList1.Images.SetKeyName(5, "icoFrmRestore")
        Me.ImageList1.Images.SetKeyName(6, "icoInfo")
        Me.ImageList1.Images.SetKeyName(7, "fileNO")
        Me.ImageList1.Images.SetKeyName(8, "fileOK")
        Me.ImageList1.Images.SetKeyName(9, "icoSaveAll")
        Me.ImageList1.Images.SetKeyName(10, "icoRefresh")
        Me.ImageList1.Images.SetKeyName(11, "icoOpen")
        Me.ImageList1.Images.SetKeyName(12, "icoRedo")
        Me.ImageList1.Images.SetKeyName(13, "provAll2")
        Me.ImageList1.Images.SetKeyName(14, "provOEM")
        Me.ImageList1.Images.SetKeyName(15, "provOthers")
        Me.ImageList1.Images.SetKeyName(16, "cmdPort2")
        Me.ImageList1.Images.SetKeyName(17, "icoDelete")
        Me.ImageList1.Images.SetKeyName(18, "icoSave")
        Me.ImageList1.Images.SetKeyName(19, "provAll")
        Me.ImageList1.Images.SetKeyName(20, "cmdSignature")
        Me.ImageList1.Images.SetKeyName(21, "cmdPort")
        Me.ImageList1.Images.SetKeyName(22, "Search.ico")
        Me.ImageList1.Images.SetKeyName(23, "icoRestore")
        Me.ImageList1.Images.SetKeyName(24, "icoError")
        Me.ImageList1.Images.SetKeyName(25, "icoRestoreClass")
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EsciDaDriverBackupToolStripMenuItem, Me.menuLang, Me.UtilitàToolStripMenuItem, Me.ToolStripMenuItem1})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1308, 24)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'EsciDaDriverBackupToolStripMenuItem
        '
        Me.EsciDaDriverBackupToolStripMenuItem.Name = "EsciDaDriverBackupToolStripMenuItem"
        Me.EsciDaDriverBackupToolStripMenuItem.Size = New System.Drawing.Size(131, 20)
        Me.EsciDaDriverBackupToolStripMenuItem.Text = "Esci da DriverBackup!"
        '
        'menuLang
        '
        Me.menuLang.Name = "menuLang"
        Me.menuLang.Size = New System.Drawing.Size(55, 20)
        Me.menuLang.Text = "Lingua"
        '
        'UtilitàToolStripMenuItem
        '
        Me.UtilitàToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BuilderRigaDiComandoToolStripMenuItem, Me.MostraDispositiviPCIToolStripMenuItem})
        Me.UtilitàToolStripMenuItem.Name = "UtilitàToolStripMenuItem"
        Me.UtilitàToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.UtilitàToolStripMenuItem.Text = "Utilità"
        '
        'BuilderRigaDiComandoToolStripMenuItem
        '
        Me.BuilderRigaDiComandoToolStripMenuItem.Name = "BuilderRigaDiComandoToolStripMenuItem"
        Me.BuilderRigaDiComandoToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.BuilderRigaDiComandoToolStripMenuItem.Text = "Builder riga di comando"
        '
        'MostraDispositiviPCIToolStripMenuItem
        '
        Me.MostraDispositiviPCIToolStripMenuItem.Name = "MostraDispositiviPCIToolStripMenuItem"
        Me.MostraDispositiviPCIToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.MostraDispositiviPCIToolStripMenuItem.Text = "Mostra dispositivi PCI"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem4, Me.GuidaAlBackupToolStripMenuItem, Me.GuidaAlRipristinoToolStripMenuItem, Me.GuidaAllaRigaDiComandoToolStripMenuItem, Me.InfoSuDriverBackupToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(24, 20)
        Me.ToolStripMenuItem1.Text = "&?"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(216, 22)
        Me.ToolStripMenuItem4.Text = "Guida"
        '
        'GuidaAlBackupToolStripMenuItem
        '
        Me.GuidaAlBackupToolStripMenuItem.Name = "GuidaAlBackupToolStripMenuItem"
        Me.GuidaAlBackupToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.GuidaAlBackupToolStripMenuItem.Text = "Guida al backup"
        '
        'GuidaAlRipristinoToolStripMenuItem
        '
        Me.GuidaAlRipristinoToolStripMenuItem.Name = "GuidaAlRipristinoToolStripMenuItem"
        Me.GuidaAlRipristinoToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.GuidaAlRipristinoToolStripMenuItem.Text = "Guida al ripristino"
        '
        'GuidaAllaRigaDiComandoToolStripMenuItem
        '
        Me.GuidaAllaRigaDiComandoToolStripMenuItem.Name = "GuidaAllaRigaDiComandoToolStripMenuItem"
        Me.GuidaAllaRigaDiComandoToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.GuidaAllaRigaDiComandoToolStripMenuItem.Text = "Guida alla riga di comando"
        '
        'InfoSuDriverBackupToolStripMenuItem
        '
        Me.InfoSuDriverBackupToolStripMenuItem.Name = "InfoSuDriverBackupToolStripMenuItem"
        Me.InfoSuDriverBackupToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.InfoSuDriverBackupToolStripMenuItem.Text = "Info su DriverBackup!"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblBarInfo, Me.devScanBar})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 606)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1308, 22)
        Me.StatusStrip1.TabIndex = 7
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblBarInfo
        '
        Me.lblBarInfo.AutoSize = False
        Me.lblBarInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBarInfo.Name = "lblBarInfo"
        Me.lblBarInfo.Size = New System.Drawing.Size(250, 17)
        Me.lblBarInfo.Visible = False
        '
        'devScanBar
        '
        Me.devScanBar.Name = "devScanBar"
        Me.devScanBar.Size = New System.Drawing.Size(350, 16)
        Me.devScanBar.Visible = False
        '
        'devInfoBox
        '
        Me.devInfoBox.Controls.Add(Me.pictDev)
        Me.devInfoBox.Controls.Add(Me.lblFiles)
        Me.devInfoBox.Controls.Add(Me.lblOpSystem)
        Me.devInfoBox.Controls.Add(Me.lblDevPort)
        Me.devInfoBox.Controls.Add(Me.lblDevTipo)
        Me.devInfoBox.Controls.Add(Me.lblID)
        Me.devInfoBox.Controls.Add(Me.lblData)
        Me.devInfoBox.Controls.Add(Me.lblFirma)
        Me.devInfoBox.Controls.Add(Me.lblFileINF)
        Me.devInfoBox.Controls.Add(Me.lblProduttore)
        Me.devInfoBox.Controls.Add(Me.lblVersione)
        Me.devInfoBox.Controls.Add(Me.lblPortRest)
        Me.devInfoBox.Controls.Add(Me.Label7)
        Me.devInfoBox.Controls.Add(Me.Label6)
        Me.devInfoBox.Controls.Add(Me.Label5)
        Me.devInfoBox.Controls.Add(Me.Label4)
        Me.devInfoBox.Controls.Add(Me.Label3)
        Me.devInfoBox.Controls.Add(Me.Label2)
        Me.devInfoBox.Controls.Add(Me.Label1)
        Me.devInfoBox.Location = New System.Drawing.Point(24, 429)
        Me.devInfoBox.Name = "devInfoBox"
        Me.devInfoBox.Size = New System.Drawing.Size(561, 162)
        Me.devInfoBox.TabIndex = 22
        Me.devInfoBox.TabStop = False
        Me.devInfoBox.Text = "Informazioni sul device selezionato"
        Me.devInfoBox.Visible = False
        '
        'pictDev
        '
        Me.pictDev.Location = New System.Drawing.Point(25, 115)
        Me.pictDev.Name = "pictDev"
        Me.pictDev.Size = New System.Drawing.Size(33, 28)
        Me.pictDev.TabIndex = 18
        Me.pictDev.TabStop = False
        '
        'lblFiles
        '
        Me.lblFiles.AutoSize = True
        Me.lblFiles.Location = New System.Drawing.Point(65, 138)
        Me.lblFiles.Name = "lblFiles"
        Me.lblFiles.Size = New System.Drawing.Size(45, 13)
        Me.lblFiles.TabIndex = 17
        Me.lblFiles.Text = "Label12"
        '
        'lblOpSystem
        '
        Me.lblOpSystem.AutoSize = True
        Me.lblOpSystem.Location = New System.Drawing.Point(65, 116)
        Me.lblOpSystem.Name = "lblOpSystem"
        Me.lblOpSystem.Size = New System.Drawing.Size(39, 13)
        Me.lblOpSystem.TabIndex = 16
        Me.lblOpSystem.Text = "Label8"
        '
        'lblDevPort
        '
        Me.lblDevPort.AutoSize = True
        Me.lblDevPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDevPort.ForeColor = System.Drawing.Color.Black
        Me.lblDevPort.Location = New System.Drawing.Point(369, 90)
        Me.lblDevPort.Name = "lblDevPort"
        Me.lblDevPort.Size = New System.Drawing.Size(45, 13)
        Me.lblDevPort.TabIndex = 15
        Me.lblDevPort.Text = "Label16"
        Me.lblDevPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDevTipo
        '
        Me.lblDevTipo.AutoSize = True
        Me.lblDevTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDevTipo.ForeColor = System.Drawing.Color.Black
        Me.lblDevTipo.Location = New System.Drawing.Point(369, 70)
        Me.lblDevTipo.Name = "lblDevTipo"
        Me.lblDevTipo.Size = New System.Drawing.Size(45, 13)
        Me.lblDevTipo.TabIndex = 14
        Me.lblDevTipo.Text = "Label15"
        '
        'lblID
        '
        Me.lblID.AutoSize = True
        Me.lblID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblID.ForeColor = System.Drawing.Color.Black
        Me.lblID.Location = New System.Drawing.Point(369, 50)
        Me.lblID.Name = "lblID"
        Me.lblID.Size = New System.Drawing.Size(45, 13)
        Me.lblID.TabIndex = 13
        Me.lblID.Text = "Label14"
        '
        'lblData
        '
        Me.lblData.AutoSize = True
        Me.lblData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData.ForeColor = System.Drawing.Color.Black
        Me.lblData.Location = New System.Drawing.Point(369, 26)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(45, 13)
        Me.lblData.TabIndex = 12
        Me.lblData.Text = "Label13"
        '
        'lblFirma
        '
        Me.lblFirma.AutoSize = True
        Me.lblFirma.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirma.ForeColor = System.Drawing.Color.Black
        Me.lblFirma.Location = New System.Drawing.Point(124, 90)
        Me.lblFirma.Name = "lblFirma"
        Me.lblFirma.Size = New System.Drawing.Size(45, 13)
        Me.lblFirma.TabIndex = 11
        Me.lblFirma.Text = "Label12"
        '
        'lblFileINF
        '
        Me.lblFileINF.AutoSize = True
        Me.lblFileINF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileINF.ForeColor = System.Drawing.Color.Black
        Me.lblFileINF.Location = New System.Drawing.Point(124, 70)
        Me.lblFileINF.Name = "lblFileINF"
        Me.lblFileINF.Size = New System.Drawing.Size(45, 13)
        Me.lblFileINF.TabIndex = 10
        Me.lblFileINF.Text = "Label11"
        '
        'lblProduttore
        '
        Me.lblProduttore.AutoSize = True
        Me.lblProduttore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduttore.ForeColor = System.Drawing.Color.Black
        Me.lblProduttore.Location = New System.Drawing.Point(124, 50)
        Me.lblProduttore.Name = "lblProduttore"
        Me.lblProduttore.Size = New System.Drawing.Size(45, 13)
        Me.lblProduttore.TabIndex = 9
        Me.lblProduttore.Text = "Label10"
        '
        'lblVersione
        '
        Me.lblVersione.AutoSize = True
        Me.lblVersione.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersione.ForeColor = System.Drawing.Color.Black
        Me.lblVersione.Location = New System.Drawing.Point(124, 30)
        Me.lblVersione.Name = "lblVersione"
        Me.lblVersione.Size = New System.Drawing.Size(39, 13)
        Me.lblVersione.TabIndex = 8
        Me.lblVersione.Text = "Label9"
        '
        'lblPortRest
        '
        Me.lblPortRest.AutoSize = True
        Me.lblPortRest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPortRest.Location = New System.Drawing.Point(268, 90)
        Me.lblPortRest.Name = "lblPortRest"
        Me.lblPortRest.Size = New System.Drawing.Size(64, 13)
        Me.lblPortRest.TabIndex = 7
        Me.lblPortRest.Text = "Portabilità"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(22, 90)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Firma digitale"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(268, 70)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Tipo"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(266, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Identificatore"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(266, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Data di rilascio"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(22, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(101, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "File installazione"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(22, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Produttore"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Versione"
        '
        'treeDevices
        '
        Me.treeDevices.CheckBoxes = True
        Me.treeDevices.ImageIndex = 0
        Me.treeDevices.ImageList = Me.ImageList1
        Me.treeDevices.Location = New System.Drawing.Point(25, 104)
        Me.treeDevices.Name = "treeDevices"
        Me.treeDevices.SelectedImageIndex = 0
        Me.treeDevices.Size = New System.Drawing.Size(704, 322)
        Me.treeDevices.TabIndex = 18
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1, Me.cmdBackupMode, Me.cmdRestoreMode, Me.lblDispTrovati, Me.cmdOfflineMode})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1308, 25)
        Me.ToolStrip1.TabIndex = 25
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(190, 22)
        Me.ToolStripLabel1.Text = "Selezionare l'operazione desiderata"
        '
        'cmdBackupMode
        '
        Me.cmdBackupMode.BackColor = System.Drawing.Color.DarkOrange
        Me.cmdBackupMode.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdBackupMode.Name = "cmdBackupMode"
        Me.cmdBackupMode.Size = New System.Drawing.Size(50, 22)
        Me.cmdBackupMode.Text = "Backup"
        Me.cmdBackupMode.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'cmdRestoreMode
        '
        Me.cmdRestoreMode.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdRestoreMode.Name = "cmdRestoreMode"
        Me.cmdRestoreMode.Size = New System.Drawing.Size(61, 22)
        Me.cmdRestoreMode.Text = "Ripristino"
        '
        'lblDispTrovati
        '
        Me.lblDispTrovati.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.lblDispTrovati.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDispTrovati.Name = "lblDispTrovati"
        Me.lblDispTrovati.Size = New System.Drawing.Size(105, 22)
        Me.lblDispTrovati.Text = "Dispositivi trovati"
        Me.lblDispTrovati.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdOfflineMode
        '
        Me.cmdOfflineMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.cmdOfflineMode.Image = CType(resources.GetObject("cmdOfflineMode.Image"), System.Drawing.Image)
        Me.cmdOfflineMode.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdOfflineMode.Name = "cmdOfflineMode"
        Me.cmdOfflineMode.Size = New System.Drawing.Size(146, 22)
        Me.cmdOfflineMode.Text = "Backup da sistema offline"
        '
        'panelBackup
        '
        Me.panelBackup.Controls.Add(Me.cmdUpdate)
        Me.panelBackup.Controls.Add(Me.cmdBackup)
        Me.panelBackup.Location = New System.Drawing.Point(591, 429)
        Me.panelBackup.Name = "panelBackup"
        Me.panelBackup.Size = New System.Drawing.Size(135, 137)
        Me.panelBackup.TabIndex = 26
        '
        'cmdUpdate
        '
        Me.cmdUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdUpdate.Location = New System.Drawing.Point(4, 44)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(127, 35)
        Me.cmdUpdate.TabIndex = 1
        Me.cmdUpdate.Text = "Aggiorna"
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'cmdBackup
        '
        Me.cmdBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdBackup.Location = New System.Drawing.Point(3, 3)
        Me.cmdBackup.Name = "cmdBackup"
        Me.cmdBackup.Size = New System.Drawing.Size(129, 35)
        Me.cmdBackup.TabIndex = 0
        Me.cmdBackup.Text = "Avvia backup"
        Me.cmdBackup.UseVisualStyleBackColor = True
        '
        'panelRestore
        '
        Me.panelRestore.Controls.Add(Me.cmdBkExtInfo)
        Me.panelRestore.Controls.Add(Me.cmdOpenBkFile)
        Me.panelRestore.Controls.Add(Me.cmdRestore)
        Me.panelRestore.Location = New System.Drawing.Point(735, 470)
        Me.panelRestore.Name = "panelRestore"
        Me.panelRestore.Size = New System.Drawing.Size(134, 136)
        Me.panelRestore.TabIndex = 27
        '
        'cmdBkExtInfo
        '
        Me.cmdBkExtInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdBkExtInfo.Location = New System.Drawing.Point(3, 86)
        Me.cmdBkExtInfo.Name = "cmdBkExtInfo"
        Me.cmdBkExtInfo.Size = New System.Drawing.Size(129, 33)
        Me.cmdBkExtInfo.TabIndex = 2
        Me.cmdBkExtInfo.Text = "Informazioni backup"
        Me.cmdBkExtInfo.UseVisualStyleBackColor = True
        '
        'cmdOpenBkFile
        '
        Me.cmdOpenBkFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOpenBkFile.Location = New System.Drawing.Point(3, 44)
        Me.cmdOpenBkFile.Name = "cmdOpenBkFile"
        Me.cmdOpenBkFile.Size = New System.Drawing.Size(131, 36)
        Me.cmdOpenBkFile.TabIndex = 1
        Me.cmdOpenBkFile.Text = "Apri file backup"
        Me.cmdOpenBkFile.UseVisualStyleBackColor = True
        '
        'cmdRestore
        '
        Me.cmdRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdRestore.Location = New System.Drawing.Point(3, 3)
        Me.cmdRestore.Name = "cmdRestore"
        Me.cmdRestore.Size = New System.Drawing.Size(130, 35)
        Me.cmdRestore.TabIndex = 0
        Me.cmdRestore.Text = "Ripristina"
        Me.cmdRestore.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(22, 441)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(359, 15)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Fare clic su un device per visualizzarne le informazioni"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label10.Location = New System.Drawing.Point(119, 473)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(381, 55)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "DriverBackup 2!"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(424, 553)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(115, 13)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "by Giuseppe Greco"
        '
        'cmdOthers
        '
        Me.cmdOthers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOthers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOthers.Location = New System.Drawing.Point(257, 62)
        Me.cmdOthers.Name = "cmdOthers"
        Me.cmdOthers.Size = New System.Drawing.Size(113, 23)
        Me.cmdOthers.TabIndex = 21
        Me.cmdOthers.Text = "Terze parti"
        Me.cmdOthers.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdOthers.UseVisualStyleBackColor = True
        '
        'cmdOem
        '
        Me.cmdOem.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOem.Location = New System.Drawing.Point(137, 62)
        Me.cmdOem.Name = "cmdOem"
        Me.cmdOem.Size = New System.Drawing.Size(113, 23)
        Me.cmdOem.TabIndex = 20
        Me.cmdOem.Text = "Oem"
        Me.cmdOem.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdOem.UseVisualStyleBackColor = True
        '
        'cmdAll
        '
        Me.cmdAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAll.ForeColor = System.Drawing.Color.Blue
        Me.cmdAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAll.Location = New System.Drawing.Point(22, 62)
        Me.cmdAll.Name = "cmdAll"
        Me.cmdAll.Size = New System.Drawing.Size(109, 23)
        Me.cmdAll.TabIndex = 19
        Me.cmdAll.Text = "Tutti"
        Me.cmdAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdAll.UseVisualStyleBackColor = True
        '
        'pckInfoBox
        '
        Me.pckInfoBox.Controls.Add(Me.lblPckInfo)
        Me.pckInfoBox.Controls.Add(Me.pictPck)
        Me.pckInfoBox.Controls.Add(Me.lblPckSysVersion)
        Me.pckInfoBox.Controls.Add(Me.lblPckDrvVersion)
        Me.pckInfoBox.Controls.Add(Me.lblPckDate)
        Me.pckInfoBox.Controls.Add(Me.lblPckSystem)
        Me.pckInfoBox.Controls.Add(Me.lblPckComputerName)
        Me.pckInfoBox.Controls.Add(Me.lblPckDesc)
        Me.pckInfoBox.Controls.Add(Me.Label16)
        Me.pckInfoBox.Controls.Add(Me.Label15)
        Me.pckInfoBox.Controls.Add(Me.Label14)
        Me.pckInfoBox.Controls.Add(Me.Label13)
        Me.pckInfoBox.Controls.Add(Me.Label12)
        Me.pckInfoBox.Controls.Add(Me.Label8)
        Me.pckInfoBox.Location = New System.Drawing.Point(747, 286)
        Me.pckInfoBox.Name = "pckInfoBox"
        Me.pckInfoBox.Size = New System.Drawing.Size(561, 161)
        Me.pckInfoBox.TabIndex = 31
        Me.pckInfoBox.TabStop = False
        Me.pckInfoBox.Text = "Informazioni estese backup"
        '
        'lblPckInfo
        '
        Me.lblPckInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPckInfo.Location = New System.Drawing.Point(71, 123)
        Me.lblPckInfo.Name = "lblPckInfo"
        Me.lblPckInfo.Size = New System.Drawing.Size(460, 28)
        Me.lblPckInfo.TabIndex = 13
        Me.lblPckInfo.Text = "Label17"
        '
        'pictPck
        '
        Me.pictPck.Location = New System.Drawing.Point(23, 122)
        Me.pictPck.Name = "pictPck"
        Me.pictPck.Size = New System.Drawing.Size(32, 28)
        Me.pictPck.TabIndex = 12
        Me.pictPck.TabStop = False
        '
        'lblPckSysVersion
        '
        Me.lblPckSysVersion.AutoSize = True
        Me.lblPckSysVersion.Location = New System.Drawing.Point(458, 50)
        Me.lblPckSysVersion.Name = "lblPckSysVersion"
        Me.lblPckSysVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblPckSysVersion.TabIndex = 11
        Me.lblPckSysVersion.Text = "Label22"
        '
        'lblPckDrvVersion
        '
        Me.lblPckDrvVersion.AutoSize = True
        Me.lblPckDrvVersion.Location = New System.Drawing.Point(458, 30)
        Me.lblPckDrvVersion.Name = "lblPckDrvVersion"
        Me.lblPckDrvVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblPckDrvVersion.TabIndex = 10
        Me.lblPckDrvVersion.Text = "Label21"
        '
        'lblPckDate
        '
        Me.lblPckDate.AutoSize = True
        Me.lblPckDate.Location = New System.Drawing.Point(133, 90)
        Me.lblPckDate.Name = "lblPckDate"
        Me.lblPckDate.Size = New System.Drawing.Size(45, 13)
        Me.lblPckDate.TabIndex = 9
        Me.lblPckDate.Text = "Label20"
        '
        'lblPckSystem
        '
        Me.lblPckSystem.AutoSize = True
        Me.lblPckSystem.Location = New System.Drawing.Point(133, 70)
        Me.lblPckSystem.Name = "lblPckSystem"
        Me.lblPckSystem.Size = New System.Drawing.Size(45, 13)
        Me.lblPckSystem.TabIndex = 8
        Me.lblPckSystem.Text = "Label19"
        '
        'lblPckComputerName
        '
        Me.lblPckComputerName.AutoSize = True
        Me.lblPckComputerName.Location = New System.Drawing.Point(133, 50)
        Me.lblPckComputerName.Name = "lblPckComputerName"
        Me.lblPckComputerName.Size = New System.Drawing.Size(45, 13)
        Me.lblPckComputerName.TabIndex = 7
        Me.lblPckComputerName.Text = "Label18"
        '
        'lblPckDesc
        '
        Me.lblPckDesc.AutoSize = True
        Me.lblPckDesc.Location = New System.Drawing.Point(133, 30)
        Me.lblPckDesc.Name = "lblPckDesc"
        Me.lblPckDesc.Size = New System.Drawing.Size(45, 13)
        Me.lblPckDesc.TabIndex = 6
        Me.lblPckDesc.Text = "Label17"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(20, 30)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(73, 13)
        Me.Label16.TabIndex = 5
        Me.Label16.Text = "Descrizione"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(301, 30)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(141, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "Versione DriverBackup!"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(301, 50)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(102, 13)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "Versione sistema"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(20, 90)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(107, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Data di creazione"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(20, 50)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(95, 13)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Nome computer"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(20, 70)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Sistema di origine"
        '
        'cmdFullPort
        '
        Me.cmdFullPort.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFullPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFullPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFullPort.Location = New System.Drawing.Point(479, 62)
        Me.cmdFullPort.Name = "cmdFullPort"
        Me.cmdFullPort.Size = New System.Drawing.Size(122, 23)
        Me.cmdFullPort.TabIndex = 24
        Me.cmdFullPort.Text = "Piena portabilità"
        Me.cmdFullPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFullPort.UseVisualStyleBackColor = False
        '
        'cmdSignature
        '
        Me.cmdSignature.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSignature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSignature.Location = New System.Drawing.Point(607, 62)
        Me.cmdSignature.Name = "cmdSignature"
        Me.cmdSignature.Size = New System.Drawing.Size(122, 23)
        Me.cmdSignature.TabIndex = 23
        Me.cmdSignature.Text = "Firma digitale"
        Me.cmdSignature.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSignature.UseVisualStyleBackColor = True
        '
        'popupMenu
        '
        Me.popupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.popupSelectAll, Me.popupCancelSelect, Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripSeparator3, Me.popupAll, Me.popupOem, Me.popupThird, Me.ToolStripSeparator4, Me.popupSignature, Me.popupPortability})
        Me.popupMenu.Name = "popupMenu"
        Me.popupMenu.Size = New System.Drawing.Size(167, 214)
        '
        'popupSelectAll
        '
        Me.popupSelectAll.Name = "popupSelectAll"
        Me.popupSelectAll.Size = New System.Drawing.Size(166, 22)
        Me.popupSelectAll.Text = "Seleziona tutto"
        '
        'popupCancelSelect
        '
        Me.popupCancelSelect.Name = "popupCancelSelect"
        Me.popupCancelSelect.Size = New System.Drawing.Size(166, 22)
        Me.popupCancelSelect.Text = "Annulla selezione"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(166, 22)
        Me.ToolStripMenuItem2.Text = "Comprimi albero"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(166, 22)
        Me.ToolStripMenuItem3.Text = "Espandi albero"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(163, 6)
        '
        'popupAll
        '
        Me.popupAll.Checked = True
        Me.popupAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.popupAll.Name = "popupAll"
        Me.popupAll.Size = New System.Drawing.Size(166, 22)
        Me.popupAll.Text = "Tutti"
        '
        'popupOem
        '
        Me.popupOem.Checked = True
        Me.popupOem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.popupOem.Name = "popupOem"
        Me.popupOem.Size = New System.Drawing.Size(166, 22)
        Me.popupOem.Text = "Oem (Inclusi)"
        '
        'popupThird
        '
        Me.popupThird.Checked = True
        Me.popupThird.CheckState = System.Windows.Forms.CheckState.Checked
        Me.popupThird.Name = "popupThird"
        Me.popupThird.Size = New System.Drawing.Size(166, 22)
        Me.popupThird.Text = "Terze parti"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(163, 6)
        '
        'popupSignature
        '
        Me.popupSignature.Checked = True
        Me.popupSignature.CheckState = System.Windows.Forms.CheckState.Checked
        Me.popupSignature.Name = "popupSignature"
        Me.popupSignature.Size = New System.Drawing.Size(166, 22)
        Me.popupSignature.Text = "Firma digitale"
        '
        'popupPortability
        '
        Me.popupPortability.Checked = True
        Me.popupPortability.CheckState = System.Windows.Forms.CheckState.Checked
        Me.popupPortability.Name = "popupPortability"
        Me.popupPortability.Size = New System.Drawing.Size(166, 22)
        Me.popupPortability.Text = "Piena portabilità"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(770, 641)
        Me.Controls.Add(Me.pckInfoBox)
        Me.Controls.Add(Me.panelBackup)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.panelRestore)
        Me.Controls.Add(Me.devInfoBox)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.treeDevices)
        Me.Controls.Add(Me.cmdFullPort)
        Me.Controls.Add(Me.cmdSignature)
        Me.Controls.Add(Me.cmdOthers)
        Me.Controls.Add(Me.cmdOem)
        Me.Controls.Add(Me.cmdAll)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label11)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DriverBackup! 2.0.0"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.devInfoBox.ResumeLayout(False)
        Me.devInfoBox.PerformLayout()
        CType(Me.pictDev, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.panelBackup.ResumeLayout(False)
        Me.panelRestore.ResumeLayout(False)
        Me.pckInfoBox.ResumeLayout(False)
        Me.pckInfoBox.PerformLayout()
        CType(Me.pictPck, System.ComponentModel.ISupportInitialize).EndInit()
        Me.popupMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents devInfoBox As System.Windows.Forms.GroupBox
    Friend WithEvents lblDevPort As System.Windows.Forms.Label
    Friend WithEvents lblDevTipo As System.Windows.Forms.Label
    Friend WithEvents lblID As System.Windows.Forms.Label
    Friend WithEvents lblData As System.Windows.Forms.Label
    Friend WithEvents lblFirma As System.Windows.Forms.Label
    Friend WithEvents lblFileINF As System.Windows.Forms.Label
    Friend WithEvents lblProduttore As System.Windows.Forms.Label
    Friend WithEvents lblVersione As System.Windows.Forms.Label
    Friend WithEvents lblPortRest As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdOthers As System.Windows.Forms.Button
    Friend WithEvents cmdOem As System.Windows.Forms.Button
    Friend WithEvents cmdAll As System.Windows.Forms.Button
    Friend WithEvents treeDevices As System.Windows.Forms.TreeView
    Friend WithEvents cmdSignature As System.Windows.Forms.Button
    Friend WithEvents cmdFullPort As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cmdBackupMode As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmdRestoreMode As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblDispTrovati As System.Windows.Forms.ToolStripLabel
    Friend WithEvents panelBackup As System.Windows.Forms.Panel
    Friend WithEvents panelRestore As System.Windows.Forms.Panel
    Friend WithEvents cmdOpenBkFile As System.Windows.Forms.Button
    Friend WithEvents cmdRestore As System.Windows.Forms.Button
    Friend WithEvents cmdBackup As System.Windows.Forms.Button
    Friend WithEvents cmdUpdate As System.Windows.Forms.Button
    Friend WithEvents lblBarInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents devScanBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblOpSystem As System.Windows.Forms.Label
    Friend WithEvents lblFiles As System.Windows.Forms.Label
    Friend WithEvents pictDev As System.Windows.Forms.PictureBox
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EsciDaDriverBackupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GuidaAlBackupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GuidaAlRipristinoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GuidaAllaRigaDiComandoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InfoSuDriverBackupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UtilitàToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuilderRigaDiComandoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdBkExtInfo As System.Windows.Forms.Button
    Friend WithEvents pckInfoBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblPckSysVersion As System.Windows.Forms.Label
    Friend WithEvents lblPckDrvVersion As System.Windows.Forms.Label
    Friend WithEvents lblPckDate As System.Windows.Forms.Label
    Friend WithEvents lblPckSystem As System.Windows.Forms.Label
    Friend WithEvents lblPckComputerName As System.Windows.Forms.Label
    Friend WithEvents lblPckDesc As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblPckInfo As System.Windows.Forms.Label
    Friend WithEvents pictPck As System.Windows.Forms.PictureBox
    Friend WithEvents popupMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents popupSelectAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents popupCancelSelect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents popupAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents popupOem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents popupThird As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents popupSignature As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents popupPortability As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuLang As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MostraDispositiviPCIToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdOfflineMode As System.Windows.Forms.ToolStripButton

End Class
