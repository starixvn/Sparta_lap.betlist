"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service"], function ($, Site, CustomerListState, CustomerListService) {
    var _state;

    var _me = {
        initialize: function () {
            _state = CustomerListState.getState();

            $("#report-content").on("change", "input[type=checkbox].baStatus", function () {
                _me.updateBASetting($(this));
            });
        },

        updateBASetting: function ($context) {
            var canUpdateBA = $("#CanUpdateBA").val();
            if (!canUpdateBA) {
                alert($("#CanUpdateBAMessage").val());
                return;
            }

            var data = $context.data("val");
            var confirmMsg = data.isAllowBA ? Site.getSingleResource("AlertTurnOnBAMsg") : Site.getSingleResource("AlertTurnOffBAMsg");
            var currentBAStatus = !data.isAllowBA;

            if (window.confirm(confirmMsg)) {
                var url = Site.resolveClientUrl("CustomerAPI/UpdateBAStatus");
                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    success: function (response) {
                        if (response.IsSuccessful) {
                            var rowIndex = ($context.closest("tr").attr("id") - 1) % _state.pagingOption.pagesize;
                            _state.cachedData.Data[rowIndex].BAStatus = data.isAllowBA;
                            _state.cachedData.Data[rowIndex].OddsGroup = response.OddsGroup;
                            CustomerListService.bindData(_state.cachedData, true);
                        }
                        else {
                            $context.prop("checked", currentBAStatus);
                            alert(response.Message);
                        }
                    },
                    error: function () {
                        $context.prop("checked", currentBAStatus);
                    },
                    complete: function () {
                        Site.unBlockUI();
                    },
                    statusCode:
                    {
                        404: function () {
                            $context.prop("checked", currentBAStatus);
                            alert('Error-Page not found');
                        }
                    }
                });
            }
            else {
                $context.prop("checked", currentBAStatus);
            }
        }
    };

    return _me;
});