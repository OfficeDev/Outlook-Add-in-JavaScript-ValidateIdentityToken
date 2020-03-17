---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
- csharp
description: В этом примере показано, как создать службу .NET Framework, которая проверяет маркеры идентификации клиента Exchange.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
urlFragment: outlook-add-in-validate-a-client-identity-token-using-the-net-framework
---

# Надстройка Outlook: Проверка токена удостоверения клиента (.NET)

**Содержание**

* [Сводка](#summary)
* [Предварительные требования](#prerequisites)
* [Ключевые компоненты примера](#components)
* [Описание кода](#codedescription)
* [Сборка и отладка](#build)
* [Устранение неполадок](#troubleshooting)
* [Вопросы и комментарии](#questions)
* [Дополнительные ресурсы](#additional-resources)

<a name="summary"></a>
## Сводка
В этом примере показано, как создать службу .NET Framework, которая проверяет токены удостоверения клиента Exchange.

<a name="prerequisites"></a>
## Предварительные требования ##

Для этого примера требуются приведенные ниже компоненты.  

  - Visual Studio 2013 (обновление 5) или Visual Studio 2015 с инструментами разработчика Microsoft Office. 
  - Компьютер с Exchange 2013 и по крайней мере одной учетной записью электронной почты или учетной записью Office 365. Вы можете [присоединиться к Программе разработчика Office 365 и получить бесплатную годовую подписку на Office 365](https://aka.ms/devprogramsignup).
  - Любой браузер, поддерживающий ECMAScript 5.1, HTML5 и CSS3, например Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6 или более поздние версии этих браузеров.
  - Microsoft.IdentityModel.dll и Microsoft.IdentityModel.Extensions.dll. Вы можете установить их из консоли диспетчера пакетов: 
	- Установить-пакет Microsoft.Identity.Model.Extensions
	- Установочный пакет Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4). Вы также можете установить это с помощью nuget, используя команду `Install-Package Microsoft.AspNET.MVC`.
  - Опыт программирования на JavaScript и работы с веб-службами.

<a name="components"></a>
## Ключевые компоненты примера
Пример решения содержит следующие ключевые файлы:

Объект **IdentityToken**

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml): Файл манифеста для почтовой надстройки для Outlook.

**IdentityTokenWeb** проект

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html): Пользовательский интерфейс HTML для надстройки.
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js): Логика, которая обрабатывает запрос и использование токена идентификации.

**IdentityTokenService** проект

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs): Сервисный объект, который предоставляет бизнес-логику для примера сервиса Web API.
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs): Связывает маршрутизацию по умолчанию для службы Web API.
- Папка "модели"  

  | Имя файла | Description |
|------|------|
| [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) | Статический объект, который предоставляет идентификаторы для частей токена удостоверения клиента. |
| [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) | Объект, представляющий документ метаданных проверки подлинности, полученный из расположения, указанного в маркере удостоверения клиента. |
| [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) | Статический объект, декодированный URL-адрес Base-64, закодированный как указанный в RFC 4648\. |
| [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) | Предоставляет строковые значения, которые должны быть сопоставлены с маркером удостоверения клиента. Также предоставляет обратный вызов проверки сертификата, который можно использовать для проверки. |
| [```DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) | Обозначает действительный веб-маркер JSON (JWT), декодированный с помощью маркера удостоверения клиента Base-64, закодированного URL-адресом. Если токен не является допустимым, конструктор для объекта **DecodedJSONToken** вызывает ошибку **ApplicationException**. |
| [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) | Объект, представляющий декодированную и проверенную лексему удостоверения клиента. |
| [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) | Объект, обозначающий запрос REST в надстройке. |
| [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) | Объект, представляющий отклик веб-службы. |
| [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) | Объект, обозначающий документ метаданных проверки подлинности, отправленный с сервера Exchange. |
| [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) | Статический объект, который декодирует токен удостоверения клиента, закодированного в Base-64, из надстройки "почта" для Outlook. |

<a name="codedescription"></a>
## Описание
кода В этом примере показано, как создать службу .NET Framework, которая проверяет токен доступа клиента Exchange. Сервер Exchange выдает токен, который является уникальным для почтового ящика на сервере. Вы можете использовать этот токен, чтобы связать почтовый ящик со службами, которые вы предоставляете, с почтовой надстройкой для Outlook.

Образец делится на две части:  
- Почтовая надстройка для Outlook, которая работает в вашем почтовом клиенте. Он запрашивает токен идентификации с сервера Exchange и отправляет этот токен веб-службе.
- Веб-сервис, который проверяет токен от клиента. Веб-служба отвечает содержимым токена, который затем отображает надстройка.

Веб-служба использует следующие шаги для обработки токена:  
1. Декодирует идентификационный токен, чтобы получить URL-адрес документа метаданных аутентификации сервера Exchange. На этом этапе служба также проверяет, не истек ли токен, и проверяет номер версии токена.  
2. Если идентификационный токен проходит первый шаг, служба использует информацию в документе метаданных аутентификации, чтобы получить сертификат, который использовался для подписи токена с сервера.  
3. Если токен действителен, служба возвращает его в почтовую надстройку для Outlook для отображения.

Сервис никак не использует токен. Он отвечает информацией, содержащейся в токене, или сообщением об ошибке, если токен недействителен. 

В этом примере также требуется функция проверки сертификата X.509, которая позволяет службе отвечать на запросы, подписанные самозаверяющим сертификатом, выпущенным сервером Exchange. Сервер Exchange будет использовать этот самозаверяющий сертификат по умолчанию. Если ваш сервер Exchange имеет действительный сертификат, который восходит к корневому провайдеру, эта функция проверки не требуется. Для получения дополнительной информации о функции [проверки см. Проверка сертификатов X509 для SSL по HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx).


<a name="build"></a>
## Сборка и отладка ##
Надстройка будет активирована для любого сообщения электронной почты в папке «Входящие» пользователя. Вы можете упростить тестирование надстройки, отправив одно или несколько сообщений электронной почты в свою тестовую учетную запись перед запуском образца.

1. Откройте решение в Visual Studio и нажмите F5, чтобы создать и развернуть образец. 
2. Подключитесь к учетной записи Exchange, указав адрес электронной почты и пароль для сервера Exchange 2013, и разрешите серверу настроить учетную запись электронной почты.  
3. В браузере войдите в систему с учетной записью электронной почты, введя имя учетной записи и пароль.  
4. Выберите сообщение в папке Входящие и нажмите **Подтвердить идентификатор** на панели надстроек над сообщением.  
   Надстройка загружает и отображает содержимое токена идентификации клиента.
   
>Если вы запускаете образец на сервере Exchange, который использует самозаверяющий сертификат по умолчанию, вы получите ошибку сертификата при открытии веб-браузера. Убедившись, что браузер открывает правильный URL-адрес, просмотрев веб-адрес, выберите **Перейти на этот веб-сайт**, чтобы запустить Outlook Web App.


<a name="troubleshooting"></a>
## Устранение неполадок
При использовании Outlook Web App для проверки почтовой надстройки для Outlook могут возникнуть следующие проблемы:

- Панель надстроек не отображается при выборе сообщения. В этом случае перезапустите надстройку, выбрав **Отладка - Остановить отладку** в окне Visual Studio, затем нажмите клавишу F5, чтобы перестроить и развернуть надстройку.  
- Изменения в коде JavaScript могут не учитываться при развертывании и запуске надстройки. Если изменения не получены, очистите кэш в веб-браузере, выбрав **Сервис - Свойства обозревателя** и нажав кнопку **Удалить**. Удалите временные файлы Интернета, а затем перезапустите надстройку.
- Если вы не видите ссылку на System.Web.Mvc в списке «Ссылки», обязательно добавьте ее.

Если надстройка загружается, но не запускается, попробуйте собрать решение в Visual Studio (**Build> Build Solution**). Проверьте список ошибок на наличие недостающих зависимостей и добавьте их при необходимости.

<a name="questions"></a>
## Вопросы и комментарии

- Если у вас возникли проблемы с запуском этого примера, [сообщите о неполадке](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues).
- Вопросы о разработке надстроек Office в целом следует размещать в [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Убедитесь в том, что ваши вопросы и комментарии помечены `Office надстройки`.

<a name="additional-resources"></a>
## Дополнительные ресурсы
- [надстройки Office](https://msdn.microsoft.com/library/office/jj220060.aspx) документации в MSDN
- [Web API: Официальный сайт Microsoft ASP.NET](http://www.asp.net/web-api)  
- [Проверка сертификатов X509 для SSL через HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [Аутентификация почтовой надстройки с помощью токенов Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Проверка токена удостоверения Exchange 2013](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [Дополнительные примеры надстроек](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Авторские права
(c) Корпорация Майкрософт (Microsoft Corporation), 2015. Все права защищены.


Этот проект соответствует [Правилам поведения разработчиков открытого кода Майкрософт](https://opensource.microsoft.com/codeofconduct/). Дополнительные сведения см. в разделе [часто задаваемых вопросов о правилах поведения](https://opensource.microsoft.com/codeofconduct/faq/). Если у вас возникли вопросы или замечания, напишите нам по адресу [opencode@microsoft.com](mailto:opencode@microsoft.com).
