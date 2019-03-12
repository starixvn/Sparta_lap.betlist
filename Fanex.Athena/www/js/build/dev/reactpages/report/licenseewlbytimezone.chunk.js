/******/ (function(modules) { // webpackBootstrap
/******/ 	// install a JSONP callback for chunk loading
/******/ 	function webpackJsonpCallback(data) {
/******/ 		var chunkIds = data[0];
/******/ 		var moreModules = data[1];
/******/ 		var executeModules = data[2];
/******/
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0, resolves = [];
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(installedChunks[chunkId]) {
/******/ 				resolves.push(installedChunks[chunkId][0]);
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 		for(moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				modules[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(parentJsonpFunction) parentJsonpFunction(data);
/******/
/******/ 		while(resolves.length) {
/******/ 			resolves.shift()();
/******/ 		}
/******/
/******/ 		// add entry modules from loaded chunk to deferred list
/******/ 		deferredModules.push.apply(deferredModules, executeModules || []);
/******/
/******/ 		// run deferred modules when all chunks ready
/******/ 		return checkDeferredModules();
/******/ 	};
/******/ 	function checkDeferredModules() {
/******/ 		var result;
/******/ 		for(var i = 0; i < deferredModules.length; i++) {
/******/ 			var deferredModule = deferredModules[i];
/******/ 			var fulfilled = true;
/******/ 			for(var j = 1; j < deferredModule.length; j++) {
/******/ 				var depId = deferredModule[j];
/******/ 				if(installedChunks[depId] !== 0) fulfilled = false;
/******/ 			}
/******/ 			if(fulfilled) {
/******/ 				deferredModules.splice(i--, 1);
/******/ 				result = __webpack_require__(__webpack_require__.s = deferredModule[0]);
/******/ 			}
/******/ 		}
/******/ 		return result;
/******/ 	}
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// object to store loaded and loading chunks
/******/ 	// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 	// Promise = chunk loading, 0 = chunk loaded
/******/ 	var installedChunks = {
/******/ 		"www/js/build/dev/reactpages/report/licenseewlbytimezone": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
/******/
/******/ 	// script path function
/******/ 	function jsonpScriptSrc(chunkId) {
/******/ 		return __webpack_require__.p + "www/js/build/dev/reactpages/lib/" + ({"xlsx-style-sparta":"xlsx-style-sparta"}[chunkId]||chunkId) + ".chunk.js"
/******/ 	}
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/ 	// This file contains only the entry chunk.
/******/ 	// The chunk loading function for additional chunks
/******/ 	__webpack_require__.e = function requireEnsure(chunkId) {
/******/ 		var promises = [];
/******/
/******/
/******/ 		// JSONP chunk loading for javascript
/******/
/******/ 		var installedChunkData = installedChunks[chunkId];
/******/ 		if(installedChunkData !== 0) { // 0 means "already installed".
/******/
/******/ 			// a Promise means "currently loading".
/******/ 			if(installedChunkData) {
/******/ 				promises.push(installedChunkData[2]);
/******/ 			} else {
/******/ 				// setup Promise in chunk cache
/******/ 				var promise = new Promise(function(resolve, reject) {
/******/ 					installedChunkData = installedChunks[chunkId] = [resolve, reject];
/******/ 				});
/******/ 				promises.push(installedChunkData[2] = promise);
/******/
/******/ 				// start chunk loading
/******/ 				var head = document.getElementsByTagName('head')[0];
/******/ 				var script = document.createElement('script');
/******/ 				var onScriptComplete;
/******/
/******/ 				script.charset = 'utf-8';
/******/ 				script.timeout = 120;
/******/ 				if (__webpack_require__.nc) {
/******/ 					script.setAttribute("nonce", __webpack_require__.nc);
/******/ 				}
/******/ 				script.src = jsonpScriptSrc(chunkId);
/******/
/******/ 				onScriptComplete = function (event) {
/******/ 					// avoid mem leaks in IE.
/******/ 					script.onerror = script.onload = null;
/******/ 					clearTimeout(timeout);
/******/ 					var chunk = installedChunks[chunkId];
/******/ 					if(chunk !== 0) {
/******/ 						if(chunk) {
/******/ 							var errorType = event && (event.type === 'load' ? 'missing' : event.type);
/******/ 							var realSrc = event && event.target && event.target.src;
/******/ 							var error = new Error('Loading chunk ' + chunkId + ' failed.\n(' + errorType + ': ' + realSrc + ')');
/******/ 							error.type = errorType;
/******/ 							error.request = realSrc;
/******/ 							chunk[1](error);
/******/ 						}
/******/ 						installedChunks[chunkId] = undefined;
/******/ 					}
/******/ 				};
/******/ 				var timeout = setTimeout(function(){
/******/ 					onScriptComplete({ type: 'timeout', target: script });
/******/ 				}, 120000);
/******/ 				script.onerror = script.onload = onScriptComplete;
/******/ 				head.appendChild(script);
/******/ 			}
/******/ 		}
/******/ 		return Promise.all(promises);
/******/ 	};
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// on error function for async loading
/******/ 	__webpack_require__.oe = function(err) { console.error(err); throw err; };
/******/
/******/ 	var jsonpArray = window["webpackJsonp"] = window["webpackJsonp"] || [];
/******/ 	var oldJsonpFunction = jsonpArray.push.bind(jsonpArray);
/******/ 	jsonpArray.push = webpackJsonpCallback;
/******/ 	jsonpArray = jsonpArray.slice();
/******/ 	for(var i = 0; i < jsonpArray.length; i++) webpackJsonpCallback(jsonpArray[i]);
/******/ 	var parentJsonpFunction = oldJsonpFunction;
/******/
/******/
/******/ 	// add entry module to deferred list
/******/ 	deferredModules.push(["./www/js/source/reactpages/pages/licenseewl-by-timezone/components/index.jsx","common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./cptable":
/*!**************************!*\
  !*** external "cptable" ***!
  \**************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = cptable;

/***/ }),

/***/ "./jszip":
/*!************************!*\
  !*** external "jszip" ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = jszip;

/***/ }),

/***/ "./ods":
/*!**********************!*\
  !*** external "ods" ***!
  \**********************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ods;

/***/ }),

/***/ "./www/js/source/reactpages/actions/baseCurrencyAction.js":
/*!****************************************************************!*\
  !*** ./www/js/source/reactpages/actions/baseCurrencyAction.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setBaseCurrencyList = exports.setSelectedBaseCurrency = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedBaseCurrency = exports.setSelectedBaseCurrency = function setSelectedBaseCurrency(baseCurrency) {
    return {
        type: types.SET_SELECTED_BASECURRENCY,
        data: baseCurrency
    };
};

var setBaseCurrencyList = exports.setBaseCurrencyList = function setBaseCurrencyList(baseCurrencyList) {
    return {
        type: types.SET_BASECURRENCY_LIST,
        data: baseCurrencyList
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/currencyAction.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/actions/currencyAction.js ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setSelectedCurrency = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedCurrency = exports.setSelectedCurrency = function setSelectedCurrency(item) {
    return {
        type: types.SET_SELECTED_CURRENCY,
        data: item
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/excludeTestAction.js":
/*!***************************************************************!*\
  !*** ./www/js/source/reactpages/actions/excludeTestAction.js ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.changeIsExcludeTest = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var changeIsExcludeTest = exports.changeIsExcludeTest = function changeIsExcludeTest(checked) {
    return {
        type: types.CHANGE_IS_EXCLUDE_TEST,
        data: checked
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/loaderAction.js":
/*!**********************************************************!*\
  !*** ./www/js/source/reactpages/actions/loaderAction.js ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.toggleLoader = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var toggleLoader = exports.toggleLoader = function toggleLoader(show) {
    return {
        type: types.TOGGLE_LOADER,
        data: show
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/productAction.js":
/*!***********************************************************!*\
  !*** ./www/js/source/reactpages/actions/productAction.js ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setSelectedProduct = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedProduct = exports.setSelectedProduct = function setSelectedProduct(product) {
    return {
        type: types.SET_SELECTED_PRODUCT,
        data: product
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/types.js":
/*!***************************************************!*\
  !*** ./www/js/source/reactpages/actions/types.js ***!
  \***************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var FETCH_DATA = exports.FETCH_DATA = 'FETCH_DATA';
var SET_GRID_DATA = exports.SET_GRID_DATA = "SET_GRID_DATA";

/* Product */
var SET_SELECTED_PRODUCT = exports.SET_SELECTED_PRODUCT = "SET_SELECTED_PRODUCT";

/* Base Currency */
var SET_SELECTED_BASECURRENCY = exports.SET_SELECTED_BASECURRENCY = "SET_SELECTED_BASECURRENCY";
var SET_BASECURRENCY_LIST = exports.SET_BASECURRENCY_LIST = "SET_BASECURRENCY_LIST";

/* Currency */
var SET_SELECTED_CURRENCY = exports.SET_SELECTED_CURRENCY = "SET_SELECTED_CURRENCY";

/* Loader */
var TOGGLE_LOADER = exports.TOGGLE_LOADER = "TOGGLE_LOADER";

/* Exclude Test Data */
var CHANGE_IS_EXCLUDE_TEST = exports.CHANGE_IS_EXCLUDE_TEST = "CHANGE_IS_EXCLUDE_TEST";

/* User mini guide */
var USER_GUIDE_CONTENT = exports.USER_GUIDE_CONTENT = "USER_GUIDE_CONTENT;";

/* Date range */
var SET_DATE_RANGE = exports.SET_DATE_RANGE = 'SET_DATE_RANGE';
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = 'GET_DATE_CONSTRAINT';

/* Convert all figures to base currency */
var CHANGE_IS_CONVERT_ALL = exports.CHANGE_IS_CONVERT_ALL = "CHANGE_IS_CONVERT_ALL";

/* Status */
var SET_SELECTED_STATUS = exports.SET_SELECTED_STATUS = "SET_SELECTED_STATUS";

/* Platform  */
var SET_SELECTED_PLATFORM = exports.SET_SELECTED_PLATFORM = "SET_SELECTED_PLATFORM";

/* Live Status  */
var SET_SELECTED_LIVESTATUS = exports.SET_SELECTED_LIVESTATUS = "SET_SELECTED_LIVESTATUS";

var SET_USERNAME = exports.SET_USERNAME = "SET_USERNAME";

var SET_REQUEST_PARAM = exports.SET_REQUEST_PARAM = "SET_REQUEST_PARAM";

var SET_REPORT_TITLE = exports.SET_REPORT_TITLE = "SET_REPORT_TITLE";

/***/ }),

/***/ "./www/js/source/reactpages/actions/userGuideAction.js":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/actions/userGuideAction.js ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setUserGuideContent = undefined;
exports.getUserGuideContent = getUserGuideContent;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

var _site = __webpack_require__(/*! ../lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var fetchData = function fetchData(resourceName) {
    var url = _site2.default.resolveClientUrl('UserGuide/Index?resourcename=' + resourceName) + "&_=" + new Date().getTime();;
    return _axios2.default.get(url, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    });
};

function getUserGuideContent(params) {
    return function (dispatch) {
        fetchData(params).then(function (response) {
            dispatch(setUserGuideContent(response.data));
        });
    };
}

var setUserGuideContent = exports.setUserGuideContent = function setUserGuideContent(data) {
    return {
        type: types.USER_GUIDE_CONTENT,
        data: data
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/components/Loader.jsx":
/*!********************************************************!*\
  !*** ./www/js/source/reactpages/components/Loader.jsx ***!
  \********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var Loader = function Loader(_ref) {
    var isLoading = _ref.isLoading;

    return _react2.default.createElement(
        'div',
        { className: 'loading-process default', style: { display: '' + (isLoading ? 'block' : 'none') } },
        _react2.default.createElement(
            'div',
            { className: 'icon-loading' },
            _react2.default.createElement(
                'div',
                { className: 'loader-inner ball-beat' },
                _react2.default.createElement('div', null),
                _react2.default.createElement('div', null),
                _react2.default.createElement('div', null)
            )
        )
    );
};

exports.default = Loader;

/***/ }),

/***/ "./www/js/source/reactpages/components/NoDataTitle.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/NoDataTitle.jsx ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _langHelper = __webpack_require__(/*! ../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var NoDataTitle = function NoDataTitle() {
    return _react2.default.createElement(
        'p',
        { className: 'no-info-row center-align' },
        _langHelper2.default.getSingleResource("NoInfo")
    );
};

exports.default = NoDataTitle;

/***/ }),

/***/ "./www/js/source/reactpages/components/Overlay.jsx":
/*!*********************************************************!*\
  !*** ./www/js/source/reactpages/components/Overlay.jsx ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _classnames = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");

var _classnames2 = _interopRequireDefault(_classnames);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var wrapperClasses = {
    OVERLAY: "overlay",
    OVERLAY_OPEN: "overlay-open"
};

var Overlay = function (_Component) {
    _inherits(Overlay, _Component);

    function Overlay(props) {
        _classCallCheck(this, Overlay);

        var _this = _possibleConstructorReturn(this, (Overlay.__proto__ || Object.getPrototypeOf(Overlay)).call(this, props));

        _this._unRegisterEvents = function () {
            var closeOnOutsideClick = _this.props.closeOnOutsideClick;


            if (closeOnOutsideClick) {
                document.removeEventListener('click', _this._handleClickOutSide, true);
            }
        };

        _this._handleClickOutSide = function (event) {
            var clickOutside = _this.containerElement && !_this.containerElement.contains(event.target);

            if (clickOutside && _this.state.isOpening) {
                _this._close();
            }
        };

        _this.containerElement = null;
        _this.refHandlers = {
            container: function container(ref) {
                _this.containerElement = ref;
            }
        };

        _this.state = {
            isOpening: _this.props.isOpening
        };
        return _this;
    }

    _createClass(Overlay, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            if (this.props.isOpening !== prevProps.isOpening) {
                var onOpen = this.props.onOpen;

                this.props.isOpening && onOpen && onOpen();
                this.setState({ isOpening: this.props.isOpening });
            }
        }
    }, {
        key: 'componentDidMount',
        value: function componentDidMount() {
            this._registerEvents();
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            this._unRegisterEvents();
        }
    }, {
        key: '_registerEvents',
        value: function _registerEvents() {
            var closeOnOutsideClick = this.props.closeOnOutsideClick;


            if (closeOnOutsideClick) {
                document.addEventListener('click', this._handleClickOutSide, true);
            }
        }
    }, {
        key: '_close',
        value: function _close() {
            var onClose = this.props.onClose;

            this.setState({ isOpening: false }, function () {
                return onClose && onClose();
            });
        }
    }, {
        key: 'render',
        value: function render() {
            var _props = this.props,
                className = _props.className,
                children = _props.children;

            var mergedClassName = (0, _classnames2.default)(wrapperClasses.OVERLAY, _defineProperty({}, wrapperClasses.OVERLAY_OPEN, this.state.isOpening), className);
            var style = {
                display: this.state.isOpening ? "block" : "none"
            };

            return _react2.default.createElement(
                'div',
                { className: mergedClassName, style: style, ref: this.refHandlers.container },
                children
            );
        }
    }]);

    return Overlay;
}(_react.Component);

Overlay.propTypes = {
    closeOnOutsideClick: _propTypes2.default.bool,
    isOpening: _propTypes2.default.bool,
    onOpen: _propTypes2.default.func,
    onClose: _propTypes2.default.func
};
Overlay.defaultProps = {
    closeOnOutsideClick: true,
    isOpening: false
};
exports.default = Overlay;

/***/ }),

/***/ "./www/js/source/reactpages/components/ReportTitle.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/ReportTitle.jsx ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var ReportTitle = function ReportTitle(_ref) {
    var content = _ref.content;

    return _react2.default.createElement(
        "div",
        { className: "report-title-container" },
        _react2.default.createElement(
            "p",
            { className: "report-title" },
            content
        )
    );
};

exports.default = ReportTitle;

/***/ }),

/***/ "./www/js/source/reactpages/components/UserGuide.jsx":
/*!***********************************************************!*\
  !*** ./www/js/source/reactpages/components/UserGuide.jsx ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _Popup = __webpack_require__(/*! ./popup/Popup */ "./www/js/source/reactpages/components/popup/Popup.jsx");

var _Popup2 = _interopRequireDefault(_Popup);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var UserGuide = function (_React$Component) {
    _inherits(UserGuide, _React$Component);

    function UserGuide(props) {
        _classCallCheck(this, UserGuide);

        var _this = _possibleConstructorReturn(this, (UserGuide.__proto__ || Object.getPrototypeOf(UserGuide)).call(this, props));

        _this.onClickHandler = function () {
            _this.setState({
                isShow: true
            });

            _this.props.onClick && _this.props.onClick();
        };

        _this.onCloseHandler = function () {
            _this.setState({
                isShow: false
            });
        };

        _this.state = {
            isShow: false
        };
        return _this;
    }

    _createClass(UserGuide, [{
        key: 'render',
        value: function render() {
            var _props = this.props,
                className = _props.className,
                title = _props.title,
                content = _props.content,
                contentTitle = _props.contentTitle;

            var isShow = this.state.isShow && content.length > 0;

            return _react2.default.createElement(
                'div',
                null,
                _react2.default.createElement('i', { className: className, onClick: this.onClickHandler, title: title }),
                _react2.default.createElement(
                    _Popup2.default,
                    { width: 640, height: 480, isShow: isShow, title: contentTitle, onClose: this.onCloseHandler, isHtml: true },
                    content
                )
            );
        }
    }]);

    return UserGuide;
}(_react2.default.Component);

UserGuide.propTypes = {
    title: _propTypes2.default.string,
    className: _propTypes2.default.string,
    content: _propTypes2.default.string,
    contentTitle: _propTypes2.default.string
};
UserGuide.defaultProps = {
    className: 'icon-book right',
    content: ''
};
exports.default = UserGuide;
;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/Calendar.js":
/*!********************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/Calendar.js ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _parseInput = __webpack_require__(/*! ./utils/parseInput.js */ "./www/js/source/reactpages/components/date-range/utils/parseInput.js");

var _parseInput2 = _interopRequireDefault(_parseInput);

var _DayCell = __webpack_require__(/*! ./DayCell.js */ "./www/js/source/reactpages/components/date-range/DayCell.js");

var _DayCell2 = _interopRequireDefault(_DayCell);

var _styles = __webpack_require__(/*! ./styles.js */ "./www/js/source/reactpages/components/date-range/styles.js");

var _styles2 = _interopRequireDefault(_styles);

var _constants = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/components/date-range/constants.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var START_OF_MONTH = 0;
var END_OF_MONTH = 11;
var START_DAY_OF_MONTH = 1;
var DEFAULT_YEAR_RANGE = 5;

function checkRange(dayMoment, range) {
  return dayMoment.isBetween(range['startDate'], range['endDate']) || dayMoment.isBetween(range['endDate'], range['startDate']);
}

function checkStartEdge(dayMoment, range) {
  var startDate = range.startDate;


  return dayMoment.startOf('day').isSame(startDate.startOf('day'));
}

function checkEndEdge(dayMoment, range) {
  var endDate = range.endDate;


  return dayMoment.endOf('day').isSame(endDate.endOf('day'));
}

function isOusideMinMax(dayMoment, minDate, maxDate, format) {
  return minDate && dayMoment.isBefore((0, _parseInput2.default)(minDate, format, 'startOf')) || maxDate && dayMoment.isAfter((0, _parseInput2.default)(maxDate, format, 'endOf'));
}

