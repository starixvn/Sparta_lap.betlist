/*global Site*/
/// <reference path="/@JSense.js" />
var WinLossByBetType = (function ($) {
    "use strict";
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length == 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $(".total-content").height() - 25;
            gridHeight = Math.max(120, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initialize: function () {
            WinLossByBetType.initEvent();
            Site.createSortingIcon(WinLossByBetType.sortReport);
            WinLossByBetType.resizeGrid();
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
    var reportGrid = $("#ReportGrid");
    WinLossByBetType.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});

