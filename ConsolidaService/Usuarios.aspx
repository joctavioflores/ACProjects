<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Usuarios.aspx.vb" Inherits="ConsolidaService.Usuarios" %>

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
    <script src="js/jquery.fancybox-1.3.4.js" type="text/javascript"></script>
    <script src="js/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>

    <link href="css/General.css" rel="stylesheet" type="text/css" />
    <link href="js/jquery-ui-1.9.2.custom/css/smoothness/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="js/cm_jscom_autocomplete/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="css/cm_css_valida-campos/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
    <link href="css/cm_css_valida-fechahora/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />
    <link href="css/cm_css_valida-fechahora/jquery-ui-timepicker.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />

     <script type="text/jscript">



         function BajaUsuario(idUsuario) {
            
             $.ajax({
                 type: "POST",
                 url: "ws/ValidaUsuario.asmx/Baja",
                 data: "{'empSex':'" + idUsuario + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (msg) {
                     // Do magic here.
                     valor = msg.d;
                     alert("¡El registro fue eliminado!");
                     
                 }
             });
             document.getElementById("form1").submit();
         }


         function OpenAltaUsuario(idUsuario) {

             $.fancybox({
                 'scrolling': 'yes',
                 'width': 500,
                 'height': 250,
                 'transitionIn': 'fade',
                 'transitionOut': 'fade',
                 'titlePosition': 'inside',
                 'type': 'iframe',
                 'href': "AltaUsuario.aspx?idUsuario=" + idUsuario,
                 'onClosed': function () {
                     document.getElementById("form1").submit();
                 }
             });

         }

             

         $(document).ready(function () {


             $('.Usuario').autocomplete('ashx/CatUsuarios.ashx', {
                 width: 420,
                 max: 10,
                 cacheLength: 1,
                 highlight: false,
                 scroll: true,
                 scrollHeight: 300,
                 formatItem: function (data, i, n, value) {
                     var id = data[0];
                     var sUsuario = data[1];
                     var sNick = data[2];
                     var srol = data[3];
                    
                     try {
                         return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Document_New.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Perfil: <label title='" + srol + "'>" + srol.substring(0, 30) + "..." + "<br/>Usuario: " + sUsuario + "<br/>Nick: <label title='" + sNick + "'>" + sNick.substring(0, 30) + "..." + "</label></p></td></tr></table>";
                     } finally {
                         id = null;
                         sUsuario = null;
                         sNick = null;
                         srol = null

                     }
                 },
                 formatResult: function (data, value) {
                     return data[1];
                 }
             });

             $('.Usuario').result(function (event, data, formatted) {
                 if (data) {

                     // Extract the data values
                     var id = data[0];
                     var sUsuario = data[1];
                     var sNick = data[2];
                     var srol = data[3];

                     $('.Usuario').val(sUsuario);
                     $('.Nick').val(sNick);
                     $('.idUsuario').val(id);
                     id = null;
                     sUsuario = null;
                     sNick = null;
                     srol = null

                 }
             });

             $('.Nick').autocomplete('ashx/CatUsuarios.ashx', {
                 width: 420,
                 max: 10,
                 cacheLength: 1,
                 highlight: false,
                 scroll: true,
                 scrollHeight: 300,
                 formatItem: function (data, i, n, value) {
                     var id = data[0];
                     var sUsuario = data[1];
                     var sNick = data[2];
                     var srol = data[3];

                     try {
                         return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Document_New.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Perfil: <label title='" + srol + "'>" + srol.substring(0, 30) + "..." + "<br/>Usuario: " + sUsuario + "<br/>Nick: <label title='" + sNick + "'>" + sNick.substring(0, 30) + "..." + "</label></p></td></tr></table>";
                     } finally {
                         id = null;
                         sUsuario = null;
                         sNick = null;
                         srol = null

                     }
                 },
                 formatResult: function (data, value) {
                     return data[1];
                 }
             });

             $('.Nick').result(function (event, data, formatted) {
                 if (data) {

                     // Extract the data values
                     var id = data[0];
                     var sUsuario = data[1];
                     var sNick = data[2];
                     var srol = data[3];

                     $('.Usuario').val(sUsuario);
                     $('.Nick').val(sNick);
                     $('.idUsuario').val(id);
                     id = null;
                     sUsuario = null;
                     sNick = null;
                     srol = null

                 }
             });

         });
         
     </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table border="0" width="99%">
    <tr>    
    <td>
    <ul>
    <li>
    USUARIO
    <div>
    <input id="Usuario" class="Usuario" runat="server" type="text" style="width:320px" />
    <input id="idUsuario" class="idUsuario" runat="server" type="hidden"/>
    </div>
    </li>
    </ul>
    </td>
    <td>
    <ul>
    <li>
    NICK
    <div>         
    <input id="Nick" class="Nick" runat="server" type="text" style="width:320px" />
    </div>
    </li>         
    </ul>
    </td>        
    <td>    
    </br>
    <input type="button" id="btnAlta" value="ALTA" class="boton" onclick="javascript:OpenAltaUsuario(0);" />
    <asp:Button ID="btnbuscar" runat="server" Text="BUSCAR" CssClass="btn boton" />            
    </td>        
    </tr>
    </table>
    </div>
    <div>
    <asp:GridView ID="lstusuarios" runat="server" 
    GridLines="None"
    AllowPaging="true"
    CssClass="mGrid"
    PagerStyle-CssClass="pgr"
    AutoGenerateColumns="False"
    AlternatingRowStyle-CssClass="alt">
    <Columns> 
            <asp:TemplateField ItemStyle-Width="40">
                <ItemTemplate> 
                   <a id="a1" class="editlink" href="#" onclick="javascript:OpenAltaUsuario('<%#Eval("idusuario")%>')"><asp:Image ID="Image1" runat="server" ImageUrl="images/EDIT.GIF" Height="25" Width="25" BorderStyle="None"></asp:Image></a>                                  
                </ItemTemplate>
                <ItemStyle Width="40px" />
            </asp:TemplateField> 
            <asp:TemplateField ItemStyle-Width="40">
                <ItemTemplate> 
                   <a id="a1" class="editlink" href="#" onclick="javascript:BajaUsuario('<%#Eval("idusuario")%>')"><asp:Image ID="Image1" runat="server" ImageUrl="images/DEL2.GIF" Height="25" Width="25" BorderStyle="None"></asp:Image></a>                                  
                </ItemTemplate>
                <ItemStyle Width="40px" />
            </asp:TemplateField>    
          <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                SortExpression="Nombre" ItemStyle-Width="300" >
                 </asp:BoundField>
          <asp:BoundField DataField="NICK" HeaderText="NICK" 
                SortExpression="NICK" ItemStyle-Width="300" >
                 </asp:BoundField>
          <asp:BoundField DataField="Rol" HeaderText="Rol" 
                SortExpression="Rol" ItemStyle-Width="300" >
                 </asp:BoundField>
    </Columns>
    </asp:GridView>    
    </div>
    </form>
</body>
</html>
