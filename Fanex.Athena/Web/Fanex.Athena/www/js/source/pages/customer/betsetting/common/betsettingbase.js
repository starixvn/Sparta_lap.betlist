"use strict";
define(["jquery", "common/site", "common/popup", "./constant", "lib/numberformat", "./betsetting.helper", "./betsetting.validation"],
    function ($, Site, Popup, Constant, NumberFormatter, BetSettingHelper, BetSettingValidation) {
        var Product = Constant.Product;

        var _me = {
            initialize: function () {
                $("#product-dropdown").on('change', function () {
                    var url = $(this).find("option:selected").data("href");
                    Site.blockUI();
                    Site.redirect(url);
                });
            },

            getSportIds: function () {
                var $idList = $("#IDList");
                return $idList.length ? $idList.val().split(",") : [];
            },

            setEnable: function () {
                var editable = $("#editable").val();
                var isEnable = !editable || editable === "True";

                if (!isEnable) {
                    var sportIds = _me.getSportIds();

                    for (var i = 0; i < sportIds.length; i++) {
                        var sportId = sportIds[i];
                        var selector = "#min-bet-" + sportId + "," + "#max-bet-" + sportId + "," + "#max-per-match-" + sportId;
                        $(selector).prop("disabled", true);
                    }
                }
            },

            formatInputText: function () {
                var formatter = new NumberFormatter();

                $("input[type=text]").blur(function () {
                    var inputValue = $(this).val().replace(/,/g, "");
                    var formattedValue = formatter.formatNumber(inputValue, { format: "#,###,##0.##" });
                    $(this).val(formattedValue);
                });
            },

            resetInput: function () {
                $('#chkCopyAll').removeAttr('checked');
                $('input[id^="min"], input[id^="max-bet"], input[id^="max-per"]').each(function () {
                    $(this).val($(this).attr('data-original')).prop('disabled', false);
                });
            },

            showComfirm: function ($target, resourceMessage, callback) {
                var productId = parseInt($("#ProductId").val());
                var isValid = productId === Product.LiveCasino ? true :
                             productId === Product.NumberGame ? BetSettingValidation.checkValidationNumberGame() :
                                                                BetSettingValidation.checkLimitMinMaxBets(_me.getSportIds());
                if (!isValid) {
                    return;
                }

                var custId = $target.attr("data-cust-id");
                var userName = $target.attr("data-user-name");
                var nickName = $target.attr("data-nick-name");

                _me.showConfirmAction(Site.getSingleResource(resourceMessage), custId, userName, nickName, callback);
            },

            showConfirmAction: function (message, custId, userName, nickName, callback) {
                var options = {
                    title: Site.getSingleResource("Confirmation"),
                    height: "auto",
                    id: "confirm-update-betsetting",
                    onOpen: function () {
                        $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
                    },
                    onClose: function () {
                        $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                        $("#confirm-update-betsetting").remove();
                    },
                    width: 450
                };

                var buttons = [
                    {
                        text: Site.getSingleResource("OK"),
                        isClosedOnClick: true,
                        click: function () {
                            callback(custId, userName, nickName);
                        },
                        classes: "btn btn-submit"
                    }, {
                        text: Site.getSingleResource("Cancel"),
                        isClosedOnClick: true,
                        classes: "btn btn-cancel"
                    }];

                Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
            },

            getBetSettingParamsForSports: function () {
                var params = {};
                var sportIds = _me.getSportIds();
                for (var i = 0; i < sportIds.length; i++) {
                    $.extend(params, _me.getBetSettingParams(sportIds[i]));
                }

                return params;
            },

            getBetSettingParams: function (sportId) {
                var $minbet = $("#min-bet-" + sportId);
                var $maxbet = $("#max-bet-" + sportId);
                var $maxPerMatch = $("#max-per-match-" + sportId);

                if ($minbet.length === 0 ||
                    $maxbet.length === 0 ||
                    $maxPerMatch.length === 0) {
                    return "";
                }

                var isUpdate = true;
                var originalMinBet = $('#min-bet-' + sportId).attr('data-original');
                var originalMaxBet = $('#max-bet-' + sportId).attr('data-original');
                var originalMaxPerMatch = $('#max-per-match-' + sportId).attr('data-original');

                // Check if data is changed
                if ($minbet.val() === originalMinBet
                    && $maxbet.val() === originalMaxBet
                    && $maxPerMatch.val() === originalMaxPerMatch) {
                    isUpdate = false;
                }

                // Data is changed, update it
                var minBet = BetSettingHelper.convertToFloat($minbet.val());
                var maxBet = BetSettingHelper.convertToFloat($maxbet.val());
                var maxPerMatcth = BetSettingHelper.convertToFloat($maxPerMatch.val());
                var betType = $("#bet-type-" + sportId).val();

                var result = {};
                result["minbet" + sportId] = minBet;
                result["maxbet" + sportId] = maxBet;
                result["maxpermatch" + sportId] = maxPerMatcth;
                result["update" + sportId] = isUpdate;
                result["bettype" + sportId] = betType;

                return result;
            },

            updateBetSetting: function (url, params) {
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: url,
                    data: params,
                    dataType: "json",
                    success: function (result) {
                        var messageResource = Number(result.Code) === 0 ? "MessageSuccess" : "Fail";
                        BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message);
                    }
                });
            }
        };

        return _me;
    });