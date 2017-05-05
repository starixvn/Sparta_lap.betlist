"use strict";
define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker", "components/scrollablegrid", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, GridHelper) {
        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker("Date");
                Site.addUserGuide({
                    resourceName: "DailyRacingWL",
                    title: Site.getSingleResource("DailyRacingWinLoss")
                });
                _me.initEvent();
                GridHelper.resizeGrid();
            },
            initEvent: function () {
                $('#SubmitButton').click(function () {
                    _me.submitForm();
                    return false;
                });

                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    $('#ReportForm').submit();
                });

                $(".body-content").on("click", ".bet-list-link", function () {
                    var target = $(this);
                    var leagueId = target.attr("data-league");
                    var raceNo = target.attr("data-race-no");
                    var currency = target.attr("data-currency");
                    _me.viewRacingWinlossBetList(leagueId, raceNo, currency);
                });

                $(window).on("resize", function () {
                    GridHelper.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },
            viewRacingWinlossBetList: function (leagueid, raceNo, currency) {
                var url = Site.resolveClientUrl("DailyRacingWinLossBetList/Index");
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "date", $("#Date").val());
                url = QueryString.setParam(url, "leagueid", leagueid);
                url = QueryString.setParam(url, "raceno", raceNo);
                url = QueryString.setParam(url, "type", "RacingWinloss");
                url = QueryString.setParam(url, "currencyid", currency);

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