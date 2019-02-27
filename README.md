---
topic: sample
products:
- Outlook
- Office 365
languages:
- JavaScript
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
---
# Outlook Add-in: Validate a client identity token using the .NET Framework

**Table of contents**

* [Summary](#summary)
* [Prerequisites](#prerequisites)
* [Key components of the sample](#components)
* [Description of the code](#codedescription)
* [Build and debug](#build)
* [Troubleshooting](#troubleshooting)
* [Questions and comments](#questions)
* [Additional resources](#additional-resources)

<a name="summary"></a>
##Summary
This sample shows how to create a .NET Framework service that validates Exchange client identity tokens.

<a name="prerequisites"></a>
## Prerequisites ##

This sample requires the following:  

  - Visual Studio 2013(Update 5) or Visual Studio 2015, with Microsoft Office Developer Tools. 
  - A computer running Exchange 2013 with at least one email account, or an Office 365 account. You can [join the Office 365 Developer Program and get a free 1 year subscription to Office 365](https://aka.ms/devprogramsignup).
  - Any browser that supports ECMAScript 5.1, HTML5, and CSS3, such as Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6, or a later version of these browsers.
  - Microsoft.IdentityModel.dll and Microsoft.IdentityModel.Extensions.dll. You can install these from the Package Manager Console: 
	- Install-Package Microsoft.Identity.Model.Extensions
	- Install-Package Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4). You can also install this with nuget using the command `Install-Package Microsoft.AspNET.MVC`.
  - Familiarity with JavaScript programming and web services.

<a name="components"></a>
## Key components of the sample
The sample solution contains the following key files:

**IdentityToken** project

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml): The manifest file for the mail add-in for Outlook.

**IdentityTokenWeb** project

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html): The HTML user interface for the add-in.
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js): The logic that handles requesting and using the identity token.

**IdentityTokenService** project

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs): The service object that provides the business logic for the sample Web API service.
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs): Binds the default routing for the Web API service.
- Models folder  

  | File name | Description |
  |------|------|
  | [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) |  The static object that provides identifiers for the parts of the client identity token. |
  | [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) |  The object that represents the authentication metadata document retrieved from the location specified in the client identity token. |
  | [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) |  The static object that decodes a URL that has been base-64 URL-encoded, as specified in RFC 4648. |
  | [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) |  Provides string values that must be matched in the client identity token. Also provides a certificate validation callback suitable for test use. |
  | [```DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) |  Represents a valid JSON Web Token (JWT) decoded from the base-64 URL-encoded client identity token. If the token is not valid, the constructor for the **DecodedJSONToken** object will throw an **ApplicationException** error. | 
  | [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) |  The object that represents the decoded and validated client identity token. | 
  | [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) |  The object that represents the REST request from the add-in. | 
  | [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) |  The object that represents the REST response from the web service. | 
  | [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) |  The object that represents the authentication metadata document sent from the Exchange server. |
  | [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) |  The static object that decodes the base-64 URL-encoded client identity token from the mail add-in for Outlook. |

<a name="codedescription"></a>
##Description of the code
This sample shows you how to create a .NET Framework service that validates an Exchange client access token. The Exchange server issues a token that is unique to the mailbox on the server. You can use this token to associate a mailbox with services that you provide to a mail add-in for Outlook.

The sample is divided into two parts:  
- A mail add-in for Outlook that runs in your email client. It requests an identity token from the Exchange server and sends this token to the web service.
- A web service that validates the token from the client. The web service responds with the contents of the token, which the add-in then displays.

The web service uses the following steps to process the token:  
1. Decodes the identity token to get the URL for the Exchange server's authentication metadata document. During this step, the service also checks whether the token has expired and checks the version number on the token.  
2. If the identity token passes the first step, the service uses the information in the authentication metadata document to get the certificate that was used to sign the token from the server.  
3. If the token is valid, the service returns it to the mail add-in for Outlook for display.

The service does not use the token in any way. It responds with the information contained in the token, or with an error message if the token is not valid. 

This sample also requires an X.509 certificate validation function that allows the service to respond to requests that are signed with a self-signed certificate issued by the Exchange server. The Exchange server will use this self-signed certificate by default. If your Exchange server has a valid certificate that traces back to a root provider, this validation function is not required. For more information about the validation function, see  [Validating X509 Certificates for SSL over HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx).


<a name="build"></a>
## Build and debug ##
The add-in will be activated on any email message in the user's Inbox. You can make it easier to test the add-in by sending one or more email messages to your test account before you run the sample.

1. Open the solution in Visual Studio, and press F5 to build and deploy the sample. 
2. Connect to an Exchange account by providing the email address and password for an Exchange 2013 server, and allow the server to configure the email account.  
3. In the browser, log on with the email account by entering the account name and password.  
4. Select a message in the Inbox, and click **Validate Identity Token** in the add-in bar that renders above the message.  
   The add-in loads and displays the contents of the client identity token.
   
>If you're running the sample on an Exchange server that's using the default self-signed certificate, you'll get a certificate error when the web browser opens. After you verify that the browser is opening the correct URL by looking at the web address, select **Continue to this Web site** to start Outlook Web App.


<a name="troubleshooting"></a>
## Troubleshooting
You might encounter the following issues when you use Outlook Web App to test a mail add-in for Outlook:

- The add-in bar does not appear when a message is selected. If this occurs, restart the add-in by selecting **Debug - Stop Debugging** in the Visual Studio window, then press F5 to rebuild and deploy the add-in.  
- Changes to the JavaScript code might not be picked up when you deploy and run the add-in. If the changes are not picked up, clear the cache on the web browser by selecting **Tools - Internet options** and selecting the **Delete** button. Delete the temporary Internet files and then restart the add-in.
- If you don't see reference to System.Web.Mvc listed under References, make sure you add it.

If the add-in loads but does not run, try to build the solution in Visual Studio (**Build > Build Solution**). Check the Error List for missing dependencies and add them as needed.

<a name="questions"></a>
## Questions and comments

- If you have any trouble running this sample, please [log an issue](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues).
- Questions about Office Add-in development in general should be posted to [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Make sure that your questions or comments are tagged with `office-addins`.

<a name="additional-resources"></a>
## Additional resources
- [Office Add-ins](https://msdn.microsoft.com/library/office/jj220060.aspx) documentation on MSDN
- [Web API: The Official Microsoft ASP.NET Site](http://www.asp.net/web-api)  
- [Validating X509 Certificates for SSL over HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [Authenticating a mail add-in by using Exchange 2013 identity tokens](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Validate an Exchange 2013 identity token](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [More Add-in samples](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Copyright
Copyright (c) 2015 Microsoft. All rights reserved.


This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
