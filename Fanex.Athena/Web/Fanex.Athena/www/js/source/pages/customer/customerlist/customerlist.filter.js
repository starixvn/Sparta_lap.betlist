"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service", "components/pluginhelper/autocomplete", "lib/jquery.multiselect"],
    function ($, Site, CustomerListState, CustomerListService, AutoComplete) {
        var _state;

        var _me = {
            initialize: function () {
                _state = CustomerListState.getState();
                Site.addUserGuide({
                    resourceName: "CustomerList",
                    title: Site.getSingleResource("CustomerList")
                });
                AutoComplete.initialize({
                    url: Site.resolveClientUrl("CustomerAPI/SearchCustomer"),
                    target: "#Username"
                });
                _me.initEvent();
            },

            initEvent: function () {
                $('#SubmitButton').click(function () {
                    _state.pagingOption.pageindex = 1;
                    CustomerListService.submit();
                });

                _me.initGridColumnOption();
                _me.initOddGroupOption();
            },

            initGridColumnOption: function () {
                var data = $.parseJSON($("#AllowedGridColumns").val());
                var totalColumns = data.length;
                var dataUI = [];
                var checkedCount = 0;
                for (var i = 0; i < totalColumns; i++) {
                    var isChecked = Site.asBool(data[i].Checked);
                    if (isChecked) {
                        checkedCount++;
                    }
                    dataUI.push({
                        Id: data[i].Name,
                        Value: data[i].Name,
                        Text: Site.getSingleResource(data[i].Name),
                        Checked: isChecked,
                        Enable: true,
                        Css: "",
                        CssClass: ""
                    });
                }
                var ischeckAll = checkedCount === totalColumns;
                $("#grid-columns-multiselect").multiSelect({
                    minWidth: 160,
                    maxHeight: 400,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: ischeckAll,
                    showFooter: false,
                    checkAllText: Site.getSingleResource('All'),
                    noneSelectedText: Site.getSingleResource("Columns"),
                    selectedText: Site.getSingleResource("SelectedItemCount"),
                    listData: dataUI,
                    onCheck: function () {
                        var checkedItems = $("#grid-columns-multiselect").multiSelect("getCheckedItems");
                        if (checkedItems.length === 0) {
                            $("#SubmitButton").prop("disabled", true);
                            $("#grid-columns-multiselect-null").removeClass("hide");
                            $("#grid-columns-option-container input[type=checkbox]:checked").prop("checked", false);
                        }
                        else {
                            $("#grid-columns-option-container input[type=checkbox]:checked").prop("checked", false);
                            for (var i = 0; i < checkedItems.length; i++) {
                                $('#grid-columns-option-container input#' + checkedItems[i]).prop("checked", true);
                            }
                            $("#SubmitButton").prop("disabled", false);
                            $("#grid-columns-multiselect-null").addClass("hide");
                        }
                    }
                });
            },

            initOddGroupOption: function () {
                var data = $("#OddsGroups").val();
                if (!data) {
                    return;
                }

                data = $.parseJSON(data);
                var totalColumns = data.length;
                var dataUI = [];
                var checkedCount = 0;
                for (var i = 0; i < totalColumns; i++) {
                    var isChecked = Site.asBool(data[i].Checked);
                    if (isChecked) {
                        checkedCount++;
                    }

                    dataUI.push({
                        Id: data[i].Name,
                        Value: data[i].Name,
                        Text: Site.getSingleResource(data[i].Name),
                        Checked: isChecked,
                        Enable: true,
                        Css: "",
                        CssClass: ""
                    });
                }

                var ischeckAll = checkedCount === totalColumns;
                $("#odds-group-multiselect").multiSelect({
                    minWidth: 100,
                    maxHeight: 400,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: ischeckAll,
                    showFooter: false,
                    checkAllText: Site.getSingleResource('All'),
                    noneSelectedText: Site.getSingleResource("Odds"),
                    selectedText: Site.getSingleResource("SelectedItemCount"),
                    listData: dataUI,
                    onCheck: function () {
                        var checkedItems = $("#odds-group-multiselect").multiSelect("getCheckedItems");
                        if (checkedItems.length === 0) {
                            $("#SubmitButton").prop("disabled", true);
                            $("#odds-group-multiselect-null").removeClass("hide");
                        }
                        else {
                            $("#SubmitButton").prop("disabled", false);
                            $("#odds-group-multiselect-null").addClass("hide");
                        }
                    }
                });
            }
        };

        return _me;
    });