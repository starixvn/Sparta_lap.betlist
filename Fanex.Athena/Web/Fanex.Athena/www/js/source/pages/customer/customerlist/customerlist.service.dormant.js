"use strict";
define(["jquery", "common/site", "./customerlist.gridoption.dormant", "lib/jqgrid/jquery.jqgrid.additional",
    "components/paging", "./customerlist.state", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min", "extensions/jquery.extension"],
function ($, Site, CustomerListOptionDormant, jqGridAdditional, PagingHelper, CustomerListState) {
    var _ajaxRequest = null;

    var _state;
    
    var _me = {
        initialize: function () {
            _state = CustomerListState.getDormantState();
        },

        submit: function () {
            
            var data = _me.getPostData();
            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CustomerListOptionDormant.urlData),
                success: function (response) {
                    _me.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                error: function () {
                    Site.unBlockUI();
                }
            };

            if (_ajaxRequest !== null) {
                _ajaxRequest.abort();
            }
            _ajaxRequest = $.ajax(option);
        },

        getPostData: function () {
            var data = {};

            data.PageIndex = _state.pagingOption.pageindex;
            data.PageSize = _state.pagingOption.pagesize;
            data.CurrencyId = $("#CurrencyId").val();
            data.Username = Site.asNull($("#Username").val());
            data.CurrencyName = $("#CurrencyName").val();
            data.BaseCurrency = $("#BaseCurrency").val();

            $("#excelDormantParam").val(JSON.stringify(data));

            $("#CurrencyId").attr('data-original', data.CurrencyId);
            $("#BaseCurrency").attr('data-original', data.BaseCurrency);

            return data;
        },

        bindData: function (gridData) {
            var width = 708;
            var $container = $("#tabDormantCustomer");
            jqGridAdditional.clearGrid("DormantCustomerGrid");

            var hasData = gridData && gridData.TotalRows > 0;
            $("#DormantCustomerPager").toggle(hasData);

            if (!hasData) {
                jqGridAdditional.setNoInfo();
                $container.find(".panel-heading").width(width);
                $container.find("#noInfoRow").width(width - 10);
                $("#num-of-dormant-customer").html("0 ");
                Site.unBlockUI();
                return;
            }

            $("#num-of-dormant-customer").html(gridData.TotalRows + " ");

            var pagingOption = _state.pagingOption;
            pagingOption.totalrow = parseInt(gridData.TotalRows);
            PagingHelper.render(pagingOption);

            var $reportGrid = $("#DormantCustomerGrid");
            
            var gridOption = CustomerListOptionDormant.getGridOption();
            gridOption.data = gridData.Data;
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete($container);
            };

            // Show the grid
            $reportGrid.jqGrid(gridOption);

            jqGridAdditional.updateGridHeight($container);
            
            $("#DormantCustomerGrid").jqGrid('setGridWidth', width);
            $container.find(".panel-heading").width(width)
            $("#DormantCustomerPager").width(width);
            if ($.active === 1) {
                Site.unBlockUI();
            }
        },

        resizeGrid: function (gridSelector) {
            var $reportGrid = $(gridSelector);
            jqGridAdditional.updateGridHeight($reportGrid.closest(".theme_jqGrid"));
            jqGridAdditional.updateGridWidth($reportGrid);
        }
    };

    return _me;
});