"use strict";
define(["jquery", "common/site", "common/querystring", "./customerlist.filter", "./customerlist.grid", "./customerlist.service", "./customerlist.commision", "./customerlist.custoption"],
    function ($, Site, QueryString, CustomerListFilter, CustomerListGrid, CustomerListService, CustomerComm, CustomerListViewOption) {
        $(document).ready(function () {
            Site.initialize();
            CustomerListService.initialize();
            CustomerListFilter.initialize();
            CustomerListGrid.initialize();
            CustomerListViewOption.initialize();

            if (QueryString.getParam(window.location.href, "userName") ||
                QueryString.getParam(window.location.href, "groupId")) {
                setTimeout(CustomerListService.submit, 500);
            } else {
                Site.unBlockUI();
            }

            CustomerComm.initialize();
        });
    });