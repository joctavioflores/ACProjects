<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Reporte.aspx.vb" Inherits="ConsolidaService.Reporte" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
    var ll_usuario = "<%= sUsuario %>";
    var tipoXML = ""
    var ArchivoXML = ""

    
        //ValidandoMascara
        function checkFolios(field, rules, i, options) {
            var ini = $("#<%= FolioIni.ClientID %>").val();
            var fin = $("#<%= FolioFin.ClientID %>").val();
            //        alert("ini: " + ini + " fin: " + fin);
            if (ini > fin) {
                return options.allrules.validateDiferente.alertText;
            }
        }

        function ReporteSelected() {

            var tipo = $("#Reporte").val();            
            
            if (tipo == 0) {
                //REPORTES SIN SELECCIONAR   
                tipoXML = ""
                ArchivoXML = ""        
                $(".niveles").hide()
                $(".cuentas").hide()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").hide()
                $(".periodos").hide()
                $(".feini").hide()
                $(".fefin").hide()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").hide() 
            }


            if (tipo == 1) {
                //LIBRO DIARIO
                tipoXML = "POLIZAS"
                ArchivoXML = "POLIZAS"   
                $(".niveles").hide()
                $(".cuentas").hide()               
                $(".fini").show()
                $(".ffin").show()               
                $(".tpoliza").show()
                $(".movimientos").show()
                $(".conceptos").show()
                $(".ejercicios").show()
                $(".periodos").show()
                $(".feini").hide()
                $(".fefin").hide()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").show()
                $(".exportar2").show()
                $(".exportar3").hide()
                $(".exportar4").hide() 
               
                  
            }

            if (tipo == 2) {
                //LIBRO MAYOR
                tipoXML = ""
                ArchivoXML = ""   
                $(".niveles").hide()
                $(".cuentas").hide()               
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").show()
                $(".periodos").show()
                $(".feini").hide()
                $(".fefin").hide()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").show()
                $(".exportar4").hide() 
                  
            }

            if (tipo == 3) {
                //AUXILIARES DE CUENTAS
                tipoXML = ""
                ArchivoXML = ""   
                $(".niveles").hide()
                $(".cuentas").hide()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").hide()
                $(".periodos").hide()
                $(".feini").show()
                $(".fefin").show()
                $(".cuentaini").show()
                $(".cuentafin").show()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").hide() 
            }

            if (tipo == 4) {
                //BALANZA DE COMPROBACIÓN
                tipoXML = "BALANZA"
                ArchivoXML = "BALANZA"   
                $(".niveles").show()
                $(".cuentas").show()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").show()
                $(".periodos").show()
                $(".feini").hide()
                $(".fefin").hide()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()                
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").show() 
            }

            if (tipo == 5) {
                //BALANCE GENERAL
                tipoXML = ""
                ArchivoXML = ""   
                $(".niveles").hide()
                $(".cuentas").hide()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").show()
                $(".periodos").show()
                $(".feini").hide()
                $(".fefin").hide()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").hide() 
            }

            if (tipo == 6) {
                //ESTADO DE RESULTADOS
                tipoXML = ""
                ArchivoXML = ""   
                $(".niveles").hide()
                $(".cuentas").hide()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").show()
                $(".periodos").show()
                $(".feini").hide()
                $(".fefin").hide()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").hide() 
            } 
           
            if (tipo == 7) {
                //ESTADO DE CAMBIOS
                tipoXML = ""
                ArchivoXML = ""   
                $(".niveles").hide()
                $(".cuentas").hide()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").hide()
                $(".periodos").hide()
                $(".feini").show()
                $(".fefin").show()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").hide() 

            }
            if (tipo == 8) {
                //VARIACIONES EN EL CAPITAL
                tipoXML = ""
                ArchivoXML = ""   
                $(".niveles").hide()
                $(".cuentas").hide()
                $(".fini").hide()
                $(".ffin").hide()
                $(".tpoliza").hide()
                $(".movimientos").hide()
                $(".conceptos").hide()
                $(".ejercicios").hide()
                $(".periodos").hide()
                $(".feini").show()
                $(".fefin").show()
                $(".cuentaini").hide()
                $(".cuentafin").hide()
                $(".exportar").hide()
                $(".exportar2").hide()
                $(".exportar3").hide()
                $(".exportar4").hide() 

            }


        }

        function OpenConsolidar(tipo,archivo) {
            $.fancybox({
                'scrolling': 'yes',
                'width': 700,
                'height': 200,
                'transitionIn': 'fade',
                'transitionOut': 'fade',
                'titlePosition': 'inside',
                'type': 'iframe',
                'href': "ProgressBar.aspx?descripcion=" + tipoXML + "&ejercicio=" + $("#Ejercicio").val() + "&periodo=" + $("#Periodo").val() + "&estado=REPORTADO&razon=" + $("#Razon").val() + "&agencia=" + $("#Agencia").val() + "&usuario=" + ll_usuario + "&archivo=" + ArchivoXML,
                'onClosed': function () {
                    document.getElementById("form1").submit();
                }
            });

        }

        function OpenAuxiliar(tipo) {
            $.fancybox({
                'scrolling': 'yes',
                'width': 700,
                'height': 200,
                'transitionIn': 'fade',
                'transitionOut': 'fade',
                'titlePosition': 'inside',
                'type': 'iframe',
                'href': "ProgressBar.aspx?descripcion=" + tipo + "&ejercicio=" + $("#Ejercicio").val() + "&periodo=" + $("#Periodo").val() + "&estado=REPORTADO&razon=" + $("#Razon").val() + "&agencia=" + $("#Agencia").val() + "&usuario=" + ll_usuario + "&archivo=" + tipo,
                'onClosed': function () {
                    document.getElementById("form1").submit();
                }
            });

        }


        $(document).ready(function () {

            ll_usuario = "<%= sUsuario %>";
                    
            ReporteSelected();

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


            $('.Clave1').autocomplete('ashx/CatCuentasDesN3.ashx', {
                width: 420,
                max: 10,
                cacheLength: 1,
                highlight: false,
                scroll: true,
                scrollHeight: 300,
                formatItem: function (data, i, n, value) {
                    var id = data[0];
                    var Clave1 = data[1];
                    var nombre = data[2];
                    var padre = data[3];
                    try {
                        return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Document_New.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Origen: <label title='" + padre + "'>" + padre.substring(0, 30) + "..." + "<br/>Clave1: " + Clave1 + "<br/>Nombre: <label title='" + nombre + "'>" + nombre.substring(0, 30) + "..." + "</label></p></td></tr></table>";
                    } finally {
                        id = null;
                        Clave1 = null;
                        nombre = null;
                        padre = null;
                    }
                },
                formatResult: function (data, value) {
                    return data[1];
                }
            });

            $('.Clave1').result(function (event, data, formatted) {
                if (data) {

                    // Extract the data values
                    var id = data[0];
                    var Clave1 = data[1];
                    var nombre = data[2];


                    $('.Clave1').val(Clave1);
                    id = null;
                    Clave1 = null;
                    nombre = null;

                }
            });

            

            $('.Clave2').autocomplete('ashx/CatCuentasDesN3.ashx', {
                width: 420,
                max: 10,
                cacheLength: 1,
                highlight: false,
                scroll: true,
                scrollHeight: 300,
                formatItem: function (data, i, n, value) {
                    var id = data[0];
                    var Clave2 = data[1];
                    var nombre = data[2];
                    var padre = data[3];
                    try {
                        return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Document_New.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Origen: <label title='" + padre + "'>" + padre.substring(0, 30) + "..." + "<br/>Clave2: " + Clave2 + "<br/>Nombre: <label title='" + nombre + "'>" + nombre.substring(0, 30) + "..." + "</label></p></td></tr></table>";
                    } finally {
                        id = null;
                        Clave2 = null;
                        nombre = null;
                        padre = null;
                    }
                },
                formatResult: function (data, value) {
                    return data[1];
                }
            });

            $('.Clave2').result(function (event, data, formatted) {
                if (data) {

                    // Extract the data values
                    var id = data[0];
                    var Clave2 = data[1];
                    var nombre = data[2];


                    $('.Clave2').val(Clave2);
                    id = null;
                    Clave2 = null;
                    nombre = null;

                }
            });



            $(".movimiento").autocomplete('ashx/CatMovimientosDes.ashx', {
                width: 320,
                max: 10,
                highlight: false,
                scroll: true,
                scrollHeight: 300,
                formatItem: function (data, i, n, value) {

                    // Extract the data values
                    var id = data[0];
                    var descripcion = data[1];

                    return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Dots.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Movimiento: " + data[0] + "<br/> " + data[1] + "</p></td></tr></table>";

                },
                formatResult: function (data, value) {

                    return data[1];

                }
            });

            $(".movimiento").result(function (event, data, formatted) {

                if (data) {

                    // Extract the data values
                    var id = data[0];
                    var descripcion = data[1];
                    $("#idDesMov1").val(data[0]);
                    //                window.location = 'co_movHistoricoPolCap.aspx?idDesMov=' + data[0];
                }
            });



            $(".concepto").autocomplete('ashx/CatConceptosDes.ashx', {
                width: 320,
                max: 10,
                highlight: false,
                scroll: true,
                scrollHeight: 300,
                formatItem: function (data, i, n, value) {

                    // Extract the data values
                    var id = data[0];
                    var descripcion = data[1];
                    
                    return "<table><tr><td style='vertical-align: middle'><img style='display: inline-block; padding-left:5px; padding-right:5px' src='images/Dots.png' alt='' width='25' height='25'" + value + "'/></td><td><p style='display:inline-block'>Movimiento: " + data[0] + "<br/> <label title='" + data[1] + "'>" + data[1].substring(0, 30) + "..." + "</label></p></td></tr></table>";
                },
                formatResult: function (data, value) {
                    return data[1];
                }
            });

            $(".concepto").result(function (event, data, formatted) {

                if (data) {

                    // Extract the data values
                    var id = data[0];
                    var descripcion = data[1];

                    $("#idHPol1").val(data[0]);

                }
            });

        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <input type="hidden" id="idEval" name="idEval" runat="server" /> 
 <div style="display:none">
 </div> 
 <div class="titulo"                 

                style="background-color: #333333; color: #FFFFFF; font-family: Arial; font-weight: bold; font-size: 18px;">
             <asp:Image ID="Image1" runat="server"
            ImageUrl="images/gov.gif" /><label id="title" class="title" runat="server">REPORTES CONSOLIDADOS</label>   </div>      
            <table>
<tr>
<td>
    <ul>
    <li>
        Razon Social
    <div>         
        <asp:DropDownList ID="Razon" runat="server" AutoPostBack="true">           
        </asp:DropDownList>
    <%--    <ajax:CascadingDropDown ID="ccdRazon" runat="server" Category="Razon" TargetControlID="Razon" 
        PromptText="SELECT RAZON" LoadingText="CARGANDO RAZON.." ServiceMethod="BindRazonDetails" 
        ServicePath="CascadingDropdown.asmx">
        </ajax:CascadingDropDown>
        --%>
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
            <asp:ListItem Text = "--SELECT AGENCIA--" Value = ""></asp:ListItem>
        </asp:DropDownList>
        
    </div>
    </li>         
    </ul>
</td>
<td>
    <ul>
    <li>
        Reporte
    <div>         
        <asp:DropDownList ID="Reporte" runat="server" Enabled = "false" AutoPostBack = "true" >
            <asp:ListItem Value="0">-- SELECCIONAR REPORTE --</asp:ListItem>
            <asp:ListItem Value="4">BALANZA DE COMPROBACIÓN</asp:ListItem>
            <%-- <asp:ListItem Value="1">LIBRO DIARIO</asp:ListItem>
            <asp:ListItem Value="2">LIBRO MAYOR</asp:ListItem>
            <asp:ListItem Value="3">AUXILIARES DE CUENTAS</asp:ListItem>
            <asp:ListItem Value="5">BALANCE GENERAL</asp:ListItem>
            <asp:ListItem Value="6">ESTADO DE RESULTADOS</asp:ListItem>   
            <asp:ListItem Value="7">ESTADO DE CAMBIOS </asp:ListItem>
            <asp:ListItem Value="8">VARIACIONES EN EL CAPITAL</asp:ListItem>    --%>
        </asp:DropDownList>
        
    </div>
    </li>         
    </ul>
</td>
<%--<td>
    <ul>
    <li>
        Catálogo
    <div>         
        &nbsp;<asp:DropDownList ID="Catalogo" runat="server"  Width="100px">
           <asp:ListItem Value="L" Selected="True">Local</asp:ListItem>        
        </asp:DropDownList> 
    </div>
    </li>         
    </ul>
</td>--%>
   <td id="ejercicios" runat="server" class="ejercicios">
    <ul>
    <li>
        Ejercicio
    <div>
        <asp:DropDownList ID="Ejercicio" runat="server" Width="80px">
        </asp:DropDownList>        
    </div>
    </li>
      
    </ul>
    </td>    
    <td id="periodos" runat="server" class="periodos">
    <ul>
    <li>
            Periodo
        <div>
            <asp:DropDownList ID="Periodo" runat="server" Width="100px">                
            </asp:DropDownList>
        </div>
        </li>        
    </ul>
</td>
<td id="tpoliza" runat="server" class="tpoliza">
    <ul>   
    <li>
    Tipo De Poliza
    <div>
        <asp:DropDownList runat="server" ID="DDLTipoPoliza">
            <asp:ListItem Text="-- Todos --" Value="0"></asp:ListItem>
            <asp:ListItem Text="Diario" Value="1"></asp:ListItem>
            <asp:ListItem Text="Ingresos" Value="2"></asp:ListItem>
            <asp:ListItem Text="Egresos" Value="3"></asp:ListItem>
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
</tr>
<tr>

  
    <td id="niveles" runat="server" class="niveles">
    <ul>
    <li>
        Nivel
    <div>         
        &nbsp;<asp:DropDownList ID="Nivel" runat="server" Width="100px">
            <asp:ListItem Value="1">NIVEL 1</asp:ListItem>
            <asp:ListItem Value="2">NIVEL 2</asp:ListItem>
            <asp:ListItem Value="3">NIVEL 3</asp:ListItem>
            <asp:ListItem Value="4">NIVEL 4</asp:ListItem>
            <asp:ListItem Value="5">NIVEL 5</asp:ListItem>
            <asp:ListItem Value="6">NIVEL 6</asp:ListItem>    
            <asp:ListItem Value="7">NIVEL 7</asp:ListItem>                         
        </asp:DropDownList>
    </div>
    </li>         
    </ul>
    </td>
    <td id="cuentas" runat="server" class="cuentas">
    <ul>
        <li>
            Cuentas a Consultar
         <div>
            <asp:DropDownList ID="Todas" runat="server">
               <asp:ListItem Value="S">TODAS</asp:ListItem>
               <asp:ListItem Value="C">CON SALDO</asp:ListItem>
               <asp:ListItem Value="N">CON MOVIMIENTOS</asp:ListItem>               
               <asp:ListItem Value="P">CON SALDO Y MOVIMIENTOS</asp:ListItem>               
            </asp:DropDownList>
        </div>
        </li>           
        </ul>
    </td>

    <td id="fini" runat="server" class="fini">
    <ul>
        <li>
        Folio
            Inicial
        <div>
            <asp:TextBox id="FolioIni" runat="server" Width="100" class="validate[custom[onlyNumberSp],funcCall[checkFolios]]"></asp:TextBox>       
        </div>
        </li>
    </ul>
    </td>
    <td id="ffin" runat="server" class="ffin">
    <ul>
        <li>
        Folio
            Final
        <div>
            <asp:TextBox id="FolioFin" runat="server" Width="100" class="validate[custom[onlyNumberSp],funcCall[checkFolios]]"></asp:TextBox>       
        </div>
        </li>       
        </ul>
    </td>

   
<td colspan="2"  id="movimientos" runat="server" class="movimientos">
    <ul>
        <li>
            Movimiento
            <div>         
                <input type="text" id="movimiento" runat="server" name="movimiento" class="movimiento" value="" size="50"/> 
                <input type="hidden" id="idDesMov1" runat="server" name="idDesMov1" class="idDesMov1" value="0"/>     
            </div>

        </li>
    </ul>
</td>
<td colspan="2"  id="conceptos" runat="server" class="conceptos" >
<ul>
<li>
    Concepto
    <div>
        <input type="text" id="concepto"  runat="server"  name="concepto" class="concepto" value="" size="50"/> 
        <input type="hidden" id="idHPol1" runat="server" name="idHPol1" class="idHPol1" value="0"/>         
    </div>
  </li>    
  </ul>
</td>


    <td colspan="2" id="cuentaini" runat="server" class="cuentaini">
    <ul>
    <li>
    CUENTA INICIAL
    <div>
    <input id="Clave1" class="Clave1" runat="server" type="text" style="width:99%" />
    </div>
    </li>
    </ul>
    </td>
    <td colspan="2" id="cuentafin" runat="server" class="cuentafin">
    <ul>
    <li>
    CUENTA FINAL
    <div>
    <input id="Clave2" class="Clave2" runat="server" type="text" style="width:99%" />
    </div>
    </li>
    </ul>
    </td>
<%--<td style="vertical-align: middle" class="style1">
<asp:Button id="BtnExportar" Text="Exportar a Excel" runat="server" CssClass="boton" 
        onclick="btnExportar_Click"/>
    </td>--%>
</tr>
<tr>
<td>
</br>
<asp:Button id="btnBuscar" Text="Consultar" runat="server" CssClass="boton" 
        onclick="btnBuscar_Click"/>
</td>
<td id="exportar" runat="server" class="exportar">
</br>
<input type ="button" id="exp" runat="server" class="boton"  value="POLIZAS XML" onclick="javascript:OpenAuxiliar('POLIZAS');" />
</td>
<td id="exportar2" runat="server" class="exportar2">
</br>
<input type ="button" id="exp2" runat="server" class="boton"  value="CFDI XML" onclick="javascript:OpenAuxiliar('AUXILIARFOLIOS');" /> 
</td>
<td id="exportar3" runat="server" class="exportar3">
</br>
<input type ="button" id="exp3" runat="server" class="boton"  value="AUXILIAR XML" onclick="javascript:OpenAuxiliar('AUXILIARCUENTAS');" /> 
</td>
<td id="exportar4" runat="server" class="exportar4">
</br>
<input type ="button" id="Button1" runat="server" class="boton"  value="BALANZA XML" onclick="javascript:OpenAuxiliar('BALANZA');" /> 
<asp:Button id="BtnExportar" Text="Exportar a Excel" runat="server" CssClass="boton" onclick="btnExportar_Click"/>
</td>
</tr>
</table>
 
        <div id="reportediv" runat="server" style="padding-top:10px">
            <CR:CrystalReportViewer ID="Rpt_Balanza" runat="server" Height="50px" 
             Width="350px" AutoDataBind="false" 
            HasCrystalLogo="False" GroupTreeStyle-ShowLines="True" />
        </div>
    </div>
    </form>
</body>
</html>
