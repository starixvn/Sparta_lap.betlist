"use strict";
define(["jquery", "common/site"], function ($, Site) {
    var setLink = function (rowObject) {
        var drilldownParam = {
            CustId: rowObject.CustId,
            Date: rowObject.Date,
            TransType: rowObject.TransType,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        }

        return rowObject.TransTypeHasBetList ?
            '<a href="javascript:;" class="betlistDrilldown" data-param="' + JSON.stringify(drilldownParam) + '"></a>'
            : rowObject.FullStatementDate;
    };

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        sortable: false,
        resizable: false
    };
    var date = {
        name: 'Date',
        index: 'Date',
        width: 150,
        align: 'center',
        sortable: false,
        formatter: function (cellValue, option, rowObject) {
            return setLink(rowObject);
        }
    };
    var remark = {
        name: 'Remark',
        index: 'Remark',
        width: 205,
        align: 'right',
        sortable: false
    };
    var turnover = {
        name: 'Turnover',
        index: 'Turnover',
        width: 120,
        align: 'right',
        sortable: false
    };

    var creditDebit = {
        name: 'CreditDebit',
        index: 'CreditDebit',
        width: 120,
        align: 'right',
        sortable: false
    };

    var commission = {
        name: 'Commission',
        index: 'Commission',
        width: 120,
        align: 'right',
        sortable: false,
        resizable: false
    };
    var balance = {
        name: 'Balance',
        index: 'Balance',
        width: 120,
        align: 'right',
        sortable: false
    };

    var columnNames = ['#', 'Date', 'Remark', 'Turnover', 'CreditDebit', 'Commission', 'Balance'];
    var columnModel = [indexColumn, date, remark, turnover, creditDebit, commission, balance];

    var _me = {
        gridOption: {
            datatype: 'local',
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            colModel: columnModel,
            colNames: [],
            pager: '#pager2',
            viewrecords: true,
            autowidth: false,
            shrinkToFit: true,
            scroll: false,
            loadonce: true,
            gridview: true,
            userDataOnFooter: true,
            jsonReader: { repeatitems: false },
            cmTemplate: { sortable: false },
            onPaging: function () {
                Site.blockUI();
            }
        },
        getColNames: function () {
            return Site.getResources(columnNames);
        }
    };

    return _me;
});