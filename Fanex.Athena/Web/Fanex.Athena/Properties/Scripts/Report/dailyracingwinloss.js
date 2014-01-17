/// <reference path="/@JSense.js" />
var DailyRacingWinLoss = (function ($) {
    return {
        initialize: function () {
            DailyRacingWinLoss.initEvent();
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                DailyRacingWinLoss.submitForm();
                return false;
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
            });

            $("#data-content").on("click", ".bet-list-link", function (e) {
                var target = $(this);
                var leagueId = target.attr("data-league");
                var raceNo = target.attr("data-race-no");
                var currency = target.attr("data-currency");
                DailyRacingWinLoss.viewRacingWinlossBetList(leagueId, raceNo, currency);
            });
        },
        viewRacingWinlossBetList: function (leagueid, raceNo, currency) {
            var url = resolveClientUrl("BetList/DailyRacingWl");
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

    }
})(jQuery);
$(window).load(function () {
    DailyRacingWinLoss.initialize();
});