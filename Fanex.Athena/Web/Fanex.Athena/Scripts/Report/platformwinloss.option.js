/*global Site,jqGridAdditional*/
/*exported PlatformWinLossOption*/
/// <reference path="/@JSense.js" />
//##
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var PlatformWinLossOption = (function () {
    "use strict";
    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
        {
            DrillDownCurrencyId: rowObject.CurrencyId,
            DrillDownCurrencyName: rowObject.CurrencyName,
            CustomerId: rowObject.CustomerId,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        };
        return '<a class="bet-list-link" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
    };

    var drilldownLink = function (cellvalue, options, rowObject) {
        var param =
        {
            PlatformType: rowObject.PlatformType > 0 ? rowObject.PlatformType : Number(rowObject.Parent),
            ProductCode: rowObject.PlatformType < 0 ? rowObject.ProductCode : undefined
        };

        return '<a class="drilldown" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
    };

    var idColumn = {
        name: 'PlatformType',
        index: 'PlatformType',
        key: true,
        hidden: true,
        align: 'center'
    };

    var platformNameColumn = {
        name: 'Name',
        index: 'Name',
        width: 120,
        align: 'left',
        valign: 'middle',
        sortable: false,
        resizable: false,
        formatter: drilldownLink
    };

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 60,
        align: 'center',
        valign: 'middle',
        sortable: false,
        resizable: false,
        cellattr: jqGridAdditional.rowSpanSetting
    };
    var externalIdColumn = {
        name: 'ExternalId',
        index: 'ExternalId',
        width: 85,
        align: 'left',
        sortable: true,
        resizable: false,
        formatter: betlistLink
    };
    var systemIdColumn = {
        name: 'SystemId',
        index: 'SystemId',
        width: 120,
        align: 'left',
        sortable: true,
        resizable: false,
        formatter: betlistLink
    };
    var currencyColumn = {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 60,
        align: 'center',
        sortable: true,
        resizable: false
    };
    var turnoverColumn = {
        name: 'Turnover',
        index: 'Turnover',
        resizable: false,
        width: 85,
        align: 'right',
        sortable: true,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var turnoverPercentColumn = {
        name: 'TurnoverPercent',
        index: 'TurnoverPercent',
        resizable: false,
        width: 85,
        align: 'right',
        sortable: true,
        formatter: 'currency',
        formatoptions: { decimalPlaces: 2, suffix: '%' }
    };
    var totalTransColumn = {
        name: 'TotalTransactions',
        index: 'TotalTransactions',
        width: 95,
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
    var customerWlColumn = {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal
    };
    var playerCommColumn = {
        name: 'CustomerComm',
        index: 'CustomerComm',
        width: 50,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal
    };
    var customerWlTotalColumn = {
        name: 'CustomerTotal',
        index: 'CustomerTotal',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal
    };
    var licenseeWlColumn = {
        name: 'LicenseeWL',
        classes: 'highlight',
        index: 'LicenseeWL',
        width: 85,
        align: 'right',
        sortable: true,
        resizable: false,
        frozen: true,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };
    var baCommissionColumn = {
        name: 'BACommission',
        classes: 'highlight',
        index: 'BACommission',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var racingRevenueColumn = {
        name: 'RacingRevenue',
        classes: 'highlight',
        index: 'RacingRevenue',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var amtDueToBaColumn = {
        name: 'AmtDueToBA',
        index: 'AmtDueToBA',
        width: 60,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var amtDueToRacingColumn = {
        name: 'AmtDueToRacing',
        index: 'AmtDueToRacing',
        width: 70,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };
    var currencyIdColumn = {
        name: 'CurrencyId',
        index: 'CurrencyId',
        width: 0,
        align: 'center',
        sortable: false,
        resizable: false,
        hidden: true
    };
    var customerIdColumn = {
        name: 'CustomerId',
        index: 'CustomerId',
        width: 0,
        align: 'center',
        sortable: false,
        resizable: false,
        hidden: true
    };

    var colNameList = ['Id', 'Platform','#', 'ExternalId', "SystemId", "Currency", 'Turnover','%Turnover', 'TotalTransaction', '@[Win]/@[Loss]',
                'Comm', 'Total', 'LicenseeW_L', 'BA<br/>@[Commission]', '@[RacingRevenue]', '@[AmtDueToBA]', '@[AmtDueToRacing]', 'CurrencyId', 'CustomerId'];
    var colModelList = [idColumn, platformNameColumn, indexColumn, externalIdColumn, systemIdColumn, currencyColumn, turnoverColumn,
                turnoverPercentColumn, totalTransColumn, customerWlColumn, playerCommColumn, customerWlTotalColumn, licenseeWlColumn,
                baCommissionColumn, racingRevenueColumn, amtDueToBaColumn, amtDueToRacingColumn, currencyIdColumn, customerIdColumn];
    var customerWlTotalIndex = 10;

    var me = {
        hasCustomerComm: true,
        pageSize: 50,
        nonePagingSize: 2000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
        Platforms:
        {
            "0": Site.getSingleResource("All"),
            "1": Site.getSingleResource("Asia"),
            "2": Site.getSingleResource("NewAsia"),
            "3": Site.getSingleResource("Euro"),
            "4": Site.getSingleResource("Wap"),
            "5": Site.getSingleResource("SmartSite")
        },
        totalColspan: [0, 4],
        urlData: 'api/report/PlatformWinLoss',
        urlExcelData: 'api/report/PlatformWinLoss',
        gridOption: {
            datatype: 'jsonstring',
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
            treeGrid: true,
            treeGridModel: 'adjacency',
            treedatatype: "local",
            ExpandColumn: 'Name',
            treeReader: {
                level_field: "Level",
                parent_id_field: "Parent", 
                leaf_field: "IsLeaf"
            },
            userData: [],
            pager: '#ppdata',
            beforeSelectRow: function () {
                return false;
            },
            onRightClickRow: function () {
                return false;
            }
        }
    };

    function isProductChecked(productOptions, productCode) {
        var length = productOptions.length;

        for (var i = 0; i < length; i++) {
            if (productOptions[i].Code === productCode) {
                return productOptions[i].Status;
            }
        }

        return false;
    }

    me.getColName = function () {
        // Column Customer Wl
        colNameList[customerWlTotalIndex] = me.hasCustomerComm ? 'Total' : 'Customer W/L';
        return Site.getResources(colNameList);
    };

    me.getColModel = function (level, productOptions) {
        platformNameColumn.hidden = level === 1;
        turnoverPercentColumn.hidden = level === 1;
        indexColumn.hidden = level === 0;
        systemIdColumn.hidden = level === 0;
        externalIdColumn.hidden = level === 0;
        currencyColumn.hidden = level === 0;
        baCommissionColumn.hidden = level === 0;
        amtDueToBaColumn.hidden = level === 0;
        racingRevenueColumn.hidden = level === 0;
        amtDueToRacingColumn.hidden = level === 0;

        licenseeWlColumn.classes = (level === 0 ? null : "highlight");
        baCommissionColumn.classes = licenseeWlColumn.classes;

        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;

        if (level === 1) {
            var allowedBa = isProductChecked(productOptions, "BA");
            var allowedRacing = isProductChecked(productOptions, "RB");

            baCommissionColumn.hidden = !allowedBa;
            amtDueToBaColumn.hidden = !allowedBa;
            racingRevenueColumn.hidden = !allowedRacing;
            amtDueToRacingColumn.hidden = !allowedRacing;
        }

        if (level === 0)
        {
            var columns = Site.clone(colModelList);

            for (var i = 0; i < columns.length; i++)
            {   
                columns[i].sortable = false;
            }

            return columns;
        }

        return colModelList;
    };

    me.getGridOption = function (level, productOptions) {
        me.gridOption.colNames = me.getColName();
        me.gridOption.colModel = me.getColModel(level, productOptions);

        return me.gridOption;
    };

    me.getTotalColModel = function () {
        var colModel = me.getColModel();
        return colModel;
    };

    return me;
})();