/*global jqGridAdditional, MicroGamingWinLossOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var MicroGamingWinLoss = (function ($) {
    "use strict";
    var VALID_MONTH_RANGE = 3;
    var ajaxRequest = null;

    return {
        resizeGrid: function () {
            setTimeout(function () {
                if ($(".ui-jqgrid-bdiv").length === 0) {
                    return;
                }

                var gridTop = $(".ui-jqgrid-bdiv").offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv:visible").height()
                                - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(120, gridHeight);

                var gridContainer = $("#gview_ReportGrid");
                var bodyHeight = window.innerHeight - gridContainer.offset().top;
                var actualHeight = $("#ReportGrid").height();
                var contentHeight = actualHeight > bodyHeight ? bodyHeight : actualHeight;
                var tableWidth = $("#ReportGrid").width();
                var contentWidth = $(".ui-jqgrid-bdiv").width();
                var newHeight = tableWidth > contentWidth ? contentHeight + 18 : contentHeight;
                $(".ui-jqgrid-bdiv").height(Math.min(newHeight, gridHeight))
                                    .css("max-height", gridHeight);
            }, 100);
        },
        initialize: function () {
            MicroGamingWinLoss.initEvent();
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, MicroGamingWinLossOption.urlData);
            
            // Resize grid when pressing collapse/expand group
            $(".report-content").on("click", ".tree-wrap-ltr", MicroGamingWinLoss.resizeGrid);
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                $('#IsExportExcel').val(false);
                MicroGamingWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#MicroGamingWinLossForm");
                var url = Site.resolveClientUrl(MicroGamingWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = $('#ReportTitle').val() + ', {0} → {1} - {2}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyTextList =
            {
                "-3": viewinKey.replace("{0}", "GBP"),
                "-2": viewinKey.replace("{0}", "USD"),
                "-1": viewinKey.replace("{0}", "EUR"),
                "0": Site.getSingleResource('AllCurrencies')
            };

            var currencyStr = currencyTextList[currencyId] === undefined
                ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + currencyId + '"]').text())
                : currencyTextList[currencyId];

            var title = $.format(titlePattern, fromdate.toString("MM/dd/yyyy"), todate.toString("MM/dd/yyyy"), currencyStr);
            $("div.report-title").html(title);
        },
        //#end

        //#Get Data
        ajaxComplete: function (response) {
            if (response != null) {
                MicroGamingWinLoss.setReportTitle($("#CurrencyId").val(),
                     $("#FromDate").val(), $("#ToDate").val());
                MicroGamingWinLoss.bindData(response);
            }
        },

        submit: function () {
            var mainForm = $("#MicroGamingWinLossForm");
            var postdata = mainForm.serializeObject();
            if (!postdata.FromDate) {
                var fromDate = new Date(postdata.Year, postdata.Month - 1, postdata.FromDay);
                var toDate = new Date(postdata.Year, postdata.Month - 1, postdata.ToDay);

                postdata.FromDate = fromDate.toDateString();
                postdata.ToDate = toDate.toDateString();
            }

            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(MicroGamingWinLossOption.urlData),
                success: function (response) {
                    MicroGamingWinLoss.ajaxComplete(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
        },

        //#BindReport
        bindTotal: function (totalData, summaryTotal) {
            var columnModel = MicroGamingWinLossOption.getTotalColModel();
            var colspan = MicroGamingWinLossOption.totalColspan;
            var summaryColspan = colspan + 1;

            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        },

        bindData: function (response) {
            var $gridBody = $(".ui-jqgrid-bdiv");

            var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;

            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");

            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;

            var gridOption = MicroGamingWinLossOption.getGridOption();
            gridOption.data = response.Data;
            gridOption.rowTotal = numOfRow;
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);
                $("#pg_ppdata").hide();
                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    MicroGamingWinLoss.bindTotal(response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    MicroGamingWinLoss.resizeGrid();
                }

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            reportGrid.jqGrid('showCol', 'rn');

            jqGridAdditional.resetLayout(reportGrid);

            $gridBody = $(".ui-jqgrid-bdiv");
            if ($gridBody.length > 0) {
                $gridBody[0].scrollLeft = scrollLeft;
            }
        }
        //#end
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    MicroGamingWinLoss.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    MicroGamingWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});
