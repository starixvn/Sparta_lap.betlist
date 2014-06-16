/*global Site, TurnoverOption, jqGridAdditional*/
/// <reference path="/@JSense.js" />
"use strict";
var Turnover = (function ($) {
    var ajaxRequest = null;
    var _postdata;
    var _fromDate, _toDate, _currencyName, _currencyId;
    return {
        defaultUserName: Site.getSingleResource('ExternalIdOrSystemId'),
        resizeGrid: function () {
            setTimeout(function () {
                var gridTop = $("#ReportGrid").offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv").height() - 10;
                gridHeight = Math.max(120, gridHeight);
                $(".ui-jqgrid-bdiv").css("height", "auto").css("max-height", gridHeight);
            }, 100);
        },
        initialize: function () {
            Turnover.initMonthPicker();
            Turnover.initEvent();
            Turnover.initAutoComplete();
            Turnover.setDefaultUsername();

            TurnoverOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            TurnoverOption.hasCasinoTransfer = Site.asBool($("#IsAllowedCasinoTransfer").val());
            TurnoverOption.isAllowBa = Site.asBool($("#IsAllowBa").val());
        },
        initMonthPicker: function () {
            $('#DateControl').monPickr({
                minDate: $('#MinDate').val(),
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
                },
                log: function (/*msg*/) {
                    //console.warn(msg);
                }
            });
        },

        initEvent: function () {
            $('#Username').focus(function () {
                Turnover.setUsernameOnFocus();
            });

            $('#Username').blur(function () {
                Turnover.setDefaultUsername();
            });

            $("#SubmitButton").click(function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                Turnover.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#TurnoverForm");
                var url = Site.resolveClientUrl(TurnoverOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $("#data-content").scroll(function () {
                $('#header-content').scrollLeft($('#data-content').scrollLeft());
                $('#total-content').scrollLeft($('#data-content').scrollLeft());
            });
        },

        initAutoComplete: function () {
            var txtUserName = $('#Username');
            var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({ serviceUrl: serUrl, minChars: 2 });
            }
        },

        setDefaultUsername: function () {
            var username = $('#Username').val();
            if (username === '' || username === Turnover.defaultUserName) {
                $('#Username').val(Turnover.defaultUserName);
                $('#Username').attr('class', 'default-username');
            }
            else {
                $('#Username').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#Username').val();
            if (username === Turnover.defaultUserName) {
                $('#Username').val('');
                $('#Username').removeAttr('class');
            }
        },
        setReportTitle: function () {
            var titlePattern = Site.getSingleResource('Turnover');
            var title = null;
            var username = $('#Username').val().trim();
            var currencyTextList =
                {
                    "-2": "usd",
                    "-1": "eur",
                    "0": Site.getSingleResource('AllCurrencies')
                };

            var currencyName = currencyTextList[_currencyId] === undefined
            ? $('#CurrencyId option[value="' + _currencyId + '"]').text().trim() : currencyTextList[_currencyId];

            if (username === '' || username === Turnover.defaultUserName) {
                titlePattern += ", {0} - {1}";
                var currencyStr = Site.getSingleResource("Viewin").replace("{0}", currencyName.toUpperCase());
               
                title = $.format(titlePattern, _fromDate.toString("M/dd/yyyy") + ' → ' + _toDate.toString("M/dd/yyyy"), currencyStr);
            } else {
                titlePattern += ", {0} → {1} {2} ({3})";
                title = $.format(titlePattern, _fromDate.toString("M/dd/yyyy") + ' → ' + _toDate.toString("M/dd/yyyy"),
                        Site.getSingleResource('Customer'), username, currencyName);
            }

            $("div.report-title").html(title);
        },
        ajaxComplete: function (response) {
            if (response != null) {
                //reset layout

                Turnover.setReportTitle();

                // Turnover.renderBreadCrumb(level);
                Turnover.bindData(response);
            }
        },

        submit: function () {

            if ($("#ProductList input:checked").length === 0) {
                return $("#product-null").removeClass("hide");
            } else {
                $("#product-null").addClass("hide");
            }

            Site.blockUI();

            var mainForm = $("#TurnoverForm");
            var postdata = mainForm.serializeObject();
            _fromDate = new Date(postdata.Year, postdata.Month - 1, postdata.FromDay);
            _toDate = new Date(postdata.Year, postdata.Month - 1, postdata.ToDay);
            _currencyName = $("#CurrencyName").val().toUpperCase();
            _currencyId = $("#CurrencyId").val();

            postdata.BaseCurrency = postdata.BaseCurrency.toUpperCase();

            postdata.FromDate = _fromDate.toDateString();
            postdata.ToDate = _toDate.toDateString();
            _postdata = postdata;
            var option =
            {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(TurnoverOption.urlData),
                // error: function (xhRequest, errorText, thrownError) { },
                success: function (response) {
                    $("#BackLink").hide();
                    Turnover.ajaxComplete(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    $(".report-title").show();
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
        },

        bindData: function (response) {

            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            $("#paging").hide();
            var numOfRow = response.Data.length;
            // var numOfTotalRow = response.Total.length;
            var onPaging = false;
            var gridOption = TurnoverOption.getGridOption();


            gridOption.footerrow = true;
            gridOption.userDataOnFooter = true;

            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowNum = TurnoverOption.pageSize;
            gridOption.rowList = TurnoverOption.rowList;
            gridOption.rowTotal = numOfRow;
            gridOption.pager = '';
            gridOption.onPaging = function (pgButton) {
                if (numOfRow > TurnoverOption.pageSize) {
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
                // jqGridAdditional.gridComplete(this, gridOption, 1);

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                    $("tr.no-info-row td:first").css("text-align", "left").css("padding-left", window.innerWidth / 2 - 50)
                }
                else {
                    if (onPaging) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        Turnover.bindTotal(response.TotalRows);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                var groupHeaders = [{
                    startColumnName: "TurnoverBA",
                    numberOfColumns: 3,
                    titleText: Site.getSingleResource('BA')
                }];

                if (TurnoverOption.hasCustomerComm) {
                    groupHeaders.push({
                        startColumnName: "WinLoss",
                        numberOfColumns: 3,
                        titleText: Site.getSingleResource('CustomerW_L')
                    });
                }

                reportGrid.jqGrid('setGroupHeaders',
                {
                    useColSpanStyle: true,
                    groupHeaders: groupHeaders
                });

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            //set rowspan for footer row
            $("table.ui-jqgrid-ftable").tableRowSpan([TurnoverOption.currencyIndex]);
            ///reset layout
            jqGridAdditional.resetLayout(reportGrid);
            setTimeout(function () {
                Turnover.resizeGrid();
            }, 100);
        },

        bindTotal: function (totalData) {
            var columnModel = TurnoverOption.getColModel();
            var colspan = 2;
            jqGridAdditional.assignTotal(totalData, columnModel);
            jqGridAdditional.setGroupFooters(colspan, 0, 0);
            var $td = $(".ui-jqgrid-sdiv table tr td");
            $td.css("background-color", "#b2dfef");
            $($td[2]).css("text-align", "center").html("-");
        }
    };
})(jQuery);

$(window).load(function () {
    Turnover.initialize();
    Turnover.resizeGrid();
});
$(window).resize(function () {
    // Turnover.resizeGrid();
});

$(window).unload(function () {
    $(window).unbind("resize");
});