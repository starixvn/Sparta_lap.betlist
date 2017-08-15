define([
    "jquery",
    "common/site",
    "lib/jqgrid/jquery.jqgrid.additional",
    "./customerbalance.option",
    "components/pluginhelper/daterangepicker",
    "components/pluginhelper/multiselect",
    "helpers/daterange",
    "components/paging",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, Site, jqGridAdditional, CustomerBalanceOption, DatePicker, MultiSelect, DateHelper, PagingHelper) {
        "use strict";

        var VALID_MONTH_RANGE = 2;

        var _ajaxRequest = null;
        var _sortColumnIndex = "";
        var _sortType = "desc";
        var _isFirstLoad = true;
        var _pagingOption =
        {
            pageindex: 1,
            pagesize: 1000,
            pagelist: [1000, 2000],
            totalrow: 1,
            elementcontainer: ".paging",
            method: "AJAX",
            ajaxfunction: "CustomerBalance.ajaxPaging()",
            alwayshow: true
        };
        var _selector = {
            SubmitButton: "#SubmitButton",
            ExportExcelInput: "#IsExportExcel",
            ExportToExcel: "#ExportToExcel",
            Form: "#CustomerBalanceForm",
            Title: ".report-title",
            Header: ".panel-heading",
            GridId: "ReportGrid",
            OrderItemInput: "#OrderItem"
        };

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "CustomerBalance",
                title: Site.getSingleResource("CustomerBalance")
            });
            MultiSelect.initProductList();
            DatePicker.initDateRangePicker();
            DateHelper.setConstraintMonthRange(VALID_MONTH_RANGE, CustomerBalanceOption.urlExcelData);

            _me.initEvent();
            _me.initSetting();
        }

        function initEvent() {
            $(_selector.SubmitButton).click(function () {
                _pagingOption.pageindex = 1;
                _me.submit();
            });

            $(_selector.ExportToExcel).click(function () {
                $(_selector.ExportExcelInput).val(true);
                var mainForm = $(_selector.Form);
                var url = Site.resolveClientUrl(CustomerBalanceOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(_selector.ExportExcelInput).val(false);
                return false;
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });

            window.CustomerBalance = {
                ajaxPaging: _me.ajaxPaging
            };
        }

        function initSetting() {
            CustomerBalanceOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            CustomerBalanceOption.isAllowBA = Site.asBool($("#IsAllowedBACommission").val());
        }

        function submit() {
            var data = getPostData();

            var option =
            {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CustomerBalanceOption.urlData),
                success: function (response) {
                    setReportTitle(data.FromDate, data.ToDate, data.CurrencyName);
                    _me.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            };

            if (_ajaxRequest != null) {
                _ajaxRequest.abort();
            }

            _ajaxRequest = $.ajax(option);
            return true;
        }

        function bindData(response) {
            Site.blockUI();
            jqGridAdditional.clearGrid(_selector.GridId);
            _isFirstLoad = true;

            createPaging(response.TotalRows);

            if (response.TotalRows === 0) {
                jqGridAdditional.setNoInfo();
                Site.unBlockUI();
                return;
            }

            var gridOption = CustomerBalanceOption.getGridOption(response.TotalRows);
            gridOption.data = response.Data;
            gridOption.onSortCol = function (index) {
                sort(index);
                return "stop";
            };
            gridOption.gridComplete = function () {
                onGridComplete(this, gridOption, response);
                Site.unBlockUI();
            };

            var reportGrid = $("#" + _selector.GridId);
            reportGrid.jqGrid(gridOption);

            CustomerBalanceOption.setGroupHeaderColumns(reportGrid);
        }

        function ajaxPaging() {
            _pagingOption = PagingHelper.getSetting();
            _me.submit();
        }

        function getPostData() {
            var data = $(_selector.Form).serializeObject();
            data.PageIndex = _pagingOption.pageindex;
            data.PageSize = _pagingOption.pagesize;
            return data;
        }

        function setReportTitle(fromDate, toDate, currencyName) {
            var titlePattern = "{0}, {1} → {2}, {3}";
            var title = $.format(titlePattern,
                Site.getSingleResource("CustomerBalance"),
                fromDate.toString("M/dd/yyyy"),
                toDate.toString("M/dd/yyyy"),
                Site.getSingleResource("Viewin").replace("{0}", currencyName));

            $(_selector.Title).html(title);
            $(_selector.Header).removeClass("hide");
        }

        function createPaging(totalRows) {
            _pagingOption.totalrow = totalRows;
            if (_pagingOption.totalrow > 0 && _pagingOption.totalrow > _pagingOption.pagelist[0]) {
                $(_pagingOption.elementcontainer).show();
                PagingHelper.render(_pagingOption);
            }
            else {
                $(_pagingOption.elementcontainer).hide();
            }
        }

        function sort(index) {
            if (_pagingOption.totalrow > 1) {
                _sortType = index === _sortColumnIndex ? (_sortType === "desc" ? "asc" : "desc") : "desc";
                window.setTimeout(function () {
                    var orderColumnCode = CustomerBalanceOption.getOrderColumnCode(index, _sortType);
                    if (orderColumnCode === 0) {
                        _sortColumnIndex = "";
                        _sortType = "";
                    }
                    else {
                        _sortColumnIndex = index;
                    }
                    $(_selector.OrderItemInput).val(orderColumnCode);
                    _me.submit();
                }, 100);
            }
        }

        function onGridComplete(grid, gridOption, response) {
            if (_isFirstLoad) {
                jqGridAdditional.gridComplete();
                if (_pagingOption.totalrow > 1) {
                    var reportGrid = $("#" + _selector.GridId);
                    jqGridAdditional.addSortableHeaders(reportGrid, _sortColumnIndex, _sortType);
                }

                jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
                jqGridAdditional.setGroupFooters(CustomerBalanceOption.totalColspan);
                resizeGrid();
                _isFirstLoad = false;
            }
        }

        function resizeGrid() {
            var reportGrid = $("#" + _selector.GridId);
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent,
            submit: submit,
            bindData: bindData,
            ajaxPaging: ajaxPaging,
            initSetting: initSetting
        };

        return _me;
    });