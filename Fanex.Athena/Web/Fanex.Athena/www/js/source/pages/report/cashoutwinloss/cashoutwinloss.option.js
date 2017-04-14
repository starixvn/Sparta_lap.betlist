"use strict";
define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional"], function ($, Site, jqGridAdditional) {
    var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return cashoutDrillDown(' + level + ',' +
         currencyId + ',' + custId + ',\'' + datetime + '\');">' + cellvalue + '</a>';
    };
    var dateFormatter = function (cellvalue, options, rowObject) {
        cellvalue = $.fn.fmatter.call(this, "date", cellvalue, options, rowObject);
        return setDrillDownLink(1, 0, 0, rowObject.Date, cellvalue);
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
        width: 45,
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
        width: 100,
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
        width: 150,
        align: 'left',
        sortable: false,
        formatter: betlistLink
    };

    var currencyColumn =
    {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 80,
        align: 'center',
        sortable: false,
        resizable: false,
        formatter: currencyFormatter
    };

    var cashOutTimesColumn =
    {
        name: 'CashOutTimes',
        index: 'CashOutTimes',
        width: 120,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var cashOutTicketsColumn =
    {
        name: 'CashOutTickets',
        index: 'CashOutTickets',
        width: 100,
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

    var cashOutValueColumn =
    {
        name: 'CashOutValue',
        index: 'CashOutValue',
        width: 120,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
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
        width: 100,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var settleAmountBuyBackCommColumn =
    {
        name: 'SettleAmountBuyBackComm',
        index: 'SettleAmountBuyBackComm',
        width: 100,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var settleAmountTotalColumn =
    {
        name: 'SettleAmountTotal',
        index: 'SettleAmountTotal',
        width: 100,
        align: 'right',
        sortable: true,
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

    var dummyColumn =
    {
        name: 'Dummy',
        index: 'Dummy',
        width: 100,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    //#endregion

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
      'CashOutTimes', 'CashOutTickets', 'CashOutValue', 'TicketPayout', 'CashOutCommission',
      'TicketPayout', 'BBAmtComm', 'Total', 'CurrencyId', 'CustomerId'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
        cashOutTimesColumn, cashOutTicketsColumn, cashOutValueColumn, ticketPayoutColumn, cashOutCommissionColumn,
        settleAmountTicketPayoutColumn, settleAmountBuyBackCommColumn, settleAmountTotalColumn, currencyIdColumn, customerIdColumn];

    function addBuyBackAmountColumns(columns, columnIndex) {
        var buyBackAmountColumn = Site.clone(dummyColumn);
        buyBackAmountColumn.name = "TotalBuyBackAmount";
        buyBackAmountColumn.index = "TotalBuyBackAmount";

        var buyBackAmountSBColumn = Site.clone(dummyColumn);
        buyBackAmountSBColumn.name = "BuyBackAmountSB";
        buyBackAmountSBColumn.index = "BuyBackAmountSB";

        var buyBackAmountBAColumn = Site.clone(dummyColumn);
        buyBackAmountBAColumn.name = "BuyBackAmountBA";
        buyBackAmountBAColumn.index = "BuyBackAmountBA";

        columns.insertAfter(buyBackAmountColumn, columnIndex);
        columns.insertAfter(buyBackAmountBAColumn, columnIndex);
        columns.insertAfter(buyBackAmountSBColumn, columnIndex);
    }

    var _me = {
        licenseeId: 0,
        isLicenseeOfferBA: false,
        buyBackAmountColumnIndex: 7,
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
            autowidth: false,
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
        },

        setGroupHeaders: function ($grid) {
            var groupBuyBackAmount = _me.isLicenseeOfferBA ?
            {
                startColumnName: "BuyBackAmountSB",
                numberOfColumns: 3,
                titleText: Site.getSingleResource('BuyBackAmount')
            } : {};
            var groupSettleAmount = {
                startColumnName: "SettleAmountTicketPayout",
                numberOfColumns: 3,
                titleText: Site.getSingleResource('SettleAmount')
            };
            $grid.jqGrid('setGroupHeaders', {
                useColSpanStyle: true,
                groupHeaders: [groupBuyBackAmount, groupSettleAmount]
            });
        },

        getColName: function () {
            var temp = Site.clone(colNameList);
            if (_me.isLicenseeOfferBA) {
                temp.insertAfter("Total", _me.buyBackAmountColumnIndex);
                temp.insertAfter("BA", _me.buyBackAmountColumnIndex);
                temp.insertAfter("Sportsbook", _me.buyBackAmountColumnIndex);
            }
            return Site.getResources(temp);
        },

        getColModel: function (level) {
            level = Number(level);

            currencyColumn.formatter = level === 0 ? currencyFormatter : null;

            dateColumn.hidden = level > 0;
            dateColumn.formatoptions =
            {
                newformat: $('#reportbydaily').is(':checked') ? 'm/d/Y' : 'm/Y'
            };
            externalIdColumn.hidden = level === 0;
            systemIdColumn.hidden = level === 0;

            var temp = Site.clone(colModelList);

            if (_me.isLicenseeOfferBA) {
                addBuyBackAmountColumns(temp, _me.buyBackAmountColumnIndex);
            }

            return temp;
        },

        getGridOption: function (level) {
            _me.gridOption.colNames = _me.getColName();
            _me.gridOption.colModel = _me.getColModel(level);
            return Site.clone(_me.gridOption);
        },

        getTotalColModel: function (level) {
            var colModel = _me.getColModel(level);
            // Not show drill down link on total link
            var currencyNameCol = colModel.filter(function (item) {
                return item.name === "CurrencyName";
            })[0];

            currencyNameCol.formatter = null;

            return colModel;
        }
    };

    return _me;
});