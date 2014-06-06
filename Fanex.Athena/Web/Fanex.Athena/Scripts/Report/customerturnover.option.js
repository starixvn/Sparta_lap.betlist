/*global Site, jqGridAdditional*/
/*exported CustTurnoverOption*/
/// <reference path="/@JSense.js" />
/*
* Author:Alex.Nguyen
* Created Date:10/15/2013
*/
//#region Columns
var CustTurnoverOption = (function () {
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
        name: 'ReportBy',
        index: 'ReportBy',
        width: 60,
        align: 'center',
        valign: 'middle',
        sortable: false,
        resizable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting,
        formatter: 'date',
        formatoptions:
        {
            newformat: 'm/d/Y'
        }
    };
    var userNameColumn = {
        name: 'UserName',
        index: 'UserName',
        width: 85,
        align: 'left',
        sortable: false,
        frozen: true,
        resizable: false,
        cellattr: jqGridAdditional.rowSpanSetting
    };
    var currencyColumn = {
        name: 'Currency',
        index: 'Currency',
        width: 45,
        align: 'center',
        sortable: false,
        resizable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting
    };
    var sportColumn = {
        name: 'SportType',
        index: 'SportType',
        width: 80,
        align: 'left',
        sortable: false,
        resizable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting2
    };
    var bettypeColumn = {
        name: 'Bettype',
        index: 'Bettype',
        width: 140,
        align: 'left',
        sortable: false,
        resizable: false,
        frozen: true
    };
    var totalTransColumn = {
        name: 'TotalTransactions',
        index: 'TotalTransactions',
        width: 65,
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
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
        resizable: false,
        width: 100,
        align: 'right',
        sortable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal

    };
    var totalturnoverColumn = {
        name: 'TotalTurnover',
        index: 'TotalTurnover',
        resizable: false,
        width: 100,
        align: 'right',
        sortable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting,
        formatter: jqGridAdditional.formatDecimal

    };
    var customerWlColumn = {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal


    };
    var playerCommColumn = {
        name: 'CustomerComm',
        index: 'CustomerComm',
        width: 50,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal


    };
    var customerWlTotalColumn = {
        name: 'CustomerTotal',
        index: 'CustomerTotal',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal


    };
    //#endregion
    var colNameList = ['#', 'Date', "Username", "Currency", 'Sport', 'BetType',
    'TotalTransaction', 'Turnover', 'TotalTurnover', '@[Win]/@[Loss]', 'Comm', 'Total'];

    var colModelList = [indexColumn, dateColumn, userNameColumn, currencyColumn, sportColumn, bettypeColumn,
                    totalTransColumn, turnoverColumn, totalturnoverColumn, customerWlColumn, playerCommColumn,
                    customerWlTotalColumn];
    var customerWlTotalIndex = 11;

    var me = {
        hasCustomerComm: false,
        totalColspan: 6,
        urlData: 'report/CustomerTurnover',
        urlExcelData: 'report/CustomerTurnoverExcel',
        reportBy: 1, //1:daily ,2 :monthly,
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

    var getColName = function () {
        // Column Customer Wl
        colNameList[customerWlTotalIndex] = me.hasCustomerComm ? 'Total' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    var getColModel = function () {
        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;

        if (Number(me.reportBy) === 2) {

            dateColumn.formatoptions = {
                newformat: 'm/Y'
            };
        } else {
            dateColumn.formatoptions = {
                newformat: 'm/d/Y'
            };
        }

        return colModelList;
    };

    me.getGridOption = function () {
        me.gridOption.colNames = getColName();
        me.gridOption.colModel = getColModel();

        return me.gridOption;
    };

    return me;
})();