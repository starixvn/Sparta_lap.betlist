define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional", 'common/popup', "./customerlist.state", "./customerlist.grid"],
    function ($, Site, jqGridAdditional, Popup, CustomerListState, CustomerListGrid) {
        "use strict";

        var _state;

        var _me = {
            initialize: function () {
                _state = CustomerListState.getState();

                $('#MoveToGroup').click(function () {
                    _me.showMoveCustomerToGroup();
                });

                $('#UpdateComm').click(function () {
                    var groupCommId = $('#UpdateComm').data('group-id');
                    _me.showUpdateCustComm(groupCommId);
                });

                $('#RemoveFromGroup').click(_me.showRemoveFromGroup);

                $("#GroupListSelect").change(function () {
                    _me.newGroupChanged(this);
                });

                $('#CustSBComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), 'CustSBComm');
                });

                $('#CustParlayComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), 'CustParlayComm');
                });

                $('#CustRBComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), 'CustRBComm');
                });

                $('#CustNGComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), 'CustNGComm');
                });

                $('#CustLCComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), 'CustLCComm');
                });

                $('#CustCSComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), 'CustCSComm');
                });
            },

            commissionOnChange: function (value, idName) {
                var comm2 = "#" + idName + "2", comm3 = "#" + idName + "3";

                if (Number(value) === 15) {
                    $(comm2).val(0);
                    $(comm2).selectList("update", {
                        isDisabled: true,
                        selectionId: "select-list-" + idName + "2",
                        labelText: "0"
                    });
                    $(comm3).val(0);
                    $(comm3).selectList("update", {
                        isDisabled: true,
                        selectionId: "select-list-" + idName + "3",
                        labelText: "0"
                    });
                } else {
                    $(comm2).selectList("disabled", false);
                    $(comm3).selectList("disabled", false);
                }
            },

            // This function used to remove data row after updated/remove or move group commission.
            processGroupCommItem: function (status) {
                var selectList = [];
                var list = $("input:checkbox.check-child:checked");
                for (var i = 0; i < list.length; i++) {
                    selectList.push(list[i].id);
                }

                if (status) {
                    for (var j = 0; j < selectList.length; j++) {
                        $('#row' + selectList[j]).remove();
                    }
                }
            },

            /* move customer to group*/
            showMoveCustomerToGroup: function () {
                $('#lblMoveToGroupMsg').html("");
                _me.clearNotification();

                Popup.openPopupWithButton($("#MoveToGroupPopUp"),
                    {
                        title: Site.getSingleResource('MoveToGroup'),
                        id: 'MoveToGroupPopUp',
                        height: "auto",
                        width: 350
                    },
                    [
                        {
                            text: Site.getSingleResource('OK'),
                            click: function () {
                                _me.moveCustomerToGroup();
                            },
                            classes: "btn btn-submit"
                        },
                        {
                            text: Site.getSingleResource('Cancel'),
                            isClosedOnClick: true,
                            classes: "btn btn-cancel"
                        }
                    ]
                );
            },

            moveCustomerToGroup: function () {
                var groupid = Number($('#GroupListSelect').val());

                if (groupid === -1) {
                    _me.showMsg(Site.getSingleResource('ChooseGroupMsg'), false);
                    return;
                }

                var selectedCustomers = _state.selectedCustomers;

                if (Object.keys(selectedCustomers).length === 0) {
                    _me.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                    return;
                }

                var custList = [];

                for (var custId in selectedCustomers) {
                    if (selectedCustomers.hasOwnProperty(custId)) {
                        var currentGroupId = selectedCustomers[custId];
                        if (currentGroupId === 0 || currentGroupId !== groupid) {
                            custList.push(custId);
                        }
                    }
                }

                if (custList.length === 0) {
                    _me.showMsg(Site.getSingleResource('CustomersExistedinGroupMsg'), false);
                    return;
                }
                var param =
                {
                    CustomerIds: custList.toString(),
                    GroupComId: groupid
                };
                $.ajax({
                    type: "POST",
                    data: param,
                    url: Site.resolveClientUrl("CustomerAPI/MoveToGroupCom"),
                    dataType: "json",
                    beforeSend: function () {
                        _me.ajaxStart();
                    },
                    complete: function () {
                        _me.ajaxStop();
                    },
                    success: function (result) {
                        if (result.Success) {
                            _me.showMsg(Site.getSingleResource('MoveSuccessfullyMsg'), result.Success);

                            var groupName = result.ExtendedData.GroupName;
                            if (groupName.length > 17) {
                                groupName = groupName.substring(0, 14);
                                groupName += "...";
                            }
                            var groupNameText = "<span title='" + result.ExtendedData.GroupName + "'>" + groupName + "</span>";

                            CustomerListGrid.updateGridGroupAndComm(groupid, groupNameText, result.ExtendedData.GroupColor,
                                                 result.ExtendedData.SBComm, result.ExtendedData.NGComm,
                                                result.ExtendedData.RCComm, result.ExtendedData.LCComm, result.ExtendedData.RNGComm);
                        }
                        else {
                            _me.showMsg(result.ErrorMessage, result.Success);
                        }
                    }
                });
            },
            /* End- move customer to group*/

            /* update customer*/
            showUpdateCustComm: function (groupCommId) {
                var isShowInfo = false;
                var selectedCustomers = _state.selectedCustomers;

                for (var custId in selectedCustomers) {
                    if (selectedCustomers.hasOwnProperty(custId) && selectedCustomers[custId] !== 0) {
                        isShowInfo = true;
                        break;
                    }
                }

                $('#InfoMsg').toggle(isShowInfo);
                _me.clearNotification();

                Popup.openPopupWithButton($("#UpdateCustCommPopUp"),
                    {
                        title: Site.getSingleResource("UpdateCustomerCommission"),
                        id: 'UpdateCustCommPopUp',
                        height: "auto",
                        width: 500
                    },
                    [
                        {
                            text: Site.getSingleResource('Save'),
                            click: function () {
                                _me.updateCustomerComm(groupCommId);
                            },
                            classes: "btn btn-submit"
                        },
                        {
                            text: Site.getSingleResource('Cancel'),
                            isClosedOnClick: true,
                            classes: "btn btn-cancel"
                        }
                    ]
                );
            },
            initCommissionParam: function (id) {
                var comm1 = parseInt($(id + 1).val());
                var comm2 = parseInt($(id + 2).val());
                var comm3 = parseInt($(id + 3).val());
                var comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
                return comm;
            },
            updateCustomerComm: function (groupCommId) {
                var custList = Object.keys(_state.selectedCustomers);

                if (custList.length === 0) {
                    _me.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                    return;
                }

                var sbcom = _me.initCommissionParam('#CustSBComm');
                var parlayCom = _me.initCommissionParam('#CustParlayComm');
                var rbcom = _me.initCommissionParam('#CustRBComm');
                var ngcom = _me.initCommissionParam('#CustNGComm');
                var lccom = _me.initCommissionParam('#CustLCComm');
                var cscom = _me.initCommissionParam('#CustCSComm');
                var param =
                {
                    GroupId: groupCommId,
                    CustomerIds: custList.toString(),
                    SBComm: sbcom,
                    ParlayComm: parlayCom,
                    RCComm: rbcom,
                    NGComm: ngcom,
                    LCComm: lccom,
                    CSComm: cscom
                };

                $.ajax({
                    type: "POST",
                    data: param,
                    url: Site.resolveClientUrl("CustomerAPI/UpdateCustomerCom"),
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    dataType: 'json',
                    beforeSend: function () {
                        _me.ajaxStart();
                    },
                    complete: function () {
                        _me.ajaxStop();
                    },
                    success: function (result) {
                        if (result.Success) {
                            _me.showMsg(Site.getSingleResource('UpdateCommSucessfulMsg'), result.Success);

                            CustomerListGrid.updateGridGroupAndComm(0, result.ExtendedData.GroupName, result.ExtendedData.GroupColor,
                                                 result.ExtendedData.SBComm, result.ExtendedData.NGComm,
                                                result.ExtendedData.RCComm, result.ExtendedData.LCComm, result.ExtendedData.RNGComm);
                        }
                        else {
                            _me.showMsg(result.ErrorMessage, result.Success);
                        }
                    }
                });
            },
            /* end-update custom*/

            /* remove from group*/
            showRemoveFromGroup: function () {
                var msg = Object.keys(_state.selectedCustomers).length > 1
                    ? Site.getSingleResource('RemoveTheseCustomersFromGroupMsg')
                    : Site.getSingleResource('RemoveThisCustomerFromGroupMsg');

                $("#RemoveConfirmMsg").html(msg);

                _me.clearNotification();

                Popup.openPopupWithButton($("#ConfirmRemoveFromGroupPopUp"),
                    {
                        title: Site.getSingleResource("RemoveFromGroup"),
                        id: 'ConfirmRemoveFromGroupPopUp',
                        height: "auto",
                        width: 400
                    },
                    [
                        {
                            text: Site.getSingleResource('OK'),
                            click: function () {
                                _me.removeFromGroupComm();
                            },
                            classes: "btn btn-submit"
                        },
                        {
                            text: Site.getSingleResource('Cancel'),
                            isClosedOnClick: true,
                            click: function () {
                                $('input[name="RadioComm"]:eq(1)').prop("checked", true);
                            },
                            classes: "btn btn-cancel"
                        }
                    ]
                );
            },

            removeFromGroupComm: function () {
                var checkedCustomers = _state.selectedCustomers;
                var numberOfCheckedCustomers = Object.keys(checkedCustomers).length;
                var groupList = [];

                for (var customerId in checkedCustomers) {
                    if (checkedCustomers.hasOwnProperty(customerId) && checkedCustomers[customerId] > 0) {
                        groupList.push(checkedCustomers[customerId]);
                    }
                }

                if (numberOfCheckedCustomers === 0) {
                    _me.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                    return;
                }

                if (groupList.length === 0) {
                    _me.showMsg(Site.getSingleResource('CustomerNotBelongAnyGroupMsg'), false);
                    return;
                }

                var iskeepcom = $('#KeepComm').is(':checked');
                var param =
                {
                    CustomerIds: Object.keys(checkedCustomers).toString(),
                    isRemovePlayercomm: !iskeepcom
                };

                $.ajax({
                    type: "POST",
                    data: param,
                    url: Site.resolveClientUrl("CustomerAPI/RemoveFromGroupCom"),
                    dataType: "json",
                    beforeSend: _me.ajaxStart,
                    complete: _me.ajaxStop,
                    success: function (result) {
                        if (result.Success) {
                            if (numberOfCheckedCustomers > 0 && groupList.length > 0 && groupList.length < numberOfCheckedCustomers) {
                                _me.showMsg(result.Message + "! </br> " + Site.getSingleResource('CustomersBelongGroupsBeRemovedMsg'), true);
                            }
                            else {
                                _me.showMsg(result.Message, result.Success);
                            }
                            if (param.isRemovePlayercomm) {
                                CustomerListGrid.updateGridGroupAndComm(0, result.ExtendedData.GroupName, "",
                                                 result.ExtendedData.SBComm, result.ExtendedData.NGComm,
                                                result.ExtendedData.RCComm, result.ExtendedData.LCComm, result.ExtendedData.RNGComm);
                            }
                            else {
                                CustomerListGrid.updateGridGroup(0, result.ExtendedData.GroupName, result.ExtendedData.GroupColor);
                            }
                        }
                        else {
                            _me.showMsg(result.ErrorMessage, result.Success);
                        }
                    }
                });
            },

            showMsg: function (msg, isSuccess) {
                if (isSuccess) {
                    $(".success-message").html(msg).show();
                    $(".error-message").hide();
                }
                else {
                    $(".error-message").html(msg).show();
                    $(".success-message").hide();
                }
            },

            ajaxStart: function () {
                $(".error-message").hide();
                $(".success-message").hide();
                $(".ajax-loading").show();
                $("button", ".popup-button").attr("disabled", true);
                $(".popup-close").addClass("disabled");
            },

            ajaxStop: function () {
                $(".ajax-loading").hide();
                $("button", ".popup-button").attr("disabled", false);
                $(".popup-close").removeClass("disabled");
            },

            clearNotification: function () {
                $(".ajax-loading").hide();
                $(".error-message").hide();
                $(".success-message").hide();
            },

            newGroupChanged: function (obj) {
                var groupId = obj.options[obj.selectedIndex].value;
                $('#NewSbComm').text($("#SBComm" + groupId).val());
                $('#NewParlayComm').text($("#ParlayComm" + groupId).val());
                $('#NewRbComm').text($("#RBComm" + groupId).val());
                $('#NewNgComm').text($("#NGComm" + groupId).val());
                $('#NewLcComm').text($("#LCComm" + groupId).val());
                $('#NewCsComm').text($("#CSComm" + groupId).val());
            }
        };

        return _me;
    });