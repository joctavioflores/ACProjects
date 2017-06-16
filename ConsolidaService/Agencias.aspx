<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Agencias.aspx.vb" Inherits="ConsolidaService.Agencias" %>

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
            ImageUrl="~/images/gov.gif" />CATÁLOGO DE AGENCIAS</td>
        </tr>        
    </table>    
    </div>
    <div>
    <asp:GridView ID="lstagencias" runat="server" 
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
