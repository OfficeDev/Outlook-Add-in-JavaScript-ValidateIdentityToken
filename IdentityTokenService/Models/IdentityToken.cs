﻿/*
* Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.
*/

using Microsoft.IdentityModel.S2S.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;

namespace IdentityTokenService.Models
{
    public class IdentityToken
    {
        public string msexchuid { get; set; }
        public string amurl { get; set; }
        public string uniqueID
        {
            get { return ComputeUniqueIdentification(); }
        }

        public string iss { get; set; }
        public string x5t { get; set; }
        public DateTime nbf { get; set; }
        public DateTime exp { get; set; }
        public string aud { get; set; }
        public string version { get; set; }
        public bool isbrowserhostedapp { get; set; }
        public string appctxsender { get; set; }

        // Salt to apply when creating unique ID.
        private byte[] Salt = new byte[] { 25, 139, 201, 13 };

        private string ComputeUniqueIdentification()
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(string.Concat(msexchuid, amurl));

            // Combine input bytes and salt.
            byte[] saltedInput = new byte[Salt.Length + inputBytes.Length];
            Salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, Salt.Length);

            // Compute the unique key.
            byte[] hashedBytes = SHA256CryptoServiceProvider.Create().ComputeHash(saltedInput);

            // Convert the hashed value to a string and return.
            return BitConverter.ToString(hashedBytes);
        }

        private JsonWebSecurityTokenHandler GetSecurityTokenHandler(string audience,
            string authMetadataEndpoint,
            X509Certificate2 currentCertificate)
        {
            JsonWebSecurityTokenHandler jsonTokenHandler = new JsonWebSecurityTokenHandler();
            jsonTokenHandler.Configuration = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration();

            jsonTokenHandler.Configuration.AudienceRestriction = new Microsoft.IdentityModel.Tokens.AudienceRestriction(AudienceUriMode.Always);
            jsonTokenHandler.Configuration.AudienceRestriction.AllowedAudienceUris.Add(
              new Uri(audience, UriKind.RelativeOrAbsolute));

            jsonTokenHandler.Configuration.CertificateValidator = X509CertificateValidator.None;

            jsonTokenHandler.Configuration.IssuerTokenResolver =
              SecurityTokenResolver.CreateDefaultSecurityTokenResolver(
                new ReadOnlyCollection<SecurityToken>(new List<SecurityToken>(
                  new SecurityToken[]
            {
              new X509SecurityToken(currentCertificate)
            })), false);

            Microsoft.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry issuerNameRegistry =
                new Microsoft.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry();
            issuerNameRegistry.AddTrustedIssuer(currentCertificate.Thumbprint, Config.ExchangeApplicationIdentifier);
            jsonTokenHandler.Configuration.IssuerNameRegistry = issuerNameRegistry;

            return jsonTokenHandler;
        }

        public IdentityToken(IdentityTokenRequest rawToken, string audience, string authMetadataEndpoint)
        {
            X509Certificate2 currentCertificate = null;

            currentCertificate = AuthMetadata.GetSigningCertificate(new Uri(authMetadataEndpoint));

            JsonWebSecurityTokenHandler jsonTokenHandler =
                GetSecurityTokenHandler(audience, authMetadataEndpoint, currentCertificate);

            SecurityToken jsonToken = jsonTokenHandler.ReadToken(rawToken.token);
            JsonWebSecurityToken webToken = (JsonWebSecurityToken)jsonToken;

            x5t = currentCertificate.Thumbprint;
            iss = webToken.Issuer;
            aud = webToken.Audience;
            exp = webToken.ValidTo;
            nbf = webToken.ValidFrom;
            foreach (JsonWebTokenClaim claim in webToken.Claims)
            {
                if (claim.ClaimType.Equals(AuthClaimTypes.AppContextSender))
                {
                    appctxsender = claim.Value;
                }

                if (claim.ClaimType.Equals(AuthClaimTypes.IsBrowserHostedApp))
                {
                    isbrowserhostedapp = claim.Value == "true";
                }

                if (claim.ClaimType.Equals(AuthClaimTypes.AppContext))
                {
                    string[] appContextClaims = claim.Value.Split(',');
                    Dictionary<string, string> appContext =
                        new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(claim.Value);
                    amurl = appContext[AuthClaimTypes.MsExchAuthMetadataUrl];
                    msexchuid = appContext[AuthClaimTypes.MsExchImmutableId];
                    version = appContext[AuthClaimTypes.MsExchTokenVersion];
                }
            }
        }

    }
}


// *********************************************************
//
// Outlook-Add-in-JavaScript-ValidateIdentityToken, https://github.com/OfficeDev/Outlook-Add-in-JavaScript-ValidateIdentityToken
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// *********************************************************