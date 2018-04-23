<%@ WebHandler Language="C#" Class="Image" %>

using System.Web;
using System.Linq;

public class Image : IHttpHandler {

    ArtsDataClassDataContext dataContext1;
    public ArtsDataClassDataContext DataContext { get { return dataContext1; } }

    public void ProcessRequest(HttpContext context) {
        if (context.Request.QueryString["path"] == null) {
            context.Response.StatusCode = 404;
            context.Response.Write("The Image path is empty");
            return;
        }

        string fPath = context.Request.QueryString["path"];
        string[] fPathParts = fPath.Split('/');

        dataContext1 = new ArtsDataClassDataContext();
        Art current = FindDbFileItem(fPathParts[1]);

        byte[] data = current.Data.ToArray();
        context.Response.Clear();
        context.Response.ContentType = "image";
        context.Response.BinaryWrite(data);
    }

    protected Art FindDbFileItem(string file) {
        return
            (from dbItem in DataContext.Arts
             where !dbItem.IsFolder.Value && dbItem.Name == file
             select dbItem).FirstOrDefault();
    }

    public bool IsReusable { get { return false; } }
}