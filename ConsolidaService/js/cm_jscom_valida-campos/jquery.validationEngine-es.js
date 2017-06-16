

(function ($) {
    $.fn.validationEngineLanguage = function () {
    };
    $.validationEngineLanguage = {
        newLang: function () {
            $.validationEngineLanguage.allRules = {
                "required": { // Add your regex rules here, you can take telephone as an example
                    "regex": "none",
                    "alertText": "* Este campo es obligatorio",
                    "alertTextCheckboxMultiple": "* Por favor selecciona una opción",
                    "alertTextCheckboxe": "* Este checkbox está requerido"
                },
                "minSize": {
                    "regex": "none",
                    "alertText": "* Mínimo de ",
                    "alertText2": " caracteres autorizados"
                },
                "maxSize": {
                    "regex": "none",
                    "alertText": "* Máximo de ",
                    "alertText2": " caracteres autorizados"
                },
                "min": {
                    "regex": "none",
                    "alertText": "* Valor mínimo es "
                },
                "max": {
                    "regex": "none",
                    "alertText": "* Valor máximo es "
                },
                "past": {
                    "regex": "none",
                    "alertText": "* Fecha anterior a "
                },
                "future": {
                    "regex": "none",
                    "alertText": "* Fecha posterior a "
                },
                "maxCheckbox": {
                    "regex": "none",
                    "alertText": "* Se ha excedido el número de opciones permitidas"
                },
                "minCheckbox": {
                    "regex": "none",
                    "alertText": "* Por favor seleccione ",
                    "alertText2": " opciones"
                },
                "equals": {
                    "regex": "none",
                    "alertText": "* Los campos no coinciden"
                },
                "rfc": {
                    "regex": /^([0-9a-zA-Z]{10}[\-]{1}[0-9a-zA-Z]{3})?([0-9a-zA-Z]{13})?$/,
                    "alertText": "* RFC inválido"
                },
                "phone": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([\+][0-9]{1,3}[ \.\-])?([\(]{1}[0-9]{2,6}[\)])?([0-9 \.\-\/]{3,20})((x|ext|extension)[ ]?[0-9]{1,4})?$/,
                    "alertText": "* Número de teléfono inválido"
                },
                "email": {
                    // Shamelessly lifted from Scott Gonzalez via the Bassistance Validation plugin http://projects.scottsplayground.com/email_address_validation/
                    "regex": /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/,
                    "alertText": "* Correo inválido"
                },
                "integer": {
                    "regex": /^[\-\+]?\d+$/,
                    "alertText": "* No es un valor entero válido"
                },
                "number": {
                    // Number, including positive, negative, and floating decimal. credit: orefalo
                    "regex": /^[\-\+]?(([0-9]+)([\.,]([0-9]+))?|([\.,]([0-9]+))?)$/,
                    "alertText": "* No es un valor decimal válido"
                },
                "date": {
                    "regex": /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/,
                    "alertText": "* Fecha inválida, por favor utilize el formato AAAA-MM-DD"
                },
                "localdate": {
                    "regex": /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/,
                    "alertText": "* Fecha inválida, por favor utilize el formato DD-MM-AAAA"
                },
                "year": {
                    "regex": /^\d{4}$/,
                    "alertText": "* Año invalido, por favor utilize el formato AAAA"
                },
                "generaltime": {
                    "regex": /^\d{2}:\d{2}$/,
                    "alertText": "* Tiempo, por favor utilize el formato HH:MM"
                },
                "hora24": {
                    "regex": /^([0-1]\d|2[0-3]):[0-5]\d$/,
                    "alertText": "* Hora invalida, por favor utilize el formato HH:MM [24hrs]"
                },
                "hora12": {
                    "regex": /^(0[1-9]|1[0-2]):[0-5]\d$/,
                    "alertText": "* Hora invalida, por favor utilize el formato HH:MM [12hrs]"
                },
                "ipv4": {
                    "regex": /^((([01]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))[.]){3}(([0-1]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))$/,
                    "alertText": "* Direccion IP inválida"
                },
                "url": {
                    "regex": /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/,
                    "alertText": "* URL Inválida"
                },
                "filepath": {
                    "regex": /^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))$/,
                    "alertText": "* Ruta invalida"
                },
                "folderpath": {
                    "regex": /^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))$/,
                    "alertText": "* Ruta invalida"
                },
                "currency": {
                    "regex": /^\$?\-?([1-9]{1}[0-9]{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))$|^\-?\$?([1-9]{1}\d{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))$|^\(\$?([1-9]{1}\d{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))\)$/,
                    "alertText": "* Formato incorrecto"
                },
                "onlyNumberSp": {
                    "regex": /^[0-9\ ]+$/,
                    "alertText": "* Sólo números"
                },
                "onlyLetterSp": {
                    "regex": /^[a-zA-Z\ \']+$/,
                    "alertText": "* Sólo letras"
                },
                "onlyLetter": {
                    "regex": /^[a-zA-Z\ \']+$/,
                    "alertText": "* Letters only"
                },
                "onlyXLS": {
                    "regex": /^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ].*))(.xls|.XLS)$/,
                    "alertText": "* Solo se permiten archivo(s) con extension .xls"
                },
                "onlyCutomMask": {
                    "regex": /^[0-9a-zA-Z]+$/,
                    "alertText": "* Sólo se permiten los siguientes caracteres (a-Z Ó A-z Ó 0-9)"
                },
                "onlyLetterNumber": {
                    "regex": /^[0-9a-zA-Z]+$/,
                    "alertText": "* No se permiten caracteres especiales"
                },
                "onlyLetterNumber": {
                    "regex": /^[0-9a-zA-Z]+$/,
                    "alertText": "* No se permiten caracteres especiales"
                },
                // --- CUSTOM RULES -- Those are specific to the demos, they can be removed or changed to your likings
                "ajaxUserCall": {
                    "url": "ajaxValidateFieldUser",
                    // you may want to pass extra data on the ajax call
                    "extraData": "name=eric",
                    "alertTextLoad": "* Cargando, espere por favor",
                    "alertText": "* Este nombre de usuario ya se encuentra usado"
                },
                "ajaxNameCall": {
                    // remote json service location
                    "url": "ajaxValidateFieldName",
                    // error
                    "alertText": "* Este nombre ya se encuentra usado",
                    // if you provide an "alertTextOk", it will show as a green prompt when the field validates
                    "alertTextOk": "* Este nombre está disponible",
                    // speaks by itself
                    "alertTextLoad": "* Cargando, espere por favor"
                },
                "cuentaexiste": {
                    "alertText": "* La Cuenta ya Existe"
                },                
                "cuentavalida": {
                    "alertText": "* Cuenta no valida <br /><a id='a2' href='#' onclick='OpenCuenta()'>Crear Cuenta</a>"
                },
                "ctamovvalida": {
                    "alertText": "* Cuenta no permitida <br /><a id='a2' href='#' onclick='OpenCuenta()'>Ver Información</a>"
                },
                "cuentaconsaldos": {
                    "alertText": "* No se permite generar subcuenta de una Cuenta que ya tiene movimientos </a>"
                },
                "cuentanivel": {
                    "alertText": "* No se permite generar subcuenta de una Cuenta de nivel 5 </a>"
                },
                "proveedorvalida": {
                    "alertText": "* Proveedor no valido <br /><a id='a2' href='#' onclick='OpenProveedor()'>Crear Proveedor</a>"
                },
                "proveedorcuenta": {
                    "alertText": "* Falta Asociación a Cuenta <br /><a id='a2' href='#' onclick='OpenProveedor()'>Editar Proveedor</a>"
                },
                "empleadocuenta": {
                    "alertText": "* Falta Asociación a Cuenta <br /><a id='a2' href='#' onclick='OpenEmpleado()'>Editar Empleado</a>"
                },
                "clientecuenta": {
                    "alertText": "* Falta Asociación a Cuenta <br /><a id='a2' href='#' onclick='OpenCliente()'>Editar Cliente</a>"
                },
                "tipoproveedor": {
                    "alertText": "* Tipo de Proveedor no valido <br /><a id='a2' href='#' onclick='OpenProveedor()'>Editar Proveedor</a>"
                },
                "referenciavalida": {
                    "alertText": "* Referencia no valida</a>"
                },
                "tienehijas": {
                    "alertText": "* No se permite el uso de cuentas padre para aplicacion de asientos contables<br />Verificar catalogo y nivel de cuentas"
                },
                "bancovalida": {
                    "alertText": "* No Cuenta no valido <br /><a id='a2' href='#' onclick='OpenBanco()'>Crear No Cuenta</a>"
                },
                "departamentovalida": {
                    "alertText": "* Debe de seleccionar un departamento"
                },
                "periodovalido": {
                    "alertText": "* Periodo cerrado <br /><a id='a2' href='#' onclick='OpenPeriodo()'>Enviar Solicitud de Periodo</a>"
                },
                "parametrovalida": {
                    "alertText": "* Parametro no valido <br /><a id='a2' href='#' onclick='OpenParametro(0)'>Crear Parametro</a>"
                },
                "mascaraIncorrecta": {
                    "alertText": "* El valor del campo no cumple con la mascara asignada"
                },
                "validateDiferente": {
                    "alertText": "* El valor del campo debe ser distinto al valor actual"
                },
                "validateMayorCero": {
                    "alertText": "* El valor del campo debe ser mayor a cero"
                },
                "validateDesde": {
                    "alertText": "* El valor del campo debe ser menor"
                },
                "saldosiguales": {
                    "alertText": "* Los saldos no son iguales"
                },
                "validateHasta": {
                    "alertText": "* El valor del campo debe ser mayo"
                },
                "movimientovalido": {
                    "alertText": "* Movimiento No Valido"
                },
                "validate2fields": {
                    "alertText": "* Por favor entrar HELLO"
                },
                "proveedorrepetido": {
                    "alertText": "* Ya existe un proveedor con esta razon social"
                },
                "rfcvalido": {
                    "regex": /^([a-zñA-ZÑ&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[a-zA-Z\d]{3})?$/,
                    "alertText": "* RFC no valido "
                }
            };

        }
    };
    $.validationEngineLanguage.newLang();
})(jQuery);

