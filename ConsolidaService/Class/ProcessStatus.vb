Public Class ProcessStatus
    Public Sub New(name As String)
        m_Name = name
    End Sub

    Private m_Status As Integer = 0

    Public Property Status() As Integer
        Get
            Return m_Status
        End Get
        Set(value As Integer)
            m_Status = value
        End Set
    End Property

    Private m_Name As String = ""

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(value As String)
            m_Name = value
        End Set
    End Property

    Public Sub IncrementStatus()
        SyncLock Me
            m_Status += 1
        End SyncLock
    End Sub

    Public Sub IncrementStatus(ByVal count As Integer)
        SyncLock Me
            m_Status += count
        End SyncLock
    End Sub

    Public Sub SetStatus(ByVal count As Integer)
        SyncLock Me
            m_Status = count
        End SyncLock
    End Sub
End Class
