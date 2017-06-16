<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AltaUsuario.aspx.vb" Inherits="ConsolidaService.AltaUsuario" %>

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

    <script type="text/javascript">
        var idUsuario = "<%= iidsuario %>";
        
        function checkUsuario() {
            var valor;
            var valor2;
            var p1 =  $("#pass1").val();
            var p2 =  $("#pass2").val();
            var name = $("#nombre").val();
            var nick = $("#usuario").val();

            if (name != "" && nick != "") {

                if (p1 != "" && p1 == p2) {


                    $.ajax({
                        type: "POST",
                        url: "ws/ValidaUsuario.asmx/ValidaNick",
                        data: "{'empSex':'" + idUsuario + "|" + $("#nombre").val() + "|" + $("#usuario").val() + "|" + $("#pass1").val() + "|" + $("#<%= DDLROL.ClientID %>").val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {                           
                            // Do magic here.
                            valor = msg.d;
                        }
                    });

                    if (valor == "SI")
                        alert("¡ El usuario ya se encuentra registrado !");
                    
                    if (valor == "NO") {

                        $.ajax({
                            type: "POST",
                            url: "ws/ValidaUsuario.asmx/Alta",
                            data: "{'empSex':'" + idUsuario + "|" + $("#nombre").val() + "|" + $("#usuario").val() + "|" + $("#pass1").val() + "|" + $("#<%= DDLROL.ClientID %>").val() + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                alert("La información se ha guardado")
                                // Do magic here.
                                valor2 = msg.d;
                                parent.$.fancybox.close();

                            }
                        });

                    }                    
                }
                else {

                    alert("¡ Contraseña incorrecta o vacia !");
                }
            }
            else { 

                alert("¡ Falta definir el nombre o el usuario !")
            }
        }

    </script>



</head>
<body>
    <form id="form1" runat="server">
    <div>        
    <table border="0" width="100%" class="tabla-principal">  
        <tr>
            <td colspan="2" class="titulo"                 
                style="background-color: #333333; color: #FFFFFF; font-family: 'Arial Black'; font-weight: bold; font-size: 11px;">
             <asp:Image ID="Image1" runat="server"
            ImageUrl="images/gov.gif" /><asp:Label ID="titulo" runat="server" Text="ALTA USUARIO"></asp:Label></td>
        </tr>        
    </table>
    </div>
    <div>   
    <table width="100%">
    <tr>
    <td>
    <ul>
    <li>
    NOMBRE
    <div>
    <input type="text" id="nombre" runat="server" style="width:99%; text-transform:uppercase;" />
    </div>
    </li>
    </ul>
    </td>
    <td>
    <ul>
    <li>
    USUARIO
    <div>
    <input type="text" id="usuario" runat="server" style="width:99%; text-transform:uppercase;" />
    </div>
    </li>
    </ul>
    </td>
    </tr>    
    <tr>
    <td>
    <ul>
    <li>
    CONTRASEÑA
    <div>
    <input type="password" id="pass1" runat="server" style="width:99%; text-transform:uppercase;" />
    </div>
    </li>
    </ul>
    </td>
    <td>
    <ul>
    <li>
    CONFIRMAR CONTRSEÑA
    <div>
    <input type="password" id="pass2" runat="server" style="width:99%; text-transform:uppercase;" />
    </div>
    </li>
    </ul>
    </td>
    </tr>
    <tr>
    <td>
    <ul>
    <li>
    ROL DE USUARIO
    <div>
    <asp:DropDownList ID="DDLROL" runat="server">
    </asp:DropDownList>
    </div>
    </li>
    </ul>
    </td>
    </tr>
    <tr>
    <td colspan ="2" style="text-align: center">
    <input type="submit" id="acepta" name="acepta" value="ACEPTAR" class="boton" onclick="checkUsuario();" /> <input id="btnCancel" type="button" value="CANCELAR" class="boton" onclick="parent.$.fancybox.close();" />
    </td>
    </tr>
    </table>    
    </div>
    </form>
</body>
</html>
