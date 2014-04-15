/*global Site,jqGridAdditional*/
/*exported MobileWinLossOption*/
/// <reference path="/@JSense.js" />
//##
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var MobileWinLossOption = (function () {
    "use strict";
    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
        {
            DrillDownFromDate: rowObject.Date,
            DrillDownToDate: rowObject.Date,
            DrillDownCurrencyId: rowObject.CurrencyId,
            CustomerId: rowObject.CustomerId,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        };
        return '<a class="bet-list-link" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
    };

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        valign: 'middle',
        sortable: false,
        resizable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting
    };
    var externalIdColumn = {
        name: 'ExternalId',
        index: 'ExternalId',
        width: 85,
        align: 'left',
        sortable: true,
        frozen: true,
        resizable: false
    };
    var systemIdColumn = {
        name: 'SystemId',
        index: 'SystemId',
        width: 120,
        align: 'left',
        sortable: true,
        resizable: false,
        frozen: true
    };
    var currencyColumn = {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 60,
        align: 'center',
        sortable: true,
        resizable: false,
        frozen: true
    };
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
        resizable: false,
        width: 85,
        align: 'right',
        sortable: true,
        formatter: 'currency',
        frozen: true,
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var totalTransColumn = {
        name: 'TotalTransactions',
        index: 'TotalTransactions',
        width: 95,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'integer',
        frozen: true,
        formatoptions:
        {
            thousandsSeparator: ","
        }
    };
    var customerWlColumn = {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal,
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var playerCommColumn = {
        name: 'CustomerComm',
        index: 'CustomerComm',
        width: 50,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal,
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var customerWlTotalColumn = {
        name: 'CustomerTotal',
        index: 'CustomerTotal',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal,
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var licenseeWlColumn = {
        name: 'LicenseeWl',
        classes: 'highlight',
        index: 'LicenseeWl',
        width: 85,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };
    var baCommissionColumn = {
        name: 'BaCommission',
        classes: 'highlight',
        index: 'BaCommission',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var racingRevenueColumn = {
        name: 'RacingRevenue',
        classes: 'highlight',
        index: 'RacingRevenue',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var amtDueToBaColumn = {
        name: 'AmtDueToBa',
        index: 'AmtDueToBa',
        width: 60,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var amtDueToRacingColumn = {
        name: 'AmtDueToRacing',
        index: 'AmtDueToRacing',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        }
    };
    var currencyIdColumn = {
        name: 'CurrencyId',
        index: 'CurrencyId',
        width: 0,
        align: 'center',
        sortable: false,
        resizable: false,
        hidden: true
    };
    var customerIdColumn = {
        name: 'CustomerId',
        index: 'CustomerId',
        width: 0,
        align: 'center',
        sortable: false,
        resizable: false,
        hidden: true
    };

    var colNameList = ['#', 'ExternalId', "SystemId", "Currency", 'Turnover', 'TotalTransaction', '@[Win]/@[Loss]', 
                'Comm', 'Total', 'LicenseeW_L', 'BA<br/>@[Commission]', '@[RacingRevenue]', '@[AmtDueToBA]', '@[AmtDueToRacing]', 'CurrencyId', 'CustomerId'];
    var colModelList = [indexColumn, externalIdColumn, systemIdColumn, currencyColumn, turnoverColumn,
                totalTransColumn, customerWlColumn, playerCommColumn, customerWlTotalColumn, licenseeWlColumn, 
                baCommissionColumn, racingRevenueColumn, amtDueToBaColumn, amtDueToRacingColumn, currencyIdColumn, customerIdColumn];
    var customerWlTotalIndex = 8;

    var me = {
        licenseeId: 0,
        hasCustomerComm: true,
        pageSize: 50,
        nonePagingSize: 5000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
        sites:
        {
            "1": Site.getSingleResource("Wap"),
            "2": Site.getSingleResource("SmartPhone"),
            "3": Site.getSingleResource("Others")
        },
        totalColspan: [3],
        urlData: 'api/report/MobileWinloss',
        urlExcelData: 'api/report/MobileWinloss',
        gridOption: {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            viewrecords: true,
            autowidth: true,
            forceFit: false,
            shrinkToFit: true,
            minWidth: 800,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            gridview: true,
            footerrow: true,
            userDataOnFooter: true,
            cmTemplate: { title: true }, //enable tooltip
            userData: [],

            beforeSelectRow: function () {
                return false;
            },
            onRightClickRow: function () {
                return false;
            }
        }
    };

    me.getColName = function () {
        // Column Customer Wl
        colNameList[customerWlTotalIndex] = me.hasCustomerComm ? 'Total' : 'Customer W/L';
        return Site.getResources(colNameList);
    };

    me.getColModel = function () {
        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;

        // Column hidden by product
        var allowedBa = $("#ProductList input[data-product-code=BA]:checked").length > 0;
        var allowedRacing = $("#ProductList input[data-product-code=RB]:checked").length > 0;

        baCommissionColumn.hidden = !allowedBa;
        amtDueToBaColumn.hidden = !allowedBa;
        racingRevenueColumn.hidden = !allowedRacing;
        amtDueToRacingColumn.hidden = !allowedRacing;

        // Row span setting
        indexColumn.cellattr = jqGridAdditional.rowSpanSetting;

        externalIdColumn.formatter = betlistLink;
        systemIdColumn.formatter = betlistLink;

        return colModelList;
    };

    me.getGridOption = function () {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel();

        return me.gridOption;
    };

    me.getTotalColModel = function () {
        var colModel = me.getColModel();
        return colModel;
    };

    return me;
})();