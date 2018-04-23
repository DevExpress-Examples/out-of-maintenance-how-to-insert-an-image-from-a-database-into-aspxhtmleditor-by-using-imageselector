Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata
Imports System.Threading

Public Class FileManagerDemoXpoHelper
	Public Shared Function GetNewSession() As Session
		Return New Session(DataLayer)
	End Function

	Private ReadOnly Shared _lockObject As Object = New Object()
	Private Shared _dataLayer As Object
	Private Shared ReadOnly Property DataLayer() As IDataLayer
		Get
			If _dataLayer Is Nothing Then
				SyncLock _lockObject
					If Thread.VolatileRead(_dataLayer) Is Nothing Then
						Thread.VolatileWrite(_dataLayer, GetDataLayer())
					End If
				End SyncLock
			End If
			Return CType(_dataLayer, IDataLayer)
		End Get
	End Property

	Private Shared Function GetDataLayer() As IDataLayer
		XpoDefault.Session = Nothing
		Dim conn As String = AccessConnectionProvider.GetConnectionString("|DataDirectory|\Data.mdb")
		Dim dict As XPDictionary = New DevExpress.Xpo.Metadata.ReflectionDictionary()
		Dim store As IDataStore = XpoDefault.GetConnectionProvider(conn, AutoCreateOption.SchemaAlreadyExists)
		dict.GetDataStoreSchema(GetType(PersistentObjects.ArtsEntity).Assembly)

		Dim dl As IDataLayer = New ThreadSafeDataLayer(dict, store)
		Return dl
	End Function
End Class
