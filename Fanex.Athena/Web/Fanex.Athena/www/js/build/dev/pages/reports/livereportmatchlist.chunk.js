webpackJsonp([65],{

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(308);


/***/ }),

/***/ 307:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(8)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString) {
	    "use strict";
	    function setSumParams(url, totalValue) {
	        url = QueryString.setParam(url, "totalrows", totalValue.BetCount);
	        url = QueryString.setParam(url, "sumwinlost", totalValue.CustomerWinLoss);
	        url = QueryString.setParam(url, "sumplayercomm", totalValue.CustomerComm);
	        url = QueryString.setParam(url, "sumlicenseewl", totalValue.LicenseeWinLoss);
	        url = QueryString.setParam(url, "sumcomm", totalValue.LicenseeComm);
	        url = QueryString.setParam(url, "sumturnover", totalValue.TurnOver);
	        url = QueryString.setParam(url, "sumamtdueto3rd", totalValue.AmountDue);
	
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
	                productOptions = ["SB", "BA", "NG", "RB", "LC", "VS", "CS", "BG"];
	            }
	            else {
	                productOptions.push(productCode);
	            }
	        }
	
	        return productOptions;
	    }
	
	    return {
	        setSumParams: setSumParams,
	        getMatchListProductOption: getMatchListProductOption,
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 308:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(309)], __WEBPACK_AMD_DEFINE_RESULT__ = function (MatchList) {
	    "use strict";
	    $(MatchList.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }),

/***/ 309:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(8), __webpack_require__(7), __webpack_require__(307)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString, Site, Helper) {
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

/***/ })

});
//# sourceMappingURL=livereportmatchlist.chunk.js.map