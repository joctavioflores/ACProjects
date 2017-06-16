Public Class Site
    Inherits System.Web.UI.MasterPage
    Dim db2 As Persistencia = New Persistencia
    Dim clientIPAddress As String = ""
    Dim myCookie As HttpCookie
    Dim strHostName As String
    Dim SQLProv As New DataTable
    Dim SQLParam As New DataTable
    Dim SQLCtrUs As New DataTable
    Dim SQLMenus As New DataTable
    Dim ls_SitioDealer As String = ""
    Dim Script As String
    Dim ls_usuario As String = ""
    Dim ls_rol As String = ""
    Dim ls_estado As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            strHostName = System.Net.Dns.GetHostName()

            

            'VALIDA QUE LA URL CONTENGA EL USUARIO
            If Not IsNothing(Request.QueryString("sUsuario")) And Request.QueryString("sUsuario") <> "" Then

                'VALIDA QUE ESTE FIRMADO EL USUARIO EN VDEALER 
                SQLProv = Persistencia.GetDataTable(" select USUARIO, convert(VARCHAR(10),DateAdd(day,(FECHA),'12/28/1800'),103) AS FECHA " &
                                           " from tbcm_usrfirmados " &
                                           " where rtrim(USUARIO) = '" + Request.QueryString("sUsuario") + "'" &
                                           " And convert(smalldatetime,DateAdd(day,(FECHA),'12/28/1800'),103) = convert(VARCHAR(10),'" + Date.Now.ToString("yyyy-MM-dd") + "',103)")

                If SQLProv.Rows.Count() > 0 Then


                    If (Not Request.Cookies("UserSettings") Is Nothing) Then
                        myCookie = New HttpCookie("UserSettings")

                        ls_usuario = myCookie("Usuario")
                        ls_rol = myCookie("IdRol")
                        ls_estado = myCookie("IdStatus")

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

                    End If


                    If Request.QueryString("sUsuario") = "GAC" Then
                        Response.Cookies("UserSettings")("IdUsuario") = "0"
                        Response.Cookies("UserSettings")("IdStatus") = "A"
                        Response.Cookies("UserSettings")("IdRol") = "4"
                    Else
                        SQLCtrUs = Persistencia.GetDataTable("SELECT IdUsuario,IdStatus,IdRol FROM USUARIO WHERE NickName LIKE '" + Request.QueryString("sUsuario") + "'")

                        If SQLCtrUs.Rows.Count > 0 Then

                            Response.Cookies("UserSettings")("IdUsuario") = SQLCtrUs.Rows(0).Item("IdUsuario")
                            Response.Cookies("UserSettings")("IdStatus") = SQLCtrUs.Rows(0).Item("IdStatus")
                            Response.Cookies("UserSettings")("IdRol") = SQLCtrUs.Rows(0).Item("IdRol")

                            'SI EL USUARIO NO ES ADEMINISTRADOR OCULTAR EL ACCESO AL CONTROL DE USUARIOS 
                            If Not CInt(SQLCtrUs.Rows(0).Item("IdRol")) = 4 Then
                                usuarios.Visible = False
                            End If

                        End If

                    End If

                    Response.Cookies("UserSettings")("Usuario") = Request.QueryString("sUsuario")
                    Response.Cookies("UserSettings")("IP") = clientIPAddress
                    Response.Cookies("UserSettings")("HOSTNAME") = strHostName

                    Response.Cookies("UserSettings").Expires = DateTime.Now.AddDays(1)

                    myCookie = HttpContext.Current.Request.Cookies("UserSettings")

                    Session.Remove("sUsuario")
                    Session.Remove("sPerfil")
                    If IsNothing(Session("sUsuario")) Then Session("sUsuario") = Request.QueryString("sUsuario")
                    If IsNothing(Session("sPerfil")) Then Session("sPerfil") = Request.QueryString("sPerfil")



                Else
                    Response.Redirect("~/Login.aspx")
                End If


            ElseIf (Not Request.Cookies("UserSettings") Is Nothing) Then

                myCookie = HttpContext.Current.Request.Cookies("UserSettings")

                'VALIDA QUE LA COOKIE DEL USUARIO ESTE FIRMADO EN VDEALER 
                SQLProv = Persistencia.GetDataTable(" select USUARIO, convert(VARCHAR(10),DateAdd(day,(FECHA),'12/28/1800'),103) AS FECHA " &
                                           " from tbcm_usrfirmados " &
                                           " where rtrim(USUARIO) = '" + myCookie("Usuario") + "' " &
                                           " And convert(smalldatetime,DateAdd(day,(FECHA),'12/28/1800'),103) = convert(VARCHAR(10),'" + Date.Now.ToString("yyyy-MM-dd") + "',103)")

                If Not SQLProv.Rows.Count() > 0 Then
                    Response.Redirect("~/Login.aspx")
                End If
            Else
                Response.Redirect("~/Login.aspx")

            End If




        End If


    End Sub

End Class