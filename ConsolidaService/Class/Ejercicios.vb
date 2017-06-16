Public Class Ejercicios

    Public Shared result As Object() = New Object(1) {}

    Public Property usuario() As String
        Get
            Return m_usuario
        End Get
        Set(ByVal value As String)
            m_usuario = value
        End Set
    End Property
    Public Shared m_usuario As String

    Public Property ej() As Integer
        Get
            Return m_ej
        End Get
        Set(ByVal value As Integer)
            m_ej = value
        End Set
    End Property
    Public Shared m_ej As Integer

    Public Property pe() As Integer
        Get
            Return m_pe
        End Get
        Set(ByVal value As Integer)
            m_pe = value
        End Set
    End Property
    Public Shared m_pe As Integer

    Public Property idHPol() As Integer
        Get
            Return m_idHPol
        End Get
        Set(ByVal value As Integer)
            m_idHPol = value
        End Set
    End Property
    Public Shared m_idHPol As Integer

    Public Property registro() As Integer
        Get
            Return m_registro
        End Get
        Set(ByVal value As Integer)
            m_registro = value
        End Set
    End Property
    Public Shared m_registro As Integer

    Public Property razon() As Integer
        Get
            Return m_razon
        End Get
        Set(ByVal value As Integer)
            m_razon = value
        End Set
    End Property
    Public Shared m_razon As Integer

    Public Property agencia() As Integer
        Get
            Return m_agencia
        End Get
        Set(ByVal value As Integer)
            m_agencia = value
        End Set
    End Property
    Public Shared m_agencia As Integer


    Public Property pathEncabezados() As String
        Get
            Return m_pathEncabezados
        End Get
        Set(ByVal value As String)
            m_pathEncabezados = value
        End Set
    End Property
    Public Shared m_pathEncabezados As String

    Public Property pathMovimientos() As String
        Get
            Return m_pathMovimientos
        End Get
        Set(ByVal value As String)
            m_pathMovimientos = value
        End Set
    End Property
    Public Shared m_pathMovimientos As String

    Public Property pathSaldos() As String
        Get
            Return m_pathSaldos
        End Get
        Set(ByVal value As String)
            m_pathSaldos = value
        End Set
    End Property
    Public Shared m_pathSaldos As String

    Public Property pathComprobantes() As String
        Get
            Return m_pathComprobantes
        End Get
        Set(ByVal value As String)
            m_pathComprobantes = value
        End Set
    End Property
    Public Shared m_pathComprobantes As String

    Public Property pathCheques() As String
        Get
            Return m_pathCheques
        End Get
        Set(ByVal value As String)
            m_pathCheques = value
        End Set
    End Property
    Public Shared m_pathCheques As String

    Public Property pathTransferencias() As String
        Get
            Return m_pathTransferencias
        End Get
        Set(ByVal value As String)
            m_pathTransferencias = value
        End Set
    End Property
    Public Shared m_pathTransferencias As String

    Public Property pathTarget() As String
        Get
            Return m_pathTarget
        End Get
        Set(ByVal value As String)
            m_pathTarget = value
        End Set
    End Property
    Public Shared m_pathTarget As String

    Public Property pathXML() As String
        Get
            Return m_pathXML
        End Get
        Set(ByVal value As String)
            m_pathXML = value
        End Set
    End Property
    Public Shared m_pathXML As String

    Public Property NumOrden() As String
        Get
            Return m_NumOrden
        End Get
        Set(ByVal value As String)
            m_NumOrden = value
        End Set
    End Property
    Public Shared m_NumOrden As String

    Public Property NumTramite() As String
        Get
            Return m_NumTramite
        End Get
        Set(ByVal value As String)
            m_NumTramite = value
        End Set
    End Property
    Public Shared m_NumTramite As String

    Public Property TipoSolicitud() As String
        Get
            Return m_TipoSolicitud
        End Get
        Set(ByVal value As String)
            m_TipoSolicitud = value
        End Set
    End Property
    Public Shared m_TipoSolicitud As String

    Public Shared pathFile As List(Of String) = New List(Of String)



End Class
