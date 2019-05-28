/*global Site*/
/*exported ProductListHelper*/
///
"use strict";
var ProductListHelper = (function () {
    return {
        initProductList: function () {
            if ($('#ProductList').length === 0) {
                return;
            }

            Site.setMultiSelectForDropDownList('grid-products-multiselect', $("#productsData").val(), 'Code', 'Name', 'Status');
            $("form").submit(function () {
                if ($("#product-container input[type=checkbox]:checked").length === 0) {
                    $('<div title="' + Site.getSingleResource('LrfNotification') + '">' + Site.getSingleResource('AlertChooseProductMsg') + '!</div>').dialog();
                    Site.unBlockUI();
                    return false;
                }
                return true;

            });
        }
    };
}());