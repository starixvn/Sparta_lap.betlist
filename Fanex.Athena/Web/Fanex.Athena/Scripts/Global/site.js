/*global QueryString, _language_en_US, _page*/
/// <reference path="/@JSense.js" />
/* Preset */
// root JSON object
"use strict";
var Site = (function ($) {
    var BASE_CURRENT_LIST = ["EUR", "USD", "GBP"];
    var REQUEST_VERIFICATION_TOKEN;
    var _historyDate = null;
    var _licenseeId = null;

    function setLicenseeOption(urlOrParams, licenseeType, currencyId, currencyName, baseCurrency) {
        licenseeType = licenseeType === undefined ? "" : licenseeType;
        currencyName = $.trim(currencyName);
        // Set Url
        if (typeof (urlOrParams) === "string") {
            urlOrParams = QueryString.setParam(urlOrParams, "LicenseeType", licenseeType);
            urlOrParams = QueryString.setParam(urlOrParams, "CurrencyId", currencyId);
            urlOrParams = QueryString.setParam(urlOrParams, "CurrencyName", currencyName);
            urlOrParams = QueryString.setParam(urlOrParams, "BaseCurrency", baseCurrency);
        }
        else {
            urlOrParams.LicenseeType = licenseeType;
            urlOrParams.CurrencyId = currencyId;
            urlOrParams.CurrencyName = currencyName;
            urlOrParams.BaseCurrency = baseCurrency;
        }

        return urlOrParams;
    }

    function getDifferenceMonth(fromDate, toDate) {
        return toDate.getMonth() - fromDate.getMonth() + (12 * (toDate.getFullYear() - fromDate.getFullYear())) + 1;
    }

    return {
        isLoadedNotificationPopup: false,
        // A flag used to determine whether browser need to be overridden innerHeight/innerWidth or not (ex: IE8 in low resolution)
        needOverrideInnerSize: false,
        initialize: function () {
            REQUEST_VERIFICATION_TOKEN = $("input[name=__RequestVerificationToken]").val();
            Site.currencyConstraint();
            Site.initEvent();
            _historyDate = new Date($("#HistoryDate").val());
        },
        initEvent: function () {
            $("#CurrencyId").change(function () {
                Site.currencyConstraint(true);
            });

            $.ajaxSetup({
                cache: false,
                data: {
                    __RequestVerificationToken: REQUEST_VERIFICATION_TOKEN
                },
                statusCode: {
                    0: function () {
                        Site.unBlockUI();
                    },
                    401: function (/*jqXHR, textStatus, errorThrown*/) {
                        window.location.href = Site.resolveClientUrl("Utility/LocationRestriction");
                    },
                    500: function () {
                        Site.unBlockUI();
                        alert(Site.getSingleResource("ErrorProcessingMsg"));
                    }
                }
            });
        },
        setConstraintMonthRange: function (monthRange, url) {
            var errorMessage = Site.getSingleResource("DateRangeExceedMsg").replace("{0}", monthRange);

            function isValidRange() {
                var fromDate = new Date($("#FromDate").val());
                var toDate = new Date($("#ToDate").val());

                return getDifferenceMonth(fromDate, toDate) <= monthRange;
            }

            if (url && typeof url === "string") {
                $(document).ajaxSend(function (event, jqxhr, settings) {
                    if (settings.url.search(new RegExp(url, "i")) > -1 && isValidRange() === false) {
                        Site.showMessaegBox(errorMessage);
                        jqxhr.abort();
                    }
                });
            }

            $("form").submit(function () {
                if (isValidRange() === false) {
                    Site.showMessaegBox(errorMessage);
                    return false;
                }

                return true;
            });
        },
        showMessaegBox: function (message) {
            $('<div title="' + Site.getSingleResource("LrfNotification") + '">' + message + '!</div>').dialog();
        },
        getLicenseeId: function () {
            if (!_licenseeId) {
                _licenseeId = parseInt($("#LicenseeId").val());
            }

            return _licenseeId;
        },
        getCurrencyName: function (currencyId) {
            return Site.userCurrencies[currencyId];
        },
        getHistoryDate: function () {
            return _historyDate;
        },
        isBaseCurrency: function (currencyName) {
            return $.inArray(currencyName.toUpperCase(), BASE_CURRENT_LIST) > -1;
        },
        addUserGuide: function (options) {
            var defaultOptions = {
                buttonSelector: "#UserGuide"
            };
            options = $.extend(defaultOptions, options);
            options.title = options.title || "";
            $("body").prepend('<div id="UserGuidePopup" style="display: none" ></div>');
            var url = Site.resolveClientUrl('UserGuide/Index?resourcename=' + options.resourceName) + "&_" + (new Date().getTime());
            var popup;
            $(options.buttonSelector).click(function () {
                var $userGuidePopup = $("#UserGuidePopup");
                if ($userGuidePopup.is(":visible")) {
                    return;
                }

                if (!popup) {
                    $userGuidePopup.empty().attr('style', '').hide()
                        .load(url, function (response, status) {
                            if (status === "success") {
                                setTimeout(function () {
                                    popup = $userGuidePopup.flatPopup(options);
                                    popup.show();
                                }, 50);
                            } else {
                                $userGuidePopup.hide();
                            }
                        });
                } else {
                    popup.show();
                }
            });
        },
        overrideInnerSize: function () {
            if (Site.needOverrideInnerSize) {
                parent.innerHeight = parent.document.documentElement.clientHeight;
                window.innerHeight = window.document.documentElement.clientHeight;
                parent.innerWidth = parent.document.documentElement.clientWidth;
                window.innerWidth = window.document.documentElement.clientWidth;
            }
        },
        addFilterCollapseExpand: function (containerSelector) {
            if (!containerSelector) {
                containerSelector = ".content-filter:has(form)";
            }

            var $handler = $("<div class='filter-handle filter-handle-collapse'></div>");
            var $container = $(containerSelector);

            $container.append($handler);

            var originalHeight = $container.height();
            var originalTop = originalHeight - 15;
            var isCollapsed = false;
            $handler.css("top", originalTop);
            $handler.click(function () {
                isCollapsed = !isCollapsed;
                $container.find("*").toggle();
                $container.height(isCollapsed ? 5 : originalHeight + 5);
                $handler.css("top", isCollapsed ? -15 : originalTop);
                $(this).show().toggleClass("filter-handle-collapse filter-handle-expand");
                // $container.css("background", isCollapsed ? "#E7E7E7" : "").addClass("content-filter");
                $(window).resize();
            });
        },
        asNull: function (text) {
            if (text != null) {
                text = text.trim();
                if (text === '') {
                    return null;
                }
            }

            return text;
        },

        gotoErrorPage: function (status) {
            if (status === undefined || status !== 0) {
                window.location.href = Site.resolveClientUrl("Utility/Error");
            } else {
                Site.unBlockUI();
            }
        },

        unBlockUI: function () {
            $('div.block-body').hide();
            $('div.loading').hide();
            try {
                $.unblockUI();
            } catch (err) {
            }
        },
        blockUI: function () {
            $('div.block-body').show();
            $('div.loading').show();
            try {
                $.blockUI({
                    message: "",
                    timeout: 30000
                });
            }
            catch (err) {
            }
        },

        setLicenseeOptionParam: function (urlOrParams) {
            var licenseeType = $("#LicenseeType").val();
            var currencyId = $("#CurrencyId").val();
            var baseCurrency = $("#BaseCurrency").val();
            var currencyName = $("#CurrencyId option[value=" + currencyId + "]").text();
            if (currencyName === undefined) {
                currencyName = $("#CurrencyName").val();
            }

            return setLicenseeOption(urlOrParams, licenseeType, currencyId, currencyName, baseCurrency);
        },

        setLicenseeOptionOriginalParam: function (urlOrParams) {
            var licenseeType = $("#LicenseeType").attr('data-original');
            var currencyId = $("#CurrencyId").attr('data-original');
            var baseCurrency = $("#BaseCurrency").attr('data-original');
            var currencyName = $("#CurrencyId option[value=" + currencyId + "]").text();
            if (currencyName === undefined) {
                currencyName = $("#CurrencyName").val();
            }
            return setLicenseeOption(urlOrParams, licenseeType, currencyId, currencyName, baseCurrency);
        },

        updateOriginalLicenseeOption: function () {
            var objectIdList = ["#LicenseeType", "#CurrencyId", "#BaseCurrency", "#CurrencyName"];
            $(objectIdList).each(function (i, e) {
                $(e).attr("data-original", $(e).val());
            });
        },

        setProductOptionParamList: function (urlOrParams) {
            // Set Url
            if (typeof (urlOrParams) === "string") {
                var counter = 0;
                $(".childCheckbox", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode !== undefined && productCode.length > 0) {
                        urlOrParams = Site.setProductOptionParam(urlOrParams, counter, productCode, $(element).prop("checked"));
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
        },

        setProductOptionOriginalParamList: function (urlOrParams) {
            if (typeof (urlOrParams) === "string") {
                var counter = 0;
                $(".childCheckbox", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode !== undefined && productCode.length > 0) {
                        urlOrParams = Site.setProductOptionParam(urlOrParams, counter, productCode, $(element).attr("data-original"));
                        counter++;
                    }
                });

                return urlOrParams;
            }
            else {
                urlOrParams.ProductOptions = [];
                $(".childCheckbox']", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode !== undefined && productCode.length > 0) {
                        urlOrParams.ProductOptions.push({ Code: productCode, Name: productCode, Status: $(element).attr("data-original") });
                    }
                });

                return urlOrParams;
            }
        },

        setProductOptionParam: function (url, counter, productCode, status) {
            productCode = productCode == null || productCode === undefined ? undefined : productCode.toUpperCase();
            url = QueryString.setParam(url, "ProductOptions[" + counter + "].Code", productCode);
            url = QueryString.setParam(url, "ProductOptions[" + counter + "].Name", productCode);
            url = QueryString.setParam(url, "ProductOptions[" + counter + "].Status", status);

            return url;
        },

        setDateRangeParam: function (url) {
            url = QueryString.setParam(url, "FromDate", $("#FromDate").val());
            url = QueryString.setParam(url, "ToDate", $("#ToDate").val());

            return url;
        },

        setDateRangeOriginalParam: function (url) {
            url = QueryString.setParam(url, "FromDate", $("#FromDate").attr('data-original'));
            url = QueryString.setParam(url, "ToDate", $("#ToDate").attr('data-original'));

            return url;
        },

        initBlockUI: function () {
            try {
                $(".blockui").click(function () {
                    Site.blockUI();
                });

                Site.unBlockUI();
            } catch (err) {
            }
        },

        currencyConstraint: function (userChange) {
            var currency = Number($("#CurrencyId").val());
            var currencyName = $("#CurrencyId option:selected").text();
            currencyName = $.trim(currencyName);
            $("#CurrencyName").val(currencyName);

            // Handle outside based currency list in some reports (customer wl, balance)
            var baseCurrencyObj = $("#BaseCurrency");
            var noConvertOption = $("option[value='']", baseCurrencyObj);
            if (currency === 0 || Site.isBaseCurrency(currencyName)) {
                noConvertOption.remove();
                if (userChange) {
                    $("option[value='" + currencyName.toLowerCase() + "']", baseCurrencyObj).attr("selected", "selected");
                    baseCurrencyObj.val(currencyName.toLowerCase());
                }
            }
            else {
                if (noConvertOption.length === 0
                    && baseCurrencyObj.find('option').length > 0) { // make sure baseCurrencyObj is select object
                    $("<option value=''selected=selected>" + currencyName + "</option>").appendTo(baseCurrencyObj);
                } else {
                    $("option[value='']", baseCurrencyObj).text(currencyName);
                }
                if (userChange) {
                    baseCurrencyObj.val("");
                }
            }

            // Handle inside based currency list in some reports(licensee wl, wl by product)
            if ($("option[value=-1]", "#CurrencyId").length > 0) {
                var basedCurrencyMap = { "-1": "eur", "-2": "usd", "-3": "gbp" };
                var basedCurrency = basedCurrencyMap[currency.toString()];
                basedCurrency = basedCurrency === undefined ? "" : basedCurrency;
                $("#BaseCurrency").val(basedCurrency);
            }
        },

        sortReport: function (sortColumn) {
            var sortDirection = $("#SortColumn").val() === sortColumn ? $("#SortDirection").val() === "DESC" ? "ASC" : "DESC" : "ASC";

            $("#SortColumn").val(sortColumn);
            $("#SortDirection").val(sortDirection);

            var currencyName = $("#CurrencyId option:selected").text();
            currencyName = $.trim(currencyName);
            $("#CurrencyName").val(currencyName);
        },

        createSortingIcon: function (sortHandler) {
            if ($("#RowCount").val() <= 1) {
                $("#header-content .sort-heading").removeClass("sort-heading");
            }
            else {
                $("#header-content .sort-heading").each(function (index, element) {
                    var headerDiv = $(element);
                    var sortColumn = headerDiv.attr("data-column-name");
                    if (sortColumn === $("#SortColumn").val()) {
                        headerDiv.addClass("sorted-heading");

                        var sortClass = $("#SortDirection").val() === "DESC" ? "sort-desc" : "sort-asc";
                        var sortSpan = $("<span></span");
                        sortSpan.addClass(sortClass);
                        var left = 0;
                        var top = 0;
                        if (headerDiv.hasClass("row2")) {
                            top = (headerDiv.height() - sortSpan.height()) / 2;
                            sortSpan.css({
                                top: top,
                                right: 0
                            });
                        }
                        else if ($(".grid-header-two-line", headerDiv).length > 0) {
                            left = (headerDiv.width() - sortSpan.width()) / 2;
                            sortSpan.css({
                                top: 19,
                                right: 0
                            });
                        }
                        else {
                            left = (headerDiv.width() - sortSpan.width()) / 2 + 35;
                            sortSpan.css({
                                top: 19,
                                right: 0
                            });
                        }

                        sortSpan.appendTo(headerDiv);
                    }

                    $(element).click(function () {
                        Site.sortReport($(element).attr("data-column-name"));
                        sortHandler($(element).attr("data-column-name"));
                    });
                });
            }
        },

        detectBrowser: function (userAgent) {
            if (userAgent === undefined) { userAgent = navigator.userAgent; }
            userAgent = userAgent.toLowerCase();
            var match = /(chrome)[ \/]([\w.]+)/.exec(userAgent) || /(webkit)[ \/]([\w.]+)/.exec(userAgent) ||
                    /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(userAgent) || /(msie) ([\w.]+)/.exec(userAgent) ||
                     userAgent.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(userAgent) || [];

            return {
                browser: match[1] || "",
                version: match[2] || "0"
            };
        },

        ///
        getMaxHeight: function (element) {
            /// <summary>
            ///
            /// </summary>
            /// <param name="element"></param>
            var height = 0;

            var title = 20;
            var footer = 20;
            var headerTitle = 26;
            var headerFilter = $("div.content-filter").outerHeight();
            var exHeight = footer + headerTitle + headerFilter + title;

            var body = window.document.body;
            if (window.innerHeight) {
                height = window.innerHeight;
            } else if (body.parentElement.clientHeight) {
                height = body.parentElement.clientHeight;
            } else if (body && body.clientHeight) {
                height = body.clientHeight;
            }
            var offset = element.offset();
            return height - offset.top - exHeight;
        },

        getMaxWidth: function () {
            /// <summary>
            ///
            /// </summary>
            /// <param name="element"></param>
            /// <returns type=""></returns>
            var width = 0;
            var marginRight = 5;

            var body = window.document.body;
            if (window.innerWidth) {
                width = window.innerWidth;
            } else if (body.parentElement.clientWidth) {
                width = body.parentElement.clientWidth;
            } else if (body && body.clientWidth) {
                width = body.clientWidth;
            }

            return width - marginRight;
        },

        clone: function (obj) {
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
                    array[i] = Site.clone(obj[i]);
                }
                return array;
            }

            // Handle Object
            if (obj instanceof Object) {
                var copy = {};
                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr)) {
                        copy[attr] = Site.clone(obj[attr]);
                    }
                }
                return copy;
            }

            throw new Error("Unable to copy obj! Its type isn't supported.");
        },

        asBool: function (input) {
            /// <summary>
            /// Cast a string to bool
            /// </summary>
            /// <param name="input">string or object</param>
            /// <returns type="">bool type</returns>
            if (typeof input === 'string') {
                return input.toLowerCase() === 'true';
            }

            return !!input;
        },
        getSingleResource: function (enghlishName) {
            /// <summary>
            /// convert text to orther language
            /// </summary>
            /// <param name="enghlishName"></param>

            if (enghlishName == null || enghlishName === undefined) {
                return enghlishName;
            }

            var keyName = "_language_" + _page.lang.replace("-", "_");

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
        },
        getResources: function (arrayText) {
            /// <summary>
            /// convert element-text in array to other language
            /// </summary>
            /// <param name="arrayText">array text</param>

            if (arrayText == null) {
                return null;
            }
            if (arrayText.constructor !== Array) {
                return null;
            }

            var newArray = [];
            for (var i = 0; i < arrayText.length; i++) {
                newArray.push(Site.getSingleResource(arrayText[i]));
            }
            return newArray;
        },
        isNumber: function (text) {
            return !isNaN(parseFloat(text)) && isFinite(text);
        },
        isInteger: function (text) {
            return typeof Site.isNumber(text) && parseFloat(text) === parseInt(text, 10) && !isNaN(text);
        },
        /* clone: function (obj) {
        if (null == obj || "object" !== typeof obj) {
        return obj;
        }
        var copy = obj.constructor();
        for (var attr in obj) {
        if (obj.hasOwnProperty(attr)) {
        copy[attr] = obj[attr];
        }
        }
        return copy;
        },
        */
        signOut: function () {
            var index = window;
            while (index !== index.parent) // Jump out
            {
                index = index.parent;
            }
            index.location = Site.resolveClientUrl('Header/SignOut');
        },

        setMultiSelectForDropDownList: function (dropDownListId, dataSource, valueField, textField, checkField) {
            var data = $.parseJSON(dataSource);
            var length = data.length;
            var dataUI = [];
            var checkedCount = 0;
            for (var i = 0; i < length; i++) {
                var item = data[i];
                var isChecked = Site.asBool(item[checkField]);
                if (isChecked) {
                    checkedCount++;
                }

                dataUI.push({
                    Id: item[valueField],
                    Value: item[valueField],
                    Text: Site.getSingleResource(item[textField]),
                    Checked: isChecked,
                    Enable: true
                });
            }

            var isCheckAll = checkedCount === length;
            $("#" + dropDownListId).multiSelect(
                {
                    minWidth: 160,
                    maxHeight: 400,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: isCheckAll,
                    checkAllText: Site.getSingleResource('All'),
                    showFooter: false,
                    listData: dataUI,
                    onCheck: function () {
                        var checkedItems = $("#" + dropDownListId).multiSelect("getCheckedItems");
                        $("#product-container input[type=checkbox]:checked").prop("checked", false);
                        var hasItemsChecked = checkedItems.length > 0;
                        if (hasItemsChecked) {
                            for (var i = 0; i < checkedItems.length; i++) {
                                $('#product-container #' + checkedItems[i]).prop("checked", true);
                            }
                        }

                        $("#SubmitButton").prop("disabled", !hasItemsChecked);
                        $(".error-img").toggleClass("hide", hasItemsChecked);
                    }
                });
        }
    };
})(jQuery);

