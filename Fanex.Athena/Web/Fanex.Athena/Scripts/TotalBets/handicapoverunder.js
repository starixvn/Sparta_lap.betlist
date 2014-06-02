/// <reference path="/@JSense.js" />
var HandicapOU = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            HandicapOU.initEvent();
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                HandicapOU.submit();
            });

            // Forcast button click
            $("#Refresh-Content").on("click", '.button-add', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                HandicapOU.viewForecast(matchId, betType);
            });
        },
        viewNewForecast: function (leagueId, matchId, currencyId, baseCurrency) {
            var url = Site.resolveClientUrl("Forecast/NewForecastHdpOu");
            url = QueryString.setParam(url, "leagueId", leagueId);
            url = QueryString.setParam(url, "matchId", matchId);
            url = QueryString.setParam(url, "currencyId", currencyId);
            url = QueryString.setParam(url, "baseCurrency", baseCurrency);

            window.location.href = url;
        },
        viewForecast: function (matchId, betType) {
            var url = Site.resolveClientUrl("Forecast/ForeCastAh?matchid=" + matchId + '&bettype=' + betType);
            url = Site.setLicenseeOptionOriginalParam(url);

            var popH = 250, popW = 700;

            ForecastPopup.showPopup(popW, popH, url);
        },
        submit: function () {
            Site.blockUI();
            $("#HandicapOUForm").submit();
        }
    };
})(jQuery);
