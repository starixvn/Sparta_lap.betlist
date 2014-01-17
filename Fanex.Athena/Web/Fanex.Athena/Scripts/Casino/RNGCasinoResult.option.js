/// <reference path="/@JSense.js" />
//##
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var RNGCasinoOption = (function () {
    var historyDetailLink = function (cellvalue, options, rowObject) {
        var currencyId = $("#casino-detail-currency-id").val();
        var currencyName = $("#casino-detail-currency-name").val();
        var baseCurrency = $("#casino-detail-base-currency").val();
        var drillDownCur = currencyId == 0 || baseCurrency != "" ? baseCurrency : currencyName;
        var url = Site.resolveClientUrl("HandHistory/RNGCasinoDetail");
        url = QueryString.setParam(url, "userBetId", rowObject.UserBetId);
        url = QueryString.setParam(url, "gameTypeId", rowObject.GameTypeId);
        url = QueryString.setParam(url, "gameTypeName", rowObject.GameTypeName);
        url = QueryString.setParam(url, "currencyId", currencyId);
        url = QueryString.setParam(url, "currencyName", drillDownCur);
        url = QueryString.setParam(url, "playerHistoryNo", rowObject.PlayerHistoryNo);

        return '<a class="hand-history-detail-link" data-url="' + url + '" href="javascript:;" >' + cellvalue + '</a>';
    };

    var sessionColumn = {
        name: 'SessionLogId',
        index: 'SessionLogId',
        width: 70,
        align: 'center',
        valign: 'middle',
        sortable: false,
        resizable: false,
        frozen: true
    };
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
    var gameIdColumn = {
        name: 'GameId',
        index: 'GameId',
        width: 80,
        align: 'left',
        sortable: false,
        frozen: true,
        resizable: false
    };
    var playHistoryNoColumn = {
        name: 'PlayerHistoryNo',
        index: 'PlayerHistoryNo',
        width: 90,
        align: 'left',
        sortable: false,
        resizable: false,
        frozen: true
    };
    var gameTypeNameColumn = {
        name: 'GameTypeName',
        index: 'GameTypeName',
        width: 100,
        align: 'left',
        sortable: false,
        resizable: false,
        frozen: true
    };
    var gameGroupNameColumn = {
        name: 'GameGroupName',
        index: 'GameGroupName',
        width: 70,
        align: 'left',
        sortable: false,
        resizable: false,
        frozen: true
    };
    var startTimeColumn = {
        name: 'StartTime',
        index: 'StartTime',
        width: 70,
        align: 'center',
        sortable: false,
        resizable: false,
        frozen: true
    };
    var endTimeColumn = {
        name: 'EndTime',
        index: 'EndTime',
        width: 70,
        align: 'center',
        sortable: false,
        resizable: false,
        frozen: true
    };
    var beginBalanceColumn = {
        name: 'BeginBalance',
        index: 'BeginBalance',
        width: 70,
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
    var endBlanceColumn = {
        name: 'EndBlance',
        index: 'EndBlance',
        width: 70,
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
        },
        summaryType: 'count',
        summaryTpl: '<b>' + Site.getSingleResource('Total') + '</b>'
    };
    var stakeColumn = {
        name: 'Stake',
        index: 'Stake',
        width: 40,
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
        },
        summaryType: 'sum'
    };
    var betColumn = {
        name: 'Bet',
        index: 'Bet',
        width: 40,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        frozen: true,
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        },
        summaryType: 'sum'
    };
    var winLossColumn = {
        name: 'WinLoss',
        index: 'WinLoss',
        width: 50,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        title: true,
        formatter: 'currency',
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        },
        summaryType: 'sum'
    };
    var rollingCommColumn = {
        name: 'RollingComm',
        index: 'RollingComm',
        width: 40,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        title: true,
        formatter: 'currency',
        formatoptions:
        {
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2
        },
        summaryType: 'sum'
    };
    var resultColumn = {
        name: 'Result',
        index: 'Result',
        width: 0,
        align: 'left',
        sortable: false,
        resizable: false
    };

    var colNameList = ['SessionLogId', '#', 'GameId', "PlayerHistoryNo", "GameType", 'GameGroup', 'StartDate', 'EndDate', 'StartBalance', 'EndBalance', 'Stake', 'Bet', '@[Win]/@[Loss]', 'RollingComm', 'Results'];
    var colModelList = [sessionColumn, indexColumn, gameIdColumn, playHistoryNoColumn, gameTypeNameColumn, gameGroupNameColumn, startTimeColumn, endTimeColumn, beginBalanceColumn, endBlanceColumn, stakeColumn, betColumn, winLossColumn, rollingCommColumn, resultColumn];
    var customerWlTotalIndex = 8;
    var licenseeWlIndex = 9;

    var me = {
        licenseeId: 0,
        hasCustomerComm: true,
        pageSize: 500,
        totalColspan: [9],
        urlData: 'HandHistory/RNGCasinoData',
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
            rowList: [100, 200, 300, 400, 500],
            rowNum: 500,
            minWidth: 800,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            gridview: true,
            sortname: 'SessionLogId',
            grouping: true,
            groupingView: {
                groupField: ['SessionLogId'],
                groupColumnShow: [false],
                groupText: ['<b>' + Site.getSingleResource('SessionID') + ': {0}</b>'],
                groupSummary: [true],
                groupOrder: ['desc'],
                groupCollapse: false,
                showSummaryOnHide: true
            },
            footerrow: true,
            cmTemplate: { title: true },
            userDataOnFooter: true,
            userData: [],

            beforeSelectRow: function () {
                return false;
            },
            onRightClickRow: function () {
                return false;
            }
        },
        maxHeight: 500
    };

    me.getColName = function () {
        return Site.getResources(colNameList);
    };

    me.getColModel = function () {
        // Show/Hide RollingComm column if licensee Offer/Not Customer commission.
        rollingCommColumn.hidden = !me.hasCustomerComm;

        resultColumn.formatter = historyDetailLink;
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