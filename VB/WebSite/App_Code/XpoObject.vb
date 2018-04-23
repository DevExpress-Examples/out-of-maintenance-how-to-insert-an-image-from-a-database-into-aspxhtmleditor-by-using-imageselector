Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata

Namespace PersistentObjects
	<Persistent("Arts")> _
	Public Class ArtsEntity
		Inherits XPCustomObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private lastWriteTime_Renamed As DateTime
		Private name_Renamed As String
		Private id_Renamed As Integer
		Private pid_Renamed As Integer
		Private isFolder_Renamed As Boolean

		Public Property LastWriteTime() As DateTime
			Get
				Return lastWriteTime_Renamed
			End Get
			Set(ByVal value As DateTime)
				SetPropertyValue("LastWriteTime", lastWriteTime_Renamed, value)
			End Set
		End Property
		Public Property Name() As String
			Get
				Return name_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", name_Renamed, value)
			End Set
		End Property
		<Persistent("ID"), Key(AutoGenerate := True)> _
		Public Property Id() As Integer
			Get
				Return id_Renamed
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Id", id_Renamed, value)
			End Set
		End Property
		<Persistent("ParentID")> _
		Public Property Pid() As Integer
			Get
				Return pid_Renamed
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Pid", pid_Renamed, value)
			End Set
		End Property
		Public Property IsFolder() As Boolean
			Get
				Return isFolder_Renamed
			End Get
			Set(ByVal value As Boolean)
				SetPropertyValue("IsFolder", isFolder_Renamed, value)
			End Set
		End Property
		<Delayed(True)> _
		Public Property Data() As Byte()
			Get
				Return GetDelayedPropertyValue(Of Byte())("Data")
			End Get
			Set(ByVal value As Byte())
				SetDelayedPropertyValue(Of Byte())("Data", value)
			End Set
		End Property
	End Class
End Namespace
