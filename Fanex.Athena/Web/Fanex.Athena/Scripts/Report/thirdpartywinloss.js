/*global jqGridAdditional, ThirdPartyWinLossOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var ThirdPartyWinLoss = (function ($) {
    "use strict";
    var cacheData = {};
    var ajaxRequest = null;
    var _level = 0;
    var _submitData;

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
            ThirdPartyWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
            ThirdPartyWinLoss.initEvent();
            initMonthPicker();
            Site.addUserGuide({
                resourceName: "AGCasinoWL",
                title: $("#ReportTitle").val()
            });
        },
        initEvent: function () {
            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _level = 0;
                ThirdPartyWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#ThirdPartyWinLossForm");
                var url = Site.resolveClientUrl(ThirdPartyWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                _level = level;
                return ThirdPartyWinLoss.goBack(level);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/report/ThirdPartyWinLoss';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }

            ThirdPartyWinLossOption.urlExcelData = temp;
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
                $(".level1").showHide(ThirdPartyWinLossOption.licenseeId === ThirdPartyWinLossOption.LICENSEE_12BET);
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
                        ProductId: $("#ProductId").val(),
                        ReportBy: reportBy
                    };
                    ThirdPartyWinLoss.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }

                ThirdPartyWinLoss.renderBreadCrumb(level);
                ThirdPartyWinLoss.bindData(level, response);
            }
        },

        submit: function () {
            var mainForm = $("#ThirdPartyWinLossForm");
            var postdata = mainForm.serializeObject();
            var fromDate = new Date(postdata.Year, postdata.Month - 1, postdata.FromDay);
            var toDate = new Date(postdata.Year, postdata.Month - 1, postdata.ToDay);

            postdata.FromDate = fromDate.toDateString();
            postdata.ToDate = toDate.toDateString();

            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(ThirdPartyWinLossOption.urlData),
                success: function (response) {
                    ThirdPartyWinLoss.setExcelUrl(0, 0, 0, "", "");
                    ThirdPartyWinLoss.ajaxComplete(0, response);
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
            var fdate = new Date(Date.parse(datetime)).toString("MM/dd/yyyy");
            var tdate = new Date(Date.parse(datetime)).toString("MM/dd/yyyy");
            if (Number(param.ReportBy) === 2) {
                fdate = _submitData.FromDate;
                tdate = _submitData.ToDate;
            }
            var option = {
                type: "POST",
                data: JSON.stringify(param),
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: Site.resolveClientUrl(ThirdPartyWinLossOption.urlData +
                    "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                    "&fdate=" + fdate + "&tdate=" + tdate),
                success: function (response) {
                    ThirdPartyWinLoss.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && Number(param.CurrencyId) === 0) {
                            ThirdPartyWinLoss.setReportTitle(currencyId, fdate, tdate);
                        } else {
                            ThirdPartyWinLoss.setReportTitle(param.CurrencyId, fdate, tdate);
                        }
                        //update excel ULR
                        ThirdPartyWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate);
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
                ThirdPartyWinLoss.renderBreadCrumb(level);
                ThirdPartyWinLoss.bindData(0, cacheData[level]);
                //reset excel Link
                ThirdPartyWinLoss.setExcelUrl(0, 0, 0, "", "");
                //re-set title
                var fromdate = new Date(_submitData.FromDate);
                var todate = new Date(_submitData.ToDate);
                var currencyId = _submitData.CurrencyId;
                ThirdPartyWinLoss.setReportTitle(currencyId, fromdate, todate);
            }
            else if (level === 1) {
                ThirdPartyWinLoss.bindData(1, cacheData[level]);
                ThirdPartyWinLossOption.urlExcelData = ThirdPartyWinLossOption.urlExcelData.replace("level=2", "level=1");
                ThirdPartyWinLoss.renderBreadCrumb(level);
            }
        },

        //#BindReport
        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = ThirdPartyWinLossOption.getTotalColModel(level);
            var colspan = ThirdPartyWinLossOption.totalColspan[level];
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

            var gridOption = ThirdPartyWinLossOption.getGridOption(level);
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
                    ThirdPartyWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    ThirdPartyWinLoss.resizeGrid();
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
    ThirdPartyWinLoss.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    ThirdPartyWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});
