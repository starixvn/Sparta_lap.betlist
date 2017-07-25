/*global _language_en_US*/
define(["jquery", "common/querystring", "common/popup", "extensions/prototype", "components/selectlist"],  
    function ($, QueryString, Popup) {// jshint ignore:line
        "use strict";
        var BASE_CURRENT_LIST = ["EUR", "USD", "GBP"];
        var REQUEST_VERIFICATION_TOKEN;
        var _historyDate = null;
        var _licenseeId = null;
        var _rootUrl = null;
        var _pageLanguage = null;

        function setProductCodeListParam(url, productCodeList){
            return QueryString.setParam(url, "ProductCodeList", productCodeList);
        }

        function reload() {
            window.location.reload();
        }

        function resolveClientUrl(relativeUrl) {
            return _rootUrl + relativeUrl;
        }

        function setLicenseeOption(urlOrParams, currencyId, currencyName, baseCurrency) {
            currencyName = $.trim(currencyName);
            // Set Url
            if (typeof (urlOrParams) === "string") {
                urlOrParams = QueryString.setParam(urlOrParams, "CurrencyId", currencyId);
                urlOrParams = QueryString.setParam(urlOrParams, "CurrencyName", currencyName);
                urlOrParams = QueryString.setParam(urlOrParams, "BaseCurrency", baseCurrency);
            }
            else {
                urlOrParams.CurrencyId = currencyId;
                urlOrParams.CurrencyName = currencyName;
                urlOrParams.BaseCurrency = baseCurrency;
            }

            return urlOrParams;
        }

        function initSelectElements() {
            $('select.select').not('#CurrencyId').selectList();

            $('select#CurrencyId').selectList({ canSearch: true });
        }

        function initialize() {
            REQUEST_VERIFICATION_TOKEN = $("input[name=__RequestVerificationToken]").val();
            initSelectElements();
            currencyConstraint();
            initEvent();
            _historyDate = new Date($("#HistoryDate").val());
            _rootUrl = $('#RootUrl').val();
            _pageLanguage = $('#PageLanguage').val();
        }
        function initEvent() {
            $("#CurrencyId").change(function () {
                currencyConstraint(true);
            });

            $.ajaxSetup({
                cache: false,
                headers: { '__RequestVerificationToken': REQUEST_VERIFICATION_TOKEN },
                statusCode: {
                    0: unBlockUI(),
                    400: function () {
                        unBlockUI();
                        alert(getSingleResource("InvalidRequestMsg"));
                    },
                    401: function () {
                        window.location.href = resolveClientUrl("Utility/LocationRestriction");
                    },
                    403: function () {
                        unBlockUI();
                        alert(getSingleResource("HttpError_403"));
                    },
                    500: function () {
                        unBlockUI();
                        alert(getSingleResource("ErrorProcessingMsg"));
                    }
                }
            });
        }
        function getHistoryDate() {
            return _historyDate;
        }
        function isBaseCurrency(currencyName) {
            return $.inArray(currencyName.toUpperCase(), BASE_CURRENT_LIST) > -1;
        }
        function addUserGuide(options) {
            var defaultOptions = {
                buttonSelector: "#UserGuide",
                id: "UserGuidePopup"
            };
            options = $.extend(defaultOptions, options);
            options.title = options.title || "";
            var url = resolveClientUrl('UserGuide/Index?resourcename=' + options.resourceName) + "&_" + (new Date().getTime());
            var $popup;
            var isRequesting = false;
            $(options.buttonSelector).click(function () {
                var $userGuidePopup = $('<div style="display: none" ></div>');

                if (!$popup) {
                    if (isRequesting === false) {
                        isRequesting = true;
                        $userGuidePopup.empty().attr('style', '').hide()
                            .load(url, function (response, status) {
                                if (status === "success") {
                                    setTimeout(function () {
                                        Popup.openPopup($userGuidePopup, defaultOptions);
                                        $popup = $("#" + defaultOptions.id);
                                    }, 50);
                                } else {
                                    $userGuidePopup.hide();
                                    isRequesting = false;
                                }
                            });
                    }
                } else {
                    $popup.flatPopup("show");
                }
            });
        }
        function asNull(text) {
            if (text != null) {
                text = text.trim();
                if (text === '') {
                    return null;
                }
            }

            return text;
        }

        function unBlockUI() {
            $('div.block-body').hide();
            $('div.loading').hide();
        }
        function blockUI() {
            $('div.block-body').show();
            $('div.loading').show();
        }

        function setLicenseeOptionParam(urlOrParams) {
            var currencyId = $("#CurrencyId").val();
            var baseCurrency = $("#BaseCurrency").val();

            var $currencyItem = $("#CurrencyId option[value=" + currencyId + "]");
            var currencyName = $currencyItem.length ? $currencyItem.text() : $("#CurrencyName").val();

            return setLicenseeOption(urlOrParams || {}, currencyId, currencyName, baseCurrency);
        }

        function setLicenseeOptionOriginalParam(urlOrParams) {
            var currencyId = $("#CurrencyId").attr('data-original');
            var baseCurrency = $("#BaseCurrency").attr('data-original');

            var $currencyItem = $("#CurrencyId option[value=" + currencyId + "]");
            var currencyName = $currencyItem.length ? $currencyItem.text() : $("#CurrencyName").val();

            return setLicenseeOption(urlOrParams, currencyId, currencyName, baseCurrency);
        }

        function updateOriginalLicenseeOption() {
            var objectIdList = ["#CurrencyId", "#BaseCurrency", "#CurrencyName"];
            $(objectIdList).each(function (i, e) {
                $(e).attr("data-original", $(e).val());
            });
        }

        function setProductOptionParamList(urlOrParams) {
            // Set Url
            if (typeof (urlOrParams) === "string") {
                var counter = 0;
                $(".childCheckbox", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode !== undefined && productCode.length > 0) {
                        urlOrParams = setProductOptionParam(urlOrParams, counter, productCode, $(element).prop("checked"));
                        counter++;
                    }
                });

                return urlOrParams;
            }
            else {
                urlOrParams.ProductOptions = [];
                $(".childCheckbox", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode !== undefined && productCode.length > 0) {
                        urlOrParams.ProductOptions.push({ Code: productCode, Name: productCode, Status: $(element).prop("checked") });
                    }
                });

                return urlOrParams;
            }
        }

        function setProductOptionOriginalParamList(urlOrParams) {
            var productCodeList = [];
            $(".childCheckbox", "#ProductList").each(function (index, element) {
                var productCode = $(element).attr('data-product-code');
                if (productCode && $(element).attr("data-original") === 'true') {
                    productCodeList.push(productCode.toUpperCase());
                }
            });

            if (typeof (urlOrParams) === "string") {
                return QueryString.setParam(urlOrParams, "ProductCodeList", productCodeList);
            }
            else {
                urlOrParams.ProductCodeList = productCodeList.toString();
                return urlOrParams;
            }
        }

        function setProductOptionParam(url, counter, productCode, status) {
            productCode = productCode == null || productCode === undefined ? undefined : productCode.toUpperCase();
            url = QueryString.setParam(url, "ProductOptions[" + counter + "].Code", productCode);
            url = QueryString.setParam(url, "ProductOptions[" + counter + "].Name", productCode);
            url = QueryString.setParam(url, "ProductOptions[" + counter + "].Status", status);

            return url;
        }

        function setDateRangeParam(url) {
            url = QueryString.setParam(url, "FromDate", $("#FromDate").val());
            url = QueryString.setParam(url, "ToDate", $("#ToDate").val());

            return url;
        }

        function setDateRangeOriginalParam(url) {
            url = QueryString.setParam(url, "FromDate", $("#FromDate").attr('data-original'));
            url = QueryString.setParam(url, "ToDate", $("#ToDate").attr('data-original'));

            return url;
        }

        function currencyConstraint(userChange) {
            var currency = Number($("#CurrencyId").val());
            var currencyName = $("#CurrencyId option:selected").text();
            currencyName = $.trim(currencyName);
            $("#CurrencyName").val(currencyName);

            // Handle outside based currency list in some reports (customer wl, balance)
            var $baseCurrency = $("#BaseCurrency");
            var noConvertOption = $("option[value='']", $baseCurrency);
            if (currency === 0 || isBaseCurrency(currencyName)) {
                noConvertOption.remove();
                if (userChange && currency !== 0) {
                    $("option[value='" + currencyName.toLowerCase() + "']", $baseCurrency).prop("selected", true);
                    $baseCurrency.val(currencyName.toLowerCase());
                }
            }
            else {
                if (noConvertOption.length === 0
                    && $baseCurrency.find('option').length > 0) { // make sure $baseCurrency is select object
                    $("<option value='' selected>" + currencyName + "</option>").appendTo($baseCurrency);
                } else {
                    $("option[value='']", $baseCurrency).text(currencyName);
                }
                if (userChange) {
                    $baseCurrency.val("");
                    $("option[value='']", $baseCurrency).text(currencyName);
                }
            }

            // Handle inside based currency list in some reports(licensee wl, wl by product)
            if ($("option[value=-1]", "#CurrencyId").length > 0) {
                var basedCurrencyMap = { "-1": "eur", "-2": "usd", "-3": "gbp" };
                var basedCurrency = basedCurrencyMap[currency.toString()];
                basedCurrency = basedCurrency === undefined ? "" : basedCurrency;
                $baseCurrency.val(basedCurrency);
            }

            $("select#BaseCurrency").selectList();
        }

        function clone(obj) {
            /// <summary>
            ///
            /// The below function will work adequatly for the six simple types I mentioned,
            /// as long as data in the objects and arrays form a tree structure.That is, there isn't
            /// more than one reference to the same data in the object.
            /// Handle the 3 simple types, and null or undefined
            /// </summary>
            /// <param name="obj"></param>
            /// <returns type=""></returns>

            if (null == obj || "object" !== typeof obj) {
                return obj;
            }
            // Handle Date
            if (obj instanceof Date) {
                var date = new Date();
                date.setTime(obj.getTime());
                return date;
            }

            // Handle Array
            if (obj instanceof Array) {
                var array = [];
                for (var i = 0, len = obj.length; i < len; i++) {
                    array[i] = clone(obj[i]);
                }
                return array;
            }

            // Handle Object
            if (obj instanceof Object) {
                var copy = {};
                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr)) {
                        copy[attr] = clone(obj[attr]);
                    }
                }
                return copy;
            }

            throw new Error("Unable to copy obj! Its type isn't supported.");
        }

        function asBool(input) {
            if (typeof input === 'string') {
                return input.toLowerCase() === 'true';
            }

            return !!input;
        }
        function getSingleResource(enghlishName) {
            if (enghlishName == null || enghlishName === undefined) {
                return enghlishName;
            }

            var keyName = "_language_" + _pageLanguage.replace("-", "_");

            var resources = window[keyName];
            if (!resources) {
                resources = _language_en_US; // jshint ignore:line
            }

            var mapResource = function (key) {
                var item = resources[key];
                if (item === undefined) {
                    item = _language_en_US[key]; // jshint ignore:line
                    return item !== undefined ? item : key;
                } else {
                    return item;
                }
            };
            if (enghlishName.indexOf("@") > -1) {
                var keys = enghlishName.match(/\@{1}\[{1}[a-zA-Z\d\_]+\]{1}/g);
                if (keys != null) {
                    var temp = enghlishName;
                    for (var i = 0; i < keys.length; i++) {
                        var realKey = keys[i].replace("@[", "").replace("]", "");
                        temp = temp.replace(keys[i], mapResource(realKey));
                    }
                    return temp;
                }
                return mapResource(enghlishName);
            }
            else {
                return mapResource(enghlishName);
            }
        }

        function getResources(arrayText) {
            if (arrayText == null) {
                return null;
            }
            if (arrayText.constructor !== Array) {
                return null;
            }

            var newArray = [];
            for (var i = 0; i < arrayText.length; i++) {
                newArray.push(getSingleResource(arrayText[i]));
            }
            return newArray;
        }
        function isNumber(text) {
            return !isNaN(parseFloat(text)) && isFinite(text);
        }
        function isInteger(text) {
            return typeof isNumber(text) && parseFloat(text) === parseInt(text, 10) && !isNaN(text);
        }

        function redirect(url) {
            window.location.href = url;
        }

        function getCurrentUrl() {
            return window.location.href;
        }

        function getLocation() {
            return window.location;
        }

        function debounce(func, wait, immediate) {
            var timeout;
            return function () {
                var context = this,
                    args = arguments;
                var later = function () {
                    timeout = null;
                    if (!immediate) {
                        func.apply(context, args);
                    }
                };
                var callNow = immediate && !timeout;
                clearTimeout(timeout);
                timeout = setTimeout(later, wait || 500);
                if (callNow) {
                    func.apply(context, args);
                }
            };
        }

        return {
            initialize: initialize,
            setProductCodeListParam: setProductCodeListParam,
            reload: reload,
            resolveClientUrl: resolveClientUrl,
            getHistoryDate: getHistoryDate,
            isBaseCurrency: isBaseCurrency,
            addUserGuide: addUserGuide,
            asNull: asNull,
            unBlockUI: unBlockUI,
            blockUI: blockUI,
            setLicenseeOptionParam: setLicenseeOptionParam,
            setLicenseeOptionOriginalParam: setLicenseeOptionOriginalParam,
            updateOriginalLicenseeOption: updateOriginalLicenseeOption,
            setProductOptionParamList: setProductOptionParamList,
            setProductOptionOriginalParamList: setProductOptionOriginalParamList,
            setProductOptionParam: setProductOptionParam,
            setDateRangeParam: setDateRangeParam,
            setDateRangeOriginalParam: setDateRangeOriginalParam,
            currencyConstraint: currencyConstraint,
            clone: clone,
            asBool: asBool,
            getSingleResource: getSingleResource,
            getResources: getResources,
            isNumber: isNumber,
            isInteger: isInteger,
            redirect: redirect,
            getCurrentUrl: getCurrentUrl,
            getLocation: getLocation,
            initSelectElements: initSelectElements,
            debounce: debounce
        };
    });