var Calendar = function (_Component) {
  _inherits(Calendar, _Component);

  function Calendar(props, context) {
    _classCallCheck(this, Calendar);

    var _this = _possibleConstructorReturn(this, (Calendar.__proto__ || Object.getPrototypeOf(Calendar)).call(this, props, context));

    var format = props.format,
        range = props.range,
        theme = props.theme,
        offset = props.offset,
        firstDayOfWeek = props.firstDayOfWeek,
        shownDate = props.shownDate;


    var date = (0, _parseInput2.default)(props.date, format, 'startOf');
    var showDate = (shownDate || range && range['endDate'] || date).clone();
    var state = {
      date: date,
      shownDate: showDate,
      firstDayOfWeek: firstDayOfWeek || _moment2.default.localeData().firstDayOfWeek()
    };

    _this.state = state;
    _this.styles = (0, _styles2.default)(theme);
    return _this;
  }

  _createClass(Calendar, [{
    key: 'componentDidMount',
    value: function componentDidMount() {
      var onInit = this.props.onInit;

      onInit && onInit(this.state.date);
    }
  }, {
    key: 'getShownDate',
    value: function getShownDate() {
      var link = this.props.link;


      var shownDate = link || this.state.shownDate;

      return shownDate;
    }
  }, {
    key: 'handleSelect',
    value: function handleSelect(newDate) {
      var _props = this.props,
          link = _props.link,
          onChange = _props.onChange;


      onChange && onChange(newDate);

      if (!link) {
        this.setState({ date: newDate });
      }
    }
  }, {
    key: 'nextButtonClickHandler',
    value: function nextButtonClickHandler(event) {
      event.preventDefault();

      if (!this.disableNextButton) {
        this.changeMonth(this.state.shownDate.month() + 1, event);
      }
    }
  }, {
    key: 'prevButtonClickHandler',
    value: function prevButtonClickHandler(event) {
      event.preventDefault();
      if (!this.disablePrevButton) {
        this.changeMonth(this.state.shownDate.month() - 1, event);
      }
    }
  }, {
    key: 'changeMonth',
    value: function changeMonth(direction, event) {
      event.preventDefault();
      var selectedMonth = Number(direction);
      var year = this.state.shownDate.year();

      if (selectedMonth < START_OF_MONTH) {
        selectedMonth = END_OF_MONTH;
        year -= 1;
      }

      if (selectedMonth > END_OF_MONTH) {
        selectedMonth = START_OF_MONTH;
        year += 1;
      }

      var _props2 = this.props,
          link = _props2.link,
          linkCB = _props2.linkCB;


      var newDate = (0, _moment2.default)([year, selectedMonth, this.state.shownDate.date()]);
      if (!newDate.isValid()) {
        newDate = (0, _moment2.default)([year, selectedMonth, START_DAY_OF_MONTH]);
      }

      if (link && linkCB) {
        return linkCB(newDate);
      }

      this.setState({
        shownDate: newDate
      });
    }
  }, {
    key: 'changeYear',
    value: function changeYear(event) {
      event.preventDefault();

      var selectedYear = Number(event.target.value);
      var newDate = (0, _moment2.default)([selectedYear, this.state.shownDate.month(), this.state.shownDate.date()]);
      if (!newDate.isValid()) {
        newDate = (0, _moment2.default)([selectedYear, this.state.shownDate.month(), START_DAY_OF_MONTH]);
      }

      this.setState({
        shownDate: newDate
      });
    }
  }, {
    key: 'renderMonthList',
    value: function renderMonthList(classes) {
      var _this2 = this;

      var styles = this.styles;
      var _props3 = this.props,
          onlyClasses = _props3.onlyClasses,
          locale = _props3.locale;

      var monthLabels = locale.months;

      var monthOptions = [];
      for (var i = 0; i < 12; i++) {
        var isDisabled = this._isDisabledMonth(i);
        monthOptions.push(_react2.default.createElement(
          'option',
          { disabled: isDisabled, key: 'month' + i, value: i },
          monthLabels[i]
        ));
      }

      return _react2.default.createElement(
        'select',
        { style: onlyClasses ? undefined : styles["MonthAndYearSelect"], className: classes.monthAndYearSelect,
          onChange: function onChange(e) {
            return _this2.changeMonth(e.target.value, e);
          }, value: this.state.shownDate.month() },
        monthOptions
      );
    }
  }, {
    key: '_isDisabledMonth',
    value: function _isDisabledMonth(currentMonth) {
      var range = this.limitedDate;
      var currentYear = this.state.shownDate.year();

      return currentMonth < range.startDate.month() && currentYear <= range.startDate.year() || currentMonth > range.endDate.month() && currentYear >= range.endDate.year();
    }
  }, {
    key: 'renderYearList',
    value: function renderYearList(classes) {
      var styles = this.styles;
      var onlyClasses = this.props.onlyClasses;

      var range = this.limitedDate;

      var yearOptions = [];
      for (var i = range.startDate.year(); i <= range.endDate.year(); i++) {
        yearOptions.push(_react2.default.createElement(
          'option',
          { key: 'year' + i, value: i },
          i
        ));
      }

      return _react2.default.createElement(
        'select',
        { style: onlyClasses ? undefined : styles["MonthAndYearSelect"], className: classes.monthAndYearSelect,
          onChange: this.changeYear.bind(this), value: this.state.shownDate.year() },
        yearOptions
      );
    }
  }, {
    key: 'renderMonthAndYear',
    value: function renderMonthAndYear(classes) {
      var shownDate = this.getShownDate();
      var year = shownDate.year();
      var styles = this.styles;
      var _props4 = this.props,
          minDate = _props4.minDate,
          maxDate = _props4.maxDate,
          onlyClasses = _props4.onlyClasses,
          showMonthArrow = _props4.showMonthArrow;


      return _react2.default.createElement(
        'div',
        { style: onlyClasses ? undefined : styles['MonthAndYear'], className: classes.monthAndYearWrapper },
        showMonthArrow ? _react2.default.createElement(
          'button',
          {
            type: 'button', disabled: this.disablePrevButton,
            style: onlyClasses ? undefined : _extends({}, styles['MonthButton'], { float: 'left' }),
            className: classes.prevButton,
            onClick: this.prevButtonClickHandler.bind(this) },
          _react2.default.createElement('i', { style: onlyClasses ? undefined : _extends({}, styles['MonthArrow'], styles['MonthArrowPrev']) })
        ) : null,
        _react2.default.createElement(
          'span',
          null,
          _react2.default.createElement(
            'span',
            { className: classes.month },
            this.renderMonthList(classes)
          ),
          _react2.default.createElement(
            'span',
            { className: classes.monthAndYearDivider },
            '\xA0'
          ),
          _react2.default.createElement(
            'span',
            { className: classes.year },
            this.renderYearList(classes)
          )
        ),
        showMonthArrow ? _react2.default.createElement(
          'button',
          {
            type: 'button', disabled: this.disableNextButton,
            style: onlyClasses ? undefined : _extends({}, styles['MonthButton'], { float: 'right' }),
            className: classes.nextButton,
            onClick: this.nextButtonClickHandler.bind(this) },
          _react2.default.createElement('i', { style: onlyClasses ? undefined : _extends({}, styles['MonthArrow'], styles['MonthArrowNext']) })
        ) : null
      );
    }
  }, {
    key: 'renderWeekdays',
    value: function renderWeekdays(classes) {
      var dow = this.state.firstDayOfWeek;
      var weekdays = [];
      var styles = this.styles;
      var _props5 = this.props,
          onlyClasses = _props5.onlyClasses,
          locale = _props5.locale;

      var weekDays = locale.weekdays;

      var dayLength = 7 + dow;
      for (var i = dow; i < dayLength; i++) {
        var weekDayIndex = i % 7;
        var day = weekDays[weekDayIndex];
        weekdays.push(_react2.default.createElement(
          'span',
          { style: onlyClasses ? undefined : styles['Weekday'], className: classes.weekDay, key: i + day },
          day
        ));
      }

      return weekdays;
    }
  }, {
    key: 'renderDays',
    value: function renderDays(classes) {
      var _this3 = this;

      // TODO: Split this logic into smaller chunks
      var styles = this.styles;
      var _props6 = this.props,
          range = _props6.range,
          minDate = _props6.minDate,
          maxDate = _props6.maxDate,
          format = _props6.format,
          onlyClasses = _props6.onlyClasses,
          disableDaysBeforeToday = _props6.disableDaysBeforeToday,
          specialDays = _props6.specialDays;


      var shownDate = this.getShownDate();
      var _state = this.state,
          date = _state.date,
          firstDayOfWeek = _state.firstDayOfWeek;

      var dateUnix = date.unix();

      var monthNumber = shownDate.month();
      var dayCount = shownDate.daysInMonth();
      var startOfMonth = shownDate.clone().startOf('month').isoWeekday();

      var lastMonth = shownDate.clone().month(monthNumber - 1);
      var lastMonthNumber = lastMonth.month();
      var lastMonthDayCount = lastMonth.daysInMonth();

      var nextMonth = shownDate.clone().month(monthNumber + 1);
      var nextMonthNumber = nextMonth.month();

      var days = [];

      // Previous month's days
      var diff = Math.abs(firstDayOfWeek - (startOfMonth + 7)) % 7;
      for (var i = diff - 1; i >= 0; i--) {
        var dayMoment = lastMonth.clone().date(lastMonthDayCount - i);
        days.push({ dayMoment: dayMoment, isPassive: true });
      }

      // Current month's days
      for (var _i = 1; _i <= dayCount; _i++) {
        var _dayMoment = shownDate.clone().date(_i);
        // set days before today to isPassive
        var _today = (0, _moment2.default)();
        if (disableDaysBeforeToday && Number(_dayMoment.diff(_today, "days")) <= -1) {
          days.push({ dayMoment: _dayMoment, isPassive: true });
        } else {
          days.push({ dayMoment: _dayMoment });
        }
      }

      // Next month's days
      var remainingCells = 42 - days.length; // 42cells = 7days * 6rows
      for (var _i2 = 1; _i2 <= remainingCells; _i2++) {
        var _dayMoment2 = nextMonth.clone().date(_i2);
        days.push({ dayMoment: _dayMoment2, isPassive: true });
      }

      var today = (0, _moment2.default)().startOf('day');
      return days.map(function (data, index) {
        var dayMoment = data.dayMoment,
            isPassive = data.isPassive;

        var isSelected = !range && dayMoment.unix() === dateUnix;
        var isInRange = range && checkRange(dayMoment, range);
        var isStartEdge = range && checkStartEdge(dayMoment, range);
        var isEndEdge = range && checkEndEdge(dayMoment, range);
        var isEdge = isStartEdge || isEndEdge;
        var isToday = today.isSame(dayMoment);
        var isSunday = dayMoment.day() === 0;
        var isSpecialDay = specialDays && specialDays.some(function (specialDay) {
          return dayMoment.endOf('day').isSame(specialDay.date.endOf('day'));
        });
        var isOutsideMinMax = isOusideMinMax(dayMoment, minDate, maxDate, format);

        return _react2.default.createElement(_DayCell2.default, _extends({
          onSelect: _this3.handleSelect.bind(_this3)
        }, data, {
          theme: styles,
          isStartEdge: isStartEdge,
          isEndEdge: isEndEdge,
          isSelected: isSelected || isEdge,
          isInRange: isInRange,
          isSunday: isSunday,
          isSpecialDay: isSpecialDay,
          isToday: isToday,
          key: index,
          isPassive: isPassive || isOutsideMinMax,
          onlyClasses: onlyClasses,
          classNames: classes
        }));
      });
    }
  }, {
    key: 'render',
    value: function render() {
      var styles = this.styles;
      var _props7 = this.props,
          onlyClasses = _props7.onlyClasses,
          classNames = _props7.classNames;


      var classes = _extends({}, _styles.defaultClasses, classNames);

      return _react2.default.createElement(
        'div',
        { style: onlyClasses ? undefined : _extends({}, styles['Calendar'], this.props.style), className: classes.calendar },
        _react2.default.createElement(
          'div',
          { className: classes.monthAndYear },
          this.renderMonthAndYear(classes)
        ),
        _react2.default.createElement(
          'div',
          { className: classes.weekDays },
          this.renderWeekdays(classes)
        ),
        _react2.default.createElement(
          'div',
          { className: classes.days },
          this.renderDays(classes)
        )
      );
    }
  }, {
    key: 'limitedDate',
    get: function get() {
      var _props8 = this.props,
          minDate = _props8.minDate,
          maxDate = _props8.maxDate;

      var m = (0, _moment2.default)();
      var startDate = void 0,
          endDate = void 0;

      if (minDate) {
        startDate = _moment2.default.isMoment(minDate) ? minDate : (0, _moment2.default)(minDate, _constants.MomentFormat.default);
      } else {
        startDate = (0, _moment2.default)([m.year() - DEFAULT_YEAR_RANGE, START_OF_MONTH]);
      }

      if (maxDate) {
        endDate = _moment2.default.isMoment(maxDate) ? maxDate : (0, _moment2.default)(maxDate, _constants.MomentFormat.default);
      } else {
        endDate = (0, _moment2.default)([m.year(), END_OF_MONTH]).endOf("month");
      }

      return { startDate: startDate, endDate: endDate };
    }
  }, {
    key: 'disablePrevButton',
    get: function get() {
      var range = this.limitedDate;
      var startDate = range.startDate;
      var currentDate = this.state.shownDate;
      var prevMonth = this.state.shownDate.month() - 1;

      if (prevMonth < startDate.month() && currentDate.year() <= startDate.year()) {
        return true;
      }

      return false;
    }
  }, {
    key: 'disableNextButton',
    get: function get() {
      var range = this.limitedDate;
      var endDate = range.endDate;
      var currentDate = this.state.shownDate;
      var nextMonth = this.state.shownDate.month() + 1;

      if (nextMonth > endDate.month() && currentDate.year() >= endDate.year()) {
        return true;
      }

      return false;
    }
  }]);

  return Calendar;
}(_react.Component);

Calendar.defaultProps = {
  format: _constants.MomentFormat.default,
  theme: {},
  showMonthArrow: true,
  disableDaysBeforeToday: false,
  onlyClasses: false,
  classNames: {},
  specialDays: []
};

Calendar.propTypes = {
  showMonthArrow: _propTypes2.default.bool,
  disableDaysBeforeToday: _propTypes2.default.bool,
  lang: _propTypes2.default.string,
  sets: _propTypes2.default.string,
  range: _propTypes2.default.shape({
    startDate: _propTypes2.default.object,
    endDate: _propTypes2.default.object
  }),
  minDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
  maxDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
  date: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.string, _propTypes2.default.func]),
  format: _propTypes2.default.string.isRequired,
  firstDayOfWeek: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]),
  onChange: _propTypes2.default.func,
  onInit: _propTypes2.default.func,
  link: _propTypes2.default.oneOfType([_propTypes2.default.shape({
    startDate: _propTypes2.default.object,
    endDate: _propTypes2.default.object
  }), _propTypes2.default.bool]),
  linkCB: _propTypes2.default.func,
  theme: _propTypes2.default.object,
  onlyClasses: _propTypes2.default.bool,
  specialDays: _propTypes2.default.array,
  classNames: _propTypes2.default.object,
  locale: _propTypes2.default.object
};

exports.default = Calendar;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/DatePicker.jsx":
/*!***********************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/DatePicker.jsx ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _parseInput = __webpack_require__(/*! ./utils/parseInput.js */ "./www/js/source/reactpages/components/date-range/utils/parseInput.js");

var _parseInput2 = _interopRequireDefault(_parseInput);

var _defaultOptions = __webpack_require__(/*! ./defaultOptions.js */ "./www/js/source/reactpages/components/date-range/defaultOptions.js");

var _defaultOptions2 = _interopRequireDefault(_defaultOptions);

var _constants = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/components/date-range/constants.js");

var _styles = __webpack_require__(/*! ./styles.js */ "./www/js/source/reactpages/components/date-range/styles.js");

var _classnames = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");

var _classnames2 = _interopRequireDefault(_classnames);

var _Overlay = __webpack_require__(/*! ../Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");

var _Overlay2 = _interopRequireDefault(_Overlay);

var _Calendar = __webpack_require__(/*! ./Calendar.js */ "./www/js/source/reactpages/components/date-range/Calendar.js");

var _Calendar2 = _interopRequireDefault(_Calendar);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DatePicker = function (_Component) {
    _inherits(DatePicker, _Component);

    function DatePicker(props) {
        _classCallCheck(this, DatePicker);

        var _this = _possibleConstructorReturn(this, (DatePicker.__proto__ || Object.getPrototypeOf(DatePicker)).call(this, props));

        _this.show = function () {
            _this.setState({ show: true });
        };

        var locale = props.locale,
            date = props.date,
            format = props.format;


        _this.locale = _extends({}, _defaultOptions2.default.locale, locale);

        var selectedDate = (0, _parseInput2.default)(date, format, 'startOf');
        _this.state = {
            selectedDate: selectedDate,
            show: false
        };
        return _this;
    }

    _createClass(DatePicker, [{
        key: 'handleSelect',
        value: function handleSelect(date) {
            var onChange = this.props.onChange;


            onChange && onChange(date);

            this.setState({ selectedDate: date, show: false });
        }
    }, {
        key: 'render',
        value: function render() {
            var _props = this.props,
                format = _props.format,
                firstDayOfWeek = _props.firstDayOfWeek,
                minDate = _props.minDate,
                maxDate = _props.maxDate,
                classNames = _props.classNames;

            var classes = _extends({}, _styles.defaultClasses, classNames);
            var show = this.state.show;


            return _react2.default.createElement(
                'div',
                { className: 'daterange-container' },
                _react2.default.createElement(
                    'div',
                    { className: 'daterange-label', onClick: this.show },
                    _react2.default.createElement('input', { readOnly: true, className: 'input-text datepicker', type: 'text', autoComplete: 'off', value: this.dateText }),
                    _react2.default.createElement('i', { className: 'icon icon-calendar' })
                ),
                _react2.default.createElement(
                    _Overlay2.default,
                    { isOpening: show },
                    _react2.default.createElement(
                        'div',
                        { className: classes.dateRange },
                        _react2.default.createElement(_Calendar2.default, {
                            shownDate: this.state.selectedDate,
                            format: format,
                            firstDayOfWeek: firstDayOfWeek,
                            minDate: minDate,
                            maxDate: maxDate,
                            classNames: classNames,
                            onChange: this.handleSelect.bind(this),
                            locale: this.locale })
                    )
                )
            );
        }
    }, {
        key: 'dateText',
        get: function get() {
            var selectedDate = this.state.selectedDate;

            if (selectedDate) {
                return '' + selectedDate.format(this.props.format);
            }

            return "";
        }
    }]);

    return DatePicker;
}(_react.Component);

DatePicker.propTypes = {
    format: _propTypes2.default.string,
    firstDayOfWeek: _propTypes2.default.number,
    minDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
    maxDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
    classNames: _propTypes2.default.object,
    onChange: _propTypes2.default.func,
    onInit: _propTypes2.default.func,
    locale: _propTypes2.default.object,
    date: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string])
};
DatePicker.defaultProps = {
    format: _constants.MomentFormat.default
};
exports.default = DatePicker;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/DateRange.js":
/*!*********************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/DateRange.js ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _parseInput = __webpack_require__(/*! ./utils/parseInput.js */ "./www/js/source/reactpages/components/date-range/utils/parseInput.js");

var _parseInput2 = _interopRequireDefault(_parseInput);

var _Calendar = __webpack_require__(/*! ./Calendar.js */ "./www/js/source/reactpages/components/date-range/Calendar.js");

var _Calendar2 = _interopRequireDefault(_Calendar);

var _PredefinedRanges = __webpack_require__(/*! ./PredefinedRanges.js */ "./www/js/source/reactpages/components/date-range/PredefinedRanges.js");

var _PredefinedRanges2 = _interopRequireDefault(_PredefinedRanges);

var _styles = __webpack_require__(/*! ./styles.js */ "./www/js/source/reactpages/components/date-range/styles.js");

var _styles2 = _interopRequireDefault(_styles);

var _classnames = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");

var _classnames2 = _interopRequireDefault(_classnames);

var _constants = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/components/date-range/constants.js");

var _defaultOptions = __webpack_require__(/*! ./defaultOptions.js */ "./www/js/source/reactpages/components/date-range/defaultOptions.js");

var _defaultOptions2 = _interopRequireDefault(_defaultOptions);

