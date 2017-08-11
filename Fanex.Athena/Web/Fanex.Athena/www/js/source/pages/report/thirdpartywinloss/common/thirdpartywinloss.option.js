define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional"],
    function ($, Site, jqGridAdditional) {
        "use strict";
        var LICENSEE_12BET = 1;
        var REPORT_BY_DAILY = 1;

        var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
            var param = {
                Level: level,
                CurrencyId: currencyId,
                CustId: custId,
                Datetime: datetime
            }

            return '<a class="drilldown" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
        };

        var currencyFormatter = function (cellvalue, options, rowObject) {
            var level = _me.licenseeId === LICENSEE_12BET ? 1 : 2;
            return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
        };

        var systemIdFormatter = function (cellvalue, options, rowObject) {
            return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
        };

        var indexColumn =
        {
            name: 'Index',
            index: 'Index',
            resizable: false,
            width: 30,
            align: 'center',
            valign: 'middle',
            frozen: true,
            cellattr: jqGridAdditional.rowSpanSetting,
            sortable: false
        };

        var dateColumn =
        {
            name: 'Date',
            index: 'Date',
            resizable: false,
            width: 55,
            align: 'center',
            valign: 'middle',
            sortable: false,
            frozen: true,
            cellattr: jqGridAdditional.rowSpanSetting,
            formatter: 'date',
            formatoptions:
            {
                newformat: 'm/d/Y'
            }
        };

        var externalIdColumn =
        {
            name: 'ExternalId',
            index: 'ExternalId',
            resizable: false,
            width: 120,
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

        var currencyColumn =
        {
            name: 'CurrencyName',
            index: 'CurrencyName',
            width: 60,
            align: 'center',
            sortable: false,
            resizable: false
        };

        var turnoverColumn =
        {
            name: 'TurnOver',
            index: 'TurnOver',
            width: 75,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            hidden: false,
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        var totalTransColumn =
        {
            name: 'TotalTransaction',
            index: 'TotalTransaction',
            width: 75,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'integer',
            frozen: true,
            formatoptions:
            {
                thousandsSeparator: ","
            }
        };

        var customerWlColumn =
        {
            name: 'CustomerWinLoss',
            index: 'CustomerWinLoss',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        var licenseeWlColumn =
        {
            name: 'LicenseeWL',
            index: 'LicenseeWL',
            width: 100,
            align: 'right',
            sortable: false,
            resizable: false,
            hidden: false,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };

        var currencyIdColumn =
        {
            name: 'CurrencyId',
            index: 'CurrencyId',
            width: 2,
            align: 'center',
            sortable: false,
            resizable: false,
            hidden: true
        };

        var customerIdColumn =
        {
            name: 'CustomerId',
            index: 'CustomerId',
            width: 2,
            align: 'center',
            sortable: false,
            resizable: false,
            hidden: true
        };

        var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
            'Turnover', 'TotalTransaction', 'CustomerW_L', 'LicenseeW_L', 'CurrencyId', 'CustomerId'];

        var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
            turnoverColumn, totalTransColumn, customerWlColumn, licenseeWlColumn, currencyIdColumn, customerIdColumn];

        var _me =
        {
            canDrilldown: true,
            licenseeId: 0,
            totalColspan: [2, 2, 3],
            urlData: 'api/ThirdPartyWinLossReport/Index',
            urlExcelData: 'api/ThirdPartyWinLossReport/Index',
            currencyColumnIndex: 4,
            gridOption:
            {
                datatype: 'local',
                colNames: [],
                colModel: [],
                data: [],
                userDataOnFooter: true,
                userData: [],
                height: '100%',
                autowidth: false,
                forceFit: false,
                shrinkToFit: true,
                loadtext: '',
                loadui: 'disable',
                loadonce: true,
                footerrow: true,
                rowNum: 50000,
                grouping: true,
                viewrecords: true,
                gridview: true,
                rownumbers: false
            },

            getColName: function () {
                return Site.getResources(colNameList);
            },

            getColModel: function (level, reportBy, canDrilldown) {
                level = Number(level);

                if (canDrilldown) {
                    currencyColumn.formatter = level === 0 ? currencyFormatter : null;
                    systemIdColumn.formatter = level === 1 ? systemIdFormatter : null;

                    dateColumn.hidden = level > 0;
                }

                dateColumn.formatoptions =
                {
                    newformat: parseInt(reportBy) === REPORT_BY_DAILY ? 'm/d/Y' : 'm/Y'
                };
                externalIdColumn.hidden = level < 2;
                systemIdColumn.hidden = level === 0;

                return Site.clone(colModelList);
            },
            getGridOption: function (level, reportByDaily, canDrilldown) {
                var gridOption = Site.clone(_me.gridOption);
                gridOption.colNames = _me.getColName();
                gridOption.colModel = _me.getColModel(level, reportByDaily, canDrilldown);
                return gridOption;
            },

            getTotalColModel: function (level, reportByDaily) {
                var colModel = _me.getColModel(level, reportByDaily);
                // Not show drill down link on total link
                colModel[_me.currencyColumnIndex].formatter = null;

                return colModel;
            }
        };
        return _me;
    });