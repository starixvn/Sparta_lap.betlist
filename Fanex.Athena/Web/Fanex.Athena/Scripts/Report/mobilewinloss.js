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
    var _submitData = {};
    var _drilldownData = {};

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
                var gridTop = $(".ui-jqgrid-bdiv").offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv").height() - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(130, gridHeight);
                $(".ui-jqgrid-bdiv").css("height", "auto").css("max-height", gridHeight);
            }, 100);
        },
        hasHorizontalScrollBar: function (element) {
            return element.get(0) ? element.get(0).scrollWidth > (element.innerWidth() - 30) : false;
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
                title: Site.getSingleResource("PlatformWinLoss")
            });
        },
        initSetting: function () {
            _pageSize = MobileWinLossOption.pageSize;
            MobileWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
        },
        initMonthPicker: function () {
            $('#DateControl').monPickr({
                minDate: Site.getHistoryDate(),
                maxDate: $("#MaxDate").val(),
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
                var dataDrillDown = $.parseJSON($(this).attr("data-drill-down"));
                MobileWinLoss.openBetlistPopUp(dataDrillDown);
            });

            $("body").on("click", ".drill-down-link", function () {
                var dataDrillDown = $.parseJSON($(this).attr("data-drill-down"));
                MobileWinLoss.drillDown(dataDrillDown);
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
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },
        setReportTitle: function (data) {

            var titlePattern = Site.getSingleResource('PlatformWinLoss')
                + ', {0} → {1} - {2} - '
                + Site.getSingleResource('Platform') + ': {3}{4}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyTextList =
            {
                "-3": viewinKey.replace("{0}", "GBP"),
                "-2": viewinKey.replace("{0}", "USD"),
                "-1": viewinKey.replace("{0}", "EUR"),
                "0": Site.getSingleResource('AllCurrencies')
            };

            var currencyStr = currencyTextList[data.CurrencyId] === undefined
                ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + data.CurrencyId + '"]').text())
                : currencyTextList[data.CurrencyId];

            var platformName = MobileWinLossOption.Platforms[data.Platform];

            var licenseeWLNote = jqGridAdditional.getLicenseeWLNote(_submitData.ProductOptions, "Product: ");
            var title = $.format(titlePattern, _submitData.FromDate.toString("M/d/yyyy"), _submitData.ToDate.toString("M/d/yyyy"),
                        currencyStr, platformName, (licenseeWLNote.trim() === '' ? '' : ', ' + licenseeWLNote));

            $(".report-title").html(title);
        },
        //#endregion

        //#region BindReport

        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = MobileWinLossOption.getTotalColModel();
            if (level === 0) {
                jqGridAdditional.assignTotal(totalData, columnModel);
                jqGridAdditional.setGroupFooters(1);
                //$('.ui-jqgrid-ftable td:visible:first').html(Site.getSingleResource('Total'));
            }
            else {
                var colspan = MobileWinLossOption.totalColspan[level];
                var summaryColspan = colspan + 1;
                jqGridAdditional.assignTotal(totalData, columnModel);
                var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
                jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
            }
        },
        bindData: function (level, response) {
            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            var onPaging = false;
            var gridOption = MobileWinLossOption.getGridOption(level);
            var numOfRow = response.Data.length;
            var numOfTotalRow = 1;//response.Total.length;
            gridOption.datastr = response.Data;
          //  gridOption.userData = response.TotalEur;
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
                        MobileWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                MobileWinLoss.resizeGrid();
                $(".cell-wrapperleaf", "#ReportGrid").closest("tr").find("td").addClass("highlight");
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
                        MobileWinLoss.setReportTitle(_submitData);
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

            var fromdate = new Date($("#Year").val(), $("#Month").val() - 1, $("#FromDay").val(), 0, 0, 0, 0);
            var todate = new Date($("#Year").val(), $("#Month").val() - 1, $("#ToDay").val(), 0, 0, 0, 0);

            var data = $("#MobileWinLossForm").serializeObject();
            data.FromDate = fromdate;
            data.ToDate = todate;
            data.IsExcludeTest = $('#IsExcludeTest').is(':checked');
            data.Level = 0;
            $.ajax({
                type: "POST",
                data: data,
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(MobileWinLossOption.urlData),
                success: function (response) {
                    _submitData = {
                        FromDay: data.FromDay,
                        ToDay: data.ToDay,
                        Month: data.Month,
                        Year: data.Year,
                        ProductOptions: response.ProductOptions,
                        DrawFilter: data.DrawFilter,
                        Platform: data.Platform,
                        CurrencyId: data.CurrencyId,
                        FromDate: new Date(data.FromDate),
                        ToDate: new Date(data.ToDate),
                        CurrencyName: data.CurrencyName,
                        IsExcludeTest: data.IsExcludeTest
                    };

                    MobileWinLoss.setExcelUrl(0, 0, 0, "");
                    MobileWinLoss.ajaxComplete(0, response);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });

            return true;
        },

        drillDown: function (data) {

            var param =
            {
                Level: 1,
                FromDay: _submitData.FromDay,
                ToDay: _submitData.ToDay,
                Month: _submitData.Month,
                Year: _submitData.Year,
                FromDate: _submitData.FromDate,
                ToDate: _submitData.ToDate,
                ProductOptions: $.parseJSON(_submitData.ProductOptions),
                Platform: data.Platform,
                DrawFilter: _submitData.DrawFilter,
                IsExcludeTest: _submitData.IsExcludeTest,
                CurrencyId: _submitData.CurrencyId,
                BaseCurrency: $("#BaseCurrency").attr("data-original")
            };
            _drilldownData = {
                Platform: data.Platform
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(param),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(MobileWinLossOption.urlData),
                success: function (response) {
                    MobileWinLoss.ajaxComplete(1, response);
                    if (response != null && response.Data.length > 0) {
                        MobileWinLoss.setReportTitle(param);
                        MobileWinLoss.setExcelUrl(param);
                    }
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        },

        goBack: function (level) {
            MobileWinLoss.bindData(level, cacheData[level]);
            if (level === 0) {
                MobileWinLoss.renderBreadCrumb(level);
                MobileWinLoss.setExcelUrl(0, 0, 0, "");
                MobileWinLoss.setReportTitle({
                    CurrencyId: _submitData.CurrencyId,
                    Platform: _submitData.Platform
                });
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
            dataDrillDown.BaseCurrency = $("#BaseCurrency").attr("data-original");

            var url = Site.resolveClientUrl("BetList/MobileWl");

            url = QueryString.setParam(url, "fromdate", _submitData.FromDate);
            url = QueryString.setParam(url, "todate", _submitData.ToDate);
            url = QueryString.setParam(url, "SiteType", _drilldownData.Platform);
            url = QueryString.setParam(url, "excludetest", _submitData.IsExcludeTest);
            url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
            url = QueryString.setParam(url, "CurrencyName", dataDrillDown.DrillDownCurrencyName);
            url = QueryString.setParam(url, "drawfilter", _submitData.DrawFilter);
            url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
            url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
            url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
            url = QueryString.setParam(url, "BaseCurrency", $("#BaseCurrency").attr("data-original"));
            url = QueryString.setParam(url, "OriginalCurrenceId", _submitData.CurrencyId);
            url = QueryString.setParam(url, "OriginalCurrencyName", _submitData.CurrencyName);

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