"use strict";
define(["jquery", "common/querystring", "common/site", "./agcasinowinloss.option", "../report", "lib/jqgrid/jquery.jqgrid.additional",
        "components/pluginhelper/daterangepicker", "moment", "helpers/daterange",
        "extensions/jquery.extension", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, QueryString, Site, AgCasinoWinLossOption, Report, jqGridAdditional, DatePicker, moment, DateHelper) {
        var cacheData = {};
        var ajaxRequest = null;
        var totalRowCount = 0;
        var _level = 0;
        var VALID_MONTH_RANGE = 3;
        var _submitData;
        var _drilldownData;

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

                $(".main-content").on("click", ".drilldown", function () {
                    var data = JSON.parse($(this).attr('data-drilldown'));
                    _me.drillDown(data);
                });

                $(window).on("resize", function () {
                    _me.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },
            resizeGrid: function(){  
                var reportGrid = $("#ReportGrid");
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(reportGrid);
            },
            //#region Layout
            setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
                var temp = 'api/AgCasinoWinLossReport/Index';
                if (level > 0) {
                    temp += "?level=" + level + "&currencyId="
                    + currencyId + "&custId=" + custId + "&fdate=" + moment(fdate).format("MM/DD/YYYY")
                     + "&tdate=" + moment(tdate).format("MM/DD/YYYY");
                }

                AgCasinoWinLossOption.urlExcelData = temp;
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
                    if (level === 0) {
                        _drilldownData = {
                            IsExcludedTestData: $("#IsExcludedTestData").is(":checked"),
                            CurrencyId: $("#CurrencyId").val(),
                            FromDate: _submitData.FromDate,
                            ToDate: _submitData.ToDate,
                            CurrencyName: $("#CurrencyId option:selected").text(),
                            ReportBy: reportBy,
                            BaseCurrency: $("#BaseCurrency").val()
                        };

                        _me.setReportTitle($("#CurrencyId").val(), _submitData.FromDate, _submitData.ToDate);
                    }

                    Report.renderBreadCrumb(level);
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

            drillDown: function (data) {

                data.level = Number(data.level);

                if (_level !== data.level) {
                    _level = data.level;
                }

                var param =
                {
                    IsExcludedTestData: _drilldownData.IsExcludedTestData,
                    CurrencyId: _drilldownData.CurrencyId,
                    FromDate: _drilldownData.FromDate,
                    ToDate: _drilldownData.ToDate,
                    ReportBy: _drilldownData.ReportBy,
                    BaseCurrency: _drilldownData.BaseCurrency
                };

                var fdate = new Date(Date.parse(data.datetime));
                var tdate = new Date(Date.parse(data.datetime));
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
                
                var option = {
                    type: "POST",
                    data: JSON.stringify(param),
                    dataType: "json",
                    contentType: "application/json",
                    url: Site.resolveClientUrl(AgCasinoWinLossOption.urlData +
                        "?level=" + data.level + "&currencyId=" + data.currencyId + "&custId=" + data.custId + "&fdate=" +
                        moment(fdate).format("MM/DD/YYYY") + "&tdate=" + moment(tdate).format("MM/DD/YYYY")),
                    success: function (response) {
                        _me.ajaxComplete(data.level, response);
                        if (response != null && response.Data.length > 0) {
                            if (data.level > 0 && Number(param.CurrencyId) === 0) {
                                _me.setReportTitle(data.currencyId, fdate, tdate);
                            } else {
                                _me.setReportTitle(param.CurrencyId, fdate, tdate);
                            }
                        }

                        _me.setExcelUrl(data.level, data.currencyId, data.custId, fdate, tdate);
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
                    Report.renderBreadCrumb(level);
                    _me.bindData(0, cacheData[level]);
                    //reset excel Link
                    _me.setExcelUrl(0, 0, 0, "", "");
                    //re-set title
                    var fromdate = new Date(_drilldownData.FromDate);
                    var todate = new Date(_drilldownData.ToDate);
                    var currencyId = _drilldownData.CurrencyId;
                    _me.setReportTitle(currencyId, fromdate, todate);
                }
                else if (level === 1) {
                    _me.bindData(1, cacheData[level]);
                    AgCasinoWinLossOption.urlExcelData = AgCasinoWinLossOption.urlExcelData.replace("level=2", "level=1");
                    Report.renderBreadCrumb(level);
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
                jqGridAdditional.clearGrid("ReportGrid");
                var numOfRow = response.Data.length;
                if (numOfRow === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                level = Number(level);
                var $gridBody = $(".ui-jqgrid-bdiv");

                var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;

                jqGridAdditional.clearGrid("ReportGrid");

                var reportGrid = $("#ReportGrid");

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

                        _me.resizeGrid();
                    }

                    Site.unBlockUI();
                };

                // Show the grid
                reportGrid.jqGrid(gridOption);

                reportGrid.jqGrid('showCol', 'rn');
                $gridBody = $(".ui-jqgrid-bdiv");
                if ($gridBody.length > 0) {
                    $gridBody[0].scrollLeft = scrollLeft;
                }
            }
            //#end
        };

        return _me;
    });
