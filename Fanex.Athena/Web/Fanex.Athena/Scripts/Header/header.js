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
        top.frames["main"].Site.blockUI();
        $.ajax({
            url: Site.resolveClientUrl("Header/ChangeLanguage?languagesite=") + lang,
            async: false,
            type: "GET",
            cache: false,
            success: function (result) {
                if (result === "1") {
                    top.frames["main"].location.href = Site.resolveClientUrl("Report/Balance");
                    top.frames["menu"].location.href = Site.resolveClientUrl("Menu/Index");
                    top.frames["header"].location.href = Site.resolveClientUrl("Header/Index");
                    top.frames["frFooter"].location.href = top.frames["frFooter"].location.href;
                }
            }
        });
    });
    $("#ddlLicensee").change(function () {
        var licenseeId = $(this).val();
        top.frames["main"].location.href = Site.resolveClientUrl("Account/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
    });
    $("#change-pass-btn").click(function () {
        window.parent.main.document.location.href = Site.resolveClientUrl("Utility/ChangePass/");
    });
    $("#lougout-btn").click(function () {
        Site.signOut();
    });
});