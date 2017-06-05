/*global Site,JSLINQ, QueryString, PagingHeper, jqGridAdditional, CustomerListCustOption, CustomersOption */
/// <reference path="/@JSense.js" />
define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional", './customerlist.custoption', "lib/jquery.multiselect"],
    function ($, Site, jqGridAdditional, CustomerListCustOption) {
        "use strict";

        var ajaxRequest = null;
        var _$specialLevelCell, _$oddsGroupCell = null;
        var _isAllowedCustomerCommission = false;
        var _productAccess = null;
        var _cacheData = null;
        var _checkedAllRows = false;
        var _checkedCustomerArray = [];
        var _checkedCustomerObject = {}; // used to quickly check if a customer is checked or not
        var _POPUP_LIST = ["#view-cust-option", "#update-cust-status-popup", "#LevelListContainer", "#OddsGroupListContainer"];

        var _pagingOption =
            {
                pageindex: 1,
                pagesize: 1000,
                pagelist: [1000, 2000, 3000, 4000],
                totalrow: 1,
                elementcontainer: ".paging",
                method: "AJAX",
                ajaxfunction: "CustomerList.ajaxPaging()",
                alwayshow: true
            };

        function setSpecialLevel($listItem, level) {
            level = Number(level);
            var oldValue = Number($listItem.attr("data-level"));
            if (oldValue === level) {
                closeSpecialLevelList();
                return;
            }
            var custId = $listItem.attr("data-custid");

            var message = level === 0 ?
                            Site.getSingleResource('AlertRemoveSpecialLevelForCustomerMsg') :
                            Site.getSingleResource('AlertSetSpecialLevelForCustomerMsg');
            if (!confirm(message)) {
                return;
            }

            $listItem.html(level === 0 ? Site.getSingleResource("None") : level)
                     .attr('class', 'level-' + level);
            var param =
                {
                    custId: custId,
                    level: level
                };
            $.ajax({
                type: "POST",
                data: param,
                url: Site.resolveClientUrl("CustomerAPI/UpdateSpecialLevel"),
                dataType: "json",
                beforeSend: function () {
                    $listItem.attr('class', 'level-' + level);
                },
                success: function (data) {
                    if (data.Success) {
                        var rowIndex = ($listItem.closest("tr").attr("id") - 1) % _pagingOption.pagesize;
                        _cacheData.Data[rowIndex].SpecialLevel = level;
                        bindData(_cacheData, true);
                    }
                    else {
                        $listItem.html(oldValue === 0 ? Site.getSingleResource("None") : oldValue);
                        $listItem.attr('class', 'level-' + oldValue);
                        alert(data.ErrorMessage);
                    }
                    closeSpecialLevelList();
                },
                error: function (/*jqXHR, textStatus, errorThrown*/) {
                    $listItem.html(oldValue === 0 ? Site.getSingleResource("None") : oldValue);
                    $listItem.attr('class', 'level-' + oldValue);
                    closeSpecialLevelList();
                }
            });
        }

        function closeSpecialLevelList() {
            var popup = $("#LevelListContainer");
            if (popup.is(":visible")) {
                popup.hide();
            }
        }

        function viewSpecialLevelList($context) {
            var custId = $context.attr("data-custid");
            var popup = $("#LevelListContainer");
            var position = $context.offset();
            popup.css({
                top: position.top + $context.height(),
                left: $context.offset().left
            });

            if (popup.attr("custid") === custId) {
                popup.toggle();
            }
            else {
                var link = $("#cust-option-link-" + custId);
                popup.attr("custid", custId);
                popup.attr("externalid", link.attr("externalid"));
                popup.attr("systemid", link.attr("systemid"));
                popup.show();
            }

            return false;
        }

        function setOddsGroup($listItem, oddsGroup) {
            var oldValue = $listItem.attr("data-group");
            if (oldValue === oddsGroup) {
                closeOddsGroupList();
                return;
            }

            var message = Site.getSingleResource('AlertSetOddsGroupForCustomerMsg');
            message = message.replace("{0}", oddsGroup);
            if (!confirm(message)) {
                return;
            }

            var custId = $listItem.attr("data-custid");
            $listItem.html(oddsGroup === 0 ? Site.getSingleResource("None") : oddsGroup);
            var requestUrl = Site.resolveClientUrl("CustomerAPI/UpdateOddsGroup");
            $listItem.attr('class', 'group-' + oddsGroup);
            var param =
                {
                    custId: custId,
                    oddsGroup: oddsGroup
                };
            $.ajax({
                type: "POST",
                data: param,
                url: requestUrl,
                dataType: "json",
                beforeSend: function () {
                    $listItem.attr('class', 'group-' + oddsGroup);
                },
                success: function (data) {
                    if (data.Success) {
                        var rowIndex = ($listItem.closest("tr").attr("id") - 1) % _pagingOption.pagesize;
                        _cacheData.Data[rowIndex].OddsGroup = oddsGroup;
                        bindData(_cacheData, true);
                    }
                    else {
                        $listItem.html(oldValue);
                        $listItem.attr('class', 'group-' + oldValue);
                        alert(data.ErrorMessage);
                    }
                    closeOddsGroupList();
                },
                error: function (/*jqXHR, textStatus, errorThrown*/) {
                    $listItem.html(oldValue);
                    $listItem.attr('class', 'group-' + oldValue);
                    closeOddsGroupList();
                }
            });
        }

        function closeOddsGroupList() {
            var popup = $("#OddsGroupListContainer");
            if (popup.is(":visible")) {
                popup.hide();
            }
        }

        function viewOddsGroupList($context) {
            if ($context.next(".arrow1").length === 0) {
                return;
            }

            var custId = $context.attr("data-custid");
            var popup = $("#OddsGroupListContainer");
            var position = $context.offset();
            popup.css({
                top: position.top + $context.height(),
                left: $context.offset().left
            });

            if (popup.attr("custid") === custId) {
                popup.toggle();
            }
            else {
                var link = $("#cust-option-link-" + custId);
                popup.attr("custid", custId);
                popup.attr("externalid", link.attr("externalid"));
                popup.attr("systemid", link.attr("systemid"));
                popup.show();
            }

            return false;
        }

        function processGridOption(hasData, showingColumns) {
            var gridOption = CustomersOption.getGridOption(_productAccess, _isAllowedCustomerCommission);

            // Change formatter for Index col
            if (hasData && _isAllowedCustomerCommission && showingColumns["Group"] !== undefined) {
                gridOption.colModel[0].formatter = function (cellValue, options, rowObject) {
                    var checkedText = _checkedAllRows || _checkedCustomerObject[rowObject.CustId] === true ? "checked" : "";
                    return "<input type='checkbox' " + checkedText + " title='" + rowObject.CustId +
                            "' id='" + rowObject.CustId + "' data-group-comm-id='" + rowObject.GroupCommId + "' class='select-cust-checkbox check-child' />";
                };
            } else {
                gridOption.colModel[0].formatter = undefined;
            }

            var collength = gridOption.colModel.length;
            var colIndex = 0;
            while (colIndex < collength) {
                var col = gridOption.colModel[colIndex];
                if (!col.name || col.name === "Index") {
                    colIndex++;
                    continue;
                }

                var colName = col.name.match(/^\w\wComm$/) !== null ? "Commission" : col.name;

                if (showingColumns[colName] === undefined) {
                    gridOption.colModel.splice(colIndex, 1);
                    gridOption.colNames.splice(colIndex, 1);
                    collength--;
                } else {
                    colIndex++;
                }
            }

            return gridOption;
        }

        function bindData(response, keepScrollPosition) {
            var oldScrollTop = keepScrollPosition === true ? $(".ui-jqgrid-bdiv").scrollTop() : 0;
            jqGridAdditional.clearGrid("ReportGrid");

            var $gridBody = $(".ui-jqgrid-bdiv");
            var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;
            var reportGrid = $("#ReportGrid");

            $("#num-of-customer").html(response.TotalRows + " ");

            _pagingOption.totalrow = parseInt(response.TotalRows);
            if (_pagingOption.totalrow > 0) {
                $(".paging").show();
                PagingHeper.render(_pagingOption);
            }

            var hasData = response !== null && response.TotalRows > 0;

            var showingColumns = $("#ui-multiselect-options-grid-columns-multiselect input").serializeObject();
            var gridOption = processGridOption(hasData, showingColumns);
            gridOption.width = gridOption.colModel.length < 7 ? 650 : undefined;
            gridOption.data = response.Data;

            //show report title
            $(".report-title").removeClass("hide");
            // Show the grid
            reportGrid.jqGrid(gridOption);
            CustomersOption.setGroupHeaderColumns(reportGrid);

            if (_isAllowedCustomerCommission && showingColumns["Group"] !== undefined) {
                $("#jqgh_ReportGrid_Index").attr("class", "").html("<input type='checkbox' class='' id='CheckAll' />");
            } else {
                $("#jqgh_ReportGrid_Index").html("#");
            }

            $gridBody = $(".ui-jqgrid-bdiv");
            if ($gridBody.length > 0) {
                $gridBody[0].scrollLeft = scrollLeft;
            }

            $("#CheckAll").prop("checked", _checkedAllRows);
            // $(".select-cust-checkbox").checkboxgroup();

            var gridWidth = reportGrid.width();

            $(".paging").width(gridWidth);
            $(".report-title").width(gridWidth - 25);
            $(".content-filter").width(Math.max(1040, gridWidth));
            var gridTop = reportGrid.offset().top;
            var gridHeight = $(window).height() - gridTop - 50;
            gridHeight = Math.max(200, gridHeight);
            $(".theme_jqGrid .ui-jqgrid-bdiv").css("max-height", gridHeight);

            if (hasData) {
                $("div.paging").show();
            } else {
                jqGridAdditional.setNoInfo(gridOption.colModel);
                $("div.paging").hide();
            }

            jqGridAdditional.resizeGrid();
            if (keepScrollPosition === true) {
                $(".ui-jqgrid-bdiv").scrollTop(oldScrollTop);
            }
            Site.unBlockUI();

            _me.addGridScrollEventToHidePopups();
        }

        function convertCustomerArrayToObject(array) {
            var obj = {};
            for (var i = 0; i < array.length; ++i) {
                var id = array[i].Id;
                obj[id] = true;
            }
            return obj;
        }

        function addGridCheckBoxesEvent() {
            $("#report-content").on("change", "#CheckAll", function () {
                _checkedAllRows = this.checked;

                if (_checkedAllRows) {
                    _checkedCustomerArray = JSLINQ(_cacheData.Data).Select(function (x) { return { Id: x.CustId, Group: x.GroupCommId }; }).ToArray();
                    _checkedCustomerObject = convertCustomerArrayToObject(_checkedCustomerArray);
                } else {
                    _checkedCustomerArray = [];
                    _checkedCustomerObject = {};
                }

                bindData(_cacheData);
            });

            $("#report-content").on("change", ".select-cust-checkbox", function () {
                var custid = Number(this.id);
                var group = $(this).attr("data-group-comm-id");
                if (this.checked) {
                    _checkedCustomerObject[custid] = true;
                    _checkedCustomerArray.push({ Id: custid, Group: group });
                    if (_checkedCustomerArray.length === _cacheData.Data.length) {
                        $("#CheckAll").prop("checked", true);
                        _checkedAllRows = true;
                    }
                } else {
                    delete _checkedCustomerObject[custid];
                    for (var i = 0; i < _checkedCustomerArray.length; i++) {
                        if (_checkedCustomerArray[i].Id === custid) {
                            _checkedCustomerArray.splice(i, 1);
                            break;
                        }
                    }

                    _checkedAllRows = false;
                    $("#CheckAll").prop("checked", false);
                }
            });
        }

        function updateGroupForCheckCustomerArray(groupid) {
            $.each(_checkedCustomerArray, function (index, item) {
                item.Group = groupid;
            });
        }

        var _me = {
            updateGridGroup: function (groupid, groupName, groupColor) {
                var count = 0;
                var length = _checkedCustomerArray.length;
                $.each(_cacheData.Data, function (index, item) {
                    if (count === length) {
                        return false;
                    }

                    if (_checkedCustomerObject[item.CustId] === true) {
                        item.GroupCommId = groupid;
                        item.Group = groupName;
                        item.GroupColor = groupColor;
                        count++;
                    }
                });

                updateGroupForCheckCustomerArray(groupid);
                bindData(_cacheData);
            },
            updateGridGroupAndComm: function (groupid, groupName, groupColor, sbCom, ngCom, rbCom, lcCom, csCom) {
                var count = 0;
                var length = _checkedCustomerArray.length;

                $.each(_cacheData.Data, function (index, item) {
                    if (count === length) {
                        return false;
                    }
                    if (_checkedCustomerObject[item.CustId] === true) {
                        item.GroupCommId = groupid;
                        item.Group = groupName;
                        item.GroupColor = groupColor;
                        item.SBComm = sbCom;
                        item.NGComm = ngCom;
                        item.RBComm = rbCom;
                        item.LCComm = lcCom;
                        item.CSComm = csCom;
                        count++;
                    }
                });

                updateGroupForCheckCustomerArray(groupid);
                bindData(_cacheData);
            },

            getCheckedCustomers: function () {
                return _checkedCustomerArray;
            },
            ajaxPaging: function () {
                _pagingOption = PagingHeper.getSetting();
                _me.submit();
            },
            initialize: function () {
                Site.initialize();
                
                $("#SubmitButtonComm").prop("disabled", true);
                _me.initEvent();
                _me.initPopup();
                _me.initAutoComplete();
                _me.initGridColumnOption();
                _me.initOddGroupOption();
                $("#SubmitButtonComm").prop("disabled", false);
                _isAllowedCustomerCommission = $("#IsAllowedCustomerCommission").val() === "True";
                _productAccess = JSON.parse($("#ProductAccess").val());
                addGridCheckBoxesEvent();
                Site.addUserGuide({
                    resourceName: "CustomerList",
                    title: Site.getSingleResource("CustomerList")
                });
            },
            initEvent: function () {
                _me.loadPostbackParam();

                $("#report-content").on("click", ".specialLevel *", function () {
                    _$specialLevelCell = $(this).parent().find("div");
                    viewSpecialLevelList(_$specialLevelCell);
                });

                $("#LevelListContainer .levelLink").click(function () {
                    var level = $(this).attr("data-level");
                    setSpecialLevel(_$specialLevelCell, level);
                });

                $("#report-content").on("click", ".oddsGroup *", function () {
                    _$oddsGroupCell = $(this).parent().find("div");
                    viewOddsGroupList(_$oddsGroupCell);
                });

                $("#OddsGroupListContainer .oddsGroupLink").click(function () {
                    var group = $(this).attr("data-group");
                    setOddsGroup(_$oddsGroupCell, group);
                });

                $('#SubmitButtonComm').click(function () {
                    _pagingOption.pageindex = 1;
                    _me.submit();
                });

                $("#txtUsername").click(_me.setUsernameOnFocus)
                                 .blur(_me.setDefaultUsername);

                $("#report-content").on("click", ".ba-off, .ba-on", function (event) {
                    event.preventDefault();
                    return _me.updateBASetting(this);
                });

                $("#report-content").on("click", ".cust-status-option", function () {
                    _me.viewUpdatePopUp($(this));
                    return false;
                });

                $("#report-content").on("click", ".custoptionlink", function () {
                    CustomerListCustOption.viewCustOption($(this));
                    return false;
                });

                $("#popup-update").on("click", _me.updateCustStatus);
                $("#popup-close").on("click", _me.closeUpdatePopUp);

                $("#popup-suspended").change(_me.custStatusCheckedChanged);
                $("#popup-closed").change(_me.custStatusCheckedChanged);

                $("#bet-setting-link").on("click", _me.viewBetSetting);
                $("#bet-list-link").on("click", _me.viewBetList);
                $("#statement-link").on("click", _me.viewStatement);
                $("#login-info-link").on("click", _me.viewLogInfo);

                $("#close-cust-option-link").on("click", CustomerListCustOption.closeCustOption);
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
                $("#grid-columns-multiselect").multiSelect(
                {
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
                            $("#SubmitButtonComm").prop("disabled", true);
                            $("#grid-columns-multiselect-null").removeClass("hide");
                            $("#grid-columns-option-container input[type=checkbox]:checked").prop("checked", false);
                        }
                        else {
                            $("#grid-columns-option-container input[type=checkbox]:checked").prop("checked", false);
                            for (var i = 0; i < checkedItems.length; i++) {
                                $('#grid-columns-option-container input#' + checkedItems[i]).prop("checked", true);
                            }
                            $("#SubmitButtonComm").prop("disabled", false);
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
                $("#odds-group-multiselect").multiSelect(
                {
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
                            $("#SubmitButtonComm").prop("disabled", true);
                            $("#odds-group-multiselect-null").removeClass("hide");
                        }
                        else {
                            $("#SubmitButtonComm").prop("disabled", false);
                            $("#odds-group-multiselect-null").addClass("hide");
                        }
                    }
                });
            },
            initAutoComplete: function () {
                var $username = $('#txtUsername');
                var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer");
                if ($username.length > 0) {
                    $username.autocomplete({ serviceUrl: serUrl, minChars: 2 });
                    _me.setDefaultUsername();
                }
            },
            initPopup: function () {
                $(document).mouseup(function (e) {
                    $(_POPUP_LIST).each(function (index, element) {
                        var container = $(element);
                        if (container.is(":visible")) {
                            var inWrapper = container.get(0) === e.target || container.has(e.target).length > 0;
                            if (!inWrapper) {
                                container.hide();
                            }
                        }
                    });
                });
            },
            addGridScrollEventToHidePopups: function () {
                // Hide popup if scroll
                $(".ui-jqgrid-bdiv").on("scroll", function () {
                    $(_POPUP_LIST).each(function (index, element) {
                        var popup = $(element);
                        if (popup.is(":visible")) {
                            popup.hide();
                        }
                    });
                });
            },
            submit: function () {
                Site.blockUI();
                _checkedCustomerObject = {};
                _checkedCustomerArray = [];
                _checkedAllRows = false;
                var data = {};

                data.GridColumns = [];
                var $gridColumnMultiselect = $("#ui-multiselect-options-grid-columns-multiselect ul.ui-multiselect-checkboxes input");
                $gridColumnMultiselect.each(function (index, element) {
                    data.GridColumns.push({
                        Name: element.name,
                        Checked: element.checked
                    });
                });
                if (!_me.hasSelectedMultiselectData(data.GridColumns)) {
                    return;
                }

                if ($("#OddsGroups").length > 0) {
                    data.OddsGroups = [];
                    var $oddsGroupsMultiselect = $("#ui-multiselect-options-odds-group-multiselect .ui-multiselect-checkboxes input");
                    $oddsGroupsMultiselect.each(function (index, element) {
                        data.OddsGroups.push({
                            Name: element.name,
                            Checked: element.checked
                        });
                    });

                    if (!_me.hasSelectedMultiselectData(data.OddsGroups)) {
                        return;
                    }
                }

                data.PageIndex = _pagingOption.pageindex;
                data.PageSize = _pagingOption.pagesize;
                data.CurrencyId = $("#CurrencyId").val();
                data.SpecialLevel = $("#special-level-list-search").val();
                data.Username = Site.asNull($("#txtUsername").val());
                if (data.Username === Site.getSingleResource('ExternalIdOrSystemId')) {
                    data.Username = null;
                }
                data.CurrencyName = $("#CurrencyName").val();
                data.BaFilter = $("#BAFilterOption").val();
                data.GroupCommId = $("#DdlGroupCommId").val();
                data.BaseCurrency = $("#BaseCurrency").val();
                data.CustStatus = $("#CustStatusOption").val();

                $("#BaseCurrency").attr("data-original", data.BaseCurrency);

                $("#excelParam").val(JSON.stringify(data));

                $("#CurrencyId").attr('data-original', data.CurrencyId);
                $("#BaseCurrency").attr('data-original', data.BaseCurrency);

                var option = {
                    type: "POST",
                    data: data,
                    dataType: "json",
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    cache: false,
                    url: Site.resolveClientUrl(CustomersOption.urlData),
                    success: function (response) {
                        _cacheData = response;
                        bindData(response);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    error: function () {
                        Site.unBlockUI();
                        alert(Site.getSingleResource("ErrorProcessingMsg"));
                    }
                };
                if (ajaxRequest != null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);

                return true;
            },
            hasSelectedMultiselectData: function (dataList) {
                var totalItem = dataList.length;
                var checkedItemCount = 0;
                for (var i = 0; i < totalItem; i++) {
                    if (dataList[i].Checked) {
                        checkedItemCount++;
                    }
                }

                return checkedItemCount > 0;
            },
            loadPostbackParam: function () {
                var username = $('#Username').val();
                if (username === '' || username === undefined) {
                    $('#txtUsername').val(Site.getSingleResource('ExternalIdOrSystemId'));
                }
                else {
                    $('#txtUsername').val(username);
                    $('#txtUsername').attr('class', '');
                }
                Site.currencyConstraint();
            },

            setDefaultUsername: function () {
                var $username = $('#txtUsername');
                var username = $username.val();
                if (username === '' || username === Site.getSingleResource('ExternalIdOrSystemId')) {
                    $username.val(Site.getSingleResource('ExternalIdOrSystemId'))
                             .attr('class', 'default-username');
                }
                else {
                    $username.attr('class', '');
                }
            },

            setUsernameOnFocus: function () {
                var $username = $('#txtUsername');
                var username = $username.val();
                if (username === Site.getSingleResource('ExternalIdOrSystemId')) {
                    $username.val('').removeAttr('class');
                }
            },

            onSpecialLevelFilterChanged: function () {
                var list = $('#special-level-list-search');
                list.attr('class', 'level-' + list.val());
            },

            // -------------Update cust status----------------------------------------
            // view popup update
            viewUpdatePopUp: function ($context) {
                var custId = $context.attr("custid");
                var statusLink = $("#edit-cust-status-" + custId);
                var popup = $("#update-cust-status-popup");

                var position = $context.offset();
                popup.css({
                    top: position.top + $context.height(),
                    left: $context.offset().left
                });

                if (popup.attr("custid") === custId) {
                    popup.toggle();
                }
                else {
                    var isClosed = statusLink.attr("isclosed").toLowerCase() === "true";
                    var isSuspended = statusLink.attr("issuspended").toLowerCase() === "true";
                    var externalId = statusLink.attr("externalid");

                    $("#popup-external-id").html(externalId);
                    $("#popup-suspended").prop('checked', isSuspended);
                    $("#popup-closed").prop('checked', isClosed);

                    popup.attr("custid", custId);
                    popup.attr("closed", isClosed);
                    popup.attr("suspended", isSuspended);

                    popup.show();
                }

                return false;
            },

            closeUpdatePopUp: function () {
                var popup = $("#update-cust-status-popup");
                if (popup.is(":visible")) {
                    $("#update-cust-status-popup").hide();
                }
            },

            // option popup
            custStatusCheckedChanged: function () {
                var popup = $("#update-cust-status-popup");
                var isSuspended = $("#popup-suspended").is(':checked');
                var isClosed = $("#popup-closed").is(':checked');

                var changed = Site.asBool(popup.attr("closed")) !== isClosed || Site.asBool(popup.attr("suspended")) !== isSuspended;
                $("#popup-update").prop("disabled", !changed);
            },

            // ---------------update cust status------------------------------------------------
            updateCustStatus: function () {
                if (!window.confirm(Site.getSingleResource('ChangeStatusOfThisCustomerMsg'))) {
                    _me.closeUpdatePopUp();
                    return;
                }

                var popup = $("#update-cust-status-popup");
                var custId = popup.attr("custid");
                var statusLink = $("#edit-cust-status-" + custId);

                var isSuspended = $("#popup-suspended").is(':checked');
                var isClosed = $("#popup-closed").is(':checked');
                var requestUrl = Site.resolveClientUrl("CustomerAPI/UpdateCustStatus");
                $.ajax({
                    type: "POST",
                    data:
                    {
                        custId: custId,
                        isSuspended: isSuspended,
                        isClosed: isClosed
                    },
                    url: requestUrl,
                    dataType: "json",
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                        _me.closeUpdatePopUp(custId);
                    },
                    success: function (result) {
                        if (Number(result.Code) === 0) {
                            var color = isClosed === true ? "#ccc7ba" : isSuspended === true ? "#f8ded7" : "#F6F8F9";

                            var statusText = isClosed === true ? Site.getSingleResource("Closed") :
                                        isSuspended === true ? Site.getSingleResource("Suspended") : Site.getSingleResource("Open");

                            statusLink.html(statusText + '<span title="' + Site.getSingleResource('Status') + '" class="arrow1" >&nbsp;&nbsp;&nbsp;</span>');
                            statusLink.attr("isclosed", isClosed);
                            statusLink.attr("issuspended", isSuspended);

                            $("#row" + custId).css("background-color", color);
                        }
                        else {
                            alert(result.Message);
                        }
                    }
                });
            },
            // -----------------------------------------------------------------------

            // --------------update BA Status ----------------------------------------
            updateBASetting: function (context) {
                var canUpdateBA = $("#CanUpdateBA").val();
                var canUpdateBAMessage = $("#CanUpdateBAMessage").val();
                if (!canUpdateBA) {
                    alert(canUpdateBAMessage);
                    return;
                }
                var $context = $(context);
                var paramString = $context.attr("data-val");
                var params = paramString.split("#");
                var baparam = (params[2] === "True" || params[2] === "true") ? true : false;
                var confirmMsg = params[2].toLowerCase() === "true" ? Site.getSingleResource("AlertTurnOnBAMsg") : Site.getSingleResource("AlertTurnOffBAMsg");
                if (params.length === 4) {
                    var data = {
                        custId: params[1],
                        isAllowBA: baparam,
                        externalId: params[3]
                    };
                    var requestUrl = Site.resolveClientUrl("CustomerAPI/UpdateBAStatus");
                    if (window.confirm(confirmMsg)) {
                        var currentBAStatus = $context.html();
                        var ajaxloading = $("#ajaxLoaDingUpdateBA").html();
                        $.ajax({
                            type: "POST",
                            url: requestUrl,
                            data: data,
                            beforeSend: function () {
                                $context.html(ajaxloading);
                            },
                            complete: function () { },
                            success: function (data) {
                                if (data.IsSuccessful) {
                                    var rowIndex = ($context.closest("tr").attr("id") - 1) % _pagingOption.pagesize;
                                    _cacheData.Data[rowIndex].BAStatus = baparam;
                                    _cacheData.Data[rowIndex].OddsGroup = data.OddsGroup;
                                    bindData(_cacheData, true);
                                }
                                else {
                                    $context.html(currentBAStatus);
                                    alert(data.Message);
                                }
                            },
                            error: function () {
                                $context.html(currentBAStatus);
                            },
                            statusCode:
                            {
                                404: function () {
                                    $context.html(currentBAStatus);
                                    alert('Error-Page not found');
                                }
                            }
                        });
                        return;
                    }
                    return;
                }
            }
        };

        return _me;
    });
