'===============================================================================
' EntitySpaces 2009 by EntitySpaces, LLC
' Persistence Layer and Business Objects for Microsoft .NET
' EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC
' http://www.entityspaces.net
'===============================================================================

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Data
Imports System.ComponentModel
Imports System.Reflection

Imports EntitySpaces.Interfaces
Imports EntitySpaces.Core

Namespace BusinessObjects

    <Serializable()> _ 
    Public MustInherit Class CollectionBase(Of T As {esEntity, New})
        Inherits esEntityCollection(Of T)

        ' For some reason we need this in the VB Version or errors are thrown, this is needed
        ' to make the DebuggerDisplay<> attribute work
        Public Shadows ReadOnly Property Count() As Integer
            Get
                Return MyBase.Count
            End Get
        End Property

		
    End Class
    
End Namespace
