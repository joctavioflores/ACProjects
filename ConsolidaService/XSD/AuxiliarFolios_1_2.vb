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
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
 System.SerializableAttribute(), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios"), _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios", IsNullable:=False)> _
Partial Public Class RepAuxFol

    <System.Xml.Serialization.XmlAttributeAttribute(AttributeName:="schemaLocation", Namespace:="http://www.w3.org/2001/XMLSchema-instance")> _
    Public schemaLocation As String = "www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios " + "http://www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios/AuxiliarFolios_1_2.xsd"

    Private detAuxFolField() As RepAuxFolDetAuxFol

    Private versionField As String

    Private rFCField As String

    Private mesField As String

    Private anioField As Integer

    Private tipoSolicitudField As String

    Private numOrdenField As String

    Private numTramiteField As String

    Private selloField As String

    Private noCertificadoField As String

    Private certificadoField As String

    Public Sub New()
        MyBase.New()
        Me.versionField = "1.2"
    End Sub

    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("DetAuxFol")> _
    Public Property DetAuxFol() As RepAuxFolDetAuxFol()
        Get
            Return Me.detAuxFolField
        End Get
        Set(ByVal value As RepAuxFolDetAuxFol())
            Me.detAuxFolField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Version() As String
        Get
            Return Me.versionField
        End Get
        Set(ByVal value As String)
            Me.versionField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property RFC() As String
        Get
            Return Me.rFCField
        End Get
        Set(ByVal value As String)
            Me.rFCField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Mes() As String
        Get
            Return Me.mesField
        End Get
        Set(ByVal value As String)
            Me.mesField = Value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Anio() As Integer
        Get
            Return Me.anioField
        End Get
        Set(ByVal value As Integer)
            Me.anioField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property TipoSolicitud() As String
        Get
            Return Me.tipoSolicitudField
        End Get
        Set(ByVal value As String)
            Me.tipoSolicitudField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property NumOrden() As String
        Get
            Return Me.numOrdenField
        End Get
        Set(ByVal value As String)
            Me.numOrdenField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property NumTramite() As String
        Get
            Return Me.numTramiteField
        End Get
        Set(ByVal value As String)
            Me.numTramiteField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Sello() As String
        Get
            Return Me.selloField
        End Get
        Set(ByVal value As String)
            Me.selloField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property noCertificado() As String
        Get
            Return Me.noCertificadoField
        End Get
        Set(ByVal value As String)
            Me.noCertificadoField = value
        End Set
    End Property

    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Certificado() As String
        Get
            Return Me.certificadoField
        End Get
        Set(ByVal value As String)
            Me.certificadoField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios")>  _
Partial Public Class RepAuxFolDetAuxFol
    
    Private comprNalField() As RepAuxFolDetAuxFolComprNal
    
    Private comprNalOtrField() As RepAuxFolDetAuxFolComprNalOtr
    
    Private comprExtField() As RepAuxFolDetAuxFolComprExt
    
    Private numUnIdenPolField As String
    
    Private fechaField As String
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("ComprNal")>  _
    Public Property ComprNal() As RepAuxFolDetAuxFolComprNal()
        Get
            Return Me.comprNalField
        End Get
        Set
            Me.comprNalField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("ComprNalOtr")>  _
    Public Property ComprNalOtr() As RepAuxFolDetAuxFolComprNalOtr()
        Get
            Return Me.comprNalOtrField
        End Get
        Set
            Me.comprNalOtrField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlElementAttribute("ComprExt")>  _
    Public Property ComprExt() As RepAuxFolDetAuxFolComprExt()
        Get
            Return Me.comprExtField
        End Get
        Set
            Me.comprExtField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property NumUnIdenPol() As String
        Get
            Return Me.numUnIdenPolField
        End Get
        Set
            Me.numUnIdenPolField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Fecha() As String
        Get
            Return Me.fechaField
        End Get
        Set(ByVal value As String)
            Me.fechaField = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios")>  _
