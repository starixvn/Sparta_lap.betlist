"use strict";
define(['jquery', 'common/site', 'common/querystring', './betlist', 'components/pluginhelper/daterangepicker'], function ($, Site, QueryString, BetList, DatePicker) {
    window.serverPaging = true; // used in betlist.js
    var KEENOCEAN_LICENSEEID = 96;

    var showOrHideExcelIcon = function () {
        var $exportExcel = $("#ExportToExcel");
        if (Site.getLicenseeId() === KEENOCEAN_LICENSEEID) {
            $exportExcel.show();
            return;
        }

        var currency = parseInt($("#CurrencyId").val(), 10);
        $exportExcel.toggle(currency !== 0);
    };

    var _me = {
        initialize: function () {
            Site.initialize();
            _me.initEvent();
            showOrHideExcelIcon();
            Site.addUserGuide({
                resourceName: "DailyBetList",
                title: Site.getSingleResource("DailyBetList")
            });
            DatePicker.initSingleDatePicker("Date");
        },

        initEvent: function () {
            $('#SubmitButton').click(function () {
                _me.submit();
            });

            $("#ExportToExcel").on("click", function () {
                _me.exportToExcel();
            });

            $("#CurrencyId").change(function () {
                showOrHideExcelIcon();
            });

            $(window).on("resize", function () {
                BetList.resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        },

        submit: function () {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/DailyBetList/index');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "productCode", $("#ProductCode").val());
            url = QueryString.setParam(url, "status", $("#DailyBetListStatus").val());
            url = QueryString.setParam(url, "date", $("#Date").val());
            BetList.loadDataPaging(url);
        },

        exportToExcel: function () {
            var url = Site.getCurrentUrl();
            var size = $('#BetListGrid').getGridParam('rowNum');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "IsExportExcel", true);
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "productCode", $("#ProductCode").val());
            url = QueryString.setParam(url, "DailyBetListStatus", $("#DailyBetListStatus").val());
            url = QueryString.setParam(url, "PageIndex", 1);
            url = QueryString.setParam(url, "PageSize", size);
            var sortParams = BetList.getSortParams();
            if (sortParams) {
                url = QueryString.setParam(url, "SortBy", sortParams.SortBy);
                url = QueryString.setParam(url, "SortOrder", sortParams.SortOrder);
            }
            Site.redirect(url);
        }
    };

    return _me;
});