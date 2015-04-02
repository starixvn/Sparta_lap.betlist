/*global Site*/
/*exported CustomerOption*/
/// <reference path="/@JSense.js" />
"use strict";
var CustomerOption = (function () {
    var setLink = function (rowObject) {
        return rowObject.TransTypeHasBetList ? '<a  class=\'link\' href="javascript:OutstandingBetList.viewDailyStatementBetList' +
                '(\'' + rowObject.CustId + '\',\'' + rowObject.Date + '\',\'' + rowObject.TransType + '\',\'' +
                rowObject.ExternalId + '\',\'' + rowObject.SystemId + '\');">' +
         rowObject.FullStatementDate + '</a>' : rowObject.FullStatementDate;
    };

    var columnNames = ['#', 'Date', 'Remark', 'Turnover', 'CreditDebit', 'Commission', 'Balance'];

    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        sortable: false,
        resizable: false
    };
    var date =
    {
        name: 'Date',
        index: 'Date',
        width: 150,
        align: 'center',
        sortable: false,
        formatter: function (cellValue, option, rowObject) {
            return setLink(rowObject);
        }
    };
    var remark =
    {
        name: 'Remark',
        index: 'Remark',
        width: 205,
        align: 'right',
        sortable: false
    };
    var turnover =
    {
        name: 'Turnover',
        index: 'Turnover',
        width: 120,
        align: 'right',
        sortable: false
    };

    var creditDebit =
    {
        name: 'CreditDebit',
        index: 'CreditDebit',
        width: 120,
        align: 'right',
        sortable: false
    };

    var commission =
    {
        name: 'Commission',
        index: 'Commission',
        width: 120,
        align: 'right',
        sortable: false,
        resizable: false
    };
    var balance =
    {
        name: 'Balance',
        index: 'Balance',
        width: 120,
        align: 'right',
        sortable: false
    };

    var columnModel = [indexColumn, date, remark, turnover, creditDebit, commission, balance];

    return {
        gridOption:
        {
            datatype: 'local',
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            pager: '#pager2',
            loadtext: "Loading...",
            viewrecords: true,
            autowidth: true,
            shrinkToFit: true,
            scroll: false,
            loadonce: true,
            gridview: true,
            userDataOnFooter: true,
            jsonReader: { repeatitems: false },
            cmTemplate: { sortable: false },
            beforeSelectRow: function () {
                return false;
            },
            onRightClickRow: function () {
                return false;
            },
            onPaging: function (/*pgButton*/) {
                Site.blockUI();
            }
        },
        columnModel: columnModel,
        columnNames: Site.getResources(columnNames),
        maxHeight: 720
    };
})();