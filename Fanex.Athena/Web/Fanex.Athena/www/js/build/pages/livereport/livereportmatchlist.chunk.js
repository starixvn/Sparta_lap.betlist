<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
webpackJsonp([17],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(214);


/***/ },

/***/ 213:
/***/ function(module, exports, __webpack_require__) {

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

/***/ },

/***/ 214:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(215)], __WEBPACK_AMD_DEFINE_RESULT__ = function (MatchList) {
	    "use strict";
	    $(MatchList.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 215:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(8), __webpack_require__(7), __webpack_require__(213)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString, Site, Helper) {
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
	
	        $(productOptions).each(function (index, element) {
	            url = Site.setProductOptionParam(url, index, element.toUpperCase(), true);
	        });
	
	        Site.blockUI();
	        Site.redirect(url);
	    }
	
	    return {
	        initialize: initialize
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }

});
//# sourceMappingURL=livereportmatchlist.chunk.js.map
=======
webpackJsonp([18],{0:function(t,e,a){t.exports=a(214)},213:function(t,e,a){var r,n;r=[a(8)],n=function(t){"use strict";function e(e,a){return e=t.setParam(e,"totalrows",a.BetCount),e=t.setParam(e,"sumwinlost",a.CustomerWinLoss),e=t.setParam(e,"sumplayercomm",a.CustomerComm),e=t.setParam(e,"sumlicenseewl",a.LicenseeWinLoss),e=t.setParam(e,"sumcomm",a.LicenseeComm),e=t.setParam(e,"sumturnover",a.TurnOver),e=t.setParam(e,"sumamtdueto3rd",a.AmountDue)}function a(t,e){t=Number(t);var a=[];return t>0?180===t||186===t?a.push("VS"):162===t?a.push("LC"):("SB"!==e&&"ALL"!==e||a.push("SB"),"BA"!==e&&"ALL"!==e||a.push("BA")):"ALL"===e?a=["SB","BA","NG","RB","LC","VS","CS","BG"]:a.push(e),a}return{setSumParams:e,getMatchListProductOption:a}}.apply(e,r),!(void 0!==n&&(t.exports=n))},214:function(t,e,a){var r,n;(function(i){r=[a(215)],n=function(t){"use strict";i(t.initialize)}.apply(e,r),!(void 0!==n&&(t.exports=n))}).call(e,a(1))},215:function(t,e,a){var r,n;(function(i){r=[a(8),a(7),a(213)],n=function(t,e,a){"use strict";function r(){e.initialize(),n()}function n(){i("body").on("click",".match-detail-link",function(){o(this)})}function o(r){var n=e.resolveClientUrl("LiveReportBetList/Match"),o=i("#CurrencyName").attr("data-original"),s=i(r).attr("data-drill-down");s=i.parseJSON(s),n=e.setLicenseeOptionOriginalParam(n),n=e.setDateRangeOriginalParam(n),n=t.setParam(n,"CurrencyName",o),n=t.setParam(n,"MatchId",s.MatchId),n=t.setParam(n,"HomeId",s.HomeId),n=t.setParam(n,"AwayId",s.AwayId),n=t.setParam(n,"BetType",s.BetType),n=t.setParam(n,"LiveIndicator",s.LiveIndicator),n=a.setSumParams(n,s);var u=a.getMatchListProductOption(i("#SportId").val(),i("#ProductCode").val());i(u).each(function(t,a){n=e.setProductOptionParam(n,t,a.toUpperCase(),!0)}),e.blockUI(),e.redirect(n)}return{initialize:r}}.apply(e,r),!(void 0!==n&&(t.exports=n))}).call(e,a(1))}});
>>>>>>> 12a6046... #67998 - Add Handicap Over Under Report
=======
webpackJsonp([18],{0:function(t,e,a){t.exports=a(216)},215:function(t,e,a){var r,n;r=[a(8)],n=function(t){"use strict";function e(e,a){return e=t.setParam(e,"totalrows",a.BetCount),e=t.setParam(e,"sumwinlost",a.CustomerWinLoss),e=t.setParam(e,"sumplayercomm",a.CustomerComm),e=t.setParam(e,"sumlicenseewl",a.LicenseeWinLoss),e=t.setParam(e,"sumcomm",a.LicenseeComm),e=t.setParam(e,"sumturnover",a.TurnOver),e=t.setParam(e,"sumamtdueto3rd",a.AmountDue)}function a(t,e){t=Number(t);var a=[];return t>0?180===t||186===t?a.push("VS"):162===t?a.push("LC"):("SB"!==e&&"ALL"!==e||a.push("SB"),"BA"!==e&&"ALL"!==e||a.push("BA")):"ALL"===e?a=["SB","BA","NG","RB","LC","VS","CS","BG"]:a.push(e),a}return{setSumParams:e,getMatchListProductOption:a}}.apply(e,r),!(void 0!==n&&(t.exports=n))},216:function(t,e,a){var r,n;(function(i){r=[a(217)],n=function(t){"use strict";i(t.initialize)}.apply(e,r),!(void 0!==n&&(t.exports=n))}).call(e,a(1))},217:function(t,e,a){var r,n;(function(i){r=[a(8),a(7),a(215)],n=function(t,e,a){"use strict";function r(){e.initialize(),n()}function n(){i("body").on("click",".match-detail-link",function(){o(this)})}function o(r){var n=e.resolveClientUrl("LiveReportBetList/Match"),o=i("#CurrencyName").attr("data-original"),s=i(r).attr("data-drill-down");s=i.parseJSON(s),n=e.setLicenseeOptionOriginalParam(n),n=e.setDateRangeOriginalParam(n),n=t.setParam(n,"CurrencyName",o),n=t.setParam(n,"MatchId",s.MatchId),n=t.setParam(n,"HomeId",s.HomeId),n=t.setParam(n,"AwayId",s.AwayId),n=t.setParam(n,"BetType",s.BetType),n=t.setParam(n,"LiveIndicator",s.LiveIndicator),n=a.setSumParams(n,s);var u=a.getMatchListProductOption(i("#SportId").val(),i("#ProductCode").val());i(u).each(function(t,a){n=e.setProductOptionParam(n,t,a.toUpperCase(),!0)}),e.blockUI(),e.redirect(n)}return{initialize:r}}.apply(e,r),!(void 0!==n&&(t.exports=n))}).call(e,a(1))}});
>>>>>>> e5a9a5d... #67998 - Show data table in Handicap Over Under Report
=======
webpackJsonp([19],{0:function(t,e,a){t.exports=a(221)},220:function(t,e,a){var r,n;r=[a(8)],n=function(t){"use strict";function e(e,a){return e=t.setParam(e,"totalrows",a.BetCount),e=t.setParam(e,"sumwinlost",a.CustomerWinLoss),e=t.setParam(e,"sumplayercomm",a.CustomerComm),e=t.setParam(e,"sumlicenseewl",a.LicenseeWinLoss),e=t.setParam(e,"sumcomm",a.LicenseeComm),e=t.setParam(e,"sumturnover",a.TurnOver),e=t.setParam(e,"sumamtdueto3rd",a.AmountDue)}function a(t,e){t=Number(t);var a=[];return t>0?180===t||186===t?a.push("VS"):162===t?a.push("LC"):("SB"!==e&&"ALL"!==e||a.push("SB"),"BA"!==e&&"ALL"!==e||a.push("BA")):"ALL"===e?a=["SB","BA","NG","RB","LC","VS","CS","BG"]:a.push(e),a}return{setSumParams:e,getMatchListProductOption:a}}.apply(e,r),!(void 0!==n&&(t.exports=n))},221:function(t,e,a){var r,n;(function(i){r=[a(222)],n=function(t){"use strict";i(t.initialize)}.apply(e,r),!(void 0!==n&&(t.exports=n))}).call(e,a(1))},222:function(t,e,a){var r,n;(function(i){r=[a(8),a(7),a(220)],n=function(t,e,a){"use strict";function r(){e.initialize(),n()}function n(){i("body").on("click",".match-detail-link",function(){o(this)})}function o(r){var n=e.resolveClientUrl("LiveReportBetList/Match"),o=i("#CurrencyName").attr("data-original"),s=i(r).attr("data-drill-down");s=i.parseJSON(s),n=e.setLicenseeOptionOriginalParam(n),n=e.setDateRangeOriginalParam(n),n=t.setParam(n,"CurrencyName",o),n=t.setParam(n,"MatchId",s.MatchId),n=t.setParam(n,"HomeId",s.HomeId),n=t.setParam(n,"AwayId",s.AwayId),n=t.setParam(n,"BetType",s.BetType),n=t.setParam(n,"LiveIndicator",s.LiveIndicator),n=a.setSumParams(n,s);var u=a.getMatchListProductOption(i("#SportId").val(),i("#ProductCode").val());i(u).each(function(t,a){n=e.setProductOptionParam(n,t,a.toUpperCase(),!0)}),e.blockUI(),e.redirect(n)}return{initialize:r}}.apply(e,r),!(void 0!==n&&(t.exports=n))}).call(e,a(1))}});
>>>>>>> 8e70b06... #67998 - Add Button to table and update for calling initMaterialSelect function in site.js
