/*global Site, jqGridAdditional*/
/// <reference path="/@JSense.js" />
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var AgCasinoWinLossOption = (function () {
    "use strict";
    // store state of toggle() function
    var _expanseTypes = {};

    var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return AgCasinoWinLoss.drillDown(' + level + ',' +
         currencyId + ',' + custId + ',\'' + datetime + '\');">' + cellvalue + '</a>';
    };

    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = Number(AgCasinoWinLossOption.licenseeId) === 1 ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
    };

    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
    };

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

    //#region Columns

    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        resizable: false,
        width: 30,
        align: 'center',
        valign: 'middle',
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
        sortable: true,
        formatter: 'date',
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
        sortable: true
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: true,
        formatter: function (cellvalue, options, rowObject) {
            return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
        }
    };

    var currencyColumn =
    {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 60,
        align: 'center',
        sortable: true,
        resizable: false,
        formatter: currencyFormatter
    };

    var turnoverColumn =
    {
        name: 'TotalTurnOverSingle',
        index: 'TotalTurnOverSingle',
        width: 75,
        align: 'right',
        sortable: true,
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
        sortable: true,
        resizable: false,
        formatter: 'integer',
        frozen: true,
        formatoptions:
        {
            thousandsSeparator: ","
        }
    };

    var customerWlColumn =
    {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var licenseeWlColumn =
    {
        name: 'LicenseeWL',
        classes: 'highlight',
        index: 'LicenseeWL',
        width: 100,
        align: 'right',
        sortable: true,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    var currencyIdColumn =
    {
        name: 'CurrencyId',
        index: 'CurrencyId',
        width: 2,
        align: 'center',
        sortable: true,
        resizable: false,
        hidden: true
    };

    var customerIdColumn =
    {
        name: 'CustomerId',
        index: 'CustomerId',
        width: 2,
        align: 'center',
        sortable: true,
        resizable: false,
        hidden: true
    };

    //#endregion

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
      'Turnover', 'TotalTransaction', 'Customer_WL', 'LicenseeW_L', 'CurrencyId', 'CustomerId'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
        turnoverColumn, totalTransColumn, customerWlColumn, licenseeWlColumn, currencyIdColumn, customerIdColumn];

    var me =
    {
        licenseeId: 0,
        currencyIndex: 4,
        turnoverIndex: 5,
        licenseeWlIndex: 10,
        customerWlTotalIndex: 9,
        hasCustomerComm: true,
        pageSize: 50,
        nonePagingSize: 5000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
        totalColspan: [2, 2, 3],
        //level 0,1,2
        urlData: 'api/report/AgCasinoWl',
        urlExcelData: 'api/report/AgCasinoWl',
        urlResources: 'Report/AgCasinoWLResource',
        gridOption:
        {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: '100%',
            autowidth: true,
            forceFit: false,
            shrinkToFit: true,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            userDataOnFooter: true,
            userData: [],
            grouping: true,
            viewrecords: true,
            gridview: true,
            rownumbers: true
        },
        restoreExpanse: function () {
            for (var property in _expanseTypes) {
                if (_expanseTypes.hasOwnProperty(property)) {
                    me.toggle(_expanseTypes[property], property);
                }
            }
        }
    };

    me.getColName = function () {
        return Site.getResources(colNameList);
    };

    me.getColModel = function (level) {
        // Column hidden by customer comm
       // customerWlColumn.hidden = !me.hasCustomerComm;
        level = Number(level);
        //set width of licenseeWL column
        if (!me.hasCustomerComm) { licenseeWlColumn.width = 90; }
        // Column hidden by level
        currencyColumn.formatter = level === 0 ? currencyFormatter : null;
        systemIdColumn.formatter = level === 1 ? systemIdFormatter : null;

        dateColumn.hidden = level > 0;
        externalIdColumn.hidden = level < 2;
        systemIdColumn.hidden = level === 0;

        // Row span setting
        // indexColumn.cellattr = level == 0 ? jqGridAdditional.rowSpanSetting : null;
        // dateColumn.cellattr = level == 0 ? jqGridAdditional.rowSpanSetting : null;
        if (level === 2) {
            externalIdColumn.formatter = betlistLink;
            systemIdColumn.formatter = betlistLink;
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
        return colModel;
    };
    return me;
})();