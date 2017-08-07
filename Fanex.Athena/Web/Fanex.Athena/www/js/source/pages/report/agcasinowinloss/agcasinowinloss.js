"use strict";
define(["jquery", "common/querystring", "common/site", "./agcasinowinloss.option", "../report", "lib/jqgrid/jquery.jqgrid.additional",
        "components/pluginhelper/daterangepicker", "moment", "common/popup", "helpers/daterange",
        "extensions/jquery.extension", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, QueryString, Site, AgCasinoWinLossOption, Report, jqGridAdditional, DatePicker, moment, Popup, DateHelper) {
        "use strict";
        var cacheData = {};
        var ajaxRequest = null;
        var totalRowCount = 0;
        var _level = 0;
        var VALID_MONTH_RANGE = 3;
        var _submitData;

        var _me = {
            getTotalRowCount: function () {
                return totalRowCount;
            },
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                _me.initEvent();
                Site.addUserGuide({
                    resourceName: "AGCasinoWL",
                    title: Site.getSingleResource("AGCasinoWinLoss")
                });
                DateHelper.setConstraintMonthRange(VALID_MONTH_RANGE, AgCasinoWinLossOption.urlExcelData);
            },
            initEvent: function () {
                $('#SubmitButton').click(function () {
                    // Site.blockUI();
                    $('#IsExportExcel').val(false);
                    _level = 0;
                    _me.submit();
                });

                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    var mainForm = $("#MainForm");
                    var url = Site.resolveClientUrl(AgCasinoWinLossOption.urlExcelData);
                    mainForm.attr("action", url);
                    mainForm.submit();
                    $('#IsExportExcel').val(false);
                    return false;
                });

                $("#level-link-container").on("click", ".level-link", function () {
                    var level = Number($(this).attr("data-level"));
                    _level = level;
                    return _me.goBack(level);
                });

                $(window).on("resize", function () {
                    "use strict";
                    var reportGrid = $("#ReportGrid");
                    jqGridAdditional.resetLayout(reportGrid);
                    jqGridAdditional.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            //#region Layout
            setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
                var temp = 'api/AgCasinoWinLossReport/Index';
                if (level > 0) {
                    temp += "?level=" + level + "&currencyId="
                    + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                     + "&tdate=" + tdate.toString("MM/dd/yyyy");
                }

                AgCasinoWinLossOption.urlExcelData = temp;
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
                var titlePattern = Site.getSingleResource('AGCasinoWinLoss') + ', {0} → {1} - {2}';
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
                    var reportBy = $("input[name='reportby']:checked").val();
                    cacheData[level] = response;
                    //reset layout
                    if (level === 0) {
                        //update drildownValue
                        $("#OriginalIsExcludedTestData").val($("#IsExcludedTestData").is(":checked"));
                        $("#OriginalCurrencyId").val($("#CurrencyId").val());
                        $("#OriginalFromDate").val(_submitData.FromDate);
                        $("#OriginalToDate").val(_submitData.ToDate);
                        $("#OriginalCurrencyName").val($("#CurrencyId option:selected").text());
                        $("#BaseCurrency").attr("data-original", $("#BaseCurrency").val());
                        $("#OriginalReportBy").val(reportBy);
                        _me.setReportTitle($("#CurrencyId").val(), _submitData.FromDate, _submitData.ToDate);
                    }

                    _me.renderBreadCrumb(level);
                    _me.bindData(level, response);
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
                    url: Site.resolveClientUrl(AgCasinoWinLossOption.urlData),
                    success: function (response) {
                        _me.setExcelUrl(0, 0, 0, "", "");
                        _me.ajaxComplete(0, response);
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
                    IsExcludedTestData: $("#OriginalIsExcludedTestData").val(),
                    CurrencyId: $("#OriginalCurrencyId").val(),
                    FromDate: $("#OriginalFromDate").val(),
                    ToDate: $("#OriginalToDate").val(),
                    ReportBy: $("#OriginalReportBy").val(),
                    BaseCurrency: $("#BaseCurrency").attr("data-original")
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
                        tdate = _me.lastDateOfMonth(year, month);
                    }
                }
                var postData = JSON.stringify(param);

                var option = {
                    type: "POST",
                    data: postData,
                    dataType: "json",
                    contentType: "application/json",
                    cache: false,
                    url: Site.resolveClientUrl(AgCasinoWinLossOption.urlData +
                        "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId + "&fdate=" +
                        fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                    success: function (response) {
                        _me.ajaxComplete(level, response);
                        if (response != null && response.Data.length > 0) {
                            if (level > 0 && Number(param.CurrencyId) === 0) {
                                _me.setReportTitle(currencyId, fdate, tdate);
                            } else {
                                _me.setReportTitle(param.CurrencyId, fdate, tdate);
                            }
                            //update excel ULR
                            _me.setExcelUrl(level, currencyId, customerId, fdate, tdate);
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
                    _me.renderBreadCrumb(level);
                    _me.bindData(0, cacheData[level]);
                    //reset excel Link
                    _me.setExcelUrl(0, 0, 0, "", "");
                    //re-set title
                    var fromdate = new Date($("#OriginalFromDate").val());
                    var todate = new Date($("#OriginalToDate").val());
                    var currencyId = $("#OriginalCurrencyId").val();
                    _me.setReportTitle(currencyId, fromdate, todate);
                }
                else if (level === 1) {
                    _me.bindData(1, cacheData[level]);
                    AgCasinoWinLossOption.urlExcelData = AgCasinoWinLossOption.urlExcelData.replace("level=2", "level=1");
                    _me.renderBreadCrumb(level);
                }
            },

            //#BindReport
            bindTotal: function (level, totalData, summaryTotal) {
                var columnModel = AgCasinoWinLossOption.getTotalColModel(level);
                var colspan = AgCasinoWinLossOption.totalColspan[level];
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

                var gridOption = AgCasinoWinLossOption.getGridOption(level);
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
                        _me.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());

                        jqGridAdditional.resizeGrid();
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

        return _me;
    });
