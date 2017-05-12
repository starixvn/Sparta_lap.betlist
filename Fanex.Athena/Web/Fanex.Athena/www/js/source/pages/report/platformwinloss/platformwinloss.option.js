/// <reference path="/@JSense.js" />
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
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
            return '<a class="bet-list-link" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
        };
        var drilldownLink = function (cellvalue, options, rowObject) {
            var param =
            {
                PlatformType: rowObject.PlatformType > 0 ? rowObject.PlatformType : Number(rowObject.Parent),
                ProductCode: rowObject.PlatformType < 0 ? rowObject.ProductCode : undefined
            };

            return '<a class="drilldown" href="javascript:;" data-drill-down=' + JSON.stringify(param) + '>' + cellvalue + '</a>';
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
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS,
            classes: "highlight"
        };
        var racingRevenueColumn = {
            name: "RacingRevenue",
            classes: "highlight",
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
                "Id",
                "Platform",
                "Turnover",
                "%Turnover",
                "TotalTransaction",
                "LicenseeW_L",
                "CurrencyId",
                "CustomerId"],
            colModels: [
                idColumn,
                platformNameColumn,
                turnoverColumn,
                turnoverPercentColumn,
                totalTransColumn,
                licenseeWlColumn,
                currencyIdColumn,
                customerIdColumn]
        };
        var columnConfigLevel1 = {
            colNames: [
                "Id",
                "#",
                "ExternalId",
                "SystemId",
                "Currency",
                "Turnover",
                "TotalTransaction",
                "LicenseeW_L",
                "CurrencyId",
                "CustomerId"],
            colModels: [
                idColumn,
                indexColumn,
                externalIdColumn,
                systemIdColumn,
                currencyColumn,
                turnoverColumn,
                totalTransColumn,
                licenseeWlColumn,
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
            var customerWlIndex = 4;

            customerWL.forEach(function (item) {
                cols.insertAfter(item, customerWlIndex);
                ++customerWlIndex;
            });

            return cols;
        };

        function getColConfigLevel1(cols, customerWL, colsBA, colsRB) {
            var customerWlIndex = 6;
            customerWL.forEach(function (item) {
                cols.insertAfter(item.name, item.indexAfter);
                ++customerWlIndex;
            });

            if (colsBA != null) {
                var bAIndex = customerWlIndex + 1;
                columnConfigBA.forEach(function (item) {
                    cols.insertAfter(item, bAIndex);
                    ++bAIndex;
                });
            }

            if (colsRB != null) {
                var rBIndex = allowedBa == true ? customerWlIndex + 2 : customerWlIndex + 1;
                columnConfigRB.forEach(function (item) {
                    cols.insertAfter(item, rBIndex);
                    rBIndex == true ? rBIndex + 2 : rBIndex + 1;
                });
            }

            return cols;
        };

        function getColNamesLevel0(hasCustomerComm) {
            var customerWL = hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colNames : columnConfigCustomerComm.colsWithoutComm.colNames;

            return Site.getResources(getColConfigLevel0(columnConfigLevel0.colNames, customerWL));
        };

        function getColNamesLevel1(hasCustomerComm, allowedBa, allowedRacing) {
            var customerWL = hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colNames : columnConfigCustomerComm.colsWithoutComm.colNames;
            var colsBA = null;
            var colsRB = null;

            if (allowedBa) {
                colsBA = columnConfigBA.colNames;
            }

            if (allowedRacing) {
                colsRB = columnConfigRB.colNames;
            }

            return Site.getResources(getColConfigLevel1(columnConfigLevel1.colNames, customerWL, colsBA, colsRB));
        };

        function getColModelsLevel0(hasCustomerComm) {
            var customerWL = hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colModels : columnConfigCustomerComm.colsWithoutComm.colModels;

            var colModels = getColConfigLevel0(columnConfigLevel0.colModels, customerWL);

            colModels.forEach(function (item) {
                item.sortable = false;
            });
        };

        function getColModelsLevel1(hasCustomerComm, allowedBa, allowedRacing) {
            var customerWL = hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colModels : columnConfigCustomerComm.colsWithoutComm.colModels;
            var colsBA = null;
            var colsRB = null;

            if (allowedBa) {
                colsBA = columnConfigBA.colModels;
            }

            if (allowedRacing) {
                colsRB = columnConfigRB.colModels;
            }

            licenseeWlColumn.classes = "highlight";

            return getColConfigLevel1(columnConfigLevel1.colModels, customerWL, colsBA, colsRB);
        };

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
            hasCustomerComm: true,
            pageSize: 50,
            nonePagingSize: 2000,
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            totalColspan: [0, 4],
            urlData: "api/PlatformWinLossReport/Index",
            urlExcelData: "api/PlatformWinLossReport/Index",
            gridOption: {
                datatype: "jsonstring",
                colNames: [],
                colModel: [],
                data: [],
                viewrecords: true,
                height: '100%',
                autowidth: false,
                forceFit: false,
                shrinkToFit: true,
                loadtext: "loading...",
                loadui: "disable",
                loadonce: true,
                gridview: true,
                footerrow: true,
                userDataOnFooter: true,
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
                userData: [],
                pager: "#ppdata",
                beforeSelectRow: function () {
                    return false;
                },
                onRightClickRow: function () {
                    return false;
                }
            },

            getColName: function (level, productOptions) {
                if (level == 0) {
                    var customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colNames : columnConfigCustomerComm.colsWithoutComm.colNames;
                    return Site.getResources(getColConfigLevel0(columnConfigLevel0.colNames, customerWL));
                }

                var allowedBa = isProductChecked(productOptions, "BA");
                var allowedRacing = isProductChecked(productOptions, "RB");
                var colsBA = allowedBa ? columnConfigBA.colNames : null;
                var colsRB = allowedRacing ? columnConfigRB.colNames : null;
                var customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colNames : columnConfigCustomerComm.colsWithoutComm.colNames;

                return Site.getResources(getColConfigLevel1(columnConfigLevel1.colNames, customerWL, colsBA, colsRB));
            },

            getColModel: function (level, productOptions) {
                if (level == 0) {
                    var customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colModels : columnConfigCustomerComm.colsWithoutComm.colModels;

                    var colModels = getColConfigLevel0(columnConfigLevel0.colModels, customerWL);

                    colModels.forEach(function (item) {
                        item.sortable = false;
                    });

                    return colModels;
                }

                var allowedBa = isProductChecked(productOptions, "BA");
                var allowedRacing = isProductChecked(productOptions, "RB");
                var colsBA = allowedBa ? columnConfigBA.colModels : null;
                var colsRB = allowedRacing ? columnConfigRB.colModels : null;
                var customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colModels : columnConfigCustomerComm.colsWithoutComm.colModels;
                licenseeWlColumn.classes = "highlight";

                return getColConfigLevel1(columnConfigLevel1.colModels, customerWL, colsBA, colsRB);
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