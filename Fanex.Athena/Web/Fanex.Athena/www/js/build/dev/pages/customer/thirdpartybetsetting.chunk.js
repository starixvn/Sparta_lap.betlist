webpackJsonp([37],{

<<<<<<< HEAD
/***/ 131:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(2), __webpack_require__(120), __webpack_require__(146)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup) {
    
    function isInteger(text) {
        return /^\+?\d+$/.test(text);
    }
=======
/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(297);
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

    var _me = {
        showDialogMesage: function (title, message) {
            var options = {
                title: title,
                height: "auto",
                id: "result-update-betsetting",
                onOpen: function () {
                    $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
                },
                onClose: function () {
                    $("#result-update-betsetting").remove();
                    $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                    setTimeout(function () {
                        Site.blockUI();
                        Site.getLocation().reload();
                    }, 500);
                },
                width: 450
            };

            var buttons = [
                {
                    text: Site.getSingleResource("OK"),
                    isClosedOnClick: true,
                    classes: "btn btn-submit"
                }];

            Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
        },

        convertToFloat: function (str) {
            var temp = str.replace(/,/g, "");
            var temp2 = temp.replace(".", "");

            if (isInteger(temp2)) {
                return parseFloat(temp);
            }
            else {
                return NaN;
            }
        },

        showBalloonMsg: function (elementSelector, strMsg) {
            var contents = '<div onclick="hideBalloonMsg(\'' + elementSelector + '\')" style="width:140px;" class="msgerr">' + strMsg + '</div>';

            $(elementSelector).blur(function () { _me.hideBalloonMsg(elementSelector); })
                              .focus()
                              .showBalloon({
                                  contents: contents
                              });
        },
        hideBalloonMsg: function (elementSelector) {
            $(elementSelector).hideBalloon();
        }
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

<<<<<<< HEAD
/***/ 138:
/***/ (function(module, exports, __webpack_require__) {
=======
/***/ 242:
/***/ function(module, exports, __webpack_require__) {
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [], __WEBPACK_AMD_DEFINE_RESULT__ = function () {
    var Product = {
        Sportsbook: 0,
        Racing: 1,
        NumberGame: 2,
        LiveCasino: 4,
        VirtualSports: 5,
        RngCasino: 6,
        Keno: 8,
        AgCasino: 10,
        GoldDeluxe: 11,
        Macau: 12,
        BetTrade: 13,
        MgcLiveCasino: 14,
        Colossusbets: 15,
        MgcRngCasino: 17,
        AllBet: 21,
        VoidBridge: 22,
        FishingWorld: 24,
        Cashout: 25,
        VirtualSports2: 27,
        Lottery: 28
    };

    var _me = {
        Product: Product
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

<<<<<<< HEAD
/***/ 142:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/**
 * jquery.numberformatter - Formatting/Parsing Numbers in jQuery
 *
 * Originally written by
 * Michael Abernethy (mike@abernethysoft.com),
 * Andrew Parry (aparry0@gmail.com)
 *
 * Updated by Sven Busse
 *
 * Dual licensed under the MIT (MIT-LICENSE.txt)
 * and GPL (GPL-LICENSE.txt) licenses.
 *
 * @author Sven Busse
 **/

// Customize to use AMD module
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
    function NumberFormatter() {
        this.defaults = {
            format: "#,###.00",
            locale: "us",
            decimalSeparatorAlwaysShown: false,
            nanForceZero: true,
            round: true,
            isFullLocale: false,
            overrideGroupSep: null,
            overrideDecSep: null,
            overrideNegSign: null,
            isPercentage: false,            // treats the input as a percentage (i.e. input multiplied by 100)
            autoDetectPercentage: true      // will search if the format string ends with '%', if it does then the above option is implicitly set
        };

        var nfLocales = { initialized: false };

        var nfLocalesLikeUS = ['ae', 'ar', 'au', 'ca', 'cn', 'eg', 'gb', 'hk', 'il', 'in', 'jp', 'mx', 'sa', 'sk', 'th', 'tw', 'us', 'za'];
        var nfLocalesLikeDE = ['at', 'be', 'br', 'de', 'dk', 'ee', 'es', 'gr', 'it', 'lt', 'lv', 'nl', 'pt', 'tr', 'vn'];
        var nfLocalesLikeFR = ['bg', 'cz', 'fi', 'fr', 'no', 'pl', 'ro', 'ru', 'se'];
        var nfLocalesLikeCH = ['ch'];

        var nfLocaleFormatting = [[".", ","], [",", "."], [",", " "], [".", "'"]];
        var nfAllLocales = [nfLocalesLikeUS, nfLocalesLikeDE, nfLocalesLikeFR, nfLocalesLikeCH];

        function init() {
            // write the arrays into the hashtable
            for (var localeGroupIdx = 0; localeGroupIdx < nfAllLocales.length; localeGroupIdx++) {
                var localeGroup = nfAllLocales[localeGroupIdx];
                for (var i = 0; i < localeGroup.length; i++) {
                    nfLocales[localeGroup[i]] = localeGroupIdx;
                }
            }

            nfLocales.initialized = true;
        }

        function formatCodes(locale, isFullLocale) {
            if (!nfLocales.initialized)
                init();

            // default values
            var dec = ".";
            var group = ",";
            var neg = "-";

            if (isFullLocale == false) {
                // Extract and convert to lower-case any language code from a real 'locale' formatted string, if not use as-is
                // (To prevent locale format like : "fr_FR", "en_US", "de_DE", "fr_FR", "en-US", "de-DE")
                if (locale.indexOf('_') != -1)
                    locale = locale.split('_')[1].toLowerCase();
                else if (locale.indexOf('-') != -1)
                    locale = locale.split('-')[1].toLowerCase();
            }

            // hashtable lookup to match locale with codes
            var codesIndex = nfLocales[locale];
            if (codesIndex) {
                var codes = nfLocaleFormatting[codesIndex];
                if (codes) {
                    dec = codes[0];
                    group = codes[1];
                }
            }
            return { dec: dec, group: group, neg: neg };
        }

        function _roundNumber(number, decimalPlaces) {
            var power = Math.pow(10, decimalPlaces || 0);
            var value = String(Math.round(number * power) / power);

            // ensure the decimal places are there
            if (decimalPlaces > 0) {
                var dp = value.indexOf(".");
                if (dp == -1) {
                    value += '.';
                    dp = 0;
                } else {
                    dp = value.length - (dp + 1);
                }

                while (dp < decimalPlaces) {
                    value += '0';
                    dp++;
                }
            }
            return value;
        }

        /**
         * Formats a Number object into a string, using the given formatting options
         *
         * @param {Object} numberString
         * @param {Object} options
         */
        function _formatNumber(number, options, suffix, prefix, negativeInFront) {
            options = $.extend({}, this.defaults, options);
            var formatData = formatCodes(options.locale.toLowerCase(), options.isFullLocale);

            var dec = formatData.dec;
            var group = formatData.group;
            var neg = formatData.neg;

            // check overrides
            if (options.overrideGroupSep != null) {
                group = options.overrideGroupSep;
            }
            if (options.overrideDecSep != null) {
                dec = options.overrideDecSep;
            }
            if (options.overrideNegSign != null) {
                neg = options.overrideNegSign;
            }

            // Check NAN handling
            var forcedToZero = false;
            if (isNaN(number)) {
                if (options.nanForceZero == true) {
                    number = 0;
                    forcedToZero = true;
                } else {
                    return '';
                }
            }

            // special case for percentages
            if (options.isPercentage == true || (options.autoDetectPercentage && suffix.charAt(suffix.length - 1) == '%')) {
                number = number * 100;
            }

            var returnString = "";
            if (options.format.indexOf(".") > -1) {
                var decimalPortion = dec;
                var decimalFormat = options.format.substring(options.format.lastIndexOf(".") + 1);

                // round or truncate number as needed
                if (options.round == true)
                    number = Number(_roundNumber(number, decimalFormat.length));
                else {
                    var numStr = number.toString();
                    if (numStr.lastIndexOf('.') > 0) {
                        numStr = numStr.substring(0, numStr.lastIndexOf('.') + decimalFormat.length + 1);
                    }
                    number = Number(numStr);
                }

                var decimalValue = Number(number.toString().substring(number.toString().indexOf('.')));
                var decimalString = String(_roundNumber(decimalValue, decimalFormat.length));
                decimalString = decimalString.substring(decimalString.lastIndexOf('.') + 1);
                for (var i = 0; i < decimalFormat.length; i++) {
                    if (decimalFormat.charAt(i) == '#' && decimalString.charAt(i) != '0') {
                        decimalPortion += decimalString.charAt(i);
                    } else if (decimalFormat.charAt(i) == '#' && decimalString.charAt(i) == '0') {
                        var notParsed = decimalString.substring(i);
                        if (notParsed.match('[1-9]')) {
                            decimalPortion += decimalString.charAt(i);
                        } else
                            break;
                    } else if (decimalFormat.charAt(i) == "0")
                        decimalPortion += decimalString.charAt(i);
                }
                returnString += decimalPortion
            } else
                number = Math.round(number);

            var ones = Math.floor(number);
            if (number < 0)
                ones = Math.ceil(number);

            var onesFormat = "";
            if (options.format.indexOf(".") == -1)
                onesFormat = options.format;
            else
                onesFormat = options.format.substring(0, options.format.indexOf("."));

            var onePortion = "";
            if (!(ones == 0 && onesFormat.substr(onesFormat.length - 1) == '#') || forcedToZero) {
                // find how many digits are in the group
                var oneText = String(Math.abs(ones));
                var groupLength = 9999;
                if (onesFormat.lastIndexOf(",") != -1)
                    groupLength = onesFormat.length - onesFormat.lastIndexOf(",") - 1;
                var groupCount = 0;
                for (i = oneText.length - 1; i > -1; i--) {
                    onePortion = oneText.charAt(i) + onePortion;
                    groupCount++;
                    if (groupCount == groupLength && i != 0) {
                        onePortion = group + onePortion;
                        groupCount = 0;
                    }
                }

                // account for any pre-data padding
                if (onesFormat.length > onePortion.length) {
                    var padStart = onesFormat.indexOf('0');
                    if (padStart != -1) {
                        var padLen = onesFormat.length - padStart;

                        // pad to left with 0's or group char
                        var pos = onesFormat.length - onePortion.length - 1;
                        while (onePortion.length < padLen) {
                            var padChar = onesFormat.charAt(pos);
                            // replace with real group char if needed
                            if (padChar == ',')
                                padChar = group;
                            onePortion = padChar + onePortion;
                            pos--;
                        }
                    }
                }
            }

            if (!onePortion && onesFormat.indexOf('0', onesFormat.length - 1) !== -1)
                onePortion = '0';

            returnString = onePortion + returnString;

            // handle special case where negative is in front of the invalid characters
            if (number < 0 && negativeInFront && prefix.length > 0)
                prefix = neg + prefix;
            else if (number < 0)
                returnString = neg + returnString;

            if (!options.decimalSeparatorAlwaysShown) {
                if (returnString.lastIndexOf(dec) == returnString.length - 1) {
                    returnString = returnString.substring(0, returnString.length - 1);
                }
            }
            returnString = prefix + returnString + suffix;
            return returnString;
        }

        /*  Formatting Methods  */

        /**
         * First parses a string and reformats it with the given options.
         *
         * @param {Object} numberString
         * @param {Object} options
         */
        this.formatNumber = function (numberString, options) {
            options = $.extend({}, this.defaults, options);
            var formatData = formatCodes(options.locale.toLowerCase(), options.isFullLocale);

            var dec = formatData.dec;
            var group = formatData.group;
            var neg = formatData.neg;

            var validFormat = "0#-,.";

            // in the prefix & suffix part of the format string, characters can be escaped
            // by surrounding them in single quotes, so they are not intepreted as formatting placeholders
            var singleQuote = '\'';
            var nextCharIsQuoted = false;

            // strip all the invalid characters at the beginning and the end
            // of the format, and we'll stick them back on at the end
            // make a special case for the negative sign "-" though, so
            // we can have formats like -$23.32
            var prefix = "";
            var negativeInFront = false;
            for (var i = 0; i < options.format.length; i++) {
                if (options.format.charAt(i) == singleQuote) {
                    if (options.format.charAt(i + 1) == singleQuote) {
                        prefix = prefix + singleQuote;
                    }
                    nextCharIsQuoted = !nextCharIsQuoted && options.format.charAt(i + 2) == singleQuote;
                } else if (validFormat.indexOf(options.format.charAt(i)) == -1 || nextCharIsQuoted) {
                    prefix = prefix + options.format.charAt(i);
                } else {
                    if (i == 0 && options.format.charAt(i) == '-') {
                        negativeInFront = true;
                    } else {
                        // end of prefix has been reached, remove it from format string
                        options.format = options.format.substring(i, options.format.length);
                        break;
                    }
                }
            }
            var suffix = "";
            for (i = options.format.length - 1; i >= 0; i--) {
                if (options.format.charAt(i) == singleQuote) {
                    if (options.format.charAt(i - 1) == singleQuote) {
                        suffix = singleQuote + suffix;
                    }
                    nextCharIsQuoted = !nextCharIsQuoted && options.format.charAt(i - 2) == singleQuote;
                } else if (validFormat.indexOf(options.format.charAt(i)) == -1 || nextCharIsQuoted) {
                    suffix = options.format.charAt(i) + suffix;
                } else {
                    // beginning of suffix has been reached, remove it from format string
                    options.format = options.format.substring(0, i + 1);
                    break;
                }
            }

            // now we need to convert it into a number
            //while (numberString.indexOf(group) > -1)
            //  numberString = numberString.replace(group, '');
            //var number = new Number(numberString.replace(dec, ".").replace(neg, "-"));
            var number = Number(numberString);

            return _formatNumber(number, options, suffix, prefix, negativeInFront);
        };
    }

    return NumberFormatter;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__))
=======
/***/ 243:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(9), __webpack_require__(242), __webpack_require__(244), __webpack_require__(245), __webpack_require__(247)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup, Constant, NumberFormatter, BetSettingHelper, BetSettingValidation) {
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
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ },

<<<<<<< HEAD
/***/ 143:
/***/ (function(module, exports, __webpack_require__) {
=======
/***/ 244:
/***/ function(module, exports, __webpack_require__) {
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(2), __webpack_require__(131)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, BetSettingHelper) {
    var SPORTID_NUMBERGAME = 161;

    var _me = {
        checkValidationNumberGame: function () {
            if (!_me.checkLimitMinMaxBet(SPORTID_NUMBERGAME)) {
                return false;
            }

            var maxPerBallSelector = "#max-per-ball-161";
            var maxPerBall = BetSettingHelper.convertToFloat($(maxPerBallSelector).val());
            var maxPerBallLimit = BetSettingHelper.convertToFloat($(maxPerBallSelector).attr('data-max-value'));

            if (isNaN(maxPerBall)) {
                BetSettingHelper.showBalloonMsg(maxPerBallSelector, Site.getSingleResource('IncorrectMaxPerBall'));
                return false;
            }

            if (maxPerBallLimit > 0 && maxPerBall > maxPerBallLimit) {
                BetSettingHelper.showBalloonMsg(maxPerBallSelector, Site.getSingleResource('MaxPerBallLessThanOrEqual') + " " + maxPerBallLimit);
                return false;
            }

            return true;
        },

        checkLimitMinMaxBet: function (sportId) {
            var minBetId = "#min-bet-" + sportId;
            var maxBetId = "#max-bet-" + sportId;
            var maxPerMatchId = "#max-per-match-" + sportId;

            var $minBetTextBox = $(minBetId);
            var $maxBetTextBox = $(maxBetId);
            var $maxPerMatchTextBox = $(maxPerMatchId);
            if ($minBetTextBox.length === 0 ||
                $maxBetTextBox.length === 0 ||
                $maxPerMatchTextBox.length === 0) {
                return true;
            }

            // Get value in attribute
            var limitMinBet = BetSettingHelper.convertToFloat($minBetTextBox.attr('data-min-value'));
            var limitMaxBet = BetSettingHelper.convertToFloat($maxBetTextBox.attr('data-max-value'));
            var limitMaxPerMatch = BetSettingHelper.convertToFloat($maxPerMatchTextBox.attr('data-max-value'));
            var limitMaxPerMatchMin = BetSettingHelper.convertToFloat($maxPerMatchTextBox.attr('data-min-value'));

            var minBet = BetSettingHelper.convertToFloat($minBetTextBox.val());
            var maxBet = BetSettingHelper.convertToFloat($maxBetTextBox.val());
            var maxPerMatcth = BetSettingHelper.convertToFloat($maxPerMatchTextBox.val());

            // Check if minbet, maxbet, maxpermatch is valid number
            if (isNaN(minBet)) {
                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('IncorrectMinBet'));
                return false;
            }
            if (isNaN(maxBet)) {
                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('IncorrectMaxBet'));
                return false;
            }
            if (isNaN(maxPerMatcth)) {
                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('IncorrectMaxPerMatch'));
                return false;
            }

            if (minBet === 0 && maxBet === 0 && maxPerMatcth === 0) { return true; }

            // Check contrains
            // min bet <=maxbet <= max per match
            if (limitMinBet > 0 && minBet < BetSettingHelper.convertToFloat(limitMinBet.toString())) {
                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('MinBetGreaterThanOrEqual') + " " + limitMinBet);
                return false;
            }
            if (limitMaxBet > 0 && maxBet > BetSettingHelper.convertToFloat(limitMaxBet.toString())) {
                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetLessThanOrEqual') + " " + limitMaxBet);
                return false;
            }
            if (limitMaxPerMatchMin > 0 && maxPerMatcth < BetSettingHelper.convertToFloat(limitMaxPerMatchMin.toString())) {
                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchMoreThanOrEqual') + " " + limitMaxPerMatchMin);
                return false;
            }
            if (limitMaxPerMatch > 0 && maxPerMatcth > BetSettingHelper.convertToFloat(limitMaxPerMatch.toString())) {
                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchLessThanOrEqual') + " " + limitMaxPerMatch);
                return false;
            }
            if (minBet > maxBet) {
                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('MinBetCanNotGreaterThanMaxBet'));
                return false;
            }
            if (maxBet > maxPerMatcth) {
                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch'));
                return false;
            }

            return true;
        },

        checkLimitMinMaxBets: function (sportIds) {
            for (var i = 0; i < sportIds.length; i++) {
                if (!_me.checkLimitMinMaxBet(sportIds[i])) {
                    return false;
                }
            }

            return true;
        }
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

<<<<<<< HEAD
/***/ 145:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(2), __webpack_require__(120), __webpack_require__(138), __webpack_require__(142), __webpack_require__(131), __webpack_require__(143)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup, Constant, NumberFormatter, BetSettingHelper, BetSettingValidation) {
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
    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
=======
/***/ 245:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(9), __webpack_require__(246)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup) {
	    
	    function isInteger(text) {
	        return /^\+?\d+$/.test(text);
	    }
	
	    var _me = {
	        showDialogMesage: function (title, message) {
	            var options = {
	                title: title,
	                height: "auto",
	                id: "result-update-betsetting",
	                onOpen: function () {
	                    $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
	                },
	                onClose: function () {
	                    $("#result-update-betsetting").remove();
	                    $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
	                    setTimeout(function () {
	                        Site.blockUI();
	                        Site.getLocation().reload();
	                    }, 500);
	                },
	                width: 450
	            };
	
	            var buttons = [
	                {
	                    text: Site.getSingleResource("OK"),
	                    isClosedOnClick: true,
	                    classes: "btn btn-submit"
	                }];
	
	            Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
	        },
	
	        convertToFloat: function (str) {
	            var temp = str.replace(/,/g, "");
	            var temp2 = temp.replace(".", "");
	
	            if (isInteger(temp2)) {
	                return parseFloat(temp);
	            }
	            else {
	                return NaN;
	            }
	        },
	
	        showBalloonMsg: function (elementSelector, strMsg) {
	            var contents = '<div onclick="hideBalloonMsg(\'' + elementSelector + '\')" style="width:140px;" class="msgerr">' + strMsg + '</div>';
	
	            $(elementSelector).blur(function () { _me.hideBalloonMsg(elementSelector); })
	                              .focus()
	                              .showBalloon({
	                                  contents: contents
	                              });
	        },
	        hideBalloonMsg: function (elementSelector) {
	            $(elementSelector).hideBalloon();
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ },

<<<<<<< HEAD
/***/ 146:
/***/ (function(module, exports, __webpack_require__) {
=======
/***/ 246:
/***/ function(module, exports, __webpack_require__) {
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/* WEBPACK VAR INJECTION */(function(jQuery) {/// <reference path="/@JSense.js" />
/**
* Hover balloon on elements without css and images.
*
* Copyright (c) 2011 Hayato Takenaka
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
* @author: Hayato Takenaka (http://urin.take-uma.net)
* @version: 0.3.0 - 2012/02/25
**/
; (function ($) {
    //-----------------------------------------------------------------------------
    // Private
    //-----------------------------------------------------------------------------
    // Helper for meta programming
    var Meta = {};
    Meta.pos = $.extend(["top", "bottom", "left", "right"], { camel: ["Top", "Bottom", "Left", "Right"] });
    Meta.size = $.extend(["height", "width"], { camel: ["Height", "Width"] });
    Meta.getRelativeNames = function (position) {
        var idx = {
            pos: {
                o: position,                                          // origin
                f: (position % 2 == 0) ? position + 1 : position - 1, // faced
                p1: (position % 2 == 0) ? position : position - 1,
                p2: (position % 2 == 0) ? position + 1 : position,
                c1: (position < 2) ? 2 : 0,
                c2: (position < 2) ? 3 : 1
            },
            size: {
                p: (position < 2) ? 0 : 1, // parallel
                c: (position < 2) ? 1 : 0  // cross
            }
        };
        var names = {};
        for (var m1 in idx) {
            if (!names[m1]) names[m1] = {};
            for (var m2 in idx[m1]) {
                names[m1][m2] = Meta[m1][idx[m1][m2]];
                if (!names.camel) names.camel = {};
                if (!names.camel[m1]) names.camel[m1] = {};
                names.camel[m1][m2] = Meta[m1].camel[idx[m1][m2]];
            }
        }
        names.isTopLeft = (names.pos.o == names.pos.p1);
        return names;
    };

    // Helper class to handle position and size as numerical pixels.
    function NumericalBoxElement() { this.initialize.apply(this, arguments); }
    (function () {
        // Method factories
        var Methods = {
            setBorder: function (pos, isVertical) {
                return function (value) {
                    this.$.css("border-" + pos.toLowerCase() + "-width", value + "px");
                    this["border" + pos] = value;
                    return (this.isActive) ? digitalize(this, isVertical) : this;
                }
            },
            setPosition: function (pos, isVertical) {
                return function (value) {
                    this.$.css(pos.toLowerCase(), value + "px");
                    this[pos.toLowerCase()] = value;
                    return (this.isActive) ? digitalize(this, isVertical) : this;
                }
            }
        };

        NumericalBoxElement.prototype = {
            initialize: function ($element) {
                this.$ = $element;
                $.extend(true, this, this.$.offset(), { center: {}, inner: { center: {}} });
                for (var i = 0; i < Meta.pos.length; i++) {
                    this["border" + Meta.pos.camel[i]] = parseInt(this.$.css("border-" + Meta.pos[i] + "-width")) || 0;
                }
                this.active();
            },
            active: function () { this.isActive = true; digitalize(this); return this; },
            inactive: function () { this.isActive = false; return this; }
        };
        for (var i = 0; i < Meta.pos.length; i++) {
            NumericalBoxElement.prototype["setBorder" + Meta.pos.camel[i]] = Methods.setBorder(Meta.pos.camel[i], (i < 2));
            if (i % 2 == 0)
                NumericalBoxElement.prototype["set" + Meta.pos.camel[i]] = Methods.setPosition(Meta.pos.camel[i], (i < 2));
        }

        function digitalize(box, isVertical) {
            if (isVertical == undefined) { digitalize(box, true); return digitalize(box, false); }
            var m = Meta.getRelativeNames((isVertical) ? 0 : 2);
            box[m.size.p] = box.$["outer" + m.camel.size.p]();
            box[m.pos.f] = box[m.pos.o] + box[m.size.p];
            box.center[m.pos.o] = box[m.pos.o] + box[m.size.p] / 2;
            box.inner[m.pos.o] = box[m.pos.o] + box["border" + m.camel.pos.o];
            box.inner[m.size.p] = box.$["inner" + m.camel.size.p]();
            box.inner[m.pos.f] = box.inner[m.pos.o] + box.inner[m.size.p];
            box.inner.center[m.pos.o] = box.inner[m.pos.f] + box.inner[m.size.p] / 2;
            return box;
        }
    })();

    // Adjust position of balloon body
    function makeupBalloon($target, $balloon, options) {
        $balloon.stop(true, true);
        var outerTip, innerTip,
			initTipStyle = { position: "absolute", height: "0", width: "0", border: "solid 0 transparent" },
			target = new NumericalBoxElement($target),
			balloon = new NumericalBoxElement($balloon);
        balloon.setTop(-options.offsetY
			+ ((options.position && options.position.indexOf("top") >= 0) ? target.top - balloon.height
			: ((options.position && options.position.indexOf("bottom") >= 0) ? target.bottom
			: target.center.top - balloon.height / 2)));
        balloon.setLeft(options.offsetX
			+ ((options.position && options.position.indexOf("left") >= 0) ? target.left - balloon.width
			: ((options.position && options.position.indexOf("right") >= 0) ? target.right
			: target.center.left - balloon.width / 2)));
        if (options.tipSize > 0) {
            // Add hidden balloon tips into balloon body.
            if ($balloon.data("outerTip")) { $balloon.data("outerTip").remove(); $balloon.removeData("outerTip"); }
            if ($balloon.data("innerTip")) { $balloon.data("innerTip").remove(); $balloon.removeData("innerTip"); }
            outerTip = new NumericalBoxElement($("<div>").css(initTipStyle).appendTo($balloon));
            innerTip = new NumericalBoxElement($("<div>").css(initTipStyle).appendTo($balloon));
            // Make tip triangle, adjust position of tips.
            var m;
            for (var i = 0; i < Meta.pos.length; i++) {
                m = Meta.getRelativeNames(i);
                if (balloon.center[m.pos.c1] >= target[m.pos.c1] &&
					balloon.center[m.pos.c1] <= target[m.pos.c2]) {
                    if (i % 2 == 0) {
                        if (balloon[m.pos.o] >= target[m.pos.o] && balloon[m.pos.f] >= target[m.pos.f]) break;
                    } else {
                        if (balloon[m.pos.o] <= target[m.pos.o] && balloon[m.pos.f] <= target[m.pos.f]) break;
                    }
                }
                m = null;
            }
            if (m) {
                balloon["set" + m.camel.pos.p1]
					(balloon[m.pos.p1] + ((m.isTopLeft) ? 1 : -1) * (options.tipSize - balloon["border" + m.camel.pos.o]));
                makeTip(balloon, outerTip, m, options.tipSize, $balloon.css("border-" + m.pos.o + "-color"));
                makeTip(balloon, innerTip, m, options.tipSize - 2 * balloon["border" + m.camel.pos.o], $balloon.css("background-color"));
                $balloon.data("outerTip", outerTip.$).data("innerTip", innerTip.$);
            } else {
                $.each([outerTip.$, innerTip.$], function () { this.remove(); });
            }
        }
        // Make up balloon tip.
        function makeTip(balloon, tip, m, tipSize, color) {
            var len = Math.round(tipSize / 1.7320508);
            tip.inactive()
				["setBorder" + m.camel.pos.f](tipSize)
				["setBorder" + m.camel.pos.c1](len)
				["setBorder" + m.camel.pos.c2](len)
				["set" + m.camel.pos.p1]((m.isTopLeft) ? -tipSize : balloon.inner[m.size.p])
				["set" + m.camel.pos.c1](balloon.inner[m.size.c] / 2 - len)
				.active()
				.$.css("border-" + m.pos.f + "-color", color);
        }
    }

    // True if the event comes from the target or balloon.
    function isValidTargetEvent($target, e) {
        var b = $target.data("balloon") && $target.data("balloon").get(0);
        return !(b && (b == e.relatedTarget || $.contains(b, e.relatedTarget)));
    }

    //-----------------------------------------------------------------------------
    // Public
    //-----------------------------------------------------------------------------
    $.fn.balloon = function (options) {
        options = $.extend(true, {}, $.balloon.defaults, options);
        return this.one("mouseenter", function (e) {
            var $target = $(this), t = this;
            var $balloon = $target.unbind("mouseenter", arguments.callee)
				.showBalloon(options).mouseenter(function (e) {
				    isValidTargetEvent($target, e) && $target.showBalloon();
				}).data("balloon");
            if ($balloon) {
                $balloon.mouseleave(function (e) {
                    if (t == e.relatedTarget || $.contains(t, e.relatedTarget)) return;
                    $target.hideBalloon();
                }).mouseenter(function (e) { $target.showBalloon(); });
            }
        }).mouseleave(function (e) {
            var $target = $(this);
            isValidTargetEvent($target, e) && $target.hideBalloon();
        });
    };

    $.fn.showBalloon = function (options) {
        var $target, $balloon, offTimer;
        if (!$.balloon.defaults.css) $.balloon.defaults.css = {};
        if (options || !this.data("options"))
            this.data("options", $.extend(true, {}, $.balloon.defaults, options));
        options = this.data("options");
        return this.each(function () {
            $target = $(this);
            (offTimer = $target.data("offTimer")) && clearTimeout(offTimer);
            var contents = $.isFunction(options.contents)
				? options.contents()
				: (options.contents || $target.attr("title"));
            var isNew = !($balloon = $target.data("balloon"));
            if (isNew) $balloon = $("<div>").append(contents);
            if (!options.url && (!$balloon || $balloon.html() == "")) return;
            if (!isNew && contents && contents != $balloon.html()) $balloon.empty().append(contents);
            $target.removeAttr("title");
            if (options.url) {
                $balloon.load($.isFunction(options.url) ? options.url(this) : options.url, function (res, sts, xhr) {
                    if (options.ajaxComplete) options.ajaxComplete(res, sts, xhr);
                    makeupBalloon($target, $balloon, options);
                });
            }
            if (isNew) {
                $balloon
					.addClass(options.classname)
					.css(options.css)
					.css({ visibility: "hidden", position: "absolute" })
					.appendTo("body");
                $target.data("balloon", $balloon);
                makeupBalloon($target, $balloon, options);
                $balloon.hide().css("visibility", "visible");
            } else {
                makeupBalloon($target, $balloon, options);
            }
            $target.data("onTimer", setTimeout(function () {
                if (options.showAnimation) {
                    options.showAnimation.apply($balloon.stop(true, true), [options.showDuration]);
                } else {
                    $balloon.show(options.showDuration, function () {
                        if (this.style.removeAttribute) { this.style.removeAttribute("filter"); }
                    });
                }
            }, options.delay));
        });
    };

    $.fn.hideBalloon = function () {
        var options = this.data("options"), onTimer, offTimer;
        return this.each(function () {
            var $target = $(this);
            (onTimer = $target.data("onTimer")) && clearTimeout(onTimer);
            (offTimer = $target.data("offTimer")) && clearTimeout(offTimer);
            $target.data("offTimer", setTimeout(function () {
                var $balloon = $target.data("balloon");
                if (options.hideAnimation) {
                    $balloon && options.hideAnimation.apply($balloon.stop(true, true), [options.hideDuration]);
                } else {
                    $balloon && $balloon.stop(true, true).hide(options.hideDuration);
                }
            },
			options.minLifetime));
        });
    };

    $.balloon = {
        defaults: {
            contents: null, url: null, ajaxComplete: null, classname: null,
            position: "top", offsetX: 0, offsetY: 0, tipSize: 12,
            delay: 0, minLifetime: 200,
            showDuration: 100, showAnimation: null,
            hideDuration: 80, hideAnimation: function (d) { this.fadeOut(d); },
            css: {
                minWidth: "20px",
                padding: "5px",
                borderRadius: "6px",
                border: "solid 1px #777",
                boxShadow: "4px 4px 4px #555",
                color: "#666",
                backgroundColor: "#efefef",
                opacity: ($.support.opacity) ? "0.85" : null,
                zIndex: "32767",
                textAlign: "left"
            }
        }
    };
})(jQuery);
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

<<<<<<< HEAD
/***/ 153:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(2), __webpack_require__(131)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, BetSettingHelper) {
    var _me = {
        update: function (url, postData) {
            $.ajax({
                type: 'POST',
                url: url,
                cache: false,
                data: postData,
                dataType: "json",
                success: function (result) {
                    var messageResource = result.IsSuccessful ? "MessageSuccess" : "Fail";
                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message);
                },
                error: function (request, status, err) {
                    BetSettingHelper.showDialogMesage(Site.getSingleResource("Error"), err);
                }
            });
        },

        getCommonPostData: function ($context) {
            var productId = parseInt($("#ProductId").val());
            var nickName = $context.data("nick-name");
            // update value  (0: enable, 1: disable) is opposed with current value (1: enable, 0: disable)
            var status = $context.data("status");
            var custId = $context.data("cust-id");

            return {
                productId: productId,
                nickname: nickName,
                status: status,
                custid: custId
            };
        }
    };

    return _me;
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
=======
/***/ 247:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(245)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, BetSettingHelper) {
	    var SPORTID_NUMBERGAME = 161;
	
	    var _me = {
	        checkValidationNumberGame: function () {
	            if (!_me.checkLimitMinMaxBet(SPORTID_NUMBERGAME)) {
	                return false;
	            }
	
	            var maxPerBallSelector = "#max-per-ball-161";
	            var maxPerBall = BetSettingHelper.convertToFloat($(maxPerBallSelector).val());
	            var maxPerBallLimit = BetSettingHelper.convertToFloat($(maxPerBallSelector).attr('data-max-value'));
	
	            if (isNaN(maxPerBall)) {
	                BetSettingHelper.showBalloonMsg(maxPerBallSelector, Site.getSingleResource('IncorrectMaxPerBall'));
	                return false;
	            }
	
	            if (maxPerBallLimit > 0 && maxPerBall > maxPerBallLimit) {
	                BetSettingHelper.showBalloonMsg(maxPerBallSelector, Site.getSingleResource('MaxPerBallLessThanOrEqual') + " " + maxPerBallLimit);
	                return false;
	            }
	
	            return true;
	        },
	
	        checkLimitMinMaxBet: function (sportId) {
	            var minBetId = "#min-bet-" + sportId;
	            var maxBetId = "#max-bet-" + sportId;
	            var maxPerMatchId = "#max-per-match-" + sportId;
	
	            var $minBetTextBox = $(minBetId);
	            var $maxBetTextBox = $(maxBetId);
	            var $maxPerMatchTextBox = $(maxPerMatchId);
	            if ($minBetTextBox.length === 0 ||
	                $maxBetTextBox.length === 0 ||
	                $maxPerMatchTextBox.length === 0) {
	                return true;
	            }
	
	            // Get value in attribute
	            var limitMinBet = BetSettingHelper.convertToFloat($minBetTextBox.attr('data-min-value'));
	            var limitMaxBet = BetSettingHelper.convertToFloat($maxBetTextBox.attr('data-max-value'));
	            var limitMaxPerMatch = BetSettingHelper.convertToFloat($maxPerMatchTextBox.attr('data-max-value'));
	            var limitMaxPerMatchMin = BetSettingHelper.convertToFloat($maxPerMatchTextBox.attr('data-min-value'));
	
	            var minBet = BetSettingHelper.convertToFloat($minBetTextBox.val());
	            var maxBet = BetSettingHelper.convertToFloat($maxBetTextBox.val());
	            var maxPerMatcth = BetSettingHelper.convertToFloat($maxPerMatchTextBox.val());
	
	            // Check if minbet, maxbet, maxpermatch is valid number
	            if (isNaN(minBet)) {
	                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('IncorrectMinBet'));
	                return false;
	            }
	            if (isNaN(maxBet)) {
	                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('IncorrectMaxBet'));
	                return false;
	            }
	            if (isNaN(maxPerMatcth)) {
	                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('IncorrectMaxPerMatch'));
	                return false;
	            }
	
	            if (minBet === 0 && maxBet === 0 && maxPerMatcth === 0) { return true; }
	
	            // Check contrains
	            // min bet <=maxbet <= max per match
	            if (limitMinBet > 0 && minBet < BetSettingHelper.convertToFloat(limitMinBet.toString())) {
	                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('MinBetGreaterThanOrEqual') + " " + limitMinBet);
	                return false;
	            }
	            if (limitMaxBet > 0 && maxBet > BetSettingHelper.convertToFloat(limitMaxBet.toString())) {
	                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetLessThanOrEqual') + " " + limitMaxBet);
	                return false;
	            }
	            if (limitMaxPerMatchMin > 0 && maxPerMatcth < BetSettingHelper.convertToFloat(limitMaxPerMatchMin.toString())) {
	                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchMoreThanOrEqual') + " " + limitMaxPerMatchMin);
	                return false;
	            }
	            if (limitMaxPerMatch > 0 && maxPerMatcth > BetSettingHelper.convertToFloat(limitMaxPerMatch.toString())) {
	                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchLessThanOrEqual') + " " + limitMaxPerMatch);
	                return false;
	            }
	            if (minBet > maxBet) {
	                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('MinBetCanNotGreaterThanMaxBet'));
	                return false;
	            }
	            if (maxBet > maxPerMatcth) {
	                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch'));
	                return false;
	            }
	
	            return true;
	        },
	
	        checkLimitMinMaxBets: function (sportIds) {
	            for (var i = 0; i < sportIds.length; i++) {
	                if (!_me.checkLimitMinMaxBet(sportIds[i])) {
	                    return false;
	                }
	            }
	
	            return true;
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ },

<<<<<<< HEAD
/***/ 392:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(393);

=======
/***/ 248:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(245)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, BetSettingHelper) {
	    var _me = {
	        update: function (url, postData) {
	            $.ajax({
	                type: 'POST',
	                url: url,
	                cache: false,
	                data: postData,
	                dataType: "json",
	                success: function (result) {
	                    var messageResource = result.IsSuccessful ? "MessageSuccess" : "Fail";
	                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message);
	                },
	                error: function (request, status, err) {
	                    BetSettingHelper.showDialogMesage(Site.getSingleResource("Error"), err);
	                }
	            });
	        },
	
	        getCommonPostData: function ($context) {
	            var productId = parseInt($("#ProductId").val());
	            var nickName = $context.data("nick-name");
	            // update value  (0: enable, 1: disable) is opposed with current value (1: enable, 0: disable)
	            var status = $context.data("status");
	            var custId = $context.data("cust-id");
	
	            return {
	                productId: productId,
	                nickname: nickName,
	                status: status,
	                custid: custId
	            };
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ },

<<<<<<< HEAD
/***/ 393:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(394)], __WEBPACK_AMD_DEFINE_RESULT__ = function (ThirdPartyBetSetting) {
    $(ThirdPartyBetSetting.initialize);
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))
=======
/***/ 297:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(298)], __WEBPACK_AMD_DEFINE_RESULT__ = function (ThirdPartyBetSetting) {
	    $(ThirdPartyBetSetting.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ },

<<<<<<< HEAD
/***/ 394:
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿/* Macau, MgcLiveCasino, Colossusbets, MgcRngCasino, BetTrade, Cashout */

!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(2), __webpack_require__(138), __webpack_require__(145), __webpack_require__(153)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Constant, BetSettingBase, BetSettingStatus) {
        var Product = Constant.Product;
        var CHECKBOX_SELECTOR = "#enable-product";
        var PRODUCT_CONTAIN_CHECKBOX = [Product.MgcLiveCasino, Product.MgcRngCasino, Product.BetTrade, Product.Cashout];

        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();
                _me.initEvent();
            },

            initEvent: function () {
                $("#btnUpdateStatus").click(function () {
                    _me.updateStatus($(this));
                });

                $("#btnReset").on("click", _me.reset);
            },

            reset: function () {
                var $checkbox = $(CHECKBOX_SELECTOR);
                $checkbox.prop("checked", $checkbox.data("original") === 1);
            },

            updateStatus: function ($context) {
                var productId = parseInt($("#ProductId").val());
                var url = _me.getUrlByProduct(productId);
                if (url === null) {
                    return;
                }

                var params = BetSettingStatus.getCommonPostData($context);

                if (PRODUCT_CONTAIN_CHECKBOX.indexOf(productId) > -1) {
                    params.status = _me.getStatus();
                }

                BetSettingStatus.update(Site.resolveClientUrl(url), params);
            },

            getUrlByProduct: function (productId) {
                var url = null;
                switch (productId) {
                    case Product.Macau:
                        url = "ThirdPartyBetSetting/UpdateMacauStatus";
                        break;
                    case Product.MgcLiveCasino:
                        url = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
                        break;
                    case Product.Colossusbets:
                        url = "ThirdPartyBetSetting/UpdateColossusBetsStatus";
                        break;
                    case Product.MgcRngCasino:
                        url = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
                        break;
                    case Product.BetTrade:
                        url = "ThirdPartyBetSetting/UpdateBetTradeStatus";
                        break;
                    case Product.Cashout:
                        url = "ThirdPartyBetSetting/UpdateCashOutStatus";
                        break;
                }

                return url;
            },

            getStatus: function () {
                return $(CHECKBOX_SELECTOR).is(":checked") ? 1 : 0;
            }
        };

        return _me;
    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
=======
/***/ 298:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* Macau, MgcLiveCasino, Colossusbets, MgcRngCasino, BetTrade, Cashout */
	"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(242), __webpack_require__(243), __webpack_require__(248)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Constant, BetSettingBase, BetSettingStatus) {
	        var Product = Constant.Product;
	        var CHECKBOX_SELECTOR = "#enable-product";
	        var PRODUCT_CONTAIN_CHECKBOX = [Product.MgcLiveCasino, Product.MgcRngCasino, Product.BetTrade, Product.Cashout];
	
	        var _me = {
	            initialize: function () {
	                Site.initialize();
	                BetSettingBase.initialize();
	                _me.initEvent();
	            },
	
	            initEvent: function () {
	                $("#btnUpdateStatus").click(function () {
	                    _me.updateStatus($(this));
	                });
	
	                $("#btnReset").on("click", _me.reset);
	            },
	
	            reset: function () {
	                var $checkbox = $(CHECKBOX_SELECTOR);
	                $checkbox.prop("checked", $checkbox.data("original") === 1);
	            },
	
	            updateStatus: function ($context) {
	                var productId = parseInt($("#ProductId").val());
	                var url = _me.getUrlByProduct(productId);
	                if (url === null) {
	                    return;
	                }
	
	                var params = BetSettingStatus.getCommonPostData($context);
	
	                if (PRODUCT_CONTAIN_CHECKBOX.indexOf(productId) > -1) {
	                    params.status = _me.getStatus();
	                }
	
	                BetSettingStatus.update(Site.resolveClientUrl(url), params);
	            },
	
	            getUrlByProduct: function (productId) {
	                var url = null;
	                switch (productId) {
	                    case Product.Macau:
	                        url = "ThirdPartyBetSetting/UpdateMacauStatus";
	                        break;
	                    case Product.MgcLiveCasino:
	                        url = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
	                        break;
	                    case Product.Colossusbets:
	                        url = "ThirdPartyBetSetting/UpdateColossusBetsStatus";
	                        break;
	                    case Product.MgcRngCasino:
	                        url = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
	                        break;
	                    case Product.BetTrade:
	                        url = "ThirdPartyBetSetting/UpdateBetTradeStatus";
	                        break;
	                    case Product.Cashout:
	                        url = "ThirdPartyBetSetting/UpdateCashOutStatus";
	                        break;
	                }
	
	                return url;
	            },
	
	            getStatus: function () {
	                return $(CHECKBOX_SELECTOR).is(":checked") ? 1 : 0;
	            }
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ }

},[392]);
//# sourceMappingURL=thirdpartybetsetting.chunk.js.map