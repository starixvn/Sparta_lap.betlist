/// <reference path="/@JSense.js" />
//##
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var MobileWinLossOption = (function () {
    var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return MobileWinLoss.drillDown(' + level + ',' + currencyId + ',' + custId + ',\'' + datetime + '\');">' + cellvalue + '</a>';
    };
    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = MobileWinLossOption.licenseeId == 1 ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
    };
    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
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
    var dateColumn = {
        name: 'Date',
        index: 'Date',
        width: 55,
        align: 'center',
        valign: 'middle',
        sortable: false,
        resizable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting,
        formatter: 'date',
        formatoptions:
        {
            newformat: 'm/d/Y'
        }
    };
    var externalIdColumn = {
        name: 'ExternalId',
        index: 'ExternalId',
        width: 85,
        align: 'left',
        sortable: false,
        frozen: true,
        resizable: false
    };
    var systemIdColumn = {
        name: 'SystemId',
        index: 'SystemId',
        width: 120,
        align: 'left',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: function (cellvalue, options, rowObject) {
            return MobileWinLoss.setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
        }
    };
    var currencyColumn = {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 55,
        align: 'center',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: currencyFormatter
    };
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
        resizable: false,
        width: 100,
        align: 'right',
        sortable: false,

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
        width: 65,
        align: 'right',
        sortable: false,
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
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency',
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
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency',
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
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency',
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
        sortable: false,
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
        sortable: false,
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
        sortable: false,
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
        width: 75,
        align: 'right',
        sortable: false,
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
        sortable: false,
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

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency", 'Turnover', 'TotalTransaction', '@[Win]/@[Loss]', 'Comm', 'Total', 'LicenseeW_L', 'BA<br/>@[Commission]', '@[RacingRevenue]', '@[AmtDueToBA]', '@[AmtDueToRacing]', 'CurrencyId', 'CustomerId'];
    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn, turnoverColumn, totalTransColumn, customerWlColumn, playerCommColumn, customerWlTotalColumn, licenseeWlColumn, baCommissionColumn, racingRevenueColumn, amtDueToBaColumn, amtDueToRacingColumn, currencyIdColumn, customerIdColumn];
    var customerWlTotalIndex = 9;
    var licenseeWlIndex = 10;

    var me = {
        licenseeId: 0,
        hasCustomerComm: true,
        pageSize: 50,
        nonePagingSize: 5000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
        sites:
        {
            "1": "Wap",
            "2": "SmartPhone",
            "3": "Others"
        },
        totalColspan: [2, 2, 3],
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

    me.getColModel = function (level) {
        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;

        // Column hidden by level
        currencyColumn.formatter = level == 0 ? currencyFormatter : null;
        systemIdColumn.formatter = level == 1 ? systemIdFormatter : null;

        dateColumn.hidden = level > 0;
        externalIdColumn.hidden = level < 2;
        systemIdColumn.hidden = level == 0;

        // Column hidden by product
        var allowedBa = $("#ProductList input[data-product-code=BA]:checked").length > 0;
        var allowedRacing = $("#ProductList input[data-product-code=RB]:checked").length > 0;

        baCommissionColumn.hidden = !allowedBa;
        amtDueToBaColumn.hidden = !allowedBa;
        racingRevenueColumn.hidden = !allowedRacing;
        amtDueToRacingColumn.hidden = !allowedRacing;

        // Row span setting
        indexColumn.cellattr = level == 0 ? jqGridAdditional.rowSpanSetting : null;
        dateColumn.cellattr = level == 0 ? jqGridAdditional.rowSpanSetting : null;

        return colModelList;
    };

    me.getGridOption = function (level) {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel(level);

        return me.gridOption;
    };

    me.getTotalColModel = function (level) {
        var colModel = me.getColModel(level);

        // Not show drill down link on total link
        currencyColumn.formatter = null;

        return colModel;
    };

    return me;
})();