/*global Site, JSLINQ, jqGridAdditional*/
/// <reference path="/@JSense.js" />
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var LicenseeWinLossOption = (function () {
    "use strict";
    var IGNORED_LICENSEEWL_PRODUCTS = ["RB", "BA"];
    var IGNORED_TURNOVER_PRODUCTS = ["BT"];

    // store state of toggle() function
    var _expanseTypes = {};

    var setDrillDownLink = function (level, currencyId, custId, datetime, currencyName, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return LicenseeWinLoss.drillDown(' + level + ',' +
         currencyId + ',' + custId + ',\'' + datetime + '\',\'' + currencyName + '\');">' + cellvalue + '</a>';
    };

    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = Number(LicenseeWinLossOption.licenseeId) === 1 ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, rowObject.CurrencyName, cellvalue);
    };

    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, rowObject.CurrencyName, cellvalue);
    };

    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
        {
            DrillDownFromDate: rowObject.Date,
            DrillDownToDate: rowObject.Date,
            DrillDownCurrencyId: rowObject.CurrencyId,
            DrillDownCurrencyName: rowObject.CurrencyName,
            CustomerId: rowObject.CustomerId,
            ExternalId: rowObject.ExternalId,
            SystemId: rowObject.SystemId
        };

        return '<a class="bet-list-link" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
    };

    //#region Columns

    var indexColumn =
    {
        name: 'Index',
        index: 'Index',
        resizable: false,
        width: 30,
        align: 'center',
        valign: 'middle',
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
        sortable: true,
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
        sortable: true
    };

    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        resizable: false,
        width: 120,
        align: 'left',
        sortable: true,
        formatter: function (cellvalue, options, rowObject) {
            return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
        }
    };

    var currencyColumn =
    {
        name: 'CurrencyName',
        index: 'CurrencyName',
        width: 60,
        align: 'center',
        sortable: true,
        resizable: false,
        formatter: currencyFormatter
    };

    var turnoverColumn =
    {
        name: 'TotalTurnOverSingle',
        index: 'TotalTurnOverSingle',
        width: 85,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var actualStakeColumn =
    {
        name: 'TotalActualStakeSingle',
        index: 'TotalActualStakeSingle',
        width: 80,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        hidden: false,
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var buybackColumn =
    {
        name: 'TotalBuyBackAmountSingle',
        index: 'TotalBuyBackAmountSingle',
        width: 80,
        align: 'right',
        sortable: true,
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
        sortable: true,
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
        sortable: true,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var playerCommColumn =
    {
        name: 'CustomerComm',
        index: 'CustomerComm',
        width: 50,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var customerWlTotalColumn =
    {
        name: 'CustomerTotal',
        index: 'CustomerTotal',
        width: 90,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: jqGridAdditional.formatDecimal
    };

    var licenseeWlColumn =
    {
        name: 'TotalLicenseeWLSingle',
        classes: 'highlight',
        index: 'TotalLicenseeWLSingle',
        width: 100,
        align: 'right',
        sortable: true,
        resizable: false,
        hidden: false,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    var betTradeCommissionColumn =
    {
        name: 'BetTradeCommission',
        classes: 'highlight',
        index: 'BetTradeCommission',
        width: 80,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var baCommissionColumn =
    {
        name: 'BACommission',
        classes: 'highlight',
        index: 'BACommission',
        width: 80,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var racingRevenueColumn =
    {
        name: 'RacingRevenue',
        classes: 'highlight',
        index: 'RacingRevenue',
        width: 55,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var amtDueToBetTradeColumn =
    {
        name: 'AmtDueToBetTrade',
        index: 'AmtDueToBetTrade',
        width: 68,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var amtDueToBaColumn =
    {
        name: 'AmtDueToBA',
        index: 'AmtDueToBA',
        width: 68,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var amtDueToRacingColumn =
    {
        name: 'AmtDueToRacing',
        index: 'AmtDueToRacing',
        width: 65,
        align: 'right',
        sortable: true,
        resizable: false,
        formatter: 'currency',
        formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
    };

    var currencyIdColumn =
    {
        name: 'CurrencyId',
        index: 'CurrencyId',
        width: 2,
        align: 'center',
        sortable: true,
        resizable: false,
        hidden: true
    };

    var customerIdColumn =
    {
        name: 'CustomerId',
        index: 'CustomerId',
        width: 2,
        align: 'center',
        sortable: true,
        resizable: false,
        hidden: true
    };

    var productColumn =
    {
        name: 'productName',
        index: 'productName',
        width: 90,
        sortable: true,
        resizable: false,
        align: "right",
        hidden: true,
        title: true,
        formatter: jqGridAdditional.formatLicenseeWl
    };

    //#endregion

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency", 'Turnover', 'ActualStake', 'BuyBackAmount',
     'TotalTransaction', '@[Win]/@[Loss]', 'Comm', 'Total', 'LicenseeW_L', '@[BetTrade]<br/>@[Commission]', 'BA<br/>@[Commission]',
     '@[RacingRevenue]', '@[AmtDueToBetTrade]', '@[AmtDueToBA]', '@[AmtDueToRacing]', 'CurrencyId', 'CustomerId'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
        turnoverColumn, actualStakeColumn, buybackColumn, totalTransColumn, customerWlColumn, playerCommColumn,
        customerWlTotalColumn, licenseeWlColumn, betTradeCommissionColumn, baCommissionColumn, racingRevenueColumn,
        amtDueToBetTradeColumn, amtDueToBaColumn, amtDueToRacingColumn, currencyIdColumn, customerIdColumn];

    var me =
    {
        licenseeId: 0,
        currencyIndex: 4,
        turnoverIndex: 5,
        licenseeWlIndex: 12,
        customerWlTotalIndex: 11,
        hasCustomerComm: true,
        pageSize: 50,
        nonePagingSize: 2000,
        rowNum: 50,
        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
        totalColspan: [2, 2, 3],
        //level 0,1,2
        urlData: 'api/report/LicenseeWl',
        urlExcelData: 'api/report/LicenseeWl',
        urlResources: 'Report/LicenseeWLResource',
        gridOption:
        {
            datatype: 'local',
            colNames: [],
            colModel: [],
            data: [],
            height: '100%',
            autowidth: true,
            forceFit: false,
            shrinkToFit: true,
            loadtext: 'loading...',
            loadui: 'disable',
            loadonce: true,
            footerrow: true,
            userDataOnFooter: true,
            userData: [],
            grouping: true,
            viewrecords: true,
            gridview: true,
            rownumbers: true
        },
        restoreExpanse: function () {
            for (var property in _expanseTypes) {
                if (_expanseTypes.hasOwnProperty(property)) {
                    me.toggle(_expanseTypes[property], property);
                }
            }
        }
    };

    function isProductChecked(productOptions, productCode) {
        var length = productOptions.length;

        for (var i = 0; i < length; i++) {
            if (productOptions[i].Code === productCode) {
                return productOptions[i].Status;
            }
        }

        return null;
    }

    function isIgnoredTurnoverProduct(productCode) {
        return $.inArray(productCode, IGNORED_TURNOVER_PRODUCTS) >= 0;
    }

    function isIgnoredLicenseeWLProduct(productCode) {
        return $.inArray(productCode, IGNORED_LICENSEEWL_PRODUCTS) >= 0;
    }

    function turnoverProductsFilter(item) {
        return item.Status === true && isIgnoredTurnoverProduct(item.Code) === false;
    }

    function licenseeWLProductsFilter(item) {
        return item.Status === true && isIgnoredLicenseeWLProduct(item.Code) === false;
    }

    function addBuyBackAmountColumns(columns, columnIndex) {
        var buyBackAmountColumn = Site.clone(productColumn);
        buyBackAmountColumn.name = "TotalBuyBackAmount";
        buyBackAmountColumn.index = "TotalBuyBackAmount";

        var buyBackAmountSBColumn = Site.clone(productColumn);
        buyBackAmountSBColumn.name = "BuyBackAmountSB";
        buyBackAmountSBColumn.index = "BuyBackAmountSB";

        var buyBackAmountBAColumn = Site.clone(productColumn);
        buyBackAmountBAColumn.name = "BuyBackAmountBA";
        buyBackAmountBAColumn.index = "BuyBackAmountBA";

        columns.insertAfter(buyBackAmountColumn, columnIndex);
        columns.insertAfter(buyBackAmountBAColumn, columnIndex);
        columns.insertAfter(buyBackAmountSBColumn, columnIndex);
    }

    function addActualStakeColumns(columns, columnIndex) {
        var actualStakeColumn = Site.clone(productColumn);
        actualStakeColumn.name = "TotalActualStake";
        actualStakeColumn.index = "TotalActualStake";

        var actualStakeSBColumn = Site.clone(productColumn);
        actualStakeSBColumn.name = "ActualStakeSB";
        actualStakeSBColumn.index = "ActualStakeSB";

        var actualStakeBAColumn = Site.clone(productColumn);
        actualStakeBAColumn.name = "ActualStakeBA";
        actualStakeBAColumn.index = "ActualStakeBA";

        columns.insertAfter(actualStakeColumn, columnIndex);
        columns.insertAfter(actualStakeBAColumn, columnIndex);
        columns.insertAfter(actualStakeSBColumn, columnIndex);
    }

    me.setGroupHeaderColumns = function (grid, productOptions) {
        var numOfTurnover = JSLINQ(productOptions).Count(turnoverProductsFilter) + 1;
        var lastOfTurnover = JSLINQ(productOptions).Last(turnoverProductsFilter);

        var numOfLicenseeWL = JSLINQ(productOptions).Count(licenseeWLProductsFilter) + 1;
        var lastOfLicenseeWL = JSLINQ(productOptions).Last(licenseeWLProductsFilter);

        var groupCustomerWinloss = me.hasCustomerComm ?
        {
            startColumnName: 'CustomerWinLoss',
            numberOfColumns: 3,
            titleText: Site.getSingleResource('CustomerW_L')
        } : {};
        var groupTurnoverWL = lastOfTurnover != null ?
        {
            startColumnName: "TurnOver" + lastOfTurnover.Code,
            numberOfColumns: numOfTurnover,
            titleText: Site.getSingleResource('Turnover')
        } : {};
        var groupLicenseeWL = lastOfLicenseeWL != null ?
        {
            startColumnName: "LicenseeWL" + lastOfLicenseeWL.Code,
            numberOfColumns: numOfLicenseeWL,
            titleText: Site.getSingleResource('LicenseeW_L')
        } : {};

        var groupBuyBackAmount = lastOfLicenseeWL != null ?
        {
            startColumnName: "BuyBackAmountSB",
            numberOfColumns: 3,
            titleText: Site.getSingleResource('BuyBackAmount')
        } : {};

        var groupActualStake = lastOfLicenseeWL != null ?
        {
            startColumnName: "ActualStakeSB",
            numberOfColumns: 3,
            titleText: Site.getSingleResource('ActualStake')
        } : {};

        grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [groupTurnoverWL, groupLicenseeWL, groupCustomerWinloss, groupBuyBackAmount, groupActualStake]
        });
    };

    me.getColName = function (productOptions) {
        // Column Customer Wl
        var temp = Site.clone(colNameList);
        var turnOverIndex = me.turnoverIndex;
        var licenseeIndex = me.licenseeWlIndex;
        var customerWLTotalIndex = me.customerWlTotalIndex;
        temp[me.customerWlTotalIndex] = me.hasCustomerComm ? 'Total' : 'CustomerW_L';
        $.each(productOptions, function (index, item) {
            //insert turn over column
            if (item.Status === true) {
                if (isIgnoredTurnoverProduct(item.Code) === false) {
                    temp.insertAfter(item.Name, me.currencyIndex);
                    turnOverIndex++;
                    licenseeIndex++;
                    customerWLTotalIndex++;
                }
                if (isIgnoredLicenseeWLProduct(item.Code) === false) {
                    temp.insertAfter(item.Name, customerWLTotalIndex);
                    licenseeIndex++;
                }
            }
        });
        //add total column
        temp.insertBefore("Total", turnOverIndex);
        licenseeIndex++;
        temp.insertBefore("Total", licenseeIndex);

        var actualStakeIndex = turnOverIndex + 1;
        if (isProductChecked(productOptions, "BA") !== null) {
            temp.insertAfter("Total", actualStakeIndex);
            temp.insertAfter("BA", actualStakeIndex);
            temp.insertAfter("Sportsbook", actualStakeIndex);
        }

        var buyBackIndex = actualStakeIndex + 4;
        if (isProductChecked(productOptions, "BA") !== null) {
            temp.insertAfter("Total", buyBackIndex);
            temp.insertAfter("BA", buyBackIndex);
            temp.insertAfter("Sportsbook", buyBackIndex);
        }

        return Site.getResources(temp);
    };

    me.getColModel = function (level, reportBy, productOptions) {
        reportBy = Number(reportBy);

        // Column hidden by customer comm
        customerWlColumn.hidden = !me.hasCustomerComm;
        playerCommColumn.hidden = !me.hasCustomerComm;
        level = Number(level);
        //set width of licenseeWL column
        if (!me.hasCustomerComm) { licenseeWlColumn.width = 90; }
        // Column hidden by level
        currencyColumn.formatter = level === 0 ? currencyFormatter : null;
        systemIdColumn.formatter = level === 1 ? systemIdFormatter : null;

        dateColumn.hidden = level > 0;
        externalIdColumn.hidden = level < 2;
        systemIdColumn.hidden = level === 0;

        // Column hidden by product
        var allowedSb = isProductChecked(productOptions, "SB");
        var allowedBa = isProductChecked(productOptions, "BA");
        var allowedRacing = isProductChecked(productOptions, "RB");
        var allowedBetTrade = isProductChecked(productOptions, "BT");
        buybackColumn.hidden = !allowedBetTrade;
        actualStakeColumn.hidden = !allowedBetTrade;
        baCommissionColumn.hidden = !allowedBa;
        amtDueToBaColumn.hidden = !allowedBa;
        racingRevenueColumn.hidden = !allowedRacing;
        amtDueToRacingColumn.hidden = !allowedRacing;
        betTradeCommissionColumn.hidden = !allowedBetTrade;
        amtDueToBetTradeColumn.hidden = allowedSb === false || allowedBa === false || !allowedBetTrade;

        // Row span setting
        // indexColumn.cellattr = level == 0 ? jqGridAdditional.rowSpanSetting : null;
        // dateColumn.cellattr = level == 0 ? jqGridAdditional.rowSpanSetting : null;
        if (level === 2) {
            externalIdColumn.formatter = betlistLink;
            systemIdColumn.formatter = betlistLink;
        }
        if (reportBy === 2) {
            dateColumn.formatoptions = {
                newformat: 'm/Y'
            };
        } else {
            dateColumn.formatoptions = {
                newformat: 'm/d/Y'
            };
        }

        var temp = Site.clone(colModelList);

        var turnOverIndex = me.turnoverIndex;

        var licenseeIndex = me.licenseeWlIndex;

        var customerWLTotalIndex = me.customerWlTotalIndex;
        $.each(productOptions, function (index, item) {
            //insert turn over column
            if (item.Status === true) {
                // Exclude bettrade
                if (isIgnoredTurnoverProduct(item.Code) === false) {
                    var turnOver = Site.clone(productColumn);
                    turnOver.name = "TurnOver" + item.Code;
                    turnOver.index = "TurnOver" + item.Code;
                    turnOver.sortable = true;

                    temp.insertAfter(turnOver, me.currencyIndex);
                    turnOverIndex++;
                    licenseeIndex++;
                    customerWLTotalIndex++;
                }

                if (isIgnoredLicenseeWLProduct(item.Code) === false) {
                    var licenseeWL = Site.clone(productColumn);
                    licenseeWL.name = "LicenseeWL" + item.Code;
                    licenseeWL.index = "LicenseeWL" + item.Code;
                    licenseeWL.classes = 'highlight';
                    licenseeWL.sortable = true;

                    temp.insertAfter(licenseeWL, customerWLTotalIndex);
                    licenseeIndex++;
                }
            }
        });

        //add total

        var turnOverTotal = Site.clone(productColumn);
        turnOverTotal.name = "TotalTurnOver";
        turnOverTotal.index = "TotalTurnOver";
        temp.insertBefore(turnOverTotal, turnOverIndex);
        licenseeIndex++;
        var licenseeWLTotal = Site.clone(productColumn);
        licenseeWLTotal.name = "TotalLicenseeWL";
        licenseeWLTotal.index = "TotalLicenseeWL";
        licenseeWLTotal.classes = 'highlight';
        temp.insertBefore(licenseeWLTotal, licenseeIndex);

        if (allowedBa !== null) {
            var actualStakeIndex = turnOverIndex + 1;
            addActualStakeColumns(temp, actualStakeIndex)
            addBuyBackAmountColumns(temp, actualStakeIndex + 4);
        }

        return temp;
    };

    me.getGridOption = function (level, reportBy, productOptions) {
        me.gridOption.colNames = me.getColName(productOptions);
        me.gridOption.colModel = me.getColModel(level, reportBy, productOptions);
        return Site.clone(me.gridOption);
    };

    me.getTotalColModel = function (level, productOptions) {
        var colModel = me.getColModel(level, -1, productOptions);
        // Not show drill down link on total link
        var curCol = JSLINQ(colModel).First(function (item) {
            return item.name === "CurrencyName";
        });

        curCol.formatter = null;
        return colModel;
    };

    me.addProductButton = function (grid, gridId, gridName, productOptions) {
        /// <summary>
        /// add collapse/expand button to header product columns
        /// </summary>
        /// <param name="grid">grid instance</param>

        var numOfActiveLicPro = JSLINQ(productOptions).Count(licenseeWLProductsFilter);
        var numOfActiveTurnPro = JSLINQ(productOptions).Count(turnoverProductsFilter);
        var isAllowedBetTrade = isProductChecked(productOptions, "BT");
        var isAllowBA = isProductChecked(productOptions, "BA");
        var clear = function (button) {
            $(button).unbind("click").remove();
        };
        //reset licenseWL column
        var curWidth = $("#ReportGrid_TotalLicenseeWLSingle div").width();

        if (curWidth < 100) {
            $("#ReportGrid_TotalLicenseeWLSingle div").css("width", "100px");
        }

        //#region Toggle
        // column params: TurnOver, LicenseeWL, BuyBackAmount
        var toggle = function (isShow, column) {
            if ((column === "LicenseeWL" && numOfActiveLicPro <= 1) ||
                (column === "TurnOver" && numOfActiveTurnPro <= 1) ||
                (column === "BuyBackAmount" && (!isAllowedBetTrade || isAllowBA === null)) ||
                (column === "ActualStake" && (!isAllowedBetTrade || isAllowBA === null))) {
                return;
            }
            _expanseTypes[column] = isShow;

            var param = isShow ? "showCol" : "hideCol";
            var fColumns = [];

            if (column === "BuyBackAmount" || column === "ActualStake") {
                var sbColumnName = column + "SB";
                var baColumnName = column + "BA";
                fColumns = [gridName + "_" + sbColumnName, gridName + "_" + baColumnName];
                grid.jqGrid(param, sbColumnName);
                grid.jqGrid(param, baColumnName);
            } else {
                // footer product columns
                $.each(productOptions, function (index, value) {
                    //insert turn over column
                    if (value.Status === true) {
                        var columnName = column + value.Code;
                        fColumns.push(gridName + "_" + columnName);
                        grid.jqGrid(param, columnName);
                    }
                });
            }
            var totalColumn = 'Total' + column;

            var fTotalColumn = gridName + "_" + totalColumn;

            fColumns.push(fTotalColumn);

            grid.jqGrid(param, totalColumn);

            /*show /hide single column*/
            var singleColumn = totalColumn + 'Single';

            var singleParam = isShow ? "hideCol" : "showCol";

            grid.jqGrid(singleParam, singleColumn);

            /*show/hide footer column*/
            jqGridAdditional.setFooterColumns(param, fColumns);

            var singleFColumn = gridName + "_" + singleColumn;

            jqGridAdditional.setFooterColumns(singleParam, [singleFColumn]);

            if (column === "TurnOver") {
                addTurnOverExpand();
            }
            else if (column === "LicenseeWL") {
                addLicenseeExpand();
            }
            else if (column === "ActualStake") {
                addActualStakeExpand();
            }
            else {
                addBuyBackExpand();
            }

            /*jqgrid always clears grouping header after events complete*/
            addTurnOverCollapse();

            addLicenseeWlCollapse();

            addBuyBackCollapse();

            addActualStakeCollapse();
            
            //update footer row
            $("#footer").html($(".ui-jqgrid-ftable").html());

            $('.report-content').css('cursor', 'default');
            $(window).resize();
            return false;
        };

        me.toggle = toggle;
        //#end

        function addToggleButton($container, $element, isToggle, column) {
            $element.css({ "float": "right", "height": "17px" })
            .appendTo($container).button({ icons: { primary: "ui-icon-circle-triangle-e" }, text: false })
            .click(function () { return toggle(isToggle, column); });
        }

        var addTurnOverExpand = function () {
            if (numOfActiveTurnPro > 1) {
                clear("#btnTurnOverExpand");
                var $container = $("#" + gridId + "_" + "TotalTurnOverSingle");
                var $button = $('<button title="Expand to view details" id="btnTurnOverExpand">');
                addToggleButton($container, $button, true, "TurnOver");
            }
        };

        var addTurnOverCollapse = function () {
            if (numOfActiveTurnPro > 1) {
                clear("#btnTurnOverCollapse");

                var $container = $(grid).closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column-header:first");

                var $button = $('<button id="btnTurnOverCollapse">');
                addToggleButton($container, $button, false, "TurnOver");
            }
        };

        var addLicenseeExpand = function () {
            if (numOfActiveLicPro > 1) {
                clear("#btnLicenseeWLExpand");
                var $container = $("#" + gridId + "_" + "TotalLicenseeWLSingle");
                var $button = $('<button title="Expand to view details" id="btnLicenseeWLExpand">');
                addToggleButton($container, $button, true, "LicenseeWL");
            }
        };

        var addLicenseeWlCollapse = function () {
            if (numOfActiveLicPro > 1) {
                clear("#btnLicenseeWLCollapse");

                var $container = $(grid).closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column-header:last");
                var $button = $('<button id="btnLicenseeWLCollapse">');
                addToggleButton($container, $button, false, "LicenseeWL");
            }
        };

        var addBuyBackExpand = function () {
            if (isAllowBA !== null) {
                clear("#btnBuyBackExpand");
                var $container = $("#" + gridId + "_" + "TotalBuyBackAmountSingle");
                var $button = $('<button title="Expand to view details" id="btnBuyBackExpand">');
                addToggleButton($container, $button, true, "BuyBackAmount");
            }
        };

        var addBuyBackCollapse = function () {
            if (isAllowBA !== null) {
                clear("#btnBuyBackCollapse");
                var $container = $(grid).closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column-header");
                $container = $container[$container.length - (me.hasCustomerComm ? 3 : 2)];
                var $button = $('<button id="btnBuyBackCollapse">');
                addToggleButton($container, $button, false, "BuyBackAmount");
            }
        };

        var addActualStakeCollapse = function () {
            if (isAllowBA !== null) {
                clear("#btnActualStakeCollapse");
                var $container = $(grid).closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column-header");
                $container = $container[$container.length - (me.hasCustomerComm ? 4 : 3)];
                var $button = $('<button id="btnActualStakeCollapse">');
                addToggleButton($container, $button, false, "ActualStake");
            }
        };

        var addActualStakeExpand = function () {
            if (isAllowBA !== null) {
                clear("#btnActualStakeExpand");
                var $container = $("#" + gridId + "_" + "TotalActualStakeSingle");
                var $button = $('<button title="Expand to view details" id="btnActualStakeExpand">');
                addToggleButton($container, $button, true, "ActualStake");
            }
        };
        // Collapsing when loading completed
        addTurnOverCollapse();
        addTurnOverExpand();

        addLicenseeExpand();
        addLicenseeWlCollapse();

        addBuyBackCollapse();
        addBuyBackExpand();

        addActualStakeCollapse();
        addActualStakeExpand();
    };

    return me;
})();