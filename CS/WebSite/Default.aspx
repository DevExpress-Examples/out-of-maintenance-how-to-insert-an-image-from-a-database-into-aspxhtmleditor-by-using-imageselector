<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v11.1, Version=11.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxHtmlEditor ID="ASPxHtmlEditor1" runat="server">
            <SettingsImageSelector Enabled="true">
            </SettingsImageSelector>
            <SettingsForms InsertImageFormPath="~/DevExpress/ASPxHtmlEditorForms/InsertImageForm.ascx">
            </SettingsForms>
        </dx:ASPxHtmlEditor>
    </div>
    </form>
</body>
</html>
