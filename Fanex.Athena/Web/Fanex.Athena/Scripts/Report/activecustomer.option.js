/*global Site, JSLINQ, jqGridAdditional*/
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
        name: 'productName',
        index: 'productName',
        width: 90,
        sortable: true,
        resizable: false,
        align: "right",
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    //#endregion

    var colNameList = ['Month', 'Total'];

    var colModelList = [dateColumn, numMemberColumn];

    var me =
    {
        licenseeId: 0,
        dateColumnIndex: 0,
        turnoverIndex: 1,
        pageSize: 50,
        nonePagingSize: 5000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
        totalColspan: [2, 2, 3],
        urlData: 'api/report/ActiveCustomer',
        urlExcelData: 'api/report/ActiveCustomer',
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
        var num = JSLINQ(productOptions).Count(function (item) {
            return item.Status === true;
        });
        var last = JSLINQ(productOptions).Last(function (item) {
            return item.Status === true;
        });

        grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [{
                startColumnName: "CustomerCount" + last.Code,
                numberOfColumns: num,
                titleText: Site.getSingleResource('Product')
            }]
        });
    };

    me.getColName = function (productOptions) {
        var temp = Site.clone(colNameList);
        var turnOverIndex = me.turnoverIndex;

        $.each(productOptions, function (index, value) {
            if (value.Status === true) {
                temp.insertAfter(value.Name, me.dateColumnIndex);
                turnOverIndex++;
            }
        });

        return Site.getResources(temp);
    };

    me.getColModel = function (reportBy, productOptions) {
        reportBy = Number(reportBy);

        var temp = Site.clone(colModelList);

        var turnOverIndex = me.turnoverIndex;

        $.each(productOptions, function (index, value) {
            //insert turn over column

            if (value.Status === true) {
                var column = Site.clone(productColumn);
                column.name = "CustomerCount" + value.Code;
                column.index = column.name;
                temp.insertAfter(column, me.dateColumnIndex);
                turnOverIndex++;
            }
        });

        return temp;
    };

    me.getGridOption = function (reportBy, productOptions) {
        me.gridOption.colNames = me.getColName(productOptions);
        me.gridOption.colModel = me.getColModel(reportBy, productOptions);
        return Site.clone(me.gridOption);
    };

    return me;
})();