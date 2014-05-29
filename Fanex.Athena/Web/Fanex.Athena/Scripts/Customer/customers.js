/*global Site,JSLINQ, QueryString, pagingHeper, jqGridAdditional, CustomersOption */
/// <reference path="/@JSense.js" />
window.KeepBlockingAfterInit = true;
var CustomerList = (function ($) {
    "use strict";
    var ajaxRequest = null;
    var $specialLevelCell = null;
    var _isAllowedCustomerCommission = false;
    var _productAccess = null;
    var _cacheData = null;
    var _checkedAllRows = false;
    var _checkedCustomerArray = [];
    var _checkedCustomerObject = {}; // used to quickly check if a customer is checked or not

    var _pagingOption =
        {
            pageindex: 1,
            pagesize: 1000,
            pagelist: [1000, 2000, 3000, 4000, 5000],
            totalrow: 1,
            elementcontainer: ".paging",
            method: "AJAX",
            ajaxfunction: "CustomerList.ajaxPaging()",
            alwayshow: true
        };

    function setSpecialLevel(list, level) {
        level = Number(level);
        var oldValue = Number(list.attr("data-level"));
        if (oldValue === level) {
            closeSpecialLevelList();
            return;
        }
        var custId = list.attr("data-custid");

        var msg = level === 0 ? Site.getSingleResource('AlertRemoveSpecialLevelForCustomerMsg') : Site.getSingleResource('AlertSetSpecialLevelForCustomerMsg');
        var answer = confirm(msg);
        if (!answer) {
            list.html(oldValue);
            list.attr('class', 'level-' + oldValue);
            return;
        }

        list.html(level === 0 ? Site.getSingleResource("None") : level);
        var requestUrl = Site.resolveClientUrl("CustomerAPI/UpdateSpecialLevel");
        list.attr('class', 'level-' + level);
        var param =
            {
                custId: custId,
                level: level
            };
        $.ajax({
            type: "POST",
            data: JSON.stringify(param),
            url: requestUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                list.attr('class', 'level-' + level);
            },
            success: function (data) {

                if (data.Success) {
                    list.attr("data-level", level);
                    list.unbind("change");
                    list.change(function () {
                        CustomerList.setSpecialLevel($(this), custId, level);
                    });
                }
                else {
                    list.html(oldValue);
                    list.attr('class', 'level-' + oldValue);
                    alert(data.ErrorMessage);
                }
                closeSpecialLevelList();
            },
            error: function (/*jqXHR, textStatus, errorThrown*/) {
                list.html(oldValue);
                list.attr('class', 'level-' + oldValue);
                alert("Error");
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

    function viewSpecialLevelList(context) {
        var custId = context.attr("data-custid");
        var popup = $("#LevelListContainer");
        var link = $("#cust-option-link-" + custId);

        if (popup.attr("custid") === custId) {
            popup.toggle();
        }
        else {
            popup.attr("custid", custId);
            popup.attr("externalid", link.attr("externalid"));
            popup.attr("systemid", link.attr("systemid"));
            var position = $(context).offset();
            popup.css({
                top: position.top + $(context).height(),
                left: $(context).offset().left
            });

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

    function bindData(response) {
        jqGridAdditional.clearGrid("ReportGrid");

        var $gridBody = $(".ui-jqgrid-bdiv");
        var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;
        var reportGrid = $("#ReportGrid");

        $("#num-of-customer").html(response.TotalRows + " ");

        _pagingOption.totalrow = parseInt(response.TotalRows);
        if (_pagingOption.totalrow > 0) {
            $(".paging").show();
            pagingHeper.render(_pagingOption);
        }

        var hasData = response !== null && response.TotalRows > 0;

        var showingColumns = $("#ui-multiselect-options-grid-columns-multiselect input").serializeObject();
        var gridOption = processGridOption(hasData, showingColumns);

        gridOption.scroll = 1;
        gridOption.data = response.Data;
        gridOption.rowNum = 40; // _pagingOption.pagesize;

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

        if (gridWidth < 500) {
            gridWidth = 500;
        } else {
            gridWidth += 20;
        }
        gridWidth = Math.min(1030, gridWidth);
        reportGrid.jqGrid("setGridWidth", gridWidth);
        $(".paging").width(gridWidth);
        $(".report-title").width(gridWidth - 25);
        $(".content-filter").width(Math.max(1030, gridWidth));

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

        Site.unBlockUI();
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

    return {
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
                    item.SbComm = sbCom;
                    item.NgComm = ngCom;
                    item.RbComm = rbCom;
                    item.LcComm = lcCom;
                    item.csComm = csCom;
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
            _pagingOption = pagingHeper.getSetting();
            CustomerList.submit();
        },
        mousePosition: { x: 0, y: 0 },
        initialize: function () {
            $("#SubmitButtonComm").prop("disabled", true);
            CustomerList.initEvent();
            CustomerList.initPopup();
            CustomerList.initAutoComplete();
            CustomerList.initGridColumnOption();
            $("#SubmitButtonComm").prop("disabled", false);
            _isAllowedCustomerCommission = $("#IsAllowedCustomerCommission").val() === "True";
            _productAccess = JSON.parse($("#ProductAccess").val());
            addGridCheckBoxesEvent();
        },
        initEvent: function () {
            CustomerList.loadPostbackParam();

            $("#report-content").on("click", ".specialLevel *", function () {
                var $this = $(this).parent().find("div");
                $specialLevelCell = $this;
                viewSpecialLevelList($this);
            });

            $("#LevelListContainer .levelLink").click(function () {
                var $this = $(this);
                var level = $this.attr("data-level");
                setSpecialLevel($specialLevelCell, level);
            });

            $('#SubmitButtonComm').click(function () {
                _pagingOption.pageindex = 1;
                CustomerList.submit();
            });

            $("#txtUsername").click(CustomerList.setUsernameOnFocus)
                             .blur(CustomerList.setDefaultUsername);

            $("#report-content").on("click", ".ba-off, .ba-on", function (event) {
                event.preventDefault();
                return CustomerList.updateBASetting(this);
            });

            $("#report-content").on("click", ".cust-status-option", function () {
                CustomerList.viewUpdatePopUp($(this));
                return false;
            });

            $("#report-content").on("click", ".custoptionlink", function () {
                CustomerList.viewCustOption($(this));
                return false;
            });

            $("#popup-update").on("click", CustomerList.updateCustStatus);
            $("#popup-close").on("click", CustomerList.closeUpdatePopUp);

            $("#popup-suspended").change(CustomerList.custStatusCheckedChanged);
            $("#popup-closed").change(CustomerList.custStatusCheckedChanged);

            $("#bet-setting-link").on("click", CustomerList.viewBetSetting);
            $("#bet-list-link").on("click", CustomerList.viewBetList);
            $("#statement-link").on("click", CustomerList.viewStatement);
            $("#login-info-link").on("click", CustomerList.viewLogInfo);

            $("#close-cust-option-link").on("click", CustomerList.closeCustOption);

            $(document).on('mousedown', function (e) {
                CustomerList.mousePosition = { x: e.pageX, y: e.pageY };
            });
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
        initAutoComplete: function () {
            var txtUserName = $('#txtUsername');
            var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({ serviceUrl: serUrl, minChars: 2 });
                CustomerList.setDefaultUsername();
            }
        },
        initPopup: function () {
            var popupList = ["#view-cust-option", "#update-cust-status-popup", "#LevelListContainer"];
            $(document).mouseup(function (e) {
                $(popupList).each(function (index, element) {
                    var container = $(element);
                    if (container.is(":visible")) {
                        var inWrapper = container.get(0) === e.target || container.has(e.target).length > 0;
                        if (!inWrapper) {
                            container.hide();
                        }
                    }
                });
            });

            // Hide popup if scroll
            $("#data-content").scroll(function () {
                $(popupList).each(function (index, element) {
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

            data.LicenseeType = Site.asNull($("#LicenseeType").val());
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

            data.GridColumns = [];
            $("#ui-multiselect-options-grid-columns-multiselect ul.ui-multiselect-checkboxes input").each(function (index, element) {
                data.GridColumns.push({
                    Name: element.name,
                    Checked: element.checked
                });
            });
            $("#excelParam").val(JSON.stringify(data));

            $("#LicenseeType").attr('data-original', data.LicenseeType);
            $("#CurrencyId").attr('data-original', data.CurrencyId);
            $("#BaseCurrency").attr('data-original', data.BaseCurrency);

            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(CustomersOption.urlData),
                error: function (/*xhRequest, errorText, thrownError*/) { Site.gotoErrorPage(); },
                success: function (response) {
                    _cacheData = response;
                    bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
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
            var username = $('#txtUsername').val();
            if (username === '' || username === Site.getSingleResource('ExternalIdOrSystemId')) {
                $('#txtUsername').val(Site.getSingleResource('ExternalIdOrSystemId'));
                $('#txtUsername').attr('class', 'default-username');
            }
            else {
                $('#txtUsername').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#txtUsername').val();
            if (username === Site.getSingleResource('ExternalIdOrSystemId')) {
                $('#txtUsername').val('');
                $('#txtUsername').removeAttr('class');
            }
        },

        onSpecialLevelFilterChanged: function () {
            var list = $('#special-level-list-search');
            list.attr('class', 'level-' + list.val());
        },

        viewCustOption: function (context) {
            var custId = context.attr("custid");
            var popup = $("#view-cust-option");
            var link = $("#cust-option-link-" + custId);

            if (popup.attr("custid") === custId) {
                popup.toggle();
            }
            else {
                popup.attr("custid", custId);
                popup.attr("externalid", link.attr("externalid"));
                popup.attr("systemid", link.attr("systemid"));

                popup.css({
                    top: CustomerList.mousePosition.y + 10,
                    left: CustomerList.mousePosition.x - 40
                });

                popup.show();
            }

            return false;
        },

        closeCustOption: function () {
            var popup = $("#view-cust-option");
            if (popup.is(":visible")) {
                popup.hide();
            }
        },

        // -------------Update cust status----------------------------------------
        // view popup update
        viewUpdatePopUp: function (context) {
            var custId = context.attr("custid");
            var statusLink = $("#edit-cust-status-" + custId);
            var popup = $("#update-cust-status-popup");

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

                popup.css({
                    top: CustomerList.mousePosition.y + 10,
                    left: CustomerList.mousePosition.x - 90
                });

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
                CustomerList.closeUpdatePopUp();
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
                    CustomerList.closeUpdatePopUp(custId);
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
                },
                error: function (jqXHR, textStatus) {
                    alert(textStatus);
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
            var paramString = $(context).attr("data-val");
            var params = paramString.split("#");
            var baparam = (params[2] === "True" || params[2] === "true") ? true : false;
            var confirmMsg = params[2].toLowerCase() === "true" ? Site.getSingleResource("AlertTurnOnBAMsg") : Site.getSingleResource("AlertTurnOffBAMsg");
            if (params.length === 4) {
                var requestUrl = Site.resolveClientUrl("CustomerAPI/UpdateBAStatus?custId=" + params[1] + "&isAllowBA=" + baparam + "&externalId=" + params[3]);
                var isUpdate = window.confirm(confirmMsg);
                if (isUpdate) {
                    var ctx = context;
                    var currentBAStatus = $(ctx).html();
                    var ajaxloading = $("#ajaxLoaDingUpdateBA").html();
                    $.ajax({
                        type: "POST",
                        url: requestUrl,

                        beforeSend: function () {
                            $(ctx).html(ajaxloading);
                        },

                        complete: function () { },
                        success: function (data) {
                            //ConfirmMessage
                            if (data.IsSuccessful) {
                                $(ctx).html("")
                                      .removeClass("ba-on ba-off")
                                      .addClass('ba-' + data.BAStatus.toLowerCase());
                                if (data.IsBA) {
                                    $(ctx).css("color", "#000000");
                                }
                                else {
                                    $(ctx).css("color", "#cc0033");
                                }
                                var hrefString = "#" + params[1] + "#" + data.IsBA + "#" + params[3];
                                $(ctx).attr("data-val", hrefString);
                                $(ctx).attr("title", data.Tooltip);
                            }
                            else {
                                $(ctx).html(currentBAStatus);
                                alert(data.Message);
                            }
                        },
                        error: function (jqXHR, textStatus) {
                            $(ctx).html(currentBAStatus);
                            alert(textStatus);
                        },
                        statusCode:
                        {
                            404: function () {
                                $(ctx).html(currentBAStatus);
                                alert('Error-Page not found');
                            }
                        }
                    });
                    return;
                }
                return;
            }
        },
        //------------------------------------------------------------------------

        // ------------Bet setting------------------------------------------------
        viewBetSetting: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption();

            var url = Site.resolveClientUrl("BetSetting/Index?custid=" + custId + "&username=" + systemId + "&nickname=" + externalId);

            var popH = 440, popW = 905;

            var dialogFrame = $("#bet-setting-frame");
            dialogFrame.attr("height", popH);
            dialogFrame.attr("width", popW);
            dialogFrame.attr("src", url);

            var dialogForm = $('#bet-setting-dialog');
            dialogForm.dialog({
                modal: true,
                width: popW + 15,
                height: popH + 70,
                close: function (/*event, ui*/) {
                    dialogFrame.attr("src", "");
                }
            });
        },
        //------------------------------------------------------------------------

        // ---------------View Log Info ------------------------------------------
        viewLogInfo: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption();

            var url = "Customer/LogInfo?custid=" + custId;
            url += "&externalid=" + externalId;
            url += "&systemid=" + systemId;
            url = Site.resolveClientUrl(url);

            var dialog = $('<div title="' + Site.getSingleResource('LogInfo') + '" style="display:none" class="loading"></div>').appendTo('body');

            dialog.dialog({
                // add a close listener to prevent adding multiple divs to the document
                close: function (/*event, ui*/) {
                    // remove div with all data and events
                    dialog.remove();
                },
                width: 360,
                height: 280,
                modal: true
            });
            // load remote content
            dialog.load(url,
                {}, // omit this param object to issue a GET request instead a POST request, otherwise you may provide post parameters within the object
                function (/*responseText, textStatus, XMLHttpRequest*/) {
                    dialog.removeClass('loading');
                }
            );
            //prevent the browser to follow the link
            return false;
        },

        // ------------------------------------------------------------------------
        // ---------------View Bet List -------------------------------------------
        viewBetList: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption(custId);
            var url = Site.resolveClientUrl("BetList/CustomerOutstanding");
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "betlistname", "betlist");
            url = Site.setLicenseeOptionOriginalParam(url);
            var popH = Math.max($(window).height() - 120, 300), popW = 900;

            var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('BetList') + '"><iframe scrolling="no" frameborder="0" width="' +
                         popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 70
            });
        },

        // ------------------------------------------------------------------------
        // ---------------View Statement-------------------------------------------
        viewStatement: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption(custId);
            var url = Site.resolveClientUrl("BetList/CustomerOutstanding");
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "betlistname", "dailystatement");
            url = Site.setLicenseeOptionOriginalParam(url);
            var popH = Math.max($(window).height() - 120, 300), popW = 900;

            var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('DailyStatement') + '"><iframe frameborder="0" width="' +
                        popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 70
            });
        },
        // ------------------------------------------------------------------------
        // ---------------View Running Betlist-------------------------------------
        viewRunningBetlist: function (custId, systemId, externalId) {
            var url = Site.resolveClientUrl("BetList/CustomerOutstanding");
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "betlistname", "betlistrunning");
            url = Site.setLicenseeOptionOriginalParam(url);

            var popH = 700, popW = 900;

            var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('BetListRunning') + '"><iframe frameborder="0" width="' +
                         popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 50
            });
        }
    };
})(jQuery);

$(document).ready(function () {
    "use strict";
    parent.document.title = 'LRF - ' + $('.content-title').html();
    CustomerList.initialize();
    CustomerList.submit();
});