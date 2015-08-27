/*global Site, JSLINQ, jqGridAdditional*/
/// <reference path="/@JSense.js" />
//#regions licenseeWL options
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var WinLossByProductOption = (function () {
    "use strict";
    var setDrillDownLink = function (level, currencyId, custId, datetime, currencyName, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return WinLossByProduct.drillDown(' + level +
         ',' + currencyId + ',' + custId + ',\'' + datetime + '\',\'' + currencyName + '\');">' + cellvalue + '</a>';
    };
    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = WinLossByProductOption.licenseeId === 1 ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, rowObject.CurrencyName, cellvalue);
    };
    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, rowObject.CurrencyName, cellvalue);
    };
    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
            {
                DrillDownFromDate: rowObject.Date,
                DrillDownToDate: rowObject.Date,
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
            width: 30,
            align: 'center',
            valign: 'middle',
            sorttype: 'int',
            sortable: false,
            resizable: false
        };
    var dateColumn =
        {
            name: 'Date',
            index: 'Date',
            width: 55,
            align: 'center',
            valign: 'middle',
            sortable: false,
            resizable: false,
            formatter: 'date',
            formatoptions:
            {
                newformat: 'm/d/Y'
            },
            hidden: true
        };
    var externalIdColumn =
        {
            name: 'ExternalId',
            index: 'ExternalId',
            width: 95,
            align: 'left',
            sortable: false,
            resizable: false
        };
    var systemIdColumn =
        {
            name: 'SystemId',
            index: 'SystemId',
            width: 120,
            align: 'left',
            sortable: false,
            resizable: false
        };
    var currencyColumn =
        {
            name: 'CurrencyName',
            index: 'CurrencyName',
            width: 55,
            align: 'center',
            sortable: false,
            resizable: false,
            formatter: currencyFormatter
        };
    var turnoverColumn =
        {
            name: 'TurnOver',
            index: 'TurnOver',
            width: 90,
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
            width: 68,
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
            width: 100,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };
    var playerCommColumn =
        {
            name: 'CustomerComm',
            index: 'CustomerComm',
            width: 70,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };
    var customerWlTotalColumn =
        {
            name: 'CustomerTotal',
            index: 'CustomerTotal',
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
    var LicenseeWlCommColumn =
        {
            name: 'LicenseeWLComm',
            classes: 'highlight',
            index: 'LicenseeWLComm',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

    var amtDueToColumn =
        {
            name: 'AmountDue',
            index: 'AmountDue',
            width: 85,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

    var currencyIdColumn =
        {
            name: 'CurrencyId',
            index: 'CurrencyId',
            width: 0,
            align: 'center',
            sortable: false,
            resizable: false,
            hidden: true
        };
    var customerIdColumn =
        {
            name: 'CustomerId',
            index: 'CustomerId',
            width: 0,
            align: 'center',
            sortable: false,
            resizable: false,
            hidden: true
        };
    var productColumn =
        {
            name: 'ProductName',
            index: 'productName',
            width: 90,
            sortable: false,
            resizable: false,
            align: "left",
            hidden: false,
            title: true
        };

    //#endregion
    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency", 'Product', 'Turnover',
                'TotalTransaction', '@[Win]/@[Loss]', 'Comm', 'Total', 'LicenseeW_L', 'Comm', 'AmountDue', 'CurrencyId', 'CustomerId'];
    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn, productColumn, turnoverColumn,
                totalTransColumn, customerWlColumn, playerCommColumn, customerWlTotalColumn,
                 licenseeWlColumn, LicenseeWlCommColumn, amtDueToColumn, currencyIdColumn, customerIdColumn];

    var me =
        {
            licenseeId: 0,
            currencyIndex: 4,
            systemIdIndex: 3,
            externalIdIndex: 2,
            customerWlTotalIndex: 10,
            hasCustomerComm: true,
            IsAllowBA: true,
            IsAllowRB: true,
            pageSize: 50,
            nonePagingSize: 2000,
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            totalColspan: [1, 2, 3],
            //level 0,1,2
            urlData: 'api/report/WinLostByProduct',
            urlExcelData: 'api/report/WinLostByProduct',
            gridOption:
            {
                datatype: 'local',
                colNames: [],
                colModel: [],
                data: [],
                height: 'auto',
                autowidth: true,
                forceFit: false,
                shrinkToFit: true,
                loadtext: 'loading...',
                loadui: 'disable',
                loadonce: true,
                footerrow: true,
                pager: '#ppdata',
                userDataOnFooter: true,
                userData: [],
                viewrecords: true,
                gridview: true
            }
        };

    //# getGidOptionregion

    me.getColName = function () {
        // Column Customer Wl
        colNameList[me.customerWlTotalIndex] = me.hasCustomerComm ? 'Total' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    me.getColModel = function (level, reportBy) {
        level = Number(level);
        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;
        LicenseeWlCommColumn.hidden = !me.IsAllowBA && !me.IsAllowRB;
        // Column hidden by level
        currencyColumn.formatter = level === 0 ? currencyFormatter : null;
        systemIdColumn.formatter = level === 1 ? systemIdFormatter : null;

        dateColumn.hidden = level > 0;
        externalIdColumn.hidden = level < 2;
        systemIdColumn.hidden = level === 0;

        if (level === 2) {
            externalIdColumn.formatter = betlistLink;
            systemIdColumn.formatter = betlistLink;
        }

        if (Number(reportBy) === 2) {
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

    me.getGridOption = function (level, reportBy) {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel(level, reportBy);

        if (level === 0) {
            me.gridOption.grouping = true;
            me.gridOption.groupingView = { groupField: ['Date'], groupColumnShow: [false] };
        } else {
            me.gridOption.grouping = false;
            me.gridOption.groupingView = null;
        }

        return me.gridOption;
    };
    //#end

    me.getTotalColModel = function (level) {
        var colModel = me.getColModel(level, -1);
        // Not show drill down link on total link
        var curCol = JSLINQ(colModel).First(function (item) {
            return item.name === "CurrencyName";
        });

        curCol.formatter = null;
        return colModel;
    };

    return me;
})();