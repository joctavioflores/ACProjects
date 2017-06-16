Imports System.Web
Imports System.Web.Services

Public Class CatCuentasDesN3
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim cveCTA As String = HttpContext.Current.Request.QueryString("q")
        If Not IsNothing(cveCTA) Then

            Dim lst_cuntas = Persistencia.GetDataTable("select top 10 * from tbco_abccuenta where clavelocal like '%" + cveCTA + "%' or nombre like '%" + cveCTA + "%'")

            For Each cuenta In lst_cuntas.AsEnumerable

                Dim ll_padre = CInt(cuenta.Item("IdPadreLocal"))
                Dim NPadre As String = "Cuenta Raiz"

                If (ll_padre > 0) Then
                    Dim Padre = Persistencia.GetDataTable("select * from tbco_abcuenta where idcta = " + ll_padre.ToString)
                    If Padre.Rows.Count > 0 Then NPadre = Padre.Rows(0).Item("Nombre")
                End If

                context.Response.Write(cuenta.Item("IdCTA").ToString + "|" + cuenta.Item("ClaveLocal") + "|" + cuenta.Item("Nombre") + "|" + NPadre + Environment.NewLine)
            Next

        End If


    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class