var _Overlay = __webpack_require__(/*! ../Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");

var _Overlay2 = _interopRequireDefault(_Overlay);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DateRange = function (_Component) {
  _inherits(DateRange, _Component);

  function DateRange(props, context) {
    _classCallCheck(this, DateRange);

    var _this = _possibleConstructorReturn(this, (DateRange.__proto__ || Object.getPrototypeOf(DateRange)).call(this, props, context));

    _this.show = function () {
      _this.calendarSelectionStep = 0;
      _this.setState({ show: true });
    };

    _this.handleApply = function () {
      var _this$props = _this.props,
          onClickApplyButton = _this$props.onClickApplyButton,
          onChange = _this$props.onChange;

      var range = _this.state.range;
      _this.originalRange = Object.assign({}, range);
      onClickApplyButton && onClickApplyButton(range, _this.isValid);
      onChange && onChange(range, _this.isValid);
      _this.close();
    };

    _this.handleCancel = function () {
      _this.setRange(_this.originalRange, false);
    };

    var format = props.format,
        linkedCalendars = props.linkedCalendars,
        theme = props.theme;


    var startDate = (0, _parseInput2.default)(props.startDate, format, 'startOf');
    var endDate = (0, _parseInput2.default)(props.endDate, format, 'endOf');
    _this.styles = (0, _styles2.default)(theme);
    _this.calendarSelectionStep = 0;
    _this.locale = _extends({}, _defaultOptions2.default.locale, props.locale);
    _this.ranges = _extends({}, _defaultOptions2.default.ranges(props.today, props.minDate, props.maxDate, props.limitedMonthRange), props.ranges);

    _this.isValid = true;
    _this.originalRange = { startDate: startDate, endDate: endDate };

    _this.state = {
      range: { startDate: startDate, endDate: endDate },
      link: linkedCalendars && endDate,
      show: false
    };
    return _this;
  }

  _createClass(DateRange, [{
    key: 'close',
    value: function close() {
      this.setState({ show: false });
    }
  }, {
    key: 'componentDidMount',
    value: function componentDidMount() {
      var onInit = this.props.onInit;

      onInit && onInit(this.state.range);
    }
  }, {
    key: 'orderRange',
    value: function orderRange(range) {
      var startDate = range.startDate,
          endDate = range.endDate;

      var swap = startDate.isAfter(endDate);

      if (!swap) return range;

      return {
        startDate: endDate,
        endDate: startDate
      };
    }
  }, {
    key: 'setRange',
    value: function setRange(range, show) {
      range = this.orderRange(range);
      this.isValid = this.isValidMonthRange(range);

      if (this.isValid) {
        var newState = show === undefined ? { range: range } : { range: range, show: show };
        this.setState(newState);
      } else {
        var onChange = this.props.onChange;

        onChange && onChange(range, false);
      }
    }
  }, {
    key: 'handlePredefinedRangesSelect',
    value: function handlePredefinedRangesSelect(range) {
      this.setRange(range, false);

      this.originalRange = Object.assign({}, range);
      var onChange = this.props.onChange;

      onChange && onChange(range, true);
    }
  }, {
    key: 'handleCalendarSelect',
    value: function handleCalendarSelect(date) {
      var _state$range = this.state.range,
          startDate = _state$range.startDate,
          endDate = _state$range.endDate;


      var range = {
        startDate: startDate,
        endDate: endDate
      };

      switch (this.calendarSelectionStep) {
        case 0:
          range['startDate'] = date;
          range['endDate'] = date;
          this.calendarSelectionStep = 1;
          break;

        case 1:
          range['endDate'] = date;
          this.calendarSelectionStep = 0;
          break;
      }

      this.setRange(range);
    }
  }, {
    key: 'isValidMonthRange',
    value: function isValidMonthRange(date) {
      var limitedMonthRange = this.props.limitedMonthRange;


      if (limitedMonthRange) {
        var startDate = date.startDate;
        var endDate = date.endDate;
        var diffMonth = 12 * (endDate.year() - startDate.year()) + endDate.month() - startDate.month() + 1;

        return diffMonth <= limitedMonthRange;
      }

      return true;
    }
  }, {
    key: 'handleLinkChange',
    value: function handleLinkChange(newDate) {
      this.setState({
        link: newDate
      });
    }
  }, {
    key: 'render',
    value: function render() {
      var _this2 = this;

      var _props = this.props,
          showRange = _props.showRange,
          format = _props.format,
          linkedCalendars = _props.linkedCalendars,
          calendars = _props.calendars,
          firstDayOfWeek = _props.firstDayOfWeek,
          minDate = _props.minDate,
          maxDate = _props.maxDate,
          classNames = _props.classNames,
          onlyClasses = _props.onlyClasses,
          specialDays = _props.specialDays,
          lang = _props.lang,
          disableDaysBeforeToday = _props.disableDaysBeforeToday,
          showMonthArrow = _props.showMonthArrow;
      var _state = this.state,
          range = _state.range,
          link = _state.link;
      var styles = this.styles;


      var classes = _extends({}, _styles.defaultClasses, classNames);
      var calendarsCount = Number(calendars) - 1;

      var buttonHandlers = {
        apply: this.handleApply,
        cancel: this.handleCancel
      };

      return _react2.default.createElement(
        'div',
        { className: 'daterange-container' },
        _react2.default.createElement(
          'div',
          { className: 'daterange-label', onClick: this.show },
          _react2.default.createElement('input', { readOnly: true, name: 'DateRange', id: 'DateRange', className: 'input-text datepicker', type: 'text', autoComplete: 'off', value: this.dateRangeText }),
          _react2.default.createElement('i', { className: 'icon icon-calendar' })
        ),
        _react2.default.createElement(
          _Overlay2.default,
          { isOpening: this.state.show, onClose: this.handleCancel },
          _react2.default.createElement(
            'div',
            { className: classes.dateRange,
              ref: function ref(element) {
                return _this2.dateRangeContainer = element;
              } },
            showRange && _react2.default.createElement(_PredefinedRanges2.default, {
              format: format,
              ranges: this.ranges,
              range: range,
              theme: styles,
              onSelect: function onSelect(range) {
                return _this2.handlePredefinedRangesSelect(range);
              },
              onlyClasses: onlyClasses,
              classNames: classes,
              locale: this.locale,
              buttonHandlers: buttonHandlers }),
            function () {
              var _calendars = [];
              for (var i = calendarsCount; i >= 0; i--) {
                var shownDate = i == calendarsCount ? range.startDate : range.endDate;
                _calendars.push(_react2.default.createElement(_Calendar2.default, {
                  showMonthArrow: showMonthArrow,
                  shownDate: shownDate,
                  disableDaysBeforeToday: disableDaysBeforeToday,
                  lang: lang,
                  key: i,
                  link: linkedCalendars && link,
                  linkCB: _this2.handleLinkChange.bind(_this2),
                  range: range,
                  format: format,
                  firstDayOfWeek: firstDayOfWeek,
                  theme: styles,
                  minDate: minDate,
                  maxDate: maxDate,
                  onlyClasses: onlyClasses,
                  specialDays: specialDays,
                  classNames: classes,
                  onChange: _this2.handleCalendarSelect.bind(_this2),
                  locale: _this2.locale }));
              }
              return _calendars;
            }()
          )
        )
      );
    }
  }, {
    key: 'dateRangeText',
    get: function get() {
      var range = this.state.range;
      if (range.startDate && range.endDate) {
        var format = this.props.format;

        return range.startDate.format(format) + ' - ' + range.endDate.format(format);
      }

      return "";
    }
  }]);

  return DateRange;
}(_react.Component);

DateRange.defaultProps = {
  linkedCalendars: false,
  theme: {},
  format: _constants.MomentFormat.default,
  calendars: 2,
  onlyClasses: true,
  classNames: {},
  specialDays: [],
  twoStepChange: false,
  showRange: true
};

DateRange.propTypes = {
  format: _propTypes2.default.string,
  firstDayOfWeek: _propTypes2.default.number,
  calendars: _propTypes2.default.oneOfType([_propTypes2.default.string, _propTypes2.default.number]),
  startDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
  endDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
  minDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
  maxDate: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string]),
  dateLimit: _propTypes2.default.func,
  ranges: _propTypes2.default.object,
  linkedCalendars: _propTypes2.default.bool,
  twoStepChange: _propTypes2.default.bool,
  theme: _propTypes2.default.object,
  onInit: _propTypes2.default.func,
  onChange: _propTypes2.default.func,
  onlyClasses: _propTypes2.default.bool,
  specialDays: _propTypes2.default.array,
  classNames: _propTypes2.default.object,
  onClickApplyButton: _propTypes2.default.func,
  showRange: _propTypes2.default.bool,
  limitedMonthRange: _propTypes2.default.number,
  today: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string])
};

exports.default = DateRange;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/DayCell.js":
/*!*******************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/DayCell.js ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _classnames2 = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");

var _classnames3 = _interopRequireDefault(_classnames2);

var _styles = __webpack_require__(/*! ./styles.js */ "./www/js/source/reactpages/components/date-range/styles.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DayCell = function (_Component) {
  _inherits(DayCell, _Component);

  function DayCell(props, context) {
    _classCallCheck(this, DayCell);

    var _this = _possibleConstructorReturn(this, (DayCell.__proto__ || Object.getPrototypeOf(DayCell)).call(this, props, context));

    _this.state = {
      hover: false,
      active: false
    };

    _this.styles = _this.props.theme;
    return _this;
  }

  _createClass(DayCell, [{
    key: 'handleMouseEvent',
    value: function handleMouseEvent(event) {
      event.preventDefault();

      if (this.props.isPassive) return null;

      var newState = {};

      switch (event.type) {
        case 'mouseenter':
          newState['hover'] = true;
          break;

        case 'mouseup':
        case 'mouseleave':
          newState['hover'] = false;
          newState['active'] = false;
          break;

        case 'mousedown':
          newState['active'] = true;
          break;
      }

      this.setState(newState);
    }
  }, {
    key: 'handleSelect',
    value: function handleSelect(event) {
      event.preventDefault();

      if (this.props.isPassive) return null;

      this.props.onSelect(this.props.dayMoment);
    }
  }, {
    key: 'getStateStyles',
    value: function getStateStyles() {
      var _state = this.state,
          hover = _state.hover,
          active = _state.active;
      var _props = this.props,
          isSelected = _props.isSelected,
          isInRange = _props.isInRange,
          isPassive = _props.isPassive,
          isStartEdge = _props.isStartEdge,
          isEndEdge = _props.isEndEdge,
          dayMoment = _props.dayMoment,
          isToday = _props.isToday,
          isSunday = _props.isSunday,
          isSpecialDay = _props.isSpecialDay;
      var styles = this.styles;


      var hoverStyle = hover ? styles['DayHover'] : {};
      var activeStyle = active ? styles['DayActive'] : {};
      var passiveStyle = isPassive ? styles['DayPassive'] : {};
      var startEdgeStyle = isStartEdge ? styles['DayStartEdge'] : {};
      var endEdgeStyle = isEndEdge ? styles['DayEndEdge'] : {};
      var selectedStyle = isSelected ? styles['DaySelected'] : {};
      var inRangeStyle = isInRange ? styles['DayInRange'] : {};
      var todayStyle = isToday ? styles['DayToday'] : {};
      var sundayStyle = isSunday ? styles['DaySunday'] : {};
      var specialDayStyle = isSpecialDay ? styles['DaySpecialDay'] : {};

      return _extends({}, todayStyle, sundayStyle, specialDayStyle, inRangeStyle, hoverStyle, passiveStyle, activeStyle, selectedStyle, startEdgeStyle, endEdgeStyle);
    }
  }, {
    key: 'getClassNames',
    value: function getClassNames(classes) {
      var _classnames;

      var _props2 = this.props,
          isSelected = _props2.isSelected,
          isInRange = _props2.isInRange,
          isPassive = _props2.isPassive,
          isStartEdge = _props2.isStartEdge,
          isEndEdge = _props2.isEndEdge,
          isToday = _props2.isToday,
          isSunday = _props2.isSunday,
          isSpecialDay = _props2.isSpecialDay;


      return (0, _classnames3.default)((_classnames = {}, _defineProperty(_classnames, classes.day, true), _defineProperty(_classnames, classes.dayActive, isSelected), _defineProperty(_classnames, classes.dayPassive, isPassive), _defineProperty(_classnames, classes.dayInRange, isInRange), _defineProperty(_classnames, classes.dayStartEdge, isStartEdge), _defineProperty(_classnames, classes.dayEndEdge, isEndEdge), _defineProperty(_classnames, classes.dayToday, isToday), _defineProperty(_classnames, classes.daySunday, isSunday), _defineProperty(_classnames, classes.daySpecialDay, isSpecialDay), _classnames));
    }
  }, {
    key: 'render',
    value: function render() {
      var _props3 = this.props,
          dayMoment = _props3.dayMoment,
          onlyClasses = _props3.onlyClasses,
          classNames = _props3.classNames;
      var styles = this.styles;

      var stateStyle = this.getStateStyles();
      var classes = this.getClassNames(classNames);
      var dayWrapperStyles = {
        width: styles['Day'].width,
        height: styles['Day'].height,
        display: styles['Day'].display
      };

      return _react2.default.createElement(
        'span',
        {
          style: onlyClasses ? undefined : dayWrapperStyles,
          onClick: this.handleSelect.bind(this) },
        _react2.default.createElement(
          'span',
          {
            onMouseEnter: this.handleMouseEvent.bind(this),
            onMouseLeave: this.handleMouseEvent.bind(this),
            onMouseDown: this.handleMouseEvent.bind(this),
            onMouseUp: this.handleMouseEvent.bind(this),
            className: classes,
            style: onlyClasses ? undefined : _extends({}, styles['Day'], stateStyle) },
          dayMoment.date()
        )
      );
    }
  }]);

  return DayCell;
}(_react.Component);

DayCell.defaultProps = {
  theme: { 'Day': {} },
  onlyClasses: false
};

DayCell.propTypes = {
  dayMoment: _propTypes2.default.object.isRequired,
  onSelect: _propTypes2.default.func,
  isSelected: _propTypes2.default.bool,
  isInRange: _propTypes2.default.bool,
  isPassive: _propTypes2.default.bool,
  theme: _propTypes2.default.shape({
    Day: _propTypes2.default.object.isRequired
  }).isRequired,
  onlyClasses: _propTypes2.default.bool,
  isSpecialDay: _propTypes2.default.bool,
  classNames: _propTypes2.default.object
};

exports.default = DayCell;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/PredefinedRanges.js":
/*!****************************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/PredefinedRanges.js ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _classnames2 = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");

var _classnames3 = _interopRequireDefault(_classnames2);

var _parseInput = __webpack_require__(/*! ./utils/parseInput.js */ "./www/js/source/reactpages/components/date-range/utils/parseInput.js");

var _parseInput2 = _interopRequireDefault(_parseInput);

var _styles = __webpack_require__(/*! ./styles.js */ "./www/js/source/reactpages/components/date-range/styles.js");

var _defaultOptions = __webpack_require__(/*! ./defaultOptions.js */ "./www/js/source/reactpages/components/date-range/defaultOptions.js");

var _defaultOptions2 = _interopRequireDefault(_defaultOptions);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var PredefinedRanges = function (_Component) {
  _inherits(PredefinedRanges, _Component);

  function PredefinedRanges(props, context) {
    _classCallCheck(this, PredefinedRanges);

    var _this = _possibleConstructorReturn(this, (PredefinedRanges.__proto__ || Object.getPrototypeOf(PredefinedRanges)).call(this, props, context));

    _this.styles = _this.props.theme;
    _this.selectedItem = _this._getSelectedRange();
    return _this;
  }

  _createClass(PredefinedRanges, [{
    key: 'componentDidUpdate',
    value: function componentDidUpdate(prevProps, prevState) {
      if (this.props.range !== prevProps.range) {
        this.selectedItem = this._getSelectedRange();
      }
    }
  }, {
    key: '_getSelectedRange',
    value: function _getSelectedRange() {
      var _props = this.props,
          ranges = _props.ranges,
          range = _props.range;


      return Object.keys(ranges).find(function (name) {
        var currentRange = ranges[name];

        return range.startDate.isSame(currentRange.startDate, "day") && range.endDate.isSame(currentRange.endDate, "day");
      });
    }
  }, {
    key: 'handleSelect',
    value: function handleSelect(name, event) {
      event.preventDefault();

      this.selectedItem = name;

      var range = this.props.ranges[name];

      this.props.onSelect({
        startDate: (0, _parseInput2.default)(range['startDate'], null, 'startOf'),
        endDate: (0, _parseInput2.default)(range['endDate'], null, 'endOf')
      });
    }
  }, {
    key: 'clickButton',
    value: function clickButton(event, action) {
      event.preventDefault();
      this.props.buttonHandlers[action]();
    }
  }, {
    key: 'renderRangeList',
    value: function renderRangeList(classes) {
      var _this2 = this;

      var _props2 = this.props,
          ranges = _props2.ranges,
          range = _props2.range,
          onlyClasses = _props2.onlyClasses;
      var styles = this.styles;

      var gotActiveRange = false;

      return Object.keys(ranges).map(function (name) {
        var _classnames;

        var active = _this2.selectedItem === name;
        gotActiveRange = !gotActiveRange && active;

        var style = _extends({}, styles['PredefinedRangesItem'], gotActiveRange ? styles['PredefinedRangesItemActive'] : {});

        var predefinedRangeClass = (0, _classnames3.default)((_classnames = {}, _defineProperty(_classnames, classes.predefinedRangesItem, true), _defineProperty(_classnames, classes.predefinedRangesItemActive, gotActiveRange), _classnames));

        return _react2.default.createElement(
          'a',
          {
            href: '#',
            key: 'range-' + name,
            className: predefinedRangeClass,
            style: onlyClasses ? undefined : style,
            onClick: function onClick(e) {
              return _this2.handleSelect(name, e);
            }
          },
          name
        );
      });
    }
  }, {
    key: 'renderRangeButtons',
    value: function renderRangeButtons(classes) {
      var _this3 = this;

      var _props3 = this.props,
          style = _props3.style,
          onlyClasses = _props3.onlyClasses,
          locale = _props3.locale;

      var locales = _extends({}, _defaultOptions2.default.locale, locale);
      var styles = this.styles;

      var action = {
        apply: "apply",
        cancel: "cancel"
      };

      return _react2.default.createElement(
        'div',
        { className: _styles.defaultClasses.rangeButtonGroup },
        _react2.default.createElement(
          'button',
          { className: classes.applyButton,
            style: onlyClasses ? undefined : _extends({}, styles['RangeApplyButton'], style),
            type: 'button',
            onClick: function onClick(e) {
              return _this3.clickButton(e, action.apply);
            } },
          locales.applyLabel
        ),
        _react2.default.createElement(
          'button',
          { className: classes.cancelButton,
            style: onlyClasses ? undefined : _extends({}, styles['RangeCancelButton'], style),
            onClick: function onClick(e) {
              return _this3.clickButton(e, action.cancel);
            },
            type: 'button' },
          locales.cancelLabel
        )
      );
    }
  }, {
    key: 'render',
    value: function render() {
      var _props4 = this.props,
          style = _props4.style,
          onlyClasses = _props4.onlyClasses,
          classNames = _props4.classNames;
      var styles = this.styles;


      var classes = _extends({}, _styles.defaultClasses, classNames);

      return _react2.default.createElement(
        'div',
        {
          style: onlyClasses ? undefined : _extends({}, styles['PredefinedRanges'], style),
          className: classes.predefinedRanges
        },
        this.renderRangeList(classes),
        this.renderRangeButtons(classes)
      );
    }
  }]);

  return PredefinedRanges;
}(_react.Component);

PredefinedRanges.defaultProps = {
  onlyClasses: false,
  classNames: {}
};

PredefinedRanges.propTypes = {
  ranges: _propTypes2.default.object.isRequired,
  onlyClasses: _propTypes2.default.bool,
  classNames: _propTypes2.default.object
};

exports.default = PredefinedRanges;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/constants.js":
/*!*********************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/constants.js ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
var MomentFormat = exports.MomentFormat = {
    default: "MM/DD/YYYY",
    monthSelect: "MMM"
};

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/defaultOptions.js":
/*!**************************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/defaultOptions.js ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _langHelper = __webpack_require__(/*! ../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _locale = __webpack_require__(/*! ./utils/locale */ "./www/js/source/reactpages/components/date-range/utils/locale.js");

var _locale2 = _interopRequireDefault(_locale);

var _constants = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/components/date-range/constants.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var addRange = function addRange(rangeObj, name, start, end) {
    rangeObj[_langHelper2.default.getSingleResource(name)] = {
        startDate: start,
        endDate: end
    };
};

var addOneDay = function addOneDay(rangeObj, name, day, min, max) {
    if (day >= min && day <= max) {
        addRange(rangeObj, name, day, day);
    }
};

var addDistanceDays = function addDistanceDays(rangeObj, name, start, end, min, max) {
    if (!(max < start || end < min)) {
        addRange(rangeObj, name, start < min ? min : start, end > max ? max : end);
    }
};

