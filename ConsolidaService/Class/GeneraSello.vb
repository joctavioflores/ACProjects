Imports System.IO
Imports System.Security.Cryptography
Imports System.Security
Imports GoVirtualMCo.JavaScience
Imports ConsolidaService.JavaScience

Public Class GeneraSello


    Public Shared Function GetCadenaOriginal(ByVal xmlDoc As String, ByVal fileXSLT As String) As String
        Dim strCadenaOriginal As String
        Dim newFile = Path.GetTempFileName()

        Dim Xsl = New System.Xml.Xsl.XslCompiledTransform()
        Xsl.Load(fileXSLT)
        Xsl.Transform(xmlDoc, newFile)
        Xsl = Nothing

        Dim sr = New IO.StreamReader(newFile)
        strCadenaOriginal = sr.ReadToEnd
        sr.Close()

        'Eliminamos el archivo Temporal
        System.IO.File.Delete(newFile)

        fileXSLT = Nothing
        newFile = Nothing
        Xsl = Nothing
        sr.Dispose()

        Return strCadenaOriginal
    End Function


    Public Shared Function ObtenerSelloDigital(ByVal cadenaOriginal As String, ByVal rutaLlavePrivada As String, ByVal password As String) As String
        Dim passwordSeguro As New SecureString()
        passwordSeguro.Clear()
        For Each c As Char In password.ToCharArray()
            passwordSeguro.AppendChar(c)
        Next

        Dim llavePrivadaBytes As Byte() = System.IO.File.ReadAllBytes(rutaLlavePrivada)
        Dim rsa As RSACryptoServiceProvider = opensslkey.DecodeEncryptedPrivateKeyInfo(llavePrivadaBytes, passwordSeguro)
        Dim hasher As New SHA1CryptoServiceProvider()
        Dim bytesFirmados As Byte() = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(cadenaOriginal), hasher)
        Dim selloDigital As String = Convert.ToBase64String(bytesFirmados)
        Return selloDigital

    End Function


End Class
