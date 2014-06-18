/*global jqGridAdditional, JSLINQ, LicenseeWinLossOption, QueryString, Report, Site */
/// <reference path="/@JSense.js" />
///
var LicenseeWinLoss = (function ($) {
    "use strict";
    var cacheData = {};
    var ajaxRequest = null;
    var totalRowCount = 0;
    var _sortColumnName = null;
    var _sortOrder = "desc";
    var _level = 0;
    var _currencyId = null;
    var _customerId = null;
    var _datetime = null;
    var _removeSorting = false;
    var _reportBy = null;
    var _pageSize = 0;
    var _resizeTimer = null;
    function sortData(arrayData) {
        var data = _sortOrder === "asc" ?
             JSLINQ(arrayData).OrderBy(function (x) { return x[_sortColumnName]; }).items
             : JSLINQ(arrayData).OrderByDescending(function (x) { return x[_sortColumnName]; }).items;
        for (var i = 0; i < data.length; i++) {
            data[i].Index = i + 1;
        }

        return data;
    }

    return {
        resizeGrid: function () {
            clearTimeout(_resizeTimer);
            _resizeTimer = setTimeout(function () {
                var gridTop = $("#ReportGrid").offset().top;
                var scroll = Site.getScrollBarState();
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv:visible").height()
                            - $(".ui-jqgrid-pager:visible").height() - (scroll.hScrollbar ? 25 : (scroll.vScrollbar ? 25 : 10));
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
            }, 200);
        },
        getTotalRowCount: function () {
            return totalRowCount;
        },
        initialize: function () {
            _pageSize = LicenseeWinLossOption.pageSize;
            LicenseeWinLoss.initEvent();
            LicenseeWinLoss.initSetting();
            LicenseeWinLossOption.gridOption.onSortCol = function (index/*, columnIndex, sortOrder*/) {
                Site.blockUI();
                window.setTimeout(function () {
                    _removeSorting = false;
                    if (_sortColumnName === index) {
                        _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        _sortOrder = "asc";
                        _sortColumnName = index;
                    }

                    cacheData[_level].Data = sortData(cacheData[_level].Data);

                    LicenseeWinLoss.bindData(_level, _reportBy, cacheData[_level]);

                    Site.unBlockUI();

                }, 100);
                return 'stop';
            };

            Site.addFilterCollapseExpand();
        },

        initSetting: function () {
            LicenseeWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            LicenseeWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },

        initEvent: function () {
            /* open betlist popup*/
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                LicenseeWinLoss.openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _level = 0;
                LicenseeWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#licenseewinloss");
                var url = Site.resolveClientUrl(LicenseeWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                _level = level;
                return LicenseeWinLoss.goBack(level);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/report/LicenseeWL';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }
            LicenseeWinLossOption.urlExcelData = temp;
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
                $(".level1").showHide(Number(LicenseeWinLossOption.licenseeId) === 1);
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('LicenseeWinLoss') + ', {0} → {1} - {2}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyTextList =
            {
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
                _reportBy = reportBy;
                cacheData[level] = response;
                //reset layout
                if (level === 0) {
                    //update productOption
                    $("#ProductOptionJson").val(response.ProductOptions);
                    //update drildownValue
                    $("#OriginalDrawFilter").val($("input[name='drawfilter']:checked").val());
                    $("#OriginalIsExcludedTestData").val($("#IsExcludedTestData").is(":checked"));
                    $("#OriginalCurrencyId").val($("#CurrencyId").val());
                    $("#OriginalReportBy").val(reportBy);
                    $("#OriginalFromDate").val($("#FromDate").val());
                    $("#OriginalToDate").val($("#ToDate").val());
                    $("#OriginalCurrencyName").val($("#CurrencyId option:selected").text());

                    LicenseeWinLoss.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());
                }

                LicenseeWinLoss.renderBreadCrumb(level);
                LicenseeWinLoss.bindData(level, reportBy, response);
            }
        },

        submit: function () {

            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var mainForm = $("#licenseewinloss");
            var option = {
                type: "POST",
                data: mainForm.serialize() +
                    (_sortColumnName ?
                        "&sortby=" + encodeURI(_sortColumnName) + "&sortOrder=" + _sortOrder : ""),
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(LicenseeWinLossOption.urlData),
                error: function (xhr) {
                    Site.gotoErrorPage(xhr.status);
                },
                success: function (response) {
                    LicenseeWinLoss.setExcelUrl(0, 0, 0, "", "");
                    LicenseeWinLoss.ajaxComplete(0, response);
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

        drillDown: function (level, currencyId, customerId, datetime, keepSorting) {
            level = Number(level);
            if (!keepSorting) {
                _removeSorting = true;
            }

            if (_level !== level) {
                _level = level;
                _currencyId = currencyId;
                _customerId = customerId;
                _datetime = datetime;
            }
            var productOptions = $.parseJSON($("#ProductOptionJson").val());
            var param =
            {
                ProductOptions: productOptions,
                DrawFilter: $("#OriginalDrawFilter").val(),
                IsExcludedTestData: $("#OriginalIsExcludedTestData").val(),
                CurrencyId: $("#OriginalCurrencyId").val(),
                ReportBy: $("#OriginalReportBy").val(),
                FromDate: $("#OriginalFromDate").val(),
                ToDate: $("#OriginalToDate").val(),
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
                    tdate = LicenseeWinLoss.lastDateOfMonth(year, month);
                }
            }
            if (keepSorting && _sortColumnName) {
                param.SortBy = _sortColumnName;
                param.SortOrder = _sortOrder;
            }

            var postData = JSON.stringify(param);


            var option = {
                type: "POST",
                data: postData,
                dataType: "json",
                contentType: "application/json",
                cache: false,

                url: Site.resolveClientUrl(LicenseeWinLossOption.urlData +
                    "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId + "&fdate=" +
                    fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                error: function (/*xhRequest, errorText, thrownError*/) { },
                success: function (response) {
                    LicenseeWinLoss.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && Number(param.CurrencyId) === 0) {
                            LicenseeWinLoss.setReportTitle(currencyId, fdate, tdate);
                        } else {
                            LicenseeWinLoss.setReportTitle(param.CurrencyId, fdate, tdate);
                        }
                        //update excel ULR
                        LicenseeWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate);
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
            var reportBy = $("#OriginalReportBy").val();

            if (level === 0) {
                LicenseeWinLoss.renderBreadCrumb(level);
                LicenseeWinLoss.bindData(0, reportBy, cacheData[level]);
                //reset excel Link
                LicenseeWinLoss.setExcelUrl(0, 0, 0, "", "");
                //re-set title
                var fromdate = new Date($("#OriginalFromDate").val());
                var todate = new Date($("#OriginalToDate").val());
                var currencyId = $("#OriginalCurrencyId").val();
                LicenseeWinLoss.setReportTitle(currencyId, fromdate, todate);
            }
            else if (level === 1) {
                LicenseeWinLoss.bindData(1, reportBy, cacheData[level]);
                LicenseeWinLossOption.urlExcelData = LicenseeWinLossOption.urlExcelData.replace("level=2", "level=1");
                LicenseeWinLoss.renderBreadCrumb(level);
            }
        },

        //#BindReport

        bindTotal: function (level, totalData, summaryTotal) {
            var productOptions = $.parseJSON($("#ProductOptionJson").val());
            productOptions = productOptions.reverse(); //make orders of product columns like excel file
            var columnModel = LicenseeWinLossOption.getTotalColModel(level, productOptions);
            var colspan = LicenseeWinLossOption.totalColspan[level];
            var summaryColspan = colspan + 1;

            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        },

        bindData: function (level, reportBy, response) {
            level = Number(level);
            var $gridBody = $(".ui-jqgrid-bdiv");

            var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;

            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");


            var productOptions = $.parseJSON($("#ProductOptionJson").val());

            productOptions = productOptions.reverse(); //make orders of product columns like excel file

            var numOfRow = response.Data.length;

            var numOfTotalRow = response.Total.length;
            totalRowCount = numOfTotalRow;

            var onPaging = false;

            var gridOption = LicenseeWinLossOption.getGridOption(level, reportBy, productOptions);
            gridOption.rownumbers = true;
            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowNum = level === 2 ? _pageSize : LicenseeWinLossOption.nonePagingSize;
            gridOption.rowList = LicenseeWinLossOption.rowList;
            gridOption.rowTotal = numOfRow;
            gridOption.pager = '#ppdata';
            gridOption.onPaging = function (pgButton) {
                if (level === 2 && numOfRow > LicenseeWinLossOption.pageSize) {
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
                level = Number(level);
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);
                if (level === 2 && numOfRow > LicenseeWinLossOption.pageSize) {
                    _pageSize = $("#ReportGrid").getGridParam("rowNum");
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging && level === 2) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        LicenseeWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }
                /*group header*/
                LicenseeWinLossOption.addProductButton(reportGrid, "jqgh_ReportGrid", "ReportGrid", productOptions);

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            if (!_removeSorting) {
                reportGrid.closest("div.ui-jqgrid-view")
                    .find("div.ui-jqgrid-hdiv table.ui-jqgrid-htable tr.ui-jqgrid-labels > th.ui-th-column > div.ui-jqgrid-sortable")
                    .each(function (index, element) {
                        var $this = $(element);
                        $this.find("span.s-ico").remove();

                        if (element.id.indexOf(_sortColumnName) > -1) {
                            var sortClass = "sort-" + _sortOrder;
                            var sortSpan = $("<span style='display: inline; margin: 2px 0px 0px 2px'></span");
                            sortSpan.addClass(sortClass);
                            $this.append(sortSpan);
                            $this.addClass("sorted-heading");
                        }

                    });
            }

            reportGrid.jqGrid('showCol', 'rn');
            LicenseeWinLossOption.setGroupHeaderColumns(reportGrid, productOptions);

            jqGridAdditional.showRecommendedBrowser(gridOption.pager);

            jqGridAdditional.resetLayout(reportGrid);

            LicenseeWinLossOption.restoreExpanse();

            $gridBody = $(".ui-jqgrid-bdiv");
            if ($gridBody.length > 0) {
                $gridBody[0].scrollLeft = scrollLeft;
            }

            LicenseeWinLoss.resizeGrid();
        },

        //#end

        openBetlistPopUp: function (dataDrillDown) {
            dataDrillDown = $.parseJSON(dataDrillDown);
            dataDrillDown.DrawFilter = $("#OriginalDrawFilter").val();
            dataDrillDown.IsExcludedTestData = $("#OriginalIsExcludedTestData").val() === 'True';
            dataDrillDown.CurrencyId = $("#OriginalCurrencyId").val();
            dataDrillDown.CurrencyName = $("#OriginalCurrencyName").val();
            dataDrillDown.BaseCurrency = $("#BaseCurrency").attr("data-original");

            if (typeof dataDrillDown.DrillDownFromDate === 'string') {
                dataDrillDown.DrillDownFromDate = new Date(Date.parse(dataDrillDown.DrillDownFromDate));
                dataDrillDown.DrillDownToDate = new Date(Date.parse(dataDrillDown.DrillDownToDate));
            }

            var reportBy = Number($("#OriginalReportBy").val());
            if (reportBy === 2) {
                var fday = 1;
                var month = dataDrillDown.DrillDownFromDate.getMonth();
                var year = dataDrillDown.DrillDownFromDate.getFullYear();

                var orignalFDate = new Date($("#OriginalFromDate").val());
                var originalTDate = new Date($("#OriginalToDate").val());

                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                dataDrillDown.DrillDownFromDate = new Date(year, month, fday, 0, 0, 0, 0);
                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    dataDrillDown.DrillDownToDate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    dataDrillDown.DrillDownToDate = LicenseeWinLoss.lastDateOfMonth(year, month);
                }
            }

            var currencyName = $.trim($("#CurrencyId option[value=" + dataDrillDown.DrillDownCurrencyId + "]").text());

            var url = Site.resolveClientUrl("BetList/LicenseeWl");
            url = QueryString.setParam(url, "fromdate", dataDrillDown.DrillDownFromDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "todate", dataDrillDown.DrillDownToDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "excludetest", dataDrillDown.IsExcludedTestData);
            url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
            url = QueryString.setParam(url, "CurrencyName", currencyName);
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
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    LicenseeWinLoss.initialize();
    //alert(Date.parse('Apr 5, 2013'));
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    LicenseeWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});