exports.default = {
    ranges: function ranges(today, min, max, limitMonths) {
        today = !today ? (0, _moment2.default)() : !_moment2.default.isMoment(today) ? (0, _moment2.default)(today, _constants.MomentFormat.default) : today;
        max = !max ? today : !_moment2.default.isMoment(max) ? (0, _moment2.default)(max, _constants.MomentFormat.default) : max;
        min = !min ? today : !_moment2.default.isMoment(min) ? (0, _moment2.default)(min, _constants.MomentFormat.default) : min;

        var range = {};

        if (today < min) {
            return range;
        }

        addOneDay(range, "Today", today, min, max);

        var yesterday = today.clone().add(-1, 'days');
        addOneDay(range, "Yesterday", yesterday, min, max);

        var startCurrentWeek = today.clone().startOf('week');
        var endCurrentWeek = today;
        addDistanceDays(range, "Currentweek", startCurrentWeek, endCurrentWeek, min, max);

        var startLastWeek = today.clone().subtract(1, 'week').startOf('week');
        var endLastWeek = startLastWeek.clone().add(6, 'days');
        addDistanceDays(range, "Lastweek", startLastWeek, endLastWeek, min, max);

        var startCurrentmonth = today.clone().startOf('month');
        var endCurrentmonth = today;
        addDistanceDays(range, "Currentmonth", startCurrentmonth, endCurrentmonth, min, max);

        var startLastmonth = today.clone().subtract(1, 'month').startOf('month');
        var endLastmonth = startLastmonth.clone().endOf('month');
        addDistanceDays(range, "Lastmonth", startLastmonth, endLastmonth, min, max);

        if (limitMonths && limitMonths > 1) {
            var startSincelastmonth = today.clone().subtract(1, 'month').startOf('month');
            var endSincelastmonth = today;
            addDistanceDays(range, "Sincelastmonth", startSincelastmonth, endSincelastmonth, min, max);
        }

        return range;
    },

    locale: _extends({
        applyLabel: _langHelper2.default.getSingleResource("OK"),
        cancelLabel: _langHelper2.default.getSingleResource("Cancel")
    }, _locale2.default)
};

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/index.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/index.js ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.DatePicker = exports.Calendar = exports.DateRange = undefined;

var _DateRange = __webpack_require__(/*! ./DateRange.js */ "./www/js/source/reactpages/components/date-range/DateRange.js");

var _DateRange2 = _interopRequireDefault(_DateRange);

var _Calendar = __webpack_require__(/*! ./Calendar.js */ "./www/js/source/reactpages/components/date-range/Calendar.js");

var _Calendar2 = _interopRequireDefault(_Calendar);

var _DatePicker = __webpack_require__(/*! ./DatePicker */ "./www/js/source/reactpages/components/date-range/DatePicker.jsx");

var _DatePicker2 = _interopRequireDefault(_DatePicker);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.DateRange = _DateRange2.default;
exports.Calendar = _Calendar2.default;
exports.DatePicker = _DatePicker2.default;

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/styles.js":
/*!******************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/styles.js ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var defaultClasses = exports.defaultClasses = {
    show: 'rdr-Show',
    hide: 'rdr-Hide',
    calendar: 'rdr-Calendar',
    dateRange: 'rdr-DateRange',
    predefinedRanges: 'rdr-PredefinedRanges',
    predefinedRangesItem: 'rdr-PredefinedRangesItem',
    predefinedRangesItemActive: 'rdr-PredefinedRangesItemActive',
    monthAndYear: 'rdr-MonthAndYear',
    monthAndYearSelect: 'rdr-MonthAndYearSelect',
    weekDays: 'rdr-WeekDays',
    weekDay: 'rdr-WeekDay',
    days: 'rdr-Days',
    day: 'rdr-Day',
    dayActive: 'is-selected',
    dayPassive: 'is-passive',
    dayInRange: 'is-inRange',
    monthAndYearWrapper: 'rdr-MonthAndYear-innerWrapper',
    prevButton: 'rdr-MonthAndYear-button prev',
    nextButton: 'rdr-MonthAndYear-button next',
    month: 'rdr-MonthAndYear-month',
    monthAndYearDivider: 'rdr-MonthAndYear-divider',
    year: 'rdr-MonthAndYear-year',
    daySunday: 'rdr-Sunday',
    daySpecialDay: 'rdr-SpecialDay',
    rangeButtonGroup: 'range-button-group',
    applyButton: 'rdr-Button rdr-ApplyButton',
    cancelButton: 'rdr-Button rdr-CancelButton'
};

var defaultTheme = {
    DateRange: {
        display: 'none',
        boxSizing: 'border-box',
        background: '#ffffff',
        borderRadius: '2px',
        position: "absolute",
        width: 735,
        zIndex: 1,
        top: 60
    },
    Calendar: {
        width: 280,
        padding: 10,
        background: '#ffffff',
        borderRadius: '2px',
        display: 'inline-block',
        boxSizing: 'border-box',
        letterSpacing: 0,
        color: '#000000'
    },

    Day: {
        boxSizing: 'border-box',
        display: 'inline-block',
        letterSpacing: 'initial',
        textAlign: 'center',
        fontSize: 12,
        cursor: 'pointer',
        transition: 'transform .1s ease'
    },

    DayPassive: {
        opacity: 0.4,
        cursor: 'normal'
    },

    DayHover: {
        background: 'rgba(78, 184, 94, 0.4)'
    },

    DayToday: {},

    DaySunday: {},

    DaySpecialDay: {},

    DayActive: {
        background: '#95a5a6',
        color: '#ffffff',
        transform: 'scale(0.9)'
    },

    DaySelected: {
        background: '#4eb85e',
        color: '#ffffff'
    },

    DayStartEdge: {},

    DayEndEdge: {},

    DayInRange: {
        background: 'rgba(78, 184, 94, 0.2)',
        color: '#95a5a6'
    },

    Weekday: {
        boxSizing: 'border-box',
        display: 'inline-block',
        letterSpacing: 'initial',
        textAlign: 'center',
        fontSize: 12,
        fontWeight: '600',
        marginBottom: 1
    },

    MonthAndYear: {
        textAlign: 'center',
        boxSizing: 'border-box',
        fontSize: 12,
        padding: '5px 0',
        height: 40,
        lineHeight: '18px'
    },

    MonthButton: {
        display: 'block',
        boxSizing: 'border-box',
        height: 25,
        width: 25,
        padding: 0,
        margin: '0 10px',
        border: 'none',
        boxShadow: 'none',
        outline: 'none',
        background: '#fff',
        cursor: 'pointer'
    },

    MonthArrow: {
        display: 'block',
        width: 0,
        height: 0,
        padding: 0,
        margin: 0,
        border: '5px solid transparent',
        textAlign: 'center'
    },

    MonthArrowPrev: {
        borderRightWidth: '7px',
        borderRightColor: '#34495e',
        marginLeft: 3
    },

    MonthArrowNext: {
        borderLeftWidth: '7px',
        borderLeftColor: '#34495e',
        marginLeft: 10
    },

    MonthAndYearSelect: {
        height: 25,
        width: 65
    },

    PredefinedRanges: {
        width: 160,
        display: 'inline-block',
        verticalAlign: 'top',
        marginLeft: 10,
        marginTop: 10
    },

    PredefinedRangesItem: {
        display: 'block',
        fontSize: 12,
        color: '#2c3e50',
        padding: '10px 14px',
        borderRadius: '4px',
        textDecoration: 'none',
        marginBottom: 3
    },

    PredefinedRangesItemActive: {
        background: '#4c6375',
        color: '#fff'
    },

    RangeButton: {
        border: 'none',
        borderRadius: '2px',
        boxShadow: '0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 3px 1px -2px rgba(0, 0, 0, 0.2), 0 1px 5px 0 rgba(0, 0, 0, 0.12)',
        height: 25,
        lineHeight: '25px',
        textAlign: 'center',
        letterSpacing: '0.5px',
        display: 'inline-block',
        outline: 0,
        verticalAlign: 'middle',
        width: '48%',
        color: '#fff'
    },

    RangeApplyButton: {
        marginRight: 'calc(4%)',
        backgroundColor: '#f05b1f'
    },

    RangeCancelButton: {
        backgroundColor: '#757985'
    }
};

exports.default = function () {
    var customTheme = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

    var calendarWidth = defaultTheme.Calendar.width;
    var calendarPadding = defaultTheme.Calendar.padding;
    var cellMargin = defaultTheme.Day.margin || 0;

    if (customTheme.Calendar && customTheme.Calendar.hasOwnProperty('width')) {
        calendarWidth = customTheme.Calendar.width;
    }

    if (customTheme.Calendar && customTheme.Calendar.hasOwnProperty('padding')) {
        calendarPadding = customTheme.Calendar.padding;
    }

    if (customTheme.Day && customTheme.Day.hasOwnProperty('margin')) {
        cellMargin = customTheme.Day.margin;
    }

    var cellSize = (parseInt(calendarWidth) - parseInt(calendarPadding) * 2) / 7 - parseInt(cellMargin) * 2;

    return {
        DateRange: _extends({}, defaultTheme.DateRange, customTheme.DateRange),

        Calendar: _extends({}, defaultTheme.Calendar, customTheme.Calendar),

        Day: _extends({
            width: cellSize,
            height: cellSize,
            lineHeight: cellSize + 'px'
        }, defaultTheme.Day, customTheme.Day),

        DayPassive: _extends({}, defaultTheme.DayPassive, customTheme.DayPassive),

        DayHover: _extends({}, defaultTheme.DayHover, customTheme.DayHover),

        DayToday: _extends({}, defaultTheme.DayToday, customTheme.DayToday),
        DaySunday: _extends({}, defaultTheme.DaySunday, customTheme.DaySunday),
        DaySpecialDay: _extends({}, defaultTheme.DaySpecialDay, customTheme.DaySpecialDay),

        DayActive: _extends({}, defaultTheme.DayActive, customTheme.DayActive),

        DaySelected: _extends({}, defaultTheme.DaySelected, customTheme.DaySelected),

        DayStartEdge: _extends({}, defaultTheme.DayStartEdge, customTheme.DayStartEdge),

        DayEndEdge: _extends({}, defaultTheme.DayEndEdge, customTheme.DayEndEdge),

        DayInRange: _extends({}, defaultTheme.DayInRange, customTheme.DayInRange),

        Weekday: _extends({
            width: cellSize,
            height: cellSize / 2,
            lineHeight: cellSize / 2 + 'px'
        }, defaultTheme.Weekday, customTheme.Weekday),

        MonthAndYear: _extends({}, defaultTheme.MonthAndYear, customTheme.MonthAndYear),

        MonthAndYearSelect: _extends({}, defaultTheme.MonthAndYearSelect, customTheme.MonthAndYearSelect),

        MonthButton: _extends({}, defaultTheme.MonthButton, customTheme.MonthButton),

        MonthArrow: _extends({}, defaultTheme.MonthArrow, customTheme.MonthArrow),

        MonthArrowPrev: _extends({}, defaultTheme.MonthArrowPrev, customTheme.MonthArrowPrev),

        MonthArrowNext: _extends({}, defaultTheme.MonthArrowNext, customTheme.MonthArrowNext),

        PredefinedRanges: _extends({}, defaultTheme.PredefinedRanges, customTheme.PredefinedRanges),

        PredefinedRangesItem: _extends({}, defaultTheme.PredefinedRangesItem, customTheme.PredefinedRangesItem),

        PredefinedRangesItemActive: _extends({}, defaultTheme.PredefinedRangesItemActive, customTheme.PredefinedRangesItemActive),

        RangeApplyButton: _extends({}, defaultTheme.RangeButton, defaultTheme.RangeApplyButton, customTheme.RangeApplyButton),

        RangeCancelButton: _extends({}, defaultTheme.RangeButton, defaultTheme.RangeCancelButton, customTheme.RangeCancelButton)
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/utils/locale.js":
/*!************************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/utils/locale.js ***!
  \************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = {
    months: [_langHelper2.default.getSingleResource('Jan'), _langHelper2.default.getSingleResource('Feb'), _langHelper2.default.getSingleResource('Mar'), _langHelper2.default.getSingleResource('Apr'), _langHelper2.default.getSingleResource('May'), _langHelper2.default.getSingleResource('Jun'), _langHelper2.default.getSingleResource('Jul'), _langHelper2.default.getSingleResource('Aug'), _langHelper2.default.getSingleResource('Sep'), _langHelper2.default.getSingleResource('Oct'), _langHelper2.default.getSingleResource('Nov'), _langHelper2.default.getSingleResource('Dec')],

    weekdays: [_langHelper2.default.getSingleResource('Su'), _langHelper2.default.getSingleResource('Mo'), _langHelper2.default.getSingleResource('Tu'), _langHelper2.default.getSingleResource('We'), _langHelper2.default.getSingleResource('Th'), _langHelper2.default.getSingleResource('Fr'), _langHelper2.default.getSingleResource('Sa')]
};

/***/ }),

/***/ "./www/js/source/reactpages/components/date-range/utils/parseInput.js":
/*!****************************************************************************!*\
  !*** ./www/js/source/reactpages/components/date-range/utils/parseInput.js ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = parseInput;

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function parseInput(input, format, timeOfDay) {
  var output = null;

  if (typeof input === 'undefined' || typeof input === 'null' || !input || input === '') {
    output = (0, _moment2.default)()[timeOfDay]('day');
  } else if (typeof input === 'string') {
    output = (0, _moment2.default)(input, format)[timeOfDay]('day');
  } else if (typeof input === 'function') {
    output = parseInput(input((0, _moment2.default)()[timeOfDay]('day')), format, timeOfDay);
  } else if (input._isAMomentObject) {
    output = input[timeOfDay]('day').clone();
  }

  return output;
}

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/components/Cell.jsx":
/*!************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/components/Cell.jsx ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Cell = function (_Component) {
    _inherits(Cell, _Component);

    function Cell(props) {
        _classCallCheck(this, Cell);

        return _possibleConstructorReturn(this, (Cell.__proto__ || Object.getPrototypeOf(Cell)).call(this, props));
    }

    _createClass(Cell, [{
        key: 'render',
        value: function render() {
            var _props = this.props,
                header = _props.header,
                colWidth = _props.colWidth,
                sortable = _props.sortable,
                sorting = _props.sorting,
                asc = _props.asc,
                onClick = _props.onClick,
                rest = _objectWithoutProperties(_props, ['header', 'colWidth', 'sortable', 'sorting', 'asc', 'onClick']);

            return header ? sortable ? _react2.default.createElement(
                'th',
                _extends({}, rest, { className: 'sortable ' + (sorting ? 'sorting' : ''), onClick: onClick }),
                this.props.children,
                _react2.default.createElement('span', { className: '' + (sorting ? asc ? 'asc' : 'desc' : '') })
            ) : _react2.default.createElement(
                'th',
                rest,
                this.props.children
            ) : _react2.default.createElement(
                'td',
                rest,
                this.props.children
            );
        }
    }]);

    return Cell;
}(_react.Component);

Cell.propTypes = {
    header: _propTypes2.default.bool,
    sortable: _propTypes2.default.bool,
    sorting: _propTypes2.default.bool,
    asc: _propTypes2.default.bool,
    onClick: _propTypes2.default.func
};
Cell.defaultProps = {
    header: false,
    sortable: false,
    sorting: false,
    asc: true,
    onClick: function onClick() {}
};
exports.default = Cell;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/components/Pager.jsx":
/*!*************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/components/Pager.jsx ***!
  \*************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Pager = function (_Component) {
    _inherits(Pager, _Component);

    function Pager(props) {
        _classCallCheck(this, Pager);

        var _this = _possibleConstructorReturn(this, (Pager.__proto__ || Object.getPrototypeOf(Pager)).call(this, props));

        _this.onKeyUpHandle = function (e, totalPage) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code === 13) {
                var value = parseInt(_this.state.inputValue);
                if (value > 0 && value <= totalPage) {
                    _this.goToPage(value, _this.props.pageOption.PageSize);
                }
            }
        };

        _this.onInputChangeHandler = function (e) {
            var pattern = /^\d*$/;
            var value = e.target.value;

            var isValidNumber = pattern.test(value);
            if (isValidNumber) {
                _this._updateState(value);
            }
        };

        _this.state = {
            inputValue: _this.props.pageOption.PageIndex
        };
        return _this;
    }

    _createClass(Pager, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            var pageIndex = this.props.pageOption.PageIndex;
            if (pageIndex !== prevProps.pageOption.PageIndex) {
                this._updateState(pageIndex);
            }
        }
    }, {
        key: 'goToPage',
        value: function goToPage(index, pageSize) {
            this.props.onPaging && this.props.onPaging(index, pageSize);
        }
    }, {
        key: 'onClickHandle',
        value: function onClickHandle(isDisable, handleFunc) {
            if (!isDisable) {
                handleFunc();
            }
        }
    }, {
        key: '_updateState',
        value: function _updateState(inputValue) {
            this.setState({ inputValue: inputValue });
        }
    }, {
        key: '_renderPageSizeSelection',
        value: function _renderPageSizeSelection(isDisable, pageIndex, pageSize) {
            var _this2 = this;

            return _react2.default.createElement(
                'select',
                { className: 'pagesize-select', disabled: isDisable, value: pageSize, onChange: function onChange(e) {
                        return _this2.goToPage(1, Number(e.target.value));
                    } },
                this._renderPageSizeOption(this.props.pageOption.PageList)
            );
        }
    }, {
        key: '_renderPageSizeOption',
        value: function _renderPageSizeOption(pageList) {
            return pageList.map(function (item, index) {
                return _react2.default.createElement(
                    'option',
                    { key: 'pagesize' + index, value: item },
                    item
                );
            });
        }
    }, {
        key: '_renderPageIcon',
        value: function _renderPageIcon(iconClass, isDisable, onClickFunc) {
            var _this3 = this;

            return _react2.default.createElement('a', { href: 'javascript:;', className: 'icon ' + iconClass + ' ' + (isDisable ? "disable" : ""), onClick: function onClick() {
                    return _this3.onClickHandle(isDisable, onClickFunc);
                } });
        }
    }, {
        key: '_renderPageInput',
        value: function _renderPageInput(index, totalPage) {
            var _this4 = this;

            return _react2.default.createElement(
                'div',
                { className: 'page-input' },
                _react2.default.createElement(
                    'span',
                    null,
                    'Page '
                ),
                _react2.default.createElement('input', { type: 'text', className: 'page-form', value: index, maxLength: '4', size: '2',
                    onChange: this.onInputChangeHandler,
                    onKeyUp: function onKeyUp(e) {
                        return _this4.onKeyUpHandle(e, totalPage);
                    } }),
                _react2.default.createElement(
                    'span',
                    null,
                    ' of ' + totalPage,
                    ' '
                )
            );
        }
    }, {
        key: '_renderPageLabel',
        value: function _renderPageLabel(index, totalPage) {
            return _react2.default.createElement(
                'div',
                { className: 'page-input' },
                _react2.default.createElement(
                    'span',
                    null,
                    'Page ',
                    index + ' of ' + totalPage + ' '
                )
            );
        }
    }, {
        key: '_renderPageInfo',
        value: function _renderPageInfo(index, pageSize, totalItem) {
            var viewFrom = (index - 1) * pageSize + 1;
            var viewTo = index * pageSize;
            return _react2.default.createElement(
                'span',
                { className: 'page-info' },
                'View ' + viewFrom + '-' + (viewTo > totalItem ? totalItem : viewTo) + ' of ' + totalItem.toLocaleString()
            );
        }
    }, {
        key: 'render',
        value: function render() {
            var _this5 = this;

            var _props = this.props,
                isShowPagingInfo = _props.isShowPagingInfo,
                isAllowInputPageIndex = _props.isAllowInputPageIndex,
                pageOption = _props.pageOption,
                alwaysVisible = _props.alwaysVisible;
            var PageIndex = pageOption.PageIndex,
                PageSize = pageOption.PageSize,
                TotalItem = pageOption.TotalItem,
                PageList = pageOption.PageList;
            var inputValue = this.state.inputValue;


            var isDisable = isNaN(parseInt(inputValue));

            var isShow = alwaysVisible || TotalItem > PageList[0];
            var totalPage = Math.ceil(TotalItem / PageSize);

            return _react2.default.createElement(
                'div',
                { className: 'page-container center-align', style: { display: '' + (isShow ? 'block' : 'none') } },
                this._renderPageIcon("page-first", PageIndex === 1, function () {
                    return _this5.goToPage(1, PageSize);
                }),
                this._renderPageIcon("page-prev", isDisable || PageIndex <= 1 || PageIndex > totalPage, function () {
                    return _this5.goToPage(PageIndex - 1, PageSize);
                }),
                isAllowInputPageIndex ? this._renderPageInput(inputValue, totalPage) : this._renderPageLabel(PageIndex, totalPage),
                this._renderPageIcon("page-next", isDisable || PageIndex >= totalPage || PageIndex < 1, function () {
                    return _this5.goToPage(PageIndex + 1, PageSize);
                }),
                this._renderPageIcon("page-last", PageIndex === totalPage, function () {
                    return _this5.goToPage(totalPage, PageSize);
                }),
                this._renderPageSizeSelection(isDisable, PageIndex, PageSize),
                isShowPagingInfo && this._renderPageInfo(PageIndex, PageSize, TotalItem)
            );
        }
    }]);

    return Pager;
}(_react.Component);

Pager.propTypes = {
    pageOption: _propTypes2.default.shape({
        PageIndex: _propTypes2.default.number,
        PageSize: _propTypes2.default.number,
        PageList: _propTypes2.default.arrayOf(_propTypes2.default.number),
        TotalItem: _propTypes2.default.number
    }),
    onPaging: _propTypes2.default.func,
    isShowPagingInfo: _propTypes2.default.bool,
    isAllowInputPageIndex: _propTypes2.default.bool,
    alwaysVisible: _propTypes2.default.bool
};
Pager.defaultProps = {
    pageOption: {
        PageIndex: 1,
        PageSize: 50,
        PageList: [50, 100, 200],
        TotalItem: 0
    },
    isShowPagingInfo: true,
    isAllowInputPageIndex: true,
    alwaysVisible: false
};
exports.default = Pager;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/components/Row.jsx":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/components/Row.jsx ***!
  \***********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Row = function (_Component) {
    _inherits(Row, _Component);

    function Row(props) {
        _classCallCheck(this, Row);

        return _possibleConstructorReturn(this, (Row.__proto__ || Object.getPrototypeOf(Row)).call(this, props));
    }

    _createClass(Row, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'tr',
                this.props,
                this.props.children
            );
        }
    }]);

    return Row;
}(_react.Component);

exports.default = Row;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/components/RowLayout.jsx":
/*!*****************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/components/RowLayout.jsx ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var RowLayout = function (_Component) {
    _inherits(RowLayout, _Component);

    function RowLayout(props) {
        _classCallCheck(this, RowLayout);

        return _possibleConstructorReturn(this, (RowLayout.__proto__ || Object.getPrototypeOf(RowLayout)).call(this, props));
    }

    _createClass(RowLayout, [{
        key: '_getRowLayout',
        value: function _getRowLayout() {
            var columnLayout = this.props.columnLayout;


            return columnLayout.map(function (cellWidth, index) {
                var width = isNaN(cellWidth) ? cellWidth : cellWidth + 'px';

                return _react2.default.createElement('td', { key: 'cellLayout' + index, style: { width: width } });
            });
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'tr',
                { className: 'first-row' },
                this._getRowLayout()
            );
        }
    }]);

    return RowLayout;
}(_react.Component);

RowLayout.propTypes = {
    columnLayout: _propTypes2.default.arrayOf(_propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string])).isRequired
};
exports.default = RowLayout;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/components/Table.jsx":
/*!*************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/components/Table.jsx ***!
  \*************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _RowLayout = __webpack_require__(/*! ./RowLayout */ "./www/js/source/reactpages/components/fixed-table-header/components/RowLayout.jsx");

