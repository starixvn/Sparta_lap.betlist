/*global Site, CustomerList, JSLINQ*/
/// <reference path="/@JSense.js" />
"use strict";
var CustomerComm = (function ($) {
    return {
        initialize: function () {
            // bind customer button event
            $('#MoveToGroup').click(function () {
                CustomerComm.showMoveCustomerToGroup();
            });

            $('#UpdateComm').click(function () {
                var groupCommId = $('#UpdateComm').attr('data-group-id');
                CustomerComm.showUpdateCustComm(groupCommId);
            });

            $('#RemoveFromGroup').click(function () {
                CustomerComm.showRemoveFromGroup();
            });

            $("#GroupListSelect").change(function () {
                CustomerComm.newGroupChanged(this);
            });

            // Commission dropdownlist change
            $('#CustSBComm1').on('change', function () {
                CustomerComm.commissionOnChange($(this).val(), '#CustSBComm');
            });

            $('#CustRBComm1').on('change', function () {
                CustomerComm.commissionOnChange($(this).val(), '#CustRBComm');
            });

            $('#CustNGComm1').on('change', function () {
                CustomerComm.commissionOnChange($(this).val(), '#CustNGComm');
            });

            $('#CustLCComm1').on('change', function () {
                CustomerComm.commissionOnChange($(this).val(), '#CustLCComm');
            });

            $('#CustCSComm1').on('change', function () {
                CustomerComm.commissionOnChange($(this).val(), '#CustCSComm');
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

        //This function used to remove data row after updated/remove or move group commission.
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
            CustomerComm.clearNotification();

            $("#MoveToGroupPopUp").dialog({
                resizable: false,
                height: 260,
                width: 320,
                modal: true,
                buttons: [
                    {
                        text: Site.getSingleResource('OK'),
                        click: function () {
                            CustomerComm.moveCustomerToGroup($(this));
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

        moveCustomerToGroup: function () {
            var groupid = parseInt($('#GroupListSelect').val(), 10);
            var checkedCustomers = CustomerList.getCheckedCustomers();
            var custList = [];

            for (var i = 0; i < checkedCustomers.length; i++) {
                var currenntGroupId = Number(checkedCustomers[i].Group);                
                if (currenntGroupId === 0 || currenntGroupId !== groupid) {
                    custList.push(checkedCustomers[i].Id);
                }
            }

            if (checkedCustomers.length === 0) {
                CustomerComm.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                return;
            }

            //GroupId = - 1 is show all group.
            if (groupid === -1) {
                CustomerComm.showMsg(Site.getSingleResource('ChooseGroupMsg'), false);
                return;
            }
            if (checkedCustomers.length > 0 && custList.length === 0) {
                CustomerComm.showMsg(Site.getSingleResource('CustomersExistedinGroupMsg'), false);
                return;
            }
            var param =
            {
                Customers: custList,
                GroupComId: groupid
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(param),
                url: Site.resolveClientUrl("CustomerAPI/MoveToGroupCom"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    CustomerComm.ajaxStart();
                },
                complete: function () {
                    CustomerComm.ajaxStop();
                },
                success: function (result) {
                    if (result.Success) {
                        CustomerComm.showMsg(Site.getSingleResource('MoveSuccessulMsg'), result.Success);

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
                        CustomerComm.showMsg(result.ErrorMessage, result.Success);
                    }
                },
                error: function () { }
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
            CustomerComm.clearNotification();

            $("#UpdateCustCommPopUp").dialog({
                resizable: false,
                height: 300,
                width: 500,
                modal: true,
                buttons:
                [
                    {
                        text: Site.getSingleResource('Save'),
                        click: function () {
                            CustomerComm.updateCustomerComm($(this), groupCommId);
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
                CustomerComm.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                return;
            }

            var sbcom = CustomerComm.initCommissionParam('#CustSBComm');
            var rbcom = CustomerComm.initCommissionParam('#CustRBComm');
            var ngcom = CustomerComm.initCommissionParam('#CustNGComm');
            var lccom = CustomerComm.initCommissionParam('#CustLCComm');
            var cscom = CustomerComm.initCommissionParam('#CustCSComm');
            var param =
            {
                GroupId: groupCommId,
                Customers: custList,
                SBComm: sbcom,
                RCComm: rbcom,
                NGComm: ngcom,
                LCComm: lccom,
                CSComm: cscom
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(param),
                url: Site.resolveClientUrl("CustomerAPI/UpdateCustomerCom"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    CustomerComm.ajaxStart();
                },
                complete: function () {
                    CustomerComm.ajaxStop();
                },
                success: function (result) {
                    if (result.Success) {
                        //updated = true;
                        CustomerComm.showMsg(Site.getSingleResource('UpdateCommSucessfulMsg'), result.Success);

                        CustomerList.updateGridGroupAndComm(0, result.ExtendedData.GroupName, result.ExtendedData.GroupColor,
                                             result.ExtendedData.SBComm, result.ExtendedData.NGComm,
                                            result.ExtendedData.RCComm, result.ExtendedData.LCComm, result.ExtendedData.RNGComm);
                    }
                    else {
                        CustomerComm.showMsg(result.ErrorMessage, result.Success);
                    }
                },
                error: function (/*jqXHR, textStatus, errorThrown*/) { }
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

            CustomerComm.clearNotification();

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
                            CustomerComm.removeFromGroupComm($(this));
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
                CustomerComm.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                return;
            }
            if (checkedCustomers.length > 0 && custList.length === 0) {
                CustomerComm.showMsg(Site.getSingleResource('CustomerNotBelongAnyGroupMsg'), false);
                return;
            }

            var iskeepcom = $('#KeepComm').is(':checked');
            var param =
            {
                Customers: custList,
                isRemovePlayercomm: !iskeepcom
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(param),
                url: Site.resolveClientUrl("CustomerAPI/RemoveFromGroupCom"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    CustomerComm.ajaxStart();
                },
                complete: function () {
                    CustomerComm.ajaxStop();
                },
                success: function (result) {
                    if (result.Success) {
                        //removed = true;

                        if (checkedCustomers.length > 0 && custList.length > 0 && custList.length < checkedCustomers.length) {
                            CustomerComm.showMsg(result.Message + "! </br> " + Site.getSingleResource('CustomersBelongGroupsBeRemovedMsg'), true);
                        }
                        else {
                            CustomerComm.showMsg(result.Message, result.Success);
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
                        CustomerComm.showMsg(result.ErrorMessage, result.Success);
                    }
                },
                error: function (/*jqXHR, textStatus, errorThrown*/) { }
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
            $('#NewSbComm').val($("#SBComm" + groupId).val());
            $('#NewRbComm').val($("#RBComm" + groupId).val());
            $('#NewNgComm').val($("#NGComm" + groupId).val());
            $('#NewLcComm').val($("#LCComm" + groupId).val());
            $('#NewCsComm').val($("#CSComm" + groupId).val());
        }
    };
})(jQuery);

jQuery(function () {
    CustomerComm.initialize();
});