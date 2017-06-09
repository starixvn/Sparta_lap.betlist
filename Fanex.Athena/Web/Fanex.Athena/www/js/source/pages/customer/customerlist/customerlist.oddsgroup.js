"use strict";
define(["jquery", "common/site", "./customerlist.state", "./customerlist.service"], function ($, Site, CustomerListState, CustomerListService) {
    var _$oddsGroupCell;
    var _state;

    var _me = {
        initialize: function () {
            _state = CustomerListState.getState();

            $("#report-content").on("click", ".oddsGroup", function () {
                _$oddsGroupCell = $(this);
                _me.viewList(_$oddsGroupCell);
            });

            $("#OddsGroupListContainer .oddsGroupLink").click(function () {
                var group = $(this).data("group");
                _me.setValue(_$oddsGroupCell, group);
            });
        },

        viewList: function ($context) {
            if ($context.hasClass("disabled")) {
                return;
            }

            var custId = $context.attr("data-custid");
            var $popup = $("#OddsGroupListContainer");
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

        setValue: function ($listItem, oddsGroup) {
            var oldValue = $listItem.data("group");
            if (oldValue === oddsGroup) {
                _me.close();
                return;
            }

            var message = Site.getSingleResource('AlertSetOddsGroupForCustomerMsg');
            message = message.replace("{0}", oddsGroup);
            if (!confirm(message)) {
                return;
            }

            var custId = $listItem.data("custid");
            $listItem.find(".grid-dropdown-label").html(oddsGroup === 0 ? Site.getSingleResource("None") : oddsGroup)
                                                    .attr('class', 'group-' + oddsGroup);

            var url = Site.resolveClientUrl("CustomerAPI/UpdateOddsGroup");
            var param = {
                custId: custId,
                oddsGroup: oddsGroup
            };

            $.ajax({
                type: "POST",
                data: param,
                url: url,
                dataType: "json",
                success: function (data) {
                    if (data.Success) {
                        var rowIndex = ($listItem.closest("tr").attr("id") - 1) % _pagingOption.pagesize;
                        _state.cacheData.Data[rowIndex].OddsGroup = oddsGroup;
                        CustomerService.bindData(_state.cacheData, true);
                    }
                    else {
                        $listItem.find(".grid-dropdown-label").html(oldValue)
                            .attr('class', 'group-' + oldValue);
                        alert(data.ErrorMessage);
                    }
                    closeOddsGroupList();
                },
                error: function () {
                    $listItem.find(".grid-dropdown-label").html(oldValue)
                            .attr('class', 'group-' + oldValue);
                    _me.close();
                }
            });
        },

        close: function () {
            $("#OddsGroupListContainer").fadeOut();
        }
    };

    return _me;
});