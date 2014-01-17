var OutstandingBetList = (function ($) {
    return {
        initialize: function () {
            OutstandingBetList.initEvent();
            var params = OutstandingBetList.getParams();
            OutstandingBetList.submit(params);
            OutstandingBetList.CheckGetAll();
        },

        initEvent: function () {

            $("#ViewOption").click(functin(){
                var params = OutstandingBetList.getParamsSubmit();
                OutstandingBetList.submit(params);
            });

            $("#GetAllDate").click(function(){
                OutstandingRunningBetList.CheckGetAll();
            });

            //Submit button
            $('#SubmitButton').click(function () {
                var params = OutstandingBetList.getParamsSubmit();
                OutstandingRunningBetList.submit(params);
            });
        },

        getUrlParams : function(sParam){
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++)
        	{
        	    var sParameterName = sURLVariables[i].split('=');
        	    if (sParameterName[0] == sParam)
        	    {
        	        return sParameterName[1];
        	    }
        	}
        },


        getParams : function () {
            var params = {
            "custId": OutstandingRunningBetList.getUrlParams("custid"),
            "getAllDate" : OutstandingRunningBetList.getUrlParams("getalldate"),
            "showBA" : OutstandingRunningBetList.getUrlParams("showba"),
            "showLC" : OutstandingRunningBetList.getUrlParams("showlc"),
            "showNG" : OutstandingRunningBetList.getUrlParams("showng"),
            "showRB" : OutstandingRunningBetList.getUrlParams("showrb"),
            "showSB" : OutstandingRunningBetList.getUrlParams("showsb"),
            "showVS" : OutstandingRunningBetList.getUrlParams("showvs"),
            "fromdate" : "",
            "todate" : ""
            };
            return params; 
        },

        getParamsSubmit : function(){
            var params = {
            "custId": OutstandingRunningBetList.getUrlParams("custid"),
            "getAllDate" : $("#GetAllDate").is(':checked'),
            "showSB" : $("#ProductOptions0").is(':checked'),
            "showRB" : $("#ProductOptions1").is(':checked'),
            "showNG" : $("#ProductOptions2").is(':checked'),
            "showBA" : $("#ProductOptions3").is(':checked'),
            "showLC" : $("#ProductOptions4").is(':checked'),
            "showVS" : $("#ProductOptions5").is(':checked'),
            "fromdate" : $("#FromDate").val(),
            "todate" : $("#ToDate").val()
            };
            return params;             
        },

        CheckGetAll : function() {
            var isCheckAll = $("#GetAllDate").is(':checked');
    
            if(isCheckAll)
            {
                $('#FromDate').attr('disabled', 'disabled');                
                $('#ToDate').attr('disabled', 'disabled');                
            }
            else
            {
                $('#FromDate').removeAttr('disabled');                
                $('#ToDate').removeAttr('disabled');                
            }       
        },

        submitCompletedBetList: function(params)
        {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/OutstandingCompletedBetList/Get');
            submit(params, url);
            BetList.loadData(params, url, { showSummary: true });
        },

        submitRunningBetList: function(params)
        {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/OutstandingRunningBetList/Get');
            submit(params, url);
            BetList.loadData(params, url, { showSummary: true });
        },

        submitDailyStatementBetList: function(params)
        {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/OutstandingDailyStatementBetList/Get');
            submit(params, url);
            BetList.loadData(params, url, { showSummary: true });
        },

        submit: function (params) {
            var betlistname = $("#ViewOption").find(":selected").text();
            if(betlistname == "RunningBetList")
            {
                OutstandingBetList.submitRunningBetList(param);
            }
            else if(betlistname == "CompletedBetList")
            {
                OutstandingBetList.submitCompletedBetList(params);
            }
            else
            {
                OutstandingBetList.submitDailyStatementBetList(params);
            }
        },
    };
})(jQuery);

$(document).ready(function () {
    OutstandingRunningBetList.initialize();
});