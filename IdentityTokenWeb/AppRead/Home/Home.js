/*
* Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.
*/

/// <reference path="../App.js" />
var _mailbox;
var _xhr;

(function () {
    "use strict";

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            app.initialize();

            _mailbox = Office.context.mailbox;
            _mailbox.getUserIdentityTokenAsync(getUserIdentityTokenCallback);
        });
    };

    function getUserIdentityTokenCallback(asyncResult) {
        var token = asyncResult.value;

        _xhr = new XMLHttpRequest();
        _xhr.open("POST", "https://localhost:44311/api/IdentityToken/");
        _xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        _xhr.onreadystatechange = readyStateChange;

        var request = new Object();
        request.token = token;

        _xhr.send(JSON.stringify(request));
    }

    function readyStateChange() {
        if (_xhr.readyState == 4 && _xhr.status == 200) {

            var response = JSON.parse(_xhr.responseText);

            if (undefined == response.errorMessage) {
                document.getElementById("msexchuid").value = response.token.msexchuid;
                document.getElementById("amurl").value = response.token.amurl;
                document.getElementById("uniqueID").value = response.token.uniqueID;
                document.getElementById("aud").value = response.token.aud;
                document.getElementById("iss").value = response.token.iss;
                document.getElementById("x5t").value = response.token.x5t;
                document.getElementById("nbf").value = response.token.nbf;
                document.getElementById("exp").value = response.token.exp;

                document.getElementById("rsp").value = _xhr.responseText;
                document.getElementById("error").value = "Complete.";
            }
            else {
                document.getElementById("error").value = response.error;
                app.showNotification("Error!", response.errorMessage);
            }
        }
    }
})();


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