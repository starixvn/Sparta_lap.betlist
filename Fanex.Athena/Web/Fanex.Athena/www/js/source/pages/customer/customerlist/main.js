"use strict";
define(["jquery", "common/site", "common/querystring", "./customerlist.filter", "./customerlist.grid", "./customerlist.service", "./customerlist.commision", "./customerlist.custoption"],
    function ($, Site, QueryString, CustomerListFilter, CustomerListGrid, CustomerListService, CustomerComm, CustomerListViewOption) {
        $(document).ready(function () {
            Site.initialize();
            CustomerListService.initialize();
            CustomerListFilter.initialize();
            CustomerListGrid.initialize();
            CustomerListViewOption.initialize();

            var url = Site.getCurrentUrl();
            if (QueryString.getParam(url, "userName") ||
                QueryString.getParam(url, "groupId")) {
                setTimeout(CustomerListService.submit, 500);
            } else {
                Site.unBlockUI();
            }

            CustomerComm.initialize();
        });
    });