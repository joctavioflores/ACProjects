<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteCargaEat.aspx.vb" Inherits="ConsolidaService.ReporteCargaEat" %>

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

        function BindGrid() {

            document.getElementById('<%=btnBuscar.ClientID%>').click();
        }

        function CompruebaEstado(Distribuidor, FechaTraslado, RutaServerWeb, ScriptFile) {
            var valor;
            
//            alert("Información para replicar:\n\r" + Distribuidor + "\n\r" + FechaTraslado + "\n\r" + RutaServerWeb + "\n\r" + ScriptFile);
//            alert("Llamando al santo desde:\n\r" + RutaServerWeb + "/GoVirtualMCo/WsVDealer/WsCargarDatos.asmx/EjecutaSQLEAT")
//            
            //$.support.cors = true;
            
            $.ajax({
                type: "POST",
                url: "ReporteCargaEat.aspx/EnviaPeticion",
                data: "{'empSex':'" + Distribuidor + "|" + FechaTraslado + "|" + RutaServerWeb + "|" + ScriptFile + " '}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    // Do magic here.
                    valor = msg.d;
                }

            });
            
            alert(valor);

//            if (valor == "SI") {
//                alert("¡ Comprobante eliminado !")
                BindGrid();
//            }
        
        
        }

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


        $(document).ready(function () {

            $("#<%= fechaini.ClientID %>").datepicker({
                dateFormat: 'dd/mm/yy',
                showOn: "focus",
                beforeShow: function (input, inst) {
                    inst.dpDiv.css({ marginTop: -(input.offsetHeight - 30) + 'px', marginRight: input.offsetWidth + 'px' });
                }

            });


            $("#<%= fechafin.ClientID %>").datepicker({
                dateFormat: 'dd/mm/yy',
                showOn: "focus",
                beforeShow: function (input, inst) {
                    inst.dpDiv.css({ marginTop: -(input.offsetHeight - 30) + 'px', marginRight: input.offsetWidth + 'px' });
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
                ImageUrl="~/images/gov.gif" />REPORTE CARGA EAT</td>
            </tr>        
        </table>
        </div>
        <table>
        <tr>
<td>
    <ul>
    <li>
        Razon Social
    <div>         
        <asp:DropDownList ID="Razon" runat="server" AutoPostBack="true">
        </asp:DropDownList>
   
    </div>
    </li>         
    </ul>
</td>
<td>
    <ul>
    <li>
        Agencia
    <div>         
        <asp:DropDownList ID="Agencia" runat="server" Enabled = "false" AutoPostBack = "true" >
            <asp:ListItem Text = "--SELECT AGENCIA--" Value = "0"></asp:ListItem>
        </asp:DropDownList>
        
    </div>
    </li>         
    </ul>
</td>
<td>
    <ul>
    <li>
        ESTADO CARGA
    <div>         
        <asp:DropDownList ID="Estado" runat="server" AutoPostBack = "true" >           
        </asp:DropDownList>
        
    </div>
    </li>         
    </ul>
</td>
<td id="feini" runat="server" class="feini">
    <ul>
    <li>
    Fecha Inicial
    <div>
        <asp:TextBox id="fechaini" runat="server" Width="100" 
            style="text-align: center"></asp:TextBox>       
    </div>
    </li>
    </ul>
    </td>
   <td id="fefin" runat="server" class="fefin">
    <ul>
        <li>
        Fecha Final
        <div>
            <asp:TextBox id="fechafin" runat="server" Width="100" 
                style="text-align: center"></asp:TextBox>       
        </div>
        </li>           
        </ul>
    </td>
    <td>
    <br/>
    <asp:Button id="btnBuscar" Text="Buscar" runat="server" CssClass="boton" 
            onclick="btnBuscar_Click"/>
    </td>
        </table>
    
    <div>
        <asp:GridView ID="lstCargaEat" runat="server" 
            GridLines="None"
            AutoGenerateColumns="False" 
            AllowPaging="true"
            CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt">
        <Columns>
       <asp:BoundField SortExpression="distribuidor" HeaderText="Distribuidor" ItemStyle-HorizontalAlign="Center" 
                DataField="distribuidor">
       </asp:BoundField>
        <asp:BoundField SortExpression="fechatraslado" HeaderText="Fecha Traslado" ItemStyle-HorizontalAlign="Center" 
                DataField="fechatraslado">
       </asp:BoundField>
       <asp:BoundField SortExpression="unidades" HeaderText="Unidades" ItemStyle-HorizontalAlign="Center" 
                DataField="unidades">
       </asp:BoundField>
       <asp:BoundField SortExpression="cargadas" HeaderText="Cargadas" ItemStyle-HorizontalAlign="Center" 
                DataField="cargadas">
       </asp:BoundField>
       <asp:BoundField SortExpression="sincargar" HeaderText="Sin Cargar" ItemStyle-HorizontalAlign="Center" 
                DataField="sincargar">
       </asp:BoundField>
        <asp:BoundField SortExpression="carga" HeaderText="Carga" ItemStyle-HorizontalAlign="Center" 
                DataField="carga">
       </asp:BoundField>
     <%--  <asp:BoundField SortExpression="cargaremota" HeaderText="Carga Remota" ItemStyle-HorizontalAlign="Center" 
                DataField="cargaremota">
       </asp:BoundField>--%>
       <asp:TemplateField HeaderText="Carga Remota" SortExpression="cargaremota">
        <ItemStyle HorizontalAlign="Center" />
        <ItemTemplate>
            <a id="a2" href="#" onclick="javascript:CompruebaEstado(<%#Eval("idDistribuidor")%>,'<%#Eval("fechatraslado")%>','<%#Eval("RutaServerWeb")%>','<%#Eval("Archivo")%>')"><%#Eval("cargaremota") %></a>        
            <input type="hidden" runat="server" id="lbDistribuidor" class="lbDistribuidor" value='<%#Eval("idDistribuidor") %>' /> 
            <input type="hidden" runat="server" id="lbRutaServerWeb" class="lbRutaServerWeb" value='<%#Eval("RutaServerWeb") %>' /> 
            <input type="hidden" runat="server" id="lbcargaremota" class="lbcargaremota" value='<%#Eval("Archivo") %>' /> 
        </ItemTemplate>
     </asp:TemplateField>
       <asp:BoundField SortExpression="usuario" HeaderText="Usuario" ItemStyle-HorizontalAlign="Center" 
                DataField="usuario">
       </asp:BoundField>
        <asp:BoundField SortExpression="fecharegistro" HeaderText="Fecha Registro" ItemStyle-HorizontalAlign="Center" 
                DataField="fecharegistro">
       </asp:BoundField>
     </Columns>
       <EmptyDataTemplate>                
 
            <div style='display:block; text-align:center; font-size:12px'><img style='padding-left:5px; padding-right:5px' src='images/document_warning.png' alt='' width='125' height='125'/><h1>NO EXISTEN ELEMENTOS</h1></div>    
        </EmptyDataTemplate>
        </asp:GridView>    
    </div>
    <div id="fHPol" style="padding-top:30px;"/>
    </form>
</body>
</html>
