Imports System.IO
Public Class FrmArticulo
    'Variables para almacenar la imagen del artículo'
    Private RutaOrigen As String
    Private RutaDestino As String
    Private Directorio As String = "C:\sistema\"

    Private Sub Formato()
        'La columna con el check estará deshabilitada al inicio'
        DgvListado.Columns(0).Visible = False
        DgvListado.Columns(2).Visible = False
        DgvListado.Columns(0).Width = 100
        DgvListado.Columns(1).Width = 100
        DgvListado.Columns(3).Width = 100
        DgvListado.Columns(4).Width = 100
        DgvListado.Columns(5).Width = 150
        DgvListado.Columns(6).Width = 100
        DgvListado.Columns(7).Width = 100
        DgvListado.Columns(8).Width = 200
        DgvListado.Columns(9).Width = 100
        DgvListado.Columns(10).Width = 100


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
            Dim Neg As New Negocio.NArticulo
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
            Dim Neg As New Negocio.NArticulo
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
        TxtCodigo.Text = ""
        TxtPrecioVenta.Text = ""
        TxtStock.Text = ""
        txtImagen.Text = ""
        PicImagen.Image = Nothing
        RutaOrigen = ""
    End Sub

    Private Sub CargarCategoria()
        Try
            Dim Neg As New Negocio.NCategoria
            CboCategoria.DataSource = Neg.Seleccionar
            CboCategoria.ValueMember = "idcategoria"
            CboCategoria.DisplayMember = "nombre"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FrmArticulo_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Listar()
        Me.CargarCategoria()
    End Sub

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs)
        Me.Buscar()
    End Sub

    Private Sub BtnCargarImagen_Click(sender As Object, e As EventArgs) Handles btnCargarImagen.Click
        Dim file As New OpenFileDialog()
        file.Filter = "Image Files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
        If file.ShowDialog() = DialogResult.OK Then
            PicImagen.Image = Image.FromFile(file.FileName)
            RutaOrigen = file.FileName
            txtImagen.Text = file.FileName.Substring(file.FileName.LastIndexOf("\") + 1)
        End If

    End Sub

    Private Sub BtnInsertar_Click(sender As Object, e As EventArgs) Handles BtnInsertar.Click
        Try
            If Me.ValidateChildren = True And CboCategoria.Text <> "" And TxtNombre.Text <> "" And TxtPrecioVenta.Text <> "" And TxtStock.Text <> "" Then
                Dim Obj As New Entidades.Articulo
                Dim Neg As New Negocio.NArticulo

                Obj.IdCategoria = CboCategoria.SelectedValue
                Obj.Codigo = TxtCodigo.Text
                Obj.Nombre = TxtNombre.Text
                Obj.PrecioVenta = TxtPrecioVenta.Text
                Obj.Stock = TxtStock.Text
                Obj.Imagen = txtImagen.Text
                Obj.Descripcion = TxtDescripcion.Text

                If (Neg.Insertar(Obj)) Then
                    MsgBox("Se ha registrado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                    If (txtImagen.Text <> "") Then
                        RutaDestino = Directorio & txtImagen.Text
                        File.Copy(RutaOrigen, RutaDestino)
                    End If

                    Me.Listar()
                Else
                    MsgBox("No se ha podido registrar", vbOKOnly + vbCritical, "Proceso fallido")
                End If

            Else
                MsgBox("Rellene todos los campos obligatorios (*)", vbOKOnly + vbCritical, "¡Advertencia!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.Limpiar()
        TabGeneral.SelectedIndex = 0
    End Sub

    Private Sub DgvListado_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvListado.CellDoubleClick
        Try
            TxtId.Text = DgvListado.SelectedCells.Item(1).Value
            CboCategoria.SelectedValue = DgvListado.SelectedCells.Item(2).Value
            TxtCodigo.Text = DgvListado.SelectedCells.Item(4).Value
            TxtNombre.Text = DgvListado.SelectedCells.Item(5).Value
            TxtPrecioVenta.Text = DgvListado.SelectedCells.Item(6).Value
            TxtStock.Text = DgvListado.SelectedCells.Item(7).Value
            TxtDescripcion.Text = DgvListado.SelectedCells.Item(8).Value
            Dim Imagen As String
            Imagen = DgvListado.SelectedCells.Item(9).Value
            If (Imagen <> "") Then
                PicImagen.Image = Image.FromFile(Directorio & Imagen)
                txtImagen.Text = Imagen
            End If
            BtnInsertar.Visible = False
            BtnActualizar.Visible = True
            TabGeneral.SelectedIndex = 1
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnActualizar_Click(sender As Object, e As EventArgs) Handles BtnActualizar.Click
        Try
            If Me.ValidateChildren = True And CboCategoria.Text <> "" And TxtNombre.Text <> "" And TxtPrecioVenta.Text <> "" And TxtStock.Text <> "" And TxtId.Text <> "" Then
                Dim Obj As New Entidades.Articulo
                Dim Neg As New Negocio.NArticulo

                Obj.IdArticulo = TxtId.Text
                Obj.IdCategoria = CboCategoria.SelectedValue
                Obj.Codigo = TxtCodigo.Text
                Obj.Nombre = TxtNombre.Text
                Obj.PrecioVenta = TxtPrecioVenta.Text
                Obj.Stock = TxtStock.Text
                Obj.Imagen = txtImagen.Text
                Obj.Descripcion = TxtDescripcion.Text

                If (Neg.Actualizar(Obj)) Then
                    MsgBox("Se ha actualizado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                    If (txtImagen.Text <> "" And RutaOrigen <> "") Then
                        RutaDestino = Directorio & txtImagen.Text
                        File.Copy(RutaOrigen, RutaDestino)
                    End If

                    Me.Listar()
                Else
                    MsgBox("No se ha podido actualizar", vbOKOnly + vbCritical, "Proceso fallido")
                End If

            Else
                MsgBox("Rellene todos los campos obligatorios (*)", vbOKOnly + vbCritical, "¡Advertencia!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnBuscar_Click_1(sender As Object, e As EventArgs) Handles BtnBuscar.Click
        Me.Buscar()
    End Sub

    Private Sub DgvListado_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvListado.CellContentClick
        If e.ColumnIndex = DgvListado.Columns.Item("Seleccionar").Index Then
            Dim chkcell As DataGridViewCheckBoxCell = DgvListado.Rows(e.RowIndex).Cells("Seleccionar")
            chkcell.Value = Not chkcell.Value
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

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If (MsgBox("¿Está seguro que desea eliminar los registros seleccionados?", vbYesNo + vbQuestion, "Eliminar registros") = vbYes) Then
            Try
                Dim Neg As New Negocio.NArticulo
                For Each row As DataGridViewRow In DgvListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Seleccionar").Value)
                    If marcado Then
                        Dim oneKey As Integer = Convert.ToInt32(row.Cells("ID").Value)
                        Dim Imagen As String = Convert.ToString(row.Cells("Imagen").Value)
                        Neg.Eliminar(oneKey)
                        File.Delete(Directorio & Imagen)
                    End If
                Next
                MsgBox("Se ha eliminado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                Me.Listar()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub BtnActivar_Click(sender As Object, e As EventArgs) Handles BtnActivar.Click
        If (MsgBox("¿Está seguro que desea activar los registros seleccionados?", vbYesNo + vbQuestion, "Eliminar registros") = vbYes) Then
            Try
                Dim Neg As New Negocio.NArticulo
                For Each row As DataGridViewRow In DgvListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Seleccionar").Value)
                    If marcado Then
                        Dim oneKey As Integer = Convert.ToInt32(row.Cells("ID").Value)
                        Neg.Activar(oneKey)
                    End If
                Next
                MsgBox("Se ha eliminado correctamente", vbOKOnly + vbInformation, "Proceso exitoso")
                Me.Listar()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub BtnDesactivar_Click(sender As Object, e As EventArgs) Handles BtnDesactivar.Click
        If (MsgBox("¿Está seguro que desea desactivar los registros seleccionados?", vbYesNo + vbQuestion, "Eliminar registros") = vbYes) Then
            Try
                Dim Neg As New Negocio.NArticulo
                For Each row As DataGridViewRow In DgvListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Seleccionar").Value)
                    If marcado Then
                        Dim oneKey As Integer = Convert.ToInt32(row.Cells("ID").Value)
                        Neg.Desactivar(oneKey)
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