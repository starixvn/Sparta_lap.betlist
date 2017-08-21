define([
    "jquery",
    "common/querystring",
    "common/site",
    "lib/jqgrid/jquery.jqgrid.additional",
    "./customerturnover.option",
    "components/pluginhelper/autocomplete",
    "components/pluginhelper/daterangepicker",
    "helpers/daterange",
    "lib/jquery.multiselect",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension",
    "lib/linq"],
    function ($, QueryString, Site, jqGridAdditional, CustomerTurnoverOption, AutoComplete, DatePicker, DateHelper) {
        "use strict";

        var VALID_MONTH_RANGE = 1;
        var _sports = [];
        var _bettypes = [];
        var _checkedSports = [];
        var _checkedBettypes = [];
        var _url = {
            GetSports: "CustomerTurnoverReport/GetSportsAndProducts",
            GetBetTypes: "ReportAPI/GetBettypesBySport",
            SearchCustomer: "CustomerAPI/SearchCustomer"
        };
        var _selector = {
            SubmitButton: "#SubmitButton",
            ExportExcelInput: "#IsExportExcel",
            ExportToExcel: "#ExportToExcel",
            Form: "#CustomerTurnoverReport",
            Title: ".report-title",
            Header: ".panel-heading",
            GridId: "ReportGrid",
            Odds: "#Odds",
            TickerFilter: "#TickerFilter",
            Username: "#Username",
            ProductMultiSelect: "#grid-products-multiselect",
            BetTypeMultiSelect: "#grid-bettypes-multiselect",
            BetTypeList: "#BetTypeList",
            ProductList: "#ProductList"
        };
        var _ajaxRequestGetBettype;

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "CustomerTurnover",
                title: Site.getSingleResource("CustomerTurnover")
            });
            DatePicker.initDateRangePicker();
            DateHelper.setConstraintMonthRange(VALID_MONTH_RANGE, CustomerTurnoverOption.urlData);

            _me.initAutoComplete();
            _me.initTicketFilter();
            _me.initEvent();
            _me.initProductTree();
        }

        function initEvent() {
            $(_selector.Odds).keypress(function (event) {
                handleOddInput(event);
            });

            $(_selector.SubmitButton).click(function () {
                _me.submit();
            });

            $(_selector.ExportToExcel).click(function () {
                $(_selector.ExportExcelInput).val(true);
                var mainForm = $(_selector.Form);
                var url = Site.resolveClientUrl(CustomerTurnoverOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(_selector.ExportExcelInput).val(false);
                return false;
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        }

        function initTicketFilter() {
            $(_selector.TickerFilter + " input[type=checkbox]").checkboxgroup();
            $(_selector.TickerFilte).on("change", "input[type=checkbox]", function () {
                checkValidFilter($(this));
            });
        }

        function initAutoComplete() {
            var option = {
                url: Site.resolveClientUrl(_url.SearchCustomer),
                target: _selector.Username
            };

            AutoComplete.initialize(option);
        }

        function initProductTree() {
            $.getJSON(Site.resolveClientUrl(_url.GetSports), function (data) {
                _sports = data;
                _checkedSports = data;
                renderProductTree();
                /*loadd all bettype*/
                loadBettypes();
            });
        }

        function submit() {
        }

        function bindData() {
        }

        function renderProductTree() {
            var groupData = groupProductList();

            $(_selector.ProductMultiSelect).multiSelect({
                minWidth: 200,
                maxHeight: 400,
                hasOptGroup: true,
                listData: groupData,
                isUsedCustomCheckAllText: true,
                customCheckAllText: Site.getSingleResource('All'),
                checkAllText: Site.getSingleResource('All'),
                noItemText: Site.getSingleResource('NoProductFound'),
                unCheckAllText: Site.getSingleResource('UncheckAll'),
                updateResult: '',
                noneSelectedText: Site.getSingleResource('SelectOptions'),
                selectedText: Site.getSingleResource('SelectedItemCount'),
                tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                selectedList: 0,
                isCheckAll: true,
                isRetrieveDataAfterClose: false,
                multiple: true,
                shadow: true,
                showHeader: true,
                showFooter: false,
                showTooltip: true,
                onCheck: function () {
                    var checkedItems = $(_selector.ProductMultiSelect).multiSelect("getCheckedItems");
                    var numberOfCheckedItems = checkedItems.length;
                    /*update checked sports*/
                    _checkedSports = [];
                    for (var i = 0; i < numberOfCheckedItems; i++) {
                        addCheckedSport(checkedItems[i]);
                    }
                    /*Load bet types*/
                    loadBettypes();
                    $(_selector.ProductList).toggleClass("invalid", numberOfCheckedItems === 0);
                    $(_selector.SubmitButton).prop("disabled", numberOfCheckedItems === 0);
                }
            });
        }

        function groupProductList() {
            var groupData = [];
            var parents = Enumerable.From(_sports).Where(function (s) { return s.ParentId === -1; }).ToArray();
            $.each(parents, function (index, value) {
                var parent = {
                    Id: index.toString(),
                    Value: "-1-" + value.ID,
                    Text: value.Name,
                    Checked: value.Checked,
                    ParrentId: -1,
                    Enable: true,
                    Css: "",
                    CssClass: ""
                };
                var cindex = 0;
                parent.Items = Enumerable.From(_sports).Where(function (child) {
                    return child.ParentId === value.ID && child.ID > 0 && child.ParentId > -1;
                }).Select(function (child) {
                    return {
                        Id: parent.Id + "-" + cindex++,
                        Value: child.ParentId + "-" + child.ID,
                        Text: child.Name,
                        ParrentId: child.ParentId,
                        Checked: child.Checked,
                        Enable: true,
                        Css: "",
                        CssClass: "",
                        GroupName: parent.Text
                    };
                }).ToArray();

                groupData.push(parent);
            });

            return groupData;
        }

        function addCheckedSport(checkedValue) {
            var modelItem = Enumerable.From(_sports).FirstOrDefault(null, function (item) {
                return (item.ParentId + "-" + item.ID) === checkedValue;
            });

            if (modelItem != null) {
                modelItem.Checked = true;
                _checkedSports.push(modelItem);
                /*add parent item*/

                if (modelItem.ParentId > -1) {
                    var parent = Enumerable.From(_checkedSports).FirstOrDefault(null, function (item) {
                        return item.ID === modelItem.ParentId && item.ParentId === -1;
                    });

                    /* if not found parrent item in checked sports
                    * Continue find parent item in sports and add it to checked sports.
                    */
                    if (parent == null) {
                        parent = Enumerable.From(_sports).FirstOrDefault(null, function (item) {
                            return item.ID === modelItem.ParentId && item.ParentId === -1;
                        });
                        if (parent != null) {
                            _checkedSports.push(parent);
                        }
                    }
                }
            }
        }

        function loadBettypes() {
            if (_checkedSports.length === 0) {
                renderBetTypeTree([]);
                return;
            }
            var option = {
                type: "POST",
                data: JSON.stringify(_checkedSports),
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: _url.GetBetTypes,
                success: function (response) {
                    _bettypes = response;
                    _checkedBettypes = response;
                    renderBetTypeTree(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            };
            if (_ajaxRequestGetBettype != null) {
                _ajaxRequestGetBettype.abort();
            }
            _ajaxRequestGetBettype = $.ajax(option);
        }

        function renderBetTypeTree(data) {
            var dataList = [];
            $.each(data, function (index, value) {
                var item = {
                    Id: index.ID,
                    Value: value.ID,
                    Text: value.Name,
                    Checked: value.Checked,
                    Enable: true,
                    Css: "",
                    CssClass: ""
                };
                dataList.push(item);
            });
            var hasCheckedItems = dataList.length > 0;
            $(_selector.BetTypeList).toggleClass("invalid", !hasCheckedItems);
            $(_selector.SubmitButton).prop("disabled", !hasCheckedItems);

            $(_selector.BetTypeMultiSelect).multiSelect({
                //showCheckAll: true,
                minWidth: 200,
                maxHeight: 400,
                listData: dataList,
                hasOptGroup: false,
                onCheck: function () {
                    var checkedItems = $(_selector.BetTypeMultiSelect).multiSelect("getCheckedItems");
                    var numberOfCheckedItems = checkedItems.length;
                    /*update checked bet types*/
                    _checkedBettypes = [];
                    for (var i = 0; i < numberOfCheckedItems; i++) {
                        addCheckedBettype(checkedItems[i]);
                    }
                    $(_selector.BetTypeList).toggleClass("invalid", numberOfCheckedItems === 0);
                    $(_selector.SubmitButton).prop("disabled", numberOfCheckedItems === 0);
                },
                isUsedCustomCheckAllText: true,
                customCheckAllText: Site.getSingleResource('All'),
                checkAllText: Site.getSingleResource('All'),
                noItemText: Site.getSingleResource('NoBettypeFound'),
                unCheckAllText: Site.getSingleResource('UncheckAll'),
                updateResult: '',
                noneSelectedText: Site.getSingleResource('SelectOptions'),
                selectedText: Site.getSingleResource('SelectedItemCount'),
                tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                selectedList: 0,
                isCheckAll: true,
                isRetrieveDataAfterClose: false,
                multiple: true,
                shadow: true,
                showFooter: false,
                showHeader: true,
                showTooltip: true
            });
        }

        function addCheckedBettype(checkedValue) {
            var modelItem = Enumerable.From(_bettypes).FirstOrDefault(null, function (item) {
                return item.ID.toString() === checkedValue.toString();
            });
            if (modelItem !== null) {
                modelItem.Checked = true;
                _checkedBettypes.push(modelItem);
            }
        }

        function checkValidFilter($element) {
            var validBetType = _checkedBettypes.length > 0;
            var validTicketFilter = $(_selector.TickerFilter + " input:checked").length > 0;
            var isValid = validBetType && validTicketFilter;

            if ($element.attr("id") === "AllTicket") {
                isValid = $element.prop("checked") && validBetType;
            }

            $(_selector.SubmitButton).prop("disabled", !isValid);
        }

        function handleOddInput(event) {
            /*--------------------------------------------------------------
                Backspace, tab, enter, end, home, left, right
                Don't support the del key in Opera because del == . == 46.
                --------------------------------------------------------------*/
            var controlKeys = [8, 9, 13, 35, 36, 37, 39, 46];

            /*--------------------------------------------------------------
            IE doesn't support indexOf
            --------------------------------------------------------------*/
            var isControlKey = controlKeys.join(",").match(new RegExp(event.which));

            /*--------------------------------------------------------------
            Some browsers just don't raise events for control keys. Easy.
            e.g. Safari backspace.
            --------------------------------------------------------------*/
            if (!event.which ||
                // Control keys in most browsers. e.g. Firefox tab is 0
             48 <= event.which && event.which <= 57 ||
                // Always 0 through 9
                //(48 == event.which && $(this).attr("value")) ||   // No 0 first digit
                isControlKey) {
                // Opera assigns values for control keys.
                return;
            }
            else {
                event.preventDefault();
            }
        }

        function resizeGrid() {
            var reportGrid = $("#" + _selector.GridId);
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: initialize,
            initAutoComplete: initAutoComplete,
            initTicketFilter: initTicketFilter,
            initEvent: initEvent,
            initProductTree: initProductTree,
            submit: submit,
            bindData: bindData
        };

        return _me;
    });