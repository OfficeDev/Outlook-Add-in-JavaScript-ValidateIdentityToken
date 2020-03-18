---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
- csharp
description: Este ejemplo muestra cómo crear un servicio de .NET Framework que valide tokens de identidad de cliente de Exchange.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
urlFragment: outlook-add-in-validate-a-client-identity-token-using-the-net-framework
---

# Complemento de Outlook: Validar un token de identidad de cliente (.NET)

**Tabla de contenido**

* [Resumen](#summary)
* [Requisitos previos](#prerequisites)
* [Componentes clave del ejemplo](#components)
* [Descripción del código](#codedescription)
* [Compilar y depurar](#build)
* [Solución de problemas](#troubleshooting)
* [Preguntas y comentarios](#questions)
* [Recursos adicionales](#additional-resources)

<a name="summary"></a>
## Resumen
Este ejemplo muestra cómo crear un servicio de .NET Framework que valide tokens de identidad de cliente de Exchange.

<a name="prerequisites"></a>
## Requisitos previos ##

Este ejemplo necesita lo siguiente:  

  - Visual Studio 2013 (actualización 5) o Visual Studio 2015, con las herramientas para desarrolladores de Microsoft Office. 
  - Un equipo que ejecute Exchange 2013 y, como mínimo, una cuenta de correo electrónico o una cuenta de Office 365. Puede [participar en el programa para desarrolladores Office 365 y obtener una suscripción gratuita durante 1 año a Office 365](https://aka.ms/devprogramsignup).
  - Cualquier explorador que admita ECMAScript 5.1, HTML5 y CSS3, como Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6 o una versión posterior de estos exploradores.
  - Microsoft.IdentityModel.dll y Microsoft.IdentityModel.Extensions.dll. Puede instalarlos desde la Consola del Administrador de paquetes: 
	- Install-Package Microsoft.Identity.Model.Extensions
	- Install-Package Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4). También puede instalarlo con NuGet usando el comando `Install-Package Microsoft.AspNET.MVC`.
  - Familiaridad con los servicios web y la programación de JavaScript.

<a name="components"></a>
## Componentes clave del ejemplo
La solución de ejemplo contiene los archivos clave siguientes:

Proyecto **IdentityToken**

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml): El archivo de manifiesto para el complemento de correo de Outlook.

Proyecto **IdentityTokenWeb**

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html): La interfaz de usuario HTML para el complemento.
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js): La lógica que controla la solicitud y el uso del token de identidad.

Proyecto **IdentityTokenService**

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs): El objeto de servicio que proporciona la lógica empresarial del servicio Web API de ejemplo.
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs): Enlaza el enrutamiento predeterminado para el servicio de Web API.
- Carpeta de modelos  

  | Nombre de archivo | Descripción |
|------|------|
| [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) | El objeto estático que proporciona los identificadores para las partes del token de autenticación del cliente. |
| [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) | El objeto que representa el documento de autenticación de metadatos recuperado de la ubicación especificada en el token de identidad del cliente. |
| [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) | El objeto estático que descodifica una dirección URL que tiene una codificación URL en base 64, como se indica en RFC 4648\. |
| [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) | Proporciona valores de cadena que deben conciliarse en el token de identidad del cliente. También proporciona una devolución de llamada de validación del certificado apta para el uso de prueba. |
| [```DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) | Representa un token web de JSON (JWT) válido descodificado desde el token de identidad del cliente con codificación URL en base 64. Si el token no es válido, el constructor del objeto **DecodedJSONToken** arrojará un error **ApplicationException**. |
| [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) | El objeto que representa el token de identidad del cliente descodificado y validado. |
| [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) | El objeto que representa la solicitud REST del complemento. |
| [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) | El objeto que representa la respuesta REST del servicio web. |
| [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) | El objeto que representa el documento de autenticación de metadatos enviado desde el servidor Exchange. |
| [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) | El objeto estático que descodifica el token de identidad del cliente con codificación URL en base 64 desde el complemento de correo para Outlook. |

<a name="codedescription"></a>
##Descripción
del código Este ejemplo le muestra cómo crear un servicio de .NET Framework que valide un token de acceso de un cliente de Exchange. El servidor de Exchange envía un token que es único para el buzón del servidor. Puede usar este token para asociar un buzón con servicios que proporciona a un complemento de correo para Outlook.

El ejemplo se divide en dos partes:  
- Un complemento de correo para Outlook que se ejecuta en su cliente de correo electrónico. Solicita un token de identidad al servidor de Exchange y envía este token al servicio web.
- Un servicio web que valida el token desde el cliente. El servicio web responde con el contenido del token, que luego muestra el complemento.

El servicio web emplea los siguientes pasos para procesar el token:  
1. Descodifica el token de identidad para obtener la dirección URL del documento de autenticación de metadatos del servidor de Exchange. Durante este paso, el servicio también comprueba si el token ha expirado y comprueba el número de versión del token.  
2. Si el token de identidad supera el primer paso, el servicio usa la información en el documento de autenticación de metadatos para obtener el certificado que se usó para firmar el token desde el servidor.  
3. Si el token es válido, el servicio lo devuelve al complemento de correo para Outlook para su visualización.

El servicio no utiliza el token en ningún modo. Responde con la información contenida en el token o bien con un mensaje de error si el token no es válido. 

Este ejemplo también requiere una función de validación del certificado X.509 que permite que el servicio responda a las solicitudes firmadas con un certificado autofirmado emitido por el servidor de Exchange. El servidor de Exchange usará este certificado autofirmado de forma predeterminada. Si su servidor de Exchange tiene un certificado válido que rastrea el origen a un proveedor raíz, esta función de validación no es necesaria. Para obtener más información sobre la función de validación, vea [Validación de certificados X509 para SSL sobre HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx).


<a name="build"></a>
## Compilar y depurar ##
El complemento se activará en cualquier mensaje de correo electrónico de la bandeja de entrada del usuario. Puede hacer que sea más fácil probar el complemento enviando uno o más mensajes de correo electrónico a la cuenta de prueba antes de ejecutar el ejemplo.

1. Abra la solución en Visual Studio y seleccione F5 para compilar e implementar el ejemplo. 
2. Conecte a una cuenta de Exchange proporcionando la dirección de correo electrónico y la contraseña de un servidor de Exchange 2013 y permita que el servidor configure la cuenta de correo electrónico.  
3. En el explorador, inicie sesión con la cuenta de correo electrónico escribiendo el nombre de la cuenta y la contraseña.  
4. Seleccione un mensaje de la bandeja de entrada y haga clic en **Validar token de identidad** en la barra del complemento que se representa encima del mensaje.  
   El complemento carga y se muestra el contenido del token de identidad del cliente.
   
>Si está ejecutando el ejemplo en un servidor de Exchange que usa el certificado autofirmado predeterminado, recibirá un error de certificado cuando se abra el explorador web. Luego de que haya comprobado que el navegador está abriendo la dirección URL correcta en la dirección web, seleccione **Continuar a este sitio web** para iniciar Outlook Web App.


<a name="troubleshooting"></a>
## Solución de problemas
Es posible que se produzcan los siguientes problemas al usar Outlook Web App para probar un complemento de correo para Outlook:

- La barra de complemento no aparece cuando se selecciona un mensaje. Si esto ocurre, vuelva a iniciar el complemento seleccionando **Depuración: detener depuración** en la ventana de Visual Studio y presione F5 para recompilar e implementar el complemento.  
- Es posible que los cambios en el código de JavaScript no se hayan recogido al implementar y ejecutar el complemento. Si no se han añadido los cambios, borre la memoria caché en el explorador web. Para ello, seleccione **herramientas: opciones de Internet** y seleccione el botón **eliminar**. Elimine los archivos temporales de Internet y reinicie el complemento.
- Si no ve la referencia a System.Web.Mvc enumerada en Referencias, asegúrese de agregarla.

Si el complemento se carga, pero no se ejecuta, pruebe a crear la solución en Visual Studio (**compilación > compilación de la solución**). Busque en la lista de errores las dependencias que faltan y agréguelas según sea necesario.

<a name="questions"></a>
## Preguntas y comentarios

- Si tiene algún problema para ejecutar este ejemplo, [registre un problema](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues).
- Las preguntas sobre el desarrollo de complementos para Office en general deben enviarse a [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Asegúrese de que sus preguntas o comentarios se etiquetan con `office-addins`.

<a name="additional-resources"></a>
## Recursos adicionales
- Documentación de [complementos de Office](https://msdn.microsoft.com/library/office/jj220060.aspx) sobre MSDN
- [API web: El sitio oficial de Microsoft ASP.NET](http://www.asp.net/web-api)  
- [Validación de certificados X509 para SSL sobre HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [Autenticación de un complemento de correo mediante los tokens de identidad de Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Validar un token de identidad de Exchange 2013](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [Más complementos de ejemplo](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Derechos de autor
Copyright (c) 2015 Microsoft. Todos los derechos reservados.


Este proyecto ha adoptado el [Código de conducta de código abierto de Microsoft](https://opensource.microsoft.com/codeofconduct/). Para obtener más información, vea [Preguntas frecuentes sobre el código de conducta](https://opensource.microsoft.com/codeofconduct/faq/) o póngase en contacto con [opencode@microsoft.com](mailto:opencode@microsoft.com) si tiene otras preguntas o comentarios.
