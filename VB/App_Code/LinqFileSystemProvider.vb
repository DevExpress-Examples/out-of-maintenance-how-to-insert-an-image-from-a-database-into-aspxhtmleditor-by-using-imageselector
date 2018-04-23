Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Web
Imports System.IO

Public Class LinqFileSystemProvider
    Inherits FileSystemProviderBase

    Private Const DbRootItemId As Integer = 1


    Private dataContext_Renamed As ArtsDataClassDataContext

    Private folderCache_Renamed As Dictionary(Of Integer, Art)

    Private rootFolderDisplayName_Renamed As String

    Public Sub New(ByVal rootFolder As String)
        MyBase.New(rootFolder)
        Me.dataContext_Renamed = New ArtsDataClassDataContext()
        RefreshFolderCache()
    End Sub

    Public ReadOnly Property DataContext() As ArtsDataClassDataContext
        Get
            Return dataContext_Renamed
        End Get
    End Property

    ' Used to decrease the number of recursive LINQ to SQL queries made to a database
    Public ReadOnly Property FolderCache() As Dictionary(Of Integer, Art)
        Get
            Return folderCache_Renamed
        End Get
    End Property

    Public Overrides ReadOnly Property RootFolderDisplayName() As String
        Get
            Return rootFolderDisplayName_Renamed
        End Get
    End Property

    Public Overrides Function GetFiles(ByVal folder As FileManagerFolder) As IEnumerable(Of FileManagerFile)
        Dim dbFolderItem As Art = FindDbFolderItem(folder)
        Return From dbItem In DataContext.Arts _
               Where (Not dbItem.IsFolder.Value) AndAlso dbItem.ParentID.Value = dbFolderItem.ID _
               Select New FileManagerFile(Me, folder, dbItem.Name, dbItem.ID.ToString())
    End Function
    Public Overrides Function GetFolders(ByVal parentFolder As FileManagerFolder) As IEnumerable(Of FileManagerFolder)
        Dim dbFolderItem As Art = FindDbFolderItem(parentFolder)
        Return From dbItem In FolderCache.Values _
               Where dbItem.IsFolder.Value AndAlso dbItem.ParentID.Value = dbFolderItem.ID _
               Select New FileManagerFolder(Me, parentFolder, dbItem.Name, dbItem.ID.ToString())
    End Function

    Public Overrides Function Exists(ByVal file As FileManagerFile) As Boolean
        Return FindDbFileItem(file) IsNot Nothing
    End Function
    Public Overrides Function Exists(ByVal folder As FileManagerFolder) As Boolean
        Return FindDbFolderItem(folder) IsNot Nothing
    End Function
    Public Overrides Function ReadFile(ByVal file As FileManagerFile) As Stream
        Return New MemoryStream(FindDbFileItem(file).Data.ToArray())
    End Function
    Public Overrides Function GetLastWriteTime(ByVal file As FileManagerFile) As Date
        Dim dbFileItem = FindDbFileItem(file)
        Return dbFileItem.LastWriteTime.GetValueOrDefault(Date.Now)
    End Function

    Protected Function FindDbFileItem(ByVal file As FileManagerFile) As Art
        Dim dbFolderItem As Art = FindDbFolderItem(file.Folder)
        If dbFolderItem Is Nothing Then
            Return Nothing
        End If
        Return ( _
            From dbItem In DataContext.Arts _
            Where dbItem.ParentID.Value = dbFolderItem.ID AndAlso (Not dbItem.IsFolder.Value) AndAlso dbItem.Name = file.Name _
            Select dbItem).FirstOrDefault()
    End Function
    Protected Function FindDbFolderItem(ByVal folder As FileManagerFolder) As Art
        Return ( _
            From dbItem In FolderCache.Values _
            Where dbItem.IsFolder.Value AndAlso GetRelativeName(dbItem) = folder.RelativeName _
            Select dbItem).FirstOrDefault()
    End Function

    ' Returns the path to a specified folder relative to a root folder
    Protected Function GetRelativeName(ByVal dbFolder As Art) As String
        If dbFolder.ID = DbRootItemId Then
            Return String.Empty
        End If
        If dbFolder.ParentID = DbRootItemId Then
            Return dbFolder.Name
        End If
        If Not FolderCache.ContainsKey(dbFolder.ParentID.Value) Then
            Return Nothing
        End If
        Dim name As String = GetRelativeName(FolderCache(dbFolder.ParentID.Value))
        Return If(name Is Nothing, Nothing, Path.Combine(name, dbFolder.Name))
    End Function

    ' Caches folder names in memory and obtains a root folder's name
    Protected Sub RefreshFolderCache()
        Me.folderCache_Renamed = ( _
            From dbItem In DataContext.Arts _
            Where dbItem.IsFolder.Value _
            Select dbItem).ToDictionary(Function(i) i.ID)

        Me.rootFolderDisplayName_Renamed = ( _
            From dbItem In FolderCache.Values _
            Where dbItem.ID = DbRootItemId _
            Select dbItem.Name).First()
    End Sub
End Class