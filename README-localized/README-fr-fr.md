---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
- csharp
description: Cet exemple montre comment créer un service d’infrastructure .NET qui valide les jetons d’identité du client Exchange.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/13/2015 3:58:49 PM
urlFragment: outlook-add-in-validate-a-client-identity-token-using-the-net-framework
---

# Complément Outlook : Valider le jeton d’identité du client (.NET)

**Table des matières**

* [Résumé](#summary)
* [Conditions préalables](#prerequisites)
* [Composants clés de l’exemple](#components)
* [Description du code](#codedescription)
* [Création et débogage](#build)
* [Résolution des problèmes](#troubleshooting)
* [Questions et commentaires](#questions)
* [Ressources supplémentaires](#additional-resources)

<a name="summary"></a>
## Résumé
Cet exemple décrit comment créer un service .NET Framework qui valide les jetons d’identité client Exchange.

<a name="prerequisites"></a>
## Conditions préalables ##

Cet exemple nécessite les éléments suivants :  

  - Visual Studio 2013 (mise à jour 5) ou Visual Studio 2015, avec les outils de développement Microsoft Office. 
  - Un ordinateur exécutant Exchange 2013 avec au moins un compte de messagerie ou un compte Office 365. Vous pouvez [Participer au programme pour les développeurs Office 365 et obtenir un abonnement gratuit d’un an à Office 365](https://aka.ms/devprogramsignup).
  - Tout navigateur qui prend en charge ECMAScript 5.1, HTML5 et CSS3, tel qu’Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6 ou une version ultérieure de ces navigateurs.
  - Microsoft.IdentityModel.dll et Microsoft.IdentityModel.Extensions.dll. Vous pouvez installer celles-ci à partir de la console Package Manager : 
	- Install-Package Microsoft.Identity.Model.Extensions
	- Install-Package Microsoft.IdentityModel
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4). Vous pouvez également l’installer avec NuGet à l’aide de la commande `installer-package Microsoft.AspNET.MVC`.
  - Être familiarisé avec les services web et de programmation JavaScript.

<a name="components"></a>
## Composants clés de l’exemple
La solution de l’exemple contient les fichiers clés suivants :

Projet **IdentityToken** 

- [```IdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityToken/IdentityTokenManifest/IdentityToken.xml): Fichier manifeste pour le complément courrier pour Outlook.

Projet **IdentityTokenWeb** 

- [```IdentityTokenWeb/AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.html): Interface utilisateur HTML pour le complément.
- [```IdentityTokenWeb/AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenWeb/AppRead/Home/Home.js): Logique gérant les demandes et l’utilisation du jeton d’identité.

Projet **IdentityTokenService** 

- [```IdentityTokenService/Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Controllers/IdentityTokenController.cs): Objet de service qui fournit la logique métier pour l’exemple de service d'API web.
- [```IdentityTokenService/App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/App_Start/WebApiConfig.cs): Lie le routage par défaut pour le service API web.
- Dossier de modèles  

  | Nom de fichier | Description |
|------|------|
| [```AuthClaimTypes.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthClaimTypes.cs) | Objet statique qui fournit des identificateurs pour les parties du jeton d’identité client. |
| [```AuthMetadata.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/AuthMetadata.cs) | Objet qui représente le document de métadonnées d’authentification extrait de l’emplacement spécifié dans le jeton d’identité de client. |
| [```Base64UrlEncoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Base64UrlEncoder.cs) | Objet statique qui décode une URL qui a été encodée URL de base 64, comme spécifié dans la RFC 4648\. |
| [```Config.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/Config.cs) | Fournit des valeurs de chaîne qui doivent être mises en correspondance dans le jeton d’identité de client. Fournit également un rappel de validation de certificat approprié pour l’utilisation des tests. |
| [```DecodedJSONToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/DecodedJsonToken.cs) | Représente un jeton Web JSON valide (JWT) à partir du jeton d’identité de client de base-64 encodé par URL. Si le jeton n’est pas valide, le constructeur de l’objet **DecodedJSONToken** génère une erreur**ApplicationException** . |
| [```IdentityToken.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityToken.cs) | Objet qui représente le jeton d’identité de client décodé et validé. |
| [```IdentityTokenRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenRequest.cs) | Objet qui représente la demande REST du complément. |
| [```IdentityTokenResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/IdentityTokenResponse.cs) | Objet qui représente la réponse REST du service Web. |
| [```JsonAuthMetadataDocument.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonAuthMetadataDocument.cs) | Objet qui représente le document de métadonnées d’authentification envoyé à partir du serveur Exchange. |
| [```JsonTokenDecoder.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/blob/master/IdentityTokenService/Models/JsonTokenDecoder.cs) | Objet statique qui décode le jeton d’identité du client de base-64 encodé par URL à partir du complément courrier pour Outlook. |

<a name="codedescription"></a>
##Description
du code Cet exemple vous montre comment créer un service .NET Framework qui valide un jeton d’accès client Exchange. Le serveur Exchange publie un jeton propre à la boîte aux lettres sur le serveur. Vous pouvez utiliser ce jeton pour associer une boîte aux lettres aux services que vous fournissez à un complément de courrier pour Outlook.

L’exemple se divise en deux parties :  
– Un complément de courrier pour Outlook qui s’exécute dans votre client de messagerie. Il demande un jeton d’identité au serveur Exchange Server et envoie ce jeton au service web.
– Service web qui valide le jeton d’accès du client. Le service Web répond avec le contenu du jeton, que le complément affiche alors.

Le service suit les étapes suivantes pour traiter le jeton :  
1. Décode le jeton d’identité pour obtenir l’URL du document de métadonnées d’authentification du serveur Exchange. Au cours de cette étape, le service vérifie également si le jeton a expiré et vérifie le numéro de version sur le jeton.  
2. Si le jeton d’identité passe la première étape, le service utilise les informations du document de métadonnées d’authentification pour obtenir le certificat utilisé pour signer le jeton à partir du serveur.  
3. Si le jeton est valide, le service le renvoie au complément courrier pour Outlook pour l’afficher.

Le service n’utilise pas le jeton d’aucune façon. Il répond avec les informations contenues dans le jeton, ou par un message d’erreur si le jeton n’est pas valide. 

Cet exemple requiert également une fonction de validation de certificat X.509 qui permet au service de répondre aux demandes qui sont signées à l’aide d’un certificat auto-signé émis par le serveur Exchange. Par défaut, le serveur Exchange utilise ce certificat auto-signé. Si votre serveur Exchange dispose d’un certificat valide qui effectue le suivi auprès d’un fournisseur racine, cette fonction de validation n’est pas obligatoire. Pour plus d’informations sur la fonction validation, voir [Validation des certificats X509 pour SSL sur HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx).


<a name="build"></a>
## Création et débogage ##
Le complément sera activé sur tout message électronique figurant dans la boîte de réception de l’utilisateur. Vous pouvez simplifier le test du complément en envoyant un ou plusieurs courriers électroniques à votre compte de test avant d’exécuter l’exemple.

1. Ouvrez la solution dans Visual Studio, puis appuyez sur F5 pour créer et déployer l’exemple. 
2. Connectez-vous à un compte Exchange en fournissant l’adresse de courrier et le mot de passe d’un serveur Exchange 2013, puis autorisez le serveur à configurer le compte de messagerie.  
3. Dans le navigateur, connectez-vous avec le compte de courrier en entrant le nom du compte et le mot de passe.  
4. Sélectionnez un message dans la boîte de réception, puis cliquez sur **Valider un jeton d'identité** dans la barre de complément qui se présente au-dessus du message.  
   Le complément se charge et affiche le contenu du jeton d’identité du client.
   
>Si vous exécutez l’exemple sur un serveur Exchange Server qui utilise le certificat auto-signé par défaut, vous recevrez une erreur de certificat lorsque le navigateur Web s’ouvre. Une fois que vous avez vérifié que le navigateur ouvre l’URL correcte en examinant l’adresse Web, sélectionnez **Continuer sur ce site Web** pour démarrer Outlook Web App.


<a name="troubleshooting"></a>
## Résolution des problèmes
Vous pouvez rencontrer les problèmes suivants lorsque vous utilisez Outlook Web App pour tester un complément courrier pour Outlook :

- La barre de complément n'apparaît pas lorsque le message est sélectionné. Si c’est le cas, redémarrez le complément en sélectionnant **Debug – arrêter le débogage** dans la fenêtre Visual Studio, puis appuyez sur F5 pour regénérer et déployer le complément.  
- Les modifications apportées au code JavaScript peuvent ne pas être prises en compte lors du déploiement et de l’exécution du complément. Si les modifications ne sont pas prises en compte, effacez le cache du navigateur web en sélectionnant **Outils – Options Internet** puis sélectionnez **Supprimer...** Supprimez les fichiers Internet temporaires, puis redémarrez le complément.
- Si vous ne voyez pas de référence à System.Web.Mvc répertorié sous Références, veillez à l’ajouter.

Si le complément se charge mais ne s’exécute pas, essayez de générer la solution dans Visual Studio (**Build > Générer une solution**). Recherchez les dépendances manquantes dans la Liste des erreurs et ajoutez-les si nécessaire.

<a name="questions"></a>
## Questions et commentaires

- Si vous rencontrez des difficultés pour exécuter cet exemple, veuillez [consigner un problème](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken/issues).
- Si vous avez des questions générales sur le développement de compléments Office, envoyez-les sur [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Posez vos questions ou envoyez vos commentaires en incluant la balise `office-addins`.

<a name="additional-resources"></a>
## Ressources supplémentaires
- Documentation pour [Compléments Office](https://msdn.microsoft.com/library/office/jj220060.aspx) sur MSDN.
- [API Web : Le site officiel Microsoft ASP.NET](http://www.asp.net/web-api)  
- [Validation de certificats X509 pour SSL sur HTTP](http://msdn.microsoft.com/library/bb408523(EXCHG.80).aspx)  
- [Authentification d’un complément de courrier à l’aide de jetons d’identité Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Valider un jeton d’identité Exchange 2013](http://msdn.microsoft.com/library/office/apps/fp179819(v=office.15))  
- [Autres exemples de compléments](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Copyright
Copyright (c) 2015 Microsoft. Tous droits réservés.


Ce projet a adopté le [code de conduite Open Source de Microsoft](https://opensource.microsoft.com/codeofconduct/). Pour en savoir plus, reportez-vous à la [FAQ relative au code de conduite](https://opensource.microsoft.com/codeofconduct/faq/) ou contactez [opencode@microsoft.com](mailto:opencode@microsoft.com) pour toute question ou tout commentaire.
