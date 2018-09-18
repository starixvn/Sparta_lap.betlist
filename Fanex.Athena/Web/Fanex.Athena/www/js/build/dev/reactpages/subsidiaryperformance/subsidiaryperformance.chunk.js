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
/******/ 		"www/js/build/dev/reactpages/subsidiaryperformance/subsidiaryperformance": 0
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
/******/ 	deferredModules.push(["./www/js/source/reactpages/pages/subsidiaryperformance/components/index.jsx","common"]);
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
                'View ' + viewFrom + '-' + (viewTo > totalItem ? totalItem : viewTo) + ' of ' + totalItem
            );
        }
    }, {
        key: 'render',
        value: function render() {
            var _this5 = this;

            var _props = this.props,
                isShowPagingInfo = _props.isShowPagingInfo,
                isAllowInputPageIndex = _props.isAllowInputPageIndex,
                pageOption = _props.pageOption;
            var PageIndex = pageOption.PageIndex,
                PageSize = pageOption.PageSize,
                TotalItem = pageOption.TotalItem,
                PageList = pageOption.PageList;
            var inputValue = this.state.inputValue;


            var isDisable = isNaN(parseInt(inputValue));

            var isShow = TotalItem > PageList[0];
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
    isAllowInputPageIndex: _propTypes2.default.bool
};
Pager.defaultProps = {
    pageOption: {
        PageIndex: 1,
        PageSize: 50,
        PageList: [50, 100, 200],
        TotalItem: 0
    },
    isShowPagingInfo: true,
    isAllowInputPageIndex: true
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
                onPaging = _props2.onPaging;

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
                isPaging && pageOption && _react2.default.createElement(_Pager2.default, { pageOption: pageOption, onPaging: onPaging })
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
    adjustedHeight: _propTypes2.default.number
};
Table.defaultProps = {
    width: _constants.MAX_WIDTH,
    maxWidth: _constants.MAX_WIDTH,
    autoWidth: true,
    isPaging: false,
    adjustedHeight: 0,
    bodyHeight: 0,
    autoHeight: true
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

/***/ "./www/js/source/reactpages/components/multiple-select/components/MultipleSelect.jsx":
/*!*******************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/multiple-select/components/MultipleSelect.jsx ***!
  \*******************************************************************************************/
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

var _MultipleSelectLabel = __webpack_require__(/*! ./MultipleSelectLabel */ "./www/js/source/reactpages/components/multiple-select/components/MultipleSelectLabel.jsx");

var _MultipleSelectLabel2 = _interopRequireDefault(_MultipleSelectLabel);

var _MultipleSelectOptionList = __webpack_require__(/*! ./MultipleSelectOptionList */ "./www/js/source/reactpages/components/multiple-select/components/MultipleSelectOptionList.jsx");

var _MultipleSelectOptionList2 = _interopRequireDefault(_MultipleSelectOptionList);

var _OptionAll = __webpack_require__(/*! ./OptionAll */ "./www/js/source/reactpages/components/multiple-select/components/OptionAll.jsx");

var _OptionAll2 = _interopRequireDefault(_OptionAll);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var KEY_NAME = "key";
var VALUE_NAME = "value";
var STATUS_NAME = "checked";

var id = 1;

var MultipleSelect = function (_Component) {
    _inherits(MultipleSelect, _Component);

    function MultipleSelect(props) {
        _classCallCheck(this, MultipleSelect);

        var _this = _possibleConstructorReturn(this, (MultipleSelect.__proto__ || Object.getPrototypeOf(MultipleSelect)).call(this, props));

        _this.onChangeHandler = function (item) {
            var _selectedItem;

            var _this$props = _this.props,
                keyField = _this$props.keyField,
                valueField = _this$props.valueField,
                statusField = _this$props.statusField;


            var selectedItem = (_selectedItem = {}, _defineProperty(_selectedItem, keyField, item.key), _defineProperty(_selectedItem, valueField, item.value), _defineProperty(_selectedItem, statusField, item.checked), _selectedItem);

            var newDataSource = _this.state.dataSource.slice(0);
            var itemToUpdate = newDataSource.find(function (x) {
                return x.key === item.key;
            });
            itemToUpdate.checked = item.checked;

            _this._callBackToParent(selectedItem, newDataSource);

            _this.setState({ dataSource: newDataSource });
        };

        _this.onToggle = function () {
            _this.setState(function (prevState) {
                return { showOptionList: !prevState.showOptionList };
            });
        };

        _this.checkAllHandler = function (checked) {
            var newDataSource = _this.state.dataSource.map(function (item) {
                return {
                    key: item.key,
                    value: item.value,
                    checked: checked
                };
            });

            _this._callBackToParent(null, newDataSource);

            _this.setState({ dataSource: newDataSource });
        };

        _this._close = function () {
            _this.setState({ showOptionList: false });
        };

        _this._handleDocumentClick = function (event) {
            var clickOutside = _this.wrapper && !_this.wrapper.contains(event.target);

            if (clickOutside && _this.state.showOptionList) {
                _this._close();
            }
        };

        _this.id = 'multiple-select-' + (props.id || id++);

        _this.state = {
            showOptionList: false,
            dataSource: _this._convertDataSourceToState(props)
        };
        return _this;
    }

    _createClass(MultipleSelect, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            document.addEventListener('click', this._handleDocumentClick);
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            document.removeEventListener('click', this._handleDocumentClick);
        }
    }, {
        key: '_callBackToParent',
        value: function _callBackToParent(selectedItem, newDataSource) {
            var selectedItemsKey = this._getSelectedItemKey(newDataSource);
            this.props.onChange && this.props.onChange(selectedItem, selectedItemsKey);
        }
    }, {
        key: '_convertDataSourceToState',
        value: function _convertDataSourceToState(_ref) {
            var keyField = _ref.keyField,
                valueField = _ref.valueField,
                statusField = _ref.statusField,
                dataSource = _ref.dataSource;

            return dataSource.map(function (item) {
                var _ref2;

                return _ref2 = {}, _defineProperty(_ref2, KEY_NAME, item[keyField]), _defineProperty(_ref2, VALUE_NAME, item[valueField]), _defineProperty(_ref2, STATUS_NAME, item[statusField]), _ref2;
            });
        }
    }, {
        key: '_getSelectedItemKey',
        value: function _getSelectedItemKey(dataSource) {
            var selectedItemKey = [];

            for (var i = 0; i < dataSource.length; i++) {
                var item = dataSource[i];

                if (item.checked) {
                    selectedItemKey.push(item.key);
                }
            }

            return selectedItemKey.join(",");
        }
    }, {
        key: '_renderOptionAll',
        value: function _renderOptionAll() {
            if (this.props.hasAllOption) {
                var checkedItemCount = this.state.dataSource.filter(function (x) {
                    return x.checked;
                }).length;
                var checkedAll = checkedItemCount === this.state.dataSource.length;

                return _react2.default.createElement(_OptionAll2.default, { id: this.id, checked: checkedAll, label: this.props.optionAllLabel, onChange: this.checkAllHandler });
            }

            return null;
        }
    }, {
        key: 'render',
        value: function render() {
            var _this2 = this;

            var _props = this.props,
                noneSelectedLabel = _props.noneSelectedLabel,
                maxDisplayItemCount = _props.maxDisplayItemCount,
                hasAllOption = _props.hasAllOption;


            return _react2.default.createElement(
                'div',
                { className: 'multiple-select-container', id: this.id, ref: function ref(element) {
                        return _this2.wrapper = element;
                    } },
                _react2.default.createElement(_MultipleSelectLabel2.default, {
                    selectedItems: this.selectedItems,
                    onToggle: this.onToggle,
                    noneSelectedLabel: noneSelectedLabel,
                    maxDisplayItemCount: maxDisplayItemCount }),
                _react2.default.createElement(
                    'div',
                    { className: 'multiple-select-default multiple-select-options-container',
                        style: { display: this.state.showOptionList ? "block" : "none" } },
                    this._renderOptionAll(),
                    _react2.default.createElement(_MultipleSelectOptionList2.default, { id: this.id,
                        dataSource: this.state.dataSource,
                        onChange: this.onChangeHandler })
                )
            );
        }
    }, {
        key: 'selectedItems',
        get: function get() {
            return this.state.dataSource.filter(function (item) {
                return item.checked;
            });
        }
    }]);

    return MultipleSelect;
}(_react.Component);

