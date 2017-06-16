<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Catalogo.aspx.vb" Inherits="ConsolidaService.Catalogo1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-ui-1.9.2.custom/js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.9.2.custom/js/jquery-ui-1.9.2.custom.js" type="text/javascript"></script>    
    <script src="js/cm_jscom_valida-campos/jquery.validationEngine-es.js" type="text/javascript"></script>    
    <script src="js/cm_jscom_autocomplete/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="js/cm_jscom_valida-fechahora/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="js/cm_jscom_valida-fechahora/jquery.ui.timepicker.js" type="text/javascript"></script>
    

    <link href="css/General.css" rel="stylesheet" type="text/css" />
    <link href="js/jquery-ui-1.9.2.custom/css/smoothness/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="js/cm_jscom_autocomplete/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="css/cm_css_valida-campos/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
    <link href="css/cm_css_valida-fechahora/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    <link href="css/cm_css_valida-fechahora/jquery-ui-timepicker.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.fancybox.css" rel="stylesheet" type="text/css" />    
    <script src="js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="js/jquery.fancybox.pack.js" type="text/javascript"></script>


    <style type="text/css">

        .ui-dialog 
        {
            font-family: Trebuchet MS;
            background-color: #D8D8D8;
            border: 2px solid;
            padding: 5px;
        }
       
        .ui-dialog-titlebar
        {
            font-size: large;
            font-weight: bold;
        }
       
        .ui-dialog-titlebar-close {
            visibility: hidden;
        }
        
    </style>

     <script type="text/jscript">

         var ll_usuario = "<%= sUsuario %>";
         var ll_RFC = "<%= sRFC %>";
         var ll_razon = "<%= iRazon %>";
         var tipoXML = ""
         var ArchivoXML = ""

         function OpenConsolidar(tipo) {
             $.fancybox({
                 'scrolling': 'yes',
                 'width': 700,
                 'height': 200,
                 'transitionIn': 'fade',
                 'transitionOut': 'fade',
                 'titlePosition': 'inside',
                 'type': 'iframe',
                 'href': "ProgressBar.aspx?descripcion=" + tipo + "&ejercicio=0&periodo=0&estado=REPORTADO&razon=" + $("#Razon").val() + "&agencia=0&usuario=" + ll_usuario + "&archivo=" + tipo,
                 'onClosed': function () {
                     document.getElementById("form1").submit();
                 }
             });

         }

         $(document).ready(function () {

             ll_usuario = "<%= sUsuario %>";
             ll_RFC = "<%= sRFC %>";
             ll_razon = "<%= iRazon %>";

             $('.clave').autocomplete('ashx/CatCuentasDesN3.ashx', {
                 width: 420,
                 max: 10,
                 cacheLength: 1,
                 highlight: false,
                 scroll: true,
                 scrollHeight: 300,
                 formatItem: function (data, i, n, value) {
                     var id = data[0];
                     var Clave1 = data[1];
                     var nombre = data[2];
                     var padre = data[3];
                     try {
                         return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Document_New.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Origen: <label title='" + padre + "'>" + padre.substring(0, 30) + "..." + "<br/>Clave1: " + Clave1 + "<br/>Nombre: <label title='" + nombre + "'>" + nombre.substring(0, 30) + "..." + "</label></p></td></tr></table>";
                     } finally {
                         id = null;
                         Clave1 = null;
                         nombre = null;
                         padre = null;
                     }
                 },
                 formatResult: function (data, value) {
                     return data[1];
                 }
             });

             $('.clave').result(function (event, data, formatted) {
                 if (data) {

                     // Extract the data values
                     var id = data[0];
                     var Clave1 = data[1];
                     var nombre = data[2];


                     $('.Clave').val(Clave1);
                     id = null;
                     Clave1 = null;
                     nombre = null;

                 }
             });

         });
         
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="background-color: #E6E6E6">
    <input type="hidden" id="archivo" class="archivo" runat="server" value="" />
    <table border="0" width="100%" class="tabla-principal">  
        <tr>
            <td colspan="2" class="titulo"                 
                style="background-color: #333333; color: #FFFFFF; font-family: Arial; font-weight: bold; font-size: 18px;">
             <asp:Image ID="Image1" runat="server"
            ImageUrl="~/images/gov.gif" />CATÁLOGO DE CUENTAS</td>
        </tr>        
    </table>
    <table border="0" width="99%">
    <tr>
    <td>
    <ul>
    <li>
        RAZON SOCIAL
    <div>         
        <asp:DropDownList ID="Razon" runat="server">
        </asp:DropDownList>
        
    </div>
    </li>         
    </ul>
   </td>
    <td>
    <ul>
    <li>
    CUENTA
    <div>
    <input id="clave" class="clave" runat="server" type="text" style="width:320px" />
    </div>
    </li>
    </ul>
    </td>
    <td>
    <ul>
    <li>
    REGISTROS
    <div>         
        &nbsp;<asp:DropDownList ID="lstregistros" runat="server" Width="100px">            
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="20">20</asp:ListItem>
            <asp:ListItem Value="50">50</asp:ListItem>
            <asp:ListItem Value="100">100</asp:ListItem>            
        </asp:DropDownList>
    </div>
    </li>         
    </ul>
    </td>     

    <td>
    <ul>
    <li>
    Nivel
    <div>         
        &nbsp;<asp:DropDownList ID="Nivel" runat="server" Width="100px">
            <asp:ListItem Value="" Selected="True">-- TODAS --</asp:ListItem>
            <asp:ListItem Value="1">NIVEL 1</asp:ListItem>
            <asp:ListItem Value="2">NIVEL 2</asp:ListItem>
            <asp:ListItem Value="3">NIVEL 3</asp:ListItem>
            <asp:ListItem Value="4">NIVEL 4</asp:ListItem>
            <asp:ListItem Value="5">NIVEL 5</asp:ListItem>
            <asp:ListItem Value="6">NIVEL 6</asp:ListItem>                        
        </asp:DropDownList>
    </div>
    </li>         
    </ul>
    </td>     
    <td>    
    </br>
    <asp:Button ID="btnbuscar" runat="server" Text="BUSCAR" CssClass="btn boton" />            
    </td>   
     <td>    
    </br>
    <%--<asp:Button ID="expXML" runat="server" Text="GENERAR XML" CssClass="btn boton" /> --%>   
    <input type ="button" id="exp" runat="server" class="boton"  value="GENERAR XML" onclick="javascript:OpenConsolidar('CATALOGO');" />      
    </td>   
    </tr>
    </table>
    </div>
    <div>
    <asp:GridView ID="lstcatalogo" runat="server" 
    GridLines="None"
    AllowPaging="true"
    CssClass="mGrid"
    PagerStyle-CssClass="pgr"
    AlternatingRowStyle-CssClass="alt">
    </asp:GridView>    
</div>
    </form>
</body>
</html>
