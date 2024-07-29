Public Class Login
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Application.Exit()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Dim obj = New Billing
        obj.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If UnameTb.Text = "" Or PasswordTb.Text = "" Then
            MsgBox("Enter Username and Password")
        ElseIf UnameTb.Text = "Admin" And PasswordTb.Text = "Password" Then
            Dim obj = New Items
            obj.Show()
            Me.Hide()
        Else
            MsgBox("Wrong Username or Password")
            UnameTb.Text = ""
            PasswordTb.Text = ""
        End If
    End Sub

    Private Sub UnameTb_TextChanged(sender As Object, e As EventArgs) Handles UnameTb.TextChanged
        UnameTb.Focus()
    End Sub
End Class