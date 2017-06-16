'Diego Alberto Nieto Flores
'Creado el 21 de Septiembre de 2011
'Objetivo: Clase para la internoexion a la base de datos de sql server, contiene metodos para ejecutar consultas, regresandonos diferentes tipos de objetos con la informacion

Imports System.Data.SqlClient
Imports System.Web

Public Class Persistencia
    Public Shared strCon As String = GlobalVariables.m_SC
    Dim myCookie As HttpCookie
    'OBJETIVO: Generar una consulta a la base de datos y obtener sus datos en un DataTable
    'ENTRADAS: Una cadena con la consulta deseada
    'SALIDAS: Un Datatable con los datos de la consulta
    Public Shared Function GetDataTable(ByVal Query As String, Optional ByVal conexion As String = "") As DataTable

        Dim dtSalida = New DataTable()
        'Generamos la conexion a la base de datos
        Using conn As SqlConnection = New SqlConnection(strCon)
            Try
                conn.Open()
                'Generasmos la consulta que se envia como parametro con un DataAdapter
                Dim da As SqlDataAdapter = New SqlDataAdapter(" SET LANGUAGE ENGLISH " + Query, strCon)
                da.SelectCommand.CommandTimeout = 100000
                da.Fill(dtSalida)
            Catch ex As Exception
                Console.WriteLine(ex.Message)

            End Try
        End Using

        Return dtSalida

    End Function

    'Cadena de conexión
    Public Shared Function ConsultaSQL(ByRef Qry As String) As DataTable

        Dim dtSalida = New DataTable()
        Try
            Using cn As SqlConnection = New SqlConnection(strCon)
                cn.Open()
                Using cmd As SqlCommand = New SqlCommand()
                    cmd.Connection = cn
                    cmd.CommandText = Qry
                    cmd.CommandTimeout = 100000
                    Using myReader As SqlDataReader = cmd.ExecuteReader()
                        Try
                            dtSalida.Load(myReader)
                            Return dtSalida
                        Catch ex As Exception
                            Return Nothing
                        End Try

                    End Using
                End Using
                cn.Close()
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Return dtSalida

    End Function

    Public Shared Function EjecutarSQL(ByVal Query As String, Optional ByVal conexion As String = "") As Integer
        Dim sqlCmd As SqlCommand
        Dim filas As Integer = 0

        Using conn As SqlConnection = New SqlConnection(strCon)
            Try
                conn.Open()

                'Generamos el comando que se envia como parametro 
                sqlCmd = New SqlCommand(" SET LANGUAGE ENGLISH " + Query, conn)
                sqlCmd.CommandTimeout = 500000
                'Obtenemos el número de filas afectadas por el comando
                filas = sqlCmd.ExecuteNonQuery()
            Catch ex As Exception

                Console.WriteLine(ex.Message)
            End Try

        End Using

        Return filas

    End Function

    Public Function GetstrCon() As String
        Return strCon
    End Function

End Class
