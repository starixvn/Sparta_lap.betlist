"use strict";
define([
    "jquery",
    "common/site",
    "common/querystring",
    "components/pluginhelper/daterangepicker",
    "components/scrollablegrid",
    "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, GridHelper) {
        var _me = {
            initialize: function () {
                Site.initialize();
                GridHelper.resizeGrid();
                _me.initDateRangePicker();
                Site.createSortingIcon(_me.submitForm);
                Site.addUserGuide({
                    resourceName: "WinLossByBetType",
                    title: Site.getSingleResource("WinLossByBetType")
                });
                _me.initEvent();
            },

            initDateRangePicker: function () {
                var ranges = ["Today", "Yesterday", "Currentweek"];
                DatePicker.initDateRangePicker(false, ranges);
            },

            initEvent: function () {
                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    $('#WinLossByBetTypeForm').submit();
                });
                $('#SubmitButton').click(function () {
                    _me.submitForm();
                });

                $(".body-content").on("click", ".bet-list-link", function () {
                    _me.viewBetList($(this).data("bettype"));
                });

                $(window).on("resize", function () {
                    GridHelper.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            viewBetList: function (betType) {
                var url = Site.resolveClientUrl("WinLossByBetTypeBetList/Index");
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "fromdate", $("#FromDate").val());
                url = QueryString.setParam(url, "todate", $("#ToDate").val());
                url = QueryString.setParam(url, "bettype", betType);

                Site.blockUI();
                Site.redirect(url);
            },

            submitForm: function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                $('#WinLossByBetTypeForm').submit();
                return false;
            }
        };

        return _me;
    });