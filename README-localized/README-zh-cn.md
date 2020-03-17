---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
- csharp
description: 本示例演示了如何创建 .NET Framework 服务来验证 Exchange 客户端标识令牌。
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
urlFragment: outlook-add-in-validate-a-client-identity-token-using-the-net-framework
---

# Outlook 外接程序：验证客户端标识令牌 (.NET)

**目录**

* [摘要](#summary)
* [先决条件](#prerequisites)
* [示例主要组件](#components)
* [代码说明](#codedescription)
* [构建和调试](#build)
* [疑难解答](#troubleshooting)
* [问题和意见](#questions)
* [其他资源](#additional-resources)

<a name="summary"></a>
## 摘要
本示例演示如何创建 .NET Framework 服务来验证 Exchange 客户端标识令牌。

<a name="prerequisites"></a>
## 先决条件 ##

此示例要求如下：  

  - Visual Studio 2013（更新 5）或 Visual Studio 2015，具有 Microsoft Office 开发人员工具。 
  - 运行至少具有一个电子邮件帐户或 Office 365 帐户的 Exchange 2013 的计算机。你可以[参加 Office 365 开发人员计划并获取为期 1 年的免费 Office 365 订阅](https://aka.ms/devprogramsignup)。
  - 任何支持 ECMAScript 5.1、HTML5 和 CSS3 的浏览器，如 Internet Explorer 9、Chrome 13、Firefox 5、Safari 5.0.6 以及这些浏览器的更高版本。
  - Microsoft.IdentityModel.dll 和 Microsoft.IdentityModel.Extensions.dll。你可以通过程序包管理器控制台安装这些程序包： 
	- 安装程序包 Microsoft.Identity.Model.Extensions
	- 安装程序包 Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4).此外，你还可以使用命令 `Install-Package Microsoft.AspNET.MVC` 将它与 Nuget 安装在一起。
  - 熟悉 JavaScript 编程和 Web 服务。

<a name="components"></a>
## 示例主要组件
本示例解决方案包含以下主要文件：

**IdentityToken** 项目

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml):Outlook 邮件外接程序的清单文件。

**IdentityTokenWeb** 项目

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html):外接程序的 HTML 用户界面。
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js):用于处理请求和使用标识令牌的逻辑。

**IdentityTokenService** 项目

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs):为示例 Web API 服务提供业务逻辑的服务对象。
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs):为 Web API 服务绑定默认路由。
- 模型文件夹  

  | 文件名 | 说明 |
|------|------|
| [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) | 为客户端标识令牌的部件提供标识符的静态逻辑。 |
| [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) | 表示从客户端标识令牌中指定的位置检索的身份验证元数据文档的对象。 |
| [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) | 已根据 RFC 4648 中的规定对 64 位编码的 URL 进行解码的静态对象。 |
| [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) | 提供必须在客户端标识令牌中匹配的字符串值。也提供适合于测试使用的证书验证回调。 |
| [```DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) | 表示从 64 位编码 URL 客户端标识令牌中解码的有效 JSON Web 令牌 (JWT)。如果令牌无效，则 **DecodedJSONToken** 对象的构造函数将会引发 **ApplicationException** 错误。 |
| [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) | 表示已编码和已验证客户端标识令牌的对象。 |
| [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) | 表示来自外接程序的 REST 请求的对象。 |
| [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) | 表示来自 Web 服务的 REST 响应的对象。 |
| [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) | 表示从 Exchange 服务器发送的身份验证元数据文档的对象。 |
| [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) | 用于解码 Outlook 邮件外接程序中的 64 位编码 URL 客户端标识令牌的静态对象。 |

<a name="codedescription"></a>
##代码描述
本示例向你演示如何创建 .NET Framework 服务来验证 Exchange 客户端访问令牌。Exchange 服务器签发一个对服务器上的邮箱具有唯一性的令牌。你可以使用此令牌将邮箱与为 Outlook 邮件外接程序提供的服务关联起来。

本示例分为两个部分：  
- 在电子邮件客户端运行的 Outlook 邮件外接程序。它需要来自 Exchange 服务器中的标识令牌，并将此令牌发送至 Web 服务。
- Web 服务用于验证客户端中的令牌。Web 服务使用令牌内容进行响应，外接程序随后将显示该内容。

Web 服务使用以下步骤处理令牌：  
1.解码标识令牌，以获取 Exchange 服务器身份验证元数据文档的 URL。在此步骤中，服务还会检查令牌是否已过期并检查令牌上的版本号。  
2.如果标识令牌通过第一步，则服务将使用身份验证元数据文档中的信息，以获得用于从服务器签署令牌的证书。  
3.如果令牌有效，则服务会将其返回至 Outlook 邮件外接程序，以便显示。

服务不会通过任何方式使用令牌。它将使用令牌中包含的信息或者错误消息（如果令牌无效）进行响应。 

本示例还要求 X.509 证书验证功能，该功能允许服务响应使用 Exchange 服务器签发的自签名证书签署的请求。默认情况下，Exchange 服务器将使用此自签名证书。如果 Exchange 服务器拥有可追踪到根提供程序的有效证书，则无需使用此验证功能。有关验证功能的更多信息，请参阅[通过 HTTP 验证 X509 SSL 证书](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)。


<a name="build"></a>
## 构建和调试 ##
用户收件箱中的任何电子邮件均会激活外接程序。在运行本示例之前，可以向测试帐户发送一封或多封电子邮件，以此更轻松地测试外接程序。

1. 在 Visual Studio 中打开解决方案，按 F5 构建和部署示例。 
2. 通过为 Exchange 2013 服务器提供电子邮件地址和密码连接至 Exchange 帐户，然后允许服务器配置电子邮件帐户。  
3. 在浏览器中，通过输入帐户名称和密码登录电子邮件帐户。  
4. 选择收件箱中的一封邮件，然后在呈现上述邮件的外接程序栏中单击**验证标识令牌**。  
   外接程序将加载并显示客户端标识令牌的内容。
   
>如果你在使用默认自签名证书的 Exchange 服务器上运行本示例，则在 Web 浏览器打开时，将会收到一条证书错误消息。通过查看 Web 地址确认浏览器打开正确的 URL 之后，选择**继续转到此网站**以启动 Outlook Web App。


<a name="troubleshooting"></a>
## 疑难解答
使用 Outlook Web App 测试 Outlook 邮件外接程序时，你可能会遇到以下问题：

- 选中邮件后，不会显示外接程序栏。如果发生此情况，请在 Visual Studio 窗口中选择**调试 - 停止调试**重启外接程序，然后按 F5 重建并部署外接程序。  
- 部署和运行外接程序时，可能不会记录对 JavaScript 代码的更改。如果更改未记录，请清除 Web 浏览器上的缓存，方法是选择**工具 - Internet 选项**并选择**删除**按钮。删除临时 Internet 文件，然后重启外接程序。
- 如果未看到“引用”下面列出的对 System.Web.Mvc 的引用，请确保已添加它。

如果外接程序已加载但未运行，请尝试在 Visual Studio 中构建解决方案（**构建 > 构建解决方案**）。查看错误列表中是否存在缺失的依赖项，并视需要添加它们。

<a name="questions"></a>
## 问题和意见

- 如果你在运行此示例时遇到任何问题，请[记录问题](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues)。
- 与 Office 外接程序开发相关的问题一般应发布到 [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins)。确保你的问题或意见使用 `Office 外接程序`进行了标记。

<a name="additional-resources"></a>
## 其他资源
- MSDN 上的 [Office 外接程序](https://msdn.microsoft.com/library/office/jj220060.aspx)文档
- [Web API：官方 Microsoft ASP.NET 网站](http://www.asp.net/web-api)  
- [通过 HTTP 验证 X509 SSL 证书](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [使用 Exchange 2013 标识令牌对邮件外接程序进行身份验证](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [验证 Exchange 2013 标识令牌](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [更多外接程序示例](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## 版权信息
版权所有 (c) 2015 Microsoft。保留所有权利。


此项目已采用 [Microsoft 开放源代码行为准则](https://opensource.microsoft.com/codeofconduct/)。有关详细信息，请参阅[行为准则 FAQ](https://opensource.microsoft.com/codeofconduct/faq/)。如有其他任何问题或意见，也可联系 [opencode@microsoft.com](mailto:opencode@microsoft.com)。
