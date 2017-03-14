define(
    ["jquery", "common/querystring", "common/site", "lib/jqgrid/jquery.jqgrid.additional"],
    function ($, QueryString, Site, jqGridAdditional) {
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
        width: 100,
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
        width: 100,
        align: 'right',
        sortable: false,
        formatter: 'currency',
        frozen: true,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var bonusColumn = {
        name: 'Bonus',
        index: 'Bonus',
        width: 85,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        frozen: true
    };
    var adjustmentColumn = {
        name: 'Adjustment',
        index: 'Adjustment',
        width: 75,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
        width: 120,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var drawGameColumn = {
        name: 'DrawGame',
        index: 'DrawGame',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: 'currency'
    };
    var netTurnoverColumn = {
        name: 'NetTurnover',
        index: 'NetTurnover',
        width: 120,
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
        width: 70,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency'
    };
    var winLossColumn = {
        name: 'WinLoss',
        index: 'WinLoss',
        width: 100,
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
        width: 100,
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
        width: 70,
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

    var _me = {
        licenseeId: 0,
        hasCustomerComm: true,
        totalColspan: [3],
        urlData: 'reportapi/Turnover',
        urlExcelData: 'TurnoverReport/Excel',
        gridOption: {
            rowNum: 1000,
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            viewrecords: true,
            autowidth: false,
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
            userData: []
        }
    };

    _me.getColName = function () {
        // Column Customer Wl
        colNameList[customerWlTotalIndex] = _me.hasCustomerComm ? 'WinLoss' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    _me.getColModel = function () {
        // Column hidden by customer comm
        playerCommColumn.hidden = !_me.hasCustomerComm;
        winlossCommColumn.hidden = !_me.hasCustomerComm;

        // Column hidden by product
        transfer3rdColumn.hidden = !_me.hasCasinoTransfer;
        turnoverBAColumn.hidden = !_me.isAllowBa;
        commBAColumn.hidden = !_me.isAllowBa;
        amountDueToBAColumn.hidden = !_me.isAllowBa;

        return colModelList;
    };

    _me.getGridOption = function () {
        _me.gridOption.colNames = _me.getColName();
        _me.gridOption.colModel = _me.getColModel();
        return _me.gridOption;
    };

    _me.getTotalColModel = function () {
        var colModel = _me.getColModel();
        return colModel;
    };

    return _me;
});