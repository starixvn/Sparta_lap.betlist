/*global Site, Report, QueryString, jqGridAdditional, WinLossByProductOption*/
/// <reference path="/@JSense.js" />

var WinLossByProduct = (function ($) {
    "use strict";
    var LICENSEE_12BET = 1;
    var REPORT_BY_MONTHLY = 2;
    var VALID_MONTH_RANGE = 3;
    var _cacheData = [];
    var ajaxRequest = null;
    var _memberId, _systemId; // store data when submit level 0
    var _submitData = {}; // cache submit data of level 0.

    function setDefaultUsername() {
        var $username = $('#Username');
        var username = $username.val();
        if (username === '' || username === Site.getSingleResource('ExternalIdOrSystemId')) {
            $username.val(Site.getSingleResource('ExternalIdOrSystemId'))
                          .attr('class', 'default-username');
        }
        else {
            $username.attr('class', '');
        }
    }

    function setUsernameOnFocus() {
        var $username = $('#Username');
        var username = $username.val();
        if (username === Site.getSingleResource('ExternalIdOrSystemId')) {
            $username.val('').removeAttr('class');
        }
    }

    function initAutoComplete() {
        var $username = $('#Username');
        var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
        if ($username.length > 0) {
            $username.autocomplete({ serviceUrl: serUrl, minChars: 2 });
            setDefaultUsername();
        }
    }

    return {
        resizeGrid: function () {
            setTimeout(function () {
                if ($(".ui-jqgrid-bdiv").length === 0) {
                    return;
                }
                var gridTop = $(".ui-jqgrid-bdiv").offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv").height() - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(130, gridHeight);
                $(".ui-jqgrid-bdiv").css("height", "auto").css("max-height", gridHeight);
            }, 100);
        },
        //#region Init
        initialize: function () {
            WinLossByProduct.initEvent();
            WinLossByProduct.initSetting();
            initAutoComplete();
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, WinLossByProductOption.urlExcelData);
            $('#Username').val(Site.getSingleResource('ExternalIdOrSystemId'));
            Site.addFilterCollapseExpand();
            Site.addUserGuide({
                resourceName: "WinLossByProduct",
                title: Site.getSingleResource("WinLossByProduct")
            });
        },

        initSetting: function () {
            WinLossByProductOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            WinLossByProductOption.IsAllowBA = Site.asBool($("#IsAllowedBACommission").val());
            WinLossByProductOption.IsAllowRB = Site.asBool($("#IsAllowedRacing").val());
            WinLossByProductOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },

        initEvent: function () {
            /// <summary>
            /// Inititalize events
            /// </summary>
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                WinLossByProduct.openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                WinLossByProduct.submit();
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
                var level = $(this).attr("data-level");
                return WinLossByProduct.goBack(level);
            });

            $("#Username").click(setUsernameOnFocus)
                             .blur(setDefaultUsername);
        },

        //#endregion

        //#region Utilities

        setExcelUrl: function (level, currencyId, custId, fdate, tdate, currencyName) {
            var temp = 'api/report/WinLostByProduct';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId=" + currencyId + "&custId=" + custId + "&fdate=" +
                fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy") +
                "&memberid=" + _memberId + "&systemid=" + _systemId + "&currencyname=" + currencyName;
            }
            WinLossByProductOption.urlExcelData = temp;
        },

        renderBreadCrumb: function (level) {
            level = Number(level);
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
                $(".level1").showHide(WinLossByProductOption.licenseeId === LICENSEE_12BET);
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },

        setReportTitle: function (currencyId, fromdate, todate, currencyName) {
            var titlePattern = Site.getSingleResource('WinLossByProduct') + ', {0} → {1} - {2}{3}';
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
                ? viewinKey.replace("{0}", currencyName || Site.getCurrencyName(currencyId))
                : currencyTextList[currencyId];

            var licenseeWLNote = jqGridAdditional.getLicenseeWLNote(_submitData.ProductOptions, _systemId, true);

            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"),
           currencyStr, (licenseeWLNote.trim() === '' ? '' : ', ' + licenseeWLNote));
            $("div.report-title").html(title);
        },

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        //#endregion

        //#region Get Data

        ajaxComplete: function (level, response) {
            if (response != null) {
                var reportBy = $("input[name='ReportBy']:checked").val();
                //reset layout
                _cacheData[level] = response;
                WinLossByProduct.renderBreadCrumb(level);
                WinLossByProduct.bindData(level, reportBy, response);
            }
        },

        submit: function () {
            Report.setOriginalProduct();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            Site.updateOriginalLicenseeOption();
            var data = $("#winlossbyproduct").serializeObject();

            if (data.Username === Site.getSingleResource('ExternalIdOrSystemId')) {
                data.Username = null;
            }

            var option =
            {
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
                    WinLossByProduct.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());
                    WinLossByProduct.setExcelUrl(0, 0, 0, "", "", "");
                    WinLossByProduct.ajaxComplete(0, response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    // Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }

            ajaxRequest = $.ajax(option);
            return true;
        },

        //#endregion

        //#region drillDown

        drillDown: function (level, currencyId, customerId, datetime, currencyName) {
            var param =
            {
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
            var fdate = new Date(datetime);
            var tdate = new Date(datetime);
            if (param.ReportBy === REPORT_BY_MONTHLY) {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = param.FromDate;
                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                fdate = new Date(year, month, fday, 0, 0, 0, 0);
                var originalTDate = param.ToDate;

                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    tdate = WinLossByProduct.lastDateOfMonth(year, month);
                }
            }
            var option =
            {
                type: "POST",
                data: JSON.stringify(param),
                dataType: "json",
                contentType: "application/json",
                cache: false,

                url: Site.resolveClientUrl(WinLossByProductOption.urlData + "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                     "&fdate=" + fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                success: function (response) {
                    WinLossByProduct.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && param.CurrencyId === '0') {
                            WinLossByProduct.setReportTitle(currencyId, fdate, tdate, currencyName);
                        } else {
                            WinLossByProduct.setReportTitle(param.CurrencyId, fdate, tdate, currencyName);
                        }
                        //update excel ULR
                        WinLossByProduct.setExcelUrl(level, currencyId, customerId, fdate, tdate, currencyName);
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
            var reportBy = _submitData.ReportBy;
            Site.blockUI();

            level = Number(level);
            setTimeout(function () {
                if (level === 0) {
                    WinLossByProduct.renderBreadCrumb(level);
                    WinLossByProduct.bindData(0, reportBy, _cacheData[0]);

                    //reset excel Link
                    WinLossByProduct.setExcelUrl(0, 0, 0, "", "", "");

                    //re-set title                
                    WinLossByProduct.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }
                else if (level === 1) {
                    WinLossByProduct.bindData(1, reportBy, _cacheData[1]);
                    WinLossByProductOption.urlExcelData = WinLossByProductOption.urlExcelData.replace("level=2", "level=1");
                    WinLossByProduct.renderBreadCrumb(level);
                }
            }, 50);
        },

        //#endregion

        //#region Bind Data
        bindSummaryTotal: function (columnModel, summaryTotal) {
            /// <summary>
            /// Bind summary total data to footer rows
            /// </summary>
            /// <param name="columnModel"></param>
            /// <param name="summaryTotal"></param>
            /// <returns type="int">Number of iserted row</returns>
            if (summaryTotal === undefined || summaryTotal.length === 0) {
                return;
            }

            var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
            var rowCount = 0;
            var currencyList = [];

            function reverseArray(temp) {
                /// reverse array before bind to browser (no change sorting)
                $(temp.reverse()).each(function (totalIndex, totalElement) {
                    var row = mainFooter.clone();
                    row.addClass("total-eur total-eur-none-top-se");
                    var cellList = $("td", row);
                    var firstCell = null;
                    $(columnModel).each(function (cellIndex, modelElement) {
                        var cell = cellList.get(cellIndex);
                        var isCustomFormater = typeof (modelElement.formatter) === "function";
                        var rawValue = totalElement[modelElement.name];
                        var value = isCustomFormater ? modelElement.formatter(rawValue, modelElement.formatoptions, row) :
                                $.fn.fmatter(modelElement.formatter, rawValue, modelElement.formatoptions);
                        var titleValue = value;
                        if ($.isNumeric(rawValue) && rawValue < 0) {
                            var wrapper = document.createElement('div');
                            wrapper.innerHTML = value;
                            titleValue = wrapper.outerHtml;
                        }

                        $(cell).html(value).attr("title", titleValue);
                        firstCell = !firstCell ? $(cell) : firstCell;
                    });

                    firstCell.html("Total in " + currency);
                    firstCell.addClass("group-" + currency);
                    row.insertAfter(mainFooter);
                    rowCount++;
                });
            }

            for (var currency in summaryTotal) {
                currencyList.push(currency);
                var summaryData = summaryTotal[currency];
                var temp = Site.clone(summaryData);
                reverseArray(temp);
            }

            $(".ui-jqgrid-ftable").attr("data-total-currency-list", currencyList.join());
            mainFooter.remove();

            return rowCount;
        },

        bindTotal: function (level, totalData, summaryTotal) {
            var productOptions = $.parseJSON(_submitData.ProductOptions);
            var columnModel = WinLossByProductOption.getTotalColModel(level, productOptions);
            var colspan = WinLossByProductOption.totalColspan[level];
            var summaryColspan = colspan + 1;
            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = WinLossByProduct.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, true);
        },

        bindData: function (level, reportBy, response) {
            $("#ReportGrid").hide();
            jqGridAdditional.clearGrid("ReportGrid");

            level = Number(level);
            var reportGrid = $("#ReportGrid");
            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;
            var onPaging = false;

            var gridOption = WinLossByProductOption.getGridOption(level, reportBy);
            gridOption.data = response.Data;
            gridOption.sortname = "Index";
            gridOption.rowNum = level === 2 ? WinLossByProductOption.pageSize : WinLossByProductOption.nonePagingSize;
            gridOption.rowList = WinLossByProductOption.rowList;
            gridOption.rowTotal = numOfRow;
            gridOption.onPaging = function (pgButton) {
                if (level === 2 && numOfRow > WinLossByProductOption.pageSize) {
                    if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                        Site.blockUI();
                    }
                    onPaging = true;
                    setTimeout(function () {
                        WinLossByProduct.resizeGrid();
                    }, 1000);
                }
                else {
                    return 'stop';
                }
            };
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow);
                switch (level) {
                    case 0:
                        reportGrid.tableRowSpan([0, 4]);
                        break;
                    case 1:

                        reportGrid.tableRowSpan([0, WinLossByProductOption.systemIdIndex], WinLossByProductOption.currencyIndex);
                        break;
                    case 2:
                        reportGrid.tableRowSpan([0, WinLossByProductOption.externalIdIndex, WinLossByProductOption.systemIdIndex],
                                                     WinLossByProductOption.currencyIndex);

                        break;
                }
                if (level === 2 && numOfRow > WinLossByProductOption.pageSize) {
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel, response.ErrorMessage);
                }
                else {
                    if (onPaging && level === 2) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        WinLossByProduct.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                Site.unBlockUI();
            };

            setTimeout(function () {
                reportGrid.jqGrid(gridOption);

                if (WinLossByProductOption.hasCustomerComm) {
                    jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
                }

                //set rowspan for footer row
                $("table.ui-jqgrid-ftable").tableRowSpan([WinLossByProductOption.currencyIndex]);
                if (level === 2) {
                    jqGridAdditional.showRecommendedBrowser(gridOption.pager);
                }

                $("#ReportGrid").show();
                ///reset layout
                jqGridAdditional.resetLayout(reportGrid);
                WinLossByProduct.resizeGrid();
            }, 50);
        },

        //#endregion

        //#region Betlist

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

            if (_submitData.ReportBy === REPORT_BY_MONTHLY) {
                var fday = 1;
                var month = dataDrillDown.DrillDownFromDate.getMonth();
                var year = dataDrillDown.DrillDownFromDate.getFullYear();

                var orignalFDate = _submitData.FromDate;
                var originalTDate = _submitData.ToDate;

                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                dataDrillDown.DrillDownFromDate = new Date(year, month, fday, 0, 0, 0, 0);
                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    dataDrillDown.DrillDownToDate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    dataDrillDown.DrillDownToDate = WinLossByProduct.lastDateOfMonth(year, month);
                }
            }

            var url = Site.resolveClientUrl("BetList/LicenseeWl");
            url = Site.setProductOptionOriginalParamList(url);
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", dataDrillDown.DrillDownFromDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "todate", dataDrillDown.DrillDownToDate.toString("MM/dd/yyyy"));
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
            var dialogForm = '<div title="Customer Bet List"><iframe scrolling="no" frameborder="0" width="' + popW +
                         '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 50
            });
        }

        //#endregion
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    WinLossByProduct.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    WinLossByProduct.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});