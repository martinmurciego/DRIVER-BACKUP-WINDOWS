<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCmdBuilder
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtOutput = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.groupBackup = New System.Windows.Forms.GroupBox
        Me.chkUseOfflineName = New System.Windows.Forms.CheckBox
        Me.chkOffline = New System.Windows.Forms.CheckBox
        Me.txtSysPath = New System.Windows.Forms.TextBox
        Me.lblSysPath = New System.Windows.Forms.Label
        Me.txtDateFormat = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.chkAutoRestore = New System.Windows.Forms.CheckBox
        Me.chkOverwrite = New System.Windows.Forms.CheckBox
        Me.txtBackupFile = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.txtDevFormat = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtFormat = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button5 = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.groupRestore = New System.Windows.Forms.GroupBox
        Me.Button4 = New System.Windows.Forms.Button
        Me.chkForceUpdate = New System.Windows.Forms.CheckBox
        Me.chkPnPUpdate = New System.Windows.Forms.CheckBox
        Me.txtRestoreFile = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.optRestore = New System.Windows.Forms.RadioButton
        Me.optBackup = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkVerbose = New System.Windows.Forms.CheckBox
        Me.optRecommended = New System.Windows.Forms.RadioButton
        Me.chkLog = New System.Windows.Forms.CheckBox
        Me.chkPort = New System.Windows.Forms.CheckBox
        Me.chkSignature = New System.Windows.Forms.CheckBox
        Me.optThird = New System.Windows.Forms.RadioButton
        Me.optOEM = New System.Windows.Forms.RadioButton
        Me.optAll = New System.Windows.Forms.RadioButton
        Me.groupBackup.SuspendLayout()
        Me.groupRestore.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(590, 32)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Questa finestra permette di generare rapidamente una serie di comandi inseribili " & _
            "tramite riga di comando per automatizzare le funzionalità di DriverBackup!"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 486)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(39, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Output"
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(77, 486)
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(532, 20)
        Me.txtOutput.TabIndex = 5
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(511, 527)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(98, 30)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(292, 527)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(101, 29)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Copia"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(399, 527)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(106, 29)
        Me.Button3.TabIndex = 8
        Me.Button3.Text = "Genera"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'groupBackup
        '
        Me.groupBackup.Controls.Add(Me.chkUseOfflineName)
        Me.groupBackup.Controls.Add(Me.chkOffline)
        Me.groupBackup.Controls.Add(Me.txtSysPath)
        Me.groupBackup.Controls.Add(Me.lblSysPath)
        Me.groupBackup.Controls.Add(Me.txtDateFormat)
        Me.groupBackup.Controls.Add(Me.Label10)
        Me.groupBackup.Controls.Add(Me.chkAutoRestore)
        Me.groupBackup.Controls.Add(Me.chkOverwrite)
        Me.groupBackup.Controls.Add(Me.txtBackupFile)
        Me.groupBackup.Controls.Add(Me.Label7)
        Me.groupBackup.Controls.Add(Me.Label6)
        Me.groupBackup.Controls.Add(Me.txtDesc)
        Me.groupBackup.Controls.Add(Me.txtDevFormat)
        Me.groupBackup.Controls.Add(Me.Label5)
        Me.groupBackup.Controls.Add(Me.txtFormat)
        Me.groupBackup.Controls.Add(Me.Label2)
        Me.groupBackup.Controls.Add(Me.Button5)
        Me.groupBackup.Controls.Add(Me.txtPath)
        Me.groupBackup.Controls.Add(Me.Label3)
        Me.groupBackup.Location = New System.Drawing.Point(22, 116)
        Me.groupBackup.Name = "groupBackup"
        Me.groupBackup.Size = New System.Drawing.Size(581, 308)
        Me.groupBackup.TabIndex = 9
        Me.groupBackup.TabStop = False
        '
        'chkUseOfflineName
        '
        Me.chkUseOfflineName.AutoSize = True
        Me.chkUseOfflineName.Location = New System.Drawing.Point(14, 273)
        Me.chkUseOfflineName.Name = "chkUseOfflineName"
        Me.chkUseOfflineName.Size = New System.Drawing.Size(365, 17)
        Me.chkUseOfflineName.TabIndex = 57
        Me.chkUseOfflineName.Text = "Utilizza nome del computer offline per espandere %COMPUTERNAME%"
        Me.chkUseOfflineName.UseVisualStyleBackColor = True
        Me.chkUseOfflineName.Visible = False
        '
        'chkOffline
        '
        Me.chkOffline.AutoSize = True
        Me.chkOffline.Location = New System.Drawing.Point(305, 244)
        Me.chkOffline.Name = "chkOffline"
        Me.chkOffline.Size = New System.Drawing.Size(189, 17)
        Me.chkOffline.TabIndex = 56
        Me.chkOffline.Text = "Modalità backup da sistema offline"
        Me.chkOffline.UseVisualStyleBackColor = True
        '
        'txtSysPath
        '
        Me.txtSysPath.Location = New System.Drawing.Point(160, 172)
        Me.txtSysPath.Name = "txtSysPath"
        Me.txtSysPath.Size = New System.Drawing.Size(333, 20)
        Me.txtSysPath.TabIndex = 55
        '
        'lblSysPath
        '
        Me.lblSysPath.AutoSize = True
        Me.lblSysPath.Location = New System.Drawing.Point(15, 178)
        Me.lblSysPath.Name = "lblSysPath"
        Me.lblSysPath.Size = New System.Drawing.Size(87, 13)
        Me.lblSysPath.TabIndex = 54
        Me.lblSysPath.Text = "Percorso sistema"
        '
        'txtDateFormat
        '
        Me.txtDateFormat.Location = New System.Drawing.Point(158, 143)
        Me.txtDateFormat.Name = "txtDateFormat"
        Me.txtDateFormat.Size = New System.Drawing.Size(336, 20)
        Me.txtDateFormat.TabIndex = 53
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(13, 150)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 52
        Me.Label10.Text = "Formato data"
        '
        'chkAutoRestore
        '
        Me.chkAutoRestore.AutoSize = True
        Me.chkAutoRestore.Location = New System.Drawing.Point(14, 244)
        Me.chkAutoRestore.Name = "chkAutoRestore"
        Me.chkAutoRestore.Size = New System.Drawing.Size(241, 17)
        Me.chkAutoRestore.TabIndex = 51
        Me.chkAutoRestore.Text = "Crea files per il ripristino automatico dei drivers"
        Me.chkAutoRestore.UseVisualStyleBackColor = True
        '
        'chkOverwrite
        '
        Me.chkOverwrite.AutoSize = True
        Me.chkOverwrite.Location = New System.Drawing.Point(14, 208)
        Me.chkOverwrite.Name = "chkOverwrite"
        Me.chkOverwrite.Size = New System.Drawing.Size(450, 17)
        Me.chkOverwrite.TabIndex = 50
        Me.chkOverwrite.Text = "Attiva sovrascrittura, se necessario, dei files presenti sul percorso di backup, " & _
            "(Sconsigliato)"
        Me.chkOverwrite.UseVisualStyleBackColor = True
        '
        'txtBackupFile
        '
        Me.txtBackupFile.Location = New System.Drawing.Point(159, 66)
        Me.txtBackupFile.Name = "txtBackupFile"
        Me.txtBackupFile.Size = New System.Drawing.Size(334, 20)
        Me.txtBackupFile.TabIndex = 48
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 69)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 13)
        Me.Label7.TabIndex = 47
        Me.Label7.Text = "File Backup"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 46
        Me.Label6.Text = "Descrizione"
        '
        'txtDesc
        '
        Me.txtDesc.Location = New System.Drawing.Point(158, 41)
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(335, 20)
        Me.txtDesc.TabIndex = 45
        Me.txtDesc.Text = "[Inserire qui una descrizione]"
        '
        'txtDevFormat
        '
        Me.txtDevFormat.Location = New System.Drawing.Point(159, 117)
        Me.txtDevFormat.Name = "txtDevFormat"
        Me.txtDevFormat.Size = New System.Drawing.Size(335, 20)
        Me.txtDevFormat.TabIndex = 44
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(141, 13)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Formato percorso dispositivo"
        '
        'txtFormat
        '
        Me.txtFormat.Location = New System.Drawing.Point(159, 91)
        Me.txtFormat.Name = "txtFormat"
        Me.txtFormat.Size = New System.Drawing.Size(335, 20)
        Me.txtFormat.TabIndex = 42
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 94)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "Formato percorso"
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(507, 15)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(69, 27)
        Me.Button5.TabIndex = 40
        Me.Button5.Text = "Sfoglia.."
        Me.Button5.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(158, 15)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(335, 20)
        Me.txtPath.TabIndex = 39
        Me.txtPath.Text = "[Fare clc su Sfoglia per selezionare un percorso]"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 38
        Me.Label3.Text = "Percorso"
        '
        'groupRestore
        '
        Me.groupRestore.Controls.Add(Me.Button4)
        Me.groupRestore.Controls.Add(Me.chkForceUpdate)
        Me.groupRestore.Controls.Add(Me.chkPnPUpdate)
        Me.groupRestore.Controls.Add(Me.txtRestoreFile)
        Me.groupRestore.Controls.Add(Me.Label9)
        Me.groupRestore.Location = New System.Drawing.Point(620, 222)
        Me.groupRestore.Name = "groupRestore"
        Me.groupRestore.Size = New System.Drawing.Size(581, 184)
        Me.groupRestore.TabIndex = 10
        Me.groupRestore.TabStop = False
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(513, 19)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(52, 22)
        Me.Button4.TabIndex = 20
        Me.Button4.Text = "Sfoglia"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'chkForceUpdate
        '
        Me.chkForceUpdate.AutoSize = True
        Me.chkForceUpdate.Location = New System.Drawing.Point(14, 124)
        Me.chkForceUpdate.Name = "chkForceUpdate"
        Me.chkForceUpdate.Size = New System.Drawing.Size(340, 17)
        Me.chkForceUpdate.TabIndex = 19
        Me.chkForceUpdate.Text = "Modifica informazioni sul percorso qualora il drivers sia già installato"
        Me.chkForceUpdate.UseVisualStyleBackColor = True
        '
        'chkPnPUpdate
        '
        Me.chkPnPUpdate.AutoSize = True
        Me.chkPnPUpdate.Location = New System.Drawing.Point(14, 90)
        Me.chkPnPUpdate.Name = "chkPnPUpdate"
        Me.chkPnPUpdate.Size = New System.Drawing.Size(369, 17)
        Me.chkPnPUpdate.TabIndex = 18
        Me.chkPnPUpdate.Text = "Avvia aggiornamento periferiche Plug && Play dopo il ripristino (Consigliato)"
        Me.chkPnPUpdate.UseVisualStyleBackColor = True
        '
        'txtRestoreFile
        '
        Me.txtRestoreFile.Location = New System.Drawing.Point(121, 21)
        Me.txtRestoreFile.Name = "txtRestoreFile"
        Me.txtRestoreFile.Size = New System.Drawing.Size(384, 20)
        Me.txtRestoreFile.TabIndex = 2
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "File di backup"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(19, 442)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(579, 32)
        Me.Label4.TabIndex = 50
        Me.Label4.Text = "Le variabili di sistema inserite fra coppie di % nei nomi di file o directory sar" & _
            "anno automaticamente espanse (Esempio: %windir%)"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optRestore)
        Me.GroupBox1.Controls.Add(Me.optBackup)
        Me.GroupBox1.Location = New System.Drawing.Point(22, 43)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(125, 77)
        Me.GroupBox1.TabIndex = 51
        Me.GroupBox1.TabStop = False
        '
        'optRestore
        '
        Me.optRestore.AutoSize = True
        Me.optRestore.Location = New System.Drawing.Point(14, 50)
        Me.optRestore.Name = "optRestore"
        Me.optRestore.Size = New System.Drawing.Size(68, 17)
        Me.optRestore.TabIndex = 4
        Me.optRestore.TabStop = True
        Me.optRestore.Text = "Ripristino"
        Me.optRestore.UseVisualStyleBackColor = True
        '
        'optBackup
        '
        Me.optBackup.AutoSize = True
        Me.optBackup.Checked = True
        Me.optBackup.Location = New System.Drawing.Point(16, 19)
        Me.optBackup.Name = "optBackup"
        Me.optBackup.Size = New System.Drawing.Size(62, 17)
        Me.optBackup.TabIndex = 3
        Me.optBackup.TabStop = True
        Me.optBackup.Text = "Backup"
        Me.optBackup.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkVerbose)
        Me.GroupBox2.Controls.Add(Me.optRecommended)
        Me.GroupBox2.Controls.Add(Me.chkLog)
        Me.GroupBox2.Controls.Add(Me.chkPort)
        Me.GroupBox2.Controls.Add(Me.chkSignature)
        Me.GroupBox2.Controls.Add(Me.optThird)
        Me.GroupBox2.Controls.Add(Me.optOEM)
        Me.GroupBox2.Controls.Add(Me.optAll)
        Me.GroupBox2.Location = New System.Drawing.Point(153, 43)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(449, 77)
        Me.GroupBox2.TabIndex = 52
        Me.GroupBox2.TabStop = False
        '
        'chkVerbose
        '
        Me.chkVerbose.AutoSize = True
        Me.chkVerbose.Location = New System.Drawing.Point(337, 50)
        Me.chkVerbose.Name = "chkVerbose"
        Me.chkVerbose.Size = New System.Drawing.Size(94, 17)
        Me.chkVerbose.TabIndex = 7
        Me.chkVerbose.Text = "Verbose mode"
        Me.chkVerbose.UseVisualStyleBackColor = True
        '
        'optRecommended
        '
        Me.optRecommended.AutoSize = True
        Me.optRecommended.Location = New System.Drawing.Point(301, 19)
        Me.optRecommended.Name = "optRecommended"
        Me.optRecommended.Size = New System.Drawing.Size(72, 17)
        Me.optRecommended.TabIndex = 6
        Me.optRecommended.TabStop = True
        Me.optRecommended.Text = "Consigliati"
        Me.optRecommended.UseVisualStyleBackColor = True
        '
        'chkLog
        '
        Me.chkLog.AutoSize = True
        Me.chkLog.Checked = True
        Me.chkLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLog.Location = New System.Drawing.Point(246, 50)
        Me.chkLog.Name = "chkLog"
        Me.chkLog.Size = New System.Drawing.Size(64, 17)
        Me.chkLog.TabIndex = 5
        Me.chkLog.Text = "Logging"
        Me.chkLog.UseVisualStyleBackColor = True
        '
        'chkPort
        '
        Me.chkPort.AutoSize = True
        Me.chkPort.Location = New System.Drawing.Point(125, 50)
        Me.chkPort.Name = "chkPort"
        Me.chkPort.Size = New System.Drawing.Size(101, 17)
        Me.chkPort.TabIndex = 4
        Me.chkPort.Text = "Piena portabilità"
        Me.chkPort.UseVisualStyleBackColor = True
        '
        'chkSignature
        '
        Me.chkSignature.AutoSize = True
        Me.chkSignature.Location = New System.Drawing.Point(20, 50)
        Me.chkSignature.Name = "chkSignature"
        Me.chkSignature.Size = New System.Drawing.Size(87, 17)
        Me.chkSignature.TabIndex = 3
        Me.chkSignature.Text = "Firma digitale"
        Me.chkSignature.UseVisualStyleBackColor = True
        '
        'optThird
        '
        Me.optThird.AutoSize = True
        Me.optThird.Location = New System.Drawing.Point(184, 19)
        Me.optThird.Name = "optThird"
        Me.optThird.Size = New System.Drawing.Size(75, 17)
        Me.optThird.TabIndex = 2
        Me.optThird.Text = "Terze parti"
        Me.optThird.UseVisualStyleBackColor = True
        '
        'optOEM
        '
        Me.optOEM.AutoSize = True
        Me.optOEM.Location = New System.Drawing.Point(93, 19)
        Me.optOEM.Name = "optOEM"
        Me.optOEM.Size = New System.Drawing.Size(49, 17)
        Me.optOEM.TabIndex = 1
        Me.optOEM.Text = "OEM"
        Me.optOEM.UseVisualStyleBackColor = True
        '
        'optAll
        '
        Me.optAll.AutoSize = True
        Me.optAll.Checked = True
        Me.optAll.Location = New System.Drawing.Point(20, 19)
        Me.optAll.Name = "optAll"
        Me.optAll.Size = New System.Drawing.Size(46, 17)
        Me.optAll.TabIndex = 0
        Me.optAll.TabStop = True
        Me.optAll.Text = "Tutti"
        Me.optAll.UseVisualStyleBackColor = True
        '
        'frmCmdBuilder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(623, 569)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.groupRestore)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.groupBackup)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.Name = "frmCmdBuilder"
        Me.Text = "Builder della riga di comando"
        Me.groupBackup.ResumeLayout(False)
        Me.groupBackup.PerformLayout()
        Me.groupRestore.ResumeLayout(False)
        Me.groupRestore.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents groupBackup As System.Windows.Forms.GroupBox
    Friend WithEvents chkAutoRestore As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverwrite As System.Windows.Forms.CheckBox
    Friend WithEvents txtBackupFile As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtDevFormat As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtFormat As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents groupRestore As System.Windows.Forms.GroupBox
    Friend WithEvents txtRestoreFile As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkForceUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents chkPnPUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents optRestore As System.Windows.Forms.RadioButton
    Friend WithEvents optBackup As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkPort As System.Windows.Forms.CheckBox
    Friend WithEvents chkSignature As System.Windows.Forms.CheckBox
    Friend WithEvents optThird As System.Windows.Forms.RadioButton
    Friend WithEvents optOEM As System.Windows.Forms.RadioButton
    Friend WithEvents optAll As System.Windows.Forms.RadioButton
    Friend WithEvents chkLog As System.Windows.Forms.CheckBox
    Friend WithEvents txtDateFormat As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents optRecommended As System.Windows.Forms.RadioButton
    Friend WithEvents chkVerbose As System.Windows.Forms.CheckBox
    Friend WithEvents chkOffline As System.Windows.Forms.CheckBox
    Friend WithEvents txtSysPath As System.Windows.Forms.TextBox
    Friend WithEvents lblSysPath As System.Windows.Forms.Label
    Friend WithEvents chkUseOfflineName As System.Windows.Forms.CheckBox
End Class
