Imports CrystalDecisions.CrystalReports.Engine

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Serialization

Public Class Reporte
    Inherits System.Web.UI.Page
    Dim pol As Polizas = New Polizas()
    'Dim blz As 
    Dim db2 As Persistencia = New Persistencia
    Dim SQLTable As DataTable
    Dim SQLPoliza As DataTable
    Dim SQLTrs As DataTable
    Dim SQLCheques As DataTable
    Dim SQLTranfer As DataTable
    Dim SQLComprob As DataTable
    Dim reportB As ReportDocument = New ReportDocument()
    Dim Fchaini As String = ""
    Dim Fchafin As String = ""
    Dim myCookie As HttpCookie
    Public sUsuario As String = ""
  

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        reportB.Close()
        reportB.Dispose()

        Dim lreporte = Request.QueryString("idReporte")
        Dim lNombre = Request.QueryString("Nombre")
        Image1.Attributes.Add("innerhtml", lNombre)
        buscar()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Reporte.Attributes.Add("onChange", "ReporteSelected()")

        myCookie = HttpContext.Current.Request.Cookies("UserSettings")
        If Not IsNothing(Request.QueryString("sUsuario")) Then
            sUsuario = Request.QueryString("sUsuario")
        ElseIf Not IsNothing(myCookie) Then
            sUsuario = myCookie("Usuario")
        End If


        If Not IsPostBack Then

           

            If Razon.Items.Count = 0 Then
                Dim donde As String = ""

                Select Case myCookie("idRol")
                    Case "1"
                        donde = "where idrazon = 1"
                    Case "2"
                        donde = "where idrazon = 2"
                    Case "3"
                        donde = "where idrazon = 3"
                End Select


                Razon.DataSource = Persistencia.GetDataTable("select * from tbco_razonsocial " + donde)
                Razon.DataTextField = "razon"
                Razon.DataValueField = "idrazon"
                Razon.DataBind()

                ' insert an item at the beginning of the list
                '----------------------------------------------------
                Razon.Items.Insert(0, New ListItem("-- SELECT RAZON --", "0"))
            End If


            If Ejercicio.Items.Count = 0 Then
                Ejercicio.DataSource = Persistencia.GetDataTable("select * from Tbco_Ejercicios ")
                Ejercicio.DataTextField = "Anio"
                Ejercicio.DataValueField = "idEjercicio"
                Ejercicio.DataBind()
            End If

            If Ejercicio.Items.Count > 0 Then Ejercicio.SelectedValue = Ejercicio.Items.FindByText(Year(Date.Today).ToString).Value

            Periodo.DataSource = Persistencia.GetDataTable("select idperiodo ,idEjercicio,Mes, Nombre, Activo,FechaIni, FechaFin,Dias from tbco_periodos where idEjercicio=" + Ejercicio.SelectedValue.ToString.ToUpper + " order by Mes")

            Periodo.DataTextField = "nombre"
            Periodo.DataValueField = "idPeriodo"
            Periodo.DataBind()

            If Periodo.Items.Count > 0 Then Periodo.SelectedValue = Periodo.Items.FindByValue(Month(Date.Today)).Value
            Fchaini = DateAdd(DateInterval.Day, -8, Now.Date).ToString("dd/MM/yyyy")
            Fchafin = DateTime.Parse(Now.Date).ToString("dd/MM/yyyy")

            fechaini.Text = Fchaini
            fechafin.Text = Fchafin
        End If

        buscar()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        If Not IsNothing(reportB) Then
            reportB.Close()
            reportB.Dispose()
            'Rpt_Balanza.Dispose()
            'Rpt_Balanza = Nothing
        End If

    End Sub

    Protected Sub buscar()

        Try


            If Agencia.SelectedValue <> Nothing And Ejercicio.SelectedValue <> Nothing And Periodo.SelectedValue <> Nothing And Reporte.SelectedValue > 0 And Razon.SelectedValue > 0 Then

                reportB = New ReportDocument

                Select Case Reporte.SelectedValue
                    Case 1
                        'LIBRO DIARIO
                        Dim folio_ini As String = "0"
                        Dim folio_fin As String = "0"
                        Dim ll_concepto As String = ""
                        Dim ll_movimiento As String = ""

                        Try


                            If FolioIni.Text <> "" And FolioFin.Text <> "" Then
                                folio_ini = FolioIni.Text
                                folio_fin = FolioFin.Text
                            Else
                                folio_ini = "0"
                                folio_fin = "0"
                            End If

                            If concepto.Value <> "" Then
                                ll_concepto = concepto.Value.Trim
                            Else
                                ll_concepto = ""
                            End If

                            If movimiento.Value <> "" Then
                                ll_movimiento = movimiento.Value.Trim
                            Else
                                ll_movimiento = ""
                            End If

                            reportB.Load(Server.MapPath("Reportes/rptDiario.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_RptDiarioC " + Razon.SelectedValue.ToString + "," + Agencia.SelectedValue.ToString + "," &
                                                                    Ejercicio.SelectedValue.ToString + "," + Periodo.SelectedValue.ToString + "," &
                                                                    DDLTipoPoliza.SelectedValue.ToString + "," + folio_ini + "," &
                                                                    folio_fin + ",'" + ll_concepto + "',null,'" &
                                                                    ll_movimiento + "'"))


                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()




                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try


                    Case 2
                        'LIBRO MAYOR
                        Try

                            reportB.Load(Server.MapPath("Reportes/rptLMayor.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_LibroMayorC " + Razon.SelectedValue.ToString + "," + Agencia.SelectedValue.ToString + "," + Ejercicio.SelectedValue.ToString + "," &
                                     Periodo.SelectedValue.ToString + ",'S'"))


                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()
                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try

                    Case 3
                        'AUXILIARES DE CUENTAS
                        Fchaini = fechaini.Text
                        Fchafin = fechafin.Text


                        If CType(Fchaini.Trim, Date) <= CType(Fchafin.Trim, Date) And (Clave1.Value.Trim <> "" And Clave2.Value.Trim <> "") Then
                            Try
                                reportB.Load(Server.MapPath("Reportes/rptAuxiliares.rpt"))
                                reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                                reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_rptAuxC " + Razon.SelectedValue.ToString + "," + Agencia.SelectedValue.ToString + ",'" + Fchaini.Trim + "','" &
                                                                        Fchafin.Trim + "','" + Clave1.Value.Trim + "','" + Clave2.Value.Trim + "'"))


                                Rpt_Balanza.ReportSource = reportB
                                Rpt_Balanza.RefreshReport()

                            Catch ex As Exception
                                reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                            End Try
                        End If

                    Case 4
                        'BALANZA DE COMPROBACIÓN 
                        Try
                            reportB.Load(Server.MapPath("Reportes/rptBalanza.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_RptBalanzaC 'L'," + Razon.SelectedValue.ToString + "," &
                                                                   Agencia.SelectedValue.ToString + "," + Nivel.SelectedValue.ToString + "," + Ejercicio.SelectedValue.ToString + "," &
                                                                   Periodo.SelectedValue.ToString + ",'" + Todas.SelectedValue.ToString + "'"))


                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()
                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try

                    Case 5
                        'BALANCE GENERAL 
                        Try
                            reportB.Load(Server.MapPath("Reportes/rptBalanceGeneral.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_ReporteBalanceC " + Razon.SelectedValue.ToString + "," + Agencia.SelectedValue.ToString + ",'B'," &
                                                                            Ejercicio.SelectedValue.ToString + "," + Periodo.SelectedValue.ToString + ",43"))


                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()

                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try

                    Case 6
                        'ESTADO DE RESULTADOS
                        Try
                            reportB.Load(Server.MapPath("Reportes/rptEdoResultados.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_ReporteCuadradoC " + Razon.SelectedValue.ToString + "," + Agencia.SelectedValue.ToString + ",'D'," &
                                                    Ejercicio.SelectedValue.ToString + "," + Periodo.SelectedValue.ToString + ",39"))

                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()


                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try
                    Case 7
                        'ESTADO DE CAMBIOS
                        Try
                            reportB.Load(Server.MapPath("Reportes/FlujoEfectivo.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_RptCambioFlujo "))

                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()


                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try


                    Case 8
                        'VARIACIONES EN EL CAPITAL
                        Try
                            reportB.Load(Server.MapPath("Reportes/CapitalContable.rpt"))
                            reportB.DataSourceConnections.Clear() ' Clear existing login/datasource for report object 
                            reportB.SetDataSource(Persistencia.GetDataTable(" exec COspa_RptCapitalContable "))

                            Rpt_Balanza.ReportSource = reportB
                            Rpt_Balanza.RefreshReport()


                        Catch ex As Exception
                            reportediv.InnerHtml = "<div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1><p>" + ex.Message + "</p></div>"
                        End Try



                End Select

            Else

                reportB.Close()
                reportB.Dispose()
                Rpt_Balanza.ReportSource = Nothing
                Rpt_Balanza.RefreshReport()

            End If
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub Razon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Razon.SelectedIndexChanged

        Agencia.Items.Clear()
        Reporte.SelectedIndex = 0
        If Agencia.Items.Count = 0 Then
           
            Agencia.DataSource = Persistencia.GetDataTable(" select * from tbco_agencias WHERE IDRAZON =  " + Razon.SelectedItem.Value.ToString)
            Agencia.DataTextField = "Agencia"
            Agencia.DataValueField = "iddistribuidor"
            Agencia.DataBind()

            ' insert an item at the beginning of the list
            '----------------------------------------------------
            Agencia.Items.Insert(0, New ListItem("-- SELECT AGENCIA --", "0"))


            If Agencia.Items.Count > 1 Then
                Agencia.Enabled = True
                Reporte.Enabled = True

            Else
                Agencia.Enabled = False
                Reporte.Enabled = False


            End If

        End If
    End Sub

    Protected Sub Rpt_Balanza_ReportRefresh(source As Object, e As CrystalDecisions.Web.ViewerEventArgs) Handles Rpt_Balanza.ReportRefresh
        'Don't  handle refresh event on refresh button click
        e.Handled = True

        'Manually refresh 
        CType(Rpt_Balanza.ReportSource, ReportClass).Refresh()

        'AReset dynamic database connection information
        '...

        'Reset report source
        '...

        'Reset SQL parameters
        '...
    End Sub

    Protected Sub Reporte_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Reporte.SelectedIndexChanged
        reportB.Close()
        reportB.Dispose()
        Rpt_Balanza.ReportSource = Nothing
        Rpt_Balanza.RefreshReport()
    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExportar.Click


        Dim GvBalanza As GridView = New GridView()

        GvBalanza.DataSource = Persistencia.GetDataTable(" exec COspa_RptBalanzaC 'L'," + Razon.SelectedValue.ToString + "," &
                                                                   Agencia.SelectedValue.ToString + "," + Nivel.SelectedValue.ToString + "," + Ejercicio.SelectedValue.ToString + "," &
                                                                   Periodo.SelectedValue.ToString + ",'" + Todas.SelectedValue.ToString + "','S'")

        GvBalanza.DataBind()

        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        GvBalanza.EnableViewState = False
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        pagina.Controls.Add(form)
        form.Controls.Add(GvBalanza)
        pagina.RenderControl(htw)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=BALANZA" + Periodo.SelectedValue.ToString.PadLeft(2, "0") + Ejercicio.SelectedValue.ToString + ".xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()

    End Sub

End Class