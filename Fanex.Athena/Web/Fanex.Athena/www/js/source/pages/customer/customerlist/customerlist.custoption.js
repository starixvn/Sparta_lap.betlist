define(["jquery", "common/site", "common/querystring", "common/popup", "./customerlist.service"],
    function ($, Site, QueryString, Popup, CustomerListService) {
        "use strict";

        var OPTIONLIST_SELECTOR = "#ViewOptionList";

        function getCustomerInfo() {
            var $optionList = $(OPTIONLIST_SELECTOR);
            return {
                CustId: $optionList.data("custid"),
                SystemId: $optionList.data("systemid"),
                ExternalId: $optionList.data("externalid"),
            };
        }

        var _me = {
            initialize: function () {
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

                var popupOption = { id: "viewOptionBetSettingFrame", width: 905, height: 600, title: Site.getSingleResource("BetSetting") };
                Popup.openPopupWithIframe(url, popupOption);
            },

            viewLogInfo: function () {
                _me.closeCustOption();

                var customer = getCustomerInfo();
                var url = Site.resolveClientUrl("Customer/LogInfo");
                url = QueryString.setParams(url, customer);

                var popupOption = { width: 360, height: 280, title: Site.getSingleResource("LogInfo") };

                var $popup = $('<div style="display: none" class="customer-logInfo"></div>');

                $popup.load(url, function (response, status) {
                    if (status === "success") {
                        Popup.openPopup($(this), popupOption);
                    }
                });

                return false;
            },

            viewBetList: function () {
                _me.closeCustOption();
                var customer = getCustomerInfo();

                var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
                url = QueryString.setParams(url, customer);
                url = QueryString.setParam(url, "betlistname", "betlist");
                url = Site.setLicenseeOptionOriginalParam(url);

                var popH = Math.min(window.innerHeight - 100, 600), popW = 900;
                var popupOption = { id: "viewOptionFrame", width: popW + 30, height: popH + 70 };

                Popup.openPopupWithIframe(url, popupOption);
            },

            viewStatement: function () {
                var customer = getCustomerInfo();
                _me.closeCustOption();

                var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
                url = QueryString.setParams(url, customer);
                url = QueryString.setParam(url, "betlistname", "dailystatement");
                url = Site.setLicenseeOptionOriginalParam(url);

                var popH = Math.min(window.innerHeight - 100, 600), popW = 900;
                var popupOption = { id: "viewOptionFrame", width: popW + 30, height: popH + 70 };

                Popup.openPopupWithIframe(url, popupOption);
            }
        };

        return _me;
    });