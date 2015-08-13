/*
* Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.
*/

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace IdentityTokenService.Models
{
    public static class JsonTokenDecoder
    {
        public static DecodedJsonToken Decode(IdentityTokenRequest rawToken)
        {
            string[] tokenParts = rawToken.token.Split('.');

            if (tokenParts.Length != 3)
            {
                throw new ApplicationException("Token must have three parts separated by '.' characters.");
            }

            string encodedHeader = tokenParts[0];
            string encodedPayload = tokenParts[1];
            string signature = tokenParts[2];

            string decodedHeader = Base64UrlEncoder.Decode(encodedHeader);
            string decodedPayload = Base64UrlEncoder.Decode(encodedPayload);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Dictionary<string, string> header = serializer.Deserialize<Dictionary<string, string>>(decodedHeader);
            Dictionary<string, string> payload = serializer.Deserialize<Dictionary<string, string>>(decodedPayload);

            return new DecodedJsonToken(header, payload, signature);
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