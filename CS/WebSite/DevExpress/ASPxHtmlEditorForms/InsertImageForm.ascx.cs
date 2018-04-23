using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxHtmlEditor.Localization;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxUploadControl;

public partial class InsertImageForm : HtmlEditorUserControl {

    protected override void PrepareChildControls() {
        ckbSaveToServer.ClientVisible = false;
        rblFromThisComputer.ClientEnabled = !string.IsNullOrEmpty(HtmlEditor.SettingsImageUpload.UploadImageFolder);
        uplImage.UploadMode = HtmlEditor.SettingsImageUpload.UseAdvancedUploadMode ? UploadControlUploadMode.Advanced : UploadControlUploadMode.Standard;
        Localize();

        base.PrepareChildControls();

        bool imageSelectorEnabled = HtmlEditor.SettingsImageSelector.Enabled;
        txbInsertImageUrl.Buttons[0].Visible = imageSelectorEnabled;
        BrowsePopup.Visible = imageSelectorEnabled;
    }
    void Localize() {
        rblFromTheWeb.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_FromWeb);
        rblFromThisComputer.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_FromLocal);
        lblImageUrl.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_EnterUrl) + ":";
        txbInsertImageUrl.ValidationSettings.RequiredField.ErrorText = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.RequiredFieldError);
        ckbSaveToServer.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_SaveToServer);
        lblBrowse.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_UploadInstructions) + ":";
        ckbMoreImageOptions.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_MoreOptions);
        btnInsertImage.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonInsert);
        btnChangeImage.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonChange);
        btnInsertImageCancel.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonCancel);
        BrowsePopup.HeaderText = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.SelectImage);
        txbInsertImageUrl.Buttons[0].ToolTip = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertImage_SelectImage);
        dxHiddenField.Add("RequiredFieldError", ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.RequiredFieldError));
    }
    protected override ASPxEditBase[] GetChildDxEdits() {
        return new ASPxEditBase[] {
			rblFromTheWeb, rblFromThisComputer,
			txbInsertImageUrl, 
            ckbSaveToServer, 
            ckbMoreImageOptions
        };
    }
    protected override ASPxButton[] GetChildDxButtons() {
        return new ASPxButton[] { btnInsertImage, btnChangeImage, btnInsertImage, btnInsertImageCancel };
    }
    protected override ASPxHtmlEditorRoundPanel GetChildDxHtmlEditorRoundPanel() {
        return rpInsertImage;
    }

    protected string SaveUploadFile() {
        string fileName = "";
        if(HasFile()) {
            string uploadFolder = HtmlEditor.SettingsImageUpload.UploadImageFolder;
            fileName = MapPath(uploadFolder) + uplImage.UploadedFiles[0].FileName;
            try {
                uplImage.UploadedFiles[0].SaveAs(fileName, false);
            }
            catch(IOException) {
                fileName = MapPath(uploadFolder) + uplImage.GetRandomFileName();
                uplImage.UploadedFiles[0].SaveAs(fileName);
            }
        }
        return Path.GetFileName(fileName);
    }
    protected bool HasFile() {
        return uplImage.UploadedFiles != null && uplImage.UploadedFiles.Length > 0 && uplImage.UploadedFiles[0].FileName != "";
    }
    protected void uplImage_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs args) {
        try {
            args.CallbackData = SaveUploadFile();
        }
        catch(Exception e) {
            args.IsValid = false;
            args.ErrorText = e.Message;
        }
    }
}