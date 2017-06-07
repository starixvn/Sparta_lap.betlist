define(["jquery", "common/site", "common/querystring", "./customerlist", "./customerlist.commision"],
    function ($, Site, QueryString, CustomerList, CustomerComm) {
        "use strict";

        $(document).ready(function () {
            CustomerList.initialize();

            if (QueryString.getParam(window.location.href, "userName") ||
                QueryString.getParam(window.location.href, "groupId")) {
                setTimeout(CustomerList.submit, 500);
            } else {
                Site.unBlockUI();
            }

            CustomerComm.initialize();
        });
    });