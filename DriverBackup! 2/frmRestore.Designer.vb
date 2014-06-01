<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRestore
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
        Me.devTree = New System.Windows.Forms.TreeView
        Me.chkPnPUpdate = New System.Windows.Forms.CheckBox
        Me.cmdLog = New System.Windows.Forms.Button
        Me.cmdContinue = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdRestore = New System.Windows.Forms.Button
        Me.lblDevFound = New System.Windows.Forms.Label
        Me.chkForceUpdate = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'devTree
        '
        Me.devTree.Location = New System.Drawing.Point(18, 36)
        Me.devTree.Name = "devTree"
        Me.devTree.Size = New System.Drawing.Size(505, 270)
        Me.devTree.TabIndex = 0
        '
        'chkPnPUpdate
        '
        Me.chkPnPUpdate.AutoSize = True
        Me.chkPnPUpdate.Location = New System.Drawing.Point(18, 326)
        Me.chkPnPUpdate.Name = "chkPnPUpdate"
        Me.chkPnPUpdate.Size = New System.Drawing.Size(369, 17)
        Me.chkPnPUpdate.TabIndex = 1
        Me.chkPnPUpdate.Text = "Avvia aggiornamento periferiche Plug && Play dopo il ripristino (Consigliato)"
        Me.chkPnPUpdate.UseVisualStyleBackColor = True
        '
        'cmdLog
        '
        Me.cmdLog.Location = New System.Drawing.Point(194, 423)
        Me.cmdLog.Name = "cmdLog"
        Me.cmdLog.Size = New System.Drawing.Size(105, 30)
        Me.cmdLog.TabIndex = 12
        Me.cmdLog.Text = "Salva file log"
        Me.cmdLog.UseVisualStyleBackColor = True
        Me.cmdLog.Visible = False
        '
        'cmdContinue
        '
        Me.cmdContinue.Location = New System.Drawing.Point(26, 403)
        Me.cmdContinue.Name = "cmdContinue"
        Me.cmdContinue.Size = New System.Drawing.Size(105, 28)
        Me.cmdContinue.TabIndex = 11
        Me.cmdContinue.Text = "Continua.."
        Me.cmdContinue.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(26, 427)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(105, 28)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.Text = "Annulla"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(305, 424)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(105, 29)
        Me.cmdOk.TabIndex = 9
        Me.cmdOk.Text = "Ok"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'cmdRestore
        '
        Me.cmdRestore.Location = New System.Drawing.Point(416, 424)
        Me.cmdRestore.Name = "cmdRestore"
        Me.cmdRestore.Size = New System.Drawing.Size(104, 29)
        Me.cmdRestore.TabIndex = 8
        Me.cmdRestore.Text = "Ripristina"
        Me.cmdRestore.UseVisualStyleBackColor = True
        '
        'lblDevFound
        '
        Me.lblDevFound.AutoSize = True
        Me.lblDevFound.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDevFound.Location = New System.Drawing.Point(15, 9)
        Me.lblDevFound.Name = "lblDevFound"
        Me.lblDevFound.Size = New System.Drawing.Size(126, 13)
        Me.lblDevFound.TabIndex = 13
        Me.lblDevFound.Text = "Drivers selezionati: 0"
        '
        'chkForceUpdate
        '
        Me.chkForceUpdate.AutoSize = True
        Me.chkForceUpdate.Location = New System.Drawing.Point(18, 355)
        Me.chkForceUpdate.Name = "chkForceUpdate"
        Me.chkForceUpdate.Size = New System.Drawing.Size(133, 17)
        Me.chkForceUpdate.TabIndex = 14
        Me.chkForceUpdate.Text = "Aggiornamento file INF"
        Me.chkForceUpdate.UseVisualStyleBackColor = True
        '
        'frmRestore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(539, 465)
        Me.Controls.Add(Me.chkForceUpdate)
        Me.Controls.Add(Me.lblDevFound)
        Me.Controls.Add(Me.cmdLog)
        Me.Controls.Add(Me.cmdContinue)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cmdRestore)
        Me.Controls.Add(Me.chkPnPUpdate)
        Me.Controls.Add(Me.devTree)
        Me.MaximizeBox = False
        Me.Name = "frmRestore"
        Me.Text = "Ripristina drivers selezionati"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents devTree As System.Windows.Forms.TreeView
    Friend WithEvents chkPnPUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents cmdLog As System.Windows.Forms.Button
    Friend WithEvents cmdContinue As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdRestore As System.Windows.Forms.Button
    Friend WithEvents lblDevFound As System.Windows.Forms.Label
    Friend WithEvents chkForceUpdate As System.Windows.Forms.CheckBox
End Class
