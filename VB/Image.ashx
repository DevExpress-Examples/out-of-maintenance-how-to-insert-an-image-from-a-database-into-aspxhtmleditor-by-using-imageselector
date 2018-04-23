<%@ WebHandler Language="vb" Class="Image" %>

Imports System.Web
Imports System.Linq

Public Class Image
    Implements IHttpHandler

    Dim dataContext1 As ArtsDataClassDataContext
    Public ReadOnly Property DataContext() As ArtsDataClassDataContext
        Get
            Return dataContext1
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If context.Request.QueryString("path") Is Nothing Then
            context.Response.StatusCode = 404
            context.Response.Write("The Image path is empty")
            Return
        End If

        Dim fPath As String = context.Request.QueryString("path")
        Dim fPathParts() As String = fPath.Split("/"c)

        dataContext1 = New ArtsDataClassDataContext()
        Dim current As Art = FindDbFileItem(fPathParts(1))

        Dim data() As Byte = current.Data.ToArray()
        context.Response.Clear()
        context.Response.ContentType = "image"
        context.Response.BinaryWrite(data)
    End Sub

    Protected Function FindDbFileItem(ByVal file As String) As Art
        Return ( _
            From dbItem In DataContext.Arts _
            Where (Not dbItem.IsFolder.Value) AndAlso dbItem.Name = file _
            Select dbItem).FirstOrDefault()
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class