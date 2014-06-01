<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRemove
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
        Me.devTree = New System.Windows.Forms.TreeView
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(275, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Lista dei drivers di dispositivo ripristinati da DriverBackup!"
        '
        'devTree
        '
        Me.devTree.CheckBoxes = True
        Me.devTree.Location = New System.Drawing.Point(12, 41)
        Me.devTree.Name = "devTree"
        Me.devTree.Size = New System.Drawing.Size(468, 291)
        Me.devTree.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(371, 348)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(109, 29)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(236, 348)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(130, 29)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Rimuovi la selezione"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmRemove
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 387)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.devTree)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.Name = "frmRemove"
        Me.Text = "Rimozione drivers"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents devTree As System.Windows.Forms.TreeView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
