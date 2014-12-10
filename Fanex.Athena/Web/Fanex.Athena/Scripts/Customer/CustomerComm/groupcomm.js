/*global Site*/
/// <reference path="/@JSense.js" />
"use strict";
var GroupComm = (function ($) {
    return {
        getcolorPalette: function () {
            return ['000000', '000033', '000066', '000099 ', '0000CC', '0000FF',
                '003300', '003333', '003366', '003399 ', '0033CC', '0033FF',
                '006600', '006633', '006666 ', '006699 ', '0066CC', '0066FF',
                '009900', '009933', '009966 ', '009999 ', '0099CC', '0099FF',
                '00CC00', '00CC33', '00CC66 ', '00CC99 ', '00CCCC', '00CCFF',
                '00FF00', '00FF33', '00FF66 ', '00FF99 ', '00FFCC', '00FFFF',
                '330000', '330033', '330066 ', '330099 ', '3300CC', '3300FF',
                '333300', '333333', '333366 ', '333399 ', '3333CC', '3333FF',
                '336600', '336633', '336666 ', '336699 ', '3366CC', '3366FF',
                '339900', '339933', '339966 ', '339999 ', '3399CC', '3399FF',
                '33CC00', '33CC33', '33CC66 ', '33CC99 ', '33CCCC', '33CCFF',
                '33FF00', '33FF33', '33FF66 ', '33FF99 ', '33FFCC', '33FFFF',
                '660000', '660033', '660066 ', '660099 ', '6600CC', '6600FF',
                '663300', '663333', '663366 ', '663399 ', '6633CC', '6633FF',
                '666600', '666633', '666666 ', '666699 ', '6666CC', '6666FF',
                '669900', '669933', '669966 ', '669999 ', '6699CC', '6699FF',
                '66CC00', '66CC33', '66CC66 ', '66CC99 ', '66CCCC', '66CCFF',
                '66FF00', '66FF33', '66FF66 ', '66FF99 ', '66FFCC', '66FFFF',
                '990000', '990033', '990066 ', '990099 ', '9900CC', '9900FF',
                '993300', '993333', '993366 ', '993399 ', '9933CC', '9933FF',
                '996600', '996633', '996666 ', '996699 ', '9966CC', '9966FF',
                '999900', '999933', '999966 ', '999999 ', '9999CC', '9999FF',
                '99CC00', '99CC33', '99CC66 ', '99CC99 ', '99CCCC', '99CCFF',
                '99FF00', '99FF33', '99FF66 ', '99FF99 ', '99FFCC', '99FFFF',
                'CC0000', 'CC0033', 'CC0066 ', 'CC0099 ', 'CC00CC', 'CC00FF',
                'CC3300', 'CC3333', 'CC3366 ', 'CC3399 ', 'CC33CC', 'CC33FF',
                'CC6600', 'CC6633', 'CC6666 ', 'CC6699 ', 'CC66CC', 'CC66FF',
                'CC9900', 'CC9933', 'CC9966 ', 'CC9999 ', 'CC99CC', 'CC99FF',
                'CCCC00', 'CCCC33', 'CCCC66 ', 'CCCC99 ', 'CCCCCC', 'CCCCFF',
                'CCFF00', 'CCFF33', 'CCFF66 ', 'CCFF99 ', 'CCFFCC', 'CCFFFF',
                'FF0000', 'FF0033', 'FF0066 ', 'FF0099 ', 'FF00CC', 'FF00FF',
                'FF3300', 'FF3333', 'FF3366 ', 'FF3399 ', 'FF33CC', 'FF33FF',
                'FF6600', 'FF6633', 'FF6666 ', 'FF6699 ', 'FF66CC', 'FF66FF',
                'FF9900', 'FF9933', 'FF9966 ', 'FF9999 ', 'FF99CC', 'FF99FF',
                'FFCC00', 'FFCC33', 'FFCC66 ', 'FFCC99 ', 'FFCCCC', 'FFCCFF',
                'FFFF00', 'FFFF33', 'FFFF66', 'FFFF99', 'FFFFCC', 'FFFFFF'];
        },
        initialize: function () {
            GroupComm.initEvent();
        },
        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#CommissionGroupForm').submit();
            });

            $('#NewGroupButton').click(function () {
                window.location.href = Site.resolveClientUrl("CustomerComGroup/Create");
            });

            $('#Cancel').click(function () {
                window.location.href = Site.resolveClientUrl("CustomerComGroup/Index");
            });

            $('#keep').click(function () {
                $('input[name="IsRemovePlayerComm"]:eq(0)').prop("checked", true);
            });
            $('#clear').click(function () {
                $('input[name="IsRemovePlayerComm"]:eq(1)').prop("checked", true);
            });
        },
        //Navigate customer list when clink on No.Of customer
        navigateNoOfCustomer: function (groupId) {
            window.location.href = Site.resolveClientUrl("Customer/CustomerList?groupId=" + groupId);
        },
        commissionOnChange: function (value, id) {
            if (Number(value) === 15) {
                $(id + 2 + ', ' + id + 3).val(0);
                $(id + 2 + ', ' + id + 3).prop('disabled', true);
            } else {
                $(id + 2 + ', ' + id + 3).prop('disabled', false);
            }
        },
        initDeleteGroup: function (groupId) {
            // Double check default group before delete
            var url = Site.resolveClientUrl("CustomerComGroup/GetDefaultGroup");
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'JSon',
                cache: false,
                success: function (id) {
                    // Delete group
                    GroupComm.deleteGroup(groupId, id.id.toString() === groupId.toString() ? true : false);
                }
            });
        },
        deleteGroup: function (groupId, isDefault) {
            // Show confirm dialog
            if (isDefault) {
                GroupComm.showConfirmAction(Site.getSingleResource('DefaultGroupMsg'), GroupComm.doDelete, "#delete-group-popup", groupId, true);
            }
            else {
                $("#Delete").dialog({
                    modal: true,
                    width: 300,
                    title: Site.getSingleResource("DeleteCommissionGroup"),
                    height: 150,
                    buttons: {
                        "Delete": function () {
                            $("#confirm-delete").dialog({
                                resizable: false,
                                height: 150,
                                width: 300,
                                modal: true,
                                buttons: {
                                    "OK": function () {
                                        $("#Delete").dialog("close");
                                        GroupComm.process(groupId, $('input[name="IsRemovePlayerComm"]:checked').val(), false);
                                    },
                                    Cancel: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        },
                        Cancel: function () {
                            $('input[name="IsRemovePlayerComm"]').prop("checked", false);
                            $('input[name="IsRemovePlayerComm"]:eq(1)').prop("checked", true);
                            $(this).dialog("close");
                        }
                    },
                    open: function () {
                    },
                    resizable: false
                });
            }
        },
        process: function (groupId, isRemovePlayerComm, isResetDefault) {
            var url = "CustomerComGroup/Delete/?groupId=" + groupId + "&isRemovePlayerComm=" + isRemovePlayerComm + "&isResetDefault=" + isResetDefault;

            $.ajax({
                url: Site.resolveClientUrl(url),
                type: 'GET',
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (result) {
                    if (result.ErrorCode > 0 || result.ErrorMessage.lengh > 0) {
                        $('#confirm-delete').html("<span style='padding-top:10px;'>" + result.ErrorMessage + "</span>");
                        $("#confirm-delete").dialog({
                            title: result.ErrorCode > 0 ? "Error" : "Successful",
                            resizable: false,
                            height: 150,
                            width: 300,
                            modal: true,
                            buttons: {
                                OK: function () {
                                    $(this).dialog("close");
                                    window.location.href = Site.resolveClientUrl("CustomerComGroup/Index");
                                }
                            }
                        });
                    } else {
                        window.location.href = Site.resolveClientUrl("CustomerComGroup/Index");
                    }

                    Site.unBlockUI();
                }
            });
        },
        doDelete: function (groupId, isResetDefault) {
            $("#Delete").dialog({
                modal: true,
                width: 300,
                title: "Delete Commission group",
                height: 150,
                buttons: {
                    "Delete": function () {
                        $("#confirm-delete").dialog({
                            resizable: false,
                            height: 150,
                            width: 300,
                            modal: true,
                            buttons: {
                                "OK": function () {
                                    $("#Delete").dialog("close");
                                    GroupComm.process(groupId, $('input[name="IsRemovePlayerComm"]:checked').val(), isResetDefault);
                                },
                                Cancel: function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    },
                    Cancel: function () {
                        $('input[name="IsRemovePlayerComm"]').prop("checked", false);
                        $('input[name="IsRemovePlayerComm"]:eq(1)').prop("checked", false);
                        $(this).dialog("close");
                    }
                },
                open: function () {
                },
                resizable: false
            });
        },
        initDefaultParam: function (id) {
            var isDefault = $('#IsDefaultGroup').is(':checked');
            var defaultGroupId = parseInt($('#DefaultGroupId').val());
            var currentGroupId = parseFloat($('#GroupCommId').val());
            if (isDefault && defaultGroupId !== currentGroupId && defaultGroupId !== 0) {
                GroupComm.showConfirmAction(Site.getSingleResource('GroupSetAsDefaultMsg'), GroupComm.doSubmit, id);
            } else {
                $('form').submit();
            }
        },
        showConfirmAction: function (message, callback, id, groupId, isResetDefault) {
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                title: Site.getSingleResource("Confirm"),
                height: 150,
                buttons: {
                    OK: function () {
                        $(id).dialog("close");
                        callback(groupId, isResetDefault);
                    },
                    Cancel: function () {
                        $(id).dialog("close");
                    }
                },
                close: function () {
                    $(id).remove();
                }
            };
            $('<div id="' + id.substr(1) + '" title="">' + message + '</div>').dialog(options);
        },
        doSubmit: function () {
            $('#IsResetDefault').val(true);
            $('form').submit();
        },
        initCommParam: function () {
            var sbComm1 = parseInt($('#DdlSBComm1').val());
            var sbComm2 = parseInt($('#DdlSBComm2').val());
            var sbComm3 = parseInt($('#DdlSBComm3').val());
            var rcComm1 = parseInt($('#DdlRCComm1').val());
            var rcComm2 = parseInt($('#DdlRCComm2').val());
            var rcComm3 = parseInt($('#DdlRCComm3').val());
            var ngComm1 = parseInt($('#DdlNGComm1').val());
            var ngComm2 = parseInt($('#DdlNGComm2').val());
            var ngComm3 = parseInt($('#DdlNGComm3').val());
            var lcComm1 = parseInt($('#DdlLCComm1').val());
            var lcComm2 = parseInt($('#DdlLCComm2').val());
            var lcComm3 = parseInt($('#DdlLCComm3').val());
            var csComm1 = parseInt($('#DdlCSComm1').val());
            var csComm2 = parseInt($('#DdlCSComm2').val());
            var csComm3 = parseInt($('#DdlCSComm3').val());
            var sbComm = parseFloat((sbComm1 + sbComm2 / 10 + sbComm3 / 100) / 100).toFixed(4);
            var rcComm = parseFloat((rcComm1 + rcComm2 / 10 + rcComm3 / 100) / 100).toFixed(4);
            var ngComm = parseFloat((ngComm1 + ngComm2 / 10 + ngComm3 / 100) / 100).toFixed(4);
            var lcComm = parseFloat((lcComm1 + lcComm2 / 10 + lcComm3 / 100) / 100).toFixed(4);
            var csComm = parseFloat((csComm1 + csComm2 / 10 + csComm3 / 100) / 100).toFixed(4);

            sbComm = isNaN(sbComm) ? 0 : sbComm;
            rcComm = isNaN(rcComm) ? 0 : rcComm;
            ngComm = isNaN(ngComm) ? 0 : ngComm;
            lcComm = isNaN(lcComm) ? 0 : lcComm;
            csComm = isNaN(csComm) ? 0 : csComm;

            $('#SBComm').val(sbComm);
            $('#RCComm').val(rcComm);
            $('#NGComm').val(ngComm);
            $('#LCComm').val(lcComm);
            $('#CSComm').val(csComm);

            // Set any commission in drop down list
            $('#SBComm1').val(sbComm1);
            $('#SBComm2').val(sbComm2);
            $('#SBComm3').val(sbComm3);

            $('#RCComm1').val(rcComm1);
            $('#RCComm2').val(rcComm2);
            $('#RCComm3').val(rcComm3);

            $('#NGComm1').val(ngComm1);
            $('#NGComm2').val(ngComm2);
            $('#NGComm3').val(ngComm3);

            $('#LCComm1').val(lcComm1);
            $('#LCComm2').val(lcComm2);
            $('#LCComm3').val(lcComm3);

            $('#CSComm1').val(csComm1);
            $('#CSComm2').val(csComm2);
            $('#CSComm3').val(csComm3);
        }
    };
})(jQuery);

$(window).load(function () {
    GroupComm.initialize();
});