/*global Site*/
/*exported Report*/
/// <reference path="/@JSense.js" />
"use strict";
var ProductListHelper = (function () {
    return {
        initProductList: function () {
            Site.setMultiSelectForDropDownList('grid-products-multiselect', $("#productsData").val(), 'Code', 'Name', 'Status');
            $("form").submit(function (event) {
                if ($("#product-container input[type=checkbox]:checked").length == 0) {
                    $('<div title="@ViewResources.LrfNotification">' + '@ViewResources.AlertChooseProductMsg' + '!</div>').dialog();
                    Site.unBlockUI();
                    return false;
                }
                return true;

            });
        }
    };
}());