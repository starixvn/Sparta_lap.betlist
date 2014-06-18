/*global Site*/
/*exported LogInLogOption*/
/// <reference path="/@JSense.js" />
"use strict";
var LogInLogOption = (function () {
    var userNameColumn = {
        name: 'UserName',
        index: 'UserName',
        resizable: false,
        width: 150,
        align: 'left',
        valign: 'middle',
        sortable: true
    };
    var createdDateColumn = {
        name: 'CreatedDate',
        index: 'CreatedDate',
        resizable: false,
        width: 100,
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
    var lastTxnColumn = {
        name: 'LastTxn',
        index: 'LastTxn',
        resizable: false,
        width: 100,
        align: 'center',
        sortable: false,
        formatter: 'date',
        formatoptions:
        {
            srcformat: 'Y-m-d h:i:s A',
            newformat: 'n/j/Y h:i:s A'
        }
    };

    var colNameList = ['Username', 'CreatedDate', 'LastTxn'];
    var colModelList = [userNameColumn, createdDateColumn, lastTxnColumn];

    var gridOption = {
        jsonReader: {
            root: "rows",
            repeatitems: false
        },
        rownumbers: true,
        height: 650,
        url: {},
        datatype: 'json',
        colNames: colNameList,
        colModel: colModelList,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
        viewrecords: true,
        loadtext: 'loading...',
        loadui: 'disable',
        autowidth: true,
        scroll: false,
        loadonce: true,
        gridview: true,
        sortorder: "desc",
        pager: null,
        beforeSelectRow: function () {
            return false;
        },
        onRightClickRow: function () {
            return false;
        }
    };

    return {
        gridOption: gridOption,
        getColumnNames: function () {
            return Site.getResources(colNameList);
        },
        getColModels: function () {
            return colModelList;
        }
    };
})(jQuery);