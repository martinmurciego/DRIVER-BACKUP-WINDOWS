<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBackRest
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
        Me.groupBack = New System.Windows.Forms.GroupBox
        Me.txtDevFormat = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkAutorun = New System.Windows.Forms.CheckBox
        Me.chkAutoRestore = New System.Windows.Forms.CheckBox
        Me.chkOverwrite = New System.Windows.Forms.CheckBox
        Me.txtFormat = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.groupInfo = New System.Windows.Forms.GroupBox
        Me.lblCurrDevice = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.barFiles = New System.Windows.Forms.ProgressBar
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblCurrFile = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblCompletePath = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.barDevices = New System.Windows.Forms.ProgressBar
        Me.Label9 = New System.Windows.Forms.Label
        Me.devTree = New System.Windows.Forms.TreeView
        Me.lblDevFound = New System.Windows.Forms.Label
        Me.cmdAction = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdContinue = New System.Windows.Forms.Button
        Me.groupBack.SuspendLayout()
        Me.groupInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBack
        '
        Me.groupBack.Controls.Add(Me.txtDevFormat)
        Me.groupBack.Controls.Add(Me.Label5)
        Me.groupBack.Controls.Add(Me.chkAutorun)
        Me.groupBack.Controls.Add(Me.chkAutoRestore)
        Me.groupBack.Controls.Add(Me.chkOverwrite)
        Me.groupBack.Controls.Add(Me.txtFormat)
        Me.groupBack.Controls.Add(Me.Label2)
        Me.groupBack.Controls.Add(Me.Button3)
        Me.groupBack.Controls.Add(Me.txtPath)
        Me.groupBack.Controls.Add(Me.Label1)
        Me.groupBack.Location = New System.Drawing.Point(15, 436)
        Me.groupBack.Name = "groupBack"
        Me.groupBack.Size = New System.Drawing.Size(587, 205)
        Me.groupBack.TabIndex = 1
        Me.groupBack.TabStop = False
        Me.groupBack.Text = "Opzioni backup"
        '
        'txtDevFormat
        '
        Me.txtDevFormat.Location = New System.Drawing.Point(164, 91)
        Me.txtDevFormat.Name = "txtDevFormat"
        Me.txtDevFormat.Size = New System.Drawing.Size(335, 20)
        Me.txtDevFormat.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 94)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(141, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Formato percorso dispositivo"
        '
        'chkAutorun
        '
        Me.chkAutorun.AutoSize = True
        Me.chkAutorun.Location = New System.Drawing.Point(15, 172)
        Me.chkAutorun.Name = "chkAutorun"
        Me.chkAutorun.Size = New System.Drawing.Size(460, 17)
        Me.chkAutorun.TabIndex = 7
        Me.chkAutorun.Text = "Abilita esecuzione automatica (Autorun) del ripristino da CD\DVD-ROM o memorie fl" & _
            "ash USB"
        Me.chkAutorun.UseVisualStyleBackColor = True
        '
        'chkAutoRestore
        '
        Me.chkAutoRestore.AutoSize = True
        Me.chkAutoRestore.Location = New System.Drawing.Point(15, 149)
        Me.chkAutoRestore.Name = "chkAutoRestore"
        Me.chkAutoRestore.Size = New System.Drawing.Size(241, 17)
        Me.chkAutoRestore.TabIndex = 6
        Me.chkAutoRestore.Text = "Crea files per il ripristino automatico dei drivers"
        Me.chkAutoRestore.UseVisualStyleBackColor = True
        '
        'chkOverwrite
        '
        Me.chkOverwrite.AutoSize = True
        Me.chkOverwrite.Location = New System.Drawing.Point(15, 126)
        Me.chkOverwrite.Name = "chkOverwrite"
        Me.chkOverwrite.Size = New System.Drawing.Size(450, 17)
        Me.chkOverwrite.TabIndex = 5
        Me.chkOverwrite.Text = "Attiva sovrascrittura, se necessario, dei files presenti sul percorso di backup, " & _
            "(Sconsigliato)"
        Me.chkOverwrite.UseVisualStyleBackColor = True
        '
        'txtFormat
        '
        Me.txtFormat.Location = New System.Drawing.Point(164, 55)
        Me.txtFormat.Name = "txtFormat"
        Me.txtFormat.Size = New System.Drawing.Size(335, 20)
        Me.txtFormat.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Formato percorso"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(513, 19)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(69, 27)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Sfoglia.."
        Me.Button3.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(164, 19)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(335, 20)
        Me.txtPath.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Percorso"
        '
        'groupInfo
        '
        Me.groupInfo.Controls.Add(Me.lblCurrDevice)
        Me.groupInfo.Controls.Add(Me.Label7)
        Me.groupInfo.Controls.Add(Me.barFiles)
        Me.groupInfo.Controls.Add(Me.Label6)
        Me.groupInfo.Controls.Add(Me.lblCurrFile)
        Me.groupInfo.Controls.Add(Me.Label4)
        Me.groupInfo.Controls.Add(Me.lblCompletePath)
        Me.groupInfo.Controls.Add(Me.Label3)
        Me.groupInfo.Controls.Add(Me.barDevices)
        Me.groupInfo.Controls.Add(Me.Label9)
        Me.groupInfo.Location = New System.Drawing.Point(15, 436)
        Me.groupInfo.Name = "groupInfo"
        Me.groupInfo.Size = New System.Drawing.Size(587, 195)
        Me.groupInfo.TabIndex = 6
        Me.groupInfo.TabStop = False
        Me.groupInfo.Text = "Operazioni in corso"
        '
        'lblCurrDevice
        '
        Me.lblCurrDevice.AutoSize = True
        Me.lblCurrDevice.Location = New System.Drawing.Point(142, 133)
        Me.lblCurrDevice.Name = "lblCurrDevice"
        Me.lblCurrDevice.Size = New System.Drawing.Size(39, 13)
        Me.lblCurrDevice.TabIndex = 7
        Me.lblCurrDevice.Text = "Label8"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 133)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Device corrente"
        '
        'barFiles
        '
        Me.barFiles.Location = New System.Drawing.Point(145, 94)
        Me.barFiles.Name = "barFiles"
        Me.barFiles.Size = New System.Drawing.Size(433, 13)
        Me.barFiles.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 94)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Files copiati"
        '
        'lblCurrFile
        '
        Me.lblCurrFile.AutoSize = True
        Me.lblCurrFile.Location = New System.Drawing.Point(142, 67)
        Me.lblCurrFile.Name = "lblCurrFile"
        Me.lblCurrFile.Size = New System.Drawing.Size(39, 13)
        Me.lblCurrFile.TabIndex = 3
        Me.lblCurrFile.Text = "Label5"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "File corrente"
        '
        'lblCompletePath
        '
        Me.lblCompletePath.AutoSize = True
        Me.lblCompletePath.Location = New System.Drawing.Point(142, 31)
        Me.lblCompletePath.Name = "lblCompletePath"
        Me.lblCompletePath.Size = New System.Drawing.Size(39, 13)
        Me.lblCompletePath.TabIndex = 1
        Me.lblCompletePath.Text = "Label4"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Percorso completo"
        '
        'barDevices
        '
        Me.barDevices.Location = New System.Drawing.Point(145, 161)
        Me.barDevices.Name = "barDevices"
        Me.barDevices.Size = New System.Drawing.Size(432, 13)
        Me.barDevices.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 161)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Devices copiati"
        '
        'devTree
        '
        Me.devTree.Location = New System.Drawing.Point(17, 44)
        Me.devTree.Name = "devTree"
        Me.devTree.Size = New System.Drawing.Size(587, 381)
        Me.devTree.TabIndex = 7
        '
        'lblDevFound
        '
        Me.lblDevFound.AutoSize = True
        Me.lblDevFound.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDevFound.Location = New System.Drawing.Point(14, 18)
        Me.lblDevFound.Name = "lblDevFound"
        Me.lblDevFound.Size = New System.Drawing.Size(45, 13)
        Me.lblDevFound.TabIndex = 8
        Me.lblDevFound.Text = "Label5"
        Me.lblDevFound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdAction
        '
        Me.cmdAction.Location = New System.Drawing.Point(486, 648)
        Me.cmdAction.Name = "cmdAction"
        Me.cmdAction.Size = New System.Drawing.Size(116, 26)
        Me.cmdAction.TabIndex = 9
        Me.cmdAction.Text = "Avvia backup"
        Me.cmdAction.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(366, 648)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(116, 26)
        Me.cmdClose.TabIndex = 10
        Me.cmdClose.Text = "Chiudi"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(488, 647)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(114, 26)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.Text = "Annulla"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdContinue
        '
        Me.cmdContinue.Location = New System.Drawing.Point(488, 648)
        Me.cmdContinue.Name = "cmdContinue"
        Me.cmdContinue.Size = New System.Drawing.Size(114, 25)
        Me.cmdContinue.TabIndex = 12
        Me.cmdContinue.Text = "Continua..."
        Me.cmdContinue.UseVisualStyleBackColor = True
        '
        'frmBackRest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(620, 686)
        Me.Controls.Add(Me.groupBack)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdAction)
        Me.Controls.Add(Me.lblDevFound)
        Me.Controls.Add(Me.devTree)
        Me.Controls.Add(Me.cmdContinue)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.groupInfo)
        Me.MaximizeBox = False
        Me.Name = "frmBackRest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmBackRest"
        Me.groupBack.ResumeLayout(False)
        Me.groupBack.PerformLayout()
        Me.groupInfo.ResumeLayout(False)
        Me.groupInfo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents groupBack As System.Windows.Forms.GroupBox
    Friend WithEvents chkAutorun As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoRestore As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverwrite As System.Windows.Forms.CheckBox
    Friend WithEvents txtFormat As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents groupInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblCurrDevice As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents barFiles As System.Windows.Forms.ProgressBar
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblCurrFile As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblCompletePath As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents barDevices As System.Windows.Forms.ProgressBar
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents devTree As System.Windows.Forms.TreeView
    Friend WithEvents lblDevFound As System.Windows.Forms.Label
    Friend WithEvents cmdAction As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdContinue As System.Windows.Forms.Button
    Friend WithEvents txtDevFormat As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
