Public Class orderFrm

    'Delcare variables
    Private m_coffeePrice As Double
    Private m_tax As Double = 0.09
    Dim coffeeList As New List(Of Coffee)
    Dim tempCoffee As New Coffee
    Dim coffees As Collection = Controller.getCoffeeDB
    Dim cboCoffees As New ArrayList
    Dim lblCoffees As New ArrayList
    Dim lblCoffeePrices As New ArrayList
    Dim txtPrices As New ArrayList
    Dim nudQtys As New ArrayList
    Dim txtTotals As New ArrayList
    Dim totalArray As New ArrayList

    'This will clear the entire form to allow for a new order to be added.
    Private Sub btnNewOrder_Click(sender As System.Object, e As System.EventArgs) Handles btnNewOrder.Click
        'clear controls
        Dim cbo As ComboBox
        Dim txt As TextBox
        Dim txt2 As TextBox
        Dim qty As NumericUpDown
        
        Dim last = cboCoffees.Count - 1
        Dim index As Integer
        For index = last To 1 Step -1
            cbo = cboCoffees(index)
            cboCoffees.RemoveAt(index)
            cbo.Dispose()

            txt = txtPrices(index)
            txtPrices.RemoveAt(index)
            txt.Dispose()
            
            txt2 = txtTotals(index)
            txtTotals.RemoveAt(index)
            txt2.Dispose()      

            qty = nudQtys(index)
            nudQtys.RemoveAt(index)
            qty.Dispose
            
        Next
        
        cboCoffee.Select()
        cboCoffee.SelectedIndex = -1
        txtFoodTotal.Clear()
        txtSalesTax.Clear()
        txtOrderTotal.Clear()
        txtPrice.Clear()
        txtTotal.Clear()
        nudQty.Value = 1

        'update the order number automatically
        frmMain.orderNumber = CInt(txtOrderNum.Text) + 1
        txtOrderNum.Text = (frmMain.orderNumber).tostring
        Me.Text = "Order Number: " + frmMain.orderNumber.tostring
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    'handles loading everything for the menu
    Private Sub orderFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.InitializeComboBox(cboCoffee)
        Me.InitializeTextBox(txtPrice)
        Me.InitializeNumericUpDown(nudQty)
        Me.InitializeTextBox2(txtTotal)

        For Each coffee In coffees
            coffeeList.Add(coffee)
        Next

        cboCoffees.Add(cboCoffee)
        txtPrices.Add(txtPrice)
        nudQtys.Add(nudQty)
        txtTotals.Add(txtTotal)

        addCoffeeList()
        populateList()

        'updates the order number
        txtOrderNum.Text = (frmMain.orderNumber).tostring
        Me.Text = "Order Number: " + frmMain.orderNumber.tostring
    End Sub

    'adds items to the coffeelist
    Public Sub addCoffeeList()
        For Each Coffee As Coffee In coffeeList
            cboCoffee.Items.Add(Coffee.name)
        Next
    End Sub

    'populates the coffee menu
    Public Sub populateList()
        For Each Coffee As Coffee In coffeeList
            lblCoffeeList.Text += Coffee.name + vbCrLf
            lblCoffeePrice.Text += FormatCurrency(Coffee.price) + vbCrLf
        Next
    End Sub

    'adds all of the events so that you can add runtime created field to the menu
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Add a new row
        Dim cbo As New ComboBox

        cbo.Left = cboCoffee.Left
        cbo.Size = cboCoffee.Size
        cbo.Tag = cboCoffee.Tag
        cbo.DropDownStyle = cboCoffee.DropDownStyle

        Dim n1 As Integer = cboCoffees.Count - 1
        cbo.Top = cboCoffees(n1).top + 25

        ' Load values into combo box
        Me.InitializeComboBox(cbo)

        ' Add combo box to array list and panel
        cboCoffees.Add(cbo)
        cbo.Tag = cboCoffees.Count - 1
        Me.pnlOrder.Controls.Add(cbo)

        'Register as event listener
        AddHandler cbo.SelectedIndexChanged, AddressOf cboCoffee_SelectedIndexChanged1

        ' Add a new price text box
        Dim txt As New TextBox

        txt.Left = txtPrice.Left
        txt.Size = txtPrice.Size
        txt.ReadOnly = txtPrice.ReadOnly
        txt.Enabled = txtPrice.Enabled
        Dim n2 As Integer = txtPrices.Count - 1
        txt.Top = txtPrices(n2).top + 25
        txt.TextAlign = HorizontalAlignment.Right

        ' Add text box to array list and panel
        txtPrices.Add(txt)
        txt.Tag = txtPrices.Count - 1
        Me.pnlOrder.Controls.Add(txt)

        Dim nud As New NumericUpDown
        nud.Left = nudQty.Left
        nud.Size = nudQty.Size
        nud.Minimum = 1
        nud.ReadOnly = nudQty.ReadOnly

        Dim n3 As Integer = nudQtys.Count - 1
        nud.Top = nudQtys(n3).top + 25
        nud.TextAlign = HorizontalAlignment.Center
        nudQtys.Add(nud)
        nud.Tag = nudQtys.Count - 1
        Me.pnlOrder.Controls.Add(nud)

        AddHandler nud.ValueChanged, AddressOf nudQty_ValueChanged1

        'create new total textbox 
        Dim txt2 As New TextBox

        'assign values of default to generated total text box
        txt2.Left = txtTotal.Left
        txt2.Size = txtTotal.Size
        txt2.ReadOnly = txtTotal.ReadOnly
        txt2.Enabled = txtTotal.Enabled
        'add the text box
        Dim n4 As Integer = txtTotals.Count - 1
        txt2.Top = txtTotals(n4).top + 25
        txt2.TextAlign = HorizontalAlignment.Right

        txtTotals.Add(txt2)
        txt.Tag = txtTotals.Count - 1
        Me.pnlOrder.Controls.Add(txt2)

    End Sub

    'initializes comboboxs 
    Public Function InitializeComboBox(ByVal cbo As ComboBox) As ComboBox
        ' Intialize values
        For Each Coffee As Coffee In coffeeList
            cbo.Items.Add(Coffee.name)
        Next
        Return cbo
    End Function

    'initializes the price text boxs
    Public Function InitializeTextBox(ByVal txt As TextBox) As TextBox
        Return txt
    End Function

    'initializes the total text boxs
    Public Function InitializeTextBox2(ByVal txt As TextBox) As TextBox
        Return txt
    End Function

    'initializes a nud
    Public Function InitializeNumericUpDown(ByVal nud As NumericUpDown) As NumericUpDown
        Return nud
    End Function

    'handles what happens when user selects a different coffee.
    Private Sub cboCoffee_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCoffee.SelectedIndexChanged
        ' Add the item to the textbox on the right
        Try
            txtPrice.Text = FormatCurrency(coffeeList(cboCoffee.SelectedIndex).price.ToString)

            Dim total As Double
            Dim qty As Integer
            Dim price As Double
            price = coffeeList(cboCoffee.SelectedIndex).price
            qty = nudQty.Value
            total = price * qty

            txtTotal.Text = FormatCurrency(total.ToString)
        Catch

        End Try
    End Sub 

    'handles what happens when user selects a different coffee for runtime created fields.
    Private Sub cboCoffee_SelectedIndexChanged1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Sender.tag looks up item selected and sends corresponding price to textbox price
        Try
            txtPrices(sender.tag).text = FormatCurrency(coffeeList(sender.SelectedIndex).price.ToString)

            Dim total2 As Double
            Dim qty2 As Integer
            Dim price2 As Double
            price2 = coffeeList(cboCoffees(sender.tag).SelectedIndex).price
            qty2 = nudQtys(sender.tag).Value
            total2 = price2 * qty2

            txtTotals(sender.tag).Text = FormatCurrency(total2.ToString)

        Catch
        End Try
    End Sub 

    'removes one row of runtime created fields at a time 
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        ' Delete textboxes in reverse order of being added

        Dim cbo As ComboBox
        Dim nud As NumericUpDown
        Dim txt As TextBox
        Dim txt2 As TextBox
        Dim last = cboCoffees.Count - 1
        If last > 0 Then
            Dim index As Integer
            For index = last To last Step -1
                cbo = cboCoffees(index)
                cboCoffees.RemoveAt(index)
                cbo.Dispose()
                txt = txtPrices(index)
                txtPrices.RemoveAt(index)
                txt.Dispose()
                nud = nudQtys(index)
                nudQtys.RemoveAt(index)
                nud.Dispose()
                txt2 = txtTotals(index)
                txtTotals.RemoveAt(index)
                txt2.Dispose()
            Next
        End If
        cboCoffee.Select()
    End Sub 

    'changes the nudQty button and then changes the values for that item
    Private Sub nudQty_ValueChanged(sender As Object, e As EventArgs) Handles nudQty.ValueChanged
        Dim qty As Double
        qty = Double.Parse(nudQty.Value)

        Try
            Dim newTotal As Double
            newTotal = Double.Parse(coffeeList(cboCoffee.SelectedIndex).price) * qty
            txtTotal.Text = FormatCurrency(newTotal.ToString)
        Catch ex As Exception
            'do nothing
        End Try

    End Sub

    'changes one of the runtime added nudQty buttons and then changes the values for that item
    Private Sub nudQty_ValueChanged1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            Dim total2 As Double
            Dim qty2 As Integer
            Dim price2 As Double

            price2 = coffeeList(cboCoffees(sender.tag).SelectedIndex).price

            qty2 = nudQtys(sender.tag).Value
            total2 = price2 * qty2

            txtTotals(sender.tag).Text = FormatCurrency(total2.ToString)
        Catch
            'do nothing
        End Try
    End Sub
    
    'totals up the order
    Private Sub btnTotal_Click(sender As Object, e As EventArgs) Handles btnTotal.Click
        Dim subTotal As Double
        Dim firstTotal As Double
        Dim fullTotal As Double
        Dim tax As Double
        Dim finalTotal As Double

        If txtServer.TextLength = Nothing Then
            MessageBox.Show("Please enter a server name before totaling an order.")
        Else
            'Try
            '    firstTotal = CDbl(txtTotal.Text)
            'Catch
            'End Try


            For Each txt In txtTotals
                Try
                    subTotal += CDbl(txt.text)
                Catch

                End Try
            Next
            txtFoodTotal.Text = FormatCurrency(subTotal)
            fullTotal = subTotal + firstTotal
            tax = fullTotal * m_tax
            txtSalesTax.Text = FormatCurrency(tax.ToString)
            finalTotal = tax + fullTotal
            txtOrderTotal.Text = FormatCurrency(finalTotal.ToString)
        End If
    End Sub
End Class
