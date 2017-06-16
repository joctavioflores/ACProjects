Imports System.IO
Imports System.Xml
Imports System.Threading
Imports Ionic.Zip
Imports System.Data.OleDb

Public Class Importar
    Inherits System.Web.UI.Page
    Public sqldata As DataTable = Nothing
    Dim myCookie As HttpCookie
    Public sUsuario As String
    Dim Script As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim contentLength As Integer = myfile.PostedFile.ContentLength
        'Dim contentType As Integer = myfile.PostedFile.ContentType
        'Dim fileName As String = myfile.PostedFile.FileName

        myCookie = HttpContext.Current.Request.Cookies("UserSettings")
        If Not IsNothing(Request.QueryString("sUsuario")) Then
            sUsuario = Request.QueryString("sUsuario")
        ElseIf Not IsNothing(myCookie) Then
            sUsuario = myCookie("Usuario")
        End If

        If Not IsPostBack Then

            Dim donde As String = ""

            If Razon.Items.Count = 0 Then


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
                Razon.Items.Insert(0, New ListItem("-- SELECT RAZON --", ""))
            End If

            If DDLEJERCICIO.Items.Count = 0 Then
                DDLEJERCICIO.DataSource = Persistencia.GetDataTable(" select * from Tbco_Ejercicios ")
                DDLEJERCICIO.DataTextField = "Anio"
                DDLEJERCICIO.DataValueField = "idEjercicio"
                DDLEJERCICIO.DataBind()
            End If

            If DDLEJERCICIO.Items.Count > 0 Then DDLEJERCICIO.SelectedValue = DDLEJERCICIO.Items.FindByText(Year(Date.Today).ToString).Value

            DDLPERIODO.DataSource = Persistencia.GetDataTable("select idperiodo, idEjercicio, Mes, Nombre, Activo, FechaIni, FechaFin,Dias from tbco_periodos where idEjercicio=" + DDLEJERCICIO.SelectedValue.ToString.ToUpper + " order by Mes")

            DDLPERIODO.DataTextField = "nombre"
            DDLPERIODO.DataValueField = "idPeriodo"
            DDLPERIODO.DataBind()

            If DDLPERIODO.Items.Count > 0 Then DDLPERIODO.SelectedValue = DDLPERIODO.Items.FindByValue(Month(Date.Today)).Value

            Select Case myCookie("idRol")
                Case "1"
                    donde = " where  idrazon = 1"
                Case "2"
                    donde = " where  idrazon = 2"
                Case "3"
                    donde = " where  idrazon = 3"
                Case Else
                    donde = " where  idrazon = 0"
            End Select

            sqldata = Persistencia.GetDataTable(" select isnull(Razon,'') as Razon from tbco_razonsocial " + donde)

            If sqldata.Rows.Count > 0 Then
                lbImp.Text = lbImp.Text + " - " + sqldata.Rows(0).Item("Razon").ToString.ToUpper
            End If




        End If

        If archivo.Value <> "" Then
            myfile.PostedFile.SaveAs(Server.MapPath("~/uploads/") + Path.GetFileName(myfile.PostedFile.FileName))
            archivo.Value = ""

            Script = "<script language=""javascript"">{ $(document).ready(function () { OpenConsolidar('" + Path.GetFileName(myfile.PostedFile.FileName) + "'); }); }</script>"
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "MiScriptSetNombreCierra", Script, False)

            'If Path.GetExtension(myfile.PostedFile.FileName) = ".xml" Then
            '    Dim xmlDoc As New XmlDocument()
            '    xmlDoc.Load("c:\texto\" + myfile.PostedFile.FileName)
            '    ThreadPool.QueueUserWorkItem(AddressOf OpenXML, xmlDoc.InnerXml)
            'End If

            'If Path.GetExtension(myfile.PostedFile.FileName) = ".xls" Or Path.GetExtension(myfile.PostedFile.FileName) = ".xlsx" Then
            '    ThreadPool.QueueUserWorkItem(AddressOf OpenXML, "c:\texto\" + myfile.PostedFile.FileName)
            'End If

        End If

        Buscar()

    End Sub

    Public Sub Buscar()

        Dim donde As String = ""

        Select Case myCookie("idRol")
            Case "1"
                donde = " and  idrazon = 1"
            Case "2"
                donde = " and  idrazon = 2"
            Case "3"
                donde = " and  idrazon = 3"
        End Select


        sqldata = Persistencia.GetDataTable("select a.idrazon ,a.iddistribuidor ,p.idEjercicio, p.idPeriodo, a.agencia as DISTRIBUIDOR, p.nombre as PERIODO, ejercicio as EJERCICIO, c.fechaini as FECHAINI, c.fechafin as FECHAFIN, CASE WHEN estado = 'P' THEN 'PROCESANDO' WHEN estado = 'G' THEN 'GENERADO' WHEN estado = 'I' THEN 'IMPORTADO' ELSE 'REPORTADO' END AS ESTADO, usuario as USUARIO " &
                                            " from TBCO_CONSOLIDA c inner join " &
                                            " tbco_agencias a on a.iddistribuidor = c.iddistribuidor  inner join " &
                                            " tbco_periodos p on p.idperiodo = c.periodo and c.ejercicio = p.idejercicio " &
                                            " where a.idrazon like '" + Razon.SelectedValue.ToString + "' and ejercicio =" + DDLEJERCICIO.SelectedValue.ToString + " and periodo = " + DDLPERIODO.SelectedValue.ToString + donde)
        consolidacion.DataSource = sqldata
        consolidacion.DataBind()

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

        If Archivo.Trim.Length > 0 Then

            If Path.GetExtension(Archivo) = ".xls" Or Path.GetExtension(Archivo) = ".xlsx" Then

                If FileIO.FileSystem.FileExists(Archivo) Then

                    Dim fileinfo As System.IO.FileInfo = FileIO.FileSystem.GetFileInfo(Archivo)

                    Dim connString As String

                    If fileinfo.Extension = ".xls" Then
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Archivo + ";Extended Properties=Excel 8.0;"
                    Else
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Archivo + ";Extended Properties=Excel 12.0 Xml;"
                    End If
                    ' Dim connString As String = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filename + ";Extended Properties=Excel 8.0;"

                    ' Create the connection object
                    Dim oledbConn As OleDbConnection = New OleDbConnection(connString)
                    Try
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

                            agencia = dt.Rows(0).Item("Distribuidor")
                            ejercicio = dt.Rows(0).Item("Ejercicio")
                            periodo = dt.Rows(0).Item("Periodo")

                            SQLProv = Persistencia.GetDataTable("select iddistribuidor from tbco_consolida where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
                            If SQLProv.Rows.Count > 0 Then
                                Persistencia.EjecutarSQL("update tbco_consolida set estado = 'P', fechaini = getdate(), fechafin = getdate() where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
                            Else
                                Persistencia.EjecutarSQL(" insert into tbco_consolida (iddistribuidor, ejercicio, periodo, usuario, estado, fechaini, fechafin) " &
                                                         " values (" + agencia.ToString + "," + ejercicio.ToString + "," + periodo.ToString + ",'" + myCookie("Usuario") + "','P', getdate(), getdate())")
                            End If

                        End If


                        For Each cuenta In dt.AsEnumerable
                            Try
                                'Inicia importación de catálogo de cuentas
                                If Not IsNothing(cuenta.Item(0).ToString.Trim) And cuenta.Item(0).ToString.Trim <> "" And Not IsDBNull(cuenta.Item(0).ToString.Trim) Then

                                    Dim dtCuenta As DataTable = Nothing
                                    dtCuenta = Persistencia.GetDataTable("select * from tbco_catalogo c inner join tbcm_EmpresaCon e on c.idEmpresa = e.idEmpresa where clave like '" + cuenta.Item(0).ToString.Trim + "' And e.Distribuidor = " + cuenta.Item(3).ToString.Trim)

                                    If dtCuenta.Rows.Count > 0 Then

                                        Persistencia.EjecutarSQL("if not exists (select idcta from tbco_saldosC where cuentanum = " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " And idEmpresa = " + dtCuenta.Rows(0).Item("idEmpresa").ToString + " And idSucursal = " + dtCuenta.Rows(0).Item("idSucursal").ToString + " And idDistribuidor = " + cuenta.Item(3).ToString.Trim + " And idPeriodo = " + cuenta.Item(5).ToString + " And idEjercicio = " + cuenta.Item(4).ToString + " )" &
                                                                 " insert into tbco_SaldosC (idCta,CuentaNum,idEmpresa,idSucursal,idDistribuidor, " &
                                                                 " idEjercicio, idPeriodo, SaldoIni, SaldoFin, FechaModif, " &
                                                                 " Cargos, Abonos,Status,Movimientos )" &
                                                                 " values (" + dtCuenta.Rows(0).Item("idcta").ToString + "," + dtCuenta.Rows(0).Item("CuentaNum").ToString + "," + dtCuenta.Rows(0).Item("idEmpresa").ToString + "," + dtCuenta.Rows(0).Item("idSucursal").ToString + "," + cuenta.Item(3).ToString.Trim &
                                                                 " ," + cuenta.Item(4).ToString + "," + cuenta.Item(5).ToString + "," + cuenta.Item(6).ToString + "," + cuenta.Item(9).ToString + ",'" + Now.Date.ToString("yyyy-MM-dd") + "'," &
                                                                 cuenta.Item(7).ToString + "," + cuenta.Item(8).ToString + ",'C'," + cuenta.Item(10).ToString + ")" &
                                                                 " else " &
                                                                 " update tbco_saldosC " &
                                                                 " set SaldoIni = " + cuenta.Item(6).ToString &
                                                                 " ,SaldoFin = " + cuenta.Item(9).ToString &
                                                                 " ,Cargos = " + cuenta.Item(7).ToString &
                                                                 " ,Abonos = " + cuenta.Item(8).ToString &
                                                                 " ,Movimientos = " + cuenta.Item(10).ToString &
                                                                 " ,FechaModif = '" + Now.Date.ToString("yyyy-MM-dd") + "'" &
                                                                 " where cuentanum = " + dtCuenta.Rows(0).Item("CuentaNum").ToString + " And idEmpresa = " + dtCuenta.Rows(0).Item("idEmpresa").ToString + " And idSucursal = " + dtCuenta.Rows(0).Item("idSucursal").ToString &
                                                                 " And idDistribuidor = " + cuenta.Item(3).ToString.Trim + " And idPeriodo = " + cuenta.Item(5).ToString + " And idEjercicio = " + cuenta.Item(4).ToString)

                                    End If


                                End If


                            Catch ex As Exception
                                mensaje = ex.Message
                            End Try
                        Next

                        Persistencia.EjecutarSQL("update tbco_consolida set estado = 'I', fechafin = getdate() where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)

                    Catch ex As Exception
                        mensaje = ex.Message

                    End Try



                End If

            End If

            If Path.GetExtension(myfile.PostedFile.FileName) = ".xml" Then


                Dim xmlDoc As New XmlDocument()
                xmlDoc.LoadXml(Archivo)

                'Deserialize XML
                Dim root As XmlNode = xmlDoc.DocumentElement
                Dim delivery As XmlNode = root.SelectSingleNode("/delivery")

                razon = delivery.Attributes("idRazon").Value
                agencia = delivery.Attributes("idDistribuidor").Value()
                ejercicio = delivery.Attributes("idEjercicio").Value()
                periodo = delivery.Attributes("idPeriodo").Value()

                SQLProv = Persistencia.GetDataTable("select iddistribuidor from tbco_consolida where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
                If SQLProv.Rows.Count > 0 Then
                    Persistencia.EjecutarSQL("update tbco_consolida set estado = 'P', fechaini = getdate(), fechafin = getdate() where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
                Else
                    Persistencia.EjecutarSQL(" insert into tbco_consolida (iddistribuidor, ejercicio, periodo, usuario, estado, fechaini, fechafin) " &
                                             " values (" + agencia.ToString + "," + ejercicio.ToString + "," + periodo.ToString + ",'" + myCookie("Usuario") + "','P', getdate(), getdate())")
                End If


                Persistencia.EjecutarSQL("delete from tbco_hchequera where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
                Persistencia.EjecutarSQL("delete from tbco_htransferencia where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
                Persistencia.EjecutarSQL("delete from tbco_comprobanteC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
                Persistencia.EjecutarSQL("delete from tbco_movHistoricoC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
                Persistencia.EjecutarSQL("delete from tbco_movSaldosC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
                Persistencia.EjecutarSQL("delete from tbco_SaldosC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)


                Dim documents As XmlNode = root.SelectSingleNode("/delivery/documents")

                Dim strAsBytes() As Byte = New System.Text.UTF8Encoding().GetBytes(Archivo)
                Dim ms As New System.IO.MemoryStream(strAsBytes)



                Dim oFileStream As System.IO.FileStream

                For Each document In documents

                    Select Case document.Attributes("type").Value()
                        Case "Encabezados"

                            If document.Attributes("hasData").Value() <> "false" Then
                                Try
                                    Dim pathEncabezado = document.InnerText()
                                    Dim BytesEncabezado As Byte() = Convert.FromBase64String(pathEncabezado)
                                    Dim zipByte = DescifrarArchivo(BytesEncabezado, key)
                                    oFileStream = New System.IO.FileStream(Context.Server.MapPath("~/uploads/Encabezado.zip"), System.IO.FileMode.Create)
                                    oFileStream.Write(zipByte, 0, zipByte.Length)
                                    oFileStream.Close()

                                    Dim ZipToUnpack As String = oFileStream.Name
                                    Dim TargetDir As String = Context.Server.MapPath("~/uploads/")
                                    'Console.WriteLine("Extracting file {0} to {1}", ZipToUnpack, TargetDir)
                                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                                        Dim e As ZipEntry
                                        ' here, we extract every entry, but we could extract    
                                        ' based on entry name, size, date, etc.   
                                        For Each e In zip1
                                            e.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                                            'Se lee y transforma el archivo en una tabla
                                            TextFile = e.FileName

                                        Next
                                    End Using
                                    File.Delete(ZipToUnpack)
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile + ".temp"))
                                    tabla = txt_to_data(Context.Server.MapPath("~/uploads/" + TextFile), False, "@|")
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile))

                                    For Each inv In tabla.AsEnumerable

                                        If Not IsDBNull(inv.Item(14)) Then

                                            Persistencia.EjecutarSQL(" insert into tbco_movHistoricoC (idRazon,idDistribuidor,idHPol, " &
                                                           " idPoliza,idUsuario,idDepto,idEmpresa,idSucursal,idEjercicio, " &
                                                           " idPeriodo,PFolio,SFolio,idDesMov,Concepto,Fecha,SCargo,SAbono,Estado) " &
                                                           " values (" + inv.Item(0) + "," + inv.Item(1) + "," + inv.Item(2) + "," &
                                                            inv.Item(3) + ",'" + inv.Item(4) + "'," + inv.Item(5) + "," + inv.Item(6) + "," + inv.Item(7) + "," + inv.Item(8) + "," &
                                                            inv.Item(9) + "," + inv.Item(10) + ",'" + inv.Item(11) + "'," + inv.Item(12) + ",'" + inv.Item(13) + "','" + CDate(inv.Item(14)).ToString("yyyy-MM-dd") + "'," + inv.Item(15) + "," + inv.Item(16) + ",'" + inv.Item(17) + "')")
                                        End If

                                    Next



                                Catch ex As Exception

                                End Try
                            End If



                        Case "Movimientos"
                            If document.Attributes("hasData").Value() <> "false" Then
                                Try
                                    Dim pathMovimientos = document.InnerText()
                                    Dim BytesMovimientos As Byte() = Convert.FromBase64String(pathMovimientos)
                                    Dim zipByte = DescifrarArchivo(BytesMovimientos, key)
                                    oFileStream = New System.IO.FileStream(Context.Server.MapPath("~/uploads/Movimientos.zip"), System.IO.FileMode.Create)
                                    oFileStream.Write(zipByte, 0, zipByte.Length)
                                    oFileStream.Close()

                                    Dim ZipToUnpack As String = oFileStream.Name
                                    Dim TargetDir As String = Context.Server.MapPath("~/uploads/")
                                    'Console.WriteLine("Extracting file {0} to {1}", ZipToUnpack, TargetDir)
                                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                                        Dim e As ZipEntry
                                        ' here, we extract every entry, but we could extract    
                                        ' based on entry name, size, date, etc.   
                                        For Each e In zip1
                                            e.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                                            'Se lee y transforma el archivo en una tabla
                                            TextFile = e.FileName

                                        Next
                                    End Using
                                    File.Delete(ZipToUnpack)
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile + ".temp"))
                                    tabla = txt_to_data(Context.Server.MapPath("~/uploads/" + TextFile), False, "@|")
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile))

                                    For Each mvs In tabla.AsEnumerable

                                        If Not IsDBNull(mvs.Item(16)) Then

                                            Persistencia.EjecutarSQL(" insert into tbco_movSaldosC (idRazon,idDistribuidor,idHPol," &
                                                            " idCta, tipo, Registro, idDesMov, idEjercicio, idPeriodo, ImporteD, " &
                                                            " ImporteH, concepto, Estado, gOrigen, gRef, IdUsuario, Fecha, idComprobante ) " &
                                                           " values (" + mvs.Item(0) + "," + mvs.Item(1) + "," + mvs.Item(2) + "," &
                                                            mvs.Item(3) + ",'" + mvs.Item(4) + "'," + mvs.Item(5) + "," + mvs.Item(6) + "," + mvs.Item(7) + "," + mvs.Item(8) + "," + mvs.Item(9) + "," &
                                                            mvs.Item(10) + ",'" + mvs.Item(11) + "','" + mvs.Item(12) + "','" + mvs.Item(13) + "','" + mvs.Item(14) + "','" + mvs.Item(15) + "','" + CDate(mvs.Item(16)).ToString("yyyy-MM-dd") + "'," + mvs.Item(17) + ")")
                                        End If

                                    Next



                                Catch ex As Exception

                                End Try
                            End If


                        Case "Comprobantes"
                            If document.Attributes("hasData").Value() <> "false" Then
                                Try
                                    Dim pathComprobantes = document.InnerText()
                                    Dim BytesComprobantes As Byte() = Convert.FromBase64String(pathComprobantes)
                                    Dim zipByte = DescifrarArchivo(BytesComprobantes, key)
                                    oFileStream = New System.IO.FileStream(Context.Server.MapPath("~/uploads/Comprobantes.zip"), System.IO.FileMode.Create)
                                    oFileStream.Write(zipByte, 0, zipByte.Length)
                                    oFileStream.Close()

                                    Dim ZipToUnpack As String = oFileStream.Name
                                    Dim TargetDir As String = Context.Server.MapPath("~/uploads/")

                                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                                        Dim e As ZipEntry
                                        ' here, we extract every entry, but we could extract    
                                        ' based on entry name, size, date, etc.   
                                        For Each e In zip1
                                            e.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                                            'Se lee y transforma el archivo en una tabla
                                            TextFile = e.FileName

                                        Next
                                    End Using
                                    File.Delete(ZipToUnpack)
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile + ".temp"))
                                    tabla = txt_to_data(Context.Server.MapPath("~/uploads/" + TextFile), False, "@|")
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile))

                                    For Each cmp In tabla.AsEnumerable

                                        If Not IsDBNull(cmp.Item(6)) Then

                                            Persistencia.EjecutarSQL(" insert into tbco_comprobanteC (idRazon, idDistribuidor, idEjercicio, idPeriodo, idComprobante, UUID, Fecha, NumFactExt  " &
                                                     "  , Moneda, TipoCambio,  Folio, Serie " &
                                                     "  ,TipoComprobante, Emisor, RFCEmisor " &
                                                     "  ,Receptor,  RFCReceptor, Subtotal, TaxId " &
                                                     "  ,IvaTr, IvaRt, Total, Version)" &
                                                           " values (" + cmp.Item(0) + "," + cmp.Item(1) + "," + cmp.Item(2) + "," + cmp.Item(3) + "," + cmp.Item(4) + ",'" + cmp.Item(5) + "','" + CDate(cmp.Item(6)).ToString("yyyy-MM-dd") + "','" &
                                                            cmp.Item(7) + "','" + cmp.Item(8) + "'," + cmp.Item(9) + ",'" + cmp.Item(10) + "','" + cmp.Item(11) + "','" + cmp.Item(12) + "','" + cmp.Item(13) + "','" &
                                                            cmp.Item(14) + "','" + cmp.Item(15) + "','" + cmp.Item(16) + "'," + cmp.Item(17) + ",'" + cmp.Item(18) + "'," + cmp.Item(19) + "," + cmp.Item(20) + "," &
                                                            cmp.Item(21) + ",'" + cmp.Item(22) + "')")
                                        End If

                                    Next



                                Catch ex As Exception

                                End Try
                            End If



                        Case "Cheques"
                            If document.Attributes("hasData").Value() <> "false" Then
                                Try
                                    Dim pathCheques = document.InnerText()
                                    Dim BytesComprobantes As Byte() = Convert.FromBase64String(pathCheques)
                                    Dim zipByte = DescifrarArchivo(BytesComprobantes, key)
                                    oFileStream = New System.IO.FileStream(Context.Server.MapPath("~/uploads/Cheques.zip"), System.IO.FileMode.Create)
                                    oFileStream.Write(zipByte, 0, zipByte.Length)
                                    oFileStream.Close()

                                    Dim ZipToUnpack As String = oFileStream.Name
                                    Dim TargetDir As String = Context.Server.MapPath("~/uploads/")

                                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                                        Dim e As ZipEntry
                                        ' here, we extract every entry, but we could extract    
                                        ' based on entry name, size, date, etc.   
                                        For Each e In zip1
                                            e.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                                            'Se lee y transforma el archivo en una tabla
                                            TextFile = e.FileName

                                        Next
                                    End Using
                                    File.Delete(ZipToUnpack)
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile + ".temp"))
                                    tabla = txt_to_data(Context.Server.MapPath("~/uploads/" + TextFile), False, "@|")
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile))

                                    For Each cmp In tabla.AsEnumerable

                                        If Not IsDBNull(cmp.Item(6)) Then

                                            Persistencia.EjecutarSQL(" insert into tbco_hchequera (idRazon,idDistribuidor,idEjercicio,idPeriodo,idHpol " &
                                                                     " ,Num,Banco,CtaOri,Fecha,Monto,AFavor,RFC)" &
                                                                     " values (" + cmp.Item(0) + "," + cmp.Item(1) + "," + cmp.Item(2) + "," + cmp.Item(3) + "," + cmp.Item(4) + ",'" + cmp.Item(5) + "','" + cmp.Item(6) + "','" &
                                                                     cmp.Item(7) + "','" + CDate(cmp.Item(8)).ToString("yyyy-MM-dd") + "'," + cmp.Item(9) + ",'" + cmp.Item(10) + "','" + cmp.Item(11) + "')")
                                        End If

                                    Next



                                Catch ex As Exception

                                End Try
                            End If


                        Case "Transferencias"
                            If document.Attributes("hasData").Value() <> "false" Then
                                Try
                                    Dim pathTransferencias = document.InnerText()
                                    Dim BytesTransferencias As Byte() = Convert.FromBase64String(pathTransferencias)
                                    Dim zipByte = DescifrarArchivo(BytesTransferencias, key)
                                    oFileStream = New System.IO.FileStream(Context.Server.MapPath("~/uploads/Comprobantes.zip"), System.IO.FileMode.Create)
                                    oFileStream.Write(zipByte, 0, zipByte.Length)
                                    oFileStream.Close()

                                    Dim ZipToUnpack As String = oFileStream.Name
                                    Dim TargetDir As String = Context.Server.MapPath("~/uploads/")

                                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                                        Dim e As ZipEntry
                                        ' here, we extract every entry, but we could extract    
                                        ' based on entry name, size, date, etc.   
                                        For Each e In zip1
                                            e.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                                            'Se lee y transforma el archivo en una tabla
                                            TextFile = e.FileName

                                        Next
                                    End Using
                                    File.Delete(ZipToUnpack)
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile + ".temp"))
                                    tabla = txt_to_data(Context.Server.MapPath("~/uploads/" + TextFile), False, "@|")
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile))

                                    For Each cmp In tabla.AsEnumerable

                                        If Not IsDBNull(cmp.Item(6)) Then

                                            Persistencia.EjecutarSQL(" insert into tbco_htransferencia (idRazon,idDistribuidor,idEjercicio,idPeriodo,idHpol " &
                                                                     ",CtaOri,BancoOri,Monto,CtaDest,BancoDest,Fecha,AFavor,RFC)" &
                                                           " values (" + cmp.Item(0) + "," + cmp.Item(1) + "," + cmp.Item(2) + "," + cmp.Item(3) + "," + cmp.Item(4) + ",'" + cmp.Item(5) + "','" + cmp.Item(6) + "'," &
                                                            cmp.Item(7) + ",'" + cmp.Item(8) + "','" + cmp.Item(9) + "','" + CDate(cmp.Item(10)).ToString("yyyy-MM-dd") + "','" + cmp.Item(11) + "','" + cmp.Item(12) + "')")
                                        End If

                                    Next



                                Catch ex As Exception

                                End Try
                            End If

                        Case "Saldos"
                            If document.Attributes("hasData").Value() <> "false" Then
                                Try
                                    Dim pathSaldos = document.InnerText()
                                    Dim BytesSaldos As Byte() = Convert.FromBase64String(pathSaldos)
                                    Dim zipByte = DescifrarArchivo(BytesSaldos, key)
                                    oFileStream = New System.IO.FileStream(Context.Server.MapPath("~/uploads/Saldos.zip"), System.IO.FileMode.Create)
                                    oFileStream.Write(zipByte, 0, zipByte.Length)
                                    oFileStream.Close()

                                    Dim ZipToUnpack As String = oFileStream.Name
                                    Dim TargetDir As String = Context.Server.MapPath("~/uploads/")
                                    'Console.WriteLine("Extracting file {0} to {1}", ZipToUnpack, TargetDir)
                                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                                        Dim e As ZipEntry
                                        ' here, we extract every entry, but we could extract    
                                        ' based on entry name, size, date, etc.   
                                        For Each e In zip1
                                            e.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                                            'Se lee y transforma el archivo en una tabla
                                            TextFile = e.FileName

                                        Next
                                    End Using
                                    File.Delete(ZipToUnpack)
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile + ".temp"))
                                    tabla = txt_to_data(Context.Server.MapPath("~/uploads/" + TextFile), False, "@|")
                                    File.Delete(Context.Server.MapPath("~/uploads/" + TextFile))

                                    For Each inv In tabla.AsEnumerable

                                        'Si el campo movimiento es nulo asigna un valor a cero.
                                        If inv.Item(12) = "" Then inv.Item(12) = "0.00"

                                        Persistencia.EjecutarSQL(" insert into tbco_SaldosC (idRazon,idDistribuidor,idCta, " &
                                                       " idEjercicio, idPeriodo, Dia, SaldoIni, SaldoFin, FechaModif, " &
                                                       " Cargos, Abonos,Status,Movimientos )" &
                                                       " values (" + inv.Item(0) + "," + inv.Item(1) + "," + inv.Item(2) + "," &
                                                        inv.Item(3) + "," + inv.Item(4) + "," + inv.Item(5) + "," + inv.Item(6) + "," + inv.Item(7) + ",'" + CDate(inv.Item(8)).ToString("yyyy-MM-dd") + "'," &
                                                        inv.Item(9) + "," + inv.Item(10) + ",'" + inv.Item(11) + "'," + inv.Item(12) + ")")

                                    Next



                                Catch ex As Exception

                                End Try
                            End If


                    End Select


                Next
                Persistencia.EjecutarSQL("update tbco_consolida set estado = 'I', fechafin = getdate() where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)


            End If


        End If
    End Sub

    Private Function DescifrarArchivo(origen As Byte(), key As AesKey) As Byte()

        Return AesUtil.DecryptFile(New MemoryStream(origen), key)

    End Function

    Private Function txt_to_data(ByVal filename As String, ByVal header As Boolean, ByVal delimiter As String) As DataTable
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