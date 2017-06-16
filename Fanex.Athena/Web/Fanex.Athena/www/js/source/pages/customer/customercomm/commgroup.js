define(["jquery", "common/site"], function ($, Site) {
    "use strict";

    function goToSite(relativeUrl, paramString) {
        var href = Site.resolveClientUrl(relativeUrl);
        if (paramString) {
            href += paramString;
        }
        Site.redirect(href);
    }

    function goToCustomerList(groupCommId) {
        if (isParentSameOrigin()) {
            $("li>#MenuItem55", window.parent.document).parent().removeClass("active");
            $("li>#MenuItem43", window.parent.document).parent().addClass("active");
            _me.setPageTitle(Site.getSingleResource("CustomerList"));
        }
        _me.goToSite("Customer/CustomerList", "?groupId=" + groupCommId);
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