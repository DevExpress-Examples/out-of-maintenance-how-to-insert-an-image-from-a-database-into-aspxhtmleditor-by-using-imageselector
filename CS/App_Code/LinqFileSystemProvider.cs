using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;
using System.IO;

public class LinqFileSystemProvider : FileSystemProviderBase {
    const int DbRootItemId = 1;

    ArtsDataClassDataContext dataContext;
    Dictionary<int, Art> folderCache;
    string rootFolderDisplayName;

    public LinqFileSystemProvider(string rootFolder)
        : base(rootFolder) {
        this.dataContext = new ArtsDataClassDataContext();
        RefreshFolderCache();
    }

    public ArtsDataClassDataContext DataContext { get { return dataContext; } }

    // Used to decrease the number of recursive LINQ to SQL queries made to a database
    public Dictionary<int, Art> FolderCache { get { return folderCache; } }

    public override string RootFolderDisplayName { get { return rootFolderDisplayName; } }

    public override IEnumerable<FileManagerFile> GetFiles(FileManagerFolder folder) {
        Art dbFolderItem = FindDbFolderItem(folder);
        return
            from dbItem in DataContext.Arts
            where !dbItem.IsFolder.Value && dbItem.ParentID.Value == dbFolderItem.ID
            select new FileManagerFile(this, folder, dbItem.Name, dbItem.ID.ToString());
    }
    public override IEnumerable<FileManagerFolder> GetFolders(FileManagerFolder parentFolder) {
        Art dbFolderItem = FindDbFolderItem(parentFolder);
        return
            from dbItem in FolderCache.Values
            where dbItem.IsFolder.Value && dbItem.ParentID.Value == dbFolderItem.ID
            select new FileManagerFolder(this, parentFolder, dbItem.Name, dbItem.ID.ToString());
    }

    public override bool Exists(FileManagerFile file) {
        return FindDbFileItem(file) != null;
    }
    public override bool Exists(FileManagerFolder folder) {
        return FindDbFolderItem(folder) != null;
    }
    public override Stream ReadFile(FileManagerFile file) {
        return new MemoryStream(FindDbFileItem(file).Data.ToArray());
    }
    public override DateTime GetLastWriteTime(FileManagerFile file) {
        var dbFileItem = FindDbFileItem(file);
        return dbFileItem.LastWriteTime.GetValueOrDefault(DateTime.Now);
    }

    protected Art FindDbFileItem(FileManagerFile file) {
        Art dbFolderItem = FindDbFolderItem(file.Folder);
        if (dbFolderItem == null)
            return null;
        return
            (from dbItem in DataContext.Arts
             where dbItem.ParentID.Value == dbFolderItem.ID && !dbItem.IsFolder.Value && dbItem.Name == file.Name
             select dbItem).FirstOrDefault();
    }
    protected Art FindDbFolderItem(FileManagerFolder folder) {
        return
            (from dbItem in FolderCache.Values
             where dbItem.IsFolder.Value && GetRelativeName(dbItem) == folder.RelativeName
             select dbItem).FirstOrDefault();
    }

    // Returns the path to a specified folder relative to a root folder
    protected string GetRelativeName(Art dbFolder) {
        if (dbFolder.ID == DbRootItemId) return string.Empty;
        if (dbFolder.ParentID == DbRootItemId) return dbFolder.Name;
        if (!FolderCache.ContainsKey(dbFolder.ParentID.Value)) return null;
        string name = GetRelativeName(FolderCache[dbFolder.ParentID.Value]);
        return name == null ? null : Path.Combine(name, dbFolder.Name);
    }

    // Caches folder names in memory and obtains a root folder's name
    protected void RefreshFolderCache() {
        this.folderCache = (
            from dbItem in DataContext.Arts
            where dbItem.IsFolder.Value
            select dbItem
        ).ToDictionary(i => i.ID);

        this.rootFolderDisplayName = (
            from dbItem in FolderCache.Values
            where dbItem.ID == DbRootItemId
            select dbItem.Name).First();
    }
}