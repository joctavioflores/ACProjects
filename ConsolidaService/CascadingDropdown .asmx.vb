Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports AjaxControlToolkit

' Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class CascadingDropdown
    Inherits System.Web.Services.WebService
    Dim SQLProv As New DataTable
    Dim myCookie As HttpCookie

    <WebMethod()> _
    Public Function BindRazonDetails(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim donde As String = ""
        myCookie = HttpContext.Current.Request.Cookies("UserSettings")

        Select Case myCookie("idRol")
            Case "1"
                donde = "where idrazon = 1"
            Case "2"
                donde = "where idrazon = 2"
            Case "3"
                donde = "where idrazon = 3"
        End Select
        SQLProv = Persistencia.GetDataTable("select * from tbco_razonsocial " + donde)


        Dim RazonDetails As New List(Of CascadingDropDownNameValue)()
        For Each lsrazon In SQLProv.AsEnumerable
            RazonDetails.Add(New CascadingDropDownNameValue(lsrazon.Item("razon"), lsrazon.Item("idrazon")))
        Next

        Return RazonDetails.ToArray
    End Function

End Class