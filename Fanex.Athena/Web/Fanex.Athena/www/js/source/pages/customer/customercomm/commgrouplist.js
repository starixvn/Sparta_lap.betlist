define(["jquery", "common/site", "common/popup"],
    function ($, Site, Popup) {
        "use strict";

        var deleteGroupData = {
            groupId: 0,
            isRemovePlayerComm: false,
            isResetDefault: false
        };

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "CommissionGroupList",
                title: Site.getSingleResource("CommissionGroupList")
            });

            _me.initEvent();
        }

        function initEvent() {
            $("#ExportToExcel").click(function () {
                $("#CommissionGroupForm").submit();
            });

            $("#comm-group-list").on("click", ".delete-comm-group", function (e) {
                _me.deleteCommGroup($(e.target).data("id"));
            });
        }

        function deleteCommGroup(groupId) {
            $.ajax({
                url: Site.resolveClientUrl("CustomerComGroup/GetDefaultGroup"),
                type: 'GET',
                dataType: 'JSon',
                cache: false,
                success: function (response) {
                    deleteGroupData.groupId = groupId;
                    if (response.id.toString() === groupId.toString()) {
                        deleteGroupData.isResetDefault = true;
                        showConfirmDeleteDefaulCommGroupPopup();
                    }
                    else {
                        deleteGroupData.isResetDefault = false;
                        showOptionDeleteCommGroupPopup();
                    }
                }
            });
        }

        function doDeleteCommGroup() {
            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl("CustomerComGroup/Delete"),
                data: deleteGroupData,
                success: function (result) {
                    if (result.ErrorCode > 0 || result.ErrorMessage.lengh > 0) {
                        showResultPopup(result.ErrorCode > 0 ? "Error" : "Successful", result.ErrorMessage);
                    } else {
                        Site.redirect("CustomerComGroup/Index");
                    }

                    Site.unBlockUI();
                },
                beforeSend: function () {
                    Site.blockUI();
                },
            });
        }

        function showConfirmDeleteDefaulCommGroupPopup() {
            var options = {
                model: true,
                title: Site.getSingleResource("Confirm"),
                height: 150,
                id: "del-default-comm-group"
            };

            var buttons = [
                {
                    text: "OK",
                    isClosedOnClick: true,
                    click: showOptionDeleteCommGroupPopup,
                    classes: "btn btn-submit"
                }, {
                    text: "Cancel",
                    isClosedOnClick: true,
                    classes: "btn btn-cancel"
                }];

            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource("DefaultGroupMsg") + '</p></div>', options, buttons);
        }

        function showOptionDeleteCommGroupPopup() {
            var popup = $("#option-del-comm-group");

            if (popup.length === 0) {
                popup = $("#confirm-delete-comm-group");
                popup.remove();
                popup.find('input[name="IsRemovePlayerComm"]').on("click", function () {
                    $(this).prop("checked", true);
                });
            }

            var options = {
                model: true,
                title: Site.getSingleResource("DeleteCommissionGroup"),
                height: 160,
                id: "option-del-comm-group"
            };

            var buttons = [
                {
                    text: "Delete",
                    isClosedOnClick: true,
                    click: function () {
                        deleteGroupData.isRemovePlayerComm = $('input[name="IsRemovePlayerComm"]:checked').val();
                        showConfirmDeleteCommGroupPopup();
                    },
                    classes: "btn btn-submit"
                }, {
                    text: "Cancel",
                    isClosedOnClick: true,
                    click: function () {
                        $('#IsRemovePlayerComm').prop("checked", true);
                    },
                    classes: "btn btn-cancel",
                    id: "option-del-comm-group-cancel"
                }];

            Popup.openPopupWithButton(popup, options, buttons);
        }

        function showConfirmDeleteCommGroupPopup() {
            var options = {
                model: true,
                title: Site.getSingleResource("Confirm"),
                height: 150,
                id: "confirm-del-comm-group"
            };

            var buttons = [
                {
                    text: "OK",
                    isClosedOnClick: true,
                    click: doDeleteCommGroup,
                    classes: "btn btn-submit"
                }, {
                    text: "Cancel",
                    isClosedOnClick: true,
                    click: showOptionDeleteCommGroupPopup,
                    classes: "btn btn-cancel",
                    id: "confirm-del-comm-group-cancel"
                }];

            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource("AreYouSure") + '</p></div>', options, buttons);
        }

        function showResultPopup(title, message) {
            var options = {
                model: true,
                title: "",
                height: 150,
                id: "result-del-comm-group"
            };

            if ($("#result-del-comm-group").lengh > 0)
            {
                $("#result-msg").html(message);
            }

            var buttons = [
                {
                    text: "OK",
                    isClosedOnClick: true,
                    classes: "btn btn-cancel",
                    id: "result-OK"
                }];

            Popup.openPopupWithButton('<div><p id="result-msg">' + message + '</p></div>', options, buttons);
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent,
            deleteCommGroup: deleteCommGroup
        };

        return _me;
    }
);