/*global _page, CryptoJS, resolveClientUrl */
/*exported InitLogin, GetValidateImage,DoLogin, DoEnter*/
/*
* Created 20100113@Lee - Sign-in javascript functions
* Revision ?@? - ...
*
*/
"use strict";
function isIE() { return (/msie/i).test(navigator.userAgent) && !/opera/i.test(navigator.userAgent); }
window.IE = isIE();

var LanguageMessages = {
    errUserName: "Please input username and password",
    errValidCode: "You must input validation code."
};

function getUserSalt() {
    var salt;
    var userName = $("#userName").val();
    if (userName) {
        var url = resolveClientUrl("Authorization/GetUserSalt?userName=" + userName);
        $.ajax({
            url: url,
            type: "GET",
            async: false,
            contentType: "application/json; charset=utf-8",
            cache: false,
            beforeSend: function () {
            },
            success: function (result) {
                salt = result;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    return salt;
}

function DoLogin() {
    var errObj = LanguageMessages;
    if ($('#userName').val() === "") {
        $("#errmsg").html(errObj.errUserName);
        $('#userName').focus();
        return false;
    }
    if ($('#passWord').val() === "") {
        $("#errmsg").html(errObj.errUserName);
        $('#passWord').focus();
        return false;
    }
    if ($('#captcha').val() === "") {
        $("#errmsg").html(errObj.errValidCode);
        $('#captcha').focus();
        return false;
    }

    $('#btnLogin').attr('disabled', 'disabled');


    var $username = $("#userName");
    var $password = $("#passWord");

    $username.val($username.val().trim());
    var isUserNameValid = $username.val() !== "";
    var isPasswordValid = $password.val() !== "";
    var $loginToken = $('#LogInToken');
    var $loginTokenTemp = $('#LogInTokenTemp');

    var userSalt = getUserSalt();

    $loginToken.val($loginTokenTemp.val());
    var hashPassword = CryptoJS.SHA1($password.val() + userSalt).toString(CryptoJS.enc.Hex).toUpperCase();
    var encryptedPass = CryptoJS.SHA1(hashPassword + $loginToken.val()).toString(CryptoJS.enc.Hex).toUpperCase();
    $password.val(encryptedPass);

    if (isUserNameValid && isPasswordValid) {
        document.fLogin.submit();
        return true;
    }

    return false;
}

function GetValidateImage() {
    var objDate = new Date();
    var url = $("#captchaUrl").val() + "?code=" + objDate.getMilliseconds();
    $("#imgCode").attr("src", url);

    setTimeout(GetValidateImage, 120000);
}
function DoEnter(evt, action) {
    if (window.IE) {
        if (window.event.keyCode === 13) {
            eval(action); // jshint ignore:line
        }
    }
    else {
        if (evt.keyCode === 13) {
            eval(action); // jshint ignore:line
        }
    }
}

function InitLogin() {
    // Do not allow wrapping sign-in page
    if (window.parent != window) { // jshint ignore:line
        window.parent.location = location.href;
    }

    window.GetValidateImage();

    if (typeof _page.errCode !== 'undefined' && _page.errCode !== 0) {
        $("#errmsg").html(_page.errMsg);
        if (_page.errCode === 101) {
            $('#txtCaptcha').focus().select();
        }
    }
}

$(window).load(function () {
    window.InitLogin();
});