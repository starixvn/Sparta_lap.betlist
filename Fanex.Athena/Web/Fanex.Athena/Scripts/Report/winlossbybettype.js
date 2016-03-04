/*global Site*/
/// <reference path="/@JSense.js" />
var WinLossByBetType = (function ($) {
    "use strict";
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $(".total-content").height() - 40;
            gridHeight = Math.max(120, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initialize: function () {
            WinLossByBetType.initEvent();
            Site.createSortingIcon(WinLossByBetType.sortReport);
            WinLossByBetType.resizeGrid();
            Site.addUserGuide({
                resourceName: "WinLossByBetType",
                title: Site.getSingleResource("WinLossByBetType")
            });
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#WinLossByBetTypeForm').submit();
            });

            $('#SubmitButton').click(function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                $('#WinLossByBetTypeForm').submit();
            });

            $("#data-content").on("click", ".bettype-link", function () {
                WinLossByBetType.viewBetList($(this).data("bettype"));
            });
        },
        viewBetList: function (betType) {
            var url = Site.resolveClientUrl("BetList/WinLossByBetType");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").val());
            url = QueryString.setParam(url, "todate", $("#ToDate").val());
            url = QueryString.setParam(url, "bettype", betType);

            Site.blockUI();
            window.location.href = url;
        },
        sortReport: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#WinLossByBetTypeForm').submit();
            return false;
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    WinLossByBetType.initialize();
});

$(window).resize(function () {
    "use strict";
    WinLossByBetType.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});