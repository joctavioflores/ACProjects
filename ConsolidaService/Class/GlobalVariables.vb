Public Class GlobalVariables
    Public Property USUARIO() As String
        Get
            Return m_USUARIO
        End Get
        Set(ByVal value As String)
            m_USUARIO = value
        End Set
    End Property
    Public Shared m_USUARIO As String

    Public Property PASSWORD() As String
        Get
            Return m_PASSWORD
        End Get
        Set(ByVal value As String)
            m_PASSWORD = value
        End Set
    End Property
    Public Shared m_PASSWORD As String

    Public Property SC() As String
        Get
            Return m_SC
        End Get
        Set(ByVal value As String)
            m_SC = value
        End Set
    End Property
    Public Shared m_SC As String = "Data Source=localhost;Initial Catalog=central;Persist Security Info=True;User ID=sa; Password=GoVirtual"


    Public Property DB() As String
        Get
            Return m_DB
        End Get
        Set(ByVal value As String)
            m_DB = value
        End Set
    End Property
    Public Shared m_DB As String = "central"

    Public Property SESSIONTIMEOUT() As Integer
        Get
            Return m_SESSIONTIMEOUT
        End Get
        Set(ByVal value As Integer)
            m_SESSIONTIMEOUT = value
        End Set
    End Property
    Public Shared m_SESSIONTIMEOUT As Integer

End Class
