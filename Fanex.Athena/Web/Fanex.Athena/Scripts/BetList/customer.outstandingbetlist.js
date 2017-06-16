/*global Site, Report, QueryString, CustomerDailyStatement, BetList, ProductListHelper*/
/// <reference path="/@JSense.js" />
"use strict";
var OutstandingBetList = (function ($) {
    function setProductOptionParamList(params) {
        var productCodeList = $("#grid-products-multiselect").multiSelect("getCheckedItems");
        params.ProductCodeList = productCodeList.toString();
        return params;
    }

    return {
        initialize: function () {
            //show or hide products dropdown list by ViewOption
            OutstandingBetList.showHideProductsList();

            //Set columns need hide
            OutstandingBetList.hiddenColList();

            //Init event
            OutstandingBetList.initEvent();

            ProductListHelper.initProductList();

            //Get params send from URL
            var params = OutstandingBetList.getParamsSubmit();
            OutstandingBetList.submit(params);
        },
        initEvent: function () {
            $("#ViewOption").change(function () {
                Site.blockUI();
                var betlistName = $("#ViewOption").find(":selected").val();
                var title = betlistName === "betlistrunning"
                    ? Site.getSingleResource("BetListRunning")
                    : (betlistName === "betlist"
                            ? Site.getSingleResource("BetList")
                            : Site.getSingleResource("DailyStatement"));

                var $popupContainer = $(window.frameElement).closest(".ui-dialog");
                $('.ui-dialog-title', $popupContainer).html(title);

                var currentcurrency = QueryString.getParam(location.href, "currencyname");
                var url = location.href;
                url = QueryString.setParam(url, "currencyname", currentcurrency);
                url = QueryString.setParam(url, "betlistname", betlistName);
                url = Site.setProductOptionOriginalParamList(url);
                Site.redirect(url);
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

            if (betlistName === "betlistrunning") {
                columsHidden = "externalid,systemid,winloss,licenseewl";
            }
            else if (betlistName === "betlist") {
                columsHidden = "externalid,systemid";
            }
            else {
                columsHidden = "externalid,systemid";
            }
            $("#HiddenColumnList").val(columsHidden);
        },

        showHideProductsList: function () {
            var productsListContainer = $("#ProductList");
            var betlistName = $("#ViewOption").find(":selected").val();
            if (betlistName === "dailystatement") {
                productsListContainer.prev().hide();
            }
            else {
                //Remove new-line class of label to display inline, because it's a common function in razorhelper
                productsListContainer.prev().removeClass("new-line");
            }
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

            return setProductOptionParamList(params);
        },

        submitCompletedBetList: function (params) {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/OutstandingCompletedBetList/Index');
            BetList.loadData(params, url);
        },

        submitRunningBetList: function (params) {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/OutstandingRunningBetList/Index');
            BetList.loadData(params, url);
        },

        submitDailyStatementBetList: function (params) {
            BetList.initialize();
            var url = Site.resolveClientUrl('api/OutstandingDailyStatementBetList/Index');
            CustomerDailyStatement.loadData(params, url);
        },

        submit: function (params) {
            BetList.ValidProducts();
            var betlistName = $("#ViewOption").find(":selected").val();
            if (betlistName === "dailystatement") {
                $("#ProductList").hide();
                OutstandingBetList.submitDailyStatementBetList(params);
            }
            else if (betlistName === "betlist") {
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

            var url = Site.resolveClientUrl('api/DailyStatementBetList/Index');

            var params = $.parseJSON($("#BetListMetaModel").val());

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

if (!window._IsInJsTestingMode) {
    $(window).load(function () {
        OutstandingBetList.initialize();
    });
}