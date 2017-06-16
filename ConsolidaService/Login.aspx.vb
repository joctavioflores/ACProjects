Public Class Login
    Inherits System.Web.UI.Page
    Dim SQLProv As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If user_name.Value.Trim <> "" And password_field.Value.Trim <> "" Then
            SQLProv = Persistencia.GetDataTable("exec AbcUSUARIO 4,'','',0,'','" + user_name.Value.Trim + "','" + password_field.Value.Trim + "',0,0")
            If SQLProv.Rows.Count() > 0 Then
                If SQLProv.Rows(0).Item(0) = "0" Then
                    'El usuario existe.
                    Select Case SQLProv.Rows(0).Item(6)
                        Case "Administrador", "Consulta"
                            Response.Redirect("~/Default.aspx?sUsuario=" + user_name.Value.Trim)
                        Case Else
                            leyenada.Text = "EL USUARIO NO EXISTE"

                    End Select
                ElseIf SQLProv.Rows(0).Item(2) = "777" Then
                    'No existe el usuario.
                    'Response.Redirect("~/Login.aspx")
                    leyenada.Text = "EL USUARIO NO EXISTE"
                End If
            Else
                leyenada.Text = "ERROR AL FIRMAL EL USUARIO"
            End If
        End If
    End Sub

End Class