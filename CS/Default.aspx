<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v15.1, Version=15.1.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v15.1, Version=15.1.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxHtmlEditor ID="ASPxHtmlEditor1" runat="server">
                <SettingsDialogs>
                    <InsertImageDialog>
                        <SettingsImageSelector Enabled="true" ProviderType="Custom" 
                            CustomFileSystemProviderTypeName="LinqFileSystemProvider" RootFolderUrlPath="Image.ashx?path=">
                            <CommonSettings RootFolder="Arts" ThumbnailFolder="~\FileManager\Thumbnails\" UseAppRelativePath="true" />
                        </SettingsImageSelector>
                    </InsertImageDialog>
                </SettingsDialogs>
            </dx:ASPxHtmlEditor>
        </div>
    </form>
</body>
</html>
