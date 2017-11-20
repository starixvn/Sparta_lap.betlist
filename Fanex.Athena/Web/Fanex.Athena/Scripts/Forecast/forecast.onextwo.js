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
                ForecastOneXTwo.submit();
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
            var url = Site.resolveClientUrl("RunningMatchBetList/Index");
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
        submit: function () {
            Site.blockUI();
            $("#ForecastOneXTwoForm").submit();
        }
    };
})(jQuery);

if (!window._IsInJsTestingMode) {
    $(window).load(function () {
        var parentWindow;
        try {
            parentWindow = window.parent.location.host === window.location.host ? window.parent : window;
        }catch(exception) {
            parentWindow = window;
        }

        var height = Math.min(parentWindow.innerHeight - 20, 460);
        var top = (parentWindow.innerHeight - height) / 2;

        // Set default value for popup
        parentWindow.$('.ui-dialog').css({ width: 700 + 'px', height: height + 'px', top: top + 'px', left: '20%', margin: 'auto' });
        parentWindow.$('.ui-dialog-title').html(Site.getSingleResource('Forecast') + ' - ' + Site.getSingleResource('OneXTwo'));
        
        parentWindow.$('#myframe').attr('width', 690, 'height', height);
        parentWindow.$('#forecast-dialog')
            .attr('style', 'width:680px;height:' + (height - 60) + 'px')
            .css({ 'overflow': 'hidden' });
    });

    $(window).resize(function () {
        ForecastOneXTwo.resizeGrid();
    });

    $(window).unload(function () {
        $(window).unbind("resize");
    });
}