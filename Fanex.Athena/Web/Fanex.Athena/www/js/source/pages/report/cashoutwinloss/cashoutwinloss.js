"use strict";
define(["jquery", "common/querystring", "common/site", "./cashoutwinloss.option", "../report",
        "lib/jqgrid/jquery.jqgrid.additional", "components/pluginhelper/daterangepicker", "moment", "common/popup",
        "extensions/jquery.extension", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, QueryString, Site, CashOutWinLossOption, Report, jqGridAdditional, DatePicker, moment, Popup) {
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
                    resourceName: "CashOutWL",
                    title: Site.getSingleResource("CashOutWinLoss")
                });
                Site.setConstraintMonthRange(VALID_MONTH_RANGE, CashOutWinLossOption.urlExcelData);
                _me.initEvent();
                _me.initSetting();
                _me.registerGlobalFunction();
            },

            initSetting: function () {
                CashOutWinLossOption.isLicenseeOfferBA = $("#IsLicenseeOfferBA").val() === "1";
            },

            registerGlobalFunction: function () {
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
                    if (level < 2) {
                        _me.goBack(level);
                    }
                });

                $("body").on("click", ".bet-list-link", function () {
                    var dataDrillDown = $(this).attr("data-drill-down");
                    _me.openBetlistPopUp(dataDrillDown);
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

                    Report.renderBreadCrumb(level);
                    _me.bindData(level, response);
                }
            },

            setReportTitle: function (currencyId, fromdate, todate) {
                var titlePattern = Site.getSingleResource('CashOutWinLoss') + ', {0} → {1} - {2}';
                var currencyName = Report.getCurrencyNameForReportTitle(currencyId);
                var title = $.format(titlePattern,
                    moment(fromdate, "MM/DD/YYYY").format("MM/DD/YYYY"),
                    moment(todate, "MM/DD/YYYY").format("MM/DD/YYYY"),
                    currencyName);
                $(".report-title").html(title);
                $(".panel-heading").removeClass("hide");
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
                Report.renderBreadCrumb(level);
                _me.bindData(level, cacheData[level]);

                if (level === 0) {
                    _me.setExcelUrl(0, 0, 0, "", "");
                    _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }
                else if (level === 1) {
                    CashOutWinLossOption.urlExcelData = CashOutWinLossOption.urlExcelData.replace("level=2", "level=1");
                }
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
                    BaseCurrency: $("#BaseCurrency").attr("data-original")
                };

                var drilldownDate = new Date(datetime);
                var dateRange = Report.GetDrilldownDateRange(param.ReportBy, param.FromDate, param.ToDate, drilldownDate);

                var postData = JSON.stringify(param);
                var url = Site.resolveClientUrl(CashOutWinLossOption.urlData) +
                        "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                        "&fdate=" + moment(dateRange.FromDate).format("MM/DD/YYYY") + "&tdate=" + moment(dateRange.ToDate).format("MM/DD/YYYY");

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
                                _me.setReportTitle(currencyId, dateRange.FromDate, dateRange.ToDate);
                            } else {
                                _me.setReportTitle(param.CurrencyId, dateRange.FromDate, dateRange.ToDate);
                            }

                            _me.setExcelUrl(level, currencyId, customerId, dateRange.FromDate, dateRange.ToDate);
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
            openBetlistPopUp: function (dataDrillDown) {
                dataDrillDown = $.parseJSON(dataDrillDown);
                var dateRange = Report.GetDrilldownDateRange(_submitData.ReportBy, _submitData.FromDate, _submitData.ToDate, dataDrillDown.DrillDownDate);
                var url = Site.resolveClientUrl("CustomerWinLossBetList/Index");
                url = QueryString.setParam(url, "fromdate", moment(dateRange.FromDate).format("MM/DD/YYYY"));
                url = QueryString.setParam(url, "todate", moment(dateRange.ToDate).format("MM/DD/YYYY"));
                url = QueryString.setParam(url, "excludetest", _submitData.IsExcludedTestData);
                url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
                url = QueryString.setParam(url, "CurrencyName", dataDrillDown.DrillDownCurrencyName);
                url = QueryString.setParam(url, "drawfilter", _submitData.DrawFilter);
                url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
                url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
                url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
                url = QueryString.setParam(url, "BaseCurrency", _submitData.BaseCurrency);
                url = QueryString.setParam(url, "productoptions[0].code", "CO");
                url = QueryString.setParam(url, "productoptions[0].status", "true");
                url = QueryString.setParam(url, "excelfilename", "CashOutWinLossBetList");

                var popH = Math.min($(window).height() - 100, 650), popW = 920;
                var popupOption = { id: "betlist-popup", width: popW + 50, height: popH + 50, title: "Customer Bet List" };
                Popup.openPopupWithIframe(url, popupOption);
            },
            lastDateOfMonth: function (year, month) {
                return moment([year, month + 1]).endOf('month');
            }
        };

        return _me;
    });