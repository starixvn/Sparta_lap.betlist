/*global Site, jqGridAdditional*/
/*exported CustomerWinLossOption*/
/// <reference path="/@JSense.js" />

var CustomerWinLossOption = (function () {
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
        sortable: true,
        cellattr: function () {
            return ' style="text-overflow: ellipsis"';
        },
        formatter: function (cellvalue, options, rowObject) {
            return '<a class="bet-list-link" href="javascript:;" data-cust-id=' + rowObject.CustId +
                ' data-system-id=' + rowObject.SystemId +
                ' data-external-id=' + rowObject.ExternalId + '>' + cellvalue + '</a>';
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
        sortable: true
    };

    var turnoverColumn =
    {
        name: "Turnover",
        index: "Turnover",
        width: 110,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var totalTransactionColumn =
    {
        name: "TotalTransaction",
        index: "TotalTransaction",
        width: 90,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var customerWlColumn =
   {
       name: 'CustomerWL',
       index: 'CustomerWL',
       width: 90,
       align: 'right',
       sortable: true,
       resizable: false,
       formatter: jqGridAdditional.formatDecimal
   };

    var playerCommColumn =
    {
        name: 'PlayerComm',
        index: 'PlayerComm',
        width: 50,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var customerWlTotalColumn =
    {
        name: 'CustomerTotalWL',
        index: 'CustomerTotalWL',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var betEffectiveColumn =
    {
        name: "BetEffective",
        index: "BetEffective",
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var licenseeWLColumn =
    {
        name: "LicenseeWL",
        index: "LicenseeWL",
        classes: 'highlight',
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var bacommColumn =
    {
        name: "BAComm",
        index: "BAComm",
        classes: 'highlight',
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var racingRevenueColumn =
    {
        name: "RacingRevenue",
        index: "RacingRevenue",
        classes: 'highlight',
        width: 100,
        align: "right",
        sortable: false,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var amtDueToBAColumn =
    {
        name: "AmtDueToBA",
        index: "AmtDueToBA",
        width: 100,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var amtDueToRacingColumn =
    {
        name: "AmtDueToRacing",
        index: "AmtDueToRacing",
        width: 90,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "currency",
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var colNameList = [
        "#",
        "ExternalId",
        "SystemId",
        "Turnover",
        "TotalTransaction",
        '@[Win]/@[Loss]',
        'Comm',
        'Total',
        "EffectiveBettingValue",
        "LicenseeW_L",
        "BACommission",
        "RacingRevenue",
        "AmtDueToBA",
        "AmtDueToRacing"
    ];

    var colModelList = [
        indexColumn,
        externalIdColumn,
        systemIdColumn,
        turnoverColumn,
        totalTransactionColumn,
        customerWlColumn, 
        playerCommColumn,
        customerWlTotalColumn,
        betEffectiveColumn,
        licenseeWLColumn,
        bacommColumn,
        racingRevenueColumn,
        amtDueToBAColumn,
        amtDueToRacingColumn
    ];

    var me =
    {
        urlData: "api/CustomerBalanceReport/Index",
        urlExcelData: "api/CustomerBalanceReport/Index",
        hasCustomerComm: true,
        isAllowBA: true,
        customerCommIndex: 11,
        productBAIndex: 13,
        totalColspan: 3,
        gridOption:
        {
            datatype: "local",
            colNames: [],
            colModel: [],
            data: [],
            height: "auto",
            autowidth: true,
            forceFit: true,
            shrinkToFit: true,
            loadtext: '',
            loadui: "disable",
            loadonce: true,
            footerrow: true,
            userDataOnFooter: false,
            viewrecords: false,
            gridview: true,
            rownumbers: false,
            sortable: false,
            scroll: 1,
            rowNum: 100
        },

        getGridOption: function (totalRows) {
            me.gridOption.colNames = me.getColName();
            me.gridOption.colModel = me.getColModel(totalRows);
            return Site.clone(me.gridOption);
        },

        setGroupHeaderColumns: function (grid) {
            var groupCustomerWinloss = me.hasCustomerComm ?
            {
                startColumnName: "CustomerWL",
                numberOfColumns: 3,
                titleText: Site.getSingleResource("CustomerW_L")
            } : {};

            grid.jqGrid("setGroupHeaders",
            {
                useColSpanStyle: true,
                groupHeaders: [groupCustomerWinloss]
            });
        },

        getColName: function () {
            var colNames = Site.clone(colNameList);

            return Site.getResources(colNames);
        },

        getColModel: function (totalRows) {
            customerWlColumn.hidden = !me.hasCustomerComm;
            playerCommColumn.hidden = !me.hasCustomerComm;
            var colModels = Site.clone(colModelList);

            if (totalRows <= 1) {
                $.each(colModels, function (index, item) {
                    item.sortable = false;
                });
            }

            return colModels;
        }
    };

    return me;
})();