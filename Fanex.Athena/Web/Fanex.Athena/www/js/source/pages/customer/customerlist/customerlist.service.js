"use strict";
define(["jquery", "common/site", "./customerlist.option", "lib/jqgrid/jquery.jqgrid.additional", "components/paging", "./customerlist.state",
    "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min", "extensions/jquery.extension"],
function ($, Site, CustomerListOption, jqGridAdditional, PagingHelper, CustomerListState) {
    var _POPUP_LIST = ["#ViewOptionList", "#update-cust-status-popup", "#LevelListContainer", "#OddsGroupListContainer"];
    var _isAllowedCustomerCommission;
    var _productAccess;
    var _ajaxRequest = null;

    var _state;

    var _me = {
        initialize: function () {
            _isAllowedCustomerCommission = $("#IsAllowedCustomerCommission").val() === "True";
            _productAccess = JSON.parse($("#ProductAccess").val());
            _state = CustomerListState.getState();
        },

        submit: function () {
            _state.selectedCustomers = {};
            _state.checkedAllRows = false;

            var data = _me.getPostData();
            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CustomerListOption.urlData),
                success: function (response) {
                    _state.cachedData = response;
                    _me.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                error: function () {
                    Site.unBlockUI();
                    alert(Site.getSingleResource("ErrorProcessingMsg"));
                }
            };

            if (_ajaxRequest !== null) {
                _ajaxRequest.abort();
            }
            _ajaxRequest = $.ajax(option);
        },

        getPostData: function () {
            var data = {};

            data.GridColumns = [];
            var $gridColumnMultiselect = $("#ui-multiselect-options-grid-columns-multiselect ul.ui-multiselect-checkboxes input");
            $gridColumnMultiselect.each(function (index, element) {
                data.GridColumns.push({
                    Name: element.name,
                    Checked: element.checked
                });
            });

            var gridColumnCount = data.GridColumns.filter(function (item) { return item.Checked; }).length;
            if (gridColumnCount === 0) {
                return;
            }

            if ($("#OddsGroups").length > 0) {
                data.OddsGroups = [];
                var $oddsGroupsMultiselect = $("#ui-multiselect-options-odds-group-multiselect .ui-multiselect-checkboxes input");
                $oddsGroupsMultiselect.each(function (index, element) {
                    data.OddsGroups.push({
                        Name: element.name,
                        Checked: element.checked
                    });
                });

                var oddsGroupCount = data.OddsGroups.filter(function (item) { return item.Checked; }).length;
                if (oddsGroupCount === 0) {
                    return;
                }
            }

            data.PageIndex = _state.pagingOption.pageindex;
            data.PageSize = _state.pagingOption.pagesize;
            data.CurrencyId = $("#CurrencyId").val();
            data.SpecialLevel = $("#special-level-list-search").val();
            data.Username = Site.asNull($("#Username").val());
            data.CurrencyName = $("#CurrencyName").val();
            data.BaFilter = $("#BAFilterOption").val();
            data.GroupCommId = $("#DdlGroupCommId").val();
            data.BaseCurrency = $("#BaseCurrency").val();
            data.CustStatus = $("#CustStatusOption").val();

            $("#excelParam").val(JSON.stringify(data));

            $("#CurrencyId").attr('data-original', data.CurrencyId);
            $("#BaseCurrency").attr('data-original', data.BaseCurrency);

            return data;
        },

        bindData: function (response, keepScrollPosition) {
            jqGridAdditional.clearGrid("ReportGrid");
            //show report title
            $(".panel-heading").removeClass("hide");

            var hasData = response && response.TotalRows > 0;
            $("div.paging").toggle(hasData);

            if (!hasData) {
                jqGridAdditional.setNoInfo();
                $("#num-of-customer").html("0 ");
                Site.unBlockUI();
                return;
            }

            var oldScrollTop = $(".ui-jqgrid-bdiv").scrollTop();
            $("#num-of-customer").html(response.TotalRows + " ");

            var pagingOption = _state.pagingOption;
            pagingOption.totalrow = parseInt(response.TotalRows);
            PagingHelper.render(pagingOption);

            var showingColumns = $("#ui-multiselect-options-grid-columns-multiselect input").serializeObject();
            var gridOption = _me.processGridOption(showingColumns);
            gridOption.data = response.Data;
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete();
            };

            // Show the grid
            var reportGrid = $("#ReportGrid");
            reportGrid.jqGrid(gridOption);
            CustomerListOption.setGroupHeaderColumns(reportGrid);

            if (_isAllowedCustomerCommission && showingColumns["Group"] !== undefined) {
                $("#jqgh_ReportGrid_Index").attr("class", "").html("<input type='checkbox' id='CheckAll' /><label for='CheckAll'></label>");
            } else {
                $("#jqgh_ReportGrid_Index").html("#");
            }

            $("#CheckAll").prop("checked", _state.checkedAllRows);
            _me.resizeGrid();

            if (keepScrollPosition === true) {
                $(".ui-jqgrid-bdiv").scrollTop(oldScrollTop);
            }

            Site.unBlockUI();

            _me.addGridScrollEventToHidePopups();
        },

        resizeGrid: function () {
            var reportGrid = $("#ReportGrid");
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        },

        addGridScrollEventToHidePopups: function () {
            // Hide popup if scroll
            $(".ui-jqgrid-bdiv").on("scroll", function () {
                $(_POPUP_LIST).each(function (index, element) {
                    var popup = $(element);
                    if (popup.is(":visible")) {
                        popup.hide();
                    }
                });
            });
        },

        processGridOption: function (showingColumns) {
            var gridOption = CustomerListOption.getGridOption(_productAccess, _isAllowedCustomerCommission);
            var indexColumn = gridOption.colModel[0];

            _me.updateIndexColumnFormatter(indexColumn, showingColumns);
            _me.toggleColumn(gridOption, showingColumns);

            return gridOption;
        },

        updateIndexColumnFormatter: function (indexColumn, showingColumns) {
            var indexFormatter = function (cellValue, options, rowObject) {
                var checkedText = $("#CheckAll").prop("checked") || _state.selectedCustomers[rowObject.CustId] !== undefined ? "checked" : "";

                return "<input type='checkbox' " + checkedText + " title='" + rowObject.CustId +
                        "' id='" + rowObject.CustId + "' data-group-comm-id='" + rowObject.GroupCommId + "' class='select-cust-checkbox check-child' />" +
                    "<label for='" + rowObject.CustId + "'></label>";
            };

            if (_isAllowedCustomerCommission && showingColumns["Group"] !== undefined) {
                indexColumn.formatter = indexFormatter;
            }
            else {
                indexColumn.formatter = undefined;
            }
        },

        toggleColumn: function (gridOption, showingColumns) {
            var colNames = gridOption.colNames;
            var colModel = gridOption.colModel;
            var collength = colModel.length;
            var colIndex = 0;
            while (colIndex < collength) {
                var col = colModel[colIndex];
                if (!col.name || col.name === "Index") {
                    colIndex++;
                    continue;
                }

                var colName = col.name.match(/^\w\wComm$/) !== null ? "Commission" : col.name;

                if (showingColumns[colName] === undefined) {
                    colModel.splice(colIndex, 1);
                    colNames.splice(colIndex, 1);
                    collength--;
                }
                else {
                    colIndex++;
                }
            }
        },

        showGridDropdownById: function (listId, $context) {
            var $popup = $("#" + listId);
            var position = $context.offset();
            var isOverFlow = position.top + $popup.outerHeight() > window.innerHeight - 15;

            var popupTop = isOverFlow ? position.top - $popup.outerHeight() - 1 :
                                        position.top + $context.outerHeight() + 1;

            $popup.css({
                top: popupTop,
                left: position.left,
                width: $context.outerWidth()
            });

            var custId = $context.data("custid");
            if ($popup.data("custid") === custId) {
                $popup.toggle();
            }
            else {
                var $customerOption = $("#cust-option-link-" + custId);
                $popup.data("custid", custId);
                $popup.data("externalid", $customerOption.data("externalid"));
                $popup.data("systemid", $customerOption.data("systemid"));
                $popup.show();
            }

            return false;
        },

        updateGridData: function (option) {
            _me.closePopup(option.popUpSelector);
            var oldValue = option.oldValue;
            var newValue = option.newValue;

            if (oldValue === newValue || !window.confirm(option.confirmMessage)) {
                return;
            }

            $.ajax({
                type: "POST",
                data: option.param,
                url: option.url,
                dataType: "json",
                success: function (data) {
                    if (data.Success) {
                        var rowIndex = (option.$context.closest("tr").attr("id") - 1) % _state.pagingOption.pagesize;
                        var dataItem = _state.cachedData.Data[rowIndex];
                        dataItem[option.dataField] = newValue;
                        _me.bindData(_state.cachedData, true);
                    }
                    else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        },

        closePopup: function (popUpSelector) {
            $(popUpSelector).fadeOut();
        }
    };

    return _me;
});