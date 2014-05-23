"use strict";
/*global Site */

var PlaytechCasino = (function ($) {
    return {
        initialize: function () {
            PlaytechCasino.initEvent();
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {                
                $('#PlaytechCasinoExcelForm').submit();
            });

            $(".drill-down-link").click(function () {
                PlaytechCasino.drillDown(this);
            });

            $(".level-link").click(function () {
                PlaytechCasino.goBack(this);
            });

            //Submit button
            $('#SubmitButton').click(function () {
                PlaytechCasino.submit();
            });

            var contentWidth = $(".data-content").width() + 5;
            $(".content-filter").width(contentWidth < 650 ? 650 : contentWidth);
        },
        submit: function () {
            Site.blockUI();
            $("#Level").val(0);
            $('#IsExportExcel').val(false);
            $('#PlaytechCasinoForm').submit();
        },
        drillDown: function (context) {
            var drillDownLevel = $(context).attr("data-level");
            var drillDownCurrency = $(context).attr("data-currency");
            var drillDownDate = $(context).attr("data-date");
            var drillDownCust = $(context).attr("data-custid");

            $('#IsExportExcel').val(false);
            $("#Level").val(drillDownLevel);
            $("#DrillDownCurrency").val(drillDownCurrency);
            $("#CustId").val(drillDownCust);
            if (drillDownDate !== undefined) {
                $("#DrillDownFromDate").val(drillDownDate);
                $("#DrillDownToDate").val(drillDownDate);
            }

            $('#PlaytechCasinoForm').submit();
        },
        goBack: function (context) {
            var currentLevel = parseInt($(".current-level").attr("data-level"), 10);
            var licenseeId = parseInt($("#LicenseeId").val(), 10);
            var goBackLevel = parseInt($(context).attr("data-level"), 10);

            if (goBackLevel === 1) { // go back agent level                
                history.go(-1);
            }
            else { // go back licensee level
                if (currentLevel === 1 || licenseeId !== 1) { // agent level or member level of non 12bet licensee
                    history.go(-1);
                }
                else { // member level
                    history.go(-2);
                }
            }
        }
    };
})(jQuery);

$(window).load(function () {
    PlaytechCasino.initialize();
});