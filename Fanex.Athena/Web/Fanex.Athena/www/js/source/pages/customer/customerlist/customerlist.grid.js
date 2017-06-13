"use strict";
define(["jquery", "common/site", "common/querystring", "./customerlist.state", "./customerlist.service", "./customerlist.bastatus", "./customerlist.speciallevel", "./customerlist.oddsgroup",
    "components/paging", "common/popup"],
    function ($, Site, QueryString, CustomerListState, CustomerListService, CustomerListBAStatus, CustomerListSpecialLevel, CustomerListOddsGroup, PagingHelper, Popup) {
        var _state;

        function updateGroupForCheckCustomerArray(groupid) {
            var selectedCustomers = _state.selectedCustomers;
            for (var custId in selectedCustomers) {
                if (selectedCustomers.hasOwnProperty(custId)) {
                    selectedCustomers[custId] = groupid;
                }
            }
        }

        var _me = {
            initialize: function () {
                _state = CustomerListState.getState();
                window.CustomerList = {
                    ajaxPaging: _me.ajaxPaging
                };

                $("#SubmitButton").prop("disabled", true);

                CustomerListBAStatus.initialize();
                CustomerListSpecialLevel.initialize();
                CustomerListOddsGroup.initialize();

                _me.initEvent();
                $("#SubmitButton").prop("disabled", false);
            },

            ajaxPaging: function () {
                _state.pagingOption = PagingHelper.getSetting();
                CustomerListService.submit();
            },

            initEvent: function () {
                $('#ExportToExcel').click(function () {
                    $(this).closest('form').submit();
                });

                _me.registerCheckboxEvent();

                $("#report-content").on("click", ".pendingFunds", function () {
                    var $this = $(this);
                    var custId = $this.data("custid");
                    var systemId = $this.data("systemid");
                    var externalId = $this.data("externalid");

                    _me.viewRunningBetlist(custId, systemId, externalId);
                });

                $("#popup-update").on("click", _me.updateCustStatus);
                $("#popup-close").on("click", _me.closeUpdatePopUp);

                $("#popup-suspended").change(_me.custStatusCheckedChanged);
                $("#popup-closed").change(_me.custStatusCheckedChanged);

                $("#bet-setting-link").on("click", _me.viewBetSetting);
                $("#bet-list-link").on("click", _me.viewBetList);
                $("#statement-link").on("click", _me.viewStatement);
                $("#login-info-link").on("click", _me.viewLogInfo);
            },

            registerCheckboxEvent: function(){
                $("#report-content").on("change", "#CheckAll", function () {
                    var isCheckedAll = this.checked;
                    var selectedCustomers = {};

                    if (isCheckedAll) {
                        _state.cachedData.Data.map(function (item) {
                            selectedCustomers[item.CustId] = true;
                        });
                    }

                    _state.selectedCustomers = selectedCustomers;
                    _state.checkedAllRows = isCheckedAll;
                    CustomerListService.bindData(_state.cachedData);
                });

                $("#report-content").on("change", ".select-cust-checkbox", function () {
                    var custid = Number(this.id);
                    var group = $(this).attr("data-group-comm-id");
                    var selectedCustomers = _state.selectedCustomers;

                    if (this.checked) {
                        selectedCustomers[custid] = group;
                    } else {
                        delete selectedCustomers[custid];
                    }

                    _state.checkedAllRows = Object.keys(selectedCustomers).length === _state.cachedData.Data.length;
                    $("#CheckAll").prop("checked", _state.checkedAllRows);
                });
            },

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

            closeUpdatePopUp: function () {
                var popup = $("#update-cust-status-popup");
                if (popup.is(":visible")) {
                    $("#update-cust-status-popup").hide();
                }
            },

            custStatusCheckedChanged: function () {
                var popup = $("#update-cust-status-popup");
                var isSuspended = $("#popup-suspended").is(':checked');
                var isClosed = $("#popup-closed").is(':checked');

                var changed = Site.asBool(popup.attr("closed")) !== isClosed || Site.asBool(popup.attr("suspended")) !== isSuspended;
                $("#popup-update").prop("disabled", !changed);
            },

            viewRunningBetlist: function (custId, systemId, externalId) {
                var url = Site.resolveClientUrl("CustomerOutstandingBetList/Index");
                url = QueryString.setParam(url, "custid", custId);
                url = QueryString.setParam(url, "externalid", externalId);
                url = QueryString.setParam(url, "systemid", systemId);
                url = QueryString.setParam(url, "betlistname", "betlistrunning");
                url = Site.setLicenseeOptionOriginalParam(url);

                var popH = Math.min(window.innerHeight - 100, 700), popW = 900;
                var popupOption = { id: "betlist-popup", width: popW + 30, height: popH + 70, title: Site.getSingleResource('BetListRunning') };
                Popup.openPopupWithIframe(url, popupOption);
            },
            updateGridGroupAndComm: function (groupid, groupName, groupColor, sbCom, ngCom, rbCom, lcCom, csCom) {
                var count = 0;
                var length = Object.keys(_state.selectedCustomers).length;

                $.each(_state.cachedData.Data, function (index, item) {
                    if (count === length) {
                        return false;
                    }
                    if (_state.selectedCustomers[item.CustId] !== undefined) {
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
                CustomerListService.bindData(_state.cachedData);
            },

        };

        return _me;
    });