Public Class RazonSocial
    Inherits System.Web.UI.Page
    Dim SQLTable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Buscar()
    End Sub
    Public Sub Buscar()
        SQLTable = Persistencia.GetDataTable(" select IDRAZON, RAZON from tbco_razonsocial")
        lstrazon.DataSource = SQLTable
        lstrazon.DataBind()

    End Sub

    Protected Sub lstcatalogo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles lstrazon.PageIndexChanging
        lstrazon.PageIndex = e.NewPageIndex
        lstrazon.DataBind()
    End Sub

    
End Class