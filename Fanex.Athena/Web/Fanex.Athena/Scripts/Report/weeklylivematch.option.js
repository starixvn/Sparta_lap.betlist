/*global Site*/
/*exported WeeklyLiveMatchOption*/
/// <reference path="/@JSense.js" />

var WeeklyLiveMatchOption = (function () {
    "use strict";

    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        resizable: false,
        width: 70,
        align: 'center',
        valign: 'middle',
        sortable: false
    };

    var sportNameColumn =
    {
        name: 'SportName',
        index: 'SportName',
        width: 80,
        align: 'left',
        sortable: false,
        resizable: false
    };

    var numOfMatchColumn =
    {
        width: 110,
        sortable: false,
        resizable: false,
        align: "right",
        title: true,
        formatter: 'integer',
        formatoptions:
        {
            thousandsSeparator: ","
        }
    };

    var colNameList = ['#', 'Sport'];

    var colModelList = [indexColumn, sportNameColumn];

    var me =
    {
        urlData: 'api/WeeklyLiveMatchReport/index',
        urlExcelData: 'api/WeeklyLiveMatchReport/index',
        gridOption:
        {
            rowNum: 500,
            datatype: 'local',
            height: '100%',
            autowidth: false,
            forceFit: true,
            shrinkToFit: true,
            minWidth: 450,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            userDataOnFooter: true,
            viewrecords: true,
            gridview: true,
            rownumbers: false,
            localReader: {
                repeatitems: true,
                cell: "Cell",
                id: "Index",
            },
        }
    };

    me.getGridOption = function (numOfWeek, weekHeaders) {
        var columnModels = Site.clone(colModelList);
        var columnNames = Site.clone(colNameList);
        for (var i = 0; i < numOfWeek; i++) {
            columnNames.push(weekHeaders[i]);

            var column = Site.clone(numOfMatchColumn);
            column.name = "Week" + i;
            column.index = column.name;

            columnModels.push(column);
        }

        me.gridOption.colNames = Site.getResources(columnNames);
        me.gridOption.colModel = columnModels;

        return Site.clone(me.gridOption);
    };

    return me;
})();