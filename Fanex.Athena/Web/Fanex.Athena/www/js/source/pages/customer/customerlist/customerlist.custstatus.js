/* This column only view in licensee HAIFA */
"use strict";
define(["jquery", "common/site"], function ($, Site) {
    var _me = {
        initialize: function () {
            _me.initEvent();
        },

        initEvent: function () {
            $("#report-content").on("click", ".custStatusOption", function () {
                _me.showUpdatePopUp($(this));
                return false;
            });

            $("#custStatusUpdate").on("click", function () {
                _me.updateCustStatus();
            });

            $("#custStatusCancel").on("click", function () {
                _me.closeUpdatePopUp();
            });

            $("#UpdateCustStatus").on("change", ".chkCustStatus", function () {
                _me.changeStatus();
            });

            $("html").click(function (e) {
                if ($(e.target).closest("#UpdateCustStatus").length === 0) {
                    $("#UpdateCustStatus").fadeOut();
                }
            });
        },

        showUpdatePopUp: function ($context) {
            var custId = $context.data("custid");
            var $statusLink = $("#edit-cust-status-" + custId);
            var $popup = $("#UpdateCustStatus");

            var position = $context.offset();
            $popup.css({
                top: position.top + $context.outerHeight() + 1,
                left: position.left,
                width: "auto"
            });

            if ($popup.data("custid") === custId) {
                $popup.fadeToggle();
            }
            else {
                var isClosed = $statusLink.data("isclosed");
                var isSuspended = $statusLink.data("issuspended");
                var externalId = $statusLink.data("externalid");

                $("#custStatus-externalId").text(externalId);
                $("#custStatus-suspended").prop('checked', isSuspended);
                $("#custStatus-closed").prop('checked', isClosed);

                $popup.data("custid", custId);
                $popup.data("closed", isClosed);
                $popup.data("suspended", isSuspended);

                $("#custStatusUpdate").prop("disabled", true);
                $popup.fadeIn();
            }
        },

        closeUpdatePopUp: function () {
            var $popup = $("#UpdateCustStatus");
            if ($popup.is(":visible")) {
                $popup.fadeOut();
            }
        },

        updateCustStatus: function () {
            if (!window.confirm(Site.getSingleResource('ChangeStatusOfThisCustomerMsg'))) {
                _me.closeUpdatePopUp();
                return;
            }

            var $popup = $("#UpdateCustStatus");
            var custId = $popup.data("custid");
            var $statusLink = $("#edit-cust-status-" + custId);

            var isSuspended = $("#custStatus-suspended").prop('checked');
            var isClosed = $("#custStatus-closed").prop('checked');
            var url = Site.resolveClientUrl("CustomerAPI/UpdateCustStatus");
            var params = {
                custId: custId,
                isSuspended: isSuspended,
                isClosed: isClosed
            };

            $.ajax({
                type: "POST",
                data: params,
                url: url,
                dataType: "json",
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                    _me.closeUpdatePopUp(custId);
                },
                success: function (result) {
                    var isSuccess = Number(result.Code) === 0;
                    if (isSuccess) {
                        var statusText = isClosed === true ? Site.getSingleResource("Closed") :
                                    isSuspended === true ? Site.getSingleResource("Suspended") : Site.getSingleResource("Open");

                        $statusLink.text(statusText);
                        $statusLink.data("isclosed", isClosed);
                        $statusLink.data("issuspended", isSuspended);
                    }
                    else {
                        alert(result.Message);
                    }
                }
            });
        },

        changeStatus: function () {
            var $popup = $("#UpdateCustStatus");
            var isSuspended = $("#custStatus-suspended").prop('checked');
            var isClosed = $("#custStatus-closed").prop('checked');

            var changed = $popup.data("closed") !== isClosed || $popup.data("suspended") !== isSuspended;
            $("#custStatusUpdate").prop("disabled", !changed);
        }
    };

    return _me;
});