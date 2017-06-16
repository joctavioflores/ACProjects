Imports System.Web
Imports System.Web.Services

Public Class CatConceptosDes
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim mov As String = HttpContext.Current.Request.QueryString("q")
        If Not IsNothing(mov) Then

            Dim lst_mov = Persistencia.GetDataTable(" select TOP 10 * from tbco_movHistoricoC where concepto like '%" + mov + "%' or rtrim(cast(idhpol as char(10))) like '%" + mov + "%'")


            For Each MovCuentas In lst_mov.AsEnumerable
                context.Response.Write(MovCuentas.Item("idHPol").ToString + "|" + MovCuentas.Item("Concepto") + Environment.NewLine)
            Next

        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class