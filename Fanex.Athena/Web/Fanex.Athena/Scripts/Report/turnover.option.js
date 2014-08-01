/*global Site, jqGridAdditional*/
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
        frozen: true
    };
    var dateColumn = {
        name: 'Date',
        index: 'Date',
        width: 85,
        align: 'center',
        sortable: false,
        frozen: true,
        resizable: false
    };
    var openingBalanceColumn = {
        name: 'OpeningBalance',
        index: 'OpeningBalance',
        width: 110,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var depositColumn = {
        name: 'Deposit',
        index: 'Deposit',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var withdrawalColumn = {
        name: 'Withdrawal',
        index: 'Withdrawal',
        resizable: false,
        width: 85,
        align: 'right',
        sortable: false,
        formatter: 'currency',
        frozen: true,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var bonusColumn = {
        name: 'Bonus',
        index: 'Bonus',
        width: 60,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        frozen: true
    };
    var adjustmentColumn = {
        name: 'Adjustment',
        index: 'Adjustment',
        width: 70,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var drawGameColumn = {
        name: 'DrawGame',
        index: 'DrawGame',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var netTurnoverColumn = {
        name: 'NetTurnover',
        index: 'NetTurnover',
        width: 85,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        title: true,
        formatter: 'currency'
    };
    var pendingFundsColumn = {
        name: 'PendingFunds',
        index: 'PendingFunds',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var winLossColumn = {
        name: 'WinLoss',
        index: 'WinLoss',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var playerCommColumn = {
        name: 'PlayerComm',
        index: 'PlayerComm',
        width: 60,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var winlossCommColumn = {
        name: 'WinLossComm',
        index: 'WinLossComm',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
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
                 winLossColumn, playerCommColumn, winlossCommColumn, closingBalanceColumn,
                affiliateTransferColumn, transfer3rdColumn, turnoverBAColumn, commBAColumn, amountDueToBAColumn];
    var customerWlTotalIndex = 11;

    var me = {
        licenseeId: 0,
        hasCustomerComm: true,
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
            rowNum: 1000,
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
            userData: []
        }
    };

    me.getColName = function () {
        // Column Customer Wl
        colNameList[customerWlTotalIndex] = me.hasCustomerComm ? 'WinLoss' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    me.getColModel = function () {
        // Column hidden by customer comm
        playerCommColumn.hidden = !me.hasCustomerComm;
        winlossCommColumn.hidden = !me.hasCustomerComm;

        // Column hidden by product
        transfer3rdColumn.hidden = !TurnoverOption.hasCasinoTransfer;
        turnoverBAColumn.hidden = !TurnoverOption.isAllowBa;
        commBAColumn.hidden = !TurnoverOption.isAllowBa;
        amountDueToBAColumn.hidden = !TurnoverOption.isAllowBa;

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