"use strict";
define([], function () {
    var state;

    var _pagingOption = {
        pageindex: 1,
        pagesize: 1000,
        pagelist: [1000, 2000, 3000, 4000],
        totalrow: 1,
        elementcontainer: ".paging",
        method: "AJAX",
        ajaxfunction: "CustomerList.ajaxPaging()",
        alwayshow: true
    };

    function initState() {
        return {
            cachedData: {},
            selectedCustomers: {},
            checkedAllRows: false,
            pagingOption: _pagingOption
        };
    }

    return {
        getState: function () {
            if (!state) {
                state = initState();
            }

            return state;
        }
    };
});