"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service"], function ($, Site, CustomerListState, CustomerListService) {
    var _$oddsGroupCell;
    var _state;

    var _me = {
        initialize: function () {
            _state = CustomerListState.getState();

            $("#report-content").on("click", ".oddsGroup", function () {
                _$oddsGroupCell = $(this);
                _me.showGridDropdown();
            });

            $("#report-content").on("focusout", ".oddsGroup", function (e) {
                if (!$(e.relatedTarget).is(".oddsGroup") && !$(e.relatedTarget).is(".oddsGroupLink")) {
                    $("#OddsGroupListContainer").hide();
                }
            });

            $("#OddsGroupListContainer").on("click", ".oddsGroupLink", function () {
                var group = $(this).data("group");
                _me.setValue(group);
            });
        },

        showGridDropdown: function () {
            if (_$oddsGroupCell.hasClass("disabled")) {
                return;
            }

            CustomerListService.showGridDropdownById("OddsGroupListContainer", _$oddsGroupCell);
        },

        setValue: function (oddsGroup) {
            var confirmMessage = Site.getSingleResource('AlertSetOddsGroupForCustomerMsg');
            confirmMessage = confirmMessage.replace("{0}", oddsGroup);

            var option = {
                dataField: "OddsGroup",
                popUpSelector: "#OddsGroupListContainer",
                $context: _$oddsGroupCell,
                param: {
                    custId: _$oddsGroupCell.data("custid"),
                    oddsGroup: oddsGroup
                },
                url: Site.resolveClientUrl("CustomerAPI/UpdateOddsGroup"),
                oldValue: _$oddsGroupCell.data("group"),
                newValue: oddsGroup,
                confirmMessage: confirmMessage
            };

            CustomerListService.updateGridData(option);
        }
    };

    return _me;
});