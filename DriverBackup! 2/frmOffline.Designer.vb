<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOffline
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
        Me.txtsysPath = New System.Windows.Forms.TextBox
        Me.cmdBrowse = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblPcName = New System.Windows.Forms.Label
        Me.chkUseOfflineName = New System.Windows.Forms.CheckBox
        Me.cmdLoad = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtsysPath
        '
        Me.txtsysPath.Location = New System.Drawing.Point(11, 72)
        Me.txtsysPath.Name = "txtsysPath"
        Me.txtsysPath.Size = New System.Drawing.Size(560, 20)
        Me.txtsysPath.TabIndex = 0
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Location = New System.Drawing.Point(482, 187)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(87, 27)
        Me.cmdBrowse.TabIndex = 1
        Me.cmdBrowse.Text = "Sfoglia"
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 108)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(130, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Nome del computer offline"
        '
        'lblPcName
        '
        Me.lblPcName.AutoSize = True
        Me.lblPcName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPcName.Location = New System.Drawing.Point(166, 108)
        Me.lblPcName.Name = "lblPcName"
        Me.lblPcName.Size = New System.Drawing.Size(0, 16)
        Me.lblPcName.TabIndex = 3
        '
        'chkUseOfflineName
        '
        Me.chkUseOfflineName.AutoSize = True
        Me.chkUseOfflineName.Checked = True
        Me.chkUseOfflineName.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUseOfflineName.Location = New System.Drawing.Point(12, 147)
        Me.chkUseOfflineName.Name = "chkUseOfflineName"
        Me.chkUseOfflineName.Size = New System.Drawing.Size(380, 17)
        Me.chkUseOfflineName.TabIndex = 4
        Me.chkUseOfflineName.Text = "Usa nome del computer remoto per espandere il tag %COMPUTERNAME%"
        Me.chkUseOfflineName.UseVisualStyleBackColor = True
        '
        'cmdLoad
        '
        Me.cmdLoad.Location = New System.Drawing.Point(391, 187)
        Me.cmdLoad.Name = "cmdLoad"
        Me.cmdLoad.Size = New System.Drawing.Size(85, 26)
        Me.cmdLoad.TabIndex = 5
        Me.cmdLoad.Text = "Carica"
        Me.cmdLoad.UseVisualStyleBackColor = True
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(300, 187)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(85, 26)
        Me.cmdOk.TabIndex = 6
        Me.cmdOk.Text = "Ok"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(18, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(553, 35)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Connettere al computer il disco rigido o un'altro supporto di memorizzazione dove" & _
            " è contenuta l'installazione del sistema di cui si vuole effettuare il backup."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(27, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(544, 20)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "NB: Le versioni di Windows supportate sono NT/2000/Xp/Vista/7."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmOffline
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(584, 226)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdLoad)
        Me.Controls.Add(Me.chkUseOfflineName)
        Me.Controls.Add(Me.lblPcName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.txtsysPath)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOffline"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Backup offline"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtsysPath As System.Windows.Forms.TextBox
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblPcName As System.Windows.Forms.Label
    Friend WithEvents chkUseOfflineName As System.Windows.Forms.CheckBox
    Friend WithEvents cmdLoad As System.Windows.Forms.Button
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