MultipleSelect.propTypes = {
    dataSource: _propTypes2.default.arrayOf(_propTypes2.default.object).isRequired,
    keyField: _propTypes2.default.string,
    valueField: _propTypes2.default.string,
    statusField: _propTypes2.default.string,
    noneSelectedLabel: _propTypes2.default.string,
    optionAllLabel: _propTypes2.default.string,
    maxDisplayItemCount: _propTypes2.default.number,
    onChange: _propTypes2.default.func,
    hasAllOption: _propTypes2.default.bool
};
MultipleSelect.defaultProps = {
    keyField: KEY_NAME,
    valueField: VALUE_NAME,
    statusField: STATUS_NAME,
    hasAllOption: true
};
exports.default = MultipleSelect;

/***/ }),

/***/ "./www/js/source/reactpages/components/multiple-select/components/MultipleSelectLabel.jsx":
/*!************************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/multiple-select/components/MultipleSelectLabel.jsx ***!
  \************************************************************************************************/
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

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var NO_SELECTED_LABEL = "Select options";
var MAX_SELECTED_ITEM_FOR_DISPLAY = 3;

var MultipleSelectLabel = function (_Component) {
    _inherits(MultipleSelectLabel, _Component);

    function MultipleSelectLabel(props) {
        _classCallCheck(this, MultipleSelectLabel);

        var _this = _possibleConstructorReturn(this, (MultipleSelectLabel.__proto__ || Object.getPrototypeOf(MultipleSelectLabel)).call(this, props));

        _this.state = {
            noneSelectedLabel: props.noneSelectedLabel
        };
        return _this;
    }

    _createClass(MultipleSelectLabel, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'button',
                { type: 'button', className: 'multiple-select-default multiple-select-label',
                    onClick: this.props.onToggle },
                _react2.default.createElement(
                    'span',
                    { className: 'text-display', title: this.selectedItemsString },
                    this.selectedItemsString
                ),
                _react2.default.createElement('b', { className: 'caret' })
            );
        }
    }, {
        key: 'selectedItemsString',
        get: function get() {
            var _props = this.props,
                selectedItems = _props.selectedItems,
                noneSelectedLabel = _props.noneSelectedLabel,
                maxDisplayItemCount = _props.maxDisplayItemCount;

            var selectedValues = selectedItems.map(function (item) {
                return item.value;
            });

            var displayText = noneSelectedLabel;
            var selectedItemCount = selectedValues.length;

            if (selectedItemCount >= maxDisplayItemCount) {
                displayText = selectedItemCount + ' selected';
            } else if (selectedItemCount >= 1) {
                displayText = selectedValues.join(", ");
            }

            return displayText;
        }
    }]);

    return MultipleSelectLabel;
}(_react.Component);

MultipleSelectLabel.propTypes = {
    selectedItems: _propTypes2.default.arrayOf(_propTypes2.default.object).isRequired,
    noneSelectedLabel: _propTypes2.default.string,
    maxDisplayItemCount: _propTypes2.default.number
};
MultipleSelectLabel.defaultProps = {
    noneSelectedLabel: NO_SELECTED_LABEL,
    maxDisplayItemCount: MAX_SELECTED_ITEM_FOR_DISPLAY
};
exports.default = MultipleSelectLabel;

/***/ }),

/***/ "./www/js/source/reactpages/components/multiple-select/components/MultipleSelectOptionList.jsx":
/*!*****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/multiple-select/components/MultipleSelectOptionList.jsx ***!
  \*****************************************************************************************************/
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

var _Option = __webpack_require__(/*! ./Option */ "./www/js/source/reactpages/components/multiple-select/components/Option.jsx");

