/*global jqGridAdditional, GoldDeluxeWinLossOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var GoldDeluxeWinLoss = (function ($) {
    "use strict";
    var cacheData = {};
    var ajaxRequest = null;
    var _level = 0;
    var _submitData;
    var VALID_MONTH_RANGE = 3;
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
            GoldDeluxeWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
            GoldDeluxeWinLoss.initEvent();
            Site.addUserGuide({
                resourceName: "GoldDeluxeWinLoss",
                title: Site.getSingleResource("GoldDeluxeWinLoss")
            });
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, GoldDeluxeWinLossOption.urlExcelData);
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _level = 0;
                GoldDeluxeWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#GoldDeluxeWinLossForm");
                var url = Site.resolveClientUrl(GoldDeluxeWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                _level = level;
                return GoldDeluxeWinLoss.goBack(level);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/report/GoldDeluxeWinLoss';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }

            GoldDeluxeWinLossOption.urlExcelData = temp;
        },

        renderBreadCrumb: function (level) {
            //hide history link
            level = Number(level);
            if (level === 0) {
                $(".breadCrumbHolder").hide();
            }
            else {
                /*show history link*/
                $(".breadCrumbHolder").show();
                $("#history-link-container").width($("#ReportGrid").width() + 17);
                //add more 20px for scrolling
                $("div#history-link-container li.level0").show();

                /*show history link*/
                $(".breadCrumbHolder").show();
                $(".level0").show();

                $(".level2").showHide(level === 2);
                $(".level1").showHide(GoldDeluxeWinLossOption.licenseeId === GoldDeluxeWinLossOption.LICENSEE_12BET);
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('GoldDeluxeWinLoss') + ', {0} → {1} - {2}';
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
        ajaxComplete: function (level, response) {
            level = Number(level);
            if (response != null) {
                var reportBy = $("input[name='reportby']:checked").val();
                cacheData[level] = response;
                //reset layout
                if (level === 0) {
                    _submitData = {
                        IsExcludedTestData: $("#IsExcludedTestData").is(":checked"),
                        CurrencyId: $("#CurrencyId").val(),
                        FromDate: $("#FromDate").val(),
                        ToDate: $("#ToDate").val(),
                        CurrencyName: $("#CurrencyId option:selected").text(),
                        BaseCurrency: $("#BaseCurrency").val(),
                        ReportBy: reportBy
                    };

                    GoldDeluxeWinLoss.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }

                GoldDeluxeWinLoss.renderBreadCrumb(level);
                GoldDeluxeWinLoss.bindData(level, response);
            }
        },

        submit: function () {
            var mainForm = $("#GoldDeluxeWinLossForm");
            var postdata = mainForm.serializeObject();

            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(GoldDeluxeWinLossOption.urlData),
                success: function (response) {
                    GoldDeluxeWinLoss.setExcelUrl(0, 0, 0, "", "");
                    GoldDeluxeWinLoss.ajaxComplete(0, response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
        },

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        drillDown: function (level, currencyId, customerId, datetime) {
            level = Number(level);

            if (_level !== level) {
                _level = level;
            }

            var param =
            {
                IsExcludedTestData: _submitData.IsExcludedTestData,
                CurrencyId: _submitData.CurrencyId,
                FromDate: _submitData.FromDate,
                ToDate: _submitData.ToDate,
                BaseCurrency: _submitData.BaseCurrency,
                ReportBy: _submitData.ReportBy
            };
            var fdate = new Date(Date.parse(datetime));
            var tdate = new Date(Date.parse(datetime));
            if (Number(param.ReportBy) === 2) {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = new Date(param.FromDate);
                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                fdate = new Date(year, month, fday, 0, 0, 0, 0);
                var originalTDate = new Date(param.ToDate);

                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    tdate = GoldDeluxeWinLoss.lastDateOfMonth(year, month);
                }
            }

            var option = {
                type: "POST",
                data: JSON.stringify(param),
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: Site.resolveClientUrl(GoldDeluxeWinLossOption.urlData +
                    "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId + "&fdate=" +
                    fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                success: function (response) {
                    GoldDeluxeWinLoss.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && Number(param.CurrencyId) === 0) {
                            GoldDeluxeWinLoss.setReportTitle(currencyId, fdate, tdate);
                        } else {
                            GoldDeluxeWinLoss.setReportTitle(param.CurrencyId, fdate, tdate);
                        }
                        //update excel ULR
                        GoldDeluxeWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate);
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);
        },

        goBack: function (level) {
            if (level === 0) {
                GoldDeluxeWinLoss.renderBreadCrumb(level);
                GoldDeluxeWinLoss.bindData(0, cacheData[level]);
                //reset excel Link
                GoldDeluxeWinLoss.setExcelUrl(0, 0, 0, "", "");
                //re-set title
                var fromdate = new Date(_submitData.FromDate);
                var todate = new Date(_submitData.ToDate);
                var currencyId = _submitData.CurrencyId;
                GoldDeluxeWinLoss.setReportTitle(currencyId, fromdate, todate);
            }
            else if (level === 1) {
                GoldDeluxeWinLoss.bindData(1, cacheData[level]);
                GoldDeluxeWinLossOption.urlExcelData = GoldDeluxeWinLossOption.urlExcelData.replace("level=2", "level=1");
                GoldDeluxeWinLoss.renderBreadCrumb(level);
            }
        },

        //#BindReport
        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = GoldDeluxeWinLossOption.getTotalColModel(level);
            var colspan = GoldDeluxeWinLossOption.totalColspan[level];
            var summaryColspan = colspan + 1;

            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        },

        bindData: function (level, response) {
            level = Number(level);
            var $gridBody = $(".ui-jqgrid-bdiv");

            var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;

            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");

            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;

            var gridOption = GoldDeluxeWinLossOption.getGridOption(level);
            gridOption.data = response.Data;
            gridOption.rowTotal = numOfRow;

            gridOption.gridComplete = function () {
                level = Number(level);
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);
                $("#pg_ppdata").hide();
                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    GoldDeluxeWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    GoldDeluxeWinLoss.resizeGrid();
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
    GoldDeluxeWinLoss.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    GoldDeluxeWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});