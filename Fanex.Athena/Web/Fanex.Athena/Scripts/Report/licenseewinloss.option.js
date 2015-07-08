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

    var setDrillDownLink = function (level, currencyId, custId, datetime, cellvalue) {
        return '<a class="drilldown" href="javascript:;" onclick="return LicenseeWinLoss.drillDown(' + level + ',' +
         currencyId + ',' + custId + ',\'' + datetime + '\');">' + cellvalue + '</a>';
    };

    var currencyFormatter = function (cellvalue, options, rowObject) {
        var level = Number(LicenseeWinLossOption.licenseeId) === 1 ? 1 : 2;
        return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, cellvalue);
    };

    var systemIdFormatter = function (cellvalue, options, rowObject) {
        return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, cellvalue);
    };

    var betlistLink = function (cellvalue, options, rowObject) {
        var param =
        {
            DrillDownFromDate: rowObject.Date,
            DrillDownToDate: rowObject.Date,
            DrillDownCurrencyId: rowObject.CurrencyId,
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
        width: 75,
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

    var colNameList = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
      'Turnover', 'TotalTransaction', '@[Win]/@[Loss]', 'Comm', 'Total', 'LicenseeW_L', '@[BetTrade]<br/>@[Commission]', 'BA<br/>@[Commission]',
     '@[RacingRevenue]', '@[AmtDueToBetTrade]', '@[AmtDueToBA]', '@[AmtDueToRacing]', 'CurrencyId', 'CustomerId'];

    var colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
        turnoverColumn, totalTransColumn, customerWlColumn, playerCommColumn, customerWlTotalColumn, licenseeWlColumn, betTradeCommissionColumn,
        baCommissionColumn, racingRevenueColumn, amtDueToBetTradeColumn, amtDueToBaColumn, amtDueToRacingColumn, currencyIdColumn, customerIdColumn];

    var me =
    {
        licenseeId: 0,
        currencyIndex: 4,
        turnoverIndex: 5,
        licenseeWlIndex: 10,
        customerWlTotalIndex: 9,
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

        grid.jqGrid('setGroupHeaders',
        {
            useColSpanStyle: true,
            groupHeaders: [groupTurnoverWL, groupLicenseeWL, groupCustomerWinloss]
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

        var expandHeaders = null, collapseHeaders = null;
        var mumOfActiveLicPro = JSLINQ(productOptions).Count(licenseeWLProductsFilter);
        var mumOfActiveTurnPro = JSLINQ(productOptions).Count(turnoverProductsFilter);
        var clear = function (button) {
            $(button).unbind("click").remove();
        };
        //reset licenseWL column
        var curWidth = $("#ReportGrid_TotalLicenseeWLSingle div").width();

        if (curWidth < 100) {
            $("#ReportGrid_TotalLicenseeWLSingle div").css("width", "100px");
        }

        //#region Toggle
        var toggle = function (isShow, type) {
            type = Number(type);
            _expanseTypes[type] = isShow;

            var param = isShow ? "showCol" : "hideCol";
            var extCol = type === 0 ? "TurnOver" : "LicenseeWL";
            var fColumns = [];

            //footer product columns
            $.each(productOptions, function (index, value) {
                //insert turn over column
                if (value.Status === true) {
                    var columnName = extCol + value.Code;
                    fColumns.push(gridName + "_" + columnName);
                    grid.jqGrid(param, columnName);
                }
            });

            var totalColumn = type === 0 ? 'TotalTurnOver' : 'TotalLicenseeWL';

            var fTotalColumn = gridName + "_" + totalColumn;

            fColumns.push(fTotalColumn);

            grid.jqGrid(param, totalColumn);

            /*show /hide single column*/
            var singleColumn = type === 0 ? "TotalTurnOverSingle" : "TotalLicenseeWLSingle";

            var singleParam = isShow ? "hideCol" : "showCol";

            grid.jqGrid(singleParam, singleColumn);

            /*show/hide footer column*/
            jqGridAdditional.setFooterColumns(param, fColumns);

            var singleFColumn = gridName + "_" + singleColumn;

            jqGridAdditional.setFooterColumns(singleParam, [singleFColumn]);

            if (type === 0) {
                addTurnOverExpand();
            }
            else {
                addLicenseeExpand();
            }

            /*jqgrid always clears grouping header after events complete*/
            addTurnOverCollapse();

            addLicenseeWlCollapse();
            //update footer row
            $("#footer").html($(".ui-jqgrid-ftable").html());

            $('.report-content').css('cursor', 'default');
            $(window).resize();
            return false;
        };

        me.toggle = toggle;
        //#end

        //#region add expand button

        var addTurnOverExpand = function () {
            if (mumOfActiveTurnPro > 1) {
                clear("#btnTurnOverExpand");

                expandHeaders = $(grid).closest("div.ui-jqgrid-view")
                        .find("div.ui-jqgrid-hdiv table.ui-jqgrid-htable tr.ui-jqgrid-labels > th.ui-th-column > div.ui-jqgrid-sortable");

                var turnoverSingle = JSLINQ(expandHeaders).First(function (item) {
                    return item.id === gridId + "_" + "TotalTurnOverSingle";
                });

                $('<button title="Expand to view details" id="btnTurnOverExpand">').css({
                    "float": "right",
                    "height": "17px"
                }).appendTo(turnoverSingle).button({
                    icons:
                {
                    primary: "ui-icon-circle-triangle-e"
                },
                    text: false
                }).click(function () {
                    return toggle(true, 0);
                });
            }
        };

        var addLicenseeExpand = function () {
            if (mumOfActiveLicPro > 1) {
                clear("#btnLicenseeWLExpand");

                expandHeaders = $(grid).closest(".ui-jqgrid-view")
                            .find(".ui-jqgrid-hdiv .ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column > .ui-jqgrid-sortable");

                var licenseeWLSingle = JSLINQ(expandHeaders).First(function (item) {
                    return item.id === gridId + "_" + "TotalLicenseeWLSingle";
                });

                $('<button title="Expand to view details" id="btnLicenseeWLExpand">').css({
                    "float": "right",
                    "height": "17px"
                }).appendTo(licenseeWLSingle).button({
                    icons:
                {
                    primary: "ui-icon-circle-triangle-e"
                },
                    text: false
                }).click(function () {
                    return toggle(true, 1);
                });
            }
        };
        //#end

        //#region add collapse button
        var addTurnOverCollapse = function () {
            collapseHeaders = $(grid).closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-hdiv .ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column-header");

            clear("#btnTurnOverCollapse");

            $('<button id="btnTurnOverCollapse">').css({
                "float": "right",
                "height": "17px"
            }).appendTo(collapseHeaders[0]).button({
                icons:
                {
                    primary: "ui-icon-circle-triangle-w"
                },
                text: false
            }).click(function () {
                return toggle(false, 0);
            });
        };

        var addLicenseeWlCollapse = function () {
            clear("#btnLicenseeWLCollapse");

            collapseHeaders = $(grid).closest(".ui-jqgrid-view")
                        .find(".ui-jqgrid-hdiv .ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column-header");

            var licenseeWlIndex = me.hasCustomerComm ? 2 : 1;
            $('<button id="btnLicenseeWLCollapse">').css({
                "float": "right",
                "height": "17px"
            }).appendTo(collapseHeaders[licenseeWlIndex]).button({
                icons:
                {
                    primary: "ui-icon-circle-triangle-w"
                },
                text: false
            }).click(function () {
                return toggle(false, 1);
            });
        };

        //#end

        //#region  Excute your code
        ////Collapsing when loading completed
        addTurnOverCollapse();

        addLicenseeExpand();

        addTurnOverExpand();

        addLicenseeWlCollapse();
        //#endregion
    };

    return me;
})();