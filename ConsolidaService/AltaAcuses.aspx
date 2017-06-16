<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AltaAcuses.aspx.vb" Inherits="ConsolidaService.AltaAcuses" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"> 
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="js/jquery-1.11.0.js" type="text/javascript"></script>   
    <script src="js/bootstrap.min.js" type="text/javascript"></script> 
    <%--<script src="js/jasny-bootstrap.min.js" type="text/javascript"></script>  --%>
    <script src="js/bootstrap-select.js" type="text/javascript"></script>
    <script src="js/bootstrap-switch.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="js/highcharts.js" type="text/javascript"></script>
    <script src="js/themes/gray.js" type="text/javascript"></script>
    <script src="js/bootstrap-filestyle.min.js" type="text/javascript"></script>
    <%--<script src="js/bootstrap.file-input.js" type="text/javascript"></script>--%>
    <link href="css/bootstrap.min.css" rel="stylesheet"  media='screen,print' />   
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css" rel="stylesheet">
    <%--<link href="css/jasny-bootstrap.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/bootstrap-select.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-switch.min.css" rel="stylesheet"type="text/css" />
    <link href="css/datepicker.css" rel="stylesheet" type="text/css" />
    <link href="css/General.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.fancybox.css" rel="stylesheet" type="text/css" />    
    <script src="js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="js/jquery.fancybox.pack.js" type="text/javascript"></script>

    <script type="text/jscript">
       

        function ReAbrirAcuse() {
            alert('¡ Acuse registrado !');
            parent.OpenControl('ControlAcuses.aspx?idEmpresa=' + $("#idEmpresa").val() + '&Ejercicio=' + $("#Ejercicio").val() + '&Periodo=' + $("#Periodo").val() + '&Usuario=' + $("#Usuario").val());
        }

        $(document).ready(function () {
            $(":file").filestyle({ input: false, buttonText: "Cargar Archivo" });

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
                        case "pdf":
                        case "xml":
                        case "xls":
                        case "xlsx":                       
                            break;
                        default:
                            // Cancel the form submission
                            alert("¡ Formato " + extension + " no admitido !\r\nDebe de seleccionar un archivo XML,XLS,XLSX o PDF");
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


</head>
<body>
    <form id="form1" runat="server">
     
    <!--Aqui va la declaración del Ajax-->       
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>     
    <!-- Finaliza declaración del Ajax  -->
    <input type="hidden" id="idEmpresa" runat="server" value="" />
    <input type="hidden" id="RFC" runat="server" value="" />
    <input type="hidden" id="Fecha" runat="server" value="" />
    <input type="hidden" id="TipoReporte" runat="server" value="" />
    <input type="hidden" id="TipoEnvio" runat="server" value="" />
    <input type="hidden" id="Ejercicio" runat="server" value="" />
    <input type="hidden" id="Periodo" runat="server" value="" />
    <input type="hidden" id="Usuario" runat="server" value="" />
    <div class="span12"> 
    <div style="background-color: #E6E6E6">
       <input type="hidden" id="archivo" class="archivo" runat="server" value="" />
        <table border="0" width="100%" class="tabla-principal">  
            <tr>
                <td colspan="2" class="titulo"                 
                    style="background-color: #333333; color: #FFFFFF; font-family: Arial; font-weight: bold; font-size: 18px;">
                 <asp:Image ID="Image1" runat="server"
                ImageUrl="~/images/gov.gif" />CAPTURA DE ACUSES</td>
            </tr>
            
        </table>
        <table  border="0" width="100%">
                    <tr>
                    <td>
                    <ul>
                    <li>
                    Folio
                    <div>
                    <input id="txtFolio" class="txtFolio" runat="server" type="text" />
                    </div>
                    </li>
                    </ul>
                    </td>     
                    <td style="padding:10 0 10 0; margin:0 0 0 0">        
                    </br>
                    <input id="myfile" name="myFile" onclick="javascript:showFileName();" runat="server" type="file" class="filestyle" data-input="false" data-buttonText="Cargar Archivo">            
                    </td>
                    <td style="padding:10 0 10 0; margin:0 0 0 0">
                    </br>
                    <input type="submit"  value="Procesar Acuse" class="btn boton" />
                    </td>
                    </tr>      
                    </table>
    </div>
    </div>
    <div id="fHPol" style="padding-top:30px;"/>
    </form>
</body>
</html>
