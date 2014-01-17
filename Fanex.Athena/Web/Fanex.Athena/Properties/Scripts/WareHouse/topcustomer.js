/// <reference path="/@JSense.js" />
var WareHouseTopCustomer = (function ($) {
    return {
        serializeParam: function () {
            var licenseeId = WareHouseTopCustomer.getLicenseeId();
            $('#LicenseeId').val(licenseeId);
            $('#ActiveMin').val($('#SelectReportBy').val());
            $('#SportId').val($('#SelectSport').val());
        },

        getLicenseeId: function () {
            return $('#LicenseeList').length > 0 ? $('#LicenseeList').val() : $('#LicenseeId').val();
        },

        initExcelExporting: function () {
            WareHouseTopCustomer.serializeParam();
            $('#IsExportExcel').val(true);
            $('#TopCustomerForm').submit();
        },

        initialize: function () {
            var currentSportSelected = parseInt($("#SelectSport").val(), 10);
            if (currentSportSelected != 0) {
                $("#WinLossTotalMin").attr("disabled", "disabled");
                $("#SelectReportBy").attr("disabled", "disabled");
                $("#note1").hide();
                $("#note3").show();
            }

            $("#SelectSport").on("change", function () {
                var value = parseInt($(this).val(), 10);
                if (value == 0) {
                    $("#WinLossTotalMin").removeAttr("disabled");
                    $("#SelectReportBy").removeAttr("disabled");
                    $("#note1").show();
                    $("#note3").hide();
                } else {

                    $("#WinLossTotalMin").attr("disabled", "disabled");
                    $("#SelectReportBy").attr("disabled", "disabled");
                    $("#note1").hide();
                    $("#note3").show();
                }
            });

            Site.currencyConstraint();

            $('#ExportToExcel').click(function () {
                WareHouseTopCustomer.initExcelExporting();
            });

            $('#LicenseeList').change(function () {
                WareHouseTopCustomer.getCurrencyListAjax();
            });

            $('#SubmitButton').click(function () {
                WareHouseTopCustomer.submitForm();
            });
        },

        getCurrencyListAjax: function () {
            Site.blockUI();
            var params = {
                "licenseeid": WareHouseTopCustomer.getLicenseeId(),
                "currencyid": $("#CurrencyId").val() ,
                "currencyname":$("#CurrencyName").val()
            };

            $.ajax({
                type: 'POST',
                url: "GetCurrecyList",
                data: params,
                success: function (result) {
                    $("#CurrencyResult").html(result);
                    $("currencyid").val(params.currencyId);
                    Site.currencyConstraint(true);
                },
                error: function () {
                    //window.location.reload();
                }
            });
            Site.unBlockUI();
        },

        getSportAjax: function () {
            Site.blockUI();
            var params = "FromDate=" + $('#FromDate').val() + "&ToDate=" + $('#ToDate').val();
            $.ajax({
                type: 'POST',
                url: "GetSportList",
                data: params,
                success: function (result) {
                    $("#SportList").html(result);
                    $("#note1").show();
                    $("#note3").hide();
                },
                error: function () {
                    //window.location.reload();
                }
            });
            Site.unBlockUI();
        },
        submitForm: function () {
            var betCountMin = parseInt($('#BetCountMin').val());
            var winlossTotal = parseFloat($('WinLossTotalMin').val());
            if (isNaN(betCountMin)) {
                $('#BetCountMin').val(0);
            }
            if (isNaN(winlossTotal)) {
                $('WinLossTotalMin').val(0);
            }
            Site.blockUI();
            WareHouseTopCustomer.serializeParam();
            $('#IsExportExcel').val(false);
            $('#TopCustomerForm').submit();
        }
    };

})(jQuery);

$(window).load(function () {
    WareHouseTopCustomer.initialize();
});
