/// <reference path="/@JSense.js" />
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
            if (value == 15) {
                $(id + 2 + ', ' + id + 3).val(0);
                $(id + 2 + ', ' + id + 3).attr('disabled', 'disabled');
            } else {
                $(id + 2 + ', ' + id + 3).removeAttr('disabled');
            }
        },

        //This function used to remove data row after updated/remove or move group commission.
        processGroupCommItem: function (status) {
            var selectList = new Array();
            var list = $("input:checkbox.check-child:checked");
            for (var i = 0; i < list.length; i++) {
                selectList.push(list[i].id);
            };

            if (status) {
                for (var i = 0; i < selectList.length; i++) {
                    $('#row' + selectList[i]).remove();
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

        moveCustomerToGroup: function (ctx) {
            var groupid = parseInt($('#GroupListSelect').val(), 10);
            var strList = new Array();
            var custList = new Array();
            var list = $("input:checkbox.check-child:checked");
            for (var i = 0; i < list.length; i++) {
                strList.push($(list[i]).attr("id"));
                var currenntGroupId = parseInt($(list[i]).attr("data-group-comm-id"));
                if (currenntGroupId == 0 || currenntGroupId != groupid) {
                    custList.push($(list[i]).attr("id"));
                }
            };

            if (strList.length == 0) {
                CustomerComm.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                return;
            };

            //GroupId = - 1 is show all group.
            if (groupid == -1) {
                CustomerComm.showMsg(Site.getSingleResource('ChooseGroupMsg'), false);
                return;
            }
            if (strList.length > 0 && custList.length == 0) {
                CustomerComm.showMsg(Site.getSingleResource('CustomersExistedinGroupMsg'), false)
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
                        $.each(param.Customers, function (index, item) {                            
                            var groupName = result.ExtendedData.GroupName;
                            if (groupName.length > 17) {
                                groupName = groupName.substring(0, 14);
                                groupName += "...";
                            };
                            var groupNameText = "<span title='" + result.ExtendedData.GroupName + "'>" + groupName + "</span>";
                            
                            $("div#groupName-" + item).css("background-color", result.ExtendedData.GroupColor)
                                                      .html(groupNameText);

                            $("td#SbComm-" + item).html(result.ExtendedData.SBComm);
                            $("td#NgComm-" + item).html(result.ExtendedData.NGComm);
                            $("td#RbComm-" + item).html(result.ExtendedData.RCComm);
                            $("td#LcComm-" + item).html(result.ExtendedData.LCComm);
                            $("td#CsComm-" + item).html(result.ExtendedData.RNGComm);
                            $("input#" + item).attr("data-group-comm-id", groupid);
                        });
                    }
                    else {
                        CustomerComm.showMsg(result.ErrorMessage, result.Success);
                    };
                },
                error: function () { }
            });
        },
        /* End- move customer to group*/

        /* update customer*/
        showUpdateCustComm: function (groupCommId) {
            var list = $("input:checkbox.check-child:checked");
            var isShowInfo = false;
            for (var i = 0; i < list.length; i++) {
                if ($(list[i]).attr("data-group-comm-id") != 0) {
                    isShowInfo = true;
                };
            };

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
            var custList = new Array();
            var list = $("input:checkbox.check-child:checked");
            for (var i = 0; i < list.length; i++) {
                custList.push($(list[i]).attr("id"));
            };

            if (custList.length == 0) {
                CustomerComm.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                return;
            };

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
                        $.each(param.Customers, function (index, item) {
                            $("#groupName-" + item).css("background-color", result.ExtendedData.GroupColor)
                                                   .html(result.ExtendedData.GroupName);

                            $("#SbComm-" + item).html(result.ExtendedData.SBComm);
                            $("#NgComm-" + item).html(result.ExtendedData.NGComm);
                            $("#RbComm-" + item).html(result.ExtendedData.RCComm);
                            $("#LcComm-" + item).html(result.ExtendedData.LCComm);
                            $("#CsComm-" + item).html(result.ExtendedData.RNGComm);
                            $("#" + item).attr("data-group-comm-id", 0);
                        });
                    }
                    else {
                        CustomerComm.showMsg(result.ErrorMessage, result.Success);
                    };
                },
                error: function (jqXHR, textStatus, errorThrown) { }
            });
        },
        /* end-update custom*/

        /* remove from group*/
        showRemoveFromGroup: function () {
            var childCheckedCount = $("input:checkbox.check-child:checked").length;
            var msg = childCheckedCount > 1
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

        removeFromGroupComm: function (ctx) {
            var strList = new Array();
            var custList = new Array();
            var list = $("input:checkbox.check-child:checked");
            for (var i = 0; i < list.length; i++) {
                strList.push($(list[i]).attr("id"));
                var currentGroupId = parseInt($(list[i]).attr("data-group-comm-id"));
                if (currentGroupId > 0) {
                    custList.push($(list[i]).attr("id"));
                }
            };

            if (strList.length == 0) {
                CustomerComm.showMsg(Site.getSingleResource('YouNotChooseAnyCustomerMsg'), false);
                return;
            };
            if (strList.length > 0 && custList.length == 0) {
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

                        if (strList.length > 0 && custList.length > 0 && custList.length < strList.length) {
                            CustomerComm.showMsg(result.Message + "! </br> " + Site.getSingleResource('CustomersBelongGroupsBeRemovedMsg'), true);
                        }
                        else {
                            CustomerComm.showMsg(result.Message, result.Success);
                        }
                        if (param.isRemovePlayercomm) {
                            $.each(param.Customers, function (index, item) {
                                $("div#groupName-" + item).css("background-color", "")
                                                          .html(result.ExtendedData.GroupName);

                                $("td#SbComm-" + item).html(result.ExtendedData.SBComm.toFixed(2));
                                $("td#NgComm-" + item).html(result.ExtendedData.NGComm.toFixed(2));
                                $("td#RbComm-" + item).html(result.ExtendedData.RCComm.toFixed(2));
                                $("td#LcComm-" + item).html(result.ExtendedData.LCComm.toFixed(2));
                                $("td#CsComm-" + item).html(result.ExtendedData.RNGComm.toFixed(2));
                                $("input#" + item).attr("data-group-comm-id", 0);
                            });
                        }
                        else {
                            $.each(param.Customers, function (index, item) {
                                $("div#groupName-" + item).css("background-color", result.ExtendedData.GroupColor);
                                $("div#groupName-" + item).html(result.ExtendedData.GroupName);
                                $("input#" + item).attr("data-group-comm-id", 0);
                            });
                        }
                    }
                    else {
                        CustomerComm.showMsg(result.ErrorMessage, result.Success);
                    };
                },
                error: function (jqXHR, textStatus, errorThrown) { }
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
    }
})(jQuery);

jQuery(function () {
    CustomerComm.initialize();
});