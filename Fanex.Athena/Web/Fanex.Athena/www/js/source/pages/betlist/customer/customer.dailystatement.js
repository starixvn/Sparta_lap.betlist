"use strict";
define(["jquery", "common/site", "common/querystring", "./customer.dailystatement.option",
    "lib/jqgrid/jquery.jqgrid.additional", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, QueryString, DailyStatementOption, jqGridAdditional) {
        var _me = {
            loadData: function (params, url) {
                jqGridAdditional.clearGrid("BetListGrid");

                $.ajax({
                    type: "POST",
                    data: JSON.stringify(params),
                    contentType: 'application/json',
                    dataType: "json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.bindData(response);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                });
            },

            getGridOption: function () {
                var gridOption = DailyStatementOption.gridOption;
                // Specify the hidden column
                var hiddenColumnList = $("#HiddenColumnList").val();
                var hiddenColumnTokens = hiddenColumnList.split(",");
                var showSummary = hiddenColumnList.indexOf("licenseewl") < 0;

                $.each(hiddenColumnTokens, function (index, value) {
                    jqGridAdditional.setColumnVisible(gridOption.colModel, value, true);
                });
                gridOption.colNames = DailyStatementOption.getColNames();
                gridOption.footerrow = showSummary;

                return gridOption;
            },

            bindData: function (response) {
                var data = response.Main;
                var summary = response.Total;
                var sumWinLoss = jqGridAdditional.formatDecimal(summary.SumWinLoss);
                var sumLicenseeWl = jqGridAdditional.formatLicenseeWl(summary.SumLicenseeWl);

                var gridOption = _me.getGridOption();
                gridOption.data = data;
                gridOption.rowNum = data.length;
                gridOption.userData = { WinLoss: sumWinLoss, LicenseeWl: sumLicenseeWl };

                $("#BetListGrid").jqGrid(gridOption);
            }
        };

        return _me;
    });