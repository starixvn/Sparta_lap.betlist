define(["jquery", "lib/jqgrid/jquery.jqgrid.additional", "../common/thirdpartywinloss.option"],
    function ($, jqGridAdditional, ThirdPartyWinLossOption) {
        "use strict";

        var LICENSEE_12BET = 1;
        var REPORT_BY_DAILY = 1;

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

        var _me = new ThirdPartyWinLossOption({
            TotalColspan: [2, 2, 3],
            UrlData: 'api/ThirdPartyWinLossReport/Index',
            UrlExcelData: 'api/ThirdPartyWinLossReport/Index',
            ColNameList: colNameList,
            ColModelList: colModelList
        });

        _me.getColModel = function (level, reportBy) {
            level = Number(level);
            dateColumn.hidden = level > 0;
            dateColumn.formatoptions =
            {
                newformat: parseInt(reportBy) === REPORT_BY_DAILY ? 'm/d/Y' : 'm/Y'
            };

            currencyColumn.formatter = level === 0 ? function (cellvalue, options, rowObject) {
                var level = _me.licenseeId === LICENSEE_12BET ? 1 : 2;
                return _me.currencyFormatter(cellvalue, options, rowObject, level);
            } : null;
            systemIdColumn.formatter = level === 1 ? _me.systemIdFormatter : null;

            externalIdColumn.hidden = level < 2;
            systemIdColumn.hidden = level === 0;
            return ThirdPartyWinLossOption.prototype.getColModel.call(this);
        };

        return _me;
    });