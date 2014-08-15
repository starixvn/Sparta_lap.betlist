/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
"use strict";
var RacingCommission = (function ($) {
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $(".total-content").height() - 30;
            gridHeight = Math.max(120, gridHeight);

            $grid.css("height", "auto").css("max-height", gridHeight);
            $(".report-content").css("max-height", "");
        },
        initialize: function () {
            RacingCommission.initEvent();
            Site.createSortingIcon(RacingCommission.sortReport);
        },
        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#ReportForm').submit();
            });
            $('#SubmitButton').click(function () {
                $("#SortColumn").val("ExternalID");
                $("#SortDirection").val("ASC");
                RacingCommission.submitForm();
            });

            $('#sortReport').click(function () {
                RacingCommission.sortReport();
            });

            $("#data-content").on("click", ".bet-list-link", function () {
                var target = $(this);
                var custId = target.attr("data-cust-id");
                var systemId = target.attr("data-system-id");
                var externalId = target.attr("data-external-id");
                RacingCommission.viewRacingBetList(custId, systemId, externalId);
            });
        },

        viewRacingBetList: function (custid, systemId, externalId) {
            var url = Site.resolveClientUrl("BetList/RacingCommission");
            url = Site.setProductOptionParam(url, 0, "RB", true);
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").attr("data-original"));
            url = QueryString.setParam(url, "todate", $("#ToDate").attr("data-original"));
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemId", systemId);
            url = QueryString.setParam(url, "externalId", externalId);

            Site.blockUI();
            window.location.href = url;
        },
        submitForm: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#ReportForm').submit();
        },
        sortReport: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#ReportForm').submit();
        }
    };
})(jQuery);

$(window).load(function () {
    RacingCommission.initialize();
    RacingCommission.resizeGrid();
});

$(window).resize(function () {
    RacingCommission.resizeGrid();
});

$(window).unload(function () {
    $(window).unbind("resize");
});