/*global Site, QueryString, LiveReport*/
/// <reference path="/@JSense.js" />
var LiveReportMatchList = (function ($) {
    "use strict";
    return {
        initialize: function () {
            LiveReportMatchList.initEvent();
            LiveReportMatchList.resizeContent();
        },
        initEvent: function () {
            $("body").on("click", ".match-detail-link", function () {
                LiveReportMatchList.viewMatchBetList(this);
            });
        },

        viewMatchBetList: function (target) {
            var url = "LiveReportBetList/Match";
            var currencyName = $('#CurrencyName').attr('data-original');
            url = Site.resolveClientUrl(url);

            var data = $(target).attr("data-drill-down");
            data = $.parseJSON(data);
            url = Site.setLicenseeOptionOriginalParam(url);
            url = Site.setDateRangeOriginalParam(url);
            url = QueryString.setParam(url, "CurrencyName", currencyName);
            url = QueryString.setParam(url, "MatchId", data.MatchId);
            url = QueryString.setParam(url, "HomeId", data.HomeId);
            url = QueryString.setParam(url, "AwayId", data.AwayId);
            url = QueryString.setParam(url, "BetType", data.BetType);
            url = QueryString.setParam(url, "LiveIndicator", data.LiveIndicator);
            url = LiveReport.setSumParams(url, data);

            var productOptions = LiveReport.getMatchListProductOption($("#SportId").val(), $("#ProductCode").val());

            url = QueryString.setParam(url, "ProductCodeList", productOptions);
            Site.blockUI();
            location.href = url;
        },
        resizeContent: function () {
            var contentWidth = $(".data-content").width();
            $(".content-main").width(contentWidth);
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    LiveReportMatchList.initialize();
});