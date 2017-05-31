webpackJsonp([45],{

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(234);


/***/ }),

/***/ 234:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(235)], __WEBPACK_AMD_DEFINE_RESULT__ = function (CustomerCommGroupList) {
	    "use strict";
	
	    $(CustomerCommGroupList.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }),

/***/ 235:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(236)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, GridHelper) {
	        "use strict";
	
	        function initialize() {
	            Site.initialize();
	            Site.addUserGuide({
	                resourceName: "CommissionGroupList",
	                title: Site.getSingleResource("CommissionGroupList")
	            });
	
	            _me.initEvent();
	        }
	
	        function initEvent() {
	
	        }
	
	        var _me = {
	            initialize: initialize,
	            initEvent: initEvent
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 236:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	    var _me = {
	        resizeGrid: function ($scrollableContent) {
	            var $grid = $scrollableContent || $(".body-content");
	            if ($grid.length === 0) {
	                return;
	            }
	
	            _me.updateHeight($grid);
	            _me.updateWidth($grid);
	        },
	
	        updateHeight: function ($grid) {
	            var gridTop = $grid.offset().top;
	            var footerSite = 18;
	            var footerHeight = $(".footer-content").length > 0 ? $(".footer-content").height() : 0;
	            var pagingHeight = $(".pagingContainer").length > 0 ? $(".pagingContainer").height() : 0;
	
	            var gridHeight = window.innerHeight - gridTop - footerHeight - pagingHeight - footerSite - 10;
	            gridHeight = Math.max(150, gridHeight);
	            $grid.css("max-height", gridHeight);
	        },
	
	        updateWidth: function ($grid) {
	            if (!$grid.hasVerticalScrollBar()) {
	                $grid.addClass("noScroll");
	            }
	            else {
	                $grid.find("table").width($grid.width() - 17);
	            }
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ })

});
//# sourceMappingURL=commgrouplist.chunk.js.map