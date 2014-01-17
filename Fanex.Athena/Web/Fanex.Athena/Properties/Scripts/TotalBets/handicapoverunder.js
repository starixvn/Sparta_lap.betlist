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
        viewForecast: function (matchId, betType) {

            var url = Site.resolveClientUrl("Forecast/ForeCastAh?matchid=" + matchId + '&bettype=' + betType);
            url = Site.setLicenseeOptionOriginalParam(url);
            
            var popH = 200, popW = 660;

            ForecastPopup.showPopup(popW, popH, url);
        },
        submit: function () {
            Site.blockUI();
            $("#HandicapOUForm").submit();
        }
    };
})(jQuery);