"use strict";
define([
    "jquery",
    "common/site",
    "common/querystring",
    "./customerlist.state",
    "./customerlist.service",
    "./customerlist.service.dormant",
    "./customerlist.bastatus",
    "./customerlist.speciallevel",
    "./customerlist.oddsgroup",
    "./customerlist.custstatus",
    "components/paging",
    "common/popup",
    "lib/jqgrid/jquery.jqgrid.additional"],
    function (
        $,
        Site,
        QueryString,
        CustomerListState,
        CustomerListService,
        CustomerListServiceDormant,
        CustomerListBAStatus,
        CustomerListSpecialLevel,
        CustomerListOddsGroup,
        CustomerListCustStatus,
        PagingHelper,
        Popup,
        jqGridAdditional) {
        var _state;
        var _dormantState;
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
                _dormantState = CustomerListState.getDormantState();
                window.CustomerList = {
                    ajaxPaging: _me.ajaxPaging,
                    ajaxPagingDormant: _me.ajaxPagingDormant,
                };

                $("#SubmitButton").prop("disabled", true);

                CustomerListBAStatus.initialize();
                CustomerListSpecialLevel.initialize();
                CustomerListOddsGroup.initialize();
                CustomerListCustStatus.initialize();

                _me.initEvent();
                $("#SubmitButton").prop("disabled", false);
            },

            ajaxPaging: function () {
                _state.pagingOption = PagingHelper.getSetting();
                CustomerListService.submit();
            },

            ajaxPagingDormant: function () {
             //   _dormantState.pagingOption = PagingHelper.getSetting();
                CustomerListServiceDormant.submit();
            },

            initEvent: function () {
                $('#ExportToExcel').click(function () {
                    $(this).closest('form').submit();
                });

                _me.registerCheckboxEvent();

                $("#active-customer-report-content").on("click", ".pendingFunds", function () {
                    var $this = $(this);
                    var custId = $this.data("custid");
                    var systemId = $this.data("systemid");
                    var externalId = $this.data("externalid");

                    _me.viewRunningBetlist(custId, systemId, externalId);
                });

                $(window).on("resize", function () {
                    CustomerListService.resizeGrid("#ReportGrid");
                    jqGridAdditional.updateGridHeight($("#tabDormantCustomer"));
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            registerCheckboxEvent: function () {
                $("#active-customer-report-content").on("change", "#CheckAll", function () {
                    var isCheckedAll = this.checked;
                    var selectedCustomers = {};

                    if (isCheckedAll) {
                        _state.cachedData.Data.map(function (item) {
                            selectedCustomers[item.CustId] = item.GroupCommId;
                        });
                    }

                    _state.selectedCustomers = selectedCustomers;
                    _state.checkedAllRows = isCheckedAll;
                    CustomerListService.bindActiveCustomerData(_state.cachedData);
                });

                $("#active-customer-report-content").on("change", ".select-cust-checkbox", function () {
                    var custid = Number(this.id);
                    var group = $(this).data("group-comm-id");
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
                window.logOpenBetlistEvent && window.logOpenBetlistEvent();
            },

            updateGridGroupAndComm: function (groupid, groupName, groupColor, sbCom, ngCom, csCom, abCom) {
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
                        item.CSComm = csCom;
                        count++;
                    }
                });

                updateGroupForCheckCustomerArray(groupid);
                CustomerListService.bindActiveCustomerData(_state.cachedData);
            },

            updateGridGroup: function (groupid, groupName, groupColor) {
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
                        count++;
                    }
                });

                updateGroupForCheckCustomerArray(groupid);
                CustomerListService.bindActiveCustomerData(_state.cachedData);
            }
        };

        return _me;
    });