Public Class Agencias
    Inherits System.Web.UI.Page
    Dim SQLTable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Buscar()
    End Sub

    Public Sub Buscar()
        SQLTable = Persistencia.GetDataTable(" select  iddistribuidor as DISTRIBUIDOR, Nick as CLAVE, Agencia AS AGENCIA, Razon AS RAZON from tbco_agencias a " &
                                             " inner join tbco_razonsocial r on r.idrazon = a.idrazon")
        lstagencias.DataSource = SQLTable
        lstagencias.DataBind()

    End Sub

    Protected Sub lstcatalogo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles lstagencias.PageIndexChanging
        lstagencias.PageIndex = e.NewPageIndex
        lstagencias.DataBind()
    End Sub
    
    
End Class