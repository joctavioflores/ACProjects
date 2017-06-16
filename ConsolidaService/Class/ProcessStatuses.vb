Imports System.Threading
Imports System.Xml
Imports System.IO
Imports Ionic.Zip
Imports System.Runtime.Remoting.Contexts
Imports System.Xml.Serialization
Imports System.Xml.Xsl
Imports System.Data.OleDb
Imports System.ServiceModel

Public Class ProcessStatuses
    Dim db As Persistencia = New Persistencia()
    Dim SQLProv As New DataTable
    Dim res As Integer = 0


    Private Const m_SessionKey As String = "ProcessStatusesKey"

    Public Shared Function [Get]() As ArrayList
        If HttpContext.Current.Session(m_SessionKey) Is Nothing Then
            HttpContext.Current.Session(m_SessionKey) = New ArrayList()
        End If
        Return DirectCast(HttpContext.Current.Session(m_SessionKey), ArrayList)
    End Function

    Public Shared Sub StartProcessing(ByVal data As Object)
        Dim process As ProcessStatus = DirectCast(DirectCast(data, Object())(0), ProcessStatus)
        While process.Status < 100
            process.IncrementStatus()
            Dim rnd As New Random(DateTime.Now.GetHashCode())
            Thread.Sleep((CInt(rnd.NextDouble() * 40) + 10) * 10)
        End While
        Thread.Sleep(2000)
        ArrayList.Synchronized(DirectCast(DirectCast(data, Object())(1), ArrayList)).Remove(process)
    End Sub

    Private Shared Sub SendCarga(ByVal datos As Object)
        Dim usuario As String = DirectCast(DirectCast(datos, Object())(0), String)
        Dim distribuidor As String = DirectCast(DirectCast(datos, Object())(1), String)
        Dim fecha As String = DirectCast(DirectCast(datos, Object())(2), String)
        Dim ruta As String = DirectCast(DirectCast(datos, Object())(3), String)
        Dim xml As String = DirectCast(DirectCast(datos, Object())(4), String)
        Dim xmlDoc As New XmlDocument()
        Dim consolida = Nothing

        xmlDoc.LoadXml(xml)

        consolida = New CargarDatos.WSCargaDatosSoapClient
        consolida.Endpoint.Address = New EndpointAddress(ruta + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")

        consolida.SendDatos(usuario, distribuidor, fecha, Encoding.[Default].GetBytes(xmlDoc.OuterXml)).Trim()


    End Sub

    Private Shared Sub SendSQL(ByVal datos As Object)
        Dim usuario As String = DirectCast(DirectCast(datos, Object())(0), String)
        Dim distribuidor As String = DirectCast(DirectCast(datos, Object())(1), String)
        Dim fecha As String = DirectCast(DirectCast(datos, Object())(2), String)
        Dim ruta As String = DirectCast(DirectCast(datos, Object())(3), String)
       
        Try
            If File.Exists(ruta) Then
                Dim sqllines As String
                Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(ruta)
                    sqllines = FileReader.ReadToEnd
                    Persistencia.EjecutarSQL(sqllines)
                End Using
            End If
        Catch ex As Exception

        End Try

    End Sub

    Public Shared Sub Start(ByVal data As Object)
        'Dim ADOCFD As ProjectFEAsp.FEAsp = New ProjectFEAsp.FEAsp
        'ADOCFD.Inicializa("")
        Dim XMLTipo As String = DirectCast(DirectCast(data, Object())(2), String)
        Dim myCookie As HttpCookie = DirectCast(DirectCast(data, Object())(3), HttpCookie)
        Dim Archivo As String = DirectCast(DirectCast(data, Object())(4), String)
        Dim pol As Polizas = New Polizas()
        Dim cat As Catalogo = New Catalogo()
        Dim blz As Balanza = New Balanza()
        Dim auxfolios As RepAuxFol = New RepAuxFol()
        Dim auxcta As AuxiliarCtas = New AuxiliarCtas()
        Dim key As String = ""
        Dim psw As String = ""
        Dim cadenaorginal As String = ""
        Dim SQLTable As DataTable
        Dim SQLPoliza As DataTable
        Dim SQLTrs As DataTable
        Dim SQLCheques As DataTable
        Dim SQLTranfer As DataTable
        Dim SQLComprob As DataTable
        Dim SQLCuenta As DataTable
        Dim SQLSubCuenta As DataTable
        Dim process As ProcessStatus = DirectCast(DirectCast(data, Object())(0), ProcessStatus)



        Select Case XMLTipo

            Case "EAT"
                Dim Password = "G@V18TU4L"
                Dim keypsswrd = AesUtil.GetAesKeys(Password)
                Dim xmlDoc As New XmlDocument()
                Dim razon As Integer = 0
                Dim agencia As Integer = 0
                Dim ejercicio As Integer = 0
                Dim periodo As Integer = 0
                Dim tabla As DataTable = New DataTable
                Dim SQLCOMPROBACION As DataTable = New DataTable
                Dim SQLParametrosGRLS As DataTable = New DataTable
                Dim SQLProv As DataTable = New DataTable
                Dim TextFile As String = ""
                Dim mensaje As String = ""
                Dim RutaServerWeb As String = ""
                Dim NombreDistribuidor As String = ""
                Dim EjecutraScrit As String = ""
                Dim FinalizaTran As String = ""
                Dim sqlScript = ""
                Dim sDistribuidor As String = ""
                Dim iAnoModelo As Integer
                Dim sChasis As String
                Dim sNoMotor As String
                Dim sModel As String
                Dim sColorE As String
                Dim sColorI As String
                Dim sTipoCompra As String
                Dim sNoFactura As String
                Dim dPrecioF As Decimal
                Dim sFechaF As String
                Dim sFechaR As String
                Dim sorigen As String
                Dim sPedimento As String
                Dim sFechaP As String
                Dim sbasico As String
                Dim sTipo As String
                Dim sDepto As String
                Dim iAnoModelo2 As Integer
                Dim sFechaX As String
                Dim sFechaRecibio As String
                Dim sOrdenCompra As String
                Dim sFechaTraslado As String
                Dim s2FechaTraslado As String = ""
                Dim sw As StreamWriter
                Dim SQLsw As StreamWriter
                Dim COMPROBACION As String = ""
                Dim CerrarArchivo As Boolean = False
                Dim RutaArchivo As String = ""
                Dim NombreArchivo As String = ""
                Dim Encabezados As String = ""
                Dim BytesEncabezado As Byte() = Nothing
                Dim countUnidades As Integer = 0
                Dim mensajeWS As String = ""
                Dim FechaIni As String = ""
                Dim FechaFin As String = ""

                Dim consolida = Nothing
                'Dim giError As Integer


                If Archivo.Trim.Length > 0 Then
                    Try
                        If Path.GetExtension(Archivo) = ".txt" Then
                            If FileIO.FileSystem.FileExists(Archivo) Then
                                process.SetStatus(10)
                                process.Name = "OBTENIENDO INFORMACIÓN DE ARCHIVO"

                                tabla = txt_to_data(Archivo, False, ",")

                                If tabla.Rows.Count > 0 Then

                                    Dim llcount = 1
                                    Dim rcta = tabla.Rows.Count

                                    Try
                                        If Not FileIO.FileSystem.FileExists("c:\texto\" + "BAT_deteneragente.bat") Then

                                            process.SetStatus(15)
                                            process.Name = "DETENIENDO AGENTE SQLSERVER ESPERE UN MOMENTO..."

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

                                            Thread.Sleep(20000)

                                        End If
                                    Catch ex As Exception

                                    End Try

                                    process.SetStatus(15)
                                    process.Name = "INICIA LECTURA DEL ARCHIVO"
                                    Thread.Sleep(20)
                                    'Persistencia.EjecutarSQL(" ALTER TABLE tbve_unidad DISABLE TRIGGER VEtrg_tbve_Unidad")

                                    Dim OrderEAT = From o As DataRow In tabla.Rows Order By o.Item(0) Ascending, o.Item(15)

                                    For Each eat In OrderEAT.AsEnumerable
                                        Try

                                            sFechaF = Trim(eat.Item(11))
                                            sFechaR = Trim(eat.Item(12))
                                            sFechaTraslado = eat.Item(15)
                                            sFechaX = eat.Item(23)
                                            sFechaRecibio = Trim(eat.Item(24))
                                            sFechaP = Trim(eat.Item(18))
                                            iAnoModelo = eat.Item(2)
                                            sChasis = eat.Item(3)
                                            sNoMotor = eat.Item(4)
                                            sModel = eat.Item(5)
                                            sColorE = eat.Item(6)
                                            sColorI = eat.Item(7)
                                            sTipoCompra = eat.Item(8)
                                            sNoFactura = eat.Item(9)
                                            dPrecioF = eat.Item(10)
                                            sorigen = eat.Item(16)
                                            sPedimento = eat.Item(17)
                                            sbasico = eat.Item(19)
                                            sTipo = eat.Item(20)
                                            sDepto = eat.Item(21)
                                            iAnoModelo2 = eat.Item(22)
                                            sOrdenCompra = eat.Item(27)

                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "DANDO FORMATO A FECHAS ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                            ' se arreglan las fechas
                                            Dim fecha1 As String(), dia As String, mes As String, anio As String
                                            If Len(sFechaF) > 6 Then
                                                fecha1 = Split(sFechaF, "/")
                                                dia = fecha1(1)
                                                mes = fecha1(0)
                                                anio = fecha1(2)
                                                If Len(dia) < 2 Then
                                                    dia = "0" + dia
                                                End If
                                                If Len(mes) < 2 Then
                                                    mes = "0" + mes
                                                End If
                                                sFechaF = dia + "/" + mes + "/" + anio
                                                Array.Clear(fecha1, 0, fecha1.Length)
                                                dia = ""
                                                mes = ""
                                                anio = ""
                                            End If

                                            '------------------------------------------------
                                            If Len(sFechaR) > 6 Then
                                                fecha1 = Split(sFechaR, "/")
                                                dia = fecha1(1)
                                                mes = fecha1(0)
                                                anio = fecha1(2)
                                                If Len(dia) < 2 Then
                                                    dia = "0" + dia
                                                End If
                                                If Len(mes) < 2 Then
                                                    mes = "0" + mes
                                                End If
                                                sFechaR = dia + "/" + mes + "/" + anio
                                                Array.Clear(fecha1, 0, fecha1.Length)
                                                dia = ""
                                                mes = ""
                                                anio = ""
                                            End If


                                            '------------------------------------------------
                                            If Len(sFechaP) > 6 Then
                                                fecha1 = Split(sFechaP, "/")
                                                dia = fecha1(1)
                                                mes = fecha1(0)
                                                anio = fecha1(2)
                                                If Len(dia) < 2 Then
                                                    dia = "0" + dia
                                                End If
                                                If Len(mes) < 2 Then
                                                    mes = "0" + mes
                                                End If
                                                sFechaP = dia + "/" + mes + "/" + anio
                                                Array.Clear(fecha1, 0, fecha1.Length)
                                                dia = ""
                                                mes = ""
                                                anio = ""
                                            End If



                                            '------------------------------------------------
                                            If Len(sFechaRecibio) > 6 Then
                                                fecha1 = Split(sFechaRecibio, "/")
                                                dia = fecha1(1)
                                                mes = fecha1(0)
                                                anio = fecha1(2)
                                                If Len(dia) < 2 Then
                                                    dia = "0" + dia
                                                End If
                                                If Len(mes) < 2 Then
                                                    mes = "0" + mes
                                                End If
                                                sFechaRecibio = dia + "/" + mes + "/" + anio
                                                Array.Clear(fecha1, 0, fecha1.Length)
                                                dia = ""
                                                mes = ""
                                                anio = ""
                                            End If

                                            '------------------------------------------------
                                            If Len(sFechaTraslado) > 6 Then
                                                fecha1 = Split(sFechaTraslado, "/")
                                                dia = fecha1(1)
                                                mes = fecha1(0)
                                                anio = fecha1(2)
                                                If Len(dia) < 2 Then
                                                    dia = "0" + dia
                                                End If
                                                If Len(mes) < 2 Then
                                                    mes = "0" + mes
                                                End If
                                                sFechaTraslado = dia + "/" + mes + "/" + anio
                                                Array.Clear(fecha1, 0, fecha1.Length)
                                                dia = ""
                                                mes = ""
                                                anio = ""
                                            End If

                                            If sDistribuidor <> Mid(eat.Item(0), 3, Len(eat.Item(0))) Or s2FechaTraslado <> sFechaTraslado Then

                                                If sDistribuidor <> "" Then

                                                    'Cierra archivo lo empaqueta y envía petición a cliente para entrega de información
                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                    process.Name = "CERRANDO ARCHIVO Y MANDANDO EMPAQUETADO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                    countUnidades = countUnidades - 1

                                                    COMPROBACION = COMPROBACION + Environment.NewLine + " select u.distribuidor, v.vin, u.fechaTraslado ,case when v.vin = u.vin And u.vin = u2.vin then 'OK' else 'ERROR' end as Cargado " + Environment.NewLine &
                                                                                                         " into #estadocarga" + sDistribuidor + Environment.NewLine &
                                                                                                         " from #vin" + sDistribuidor + " v " + Environment.NewLine &
                                                                                                         " left join tbve_Unidad u on v.vin = u.Vin " + Environment.NewLine &
                                                                                                         " left join tbve_unidad2 u2 on u.Vin = u2.Vin And u.fechaTraslado = u2.fechatraslado " + Environment.NewLine &
                                                                                                         " where u.idStatus = 0 And u.fechatraslado = @fechatralado" + Environment.NewLine

                                                    COMPROBACION = COMPROBACION + Environment.NewLine + " delete from tbve_estadocargaglobal  " + Environment.NewLine &
                                                                                                        " where distribuidor = " + sDistribuidor + "  And fechatraslado = @fechatralado"


                                                    COMPROBACION = COMPROBACION + Environment.NewLine + " select distribuidor,fechaTraslado, COUNT(VIN) as Unidades " + Environment.NewLine &
                                                                                                        " ,case when Cargado ='OK' then  COUNT(VIN) else 0 end as cargadas " + Environment.NewLine &
                                                                                                        " ,case when Cargado ='ERROR' then  COUNT(VIN) else 0 end as sincargar " + Environment.NewLine &
                                                                                                        " ,'" + RutaArchivo + "' as ruta,'" + NombreArchivo + ".sql" + "' as archivo, '' as carga " + Environment.NewLine &
                                                                                                        " into #previo" + sDistribuidor + Environment.NewLine &
                                                                                                        " from #estadocarga" + sDistribuidor + Environment.NewLine &
                                                                                                        " group by distribuidor,fechaTraslado, Cargado " + Environment.NewLine

                                                    COMPROBACION = COMPROBACION + Environment.NewLine + " insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,usuario)  " + Environment.NewLine &
                                                                                                       " select distribuidor,fechaTraslado, sum(unidades), sum(cargadas), sum(sincargar) " + Environment.NewLine &
                                                                                                       " ,ruta " + Environment.NewLine &
                                                                                                       " ,archivo " + Environment.NewLine &
                                                                                                       " ,case when  sum(unidades)<>sum(cargadas) then 'ERROR' else 'OK' end " + Environment.NewLine &
                                                                                                       " ,'" + VProcesos.m_usuario + "'" + Environment.NewLine &
                                                                                                       " from #previo" + sDistribuidor + Environment.NewLine &
                                                                                                       " group by  distribuidor,fechaTraslado,ruta ,archivo " + Environment.NewLine


                                                    COMPROBACION = COMPROBACION + Environment.NewLine + " DROP TABLE #previo" + sDistribuidor.ToString + Environment.NewLine &
                                                                                                        " DROP TABLE #estadocarga" + sDistribuidor.ToString + Environment.NewLine &
                                                                                                        " DROP TABLE #vin" + sDistribuidor.ToString + Environment.NewLine &
                                                                                                        Environment.NewLine + " end" + Environment.NewLine &
                                                                                                        Environment.NewLine + " select * from tbve_estadocargaglobal " + Environment.NewLine &
                                                                                                        " where distribuidor = " + sDistribuidor + " And fechatraslado = @fechatralado " + Environment.NewLine &
                                                                                                        FinalizaTran + " tran test "



                                                    SQLsw.WriteLine(COMPROBACION)
                                                    SQLsw.WriteLine("ALTER TABLE tbve_unidad ENABLE TRIGGER VEtrg_tbve_Unidad ")
                                                    SQLsw.WriteLine("end")
                                                    SQLsw.Close()
                                                    sw.Close()

                                                    Try

                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                        process.Name = "EJECUTANDO SCRIPT [ " + NombreArchivo + ".sql ]<br/>DEL DISTRIBUIDOR  [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"


                                                        SQLCOMPROBACION = Persistencia.GetDataTable(" select * from tbve_estadocargaglobal " + Environment.NewLine &
                                                                                                    " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) And carga LIKE 'SYNC%'")

                                                        If Not SQLCOMPROBACION.Rows.Count > 0 Then

                                                            Try
                                                                If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                                                    Dim sqllines As String
                                                                    Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                                                        sqllines = FileReader.ReadToEnd
                                                                        Persistencia.EjecutarSQL(sqllines)

                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                        process.Name = "SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                        mensaje = mensaje + "<br/>SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                        If FinalizaTran = "rollback" Then mensaje = mensaje + " [MODO PRUEBA] "
                                                                        mensaje = mensaje + "<br/>[ " + NombreArchivo + ".sql ]"

                                                                        sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                    "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                    "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'PRUEBA'," Else sqlScript = sqlScript + "'CARGADO',"
                                                                        sqlScript = sqlScript + "'SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                                                        sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                                                        "    update tbve_estadocargaglobal " + Environment.NewLine &
                                                                        "    set fecharegistro = getdate(), observaciones = 'SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'CARGADO'"
                                                                        sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                                        Persistencia.GetDataTable(sqlScript)
                                                                        'Thread.Sleep(2000)

                                                                    End Using
                                                                End If

                                                            Catch ex As Exception
                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                                                sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                            "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                            "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                            " else " + Environment.NewLine &
                                                                            "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                                                            "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                                                Persistencia.EjecutarSQL(sqlScript)

                                                                'Thread.Sleep(2000)
                                                            End Try

                                                            'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf SendSQL), New Object() {VProcesos.m_usuario, sDistribuidor, CDate(sFechaTraslado).ToString("ddMMyy"), RutaArchivo + "/" + NombreArchivo + ".sql"})

                                                        End If


                                                        'Se comprime encabezados
                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                        process.Name = "EMPAQUETADO DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                        If File.Exists(RutaArchivo + "/" + NombreArchivo + ".txt") Then
                                                            Using zip As ZipFile = New ZipFile
                                                                zip.AddFile(RutaArchivo + "/" + NombreArchivo + ".txt", "")
                                                                zip.Save(RutaArchivo + "/" + NombreArchivo + ".zip")
                                                            End Using
                                                        End If

                                                        File.Delete(RutaArchivo + "/" + NombreArchivo + ".txt")

                                                        'Ciframos Archivos
                                                        '50%
                                                        'Thread.Sleep(2000)
                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                        process.Name = "CIFRADO DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                        'Usando el algoritmo AES con la Key y IV. 
                                                        If File.Exists(RutaArchivo + "/" + NombreArchivo + ".zip") Then CifrarArchivo(RutaArchivo + "/" + NombreArchivo + ".zip", RutaArchivo + "/" + NombreArchivo + ".dat", keypsswrd)

                                                        File.Delete(RutaArchivo + "/" + NombreArchivo + ".zip")


                                                        'Convertimos el archivo cifrado a una cadena de Bytes  
                                                        'Thread.Sleep(2000)
                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                        process.Name = "GENERANDO CADENA DE BYTES DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                        If File.Exists(RutaArchivo + "/" + NombreArchivo + ".dat") Then BytesEncabezado = System.IO.File.ReadAllBytes(RutaArchivo + "/" + NombreArchivo + ".dat")
                                                        File.Delete(RutaArchivo + "/" + NombreArchivo + ".dat")

                                                        'Convertimos el archivo a un String base64
                                                        'Para su envió dentro del XML
                                                        'Thread.Sleep(2000)
                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                        process.Name = "ENCRIPTANDO INFORMACIÓN DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                        If Not IsNothing(BytesEncabezado) Then Encabezados = Convert.ToBase64String(BytesEncabezado)


                                                        If Not IsNothing(Encabezados) And Not IsDBNull(Encabezados) And Len(Encabezados) > 0 Then

                                                            'Se inicia la creación del XML
                                                            Dim xml As String = "<?xml version='1.0' encoding='UTF-8' ?>"

                                                            'Agregamos los datos del distribuidor
                                                            xml += "<delivery Usuario ='" + VProcesos.m_usuario + "' Distribuidor='" & sDistribuidor & "' FechaTraslado='" & s2FechaTraslado & "' EjecutaScript='" + EjecutraScrit + "' FinalizaTransaccion = '" + FinalizaTran + "'>"
                                                            xml += "<documents>"

                                                            'Agregamos cada uno de los archivos, aunque como mínimo se puede tener solo 1. 
                                                            'Solo puede existir un archivo de cada tipo    

                                                            xml += GetFileXML("Encabezados", Encabezados.Trim, True)

                                                            xml += "</documents>"
                                                            xml += "</delivery>"
                                                            File.WriteAllText(RutaArchivo + "/" + NombreArchivo + ".xml", xml)

                                                            Try
                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                process.Name = "ENVIANDO INFORMACIÓN POR WEB SERVICE<br/>AL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ESPERE UN MOMENTO... ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                                mensajeWS = ""
                                                                consolida = New CargarDatos.WSCargaDatosSoapClient
                                                                consolida.Endpoint.Address = New EndpointAddress(RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")
                                                                mensajeWS = consolida.ComprobarCargaEAT(VProcesos.m_usuario, sDistribuidor, s2FechaTraslado)

                                                                If mensajeWS <> "OK" Then



                                                                    Try
                                                                        Dim arreglo As String()
                                                                        xmlDoc.LoadXml(xml)
                                                                        mensajeWS = ""
                                                                        consolida = New CargarDatos.WSCargaDatosSoapClient
                                                                        consolida.Endpoint.Address = New EndpointAddress(RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")

                                                                        mensajeWS = consolida.SendDatos(VProcesos.m_usuario, sDistribuidor, s2FechaTraslado, Encoding.[Default].GetBytes(xmlDoc.OuterXml)).Trim()
                                                                        arreglo = mensajeWS.Split("|")

                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                        process.Name = "INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>CON ESTATUS [" + arreglo(1) + "]"
                                                                        mensaje = mensaje + "<br/>INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>CON ESTATUS [" + arreglo(1) + "]"
                                                                        mensaje = mensaje + "<br/>[ " + arreglo(0) + " ]"

                                                                        sqlScript = " update tbve_estadocargaglobal " + Environment.NewLine &
                                                                                    " set ObservacionesRemoto = 'INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] CON ESTATUS [" + arreglo(1) + "]'" + Environment.NewLine
                                                                        sqlScript = sqlScript + " + ' [" + arreglo(0) + "] ' " + Environment.NewLine
                                                                        sqlScript = sqlScript + ", cargaremota = '" + arreglo(1) + "'" + Environment.NewLine
                                                                        sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"
                                                                        Persistencia.GetDataTable(sqlScript)
                                                                        'Thread.Sleep(2000)

                                                                        Try

                                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                                            process.Name = "ACTUALIZANDO ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ] ESPERE UN MOMENTO... ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                                            CambiarOpcion(RutaArchivo + NombreArchivo + ".sql", "@opcion as int = 1", "@opcion as int = 2")


                                                                            Try
                                                                                If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                                                                    Dim sqllines As String
                                                                                    Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                                                                        sqllines = FileReader.ReadToEnd
                                                                                        Persistencia.EjecutarSQL(sqllines)
                                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                                        process.Name = "SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                                        mensaje = mensaje + "<br/>SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                                        If FinalizaTran = "rollback" Then mensaje = mensaje + " [MODO PRUEBA] "
                                                                                        mensaje = mensaje + "<br/>[ " + NombreArchivo + ".sql ]"

                                                                                        sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                                    "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                                    "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'SYNC PRUEBA'," Else sqlScript = sqlScript + "'SYNC',"
                                                                                        sqlScript = sqlScript + "'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                                                                        sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                                                                        "    update tbve_estadocargaglobal " + Environment.NewLine &
                                                                                        "    set fecharegistro = getdate(), observaciones = 'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'SYNC PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'SYNC'"
                                                                                        sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                                                        Persistencia.GetDataTable(sqlScript)

                                                                                        File.Delete(RutaArchivo + "/" + NombreArchivo + ".xml")

                                                                                        'Thread.Sleep(2000)

                                                                                    End Using
                                                                                End If

                                                                            Catch ex As Exception
                                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                                process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                                mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                                                                sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                            "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                            "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                                            " else " + Environment.NewLine &
                                                                                            "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                                                                            "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                                                                Persistencia.EjecutarSQL(sqlScript)

                                                                                'Thread.Sleep(2000)
                                                                            End Try


                                                                        Catch ex As Exception
                                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                                            process.Name = "ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                            mensaje = mensaje + "<br/>ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>" + ex.Message

                                                                            'Thread.Sleep(2000)
                                                                        End Try

                                                                    Catch ex As Exception

                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                        process.Name = "ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                                        mensaje = mensaje + "<br/>ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                                        mensaje = mensaje + "<br/>" + Replace(ex.Message, System.Environment.NewLine, "<br/>")
                                                                        mensaje = mensaje + "<br/>" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx"
                                                                        sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                    "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,cargaremota,ObservacionesRemoto,usuario) " + Environment.NewLine &
                                                                                    "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                                    " else " + Environment.NewLine &
                                                                                    "	 update tbve_estadocargaglobal set fecharegistro = getdate(), cargaremota = 'ERROR' ,ObservacionesRemoto = 'ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]'" + Environment.NewLine &
                                                                                    "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "


                                                                        Persistencia.EjecutarSQL(sqlScript)


                                                                        'Thread.Sleep(2000)

                                                                    End Try


                                                                Else

                                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                                    process.Name = "YA EXISTE UNA CARGA DE UNIDADES EN EL SERVIDOR REMOTO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                    mensaje = mensaje + "<br/>YA EXISTE UNA CARGA DE UNIDADES EN EL SERVIDOR REMOTO DEL DISTRIBUIDOR  [ " + sDistribuidor + " " + NombreDistribuidor + " ]"

                                                                    sqlScript = " update tbve_estadocargaglobal " + Environment.NewLine &
                                                                     " set fecharegistro = getdate(), ObservacionesRemoto = 'INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] CON ESTATUS [OK]'" + Environment.NewLine
                                                                    sqlScript = sqlScript + " + ' [OK] ' " + Environment.NewLine
                                                                    sqlScript = sqlScript + ", cargaremota = 'OK'" + Environment.NewLine
                                                                    sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                                    Persistencia.GetDataTable(sqlScript)

                                                                    Try

                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                        process.Name = "ACTUALIZANDO ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ] ESPERE UN MOMENTO... ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                                        CambiarOpcion(RutaArchivo + NombreArchivo + ".sql", "@opcion as int = 1", "@opcion as int = 2")


                                                                        Try
                                                                            If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                                                                Dim sqllines As String
                                                                                Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                                                                    sqllines = FileReader.ReadToEnd
                                                                                    Persistencia.EjecutarSQL(sqllines)
                                                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                                                    process.Name = "SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                                    mensaje = mensaje + "<br/>SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                                    If FinalizaTran = "rollback" Then mensaje = mensaje + " [MODO PRUEBA] "
                                                                                    mensaje = mensaje + "<br/>[ " + NombreArchivo + ".sql ]"

                                                                                    sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                                "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                                "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                                                                    If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'SYNC PRUEBA'," Else sqlScript = sqlScript + "'SYNC',"
                                                                                    sqlScript = sqlScript + "'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                                                                    If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                                                                    sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                                                                    "    update tbve_estadocargaglobal " + Environment.NewLine &
                                                                                    "    set fecharegistro = getdate(), observaciones = 'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                                                                    If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                                                                    If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'SYNC PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'SYNC'"
                                                                                    sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                                                    Persistencia.GetDataTable(sqlScript)
                                                                                    File.Delete(RutaArchivo + "/" + NombreArchivo + ".xml")

                                                                                    'Thread.Sleep(2000)

                                                                                End Using
                                                                            End If

                                                                        Catch ex As Exception
                                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                                            process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                            mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                                                            sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                        "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                        "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                                        " else " + Environment.NewLine &
                                                                                        "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                                                                        "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                                                            Persistencia.EjecutarSQL(sqlScript)

                                                                            'Thread.Sleep(2000)
                                                                        End Try


                                                                    Catch ex As Exception
                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                        process.Name = "ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                        mensaje = mensaje + "<br/>ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>" + ex.Message

                                                                        'Thread.Sleep(2000)
                                                                    End Try



                                                                End If

                                                            Catch ex As Exception
                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                process.Name = "ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                                mensaje = mensaje + "<br/>ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                                mensaje = mensaje + "<br/>" + Replace(ex.Message, System.Environment.NewLine, "<br/>")
                                                                mensaje = mensaje + "<br/>" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx"
                                                                sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                            "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,cargaremota,ObservacionesRemoto,usuario) " + Environment.NewLine &
                                                                            "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                            " else " + Environment.NewLine &
                                                                            "	 update tbve_estadocargaglobal set fecharegistro = getdate(), cargaremota = 'ERROR' ,ObservacionesRemoto = 'ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]'" + Environment.NewLine &
                                                                            "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "


                                                                Persistencia.EjecutarSQL(sqlScript)

                                                                'Thread.Sleep(2000)
                                                            End Try

                                                        Else
                                                            mensaje = mensaje + "</br>" + "EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] NO TIENE INFORMACIÓN PARA SER ENVIADA "
                                                            'Thread.Sleep(2000)
                                                        End If


                                                    Catch ex As Exception
                                                        mensaje = mensaje + "</br>" + ex.Message
                                                    End Try


                                                    s2FechaTraslado = ""
                                                    NombreDistribuidor = ""
                                                    RutaServerWeb = ""
                                                    EjecutraScrit = ""
                                                    FinalizaTran = ""

                                                End If

                                                RutaArchivo = VProcesos.m_pathXML
                                                NombreArchivo = "EAT_" + Mid(eat.Item(0), 3, Len(eat.Item(0))) + "_" + CDate(sFechaTraslado).ToString("ddMMyy")
                                                countUnidades = 1
                                                COMPROBACION = ""

                                                SQLParametrosGRLS = Persistencia.GetDataTable(" select Distribuidor, Descripcion, isnull(RutaServerWeb,'') as RutaServerWeb, isnull(EjecutaScript,'False') as EjecutaScript , isnull(FinalizaTran,'rollback') as FinalizaTran " + Environment.NewLine &
                                                                                              " from tbcm_SucursalCon where bandMatriz = 1 And Distribuidor = " + Mid(eat.Item(0), 3, Len(eat.Item(0))))

                                                If SQLParametrosGRLS.Rows.Count > 0 Then

                                                    RutaServerWeb = SQLParametrosGRLS.Rows(0).Item("RutaServerWeb")
                                                    NombreDistribuidor = SQLParametrosGRLS.Rows(0).Item("Descripcion")
                                                    EjecutraScrit = SQLParametrosGRLS.Rows(0).Item("EjecutaScript")
                                                    FinalizaTran = SQLParametrosGRLS.Rows(0).Item("FinalizaTran")

                                                End If



                                                SQLsw = New StreamWriter(RutaArchivo + "/" + NombreArchivo + ".sql", False, System.Text.Encoding.UTF8)

                                                SQLsw.WriteLine("if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + Mid(eat.Item(0), 3, Len(eat.Item(0))) + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(sFechaTraslado).ToString("yyyy-MM-dd") + "')) And carga = 'OK')")
                                                SQLsw.WriteLine("begin")
                                                SQLsw.WriteLine("ALTER TABLE tbve_unidad DISABLE TRIGGER VEtrg_tbve_Unidad")
                                                SQLsw.WriteLine("begin tran test ")

                                                SQLsw.WriteLine("declare @fechatralado as int, @opcion as int = 1 ")
                                                SQLsw.WriteLine("set @fechatralado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(sFechaTraslado).ToString("yyyy-MM-dd") + "'))")

                                                SQLsw.WriteLine(" if not exists(select distribuidor from tbve_estadocargaglobal  where distribuidor = " + Mid(eat.Item(0), 3, Len(eat.Item(0))) + "  And fechatraslado = @fechatralado )  ")
                                                SQLsw.WriteLine(" insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,usuario)  ")
                                                SQLsw.WriteLine(" select  " + Mid(eat.Item(0), 3, Len(eat.Item(0))) + ",@fechatralado,0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',CASE WHEN @opcion= 1 THEN 'PROCESO' ELSE 'SYNC' END,'" + VProcesos.m_usuario + "'")
                                                SQLsw.WriteLine(" else  ")
                                                SQLsw.WriteLine(" update tbve_estadocargaglobal set fecharegistro = getdate(), carga = CASE WHEN @opcion= 1 THEN 'PROCESO' ELSE 'SYNC' END  where distribuidor = " + Mid(eat.Item(0), 3, Len(eat.Item(0))) + "  And fechatraslado = @fechatralado ")

                                                sw = New StreamWriter(RutaArchivo + "/" + NombreArchivo + ".txt", False, System.Text.Encoding.UTF8)

                                            End If

                                            sDistribuidor = Mid(eat.Item(0), 3, Len(eat.Item(0)))
                                            s2FechaTraslado = sFechaTraslado
                                            If FechaIni = "" Then FechaIni = s2FechaTraslado

                                            If Len(CStr(dPrecioF)) = 0 Or CDbl(dPrecioF) = 0 Then
                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                process.Name = "UNIDAD [" + sChasis + "] DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]  SIN COSTO ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                mensaje = mensaje + "</br> UNIDAD [" + sChasis + "] DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]  SIN COSTO "
                                                Thread.Sleep(20)
                                            End If


                                            process.Name = "REGISTRANDO UNIDAD [" + sChasis + "] EN ARCHIVO<br/>DEL DITRIBUIDOR [" + sDistribuidor + "] PARA SER EMPAQUETADO  ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                            If countUnidades = 1 Then

                                                COMPROBACION = COMPROBACION + Environment.NewLine + " if @opcion = 1  " + Environment.NewLine &
                                                                              " begin " + Environment.NewLine &
                                                                              Environment.NewLine + " select '" + sChasis.ToString + "' as vin " + Environment.NewLine &
                                                                              " into #vin" + sDistribuidor + Environment.NewLine
                                            Else
                                                COMPROBACION = COMPROBACION + " union " + Environment.NewLine &
                                                                              " select '" + sChasis.ToString + "' as vin " + Environment.NewLine
                                            End If
                                            Thread.Sleep(20)


                                            sw.WriteLine(sDistribuidor.ToString + "@|" + sOrdenCompra.ToString + "@|" + sChasis.ToString + "@|" + sFechaTraslado.ToString + "@|" + sNoMotor.ToString + "@|" &
                                                 sTipo.ToString + "@|" + sColorE.ToString + "@|" + iAnoModelo.ToString + "@|" &
                                                 sFechaRecibio.ToString + "@|" + sColorI.ToString + "@|" + sTipoCompra.ToString + "@|" &
                                                 sNoFactura + "@|" + dPrecioF.ToString + "@|" + sFechaF.ToString + "@|" + sFechaR.ToString + "@|" + sPedimento.ToString + "@|" + sFechaP.ToString + "@|" + sDepto.ToString + "@|")


                                            SQLsw.WriteLine("exec vespa_altaunidad @opcion, 'P', '" + sOrdenCompra.ToString + "', '" + sChasis.ToString + "', '" + sFechaTraslado.ToString + "', '" + sNoMotor.ToString + "', '" + sTipo.ToString + "', '" + sColorE.ToString &
                                                            "', " + iAnoModelo.ToString + ", 0, 0, '', '', '" + sFechaRecibio.ToString + "', 0, '', 0, '" + sDistribuidor.ToString + "', '" + sColorI.ToString + "', '" + sTipoCompra.ToString + "', '" + sNoFactura.ToString &
                                                            "', " + dPrecioF.ToString + ", '" + sFechaF.ToString + "', '" + sFechaR.ToString + "', '" + sPedimento.ToString + "', '" + sFechaP.ToString + "', '" + sDepto.ToString + "', '" + sTipo.ToString + "'")

                                            countUnidades = countUnidades + 1
                                            llcount = llcount + 1

                                        Catch ex As Exception
                                            mensaje = mensaje + "</br>" + ex.Message
                                            llcount = llcount + 1
                                        End Try
                                    Next

                                    llcount = llcount - 1
                                    If FechaFin = "" Then FechaFin = s2FechaTraslado

                                    If Not IsNothing(sw) And Not IsDBNull(sw) Then

                                        'Cierra ultimo archivo lo empaqueta y envía petición a cliente para entrega de información
                                        process.SetStatus(90)
                                        process.Name = "CERRANDO ARCHIVO Y MANDANDO EMPAQUETADO  DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                        countUnidades = countUnidades - 1

                                        COMPROBACION = COMPROBACION + Environment.NewLine + " select u.distribuidor, v.vin, u.fechaTraslado ,case when v.vin = u.vin And u.vin = u2.vin then 'OK' else 'ERROR' end as Cargado " + Environment.NewLine &
                                                                                            " into #estadocarga" + sDistribuidor + Environment.NewLine &
                                                                                            " from #vin" + sDistribuidor + " v " + Environment.NewLine &
                                                                                            " left join tbve_Unidad u on v.vin = u.Vin " + Environment.NewLine &
                                                                                            " left join tbve_unidad2 u2 on u.Vin = u2.Vin And u.fechaTraslado = u2.fechatraslado " + Environment.NewLine &
                                                                                            " where u.idStatus = 0 And u.fechatraslado = @fechatralado" + Environment.NewLine

                                        COMPROBACION = COMPROBACION + Environment.NewLine + " delete from tbve_estadocargaglobal  " + Environment.NewLine &
                                                                                            " where distribuidor = " + sDistribuidor + "  And fechatraslado = @fechatralado"


                                        COMPROBACION = COMPROBACION + Environment.NewLine + " select distribuidor,fechaTraslado, COUNT(VIN) as Unidades " + Environment.NewLine &
                                                                                            " ,case when Cargado ='OK' then  COUNT(VIN) else 0 end as cargadas " + Environment.NewLine &
                                                                                            " ,case when Cargado ='ERROR' then  COUNT(VIN) else 0 end as sincargar " + Environment.NewLine &
                                                                                            " ,'" + RutaArchivo + "' as ruta,'" + NombreArchivo + ".sql" + "' as archivo, '' as carga " + Environment.NewLine &
                                                                                            " into #previo" + sDistribuidor + Environment.NewLine &
                                                                                            " from #estadocarga" + sDistribuidor + Environment.NewLine &
                                                                                            " group by distribuidor,fechaTraslado, Cargado " + Environment.NewLine

                                        COMPROBACION = COMPROBACION + Environment.NewLine + " insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,usuario)  " + Environment.NewLine &
                                                                                            " select distribuidor,fechaTraslado, sum(unidades), sum(cargadas), sum(sincargar) " + Environment.NewLine &
                                                                                            " ,ruta " + Environment.NewLine &
                                                                                            " ,archivo " + Environment.NewLine &
                                                                                            " ,case when  sum(unidades)<>sum(cargadas) then 'ERROR' else 'OK' end " + Environment.NewLine &
                                                                                            " ,'" + VProcesos.m_usuario + "'" + Environment.NewLine &
                                                                                            " from #previo" + sDistribuidor + Environment.NewLine &
                                                                                            " group by  distribuidor,fechaTraslado,ruta ,archivo " + Environment.NewLine

                                        COMPROBACION = COMPROBACION + Environment.NewLine + " DROP TABLE #previo" + sDistribuidor.ToString + Environment.NewLine &
                                                                                            " DROP TABLE #estadocarga" + sDistribuidor.ToString + Environment.NewLine &
                                                                                            " DROP TABLE #vin" + sDistribuidor.ToString + Environment.NewLine &
                                                                                            Environment.NewLine + " end" + Environment.NewLine &
                                                                                            Environment.NewLine + " select * from tbve_estadocargaglobal " + Environment.NewLine &
                                                                                            " where distribuidor = " + sDistribuidor + " And fechatraslado = @fechatralado " + Environment.NewLine &
                                                                                            FinalizaTran + " tran test "


                                        'SQLsw.WriteLine("commit tran test ")
                                        SQLsw.WriteLine(COMPROBACION)
                                        SQLsw.WriteLine("ALTER TABLE tbve_unidad ENABLE TRIGGER VEtrg_tbve_Unidad ")
                                        SQLsw.WriteLine("end")
                                        SQLsw.Close()
                                        sw.Close()

                                        Try

                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "EJECUTANDO SCRIPT [ " + NombreArchivo + ".sql ]<br/>DEL DISTRIBUIDOR  [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"


                                            SQLCOMPROBACION = Persistencia.GetDataTable(" select * from tbve_estadocargaglobal " + Environment.NewLine &
                                                                                        " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) And carga LIKE 'SYNK%'")

                                            If Not SQLCOMPROBACION.Rows.Count > 0 Then

                                                Try
                                                    If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                                        Dim sqllines As String
                                                        Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                                            sqllines = FileReader.ReadToEnd
                                                            Persistencia.EjecutarSQL(sqllines)
                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                            process.Name = "SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                            mensaje = mensaje + "<br/>SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                            If FinalizaTran = "rollback" Then mensaje = mensaje + " [MODO PRUEBA] "
                                                            mensaje = mensaje + "<br/>[ " + NombreArchivo + ".sql ]"

                                                            sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                     "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                     "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'PRUEBA'," Else sqlScript = sqlScript + "'CARGADO',"
                                                            sqlScript = sqlScript + "'SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                                            sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                                            "    update tbve_estadocargaglobal " + Environment.NewLine &
                                                            "    set fecharegistro = getdate(), observaciones = 'SE CARGO INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'CARGADO'"
                                                            sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                            Persistencia.GetDataTable(sqlScript)
                                                            'Thread.Sleep(2000)

                                                        End Using
                                                    End If

                                                Catch ex As Exception
                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                    process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                    mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                                    sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                " else " + Environment.NewLine &
                                                                "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                                                "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                                    Persistencia.EjecutarSQL(sqlScript)

                                                    Thread.Sleep(2000)
                                                End Try

                                                'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf SendSQL), New Object() {VProcesos.m_usuario, sDistribuidor, CDate(sFechaTraslado).ToString("ddMMyy"), RutaArchivo + "/" + NombreArchivo + ".sql"})

                                            End If


                                            'Se comprime encabezados
                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "EMPAQUETADO DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                            If File.Exists(RutaArchivo + "/" + NombreArchivo + ".txt") Then
                                                Using zip As ZipFile = New ZipFile
                                                    zip.AddFile(RutaArchivo + "/" + NombreArchivo + ".txt", "")
                                                    zip.Save(RutaArchivo + "/" + NombreArchivo + ".zip")
                                                End Using
                                            End If

                                            File.Delete(RutaArchivo + "/" + NombreArchivo + ".txt")

                                            'Ciframos Archivos
                                            '50%
                                            'Thread.Sleep(2000)
                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "CIFRADO DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                            'Usando el algoritmo AES con la Key y IV. 
                                            If File.Exists(RutaArchivo + "/" + NombreArchivo + ".zip") Then CifrarArchivo(RutaArchivo + "/" + NombreArchivo + ".zip", RutaArchivo + "/" + NombreArchivo + ".dat", keypsswrd)

                                            File.Delete(RutaArchivo + "/" + NombreArchivo + ".zip")


                                            'Convertimos el archivo cifrado a una cadena de Bytes  
                                            'Thread.Sleep(2000)
                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "GENERANDO CADENA DE BYTES DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                            If File.Exists(RutaArchivo + "/" + NombreArchivo + ".dat") Then BytesEncabezado = System.IO.File.ReadAllBytes(RutaArchivo + "/" + NombreArchivo + ".dat")
                                            File.Delete(RutaArchivo + "/" + NombreArchivo + ".dat")

                                            'Convertimos el archivo a un String base64
                                            'Para su envió dentro del XML
                                            'Thread.Sleep(2000)
                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "ENCRIPTANDO INFORMACIÓN DE INFORMACIÓN DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                            If Not IsNothing(BytesEncabezado) Then Encabezados = Convert.ToBase64String(BytesEncabezado)


                                            If Not IsNothing(Encabezados) And Not IsDBNull(Encabezados) And Len(Encabezados) > 0 Then

                                                'Se inicia la creación del XML
                                                Dim xml As String = "<?xml version='1.0' encoding='UTF-8' ?>"

                                                'Agregamos los datos del distribuidor
                                                xml += "<delivery Usuario ='" + VProcesos.m_usuario + "' Distribuidor='" & sDistribuidor & "' FechaTraslado='" & s2FechaTraslado & "' EjecutaScript='" + EjecutraScrit + "' FinalizaTransaccion = '" + FinalizaTran + "'>"
                                                xml += "<documents>"

                                                'Agregamos cada uno de los archivos, aunque como mínimo se puede tener solo 1. 
                                                'Solo puede existir un archivo de cada tipo    

                                                xml += GetFileXML("Encabezados", Encabezados.Trim, True)

                                                xml += "</documents>"
                                                xml += "</delivery>"
                                                File.WriteAllText(RutaArchivo + "/" + NombreArchivo + ".xml", xml)

                                                Try
                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                    process.Name = "ENVIANDO INFORMACIÓN POR WEB SERVICE<br/>AL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] ESPERE UN MOMENTO... ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                    mensajeWS = ""
                                                    consolida = New CargarDatos.WSCargaDatosSoapClient
                                                    consolida.Endpoint.Address = New EndpointAddress(RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")
                                                    mensajeWS = consolida.ComprobarCargaEAT(VProcesos.m_usuario, sDistribuidor, s2FechaTraslado)

                                                    If mensajeWS <> "OK" Then
                                                        Try
                                                            Dim arreglo As String()
                                                            xmlDoc.LoadXml(xml)
                                                            mensajeWS = ""
                                                            consolida = New CargarDatos.WSCargaDatosSoapClient
                                                            consolida.Endpoint.Address = New EndpointAddress(RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx")

                                                            mensajeWS = consolida.SendDatos(VProcesos.m_usuario, sDistribuidor, s2FechaTraslado, Encoding.[Default].GetBytes(xmlDoc.OuterXml)).Trim()
                                                            arreglo = mensajeWS.Split("|")

                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                            process.Name = "INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>CON ESTATUS [" + arreglo(1) + "]"
                                                            mensaje = mensaje + "<br/>INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>CON ESTATUS [" + arreglo(1) + "]"
                                                            mensaje = mensaje + "<br/>[ " + arreglo(0) + " ]"

                                                            sqlScript = " update tbve_estadocargaglobal " + Environment.NewLine &
                                                                        " set fecharegistro = getdate(), ObservacionesRemoto = 'INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] CON ESTATUS [" + arreglo(1) + "]'" + Environment.NewLine
                                                            sqlScript = sqlScript + " + ' [" + arreglo(0) + "] ' " + Environment.NewLine
                                                            sqlScript = sqlScript + ", cargaremota = '" + arreglo(1) + "'" + Environment.NewLine
                                                            sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"
                                                            Persistencia.GetDataTable(sqlScript)
                                                            'Thread.Sleep(2000)
                                                            Try

                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                process.Name = "ACTUALIZANDO ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ] ESPERE UN MOMENTO... ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                                CambiarOpcion(RutaArchivo + NombreArchivo + ".sql", "@opcion as int = 1", "@opcion as int = 2")

                                                                Try
                                                                    If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                                                        Dim sqllines As String
                                                                        Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                                                            sqllines = FileReader.ReadToEnd
                                                                            Persistencia.EjecutarSQL(sqllines)
                                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                                            process.Name = "SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                            mensaje = mensaje + "<br/>SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                            If FinalizaTran = "rollback" Then mensaje = mensaje + " [MODO PRUEBA] "
                                                                            mensaje = mensaje + "<br/>[ " + NombreArchivo + ".sql ]"

                                                                            sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                        "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                        "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'SYNC PRUEBA'," Else sqlScript = sqlScript + "'SYNC',"
                                                                            sqlScript = sqlScript + "'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                                                            sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                                                            "    update tbve_estadocargaglobal " + Environment.NewLine &
                                                                            "    set fecharegistro = getdate(), observaciones = 'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                                                            If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'SYNC PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'SYNC'"
                                                                            sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                                            Persistencia.GetDataTable(sqlScript)
                                                                            File.Delete(RutaArchivo + "/" + NombreArchivo + ".xml")
                                                                            'Thread.Sleep(2000)

                                                                        End Using
                                                                    End If

                                                                Catch ex As Exception
                                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                                    process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                    mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                                                    sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                                " else " + Environment.NewLine &
                                                                                "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                                                                "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                                                    Persistencia.EjecutarSQL(sqlScript)

                                                                    'Thread.Sleep(2000)
                                                                End Try

                                                            Catch ex As Exception
                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                process.Name = "ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                mensaje = mensaje + "<br/>ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>" + ex.Message
                                                                'Thread.Sleep(2000)
                                                            End Try

                                                        Catch ex As Exception

                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                            process.Name = "ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                            mensaje = mensaje + "<br/>ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                            mensaje = mensaje + "<br/>" + Replace(ex.Message, System.Environment.NewLine, "<br/>")
                                                            mensaje = mensaje + "<br/>" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx"

                                                            sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                        "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,cargaremota,ObservacionesRemoto,usuario) " + Environment.NewLine &
                                                                        "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                        " else " + Environment.NewLine &
                                                                        "	 update tbve_estadocargaglobal set fecharegistro = getdate(), cargaremota = 'ERROR' ,ObservacionesRemoto = 'ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]'" + Environment.NewLine &
                                                                        "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "


                                                            Persistencia.EjecutarSQL(sqlScript)


                                                            'Thread.Sleep(2000)

                                                        End Try

                                                    Else

                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                        process.Name = "YA EXISTE UNA CARGA DE UNIDADES EN EL SERVIDOR REMOTO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                        mensaje = mensaje + "<br/>YA EXISTE UNA CARGA DE UNIDADES EN EL SERVIDOR REMOTO DEL DISTRIBUIDOR  [ " + sDistribuidor + " " + NombreDistribuidor + " ]"

                                                        sqlScript = " update tbve_estadocargaglobal " + Environment.NewLine &
                                                                       " set fecharegistro = getdate(), ObservacionesRemoto = 'INFORMACIÓN RECIBIDA POR EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] CON ESTATUS [OK]'" + Environment.NewLine
                                                        sqlScript = sqlScript + " + ' [OK] ' " + Environment.NewLine
                                                        sqlScript = sqlScript + ", cargaremota = 'OK'" + Environment.NewLine
                                                        sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"
                                                        Persistencia.GetDataTable(sqlScript)


                                                        Try

                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                            process.Name = "ACTUALIZANDO ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ] ESPERE UN MOMENTO... ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                            CambiarOpcion(RutaArchivo + NombreArchivo + ".sql", "@opcion as int = 1", "@opcion as int = 2")


                                                            Try
                                                                If File.Exists(RutaArchivo + "/" + NombreArchivo + ".sql") Then
                                                                    Dim sqllines As String
                                                                    Using FileReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(RutaArchivo + "/" + NombreArchivo + ".sql")

                                                                        sqllines = FileReader.ReadToEnd
                                                                        Persistencia.EjecutarSQL(sqllines)
                                                                        process.SetStatus(CInt((llcount * 90) / rcta))
                                                                        process.Name = "SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                        mensaje = mensaje + "<br/>SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                                        If FinalizaTran = "rollback" Then mensaje = mensaje + " [MODO PRUEBA] "
                                                                        mensaje = mensaje + "<br/>[ " + NombreArchivo + ".sql ]"

                                                                        sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                                    "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                                    "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql',"
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + "'SYNC PRUEBA'," Else sqlScript = sqlScript + "'SYNC',"
                                                                        sqlScript = sqlScript + "'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]"
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " [MODO PRUEBA]'," Else sqlScript = sqlScript + "',"
                                                                        sqlScript = sqlScript + "'" + VProcesos.m_usuario + "' " + Environment.NewLine + " else " + Environment.NewLine &
                                                                        "    update tbve_estadocargaglobal " + Environment.NewLine &
                                                                        "    set fecharegistro = getdate(), observaciones = 'SE SINCRONIZÓ INFORMACIÓN EN SERVIDOR CENTRAL DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " + ' [MODO PRUEBA] ' " + Environment.NewLine
                                                                        If FinalizaTran = "rollback" Then sqlScript = sqlScript + " ,carga = 'SYNC PRUEBA'" Else sqlScript = sqlScript + " ,carga = 'SYNC'"
                                                                        sqlScript = sqlScript + " where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "'))"

                                                                        Persistencia.GetDataTable(sqlScript)
                                                                        File.Delete(RutaArchivo + "/" + NombreArchivo + ".xml")
                                                                        'Thread.Sleep(2000)

                                                                    End Using
                                                                End If

                                                            Catch ex As Exception
                                                                process.SetStatus(CInt((llcount * 90) / rcta))
                                                                process.Name = "ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ] ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                                mensaje = mensaje + "<br/>ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ " + NombreArchivo + ".sql ]"

                                                                sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                            "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,carga,Observaciones,usuario) " + Environment.NewLine &
                                                                            "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                            " else " + Environment.NewLine &
                                                                            "	 update tbve_estadocargaglobal set fecharegistro = getdate(), carga = 'ERROR' ,Observaciones = 'ERROR AL EJECUTAR EL ARCHIVO DEL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ " + NombreArchivo + ".sql ]'" + Environment.NewLine &
                                                                            "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "

                                                                Persistencia.EjecutarSQL(sqlScript)

                                                                'Thread.Sleep(2000)
                                                            End Try


                                                        Catch ex As Exception
                                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                                            process.Name = "ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]"
                                                            mensaje = mensaje + "<br/>ERROR AL ACTUALIZAR ESTATUS DE UNIDADES DEL DISTRIBUIDOR A SINCRONIZADAS [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>" + ex.Message

                                                            'Thread.Sleep(2000)
                                                        End Try



                                                    End If
                                                Catch ex As Exception
                                                    process.SetStatus(CInt((llcount * 90) / rcta))
                                                    process.Name = "ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                    mensaje = mensaje + "<br/>ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ]<br/>[ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]"
                                                    mensaje = mensaje + "<br/>" + Replace(ex.Message, System.Environment.NewLine, "<br/>")
                                                    mensaje = mensaje + "<br/>" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx"
                                                    sqlScript = " if not exists(select distribuidor from tbve_estadocargaglobal where distribuidor = " + sDistribuidor + "  And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) ) " + Environment.NewLine &
                                                                "	 insert into tbve_estadocargaglobal (distribuidor,fechatraslado,unidades,cargadas,sincargar,Ruta,Archivo,cargaremota,ObservacionesRemoto,usuario) " + Environment.NewLine &
                                                                "	 select  " + sDistribuidor + ",datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')),0,0,0,'" + RutaArchivo + "','" + NombreArchivo + ".sql','ERROR','ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]','" + VProcesos.m_usuario + "' " + Environment.NewLine &
                                                                " else " + Environment.NewLine &
                                                                "	 update tbve_estadocargaglobal set fecharegistro = getdate(), cargaremota = 'ERROR' ,ObservacionesRemoto = 'ERROR DE COMUNICACIÓN CON EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] [ INFORMACIÓN NO CARGADA EN SERVIDOR REMOTO ]'" + Environment.NewLine &
                                                                "	 where distribuidor = " + sDistribuidor + " And fechatraslado = datediff(day,'12/28/1800',convert(datetime,'" + CDate(s2FechaTraslado).ToString("yyyy-MM-dd") + "')) "


                                                    Persistencia.EjecutarSQL(sqlScript)

                                                    Thread.Sleep(2000)
                                                End Try

                                            Else
                                                mensaje = mensaje + "</br>" + "EL DISTRIBUIDOR [ " + sDistribuidor + " " + NombreDistribuidor + " ] NO TIENE INFORMACIÓN PARA SER ENVIADA "
                                                Thread.Sleep(2000)
                                            End If


                                        Catch ex As Exception
                                            mensaje = mensaje + "</br>" + ex.Message
                                        End Try

                                        s2FechaTraslado = ""
                                        NombreDistribuidor = ""
                                        RutaServerWeb = ""

                                    End If

                                    Try
                                        If Not FileIO.FileSystem.FileExists("c:\texto\" + "BAT_iniciaragente.bat") Then
                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "INICIANDO AGENTE SQL SERVER ESPERE UN MOMENTO..."
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

                                            Thread.Sleep(20000)
                                        End If
                                    Catch ex As Exception

                                    End Try


                                End If

                            End If

                        End If
                    Catch ex As Exception
                        Thread.Sleep(10)
                        mensaje = mensaje + "</br>" + ex.Message
                        process.SetStatus(100)
                        process.Name = "ERROR AL PROCESAR EL ARCHIVO<br/>" '+ mensaje
                    End Try

                End If

                '100%
                process.SetStatus(100)
                process.Name = "IMPORTACIÓN FINALIZADA<br/>" '+ mensaje


            Case "IMPORTAR"

                Dim keypsswrd = AesUtil.GetAesKeys("G@V18TU4L")
                Dim razon As Integer = 0
                Dim agencia As Integer = 0
                Dim ejercicio As Integer = 0
                Dim periodo As Integer = 0
                Dim tabla As DataTable = New DataTable
                Dim SQLDC As DataTable = New DataTable
                Dim SQLProv As DataTable = New DataTable
                Dim TextFile As String = ""
                Dim mensaje As String = ""

                If Archivo.Trim.Length > 0 Then
                    Try


                        If Path.GetExtension(Archivo) = ".xls" Or Path.GetExtension(Archivo) = ".xlsx" Then

                            If FileIO.FileSystem.FileExists(Archivo) Then

                                process.SetStatus(10)
                                process.Name = "OBTENIENDO INFORMACIÓN DE ARCHIVO"

                                Dim fileinfo As System.IO.FileInfo = FileIO.FileSystem.GetFileInfo(Archivo)

                                Dim connString As String

                                If fileinfo.Extension = ".xls" Then
                                    connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Archivo + ";Extended Properties=Excel 8.0;"
                                Else
                                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Archivo + ";Extended Properties=Excel 12.0 Xml;"
                                End If
                                ' Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filename + ";Extended Properties=Excel 8.0;"

                                process.SetStatus(15)
                                process.Name = "ESTABLECIENDO CONEXIÓN PARA EXTRAER INFORMACIÓN"

                                ' Create the connection object
                                Dim oledbConn As OleDbConnection = New OleDbConnection(connString)
                                Try

                                    process.SetStatus(15)
                                    process.Name = "ABRIENDO CONEXIÓN"

                                    ' Open connection
                                    oledbConn.Open()
                                    ' Create OleDbCommand object and select data from worksheet Sheet1
                                    Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [Hoja1$]", oledbConn)

                                    ' Create new OleDbDataAdapter
                                    Dim oleda As OleDbDataAdapter = New OleDbDataAdapter()

                                    oleda.SelectCommand = cmd

                                    ' Create a DataSet which will hold the data extracted from the worksheet.
                                    Dim ds As DataSet = New DataSet()

                                    ' Fill the DataSet from the data extracted from the worksheet.
                                    oleda.Fill(ds, "Hoja1")


                                    Dim dt As DataTable = ds.Tables("Hoja1")

                                    If dt.Rows.Count > 0 Then

                                        Dim llcount = 1
                                        Dim rcta = dt.Rows.Count
                                        Dim dtRazon As DataTable = Nothing
                                        process.SetStatus(15)
                                        process.Name = "INICIA LECTURA DEL ARCHIVO"


                                        agencia = dt.Rows(0).Item("Distribuidor")
                                        dtRazon = Persistencia.GetDataTable("select idEmpresa, Descripcion from tbcm_SucursalCon where Distribuidor = " + agencia.ToString + " And bandMatriz = 1")
                                        razon = dtRazon.Rows(0).Item("idEmpresa")
                                        ejercicio = dt.Rows(0).Item("Ejercicio")
                                        periodo = dt.Rows(0).Item("Periodo")

                                        SQLProv = Persistencia.GetDataTable("select iddistribuidor from tbco_consolida where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
                                        If SQLProv.Rows.Count > 0 Then
                                            Persistencia.EjecutarSQL("update tbco_consolida set estado = 'P', fechaini = getdate(), fechafin = getdate(), usuario = '" + myCookie("Usuario") + "' where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
                                        Else
                                            Persistencia.EjecutarSQL(" insert into tbco_consolida (iddistribuidor, ejercicio, periodo, usuario, estado, fechaini, fechafin) " +
                                                                     " values (" + agencia.ToString + "," + ejercicio.ToString + "," + periodo.ToString + ",'" + myCookie("Usuario") + "','P', getdate(), getdate())")
                                        End If




                                        For Each cuenta In dt.AsEnumerable

                                            process.SetStatus(CInt((llcount * 90) / rcta))
                                            process.Name = "AGREGANDO REGISTROS DE BALANZA [" + dtRazon.Rows(0).Item("Descripcion") + "] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                            Try
                                                'Inicia importación de balanza de COMPROBACION. 
                                                If Not IsNothing(cuenta.Item(0).ToString.Trim) And cuenta.Item("Clave").ToString.Trim <> "" And Not IsDBNull(cuenta.Item("Clave").ToString.Trim) Then

                                                    Dim dtCuenta As DataTable = Nothing
                                                    Dim dtExisteCuenta As DataTable = Nothing
                                                    dtCuenta = Persistencia.GetDataTable("select * from tbco_catalogo where clave like '" + cuenta.Item("Clave").ToString.Trim + "' And idDistribuidor = case when nivel < 7 then 0 else " + agencia.ToString + " end ")

                                                    If dtCuenta.Rows.Count > 0 Then

                                                        process.Name = "INTENTANDO INTEGRA CUENTA [" + dtRazon.Rows(0).Item("Descripcion") + "]</br>[ " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " ] ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                        Persistencia.EjecutarSQL("if not exists (select idcta from tbco_saldosC where cuentanum = " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " And idEmpresa = " + dtCuenta.Rows(0).Item("idEmpresa").ToString + " And idSucursal = " + dtCuenta.Rows(0).Item("idSucursal").ToString + " And idDistribuidor = " + cuenta.Item("Distribuidor").ToString.Trim + " And idPeriodo = " + cuenta.Item("Periodo").ToString + " And idEjercicio = " + cuenta.Item("Ejercicio").ToString + " )" +
                                                                                 " insert into tbco_SaldosC (idCta,CuentaNum,idEmpresa,idSucursal,idDistribuidor, " +
                                                                                 " idEjercicio, idPeriodo, SaldoIni, SaldoFin, FechaModif, " +
                                                                                 " Cargos, Abonos,Status,Movimientos )" +
                                                                                 " values (" + dtCuenta.Rows(0).Item("idcta").ToString + "," + dtCuenta.Rows(0).Item("CuentaNum").ToString + "," + dtCuenta.Rows(0).Item("idEmpresa").ToString + "," + dtCuenta.Rows(0).Item("idSucursal").ToString + "," + cuenta.Item("Distribuidor").ToString.Trim +
                                                                                 " ," + cuenta.Item("Ejercicio").ToString + "," + cuenta.Item("Periodo").ToString + "," + cuenta.Item("SaldoIni").ToString + "," + cuenta.Item("SaldoFin").ToString + ",'" + Now.Date.ToString("yyyy-MM-dd") + "'," +
                                                                                 cuenta.Item("Cargos").ToString + "," + cuenta.Item("Abonos").ToString + ",'C',0)" +
                                                                                 " else " +
                                                                                 " update tbco_saldosC " +
                                                                                 " set SaldoIni = " + cuenta.Item("SaldoIni").ToString +
                                                                                 " ,SaldoFin = " + cuenta.Item("SaldoFin").ToString +
                                                                                 " ,Cargos = " + cuenta.Item("Cargos").ToString +
                                                                                 " ,Abonos = " + cuenta.Item("Abonos").ToString +
                                                                                 " ,Movimientos = 0" +
                                                                                 " ,FechaModif = '" + Now.Date.ToString("yyyy-MM-dd") + "'" +
                                                                                 " where cuentanum = " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " And idEmpresa = " + dtCuenta.Rows(0).Item("idEmpresa").ToString + " And idSucursal = " + dtCuenta.Rows(0).Item("idSucursal").ToString +
                                                                                 " And idDistribuidor = " + cuenta.Item("Distribuidor").ToString.Trim + " And idPeriodo = " + cuenta.Item("Periodo").ToString + " And idEjercicio = " + cuenta.Item("Ejercicio").ToString)

                                                        dtExisteCuenta = Persistencia.GetDataTable("select * from tbco_saldosC where cuentanum = " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " And idEmpresa = " + dtCuenta.Rows(0).Item("idEmpresa").ToString + " And idSucursal = " + dtCuenta.Rows(0).Item("idSucursal").ToString + " And idDistribuidor = " + cuenta.Item("Distribuidor").ToString.Trim + " And idPeriodo = " + cuenta.Item("Periodo").ToString + " And idEjercicio = " + cuenta.Item("Ejercicio").ToString)

                                                        If Not dtExisteCuenta.Rows.Count > 0 Then
                                                            process.Name = "LA CUENTA [ " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " ]</br>NO SE HA PODIDO REGISTRAR ( " + llcount.ToString + " / " + rcta.ToString + " )"
                                                            mensaje = mensaje + "</br> LA CUENTA [ " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " ] NO SE HA PODIDO REGISTRAR "
                                                        End If

                                                    End If


                                                End If


                                            Catch ex As Exception
                                                mensaje = mensaje + "</br>" + ex.Message
                                                process.SetStatus(100)
                                                process.Name = "ERROR AL PROCESAR EL ARCHIVO " + mensaje
                                            End Try

                                            llcount += 1

                                        Next

                                        If mensaje = "" Then
                                            'INICIA GENERACIÓN DE XML PARA BALANZA DE COMPROBACION
                                            Try
                                                'Atributo requerido para la expresión de la versión del formato
                                                'Thread.Sleep(2000)
                                                process.SetStatus(10)
                                                process.Name = "GENERANDO ENCABEZADO XML"
                                                blz.Version = "1.1"
                                                cadenaorginal = "||1.1"

                                                'Atributo requerido para expresar el RFC del contribuyente que envía los datos
                                                SQLTable = Persistencia.GetDataTable("select upper(rtrim(RFC)) as RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param where idRazonsocial = " + razon.ToString)
                                                If SQLTable.Rows.Count > 0 Then
                                                    blz.RFC = SQLTable.Rows(0).Item("RFC")
                                                    blz.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                                                    blz.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                                                    psw = SQLTable.Rows(0).Item("PswKey")
                                                    key = SQLTable.Rows(0).Item("RutaKey")
                                                    'ADOCFD.archivoprivk = key
                                                    'ADOCFD.passwordprivk = psw
                                                Else
                                                    blz.RFC = "XAXX010101000"
                                                    blz.noCertificado = "00001000000300527322"
                                                End If

                                                cadenaorginal += "|" + blz.RFC.Trim

                                                'Atributo requerido para expresar el mes al que 
                                                'corresponden la balanza
                                                blz.Mes = periodo.ToString.PadLeft(2, "0")
                                                cadenaorginal += "|" + periodo.ToString.PadLeft(2, "0")

                                                'Atributo requerido para expresar el año al que 
                                                'corresponden la balanza

                                                blz.Anio = ejercicio
                                                cadenaorginal += "|" + ejercicio.ToString

                                                'Atributo requerido para expresar el tipo de envío de la balanza (N - Normal; C - Complementaria)
                                                SQLTable = Persistencia.GetDataTable("select MAX(FECHA) AS FECHA from tbco_HistoricoCE where tiporeporte like 'BLZ' And rfc like '" + blz.RFC + "' And Periodo = " + periodo.ToString + "  And Ejercicio = " + ejercicio.ToString + " And TipoEnvio = 'N' And Folio is not null And DocumentoAcuse is not null ")
                                                If SQLTable.Rows.Count > 0 And IsDate(SQLTable.Rows(0).Item("FECHA")) Then

                                                    blz.TipoEnvio = "C"
                                                    cadenaorginal += "|" + blz.TipoEnvio

                                                    blz.FechaModBalSpecified = True
                                                    blz.FechaModBal = CDate(SQLTable.Rows(0).Item("FECHA")).ToString("yyyy-MM-dd")

                                                    cadenaorginal += "|" + blz.FechaModBal.ToString

                                                    Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " +
                                                                    "values ('" + blz.RFC + "'," + periodo.ToString + "," + ejercicio.ToString + ",'BLZ','C','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")

                                                Else



                                                    blz.TipoEnvio = "N"
                                                    cadenaorginal += "|" + blz.TipoEnvio

                                                    Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " +
                                                                    "values ('" + blz.RFC + "'," + periodo.ToString + "," + ejercicio.ToString + ",'BLZ','N','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")
                                                End If




                                                'AGREGANDO REGISTROS A LA BALANZA
                                                SQLCuenta = Persistencia.GetDataTable("exec COspa_RptBalanzaC 'L', " + razon.ToString + ", NULL, 5," + ejercicio.ToString + "," + periodo.ToString + ",'E'")
                                                Dim lstcta As New List(Of BalanzaCtas)
                                                If SQLCuenta.Rows.Count > 0 Then
                                                    llcount = 1
                                                    rcta = SQLCuenta.Rows.Count
                                                    For Each cta In SQLCuenta.AsEnumerable
                                                        'INCREMENTANDO % PROGRESO DE CUENTAS
                                                        process.SetStatus(CInt((llcount * 80) / rcta))
                                                        process.Name = "AGREGANDO CUENTAS ( " + llcount.ToString + " / " + rcta.ToString + " )"

                                                        Dim blzcta As BalanzaCtas = New BalanzaCtas()

                                                        blzcta.NumCta = cta.Item("CLAVELOCAL")
                                                        blzcta.SaldoIni = cta.Item("SALDOINI")
                                                        blzcta.Debe = cta.Item("TOTALCARGOS")
                                                        blzcta.Haber = cta.Item("TOTALABONOS")
                                                        blzcta.SaldoFin = cta.Item("ACUMULADO")

                                                        cadenaorginal += "|" + blzcta.NumCta + "|" + blzcta.SaldoIni.ToString + "|" + blzcta.Debe.ToString + "|" + blzcta.Haber.ToString + "|" + blzcta.SaldoFin.ToString

                                                        llcount += 1
                                                        lstcta.Add(blzcta)
                                                    Next

                                                    blz.Ctas = lstcta.ToArray
                                                End If

                                                cadenaorginal += "||"

                                                Dim StringWriter As New System.IO.StringWriter
                                                Dim XsltTransformation As New XslCompiledTransform(True)
                                                Dim XsltArgumentList As New XsltArgumentList

                                                Dim request_serializer As XmlSerializer = Nothing
                                                Dim request_writer As StreamWriter = Nothing

                                                '85%
                                                process.SetStatus(85)
                                                process.Name = "GENERANDO ARCHIVO XML PARA CADENA"
                                                request_serializer = New XmlSerializer(GetType(Balanza))
                                                request_writer = New StreamWriter(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml")
                                                request_serializer.Serialize(request_writer, blz)
                                                request_writer.Close()


                                                '90%
                                                process.SetStatus(90)
                                                process.Name = "GENERANDO CADENA ORIGINAL"
                                                Dim StylesheetPath As String = VProcesos.m_pathXSLT + "BalanzaCOMPROBACION_1_1.xslt"
                                                Dim SitemapPath As String = VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml"

                                                Try
                                                    XsltTransformation.Load(StylesheetPath)
                                                    XsltTransformation.Transform(SitemapPath, XsltArgumentList, StringWriter)
                                                Catch ex As Xsl.XsltException
                                                    Throw ex
                                                Catch ex As Exception
                                                    Throw ex
                                                End Try
                                                cadenaorginal = StringWriter.ToString()

                                                'GENERANDO SELLO DE CATÁLOGO 
                                                process.SetStatus(95)
                                                process.Name = "GENERANDO SELLO DE CUENTAS"

                                                'DEFINE FECHA PARA GENERAR SELLO
                                                Dim sello = GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw)
                                                blz.Sello = sello

                                                '99%
                                                process.SetStatus(99)
                                                process.Name = "GENERANDO ARCHIVO XML CON SELLO"
                                                request_serializer = New XmlSerializer(GetType(Balanza))
                                                request_writer = New StreamWriter(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml")
                                                request_serializer.Serialize(request_writer, blz)
                                                request_writer.Close()

                                                '100%
                                                process.SetStatus(100)
                                                process.Name = "COMPRIMIENDO ARCHIVO A ZIP"
                                                Using zip As ZipFile = New ZipFile
                                                    zip.AddFile(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml", "")
                                                    zip.Save(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".zip")

                                                End Using
                                                File.Delete(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml")

                                            Catch ex As Exception
                                                process.SetStatus(100)
                                                process.Name = "ERROR AL GENERAR XML DE LA BALANZA</br>" + ex.Message
                                            End Try
                                        End If

                                    End If

                                    process.SetStatus(90)
                                    process.Name = "ACTUALIZANDO ESTATUS..."
                                    Persistencia.EjecutarSQL("update tbco_consolida set estado = 'G', fechafin = getdate() where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)

                                Catch ex As Exception

                                    mensaje = mensaje + "</br>" + ex.Message
                                    process.SetStatus(100)
                                    process.Name = "ERROR AL PROCESAR EL ARCHIVO " + mensaje

                                End Try

                            End If

                        End If


                    Catch ex As Exception
                        mensaje = mensaje + "</br>" + ex.Message
                        process.SetStatus(100)
                        process.Name = "ERROR AL PROCESAR EL ARCHIVO " + mensaje
                    End Try

                End If

                '100%
                process.SetStatus(100)
                process.Name = "IMPORTACIÓN FINALIZADA " + mensaje

            Case "CATALOGO"
                Try

                    'Atributo requerido para la expresión de la versión
                    'del(formato)
                    '10%
                    'Thread.Sleep(2000)
                    process.SetStatus(10)
                    process.Name = "GENERANDO ENCABEZADO XML"
                    cat.Version = "1.1"
                    cadenaorginal = "||1.1"

                    'Atributo requerido para expresar el RFC del
                    'contribuyente que envía los datos

                    'Atributo opcional para expresar el número de serie del 
                    'certificado de sello digital que ampara el archivo de contabilidad electrónica, 
                    'de acuerdo al acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.

                    SQLTable = Persistencia.GetDataTable("select upper(rtrim(RFC)) as RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param where idRazonsocial = " + VProcesos.m_razon.ToString)
                    If SQLTable.Rows.Count > 0 Then
                        cat.RFC = SQLTable.Rows(0).Item("RFC")
                        cat.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        cat.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")
                        'ADOCFD.archivoprivk = key
                        'ADOCFD.passwordprivk = psw
                    Else
                        cat.RFC = "XAXX010101000"
                        cat.noCertificado = "00001000000300527322"
                    End If

                    cadenaorginal += "|" + cat.RFC.Trim


                    'Atributo requerido para expresar el mes en que
                    'inicia la vigencia del catálogo para la balanza
                    cat.Mes = Now.Month.ToString().PadLeft(2, "0")
                    cadenaorginal += "|" + Now.Month.ToString().PadLeft(2, "0")
                    'Atributo requerido para expresar el año en que inicia
                    'la vigencia del catálogo para la balanza
                    cat.Anio = Now.Year
                    cadenaorginal += "|" + Now.Year.ToString

                    'Atributo requerido para expresar el tipo de envío de la balanza (N - Normal; C - Complementaria)
                    SQLTable = Persistencia.GetDataTable("select MAX(FECHA) AS FECHA from tbco_HistoricoCE where tiporeporte like 'CTL' And rfc like '" + cat.RFC + "' And Periodo = " + cat.Mes.ToString + "  And Ejercicio = " + cat.Anio.ToString + " And TipoEnvio = 'N' And Folio is not null And DocumentoAcuse is not null ")
                    If SQLTable.Rows.Count > 0 And IsDate(SQLTable.Rows(0).Item("FECHA")) Then

                        Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " +
                                                 "   values ('" + cat.RFC + "'," + cat.Mes.ToString + "," + cat.Anio.ToString + ",'CTL','C','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")

                    Else

                        Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " +
                                                 "   values ('" + cat.RFC + "'," + cat.Mes.ToString + "," + cat.Anio.ToString + ",'CTL','N','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")
                    End If


                    'AGREGANDO CUENTAS AL CATÁLOGO
                    Dim idx As Integer = 0
                    SQLCuenta = Persistencia.GetDataTable("select * from tbco_catalogo where CodigoSat is not null And CodigoSat <> ''")
                    Dim lstcta As New List(Of CatalogoCtas)
                    If SQLCuenta.Rows.Count > 0 Then
                        Dim llcount = 1
                        Dim rcta = SQLCuenta.Rows.Count
                        For Each cta In SQLCuenta.AsEnumerable
                            'INCREMENTANDO % PROGRESO DE POLIZAS
                            'Thread.Sleep(10)
                            process.SetStatus(CInt((llcount * 80) / rcta))
                            process.Name = "AGREGANDO CUENTAS ( " + llcount.ToString + " / " + rcta.ToString + " )"

                            Dim cuenta As CatalogoCtas = New CatalogoCtas()
                            cuenta.CodAgrup = cta.Item("CodigoSat")
                            cuenta.NumCta = cta.Item("CLAVE")
                            cuenta.Desc = cta.Item("NOMBRE")

                            SQLSubCuenta = Persistencia.GetDataTable("select * from tbco_catalogo where CuentaNum =  " + cta.Item("idpadre").ToString)
                            If SQLSubCuenta.Rows.Count > 0 Then
                                cuenta.SubCtaDe = SQLSubCuenta.Rows(0).Item("CLAVE")
                            Else
                                cuenta.SubCtaDe = ""
                            End If

                            cuenta.Nivel = cta.Item("NIVEL")
                            cuenta.Natur = cta.Item("NATURALEZA")

                            cadenaorginal += "|" + cuenta.CodAgrup.Trim + "|" + cuenta.NumCta.Trim + "|" + cuenta.Desc.Trim + "|" + cuenta.SubCtaDe.Trim + "|" + cuenta.Nivel.ToString + "|" + cuenta.Natur.Trim
                            llcount += 1
                            lstcta.Add(cuenta)
                        Next
                        cat.Ctas = lstcta.ToArray
                    End If
                    cadenaorginal += "||"

                    Dim StringWriter As New System.IO.StringWriter
                    Dim XsltTransformation As New XslCompiledTransform(True)
                    Dim XsltArgumentList As New XsltArgumentList

                    Dim request_serializer As XmlSerializer = Nothing
                    Dim request_writer As StreamWriter = Nothing

                    '85%
                    process.SetStatus(85)
                    process.Name = "GENERANDO ARCHIVO XML PARA CADENA"
                    request_serializer = New XmlSerializer(GetType(Catalogo))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + cat.RFC + cat.Anio.ToString + CInt(cat.Mes).ToString.PadLeft(2, "0") + "CT.xml")
                    request_serializer.Serialize(request_writer, cat)
                    request_writer.Close()


                    '90%
                    process.SetStatus(90)
                    process.Name = "GENERANDO CADENA ORIGINAL"
                    Dim StylesheetPath As String = VProcesos.m_pathXSLT + "CatalogoCuentas_1_1.xslt"
                    Dim SitemapPath As String = VProcesos.m_pathXML + cat.RFC + cat.Anio.ToString + CInt(cat.Mes).ToString.PadLeft(2, "0") + "CT.xml"

                    Try
                        XsltTransformation.Load(StylesheetPath)
                        XsltTransformation.Transform(SitemapPath, XsltArgumentList, StringWriter)
                    Catch ex As Xsl.XsltException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    End Try
                    cadenaorginal = StringWriter.ToString()

                    'GENERANDO SELLO DE CATÁLOGO 
                    process.SetStatus(95)
                    process.Name = "GENERANDO SELLO DE CUENTAS"

                    'DEFINE FECHA PARA GENERAR SELLO
                    'ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw) 'ADOCFD.GenerarSello(cadenaorginal) 
                    cat.Sello = sello

                    '99%
                    process.SetStatus(99)
                    process.Name = "GENERANDO ARCHIVO XML CON SELLO"
                    request_serializer = New XmlSerializer(GetType(Catalogo))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + cat.RFC + cat.Anio.ToString + CInt(cat.Mes).ToString.PadLeft(2, "0") + "CT.xml")
                    request_serializer.Serialize(request_writer, cat)
                    request_writer.Close()


                    '100%
                    process.SetStatus(100)
                    process.Name = "COMPRIMIENDO ARCHIVO A ZIP"
                    Using zip As ZipFile = New ZipFile
                        zip.AddFile(VProcesos.m_pathXML + cat.RFC + cat.Anio.ToString + CInt(cat.Mes).ToString.PadLeft(2, "0") + "CT.xml", "")
                        zip.Save(VProcesos.m_pathXML + cat.RFC + cat.Anio.ToString + CInt(cat.Mes).ToString.PadLeft(2, "0") + "CT.zip")

                    End Using
                    File.Delete(VProcesos.m_pathXML + cat.RFC + cat.Anio.ToString + CInt(cat.Mes).ToString.PadLeft(2, "0") + "CT.xml")


                Catch ex As Exception
                    process.SetStatus(100)
                    process.Name = "ERROR AL GENERARL EL ARCHIVO </br>" + ex.Message
                End Try

            Case "POLIZAS"
                'GENERANDO ENCABEZADO XML
                Dim ll_razon As String = ""
                Dim ll_agencia As String = ""

                If VProcesos.m_razon > 0 Then
                    ll_razon = VProcesos.m_razon.ToString
                End If

                If VProcesos.m_agencia > 0 Then
                    ll_agencia = VProcesos.m_agencia.ToString
                End If

                Try

                    'Atributo requerido para la expresión de la versión
                    'del(formato)
                    '10%
                    'Thread.Sleep(2000)
                    process.SetStatus(10)
                    process.Name = "GENERANDO ENCABEZADO XML"

                    pol.Version = "1.1"

                    cadenaorginal = "||1.1"

                    'Atributo requerido para expresar el RFC del
                    'contribuyente que envía los datos

                    'Atributo opcional para expresar el número de serie del 
                    'certificado de sello digital que ampara el archivo de contabilidad electrónica, 
                    'de acuerdo al acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.

                    SQLTable = Persistencia.GetDataTable("select upper(rtrim(RFC)) as RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param where idRazonsocial = " + VProcesos.m_razon.ToString)
                    If SQLTable.Rows.Count > 0 Then
                        pol.RFC = SQLTable.Rows(0).Item("RFC")
                        pol.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        pol.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")
                    Else
                        pol.RFC = "XAXX010101000"
                        pol.noCertificado = "00001000000300527322"
                    End If

                    cadenaorginal += "|" + pol.RFC.Trim


                    'Atributo requerido para expresar el mes al que 
                    'corresponden las polizas a reportar

                    pol.Mes = VProcesos.m_pe.ToString().PadLeft(2, "0")
                    cadenaorginal += "|" + VProcesos.m_pe.ToString().PadLeft(2, "0")

                    'Atributo requerido para expresar el año al que 
                    'corresponden las polizas a reportar

                    pol.Anio = VProcesos.m_ej
                    cadenaorginal += "|" + Now.Year.ToString()


                    pol.TipoSolicitud = VProcesos.m_TipoSolicitud
                    cadenaorginal += "|" + pol.TipoSolicitud
                    Select Case pol.TipoSolicitud
                        Case "AF", "FC"
                            pol.NumOrden = VProcesos.m_NumOrden
                            cadenaorginal += "|" + pol.NumOrden
                        Case "DE", "CO"
                            pol.NumTramite = VProcesos.m_NumTramite
                            cadenaorginal += "|" + pol.NumTramite
                    End Select

                    SQLPoliza = Persistencia.GetDataTable(" select idHPol, idRazon, idDistribuidor, idEjercicio, idPeriodo, CASE WHEN IDPOLIZA = 1 THEN 3 WHEN IDPOLIZA = 2 THEN 1 WHEN IDPOLIZA = 3 THEN 2 END TIPO, " +
                                                          " RTRIM(SFOLIO) AS NUM, CONVERT(VARCHAR(10),FECHA,103) AS FECHA, CONCEPTO " +
                                                          " from tbco_movHistoricoC where idejercicio = " + VProcesos.m_ej.ToString + " and idperiodo = " + VProcesos.m_pe.ToString + " and idRazon = " + VProcesos.m_razon.ToString)
                    Dim lstpol As New List(Of PolizasPoliza)


                    If SQLPoliza.Rows.Count > 0 Then

                        Dim llcount = 1
                        Dim rpol = SQLPoliza.Rows.Count

                        For Each pl In SQLPoliza.AsEnumerable
                            'INCREMENTANDO % PROGRESO DE POLIZAS
                            process.SetStatus(CInt((llcount * 80) / rpol))
                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )"

                            Dim poliza As PolizasPoliza = New PolizasPoliza()
                            'poliza. = pl.Item("TIPO")
                            poliza.NumUnIdenPol = pl.Item("NUM")
                            poliza.Fecha = CDate(pl.Item("FECHA")).ToString("yyyy-MM-dd")
                            poliza.Concepto = pl.Item("CONCEPTO")

                            cadenaorginal += "|" + poliza.NumUnIdenPol.Trim + "|" + poliza.Fecha + "|" + poliza.Concepto.Trim


                            'AGREGANDO TRANSACCIONES
                            SQLTrs = Persistencia.GetDataTable(" select idHPol, c.clavelocal as NumCta, c.nombre as DesCta, s.concepto, s.importeD as Cargo, " +
                                                                " s.importeH as Abono, s.registro, 'MXN' as Moneda, 0.00 as TipoCamb " +
                                                                " from tbco_movSaldosC s inner join " +
                                                                " tbco_abccuenta c on s.idcta = c.idcta And s.idRazon = c.idRazon " +
                                                                " where idHPol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                                " And idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                                " And idPeriodo =  " + pl.Item("idPeriodo").ToString)


                            Dim lsttrs As New List(Of PolizasPolizaTransaccion)
                            If SQLTrs.Rows.Count > 0 Then

                                Dim lldet = 1
                                Dim rdet = SQLTrs.Rows.Count

                                For Each tr In SQLTrs.AsEnumerable
                                    'INCREMENTANDO % PROGRESO DE TRANSACCIONES
                                    process.SetStatus(CInt((lldet * 99) / rdet))
                                    process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO TRANSACCIONES ( " + lldet.ToString + " / " + rdet.ToString + " )"

                                    Dim trans As PolizasPolizaTransaccion = New PolizasPolizaTransaccion()

                                    trans.NumCta = tr.Item("NumCta")
                                    trans.DesCta = tr.Item("DesCta")
                                    trans.Concepto = tr.Item("concepto")
                                    trans.Debe = tr.Item("Cargo")
                                    trans.Haber = tr.Item("Abono")

                                    cadenaorginal += "|" + trans.NumCta.Trim + "|" + trans.Concepto.Trim + "|" + trans.Debe.ToString + "|" + trans.Haber.ToString

                                    'AGREGAR COMPROBANTES NACIONALES CFDI
                                    SQLComprob = Persistencia.GetDataTable(" select isnull(UUID,'') as UUID, isnull(Subtotal,0) as Monto, isnull(RFCEmisor,'') as RFC " +
                                                           " from tbco_comprobantec c inner join tbco_movSaldosC s on s.idComprobante = c.idComprobante" +
                                                           " And s.idEjercicio = c.idEjercicio And s.idPeriodo = c.idPeriodo And s.idRazon = c.idRazon And s.idDistribuidor = c.idDistribuidor" +
                                                           " where c.TipoComprobante = 'CFDI' And s.idhpol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                           " And s.idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And s.idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                           " And s.idPeriodo =  " + pl.Item("idPeriodo").ToString + " And s.Registro = " + tr.Item("registro").ToString)

                                    Dim lstnaCFDI As New List(Of PolizasPolizaTransaccionCompNal)
                                    If SQLComprob.Rows.Count > 0 Then

                                        Dim llCFDI = 1
                                        Dim rCFDI = SQLComprob.Rows.Count

                                        For Each com In SQLComprob.AsEnumerable
                                            'INCREMENTANDO % PROGRESO DE CFDI
                                            process.SetStatus(CInt((llCFDI * 99) / rCFDI))
                                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CFDI ( " + llCFDI.ToString + " / " + rCFDI.ToString + " )"

                                            Dim comprobante As PolizasPolizaTransaccionCompNal = New PolizasPolizaTransaccionCompNal()

                                            comprobante.UUID_CFDI = com.Item("UUID")
                                            comprobante.MontoTotal = com.Item("Monto")
                                            comprobante.RFC = com.Item("RFC")

                                            cadenaorginal += "|" + comprobante.UUID_CFDI.Trim '+ "|" + comprobante.MontoTotal.ToString + "|" + comprobante.RFC.Trim

                                            lstnaCFDI.Add(comprobante)
                                            llCFDI += 1

                                        Next
                                        'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                        trans.CompNal = lstnaCFDI.ToArray

                                    End If


                                    'AGREGAR COMPROBANTES NACIONALES CFD
                                    SQLComprob = Persistencia.GetDataTable(" select isnull(Folio,0) as Folio, isnull(Serie,'') as Serie, isnull(Subtotal,0) as Monto, isnull(RFCEmisor,'') as RFC " +
                                                                           " from tbco_comprobantec c inner join tbco_movSaldosC s on s.idComprobante = c.idComprobante" +
                                                                           " And s.idEjercicio = c.idEjercicio And s.idPeriodo = c.idPeriodo And s.idRazon = c.idRazon And s.idDistribuidor = c.idDistribuidor " +
                                                                           " where c.TipoComprobante = 'CFDI' And s.idhpol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                                           " And s.idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And s.idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                                           " And s.idPeriodo =  " + pl.Item("idPeriodo").ToString + " And s.Registro = " + tr.Item("registro").ToString)

                                    Dim lstnaCFD As New List(Of PolizasPolizaTransaccionCompNalOtr)

                                    If SQLComprob.Rows.Count > 0 Then

                                        Dim llCFD = 1
                                        Dim rCFD = SQLComprob.Rows.Count

                                        For Each com In SQLComprob.AsEnumerable

                                            'INCREMENTANDO % PROGRESO DE CFD
                                            process.SetStatus(CInt((llCFD * 99) / rCFD))
                                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CFD ( " + llCFD.ToString + " / " + rCFD.ToString + " )"


                                            Dim comprobante As PolizasPolizaTransaccionCompNalOtr = New PolizasPolizaTransaccionCompNalOtr()

                                            comprobante.CFD_CBB_NumFol = com.Item("Folio")
                                            comprobante.CFD_CBB_Serie = com.Item("Serie")
                                            comprobante.MontoTotal = com.Item("Monto")
                                            comprobante.RFC = com.Item("RFC")

                                            cadenaorginal += "|" + comprobante.CFD_CBB_Serie.Trim + "|" + comprobante.CFD_CBB_NumFol.ToString ' + "|" + comprobante.MontoTotal.ToString + "|" + comprobante.RFC.Trim

                                            lstnaCFD.Add(comprobante)
                                            llCFD += 1
                                        Next
                                        'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                        trans.CompNalOtr = lstnaCFD.ToArray

                                    End If



                                    'AGREGAR COMPROBANTE IMPORTANDOS
                                    SQLComprob = Persistencia.GetDataTable("select NumFactExt, Moneda, isnull(Subtotal,0) as Monto " +
                                                                 " from tbco_comprobantec c inner join tbco_movSaldosC s on s.idComprobante = c.idComprobante" +
                                                                 " And s.idEjercicio = c.idEjercicio And s.idPeriodo = c.idPeriodo And s.idRazon = c.idRazon And s.idDistribuidor = c.idDistribuidor " +
                                                                 " where TipoComprobante  = 'EXT' And s.idhpol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                                 " And s.idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And s.idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                                 " And s.idPeriodo =  " + pl.Item("idPeriodo").ToString + " And s.Registro = " + tr.Item("registro").ToString)

                                    Dim lstex As New List(Of PolizasPolizaTransaccionCompExt)
                                    If SQLComprob.Rows.Count > 0 Then

                                        Dim llex = 1
                                        Dim rex = SQLComprob.Rows.Count

                                        For Each com In SQLComprob.AsEnumerable

                                            'INCREMENTANDO % PROGRESO DE COMPROBANTE EXTRANEJEROS
                                            process.SetStatus(CInt((llex * 99) / rex))
                                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO COMPROBANTE EXTRANJERO ( " + llex.ToString + " / " + rex.ToString + " )"

                                            Dim comprobante As PolizasPolizaTransaccionCompExt = New PolizasPolizaTransaccionCompExt()

                                            comprobante.NumFactExt = com.Item("NumFactExt")
                                            comprobante.MontoTotal = com.Item("Monto")
                                            comprobante.Moneda = com.Item("Modena")


                                            cadenaorginal += "|" + comprobante.NumFactExt.Trim '+ "|" + comprobante.MontoTotal.ToString + "|" + comprobante.Moneda.Trim
                                            lstex.Add(comprobante)
                                            llex += 1
                                        Next
                                        'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                        trans.CompExt = lstex.ToArray

                                    End If



                                    'AGREGANDO CHEQUES
                                    SQLCheques = Persistencia.GetDataTable(" select isnull(Num,'') as Num, isnull(Banco,'') as Banco, isnull(CtaOri,'') as CtaOri, isnull(Fecha,'') as Fecha, Monto, AFavor as Benef, RFC " +
                                                                            " from tbco_hchequera " +
                                                                            " where idhpol = " + pl.Item("idhpol").ToString + " And idEjercicio = " + VProcesos.m_ej.ToString + " And idPeriodo = " + VProcesos.m_pe.ToString +
                                                                            " And idRazon = " + pl.Item("idRazon").ToString + " And idDistribuidor = " + pl.Item("idDistribuidor").ToString)

                                    Dim lstchq As New List(Of PolizasPolizaTransaccionCheque)
                                    If SQLCheques.Rows.Count > 0 Then

                                        Dim llch = 1
                                        Dim rch = SQLCheques.Rows.Count

                                        For Each chq In SQLCheques.AsEnumerable

                                            'INCREMENTANDO % PROGRESO DE COMPROBANTE EXTRANEJEROS
                                            process.SetStatus(CInt((llch * 99) / rch))
                                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CHEQUE ( " + llch.ToString + " / " + rch.ToString + " )"


                                            If chq.Item("Num") <> "" Then
                                                Dim cheque As PolizasPolizaTransaccionCheque = New PolizasPolizaTransaccionCheque()

                                                cheque.Num = chq.Item("Num")
                                                cheque.BanEmisNal = chq.Item("Banco")
                                                cheque.CtaOri = chq.Item("CtaOri")
                                                cheque.Fecha = chq.Item("Fecha")
                                                cheque.Benef = chq.Item("Benef")
                                                cheque.RFC = chq.Item("RFC")
                                                cheque.Monto = chq.Item("Monto")

                                                cadenaorginal += "|" + cheque.Num.Trim + "|" + cheque.BanEmisNal.Trim + "|" + cheque.BanEmisExt.Trim + "|" + cheque.CtaOri.Trim + "|" + cheque.Fecha.ToString + "|" + cheque.Benef.Trim + "|" + cheque.RFC.Trim + "|" + cheque.Monto.ToString + "|" + cheque.Moneda.ToString + "|" + cheque.TipCamb.ToString


                                                lstchq.Add(cheque)
                                            End If


                                            llch += 1
                                        Next
                                        'SE AGREGA ARRELO DE CHEQUES
                                        trans.Cheque = lstchq.ToArray

                                    End If

                                    'AGREGANDO TRANSFERENCIAS
                                    SQLTranfer = Persistencia.GetDataTable(" select isnull(CtaOri,'') as CtaOri , isnull(BancoOri,'') as BancoOri, Monto, isnull(CtaDest,'') as CtaDest, isnull(BancoDest,'') as BancoDest , isnull(Fecha,'') as Fecha, AFavor as Benef, RFC  " +
                                                                            " from tbco_htransferencia" +
                                                                            " where idhpol = " + tr.Item("idhpol").ToString + " And idEjercicio = " + VProcesos.m_ej.ToString + " And idPeriodo = " + VProcesos.m_pe.ToString +
                                                                            " And idRazon = " + pl.Item("idRazon").ToString + " And idDistribuidor = " + pl.Item("idDistribuidor").ToString)

                                    Dim lsttra As New List(Of PolizasPolizaTransaccionTransferencia)
                                    If SQLTranfer.Rows.Count > 0 Then
                                        Dim lltr = 1
                                        Dim rtr = SQLTranfer.Rows.Count
                                        For Each trf In SQLTranfer.AsEnumerable
                                            'INCREMENTANDO % PROGRESO DE COMPROBANTE EXTRANEJEROS
                                            process.SetStatus(CInt((lltr * 99) / rtr))
                                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CHEQUE ( " + lltr.ToString + " / " + rtr.ToString + " )"

                                            If trf.Item("CtaOri") <> "" Then
                                                Dim transfer As PolizasPolizaTransaccionTransferencia = New PolizasPolizaTransaccionTransferencia()

                                                transfer.CtaOri = trf.Item("CtaOri")
                                                transfer.BancoOriNal = trf.Item("BancoOri")
                                                transfer.CtaDest = trf.Item("CtaDest")
                                                transfer.BancoDestNal = trf.Item("BancoDest")
                                                transfer.Fecha = trf.Item("Fecha")
                                                transfer.Benef = trf.Item("Benef")
                                                transfer.RFC = trf.Item("RFC")
                                                transfer.Monto = trf.Item("Monto")

                                                cadenaorginal += "|" + transfer.CtaOri.Trim + "|" + transfer.BancoOriNal.Trim + "|" + transfer.BancoOriExt.Trim + "|" + transfer.CtaDest.Trim + "|" + transfer.BancoDestNal.Trim + "|" + transfer.BancoDestExt.Trim + "|" + transfer.Fecha.ToString + "|" + transfer.Benef.Trim + "|" + transfer.RFC.Trim + "|" + transfer.Monto.ToString + "|" + transfer.Moneda.ToString + "|" + transfer.TipCamb.ToString

                                                lsttra.Add(transfer)
                                            End If



                                            lltr += 1
                                        Next
                                        'SE AGREGA ARRELO DE TRANSFERENCIAS
                                        trans.Transferencia = lsttra.ToArray

                                    End If



                                    lldet += 1
                                    lsttrs.Add(trans)
                                Next

                                'SE AGREGA ARRELO DE TRANSACCIONES
                                poliza.Transaccion = lsttrs.ToArray
                            End If

                            llcount += 1
                            lstpol.Add(poliza)
                        Next
                        'SE AGREGA ARREGLO DE PÓLIZAS
                        pol.Poliza = lstpol.ToArray
                    End If

                    cadenaorginal += "||"

                    Dim StringWriter As New System.IO.StringWriter
                    Dim XsltTransformation As New XslCompiledTransform(True)
                    Dim XsltArgumentList As New Xsl.XsltArgumentList


                    Dim request_serializer As XmlSerializer = Nothing
                    Dim request_writer As StreamWriter = Nothing

                    '85%
                    process.SetStatus(85)
                    process.Name = "GENERANDO ARCHIVO XML PARA CADENA"
                    request_serializer = New XmlSerializer(GetType(Polizas))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + pol.RFC + pol.Anio.ToString + CInt(pol.Mes).ToString.PadLeft(2, "0") + "PL.xml")
                    request_serializer.Serialize(request_writer, pol)
                    request_writer.Close()

                    '90%
                    process.SetStatus(90)
                    process.Name = "GENERANDO CADENA ORIGINAL"
                    Dim StylesheetPath As String = VProcesos.m_pathXSLT + "PolizasPeriodo_1_1.xslt"
                    Dim SitemapPath As String = VProcesos.m_pathXML + pol.RFC + pol.Anio.ToString + CInt(pol.Mes).ToString.PadLeft(2, "0") + "PL.xml"


                    Try
                        XsltTransformation.Load(StylesheetPath)
                        XsltTransformation.Transform(SitemapPath, XsltArgumentList, StringWriter)
                    Catch ex As Xsl.XsltException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    End Try
                    cadenaorginal = StringWriter.ToString()

                    'GENERANDO SELLO DE CATÁLOGO 
                    process.SetStatus(95)
                    process.Name = "GENERANDO SELLO DE POLIZAS"

                    'DEFINE FECHA PARA GENERAR SELLO
                    'ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw) 'ADOCFD.GenerarSello(cadenaorginal) 
                    pol.Sello = sello


                    '99%
                    process.SetStatus(99)
                    process.Name = "GENERANDO ARCHIVO XML CON SELLO"

                    request_serializer = New XmlSerializer(GetType(Polizas))


                    request_writer = New StreamWriter(VProcesos.m_pathXML + pol.RFC + pol.Anio.ToString + CInt(pol.Mes).ToString.PadLeft(2, "0") + "PL.xml")
                    request_serializer.Serialize(request_writer, pol)
                    request_writer.Close()


                    '100%
                    process.SetStatus(100)
                    process.Name = "COMPRIMIENDO ARCHIVO A ZIP"
                    Using zip As ZipFile = New ZipFile
                        zip.AddFile(VProcesos.m_pathXML + pol.RFC + pol.Anio.ToString + CInt(pol.Mes).ToString.PadLeft(2, "0") + "PL.xml", "")
                        zip.Save(VProcesos.m_pathXML + pol.RFC + pol.Anio.ToString + CInt(pol.Mes).ToString.PadLeft(2, "0") + "PL.zip")

                    End Using
                    File.Delete(VProcesos.m_pathXML + pol.RFC + pol.Anio.ToString + CInt(pol.Mes).ToString.PadLeft(2, "0") + "PL.xml")
                Catch ex As Exception
                    process.SetStatus(100)
                    process.Name = "ERROR AL GENERARL EL ARCHIVO </br>" + ex.Message
                End Try
            Case "AUXILIARFOLIOS"
                'GENERANDO ENCABEZADO XML
                Try

                    'Atributo requerido para la expresión de la versión
                    'del(formato)
                    '10%
                    'Thread.Sleep(2000)
                    process.SetStatus(10)
                    process.Name = "GENERANDO ENCABEZADO XML"

                    auxfolios.Version = "1.2"

                    cadenaorginal = "||1.2"

                    'Atributo requerido para expresar el RFC del
                    'contribuyente que envía los datos

                    'Atributo opcional para expresar el número de serie del 
                    'certificado de sello digital que ampara el archivo de contabilidad electrónica, 
                    'de acuerdo al acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.

                    SQLTable = Persistencia.GetDataTable("select upper(rtrim(RFC)) as RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param where idRazonsocial = " + VProcesos.m_razon.ToString)
                    If SQLTable.Rows.Count > 0 Then
                        auxfolios.RFC = SQLTable.Rows(0).Item("RFC")
                        auxfolios.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        auxfolios.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")
                        'ADOCFD.archivoprivk = key
                        'ADOCFD.passwordprivk = psw
                    Else
                        auxfolios.RFC = "XAXX010101000"
                        auxfolios.noCertificado = "00001000000300527322"
                    End If

                    cadenaorginal += "|" + auxfolios.RFC.Trim


                    'Atributo requerido para expresar el mes al que 
                    'corresponden las polizas a reportar
                    auxfolios.Mes = VProcesos.m_pe.ToString().PadLeft(2, "0")
                    cadenaorginal += "|" + VProcesos.m_pe.ToString().PadLeft(2, "0")

                    'Atributo requerido para expresar el año al que 
                    'corresponden las polizas a reportar
                    auxfolios.Anio = VProcesos.m_ej
                    cadenaorginal += "|" + Now.Year.ToString()


                    auxfolios.TipoSolicitud = VProcesos.m_TipoSolicitud
                    cadenaorginal += "|" + auxfolios.TipoSolicitud
                    Select Case auxfolios.TipoSolicitud
                        Case "AF", "FC"
                            auxfolios.NumOrden = VProcesos.m_NumOrden
                            cadenaorginal += "|" + auxfolios.NumOrden
                        Case "DE", "CO"
                            auxfolios.NumTramite = VProcesos.m_NumTramite
                            cadenaorginal += "|" + auxfolios.NumTramite
                    End Select

                    SQLPoliza = Persistencia.GetDataTable(" select h.idHPol, h.idRazon, h.idDistribuidor, h.idEjercicio, h.idPeriodo, CASE WHEN h.IDPOLIZA = 1 THEN 3 WHEN h.IDPOLIZA = 2 THEN 1 WHEN h.IDPOLIZA = 3 THEN 2 END TIPO, " +
                                                          " RTRIM(h.SFOLIO) AS NUM, CONVERT(VARCHAR(10),h.FECHA,103) AS FECHA, h.CONCEPTO " +
                                                          " from tbco_movHistoricoC h " +
                                                          " inner join tbco_movSaldosC s on h.idHPol = s.idHPol And h.idRazon = s.idRazon " +
                                                          " And h.idDistribuidor = s.idDistribuidor And s.idPeriodo = h.idPeriodo And s.idEjercicio = s.idEjercicio " +
                                                          " inner join tbco_comprobantec c on c.idComprobante = s.idComprobante And c.idEjercicio = s.idEjercicio " +
                                                          " And c.idPeriodo = s.idPeriodo And c.idRazon = s.idRazon And c.idDistribuidor = s.idDistribuidor " +
                                                          " where h.idejercicio = " + VProcesos.m_ej.ToString + " and h.idperiodo = " + VProcesos.m_pe.ToString + " and h.idRazon = " + VProcesos.m_razon.ToString)

                    Dim lstpol As New List(Of RepAuxFolDetAuxFol)


                    If SQLPoliza.Rows.Count > 0 Then

                        Dim llcount = 1
                        Dim rpol = SQLPoliza.Rows.Count

                        For Each pl In SQLPoliza.AsEnumerable
                            'INCREMENTANDO % PROGRESO DE POLIZAS
                            process.SetStatus(CInt((llcount * 80) / rpol))
                            process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )"

                            Dim poliza As RepAuxFolDetAuxFol = New RepAuxFolDetAuxFol()
                            'poliza. = pl.Item("TIPO")
                            poliza.NumUnIdenPol = pl.Item("NUM")
                            poliza.Fecha = CDate(pl.Item("FECHA")).ToString("yyyy-MM-dd")

                            cadenaorginal += "|" + poliza.NumUnIdenPol.Trim + "|" + poliza.Fecha


                            'AGREGAR COMPROBANTES NACIONALES CFDI
                            SQLComprob = Persistencia.GetDataTable(" select isnull(UUID,'') as UUID, isnull(Subtotal,0) as Monto, isnull(RFCEmisor,'') as RFC " +
                                                   " from tbco_comprobantec c inner join tbco_movSaldosC s on s.idComprobante = c.idComprobante" +
                                                   " And s.idEjercicio = c.idEjercicio And s.idPeriodo = c.idPeriodo And s.idRazon = c.idRazon " +
                                                   " where c.TipoComprobante = 'CFDI' And s.idhpol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                   " And s.idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And s.idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                   " And s.idPeriodo =  " + pl.Item("idPeriodo").ToString)

                            Dim lstnaCFDI As New List(Of RepAuxFolDetAuxFolComprNal)
                            If SQLComprob.Rows.Count > 0 Then

                                Dim llCFDI = 1
                                Dim rCFDI = SQLComprob.Rows.Count

                                For Each com In SQLComprob.AsEnumerable
                                    'INCREMENTANDO % PROGRESO DE CFDI
                                    process.SetStatus(CInt((llCFDI * 99) / rCFDI))
                                    process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CFDI ( " + llCFDI.ToString + " / " + rCFDI.ToString + " )"

                                    Dim comprobante As RepAuxFolDetAuxFolComprNal = New RepAuxFolDetAuxFolComprNal()

                                    comprobante.UUID_CFDI = com.Item("UUID")
                                    comprobante.MontoTotal = com.Item("Monto")
                                    comprobante.RFC = com.Item("RFC")

                                    cadenaorginal += "|" + comprobante.UUID_CFDI.Trim '+ "|" + comprobante.MontoTotal.ToString + "|" + comprobante.RFC.Trim

                                    lstnaCFDI.Add(comprobante)
                                    llCFDI += 1

                                Next
                                'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                poliza.ComprNal = lstnaCFDI.ToArray

                            End If


                            'AGREGAR COMPROBANTES NACIONALES CFD
                            SQLComprob = Persistencia.GetDataTable(" select isnull(Folio,0) as Folio, isnull(Serie,'') as Serie, isnull(Subtotal,0) as Monto, isnull(RFCEmisor,'') as RFC " +
                                                                   " from tbco_comprobantec c inner join tbco_movSaldosC s on s.idComprobante = c.idComprobante" +
                                                                   " And s.idEjercicio = c.idEjercicio And s.idPeriodo = c.idPeriodo And s.idRazon = c.idRazon " +
                                                                   " where c.TipoComprobante = 'CFDI' And s.idhpol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                                   " And s.idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And s.idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                                   " And s.idPeriodo =  " + pl.Item("idPeriodo").ToString)

                            Dim lstnaCFD As New List(Of RepAuxFolDetAuxFolComprNalOtr)

                            If SQLComprob.Rows.Count > 0 Then

                                Dim llCFD = 1
                                Dim rCFD = SQLComprob.Rows.Count

                                For Each com In SQLComprob.AsEnumerable

                                    'INCREMENTANDO % PROGRESO DE CFD
                                    process.SetStatus(CInt((llCFD * 99) / rCFD))
                                    process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CFD ( " + llCFD.ToString + " / " + rCFD.ToString + " )"


                                    Dim comprobante As RepAuxFolDetAuxFolComprNalOtr = New RepAuxFolDetAuxFolComprNalOtr()

                                    comprobante.CFD_CBB_NumFol = com.Item("Folio")
                                    comprobante.CFD_CBB_Serie = com.Item("Serie")
                                    comprobante.MontoTotal = com.Item("Monto")
                                    comprobante.RFC = com.Item("RFC")

                                    cadenaorginal += "|" + comprobante.CFD_CBB_Serie.Trim + "|" + comprobante.CFD_CBB_NumFol.ToString ' + "|" + comprobante.MontoTotal.ToString + "|" + comprobante.RFC.Trim

                                    lstnaCFD.Add(comprobante)
                                    llCFD += 1
                                Next
                                'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                poliza.ComprNalOtr = lstnaCFD.ToArray

                            End If



                            'AGREGAR COMPROBANTE IMPORTANDOS
                            SQLComprob = Persistencia.GetDataTable("select NumFactExt, Moneda, isnull(Subtotal,0) as Monto " +
                                                         " from tbco_comprobantec c inner join tbco_movSaldosC s on s.idComprobante = c.idComprobante" +
                                                         " And s.idEjercicio = c.idEjercicio And s.idPeriodo = c.idPeriodo And s.idRazon = c.idRazon " +
                                                         " where TipoComprobante  = 'EXT' And s.idhpol = " + pl.Item("idHPol").ToString + " and s.idRazon = " + pl.Item("idRazon").ToString +
                                                         " And s.idDistribuidor = " + pl.Item("idDistribuidor").ToString + " And s.idEjercicio = " + pl.Item("idEjercicio").ToString +
                                                         " And s.idPeriodo =  " + pl.Item("idPeriodo").ToString)

                            Dim lstex As New List(Of RepAuxFolDetAuxFolComprExt)
                            If SQLComprob.Rows.Count > 0 Then

                                Dim llex = 1
                                Dim rex = SQLComprob.Rows.Count

                                For Each com In SQLComprob.AsEnumerable

                                    'INCREMENTANDO % PROGRESO DE COMPROBANTE EXTRANEJEROS
                                    process.SetStatus(CInt((llex * 99) / rex))
                                    process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO COMPROBANTE EXTRANJERO ( " + llex.ToString + " / " + rex.ToString + " )"

                                    Dim comprobante As RepAuxFolDetAuxFolComprExt = New RepAuxFolDetAuxFolComprExt()

                                    comprobante.NumFactExt = com.Item("NumFactExt")
                                    comprobante.MontoTotal = com.Item("Monto")
                                    comprobante.Moneda = com.Item("Modena")


                                    cadenaorginal += "|" + comprobante.NumFactExt.Trim '+ "|" + comprobante.MontoTotal.ToString + "|" + comprobante.Moneda.Trim
                                    lstex.Add(comprobante)
                                    llex += 1
                                Next
                                'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                poliza.ComprExt = lstex.ToArray

                            End If

                            llcount += 1
                            lstpol.Add(poliza)
                        Next
                        'SE AGREGA ARREGLO DE PÓLIZAS
                        auxfolios.DetAuxFol = lstpol.ToArray
                    End If

                    cadenaorginal += "||"



                    Dim StringWriter As New System.IO.StringWriter
                    Dim XsltTransformation As New XslCompiledTransform(True)
                    Dim XsltArgumentList As New XsltArgumentList

                    Dim request_serializer As XmlSerializer = Nothing
                    Dim request_writer As StreamWriter = Nothing

                    '85%
                    process.SetStatus(85)
                    process.Name = "GENERANDO ARCHIVO XML PARA CADENA"
                    request_serializer = New XmlSerializer(GetType(RepAuxFol))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + auxfolios.RFC + auxfolios.Anio.ToString + CInt(auxfolios.Mes).ToString.PadLeft(2, "0") + "XF.xml")
                    request_serializer.Serialize(request_writer, auxfolios)
                    request_writer.Close()

                    '90%
                    process.SetStatus(90)
                    process.Name = "GENERANDO CADENA ORIGINAL"
                    Dim StylesheetPath As String = VProcesos.m_pathXSLT + "AuxiliarFolios_1_1.xslt"
                    Dim SitemapPath As String = VProcesos.m_pathXML + auxfolios.RFC + auxfolios.Anio.ToString + CInt(auxfolios.Mes).ToString.PadLeft(2, "0") + "XF.xml"


                    Try
                        XsltTransformation.Load(StylesheetPath)
                        XsltTransformation.Transform(SitemapPath, XsltArgumentList, StringWriter)
                    Catch ex As Xsl.XsltException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    End Try
                    cadenaorginal = StringWriter.ToString()

                    'GENERANDO SELLO DE CATÁLOGO 
                    process.SetStatus(95)
                    process.Name = "GENERANDO SELLO DE FOLIOS"

                    'DEFINE FECHA PARA GENERAR SELLO
                    'ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw) 'ADOCFD.GenerarSello(cadenaorginal) 
                    auxfolios.Sello = sello


                    '99%
                    process.SetStatus(99)
                    process.Name = "GENERANDO ARCHIVO XML CON SELLO"

                    request_serializer = New XmlSerializer(GetType(RepAuxFol))


                    request_writer = New StreamWriter(VProcesos.m_pathXML + auxfolios.RFC + auxfolios.Anio.ToString + CInt(auxfolios.Mes).ToString.PadLeft(2, "0") + "XF.xml")
                    request_serializer.Serialize(request_writer, auxfolios)
                    request_writer.Close()


                    '100%
                    process.SetStatus(100)
                    process.Name = "COMPRIMIENDO ARCHIVO A ZIP"
                    Using zip As ZipFile = New ZipFile
                        zip.AddFile(VProcesos.m_pathXML + auxfolios.RFC + auxfolios.Anio.ToString + CInt(auxfolios.Mes).ToString.PadLeft(2, "0") + "XF.xml", "")
                        zip.Save(VProcesos.m_pathXML + auxfolios.RFC + auxfolios.Anio.ToString + CInt(auxfolios.Mes).ToString.PadLeft(2, "0") + "XF.zip")

                    End Using
                    File.Delete(VProcesos.m_pathXML + auxfolios.RFC + auxfolios.Anio.ToString + CInt(auxfolios.Mes).ToString.PadLeft(2, "0") + "XF.xml")

                Catch ex As Exception
                    process.SetStatus(100)
                    process.Name = "ERROR AL GENERARL EL ARCHIVO </br>" + ex.Message
                End Try

            Case "AUXILIARCUENTAS"
                'GENERANDO ENCABEZADO XML
                Try

                    'Atributo requerido para la expresión de la versión
                    'del(formato)
                    '10%
                    'Thread.Sleep(2000)
                    process.SetStatus(10)
                    process.Name = "GENERANDO ENCABEZADO XML"

                    auxcta.Version = "1.1"

                    cadenaorginal = "||1.1"

                    'Atributo requerido para expresar el RFC del
                    'contribuyente que envía los datos

                    'Atributo opcional para expresar el número de serie del 
                    'certificado de sello digital que ampara el archivo de contabilidad electrónica, 
                    'de acuerdo al acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.

                    SQLTable = Persistencia.GetDataTable("select upper(rtrim(RFC)) as RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param where idRazonsocial = " + VProcesos.m_razon.ToString)
                    If SQLTable.Rows.Count > 0 Then
                        auxcta.RFC = SQLTable.Rows(0).Item("RFC")
                        auxcta.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        auxcta.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")
                        'ADOCFD.archivoprivk = key
                        'ADOCFD.passwordprivk = psw
                    Else
                        auxcta.RFC = "XAXX010101000"
                        auxcta.noCertificado = "00001000000300527322"
                    End If

                    cadenaorginal += "|" + auxcta.RFC.Trim

                    'Atributo requerido para expresar el mes al que 
                    'corresponden las polizas a reportar

                    auxcta.Mes = VProcesos.m_pe.ToString().PadLeft(2, "0")
                    cadenaorginal += "|" + VProcesos.m_pe.ToString().PadLeft(2, "0")

                    'Atributo requerido para expresar el año al que 
                    'corresponden las polizas a reportar
                    auxcta.Anio = VProcesos.m_ej
                    cadenaorginal += "|" + VProcesos.m_ej.ToString


                    auxcta.TipoSolicitud = VProcesos.m_TipoSolicitud
                    cadenaorginal += "|" + auxfolios.TipoSolicitud
                    Select Case auxcta.TipoSolicitud
                        Case "AF", "FC"
                            auxcta.NumOrden = VProcesos.m_NumOrden
                            cadenaorginal += "|" + auxcta.NumOrden
                        Case "DE", "CO"
                            auxcta.NumTramite = VProcesos.m_NumTramite
                            cadenaorginal += "|" + auxcta.NumTramite
                    End Select

                    Dim cuentas = Persistencia.GetDataTable(" select c.idcta, c.clavelocal as clave, c.nombre, sum(s.saldoini) as saldoini , sum(s.saldofin) as saldofin " +
                                                    " from tbco_SaldosC s  " +
                                                    " inner join tbco_abcCuenta c on c.idCTA = s.idCta And c.idRazon = s.idRazon " +
                                                    " where s.idPeriodo = " + VProcesos.m_pe.ToString() + " And s.idEjercicio = " + VProcesos.m_ej.ToString + " And s.idRazon = " + VProcesos.m_razon.ToString + " and c.idCTA in ( " +
                                                    " select idCTA from tbco_movSaldosC where idPeriodo = " + VProcesos.m_pe.ToString() + " And idEjercicio = " + VProcesos.m_ej.ToString + " And idRazon = " + VProcesos.m_razon.ToString + ") " +
                                                    " Group By c.idcta, c.clavelocal, c.nombre" +
                                                    " Order by c.idCTA")


                    Dim lstaux As New List(Of AuxiliarCtasCuenta)

                    If cuentas.Rows.Count > 0 Then

                        Dim llcount = 1

                        Dim rpol = cuentas.Rows.Count
                        For Each pl In cuentas.AsEnumerable

                            'INCREMENTANDO % PROGRESO DE POLIZAS
                            process.SetStatus(CInt((llcount * 80) / rpol))
                            process.Name = "GENERANDO AUXILIARES ( " + llcount.ToString + " / " + rpol.ToString + " )"

                            Dim aux As AuxiliarCtasCuenta = New AuxiliarCtasCuenta()
                            'poliza. = pl.Item("TIPO")
                            aux.NumCta = pl.Item("Clave")
                            aux.DesCta = pl.Item("Nombre")
                            aux.SaldoIni = pl.Item("SaldoIni")
                            aux.SaldoFin = pl.Item("SaldoFin")


                            cadenaorginal += "|" + aux.NumCta.Trim + "|" + aux.DesCta + "|" + aux.SaldoIni.ToString + "|" + aux.SaldoFin.ToString


                            'AGREGAR CUENTAS DE AUXILIARES DE FOLIOS
                            Dim detalle = Persistencia.GetDataTable(" select convert(varchar(20),h.Fecha,103) as Fecha, ltrim(isnull(h.sfolio,'')) as Folio, h.concepto, isnull(s.ImporteD,0) as Debe, isnull(s.ImporteH,0) as Haber " +
                                                            " from tbco_movHistoricoC h " +
                                                            " inner join tbco_movSaldosC s on h.idHPol = s.idHPol And h.idRazon = s.idRazon And h.idDistribuidor = s.idDistribuidor " +
                                                            " where h.idPeriodo = " + VProcesos.m_pe.ToString() + " And s.idEjercicio = " + VProcesos.m_ej.ToString() + " And s.idRazon = " + VProcesos.m_razon.ToString() + " And s.idCta = " + pl.Item("idCta").ToString +
                                                            " order by h.Fecha, h.PFolio ")

                            Dim lstdetalle As New List(Of AuxiliarCtasCuentaDetalleAux)
                            If detalle.Rows.Count > 0 Then

                                Dim llCFDI = 1
                                Dim rCFDI = detalle.Rows.Count

                                For Each com In detalle.AsEnumerable

                                    'INCREMENTANDO % PROGRESO DE DETALLE DE AUXILIARES
                                    process.SetStatus(CInt((llCFDI * 99) / rCFDI))
                                    process.Name = "GENERANDO AUXILIARES ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO DETALLES ( " + llCFDI.ToString + " / " + rCFDI.ToString + " )"

                                    Dim auxdetalle As AuxiliarCtasCuentaDetalleAux = New AuxiliarCtasCuentaDetalleAux()
                                    auxdetalle.Fecha = CDate(com.Item("Fecha")).ToString("yyyy-MM-dd")
                                    auxdetalle.NumUnIdenPol = com.Item("Folio")
                                    auxdetalle.Concepto = com.Item("Concepto")
                                    auxdetalle.Debe = com.Item("Debe")
                                    auxdetalle.Haber = com.Item("Haber")

                                    cadenaorginal += "|" + auxdetalle.Fecha + "|" + auxdetalle.NumUnIdenPol + "|" + auxdetalle.Debe.ToString + "|" + auxdetalle.Haber.ToString

                                    lstdetalle.Add(auxdetalle)
                                    llCFDI += 1

                                Next
                                'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                                aux.DetalleAux = lstdetalle.ToArray

                            End If


                            llcount += 1
                            lstaux.Add(aux)
                        Next
                        'SE AGREGA ARREGLO DE PÓLIZAS
                        auxcta.Cuenta = lstaux.ToArray

                    End If

                    cadenaorginal += "||"

                    Dim StringWriter As New System.IO.StringWriter
                    Dim XsltTransformation As New XslCompiledTransform(True)
                    Dim XsltArgumentList As New XsltArgumentList

                    Dim request_serializer As XmlSerializer = Nothing
                    Dim request_writer As StreamWriter = Nothing

                    '85%
                    process.SetStatus(85)
                    process.Name = "GENERANDO ARCHIVO XML PARA CADENA"
                    request_serializer = New XmlSerializer(GetType(AuxiliarCtas))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + auxcta.RFC + auxcta.Anio.ToString + CInt(auxcta.Mes).ToString.PadLeft(2, "0") + "XC.xml")
                    request_serializer.Serialize(request_writer, auxcta)
                    request_writer.Close()


                    '90%
                    process.SetStatus(90)
                    process.Name = "GENERANDO CADENA ORIGINAL"
                    Dim StylesheetPath As String = VProcesos.m_pathXSLT + "AuxiliarCtas_1_1.xslt"
                    Dim SitemapPath As String = VProcesos.m_pathXML + auxcta.RFC + auxcta.Anio.ToString + CInt(auxcta.Mes).ToString.PadLeft(2, "0") + "XC.xml"

                    Try
                        XsltTransformation.Load(StylesheetPath)
                        XsltTransformation.Transform(SitemapPath, XsltArgumentList, StringWriter)
                    Catch ex As Xsl.XsltException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    End Try
                    cadenaorginal = StringWriter.ToString()

                    'GENERANDO SELLO DE CATÁLOGO 
                    process.SetStatus(95)
                    process.Name = "GENERANDO SELLO DE CUENTAS"

                    'DEFINE FECHA PARA GENERAR SELLO
                    'ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw) 'ADOCFD.GenerarSello(cadenaorginal) 
                    auxcta.Sello = sello

                    '99%
                    process.SetStatus(99)
                    process.Name = "GENERANDO ARCHIVO XML CON SELLO"
                    request_serializer = New XmlSerializer(GetType(AuxiliarCtas))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + auxcta.RFC + auxcta.Anio.ToString + CInt(auxcta.Mes).ToString.PadLeft(2, "0") + "XC.xml")
                    request_serializer.Serialize(request_writer, auxcta)
                    request_writer.Close()


                    '100%
                    process.SetStatus(100)
                    process.Name = "COMPRIMIENDO ARCHIVO A ZIP"
                    Using zip As ZipFile = New ZipFile
                        zip.AddFile(VProcesos.m_pathXML + auxcta.RFC + auxcta.Anio.ToString + CInt(auxcta.Mes).ToString.PadLeft(2, "0") + "XC.xml", "")
                        zip.Save(VProcesos.m_pathXML + auxcta.RFC + auxcta.Anio.ToString + CInt(auxcta.Mes).ToString.PadLeft(2, "0") + "XC.zip")

                    End Using
                    File.Delete(VProcesos.m_pathXML + auxcta.RFC + auxcta.Anio.ToString + CInt(auxcta.Mes).ToString.PadLeft(2, "0") + "XC.xml")


                Catch ex As Exception
                    process.SetStatus(100)
                    process.Name = "ERROR AL GENERARL EL ARCHIVO </br>" + ex.Message
                End Try

            Case "BALANZA"
                Try
                    'Atributo requerido para la expresión de la versión del formato
                    'Thread.Sleep(2000)
                    process.SetStatus(10)
                    process.Name = "GENERANDO ENCABEZADO XML"
                    blz.Version = "1.1"
                    cadenaorginal = "||1.1"

                    'Atributo requerido para expresar el RFC del contribuyente que envía los datos
                    SQLTable = Persistencia.GetDataTable("select upper(rtrim(RFC)) as RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param where idRazonsocial = " + VProcesos.m_razon.ToString)
                    If SQLTable.Rows.Count > 0 Then
                        blz.RFC = SQLTable.Rows(0).Item("RFC")
                        blz.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        blz.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")
                        'ADOCFD.archivoprivk = key
                        'ADOCFD.passwordprivk = psw
                    Else
                        blz.RFC = "XAXX010101000"
                        blz.noCertificado = "00001000000300527322"
                    End If

                    cadenaorginal += "|" + blz.RFC.Trim

                    'Atributo requerido para expresar el mes al que 
                    'corresponden la balanza
                    blz.Mes = VProcesos.m_pe.ToString.PadLeft(2, "0")
                    cadenaorginal += "|" + VProcesos.m_pe.ToString.PadLeft(2, "0")

                    'Atributo requerido para expresar el año al que 
                    'corresponden la balanza

                    blz.Anio = VProcesos.m_ej
                    cadenaorginal += "|" + VProcesos.m_ej.ToString

                    'Atributo requerido para expresar el tipo de envío de la balanza (N - Normal; C - Complementaria)
                    SQLTable = Persistencia.GetDataTable("select MAX(FECHA) AS FECHA from tbco_HistoricoCE where tiporeporte like 'BLZ' And rfc like '" + blz.RFC + "' And Periodo = " + VProcesos.m_pe.ToString + "  And Ejercicio = " + VProcesos.m_ej.ToString + " And TipoEnvio = 'N' And Folio is not null And DocumentoAcuse is not null ")
                    If SQLTable.Rows.Count > 0 And IsDate(SQLTable.Rows(0).Item("FECHA")) Then

                        blz.TipoEnvio = "C"
                        cadenaorginal += "|" + blz.TipoEnvio

                        blz.FechaModBalSpecified = True
                        blz.FechaModBal = CDate(SQLTable.Rows(0).Item("FECHA")).ToString("yyyy-MM-dd")

                        cadenaorginal += "|" + blz.FechaModBal.ToString

                        Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " +
                                        "values ('" + blz.RFC + "'," + VProcesos.m_pe.ToString + "," + VProcesos.m_ej.ToString + ",'BLZ','C','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")

                    Else



                        blz.TipoEnvio = "N"
                        cadenaorginal += "|" + blz.TipoEnvio

                        Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " +
                                        "values ('" + blz.RFC + "'," + VProcesos.m_pe.ToString + "," + VProcesos.m_ej.ToString + ",'BLZ','N','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")
                    End If




                    'AGREGANDO REGISTROS A LA BALANZA
                    SQLCuenta = Persistencia.GetDataTable("exec COspa_RptBalanzaC 'L', " + VProcesos.m_razon.ToString + ", NULL, 5," + VProcesos.m_ej.ToString + "," + VProcesos.m_pe.ToString + ",'E'")
                    Dim lstcta As New List(Of BalanzaCtas)
                    If SQLCuenta.Rows.Count > 0 Then
                        Dim llcount = 1
                        Dim rcta = SQLCuenta.Rows.Count
                        For Each cta In SQLCuenta.AsEnumerable
                            'INCREMENTANDO % PROGRESO DE CUENTAS
                            process.SetStatus(CInt((llcount * 80) / rcta))
                            process.Name = "AGREGANDO CUENTAS ( " + llcount.ToString + " / " + rcta.ToString + " )"

                            Dim blzcta As BalanzaCtas = New BalanzaCtas()

                            blzcta.NumCta = cta.Item("CLAVELOCAL")
                            blzcta.SaldoIni = cta.Item("SALDOINI")
                            blzcta.Debe = cta.Item("TOTALCARGOS")
                            blzcta.Haber = cta.Item("TOTALABONOS")
                            blzcta.SaldoFin = cta.Item("ACUMULADO")

                            cadenaorginal += "|" + blzcta.NumCta + "|" + blzcta.SaldoIni.ToString + "|" + blzcta.Debe.ToString + "|" + blzcta.Haber.ToString + "|" + blzcta.SaldoFin.ToString


                            llcount += 1
                            lstcta.Add(blzcta)
                        Next

                        blz.Ctas = lstcta.ToArray
                    End If

                    cadenaorginal += "||"

                    Dim StringWriter As New System.IO.StringWriter
                    Dim XsltTransformation As New XslCompiledTransform(True)
                    Dim XsltArgumentList As New XsltArgumentList

                    Dim request_serializer As XmlSerializer = Nothing
                    Dim request_writer As StreamWriter = Nothing

                    '85%
                    process.SetStatus(85)
                    process.Name = "GENERANDO ARCHIVO XML PARA CADENA"
                    request_serializer = New XmlSerializer(GetType(Balanza))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml")
                    request_serializer.Serialize(request_writer, blz)
                    request_writer.Close()


                    '90%
                    process.SetStatus(90)
                    process.Name = "GENERANDO CADENA ORIGINAL"
                    Dim StylesheetPath As String = VProcesos.m_pathXSLT + "BalanzaCOMPROBACION_1_1.xslt"
                    Dim SitemapPath As String = VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml"

                    Try
                        XsltTransformation.Load(StylesheetPath)
                        XsltTransformation.Transform(SitemapPath, XsltArgumentList, StringWriter)
                    Catch ex As Xsl.XsltException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    End Try
                    cadenaorginal = StringWriter.ToString()

                    'GENERANDO SELLO DE CATÁLOGO 
                    process.SetStatus(95)
                    process.Name = "GENERANDO SELLO DE CUENTAS"

                    'DEFINE FECHA PARA GENERAR SELLO
                    'ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw) 'ADOCFD.GenerarSello(cadenaorginal)
                    blz.Sello = sello

                    '99%
                    process.SetStatus(99)
                    process.Name = "GENERANDO ARCHIVO XML CON SELLO"
                    request_serializer = New XmlSerializer(GetType(Balanza))
                    request_writer = New StreamWriter(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml")
                    request_serializer.Serialize(request_writer, blz)
                    request_writer.Close()


                    '100%
                    process.SetStatus(100)
                    process.Name = "COMPRIMIENDO ARCHIVO A ZIP"
                    Using zip As ZipFile = New ZipFile
                        zip.AddFile(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml", "")
                        zip.Save(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".zip")

                    End Using
                    File.Delete(VProcesos.m_pathXML + blz.RFC + blz.Anio.ToString + CInt(blz.Mes).ToString.PadLeft(2, "0") + "B" + blz.TipoEnvio + ".xml")

                Catch ex As Exception
                    process.SetStatus(100)
                    process.Name = "ERROR AL GENERAR LA BALANZA</br>" + ex.Message
                End Try

        End Select

        Thread.Sleep(2000)
        ArrayList.Synchronized(DirectCast(DirectCast(data, Object())(1), ArrayList)).Remove(process)
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

    Public Shared Function GetFileXML(ByVal tipo As String, ByVal datos As String, ByVal comprimido As Boolean) As String
        Dim xml As String = ""
        xml += "<document type='" + tipo + "' hasData='" + comprimido.ToString.ToLower + "' >"
        xml += "<data>"
        xml += datos.Trim
        xml += "</data>"
        xml += "</document>"
        Return xml
    End Function

    Public Shared Sub CifrarArchivo(ByVal origen As String, ByVal destino As String, ByVal key As AesKey)

        Using fsIn As New FileStream(origen, FileMode.Open)
            AesUtil.EncryptFile(fsIn, destino, key)
        End Using
    End Sub

    Public Shared Function DescifrarArchivo(ByVal origen As Byte(), ByVal key As AesKey) As Byte()

        Return AesUtil.DecryptFile(New MemoryStream(origen), key)

    End Function

    Private Shared Function txt_to_data(ByVal filename As String, ByVal header As Boolean, ByVal delimiter As String) As DataTable
        'New datatable
        Dim dt As New DataTable

        'Read the contents of the textfile into an array
        Dim sr As New IO.StreamReader(filename)
        Dim txtlines() As String = sr.ReadToEnd.Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

        'Return nothing if there's nothing in the textfile
        If txtlines.Count = -1 Then
            Return Nothing
        End If

        Dim column_count As Integer = 0
        For Each col As String In txtlines(0).Split({delimiter}, StringSplitOptions.None)
            If header Then
                'If there's a header then add it by it's name
                dt.Columns.Add(col)
                dt.Columns(column_count).Caption = col
            Else
                'If there's no header then add it by the column count
                dt.Columns.Add(String.Format("Column{0}", column_count))
                dt.Columns(column_count).Caption = String.Format("Column{0}", column_count + 1)
            End If

            column_count += 1
        Next

        If header Then
            For rows As Integer = 1 To txtlines.Count - 1 'start at one because there's a header for the first line(0)
                'Declare a new datarow
                Dim dr As DataRow = dt.NewRow

                'Set the column count back to 0, we can reuse this variable ;]
                column_count = 0
                For Each col As String In txtlines(rows).Split({delimiter}, StringSplitOptions.None) 'Each column in the row
                    'The column in cue is set for the datarow
                    dr(column_count) = col
                    column_count += 1
                Next

                'Add the row
                dt.Rows.Add(dr)
            Next
        Else
            For rows As Integer = 0 To txtlines.Count - 1 'start at zero because there's no header
                'Declare a new datarow
                Dim dr As DataRow = dt.NewRow

                'Set the column count back to 0, we can reuse this variable ;]
                column_count = 0
                For Each col As String In txtlines(rows).Split({delimiter}, StringSplitOptions.None) 'Each column in the row
                    'The column in cue is set for the datarow
                    dr(column_count) = col
                    column_count += 1
                Next

                'Add the row
                dt.Rows.Add(dr)
            Next
        End If
        sr.Close()
        Return dt
    End Function


End Class
