Public Class frmDonate

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Utils.OpenShellFile(Me.Handle, LinkLabel1.Text)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub frmDonate_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim regKey As RegistryKey = Nothing

        Try
            If chkShowAgain.Checked = False Then Return

            My.Settings.CheckDonate = True


        Catch ex As Exception
        Finally
            If regKey IsNot Nothing Then regKey.Close()
        End Try
    End Sub


    Private Sub frmDonate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class