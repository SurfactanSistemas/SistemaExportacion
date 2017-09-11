Imports System.Data.SqlClient

Public Class Proforma


    Private Const YMARGEN = 1.5
    Private Const XMARGEN = 4.9
    Private WRow, Wcol As Integer
    
    Private Sub Proforma_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        dgvProductos.Rows.Clear()

        For i = 0 To 4
            dgvProductos.Rows.Add("", "", "", "", "")
        Next

        dgvProductos.ClearSelection()

        txtFechaAux.Visible = False

        WRow = -1
        Wcol = -1

    End Sub

    Private Sub Proforma_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtNroProforma.Focus()
    End Sub

    Private Sub txtNroProforma_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNroProforma.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtNroProforma.Text) = "" Then : Exit Sub : End If

            txtFecha.Focus()

        ElseIf e.KeyData = Keys.Escape Then
            txtNroProforma.Text = ""
        End If

    End Sub

    Private Sub txtFecha_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFecha.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtFecha.Text.Replace("/", "")) = "" Then : Exit Sub : End If

            If Helper._ValidarFecha(txtFecha.Text) Then
                txtCliente.Focus()
            End If

        ElseIf e.KeyData = Keys.Escape Then
            txtFecha.Text = ""
        End If

    End Sub

    Private Sub txtCliente_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCliente.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtCliente.Text) = "" Then : Exit Sub : End If

            Dim cliente = _BuscarCliente(txtCliente.Text)

            If Not IsNothing(cliente) Then
                ' Guardar y llenar los datos que se puedan extraer del registro de cliente.
                txtCliente.Text = cliente("Cliente")
                txtDescripcionCliente.Text = cliente("Razon")
                txtDireccionCliente.Text = cliente("Direccion")
                txtLocalidadCliente.Text = cliente("Localidad")

                txtCondicionPago.Focus()
            Else
                MsgBox("Cliente inexistente.", MsgBoxStyle.Information)
                txtCliente.Focus()
            End If

        ElseIf e.KeyData = Keys.Escape Then
            txtCliente.Text = ""
        End If

    End Sub

    Private Function _BuscarCliente(ByVal Codigo As String) As DataRow
        Dim resultados As New DataTable
        Dim cn As SqlConnection = New SqlConnection()
        Dim cm = "SELECT cliente, Razon, Direccion, Localidad FROM Cliente WHERE Cliente = '" & Codigo.Trim() & "'"
        Dim dr As New SqlDataAdapter(cm, cn)

        Try
            cn.ConnectionString = Helper._ConectarA("SurfactanSA")
            cn.Open()

            dr.Fill(resultados)

        Catch ex As Exception
            MsgBox("Hubo un problema al querer consultar la Base de Datos.", MsgBoxStyle.Critical)
        Finally

            dr = Nothing
            cn.Close()
            cn = Nothing
            cm = Nothing

        End Try

        If resultados.Rows.Count > 0 Then
            Return Helper._NormalizarFila(resultados.Rows(0))
        Else
            Return Nothing
        End If

    End Function

    Private Sub txtDireccionCliente_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDireccionCliente.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtDireccionCliente.Text) = "" Then : Exit Sub : End If

            txtLocalidadCliente.Focus()

        ElseIf e.KeyData = Keys.Escape Then
            txtDireccionCliente.Text = ""
        End If

    End Sub

    Private Sub txtLocalidadCliente_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLocalidadCliente.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtLocalidadCliente.Text) = "" Then : Exit Sub : End If

            txtCondicionPago.Focus()

        ElseIf e.KeyData = Keys.Escape Then
            txtLocalidadCliente.Text = ""
        End If

    End Sub

    Private Sub txtCondicionPago_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCondicionPago.KeyDown, TextBox1.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtCondicionPago.Text) = "" Then : Exit Sub : End If

            txtOCCliente.Focus()

        ElseIf e.KeyData = Keys.Escape Then
            txtCondicionPago.Text = ""
        End If

    End Sub

    Private Sub txtOCCliente_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOCCliente.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtOCCliente.Text) = "" Then : Exit Sub : End If

            cmbCondicion.Focus()
            cmbCondicion.DroppedDown = True

        ElseIf e.KeyData = Keys.Escape Then
            txtOCCliente.Text = ""
        End If

    End Sub

    Private Sub cmbCondicion_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCondicion.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(cmbCondicion.SelectedItem) = "" Then : Exit Sub : End If

            txtVia.Focus()

        ElseIf e.KeyData = Keys.Escape Then
            cmbCondicion.SelectedIndex = 0
        End If

    End Sub

    Private Sub cmbCondicion_DropDownClosed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCondicion.DropDownClosed
        If cmbCondicion.SelectedIndex > 0 Then
            txtVia.Focus()
        End If
    End Sub

    Private Sub txtVia_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtVia.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtVia.Text) = "" Then : Exit Sub : End If

            dgvProductos.CurrentCell = dgvProductos.Rows(0).Cells(0)
            dgvProductos.Focus()

        ElseIf e.KeyData = Keys.Escape Then
            txtVia.Text = ""
        End If

    End Sub

    'With txtCodigo
    '    .CurrentCell = .Rows(iRow).Cells(iCol + 1)

    '    Dim _location As Point = .GetCellDisplayRectangle(2, iRow, False).Location

    '    .ClearSelection()
    '    _location.Y += .Location.Y + (.CurrentCell.Size.Height / 4) - YMARGEN
    '    _location.X += .Location.X + (.CurrentCell.Size.Width - txtFechaAux.Size.Width) - XMARGEN
    '    txtFechaAux.Location = _location
    '    txtFechaAux.Text = .Rows(iRow).Cells(2).Value
    '    WRow = iRow
    '    Wcol = iCol
    '    txtFechaAux.Visible = True
    '    txtFechaAux.Focus()
    'End With

    Private Function _BuscarTerminado(ByVal terminado As String) As DataRow
        Dim resultados As New DataTable
        Dim cn As SqlConnection = New SqlConnection()
        Dim cm = "SELECT * FROM Terminado WHERE Codigo = '" & terminado.Trim() & "'"
        Dim dr As New SqlDataAdapter(cm, cn)

        Try

            cn.ConnectionString = Helper._ConectarA("SurfactanSA")
            cn.Open()

            dr.Fill(resultados)

        Catch ex As Exception
            MsgBox("Hubo un problema al querer consultar la Base de Datos.", MsgBoxStyle.Critical)
        Finally

            dr = Nothing
            cn.Close()
            cn = Nothing
            cm = Nothing

        End Try

        If resultados.Rows.Count > 0 Then
            Return Helper._NormalizarFila(resultados.Rows(0))
        Else
            Return Nothing
        End If

    End Function

    Private Sub txtFechaAux_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFechaAux.KeyDown

        If e.KeyData = Keys.Enter Then
            If Trim(txtFechaAux.Text.Replace("-", "")) = "" Then : Exit Sub : End If

            If WRow >= 0 And Wcol >= 0 Then

                With dgvProductos
                    .Rows(WRow).Cells(0).Value = txtFechaAux.Text

                    Dim terminado = _BuscarTerminado(txtFechaAux.Text)

                    If Not IsNothing(terminado) Then
                        .Rows(WRow).Cells(0).Value = terminado("Codigo")
                        .Rows(WRow).Cells(1).Value = terminado("Descripcion")

                        .CurrentCell = .Rows(WRow).Cells(2)
                        .Focus()

                        txtFechaAux.Visible = False
                        txtFechaAux.Location = New Point(680, 390) ' Lo reubicamos lejos de la grilla.
                    Else
                        txtFechaAux.Focus()
                    End If
                End With

            End If

        ElseIf e.KeyData = Keys.Escape Then
            txtFechaAux.Text = ""
        End If

    End Sub

    Private Function _EsNumero(ByVal keycode As Integer) As Boolean
        Return (keycode >= 48 And keycode <= 57) Or (keycode >= 96 And keycode <= 105)
    End Function

    Private Function _EsControl(ByVal keycode) As Boolean
        Dim valido As Boolean = False

        Select Case keycode
            Case Keys.Enter, Keys.Escape, Keys.Right, Keys.Left, Keys.Back
                valido = True
            Case Else
                valido = False
        End Select

        Return valido
    End Function

    Private Function _EsDecimal(ByVal keycode As Integer) As Boolean
        Return (keycode >= 48 And keycode <= 57) Or (keycode >= 96 And keycode <= 105) Or (keycode = 110 Or keycode = 190)
    End Function

    Private Function _EsNumeroOControl(ByVal keycode) As Boolean
        Dim valido As Boolean = False

        If _EsNumero(CInt(keycode)) Or _EsControl(keycode) Then
            valido = True
        Else
            valido = False
        End If

        Return valido
    End Function

    Private Function _EsDecimalOControl(ByVal keycode) As Boolean
        Dim valido As Boolean = False

        If _EsDecimal(CInt(keycode)) Or _EsControl(keycode) Then
            valido = True
        Else
            valido = False
        End If

        Return valido
    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        With dgvProductos
            If .Focused Or .IsCurrentCellInEditMode Then ' Detectamos los ENTER tanto si solo estan en foco o si estan en edición una celda.
                .CommitEdit(DataGridViewDataErrorContexts.Commit) ' Guardamos todos los datos que no hayan sido confirmados.

                Dim iCol = .CurrentCell.ColumnIndex
                Dim iRow = .CurrentCell.RowIndex
                Dim valor = .CurrentCell.Value

                ' Limitamos los caracteres permitidos para cada una de las columnas.
                Select Case iCol
                    'Case 1
                    'If Not _EsNumeroOControl(keyData) Then
                    '    Return True
                    'End If
                    Case 1
                        If Not _EsDecimalOControl(keyData) Then
                            Return True
                        End If
                    Case Else

                End Select

                If msg.WParam.ToInt32() = Keys.Enter Then

                    If valor <> "" Then

                        Select Case iCol
                            Case 2, 3
                                _RecalcularTotalFila(iRow)
                            Case Else

                        End Select

                        _NormalizarNumerosGrilla()

                    End If

                    Select Case iCol
                        Case 4
                            If iRow = 4 Then
                                .CurrentCell = .Rows(iRow).Cells(iCol)
                            Else
                                .CurrentCell = .Rows(iRow + 1).Cells(0)
                            End If

                        Case Else
                            .CurrentCell = .Rows(iRow).Cells(iCol + 1)
                    End Select

                    Return True

                ElseIf msg.WParam.ToInt32() = Keys.Escape Then
                    .Rows(iRow).Cells(iCol).Value = ""

                    If iCol = 4 Then
                        .CurrentCell = .Rows(iRow).Cells(iCol - 1)
                    Else
                        .CurrentCell = .Rows(iRow).Cells(iCol + 1)
                    End If

                    .CurrentCell = .Rows(iRow).Cells(iCol)
                End If
            End If

        End With

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub dgvProductos_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvProductos.CellEnter
        With dgvProductos
            If e.ColumnIndex = 0 Then

                Dim _location As Point = .GetCellDisplayRectangle(0, e.RowIndex, False).Location

                _location.Y += .Location.Y + (.CurrentCell.Size.Height / 4) - YMARGEN
                _location.X += .Location.X + (.CurrentCell.Size.Width - txtFechaAux.Size.Width) - XMARGEN
                txtFechaAux.Location = _location
                txtFechaAux.Text = .Rows(e.RowIndex).Cells(0).Value
                WRow = e.RowIndex
                Wcol = e.ColumnIndex
                txtFechaAux.Visible = True
                txtFechaAux.Focus()
                .ClearSelection()
            End If
        End With
    End Sub

    Private Sub _RecalcularTotalFila(ByVal iRow As Integer)

        Dim WTotal = 0.0

        With dgvProductos.Rows(iRow)
            WTotal += Val(.Cells(2).Value)
            WTotal *= Val(.Cells(3).Value)

            .Cells(4).Value = Helper.formatonumerico(WTotal)
        End With

    End Sub

    Private Sub _NormalizarNumerosGrilla()
        Dim WTotal = 0.0

        For Each row As DataGridViewRow In dgvProductos.Rows
            With row
                .Cells(2).Value = IIf(.Cells(2).Value <> "", Helper.formatonumerico(.Cells(2).Value), "")
                .Cells(3).Value = IIf(.Cells(3).Value <> "", Helper.formatonumerico(.Cells(3).Value), "")
                '.Cells(4).Value = IIf(.Cells(4).Value <> "", Helper.formatonumerico(.Cells(4).Value), "")
                WTotal += (Val(.Cells(2).Value) * Val(.Cells(3).Value))
            End With
        Next

        txtTotal.Text = Helper.formatonumerico(WTotal)
    End Sub

End Class