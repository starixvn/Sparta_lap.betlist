define(["jquery", "common/site"],
    function ($, Site) {
        "use strict";

        var externalIdColumn =
        {
            name: 'ExternalId',
            index: 'ExternalId',
            resizable: false,
            width: 200,
            align: 'left',
            sortable: false
        };

        var systemIdColumn =
        {
            name: 'SystemId',
            index: 'SystemId',
            resizable: false,
            width: 200,
            align: 'left',
            sortable: false
        };

        var balanceColumn = {
            name: 'Balance',
            index: 'Balance',
            width: 150,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'number',
            formatteroptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            frozen: true
        };
        var archivedDateColumn =
        {
            name: 'ArchivedDate',
            index: 'ArchivedDate',
            width: 140,
            align: 'center',
            formatter: "date",
            sortable: false,
            resizable: false
        };
        //#endregion
        var colNameList = ['ExternalId', "SystemId", "Balance", 'ArchivedDate'];

        var colModelList = [externalIdColumn, systemIdColumn, balanceColumn, archivedDateColumn];

        var me = {
            urlData: 'Customer/DormantCustomerList',
            gridOption: {
                datatype: 'local',
                colNames: [],
                colModel: [],
                data: [],
                height: 'auto',
                viewrecords: false,
                autowidth: false,
                forceFit: true,
                shrinkToFit: false,
                minWidth: 300,
                loadtext: '',
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

        
        var getColName = function () {
            var temp = Site.clone(colNameList);
            return Site.getResources(temp);
        };

        var getColModel = function () {
            var temp = Site.clone(colModelList);

            return temp;
        };

        me.getGridOption = function () {
            me.gridOption.colNames = getColName();
            me.gridOption.colModel = getColModel();
            return me.gridOption;
        };

        return me;
    });