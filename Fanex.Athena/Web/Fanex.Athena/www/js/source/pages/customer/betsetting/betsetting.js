"use strict";
define(["jquery", "common/site", "./constant", "./betsetting.reset", "./betsetting.update", "./betsetting.status", "./betsetting.helper", "lib/numberformat"],
    function ($, Site, Constant, BetSettingReset, BetSettingUpdate, BetSettingStatus, BetSettingHelper, NumberFormatter) {
        var Product = Constant.Product;
        var _formatter = new NumberFormatter();

        var _sportIds = [];

        var _me = {
            initialize: function () {
                Site.initialize();
                var $idList = $("#IDList");
                if ($idList.length > 0) {
                    _sportIds = $idList.val().split(",");
                    BetSettingUpdate.initialize(_sportIds);
                }

                _me.initMenu();
                _me.initEvent();
            },
            initEvent: function () {
                // Copy all sportsbook (sportsbook tab)
                $('#chkCopyAll').click(function () {
                    BetSettingUpdate.toggleCopyAllValue($(this));
                });

                // Bind event
                $("#btnReset").on("click", function () {
                    BetSettingReset.reset();
                });

                $("#btnSubmitBetSetting").on("click", function () {
                    BetSettingUpdate.update.call(this);
                });

                $("#btnUpdateStatus").click(function () {
                    BetSettingStatus.update($(this));
                });

                $("#btnDisableEsports").on("click", _me.updateEsportStatus);

                // Formating number in text box.
                _me.formatInputText();
            },
            formatInputText: function () {
                $("input[type=text]").blur(function () {
                    var inputValue = $(this).val().replace(/,/g, "");
                    var formattedValue = _formatter.formatNumber(inputValue, { format: "#,###,##0.##" });
                    $(this).val(formattedValue);
                });
            },
            // Displaying selected menu tab
            initMenu: function () {
                var productId = parseInt($("#ProductId").val());
                var editable = $("#editable").val();
                var enable = !editable || editable === "True";
                $("#product-dropdown").on('change', function (event) {
                    var url = $(this).find("option:selected").data("href");
                    Site.blockUI();
                    Site.redirect(url);
                });
                _me.checkEnableBetSetting(productId, enable);
            },

            checkEnableBetSetting: function (currentProduct, enable) {
                if (currentProduct === Product.LiveCasino) {
                    _me.checkEnableLiveCasino(enable);
                    return;
                }

                _me.checkEnable(enable);

                if (currentProduct === Product.NumberGame) {
                    $("#max-per-ball-161").prop('disabled', !enable);
                } else if (currentProduct === Product.Sportsbook) {
                    $('#copy-settting').toggle(enable);
                }
            },

            checkEnableLiveCasino: function (enable) {
                var gameTypeCount = $("#gameTypeCount").val();
                if (!enable) {
                    for (var i = 1; i <= gameTypeCount; i++) {
                        $("input[name=gameTypeName" + i + "]").prop('disabled', true);
                    }
                    $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
                }
            },

            checkEnable: function (enable) {
                for (var i = 0; i < _sportIds.length; i++) {
                    var sportId = _sportIds[i];
                    var selector = "#min-bet-" + sportId + "," + "#max-bet-" + sportId + "," + "#max-per-match-" + sportId;
                    $(selector).prop("disabled", !enable);
                };

                if (!enable) {
                    $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
                }
            },

            updateEsportStatus: function () {
                var currentStatus = Site.asBool($(this).data("current-status"));
                var eSportText = $("#esport-text-from-remotingref").val();
                var custId = $(this).data("cust-id");
                var url = Site.resolveClientUrl("SportbooksBetSetting/UpdateEsportStatus");
                $.ajax({
                    type: 'POST',
                    url: url,
                    cache: false,
                    data: {
                        isDisabled: !currentStatus,
                        custid: custId
                    },
                    dataType: "json",
                    success: function (result) {
                        var isSuccess = result.Code.toString() === "0";

                        var messageResource = isSuccess ? "MessageSuccess" : "Fail";
                        BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message, false);

                        if (isSuccess) {
                            $("#btnDisableEsports").attr("data-current-status", !currentStatus);
                            $("#btnDisableEsports").val(Site.getSingleResource(!currentStatus ? 'Enable' : 'Disable') + " " + eSportText);
                        }
                    },
                    error: function (request, status, err) {
                        BetSettingHelper.showDialogMesage(Site.getSingleResource("Error"), err, false);
                    }
                });
            }
        };

        return _me;
    });