/// <reference path="/@JSense.js" />
var Totalbets = (function ($) {
    return {
        initEvent: function () {
            // Init refresh countdown
            RefreshCountDown.initialize();

            // Forcast number click
            $("#Refresh-Content").on("click", '.forcast-number', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                Totalbets.viewRunningBetListMatch(matchId, betType);
            });
        },
        viewRunningBetListMatch: function (matchId, betType) {
            var url = Site.resolveClientUrl("BetList/RunningBetListOfMatch");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "bettype", betType);

            window.location.href = url;
        },
        viewRunningBetListMultiBettype: function (matchId, betType) {
            var url = Site.resolveClientUrl("BetList/RunningBetListOfMatch");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "bettype", betType);

            window.location.href = url;
        },
        initialize: function () {
            Site.unBlockUI();
            this.initEvent();
        }
    };
})(jQuery);