var _Option2 = _interopRequireDefault(_Option);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var MultipleSelectOptionList = function (_Component) {
    _inherits(MultipleSelectOptionList, _Component);

    function MultipleSelectOptionList(props) {
        _classCallCheck(this, MultipleSelectOptionList);

        return _possibleConstructorReturn(this, (MultipleSelectOptionList.__proto__ || Object.getPrototypeOf(MultipleSelectOptionList)).call(this, props));
    }

    _createClass(MultipleSelectOptionList, [{
        key: '_renderOptionList',
        value: function _renderOptionList() {
            var _props = this.props,
                dataSource = _props.dataSource,
                onChange = _props.onChange,
                id = _props.id;

            var optionList = dataSource.map(function (item, index) {
                return _react2.default.createElement(_Option2.default, {
                    id: id + '-optionItem-' + index,
                    key: id + 'optionItem' + index,
                    itemData: item,
                    onChange: onChange });
            });

            return optionList;
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'ul',
                { className: 'multiple-select-options' },
                this._renderOptionList()
            );
        }
    }]);

    return MultipleSelectOptionList;
}(_react.Component);

MultipleSelectOptionList.propTypes = {
    dataSource: _propTypes2.default.arrayOf(_propTypes2.default.object).isRequired,
    id: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]).isRequired,
    onChange: _propTypes2.default.func
};
exports.default = MultipleSelectOptionList;

/***/ }),

/***/ "./www/js/source/reactpages/components/multiple-select/components/Option.jsx":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/components/multiple-select/components/Option.jsx ***!
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

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Option = function (_Component) {
    _inherits(Option, _Component);

    function Option(props) {
        _classCallCheck(this, Option);

        var _this = _possibleConstructorReturn(this, (Option.__proto__ || Object.getPrototypeOf(Option)).call(this, props));

        _this.onChange = function (event) {
            event.stopPropagation();

            var _this$props = _this.props,
                itemData = _this$props.itemData,
                onChange = _this$props.onChange;


            var newItemData = {
                key: itemData.key,
                value: itemData.value,
                checked: !itemData.checked
            };
            onChange && onChange(newItemData);
        };

        return _this;
    }

    _createClass(Option, [{
        key: 'render',
        value: function render() {
            var _props = this.props,
                itemData = _props.itemData,
                id = _props.id;


            return _react2.default.createElement(
                'li',
                { className: 'multiple-select-item' },
                _react2.default.createElement('input', {
                    id: id,
                    type: 'checkbox',
                    className: 'option-checkbox',
                    checked: itemData.checked,
                    onChange: this.onChange
                }),
                _react2.default.createElement(
                    'label',
                    { className: 'option-label', htmlFor: id },
                    itemData.value
                )
            );
        }
    }]);

    return Option;
}(_react.Component);

Option.propTypes = {
    itemData: _propTypes2.default.object.isRequired,
    id: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]).isRequired,
    onChange: _propTypes2.default.func
};
exports.default = Option;

/***/ }),

/***/ "./www/js/source/reactpages/components/multiple-select/components/OptionAll.jsx":
/*!**************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/multiple-select/components/OptionAll.jsx ***!
  \**************************************************************************************/
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

var _Option = __webpack_require__(/*! ./Option */ "./www/js/source/reactpages/components/multiple-select/components/Option.jsx");

var _Option2 = _interopRequireDefault(_Option);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var LABEL = "All";
var DEFAULT_CHECKED = false;

var OptionAll = function (_Component) {
    _inherits(OptionAll, _Component);

    function OptionAll(props) {
        _classCallCheck(this, OptionAll);

        var _this = _possibleConstructorReturn(this, (OptionAll.__proto__ || Object.getPrototypeOf(OptionAll)).call(this, props));

        _this.changeHandler = function (_ref) {
            var checked = _ref.checked;

            _this.props.onChange && _this.props.onChange(checked);
        };

        _this.itemData = {
            key: "All",
            value: props.label
        };
        return _this;
    }

    _createClass(OptionAll, [{
        key: 'render',
        value: function render() {
            var _props = this.props,
                id = _props.id,
                checked = _props.checked;

            this.itemData.checked = checked;

            return _react2.default.createElement(
                'ul',
                { className: 'multiple-select-options multiple-select-option-all' },
                _react2.default.createElement(_Option2.default, {
                    id: id + '-optionItemAll',
                    itemData: this.itemData,
                    onChange: this.changeHandler })
            );
        }
    }]);

    return OptionAll;
}(_react.Component);

OptionAll.propTypes = {
    onChange: _propTypes2.default.func,
    id: _propTypes2.default.oneOfType([_propTypes2.default.number, _propTypes2.default.string]).isRequired,
    label: _propTypes2.default.string,
    checked: _propTypes2.default.bool
};
OptionAll.defaultProps = {
    label: LABEL,
    checked: DEFAULT_CHECKED
};
exports.default = OptionAll;

/***/ }),

/***/ "./www/js/source/reactpages/components/multiple-select/index.jsx":
/*!***********************************************************************!*\
  !*** ./www/js/source/reactpages/components/multiple-select/index.jsx ***!
  \***********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _MultipleSelect = __webpack_require__(/*! ./components/MultipleSelect */ "./www/js/source/reactpages/components/multiple-select/components/MultipleSelect.jsx");

var _MultipleSelect2 = _interopRequireDefault(_MultipleSelect);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _MultipleSelect2.default;

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

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/dataHandlerAction.js":
/*!*******************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/actions/dataHandlerAction.js ***!
  \*******************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.getGridData = getGridData;
exports.exportExcel = exportExcel;
exports.setGridData = setGridData;

var _types = __webpack_require__(/*! ./types.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/types.js");

var types = _interopRequireWildcard(_types);

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

var _site = __webpack_require__(/*! ../../../lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var fetchData = function fetchData(params) {
    var url = _site2.default.resolveClientUrl('api/SubsidiaryPerformanceReport/Index');
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

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/filterHandlerAction.js":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/actions/filterHandlerAction.js ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.changeConvertAll = exports.setDateRange = exports.setSelectedSubsidiary = undefined;

var _types = __webpack_require__(/*! ./types.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedSubsidiary = exports.setSelectedSubsidiary = function setSelectedSubsidiary(subsidiaryKeys) {
    return {
        type: types.SET_SUBSIDIARY,
        data: subsidiaryKeys
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

var changeConvertAll = exports.changeConvertAll = function changeConvertAll(checked) {
    return {
        type: types.CHANGE_CONVERT_ALL,
        data: checked
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/index.js":
/*!*******************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/actions/index.js ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _dataHandlerAction = __webpack_require__(/*! ./dataHandlerAction.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/dataHandlerAction.js");

var dataHandlerAction = _interopRequireWildcard(_dataHandlerAction);

var _filterHandlerAction = __webpack_require__(/*! ./filterHandlerAction.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/filterHandlerAction.js");

var filterHandlerAction = _interopRequireWildcard(_filterHandlerAction);

var _productAction = __webpack_require__(/*! ../../../actions/productAction */ "./www/js/source/reactpages/actions/productAction.js");

