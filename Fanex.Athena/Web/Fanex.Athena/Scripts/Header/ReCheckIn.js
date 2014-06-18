/*global Site, _page*/
/// <reference path="/@JSense.js" />
"use strict";
var handle;

function closeBrowser() {
    var index = window;
    while (index !== index.parent) // Jump out
    {
        index = index.parent;
    }

    index.close();
}

function ReCheckin() {
    var Success = 0;
    // var Unknown = -1;
    // var AccessDenied = 1;
    // var AccountClosed = 2;
    // var KickedOut = 3;
    // var UnderMaintenance = 4;
    var SessionOverride = 5;

    var isInternal = $("#IsInternal").val() === "1";
    $.ajax({
        url: Site.resolveClientUrl("Authorization/ReCheckIn?onid=") + _page.OnUserID,
        async: true,
        type: "GET",
        cache: false,
        success: function (result) {
            var errCode = result.ErrCode;
            if (errCode !== Success) {
                clearInterval(handle);
                alert(result.ErrMsg, true);
                if (isInternal || SessionOverride === errCode) {
                    var signOutUrl = Site.resolveClientUrl('Header/SignOut');
                    $.ajax({
                        url: signOutUrl,
                        complete: function () {
                            closeBrowser();
                        },
                        type: "GET",
                        async: true
                    });
                }
                else {
                    Site.signOut();
                }
            }
        },
        error: function (/*request, status, error*/) { }
    });
}

$(window).load(function () {
    handle = setInterval(ReCheckin, 30000);
});