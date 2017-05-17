/// <reference path="/@JSense.js" />
/*
* Note:in this report, parameter level have 2 value
*  "0" for master and "1" for member
*/

define(["jquery", "common/querystring", "common/site", "lib/jqgrid/jquery.jqgrid.additional", "lib/linq"],
    function ($, QueryString, Site, jqGridAdditional) {
        "use strict";
        var BA_PRODUCTCODE = "BA";
        var RB_PRODUCTCODE = "RB";

        var betlistLink = function (cellvalue, options, rowObject) {
            var param =
            {
                DrillDownCurrencyId: rowObject.CurrencyId,
                DrillDownCurrencyName: rowObject.CurrencyName,
                CustomerId: rowObject.CustomerId,
                ExternalId: rowObject.ExternalId,
                SystemId: rowObject.SystemId
            };
            return "<a class='bet-list-link' href='javascript:;' data-drill-down=" + JSON.stringify(param) + ">" + cellvalue + "</a>";
        };
        var drilldownLink = function (cellvalue, options, rowObject) {
            var param =
            {
                PlatformType: rowObject.PlatformType > 0 ? rowObject.PlatformType : Number(rowObject.Parent),
                ProductCode: rowObject.PlatformType < 0 ? rowObject.ProductCode : undefined
            };

            return "<a class='drilldown' href='javascript:;' data-drill-down=" + JSON.stringify(param) + ">" + cellvalue + "</a>";
        };
        var idColumn = {
            name: "PlatformType",
            index: "PlatformType",
            key: true,
            hidden: true,
            align: "center"
        };
        var platformNameColumn = {
            name: "Name",
            index: "Name",
            width: 120,
            align: "left",
            valign: "middle",
            sortable: false,
            resizable: false,
            formatter: drilldownLink
        };
        var indexColumn = {
            name: "Index",
            index: "Index",
            width: 60,
            align: "center",
            valign: "middle",
            sortable: false,
            resizable: false,
            cellattr: jqGridAdditional.rowSpanSetting
        };
        var externalIdColumn = {
            name: "ExternalId",
            index: "ExternalId",
            width: 85,
            align: "left",
            sortable: true,
            resizable: false,
            formatter: betlistLink
        };
        var systemIdColumn = {
            name: "SystemId",
            index: "SystemId",
            width: 120,
            align: "left",
            sortable: true,
            resizable: false,
            formatter: betlistLink
        };
        var currencyColumn = {
            name: "CurrencyName",
            index: "CurrencyName",
            width: 60,
            align: "center",
            sortable: true,
            resizable: false
        };
        var turnoverColumn = {
            name: "Turnover",
            index: "Turnover",
            resizable: false,
            width: 85,
            align: "right",
            sortable: true,
            formatter: "currency",
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        var turnoverPercentColumn = {
            name: "TurnoverPercent",
            index: "TurnoverPercent",
            resizable: false,
            width: 85,
            align: "right",
            sortable: true,
            formatter: "currency",
            formatoptions: { decimalPlaces: 2, suffix: "%" }
        };
        var totalTransColumn = {
            name: "TotalTransactions",
            index: "TotalTransactions",
            width: 95,
            align: "right",
            sortable: true,
            resizable: false,
            formatter: "integer",
            frozen: true,
            formatoptions:
            {
                thousandsSeparator: ","
            }
        };
        var customerWlColumn = {
            name: "CustomerWinLoss",
            index: "CustomerWinLoss",
            width: 90,
            align: "right",
            sortable: true,
            resizable: false,
            frozen: true,
            formatter: jqGridAdditional.formatDecimal
        };
        var playerCommColumn = {
            name: "CustomerComm",
            index: "CustomerComm",
            width: 50,
            align: "right",
            sortable: true,
            resizable: false,
            frozen: true,
            formatter: jqGridAdditional.formatDecimal
        };
        var customerWlTotalColumn = {
            name: "CustomerTotal",
            index: "CustomerTotal",
            width: 90,
            align: "right",
            sortable: true,
            resizable: false,
            frozen: true,
            formatter: jqGridAdditional.formatDecimal
        };
        var licenseeWlColumn = {
            name: "LicenseeWL",
            index: "LicenseeWL",
            width: 85,
            align: "right",
            sortable: true,
            resizable: false,
            frozen: true,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };
        var baCommissionColumn = {
            name: "BACommission",
            index: "BACommission",
            width: 70,
            align: "right",
            sortable: true,
            resizable: false,
            formatter: "currency",
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        var racingRevenueColumn = {
            name: "RacingRevenue",
            index: "RacingRevenue",
            width: 70,
            align: "right",
            sortable: true,
            resizable: false,
            formatter: "currency",
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        var amtDueToBaColumn = {
            name: "AmtDueToBA",
            index: "AmtDueToBA",
            width: 60,
            align: "right",
            sortable: true,
            resizable: false,
            formatter: "currency",
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        var amtDueToRacingColumn = {
            name: "AmtDueToRacing",
            index: "AmtDueToRacing",
            width: 70,
            align: "right",
            sortable: true,
            resizable: false,
            formatter: "currency",
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        var currencyIdColumn = {
            name: "CurrencyId",
            index: "CurrencyId",
            width: 0,
            align: "center",
            sortable: false,
            resizable: false,
            hidden: true
        };
        var customerIdColumn = {
            name: "CustomerId",
            index: "CustomerId",
            width: 0,
            align: "center",
            sortable: false,
            resizable: false,
            hidden: true
        };
        var columnConfigLevel0 = {
            colNames: [           
                "Platform",
                "Turnover",
                "%Turnover",
                "TotalTransaction",
                "LicenseeW_L",
                "Id",
                "CurrencyId",
                "CustomerId"],
            colModels: [              
                platformNameColumn,
                turnoverColumn,
                turnoverPercentColumn,
                totalTransColumn,
                licenseeWlColumn,
                idColumn,
                currencyIdColumn,
                customerIdColumn]
        };
        var columnConfigLevel1 = {
            colNames: [               
                "#",
                "ExternalId",
                "SystemId",
                "Currency",
                "Turnover",
                "TotalTransaction",
                "LicenseeW_L",
                "Id",
                "CurrencyId",
                "CustomerId"],
            colModels: [               
                indexColumn,
                externalIdColumn,
                systemIdColumn,
                currencyColumn,
                turnoverColumn,
                totalTransColumn,
                licenseeWlColumn,
                idColumn,
                currencyIdColumn,
                customerIdColumn]
        };
        var columnConfigCustomerComm = {
            colsCustomerComm: {
                colNames: ["@[Win]/@[Loss]", "Comm", "Total"],
                colModels: [customerWlColumn, playerCommColumn, customerWlTotalColumn]
            },
            colsWithoutComm: {
                colNames: ["Customer W/L"],
                colModels: [customerWlTotalColumn]
            }
        };
        var columnConfigBA = {
            colNames: ["BA<br/>@[Commission]", "@[AmtDueToBA]"],
            colModels: [baCommissionColumn, amtDueToBaColumn]
        };
        var columnConfigRB = {
            colNames: ["@[RacingRevenue]", "@[AmtDueToRacing]"],
            colModels: [racingRevenueColumn, amtDueToRacingColumn]
        };

        function getColConfigLevel0(cols, customerWL) {
            var customerWlIndex = 3;

            customerWL.forEach(function (item) {
                cols.insertAfter(item, customerWlIndex);
                ++customerWlIndex;
            });

            return cols;
        }

        function getColConfigLevel1(cols, customerWL, colsBA, colsRB) {
            var customerWlIndex = 5;
            customerWL.forEach(function (item) {
                cols.insertAfter(item, customerWlIndex);
                ++customerWlIndex;
            });

            if (colsBA != null) {
                var bAIndex = customerWlIndex + 1;
                colsBA.forEach(function (item) {
                    cols.insertAfter(item, bAIndex);
                    ++bAIndex;
                });
            }

            if (colsRB != null) {
                var rBIndex = colsBA != null ? customerWlIndex + 2 : customerWlIndex + 1;
                colsRB.forEach(function (item) {
                    cols.insertAfter(item, rBIndex);
                    rBIndex = colsBA != null ? rBIndex + 2 : rBIndex + 1;
                });
            }

            return cols;
        }

        function isProductChecked(productOptions, productCode) {
            var length = productOptions.length;

            for (var i = 0; i < length; i++) {
                if (productOptions[i].Code === productCode) {
                    return productOptions[i].Status;
                }
            }

            return false;
        }

        var _me = {
            licenseeId: 0,
            hasCustomerComm: true,
            pageSize: 50,         
            totalColspan: [0, 4],
            urlData: "api/PlatformWinLossReport/Index",
            urlExcelData: "api/PlatformWinLossReport/Index",
            gridOption: {
                datatype: "jsonstring",
                colNames: [],
                colModel: [],
                data: [],
                viewrecords: true,
                height: "100%",
                autowidth: false,
                forceFit: false,
                shrinkToFit: true,
                loadtext: "loading...",
                loadui: "disable",
                loadonce: true,
                gridview: true,
                footerrow: true,
                cmTemplate: { title: true }, //enable tooltip
                treeGrid: true,
                treeGridModel: "adjacency",
                treedatatype: "local",
                ExpandColumn: "Name",
                treeReader: {
                    level_field: "Level",       // jshint ignore:line
                    parent_id_field: "Parent",  // jshint ignore:line
                    leaf_field: "IsLeaf"        // jshint ignore:line
                },
                treeIcons: {plus:"ui-icon-circlesmall-plus icon-arrow", minus:"ui-icon-circlesmall-minus icon-arrow", leaf:""},
                pager: "#ppdata",
                rowNum: 50,
                rowList: [50, 100, 200, 300, 400, 500, 1000, 2000]
            },

            getColName: function (level, productOptions) {
                var customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colNames : columnConfigCustomerComm.colsWithoutComm.colNames;

                if (level === 0) {                  
                    return Site.getResources(getColConfigLevel0(Site.clone(columnConfigLevel0.colNames), customerWL));
                }

                var allowedBa = isProductChecked(productOptions, BA_PRODUCTCODE);
                var allowedRacing = isProductChecked(productOptions, RB_PRODUCTCODE);
                var colsBA = allowedBa ? columnConfigBA.colNames : null;
                var colsRB = allowedRacing ? columnConfigRB.colNames : null;

                return Site.getResources(getColConfigLevel1(Site.clone(columnConfigLevel1.colNames), customerWL, colsBA, colsRB));
            },

            getColModel: function (level, productOptions) {
                var customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colModels : columnConfigCustomerComm.colsWithoutComm.colModels;

                if (level === 0) {
                    var colModels = getColConfigLevel0(Site.clone(columnConfigLevel0.colModels), customerWL);

                    colModels.forEach(function (item) {
                        item.sortable = false;
                    });

                    return colModels;
                }

                var allowedBa = isProductChecked(productOptions, BA_PRODUCTCODE);
                var allowedRacing = isProductChecked(productOptions, RB_PRODUCTCODE);
                var colsBA = allowedBa ? columnConfigBA.colModels : null;
                var colsRB = allowedRacing ? columnConfigRB.colModels : null;

                return getColConfigLevel1(Site.clone(columnConfigLevel1.colModels), customerWL, colsBA, colsRB);
            },

            getGridOption : function (level, productOptions) {
                _me.gridOption.colNames = _me.getColName(level, productOptions);
                _me.gridOption.colModel = _me.getColModel(level, productOptions);

                return _me.gridOption;
            }
        };

        return _me;
    }
);