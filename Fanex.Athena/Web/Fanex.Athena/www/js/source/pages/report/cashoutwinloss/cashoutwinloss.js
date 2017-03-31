"use strict";
define(["jquery", "common/querystring", "common/site", "./cashoutwinloss.option", "../report",
        "lib/jqgrid/jquery.jqgrid.additional", "components/pluginhelper/daterangepicker", "moment",
        "extensions/jquery.extension", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, QueryString, Site, CashOutWinLossOption, Report, jqGridAdditional, DatePicker, moment) {
        var cacheData = {};
        var ajaxRequest = null;
        var _level = 0;
        var VALID_MONTH_RANGE = 3;
        var _submitData;

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                Site.addUserGuide({
                    resourceName: "AGCasinoWL",
                    title: Site.getSingleResource("AGCasinoWinLoss")
                });
                Site.setConstraintMonthRange(VALID_MONTH_RANGE, CashOutWinLossOption.urlExcelData);
                _me.initEvent();
                _me.initSetting();
                _me.registerGlobalFunction();
            },

            initSetting: function () {
                CashOutWinLossOption.isLicenseeOfferBA = $("#IsLicenseeOfferBA").val() === "1";
                CashOutWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
            },

            registerGlobalFunction: function(){
                window.cashoutDrillDown = _me.drillDown;
            },

            initEvent: function () {
                $('#SubmitButton').click(function () {
                    $('#IsExportExcel').val(false);
                    _level = 0;
                    _me.submit();
                });

                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    var $mainForm = $("#MainForm");
                    var url = Site.resolveClientUrl(CashOutWinLossOption.urlExcelData);
                    $mainForm.attr("action", url);
                    $mainForm.submit();
                    $('#IsExportExcel').val(false);
                    return false;
                });

                $("#level-link-container").on("click", ".level-link", function () {
                    var level = Number($(this).data("level"));
                    _level = level;
                    return _me.goBack(level);
                });

                $(window).on("resize", function () {
                    _me.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            submit: function () {
                var $mainForm = $("#MainForm");
                var postdata = $mainForm.serializeObject();
                _submitData = postdata;
                var option = {
                    type: "POST",
                    data: postdata,
                    dataType: "json",
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    cache: false,
                    url: Site.resolveClientUrl(CashOutWinLossOption.urlData),
                    success: function (response) {
                        _me.setExcelUrl(0, 0, 0, "", "");
                        _me.ajaxComplete(0, response);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);

                return true;
            },

            setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
                var temp = 'api/CashOutWinLossReport/Index';
                if (level > 0) {
                    temp += "?level=" + level + "&currencyId=" + currencyId + "&custId=" + custId +
                        "&fdate=" + moment(fdate).format("MM/DD/YYYY") + "&tdate=" + moment(tdate).format("MM/DD/YYYY");
                }

                CashOutWinLossOption.urlExcelData = temp;
            },

            ajaxComplete: function (level, response) {
                level = Number(level);
                if (response !== null) {
                    cacheData[level] = response;
                    if (level === 0) {
                        _me.setReportTitle($("#CurrencyId").val(), _submitData.FromDate, _submitData.ToDate);
                    }

                    _me.renderBreadCrumb(level);
                    _me.bindData(level, response);
                }
            },

            setReportTitle: function (currencyId, fromdate, todate) {
                var titlePattern = Site.getSingleResource('CashOutWinLoss') + ', {0} → {1} - {2}';
                var currencyName = _me.getCurrencyName(currencyId);
                var title = $.format(titlePattern, moment(fromdate, "MM/DD/YYYY").format("MM/DD/YYYY"), moment(todate, "MM/DD/YYYY").format("MM/DD/YYYY"), currencyName);
                $(".report-title").html(title);
                $(".panel-heading").removeClass("hide");
            },

            getCurrencyName: function (currencyId) {
                var viewinKey = Site.getSingleResource("Viewin");
                var currencyTextList = {
                    "-3": viewinKey.replace("{0}", "GBP"),
                    "-2": viewinKey.replace("{0}", "USD"),
                    "-1": viewinKey.replace("{0}", "EUR"),
                    "0": Site.getSingleResource('AllCurrencies')
                };

                return currencyTextList[currencyId] === undefined
                    ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + currencyId + '"]').text())
                    : currencyTextList[currencyId];
            },

            renderBreadCrumb: function (level) {
                level = Number(level);
                if (level === 0) {
                    $(".breadCrumbHolder").hide();
                }
                else {
                    $(".breadCrumbHolder").show();
                    $("#level-link-container .level0").show();

                    var isLevel2 = level === 2;
                    $("#level-link-container .level2").toggle(isLevel2).toggleClass("active", isLevel2);
                    $("#level-link-container .level1").toggle(CashOutWinLossOption.licenseeId === 1).toggleClass("active", !isLevel2);
                }
            },

            bindData: function (level, response) {
                jqGridAdditional.clearGrid("ReportGrid");
                var numOfRow = response.Data.length;
                if (numOfRow === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                level = Number(level);
                var reportGrid = $("#ReportGrid");

                var numOfTotalRow = response.Total.length;
                var gridOption = CashOutWinLossOption.getGridOption(level);
                gridOption.data = response.Data;
                gridOption.rowTotal = numOfRow;

                gridOption.gridComplete = function () {
                    level = Number(level);
                    jqGridAdditional.gridComplete();
                    $("#pg_ppdata").hide();
                    _me.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());

                    CashOutWinLossOption.setGroupHeaders(reportGrid);
                    _me.resizeGrid();
                    Site.unBlockUI();
                };

                reportGrid.jqGrid(gridOption);
            },

            bindTotal: function (level, totalData, summaryTotal) {
                var columnModel = CashOutWinLossOption.getTotalColModel(level);
                var colspan = CashOutWinLossOption.totalColspan[level];
                var summaryColspan = colspan + 1;

                jqGridAdditional.assignTotal(totalData, columnModel);
                var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
                jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
            },

            resizeGrid: function () {
                var reportGrid = $("#ReportGrid");
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(reportGrid);
            },

            goBack: function (level) {
                if (level === 0) {
                    _me.renderBreadCrumb(level);
                    _me.bindData(0, cacheData[level]);
                    _me.setExcelUrl(0, 0, 0, "", "");
                    _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }
                else if (level === 1) {
                    _me.bindData(1, cacheData[level]);
                    CashOutWinLossOption.urlExcelData = CashOutWinLossOption.urlExcelData.replace("level=2", "level=1");
                    _me.renderBreadCrumb(level);
                }
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
                    ReportBy: _submitData.ReportBy,
                    BaseCurrency: $("#BaseCurrency").attr("data-original")
                };
                var momentDate = moment(datetime);
                var fdate = momentDate.clone();
                var tdate = momentDate.clone();

                if (Number(param.ReportBy) === 2) {
                    var fday = 1;
                    var month = fdate.month();
                    var year = fdate.year();

                    var orignalFDate = moment(param.FromDate);
                    if (month === orignalFDate.month()) {
                        fday = orignalFDate.date();
                    }
                    fdate = moment([year, month, fday, 0, 0, 0, 0]);

                    var originalTDate = moment(param.ToDate);
                    if (month === originalTDate.month()) {
                        var tday = originalTDate.date();
                        tdate = moment([year, month, tday, 0, 0, 0, 0]);
                    }
                    else {
                        tdate = _me.lastDateOfMonth(year, month);
                    }
                }

                var postData = JSON.stringify(param);
                var url = Site.resolveClientUrl(CashOutWinLossOption.urlData) +
                        "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                        "&fdate=" + fdate.format("MM/DD/YYYY") + "&tdate=" + tdate.format("MM/DD/YYYY");

                var option = {
                    type: "POST",
                    data: postData,
                    dataType: "json",
                    contentType: "application/json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.ajaxComplete(level, response);
                        if (response !== null && response.Data.length > 0) {
                            if (level > 0 && Number(param.CurrencyId) === 0) {
                                _me.setReportTitle(currencyId, fdate, tdate);
                            } else {
                                _me.setReportTitle(param.CurrencyId, fdate, tdate);
                            }

                            _me.setExcelUrl(level, currencyId, customerId, fdate, tdate);
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);
            },

            lastDateOfMonth: function (year, month) {
                return moment([year, month + 1]).endOf('month');
            }
        };

        return _me;
    });