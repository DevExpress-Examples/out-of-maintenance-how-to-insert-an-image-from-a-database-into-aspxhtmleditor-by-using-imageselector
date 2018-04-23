<%@ WebHandler Language="vb" Class="Image" %>

Imports System
Imports System.Web
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports PersistentObjects

Public Class Image
	Implements IHttpHandler

	Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
		If context.Request.QueryString("path") Is Nothing Then
			Return
		End If

		Dim fPath As String = context.Request.QueryString("path")
		Dim fPathParts() As String = fPath.Split("/"c)

		Dim session As Session = FileManagerDemoXpoHelper.GetNewSession()

		Dim current As ArtsEntity = session.FindObject(Of ArtsEntity)(New BinaryOperator("IsFolder", True) And New BinaryOperator("Pid", -1))
		For i As Integer = 0 To fPathParts.Length - 1
			current = session.FindObject(Of ArtsEntity)(New BinaryOperator("IsFolder", i <> fPathParts.Length - 1) And New BinaryOperator("Pid", current.Id) And New BinaryOperator("Name", fPathParts(i)))
		Next i

		Dim data() As Byte = current.Data
		context.Response.Clear()
		context.Response.ContentType = "image"
		context.Response.BinaryWrite(data)
	End Sub

	Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
		Get
			Return False
		End Get
	End Property
End Class