/*override alert function*/
window.oldAlert = window.alert;
window.alert = function (message, fallback) {
    if (fallback) {
        window.oldAlert(message);
        return;
    }
    $(document.createElement('div'))
        .attr({ title: Site.getSingleResource('Alert'), 'class': 'alert' })
        .html(message)
        .dialog({
            buttons: { OK: function () { $(this).dialog('close'); } },
            close: function () { $(this).remove(); },
            draggable: true,
            modal: true,
            resizable: false,
            width: 'auto'
        });
};
$(window).load(function () {
    Site.initBlockUI();
    Site.initialize();
    Site.needOverrideInnerSize = window.innerHeight === undefined;
    Site.overrideInnerSize();
});
$(window).resize(function () {
    Site.overrideInnerSize();
});

$(window).unload(function () {
    $(window).unbind("resize");
});

$(function () {
    var $marquee = $("#notification-marquee");
    if ("marquee" in $marquee) {
        $marquee.marquee({ speed: 30 });
    }

    if (Site.isLoadedNotificationPopup === true) {
        return;
    }

    var $popup = $("#NotificationPopup");
    if ($popup.length > 0) {
        var options = {
            height: 160,
            title: Site.getSingleResource("Notification")
        };
        var popup = $popup.flatPopup(options);
        popup.show();

        $popup.find(".popup-close").click(function () {
            $.cookie("isClosedNotification", "1", { path: '/' });
        });
        Site.isLoadedNotificationPopup = true;
    }
});