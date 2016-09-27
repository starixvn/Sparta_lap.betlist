/*global Site, jqGridAdditional, JSLINQ*/
/*exported ColossusBetsOption*/
/// <reference path="/@JSense.js" />

var ColossusBetsOption = (function () {
    "use strict";

    var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return ColossusBets.drillDown(' + level + ',' +
         currencyId + ',' + custId + ',\'' + datetime + '\');">' + cellvalue + '</a>';
    };

    var dateFormatter = function (cellvalue, options, rowObject) {
        cellvalue = $.fn.fmatter.call(this, "date", cellvalue, options, rowObject);
        return setDrillDownLink(1, 0, 0, rowObject.Date, cellvalue);
    };

    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = ColossusBetsOption.licenseeId === ColossusBetsOption.LICENSEE_12BET ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
    };

    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
    };

    //#region Columns

    var indexColumn = { 
        name: 'Index',
        index: 'Index',
        resizable: false,
        width: 30,
        align: 'center',
        valign: 'middle',
        frozen: true,
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
        sortable: false
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: false
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

    var turnoverColumn =
    {
        name: 'TurnOver',
        index: 'TurnOver',
        width: 75,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var totalTransColumn =
    {
        name: 'TotalTransaction',
        index: 'TotalTransaction',
        width: 75,
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

    var winningPrizeColumn =
    {
        name: 'WinningPrize',
        index: 'WinningPrize',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var cashOutColumn =
    {
        name: 'CashOut',
        index: 'CashOut',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var cashInShareColumn =
    {
        name: 'CashIn',
        index: 'CashIn',
        classes: 'highlight',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var commissionShareColumn =
    {
        name: 'CommissionShare',
        classes: 'highlight',
        index: 'CommissionShare',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    var amountDueToCBColumn =
    {
        name: 'AmountDueToCB',
        index: 'AmountDueToCB',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var grandPrizeColumn =
    {
        name: 'GrandPrize',
        index: 'GrandPrize',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
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
      'Turnover', 'TotalTransaction', 'WinningPrize', 'CashOut', 'CashInShare', 'CommissionShare',
      'AmountDueToCB', 'GrandPrize', 'CurrencyId', 'CustomerId'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
        turnoverColumn, totalTransColumn, winningPrizeColumn, cashOutColumn, cashInShareColumn, commissionShareColumn,
        amountDueToCBColumn, grandPrizeColumn, currencyIdColumn, customerIdColumn];

    var me =
    {
        canDrilldown: true,
        LICENSEE_12BET: 1,
        licenseeId: 0,
        totalColspan: [2, 2, 3],
        urlData: 'api/ColossusBetsWinlossReport/Index',
        urlExcelData: 'api/ColossusBetsWinlossReport/Index',
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
            rowNum: 50000,
            grouping: true,
            viewrecords: true,
            gridview: true,
            pager: '#ppdata',
            rownumbers: true
        }
    };

    me.setGroupHeader = function (grid) {
        var group = {
            startColumnName: 'WinningPrize',
            numberOfColumns: 2,
            titleText: Site.getSingleResource('SmallPrize')
        };
        
        grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [group]
        });
    };

    me.getColName = function () {
        return Site.getResources(colNameList);
    };

    me.getColModel = function (level, canDrilldown) {
        level = Number(level);

        if (canDrilldown) {
            currencyColumn.formatter = level === 0 ? currencyFormatter : null;
            systemIdColumn.formatter = level === 1 ? systemIdFormatter : null;

            dateColumn.hidden = level > 0;
        }

        dateColumn.formatoptions =
        {
            newformat: $('#reportbydaily').is(':checked') ? 'm/d/Y' : 'm/Y'
        };
        externalIdColumn.hidden = level < 2;
        systemIdColumn.hidden = level === 0;

        return Site.clone(colModelList);
    };

    me.getGridOption = function (level, canDrilldown) {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel(level, canDrilldown);
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