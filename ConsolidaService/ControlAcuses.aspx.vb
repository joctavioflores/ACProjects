Public Class ControlAcuses
    Inherits System.Web.UI.Page
    Public sqldata As DataTable = Nothing
    Dim myCookie As HttpCookie
    Public sUsuario As String
    Dim Script As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Buscar()
    End Sub

    Public Sub Buscar()
        sqldata = Persistencia.GetDataTable(" SELECT e.idEmpresa ,Empresa ,h.RFC ,TipoReporte ,TipoEnvio ,Ejercicio , Periodo, Fecha as FechaReal, convert(varchar(10),Fecha,103) as Fecha ,Usuario ,isnull(Folio ,'S/F') AS Acuse " &
                                            " FROM tbco_HistoricoCE h " &
                                            " INNER JOIN tbcm_EmpresaCon e on h.RFC = e.rfc " &
                                            " where e.idEmpresa = " + Request.QueryString("idEmpresa"))
        lstcatalogo.DataSource = sqldata
        lstcatalogo.DataBind()
    End Sub

    Protected Sub lstcatalogo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles lstcatalogo.PageIndexChanging
        lstcatalogo.PageIndex = e.NewPageIndex
        lstcatalogo.DataBind()
    End Sub
End Class