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
    Public MustInherit Class QueryBase
        Inherits esDynamicQuery
		
    End Class
    
End Namespace