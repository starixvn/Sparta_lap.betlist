/// <reference path="/@JSense.js" />
var OutstandingBetList = (function ($) {
    return {
        initialize: function () {

            //Hide label product, because it's a common function in razorhelper
            $("#ProductList").prev().hide();

            //Set columns need hide
            OutstandingBetList.hiddenColList();

            //Init event
            OutstandingBetList.initEvent();

            //Get params send from URL
            var params = OutstandingBetList.getParamsSubmit();
            OutstandingBetList.submit(params);
        },
        initEvent: function () {

            $("#ViewOption").change(function () {
                Site.blockUI();

                var title = betlistName == "betlistrunning" 
                    ? Site.getSingleResource("BetListRunning")
                    : (betlistName == "betlist"
                            ? Site.getSingleResource("BetList") 
                            : Site.getSingleResource("DailyStatement"));
                $('.ui-dialog-title', window.parent.document).html(title);

                var betlistName = $("#ViewOption").find(":selected").val();
                var url = location.href;
                url = QueryString.setParam(url, "betlistname", betlistName);
                url = Site.setProductOptionOriginalParamList(url);
                location.href = url;


            });

            //Submit button
            $('#SubmitButton').click(function () {
                Report.setOriginalProduct();
                //Get params when user submit
                var params = OutstandingBetList.getParamsSubmit();
                OutstandingBetList.submit(params);
            });
        },

        hiddenColList: function () {

            //Get item chooses from dropdownlist and set string columns need to hide
            var betlistName = $("#ViewOption").find(":selected").val();

            var columsHidden = "";

            if (betlistName == "betlistrunning") {
                columsHidden = "externalid,systemid,winloss,licenseewl";
            }
            else if (betlistName == "betlist") {
                columsHidden = "externalid,systemid";
            }
            else {
                columsHidden = "externalid,systemid";
            }
            $("#HiddenColumnList").val(columsHidden);
        },

        getParamsSubmit: function () {
            //Get all params from submit
            var params = $.parseJSON($("#BetListMetaModel").val());
            var url = location.href;
            params.CustId = QueryString.getParam(url, "custid");
            params.ExternalId = QueryString.getParam(url, "externalid");
            params.SystemId = QueryString.getParam(url, "systemid");
            params.FromDate = $("#FromDate").val();
            params.ToDate = $("#ToDate").val();

            return Site.setProductOptionParamList(params);
        },

        submitCompletedBetList: function (params) {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/OutstandingCompletedBetList');
            BetList.loadData(params, url);
        },

        submitRunningBetList: function (params) {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/OutstandingRunningBetList');
            BetList.loadData(params, url);
        },

        submitDailyStatementBetList: function (params) {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/OutstandingDailyStatementBetList');
            CustomerDailyStatement.loadData(params, url)
        },

        submit: function (params) {
            var betlistName = $("#ViewOption").find(":selected").val();
            if (betlistName == "dailystatement") {
                $("#ProductList").hide();
                OutstandingBetList.submitDailyStatementBetList(params);

            }
            else if (betlistName == "betlist") {
                $("#ProductList").show();
                OutstandingBetList.submitCompletedBetList(params);
            }
            else {
                $("#ProductList").show();
                OutstandingBetList.submitRunningBetList(params);
            }
        },


        viewDailyStatementBetList: function (custId, date, transType, externalId, systemId) {

            BetList.initialize();

            var url = Site.resolveClientUrl('api/betlist/BetListDailyStatement');

            var params = $.parseJSON($("#BetListMetaModel").val()); ;

            params.CustId = custId;
            params.FromDate = date;
            params.TransType = transType;
            params.ExternalId = externalId;
            params.SystemId = systemId;

            var contentFilter = $('.content-filter');
            contentFilter.remove();
            BetList.loadData(params, url);
        }

    };
})(jQuery);

$(window).load(function () {
    OutstandingBetList.initialize();
});
