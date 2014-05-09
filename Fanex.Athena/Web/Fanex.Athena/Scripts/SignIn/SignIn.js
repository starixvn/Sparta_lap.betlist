/*
* Created 20100113@Lee - Sign-in javascript functions
* Revision ?@? - ... 
* 
*/
function isIE() { return /msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent); }
IE = isIE();

var LanguageMessages = {
    errUserName: "Please input username and password",
    errValidCode: "You must input validation code."
};
function DoLogin() {
    var errObj = LanguageMessages;
    if ($('#userName').val() == "") {
        $("#errmsg").html(errObj.errUserName);
        $('#userName').focus();
        return false;
    }
    if ($('#passWord').val() == "") {
        $("#errmsg").html(errObj.errUserName)
        $('#passWord').focus();
        return false;
    }
    if ($('#captcha').val() == "") {
        $("#errmsg").html(errObj.errValidCode);
        $('#captcha').focus();
        return false;
    }

    $('#btnLogin').attr('disabled', 'disabled');

    document.fLogin.submit();

    return true;
}

function GetValidateImage() {
    var objDate = new Date();
    var url = $("#captchaUrl").val() + "?code=" + objDate.getMilliseconds();
    $("#imgCode").attr("src", url);

    setTimeout(GetValidateImage, 120000);
}
function DoEnter(evt, action) {
    if (IE) {
        if (window.event.keyCode == 13) eval(action);
    }
    else {
        if (evt.keyCode == 13) eval(action);
    }
}

function InitLogin() {
    // Do not allow wrapping sign-in page
    if (window.parent != window) {
        window.parent.location = location.href;
    }

    GetValidateImage();
    
    if (typeof _page.errCode != 'undefined' && _page.errCode != 0) {
        $("#errmsg").html(_page.errMsg);
        if (_page.errCode == 101) {
            $('#txtCaptcha').focus();
        }
    }
}

$(window).load(function () {
    InitLogin();

});
