/// <reference path="/@JSense.js" />

function ReCheckin() {

    var Success = 0;
    var Unknown = -1;
    var AccessDenied = 1;
    var AccountClosed = 2;
    var KickedOut = 3;
    var UnderMaintenance = 4;
    var SessionOverride = 5;

    var isInternal = $("#IsInternal").val() == "1";
    $.ajax({
        url: Site.getBaseUrl() + "_authorization/handlers/recheckin.ashx?onid=" + _page.OnUserID,
        async: true,
        type: "POST",
        dataType: "json",
        success: function (result) {
            var errCode = result.errCode;
            if (errCode != Success) {
                clearInterval(handle);
                var errMsg = result.errMsg;
                alert(errMsg,true);
                if (isInternal || SessionOverride == errCode) {

                    var signOutUrl = Site.getBaseUrl() + '_Authorization/Handlers/SignOut.ashx';
                    $.ajax({
                        url: signOutUrl,
                        complete: function (rs) {
                            closeBrowser();
                        },
                        type: "POST",
                        async: true
                    });
                }
                else {
                    Site.signOut();
                }
            }
        },
        error: function (request, status, error) { }
    });
}

function closeBrowser() {
    var index = window;
    while (index != index.parent) // Jump out
    {
        index = index.parent;
    }

    index.close();
}

var handle;
$(window).load(function () {
    handle = setInterval("ReCheckin()", 30000);
});

