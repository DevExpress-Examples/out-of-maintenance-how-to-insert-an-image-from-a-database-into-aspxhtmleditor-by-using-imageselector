<%@ WebHandler Language="C#" Class="Image" %>

using System;
using System.Web;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using PersistentObjects;

public class Image : IHttpHandler {

    public void ProcessRequest(HttpContext context) {
        if (context.Request.QueryString["path"] == null)
            return;
        
        string fPath = context.Request.QueryString["path"];
        string[] fPathParts = fPath.Split('/');
        
        Session session = FileManagerDemoXpoHelper.GetNewSession();

        ArtsEntity current = session.FindObject<ArtsEntity>(new BinaryOperator("IsFolder", true) & new BinaryOperator("Pid", -1));
        for (int i = 0; i < fPathParts.Length; i++) {
            current = session.FindObject<ArtsEntity>(
                new BinaryOperator("IsFolder", i != fPathParts.Length - 1)
                & new BinaryOperator("Pid", current.Id)
                & new BinaryOperator("Name", fPathParts[i])
            );
        }

        byte[] data = current.Data;
        context.Response.Clear();
        context.Response.ContentType = "image";
        context.Response.BinaryWrite(data);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}