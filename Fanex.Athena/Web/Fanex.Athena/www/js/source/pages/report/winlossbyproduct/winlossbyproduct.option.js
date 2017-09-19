define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional"], function ($, Site, jqGridAdditional) {
    "use strict";

    var setDrillDownLink = function (level, currencyId, custId, datetime, currencyName, cellvalue) {
        var dataDrilldown = {
            level: level,
            currencyId: currencyId,
            custId: custId,
            datetime: datetime,
            currencyName: currencyName
        };

        return '<a class="drilldown" href="javascript:;" data-drill-down=' + JSON.stringify(dataDrilldown) + '>' + cellvalue + '</a>';
    };
    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = _me.licenseeId === 1 ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, rowObject.CurrencyName, cellvalue);
    };
    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, rowObject.CurrencyName, cellvalue);
    };
    var betlistLink = function (cellvalue, options, rowObject) {
        var param = {
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

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        valign: 'middle',
        sorttype: 'int',
        sortable: false,
        resizable: false
    };
    var dateColumn = {
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
    var externalIdColumn = {
        name: 'ExternalId',
        index: 'ExternalId',
        width: 95,
        align: 'left',
        sortable: false,
        resizable: false
    };
    var systemIdColumn = {
        name: 'SystemId',
        index: 'SystemId',
        width: 120,
        align: 'left',
        sortable: false,
        resizable: false
    };
    var currencyColumn = {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 60,
        align: 'center',
        sortable: false,
        resizable: false,
        formatter: currencyFormatter
    };
    var turnoverColumn = {
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
    var totalTransColumn = {
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
    var customerWlColumn = {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };
    var playerCommColumn = {
        name: 'CustomerComm',
        index: 'CustomerComm',
        width: 70,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };
    var customerWlTotalColumn = {
        name: 'CustomerTotal',
        index: 'CustomerTotal',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };
    var licenseeWlColumn = {
        name: 'LicenseeWL',
        index: 'LicenseeWL',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };
    var LicenseeWlCommColumn = {
        name: 'LicenseeWLComm',
        index: 'LicenseeWLComm',
        width: 70,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var amtDueToColumn = {
        name: 'AmountDue',
        index: 'AmountDue',
        width: 85,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var productColumn = {
        name: 'ProductName',
        index: 'productName',
        width: 160,
        sortable: false,
        resizable: false,
        align: "left",
        hidden: false,
        title: true
    };

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency", 'Product', 'Turnover',
                'TotalTransaction', '@[Win]/@[Loss]', 'Comm', 'Total', 'LicenseeW_L', 'Comm', 'AmountDue'];
    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn, productColumn, turnoverColumn,
                totalTransColumn, customerWlColumn, playerCommColumn, customerWlTotalColumn,
                 licenseeWlColumn, LicenseeWlCommColumn, amtDueToColumn];

    var _me = {
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
        totalColspan: [1, 2, 3],
        //level 0,1,2
        urlData: 'api/WinLossByProductReport/Index',
        urlExcelData: 'api/WinLossByProductReport/Index',
        gridOption:
        {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            autowidth: false,
            forceFit: false,
            shrinkToFit: true,
            loadtext: '',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            pager: '#ppdata',
            userDataOnFooter: true,
            userData: [],
            viewrecords: true,
            gridview: true,
            sortname: "Index",
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000]
        }
    };

    _me.getColName = function () {
        // Column Customer Wl
        colNameList[_me.customerWlTotalIndex] = _me.hasCustomerComm ? 'Total' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    _me.getColModel = function (level, reportBy) {
        level = Number(level);
        // Column hidden by customer comm
        customerWlColumn.hidden = !_me.hasCustomerComm;
        playerCommColumn.hidden = !_me.hasCustomerComm;
        LicenseeWlCommColumn.hidden = !_me.IsAllowBA && !_me.IsAllowRB;
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

    _me.getGridOption = function (level, reportBy) {
        _me.gridOption.colNames = _me.getColName();
        _me.gridOption.colModel = _me.getColModel(level, reportBy);

        if (level === 0) {
            _me.gridOption.grouping = true;
            _me.gridOption.groupingView = {
                groupField: ['Date'],
                groupColumnShow: [false],
                plusicon: "ui-icon-circlesmall-plus icon-arrow",
                minusicon: "ui-icon-circlesmall-minus icon-arrow"
            };
        } else {
            _me.gridOption.grouping = false;
            _me.gridOption.groupingView = null;
        }

        return _me.gridOption;
    };

    _me.getTotalColModel = function (level) {
        var colModel = _me.getColModel(level, -1);
        // Not show drill down link on total link
        var curCol = colModel.filter(function (item) {
            return item.name === "CurrencyName";
        })[0];

        curCol.formatter = null;
        return colModel;
    };

    return _me;
});