<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ControlAcuses.aspx.vb" Inherits="ConsolidaService.ControlAcuses" %>

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

        function OpenAcuse(pagina) {

            $.fancybox({
                'scrolling': 'yes',
                'width': 600,
                'height': 400,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic',
                'titlePosition': 'inside',
                'type': 'iframe',
                'autoDimensions': 'true',
                'hideOnOverlayClick': 'false',
                'autoScale': 'true',
                'centerOnScroll': 'true',
                'titleShow': 'false',
                'href': pagina,
                'onClosed': function () {
                    document.getElementById("form1").submit();
                }
            });

        }

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
                ImageUrl="~/images/gov.gif" />CONTROL DE ACUSES</td>
            </tr>        
        </table>
    </div>
    <div>
        <asp:GridView ID="lstcatalogo" runat="server" 
            GridLines="None"
            AutoGenerateColumns="False" 
            AllowPaging="true"
            CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt">
        <Columns>
       <asp:BoundField SortExpression="Empresa" HeaderText="Empresa" ItemStyle-HorizontalAlign="Center" 
                DataField="Empresa">
       </asp:BoundField>
        <asp:BoundField SortExpression="TipoReporte" HeaderText="TipoReporte" ItemStyle-HorizontalAlign="Center" 
                DataField="TipoReporte">
       </asp:BoundField>
       <asp:BoundField SortExpression="TipoEnvio" HeaderText="TipoEnvio" ItemStyle-HorizontalAlign="Center" 
                DataField="TipoEnvio">
       </asp:BoundField>
       <asp:BoundField SortExpression="Ejercicio" HeaderText="Ejercicio" ItemStyle-HorizontalAlign="Center" 
                DataField="Ejercicio">
       </asp:BoundField>
       <asp:BoundField SortExpression="Periodo" HeaderText="Periodo" ItemStyle-HorizontalAlign="Center" 
                DataField="Periodo">
       </asp:BoundField>
        <asp:BoundField SortExpression="Fecha" HeaderText="Fecha" ItemStyle-HorizontalAlign="Center" 
                DataField="Fecha">
       </asp:BoundField>
       <asp:BoundField SortExpression="Usuario" HeaderText="Usuario" ItemStyle-HorizontalAlign="Center" 
                DataField="Usuario">
       </asp:BoundField>
       <asp:TemplateField HeaderText="Acuse" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                    <a id="btnEstado" href="#" onclick="javascript:parent.OpenAcuse('AltaAcuses.aspx?idEmpresa=<%#Eval("idEmpresa")%>&RFC=<%#Eval("RFC")%>&Fecha=<%#Eval("FechaReal")%>&TipoReporte=<%#Eval("TipoReporte")%>&TipoEnvio=<%#Eval("TipoEnvio")%>&Ejercicio=<%#Eval("Ejercicio")%>&Periodo=<%#Eval("Periodo")%>&Usuario=<%#Eval("Usuario")%>',<%#Eval("idEmpresa")%>,<%#Eval("Ejercicio")%>,<%#Eval("Periodo")%>,'<%#Eval("Usuario")%>');"><%#Eval("Acuse")%></a>                
            </ItemTemplate>             
         </asp:TemplateField>   
     </Columns>

        </asp:GridView>    
    </div>
    <div id="fHPol" style="padding-top:30px;"/>
    </form>
</body>
</html>
