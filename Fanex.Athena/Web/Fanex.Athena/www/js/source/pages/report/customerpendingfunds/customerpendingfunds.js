define([
    "jquery",
    "common/querystring",
    "common/site",
    "lib/jqgrid/jquery.jqgrid.additional",
    "./customerpendingfunds.option",
    "components/pluginhelper/multiselect",
    "components/paging",
    "common/popup",
    "../report",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, QueryString, Site, jqGridAdditional, CustomerPedingFundsOption, MultiSelect, PagingHelper, Popup, Report) {
        "use strict";

        var _ajaxRequest = null;
        var _sortDirection = "";
        var _sortColumn = "";
        var _isFirstLoad = true;
        var _submitData = null;
        var _pagingOption =
        {
            pageindex: 1,
            pagesize: 1000,
            pagelist: [1000, 2000],
            totalrow: 1,
            elementcontainer: ".paging",
            method: "AJAX",
            ajaxfunction: "CustPendingFunds.ajaxPaging()",
            alwayshow: true
        };
        var _selector = {
            SubmitButton: "#SubmitButton",
            ExportExcelInput: "#IsExportExcel",
            ExportToExcel: "#ExportToExcel",
            Form: "#CustomerPendingFundsForm",
            GridId: "ReportGrid",
            SortColumnInput: "#SortColumn",
            SortDirectionInput: "#SortDirection",
            BetListLink: ".bet-list-link"
        };

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "CustomerPendingFunds",
                title: Site.getSingleResource("CustomerPedingFunds")
            });
            MultiSelect.initProductList();

            _me.initEvent();
            _me.initSetting();
        }

        function initEvent() {
            $(_selector.SubmitButton).click(function () {
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return;
                }

                _pagingOption.pageindex = 1;
                _submitData = null;
                _me.submit();
            });

            $(_selector.ExportToExcel).click(function () {
                $(_selector.ExportExcelInput).val(true);
                var mainForm = $(_selector.Form);
                var url = Site.resolveClientUrl(CustomerPedingFundsOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(_selector.ExportExcelInput).val(false);
                return false;
            });

            $(".main-content").on("click", _selector.BetListLink, function () {
                var data = $(this).data("drill-down");
                _me.viewPendingFundsBetList(data.CustId, data.SystemId, data.ExternalId);
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });

            window.CustPendingFunds = {
                ajaxPaging: _me.ajaxPaging
            };
        }

        function initSetting() {
            _sortColumn = $(_selector.SortColumnInput).val();
            _sortDirection = $(_selector.SortDirectionInput).val();
        }

        function submit() {
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            buildPostData();

            var option =
            {
                type: "POST",
                data: _submitData,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CustomerPedingFundsOption.urlData),
                success: function (response) {
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

            var gridOption = CustomerPedingFundsOption.getGridOption(response.TotalRows);
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
        }

        function ajaxPaging() {
            _pagingOption = PagingHelper.getSetting();
            _me.submit();
        }

        function viewPendingFundsBetList(custid, systemId, externalId) {
            var url = Site.resolveClientUrl("PendingFundsBetList/Index");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = Site.setProductOptionOriginalParamList(url);
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);

            var title = Site.getSingleResource("BetListRunning");
            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var popupOption = { id: "betlist-popup", width: popW + 50, height: popH + 50, title: title };
            Popup.openPopupWithIframe(url, popupOption);
        }

        function buildPostData() {
            if (!_submitData) {
                _submitData = $(_selector.Form).serializeObject();
            }
            _submitData.PageIndex = _pagingOption.pageindex;
            _submitData.PageSize = _pagingOption.pagesize;
            _submitData.SortColumn = _sortColumn;
            _submitData.SortDirection = _sortDirection;
        }

        function createPaging(totalRows) {
            _pagingOption.totalrow = totalRows;
            if (_pagingOption.totalrow > _pagingOption.pagelist[0]) {
                $(_pagingOption.elementcontainer).show();
                PagingHelper.render(_pagingOption);
            }
            else {
                $(_pagingOption.elementcontainer).hide();
            }
        }

        function sort(index) {
            if (_pagingOption.totalrow > 1) {
                _sortDirection = index === _sortColumn ? (_sortDirection === "asc" ? "desc" : "asc") : "asc";
                _sortColumn = index;
                window.setTimeout(function () {
                    $(_selector.SortColumnInput).val(_sortColumn);
                    $(_selector.SortDirectionInput).val(_sortDirection);
                    _me.submit();
                }, 100);
            }
        }

        function onGridComplete(grid, gridOption, response) {
            if (_isFirstLoad) {
                jqGridAdditional.gridComplete();

                var reportGrid = $("#" + _selector.GridId);
                if (_pagingOption.totalrow > 1) {
                    jqGridAdditional.addSortableHeaders(reportGrid, _sortColumn, _sortDirection);
                }
                else {
                    jqGridAdditional.removeCursorOnNonSortableHeader(reportGrid);
                }

                jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
                jqGridAdditional.setGroupFooters(CustomerPedingFundsOption.totalColspan);
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
            initSetting: initSetting,
            viewPendingFundsBetList: viewPendingFundsBetList
        };

        return _me;
    });