/*
* Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.
*/

using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;

namespace IdentityTokenService.Models
{
    public static class AuthMetadata
    {
        public static X509Certificate2 GetSigningCertificate(Uri authMetadataEndpoint)
        {
            JsonAuthMetadataDocument document = GetMetadataDocument(authMetadataEndpoint);

            if (null != document.keys && document.keys.Length > 0)
            {
                JsonKey signingKey = document.keys[0];

                if (null != signingKey && null != signingKey.keyValue)
                {
                    return new X509Certificate2(Encoding.UTF8.GetBytes(signingKey.keyValue.value));
                }
            }

            throw new ApplicationException("The metadata document does not contain a signing certificate.");
        }

        public static JsonAuthMetadataDocument GetMetadataDocument(Uri authMetadataEndpoint)
        {
            ServicePointManager.ServerCertificateValidationCallback = Config.CertificateValidationCallback;

            byte[] acsMetadata;
            using (WebClient webClient = new WebClient())
            {
                acsMetadata = webClient.DownloadData(authMetadataEndpoint);
            }
            string jsonResponseString = Encoding.UTF8.GetString(acsMetadata);

            JsonAuthMetadataDocument document = new JavaScriptSerializer().Deserialize<JsonAuthMetadataDocument>(jsonResponseString);

            if (null == document)
            {
                throw new ApplicationException(String.Format("No authentication metadata document found at {0}.", authMetadataEndpoint));
            }

            return document;
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