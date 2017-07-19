/*global _page, resolveClientUrl */

define(["jquery", "crypto-js"], function ($, CryptoJS) {
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

    function getValidateImage() {
        var url = $("#captchaUrl").val();
        $("#imgCode").attr("src", url);

        setTimeout(getValidateImage, 120000);
    }

    function checkInputValid() {
        var $username = $("#userName");
        var $password = $("#passWord");
        var $captcha = $('#captcha');

        if ($username.val() && $password.val() && $captcha.val()) {
            return true;
        }

        var $errorMessage = $("#errmsg");
        $errorMessage.html(LanguageMessages.errUserName);

        if ($username.val() === "") {
            $username.focus();
        }
        else if ($password.val() === "") {
            $password.focus();
        }
        else {
            $errorMessage.html(LanguageMessages.errValidCode);
            $('#captcha').focus();
        }

        $errorMessage.css("opacity", 1);
        return false;
    }

    function InitLogin() {
        // Do not allow wrapping sign-in page
        if (window.parent !== window) { // jshint ignore:line
            window.parent.location = location.href;
        }

        getValidateImage();

        if (typeof _page.errCode !== 'undefined' && _page.errCode !== 0) {
            $("#errmsg").html(_page.errMsg).css("opacity", 1);
            if (_page.errCode === 101) {
                $('#txtCaptcha').focus().select();
            }
        }

        document.fLogin.onsubmit = function () {
            var $username = $("#userName");
            $username.val($.trim($username.val()));
            if (!checkInputValid()) {
                return false;
            }

            $('#btnLogin').prop('disabled', true);

            var $loginToken = $('#LogInToken');
            var $loginTokenTemp = $('#LogInTokenTemp');

            var userSalt = getUserSalt();

            $loginToken.val($loginTokenTemp.val());
            var $password = $("#passWord");
            var hashPassword = CryptoJS.SHA1($password.val() + userSalt).toString(CryptoJS.enc.Hex).toUpperCase();
            var encryptedPass = CryptoJS.SHA1(hashPassword + $loginToken.val() + userSalt).toString(CryptoJS.enc.Hex).toUpperCase();
            $password.val(encryptedPass);
            return true;
        };

        $("#captcha-img").on("click", function () {
            getValidateImage();
        });
    }

    return {
        InitLogin: InitLogin
    };
});