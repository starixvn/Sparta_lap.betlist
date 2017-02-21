define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker", "components/paging", "components/scrollabletable", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, Paging, GridHelper) {
    var _me = {
        initialize: function () {
            Site.initialize();
            _me.initEvent();
            Site.createSortingIcon(_me.submitForm);
            Site.addUserGuide({
                resourceName: "RacingCommission",
                title: Site.getSingleResource("RacingCommission")
            });
            _me.initDateRangePicker();
            GridHelper.resizeGrid();
            Paging.initialize();
        },
        initDateRangePicker: function(){
            var ranges = ["Today", "Yesterday", "Currentweek"];
            DatePicker.initDateRangePicker(false, ranges);
        },
        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#ReportForm').submit();
            });
            $('#SubmitButton').click(function () {
                $("#SortColumn").val("ExternalID");
                $("#SortDirection").val("ASC");
                _me.submitForm();
            });

            $(".body-content").on("click", ".bet-list-link", function () {
                var $this = $(this);
                var custId = $this.attr("data-cust-id");
                var systemId = $this.attr("data-system-id");
                var externalId = $this.attr("data-external-id");
                _me.viewRacingBetList(custId, systemId, externalId);
            });
        },

        viewRacingBetList: function (custid, systemId, externalId) {
            var url = Site.resolveClientUrl("RacingCommissionBetList/Index");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").val());
            url = QueryString.setParam(url, "todate", $("#ToDate").val());
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemId", systemId);
            url = QueryString.setParam(url, "externalId", externalId);

            Site.blockUI();
            Site.redirect(url);
        },
        submitForm: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#ReportForm').submit();
        }
    };

    return _me;
});