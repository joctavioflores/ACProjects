Imports System
Imports System.Collections
Imports System.Threading
Imports System.Web
Imports System.Reflection
Imports System.IO
Imports System.Xml

Public Class Command
#Region "Command functionality"

    Private m_CommandName As String = ""

    Public Sub New(commandName As String)
        m_CommandName = commandName
    End Sub

    Public Shared Function Create(commandName As String) As Command
        Return New Command(commandName)
    End Function

    Public Function Execute(data As Object) As Object
        Dim type As Type = Me.[GetType]()
        Dim method As MethodInfo = type.GetMethod(m_CommandName)
        Dim args As Object() = New Object() {data}
        Try
            Return method.Invoke(Me, args)
        Catch ex As Exception
            ' TODO: Add logging functionality
            Throw
        End Try
    End Function

#End Region

#Region "Public execution commands"

    Public Function GetTime(data As Object) As Object
        Return DateTime.Now
    End Function

    Public Function GetProductNameFromDatabase(data As Object) As Object
        ' TODO: add real functionality later
        If CInt(data) = 1 Then
            Return "Computer"
        Else
            Return "Unknown"
        End If
    End Function

    Public Function LaunchNewProcess(data As Object) As Object
        Dim a() As String
        Dim myCookie As HttpCookie
        a = CType(data, String).Split("|")

        Dim newProcess As New ProcessStatus(a(0))

        Dim allProcesses As ArrayList = ProcessStatuses.[Get]()

        ArrayList.Synchronized(allProcesses).Add(newProcess)
        myCookie = HttpContext.Current.Request.Cookies("UserSettings")

        Select Case a(0)
            Case "EAT"
                VProcesos.m_ej = a(1)
                VProcesos.m_pe = a(2)
                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/ScriptsEAT/")


                'Archivo a importar
                If Path.GetExtension("c:\texto\" + a(9)) = ".txt" Then
                    ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, "c:\texto\" + a(9)})
                End If

            Case "POLIZAS"
                VProcesos.m_ej = a(1)
                VProcesos.m_pe = a(2)
                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_TipoSolicitud = a(10)

                Select Case a(10)
                    Case "AF", "FC"
                        VProcesos.m_NumOrden = a(11)
                    Case "DE", "CO"
                        VProcesos.m_NumTramite = a(11)
                End Select
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/")
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, ""})


            Case "CATALOGO"

                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/")
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, ""})


            Case "AUXILIARFOLIOS"

                VProcesos.m_ej = a(1)
                VProcesos.m_pe = a(2)
                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_TipoSolicitud = a(10)

                Select Case a(10)
                    Case "AF", "FC"
                        VProcesos.m_NumOrden = a(11)
                    Case "DE", "CO"
                        VProcesos.m_NumTramite = a(11)
                End Select
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/")
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, ""})


            Case "AUXILIARCUENTAS"

                VProcesos.m_ej = a(1)
                VProcesos.m_pe = a(2)
                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_TipoSolicitud = a(10)

                Select Case a(10)
                    Case "AF", "FC"
                        VProcesos.m_NumOrden = a(11)
                    Case "DE", "CO"
                        VProcesos.m_NumTramite = a(11)
                End Select
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/")
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, ""})


            Case "BALANZA"
                VProcesos.m_ej = a(1)
                VProcesos.m_pe = a(2)
                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/")
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, ""})

            Case "IMPORTAR"

                VProcesos.m_ej = a(1)
                VProcesos.m_pe = a(2)
                VProcesos.m_razon = a(4)
                VProcesos.m_agencia = a(5)
                VProcesos.m_usuario = a(6)
                VProcesos.m_pathXSLT = HttpContext.Current.Server.MapPath("~/xslt/")
                VProcesos.m_pathXML = HttpContext.Current.Server.MapPath("~/uploads/")

                'Archivo a importar

                If Path.GetExtension(HttpContext.Current.Server.MapPath("~/uploads/") + a(9)) = ".xml" Then
                    Dim xmlDoc As New XmlDocument()
                    xmlDoc.Load(HttpContext.Current.Server.MapPath("~/uploads/") + a(9))
                    ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, xmlDoc.InnerXml})
                End If

                If Path.GetExtension(HttpContext.Current.Server.MapPath("~/uploads/") + a(9)) = ".xls" Or Path.GetExtension(HttpContext.Current.Server.MapPath("~/uploads/") + a(9)) = ".xlsx" Then
                    ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ProcessStatuses.Start), New Object() {newProcess, allProcesses, a(0), myCookie, HttpContext.Current.Server.MapPath("~/uploads/") + a(9)})
                End If




        End Select



        Return Nothing
    End Function


    Public Function GetProcessStatuses(data As Object) As Object
        Dim allProcesses As ArrayList = ProcessStatuses.[Get]()
        SyncLock allProcesses.SyncRoot
            Return allProcesses.ToArray()
        End SyncLock
    End Function

#End Region
End Class
