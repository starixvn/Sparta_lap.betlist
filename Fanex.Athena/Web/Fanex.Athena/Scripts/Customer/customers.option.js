/// <reference path="/@JSense.js" />
/*
* Author:Alex.Nguyen
* Created Date:10/15/2013
*/
//#region Columns
var CustomersOption = (function () {

    var indexColumn = {
        name: 'Index',
        index: 'Index',
        width: 30,
        align: 'center',
        valign: 'middle',
        sortable: false,
        resizable: false,
        frozen: true       
    };


    var groupColumn =
    {
        name: 'GroupName',
        index: 'GroupName',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: true
    };

    var externalIdColumn =
    {
        name: 'ExternalId',
        index: 'ExternalId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: true
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: true
    };
   
    var availableFundColumn = {
        name: 'AvailableFund',
        index: 'AvailableFund',
        width: 65,
        align: 'right',
        sortable: false,
        resizable: false,
        formatter: 'integer',
        frozen: true,
        formatter: jqGridAdditional.formatDecimal
    };
    var pedingFundColumn = {
        name: 'PendindFund',
        index: 'PendindFund',
        resizable: false,
        width: 100,
        align: 'right',
        sortable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal

    };
    var statusColumn =
    {
        name: 'Status',
        index: 'Status',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: true
    };
    var specialLevelColumn = {
        name: 'TotalTurnover',
        index: 'TotalTurnover',
        resizable: false,
        width: 100,
        align: 'right',
        sortable: false,
        frozen: true,
        cellattr: jqGridAdditional.rowSpanSetting

    };
    var customerOptionColumn = {
        name: 'CustomerWinLoss',
        index: 'CustomerWinLoss',
        width: 90,
        align: 'right',
        sortable: false,
        resizable: false,
        frozen: true,
        formatter: jqGridAdditional.formatDecimal

    };
    var commissionColumn =
    {
        name: 'Commission',
        classes: 'highlight',
        index: 'Commission',
        width: 100,
        align: 'right',
        sortable: true,
        resizable: false,
        hidden: false,
        title: true
    };
    //#endregion
    var colNameList = ['#','Group', 'ExternalId', "SystemId", "AvailableFunds", 'PendingFunds', 'SpecialLevel',
    'CustomerOption', 'Commission'];

    var colModelList = [indexColumn, groupColumn, externalIdColumn, systemIdColumn, availableFundColumn, 
                    pedingFundColumn, statusColumn, specialLevelColumn, customerOptionColumn, commissionColumn];
    

    var me = {
        hasCustomerComm: false,
        totalColspan: 6,
        urlData: 'Customer/CustomerList',
        urlExcelData: 'Customer/ExportExcel',
        reportBy: 1, //1:daily ,2 :monthly,
        gridOption: {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: 'auto',
            viewrecords: true,
            autowidth: true,
            forceFit: false,
            shrinkToFit: true,
            minWidth: 800,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            gridview: true,
            footerrow: true,
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

    var getColName = function () {
        // Column Customer Wl
        colNameList[customerWlTotalIndex] = me.hasCustomerComm ? 'Total' : 'CustomerW_L';
        return Site.getResources(colNameList);
    };

    var getColModel = function () {
        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;

        if (me.reportBy == 2) {

            dateColumn.formatoptions = {
                newformat: 'm/Y'
            };
        } else {
            dateColumn.formatoptions = {
                newformat: 'm/d/Y'
            };
        }

        return colModelList;
    };

    me.getGridOption = function () {
        me.gridOption.colNames = getColName();
        me.gridOption.colModel = getColModel();

        return me.gridOption;
    };


    return me;
})();