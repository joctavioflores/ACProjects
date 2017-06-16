Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks



<Flags()> _
Public Enum AesOperation
    Encrypt = 1
    Decrypt = 2
End Enum

Public Class AesUtil

    Public Shared Function GetAesKeys(EncryptionKey As String) As AesKey
        Using aesencryptor As Aes = Aes.Create()

            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})

            aesencryptor.Key = pdb.GetBytes(32)
            aesencryptor.IV = pdb.GetBytes(16)

            Return New AesKey(aesencryptor.Key, aesencryptor.IV)
        End Using

    End Function

    Public Shared Function DecryptFile(file As Stream, EncryptionKey As String) As Byte()
        Dim FileByte = New List(Of Byte)(1000)

        Using aesencryptor As Aes = Aes.Create()

            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
            &H65, &H64, &H76, &H65, &H64, &H65, _
            &H76})

            aesencryptor.Key = pdb.GetBytes(32)
            aesencryptor.IV = pdb.GetBytes(16)

            Using cs As New CryptoStream(file, aesencryptor.CreateDecryptor(), CryptoStreamMode.Read)
                Dim data As Integer
                While (InlineAssignHelper(data, cs.ReadByte())) <> -1
                    FileByte.Add(CByte(data))
                End While
            End Using
        End Using
        Return FileByte.ToArray()
    End Function


    Public Shared Function DecryptFile(file As Stream, key As AesKey) As Byte()
        Dim FileByte = New List(Of Byte)(1000)

        Using aesencryptor As Aes = Aes.Create()
            aesencryptor.Key = key.key
            aesencryptor.IV = key.IV

            Using cs As New CryptoStream(file, aesencryptor.CreateDecryptor(), CryptoStreamMode.Read)
                Dim data As Integer
                While (InlineAssignHelper(data, cs.ReadByte())) <> -1
                    FileByte.Add(CByte(data))
                End While
            End Using
        End Using
        Return FileByte.ToArray()
    End Function

    Public Shared Function DecryptText(base64Text As String, key As AesKey) As String

        Dim text = ""


        Using aesencryptor As Aes = Aes.Create()
            Dim bytes As Byte() = Convert.FromBase64String(base64Text)
            Using msDecrypt As New MemoryStream(bytes)
                aesencryptor.Key = key.key
                aesencryptor.IV = key.IV

                Dim decryptor As ICryptoTransform = aesencryptor.CreateDecryptor(aesencryptor.Key, aesencryptor.IV)
                Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                    Using srDecrypt As New StreamReader(csDecrypt)


                        text = srDecrypt.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using

        Return text
    End Function

    Public Shared Function DecryptText(base64Text As String, EncryptionKey As String) As String

        Dim text = ""


        Using aesencryptor As Aes = Aes.Create()

            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})

            Dim bytes As Byte() = Convert.FromBase64String(base64Text)
            Using msDecrypt As New MemoryStream(bytes)
                aesencryptor.Key = pdb.GetBytes(32)
                aesencryptor.IV = pdb.GetBytes(16)

                Dim decryptor As ICryptoTransform = aesencryptor.CreateDecryptor(aesencryptor.Key, aesencryptor.IV)
                Using csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
                    Using srDecrypt As New StreamReader(csDecrypt)

                        text = srDecrypt.ReadToEnd()

                    End Using
                End Using
            End Using
        End Using

        Return text
    End Function

    Public Shared Function EncryptText(plainText As String, EncryptionKey As String) As String

        Dim outPut = ""
        Using aesencryptor As Aes = Aes.Create()

            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})

            aesencryptor.Key = pdb.GetBytes(32)
            aesencryptor.IV = pdb.GetBytes(16)


            Dim encryptor As ICryptoTransform = aesencryptor.CreateEncryptor(aesencryptor.Key, aesencryptor.IV)


            Using msEncrypt As New MemoryStream()

                Using csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                    Using swEncrypt As New StreamWriter(csEncrypt)
                        swEncrypt.Write(plainText)
                    End Using
                End Using
                outPut = Convert.ToBase64String(msEncrypt.ToArray())
            End Using
        End Using
        Return outPut
    End Function





    Public Shared Function EncryptText(plainText As String, AesKey As AesKey) As String

        Dim outPut = ""

        Using aesencryptor As Aes = Aes.Create()



            aesencryptor.Key = AesKey.key
            aesencryptor.IV = AesKey.IV


            Dim encryptor As ICryptoTransform = aesencryptor.CreateEncryptor(aesencryptor.Key, aesencryptor.IV)


            Using msEncrypt As New MemoryStream()

                Using csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
                    Using swEncrypt As New StreamWriter(csEncrypt)
                        swEncrypt.Write(plainText)
                    End Using
                End Using
                outPut = Convert.ToBase64String(msEncrypt.ToArray())
            End Using
        End Using
        Return outPut
    End Function

    Public Shared Function EncryptFile(fsIn As FileStream, outputFile As String, EncryptionKey As String) As String

        Using aesencryptor As Aes = Aes.Create()

            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
              &H65, &H64, &H76, &H65, &H64, &H65, _
              &H76})

            aesencryptor.Key = pdb.GetBytes(32)
            aesencryptor.IV = pdb.GetBytes(16)


            Using fsCrypt As New FileStream(outputFile, FileMode.Create)
                Using encryptor As ICryptoTransform = aesencryptor.CreateEncryptor(aesencryptor.Key, aesencryptor.IV)
                    Using cs As New CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write)
                        Dim data As Integer
                        While (InlineAssignHelper(data, fsIn.ReadByte())) <> -1
                            cs.WriteByte(CByte(data))
                        End While
                    End Using

                End Using
            End Using
        End Using
        Return ""

    End Function




    Public Shared Function EncryptFile(fsIn As FileStream, outputFile As String, AesKey As AesKey) As String

        Using aesencryptor As Aes = Aes.Create()
            aesencryptor.Key = AesKey.key
            aesencryptor.IV = AesKey.IV


            Using fsCrypt As New FileStream(outputFile, FileMode.Create)
                Using encryptor As ICryptoTransform = aesencryptor.CreateEncryptor(AesKey.key, AesKey.IV)
                    Using cs As New CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write)
                        Dim data As Integer
                        While (InlineAssignHelper(data, fsIn.ReadByte())) <> -1
                            cs.WriteByte(CByte(data))
                        End While
                    End Using

                End Using
            End Using
        End Using
        Return ""

    End Function

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

End Class
