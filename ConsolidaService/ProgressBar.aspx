<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProgressBar.aspx.vb" Inherits="ConsolidaService.ProgressBar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>    

    <link href="css/General.css" rel="stylesheet" type="text/css" />    
    <link href="css/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/Main.js" type="text/javascript"></script>
    <script src="js/jquery.fancybox-1.3.4.js" type="text/javascript"></script>
    
</head>
<body>
    
    <form id="form1" runat="server">
    <asp:ScriptManager 
        EnablePageMethods="true" 
        ID="MainSM" 
        runat="server" 
        ScriptMode="Release"
        LoadScriptsBeforeUI="true">
        <Scripts>            
            <asp:ScriptReference Path="js/Main.js" />
        </Scripts>
    </asp:ScriptManager>      
    <script type="text/javascript">
  
      var des = "<%= sDescripcion %>";


      function SelectEjercicio() {

          if (des == 'DIOT') {

              var data = $("#process").val();
              var arr = data.split('|');

              arr[1] = $("#Ejercicio").val();

              $("#process").val(arr.join('|'));
          }
      }

      function SelectPeriodo() {

          if (des == 'DIOT') {

              var data = $("#process").val();
              var arr = data.split('|');

              arr[2] = $("#Periodo").val();

              $("#process").val(arr.join('|'));

          }
      }

      function SelectOperacion() {

          if (des == 'POLIZAS') {

              var data = $("#process").val();
              var arr = data.split('|');

              arr[10] = $("#TipoOperacion").val();
              arr[11] = $("#txtReferencia").val();


              $("#process").val(arr.join('|'));

              //alert($("#process").val());

          }
      }


      function AbrirReporte() {

          //alert("Abre reporte...");
          parent.openReporte();    
      
      }

      $(document).ready(function () {          

          //You don't need '' and when binding function, bind the function name not a string
          $("#txtReferencia").keyup(function () {

              var data = $("#process").val();
              var arr = data.split('|');

              arr[10] = $("#TipoOperacion").val();
              arr[11] = $("#txtReferencia").val();


              $("#process").val(arr.join('|'));

              //alert($("#process").val());

          });
      });   


      function Ejecuta(valor) {
          mainScreen.ExecuteCommand('LaunchNewProcess', 'mHandlers.Void', valor);
      }

      Sys.Application.add_load(
            applicationLoadHandler
            );
      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
            endRequestHandler
            );
      Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(
            beginRequestHandler
            );
      var mHandlers = {};
      mHandlers.Void = function (obj) {
          // nothing to process
          $('#consolidar').attr('disabled', 'disabled');

      };
      mHandlers.GetStatuses = function () {


          // call server method
          // which gets an array
          // of currently executing processes
          mainScreen.ExecuteCommand(
                'GetProcessStatuses',
                'mHandlers.ProcessStatuses',
                null);
          setTimeout(
                "mHandlers.GetStatuses();",
                parseInt(
                    500//$get("lblSlider.ClientID").innerHTML
                    )
                );
      };
      mHandlers.ProcessStatuses = function (obj) {

          var resultDiv = $get("resultDiv");
          if (obj) {
              resultDiv.innerHTML =
                    mHandlers.BuildProcessList(obj);
          } else {
              resultDiv.innerHTML = "&nbsp;";
          }
      };
      mHandlers.BuildProcessList = function (obj) {
          var i = 0;
          var l = 0;
          var count = 0;

          if (obj[i].Status == 100) {
              $('#finaliza').text("EL PROCESO HA FINALIZADO");
          }

          if (obj.length == 0)
              return "&nbsp;";

          var result = "<table " +
                         "cellspacing='0' " +
                         "cellpadding='3' " +
                         "width='99%'>";

          result += "<td " +
                            "align='left' " +
                            "style='width:1%; white-space:nowrap'>" +
                            "<b>Proceso</b>" +
                          "</td>";

          result += "<td " +
                            "align='left' " +
                            "style='width:89%;'>" +
                            "<b>Progreso</b>" +
                          "</td>";

          result += "<td " +
                            "align='left' " +
                            "style='width:10%; white-space:nowrap'>" +
                            "<b>Completado</b>" +
                          "</td>";

          for (i = 0; i < obj.length; i++) {

              count = obj[i].Status;


              result += "<tr>";

              result += "<td " +
                            "align='left' " +
                            "style='width:1%; white-space:nowrap'>" +
                            obj[i].Name +
                          "</td>";

              result += "<td " +
                            "align='left' " +
                            "style='width:89%;'>" +
                                "<div " +
                                    "style='width:100%; " +
                                    "background-color:white' " +
                                    ">" +
                                    "<div " +
                                        "style='width:" +
                                        obj[i].Status +
                                        "%; background-color:" +
                                            (obj[i].Status < 100 ?
                                                "red" : "green") +
                                            ";'>" +
                                        "&nbsp;</div>" +
                                "</div>" +
                          "</td>";

              result += "<td " +
                            "align='left' " +
                            "style='width:10%; white-space:nowrap'>" +
                            obj[i].Status + " %" +
                          "</td>";

              result += "</tr>";
          }

          result += "</table>";

          return result;
      };
    </script>
    <div>
    <table border="0" width="100%" class="tabla-principal">  
        <tr>
            <td colspan="2" class="titulo"                 
                style="background-color: #333333; color: #FFFFFF; font-family: 'Arial Black'; font-weight: bold; font-size: 11px;">
             <asp:Image ID="Image1" runat="server"
            ImageUrl="~/images/gov.gif" /><label id="leyenda" runat="server"></label></td>
        </tr>        
    </table>    
        <br />        
    </div>    
    <div>        
        <input 
            type="hidden" runat="server"            
            id="process" 
            value="" 
            />       
    </div>   
   
    <div style="font-family: 'Arial Black'; font-weight: bold; font-size: 11px; text-align:center; border: dashed 1px black;" 
        id="resultDiv">  
              
        &nbsp;&nbsp;
        <br />           
        <asp:Label ID="lbInst" runat="server"></asp:Label> 
        &nbsp;&nbsp;        
        <br />    
        <asp:Label ID="lbEjercicio" runat="server" Text="Ejercicio" Visible = "false"></asp:Label>
        <asp:DropDownList ID="Ejercicio" runat="server" Visible="false"></asp:DropDownList>
        <asp:Label ID="lbPeriodo" runat="server" Text="Periodo" Visible = "false"></asp:Label>
        <asp:DropDownList ID="Periodo" runat="server" Visible ="false"></asp:DropDownList>       
        <asp:Label ID="lbTipoOperacion" runat="server" Text="Tipo Operacion" Visible = "false"></asp:Label>
        <asp:DropDownList ID="TipoOperacion" runat="server" Visible="false">
            <asp:ListItem Text="Acto de Fiscalización" Value="AF" Selected></asp:ListItem>
            <asp:ListItem Text="Fiscalización Compulsa" Value="FC"></asp:ListItem>
            <asp:ListItem Text="Devolución" Value="DE"></asp:ListItem>
            <asp:ListItem Text="Compensación" Value="CO"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="lbReferencia" runat="server" Text="Referencia" Visible = "false"></asp:Label>
        <asp:TextBox ID="txtReferencia" runat="server" Width ="250" Visible ="false"></asp:TextBox>        
         &nbsp;&nbsp;        
         <br />     
        
    </div>   
    <div>
        &nbsp;&nbsp;        
        <br />    
        <input
            id="consolidar" 
            runat="server"
            type="button" 
            class="boton"
            value="Generar" 
            onclick="            
                    mainScreen.ExecuteCommand(
                    'LaunchNewProcess', 
                    'mHandlers.Void', 
                    $get('process').value);
                    "
             />
             <input id="btnPreviewRpt" runat="server" type="button" class="boton" value="Ver Reporte" onclick="AbrirReporte();" Visible = "false" />
             <input id="cerrar" runat="server" type="button" class="boton" value="Cerrar" onclick="parent.$.fancybox.close();" />
             <%--<asp:Button ID="btnDetener" runat="server" Text="Detener" CssClass="boton" />
             <asp:Button ID="btnIniciar" runat="server" Text="Iniciar" CssClass="boton" />--%>
    </div>
    <div>
     &nbsp;&nbsp;        
        <br />    
    <table style="font-family: 'Arial Black'; font-weight: bold; font-size: 11px; text-align:center;" width="99%">
    <tr>
    <td>
    <label id="finaliza" runat="server"></label>
    </td>
    </tr> 
    </table>   
    </div>
    </form>
</body>
</html>
