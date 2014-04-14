/*global Site */
/*exported CustomersOption */
/// <reference path="/@JSense.js" />
/*
* Author:Alex.Nguyen
* Created Date:10/15/2013
*/
//#region Columns
var CustomersOption = (function () {
    "use strict";

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        valign: 'middle',
        sortable: false
    };

    var groupColumn =
    {
        name: 'Group',
        index: 'Group',
        resizable: false,
        width: 90,
        align: 'left',
        sortable: false,
        formatter: function (cellValue, options, rowObject) {
            if (Number(rowObject.GroupCommId) === 0) {
                cellValue = Site.getSingleResource("None");
            }

            return "<div id='groupName-" + rowObject.CustId + "' style='background-color:" + rowObject.GroupColor +
                     "' class='commission-color'>" + cellValue + "</div>";
        }

    };

    var externalIdColumn =
    {
        name: 'ExternalId',
        index: 'ExternalId',
        resizable: false,
        width: 110,
        align: 'left',
        sortable: false
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: false
    };

    var availableFundColumn = {
        name: 'AvailableFunds',
        index: 'AvailableFunds',
        width: 100,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'number',
        formatteroptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        frozen: true
    };
    var pedingFundColumn = {
        name: 'PendingFunds',
        index: 'PendindFunds',
        resizable: false,
        width: 100,
        align: 'right',
        sortable: false,
        frozen: true,
        formatter: 'number',
        formatteroptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' }
    };
    var statusColumn =
    {
        name: 'Status',
        index: 'Status',
        resizable: false,
        width: 80,
        align: 'center',
        sortable: false,
        formatter: function (cellValue, options, rowObject) {
            return "<span custid='" + rowObject.CustId + "' class='link cust-status-option' issuspended='" +
                rowObject.IsSuspended + "'  isclosed='" + rowObject.IsClosed + "' externalid='" + rowObject.ExternalId +
                "' id='edit-cust-status-" + rowObject.CustId + "'>" + cellValue +
                "<span class='arrow1' title='" + Site.getSingleResource("Status") + "'>&nbsp;&nbsp;&nbsp;</span></span>";
        }
    };
    var baStatusColumn =
    {
        name: 'BaStatus',
        index: 'BaStatus',
        resizable: false,
        width: 80,
        align: 'left',
        sortable: false,
        formatter: function (cellValue, options, rowObject) {
            var title = Site.getSingleResource(cellValue ? "TurnOffBaTooltip" : "TurnOnBaTooltip");
            return "<div title='" + title + "' data-val='#" + rowObject.CustId + "#" + (!cellValue) + "#" + rowObject.ExternalId +
                    "' class='ba-" + (cellValue ? "on" : "off") + "'></div>";
        }
    };
    var specialLevelColumn = {
        name: 'SpecialLevel',
        index: 'SpecialLevel',
        resizable: false,
        width: 90,
        align: 'right',
        sortable: false,
        frozen: true,
        classes: "specialLevel",
        formatter: function (cellValue, options, rowObject) {
            return "<div data-custid='" + rowObject.CustId + "'  data-level='" + cellValue + "' class='level-" + cellValue +
                     "' style='text-align: center;color:black;width:80px;float:left'>" + cellValue +
                     "</div><span style='float:right' class='arrow1' title='" + Site.getSingleResource("SpecialLevel") + "'>&nbsp;&nbsp;&nbsp;</span></span>";
        }

    };
    var customerOptionColumn = {
        name: 'CustomerOption',
        index: 'CustomerOption',
        width: 80,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: function (cellValue, options, rowObject) {
            return "<div systemid='" + rowObject.SystemId + "' externalid='" + rowObject.ExternalId + "' custid='" + rowObject.CustId +
            "' title='" + Site.getSingleResource("CustomerOption") +
            "' class='link cust-option-link' id='cust-option-link-" + rowObject.CustId + "'>" + Site.getSingleResource("View") +
            "<img alt='' style='width:10px;height:10px' src='" + Site.resolveClientUrl("Content/MemberInfo/images/arrow.gif?") + "'> </div>";

        }

    };
    var commissionColumn =
    {
        name: 'Commission',
        index: 'Commission',
        width: 40,
        align: 'right',
        sortable: false,
        resizable: false,
        title: true,
        formatter: 'number',
        formatteroptions: { decimalSeparator: ".", decimalPlaces: 2, defaultValue: '0.00' }
    };
    //#endregion
    var colNameList = ['#', 'ExternalId', "SystemId", "AvailableFunds", 'PendingFunds',
     'Status', 'BaStatus', 'SpecialLevel', 'CustomerOption'];

    var colModelList = [indexColumn, externalIdColumn, systemIdColumn, availableFundColumn,
                    pedingFundColumn, statusColumn, baStatusColumn, specialLevelColumn, customerOptionColumn];


    var me = {
        urlData: 'Customer/CustomerList',
        urlExcelData: 'Customer/ExportExcel',
        gridOption: {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            viewrecords: false,
            autowidth: false,
            forceFit: false,
            shrinkToFit: false,
            minWidth: 800,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            gridview: true,
            footerrow: false,
            userDataOnFooter: true,
            cmTemplate: { title: true }, //enable tooltip
            userData: [],
            beforeSelectRow: function () {
                return false;
            },
            onRightClickRow: function () {
                return false;
            }
        }
    };

    var products = ["Rb", "Ng", "Lc", "Cs"];
    var numOfProductColumns = 0;

    var getColName = function (productAccess, isAllowedCustomerCommission) {
        numOfProductColumns = 1;
        var temp = Site.clone(colNameList);
        if (isAllowedCustomerCommission) {
            temp.insertAfter("Group", 0);
            temp.push("SB");

            $.each(products, function (index, value) {
                if (productAccess["IsAllow" + value]) {
                    numOfProductColumns++;
                    temp.push(value.toUpperCase());
                }
            });
        }

        return Site.getResources(temp);
    };

    var getColModel = function (productAccess, isAllowedCustomerCommission) {        
        var temp = Site.clone(colModelList);
        if (isAllowedCustomerCommission) {

            var getCellAttrFunction = function (colName) {
                return function (rowId, val, rowObj) {
                    return " id='" + colName + "-" + rowObj.CustId + "' ";
                };
            };

            var col = Site.clone(groupColumn);
            temp.insertAfter(col, 0);

            col = Site.clone(commissionColumn);
            col.name = "SbComm";
            col.index = col.name;
            col.cellattr = getCellAttrFunction(col.name);
            temp.push(col);

            $.each(products, function (index, value) {
                if (productAccess["IsAllow" + value]) {
                    col = Site.clone(commissionColumn);
                    col.name = value + "Comm";
                    col.index = col.name;
                    col.cellattr = getCellAttrFunction(col.name);
                    temp.push(col);
                }
            });
        }

        return temp;
    };

    me.setGroupHeaderColumns = function (grid) {
        grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [{
                startColumnName: "SbComm",
                numberOfColumns: numOfProductColumns,
                titleText: Site.getSingleResource('Commission') + " (%)"
            }]
        });
    };

    me.getGridOption = function (productAccess, isAllowedCustomerCommission) {
        me.gridOption.colNames = getColName(productAccess, isAllowedCustomerCommission);
        me.gridOption.colModel = getColModel(productAccess, isAllowedCustomerCommission);

        return me.gridOption;
    };


    return me;
})();