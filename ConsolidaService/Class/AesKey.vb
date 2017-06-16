
Public Class AesKey

    Public Property key() As Byte()
        Get
            Return m_key
        End Get
        Private Set(value As Byte())
            m_key = value
        End Set
    End Property
    Private m_key As Byte()
    Public Property IV() As Byte()
        Get
            Return m_IV
        End Get
        Private Set(value As Byte())
            m_IV = value
        End Set
    End Property
    Private m_IV As Byte()
    Public Property IVBase64() As String
        Get
            Return m_IVBase64
        End Get
        Private Set(value As String)
            m_IVBase64 = value
        End Set
    End Property
    Private m_IVBase64 As String
    Public Property KeyBase64() As String
        Get
            Return m_KeyBase64
        End Get
        Private Set(value As String)
            m_KeyBase64 = value
        End Set
    End Property
    Private m_KeyBase64 As String

    Public Sub New(key As Byte(), IV As Byte())
        Me.key = key
        Me.IV = IV
        Me.IVBase64 = Convert.ToBase64String(IV)
        Me.KeyBase64 = Convert.ToBase64String(key)
    End Sub



    Public Sub New(key As String, IV As String)

        Me.key = Convert.FromBase64String(key)
        Me.IV = Convert.FromBase64String(IV)
        Me.IVBase64 = IV


        Me.KeyBase64 = key
    End Sub

End Class
