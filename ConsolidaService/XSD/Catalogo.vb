﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'This source code was auto-generated by xsd, Version=2.0.50727.3038.
'

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://tempuri.org/Catalogo.xsd"),  _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://tempuri.org/Catalogo.xsd", IsNullable:=false)>  _
Partial Public Class Catalogo
    
    Private ctasField() As CatalogoCtas
    
    Private versionField As String
    
    Private rFCField As String
    
    Private totalCtasField As Integer
    
    Private mesField As CatalogoMes
    
    Private anoField As Integer
    
    Public Sub New()
        MyBase.New
        Me.versionField = "1.0"
    End Sub
    
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Ctas")>  _
    Public Property Ctas() As CatalogoCtas()
        Get
            Return Me.ctasField
        End Get
        Set
            Me.ctasField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Version() As String
        Get
            Return Me.versionField
        End Get
        Set
            Me.versionField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property RFC() As String
        Get
            Return Me.rFCField
        End Get
        Set
            Me.rFCField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property TotalCtas() As Integer
        Get
            Return Me.totalCtasField
        End Get
        Set
            Me.totalCtasField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Mes() As CatalogoMes
        Get
            Return Me.mesField
        End Get
        Set
            Me.mesField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Ano() As Integer
        Get
            Return Me.anoField
        End Get
        Set
            Me.anoField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038"),  _
 System.SerializableAttribute(),  _
 System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://tempuri.org/Catalogo.xsd")>  _
Partial Public Class CatalogoCtas
    
    Private codAgrupField As String
    
    Private numCtaField As String
    
    Private descField As String
    
    Private subCtaDeField As String
    
    Private nivelField As Integer
    
    Private naturField As String
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property CodAgrup() As String
        Get
            Return Me.codAgrupField
        End Get
        Set
            Me.codAgrupField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property NumCta() As String
        Get
            Return Me.numCtaField
        End Get
        Set
            Me.numCtaField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Desc() As String
        Get
            Return Me.descField
        End Get
        Set
            Me.descField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property SubCtaDe() As String
        Get
            Return Me.subCtaDeField
        End Get
        Set
            Me.subCtaDeField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Nivel() As Integer
        Get
            Return Me.nivelField
        End Get
        Set
            Me.nivelField = value
        End Set
    End Property
    
    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>  _
    Public Property Natur() As String
        Get
            Return Me.naturField
        End Get
        Set
            Me.naturField = value
        End Set
    End Property
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038"),  _
 System.SerializableAttribute(),  _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://tempuri.org/Catalogo.xsd")>  _
Public Enum CatalogoMes
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("01")>  _
    Item01
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("02")>  _
    Item02
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("03")>  _
    Item03
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("04")>  _
    Item04
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("05")>  _
    Item05
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("06")>  _
    Item06
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("07")>  _
    Item07
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("08")>  _
    Item08
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("09")>  _
    Item09
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("10")>  _
    Item10
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("11")>  _
    Item11
    
    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("12")>  _
    Item12
End Enum
