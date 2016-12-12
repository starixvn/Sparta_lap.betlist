define(['jquery', 'common/site', 'common/querystring', 'components/refreshcountdown'], function ($, Site, QueryString, RefreshCountDown) {
    var _me = {
        resizeGrid: function () {
            var $grid = $(".body-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;

            var gridHeight = window.innerHeight - gridTop - $(".footer-content").height();
            gridHeight = Math.max(150, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initEvent: function () {
            // Init refresh countdown
            RefreshCountDown.initialize();

            // Forcast number click
            $("#Refresh-Content").on("click", '.forcast-number', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                _me.viewRunningBetListMatch(matchId, betType);
            });

            $(document).ajaxComplete(function () {
                _me.resizeGrid();
            });
        },
        viewRunningBetListMatch: function (matchId, betType) {
            var url = Site.resolveClientUrl("RunningMatchBetList/Index");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "bettype", betType);
            url = QueryString.setParam(url, "isShowSumStake", true);

            Site.redirect(url);
        },
        viewRunningBetListMultiBettype: function (matchId, betType) {
            var url = Site.resolveClientUrl("RunningBettypesOfMatchBetList/Index");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "multiplebettype", betType);
            url = QueryString.setParam(url, "isShowSumStake", true);

            Site.redirect(url);
        },
        initialize: function () {
            Site.unBlockUI();
            this.initEvent();
            _me.resizeGrid();
        }
    };

    return _me;
});