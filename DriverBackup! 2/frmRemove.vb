Public Class frmRemove

    Dim WithEvents devRemover As DeviceRemove
    Dim internalList As Dictionary(Of String, String)

    Private Sub DeviceUsed(ByVal sender As Object, ByVal e As CancelEventArgs) Handles devRemover.DeviceUsed
        If MsgBox(GetLangStr("FRMREMOVE:USERFORCE"), MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub frmRemove_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadControls(frmMain.ImageList1)
        ShowDevices()
    End Sub

    Private Function ByPass() As Boolean
        Return False
    End Function

    Public Sub RemoveDevices()
        Try

            Dim removed As Integer

            For Each currNode As TreeNode In devTree.Nodes ' As KeyValuePair(Of String, String) In Me.internalList
                If Not currNode.Checked Then Continue For
                If devRemover.RemoveOEMInf(currNode.Tag) = True Then
                    currNode.ImageKey = "icoDelete"
                    removed += 1
                Else
                    currNode.ImageKey = "icoError"
                End If
            Next

            MsgBox([String].Format(GetLangStr("FRMREMOVE:REMOVED"), removed, Me.internalList.Count), MsgBoxStyle.Information)

        Catch ex As Exception
        Finally
            internalList = devRemover.GetInstalledDevices()
            ShowDevices()
        End Try
    End Sub

    Public Sub ShowDevices()
        'Implementazione del metodo ShowDevices
        Dim currNode As TreeNode
        Dim devFnd As Integer

        Try
            devTree.Visible = False
            devTree.Nodes.Clear()
            'Lancia un'eccezione se la collezione non contiene devices

            If internalList Is Nothing OrElse internalList.Count <= 0 Then Throw New ArgumentException

            For Each dv As KeyValuePair(Of String, String) In Me.internalList
                currNode = devTree.Nodes.Add(dv.Value)
                currNode.Tag = dv.Key
                currNode.ImageKey = "icoSave"
                currNode.SelectedImageKey = currNode.ImageKey
                currNode.Checked = False
                devFnd += 1
            Next

        Catch ex As Exception
            devTree.Nodes.Add("NODEV", GetLangStr("FRMMAIN:NODEVICES"), "icoInfo")
        Finally
            devTree.Visible = True
        End Try
    End Sub

    Public Sub LoadControls(ByVal imgList As ImageList)
        Try
            devTree.Nodes.Clear()
            devTree.ImageList = imgList

            devRemover = DeviceRemove.Create()

            If devRemover Is Nothing Then
                'Impossibile accedere al registro
                MsgBox(GetLangStr("ERROR:RegistryAccess"), MsgBoxStyle.Critical)
                Me.Close() 'Chiude la finestra
            End If

            internalList = New Dictionary(Of String, String)

            internalList = devRemover.GetInstalledDevices()
            Me.Icon = frmMain.Icon

        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RemoveDevices()

    End Sub
End Class