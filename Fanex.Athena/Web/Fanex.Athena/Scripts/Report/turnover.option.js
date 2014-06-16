/*global Site,jqGridAdditional*/
/*exported MobileWinLossOption*/
/// <reference path="/@JSense.js" />
//##


var TurnoverOption = (function () {
    "use strict";

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
        width: 85,
        align: 'center',
        sortable: true,
        frozen: true,
        resizable: false
    };
    var openingBalanceColumn = {
        name: 'OpeningBalance',
        index: 'OpeningBalance',
        width: 110,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal
    };
    var depositColumn = {
        name: 'Deposit',
        index: 'Deposit',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal
    };
    var withdrawalColumn = {
        name: 'Withdrawal',
        index: 'Withdrawal',
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
    var bonusColumn = {
        name: 'Bonus',
        index: 'Bonus',
        width: 50,
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
    var adjustmentColumn = {
        name: 'Adjustment',
        index: 'Adjustment',
        width: 70,
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
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
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
    var drawGameColumn = {
        name: 'DrawGame',
        index: 'DrawGame',
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
    var netTurnoverColumn = {
        name: 'NetTurnover',
        index: 'NetTurnover',
        width: 85,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };
    var pendingFundsColumn = {
        name: 'PendingFunds',
        index: 'PendingFunds',
        width: 90,
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
    var winLossColumn = {
        name: 'WinLoss',
        index: 'WinLoss',
        width: 90,
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
    var playerCommColumn = {
        name: 'PlayerComm',
        index: 'PlayerComm',
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
    var closingBalanceColumn = {
        name: 'ClosingBalance',
        index: 'ClosingBalance',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var affiliateTransferColumn = {
        name: 'AffiliateTransfer',
        index: 'AffiliateTransfer',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };

    var transfer3rdColumn = {
        name: 'Transfer3rd',
        index: 'Transfer3rd',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var turnoverBAColumn = {
        name: 'TurnoverBA',
        index: 'TurnoverBA',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var commBAColumn = {
        name: 'CommBA',
        index: 'CommBA',
        width: 60,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var amountDueToBAColumn = {
        name: 'AmountDueToBA',
        index: 'AmountDueToBA',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };

    var colNameList = ['#', 'Day', "OpeningBalance", "Deposit", 'Withdrawal', 'Bonus', 'Adjustment',
                'Turnover', 'DrawGame', 'NetTurnover', 'PendingFunds',
                'WinLoss', 'Comm', 'Total', 'ClosingBalance',
                 'AffiliateTransfer', 'CasinoTransfer', 'Turnover', 'Comm', 'AmountDueTo'];
    var colModelList = [indexColumn, dateColumn, openingBalanceColumn, depositColumn, withdrawalColumn, bonusColumn, adjustmentColumn,
                 turnoverColumn, drawGameColumn, netTurnoverColumn, pendingFundsColumn,
                 winLossColumn, playerCommColumn, amtDueToRacingColumn, closingBalanceColumn,
                affiliateTransferColumn, transfer3rdColumn, turnoverBAColumn, commBAColumn, amountDueToBAColumn];
    var customerWlTotalIndex = 11;

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
        urlData: 'report/Turnover',
        urlExcelData: 'report/Turnover',
        gridOption: {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            viewrecords: true,
            //  autowidth: true,
            forceFit: false,
            //   shrinkToFit: true,
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
        colNameList[customerWlTotalIndex] = me.hasCustomerComm ? 'WinLoss' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    me.getColModel = function () {
        // Column hidden by customer comm
        //     adjustmentColumn.hidden = !me.hasCustomerComm;
        //     turnoverColumn.hidden = !me.hasCustomerComm;

        // Column hidden by product

        transfer3rdColumn.hidden = !TurnoverOption.hasCasinoTransfer;
        turnoverBAColumn.hidden = !TurnoverOption.isAllowBa;
        commBAColumn.hidden = !TurnoverOption.isAllowBa;
        amountDueToBAColumn.hidden = !TurnoverOption.isAllowBa;

        /*     var allowedBa = $("#ProductList input[data-product-code=BA]:checked").length > 0;
        var allowedRacing = $("#ProductList input[data-product-code=RB]:checked").length > 0;

        pendingFundsColumn.hidden = !allowedBa;
        playerCommColumn.hidden = !allowedBa;
        winLossColumn.hidden = !allowedRacing;
        amtDueToRacingColumn.hidden = !allowedRacing;

        // Row span setting
        indexColumn.cellattr = jqGridAdditional.rowSpanSetting;
        */
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