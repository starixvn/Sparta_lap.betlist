/*global Site, QueryString, LiveReport*/
/// <reference path="/@JSense.js" />
define(["querystring", "site", "./livereport"], function (QueryString, Site, LiveReport) {
    "use strict";

    $(initialize);

    function initialize() {
        initEvent();
    }

    function initEvent() {
        $("body").on("click", ".match-detail-link", function () {
            viewMatchBetList(this);
        });
    }

    function viewMatchBetList(target) {
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

        $(productOptions).each(function (index, element) {
            url = Site.setProductOptionParam(url, index, element.toUpperCase(), true);
        });

        Site.blockUI();
        location.href = url;
    }
});