Imports System.Web
Imports System.Web.Services
Imports System.ComponentModel

Public Class CatMovimientosDes
    Implements System.Web.IHttpHandler
   

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim mov As String = HttpContext.Current.Request.QueryString("q")
        If Not IsNothing(mov) Then

            'Dim lst_mov = From p In db.Tbco_abcDesMovPol
            '                 Order By p.Descripcion Ascending
            '                 Where p.Descripcion.Contains(mov) Or p.IdDesMov.ToString.Contains(mov)
            '                 Select p

            'For Each MovCuentas In lst_mov  
            '    context.Response.Write(MovCuentas.idDesMov.ToString + "|" + MovCuentas.Descripcion + "|" + MovCuentas.Estatus.ToString + "|" + MovCuentas.idPol.ToString + "|" + MovCuentas.idDepto.ToString + Environment.NewLine)
            'Next

            Dim lst_mov = Persistencia.GetDataTable(" SELECT TOP 10 D.IDDESMOV, D.DESCRIPCION, D.ESTATUS, D.IDPOL, D.IDDEPTO, CASE WHEN (SELECT COUNT(IDDESMOV) FROM TBCO_CTAMOV WHERE IDDESMOV = D.IDDESMOV) > 0 THEN '../images/DotsDown-Blue.png' ELSE '../images/DotsDown.png' END AS IMAGEN  FROM TBCO_ABCDESMOVPOL D " &
                                           " WHERE D.DESCRIPCION LIKE '%" + mov + "%' OR D.IDDESMOV LIKE '%" + mov + "%'")


            If lst_mov.Rows.Count > 0 Then

                For Each MovCuentas In lst_mov.AsEnumerable
                    context.Response.Write(MovCuentas.Item("idDesMov").ToString.Trim + "|" + MovCuentas.Item("Descripcion").ToString.Trim + "|" + MovCuentas.Item("Estatus").ToString.Trim + "|" + MovCuentas.Item("idPol").ToString.Trim + "|" + MovCuentas.Item("idDepto").ToString.Trim + "|" + MovCuentas.Item("IMAGEN").ToString.Trim + Environment.NewLine)
                Next

            End If




        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class