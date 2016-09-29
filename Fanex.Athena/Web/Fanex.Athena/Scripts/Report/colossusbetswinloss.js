/*global jqGridAdditional, ColossusBetsOption, Report, Site, QueryString */
/// <reference path="/@JSense.js" />
///
var ColossusBets = (function ($) {
    "use strict";
    var cacheData = {};
    var ajaxRequest = null;
    var _level = 0;
    var _submitData;

    function openBetlistPopUp(dataDrillDown) {
        dataDrillDown = $.parseJSON(dataDrillDown);
        dataDrillDown.DrawFilter = _submitData.DrawFilter;
        dataDrillDown.IsExcludedTestData = _submitData.IsExcludedTestData;
        dataDrillDown.CurrencyId = _submitData.CurrencyId;
        dataDrillDown.CurrencyName = _submitData.CurrencyName;
        dataDrillDown.BaseCurrency = $("#BaseCurrency").attr("data-original");

        var dateRange = Report.GetDrilldownDateRange(_submitData.ReportBy, _submitData.FromDate, _submitData.ToDate, dataDrillDown.DrillDownDate);

        var url = Site.resolveClientUrl("LicenseeWinLossBetList/Index");
        url = QueryString.setParam(url, "fromdate", dateRange.FromDate.toString("MM/dd/yyyy"));
        url = QueryString.setParam(url, "todate", dateRange.ToDate.toString("MM/dd/yyyy"));
        url = QueryString.setParam(url, "excludetest", dataDrillDown.IsExcludedTestData);
        url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
        url = QueryString.setParam(url, "CurrencyName", dataDrillDown.DrillDownCurrencyName);
        url = QueryString.setParam(url, "drawfilter", dataDrillDown.DrawFilter);
        url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
        url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
        url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
        url = QueryString.setParam(url, "BaseCurrency", dataDrillDown.BaseCurrency);
        url = QueryString.setParam(url, "OriginalCurrenceId", dataDrillDown.CurrencyId);
        url = QueryString.setParam(url, "OriginalCurrencyName", dataDrillDown.CurrencyName);

        url = Site.setProductOptionOriginalParamList(url);

        var popH = Math.min($(window).height() - 100, 650), popW = 920;
        var dialogForm = '<div title="Customer Bet List"><iframe scrolling="no" frameborder="0" width="' +
                         popW + '" height="100%" src="' + url + '" ></iframe></div>';

        $(dialogForm).dialog({
            modal: true,
            width: popW + 50,
            height: popH + 50
        });
    }

    return {
        initialize: function () {
            ColossusBets.initEvent();
        },
        initEvent: function () {
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _level = 0;
                ColossusBets.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#MainForm");
                var url = Site.resolveClientUrl(ColossusBetsOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                _level = level;
                return ColossusBets.goBack(level);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/ColossusBetsWinLossReport/Index';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }

            ColossusBetsOption.urlExcelData = temp;
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

                $(".level1").show();
                $(".level1").toggleClass("level-link", level !== 1);
            }
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
        ajaxComplete: function (level, response) {
            level = Number(level);
            if (response !== null) {
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
                        ProductId: $("#ProductId").val(),
                        ReportBy: reportBy
                    };
                    ColossusBets.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }

                ColossusBets.renderBreadCrumb(level);
                ColossusBets.bindData(level, response);
                ColossusBetsOption.setGroupHeader($('#ReportGrid'));
            }
        },

        submit: function () {
            var mainForm = $("#MainForm");
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
                url: Site.resolveClientUrl(ColossusBetsOption.urlData),
                success: function (response) {
                    ColossusBets.setExcelUrl(0, 0, 0, "", "");
                    ColossusBets.ajaxComplete(0, response);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            };
            if (ajaxRequest !== null) {
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

            var param =
            {
                IsExcludedTestData: _submitData.IsExcludedTestData,
                CurrencyId: _submitData.CurrencyId,
                FromDate: _submitData.FromDate,
                ToDate: _submitData.ToDate,
                BaseCurrency: _submitData.BaseCurrency,
                ProductId: _submitData.ProductId,
                ReportBy: _submitData.ReportBy
            };

            var drilldownDateRange = Report.GetDrilldownDateRange(param.ReportBy, param.FromDate, param.ToDate, datetime);

            var option = {
                type: "POST",
                data: JSON.stringify(param),
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: Site.resolveClientUrl(ColossusBetsOption.urlData +
                    "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                    "&fdate=" + drilldownDateRange.FromDate.toString("MM/dd/yyyy") + "&tdate=" + drilldownDateRange.ToDate.toString("MM/dd/yyyy")),
                success: function (response) {
                    ColossusBets.ajaxComplete(level, response);
                    if (response !== null && response.Data.length > 0) {
                        if (level > 0 && Number(param.CurrencyId) === 0) {
                            ColossusBets.setReportTitle(currencyId, drilldownDateRange.FromDate, drilldownDateRange.ToDate);
                        } else {
                            ColossusBets.setReportTitle(param.CurrencyId, drilldownDateRange.FromDate, drilldownDateRange.ToDate);
                        }
                    }

                    ColossusBets.setExcelUrl(level, currencyId, customerId, drilldownDateRange.FromDate, drilldownDateRange.ToDate);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            };
            if (ajaxRequest !== null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);
        },

        goBack: function (level) {
            if (level === 0) {
                ColossusBets.renderBreadCrumb(level);
                ColossusBets.bindData(0, cacheData[level]);
                ColossusBets.setExcelUrl(0, 0, 0, "", "");
                var fromdate = new Date(_submitData.FromDate);
                var todate = new Date(_submitData.ToDate);
                var currencyId = _submitData.CurrencyId;
                ColossusBets.setReportTitle(currencyId, fromdate, todate);
                ColossusBetsOption.setGroupHeader($('#ReportGrid'));
            }
        },

        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = ColossusBetsOption.getTotalColModel(level);
            var colspan = ColossusBetsOption.totalColspan[level];
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

            var gridOption = ColossusBetsOption.getGridOption(level);
            gridOption.data = response.Data;
            gridOption.rowTotal = numOfRow;

            gridOption.gridComplete = function () {
                level = Number(level);
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);
                $("#pg_ppdata").hide();
                if (response === null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    ColossusBets.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    jqGridAdditional.resizeGrid();
                }

                Site.unBlockUI();
            };

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
    ColossusBets.initialize();
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