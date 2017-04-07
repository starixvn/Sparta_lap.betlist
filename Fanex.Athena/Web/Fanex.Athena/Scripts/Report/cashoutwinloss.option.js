/*global Site, jqGridAdditional, JSLINQ*/
/*exported CashOutWinLossOption*/
/// <reference path="/@JSense.js" />

var CashOutWinLossOption = (function () {
    "use strict";

    var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return CashOutWinLoss.drillDown(' + level + ',' +
         currencyId + ',' + custId + ',\'' + datetime + '\');">' + cellvalue + '</a>';
    };

    var dateFormatter = function (cellvalue, options, rowObject) {
        cellvalue = $.fn.fmatter.call(this, "date", cellvalue, options, rowObject);
        return setDrillDownLink(1, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
    };
    var currencyFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(1, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
    };

    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
        {
            DrillDownDate: rowObject.Date,
            DrillDownCurrencyId: rowObject.CurrencyId,
            DrillDownCurrencyName: rowObject.CurrencyName,
            CustomerId: rowObject.CustomerId,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        };

        return '<a class="bet-list-link" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
    };

    //#region Columns

    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        resizable: false,
        width: 30,
        align: 'center',
        valign: 'middle',
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting,
        sortable: false
    };

    var dateColumn =
    {
        name: 'Date',
        index: 'Date',
        resizable: false,
        width: 55,
        align: 'center',
        valign: 'middle',
        sortable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting,
        formatter: dateFormatter,
        formatoptions:
        {
            newformat: 'm/d/Y'
        }
    };

    var externalIdColumn =
    {
        name: 'ExternalId',
        index: 'ExternalId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: false,
        formatter: betlistLink
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: false,
        formatter: betlistLink
    };

    var currencyColumn =
    {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 60,
        align: 'center',
        sortable: false,
        resizable: false
    };

    var cashOutTimesColumn =
    {
        name: 'CashOutTimes',
        index: 'CashOutTimes',
        width: 75,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'integer',
        hidden: false
    };

    var cashOutTicketsColumn =
    {
        name: 'CashOutTickets',
        index: 'CashOutTickets',
        width: 75,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'integer',
        frozen: true
    };

    var cashOutValueColumn =
    {
        name: 'CashOutValue',
        index: 'CashOutValue',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var buyBackAmoutSBColumn =
    {
        name: 'BuyBackAmountSB',
        index: 'BuyBackAmountSB',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var buyBackAmoutBAColumn =
    {
        name: 'BuyBackAmountBA',
        index: 'BuyBackAmountBA',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var buyBackAmoutTotalColumn =
    {
        name: 'TotalBuyBackAmount',
        index: 'TotalBuyBackAmount',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var ticketPayoutColumn =
    {
        name: 'TicketPayout',
        index: 'TicketPayout',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var cashOutCommissionColumn =
    {
        name: 'CashOutCommission',
        index: 'CashOutCommission',
        classes: 'highlight',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var settleAmountTicketPayoutColumn =
    {
        name: 'SettleAmountTicketPayout',
        index: 'SettleAmountTicketPayout',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var settleAmountBuyBackCommColumn =
    {
        name: 'SettleAmountBuyBackComm',
        index: 'SettleAmountBuyBackComm',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var settleAmountTotalColumn =
    {
        name: 'SettleAmountTotal',
        index: 'SettleAmountTotal',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var currencyIdColumn =
    {
        name: 'CurrencyId',
        index: 'CurrencyId',
        width: 2,
        align: 'center',
        sortable: false,
        resizable: false,
        hidden: true
    };

    var customerIdColumn =
    {
        name: 'CustomerId',
        index: 'CustomerId',
        width: 2,
        align: 'center',
        sortable: false,
        resizable: false,
        hidden: true
    };

    //#endregion

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
      'CashOutTimes', 'CashOutTickets', 'CashOutValue', 'ProductSB', 'ProductBA', 'Total', 'TicketPayout', 'CashOutCommission',
      'TicketPayout', 'BBAmtComm', 'Total', 'CurrencyId', 'CustomerId'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn, cashOutTimesColumn, cashOutTicketsColumn, cashOutValueColumn,
        buyBackAmoutSBColumn, buyBackAmoutBAColumn, buyBackAmoutTotalColumn, ticketPayoutColumn, cashOutCommissionColumn, 
        settleAmountTicketPayoutColumn, settleAmountBuyBackCommColumn, settleAmountTotalColumn, currencyIdColumn, customerIdColumn];

    var me =
    {
        isLicenseeOfferBA: false,
        buyBackAmountTotalColumnIndex: 10,
        totalColspan: [2, 3],
        urlData: 'api/CashOutWinLossReport/Index',
        urlExcelData: 'api/CashOutWinLossReport/Index',
        gridOption:
        {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            userDataOnFooter: true,
            userData: [],
            height: '100%',
            autowidth: true,
            forceFit: false,
            shrinkToFit: true,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            rowNum: 2000,
            grouping: true,
            viewrecords: true,
            gridview: true,
            pager: '#ppdata',
            rownumbers: true
        }
    };

    me.setGroupHeaders = function ($grid) {
        var groupBuyBackAmount = me.isLicenseeOfferBA ?
            {
                startColumnName: "BuyBackAmountSB",
                numberOfColumns: 3,
                titleText: Site.getSingleResource('BuyBackAmount')
            } : {};
        var groupSettleAmount =
            {
                startColumnName: "SettleAmountTicketPayout",
                numberOfColumns: 3,
                titleText: Site.getSingleResource('SettleAmount')
            };
        $grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [groupBuyBackAmount, groupSettleAmount]
        });
    };
    me.getColName = function () {
        var columns = Site.clone(colNameList);
        if (!me.isLicenseeOfferBA)
        {
            columns[me.buyBackAmountTotalColumnIndex] = "BuyBackAmount";
        }

        return Site.getResources(columns);
    };

    me.getColModel = function (level) {
        level = Number(level);

        currencyColumn.formatter = level === 0 ? currencyFormatter : null;
        
        dateColumn.hidden = level > 0;
        dateColumn.formatoptions =
        {
            newformat: $('#reportbydaily').is(':checked') ? 'm/d/Y' : 'm/Y'
        };
        externalIdColumn.hidden = level === 0;
        systemIdColumn.hidden = level === 0;
        if (!me.isLicenseeOfferBA)
        {
            buyBackAmoutSBColumn.hidden = true;
            buyBackAmoutBAColumn.hidden = true;
        }
        
        var temp = Site.clone(colModelList);

        return temp;
    };

    me.getGridOption = function (level) {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel(level);
        return Site.clone(me.gridOption);
    };

    me.getTotalColModel = function (level) {
        var colModel = me.getColModel(level);
        // Not show drill down link on total link
        var curCol = JSLINQ(colModel).First(function (item) {
            return item.name === "CurrencyName";
        });

        curCol.formatter = null;
        return colModel;
    };

    return me;
})();