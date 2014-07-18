"use strict";
/*global Site */

var PlaytechCasino = (function ($) {
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $("#ContentMainFooter").height() - 30;
            gridHeight = Math.max(120, gridHeight);
            $grid.css("height", "auto");
            $("#ContentMainContent").css("max-height", gridHeight);
        },
        initialize: function () {
            PlaytechCasino.initEvent();
            PlaytechCasino.resizeGrid();
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#PlaytechCasinoExcelForm').submit();
            });

            $(".drill-down-link").click(function () {
                PlaytechCasino.drillDown(this);
            });

            $(".level-link").click(function () {
                PlaytechCasino.goBack(this);
            });

            //Submit button
            $('#SubmitButton').click(function () {
                PlaytechCasino.submit();
            });

            var contentWidth = $(".data-content").width() + 5;
            $(".content-filter").width(contentWidth < 650 ? 650 : contentWidth);
        },
        submit: function () {
            Site.blockUI();
            $("#Level").val(0);
            $('#IsExportExcel').val(false);
            $('#PlaytechCasinoForm').submit();
        },
        drillDown: function (context) {
            var drillDownLevel = $(context).attr("data-level");
            var drillDownCurrency = $(context).attr("data-currency");
            var drillDownDate = $(context).attr("data-date");
            var drillDownCust = $(context).attr("data-custid");

            $('#IsExportExcel').val(false);
            $("#Level").val(drillDownLevel);
            $("#DrillDownCurrency").val(drillDownCurrency);
            $("#CustId").val(drillDownCust);
            if (drillDownDate !== undefined) {
                $("#DrillDownFromDate").val(drillDownDate);
                $("#DrillDownToDate").val(drillDownDate);
            }

            $('#PlaytechCasinoForm').submit();
        },
        goBack: function () {
            // var currentLevel = parseInt($(".current-level").attr("data-level"), 10);
            // var licenseeId = parseInt($("#LicenseeId").val(), 10);
            // var goBackLevel = parseInt($(context).attr("data-level"), 10);

            // + LB365 Levels:  0 - licensee, 1 - member
            // + Others (not implemented yet): 0 - licensee, 1 - agent, 2 - member
            // go back licensee level
            PlaytechCasino.submit();
        }
    };
})(jQuery);

$(window).load(function () {
    PlaytechCasino.initialize();
});