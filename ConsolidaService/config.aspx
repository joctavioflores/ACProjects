<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="config.aspx.vb" Inherits="ConsolidaService.config" %>

<html>


<head id="Head1" runat="server">
    <title>IFrame</title>

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
  
    <script type="text/javascript">

        $(document).ready(function () {

            $("#fechaini").datepicker({
                dateFormat: 'dd/mm/yy',
                showOn: "focus",
                beforeShow: function (input, inst) {
                    inst.dpDiv.css({ marginTop: -(input.offsetHeight - 30) + 'px', marginRight: input.offsetWidth + 'px' });
                }

            });


            $("#fechafin ").datepicker({
                dateFormat: 'dd/mm/yy',
                showOn: "focus",
                beforeShow: function (input, inst) {
                    inst.dpDiv.css({ marginTop: -(input.offsetHeight - 30) + 'px', marginRight: input.offsetWidth + 'px' });
                }

            });



        });
    </script>  


    <style type="text/css">
        #Select1
        {
            width: 72px;
        }
    </style>


</head>
<body>
<form id="form1" runat="server">
      <table border="0" width="100%">  
          <tr>
            <td colspan="6" class="titulo"                 
                style="background-color: #333333; color: #FFFFFF; font-family: 'Arial Black'; font-weight: bold; font-size: 11px;">
             <asp:Image ID="Image1" runat="server"
            ImageUrl="./images/gov.gif" />CONFIGURACIÓN DE CORREO</td>
        </tr>
        <tr>
        <td>
    <ul>
    <li>
        Razon Social
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
        Agencia
    <div>         
        <asp:DropDownList ID="Agencia" runat="server">
        </asp:DropDownList>
        
    </div>
    </li>         
    </ul>
    </td>

    <td>
       
       <ul>
       <li>
       RFC
       <div>
        <asp:TextBox id="rfc" runat="server" MaxLength="13"  style="width:80%"></asp:TextBox>
       </div>
       </li>
       </ul>
       </td>
       <td>
       <ul>
       <li>
       USUARIO
       <div>
        <asp:TextBox id="usuario1" runat="server" style="width:80%"></asp:TextBox>
       </div>
       </li>
       </ul>
       </td>
           
       <td>
       <ul>
       <li>
       PASSWORD
       <div>
        <asp:TextBox id="pass" runat="server" style="width:80%" TextMode="Password"></asp:TextBox>
        </div>
        </li>
       </ul>
       </td>
       
       <tr>
       <td>
       
       <ul>
        <li>
       HOST
       <div>       
         <asp:TextBox id="host1" runat="server" style="width:80%"></asp:TextBox>
       </div>
       </li>
       </ul>
       </td>
            
       <td>
       <ul>
       <li>
       PUERTO
       <div>
        <asp:TextBox id="puerto1" runat="server" style="width:80%"></asp:TextBox>
       </div>
       </li>
       </ul>
       </td>
       <td colspan="3">
       <ul>
       <li>
       DE:
       <div>
        <asp:TextBox id="de" runat="server" style="width:80%"></asp:TextBox>
        </div>
        </li>
       </ul>
       </td>
       
       

   </tr>
        
    <tr>
    <td colspan="2">
       <ul>
       <li>
       PARA:
       <div>
       <asp:TextBox type="text" id="para" runat="server" style="width:80%"></asp:TextBox>
        </div>
        </li>
       </ul>       
       </td>
       <td colspan="3">
       <ul>
       <li>
       CC:
       <div>
       <asp:TextBox id="cc1" runat="server" style="width:80%"></asp:TextBox>
           
        </div>
        </li>
       </ul>
       </td>
    </tr>
    <tr>
        
       
       <td colspan="2">
       <ul>
       <li>
       PROGRAMAR PERIODO DE ENVIO POR:
       <div>       
        <asp:RadioButtonList ID="RadioButtonList1" runat="Server" AutoPostBack="True" >
                            <asp:ListItem Text="FECHA EN ESPECIFICO" Value="fecha" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="REPETIR DÍA DE LA SEMANA" Value="dia"></asp:ListItem>                         
                        </asp:RadioButtonList>
       </div>
       </li>
       </ul>
       </td>
       <td >
        <ul>
        <li id="wrapfecha"  runat="server" style="display:block">
        FECHA
        <div>
            <asp:TextBox id="fechafin" runat="server"></asp:TextBox>       
        </div>
        </li>
        <li id="wraplista" runat="server" style="display:none">
        SELECCIONAR DÍAS
        <div>
        <asp:CheckBoxList ID="dias" runat="server">
            <asp:ListItem Text="Lunes" Value ="lun"></asp:ListItem>
            <asp:ListItem Text="Martes" Value ="mar"></asp:ListItem>
            <asp:ListItem Text="Miercoles" Value ="mie"></asp:ListItem>
            <asp:ListItem Text="Jueves" Value ="jue"></asp:ListItem>
            <asp:ListItem Text="Viernes" Value ="vie"></asp:ListItem>
            <asp:ListItem Text="Sabado" Value ="sab"></asp:ListItem>
            <asp:ListItem Text="Domingo" Value ="dom"></asp:ListItem>
        </asp:CheckBoxList>
       
        </div>        
        </li>
        </ul>
        </td>
        <td>
        <ul>
        <li>
        HORA ACTUAL
        <div>
         <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>
        </li>
        </ul>
        </td>
        <td>
        <ul>
        <li>
        HORA DE ENVÍO
        <div>
         <asp:textbox ID="tiempo1" runat="server" ></asp:textbox>
        </div>
        </li>
        </ul>
        </td>
        </tr>   
        <tr>        
        <td>
        <ul>
        <li>        
        NoCertificado 
        <div>        
        <input type="text" id="NoCertificado" runat="server" style="width:80%" />
        </div>
        </li>
        </ul>
        </td>
        <td>
        <ul>
        <li>        
        RutaCer 
        <div>        
        <input type="text" id="RutaCer" runat="server" style="width:80%" />
        </div>
        </li>
        </ul>
        </td>
         <td>
        <ul>
        <li>        
        RutaKey 
        <div>        
        <input type="text" id="RutaKey" runat="server" style="width:80%" />
        </div>
        </li>
        </ul>
        </td>
        <td>
        <ul>
        <li>        
        PswKey 
        <div>        
        <input type="text" id="PswKey" runat="server" style="width:80%" />
        </div>
        </li>
        </ul>
        </td>   
        </tr>
     </table>            
    
<br />
<asp:Button ID="idguardar1" Text="Guardar" runat="server" CssClass="boton" />
<asp:Button ID="btnenviar" Text="Enviar" runat="server" CssClass="boton" />
<asp:Button ID="idcancelar1" Text="Cancelar" runat="server" CssClass="boton" />
      <asp:TextBox ID="dia" runat="server" Visible="False"></asp:TextBox>
   </form>

   </body>
</html>