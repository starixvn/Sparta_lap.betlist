"use strict";
define(["jquery"], function ($) {
    var logIdColumn = {
        name: 'LogId',
        index: 'LogId',
        resizable: false,
        width: 50,
        align: 'center',
        sortable: false
    };
    var logDateColumn = {
        name: 'LogDate',
        index: 'LogDate',
        resizable: false,
        width: 60,
        align: 'center',
        valign: 'middle',
        sortable: false,
        formatter: 'date',
        formatoptions:
        {
            srcformat: 'Y-m-d h:i:s A',
            newformat: 'n/j/Y h:i:s A'
        }
    };
    var editedByColumn = {
        name: 'EditedBy',
        index: 'EditedBy',
        resizable: false,
        width: 80,
        align: 'left',
        valign: 'middle',
        sortable: true
    };
    var actionColumn = {
        name: 'ActionName',
        index: 'ActionName',
        resizable: false,
        width: 150,
        align: 'left',
        sortable: false
    };
    var detailColumn = {
        name: 'Details',
        index: 'Details',
        resizable: false,
        width: 600,
        align: 'center',
        title: false,
        sortable: false,
        classes: 'jqgrid-detail-col'
    };

    var colNameList = ['LogID', 'LogDate', 'Editedby', 'Action', 'Details'];
    var colModelList = [logIdColumn, logDateColumn, editedByColumn, actionColumn, detailColumn];

    var _me = {
        gridOption: {
            jsonReader: {
                root: "rows",
                repeatitems: false
            },
            height: "auto",
            url: {},
            datatype: 'json',
            colNames: [],
            colModel: colModelList,
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            viewrecords: true,
            loadui: 'disable',
            autowidth: false,
            forceFit: false,
            shrinkToFit: true,
            scroll: false,
            loadonce: true,
            gridview: true,
            sortorder: "desc",
            pager: '#ppdata'
        },

        getColNames: function () {
            return colNameList
        }
    }

    return _me;;
});