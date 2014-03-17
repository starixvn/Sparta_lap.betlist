
$(document).ready(function () {
    // init clock
    var options = {
        serverTime: new Date($('#current-time-clock').html()),
        format: '%H:%M:%S %b %d, %Y GMT'
    };
    $('#current-time-clock').jclock(options);

    $("#languagesite").change(function () {
        var lang = $(this).val();

        $.ajax({
            url: resolveClientUrl("Header/ChangeLanguage?languagesite=" + lang),
            async: false,
            type: "GET",
            cache: false,
            success: function (result) {
                if (result == "1") {
                    top.frames["menu"].location.href = resolveClientUrl("_menu/Menu.aspx");
                    top.frames["header"].location.href = resolveClientUrl("Header/Index");
                    top.frames["main"].location.href = resolveClientUrl("Report/Balance");
                    top.frames["frFooter"].location.href = top.frames["frFooter"].location.href;
                }
            },
            error: function (request, status, error) { }
        });
    });
    $("#ddlLicensee").change(function () {
        var licenseeId = $(this).val();
        top.frames["main"].location.href = resolveClientUrl("Account/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + +(new Date().getTime());

    });
});
