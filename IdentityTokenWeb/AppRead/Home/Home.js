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
                $("#msexchuid").val(response.token.msexchuid);
                $("#amurl").val(response.token.amurl);
                $("#uniqueID").val(response.token.uniqueID);
                $("#aud").val(response.token.aud);
                $("#iss").val(response.token.iss);
                $("#x5t").val(response.token.x5t);
                $("#nbf").val(response.token.nbf);
                $("#exp").val(response.token.exp);

                $("#rsp").val(_xhr.responseText);
                $("#error").val("none");
            }
            else {
                $("#error").val(response.error);
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