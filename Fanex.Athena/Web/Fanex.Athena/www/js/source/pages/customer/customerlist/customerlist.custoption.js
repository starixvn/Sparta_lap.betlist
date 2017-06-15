define(["jquery", "common/site", "common/querystring", "common/popup", "./customerlist.service"],
    function ($, Site, QueryString, Popup, CustomerListService) {
        "use strict";

        var OPTIONLIST_SELECTOR = "#ViewOptionList";

        function getCustomerInfo() {
            var $menu = $(OPTIONLIST_SELECTOR);
            return {
                CustId: $menu.attr("custid"),
                SystemId: $menu.attr("systemid"),
                ExternalId: $menu.attr("externalid"),
            };
        }

        var _me = {
            initialize: function(){
                $("#report-content").on("click", ".custoptionlink", function () {
                    CustomerListService.showGridDropdownById("ViewOptionList", $(this));
                });

                $("#bet-setting-link").on("click", _me.viewBetSetting);
                $("#bet-list-link").on("click", _me.viewBetList);
                $("#statement-link").on("click", _me.viewStatement);
                $("#login-info-link").on("click", _me.viewLogInfo);
            },

            closeCustOption: function () {
                var popup = $(OPTIONLIST_SELECTOR);
                if (popup.is(":visible")) {
                    popup.hide();
                }
            },

            viewBetSetting: function () {
                var customer = getCustomerInfo();

                _me.closeCustOption();

                var url = Site.resolveClientUrl("SportsbookBetSetting/Index?custid=" +
                    customer.CustId + "&username=" + customer.SystemId + "&nickname=" + customer.ExternalId);

                var popupOption = { width: 905, height: 480, title: Site.getSingleResource("BetSetting") };
                Popup.openPopupWithIframe(url, popupOption);
            },

            viewLogInfo: function () {
                _me.closeCustOption();

                var customer = getCustomerInfo();
                var url = QueryString.setParams("Customer/LogInfo", customer);
                url = Site.resolveClientUrl(url);

                var dialog = $('<div title="' + Site.getSingleResource('LogInfo') + '" style="display:none" class="loading"></div>').appendTo('body');

                dialog.dialog({
                    close: function () {
                        dialog.remove();
                    },
                    width: 360,
                    height: 280,
                    modal: true
                });
                dialog.load(url,
                    {},
                    function () {
                        dialog.removeClass('loading');
                    }
                );
                return false;
            },

            viewBetList: function () {
                _me.closeCustOption();
                var customer = getCustomerInfo();

                var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
                url = QueryString.setParams(url, customer);
                url = QueryString.setParam(url, "betlistname", "betlist");
                url = Site.setLicenseeOptionOriginalParam(url);

                var popH = Math.min(window.innerHeight - 100, 500), popW = 900;
                var popupOption = { width: popW + 30, height: popH + 70, title: Site.getSingleResource("BetList") };

                Popup.openPopupWithIframe(url, popupOption);
            },

            viewStatement: function () {
                var customer = getCustomerInfo();
                _me.closeCustOption();

                var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
                url = QueryString.setParams(url, customer);
                url = QueryString.setParam(url, "betlistname", "dailystatement");
                url = Site.setLicenseeOptionOriginalParam(url);

                var popH = Math.min(window.innerHeight - 100, 500), popW = 900;
                var popupOption = { width: popW + 30, height: popH + 70, title: Site.getSingleResource("DailyStatement") };

                Popup.openPopupWithIframe(url, popupOption);
            }
        };

        return _me;
    });