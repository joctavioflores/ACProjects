Imports System.Xml.Serialization
Imports System.IO

Public Class Catalogo1
    Inherits System.Web.UI.Page
    Dim cat As Catalogo = New Catalogo()
    Dim SQLTable As DataTable
    Dim SQLCuenta As DataTable
    Dim SQLSubCuenta As DataTable
    Dim myCookie As HttpCookie
    Public iRazon As Integer
    Public iAgencia As Integer
    Public sUsuario As String = ""
    Public sRFC As String = ""
    Dim sqlCmd As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        myCookie = HttpContext.Current.Request.Cookies("UserSettings")
        If Not IsNothing(Request.QueryString("sUsuario")) Then
            sUsuario = Request.QueryString("sUsuario")
        ElseIf Not IsNothing(myCookie) Then
            sUsuario = myCookie("Usuario")
        End If
        If myCookie("idRol") <> 4 Then
            iRazon = myCookie("idRol")
        Else
            iRazon = 1
        End If


        If Not IsPostBack() Then

            SQLTable = Persistencia.GetDataTable("select ISNULL(RFC,'XAXX010101000') from tbco_config")
            If SQLTable.Rows.Count > 0 Then
                sRFC = CInt(SQLTable.Rows(0).Item("RFC"))
            Else
                sRFC = "XAXX010101000"
            End If

            If Razon.Items.Count = 0 Then

                Dim donde As String = ""

                Select Case myCookie("idRol")
                    Case "1"
                        donde = "where idrazon = 1"
                    Case "2"
                        donde = "where idrazon = 2"
                    Case "3"
                        donde = "where idrazon = 3"
                End Select


                Razon.DataSource = Persistencia.GetDataTable("select * from tbco_razonsocial " + donde)
                Razon.DataTextField = "razon"
                Razon.DataValueField = "idrazon"
                Razon.DataBind()

                ' insert an item at the beginning of the list
                '----------------------------------------------------
                Razon.Items.Insert(0, New ListItem("-- Todas --", "0"))

                If Razon.Items.Count > 0 Then Razon.SelectedValue = Razon.Items.FindByValue(iRazon).Value
            End If

        End If


        Buscar()
    End Sub

    Public Sub Buscar()


        sqlCmd = " select CLAVE, RTRIM(CodigoSat) AS CGROUP , NOMBRE AS CUENTA " &
                 " , NIVEL, CASE WHEN NATURALEZA = 'D' THEN 'DEUDORA' ELSE 'ACREEDORA' END AS NATURALEZA " &
                 " , CASE WHEN EF = 'BG' THEN 'BALANCE' ELSE 'RESULTADOS' END AS TIPO  " &
                 " from tbco_catalogo  " &
                 " where nivel <= 6  " &
                 " union  " &
                 " select CLAVE, RTRIM(CodigoSat) AS CGROUP , NOMBRE + ' ' + CAST(S.Distribuidor AS VARCHAR)   + ' ' +  S.Descripcion AS CUENTA  " &
                 " , NIVEL, CASE WHEN NATURALEZA = 'D' THEN 'DEUDORA' ELSE 'ACREEDORA' END AS NATURALEZA " &
                 " , CASE WHEN EF = 'BG' THEN 'BALANCE' ELSE 'RESULTADOS' END AS TIPO  " &
                 " from tbco_catalogo c " &
                 " inner join tbcm_SucursalCon s on c.idSucursal = s.idSucursalCm " &
                 " where cast(c.nivel as varchar(2)) like '%" + Nivel.SelectedValue.ToString + "%'  and c.clave like '%" + clave.Value + "%' "

        If Razon.SelectedValue > 0 Then

            sqlCmd = sqlCmd + " And c.idEmpresa = " + Razon.SelectedValue.ToString

        End If


        sqlCmd = sqlCmd + " ORDER BY CLAVE "

        SQLTable = Persistencia.GetDataTable(sqlCmd)

        lstcatalogo.DataSource = SQLTable
        lstcatalogo.DataBind()

        If lstcatalogo.Rows.Count > 0 Then
            lstcatalogo.PageSize = lstregistros.SelectedValue
        End If

    End Sub

    Protected Sub lstcatalogo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles lstcatalogo.PageIndexChanging
        lstcatalogo.PageIndex = e.NewPageIndex
        lstcatalogo.DataBind()
    End Sub

    Protected Sub btnbuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnbuscar.Click
        Buscar()
    End Sub

End Class