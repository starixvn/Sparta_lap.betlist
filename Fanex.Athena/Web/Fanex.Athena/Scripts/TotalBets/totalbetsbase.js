/// <reference path="/@JSense.js" />
var Totalbets = (function ($) {
    return {
        resizeGrid: function () {
            var $grid = $(".data-content");
            if ($grid.length == 0) {
                return;
            }

            var gridTop = $grid.offset().top;

            var gridHeight = window.innerHeight - gridTop - 10;
            gridHeight = Math.max(150, gridHeight);
            gridHeight = Math.min($grid.height(), gridHeight);
            $grid.css("height", gridHeight);
        },
        initEvent: function () {
            // Init refresh countdown
            RefreshCountDown.initialize();

            // Forcast number click
            $("#Refresh-Content").on("click", '.forcast-number', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                Totalbets.viewRunningBetListMatch(matchId, betType);
            });

            $(document).ajaxComplete(function (event, request, settings) {
                Totalbets.resizeGrid();
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
            var url = Site.resolveClientUrl("BetList/RunningBetListByMultipleBettype");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "multiplebettype", betType);

            window.location.href = url;
        },
        initialize: function () {
            Site.unBlockUI();
            this.initEvent();
            Totalbets.resizeGrid();
        }
    };
})(jQuery);

$(window).resize(function () {
    "use strict";
    Totalbets.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});