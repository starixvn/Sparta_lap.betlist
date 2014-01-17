/// <reference path="/@JSense.js" />
var WareHouseWinLoss = (function ($) {
    return {
        serializeParam: function () {
            var licenseeId = WareHouseWinLoss.getLicenseeId();
            $('#LicenseeId').val(licenseeId);
            $('#ReportBy').val($('#SelectReportBy').val());
        },

        getLicenseeId: function () {
            return $('#LicenseeList').length > 0 ? $('#LicenseeList').val() : $('#LicenseeId').val();
        },

        initExcelExporting: function () {
            WareHouseWinLoss.serializeParam();
            $('#IsExportExcel').val(true);
            $('#WarehouseWinLossForm').submit();
        },
        initialize: function () {
            $('#LicenseeList').change(function () {
                WareHouseWinLoss.getCurrencyListAjax();
            });

            $('#SubmitButton').click(function () {
                WareHouseWinLoss.submitForm();
            });

            $('#ExportToExcel').click(function () {
                WareHouseWinLoss.initExcelExporting();
            });
        },

        getCurrencyListAjax: function () {
            Site.blockUI();
            WareHouseWinLoss.serializeParam();

            var params = {
                "licenseeid": WareHouseWinLoss.getLicenseeId(),
                "currencyid": $("#CurrencyId").val(),
                "currencyname": $("#CurrencyName").val()
            };
            $.ajax({
                type: 'POST',
                url: "GetCurrecyList",
                data: params,
                success: function (result) {
                    $("#CurrencyResult").html(result);
                    $("currencyid").val(params.currencyId);
                },
                error: function () {
                    //window.location.reload();
                }
            });

            Site.unBlockUI();
        },

        submitForm: function () {
            Site.blockUI();
            WareHouseWinLoss.serializeParam();
            $('#IsExportExcel').val(false);
        }
    };
})(jQuery);

$(window).load(function () {
    WareHouseWinLoss.initialize();
});