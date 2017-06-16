Imports System.Web
Imports System.Web.Services

Public Class CatUsuarios
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim nombre As String = HttpContext.Current.Request.QueryString("q")
        If Not IsNothing(nombre) Then

            Dim lst_usuarios = Persistencia.GetDataTable("SELECT idUsuario, upper(NOMBRE) as NOMBRE, upper(NICKNAME) AS NICK, ROL FROM USUARIO S INNER JOIN ROLES_USUARIO R ON S.idrol = R.IdRol where nombre like '%" + nombre + "%' or NickName like '%" + nombre + "%'")

            For Each usuario In lst_usuarios.AsEnumerable

                context.Response.Write(usuario.Item("idUsuario").ToString + "|" + usuario.Item("NOMBRE").ToString + "|" + usuario.Item("NICK") + "|" + usuario.Item("ROL") + Environment.NewLine)
            Next

        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class