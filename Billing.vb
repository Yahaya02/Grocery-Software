

Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.Text.RegularExpressions

Public Class Billing
    Private Sub AddBill()
        Try
            con.Open()
            Dim query As String = "INSERT INTO BillTbl (ClientName,Amount,BDate) VALUES (@ClientName, @TotalAmount, @BillDate)"
            Dim cmd As SqlCommand = New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@ClientName", ClNameTb.Text)
            cmd.Parameters.AddWithValue("@TotalAmount", GrdTotal)
            cmd.Parameters.AddWithValue("@BillDate", DateTime.Today.Date)
            cmd.ExecuteNonQuery()
            MsgBox("Bill Saved Successfully")
            con.Close()
            TotalLbl.Text = "Total"
            BillDGV.Rows.Clear()
            ' Not sure what this line is for, you may need to adjust it as needed
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Private Sub UpdateItem()
        Dim newQty = stock - Convert.ToInt32(QtyTb.Text)
        Try
            con.open()
            Dim query = "Update ItemTbl set ItQty=" & newQty & "where ItId=" & key & ""
            Dim cmd As SqlCommand
            cmd = New SqlCommand(query, con)
            cmd.ExecuteNonQuery()
            MsgBox("Item Updated Successfully")
            con.closoe()
            DisplayItem()
        Catch ex As Exception

        End Try
    End Sub
    Dim i = 0, GrdTotal = 0
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ItNameTb.Text = "" Then
            MsgBox("Select The Item")
        ElseIf ClNameTb.Text = "" Or QtyTb.Text = "" Then
            MsgBox("Enter Quantity and Client Name")
        Else
            Dim selectedQty As Integer = Convert.ToInt32(QtyTb.Text)
            Dim unitPrice As Integer = Convert.ToInt32(PriceTb.Text)
            Dim total As Integer = selectedQty * unitPrice

            If selectedQty > stock Then
                MsgBox("Not enough stock available.")
            Else
                Dim rnum As Integer = BillDGV.Rows.Add()
                i = i + 1

                ' Populate DataGridView with item details
                BillDGV.Rows.Item(rnum).Cells("Column1").Value = i
                BillDGV.Rows.Item(rnum).Cells("Column2").Value = ItNameTb.Text
                BillDGV.Rows.Item(rnum).Cells("Column3").Value = unitPrice
                BillDGV.Rows.Item(rnum).Cells("Column4").Value = selectedQty
                BillDGV.Rows.Item(rnum).Cells("Column5").Value = total

                GrdTotal = GrdTotal + total
                TotalLbl.Text = "GH" + Convert.ToString(GrdTotal)

                ' Subtract the purchased quantity from stock in the ItemTbl
                Try
                    con.Open()
                    Dim newQty = stock - selectedQty ' Calculate the new quantity
                    Dim query = "UPDATE ItemTbl SET ItQty = @NewQty WHERE ItId = @ItemId"
                    Dim cmd As SqlCommand = New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@NewQty", newQty)
                    cmd.Parameters.AddWithValue("@ItemId", key)
                    cmd.ExecuteNonQuery()
                    con.Close()

                    ' Refresh the display of items in ItemDGV
                    DisplayItem()
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

                Reset()
            End If
        End If

    End Sub

    Dim con = New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\FONZY D POCKET\Documents\GroceyDbVb.mdf;Integrated Security=True;Connect Timeout=30")
    Private Sub DisplayItem()
        Try
            Debug.WriteLine("Opening connection...")
            If con.State <> ConnectionState.Open Then
                con.Open()
            End If
            Debug.WriteLine("Connection opened successfully.")

            Dim query = "SELECT * FROM ItemTbl"
            Dim cmd As New SqlCommand(query, con)
            Dim adapter As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            adapter.Fill(ds)
            ItemDGV.DataSource = ds.Tables(0)

            con.Close() ' Close the connection after filling the data
        Catch ex As SqlException
            MsgBox("SQL Error: " & ex.Message)
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub



    Private Sub Billing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayItem()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        TotalLbl.Text = "TotalLbl"
        BillDGV.Rows.Clear()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Application.Exit()
    End Sub
    Dim key = 0, stock = 0
    Private Sub Reset()
        ItNameTb.Text = ""
        PriceTb.Text = ""
        QtyTb.Text = ""
        'This is the reason why the price wouldn't show' never uncoment it'    (TotalLbl.Text = "Total")
        key = 0
        stock = 0
    End Sub
    Private Sub Button4_click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim obj = New Login
        obj.Show()
        Me.Hide()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click, Button4.Click
        If ClNameTb.Text = "" Then
            MsgBox("Enter Client Name")
        Else
            AddBill()

            ' Create a new instance of PrintPreviewDialog
            Dim printPreviewDialog As New PrintPreviewDialog()

            ' Assign the PrintDocument to the PrintPreviewDialog
            printPreviewDialog.Document = PrintDocument1

            ' Show the PrintPreviewDialog
            printPreviewDialog.ShowDialog()

            ' Dispose of the PrintPreviewDialog after use
            printPreviewDialog.Dispose()
        End If
    End Sub


    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawString("Grocery Shop", New Font("Century Gothic", 25, FontStyle.Bold), Brushes.MidnightBlue, New Point(100, 50))
        e.Graphics.DrawString("=====Your Bill====", New Font("Century Gothic", 16), Brushes.MidnightBlue, New Point(100, 100))

        ' Draw the DataGridView content
        Dim bm As New Bitmap(BillDGV.Width, BillDGV.Height)
        BillDGV.DrawToBitmap(bm, New Rectangle(0, 0, BillDGV.Width, BillDGV.Height))
        e.Graphics.DrawImage(bm, New Point(100, 130))

        ' Calculate and draw the total amount
        Dim totalAmountString As String = "Total Amount GH " + Decimal.Parse(GrdTotal).ToString("N2")
        ' Dim totalAmountString As String = "Total Amount GH " + GrdTotal.ToString("N2")
        'Dim totalAmountString As String = $"Total Amount GH {GrdTotal.ToString("N2")}" ' Example: "Total Amount GH 100.00"
        e.Graphics.DrawString(totalAmountString, New Font("Century Gothic", 16), Brushes.MidnightBlue, New Point(100, 130 + BillDGV.Height + 20))

        e.Graphics.DrawString("====Thanks For Buying In Our Shop====", New Font("Century Gothic", 16), Brushes.MidnightBlue, New Point(100, 130 + BillDGV.Height + 50))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Reset()
    End Sub

    Private Sub ItemDGV_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ItemDGV.CellMouseClick
        Dim row As DataGridViewRow = ItemDGV.Rows(e.RowIndex)
        ItNameTb.Text = row.Cells(1).Value.ToString

        PriceTb.Text = row.Cells(3).Value.ToString

        If ItNameTb.Text = "" Then
            key = 0
        Else
            key = Convert.ToInt32(row.Cells(0).Value.ToString)
            stock = Convert.ToInt32(row.Cells(2).Value.ToString)
        End If
    End Sub

    Private Sub ClNameTb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ClNameTb.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            ClNameTb.BackColor = Color.DodgerBlue
        Else
            ClNameTb.BackColor = Color.White


        End If
    End Sub



    Private Sub QtyTb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles QtyTb.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            QtyTb.BackColor = Color.DodgerBlue
        Else
            QtyTb.BackColor = Color.White


        End If
    End Sub

    Private Sub TextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ClNameTb.KeyPress, QtyTb.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.SelectNextControl(CType(sender, Control), True, True, True, True)
        End If
    End Sub

End Class