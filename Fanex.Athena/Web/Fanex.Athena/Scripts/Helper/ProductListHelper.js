/*global Site*/
/*exported ProductListHelper*/
/// <reference path="/@JSense.js" />
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
                    $('<div title="@ViewResources.LrfNotification">' + '@ViewResources.AlertChooseProductMsg' + '!</div>').dialog();
                    Site.unBlockUI();
                    return false;
                }
                return true;

            });
        }
    };
}());