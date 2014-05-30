/// <reference path="/@JSense.js" />
var OddEvenDND = (function ($) {
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
        initialize: function () {
            Totalbets.initialize();
            OddEvenDND.initEvent();
            OddEvenDND.resizeGrid();

            $(document).ajaxComplete(function (event, request, settings) {
                OddEvenDND.resizeGrid();
            });
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                OddEvenDND.submit();
            });

            // Forcast button click
            $("#Refresh-Content").on("click", '.button-add', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                OddEvenDND.viewForecast1X2(matchId, betType);
            });
        },
        viewForecast1X2: function (matchId, betType) {
            var url = Site.resolveClientUrl("Forecast/ForecastOneXTwoPopup?matchid=" + matchId + '&bettype=' + betType + '&ispopup=true');
            url = Site.setLicenseeOptionOriginalParam(url);

            var popH = 300, popW = 700;

            ForecastPopup.showPopup(popW, popH, url);
        },
        submit: function () {
            Site.blockUI();
            $("#OddEvenDrawNoDrawForm").submit();
        }
    };
})(jQuery);