var productAction = _interopRequireWildcard(_productAction);

var _baseCurrencyAction = __webpack_require__(/*! ../../../actions/baseCurrencyAction */ "./www/js/source/reactpages/actions/baseCurrencyAction.js");

var baseCurrencyAction = _interopRequireWildcard(_baseCurrencyAction);

var _loaderAction = __webpack_require__(/*! ../../../actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");

var loaderAction = _interopRequireWildcard(_loaderAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, dataHandlerAction, filterHandlerAction, productAction, baseCurrencyAction, loaderAction);

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/types.js":
/*!*******************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/actions/types.js ***!
  \*******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var FETCH_DATA = exports.FETCH_DATA = 'FETCH_DATA';
var SET_GRID_DATA = exports.SET_GRID_DATA = "SET_GRID_DATA";

var GET_SUBSIDIARY = exports.GET_SUBSIDIARY = "GET_SUBSIDIARY";
var SET_SUBSIDIARY = exports.SET_SUBSIDIARY = "SET_SUBSIDIARY";

var SET_DATE_RANGE = exports.SET_DATE_RANGE = "SET_DATE_RANGE";
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = "GET_DATE_CONSTRAINT";
var CHANGE_CONVERT_ALL = exports.CHANGE_CONVERT_ALL = "CHANGE_CONVERT_ALL";

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/components/AppContainer.jsx":
/*!******************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/components/AppContainer.jsx ***!
  \******************************************************************************************/
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

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _index = __webpack_require__(/*! ../actions/index */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/index.js");

var _DataGrid = __webpack_require__(/*! ./DataGrid */ "./www/js/source/reactpages/pages/subsidiaryperformance/components/DataGrid.jsx");

var _DataGrid2 = _interopRequireDefault(_DataGrid);

var _FormFilter = __webpack_require__(/*! ./FormFilter */ "./www/js/source/reactpages/pages/subsidiaryperformance/components/FormFilter.jsx");

var _FormFilter2 = _interopRequireDefault(_FormFilter);

var _Loader = __webpack_require__(/*! ../../../components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");

var _Loader2 = _interopRequireDefault(_Loader);

var _AppLayout = __webpack_require__(/*! ../../../layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");

var _AppLayout2 = _interopRequireDefault(_AppLayout);

var _ReportTitle = __webpack_require__(/*! ../../../components/ReportTitle */ "./www/js/source/reactpages/components/ReportTitle.jsx");

var _ReportTitle2 = _interopRequireDefault(_ReportTitle);

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/service.js");

var _service2 = _interopRequireDefault(_service);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var AppContainer = exports.AppContainer = function (_Component) {
  _inherits(AppContainer, _Component);

  function AppContainer(props) {
    _classCallCheck(this, AppContainer);

    var _this = _possibleConstructorReturn(this, (AppContainer.__proto__ || Object.getPrototypeOf(AppContainer)).call(this, props));

    _this.reportTitle = null;
    return _this;
  }

  _createClass(AppContainer, [{
    key: 'componentDidUpdate',
    value: function componentDidUpdate(prevProps, prevState) {
      if (this.props.isLoading && !prevProps.isLoading) {
        var _props = this.props,
            dateRange = _props.dateRange,
            selectedBaseCurrency = _props.selectedBaseCurrency;

        this.reportTitle = _service2.default.getReportTitle(dateRange, selectedBaseCurrency);
      }
    }
  }, {
    key: 'render',
    value: function render() {
      return _react2.default.createElement(
        _AppLayout2.default,
        null,
        _react2.default.createElement(_Loader2.default, { isLoading: this.props.isLoading }),
        _react2.default.createElement(_FormFilter2.default, null),
        _react2.default.createElement(
          'div',
          { className: 'result-content' },
          this.reportTitle && _react2.default.createElement(_ReportTitle2.default, { content: this.reportTitle }),
          _react2.default.createElement(_DataGrid2.default, null)
        )
      );
    }
  }]);

  return AppContainer;
}(_react.Component);

function mapStateToProps(state) {
  var isLoading = state.isLoading,
      dateRange = state.dateRange,
      selectedBaseCurrency = state.selectedBaseCurrency;

  return { isLoading: isLoading, dateRange: dateRange, selectedBaseCurrency: selectedBaseCurrency };
}

exports.default = (0, _reactRedux.connect)(mapStateToProps)(AppContainer);

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/components/DataGrid.jsx":
/*!**************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/components/DataGrid.jsx ***!
  \**************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.DataGrid = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _fixedTableHeader = __webpack_require__(/*! ../../../components/fixed-table-header */ "./www/js/source/reactpages/components/fixed-table-header/index.jsx");

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _numberFormat = __webpack_require__(/*! ../../../lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");

var _numberFormat2 = _interopRequireDefault(_numberFormat);

var _NoDataTitle = __webpack_require__(/*! ../../../components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");

var _NoDataTitle2 = _interopRequireDefault(_NoDataTitle);

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/service.js");

var _service2 = _interopRequireDefault(_service);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DataGrid = exports.DataGrid = function (_Component) {
    _inherits(DataGrid, _Component);

    function DataGrid(props) {
        _classCallCheck(this, DataGrid);

        return _possibleConstructorReturn(this, (DataGrid.__proto__ || Object.getPrototypeOf(DataGrid)).call(this, props));
    }

    _createClass(DataGrid, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            if (this.props.gridData !== prevProps.gridData) {
                this.props.toggleLoader(false);
            }
        }
    }, {
        key: '_buildHeader',
        value: function _buildHeader() {
            var headerText = _service2.default.getHeaderText();

            return _react2.default.createElement(
                _fixedTableHeader.Row,
                { style: { height: 32 } },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.subsidiary },
                    headerText.subsidiary
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.currency },
                    headerText.currency
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.product },
                    headerText.product
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.betCount },
                    headerText.betCount
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.turnover },
                    headerText.turnover
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.customerWL },
                    headerText.customerWL
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, title: headerText.subsidiaryWL },
                    headerText.subsidiaryWL
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.subsidiaryComm },
                    headerText.subsidiaryComm
                )
            );
        }
    }, {
        key: '_buildBody',
        value: function _buildBody() {
            var data = this.props.gridData.Data;
            var rows = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var rowSpan = item.Details.length;

                var isFirstRow = true;
                for (var j = 0; j < item.Details.length; j++) {
                    var detail = item.Details[j];
                    var betcount = _numberFormat2.default.format(detail.BetCount),
                        turnover = _numberFormat2.default.formatDecimal(detail.Turnover),
                        customerWinLoss = _numberFormat2.default.formatDecimal(detail.CustomerWinLoss),
                        subsidiaryWinLoss = _numberFormat2.default.formatDecimal(detail.SubsidiaryWinLoss),
                        subsidiaryComm = _numberFormat2.default.formatDecimal(detail.SubsidiaryComm);

                    rows.push(_react2.default.createElement(
                        _fixedTableHeader.Row,
                        { key: 'tr' + i + '-detail' + j },
                        isFirstRow ? _react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { title: item.Subsidiary, rowSpan: rowSpan },
                            item.Subsidiary
                        ) : undefined,
                        isFirstRow ? _react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { title: item.CurrencyName, rowSpan: rowSpan, className: 'center-align' },
                            item.CurrencyName
                        ) : undefined,
                        _react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { title: detail.Product },
                            detail.Product
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
                            { title: customerWinLoss, className: 'right-align' },
                            customerWinLoss
                        ),
                        _react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { title: subsidiaryWinLoss, className: 'right-align ' + (detail.SubsidiaryWinLoss < 0 ? "negative" : "") },
                            subsidiaryWinLoss
                        ),
                        _react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { title: subsidiaryComm, className: 'right-align' },
                            subsidiaryComm
                        )
                    ));

                    isFirstRow = false;
                }
            }

            return rows;
        }
    }, {
        key: '_buildFooter',
        value: function _buildFooter() {
            var totalData = this.props.gridData.Total;
            var footer = totalData.map(function (item, index) {
                var betcount = _numberFormat2.default.format(item.BetCount),
                    turnover = _numberFormat2.default.formatDecimal(item.Turnover),
                    customerWinLoss = _numberFormat2.default.formatDecimal(item.CustomerWinLoss),
                    subsidiaryWinLoss = _numberFormat2.default.formatDecimal(item.SubsidiaryWinLoss),
                    subsidiaryComm = _numberFormat2.default.formatDecimal(item.SubsidiaryComm);

                return _react2.default.createElement(
                    _fixedTableHeader.Row,
                    { className: 'fixed-table-total', key: 'total' + index },
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: item.Subsidiary },
                        item.Subsidiary
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: item.CurrencyName, className: 'center-align', colSpan: '2' },
                        item.CurrencyName
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
                        { title: customerWinLoss, className: 'right-align' },
                        customerWinLoss
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: subsidiaryWinLoss, className: 'right-align ' + (item.SubsidiaryWinLoss < 0 ? "negative" : "") },
                        subsidiaryWinLoss
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: subsidiaryComm, className: 'right-align' },
                        subsidiaryComm
                    )
                );
            });

            footer.push(this._buildGrandTotal());

            return footer;
        }
    }, {
        key: '_buildGrandTotal',
        value: function _buildGrandTotal() {
            var grandTotal = this.props.gridData.GrandTotal;
            if (!grandTotal) {
                return null;
            }

            var betcount = _numberFormat2.default.format(grandTotal.BetCount),
                turnover = _numberFormat2.default.formatDecimal(grandTotal.Turnover),
                customerWinLoss = _numberFormat2.default.formatDecimal(grandTotal.CustomerWinLoss),
                subsidiaryWinLoss = _numberFormat2.default.formatDecimal(grandTotal.SubsidiaryWinLoss),
                subsidiaryComm = _numberFormat2.default.formatDecimal(grandTotal.SubsidiaryComm);

            return _react2.default.createElement(
                _fixedTableHeader.Row,
                { key: 'grandTotal', className: 'fixed-table-grant-total' },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: grandTotal.Subsidiary },
                    grandTotal.Subsidiary
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: grandTotal.CurrencyName, className: 'center-align', colSpan: '2' },
                    grandTotal.CurrencyName
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
                    { title: customerWinLoss, className: 'right-align' },
                    customerWinLoss
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: subsidiaryWinLoss, className: 'right-align ' + (grandTotal.SubsidiaryWinLoss < 0 ? "negative" : "") },
                    subsidiaryWinLoss
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { title: subsidiaryComm, className: 'right-align' },
                    subsidiaryComm
                )
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
                return _react2.default.createElement(_NoDataTitle2.default, null);
            }

            return _react2.default.createElement(_fixedTableHeader.Table, {
                minWidth: 600,
                autoWidth: true,
                bodyHeight: 300,
                header: this._buildHeader(),
                body: this._buildBody(),
                footer: this._buildFooter()
            });
        }
    }]);

    return DataGrid;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    var toggleLoader = _index.ActionCreators.toggleLoader;

    return (0, _redux.bindActionCreators)({ toggleLoader: toggleLoader }, dispatch);
}
function mapStateToProps(state) {
    var gridData = state.gridData;

    return { gridData: gridData };
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(DataGrid);

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/components/FormFilter.jsx":
/*!****************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/components/FormFilter.jsx ***!
  \****************************************************************************************/
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

var _index = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _reactSelect = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");

var _reactSelect2 = _interopRequireDefault(_reactSelect);

var _multipleSelect = __webpack_require__(/*! @jsroot/components/multiple-select */ "./www/js/source/reactpages/components/multiple-select/index.jsx");

var _multipleSelect2 = _interopRequireDefault(_multipleSelect);

var _index2 = __webpack_require__(/*! @jsroot/components/date-range/index */ "./www/js/source/reactpages/components/date-range/index.js");

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _reportExcel = __webpack_require__(/*! ../lib/reportExcel */ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/reportExcel.js");

var _reportExcel2 = _interopRequireDefault(_reportExcel);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _AppData = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");

var _AppData2 = _interopRequireDefault(_AppData);

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/service.js");

var _service2 = _interopRequireDefault(_service);

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
                dateRange = _this$props.dateRange,
                selectedBaseCurrency = _this$props.selectedBaseCurrency;

            var params = _this._getParams();
            if (!params) {
                return;
            }

            _this.props.exportExcel(params).then(function (response) {
                if (!response.data.Data.length) {
                    alert(_langHelper2.default.getSingleResource("NoInfo"));
                    return;
                }

                var title = _service2.default.getReportTitle(dateRange, selectedBaseCurrency);
                var excel = new _reportExcel2.default(response.data, title);
                excel.export(function () {
                    _this.props.toggleLoader(false);
                });
            });
        };

        _this.productChangeHandler = function (currentItem, selectedItemsKey) {
            _this.props.setSelectedProduct(selectedItemsKey);
        };

        _this.subsidiaryChangeHandler = function (currentItem, selectedItemsKey) {
            _this.props.setSelectedSubsidiary(selectedItemsKey);
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
                    SubsidiaryList: this.props.selectedSubsidiary,
                    ProductCodeList: this.props.selectedProduct,
                    ConvertAll: this.props.convertAll,
                    FromDate: _site2.default.convertDateToString(fromDate),
                    ToDate: _site2.default.convertDateToString(toDate)
                }
            };
        }
    }, {
        key: '_renderSubsidiarySelectList',
        value: function _renderSubsidiarySelectList() {
            var subsidiaryList = this.props.subsidiaryList;


            return subsidiaryList && subsidiaryList.length ? _react2.default.createElement(_multipleSelect2.default, {
                dataSource: this.props.subsidiaryList,
                keyField: "Name",
                valueField: "Name",
                statusField: "Checked",
                optionAllLabel: _langHelper2.default.getSingleResource("All"),
                noneSelectedLabel: _langHelper2.default.getSingleResource("SelectOptions"),
                onChange: this.subsidiaryChangeHandler }) : null;
        }
    }, {
        key: '_renderProductSelectList',
        value: function _renderProductSelectList() {
            var productOptions = _AppData2.default.productOptions;
            return productOptions && productOptions.length && _react2.default.createElement(_multipleSelect2.default, {
                dataSource: productOptions,
                keyField: "Code",
                valueField: "Name",
                statusField: "Status",
                optionAllLabel: _langHelper2.default.getSingleResource("All"),
                noneSelectedLabel: _langHelper2.default.getSingleResource("SelectOptions"),
                onChange: this.productChangeHandler });
        }
    }, {
        key: '_renderBaseCurrencySelectList',
        value: function _renderBaseCurrencySelectList() {
            var _this2 = this;

            var selectedBaseCurrency = this.props.selectedBaseCurrency;

            var baseCurrencyList = _AppData2.default.baseCurrencyList;
            return baseCurrencyList && baseCurrencyList.length && _react2.default.createElement(_reactSelect2.default, { id: 'ddlBased',
                name: 'form-field-name',
                value: selectedBaseCurrency,
                options: baseCurrencyList,
                onChange: function onChange(item) {
                    return _this2.props.setSelectedBaseCurrency(item.value);
                } });
        }
    }, {
        key: '_renderDateRange',
        value: function _renderDateRange() {
            var _props = this.props,
                dateRange = _props.dateRange,
                dateConstraint = _props.dateConstraint;


            return _react2.default.createElement(_index2.DateRange, { startDate: dateRange.fromDate, endDate: dateRange.toDate,
                minDate: dateConstraint.minDate, maxDate: dateConstraint.maxDate,
                today: dateRange.toDate, limitedMonthRange: RANGE_MONTH_CONSTRAINT,
                onChange: this.applyDateRangeHandler });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this3 = this;

            return _react2.default.createElement(
                'div',
                { className: 'page-filter' },
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-3 col-sm-5 col-lg-2 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("Subsidiary")
                            ),
                            this._renderSubsidiarySelectList()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-4 col-sm-6 col-lg-3 col-xl-3' },
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
                        { className: 'col-12 col-md-3 col-sm-5 col-lg-3 col-xl-3' },
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
                        { className: 'col-12 col-md-2 col-sm-4 col-lg-2 col-xl-2' },
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
                        { className: 'col-12 col-md-12 col-sm-12 col-lg-6 col-xl-6' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'div',
                                { className: 'checkbox-group' },
                                _react2.default.createElement('input', { type: 'checkbox', className: 'checkbox', checked: this.props.convertAll, id: 'IsConvertAll',
                                    onClick: function onClick() {
                                        return _this3.props.changeConvertAll(!_this3.props.convertAll);
                                    } }),
                                _react2.default.createElement(
                                    'label',
                                    { htmlFor: 'IsConvertAll' },
                                    _langHelper2.default.getSingleResource("ConvertAllFiguresToBaseCurrency")
                                )
                            ),
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
                    )
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'caret-block' },
                    _react2.default.createElement('i', { className: 'icon-excel right', onClick: this.exportExcel, title: _langHelper2.default.getSingleResource("ExportToExcel") })
                )
            );
        }
    }, {
        key: 'isValidModel',
        get: function get() {
            var _props2 = this.props,
                selectedSubsidiary = _props2.selectedSubsidiary,
                selectedProduct = _props2.selectedProduct;

            return selectedSubsidiary !== "" && selectedProduct !== "";
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

    return rest;
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(FormFilter);

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/components/index.jsx":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/components/index.jsx ***!
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

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _AppContainer = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/pages/subsidiaryperformance/components/AppContainer.jsx");

var _AppContainer2 = _interopRequireDefault(_AppContainer);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../reducers/index.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/reducers/index.js");

var _index2 = _interopRequireDefault(_index);

var _store = __webpack_require__(/*! ../../../lib/store */ "./www/js/source/reactpages/lib/store.js");

var _store2 = _interopRequireDefault(_store);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var defaultState = Object.assign({}, window.initialState);
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

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/reportExcel.js":
/*!*********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/lib/reportExcel.js ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _ref, _ref2, _ref4, _ref5, _ref6, _ref7, _ref8;

var _excel = __webpack_require__(/*! ../../../lib/excel */ "./www/js/source/reactpages/lib/excel.js");

var _excel2 = _interopRequireDefault(_excel);

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _excelStyle = __webpack_require__(/*! ../../../constants/excelStyle */ "./www/js/source/reactpages/constants/excelStyle.js");

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/service.js");

var _service2 = _interopRequireDefault(_service);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var COLUMN_COUNT = 8;
var TITLE_ROW_INDEX = 0;
var HEADER_ROW_INDEX = 1;
var DATA_ROW_INDEX = 2;

var SUBSIDIARY_COLUMN_INDEX = 0;
var CURRENCY_COLUMN_INDEX = 1;
var PRODUCT_COLUMN_INDEX = 2;
var BETCOUNT_COLUMN_INDEX = 3;
var TURNOVER_COLUMN_INDEX = 4;
var CUSTOMERWL_COLUMN_INDEX = 5;
var SUBSIDIARYWL_COLUMN_INDEX = 6;
var SUBSIDIARYCOMM_COLUMN_INDEX = 7;

var MAIN_SECTION_STYLE = "mainSectionStyle";
var TOTAL_SECTION_STYLE = "totalSectionStyle";
var GRAND_TOTAL_SECTION_STYLE = "grandTotalSectionStyle";

var fields = {
    subsidiary: "Subsidiary",
    currencyName: "CurrencyName",
    product: "Product",
    betCount: "BetCount",
    turnover: "Turnover",
    customerWinLoss: "CustomerWinLoss",
    subsidiaryWinLoss: "SubsidiaryWinLoss",
    subsidiaryComm: "SubsidiaryComm"
};

var columnConfigs = [(_ref = {
    field: fields.subsidiary
}, _defineProperty(_ref, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.rowspan), _defineProperty(_ref, TOTAL_SECTION_STYLE, _excelStyle.totalCellStyle.default), _defineProperty(_ref, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.default), _ref), (_ref2 = {
    field: fields.currencyName
}, _defineProperty(_ref2, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.center), _defineProperty(_ref2, TOTAL_SECTION_STYLE, _excelStyle.totalCellStyle.center), _defineProperty(_ref2, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.center), _ref2), _defineProperty({
    field: fields.product
}, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.default), (_ref4 = {
    field: fields.betCount
}, _defineProperty(_ref4, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.number), _defineProperty(_ref4, TOTAL_SECTION_STYLE, _excelStyle.totalCellStyle.number), _defineProperty(_ref4, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.number), _ref4), (_ref5 = {
    field: fields.turnover
}, _defineProperty(_ref5, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.decimal), _defineProperty(_ref5, TOTAL_SECTION_STYLE, _excelStyle.totalCellStyle.decimal), _defineProperty(_ref5, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.decimal), _ref5), (_ref6 = {
    field: fields.customerWinLoss
}, _defineProperty(_ref6, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.decimal), _defineProperty(_ref6, TOTAL_SECTION_STYLE, _excelStyle.totalCellStyle.decimal), _defineProperty(_ref6, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.decimal), _ref6), (_ref7 = {
    field: fields.subsidiaryWinLoss
}, _defineProperty(_ref7, MAIN_SECTION_STYLE, { positive: _excelStyle.bodyCellStyle.decimal, negative: _excelStyle.bodyCellStyle.negativeDecimal }), _defineProperty(_ref7, TOTAL_SECTION_STYLE, { positive: _excelStyle.totalCellStyle.decimal, negative: _excelStyle.totalCellStyle.negativeDecimal }), _defineProperty(_ref7, GRAND_TOTAL_SECTION_STYLE, { positive: _excelStyle.grandTotalCellStyle.decimal, negative: _excelStyle.grandTotalCellStyle.negativeDecimal }), _ref7), (_ref8 = {
    field: fields.subsidiaryComm
}, _defineProperty(_ref8, MAIN_SECTION_STYLE, _excelStyle.bodyCellStyle.decimal), _defineProperty(_ref8, TOTAL_SECTION_STYLE, _excelStyle.totalCellStyle.decimal), _defineProperty(_ref8, GRAND_TOTAL_SECTION_STYLE, _excelStyle.grandTotalCellStyle.decimal), _ref8)];

var ReportExcel = function () {
    function ReportExcel() {
        var gridData = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
        var title = arguments[1];
        var fileName = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "SubsidiaryPerformance";

        _classCallCheck(this, ReportExcel);

        this.worksheet = {};
        this.title = title;
        this.fileName = fileName;
        this.gridData = gridData;
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
            var headerText = _service2.default.getHeaderText();
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
            this._createGrandTotalData();
        }
    }, {
        key: '_createMainData',
        value: function _createMainData() {
            var mainData = this.gridData.Data;

            for (var i = 0; i < mainData.length; i++) {
                var item = mainData[i];
                var details = item.Details;

                this._createRowByColumnIndex(item, MAIN_SECTION_STYLE, SUBSIDIARY_COLUMN_INDEX, CURRENCY_COLUMN_INDEX);

                this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 0 }, e: { r: this.currentRowIndex + details.length - 1, c: 0 } });
                this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 1 }, e: { r: this.currentRowIndex + details.length - 1, c: 1 } });

                for (var detailIndex = 0; detailIndex < details.length; detailIndex++) {
                    var detail = details[detailIndex];
                    this._createRowByColumnIndex(detail, MAIN_SECTION_STYLE, PRODUCT_COLUMN_INDEX, SUBSIDIARYCOMM_COLUMN_INDEX);
                    this.currentRowIndex++;
                }
            }
        }
    }, {
        key: '_createFooterData',
        value: function _createFooterData() {
            var totalDataList = this.gridData.Total;

            for (var i = 0; i < totalDataList.length; i++) {
                this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 1 }, e: { r: this.currentRowIndex, c: 2 } });
                var item = totalDataList[i];
                this._createRowByColumnIndex(item, TOTAL_SECTION_STYLE, SUBSIDIARY_COLUMN_INDEX, SUBSIDIARYCOMM_COLUMN_INDEX);
                this.currentRowIndex++;
            }
        }
    }, {
        key: '_createGrandTotalData',
        value: function _createGrandTotalData() {
            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 1 }, e: { r: this.currentRowIndex, c: 2 } });
            var grandTotal = this.gridData.GrandTotal;
            this._createRowByColumnIndex(grandTotal, GRAND_TOTAL_SECTION_STYLE, SUBSIDIARY_COLUMN_INDEX, SUBSIDIARYCOMM_COLUMN_INDEX);
        }
    }, {
        key: '_createExcelConfig',
        value: function _createExcelConfig() {
            var cellStart = _excel2.default.getCellAddress({ r: 0, c: 0 });
            var cellEnd = _excel2.default.getCellAddress({ r: this.currentRowIndex, c: COLUMN_COUNT - 1 });

            this.worksheet["!merges"] = this.mergeConfig;
            this.worksheet["!ref"] = cellStart + ':' + cellEnd;
            var wscols = [{ wpx: 150 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }, { wpx: 120 }];

            this.worksheet['!cols'] = wscols;
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
            var field = columnConfig.field;

            var cellAddress = _excel2.default.getCellAddress({ c: columnIndex, r: this.currentRowIndex });
            var type = typeof item[columnConfig.field] === "number" ? "n" : "s";
            var value = item[field] !== null ? item[field] : "";

            var style = columnConfig[sectionStyle];
            if (field === fields.subsidiaryWinLoss) {
                style = value < 0 ? style.negative : style.positive;
            }

            return { cellAddress: cellAddress, type: type, value: value, style: style };
        }
    }]);

    return ReportExcel;
}();

