Public Class _Default
    Inherits System.Web.UI.Page
    Dim myCookie As HttpCookie

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("sResult") = "Salir" Then
            myCookie = New HttpCookie("UserSettings")
            myCookie.Expires = DateTime.Now.AddDays(-1D)
            Response.Cookies.Add(myCookie)

            'ELIMINA LAS VARIABLES DE SESSION 
            Session.Remove("sUsuario")
            Session.Remove("sPerfil")

            'ELIMINA EL FORMULARIO DEL CACHE
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()

            For Each de As DictionaryEntry In HttpContext.Current.Cache
                HttpContext.Current.Cache.Remove(DirectCast(de.Key, String))
            Next

            Response.Redirect("~/Login.aspx")
        End If



        If Not IsPostBack() Then
            myCookie = HttpContext.Current.Request.Cookies("UserSettings")
            If Not IsNothing(Request.QueryString("sUsuario")) Then
                lbUsuario.InnerText = Request.QueryString("sUsuario")
            ElseIf Not IsNothing(myCookie) Then
                lbUsuario.InnerText = myCookie("Usuario")
            End If
        End If


    End Sub

End Class