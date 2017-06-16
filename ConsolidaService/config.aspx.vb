
Imports ConsolidaService.GlobalVariables
Imports ConsolidaService.Persistencia
Imports System.Net.Mail
Imports System.Net.NetworkInformation
Imports System.Net
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Security
Imports System


Public Class config


    Inherits System.Web.UI.Page

    ' Dim conexion As New SqlConnection("Data Source=localhost;Initial Catalog=central;Persist Security Info=True;User ID=SA;")
    Dim sqlinsertpregunta, boxrfc, boxpara, mensaje, attach, boxcc1, boxde, boxpass, boxusuario, boxpuerto, boxhost, check, cadena As String
    Dim timepovar As String
    Dim tabla As DataTable
    Dim SQLData As DataTable = Nothing
    Dim boxfecha
    Dim control1 As String = ""
    Dim control As Boolean
    Dim usu, distribuidor, periodo, fini, ffin, estado, ejercicio
    Dim ProcesosLocales As New Process() '= Process.GetProcessesByName("Checatiempo.exe")
    Dim mesac, contiene, diahoy, cont0, cont1, cont2, cont3, cont4, cont5, cont6
    Dim arraydias(6)
    Dim lun, mar, mie, jue, vie, sab, dom As Boolean
    Dim a() As String
    Dim myCookie As HttpCookie
    Public iRazon As Integer
    Public iAgencia As Integer
    Public sUsuario As String = ""
    Public sRFC As String = ""

    '  Dim cryptostream As New CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write)

    Dim cadenasecret, cadiv As String
    Dim encrpdeskey() As Byte
    Dim DES As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
   
    'Dim ProcesosLocales As Process() = Process.GetProcessesByName("Checatiempo.exe")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        myCookie = HttpContext.Current.Request.Cookies("UserSettings")
        If Not IsNothing(Request.QueryString("sUsuario")) Then
            sUsuario = Request.QueryString("sUsuario")
        ElseIf Not IsNothing(myCookie) Then
            sUsuario = myCookie("Usuario")
        End If
        If myCookie("idRol") <> 4 Then
            iRazon = myCookie("idRol")
        Else
            iRazon = 1
        End If




        If Not IsPostBack Then
            Label1.Text = "Hora Actual    " & String.Format("{0:HH:mm:ss}", DateTime.Now)
            mesac = Date.Now
        

            If Razon.Items.Count = 0 Then
                Razon.DataSource = Persistencia.GetDataTable("select * from tbco_razonsocial")
                Razon.DataTextField = "razon"
                Razon.DataValueField = "idrazon"
                Razon.DataBind()

                ' insert an item at the beginning of the list
                '----------------------------------------------------
                Razon.Items.Insert(0, New ListItem("-- Todas --", "0"))

                If Razon.Items.Count > 0 Then Razon.SelectedValue = Razon.Items.FindByValue(iRazon).Value


            End If

            If Agencia.Items.Count = 0 Then
                Agencia.DataSource = Persistencia.GetDataTable(" select * from tbco_agencias ")
                Agencia.DataTextField = "Agencia"
                Agencia.DataValueField = "iddistribuidor"
                Agencia.DataBind()

                ' insert an item at the beginning of the list
                '----------------------------------------------------
                Agencia.Items.Insert(0, New ListItem("-- Todas --", "0"))

            End If

            cargar()
        End If
        



    End Sub

    Protected Sub idguardar1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles idguardar1.Click


       







        boxrfc = rfc.Text
        boxpuerto = puerto1.Text
        boxpara = para.Text
        boxcc1 = cc1.Text
        boxde = de.Text
        boxpass = pass.Text

        'cadenasecret = Encoding.ASCII.GetBytes(boxpass)
        boxusuario = usuario1.Text
        boxhost = host1.Text
        'boxfecha = fechaini.Text
        'control1 = control




        'If IsDate(tiempo1.Text("HH:mm")) Then
        Try
            If IsDate(CDate(tiempo1.Text).ToString("HH:mm")) Then

                timepovar = tiempo1.Text

            End If
        Catch ex As Exception

            Response.Output.Write("<script>javascript:window.alert('EL FORMATO DE LA HORA INGRESADA ES INCORRECTA, HORA:MINUTOS 24HORAS')</script>")
        End Try



        If RadioButtonList1.SelectedValue = "dia" Then


            RadioButtonList1.Visible = True
            boxfecha = "1900-01-01"

            For i As Integer = 0 To dias.Items.Count - 1
                If dias.Items(i).Selected Then
                    check += dias.Items(i).Value + "-"
                End If
            Next

        ElseIf RadioButtonList1.SelectedValue = "fecha" Then
            boxfecha = fechafin.Text

            check = ""


        End If



        tabla = Persistencia.GetDataTable("SELECT * FROM tbco_configuracion")

        If tabla.Rows.Count > 0 Then

            If timepovar <> "" And boxusuario <> "" And boxpass <> "" Then



                SQLData = Persistencia.GetDataTable(" SELECT * FROM TBCO_PARAM where idRazonsocial = " + iRazon.ToString)

                If SQLData.Rows.Count() > 0 Then

                    Persistencia.EjecutarSQL(" UPDATE TBCO_PARAM " &
                                    " SET rfc = '" + rfc.Text + "'" &
                                    " ,NoCertificado = '" + NoCertificado.Value.ToString + "'" &
                                    " ,RutaCer = '" + RutaCer.Value.ToString + "'" &
                                    " ,RutaKey = '" + RutaKey.Value.ToString + "'" &
                                    " ,PswKey = '" + PswKey.Value.ToString + "' " &
                                    " where idRazonsocial = " + iRazon.ToString)

                Else

                    Persistencia.EjecutarSQL(" insert into TBCO_PARAM (idRazonsocial,rfc,NoCertificado,RutaCer,RutaKey,PswKey) values (" + iRazon.ToString + ",'" + rfc.Text + "', '" + NoCertificado.Value.ToString + "', '" + RutaCer.Value.ToString + "', '" + RutaKey.Value.ToString + "', '" + PswKey.Value.ToString + "')")


                End If



                Persistencia.EjecutarSQL("UPDATE tbco_configuracion SET host='" + boxhost + "', puerto='" + boxpuerto + "', usuario='" + boxusuario + "', pass='" + encryps(boxpass) + "', de='" + boxde + "', para='" + boxpara + "', cc='" + boxcc1 + "', fecha='" + CDate(boxfecha).ToString("yyyy-MM-dd") + "', hora='" + timepovar + "', rfc='" + boxrfc + "', dia='" + check + "', idRazon=" + Razon.SelectedValue + ", idDistribuidor=" + Agencia.SelectedValue + "")


                Response.Output.Write("<script>javascript:window.alert('SU CONFIGURACIÓN SE GUARDO SIN PROBLEMAS')</script>")

            Else

                Response.Output.Write("<script>javascript:window.alert('NO SE REALIZO SU MODIFICACIÓN, FALTA CAPTURAR INFORMACIÓN')</script>")


            End If

        Else

            Persistencia.EjecutarSQL("INSERT INTO tbco_configuracion VALUES ('" & boxhost & "','" & boxpuerto & "','" & boxusuario & "','" & boxpass & "','" & boxde & "','" & boxpara & "','" & boxcc1 & "','" & CDate(boxfecha).ToString("yyyy-MM-dd") & "','" & timepovar & "','" & boxrfc & "','" & check & "'," & Razon.SelectedValue & "," & Agencia.SelectedValue & ")")

        End If


        If control = True Or control1 <> "" Then

            ' AQUI VAN TODOS LOS DATOS PARA CORREOS
            If pass.Text <> "" Then

                abre()
                control = False

                'ElseIf pass.Text = "" Then

                '    KillProcess("Checatiempo.exe")

            End If

        Else

            KillProcess("Checatiempo.exe")

            control = True
            control1 = "Va"

            'AQUI VAN TODOS LOS DATOS PARA CORREOS
            If pass.Text <> "" Then

                abre()

            End If
        End If

        cargar()
    End Sub

    Public Sub KillProcess(ByVal processName As String)

        On Error GoTo ErrHandler

        Dim oWMI

        Dim ret

        Dim sService

        Dim oWMIServices

        Dim oWMIService

        Dim oServices

        Dim oService

        Dim servicename

        oWMI = GetObject("winmgmts:")

        oServices = oWMI.InstancesOf("win32_process")

        For Each oService In oServices



            servicename = LCase(Trim(CStr(oService.Name) & ""))



            If InStr(1, servicename, LCase(processName), vbTextCompare) > 0 Then

                ret = oService.Terminate

            End If



        Next


        oServices = Nothing

        oWMI = Nothing


