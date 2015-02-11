/*global jqGridAdditional, AgCasinoBalanceOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var AgCasinoBalance = (function ($) {
    "use strict";
    var cacheData = {};
    var ajaxRequest = null;
    var _submitData = null;
    var _level = 0;
    function initMonthPicker() {
        var year = $("#Year").val();
        var month = $("#Month").val();
        var fromDay = $("#FromDay").val();
        var toDay = $("#ToDay").val();

        $('#DateControl').monPickr({
            minDate: $('#MinDate').val(),
            maxDate: $('#ToDate').val(),
            showDateRange: true,
            fromDayId: 'MonthPickerFromDay',
            toDayId: 'MonthPickerToDay',
            month: new Date(year, month - 1, fromDay),
            fromDay: fromDay,
            toDay: toDay,
            afterChangeMonth: function () {
                var fromDate = $("#DateControl").monPickr("getFromDate");
                $('#Year').val(fromDate.getFullYear());
                $('#Month').val(fromDate.getMonth() + 1);
            },
            afterChangeDay: function () {
                var fromDate = $("#DateControl").monPickr("getFromDate");
                var toDate = $("#DateControl").monPickr("getToDate");
                $("#FromDate").val(fromDate.toString("MM/dd/yyyy"));
                $("#ToDate").val(toDate.toString("MM/dd/yyyy"));
                $('#FromDay').val(fromDate.getDate());
                $('#ToDay').val(toDate.getDate());
            },
            log: function (/*msg*/) {
                //console.warn(msg);
            }
        });
    }

    return {
        resizeGrid: function () {
            setTimeout(function () {
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
            AgCasinoBalance.initEvent();
            initMonthPicker();
            Site.addUserGuide({
                resourceName: "AgCasinoBalance",
                title: Site.getSingleResource("AgCasinoBalance")
            });
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _level = 0;
                AgCasinoBalance.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#AgCasinoBalanceForm");
                var url = Site.resolveClientUrl(AgCasinoBalanceOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                _level = level;
                return AgCasinoBalance.goBack(level);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            $("#DrillDownCurrencyId").val(currencyId);
            $("#DrillDownCustomerId").val(custId);
            $("#DrillDownFromDate").val(fdate.toString("MM/dd/yyyy"));
            $("#DrillDownToDate").val(tdate.toString("MM/dd/yyyy"));
            $("#DrillDownLevel").val(level);
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
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('AGCasinoBalance') + ', {0} → {1} - {2}';
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

            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"), currencyStr);
            $("div.report-title").html(title);
        },
        //#end

        //#Get Data
        ajaxComplete: function (level, response) {
            level = Number(level);
            if (response != null) {
                cacheData[level] = response;
                if (level === 0) {
                    AgCasinoBalance.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());
                }

                AgCasinoBalance.renderBreadCrumb(level);
                AgCasinoBalance.bindData(level, response);
            }
        },

        submit: function () {
            var mainForm = $("#AgCasinoBalanceForm");
            var postdata = mainForm.serializeObject();
            var fromDate = new Date(postdata.Year, postdata.Month - 1, postdata.FromDay);
            var toDate = new Date(postdata.Year, postdata.Month - 1, postdata.ToDay);

            postdata.FromDate = fromDate;
            postdata.ToDate = toDate;
            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(AgCasinoBalanceOption.urlData),
                success: function (response) {
                    AgCasinoBalance.setExcelUrl(0, 0, 0, "", "");
                    _submitData = postdata;
                    AgCasinoBalance.ajaxComplete(0, response);
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

        drillDown: function (level, currencyId, customerId, datetime) {
            level = Number(level);

            if (_level !== level) {
                _level = level;
            }

            var fdate = new Date(Date.parse(datetime));
            var tdate = new Date(Date.parse(datetime));

            var param = {
                IsExcludedTestData: _submitData.IsExcludedTestData,
                CurrencyId: _submitData.CurrencyId,
                FromDate: _submitData.FromDate,
                ToDate: _submitData.ToDate,
                BaseCurrency: _submitData.BaseCurrency,
                DrillDownCurrencyId: currencyId,
                DrillDownFromDate: fdate.toString("MM/dd/yyyy"),
                DrillDownToDate: tdate.toString("MM/dd/yyyy"),
                DrillDownCustomerId: customerId,
                DrillDownLevel: level
            };

            var postData = JSON.stringify(param);

            var option = {
                type: "POST",
                data: postData,
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: Site.resolveClientUrl(AgCasinoBalanceOption.urlData),
                success: function (response) {
                    AgCasinoBalance.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && Number(param.CurrencyId) === 0) {
                            AgCasinoBalance.setReportTitle(currencyId, fdate, tdate);
                        } else {
                            AgCasinoBalance.setReportTitle(param.CurrencyId, fdate, tdate);
                        }
                        //update excel ULR
                        AgCasinoBalance.setExcelUrl(level, currencyId, customerId, fdate, tdate);
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
                AgCasinoBalance.renderBreadCrumb(level);
                AgCasinoBalance.bindData(0, cacheData[level]);
                //reset excel Link
                AgCasinoBalance.setExcelUrl(0, 0, 0, "", "");
                //re-set title
                var fromdate = _submitData.FromDate;
                var todate = _submitData.ToDate;
                var currencyId = _submitData.CurrencyId;
                AgCasinoBalance.setReportTitle(currencyId, fromdate, todate);
            }
            else if (level === 1) {
                AgCasinoBalance.bindData(1, cacheData[level]);
                $("#DrillDownLevel").val(level);
                AgCasinoBalance.renderBreadCrumb(level);
            }
        },

        //#BindReport
        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = AgCasinoBalanceOption.getTotalColModel(level);
            var colspan = AgCasinoBalanceOption.totalColspan[level];
            var summaryColspan = colspan + 1;

            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        },

        bindData: function (level, response) {
            level = Number(level);
            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");

            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;

            var gridOption = AgCasinoBalanceOption.getGridOption(level);
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
                    AgCasinoBalance.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    AgCasinoBalance.resizeGrid();
                }

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            reportGrid.jqGrid('showCol', 'rn');

            jqGridAdditional.resetLayout(reportGrid);
        }
        //#end
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    AgCasinoBalance.initialize();
    //alert(Date.parse('Apr 5, 2013'));
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    AgCasinoBalance.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});