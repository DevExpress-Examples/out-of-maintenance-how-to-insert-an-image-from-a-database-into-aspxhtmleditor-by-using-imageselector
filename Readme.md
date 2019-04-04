<!-- default file list -->
*Files to look at*:

* [LinqFileSystemProvider.cs](./CS/App_Code/LinqFileSystemProvider.cs) (VB: [LinqFileSystemProvider.vb](./VB/App_Code/LinqFileSystemProvider.vb))
* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
* [Image.ashx](./CS/Image.ashx) (VB: [Image.ashx](./VB/Image.ashx))
<!-- default file list end -->
# How to insert an image from a database into ASPxHtmlEditor by using ImageSelector


<p>This example illustrates how to insert an image stored in a database into the ASPxHtmlEditor form by using ImageSelector.</p>
<p>ASPxHtmlEditor does not have a built-in capability to show binary data stored in a database. Therefore, it is necessary to define a custom handler, which dynamically gets an image from the database and sends it to the user as the handler's response . For this purpose the ASHX generic handler is used. You can learn more about HTTP Handlers here: <a href="http://msdn.microsoft.com/en-us/library/bb398986.aspx"><u>HTTP Handlers and HTTP Modules Overview</u></a>.</p>
<p>In the current example images are inserted into the ASPxHtmlEditor form via ImageSelector in the following format:</p>


```aspx
<img src="Image.ashx?path=Salvador Dali/1910 - 1927/CrepuscularOldMan.jpg" alt="" />
```


<p> </p>
<p><strong>Update for version 15.1.8 and newer versions<br><br></strong>Starting from version 15.1.8, we have added the capability to operate files and folders that are stored in a physical file system, or a database, or cloud services in <a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxHtmlEditorASPxHtmlEditortopic">ASPxHtmlEditor's</a> built-in <a href="https://documentation.devexpress.com/AspNet/clsDevExpressWebASPxFileManagertopic.aspx">ASPxFileManager</a> control. <br>The <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxHtmlEditorHtmlEditorFileManagerSettingsBase_ProviderTypetopic">ProviderType</a> property is used to specify the type of a storage where a current file system is contained. See the <a href="https://documentation.devexpress.com/AspNet/CustomDocument9905.aspx">File System Providers Overview</a> topic to learn more. <br><br>To accomplish this task, create your own File System Provider, assign its name to the CustomFileSystemProviderTypeName property and set the ProviderType property to Custom, so the built-in ASPxFileManager control will operate a custom File System Provider.<br>Use the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxHtmlEditorHtmlEditorSelectorSettings_RootFolderUrlPathtopic">RootFolderUrlPath</a> property to set the path to an image handler. In this handler, get the image data and write it into Response. </p>
<p><br><strong>For older versions:</strong></p>
<p>To accomplish the described task, it is necessary to execute the following steps:</p>
<p>1) Open the SelectImageForm.ascx.cs page;<br> 2) Comment or delete the following code to avoid incorrect RootFolder setting:</p>


```cs
protected void PrepareFileManager() { //FileManager.Settings.Assign(HtmlEditor.SettingsImageSelector.CommonSettings); //if(string.IsNullOrEmpty(FileManager.Settings.RootFolder))          //  FileManager.Settings.RootFolder = HtmlEditor.SettingsImageUpload.UploadImageFolder; ... } 

```


<p>3) Change the FileManager_CustomJSProperties event to set the path to an image handler:</p>


```cs
protected void FileManager_CustomJSProperties(object sender, DevExpress.Web.ASPxClasses.CustomJSPropertiesEventArgs e) {
    e.Properties["cp_RootFolderRelativePath"] = "Image.ashx?path=";
}
```


<p>4) Get an image from a database and show it in the ASPxHtmlEditor form :<br>   a) Create an image handler (*.ashx ); <br>   b) In this handler, parse Request.QueryString and get an image from a database by using the parsed path;<br>   c) Write the obtained image into Response.</p>

<br/>


