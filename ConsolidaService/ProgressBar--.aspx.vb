Public Class ProgressBar
    Inherits System.Web.UI.Page
    Dim SQLPoliza As DataTable
    Dim SQLCatalogo As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("archivo")

            If Not IsNothing(Request.QueryString("descripcion")) Then

                leyenda.InnerText = Request.QueryString("descripcion")

                Select Case Request.QueryString("descripcion")
                    Case "POLIZAS"

                        'GENERANDO ENCABEZADO XML
                        Dim ll_razon As String = ""
                        Dim ll_agencia As String = ""

                        If CInt(Request.QueryString("razon")) > 0 Then
                            ll_razon = Request.QueryString("razon")
                        End If

                        If CInt(Request.QueryString("agencia")) > 0 Then
                            ll_agencia = Request.QueryString("agencia")
                        End If


                        SQLPoliza = Persistencia.GetDataTable(" select count(idhpol) " &
                                                 " from tbco_movHistoricoC where idejercicio = " + Request.QueryString("ejercicio") + " and idperiodo = " + Request.QueryString("periodo") &
                                                 " And cast(idRazon as varchar(2)) like '%" + ll_razon + "%' And cast(idDistribuidor as varchar(3)) like '%" + ll_agencia + "%'")

                        If Not CInt(SQLPoliza.Rows(0).Item(0)) > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS EN EL PERIODO Y AGENCIAS SELECCIONADAS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False
                        End If
                    Case "CATALOGO"
                        SQLCatalogo = Persistencia.GetDataTable(" select count(idcta) " &
                                                                " from tbco_abcCuenta ")

                        If Not CInt(SQLCatalogo.Rows(0).Item(0)) > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False
                        End If
                    Case "BALANZA"

                        'GENERANDO ENCABEZADO XML
                        Dim ll_razon As String = "NULL"
                        Dim ll_agencia As String = "NULL"

                        If CInt(Request.QueryString("razon")) > 0 Then
                            ll_razon = Request.QueryString("razon")
                        End If

                        If CInt(Request.QueryString("agencia")) > 0 Then
                            ll_agencia = Request.QueryString("agencia")
                        End If


                        SQLPoliza = Persistencia.GetDataTable("exec COspa_RptBalanzaC 'L'," + ll_razon + "," + ll_agencia + ",5," + Request.QueryString("ejercicio") + "," + Request.QueryString("periodo") + ",'S'")

                        If Not SQLPoliza.Rows.Count > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS EN EL PERIODO Y AGENCIAS SELECCIONADAS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False
                        End If
                End Select

            End If


            


        End If
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function ExecuteCommand(commandName As String, targetMethod As String, data As Object) As Object()
        Try
            Dim result As Object() = New Object(1) {}
            result(0) = Command.Create(commandName).Execute(data)
            result(1) = targetMethod
            Return result
        Catch ex As Exception
            ' TODO: add logging functionality 
            Throw
        End Try
    End Function
End Class