var _RowLayout2 = _interopRequireDefault(_RowLayout);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _tableSection = __webpack_require__(/*! ./tableSection */ "./www/js/source/reactpages/components/fixed-table-header/components/tableSection.js");

var _tableSection2 = _interopRequireDefault(_tableSection);

var _constants = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/components/fixed-table-header/constants.js");

var _Pager = __webpack_require__(/*! ./Pager */ "./www/js/source/reactpages/components/fixed-table-header/components/Pager.jsx");

var _Pager2 = _interopRequireDefault(_Pager);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

function scrollToTop(element, scrollDuration) {
    var scrollStep = -element.scrollTop / (scrollDuration / 15),
        scrollInterval = setInterval(function () {
        if (element.scrollTop != 0) {
            element.scrollBy(0, scrollStep);
        } else clearInterval(scrollInterval);
    }, 15);
}

function debounce(func, wait) {
    var timeout = void 0;
    return function () {
        var context = this,
            args = arguments;

        var later = function later() {
            func.apply(context, args);
        };

        clearTimeout(timeout);
        timeout = setTimeout(later, wait || _constants.DEFAULT_MILLISECOND_FOR_WAITING);
    };
}

var Table = function (_Component) {
    _inherits(Table, _Component);

    function Table(props) {
        _classCallCheck(this, Table);

        var _this = _possibleConstructorReturn(this, (Table.__proto__ || Object.getPrototypeOf(Table)).call(this, props));

        _initialiseProps.call(_this);

        _this.bodyWrapper = null;
        _this.footerWrapper = null;

        var headerContainerProps = { className: "header-content", isHeader: true };
        _this.Header = (0, _tableSection2.default)(headerContainerProps);

        var bodyContainerProps = { className: "body-content", getRef: function getRef(element) {
                return _this.bodyWrapper = element;
            } };
        _this.Body = (0, _tableSection2.default)(bodyContainerProps);

        var footerContainerProps = { className: "footer-content", getRef: function getRef(element) {
                return _this.footerWrapper = element;
            } };
        _this.Footer = (0, _tableSection2.default)(footerContainerProps);

        var _this$props = _this.props,
            maxWidth = _this$props.maxWidth,
            bodyHeight = _this$props.bodyHeight,
            adjustedHeight = _this$props.adjustedHeight;

        _this.diffWidth = _constants.BODY_WIDTH - maxWidth;
        _this.adjustedHeight = adjustedHeight;

        _this.state = {
            maxWidth: maxWidth,
            contentHeight: bodyHeight,
            columnsWidth: _this._getColumnsWidth(props.header)
        };
        return _this;
    }

    _createClass(Table, [{
        key: '_getColumnsWidth',
        value: function _getColumnsWidth(headerRows) {
            var _this2 = this;

            var headerIsArray = Array.isArray(headerRows);
            if (headerIsArray && headerRows.length === 0) {
                return null;
            }

            var columnsWidth = [];
            var currentNextCellIndexByRow = {};

            var getWidthByCells = function getWidthByCells(cells, currentRowIndex) {
                for (var i = 0; i < cells.length; i++) {
                    var cell = cells[i];
                    if (!cell) {
                        continue;
                    }

                    var cellProps = cell.props;
                    var colspan = Number(cellProps.colSpan);

                    if (colspan && colspan > 1) {
                        var nextRowIndex = currentRowIndex + 1;
                        if (currentNextCellIndexByRow[nextRowIndex] === undefined) {
                            currentNextCellIndexByRow[nextRowIndex] = 0;
                        }
                        var fromNextCellIndex = currentNextCellIndexByRow[nextRowIndex];
                        var toNextCellIndex = fromNextCellIndex + colspan - 1;
                        var nextCells = headerRows[nextRowIndex].props.children;

                        var nextColspanCells = _this2._getNextColspanCells(nextCells, fromNextCellIndex, toNextCellIndex);
                        currentNextCellIndexByRow[nextRowIndex] += nextColspanCells.length;
                        getWidthByCells(nextColspanCells, nextRowIndex);
                    } else {
                        var cellWidth = cellProps.colWidth ? cellProps.colWidth : _constants.DEFAULT_COLUMN_WIDTH;
                        columnsWidth.push(cellWidth);
                    }
                }
            };

            var currentRowIndex = 0;
            var cellList = headerIsArray ? headerRows[currentRowIndex].props.children : headerRows.props.children;
            getWidthByCells(cellList, currentRowIndex);

            return columnsWidth;
        }
    }, {
        key: '_getNextColspanCells',
        value: function _getNextColspanCells(cells, fromCellIndex, toCellIndex) {
            var colspanCells = [];
            for (var i = fromCellIndex; i <= toCellIndex; i++) {
                var cell = cells[i];
                if (cell.props.colSpan) {
                    toCellIndex -= 1;
                }
                colspanCells.push(cell);
            }

            return colspanCells;
        }
    }, {
        key: 'componentDidMount',
        value: function componentDidMount() {
            this._handleResize();
            window.addEventListener('resize', debounce(this._handleResize));
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            window.removeEventListener('resize', debounce(this._handleResize));
        }
    }, {
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            if (this.props.header != prevProps.header) {
                this.setState({ columnsWidth: this._getColumnsWidth(this.props.header) });
            }

            if (prevProps.body != this.props.body) {
                scrollToTop(this.bodyWrapper, 200);
            }
        }
    }, {
        key: '_calculateBodyHeight',
        value: function _calculateBodyHeight() {
            var windowHeight = window.innerHeight;
            var footerHeight = this.footerWrapper ? this.footerWrapper.offsetHeight : 0;
            var tableContentHeight = windowHeight - (this.bodyWrapper.offsetTop + footerHeight + 70) - this.adjustedHeight;

            return tableContentHeight;
        }
    }, {
        key: '_getUpdatedColumnLayout',
        value: function _getUpdatedColumnLayout() {
            var _this3 = this;

            var _props = this.props,
                width = _props.width,
                autoWidth = _props.autoWidth;

            var sumOfColumnWidth = autoWidth ? this.state.maxWidth : width;
            sumOfColumnWidth = sumOfColumnWidth - _constants.SCROLLBAR_WIDTH;

            var newColumnsWidth = this.columnWidthSum && this.state.columnsWidth.map(function (cellWidth) {
                return sumOfColumnWidth / _this3.columnWidthSum * cellWidth;
            });

            return newColumnsWidth;
        }
    }, {
        key: 'render',
        value: function render() {
            var _props2 = this.props,
                width = _props2.width,
                autoWidth = _props2.autoWidth,
                minWidth = _props2.minWidth,
                header = _props2.header,
                body = _props2.body,
                footer = _props2.footer,
                isPaging = _props2.isPaging,
                pageOption = _props2.pageOption,
                onPaging = _props2.onPaging,
                alwaysVisiblePager = _props2.alwaysVisiblePager;

            var maxWidth = this.state.maxWidth;

            var newColumnLayout = this.columnWidthSum && this.columnWidthSum !== width ? this._getUpdatedColumnLayout() : this.state.columnsWidth;

            if (!newColumnLayout) {
                return null;
            }

            var sectionProps = { width: width, autoWidth: autoWidth, minWidth: minWidth, maxWidth: maxWidth };
            var rowLayout = _react2.default.createElement(_RowLayout2.default, { columnLayout: newColumnLayout });

            var Header = this.Header;
            var Body = this.Body;
            var Footer = this.Footer;

            return _react2.default.createElement(
                'div',
                null,
                _react2.default.createElement(
                    'div',
                    { className: 'table-container', style: { maxWidth: maxWidth } },
                    _react2.default.createElement(
                        Header,
                        sectionProps,
                        rowLayout,
                        header
                    ),
                    body && _react2.default.createElement(
                        Body,
                        _extends({}, sectionProps, { maxHeight: this.state.contentHeight }),
                        rowLayout,
                        body
                    ),
                    footer && _react2.default.createElement(
                        Footer,
                        sectionProps,
                        rowLayout,
                        footer
                    )
                ),
                isPaging && pageOption && pageOption.TotalItem > 0 && _react2.default.createElement(_Pager2.default, { alwaysVisible: alwaysVisiblePager, pageOption: pageOption, onPaging: onPaging })
            );
        }
    }, {
        key: 'columnWidthSum',
        get: function get() {
            var columnsWidth = this.state.columnsWidth;
            return !columnsWidth || !columnsWidth.length || columnsWidth.find(function (x) {
                return typeof x !== "number";
            }) ? null : columnsWidth.reduce(function (prev, current) {
                return prev + current;
            }, 0);
        }
    }]);

    return Table;
}(_react.Component);

Table.propTypes = {
    width: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]),
    maxWidth: _propTypes2.default.number,
    minWidth: _propTypes2.default.number,
    autoWidth: _propTypes2.default.bool,
    bodyHeight: _propTypes2.default.number,
    autoHeight: _propTypes2.default.bool,
    header: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.arrayOf(_propTypes2.default.object)]).isRequired,
    body: _propTypes2.default.arrayOf(_propTypes2.default.object),
    footer: _propTypes2.default.arrayOf(_propTypes2.default.object),
    isPaging: _propTypes2.default.bool,
    onPaging: _propTypes2.default.func,
    pageOption: _propTypes2.default.object,
    adjustedHeight: _propTypes2.default.number,
    alwaysVisiblePager: _propTypes2.default.bool
};
Table.defaultProps = {
    width: _constants.MAX_WIDTH,
    maxWidth: _constants.MAX_WIDTH,
    autoWidth: true,
    isPaging: false,
    adjustedHeight: 0,
    bodyHeight: 0,
    autoHeight: true,
    alwaysVisiblePager: false
};

var _initialiseProps = function _initialiseProps() {
    var _this4 = this;

    this._handleResize = function (event) {
        event && event.preventDefault();

        if (_this4.bodyWrapper && _this4.props.autoHeight) {
            var bodyHeight = _this4._calculateBodyHeight();
            if (_this4.state.contentHeight !== bodyHeight) {
                _this4.setState({ contentHeight: bodyHeight });
            }
        }

        var maxWidth = document.body.clientWidth - _this4.diffWidth;
        if (_this4.state.maxWidth !== maxWidth) {
            _this4.setState({ maxWidth: maxWidth });
        }
    };
};

exports.default = Table;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/components/tableSection.js":
/*!*******************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/components/tableSection.js ***!
  \*******************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _constants = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/components/fixed-table-header/constants.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var createTableSection = function createTableSection(extendedContainerProps, extendedTableProps) {
    var _class, _temp;

    return _temp = _class = function (_Component) {
        _inherits(TableSection, _Component);

        function TableSection(props) {
            _classCallCheck(this, TableSection);

            return _possibleConstructorReturn(this, (TableSection.__proto__ || Object.getPrototypeOf(TableSection)).call(this, props));
        }

        _createClass(TableSection, [{
            key: 'render',
            value: function render() {
                var _props = this.props,
                    tableClass = _props.tableClass,
                    width = _props.width,
                    autoWidth = _props.autoWidth,
                    maxWidth = _props.maxWidth,
                    minWidth = _props.minWidth,
                    maxHeight = _props.maxHeight,
                    rest = _objectWithoutProperties(_props, ['tableClass', 'width', 'autoWidth', 'maxWidth', 'minWidth', 'maxHeight']);

                var isHeader = extendedContainerProps.isHeader,
                    getRef = extendedContainerProps.getRef,
                    restContainerProps = _objectWithoutProperties(extendedContainerProps, ['isHeader', 'getRef']);

                return _react2.default.createElement(
                    'div',
                    _extends({}, rest, { style: this.containerStyle, ref: getRef }, restContainerProps),
                    _react2.default.createElement(
                        'div',
                        { className: 'table-wrapper' },
                        _react2.default.createElement(
                            'table',
                            _extends({ style: this.tableStyle, className: tableClass }, extendedTableProps),
                            isHeader ? _react2.default.createElement(
                                'thead',
                                null,
                                this.props.children
                            ) : _react2.default.createElement(
                                'tbody',
                                null,
                                this.props.children
                            )
                        )
                    )
                );
            }
        }, {
            key: 'containerWidth',
            get: function get() {
                var _props2 = this.props,
                    autoWidth = _props2.autoWidth,
                    maxWidth = _props2.maxWidth,
                    width = _props2.width;

                return autoWidth ? maxWidth : width;
            }
        }, {
            key: 'containerStyle',
            get: function get() {
                var _props3 = this.props,
                    minWidth = _props3.minWidth,
                    maxHeight = _props3.maxHeight;


                return _extends({}, this.props.style, {
                    width: this.containerWidth,
                    minWidth: minWidth,
                    maxHeight: maxHeight
                });
            }
        }, {
            key: 'tableStyle',
            get: function get() {
                var minWidth = this.props.minWidth;

                var maxWidth = minWidth ? Math.max(this.containerWidth, minWidth) : this.containerWidth;
                var tableWidth = maxWidth - _constants.SCROLLBAR_WIDTH;

                return {
                    width: tableWidth
                };
            }
        }]);

        return TableSection;
    }(_react.Component), _class.propTypes = {
        tableClass: _propTypes2.default.string,
        width: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]),
        maxWidth: _propTypes2.default.number,
        minWidth: _propTypes2.default.number,
        autoWidth: _propTypes2.default.bool,
        maxHeight: _propTypes2.default.number
    }, _class.defaultProps = {
        tableClass: "table",
        width: _constants.MAX_WIDTH,
        maxWidth: _constants.MAX_WIDTH,
        autoWidth: true
    }, _temp;
};

exports.default = createTableSection;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/constants.js":
/*!*****************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/constants.js ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var SCROLLBAR_WIDTH = exports.SCROLLBAR_WIDTH = 19;
var BODY_WIDTH = exports.BODY_WIDTH = document.body.clientWidth;
var GUTTER = exports.GUTTER = 15;
var MAX_WIDTH = exports.MAX_WIDTH = BODY_WIDTH - GUTTER * 2;
var DEFAULT_MILLISECOND_FOR_WAITING = exports.DEFAULT_MILLISECOND_FOR_WAITING = 500;
var DEFAULT_COLUMN_WIDTH = exports.DEFAULT_COLUMN_WIDTH = 100;

/***/ }),

/***/ "./www/js/source/reactpages/components/fixed-table-header/index.jsx":
/*!**************************************************************************!*\
  !*** ./www/js/source/reactpages/components/fixed-table-header/index.jsx ***!
  \**************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Cell = exports.Row = exports.Table = undefined;

var _Table = __webpack_require__(/*! ./components/Table */ "./www/js/source/reactpages/components/fixed-table-header/components/Table.jsx");

var _Table2 = _interopRequireDefault(_Table);

var _Row = __webpack_require__(/*! ./components/Row */ "./www/js/source/reactpages/components/fixed-table-header/components/Row.jsx");

var _Row2 = _interopRequireDefault(_Row);

var _Cell = __webpack_require__(/*! ./components/Cell */ "./www/js/source/reactpages/components/fixed-table-header/components/Cell.jsx");

var _Cell2 = _interopRequireDefault(_Cell);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.Table = _Table2.default;
exports.Row = _Row2.default;
exports.Cell = _Cell2.default;

/***/ }),

/***/ "./www/js/source/reactpages/components/popup/Popup.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/popup/Popup.jsx ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _constants = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/components/popup/constants.js");

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var Popup = function Popup(props) {
    var title = props.title,
        isShow = props.isShow,
        width = props.width,
        height = props.height,
        isHtml = props.isHtml,
        children = props.children,
        iframeUrl = props.iframeUrl;

    var content = void 0;
    if (iframeUrl) {
        content = _react2.default.createElement('iframe', { frameBorder: '0', src: iframeUrl, width: '100%', height: '100%' });
    } else {
        content = isHtml ? _react2.default.createElement('div', { className: 'popup-content', dangerouslySetInnerHTML: { __html: children } }) : _react2.default.createElement(
            'div',
            { className: 'popup-content' },
            children
        );
    }

    return _react2.default.createElement(
        'div',
        { className: 'popup-container ' + (isShow ? 'show' : 'hide') },
        _react2.default.createElement('div', { className: 'popup-block-body' }),
        _react2.default.createElement(
            'div',
            { className: 'flat-popup', style: { height: height, width: width } },
            _react2.default.createElement(
                'div',
                { className: 'popup-title' },
                _react2.default.createElement(
                    'div',
                    { className: 'txt-title' },
                    title
                ),
                _react2.default.createElement(
                    'a',
                    { className: 'popup-close', href: 'javascript:;', onClick: function onClick() {
                            props.onClose && props.onClose();
                        } },
                    _react2.default.createElement('i', { className: 'icon-close' })
                )
            ),
            content
        )
    );
};

Popup.propTypes = {
    width: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]),
    height: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]),
    title: _propTypes2.default.string,
    isShow: _propTypes2.default.bool,
    onClose: _propTypes2.default.func,
    isHtml: _propTypes2.default.bool,
    iframeUrl: _propTypes2.default.string
};

Popup.defaultProps = {
    width: _constants.DEFAULT_WIDTH,
    height: _constants.DEFAULT_HEIGHT,
    isShow: false,
    isHtml: false,
    iframeUrl: null
};

exports.default = Popup;

/***/ }),

/***/ "./www/js/source/reactpages/components/popup/constants.js":
/*!****************************************************************!*\
  !*** ./www/js/source/reactpages/components/popup/constants.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var DEFAULT_WIDTH = exports.DEFAULT_WIDTH = '55%';
var DEFAULT_HEIGHT = exports.DEFAULT_HEIGHT = '55%';

/***/ }),

/***/ "./www/js/source/reactpages/constants/config.js":
/*!******************************************************!*\
  !*** ./www/js/source/reactpages/constants/config.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
var debuggingConfig = exports.debuggingConfig = {
    name: "__lrfConfig__",
    env: "local"
};

/***/ }),

