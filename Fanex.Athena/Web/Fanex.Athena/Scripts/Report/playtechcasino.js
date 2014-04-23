/* File Created: April 22, 2014 */

var PlaytechCasino = (function ($) {
    return {
        initialize: function () {
            PlaytechCasino.initializeParam();
            PlaytechCasino.initEvent();            
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                PlaytechCasino.initializeParam();
                $('#IsExportExcel').val(true);
                $('#PlaytechCasinoForm').submit();
            });

            $(".drill-down-link").click(function () {
                PlaytechCasino.drillDown(this);
            });

            //Submit button
            $('#SubmitButton').click(function () {
                PlaytechCasino.submit();
            });
        },
        initializeParam: function () {
            
        },
        submit: function () {
            Site.blockUI();
            PlaytechCasino.initializeParam();
            $("#Level").val(0);
            $('#IsExportExcel').val(false);
            $('#PlaytechCasinoForm').submit();
        },
        drillDown: function (context) {
            var drillDownLevel = $(context).attr("data-level");
            var drillDownCurrency = $(context).attr("data-currency");
            var drillDownDate = $(context).attr("data-date");
            var drillDownCust = $(context).attr("data-custid");

            $("#Level").val(drillDownLevel);
            $("#DrillDownCurrency").val(drillDownCurrency);
            $("#CustId").val(drillDownCust);
            if (drillDownDate !== undefined) {
                $("#DrillDownFromDate").val(drillDownDate);
                $("#DrillDownToDate").val(drillDownDate);
            }

            $('#PlaytechCasinoForm').submit();
        }        
    };
})(jQuery);

$(window).load(function () {
    PlaytechCasino.initialize();
});