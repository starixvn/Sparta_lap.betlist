webpackJsonp([59],{

<<<<<<< HEAD
/***/ 165:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(118)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString) {
    "use strict";
    function setSumParams(url, totalValue) {
        url = QueryString.setParam(url, "totalrows", totalValue.BetCount);
        url = QueryString.setParam(url, "sumwinlost", totalValue.CustomerWinLoss);
        url = QueryString.setParam(url, "sumplayercomm", totalValue.CustomerComm);
        url = QueryString.setParam(url, "sumlicenseewl", totalValue.LicenseeWinLoss);
        url = QueryString.setParam(url, "sumcomm", totalValue.LicenseeComm);
        url = QueryString.setParam(url, "sumturnover", totalValue.TurnOver);
        url = QueryString.setParam(url, "sumamtdueto3rd", totalValue.AmountDue);
=======
/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

<<<<<<< HEAD
	module.exports = __webpack_require__(334);
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

        return url;
    }

    function getMatchListProductOption(sportId, productCode) {
        sportId = Number(sportId);
        var productOptions = [];
        if (sportId > 0) {
            if (sportId === 180 || sportId === 186) {
                productOptions.push("VS");
            }
            else if (sportId === 162) {
                productOptions.push("LC");
            }
            else if (sportId === 190 || sportId === 191 || sportId === 104) {
                productOptions.push("VS2");
            }
            else {
                if (productCode === "SB" || productCode === "ALL") {
                    productOptions.push("SB");
                }
                if (productCode === "BA" || productCode === "ALL") {
                    productOptions.push("BA");
                }
            }
        }
        else {
            if (productCode === "ALL") {
                productOptions = ["SB", "BA", "NG", "RB", "LC", "VS", "CS", "BG", "VS2"];
            }
            else {
                productOptions.push(productCode);
            }
        }

        return productOptions;
    }
=======
	module.exports = __webpack_require__(345);
>>>>>>> 62e0118d7... 76246 - Build front-end code

    return {
        setSumParams: setSumParams,
        getMatchListProductOption: getMatchListProductOption,
    };
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

<<<<<<< HEAD
<<<<<<< HEAD
/***/ 234:
/***/ (function(module, exports, __webpack_require__) {
=======
/***/ 333:
/***/ function(module, exports, __webpack_require__) {
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

module.exports = __webpack_require__(235);
=======
/***/ 344:
/***/ (function(module, exports, __webpack_require__) {
>>>>>>> 62e0118d7... 76246 - Build front-end code


/***/ }),

<<<<<<< HEAD
<<<<<<< HEAD
/***/ 235:
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(236)], __WEBPACK_AMD_DEFINE_RESULT__ = function (MatchList) {
    "use strict";
    $(MatchList.initialize);
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))
=======
/***/ 334:
/***/ function(module, exports, __webpack_require__) {
=======
/***/ 345:
/***/ (function(module, exports, __webpack_require__) {
>>>>>>> 62e0118d7... 76246 - Build front-end code

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(346)], __WEBPACK_AMD_DEFINE_RESULT__ = function (MatchList) {
	    "use strict";
	    $(MatchList.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ }),

<<<<<<< HEAD
<<<<<<< HEAD
/***/ 236:
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(118), __webpack_require__(2), __webpack_require__(165)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString, Site, Helper) {
    "use strict";

    function initialize() {
        Site.initialize();
        initEvent();
    }

    function initEvent() {
        $("body").on("click", ".match-detail-link", function () {
            viewMatchBetList(this);
        });
    }

    function viewMatchBetList(target) {
        var url = Site.resolveClientUrl("LiveReportBetList/Match");

        var currencyName = $('#CurrencyName').attr('data-original');
        var data = $(target).attr("data-drill-down");
        data = $.parseJSON(data);
        url = Site.setLicenseeOptionOriginalParam(url);
        url = Site.setDateRangeOriginalParam(url);
        url = QueryString.setParam(url, "CurrencyName", currencyName);
        url = QueryString.setParam(url, "MatchId", data.MatchId);
        url = QueryString.setParam(url, "HomeId", data.HomeId);
        url = QueryString.setParam(url, "AwayId", data.AwayId);
        url = QueryString.setParam(url, "BetType", data.BetType);
        url = QueryString.setParam(url, "LiveIndicator", data.LiveIndicator);
        url = Helper.setSumParams(url, data);

        var productOptions = Helper.getMatchListProductOption($("#SportId").val(), $("#ProductCode").val());

        url = Site.setProductCodeListParam(url, productOptions);

        Site.blockUI();
        Site.redirect(url);
    }

    return {
        initialize: initialize
    };
}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))
=======
/***/ 335:
/***/ function(module, exports, __webpack_require__) {
=======
/***/ 346:
/***/ (function(module, exports, __webpack_require__) {
>>>>>>> 62e0118d7... 76246 - Build front-end code

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(8), __webpack_require__(7), __webpack_require__(344)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString, Site, Helper) {
	    "use strict";
	
	    function initialize() {
	        Site.initialize();
	        initEvent();
	    }
	
	    function initEvent() {
	        $("body").on("click", ".match-detail-link", function () {
	            viewMatchBetList(this);
	        });
	    }
	
	    function viewMatchBetList(target) {
	        var url = Site.resolveClientUrl("LiveReportBetList/Match");
	
	        var currencyName = $('#CurrencyName').attr('data-original');
	        var data = $(target).attr("data-drill-down");
	        data = $.parseJSON(data);
	        url = Site.setLicenseeOptionOriginalParam(url);
	        url = Site.setDateRangeOriginalParam(url);
	        url = QueryString.setParam(url, "CurrencyName", currencyName);
	        url = QueryString.setParam(url, "MatchId", data.MatchId);
	        url = QueryString.setParam(url, "HomeId", data.HomeId);
	        url = QueryString.setParam(url, "AwayId", data.AwayId);
	        url = QueryString.setParam(url, "BetType", data.BetType);
	        url = QueryString.setParam(url, "LiveIndicator", data.LiveIndicator);
	        url = Helper.setSumParams(url, data);
	
	        var productOptions = Helper.getMatchListProductOption($("#SportId").val(), $("#ProductCode").val());
	
	        url = Site.setProductCodeListParam(url, productOptions);
	
	        Site.blockUI();
	        Site.redirect(url);
	    }
	
	    return {
	        initialize: initialize
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))
>>>>>>> babeb522f... 76246 - Rebuild front-end pkg

/***/ })

},[234]);
//# sourceMappingURL=livereportmatchlist.chunk.js.map