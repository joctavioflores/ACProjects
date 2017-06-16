Imports System.IO
Imports System.Xml
Imports System.Threading

Public Class AltaAcuses
    Inherits System.Web.UI.Page
    Dim Script As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not IsNothing(Request.QueryString("idEmpresa")) Then idEmpresa.Value = Request.QueryString("idEmpresa")
            If Not IsNothing(Request.QueryString("RFC")) Then RFC.Value = Request.QueryString("RFC")
            If Not IsNothing(Request.QueryString("Fecha")) Then Fecha.Value = Request.QueryString("Fecha")
            If Not IsNothing(Request.QueryString("TipoReporte")) Then TipoReporte.Value = Request.QueryString("TipoReporte")
            If Not IsNothing(Request.QueryString("TipoEnvio")) Then TipoEnvio.Value = Request.QueryString("TipoEnvio")
            If Not IsNothing(Request.QueryString("Ejercicio")) Then Ejercicio.Value = Request.QueryString("Ejercicio")
            If Not IsNothing(Request.QueryString("Periodo")) Then Periodo.Value = Request.QueryString("Periodo")
            If Not IsNothing(Request.QueryString("Usuario")) Then Usuario.Value = Request.QueryString("Usuario")
        End If

        If archivo.Value <> "" Then
            myfile.PostedFile.SaveAs("c:\texto\" + myfile.PostedFile.FileName)

            Persistencia.EjecutarSQL("update tbco_historicoCE set FechaAcuse = getdate(), Folio = '" + txtFolio.Value + "', RutaAcuse = '" + "c:\texto\" + "', DocumentoAcuse = '" + myfile.PostedFile.FileName + "' " &
                                     "where RFC = '" + RFC.Value.ToString + "' And Fecha = '" + CDate(Fecha.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' And Ejercicio = " + Ejercicio.Value.ToString + " And Periodo = " + Periodo.Value.ToString + " And TipoReporte = '" + TipoReporte.Value.ToString + "'  And TipoEnvio = '" + TipoEnvio.Value.ToString + "'")

            'archivo.Value = ""

            Script = "<script language=""javascript"">{ $(document).ready(function () { ReAbrirAcuse(); }); }</script>"
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "MiScriptSetNombreCierra", Script, False)

            'If Path.GetExtension(myfile.PostedFile.FileName) = ".xml" Then
            '    Dim xmlDoc As New XmlDocument()
            '    xmlDoc.Load("c:\texto\" + myfile.PostedFile.FileName)
            '    ThreadPool.QueueUserWorkItem(AddressOf OpenXML, xmlDoc.InnerXml)
            'End If

            'If Path.GetExtension(myfile.PostedFile.FileName) = ".xls" Or Path.GetExtension(myfile.PostedFile.FileName) = ".xlsx" Or Path.GetExtension(myfile.PostedFile.FileName) = ".pdf" Then
            '    ThreadPool.QueueUserWorkItem(AddressOf OpenXML, "c:\texto\" + myfile.PostedFile.FileName)
            'End If

        End If
    End Sub


    Public Delegate Sub WaitCallback(ByVal state As Object)

    Private Sub OpenXML(ByVal X As Object)

        Dim Archivo As String = CType(X, String)
        Dim key = AesUtil.GetAesKeys("G@V18TU4L")
        Dim razon As Integer = 0
        Dim agencia As Integer = 0
        Dim ejercicio As Integer = 0
        Dim periodo As Integer = 0
        Dim tabla As DataTable = New DataTable
        Dim SQLDC As DataTable = New DataTable
        Dim SQLProv As DataTable = New DataTable
        Dim TextFile As String = ""
        Dim mensaje As String = ""

        
    End Sub

End Class