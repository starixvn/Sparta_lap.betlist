/*global Site,BetList,QueryString*/
/// <reference path="/@JSense.js" />

var CommonBetList = (function ($) {
    "use strict";
    var MAXIMUM_EXCEL_ROWS = 65535;

    return {
        initialize: function () {
            BetList.initialize();
            $(".content-filter").remove();
            
            var actionName = $("#ActionName").val();
            var url = Site.resolveClientUrl('api/betlist/' + actionName);
            var serverPaging = $("#IsServerPaging").val();
            if (serverPaging === "1") {
                var paramString = QueryString.getQueryString(location.href);
                BetList.loadDataPaging(url + "?" + paramString);
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
            var $grid = $('#BetListGrid');
            if ($("#IsExportAll").val() === "1") {
                var totalRows = $grid.jqGrid('getGridParam', 'records');
                if (totalRows > MAXIMUM_EXCEL_ROWS) {
                    alert(Site.getSingleResource('ExcelExceedRowsMsg'));
                    return;
                }
            }

            var url = window.location.href;
            var page = $grid.getGridParam('page');
            var size = $grid.getGridParam('rowNum');

            var serverPaging = $("#IsServerPaging").val();
            if (serverPaging === "1") {
                url = QueryString.setParam(url, "PageIndex", page);
                url = QueryString.setParam(url, "PageSize", size);
            }

            var sortParams = BetList.getSortParams();
            if (sortParams) {
                url = QueryString.setParam(url, "SortBy", sortParams.SortBy);
                url = QueryString.setParam(url, "SortOrder", sortParams.SortOrder);
            }

            url = QueryString.setParam(url, "IsExportExcel", true);
            window.location.href = url;
        }
    };
})(jQuery);
if (!window._IsInJsTestingMode) {
    $(window).load(function () {
        "use strict";
        CommonBetList.initialize();
    });
}