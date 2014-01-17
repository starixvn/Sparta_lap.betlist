/// <reference path="/@JSense.js" />

var CommonBetList = (function ($) {
    return {
        initialize: function () {
            $(".content-filter").remove();

            var actionName = $("#ActionName").val();
            var url = Site.resolveClientUrl('api/betlist/' + actionName);
            var serverPaging = $("#IsServerPaging").val();

            if (serverPaging === "1") {
                var params = QueryString.getQueryString(location.href);
                BetList.loadDataPaging(url + "?" + params);
                //BetList.loadDataPaging2(url, params);
            }
            else {
                var params = $.parseJSON($("#BetListMetaModel").val());

                // Update datetime because js-engine parser jsonDatetime wrongly
                params.FromDate = params.FromDateString;
                params.ToDate = params.ToDateString;
                params.Date = params.DateString;

                BetList.loadData(params, url, window.loadBetListSuccessCallback);
            }

            $(".content-title").width($(".content-main").width() - 50);

            $("#ExportToExcel").click(CommonBetList.exportToExcel);            
        },
        exportToExcel: function () {
            var url = window.location.href;
            var page = $('#BetListGrid').getGridParam('page');
            var size = $('#BetListGrid').getGridParam('rowNum');

            var serverPaging = $("#IsServerPaging").val();
            if (serverPaging === "1") {
                url = QueryString.setParam(url, "PageIndex", page);
                url = QueryString.setParam(url, "PageSize", size);
            }

            url = QueryString.setParam(url, "IsExportExcel", true);
            window.location.href = url;            
        }
    };
})(jQuery);

$(window).load(function () {
    CommonBetList.initialize();
});