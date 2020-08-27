Public Class FrmCategoria
    'Creación de métodos'

    'Método para definir el formato de las columnas que mostraré en el listado'
    Private Sub Formato()
        'La columna con el check estará deshabilitada al inicio'
        DgvListado.Columns(0).Visible = False
        DgvListado.Columns(0).Width = 100
        DgvListado.Columns(1).Width = 100
        DgvListado.Columns(2).Width = 150
        DgvListado.Columns(3).Width = 400
        DgvListado.Columns(4).Width = 100

        DgvListado.Columns.Item("Seleccionar").Visible = False
        BtnEliminar.Visible = False
        BtnInsertar.Visible = False
        BtnActivar.Visible = False
        BtnDesactivar.Visible = False
        ChkSeleccionar.CheckState = False
    End Sub

    'Método de listar'
    Private Sub Listar()
        Try
            Dim Neg As New Negocio.NCategoria
            DgvListado.DataSource = Neg.Listar()
            Lbltotal.Text = "Total Registros: " & DgvListado.DataSource.Rows.Count
            Me.Formato()
            Me.Limpiar()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    'Método de buscar'
    Private Sub Buscar()
        Try
            Dim Neg As New Negocio.NCategoria
            Dim Valor As String
            Valor = Txtvalor.Text
            DgvListado.DataSource = Neg.Buscar(Valor)
            Lbltotal.Text = "Total Registros: " & DgvListado.DataSource.Rows.Count
            Me.Formato()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    'Método limpiar'
    Private Sub Limpiar()
        BtnInsertar.Visible = True
        BtnActualizar.Visible = False
        Txtvalor.Text = ""
        TxtId.Text = ""
        TxtNombre.Text = ""
        TxtDescripcion.Text = ""
    End Sub


    Private Sub FrmCategoria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Listar()
    End Sub

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs) Handles BtnBuscar.Click
        Me.Buscar()
    End Sub

    Private Sub BtnInsertar_Click(sender As Object, e As EventArgs) Handles BtnInsertar.Click
        If Me.ValidateChildren = True And TxtNombre.Text <> "" Then
            Dim Obj As New Entidades.Categoria
            Dim Neg As New Negocio.NCategoria

            Obj.Nombre = TxtNombre.Text
            Obj.Descripcion = TxtDescripcion.Text

            If (Neg.Insertar(Obj)) Then
                MsgBox("Se ha registrado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                Me.Listar()
            Else
                MsgBox("No se ha podido registrar", vbOKOnly + vbCritical, "Proceso fallido")
            End If

        Else
            MsgBox("Rellene todos los campos obligatorios (*)", vbOKOnly + vbCritical, "¡Advertencia!")
        End If
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.Limpiar()
        'Luego de limpiar que me lleve al indice en el cual está el listado'
        TabGeneral.SelectedIndex = 0
    End Sub

    Private Sub TxtNombre_Validated(sender As Object, e As EventArgs) Handles TxtNombre.Validated
        If DirectCast(sender, TextBox).Text.Length > 0 Then
            Me.ErrorIcono.SetError(sender, "")
        Else
            Me.ErrorIcono.SetError(sender, "Ingrese el nombre de la categoría por favor, este dato es obligatorio")
        End If
    End Sub

    Private Sub DgvListado_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvListado.CellDoubleClick
        TxtId.Text = DgvListado.SelectedCells.Item(1).Value
        TxtNombre.Text = DgvListado.SelectedCells.Item(2).Value
        TxtDescripcion.Text = DgvListado.SelectedCells.Item(3).Value
        BtnInsertar.Visible = False
        BtnActualizar.Visible = True
        BtnActivar.Visible = False
        TabGeneral.SelectedIndex = 1
    End Sub

    Private Sub BtnActualizar_Click(sender As Object, e As EventArgs) Handles BtnActualizar.Click
        If Me.ValidateChildren = True And TxtNombre.Text <> "" And TxtId.Text <> "" Then
            Dim Obj As New Entidades.Categoria
            Dim Neg As New Negocio.NCategoria

            Obj.IdCategoria = TxtId.Text
            Obj.Nombre = TxtNombre.Text
            Obj.Descripcion = TxtDescripcion.Text

            If (Neg.Actualizar(Obj)) Then
                MsgBox("Se ha actualizado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                Me.Listar()
                TabGeneral.SelectedIndex = 0
            Else
                MsgBox("No se ha podido actualizar", vbOKOnly + vbCritical, "Proceso fallido")
            End If

        Else
            MsgBox("Rellene todos los campos obligatorios (*)", vbOKOnly + vbCritical, "¡Advertencia!")
        End If
    End Sub

    Private Sub ChkSeleccionar_CheckedChanged(sender As Object, e As EventArgs) Handles ChkSeleccionar.CheckedChanged
        If ChkSeleccionar.CheckState = CheckState.Checked Then
            DgvListado.Columns.Item("Seleccionar").Visible = True
            BtnEliminar.Visible = True
            BtnInsertar.Visible = True
            BtnActivar.Visible = True
            BtnDesactivar.Visible = True
        Else
            DgvListado.Columns.Item("Seleccionar").Visible = False
            BtnEliminar.Visible = False
            BtnInsertar.Visible = False
            BtnDesactivar.Visible = False
        End If
    End Sub

    Private Sub DgvListado_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvListado.CellContentClick
        If e.ColumnIndex = DgvListado.Columns.Item("Seleccionar").Index Then
            Dim chkcell As DataGridViewCheckBoxCell = DgvListado.Rows(e.RowIndex).Cells("Seleccionar")
            chkcell.Value = Not chkcell.Value
        End If
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If (MsgBox("¿Estás seguro de eliminar los registros seleccionados", vbYesNo + vbQuestion, "Eliminar registro") = vbYes) Then
            Try
                Dim Neg As New Negocio.NCategoria
                For Each row As DataGridViewRow In DgvListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Seleccionar").Value)
                    If marcado Then
                        Dim oneKey As Integer = Convert.ToInt32(row.Cells("ID").Value)
                        Neg.Eliminar(oneKey)
                    End If
                Next
                MsgBox("Se ha eliminado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                Me.Listar()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
End Class