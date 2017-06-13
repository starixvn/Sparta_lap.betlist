webpackJsonp([53],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(259);


/***/ },

/***/ 205:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/// <reference path="/@JSense.js" />
	///
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(206)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, BigNumber) {
	    "use strict";
	    var jqGridAdditional = (function () {
	        return {
	            DECIMAL_FORMAT_OPTIONS: {
	                decimalSeparator: ".",
	                thousandsSeparator: ",",
	                decimalPlaces: 2
	            },
	            updateGridHeight: function () {
	                var $bdiv = $(".ui-jqgrid-bdiv");
	                if ($bdiv.length === 0) {
	                    return;
	                }
	
	                var gridTop = $bdiv.offset().top;
	                var pagingHeight = $(".paging:visible").length ? $(".paging:visible").height() : 0;
	                pagingHeight = pagingHeight || $("#paging:visible").length ? $("#paging:visible").height() : 0;
	                var footerHeight = $(".ui-jqgrid-sdiv:visible").length ? $(".ui-jqgrid-sdiv:visible").height() : 0;
	                var jqgridPagerHeight = $(".ui-jqgrid-pager:visible").length ? $(".ui-jqgrid-pager:visible").outerHeight(true) : 0;
	                var footerSite = 18;
	
	                var gridHeight = window.innerHeight - gridTop - footerHeight - jqgridPagerHeight - pagingHeight - footerSite - 15;
	                gridHeight = Math.max(120, gridHeight);
	
	                $bdiv.css("max-height", gridHeight);
	            },
	            updateGridWidth: function (grid) {
	                if ($("div.ui-jqgrid-hbox").length === 0) {
	                    return;
	                }
	
	                grid.setGridParam({
	                    shrinkToFit: false
	                });
	
	                var parentWidth = grid.closest(".theme_jqGrid").width();
	                grid.jqGrid('setGridWidth', parentWidth);
	
	                var tableWidth = parentWidth - 20;
	                grid.css("width", tableWidth + "px");
	                $(".theme_jqGrid .ui-jqgrid-hdiv table.ui-jqgrid-htable").css("width", tableWidth + "px");
	                $(".theme_jqGrid .ui-jqgrid-sdiv table.ui-jqgrid-ftable").css("width", tableWidth + "px");
	
	                jqGridAdditional.updateFooterWidth();
	            },
	
	            updateFooterWidth: function () {
	                $(".footer-firstrow").remove();
	                var settingWidthRow = $(".ui-jqgrid-bdiv table.ui-jqgrid-btable tr.jqgfirstrow").clone();
	                settingWidthRow.addClass("footer-firstrow").removeClass("jqgfirstrow")
	                                .prependTo($("tbody", "table.ui-jqgrid-ftable"));
	            },
	
	            formatInteger: function (value) {
	                if (value !== undefined && value !== null) {
	                    if (value === "NaN") {
	                        return "NaN";
	                    }
	
	                    var sRegExp = new RegExp('(-?[0-9]+)([0-9]{3})');
	                    var resultValue = value + '';
	
	                    while (sRegExp.test(resultValue)) {
	                        resultValue = resultValue.replace(sRegExp, '$1,$2');
	                    }
	
	                    return resultValue;
	                }
	                return value;
	            },
	
	            formatDecimal: function (value) {
	                if (value !== undefined && value !== null) {
	                    if (isNaN(value)) {
	                        return "NaN";
	                    }
	
	                    return new BigNumber(value.toString()).toFormat(2);
	                }
	
	                return value;
	            },
	
	            formatLicenseeWl: function (value) {
	                var decimalValue = jqGridAdditional.formatDecimal(value);
	                return value < 0 ? "<span class='negative-number'>" + decimalValue + "</span>" : decimalValue;
	            },
	
	            excludeCol: function (colName, colModel, excludedCol) {
	                var result =
	                {
	                    colName: [],
	                    colModel: []
	                };
	                for (var i = 0; i < colModel.length; i++) {
	                    if ($.inArray(colModel[i].name, excludedCol) < 0) {
	                        result.colName.push(colName[i]);
	                        result.colModel.push(colModel[i]);
	                    }
	                }
	                return result;
	            },
	
	            setColumnVisible: function (models, colName, visible) {
	                for (var i = 0, l = models.length; i < l; i++) {
	                    if (models[i].name.toLowerCase() === colName.toLowerCase()) {
	                        models[i].hidden = visible;
	                        break;
	                    }
	                }
	            },
	
	            setGroupCustomerWLColumns: function (grid) {
	                grid.jqGrid('setGroupHeaders',
	                {
	                    useColSpanStyle: true,
	                    groupHeaders: [{
	                        startColumnName: 'CustomerWinLoss',
	                        numberOfColumns: 3,
	                        titleText: Site.getSingleResource('CustomerW_L')
	                    }]
	                });
	            },
	
	            resetHeaderHeight: function () {
	                $(".ui-jqgrid-htable>thead>tr.ui-jqgrid-labels > th.ui-th-column div").addClass("header-two-line");
	            },
	
	            getNewLicenseeWLNote: function (productList, caption) {
	                var temp = caption || Site.getSingleResource("LicenseeW_L");
	                var productTemp = "";
	                var productAds = ["SB", "LC", "NG", "VS", "CS", "BG", "KN", "BT", "MGCLC", "MGCRNG", "VB"];
	                for (var i = 0; i < productList.length; i++) {
	                    if ($.inArray(productList[i], productAds) > -1) {
	                        productTemp += productTemp.length === 0 ? productList[i] : ", " + productList[i];
	                    }
	                }
	                if (productTemp.length > 0) {
	                    temp += "(" + productTemp + ")";
	                }
	                else {
	                    return caption || "";
	                }
	
	                return temp;
	            },
	            // update getNewLicenseeWLNote() as well
	            getLicenseeWLNote: function (productOptionJson, caption) {
	                var productOptions = $.parseJSON(productOptionJson);
	                var temp = caption || Site.getSingleResource("LicenseeW_L");
	                var productTemp = "";
	                var productAds = ["SB", "LC", "NG", "VS", "CS", "BG", "KN", "BT", "MGCLC", "MGCRNG", "VB"];
	                for (var i = 0; i < productOptions.length; i++) {
	                    if ($.inArray(productOptions[i].Code, productAds) > -1 && Site.asBool(productOptions[i].Status)) {
	                        productTemp += productTemp.length === 0 ? productOptions[i].Code : ", " + productOptions[i].Code;
	                    }
	                }
	                if (productTemp.length > 0) {
	                    temp += "(" + productTemp + ")";
	                }
	                else {
	                    return caption || "";
	                }
	
	                return temp;
	            },
	
	            gridComplete: function () {
	                $(".ui-jqgrid-titlebar").hide();
	                $(".ui-jqgrid-hdiv").css("clear", "both");
	
	                $('#noInfoRow').remove();
	            },
	
	            clearGrid: function (reportId) {
	                $('#' + reportId).jqGrid('GridUnload');
	                $("div.theme_jqGrid > div").css('width', '');
	            },
	
	            rowSpanSetting: function (rowId, val, rawObject) {
	                var rowSpan = rawObject.RowSpan, result;
	                if (rowSpan > 1) {
	                    result = ' rowspan=' + '"' + rowSpan + '"';
	                }
	                else if (rowSpan < 0) {
	                    result = ' style="display:none;" ';
	                }
	                else {
	                    result = '';
	                }
	
	                return result;
	            },
	
	            rowSpanSetting2: function (rowId, val, rawObject) {
	                var rowSpan = rawObject.RowSpan2, result;
	                if (rowSpan > 1) {
	                    result = ' rowspan=' + '"' + rowSpan + '"';
	                }
	                else if (rowSpan < 0) {
	                    result = ' style="display:none;" ';
	                }
	                else {
	                    result = '';
	                }
	
	                return result;
	            },
	
	            assignTotal: function (totalData, columnModel) {
	                var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
	
	                if (mainFooter.length >= 0) {
	                    //add grand total
	                    $(totalData).each(function (totalIndex, totalElement) {
	                        var row = mainFooter.clone();
	                        row.addClass("grandTotal");
	                        var cellList = $("td", row);
	                        $(columnModel).each(function (cellIndex, modelElement) {
	                            var cell = cellList.get(cellIndex);
	                            var isCustomFormater = typeof (modelElement.formatter) === "function";
	                            var rawValue = totalElement[modelElement.name];
	                            var value;
	                            if (isCustomFormater) {
	                                value = modelElement.formatter(rawValue, modelElement.formatoptions, row);
	                            } else {
	                                var formatOptions = {};
	                                formatOptions[modelElement.formatter] = modelElement.formatoptions;
	                                value = $.fn.fmatter(modelElement.formatter, rawValue, formatOptions);
	                            }
	
	                            var titleValue = value;
	                            if ($.isNumeric(rawValue) && rawValue < 0) {
	                                var wrapper = document.createElement('div');
	                                wrapper.innerHTML = value;
	                                titleValue = wrapper.outerHtml;
	                            }
	                            $(cell).html(value).attr("title", titleValue);
	                        });
	                        row.insertBefore(mainFooter);
	                    });
	                }
	            },
	
	            setFooterColumns: function (param, columnNames) {
	                var isShow = "showCol" === param;
	                var footers = $("table.ui-jqgrid-ftable tr");
	                if (isShow) {
	                    $.each(columnNames, function (index, value) {
	                        $('td[aria-describedby="' + value + '"]', footers).show();
	                    });
	                }
	                else {
	                    $.each(columnNames, function (index, value) {
	                        $('td[aria-describedby="' + value + '"]', footers).hide();
	                    });
	                }
	            },
	
	            setNoInfo: function (message) {
	                var noInfoText = message || Site.getSingleResource("NoInfo");
	                var htmlNoInfor = '<p id="noInfoRow" class="no-info-row">' + noInfoText + '</p>';
	                $('#noInfoRow').remove();
	                $('.theme_jqGrid').append(htmlNoInfor);
	                $("div.ui-jqgrid-sdiv").hide();
	            },
	
	            setNoInfoWithColspan: function (colspan) {
	                var mainFooter = $(".ui-jqgrid-ftable");
	                var info = Site.getSingleResource("NoInfo");
	
	                if (mainFooter.length > 0) {
	                    mainFooter.remove();
	                }
	                var contentTable = $("table.ui-jqgrid-htable");
	                if (contentTable.find(".no-info-row").length === 0) {
	                    var temps = '<tr role="row" class="no-info-row  ui-widget-content jqgrow ui-row-ltr" style="height:auto;">' +
	                        ' <td style="text-align:center;" role="role="gridcell" colspan="' + colspan + '">' + info + '</td></tr>';
	                    contentTable.append(temps);
	                }
	
	                $("div.ui-jqgrid-sdiv").hide();
	            },
	
	            setGroupFooters: function (colspan, summaryColspan, summaryRowCount, allowScrolling) {
	                /// <summary>
	                /// set colspan for footer rows
	                /// </summary>
	                /// <param name="colspan"></param>
	                /// <param name="summaryColspan"></param>
	                /// <param name="summaryRowCount"></param>
	
	                summaryRowCount = summaryRowCount === undefined ? 0 : summaryRowCount;
	                var footerTable = $("table.ui-jqgrid-ftable");
	                var footerRowList = $("tr", footerTable);
	                var rowCount = footerRowList.length;
	                var columnIndex = 0;
	                if (rowCount > 0) {
	                    // Hide other columns to match the colspan & rowspan
	                    var processFooter = function ($td, colspan) {
	                        $td.each(function (j, cell) {
	                            if (columnIndex < colspan) {
	                                if ($(cell).is(":visible")) {
	                                    $(cell).hide();
	                                    columnIndex++;
	                                }
	                            }
	                        });
	                    };
	
	                    for (var i = 0; i < rowCount - summaryRowCount; i++) {
	                        columnIndex = 0;
	                        processFooter($("td ", footerRowList[i]), colspan);
	                    }
	
	                    //#region Render total eur section
	                    columnIndex = 0;
	                    // Hide other eur columns to match the colspan & rowspan
	                    for (var j = rowCount - summaryRowCount; j < rowCount; j++) {
	                        columnIndex = 0;
	                        processFooter($("td ", footerRowList[j]), summaryColspan);
	                    }
	                    var totalInText = Site.getSingleResource("Totalin");
	                    //#region Render total eur section
	
	                    var totalText = Site.getSingleResource("Total");
	                    // Set colspan for total section
	                    if (rowCount > 1) {
	                        // Show total text
	                        $("td:first-child", footerRowList[0]).attr("colspan", colspan)
	                                .attr("rowspan", rowCount - summaryRowCount).css("display", "")
	                                .css("width", "").html(totalText);
	                    }
	                    else {
	                        $("td:first-child", footerRowList[0]).css("display", "").attr("colspan", summaryColspan)
	                            .css("width", "").html(totalInText + " EUR");
	                        //remove border style
	                        $(".ui-jqgrid-sdiv .ui-jqgrid-hbox").css("border-top", "none");
	                    }
	
	                    // If there are many total rows such as total eur, total usd, ...
	                    var currencyListString = footerTable.attr("data-total-currency-list");
	                    if (currencyListString) {
	                        var currencyList = currencyListString.split(",");
	                        var rowspan = summaryRowCount / currencyList.length;
	                        $(currencyList).each(function (i, currency) {
	                            $("td.group-" + currency + ":first-child", footerTable)
	                                    .attr("rowspan", rowspan).css("display", "")
	                                    .attr("colspan", summaryColspan).html(totalInText + " " + currency);
	                            $("td.group-" + currency + ":gt(0)", footerTable).hide();
	                            $("td.group-" + currency + ":first-child", footerTable).parent().addClass("summarytotal");
	                        });
	                    } else {
	                        if (footerRowList.length > 1) {
	                            $(footerRowList[footerRowList.length = 1]).hide();
	                        }
	                    }
	                    /*set scrolling*/
	                    if (allowScrolling) {
	                        footerTable.closest(".ui-jqgrid-sdiv").addClass("jqgrid-footer-scroll-hacking");
	                    }
	                }
	            },
	
	            addSortableHeader: function ($grid, sortColumn, sortOrder, isLightText) {
	                $grid.closest(".ui-jqgrid-view")
	                        .find(".ui-jqgrid-sortable")
	                        .each(function (index, element) {
	                            var $this = $(element);
	                            $this.find(".s-ico").remove();
	
	                            if (element.id.indexOf(sortColumn) > -1) {
	                                var sortClass = "sort-icon-" + sortOrder + (isLightText ? " light" : "");
	                                var sortSpan = $("<span class='sort-icon'></span>");
	                                sortSpan.addClass(sortClass);
	                                $this.append(sortSpan);
	                                $this.addClass("sortable sort-heading sorted-heading");
	                            }
	                        });
	            }
	        };
	    })();
	
	    return jqGridAdditional;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 206:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_RESULT__;/*! bignumber.js v2.4.0 https://github.com/MikeMcl/bignumber.js/LICENCE */
	
	;(function (globalObj) {
	    'use strict';
	
	    /*
	      bignumber.js v2.4.0
	      A JavaScript library for arbitrary-precision arithmetic.
	      https://github.com/MikeMcl/bignumber.js
	      Copyright (c) 2016 Michael Mclaughlin <M8ch88l@gmail.com>
	      MIT Expat Licence
	    */
	
	
	    var BigNumber, cryptoObj, parseNumeric,
	        isNumeric = /^-?(\d+(\.\d*)?|\.\d+)(e[+-]?\d+)?$/i,
	        mathceil = Math.ceil,
	        mathfloor = Math.floor,
	        notBool = ' not a boolean or binary digit',
	        roundingMode = 'rounding mode',
	        tooManyDigits = 'number type has more than 15 significant digits',
	        ALPHABET = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ$_',
	        BASE = 1e14,
	        LOG_BASE = 14,
	        MAX_SAFE_INTEGER = 0x1fffffffffffff,         // 2^53 - 1
	        // MAX_INT32 = 0x7fffffff,                   // 2^31 - 1
	        POWS_TEN = [1, 10, 100, 1e3, 1e4, 1e5, 1e6, 1e7, 1e8, 1e9, 1e10, 1e11, 1e12, 1e13],
	        SQRT_BASE = 1e7,
	
	        /*
	         * The limit on the value of DECIMAL_PLACES, TO_EXP_NEG, TO_EXP_POS, MIN_EXP, MAX_EXP, and
	         * the arguments to toExponential, toFixed, toFormat, and toPrecision, beyond which an
	         * exception is thrown (if ERRORS is true).
	         */
	        MAX = 1E9;                                   // 0 to MAX_INT32
	
	    if ( typeof crypto != 'undefined' ) cryptoObj = crypto;
	
	
	    /*
	     * Create and return a BigNumber constructor.
	     */
	    function constructorFactory(configObj) {
	        var div,
	
	            // id tracks the caller function, so its name can be included in error messages.
	            id = 0,
	            P = BigNumber.prototype,
	            ONE = new BigNumber(1),
	
	
	            /********************************* EDITABLE DEFAULTS **********************************/
	
	
	            /*
	             * The default values below must be integers within the inclusive ranges stated.
	             * The values can also be changed at run-time using BigNumber.config.
	             */
	
	            // The maximum number of decimal places for operations involving division.
	            DECIMAL_PLACES = 20,                     // 0 to MAX
	
	            /*
	             * The rounding mode used when rounding to the above decimal places, and when using
	             * toExponential, toFixed, toFormat and toPrecision, and round (default value).
	             * UP         0 Away from zero.
	             * DOWN       1 Towards zero.
	             * CEIL       2 Towards +Infinity.
	             * FLOOR      3 Towards -Infinity.
	             * HALF_UP    4 Towards nearest neighbour. If equidistant, up.
	             * HALF_DOWN  5 Towards nearest neighbour. If equidistant, down.
	             * HALF_EVEN  6 Towards nearest neighbour. If equidistant, towards even neighbour.
	             * HALF_CEIL  7 Towards nearest neighbour. If equidistant, towards +Infinity.
	             * HALF_FLOOR 8 Towards nearest neighbour. If equidistant, towards -Infinity.
	             */
	            ROUNDING_MODE = 4,                       // 0 to 8
	
	            // EXPONENTIAL_AT : [TO_EXP_NEG , TO_EXP_POS]
	
	            // The exponent value at and beneath which toString returns exponential notation.
	            // Number type: -7
	            TO_EXP_NEG = -7,                         // 0 to -MAX
	
	            // The exponent value at and above which toString returns exponential notation.
	            // Number type: 21
	            TO_EXP_POS = 21,                         // 0 to MAX
	
	            // RANGE : [MIN_EXP, MAX_EXP]
	
	            // The minimum exponent value, beneath which underflow to zero occurs.
	            // Number type: -324  (5e-324)
	            MIN_EXP = -1e7,                          // -1 to -MAX
	
	            // The maximum exponent value, above which overflow to Infinity occurs.
	            // Number type:  308  (1.7976931348623157e+308)
	            // For MAX_EXP > 1e7, e.g. new BigNumber('1e100000000').plus(1) may be slow.
	            MAX_EXP = 1e7,                           // 1 to MAX
	
	            // Whether BigNumber Errors are ever thrown.
	            ERRORS = true,                           // true or false
	
	            // Change to intValidatorNoErrors if ERRORS is false.
	            isValidInt = intValidatorWithErrors,     // intValidatorWithErrors/intValidatorNoErrors
	
	            // Whether to use cryptographically-secure random number generation, if available.
	            CRYPTO = false,                          // true or false
	
	            /*
	             * The modulo mode used when calculating the modulus: a mod n.
	             * The quotient (q = a / n) is calculated according to the corresponding rounding mode.
	             * The remainder (r) is calculated as: r = a - n * q.
	             *
	             * UP        0 The remainder is positive if the dividend is negative, else is negative.
	             * DOWN      1 The remainder has the same sign as the dividend.
	             *             This modulo mode is commonly known as 'truncated division' and is
	             *             equivalent to (a % n) in JavaScript.
	             * FLOOR     3 The remainder has the same sign as the divisor (Python %).
	             * HALF_EVEN 6 This modulo mode implements the IEEE 754 remainder function.
	             * EUCLID    9 Euclidian division. q = sign(n) * floor(a / abs(n)).
	             *             The remainder is always positive.
	             *
	             * The truncated division, floored division, Euclidian division and IEEE 754 remainder
	             * modes are commonly used for the modulus operation.
	             * Although the other rounding modes can also be used, they may not give useful results.
	             */
	            MODULO_MODE = 1,                         // 0 to 9
	
	            // The maximum number of significant digits of the result of the toPower operation.
	            // If POW_PRECISION is 0, there will be unlimited significant digits.
	            POW_PRECISION = 100,                     // 0 to MAX
	
	            // The format specification used by the BigNumber.prototype.toFormat method.
	            FORMAT = {
	                decimalSeparator: '.',
	                groupSeparator: ',',
	                groupSize: 3,
	                secondaryGroupSize: 0,
	                fractionGroupSeparator: '\xA0',      // non-breaking space
	                fractionGroupSize: 0
	            };
	
	
	        /******************************************************************************************/
	
	
	        // CONSTRUCTOR
	
	
	        /*
	         * The BigNumber constructor and exported function.
	         * Create and return a new instance of a BigNumber object.
	         *
	         * n {number|string|BigNumber} A numeric value.
	         * [b] {number} The base of n. Integer, 2 to 64 inclusive.
	         */
	        function BigNumber( n, b ) {
	            var c, e, i, num, len, str,
	                x = this;
	
	            // Enable constructor usage without new.
	            if ( !( x instanceof BigNumber ) ) {
	
	                // 'BigNumber() constructor call without new: {n}'
	                if (ERRORS) raise( 26, 'constructor call without new', n );
	                return new BigNumber( n, b );
	            }
	
	            // 'new BigNumber() base not an integer: {b}'
	            // 'new BigNumber() base out of range: {b}'
	            if ( b == null || !isValidInt( b, 2, 64, id, 'base' ) ) {
	
	                // Duplicate.
	                if ( n instanceof BigNumber ) {
	                    x.s = n.s;
	                    x.e = n.e;
	                    x.c = ( n = n.c ) ? n.slice() : n;
	                    id = 0;
	                    return;
	                }
	
	                if ( ( num = typeof n == 'number' ) && n * 0 == 0 ) {
	                    x.s = 1 / n < 0 ? ( n = -n, -1 ) : 1;
	
	                    // Fast path for integers.
	                    if ( n === ~~n ) {
	                        for ( e = 0, i = n; i >= 10; i /= 10, e++ );
	                        x.e = e;
	                        x.c = [n];
	                        id = 0;
	                        return;
	                    }
	
	                    str = n + '';
	                } else {
	                    if ( !isNumeric.test( str = n + '' ) ) return parseNumeric( x, str, num );
	                    x.s = str.charCodeAt(0) === 45 ? ( str = str.slice(1), -1 ) : 1;
	                }
	            } else {
	                b = b | 0;
	                str = n + '';
	
	                // Ensure return value is rounded to DECIMAL_PLACES as with other bases.
	                // Allow exponential notation to be used with base 10 argument.
	                if ( b == 10 ) {
	                    x = new BigNumber( n instanceof BigNumber ? n : str );
	                    return round( x, DECIMAL_PLACES + x.e + 1, ROUNDING_MODE );
	                }
	
	                // Avoid potential interpretation of Infinity and NaN as base 44+ values.
	                // Any number in exponential form will fail due to the [Ee][+-].
	                if ( ( num = typeof n == 'number' ) && n * 0 != 0 ||
	                  !( new RegExp( '^-?' + ( c = '[' + ALPHABET.slice( 0, b ) + ']+' ) +
	                    '(?:\\.' + c + ')?$',b < 37 ? 'i' : '' ) ).test(str) ) {
	                    return parseNumeric( x, str, num, b );
	                }
	
	                if (num) {
	                    x.s = 1 / n < 0 ? ( str = str.slice(1), -1 ) : 1;
	
	                    if ( ERRORS && str.replace( /^0\.0*|\./, '' ).length > 15 ) {
	
	                        // 'new BigNumber() number type has more than 15 significant digits: {n}'
	                        raise( id, tooManyDigits, n );
	                    }
	
	                    // Prevent later check for length on converted number.
	                    num = false;
	                } else {
	                    x.s = str.charCodeAt(0) === 45 ? ( str = str.slice(1), -1 ) : 1;
	                }
	
	                str = convertBase( str, 10, b, x.s );
	            }
	
	            // Decimal point?
	            if ( ( e = str.indexOf('.') ) > -1 ) str = str.replace( '.', '' );
	
	            // Exponential form?
	            if ( ( i = str.search( /e/i ) ) > 0 ) {
	
	                // Determine exponent.
	                if ( e < 0 ) e = i;
	                e += +str.slice( i + 1 );
	                str = str.substring( 0, i );
	            } else if ( e < 0 ) {
	
	                // Integer.
	                e = str.length;
	            }
	
	            // Determine leading zeros.
	            for ( i = 0; str.charCodeAt(i) === 48; i++ );
	
	            // Determine trailing zeros.
	            for ( len = str.length; str.charCodeAt(--len) === 48; );
	            str = str.slice( i, len + 1 );
	
	            if (str) {
	                len = str.length;
	
	                // Disallow numbers with over 15 significant digits if number type.
	                // 'new BigNumber() number type has more than 15 significant digits: {n}'
	                if ( num && ERRORS && len > 15 && ( n > MAX_SAFE_INTEGER || n !== mathfloor(n) ) ) {
	                    raise( id, tooManyDigits, x.s * n );
	                }
	
	                e = e - i - 1;
	
	                 // Overflow?
	                if ( e > MAX_EXP ) {
	
	                    // Infinity.
	                    x.c = x.e = null;
	
	                // Underflow?
	                } else if ( e < MIN_EXP ) {
	
	                    // Zero.
	                    x.c = [ x.e = 0 ];
	                } else {
	                    x.e = e;
	                    x.c = [];
	
	                    // Transform base
	
	                    // e is the base 10 exponent.
	                    // i is where to slice str to get the first element of the coefficient array.
	                    i = ( e + 1 ) % LOG_BASE;
	                    if ( e < 0 ) i += LOG_BASE;
	
	                    if ( i < len ) {
	                        if (i) x.c.push( +str.slice( 0, i ) );
	
	                        for ( len -= LOG_BASE; i < len; ) {
	                            x.c.push( +str.slice( i, i += LOG_BASE ) );
	                        }
	
	                        str = str.slice(i);
	                        i = LOG_BASE - str.length;
	                    } else {
	                        i -= len;
	                    }
	
	                    for ( ; i--; str += '0' );
	                    x.c.push( +str );
	                }
	            } else {
	
	                // Zero.
	                x.c = [ x.e = 0 ];
	            }
	
	            id = 0;
	        }
	
	
	        // CONSTRUCTOR PROPERTIES
	
	
	        BigNumber.another = constructorFactory;
	
	        BigNumber.ROUND_UP = 0;
	        BigNumber.ROUND_DOWN = 1;
	        BigNumber.ROUND_CEIL = 2;
	        BigNumber.ROUND_FLOOR = 3;
	        BigNumber.ROUND_HALF_UP = 4;
	        BigNumber.ROUND_HALF_DOWN = 5;
	        BigNumber.ROUND_HALF_EVEN = 6;
	        BigNumber.ROUND_HALF_CEIL = 7;
	        BigNumber.ROUND_HALF_FLOOR = 8;
	        BigNumber.EUCLID = 9;
	
	
	        /*
	         * Configure infrequently-changing library-wide settings.
	         *
	         * Accept an object or an argument list, with one or many of the following properties or
	         * parameters respectively:
	         *
	         *   DECIMAL_PLACES  {number}  Integer, 0 to MAX inclusive
	         *   ROUNDING_MODE   {number}  Integer, 0 to 8 inclusive
	         *   EXPONENTIAL_AT  {number|number[]}  Integer, -MAX to MAX inclusive or
	         *                                      [integer -MAX to 0 incl., 0 to MAX incl.]
	         *   RANGE           {number|number[]}  Non-zero integer, -MAX to MAX inclusive or
	         *                                      [integer -MAX to -1 incl., integer 1 to MAX incl.]
	         *   ERRORS          {boolean|number}   true, false, 1 or 0
	         *   CRYPTO          {boolean|number}   true, false, 1 or 0
	         *   MODULO_MODE     {number}           0 to 9 inclusive
	         *   POW_PRECISION   {number}           0 to MAX inclusive
	         *   FORMAT          {object}           See BigNumber.prototype.toFormat
	         *      decimalSeparator       {string}
	         *      groupSeparator         {string}
	         *      groupSize              {number}
	         *      secondaryGroupSize     {number}
	         *      fractionGroupSeparator {string}
	         *      fractionGroupSize      {number}
	         *
	         * (The values assigned to the above FORMAT object properties are not checked for validity.)
	         *
	         * E.g.
	         * BigNumber.config(20, 4) is equivalent to
	         * BigNumber.config({ DECIMAL_PLACES : 20, ROUNDING_MODE : 4 })
	         *
	         * Ignore properties/parameters set to null or undefined.
	         * Return an object with the properties current values.
	         */
	        BigNumber.config = function () {
	            var v, p,
	                i = 0,
	                r = {},
	                a = arguments,
	                o = a[0],
	                has = o && typeof o == 'object'
	                  ? function () { if ( o.hasOwnProperty(p) ) return ( v = o[p] ) != null; }
	                  : function () { if ( a.length > i ) return ( v = a[i++] ) != null; };
	
	            // DECIMAL_PLACES {number} Integer, 0 to MAX inclusive.
	            // 'config() DECIMAL_PLACES not an integer: {v}'
	            // 'config() DECIMAL_PLACES out of range: {v}'
	            if ( has( p = 'DECIMAL_PLACES' ) && isValidInt( v, 0, MAX, 2, p ) ) {
	                DECIMAL_PLACES = v | 0;
	            }
	            r[p] = DECIMAL_PLACES;
	
	            // ROUNDING_MODE {number} Integer, 0 to 8 inclusive.
	            // 'config() ROUNDING_MODE not an integer: {v}'
	            // 'config() ROUNDING_MODE out of range: {v}'
	            if ( has( p = 'ROUNDING_MODE' ) && isValidInt( v, 0, 8, 2, p ) ) {
	                ROUNDING_MODE = v | 0;
	            }
	            r[p] = ROUNDING_MODE;
	
	            // EXPONENTIAL_AT {number|number[]}
	            // Integer, -MAX to MAX inclusive or [integer -MAX to 0 inclusive, 0 to MAX inclusive].
	            // 'config() EXPONENTIAL_AT not an integer: {v}'
	            // 'config() EXPONENTIAL_AT out of range: {v}'
	            if ( has( p = 'EXPONENTIAL_AT' ) ) {
	
	                if ( isArray(v) ) {
	                    if ( isValidInt( v[0], -MAX, 0, 2, p ) && isValidInt( v[1], 0, MAX, 2, p ) ) {
	                        TO_EXP_NEG = v[0] | 0;
	                        TO_EXP_POS = v[1] | 0;
	                    }
	                } else if ( isValidInt( v, -MAX, MAX, 2, p ) ) {
	                    TO_EXP_NEG = -( TO_EXP_POS = ( v < 0 ? -v : v ) | 0 );
	                }
	            }
	            r[p] = [ TO_EXP_NEG, TO_EXP_POS ];
	
	            // RANGE {number|number[]} Non-zero integer, -MAX to MAX inclusive or
	            // [integer -MAX to -1 inclusive, integer 1 to MAX inclusive].
	            // 'config() RANGE not an integer: {v}'
	            // 'config() RANGE cannot be zero: {v}'
	            // 'config() RANGE out of range: {v}'
	            if ( has( p = 'RANGE' ) ) {
	
	                if ( isArray(v) ) {
	                    if ( isValidInt( v[0], -MAX, -1, 2, p ) && isValidInt( v[1], 1, MAX, 2, p ) ) {
	                        MIN_EXP = v[0] | 0;
	                        MAX_EXP = v[1] | 0;
	                    }
	                } else if ( isValidInt( v, -MAX, MAX, 2, p ) ) {
	                    if ( v | 0 ) MIN_EXP = -( MAX_EXP = ( v < 0 ? -v : v ) | 0 );
	                    else if (ERRORS) raise( 2, p + ' cannot be zero', v );
	                }
	            }
	            r[p] = [ MIN_EXP, MAX_EXP ];
	
	            // ERRORS {boolean|number} true, false, 1 or 0.
	            // 'config() ERRORS not a boolean or binary digit: {v}'
	            if ( has( p = 'ERRORS' ) ) {
	
	                if ( v === !!v || v === 1 || v === 0 ) {
	                    id = 0;
	                    isValidInt = ( ERRORS = !!v ) ? intValidatorWithErrors : intValidatorNoErrors;
	                } else if (ERRORS) {
	                    raise( 2, p + notBool, v );
	                }
	            }
	            r[p] = ERRORS;
	
	            // CRYPTO {boolean|number} true, false, 1 or 0.
	            // 'config() CRYPTO not a boolean or binary digit: {v}'
	            // 'config() crypto unavailable: {crypto}'
	            if ( has( p = 'CRYPTO' ) ) {
	
	                if ( v === !!v || v === 1 || v === 0 ) {
	                    CRYPTO = !!( v && cryptoObj );
	                    if ( v && !CRYPTO && ERRORS ) raise( 2, 'crypto unavailable', cryptoObj );
	                } else if (ERRORS) {
	                    raise( 2, p + notBool, v );
	                }
	            }
	            r[p] = CRYPTO;
	
	            // MODULO_MODE {number} Integer, 0 to 9 inclusive.
	            // 'config() MODULO_MODE not an integer: {v}'
	            // 'config() MODULO_MODE out of range: {v}'
	            if ( has( p = 'MODULO_MODE' ) && isValidInt( v, 0, 9, 2, p ) ) {
	                MODULO_MODE = v | 0;
	            }
	            r[p] = MODULO_MODE;
	
	            // POW_PRECISION {number} Integer, 0 to MAX inclusive.
	            // 'config() POW_PRECISION not an integer: {v}'
	            // 'config() POW_PRECISION out of range: {v}'
	            if ( has( p = 'POW_PRECISION' ) && isValidInt( v, 0, MAX, 2, p ) ) {
	                POW_PRECISION = v | 0;
	            }
	            r[p] = POW_PRECISION;
	
	            // FORMAT {object}
	            // 'config() FORMAT not an object: {v}'
	            if ( has( p = 'FORMAT' ) ) {
	
	                if ( typeof v == 'object' ) {
	                    FORMAT = v;
	                } else if (ERRORS) {
	                    raise( 2, p + ' not an object', v );
	                }
	            }
	            r[p] = FORMAT;
	
	            return r;
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the maximum of the arguments.
	         *
	         * arguments {number|string|BigNumber}
	         */
	        BigNumber.max = function () { return maxOrMin( arguments, P.lt ); };
	
	
	        /*
	         * Return a new BigNumber whose value is the minimum of the arguments.
	         *
	         * arguments {number|string|BigNumber}
	         */
	        BigNumber.min = function () { return maxOrMin( arguments, P.gt ); };
	
	
	        /*
	         * Return a new BigNumber with a random value equal to or greater than 0 and less than 1,
	         * and with dp, or DECIMAL_PLACES if dp is omitted, decimal places (or less if trailing
	         * zeros are produced).
	         *
	         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
	         *
	         * 'random() decimal places not an integer: {dp}'
	         * 'random() decimal places out of range: {dp}'
	         * 'random() crypto unavailable: {crypto}'
	         */
	        BigNumber.random = (function () {
	            var pow2_53 = 0x20000000000000;
	
	            // Return a 53 bit integer n, where 0 <= n < 9007199254740992.
	            // Check if Math.random() produces more than 32 bits of randomness.
	            // If it does, assume at least 53 bits are produced, otherwise assume at least 30 bits.
	            // 0x40000000 is 2^30, 0x800000 is 2^23, 0x1fffff is 2^21 - 1.
	            var random53bitInt = (Math.random() * pow2_53) & 0x1fffff
	              ? function () { return mathfloor( Math.random() * pow2_53 ); }
	              : function () { return ((Math.random() * 0x40000000 | 0) * 0x800000) +
	                  (Math.random() * 0x800000 | 0); };
	
	            return function (dp) {
	                var a, b, e, k, v,
	                    i = 0,
	                    c = [],
	                    rand = new BigNumber(ONE);
	
	                dp = dp == null || !isValidInt( dp, 0, MAX, 14 ) ? DECIMAL_PLACES : dp | 0;
	                k = mathceil( dp / LOG_BASE );
	
	                if (CRYPTO) {
	
	                    // Browsers supporting crypto.getRandomValues.
	                    if ( cryptoObj && cryptoObj.getRandomValues ) {
	
	                        a = cryptoObj.getRandomValues( new Uint32Array( k *= 2 ) );
	
	                        for ( ; i < k; ) {
	
	                            // 53 bits:
	                            // ((Math.pow(2, 32) - 1) * Math.pow(2, 21)).toString(2)
	                            // 11111 11111111 11111111 11111111 11100000 00000000 00000000
	                            // ((Math.pow(2, 32) - 1) >>> 11).toString(2)
	                            //                                     11111 11111111 11111111
	                            // 0x20000 is 2^21.
	                            v = a[i] * 0x20000 + (a[i + 1] >>> 11);
	
	                            // Rejection sampling:
	                            // 0 <= v < 9007199254740992
	                            // Probability that v >= 9e15, is
	                            // 7199254740992 / 9007199254740992 ~= 0.0008, i.e. 1 in 1251
	                            if ( v >= 9e15 ) {
	                                b = cryptoObj.getRandomValues( new Uint32Array(2) );
	                                a[i] = b[0];
	                                a[i + 1] = b[1];
	                            } else {
	
	                                // 0 <= v <= 8999999999999999
	                                // 0 <= (v % 1e14) <= 99999999999999
	                                c.push( v % 1e14 );
	                                i += 2;
	                            }
	                        }
	                        i = k / 2;
	
	                    // Node.js supporting crypto.randomBytes.
	                    } else if ( cryptoObj && cryptoObj.randomBytes ) {
	
	                        // buffer
	                        a = cryptoObj.randomBytes( k *= 7 );
	
	                        for ( ; i < k; ) {
	
	                            // 0x1000000000000 is 2^48, 0x10000000000 is 2^40
	                            // 0x100000000 is 2^32, 0x1000000 is 2^24
	                            // 11111 11111111 11111111 11111111 11111111 11111111 11111111
	                            // 0 <= v < 9007199254740992
	                            v = ( ( a[i] & 31 ) * 0x1000000000000 ) + ( a[i + 1] * 0x10000000000 ) +
	                                  ( a[i + 2] * 0x100000000 ) + ( a[i + 3] * 0x1000000 ) +
	                                  ( a[i + 4] << 16 ) + ( a[i + 5] << 8 ) + a[i + 6];
	
	                            if ( v >= 9e15 ) {
	                                cryptoObj.randomBytes(7).copy( a, i );
	                            } else {
	
	                                // 0 <= (v % 1e14) <= 99999999999999
	                                c.push( v % 1e14 );
	                                i += 7;
	                            }
	                        }
	                        i = k / 7;
	                    } else if (ERRORS) {
	                        raise( 14, 'crypto unavailable', cryptoObj );
	                    }
	                }
	
	                // Use Math.random: CRYPTO is false or crypto is unavailable and ERRORS is false.
	                if (!i) {
	
	                    for ( ; i < k; ) {
	                        v = random53bitInt();
	                        if ( v < 9e15 ) c[i++] = v % 1e14;
	                    }
	                }
	
	                k = c[--i];
	                dp %= LOG_BASE;
	
	                // Convert trailing digits to zeros according to dp.
	                if ( k && dp ) {
	                    v = POWS_TEN[LOG_BASE - dp];
	                    c[i] = mathfloor( k / v ) * v;
	                }
	
	                // Remove trailing elements which are zero.
	                for ( ; c[i] === 0; c.pop(), i-- );
	
	                // Zero?
	                if ( i < 0 ) {
	                    c = [ e = 0 ];
	                } else {
	
	                    // Remove leading elements which are zero and adjust exponent accordingly.
	                    for ( e = -1 ; c[0] === 0; c.shift(), e -= LOG_BASE);
	
	                    // Count the digits of the first element of c to determine leading zeros, and...
	                    for ( i = 1, v = c[0]; v >= 10; v /= 10, i++);
	
	                    // adjust the exponent accordingly.
	                    if ( i < LOG_BASE ) e -= LOG_BASE - i;
	                }
	
	                rand.e = e;
	                rand.c = c;
	                return rand;
	            };
	        })();
	
	
	        // PRIVATE FUNCTIONS
	
	
	        // Convert a numeric string of baseIn to a numeric string of baseOut.
	        function convertBase( str, baseOut, baseIn, sign ) {
	            var d, e, k, r, x, xc, y,
	                i = str.indexOf( '.' ),
	                dp = DECIMAL_PLACES,
	                rm = ROUNDING_MODE;
	
	            if ( baseIn < 37 ) str = str.toLowerCase();
	
	            // Non-integer.
	            if ( i >= 0 ) {
	                k = POW_PRECISION;
	
	                // Unlimited precision.
	                POW_PRECISION = 0;
	                str = str.replace( '.', '' );
	                y = new BigNumber(baseIn);
	                x = y.pow( str.length - i );
	                POW_PRECISION = k;
	
	                // Convert str as if an integer, then restore the fraction part by dividing the
	                // result by its base raised to a power.
	                y.c = toBaseOut( toFixedPoint( coeffToString( x.c ), x.e ), 10, baseOut );
	                y.e = y.c.length;
	            }
	
	            // Convert the number as integer.
	            xc = toBaseOut( str, baseIn, baseOut );
	            e = k = xc.length;
	
	            // Remove trailing zeros.
	            for ( ; xc[--k] == 0; xc.pop() );
	            if ( !xc[0] ) return '0';
	
	            if ( i < 0 ) {
	                --e;
	            } else {
	                x.c = xc;
	                x.e = e;
	
	                // sign is needed for correct rounding.
	                x.s = sign;
	                x = div( x, y, dp, rm, baseOut );
	                xc = x.c;
	                r = x.r;
	                e = x.e;
	            }
	
	            d = e + dp + 1;
	
	            // The rounding digit, i.e. the digit to the right of the digit that may be rounded up.
	            i = xc[d];
	            k = baseOut / 2;
	            r = r || d < 0 || xc[d + 1] != null;
	
	            r = rm < 4 ? ( i != null || r ) && ( rm == 0 || rm == ( x.s < 0 ? 3 : 2 ) )
	                       : i > k || i == k &&( rm == 4 || r || rm == 6 && xc[d - 1] & 1 ||
	                         rm == ( x.s < 0 ? 8 : 7 ) );
	
	            if ( d < 1 || !xc[0] ) {
	
	                // 1^-dp or 0.
	                str = r ? toFixedPoint( '1', -dp ) : '0';
	            } else {
	                xc.length = d;
	
	                if (r) {
	
	                    // Rounding up may mean the previous digit has to be rounded up and so on.
	                    for ( --baseOut; ++xc[--d] > baseOut; ) {
	                        xc[d] = 0;
	
	                        if ( !d ) {
	                            ++e;
	                            xc.unshift(1);
	                        }
	                    }
	                }
	
	                // Determine trailing zeros.
	                for ( k = xc.length; !xc[--k]; );
	
	                // E.g. [4, 11, 15] becomes 4bf.
	                for ( i = 0, str = ''; i <= k; str += ALPHABET.charAt( xc[i++] ) );
	                str = toFixedPoint( str, e );
	            }
	
	            // The caller will add the sign.
	            return str;
	        }
	
	
	        // Perform division in the specified base. Called by div and convertBase.
	        div = (function () {
	
	            // Assume non-zero x and k.
	            function multiply( x, k, base ) {
	                var m, temp, xlo, xhi,
	                    carry = 0,
	                    i = x.length,
	                    klo = k % SQRT_BASE,
	                    khi = k / SQRT_BASE | 0;
	
	                for ( x = x.slice(); i--; ) {
	                    xlo = x[i] % SQRT_BASE;
	                    xhi = x[i] / SQRT_BASE | 0;
	                    m = khi * xlo + xhi * klo;
	                    temp = klo * xlo + ( ( m % SQRT_BASE ) * SQRT_BASE ) + carry;
	                    carry = ( temp / base | 0 ) + ( m / SQRT_BASE | 0 ) + khi * xhi;
	                    x[i] = temp % base;
	                }
	
	                if (carry) x.unshift(carry);
	
	                return x;
	            }
	
	            function compare( a, b, aL, bL ) {
	                var i, cmp;
	
	                if ( aL != bL ) {
	                    cmp = aL > bL ? 1 : -1;
	                } else {
	
	                    for ( i = cmp = 0; i < aL; i++ ) {
	
	                        if ( a[i] != b[i] ) {
	                            cmp = a[i] > b[i] ? 1 : -1;
	                            break;
	                        }
	                    }
	                }
	                return cmp;
	            }
	
	            function subtract( a, b, aL, base ) {
	                var i = 0;
	
	                // Subtract b from a.
	                for ( ; aL--; ) {
	                    a[aL] -= i;
	                    i = a[aL] < b[aL] ? 1 : 0;
	                    a[aL] = i * base + a[aL] - b[aL];
	                }
	
	                // Remove leading zeros.
	                for ( ; !a[0] && a.length > 1; a.shift() );
	            }
	
	            // x: dividend, y: divisor.
	            return function ( x, y, dp, rm, base ) {
	                var cmp, e, i, more, n, prod, prodL, q, qc, rem, remL, rem0, xi, xL, yc0,
	                    yL, yz,
	                    s = x.s == y.s ? 1 : -1,
	                    xc = x.c,
	                    yc = y.c;
	
	                // Either NaN, Infinity or 0?
	                if ( !xc || !xc[0] || !yc || !yc[0] ) {
	
	                    return new BigNumber(
	
	                      // Return NaN if either NaN, or both Infinity or 0.
	                      !x.s || !y.s || ( xc ? yc && xc[0] == yc[0] : !yc ) ? NaN :
	
	                        // Return ±0 if x is ±0 or y is ±Infinity, or return ±Infinity as y is ±0.
	                        xc && xc[0] == 0 || !yc ? s * 0 : s / 0
	                    );
	                }
	
	                q = new BigNumber(s);
	                qc = q.c = [];
	                e = x.e - y.e;
	                s = dp + e + 1;
	
	                if ( !base ) {
	                    base = BASE;
	                    e = bitFloor( x.e / LOG_BASE ) - bitFloor( y.e / LOG_BASE );
	                    s = s / LOG_BASE | 0;
	                }
	
	                // Result exponent may be one less then the current value of e.
	                // The coefficients of the BigNumbers from convertBase may have trailing zeros.
	                for ( i = 0; yc[i] == ( xc[i] || 0 ); i++ );
	                if ( yc[i] > ( xc[i] || 0 ) ) e--;
	
	                if ( s < 0 ) {
	                    qc.push(1);
	                    more = true;
	                } else {
	                    xL = xc.length;
	                    yL = yc.length;
	                    i = 0;
	                    s += 2;
	
	                    // Normalise xc and yc so highest order digit of yc is >= base / 2.
	
	                    n = mathfloor( base / ( yc[0] + 1 ) );
	
	                    // Not necessary, but to handle odd bases where yc[0] == ( base / 2 ) - 1.
	                    // if ( n > 1 || n++ == 1 && yc[0] < base / 2 ) {
	                    if ( n > 1 ) {
	                        yc = multiply( yc, n, base );
	                        xc = multiply( xc, n, base );
	                        yL = yc.length;
	                        xL = xc.length;
	                    }
	
	                    xi = yL;
	                    rem = xc.slice( 0, yL );
	                    remL = rem.length;
	
	                    // Add zeros to make remainder as long as divisor.
	                    for ( ; remL < yL; rem[remL++] = 0 );
	                    yz = yc.slice();
	                    yz.unshift(0);
	                    yc0 = yc[0];
	                    if ( yc[1] >= base / 2 ) yc0++;
	                    // Not necessary, but to prevent trial digit n > base, when using base 3.
	                    // else if ( base == 3 && yc0 == 1 ) yc0 = 1 + 1e-15;
	
	                    do {
	                        n = 0;
	
	                        // Compare divisor and remainder.
	                        cmp = compare( yc, rem, yL, remL );
	
	                        // If divisor < remainder.
	                        if ( cmp < 0 ) {
	
	                            // Calculate trial digit, n.
	
	                            rem0 = rem[0];
	                            if ( yL != remL ) rem0 = rem0 * base + ( rem[1] || 0 );
	
	                            // n is how many times the divisor goes into the current remainder.
	                            n = mathfloor( rem0 / yc0 );
	
	                            //  Algorithm:
	                            //  1. product = divisor * trial digit (n)
	                            //  2. if product > remainder: product -= divisor, n--
	                            //  3. remainder -= product
	                            //  4. if product was < remainder at 2:
	                            //    5. compare new remainder and divisor
	                            //    6. If remainder > divisor: remainder -= divisor, n++
	
	                            if ( n > 1 ) {
	
	                                // n may be > base only when base is 3.
	                                if (n >= base) n = base - 1;
	
	                                // product = divisor * trial digit.
	                                prod = multiply( yc, n, base );
	                                prodL = prod.length;
	                                remL = rem.length;
	
	                                // Compare product and remainder.
	                                // If product > remainder.
	                                // Trial digit n too high.
	                                // n is 1 too high about 5% of the time, and is not known to have
	                                // ever been more than 1 too high.
	                                while ( compare( prod, rem, prodL, remL ) == 1 ) {
	                                    n--;
	
	                                    // Subtract divisor from product.
	                                    subtract( prod, yL < prodL ? yz : yc, prodL, base );
	                                    prodL = prod.length;
	                                    cmp = 1;
	                                }
	                            } else {
	
	                                // n is 0 or 1, cmp is -1.
	                                // If n is 0, there is no need to compare yc and rem again below,
	                                // so change cmp to 1 to avoid it.
	                                // If n is 1, leave cmp as -1, so yc and rem are compared again.
	                                if ( n == 0 ) {
	
	                                    // divisor < remainder, so n must be at least 1.
	                                    cmp = n = 1;
	                                }
	
	                                // product = divisor
	                                prod = yc.slice();
	                                prodL = prod.length;
	                            }
	
	                            if ( prodL < remL ) prod.unshift(0);
	
	                            // Subtract product from remainder.
	                            subtract( rem, prod, remL, base );
	                            remL = rem.length;
	
	                             // If product was < remainder.
	                            if ( cmp == -1 ) {
	
	                                // Compare divisor and new remainder.
	                                // If divisor < new remainder, subtract divisor from remainder.
	                                // Trial digit n too low.
	                                // n is 1 too low about 5% of the time, and very rarely 2 too low.
	                                while ( compare( yc, rem, yL, remL ) < 1 ) {
	                                    n++;
	
	                                    // Subtract divisor from remainder.
	                                    subtract( rem, yL < remL ? yz : yc, remL, base );
	                                    remL = rem.length;
	                                }
	                            }
	                        } else if ( cmp === 0 ) {
	                            n++;
	                            rem = [0];
	                        } // else cmp === 1 and n will be 0
	
	                        // Add the next digit, n, to the result array.
	                        qc[i++] = n;
	
	                        // Update the remainder.
	                        if ( rem[0] ) {
	                            rem[remL++] = xc[xi] || 0;
	                        } else {
	                            rem = [ xc[xi] ];
	                            remL = 1;
	                        }
	                    } while ( ( xi++ < xL || rem[0] != null ) && s-- );
	
	                    more = rem[0] != null;
	
	                    // Leading zero?
	                    if ( !qc[0] ) qc.shift();
	                }
	
	                if ( base == BASE ) {
	
	                    // To calculate q.e, first get the number of digits of qc[0].
	                    for ( i = 1, s = qc[0]; s >= 10; s /= 10, i++ );
	                    round( q, dp + ( q.e = i + e * LOG_BASE - 1 ) + 1, rm, more );
	
	                // Caller is convertBase.
	                } else {
	                    q.e = e;
	                    q.r = +more;
	                }
	
	                return q;
	            };
	        })();
	
	
	        /*
	         * Return a string representing the value of BigNumber n in fixed-point or exponential
	         * notation rounded to the specified decimal places or significant digits.
	         *
	         * n is a BigNumber.
	         * i is the index of the last digit required (i.e. the digit that may be rounded up).
	         * rm is the rounding mode.
	         * caller is caller id: toExponential 19, toFixed 20, toFormat 21, toPrecision 24.
	         */
	        function format( n, i, rm, caller ) {
	            var c0, e, ne, len, str;
	
	            rm = rm != null && isValidInt( rm, 0, 8, caller, roundingMode )
	              ? rm | 0 : ROUNDING_MODE;
	
	            if ( !n.c ) return n.toString();
	            c0 = n.c[0];
	            ne = n.e;
	
	            if ( i == null ) {
	                str = coeffToString( n.c );
	                str = caller == 19 || caller == 24 && ne <= TO_EXP_NEG
	                  ? toExponential( str, ne )
	                  : toFixedPoint( str, ne );
	            } else {
	                n = round( new BigNumber(n), i, rm );
	
	                // n.e may have changed if the value was rounded up.
	                e = n.e;
	
	                str = coeffToString( n.c );
	                len = str.length;
	
	                // toPrecision returns exponential notation if the number of significant digits
	                // specified is less than the number of digits necessary to represent the integer
	                // part of the value in fixed-point notation.
	
	                // Exponential notation.
	                if ( caller == 19 || caller == 24 && ( i <= e || e <= TO_EXP_NEG ) ) {
	
	                    // Append zeros?
	                    for ( ; len < i; str += '0', len++ );
	                    str = toExponential( str, e );
	
	                // Fixed-point notation.
	                } else {
	                    i -= ne;
	                    str = toFixedPoint( str, e );
	
	                    // Append zeros?
	                    if ( e + 1 > len ) {
	                        if ( --i > 0 ) for ( str += '.'; i--; str += '0' );
	                    } else {
	                        i += e - len;
	                        if ( i > 0 ) {
	                            if ( e + 1 == len ) str += '.';
	                            for ( ; i--; str += '0' );
	                        }
	                    }
	                }
	            }
	
	            return n.s < 0 && c0 ? '-' + str : str;
	        }
	
	
	        // Handle BigNumber.max and BigNumber.min.
	        function maxOrMin( args, method ) {
	            var m, n,
	                i = 0;
	
	            if ( isArray( args[0] ) ) args = args[0];
	            m = new BigNumber( args[0] );
	
	            for ( ; ++i < args.length; ) {
	                n = new BigNumber( args[i] );
	
	                // If any number is NaN, return NaN.
	                if ( !n.s ) {
	                    m = n;
	                    break;
	                } else if ( method.call( m, n ) ) {
	                    m = n;
	                }
	            }
	
	            return m;
	        }
	
	
	        /*
	         * Return true if n is an integer in range, otherwise throw.
	         * Use for argument validation when ERRORS is true.
	         */
	        function intValidatorWithErrors( n, min, max, caller, name ) {
	            if ( n < min || n > max || n != truncate(n) ) {
	                raise( caller, ( name || 'decimal places' ) +
	                  ( n < min || n > max ? ' out of range' : ' not an integer' ), n );
	            }
	
	            return true;
	        }
	
	
	        /*
	         * Strip trailing zeros, calculate base 10 exponent and check against MIN_EXP and MAX_EXP.
	         * Called by minus, plus and times.
	         */
	        function normalise( n, c, e ) {
	            var i = 1,
	                j = c.length;
	
	             // Remove trailing zeros.
	            for ( ; !c[--j]; c.pop() );
	
	            // Calculate the base 10 exponent. First get the number of digits of c[0].
	            for ( j = c[0]; j >= 10; j /= 10, i++ );
	
	            // Overflow?
	            if ( ( e = i + e * LOG_BASE - 1 ) > MAX_EXP ) {
	
	                // Infinity.
	                n.c = n.e = null;
	
	            // Underflow?
	            } else if ( e < MIN_EXP ) {
	
	                // Zero.
	                n.c = [ n.e = 0 ];
	            } else {
	                n.e = e;
	                n.c = c;
	            }
	
	            return n;
	        }
	
	
	        // Handle values that fail the validity test in BigNumber.
	        parseNumeric = (function () {
	            var basePrefix = /^(-?)0([xbo])(?=\w[\w.]*$)/i,
	                dotAfter = /^([^.]+)\.$/,
	                dotBefore = /^\.([^.]+)$/,
	                isInfinityOrNaN = /^-?(Infinity|NaN)$/,
	                whitespaceOrPlus = /^\s*\+(?=[\w.])|^\s+|\s+$/g;
	
	            return function ( x, str, num, b ) {
	                var base,
	                    s = num ? str : str.replace( whitespaceOrPlus, '' );
	
	                // No exception on ±Infinity or NaN.
	                if ( isInfinityOrNaN.test(s) ) {
	                    x.s = isNaN(s) ? null : s < 0 ? -1 : 1;
	                } else {
	                    if ( !num ) {
	
	                        // basePrefix = /^(-?)0([xbo])(?=\w[\w.]*$)/i
	                        s = s.replace( basePrefix, function ( m, p1, p2 ) {
	                            base = ( p2 = p2.toLowerCase() ) == 'x' ? 16 : p2 == 'b' ? 2 : 8;
	                            return !b || b == base ? p1 : m;
	                        });
	
	                        if (b) {
	                            base = b;
	
	                            // E.g. '1.' to '1', '.1' to '0.1'
	                            s = s.replace( dotAfter, '$1' ).replace( dotBefore, '0.$1' );
	                        }
	
	                        if ( str != s ) return new BigNumber( s, base );
	                    }
	
	                    // 'new BigNumber() not a number: {n}'
	                    // 'new BigNumber() not a base {b} number: {n}'
	                    if (ERRORS) raise( id, 'not a' + ( b ? ' base ' + b : '' ) + ' number', str );
	                    x.s = null;
	                }
	
	                x.c = x.e = null;
	                id = 0;
	            }
	        })();
	
	
	        // Throw a BigNumber Error.
	        function raise( caller, msg, val ) {
	            var error = new Error( [
	                'new BigNumber',     // 0
	                'cmp',               // 1
	                'config',            // 2
	                'div',               // 3
	                'divToInt',          // 4
	                'eq',                // 5
	                'gt',                // 6
	                'gte',               // 7
	                'lt',                // 8
	                'lte',               // 9
	                'minus',             // 10
	                'mod',               // 11
	                'plus',              // 12
	                'precision',         // 13
	                'random',            // 14
	                'round',             // 15
	                'shift',             // 16
	                'times',             // 17
	                'toDigits',          // 18
	                'toExponential',     // 19
	                'toFixed',           // 20
	                'toFormat',          // 21
	                'toFraction',        // 22
	                'pow',               // 23
	                'toPrecision',       // 24
	                'toString',          // 25
	                'BigNumber'          // 26
	            ][caller] + '() ' + msg + ': ' + val );
	
	            error.name = 'BigNumber Error';
	            id = 0;
	            throw error;
	        }
	
	
	        /*
	         * Round x to sd significant digits using rounding mode rm. Check for over/under-flow.
	         * If r is truthy, it is known that there are more digits after the rounding digit.
	         */
	        function round( x, sd, rm, r ) {
	            var d, i, j, k, n, ni, rd,
	                xc = x.c,
	                pows10 = POWS_TEN;
	
	            // if x is not Infinity or NaN...
	            if (xc) {
	
	                // rd is the rounding digit, i.e. the digit after the digit that may be rounded up.
	                // n is a base 1e14 number, the value of the element of array x.c containing rd.
	                // ni is the index of n within x.c.
	                // d is the number of digits of n.
	                // i is the index of rd within n including leading zeros.
	                // j is the actual index of rd within n (if < 0, rd is a leading zero).
	                out: {
	
	                    // Get the number of digits of the first element of xc.
	                    for ( d = 1, k = xc[0]; k >= 10; k /= 10, d++ );
	                    i = sd - d;
	
	                    // If the rounding digit is in the first element of xc...
	                    if ( i < 0 ) {
	                        i += LOG_BASE;
	                        j = sd;
	                        n = xc[ ni = 0 ];
	
	                        // Get the rounding digit at index j of n.
	                        rd = n / pows10[ d - j - 1 ] % 10 | 0;
	                    } else {
	                        ni = mathceil( ( i + 1 ) / LOG_BASE );
	
	                        if ( ni >= xc.length ) {
	
	                            if (r) {
	
	                                // Needed by sqrt.
	                                for ( ; xc.length <= ni; xc.push(0) );
	                                n = rd = 0;
	                                d = 1;
	                                i %= LOG_BASE;
	                                j = i - LOG_BASE + 1;
	                            } else {
	                                break out;
	                            }
	                        } else {
	                            n = k = xc[ni];
	
	                            // Get the number of digits of n.
	                            for ( d = 1; k >= 10; k /= 10, d++ );
	
	                            // Get the index of rd within n.
	                            i %= LOG_BASE;
	
	                            // Get the index of rd within n, adjusted for leading zeros.
	                            // The number of leading zeros of n is given by LOG_BASE - d.
	                            j = i - LOG_BASE + d;
	
	                            // Get the rounding digit at index j of n.
	                            rd = j < 0 ? 0 : n / pows10[ d - j - 1 ] % 10 | 0;
	                        }
	                    }
	
	                    r = r || sd < 0 ||
	
	                    // Are there any non-zero digits after the rounding digit?
	                    // The expression  n % pows10[ d - j - 1 ]  returns all digits of n to the right
	                    // of the digit at j, e.g. if n is 908714 and j is 2, the expression gives 714.
	                      xc[ni + 1] != null || ( j < 0 ? n : n % pows10[ d - j - 1 ] );
	
	                    r = rm < 4
	                      ? ( rd || r ) && ( rm == 0 || rm == ( x.s < 0 ? 3 : 2 ) )
	                      : rd > 5 || rd == 5 && ( rm == 4 || r || rm == 6 &&
	
	                        // Check whether the digit to the left of the rounding digit is odd.
	                        ( ( i > 0 ? j > 0 ? n / pows10[ d - j ] : 0 : xc[ni - 1] ) % 10 ) & 1 ||
	                          rm == ( x.s < 0 ? 8 : 7 ) );
	
	                    if ( sd < 1 || !xc[0] ) {
	                        xc.length = 0;
	
	                        if (r) {
	
	                            // Convert sd to decimal places.
	                            sd -= x.e + 1;
	
	                            // 1, 0.1, 0.01, 0.001, 0.0001 etc.
	                            xc[0] = pows10[ ( LOG_BASE - sd % LOG_BASE ) % LOG_BASE ];
	                            x.e = -sd || 0;
	                        } else {
	
	                            // Zero.
	                            xc[0] = x.e = 0;
	                        }
	
	                        return x;
	                    }
	
	                    // Remove excess digits.
	                    if ( i == 0 ) {
	                        xc.length = ni;
	                        k = 1;
	                        ni--;
	                    } else {
	                        xc.length = ni + 1;
	                        k = pows10[ LOG_BASE - i ];
	
	                        // E.g. 56700 becomes 56000 if 7 is the rounding digit.
	                        // j > 0 means i > number of leading zeros of n.
	                        xc[ni] = j > 0 ? mathfloor( n / pows10[ d - j ] % pows10[j] ) * k : 0;
	                    }
	
	                    // Round up?
	                    if (r) {
	
	                        for ( ; ; ) {
	
	                            // If the digit to be rounded up is in the first element of xc...
	                            if ( ni == 0 ) {
	
	                                // i will be the length of xc[0] before k is added.
	                                for ( i = 1, j = xc[0]; j >= 10; j /= 10, i++ );
	                                j = xc[0] += k;
	                                for ( k = 1; j >= 10; j /= 10, k++ );
	
	                                // if i != k the length has increased.
	                                if ( i != k ) {
	                                    x.e++;
	                                    if ( xc[0] == BASE ) xc[0] = 1;
	                                }
	
	                                break;
	                            } else {
	                                xc[ni] += k;
	                                if ( xc[ni] != BASE ) break;
	                                xc[ni--] = 0;
	                                k = 1;
	                            }
	                        }
	                    }
	
	                    // Remove trailing zeros.
	                    for ( i = xc.length; xc[--i] === 0; xc.pop() );
	                }
	
	                // Overflow? Infinity.
	                if ( x.e > MAX_EXP ) {
	                    x.c = x.e = null;
	
	                // Underflow? Zero.
	                } else if ( x.e < MIN_EXP ) {
	                    x.c = [ x.e = 0 ];
	                }
	            }
	
	            return x;
	        }
	
	
	        // PROTOTYPE/INSTANCE METHODS
	
	
	        /*
	         * Return a new BigNumber whose value is the absolute value of this BigNumber.
	         */
	        P.absoluteValue = P.abs = function () {
	            var x = new BigNumber(this);
	            if ( x.s < 0 ) x.s = 1;
	            return x;
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber rounded to a whole
	         * number in the direction of Infinity.
	         */
	        P.ceil = function () {
	            return round( new BigNumber(this), this.e + 1, 2 );
	        };
	
	
	        /*
	         * Return
	         * 1 if the value of this BigNumber is greater than the value of BigNumber(y, b),
	         * -1 if the value of this BigNumber is less than the value of BigNumber(y, b),
	         * 0 if they have the same value,
	         * or null if the value of either is NaN.
	         */
	        P.comparedTo = P.cmp = function ( y, b ) {
	            id = 1;
	            return compare( this, new BigNumber( y, b ) );
	        };
	
	
	        /*
	         * Return the number of decimal places of the value of this BigNumber, or null if the value
	         * of this BigNumber is ±Infinity or NaN.
	         */
	        P.decimalPlaces = P.dp = function () {
	            var n, v,
	                c = this.c;
	
	            if ( !c ) return null;
	            n = ( ( v = c.length - 1 ) - bitFloor( this.e / LOG_BASE ) ) * LOG_BASE;
	
	            // Subtract the number of trailing zeros of the last number.
	            if ( v = c[v] ) for ( ; v % 10 == 0; v /= 10, n-- );
	            if ( n < 0 ) n = 0;
	
	            return n;
	        };
	
	
	        /*
	         *  n / 0 = I
	         *  n / N = N
	         *  n / I = 0
	         *  0 / n = 0
	         *  0 / 0 = N
	         *  0 / N = N
	         *  0 / I = 0
	         *  N / n = N
	         *  N / 0 = N
	         *  N / N = N
	         *  N / I = N
	         *  I / n = I
	         *  I / 0 = I
	         *  I / N = N
	         *  I / I = N
	         *
	         * Return a new BigNumber whose value is the value of this BigNumber divided by the value of
	         * BigNumber(y, b), rounded according to DECIMAL_PLACES and ROUNDING_MODE.
	         */
	        P.dividedBy = P.div = function ( y, b ) {
	            id = 3;
	            return div( this, new BigNumber( y, b ), DECIMAL_PLACES, ROUNDING_MODE );
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the integer part of dividing the value of this
	         * BigNumber by the value of BigNumber(y, b).
	         */
	        P.dividedToIntegerBy = P.divToInt = function ( y, b ) {
	            id = 4;
	            return div( this, new BigNumber( y, b ), 0, 1 );
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is equal to the value of BigNumber(y, b),
	         * otherwise returns false.
	         */
	        P.equals = P.eq = function ( y, b ) {
	            id = 5;
	            return compare( this, new BigNumber( y, b ) ) === 0;
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber rounded to a whole
	         * number in the direction of -Infinity.
	         */
	        P.floor = function () {
	            return round( new BigNumber(this), this.e + 1, 3 );
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is greater than the value of BigNumber(y, b),
	         * otherwise returns false.
	         */
	        P.greaterThan = P.gt = function ( y, b ) {
	            id = 6;
	            return compare( this, new BigNumber( y, b ) ) > 0;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is greater than or equal to the value of
	         * BigNumber(y, b), otherwise returns false.
	         */
	        P.greaterThanOrEqualTo = P.gte = function ( y, b ) {
	            id = 7;
	            return ( b = compare( this, new BigNumber( y, b ) ) ) === 1 || b === 0;
	
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is a finite number, otherwise returns false.
	         */
	        P.isFinite = function () {
	            return !!this.c;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is an integer, otherwise return false.
	         */
	        P.isInteger = P.isInt = function () {
	            return !!this.c && bitFloor( this.e / LOG_BASE ) > this.c.length - 2;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is NaN, otherwise returns false.
	         */
	        P.isNaN = function () {
	            return !this.s;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is negative, otherwise returns false.
	         */
	        P.isNegative = P.isNeg = function () {
	            return this.s < 0;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is 0 or -0, otherwise returns false.
	         */
	        P.isZero = function () {
	            return !!this.c && this.c[0] == 0;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is less than the value of BigNumber(y, b),
	         * otherwise returns false.
	         */
	        P.lessThan = P.lt = function ( y, b ) {
	            id = 8;
	            return compare( this, new BigNumber( y, b ) ) < 0;
	        };
	
	
	        /*
	         * Return true if the value of this BigNumber is less than or equal to the value of
	         * BigNumber(y, b), otherwise returns false.
	         */
	        P.lessThanOrEqualTo = P.lte = function ( y, b ) {
	            id = 9;
	            return ( b = compare( this, new BigNumber( y, b ) ) ) === -1 || b === 0;
	        };
	
	
	        /*
	         *  n - 0 = n
	         *  n - N = N
	         *  n - I = -I
	         *  0 - n = -n
	         *  0 - 0 = 0
	         *  0 - N = N
	         *  0 - I = -I
	         *  N - n = N
	         *  N - 0 = N
	         *  N - N = N
	         *  N - I = N
	         *  I - n = I
	         *  I - 0 = I
	         *  I - N = N
	         *  I - I = N
	         *
	         * Return a new BigNumber whose value is the value of this BigNumber minus the value of
	         * BigNumber(y, b).
	         */
	        P.minus = P.sub = function ( y, b ) {
	            var i, j, t, xLTy,
	                x = this,
	                a = x.s;
	
	            id = 10;
	            y = new BigNumber( y, b );
	            b = y.s;
	
	            // Either NaN?
	            if ( !a || !b ) return new BigNumber(NaN);
	
	            // Signs differ?
	            if ( a != b ) {
	                y.s = -b;
	                return x.plus(y);
	            }
	
	            var xe = x.e / LOG_BASE,
	                ye = y.e / LOG_BASE,
	                xc = x.c,
	                yc = y.c;
	
	            if ( !xe || !ye ) {
	
	                // Either Infinity?
	                if ( !xc || !yc ) return xc ? ( y.s = -b, y ) : new BigNumber( yc ? x : NaN );
	
	                // Either zero?
	                if ( !xc[0] || !yc[0] ) {
	
	                    // Return y if y is non-zero, x if x is non-zero, or zero if both are zero.
	                    return yc[0] ? ( y.s = -b, y ) : new BigNumber( xc[0] ? x :
	
	                      // IEEE 754 (2008) 6.3: n - n = -0 when rounding to -Infinity
	                      ROUNDING_MODE == 3 ? -0 : 0 );
	                }
	            }
	
	            xe = bitFloor(xe);
	            ye = bitFloor(ye);
	            xc = xc.slice();
	
	            // Determine which is the bigger number.
	            if ( a = xe - ye ) {
	
	                if ( xLTy = a < 0 ) {
	                    a = -a;
	                    t = xc;
	                } else {
	                    ye = xe;
	                    t = yc;
	                }
	
	                t.reverse();
	
	                // Prepend zeros to equalise exponents.
	                for ( b = a; b--; t.push(0) );
	                t.reverse();
	            } else {
	
	                // Exponents equal. Check digit by digit.
	                j = ( xLTy = ( a = xc.length ) < ( b = yc.length ) ) ? a : b;
	
	                for ( a = b = 0; b < j; b++ ) {
	
	                    if ( xc[b] != yc[b] ) {
	                        xLTy = xc[b] < yc[b];
	                        break;
	                    }
	                }
	            }
	
	            // x < y? Point xc to the array of the bigger number.
	            if (xLTy) t = xc, xc = yc, yc = t, y.s = -y.s;
	
	            b = ( j = yc.length ) - ( i = xc.length );
	
	            // Append zeros to xc if shorter.
	            // No need to add zeros to yc if shorter as subtract only needs to start at yc.length.
	            if ( b > 0 ) for ( ; b--; xc[i++] = 0 );
	            b = BASE - 1;
	
	            // Subtract yc from xc.
	            for ( ; j > a; ) {
	
	                if ( xc[--j] < yc[j] ) {
	                    for ( i = j; i && !xc[--i]; xc[i] = b );
	                    --xc[i];
	                    xc[j] += BASE;
	                }
	
	                xc[j] -= yc[j];
	            }
	
	            // Remove leading zeros and adjust exponent accordingly.
	            for ( ; xc[0] == 0; xc.shift(), --ye );
	
	            // Zero?
	            if ( !xc[0] ) {
	
	                // Following IEEE 754 (2008) 6.3,
	                // n - n = +0  but  n - n = -0  when rounding towards -Infinity.
	                y.s = ROUNDING_MODE == 3 ? -1 : 1;
	                y.c = [ y.e = 0 ];
	                return y;
	            }
	
	            // No need to check for Infinity as +x - +y != Infinity && -x - -y != Infinity
	            // for finite x and y.
	            return normalise( y, xc, ye );
	        };
	
	
	        /*
	         *   n % 0 =  N
	         *   n % N =  N
	         *   n % I =  n
	         *   0 % n =  0
	         *  -0 % n = -0
	         *   0 % 0 =  N
	         *   0 % N =  N
	         *   0 % I =  0
	         *   N % n =  N
	         *   N % 0 =  N
	         *   N % N =  N
	         *   N % I =  N
	         *   I % n =  N
	         *   I % 0 =  N
	         *   I % N =  N
	         *   I % I =  N
	         *
	         * Return a new BigNumber whose value is the value of this BigNumber modulo the value of
	         * BigNumber(y, b). The result depends on the value of MODULO_MODE.
	         */
	        P.modulo = P.mod = function ( y, b ) {
	            var q, s,
	                x = this;
	
	            id = 11;
	            y = new BigNumber( y, b );
	
	            // Return NaN if x is Infinity or NaN, or y is NaN or zero.
	            if ( !x.c || !y.s || y.c && !y.c[0] ) {
	                return new BigNumber(NaN);
	
	            // Return x if y is Infinity or x is zero.
	            } else if ( !y.c || x.c && !x.c[0] ) {
	                return new BigNumber(x);
	            }
	
	            if ( MODULO_MODE == 9 ) {
	
	                // Euclidian division: q = sign(y) * floor(x / abs(y))
	                // r = x - qy    where  0 <= r < abs(y)
	                s = y.s;
	                y.s = 1;
	                q = div( x, y, 0, 3 );
	                y.s = s;
	                q.s *= s;
	            } else {
	                q = div( x, y, 0, MODULO_MODE );
	            }
	
	            return x.minus( q.times(y) );
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber negated,
	         * i.e. multiplied by -1.
	         */
	        P.negated = P.neg = function () {
	            var x = new BigNumber(this);
	            x.s = -x.s || null;
	            return x;
	        };
	
	
	        /*
	         *  n + 0 = n
	         *  n + N = N
	         *  n + I = I
	         *  0 + n = n
	         *  0 + 0 = 0
	         *  0 + N = N
	         *  0 + I = I
	         *  N + n = N
	         *  N + 0 = N
	         *  N + N = N
	         *  N + I = N
	         *  I + n = I
	         *  I + 0 = I
	         *  I + N = N
	         *  I + I = I
	         *
	         * Return a new BigNumber whose value is the value of this BigNumber plus the value of
	         * BigNumber(y, b).
	         */
	        P.plus = P.add = function ( y, b ) {
	            var t,
	                x = this,
	                a = x.s;
	
	            id = 12;
	            y = new BigNumber( y, b );
	            b = y.s;
	
	            // Either NaN?
	            if ( !a || !b ) return new BigNumber(NaN);
	
	            // Signs differ?
	             if ( a != b ) {
	                y.s = -b;
	                return x.minus(y);
	            }
	
	            var xe = x.e / LOG_BASE,
	                ye = y.e / LOG_BASE,
	                xc = x.c,
	                yc = y.c;
	
	            if ( !xe || !ye ) {
	
	                // Return ±Infinity if either ±Infinity.
	                if ( !xc || !yc ) return new BigNumber( a / 0 );
	
	                // Either zero?
	                // Return y if y is non-zero, x if x is non-zero, or zero if both are zero.
	                if ( !xc[0] || !yc[0] ) return yc[0] ? y : new BigNumber( xc[0] ? x : a * 0 );
	            }
	
	            xe = bitFloor(xe);
	            ye = bitFloor(ye);
	            xc = xc.slice();
	
	            // Prepend zeros to equalise exponents. Faster to use reverse then do unshifts.
	            if ( a = xe - ye ) {
	                if ( a > 0 ) {
	                    ye = xe;
	                    t = yc;
	                } else {
	                    a = -a;
	                    t = xc;
	                }
	
	                t.reverse();
	                for ( ; a--; t.push(0) );
	                t.reverse();
	            }
	
	            a = xc.length;
	            b = yc.length;
	
	            // Point xc to the longer array, and b to the shorter length.
	            if ( a - b < 0 ) t = yc, yc = xc, xc = t, b = a;
	
	            // Only start adding at yc.length - 1 as the further digits of xc can be ignored.
	            for ( a = 0; b; ) {
	                a = ( xc[--b] = xc[b] + yc[b] + a ) / BASE | 0;
	                xc[b] %= BASE;
	            }
	
	            if (a) {
	                xc.unshift(a);
	                ++ye;
	            }
	
	            // No need to check for zero, as +x + +y != 0 && -x + -y != 0
	            // ye = MAX_EXP + 1 possible
	            return normalise( y, xc, ye );
	        };
	
	
	        /*
	         * Return the number of significant digits of the value of this BigNumber.
	         *
	         * [z] {boolean|number} Whether to count integer-part trailing zeros: true, false, 1 or 0.
	         */
	        P.precision = P.sd = function (z) {
	            var n, v,
	                x = this,
	                c = x.c;
	
	            // 'precision() argument not a boolean or binary digit: {z}'
	            if ( z != null && z !== !!z && z !== 1 && z !== 0 ) {
	                if (ERRORS) raise( 13, 'argument' + notBool, z );
	                if ( z != !!z ) z = null;
	            }
	
	            if ( !c ) return null;
	            v = c.length - 1;
	            n = v * LOG_BASE + 1;
	
	            if ( v = c[v] ) {
	
	                // Subtract the number of trailing zeros of the last element.
	                for ( ; v % 10 == 0; v /= 10, n-- );
	
	                // Add the number of digits of the first element.
	                for ( v = c[0]; v >= 10; v /= 10, n++ );
	            }
	
	            if ( z && x.e + 1 > n ) n = x.e + 1;
	
	            return n;
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber rounded to a maximum of
	         * dp decimal places using rounding mode rm, or to 0 and ROUNDING_MODE respectively if
	         * omitted.
	         *
	         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
	         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
	         *
	         * 'round() decimal places out of range: {dp}'
	         * 'round() decimal places not an integer: {dp}'
	         * 'round() rounding mode not an integer: {rm}'
	         * 'round() rounding mode out of range: {rm}'
	         */
	        P.round = function ( dp, rm ) {
	            var n = new BigNumber(this);
	
	            if ( dp == null || isValidInt( dp, 0, MAX, 15 ) ) {
	                round( n, ~~dp + this.e + 1, rm == null ||
	                  !isValidInt( rm, 0, 8, 15, roundingMode ) ? ROUNDING_MODE : rm | 0 );
	            }
	
	            return n;
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber shifted by k places
	         * (powers of 10). Shift to the right if n > 0, and to the left if n < 0.
	         *
	         * k {number} Integer, -MAX_SAFE_INTEGER to MAX_SAFE_INTEGER inclusive.
	         *
	         * If k is out of range and ERRORS is false, the result will be ±0 if k < 0, or ±Infinity
	         * otherwise.
	         *
	         * 'shift() argument not an integer: {k}'
	         * 'shift() argument out of range: {k}'
	         */
	        P.shift = function (k) {
	            var n = this;
	            return isValidInt( k, -MAX_SAFE_INTEGER, MAX_SAFE_INTEGER, 16, 'argument' )
	
	              // k < 1e+21, or truncate(k) will produce exponential notation.
	              ? n.times( '1e' + truncate(k) )
	              : new BigNumber( n.c && n.c[0] && ( k < -MAX_SAFE_INTEGER || k > MAX_SAFE_INTEGER )
	                ? n.s * ( k < 0 ? 0 : 1 / 0 )
	                : n );
	        };
	
	
	        /*
	         *  sqrt(-n) =  N
	         *  sqrt( N) =  N
	         *  sqrt(-I) =  N
	         *  sqrt( I) =  I
	         *  sqrt( 0) =  0
	         *  sqrt(-0) = -0
	         *
	         * Return a new BigNumber whose value is the square root of the value of this BigNumber,
	         * rounded according to DECIMAL_PLACES and ROUNDING_MODE.
	         */
	        P.squareRoot = P.sqrt = function () {
	            var m, n, r, rep, t,
	                x = this,
	                c = x.c,
	                s = x.s,
	                e = x.e,
	                dp = DECIMAL_PLACES + 4,
	                half = new BigNumber('0.5');
	
	            // Negative/NaN/Infinity/zero?
	            if ( s !== 1 || !c || !c[0] ) {
	                return new BigNumber( !s || s < 0 && ( !c || c[0] ) ? NaN : c ? x : 1 / 0 );
	            }
	
	            // Initial estimate.
	            s = Math.sqrt( +x );
	
	            // Math.sqrt underflow/overflow?
	            // Pass x to Math.sqrt as integer, then adjust the exponent of the result.
	            if ( s == 0 || s == 1 / 0 ) {
	                n = coeffToString(c);
	                if ( ( n.length + e ) % 2 == 0 ) n += '0';
	                s = Math.sqrt(n);
	                e = bitFloor( ( e + 1 ) / 2 ) - ( e < 0 || e % 2 );
	
	                if ( s == 1 / 0 ) {
	                    n = '1e' + e;
	                } else {
	                    n = s.toExponential();
	                    n = n.slice( 0, n.indexOf('e') + 1 ) + e;
	                }
	
	                r = new BigNumber(n);
	            } else {
	                r = new BigNumber( s + '' );
	            }
	
	            // Check for zero.
	            // r could be zero if MIN_EXP is changed after the this value was created.
	            // This would cause a division by zero (x/t) and hence Infinity below, which would cause
	            // coeffToString to throw.
	            if ( r.c[0] ) {
	                e = r.e;
	                s = e + dp;
	                if ( s < 3 ) s = 0;
	
	                // Newton-Raphson iteration.
	                for ( ; ; ) {
	                    t = r;
	                    r = half.times( t.plus( div( x, t, dp, 1 ) ) );
	
	                    if ( coeffToString( t.c   ).slice( 0, s ) === ( n =
	                         coeffToString( r.c ) ).slice( 0, s ) ) {
	
	                        // The exponent of r may here be one less than the final result exponent,
	                        // e.g 0.0009999 (e-4) --> 0.001 (e-3), so adjust s so the rounding digits
	                        // are indexed correctly.
	                        if ( r.e < e ) --s;
	                        n = n.slice( s - 3, s + 1 );
	
	                        // The 4th rounding digit may be in error by -1 so if the 4 rounding digits
	                        // are 9999 or 4999 (i.e. approaching a rounding boundary) continue the
	                        // iteration.
	                        if ( n == '9999' || !rep && n == '4999' ) {
	
	                            // On the first iteration only, check to see if rounding up gives the
	                            // exact result as the nines may infinitely repeat.
	                            if ( !rep ) {
	                                round( t, t.e + DECIMAL_PLACES + 2, 0 );
	
	                                if ( t.times(t).eq(x) ) {
	                                    r = t;
	                                    break;
	                                }
	                            }
	
	                            dp += 4;
	                            s += 4;
	                            rep = 1;
	                        } else {
	
	                            // If rounding digits are null, 0{0,4} or 50{0,3}, check for exact
	                            // result. If not, then there are further digits and m will be truthy.
	                            if ( !+n || !+n.slice(1) && n.charAt(0) == '5' ) {
	
	                                // Truncate to the first rounding digit.
	                                round( r, r.e + DECIMAL_PLACES + 2, 1 );
	                                m = !r.times(r).eq(x);
	                            }
	
	                            break;
	                        }
	                    }
	                }
	            }
	
	            return round( r, r.e + DECIMAL_PLACES + 1, ROUNDING_MODE, m );
	        };
	
	
	        /*
	         *  n * 0 = 0
	         *  n * N = N
	         *  n * I = I
	         *  0 * n = 0
	         *  0 * 0 = 0
	         *  0 * N = N
	         *  0 * I = N
	         *  N * n = N
	         *  N * 0 = N
	         *  N * N = N
	         *  N * I = N
	         *  I * n = I
	         *  I * 0 = N
	         *  I * N = N
	         *  I * I = I
	         *
	         * Return a new BigNumber whose value is the value of this BigNumber times the value of
	         * BigNumber(y, b).
	         */
	        P.times = P.mul = function ( y, b ) {
	            var c, e, i, j, k, m, xcL, xlo, xhi, ycL, ylo, yhi, zc,
	                base, sqrtBase,
	                x = this,
	                xc = x.c,
	                yc = ( id = 17, y = new BigNumber( y, b ) ).c;
	
	            // Either NaN, ±Infinity or ±0?
	            if ( !xc || !yc || !xc[0] || !yc[0] ) {
	
	                // Return NaN if either is NaN, or one is 0 and the other is Infinity.
	                if ( !x.s || !y.s || xc && !xc[0] && !yc || yc && !yc[0] && !xc ) {
	                    y.c = y.e = y.s = null;
	                } else {
	                    y.s *= x.s;
	
	                    // Return ±Infinity if either is ±Infinity.
	                    if ( !xc || !yc ) {
	                        y.c = y.e = null;
	
	                    // Return ±0 if either is ±0.
	                    } else {
	                        y.c = [0];
	                        y.e = 0;
	                    }
	                }
	
	                return y;
	            }
	
	            e = bitFloor( x.e / LOG_BASE ) + bitFloor( y.e / LOG_BASE );
	            y.s *= x.s;
	            xcL = xc.length;
	            ycL = yc.length;
	
	            // Ensure xc points to longer array and xcL to its length.
	            if ( xcL < ycL ) zc = xc, xc = yc, yc = zc, i = xcL, xcL = ycL, ycL = i;
	
	            // Initialise the result array with zeros.
	            for ( i = xcL + ycL, zc = []; i--; zc.push(0) );
	
	            base = BASE;
	            sqrtBase = SQRT_BASE;
	
	            for ( i = ycL; --i >= 0; ) {
	                c = 0;
	                ylo = yc[i] % sqrtBase;
	                yhi = yc[i] / sqrtBase | 0;
	
	                for ( k = xcL, j = i + k; j > i; ) {
	                    xlo = xc[--k] % sqrtBase;
	                    xhi = xc[k] / sqrtBase | 0;
	                    m = yhi * xlo + xhi * ylo;
	                    xlo = ylo * xlo + ( ( m % sqrtBase ) * sqrtBase ) + zc[j] + c;
	                    c = ( xlo / base | 0 ) + ( m / sqrtBase | 0 ) + yhi * xhi;
	                    zc[j--] = xlo % base;
	                }
	
	                zc[j] = c;
	            }
	
	            if (c) {
	                ++e;
	            } else {
	                zc.shift();
	            }
	
	            return normalise( y, zc, e );
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber rounded to a maximum of
	         * sd significant digits using rounding mode rm, or ROUNDING_MODE if rm is omitted.
	         *
	         * [sd] {number} Significant digits. Integer, 1 to MAX inclusive.
	         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
	         *
	         * 'toDigits() precision out of range: {sd}'
	         * 'toDigits() precision not an integer: {sd}'
	         * 'toDigits() rounding mode not an integer: {rm}'
	         * 'toDigits() rounding mode out of range: {rm}'
	         */
	        P.toDigits = function ( sd, rm ) {
	            var n = new BigNumber(this);
	            sd = sd == null || !isValidInt( sd, 1, MAX, 18, 'precision' ) ? null : sd | 0;
	            rm = rm == null || !isValidInt( rm, 0, 8, 18, roundingMode ) ? ROUNDING_MODE : rm | 0;
	            return sd ? round( n, sd, rm ) : n;
	        };
	
	
	        /*
	         * Return a string representing the value of this BigNumber in exponential notation and
	         * rounded using ROUNDING_MODE to dp fixed decimal places.
	         *
	         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
	         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
	         *
	         * 'toExponential() decimal places not an integer: {dp}'
	         * 'toExponential() decimal places out of range: {dp}'
	         * 'toExponential() rounding mode not an integer: {rm}'
	         * 'toExponential() rounding mode out of range: {rm}'
	         */
	        P.toExponential = function ( dp, rm ) {
	            return format( this,
	              dp != null && isValidInt( dp, 0, MAX, 19 ) ? ~~dp + 1 : null, rm, 19 );
	        };
	
	
	        /*
	         * Return a string representing the value of this BigNumber in fixed-point notation rounding
	         * to dp fixed decimal places using rounding mode rm, or ROUNDING_MODE if rm is omitted.
	         *
	         * Note: as with JavaScript's number type, (-0).toFixed(0) is '0',
	         * but e.g. (-0.00001).toFixed(0) is '-0'.
	         *
	         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
	         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
	         *
	         * 'toFixed() decimal places not an integer: {dp}'
	         * 'toFixed() decimal places out of range: {dp}'
	         * 'toFixed() rounding mode not an integer: {rm}'
	         * 'toFixed() rounding mode out of range: {rm}'
	         */
	        P.toFixed = function ( dp, rm ) {
	            return format( this, dp != null && isValidInt( dp, 0, MAX, 20 )
	              ? ~~dp + this.e + 1 : null, rm, 20 );
	        };
	
	
	        /*
	         * Return a string representing the value of this BigNumber in fixed-point notation rounded
	         * using rm or ROUNDING_MODE to dp decimal places, and formatted according to the properties
	         * of the FORMAT object (see BigNumber.config).
	         *
	         * FORMAT = {
	         *      decimalSeparator : '.',
	         *      groupSeparator : ',',
	         *      groupSize : 3,
	         *      secondaryGroupSize : 0,
	         *      fractionGroupSeparator : '\xA0',    // non-breaking space
	         *      fractionGroupSize : 0
	         * };
	         *
	         * [dp] {number} Decimal places. Integer, 0 to MAX inclusive.
	         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
	         *
	         * 'toFormat() decimal places not an integer: {dp}'
	         * 'toFormat() decimal places out of range: {dp}'
	         * 'toFormat() rounding mode not an integer: {rm}'
	         * 'toFormat() rounding mode out of range: {rm}'
	         */
	        P.toFormat = function ( dp, rm ) {
	            var str = format( this, dp != null && isValidInt( dp, 0, MAX, 21 )
	              ? ~~dp + this.e + 1 : null, rm, 21 );
	
	            if ( this.c ) {
	                var i,
	                    arr = str.split('.'),
	                    g1 = +FORMAT.groupSize,
	                    g2 = +FORMAT.secondaryGroupSize,
	                    groupSeparator = FORMAT.groupSeparator,
	                    intPart = arr[0],
	                    fractionPart = arr[1],
	                    isNeg = this.s < 0,
	                    intDigits = isNeg ? intPart.slice(1) : intPart,
	                    len = intDigits.length;
	
	                if (g2) i = g1, g1 = g2, g2 = i, len -= i;
	
	                if ( g1 > 0 && len > 0 ) {
	                    i = len % g1 || g1;
	                    intPart = intDigits.substr( 0, i );
	
	                    for ( ; i < len; i += g1 ) {
	                        intPart += groupSeparator + intDigits.substr( i, g1 );
	                    }
	
	                    if ( g2 > 0 ) intPart += groupSeparator + intDigits.slice(i);
	                    if (isNeg) intPart = '-' + intPart;
	                }
	
	                str = fractionPart
	                  ? intPart + FORMAT.decimalSeparator + ( ( g2 = +FORMAT.fractionGroupSize )
	                    ? fractionPart.replace( new RegExp( '\\d{' + g2 + '}\\B', 'g' ),
	                      '$&' + FORMAT.fractionGroupSeparator )
	                    : fractionPart )
	                  : intPart;
	            }
	
	            return str;
	        };
	
	
	        /*
	         * Return a string array representing the value of this BigNumber as a simple fraction with
	         * an integer numerator and an integer denominator. The denominator will be a positive
	         * non-zero value less than or equal to the specified maximum denominator. If a maximum
	         * denominator is not specified, the denominator will be the lowest value necessary to
	         * represent the number exactly.
	         *
	         * [md] {number|string|BigNumber} Integer >= 1 and < Infinity. The maximum denominator.
	         *
	         * 'toFraction() max denominator not an integer: {md}'
	         * 'toFraction() max denominator out of range: {md}'
	         */
	        P.toFraction = function (md) {
	            var arr, d0, d2, e, exp, n, n0, q, s,
	                k = ERRORS,
	                x = this,
	                xc = x.c,
	                d = new BigNumber(ONE),
	                n1 = d0 = new BigNumber(ONE),
	                d1 = n0 = new BigNumber(ONE);
	
	            if ( md != null ) {
	                ERRORS = false;
	                n = new BigNumber(md);
	                ERRORS = k;
	
	                if ( !( k = n.isInt() ) || n.lt(ONE) ) {
	
	                    if (ERRORS) {
	                        raise( 22,
	                          'max denominator ' + ( k ? 'out of range' : 'not an integer' ), md );
	                    }
	
	                    // ERRORS is false:
	                    // If md is a finite non-integer >= 1, round it to an integer and use it.
	                    md = !k && n.c && round( n, n.e + 1, 1 ).gte(ONE) ? n : null;
	                }
	            }
	
	            if ( !xc ) return x.toString();
	            s = coeffToString(xc);
	
	            // Determine initial denominator.
	            // d is a power of 10 and the minimum max denominator that specifies the value exactly.
	            e = d.e = s.length - x.e - 1;
	            d.c[0] = POWS_TEN[ ( exp = e % LOG_BASE ) < 0 ? LOG_BASE + exp : exp ];
	            md = !md || n.cmp(d) > 0 ? ( e > 0 ? d : n1 ) : n;
	
	            exp = MAX_EXP;
	            MAX_EXP = 1 / 0;
	            n = new BigNumber(s);
	
	            // n0 = d1 = 0
	            n0.c[0] = 0;
	
	            for ( ; ; )  {
	                q = div( n, d, 0, 1 );
	                d2 = d0.plus( q.times(d1) );
	                if ( d2.cmp(md) == 1 ) break;
	                d0 = d1;
	                d1 = d2;
	                n1 = n0.plus( q.times( d2 = n1 ) );
	                n0 = d2;
	                d = n.minus( q.times( d2 = d ) );
	                n = d2;
	            }
	
	            d2 = div( md.minus(d0), d1, 0, 1 );
	            n0 = n0.plus( d2.times(n1) );
	            d0 = d0.plus( d2.times(d1) );
	            n0.s = n1.s = x.s;
	            e *= 2;
	
	            // Determine which fraction is closer to x, n0/d0 or n1/d1
	            arr = div( n1, d1, e, ROUNDING_MODE ).minus(x).abs().cmp(
	                  div( n0, d0, e, ROUNDING_MODE ).minus(x).abs() ) < 1
	                    ? [ n1.toString(), d1.toString() ]
	                    : [ n0.toString(), d0.toString() ];
	
	            MAX_EXP = exp;
	            return arr;
	        };
	
	
	        /*
	         * Return the value of this BigNumber converted to a number primitive.
	         */
	        P.toNumber = function () {
	            return +this;
	        };
	
	
	        /*
	         * Return a BigNumber whose value is the value of this BigNumber raised to the power n.
	         * If m is present, return the result modulo m.
	         * If n is negative round according to DECIMAL_PLACES and ROUNDING_MODE.
	         * If POW_PRECISION is non-zero and m is not present, round to POW_PRECISION using
	         * ROUNDING_MODE.
	         *
	         * The modular power operation works efficiently when x, n, and m are positive integers,
	         * otherwise it is equivalent to calculating x.toPower(n).modulo(m) (with POW_PRECISION 0).
	         *
	         * n {number} Integer, -MAX_SAFE_INTEGER to MAX_SAFE_INTEGER inclusive.
	         * [m] {number|string|BigNumber} The modulus.
	         *
	         * 'pow() exponent not an integer: {n}'
	         * 'pow() exponent out of range: {n}'
	         *
	         * Performs 54 loop iterations for n of 9007199254740991.
	         */
	        P.toPower = P.pow = function ( n, m ) {
	            var k, y, z,
	                i = mathfloor( n < 0 ? -n : +n ),
	                x = this;
	
	            if ( m != null ) {
	                id = 23;
	                m = new BigNumber(m);
	            }
	
	            // Pass ±Infinity to Math.pow if exponent is out of range.
	            if ( !isValidInt( n, -MAX_SAFE_INTEGER, MAX_SAFE_INTEGER, 23, 'exponent' ) &&
	              ( !isFinite(n) || i > MAX_SAFE_INTEGER && ( n /= 0 ) ||
	                parseFloat(n) != n && !( n = NaN ) ) || n == 0 ) {
	                k = Math.pow( +x, n );
	                return new BigNumber( m ? k % m : k );
	            }
	
	            if (m) {
	                if ( n > 1 && x.gt(ONE) && x.isInt() && m.gt(ONE) && m.isInt() ) {
	                    x = x.mod(m);
	                } else {
	                    z = m;
	
	                    // Nullify m so only a single mod operation is performed at the end.
	                    m = null;
	                }
	            } else if (POW_PRECISION) {
	
	                // Truncating each coefficient array to a length of k after each multiplication
	                // equates to truncating significant digits to POW_PRECISION + [28, 41],
	                // i.e. there will be a minimum of 28 guard digits retained.
	                // (Using + 1.5 would give [9, 21] guard digits.)
	                k = mathceil( POW_PRECISION / LOG_BASE + 2 );
	            }
	
	            y = new BigNumber(ONE);
	
	            for ( ; ; ) {
	                if ( i % 2 ) {
	                    y = y.times(x);
	                    if ( !y.c ) break;
	                    if (k) {
	                        if ( y.c.length > k ) y.c.length = k;
	                    } else if (m) {
	                        y = y.mod(m);
	                    }
	                }
	
	                i = mathfloor( i / 2 );
	                if ( !i ) break;
	                x = x.times(x);
	                if (k) {
	                    if ( x.c && x.c.length > k ) x.c.length = k;
	                } else if (m) {
	                    x = x.mod(m);
	                }
	            }
	
	            if (m) return y;
	            if ( n < 0 ) y = ONE.div(y);
	
	            return z ? y.mod(z) : k ? round( y, POW_PRECISION, ROUNDING_MODE ) : y;
	        };
	
	
	        /*
	         * Return a string representing the value of this BigNumber rounded to sd significant digits
	         * using rounding mode rm or ROUNDING_MODE. If sd is less than the number of digits
	         * necessary to represent the integer part of the value in fixed-point notation, then use
	         * exponential notation.
	         *
	         * [sd] {number} Significant digits. Integer, 1 to MAX inclusive.
	         * [rm] {number} Rounding mode. Integer, 0 to 8 inclusive.
	         *
	         * 'toPrecision() precision not an integer: {sd}'
	         * 'toPrecision() precision out of range: {sd}'
	         * 'toPrecision() rounding mode not an integer: {rm}'
	         * 'toPrecision() rounding mode out of range: {rm}'
	         */
	        P.toPrecision = function ( sd, rm ) {
	            return format( this, sd != null && isValidInt( sd, 1, MAX, 24, 'precision' )
	              ? sd | 0 : null, rm, 24 );
	        };
	
	
	        /*
	         * Return a string representing the value of this BigNumber in base b, or base 10 if b is
	         * omitted. If a base is specified, including base 10, round according to DECIMAL_PLACES and
	         * ROUNDING_MODE. If a base is not specified, and this BigNumber has a positive exponent
	         * that is equal to or greater than TO_EXP_POS, or a negative exponent equal to or less than
	         * TO_EXP_NEG, return exponential notation.
	         *
	         * [b] {number} Integer, 2 to 64 inclusive.
	         *
	         * 'toString() base not an integer: {b}'
	         * 'toString() base out of range: {b}'
	         */
	        P.toString = function (b) {
	            var str,
	                n = this,
	                s = n.s,
	                e = n.e;
	
	            // Infinity or NaN?
	            if ( e === null ) {
	
	                if (s) {
	                    str = 'Infinity';
	                    if ( s < 0 ) str = '-' + str;
	                } else {
	                    str = 'NaN';
	                }
	            } else {
	                str = coeffToString( n.c );
	
	                if ( b == null || !isValidInt( b, 2, 64, 25, 'base' ) ) {
	                    str = e <= TO_EXP_NEG || e >= TO_EXP_POS
	                      ? toExponential( str, e )
	                      : toFixedPoint( str, e );
	                } else {
	                    str = convertBase( toFixedPoint( str, e ), b | 0, 10, s );
	                }
	
	                if ( s < 0 && n.c[0] ) str = '-' + str;
	            }
	
	            return str;
	        };
	
	
	        /*
	         * Return a new BigNumber whose value is the value of this BigNumber truncated to a whole
	         * number.
	         */
	        P.truncated = P.trunc = function () {
	            return round( new BigNumber(this), this.e + 1, 1 );
	        };
	
	
	
	        /*
	         * Return as toString, but do not accept a base argument, and include the minus sign for
	         * negative zero.
	         */
	        P.valueOf = P.toJSON = function () {
	            var str,
	                n = this,
	                e = n.e;
	
	            if ( e === null ) return n.toString();
	
	            str = coeffToString( n.c );
	
	            str = e <= TO_EXP_NEG || e >= TO_EXP_POS
	                ? toExponential( str, e )
	                : toFixedPoint( str, e );
	
	            return n.s < 0 ? '-' + str : str;
	        };
	
	
	        // Aliases for BigDecimal methods.
	        //P.add = P.plus;         // P.add included above
	        //P.subtract = P.minus;   // P.sub included above
	        //P.multiply = P.times;   // P.mul included above
	        //P.divide = P.div;
	        //P.remainder = P.mod;
	        //P.compareTo = P.cmp;
	        //P.negate = P.neg;
	
	
	        if ( configObj != null ) BigNumber.config(configObj);
	
	        return BigNumber;
	    }
	
	
	    // PRIVATE HELPER FUNCTIONS
	
	
	    function bitFloor(n) {
	        var i = n | 0;
	        return n > 0 || n === i ? i : i - 1;
	    }
	
	
	    // Return a coefficient array as a string of base 10 digits.
	    function coeffToString(a) {
	        var s, z,
	            i = 1,
	            j = a.length,
	            r = a[0] + '';
	
	        for ( ; i < j; ) {
	            s = a[i++] + '';
	            z = LOG_BASE - s.length;
	            for ( ; z--; s = '0' + s );
	            r += s;
	        }
	
	        // Determine trailing zeros.
	        for ( j = r.length; r.charCodeAt(--j) === 48; );
	        return r.slice( 0, j + 1 || 1 );
	    }
	
	
	    // Compare the value of BigNumbers x and y.
	    function compare( x, y ) {
	        var a, b,
	            xc = x.c,
	            yc = y.c,
	            i = x.s,
	            j = y.s,
	            k = x.e,
	            l = y.e;
	
	        // Either NaN?
	        if ( !i || !j ) return null;
	
	        a = xc && !xc[0];
	        b = yc && !yc[0];
	
	        // Either zero?
	        if ( a || b ) return a ? b ? 0 : -j : i;
	
	        // Signs differ?
	        if ( i != j ) return i;
	
	        a = i < 0;
	        b = k == l;
	
	        // Either Infinity?
	        if ( !xc || !yc ) return b ? 0 : !xc ^ a ? 1 : -1;
	
	        // Compare exponents.
	        if ( !b ) return k > l ^ a ? 1 : -1;
	
	        j = ( k = xc.length ) < ( l = yc.length ) ? k : l;
	
	        // Compare digit by digit.
	        for ( i = 0; i < j; i++ ) if ( xc[i] != yc[i] ) return xc[i] > yc[i] ^ a ? 1 : -1;
	
	        // Compare lengths.
	        return k == l ? 0 : k > l ^ a ? 1 : -1;
	    }
	
	
	    /*
	     * Return true if n is a valid number in range, otherwise false.
	     * Use for argument validation when ERRORS is false.
	     * Note: parseInt('1e+1') == 1 but parseFloat('1e+1') == 10.
	     */
	    function intValidatorNoErrors( n, min, max ) {
	        return ( n = truncate(n) ) >= min && n <= max;
	    }
	
	
	    function isArray(obj) {
	        return Object.prototype.toString.call(obj) == '[object Array]';
	    }
	
	
	    /*
	     * Convert string of baseIn to an array of numbers of baseOut.
	     * Eg. convertBase('255', 10, 16) returns [15, 15].
	     * Eg. convertBase('ff', 16, 10) returns [2, 5, 5].
	     */
	    function toBaseOut( str, baseIn, baseOut ) {
	        var j,
	            arr = [0],
	            arrL,
	            i = 0,
	            len = str.length;
	
	        for ( ; i < len; ) {
	            for ( arrL = arr.length; arrL--; arr[arrL] *= baseIn );
	            arr[ j = 0 ] += ALPHABET.indexOf( str.charAt( i++ ) );
	
	            for ( ; j < arr.length; j++ ) {
	
	                if ( arr[j] > baseOut - 1 ) {
	                    if ( arr[j + 1] == null ) arr[j + 1] = 0;
	                    arr[j + 1] += arr[j] / baseOut | 0;
	                    arr[j] %= baseOut;
	                }
	            }
	        }
	
	        return arr.reverse();
	    }
	
	
	    function toExponential( str, e ) {
	        return ( str.length > 1 ? str.charAt(0) + '.' + str.slice(1) : str ) +
	          ( e < 0 ? 'e' : 'e+' ) + e;
	    }
	
	
	    function toFixedPoint( str, e ) {
	        var len, z;
	
	        // Negative exponent?
	        if ( e < 0 ) {
	
	            // Prepend zeros.
	            for ( z = '0.'; ++e; z += '0' );
	            str = z + str;
	
	        // Positive exponent
	        } else {
	            len = str.length;
	
	            // Append zeros.
	            if ( ++e > len ) {
	                for ( z = '0', e -= len; --e; z += '0' );
	                str += z;
	            } else if ( e < len ) {
	                str = str.slice( 0, e ) + '.' + str.slice(e);
	            }
	        }
	
	        return str;
	    }
	
	
	    function truncate(n) {
	        n = parseFloat(n);
	        return n < 0 ? mathceil(n) : mathfloor(n);
	    }
	
	
	    // EXPORT
	
	
	    BigNumber = constructorFactory();
	    BigNumber.default = BigNumber.BigNumber = BigNumber;
	
	
	    // AMD.
	    if ( true ) {
	        !(__WEBPACK_AMD_DEFINE_RESULT__ = function () { return BigNumber; }.call(exports, __webpack_require__, exports, module), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	
	    // Node.js and other environments that support module.exports.
	    } else if ( typeof module != 'undefined' && module.exports ) {
	        module.exports = BigNumber;
	
	        // Split string stops browserify adding crypto shim.
	        if ( !cryptoObj ) try { cryptoObj = require('cry' + 'pto'); } catch (e) {}
	
	    // Browser.
	    } else {
	        if ( !globalObj ) globalObj = typeof self != 'undefined' ? self : Function('return this')();
	        globalObj.BigNumber = BigNumber;
	    }
	})(this);


/***/ },

/***/ 208:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {;(function($){
	/**
	 * jqGrid English Translation
	 * Tony Tomov tony@trirand.com
	 * http://trirand.com/blog/ 
	 * Dual licensed under the MIT and GPL licenses:
	 * http://www.opensource.org/licenses/mit-license.php
	 * http://www.gnu.org/licenses/gpl.html
	**/
	$.jgrid = $.jgrid || {};
	$.extend($.jgrid,{
		defaults : {
			recordtext: "View {0} - {1} of {2}",
			emptyrecords: "No records to view",
			loadtext: "Loading...",
			pgtext : "Page {0} of {1}"
		},
		search : {
			caption: "Search...",
			Find: "Find",
			Reset: "Reset",
			odata: [{ oper:'eq', text:'equal'},{ oper:'ne', text:'not equal'},{ oper:'lt', text:'less'},{ oper:'le', text:'less or equal'},{ oper:'gt', text:'greater'},{ oper:'ge', text:'greater or equal'},{ oper:'bw', text:'begins with'},{ oper:'bn', text:'does not begin with'},{ oper:'in', text:'is in'},{ oper:'ni', text:'is not in'},{ oper:'ew', text:'ends with'},{ oper:'en', text:'does not end with'},{ oper:'cn', text:'contains'},{ oper:'nc', text:'does not contain'},{ oper:'nu', text:'is null'},{ oper:'nn', text:'is not null'}],
			groupOps: [{ op: "AND", text: "all" },{ op: "OR",  text: "any" }],
			operandTitle : "Click to select search operation.",
			resetTitle : "Reset Search Value"
		},
		edit : {
			addCaption: "Add Record",
			editCaption: "Edit Record",
			bSubmit: "Submit",
			bCancel: "Cancel",
			bClose: "Close",
			saveData: "Data has been changed! Save changes?",
			bYes : "Yes",
			bNo : "No",
			bExit : "Cancel",
			msg: {
				required:"Field is required",
				number:"Please, enter valid number",
				minValue:"value must be greater than or equal to ",
				maxValue:"value must be less than or equal to",
				email: "is not a valid e-mail",
				integer: "Please, enter valid integer value",
				date: "Please, enter valid date value",
				url: "is not a valid URL. Prefix required ('http://' or 'https://')",
				nodefined : " is not defined!",
				novalue : " return value is required!",
				customarray : "Custom function should return array!",
				customfcheck : "Custom function should be present in case of custom checking!"
				
			}
		},
		view : {
			caption: "View Record",
			bClose: "Close"
		},
		del : {
			caption: "Delete",
			msg: "Delete selected record(s)?",
			bSubmit: "Delete",
			bCancel: "Cancel"
		},
		nav : {
			edittext: "",
			edittitle: "Edit selected row",
			addtext:"",
			addtitle: "Add new row",
			deltext: "",
			deltitle: "Delete selected row",
			searchtext: "",
			searchtitle: "Find records",
			refreshtext: "",
			refreshtitle: "Reload Grid",
			alertcap: "Warning",
			alerttext: "Please, select row",
			viewtext: "",
			viewtitle: "View selected row"
		},
		col : {
			caption: "Select columns",
			bSubmit: "Ok",
			bCancel: "Cancel"
		},
		errors : {
			errcap : "Error",
			nourl : "No url is set",
			norecords: "No records to process",
			model : "Length of colNames <> colModel!"
		},
		formatter : {
			integer : {thousandsSeparator: ",", defaultValue: '0'},
			number : {decimalSeparator:".", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00'},
			currency : {decimalSeparator:".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix:"", defaultValue: '0.00'},
			date : {
				dayNames:   [
					"Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat",
					"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
				],
				monthNames: [
					"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
					"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
				],
				AmPm : ["am","pm","AM","PM"],
				S: function (j) {return j < 11 || j > 13 ? ['st', 'nd', 'rd', 'th'][Math.min((j - 1) % 10, 3)] : 'th';},
				srcformat: 'Y-m-d',
				newformat: 'n/j/Y',
				parseRe : /[#%\\\/:_;.,\t\s-]/,
				masks : {
					// see http://php.net/manual/en/function.date.php for PHP format used in jqGrid
					// and see http://docs.jquery.com/UI/Datepicker/formatDate
					// and https://github.com/jquery/globalize#dates for alternative formats used frequently
					// one can find on https://github.com/jquery/globalize/tree/master/lib/cultures many
					// information about date, time, numbers and currency formats used in different countries
					// one should just convert the information in PHP format
					ISO8601Long:"Y-m-d H:i:s",
					ISO8601Short:"Y-m-d",
					// short date:
					//    n - Numeric representation of a month, without leading zeros
					//    j - Day of the month without leading zeros
					//    Y - A full numeric representation of a year, 4 digits
					// example: 3/1/2012 which means 1 March 2012
					ShortDate: "n/j/Y", // in jQuery UI Datepicker: "M/d/yyyy"
					// long date:
					//    l - A full textual representation of the day of the week
					//    F - A full textual representation of a month
					//    d - Day of the month, 2 digits with leading zeros
					//    Y - A full numeric representation of a year, 4 digits
					LongDate: "l, F d, Y", // in jQuery UI Datepicker: "dddd, MMMM dd, yyyy"
					// long date with long time:
					//    l - A full textual representation of the day of the week
					//    F - A full textual representation of a month
					//    d - Day of the month, 2 digits with leading zeros
					//    Y - A full numeric representation of a year, 4 digits
					//    g - 12-hour format of an hour without leading zeros
					//    i - Minutes with leading zeros
					//    s - Seconds, with leading zeros
					//    A - Uppercase Ante meridiem and Post meridiem (AM or PM)
					FullDateTime: "l, F d, Y g:i:s A", // in jQuery UI Datepicker: "dddd, MMMM dd, yyyy h:mm:ss tt"
					// month day:
					//    F - A full textual representation of a month
					//    d - Day of the month, 2 digits with leading zeros
					MonthDay: "F d", // in jQuery UI Datepicker: "MMMM dd"
					// short time (without seconds)
					//    g - 12-hour format of an hour without leading zeros
					//    i - Minutes with leading zeros
					//    A - Uppercase Ante meridiem and Post meridiem (AM or PM)
					ShortTime: "g:i A", // in jQuery UI Datepicker: "h:mm tt"
					// long time (with seconds)
					//    g - 12-hour format of an hour without leading zeros
					//    i - Minutes with leading zeros
					//    s - Seconds, with leading zeros
					//    A - Uppercase Ante meridiem and Post meridiem (AM or PM)
					LongTime: "g:i:s A", // in jQuery UI Datepicker: "h:mm:ss tt"
					SortableDateTime: "Y-m-d\\TH:i:s",
					UniversalSortableDateTime: "Y-m-d H:i:sO",
					// month with year
					//    Y - A full numeric representation of a year, 4 digits
					//    F - A full textual representation of a month
					YearMonth: "F, Y" // in jQuery UI Datepicker: "MMMM, yyyy"
				},
				reformatAfterEdit : false
			},
			baseLinkUrl: '',
			showAction: '',
			target: '',
			checkbox : {disabled:true},
			idName : 'id'
		}
	});
	})(jQuery);
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 209:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/* 
	* jqGrid  4.6.0 - jQuery Grid 
	* Copyright (c) 2008, Tony Tomov, tony@trirand.com 
	* Dual licensed under the MIT or GPL licenses 
	* http://www.opensource.org/licenses/mit-license.php 
	* http://www.gnu.org/licenses/gpl-2.0.html 
	* Date:2014-02-20 
	* Modules: grid.base.js; jquery.fmatter.js; grid.custom.js; grid.common.js; grid.subgrid.js; grid.grouping.js; grid.treegrid.js; grid.pivot.js; JsonXml.js; grid.tbltogrid.js; 
	*/
	(function(b){b.jgrid=b.jgrid||{};b.extend(b.jgrid,{version:"4.6.0",htmlDecode:function(b){return b&&("&nbsp;"===b||"&#160;"===b||1===b.length&&160===b.charCodeAt(0))?"":b?String(b).replace(/&gt;/g,">").replace(/&lt;/g,"<").replace(/&quot;/g,'"').replace(/&amp;/g,"&"):b},htmlEncode:function(b){return b?String(b).replace(/&/g,"&amp;").replace(/\"/g,"&quot;").replace(/</g,"&lt;").replace(/>/g,"&gt;"):b},format:function(e){var f=b.makeArray(arguments).slice(1);null==e&&(e="");return e.replace(/\{(\d+)\}/g,
	function(b,d){return f[d]})},msie:"Microsoft Internet Explorer"===navigator.appName,msiever:function(){var b=-1;null!=/MSIE ([0-9]{1,}[.0-9]{0,})/.exec(navigator.userAgent)&&(b=parseFloat(RegExp.$1));return b},getCellIndex:function(e){e=b(e);if(e.is("tr"))return-1;e=(e.is("td")||e.is("th")?e:e.closest("td,th"))[0];return b.jgrid.msie?b.inArray(e,e.parentNode.cells):e.cellIndex},stripHtml:function(b){b=String(b);var f=/<("[^"]*"|'[^']*'|[^'">])*>/gi;return b?(b=b.replace(f,""))&&"&nbsp;"!==b&&"&#160;"!==
	b?b.replace(/\"/g,"'"):"":b},stripPref:function(e,f){var c=b.type(e);if("string"===c||"number"===c)e=String(e),f=""!==e?String(f).replace(String(e),""):f;return f},parse:function(e){"while(1);"===e.substr(0,9)&&(e=e.substr(9));"/*"===e.substr(0,2)&&(e=e.substr(2,e.length-4));e||(e="{}");return!0===b.jgrid.useJSON&&"object"===typeof JSON&&"function"===typeof JSON.parse?JSON.parse(e):eval("("+e+")")},parseDate:function(e,f,c,d){var a=/^\/Date\((([-+])?[0-9]+)(([-+])([0-9]{2})([0-9]{2}))?\)\/$/,l="string"===
	typeof f?f.match(a):null,a=function(a,b){a=String(a);for(b=parseInt(b,10)||2;a.length<b;)a="0"+a;return a},g={m:1,d:1,y:1970,h:0,i:0,s:0,u:0},h=0,k,n,h=function(a,b){0===a?12===b&&(b=0):12!==b&&(b+=12);return b};void 0===d&&(d=b.jgrid.formatter.date);void 0===d.parseRe&&(d.parseRe=/[#%\\\/:_;.,\t\s-]/);d.masks.hasOwnProperty(e)&&(e=d.masks[e]);if(f&&null!=f)if(isNaN(f-0)||"u"!==String(e).toLowerCase())if(f.constructor===Date)h=f;else if(null!==l){if(h=new Date(parseInt(l[1],10)),l[3]){var m=60*Number(l[5])+
	Number(l[6]),m=m*("-"===l[4]?1:-1),m=m-h.getTimezoneOffset();h.setTime(Number(Number(h)+6E4*m))}}else{m=0;"ISO8601Long"===d.srcformat&&"Z"===f.charAt(f.length-1)&&(m-=(new Date).getTimezoneOffset());f=String(f).replace(/\T/g,"#").replace(/\t/,"%").split(d.parseRe);e=e.replace(/\T/g,"#").replace(/\t/,"%").split(d.parseRe);k=0;for(n=e.length;k<n;k++)"M"===e[k]&&(l=b.inArray(f[k],d.monthNames),-1!==l&&12>l&&(f[k]=l+1,g.m=f[k])),"F"===e[k]&&(l=b.inArray(f[k],d.monthNames,12),-1!==l&&11<l&&(f[k]=l+1-12,
	g.m=f[k])),"a"===e[k]&&(l=b.inArray(f[k],d.AmPm),-1!==l&&2>l&&f[k]===d.AmPm[l]&&(f[k]=l,g.h=h(f[k],g.h))),"A"===e[k]&&(l=b.inArray(f[k],d.AmPm),-1!==l&&1<l&&f[k]===d.AmPm[l]&&(f[k]=l-2,g.h=h(f[k],g.h))),"g"===e[k]&&(g.h=parseInt(f[k],10)),void 0!==f[k]&&(g[e[k].toLowerCase()]=parseInt(f[k],10));g.f&&(g.m=g.f);if(0===g.m&&0===g.y&&0===g.d)return"&#160;";g.m=parseInt(g.m,10)-1;h=g.y;70<=h&&99>=h?g.y=1900+g.y:0<=h&&69>=h&&(g.y=2E3+g.y);h=new Date(g.y,g.m,g.d,g.h,g.i,g.s,g.u);0<m&&h.setTime(Number(Number(h)+
	6E4*m))}else h=new Date(1E3*parseFloat(f));else h=new Date(g.y,g.m,g.d,g.h,g.i,g.s,g.u);if(void 0===c)return h;d.masks.hasOwnProperty(c)?c=d.masks[c]:c||(c="Y-m-d");e=h.getHours();f=h.getMinutes();g=h.getDate();m=h.getMonth()+1;l=h.getTimezoneOffset();k=h.getSeconds();n=h.getMilliseconds();var r=h.getDay(),p=h.getFullYear(),q=(r+6)%7+1,x=(new Date(p,m-1,g)-new Date(p,0,1))/864E5,G={d:a(g),D:d.dayNames[r],j:g,l:d.dayNames[r+7],N:q,S:d.S(g),w:r,z:x,W:5>q?Math.floor((x+q-1)/7)+1:Math.floor((x+q-1)/7)||
	(4>((new Date(p-1,0,1)).getDay()+6)%7?53:52),F:d.monthNames[m-1+12],m:a(m),M:d.monthNames[m-1],n:m,t:"?",L:"?",o:"?",Y:p,y:String(p).substring(2),a:12>e?d.AmPm[0]:d.AmPm[1],A:12>e?d.AmPm[2]:d.AmPm[3],B:"?",g:e%12||12,G:e,h:a(e%12||12),H:a(e),i:a(f),s:a(k),u:n,e:"?",I:"?",O:(0<l?"-":"+")+a(100*Math.floor(Math.abs(l)/60)+Math.abs(l)%60,4),P:"?",T:(String(h).match(/\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g)||
	[""]).pop().replace(/[^-+\dA-Z]/g,""),Z:"?",c:"?",r:"?",U:Math.floor(h/1E3)};return c.replace(/\\.|[dDjlNSwzWFmMntLoYyaABgGhHisueIOPTZcrU]/g,function(a){return G.hasOwnProperty(a)?G[a]:a.substring(1)})},jqID:function(b){return String(b).replace(/[!"#$%&'()*+,.\/:; <=>?@\[\\\]\^`{|}~]/g,"\\$&")},guid:1,uidPref:"jqg",randId:function(e){return(e||b.jgrid.uidPref)+b.jgrid.guid++},getAccessor:function(b,f){var c,d,a=[],l;if("function"===typeof f)return f(b);c=b[f];if(void 0===c)try{if("string"===typeof f&&
	(a=f.split(".")),l=a.length)for(c=b;c&&l--;)d=a.shift(),c=c[d]}catch(g){}return c},getXmlData:function(e,f,c){var d="string"===typeof f?f.match(/^(.*)\[(\w+)\]$/):null;if("function"===typeof f)return f(e);if(d&&d[2])return d[1]?b(d[1],e).attr(d[2]):b(e).attr(d[2]);e=b(f,e);return c?e:0<e.length?b(e).text():void 0},cellWidth:function(){var e=b("<div class='ui-jqgrid' style='left:10000px'><table class='ui-jqgrid-btable' style='width:5px;'><tr class='jqgrow'><td style='width:5px;display:block;'></td></tr></table></div>"),
	f=e.appendTo("body").find("td").width();e.remove();return 0.1<Math.abs(f-5)},cell_width:!0,ajaxOptions:{},from:function(e){return new function(e,c){"string"===typeof e&&(e=b.data(e));var d=this,a=e,l=!0,g=!1,h=c,k=/[\$,%]/g,n=null,m=null,r=0,p=!1,q="",x=[],G=!0;if("object"===typeof e&&e.push)0<e.length&&(G="object"!==typeof e[0]?!1:!0);else throw"data provides is not an array";this._hasData=function(){return null===a?!1:0===a.length?!1:!0};this._getStr=function(a){var b=[];g&&b.push("jQuery.trim(");
	b.push("String("+a+")");g&&b.push(")");l||b.push(".toLowerCase()");return b.join("")};this._strComp=function(a){return"string"===typeof a?".toString()":""};this._group=function(a,b){return{field:a.toString(),unique:b,items:[]}};this._toStr=function(a){g&&(a=b.trim(a));a=a.toString().replace(/\\/g,"\\\\").replace(/\"/g,'\\"');return l?a:a.toLowerCase()};this._funcLoop=function(d){var l=[];b.each(a,function(a,b){l.push(d(b))});return l};this._append=function(a){var b;h=null===h?"":h+(""===q?" && ":
	q);for(b=0;b<r;b++)h+="(";p&&(h+="!");h+="("+a+")";p=!1;q="";r=0};this._setCommand=function(a,b){n=a;m=b};this._resetNegate=function(){p=!1};this._repeatCommand=function(a,b){return null===n?d:null!==a&&null!==b?n(a,b):null!==m&&G?n(m,a):n(a)};this._equals=function(a,b){return 0===d._compare(a,b,1)};this._compare=function(a,b,d){var e=Object.prototype.toString;void 0===d&&(d=1);void 0===a&&(a=null);void 0===b&&(b=null);if(null===a&&null===b)return 0;if(null===a&&null!==b)return 1;if(null!==a&&null===
	b)return-1;if("[object Date]"===e.call(a)&&"[object Date]"===e.call(b))return a<b?-d:a>b?d:0;l||"number"===typeof a||"number"===typeof b||(a=String(a),b=String(b));return a<b?-d:a>b?d:0};this._performSort=function(){0!==x.length&&(a=d._doSort(a,0))};this._doSort=function(a,b){var l=x[b].by,e=x[b].dir,g=x[b].type,c=x[b].datefmt,f=x[b].sfunc;if(b===x.length-1)return d._getOrder(a,l,e,g,c,f);b++;l=d._getGroup(a,l,e,g,c);e=[];for(g=0;g<l.length;g++)for(f=d._doSort(l[g].items,b),c=0;c<f.length;c++)e.push(f[c]);
	return e};this._getOrder=function(a,e,g,c,f,h){var m=[],n=[],r="a"===g?1:-1,p,x;void 0===c&&(c="text");x="float"===c||"number"===c||"currency"===c||"numeric"===c?function(a){a=parseFloat(String(a).replace(k,""));return isNaN(a)?0:a}:"int"===c||"integer"===c?function(a){return a?parseFloat(String(a).replace(k,"")):0}:"date"===c||"datetime"===c?function(a){return b.jgrid.parseDate(f,a).getTime()}:b.isFunction(c)?c:function(a){a=a?b.trim(String(a)):"";return l?a:a.toLowerCase()};b.each(a,function(a,
	d){p=""!==e?b.jgrid.getAccessor(d,e):d;void 0===p&&(p="");p=x(p,d);n.push({vSort:p,index:a})});b.isFunction(h)?n.sort(function(a,b){a=a.vSort;b=b.vSort;return h.call(this,a,b,r)}):n.sort(function(a,b){a=a.vSort;b=b.vSort;return d._compare(a,b,r)});c=0;for(var q=a.length;c<q;)g=n[c].index,m.push(a[g]),c++;return m};this._getGroup=function(a,c,e,l,g){var f=[],h=null,k=null,m;b.each(d._getOrder(a,c,e,l,g),function(a,e){m=b.jgrid.getAccessor(e,c);null==m&&(m="");d._equals(k,m)||(k=m,null!==h&&f.push(h),
	h=d._group(c,m));h.items.push(e)});null!==h&&f.push(h);return f};this.ignoreCase=function(){l=!1;return d};this.useCase=function(){l=!0;return d};this.trim=function(){g=!0;return d};this.noTrim=function(){g=!1;return d};this.execute=function(){var c=h,e=[];if(null===c)return d;b.each(a,function(){eval(c)&&e.push(this)});a=e;return d};this.data=function(){return a};this.select=function(c){d._performSort();if(!d._hasData())return[];d.execute();if(b.isFunction(c)){var e=[];b.each(a,function(a,b){e.push(c(b))});
	return e}return a};this.hasMatch=function(){if(!d._hasData())return!1;d.execute();return 0<a.length};this.andNot=function(a,b,c){p=!p;return d.and(a,b,c)};this.orNot=function(a,b,c){p=!p;return d.or(a,b,c)};this.not=function(a,b,c){return d.andNot(a,b,c)};this.and=function(a,b,c){q=" && ";return void 0===a?d:d._repeatCommand(a,b,c)};this.or=function(a,b,c){q=" || ";return void 0===a?d:d._repeatCommand(a,b,c)};this.orBegin=function(){r++;return d};this.orEnd=function(){null!==h&&(h+=")");return d};
	this.isNot=function(a){p=!p;return d.is(a)};this.is=function(a){d._append("this."+a);d._resetNegate();return d};this._compareValues=function(a,c,e,l,g){var f;f=G?"jQuery.jgrid.getAccessor(this,'"+c+"')":"this";void 0===e&&(e=null);var h=e,m=void 0===g.stype?"text":g.stype;if(null!==e)switch(m){case "int":case "integer":h=isNaN(Number(h))||""===h?"0":h;f="parseInt("+f+",10)";h="parseInt("+h+",10)";break;case "float":case "number":case "numeric":h=String(h).replace(k,"");h=isNaN(Number(h))||""===h?
	"0":h;f="parseFloat("+f+")";h="parseFloat("+h+")";break;case "date":case "datetime":h=String(b.jgrid.parseDate(g.newfmt||"Y-m-d",h).getTime());f='jQuery.jgrid.parseDate("'+g.srcfmt+'",'+f+").getTime()";break;default:f=d._getStr(f),h=d._getStr('"'+d._toStr(h)+'"')}d._append(f+" "+l+" "+h);d._setCommand(a,c);d._resetNegate();return d};this.equals=function(a,b,c){return d._compareValues(d.equals,a,b,"==",c)};this.notEquals=function(a,b,c){return d._compareValues(d.equals,a,b,"!==",c)};this.isNull=function(a,
	b,c){return d._compareValues(d.equals,a,null,"===",c)};this.greater=function(a,b,c){return d._compareValues(d.greater,a,b,">",c)};this.less=function(a,b,c){return d._compareValues(d.less,a,b,"<",c)};this.greaterOrEquals=function(a,b,c){return d._compareValues(d.greaterOrEquals,a,b,">=",c)};this.lessOrEquals=function(a,b,c){return d._compareValues(d.lessOrEquals,a,b,"<=",c)};this.startsWith=function(a,c){var e=null==c?a:c,e=g?b.trim(e.toString()).length:e.toString().length;G?d._append(d._getStr("jQuery.jgrid.getAccessor(this,'"+
	a+"')")+".substr(0,"+e+") == "+d._getStr('"'+d._toStr(c)+'"')):(null!=c&&(e=g?b.trim(c.toString()).length:c.toString().length),d._append(d._getStr("this")+".substr(0,"+e+") == "+d._getStr('"'+d._toStr(a)+'"')));d._setCommand(d.startsWith,a);d._resetNegate();return d};this.endsWith=function(a,c){var e=null==c?a:c,e=g?b.trim(e.toString()).length:e.toString().length;G?d._append(d._getStr("jQuery.jgrid.getAccessor(this,'"+a+"')")+".substr("+d._getStr("jQuery.jgrid.getAccessor(this,'"+a+"')")+".length-"+
	e+","+e+') == "'+d._toStr(c)+'"'):d._append(d._getStr("this")+".substr("+d._getStr("this")+'.length-"'+d._toStr(a)+'".length,"'+d._toStr(a)+'".length) == "'+d._toStr(a)+'"');d._setCommand(d.endsWith,a);d._resetNegate();return d};this.contains=function(a,b){G?d._append(d._getStr("jQuery.jgrid.getAccessor(this,'"+a+"')")+'.indexOf("'+d._toStr(b)+'",0) > -1'):d._append(d._getStr("this")+'.indexOf("'+d._toStr(a)+'",0) > -1');d._setCommand(d.contains,a);d._resetNegate();return d};this.groupBy=function(b,
	c,e,l){return d._hasData()?d._getGroup(a,b,c,e,l):null};this.orderBy=function(a,c,e,l,g){c=null==c?"a":b.trim(c.toString().toLowerCase());null==e&&(e="text");null==l&&(l="Y-m-d");null==g&&(g=!1);if("desc"===c||"descending"===c)c="d";if("asc"===c||"ascending"===c)c="a";x.push({by:a,dir:c,type:e,datefmt:l,sfunc:g});return d};return d}(e,null)},getMethod:function(e){return this.getAccessor(b.fn.jqGrid,e)},extend:function(e){b.extend(b.fn.jqGrid,e);this.no_legacy_api||b.fn.extend(e)}});b.fn.jqGrid=function(e){if("string"===
	typeof e){var f=b.jgrid.getMethod(e);if(!f)throw"jqGrid - No such method: "+e;var c=b.makeArray(arguments).slice(1);return f.apply(this,c)}return this.each(function(){if(!this.grid){var d=b.extend(!0,{url:"",height:150,page:1,rowNum:20,rowTotal:null,records:0,pager:"",pgbuttons:!0,pginput:!0,colModel:[],rowList:[],colNames:[],sortorder:"asc",sortname:"",datatype:"xml",mtype:"GET",altRows:!1,selarrrow:[],savedRow:[],shrinkToFit:!0,xmlReader:{},jsonReader:{},subGrid:!1,subGridModel:[],reccount:0,lastpage:0,
	lastsort:0,selrow:null,beforeSelectRow:null,onSelectRow:null,onSortCol:null,ondblClickRow:null,onRightClickRow:null,onPaging:null,onSelectAll:null,onInitGrid:null,loadComplete:null,gridComplete:null,loadError:null,loadBeforeSend:null,afterInsertRow:null,beforeRequest:null,beforeProcessing:null,onHeaderClick:null,viewrecords:!1,loadonce:!1,multiselect:!1,multikey:!1,editurl:null,search:!1,caption:"",hidegrid:!0,hiddengrid:!1,postData:{},userData:{},treeGrid:!1,treeGridModel:"nested",treeReader:{},
	treeANode:-1,ExpandColumn:null,tree_root_level:0,prmNames:{page:"page",rows:"rows",sort:"sidx",order:"sord",search:"_search",nd:"nd",id:"id",oper:"oper",editoper:"edit",addoper:"add",deloper:"del",subgridid:"id",npage:null,totalrows:"totalrows"},forceFit:!1,gridstate:"visible",cellEdit:!1,cellsubmit:"remote",nv:0,loadui:"enable",toolbar:[!1,""],scroll:!1,multiboxonly:!1,deselectAfterSort:!0,scrollrows:!1,autowidth:!1,scrollOffset:18,cellLayout:5,subGridWidth:20,multiselectWidth:20,gridview:!1,rownumWidth:25,
	rownumbers:!1,pagerpos:"center",recordpos:"right",footerrow:!1,userDataOnFooter:!1,hoverrows:!0,altclass:"ui-priority-secondary",viewsortcols:[!1,"vertical",!0],resizeclass:"",autoencode:!1,remapColumns:[],ajaxGridOptions:{},direction:"ltr",toppager:!1,headertitles:!1,scrollTimeout:40,data:[],_index:{},grouping:!1,groupingView:{groupField:[],groupOrder:[],groupText:[],groupColumnShow:[],groupSummary:[],showSummaryOnHide:!1,sortitems:[],sortnames:[],summary:[],summaryval:[],plusicon:"ui-icon-circlesmall-plus",
	minusicon:"ui-icon-circlesmall-minus",displayField:[],groupSummaryPos:[],formatDisplayField:[],_locgr:!1},ignoreCase:!1,cmTemplate:{},idPrefix:"",multiSort:!1},b.jgrid.defaults,e||{}),a=this,c={headers:[],cols:[],footers:[],dragStart:function(c,e,g){var f=b(this.bDiv).offset().left;this.resizing={idx:c,startX:e.clientX,sOL:e.clientX-f};this.hDiv.style.cursor="col-resize";this.curGbox=b("#rs_m"+b.jgrid.jqID(d.id),"#gbox_"+b.jgrid.jqID(d.id));this.curGbox.css({display:"block",left:e.clientX-f,top:g[1],
	height:g[2]});b(a).triggerHandler("jqGridResizeStart",[e,c]);b.isFunction(d.resizeStart)&&d.resizeStart.call(a,e,c);document.onselectstart=function(){return!1}},dragMove:function(a){if(this.resizing){var b=a.clientX-this.resizing.startX;a=this.headers[this.resizing.idx];var c="ltr"===d.direction?a.width+b:a.width-b,e;33<c&&(this.curGbox.css({left:this.resizing.sOL+b}),!0===d.forceFit?(e=this.headers[this.resizing.idx+d.nv],b="ltr"===d.direction?e.width-b:e.width+b,33<b&&(a.newWidth=c,e.newWidth=b)):
	(this.newWidth="ltr"===d.direction?d.tblwidth+b:d.tblwidth-b,a.newWidth=c))}},dragEnd:function(){this.hDiv.style.cursor="default";if(this.resizing){var c=this.resizing.idx,e=this.headers[c].newWidth||this.headers[c].width,e=parseInt(e,10);this.resizing=!1;b("#rs_m"+b.jgrid.jqID(d.id)).css("display","none");d.colModel[c].width=e;this.headers[c].width=e;this.headers[c].el.style.width=e+"px";this.cols[c].style.width=e+"px";0<this.footers.length&&(this.footers[c].style.width=e+"px");!0===d.forceFit?(e=
	this.headers[c+d.nv].newWidth||this.headers[c+d.nv].width,this.headers[c+d.nv].width=e,this.headers[c+d.nv].el.style.width=e+"px",this.cols[c+d.nv].style.width=e+"px",0<this.footers.length&&(this.footers[c+d.nv].style.width=e+"px"),d.colModel[c+d.nv].width=e):(d.tblwidth=this.newWidth||d.tblwidth,b("table:first",this.bDiv).css("width",d.tblwidth+"px"),b("table:first",this.hDiv).css("width",d.tblwidth+"px"),this.hDiv.scrollLeft=this.bDiv.scrollLeft,d.footerrow&&(b("table:first",this.sDiv).css("width",
	d.tblwidth+"px"),this.sDiv.scrollLeft=this.bDiv.scrollLeft));b(a).triggerHandler("jqGridResizeStop",[e,c]);b.isFunction(d.resizeStop)&&d.resizeStop.call(a,e,c)}this.curGbox=null;document.onselectstart=function(){return!0}},populateVisible:function(){c.timer&&clearTimeout(c.timer);c.timer=null;var a=b(c.bDiv).height();if(a){var e=b("table:first",c.bDiv),g,f;if(e[0].rows.length)try{f=(g=e[0].rows[1])?b(g).outerHeight()||c.prevRowHeight:c.prevRowHeight}catch(pa){f=c.prevRowHeight}if(f){c.prevRowHeight=
	f;var h=d.rowNum;g=c.scrollTop=c.bDiv.scrollTop;var k=Math.round(e.position().top)-g,m=k+e.height();f*=h;var E,n,C;m<a&&0>=k&&(void 0===d.lastpage||parseInt((m+g+f-1)/f,10)<=d.lastpage)&&(n=parseInt((a-m+f-1)/f,10),0<=m||2>n||!0===d.scroll?(E=Math.round((m+g)/f)+1,k=-1):k=1);0<k&&(E=parseInt(g/f,10)+1,n=parseInt((g+a)/f,10)+2-E,C=!0);!n||d.lastpage&&(E>d.lastpage||1===d.lastpage||E===d.page&&E===d.lastpage)||(c.hDiv.loading?c.timer=setTimeout(c.populateVisible,d.scrollTimeout):(d.page=E,C&&(c.selectionPreserver(e[0]),
	c.emptyRows.call(e[0],!1,!1)),c.populate(n)))}}},scrollGrid:function(a){if(d.scroll){var b=c.bDiv.scrollTop;void 0===c.scrollTop&&(c.scrollTop=0);b!==c.scrollTop&&(c.scrollTop=b,c.timer&&clearTimeout(c.timer),c.timer=setTimeout(c.populateVisible,d.scrollTimeout))}c.hDiv.scrollLeft=c.bDiv.scrollLeft;d.footerrow&&(c.sDiv.scrollLeft=c.bDiv.scrollLeft);a&&a.stopPropagation()},selectionPreserver:function(a){var c=a.p,d=c.selrow,e=c.selarrrow?b.makeArray(c.selarrrow):null,f=a.grid.bDiv.scrollLeft,g=function(){var h;
	c.selrow=null;c.selarrrow=[];if(c.multiselect&&e&&0<e.length)for(h=0;h<e.length;h++)e[h]!==d&&b(a).jqGrid("setSelection",e[h],!1,null);d&&b(a).jqGrid("setSelection",d,!1,null);a.grid.bDiv.scrollLeft=f;b(a).unbind(".selectionPreserver",g)};b(a).bind("jqGridGridComplete.selectionPreserver",g)}};if("TABLE"!==this.tagName.toUpperCase())alert("Element is not a table");else if(void 0!==document.documentMode&&5>=document.documentMode)alert("Grid can not be used in this ('quirks') mode!");else{b(this).empty().attr("tabindex",
	"0");this.p=d;this.p.useProp=!!b.fn.prop;var g,f;if(0===this.p.colNames.length)for(g=0;g<this.p.colModel.length;g++)this.p.colNames[g]=this.p.colModel[g].label||this.p.colModel[g].name;if(this.p.colNames.length!==this.p.colModel.length)alert(b.jgrid.errors.model);else{var k=b("<div class='ui-jqgrid-view'></div>"),n=b.jgrid.msie;a.p.direction=b.trim(a.p.direction.toLowerCase());-1===b.inArray(a.p.direction,["ltr","rtl"])&&(a.p.direction="ltr");f=a.p.direction;b(k).insertBefore(this);b(this).removeClass("scroll").appendTo(k);
	var m=b("<div class='ui-jqgrid ui-widget ui-widget-content ui-corner-all'></div>");b(m).attr({id:"gbox_"+this.id,dir:f}).insertBefore(k);b(k).attr("id","gview_"+this.id).appendTo(m);b("<div class='ui-widget-overlay jqgrid-overlay' id='lui_"+this.id+"'></div>").insertBefore(k);b("<div class='loading ui-state-default ui-state-active' id='load_"+this.id+"'>"+this.p.loadtext+"</div>").insertBefore(k);b(this).attr({cellspacing:"0",cellpadding:"0",border:"0",role:"grid","aria-multiselectable":!!this.p.multiselect,
	"aria-labelledby":"gbox_"+this.id});var r=function(a,b){a=parseInt(a,10);return isNaN(a)?b||0:a},p=function(d,e,f,g,pa,h){var k=a.p.colModel[d],m=k.align,E='style="',n=k.classes,C=k.name,A=[];m&&(E+="text-align:"+m+";");!0===k.hidden&&(E+="display:none;");if(0===e)E+="width: "+c.headers[d].width+"px;";else if(k.cellattr&&b.isFunction(k.cellattr)&&(d=k.cellattr.call(a,pa,f,g,k,h))&&"string"===typeof d)if(d=d.replace(/style/i,"style").replace(/title/i,"title"),-1<d.indexOf("title")&&(k.title=!1),-1<
	d.indexOf("class")&&(n=void 0),A=d.replace("-style","-sti").split(/style/),2===A.length){A[1]=b.trim(A[1].replace("-sti","-style").replace("=",""));if(0===A[1].indexOf("'")||0===A[1].indexOf('"'))A[1]=A[1].substring(1);E+=A[1].replace(/'/gi,'"')}else E+='"';A.length||(A[0]="",E+='"');E+=(void 0!==n?' class="'+n+'"':"")+(k.title&&f?' title="'+b.jgrid.stripHtml(f)+'"':"");E+=' aria-describedby="'+a.p.id+"_"+C+'"';return E+A[0]},q=function(c){return null==c||""===c?"&#160;":a.p.autoencode?b.jgrid.htmlEncode(c):
	String(c)},x=function(c,d,e,f,g){var h=a.p.colModel[e];void 0!==h.formatter?(c=""!==String(a.p.idPrefix)?b.jgrid.stripPref(a.p.idPrefix,c):c,c={rowId:c,colModel:h,gid:a.p.id,pos:e},d=b.isFunction(h.formatter)?h.formatter.call(a,d,c,f,g):b.fmatter?b.fn.fmatter.call(a,h.formatter,d,c,f,g):q(d)):d=q(d);return d},G=function(a,b,c,d,e,f){b=x(a,b,c,e,"add");return'<td role="gridcell" '+p(c,d,b,e,a,f)+">"+b+"</td>"},U=function(b,c,d,e){e='<input role="checkbox" type="checkbox" id="jqg_'+a.p.id+"_"+b+'" class="cbox" name="jqg_'+
	a.p.id+"_"+b+'"'+(e?'checked="checked"':"")+"/>";return'<td role="gridcell" '+p(c,d,"",null,b,!0)+">"+e+"</td>"},M=function(a,b,c,d){c=(parseInt(c,10)-1)*parseInt(d,10)+1+b;return'<td role="gridcell" class="ui-state-default jqgrid-rownum" '+p(a,b,c,null,b,!0)+">"+c+"</td>"},ea=function(b){var c,d=[],e=0,f;for(f=0;f<a.p.colModel.length;f++)c=a.p.colModel[f],"cb"!==c.name&&"subgrid"!==c.name&&"rn"!==c.name&&(d[e]="local"===b?c.name:"xml"===b||"xmlstring"===b?c.xmlmap||c.name:c.jsonmap||c.name,!1!==
	a.p.keyIndex&&!0===c.key&&(a.p.keyName=d[e]),e++);return d},W=function(c){var d=a.p.remapColumns;d&&d.length||(d=b.map(a.p.colModel,function(a,b){return b}));c&&(d=b.map(d,function(a){return a<c?null:a-c}));return d},X=function(a,c){var d;this.p.deepempty?b(this.rows).slice(1).remove():(d=0<this.rows.length?this.rows[0]:null,b(this.firstChild).empty().append(d));a&&this.p.scroll&&(b(this.grid.bDiv.firstChild).css({height:"auto"}),b(this.grid.bDiv.firstChild.firstChild).css({height:0,display:"none"}),
	0!==this.grid.bDiv.scrollTop&&(this.grid.bDiv.scrollTop=0));!0===c&&this.p.treeGrid&&(this.p.data=[],this.p._index={})},O=function(){var c=a.p.data.length,d,e,f;d=!0===a.p.rownumbers?1:0;e=!0===a.p.multiselect?1:0;f=!0===a.p.subGrid?1:0;d=!1===a.p.keyIndex||!0===a.p.loadonce?a.p.localReader.id:a.p.colModel[a.p.keyIndex+e+f+d].name;for(e=0;e<c;e++)f=b.jgrid.getAccessor(a.p.data[e],d),void 0===f&&(f=String(e+1)),a.p._index[f]=e},$=function(c,d,e,f,g,h){var l="-1",k="",m;d=d?"display:none;":"";e="ui-widget-content jqgrow ui-row-"+
	a.p.direction+(e?" "+e:"")+(h?" ui-state-highlight":"");h=b(a).triggerHandler("jqGridRowAttr",[f,g,c]);"object"!==typeof h&&(h=b.isFunction(a.p.rowattr)?a.p.rowattr.call(a,f,g,c):{});if(!b.isEmptyObject(h)){h.hasOwnProperty("id")&&(c=h.id,delete h.id);h.hasOwnProperty("tabindex")&&(l=h.tabindex,delete h.tabindex);h.hasOwnProperty("style")&&(d+=h.style,delete h.style);h.hasOwnProperty("class")&&(e+=" "+h["class"],delete h["class"]);try{delete h.role}catch(n){}for(m in h)h.hasOwnProperty(m)&&(k+=" "+
	m+"="+h[m])}return'<tr role="row" id="'+c+'" tabindex="'+l+'" class="'+e+'"'+(""===d?"":' style="'+d+'"')+k+">"},K=function(c,d,e,f,g){var h=new Date,l="local"!==a.p.datatype&&a.p.loadonce||"xmlstring"===a.p.datatype,k=a.p.xmlReader,m="local"===a.p.datatype?"local":"xml";l&&(a.p.data=[],a.p._index={},a.p.localReader.id="_id_");a.p.reccount=0;if(b.isXMLDoc(c)){-1!==a.p.treeANode||a.p.scroll?e=1<e?e:1:(X.call(a,!1,!0),e=1);var n=b(a),C,A,R=0,p,u=!0===a.p.multiselect?1:0,z=0,x,q=!0===a.p.rownumbers?
	1:0,t,Z=[],aa,v={},w,H,s=[],L=!0===a.p.altRows?a.p.altclass:"",ia;!0===a.p.subGrid&&(z=1,x=b.jgrid.getMethod("addSubGridCell"));k.repeatitems||(Z=ea(m));t=!1===a.p.keyIndex?b.isFunction(k.id)?k.id.call(a,c):k.id:a.p.keyIndex;0<Z.length&&!isNaN(t)&&(t=a.p.keyName);m=-1===String(t).indexOf("[")?Z.length?function(a,c){return b(t,a).text()||c}:function(a,c){return b(k.cell,a).eq(t).text()||c}:function(a,b){return a.getAttribute(t.replace(/[\[\]]/g,""))||b};a.p.userData={};a.p.page=r(b.jgrid.getXmlData(c,
	k.page),a.p.page);a.p.lastpage=r(b.jgrid.getXmlData(c,k.total),1);a.p.records=r(b.jgrid.getXmlData(c,k.records));b.isFunction(k.userdata)?a.p.userData=k.userdata.call(a,c)||{}:b.jgrid.getXmlData(c,k.userdata,!0).each(function(){a.p.userData[this.getAttribute("name")]=b(this).text()});c=b.jgrid.getXmlData(c,k.root,!0);(c=b.jgrid.getXmlData(c,k.row,!0))||(c=[]);var S=c.length,I=0,y=[],D=parseInt(a.p.rowNum,10),B=a.p.scroll?b.jgrid.randId():1;0<S&&0>=a.p.page&&(a.p.page=1);if(c&&S){g&&(D*=g+1);g=b.isFunction(a.p.afterInsertRow);
	var F=!1,J;a.p.grouping&&(F=!0===a.p.groupingView.groupCollapse,J=b.jgrid.getMethod("groupingPrepare"));for(;I<S;){w=c[I];H=m(w,B+I);H=a.p.idPrefix+H;C=0===e?0:e+1;ia=1===(C+I)%2?L:"";var K=s.length;s.push("");q&&s.push(M(0,I,a.p.page,a.p.rowNum));u&&s.push(U(H,q,I,!1));z&&s.push(x.call(n,u+q,I+e));if(k.repeatitems){aa||(aa=W(u+z+q));var N=b.jgrid.getXmlData(w,k.cell,!0);b.each(aa,function(b){var c=N[this];if(!c)return!1;p=c.textContent||c.text;v[a.p.colModel[b+u+z+q].name]=p;s.push(G(H,p,b+u+z+q,
	I+e,w,v))})}else for(C=0;C<Z.length;C++)p=b.jgrid.getXmlData(w,Z[C]),v[a.p.colModel[C+u+z+q].name]=p,s.push(G(H,p,C+u+z+q,I+e,w,v));s[K]=$(H,F,ia,v,w,!1);s.push("</tr>");a.p.grouping&&(y.push(s),a.p.groupingView._locgr||J.call(n,v,I),s=[]);if(l||!0===a.p.treeGrid)v._id_=b.jgrid.stripPref(a.p.idPrefix,H),a.p.data.push(v),a.p._index[v._id_]=a.p.data.length-1;!1===a.p.gridview&&(b("tbody:first",d).append(s.join("")),n.triggerHandler("jqGridAfterInsertRow",[H,v,w]),g&&a.p.afterInsertRow.call(a,H,v,w),
	s=[]);v={};R++;I++;if(R===D)break}}!0===a.p.gridview&&(A=-1<a.p.treeANode?a.p.treeANode:0,a.p.grouping?(l||n.jqGrid("groupingRender",y,a.p.colModel.length,a.p.page,D),y=null):!0===a.p.treeGrid&&0<A?b(a.rows[A]).after(s.join("")):b("tbody:first",d).append(s.join("")));if(!0===a.p.subGrid)try{n.jqGrid("addSubGrid",u+q)}catch(Q){}a.p.totaltime=new Date-h;0<R&&0===a.p.records&&(a.p.records=S);s=null;if(!0===a.p.treeGrid)try{n.jqGrid("setTreeNode",A+1,R+A+1)}catch(O){}a.p.treeGrid||a.p.scroll||(a.grid.bDiv.scrollTop=
	0);a.p.reccount=R;a.p.treeANode=-1;a.p.userDataOnFooter&&n.jqGrid("footerData","set",a.p.userData,!0);l&&(a.p.records=S,a.p.lastpage=Math.ceil(S/D));f||a.updatepager(!1,!0);if(l){for(;R<S;){w=c[R];H=m(w,R+B);H=a.p.idPrefix+H;if(k.repeatitems){aa||(aa=W(u+z+q));var P=b.jgrid.getXmlData(w,k.cell,!0);b.each(aa,function(b){var c=P[this];if(!c)return!1;p=c.textContent||c.text;v[a.p.colModel[b+u+z+q].name]=p})}else for(C=0;C<Z.length;C++)p=b.jgrid.getXmlData(w,Z[C]),v[a.p.colModel[C+u+z+q].name]=p;v._id_=
	b.jgrid.stripPref(a.p.idPrefix,H);a.p.grouping&&J.call(n,v,R);a.p.data.push(v);a.p._index[v._id_]=a.p.data.length-1;v={};R++}a.p.grouping&&(a.p.groupingView._locgr=!0,n.jqGrid("groupingRender",y,a.p.colModel.length,a.p.page,D),y=null)}}},Y=function(c,d,e,f,g){var h=new Date;if(c){-1!==a.p.treeANode||a.p.scroll?e=1<e?e:1:(X.call(a,!1,!0),e=1);var k,l="local"!==a.p.datatype&&a.p.loadonce||"jsonstring"===a.p.datatype;l&&(a.p.data=[],a.p._index={},a.p.localReader.id="_id_");a.p.reccount=0;"local"===a.p.datatype?
	(d=a.p.localReader,k="local"):(d=a.p.jsonReader,k="json");var m=b(a),n=0,C,A,p,q=[],u=a.p.multiselect?1:0,z=!0===a.p.subGrid?1:0,x,t=!0===a.p.rownumbers?1:0,D=W(u+z+t);k=ea(k);var y,B,v,w={},H,s,L=[],ia=!0===a.p.altRows?a.p.altclass:"",S;a.p.page=r(b.jgrid.getAccessor(c,d.page),a.p.page);a.p.lastpage=r(b.jgrid.getAccessor(c,d.total),1);a.p.records=r(b.jgrid.getAccessor(c,d.records));a.p.userData=b.jgrid.getAccessor(c,d.userdata)||{};z&&(x=b.jgrid.getMethod("addSubGridCell"));v=!1===a.p.keyIndex?b.isFunction(d.id)?
	d.id.call(a,c):d.id:a.p.keyIndex;d.repeatitems||(q=k,0<q.length&&!isNaN(v)&&(v=a.p.keyName));B=b.jgrid.getAccessor(c,d.root);null==B&&b.isArray(c)&&(B=c);B||(B=[]);c=B.length;A=0;0<c&&0>=a.p.page&&(a.p.page=1);var I=parseInt(a.p.rowNum,10),F=a.p.scroll?b.jgrid.randId():1,J=!1,K;g&&(I*=g+1);"local"!==a.p.datatype||a.p.deselectAfterSort||(J=!0);var N=b.isFunction(a.p.afterInsertRow),P=[],Q=!1,O;a.p.grouping&&(Q=!0===a.p.groupingView.groupCollapse,O=b.jgrid.getMethod("groupingPrepare"));for(;A<c;){g=
	B[A];s=b.jgrid.getAccessor(g,v);void 0===s&&("number"===typeof v&&null!=a.p.colModel[v+u+z+t]&&(s=b.jgrid.getAccessor(g,a.p.colModel[v+u+z+t].name)),void 0===s&&(s=F+A,0===q.length&&d.cell&&(C=b.jgrid.getAccessor(g,d.cell)||g,s=null!=C&&void 0!==C[v]?C[v]:s)));s=a.p.idPrefix+s;C=1===e?0:e;S=1===(C+A)%2?ia:"";J&&(K=a.p.multiselect?-1!==b.inArray(s,a.p.selarrrow):s===a.p.selrow);var T=L.length;L.push("");t&&L.push(M(0,A,a.p.page,a.p.rowNum));u&&L.push(U(s,t,A,K));z&&L.push(x.call(m,u+t,A+e));y=k;d.repeatitems&&
	(d.cell&&(g=b.jgrid.getAccessor(g,d.cell)||g),b.isArray(g)&&(y=D));for(p=0;p<y.length;p++)C=b.jgrid.getAccessor(g,y[p]),w[a.p.colModel[p+u+z+t].name]=C,L.push(G(s,C,p+u+z+t,A+e,g,w));L[T]=$(s,Q,S,w,g,K);L.push("</tr>");a.p.grouping&&(P.push(L),a.p.groupingView._locgr||O.call(m,w,A),L=[]);if(l||!0===a.p.treeGrid)w._id_=b.jgrid.stripPref(a.p.idPrefix,s),a.p.data.push(w),a.p._index[w._id_]=a.p.data.length-1;!1===a.p.gridview&&(b("#"+b.jgrid.jqID(a.p.id)+" tbody:first").append(L.join("")),m.triggerHandler("jqGridAfterInsertRow",
	[s,w,g]),N&&a.p.afterInsertRow.call(a,s,w,g),L=[]);w={};n++;A++;if(n===I)break}!0===a.p.gridview&&(H=-1<a.p.treeANode?a.p.treeANode:0,a.p.grouping?l||(m.jqGrid("groupingRender",P,a.p.colModel.length,a.p.page,I),P=null):!0===a.p.treeGrid&&0<H?b(a.rows[H]).after(L.join("")):b("#"+b.jgrid.jqID(a.p.id)+" tbody:first").append(L.join("")));if(!0===a.p.subGrid)try{m.jqGrid("addSubGrid",u+t)}catch(V){}a.p.totaltime=new Date-h;0<n&&0===a.p.records&&(a.p.records=c);if(!0===a.p.treeGrid)try{m.jqGrid("setTreeNode",
	H+1,n+H+1)}catch(Y){}a.p.treeGrid||a.p.scroll||(a.grid.bDiv.scrollTop=0);a.p.reccount=n;a.p.treeANode=-1;a.p.userDataOnFooter&&m.jqGrid("footerData","set",a.p.userData,!0);l&&(a.p.records=c,a.p.lastpage=Math.ceil(c/I));f||a.updatepager(!1,!0);if(l){for(;n<c&&B[n];){g=B[n];s=b.jgrid.getAccessor(g,v);void 0===s&&("number"===typeof v&&null!=a.p.colModel[v+u+z+t]&&(s=b.jgrid.getAccessor(g,a.p.colModel[v+u+z+t].name)),void 0===s&&(s=F+n,0===q.length&&d.cell&&(e=b.jgrid.getAccessor(g,d.cell)||g,s=null!=
	e&&void 0!==e[v]?e[v]:s)));if(g){s=a.p.idPrefix+s;y=k;d.repeatitems&&(d.cell&&(g=b.jgrid.getAccessor(g,d.cell)||g),b.isArray(g)&&(y=D));for(p=0;p<y.length;p++)w[a.p.colModel[p+u+z+t].name]=b.jgrid.getAccessor(g,y[p]);w._id_=b.jgrid.stripPref(a.p.idPrefix,s);a.p.grouping&&O.call(m,w,n);a.p.data.push(w);a.p._index[w._id_]=a.p.data.length-1;w={}}n++}a.p.grouping&&(a.p.groupingView._locgr=!0,m.jqGrid("groupingRender",P,a.p.colModel.length,a.p.page,I))}}},oa=function(){function c(a){var b=0,d,e,g,h,k;
	if(null!=a.groups){(e=a.groups.length&&"OR"===a.groupOp.toString().toUpperCase())&&u.orBegin();for(d=0;d<a.groups.length;d++){0<b&&e&&u.or();try{c(a.groups[d])}catch(l){alert(l)}b++}e&&u.orEnd()}if(null!=a.rules)try{(g=a.rules.length&&"OR"===a.groupOp.toString().toUpperCase())&&u.orBegin();for(d=0;d<a.rules.length;d++)k=a.rules[d],h=a.groupOp.toString().toUpperCase(),q[k.op]&&k.field&&(0<b&&h&&"OR"===h&&(u=u.or()),u=q[k.op](u,h)(k.field,k.data,f[k.field])),b++;g&&u.orEnd()}catch(m){alert(m)}}var d=
	a.p.multiSort?[]:"",e=[],g=!1,f={},h=[],k=[],l,m,n;if(b.isArray(a.p.data)){var p=a.p.grouping?a.p.groupingView:!1,A,r;b.each(a.p.colModel,function(){m=this.sorttype||"text";"date"===m||"datetime"===m?(this.formatter&&"string"===typeof this.formatter&&"date"===this.formatter?(l=this.formatoptions&&this.formatoptions.srcformat?this.formatoptions.srcformat:b.jgrid.formatter.date.srcformat,n=this.formatoptions&&this.formatoptions.newformat?this.formatoptions.newformat:b.jgrid.formatter.date.newformat):
	l=n=this.datefmt||"Y-m-d",f[this.name]={stype:m,srcfmt:l,newfmt:n,sfunc:this.sortfunc||null}):f[this.name]={stype:m,srcfmt:"",newfmt:"",sfunc:this.sortfunc||null};if(a.p.grouping)for(r=0,A=p.groupField.length;r<A;r++)if(this.name===p.groupField[r]){var c=this.name;this.index&&(c=this.index);h[r]=f[c];k[r]=c}a.p.multiSort?this.lso&&(d.push(this.name),c=this.lso.split("-"),e.push(c[c.length-1])):g||this.index!==a.p.sortname&&this.name!==a.p.sortname||(d=this.name,g=!0)});if(a.p.treeGrid)b(a).jqGrid("SortTree",
	d,a.p.sortorder,f[d].stype||"text",f[d].srcfmt||"");else{var q={eq:function(a){return a.equals},ne:function(a){return a.notEquals},lt:function(a){return a.less},le:function(a){return a.lessOrEquals},gt:function(a){return a.greater},ge:function(a){return a.greaterOrEquals},cn:function(a){return a.contains},nc:function(a,b){return"OR"===b?a.orNot().contains:a.andNot().contains},bw:function(a){return a.startsWith},bn:function(a,b){return"OR"===b?a.orNot().startsWith:a.andNot().startsWith},en:function(a,
	b){return"OR"===b?a.orNot().endsWith:a.andNot().endsWith},ew:function(a){return a.endsWith},ni:function(a,b){return"OR"===b?a.orNot().equals:a.andNot().equals},"in":function(a){return a.equals},nu:function(a){return a.isNull},nn:function(a,b){return"OR"===b?a.orNot().isNull:a.andNot().isNull}},u=b.jgrid.from(a.p.data);a.p.ignoreCase&&(u=u.ignoreCase());if(!0===a.p.search){var z=a.p.postData.filters;if(z)"string"===typeof z&&(z=b.jgrid.parse(z)),c(z);else try{u=q[a.p.postData.searchOper](u)(a.p.postData.searchField,
	a.p.postData.searchString,f[a.p.postData.searchField])}catch(t){}}if(a.p.grouping)for(r=0;r<A;r++)u.orderBy(k[r],p.groupOrder[r],h[r].stype,h[r].srcfmt);a.p.multiSort?b.each(d,function(a){u.orderBy(this,e[a],f[this].stype,f[this].srcfmt,f[this].sfunc)}):d&&a.p.sortorder&&g&&("DESC"===a.p.sortorder.toUpperCase()?u.orderBy(a.p.sortname,"d",f[d].stype,f[d].srcfmt,f[d].sfunc):u.orderBy(a.p.sortname,"a",f[d].stype,f[d].srcfmt,f[d].sfunc));var z=u.select(),x=parseInt(a.p.rowNum,10),y=z.length,B=parseInt(a.p.page,
	10),D=Math.ceil(y/x),v={};if((a.p.search||a.p.resetsearch)&&a.p.grouping&&a.p.groupingView._locgr){a.p.groupingView.groups=[];var w,G=b.jgrid.getMethod("groupingPrepare"),s,F;if(a.p.footerrow&&a.p.userDataOnFooter){for(s in a.p.userData)a.p.userData.hasOwnProperty(s)&&(a.p.userData[s]=0);F=!0}for(w=0;w<y;w++){if(F)for(s in a.p.userData)a.p.userData[s]+=parseFloat(z[w][s]||0);G.call(b(a),z[w],w,x)}}z=z.slice((B-1)*x,B*x);f=u=null;v[a.p.localReader.total]=D;v[a.p.localReader.page]=B;v[a.p.localReader.records]=
	y;v[a.p.localReader.root]=z;v[a.p.localReader.userdata]=a.p.userData;z=null;return v}}},P=function(){a.grid.hDiv.loading=!0;if(!a.p.hiddengrid)switch(a.p.loadui){case "enable":b("#load_"+b.jgrid.jqID(a.p.id)).show();break;case "block":b("#lui_"+b.jgrid.jqID(a.p.id)).show(),b("#load_"+b.jgrid.jqID(a.p.id)).show()}},T=function(){a.grid.hDiv.loading=!1;switch(a.p.loadui){case "enable":b("#load_"+b.jgrid.jqID(a.p.id)).hide();break;case "block":b("#lui_"+b.jgrid.jqID(a.p.id)).hide(),b("#load_"+b.jgrid.jqID(a.p.id)).hide()}},
	Q=function(c){if(!a.grid.hDiv.loading){var d=a.p.scroll&&!1===c,e={},g,f=a.p.prmNames;0>=a.p.page&&(a.p.page=Math.min(1,a.p.lastpage));null!==f.search&&(e[f.search]=a.p.search);null!==f.nd&&(e[f.nd]=(new Date).getTime());null!==f.rows&&(e[f.rows]=a.p.rowNum);null!==f.page&&(e[f.page]=a.p.page);null!==f.sort&&(e[f.sort]=a.p.sortname);null!==f.order&&(e[f.order]=a.p.sortorder);null!==a.p.rowTotal&&null!==f.totalrows&&(e[f.totalrows]=a.p.rowTotal);var h=b.isFunction(a.p.loadComplete),k=h?a.p.loadComplete:
	null,l=0;c=c||1;1<c?null!==f.npage?(e[f.npage]=c,l=c-1,c=1):k=function(b){a.p.page++;a.grid.hDiv.loading=!1;h&&a.p.loadComplete.call(a,b);Q(c-1)}:null!==f.npage&&delete a.p.postData[f.npage];if(a.p.grouping){b(a).jqGrid("groupingSetup");var m=a.p.groupingView,n,p="";for(n=0;n<m.groupField.length;n++){var r=m.groupField[n];b.each(a.p.colModel,function(a,b){b.name===r&&b.index&&(r=b.index)});p+=r+" "+m.groupOrder[n]+", "}e[f.sort]=p+e[f.sort]}b.extend(a.p.postData,e);var q=a.p.scroll?a.rows.length-
	1:1,e=b(a).triggerHandler("jqGridBeforeRequest");if(!1!==e&&"stop"!==e)if(b.isFunction(a.p.datatype))a.p.datatype.call(a,a.p.postData,"load_"+a.p.id,q,c,l);else{if(b.isFunction(a.p.beforeRequest)&&(e=a.p.beforeRequest.call(a),void 0===e&&(e=!0),!1===e))return;g=a.p.datatype.toLowerCase();switch(g){case "json":case "jsonp":case "xml":case "script":b.ajax(b.extend({url:a.p.url,type:a.p.mtype,dataType:g,data:b.isFunction(a.p.serializeGridData)?a.p.serializeGridData.call(a,a.p.postData):a.p.postData,
	success:function(e,f,h){if(b.isFunction(a.p.beforeProcessing)&&!1===a.p.beforeProcessing.call(a,e,f,h))T();else{"xml"===g?K(e,a.grid.bDiv,q,1<c,l):Y(e,a.grid.bDiv,q,1<c,l);b(a).triggerHandler("jqGridLoadComplete",[e]);k&&k.call(a,e);b(a).triggerHandler("jqGridAfterLoadComplete",[e]);d&&a.grid.populateVisible();if(a.p.loadonce||a.p.treeGrid)a.p.datatype="local";1===c&&T()}},error:function(d,e,f){b.isFunction(a.p.loadError)&&a.p.loadError.call(a,d,e,f);1===c&&T()},beforeSend:function(c,d){var e=!0;
	b.isFunction(a.p.loadBeforeSend)&&(e=a.p.loadBeforeSend.call(a,c,d));void 0===e&&(e=!0);if(!1===e)return!1;P()}},b.jgrid.ajaxOptions,a.p.ajaxGridOptions));break;case "xmlstring":P();e="string"!==typeof a.p.datastr?a.p.datastr:b.parseXML(a.p.datastr);K(e,a.grid.bDiv);b(a).triggerHandler("jqGridLoadComplete",[e]);h&&a.p.loadComplete.call(a,e);b(a).triggerHandler("jqGridAfterLoadComplete",[e]);a.p.datatype="local";a.p.datastr=null;T();break;case "jsonstring":P();e="string"===typeof a.p.datastr?b.jgrid.parse(a.p.datastr):
	a.p.datastr;Y(e,a.grid.bDiv);b(a).triggerHandler("jqGridLoadComplete",[e]);h&&a.p.loadComplete.call(a,e);b(a).triggerHandler("jqGridAfterLoadComplete",[e]);a.p.datatype="local";a.p.datastr=null;T();break;case "local":case "clientside":P(),a.p.datatype="local",e=oa(),Y(e,a.grid.bDiv,q,1<c,l),b(a).triggerHandler("jqGridLoadComplete",[e]),k&&k.call(a,e),b(a).triggerHandler("jqGridAfterLoadComplete",[e]),d&&a.grid.populateVisible(),T()}}}},ha=function(c){b("#cb_"+b.jgrid.jqID(a.p.id),a.grid.hDiv)[a.p.useProp?
	"prop":"attr"]("checked",c);if(a.p.frozenColumns&&a.p.id+"_frozen")b("#cb_"+b.jgrid.jqID(a.p.id),a.grid.fhDiv)[a.p.useProp?"prop":"attr"]("checked",c)},qa=function(c,e){var d="",g="<table cellspacing='0' cellpadding='0' border='0' style='table-layout:auto;' class='ui-pg-table'><tbody><tr>",k="",l,m,n,p,q=function(c){var e;b.isFunction(a.p.onPaging)&&(e=a.p.onPaging.call(a,c));if("stop"===e)return!1;a.p.selrow=null;a.p.multiselect&&(a.p.selarrrow=[],ha(!1));a.p.savedRow=[];return!0};c=c.substr(1);
	e+="_"+c;l="pg_"+c;m=c+"_left";n=c+"_center";p=c+"_right";b("#"+b.jgrid.jqID(c)).append("<div id='"+l+"' class='ui-pager-control' role='group'><table cellspacing='0' cellpadding='0' border='0' class='ui-pg-table' style='width:100%;table-layout:fixed;height:100%;' role='row'><tbody><tr><td id='"+m+"' align='left'></td><td id='"+n+"' align='center' style='white-space:pre;'></td><td id='"+p+"' align='right'></td></tr></tbody></table></div>").attr("dir","ltr");if(0<a.p.rowList.length){k="<td dir='"+f+
	"'>";k+="<select class='ui-pg-selbox' role='listbox'>";for(m=0;m<a.p.rowList.length;m++)k+='<option role="option" value="'+a.p.rowList[m]+'"'+(a.p.rowNum===a.p.rowList[m]?' selected="selected"':"")+">"+a.p.rowList[m]+"</option>";k+="</select></td>"}"rtl"===f&&(g+=k);!0===a.p.pginput&&(d="<td dir='"+f+"'>"+b.jgrid.format(a.p.pgtext||"","<input class='ui-pg-input' type='text' size='2' maxlength='7' value='0' role='textbox'/>","<span id='sp_1_"+b.jgrid.jqID(c)+"'></span>")+"</td>");!0===a.p.pgbuttons?
	(m=["first"+e,"prev"+e,"next"+e,"last"+e],"rtl"===f&&m.reverse(),g+="<td id='"+m[0]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-first'></span></td>",g+="<td id='"+m[1]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-prev'></span></td>",g=g+(""!==d?"<td class='ui-pg-button ui-state-disabled' style='width:4px;'><span class='ui-separator'></span></td>"+d+"<td class='ui-pg-button ui-state-disabled' style='width:4px;'><span class='ui-separator'></span></td>":
	"")+("<td id='"+m[2]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-next'></span></td>"),g+="<td id='"+m[3]+"' class='ui-pg-button ui-corner-all'><span class='ui-icon ui-icon-seek-end'></span></td>"):""!==d&&(g+=d);"ltr"===f&&(g+=k);g+="</tr></tbody></table>";!0===a.p.viewrecords&&b("td#"+c+"_"+a.p.recordpos,"#"+l).append("<div dir='"+f+"' style='text-align:"+a.p.recordpos+"' class='ui-paging-info'></div>");b("td#"+c+"_"+a.p.pagerpos,"#"+l).append(g);k=b(".ui-jqgrid").css("font-size")||
	"11px";b(document.body).append("<div id='testpg' class='ui-jqgrid ui-widget ui-widget-content' style='font-size:"+k+";visibility:hidden;' ></div>");g=b(g).clone().appendTo("#testpg").width();b("#testpg").remove();0<g&&(""!==d&&(g+=50),b("td#"+c+"_"+a.p.pagerpos,"#"+l).width(g));a.p._nvtd=[];a.p._nvtd[0]=g?Math.floor((a.p.width-g)/2):Math.floor(a.p.width/3);a.p._nvtd[1]=0;g=null;b(".ui-pg-selbox","#"+l).bind("change",function(){if(!q("records"))return!1;a.p.page=Math.round(a.p.rowNum*(a.p.page-1)/
	this.value-0.5)+1;a.p.rowNum=this.value;a.p.pager&&b(".ui-pg-selbox",a.p.pager).val(this.value);a.p.toppager&&b(".ui-pg-selbox",a.p.toppager).val(this.value);Q();return!1});!0===a.p.pgbuttons&&(b(".ui-pg-button","#"+l).hover(function(){b(this).hasClass("ui-state-disabled")?this.style.cursor="default":(b(this).addClass("ui-state-hover"),this.style.cursor="pointer")},function(){b(this).hasClass("ui-state-disabled")||(b(this).removeClass("ui-state-hover"),this.style.cursor="default")}),b("#first"+b.jgrid.jqID(e)+
	", #prev"+b.jgrid.jqID(e)+", #next"+b.jgrid.jqID(e)+", #last"+b.jgrid.jqID(e)).click(function(){if(b(this).hasClass("ui-state-disabled"))return!1;var c=r(a.p.page,1),d=r(a.p.lastpage,1),g=!1,f=!0,h=!0,k=!0,l=!0;0===d||1===d?l=k=h=f=!1:1<d&&1<=c?1===c?h=f=!1:c===d&&(l=k=!1):1<d&&0===c&&(l=k=!1,c=d-1);if(!q(this.id))return!1;this.id==="first"+e&&f&&(a.p.page=1,g=!0);this.id==="prev"+e&&h&&(a.p.page=c-1,g=!0);this.id==="next"+e&&k&&(a.p.page=c+1,g=!0);this.id==="last"+e&&l&&(a.p.page=d,g=!0);g&&Q();
	return!1}));!0===a.p.pginput&&b("input.ui-pg-input","#"+l).keypress(function(c){if(13===(c.charCode||c.keyCode||0)){if(!q("user"))return!1;b(this).val(r(b(this).val(),1));a.p.page=0<b(this).val()?b(this).val():a.p.page;Q();return!1}return this})},wa=function(c,e){var d,g="",f=a.p.colModel,h=!1,k;k=a.p.frozenColumns?e:a.grid.headers[c].el;var l="";b("span.ui-grid-ico-sort",k).addClass("ui-state-disabled");b(k).attr("aria-selected","false");if(f[c].lso)if("asc"===f[c].lso)f[c].lso+="-desc",l="desc";
	else if("desc"===f[c].lso)f[c].lso+="-asc",l="asc";else{if("asc-desc"===f[c].lso||"desc-asc"===f[c].lso)f[c].lso=""}else f[c].lso=l=f[c].firstsortorder||"asc";l?(b("span.s-ico",k).show(),b("span.ui-icon-"+l,k).removeClass("ui-state-disabled"),b(k).attr("aria-selected","true")):a.p.viewsortcols[0]||b("span.s-ico",k).hide();a.p.sortorder="";b.each(f,function(b){this.lso&&(0<b&&h&&(g+=", "),d=this.lso.split("-"),g+=f[b].index||f[b].name,g+=" "+d[d.length-1],h=!0,a.p.sortorder=d[d.length-1])});k=g.lastIndexOf(a.p.sortorder);
	g=g.substring(0,k);a.p.sortname=g},ra=function(c,d,e,g,f){if(a.p.colModel[d].sortable&&!(0<a.p.savedRow.length)){e||(a.p.lastsort===d?"asc"===a.p.sortorder?a.p.sortorder="desc":"desc"===a.p.sortorder&&(a.p.sortorder="asc"):a.p.sortorder=a.p.colModel[d].firstsortorder||"asc",a.p.page=1);if(a.p.multiSort)wa(d,f);else{if(g){if(a.p.lastsort===d&&a.p.sortorder===g&&!e)return;a.p.sortorder=g}e=a.grid.headers[a.p.lastsort].el;f=a.p.frozenColumns?f:a.grid.headers[d].el;b("span.ui-grid-ico-sort",e).addClass("ui-state-disabled");
	b(e).attr("aria-selected","false");a.p.frozenColumns&&(a.grid.fhDiv.find("span.ui-grid-ico-sort").addClass("ui-state-disabled"),a.grid.fhDiv.find("th").attr("aria-selected","false"));b("span.ui-icon-"+a.p.sortorder,f).removeClass("ui-state-disabled");b(f).attr("aria-selected","true");a.p.viewsortcols[0]||a.p.lastsort===d||(a.p.frozenColumns&&a.grid.fhDiv.find("span.s-ico").hide(),b("span.s-ico",e).hide(),b("span.s-ico",f).show());c=c.substring(5+a.p.id.length+1);a.p.sortname=a.p.colModel[d].index||
	c}"stop"===b(a).triggerHandler("jqGridSortCol",[a.p.sortname,d,a.p.sortorder])?a.p.lastsort=d:b.isFunction(a.p.onSortCol)&&"stop"===a.p.onSortCol.call(a,a.p.sortname,d,a.p.sortorder)?a.p.lastsort=d:("local"===a.p.datatype?a.p.deselectAfterSort&&b(a).jqGrid("resetSelection"):(a.p.selrow=null,a.p.multiselect&&ha(!1),a.p.selarrrow=[],a.p.savedRow=[]),a.p.scroll&&(f=a.grid.bDiv.scrollLeft,X.call(a,!0,!1),a.grid.hDiv.scrollLeft=f),a.p.subGrid&&"local"===a.p.datatype&&b("td.sgexpanded","#"+b.jgrid.jqID(a.p.id)).each(function(){b(this).trigger("click")}),
	Q(),a.p.lastsort=d,a.p.sortname!==c&&d&&(a.p.lastsort=d))}},xa=function(c){c=b(a.grid.headers[c].el);c=[c.position().left+c.outerWidth()];"rtl"===a.p.direction&&(c[0]=a.p.width-c[0]);c[0]-=a.grid.bDiv.scrollLeft;c.push(b(a.grid.hDiv).position().top);c.push(b(a.grid.bDiv).offset().top-b(a.grid.hDiv).offset().top+b(a.grid.bDiv).height());return c},sa=function(c){var d,e=a.grid.headers,g=b.jgrid.getCellIndex(c);for(d=0;d<e.length;d++)if(c===e[d].el){g=d;break}return g};this.p.id=this.id;-1===b.inArray(a.p.multikey,
	["shiftKey","altKey","ctrlKey"])&&(a.p.multikey=!1);a.p.keyIndex=!1;a.p.keyName=!1;for(g=0;g<a.p.colModel.length;g++)a.p.colModel[g]=b.extend(!0,{},a.p.cmTemplate,a.p.colModel[g].template||{},a.p.colModel[g]),!1===a.p.keyIndex&&!0===a.p.colModel[g].key&&(a.p.keyIndex=g);a.p.sortorder=a.p.sortorder.toLowerCase();b.jgrid.cell_width=b.jgrid.cellWidth();!0===a.p.grouping&&(a.p.scroll=!1,a.p.rownumbers=!1,a.p.treeGrid=!1,a.p.gridview=!0);if(!0===this.p.treeGrid){try{b(this).jqGrid("setTreeGrid")}catch(za){}"local"!==
	a.p.datatype&&(a.p.localReader={id:"_id_"})}if(this.p.subGrid)try{b(a).jqGrid("setSubGrid")}catch(Aa){}this.p.multiselect&&(this.p.colNames.unshift("<input role='checkbox' id='cb_"+this.p.id+"' class='cbox' type='checkbox'/>"),this.p.colModel.unshift({name:"cb",width:b.jgrid.cell_width?a.p.multiselectWidth+a.p.cellLayout:a.p.multiselectWidth,sortable:!1,resizable:!1,hidedlg:!0,search:!1,align:"center",fixed:!0}));this.p.rownumbers&&(this.p.colNames.unshift(""),this.p.colModel.unshift({name:"rn",width:a.p.rownumWidth,
	sortable:!1,resizable:!1,hidedlg:!0,search:!1,align:"center",fixed:!0}));a.p.xmlReader=b.extend(!0,{root:"rows",row:"row",page:"rows>page",total:"rows>total",records:"rows>records",repeatitems:!0,cell:"cell",id:"[id]",userdata:"userdata",subgrid:{root:"rows",row:"row",repeatitems:!0,cell:"cell"}},a.p.xmlReader);a.p.jsonReader=b.extend(!0,{root:"rows",page:"page",total:"total",records:"records",repeatitems:!0,cell:"cell",id:"id",userdata:"userdata",subgrid:{root:"rows",repeatitems:!0,cell:"cell"}},
	a.p.jsonReader);a.p.localReader=b.extend(!0,{root:"rows",page:"page",total:"total",records:"records",repeatitems:!1,cell:"cell",id:"id",userdata:"userdata",subgrid:{root:"rows",repeatitems:!0,cell:"cell"}},a.p.localReader);a.p.scroll&&(a.p.pgbuttons=!1,a.p.pginput=!1,a.p.rowList=[]);a.p.data.length&&O();var D="<thead><tr class='ui-jqgrid-labels' role='rowheader'>",ta,F,ja,fa,ka,y,t,ba,ua=ba="",ga=[],va=[];F=[];if(!0===a.p.shrinkToFit&&!0===a.p.forceFit)for(g=a.p.colModel.length-1;0<=g;g--)if(!a.p.colModel[g].hidden){a.p.colModel[g].resizable=
	!1;break}"horizontal"===a.p.viewsortcols[1]&&(ba=" ui-i-asc",ua=" ui-i-desc");ta=n?"class='ui-th-div-ie'":"";ba="<span class='s-ico' style='display:none'><span sort='asc' class='ui-grid-ico-sort ui-icon-asc"+ba+" ui-state-disabled ui-icon ui-icon-triangle-1-n ui-sort-"+f+"'></span>"+("<span sort='desc' class='ui-grid-ico-sort ui-icon-desc"+ua+" ui-state-disabled ui-icon ui-icon-triangle-1-s ui-sort-"+f+"'></span></span>");if(a.p.multiSort)for(ga=a.p.sortname.split(","),g=0;g<ga.length;g++)F=b.trim(ga[g]).split(" "),
	ga[g]=b.trim(F[0]),va[g]=F[1]?b.trim(F[1]):a.p.sortorder||"asc";for(g=0;g<this.p.colNames.length;g++)F=a.p.headertitles?' title="'+b.jgrid.stripHtml(a.p.colNames[g])+'"':"",D+="<th id='"+a.p.id+"_"+a.p.colModel[g].name+"' role='columnheader' class='ui-state-default ui-th-column ui-th-"+f+"'"+F+">",F=a.p.colModel[g].index||a.p.colModel[g].name,D+="<div id='jqgh_"+a.p.id+"_"+a.p.colModel[g].name+"' "+ta+">"+a.p.colNames[g],a.p.colModel[g].width=a.p.colModel[g].width?parseInt(a.p.colModel[g].width,10):
	150,"boolean"!==typeof a.p.colModel[g].title&&(a.p.colModel[g].title=!0),a.p.colModel[g].lso="",F===a.p.sortname&&(a.p.lastsort=g),a.p.multiSort&&(F=b.inArray(F,ga),-1!==F&&(a.p.colModel[g].lso=va[F])),D+=ba+"</div></th>";D+="</tr></thead>";ba=null;b(this).append(D);b("thead tr:first th",this).hover(function(){b(this).addClass("ui-state-hover")},function(){b(this).removeClass("ui-state-hover")});if(this.p.multiselect){var la=[],ca;b("#cb_"+b.jgrid.jqID(a.p.id),this).bind("click",function(){a.p.selarrrow=
	[];var c=!0===a.p.frozenColumns?a.p.id+"_frozen":"";this.checked?(b(a.rows).each(function(d){0<d&&!b(this).hasClass("ui-subgrid")&&!b(this).hasClass("jqgroup")&&!b(this).hasClass("ui-state-disabled")&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id))[a.p.useProp?"prop":"attr"]("checked",!0),b(this).addClass("ui-state-highlight").attr("aria-selected","true"),a.p.selarrrow.push(this.id),a.p.selrow=this.id,c&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id),a.grid.fbDiv)[a.p.useProp?
	"prop":"attr"]("checked",!0),b("#"+b.jgrid.jqID(this.id),a.grid.fbDiv).addClass("ui-state-highlight")))}),ca=!0,la=[]):(b(a.rows).each(function(d){0<d&&!b(this).hasClass("ui-subgrid")&&!b(this).hasClass("ui-state-disabled")&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id))[a.p.useProp?"prop":"attr"]("checked",!1),b(this).removeClass("ui-state-highlight").attr("aria-selected","false"),la.push(this.id),c&&(b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(this.id),a.grid.fbDiv)[a.p.useProp?
	"prop":"attr"]("checked",!1),b("#"+b.jgrid.jqID(this.id),a.grid.fbDiv).removeClass("ui-state-highlight")))}),a.p.selrow=null,ca=!1);b(a).triggerHandler("jqGridSelectAll",[ca?a.p.selarrrow:la,ca]);b.isFunction(a.p.onSelectAll)&&a.p.onSelectAll.call(a,ca?a.p.selarrrow:la,ca)})}!0===a.p.autowidth&&(D=b(m).innerWidth(),a.p.width=0<D?D:"nw");(function(){var d=0,e=b.jgrid.cell_width?0:r(a.p.cellLayout,0),g=0,f,h=r(a.p.scrollOffset,0),k,m=!1,n,p=0,q;b.each(a.p.colModel,function(){void 0===this.hidden&&(this.hidden=
	!1);if(a.p.grouping&&a.p.autowidth){var c=b.inArray(this.name,a.p.groupingView.groupField);0<=c&&a.p.groupingView.groupColumnShow.length>c&&(this.hidden=!a.p.groupingView.groupColumnShow[c])}this.widthOrg=k=r(this.width,0);!1===this.hidden&&(d+=k+e,this.fixed?p+=k+e:g++)});isNaN(a.p.width)&&(a.p.width=d+(!1!==a.p.shrinkToFit||isNaN(a.p.height)?0:h));c.width=a.p.width;a.p.tblwidth=d;!1===a.p.shrinkToFit&&!0===a.p.forceFit&&(a.p.forceFit=!1);!0===a.p.shrinkToFit&&0<g&&(n=c.width-e*g-p,isNaN(a.p.height)||
	(n-=h,m=!0),d=0,b.each(a.p.colModel,function(b){!1!==this.hidden||this.fixed||(this.width=k=Math.round(n*this.width/(a.p.tblwidth-e*g-p)),d+=k,f=b)}),q=0,m?c.width-p-(d+e*g)!==h&&(q=c.width-p-(d+e*g)-h):m||1===Math.abs(c.width-p-(d+e*g))||(q=c.width-p-(d+e*g)),a.p.colModel[f].width+=q,a.p.tblwidth=d+q+e*g+p,a.p.tblwidth>a.p.width&&(a.p.colModel[f].width-=a.p.tblwidth-parseInt(a.p.width,10),a.p.tblwidth=a.p.width))})();b(m).css("width",c.width+"px").append("<div class='ui-jqgrid-resize-mark' id='rs_m"+
	a.p.id+"'>&#160;</div>");b(k).css("width",c.width+"px");var D=b("thead:first",a).get(0),V="";a.p.footerrow&&(V+="<table role='grid' style='width:"+a.p.tblwidth+"px' class='ui-jqgrid-ftable' cellspacing='0' cellpadding='0' border='0'><tbody><tr role='row' class='ui-widget-content footrow footrow-"+f+"'>");var k=b("tr:first",D),da="<tr class='jqgfirstrow' role='row' style='height:auto'>";a.p.disableClick=!1;b("th",k).each(function(d){ja=a.p.colModel[d].width;void 0===a.p.colModel[d].resizable&&(a.p.colModel[d].resizable=
	!0);a.p.colModel[d].resizable?(fa=document.createElement("span"),b(fa).html("&#160;").addClass("ui-jqgrid-resize ui-jqgrid-resize-"+f).css("cursor","col-resize"),b(this).addClass(a.p.resizeclass)):fa="";b(this).css("width",ja+"px").prepend(fa);fa=null;var e="";a.p.colModel[d].hidden&&(b(this).css("display","none"),e="display:none;");da+="<td role='gridcell' style='height:0px;width:"+ja+"px;"+e+"'></td>";c.headers[d]={width:ja,el:this};ka=a.p.colModel[d].sortable;"boolean"!==typeof ka&&(ka=a.p.colModel[d].sortable=
	!0);e=a.p.colModel[d].name;"cb"!==e&&"subgrid"!==e&&"rn"!==e&&a.p.viewsortcols[2]&&b(">div",this).addClass("ui-jqgrid-sortable");ka&&(a.p.multiSort?a.p.viewsortcols[0]?(b("div span.s-ico",this).show(),a.p.colModel[d].lso&&b("div span.ui-icon-"+a.p.colModel[d].lso,this).removeClass("ui-state-disabled")):a.p.colModel[d].lso&&(b("div span.s-ico",this).show(),b("div span.ui-icon-"+a.p.colModel[d].lso,this).removeClass("ui-state-disabled")):a.p.viewsortcols[0]?(b("div span.s-ico",this).show(),d===a.p.lastsort&&
	b("div span.ui-icon-"+a.p.sortorder,this).removeClass("ui-state-disabled")):d===a.p.lastsort&&(b("div span.s-ico",this).show(),b("div span.ui-icon-"+a.p.sortorder,this).removeClass("ui-state-disabled")));a.p.footerrow&&(V+="<td role='gridcell' "+p(d,0,"",null,"",!1)+">&#160;</td>")}).mousedown(function(d){if(1===b(d.target).closest("th>span.ui-jqgrid-resize").length){var e=sa(this);if(!0===a.p.forceFit){var g=a.p,f=e,h;for(h=e+1;h<a.p.colModel.length;h++)if(!0!==a.p.colModel[h].hidden){f=h;break}g.nv=
	f-e}c.dragStart(e,d,xa(e));return!1}}).click(function(c){if(a.p.disableClick)return a.p.disableClick=!1;var d="th>div.ui-jqgrid-sortable",e,g;a.p.viewsortcols[2]||(d="th>div>span>span.ui-grid-ico-sort");c=b(c.target).closest(d);if(1===c.length){var f;if(a.p.frozenColumns){var h=b(this)[0].id.substring(a.p.id.length+1);b(a.p.colModel).each(function(a){if(this.name===h)return f=a,!1})}else f=sa(this);a.p.viewsortcols[2]||(e=!0,g=c.attr("sort"));null!=f&&ra(b("div",this)[0].id,f,e,g,this);return!1}});
	if(a.p.sortable&&b.fn.sortable)try{b(a).jqGrid("sortableColumns",k)}catch(Ba){}a.p.footerrow&&(V+="</tr></tbody></table>");da+="</tr>";k=document.createElement("tbody");this.appendChild(k);b(this).addClass("ui-jqgrid-btable").append(da);var da=null,k=b("<table class='ui-jqgrid-htable' style='width:"+a.p.tblwidth+"px' role='grid' aria-labelledby='gbox_"+this.id+"' cellspacing='0' cellpadding='0' border='0'></table>").append(D),J=a.p.caption&&!0===a.p.hiddengrid?!0:!1;g=b("<div class='ui-jqgrid-hbox"+
	("rtl"===f?"-rtl":"")+"'></div>");D=null;c.hDiv=document.createElement("div");b(c.hDiv).css({width:c.width+"px"}).addClass("ui-state-default ui-jqgrid-hdiv").append(g);b(g).append(k);k=null;J&&b(c.hDiv).hide();a.p.pager&&("string"===typeof a.p.pager?"#"!==a.p.pager.substr(0,1)&&(a.p.pager="#"+a.p.pager):a.p.pager="#"+b(a.p.pager).attr("id"),b(a.p.pager).css({width:c.width+"px"}).addClass("ui-state-default ui-jqgrid-pager ui-corner-bottom").appendTo(m),J&&b(a.p.pager).hide(),qa(a.p.pager,""));!1===
	a.p.cellEdit&&!0===a.p.hoverrows&&b(a).bind("mouseover",function(a){t=b(a.target).closest("tr.jqgrow");"ui-subgrid"!==b(t).attr("class")&&b(t).addClass("ui-state-hover")}).bind("mouseout",function(a){t=b(a.target).closest("tr.jqgrow");b(t).removeClass("ui-state-hover")});var B,N,ma;b(a).before(c.hDiv).click(function(c){y=c.target;t=b(y,a.rows).closest("tr.jqgrow");if(0===b(t).length||-1<t[0].className.indexOf("ui-state-disabled")||(b(y,a).closest("table.ui-jqgrid-btable").attr("id")||"").replace("_frozen",
	"")!==a.id)return this;var d=b(y).hasClass("cbox"),e=b(a).triggerHandler("jqGridBeforeSelectRow",[t[0].id,c]);(e=!1===e||"stop"===e?!1:!0)&&b.isFunction(a.p.beforeSelectRow)&&(e=a.p.beforeSelectRow.call(a,t[0].id,c));if("A"!==y.tagName&&("INPUT"!==y.tagName&&"TEXTAREA"!==y.tagName&&"OPTION"!==y.tagName&&"SELECT"!==y.tagName||d)&&!0===e)if(B=t[0].id,N=b.jgrid.getCellIndex(y),ma=b(y).closest("td,th").html(),b(a).triggerHandler("jqGridCellSelect",[B,N,ma,c]),b.isFunction(a.p.onCellSelect)&&a.p.onCellSelect.call(a,
	B,N,ma,c),!0===a.p.cellEdit)if(a.p.multiselect&&d)b(a).jqGrid("setSelection",B,!0,c);else{B=t[0].rowIndex;try{b(a).jqGrid("editCell",B,N,!0)}catch(g){}}else if(a.p.multikey)c[a.p.multikey]?b(a).jqGrid("setSelection",B,!0,c):a.p.multiselect&&d&&(d=b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+B).is(":checked"),b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+B)[a.p.useProp?"prop":"attr"]("checked",d));else{if(a.p.multiselect&&a.p.multiboxonly&&!d){var f=a.p.frozenColumns?a.p.id+"_frozen":"";b(a.p.selarrrow).each(function(c,
	d){var e=b(a).jqGrid("getGridRowById",d);b(e).removeClass("ui-state-highlight");b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(d))[a.p.useProp?"prop":"attr"]("checked",!1);f&&(b("#"+b.jgrid.jqID(d),"#"+b.jgrid.jqID(f)).removeClass("ui-state-highlight"),b("#jqg_"+b.jgrid.jqID(a.p.id)+"_"+b.jgrid.jqID(d),"#"+b.jgrid.jqID(f))[a.p.useProp?"prop":"attr"]("checked",!1))});a.p.selarrrow=[]}b(a).jqGrid("setSelection",B,!0,c)}}).bind("reloadGrid",function(c,d){!0===a.p.treeGrid&&(a.p.datatype=a.p.treedatatype);
	d&&d.current&&a.grid.selectionPreserver(a);"local"===a.p.datatype?(b(a).jqGrid("resetSelection"),a.p.data.length&&O()):a.p.treeGrid||(a.p.selrow=null,a.p.multiselect&&(a.p.selarrrow=[],ha(!1)),a.p.savedRow=[]);a.p.scroll&&X.call(a,!0,!1);if(d&&d.page){var e=d.page;e>a.p.lastpage&&(e=a.p.lastpage);1>e&&(e=1);a.p.page=e;a.grid.bDiv.scrollTop=a.grid.prevRowHeight?(e-1)*a.grid.prevRowHeight*a.p.rowNum:0}a.grid.prevRowHeight&&a.p.scroll?(delete a.p.lastpage,a.grid.populateVisible()):a.grid.populate();
	!0===a.p._inlinenav&&b(a).jqGrid("showAddEditButtons");return!1}).dblclick(function(c){y=c.target;t=b(y,a.rows).closest("tr.jqgrow");0!==b(t).length&&(B=t[0].rowIndex,N=b.jgrid.getCellIndex(y),b(a).triggerHandler("jqGridDblClickRow",[b(t).attr("id"),B,N,c]),b.isFunction(a.p.ondblClickRow)&&a.p.ondblClickRow.call(a,b(t).attr("id"),B,N,c))}).bind("contextmenu",function(c){y=c.target;t=b(y,a.rows).closest("tr.jqgrow");0!==b(t).length&&(a.p.multiselect||b(a).jqGrid("setSelection",t[0].id,!0,c),B=t[0].rowIndex,
	N=b.jgrid.getCellIndex(y),b(a).triggerHandler("jqGridRightClickRow",[b(t).attr("id"),B,N,c]),b.isFunction(a.p.onRightClickRow)&&a.p.onRightClickRow.call(a,b(t).attr("id"),B,N,c))});c.bDiv=document.createElement("div");n&&"auto"===String(a.p.height).toLowerCase()&&(a.p.height="100%");b(c.bDiv).append(b('<div style="position:relative;'+(n&&8>b.jgrid.msiever()?"height:0.01%;":"")+'"></div>').append("<div></div>").append(this)).addClass("ui-jqgrid-bdiv").css({height:a.p.height+(isNaN(a.p.height)?"":"px"),
	width:c.width+"px"}).scroll(c.scrollGrid);b("table:first",c.bDiv).css({width:a.p.tblwidth+"px"});b.support.tbody||2===b("tbody",this).length&&b("tbody:gt(0)",this).remove();a.p.multikey&&(b.jgrid.msie?b(c.bDiv).bind("selectstart",function(){return!1}):b(c.bDiv).bind("mousedown",function(){return!1}));J&&b(c.bDiv).hide();c.cDiv=document.createElement("div");var na=!0===a.p.hidegrid?b("<a role='link' class='ui-jqgrid-titlebar-close ui-corner-all HeaderButton' />").hover(function(){na.addClass("ui-state-hover")},
	function(){na.removeClass("ui-state-hover")}).append("<span class='ui-icon ui-icon-circle-triangle-n'></span>").css("rtl"===f?"left":"right","0px"):"";b(c.cDiv).append(na).append("<span class='ui-jqgrid-title'>"+a.p.caption+"</span>").addClass("ui-jqgrid-titlebar ui-jqgrid-caption"+("rtl"===f?"-rtl":"")+" ui-widget-header ui-corner-top ui-helper-clearfix");b(c.cDiv).insertBefore(c.hDiv);a.p.toolbar[0]&&(c.uDiv=document.createElement("div"),"top"===a.p.toolbar[1]?b(c.uDiv).insertBefore(c.hDiv):"bottom"===
	a.p.toolbar[1]&&b(c.uDiv).insertAfter(c.hDiv),"both"===a.p.toolbar[1]?(c.ubDiv=document.createElement("div"),b(c.uDiv).addClass("ui-userdata ui-state-default").attr("id","t_"+this.id).insertBefore(c.hDiv),b(c.ubDiv).addClass("ui-userdata ui-state-default").attr("id","tb_"+this.id).insertAfter(c.hDiv),J&&b(c.ubDiv).hide()):b(c.uDiv).width(c.width).addClass("ui-userdata ui-state-default").attr("id","t_"+this.id),J&&b(c.uDiv).hide());a.p.toppager&&(a.p.toppager=b.jgrid.jqID(a.p.id)+"_toppager",c.topDiv=
	b("<div id='"+a.p.toppager+"'></div>")[0],a.p.toppager="#"+a.p.toppager,b(c.topDiv).addClass("ui-state-default ui-jqgrid-toppager").width(c.width).insertBefore(c.hDiv),qa(a.p.toppager,"_t"));a.p.footerrow&&(c.sDiv=b("<div class='ui-jqgrid-sdiv'></div>")[0],g=b("<div class='ui-jqgrid-hbox"+("rtl"===f?"-rtl":"")+"'></div>"),b(c.sDiv).append(g).width(c.width).insertAfter(c.hDiv),b(g).append(V),c.footers=b(".ui-jqgrid-ftable",c.sDiv)[0].rows[0].cells,a.p.rownumbers&&(c.footers[0].className="ui-state-default jqgrid-rownum"),
	J&&b(c.sDiv).hide());g=null;if(a.p.caption){var ya=a.p.datatype;!0===a.p.hidegrid&&(b(".ui-jqgrid-titlebar-close",c.cDiv).click(function(d){var e=b.isFunction(a.p.onHeaderClick),g=".ui-jqgrid-bdiv, .ui-jqgrid-hdiv, .ui-jqgrid-pager, .ui-jqgrid-sdiv",f,h=this;!0===a.p.toolbar[0]&&("both"===a.p.toolbar[1]&&(g+=", #"+b(c.ubDiv).attr("id")),g+=", #"+b(c.uDiv).attr("id"));f=b(g,"#gview_"+b.jgrid.jqID(a.p.id)).length;"visible"===a.p.gridstate?b(g,"#gbox_"+b.jgrid.jqID(a.p.id)).slideUp("fast",function(){f--;
	0===f&&(b("span",h).removeClass("ui-icon-circle-triangle-n").addClass("ui-icon-circle-triangle-s"),a.p.gridstate="hidden",b("#gbox_"+b.jgrid.jqID(a.p.id)).hasClass("ui-resizable")&&b(".ui-resizable-handle","#gbox_"+b.jgrid.jqID(a.p.id)).hide(),b(a).triggerHandler("jqGridHeaderClick",[a.p.gridstate,d]),e&&(J||a.p.onHeaderClick.call(a,a.p.gridstate,d)))}):"hidden"===a.p.gridstate&&b(g,"#gbox_"+b.jgrid.jqID(a.p.id)).slideDown("fast",function(){f--;0===f&&(b("span",h).removeClass("ui-icon-circle-triangle-s").addClass("ui-icon-circle-triangle-n"),
	J&&(a.p.datatype=ya,Q(),J=!1),a.p.gridstate="visible",b("#gbox_"+b.jgrid.jqID(a.p.id)).hasClass("ui-resizable")&&b(".ui-resizable-handle","#gbox_"+b.jgrid.jqID(a.p.id)).show(),b(a).triggerHandler("jqGridHeaderClick",[a.p.gridstate,d]),e&&(J||a.p.onHeaderClick.call(a,a.p.gridstate,d)))});return!1}),J&&(a.p.datatype="local",b(".ui-jqgrid-titlebar-close",c.cDiv).trigger("click")))}else b(c.cDiv).hide();b(c.hDiv).after(c.bDiv).mousemove(function(a){if(c.resizing)return c.dragMove(a),!1});b(".ui-jqgrid-labels",
	c.hDiv).bind("selectstart",function(){return!1});b(document).bind("mouseup.jqGrid"+a.p.id,function(){return c.resizing?(c.dragEnd(),!1):!0});a.formatCol=p;a.sortData=ra;a.updatepager=function(c,d){var e,g,f,h,k,l,m,n="",p=a.p.pager?"_"+b.jgrid.jqID(a.p.pager.substr(1)):"",q=a.p.toppager?"_"+a.p.toppager.substr(1):"";f=parseInt(a.p.page,10)-1;0>f&&(f=0);f*=parseInt(a.p.rowNum,10);k=f+a.p.reccount;if(a.p.scroll){e=b("tbody:first > tr:gt(0)",a.grid.bDiv);f=k-e.length;a.p.reccount=e.length;if(e=e.outerHeight()||
	a.grid.prevRowHeight)g=f*e,m=parseInt(a.p.records,10)*e,b(">div:first",a.grid.bDiv).css({height:m}).children("div:first").css({height:g,display:g?"":"none"}),0==a.grid.bDiv.scrollTop&&1<a.p.page&&(a.grid.bDiv.scrollTop=a.p.rowNum*(a.p.page-1)*e);a.grid.bDiv.scrollLeft=a.grid.hDiv.scrollLeft}n=a.p.pager||"";if(n+=a.p.toppager?n?","+a.p.toppager:a.p.toppager:"")m=b.jgrid.formatter.integer||{},e=r(a.p.page),g=r(a.p.lastpage),b(".selbox",n)[this.p.useProp?"prop":"attr"]("disabled",!1),!0===a.p.pginput&&
	(b(".ui-pg-input",n).val(a.p.page),h=a.p.toppager?"#sp_1"+p+",#sp_1"+q:"#sp_1"+p,b(h).html(b.fmatter?b.fmatter.util.NumberFormat(a.p.lastpage,m):a.p.lastpage)),a.p.viewrecords&&(0===a.p.reccount?b(".ui-paging-info",n).html(a.p.emptyrecords):(h=f+1,l=a.p.records,b.fmatter&&(h=b.fmatter.util.NumberFormat(h,m),k=b.fmatter.util.NumberFormat(k,m),l=b.fmatter.util.NumberFormat(l,m)),b(".ui-paging-info",n).html(b.jgrid.format(a.p.recordtext,h,k,l)))),!0===a.p.pgbuttons&&(0>=e&&(e=g=0),1===e||0===e?(b("#first"+
	p+", #prev"+p).addClass("ui-state-disabled").removeClass("ui-state-hover"),a.p.toppager&&b("#first_t"+q+", #prev_t"+q).addClass("ui-state-disabled").removeClass("ui-state-hover")):(b("#first"+p+", #prev"+p).removeClass("ui-state-disabled"),a.p.toppager&&b("#first_t"+q+", #prev_t"+q).removeClass("ui-state-disabled")),e===g||0===e?(b("#next"+p+", #last"+p).addClass("ui-state-disabled").removeClass("ui-state-hover"),a.p.toppager&&b("#next_t"+q+", #last_t"+q).addClass("ui-state-disabled").removeClass("ui-state-hover")):
	(b("#next"+p+", #last"+p).removeClass("ui-state-disabled"),a.p.toppager&&b("#next_t"+q+", #last_t"+q).removeClass("ui-state-disabled")));!0===c&&!0===a.p.rownumbers&&b(">td.jqgrid-rownum",a.rows).each(function(a){b(this).html(f+1+a)});d&&a.p.jqgdnd&&b(a).jqGrid("gridDnD","updateDnD");b(a).triggerHandler("jqGridGridComplete");b.isFunction(a.p.gridComplete)&&a.p.gridComplete.call(a);b(a).triggerHandler("jqGridAfterGridComplete")};a.refreshIndex=O;a.setHeadCheckBox=ha;a.constructTr=$;a.formatter=function(a,
	b,c,d,e){return x(a,b,c,d,e)};b.extend(c,{populate:Q,emptyRows:X,beginReq:P,endReq:T});this.grid=c;a.addXmlData=function(b){K(b,a.grid.bDiv)};a.addJSONData=function(b){Y(b,a.grid.bDiv)};this.grid.cols=this.rows[0].cells;b(a).triggerHandler("jqGridInitGrid");b.isFunction(a.p.onInitGrid)&&a.p.onInitGrid.call(a);Q();a.p.hiddengrid=!1}}}})};b.jgrid.extend({getGridParam:function(b){var f=this[0];if(f&&f.grid)return b?void 0!==f.p[b]?f.p[b]:null:f.p},setGridParam:function(e){return this.each(function(){this.grid&&
	"object"===typeof e&&b.extend(!0,this.p,e)})},getGridRowById:function(e){var f;this.each(function(){try{for(var c=this.rows.length;c--;)if(e.toString()===this.rows[c].id){f=this.rows[c];break}}catch(d){f=b(this.grid.bDiv).find("#"+b.jgrid.jqID(e))}});return f},getDataIDs:function(){var e=[],f=0,c,d=0;this.each(function(){if((c=this.rows.length)&&0<c)for(;f<c;)b(this.rows[f]).hasClass("jqgrow")&&(e[d]=this.rows[f].id,d++),f++});return e},setSelection:function(e,f,c){return this.each(function(){var d,
	a,l,g,h,k;void 0!==e&&(f=!1===f?!1:!0,!(a=b(this).jqGrid("getGridRowById",e))||!a.className||-1<a.className.indexOf("ui-state-disabled")||(!0===this.p.scrollrows&&(l=b(this).jqGrid("getGridRowById",e).rowIndex,0<=l&&(d=b(this.grid.bDiv)[0].clientHeight,g=b(this.grid.bDiv)[0].scrollTop,h=b(this.rows[l]).position().top,l=this.rows[l].clientHeight,h+l>=d+g?b(this.grid.bDiv)[0].scrollTop=h-(d+g)+l+g:h<d+g&&h<g&&(b(this.grid.bDiv)[0].scrollTop=h))),!0===this.p.frozenColumns&&(k=this.p.id+"_frozen"),this.p.multiselect?
	(this.setHeadCheckBox(!1),this.p.selrow=a.id,g=b.inArray(this.p.selrow,this.p.selarrrow),-1===g?("ui-subgrid"!==a.className&&b(a).addClass("ui-state-highlight").attr("aria-selected","true"),d=!0,this.p.selarrrow.push(this.p.selrow)):("ui-subgrid"!==a.className&&b(a).removeClass("ui-state-highlight").attr("aria-selected","false"),d=!1,this.p.selarrrow.splice(g,1),h=this.p.selarrrow[0],this.p.selrow=void 0===h?null:h),b("#jqg_"+b.jgrid.jqID(this.p.id)+"_"+b.jgrid.jqID(a.id))[this.p.useProp?"prop":"attr"]("checked",
	d),k&&(-1===g?b("#"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k)).addClass("ui-state-highlight"):b("#"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k)).removeClass("ui-state-highlight"),b("#jqg_"+b.jgrid.jqID(this.p.id)+"_"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k))[this.p.useProp?"prop":"attr"]("checked",d)),f&&(b(this).triggerHandler("jqGridSelectRow",[a.id,d,c]),this.p.onSelectRow&&this.p.onSelectRow.call(this,a.id,d,c))):"ui-subgrid"!==a.className&&(this.p.selrow!==a.id?(b(b(this).jqGrid("getGridRowById",this.p.selrow)).removeClass("ui-state-highlight").attr({"aria-selected":"false",
	tabindex:"-1"}),b(a).addClass("ui-state-highlight").attr({"aria-selected":"true",tabindex:"0"}),k&&(b("#"+b.jgrid.jqID(this.p.selrow),"#"+b.jgrid.jqID(k)).removeClass("ui-state-highlight"),b("#"+b.jgrid.jqID(e),"#"+b.jgrid.jqID(k)).addClass("ui-state-highlight")),d=!0):d=!1,this.p.selrow=a.id,f&&(b(this).triggerHandler("jqGridSelectRow",[a.id,d,c]),this.p.onSelectRow&&this.p.onSelectRow.call(this,a.id,d,c)))))})},resetSelection:function(e){return this.each(function(){var f=this,c,d;!0===f.p.frozenColumns&&
	(d=f.p.id+"_frozen");if(void 0!==e){c=e===f.p.selrow?f.p.selrow:e;b("#"+b.jgrid.jqID(f.p.id)+" tbody:first tr#"+b.jgrid.jqID(c)).removeClass("ui-state-highlight").attr("aria-selected","false");d&&b("#"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d)).removeClass("ui-state-highlight");if(f.p.multiselect){b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(f.p.id))[f.p.useProp?"prop":"attr"]("checked",!1);if(d)b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d))[f.p.useProp?"prop":
	"attr"]("checked",!1);f.setHeadCheckBox(!1)}c=null}else f.p.multiselect?(b(f.p.selarrrow).each(function(a,c){b(b(f).jqGrid("getGridRowById",c)).removeClass("ui-state-highlight").attr("aria-selected","false");b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c))[f.p.useProp?"prop":"attr"]("checked",!1);d&&(b("#"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d)).removeClass("ui-state-highlight"),b("#jqg_"+b.jgrid.jqID(f.p.id)+"_"+b.jgrid.jqID(c),"#"+b.jgrid.jqID(d))[f.p.useProp?"prop":"attr"]("checked",!1))}),f.setHeadCheckBox(!1),
	f.p.selarrrow=[],f.p.selrow=null):f.p.selrow&&(b("#"+b.jgrid.jqID(f.p.id)+" tbody:first tr#"+b.jgrid.jqID(f.p.selrow)).removeClass("ui-state-highlight").attr("aria-selected","false"),d&&b("#"+b.jgrid.jqID(f.p.selrow),"#"+b.jgrid.jqID(d)).removeClass("ui-state-highlight"),f.p.selrow=null);!0===f.p.cellEdit&&0<=parseInt(f.p.iCol,10)&&0<=parseInt(f.p.iRow,10)&&(b("td:eq("+f.p.iCol+")",f.rows[f.p.iRow]).removeClass("edit-cell ui-state-highlight"),b(f.rows[f.p.iRow]).removeClass("selected-row ui-state-hover"));
	f.p.savedRow=[]})},getRowData:function(e){var f={},c,d=!1,a,l=0;this.each(function(){var g=this,h,k;if(void 0===e)d=!0,c=[],a=g.rows.length;else{k=b(g).jqGrid("getGridRowById",e);if(!k)return f;a=2}for(;l<a;)d&&(k=g.rows[l]),b(k).hasClass("jqgrow")&&(b('td[role="gridcell"]',k).each(function(a){h=g.p.colModel[a].name;if("cb"!==h&&"subgrid"!==h&&"rn"!==h)if(!0===g.p.treeGrid&&h===g.p.ExpandColumn)f[h]=b.jgrid.htmlDecode(b("span:first",this).html());else try{f[h]=b.unformat.call(g,this,{rowId:k.id,colModel:g.p.colModel[a]},
	a)}catch(c){f[h]=b.jgrid.htmlDecode(b(this).html())}}),d&&(c.push(f),f={})),l++});return c||f},delRowData:function(e){var f=!1,c,d;this.each(function(){c=b(this).jqGrid("getGridRowById",e);if(!c)return!1;b(c).remove();this.p.records--;this.p.reccount--;this.updatepager(!0,!1);f=!0;this.p.multiselect&&(d=b.inArray(e,this.p.selarrrow),-1!==d&&this.p.selarrrow.splice(d,1));this.p.selrow=this.p.multiselect&&0<this.p.selarrrow.length?this.p.selarrrow[this.p.selarrrow.length-1]:null;if("local"===this.p.datatype){var a=
	b.jgrid.stripPref(this.p.idPrefix,e),a=this.p._index[a];void 0!==a&&(this.p.data.splice(a,1),this.refreshIndex())}if(!0===this.p.altRows&&f){var l=this.p.altclass;b(this.rows).each(function(a){1===a%2?b(this).addClass(l):b(this).removeClass(l)})}});return f},setRowData:function(e,f,c){var d,a=!0,l;this.each(function(){if(!this.grid)return!1;var g=this,h,k,n=typeof c,m={};k=b(this).jqGrid("getGridRowById",e);if(!k)return!1;if(f)try{if(b(this.p.colModel).each(function(a){d=this.name;var c=b.jgrid.getAccessor(f,
	d);void 0!==c&&(m[d]=this.formatter&&"string"===typeof this.formatter&&"date"===this.formatter?b.unformat.date.call(g,c,this):c,h=g.formatter(e,c,a,f,"edit"),l=this.title?{title:b.jgrid.stripHtml(h)}:{},!0===g.p.treeGrid&&d===g.p.ExpandColumn?b("td[role='gridcell']:eq("+a+") > span:first",k).html(h).attr(l):b("td[role='gridcell']:eq("+a+")",k).html(h).attr(l))}),"local"===g.p.datatype){var r=b.jgrid.stripPref(g.p.idPrefix,e),p=g.p._index[r],q;if(g.p.treeGrid)for(q in g.p.treeReader)g.p.treeReader.hasOwnProperty(q)&&
	delete m[g.p.treeReader[q]];void 0!==p&&(g.p.data[p]=b.extend(!0,g.p.data[p],m));m=null}}catch(x){a=!1}a&&("string"===n?b(k).addClass(c):null!==c&&"object"===n&&b(k).css(c),b(g).triggerHandler("jqGridAfterGridComplete"))});return a},addRowData:function(e,f,c,d){c||(c="last");var a=!1,l,g,h,k,n,m,r,p,q="",x,G,U,M,ea,W;f&&(b.isArray(f)?(x=!0,c="last",G=e):(f=[f],x=!1),this.each(function(){var X=f.length;n=!0===this.p.rownumbers?1:0;h=!0===this.p.multiselect?1:0;k=!0===this.p.subGrid?1:0;x||(void 0!==
	e?e=String(e):(e=b.jgrid.randId(),!1!==this.p.keyIndex&&(G=this.p.colModel[this.p.keyIndex+h+k+n].name,void 0!==f[0][G]&&(e=f[0][G]))));U=this.p.altclass;for(var O=0,$="",K={},Y=b.isFunction(this.p.afterInsertRow)?!0:!1;O<X;){M=f[O];g=[];if(x){try{e=M[G],void 0===e&&(e=b.jgrid.randId())}catch(oa){e=b.jgrid.randId()}$=!0===this.p.altRows?0===(this.rows.length-1)%2?U:"":""}W=e;e=this.p.idPrefix+e;n&&(q=this.formatCol(0,1,"",null,e,!0),g[g.length]='<td role="gridcell" class="ui-state-default jqgrid-rownum" '+
	q+">0</td>");h&&(p='<input role="checkbox" type="checkbox" id="jqg_'+this.p.id+"_"+e+'" class="cbox"/>',q=this.formatCol(n,1,"",null,e,!0),g[g.length]='<td role="gridcell" '+q+">"+p+"</td>");k&&(g[g.length]=b(this).jqGrid("addSubGridCell",h+n,1));for(r=h+k+n;r<this.p.colModel.length;r++)ea=this.p.colModel[r],l=ea.name,K[l]=M[l],p=this.formatter(e,b.jgrid.getAccessor(M,l),r,M),q=this.formatCol(r,1,p,M,e,K),g[g.length]='<td role="gridcell" '+q+">"+p+"</td>";g.unshift(this.constructTr(e,!1,$,K,M,!1));
	g[g.length]="</tr>";if(0===this.rows.length)b("table:first",this.grid.bDiv).append(g.join(""));else switch(c){case "last":b(this.rows[this.rows.length-1]).after(g.join(""));m=this.rows.length-1;break;case "first":b(this.rows[0]).after(g.join(""));m=1;break;case "after":if(m=b(this).jqGrid("getGridRowById",d))b(this.rows[m.rowIndex+1]).hasClass("ui-subgrid")?b(this.rows[m.rowIndex+1]).after(g):b(m).after(g.join("")),m=m.rowIndex+1;break;case "before":if(m=b(this).jqGrid("getGridRowById",d))b(m).before(g.join("")),
	m=m.rowIndex-1}!0===this.p.subGrid&&b(this).jqGrid("addSubGrid",h+n,m);this.p.records++;this.p.reccount++;b(this).triggerHandler("jqGridAfterInsertRow",[e,M,M]);Y&&this.p.afterInsertRow.call(this,e,M,M);O++;"local"===this.p.datatype&&(K[this.p.localReader.id]=W,this.p._index[W]=this.p.data.length,this.p.data.push(K),K={})}!0!==this.p.altRows||x||("last"===c?1===(this.rows.length-1)%2&&b(this.rows[this.rows.length-1]).addClass(U):b(this.rows).each(function(a){1===a%2?b(this).addClass(U):b(this).removeClass(U)}));
	this.updatepager(!0,!0);a=!0}));return a},footerData:function(e,f,c){function d(a){for(var b in a)if(a.hasOwnProperty(b))return!1;return!0}var a,l=!1,g={},h;void 0==e&&(e="get");"boolean"!==typeof c&&(c=!0);e=e.toLowerCase();this.each(function(){var k=this,n;if(!k.grid||!k.p.footerrow||"set"===e&&d(f))return!1;l=!0;b(this.p.colModel).each(function(d){a=this.name;"set"===e?void 0!==f[a]&&(n=c?k.formatter("",f[a],d,f,"edit"):f[a],h=this.title?{title:b.jgrid.stripHtml(n)}:{},b("tr.footrow td:eq("+d+
	")",k.grid.sDiv).html(n).attr(h),l=!0):"get"===e&&(g[a]=b("tr.footrow td:eq("+d+")",k.grid.sDiv).html())})});return"get"===e?g:l},showHideCol:function(e,f){return this.each(function(){var c=this,d=!1,a=b.jgrid.cell_width?0:c.p.cellLayout,l;if(c.grid){"string"===typeof e&&(e=[e]);f="none"!==f?"":"none";var g=""===f?!0:!1,h=c.p.groupHeader&&("object"===typeof c.p.groupHeader||b.isFunction(c.p.groupHeader));h&&b(c).jqGrid("destroyGroupHeader",!1);b(this.p.colModel).each(function(h){if(-1!==b.inArray(this.name,
	e)&&this.hidden===g){if(!0===c.p.frozenColumns&&!0===this.frozen)return!0;b("tr[role=rowheader]",c.grid.hDiv).each(function(){b(this.cells[h]).css("display",f)});b(c.rows).each(function(){b(this).hasClass("jqgroup")||b(this.cells[h]).css("display",f)});c.p.footerrow&&b("tr.footrow td:eq("+h+")",c.grid.sDiv).css("display",f);l=parseInt(this.width,10);c.p.tblwidth="none"===f?c.p.tblwidth-(l+a):c.p.tblwidth+(l+a);this.hidden=!g;d=!0;b(c).triggerHandler("jqGridShowHideCol",[g,this.name,h])}});!0===d&&
	(!0!==c.p.shrinkToFit||isNaN(c.p.height)||(c.p.tblwidth+=parseInt(c.p.scrollOffset,10)),b(c).jqGrid("setGridWidth",!0===c.p.shrinkToFit?c.p.tblwidth:c.p.width));h&&b(c).jqGrid("setGroupHeaders",c.p.groupHeader)}})},hideCol:function(e){return this.each(function(){b(this).jqGrid("showHideCol",e,"none")})},showCol:function(e){return this.each(function(){b(this).jqGrid("showHideCol",e,"")})},remapColumns:function(e,f,c){function d(a){var c;c=a.length?b.makeArray(a):b.extend({},a);b.each(e,function(b){a[b]=
	c[this]})}function a(a,c){b(">tr"+(c||""),a).each(function(){var a=this,c=b.makeArray(a.cells);b.each(e,function(){var b=c[this];b&&a.appendChild(b)})})}var l=this.get(0);d(l.p.colModel);d(l.p.colNames);d(l.grid.headers);a(b("thead:first",l.grid.hDiv),c&&":not(.ui-jqgrid-labels)");f&&a(b("#"+b.jgrid.jqID(l.p.id)+" tbody:first"),".jqgfirstrow, tr.jqgrow, tr.jqfoot");l.p.footerrow&&a(b("tbody:first",l.grid.sDiv));l.p.remapColumns&&(l.p.remapColumns.length?d(l.p.remapColumns):l.p.remapColumns=b.makeArray(e));
	l.p.lastsort=b.inArray(l.p.lastsort,e);l.p.treeGrid&&(l.p.expColInd=b.inArray(l.p.expColInd,e));b(l).triggerHandler("jqGridRemapColumns",[e,f,c])},setGridWidth:function(e,f){return this.each(function(){if(this.grid){var c=this,d,a=0,l=b.jgrid.cell_width?0:c.p.cellLayout,g,h=0,k=!1,n=c.p.scrollOffset,m,r=0,p;"boolean"!==typeof f&&(f=c.p.shrinkToFit);if(!isNaN(e)){e=parseInt(e,10);c.grid.width=c.p.width=e;b("#gbox_"+b.jgrid.jqID(c.p.id)).css("width",e+"px");b("#gview_"+b.jgrid.jqID(c.p.id)).css("width",
	e+"px");b(c.grid.bDiv).css("width",e+"px");b(c.grid.hDiv).css("width",e+"px");c.p.pager&&b(c.p.pager).css("width",e+"px");c.p.toppager&&b(c.p.toppager).css("width",e+"px");!0===c.p.toolbar[0]&&(b(c.grid.uDiv).css("width",e+"px"),"both"===c.p.toolbar[1]&&b(c.grid.ubDiv).css("width",e+"px"));c.p.footerrow&&b(c.grid.sDiv).css("width",e+"px");!1===f&&!0===c.p.forceFit&&(c.p.forceFit=!1);if(!0===f){b.each(c.p.colModel,function(){!1===this.hidden&&(d=this.widthOrg,a+=d+l,this.fixed?r+=d+l:h++)});if(0===
	h)return;c.p.tblwidth=a;m=e-l*h-r;!isNaN(c.p.height)&&(b(c.grid.bDiv)[0].clientHeight<b(c.grid.bDiv)[0].scrollHeight||1===c.rows.length)&&(k=!0,m-=n);var a=0,q=0<c.grid.cols.length;b.each(c.p.colModel,function(b){!1!==this.hidden||this.fixed||(d=this.widthOrg,d=Math.round(m*d/(c.p.tblwidth-l*h-r)),0>d||(this.width=d,a+=d,c.grid.headers[b].width=d,c.grid.headers[b].el.style.width=d+"px",c.p.footerrow&&(c.grid.footers[b].style.width=d+"px"),q&&(c.grid.cols[b].style.width=d+"px"),g=b))});if(!g)return;
	p=0;k?e-r-(a+l*h)!==n&&(p=e-r-(a+l*h)-n):1!==Math.abs(e-r-(a+l*h))&&(p=e-r-(a+l*h));c.p.colModel[g].width+=p;c.p.tblwidth=a+p+l*h+r;c.p.tblwidth>e?(k=c.p.tblwidth-parseInt(e,10),c.p.tblwidth=e,d=c.p.colModel[g].width-=k):d=c.p.colModel[g].width;c.grid.headers[g].width=d;c.grid.headers[g].el.style.width=d+"px";q&&(c.grid.cols[g].style.width=d+"px");c.p.footerrow&&(c.grid.footers[g].style.width=d+"px")}c.p.tblwidth&&(b("table:first",c.grid.bDiv).css("width",c.p.tblwidth+"px"),b("table:first",c.grid.hDiv).css("width",
	c.p.tblwidth+"px"),c.grid.hDiv.scrollLeft=c.grid.bDiv.scrollLeft,c.p.footerrow&&b("table:first",c.grid.sDiv).css("width",c.p.tblwidth+"px"))}}})},setGridHeight:function(e){return this.each(function(){if(this.grid){var f=b(this.grid.bDiv);f.css({height:e+(isNaN(e)?"":"px")});!0===this.p.frozenColumns&&b("#"+b.jgrid.jqID(this.p.id)+"_frozen").parent().height(f.height()-16);this.p.height=e;this.p.scroll&&this.grid.populateVisible()}})},setCaption:function(e){return this.each(function(){this.p.caption=
	e;b("span.ui-jqgrid-title, span.ui-jqgrid-title-rtl",this.grid.cDiv).html(e);b(this.grid.cDiv).show()})},setLabel:function(e,f,c,d){return this.each(function(){var a=-1;if(this.grid&&void 0!==e&&(b(this.p.colModel).each(function(b){if(this.name===e)return a=b,!1}),0<=a)){var l=b("tr.ui-jqgrid-labels th:eq("+a+")",this.grid.hDiv);if(f){var g=b(".s-ico",l);b("[id^=jqgh_]",l).empty().html(f).append(g);this.p.colNames[a]=f}c&&("string"===typeof c?b(l).addClass(c):b(l).css(c));"object"===typeof d&&b(l).attr(d)}})},
	setCell:function(e,f,c,d,a,l){return this.each(function(){var g=-1,h,k;if(this.grid&&(isNaN(f)?b(this.p.colModel).each(function(a){if(this.name===f)return g=a,!1}):g=parseInt(f,10),0<=g&&(h=b(this).jqGrid("getGridRowById",e)))){var n=b("td:eq("+g+")",h);if(""!==c||!0===l)h=this.formatter(e,c,g,h,"edit"),k=this.p.colModel[g].title?{title:b.jgrid.stripHtml(h)}:{},this.p.treeGrid&&0<b(".tree-wrap",b(n)).length?b("span",b(n)).html(h).attr(k):b(n).html(h).attr(k),"local"===this.p.datatype&&(h=this.p.colModel[g],
	c=h.formatter&&"string"===typeof h.formatter&&"date"===h.formatter?b.unformat.date.call(this,c,h):c,k=this.p._index[b.jgrid.stripPref(this.p.idPrefix,e)],void 0!==k&&(this.p.data[k][h.name]=c));"string"===typeof d?b(n).addClass(d):d&&b(n).css(d);"object"===typeof a&&b(n).attr(a)}})},getCell:function(e,f){var c=!1;this.each(function(){var d=-1;if(this.grid&&(isNaN(f)?b(this.p.colModel).each(function(a){if(this.name===f)return d=a,!1}):d=parseInt(f,10),0<=d)){var a=b(this).jqGrid("getGridRowById",e);
	if(a)try{c=b.unformat.call(this,b("td:eq("+d+")",a),{rowId:a.id,colModel:this.p.colModel[d]},d)}catch(l){c=b.jgrid.htmlDecode(b("td:eq("+d+")",a).html())}}});return c},getCol:function(e,f,c){var d=[],a,l=0,g,h,k;f="boolean"!==typeof f?!1:f;void 0===c&&(c=!1);this.each(function(){var n=-1;if(this.grid&&(isNaN(e)?b(this.p.colModel).each(function(a){if(this.name===e)return n=a,!1}):n=parseInt(e,10),0<=n)){var m=this.rows.length,r=0,p=0;if(m&&0<m){for(;r<m;){if(b(this.rows[r]).hasClass("jqgrow")){try{a=
	b.unformat.call(this,b(this.rows[r].cells[n]),{rowId:this.rows[r].id,colModel:this.p.colModel[n]},n)}catch(q){a=b.jgrid.htmlDecode(this.rows[r].cells[n].innerHTML)}c?(k=parseFloat(a),isNaN(k)||(l+=k,void 0===h&&(h=g=k),g=Math.min(g,k),h=Math.max(h,k),p++)):f?d.push({id:this.rows[r].id,value:a}):d.push(a)}r++}if(c)switch(c.toLowerCase()){case "sum":d=l;break;case "avg":d=l/p;break;case "count":d=m-1;break;case "min":d=g;break;case "max":d=h}}}});return d},clearGridData:function(e){return this.each(function(){if(this.grid){"boolean"!==
	typeof e&&(e=!1);if(this.p.deepempty)b("#"+b.jgrid.jqID(this.p.id)+" tbody:first tr:gt(0)").remove();else{var f=b("#"+b.jgrid.jqID(this.p.id)+" tbody:first tr:first")[0];b("#"+b.jgrid.jqID(this.p.id)+" tbody:first").empty().append(f)}this.p.footerrow&&e&&b(".ui-jqgrid-ftable td",this.grid.sDiv).html("&#160;");this.p.selrow=null;this.p.selarrrow=[];this.p.savedRow=[];this.p.records=0;this.p.page=1;this.p.lastpage=0;this.p.reccount=0;this.p.data=[];this.p._index={};this.updatepager(!0,!1)}})},getInd:function(e,
	f){var c=!1,d;this.each(function(){(d=b(this).jqGrid("getGridRowById",e))&&(c=!0===f?d:d.rowIndex)});return c},bindKeys:function(e){var f=b.extend({onEnter:null,onSpace:null,onLeftKey:null,onRightKey:null,scrollingRows:!0},e||{});return this.each(function(){var c=this;b("body").is("[role]")||b("body").attr("role","application");c.p.scrollrows=f.scrollingRows;b(c).keydown(function(d){var a=b(c).find("tr[tabindex=0]")[0],e,g,h,k=c.p.treeReader.expanded_field;if(a)if(h=c.p._index[b.jgrid.stripPref(c.p.idPrefix,
	a.id)],37===d.keyCode||38===d.keyCode||39===d.keyCode||40===d.keyCode){if(38===d.keyCode){g=a.previousSibling;e="";if(g)if(b(g).is(":hidden"))for(;g;){if(g=g.previousSibling,!b(g).is(":hidden")&&b(g).hasClass("jqgrow")){e=g.id;break}}else e=g.id;b(c).jqGrid("setSelection",e,!0,d);d.preventDefault()}if(40===d.keyCode){g=a.nextSibling;e="";if(g)if(b(g).is(":hidden"))for(;g;){if(g=g.nextSibling,!b(g).is(":hidden")&&b(g).hasClass("jqgrow")){e=g.id;break}}else e=g.id;b(c).jqGrid("setSelection",e,!0,d);
	d.preventDefault()}37===d.keyCode&&(c.p.treeGrid&&c.p.data[h][k]&&b(a).find("div.treeclick").trigger("click"),b(c).triggerHandler("jqGridKeyLeft",[c.p.selrow]),b.isFunction(f.onLeftKey)&&f.onLeftKey.call(c,c.p.selrow));39===d.keyCode&&(c.p.treeGrid&&!c.p.data[h][k]&&b(a).find("div.treeclick").trigger("click"),b(c).triggerHandler("jqGridKeyRight",[c.p.selrow]),b.isFunction(f.onRightKey)&&f.onRightKey.call(c,c.p.selrow))}else 13===d.keyCode?(b(c).triggerHandler("jqGridKeyEnter",[c.p.selrow]),b.isFunction(f.onEnter)&&
	f.onEnter.call(c,c.p.selrow)):32===d.keyCode&&(b(c).triggerHandler("jqGridKeySpace",[c.p.selrow]),b.isFunction(f.onSpace)&&f.onSpace.call(c,c.p.selrow))})})},unbindKeys:function(){return this.each(function(){b(this).unbind("keydown")})},getLocalRow:function(e){var f=!1,c;this.each(function(){void 0!==e&&(c=this.p._index[b.jgrid.stripPref(this.p.idPrefix,e)],0<=c&&(f=this.p.data[c]))});return f}})})(jQuery);
	(function(a){a.fmatter={};a.extend(a.fmatter,{isBoolean:function(a){return"boolean"===typeof a},isObject:function(c){return c&&("object"===typeof c||a.isFunction(c))||!1},isString:function(a){return"string"===typeof a},isNumber:function(a){return"number"===typeof a&&isFinite(a)},isValue:function(a){return this.isObject(a)||this.isString(a)||this.isNumber(a)||this.isBoolean(a)},isEmpty:function(c){if(!this.isString(c)&&this.isValue(c))return!1;if(!this.isValue(c))return!0;c=a.trim(c).replace(/\&nbsp\;/ig,
	"").replace(/\&#160\;/ig,"");return""===c}});a.fn.fmatter=function(c,b,d,e,f){var g=b;d=a.extend({},a.jgrid.formatter,d);try{g=a.fn.fmatter[c].call(this,b,d,e,f)}catch(h){}return g};a.fmatter.util={NumberFormat:function(c,b){a.fmatter.isNumber(c)||(c*=1);if(a.fmatter.isNumber(c)){var d=0>c,e=String(c),f=b.decimalSeparator||".",g;if(a.fmatter.isNumber(b.decimalPlaces)){var h=b.decimalPlaces,e=Math.pow(10,h),e=String(Math.round(c*e)/e);g=e.lastIndexOf(".");if(0<h)for(0>g?(e+=f,g=e.length-1):"."!==f&&
	(e=e.replace(".",f));e.length-1-g<h;)e+="0"}if(b.thousandsSeparator){h=b.thousandsSeparator;g=e.lastIndexOf(f);g=-1<g?g:e.length;var f=e.substring(g),l=-1,k;for(k=g;0<k;k--)l++,0===l%3&&k!==g&&(!d||1<k)&&(f=h+f),f=e.charAt(k-1)+f;e=f}e=b.prefix?b.prefix+e:e;return e=b.suffix?e+b.suffix:e}return c}};a.fn.fmatter.defaultFormat=function(c,b){return a.fmatter.isValue(c)&&""!==c?c:b.defaultValue||"&#160;"};a.fn.fmatter.email=function(c,b){return a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):'<a href="mailto:'+
	c+'">'+c+"</a>"};a.fn.fmatter.checkbox=function(c,b){var d=a.extend({},b.checkbox),e;void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));e=!0===d.disabled?'disabled="disabled"':"";if(a.fmatter.isEmpty(c)||void 0===c)c=a.fn.fmatter.defaultFormat(c,d);c=String(c);c=(c+"").toLowerCase();return'<input type="checkbox" '+(0>c.search(/(false|f|0|no|n|off|undefined)/i)?" checked='checked' ":"")+' value="'+c+'" offval="no" '+e+"/>"};a.fn.fmatter.link=function(c,
	b){var d={target:b.target},e="";void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));d.target&&(e="target="+d.target);return a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):"<a "+e+' href="'+c+'">'+c+"</a>"};a.fn.fmatter.showlink=function(c,b){var d={baseLinkUrl:b.baseLinkUrl,showAction:b.showAction,addParam:b.addParam||"",target:b.target,idName:b.idName},e="";void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));
	d.target&&(e="target="+d.target);d=d.baseLinkUrl+d.showAction+"?"+d.idName+"="+b.rowId+d.addParam;return a.fmatter.isString(c)||a.fmatter.isNumber(c)?"<a "+e+' href="'+d+'">'+c+"</a>":a.fn.fmatter.defaultFormat(c,b)};a.fn.fmatter.integer=function(c,b){var d=a.extend({},b.integer);void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return a.fmatter.isEmpty(c)?d.defaultValue:a.fmatter.util.NumberFormat(c,d)};a.fn.fmatter.number=function(c,b){var d=a.extend({},
	b.number);void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return a.fmatter.isEmpty(c)?d.defaultValue:a.fmatter.util.NumberFormat(c,d)};a.fn.fmatter.currency=function(c,b){var d=a.extend({},b.currency);void 0!==b.colModel&&void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return a.fmatter.isEmpty(c)?d.defaultValue:a.fmatter.util.NumberFormat(c,d)};a.fn.fmatter.date=function(c,b,d,e){d=a.extend({},b.date);void 0!==b.colModel&&
	void 0!==b.colModel.formatoptions&&(d=a.extend({},d,b.colModel.formatoptions));return d.reformatAfterEdit||"edit"!==e?a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):a.jgrid.parseDate(d.srcformat,c,d.newformat,d):a.fn.fmatter.defaultFormat(c,b)};a.fn.fmatter.select=function(c,b){c=String(c);var d=!1,e=[],f,g;void 0!==b.colModel.formatoptions?(d=b.colModel.formatoptions.value,f=void 0===b.colModel.formatoptions.separator?":":b.colModel.formatoptions.separator,g=void 0===b.colModel.formatoptions.delimiter?
	";":b.colModel.formatoptions.delimiter):void 0!==b.colModel.editoptions&&(d=b.colModel.editoptions.value,f=void 0===b.colModel.editoptions.separator?":":b.colModel.editoptions.separator,g=void 0===b.colModel.editoptions.delimiter?";":b.colModel.editoptions.delimiter);if(d){var h=!0===b.colModel.editoptions.multiple?!0:!1,l=[];h&&(l=c.split(","),l=a.map(l,function(b){return a.trim(b)}));if(a.fmatter.isString(d)){var k=d.split(g),m=0,n;for(n=0;n<k.length;n++)if(g=k[n].split(f),2<g.length&&(g[1]=a.map(g,
	function(a,b){if(0<b)return a}).join(f)),h)-1<a.inArray(g[0],l)&&(e[m]=g[1],m++);else if(a.trim(g[0])===a.trim(c)){e[0]=g[1];break}}else a.fmatter.isObject(d)&&(h?e=a.map(l,function(a){return d[a]}):e[0]=d[c]||"")}c=e.join(", ");return""===c?a.fn.fmatter.defaultFormat(c,b):c};a.fn.fmatter.rowactions=function(c){var b=a(this).closest("tr.jqgrow"),d=b.attr("id"),e=a(this).closest("table.ui-jqgrid-btable").attr("id").replace(/_frozen([^_]*)$/,"$1"),e=a("#"+e),f=e[0],g=f.p,h=g.colModel[a.jgrid.getCellIndex(this)],
	l=h.frozen?a("tr#"+d+" td:eq("+a.jgrid.getCellIndex(this)+") > div",e):a(this).parent(),k={extraparam:{}},m=function(b){a.isFunction(k.afterRestore)&&k.afterRestore.call(f,b);l.find("div.ui-inline-edit,div.ui-inline-del").show();l.find("div.ui-inline-save,div.ui-inline-cancel").hide()};void 0!==h.formatoptions&&(k=a.extend(k,h.formatoptions));void 0!==g.editOptions&&(k.editOptions=g.editOptions);void 0!==g.delOptions&&(k.delOptions=g.delOptions);b.hasClass("jqgrid-new-row")&&(k.extraparam[g.prmNames.oper]=
	g.prmNames.addoper);b={keys:k.keys,oneditfunc:k.onEdit,successfunc:k.onSuccess,url:k.url,extraparam:k.extraparam,aftersavefunc:function(b,c){a.isFunction(k.afterSave)&&k.afterSave.call(f,b,c);l.find("div.ui-inline-edit,div.ui-inline-del").show();l.find("div.ui-inline-save,div.ui-inline-cancel").hide()},errorfunc:k.onError,afterrestorefunc:m,restoreAfterError:k.restoreAfterError,mtype:k.mtype};switch(c){case "edit":e.jqGrid("editRow",d,b);l.find("div.ui-inline-edit,div.ui-inline-del").hide();l.find("div.ui-inline-save,div.ui-inline-cancel").show();
	e.triggerHandler("jqGridAfterGridComplete");break;case "save":e.jqGrid("saveRow",d,b)&&(l.find("div.ui-inline-edit,div.ui-inline-del").show(),l.find("div.ui-inline-save,div.ui-inline-cancel").hide(),e.triggerHandler("jqGridAfterGridComplete"));break;case "cancel":e.jqGrid("restoreRow",d,m);l.find("div.ui-inline-edit,div.ui-inline-del").show();l.find("div.ui-inline-save,div.ui-inline-cancel").hide();e.triggerHandler("jqGridAfterGridComplete");break;case "del":e.jqGrid("delGridRow",d,k.delOptions);
	break;case "formedit":e.jqGrid("setSelection",d),e.jqGrid("editGridRow",d,k.editOptions)}};a.fn.fmatter.actions=function(c,b){var d={keys:!1,editbutton:!0,delbutton:!0,editformbutton:!1},e=b.rowId,f="";void 0!==b.colModel.formatoptions&&(d=a.extend(d,b.colModel.formatoptions));if(void 0===e||a.fmatter.isEmpty(e))return"";d.editformbutton?f+="<div title='"+a.jgrid.nav.edittitle+"' style='float:left;cursor:pointer;' class='ui-pg-div ui-inline-edit' "+("id='jEditButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'formedit'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+
	"><span class='ui-icon ui-icon-pencil'></span></div>":d.editbutton&&(f+="<div title='"+a.jgrid.nav.edittitle+"' style='float:left;cursor:pointer;' class='ui-pg-div ui-inline-edit' "+("id='jEditButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'edit'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover') ")+"><span class='ui-icon ui-icon-pencil'></span></div>");d.delbutton&&(f+="<div title='"+a.jgrid.nav.deltitle+"' style='float:left;margin-left:5px;' class='ui-pg-div ui-inline-del' "+
	("id='jDeleteButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'del'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+"><span class='ui-icon ui-icon-trash'></span></div>");f+="<div title='"+a.jgrid.edit.bSubmit+"' style='float:left;display:none' class='ui-pg-div ui-inline-save' "+("id='jSaveButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'save'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+
	"><span class='ui-icon ui-icon-disk'></span></div>";f+="<div title='"+a.jgrid.edit.bCancel+"' style='float:left;display:none;margin-left:5px;' class='ui-pg-div ui-inline-cancel' "+("id='jCancelButton_"+e+"' onclick=jQuery.fn.fmatter.rowactions.call(this,'cancel'); onmouseover=jQuery(this).addClass('ui-state-hover'); onmouseout=jQuery(this).removeClass('ui-state-hover'); ")+"><span class='ui-icon ui-icon-cancel'></span></div>";return"<div style='margin-left:8px;'>"+f+"</div>"};a.unformat=function(c,
	b,d,e){var f,g=b.colModel.formatter,h=b.colModel.formatoptions||{},l=/([\.\*\_\'\(\)\{\}\+\?\\])/g,k=b.colModel.unformat||a.fn.fmatter[g]&&a.fn.fmatter[g].unformat;if(void 0!==k&&a.isFunction(k))f=k.call(this,a(c).text(),b,c);else if(void 0!==g&&a.fmatter.isString(g))switch(f=a.jgrid.formatter||{},g){case "integer":h=a.extend({},f.integer,h);b=h.thousandsSeparator.replace(l,"\\$1");b=RegExp(b,"g");f=a(c).text().replace(b,"");break;case "number":h=a.extend({},f.number,h);b=h.thousandsSeparator.replace(l,
	"\\$1");b=RegExp(b,"g");f=a(c).text().replace(b,"").replace(h.decimalSeparator,".");break;case "currency":h=a.extend({},f.currency,h);b=h.thousandsSeparator.replace(l,"\\$1");b=RegExp(b,"g");f=a(c).text();h.prefix&&h.prefix.length&&(f=f.substr(h.prefix.length));h.suffix&&h.suffix.length&&(f=f.substr(0,f.length-h.suffix.length));f=f.replace(b,"").replace(h.decimalSeparator,".");break;case "checkbox":h=b.colModel.editoptions?b.colModel.editoptions.value.split(":"):["Yes","No"];f=a("input",c).is(":checked")?
	h[0]:h[1];break;case "select":f=a.unformat.select(c,b,d,e);break;case "actions":return"";default:f=a(c).text()}return void 0!==f?f:!0===e?a(c).text():a.jgrid.htmlDecode(a(c).html())};a.unformat.select=function(c,b,d,e){d=[];c=a(c).text();if(!0===e)return c;e=a.extend({},void 0!==b.colModel.formatoptions?b.colModel.formatoptions:b.colModel.editoptions);b=void 0===e.separator?":":e.separator;var f=void 0===e.delimiter?";":e.delimiter;if(e.value){var g=e.value;e=!0===e.multiple?!0:!1;var h=[];e&&(h=
	c.split(","),h=a.map(h,function(b){return a.trim(b)}));if(a.fmatter.isString(g)){var l=g.split(f),k=0,m;for(m=0;m<l.length;m++)if(f=l[m].split(b),2<f.length&&(f[1]=a.map(f,function(a,b){if(0<b)return a}).join(b)),e)-1<a.inArray(f[1],h)&&(d[k]=f[0],k++);else if(a.trim(f[1])===a.trim(c)){d[0]=f[0];break}}else if(a.fmatter.isObject(g)||a.isArray(g))e||(h[0]=c),d=a.map(h,function(b){var c;a.each(g,function(a,d){if(d===b)return c=a,!1});if(void 0!==c)return c});return d.join(", ")}return c||""};a.unformat.date=
	function(c,b){var d=a.jgrid.formatter.date||{};void 0!==b.formatoptions&&(d=a.extend({},d,b.formatoptions));return a.fmatter.isEmpty(c)?a.fn.fmatter.defaultFormat(c,b):a.jgrid.parseDate(d.newformat,c,d.srcformat,d)}})(jQuery);
	(function(a){a.jgrid.extend({getColProp:function(a){var c={},d=this[0];if(!d.grid)return!1;var d=d.p.colModel,h;for(h=0;h<d.length;h++)if(d[h].name===a){c=d[h];break}return c},setColProp:function(b,c){return this.each(function(){if(this.grid&&c){var d=this.p.colModel,h;for(h=0;h<d.length;h++)if(d[h].name===b){a.extend(!0,this.p.colModel[h],c);break}}})},sortGrid:function(a,c,d){return this.each(function(){var h=-1,k,e=!1;if(this.grid){a||(a=this.p.sortname);for(k=0;k<this.p.colModel.length;k++)if(this.p.colModel[k].index===
	a||this.p.colModel[k].name===a){h=k;!0===this.p.frozenColumns&&!0===this.p.colModel[k].frozen&&(e=this.grid.fhDiv.find("#"+this.p.id+"_"+a));break}-1!==h&&(k=this.p.colModel[h].sortable,e||(e=this.grid.headers[h].el),"boolean"!==typeof k&&(k=!0),"boolean"!==typeof c&&(c=!1),k&&this.sortData("jqgh_"+this.p.id+"_"+a,h,c,d,e))}})},clearBeforeUnload:function(){return this.each(function(){var b=this.grid;a.isFunction(b.emptyRows)&&b.emptyRows.call(this,!0,!0);a(document).unbind("mouseup.jqGrid"+this.p.id);
	a(b.hDiv).unbind("mousemove");a(this).unbind();b.dragEnd=null;b.dragMove=null;b.dragStart=null;b.emptyRows=null;b.populate=null;b.populateVisible=null;b.scrollGrid=null;b.selectionPreserver=null;b.bDiv=null;b.cDiv=null;b.hDiv=null;b.cols=null;var c,d=b.headers.length;for(c=0;c<d;c++)b.headers[c].el=null;this.grid=this.addJSONData=this.addXmlData=this.formatter=this.constructTr=this.setHeadCheckBox=this.refreshIndex=this.updatepager=this.sortData=this.formatCol=null})},GridDestroy:function(){return this.each(function(){if(this.grid){this.p.pager&&
	a(this.p.pager).remove();try{a(this).jqGrid("clearBeforeUnload"),a("#gbox_"+a.jgrid.jqID(this.id)).remove()}catch(b){}}})},GridUnload:function(){return this.each(function(){if(this.grid){var b=a(this).attr("id"),c=a(this).attr("class");this.p.pager&&a(this.p.pager).empty().removeClass("ui-state-default ui-jqgrid-pager ui-corner-bottom");var d=document.createElement("table");a(d).attr({id:b});d.className=c;b=a.jgrid.jqID(this.id);a(d).removeClass("ui-jqgrid-btable");1===a(this.p.pager).parents("#gbox_"+
	b).length?(a(d).insertBefore("#gbox_"+b).show(),a(this.p.pager).insertBefore("#gbox_"+b)):a(d).insertBefore("#gbox_"+b).show();a(this).jqGrid("clearBeforeUnload");a("#gbox_"+b).remove()}})},setGridState:function(b){return this.each(function(){this.grid&&("hidden"===b?(a(".ui-jqgrid-bdiv, .ui-jqgrid-hdiv","#gview_"+a.jgrid.jqID(this.p.id)).slideUp("fast"),this.p.pager&&a(this.p.pager).slideUp("fast"),this.p.toppager&&a(this.p.toppager).slideUp("fast"),!0===this.p.toolbar[0]&&("both"===this.p.toolbar[1]&&
	a(this.grid.ubDiv).slideUp("fast"),a(this.grid.uDiv).slideUp("fast")),this.p.footerrow&&a(".ui-jqgrid-sdiv","#gbox_"+a.jgrid.jqID(this.p.id)).slideUp("fast"),a(".ui-jqgrid-titlebar-close span",this.grid.cDiv).removeClass("ui-icon-circle-triangle-n").addClass("ui-icon-circle-triangle-s"),this.p.gridstate="hidden"):"visible"===b&&(a(".ui-jqgrid-hdiv, .ui-jqgrid-bdiv","#gview_"+a.jgrid.jqID(this.p.id)).slideDown("fast"),this.p.pager&&a(this.p.pager).slideDown("fast"),this.p.toppager&&a(this.p.toppager).slideDown("fast"),
	!0===this.p.toolbar[0]&&("both"===this.p.toolbar[1]&&a(this.grid.ubDiv).slideDown("fast"),a(this.grid.uDiv).slideDown("fast")),this.p.footerrow&&a(".ui-jqgrid-sdiv","#gbox_"+a.jgrid.jqID(this.p.id)).slideDown("fast"),a(".ui-jqgrid-titlebar-close span",this.grid.cDiv).removeClass("ui-icon-circle-triangle-s").addClass("ui-icon-circle-triangle-n"),this.p.gridstate="visible"))})},filterToolbar:function(b){b=a.extend({autosearch:!0,searchOnEnter:!0,beforeSearch:null,afterSearch:null,beforeClear:null,afterClear:null,
	searchurl:"",stringResult:!1,groupOp:"AND",defaultSearch:"bw",searchOperators:!1,resetIcon:"x",operands:{eq:"==",ne:"!",lt:"<",le:"<=",gt:">",ge:">=",bw:"^",bn:"!^","in":"=",ni:"!=",ew:"|",en:"!@",cn:"~",nc:"!~",nu:"#",nn:"!#"}},a.jgrid.search,b||{});return this.each(function(){var c=this;if(!this.ftoolbar){var d=function(){var d={},f=0,g,m,e={},q;a.each(c.p.colModel,function(){var l=a("#gs_"+a.jgrid.jqID(this.name),!0===this.frozen&&!0===c.p.frozenColumns?c.grid.fhDiv:c.grid.hDiv);m=this.index||
	this.name;q=b.searchOperators?l.parent().prev().children("a").attr("soper")||b.defaultSearch:this.searchoptions&&this.searchoptions.sopt?this.searchoptions.sopt[0]:"select"===this.stype?"eq":b.defaultSearch;if((g="custom"===this.stype&&a.isFunction(this.searchoptions.custom_value)&&0<l.length&&"SPAN"===l[0].nodeName.toUpperCase()?this.searchoptions.custom_value.call(c,l.children(".customelement:first"),"get"):l.val())||"nu"===q||"nn"===q)d[m]=g,e[m]=q,f++;else try{delete c.p.postData[m]}catch(k){}});
	var k=0<f?!0:!1;if(!0===b.stringResult||"local"===c.p.datatype){var l='{"groupOp":"'+b.groupOp+'","rules":[',n=0;a.each(d,function(a,b){0<n&&(l+=",");l+='{"field":"'+a+'",';l+='"op":"'+e[a]+'",';l+='"data":"'+(b+"").replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"}';n++});l+="]}";a.extend(c.p.postData,{filters:l});a.each(["searchField","searchString","searchOper"],function(a,b){c.p.postData.hasOwnProperty(b)&&delete c.p.postData[b]})}else a.extend(c.p.postData,d);var r;c.p.searchurl&&(r=c.p.url,a(c).jqGrid("setGridParam",
	{url:c.p.searchurl}));var h="stop"===a(c).triggerHandler("jqGridToolbarBeforeSearch")?!0:!1;!h&&a.isFunction(b.beforeSearch)&&(h=b.beforeSearch.call(c));h||a(c).jqGrid("setGridParam",{search:k}).trigger("reloadGrid",[{page:1}]);r&&a(c).jqGrid("setGridParam",{url:r});a(c).triggerHandler("jqGridToolbarAfterSearch");a.isFunction(b.afterSearch)&&b.afterSearch.call(c)},h=function(e,f,g){a("#sopt_menu").remove();f=parseInt(f,10);g=parseInt(g,10)+18;f='<ul id="sopt_menu" class="ui-search-menu" role="menu" tabindex="0" style="font-size:'+
	(a(".ui-jqgrid-view").css("font-size")||"11px")+";left:"+f+"px;top:"+g+'px;">';g=a(e).attr("soper");var k,h=[],q,p=0,l=a(e).attr("colname");for(k=c.p.colModel.length;p<k&&c.p.colModel[p].name!==l;)p++;p=c.p.colModel[p];l=a.extend({},p.searchoptions);l.sopt||(l.sopt=[],l.sopt[0]="select"===p.stype?"eq":b.defaultSearch);a.each(b.odata,function(){h.push(this.oper)});for(p=0;p<l.sopt.length;p++)q=a.inArray(l.sopt[p],h),-1!==q&&(k=g===b.odata[q].oper?"ui-state-highlight":"",f+='<li class="ui-menu-item '+
	k+'" role="presentation"><a class="ui-corner-all g-menu-item" tabindex="0" role="menuitem" value="'+b.odata[q].oper+'" oper="'+b.operands[b.odata[q].oper]+'"><table cellspacing="0" cellpadding="0" border="0"><tr><td width="25px">'+b.operands[b.odata[q].oper]+"</td><td>"+b.odata[q].text+"</td></tr></table></a></li>");f+="</ul>";a("body").append(f);a("#sopt_menu").addClass("ui-menu ui-widget ui-widget-content ui-corner-all");a("#sopt_menu > li > a").hover(function(){a(this).addClass("ui-state-hover")},
	function(){a(this).removeClass("ui-state-hover")}).click(function(f){f=a(this).attr("value");var g=a(this).attr("oper");a(c).triggerHandler("jqGridToolbarSelectOper",[f,g,e]);a("#sopt_menu").hide();a(e).text(g).attr("soper",f);!0===b.autosearch&&(g=a(e).parent().next().children()[0],(a(g).val()||"nu"===f||"nn"===f)&&d())})},k=a("<tr class='ui-search-toolbar' role='rowheader'></tr>"),e;a.each(c.p.colModel,function(h){var f=this,g,m;m="";var x="=",q,p=a("<th role='columnheader' class='ui-state-default ui-th-column ui-th-"+
	c.p.direction+"'></th>"),l=a("<div style='position:relative;height:100%;padding-right:0.3em;padding-left:0.3em;'></div>"),n=a("<table class='ui-search-table' cellspacing='0'><tr><td class='ui-search-oper'></td><td class='ui-search-input'></td><td class='ui-search-clear'></td></tr></table>");!0===this.hidden&&a(p).css("display","none");this.search=!1===this.search?!1:!0;void 0===this.stype&&(this.stype="text");g=a.extend({},this.searchoptions||{});if(this.search){if(b.searchOperators){m=g.sopt?g.sopt[0]:
	"select"===f.stype?"eq":b.defaultSearch;for(q=0;q<b.odata.length;q++)if(b.odata[q].oper===m){x=b.operands[m]||"";break}m="<a title='"+(null!=g.searchtitle?g.searchtitle:b.operandTitle)+"' style='padding-right: 0.5em;' soper='"+m+"' class='soptclass' colname='"+this.name+"'>"+x+"</a>"}a("td:eq(0)",n).attr("colindex",h).append(m);void 0===g.clearSearch&&(g.clearSearch=!0);g.clearSearch?(m=b.resetTitle||"Clear Search Value",a("td:eq(2)",n).append("<a title='"+m+"' style='padding-right: 0.3em;padding-left: 0.3em;' class='clearsearchclass'>"+
	b.resetIcon+"</a>")):a("td:eq(2)",n).hide();switch(this.stype){case "select":if(m=this.surl||g.dataUrl)a(l).append(n),a.ajax(a.extend({url:m,dataType:"html",success:function(e){void 0!==g.buildSelect?(e=g.buildSelect(e))&&a("td:eq(1)",n).append(e):a("td:eq(1)",n).append(e);void 0!==g.defaultValue&&a("select",l).val(g.defaultValue);a("select",l).attr({name:f.index||f.name,id:"gs_"+f.name});g.attr&&a("select",l).attr(g.attr);a("select",l).css({width:"100%"});a.jgrid.bindEv.call(c,a("select",l)[0],g);
	!0===b.autosearch&&a("select",l).change(function(){d();return!1});e=null}},a.jgrid.ajaxOptions,c.p.ajaxSelectOptions||{}));else{var r,w,u;f.searchoptions?(r=void 0===f.searchoptions.value?"":f.searchoptions.value,w=void 0===f.searchoptions.separator?":":f.searchoptions.separator,u=void 0===f.searchoptions.delimiter?";":f.searchoptions.delimiter):f.editoptions&&(r=void 0===f.editoptions.value?"":f.editoptions.value,w=void 0===f.editoptions.separator?":":f.editoptions.separator,u=void 0===f.editoptions.delimiter?
	";":f.editoptions.delimiter);if(r){var t=document.createElement("select");t.style.width="100%";a(t).attr({name:f.index||f.name,id:"gs_"+f.name});var v;if("string"===typeof r)for(m=r.split(u),v=0;v<m.length;v++)r=m[v].split(w),u=document.createElement("option"),u.value=r[0],u.innerHTML=r[1],t.appendChild(u);else if("object"===typeof r)for(v in r)r.hasOwnProperty(v)&&(u=document.createElement("option"),u.value=v,u.innerHTML=r[v],t.appendChild(u));void 0!==g.defaultValue&&a(t).val(g.defaultValue);g.attr&&
	a(t).attr(g.attr);a(l).append(n);a.jgrid.bindEv.call(c,t,g);a("td:eq(1)",n).append(t);!0===b.autosearch&&a(t).change(function(){d();return!1})}}break;case "text":w=void 0!==g.defaultValue?g.defaultValue:"";a("td:eq(1)",n).append("<input type='text' style='width:100%;padding:0px;' name='"+(f.index||f.name)+"' id='gs_"+f.name+"' value='"+w+"'/>");a(l).append(n);g.attr&&a("input",l).attr(g.attr);a.jgrid.bindEv.call(c,a("input",l)[0],g);!0===b.autosearch&&(b.searchOnEnter?a("input",l).keypress(function(a){return 13===
	(a.charCode||a.keyCode||0)?(d(),!1):this}):a("input",l).keydown(function(a){switch(a.which){case 13:return!1;case 9:case 16:case 37:case 38:case 39:case 40:case 27:break;default:e&&clearTimeout(e),e=setTimeout(function(){d()},500)}}));break;case "custom":a("td:eq(1)",n).append("<span style='width:95%;padding:0px;' name='"+(f.index||f.name)+"' id='gs_"+f.name+"'/>");a(l).append(n);try{if(a.isFunction(g.custom_element))if(t=g.custom_element.call(c,void 0!==g.defaultValue?g.defaultValue:"",g))t=a(t).addClass("customelement"),
	a(l).find(">span").append(t);else throw"e2";else throw"e1";}catch(y){"e1"===y&&a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.nodefined,a.jgrid.edit.bClose),"e2"===y?a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.novalue,a.jgrid.edit.bClose):a.jgrid.info_dialog(a.jgrid.errors.errcap,"string"===typeof y?y:y.message,a.jgrid.edit.bClose)}}}a(p).append(l);a(k).append(p);b.searchOperators||a("td:eq(0)",n).hide()});a("table thead",
	c.grid.hDiv).append(k);b.searchOperators&&(a(".soptclass",k).click(function(b){var c=a(this).offset();h(this,c.left,c.top);b.stopPropagation()}),a("body").on("click",function(b){"soptclass"!==b.target.className&&a("#sopt_menu").hide()}));a(".clearsearchclass",k).click(function(e){e=a(this).parents("tr:first");var f=parseInt(a("td.ui-search-oper",e).attr("colindex"),10),g=a.extend({},c.p.colModel[f].searchoptions||{}),g=g.defaultValue?g.defaultValue:"";"select"===c.p.colModel[f].stype?g?a("td.ui-search-input select",
	e).val(g):a("td.ui-search-input select",e)[0].selectedIndex=0:a("td.ui-search-input input",e).val(g);!0===b.autosearch&&d()});this.ftoolbar=!0;this.triggerToolbar=d;this.clearToolbar=function(d){var f={},g=0,e;d="boolean"!==typeof d?!0:d;a.each(c.p.colModel,function(){var b,d=a("#gs_"+a.jgrid.jqID(this.name),!0===this.frozen&&!0===c.p.frozenColumns?c.grid.fhDiv:c.grid.hDiv);this.searchoptions&&void 0!==this.searchoptions.defaultValue&&(b=this.searchoptions.defaultValue);e=this.index||this.name;switch(this.stype){case "select":d.find("option").each(function(c){0===
	c&&(this.selected=!0);if(a(this).val()===b)return this.selected=!0,!1});if(void 0!==b)f[e]=b,g++;else try{delete c.p.postData[e]}catch(h){}break;case "text":d.val(b||"");if(void 0!==b)f[e]=b,g++;else try{delete c.p.postData[e]}catch(k){}break;case "custom":a.isFunction(this.searchoptions.custom_value)&&0<d.length&&"SPAN"===d[0].nodeName.toUpperCase()&&this.searchoptions.custom_value.call(c,d.children(".customelement:first"),"set",b||"")}});var k=0<g?!0:!1;c.p.resetsearch=!0;if(!0===b.stringResult||
	"local"===c.p.datatype){var h='{"groupOp":"'+b.groupOp+'","rules":[',p=0;a.each(f,function(a,b){0<p&&(h+=",");h+='{"field":"'+a+'",';h+='"op":"eq",';h+='"data":"'+(b+"").replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"}';p++});h+="]}";a.extend(c.p.postData,{filters:h});a.each(["searchField","searchString","searchOper"],function(a,b){c.p.postData.hasOwnProperty(b)&&delete c.p.postData[b]})}else a.extend(c.p.postData,f);var l;c.p.searchurl&&(l=c.p.url,a(c).jqGrid("setGridParam",{url:c.p.searchurl}));var n=
	"stop"===a(c).triggerHandler("jqGridToolbarBeforeClear")?!0:!1;!n&&a.isFunction(b.beforeClear)&&(n=b.beforeClear.call(c));n||d&&a(c).jqGrid("setGridParam",{search:k}).trigger("reloadGrid",[{page:1}]);l&&a(c).jqGrid("setGridParam",{url:l});a(c).triggerHandler("jqGridToolbarAfterClear");a.isFunction(b.afterClear)&&b.afterClear()};this.toggleToolbar=function(){var b=a("tr.ui-search-toolbar",c.grid.hDiv),d=!0===c.p.frozenColumns?a("tr.ui-search-toolbar",c.grid.fhDiv):!1;"none"===b.css("display")?(b.show(),
	d&&d.show()):(b.hide(),d&&d.hide())}}})},destroyFilterToolbar:function(){return this.each(function(){this.ftoolbar&&(this.toggleToolbar=this.clearToolbar=this.triggerToolbar=null,this.ftoolbar=!1,a(this.grid.hDiv).find("table thead tr.ui-search-toolbar").remove())})},destroyGroupHeader:function(b){void 0===b&&(b=!0);return this.each(function(){var c,d,h,k,e,s;d=this.grid;var f=a("table.ui-jqgrid-htable thead",d.hDiv),g=this.p.colModel;if(d){a(this).unbind(".setGroupHeaders");c=a("<tr>",{role:"rowheader"}).addClass("ui-jqgrid-labels");
	k=d.headers;d=0;for(h=k.length;d<h;d++){e=g[d].hidden?"none":"";e=a(k[d].el).width(k[d].width).css("display",e);try{e.removeAttr("rowSpan")}catch(m){e.attr("rowSpan",1)}c.append(e);s=e.children("span.ui-jqgrid-resize");0<s.length&&(s[0].style.height="");e.children("div")[0].style.top=""}a(f).children("tr.ui-jqgrid-labels").remove();a(f).prepend(c);!0===b&&a(this).jqGrid("setGridParam",{groupHeader:null})}})},setGroupHeaders:function(b){b=a.extend({useColSpanStyle:!1,groupHeaders:[]},b||{});return this.each(function(){this.p.groupHeader=
	b;var c,d,h=0,k,e,s,f,g,m=this.p.colModel,x=m.length,q=this.grid.headers,p=a("table.ui-jqgrid-htable",this.grid.hDiv),l=p.children("thead").children("tr.ui-jqgrid-labels:last").addClass("jqg-second-row-header");k=p.children("thead");var n=p.find(".jqg-first-row-header");void 0===n[0]?n=a("<tr>",{role:"row","aria-hidden":"true"}).addClass("jqg-first-row-header").css("height","auto"):n.empty();var r,w=function(a,b){var c=b.length,d;for(d=0;d<c;d++)if(b[d].startColumnName===a)return d;return-1};a(this).prepend(k);
	k=a("<tr>",{role:"rowheader"}).addClass("ui-jqgrid-labels jqg-third-row-header");for(c=0;c<x;c++)if(s=q[c].el,f=a(s),d=m[c],e={height:"0px",width:q[c].width+"px",display:d.hidden?"none":""},a("<th>",{role:"gridcell"}).css(e).addClass("ui-first-th-"+this.p.direction).appendTo(n),s.style.width="",e=w(d.name,b.groupHeaders),0<=e){e=b.groupHeaders[e];h=e.numberOfColumns;g=e.titleText;for(e=d=0;e<h&&c+e<x;e++)m[c+e].hidden||d++;e=a("<th>").attr({role:"columnheader"}).addClass("ui-state-default ui-th-column-header ui-th-"+
	this.p.direction).css({height:"22px","border-top":"0 none"}).html(g);0<d&&e.attr("colspan",String(d));this.p.headertitles&&e.attr("title",e.text());0===d&&e.hide();f.before(e);k.append(s);h-=1}else 0===h?b.useColSpanStyle?f.attr("rowspan","2"):(a("<th>",{role:"columnheader"}).addClass("ui-state-default ui-th-column-header ui-th-"+this.p.direction).css({display:d.hidden?"none":"","border-top":"0 none"}).insertBefore(f),k.append(s)):(k.append(s),h--);m=a(this).children("thead");m.prepend(n);k.insertAfter(l);
	p.append(m);b.useColSpanStyle&&(p.find("span.ui-jqgrid-resize").each(function(){var b=a(this).parent();b.is(":visible")&&(this.style.cssText="height: "+b.height()+"px !important; cursor: col-resize;")}),p.find("div.ui-jqgrid-sortable").each(function(){var b=a(this),c=b.parent();c.is(":visible")&&c.is(":has(span.ui-jqgrid-resize)")&&b.css("top",(c.height()-b.outerHeight())/2+"px")}));r=m.find("tr.jqg-first-row-header");a(this).bind("jqGridResizeStop.setGroupHeaders",function(a,b,c){r.find("th").eq(c).width(b)})})},
	setFrozenColumns:function(){return this.each(function(){if(this.grid){var b=this,c=b.p.colModel,d=0,h=c.length,k=-1,e=!1;if(!0!==b.p.subGrid&&!0!==b.p.treeGrid&&!0!==b.p.cellEdit&&!b.p.sortable&&!b.p.scroll){b.p.rownumbers&&d++;for(b.p.multiselect&&d++;d<h;){if(!0===c[d].frozen)e=!0,k=d;else break;d++}if(0<=k&&e){c=b.p.caption?a(b.grid.cDiv).outerHeight():0;d=a(".ui-jqgrid-htable","#gview_"+a.jgrid.jqID(b.p.id)).height();b.p.toppager&&(c+=a(b.grid.topDiv).outerHeight());!0===b.p.toolbar[0]&&"bottom"!==
	b.p.toolbar[1]&&(c+=a(b.grid.uDiv).outerHeight());b.grid.fhDiv=a('<div style="position:absolute;left:0px;top:'+c+"px;height:"+d+'px;" class="frozen-div ui-state-default ui-jqgrid-hdiv"></div>');b.grid.fbDiv=a('<div style="position:absolute;left:0px;top:'+(parseInt(c,10)+parseInt(d,10)+1)+'px;overflow-y:hidden" class="frozen-bdiv ui-jqgrid-bdiv"></div>');a("#gview_"+a.jgrid.jqID(b.p.id)).append(b.grid.fhDiv);c=a(".ui-jqgrid-htable","#gview_"+a.jgrid.jqID(b.p.id)).clone(!0);if(b.p.groupHeader){a("tr.jqg-first-row-header, tr.jqg-third-row-header",
	c).each(function(){a("th:gt("+k+")",this).remove()});var s=-1,f=-1,g,m;a("tr.jqg-second-row-header th",c).each(function(){g=parseInt(a(this).attr("colspan"),10);if(m=parseInt(a(this).attr("rowspan"),10))s++,f++;g&&(s+=g,f++);if(s===k)return!1});s!==k&&(f=k);a("tr.jqg-second-row-header",c).each(function(){a("th:gt("+f+")",this).remove()})}else a("tr",c).each(function(){a("th:gt("+k+")",this).remove()});a(c).width(1);a(b.grid.fhDiv).append(c).mousemove(function(a){if(b.grid.resizing)return b.grid.dragMove(a),
	!1});a(b).bind("jqGridResizeStop.setFrozenColumns",function(c,d,e){c=a(".ui-jqgrid-htable",b.grid.fhDiv);a("th:eq("+e+")",c).width(d);c=a(".ui-jqgrid-btable",b.grid.fbDiv);a("tr:first td:eq("+e+")",c).width(d)});a(b).bind("jqGridSortCol.setFrozenColumns",function(c,d,e){c=a("tr.ui-jqgrid-labels:last th:eq("+b.p.lastsort+")",b.grid.fhDiv);d=a("tr.ui-jqgrid-labels:last th:eq("+e+")",b.grid.fhDiv);a("span.ui-grid-ico-sort",c).addClass("ui-state-disabled");a(c).attr("aria-selected","false");a("span.ui-icon-"+
	b.p.sortorder,d).removeClass("ui-state-disabled");a(d).attr("aria-selected","true");b.p.viewsortcols[0]||b.p.lastsort===e||(a("span.s-ico",c).hide(),a("span.s-ico",d).show())});a("#gview_"+a.jgrid.jqID(b.p.id)).append(b.grid.fbDiv);a(b.grid.bDiv).scroll(function(){a(b.grid.fbDiv).scrollTop(a(this).scrollTop())});!0===b.p.hoverrows&&a("#"+a.jgrid.jqID(b.p.id)).unbind("mouseover").unbind("mouseout");a(b).bind("jqGridAfterGridComplete.setFrozenColumns",function(){a("#"+a.jgrid.jqID(b.p.id)+"_frozen").remove();
	a(b.grid.fbDiv).height(a(b.grid.bDiv).height()-16);var c=a("#"+a.jgrid.jqID(b.p.id)).clone(!0);a("tr[role=row]",c).each(function(){a("td[role=gridcell]:gt("+k+")",this).remove()});a(c).width(1).attr("id",b.p.id+"_frozen");a(b.grid.fbDiv).append(c);!0===b.p.hoverrows&&(a("tr.jqgrow",c).hover(function(){a(this).addClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+a.jgrid.jqID(b.p.id)).addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+
	a.jgrid.jqID(b.p.id)).removeClass("ui-state-hover")}),a("tr.jqgrow","#"+a.jgrid.jqID(b.p.id)).hover(function(){a(this).addClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+a.jgrid.jqID(b.p.id)+"_frozen").addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover");a("#"+a.jgrid.jqID(this.id),"#"+a.jgrid.jqID(b.p.id)+"_frozen").removeClass("ui-state-hover")}));c=null});b.grid.hDiv.loading||a(b).triggerHandler("jqGridAfterGridComplete");b.p.frozenColumns=!0}}}})},destroyFrozenColumns:function(){return this.each(function(){if(this.grid&&
	!0===this.p.frozenColumns){a(this.grid.fhDiv).remove();a(this.grid.fbDiv).remove();this.grid.fhDiv=null;this.grid.fbDiv=null;a(this).unbind(".setFrozenColumns");if(!0===this.p.hoverrows){var b;a("#"+a.jgrid.jqID(this.p.id)).bind("mouseover",function(c){b=a(c.target).closest("tr.jqgrow");"ui-subgrid"!==a(b).attr("class")&&a(b).addClass("ui-state-hover")}).bind("mouseout",function(c){b=a(c.target).closest("tr.jqgrow");a(b).removeClass("ui-state-hover")})}this.p.frozenColumns=!1}})}})})(jQuery);
	(function(a){a.extend(a.jgrid,{showModal:function(a){a.w.show()},closeModal:function(a){a.w.hide().attr("aria-hidden","true");a.o&&a.o.remove()},hideModal:function(d,b){b=a.extend({jqm:!0,gb:""},b||{});if(b.onClose){var c=b.gb&&"string"===typeof b.gb&&"#gbox_"===b.gb.substr(0,6)?b.onClose.call(a("#"+b.gb.substr(6))[0],d):b.onClose(d);if("boolean"===typeof c&&!c)return}if(a.fn.jqm&&!0===b.jqm)a(d).attr("aria-hidden","true").jqmHide();else{if(""!==b.gb)try{a(".jqgrid-overlay:first",b.gb).hide()}catch(g){}a(d).hide().attr("aria-hidden",
	"true")}},findPos:function(a){var b=0,c=0;if(a.offsetParent){do b+=a.offsetLeft,c+=a.offsetTop;while(a=a.offsetParent)}return[b,c]},createModal:function(d,b,c,g,e,h,f){c=a.extend(!0,{},a.jgrid.jqModal||{},c);var k=document.createElement("div"),l,m=this;f=a.extend({},f||{});l="rtl"===a(c.gbox).attr("dir")?!0:!1;k.className="ui-widget ui-widget-content ui-corner-all ui-jqdialog";k.id=d.themodal;var n=document.createElement("div");n.className="ui-jqdialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix";
	n.id=d.modalhead;a(n).append("<span class='ui-jqdialog-title'>"+c.caption+"</span>");var q=a("<a class='ui-jqdialog-titlebar-close ui-corner-all'></a>").hover(function(){q.addClass("ui-state-hover")},function(){q.removeClass("ui-state-hover")}).append("<span class='ui-icon ui-icon-closethick'></span>");a(n).append(q);l?(k.dir="rtl",a(".ui-jqdialog-title",n).css("float","right"),a(".ui-jqdialog-titlebar-close",n).css("left","0.3em")):(k.dir="ltr",a(".ui-jqdialog-title",n).css("float","left"),a(".ui-jqdialog-titlebar-close",
	n).css("right","0.3em"));var p=document.createElement("div");a(p).addClass("ui-jqdialog-content ui-widget-content").attr("id",d.modalcontent);a(p).append(b);k.appendChild(p);a(k).prepend(n);!0===h?a("body").append(k):"string"===typeof h?a(h).append(k):a(k).insertBefore(g);a(k).css(f);void 0===c.jqModal&&(c.jqModal=!0);b={};if(a.fn.jqm&&!0===c.jqModal)0===c.left&&0===c.top&&c.overlay&&(f=[],f=a.jgrid.findPos(e),c.left=f[0]+4,c.top=f[1]+4),b.top=c.top+"px",b.left=c.left;else if(0!==c.left||0!==c.top)b.left=
	c.left,b.top=c.top+"px";a("a.ui-jqdialog-titlebar-close",n).click(function(){var b=a("#"+a.jgrid.jqID(d.themodal)).data("onClose")||c.onClose,e=a("#"+a.jgrid.jqID(d.themodal)).data("gbox")||c.gbox;m.hideModal("#"+a.jgrid.jqID(d.themodal),{gb:e,jqm:c.jqModal,onClose:b});return!1});0!==c.width&&c.width||(c.width=300);0!==c.height&&c.height||(c.height=200);c.zIndex||(g=a(g).parents("*[role=dialog]").filter(":first").css("z-index"),c.zIndex=g?parseInt(g,10)+2:950);g=0;l&&b.left&&!h&&(g=a(c.gbox).width()-
	(isNaN(c.width)?0:parseInt(c.width,10))-8,b.left=parseInt(b.left,10)+parseInt(g,10));b.left&&(b.left+="px");a(k).css(a.extend({width:isNaN(c.width)?"auto":c.width+"px",height:isNaN(c.height)?"auto":c.height+"px",zIndex:c.zIndex,overflow:"hidden"},b)).attr({tabIndex:"-1",role:"dialog","aria-labelledby":d.modalhead,"aria-hidden":"true"});void 0===c.drag&&(c.drag=!0);void 0===c.resize&&(c.resize=!0);if(c.drag)if(a(n).css("cursor","move"),a.fn.jqDrag)a(k).jqDrag(n);else try{a(k).draggable({handle:a("#"+
	a.jgrid.jqID(n.id))})}catch(r){}if(c.resize)if(a.fn.jqResize)a(k).append("<div class='jqResize ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se'></div>"),a("#"+a.jgrid.jqID(d.themodal)).jqResize(".jqResize",d.scrollelm?"#"+a.jgrid.jqID(d.scrollelm):!1);else try{a(k).resizable({handles:"se, sw",alsoResize:d.scrollelm?"#"+a.jgrid.jqID(d.scrollelm):!1})}catch(s){}!0===c.closeOnEscape&&a(k).keydown(function(b){27==b.which&&(b=a("#"+a.jgrid.jqID(d.themodal)).data("onClose")||c.onClose,
	m.hideModal("#"+a.jgrid.jqID(d.themodal),{gb:c.gbox,jqm:c.jqModal,onClose:b}))})},viewModal:function(d,b){b=a.extend({toTop:!0,overlay:10,modal:!1,overlayClass:"ui-widget-overlay",onShow:a.jgrid.showModal,onHide:a.jgrid.closeModal,gbox:"",jqm:!0,jqM:!0},b||{});if(a.fn.jqm&&!0===b.jqm)b.jqM?a(d).attr("aria-hidden","false").jqm(b).jqmShow():a(d).attr("aria-hidden","false").jqmShow();else{""!==b.gbox&&(a(".jqgrid-overlay:first",b.gbox).show(),a(d).data("gbox",b.gbox));a(d).show().attr("aria-hidden",
	"false");try{a(":input:visible",d)[0].focus()}catch(c){}}},info_dialog:function(d,b,c,g){var e={width:290,height:"auto",dataheight:"auto",drag:!0,resize:!1,left:250,top:170,zIndex:1E3,jqModal:!0,modal:!1,closeOnEscape:!0,align:"center",buttonalign:"center",buttons:[]};a.extend(!0,e,a.jgrid.jqModal||{},{caption:"<b>"+d+"</b>"},g||{});var h=e.jqModal,f=this;a.fn.jqm&&!h&&(h=!1);d="";if(0<e.buttons.length)for(g=0;g<e.buttons.length;g++)void 0===e.buttons[g].id&&(e.buttons[g].id="info_button_"+g),d+=
	"<a id='"+e.buttons[g].id+"' class='fm-button ui-state-default ui-corner-all'>"+e.buttons[g].text+"</a>";g=isNaN(e.dataheight)?e.dataheight:e.dataheight+"px";b="<div id='info_id'>"+("<div id='infocnt' style='margin:0px;padding-bottom:1em;width:100%;overflow:auto;position:relative;height:"+g+";"+("text-align:"+e.align+";")+"'>"+b+"</div>");b+=c?"<div class='ui-widget-content ui-helper-clearfix' style='text-align:"+e.buttonalign+";padding-bottom:0.8em;padding-top:0.5em;background-image: none;border-width: 1px 0 0 0;'><a id='closedialog' class='fm-button ui-state-default ui-corner-all'>"+
	c+"</a>"+d+"</div>":""!==d?"<div class='ui-widget-content ui-helper-clearfix' style='text-align:"+e.buttonalign+";padding-bottom:0.8em;padding-top:0.5em;background-image: none;border-width: 1px 0 0 0;'>"+d+"</div>":"";b+="</div>";try{"false"===a("#info_dialog").attr("aria-hidden")&&a.jgrid.hideModal("#info_dialog",{jqm:h}),a("#info_dialog").remove()}catch(k){}a.jgrid.createModal({themodal:"info_dialog",modalhead:"info_head",modalcontent:"info_content",scrollelm:"infocnt"},b,e,"","",!0);d&&a.each(e.buttons,
	function(b){a("#"+a.jgrid.jqID(this.id),"#info_id").bind("click",function(){e.buttons[b].onClick.call(a("#info_dialog"));return!1})});a("#closedialog","#info_id").click(function(){f.hideModal("#info_dialog",{jqm:h,onClose:a("#info_dialog").data("onClose")||e.onClose,gb:a("#info_dialog").data("gbox")||e.gbox});return!1});a(".fm-button","#info_dialog").hover(function(){a(this).addClass("ui-state-hover")},function(){a(this).removeClass("ui-state-hover")});a.isFunction(e.beforeOpen)&&e.beforeOpen();a.jgrid.viewModal("#info_dialog",
	{onHide:function(a){a.w.hide().remove();a.o&&a.o.remove()},modal:e.modal,jqm:h});a.isFunction(e.afterOpen)&&e.afterOpen();try{a("#info_dialog").focus()}catch(l){}},bindEv:function(d,b){a.isFunction(b.dataInit)&&b.dataInit.call(this,d,b);b.dataEvents&&a.each(b.dataEvents,function(){void 0!==this.data?a(d).bind(this.type,this.data,this.fn):a(d).bind(this.type,this.fn)})},createEl:function(d,b,c,g,e){function h(b,d,c){var e="dataInit dataEvents dataUrl buildSelect sopt searchhidden defaultValue attr custom_element custom_value".split(" ");
	void 0!==c&&a.isArray(c)&&a.merge(e,c);a.each(d,function(d,c){-1===a.inArray(d,e)&&a(b).attr(d,c)});d.hasOwnProperty("id")||a(b).attr("id",a.jgrid.randId())}var f="",k=this;switch(d){case "textarea":f=document.createElement("textarea");g?b.cols||a(f).css({width:"98%"}):b.cols||(b.cols=20);b.rows||(b.rows=2);if("&nbsp;"===c||"&#160;"===c||1===c.length&&160===c.charCodeAt(0))c="";f.value=c;h(f,b);a(f).attr({role:"textbox",multiline:"true"});break;case "checkbox":f=document.createElement("input");f.type=
	"checkbox";b.value?(d=b.value.split(":"),c===d[0]&&(f.checked=!0,f.defaultChecked=!0),f.value=d[0],a(f).attr("offval",d[1])):(d=(c+"").toLowerCase(),0>d.search(/(false|f|0|no|n|off|undefined)/i)&&""!==d?(f.checked=!0,f.defaultChecked=!0,f.value=c):f.value="on",a(f).attr("offval","off"));h(f,b,["value"]);a(f).attr("role","checkbox");break;case "select":f=document.createElement("select");f.setAttribute("role","select");g=[];!0===b.multiple?(d=!0,f.multiple="multiple",a(f).attr("aria-multiselectable",
	"true")):d=!1;if(void 0!==b.dataUrl){d=b.name?String(b.id).substring(0,String(b.id).length-String(b.name).length-1):String(b.id);var l=b.postData||e.postData;k.p&&k.p.idPrefix&&(d=a.jgrid.stripPref(k.p.idPrefix,d));a.ajax(a.extend({url:a.isFunction(b.dataUrl)?b.dataUrl.call(k,d,c,String(b.name)):b.dataUrl,type:"GET",dataType:"html",data:a.isFunction(l)?l.call(k,d,c,String(b.name)):l,context:{elem:f,options:b,vl:c},success:function(b){var d=[],c=this.elem,e=this.vl,f=a.extend({},this.options),g=!0===
	f.multiple;b=a.isFunction(f.buildSelect)?f.buildSelect.call(k,b):b;"string"===typeof b&&(b=a(a.trim(b)).html());b&&(a(c).append(b),h(c,f,l?["postData"]:void 0),void 0===f.size&&(f.size=g?3:1),g?(d=e.split(","),d=a.map(d,function(b){return a.trim(b)})):d[0]=a.trim(e),setTimeout(function(){a("option",c).each(function(b){0===b&&c.multiple&&(this.selected=!1);a(this).attr("role","option");if(-1<a.inArray(a.trim(a(this).text()),d)||-1<a.inArray(a.trim(a(this).val()),d))this.selected="selected"})},0))}},
	e||{}))}else if(b.value){var m;void 0===b.size&&(b.size=d?3:1);d&&(g=c.split(","),g=a.map(g,function(b){return a.trim(b)}));"function"===typeof b.value&&(b.value=b.value());var n,q,p=void 0===b.separator?":":b.separator;e=void 0===b.delimiter?";":b.delimiter;if("string"===typeof b.value)for(n=b.value.split(e),m=0;m<n.length;m++)q=n[m].split(p),2<q.length&&(q[1]=a.map(q,function(a,b){if(0<b)return a}).join(p)),e=document.createElement("option"),e.setAttribute("role","option"),e.value=q[0],e.innerHTML=
	q[1],f.appendChild(e),d||a.trim(q[0])!==a.trim(c)&&a.trim(q[1])!==a.trim(c)||(e.selected="selected"),d&&(-1<a.inArray(a.trim(q[1]),g)||-1<a.inArray(a.trim(q[0]),g))&&(e.selected="selected");else if("object"===typeof b.value)for(m in p=b.value,p)p.hasOwnProperty(m)&&(e=document.createElement("option"),e.setAttribute("role","option"),e.value=m,e.innerHTML=p[m],f.appendChild(e),d||a.trim(m)!==a.trim(c)&&a.trim(p[m])!==a.trim(c)||(e.selected="selected"),d&&(-1<a.inArray(a.trim(p[m]),g)||-1<a.inArray(a.trim(m),
	g))&&(e.selected="selected"));h(f,b,["value"])}break;case "text":case "password":case "button":m="button"===d?"button":"textbox";f=document.createElement("input");f.type=d;f.value=c;h(f,b);"button"!==d&&(g?b.size||a(f).css({width:"98%"}):b.size||(b.size=20));a(f).attr("role",m);break;case "image":case "file":f=document.createElement("input");f.type=d;h(f,b);break;case "custom":f=document.createElement("span");try{if(a.isFunction(b.custom_element))if(p=b.custom_element.call(k,c,b))p=a(p).addClass("customelement").attr({id:b.id,
	name:b.name}),a(f).empty().append(p);else throw"e2";else throw"e1";}catch(r){"e1"===r&&a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.nodefined,a.jgrid.edit.bClose),"e2"===r?a.jgrid.info_dialog(a.jgrid.errors.errcap,"function 'custom_element' "+a.jgrid.edit.msg.novalue,a.jgrid.edit.bClose):a.jgrid.info_dialog(a.jgrid.errors.errcap,"string"===typeof r?r:r.message,a.jgrid.edit.bClose)}}return f},checkDate:function(a,b){var c={},g;a=a.toLowerCase();g=-1!==a.indexOf("/")?
	"/":-1!==a.indexOf("-")?"-":-1!==a.indexOf(".")?".":"/";a=a.split(g);b=b.split(g);if(3!==b.length)return!1;var e=-1,h,f=g=-1,k;for(k=0;k<a.length;k++)h=isNaN(b[k])?0:parseInt(b[k],10),c[a[k]]=h,h=a[k],-1!==h.indexOf("y")&&(e=k),-1!==h.indexOf("m")&&(f=k),-1!==h.indexOf("d")&&(g=k);h="y"===a[e]||"yyyy"===a[e]?4:"yy"===a[e]?2:-1;k=[0,31,29,31,30,31,30,31,31,30,31,30,31];var l;if(-1===e)return!1;l=c[a[e]].toString();2===h&&1===l.length&&(h=1);if(l.length!==h||0===c[a[e]]&&"00"!==b[e]||-1===f)return!1;
	l=c[a[f]].toString();if(1>l.length||1>c[a[f]]||12<c[a[f]]||-1===g)return!1;l=c[a[g]].toString();if(!(h=1>l.length)&&!(h=1>c[a[g]])&&!(h=31<c[a[g]])){if(h=2===c[a[f]])e=c[a[e]],h=c[a[g]]>(0!==e%4||0===e%100&&0!==e%400?28:29);h=h||c[a[g]]>k[c[a[f]]]}return h?!1:!0},isEmpty:function(a){return a.match(/^\s+$/)||""===a?!0:!1},checkTime:function(d){var b=/^(\d{1,2}):(\d{2})([apAP][Mm])?$/;if(!a.jgrid.isEmpty(d))if(d=d.match(b)){if(d[3]){if(1>d[1]||12<d[1])return!1}else if(23<d[1])return!1;if(59<d[2])return!1}else return!1;
	return!0},checkValues:function(d,b,c,g){var e,h,f;f=this.p.colModel;if(void 0===c)if("string"===typeof b)for(c=0,g=f.length;c<g;c++){if(f[c].name===b){e=f[c].editrules;b=c;null!=f[c].formoptions&&(h=f[c].formoptions.label);break}}else 0<=b&&(e=f[b].editrules);else e=c,h=void 0===g?"_":g;if(e){h||(h=null!=this.p.colNames?this.p.colNames[b]:f[b].label);if(!0===e.required&&a.jgrid.isEmpty(d))return[!1,h+": "+a.jgrid.edit.msg.required,""];c=!1===e.required?!1:!0;if(!0===e.number&&(!1!==c||!a.jgrid.isEmpty(d))&&
	isNaN(d))return[!1,h+": "+a.jgrid.edit.msg.number,""];if(void 0!==e.minValue&&!isNaN(e.minValue)&&parseFloat(d)<parseFloat(e.minValue))return[!1,h+": "+a.jgrid.edit.msg.minValue+" "+e.minValue,""];if(void 0!==e.maxValue&&!isNaN(e.maxValue)&&parseFloat(d)>parseFloat(e.maxValue))return[!1,h+": "+a.jgrid.edit.msg.maxValue+" "+e.maxValue,""];if(!(!0!==e.email||!1===c&&a.jgrid.isEmpty(d)||(g=/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i,
	g.test(d))))return[!1,h+": "+a.jgrid.edit.msg.email,""];if(!(!0!==e.integer||!1===c&&a.jgrid.isEmpty(d)||!isNaN(d)&&0===d%1&&-1===d.indexOf(".")))return[!1,h+": "+a.jgrid.edit.msg.integer,""];if(!(!0!==e.date||!1===c&&a.jgrid.isEmpty(d)||(f[b].formatoptions&&f[b].formatoptions.newformat?(f=f[b].formatoptions.newformat,a.jgrid.formatter.date.masks.hasOwnProperty(f)&&(f=a.jgrid.formatter.date.masks[f])):f=f[b].datefmt||"Y-m-d",a.jgrid.checkDate(f,d))))return[!1,h+": "+a.jgrid.edit.msg.date+" - "+f,
	""];if(!0===e.time&&!(!1===c&&a.jgrid.isEmpty(d)||a.jgrid.checkTime(d)))return[!1,h+": "+a.jgrid.edit.msg.date+" - hh:mm (am/pm)",""];if(!(!0!==e.url||!1===c&&a.jgrid.isEmpty(d)||(g=/^(((https?)|(ftp)):\/\/([\-\w]+\.)+\w{2,3}(\/[%\-\w]+(\.\w{2,})?)*(([\w\-\.\?\\\/+@&#;`~=%!]*)(\.\w{2,})?)*\/?)/i,g.test(d))))return[!1,h+": "+a.jgrid.edit.msg.url,""];if(!0===e.custom&&(!1!==c||!a.jgrid.isEmpty(d)))return a.isFunction(e.custom_func)?(d=e.custom_func.call(this,d,h,b),a.isArray(d)?d:[!1,a.jgrid.edit.msg.customarray,
	""]):[!1,a.jgrid.edit.msg.customfcheck,""]}return[!0,"",""]}})})(jQuery);
	(function(b){b.jgrid.extend({setSubGrid:function(){return this.each(function(){var d,c;this.p.subGridOptions=b.extend({plusicon:"ui-icon-plus",minusicon:"ui-icon-minus",openicon:"ui-icon-carat-1-sw",expandOnLoad:!1,delayOnLoad:50,selectOnExpand:!1,selectOnCollapse:!1,reloadOnExpand:!0},this.p.subGridOptions||{});this.p.colNames.unshift("");this.p.colModel.unshift({name:"subgrid",width:b.jgrid.cell_width?this.p.subGridWidth+this.p.cellLayout:this.p.subGridWidth,sortable:!1,resizable:!1,hidedlg:!0,
	search:!1,fixed:!0});d=this.p.subGridModel;if(d[0])for(d[0].align=b.extend([],d[0].align||[]),c=0;c<d[0].name.length;c++)d[0].align[c]=d[0].align[c]||"left"})},addSubGridCell:function(b,c){var a="",p,n;this.each(function(){a=this.formatCol(b,c);n=this.p.id;p=this.p.subGridOptions.plusicon});return'<td role="gridcell" aria-describedby="'+n+'_subgrid" class="ui-sgcollapsed sgcollapsed" '+a+"><a style='cursor:pointer;'><span class='ui-icon "+p+"'></span></a></td>"},addSubGrid:function(d,c){return this.each(function(){var a=
	this;if(a.grid){var p=function(c,d,h){d=b("<td align='"+a.p.subGridModel[0].align[h]+"'></td>").html(d);b(c).append(d)},n=function(c,d){var h,f,e,g=b("<table cellspacing='0' cellpadding='0' border='0'><tbody></tbody></table>"),k=b("<tr></tr>");for(f=0;f<a.p.subGridModel[0].name.length;f++)h=b("<th class='ui-state-default ui-th-subgrid ui-th-column ui-th-"+a.p.direction+"'></th>"),b(h).html(a.p.subGridModel[0].name[f]),b(h).width(a.p.subGridModel[0].width[f]),b(k).append(h);b(g).append(k);c&&(e=a.p.xmlReader.subgrid,
	b(e.root+" "+e.row,c).each(function(){k=b("<tr class='ui-widget-content ui-subtblcell'></tr>");if(!0===e.repeatitems)b(e.cell,this).each(function(a){p(k,b(this).text()||"&#160;",a)});else{var c=a.p.subGridModel[0].mapping||a.p.subGridModel[0].name;if(c)for(f=0;f<c.length;f++)p(k,b(c[f],this).text()||"&#160;",f)}b(g).append(k)}));h=b("table:first",a.grid.bDiv).attr("id")+"_";b("#"+b.jgrid.jqID(h+d)).append(g);a.grid.hDiv.loading=!1;b("#load_"+b.jgrid.jqID(a.p.id)).hide();return!1},r=function(c,d){var h,
	f,e,g,k,m=b("<table cellspacing='0' cellpadding='0' border='0'><tbody></tbody></table>"),l=b("<tr></tr>");for(f=0;f<a.p.subGridModel[0].name.length;f++)h=b("<th class='ui-state-default ui-th-subgrid ui-th-column ui-th-"+a.p.direction+"'></th>"),b(h).html(a.p.subGridModel[0].name[f]),b(h).width(a.p.subGridModel[0].width[f]),b(l).append(h);b(m).append(l);if(c&&(g=a.p.jsonReader.subgrid,h=b.jgrid.getAccessor(c,g.root),void 0!==h))for(f=0;f<h.length;f++){e=h[f];l=b("<tr class='ui-widget-content ui-subtblcell'></tr>");
	if(!0===g.repeatitems)for(g.cell&&(e=e[g.cell]),k=0;k<e.length;k++)p(l,e[k]||"&#160;",k);else{var n=a.p.subGridModel[0].mapping||a.p.subGridModel[0].name;if(n.length)for(k=0;k<n.length;k++)p(l,e[n[k]]||"&#160;",k)}b(m).append(l)}f=b("table:first",a.grid.bDiv).attr("id")+"_";b("#"+b.jgrid.jqID(f+d)).append(m);a.grid.hDiv.loading=!1;b("#load_"+b.jgrid.jqID(a.p.id)).hide();return!1},v=function(c){var e,d,f,g;e=b(c).attr("id");d={nd_:(new Date).getTime()};d[a.p.prmNames.subgridid]=e;if(!a.p.subGridModel[0])return!1;
	if(a.p.subGridModel[0].params)for(g=0;g<a.p.subGridModel[0].params.length;g++)for(f=0;f<a.p.colModel.length;f++)a.p.colModel[f].name===a.p.subGridModel[0].params[g]&&(d[a.p.colModel[f].name]=b("td:eq("+f+")",c).text().replace(/\&#160\;/ig,""));if(!a.grid.hDiv.loading)switch(a.grid.hDiv.loading=!0,b("#load_"+b.jgrid.jqID(a.p.id)).show(),a.p.subgridtype||(a.p.subgridtype=a.p.datatype),b.isFunction(a.p.subgridtype)?a.p.subgridtype.call(a,d):a.p.subgridtype=a.p.subgridtype.toLowerCase(),a.p.subgridtype){case "xml":case "json":b.ajax(b.extend({type:a.p.mtype,
	url:a.p.subGridUrl,dataType:a.p.subgridtype,data:b.isFunction(a.p.serializeSubGridData)?a.p.serializeSubGridData.call(a,d):d,complete:function(c){"xml"===a.p.subgridtype?n(c.responseXML,e):r(b.jgrid.parse(c.responseText),e)}},b.jgrid.ajaxOptions,a.p.ajaxSubgridOptions||{}))}return!1},e,m,s,t=0,g,l;b.each(a.p.colModel,function(){!0!==this.hidden&&"rn"!==this.name&&"cb"!==this.name||t++});var u=a.rows.length,q=1;void 0!==c&&0<c&&(q=c,u=c+1);for(;q<u;)b(a.rows[q]).hasClass("jqgrow")&&b(a.rows[q].cells[d]).bind("click",
	function(){var c=b(this).parent("tr")[0];l=c.nextSibling;if(b(this).hasClass("sgcollapsed")){m=a.p.id;e=c.id;if(!0===a.p.subGridOptions.reloadOnExpand||!1===a.p.subGridOptions.reloadOnExpand&&!b(l).hasClass("ui-subgrid")){s=1<=d?"<td colspan='"+d+"'>&#160;</td>":"";g=b(a).triggerHandler("jqGridSubGridBeforeExpand",[m+"_"+e,e]);(g=!1===g||"stop"===g?!1:!0)&&b.isFunction(a.p.subGridBeforeExpand)&&(g=a.p.subGridBeforeExpand.call(a,m+"_"+e,e));if(!1===g)return!1;b(c).after("<tr role='row' class='ui-subgrid'>"+
	s+"<td class='ui-widget-content subgrid-cell'><span class='ui-icon "+a.p.subGridOptions.openicon+"'></span></td><td colspan='"+parseInt(a.p.colNames.length-1-t,10)+"' class='ui-widget-content subgrid-data'><div id="+m+"_"+e+" class='tablediv'></div></td></tr>");b(a).triggerHandler("jqGridSubGridRowExpanded",[m+"_"+e,e]);b.isFunction(a.p.subGridRowExpanded)?a.p.subGridRowExpanded.call(a,m+"_"+e,e):v(c)}else b(l).show();b(this).html("<a style='cursor:pointer;'><span class='ui-icon "+a.p.subGridOptions.minusicon+
	"'></span></a>").removeClass("sgcollapsed").addClass("sgexpanded");a.p.subGridOptions.selectOnExpand&&b(a).jqGrid("setSelection",e)}else if(b(this).hasClass("sgexpanded")){g=b(a).triggerHandler("jqGridSubGridRowColapsed",[m+"_"+e,e]);g=!1===g||"stop"===g?!1:!0;e=c.id;g&&b.isFunction(a.p.subGridRowColapsed)&&(g=a.p.subGridRowColapsed.call(a,m+"_"+e,e));if(!1===g)return!1;!0===a.p.subGridOptions.reloadOnExpand?b(l).remove(".ui-subgrid"):b(l).hasClass("ui-subgrid")&&b(l).hide();b(this).html("<a style='cursor:pointer;'><span class='ui-icon "+
	a.p.subGridOptions.plusicon+"'></span></a>").removeClass("sgexpanded").addClass("sgcollapsed");a.p.subGridOptions.selectOnCollapse&&b(a).jqGrid("setSelection",e)}return!1}),q++;!0===a.p.subGridOptions.expandOnLoad&&b(a.rows).filter(".jqgrow").each(function(a,c){b(c.cells[0]).click()});a.subGridXml=function(a,b){n(a,b)};a.subGridJson=function(a,b){r(a,b)}}})},expandSubGridRow:function(d){return this.each(function(){if((this.grid||d)&&!0===this.p.subGrid){var c=b(this).jqGrid("getInd",d,!0);c&&(c=b("td.sgcollapsed",
	c)[0])&&b(c).trigger("click")}})},collapseSubGridRow:function(d){return this.each(function(){if((this.grid||d)&&!0===this.p.subGrid){var c=b(this).jqGrid("getInd",d,!0);c&&(c=b("td.sgexpanded",c)[0])&&b(c).trigger("click")}})},toggleSubGridRow:function(d){return this.each(function(){if((this.grid||d)&&!0===this.p.subGrid){var c=b(this).jqGrid("getInd",d,!0);if(c){var a=b("td.sgcollapsed",c)[0];a?b(a).trigger("click"):(a=b("td.sgexpanded",c)[0])&&b(a).trigger("click")}}})}})})(jQuery);
	(function(d){d.extend(d.jgrid,{template:function(b){var k=d.makeArray(arguments).slice(1),a,c=k.length;null==b&&(b="");return b.replace(/\{([\w\-]+)(?:\:([\w\.]*)(?:\((.*?)?\))?)?\}/g,function(b,m){if(!isNaN(parseInt(m,10)))return k[parseInt(m,10)];for(a=0;a<c;a++)if(d.isArray(k[a]))for(var e=k[a],f=e.length;f--;)if(m===e[f].nm)return e[f].v})}});d.jgrid.extend({groupingSetup:function(){return this.each(function(){var b,k,a=this.p.colModel,c=this.p.groupingView;if(null===c||"object"!==typeof c&&!d.isFunction(c))this.p.grouping=
	!1;else if(c.groupField.length){void 0===c.visibiltyOnNextGrouping&&(c.visibiltyOnNextGrouping=[]);c.lastvalues=[];c._locgr||(c.groups=[]);c.counters=[];for(b=0;b<c.groupField.length;b++)c.groupOrder[b]||(c.groupOrder[b]="asc"),c.groupText[b]||(c.groupText[b]="{0}"),"boolean"!==typeof c.groupColumnShow[b]&&(c.groupColumnShow[b]=!0),"boolean"!==typeof c.groupSummary[b]&&(c.groupSummary[b]=!1),c.groupSummaryPos[b]||(c.groupSummaryPos[b]="footer"),!0===c.groupColumnShow[b]?(c.visibiltyOnNextGrouping[b]=
	!0,d(this).jqGrid("showCol",c.groupField[b])):(c.visibiltyOnNextGrouping[b]=d("#"+d.jgrid.jqID(this.p.id+"_"+c.groupField[b])).is(":visible"),d(this).jqGrid("hideCol",c.groupField[b]));c.summary=[];c.hideFirstGroupCol&&(c.formatDisplayField[0]=function(a){return a});b=0;for(k=a.length;b<k;b++)c.hideFirstGroupCol&&!a[b].hidden&&c.groupField[0]===a[b].name&&(a[b].formatter=function(){return""}),a[b].summaryType&&(a[b].summaryDivider?c.summary.push({nm:a[b].name,st:a[b].summaryType,v:"",sd:a[b].summaryDivider,
	vd:"",sr:a[b].summaryRound,srt:a[b].summaryRoundType||"round"}):c.summary.push({nm:a[b].name,st:a[b].summaryType,v:"",sr:a[b].summaryRound,srt:a[b].summaryRoundType||"round"}))}else this.p.grouping=!1})},groupingPrepare:function(b,k){this.each(function(){var a=this.p.groupingView,c=this,g,m=a.groupField.length,e,f,h,p=0;for(g=0;g<m;g++)e=a.groupField[g],h=a.displayField[g],f=b[e],h=null==h?null:b[h],null==h&&(h=f),void 0!==f&&(0===k?(a.groups.push({idx:g,dataIndex:e,value:f,displayValue:h,startRow:k,
	cnt:1,summary:[]}),a.lastvalues[g]=f,a.counters[g]={cnt:1,pos:a.groups.length-1,summary:d.extend(!0,[],a.summary)}):"object"===typeof f||(d.isArray(a.isInTheSameGroup)&&d.isFunction(a.isInTheSameGroup[g])?a.isInTheSameGroup[g].call(c,a.lastvalues[g],f,g,a):a.lastvalues[g]===f)?1===p?(a.groups.push({idx:g,dataIndex:e,value:f,displayValue:h,startRow:k,cnt:1,summary:[]}),a.lastvalues[g]=f,a.counters[g]={cnt:1,pos:a.groups.length-1,summary:d.extend(!0,[],a.summary)}):(a.counters[g].cnt+=1,a.groups[a.counters[g].pos].cnt=
	a.counters[g].cnt):(a.groups.push({idx:g,dataIndex:e,value:f,displayValue:h,startRow:k,cnt:1,summary:[]}),a.lastvalues[g]=f,p=1,a.counters[g]={cnt:1,pos:a.groups.length-1,summary:d.extend(!0,[],a.summary)}),d.each(a.counters[g].summary,function(){d.isFunction(this.st)?this.v=this.st.call(c,this.v,this.nm,b):(this.v=d(c).jqGrid("groupingCalculations.handler",this.st,this.v,this.nm,this.sr,this.srt,b),"avg"===this.st.toLowerCase()&&this.sd&&(this.vd=d(c).jqGrid("groupingCalculations.handler",this.st,
	this.vd,this.sd,this.sr,this.srt,b)))}),a.groups[a.counters[g].pos].summary=a.counters[g].summary)});return this},groupingToggle:function(b){this.each(function(){var k=this.p.groupingView,a=b.split("_"),c=parseInt(a[a.length-2],10);a.splice(a.length-2,2);var g=a.join("_"),a=k.minusicon,m=k.plusicon,e=d("#"+d.jgrid.jqID(b)),e=e.length?e[0].nextSibling:null,f=d("#"+d.jgrid.jqID(b)+" span.tree-wrap-"+this.p.direction),h=function(a){a=d.map(a.split(" "),function(a){if(a.substring(0,g.length+1)===g+"_")return parseInt(a.substring(g.length+
	1),10)});return 0<a.length?a[0]:void 0},p,r=!1,q=this.p.frozenColumns?this.p.id+"_frozen":!1,n=q?d("#"+d.jgrid.jqID(b),"#"+d.jgrid.jqID(q)):!1,n=n&&n.length?n[0].nextSibling:null;if(f.hasClass(a)){if(k.showSummaryOnHide){if(e)for(;e&&!(d(e).hasClass("jqfoot")&&parseInt(d(e).attr("jqfootlevel"),10)<=c);)d(e).hide(),e=e.nextSibling,q&&(d(n).hide(),n=n.nextSibling)}else if(e)for(;e;){k=h(e.className);if(void 0!==k&&k<=c)break;d(e).hide();e=e.nextSibling;q&&(d(n).hide(),n=n.nextSibling)}f.removeClass(a).addClass(m);
	r=!0}else{if(e)for(p=void 0;e;){k=h(e.className);void 0===p&&(p=void 0===k);if(void 0!==k){if(k<=c)break;k===c+1&&(d(e).show().find(">td>span.tree-wrap-"+this.p.direction).removeClass(a).addClass(m),q&&d(n).show().find(">td>span.tree-wrap-"+this.p.direction).removeClass(a).addClass(m))}else p&&(d(e).show(),q&&d(n).show());e=e.nextSibling;q&&(n=n.nextSibling)}f.removeClass(m).addClass(a)}d(this).triggerHandler("jqGridGroupingClickGroup",[b,r]);d.isFunction(this.p.onClickGroup)&&this.p.onClickGroup.call(this,
	b,r)});return!1},groupingRender:function(b,k,a,c){return this.each(function(){function g(a,b,c){var d=!1;if(0===b)d=c[a];else{var e=c[a].idx;if(0===e)d=c[a];else for(;0<=a;a--)if(c[a].idx===e-b){d=c[a];break}}return d}function m(a,b,c,f){var h=g(a,b,c),m=e.p.colModel,n,q=h.cnt;a="";var p;for(p=f;p<k;p++){var r="<td "+e.formatCol(p,1,"")+">&#160;</td>",t="{0}";d.each(h.summary,function(){if(this.nm===m[p].name){m[p].summaryTpl&&(t=m[p].summaryTpl);"string"===typeof this.st&&"avg"===this.st.toLowerCase()&&
	(this.sd&&this.vd?this.v/=this.vd:this.v&&0<q&&(this.v/=q));try{this.groupCount=h.cnt,this.groupIndex=h.dataIndex,this.groupValue=h.value,n=e.formatter("",this.v,p,this)}catch(a){n=this.v}r="<td "+e.formatCol(p,1,"")+">"+d.jgrid.format(t,n)+"</td>";return!1}});a+=r}return a}var e=this,f=e.p.groupingView,h="",p="",r,q,n=f.groupCollapse?f.plusicon:f.minusicon,t,y=[],z=f.groupField.length,n=n+(" tree-wrap-"+e.p.direction);d.each(e.p.colModel,function(a,b){var c;for(c=0;c<z;c++)if(f.groupField[c]===b.name){y[c]=
	a;break}});var x=0,A=d.makeArray(f.groupSummary);A.reverse();d.each(f.groups,function(g,l){if(f._locgr&&!(l.startRow+l.cnt>(a-1)*c&&l.startRow<a*c))return!0;x++;q=e.p.id+"ghead_"+l.idx;r=q+"_"+g;p="<span style='cursor:pointer;' class='ui-icon "+n+"' onclick=\"jQuery('#"+d.jgrid.jqID(e.p.id)+"').jqGrid('groupingToggle','"+r+"');return false;\"></span>";try{d.isArray(f.formatDisplayField)&&d.isFunction(f.formatDisplayField[l.idx])?(l.displayValue=f.formatDisplayField[l.idx].call(e,l.displayValue,l.value,
	e.p.colModel[y[l.idx]],l.idx,f),t=l.displayValue):t=e.formatter(r,l.displayValue,y[l.idx],l.value)}catch(C){t=l.displayValue}"header"===f.groupSummaryPos[l.idx]?(h+='<tr id="'+r+'"'+(f.groupCollapse&&0<l.idx?' style="display:none;" ':" ")+'role="row" class= "ui-widget-content jqgroup ui-row-'+e.p.direction+" "+q+'"><td style="padding-left:'+12*l.idx+'px;">'+p+d.jgrid.template(f.groupText[l.idx],t,l.cnt,l.summary)+"</td>",h+=m(g,l.idx-1,f.groups,1),h+="</tr>"):h+='<tr id="'+r+'"'+(f.groupCollapse&&
	0<l.idx?' style="display:none;" ':" ")+'role="row" class= "ui-widget-content jqgroup ui-row-'+e.p.direction+" "+q+'"><td style="padding-left:'+12*l.idx+'px;" colspan="'+k+'">'+p+d.jgrid.template(f.groupText[l.idx],t,l.cnt,l.summary)+"</td></tr>";if(z-1===l.idx){var s=f.groups[g+1],v,u=0;v=l.startRow;var B=void 0!==s?f.groups[g+1].startRow:b.length;f._locgr&&(u=(a-1)*c,u>l.startRow&&(v=u));for(;v<B&&b[v-u];v++)h+=b[v-u].join("");if("header"!==f.groupSummaryPos[l.idx]){var w;if(void 0!==s){for(w=0;w<
	f.groupField.length&&s.dataIndex!==f.groupField[w];w++);x=f.groupField.length-w}for(s=0;s<x;s++)A[s]&&(u="",f.groupCollapse&&!f.showSummaryOnHide&&(u=' style="display:none;"'),h+="<tr"+u+' jqfootlevel="'+(l.idx-s)+'" role="row" class="ui-widget-content jqfoot ui-row-'+e.p.direction+'">',h+=m(g,s,f.groups,0),h+="</tr>");x=w}}});d("#"+d.jgrid.jqID(e.p.id)+" tbody:first").append(h);h=null})},groupingGroupBy:function(b,k){return this.each(function(){"string"===typeof b&&(b=[b]);var a=this.p.groupingView;
	this.p.grouping=!0;void 0===a.visibiltyOnNextGrouping&&(a.visibiltyOnNextGrouping=[]);var c;for(c=0;c<a.groupField.length;c++)!a.groupColumnShow[c]&&a.visibiltyOnNextGrouping[c]&&d(this).jqGrid("showCol",a.groupField[c]);for(c=0;c<b.length;c++)a.visibiltyOnNextGrouping[c]=d("#"+d.jgrid.jqID(this.p.id)+"_"+d.jgrid.jqID(b[c])).is(":visible");this.p.groupingView=d.extend(this.p.groupingView,k||{});a.groupField=b;d(this).trigger("reloadGrid")})},groupingRemove:function(b){return this.each(function(){void 0===
	b&&(b=!0);this.p.grouping=!1;if(!0===b){var k=this.p.groupingView,a;for(a=0;a<k.groupField.length;a++)!k.groupColumnShow[a]&&k.visibiltyOnNextGrouping[a]&&d(this).jqGrid("showCol",k.groupField);d("tr.jqgroup, tr.jqfoot","#"+d.jgrid.jqID(this.p.id)+" tbody:first").remove();d("tr.jqgrow:hidden","#"+d.jgrid.jqID(this.p.id)+" tbody:first").show()}else d(this).trigger("reloadGrid")})},groupingCalculations:{handler:function(b,d,a,c,g,m){var e={sum:function(){return parseFloat(d||0)+parseFloat(m[a]||0)},
	min:function(){return""===d?parseFloat(m[a]||0):Math.min(parseFloat(d),parseFloat(m[a]||0))},max:function(){return""===d?parseFloat(m[a]||0):Math.max(parseFloat(d),parseFloat(m[a]||0))},count:function(){""===d&&(d=0);return m.hasOwnProperty(a)?d+1:0},avg:function(){return e.sum()}};if(!e[b])throw"jqGrid Grouping No such method: "+b;b=e[b]();null!=c&&("fixed"===g?b=b.toFixed(c):(c=Math.pow(10,c),b=Math.round(b*c)/c));return b}}})})(jQuery);
	(function(d){d.jgrid.extend({setTreeNode:function(b,c){return this.each(function(){var a=this;if(a.grid&&a.p.treeGrid)for(var h=a.p.expColInd,e=a.p.treeReader.expanded_field,k=a.p.treeReader.leaf_field,g=a.p.treeReader.level_field,f=a.p.treeReader.icon_field,n=a.p.treeReader.loaded,m,p,q,l;b<c;)l=d.jgrid.stripPref(a.p.idPrefix,a.rows[b].id),l=a.p.data[a.p._index[l]],"nested"!==a.p.treeGridModel||l[k]||(m=parseInt(l[a.p.treeReader.left_field],10),p=parseInt(l[a.p.treeReader.right_field],10),l[k]=p===
	m+1?"true":"false",a.rows[b].cells[a.p._treeleafpos].innerHTML=l[k]),m=parseInt(l[g],10),0===a.p.tree_root_level?(q=m+1,p=m):(q=m,p=m-1),q="<div class='tree-wrap tree-wrap-"+a.p.direction+"' style='width:"+18*q+"px;'>",q+="<div style='"+("rtl"===a.p.direction?"right:":"left:")+18*p+"px;' class='ui-icon ",void 0!==l[n]&&(l[n]="true"===l[n]||!0===l[n]?!0:!1),"true"===l[k]||!0===l[k]?(q+=(void 0!==l[f]&&""!==l[f]?l[f]:a.p.treeIcons.leaf)+" tree-leaf treeclick",l[k]=!0,p="leaf"):(l[k]=!1,p=""),l[e]=("true"===
	l[e]||!0===l[e]?!0:!1)&&(l[n]||void 0===l[n]),q=!1===l[e]?q+(!0===l[k]?"'":a.p.treeIcons.plus+" tree-plus treeclick'"):q+(!0===l[k]?"'":a.p.treeIcons.minus+" tree-minus treeclick'"),q+="></div></div>",d(a.rows[b].cells[h]).wrapInner("<span class='cell-wrapper"+p+"'></span>").prepend(q),m!==parseInt(a.p.tree_root_level,10)&&((l=(l=d(a).jqGrid("getNodeParent",l))&&l.hasOwnProperty(e)?l[e]:!0)||d(a.rows[b]).css("display","none")),d(a.rows[b].cells[h]).find("div.treeclick").bind("click",function(b){b=
	d.jgrid.stripPref(a.p.idPrefix,d(b.target||b.srcElement,a.rows).closest("tr.jqgrow")[0].id);b=a.p._index[b];a.p.data[b][k]||(a.p.data[b][e]?(d(a).jqGrid("collapseRow",a.p.data[b]),d(a).jqGrid("collapseNode",a.p.data[b])):(d(a).jqGrid("expandRow",a.p.data[b]),d(a).jqGrid("expandNode",a.p.data[b])));return!1}),!0===a.p.ExpandColClick&&d(a.rows[b].cells[h]).find("span.cell-wrapper").css("cursor","pointer").bind("click",function(b){b=d.jgrid.stripPref(a.p.idPrefix,d(b.target||b.srcElement,a.rows).closest("tr.jqgrow")[0].id);
	var c=a.p._index[b];a.p.data[c][k]||(a.p.data[c][e]?(d(a).jqGrid("collapseRow",a.p.data[c]),d(a).jqGrid("collapseNode",a.p.data[c])):(d(a).jqGrid("expandRow",a.p.data[c]),d(a).jqGrid("expandNode",a.p.data[c])));d(a).jqGrid("setSelection",b);return!1}),b++})},setTreeGrid:function(){return this.each(function(){var b=this,c=0,a,h=!1,e,k,g=[];if(b.p.treeGrid){b.p.treedatatype||d.extend(b.p,{treedatatype:b.p.datatype});b.p.subGrid=!1;b.p.altRows=!1;b.p.pgbuttons=!1;b.p.pginput=!1;b.p.gridview=!0;null===
	b.p.rowTotal&&(b.p.rowNum=1E4);b.p.multiselect=!1;b.p.rowList=[];b.p.expColInd=0;a="ui-icon-triangle-1-"+("rtl"===b.p.direction?"w":"e");b.p.treeIcons=d.extend({plus:a,minus:"ui-icon-triangle-1-s",leaf:"ui-icon-radio-off"},b.p.treeIcons||{});"nested"===b.p.treeGridModel?b.p.treeReader=d.extend({level_field:"level",left_field:"lft",right_field:"rgt",leaf_field:"isLeaf",expanded_field:"expanded",loaded:"loaded",icon_field:"icon"},b.p.treeReader):"adjacency"===b.p.treeGridModel&&(b.p.treeReader=d.extend({level_field:"level",
	parent_id_field:"parent",leaf_field:"isLeaf",expanded_field:"expanded",loaded:"loaded",icon_field:"icon"},b.p.treeReader));for(e in b.p.colModel)if(b.p.colModel.hasOwnProperty(e))for(k in a=b.p.colModel[e].name,a!==b.p.ExpandColumn||h||(h=!0,b.p.expColInd=c),c++,b.p.treeReader)b.p.treeReader.hasOwnProperty(k)&&b.p.treeReader[k]===a&&g.push(a);d.each(b.p.treeReader,function(a,e){e&&-1===d.inArray(e,g)&&("leaf_field"===a&&(b.p._treeleafpos=c),c++,b.p.colNames.push(e),b.p.colModel.push({name:e,width:1,
	hidden:!0,sortable:!1,resizable:!1,hidedlg:!0,editable:!0,search:!1}))})}})},expandRow:function(b){this.each(function(){var c=this;if(c.grid&&c.p.treeGrid){var a=d(c).jqGrid("getNodeChildren",b),h=c.p.treeReader.expanded_field;d(a).each(function(){var a=c.p.idPrefix+d.jgrid.getAccessor(this,c.p.localReader.id);d(d(c).jqGrid("getGridRowById",a)).css("display","");this[h]&&d(c).jqGrid("expandRow",this)})}})},collapseRow:function(b){this.each(function(){var c=this;if(c.grid&&c.p.treeGrid){var a=d(c).jqGrid("getNodeChildren",
	b),h=c.p.treeReader.expanded_field;d(a).each(function(){var a=c.p.idPrefix+d.jgrid.getAccessor(this,c.p.localReader.id);d(d(c).jqGrid("getGridRowById",a)).css("display","none");this[h]&&d(c).jqGrid("collapseRow",this)})}})},getRootNodes:function(){var b=[];this.each(function(){var c=this;if(c.grid&&c.p.treeGrid)switch(c.p.treeGridModel){case "nested":var a=c.p.treeReader.level_field;d(c.p.data).each(function(){parseInt(this[a],10)===parseInt(c.p.tree_root_level,10)&&b.push(this)});break;case "adjacency":var h=
	c.p.treeReader.parent_id_field;d(c.p.data).each(function(){null!==this[h]&&"null"!==String(this[h]).toLowerCase()||b.push(this)})}});return b},getNodeDepth:function(b){var c=null;this.each(function(){if(this.grid&&this.p.treeGrid)switch(this.p.treeGridModel){case "nested":c=parseInt(b[this.p.treeReader.level_field],10)-parseInt(this.p.tree_root_level,10);break;case "adjacency":c=d(this).jqGrid("getNodeAncestors",b).length}});return c},getNodeParent:function(b){var c=null;this.each(function(){var a=
	this;if(a.grid&&a.p.treeGrid)switch(a.p.treeGridModel){case "nested":var h=a.p.treeReader.left_field,e=a.p.treeReader.right_field,k=a.p.treeReader.level_field,g=parseInt(b[h],10),f=parseInt(b[e],10),n=parseInt(b[k],10);d(this.p.data).each(function(){if(parseInt(this[k],10)===n-1&&parseInt(this[h],10)<g&&parseInt(this[e],10)>f)return c=this,!1});break;case "adjacency":var m=a.p.treeReader.parent_id_field,p=a.p.localReader.id;d(this.p.data).each(function(){if(this[p]===d.jgrid.stripPref(a.p.idPrefix,
	b[m]))return c=this,!1})}});return c},getNodeChildren:function(b){var c=[];this.each(function(){var a=this;if(a.grid&&a.p.treeGrid)switch(a.p.treeGridModel){case "nested":var h=a.p.treeReader.left_field,e=a.p.treeReader.right_field,k=a.p.treeReader.level_field,g=parseInt(b[h],10),f=parseInt(b[e],10),n=parseInt(b[k],10);d(this.p.data).each(function(){parseInt(this[k],10)===n+1&&parseInt(this[h],10)>g&&parseInt(this[e],10)<f&&c.push(this)});break;case "adjacency":var m=a.p.treeReader.parent_id_field,
	p=a.p.localReader.id;d(this.p.data).each(function(){this[m]==d.jgrid.stripPref(a.p.idPrefix,b[p])&&c.push(this)})}});return c},getFullTreeNode:function(b){var c=[];this.each(function(){var a=this,h;if(a.grid&&a.p.treeGrid)switch(a.p.treeGridModel){case "nested":var e=a.p.treeReader.left_field,k=a.p.treeReader.right_field,g=a.p.treeReader.level_field,f=parseInt(b[e],10),n=parseInt(b[k],10),m=parseInt(b[g],10);d(this.p.data).each(function(){parseInt(this[g],10)>=m&&parseInt(this[e],10)>=f&&parseInt(this[e],
	10)<=n&&c.push(this)});break;case "adjacency":if(b){c.push(b);var p=a.p.treeReader.parent_id_field,q=a.p.localReader.id;d(this.p.data).each(function(b){h=c.length;for(b=0;b<h;b++)if(d.jgrid.stripPref(a.p.idPrefix,c[b][q])===this[p]){c.push(this);break}})}}});return c},getNodeAncestors:function(b){var c=[];this.each(function(){if(this.grid&&this.p.treeGrid)for(var a=d(this).jqGrid("getNodeParent",b);a;)c.push(a),a=d(this).jqGrid("getNodeParent",a)});return c},isVisibleNode:function(b){var c=!0;this.each(function(){if(this.grid&&
	this.p.treeGrid){var a=d(this).jqGrid("getNodeAncestors",b),h=this.p.treeReader.expanded_field;d(a).each(function(){c=c&&this[h];if(!c)return!1})}});return c},isNodeLoaded:function(b){var c;this.each(function(){if(this.grid&&this.p.treeGrid){var a=this.p.treeReader.leaf_field,h=this.p.treeReader.loaded;c=void 0!==b?void 0!==b[h]?b[h]:b[a]||0<d(this).jqGrid("getNodeChildren",b).length?!0:!1:!1}});return c},expandNode:function(b){return this.each(function(){if(this.grid&&this.p.treeGrid){var c=this.p.treeReader.expanded_field,
	a=this.p.treeReader.parent_id_field,h=this.p.treeReader.loaded,e=this.p.treeReader.level_field,k=this.p.treeReader.left_field,g=this.p.treeReader.right_field;if(!b[c]){var f=d.jgrid.getAccessor(b,this.p.localReader.id),n=d("#"+this.p.idPrefix+d.jgrid.jqID(f),this.grid.bDiv)[0],m=this.p._index[f];d(this).jqGrid("isNodeLoaded",this.p.data[m])?(b[c]=!0,d("div.treeclick",n).removeClass(this.p.treeIcons.plus+" tree-plus").addClass(this.p.treeIcons.minus+" tree-minus")):this.grid.hDiv.loading||(b[c]=!0,
	d("div.treeclick",n).removeClass(this.p.treeIcons.plus+" tree-plus").addClass(this.p.treeIcons.minus+" tree-minus"),this.p.treeANode=n.rowIndex,this.p.datatype=this.p.treedatatype,"nested"===this.p.treeGridModel?d(this).jqGrid("setGridParam",{postData:{nodeid:f,n_left:b[k],n_right:b[g],n_level:b[e]}}):d(this).jqGrid("setGridParam",{postData:{nodeid:f,parentid:b[a],n_level:b[e]}}),d(this).trigger("reloadGrid"),b[h]=!0,"nested"===this.p.treeGridModel?d(this).jqGrid("setGridParam",{postData:{nodeid:"",
	n_left:"",n_right:"",n_level:""}}):d(this).jqGrid("setGridParam",{postData:{nodeid:"",parentid:"",n_level:""}}))}}})},collapseNode:function(b){return this.each(function(){if(this.grid&&this.p.treeGrid){var c=this.p.treeReader.expanded_field;b[c]&&(b[c]=!1,c=d.jgrid.getAccessor(b,this.p.localReader.id),c=d("#"+this.p.idPrefix+d.jgrid.jqID(c),this.grid.bDiv)[0],d("div.treeclick",c).removeClass(this.p.treeIcons.minus+" tree-minus").addClass(this.p.treeIcons.plus+" tree-plus"))}})},SortTree:function(b,
	c,a,h){return this.each(function(){if(this.grid&&this.p.treeGrid){var e,k,g,f=[],n=this,m;e=d(this).jqGrid("getRootNodes");e=d.jgrid.from(e);e.orderBy(b,c,a,h);m=e.select();e=0;for(k=m.length;e<k;e++)g=m[e],f.push(g),d(this).jqGrid("collectChildrenSortTree",f,g,b,c,a,h);d.each(f,function(a){var b=d.jgrid.getAccessor(this,n.p.localReader.id);d("#"+d.jgrid.jqID(n.p.id)+" tbody tr:eq("+a+")").after(d("tr#"+d.jgrid.jqID(b),n.grid.bDiv))});f=m=e=null}})},collectChildrenSortTree:function(b,c,a,h,e,k){return this.each(function(){if(this.grid&&
	this.p.treeGrid){var g,f,n,m;g=d(this).jqGrid("getNodeChildren",c);g=d.jgrid.from(g);g.orderBy(a,h,e,k);m=g.select();g=0;for(f=m.length;g<f;g++)n=m[g],b.push(n),d(this).jqGrid("collectChildrenSortTree",b,n,a,h,e,k)}})},setTreeRow:function(b,c){var a=!1;this.each(function(){this.grid&&this.p.treeGrid&&(a=d(this).jqGrid("setRowData",b,c))});return a},delTreeNode:function(b){return this.each(function(){var c=this.p.localReader.id,a,h=this.p.treeReader.left_field,e=this.p.treeReader.right_field,k,g,f;
	if(this.grid&&this.p.treeGrid&&(a=this.p._index[b],void 0!==a)){k=parseInt(this.p.data[a][e],10);g=k-parseInt(this.p.data[a][h],10)+1;var n=d(this).jqGrid("getFullTreeNode",this.p.data[a]);if(0<n.length)for(a=0;a<n.length;a++)d(this).jqGrid("delRowData",n[a][c]);if("nested"===this.p.treeGridModel){c=d.jgrid.from(this.p.data).greater(h,k,{stype:"integer"}).select();if(c.length)for(f in c)c.hasOwnProperty(f)&&(c[f][h]=parseInt(c[f][h],10)-g);c=d.jgrid.from(this.p.data).greater(e,k,{stype:"integer"}).select();
	if(c.length)for(f in c)c.hasOwnProperty(f)&&(c[f][e]=parseInt(c[f][e],10)-g)}}})},addChildNode:function(b,c,a,h){var e=this[0];if(a){var k=e.p.treeReader.expanded_field,g=e.p.treeReader.leaf_field,f=e.p.treeReader.level_field,n=e.p.treeReader.parent_id_field,m=e.p.treeReader.left_field,p=e.p.treeReader.right_field,q=e.p.treeReader.loaded,l,u,t,w,s;l=0;var v=c,x;void 0===h&&(h=!1);if(void 0===b||null===b){s=e.p.data.length-1;if(0<=s)for(;0<=s;)l=Math.max(l,parseInt(e.p.data[s][e.p.localReader.id],
	10)),s--;b=l+1}var y=d(e).jqGrid("getInd",c);x=!1;void 0===c||null===c||""===c?(v=c=null,l="last",w=e.p.tree_root_level,s=e.p.data.length+1):(l="after",u=e.p._index[c],t=e.p.data[u],c=t[e.p.localReader.id],w=parseInt(t[f],10)+1,s=d(e).jqGrid("getFullTreeNode",t),s.length?(v=s=s[s.length-1][e.p.localReader.id],s=d(e).jqGrid("getInd",v)+1):s=d(e).jqGrid("getInd",c)+1,t[g]&&(x=!0,t[k]=!0,d(e.rows[y]).find("span.cell-wrapperleaf").removeClass("cell-wrapperleaf").addClass("cell-wrapper").end().find("div.tree-leaf").removeClass(e.p.treeIcons.leaf+
	" tree-leaf").addClass(e.p.treeIcons.minus+" tree-minus"),e.p.data[u][g]=!1,t[q]=!0));u=s+1;void 0===a[k]&&(a[k]=!1);void 0===a[q]&&(a[q]=!1);a[f]=w;void 0===a[g]&&(a[g]=!0);"adjacency"===e.p.treeGridModel&&(a[n]=c);if("nested"===e.p.treeGridModel){var r;if(null!==c){g=parseInt(t[p],10);f=d.jgrid.from(e.p.data);f=f.greaterOrEquals(p,g,{stype:"integer"});f=f.select();if(f.length)for(r in f)f.hasOwnProperty(r)&&(f[r][m]=f[r][m]>g?parseInt(f[r][m],10)+2:f[r][m],f[r][p]=f[r][p]>=g?parseInt(f[r][p],10)+
	2:f[r][p]);a[m]=g;a[p]=g+1}else{g=parseInt(d(e).jqGrid("getCol",p,!1,"max"),10);f=d.jgrid.from(e.p.data).greater(m,g,{stype:"integer"}).select();if(f.length)for(r in f)f.hasOwnProperty(r)&&(f[r][m]=parseInt(f[r][m],10)+2);f=d.jgrid.from(e.p.data).greater(p,g,{stype:"integer"}).select();if(f.length)for(r in f)f.hasOwnProperty(r)&&(f[r][p]=parseInt(f[r][p],10)+2);a[m]=g+1;a[p]=g+2}}if(null===c||d(e).jqGrid("isNodeLoaded",t)||x)d(e).jqGrid("addRowData",b,a,l,v),d(e).jqGrid("setTreeNode",s,u);t&&!t[k]&&
	h&&d(e.rows[y]).find("div.treeclick").click()}}})})(jQuery);
	(function(d){function I(d,n){var h,e,v=[],r;if(!this||"function"!==typeof d||d instanceof RegExp)throw new TypeError;r=this.length;for(h=0;h<r;h++)if(this.hasOwnProperty(h)&&(e=this[h],d.call(n,e,h,this))){v.push(e);break}return v}d.assocArraySize=function(d){var n=0,h;for(h in d)d.hasOwnProperty(h)&&n++;return n};d.jgrid.extend({pivotSetup:function(q,n){var h=[],e=[],v=[],r=[],b={grouping:!0,groupingView:{groupField:[],groupSummary:[],groupSummaryPos:[]}},f=[],c=d.extend({rowTotals:!1,rowTotalsText:"Total",
	colTotals:!1,groupSummary:!0,groupSummaryPos:"header",frozenStaticCols:!1},n||{});this.each(function(){function n(C,c,a){C=I.call(C,c,a);return 0<C.length?C[0]:null}function J(c,a){var d=0,f=!0,h;for(h in c){if(c[h]!=this[d]){f=!1;break}d++;if(d>=this.length)break}f&&(D=a);return f}function E(c,a,f,h){var g=a.length,b,k,e,l;l=d.isArray(f)?f.length:1;r=[];for(e=r.root=0;e<l;e++){var n=[],m;for(b=0;b<g;b++){if(null==f)m=k=d.trim(a[b].member)+"_"+a[b].aggregator;else{m=f[e].replace(/\s+/g,"");try{k=
	1===g?m:m+"_"+a[b].aggregator+"_"+b}catch(v){}}var t=h,u=k,x=n,y=k,w=h[k],p=a[b].member,q=c,s=void 0;switch(a[b].aggregator){case "sum":s=parseFloat(w||0)+parseFloat(q[p]||0);break;case "count":if(""===w||null==w)w=0;s=q.hasOwnProperty(p)?w+1:0;break;case "min":s=""===w||null==w?parseFloat(q[p]||0):Math.min(parseFloat(w),parseFloat(q[p]||0));break;case "max":s=""===w||null==w?parseFloat(q[p]||0):Math.max(parseFloat(w),parseFloat(q[p]||0))}t[u]=x[y]=s}r[m]=n}return h}function H(a){var d,b,g,k,e;for(g in a)if(a.hasOwnProperty(g)){if("object"!==
	typeof a[g]&&("level"===g&&(void 0===F[a.level]&&(F[a.level]="",0<a.level&&"_r_Totals"!==a.text&&(f[a.level-1]={useColSpanStyle:!1,groupHeaders:[]})),F[a.level]!==a.text&&a.children.length&&"_r_Totals"!==a.text&&0<a.level&&(f[a.level-1].groupHeaders.push({titleText:a.text}),b=f[a.level-1].groupHeaders.length,e=1===b?K:G+(b-1)*z,f[a.level-1].groupHeaders[b-1].startColumnName=h[e].name,f[a.level-1].groupHeaders[b-1].numberOfColumns=h.length-e,G=h.length),F[a.level]=a.text),a.level===l&&"level"===g&&
	0<l))if(1<z){b=1;for(d in a.fields)1===b&&f[l-1].groupHeaders.push({startColumnName:d,numberOfColumns:1,titleText:a.text}),b++;f[l-1].groupHeaders[f[l-1].groupHeaders.length-1].numberOfColumns=b-1}else f.splice(l-1,1);null!=a[g]&&"object"===typeof a[g]&&H(a[g]);if("level"===g&&0<a.level)for(d in b=0,a.fields){e={};for(k in c.aggregates[b])if(c.aggregates[b].hasOwnProperty(k))switch(k){case "member":case "label":case "aggregator":break;default:e[k]=c.aggregates[b][k]}1<z?(e.name=d,e.label=c.aggregates[b].label||
	d):(e.name=a.text,e.label="_r_Totals"===a.text?c.rowTotalsText:a.text);h.push(e);b++}}}var m,D,a,y=q.length,s,l,z,k,p=0;c.rowTotals&&0<c.yDimension.length&&(c.yDimension.splice(0,0,{dataName:c.yDimension[0].dataName}),c.yDimension[0].converter=function(){return"_r_Totals"});s=d.isArray(c.xDimension)?c.xDimension.length:0;l=c.yDimension.length;z=d.isArray(c.aggregates)?c.aggregates.length:0;if(0===s||0===z)throw"xDimension or aggregates optiona are not set!";var x;for(a=0;a<s;a++)x={name:c.xDimension[a].dataName,
	frozen:c.frozenStaticCols},x=d.extend(!0,x,c.xDimension[a]),h.push(x);x=s-1;for(var A={};p<y;){m=q[p];var t=[],u=[];k={};a=0;do t[a]=d.trim(m[c.xDimension[a].dataName]),k[c.xDimension[a].dataName]=t[a],a++;while(a<s);var g=0;D=-1;a=n(e,J,t);if(!a){g=0;if(1<=l){for(g=0;g<l;g++)u[g]=d.trim(m[c.yDimension[g].dataName]),c.yDimension[g].converter&&d.isFunction(c.yDimension[g].converter)&&(u[g]=c.yDimension[g].converter.call(this,u[g],t,u));k=E(m,c.aggregates,u,k)}else 0===l&&(k=E(m,c.aggregates,null,k));
	e.push(k)}else if(0<=D){g=0;if(1<=l){for(g=0;g<l;g++)u[g]=d.trim(m[c.yDimension[g].dataName]),c.yDimension[g].converter&&d.isFunction(c.yDimension[g].converter)&&(u[g]=c.yDimension[g].converter.call(this,u[g],t,u));a=E(m,c.aggregates,u,a)}else 0===l&&(a=E(m,c.aggregates,null,a));e[D]=a}m=0;var t=k=null,B;for(B in r){if(0===m)A.children&&void 0!==A.children||(A={text:B,level:0,children:[]}),k=A.children;else{t=null;for(a=0;a<k.length;a++)if(k[a].text===B){t=k[a];break}t?k=t.children:(k.push({children:[],
	text:B,level:m,fields:r[B]}),k=k[k.length-1].children)}m++}p++}var F=[],G=h.length,K=G;0<l&&(f[l-1]={useColSpanStyle:!1,groupHeaders:[]});H(A,0);if(c.colTotals)for(p=e.length;p--;)for(a=s;a<h.length;a++)y=h[a].name,v[y]=v[y]?v[y]+parseFloat(e[p][y]||0):parseFloat(e[p][y]||0);if(0<x)for(a=0;a<x;a++)b.groupingView.groupField[a]=h[a].name,b.groupingView.groupSummary[a]=c.groupSummary,b.groupingView.groupSummaryPos[a]=c.groupSummaryPos;else b.grouping=!1;b.sortname=h[x].name;b.groupingView.hideFirstGroupCol=
	!0});return{colModel:h,rows:e,groupOptions:b,groupHeaders:f,summary:v}},jqPivot:function(q,n,h,e){return this.each(function(){function v(b){var f=jQuery(r).jqGrid("pivotSetup",b,n),c=0<d.assocArraySize(f.summary)?!0:!1,e=d.jgrid.from(f.rows);for(b=0;b<f.groupOptions.groupingView.groupField.length;b++)e.orderBy(f.groupOptions.groupingView.groupField[b],"a","text","");jQuery(r).jqGrid(d.extend({datastr:d.extend(e.select(),c?{userdata:f.summary}:{}),datatype:"jsonstring",footerrow:c,userDataOnFooter:c,
	colModel:f.colModel,viewrecords:!0,sortname:n.xDimension[0].dataName},h||{},f.groupOptions));f=f.groupHeaders;if(f.length)for(b=0;b<f.length;b++)f[b]&&f[b].groupHeaders.length&&jQuery(r).jqGrid("setGroupHeaders",f[b]);n.frozenStaticCols&&jQuery(r).jqGrid("setFrozenColumns")}var r=this;"string"===typeof q?d.ajax(d.extend({url:q,dataType:"json",success:function(b){v(d.jgrid.getAccessor(b,e&&e.reader?e.reader:"rows"))}},e||{})):v(q)})}})})(jQuery);
	var xmlJsonClass={xml2json:function(a,b){9===a.nodeType&&(a=a.documentElement);var g=this.removeWhite(a),g=this.toObj(g),g=this.toJson(g,a.nodeName,"\t");return"{\n"+b+(b?g.replace(/\t/g,b):g.replace(/\t|\n/g,""))+"\n}"},json2xml:function(a,b){var g=function(a,b,e){var d="",f,k;if(a instanceof Array)if(0===a.length)d+=e+"<"+b+">__EMPTY_ARRAY_</"+b+">\n";else for(f=0,k=a.length;f<k;f+=1)var n=e+g(a[f],b,e+"\t")+"\n",d=d+n;else if("object"===typeof a){f=!1;d+=e+"<"+b;for(k in a)a.hasOwnProperty(k)&&
	("@"===k.charAt(0)?d+=" "+k.substr(1)+'="'+a[k].toString()+'"':f=!0);d+=f?">":"/>";if(f){for(k in a)a.hasOwnProperty(k)&&("#text"===k?d+=a[k]:"#cdata"===k?d+="<![CDATA["+a[k]+"]]\x3e":"@"!==k.charAt(0)&&(d+=g(a[k],k,e+"\t")));d+=("\n"===d.charAt(d.length-1)?e:"")+"</"+b+">"}}else"function"===typeof a?d+=e+"<"+b+"><![CDATA["+a+"]]\x3e</"+b+">":(void 0===a&&(a=""),d='""'===a.toString()||0===a.toString().length?d+(e+"<"+b+">__EMPTY_STRING_</"+b+">"):d+(e+"<"+b+">"+a.toString()+"</"+b+">"));return d},
	f="",e;for(e in a)a.hasOwnProperty(e)&&(f+=g(a[e],e,""));return b?f.replace(/\t/g,b):f.replace(/\t|\n/g,"")},toObj:function(a){var b={},g=/function/i;if(1===a.nodeType){if(a.attributes.length){var f;for(f=0;f<a.attributes.length;f+=1)b["@"+a.attributes[f].nodeName]=(a.attributes[f].nodeValue||"").toString()}if(a.firstChild){var e=f=0,h=!1,c;for(c=a.firstChild;c;c=c.nextSibling)1===c.nodeType?h=!0:3===c.nodeType&&c.nodeValue.match(/[^ \f\n\r\t\v]/)?f+=1:4===c.nodeType&&(e+=1);if(h)if(2>f&&2>e)for(this.removeWhite(a),
	c=a.firstChild;c;c=c.nextSibling)3===c.nodeType?b["#text"]=this.escape(c.nodeValue):4===c.nodeType?g.test(c.nodeValue)?b[c.nodeName]=[b[c.nodeName],c.nodeValue]:b["#cdata"]=this.escape(c.nodeValue):b[c.nodeName]?b[c.nodeName]instanceof Array?b[c.nodeName][b[c.nodeName].length]=this.toObj(c):b[c.nodeName]=[b[c.nodeName],this.toObj(c)]:b[c.nodeName]=this.toObj(c);else a.attributes.length?b["#text"]=this.escape(this.innerXml(a)):b=this.escape(this.innerXml(a));else if(f)a.attributes.length?b["#text"]=
	this.escape(this.innerXml(a)):(b=this.escape(this.innerXml(a)),"__EMPTY_ARRAY_"===b?b="[]":"__EMPTY_STRING_"===b&&(b=""));else if(e)if(1<e)b=this.escape(this.innerXml(a));else for(c=a.firstChild;c;c=c.nextSibling)if(g.test(a.firstChild.nodeValue)){b=a.firstChild.nodeValue;break}else b["#cdata"]=this.escape(c.nodeValue)}a.attributes.length||a.firstChild||(b=null)}else 9===a.nodeType?b=this.toObj(a.documentElement):alert("unhandled node type: "+a.nodeType);return b},toJson:function(a,b,g,f){void 0===
	f&&(f=!0);var e=b?'"'+b+'"':"",h="\t",c="\n";f||(c=h="");if("[]"===a)e+=b?":[]":"[]";else if(a instanceof Array){var l,d,m=[];d=0;for(l=a.length;d<l;d+=1)m[d]=this.toJson(a[d],"",g+h,f);e+=(b?":[":"[")+(1<m.length?c+g+h+m.join(","+c+g+h)+c+g:m.join(""))+"]"}else if(null===a)e+=(b&&":")+"null";else if("object"===typeof a){l=[];for(d in a)a.hasOwnProperty(d)&&(l[l.length]=this.toJson(a[d],d,g+h,f));e+=(b?":{":"{")+(1<l.length?c+g+h+l.join(","+c+g+h)+c+g:l.join(""))+"}"}else e="string"===typeof a?e+
	((b&&":")+'"'+a.replace(/\\/g,"\\\\").replace(/\"/g,'\\"')+'"'):e+((b&&":")+a.toString());return e},innerXml:function(a){var b="";if("innerHTML"in a)b=a.innerHTML;else{var g=function(a){var b="",h;if(1===a.nodeType){b+="<"+a.nodeName;for(h=0;h<a.attributes.length;h+=1)b+=" "+a.attributes[h].nodeName+'="'+(a.attributes[h].nodeValue||"").toString()+'"';if(a.firstChild){b+=">";for(h=a.firstChild;h;h=h.nextSibling)b+=g(h);b+="</"+a.nodeName+">"}else b+="/>"}else 3===a.nodeType?b+=a.nodeValue:4===a.nodeType&&
	(b+="<![CDATA["+a.nodeValue+"]]\x3e");return b};for(a=a.firstChild;a;a=a.nextSibling)b+=g(a)}return b},escape:function(a){return a.replace(/[\\]/g,"\\\\").replace(/[\"]/g,'\\"').replace(/[\n]/g,"\\n").replace(/[\r]/g,"\\r")},removeWhite:function(a){a.normalize();var b;for(b=a.firstChild;b;)if(3===b.nodeType)if(b.nodeValue.match(/[^ \f\n\r\t\v]/))b=b.nextSibling;else{var g=b.nextSibling;a.removeChild(b);b=g}else 1===b.nodeType&&this.removeWhite(b),b=b.nextSibling;return a}};
	function tableToGrid(l,m){jQuery(l).each(function(){if(!this.grid){jQuery(this).width("99%");var b=jQuery(this).width(),c=jQuery("tr td:first-child input[type=checkbox]:first",jQuery(this)),a=jQuery("tr td:first-child input[type=radio]:first",jQuery(this)),c=0<c.length,a=!c&&0<a.length,k=c||a,d=[],e=[];jQuery("th",jQuery(this)).each(function(){0===d.length&&k?(d.push({name:"__selection__",index:"__selection__",width:0,hidden:!0}),e.push("__selection__")):(d.push({name:jQuery(this).attr("id")||jQuery.trim(jQuery.jgrid.stripHtml(jQuery(this).html())).split(" ").join("_"),
	index:jQuery(this).attr("id")||jQuery.trim(jQuery.jgrid.stripHtml(jQuery(this).html())).split(" ").join("_"),width:jQuery(this).width()||150}),e.push(jQuery(this).html()))});var f=[],g=[],h=[];jQuery("tbody > tr",jQuery(this)).each(function(){var b={},a=0;jQuery("td",jQuery(this)).each(function(){if(0===a&&k){var c=jQuery("input",jQuery(this)),e=c.attr("value");g.push(e||f.length);c.is(":checked")&&h.push(e);b[d[a].name]=c.attr("value")}else b[d[a].name]=jQuery(this).html();a++});0<a&&f.push(b)});
	jQuery(this).empty();jQuery(this).addClass("scroll");jQuery(this).jqGrid(jQuery.extend({datatype:"local",width:b,colNames:e,colModel:d,multiselect:c},m||{}));for(b=0;b<f.length;b++)a=null,0<g.length&&(a=g[b])&&a.replace&&(a=encodeURIComponent(a).replace(/[.\-%]/g,"_")),null===a&&(a=b+1),jQuery(this).jqGrid("addRowData",a,f[b]);for(b=0;b<h.length;b++)jQuery(this).jqGrid("setSelection",h[b])}})};
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 253:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;// require option properties: suggestionDropdown, url, target
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(254)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	    "use strict";
	
	    var mainOption = {
	        suggestionDropdown: '#suggestionDropdown',
	        url: null,
	        target: null
	    };
	
	    var _autoComplete;
	
	    var _me = {
	        initialize: function (option) {
	            _me.initAutoComplete(option);
	            _me.initEvent();
	        },
	
	        initEvent: function () {
	            $('body').click(function () {
	                _me.hideDropdown.call(_autoComplete);
	            });
	
	            $(mainOption.target).keypress(function (e) {
	                if (e.keyCode === 13) {
	                    _me.hideDropdown.call(_autoComplete);
	                }
	            });
	        },
	
	        initAutoComplete: function (option) {
	            $.extend(mainOption, option);
	            if (!mainOption.url || !mainOption.target) {
	                throw Error("Missing data in option");
	            }
	
	            _autoComplete = $(mainOption.target).materialize_autocomplete({ // jshint ignore:line
	                ignoreCase: false,
	                multiple: {
	                    enable: false
	                },
	                dropdown: {
	                    el: mainOption.suggestionDropdown
	                },
	                getData: function (value, callback) {
	                    var url = mainOption.url + "?query=" + value;
	
	                    $.ajax({
	                        type: "GET",
	                        dataType: "json",
	                        url: url,
	                        success: function (response) {
	                            var suggestions = response.suggestions;
	
	                            if (suggestions && suggestions.length > 0) {
	                                var data = suggestions.map(function (item) {
	                                    return { id: item, text: item };
	                                });
	
	                                callback(value, data);
	                            }
	                        }
	                    });
	                }
	            });
	
	            _me.updateWidthOfSuggestionDropdown();
	        },
	
	        updateWidthOfSuggestionDropdown: function () {
	            var $autoCompleteTarget = $(mainOption.target);
	            var $suggestionDropdown = $(mainOption.suggestionDropdown);
	            if ($suggestionDropdown.width() !== $autoCompleteTarget.width()) {
	                $suggestionDropdown.width($autoCompleteTarget.width());
	            }
	        },
	
	        hideDropdown: function () {
	            this.$dropdown.html('').hide();
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 254:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;(function (root, factory) {
	
	    if(typeof module === 'object' && module.exports) {
	        module.exports = factory(__webpack_require__(1));
	    } else if(true) {
	        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (factory), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	    } else {
	        factory(root.jQuery);
	    }
	
	}(this, function ($) {
	
	    var template = function (text) {
	        var matcher = new RegExp('<%=([\\s\\S]+?)%>|<%([\\s\\S]+?)%>|$', 'g');
	
	        var escapes = {
	            "'": "'",
	            '\\': '\\',
	            '\r': 'r',
	            '\n': 'n',
	            '\u2028': 'u2028',
	            '\u2029': 'u2029'
	        };
	
	        var escapeRegExp = /\\|'|\r|\n|\u2028|\u2029/g;
	
	        var escapeChar = function(match) {
	            return '\\' + escapes[match];
	        };
	
	        var index = 0;
	        var source = "__p+='";
	
	        text.replace(matcher, function(match, interpolate, evaluate, offset) {
	            source += text.slice(index, offset).replace(escapeRegExp, escapeChar);
	            index = offset + match.length;
	
	            if (interpolate) {
	                source += "'+\n((__t=(" + interpolate + "))==null?'':__t)+\n'";
	            } else if (evaluate) {
	                source += "';\n" + evaluate + "\n__p+='";
	            }
	
	            return match;
	        });
	
	        source += "';\n";
	        source = 'with(obj||{}){\n' + source + '}\n';
	        source = "var __t,__p='',__j=Array.prototype.join," +
	            "print=function(){__p+=__j.call(arguments,'');};\n" +
	            source + 'return __p;\n';
	
	        var render;
	
	        try {
	            render = new Function('obj', source);
	        } catch (e) {
	            e.source = source;
	            throw e;
	        }
	
	        var _template = function(data) {
	            return render.call(this, data);
	        };
	
	        _template.source = 'function(obj){\n' + source + '}';
	
	        return _template;
	    };
	
	    var Autocomplete = function (el, options) {
	        this.options = $.extend(true, {}, Autocomplete.defaults, options);
	        this.$el = $(el);
	        this.$wrapper = this.$el.parent();
	        this.compiled = {};
	        this.$dropdown = null;
	        this.$appender = null;
	        this.$hidden = null;
	        this.resultCache = {};
	        this.value = '';
	        this.initialize();
	    };
	
	    Autocomplete.defaults = {
	        cacheable: true,
	        limit: 10,
	        multiple: {
	            enable: false,
	            maxSize: 4,
	            onExist: function (item) {
	                Materialize.toast('Tag: ' + item.text + '(' + item.id + ') is already added!', 2000);
	            },
	            onExceed: function (maxSize, item) {
	                Materialize.toast('Can\'t add over ' + maxSize + ' tags!', 2000);
	            },
	            onAppend: function (item) {
	                var self = this;
	                self.$el.removeClass('active');
	                self.$el.click();
	            },
	            onRemove: function (item) {
	                var self = this;
	                self.$el.removeClass('active');
	                self.$el.click();
	            }
	        },
	        hidden: {
	            enable: true,
	            el: '',
	            inputName: '',
	            required: false
	        },
	        appender: {
	            el: '',
	            tagName: 'ul',
	            className: 'ac-appender',
	            tagTemplate: '<div class="chip" data-id="<%= item.id %>" data-text="<% item.text %>"><%= item.text %>(<%= item.id %>)<i class="material-icons close">close</i></div>'
	        },
	        dropdown: {
	            el: '',
	            tagName: 'ul',
	            className: 'ac-dropdown',
	            itemTemplate: '<li class="ac-item" data-id="<%= item.id %>" data-text="<%= item.text %>"><a href="javascript:void(0)"><%= item.text %></a></li>',
	            noItem: ''
	        },
	        handlers: {
	            'setValue': '.ac-dropdown .ac-item',
	            '.ac-appender .ac-tag .close': 'remove'
	        },
	        getData: function (value, callback) {
	            callback(value, []);
	        },
	        ignoreCase: true,
	        throttling: true
	    };
	
	    Autocomplete.prototype = {
	        constructor: Autocomplete,
	        initialize: function () {
	            var self = this;
	            var timer;
	            var fetching = false;
	
	            function getItemsHtml (list) {
	                var itemsHtml = '';
	
	                if (!list.length) {
	                    return self.options.dropdown.noItem;
	                }
	
	                list.forEach(function (item, idx) {
	
	                    if (idx >= self.options.limit) {
	                        return false;
	                    }
	
	                    itemsHtml += self.compiled.item({ 'item': item});
	                });
	
	                return itemsHtml;
	            }
	
	            function handleList (value, list) {
	                var itemsHtml = getItemsHtml(list);
	                var currentValue = self.$el.val();
	
	                if (self.options.ignoreCase) {
	                    currentValue = currentValue.toUpperCase();
	                }
	
	                if (self.options.cacheable && !self.resultCache.hasOwnProperty(value)) {
	                    self.resultCache[value] = list;
	                }
	
	                if (value !== currentValue) {
	                    return false;
	                }
	
	                if(itemsHtml) {
	                    self.$dropdown.html(itemsHtml);
	                    self.$dropdown.show();
	                } else {
	                    self.$dropdown.hide();
	                }
	
	            }
	
	            self.value = self.options.multiple.enable ? [] : '';
	
	            self.compiled.tag = template(self.options.appender.tagTemplate);
	            self.compiled.item = template(self.options.dropdown.itemTemplate);
	
	            self.render();
	
	            self.$el.on('input', function (e) {
	                var $t = $(this);
	                var value = $t.val();
	
	                if (!value) {
	                    self.$dropdown.hide();
	                    return false;
	                }
	
	                if (self.options.ignoreCase) {
	                    value = value.toUpperCase();
	                }
	
	                if (self.resultCache.hasOwnProperty(value) && self.resultCache[value]) {
	                    handleList(value, self.resultCache[value]);
	                    return true;
	                }
	
	                if (self.options.throttling) {
	                    clearTimeout(timer);
	                    timer = setTimeout(function () {
	                        self.options.getData(value, handleList);
	                    }, 200);
	                    return true;
	                }
	
	                self.options.getData(value, handleList);
	            });
	
	            self.$el.on('keydown', function (e) {
	                var $t = $(this);
	                var keyCode = e.keyCode;
	                var $items, $hover;
	                // BACKSPACE KEY
	                if (keyCode == '8' && !$t.val()) {
	
	                    if (!self.options.multiple.enable) {
	                        return true;
	                    }
	
	                    if (!self.value.length) {
	                        return true;
	                    }
	
	                    var lastItem = self.value[self.value.length - 1];
	                    self.remove(lastItem);
	                    return false;
	                }
	                // UP DOWN ARROW KEY
	                if (keyCode == '38' || keyCode == '40') {
	
	                    $items = self.$dropdown.find('[data-id]');
	
	                    if (!$items.length) {
	                        return false;
	                    }
	
	                    $hover = $items.filter('.ac-hover');
	
	                    if (!$hover.length) {
	                        $items.removeClass('ac-hover');
	                        $items.eq(keyCode == '40' ? 0 : -1).addClass('ac-hover');
	                    } else {
	                        var index = $hover.index();
	                        $items.removeClass('ac-hover');
	                        $items.eq(keyCode == '40' ? (index + 1) % $items.length : index - 1).addClass('ac-hover');
	                    }
	
	                    return false;
	                }
	                // ENTER KEY CODE
	                if (keyCode == '13') {
	                    $items = self.$dropdown.find('[data-id]');
	
	                    if (!$items.length) {
	                        return false;
	                    }
	
	                    $hover = $items.filter('.ac-hover');
	
	                    if (!$hover.length) {
	                        return false;
	                    }
	
	                    self.setValue({
	                        id: $hover.data('id'),
	                        text: $hover.data('text')
	                    });
	
	                    return false;
	                }
	
	            });
	
	            self.$dropdown.on('click', '[data-id]', function (e) {
	                var $t = $(this);
	                var item = {
	                    id: $t.data('id'),
	                    text: $t.data('text')
	                };
	
	                self.setValue(item);
	            });
	
	            self.$appender.on('click', '[data-id] .close', function (e) {
	                var $t = $(this);
	                var $li = $t.closest('[data-id]');
	                var item = {
	                    id: $li.data('id'),
	                    text: $li.data('text')
	                };
	
	                self.remove(item);
	            });
	
	        },
	        render: function () {
	            var self = this;
	
	            if (self.options.dropdown.el) {
	                self.$dropdown = $(self.options.dropdown.el);
	            } else {
	                self.$dropdown = $(document.createElement(self.options.dropdown.tagName));
	                self.$dropdown.insertAfter(self.$el);
	            }
	
	            self.$dropdown.addClass(self.options.dropdown.className);
	
	            if (self.options.appender.el) {
	                self.$appender = $(self.options.appender.el);
	            } else {
	                self.$appender = $(document.createElement(self.options.appender.tagName));
	                self.$appender.insertBefore(self.$el);
	            }
	
	            if (self.options.hidden.enable) {
	
	                if (self.options.hidden.el) {
	                    self.$hidden = $(self.options.hidden.el);
	                } else {
	                    self.$hidden = $('<input type="hidden" class="validate" />');
	                    self.$wrapper.append(self.$hidden);
	                }
	
	                if (self.options.hidden.inputName) {
	                    self.$el.attr('name', self.options.hidden.inputName);
	                }
	
	                if (self.options.hidden.required) {
	                    self.$hidden.attr('required', 'required');
	                }
	
	            }
	
	            self.$appender.addClass(self.options.appender.className);
	
	        },
	        setValue: function (item) {
	            var self = this;
	
	            if (self.options.multiple.enable) {
	                self.append(item);
	            } else {
	                self.select(item);
	            }
	
	        },
	        append: function (item) {
	            var self = this;
	            var $tag = self.compiled.tag({ 'item': item });
	
	            if (self.value.some(function (selectedItem) {
	                    return selectedItem.id === item.id;
	                })) {
	
	                if ('function' === typeof self.options.multiple.onExist) {
	                    self.options.multiple.onExist.call(this, item);
	                }
	
	                return false;
	            }
	
	            if (self.value.length >= self.options.multiple.maxSize) {
	
	                if ('function' === typeof self.options.multiple.onExceed) {
	                    self.options.multiple.onExceed.call(this, self.options.multiple.maxSize);
	                }
	
	                return false;
	            }
	
	            self.value.push(item);
	            self.$appender.append($tag);
	
	            var valueStr = self.value.map(function (selectedItem) {
	                return selectedItem.id;
	            }).join(',');
	
	            if (self.options.hidden.enable) {
	                self.$hidden.val(valueStr);
	            }
	
	            self.$el.val('');
	            self.$el.data('value', valueStr);
	            self.$dropdown.html('').hide();
	
	            if ('function' === typeof self.options.multiple.onAppend) {
	                self.options.multiple.onAppend.call(self, item);
	            }
	
	        },
	        remove: function (item) {
	            var self = this;
	
	            self.$appender.find('[data-id="' + item.id + '"]').remove();
	            self.value = self.value.filter(function (selectedItem) {
	                return selectedItem.id !== item.id;
	            });
	
	            var valueStr = self.value.map(function (selectedItem) {
	                return selectedItem.id;
	            }).join(',');
	
	            if (self.options.hidden.enable) {
	                self.$hidden.val(valueStr);
	                self.$el.data('value', valueStr);
	            }
	
	            self.$dropdown.html('').hide();
	
	            if ('function' === typeof self.options.multiple.onRemove) {
	                self.options.multiple.onRemove.call(self, item);
	            }
	
	        },
	        select: function (item) {
	            var self = this;
	
	            self.value = item.text;
	            self.$el.val(item.text);
	            self.$el.data('value', item.id);
	            self.$dropdown.html('').hide();
	
	            if (self.options.hidden.enable) {
	                self.$hidden.val(item.id);
	            }
	        }
	    };
	
	    $.fn.materialize_autocomplete = function (options) {
	        var el = this;
	        var $el = $(el).eq(0);
	        var instance = $el.data('autocomplete');
	
	        if (instance && arguments.length) {
	            return instance;
	        }
	
	        var autocomplete = new Autocomplete(el, options);
	        $el.data('autocomplete', autocomplete);
	        $el.dropdown();
	        return autocomplete;
	    };
	
	}));


/***/ },

/***/ 255:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	    var logIdColumn = {
	        name: 'LogId',
	        index: 'LogId',
	        resizable: false,
	        width: 62,
	        align: 'center',
	        sortable: false
	    };
	    var logDateColumn = {
	        name: 'LogDate',
	        index: 'LogDate',
	        resizable: false,
	        width: 80,
	        align: 'center',
	        valign: 'middle',
	        sortable: false,
	        formatter: 'date',
	        formatoptions:
	        {
	            srcformat: 'Y-m-d h:i:s A',
	            newformat: 'n/j/Y h:i:s A'
	        }
	    };
	    var editedByColumn = {
	        name: 'EditedBy',
	        index: 'EditedBy',
	        resizable: false,
	        width: 80,
	        align: 'center',
	        valign: 'middle',
	        sortable: true
	    };
	    var actionColumn = {
	        name: 'ActionName',
	        index: 'ActionName',
	        resizable: false,
	        width: 150,
	        align: 'left',
	        sortable: false
	    };
	    var detailColumn = {
	        name: 'Details',
	        index: 'Details',
	        resizable: false,
	        width: 500,
	        align: 'center',
	        title: false,
	        sortable: false,
	        classes: 'jqgrid-detail-col'
	    };
	
	    var colNameList = ['LogID', 'LogDate', 'Editedby', 'Action', 'Details'];
	    var colModelList = [logIdColumn, logDateColumn, editedByColumn, actionColumn, detailColumn];
	
	    var _me = {
	        gridOption: {
	            height: "auto",
	            datatype: 'local',
	            colNames: [],
	            colModel: colModelList,
	            rowNum: 50,
	            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
	            viewrecords: true,
	            loadui: 'disable',
	            autowidth: false,
	            forceFit: false,
	            shrinkToFit: true,
	            scroll: false,
	            loadonce: true,
	            gridview: true,
	            sortorder: "desc",
	            pager: '#ppdata'
	        },
	
	        getColNames: function () {
	            return colNameList
	        }
	    }
	
	    return _me;;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 256:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(257)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	        "use strict";
	        var PICKER_SELECTOR = "#month-picker";
	        var FORMAT = "MM/dd/yyyy";
	
	        var _historyDate = new Date($("#HistoryDate").val());
	
	        function initMonthPicker(option) {
	            var $picker = $(PICKER_SELECTOR);
	            var $fromDate = $('#FromDate');
	            var $todate = $('#ToDate');
	
	            var fromdate = new Date($fromDate.val());
	            var todate = new Date($todate.val());
	
	            var defaultOption = {
	                minDate: _historyDate,
	                showDateRange: true,
	                fromDayId: 'fDay',
	                toDayId: 'tDay',
	                format: FORMAT,
	                month: fromdate,
	                fromDay: fromdate.getDate(),
	                toDay: todate.getDate(),
	                afterChangeMonth: function () {
	                    $fromDate.val($picker.monPickr("getFromDate", FORMAT));
	                    $todate.val($picker.monPickr("getToDate", FORMAT));
	                },
	                afterChangeDay: function () {
	                    $fromDate.val($picker.monPickr("getFromDate", FORMAT));
	                    $todate.val($picker.monPickr("getToDate", FORMAT));
	
	                    if (option && typeof option.customAfterChangeDay === "function") {
	                        option.customAfterChangeDay();
	                    }
	                }
	            };
	
	            if (option) {
	                $.extend(defaultOption, option);
	            }
	
	            $picker.monPickr(defaultOption);
	        }
	
	        function getFromDateString() {
	            return $(PICKER_SELECTOR).monPickr("getFromDate", "dd MMM yyyy");
	        }
	
	        function getToDateString() {
	            return $(PICKER_SELECTOR).monPickr("getToDate", "dd MMM yyyy");
	        }
	
	        return {
	            initMonthPicker: initMonthPicker,
	            getFromDateString: getFromDateString,
	            getToDateString: getToDateString
	        };
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 257:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function(jQuery) {/// <reference path="/@JSense.js" />
	/*
	* monPickr - jQuery Plugin
	* Just a simple month picker
	*
	* Examples and documentation at: http://update.later
	*
	* Version: 0.0.2 (9 Feb 2012)
	* Requires: jQuery v1.7+
	*/
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(7)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site) {
	    (function ($) {
	        var logger = { error: $.noop };
	
	        Date.prototype.toStringFormat = function (format) {
	            var shortMonthsInYear = Site.getResources(["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]),
	                longMonthsInYear = Site.getResources(["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]),
	                year = this.getFullYear(),
	                month = this.getMonth(),
	                day = this.getDate(),
	                result = format;
	
	            function strMonth(value) {
	                var monthArrayIndex = parseInt(value);
	                return shortMonthsInYear[monthArrayIndex] || value;
	            }
	
	            function strLongMonth(value) {
	                var monthArrayIndex = parseInt(value);
	                return longMonthsInYear[monthArrayIndex] || value;
	            }
	
	            result = result.replace(/yyyy/g, year);
	            result = result.replace(/yy/g, year.toString().substr(2));
	
	            result = result.replace(/MMMM/g, strLongMonth(month));
	            result = result.replace(/MMM/g, strMonth(month));
	            result = result.replace(/MM/g, month < 9 ? '0' + (month + 1) : (month + 1));
	
	            result = result.replace(/dd/g, day < 10 ? '0' + day : day);
	
	            return result;
	        };
	
	        var isInteger = function (value, writeLogToConsole) {
	            var isValid = Math.ceil(value) == Math.floor(value);
	
	            if (writeLogToConsole && !isValid) logger.error('Oops! "' + value + '" is an invalid Integer');
	
	            return isValid;
	        }
	
	        var isValidDate = function (value, writeLogToConsole) {
	            var _value = new Date(value),
	                isValid = isInteger(_value.getDate()) && isInteger(_value.getMonth()) && isInteger(_value.getFullYear());
	
	            if (writeLogToConsole && !isValid) logger.error('Oops! "' + value + '" is an invalid Date');
	
	            return isValid;
	        }
	
	        var today = null,
	            thisYear = null,
	            thisMonth = null,
	            thisDay = null,
	            firstDayThisYear = null,
	            firstDayThisMonth = null,
	            lastDayThisMonth = null,
	
	        /*
	        * Private methods
	        */
	
	            _getData = function (target) {
	                var data = $(target).data('monPickr');
	                if (!data) {
	                    logger.error('Oops! No data of monPickr is available');
	                    return null;
	                }
	                return data;
	            },
	
	            _validateMonth = function (minDate, maxDate, year, month) {
	                var minYear = minDate.getFullYear(),
	                    minMonth = minDate.getMonth(),
	                    maxYear = maxDate.getFullYear(),
	                    maxMonth = maxDate.getMonth(),
	
	                    outOfLowBoundary = year < minYear || (year == minYear && month < minMonth),
	                    outOfHighBoundary = year > maxYear || (year == maxYear && month > maxMonth);
	
	                return !outOfLowBoundary && !outOfHighBoundary;
	            },
	
	            _validateDate = function (minDate, maxDate, year, month, date) {
	                // Re-new the params cauz of time ignorance
	                var minDayInMonth = new Date(year, month, _getMinDayInMonth(minDate, year, month)),
	                    maxDayInMonth = new Date(year, month, _getMaxDayInMonth(maxDate, year, month)),
	                    date = new Date(date.getFullYear(), date.getMonth(), date.getDate());
	
	                return minDate <= date && date <= maxDayInMonth;
	            },
	
	            _checkReachBound = function (data, year, month) {
	                // diable next/prev button if the selected month reach the bound
	                var nextMonth = new Date(year, month + 1, 1),
	                    prevMonth = new Date(year, month - 1, 1);
	
	                if (!_validateMonth(data.minDate, data.maxDate, nextMonth.getFullYear(), nextMonth.getMonth())) data.btnNext.addClass('disable');
	                else data.btnNext.removeClass('disable');
	
	                if (!_validateMonth(data.minDate, data.maxDate, prevMonth.getFullYear(), prevMonth.getMonth())) data.btnPrev.addClass('disable');
	                else data.btnPrev.removeClass('disable');
	            },
	
	            _setMonth = function (data, year, month) {
	                data.year = year;
	                data.month = month;
	            },
	
	            _setMonthLabel = function (data) {
	                data.lblSelectedMonth.html(new Date(data.year, data.month, 1).toStringFormat(data.monthFormat));
	            },
	
	            _selectMonth = function (data, year, month) {
	                // invokes handler (if any) just before the event change month
	                if (typeof data.beforeChangeMonth == 'function') data.beforeChangeMonth();
	                else { logger.error('Oops! beforeChangeMonth event\'s handler (which has type "' + typeof data.beforeChangeMonth + '") is not a function'); }
	
	                // highlight selected month
	                data.dropDown.find('.monthWrap a[month].selected').removeClass('selected');
	                data.dropDown.find('.monthWrap[year=' + year + '] a[month=' + (month + 1) + ']:first').addClass('selected');
	
	                _setMonth(data, year, month);
	                _setMonthLabel(data);
	
	                // if selected year differs from current year then hide current year and show selected one
	                if (data.dropDown.find('.monthWrap.selected').attr('year') != year) {
	                    _hideCurrentYear(data);
	                    _showSelectedYear(data, year);
	                    data.dropDown.find('.year a.selected:first').parent().addClass('selected');
	                }
	
	                if (data.showDateRange) {
	                    var minDayInMonth = _getMinDayInMonth(data.minDate, data.year, data.month),
	                    maxDayInMonth = _getMaxDayInMonth(data.maxDate, data.year, data.month);
	
	                    // change fromDate and toDate
	                    _selectDay(data, data.ddlFromDate, minDayInMonth, true);
	                    _selectDay(data, data.ddlToDate, maxDayInMonth, false);
	                }
	
	                _checkReachBound(data, year, month);
	
	                // invokes handler (if any) just after the event change month
	                if (typeof data.afterChangeMonth == 'function') data.afterChangeMonth();
	                else { logger.error('Oops! afterChangeMonth event\'s handler (which has type "' + typeof data.afterChangeMonth + '") is not a function'); }
	            },
	
	            _setDay = function (data, value, isFromDay) {
	                var minDayInMonth = _getMinDayInMonth(data.minDate, data.year, data.month),
	                    maxDayInMonth = _getMaxDayInMonth(data.maxDate, data.year, data.month);
	
	                if (isFromDay) {
	                    data.fromDay = parseInt(value, 10);
	                    data.fromDate = new Date(data.year, data.month, value);
	
	                    data.ddlToDate.dropDown.find('li').each(function (index) {
	                        var day = index + 1;
	                        var isShow = day >= data.fromDay && day >= minDayInMonth && day <= maxDayInMonth;
	                        $(this).toggle(isShow);
	                    });
	                } else {
	                    data.toDay = parseInt(value, 10);
	                    data.toDate = new Date(data.year, data.month, value);
	
	                    data.ddlFromDate.dropDown.find('li').each(function (index) {
	                        var day = index + 1;
	                        var isShow = day <= data.toDay && day >= minDayInMonth && day <= maxDayInMonth;
	                        $(this).toggle(isShow);
	                    });
	                }
	            },
	
	            _setDayLabel = function (target, value) {
	                target.find('.selectedDay').html(value);
	            },
	
	            _selectDay = function (data, target, value, isFromDay) {
	                // invokes handler (if any) just before the event change day
	                if (typeof data.beforeChangeDay == 'function') data.beforeChangeDay();
	                else { logger.error('Oops! beforeChangeDay event\'s handler (which has type "' + typeof data.beforeChangeDay + '") is not a function'); }
	
	                _setDayLabel(target, value);
	                _setDay(data, value, isFromDay);
	
	                // invokes handler (if any) just after the event change day
	                if (typeof data.afterChangeDay == 'function') data.afterChangeDay();
	                else { logger.error('Oops! afterChangeDay event\'s handler (which has type "' + typeof data.afterChangeDay + '") is not a function'); }
	            },
	
	            _getMinDayInMonth = function (minDate, year, month) {
	                var _firstDayOfMonth = new Date(year, month, 1),
	                    _minDate = new Date(minDate.getFullYear(), minDate.getMonth(), minDate.getDate());
	
	                return _minDate > _firstDayOfMonth ? _minDate.getDate() : 1;
	            },
	
	            _getMaxDayInMonth = function (maxDate, year, month) {
	                var _lastDayOfMonth = new Date(year, month + 1, 0),
	                    _maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth(), maxDate.getDate());
	
	                return _lastDayOfMonth > _maxDate ? _maxDate.getDate() : _lastDayOfMonth.getDate();
	            },
	
	            _hideCurrentYear = function (data) {
	                // collapse current year
	                var currentMonthWrap = data.dropDown.find('.monthWrap.selected').eq(0);
	
	                if (data.dropDown.is(':hidden')) {
	                    currentMonthWrap.hide();
	                } else {
	                    currentMonthWrap.slideUp(data.showYearSpeed);
	                }
	
	                currentMonthWrap.removeClass('selected');
	                data.dropDown.find('.year .selected').eq(0).removeClass('selected').parent().removeClass('selected');
	            },
	
	            _showSelectedYear = function (data, year) {
	                var target = data.dropDown.find('.monthWrap[year=' + year + ']').eq(0);
	
	                target.prev('.year').find('a[year]').eq(0).addClass('selected');
	                target.addClass('selected').slideDown(data.showYearSpeed);
	            },
	
	            _showDropDown = function (data, target) {
	                target.dropDown.slideDown(data.showDropDownSpeed);
	            },
	
	            _hideDropDown = function (data, target) {
	                target.dropDown.slideUp(data.showDropDownSpeed);
	            },
	
	        _methods = {
	            init: function (options) {
	                options = options || {};
	
	                if ($.isFunction(options.log)) logger.error = options.log;
	
	                if (!this.length) { // if the initial DOM does not exist
	                    logger.error('Oops! DOM element "' + this.selector + '" does not exist');
	                    return;
	                }
	
	                if (options.today && !isValidDate(options.today, true)) return;
	
	                today = options.today ? new Date(options.today) : new Date();
	                thisYear = today.getFullYear();
	                thisMonth = today.getMonth();
	                thisDay = today.getDate();
	                firstDayThisYear = new Date(thisYear, 0, 1);
	                firstDayThisMonth = new Date(thisYear, thisMonth, 1);
	                lastDayThisMonth = new Date(thisYear, thisMonth + 1, 0);
	
	                // Create some defaults, extending them with any options that were provided
	                var settings = $.extend({
	                    minDate: firstDayThisYear,
	                    maxDate: today,
	                    month: today,
	
	                    showDateRange: false,
	                    fromDay: firstDayThisMonth.getDate(),
	                    toDay: today.getDate(),
	                    fromDayId: null, // required if showDateRange is true
	                    toDayId: null, // required if showDateRange is true
	
	                    expandYearOnhover: false,
	                    expandDayOnhover: false,
	                    showYearSpeed: 110,
	                    showDropDownSpeed: 10,
	
	                    disabled: false,
	                    sortDesc: false,
	                    monthFormat: 'MMM yyyy',
	                    nextText: 'Next Month',
	                    prevText: 'Previous Month',
	                    monPickrText: 'Show/hide Month Picker',
	                    zIndex: 9999,
	
	                    beforeChangeMonth: function () { },
	                    afterChangeMonth: function () { },
	                    beforeChangeDay: function () { },
	                    afterChangeDay: function () { }
	                }, options);
	
	                //#region VALIDATES INPUT VALUES
	
	                // Validates minDate & maxDate
	                if (!isValidDate(settings.minDate, true) || !isValidDate(settings.maxDate, true)) return;
	
	                // Convert minDate & maxDate to Date type for easier later using
	                settings.minDate = new Date(settings.minDate);
	                settings.maxDate = new Date(settings.maxDate);
	
	                // Validtes selected month
	                if (!isValidDate(settings.month, true)) return;
	
	                // Convert selected month to Date type for easier later using
	                settings.month = new Date(settings.month);
	
	                var year = settings.month.getFullYear(),
	                    month = settings.month.getMonth();
	
	                // Make sure selected month are not out of range (minDate ~ maxDate)
	                if (!_validateMonth(settings.minDate, settings.maxDate, year, month)) {
	                    logger.error('Oops! Selected month (' + settings.month.toStringFormat('MM/yyyy') +
	                        ') is out of range (' + settings.minDate.toStringFormat('MM/yyyy') + ' ~ ' + settings.maxDate.toStringFormat('MM/yyyy') + ')');
	                    return;
	                }
	
	                // Makes sure selected fromDate & toDate are not out of Range (minDate ~ maxDate)
	                var fromDate = new Date(year, month, settings.fromDay),
	                    toDate = new Date(year, month, settings.toDay);
	
	                if (!_validateDate(settings.minDate, settings.maxDate, year, month, fromDate)) {
	                    logger.error('Oops! fromDate (' + fromDate.toStringFormat('MMM dd, yyyy') + ') is out of range');
	                    return;
	                }
	
	                if (!_validateDate(settings.minDate, settings.maxDate, year, month, toDate)) {
	                    logger.error('Oops! toDate (' + toDate.toStringFormat('MMM dd, yyyy') + ') is out of range');
	                    return;
	                }
	
	                //#endregion VALIDATES INPUT VALUES
	
	                return this.each(function () {
	                    var $this = $(this),
	                        data = $this.data('monPickr');
	
	                    // Initialize the plugin if it hasn't
	                    if (!data) {
	                        $this.data('monPickr', {
	                            target: $this,
	
	                            minDate: settings.minDate,
	                            maxDate: settings.maxDate,
	                            originalYear: settings.year,
	                            originalMonth: settings.month,
	                            year: settings.month.getFullYear(),
	                            month: settings.month.getMonth(),
	
	                            showDateRange: settings.showDateRange,
	                            originalFromDay: settings.fromDay,
	                            originalToDay: settings.toDay,
	                            fromDay: settings.fromDay,
	                            toDay: settings.toDay,
	                            fromDate: fromDate,
	                            toDate: toDate,
	                            fromDayId: settings.fromDayId,
	                            toDayId: settings.toDayId,
	
	                            expandYearOnhover: settings.expandYearOnhover,
	                            expandDayOnhover: settings.expandDayOnhover,
	                            showYearSpeed: settings.showYearSpeed,
	                            showDropDownSpeed: settings.showDropDownSpeed,
	
	                            disabled: settings.disabled,
	                            sortDesc: settings.sortDesc,
	                            monthFormat: settings.monthFormat,
	                            nextText: settings.nextText,
	                            prevText: settings.prevText,
	                            monPickrText: settings.monPickrText,
	                            zIndex: settings.zIndex,
	
	                            beforeChangeMonth: settings.beforeChangeMonth,
	                            afterChangeMonth: settings.afterChangeMonth,
	                            beforeChangeDay: settings.beforeChangeDay,
	                            afterChangeDay: settings.afterChangeDay
	                        });
	
	                        data = $this.data('monPickr')
	                    }
	
	                    //#region INIT DOM ELEMENTS
	
	                    //#region Month Picker
	
	                    $this.addClass('monPickr');
	                    data.btnPrev = $('<a href="javascript:;" class="prev" title="' + data.prevText + '"><b>&lsaquo;</b></a>');
	                    data.btnNext = $('<a href="javascript:;" class="next" title="' + data.nextText + '"><b>&rsaquo;</b></a>');
	                    data.lblSelectedMonth = $('<span class="selectedMonth" title="' + data.monPickrText + '"></span>');
	                    _setMonthLabel(data);
	                    $this.append(data.btnPrev, data.lblSelectedMonth, data.btnNext);
	
	                    $this.wrap('<div class="monPickrWrap" style="z-index: ' + data.zIndex + '" />');
	                    data.wrap = $this.parents('.monPickrWrap');
	                    data.wrap.wrap('<div class="monPickrWrapParent" style="position: relative" />')
	                        .css({ 'position': 'absolute' }).parent().height(data.wrap.outerHeight());
	
	                    data.dropDown = $('<div class="monPickrDropDown" />');
	
	                    var currentYear = data.minDate.getFullYear(),
	                        currentMonth = data.minDate.getMonth(),
	                        minYear = data.minDate.getFullYear(),
	                        minMonth = data.minDate.getMonth(),
	                        maxYear = data.maxDate.getFullYear(),
	                        maxMonth = data.maxDate.getMonth(),
	                        dropDownInner, temp;
	
	                    dropDownInner = '<ul class="monPickrDropDownInner">';
	
	                    if (data.sortDesc) {
	                        currentYear = data.maxDate.getFullYear();
	                        currentMonth = data.maxDate.getMonth();
	
	                        while (currentYear > minYear || (currentYear == minYear && currentMonth >= minMonth)) {
	                            temp = currentYear == data.year ? 'selected' : '';
	                            temp += data.expandYearOnhover ? ' hover' : '';
	
	                            dropDownInner +=
	                            '<li class="year ' + temp + '">' +
	                                '<a year="' + currentYear + '" class="' + temp + '" href="javascript:;">' +
	                                    currentYear +
	                                '</a>' +
	                            '</li>';
	
	                            if (currentYear == data.year) {
	                                dropDownInner += '<li class="monthWrap selected" year="' + currentYear + '" style="display: list-item"><ul>';
	                            } else {
	                                dropDownInner += '<li class="monthWrap" year="' + currentYear + '"><ul>';
	                            }
	
	                            while ((currentYear > minYear || (currentYear == minYear && currentMonth >= minMonth)) && currentMonth >= 0) {
	                                temp = currentYear == data.year && currentMonth == data.month ? 'class="selected"' : '';
	
	                                dropDownInner +=
	                                '<li>' +
	                                    '<a month="' + (currentMonth + 1) + '" ' + temp + ' href="javascript:;">' +
	                                        new Date(currentYear, currentMonth).toStringFormat('MMMM') + '</a>' +
	                                '<li>';
	
	                                currentMonth--;
	                            }
	
	                            dropDownInner += '</ul></li>';
	
	                            currentYear--;
	                            currentMonth = 11;
	                        }
	                    } else {
	                        while (currentYear < maxYear || (currentYear == maxYear && currentMonth <= maxMonth)) {
	                            temp = currentYear == data.year ? 'selected' : '';
	                            temp += data.expandYearOnhover ? ' hover' : '';
	
	                            dropDownInner +=
	                            '<li class="year ' + temp + '">' +
	                                '<a year="' + currentYear + '" class="' + temp + '" href="javascript:;">' +
	                                    currentYear +
	                                '</a>' +
	                            '</li>';
	
	                            if (currentYear == data.year) {
	                                dropDownInner += '<li class="monthWrap selected" year="' + currentYear + '" style="display: list-item"><ul>';
	                            } else {
	                                dropDownInner += '<li class="monthWrap" year="' + currentYear + '"><ul>';
	                            }
	
	                            while ((currentYear < maxYear || (currentYear == maxYear && currentMonth <= maxMonth)) && currentMonth < 12) {
	                                temp = currentYear == data.year && currentMonth == data.month ? 'class="selected"' : '';
	
	                                dropDownInner +=
	                                '<li>' +
	                                    '<a month="' + (currentMonth + 1) + '" ' + temp + ' href="javascript:;">' +
	                                        new Date(currentYear, currentMonth).toStringFormat('MMMM') + '</a>' +
	                                '<li>';
	
	                                currentMonth++;
	                            }
	
	                            dropDownInner += '</ul></li>';
	
	                            currentYear++;
	                            currentMonth = 0;
	                        }
	                    }
	
	                    dropDownInner += '</ul>';
	
	                    data.dropDown.append(dropDownInner);
	                    $this.after(data.dropDown);
	
	                    _checkReachBound(data, year, month);
	
	                    //#region Events
	
	                    // Toggle the dropdown when users click on selected month label
	                    data.wrap.on('click', function (e) {
	                        _hideDropDown(data, data.ddlFromDate);
	                        _hideDropDown(data, data.ddlToDate);
	
	                        if ($(e.target).hasClass('selectedMonth')) {
	                            if (data.dropDown.is(':hidden')) {
	                                _showDropDown(data, data);
	                            } else {
	                                _hideDropDown(data, data);
	                            }
	                        }
	                    });
	
	                    // Collapse all dropdowns when users click outside the picker
	                    $('html').on('click', function (e) {
	                        var $target = $(e.target);
	                        var pickerId = "#" + data.target.attr("id");
	
	                        if (!$target.closest(pickerId).length) {
	                            _hideDropDown(data, data);
	                        }
	                    });
	
	                    // Toggle a dropdown when users click/hover on its "the year" link
	                    var expectedEvent = data.expandYearOnhover ? 'mouseenter' : 'click';
	                    data.dropDown.on(expectedEvent, '.year a', function () {
	                        var $this = $(this), year = $this.attr('year'),
	                            thisIsNotSelected = $this.hasClass('selected') == false;
	
	                        switch (expectedEvent) {
	                            case 'mouseenter':
	                                {
	                                    if (thisIsNotSelected) {
	                                        _hideCurrentYear(data);
	                                        _showSelectedYear(data, year); // expand this
	                                    }
	                                } break;
	                            case 'click':
	                                {
	                                    _hideCurrentYear(data);
	
	                                    if (thisIsNotSelected) {
	                                        // We have to expand "the year" which was clicked.
	                                        // Else means this is the current year itself --> we don't need to expand it
	                                        _showSelectedYear(data, year);
	                                    }
	
	                                    data.dropDown.find('.year a.selected:first').parent().addClass('selected');
	                                } break;
	                        }
	                    });
	
	                    // Change current month when users select a month
	                    data.dropDown.on('click', '.monthWrap a[month]', function () {
	                        var $this = $(this);
	                        _selectMonth(data, $this.parents('.monthWrap').attr('year'), $this.attr('month') - 1);
	                        _hideDropDown(data, data);
	                    });
	
	                    // Move to next month when users click next button
	                    data.btnNext.on('click', function () {
	                        var nextMonth = data.month + 1,
	                            year = data.year;
	
	                        if (nextMonth > 11) {
	                            nextMonth = 0;
	                            year++;
	                        }
	
	                        if (_validateMonth(data.minDate, data.maxDate, year, nextMonth)) {
	                            _selectMonth(data, year, nextMonth);
	                        }
	                    });
	
	                    // Move to previous month when users click prev button
	                    data.btnPrev.on('click', function () {
	                        var prevMonth = data.month - 1,
	                            year = data.year;
	
	                        if (prevMonth < 0) {
	                            prevMonth = 11;
	                            year--;
	                        }
	
	                        if (_validateMonth(data.minDate, data.maxDate, year, prevMonth)) {
	                            _selectMonth(data, year, prevMonth);
	                        }
	                    });
	
	                    //#endregion Events
	
	                    //#endregion Month Picker
	
	                    //#region Day Picker
	
	                    if (data.showDateRange) { // Renders date range component (fromDate & toDate)
	                        var renderDayPicker = function (data, dayPickr, dayPickrId, selectedDay) {
	                            dayPickr = $('#' + dayPickrId);
	
	                            if (dayPickr.length > 0) {
	                                dayPickr.addClass('dayPickr')
	                                    .append('<span class="selectedDay">' + selectedDay + '</span>')
	                                    .append('<a class="darr" href="javascript:;"><b>v</b></a>')
	                                    .wrap('<div class="selectedDayWrap" style="z-index: ' + data.zIndex + '" />');
	
	                                dayPickr.wrap = dayPickr.parents('.selectedDayWrap')
	                                    .wrap('<div class="selectedDayWrapParent" style="position: relative" />')
	                                    .css({ 'position': 'absolute' });
	                                dayPickr.wrap.parent().height(dayPickr.wrap.outerHeight());
	                                dayPickr.dropDown = $('<div class="dayPickrDropDown" />');
	
	                                var currentDay = 1,
	                                    minDay = _getMinDayInMonth(data.minDate, data.year, data.month),
	                                    maxDay = _getMaxDayInMonth(data.maxDate, data.year, data.month),
	                                    dayPickrDropDown;
	
	                                dayPickrDropDown = '<ul>';
	
	                                while (currentDay <= 31) {
	                                    temp = currentDay < minDay || currentDay > maxDay ? 'style="display: none"' : '';
	                                    dayPickrDropDown += '<li ' + temp + '><a href="javascript:;">' + currentDay + '</a></li>';
	                                    currentDay++;
	                                }
	
	                                dayPickrDropDown += '</ul>';
	
	                                dayPickr.dropDown.append(dayPickrDropDown);
	                                dayPickr.after(dayPickr.dropDown);
	
	                                return dayPickr;
	                            } else {
	                                logger.error('Oops! fromDayId (' + data.fromDayId + ') or toDayId (' + data.toDayId + ') does not exist');
	                            }
	                        }
	
	                        data.ddlFromDate = renderDayPicker(data, data.ddlFromDate, data.fromDayId, data.fromDay);
	                        data.ddlToDate = renderDayPicker(data, data.ddlToDate, data.toDayId, data.toDay);
	
	                        //#region Events
	
	                        var dayExpectedEvent = data.expandDayOnhover ? 'hover' : 'click',
	                            toggleDayPickr = function (e) {
	                                if (data.isClosingDropDown) {
	                                    data.isClosingDropDown = false;
	                                    return;
	                                }
	
	                                var target = e.data.target;
	
	                                _hideDropDown(data, data);
	                                if (target == data.ddlFromDate) {
	                                    _hideDropDown(data, data.ddlToDate);
	                                }
	                                else {
	                                    _hideDropDown(data, data.ddlFromDate);
	                                }
	
	                                if (target.dropDown.is(':hidden')) {
	                                    _showDropDown(data, target);
	                                } else {
	                                    _hideDropDown(data, target);
	                                }
	                            };
	
	                        // Toggle the day picker dropdown when users click/hover on selected day label
	                        data.ddlFromDate.wrap.on(dayExpectedEvent, { target: data.ddlFromDate }, toggleDayPickr);
	                        data.ddlToDate.wrap.on(dayExpectedEvent, { target: data.ddlToDate }, toggleDayPickr);
	
	                        if (!data.expandDayOnhover) {
	                            // Collapse all day picker dropdowns when users click outside the picker
	                            $('html').on('click', function (e) {
	                                var targetId = $(e.target).attr("id");
	
	                                if (targetId !== data.fromDayId && targetId !== data.toDayId) {
	                                    _hideDropDown(data, data.ddlFromDate);
	                                    _hideDropDown(data, data.ddlToDate);
	                                }
	                                else if (targetId !== data.fromDayId) {
	                                    _hideDropDown(data, data.ddlFromDate);
	                                }
	                                else {
	                                    _hideDropDown(data, data.ddlToDate);
	                                }
	                            });
	                        }
	
	                        var changeCurrentDay = function (e) {
	                            var $this = $(this),
	                                selectedDay = $this.find('a').html();
	
	                            // prevent conflict when closing day dropdown
	                            if (data.expandDayOnhover) data.isClosingDropDown = true;
	
	                            $this.parents('.dayPickrDropDown').hide();
	                            _selectDay(data, e.data.target, selectedDay, e.data.isFromDay);
	
	                            e.stopPropagation();
	                        };
	
	                        // Change current fromDay/toDay when users select a day
	                        data.ddlFromDate.dropDown.on('click', 'li', { target: data.ddlFromDate, isFromDay: true }, changeCurrentDay);
	                        data.ddlToDate.dropDown.on('click', 'li', { target: data.ddlToDate, isFromDay: false }, changeCurrentDay);
	
	                        //#endregion Events
	                    }
	
	                    //#endregion Day Picker
	
	                    //#endregion INIT DOM ELEMENTS
	                });
	            },
	
	            // Returns the current month or null if the invoker's data does not exist
	            // format may be '' which tell this to use default format
	            getMonth: function (format) {
	                var data = _getData(this);
	                if (!data) return null;
	
	                if (format != undefined) {
	                    format = format.length > 0 ? format : data.monthFormat;
	                    return new Date(data.year, data.month).toStringFormat(format);
	                } else {
	                    return data.month + 1;
	                }
	            },
	
	            // Sets the current month for the month picker.
	            // value may be a Date object or a string but it must be a full date (include year, month & day)
	            setMonth: function (value) {
	                var data = _getData(this);
	                if (!data) return;
	
	                var year, month, _value = value;
	
	                if ((typeof value).toLowerCase() != 'object') {
	                    _value = new Date(_value);
	                }
	
	                year = _value.getFullYear();
	                month = _value.getMonth();
	
	                if (isInteger(year) && isInteger(month)) {
	                    if (_validateMonth(data.minDate, data.maxDate, year, month)) {
	                        _selectMonth(data, year, month);
	                    } else {
	                        logger.error('Oops! Can\'t set month because input (' + value.toString() +
	                            ') is out of range (' + data.minDate.toStringFormat('MMM yyyy') + ' ~ ' + data.maxDate.toStringFormat('MMM yyyy') + ')');
	                    }
	                } else {
	                    logger.error('Oops! Can\'t set month because of invalid input (' + value.toString() + ')');
	                }
	            },
	
	            // Returns the current fromDate or null if the invoker's data does not exist
	            // format may be '' which tell this to use default format
	            getFromDate: function (format) {
	                var data = _getData(this);
	                if (!data) return null;
	
	                if (format != undefined) {
	                    format = format.length > 0 ? format : data.monthFormat;
	                    return data.fromDate.toStringFormat(format);
	                }
	
	                return data.fromDate;
	            },
	
	            // Returns the current toDate or null if the invoker's data does not exist
	            // format may be '' which tell this to use default format
	            getToDate: function (format) {
	                var data = _getData(this);
	                if (!data) return null;
	
	                if (format != undefined) {
	                    format = format.length > 0 ? format : data.monthFormat;
	                    return data.toDate.toStringFormat(format);
	                }
	
	                return data.toDate;
	            },
	
	            // Sets the current fromDay
	            setFromDay: function (value) {
	                var data = _getData(this);
	                if (!data) return;
	
	                var selectedDate = new Date(data.year, data.month, value);
	
	                if (isInteger(value) && _validateDate(data.minDate, data.maxDate, data.year, data.month, selectedDate)) {
	                    _selectDay(data, data.ddlFromDate, value, true);
	                } else {
	                    logger.error('Oops! input value (' + value + ') is invalid');
	                }
	            },
	
	            // Sets the current toDay
	            setToDay: function (value) {
	                var data = _getData(this);
	                if (!data) return;
	
	                var selectedDate = new Date(data.year, data.month, value);
	
	                if (isInteger(value) && _validateDate(data.minDate, data.maxDate, data.year, data.month, selectedDate)) {
	                    _selectDay(data, data.ddlToDate, value, false);
	                } else {
	                    logger.error('Oops! input value (' + value + ') is invalid');
	                }
	            },
	
	            // Resets picker to its data-original initial state
	            reset: function () {
	                var data = _getData(this);
	                if (!data) return;
	
	                _selectMonth(data, data.originalYear, data.originalMonth);
	            }
	        };
	
	        /*
	        * Public methods
	        */
	
	        $.fn.monPickr = function (method) {
	            if (_methods[method]) { // call a method but init
	                return _methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
	            } else if (typeof method === 'object' || !method) { // call init method
	                return _methods.init.apply(this, arguments);
	            } else { //
	                logger.error('Oops! Method "' + method + '" does not exist on jQuery.monPickr');
	            }
	        };
	    })(jQuery);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 258:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7),
	    __webpack_require__(205), __webpack_require__(208), __webpack_require__(209)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, jqGridAdditional) {
	        var GRID_ID = "ReportGrid";
	        var _ajaxRequest = null;
	
	        var _me = {
	            initialize: function () {
	                Site.initialize();
	                Site.addUserGuide({
	                    resourceName: "CustomerLog",
	                    title: Site.getSingleResource("CustomerLog")
	                });
	
	                $(window).on("resize", function () {
	                    _me.resizeGrid();
	                });
	
	                $(window).on("unload", function () {
	                    $(window).off("resize");
	                });
	            },
	
	            loadGrid: function (url, gridOption, hasCheckbox) {
	                var option = {
	                    type: "GET",
	                    url: url,
	                    dataType: "json",
	                    contentType: "application/json",
	                    cache: false,
	                    success: function (response) {
	                        jqGridAdditional.clearGrid(GRID_ID);
	
	                        var data = response.rows;
	                        if (data.length === 0) {
	                            jqGridAdditional.setNoInfo();
	                            return;
	                        }
	
	                        _me.bindData(data, gridOption, hasCheckbox);
	                    },
	                    beforeSend: function () {
	                        Site.blockUI();
	                    },
	                    complete: function () {
	                        Site.unBlockUI();
	                    }
	                }
	
	                _me.stopAjaxRequest();
	                _ajaxRequest = $.ajax(option);
	            },
	
	            bindData: function (data, gridOption, hasCheckbox) {
	                gridOption.data = data;
	                gridOption.gridComplete = function () {
	                    if (hasCheckbox) {
	                        _me.checkShowAll();
	                    }
	
	                    jqGridAdditional.gridComplete();
	                    _me.resizeGrid();
	                };
	
	                $('#ReportGrid').jqGrid(gridOption);
	            },
	
	            resizeGrid: function () {
	                var $grid = $("#" + GRID_ID);
	                jqGridAdditional.updateGridHeight();
	                jqGridAdditional.updateGridWidth($grid);
	            },
	
	            checkShowAll: function () {
	                var checked = $('#showall').is(':checked');
	                $('.old-value').toggleClass("hide", !checked);
	            },
	
	            stopAjaxRequest: function () {
	                if (_ajaxRequest !== null) {
	                    _ajaxRequest.abort();
	                }
	            }
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 259:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(260)], __WEBPACK_AMD_DEFINE_RESULT__ = function (GroupCommission) {
	    $(GroupCommission.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 260:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(253), __webpack_require__(255),
	    __webpack_require__(256), __webpack_require__(258)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, AutoComplete, CommonOption, MonthPicker, CustomerLogService) {
	        var _me = {
	            initialize: function () {
	                CustomerLogService.initialize();
	                MonthPicker.initMonthPicker({ minDate: $('#MinDate').val() });
	                _me.initEvent();
	
	                // Set menu active
	                $('#groupcommlogTab').addClass('active');
	            },
	
	            initEvent: function () {
	                $('#SubmitButton').on('click', function () {
	                    _me.loadGrid();
	                });
	
	                $('#showall').on('change', function () {
	                    CustomerLogService.checkShowAll();
	                });
	            },
	
	            loadGrid: function () {
	                var gridOption = CommonOption.gridOption;
	                var url = _me.getUrl();
	
	                var colNames = Site.clone(CommonOption.getColNames());
	                gridOption.colNames = Site.getResources(colNames);
	
	                var hasCheckbox = true;
	                CustomerLogService.loadGrid(url, gridOption, hasCheckbox);
	            },
	
	            getUrl: function () {
	                var fromDate = new Date($('#FromDate').val());
	                var toDate = new Date($('#ToDate').val());
	
	                return Site.resolveClientUrl('api/Log/GroupCommission?'
	                    + '&Year=' + fromDate.getFullYear() + '&Month=' + (fromDate.getMonth() + 1)
	                    + '&FromDay=' + fromDate.getDate() + '&ToDay=' + toDate.getDate());
	            }
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }

});
//# sourceMappingURL=groupcommission.chunk.js.map