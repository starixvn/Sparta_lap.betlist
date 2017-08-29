define([
    "jquery",
    "common/querystring",
    "common/site",
    "../report",
    "./customerwinloss.option",
    "lib/jqgrid/jquery.jqgrid.additional",
    "components/pluginhelper/daterangepicker",
    "components/pluginhelper/monthpicker",
    "components/pluginhelper/multiselect",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, QueryString, Site, Report, CustomerWinLossOption, jqGridAdditional, DatePicker, MonthPicker, MultiSelect) {
        "use strict";

        var _ajaxRequest = null;
        var _submitData = null;

        var _sortColumnName = "";
        var _sortOrder = "desc";
        var _isFirstLoad = false;

        var _pagingOption =
        {
            pageindex: 1,
            pagesize: 1000,
            pagelist: [1000, 2000],
            totalrow: 1,
            elementcontainer: ".paging",
            method: "AJAX",
            ajaxfunction: "CustomerWinLoss.ajaxPaging()",
            alwayshow: true
        };

        var _selector = {
            SubmitButton: "#SubmitButton",
            ExportToExcel: "#ExportToExcel",
            Form: "#CustomerWinlossForm",
            Title: ".report-title",
            Header: ".panel-heading",
            GridId: "ReportGrid",
            IsHistoryInput: "#IsHistory",
            IsAllowedBAInput: "#IsAllowedBA",
            IsAllowedRBInput: "#IsAllowedRB",
            IsAllowedCustomerCommissionInput: "#IsAllowedCustomerCommission",
            HistoryLink: "#history-report-link",
            CurrentLink: "#current-report-link"
        };

        function initialize() {
            Site.initialize();

            Site.addUserGuide({
                resourceName: "CustomerWL",
                title: Site.getSingleResource("CustomerWinLoss")
            });
            MultiSelect.initProductList();
            DatePicker.initDateRangePicker();
            if ($(_selector.IsHistoryInput).val() === "True") {
                MonthPicker.initMonthPicker();
            }

            _me.initEvent();
            _me.initSetting();
        }

        function initEvent() {
            $(_selector.SubmitButton).click(function () {
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return;
                }

                resetParam();
                _me.submit();
            });

            $(_selector.ExportToExcel).click(function () {
                var data = getPostData();
                data.PageIndex = 1;
                Site.exportExcel(data, Site.resolveClientUrl(DATA_URL));
                return false;
            });

            $(_selector.HistoryLink).click(function () {
                _me.viewHistoryReport();
            });

            $(_selector.CurrentLink).click(function () {
                _me.viewCurrentReport();
            });
        }

        function initSetting() {
            CustomerWinLossOption.isAllowBA = Site.asBool($(_selector.IsAllowedBAInput).val());
            CustomerWinLossOption.isAllowRB = Site.asBool($(_selector.IsAllowedRBInput).val());
            CustomerWinLossOption.hasCustomerComm = Site.asBool($(_selector.IsAllowedCustomerCommissionInput).val());
        }

        function submit() {

        }

        function viewHistoryReport() {
            Site.blockUI();
            window.location.href = Site.resolveClientUrl("CustomerWinlossReport/History");
        }

        function viewCurrentReport() {
            Site.blockUI();
            window.location.href = Site.resolveClientUrl("CustomerWinLossReport/Index");
        }

        function buildPostData() {
            if (!_submitData) {
                _submitData = getPostData();
            }
            else {
                _submitData.PageIndex = _pagingOption.pageindex;
                _submitData.PageSize = _pagingOption.pagesize;
                _submitData.sortColumn = _sortColumnName;
                _submitData.SortDirection = _sortOrder;
            }
        }

        function resetParam() {
            _sortColumnName = "ExternalId";
            _sortOrder = "asc";
            _pagingOption.pageindex = 1;
            _submitData = null;
        }

        function getPostData() {
            var param = $(FORM_SELECTOR).serializeObject();
            param.PageIndex = _pagingOption.pageindex;
            param.PageSize = _pagingOption.pagesize;
            param.sortColumn = _sortColumnName;
            param.SortDirection = _sortOrder;

            return param;
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent,
            initSetting: initSetting,
            submit: submit,
            viewHistoryReport: viewHistoryReport,
            viewCurrentReport: viewCurrentReport
        }

        return _me;
    });