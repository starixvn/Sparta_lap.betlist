"use strict";
define(["jquery", "common/site", "../common/constant", "../common/betsettingbase", "../common/betsetting.status", "lib/numberformat"],
    function ($, Site, Constant, BetSettingBase, BetSettingStatus, NumberFormatter) {
        var Product = Constant.Product;

        var _bettingGroupLimitCache;
        var _productId;

        function disableSubmitButtons(isDisabled) {
            var $button = $("#btnSubmitBetSetting, #btnReset").prop('disabled', isDisabled);
            $button.toggleClass("disabled", isDisabled);
        }

        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();

                _bettingGroupLimitCache = JSON.parse($("#BettingLimit").val());

                _productId = parseInt($("#ProductId").val());
                _me.initEvent();

                var $betSettingGroup = $('#betSettingGroup');
                if ($betSettingGroup.length) {
                    if ($betSettingGroup.data("default") === "") {
                        $betSettingGroup.prop("selectedIndex", -1);
                        disableSubmitButtons(true);
                    }
                    $betSettingGroup.change();
                }
            },

            initEvent: function () {
                var formatter = new NumberFormatter();

                $('#betSettingGroup').on('change', function () {
                    var selectedGroupType = this.value;
                    var groupFilter = function (item) {
                        return item.GroupType === selectedGroupType;
                    };
                    var disableSubmit = false;

                    for (var i = 0; i < _bettingGroupLimitCache.length; i++) {
                        var game = _bettingGroupLimitCache[i];
                        var group = game.Groups.filter(groupFilter)[0];
                        if (group) {
                            $("#min-" + game.GameType).html(formatter.formatNumber(group.Min, { format: "#,###" }));
                            $("#max-" + game.GameType).html(formatter.formatNumber(group.Max, { format: "#,###" }));
                        } else {
                            $("#min-" + game.GameType).html("");
                            $("#max-" + game.GameType).html("");
                            disableSubmit = true;
                        }
                    }

                    disableSubmitButtons(disableSubmit);
                });

                $("#btnSubmitBetSetting").on("click", function () {
                    if (!_me.isValid()) {
                        return;
                    }

                    var messageResource = _productId === Product.AgCasino ? 'UpdateAgCasinoBetSettings' :
                                        _productId === Product.GoldDeluxe ? 'UpdateGoldDeluxeBetSettings' : '';
                    BetSettingBase.showComfirm($(this), messageResource, _me.updateBetSetting);
                });

                $("#btnReset").on("click", _me.resetSetting);

                $("#btnUpdateStatus").click(function () {
                    _me.updateStatus($(this));
                });
            },

            resetSetting: function () {
                var $dropdown = $("#betSettingGroup");
                if ($dropdown.data("default") === "") {
                    $dropdown.prop("selectedIndex", -1).change();
                } else {
                    $dropdown.val($dropdown.data("default")).change();
                }
            },

            updateBetSetting: function (custId, userName, nickName) {
                var url = _productId === Product.AgCasino ? 'AgCasinoBetSetting/UpdateBetSetting' :
                                        _productId === Product.GoldDeluxe ? 'GoldDeluxeBetSetting/UpdateBetSetting' : null;

                if (url === null) {
                    return;
                }

                var params = {
                    custid: custId,
                    userName: userName,
                    nickName: nickName,
                    group: $("#betSettingGroup").val()
                };

                BetSettingBase.updateBetSetting(Site.resolveClientUrl(url), params);
            },

            updateStatus: function ($context) {
                var url = _productId === Product.AgCasino ? 'AgCasinoBetSetting/UpdateStatus' :
                                        _productId === Product.GoldDeluxe ? 'GoldDeluxeBetSetting/UpdateStatus' : null;

                if (url === null) {
                    return;
                }

                var params = BetSettingStatus.getCommonPostData($context);
                BetSettingStatus.update(Site.resolveClientUrl(url), params);
            },

            checkApiContainGroup: function (groupType) {
                var groupFilter = function (item) {
                    return item.GroupType === groupType;
                };
                return _bettingGroupLimitCache[0].Groups.filter(groupFilter).length > 0;
            },

            isValid: function () {
                var group = $("#betSettingGroup").val();
                if (!group) {
                    return false;
                }

                if (!_me.checkApiContainGroup(group)) {
                    alert(Site.getSingleResource("AGCasinoNoGroupChosenErrMsg"));
                    return false;
                }

                return true;
            }
        };

        return _me;
    });