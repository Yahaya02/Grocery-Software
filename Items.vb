Imports System.Data.SqlClient
Public Class Items
    Dim con = New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\FONZY D POCKET\Documents\GroceyDbVb.mdf;Integrated Security=True;Connect Timeout=30")
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ItnameTb.Text = "" Or QtyTb.Text = "" Or CatCb.SelectedIndex = -1 Or PriceTb.Text = "" Then
            MsgBox("Missing Information")
        Else
            Try
                con.open()
                Dim query = "insert into ItemTbl values ('" & ItnameTb.Text & "', " & QtyTb.Text & ", " & PriceTb.Text & ", '" & CatCb.SelectedItem.ToString() & "')"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, con)
                cmd.ExecuteNonQuery()
                MsgBox("Item Saved Successfully")
                con.close()
                DisplayItem()
                Clear()
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub Clear()
        ItnameTb.Text = ""
        QtyTb.Text = ""
        PriceTb.Text = ""
        CatCb.SelectedIndex = 0
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Clear()

    End Sub
    Private Sub DisplayItem()
        con.open()
        Dim query = "select * from ItemTbl"
        Dim cmd = New SqlCommand(query, con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        ItemDGV.DataSource = ds.Tables(0)
        con.close()
    End Sub
    Private Sub FilterByCat()
        con.open()
        Dim query = "select * from ItemTbl where ItCat = '" & SearchCb.SelectedItem.ToString() & "'"
        Dim cmd = New SqlCommand(query, con)
        Dim adapter As SqlDataAdapter
        adapter = New SqlDataAdapter(cmd)
        Dim builder As New SqlCommandBuilder(adapter)
        Dim ds As DataSet
        ds = New DataSet
        adapter.Fill(ds)
        ItemDGV.DataSource = ds.Tables(0)
        con.close()
    End Sub
    Dim key = 0
    Private Sub ItemDGV_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ItemDGV.CellMouseClick
        Dim row As DataGridViewRow = ItemDGV.Rows(e.RowIndex)
        ItnameTb.Text = row.Cells(1).Value.ToString
        QtyTb.Text = row.Cells(2).Value.ToString
        PriceTb.Text = row.Cells(3).Value.ToString
        CatCb.SelectedItem = row.Cells(4).Value.ToString
        If ItnameTb.Text = "" Then
            key = 0
        Else
            key = Convert.ToInt32(row.Cells(0).Value.ToString)
        End If
    End Sub

    Private Sub Items_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayItem()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If key = 0 Then
            MsgBox("Select Item To Delete")
        Else
            Try
                con.open()
                Dim query = "delete from ItemTbl where ItId=" & key & ""
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, con)
                cmd.ExecuteNonQuery()
                MsgBox("Item Deleted Successfully")
                con.close()
                DisplayItem()
                Clear()
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ItnameTb.Text = "" Or QtyTb.Text = "" Or CatCb.SelectedIndex = -1 Or PriceTb.Text = "" Then
            MsgBox("Missing Information")
        Else
            Try
                ' Assuming "con" is your SqlConnection object
                con.Open()
                Dim query = "UPDATE ItemTbl SET ItName = @ItName, ItQty = @ItQty, ItPrice = @ItPrice, ItCat = @ItCat WHERE ItId = @ItId"
                Dim cmd As SqlCommand = New SqlCommand(query, con)

                ' Add parameters to avoid SQL injection and improve readability
                cmd.Parameters.AddWithValue("@ItName", ItnameTb.Text)
                cmd.Parameters.AddWithValue("@ItQty", Convert.ToInt32(QtyTb.Text))
                cmd.Parameters.AddWithValue("@ItPrice", Convert.ToDecimal(PriceTb.Text))
                cmd.Parameters.AddWithValue("@ItCat", CatCb.SelectedItem.ToString())
                cmd.Parameters.AddWithValue("@ItId", key) ' Assuming key is defined somewhere

                cmd.ExecuteNonQuery()
                MsgBox("Item Updated Successfully")
                con.Close()

                DisplayItem()
                Clear()
            Catch ex As Exception
                ' Handle exceptions appropriately, such as logging or displaying an error message
                MsgBox("Error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim obj = New Login
        obj.Show()
        Me.Hide()
    End Sub
    Private Sub SearchCb_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles SearchCb.SelectionChangeCommitted
        FilterByCat()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        DisplayItem()
    End Sub


    Private Sub ItnameTb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ItnameTb.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            ItnameTb.BackColor = Color.DodgerBlue
            MsgBox("Enter Only Letters")
        Else
            ItnameTb.BackColor = Color.White


        End If
    End Sub

    Private Sub QtyTb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles QtyTb.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            QtyTb.BackColor = Color.DodgerBlue
            MsgBox("Enter Only Numbers")
        Else
            QtyTb.BackColor = Color.White


        End If
    End Sub


    Private Sub PriceTb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PriceTb.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            PriceTb.BackColor = Color.DodgerBlue
            MsgBox("Enter Only Numbers")
        Else
            PriceTb.BackColor = Color.White
        End If
    End Sub
End Class