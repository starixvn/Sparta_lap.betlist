"use strict";
define(["jquery", "common/site", "common/querystring", "pages/report/report", "./customer.dailystatement", "../betlist",
            "components/pluginhelper/daterangepicker", "components/pluginhelper/multiselect"],
    function ($, Site, QueryString, Report, DailyStatement, BetList, DatePicker, MultiSelect) {
        var Title;
        var HiddenColumn = {
            "betlistrunning": "externalid,systemid,winloss,licenseewl",
            "betlist": "externalid,systemid",
            "dailystatement": "externalid,systemid"
        };

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                MultiSelect.initProductList();
                Title = {
                    "betlistrunning": Site.getSingleResource("BetListRunning"),
                    "betlist": Site.getSingleResource("BetList"),
                    "dailystatement": Site.getSingleResource("DailyStatement")
                };

                //show or hide products dropdown list by ViewOption
                _me.showHideProductsList();

                //Set columns need hide
                _me.setHiddenColumns();

                _me.initEvent();

                var params = _me.getParamsSubmit();
                _me.submit(params);
            },
            initEvent: function () {
                $("#ViewOption").change(function () {
                    Site.blockUI();
                    var betlistName = $("#ViewOption").find(":selected").val();
                    var title = Title[betlistName];

                    var url = Site.getCurrentUrl();
                    url = QueryString.setParam(url, "betlistname", betlistName);
                    url = Site.setProductOptionOriginalParamList(url);

                    var $frame = parent.$("#popupFrame");
                    var $popUp = $frame.closest(".flat-popup");
                    $popUp.find(".txt-title").text(title);

                    Site.redirect(url);
                });

                $("#SubmitButton").click(function () {
                    Report.setOriginalProduct();
                    var params = _me.getParamsSubmit();
                    _me.submit(params);
                });

                $("#report-container").on("click", ".betlistDrilldown", function () {
                    var param = $(this).data("param");
                    _me.viewDailyStatementBetList(param);
                });
            },

            setHiddenColumns: function () {
                //Get item chooses from dropdownlist and set string columns need to hide
                var betlistName = $("#ViewOption").find(":selected").val();
                var columsHidden = HiddenColumn[betlistName];
                $("#HiddenColumnList").val(columsHidden);
            },

            showHideProductsList: function () {
                var $productsListContainer = $("#ProductList");
                var betlistName = $("#ViewOption").find(":selected").val();
                var isHideProductList = betlistName === "dailystatement";

                $productsListContainer.parent().toggleClass("hide", isHideProductList);
            },

            getParamsSubmit: function () {
                var params = $.parseJSON($("#BetListMetaModel").val());
                var url = Site.getCurrentUrl();
                params.CustId = QueryString.getParam(url, "custid");
                params.ExternalId = QueryString.getParam(url, "externalid");
                params.SystemId = QueryString.getParam(url, "systemid");
                params.FromDate = $("#FromDate").val();
                params.ToDate = $("#ToDate").val();

                return Site.setProductOptionParamList(params);
            },

            submitCompletedBetList: function (params) {
                var url = Site.resolveClientUrl('api/OutstandingCompletedBetList/Index');
                BetList.loadData(params, url);
            },

            submitRunningBetList: function (params) {
                var url = Site.resolveClientUrl('api/OutstandingRunningBetList/Index');
                BetList.loadData(params, url);
            },

            submitDailyStatementBetList: function (params) {
                var url = Site.resolveClientUrl('api/OutstandingDailyStatementBetList/Index');
                DailyStatement.loadData(params, url);
            },

            submit: function (params) {
                BetList.ValidProducts();
                BetList.registerBetListGlobalFunctions();

                var betlistName = $("#ViewOption").find(":selected").val();
                if (betlistName === "dailystatement") {
                    _me.submitDailyStatementBetList(params);
                }
                else if (betlistName === "betlist") {
                    _me.submitCompletedBetList(params);
                }
                else {
                    _me.submitRunningBetList(params);
                }
            },

            viewDailyStatementBetList: function (param) {
                BetList.registerBetListGlobalFunctions();

                var url = Site.resolveClientUrl('api/DailyStatementBetList/Index');

                var postData = $.parseJSON($("#BetListMetaModel").val());

                postData.CustId = param.CustId;
                postData.FromDate = param.Date;
                postData.TransType = param.TransType;
                postData.ExternalId = param.ExternalId;
                postData.SystemId = param.SystemId;

                $('.page-form').remove();
                BetList.loadData(postData, url);
            }
        };

        return _me;
    });