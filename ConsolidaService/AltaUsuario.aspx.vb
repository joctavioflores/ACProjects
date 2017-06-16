Public Class AltaUsuario
    Inherits System.Web.UI.Page

    Public iidsuario As Integer = 0
    Dim SQLUsuario As DataTable = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            If Not IsNothing(Request.QueryString("idUsuario")) And CInt(Request.QueryString("idUsuario")) > 0 Then
                iidsuario = CInt(Request.QueryString("idUsuario"))
                SQLUsuario = Persistencia.GetDataTable(" SELECT idusuario, upper(NOMBRE) as NOMBRE, upper(NICKNAME) AS NICK, ROL, R.IdRol FROM USUARIO S INNER JOIN ROLES_USUARIO R ON S.idrol = R.IdRol " &
                                                       " where idusuario = " + iidsuario.ToString)
                If SQLUsuario.Rows.Count > 0 Then

                    titulo.Text = "MODIFICAR USUARIO"

                    nombre.Value = SQLUsuario.Rows(0).Item("NOMBRE")
                    usuario.Value = SQLUsuario.Rows(0).Item("NICK")
                    DDLROL.SelectedValue = SQLUsuario.Rows(0).Item("IdRol")

                    nombre.Disabled = True
                    usuario.Disabled = True
                    DDLROL.Enabled = False

                End If

            End If

            If DDLROL.Items.Count = 0 Then

                DDLROL.DataSource = Persistencia.GetDataTable("SELECT IdRol, UPPER(rol) as Rol FROM ROLES_USUARIO")
                DDLROL.DataTextField = "Rol"
                DDLROL.DataValueField = "IdRol"

                DDLROL.DataBind()

            End If

        End If



    End Sub

End Class