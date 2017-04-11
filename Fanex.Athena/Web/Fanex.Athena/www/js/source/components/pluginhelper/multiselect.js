"use strict";
define(["jquery", "common/site", "lib/jquery.multiselect"], function ($, Site) {
    var _me = {
        initialize: function (dropDownListId, dataSource, valueField, textField, checkField) {
            var data = $.parseJSON(dataSource);
            var length = data.length;
            var dataUI = [];
            var checkedCount = 0;
            for (var i = 0; i < length; i++) {
                var item = data[i];
                var isChecked = item[checkField];
                if (isChecked) {
                    checkedCount++;
                }

                dataUI.push({
                    Id: item[valueField],
                    Value: item[valueField],
                    Text: Site.getSingleResource(item[textField]),
                    Checked: isChecked,
                    Enable: true
                });
            }

            var isCheckAll = checkedCount === length;
            $("#" + dropDownListId).multiSelect({
                minWidth: 200,
                maxHeight: 400,
                shadow: true,
                showHeader: true,
                isCheckAll: isCheckAll,
                checkAllText: Site.getSingleResource('All'),
                showFooter: false,
                listData: dataUI,
                onCheck: function () {
                    var checkedItems = $("#" + dropDownListId).multiSelect("getCheckedItems");
                    $("#product-container input[type=checkbox]:checked").prop("checked", false);
                    var hasItemsChecked = checkedItems.length > 0;
                    if (hasItemsChecked) {
                        for (var i = 0; i < checkedItems.length; i++) {
                            $('#product-container #' + checkedItems[i]).prop("checked", true);
                        }
                    }

                    $("#SubmitButton").prop("disabled", !hasItemsChecked);
                    $(".error-img").toggleClass("hide", hasItemsChecked);
                }
            });
        }
    };

    return _me;
});