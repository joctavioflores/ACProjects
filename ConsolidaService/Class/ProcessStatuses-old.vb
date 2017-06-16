Imports System.Threading
Imports System.Xml
Imports System.IO
Imports Ionic.Zip
Imports System.Runtime.Remoting.Contexts
Imports System.Xml.Serialization
Imports System.Xml.Xsl

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


    Public Shared Sub Start(ByVal data As Object)
        Dim ADOCFD As ProjectFEAsp.FEAsp = New ProjectFEAsp.FEAsp
        ADOCFD.Inicializa("")
        Dim XMLTipo As String = DirectCast(DirectCast(data, Object())(2), String)
        Dim db2 As Persistencia = New Persistencia(DirectCast(DirectCast(data, Object())(3), HttpCookie))
        Dim myCookie As HttpCookie = DirectCast(DirectCast(data, Object())(3), HttpCookie)
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

                    SQLTable = Persistencia.GetDataTable("select RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param")
                    If SQLTable.Rows.Count > 0 Then
                        cat.RFC = SQLTable.Rows(0).Item("RFC")
                        cat.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        cat.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")

                        ADOCFD.archivoprivk = key
                        ADOCFD.passwordprivk = psw

                    Else
                        cat.RFC = "XAXX010101000"
                        cat.noCertificado = "00001000000300527322"
                    End If

                    cadenaorginal += "|" + cat.RFC.Trim


                    'Atributo requerido para expresar el número cuentas
                    'que se relacionan en el catálogo
                    'SQLTable = Persistencia.GetDataTable("select * from tbco_abcCuenta ORDER BY CLAVELOCAL")
                    'If SQLTable.Rows.Count > 0 Then
                    '   cat. = SQLTable.Rows.Count
                    'Else
                    'cat.TotalCtas = 0
                    'End If

                    'Atributo requerido para expresar el mes en que
                    'inicia la vigencia del catálogo para la balanza
                    cat.Mes = Now.Month.ToString().PadLeft(2, "0")
                    cadenaorginal += "|" + Now.Month.ToString().PadLeft(2, "0")
                    'Atributo requerido para expresar el año en que inicia
                    'la vigencia del catálogo para la balanza
                    cat.Anio = Now.Year
                    cadenaorginal += "|" + Now.Year.ToString
                    'Atributo opcional que sirve para expresar el certificado de sello digital
                    'que ampara al archivo de contabilidad electrónica como texto, en formato base 64.


                    'AGREGANDO CUENTAS AL CATÁLOGO
                    Dim idx As Integer = 0
                    SQLCuenta = Persistencia.GetDataTable("select * from tbco_abccuenta where CodigoSAT is not null And CodigoSAT <> '' ")
                    Dim lstcta As New List(Of CatalogoCtas)
                    If SQLCuenta.Rows.Count > 0 Then
                        Dim llcount = 1
                        Dim rcta = SQLCuenta.Rows.Count
                        For Each cta In SQLCuenta.AsEnumerable
                            'INCREMENTANDO % PROGRESO DE POLIZAS

                            'Thread.Sleep(500)
                            process.SetStatus(CInt((llcount * 80) / rcta))
                            process.Name = "AGREGANDO CUENTAS ( " + llcount.ToString + " / " + rcta.ToString + " )"

                            Dim cuenta As CatalogoCtas = New CatalogoCtas()
                            cuenta.CodAgrup = cta.Item("CodigoSAT")
                            cuenta.NumCta = cta.Item("CLAVELOCAL")
                            cuenta.Desc = cta.Item("NOMBRE")

                            SQLSubCuenta = Persistencia.GetDataTable("select * from tbco_abcCuenta where idcta =  " + cta.Item("idpadrelocal").ToString)
                            If SQLSubCuenta.Rows.Count > 0 Then
                                cuenta.SubCtaDe = SQLSubCuenta.Rows(0).Item("CLAVELOCAL")
                            Else
                                cuenta.SubCtaDe = ""
                            End If

                            cuenta.Nivel = cta.Item("NIVELLOCAL")
                            cuenta.Natur = cta.Item("NATURALEZA")

                            cadenaorginal += "|" + cuenta.CodAgrup.Trim + "|" + cuenta.NumCta.Trim + "|" + cuenta.Desc.Trim + "|" + cuenta.SubCtaDe.Trim + "|" + cuenta.Nivel.ToString + "|" + cuenta.Natur.Trim
                            llcount += 1
                            lstcta.Add(cuenta)
                        Next
                        cat.Ctas = lstcta.ToArray
                    End If
                    cadenaorginal += "||"

                    'GENERANDO SELLO DE ´CATÁLOGO 
                    process.SetStatus(90)
                    process.Name = "GENERANDO SELLO DE CATÁLOGO"

                    'DEFINE FECHA PARA GENERAR SELLO
                    ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = ADOCFD.GenerarSello(cadenaorginal) 'GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw)
                    cat.Sello = sello

                    '95%
                    process.SetStatus(95)
                    process.Name = "GENERANDO ARCHIVO XML"
                    Dim request_serializer As XmlSerializer = Nothing
                    request_serializer = New XmlSerializer(GetType(Catalogo))

                    Dim request_writer As StreamWriter = Nothing
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



                End Try


            Case "POLIZAS"
                'GENERANDO ENCABEZADO XML
                
				
				
				
				
				
				
				
				
				
				
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

                    SQLTable = Persistencia.GetDataTable("select RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param")
                    If SQLTable.Rows.Count > 0 Then
                        pol.RFC = SQLTable.Rows(0).Item("RFC")
                        pol.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        pol.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")

                        ADOCFD.archivoprivk = key
                        ADOCFD.passwordprivk = psw

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




                    SQLPoliza = Persistencia.GetDataTable(" select idHPol, CASE WHEN IDPOLIZA = 1 THEN 3 WHEN IDPOLIZA = 2 THEN 1 WHEN IDPOLIZA = 3 THEN 2 END TIPO, " &
                                                          " RTRIM(SFOLIO) AS NUM, CONVERT(VARCHAR(10),FECHA,103) AS FECHA, CONCEPTO " &

                                                          " from tbco_movHistoricoPol where idejercicio = " + VProcesos.m_ej.ToString + " and idperiodo = " + VProcesos.m_pe.ToString)
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
                            SQLTrs = Persistencia.GetDataTable(" select idHPol, c.clavelocal as NumCta, c.nombre as DesCta, s.concepto, s.importeD as Cargo, " &
                                                                " s.importeH as Abono, s.registro, 'MXN' as Moneda, 0.00 as TipoCamb " &





                                                                " from tbco_movsaldospol s inner join " &
                                                                " tbco_abccuenta c on s.idcta = c.idcta " &
                                                                " where idHPol = " + pl.Item("idHPol").ToString)


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
                                    SQLComprob = Persistencia.GetDataTable(" select isnull(UUID,'') as UUID, isnull(Subtotal,0) as Monto, isnull(RFCEmisor,'') as RFC " &
                                                           " from tbco_comprobante c inner join tbco_movSaldospol s on s.idComprobante = c.idComprobante" &



                                                           " where c.TipoComprobante = 'CFDI' And s.idhpol = " + tr.Item("idhpol").ToString + " And s.Registro = " + tr.Item("registro").ToString)

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
                                    SQLComprob = Persistencia.GetDataTable(" select isnull(Folio,0) as Folio, isnull(Serie,'') as Serie, isnull(Subtotal,0) as Monto, isnull(RFCEmisor,'') as RFC " &
                                                                  " from tbco_comprobante c inner join tbco_movSaldospol s on s.idComprobante = c.idComprobante" &




                                                                  " where c.TipoComprobante = 'CFD' And s.idhpol = " + tr.Item("idhpol").ToString)

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
                                    SQLComprob = Persistencia.GetDataTable("select NumFactExt, Moneda, isnull(Subtotal,0) as Monto " &
                                                                 " from tbco_comprobante c inner join tbco_movSaldospol s on s.idComprobante = c.idComprobante" &




                                                                 " where TipoComprobante  = 'EXT' And s.idhpol = " + tr.Item("idhpol").ToString)

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
                                    SQLCheques = Persistencia.GetDataTable(" select isnull(Num,'') as Num, isnull(Banco,'') as Banco, isnull(CtaOri,'') as CtaOri, isnull(Fecha,'') as Fecha, Monto, AFavor as Benef, RFC " &



                                                                            " from tbco_hchequera" &
                                                                            " where idhpol = " + tr.Item("idhpol").ToString)

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
                                    SQLTranfer = Persistencia.GetDataTable(" select isnull(CtaOri,'') as CtaOri , isnull(BancoOri,'') as BancoOri, Monto, isnull(CtaDest,'') as CtaDest, isnull(BancoDest,'') as BancoDest , isnull(Fecha,'') as Fecha, AFavor as Benef, RFC  " &
                                                                            " from tbco_htransferencia" &



                                                                            " where idhpol = " + tr.Item("idhpol").ToString)
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
                    Dim XsltArgumentList As New XsltArgumentList


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
                    ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = ADOCFD.GenerarSello(cadenaorginal) 'GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw)
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

                    SQLTable = Persistencia.GetDataTable("select RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param")
                    If SQLTable.Rows.Count > 0 Then
                        auxfolios.RFC = SQLTable.Rows(0).Item("RFC")
                        auxfolios.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        auxfolios.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")

                        ADOCFD.archivoprivk = key
                        ADOCFD.passwordprivk = psw

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


                    SQLPoliza = Persistencia.GetDataTable(" select idHPol, CASE WHEN IDPOLIZA = 1 THEN 3 WHEN IDPOLIZA = 2 THEN 1 WHEN IDPOLIZA = 3 THEN 2 END TIPO, " &
                                                 " RTRIM(SFOLIO) AS NUM, CONVERT(VARCHAR(10),FECHA,103) AS FECHA, CONCEPTO " &






                                                 " from tbco_movHistoricoPol where idejercicio = " + VProcesos.m_ej.ToString + " and idperiodo = " + VProcesos.m_pe.ToString)

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
                            SQLComprob = Persistencia.GetDataTable(" select UUID, Subtotal as Monto, RFCEmisor as RFC " &
                                                          " from tbco_comprobante c inner join tbco_movSaldospol s on s.idComprobante = c.idComprobante" &





                                                          " where s.idhpol = " + pl.Item("idhpol").ToString)
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


                            ''AGREGAR COMPROBANTES NACIONALES CFD
                            'SQLComprob = Persistencia.GetDataTable(" select Folio, Serie, Monto, RFC " &





                            '                              " from tbco_comprobante" &
                            '                              " where UUID is null And isnull(TipoComprobante,'N') = 'N' And idhpol = " + pl.Item("idhpol").ToString)

                            'Dim lstnaCFD As New List(Of RepAuxFolDetAuxFolComprNalOtr)

                            'If SQLComprob.Rows.Count > 0 Then


                            '    Dim llCFD = 1
                            '    Dim rCFD = SQLComprob.Rows.Count

                            '    For Each com In SQLComprob.AsEnumerable

                            '        'INCREMENTANDO % PROGRESO DE CFD
                            '        process.SetStatus(CInt((llCFD * 99) / rCFD))
                            '        process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO CFD ( " + llCFD.ToString + " / " + rCFD.ToString + " )"


                            '        Dim comprobante As RepAuxFolDetAuxFolComprNalOtr = New RepAuxFolDetAuxFolComprNalOtr()

                            '        comprobante.CFD_CBB_NumFol = com.Item("Folio")
                            '        comprobante.CFD_CBB_Serie = com.Item("Serie")
                            '        comprobante.MontoTotal = com.Item("Monto")
                            '        comprobante.RFC = com.Item("RFC")

                            '        cadenaorginal += "|" + comprobante.CFD_CBB_Serie.Trim + "|" + comprobante.CFD_CBB_NumFol.ToString ' + "|" + comprobante.MontoTotal.ToString + "|" + comprobante.RFC.Trim

                            '        lstnaCFD.Add(comprobante)


                            '        llCFD += 1
                            '    Next
                            '    'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                            '    poliza.ComprNalOtr = lstnaCFD.ToArray


                            'End If



                            ''AGREGAR COMPROBANTE IMPORTANDOS
                            'SQLComprob = Persistencia.GetDataTable("select NumFactExt, Moneda, Monto " &





                            '                             " from tbco_comprobante" &
                            '                             " where isnull(TipoComprobante,'N') = 'I' And idhpol = " + pl.Item("idhpol").ToString)
                            'Dim lstex As New List(Of RepAuxFolDetAuxFolComprExt)
                            'If SQLComprob.Rows.Count > 0 Then


                            '    Dim llex = 1
                            '    Dim rex = SQLComprob.Rows.Count



                            '    For Each com In SQLComprob.AsEnumerable


                            '        'INCREMENTANDO % PROGRESO DE COMPROBANTE EXTRANEJEROS
                            '        process.SetStatus(CInt((llex * 99) / rex))
                            '        process.Name = "GENERANDO POLIZAS ( " + llcount.ToString + " / " + rpol.ToString + " )<br/>AGREGANDO COMPROBANTE EXTRANJERO ( " + llex.ToString + " / " + rex.ToString + " )"




                            '        Dim comprobante As RepAuxFolDetAuxFolComprExt = New RepAuxFolDetAuxFolComprExt()


                            '        comprobante.NumFactExt = com.Item("NumFactExt")
                            '        comprobante.MontoTotal = com.Item("Monto")
                            '        comprobante.Moneda = com.Item("Modena")





                            '        cadenaorginal += "|" + comprobante.NumFactExt.Trim '+ "|" + comprobante.MontoTotal.ToString + "|" + comprobante.Moneda.Trim
                            '        lstex.Add(comprobante)
                            '        llex += 1
                            '    Next
                            '    'SE AGREGA ARRELO DE COMPROBANTES NACIONALES
                            '    poliza.ComprExt = lstex.ToArray







                            'End If




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
                    ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = ADOCFD.GenerarSello(cadenaorginal) 'GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw)
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

                    SQLTable = Persistencia.GetDataTable("select RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param")
                    If SQLTable.Rows.Count > 0 Then
                        auxcta.RFC = SQLTable.Rows(0).Item("RFC")
                        auxcta.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        auxcta.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")

                        ADOCFD.archivoprivk = key
                        ADOCFD.passwordprivk = psw

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

                    Dim cuentas = Persistencia.GetDataTable(" select c.idcta, c.clavelocal as clave, c.nombre, s.saldoini, s.saldofin " &


                                                    " from tbco_SaldosCuenta s  " &
                                                    " inner join tbco_abccuenta c on c.idCTA = s.idCta " &
                                                    " where s.idPeriodo = " + VProcesos.m_pe.ToString() + " And s.idEjercicio = " + VProcesos.m_ej.ToString() + " and c.idCTA in ( " &
                                                    " select idCTA from tbco_movSaldosPol where idPeriodo = " + VProcesos.m_pe.ToString() + " And idEjercicio = " + VProcesos.m_ej.ToString + ") " &

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
                            Dim detalle = Persistencia.GetDataTable(" select convert(varchar(20),h.Fecha,103) as Fecha, rtrim(ltrim(isnull(h.sfolio,''))) as Folio, h.concepto, isnull(s.ImporteD,0) as Debe, isnull(s.ImporteH,0) as Haber " &


                                                            " from tbco_movHistoricoPol h " &
                                                            " inner join tbco_movSaldosPol s on h.idHPol = s.idHPol " &
                                                            " where h.idPeriodo = " + VProcesos.m_pe.ToString() + " And s.idEjercicio = " + VProcesos.m_ej.ToString() + " And s.idCta = " + pl.Item("idCta").ToString &
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
                    ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = ADOCFD.GenerarSello(cadenaorginal) 'GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw)
                    auxfolios.Sello = sello

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
                    SQLTable = Persistencia.GetDataTable("select RFC,NoCertificado,RutaCer,RutaKey,PswKey from tbco_param")
                    If SQLTable.Rows.Count > 0 Then
                        blz.RFC = SQLTable.Rows(0).Item("RFC")
                        blz.noCertificado = SQLTable.Rows(0).Item("NoCertificado")
                        blz.Certificado = Convert.ToBase64String(System.IO.File.ReadAllBytes(SQLTable.Rows(0).Item("RutaCer")))
                        psw = SQLTable.Rows(0).Item("PswKey")
                        key = SQLTable.Rows(0).Item("RutaKey")

                        ADOCFD.archivoprivk = key
                        ADOCFD.passwordprivk = psw

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
                    SQLTable = Persistencia.GetDataTable("select MAX(FECHA) AS FECHA from tbco_HistoricoCE where tiporeporte like 'BLZ' And rfc like '" + blz.RFC + "' And Periodo = " + VProcesos.m_pe.ToString + "  And Ejercicio = " + VProcesos.m_ej.ToString)
                    If SQLTable.Rows.Count > 0 And IsDate(SQLTable.Rows(0).Item("FECHA")) Then

                        blz.TipoEnvio = "C"
                        cadenaorginal += "|" + blz.TipoEnvio

                        blz.FechaModBalSpecified = True
                        blz.FechaModBal = CDate(SQLTable.Rows(0).Item("FECHA")).ToString("yyyy-MM-dd")

                        cadenaorginal += "|" + blz.FechaModBal.ToString

                        Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " &
                                        "values ('" + blz.RFC + "'," + VProcesos.m_pe.ToString + "," + VProcesos.m_ej.ToString + ",'BLZ','C','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")

                    Else



                        blz.TipoEnvio = "N"
                        cadenaorginal += "|" + blz.TipoEnvio

                        Persistencia.EjecutarSQL("insert into tbco_HistoricoCE (rfc,periodo,ejercicio,tiporeporte,tipoenvio,fecha,usuario) " &
                                        "values ('" + blz.RFC + "'," + VProcesos.m_pe.ToString + "," + VProcesos.m_ej.ToString + ",'BLZ','N','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + myCookie("Usuario") + "')")
                    End If




                    'AGREGANDO REGISTROS A LA BALANZA
                    SQLCuenta = Persistencia.GetDataTable("exec COspa_RptBalanza 'L', 1, 5," + VProcesos.m_ej.ToString + "," + VProcesos.m_pe.ToString + ",'E'")
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

                    'GENERANDO SELLO DE CATÁLOGO 
                    process.SetStatus(90)
                    process.Name = "GENERANDO SELLO DE BALANZA"

                    'DEFINE FECHA PARA GENERAR SELLO
                    ADOCFD.Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    Dim sello = ADOCFD.GenerarSello(cadenaorginal) 'GeneraSello.ObtenerSelloDigital(cadenaorginal, key, psw)

                    blz.Sello = sello

                    '95%
                    process.SetStatus(95)
                    process.Name = "GENERANDO ARCHIVO XML"
                    Dim request_serializer As XmlSerializer = Nothing
                    request_serializer = New XmlSerializer(GetType(Balanza))

                    Dim request_writer As StreamWriter = Nothing
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



                End Try

        End Select


        Thread.Sleep(2000)
        ArrayList.Synchronized(DirectCast(DirectCast(data, Object())(1), ArrayList)).Remove(process)
    End Sub

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
