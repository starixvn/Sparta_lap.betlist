"use strict";
define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker", "components/scrollablegrid", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, GridHelper) {
        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker("Date");
                Site.addUserGuide({
                    resourceName: "MatchWL",
                    title: Site.getSingleResource("MatchWinLoss")
                });
                _me.initEvent();
                GridHelper.resizeGrid();
            },

            initEvent: function () {
                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    $('#MatchWinLossForm').submit();
                });

                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $(".body-content").on("click", ".view-match-detail", function () {
                    _me.viewMatchWinlossDetail(this);
                })
                .on("click", ".match-link", function () {
                    _me.viewMatchWinlossBetList(this);
                });
            },

            submit: function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                $('#MatchWinLossForm').submit();
            },

            getProductOption: function (productCode) {
                var productOptions = JSON.parse($("#productsData").val());

                return productOptions.map(function (item) {
                    return {
                        Code: item.Code,
                        Status: item.Code === productCode
                    }
                });
            }
        };

        return _me;
    });