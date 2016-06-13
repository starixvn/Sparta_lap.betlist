/*global Site, RefreshCountDown, QueryString, ForecastPopup*/
/// <reference path="/@JSense.js" />
"use strict";
var Forecast = (function ($) {
    var matchid, baseCurrency, licenseetype, currencyid, currencyName;

    return {
        initialize: function () {
            // Init private variables
            matchid = $('#forecast-table').attr('data-match-id');
            baseCurrency = $('#forecast-table').attr('data-basecurrency');
            licenseetype = $('#forecast-table').attr('data-licenseetype');
            currencyid = $('#forecast-table').attr('data-currencyid');
            currencyName = $('#forecast-table').attr('data-currencyname');

            // Init countdown
            RefreshCountDown.initialize();
            Forecast.initEvent();
        },
        initEvent: function () {
            // Forecast link click
            $('#Refresh-Content').on('click', '.forecast-link', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                var selectedScore = parseInt($(this).attr('data-current-score'));
                Forecast.loadForecast(selectedScore, bettype);
            });

            // View forecast click
            $('#Refresh-Content').on('click', '.view-forecast-link', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                Forecast.loadForecast(null, bettype);
            });

            // View betlist click
            $('#Refresh-Content').on('click', '.view-betlist', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                Forecast.viewBetlist(bettype);
            });
        },

        loadForecast: function (selectedScore, bettype) {
            var url = Site.resolveClientUrl("AhForecast/Index?matchid=" + matchid + '&bettype=' + bettype);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);
            url = QueryString.setParam(url, "LicenseeType", licenseetype);
            url = QueryString.setParam(url, "CurrencyId", currencyid);
            if (selectedScore != null) {
                url = QueryString.setParam(url, "CurrentScore", selectedScore);
            }
            $("#BetType").val(bettype);
            $("#CurrentScore").val(selectedScore);

            $.ajax({
                url: url,
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (data) {
                    Site.unBlockUI();
                    $('#Refresh-Content').html(data);
                    RefreshCountDown.initialize();
                    RefreshCountDown.initEvent();
                    $("#txt-currentscore").focus();
                },
                type: "POST",
                async: true,
                dataType: "html"
            });
        },

        viewBetlist: function (bettype) {
            //var currencyName = $("#CurrencyId option:selected").text();

            var url = Site.resolveClientUrl("RunningMatchBetList/Index");
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);
            url = QueryString.setParam(url, "currencyid", currencyid);
            url = QueryString.setParam(url, "currencyname", currencyName);
            url = QueryString.setParam(url, "licenseetype", licenseetype);

            var popH = Math.min(parent.innerHeight - 100, 700), popW = 960;

            ForecastPopup.resizePopup(popW, popH, url);
        },
        currentScoreKeyUp: function (evt) {
            if (!evt) {
                evt = window.event;
            }
            if (evt.keyCode === 13) {
                $("#btn-currentscore").click();
            }
        },
        setCurrentScore: function () {
            var data = $("#txt-currentscore").val();
            if (data === null || data === undefined || !Site.isInteger(data)) {
                return;
            }
            var bettype = $("#Refresh-Content a.view-betlist").attr("data-bettype");
            Forecast.loadForecast(data, bettype);
        }
    };
})(jQuery);

if (!window._IsInJsTestingMode) {
    $(window).load(function () {
        parent.$('.ui-dialog').css({ 'width': 725 + 'px' });
        parent.$('.ui-dialog').css({ 'height': 330 + 'px' });
        parent.$('.ui-dialog').css({ 'position': 'absolute' });
        parent.$('.ui-dialog').css({ 'top': '25%' });
        parent.$('.ui-dialog').css({ 'left': '20%' });
        parent.$('.ui-dialog').css({ 'margin': 'auto' });
        parent.$('.ui-dialog-title').html(Site.getSingleResource("Forecast"));
        parent.$('#myframe').attr('width', '100%');
        parent.$('#myframe').attr('height', '100%');
        parent.$('#forecast-dialog').attr('style', 'width:700px;height:280px');
        parent.$('#forecast-dialog').css({ 'overflow': 'hidden' });
    });
}