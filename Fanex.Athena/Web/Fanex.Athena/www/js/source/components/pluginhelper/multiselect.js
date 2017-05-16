"use strict";
define(["jquery", "common/site", "lib/jquery.multiselect"], function ($, Site) {
    var _me = {
        initProductList: function () {
            var selector = "#grid-products-multiselect";
            var data = $.parseJSON($("#productsData").val());
            var length = data.length;
            var dataUI = [];
            var checkedCount = 0;
            for (var i = 0; i < length; i++) {
                var item = data[i];
                var isChecked = item["Status"];
                if (isChecked) {
                    checkedCount++;
                }

                dataUI.push({
                    Id: item["Code"],
                    Value: item["Code"],
                    Text: Site.getSingleResource(item["Name"]),
                    Checked: isChecked,
                    Enable: true
                });
            }

            var isCheckAll = checkedCount === length;
            $(selector).multiSelect({
                minWidth: 200,
                maxHeight: 400,
                shadow: true,
                showHeader: true,
                isCheckAll: isCheckAll,
                checkAllText: Site.getSingleResource('All'),
                showFooter: false,
                listData: dataUI,
                onCheck: function () {
                    var checkedItems = $(selector).multiSelect("getCheckedItems");
                    $("#product-container input[type=checkbox]:checked").prop("checked", false);
                    var hasItemsChecked = checkedItems.length > 0;
                    if (hasItemsChecked) {
                        for (var i = 0; i < checkedItems.length; i++) {
                            $('#product-container #' + checkedItems[i]).prop("checked", true);
                        }
                    }
                }
            });
        }
    };

    return _me;
});