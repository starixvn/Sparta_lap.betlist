/*global Site, RefreshCountDown, QueryString, ForecastPopup*/
/// <reference path="/@JSense.js" />
"use strict";
var ForecastOneXTwo = (function ($) {
    return {
        resizeGrid: function () {
            var $grid = $(".data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;

            var gridHeight = window.innerHeight - gridTop - 30;
            gridHeight = Math.max(150, gridHeight);
            gridHeight = Math.min($grid.height(), gridHeight);
            $grid.css("height", gridHeight);
        },
        initialize: function () {
            Site.unBlockUI();
            // Init refresh count down.
            RefreshCountDown.initialize();

            ForecastOneXTwo.initEvent();
            ForecastOneXTwo.resizeGrid();
            Site.addUserGuide({
                resourceName: "Forecast1x2",
                title: Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("OneXTwo")
            });
        },
        initEvent: function () {
            // Check popup
            var isPopup = $('#IsPopUp').val();
            if (isPopup === "true") {
                $('.content-title').hide();
                $('#forecast-filter').hide();

                $('.data-content').attr('style', 'max-height:300px!important');
            }

            // League change
            $('.league-list').on('change', function () {
                var leagueId = $(this).val();
                ForecastOneXTwo.loadSelectedLeague(leagueId);
            });

            // Forecast button click
            $('#Refresh-Content').on('click', '.button-add', function () {
                var matchId = $(this).attr('data-match-id');
                var betType = parseInt($(this).attr('data-bettype'));
                ForecastOneXTwo.openForecast(matchId, betType, isPopup);
            });

            // Submit button click
            $('#SubmitButton').on('click', function () {
                ForecastOneXTwo.submit();
            });
        },
        openForecast: function (matchId, betType, isPopup) {
            var url = Site.resolveClientUrl("BetList/RunningBetListOfMatch");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "bettype", betType);

            if (isPopup === "true") {
                var popH = Math.min(parent.innerHeight - 100, 650), popW = 900;
                ForecastPopup.resizePopup(popW, popH, url);
            } else {
                window.location.href = url;
            }
        },
        loadSelectedLeague: function (leagueId) {
            var url = Site.resolveClientUrl("Forecast/ForecastOneXTwo");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "LeagueId", leagueId);
            ForecastOneXTwo.submit();
        },
        submit: function () {
            Site.blockUI();
            $("#ForecastOneXTwoForm").submit();
        }
    };
})(jQuery);

$(window).load(function () {
    var height = Math.min(parent.innerHeight - 20, 460);
    // Set default value for popup
    parent.$('.ui-dialog').css({ 'width': 700 + 'px' });
    parent.$('.ui-dialog').css({ 'height': height + 'px' });
    parent.$('.ui-dialog').css({ 'top': (parent.innerHeight - height) / 2 + 'px' });
    parent.$('.ui-dialog').css({ 'left': '20%' });
    parent.$('.ui-dialog').css({ 'margin': 'auto' });
    parent.$('.ui-dialog-title').html(Site.getSingleResource('Forecast') + ' - ' + Site.getSingleResource('OneXTwo'));

    parent.$('#myframe').attr('width', 690);
    parent.$('#myframe').attr('height', height);
    parent.$('#forecast-dialog').attr('style', 'width:680px;height:' + (height - 60) + 'px');
    parent.$('#forecast-dialog').css({ 'overflow': 'hidden' });
});

$(window).resize(function () {
    ForecastOneXTwo.resizeGrid();
});

$(window).unload(function () {
    $(window).unbind("resize");
});