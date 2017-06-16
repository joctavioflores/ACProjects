Public Class Usuarios
    Inherits System.Web.UI.Page
    Dim SQLTable As DataTable = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Buscar()
    End Sub

    Public Sub Buscar()

        SQLTable = Persistencia.GetDataTable(" SELECT idusuario, upper(NOMBRE) as NOMBRE, upper(NICKNAME) AS NICK, ROL FROM USUARIO S INNER JOIN ROLES_USUARIO R ON S.idrol = R.IdRol " &
                                             " where upper(NOMBRE) like '%" + Usuario.Value + "%' or upper(NICKNAME) like '%" + Nick.Value + "%' ")
        lstusuarios.DataSource = SQLTable
        lstusuarios.DataBind()

    End Sub


    Protected Sub lstusuarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles lstusuarios.PageIndexChanging

        lstusuarios.PageIndex = e.NewPageIndex
        lstusuarios.DataBind()

    End Sub

End Class