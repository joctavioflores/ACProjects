Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ValidaUsuario
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function

    <WebMethod()> _
    Public Function Alta(ByVal empSex As String) As String

        Dim arreglo As String() = empSex.Split("|")
        Dim valor As String = ""

        If Not IsNothing(arreglo) Then

            Dim idUsuario = arreglo(0)
            Dim Nombre = arreglo(1)
            Dim Usuario = arreglo(2)
            Dim Contraseña = arreglo(3)
            Dim Rol = arreglo(4)

            If Not CInt(idUsuario) > 0 Then
                Persistencia.EjecutarSQL("exec AbcUSUARIO 1,'','',NULL,'" + Nombre.ToString.ToUpper + "','" + Usuario.ToString.ToUpper + "','" + Contraseña.ToString + "',1," + Rol.ToString + "")
            ElseIf CInt(idUsuario) > 0 Then
                Persistencia.EjecutarSQL("exec AbcUSUARIO 2,'',''," + idUsuario.ToString + ",'" + Nombre.ToString.ToUpper + "','" + Usuario.ToString.ToUpper + "','" + Contraseña.ToString + "',1," + Rol.ToString + "")
            End If

        End If

        Return valor

    End Function



    <WebMethod()> _
    Public Function Baja(ByVal empSex As String) As String

        Dim arreglo As String() = empSex.Split("|")
        Dim valor As String = ""

        If Not IsNothing(empSex) Then

            Dim idUsuario = empSex

            Persistencia.EjecutarSQL(" delete from USUARIO where idusuario =  " + idUsuario.ToString)

        End If

        Return valor

    End Function



    <WebMethod()> _
    Public Function ValidaNick(ByVal empSex As String) As String

        Dim arreglo As String() = empSex.Split("|")
        Dim valor As String = ""
        Dim SQLUsuario As DataTable = Nothing

        If Not IsNothing(arreglo) Then

            Dim idUsuario = arreglo(0)
            Dim Nombre = arreglo(1)
            Dim Usuario = arreglo(2)
            Dim Contraseña = arreglo(3)
            Dim Rol = arreglo(4)

            SQLUsuario = Persistencia.GetDataTable(" select * from USUARIO where upper(rtrim(nickname)) like '" + Usuario.ToUpper + "'")

            If SQLUsuario.Rows.Count > 0 And idUsuario = 0 Then
                valor = "SI"
            Else
                valor = "NO"
            End If

        End If

        Return valor

    End Function


End Class