exports.default = ReportExcel;

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/lib/service.js":
/*!*****************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/lib/service.js ***!
  \*****************************************************************************/
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
    subsidiary: _langHelper2.default.getSingleResource("Subsidiary"),
    currency: _langHelper2.default.getSingleResource("Currency"),
    product: _langHelper2.default.getSingleResource("Product"),
    betCount: _langHelper2.default.getSingleResource("BetCount"),
    turnover: _langHelper2.default.getSingleResource("Turnover"),
    customerWL: _langHelper2.default.getSingleResource("CustomerW_L"),
    subsidiaryWL: '' + _langHelper2.default.getSingleResource("SubsidiaryWL"),
    subsidiaryComm: _langHelper2.default.getSingleResource("Subsidiary") + ' ' + _langHelper2.default.getSingleResource("Comm")
};

var Service = function () {
    function Service() {
        _classCallCheck(this, Service);
    }

    _createClass(Service, null, [{
        key: 'getReportTitle',
        value: function getReportTitle(dateRange, selectedBaseCurrency) {
            var fromDate = _site2.default.convertDateToString(dateRange.fromDate);
            var toDate = _site2.default.convertDateToString(dateRange.toDate);
            var baseCurrency = _langHelper2.default.getSingleResource("BaseCurrency") + ': ' + selectedBaseCurrency;

            return _langHelper2.default.getSingleResource("SubsidiaryPerformance") + ', ' + fromDate + ' - ' + toDate + ', ' + baseCurrency;
        }
    }, {
        key: 'getHeaderText',
        value: function getHeaderText() {
            return headerText;
        }
    }]);

    return Service;
}();

