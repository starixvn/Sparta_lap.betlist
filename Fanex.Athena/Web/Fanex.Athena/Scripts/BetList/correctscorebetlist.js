/// <reference path="/@JSense.js" />

var CorrectScoreBetList = (function ($) {
    return {
        resizeGrid: function () {
            var gridTop = $("#BetListGrid").offset().top;
            var gridHeight = window.innerHeight - gridTop - 40;
            gridHeight = Math.max(130, gridHeight);

            $(".ui-jqgrid-bdiv").css("max-height", gridHeight);
            if ($(".ui-jqgrid-bdiv>div").height() > gridHeight) {
                $(".ui-jqgrid-bdiv").css("height", gridHeight);
            }
        },
        initialize: function () {
            CorrectScoreBetList.initEvent();
            CorrectScoreBetList.submit();
        },

        initEvent: function () {
            //Submit button
            $('#SubmitButton').click(function () {
                CorrectScoreBetList.submit();
            });
        },

        submit: function () {
            // Site.blockUI();
            BetList.initialize();
            // Load Data
            var params = {};
            params = Site.setLicenseeOptionParam(params);
            var url = Site.resolveClientUrl('api/betlist/CorrectScoreBetList');
            BetList.loadData(params, url, setTimeout(function () {
                CorrectScoreBetList.resizeGrid();
            }, 300));
        }
    };
})(jQuery);

$(window).load(function () {
    CorrectScoreBetList.initialize();
});