"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service"], function ($, Site, CustomerListState, CustomerListService) {
    var _$specialLevelCell = null;
    var _state;

    var _me = {
        initialize: function () {
            _state = CustomerListState.getState();

            $("#report-content").on("click", ".specialLevel", function () {
                _$specialLevelCell = $(this);
                _me.viewList(_$specialLevelCell);
            });

            $("#LevelListContainer .levelLink").click(function () {
                var level = $(this).data("level");
                _me.setValue(_$specialLevelCell, level);
            });
        },

        viewList: function ($context) {
            var custId = $context.attr("data-custid");
            var $popup = $("#LevelListContainer");
            var position = $context.offset();
            $popup.css({
                top: position.top + $context.height(),
                left: position.left
            });

            if ($popup.attr("custid") === custId) {
                $popup.toggle();
            }
            else {
                var $customerOption = $("#cust-option-link-" + custId);
                $popup.attr("custid", custId);
                $popup.attr("externalid", $customerOption.attr("externalid"));
                $popup.attr("systemid", $customerOption.attr("systemid"));
                $popup.show();
            }

            return false;
        },

        setValue: function ($listItem, level) {
            var oldValue = $listItem.data("level");
            if (oldValue === level) {
                _me.close();
                return;
            }

            if (!_me.isConfirmed()) {
                return;
            }

            var custId = $listItem.data("custid");
            $listItem.find(".grid-dropdown-label").html(level === 0 ? Site.getSingleResource("None") : level)
                                                .attr('class', 'level-' + level);
            var param = {
                custId: custId,
                level: level
            };

            $.ajax({
                type: "POST",
                data: param,
                url: Site.resolveClientUrl("CustomerAPI/UpdateSpecialLevel"),
                dataType: "json",
                success: function (data) {
                    if (data.Success) {
                        var rowIndex = ($listItem.closest("tr").attr("id") - 1) % _state.pagingOption.pagesize;
                        _state.cacheData.Data[rowIndex].SpecialLevel = level;
                        CustomerListService.bindData(_state.cacheData, true);
                    }
                    else {
                        $listItem.html(oldValue === 0 ? Site.getSingleResource("None") : oldValue);
                        $listItem.attr('class', 'level-' + oldValue);
                        alert(data.ErrorMessage);
                    }
                },
                error: function () {
                    $listItem.html(oldValue === 0 ? Site.getSingleResource("None") : oldValue);
                    $listItem.attr('class', 'level-' + oldValue);
                },
                complete: function () {
                    _me.close();
                }
            });
        },

        isConfirmed: function () {
            var message = level === 0 ?
                            Site.getSingleResource('AlertRemoveSpecialLevelForCustomerMsg') :
                            Site.getSingleResource('AlertSetSpecialLevelForCustomerMsg');

            return window.confirm(message);
        },

        close: function () {
            $("#LevelListContainer").fadeOut();
        }
    };

    return _me;
});