exports.default = Service;

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/reducers/dataHandlerReducer.js":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/reducers/dataHandlerReducer.js ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.gridData = undefined;

var _createReducer2 = __webpack_require__(/*! ../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

var _types = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var gridData = exports.gridData = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_GRID_DATA, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/reducers/filterHandlerReducer.js":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/reducers/filterHandlerReducer.js ***!
  \***********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.dateConstraint = exports.convertAll = exports.dateRange = exports.selectedSubsidiary = exports.subsidiaryList = undefined;

var _createReducer6 = __webpack_require__(/*! ../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer7 = _interopRequireDefault(_createReducer6);

var _types = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var subsidiaryList = exports.subsidiaryList = (0, _createReducer7.default)({}, _defineProperty({}, types.GET_SUBSIDIARY, function (state, action) {
    return action.data;
}));

var selectedSubsidiary = exports.selectedSubsidiary = (0, _createReducer7.default)({}, _defineProperty({}, types.SET_SUBSIDIARY, function (state, action) {
    return action.data;
}));

var dateRange = exports.dateRange = (0, _createReducer7.default)({}, _defineProperty({}, types.SET_DATE_RANGE, function (state, action) {
    return action.data;
}));

var convertAll = exports.convertAll = (0, _createReducer7.default)({}, _defineProperty({}, types.CHANGE_CONVERT_ALL, function (state, action) {
    return action.data;
}));

var dateConstraint = exports.dateConstraint = (0, _createReducer7.default)({}, _defineProperty({}, types.GET_DATE_CONSTRAINT, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/pages/subsidiaryperformance/reducers/index.js":
/*!********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/subsidiaryperformance/reducers/index.js ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _dataHandlerReducer = __webpack_require__(/*! ./dataHandlerReducer.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/reducers/dataHandlerReducer.js");

var dataHandlerReducer = _interopRequireWildcard(_dataHandlerReducer);

var _filterHandlerReducer = __webpack_require__(/*! ./filterHandlerReducer.js */ "./www/js/source/reactpages/pages/subsidiaryperformance/reducers/filterHandlerReducer.js");

var filterHandlerReducer = _interopRequireWildcard(_filterHandlerReducer);

var _productReducer = __webpack_require__(/*! ../../../reducers/productReducer */ "./www/js/source/reactpages/reducers/productReducer.js");

var productReducer = _interopRequireWildcard(_productReducer);

var _baseCurrencyReducer = __webpack_require__(/*! ../../../reducers/baseCurrencyReducer */ "./www/js/source/reactpages/reducers/baseCurrencyReducer.js");

var baseCurrencyReducer = _interopRequireWildcard(_baseCurrencyReducer);

var _loaderReducer = __webpack_require__(/*! ../../../reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");

var loaderReducer = _interopRequireWildcard(_loaderReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = (0, _redux.combineReducers)(Object.assign(dataHandlerReducer, filterHandlerReducer, productReducer, baseCurrencyReducer, loaderReducer));

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

/***/ })

/******/ });
//# sourceMappingURL=subsidiaryperformance.chunk.js.map