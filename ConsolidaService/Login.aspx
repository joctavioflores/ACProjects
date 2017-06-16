<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="ConsolidaService.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"> 
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="js/jquery-1.11.0.js" type="text/javascript"></script>   
    <script src="js/bootstrap.min.js" type="text/javascript"></script> 
    <script src="js/jasny-bootstrap.min.js" type="text/javascript"></script>  
    <script src="js/bootstrap-select.js" type="text/javascript"></script>
    <script src="js/bootstrap-switch.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="js/highcharts.js" type="text/javascript"></script>
    <script src="js/themes/gray.js" type="text/javascript"></script>
    <link href="css/bootstrap.min.css" rel="stylesheet" media="screen" />   
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css" rel="stylesheet"/>
    <link href="css/jasny-bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-select.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-switch.min.css" rel="stylesheet"type="text/css" />
    <link href="css/datepicker.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #808080; font-family: Arial"> 
    <form id="form1" runat="server">
    <div class="container">
    <div class="row-fluid">
        <div class="span12">
            <ul class="thumbnails">
              <li class="span12">
              <div style="padding: 10px; margin: 10px; background-color: #242424; color: #FFFFFF; font-family: Arial; font-size: 18px">
              <img src="images/v-dealer-logo.png" alt="VDealer" />              
              <div style="text-align: right; display: block;top: -25px;position: relative;">
              <p>Consolidación de Cuentas</p>                                 
              </div>
              </div>                    
            </li>
            </ul>
        </div>
    </div>
    <div class="row-fluid" style="background-color: #808080; font-family: Arial">
        <div class="offset4 span4">
            <form class="form form-horizontal well">
                <div class="control-group">
                    <label for="user-name" class="control-label" style="color: #FFFFFF">Usuario</label>
                    <div class="controls">
                        <input id="user_name" runat="server" class="user-name input-block-level" type="text"/>
                    </div>
                </div>
                <div class="control-group">
                    <label for="password-field" class="control-label" style="color: #FFFFFF">Contraseña</label>
                    <div class="controls">
                        <input id="password_field" runat="server" class="password-field input-block-level" type="password"  />
                    </div>
                </div>                                 
            </form>
        </div>  
        <div class="span4 offset4 text-center">            
                <button type="submit" class="btn boton btn-lg" >
                    <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Entrar&nbsp;&nbsp;
                </button>  
                <button type="button" class="btn btn-default btn-lg">
                  <span class="glyphicon glyphicon-remove"></span> Cancelar
                </button>              
        </div>   
        <div class="span4 offset4 text-center">
            <br />
            <asp:Label ID="leyenada" runat="server" Text="" ForeColor="#FF9933" Font-Bold="True"></asp:Label>
        </div>   
    </div>

    </div> <!-- /container -->
    </form>
     <script type="text/jscript">
         $('.dropdown-toggle').dropdown();
         $('.datepicker').datepicker();
         $('input:checkbox').bootstrapSwitch();
         $('input:radio').bootstrapSwitch();
         $('.selectpicker').selectpicker({
             showSubtext: true
         });
         $('.selectpicker').addClass('span12').selectpicker('setStyle');

         if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
             $('.selectpicker').selectpicker('mobile');
         }

       
    </script>
</body>
</body>
</html>
