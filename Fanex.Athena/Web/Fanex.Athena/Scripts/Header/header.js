/*global Site*/
/// <reference path="/@JSense.js" />
"use strict";
$(document).ready(function () {
    // init clock
    var options = {
        serverTime: new Date($('#current-time-clock').html()),
        format: '%H:%M:%S %b %d, %Y GMT'
    };
    $('#current-time-clock').jclock(options);

    $("#languagesite").change(function () {
        var lang = $(this).val();
        var mainFrame = top.frames["main"];
        if (mainFrame && mainFrame.Site && mainFrame.Site.blockUI) {
            mainFrame.Site.blockUI();
        }

        $.ajax({
            url: Site.resolveClientUrl("Header/ChangeLanguage?languagesite=") + lang,
            async: false,
            type: "GET",
            cache: false,
            success: function (result) {
                if (result === "1") {
                    if ($.cookie("isClosedNotification") !== "1") {
                        $.cookie('isFirstLoad', '1', { path: '/' });
                    }
                    top.location.reload();
                }
            }
        });
    });
    $("#ddlLicensee").change(function () {
        var licenseeId = $(this).val();
        top.frames["main"].location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
    });
    $("#change-pass-btn").click(function () {
        window.parent.main.document.location.href = Site.resolveClientUrl("Authorization/ChangePassword");
    });
    $("#lougout-btn").click(function () {
        Site.signOut();
    });
});