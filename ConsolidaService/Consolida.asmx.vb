Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.IO
Imports System.Xml
Imports Ionic.Zip
Imports System.Threading

' Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://localhost/WebService", Name:="WebServicePrueba", Description:="Servicio Web de Prueba")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Consolida
    Inherits System.Web.Services.WebService
    Dim db As Persistencia = New Persistencia()
    Dim SQLProv As New DataTable
    Dim res As Integer = 0

    <WebMethod()> _
    Public Function Schedule(ByVal razonsocial As Integer, ByVal distribuidor As Integer, ByVal fecha As DateTime) As String
        Return "Se ha programado la fecha de entrega del distribuidor: " + distribuidor.ToString
    End Function

    <WebMethod()> _
    Public Function SendMovimientos(ByVal razonsocial As Integer, ByVal distribuidor As Integer, ByVal XML() As Byte) As String

        Return "Se ha cargado los movimientos del distribuidor: " + distribuidor.ToString
    End Function

    <WebMethod()> _
    Public Function ExistenSaldos(ByVal razonsocial As Integer, ByVal distribuidor As Integer, idejercicio As Integer, idperiodo As Integer) As String

        SQLProv = Persistencia.GetDataTable(" select top 1 idhpol from tbco_movsaldosc where idRazon = " + razonsocial.ToString + " and  idDistribuidor = " + distribuidor.ToString &
                                  " and idejercicio = " + idejercicio.ToString + " and idperiodo = " + idperiodo.ToString)
        If SQLProv.Rows.Count > 0 Then
            Return "SI"
        Else
            Return "NO"
        End If

    End Function

    <WebMethod()> _
    Public Function EliminarSaldos(ByVal razonsocial As Integer, ByVal distribuidor As Integer, idejercicio As Integer, idperiodo As Integer) As String


        If res > 0 Then
            Return "OK"
        Else
            Return "ERROR"
        End If

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function SendSaldos(ByVal ejercicio As Integer, ByVal periodo As Integer, ByVal usuario As String, ByVal razonsocial As Integer, ByVal distribuidor As Integer, ByVal XML() As Byte) As String
        Dim XMLString = System.Text.Encoding.UTF8.GetString(XML)
        Dim XMLFile = Convert.ToString(Context.Server.MapPath("~/uploads/Consolidacion" + "_" + distribuidor.ToString + "_" + periodo.ToString().PadLeft(2, "0") + ejercicio.ToString + "_" + Now.Date.ToString("yyyy-MM-dd") & ".xml"))

        SQLProv = Persistencia.GetDataTable("select iddistribuidor from tbco_consolida where iddistribuidor = " + distribuidor.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
        If SQLProv.Rows.Count > 0 Then
            Persistencia.EjecutarSQL("update tbco_consolida set estado = 'P', fechaini = getdate(), fechafin = getdate() where iddistribuidor = " + distribuidor.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
        Else
            Persistencia.EjecutarSQL(" insert into tbco_consolida (iddistribuidor, ejercicio, periodo, usuario, estado, fechaini, fechafin) " &
                                     " values (" + distribuidor.ToString + "," + ejercicio.ToString + "," + periodo.ToString + ",'" + usuario + "','P', getdate(), getdate())")
        End If


        ThreadPool.QueueUserWorkItem(AddressOf OpenXML, XMLString)
        File.WriteAllText(XMLFile, XMLString)

        'Dim result As Object() = New Object(1) {}
        'result(0) = Command.Create("LaunchNewProcess").Execute("Consolidacion " + distribuidor.ToString + "|" + ejercicio.ToString + "|" + periodo.ToString + "|" + razonsocial.ToString + "|" + distribuidor.ToString + "|" + XMLString)
        'result(1) = "mHandlers.Void"

        'Dim page As Page = HttpContext.Current.Handler
        'page.ClientScript.RegisterClientScriptBlock(, "consolida", "")

        'ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "$(document).ready(function(){EnableControls();alert('Overrides successfully Updated.');DisableControls();});", true);
        Return "CARGADO"
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function SendDatos(ByVal usuario As String, ByVal distribuidor As Integer, ByVal FechaTraslado As String, ByVal XML() As Byte) As String
        Dim XMLString = System.Text.Encoding.UTF8.GetString(XML)
        Dim XMLFile = Convert.ToString(Context.Server.MapPath("~/uploads/EAT" + "_" + distribuidor.ToString + "_" + FechaTraslado.ToString() + ".xml"))

        ThreadPool.QueueUserWorkItem(AddressOf OpenXML, XMLString)
        File.WriteAllText(XMLFile, XMLString)

        'Dim result As Object() = New Object(1) {}
        'result(0) = Command.Create("LaunchNewProcess").Execute("Consolidacion " + distribuidor.ToString + "|" + ejercicio.ToString + "|" + periodo.ToString + "|" + razonsocial.ToString + "|" + distribuidor.ToString + "|" + XMLString)
        'result(1) = "mHandlers.Void"

        'Dim page As Page = HttpContext.Current.Handler
        'page.ClientScript.RegisterClientScriptBlock(, "consolida", "")

        'ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "$(document).ready(function(){EnableControls();alert('Overrides successfully Updated.');DisableControls();});", true);
        Return "CARGADO"
    End Function

    Public Delegate Sub WaitCallback(ByVal state As Object)

    Private Sub OpenXML(ByVal X As Object)
        Dim Xml As String = CType(X, String)
        Dim key = AesUtil.GetAesKeys("G@V18TU4L")
        Dim razon As Integer = 0
        Dim agencia As Integer = 0
        Dim ejercicio As Integer = 0
        Dim periodo As Integer = 0
        Dim tabla As DataTable = New DataTable
        Dim SQLDC As DataTable = New DataTable
        Dim TextFile As String = ""
        If Xml.Trim.Length > 0 Then

            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(Xml)

            'Deserialize XML
            Dim root As XmlNode = xmlDoc.DocumentElement
            Dim delivery As XmlNode = root.SelectSingleNode("/delivery")

            razon = delivery.Attributes("idRazon").Value
            agencia = delivery.Attributes("idDistribuidor").Value()
            ejercicio = delivery.Attributes("idEjercicio").Value()
            periodo = delivery.Attributes("idPeriodo").Value()

            Persistencia.EjecutarSQL("delete from tbco_hchequera where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
            Persistencia.EjecutarSQL("delete from tbco_htransferencia where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
            Persistencia.EjecutarSQL("delete from tbco_comprobanteC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
            Persistencia.EjecutarSQL("delete from tbco_movHistoricoC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
            Persistencia.EjecutarSQL("delete from tbco_movSaldosC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)
            Persistencia.EjecutarSQL("delete from tbco_SaldosC where idrazon = " + razon.ToString + " and iddistribuidor = " + agencia.ToString + " and idejercicio = " + ejercicio.ToString + " and idperiodo = " + periodo.ToString)


            Dim documents As XmlNode = root.SelectSingleNode("/delivery/documents")

            Dim strAsBytes() As Byte = New System.Text.UTF8Encoding().GetBytes(Xml)
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
            Persistencia.EjecutarSQL("update tbco_consolida set estado = 'A', fechafin = getdate() where iddistribuidor = " + agencia.ToString + " and ejercicio = " + ejercicio.ToString + " and periodo = " + periodo.ToString)
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