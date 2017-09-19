define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional", "../common/thirdpartywinloss.option"],
    function ($, Site, jqGridAdditional, ThirdPartyWinLossOption) {
        "use strict";

        var REPORT_BY_DAILY = 1;

        var indexColumn = {
            name: 'Index',
            index: 'Index',
            resizable: false,
            width: 30,
            align: 'center',
            valign: 'middle',
            frozen: true,
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

        var memWinningColumn =
        {
            name: 'MemberWinning',
            index: 'MemberWinning',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        var cashOutColumn =
        {
            name: 'CashOut',
            index: 'CashOut',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        var cashOutShareColumn =
        {
            name: 'CashOutShare',
            index: 'CashOutShare',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        var commissionShareColumn =
        {
            name: 'CommissionShare',
            index: 'CommissionShare',
            width: 100,
            align: 'right',
            sortable: false,
            resizable: false,
            hidden: false,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };

        var amountDueToCBColumn =
        {
            name: 'AmountDueToCB',
            index: 'AmountDueToCB',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        var grandPrizeColumn =
        {
            name: 'GrandPrize',
            index: 'GrandPrize',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
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
          'Turnover', 'TotalTransaction', 'CustomerWinnings', 'CashOutAmt', 'CashOutShare', 'CommissionShare',
          'AmountDueToCB', 'AmtGreaterThan30kGBP', 'CurrencyId', 'CustomerId'];

        var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
            turnoverColumn, totalTransColumn, memWinningColumn, cashOutColumn, cashOutShareColumn, commissionShareColumn,
            amountDueToCBColumn, grandPrizeColumn, currencyIdColumn, customerIdColumn];

        var _me = new ThirdPartyWinLossOption({
            TotalColspan: [2, 3],
            UrlData: 'api/ColossusBetsWinlossReport/Index',
            UrlExcelData: 'api/ColossusBetsWinlossReport/Index',
            ColNameList: colNameList,
            ColModelList: colModelList
        });

        _me.getColModel = function (level, reportBy) {
            level = Number(level);

            currencyColumn.formatter = level === 0 ? function (cellvalue, options, rowObject) {
                var level = 1;
                return _me.currencyFormatter(cellvalue, options, rowObject, level);
            } : null;

            dateColumn.hidden = level > 0;
            dateColumn.formatter = function (cellvalue, options, rowObject) {
                cellvalue = $.fn.fmatter.call(this, "date", cellvalue, options, rowObject);
                return _me.setDrillDownLink(1, 0, 0, rowObject.Date, cellvalue);
            };
            dateColumn.formatoptions =
            {
                newformat: parseInt(reportBy) === REPORT_BY_DAILY ? 'm/d/Y' : 'm/Y'
            };

            externalIdColumn.hidden = level === 0;
            systemIdColumn.hidden = level === 0;

            if (level === 1) {
                externalIdColumn.formatter = _me.betlistLink;
                systemIdColumn.formatter = _me.betlistLink;
            }
            return ThirdPartyWinLossOption.prototype.getColModel.call(this);
        };

        _me.setGroupHeader = function ($grid) {
            var group = {
                startColumnName: 'MemberWinning',
                numberOfColumns: 2,
                titleText: Site.getSingleResource('AmtLessThan30kGBP')
            };

            $grid.jqGrid('setGroupHeaders',
            {
                useColSpanStyle: true,
                groupHeaders: [group]
            });
        };

        return _me;
    });