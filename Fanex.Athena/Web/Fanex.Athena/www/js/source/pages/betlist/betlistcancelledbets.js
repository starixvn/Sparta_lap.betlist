"use strict";
define(
    ['jquery', 'common/site', 'common/querystring', './betlist'],
    function ($, Site, QueryString, BetList) {
        var _me = {
            initialize: function () {
                Site.initialize();
                _me.initEvent();
                _me.submit();
                Site.addUserGuide({
                    resourceName: "CancelledBets",
                    title: Site.getSingleResource("CancelledBets")
                });
            },

            initEvent: function () {
                $('#CustId').change(function () {
                    _me.submit();
                });

                $('#SubmitButton').click(function () {
                    $('#IsExportExcel').val(false);
                    _me.submit();
                });

                $("#ExportToExcel").click(function () {
                    _me.exportToExcel();
                });

                $(window).on("resize", function () {
                    BetList.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },
            submit: function () {
                var params = { custId: $("#CustId").val() };
                params = Site.setLicenseeOptionParam(params);

                var url = Site.resolveClientUrl('api/CancelledBetsBetList/Index');
                BetList.loadData(params, url);
            },

            exportToExcel: function () {
                var url = Site.getCurrentUrl();
                url = Site.setLicenseeOptionParam(url);
                url = QueryString.setParam(url, "custId", $("#CustId").val());
                $('#IsExportExcel').val(true);
                $('#CancellerBetsBetListForm').submit();
            }
        };

        return _me;
    });