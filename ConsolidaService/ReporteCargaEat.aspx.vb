Imports System.ServiceModel
Imports System.Xml
Imports System.IO

Public Class ReporteCargaEat
    Inherits System.Web.UI.Page
    Public sqldata As DataTable = Nothing
    Dim myCookie As HttpCookie
    Public sUsuario As String
    Dim Script As String = ""
    Dim Fchaini As String = ""
    Dim Fchafin As String = ""



    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Buscar()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then



            If Razon.Items.Count = 0 Then
                Dim donde As String = ""

                Razon.DataSource = Persistencia.GetDataTable("select * from tbco_razonsocial ")
                Razon.DataTextField = "razon"
                Razon.DataValueField = "idrazon"
                Razon.DataBind()

                ' insert an item at the beginning of the list
                '----------------------------------------------------
                Razon.Items.Insert(0, New ListItem("-- SELECT RAZON --", "0"))

            End If


            Fchaini = DateAdd(DateInterval.Day, -1, Now.Date).ToString("dd/MM/yyyy")
            Fchafin = DateTime.Parse(Now.Date).ToString("dd/MM/yyyy")

            fechaini.Text = Fchaini
            fechafin.Text = Fchafin

            If Estado.Items.Count = 0 Then

                Estado.DataSource = Persistencia.GetDataTable("select distinct cargaremota from tbve_estadocargaglobal " &
                                                              " where fechatraslado between datediff(day,'12/28/1800',convert(datetime,'" + CDate(fechaini.Text).ToString("yyyy-MM-dd") + "')) " &
                                                              " And datediff(day,'12/28/1800',convert(datetime,'" + CDate(fechafin.Text).ToString("yyyy-MM-dd") + "')) ")
                Estado.DataTextField = "cargaremota"
                Estado.DataValueField = "cargaremota"
                Estado.DataBind()

                ' insert an item at the beginning of the list
                '----------------------------------------------------
                Estado.Items.Insert(0, New ListItem("-- SELECT ESTADO --", ""))
                If Not IsNothing(Estado.Items.FindByText("ERROR")) Then
                    Estado.SelectedValue = Estado.Items.FindByText("ERROR").Value
                End If

            End If

        End If

        Buscar()

    End Sub

    Protected Sub Razon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Razon.SelectedIndexChanged

        Agencia.Items.Clear()
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


            Else
                Agencia.Enabled = False

            End If

        End If
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function EnviaPeticion(ByVal empSex As String) As String
        Dim arreglo As String() = empSex.Split("|")
        Dim xmlDoc As New XmlDocument()
        Dim mensajeWS As String = ""
        Dim sqlScript As String = ""
        Dim Distribuidor As String = arreglo(0).Trim
        Dim FechaTraslado As String = arreglo(1).Trim
        Dim RutaServerWeb As String = arreglo(2).Trim
        Dim ArchivoSQL As String = arreglo(3).Trim
        Dim NombreDistribuidor As String = ""
        Dim RutaArchivo = HttpContext.Current.Server.MapPath("~/uploads/ScriptsEAT/")
        Dim NombreArchivo = "EAT_" + Distribuidor + "_" + CDate(FechaTraslado).ToString("ddMMyy")
        Dim CallWS = Nothing
        Dim SQLParametrosGRLS As DataTable = Nothing
        Dim EjecutraScrit As String = ""
        Dim FinalizaTran As String = ""


        'Bloqueo de Agente Server SQL Server
        Try
            If Not FileIO.FileSystem.FileExists("c:\texto\" + "BAT_deteneragente.bat") Then

                Dim fsLog = CreateObject("Scripting.FileSystemObject")
                Dim FicheroTextoLog = fsLog.CreateTextFile("c:\texto\" + "BAT_deteneragente.bat", True)
                FicheroTextoLog.writeline(">nul 2>&1 ""%SYSTEMROOT%\system32\cacls.exe"" ""%SYSTEMROOT%\system32\config\system""")
                FicheroTextoLog.writeline("if '%errorlevel%' NEQ '0' (")
                FicheroTextoLog.writeline("echo Requesting administrative privileges...")
                FicheroTextoLog.writeline("goto UACPrompt")
                FicheroTextoLog.writeline(") else ( goto gotAdmin )")
                FicheroTextoLog.writeline(":UACPrompt")
                FicheroTextoLog.writeline("echo Set UAC = CreateObject^(""Shell.Application""^) > ""%temp%\getadmin.vbs""")
                FicheroTextoLog.writeline("echo UAC.ShellExecute ""%~s0"", """", """", ""runas"", 1 >> ""%temp%\getadmin.vbs""")
                FicheroTextoLog.writeline("""%temp%\getadmin.vbs""")
                FicheroTextoLog.writeline("exit /B")
                FicheroTextoLog.writeline(":gotAdmin")
                FicheroTextoLog.writeline("if exist ""%temp%\getadmin.vbs"" ( del ""%temp%\getadmin.vbs"" )")
                FicheroTextoLog.writeline("pushd ""%CD%""")
                FicheroTextoLog.writeline("CD /D ""%~dp0""")
                FicheroTextoLog.writeline("net stop ""SQL Server Agent (CORPORATIVO)""")
                FicheroTextoLog.close()
                fsLog = Nothing
                Threading.Thread.Sleep(2000)

            End If

        Catch ex As Exception

        End Try


        Try
            SQLParametrosGRLS = Persistencia.GetDataTable(" select Distribuidor, Descripcion, isnull(RutaServerWeb,'') as RutaServerWeb, isnull(EjecutaScript,'False') as EjecutaScript , isnull(FinalizaTran,'rollback') as FinalizaTran " + Environment.NewLine &
                                                          " from tbcm_SucursalCon where bandMatriz = 1 And Distribuidor = " + Distribuidor)

            If SQLParametrosGRLS.Rows.Count > 0 Then

                NombreDistribuidor = SQLParametrosGRLS.Rows(0).Item("Descripcion")
                EjecutraScrit = SQLParametrosGRLS.Rows(0).Item("EjecutaScript")
                FinalizaTran = SQLParametrosGRLS.Rows(0).Item("FinalizaTran")

            End If

            mensajeWS = ""
            CallWS = New CargarDatos.WSCargaDatosSoapClient
            CallWS.Endpoint.Address = New EndpointAddress(RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")
            mensajeWS = CallWS.ComprobarCargaEAT(VProcesos.m_usuario, Distribuidor, FechaTraslado)

            If mensajeWS <> "OK" Then

                Try
                    If File.Exists(RutaArchivo + "/" + NombreArchivo + ".xml") Then

                        xmlDoc.Load(RutaArchivo + "/" + NombreArchivo + ".xml")
                        mensajeWS = ""
                        CallWS = New CargarDatos.WSCargaDatosSoapClient
                        CallWS.Endpoint.Address = New EndpointAddress(RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")

                        mensajeWS = CallWS.SendDatos(VProcesos.m_usuario, Distribuidor, Distribuidor, Encoding.[Default].GetBytes(xmlDoc.OuterXml)).Trim()
                        arreglo = mensajeWS.Split("|")



                        sqlScript = " update tbve_estadocargaglobal " + Environment.NewLine &
                                    " set ObservacionesRemoto = 'INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] CON ESTATUS [" + arreglo(1) + "]'" + Environment.NewLine
                        sqlScript = sqlScript + " + ' [" + arreglo(0) + "] ' " + Environment.NewLine
                        sqlScript = sqlScript + ", cargaremota = '" + arreglo(1) + "'" + Environment.NewLine
                        sqlScript = sqlScript + " where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "'))"
                        Persistencia.GetDataTable(sqlScript)
                        'Thread.Sleep(2000)

                        Try


                            CambiarOpcion(RutaArchivo + NombreArchivo + ".sql", "@opcion as int = 1", "@opcion as int = 2")


                            Try
                                If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                    Dim sqllines As String
                                    Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                        sqllines = FileReader.ReadToEnd
                                        Persistencia.EjecutarSQL(sqllines)


                                        sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Distribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                    "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                    "	 select  " + Distribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'SYNC PRUEBA'," Else sqlScript = sqlScript + "'SYNC',"
                                        sqlScript = sqlScript + "'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                        sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                        "    update tbve_estadocargaglobal " + Environment.NewLine &
                                        "    set fecharegistro = getdate(), observaciones = 'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'SYNC PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'SYNC'"
                                        sqlScript = sqlScript + " where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                        Persistencia.GetDataTable(sqlScript)

                                        File.Delete(RutaArchivo + "/" + NombreArchivo + ".xml")

                                        Return "CARGA EAT REPORTADA AL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"


                                    End Using
                                Else
                                    Return "ERROR NO EXISTE ARCHIVO SQL A EJECUTAR"
                                End If

                            Catch ex As Exception
                                'Process.SetStatus(CInt((llcount * 90) / rcta))
                                'Process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                'mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Distribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                            "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                            "	 select  " + Distribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                            " else " + Environment.NewLine &
                                            "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                            "	 where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                Persistencia.EjecutarSQL(sqlScript)
                                Return "ERROR AL EJECUTAR EL ARCHIVO SQL EN SERVIDOR CENTRAL"
                                'Thread.Sleep(2000)
                            End Try

                        Catch ex As Exception
                            Return "ERROR AL REPORTAR CARGA EAT AL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"
                            'Process.SetStatus(CInt((llcount * 90) / rcta))
                            'Process.Name = "ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + Distribuidor + " " + NombreDistribuidor + " ]"
                            'mensaje = mensaje + "<br/>ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>" + ex.Message

                            'Thread.Sleep(2000)
                        End Try
                    Else
                        Return "ERROR NO EXISTE EL ARCHIVO XML PARA SER PROCESADO DEL [ " + Distribuidor + " " + NombreDistribuidor + " ]"
                    End If
                    
                Catch ex As Exception

                    'Process.SetStatus(CInt((llcount * 90) / rcta))
                    'Process.Name = "ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                    'mensaje = mensaje + "<br/>ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                    'mensaje = mensaje + "<br/>" + Replace(ex.Message, System.Environment.NewLine, "<br/>")
                    'mensaje = mensaje + "<br/>" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx"

                    sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Distribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,cargaremota,ObservacionesRemoto,usuario) " + Environment.NewLine &
                                "	 select  " + Distribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                " else " + Environment.NewLine &
                                "	 update tbve_estadocargaglobal set fecharegistro = getdate(), cargaremota = 'ERROR' ,ObservacionesRemoto = 'ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]'" + Environment.NewLine &
                                "	 where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) "


                    Persistencia.EjecutarSQL(sqlScript)
                    Return "ERROR DE COMUNICACION CON DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"

                    'Thread.Sleep(2000)

                End Try


            Else

                'Process.SetStatus(CInt((llcount * 90) / rcta))
                'Process.Name = "YA EXISTE UNA CARGA DE UNIDADES EN EL SERVIDOR REMOTO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"
                'mensaje = mensaje + "<br/>YA EXISTE UNA CARGA DE UNIDADES EN EL SERVIDOR REMOTO DEL DISTRIBUIDOR  [ " + Distribuidor + " " + NombreDistribuidor + " ]"

                sqlScript = " update tbve_estadocargaglobal " + Environment.NewLine &
                 " set fecharegistro = getdate(), ObservacionesRemoto = 'INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] CON ESTATUS [OK]'" + Environment.NewLine
                sqlScript = sqlScript + " + ' [OK] ' " + Environment.NewLine
                sqlScript = sqlScript + ", cargaremota = 'OK'" + Environment.NewLine
                sqlScript = sqlScript + " where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                Persistencia.GetDataTable(sqlScript)

                Try

                    CambiarOpcion(RutaArchivo + NombreArchivo + ".sql", "@opcion as int = 1", "@opcion as int = 2")
                    Try
                        If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                            Dim sqllines As String
                            Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                sqllines = FileReader.ReadToEnd
                                Persistencia.EjecutarSQL(sqllines)


                                sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Distribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                            "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                            "	 select  " + Distribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'SYNC PRUEBA'," Else sqlScript = sqlScript + "'SYNC',"
                                sqlScript = sqlScript + "'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                "    update tbve_estadocargaglobal " + Environment.NewLine &
                                "    set fecharegistro = getdate(), observaciones = 'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'SYNC PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'SYNC'"
                                sqlScript = sqlScript + " where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                Persistencia.GetDataTable(sqlScript)

                                File.Delete(RutaArchivo + "/" + NombreArchivo + ".xml")

                                Return "CARGA EAT REPORTADA AL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"


                            End Using
                        Else
                            Return "ERROR NO EXISTE ARCHIVO SQL A EJECUTAR"
                        End If

                    Catch ex As Exception
                        'Process.SetStatus(CInt((llcount * 90) / rcta))
                        'Process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                        'mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                        sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Distribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                    "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                    "	 select  " + Distribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                    " else " + Environment.NewLine &
                                    "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                    "	 where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                        Persistencia.EjecutarSQL(sqlScript)
                        Return "ERROR AL EJECUTAR EL ARCHIVO SQL EN SERVIDOR CENTRAL"
                        'Threading.Thread.Sleep(2000)
                    End Try


                Catch ex As Exception
                    Return "ERROR AL REPORTAR CARGA EAT AL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"
                    'Process.SetStatus(CInt((llcount * 90) / rcta))
                    'Process.Name = "ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + Distribuidor + " " + NombreDistribuidor + " ]"
                    'mensaje = mensaje + "<br/>ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>" + ex.Message

                    'Thread.Sleep(2000)
                End Try

            End If

        Catch ex As Exception
            'Process.SetStatus(CInt((llcount * 90) / rcta))
            'Process.Name = "ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
            'mensaje = mensaje + "<br/>ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
            'mensaje = mensaje + "<br/>" + Replace(ex.Message, System.Environment.NewLine, "<br/>")
            'mensaje = mensaje + "<br/>" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx"
            sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Distribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                        "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,cargaremota,ObservacionesRemoto,usuario) " + Environment.NewLine &
                        "	 select  " + Distribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                        " else " + Environment.NewLine &
                        "	 update tbve_estadocargaglobal set fecharegistro = getdate(), cargaremota = 'ERROR' ,ObservacionesRemoto = 'ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]'" + Environment.NewLine &
                        "	 where distribuidor = " + Distribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(FechaTraslado).ToString("yyyy-MM-dd") + "')) "


            Persistencia.EjecutarSQL(sqlScript)

            Return "ERROR DE COMUNICACION CON DISTRIBUIDOR [ " + Distribuidor + " " + NombreDistribuidor + " ]"

        End Try


        Try
            If Not FileIO.FileSystem.FileExists("c:\texto\" + "BAT_iniciaragente.bat") Then
              
                Dim fsLog2 = CreateObject("Scripting.FileSystemObject")
                Dim FicheroTextoLog = fsLog2.CreateTextFile("c:\texto\" + "BAT_iniciaragente.bat", True)
                FicheroTextoLog.writeline(">nul 2>&1 ""%SYSTEMROOT%\system32\cacls.exe"" ""%SYSTEMROOT%\system32\config\system""")
                FicheroTextoLog.writeline("if '%errorlevel%' NEQ '0' (")
                FicheroTextoLog.writeline("echo Requesting administrative privileges...")
                FicheroTextoLog.writeline("goto UACPrompt")
                FicheroTextoLog.writeline(") else ( goto gotAdmin )")
                FicheroTextoLog.writeline(":UACPrompt")
                FicheroTextoLog.writeline("echo Set UAC = CreateObject^(""Shell.Application""^) > ""%temp%\getadmin.vbs""")
                FicheroTextoLog.writeline("echo UAC.ShellExecute ""%~s0"", """", """", ""runas"", 1 >> ""%temp%\getadmin.vbs""")
                FicheroTextoLog.writeline("""%temp%\getadmin.vbs""")
                FicheroTextoLog.writeline("exit /B")
                FicheroTextoLog.writeline(":gotAdmin")
                FicheroTextoLog.writeline("if exist ""%temp%\getadmin.vbs"" ( del ""%temp%\getadmin.vbs"" )")
                FicheroTextoLog.writeline("pushd ""%CD%""")
                FicheroTextoLog.writeline("CD /D ""%~dp0""")
                FicheroTextoLog.writeline("net start ""SQL Server Agent (CORPORATIVO)""")
                FicheroTextoLog.close()
                fsLog2 = Nothing
                Threading.Thread.Sleep(2000)

            End If
        Catch ex As Exception

        End Try

    End Function

    Public Sub Buscar()


        Script = " select cast(ecg.distribuidor as varchar(3)) + ' ' + suc.descripcion as distribuidor, ecg.distribuidor as idDistribuidor, suc.RutaServerWeb, convert(varchar(10),DateAdd(day,(fechatraslado),'12/28/1800'),103) as fechatraslado ,unidades ,cargadas ,sincargar ,Ruta ,Archivo ,carga ,Observaciones ,cargaremota,ObservacionesRemoto,usuario,fecharegistro " &
                 " from tbve_estadocargaglobal ecg inner join  " &
                 " tbcm_SucursalCon suc on ecg.distribuidor = suc.distribuidor And suc.BandMatriz = 1 "

        Script = Script + " where fechatraslado between datediff(day,'12/28/1800',convert(datetime,'" + CDate(fechaini.Text).ToString("yyyy-MM-dd") + "')) " &
                          " And datediff(day,'12/28/1800',convert(datetime,'" + CDate(fechafin.Text).ToString("yyyy-MM-dd") + "')) "



        If Razon.SelectedValue > 0 Then
            Script = Script + " And suc.idEmpresa = " + Razon.SelectedValue.ToString
        End If

        If Agencia.SelectedValue > 0 Then
            Script = Script + " And suc.distribuidor = " + Agencia.SelectedValue.ToString
        End If

        If Estado.SelectedValue <> "" Then
            Script = Script + " And ecg.cargaremota like '" + Estado.SelectedValue.ToString + "'"
        End If


        Script = Script + " order by ecg.distribuidor asc, fechatraslado desc"

        sqldata = Persistencia.GetDataTable(Script)
        lstCargaEat.DataSource = sqldata
        lstCargaEat.DataBind()
    End Sub

    Protected Sub lstCargaEat_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles lstCargaEat.PageIndexChanging
        lstCargaEat.PageIndex = e.NewPageIndex
        lstCargaEat.DataBind()
    End Sub

    Protected Sub lstCargaEat_DataBound(sender As Object, e As EventArgs) Handles lstCargaEat.DataBound


    End Sub

    Protected Sub lstCargaEat_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles lstCargaEat.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then

        '    'Dim lbtAction As LinkButton = New LinkButton
        '    'lbtAction = TryCast(e.Row.FindControl("cargaremotaLink"), LinkButton)
        '    'If lbtAction.Text <> "OK" Then

        '    '    Dim FechaTraslado As TableCell = e.Row.Cells(1)

        '    '    Dim lbDistribuidor As HtmlInputHidden = New HtmlInputHidden
        '    '    lbDistribuidor = TryCast(e.Row.FindControl("lbDistribuidor"), HtmlInputHidden)

        '    '    Dim lbRutaServerWeb As HtmlInputHidden = New HtmlInputHidden
        '    '    lbRutaServerWeb = TryCast(e.Row.FindControl("lbRutaServerWeb"), HtmlInputHidden)

        '    '    Dim lbcargaremota As HtmlInputHidden = New HtmlInputHidden
        '    '    lbcargaremota = TryCast(e.Row.FindControl("lbcargaremota"), HtmlInputHidden)


        '    '    Dim onClientClick = "AplicarScriptRemoto(" + lbDistribuidor.Value.ToString + ",'" + FechaTraslado.Text + "','" + lbRutaServerWeb.Value.ToString + "','" + lbcargaremota.Value.ToString + "')"
        '    '    lbtAction.OnClientClick = onClientClick

        '    'End If

        'End If
    End Sub

    Public Shared Function CambiarOpcion(ByVal archivo As String, ByVal str1 As String, ByVal str2 As String) As String
        Dim myStreamReaderL1 As System.IO.StreamReader
        Dim myStream As System.IO.StreamWriter

        Dim myStr As String
        myStreamReaderL1 = System.IO.File.OpenText(archivo)
        myStr = myStreamReaderL1.ReadToEnd()
        myStreamReaderL1.Close()

        myStr = myStr.Replace(str1, str2)

        myStream = System.IO.File.CreateText(archivo)
        myStream.WriteLine(myStr)
        myStream.Close()
        Return "MODIFICADO"
    End Function

End Class