/***/ "./www/js/source/reactpages/constants/excelStyle.js":
/*!**********************************************************!*\
  !*** ./www/js/source/reactpages/constants/excelStyle.js ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var titleCellStyle = exports.titleCellStyle = {
    font: { color: { rgb: "FFFFFF" } },
    fill: {
        patternType: "solid",
        fgColor: { rgb: "4C6375" }
    }
};

var headerCellStyle = exports.headerCellStyle = {
    font: { bold: true, color: { rgb: "2C2C2C" }, sz: "12" },
    alignment: { horizontal: "center", vertical: "center" },
    fill: { patternType: "solid", fgColor: { rgb: "9EB4C3" } },
    border: { left: { style: "thin", color: { rgb: "D6D6D6" } }, bottom: { style: "thin", color: { rgb: "D6D6D6" } } }
};

var bodyTextStyle = { alignment: { vertical: "center" } };
var bodyDecimalStyle = { numFmt: "#,##0.00" };
var bodyCellStyle = exports.bodyCellStyle = {
    rowspan: bodyTextStyle,
    center: { alignment: _extends({}, bodyTextStyle.alignment, { horizontal: "center" }) },
    left: { alignment: _extends({}, bodyTextStyle.alignment, { horizontal: "left" }) },
    number: { numFmt: "#,##0" },
    decimal: bodyDecimalStyle,
    negativeDecimal: _extends({}, bodyDecimalStyle, { font: { color: { rgb: "FF0000" } } }),
    percent: { numFmt: '#,##0.00%' }
};

var totalTextStyle = { font: { bold: true } };
var totalDecimalStyle = _extends({}, totalTextStyle, { numFmt: "#,##0.00" });
var totalCellStyle = exports.totalCellStyle = {
    default: totalTextStyle,
    rowspan: _extends({}, totalTextStyle, { alignment: { horizontal: "center", vertical: "center" } }),
    center: _extends({}, totalTextStyle, { alignment: { vertical: "center", horizontal: "center" } }),
    number: _extends({}, totalTextStyle, { numFmt: "#,##0" }),
    decimal: totalDecimalStyle,
    negativeDecimal: _extends({}, totalDecimalStyle, { font: { bold: true, color: { rgb: "FF0000" } } })
};

var grandTotalTextStyle = {
    font: { bold: true },
    fill: {
        patternType: "solid",
        fgColor: { rgb: "B8C3C9" }
    },
    border: { left: { style: "thin", color: { rgb: "D6D6D6" } } }
};
var decimalGrandTotalStyle = _extends({}, grandTotalTextStyle, { numFmt: "#,##0.00" });
var grandTotalCellStyle = exports.grandTotalCellStyle = {
    default: grandTotalTextStyle,
    rowspan: _extends({}, grandTotalTextStyle, { alignment: { horizontal: "center", vertical: "center" } }),
    center: _extends({}, grandTotalTextStyle, { alignment: { horizontal: "center" } }),
    number: _extends({}, grandTotalTextStyle, { numFmt: "#,##0" }),
    decimal: decimalGrandTotalStyle,
    negativeDecimal: _extends({}, decimalGrandTotalStyle, { font: { bold: true, color: { rgb: "FF0000" } } }),
    percent: _extends({}, grandTotalTextStyle, { numFmt: "#,##0.00%" })
};

/***/ }),

/***/ "./www/js/source/reactpages/layouts/AppLayout.jsx":
/*!********************************************************!*\
  !*** ./www/js/source/reactpages/layouts/AppLayout.jsx ***!
  \********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var AppLayout = function (_Component) {
  _inherits(AppLayout, _Component);

  function AppLayout(props) {
    _classCallCheck(this, AppLayout);

    return _possibleConstructorReturn(this, (AppLayout.__proto__ || Object.getPrototypeOf(AppLayout)).call(this, props));
  }

  _createClass(AppLayout, [{
    key: 'render',
    value: function render() {
      return _react2.default.createElement(
        'div',
        { id: 'container', className: 'container-fluid' },
        this.props.children
      );
    }
  }]);

  return AppLayout;
}(_react.Component);

exports.default = AppLayout;

/***/ }),

/***/ "./www/js/source/reactpages/lib/AppData.js":
/*!*************************************************!*\
  !*** ./www/js/source/reactpages/lib/AppData.js ***!
  \*************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = window._AppData || {};

/***/ }),

/***/ "./www/js/source/reactpages/lib/createReducer.js":
/*!*******************************************************!*\
  !*** ./www/js/source/reactpages/lib/createReducer.js ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.default = createReducer;
function createReducer(initialState, handlers) {
    return function reducer() {
        var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : initialState;
        var action = arguments[1];

        if (handlers.hasOwnProperty(action.type)) {
            return handlers[action.type](state, action);
        } else {
            return state;
        }
    };
}

/***/ }),

/***/ "./www/js/source/reactpages/lib/excel.js":
/*!***********************************************!*\
  !*** ./www/js/source/reactpages/lib/excel.js ***!
  \***********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EXCEL_TYPE = "xlsx";
var MapExcelType = {
    "number": "n", "string": "s", "boolean": "b", "object": "d"
};

var _XLSX = null;

var Excel = function () {
    function Excel() {
        _classCallCheck(this, Excel);
    }

    _createClass(Excel, null, [{
        key: "process",
        value: function process(callback) {
            __webpack_require__.e(/*! import() | xlsx-style-sparta */ "xlsx-style-sparta").then(__webpack_require__.t.bind(null, /*! xlsx-style-sparta */ "./node_modules/xlsx-style-sparta/xlsx.js", 7)).then(function (XLSX) {
                _XLSX = XLSX;
                callback && callback();
            });
        }
    }, {
        key: "export",
        value: function _export(worksheet, fileName) {
            var option = { bookType: EXCEL_TYPE, bookSST: false, type: 'binary' };
            var workbook = { SheetNames: [fileName], Sheets: {}, Props: {} };
            workbook.Sheets[fileName] = worksheet;

            var workbookOutput = _XLSX.write(workbook, option);
            this.download(workbookOutput, fileName);
        }
    }, {
        key: "getCellType",
        value: function getCellType(value) {
            return MapExcelType[typeof value === "undefined" ? "undefined" : _typeof(value)];
        }
    }, {
        key: "getCellAddress",
        value: function getCellAddress(cellObject) {
            return _XLSX.utils.encode_cell(cellObject);
        }
    }, {
        key: "download",
        value: function download(workbookOut, fileName) {
            var blob = new Blob([this.sheetToArrayBuffer(workbookOut)], {
                type: "application/octet-stream"
            });

            this.downloadFromBolb(blob, fileName);
        }
    }, {
        key: "downloadFromBolb",
        value: function downloadFromBolb(blob, fileName) {
            var fileNameDownload = fileName + "." + EXCEL_TYPE;

            // IE 10+
            if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) {
                navigator.msSaveOrOpenBlob(blob, fileNameDownload);
            } else {
                var element = window.document.createElement('a');
                element.href = window.URL.createObjectURL(blob);
                element.download = fileNameDownload;
                document.body.appendChild(element);
                element.click();
                document.body.removeChild(element);
            }
        }
    }, {
        key: "sheetToArrayBuffer",
        value: function sheetToArrayBuffer(s) {
            var buffer = new ArrayBuffer(s.length);
            var view = new Uint8Array(buffer);
            for (var i = 0; i != s.length; ++i) {
                view[i] = s.charCodeAt(i) & 0xFF;
            }return buffer;
        }
    }]);

    return Excel;
}();

exports.default = Excel;

/***/ }),

/***/ "./www/js/source/reactpages/lib/langHelper.js":
/*!****************************************************!*\
  !*** ./www/js/source/reactpages/lib/langHelper.js ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var DEFAULT_RESOURCE_NAME = "_language_en_US";
var langCode = window.PageLanguage || "en-US";
var resourceName = "_language_" + langCode.replace("-", "_");

var LangHelper = function () {
    function LangHelper() {
        _classCallCheck(this, LangHelper);
    }

    _createClass(LangHelper, null, [{
        key: "getSingleResource",
        value: function getSingleResource(key) {
            var langHandler = window[resourceName];

            return langHandler && langHandler[key] || window[DEFAULT_RESOURCE_NAME] && window[DEFAULT_RESOURCE_NAME][key] || key;
        }
    }, {
        key: "getResources",
        value: function getResources(keys) {
            var langHandler = window[resourceName];

            return keys.map(function (key) {
                return langHandler && langHandler[key] || window[DEFAULT_RESOURCE_NAME] && window[DEFAULT_RESOURCE_NAME][key] || key;
            });
        }
    }, {
        key: "currentLanguage",
        get: function get() {
            return langCode;
        }
    }]);

    return LangHelper;
}();

exports.default = LangHelper;

/***/ }),

/***/ "./www/js/source/reactpages/lib/logger.js":
/*!************************************************!*\
  !*** ./www/js/source/reactpages/lib/logger.js ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _reduxLogger = __webpack_require__(/*! redux-logger */ "./node_modules/redux-logger/dist/redux-logger.js");

var _config = __webpack_require__(/*! ../constants/config */ "./www/js/source/reactpages/constants/config.js");

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var isDev = "development" === "development";
var configName = _config.debuggingConfig.name;
// middleware that logs actions
var loggerMiddleware = (0, _reduxLogger.createLogger)({
    predicate: function predicate(getState, action) {
        return isDev || window[configName] && window[configName].env === _config.debuggingConfig.env && (!window[configName].actionFilter || window[configName].actionFilter === action.type);
    }
});

var Logger = function () {
    function Logger() {
        _classCallCheck(this, Logger);
    }

    _createClass(Logger, null, [{
        key: 'getLoggerMiddleware',
        value: function getLoggerMiddleware() {
            return loggerMiddleware;
        }
    }]);

    return Logger;
}();

exports.default = Logger;

/***/ }),

/***/ "./www/js/source/reactpages/lib/numberFormat.js":
/*!******************************************************!*\
  !*** ./www/js/source/reactpages/lib/numberFormat.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var NumberFormat = function () {
    function NumberFormat() {
        _classCallCheck(this, NumberFormat);
    }

    _createClass(NumberFormat, null, [{
        key: "formatDecimal",
        value: function formatDecimal(value) {
            return this.format(value, 2);
        }
    }, {
        key: "format",
        value: function format(value) {
            var digits = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 0;

            if (typeof value !== "number" || isNaN(value)) {
                return "";
            }

            var roundedValue = this.round(value, digits);

            return roundedValue.toFixed(digits).replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,");
        }
    }, {
        key: "round",
        value: function round(value, digits) {
            var sign = value < 0 ? -1 : 1;
            var absoluteValue = Math.abs(value);
            var rounded = Number(Math.round(absoluteValue + 'e' + digits) + 'e-' + digits);

            return sign * rounded;
        }
    }]);

    return NumberFormat;
}();

exports.default = NumberFormat;

/***/ }),

/***/ "./www/js/source/reactpages/lib/site.js":
/*!**********************************************!*\
  !*** ./www/js/source/reactpages/lib/site.js ***!
  \**********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var _rootUrl = window.rootUrl || "/";
var _xsrfToken = null;

var Site = function () {
    function Site() {
        _classCallCheck(this, Site);
    }

    _createClass(Site, null, [{
        key: "getXsrfToken",
        value: function getXsrfToken() {
            if (!_xsrfToken) {
                _xsrfToken = document.querySelector("input[name=__RequestVerificationToken]").value;
            }

            return _xsrfToken;
        }
    }, {
        key: "resolveClientUrl",
        value: function resolveClientUrl(relativeUrl) {
            return _rootUrl + relativeUrl;
        }
    }, {
        key: "convertDateToString",
        value: function convertDateToString(date) {
            return typeof date === "string" ? date : date.format("MM/DD/YYYY");
        }
    }, {
        key: "asNull",
        value: function asNull(text) {
            if (text != null) {
                text = text.trim();
                if (text === '') {
                    return null;
                }
            }

            return text;
        }
    }, {
        key: "downloadFile",
        value: function downloadFile(data, fileName) {
            var url = window.URL.createObjectURL(new Blob([data]));
            var link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            window.URL.revokeObjectURL(url);
        }
    }, {
        key: "readAsJson",
        value: function readAsJson(blob, callback) {
            var reader = new FileReader();
            reader.onload = function () {
                callback && callback(JSON.parse(reader.result));
            };
            reader.readAsText(blob);
        }
    }]);

    return Site;
}();

exports.default = Site;

/***/ }),

/***/ "./www/js/source/reactpages/lib/store.js":
/*!***********************************************!*\
  !*** ./www/js/source/reactpages/lib/store.js ***!
  \***********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _reduxThunk = __webpack_require__(/*! redux-thunk */ "./node_modules/redux-thunk/lib/index.js");

var _reduxThunk2 = _interopRequireDefault(_reduxThunk);

var _logger = __webpack_require__(/*! ./logger */ "./www/js/source/reactpages/lib/logger.js");

var _logger2 = _interopRequireDefault(_logger);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var Store = function () {
    function Store() {
        _classCallCheck(this, Store);
    }

    _createClass(Store, null, [{
        key: 'createStore',
        value: function createStore(reducer, initialState) {
            var loggerMiddleware = _logger2.default.getLoggerMiddleware();

            var enhancer = (0, _redux.compose)((0, _redux.applyMiddleware)(_reduxThunk2.default, // lets us dispatch() functions
            loggerMiddleware));

            return (0, _redux.createStore)(reducer, initialState, enhancer);
        }
    }]);

    return Store;
}();

exports.default = Store;

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/dataHandlerAction.js":
/*!********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/dataHandlerAction.js ***!
  \********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.getGridData = getGridData;
exports.exportExcel = exportExcel;
exports.setGridData = setGridData;

var _types = __webpack_require__(/*! ./types.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/types.js");

var types = _interopRequireWildcard(_types);

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

var _site = __webpack_require__(/*! ../../../lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var fetchData = function fetchData(params) {
    var url = _site2.default.resolveClientUrl('api/LicenseeWinLossByTimeZoneReport/Index');
    return _axios2.default.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': _site2.default.getXsrfToken()
        }
    });
};

function getGridData(params) {
    return function (dispatch) {
        fetchData(params).then(function (response) {
            dispatch(setGridData(response.data));
        });
    };
}

function exportExcel(params) {
    return function () {
        return fetchData(params);
    };
}

function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/filterHandlerAction.js":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/filterHandlerAction.js ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.changeIsIncludeDrawData = exports.changeIsConvertAll = exports.changeGroupBy = exports.setDateRange = exports.setSelectedReportBy = exports.setSelectedTimeZone = undefined;

var _types = __webpack_require__(/*! ./types.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedTimeZone = exports.setSelectedTimeZone = function setSelectedTimeZone(value) {
    return {
        type: types.SET_TIMEZONE,
        data: value
    };
};

var setSelectedReportBy = exports.setSelectedReportBy = function setSelectedReportBy(value) {
    return {
        type: types.SET_REPORTBY,
        data: value
    };
};

var setDateRange = exports.setDateRange = function setDateRange(dateRange) {
    var range = {
        fromDate: dateRange.startDate,
        toDate: dateRange.endDate
    };

    return {
        type: types.SET_DATE_RANGE,
        data: range
    };
};

var changeGroupBy = exports.changeGroupBy = function changeGroupBy(value) {
    return {
        type: types.CHANGE_GROUPBY,
        data: value
    };
};

var changeIsConvertAll = exports.changeIsConvertAll = function changeIsConvertAll(checked) {
    return {
        type: types.CHANGE_IS_CONVERT_ALL,
        data: checked
    };
};

var changeIsIncludeDrawData = exports.changeIsIncludeDrawData = function changeIsIncludeDrawData(checked) {
    return {
        type: types.CHANGE_IS_INCLUDE_DRAW_DATA,
        data: checked
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/index.js":
/*!********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/index.js ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _dataHandlerAction = __webpack_require__(/*! ./dataHandlerAction.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/dataHandlerAction.js");

var dataHandlerAction = _interopRequireWildcard(_dataHandlerAction);

var _filterHandlerAction = __webpack_require__(/*! ./filterHandlerAction.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/filterHandlerAction.js");

var filterHandlerAction = _interopRequireWildcard(_filterHandlerAction);

var _productAction = __webpack_require__(/*! ../../../actions/productAction */ "./www/js/source/reactpages/actions/productAction.js");

var productAction = _interopRequireWildcard(_productAction);

var _currencyAction = __webpack_require__(/*! ../../../actions/currencyAction */ "./www/js/source/reactpages/actions/currencyAction.js");

var currencyAction = _interopRequireWildcard(_currencyAction);

var _baseCurrencyAction = __webpack_require__(/*! ../../../actions/baseCurrencyAction */ "./www/js/source/reactpages/actions/baseCurrencyAction.js");

var baseCurrencyAction = _interopRequireWildcard(_baseCurrencyAction);

var _loaderAction = __webpack_require__(/*! ../../../actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");

var loaderAction = _interopRequireWildcard(_loaderAction);

var _excludeTestAction = __webpack_require__(/*! ../../../actions/excludeTestAction */ "./www/js/source/reactpages/actions/excludeTestAction.js");

var excludeTestAction = _interopRequireWildcard(_excludeTestAction);

var _userGuideAction = __webpack_require__(/*! ../../../actions/userGuideAction */ "./www/js/source/reactpages/actions/userGuideAction.js");

var userGuideAction = _interopRequireWildcard(_userGuideAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, dataHandlerAction, filterHandlerAction, productAction, currencyAction, baseCurrencyAction, loaderAction, excludeTestAction, userGuideAction);

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/types.js":
/*!********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/types.js ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var FETCH_DATA = exports.FETCH_DATA = 'FETCH_DATA';
var SET_GRID_DATA = exports.SET_GRID_DATA = "SET_GRID_DATA";

var GET_TIMEZONE = exports.GET_TIMEZONE = "GET_TIMEZONE";
var SET_TIMEZONE = exports.SET_TIMEZONE = "SET_TIMEZONE";

var GET_REPORTBY = exports.GET_REPORTBY = "GET_REPORTBY";
var SET_REPORTBY = exports.SET_REPORTBY = "SET_REPORTBY";

var SET_DATE_RANGE = exports.SET_DATE_RANGE = "SET_DATE_RANGE";
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = "GET_DATE_CONSTRAINT";
var CHANGE_GROUPBY = exports.CHANGE_GROUPBY = "CHANGE_GROUPBY";
var CHANGE_IS_CONVERT_ALL = exports.CHANGE_IS_CONVERT_ALL = "CHANGE_IS_CONVERT_ALL";
var CHANGE_IS_INCLUDE_DRAW_DATA = exports.CHANGE_IS_INCLUDE_DRAW_DATA = "CHANGE_IS_INCLUDE_DRAW_DATA";

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/AppContainer.jsx":
/*!*******************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/components/AppContainer.jsx ***!
  \*******************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.AppContainer = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _datagrid = __webpack_require__(/*! ./datagrid */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/datagrid.jsx");

var _datagrid2 = _interopRequireDefault(_datagrid);

