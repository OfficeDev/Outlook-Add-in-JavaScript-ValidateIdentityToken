---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
- csharp
description: このサンプルは、Exchange クライアント ID トークンを検証する .NET Framework サービスを作成する方法を示しています。
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
urlFragment: outlook-add-in-validate-a-client-identity-token-using-the-net-framework
---

# Outlook アドイン:クライアント ID トークンを使用する (.NET)

**目次**

* [概要](#summary)
* [前提条件](#prerequisites)
* [サンプルの主要なコンポーネント](#components)
* [コードの説明](#codedescription)
* [ビルドとデバッグ](#build)
* [トラブルシューティング](#troubleshooting)
* [質問とコメント](#questions)
* [その他のリソース](#additional-resources)

<a name="summary"></a>
## 概要
このサンプルでは、Exchange クライアント ID トークンを検証する .NET Framework サービスを作成する方法を示します。

<a name="prerequisites"></a>
## 前提条件 ##

このサンプルを実行するには次のものが必要です。  

  - Visual Studio 2013 (Update 5) または Visual Studio 2015 および Microsoft Office Developer Tools。 
  - 少なくとも 1 つのメール アカウントまたは Office 365 アカウントがある Exchange 2013 を実行するコンピューター。[Office 365 Developer プログラムに参加すると、Office 365 の 1 年間無料のサブスクリプションを取得](https://aka.ms/devprogramsignup)できます。
  - Internet Explorer 9、Chrome 13、Firefox 5、Safari 5.0.6、またはこれらのブラウザーの以降のバージョンなど ECMAScript 5.1、HTML5、および CSS3 をサポートする任意のブラウザー。
  - Microsoft.IdentityModel.dll、および Microsoft.IdentityModel.Extensions.dll。これらは、パッケージ マネージャー コンソールからインストールできます。 
	- Install-Package Microsoft.Identity.Model.Extensions
	- Install-Package Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4)。`Install-Package Microsoft.AspNET.MVC` コマンドを使用して、NuGet でこれをインストールすることもできます。
  - JavaScript プログラミングと Web サービスに精通していること。

<a name="components"></a>
## サンプルの主要なコンポーネント
サンプル ソリューションに含まれる主なファイルは次のとおりです。

**IdentityToken** プロジェクト

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml):Outlook 用メール アドインのマニフェスト ファイル。

**IdentityTokenWeb** プロジェクト

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html):アドインの HTML ユーザー インターフェイス。
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js):ID トークンの要求と使用を処理するロジック。

**IdentityTokenService** プロジェクト

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs):サンプル Web API サービスのビジネス ロジックを提供するサービス オブジェクト。
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs):Web API サービスの既定のルーティングをバインドします。
- モデル フォルダー  

  | ファイル名 | 説明 |
|------|------|
| [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) | クライアント ID トークンの一部の識別子を提供する静的オブジェクト。|
| [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) | クライアント ID トークンで指定された場所から取得された認証メタデータ ドキュメントを表すオブジェクト。|
| [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) | RFC 4648 で指定されているように、base-64 で URL エンコードされた URL をデコードする静的オブジェクト。|
| [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) | クライアント ID トークンで一致する必要がある文字列値を提供します。テストでの使用に適した証明書の検証コールバックも提供します。|
| [```DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) | base-64 で URL エンコードされたクライアント ID トークンからデコードされた有効な JSON Web トークン (JWT) を表します。トークンが有効でない場合、**DecodedJSONToken** オブジェクトのコンストラクターは **ApplicationException** エラーをスローします。|
| [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) | デコードおよび検証されたクライアント ID トークンを表すオブジェクト。|
| [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) | アドインからの REST 要求を表すオブジェクト。|
| [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) | Web サービスからの REST 応答を表すオブジェクト。|
| [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) | Exchange サーバーから送信された認証メタデータ ドキュメントを表すオブジェクト。|
| [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) | Outlook 用メール アドインから base-64 で URL エンコードされたクライアント ID トークンをデコードする静的オブジェクト。|

<a name="codedescription"></a>
##コードの説明
このサンプルでは、Exchange クライアント アクセス トークンを検証する .NET Framework サービスを作成する方法を示します。サーバーのメールボックスに一意のトークンが Exchange サーバーにより発行されます。このトークンを使用して、メールボックスを、Outlook 用メール アドインに提供するサービスに関連付けることができます。

このサンプルは次の 2 つに分けられます。  
- メール クライアントで実行される Outlook 用メール アドイン。Exchange サーバーからの ID トークンを要求し、このトークンを Web サービスに送信します。
-クライアントからのトークンを検証する Web サービス。Web サービスはトークンのコンテンツで応答し、アドインが表示されます。

Web サービスは、次の手順を使用してトークンを処理します。  
1.ID トークンをデコードして、Exchange サーバーの認証メタデータ ドキュメントの URL を取得します。この手順中に、サービスはトークンの有効期限が切れているかどうか、およびトークンのバージョン番号も確認します。  
2.ID トークンが最初の手順を通過すると、サービスは認証メタデータ ドキュメントの情報を使用して、サーバーからトークンに署名するために使用された証明書を取得します。  
3.トークンが有効な場合、サービスは表示用の Outlook 用メール アドインにトークンを返します。

サービスは、トークンを一切使用しません。トークンに含まれる情報で、またトークンが有効でない場合にはエラー メッセージで応答します。 

このサンプルには、サービスが Exchange サーバーによって発行された自己署名証明書で署名された要求に応答できるようにする X.509 証明書検証機能も必要です。Exchange サーバーは、既定でこの自己署名証明書を使用します。Exchange サーバーにルート プロバイダーまでさかのぼる有効な証明書がある場合、この検証機能は必要ありません。検証機能の詳細については、「[Validating X509 Certificates for SSL over HTTP (SSL over HTTP の X509 証明書の検証)](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)」を参照してください。


<a name="build"></a>
## ビルドとデバッグ ##
アドインは、ユーザーの受信トレイのすべてのメール メッセージで有効になります。サンプルを実行する前に、1 つまたは複数のメール メッセージをテスト アカウントに送信しておくと、アドインを簡単にテストできます。

1. ソリューションを Visual Studio で開き、F5 キーを押してサンプルをビルドして展開します。 
2. Exchange 2013 サーバー用のメール アドレスとパスワードを入力して Exchange アカウントに接続し、メール アカウントを構成することをサーバーに許可します。  
3. ブラウザーで、アカウント名とパスワードを入力して、メール アカウントでログオンします。  
4. 受信トレイでメッセージを選択し、メッセージの上に表示されているアドイン バーにある [**Validate Identity Token (ID トークンを検証する)**] をクリックします。  
   アドインは、クライアント ID トークンの内容を読み込んで表示します。
   
>既定の自己署名証明書を使用している Exchange サーバーでサンプルを実行している場合、Web ブラウザーが開いたときに証明書エラーが発生します。ブラウザが正しい URL を開いていることを Web アドレスを見て確認したら、[**この Web サイトに進む**] を選択して Outlook Web App を起動します。


<a name="troubleshooting"></a>
## トラブルシューティング
Outlook Web App を使用して Outlook 用メール アドインをテストするときに、次の問題が発生する場合があります。

- メッセージが選択されているときに、アドイン バーが表示されない。この問題が発生した場合、Visual Studio のウィンドウで **[デバッグ]、[デバッグの停止]** の順に選択してアドインを再起動し、次に F5 キーを押してアドインを再ビルドして展開します。  
- アドインの展開と実行時に JavaScript コードの変更が認識されないことがある。変更が認識されない場合は、**[ツール]、[インターネット オプション]** の順に選択し、[**削除**] ボタンを選択して Web ブラウザーのキャッシュをクリアします。インターネット一時ファイルを削除してからアドインを再起動します。
- [参照設定] に一覧表示される System.Web.Mvc への参照が表示されない場合は、必ず追加してください。

アドインが読み込まれるものの動作しない場合、ソリューションを Visual Studio でビルドしてみます (**[ビルド] > [ソリューションのビルド]**)。[エラー一覧] を確認して欠落している依存関係がないかどうかを確認し、必要に応じて依存関係を追加します。

<a name="questions"></a>
## 質問とコメント

- このサンプルの実行について問題がある場合は、[問題をログに記録](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues)してください。
- Office アドイン開発全般の質問については、「[Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins)」に投稿してください。質問やコメントには、必ず `office-addins` のタグを付けてください。

<a name="additional-resources"></a>
## その他のリソース
- MSDN 上の [Office アドイン](https://msdn.microsoft.com/library/office/jj220060.aspx) ドキュメント
- [Web API:Microsoft ASP.NET の公式サイト](http://www.asp.net/web-api)  
- [Validating X509 Certificates for SSL over HTTP (SSL over HTTP の X509 証明書の検証)](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [Exchange 2013 ID トークンを使用してメール アドインを認証する](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Validate an Exchange 2013 identity token (Exchange 2013 ID トークンを検証する)](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [その他のアドイン サンプル](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## 著作権
Copyright (c) 2015 Microsoft.All rights reserved.


このプロジェクトでは、[Microsoft オープン ソース倫理規定](https://opensource.microsoft.com/codeofconduct/)が採用されています。詳細については、「[倫理規定の FAQ](https://opensource.microsoft.com/codeofconduct/faq/)」を参照してください。また、その他の質問やコメントがあれば、[opencode@microsoft.com](mailto:opencode@microsoft.com) までお問い合わせください。
