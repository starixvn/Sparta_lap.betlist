"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service", "./customerlist.service.dormant",
    "components/pluginhelper/autocomplete", "lib/jquery.multiselect"],
    function ($, Site, CustomerListState, CustomerListService, CustomerListServiceDormant, AutoComplete) {
        var _state;
        var _dormantState;

        var _me = {
            initialize: function () {
                _state = CustomerListState.getState();
                _dormantState = CustomerListState.getDormantState();
                Site.addUserGuide({
                    resourceName: "CustomerList",
                    title: Site.getSingleResource("CustomerList")
                });
                AutoComplete.initialize({
                    url: Site.resolveClientUrl("SearchCustomerAPI/SearchActiveAndDormantCustomer"),
                    target: "#Username"
                });
                _me.initEvent();
            },
            submit: function(){
                _state.pagingOption.pageindex = 1;
                _dormantState.pagingOption.pageindex = 1;
                CustomerListService.submit();
                CustomerListServiceDormant.submit();
                $(".page-results").show();
            },
            initEvent: function () {
                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $("#Username").on('keyup', function (e) {
                    if (e.keyCode === 13) {
                        _state.pagingOption.pageindex = 1;
                        _dormantState.pagingOption.pageindex = 1;
                        CustomerListService.submit();
                        CustomerListServiceDormant.submit();
                    }
                });

                _me.initGridColumnOption();
                _me.initOddGroupOption();
                _me.initSubsidiaryOption();
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
                        $("#SubmitButton").prop("disabled", checkedItems.length === 0);
                    }
                });
            },

            initSubsidiaryOption: function () {
                var data = $("#SubsidiaryList").val();
                if (!data) {
                    return;
                }

                data = $.parseJSON(data);
                var totalColumns = data.length;
                var dataUI = [{
                    Value: '(others)',
                    Text: Site.getSingleResource('Others'),
                    Checked: true,
                    Enable: true
                }];
                var checkedCount = 0;
                for (var i = 0; i < totalColumns; i++) {
                    var isChecked = Site.asBool(data[i].Checked);
                    if (isChecked) {
                        checkedCount++;
                    }

                    dataUI.push({
                        Value: data[i].Name,
                        Text: data[i].Name,
                        Checked: isChecked,
                        Enable: true
                    });
                }

                var ischeckAll = checkedCount === totalColumns;
                $("#subsidiary-multiselect").multiSelect(
                {
                    minWidth: 100,
                    maxHeight: 400,
                    shadow: true,
                    showSearchBox: true,
                    showHeader: true,
                    isCheckAll: ischeckAll,
                    showFooter: false,
                    checkAllText: Site.getSingleResource('All'),
                    selectedText: Site.getSingleResource("SelectedItemCount"),
                    listData: dataUI,
                    onCheck: function () {
                        var checkedItems = $("#subsidiary-multiselect").multiSelect("getCheckedItems");
                        $("#SubmitButton").prop("disabled", checkedItems.length === 0);
                    }
                });
            }
        };

        return _me;
    });