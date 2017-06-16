"use strict";
define(["jquery", "common/site", "common/querystring", "pages/report/report", "./customer.dailystatement", "../betlist",
            "components/pluginhelper/daterangepicker", "components/pluginhelper/multiselect"],
    function ($, Site, QueryString, Report, DailyStatement, BetList, DatePicker, MultiSelect) {
        var _viewName;
        var _viewOption;

        function getViewOption() {
            return {
                "betlistrunning": { title: Site.getSingleResource("BetListRunning"), hiddenColumn: "externalid,systemid,winloss,licenseewl", submit: _me.submitRunningBetList },
                "betlist": { title: Site.getSingleResource("BetList"), hiddenColumn: "externalid,systemid", submit: _me.submitCompletedBetList },
                "dailystatement": { title: Site.getSingleResource("DailyStatement"), hiddenColumn: "externalid,systemid", submit: _me.submitDailyStatementBetList }
            }
        }

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                MultiSelect.initProductList();

                _viewName = $("#ViewOption").val();
                _viewOption = getViewOption();
                _me.updatePopupTitle();

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
                    var betlistName = $(this).val();
                    var url = Site.getCurrentUrl();
                    url = QueryString.setParam(url, "betlistname", betlistName);
                    url = Site.setProductOptionOriginalParamList(url);

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

            updatePopupTitle: function () {
                var title = _viewOption[_viewName].title;
                var $frame = parent.$("#popupFrame");
                var $popUp = $frame.closest(".flat-popup");
                $popUp.find(".txt-title").text(title);
            },

            setHiddenColumns: function () {
                //Set string columns need to hide
                var columsHidden = _viewOption[_viewName].hiddenColumn;
                $("#HiddenColumnList").val(columsHidden);
            },

            showHideProductsList: function () {
                var $productsListContainer = $("#ProductList");
                var isHideProductList = _viewName === "dailystatement";

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

                _viewOption[_viewName].submit(params);
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