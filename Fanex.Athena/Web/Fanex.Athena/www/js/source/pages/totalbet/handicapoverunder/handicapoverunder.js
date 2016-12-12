define(['jquery', 'common/site', 'common/querystring', '../totalbetsbase', 'components/forecastpopup'],
    function ($, Site, QueryString, Totalbets, ForecastPopup) {
    "use strict";

    var _me = {
        initialize: function () {
            Site.initialize();
            Totalbets.initialize();
            _me.initEvent();
            Site.addUserGuide({
                resourceName: "TotalBetsHdpOu",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("HandicapOverUnderLive")
            });
        },
        initEvent: function () {
            $("#SubmitButton").on("click", function () {
                _me.submit();
            });

            $("#Refresh-Content").on("click", '.button-add', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                _me.viewForecast(matchId, betType);
            });

            $(window).on("resize", Totalbets.resizeGrid);

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        },
        viewNewForecast: function (leagueId, matchId, currencyId, baseCurrency) {
            var url = Site.resolveClientUrl("NewHandicapOverUnderForecast/Index");
            url = QueryString.setParam(url, "leagueId", leagueId);
            url = QueryString.setParam(url, "matchId", matchId);
            url = QueryString.setParam(url, "currencyId", currencyId);
            url = QueryString.setParam(url, "baseCurrency", baseCurrency);

            Site.redirect(url);
        },
        viewForecast: function (matchId, betType) {
            var url = Site.resolveClientUrl("AhForecast/Index?matchid=" + matchId + '&bettype=' + betType);
            url = Site.setLicenseeOptionOriginalParam(url);

            var popH = 250, popW = 700;
            ForecastPopup.showPopup(popW, popH, url);
        },
        submit: function () {
            Site.blockUI();
            $("#HandicapOUForm").submit();
        }
    };

    return _me;
});