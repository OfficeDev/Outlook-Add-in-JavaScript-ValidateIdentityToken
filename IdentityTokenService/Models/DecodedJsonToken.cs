/*
* Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.
*/

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace IdentityTokenService.Models
{
    public class DecodedJsonToken : IDisposable
    {
        private readonly Dictionary<string, string> headerClaims;
        private readonly Dictionary<string, string> payloadClaims;
        private readonly Dictionary<string, string> appContext;

        private readonly string signature;

        public DecodedJsonToken(Dictionary<string, string> header, Dictionary<string, string> payload, string signature)
        {

            // We'll start out assuming that the token is invalid.
            this.IsValid = false;

            // Set the private dictionaries that contain the claims.
            this.headerClaims = header;
            this.payloadClaims = payload;
            this.signature = signature;

            // If there is no "appctx" claim in the token, throw an ApplicationException.
            if (!this.payloadClaims.ContainsKey(AuthClaimTypes.AppContext))
            {
                throw new ApplicationException(String.Format("The {0} claim is not present.", AuthClaimTypes.AppContext));
            }

            appContext = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(payload[AuthClaimTypes.AppContext]);


            // Validate the header fields.
            this.ValidateHeader();

            // Determine if the token is within its valid time.
            this.ValidateLifetime();

            // Validate that the token was sent to the correct URL.
            this.ValidateAudience();

            // Validate the token version.
            this.ValidateVersion();

            // Make sure that the appctx contains an authentication
            // metadata location.
            this.ValidateMetadataLocation();

            // If the token passes all of the validation checks, then we
            // can assume that it is valid.
            this.IsValid = true;
        }

        public string Audience
        {
            get { return this.payloadClaims[AuthClaimTypes.Audience]; }
        }

        public bool IsValid { get; private set; }

        public string AuthMetadataUri
        {
            get { return this.appContext[AuthClaimTypes.MsExchAuthMetadataUrl]; }
        }

        private void ValidateAudience()
        {
            if (!this.payloadClaims.ContainsKey(AuthClaimTypes.Audience))
            {
                throw new ApplicationException(String.Format("The \"{0}\" claim is missing from the application context.", AuthClaimTypes.Audience));
            }

            string location = Config.Audience.Replace("/", "-").Replace("\\", "-");
            string audience = this.payloadClaims[AuthClaimTypes.Audience].Replace("/", "-").Replace("\\", "-");

            if (!location.Equals(audience))
            {
                throw new ApplicationException(String.Format(
                  "The audience URL does not match. Expected {0}; got {1}.",
                  Config.Audience, this.payloadClaims[AuthClaimTypes.Audience]));
            }
        }

        private void ValidateHeaderClaim(string key, string value)
        {
            if (!this.headerClaims.ContainsKey(key))
            {
                throw new ApplicationException(String.Format("Header does not contain \"{0}\" claim.", key));
            }

            if (!value.Equals(this.headerClaims[key]))
            {
                throw new ApplicationException(String.Format("\"{0}\" claim must be \"{0}\".", key, value));
            }
        }

        private void ValidateHeader()
        {
            ValidateHeaderClaim(AuthClaimTypes.TokenType, Config.TokenType);
            ValidateHeaderClaim(AuthClaimTypes.Algorithm, Config.Algorithm);
        }

        private void ValidateLifetime()
        {
            if (!this.payloadClaims.ContainsKey(AuthClaimTypes.ValidFrom))
            {
                throw new ApplicationException(
                  String.Format("The \"{0}\" claim is missing from the token.", AuthClaimTypes.ValidFrom));
            }

            if (!this.payloadClaims.ContainsKey(AuthClaimTypes.ValidTo))
            {
                throw new ApplicationException(
                  String.Format("The \"{0}\" claim is missing from the token.", AuthClaimTypes.ValidTo));
            }

            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            TimeSpan padding = new TimeSpan(0, 5, 0);

            DateTime validFrom = unixEpoch.AddSeconds(int.Parse(this.payloadClaims[AuthClaimTypes.ValidFrom]));
            DateTime validTo = unixEpoch.AddSeconds(int.Parse(this.payloadClaims[AuthClaimTypes.ValidTo]));

            DateTime now = DateTime.UtcNow;

            if (now < (validFrom - padding))
            {
                throw new ApplicationException(String.Format("The token is not valid until {0}.", validFrom));
            }

            if (now > (validTo + padding))
            {
                throw new ApplicationException(String.Format("The token is not valid after {0}.", validFrom));
            }
        }

        private void ValidateMetadataLocation()
        {
            if (!this.appContext.ContainsKey(AuthClaimTypes.MsExchAuthMetadataUrl))
            {
                throw new ApplicationException(String.Format("The \"{0}\" claim is missing from the token.", AuthClaimTypes.MsExchAuthMetadataUrl));
            }
        }

        private void ValidateVersion()
        {
            if (!this.appContext.ContainsKey(AuthClaimTypes.MsExchTokenVersion))
            {
                throw new ApplicationException(String.Format("The \"{0}\" claim is missing from the token.", AuthClaimTypes.MsExchTokenVersion));
            }

            if (!Config.Version.Equals(this.appContext[AuthClaimTypes.MsExchTokenVersion]))
            {
                throw new ApplicationException(String.Format(
                  "The version does not match. Expected {0}; got {1}.",
                  Config.Version, this.appContext[AuthClaimTypes.MsExchTokenVersion]));
            }
        }

        #region IDisposable Members

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }
            disposed = true;
        }

        #endregion
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