"use strict";
define(["jquery", "common/site", "common/querystring", "lib/jqgrid/jquery.jqgrid.additional", "./customerlist.filter", "./customerlist.grid",
    "./customerlist.service", "./customerlist.service.dormant", "./customerlist.commision", "./customerlist.custoption"],
    function ($, Site, QueryString, jqGridAdditional, CustomerListFilter, CustomerListGrid,
        CustomerListService, CustomerListServiceDormant, CustomerComm, CustomerListViewOption) {

        $(document).ready(function () {
            Site.initialize();
            $(".tablinks").click(function () {
                var tabId = $(this).data('tab');
                var $tab = $("#" + tabId);
                $(".tabcontent").hide();
                $(".tablinks").removeClass('active');

                $tab.show();
                $(this).addClass("active");

                var isDormantTab = tabId == 'tabDormantCustomer';
                $(".page-button input[type=button]").prop('disabled', isDormantTab);
                if (isDormantTab) {
                    jqGridAdditional.updateGridHeight($tab);
                }
            });

            CustomerListService.initialize();
            CustomerListServiceDormant.initialize();
            CustomerListFilter.initialize();
            CustomerListGrid.initialize();
            CustomerListViewOption.initialize();

            var url = Site.getCurrentUrl();
            if (QueryString.getParam(url, "userName") ||
                QueryString.getParam(url, "groupId")) {
                CustomerListService.submit();
                CustomerListServiceDormant.submit();
            } else {
                Site.unBlockUI();
            }

            CustomerComm.initialize();
        });
    });