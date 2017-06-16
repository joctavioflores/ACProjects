Imports System.Web
Imports System.Web.Services
Imports System.IO

Public Class upload
    Implements System.Web.IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim file As HttpPostedFile = context.Request.Files("Filedata")

        If Not IsNothing(file) Then
            Dim targetDirectory As String = context.Server.MapPath(context.Request("folder"))

            If Not Directory.Exists(targetDirectory) Then
                Directory.CreateDirectory(targetDirectory)
            End If

            Dim targetFilePath As String = Path.Combine(targetDirectory, file.FileName)

            file.SaveAs(targetFilePath)

            context.Response.Write("1")
        Else
            context.Response.Write("0")
        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class