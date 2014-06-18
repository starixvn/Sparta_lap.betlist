/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
var DailyRacingWinLoss = (function ($) {
    "use strict";
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $(".total-content").height() - 10;
            gridHeight = Math.max(120, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initialize: function () {
            DailyRacingWinLoss.initEvent();
            DailyRacingWinLoss.resizeGrid();
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                DailyRacingWinLoss.submitForm();
                return false;
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
            });

            $("#data-content").on("click", ".bet-list-link", function () {
                var target = $(this);
                var leagueId = target.attr("data-league");
                var raceNo = target.attr("data-race-no");
                var currency = target.attr("data-currency");
                DailyRacingWinLoss.viewRacingWinlossBetList(leagueId, raceNo, currency);
            });
        },
        viewRacingWinlossBetList: function (leagueid, raceNo, currency) {
            var url = Site.resolveClientUrl("BetList/DailyRacingWl");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "leagueid", leagueid);
            url = QueryString.setParam(url, "raceno", raceNo);
            url = QueryString.setParam(url, "type", "RacingWinloss");
            url = QueryString.setParam(url, "currencyid", currency);

            Site.blockUI();
            window.location.href = url;
        },
        submitForm: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#ReportForm').submit();
        }
    };
})(jQuery);
$(window).load(function () {
    "use strict";
    DailyRacingWinLoss.initialize();
});

$(window).resize(function () {
    "use strict";
    DailyRacingWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});