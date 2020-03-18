---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
- csharp
description: Este exemplo mostra como criar um serviço .NET Framework que valida os tokens de identidade do cliente do Exchange.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
urlFragment: outlook-add-in-validate-a-client-identity-token-using-the-net-framework
---

# Suplemento do Outlook: Validar token de identidade do cliente (.NET)

**Sumário**

* [Resumo](#summary)
* [Pré-requisitos](#prerequisites)
* [Componentes principais do exemplo](#components)
* [Descrição do código](#codedescription)
* [Criar e depurar](#build)
* [Solução de problemas](#troubleshooting)
* [Perguntas e comentários](#questions)
* [Recursos adicionais](#additional-resources)

<a name="summary"></a>
## Resumo
Esse exemplo mostra como criar um serviço do .NET Framework que valida tokens de identidade de cliente do Exchange.

<a name="prerequisites"></a>
## Pré-requisitos ##

Esse exemplo requer o seguinte:  

  - Visual Studio 2013 (Atualização 5) ou Visual Studio 2015 com as Ferramentas de Desenvolvedor do Microsoft Office. 
  - Um computador executando o Exchange 2013 com pelo menos uma conta de e-mail ou uma conta do Office 365. Você pode [participar do Programa de Desenvolvedores do Office 365 e obter uma assinatura gratuita de um ano do Office 365](https://aka.ms/devprogramsignup).
  - Qualquer navegador que ofereça suporte a ECMAScript 5.1, HTML5 e CSS3, como o Internet Explorer 9, o Chrome 13, o Firefox 5, o Safari 5.0.6 ou uma versão posterior desses navegadores.
  - Microsoft.IdentityModel.dll e Microsoft.IdentityModel.Extensions.dll. Você pode instalá-los no Console do Gerenciador de Pacotes: 
	- Instalar Pacote Microsoft.Identity.Model.Extensions
	- Instalar Pacote Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4). Você também pode instalar isso com nuget usando o comando `Instalar Pacote Microsoft.AspNET.MVC`.
  - Familiaridade com programação em JavaScript e serviços Web.

<a name="components"></a>
## Componentes principais do exemplo
A solução de exemplo contém os seguintes arquivos chave:

Projeto **IdentityToken** 

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml): O arquivo de manifesto do suplemento de e-mail do Outlook.

Projeto **IdentityTokenWeb** 

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html): Interface do usuário HTML para o suplemento.
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js): A lógica que manipula a solicitação e o uso do token de identidade.

Projeto **IdentityTokenService** 

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs): O objeto de serviço que fornece a lógica de negócios para o exemplo de serviço da API da Web.
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs): Vincula o roteamento padrão para o serviço de API da Web.
- Pasta de modelos  

  | Nome do arquivo | Descrição |
|------|------|
| [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) | O objeto estático que fornece identificadores para as partes do token de identidade do cliente. |
| [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) | O objeto que representa o documento de metadados de autenticação recuperado do local especificado no token de identidade do cliente. |
| [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) | O objeto estático que decodifica um URL codificado como base na URL 64, conforme especificado no RFC 4648\. |
| |
| [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) | Fornece valores de cadeia de caracteres que devem corresponder ao token de identidade do cliente. Também fornece um retorno de chamada de validação de certificado adequado para uso em teste. |
| [``` DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) | Representa um Token Web JSON (JWT) válido e decodificado a partir do token de identidade do cliente codificado na URL com base 64. Se o token não for válido, o construtor do objeto **DecodedJSONToken** exibirá um erro **ApplicationException**. |
| [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) | O objeto que representa o token de identidade do cliente decodificado e validado. |
| [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) | O objeto que representa a solicitação REST do suplemento. | [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) | O objeto que representa a resposta REST do serviço da web. |
| [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) | O objeto que representa o documento de metadados de autenticação enviado do servidor Exchange. |
| [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) | O objeto estático que decodifica o token de identidade do cliente codificado na URL com base 64 dos e-mails adicionados ao Outlook.

<a name="codedescription"></a>
##Descrição
do código Este exemplo mostra como criar um serviço .NET Framework que valida um token de acesso do cliente do Exchange. O servidor do Exchange emite um token exclusivo para a caixa de correio no servidor. Você pode usar esse token para associar uma caixa de correio com serviços fornecidos a um suplemento do Outlook.

O exemplo é dividido em duas partes:  
– Um suplemento de e-mail do Outlook que é executado em seu cliente de e-mail. Ele solicita um token de identidade do servidor Exchange e envia esse token para o serviço Web.
– Um serviço Web que valida o token do cliente. O serviço Web responde com o conteúdo do token, onde o suplemento é exibido.

O serviço Web usa as seguintes etapas para processar o token:  
1. Decodifica o token de identidade para obter a URL do documento de metadados de autenticação do servidor Exchange. Durante essa etapa, o serviço também verifica se o token expirou e o número da versão no token.  
2. Se o token de identidade passar pela primeira etapa, o serviço usará as informações no documento de metadados de autenticação para obter o certificado usado para assinar o token do servidor.  
3. Se o token for válido, o serviço retornará ao suplemento do e-mail do Outlook para exibição.

O serviço não usa o token de nenhuma forma. Ele responde com as informações contidas no token ou com uma mensagem de erro se o token não for válido. 

Esse exemplo também requer uma função de validação de certificado X.509 que permita ao serviço responder a solicitações assinadas com um certificado autoassinado emitido pelo servidor Exchange. O Exchange Server usará esse certificado autoassinado por padrão. Se o servidor Exchange tiver um certificado válido que rastreie de volta para um provedor raiz, essa função de validação não será necessária. Para obter mais informações sobre a função de validação, confira [Validação de Certificados X509 para SSL por HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx).


<a name="build"></a>
## Criar e depurar ##
O suplemento será ativado em qualquer mensagem de e-mail na caixa de entrada do usuário. Você pode facilitar o teste do suplemento enviando uma ou mais mensagens de e-mail para a sua conta de teste antes de executar o exemplo.

1. Após carregar a solução no Visual Studio, pressione F5 para criar e implantar o exemplo. 
2. Conecte-se a uma conta do Exchange fornecendo o endereço de e-mail e a senha de um servidor do Exchange 2013 e permita que o servidor configure a conta de e-mail.  
3. No navegador, faça logon com a conta de e-mail, digitando o nome e a senha da conta.  
4. Selecione uma mensagem na caixa de entrada e clique em **Validar Token de Identidade** na barra de suplementos que é renderizada acima da mensagem.  
   O suplemento carrega e exibe o conteúdo do token de identidade do cliente.
   
>Se você estiver executando o exemplo em um servidor Exchange que usa o certificado autoassinado padrão, receberá um erro de certificado quando o navegador da Web for aberto. Depois de verificar se o navegador está abrindo a URL correta, verificando o endereço da Web, selecione **Continuar neste site** para iniciar o Outlook Web App.


<a name="troubleshooting"></a>
## Solução de problemas
Você pode encontrar os seguintes problemas ao usar o Outlook Web App para testar um suplemento de e-mail do Outlook:

- A barra de suplementos não aparece quando uma mensagem é selecionada. Se isso ocorrer, reinicie o suplemento selecionando **Debug-Stop Debugging** na janela do Visual Studio, em seguida, pressione F5 para recriar e implantar o suplemento.  
- As alterações no código JavaScript podem não ser selecionadas quando você implanta e executa o suplemento. Se as alterações não forem selecionadas, limpe o cache do navegador da Web selecionando **Ferramentas-opções da Internet** e selecionando o botão **Excluir**. Exclua os arquivos temporários da Internet e reinicie o suplemento.
- Se você não vir a referência ao System.Web.Mvc listada em Referências, certifique-se de adicioná-la.

Se o suplemento carregar mas não funcionar, tente criar a solução no Visual Studio (**Compilação > Compilar Solução**). Verifique se faltam dependências na lista de erros e adicione-as conforme necessário.

<a name="questions"></a>
## Perguntas e comentários

- Se você tiver problemas para executar este exemplo, [relate um problema](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues).
- Perguntas sobre o desenvolvimento de Suplementos do Office em geral devem ser postadas no [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Não deixe de marcar as perguntas ou comentários com `office-addins`.

<a name="additional-resources"></a>
## Recursos adicionais
- Documentação de [Suplementos do Office](https://msdn.microsoft.com/library/office/jj220060.aspx) no MSDN
- [API Web: O Site Oficial do Microsoft ASP.NET](http://www.asp.net/web-api)  
- [Validação de Certificados X509 para SSL por HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [Autenticação de um suplemento de e-mail usando tokens de identidade do Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Validação de um token de identidade do Exchange 2013](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [Mais exemplos de Suplementos](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Direitos autorais
Copyright © 2015 Microsoft. Todos os direitos reservados.


Este projeto adotou o [Código de Conduta do Código Aberto da Microsoft](https://opensource.microsoft.com/codeofconduct/). Para saber mais, confira [Perguntas frequentes sobre o Código de Conduta](https://opensource.microsoft.com/codeofconduct/faq/) ou contate [opencode@microsoft.com](mailto:opencode@microsoft.com) se tiver outras dúvidas ou comentários.