Partial Public Class RepAuxFolDetAuxFolComprNal
    
    Private uUID_CFDIField As String
    
    Private montoTotalField As Decimal
    
    Private rFCField As String
    
    Private metPagoAuxField As String
    
    Private monedaField As String
    
    Private tipCambField As Decimal
    
    Private tipCambFieldSpecified As Boolean
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property UUID_CFDI() As String
        Get
            Return Me.uUID_CFDIField
        End Get
        Set
            Me.uUID_CFDIField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MontoTotal() As Decimal
        Get
            Return Me.montoTotalField
        End Get
        Set
            Me.montoTotalField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property RFC() As String
        Get
            Return Me.rFCField
        End Get
        Set
            Me.rFCField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MetPagoAux() As String
        Get
            Return Me.metPagoAuxField
        End Get
        Set
            Me.metPagoAuxField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Moneda() As String
        Get
            Return Me.monedaField
        End Get
        Set
            Me.monedaField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TipCamb() As Decimal
        Get
            Return Me.tipCambField
        End Get
        Set
            Me.tipCambField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlIgnoreAttribute()>  _
    Public Property TipCambSpecified() As Boolean
        Get
            Return Me.tipCambFieldSpecified
        End Get
        Set
            Me.tipCambFieldSpecified = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios")>  _
Partial Public Class RepAuxFolDetAuxFolComprNalOtr
    
    Private cFD_CBB_SerieField As String
    
    Private cFD_CBB_NumFolField As String
    
    Private montoTotalField As Decimal
    
    Private rFCField As String
    
    Private metPagoAuxField As String
    
    Private monedaField As String
    
    Private tipCambField As Decimal
    
    Private tipCambFieldSpecified As Boolean
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CFD_CBB_Serie() As String
        Get
            Return Me.cFD_CBB_SerieField
        End Get
        Set
            Me.cFD_CBB_SerieField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>  _
    Public Property CFD_CBB_NumFol() As String
        Get
            Return Me.cFD_CBB_NumFolField
        End Get
        Set
            Me.cFD_CBB_NumFolField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MontoTotal() As Decimal
        Get
            Return Me.montoTotalField
        End Get
        Set
            Me.montoTotalField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property RFC() As String
        Get
            Return Me.rFCField
        End Get
        Set
            Me.rFCField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MetPagoAux() As String
        Get
            Return Me.metPagoAuxField
        End Get
        Set
            Me.metPagoAuxField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Moneda() As String
        Get
            Return Me.monedaField
        End Get
        Set
            Me.monedaField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TipCamb() As Decimal
        Get
            Return Me.tipCambField
        End Get
        Set
            Me.tipCambField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlIgnoreAttribute()>  _
    Public Property TipCambSpecified() As Boolean
        Get
            Return Me.tipCambFieldSpecified
        End Get
        Set
            Me.tipCambFieldSpecified = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios")>  _
Partial Public Class RepAuxFolDetAuxFolComprExt
    
    Private numFactExtField As String
    
    Private taxIDField As String
    
    Private montoTotalField As Decimal
    
    Private metPagoAuxField As String
    
    Private monedaField As String
    
    Private tipCambField As Decimal
    
    Private tipCambFieldSpecified As Boolean
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property NumFactExt() As String
        Get
            Return Me.numFactExtField
        End Get
        Set
            Me.numFactExtField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TaxID() As String
        Get
            Return Me.taxIDField
        End Get
        Set
            Me.taxIDField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MontoTotal() As Decimal
        Get
            Return Me.montoTotalField
        End Get
        Set
            Me.montoTotalField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property MetPagoAux() As String
        Get
            Return Me.metPagoAuxField
        End Get
        Set
            Me.metPagoAuxField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Moneda() As String
        Get
            Return Me.monedaField
        End Get
        Set
            Me.monedaField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TipCamb() As Decimal
        Get
            Return Me.tipCambField
        End Get
        Set
            Me.tipCambField = value
        End Set
    End Property
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlIgnoreAttribute()>  _
    Public Property TipCambSpecified() As Boolean
        Get
            Return Me.tipCambFieldSpecified
        End Get
        Set
            Me.tipCambFieldSpecified = value
        End Set
    End Property
End Class

'''<comentarios/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"),  _
 System.SerializableAttribute(),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="www.sat.gob.mx/esquemas/ContabilidadE/1_1/AuxiliarFolios")>  _
Public Enum RepAuxFolMes
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("01")>  _
    Item01
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("02")>  _
    Item02
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("03")>  _
    Item03
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("04")>  _
    Item04
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("05")>  _
    Item05
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("06")>  _
    Item06
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("07")>  _
    Item07
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("08")>  _
    Item08
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("09")>  _
    Item09
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("10")>  _
    Item10
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("11")>  _
    Item11
    
    '''<comentarios/>
    <System.Xml.Serialization.XmlEnumAttribute("12")>  _
    Item12
End Enum
