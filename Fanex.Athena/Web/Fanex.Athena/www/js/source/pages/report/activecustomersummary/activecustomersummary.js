define([
        "jquery",
        "common/site",
        "../report",
        "./activecustomersummary.option",
        "components/pluginhelper/daterangepicker",
        "components/pluginhelper/multiselect",
        "helpers/daterange",
        "lib/jqgrid/jquery.jqgrid.additional",
        "lib/jqgrid/jquery.jqGrid.en",
        "lib/jqgrid/jquery.jqGrid.min",
        "extensions/jquery.extension"],
    function ($, Site, Report, ActiveCustomerOption, DatePicker, MultiSelect, DateHelper, jqGridAdditional) {
        "use strict";
        var VALID_MONTH_RANGE = 3;

        var _ajaxRequest = null;
        var _sortColumnName = "";
        var _sortOrder = "desc";
        var _onSorting = false;
        var _onPaging = false;
        var selector = {
            SubmitButton: "#SubmitButton",
            ExportExcelInput: "#IsExportExcel",
            ExportToExcel: "#ExportToExcel",
            Form: "#mainform",
            Title: ".report-title",
            Header: ".panel-heading",
            GridId: "ReportGrid"
        };

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "ActiveCustomerSummary",
                title: Site.getSingleResource("ActiveCustomerSummary")
            });

            MultiSelect.initProductList();
            DatePicker.initDateRangePicker();
            DateHelper.setConstraintMonthRange(VALID_MONTH_RANGE, "");

            _me.initEvent();
        }

        function initEvent() {
            $(selector.SubmitButton).click(function () {
                $(selector.ExportExcelInput).val(false);
                _me.submit();
            });

            $(selector.ExportToExcel).click(function () {
                $(selector.ExportExcelInput).val(true);
                var mainForm = $(selector.Form);
                var url = Site.resolveClientUrl(ActiveCustomerOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(selector.ExportExcelInput).val(false);
                return false;
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        }

        function setReportTitle(currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('ActiveCustomerSummary') + ', {0} → {1} - {2}';
            var currencyStr = Report.getCurrencyNameForReportTitle(currencyId);

            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"), currencyStr);

            $(selector.Title).html(title);
            $(selector.Header).removeClass("hide");
        }

        function submit() {
            var data = getData();

            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(ActiveCustomerOption.urlData),
                success: function (response) {
                    if (response !== null) {
                        _me.setReportTitle(data.CurrencyId, data.FromDate, data.ToDate);
                        _me.bindData(response);
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            };
            if (_ajaxRequest !== null) {
                _ajaxRequest.abort();
            }

            _ajaxRequest = $.ajax(option);

            return true;
        }

        function bindData(response) {
            jqGridAdditional.clearGrid(selector.GridId);
            _onSorting = false;
            _onPaging = false;

            var numOfRow = response.Data.length;
            if (numOfRow === 0) {
                jqGridAdditional.setNoInfo();
                return;
            }

            var reportGrid = $("#" + selector.GridId);
            var gridOption = getGridOption(reportGrid, response, numOfRow);

            if (numOfRow > ActiveCustomerOption.pageSize) {
                $(gridOption.pager).show();
            }
            else{
                $(gridOption.pager).hide();
            }

            reportGrid.jqGrid(gridOption);           
        }

        function bindTotal(grantTotalCustomer, colspan) {
            var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
            var textFooter = Site.getSingleResource("TotalActiveCustomer")
                            + " = "
                            + jqGridAdditional.formatInteger(grantTotalCustomer);

            mainFooter.find("td:eq(0)")
                .html(textFooter)
                .attr("colspan", colspan)
                .css("text-align", "left")
                .nextAll().remove();
        }

        function resizeGrid() {
            var reportGrid = $("#" + selector.GridId);
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        function getData() {
            var mainForm = $(selector.Form);
            var data = mainForm.serializeObject();
            if (_sortColumnName) {
                data.sortBy = _sortColumnName;
                data.sortOrder = _sortOrder;
            }

            return data;
        }

        function getGridOption(reportGrid, response, numOfRow) {
            var productOptions = $.parseJSON(response.ProductOptions);
            var gridOption = ActiveCustomerOption.getGridOption(productOptions, numOfRow);
            gridOption.data = response.Data;
            gridOption.rowNum = ActiveCustomerOption.pageSize;

            gridOption.gridComplete = function () {
                if (!_onSorting && !_onPaging) {
                    jqGridAdditional.gridComplete();
                    jqGridAdditional.removeCursorOnNonSortableHeader(reportGrid);
                    _me.bindTotal(response.GrantTotalCustomer, gridOption.colModel.length);
                    ActiveCustomerOption.setGroupHeaderColumns(reportGrid, productOptions);
                    resizeGrid();
                }
                if (_onSorting) {
                    jqGridAdditional.removeAndAddSortableHeader(reportGrid, _sortColumnName, _sortOrder);
                    _onSorting = false;
                }
                Site.unBlockUI();
            };

            gridOption.onSortCol = function (index, columnIndex, sortOrder) {
                Site.blockUI();
                _sortOrder = sortOrder;
                _sortColumnName = index;
                _onSorting = true;
            };

            gridOption.onPaging = function () {
                if (numOfRow > ActiveCustomerOption.pageSize) {
                    Site.blockUI();
                    _onPaging = true;
                }
                else {
                    return 'stop';
                }
            };

            return gridOption;
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent,
            setReportTitle: setReportTitle,
            submit: submit,
            bindData: bindData,
            bindTotal: bindTotal
        };

        return _me;
    });