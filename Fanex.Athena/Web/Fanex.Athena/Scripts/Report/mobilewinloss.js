/*global Site, Report, QueryString, MobileWinLossOption, JSLINQ,jqGridAdditional*/
/// <reference path="/@JSense.js" />
/// //##
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var MobileWinLoss = (function ($) {
    "use strict";
    var cacheData = {};
    var _level = null;
    var _sortColumnName = null;
    var _sortOrder = "desc";
    var _removeSorting = false;
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
                if ($(".ui-jqgrid-bdiv").length === 0) {
                    return;
                }

                var height = $("#ReportGrid").height();
                var gridTop = $(".ui-jqgrid-bdiv").offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv").height() - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(120, gridHeight);
                $(".ui-jqgrid-bdiv").css("height", height).css("max-height", gridHeight);
            }, 100);
        },
        //#region Init
        initialize: function () {
            MobileWinLoss.initSetting();
            MobileWinLoss.initMonthPicker();
            MobileWinLoss.initEvent();
            MobileWinLoss.setExcelUrl(0, 0, 0, "");
            Site.addFilterCollapseExpand();
            MobileWinLossOption.gridOption.onSortCol = function (index) {
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

                    MobileWinLoss.bindData(_level, cacheData[_level]);

                    Site.unBlockUI();
                }, 100);
                return 'stop';
            };
            Site.addUserGuide({
                resourceName: "MobileWL",
                title: Site.getSingleResource("MobileWinLoss")
            });
        },
        initSetting: function () {
            _pageSize = MobileWinLossOption.pageSize;
            MobileWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            MobileWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },
        initMonthPicker: function () {
            $('#DateControl').monPickr({
                minDate: Site.getHistoryDate(),
                showDateRange: true,
                fromDayId: 'MonthPickerFromDay',
                toDayId: 'MonthPickerToDay',
                month: new Date($('#FromDate').val()),
                fromDay: new Date($('#FromDate').val()).getDate(),
                toDay: new Date($('#ToDate').val()).getDate(),
                afterChangeMonth: function () {
                    $('#Year').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getFullYear());
                    $('#Month').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getMonth() + 1);
                },
                afterChangeDay: function () {
                    $('#FromDay').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getDate());
                    $('#ToDay').val(new Date($("#DateControl").monPickr("getToDate", "MM/dd/yyyy")).getDate());
                }
            });
        },
        initEvent: function () {
            // open betlist pop up
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                MobileWinLoss.openBetlistPopUp(dataDrillDown);
            });

            $("#SiteList input[type=checkbox]").checkboxgroup();
            // events

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                _removeSorting = true;
                MobileWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#MobileWinLossForm");
                var url = Site.resolveClientUrl(MobileWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                return MobileWinLoss.goBack(level);
            });
        },

        //#endregion

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, datetime) {
            var temp = 'api/report/MobileWinloss';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId=" + currencyId + "&custId=" + custId + "&date=" + datetime;
            }
            MobileWinLossOption.urlExcelData = temp;
        },

        renderBreadCrumb: function (level) {
            //hide history link
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
                $(".level1").showHide(MobileWinLossOption.licenseeId === 1);
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },
        setReportTitle: function (currencyId, site, fromdate, todate) {
            var titlePattern = Site.getSingleResource('MobileWinLoss')
                + ', {0} → {1} - {2} - '
                + Site.getSingleResource('Site') + ': {3}{4}';
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

            //get site
            var siteStr = MobileWinLossOption.sites[site];
            var licenseeWLNote = jqGridAdditional.getLicenseeWLNote($("#ProductOptionJson").val());
            var title = $.format(titlePattern, fromdate.toString("M/d/yyyy"), todate.toString("M/d/yyyy"),
                        currencyStr, siteStr, (licenseeWLNote.trim() === '' ? '' : ', ' + licenseeWLNote));
            $("div.report-title").html(title);
        },
        //#endregion

        //#region BindReport

        bindTotal: function (totalData, summaryTotal) {
            var columnModel = MobileWinLossOption.getTotalColModel();
            var colspan = MobileWinLossOption.totalColspan[0];
            var summaryColspan = colspan + 1;
            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        },
        bindData: function (level, response) {
            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            var onPaging = false;
            var gridOption = MobileWinLossOption.getGridOption(level);
            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;
            gridOption.data = response.Data;
            gridOption.userData = response.TotalEur;
            gridOption.rowList = MobileWinLossOption.rowList;
            gridOption.rowNum = _pageSize;

            gridOption.rowTotal = numOfRow;
            gridOption.pager = '#ppdata';

            gridOption.onPaging = function (pgButton) {
                if (numOfRow > MobileWinLossOption.pageSize) {
                    onPaging = true;
                    if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                        Site.blockUI();
                    }
                }
                else {
                    return 'stop';
                }
            };

            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);
                if (numOfRow > MobileWinLossOption.pageSize) {
                    _pageSize = Number($("#ReportGrid").getGridParam("rowNum"));
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        MobileWinLoss.bindTotal(response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                MobileWinLoss.resizeGrid();
                Site.unBlockUI();
            };
            // Show the grid
            reportGrid.jqGrid(gridOption);
            /*group header*/
            if (MobileWinLossOption.hasCustomerComm) {
                jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
            }
            else {
                jqGridAdditional.resetHeaderHeight(reportGrid);
            }
            jqGridAdditional.resetLayout(reportGrid);

            jqGridAdditional.showRecommendedBrowser(gridOption.pager);

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


        },

        //#endregion

        //#region GetData

        ajaxComplete: function (level, response) {
            _level = Number(level);
            if (response != null) {
                cacheData[level] = response;
                //reset layout
                switch (level) {
                    case 0:
                        //update productOption
                        $("#ProductOptionJson").val(response.ProductOptions);
                        //update drildownValue
                        $("#drillDownDrawFilter").val($("input[name='DrawFilter']:checked").val());
                        $("#drillDownExcludeTest").val($("#IsExcludeTest").is(":checked"));
                        $("#drillDownSite").val($("input[name='Site']:checked").val());
                        $("#drillDownCurrencyId").val($("#CurrencyId").val());
                        var fromdate = new Date($("#Year").val(), $("#Month").val() - 1, $("#FromDay").val(), 0, 0, 0, 0);
                        var todate = new Date($("#Year").val(), $("#Month").val() - 1, $("#ToDay").val(), 0, 0, 0, 0);
                        var currencyId = $("#CurrencyId").val();
                        var site = $("input[name='Site']:checked").val();
                        $("#drillDownFromDate").val(fromdate.toString("M/d/yyyy"));
                        $("#drillDownToDate").val(todate.toString("M/d/yyyy"));

                        MobileWinLoss.setReportTitle(currencyId, site, fromdate, todate);
                        break;
                    case 1:
                        break;
                }

                MobileWinLoss.renderBreadCrumb(level);
                MobileWinLoss.bindData(level, response);
            }
        },

        submit: function () {
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var mainForm = $("#MobileWinLossForm");

            $.ajax({
                type: "POST",
                data: mainForm.serialize(),
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(MobileWinLossOption.urlData),
                success: function (response) {
                    MobileWinLoss.setExcelUrl(0, 0, 0, "");
                    MobileWinLoss.ajaxComplete(0, response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });

            return true;
        },

        drillDown: function (level, currencyId, customerId, datetime) {
            var productOptions = $.parseJSON($("#ProductOptionJson").val());
            var param =
            {
                ProductOptions: productOptions,
                Site: $("#drillDownSite").val(),
                DrawFilter: $("#drillDownDrawFilter").val(),
                IsExcludeTest: $("#drillDownExcludeTest").val(),
                CurrencyId: $("#drillDownCurrencyId").val(),
                BaseCurrency: $("#BaseCurrency").attr("data-original")
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(param),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(MobileWinLossOption.urlData + "?level=" + level +
                            "&currencyId=" + currencyId + "&custId=" + customerId + "&date=" + datetime),
                success: function (response) {
                    MobileWinLoss.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        MobileWinLoss.setReportTitle(param.CurrencyId, param.Site, new Date(Date.parse(datetime)), new Date(Date.parse(datetime)));
                        //update excel ULR
                        MobileWinLoss.setExcelUrl(level, currencyId, customerId, datetime);
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
        },

        goBack: function (level) {
            MobileWinLoss.bindData(level, cacheData[level]);
            if (level === 0) {
                MobileWinLoss.renderBreadCrumb(level);
                //reset excel Link
                MobileWinLoss.setExcelUrl(0, 0, 0, "");
                //re-set title
                var fromdate = new Date($("#drillDownFromDate").val());
                var todate = new Date($("#drillDownToDate").val());
                var currencyId = $("#CurrencyId").val();
                var site = $("input[name='Site']:checked").val();
                MobileWinLoss.setReportTitle(currencyId, site, fromdate, todate);
            }
            else if (level === 1) {
                MobileWinLossOption.urlExcelData = MobileWinLossOption.urlExcelData.replace("level=2", "level=1");
                MobileWinLoss.renderBreadCrumb(level);
            }
        },

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        openBetlistPopUp: function (dataDrillDown) {
            dataDrillDown = $.parseJSON(dataDrillDown);
            dataDrillDown.DrawFilter = $("#drillDownDrawFilter").val();
            dataDrillDown.IsExcludedTestData = $("#OriginalIsExcludedTestData").val() === 'True';
            dataDrillDown.CurrencyId = $("#OriginalCurrencyId").val();
            dataDrillDown.CurrencyName = $("#OriginalCurrencyName").val();
            dataDrillDown.BaseCurrency = $("#BaseCurrency").attr("data-original");

            var currencyName = $.trim($("#CurrencyId option[value=" + dataDrillDown.DrillDownCurrencyId + "]").text());
            var url = Site.resolveClientUrl("BetList/MobileWl");

            url = QueryString.setParam(url, "fromdate", $("#drillDownFromDate").val());
            url = QueryString.setParam(url, "todate", $("#drillDownToDate").val());
            url = QueryString.setParam(url, "SiteType", $("#drillDownSite").val());
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

            var popH = Math.min(600, window.innerHeight - 100), popW = 920;
            var dialogForm = '<div title="Customer Bet List"><iframe frameborder="0" width="' + popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        }
        //#endregion
    };
})(jQuery);
$(window).load(function () {
    "use strict";
    MobileWinLoss.initialize();
});
//#end
$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    MobileWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});