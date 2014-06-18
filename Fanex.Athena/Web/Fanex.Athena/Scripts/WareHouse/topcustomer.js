/*jshint strict: false */
/*global Site */
var WareHouseTopCustomer = (function ($) {
    return {
        serializeParam: function () {
            $('#ActiveMin').val($('#SelectReportBy').val());
            $('#SportId').val($('#SelectSport').val());
        },

        initExcelExporting: function () {
            WareHouseTopCustomer.serializeParam();
            $('#IsExportExcel').val(true);
            $('#TopCustomerForm').submit();
        },

        initialize: function () {
            var currentSportSelected = parseInt($("#SelectSport").val(), 10);
            if (currentSportSelected !== 0) {
                $("#WinLossTotalMin").attr("disabled", "disabled");
                $("#SelectReportBy").attr("disabled", "disabled");
                $("#note1").hide();
                $("#note3").show();
            }

            $("#SelectSport").on("change", function () {
                var value = parseInt($(this).val(), 10);
                if (value === 0) {
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

            $('#SubmitButton').click(function () {
                WareHouseTopCustomer.submitForm();
            });
        },

        submitForm: function () {
            if (WareHouseTopCustomer.checkDateRange()) {
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
        },

        checkDateRange: function () {
            var fromDate = Date.parse($("#FromDate").val());
            var toDate = Date.parse($("#ToDate").val());

            // Check date rang limit
            var daysInMonth = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
            var timeInOneDay = 86400000; // milisecond in 1 day
            var fromMonth = fromDate.getMonth();
            var maxDaysLimit = daysInMonth[fromMonth++ % 11] + daysInMonth[fromMonth++ % 11] + daysInMonth[fromMonth % 11];
            var dateDiff = Math.round(toDate.getTime() - fromDate.getTime()) / timeInOneDay;
            if (dateDiff > maxDaysLimit) { // over 3 months
                alert('Support 3 month data per submit!');
                return false;
            }

            return true;
        }
    };
})(jQuery);

$(window).load(function () {
    WareHouseTopCustomer.initialize();
});