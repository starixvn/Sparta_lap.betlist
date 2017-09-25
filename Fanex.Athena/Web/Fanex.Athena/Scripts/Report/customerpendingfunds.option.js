/*global Site, jqGridAdditional*/
/*exported CustPendingFundsOption*/
/// <reference path="/@JSense.js" />

var CustPendingFundsOption = (function () {
    "use strict";

    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
        {
            CustId: rowObject.CustId,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        };

        return '<a class="bet-list-link" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
    };

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
        width: 200,
        align: "left",
        valign: "middle",
        sortable: true,
        formatter: betlistLink,
        cellattr: function () {
            return ' style="text-overflow: ellipsis"';
        }
    };

    var systemIdColumn =
    {
        name: "SystemId",
        index: "SystemId",
        resizable: false,
        width: 200,
        align: "left",
        valign: "middle",
        sortable: true
    };

    var betCountsColumn =
    {
        name: "BetCounts",
        index: "BetCounts",
        width: 120,
        align: "right",
        sortable: true,
        resizable: false,
        formatter: "integer",
        formatoptions:
        {
            thousandsSeparator: ","
        }
    };

    var pendingFundsColumn =
    {
        name: "PendingFunds",
        index: "PendingFunds",
        width: 115,
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
        "BetCounts",
        "PendingFunds"
    ];

    var colModelList = [
        indexColumn,
        externalIdColumn,
        systemIdColumn,
        betCountsColumn,
        pendingFundsColumn
    ];

    var me =
    {
        urlData: "api/CustomerPendingFundsReport/Index",
        urlExcelData: "api/CustomerPendingFundsReport/Index",
        totalColspan: 3,
        gridOption:
        {
            datatype: "local",
            colNames: [],
            colModel: [],
            data: [],
            height: "100%",
            autowidth: true,
            forceFit: false,
            shrinkToFit: true,
            loadtext: '',
            loadui: "disable",
            loadonce: true,
            footerrow: true,
            userDataOnFooter: false,
            gridview: true,
            rownumbers: false,
            sortable: false
        },

        getGridOption: function (totalRows) {
            me.gridOption.colNames = me.getColName();
            me.gridOption.colModel = me.getColModel(totalRows);
            return Site.clone(me.gridOption);
        },

        getColName: function () {
            return Site.getResources(colNameList);
        },

        getColModel: function (totalRows) {
            var colModels = Site.clone(colModelList);

            if (totalRows <= 1) {
                $.each(colModels, function (index, item) {
                    item.sortable = false;
                });
            }

            return colModels;
        },
    };

    return me;
})();