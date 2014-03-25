/// <reference path="/@JSense.js" />

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
            url: Site.getBaseUrl() + "Header/ChangeLanguage?languagesite=" + lang,
            async: false,
            type: "GET",
            cache: false,
            success: function (result) {
                if (result == "1") {
                    top.frames["main"].location.href = Site.getBaseUrl() + "Report/Balance";
                    top.frames["menu"].location.href = Site.getBaseUrl() + "_menu/Menu.aspx";
                    top.frames["header"].location.href = Site.getBaseUrl() + "Header/Index";
                    top.frames["frFooter"].location.href = top.frames["frFooter"].location.href;
                }
            },
            error: function (request, status, error) { }
        });
    });
    $("#ddlLicensee").change(function () {
        var licenseeId = $(this).val();
        top.frames["main"].location.href = Site.getBaseUrl() + "Account/ChangeLicensee?licenseeid=" + licenseeId + "&nocache=" + +(new Date().getTime());

    });
    $("#change-pass-btn").click(function () {
        window.parent.main.document.location.href = Site.getBaseUrl() + "_MemberInfo/ChangePass/ChangePass.aspx";

    });
    $("#lougout-btn").click(function () {
        Site.signOut();
    });
});
