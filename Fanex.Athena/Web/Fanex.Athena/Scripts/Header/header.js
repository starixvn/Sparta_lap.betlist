
function ChangeLicensee() {
    var licenseeId = $("#ddlLicensee").val();
    top.frames["main"].location.href = "../Account/ChangeLicensee?licenseeid=" + licenseeId + "&nocache=" + +(new Date().getTime());
}

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
            url: ibc.GetBaseUrl() + "_header/handlers/ChangeLanguage.ashx?languagesite=" + lang,
            async: false,
            type: "GET",
            cache: false,
            success: function (result) {
                if (result == "1") {
                    top.frames["menu"].location.href = ibc.GetBaseUrl() + "_menu/Menu.aspx";
                    top.frames["header"].location.href = ibc.GetBaseUrl() + "_header/Header.aspx";
                    top.frames["main"].location.href = ibc.GetBaseUrl() + "/Report/Balance";
                    top.frames["frFooter"].location.href = top.frames["frFooter"].location.href;
                }
            },
            error: function (request, status, error) { }
        });
    });
});
