﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.34014
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'This source code was auto-generated by xsd, Version=4.0.30319.33440.
'

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda"),  _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda", IsNullable:=false)>  _
Partial Public Class ComprobanteNissan
    
    Private headerField As ComprobanteNissanHeader
    
    Private detailField() As ComprobanteNissanPartes
    
    Private totalsField As ComprobanteNissanTotals
    
    '''<comentarios/>
    Public Property Header() As ComprobanteNissanHeader
        Get
            Return Me.headerField
        End Get
        Set
            Me.headerField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlArrayItemAttribute("Partes", IsNullable:=false)>  _
    Public Property Detail() As ComprobanteNissanPartes()
        Get
            Return Me.detailField
        End Get
        Set
            Me.detailField = value
        End Set
    End Property
    
    '''<comentarios/>
    Public Property Totals() As ComprobanteNissanTotals
        Get
            Return Me.totalsField
        End Get
        Set
            Me.totalsField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanHeader
    
    Private extras_Header_1Field() As ComprobanteNissanHeaderExtras_Header_1
    
    Private datos_ClienteField() As ComprobanteNissanHeaderDatos_Cliente
    
    Private terminos_de_pagoField() As ComprobanteNissanHeaderTerminos_de_pago
    
    Private cVEMETODOTRANSPField As String
    
    Private fACTURAField As String
    
    Private fECHAADUPAGOField As String
    
    Private fECHAFACTURAField As String
    
    Private fLETEPAGADOField As String
    
    Private fOLIOSLIQUIDACIONField As String
    
    Private fORMA_PAGOField As String
    
    Private hORAFACTURAField As String
    
    Private nOMADUPAGOField As String
    
    Private pORCENTAJE_IVAField As String
    
    Private rEF_ACCESORIOLATINOAField As String
    
    Private sERIEField As String
    
    Private tIPOFACTURAField As String
    
    Private tIPOPEDIDOField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("Extras_Header_1")>  _
    Public Property Extras_Header_1() As ComprobanteNissanHeaderExtras_Header_1()
        Get
            Return Me.extras_Header_1Field
        End Get
        Set
            Me.extras_Header_1Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("Datos_Cliente")>  _
    Public Property Datos_Cliente() As ComprobanteNissanHeaderDatos_Cliente()
        Get
            Return Me.datos_ClienteField
        End Get
        Set
            Me.datos_ClienteField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("Terminos_de_pago")>  _
    Public Property Terminos_de_pago() As ComprobanteNissanHeaderTerminos_de_pago()
        Get
            Return Me.terminos_de_pagoField
        End Get
        Set
            Me.terminos_de_pagoField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("CVEMETODO-TRANSP")>  _
    Public Property CVEMETODOTRANSP() As String
        Get
            Return Me.cVEMETODOTRANSPField
        End Get
        Set
            Me.cVEMETODOTRANSPField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property FACTURA() As String
        Get
            Return Me.fACTURAField
        End Get
        Set
            Me.fACTURAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property FECHAADUPAGO() As String
        Get
            Return Me.fECHAADUPAGOField
        End Get
        Set
            Me.fECHAADUPAGOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property FECHAFACTURA() As String
        Get
            Return Me.fECHAFACTURAField
        End Get
        Set
            Me.fECHAFACTURAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("FLETE-PAGADO")>  _
    Public Property FLETEPAGADO() As String
        Get
            Return Me.fLETEPAGADOField
        End Get
        Set
            Me.fLETEPAGADOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property FOLIOSLIQUIDACION() As String
        Get
            Return Me.fOLIOSLIQUIDACIONField
        End Get
        Set
            Me.fOLIOSLIQUIDACIONField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property FORMA_PAGO() As String
        Get
            Return Me.fORMA_PAGOField
        End Get
        Set
            Me.fORMA_PAGOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property HORAFACTURA() As String
        Get
            Return Me.hORAFACTURAField
        End Get
        Set
            Me.hORAFACTURAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property NOMADUPAGO() As String
        Get
            Return Me.nOMADUPAGOField
        End Get
        Set
            Me.nOMADUPAGOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property PORCENTAJE_IVA() As String
        Get
            Return Me.pORCENTAJE_IVAField
        End Get
        Set
            Me.pORCENTAJE_IVAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("REF_ACCESORIO-LATINOA")>  _
    Public Property REF_ACCESORIOLATINOA() As String
        Get
            Return Me.rEF_ACCESORIOLATINOAField
        End Get
        Set
            Me.rEF_ACCESORIOLATINOAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property SERIE() As String
        Get
            Return Me.sERIEField
        End Get
        Set
            Me.sERIEField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("TIPO-FACTURA")>  _
    Public Property TIPOFACTURA() As String
        Get
            Return Me.tIPOFACTURAField
        End Get
        Set
            Me.tIPOFACTURAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TIPOPEDIDO() As String
        Get
            Return Me.tIPOPEDIDOField
        End Get
        Set
            Me.tIPOPEDIDOField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanHeaderExtras_Header_1
    
    Private tipoField As String
    
    Private valorField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Tipo() As String
        Get
            Return Me.tipoField
        End Get
        Set
            Me.tipoField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Valor() As String
        Get
            Return Me.valorField
        End Get
        Set
            Me.valorField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanHeaderDatos_Cliente
    
    Private dCDIRECCION1Field As String
    
    Private dCDIRECCION3Field As String
    
    Private dCDIRECCION4Field As String
    
    Private dCDIRECCION5Field As String
    
    Private dCRAZONSOCIALField As String
    
    Private dCRFCField As String
    
    Private dCCVECLIENTEField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC-DIRECCION1")>  _
    Public Property DCDIRECCION1() As String
        Get
            Return Me.dCDIRECCION1Field
        End Get
        Set
            Me.dCDIRECCION1Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC-DIRECCION3")>  _
    Public Property DCDIRECCION3() As String
        Get
            Return Me.dCDIRECCION3Field
        End Get
        Set
            Me.dCDIRECCION3Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC-DIRECCION4")>  _
    Public Property DCDIRECCION4() As String
        Get
            Return Me.dCDIRECCION4Field
        End Get
        Set
            Me.dCDIRECCION4Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC-DIRECCION5")>  _
    Public Property DCDIRECCION5() As String
        Get
            Return Me.dCDIRECCION5Field
        End Get
        Set
            Me.dCDIRECCION5Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC-RAZONSOCIAL")>  _
    Public Property DCRAZONSOCIAL() As String
        Get
            Return Me.dCRAZONSOCIALField
        End Get
        Set
            Me.dCRAZONSOCIALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC-RFC")>  _
    Public Property DCRFC() As String
        Get
            Return Me.dCRFCField
        End Get
        Set
            Me.dCRFCField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DC.CVECLIENTE")>  _
    Public Property DCCVECLIENTE() As String
        Get
            Return Me.dCCVECLIENTEField
        End Get
        Set
            Me.dCCVECLIENTEField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanHeaderTerminos_de_pago
    
    Private tPCONDPAGO1Field As String
    
    Private tPDIASCREDITOField As String
    
    Private tPTERMINOSPAGOField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("TP-CONDPAGO1")>  _
    Public Property TPCONDPAGO1() As String
        Get
            Return Me.tPCONDPAGO1Field
        End Get
        Set
            Me.tPCONDPAGO1Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("TP-DIASCREDITO")>  _
    Public Property TPDIASCREDITO() As String
        Get
            Return Me.tPDIASCREDITOField
        End Get
        Set
            Me.tPDIASCREDITOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("TP-TERMINOSPAGO")>  _
    Public Property TPTERMINOSPAGO() As String
        Get
            Return Me.tPTERMINOSPAGOField
        End Get
        Set
            Me.tPTERMINOSPAGOField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanPartes
    
    Private cAPACIDADMOTORField As String
    
    Private cOLORField As String
    
    Private mARCAField As String
    
    Private mODELOField As String
    
    Private mOTORField As String
    
    Private oRDENCOMPRAField As String
    
    Private sERIEField As String
    
    Private tIPO2Field As String
    
    Private tRANSMISIONField As String
    
    Private cANTIDADVEHICULOSField As String
    
    Private cANTPIEZASORDENADASField As String
    
    Private cANTPIEZASPENDField As String
    
    Private cODIGOPRECIOField As String
    
    Private cVEVEHICULARField As String
    
    Private dCSVEHICULOField As String
    
    Private dESCTOLINMONEXTField As String
    
    Private dESCTOLINMONNALField As String
    
    Private iMPORTELINMONEXTField As String
    
    Private iMPORTELINMONNALField As String
    
    Private nUMREFERENCIAPZAField As String
    
    Private pPUBPZAMONEXTField As String
    
    Private pPUBPZAMONNALField As String
    
    Private pORCENTAJEDESCTOLINEAField As String
    
    Private pRECIOUNITARIOMONEXTField As String
    
    Private pRECIOUNITARIOMONNALField As String
    
    Private uNIDADDEMEDIDAField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CAPACIDADMOTOR() As String
        Get
            Return Me.cAPACIDADMOTORField
        End Get
        Set
            Me.cAPACIDADMOTORField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property COLOR() As String
        Get
            Return Me.cOLORField
        End Get
        Set
            Me.cOLORField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MARCA() As String
        Get
            Return Me.mARCAField
        End Get
        Set
            Me.mARCAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MODELO() As String
        Get
            Return Me.mODELOField
        End Get
        Set
            Me.mODELOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MOTOR() As String
        Get
            Return Me.mOTORField
        End Get
        Set
            Me.mOTORField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property ORDENCOMPRA() As String
        Get
            Return Me.oRDENCOMPRAField
        End Get
        Set
            Me.oRDENCOMPRAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property SERIE() As String
        Get
            Return Me.sERIEField
        End Get
        Set
            Me.sERIEField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("TIPO.2")>  _
    Public Property TIPO2() As String
        Get
            Return Me.tIPO2Field
        End Get
        Set
            Me.tIPO2Field = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TRANSMISION() As String
        Get
            Return Me.tRANSMISIONField
        End Get
        Set
            Me.tRANSMISIONField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CANTIDADVEHICULOS() As String
        Get
            Return Me.cANTIDADVEHICULOSField
        End Get
        Set
            Me.cANTIDADVEHICULOSField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("CANTPIEZAS-ORDENADAS")>  _
    Public Property CANTPIEZASORDENADAS() As String
        Get
            Return Me.cANTPIEZASORDENADASField
        End Get
        Set
            Me.cANTPIEZASORDENADASField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("CANTPIEZAS-PEND")>  _
    Public Property CANTPIEZASPEND() As String
        Get
            Return Me.cANTPIEZASPENDField
        End Get
        Set
            Me.cANTPIEZASPENDField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CODIGOPRECIO() As String
        Get
            Return Me.cODIGOPRECIOField
        End Get
        Set
            Me.cODIGOPRECIOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CVEVEHICULAR() As String
        Get
            Return Me.cVEVEHICULARField
        End Get
        Set
            Me.cVEVEHICULARField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property DCSVEHICULO() As String
        Get
            Return Me.dCSVEHICULOField
        End Get
        Set
            Me.dCSVEHICULOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DESCTO-LIN-MON-EXT")>  _
    Public Property DESCTOLINMONEXT() As String
        Get
            Return Me.dESCTOLINMONEXTField
        End Get
        Set
            Me.dESCTOLINMONEXTField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("DESCTO-LIN-MON-NAL")>  _
    Public Property DESCTOLINMONNAL() As String
        Get
            Return Me.dESCTOLINMONNALField
        End Get
        Set
            Me.dESCTOLINMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IMPORTE-LIN-MON-EXT")>  _
    Public Property IMPORTELINMONEXT() As String
        Get
            Return Me.iMPORTELINMONEXTField
        End Get
        Set
            Me.iMPORTELINMONEXTField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IMPORTE-LIN-MON-NAL")>  _
    Public Property IMPORTELINMONNAL() As String
        Get
            Return Me.iMPORTELINMONNALField
        End Get
        Set
            Me.iMPORTELINMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("NUMREFERENCIA-PZA")>  _
    Public Property NUMREFERENCIAPZA() As String
        Get
            Return Me.nUMREFERENCIAPZAField
        End Get
        Set
            Me.nUMREFERENCIAPZAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("P-PUB-PZA-MON-EXT")>  _
    Public Property PPUBPZAMONEXT() As String
        Get
            Return Me.pPUBPZAMONEXTField
        End Get
        Set
            Me.pPUBPZAMONEXTField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("P-PUB-PZA-MON-NAL")>  _
    Public Property PPUBPZAMONNAL() As String
        Get
            Return Me.pPUBPZAMONNALField
        End Get
        Set
            Me.pPUBPZAMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PORCENTAJE-DESCTOLINEA")>  _
    Public Property PORCENTAJEDESCTOLINEA() As String
        Get
            Return Me.pORCENTAJEDESCTOLINEAField
        End Get
        Set
            Me.pORCENTAJEDESCTOLINEAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PRECIOUNITARIO-MON-EXT")>  _
    Public Property PRECIOUNITARIOMONEXT() As String
        Get
            Return Me.pRECIOUNITARIOMONEXTField
        End Get
        Set
            Me.pRECIOUNITARIOMONEXTField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PRECIOUNITARIO-MON-NAL")>  _
    Public Property PRECIOUNITARIOMONNAL() As String
        Get
            Return Me.pRECIOUNITARIOMONNALField
        End Get
        Set
            Me.pRECIOUNITARIOMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("UNIDAD-DE-MEDIDA")>  _
    Public Property UNIDADDEMEDIDA() As String
        Get
            Return Me.uNIDADDEMEDIDAField
        End Get
        Set
            Me.uNIDADDEMEDIDAField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanTotals
    
    Private tasasField() As ComprobanteNissanTotalsTasas
    
    Private leyendas_PreciosField() As Object
    
    Private gastosField() As ComprobanteNissanTotalsGastos
    
    Private cANTLETRAField As String
    
    Private iRFField As String
    
    Private iRFMONNALField As String
    
    Private iVAMANEJOMATField As String
    
    Private iVAMMATMONNALField As String
    
    Private mONTODCTOMONNALField As String
    
    Private mONTODESCUENTOField As String
    
    Private mONTOFLETESField As String
    
    Private mONTOFLETESMONNALField As String
    
    Private mONTOMANEJOMATField As String
    
    Private mONTOSEGMONNALField As String
    
    Private mONTOSEGUROField As String
    
    Private mONTOVTANETAField As String
    
    Private mTOMMATMONNALField As String
    
    Private mTOVTANETMONNALField As String
    
    Private pORCDESCTOField As String
    
    Private pORCIVAField As String
    
    Private pORCIVAMMATERIALField As String
    
    Private iMPORTEMONNALField As String
    
    Private pRECIOBASEMONNALField As String
    
    Private pRECIOTOTMONNALField As String
    
    Private pRECIOTOTALField As String
    
    Private sUBTOTALField As String
    
    Private sUBTOTALMONNALField As String
    
    Private vTAREFACCMONNALField As String
    
    Private vTAREFACCIONESField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("Tasas")>  _
    Public Property Tasas() As ComprobanteNissanTotalsTasas()
        Get
            Return Me.tasasField
        End Get
        Set
            Me.tasasField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("Leyendas_Precios")>  _
    Public Property Leyendas_Precios() As Object()
        Get
            Return Me.leyendas_PreciosField
        End Get
        Set
            Me.leyendas_PreciosField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("Gastos")>  _
    Public Property Gastos() As ComprobanteNissanTotalsGastos()
        Get
            Return Me.gastosField
        End Get
        Set
            Me.gastosField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CANTLETRA() As String
        Get
            Return Me.cANTLETRAField
        End Get
        Set
            Me.cANTLETRAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property IRF() As String
        Get
            Return Me.iRFField
        End Get
        Set
            Me.iRFField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IRF-MON-NAL")>  _
    Public Property IRFMONNAL() As String
        Get
            Return Me.iRFMONNALField
        End Get
        Set
            Me.iRFMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IVA-MANEJO-MAT")>  _
    Public Property IVAMANEJOMAT() As String
        Get
            Return Me.iVAMANEJOMATField
        End Get
        Set
            Me.iVAMANEJOMATField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IVA-MMAT-MON-NAL")>  _
    Public Property IVAMMATMONNAL() As String
        Get
            Return Me.iVAMMATMONNALField
        End Get
        Set
            Me.iVAMMATMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MONTO-DCTO-MON-NAL")>  _
    Public Property MONTODCTOMONNAL() As String
        Get
            Return Me.mONTODCTOMONNALField
        End Get
        Set
            Me.mONTODCTOMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MONTO-DESCUENTO")>  _
    Public Property MONTODESCUENTO() As String
        Get
            Return Me.mONTODESCUENTOField
        End Get
        Set
            Me.mONTODESCUENTOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MONTOFLETES() As String
        Get
            Return Me.mONTOFLETESField
        End Get
        Set
            Me.mONTOFLETESField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MONTOFLETES-MON-NAL")>  _
    Public Property MONTOFLETESMONNAL() As String
        Get
            Return Me.mONTOFLETESMONNALField
        End Get
        Set
            Me.mONTOFLETESMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MONTOMANEJO-MAT")>  _
    Public Property MONTOMANEJOMAT() As String
        Get
            Return Me.mONTOMANEJOMATField
        End Get
        Set
            Me.mONTOMANEJOMATField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MONTOSEG-MON-NAL")>  _
    Public Property MONTOSEGMONNAL() As String
        Get
            Return Me.mONTOSEGMONNALField
        End Get
        Set
            Me.mONTOSEGMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MONTOSEGURO() As String
        Get
            Return Me.mONTOSEGUROField
        End Get
        Set
            Me.mONTOSEGUROField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MONTOVTA-NETA")>  _
    Public Property MONTOVTANETA() As String
        Get
            Return Me.mONTOVTANETAField
        End Get
        Set
            Me.mONTOVTANETAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MTO-MMAT-MON-NAL")>  _
    Public Property MTOMMATMONNAL() As String
        Get
            Return Me.mTOMMATMONNALField
        End Get
        Set
            Me.mTOMMATMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("MTOVTA-NET-MON-NAL")>  _
    Public Property MTOVTANETMONNAL() As String
        Get
            Return Me.mTOVTANETMONNALField
        End Get
        Set
            Me.mTOVTANETMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PORC-DESCTO")>  _
    Public Property PORCDESCTO() As String
        Get
            Return Me.pORCDESCTOField
        End Get
        Set
            Me.pORCDESCTOField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PORC-IVA")>  _
    Public Property PORCIVA() As String
        Get
            Return Me.pORCIVAField
        End Get
        Set
            Me.pORCIVAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PORC-IVAMMATERIAL")>  _
    Public Property PORCIVAMMATERIAL() As String
        Get
            Return Me.pORCIVAMMATERIALField
        End Get
        Set
            Me.pORCIVAMMATERIALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IMPORTE-MON-NAL")>  _
    Public Property IMPORTEMONNAL() As String
        Get
            Return Me.iMPORTEMONNALField
        End Get
        Set
            Me.iMPORTEMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PRECIOBASE-MON-NAL")>  _
    Public Property PRECIOBASEMONNAL() As String
        Get
            Return Me.pRECIOBASEMONNALField
        End Get
        Set
            Me.pRECIOBASEMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("PRECIOTOT-MON-NAL")>  _
    Public Property PRECIOTOTMONNAL() As String
        Get
            Return Me.pRECIOTOTMONNALField
        End Get
        Set
            Me.pRECIOTOTMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property PRECIOTOTAL() As String
        Get
            Return Me.pRECIOTOTALField
        End Get
        Set
            Me.pRECIOTOTALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property SUBTOTAL() As String
        Get
            Return Me.sUBTOTALField
        End Get
        Set
            Me.sUBTOTALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("SUBTOTAL-MON-NAL")>  _
    Public Property SUBTOTALMONNAL() As String
        Get
            Return Me.sUBTOTALMONNALField
        End Get
        Set
            Me.sUBTOTALMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("VTA-REFACC-MON-NAL")>  _
    Public Property VTAREFACCMONNAL() As String
        Get
            Return Me.vTAREFACCMONNALField
        End Get
        Set
            Me.vTAREFACCMONNALField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("VTA-REFACCIONES")>  _
    Public Property VTAREFACCIONES() As String
        Get
            Return Me.vTAREFACCIONESField
        End Get
        Set
            Me.vTAREFACCIONESField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanTotalsTasas
    
    Private dCSIVAField As String
    
    Private iVAField As String
    
    Private iVAMONNALField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property DCSIVA() As String
        Get
            Return Me.dCSIVAField
        End Get
        Set
            Me.dCSIVAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property IVA() As String
        Get
            Return Me.iVAField
        End Get
        Set
            Me.iVAField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute("IVA-MON-NAL")>  _
    Public Property IVAMONNAL() As String
        Get
            Return Me.iVAMONNALField
        End Get
        Set
            Me.iVAMONNALField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://repository.edicomnet.com/schemas/mx/cfd/addenda")>  _
Partial Public Class ComprobanteNissanTotalsGastos
    
    Private descripcionField As String
    
    Private importeField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Descripcion() As String
        Get
            Return Me.descripcionField
        End Get
        Set
            Me.descripcionField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Importe() As String
        Get
            Return Me.importeField
        End Get
        Set
            Me.importeField = value
        End Set
    End Property
End Class