var _formFilter = __webpack_require__(/*! ./formFilter */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/formFilter.jsx");

var _formFilter2 = _interopRequireDefault(_formFilter);

var _Loader = __webpack_require__(/*! @jsroot/components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");

var _Loader2 = _interopRequireDefault(_Loader);

var _AppLayout = __webpack_require__(/*! @jsroot/layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");

var _AppLayout2 = _interopRequireDefault(_AppLayout);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var AppContainer = exports.AppContainer = function (_Component) {
  _inherits(AppContainer, _Component);

  function AppContainer(props) {
    _classCallCheck(this, AppContainer);

    return _possibleConstructorReturn(this, (AppContainer.__proto__ || Object.getPrototypeOf(AppContainer)).call(this, props));
  }

  _createClass(AppContainer, [{
    key: 'render',
    value: function render() {
      return _react2.default.createElement(
        _AppLayout2.default,
        null,
        _react2.default.createElement(_Loader2.default, { isLoading: this.props.isLoading }),
        _react2.default.createElement(_formFilter2.default, null),
        _react2.default.createElement(_datagrid2.default, null)
      );
    }
  }]);

  return AppContainer;
}(_react.Component);

function mapStateToProps(state) {
  return Object.assign({}, { isLoading: state.isLoading });
}

exports.default = (0, _reactRedux.connect)(mapStateToProps)(AppContainer);

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/datagrid.jsx":
/*!***************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/components/datagrid.jsx ***!
  \***************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.DataGrid = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _fixedTableHeader = __webpack_require__(/*! @jsroot/components/fixed-table-header */ "./www/js/source/reactpages/components/fixed-table-header/index.jsx");

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _numberFormat = __webpack_require__(/*! @jsroot/lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");

var _numberFormat2 = _interopRequireDefault(_numberFormat);

var _ReportTitle = __webpack_require__(/*! @jsroot/components/ReportTitle */ "./www/js/source/reactpages/components/ReportTitle.jsx");

var _ReportTitle2 = _interopRequireDefault(_ReportTitle);

var _NoDataTitle = __webpack_require__(/*! @jsroot/components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");

var _NoDataTitle2 = _interopRequireDefault(_NoDataTitle);

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/service.js");

var _service2 = _interopRequireDefault(_service);

var _settingConst = __webpack_require__(/*! ../constants/settingConst */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/constants/settingConst.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DataGrid = exports.DataGrid = function (_Component) {
    _inherits(DataGrid, _Component);

    function DataGrid(props) {
        _classCallCheck(this, DataGrid);

        var _this = _possibleConstructorReturn(this, (DataGrid.__proto__ || Object.getPrototypeOf(DataGrid)).call(this, props));

        _this.reportTitle = "";
        _this.groupBy = 0;
        _this.selectedBaseCurrency = "";
        _this.selectedReportBy = 0;
        return _this;
    }

    _createClass(DataGrid, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            if (this.props.gridData !== prevProps.gridData) {
                var _props = this.props,
                    groupBy = _props.groupBy,
                    selectedReportBy = _props.selectedReportBy,
                    selectedTimeZone = _props.selectedTimeZone,
                    dateRange = _props.dateRange,
                    selectedCurrency = _props.selectedCurrency,
                    selectedBaseCurrency = _props.selectedBaseCurrency,
                    isConvertAll = _props.isConvertAll;

                this.groupBy = groupBy;
                this.selectedReportBy = selectedReportBy;
                this.selectedBaseCurrency = selectedBaseCurrency;
                this.reportTitle = _service2.default.getReportTitle(selectedTimeZone, dateRange, selectedCurrency.label, selectedBaseCurrency, isConvertAll);
                this.props.toggleLoader(false);
            }
        }
    }, {
        key: '_buildHeader',
        value: function _buildHeader() {
            var headerText = _service2.default.getHeaderText(this.selectedReportBy, this.groupBy);

            var reportByCell = this.selectedReportBy !== _settingConst.SettingConst.ReportBySummary && _react2.default.createElement(
                _fixedTableHeader.Cell,
                { colWidth: 150, header: true, title: headerText.ReportBy },
                headerText.ReportBy
            );

            return _react2.default.createElement(
                _fixedTableHeader.Row,
                { style: { height: 32 } },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 50, header: true, title: '#' },
                    headerText.Index
                ),
                reportByCell,
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.GroupBy },
                    headerText.GroupBy
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.BetCount },
                    headerText.BetCount
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.Turnover },
                    headerText.Turnover
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.CustomerComm },
                    headerText.CustomerComm
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.LicenseeWL },
                    headerText.LicenseeWL
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.LicenseeComm },
                    headerText.LicenseeComm
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.LicenseeNetWL },
                    headerText.LicenseeNetWL
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.Margin },
                    headerText.Margin
                )
            );
        }
    }, {
        key: '_buildBody',
        value: function _buildBody() {
            var data = this.props.gridData.Data;

            var groupByClassName = this.groupBy === _settingConst.SettingConst.GroupByProduct ? "left-align" : "center-align";

            var rows = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var reportBy = (0, _moment2.default)(item.ReportBy).format(this.selectedReportBy === 2 ? "MM/DD/YYYY" : "MM/YYYY"),
                    betcount = _numberFormat2.default.format(item.BetCount),
                    turnover = _numberFormat2.default.formatDecimal(item.Turnover),
                    customerComm = _numberFormat2.default.formatDecimal(item.CustomerComm),
                    licenseeWL = _numberFormat2.default.formatDecimal(item.LicenseeWinLoss),
                    licenseeComm = _numberFormat2.default.formatDecimal(item.LicenseeComm),
                    licenseeNetWL = _numberFormat2.default.formatDecimal(item.LicenseeNetWinLoss),
                    margin = _numberFormat2.default.formatDecimal(item.Margin) + "%";

                var reportByCell = this.selectedReportBy !== _settingConst.SettingConst.ReportBySummary && _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: reportBy, className: 'center-align' },
                    reportBy
                );

                rows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: 'tr' + i },
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { className: 'center-align' },
                        i + 1
                    ),
                    reportByCell,
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: item.GroupBy, className: groupByClassName },
                        item.GroupBy
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: betcount, className: 'right-align' },
                        betcount
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: turnover, className: 'right-align' },
                        turnover
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: customerComm, className: 'right-align' },
                        customerComm
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: licenseeWL, className: 'right-align ' + (item.LicenseeWinLoss < 0 ? "negative" : "") },
                        licenseeWL
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: licenseeComm, className: 'right-align' },
                        licenseeComm
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: licenseeNetWL, className: 'right-align ' + (item.LicenseeNetWinLoss < 0 ? "negative" : "") },
                        licenseeNetWL
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: margin, className: 'right-align' },
                        margin
                    )
                ));
            }

            return rows;
        }
    }, {
        key: '_buildFooter',
        value: function _buildFooter() {
            var item = this.props.gridData.Total;
            var betcount = _numberFormat2.default.format(item.BetCount),
                turnover = _numberFormat2.default.formatDecimal(item.Turnover),
                customerComm = _numberFormat2.default.formatDecimal(item.CustomerComm),
                licenseeWL = _numberFormat2.default.formatDecimal(item.LicenseeWinLoss),
                licenseeComm = _numberFormat2.default.formatDecimal(item.LicenseeComm),
                licenseeNetWL = _numberFormat2.default.formatDecimal(item.LicenseeNetWinLoss),
                margin = _numberFormat2.default.formatDecimal(item.Margin) + "%";
            var colSpan = this.selectedReportBy === _settingConst.SettingConst.ReportBySummary ? 2 : 3;
            return [_react2.default.createElement(
                _fixedTableHeader.Row,
                { className: 'fixed-table-grant-total', key: 1 },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { className: 'center-align', colSpan: colSpan },
                    _langHelper2.default.getSingleResource("Totalin") + " " + this.selectedBaseCurrency
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: betcount, className: 'right-align' },
                    betcount
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: turnover, className: 'right-align' },
                    turnover
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: customerComm, className: 'right-align' },
                    customerComm
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: licenseeWL, className: 'right-align ' + (item.LicenseeWinLoss < 0 ? "negative" : "") },
                    licenseeWL
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: licenseeComm, className: "right-align" },
                    licenseeComm
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: licenseeNetWL, className: 'right-align ' + (item.LicenseeNetWinLoss < 0 ? "negative" : "") },
                    licenseeNetWL
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: margin, className: "right-align" },
                    margin
                )
            )];
        }
    }, {
        key: '_renderNodata',
        value: function _renderNodata() {
            return _react2.default.createElement(
                'div',
                { className: 'result-content' },
                _react2.default.createElement(_ReportTitle2.default, { content: this.reportTitle }),
                _react2.default.createElement(_NoDataTitle2.default, null)
            );
        }
    }, {
        key: 'render',
        value: function render() {
            var Data = this.props.gridData.Data;

            if (!Data) {
                return null;
            }

            if (!Data.length) {
                return this._renderNodata();
            }

            return _react2.default.createElement(
                'div',
                { className: 'result-content' },
                _react2.default.createElement(_ReportTitle2.default, { content: this.reportTitle }),
                _react2.default.createElement(_fixedTableHeader.Table, {
                    minWidth: 600,
                    autoWidth: true,
                    bodyHeight: 300,
                    header: this._buildHeader(),
                    body: this._buildBody(),
                    footer: this._buildFooter()
                })
            );
        }
    }]);

    return DataGrid;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_index.ActionCreators, dispatch);
}
function mapStateToProps(state) {
    var gridData = state.gridData,
        groupBy = state.groupBy,
        selectedReportBy = state.selectedReportBy,
        selectedTimeZone = state.selectedTimeZone,
        dateRange = state.dateRange,
        selectedCurrency = state.selectedCurrency,
        selectedBaseCurrency = state.selectedBaseCurrency,
        isLoading = state.isLoading,
        isConvertAll = state.isConvertAll;

    return Object.assign({}, { gridData: gridData, groupBy: groupBy, selectedReportBy: selectedReportBy, selectedTimeZone: selectedTimeZone, dateRange: dateRange, selectedCurrency: selectedCurrency, selectedBaseCurrency: selectedBaseCurrency, isLoading: isLoading, isConvertAll: isConvertAll });
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(DataGrid);

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/formFilter.jsx":
/*!*****************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/components/formFilter.jsx ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.FormFilter = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _reactSelect = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");

var _reactSelect2 = _interopRequireDefault(_reactSelect);

var _reactMultiselect = __webpack_require__(/*! @9softstudio/react-multiselect */ "./node_modules/@9softstudio/react-multiselect/dist/index.js");

var _reactMultiselect2 = _interopRequireDefault(_reactMultiselect);

var _index2 = __webpack_require__(/*! @jsroot/components/date-range/index */ "./www/js/source/reactpages/components/date-range/index.js");

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _reportExcel = __webpack_require__(/*! ../lib/reportExcel */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/reportExcel.js");

var _reportExcel2 = _interopRequireDefault(_reportExcel);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/service.js");

var _service2 = _interopRequireDefault(_service);

var _settingConst = __webpack_require__(/*! ../constants/settingConst */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/constants/settingConst.js");

var _UserGuide = __webpack_require__(/*! @jsroot/components/UserGuide */ "./www/js/source/reactpages/components/UserGuide.jsx");

var _UserGuide2 = _interopRequireDefault(_UserGuide);

var _AppData = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");

var _AppData2 = _interopRequireDefault(_AppData);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var RANGE_MONTH_CONSTRAINT = 3;

var FormFilter = exports.FormFilter = function (_Component) {
    _inherits(FormFilter, _Component);

    function FormFilter(props) {
        _classCallCheck(this, FormFilter);

        var _this = _possibleConstructorReturn(this, (FormFilter.__proto__ || Object.getPrototypeOf(FormFilter)).call(this, props));

        _this.submit = function (event) {
            event.preventDefault();
            var params = _this._getParams();
            if (!params) {
                return;
            }

            _this.props.toggleLoader(true);
            _this.props.getGridData(params);
        };

        _this.exportExcel = function (event) {
            event.preventDefault();
            var _this$props = _this.props,
                selectedTimeZone = _this$props.selectedTimeZone,
                dateRange = _this$props.dateRange,
                selectedCurrency = _this$props.selectedCurrency,
                selectedBaseCurrency = _this$props.selectedBaseCurrency;

            var params = _this._getParams();
            if (!params) {
                return;
            }

            _this.props.toggleLoader(true);
            _this.props.exportExcel(params).then(function (response) {
                if (response.data.Data.length === 0) {
                    alert(_langHelper2.default.getSingleResource("NoInfo"));
                    _this.props.toggleLoader(false);
                    return;
                }

                var title = _service2.default.getReportTitle(selectedTimeZone, dateRange, selectedCurrency.label, selectedBaseCurrency, _this.props.isConvertAll);
                var excel = new _reportExcel2.default(response.data, title, _this.props.selectedReportBy, _this.props.groupBy, selectedBaseCurrency);
                excel.export(function () {
                    _this.props.toggleLoader(false);
                });
            });
        };

        _this.showUserGuide = function () {
            var userGuideContent = _this.props.userGuideContent;

            if (!userGuideContent || userGuideContent.length === 0) {
                _this.props.getUserGuideContent('LicenseeWLByTimeZone');
            }
        };

        _this.changeGroupBy = function (value) {
            _this.props.changeGroupBy(value);
            if (value === _settingConst.SettingConst.GroupByProduct && _this.props.selectedCurrency.value === 0) {
                _this.props.changeIsConvertAll(true);
            }
        };

        _this.productChangeHandler = function (currentItem, selectedItemsKey) {
            _this.props.setSelectedProduct(selectedItemsKey);
        };

        _this.applyDateRangeHandler = function (date) {
            var isValidMonthRange = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : true;

            if (isValidMonthRange) {
                _this.props.setDateRange(date);
            } else {
                var message = _langHelper2.default.getSingleResource("DateRangeExceedMsg").replace("{0}", RANGE_MONTH_CONSTRAINT);
                alert(message);
            }
        };

        return _this;
    }

    _createClass(FormFilter, [{
        key: '_getParams',
        value: function _getParams() {
            if (!this.isValidModel) {
                return null;
            }

            var _props$dateRange = this.props.dateRange,
                fromDate = _props$dateRange.fromDate,
                toDate = _props$dateRange.toDate;


            return {
                data: {
                    BaseCurrency: this.props.selectedBaseCurrency,
                    CurrencyId: this.props.selectedCurrency.value,
                    ProductCodeList: this.props.selectedProduct,
                    IsConvertAll: this.props.isConvertAll,
                    IsIncludeDrawData: this.props.isIncludeDrawData,
                    FromDate: _site2.default.convertDateToString(fromDate),
                    ToDate: _site2.default.convertDateToString(toDate),
                    TimeZone: this.props.selectedTimeZone,
                    ReportBy: this.props.selectedReportBy,
                    GroupBy: this.props.groupBy,
                    IsExcludeTestData: this.props.isExcludeTest
                }
            };
        }
    }, {
        key: '_renderTimeZoneSelectList',
        value: function _renderTimeZoneSelectList() {
            var _this2 = this;

            var _props = this.props,
                timeZoneList = _props.timeZoneList,
                selectedTimeZone = _props.selectedTimeZone;


            return _react2.default.createElement(_reactSelect2.default, { instanceId: 'ddlTimeZone', value: selectedTimeZone, options: timeZoneList,
                onChange: function onChange(item) {
                    return _this2.props.setSelectedTimeZone(item.value);
                } });
        }
    }, {
        key: '_renderReportBySelectList',
        value: function _renderReportBySelectList() {
            var _this3 = this;

            var _props2 = this.props,
                reportByList = _props2.reportByList,
                selectedReportBy = _props2.selectedReportBy;


            return _react2.default.createElement(_reactSelect2.default, { instanceId: 'ddlReportBy', value: selectedReportBy, options: reportByList,
                onChange: function onChange(item) {
                    return _this3.props.setSelectedReportBy(item.value);
                } });
        }
    }, {
        key: '_renderProductSelectList',
        value: function _renderProductSelectList() {
            var productOptions = _AppData2.default.productOptions;
            return productOptions && productOptions.length ? _react2.default.createElement(_reactMultiselect2.default, { id: 'ProductList',
                language: _langHelper2.default.currentLanguage,
                dataSource: productOptions,
                keyField: "Code",
                valueField: "Name",
                statusField: "Status",
                onChange: this.productChangeHandler }) : null;
        }
    }, {
        key: '_renderCurrencySelectList',
        value: function _renderCurrencySelectList() {
            var _this4 = this;

            var selectedCurrency = this.props.selectedCurrency;

            var currencyList = _AppData2.default.currencyList;
            return _react2.default.createElement(_reactSelect2.default, { instanceId: 'ddlCurrency',
                value: selectedCurrency.value,
                options: currencyList,
                onChange: function onChange(item) {
                    return _this4.props.setSelectedCurrency(item);
                } });
        }
    }, {
        key: '_renderBaseCurrencySelectList',
        value: function _renderBaseCurrencySelectList() {
            var _this5 = this;

            var selectedBaseCurrency = this.props.selectedBaseCurrency;

            var baseCurrencyList = _AppData2.default.baseCurrencyList;
            return baseCurrencyList && baseCurrencyList.length ? _react2.default.createElement(_reactSelect2.default, { instanceId: 'ddlBased',
                value: selectedBaseCurrency,
                options: baseCurrencyList,
                onChange: function onChange(item) {
                    return _this5.props.setSelectedBaseCurrency(item.value);
                } }) : null;
        }
    }, {
        key: '_renderDateRange',
        value: function _renderDateRange() {
            var _props3 = this.props,
                dateRange = _props3.dateRange,
                dateConstraint = _props3.dateConstraint;


            return _react2.default.createElement(_index2.DateRange, { startDate: dateRange.fromDate, endDate: dateRange.toDate,
                minDate: dateConstraint.minDate, maxDate: dateConstraint.maxDate,
                today: dateRange.toDate, limitedMonthRange: RANGE_MONTH_CONSTRAINT,
                onChange: this.applyDateRangeHandler });
        }
    }, {
        key: '_renderUserGuide',
        value: function _renderUserGuide() {
            var userGuideContent = this.props.userGuideContent;

            return _react2.default.createElement(_UserGuide2.default, {
                title: _langHelper2.default.getSingleResource("UserGuide"),
                contentTitle: _langHelper2.default.getSingleResource("WinLossByTimeZone"),
                onClick: this.showUserGuide,
                content: userGuideContent });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this6 = this;

            return _react2.default.createElement(
                'div',
                { className: 'page-filter' },
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-4 col-sm-7 col-lg-3 col-xl-3' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("Date")
                            ),
                            this._renderDateRange()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("TimeZone")
                            ),
                            this._renderTimeZoneSelectList()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-4 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("Product")
                            ),
                            this._renderProductSelectList()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("Currency")
                            ),
                            this._renderCurrencySelectList()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("BaseCurrency")
                            ),
                            this._renderBaseCurrencySelectList()
                        )
                    )
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-4 col-sm-4 col-lg-3 col-xl-3' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("GroupBy")
                            ),
                            _react2.default.createElement('input', { type: 'radio', value: 1, checked: this.props.groupBy == 1, name: 'GroupBy', id: 'GroupByProduct',
                                onChange: function onChange() {
                                    return _this6.changeGroupBy(1);
                                } }),
                            _react2.default.createElement(
                                'label',
                                { htmlFor: 'GroupByProduct' },
                                _langHelper2.default.getSingleResource("Product")
                            ),
                            _react2.default.createElement('input', { type: 'radio', value: 2, name: 'GroupBy', id: 'GroupByCurrency', checked: this.props.groupBy == 2,
                                onChange: function onChange() {
                                    return _this6.changeGroupBy(2);
                                } }),
                            _react2.default.createElement(
                                'label',
                                { htmlFor: 'GroupByCurrency' },
                                _langHelper2.default.getSingleResource("Currency")
                            )
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("ReportBy")
                            ),
                            this._renderReportBySelectList()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'div',
                                { className: 'checkbox-group' },
                                _react2.default.createElement('input', { type: 'checkbox', className: 'checkbox', checked: this.props.isIncludeDrawData, id: 'IncludeDrawData',
                                    onChange: function onChange() {
                                        return _this6.props.changeIsIncludeDrawData(!_this6.props.isIncludeDrawData);
                                    } }),
                                _react2.default.createElement(
                                    'label',
                                    { htmlFor: 'IncludeDrawData' },
                                    _langHelper2.default.getSingleResource("IncludeDrawData")
                                )
                            )
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-4 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'div',
                                { className: 'checkbox-group' },
                                _react2.default.createElement('input', { type: 'checkbox', className: 'checkbox', checked: this.props.groupBy === 1 && this.props.selectedCurrency.value == 0 || this.props.isConvertAll, id: 'IsConvertAll',
                                    disabled: this.props.groupBy === 1 && this.props.selectedCurrency.value == 0,
                                    onChange: function onChange() {
                                        return _this6.props.changeIsConvertAll(!_this6.props.isConvertAll);
                                    } }),
                                _react2.default.createElement(
                                    'label',
                                    { htmlFor: 'IsConvertAll' },
                                    _langHelper2.default.getSingleResource("ConvertAllFiguresToBaseCurrency")
                                )
                            )
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'div',
                                { className: 'checkbox-group' },
                                _react2.default.createElement('input', { type: 'checkbox', className: 'checkbox', checked: this.props.isExcludeTest, id: 'ExcludeTest',
                                    onChange: function onChange() {
                                        return _this6.props.changeIsExcludeTest(!_this6.props.isExcludeTest);
                                    } }),
                                _react2.default.createElement(
                                    'label',
                                    { style: { marginLeft: "0px" }, htmlFor: 'ExcludeTest' },
                                    _langHelper2.default.getSingleResource("ExcludeTestData")
                                )
                            )
                        )
                    )
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-1 col-sm-1 col-lg-1 col-xl-1' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'div',
                                { className: 'button-group' },
                                _react2.default.createElement(
                                    'button',
                                    { disabled: !this.isValidModel, type: 'button', className: 'btn btn-submit', onClick: this.submit },
                                    _langHelper2.default.getSingleResource("Submit")
                                )
                            )
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'caret-block' },
                        _react2.default.createElement('i', { className: 'icon-excel right', onClick: this.exportExcel, title: _langHelper2.default.getSingleResource("ExportToExcel") }),
                        this._renderUserGuide()
                    )
                )
            );
        }
    }, {
        key: 'isValidModel',
        get: function get() {
            return this.props.selectedProduct !== "";
        }
    }]);

    return FormFilter;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_index.ActionCreators, dispatch);
}
function mapStateToProps(state) {
    var gridData = state.gridData,
        rest = _objectWithoutProperties(state, ['gridData']);

    return Object.assign({}, rest);
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(FormFilter);

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/index.jsx":
/*!************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/components/index.jsx ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _AppContainer = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/AppContainer.jsx");

var _AppContainer2 = _interopRequireDefault(_AppContainer);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../reducers/index.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/index.js");

var _index2 = _interopRequireDefault(_index);

var _store = __webpack_require__(/*! @jsroot/lib/store */ "./www/js/source/reactpages/lib/store.js");

