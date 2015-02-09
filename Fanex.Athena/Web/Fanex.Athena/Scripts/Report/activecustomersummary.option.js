/*global Site*/
/*exported ActiveCustomerOption*/
/// <reference path="/@JSense.js" />

var ActiveCustomerOption = (function () {
    "use strict";
    //#region Columns

    var dateColumn =
    {
        name: 'TransactionDate',
        index: 'TransactionDate',
        resizable: false,
        width: 70,
        align: 'center',
        valign: 'middle',
        sortable: true,
        formatter: 'date',
        formatoptions:
        {
            newformat: 'm/Y'
        }
    };

    var numMemberColumn =
    {
        name: 'TotalCustomer',
        index: 'TotalCustomer',
        width: 80,
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

    var productColumn =
    {
        width: 90,
        sortable: true,
        resizable: false,
        align: "right",
        title: true,
        formatter: 'integer',
        formatoptions:
        {
            thousandsSeparator: ","
        }
    };

    //#endregion

    var colNameList = ['Month', 'Total'];

    var colModelList = [dateColumn, numMemberColumn];

    var me =
    {
        dateColumnIndex: 0,
        totalCustomerIndex: 1,
        pageSize: 50,
        nonePagingSize: 5000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
        totalColspan: [2, 2, 3],
        urlData: 'api/report/ActiveCustomerSummary',
        urlExcelData: 'api/report/ActiveCustomerSummary',
        gridOption:
        {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: '100%',
            autowidth: true,
            forceFit: false,
            shrinkToFit: false,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            userDataOnFooter: true,
            userData: [],
            viewrecords: true,
            gridview: true,
            rownumbers: false
        }
    };

    me.setGroupHeaderColumns = function (grid, productOptions) {
        grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [{
                startColumnName: "CustomerCount" + productOptions[0].Code,
                numberOfColumns: productOptions.length,
                titleText: Site.getSingleResource('Product')
            }]
        });
    };

    me.getGridOption = function (productOptions) {

        var columnModels = Site.clone(colModelList);
        var columnNames = Site.clone(colNameList);
        var startIndex = me.dateColumnIndex;

        $.each(productOptions, function (index, value) {
            // columnNames
            columnNames.insertAfter(value.Name, startIndex);

            // columModels
            var column = Site.clone(productColumn);
            column.name = "CustomerCount" + value.Code;
            column.index = column.name;
            if (!value.Status) {
                column.formatter = function () {
                    return "-";
                };
            }

            columnModels.insertAfter(column, startIndex);
            startIndex++;
        });

        me.gridOption.colNames = Site.getResources(columnNames);
        me.gridOption.colModel = columnModels;

        return Site.clone(me.gridOption);
    };

    return me;
})();