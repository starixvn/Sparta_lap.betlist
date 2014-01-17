/// <reference path="/@JSense.js" />

var CustomerDailyStatement = (function ($) {
    return {
        loadData: function (params, url) {
            $('#BetListGrid').jqGrid('GridUnload');
            $.ajax({
                type: "POST",
                data: JSON.stringify(params),
                contentType: 'application/json',
                dataType: "json",
                cache: false,
                url: url,
                error: function (xhRequest, errorText, thrownError) {
                    Site.unBlockUI();
                },
                success: function (response) {
                    CustomerDailyStatement.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (jqXhr, textStatus) {
                }
            });
        },

        getGridOption: function () {
            var gridOption = CustomerOption.gridOption;
            // Specify the hidden column
            var hiddenColumnList = $("#HiddenColumnList").val();
            var hiddenColumnTokens = hiddenColumnList.split(",");
            var showSummary = hiddenColumnList.indexOf("licenseewl") < 0;

            $.each(hiddenColumnTokens, function (index, value) {
                jqGridAdditional.setColumnVisible(CustomerOption.columnModel, value, true);
            });
            gridOption.colNames = CustomerOption.columnNames;
            gridOption.colModel = CustomerOption.columnModel;
            gridOption.footerrow = showSummary;

            return gridOption;
        },

        bindData: function (response, options) {
            var rawData = response.Main;
            var summary = response.Total;
            var sumWinLoss = jqGridAdditional.formatDecimal(summary.SumWinLoss);
            var sumLicenseeWl = jqGridAdditional.formatLicenseeWl(summary.SumLicenseeWl);

            var betListGrid = $("#BetListGrid");
            var gridOption = CustomerDailyStatement.getGridOption();
            var maxHeight = 650;
            // Specify the hidden column
            var hiddenColumnList = "";

            // Show the grid
            betListGrid.jqGrid({
                datatype: 'local',
                data: rawData,
                colNames: CustomerOption.columnNames,
                colModel: CustomerOption.columnModel,
                rowNum: rawData.length,
                autowidth: true,
                shrinkToFit: true,
                scroll: false,
                loadonce: true,
                gridview: true,
                footerrow: false, //options.showSummary,
                userDataOnFooter: true,
                userData: { WinLoss: sumWinLoss, LicenseeWl: sumLicenseeWl },
                beforeSelectRow: function () {
                    return false;
                },
                onRightClickRow: function () {
                    return false;
                },
                gridComplete: function () {
                    Site.unBlockUI();
                    BetList.gridComplete(this, gridOption, betListGrid);
                }
            });
        }
    };
})(jQuery);