var _store2 = _interopRequireDefault(_store);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var defaultState = Object.assign({}, window.initialState, { userGuideContent: '' });
var store = _store2.default.createStore(_index2.default, defaultState);

var App = function (_React$Component) {
  _inherits(App, _React$Component);

  function App(props) {
    _classCallCheck(this, App);

    return _possibleConstructorReturn(this, (App.__proto__ || Object.getPrototypeOf(App)).call(this, props));
  }

  _createClass(App, [{
    key: 'render',
    value: function render() {
      return _react2.default.createElement(
        _reactRedux.Provider,
        { store: store },
        _react2.default.createElement(_AppContainer2.default, null)
      );
    }
  }]);

  return App;
}(_react2.default.Component);

exports.default = App;


_reactDom2.default.render(_react2.default.createElement(App, null), document.getElementById("app-container"));

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/constants/settingConst.js":
/*!*****************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/constants/settingConst.js ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
var SettingConst = exports.SettingConst = {
    GroupByProduct: 1,
    ReportBySummary: 1,
    ReportByDaily: 2
};

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/reportExcel.js":
/*!**********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/reportExcel.js ***!
  \**********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _ref, _ref2, _ref3, _ref4, _ref5, _ref6, _ref7, _ref8, _ref9, _ref10;

var _numberFormat = __webpack_require__(/*! ../../../lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");

var _numberFormat2 = _interopRequireDefault(_numberFormat);

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _excel = __webpack_require__(/*! ../../../lib/excel */ "./www/js/source/reactpages/lib/excel.js");

var _excel2 = _interopRequireDefault(_excel);

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _excelStyle = __webpack_require__(/*! ../../../constants/excelStyle */ "./www/js/source/reactpages/constants/excelStyle.js");

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/service.js");

var _service2 = _interopRequireDefault(_service);

var _settingConst = __webpack_require__(/*! ../constants/settingConst */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/constants/settingConst.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var TITLE_ROW_INDEX = 0;
var HEADER_ROW_INDEX = 1;
var DATA_ROW_INDEX = 2;

var INDEX_COLUMN_INDEX = 0;
var REPORTBY_COLUMN_INDEX = 1;
var GROUPBY_COLUMN_INDEX = 2;
var BETCOUNT_COLUMN_INDEX = 3;
var TURNOVER_COLUMN_INDEX = 4;
var CUSTOMERCOMM_COLUMN_INDEX = 5;
var LICENSEEWL_COLUMN_INDEX = 6;
var LICENSEECOMM_COLUMN_INDEX = 7;
var LICENSEENETWL_COLUMN_INDEX = 8;
var MARGIN_COLUMN_INDEX = 9;

var COLUMN_COUNT = 10;

var MAIN_SECTION_STYLE = "mainSectionStyle";
var TOTAL_SECTION_STYLE = "totalSectionStyle";
var GRAND_TOTAL_SECTION_STYLE = "grandTotalSectionStyle";

var fields = {
    Index: "Index",
    ReportBy: "ReportBy",
    GroupBy: "GroupBy",
    BetCount: "BetCount",
    Turnover: "Turnover",
    CustomerComm: "CustomerComm",
    LicenseeWL: "LicenseeWinLoss",
    LicenseeComm: "LicenseeComm",
    LicenseeNetWL: "LicenseeNetWinLoss",
    Margin: "Margin"
};

var columnConfigs = [(_ref = {
    field: "#"
}, _defineProperty(_ref, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.center), _defineProperty(_ref, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.number), _ref), (_ref2 = {
    field: fields.ReportBy
}, _defineProperty(_ref2, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.center), _defineProperty(_ref2, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.number), _ref2), (_ref3 = {
    field: fields.GroupBy
}, _defineProperty(_ref3, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.rowspan), _defineProperty(_ref3, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.number), _ref3), (_ref4 = {
    field: fields.BetCount
}, _defineProperty(_ref4, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.number), _defineProperty(_ref4, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.number), _ref4), (_ref5 = {
    field: fields.Turnover
}, _defineProperty(_ref5, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.decimal), _defineProperty(_ref5, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.decimal), _ref5), (_ref6 = {
    field: fields.CustomerComm
}, _defineProperty(_ref6, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.decimal), _defineProperty(_ref6, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.decimal), _ref6), (_ref7 = {
    field: fields.LicenseeWL
}, _defineProperty(_ref7, MAIN_SECTION_STYLE, { positive: _excelStyle.bodyCellStyle.decimal, negative: _excelStyle.bodyCellStyle.negativeDecimal }), _defineProperty(_ref7, GRAND_TOTAL_SECTION_STYLE, { positive: _excelStyle.grandTotalCellStyle.decimal, negative: _excelStyle.grandTotalCellStyle.negativeDecimal }), _ref7), (_ref8 = {
    field: fields.LicenseeComm
}, _defineProperty(_ref8, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.decimal), _defineProperty(_ref8, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.decimal), _ref8), (_ref9 = {
    field: fields.LicenseeNetWL
}, _defineProperty(_ref9, MAIN_SECTION_STYLE, { positive: _excelStyle.bodyCellStyle.decimal, negative: _excelStyle.bodyCellStyle.negativeDecimal }), _defineProperty(_ref9, GRAND_TOTAL_SECTION_STYLE, { positive: _excelStyle.grandTotalCellStyle.decimal, negative: _excelStyle.grandTotalCellStyle.negativeDecimal }), _ref9), (_ref10 = {
    field: fields.Margin
}, _defineProperty(_ref10, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.percent), _defineProperty(_ref10, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.percent), _ref10)];

var ReportExcel = function () {
    function ReportExcel() {
        var gridData = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
        var title = arguments[1];
        var reportBy = arguments[2];
        var groupBy = arguments[3];
        var baseCurrency = arguments[4];

        _classCallCheck(this, ReportExcel);

        this.worksheet = {};
        this.title = title;
        this.fileName = "WinLossByTimeZone";
        this.gridData = gridData;
        this.reportBy = reportBy;
        this.groupBy = groupBy;
        this.baseCurrency = baseCurrency;
        this.currentRowIndex = DATA_ROW_INDEX;
        this.mergeConfig = [];
    }

    _createClass(ReportExcel, [{
        key: 'export',
        value: function _export(onFinished) {
            var _this = this;

            if (!this.gridData.Data || !this.gridData.Data.length) {
                onFinished && onFinished();
                return;
            }

            _excel2.default.process(function () {
                _this._createTitle();
                _this._createHeader();
                _this._createSheetData();
                _this._createExcelConfig();

                _excel2.default.export(_this.worksheet, _this.fileName);
                onFinished && onFinished();
            });
        }
    }, {
        key: '_createTitle',
        value: function _createTitle() {
            this.mergeConfig.push({ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: COLUMN_COUNT - 1 } });
            var address = _excel2.default.getCellAddress({ c: 0, r: TITLE_ROW_INDEX });
            this.worksheet[address] = { t: "s", v: this.title, s: _excelStyle.titleCellStyle };
        }
    }, {
        key: '_createHeader',
        value: function _createHeader() {
            var headerText = _service2.default.getHeaderText(this.reportBy, this.groupBy);
            var keys = Object.keys(headerText);
            for (var i = 0; i < keys.length; i++) {
                var address = _excel2.default.getCellAddress({ c: i, r: HEADER_ROW_INDEX });
                var header = headerText[keys[i]];
                this.worksheet[address] = { t: "s", v: header, s: _excelStyle.headerCellStyle };
            }
        }
    }, {
        key: '_createSheetData',
        value: function _createSheetData() {
            this._createMainData();
            this._createFooterData();
        }
    }, {
        key: '_createMainData',
        value: function _createMainData() {
            var mainData = this.gridData.Data;

            for (var i = 0; i < mainData.length; i++) {
                var item = mainData[i];
                var cellAddress = _excel2.default.getCellAddress({ c: INDEX_COLUMN_INDEX, r: this.currentRowIndex });
                this.worksheet[cellAddress] = { t: "s", v: i + 1, s: _excelStyle.bodyCellStyle.center };
                this._createRowByColumnIndex(item, MAIN_SECTION_STYLE, REPORTBY_COLUMN_INDEX, MARGIN_COLUMN_INDEX);
                this.currentRowIndex++;
            }
        }
    }, {
        key: '_createFooterData',
        value: function _createFooterData() {
            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 0 }, e: { r: this.currentRowIndex, c: GROUPBY_COLUMN_INDEX } });
            var totalData = this.gridData.Total;

            var cellAddress = _excel2.default.getCellAddress({ c: INDEX_COLUMN_INDEX, r: this.currentRowIndex });
            this.worksheet[cellAddress] = { t: "s", v: _langHelper2.default.getSingleResource("Totalin") + " " + this.baseCurrency, s: _excelStyle.grandTotalCellStyle.center };
            this._createRowByColumnIndex(totalData, GRAND_TOTAL_SECTION_STYLE, BETCOUNT_COLUMN_INDEX, MARGIN_COLUMN_INDEX);
        }
    }, {
        key: '_createExcelConfig',
        value: function _createExcelConfig() {
            var columnModels = [{ wpx: 50 }, { wpx: 100, hidden: this.reportBy === _settingConst.SettingConst.ReportBySummary }, { wpx: 200 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 150 }, { wpx: 150 }, { wpx: 150 }, { wpx: 150 }, { wpx: 150 }];

            var cellStart = _excel2.default.getCellAddress({ r: 0, c: 0 });
            var cellEnd = _excel2.default.getCellAddress({ r: this.currentRowIndex, c: COLUMN_COUNT - 1 });

            this.worksheet["!merges"] = this.mergeConfig;
            this.worksheet["!ref"] = cellStart + ':' + cellEnd;

            this.worksheet['!cols'] = columnModels;
        }
    }, {
        key: '_createRowByColumnIndex',
        value: function _createRowByColumnIndex(item, sectionStyle, startIndex, endIndex) {
            for (var i = startIndex; i <= endIndex; i++) {
                var _getCellObject2 = this._getCellObject(item, sectionStyle, i),
                    cellAddress = _getCellObject2.cellAddress,
                    type = _getCellObject2.type,
                    value = _getCellObject2.value,
                    style = _getCellObject2.style;

                this.worksheet[cellAddress] = { t: type, v: value, s: style };
            }
        }
    }, {
        key: '_getCellObject',
        value: function _getCellObject(item, sectionStyle, columnIndex) {
            var columnConfig = columnConfigs[columnIndex];
            if (columnIndex === GROUPBY_COLUMN_INDEX) {
                columnConfig[MAIN_SECTION_STYLE] = this.groupBy === _settingConst.SettingConst.GroupByProduct ? _excelStyle.bodyCellStyle.rowspan : _excelStyle.bodyCellStyle.center;
            }

            var field = columnConfig.field;

            var cellAddress = _excel2.default.getCellAddress({ c: columnIndex, r: this.currentRowIndex });
            var type = _excel2.default.getCellType(item[columnConfig.field]);
            var value = item[field] !== null ? item[field] : "";

            var style = columnConfig[sectionStyle];
            if (columnIndex === LICENSEEWL_COLUMN_INDEX || columnIndex === LICENSEENETWL_COLUMN_INDEX) {
                style = value < 0 ? style.negative : style.positive;
            } else if (columnIndex === REPORTBY_COLUMN_INDEX) {
                value = (0, _moment2.default)(value).format(this.reportBy === _settingConst.SettingConst.ReportByDaily ? "MM/DD/YYYY" : "MM/YYYY");
            } else if (columnIndex === MARGIN_COLUMN_INDEX) {
                value /= 100; // percentage
            }

            return { cellAddress: cellAddress, type: type, value: value, style: style };
        }
    }]);

    return ReportExcel;
}();

exports.default = ReportExcel;

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/service.js":
/*!******************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/lib/service.js ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(/*! ../../../lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var headerText = {
    Index: "#",
    ReportBy: "",
    GroupBy: "",
    BetCount: _langHelper2.default.getSingleResource("BetCount"),
    Turnover: _langHelper2.default.getSingleResource("Turnover"),
    CustomerComm: _langHelper2.default.getSingleResource("CustomerComm"),
    LicenseeWL: _langHelper2.default.getSingleResource("LicenseeWL"),
    LicenseeComm: _langHelper2.default.getSingleResource("LicenseeComm"),
    LicenseeNetWL: _langHelper2.default.getSingleResource("LicenseeNetWL"),
    Margin: _langHelper2.default.getSingleResource("Margin")
};

var Service = function () {
    function Service() {
        _classCallCheck(this, Service);
    }

    _createClass(Service, null, [{
        key: 'getReportTitle',
        value: function getReportTitle(timeZone, dateRange, currency, baseCurrency, isConvertAll) {
            var fromDate = _site2.default.convertDateToString(dateRange.fromDate);
            var toDate = _site2.default.convertDateToString(dateRange.toDate);
            var viewInByCurrency = isConvertAll ? ", " + _langHelper2.default.getSingleResource("BaseCurrency") + ": " + baseCurrency : "";
            var timeZoneText = "GMT" + (timeZone < 0 ? timeZone : "+" + timeZone);
            var currencyText = _langHelper2.default.getSingleResource("Currency") + ": " + currency;
            return _langHelper2.default.getSingleResource("WinLossByTimeZone") + ', ' + timeZoneText + ', ' + fromDate + ' \u2192 ' + toDate + ', ' + currencyText + viewInByCurrency;
        }
    }, {
        key: 'getHeaderText',
        value: function getHeaderText(reportBy, groupBy) {
            headerText.ReportBy = _langHelper2.default.getSingleResource(reportBy === 2 ? "Date" : "Month");
            headerText.GroupBy = _langHelper2.default.getSingleResource(groupBy === 1 ? "Product" : "Currency");

            return headerText;
        }
    }]);

    return Service;
}();

exports.default = Service;

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/dataHandlerReducer.js":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/dataHandlerReducer.js ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.gridData = undefined;

var _createReducer2 = __webpack_require__(/*! ../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

var _types = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var gridData = exports.gridData = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_GRID_DATA, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/filterHandlerReducer.js":
/*!************************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/filterHandlerReducer.js ***!
  \************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.dateConstraint = exports.isIncludeDrawData = exports.isConvertAll = exports.groupBy = exports.dateRange = exports.selectedReportBy = exports.reportByList = exports.selectedTimeZone = exports.timeZoneList = undefined;

var _createReducer10 = __webpack_require__(/*! ../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer11 = _interopRequireDefault(_createReducer10);

var _types = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var timeZoneList = exports.timeZoneList = (0, _createReducer11.default)({}, _defineProperty({}, types.GET_TIMEZONE, function (state, action) {
    return action.data;
}));

var selectedTimeZone = exports.selectedTimeZone = (0, _createReducer11.default)({}, _defineProperty({}, types.SET_TIMEZONE, function (state, action) {
    return action.data;
}));

var reportByList = exports.reportByList = (0, _createReducer11.default)({}, _defineProperty({}, types.GET_REPORTBY, function (state, action) {
    return action.data;
}));

var selectedReportBy = exports.selectedReportBy = (0, _createReducer11.default)({}, _defineProperty({}, types.SET_REPORTBY, function (state, action) {
    return action.data;
}));

var dateRange = exports.dateRange = (0, _createReducer11.default)({}, _defineProperty({}, types.SET_DATE_RANGE, function (state, action) {
    return action.data;
}));

var groupBy = exports.groupBy = (0, _createReducer11.default)({}, _defineProperty({}, types.CHANGE_GROUPBY, function (state, action) {
    return action.data;
}));

var isConvertAll = exports.isConvertAll = (0, _createReducer11.default)({}, _defineProperty({}, types.CHANGE_IS_CONVERT_ALL, function (state, action) {
    return action.data;
}));

var isIncludeDrawData = exports.isIncludeDrawData = (0, _createReducer11.default)({}, _defineProperty({}, types.CHANGE_IS_INCLUDE_DRAW_DATA, function (state, action) {
    return action.data;
}));

var dateConstraint = exports.dateConstraint = (0, _createReducer11.default)({}, _defineProperty({}, types.GET_DATE_CONSTRAINT, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/index.js":
/*!*********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/index.js ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _dataHandlerReducer = __webpack_require__(/*! ./dataHandlerReducer.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/dataHandlerReducer.js");

var dataHandlerReducer = _interopRequireWildcard(_dataHandlerReducer);

var _filterHandlerReducer = __webpack_require__(/*! ./filterHandlerReducer.js */ "./www/js/source/reactpages/pages/licenseewl-by-timezone/reducers/filterHandlerReducer.js");

var filterHandlerReducer = _interopRequireWildcard(_filterHandlerReducer);

var _productReducer = __webpack_require__(/*! ../../../reducers/productReducer */ "./www/js/source/reactpages/reducers/productReducer.js");

var productReducer = _interopRequireWildcard(_productReducer);

var _currencyReducer = __webpack_require__(/*! ../../../reducers/currencyReducer */ "./www/js/source/reactpages/reducers/currencyReducer.js");

var currencyReducer = _interopRequireWildcard(_currencyReducer);

var _baseCurrencyReducer = __webpack_require__(/*! ../../../reducers/baseCurrencyReducer */ "./www/js/source/reactpages/reducers/baseCurrencyReducer.js");

var baseCurrencyReducer = _interopRequireWildcard(_baseCurrencyReducer);

var _loaderReducer = __webpack_require__(/*! ../../../reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");

var loaderReducer = _interopRequireWildcard(_loaderReducer);

var _excludeTestReducer = __webpack_require__(/*! ../../../reducers/excludeTestReducer */ "./www/js/source/reactpages/reducers/excludeTestReducer.js");

var excludeTestReducer = _interopRequireWildcard(_excludeTestReducer);

var _userGuideReducer = __webpack_require__(/*! ../../../reducers/userGuideReducer */ "./www/js/source/reactpages/reducers/userGuideReducer.js");

var userGuideReducer = _interopRequireWildcard(_userGuideReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = (0, _redux.combineReducers)(Object.assign(dataHandlerReducer, filterHandlerReducer, productReducer, currencyReducer, baseCurrencyReducer, loaderReducer, excludeTestReducer, userGuideReducer));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/baseCurrencyReducer.js":
/*!******************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/baseCurrencyReducer.js ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.baseCurrencyList = exports.selectedBaseCurrency = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer3 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer4 = _interopRequireDefault(_createReducer3);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var selectedBaseCurrency = exports.selectedBaseCurrency = (0, _createReducer4.default)({}, _defineProperty({}, types.SET_SELECTED_BASECURRENCY, function (state, action) {
    return action.data;
}));

var baseCurrencyList = exports.baseCurrencyList = (0, _createReducer4.default)({}, _defineProperty({}, types.SET_BASECURRENCY_LIST, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/currencyReducer.js":
/*!**************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/currencyReducer.js ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.selectedCurrency = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var selectedCurrency = exports.selectedCurrency = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_SELECTED_CURRENCY, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/excludeTestReducer.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/excludeTestReducer.js ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.isExcludeTest = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var isExcludeTest = exports.isExcludeTest = (0, _createReducer3.default)({}, _defineProperty({}, types.CHANGE_IS_EXCLUDE_TEST, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/loaderReducer.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/loaderReducer.js ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.isLoading = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var isLoading = exports.isLoading = (0, _createReducer3.default)({}, _defineProperty({}, types.TOGGLE_LOADER, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/productReducer.js":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/productReducer.js ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.selectedProduct = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var selectedProduct = exports.selectedProduct = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_SELECTED_PRODUCT, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/userGuideReducer.js":
/*!***************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/userGuideReducer.js ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.userGuideContent = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var userGuideContent = exports.userGuideContent = (0, _createReducer3.default)({}, _defineProperty({}, types.USER_GUIDE_CONTENT, function (state, action) {
    return action.data;
}));

/***/ })

/******/ });
//# sourceMappingURL=licenseewlbytimezone.chunk.js.map