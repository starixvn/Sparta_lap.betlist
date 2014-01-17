/// <reference path="/@JSense.js" />
var ForecastHDPOU = (function ($) {
    return {
        initialize: function () {
            Site.unBlockUI();
            // Init timer count down
            RefreshCountDown.initialize();
            ForecastHDPOU.initEvent();
        },
        initEvent: function () {
            // League change
            $('.league-list').on('change', function () {
                var leagueId = $(this).val();
                ForecastHDPOU.loadSelectedLeague(leagueId);
            });

            // Forecast button click
            $('#Refresh-Content').on('click', '.button-add', function () {
                var matchId = $(this).attr('data-match-id');
                var betType = parseInt($(this).attr('data-bettype'));
                ForecastHDPOU.openForecast(matchId, betType);
            });

            // Submit button click
            $("#SubmitButton").on("click", function () {
                ForecastHDPOU.submit();
            });
        },
        openForecast: function (matchId, betType) {
            var url = Site.resolveClientUrl("Forecast/ForeCastAh?matchid=" + matchId + '&bettype=' + betType);
            url = Site.setLicenseeOptionOriginalParam(url);

            var popH = 250, popW = 700;

            ForecastPopup.showPopup(popW, popH, url);
        },
        loadSelectedLeague: function (leagueId) {
            var url = Site.resolveClientUrl("Forecast/ForecastHandicapOverUnder");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "LeagueId", leagueId);

            $.ajax({
                url: url,
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (data) {
                    Site.unBlockUI();
                    $('#Refresh-Content').html(data);
                },
                type: "POST",
                async: true,
                dataType: "html"
            });
        },
        submit: function () {
            Site.blockUI();
            $('#ForecastHandicapOverUnderForm').submit();
        }
    };
})(jQuery);