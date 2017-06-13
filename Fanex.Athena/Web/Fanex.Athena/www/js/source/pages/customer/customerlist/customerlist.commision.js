/*global Site, CustomerList, JSLINQ*/
/// <reference path="/@JSense.js" />
define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional", 'common/popup',
'./customerlist', "./customerlist.state"],
    function ($, Site, jqGridAdditional, Popup, CustomerList, CustomerListState) {
        "use strict";

        var _state;

        var _me = {
            initialize: function () {
                _state = CustomerListState.getState();

                // bind customer button event
                $('#MoveToGroup').click(function () {
                    _me.showMoveCustomerToGroup();
                });

                $('#UpdateComm').click(function () {
                    var groupCommId = $('#UpdateComm').attr('data-group-id');
                    _me.showUpdateCustComm(groupCommId);
                });

                $('#RemoveFromGroup').click(function () {
                    _me.showRemoveFromGroup();
                });

                $("#GroupListSelect").change(function () {
                    _me.newGroupChanged(this);
                });

                // Commission dropdownlist change
                $('#CustSBComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), '#CustSBComm');
                });

                $('#CustParlayComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), '#CustParlayComm');
                });

                $('#CustRBComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), '#CustRBComm');
                });

                $('#CustNGComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), '#CustNGComm');
                });

                $('#CustLCComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), '#CustLCComm');
                });

                $('#CustCSComm1').on('change', function () {
                    _me.commissionOnChange($(this).val(), '#CustCSComm');
                });
            },

            commissionOnChange: function (value, id) {
                if (Number(value) === 15) {
                    $(id + 2 + ', ' + id + 3).val(0);
                    $(id + 2 + ', ' + id + 3).attr('disabled', 'disabled');
                } else {
                    $(id + 2 + ', ' + id + 3).removeAttr('disabled');
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
                        height: 450,
                        width: 350
                    },   
                    [
                        {
                            text: Site.getSingleResource('OK'),
                            click: function () {
                                _me.moveCustomerToGroup($(this));
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
                var groupid = $('#GroupListSelect').val();
                
                //GroupId = - 1 is show all group.
                if (groupid === "-1") {
                    _me.showMsg(Site.getSingleResource('ChooseGroupMsg'), false);
                    return;
                }

                var selectedCustomers = _state.selectedCustomers

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

                            CustomerList.updateGridGroupAndComm(groupid, groupNameText, result.ExtendedData.GroupColor,
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
                var checkedCustomers = CustomerList.getCheckedCustomers();
                
                for (var i = 0; i < checkedCustomers.length; i++) {
                    if (Number(checkedCustomers[i].Group) !== 0) {
                        isShowInfo = true;
                        break;
                    }
                }

                $('#InfoMsg').css('display', isShowInfo ? 'block' : 'none');
                _me.clearNotification();

                $("#UpdateCustCommPopUp").dialog({
                    resizable: false,
                    height: 350,
                    width: 500,
                    modal: true,
                    buttons:
                    [
                        {
                            text: Site.getSingleResource('Save'),
                            click: function () {
                                _me.updateCustomerComm($(this), groupCommId);
                            }
                        },
                        {
                            text: Site.getSingleResource('Cancel'),
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            },
            initCommissionParam: function (id) {
                var comm1 = parseInt($(id + 1).val());
                var comm2 = parseInt($(id + 2).val());
                var comm3 = parseInt($(id + 3).val());
                var comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
                return comm;
            },
            updateCustomerComm: function (ctx, groupCommId) {
                var checkedCustomers = CustomerList.getCheckedCustomers();
                var custList = JSLINQ(checkedCustomers).Select(function (x) { return x.Id; }).ToArray();

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
                            //updated = true;
                            _me.showMsg(Site.getSingleResource('UpdateCommSucessfulMsg'), result.Success);

                            CustomerList.updateGridGroupAndComm(0, result.ExtendedData.GroupName, result.ExtendedData.GroupColor,
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
                var checkedCustomers = CustomerList.getCheckedCustomers();

                var msg = checkedCustomers.length > 1
                    ? Site.getSingleResource('RemoveTheseCustomersFromGroupMsg')
                    : Site.getSingleResource('RemoveThisCustomerFromGroupMsg');

                $("#RemoveConfirmMsg").html(msg);

                _me.clearNotification();

                $("#ConfirmRemoveFromGroupPopUp").dialog({
                    resizable: false,
                    height: 300,
                    width: 330,
                    modal: true,
                    buttons:
                    [
                        {
                            text: Site.getSingleResource('OK'),
                            click: function () {
                                _me.removeFromGroupComm($(this));
                            }
                        },
                        {
                            text: Site.getSingleResource('Cancel'),
                            click: function () {
                                $('input[name="RadioComm"]:eq(1)').attr("checked", "checked");
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            },

            removeFromGroupComm: function () {
                var checkedCustomers = CustomerList.getCheckedCustomers();
                var custList = [];

                for (var i = 0; i < checkedCustomers.length; i++) {
                    if (checkedCustomers[i].Group > 0) {
                        custList.push(checkedCustomers[i].Id);
                    }
                }

                if (checkedCustomers.length === 0) {
                    _me.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                    return;
                }
                if (checkedCustomers.length > 0 && custList.length === 0) {
                    _me.showMsg(Site.getSingleResource('CustomerNotBelongAnyGroupMsg'), false);
                    return;
                }

                var iskeepcom = $('#KeepComm').is(':checked');
                var param =
                {
                    CustomerIds: custList.toString(),
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
                            //removed = true;

                            if (checkedCustomers.length > 0 && custList.length > 0 && custList.length < checkedCustomers.length) {
                                _me.showMsg(result.Message + "! </br> " + Site.getSingleResource('CustomersBelongGroupsBeRemovedMsg'), true);
                            }
                            else {
                                _me.showMsg(result.Message, result.Success);
                            }
                            if (param.isRemovePlayercomm) {
                                CustomerList.updateGridGroupAndComm(0, result.ExtendedData.GroupName, "",
                                                 result.ExtendedData.SBComm, result.ExtendedData.NGComm,
                                                result.ExtendedData.RCComm, result.ExtendedData.LCComm, result.ExtendedData.RNGComm);
                            }
                            else {
                                CustomerList.updateGridGroup(0, result.ExtendedData.GroupName, result.ExtendedData.GroupColor);
                            }
                        }
                        else {
                            _me.showMsg(result.ErrorMessage, result.Success);
                        }
                    }
                });
            },

            /* end -remove from group*/

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
                $(".ui-dialog-buttonpane button:contains('Save')").attr("disabled", true).addClass("ui-state-disabled");
                $(".ui-dialog-buttonpane button:contains('OK')").attr("disabled", true).addClass("ui-state-disabled");
                $(".ui-dialog-buttonpane button:contains('Cancel')").attr("disabled", true).addClass("ui-state-disabled");
                $(".ui-dialog-titlebar-close").attr("disabled", true).css("display", "none").addClass("ui-state-disabled");
            },

            ajaxStop: function () {
                $(".ajax-loading").hide();
                $(".ui-dialog-buttonpane button:contains('Save')").attr("disabled", false).removeClass("ui-state-disabled");
                $(".ui-dialog-buttonpane button:contains('OK')").attr("disabled", false).removeClass("ui-state-disabled");
                $(".ui-dialog-buttonpane button:contains('Cancel')").attr("disabled", false).removeClass("ui-state-disabled");
                $(".ui-dialog-titlebar-close").attr("disabled", false).css("display", "inline").removeClass("ui-state-disabled");
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
