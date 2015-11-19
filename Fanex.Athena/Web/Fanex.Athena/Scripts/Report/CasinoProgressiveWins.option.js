/*global Site, jqGridAdditional*/
/*exported CasinoProgressiveWinsOption*/
/// <reference path="/@JSense.js" />

var CasinoProgressiveWinsOption = (function () {
    "use strict";

    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        resizable: false,
        sortable: false,
        width: 30,
        align: 'center',
        valign: 'middle'
    };

    var dateColumn =
    {
        name: 'WinlostDate',
        index: 'WinlostDate',
        resizable: false,
        sortable: false,
        width: 55,
        align: 'center',
        valign: 'middle',
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
        sortable: false,
        width: 120,
        align: 'left'
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        sortable: false,
        width: 120,
        align: 'left'
    };

    var refNoColumn =
    {
        name: 'RefNo',
        index: 'Refo',
        sortable: false,
        width: 75,
        align: 'center'
    };

    var choiceColumn =
    {
        name: 'Choice',
        index: 'Choice',
        sortable: false,
        width: 75,
        align: 'right'
    };

    var winAmountColumn =
    {
        name: 'WinAmount',
        index: 'WinAmount',
        sortable: false,
        width: 90,
        align: 'right',
        formatter: jqGridAdditional.formatDecimal
    };

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "ReferenceNo", 'Choice', 'Amount'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, refNoColumn, choiceColumn, winAmountColumn];

    var me =
    {
        rowNum: 5000,     
        urlData: 'api/CasinoProgressiveWinsReport/index',
        urlExcelData: 'api/CasinoProgressiveWinsReport/index',
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
            footerrow: false,
            userDataOnFooter: false,
            userData: [],
            grouping: true,
            viewrecords: true,
            gridview: true,
            rownumbers: true
        }
    };

    me.getColName = function () {
        var temp = Site.clone(colNameList);
        return Site.getResources(temp);
    };

    me.getColModel = function () {
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