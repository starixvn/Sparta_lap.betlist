/// <reference path="/@JSense.js" />
/* Preset */
// root JSON object
var Site = (function ($) {
    var setLicenseeOption = function (urlOrParams, licenseeType, currencyId, currencyName, baseCurrency) {
        licenseeType = licenseeType == undefined ? "" : licenseeType;
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
    };

    return {
        initialize: function () {
            Site.currencyConstraint();
            Site.initEvent();
        },
        initEvent: function () {
            $("#CurrencyId").change(function () {
                Site.currencyConstraint(true);
            });
        },
        unBlockUI: function () {
            $('div.block-body').hide();
            $('div.loading').hide();
            $.unblockUI();
        },

        blockUI: function () {
            $('div.block-body').show();
            $('div.loading').show();
            $.blockUI({
                message: "",
                timeout: 30000
            });
        },

        setLicenseeOptionParam: function (urlOrParams) {
            var licenseeType = $("#LicenseeType").val();
            var currencyId = $("#CurrencyId").val();
            var baseCurrency = $("#BaseCurrency").val();
            var currencyName = $("#CurrencyId option[value=" + currencyId + "]").text();
            if (currencyName == undefined) {
                currencyName = $("#CurrencyName").val();
            }

            return setLicenseeOption(urlOrParams, licenseeType, currencyId, currencyName, baseCurrency);
        },

        setLicenseeOptionOriginalParam: function (urlOrParams) {
            var licenseeType = $("#LicenseeType").attr('data-original');
            var currencyId = $("#CurrencyId").attr('data-original');
            var baseCurrency = $("#BaseCurrency").attr('data-original');
            var currencyName = $("#CurrencyId option[value=" + currencyId + "]").text();
            if (currencyName == undefined) {
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
                $("input[type='checkbox']", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode != undefined && productCode.length > 0) {
                        urlOrParams = Site.setProductOptionParam(urlOrParams, counter, productCode, $(element).prop("checked"));
                        counter++;
                    }
                });

                return urlOrParams;
            }
            else {
                urlOrParams.ProductOptions = [];
                $("input[type='checkbox']", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode != undefined && productCode.length > 0) {
                        urlOrParams.ProductOptions.push({ Code: productCode, Name: productCode, Status: $(element).prop("checked") });
                    }
                });

                return urlOrParams;
            }
        },

        setProductOptionOriginalParamList: function (urlOrParams) {
            if (typeof (urlOrParams) === "string") {
                var counter = 0;
                $("input[type='checkbox']", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode != undefined && productCode.length > 0) {
                        urlOrParams = Site.setProductOptionParam(urlOrParams, counter, productCode, $(element).attr("data-original"));
                        counter++;
                    }
                });

                return urlOrParams;
            }
            else {
                urlOrParams.ProductOptions = [];
                $("input[type='checkbox']", "#ProductList").each(function (index, element) {
                    var productCode = $(element).attr('data-product-code');
                    if (productCode != undefined && productCode.length > 0) {
                        urlOrParams.ProductOptions.push({ Code: productCode, Name: productCode, Status: $(element).attr("data-original") });
                    }
                });

                return urlOrParams;
            }
        },

        setProductOptionParam: function (url, counter, productCode, status) {
            productCode = productCode == null || productCode == undefined ? undefined : productCode.toUpperCase();
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
            $(".blockui").click(function () {
                Site.blockUI();
            });
            Site.unBlockUI();
        },

        currencyConstraint: function (userChange) {
            var currency = $("#CurrencyId").val();
            var currencyName = $("#CurrencyId option:selected").text();
            currencyName = $.trim(currencyName);
            $("#CurrencyName").val(currencyName);

            // Handle outside based currency list in some reports (customer wl, balance)
            var baseCurrencyObj = $("#BaseCurrency");
            var baseCurrency = baseCurrencyObj.val();
            var noConvertOption = $("option[value='']", baseCurrencyObj);
            if (currency == 0 || currencyName.toLowerCase() == "eur" || currencyName.toLowerCase() == "usd") {
                noConvertOption.remove();
                if (userChange) {
                    $("option[value='" + currencyName.toLowerCase() + "']", baseCurrencyObj).attr("selected", "selected");
                    baseCurrencyObj.val(currencyName.toLowerCase() == "usd" ? "usd" : "eur");
                }
            }
            else {
                if (noConvertOption.length == 0
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
                var basedCurrencyMap = { "-1": "eur", "-2": "usd" };
                var basedCurrency = basedCurrencyMap[currency.toString()];
                basedCurrency = basedCurrency === undefined ? "" : basedCurrency;
                $("#BaseCurrency").val(basedCurrency);
            }
        },

        sortReport: function (sortColumn) {
            var sortDirection = $("#SortColumn").val() == sortColumn ? $("#SortDirection").val() == "DESC" ? "ASC" : "DESC" : "ASC";

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
                    if (sortColumn == $("#SortColumn").val()) {
                        headerDiv.addClass("sorted-heading");

                        var sortClass = $("#SortDirection").val() == "DESC" ? "sort-desc" : "sort-asc";
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
            var match = /(chrome)[ \/]([\w.]+)/.exec(userAgent) || /(webkit)[ \/]([\w.]+)/.exec(userAgent) || /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(userAgent) || /(msie) ([\w.]+)/.exec(userAgent) || userAgent.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(userAgent) || [];

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

        getMaxWidth: function (element) {
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

            if (null == obj || "object" != typeof obj)
                return obj;

            // Handle Date
            if (obj instanceof Date) {
                var copy = new Date();
                copy.setTime(obj.getTime());
                return copy;
            }

            // Handle Array
            if (obj instanceof Array) {
                var copy = [];
                for (var i = 0, len = obj.length; i < len; i++) {
                    copy[i] = Site.clone(obj[i]);
                }
                return copy;
            }

            // Handle Object
            if (obj instanceof Object) {
                var copy = {};
                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr))
                        copy[attr] = Site.clone(obj[attr]);
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
            if (typeof input == 'string') {
                return input.toLowerCase() == 'true';
            }

            return !!input;
        },
        getSingleResource: function (enghlishName) {
            /// <summary>
            /// convert text to orther language
            /// </summary>
            /// <param name="enghlishName"></param>

            if (enghlishName == null || enghlishName == undefined) return enghlishName;
            var keyName = "_language_" + _page.lang.replace("-", "_");

            var resources = window[keyName];            
            if (!resources) {
                resources = _language_en_US;
            }

            var mapResource = function (key) {
                var item = resources[key];
                if (item == undefined) {
                    item = _language_en_US[key];
                    return item != undefined ? item : key;
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

            if (arrayText == null) return null;
            if (arrayText.constructor !== Array) return null;

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
            return typeof Site.isNumber(text) && parseFloat(text) == parseInt(text, 10) && !isNaN(text);
        },
        clone: function(obj) {
            if (null == obj || "object" != typeof obj) return obj;
            var copy = obj.constructor();
            for (var attr in obj) {
                if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
            }
            return copy;
        }
    };
})(jQuery);

/*override alert function*/
window.old_alert = window.alert;
window.alert = function (message, fallback) {
    if (fallback) {
        old_alert(message);
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
});