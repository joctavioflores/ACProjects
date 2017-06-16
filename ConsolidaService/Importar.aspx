<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Importar.aspx.vb" Inherits="ConsolidaService.Importar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 
<script type="text/jscript">
    
    var ll_usuario = "<%= sUsuario %>";

    function OpenAcuse(pagina,idRazon,Ejercicio,Periodo,Usuario) {

        $.fancybox({
            'scrolling': 'no',
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
              
            }
        });

    }

    function OpenConsolidar(archivo) {
                   
        $.fancybox({
            'scrolling': 'yes',
            'width': 700,
            'height': 200,
            'transitionIn': 'fade',
            'transitionOut': 'fade',
            'titlePosition': 'inside',
            'type': 'iframe',
            'href': "ProgressBar.aspx?descripcion=IMPORTAR&ejercicio=0&periodo=0&estado=REPORTADO&razon=0&agencia=0&usuario=" + ll_usuario + "&archivo=" + archivo,
            'onClosed': function () {
                document.getElementById("form1").submit();
            }
        });
    }


    function OpenControl(pagina) {
        
        $.fancybox({
            'scrolling': 'yes',
            'width': 1020,
            'height': 800,
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

        $("#form1").submit(function (submitEvent) {

            //alert("aqui estoy!!");

            var valor = $(".archivo").val()

            // get the file name, possibly with path (depends on browser)
            var filename = $("#<%= myfile.ClientID %>").val();



            // Use a regular expression to trim everything before final dot
            var extension = filename.replace(/^.*\./, '');

            // Iff there is no dot anywhere in filename, we would have extension == filename,
            // so we account for this possibility now
            if (extension == filename) {
                extension = '';
            } else {
                // if there is an extension, we convert to lower case
                // (N.B. this conversion will not effect the value of the extension
                // on the file upload.)
                extension = extension.toLowerCase();
            }

            if (valor != "") {
                switch (extension) {
                    case "xml":
                    case "xls":
                    case "xlsx":                        
                        break;
                    default:
                        // Cancel the form submission
                        alert("¡ Formato " + extension + " no admitido !\r\nDebe de seleccionar un archivo XML,XLS O XLSX");
                        submitEvent.preventDefault();
                        break;
                }
            }

        });

        $("input:file").change(function () {
            var filePath = $(this).val();
            $(".archivo").val(filePath);

        });
    });
   
    function showFileName() {
        var fil = document.getElementById("myFile");
        alert(fil.value);
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="background-color: #E6E6E6">
    <input type="hidden" id="archivo" class="archivo" runat="server" value="" />
    <table border="0" width="100%" class="tabla-principal">  
        <tr>
            <td colspan="2" class="titulo"                 
                style="background-color: #333333; color: #FFFFFF; font-family: Arial; font-weight: bold; font-size: 18px;">
             <asp:Image ID="Image1" runat="server"
            ImageUrl="~/images/gov.gif" /><asp:label ID="lbImp" runat="server">IMPORTAR ARCHIVOS</asp:label></td>
        </tr>        
    </table>
    <table border="0" width="99%">
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
     PERIODO
    <div>
    <asp:DropDownList ID="DDLPERIODO" runat="server" AutoPostBack="true">
    </asp:DropDownList>
    </div>
    </li>
    </ul>
    </td>
    <td>
    <ul>
    <li>
     EJERCICIO
    <div>
    <asp:DropDownList ID="DDLEJERCICIO" runat="server" AutoPostBack="true">
    </asp:DropDownList>
    </div>
    </li>
    </ul>
    </td>
    <td style="padding:20 0 20 0; margin:0 0 0 0">    
    </br>
    <asp:Button ID="btnbuscar" runat="server" Text="BUSCAR" CssClass="btn boton" />            
    </td>
    <td style="padding:20 0 20 0; margin:0 0 0 0">        
    </br>
    <input id="myfile" name="myFile" onclick="javascript:showFileName();" runat="server" type="file" class="filestyle" data-buttonText="Cargar Archivo">            
    </td>
    <td style="padding:10 0 10 0; margin:0 0 0 0">
    </br>
    <input type="submit"  value="Procesar Archivo" class="btn boton" />
    </td>
    </tr>
    </table>
    </div>
    <div>
    <asp:GridView ID="consolidacion" runat="server" 
    GridLines="None"
    AutoGenerateColumns="False" 
    AllowPaging="true"
    CssClass="mGrid"
    PagerStyle-CssClass="pgr"
    AlternatingRowStyle-CssClass="alt">
     <Columns>
       <asp:BoundField SortExpression="DISTRIBUIDOR" HeaderText="DISTRIBUIDOR" ItemStyle-HorizontalAlign="Center" 
                DataField="DISTRIBUIDOR">
       </asp:BoundField>
        <asp:BoundField SortExpression="PERIODO" HeaderText="PERIODO" ItemStyle-HorizontalAlign="Center" 
                DataField="PERIODO">
       </asp:BoundField>
       <asp:BoundField SortExpression="EJERCICIO" HeaderText="EJERCICIO	" ItemStyle-HorizontalAlign="Center" 
                DataField="EJERCICIO">
       </asp:BoundField>
       <asp:BoundField SortExpression="FECHAINI" HeaderText="FECHAINI" ItemStyle-HorizontalAlign="Center" 
                DataField="FECHAINI">
       </asp:BoundField>
       <asp:BoundField SortExpression="FECHAFIN" HeaderText="FECHAFIN" ItemStyle-HorizontalAlign="Center" 
                DataField="FECHAFIN">
       </asp:BoundField>
       <asp:TemplateField HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                    <a id="btnEstado" href="#" onclick="javascript:OpenControl('ControlAcuses.aspx?idEmpresa=<%#Eval("idrazon")%>&idDistribuidor=<%#Eval("iddistribuidor")%>&Ejercicio=<%#Eval("idEjercicio")%>&Periodo=<%#Eval("idPeriodo")%>&Usuario=<%#Eval("USUARIO")%>');"><%#Eval("ESTADO")%></a>                
            </ItemTemplate>             
         </asp:TemplateField>   
       <asp:BoundField SortExpression="USUARIO" HeaderText="USUARIO" ItemStyle-HorizontalAlign="Center" 
                DataField="USUARIO">
       </asp:BoundField>
     </Columns>
    <%-- COLUMNAS --%>
    <%-- FIN COLUMNAS --%>
    </asp:GridView>    
    </div>
</asp:Content>