ErrHandler:

        Err.Clear()

    End Sub

    Function abre()

        Shell("C:\inetpub\wwwroot\Checatiempo.exe", AppWinStyle.Hide)


        Return 0
    End Function

    Protected Sub btnenviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnenviar.Click
        boxrfc = rfc.Text
        boxpuerto = puerto1.Text
        boxpara = para.Text
        boxcc1 = cc1.Text
        boxde = de.Text
        boxpass = pass.Text
        boxusuario = usuario1.Text
        boxhost = host1.Text
        'boxfecha = fechaini
        boxfecha = fechafin.Text
        check = dias.SelectedValue
        Dim mes As Date = Date.Now
        Dim annio As Date = Date.Now
        Dim hora As Date = TimeOfDay
        Dim nommes As Date = Date.Now


        If (boxhost.Trim <> "" And boxpuerto.Trim <> "" And boxusuario.Trim <> "" And boxpass.Trim <> "" And boxde.Trim <> "" And boxpara.Trim <> "") Then
            'SendEmail(e.FullPath & " - Deleted")
            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()
            SmtpServer.Credentials = New NetworkCredential(boxusuario, boxpass)
            SmtpServer.Port = CInt(boxpuerto)
            SmtpServer.Host = boxhost
            'SmtpServer.Host = "smtp.govirtual.com.mx"
            mail = New MailMessage()
            mail.From = New MailAddress(boxde)
            mail.To.Add(boxpara)

            'mail.To.Add("ethan07@gmail.com, octavio.gonzalez@govirtual.com.mx")
            mail.CC.Add(boxcc1)
            'mail.To.Add("octavio.gonzalez@govirtual.com.mx")
            mail.IsBodyHtml = True
            mail.Subject = "VDealerDDC - " + Date.Now.ToString("dd/MM/yyyy HH:mm:ss")

            Dim SHtml As String = ""
            Dim Datos As DataTable = Nothing

            Datos = Persistencia.GetDataTable("select a.agencia as DISTRIBUIDOR, p.nombre as PERIODO, ejercicio as EJERCICIO, c.fechaini as FECHAINI, c.fechafin as FECHAFIN, CASE WHEN estado = 'P' THEN 'PROCESANDO' ELSE 'REPORTADO' END AS ESTADO, usuario as USUARIO  from TBCO_CONSOLIDA c inner join  tbco_agencias a on a.iddistribuidor = c.iddistribuidor  inner join  tbco_periodos p on p.idperiodo = c.periodo and c.ejercicio = p.idejercicio  where ejercicio =2014 and periodo =" & CDate(mes).ToString("MM"))

            SHtml = "<html><head><h5>CONSOLIDACIÓN DE AGENCIAS</h5><h5>" & UCase(CDate(mes).ToString("MMMM")) & "   " & CDate(annio).ToString("yyyy") & "</head><body></h5><table border=""1""><tr><th>DISTRIBUIDOR</th><th>PERÍODO</th><th>EJERCICIO</th><th>FECHAINI</th><th>FECHAFIN</th><th>ESTADO</th><th>USUARIO</th></tr>"

            For Each reg In Datos.AsEnumerable

                SHtml += "<tr><td>" & reg.Item("DISTRIBUIDOR") & "</td><td>" & reg.Item("PERIODO") & "</td><td>" & reg.Item("EJERCICIO") & "</td><td>" & reg.Item("FECHAINI") & "</td><td>" & reg.Item("FECHAFIN") & " </td><td>" & reg.Item("ESTADO") & " </td><td>" & reg.Item("USUARIO") & " </td></tr>"

            Next

            SHtml += "</table>Reporte Enviado el: " & hora & "</body></html>"

            mail.Body = SHtml

            'If attach Then

            'Adjunta archivos empaquetados
            '      mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))
            '     mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))
            '    mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))
            'Adjunta archivos cifrados
            '   mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))
            '  mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))
            ' mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))
            'Adjunta archivo XML
            'mail.Attachments.Add(New Attachment("C:\inetpub\wwwroot\Reportes"))

            'End If

            Try
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
                SmtpServer.Send(mail)
                '  MessageBox.Show("El mensaje fue enviado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                '   MessageBox.Show("¡ Ha ocurrido un error !" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException.ToString, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            'SmtpServer.SendAsync(mail, New Object)
            'MsgBox("mail send")
            mail.Dispose()
            mail = Nothing
            SmtpServer = Nothing
        End If

    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonList1.SelectedIndexChanged

        If RadioButtonList1.SelectedValue = "fecha" Then

            fechafin.Visible = True
            wrapfecha.Style.Value = "display:block"
            wraplista.Style.Value = "display:none"


        ElseIf RadioButtonList1.SelectedValue = "dia" Then

            a = Split(dia.Text, "-")

            For i As Integer = 0 To a.Count - 1 Step 1

                If a(i) = "" Then
                    Exit For
                End If
                If a(i) = "lun" Then
                    dias.Items(0).Selected = True
                End If

                If a(i) = "mar" Then
                    dias.Items(1).Selected = True

                End If

                If a(i) = "mie" Then
                    dias.Items(2).Selected = True

                End If

                If a(i) = "jue" Then
                    dias.Items(3).Selected = True
                End If

                If a(i) = "vie" Then
                    dias.Items(4).Selected = True

                End If

                If a(i) = "sab" Then
                    dias.Items(5).Selected = True

                End If


                If a(i) = "dom" Then
                    dias.Items(6).Selected = True

                End If
            Next
        

            fechafin.Visible = False
            wrapfecha.Style.Value = "display:none"
            wraplista.Style.Value = "display:block"


        End If


    End Sub

    Function cargar()

        diahoy = Date.Now.ToString("dddd")


        SQLData = Persistencia.GetDataTable(" SELECT idRazonsocial, Automatizado, EstatusAuto, " &
                                       " Semiautomatizado,EstatusSemi,Importación,EstatusImportacion," &
                                       " rutastar,SitioDealer, isnull(rfc,'') as rfc, isnull(ConsolidaSrv,'') as ConsolidaSrv, " &
                                       " Modulos, Opcion1,Opcion2,Opcion3, isnull(NoCertificado,'') as NoCertificado, isnull(RutaCer,'') as RutaCer, isnull(RutaKey,'') as RutaKey, isnull(PswKey,'') as PswKey " &
                                       " FROM TBCO_PARAM " &
                                       " where idRazonsocial = " + iRazon.ToString)

        If SQLData.Rows.Count > 0 Then

            NoCertificado.Value = SQLData.Rows(0).Item("NoCertificado")
            RutaCer.Value = SQLData.Rows(0).Item("RutaCer")
            RutaKey.Value = SQLData.Rows(0).Item("RutaKey")
            PswKey.Value = SQLData.Rows(0).Item("PswKey")

        End If



        'Consulta configuración
        tabla = Persistencia.GetDataTable(" Select host,puerto,usuario,pass,de,para, cc, isnull(fecha,getdate()) as fecha, hora," &
                                          " rfc, dia, idRazon, idDistribuidor " &
                                          " from tbco_configuracion ")


        If tabla.Rows.Count > 0 Then
            'Si existe configuración registrada carga la información

            host1.Text = tabla.Rows(0).Item("host")
            puerto1.Text = tabla.Rows(0).Item("puerto")
            usuario1.Text = tabla.Rows(0).Item("usuario")
            pass.Text = tabla.Rows(0).Item("pass")
            de.Text = tabla.Rows(0).Item("de")
            para.Text = tabla.Rows(0).Item("para")
            cc1.Text = tabla.Rows(0).Item("cc")
            fechafin.Text = tabla.Rows(0).Item("fecha")
            tiempo1.Text = tabla.Rows(0).Item("hora")
            rfc.Text = tabla.Rows(0).Item("rfc")
            dia.Text = tabla.Rows(0).Item("dia")

            'label4.Text = "Día Seleccionado" & tabla.Rows(0).Item("dia")

        End If




        ' System.Diagnostics.Process.Start("\\127.0.0.1\wwwroot" + Me.dgvConcurrencia.CurrentRow.Cells("NroCaso").Value + ".txt")

        Return 0

    End Function

    Function encryps(ByVal Input As String)

        Dim archivo As String = "\\127.0.0.1\wwwroot\desk.txt"
        Dim archivo2 As String = "\\127.0.0.1\wwwroot\IV.txt"
        Dim sr As New System.IO.StreamReader(archivo)
        Dim sr1 As New System.IO.StreamReader(archivo2)
        cadenasecret = sr.ReadToEnd()
        cadiv = sr1.ReadToEnd()

        DES.Key = Convert.FromBase64String(cadenasecret)
        DES.IV = ASCIIEncoding.ASCII.GetBytes(cadiv)

        'Dim IV() As Byte = ASCIIEncoding.ASCII.GetBytes(cadiv) 'La clave debe ser de 8 caracteres  
        'Dim EncryptionKey() As Byte = Convert.FromBase64String(cadenasecret) 'No se puede alterar la cantidad de caracteres pero si la clave  
        'DES.Key = EncryptionKey
        'DES.IV = IV

        Dim buffer() As Byte = Encoding.UTF8.GetBytes(Input)

        sr.Close()
        sr1.Close()


       
        Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length()))




        Return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length()))

    End Function

   
End Class