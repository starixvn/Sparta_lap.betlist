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
    "common/popup",
    "components/paging",
    "helpers/excel",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, QueryString, Site, Report, CustomerWinLossOption, jqGridAdditional, DatePicker, MonthPicker, MultiSelect, Popup, PagingHelper, ExcelHelper) {
        "use strict";

        var _ajaxRequest = null;
        var _submitData = null;

        var _sortColumnName = "";
        var _sortOrder = "desc";
        var _isFirstLoad = false;

        var _pagingOption =
        {
            pageindex: 1,
            pagesize: 5,
            pagelist: [5, 2000],
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
            CurrentLink: "#current-report-link",
            BetListLink: ".bet-list-link"
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
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return;
                }

                var data = getPostData();
                data.PageIndex = 1;
                ExcelHelper.exportExcel(data, Site.resolveClientUrl(CustomerWinLossOption.urlExcelData));
                return false;
            });

            $(_selector.HistoryLink).click(function () {
                _me.viewHistoryReport();
            });

            $(_selector.CurrentLink).click(function () {
                _me.viewCurrentReport();
            });

            window.CustomerWinLoss = {
                ajaxPaging: _me.ajaxPaging
            };

            $(".main-content").on("click", _selector.BetListLink, function () {
                var data = $(this).data("drill-down");
                _me.viewWinlossBetList(data.CustId, data.SystemId, data.ExternalId);
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        }

        function initSetting() {
            CustomerWinLossOption.isAllowBA = Site.asBool($(_selector.IsAllowedBAInput).val());
            CustomerWinLossOption.isAllowRB = Site.asBool($(_selector.IsAllowedRBInput).val());
            CustomerWinLossOption.hasCustomerComm = Site.asBool($(_selector.IsAllowedCustomerCommissionInput).val());
        }

        function submit() {
            Report.setOriginalProduct();

            buildPostData();

            var option =
            {
                type: "POST",
                data: _submitData,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CustomerWinLossOption.urlData),
                success: function (response) {
                    setReportTitle();
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

            var gridOption = CustomerWinLossOption.getGridOption(response.TotalRows);
            gridOption.data = response.Data;
            gridOption.onSortCol = function (index) {
                sort(index);
                return "stop";
            };
            gridOption.gridComplete = function () {
                onGridComplete(this, gridOption, response);
                Site.unBlockUI();
            };
            gridOption.scroll = response.TotalRows > gridOption.rowNum ? 1 : false;

            var reportGrid = $("#" + _selector.GridId);
            reportGrid.jqGrid(gridOption);

            CustomerWinLossOption.setGroupHeaderColumns(reportGrid);

        }

        function ajaxPaging() {
            _pagingOption = PagingHelper.getSetting();
            _me.submit();
        }

        function viewHistoryReport() {
            Site.blockUI();
            Site.redirect(Site.resolveClientUrl("CustomerWinlossReport/History"));
        }

        function viewCurrentReport() {
            Site.blockUI();
            Site.redirect(Site.resolveClientUrl("CustomerWinLossReport/Index"));
        }

        function viewWinlossBetList(custId, systemId, externalId) {
            var url = Site.resolveClientUrl("CustomerWinlossBetList/Index");
            url = QueryString.setParam(url, "ProductCodeList", getProductList());
            url = QueryString.setParam(url, "currencyid", _submitData.CurrencyId);
            url = QueryString.setParam(url, "CurrencyName", _submitData.CurrencyName);
            url = QueryString.setParam(url, "BaseCurrency", _submitData.BaseCurrency);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "fromdate", _submitData.FromDate);
            url = QueryString.setParam(url, "todate", _submitData.ToDate);
            url = QueryString.setParam(url, "ishistoryreport", $('input#IsHistory').val());

            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var popupOption = { id: "betlist-popup", width: popW + 50, height: popH + 50, title: "Customer Bet List" };
            Popup.openPopupWithIframe(url, popupOption);
        }

        function setReportTitle() {
            var titlePattern = "{0}, {1} → {2}, {3}{4}";
            var licenseeWLNote = getLicenseeWLNote(_submitData.ProductOptions);
            var title = $.format(titlePattern,
                Site.getSingleResource("CustomerWinLoss"),
                _submitData.FromDate,
                _submitData.ToDate,
                Site.getSingleResource("Viewin").replace("{0}",
                _submitData.BaseCurrency === "" ? _submitData.CurrencyName : _submitData.BaseCurrency.toUpperCase()),
                licenseeWLNote === "" ? "" : ", " + licenseeWLNote);

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
                setTimeout(function () {
                    if (_sortColumnName === index) {
                        _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        _sortOrder = "asc";
                        _sortColumnName = index;
                    }
                    _me.submit();
                }, 100);
            }
        }

        function onGridComplete(grid, gridOption, response) {
            if (_isFirstLoad) {
                jqGridAdditional.gridComplete();

                var reportGrid = $("#" + _selector.GridId);
                if (_pagingOption.totalrow > 1) {
                    jqGridAdditional.addSortableHeaders(reportGrid, _sortColumnName, _sortOrder);
                }
                else {
                    jqGridAdditional.removeCursorOnNonSortableHeader(reportGrid);
                }

                jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
                jqGridAdditional.setGroupFooters(CustomerWinLossOption.totalColspan);
                resizeGrid();
                _isFirstLoad = false;
            }
        }

        function getLicenseeWLNote() {
            var resultProducts = getProductList();
            if (resultProducts.length > 0) {
                return Site.getSingleResource("LicenseeW_L") + "(" + resultProducts.join(', ') + ")";
            }
            return "";
        }

        function getProductList() {
            var productCodeList = [];
            $(".childCheckbox", "#ProductList").each(function (index, element) {
                var productCode = $(element).attr('data-product-code');
                if (productCode && $(element).attr("data-original") === 'true') {
                    productCodeList.push(productCode.toUpperCase());
                }
            });

            return productCodeList;
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
            var param = $(_selector.Form).serializeObject();
            param.PageIndex = _pagingOption.pageindex;
            param.PageSize = _pagingOption.pagesize;
            param.sortColumn = _sortColumnName;
            param.SortDirection = _sortOrder;

            return param;
        }

        function resizeGrid() {
            var reportGrid = $("#" + _selector.GridId);
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent,
            initSetting: initSetting,
            submit: submit,
            viewHistoryReport: viewHistoryReport,
            viewCurrentReport: viewCurrentReport,
            viewWinlossBetList: viewWinlossBetList,
            ajaxPaging: ajaxPaging,
            bindData: bindData
        }

        return _me;
    });