"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service"], function ($, Site, CustomerListState, CustomerListService) {
    var _$specialLevelCell = null;
    var _state;

    var _me = {
        initialize: function () {
            _state = CustomerListState.getState();

            $("#report-content").on("click", ".specialLevel", function () {
                _$specialLevelCell = $(this);
                CustomerListService.showGridDropdownById("LevelListContainer", _$specialLevelCell);
                return false;
            });

            $("#report-content").on("blur", ".specialLevel", function () {
                setTimeout(function () {
                    $("#LevelListContainer").hide();
                }, 300);
            });

            $("#LevelListContainer").on("click", ".levelLink", function () {
                var level = $(this).data("level");
                _me.setValue(level);
            });
        },

        setValue: function (level) {
            var option = {
                dataField: "SpecialLevel",
                popUpSelector: "#LevelListContainer",
                $context: _$specialLevelCell,
                param: {
                    custId: _$specialLevelCell.data("custid"),
                    level: level
                },
                url: Site.resolveClientUrl("CustomerAPI/UpdateSpecialLevel"),
                oldValue: _$specialLevelCell.data("level"),
                newValue: level,
                confirmMessage: _me.getConfirmMessage(level)
            };

            CustomerListService.updateGridData(option);
        },

        getConfirmMessage: function (level) {
            var resourceName = level === 0 ? "AlertRemoveSpecialLevelForCustomerMsg" : "AlertSetSpecialLevelForCustomerMsg";
            return Site.getSingleResource(resourceName);
        }
    };

    return _me;
});