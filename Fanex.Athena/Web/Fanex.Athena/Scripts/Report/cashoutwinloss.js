/*global jqGridAdditional, CashOutWinLossOption, Report, Site, QueryString */
/// <reference path="/@JSense.js" />
///
var CashOutWinLoss = (function ($) {
    "use strict";
    var cacheData = {};
    var ajaxRequest = null;
    var totalRowCount = 0;
    var _level = 0;
    var VALID_MONTH_RANGE = 3;
    var _submitData;
    return {
        getTotalRowCount: function () {
            return totalRowCount;
        },
        initialize: function () {
            CashOutWinLossOption.isLicenseeOfferBA = $("#IsLicenseeOfferBA").val() === "1";
            CashOutWinLoss.initEvent();
            Site.addUserGuide({
                resourceName: "CashOutWL",
                title: Site.getSingleResource("CashOutWinLoss")
            });
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, CashOutWinLossOption.urlExcelData);
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _level = 0;
                CashOutWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#MainForm");
                var url = Site.resolveClientUrl(CashOutWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                _level = level;
                return CashOutWinLoss.goBack(level);
            });

            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                CashOutWinLoss.openBetlistPopUp(dataDrillDown);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/CashOutWinLossReport/Index';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }

            CashOutWinLossOption.urlExcelData = temp;
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
            var titlePattern = Site.getSingleResource('CashOutWinLoss') + ', {0} → {1} - {2}';
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
                //reset layout
                if (level === 0) {
                    //update drildownValue
                    CashOutWinLoss.setReportTitle($("#CurrencyId").val(), _submitData.FromDate, _submitData.ToDate);
                }

                CashOutWinLoss.renderBreadCrumb(level);
                CashOutWinLoss.bindData(level, response);
            }
        },

        submit: function () {
            var mainForm = $("#MainForm");
            var postdata = mainForm.serializeObject();
            _submitData = postdata;
            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CashOutWinLossOption.urlData),
                success: function (response) {
                    CashOutWinLoss.setExcelUrl(0, 0, 0, "", "");
                    CashOutWinLoss.ajaxComplete(0, response);
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

            if (currencyId === 0) {
                currencyId = _submitData.CurrencyId;
            }

            if (_level !== level) {
                _level = level;
            }

            var param =
            {
                IsExcludedTestData: _submitData.IsExcludedTestData,
                CurrencyId: _submitData.CurrencyId,
                FromDate: _submitData.FromDate,
                ToDate: _submitData.ToDate,
                ReportBy: _submitData.ReportBy,
                BaseCurrency: _submitData.BaseCurrency
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
                    tdate = CashOutWinLoss.lastDateOfMonth(year, month);
                }
            }
            var postData = JSON.stringify(param);

            var option = {
                type: "POST",
                data: postData,
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: Site.resolveClientUrl(CashOutWinLossOption.urlData +
                    "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId + "&fdate=" +
                    fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                success: function (response) {
                    CashOutWinLoss.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && Number(param.CurrencyId) === 0) {
                            CashOutWinLoss.setReportTitle(currencyId, fdate, tdate);
                        } else {
                            CashOutWinLoss.setReportTitle(param.CurrencyId, fdate, tdate);
                        }
                        //update excel ULR
                        CashOutWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate);
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
                CashOutWinLoss.renderBreadCrumb(level);
                CashOutWinLoss.bindData(0, cacheData[level]);
                //reset excel Link
                CashOutWinLoss.setExcelUrl(0, 0, 0, "", "");
                CashOutWinLoss.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
            }
            else if (level === 1) {
                CashOutWinLoss.bindData(1, cacheData[level]);
                CashOutWinLossOption.urlExcelData = CashOutWinLossOption.urlExcelData.replace("level=2", "level=1");
                CashOutWinLoss.renderBreadCrumb(level);
            }
        },

        //#BindReport
        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = CashOutWinLossOption.getTotalColModel(level);
            var colspan = CashOutWinLossOption.totalColspan[level];
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
            totalRowCount = numOfTotalRow;
            
            var gridOption = CashOutWinLossOption.getGridOption(level);
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
                    CashOutWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    jqGridAdditional.resizeGrid();
                }

                CashOutWinLossOption.setGroupHeaders(reportGrid);

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
        },
        openBetlistPopUp: function (dataDrillDown) {
            dataDrillDown = $.parseJSON(dataDrillDown);
            var dateRange = Report.GetDrilldownDateRange(_submitData.ReportBy, _submitData.FromDate, _submitData.ToDate, dataDrillDown.DrillDownDate);
            
            var url = Site.resolveClientUrl("CustomerWinLossBetList/Index");
            url = QueryString.setParam(url, "fromdate", dateRange.FromDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "todate", dateRange.ToDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "excludetest", _submitData.IsExcludedTestData);
            url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
            url = QueryString.setParam(url, "CurrencyName", dataDrillDown.DrillDownCurrencyName);
            url = QueryString.setParam(url, "drawfilter", _submitData.DrawFilter);
            url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
            url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
            url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
            url = QueryString.setParam(url, "BaseCurrency", _submitData.BaseCurrency);
            url = QueryString.setParam(url, "excelfilename", "CashOutWinLossBetList");
            url = Site.setProductCodeListParam(url, "CO");

            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var dialogForm = '<div title="Customer Bet List"><iframe scrolling="no" frameborder="0" width="' +
                             popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        }
        //#end
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CashOutWinLoss.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    jqGridAdditional.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});