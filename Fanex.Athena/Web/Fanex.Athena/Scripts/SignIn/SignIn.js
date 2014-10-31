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
                alert(xhr.status + ": " + thrownError);
            }
        });
    }

    return salt;
}

function GetValidateImage() {
    var objDate = new Date();
    var url = $("#captchaUrl").val() + "?code=" + objDate.getMilliseconds();
    $("#imgCode").attr("src", url);

    setTimeout(GetValidateImage, 120000);
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

    document.fLogin.onsubmit = function () {
        var errObj = LanguageMessages;
        var $username = $("#userName");
        var $password = $("#passWord");

        $username.val($username.val().trim());
        if ($username.val() === "") {
            $("#errmsg").html(errObj.errUserName);
            $username.focus();
            return false;
        }
        if ($password.val() === "") {
            $("#errmsg").html(errObj.errUserName);
            $password.focus();
            return false;
        }

        if ($('#captcha').val() === "") {
            $("#errmsg").html(errObj.errValidCode);
            $('#captcha').focus();
            return false;
        }

        $('#btnLogin').prop('disabled', true);

        var $loginToken = $('#LogInToken');
        var $loginTokenTemp = $('#LogInTokenTemp');

        var userSalt = getUserSalt();

        $loginToken.val($loginTokenTemp.val());
        var hashPassword = CryptoJS.SHA1($password.val() + userSalt).toString(CryptoJS.enc.Hex).toUpperCase();
        var encryptedPass = CryptoJS.SHA1(hashPassword + $loginToken.val()).toString(CryptoJS.enc.Hex).toUpperCase();
        $password.val(encryptedPass);
        
        return true;
    };
}

$(window).load(function () {
    window.InitLogin();
});