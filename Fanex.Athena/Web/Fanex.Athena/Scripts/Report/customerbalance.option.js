/*global Site, jqGridAdditional*/
/// <reference path="/@JSense.js" />

var CustomerBalanceOption = (function () {
    "use strict";
    var indexColumn =
    {
        name: "Index",
        index: "Index",
        resizable: false,
        width: 40,
        align: "center",
        valign: "middle",
        sortable: false
    };

    var externalIdColumn =
    {
        name: "ExternalId",
        index: "ExternalId",
        resizable: false,
        width: 120,
        align: "left",
        valign: "middle",
        sortable: false,
        cellattr: function (rowId, cellValue, rowObject) {
            return ' style="text-overflow: ellipsis"';
        }
    };

    var systemIdColumn =
    {
        name: "SystemId",
        index: "SystemId",
        resizable: false,
        width: 120,
        align: "left",
        valign: "middle",
        sortable: false
    };

    var openingBalanceColumn =
    {
        name: "OpeningBalance",
        index: "OpeningBalance",
        width: 110,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var depositColumn =
    {
        name: "Deposit",
        index: "Deposit",
        width: 90,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var withdrawalColumn =
    {
        name: "Withdrawal",
        index: "Withdrawal",
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var bonusColumn =
    {
        name: "Bonus",
        index: "Bonus",
        width: 60,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var adjustmentColumn =
    {
        name: "Adjustment",
        index: "Adjustment",
        width: 80,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var turnoverColumn =
    {
        name: "Turnover",
        index: "Turnover",
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var drawGameColumn =
    {
        name: "DrawGame",
        index: "DrawGame",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var netTurnoverColumn =
    {
        name: "NetTurnover",
        index: "NetTurnover",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var pendingFundsColumn =
    {
        name: "PendingFunds",
        index: "PendingFunds",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var customerWinLossMergeColumn =
    {
        name: "WinLoss",
        index: "CustomerW_L",
        width: 200,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var customerWinLossColumn =
    {
        name: "WinLoss",
        index: "CustomerWinLoss",
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var customerCommColumn =
    {
        name: "PlayerComm",
        index: "CustomerComm",
        width: 80,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var customerTotalColumn =
    {
        name: "TotalWL",
        index: "CustomerTotal",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var closingBalanceColumn =
    {
        name: "ClosingBalance",
        index: "ClosingBalance",
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var affiliateTransferColumn =
    {
        name: "AffiliateTransfer",
        index: "AffiliateTransfer",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var productBATurnoverColumn =
    {
        name: "TurnoverBA",
        index: "ProductBATurnover",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var productBACommColumn =
    {
        name: "CommBA",
        index: "ProductBAComm",
        width: 80,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var productBAAmountDueToColumn =
    {
        name: "AmountDueToBA",
        index: "ProductBAAmountDueTo",
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var colNameList = [
        "#",
        "ExternalId",
        "SystemId",
        "OpeningBalance",
        "Deposit",
        "Withdrawal",
        "Bonus",
        "Adjustment",
        "Turnover",
        "DrawGame",
        "NetTurnover",
        "PendingFunds",
        "ClosingBalance",
        "AffiliateTransfer",
    ];

    var colModelList = [
        indexColumn,
        externalIdColumn,
        systemIdColumn,
        openingBalanceColumn,
        depositColumn,
        withdrawalColumn,
        bonusColumn,
        adjustmentColumn,
        turnoverColumn,
        drawGameColumn,
        netTurnoverColumn,
        pendingFundsColumn,
        closingBalanceColumn,
        affiliateTransferColumn
    ];

    var customerWinLoss = {
        customerWinLossCommsColumns: [customerTotalColumn, customerCommColumn, customerWinLossColumn],
        customerWinLossCommsColumnNames: ["Total", "Comm", "WinLoss"],
        customerWinLossColumn: [customerWinLossMergeColumn],
        customerWinLossColumnNames: ["CustomerW_L"]
    };

    var productBA = {
        productBAColumns: [productBAAmountDueToColumn, productBACommColumn, productBATurnoverColumn],
        productBAColumnNames: ["AmountDueTo", "Comm", "Turnover"]
    };

    function insertColumns(columns, insertedColumns, insertedIndex) {
        $.each(insertedColumns, function (index, item) {
            columns.insertAfter(item, insertedIndex);
        });
    };

    var me =
    {
        urlData: 'api/CustomerBalanceReport/Index',
        urlExcelData: 'api/CustomerBalanceReport/Index',
        hasCustomerComm: true,
        isAllowBA: true,
        customerCommIndex: 11,
        productBAIndex: 13,
        totalColspan: 3,
        gridOption:
        {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            autowidth: true,
            forceFit: true,
            shrinkToFit: true,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            userDataOnFooter: false,
            viewrecords: false,
            gridview: true,
            rownumbers: false,
            sortable: false,
            scroll: 1,
            rowNum: 40
        },

        getGridOption: function () {
            me.gridOption.colNames = me.getColName();
            me.gridOption.colModel = me.getColModel();
            return Site.clone(me.gridOption);
        },

        setGroupHeaderColumns: function (grid) {
            var groupCustomerWinloss = me.hasCustomerComm ?
            {
                startColumnName: 'WinLoss',
                numberOfColumns: 3,
                titleText: Site.getSingleResource('CustomerW_L')
            } : {};

            var groupProductBA = me.isAllowBA ?
            {
                startColumnName: 'TurnoverBA',
                numberOfColumns: 3,
                titleText: Site.getSingleResource('ProductBA')
            } : {};

            grid.jqGrid('setGroupHeaders',
            {
                useColSpanStyle: true,
                groupHeaders: [groupCustomerWinloss, groupProductBA]
            });
        },

        getColName: function () {
            var colNames = Site.clone(colNameList);

            if (me.hasCustomerComm) {
                insertColumns(colNames, customerWinLoss.customerWinLossCommsColumnNames, me.customerCommIndex);
            }
            else {
                insertColumns(colNames, customerWinLoss.customerWinLossColumnNames, me.customerCommIndex);
            }

            me.productBAIndex = colNames.length - 1;

            if (me.isAllowBA) {
                insertColumns(colNames, productBA.productBAColumnNames, me.productBAIndex);
            }

            return Site.getResources(colNames);
        },

        getColModel: function () {
            var colModels = Site.clone(colModelList);

            if (me.hasCustomerComm) {
                insertColumns(colModels, customerWinLoss.customerWinLossCommsColumns, me.customerCommIndex);
            }
            else {
                insertColumns(colModels, customerWinLoss.customerWinLossColumns, me.customerCommIndex);
            }

            me.productBAIndex = colModels.length - 1;

            if (me.isAllowBA) {
                insertColumns(colModels, productBA.productBAColumns, me.productBAIndex);
            }

            return colModels;
        },

        getOrderColumnCode: function (indexColumn, sortType) {
            if (sortType === "asc" && indexColumn !== customerWinLossColumn.index && indexColumn !== customerWinLossMergeColumn.index) {
                return 0;
            }
                
            switch (indexColumn) {
                case customerWinLossColumn.index:
                case customerWinLossMergeColumn.index:
                    if (sortType === "desc") {
                        return 4;
                    }
                    return 5;
                case depositColumn.index:
                    return 1;
                case withdrawalColumn.index:
                    return 2;
                case turnoverColumn.index:
                    return 3;
                case openingBalanceColumn.index:
                    return 6;
                case closingBalanceColumn.index:
                    return 7;
                default:
                    return 0;
            }
        }
    }

    return me;
})();