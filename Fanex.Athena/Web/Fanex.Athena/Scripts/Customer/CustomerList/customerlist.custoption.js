/*global Site, QueryString */
/*exported CustomerListCustOption*/

var CustomerListCustOption = (function ($) {
    "use strict";

    var MENU_SELECTOR = "#view-cust-option";

    function getCustomerInfo() {
        var $menu = $(MENU_SELECTOR);
        return {
            CustId: $menu.attr("custid"),
            SystemId: $menu.attr("systemid"),
            ExternalId: $menu.attr("externalid"),
        };
    }

    function viewCustOption($context) {
        var custId = $context.attr("custid");
        var popup = $(MENU_SELECTOR);
        var link = $("#cust-option-link-" + custId);

        var position = $context.offset();
        popup.css({
            top: position.top + $context.height(),
            left: $context.offset().left
        });

        if (popup.attr("custid") === custId) {
            popup.toggle();
        }
        else {
            popup.attr("custid", custId);
            popup.attr("externalid", link.attr("externalid"));
            popup.attr("systemid", link.attr("systemid"));
            popup.show();
        }

        return false;
    }

    function closeCustOption() {
        var popup = $(MENU_SELECTOR);
        if (popup.is(":visible")) {
            popup.hide();
        }
    }

    function viewBetSetting() {
        var customer = getCustomerInfo();

        closeCustOption();

        var url = Site.resolveClientUrl("SportsbookBetSetting/Index?custid=" +
            customer.CustId + "&username=" + customer.SystemId + "&nickname=" + customer.ExternalId);

        var popH = 480, popW = 905;

        var dialogFrame = $("#bet-setting-frame");
        dialogFrame.attr("height", popH);
        dialogFrame.attr("width", popW);
        dialogFrame.attr("src", url);

        var dialogForm = $('#bet-setting-dialog');
        dialogForm.dialog({
            modal: true,
            width: popW + 15,
            height: popH + 70,
            close: function () {
                dialogFrame.attr("src", "");
            }
        });
    }

    function viewLogInfo() {
        closeCustOption();

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
    }

    function viewBetList() {
        closeCustOption();
        var customer = getCustomerInfo();
        var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
        url = QueryString.setParams(url, customer);
        url = QueryString.setParam(url, "betlistname", "betlist");
        url = Site.setLicenseeOptionOriginalParam(url);
        var popH = Math.min(window.innerHeight - 100, 500), popW = 900;

        var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('BetList') + '"><iframe scrolling="no" frameborder="0" width="' +
                     popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

        $(dialogForm).dialog({
            modal: true,
            width: popW + 30,
            height: popH + 70
        });
    }

    function viewStatement() {
        var customer = getCustomerInfo();

        closeCustOption();
        var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
        url = QueryString.setParams(url, customer);
        url = QueryString.setParam(url, "betlistname", "dailystatement");
        url = Site.setLicenseeOptionOriginalParam(url);
        var popH = Math.min(window.innerHeight - 100, 500), popW = 900;

        var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('DailyStatement') + '"><iframe frameborder="0" width="' +
                    popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

        $(dialogForm).dialog({
            modal: true,
            width: popW + 30,
            height: popH + 70
        });
    }

    function viewRunningBetlist(custId, systemId, externalId) {
        var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
        url = QueryString.setParam(url, "custid", custId);
        url = QueryString.setParam(url, "externalid", externalId);
        url = QueryString.setParam(url, "systemid", systemId);
        url = QueryString.setParam(url, "betlistname", "betlistrunning");
        url = Site.setLicenseeOptionOriginalParam(url);

        var popH = Math.min(window.innerHeight - 100, 700), popW = 900;

        var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('BetListRunning') + '"><iframe frameborder="0" width="' +
                     popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

        $(dialogForm).dialog({
            modal: true,
            width: popW + 30,
            height: popH + 70
        });
    }

    return {
        viewCustOption: viewCustOption,
        closeCustOption: closeCustOption,
        viewBetSetting: viewBetSetting,
        viewLogInfo: viewLogInfo,
        viewBetList: viewBetList,
        viewStatement: viewStatement,
        viewRunningBetlist: viewRunningBetlist
    };
})(jQuery);