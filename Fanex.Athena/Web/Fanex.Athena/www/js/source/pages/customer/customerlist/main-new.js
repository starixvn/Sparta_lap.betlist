"use strict";
define(["jquery", "common/site", "common/querystring", "./customerlist.filter", "./customerlist.grid", "./customerlist.service", "./customerlist.commision"],
    function ($, Site, QueryString, CustomerListFilter, CustomerListGrid, CustomerListService, CustomerComm) {
        $(document).ready(function () {
            Site.initialize();
            CustomerListService.initialize();
            CustomerListFilter.initialize();
            CustomerListGrid.initialize();

            if (QueryString.getParam(window.location.href, "userName") ||
                QueryString.getParam(window.location.href, "groupId")) {
                setTimeout(CustomerListService.submit, 500);
            } else {
                Site.unBlockUI();
            }

            CustomerComm.initialize();
        });
    });