Public Class ProgressBar
    Inherits System.Web.UI.Page
    Dim SQLPoliza As DataTable
    Dim SQLCatalogo As DataTable
    Public sDescripcion As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Ejercicio.Attributes.Add("onChange", "SelectEjercicio()")
        Periodo.Attributes.Add("onChange", "SelectPeriodo()")
        TipoOperacion.Attributes.Add("onChange", "SelectOperacion()")

        If Not IsPostBack() Then

            If Ejercicio.Items.Count = 0 Then
                Ejercicio.DataSource = Persistencia.GetDataTable("select * from tbco_ejercicios")
                Ejercicio.DataTextField = "Anio"
                Ejercicio.DataValueField = "idEjercicio"
                Ejercicio.DataBind()
            End If

            If Ejercicio.Items.Count > 0 Then Ejercicio.SelectedValue = Ejercicio.Items.FindByText(Year(Date.Today).ToString).Value

            Periodo.DataSource = Persistencia.GetDataTable("select idperiodo ,idEjercicio,Mes, Nombre, Activo, FechaIni, FechaFin,Dias from tbco_periodos where idEjercicio=" + Ejercicio.SelectedValue.ToString.ToUpper + " order by Mes")

            Periodo.DataTextField = "nombre"
            Periodo.DataValueField = "idPeriodo"
            Periodo.DataBind()

            If Periodo.Items.Count > 0 Then Periodo.SelectedValue = Periodo.Items.FindByValue(Month(Date.Today)).Value

            sDescripcion = Request.QueryString("descripcion")

            If Not IsNothing(Request.QueryString("descripcion")) Then

                leyenda.InnerText = Request.QueryString("descripcion")

                Select Case Request.QueryString("descripcion")

                    Case "EAT"


                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo") + "|" + TipoOperacion.SelectedValue + "|" + txtReferencia.Text

                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = False
                        TipoOperacion.Visible = False
                        lbReferencia.Visible = False
                        txtReferencia.Visible = False
                        btnPreviewRpt.Visible = True



                        lbInst.Text = "DA CLICK EN 'Importar' PARA PROCESAR EL ARCHIVO."
                        consolidar.Value = "Importar"


                    Case "POLIZAS"

                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo") + "|" + TipoOperacion.SelectedValue + "|" + txtReferencia.Text

                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = True
                        TipoOperacion.Visible = True
                        lbReferencia.Visible = True
                        txtReferencia.Visible = True
                        btnPreviewRpt.Visible = False

                        'GENERANDO ENCABEZADO XML
                        SQLPoliza = Persistencia.GetDataTable(" select count(idhpol) " &
                                                    " from tbco_movHistoricoC where idejercicio = " + Request.QueryString("ejercicio") + " and idperiodo = " + Request.QueryString("periodo") + " and idRazon = " + Request.QueryString("razon"))

                        If Not SQLPoliza.Rows.Count > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS EN EL PERIODO Y AGENCIAS SELECCIONADAS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False

                            lbTipoOperacion.Visible = False
                            TipoOperacion.Visible = False
                            lbReferencia.Visible = False
                            txtReferencia.Visible = False
                            btnPreviewRpt.Visible = False
                        End If
                        lbInst.Text = "DA CLICK EN 'Generar' PARA GENERAR EL ARCHIVO XML."
                        consolidar.Value = "Generar"

                    Case "CATALOGO"
                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo")
                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = False
                        TipoOperacion.Visible = False
                        lbReferencia.Visible = False
                        txtReferencia.Visible = False
                        btnPreviewRpt.Visible = False
                        SQLCatalogo = Persistencia.GetDataTable(" select count(idcta) " &
                                                                " from tbco_abcCuenta ")

                        If Not CInt(SQLCatalogo.Rows(0).Item(0)) > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False
                        End If
                        lbInst.Text = "DA CLICK EN 'Generar' PARA GENERAR EL ARCHIVO XML."
                        consolidar.Value = "Generar"

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

                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo")

                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = False
                        TipoOperacion.Visible = False
                        lbReferencia.Visible = False
                        txtReferencia.Visible = False
                        btnPreviewRpt.Visible = False

                        SQLPoliza = Persistencia.GetDataTable("exec COspa_RptBalanzaC 'L'," + ll_razon + "," + ll_agencia + ",5," + Request.QueryString("ejercicio") + "," + Request.QueryString("periodo") + ",'E'")

                        If Not SQLPoliza.Rows.Count > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS EN EL PERIODO Y AGENCIAS SELECCIONADAS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False
                        End If
                        lbInst.Text = "DA CLICK EN 'Generar' PARA GENERAR EL ARCHIVO XML."
                        consolidar.Value = "Generar"

                    Case "AUXILIARFOLIOS"
                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo") + "|" + TipoOperacion.SelectedValue + "|" + txtReferencia.Text

                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = True
                        TipoOperacion.Visible = True
                        lbReferencia.Visible = True
                        txtReferencia.Visible = True
                        btnPreviewRpt.Visible = False

                        'GENERANDO ENCABEZADO XML
                        SQLPoliza = Persistencia.GetDataTable(" select count(idhpol) " &
                                                   " from tbco_movHistoricoC where idejercicio = " + Request.QueryString("ejercicio") + " and idperiodo = " + Request.QueryString("periodo") + " and idRazon = " + Request.QueryString("razon"))

                        If Not CInt(SQLPoliza.Rows(0).Item(0)) > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS EN EL PERIODO Y AGENCIAS SELECCIONADAS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False

                            lbTipoOperacion.Visible = False
                            TipoOperacion.Visible = False
                            lbReferencia.Visible = False
                            txtReferencia.Visible = False
                            btnPreviewRpt.Visible = False

                        End If

                        lbInst.Text = "DA CLICK EN 'Generar' PARA GENERAR EL ARCHIVO XML."
                        consolidar.Value = "Generar"

                    Case "AUXILIARCUENTAS"
                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo") + "|" + TipoOperacion.SelectedValue + "|" + txtReferencia.Text

                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = True
                        TipoOperacion.Visible = True
                        lbReferencia.Visible = True
                        txtReferencia.Visible = True
                        btnPreviewRpt.Visible = False

                        'GENERANDO ENCABEZADO XML
                        SQLPoliza = Persistencia.GetDataTable(" select count(idhpol) " &
                                                   " from tbco_movHistoricoC where idejercicio = " + Request.QueryString("ejercicio") + " and idperiodo = " + Request.QueryString("periodo") + " and idRazon = " + Request.QueryString("razon"))

                        If Not CInt(SQLPoliza.Rows(0).Item(0)) > 0 Then
                            finaliza.InnerText = "NO EXISTEN REGISTROS EN EL PERIODO Y AGENCIAS SELECCIONADAS"
                            finaliza.Style.Add(HtmlTextWriterStyle.Color, "RED")
                            consolidar.Visible = False

                            lbTipoOperacion.Visible = False
                            TipoOperacion.Visible = False
                            lbReferencia.Visible = False
                            txtReferencia.Visible = False
                            btnPreviewRpt.Visible = False
                        End If

                        lbInst.Text = "DA CLICK EN 'Generar' PARA GENERAR EL ARCHIVO XML."
                        consolidar.Value = "Generar"



                    Case "IMPORTAR"


                        process.Value = Request.QueryString("descripcion") + "|" + Request.QueryString("ejercicio") + "|" + Request.QueryString("periodo") + "|" + Request.QueryString("estado") + "|" + Request.QueryString("razon") + "|" + Request.QueryString("agencia") + "|" + Request.QueryString("usuario") + "|" + Request.QueryString("idhpol") + "|" + Request.QueryString("registro") + "|" + Request.QueryString("archivo") + "|" + TipoOperacion.SelectedValue + "|" + txtReferencia.Text

                        Ejercicio.Visible = False
                        Periodo.Visible = False
                        lbEjercicio.Visible = False
                        lbPeriodo.Visible = False

                        lbTipoOperacion.Visible = False
                        TipoOperacion.Visible = False
                        lbReferencia.Visible = False
                        txtReferencia.Visible = False
                        btnPreviewRpt.Visible = False


                        lbInst.Text = "DA CLICK EN 'Importar' PARA PROCESAR EL ARCHIVO."
                        consolidar.Value = "Importar"


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

    'Protected Sub btnDetener_Click(sender As Object, e As EventArgs) Handles btnDetener.Click
    '    Try
    '        If Not FileIO.FileSystem.FileExists("c:\texto\" + "BAT_deteneragente.bat") Then

    '            'Dim p As New ProcessStartInfo

    '            'p.WorkingDirectory = Server.MapPath("c:\texto\")
    '            'p.FileName = "EjecutaCicloBat.bat"
    '            'p.UseShellExecute = True
    '            'Dim process As Process = process.Start(p)
    '            'process.WaitForExit(2000)


    '            Dim fsLog = CreateObject("Scripting.FileSystemObject")
    '            Dim FicheroTextoLog = fsLog.CreateTextFile("c:\texto\" + "BAT_deteneragente.bat", True)
    '            FicheroTextoLog.writeline(">nul 2>&1 ""%SYSTEMROOT%\system32\cacls.exe"" ""%SYSTEMROOT%\system32\config\system""")
    '            FicheroTextoLog.writeline("if '%errorlevel%' NEQ '0' (")
    '            FicheroTextoLog.writeline("echo Requesting administrative privileges...")
    '            FicheroTextoLog.writeline("goto UACPrompt")
    '            FicheroTextoLog.writeline(") else ( goto gotAdmin )")
    '            FicheroTextoLog.writeline(":UACPrompt")
    '            FicheroTextoLog.writeline("echo Set UAC = CreateObject^(""Shell.Application""^) > ""%temp%\getadmin.vbs""")
    '            FicheroTextoLog.writeline("echo UAC.ShellExecute ""%~s0"", """", """", ""runas"", 1 >> ""%temp%\getadmin.vbs""")
    '            FicheroTextoLog.writeline("""%temp%\getadmin.vbs""")
    '            FicheroTextoLog.writeline("exit /B")
    '            FicheroTextoLog.writeline(":gotAdmin")
    '            FicheroTextoLog.writeline("if exist ""%temp%\getadmin.vbs"" ( del ""%temp%\getadmin.vbs"" )")
    '            FicheroTextoLog.writeline("pushd ""%CD%""")
    '            FicheroTextoLog.writeline("CD /D ""%~dp0""")
    '            FicheroTextoLog.writeline("net stop SQLSERVERAGENT")
    '            FicheroTextoLog.close()
    '            fsLog = Nothing

    '        End If
    '    Catch ex As Exception

    '    End Try

    'End Sub


    Private Function RunCommand(Script As String, ParamArray Parameters() As String) As Boolean

        Dim process As System.Diagnostics.Process = Nothing
        Dim processStartInfo As System.Diagnostics.ProcessStartInfo
        Dim OK As Boolean

        processStartInfo = New System.Diagnostics.ProcessStartInfo()
        processStartInfo.FileName = """" & Environment.SystemDirectory & "\cmd.exe" & """"

        If My.Computer.Info.OSVersion >= "6" Then  ' Windows Vista or higher
            'required to invoke UAC
            processStartInfo.Verb = "runas"
        End If

        processStartInfo.Arguments = "/C """"" & Script & """ " & Join(Parameters, " ") & """"
        processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
        processStartInfo.UseShellExecute = False

        'MsgBox("About to execute the following command:" & vbCrLf & processStartInfo.FileName & vbCrLf & "with parameters:" & vbCrLf & processStartInfo.Arguments)
        Try
            process = System.Diagnostics.Process.Start(processStartInfo)
            OK = True
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Exclamation, "Unexpected Error running update script")
            OK = False
        Finally
            If process IsNot Nothing Then
                process.Dispose()
            End If
        End Try

        Return OK

    End Function

    'Protected Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
    '    Try
    '        If Not FileIO.FileSystem.FileExists("c:\texto\" + "BAT_iniciaragente.bat") Then

    '            Dim fsLog2 = CreateObject("Scripting.FileSystemObject")
    '            Dim FicheroTextoLog = fsLog2.CreateTextFile("c:\texto\" + "BAT_iniciaragente.bat", True)
    '            FicheroTextoLog.writeline(">nul 2>&1 ""%SYSTEMROOT%\system32\cacls.exe"" ""%SYSTEMROOT%\system32\config\system""")
    '            FicheroTextoLog.writeline("if '%errorlevel%' NEQ '0' (")
    '            FicheroTextoLog.writeline("echo Requesting administrative privileges...")
    '            FicheroTextoLog.writeline("goto UACPrompt")
    '            FicheroTextoLog.writeline(") else ( goto gotAdmin )")
    '            FicheroTextoLog.writeline(":UACPrompt")
    '            FicheroTextoLog.writeline("echo Set UAC = CreateObject^(""Shell.Application""^) > ""%temp%\getadmin.vbs""")
    '            FicheroTextoLog.writeline("echo UAC.ShellExecute ""%~s0"", """", """", ""runas"", 1 >> ""%temp%\getadmin.vbs""")
    '            FicheroTextoLog.writeline("""%temp%\getadmin.vbs""")
    '            FicheroTextoLog.writeline("exit /B")
    '            FicheroTextoLog.writeline(":gotAdmin")
    '            FicheroTextoLog.writeline("if exist ""%temp%\getadmin.vbs"" ( del ""%temp%\getadmin.vbs"" )")
    '            FicheroTextoLog.writeline("pushd ""%CD%""")
    '            FicheroTextoLog.writeline("CD /D ""%~dp0""")
    '            FicheroTextoLog.writeline("net start SQLSERVERAGENT")
    '            FicheroTextoLog.close()
    '            fsLog2 = Nothing


    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class