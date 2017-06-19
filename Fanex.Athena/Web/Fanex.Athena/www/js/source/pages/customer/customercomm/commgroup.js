define(["jquery", "common/site"], function ($, Site) {
    "use strict";

    function goToSite(relativeUrl) {
        var href = Site.resolveClientUrl(relativeUrl);

        Site.redirect(href);
    }

    function goToCustomerList(groupCommId) {
        if (isParentSameOrigin()) {
            // Remove active class for Commission Group List(55) menu item and add active class for Customer List(43) menu item 
            $("li>#MenuItem55", window.parent.document).parent().removeClass("active");
            $("li>#MenuItem43", window.parent.document).parent().addClass("active");
            _me.setPageTitle(Site.getSingleResource("CustomerList"));
        }
        _me.goToSite("Customer/CustomerList?groupId=" + groupCommId);
    }

    function setPageTitle(title) {
        $("#page-title", window.parent.document).text(title);
    }

    function isParentSameOrigin() {
        var sameOrigin;
        try {
            sameOrigin = window.parent.location.host === window.location.host;
        }
        catch (e) {
            sameOrigin = false;
        }
        return sameOrigin;
    }

    var _me = {
        goToSite: goToSite,
        goToCustomerList: goToCustomerList,
        setPageTitle: setPageTitle
    };

    return _me;
});