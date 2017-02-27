define(
    ["jquery", "common/querystring", "common/site", "../report", "./turnover.option",
        "lib/jqgrid/jquery.jqgrid.additional", "moment", "common/popup","components/pluginhelper/monthpicker",
        "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min", "vendor-materialize", "extensions/jquery.extension", "lib/linq", "materialize-autocomplete"],
    function ($, QueryString, Site, Report, TurnoverOption, jqGridAdditional, moment, Popup, MonthPicker) {
        "use strict";
    var ajaxRequest = null;
    var _fromDate, _toDate, _currencyName, _currencyId;
    var _autocomplete, $autoCompleteTarget;

    var _me = {
        defaultUserName: "",
        initialize: function () {
            Site.initialize();
            _me.initEvent();
            $autoCompleteTarget = $('#Username');
            _me.initAutoComplete();
            MonthPicker.initMonthPicker();

            TurnoverOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            TurnoverOption.hasCasinoTransfer = Site.asBool($("#IsAllowedCasinoTransfer").val());
            TurnoverOption.isAllowBa = Site.asBool($("#IsAllowBa").val());
            Site.setMultiSelectForDropDownList('grid-products-multiselect', $("#productsData").val(), 'Code', 'Name', 'Status');
            Site.addUserGuide({
                resourceName: "Turnover",
                title: Site.getSingleResource("Turnover")
            });
        },
        
        initEvent: function () {
            $("#TurnoverForm input").keypress(function (e) {
                if (e.keyCode === 13) {
                    $('#IsExportExcel').val(false);
                    _me.submit();
                    _autocomplete.$dropdown.html('').hide();
                }
            });

            $("#SubmitButton").click(function () {
                $('#IsExportExcel').val(false);
                _me.submit();
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

            $(window).on("resize", function () {
                _me.resizeGrid();
                _me.updateWidthOfAutoCompleteDropdown();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });

            $('body').click(function () {
                _autocomplete.$dropdown.html('').hide();
            });
        },

        initAutoComplete: function () {            
            _autocomplete = $autoCompleteTarget.materialize_autocomplete({ // jshint ignore:line
                ignoreCase: false,
                multiple: {
                    enable: false
                },
                dropdown: {
                    el: '#suggestionDropdown'
                },
                getData: function (value, callback) {
                    var url = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0&query=" + value);

                    $.ajax({
                        type: "GET",
                        dataType: "json",
                        url: url,
                        success: function (response) {
                            var suggestions = response.suggestions;

                            if (suggestions && suggestions.length > 0) {
                                var data = suggestions.map(function (item) {
                                    return { id: item, text: item };
                                });

                                callback(value, data);
                            }
                        }
                    });
                }
            });

            _me.updateWidthOfAutoCompleteDropdown();
        },

        updateWidthOfAutoCompleteDropdown: function () {
            if ($("#suggestionDropdown").width() !== $autoCompleteTarget.width()) {
                $("#suggestionDropdown").width($autoCompleteTarget.width());
            }
        },

        setReportTitle: function () {
            var titlePattern = Site.getSingleResource('Turnover');
            var title = null;
            var username = $('#Username').val().trim();
            var currencyTextList =
                {
                    "-3": "gbp",
                    "-2": "usd",
                    "-1": "eur",
                    "0": Site.getSingleResource('AllCurrencies')
                };

            var currencyName = currencyTextList[_currencyId] ? currencyTextList[_currencyId] : $('#CurrencyId option[value="' + _currencyId + '"]').text().trim();
            var dateString = moment(_fromDate).format("MM/DD/YYYY") + ' → ' + moment(_toDate).format("MM/DD/YYYY");
            currencyName = currencyName.toUpperCase();
            if (username === _me.defaultUserName) {
                titlePattern += ", {0} - {1}";
                var currencyStr = Site.getSingleResource("Viewin").replace("{0}", currencyName);
                title = $.format(titlePattern, dateString, currencyStr);
            } else {
                titlePattern += ", {0} - {1} {2} ({3})";
                title = $.format(titlePattern, dateString, Site.getSingleResource('Customer'), username, currencyName);
            }

            $(".report-title").html(title);
            $(".panel-heading").removeClass("hide");
        },
        ajaxComplete: function (response) {
            if (response !== null) {
                _me.setReportTitle();
                _me.bindData(response);
            }
        },

        setDateParam: function(){
            _fromDate = new Date($("#FromDate").val());
            _toDate = new Date($("#ToDate").val());

            $("#FromDay").val(_fromDate.getDate());
            $("#ToDay").val(_toDate.getDate());
            $("#Month").val(_fromDate.getMonth() + 1);
            $("#Year").val(_fromDate.getFullYear());
        },

        submit: function () {
            if ($("#ProductList .childCheckbox:checked").length === 0) {
                return $("#product-null").removeClass("hide");
            } else {
                $("#product-null").addClass("hide");
            }
            
            Site.blockUI();

            _me.setDateParam();
            _currencyName = $("#CurrencyName").val().toUpperCase();
            _currencyId = $("#CurrencyId").val();

            var mainForm = $("#TurnoverForm");
            var postdata = mainForm.serializeObject();
            if (postdata.Username === _me.defaultUserName) {
                postdata.Username = undefined;
            }

            postdata.BaseCurrency = postdata.BaseCurrency.toUpperCase();
           
            var option =
            {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(TurnoverOption.urlData),
                success: function (response) {
                    _me.ajaxComplete(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    $(".report-title").show();
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest !== null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
        },

        bindData: function (response) {
            jqGridAdditional.clearGrid("ReportGrid");
            var numOfRow = response.Data.length;
            if (response === null || numOfRow === 0) {
                jqGridAdditional.setNoInfo();
                return;
            }
            
            var reportGrid = $("#ReportGrid");
            var gridOption = TurnoverOption.getGridOption();
            gridOption.data = response.Data;
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete();
                _me.bindTotal(response.TotalData);

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

                _me.resizeGrid();
                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);
        },

        bindTotal: function (totalData) {
            var columnModel = TurnoverOption.getColModel();
            var colspan = 2;
            jqGridAdditional.assignTotal(totalData, columnModel);
            jqGridAdditional.setGroupFooters(colspan, 0, 0);
            var $tr = $(".ui-jqgrid-sdiv table tr");
            $tr.find("td[aria-describedby=ReportGrid_OpeningBalance], td[aria-describedby=ReportGrid_ClosingBalance]").css("text-align", "center").html("-");
        },

        resizeGrid: function () {
            var reportGrid = $("#ReportGrid");
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }
    };

    return _me;
});