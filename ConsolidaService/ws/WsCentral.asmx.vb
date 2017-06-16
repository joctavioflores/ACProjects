Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Runtime.Remoting.Contexts
Imports System.Xml.Serialization
Imports System.Xml.Xsl
Imports System.IO
Imports Ionic.Zip


' Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://govirtual.com.mx/WsCentral")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WsCentral
    Inherits System.Web.Services.WebService
    Dim db As Persistencia = New Persistencia()

    <WebMethod()> _
    Function SincronizaEAT(ByVal paramtxtsRuta As String) As String

        If FileIO.FileSystem.FileExists(paramtxtsRuta) Then

            Return "Archivo Procesado " + paramtxtsRuta

        Else
            Return "No existe el archivo " + paramtxtsRuta

        End If



    End Function

End Class