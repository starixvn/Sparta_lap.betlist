/*global Site, jqGridAdditional*/
/*exported MicroGamingWinLossOption*/
/// <reference path="/@JSense.js" />

var MicroGamingWinLossOption = (function () {
    "use strict";

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
        sortable: false,
        sorttype: function (cellValue, obj) {
            return obj.Index + '-' + (obj.RowSpan > 1 ? 0 : 1);
        }
    };

    var typeNameColumn =
    {
        name: 'TypeName',
        index: 'TypeName',
        resizable: false,
        width: 30,
        align: 'center',
        valign: 'middle',
        frozen: true,
        sortable: false,
        sorttype: function (cellValue, obj) {
            return obj.Index;
        }
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
        formatter: 'date',
        formatoptions:
        {
            newformat: 'm/d/Y'
        }
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

    var customerWlColumn =
    {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var progressiveContributionColumn =
    {
        name: 'ProgressiveContribution',
        index: 'ProgressiveContribution',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var progressivePayoutColumn =
    {
        name: 'ProgressivePayout',
        index: 'ProgressivePayout',
        width: 90,
        align: 'right',
        sortable: false,
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
        sortable: false,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    var licenseeNetWinColumn =
    {
        name: 'LicenseeNetWin',
        classes: 'highlight',
        index: 'LicenseeNetWin',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    var amountDueToMGColumn =
    {
        name: 'AmountDueToMGC',
        index: 'AmountDueToMGC',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatDecimal
    };

    var progressiveFeeColumn =
    {
        name: 'ProgressiveFee',
        index: 'ProgressiveFee',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatDecimal
    };

    //#endregion

    var colNameList = ['#', 'TypeName', 'Date', 'Currency',
      'Turnover', 'TotalTransaction', 'CustomerW_L', 'ProgressiveContribution', 'ProgressivePayout',
      'LicenseeW_L', 'LicenseeNetWin', 'AmountDueToMGC', 'ProgressiveFee'];

    var colModelList = [indexColumn, typeNameColumn, dateColumn, currencyColumn,
        turnoverColumn, totalTransColumn, customerWlColumn, progressiveContributionColumn, progressivePayoutColumn,
        licenseeWlColumn, licenseeNetWinColumn, amountDueToMGColumn, progressiveFeeColumn];

    var me =
    {
        totalColspan: 2,
        urlData: 'api/report/MicrogamingWinloss',
        urlExcelData: 'api/report/MicrogamingWinloss',
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
            groupingView: { groupField: ['TypeName'], groupColumnShow: [false] },
            viewrecords: true,
            gridview: true,
            pager: '#ppdata',
            sortname: 'Index'
        }
    };

    me.getColName = function () {
        return Site.getResources(colNameList);
    };

    me.getColModel = function () {
        dateColumn.formatoptions =
        {
            newformat: $('#reportbydaily').is(':checked') ? 'm/d/Y' : 'm/Y'
        };
        return Site.clone(colModelList);
    };

    me.getGridOption = function () {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel();
        return Site.clone(me.gridOption);
    };

    me.getTotalColModel = function () {
        return me.getColModel();
    };

    return me;
})();