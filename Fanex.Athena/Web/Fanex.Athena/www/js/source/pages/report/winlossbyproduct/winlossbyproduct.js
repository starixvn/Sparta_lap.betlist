define(["jquery", "common/site", "common/querystring", "lib/jqgrid/jquery.jqgrid.additional", "./winlossbyproduct.option", "moment", "common/popup",
    "components/pluginhelper/daterangepicker", "../report", "components/pluginhelper/multiselect", "components/pluginhelper/autocomplete",
    "extensions/jquery.extension", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, QueryString, jqGridAdditional, WinLossByProductOption, moment, Popup, DatePicker, Report, MultiSelect, AutoComplete) {
        "use strict";

        var VALID_MONTH_RANGE = 3;
        var _cacheData = [];
        var ajaxRequest = null;
        var _memberId, _systemId; // store data when submit level 0
        var _submitData = {}; // cache submit data of level 0.

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                MultiSelect.initialize('grid-products-multiselect', $("#ProductOptionJson").val(), 'Code', 'Name', 'Status');
                Site.setConstraintMonthRange(VALID_MONTH_RANGE, WinLossByProductOption.urlExcelData);
                Site.addUserGuide({
                    resourceName: "WinLossByProduct",
                    title: Site.getSingleResource("WinLossByProduct")
                });
                _me.initEvent();
                _me.initSetting();
                _me.registerGlobalFunction();

                _me.initAutoComplete();
            },

            initSetting: function () {
                WinLossByProductOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
                WinLossByProductOption.IsAllowBA = Site.asBool($("#IsAllowedBACommission").val());
                WinLossByProductOption.IsAllowRB = Site.asBool($("#IsAllowedRacing").val());
                WinLossByProductOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
            },

            initAutoComplete: function () {
                var option = {
                    url: Site.resolveClientUrl("CustomerAPI/SearchCustomer"),
                    target: "#Username"
                };

                AutoComplete.initialize(option);
            },

            registerGlobalFunction: function () {
                window.drillDown = _me.drillDown;
            },

            initEvent: function () {
                $("body").on("click", ".bet-list-link", function () {
                    var dataDrillDown = $(this).attr("data-drill-down");
                    _me.openBetlistPopUp(dataDrillDown);
                });

                $('#SubmitButton').click(function () {
                    $('#IsExportExcel').val(false);
                    _me.submit();
                });

                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    var mainForm = $("#winlossbyproduct");
                    var url = Site.resolveClientUrl(WinLossByProductOption.urlExcelData);
                    mainForm.attr("action", url);
                    mainForm.submit();
                    return false;
                });

                $("#level-link-container").on("click", ".level-link", function () {
                    var level = Number($(this).attr("data-level"));
                    if (level < 2) {
                        _me.goBack(level);
                    }
                });

                $(".main-content").on("click", ".drilldown", function () {
                    var param = $(this).data("drill-down");
                    _me.drillDown(param.level, param.currencyId, param.custId, param.datetime, param.currencyName);
                });

                $(window).on("resize", function () {
                    _me.resizeGrid();
                    _me.reUpdateGridWidth();
                    AutoComplete.updateWidthOfSuggestionDropdown();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            submit: function () {
                Report.setOriginalProduct();

                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return false;
                }

                Site.updateOriginalLicenseeOption();
                var data = $("#winlossbyproduct").serializeObject();
                Report.CleanProductOptions(data, Number($("#ProductListCount").val()));
                if (data.Username === "") {
                    data.Username = null;
                }

                var option = {
                    type: "POST",
                    data: data,
                    dataType: "json",
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    cache: false,
                    url: Site.resolveClientUrl(WinLossByProductOption.urlData),
                    success: function (response) {
                        _submitData = {
                            ProductOptions: response.ProductOptions,
                            DrawFilter: data.DrawFilter,
                            CurrencyId: data.CurrencyId,
                            ReportBy: Number(data.ReportBy),
                            FromDate: new Date(data.FromDate),
                            ToDate: new Date(data.ToDate),
                            CurrencyName: data.CurrencyName,
                            IsExcludedTestData: data.IsExcludedTestData
                        };

                        _systemId = response.SystemId;
                        _memberId = response.MemberId;
                        _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                        _me.setExcelUrl(0, 0, 0, "", "", "");
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

            setExcelUrl: function (level, currencyId, custId, fdate, tdate, currencyName) {
                var temp = "api/WinLossByProductReport/Index";
                if (level > 0) {
                    temp += "?level=" + level + "&currencyId=" + currencyId + "&custId=" + custId + "&fdate=" +
                    moment(fdate).format("MM/DD/YYYY") + "&tdate=" + moment(tdate).format("MM/DD/YYYY") +
                    "&memberid=" + _memberId + "&systemid=" + _systemId + "&currencyname=" + currencyName;
                }
                WinLossByProductOption.urlExcelData = temp;
            },

            setReportTitle: function (currencyId, fromdate, todate, currencyName) {
                var titlePattern = Site.getSingleResource('WinLossByProduct') + ', {0} → {1} - {2}{3}';
                var currencyNameOnTitle = currencyName ? Site.getSingleResource("Viewin").replace("{0}", currencyName)
                                                        : Report.getCurrencyNameForReportTitle(currencyId);
                var licenseeWLNote = jqGridAdditional.getLicenseeWLNote(_submitData.ProductOptions, _systemId);
                licenseeWLNote = licenseeWLNote === '' ? '' : ', ' + licenseeWLNote;

                var title = $.format(titlePattern,
                    moment(fromdate, "MM/DD/YYYY").format("MM/DD/YYYY"), moment(todate, "MM/DD/YYYY").format("MM/DD/YYYY"),
                    currencyNameOnTitle, licenseeWLNote);

                $(".report-title").html(title);
                $(".panel-heading").removeClass("hide");
            },

            ajaxComplete: function (level, response) {
                if (response !== null) {
                    _cacheData[level] = response;
                    Report.renderBreadCrumb(level, WinLossByProductOption.licenseeId);
                    _me.bindData(level, _submitData.ReportBy, response);
                }
            },

            bindData: function (level, reportBy, response) {
                jqGridAdditional.clearGrid("ReportGrid");
                var numOfRow = response.Data.length;
                if (numOfRow === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                level = Number(level);
                var reportGrid = $("#ReportGrid");
                var onPaging = false;

                var gridOption = WinLossByProductOption.getGridOption(level, reportBy);
                gridOption.data = response.Data;
                gridOption.rowNum = level === 2 ? WinLossByProductOption.pageSize : WinLossByProductOption.nonePagingSize;
                gridOption.rowTotal = numOfRow;
                gridOption.onPaging = function (pgButton) {
                    if (level === 2 && numOfRow > WinLossByProductOption.pageSize) {
                        if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                            Site.blockUI();
                        }
                        onPaging = true;
                    }
                    else {
                        return 'stop';
                    }
                };
                gridOption.gridComplete = function () {
                    jqGridAdditional.gridComplete();
                    _me.rowSpanByLevel(reportGrid, level);

                    // Update colspan for grouping row (because fixed table should has correct colspan value)
                    var coulumnVisibleCount = gridOption.colModel.filter(function (item) { return !item.hidden; }).length;
                    reportGrid.find("tr.jqgroup td").attr("colspan", coulumnVisibleCount - 1); // -1 for hiding of Date column

                    if (level === 2 && numOfRow > WinLossByProductOption.pageSize) {
                        $("#pg_ppdata").show();
                    }
                    else {
                        $("#pg_ppdata").hide();
                    }

                    var $footerTable = $("table.ui-jqgrid-ftable");
                    if (onPaging && level === 2) {
                        $footerTable.html($("#footer").html());
                    }
                    else {
                        _me.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($footerTable.html());
                    }

                    onPaging = false;

                    if (WinLossByProductOption.hasCustomerComm) {
                        jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
                    }

                    //set rowspan for footer row
                    $footerTable.tableRowSpan([WinLossByProductOption.currencyIndex]);

                    _me.resizeGrid();
                    _me.reUpdateGridWidth();
                    Site.unBlockUI();
                };

                reportGrid.jqGrid(gridOption);
            },

            reUpdateGridWidth: function () {
                var $gridContainer = $(".theme_jqGrid");
                var $gridFooter = $gridContainer.find(".ui-jqgrid-sdiv");
                var $gridBody = $gridContainer.find(".ui-jqgrid-bdiv");

                var shouldReUpdateGridWidth = $gridBody.hasHorizontalScrollBar() && $gridFooter.hasVerticalScrollBar();
                if (shouldReUpdateGridWidth) {
                    var tableWidth = $gridBody.width() - 20;
                    $gridContainer.find(".ui-jqgrid-hdiv").css("width", tableWidth + "px");
                    $gridBody.css("width", tableWidth + "px");
                }
            },

            rowSpanByLevel: function ($grid, level) {
                switch (level) {
                    case 0:
                        $grid.tableRowSpan([0, 4]);
                        break;
                    case 1:
                        $grid.tableRowSpan([0, WinLossByProductOption.systemIdIndex], WinLossByProductOption.currencyIndex);
                        break;
                    case 2:
                        $grid.tableRowSpan([0, WinLossByProductOption.externalIdIndex, WinLossByProductOption.systemIdIndex],
                                                     WinLossByProductOption.currencyIndex);
                        break;
                }
            },

            bindTotal: function (level, totalData, summaryTotal) {
                var productOptions = $.parseJSON(_submitData.ProductOptions);
                var columnModel = WinLossByProductOption.getTotalColModel(level, productOptions);
                var colspan = WinLossByProductOption.totalColspan[level];
                var summaryColspan = colspan + 1;
                jqGridAdditional.assignTotal(totalData, columnModel);
                var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);

                var allowScrolling = true;
                jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, allowScrolling);
            },

            goBack: function (level) {
                var reportBy = _submitData.ReportBy;
                Report.renderBreadCrumb(level, WinLossByProductOption.licenseeId);
                _me.bindData(level, reportBy, _cacheData[level]);

                if (level === 0) {
                    _me.setExcelUrl(0, 0, 0, "", "", "");
                    _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }
                else {
                    WinLossByProductOption.urlExcelData = WinLossByProductOption.urlExcelData.replace("level=2", "level=1");
                }
            },

            openBetlistPopUp: function (dataDrillDown) {
                dataDrillDown = $.parseJSON(dataDrillDown);
                dataDrillDown.DrawFilter = _submitData.DrawFilter;
                dataDrillDown.IsExcludedTestData = _submitData.IsExcludedTestData;
                dataDrillDown.CurrencyId = _submitData.CurrencyId;
                dataDrillDown.CurrencyName = _submitData.CurrencyName;
                if (typeof dataDrillDown.DrillDownFromDate === 'string') {
                    dataDrillDown.DrillDownFromDate = new Date(dataDrillDown.DrillDownFromDate);
                    dataDrillDown.DrillDownToDate = new Date(dataDrillDown.DrillDownToDate);
                }

                var dateRange = Report.GetDrilldownDateRange(_submitData.ReportBy, _submitData.FromDate, _submitData.ToDate, dataDrillDown.DrillDownFromDate);

                var url = Site.resolveClientUrl("LicenseeWinLossBetList/Index");
                url = Site.setProductOptionOriginalParamList(url);
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "fromdate", moment(dateRange.FromDate).format("MM/DD/YYYY"));
                url = QueryString.setParam(url, "todate", moment(dateRange.ToDate).format("MM/DD/YYYY"));
                url = QueryString.setParam(url, "excludetest", dataDrillDown.IsExcludedTestData);
                url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
                url = QueryString.setParam(url, "CurrencyName", dataDrillDown.DrillDownCurrencyName);
                url = QueryString.setParam(url, "drawfilter", dataDrillDown.DrawFilter);
                url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
                url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
                url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
                url = QueryString.setParam(url, "OriginalCurrenceId", dataDrillDown.CurrencyId);
                url = QueryString.setParam(url, "OriginalCurrencyName", dataDrillDown.CurrencyName);

                var popH = Math.min($(window).height() - 100, 500), popW = 920;
                var popupOption = { id: "betlist-popup", width: popW + 30, height: popH + 50, title: "Customer Bet List" };
                Popup.openPopupWithIframe(url, popupOption);
            },

            resizeGrid: function () {
                var reportGrid = $("#ReportGrid");
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(reportGrid);
            },

            drillDown: function (level, currencyId, customerId, datetime, currencyName) {
                var param = {
                    MemberId: _memberId,
                    ProductOptions: $.parseJSON(_submitData.ProductOptions),
                    DrawFilter: _submitData.DrawFilter,
                    IsExcludedTestData: _submitData.IsExcludedTestData,
                    CurrencyId: _submitData.CurrencyId,
                    ReportBy: _submitData.ReportBy,
                    FromDate: _submitData.FromDate,
                    ToDate: _submitData.ToDate,
                    BaseCurrency: $("#BaseCurrency").attr("data-original")
                };
                var drilldownDate = new Date(datetime);
                var dateRange = Report.GetDrilldownDateRange(param.ReportBy, param.FromDate, param.ToDate, drilldownDate);

                var url = Site.resolveClientUrl(WinLossByProductOption.urlData) + "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                         "&fdate=" + moment(dateRange.FromDate).format("MM/DD/YYYY") + "&tdate=" + moment(dateRange.ToDate).format("MM/DD/YYYY");
                var option = {
                    type: "POST",
                    data: JSON.stringify(param),
                    dataType: "json",
                    contentType: "application/json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.ajaxComplete(level, response);
                        if (response !== null && response.Data.length > 0) {
                            if (level > 0 && param.CurrencyId === '0') {
                                _me.setReportTitle(currencyId, dateRange.FromDate, dateRange.ToDate, currencyName);
                            } else {
                                _me.setReportTitle(param.CurrencyId, dateRange.FromDate, dateRange.ToDate, currencyName);
                            }

                            _me.setExcelUrl(level, currencyId, customerId, dateRange.FromDate, dateRange.ToDate, currencyName);
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
            }
        };

        return _me;
    });