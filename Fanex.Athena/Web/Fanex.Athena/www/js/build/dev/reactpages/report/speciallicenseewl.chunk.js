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
/******/ 		"www/js/build/dev/reactpages/report/speciallicenseewl": 0
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
/******/ 	deferredModules.push(["./www/js/source/reactpages/pages/speciallicenseewl/components/index.jsx","common"]);
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

/***/ "./www/js/source/reactpages/components/month-picker/Day.jsx":
/*!******************************************************************!*\
  !*** ./www/js/source/reactpages/components/month-picker/Day.jsx ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _Overlay = __webpack_require__(/*! ../Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");

var _Overlay2 = _interopRequireDefault(_Overlay);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Day = function (_Component) {
    _inherits(Day, _Component);

    function Day(props, context) {
        _classCallCheck(this, Day);

        var _this = _possibleConstructorReturn(this, (Day.__proto__ || Object.getPrototypeOf(Day)).call(this, props, context));

        _this.state = { showDateDrop: false };
        return _this;
    }

    _createClass(Day, [{
        key: '_renderOption',
        value: function _renderOption(min, max, value) {
            var options = [];

            for (var i = min; i <= max; i++) {
                options.push(_react2.default.createElement(
                    'li',
                    { key: i, onClick: this._onChange.bind(this, i) },
                    _react2.default.createElement(
                        'a',
                        { href: 'javascript:;' },
                        i
                    )
                ));
            }

            return options;
        }
    }, {
        key: '_onChange',
        value: function _onChange(value) {
            this.props.onChange(parseInt(value, 10));
            this.setState({ showDateDrop: false });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this2 = this;

            var value = this.props.value;

            return _react2.default.createElement(
                'div',
                { className: 'date' },
                _react2.default.createElement(
                    'label',
                    { className: 'label-text' },
                    this.props.label
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'selectedDayWrapParent' },
                    _react2.default.createElement(
                        'div',
                        { className: 'selectedDayWrap' },
                        _react2.default.createElement(
                            'div',
                            { className: 'dayPickr', onClick: function onClick() {
                                    return _this2.setState({ showDateDrop: true });
                                } },
                            _react2.default.createElement(
                                'span',
                                { className: 'selectedDay' },
                                value
                            ),
                            _react2.default.createElement(
                                'a',
                                { href: 'javascript:;', className: 'darr' },
                                _react2.default.createElement('b', null)
                            )
                        ),
                        _react2.default.createElement(
                            _Overlay2.default,
                            { isOpening: this.state.showDateDrop, onClose: this.setState.bind(this, { showDateDrop: false }) },
                            this.state.showDateDrop && _react2.default.createElement(
                                'div',
                                { className: 'dayPickrDropDown' },
                                _react2.default.createElement(
                                    'ul',
                                    null,
                                    this._renderOption(this.props.min, this.props.max, value)
                                )
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Day;
}(_react.Component);

Day.propTypes = {
    label: _propTypes2.default.string,
    value: _propTypes2.default.number.isRequired,
    min: _propTypes2.default.number.isRequired,
    max: _propTypes2.default.number.isRequired,
    onChange: _propTypes2.default.func
};
Day.defaultProps = {
    onChange: function onChange() {}
};
exports.default = Day;

/***/ }),

/***/ "./www/js/source/reactpages/components/month-picker/Month.jsx":
/*!********************************************************************!*\
  !*** ./www/js/source/reactpages/components/month-picker/Month.jsx ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _Overlay = __webpack_require__(/*! ../Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");

var _Overlay2 = _interopRequireDefault(_Overlay);

var _langHelper = __webpack_require__(/*! ../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Month = function (_Component) {
    _inherits(Month, _Component);

    function Month(props, context) {
        _classCallCheck(this, Month);

        var _this = _possibleConstructorReturn(this, (Month.__proto__ || Object.getPrototypeOf(Month)).call(this, props, context));

        var shortMonths = _langHelper2.default.getResources(['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']);
        var fullMonths = _langHelper2.default.getResources(['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']);

        _this.shortMonths = shortMonths;
        _this.fullMonths = fullMonths;

        _this.state = { expandYear: props.value.year, showMonthDrop: false };
        return _this;
    }

    _createClass(Month, [{
        key: '_onChange',
        value: function _onChange(value) {
            this.props.onChange(value);
            this.setState({ showMonthDrop: false });
        }
    }, {
        key: '_formatString',
        value: function _formatString(month, year) {
            return this.shortMonths[month] + ' ' + year;
        }
    }, {
        key: '_renderOption',
        value: function _renderOption(min, max, value) {
            var options = [];
            for (var i = min.year; i <= max.year; i++) {
                if (i === this.state.expandYear) {

                    options.push(_react2.default.createElement(
                        'li',
                        { key: i, className: 'year', style: { fontWeight: 'bold' } },
                        _react2.default.createElement(
                            'a',
                            { href: 'javascript:;' },
                            i
                        )
                    ));

                    var minMonth = i === min.year ? min.month : 0;
                    var maxMonth = i === max.year ? max.month : 11;

                    for (var j = minMonth; j <= maxMonth; j++) {
                        var isSelected = i === value.year && j === value.month;
                        var valueItem = { year: i, month: j };

                        options.push(_react2.default.createElement(
                            'li',
                            { key: '' + j + i, className: 'monthWrap', onClick: this._onChange.bind(this, valueItem) },
                            _react2.default.createElement(
                                'a',
                                { href: 'javascript:;', className: '' + (isSelected ? 'selected' : '') },
                                this.fullMonths[j]
                            )
                        ));
                    }
                } else {
                    options.push(_react2.default.createElement(
                        'li',
                        { key: i, className: 'year', onClick: this.setState.bind(this, { expandYear: i }) },
                        _react2.default.createElement(
                            'a',
                            { href: 'javascript:;' },
                            i
                        )
                    ));
                }
            }

            return options;
        }
    }, {
        key: '_getNextMonth',
        value: function _getNextMonth() {
            var month = this.props.value.month + 1;
            var year = this.props.value.year;

            if (month > 11) {
                month = 0;
                year += 1;
            }

            return { year: year, month: month };
        }
    }, {
        key: '_getPrevMonth',
        value: function _getPrevMonth() {
            var month = this.props.value.month - 1;
            var year = this.props.value.year;

            if (month < 0) {
                month = 11;
                year -= 1;
            }

            return { year: year, month: month };
        }
    }, {
        key: '_checkValidMonth',
        value: function _checkValidMonth(month, year) {
            var _props = this.props,
                min = _props.min,
                max = _props.max;


            return year > min.year && year < max.year || year === min.year && month >= min.month || year === max.year && month <= max.month;
        }
    }, {
        key: '_goToMonth',
        value: function _goToMonth(e, value) {
            e.preventDefault();

            if (value.year !== this.state.expandYear) {
                this.setState({ expandYear: value.year });
            }

            this.props.onChange(value);
        }
    }, {
        key: 'render',
        value: function render() {
            var _this2 = this;

            var nexMonth = this._getNextMonth();
            var preMonth = this._getPrevMonth();
            var validNexMonth = this._checkValidMonth(nexMonth.month, nexMonth.year);
            var validPreMonth = this._checkValidMonth(preMonth.month, preMonth.year);
            var value = this.props.value;

            return _react2.default.createElement(
                'div',
                { className: 'month' },
                _react2.default.createElement(
                    'label',
                    { className: 'label-text' },
                    this.props.label
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'monPickrWrapParent' },
                    _react2.default.createElement(
                        'div',
                        { className: 'monPickrWrap' },
                        _react2.default.createElement(
                            'div',
                            { className: 'monPickr' },
                            _react2.default.createElement(
                                'a',
                                { href: 'javascript:;', className: 'prev ' + (validPreMonth ? '' : 'disable'), onClick: function onClick(e) {
                                        return validPreMonth && _this2._goToMonth(e, preMonth);
                                    } },
                                _react2.default.createElement('b', null)
                            ),
                            _react2.default.createElement(
                                'span',
                                { className: 'selectedMonth', onClick: function onClick() {
                                        return _this2.setState({ showMonthDrop: true });
                                    } },
                                this._formatString(value.month, value.year)
                            ),
                            _react2.default.createElement(
                                'a',
                                { href: 'javascript:;', className: 'next ' + (validNexMonth ? '' : 'disable'), onClick: function onClick(e) {
                                        return validNexMonth && _this2._goToMonth(e, nexMonth);
                                    } },
                                _react2.default.createElement('b', null)
                            )
                        ),
                        _react2.default.createElement(
                            _Overlay2.default,
                            { isOpening: this.state.showMonthDrop, onClose: this.setState.bind(this, { showMonthDrop: false }) },
                            this.state.showMonthDrop && _react2.default.createElement(
                                'div',
                                { className: 'monPickrDropDown' },
                                _react2.default.createElement(
                                    'ul',
                                    { className: 'monPickrDropDownInner' },
                                    this._renderOption(this.props.min, this.props.max, value)
                                )
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Month;
}(_react.Component);

Month.propTypes = {
    label: _propTypes2.default.string,
    value: _propTypes2.default.object.isRequired,
    min: _propTypes2.default.object.isRequired,
    max: _propTypes2.default.object.isRequired,
    onChange: _propTypes2.default.func
};
Month.defaultProps = {
    label: 'View data in month',
    onChange: function onChange() {}
};
exports.default = Month;

/***/ }),

/***/ "./www/js/source/reactpages/components/month-picker/MonthPicker.jsx":
/*!**************************************************************************!*\
  !*** ./www/js/source/reactpages/components/month-picker/MonthPicker.jsx ***!
  \**************************************************************************/
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

var _Month = __webpack_require__(/*! ./Month */ "./www/js/source/reactpages/components/month-picker/Month.jsx");

var _Month2 = _interopRequireDefault(_Month);

var _Day = __webpack_require__(/*! ./Day */ "./www/js/source/reactpages/components/month-picker/Day.jsx");

var _Day2 = _interopRequireDefault(_Day);

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var MonthPicker = function (_Component) {
    _inherits(MonthPicker, _Component);

    function MonthPicker(props) {
        _classCallCheck(this, MonthPicker);

        var _this = _possibleConstructorReturn(this, (MonthPicker.__proto__ || Object.getPrototypeOf(MonthPicker)).call(this, props));

        _initialiseProps.call(_this);

        var format = 'MM/DD/YYYY';

        var minDate = (0, _moment2.default)(props.minDate, format);
        var maxDate = (0, _moment2.default)(props.maxDate, format);

        var min = { year: minDate.year(), month: minDate.month(), date: minDate.date() };
        var max = { year: maxDate.year(), month: maxDate.month(), date: maxDate.date() };
        _this.min = min;
        _this.max = max;

        var fromDate = (0, _moment2.default)(props.fromDate, format);
        var toDate = (0, _moment2.default)(props.toDate, format);

        var currentMonth = { year: fromDate.year(), month: fromDate.month() };

        _this.state = {
            month: currentMonth,
            from: fromDate.date(),
            to: toDate.date()
        };
        return _this;
    }

    _createClass(MonthPicker, [{
        key: '_formatDate',
        value: function _formatDate(date, month, year) {
            return (month < 9 ? '0' + (month + 1) : month + 1) + '/' + (date < 10 ? '0' + date : date) + '/' + year;
        }
    }, {
        key: 'render',
        value: function render() {
            var _state = this.state,
                month = _state.month,
                from = _state.from,
                to = _state.to;

            var daysInMonth = (0, _moment2.default)({ year: month.year, month: month.month }).daysInMonth();

            var startDate = month.year === this.min.year && month.month === this.min.month ? this.min.date : 1;
            var endDate = month.year === this.max.year && month.month === this.max.month ? this.max.date : daysInMonth;

            return _react2.default.createElement(
                'div',
                { className: 'month-picker' },
                _react2.default.createElement(_Month2.default, _extends({ min: this.min, max: this.max, value: month }, this.props.monthOption, { onChange: this._onMonthChange })),
                _react2.default.createElement(_Day2.default, _extends({ min: startDate, max: endDate > to ? to : endDate, value: from }, this.props.fromDateOption, { onChange: this._onFromDateChange })),
                _react2.default.createElement(_Day2.default, _extends({ min: startDate < from ? from : startDate, max: endDate, value: to }, this.props.toDateOption, { onChange: this._onToDateChange }))
            );
        }
    }]);

    return MonthPicker;
}(_react.Component);

MonthPicker.propTypes = {
    monthOption: _propTypes2.default.object.isRequired,
    fromDateOption: _propTypes2.default.object.isRequired,
    toDateOption: _propTypes2.default.object.isRequired,
    onChange: _propTypes2.default.func,

    minDate: _propTypes2.default.string.isRequired,
    maxDate: _propTypes2.default.string.isRequired,
    fromDate: _propTypes2.default.string.isRequired,
    toDate: _propTypes2.default.string.isRequired
};
MonthPicker.defaultProps = {
    label: 'View data in month',
    onChange: function onChange() {}
};

var _initialiseProps = function _initialiseProps() {
    var _this2 = this;

    this._onMonthChange = function (value) {
        var minDate = value.year === _this2.min.year && value.month === _this2.min.month ? _this2.min.date : 1;
        var maxDate = value.year === _this2.max.year && value.month === _this2.max.month ? _this2.max.date : (0, _moment2.default)({ year: value.year, month: value.month }).daysInMonth();

        _this2.setState({
            month: value,
            minDate: minDate,
            maxDate: maxDate,
            from: minDate,
            to: maxDate
        });

        _this2.props.onChange({
            fromDate: _this2._formatDate(minDate, value.month, value.year),
            toDate: _this2._formatDate(maxDate, value.month, value.year)
        });
    };

    this._onFromDateChange = function (value) {
        _this2.setState({ from: value });

        _this2.props.onChange({
            fromDate: _this2._formatDate(value, _this2.state.month.month, _this2.state.month.year),
            toDate: _this2._formatDate(_this2.state.to, _this2.state.month.month, _this2.state.month.year)
        });
    };

    this._onToDateChange = function (value) {
        _this2.setState({ to: value });

        _this2.props.onChange({
            fromDate: _this2._formatDate(_this2.state.from, _this2.state.month.month, _this2.state.month.year),
            toDate: _this2._formatDate(value, _this2.state.month.month, _this2.state.month.year)
        });
    };
};

exports.default = MonthPicker;

/***/ }),

/***/ "./www/js/source/reactpages/components/month-picker/index.js":
/*!*******************************************************************!*\
  !*** ./www/js/source/reactpages/components/month-picker/index.js ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _MonthPicker = __webpack_require__(/*! ./MonthPicker */ "./www/js/source/reactpages/components/month-picker/MonthPicker.jsx");

var _MonthPicker2 = _interopRequireDefault(_MonthPicker);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _MonthPicker2.default;

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

/***/ "./www/js/source/reactpages/constants/report.js":
/*!******************************************************!*\
  !*** ./www/js/source/reactpages/constants/report.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
var ReportLevel = exports.ReportLevel = {
    MAIN: 0,
    LICENSEE: 1,
    CUSTOMER: 2
};

var ReportBy = exports.ReportBy = {
    DAILY: 1,
    MONTHLY: 2
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

/***/ "./www/js/source/reactpages/lib/report.js":
/*!************************************************!*\
  !*** ./www/js/source/reactpages/lib/report.js ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _report = __webpack_require__(/*! ../constants/report */ "./www/js/source/reactpages/constants/report.js");

var _site = __webpack_require__(/*! ./site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var fetchData = function fetchData(relativeUrl, params) {
    var url = _site2.default.resolveClientUrl(relativeUrl);
    return _axios2.default.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': _site2.default.getXsrfToken()
        }
    });
};

var Report = function () {
    function Report() {
        _classCallCheck(this, Report);
    }

    _createClass(Report, null, [{
        key: 'getDrilldownDateRange',
        value: function getDrilldownDateRange(reportBy, fromDate, toDate, drilldownDate) {
            var fdate = typeof drilldownDate === "string" ? new Date(drilldownDate) : drilldownDate;
            var tdate = fdate;

            if (Number(reportBy) === _report.ReportBy.MONTHLY) {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = new Date(fromDate);

                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }

                fdate = new Date(year, month, fday, 0, 0, 0, 0);

                var originalTDate = new Date(toDate);

                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                } else {
                    tdate = new Date(year, month + 1, 0);
                }
            }

            return {
                fromDate: fdate,
                toDate: tdate
            };
        }
    }, {
        key: 'fetchGridData',
        value: function fetchGridData(relativeUrl, params, callBack, errorHandler) {
            return fetchData(relativeUrl, params);
        }
    }]);

    return Report;
}();

exports.default = Report;

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

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/actions/filterHandlerAction.js":
/*!*****************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/actions/filterHandlerAction.js ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setReportTitle = exports.setParams = exports.setDateRange = exports.setSelectedReportBy = undefined;

var _types = __webpack_require__(/*! ./types.js */ "./www/js/source/reactpages/pages/speciallicenseewl/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedReportBy = exports.setSelectedReportBy = function setSelectedReportBy(value) {
    return {
        type: types.SET_REPORTBY,
        data: value
    };
};

var setDateRange = exports.setDateRange = function setDateRange(dateRange) {
    var range = {
        fromDate: dateRange.fromDate,
        toDate: dateRange.toDate
    };

    return {
        type: types.SET_DATE_RANGE,
        data: range
    };
};

var setParams = exports.setParams = function setParams(params) {
    return {
        type: types.SET_PARAMS,
        data: params
    };
};

var setReportTitle = exports.setReportTitle = function setReportTitle(title) {
    return {
        type: types.SET_REPORT_TITLE,
        data: title
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/actions/index.js":
/*!***************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/actions/index.js ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _filterHandlerAction = __webpack_require__(/*! ./filterHandlerAction.js */ "./www/js/source/reactpages/pages/speciallicenseewl/actions/filterHandlerAction.js");

var filterHandlerAction = _interopRequireWildcard(_filterHandlerAction);

var _productAction = __webpack_require__(/*! ../../../actions/productAction */ "./www/js/source/reactpages/actions/productAction.js");

var productAction = _interopRequireWildcard(_productAction);

var _currencyAction = __webpack_require__(/*! ../../../actions/currencyAction */ "./www/js/source/reactpages/actions/currencyAction.js");

var currencyAction = _interopRequireWildcard(_currencyAction);

var _loaderAction = __webpack_require__(/*! ../../../actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");

var loaderAction = _interopRequireWildcard(_loaderAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, filterHandlerAction, productAction, currencyAction, loaderAction);

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/actions/types.js":
/*!***************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/actions/types.js ***!
  \***************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var SET_GRID_DATA = exports.SET_GRID_DATA = 'SET_GRID_DATA';

var GET_REPORTBY = exports.GET_REPORTBY = 'GET_REPORTBY';
var SET_REPORTBY = exports.SET_REPORTBY = 'SET_REPORTBY';

var SET_DATE_RANGE = exports.SET_DATE_RANGE = 'SET_DATE_RANGE';
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = 'GET_DATE_CONSTRAINT';

var GET_XSRF_TOKEN = exports.GET_XSRF_TOKEN = 'GET_XSRF_TOKEN';

var SET_PARAMS = exports.SET_PARAMS = 'SET_PARAMS';

var SET_REPORT_TITLE = exports.SET_REPORT_TITLE = 'SET_REPORT_TITLE';

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/components/AppContainer.jsx":
/*!**************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/components/AppContainer.jsx ***!
  \**************************************************************************************/
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

var _index = __webpack_require__(/*! ../actions/index */ "./www/js/source/reactpages/pages/speciallicenseewl/actions/index.js");

var _FormFilter = __webpack_require__(/*! ./FormFilter */ "./www/js/source/reactpages/pages/speciallicenseewl/components/FormFilter.jsx");

var _FormFilter2 = _interopRequireDefault(_FormFilter);

var _datagrid = __webpack_require__(/*! ./datagrid */ "./www/js/source/reactpages/pages/speciallicenseewl/components/datagrid.jsx");

var _datagrid2 = _interopRequireDefault(_datagrid);

var _Loader = __webpack_require__(/*! ../../../components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");

var _Loader2 = _interopRequireDefault(_Loader);

var _AppLayout = __webpack_require__(/*! ../../../layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");

var _AppLayout2 = _interopRequireDefault(_AppLayout);

var _ReportTitle = __webpack_require__(/*! ../../../components/ReportTitle */ "./www/js/source/reactpages/components/ReportTitle.jsx");

var _ReportTitle2 = _interopRequireDefault(_ReportTitle);

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
        _react2.default.createElement(_FormFilter2.default, null),
        _react2.default.createElement(
          'div',
          { className: 'result-content' },
          this.props.reportTitle && _react2.default.createElement(_ReportTitle2.default, { content: this.props.reportTitle }),
          _react2.default.createElement(_datagrid2.default, null)
        )
      );
    }
  }]);

  return AppContainer;
}(_react.Component);

function mapDispatchToProps(dispatch) {
  return (0, _redux.bindActionCreators)(_index.ActionCreators, dispatch);
}

function mapStateToProps(state) {
  return Object.assign({}, { isLoading: state.isLoading, reportTitle: state.reportTitle });
}

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(AppContainer);

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/components/FormFilter.jsx":
/*!************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/components/FormFilter.jsx ***!
  \************************************************************************************/
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

var _index = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/speciallicenseewl/actions/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _reactSelect = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");

var _reactSelect2 = _interopRequireDefault(_reactSelect);

var _reactMultiselect = __webpack_require__(/*! @9softstudio/react-multiselect */ "./node_modules/@9softstudio/react-multiselect/dist/index.js");

var _reactMultiselect2 = _interopRequireDefault(_reactMultiselect);

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _report = __webpack_require__(/*! @jsroot/lib/report */ "./www/js/source/reactpages/lib/report.js");

var _report2 = _interopRequireDefault(_report);

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/speciallicenseewl/lib/service.js");

var _reportExcel = __webpack_require__(/*! ../lib/reportExcel */ "./www/js/source/reactpages/pages/speciallicenseewl/lib/reportExcel.js");

var _reportExcel2 = _interopRequireDefault(_reportExcel);

var _index2 = __webpack_require__(/*! @jsroot/components/month-picker/index */ "./www/js/source/reactpages/components/month-picker/index.js");

var _index3 = _interopRequireDefault(_index2);

var _AppData = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");

var _AppData2 = _interopRequireDefault(_AppData);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var FormFilter = exports.FormFilter = function (_Component) {
    _inherits(FormFilter, _Component);

    function FormFilter(props) {
        _classCallCheck(this, FormFilter);

        var _this = _possibleConstructorReturn(this, (FormFilter.__proto__ || Object.getPrototypeOf(FormFilter)).call(this, props));

        _this.submit = function (event) {
            event.preventDefault();
            if (!_this.isValidModel) {
                return;
            }

            var params = _this._getParams();
            if (!params) {
                return;
            }

            _this.props.setParams(params);
        };

        _this.exportExcel = function (event) {
            event.preventDefault();

            var params = _this._getParams();
            if (!params) {
                return;
            }

            _this.props.toggleLoader(true);

            _report2.default.fetchGridData(_service.Url, { xsrfToken: _this.props.xsrfToken, data: params }).then(function (response) {
                _this._doExportExcel(params, response.data);
            }).catch(function (error) {
                _service.Service.showErrorMsg();
                _this.props.toggleLoader(false);
            });
        };

        _this.productChangeHandler = function (currentItem, selectedItemsKey) {
            _this.props.setSelectedProduct(selectedItemsKey);
        };

        _this._monthPickerHandle = function (dateRange) {
            _this.props.setDateRange(dateRange);
        };

        return _this;
    }

    _createClass(FormFilter, [{
        key: '_doExportExcel',
        value: function _doExportExcel(params, gridData) {
            var _this2 = this;

            if (!gridData.Data || !gridData.Data.length) {
                this.props.toggleLoader(false);
                alert(_langHelper2.default.getSingleResource("NoInfo"));
                return;
            }

            if (gridData.Data.length > 1) {
                _service.Service.sort(gridData.Data, params.SortBy, params.SortOrder);
            }

            var title = _service.Service.getReportTitle(params.FromDate, params.ToDate, params.CurrencyId, params.CurrencyName, params.BaseCurrency);
            var excel = new _reportExcel2.default(gridData, title, parseInt(params.ReportBy), params.ProductCodeList);
            excel.export(function () {
                _this2.props.toggleLoader(false);
            });
        }
    }, {
        key: '_getParams',
        value: function _getParams() {
            if (!this.props.xsrfToken) {
                return null;
            }

            var _props$dateRange = this.props.dateRange,
                fromDate = _props$dateRange.fromDate,
                toDate = _props$dateRange.toDate;


            return {
                FromDate: _site2.default.convertDateToString(fromDate),
                ToDate: _site2.default.convertDateToString(toDate),
                CurrencyName: this.props.selectedCurrency.label,
                CurrencyId: this.props.selectedCurrency.value,
                BaseCurrency: this.props.selectedCurrency.baseCurrency,
                ProductCodeList: this.props.selectedProduct.split(','),
                ReportBy: this.props.selectedReportBy,
                SortBy: this.props.params.SortBy,
                SortOrder: this.props.params.SortOrder,
                Timestamp: new Date().getTime()
            };
        }
    }, {
        key: '_renderReportByOption',
        value: function _renderReportByOption() {
            var _this3 = this;

            var _props = this.props,
                reportByList = _props.reportByList,
                selectedReportBy = _props.selectedReportBy;


            return _react2.default.createElement(
                'div',
                { className: 'form-group' },
                _react2.default.createElement(
                    'label',
                    { className: 'label-text' },
                    _langHelper2.default.getSingleResource('ReportBy')
                ),
                reportByList.map(function (option, i) {
                    var key = 'ReportBy' + i;
                    return _react2.default.createElement(
                        'span',
                        { key: 'Group' + key },
                        _react2.default.createElement('input', { type: 'radio', value: option.value, checked: selectedReportBy === option.value, id: key,
                            onChange: function onChange(event) {
                                return _this3.props.setSelectedReportBy(event.target.value);
                            } }),
                        _react2.default.createElement(
                            'label',
                            { htmlFor: key },
                            option.label
                        )
                    );
                })
            );
        }
    }, {
        key: '_renderProductSelectList',
        value: function _renderProductSelectList() {
            var productOptions = _AppData2.default.productOptions;
            return productOptions && productOptions.length && _react2.default.createElement(_reactMultiselect2.default, { id: 'ProductList',
                dataSource: productOptions,
                keyField: "Code",
                valueField: "Name",
                statusField: "Status",
                optionAllLabel: _langHelper2.default.getSingleResource("All"),
                noneSelectedLabel: _langHelper2.default.getSingleResource("SelectOptions"),
                onChange: this.productChangeHandler });
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
        key: '_renderMonthPicker',
        value: function _renderMonthPicker() {
            var dateRange = this.props.dateRange;

            var dateConstraint = _AppData2.default.dateConstraint;

            var monthOption = {
                label: _langHelper2.default.getSingleResource('ViewDataInMonth')
            };

            var fromDateOption = {
                label: _langHelper2.default.getSingleResource('From'),
                id: 'FromDateInput'
            };
            var toDateOption = {
                label: _langHelper2.default.getSingleResource('To'),
                id: 'ToDateInput'
            };

            return _react2.default.createElement(_index3.default, {
                minDate: dateConstraint.minDate,
                maxDate: dateConstraint.maxDate,
                fromDate: dateRange.fromDate,
                toDate: dateRange.toDate,

                onChange: this._monthPickerHandle,

                monthOption: monthOption,
                fromDateOption: fromDateOption,
                toDateOption: toDateOption });
        }
    }, {
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'page-filter' },
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-5 col-sm-12 col-lg-5 col-xl-5' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            this._renderMonthPicker()
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-3 col-sm-12 col-lg-2 col-xl-2' },
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
                        { className: 'col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2' },
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
                        { className: 'col-12 col-md-5 col-sm-12 col-lg-2 col-xl-3' },
                        this._renderReportByOption()
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-3 col-sm-12 col-lg-2 col-xl-2' },
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
                    )
                ),
                _react2.default.createElement(
                    'div',
                    { className: 'row' },
                    _react2.default.createElement(
                        'div',
                        { className: 'caret-block' },
                        _react2.default.createElement('i', { className: 'icon-excel right', onClick: this.exportExcel, title: _langHelper2.default.getSingleResource("ExportToExcel") })
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
        reportTitle = state.reportTitle,
        isLoading = state.isLoading,
        rest = _objectWithoutProperties(state, ['gridData', 'reportTitle', 'isLoading']);

    return Object.assign({}, rest);
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(FormFilter);

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/components/datagrid.jsx":
/*!**********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/components/datagrid.jsx ***!
  \**********************************************************************************/
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

var _index = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/speciallicenseewl/actions/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _fixedTableHeader = __webpack_require__(/*! ../../../components/fixed-table-header */ "./www/js/source/reactpages/components/fixed-table-header/index.jsx");

var _numberFormat = __webpack_require__(/*! ../../../lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");

var _numberFormat2 = _interopRequireDefault(_numberFormat);

var _NoDataTitle = __webpack_require__(/*! ../../../components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");

var _NoDataTitle2 = _interopRequireDefault(_NoDataTitle);

var _report = __webpack_require__(/*! ../../../lib/report */ "./www/js/source/reactpages/lib/report.js");

var _report2 = _interopRequireDefault(_report);

var _report3 = __webpack_require__(/*! ../../../constants/report */ "./www/js/source/reactpages/constants/report.js");

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/speciallicenseewl/lib/service.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DataGrid = exports.DataGrid = function (_Component) {
    _inherits(DataGrid, _Component);

    function DataGrid(props) {
        _classCallCheck(this, DataGrid);

        var _this = _possibleConstructorReturn(this, (DataGrid.__proto__ || Object.getPrototypeOf(DataGrid)).call(this, props));

        _this.state = {
            expandTurnover: false,
            expandLicensee: false,
            gridData: {}
        };
        return _this;
    }

    _createClass(DataGrid, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps) {
            if (prevProps.params.Timestamp !== this.props.params.Timestamp) {
                this._getGridData();
            }
        }
    }, {
        key: '_getGridData',
        value: function _getGridData() {
            var _this2 = this;

            this.props.toggleLoader(true);
            _report2.default.fetchGridData(_service.Url, { xsrfToken: this.props.xsrfToken, data: this.props.params }).then(function (response) {
                var result = response.data;

                var mainData = result.Data;
                if (mainData && mainData.length > 1) {
                    _service.Service.sort(mainData, _this2.props.params.SortBy, _this2.props.params.SortOrder);

                    var sortedResult = Object.assign({}, result, { Data: mainData });

                    _this2.setState({ gridData: sortedResult });
                } else {
                    _this2.setState({ gridData: result });
                }

                _this2.props.setReportTitle(_service.Service.getReportTitle(_this2.props.params.FromDate, _this2.props.params.ToDate, _this2.props.params.CurrencyId, _this2.props.params.CurrencyName, _this2.props.params.BaseCurrency));

                _this2.props.toggleLoader(false);
            }).catch(function (error) {
                _service.Service.showErrorMsg();
                _this2.props.toggleLoader(false);
            });
        }
    }, {
        key: '_onExpandTurnOverColumn',
        value: function _onExpandTurnOverColumn(e) {
            e.stopPropagation();
            this.setState(function (prevState) {
                return { expandTurnover: !prevState.expandTurnover };
            });
        }
    }, {
        key: '_onExpandLicenseeColumn',
        value: function _onExpandLicenseeColumn(e) {
            e.stopPropagation();
            this.setState(function (prevState) {
                return { expandLicensee: !prevState.expandLicensee };
            });
        }
    }, {
        key: '_sort',
        value: function _sort(sortBy) {
            this.props.toggleLoader(true);

            var param = Object.assign({}, this.props.params);
            var mainData = this.state.gridData.Data;
            if (!param.SortBy || param.SortBy != sortBy) {
                param.SortBy = sortBy;
                param.SortOrder = 'asc';
            } else {
                param.SortOrder = param.SortOrder == 'asc' ? 'desc' : 'asc';
            }

            if (mainData && mainData.length > 1) {

                _service.Service.sort(mainData, param.SortBy, param.SortOrder);

                var sortedResult = Object.assign({}, this.state.gridData, { Data: mainData });

                this.setState({ gridData: sortedResult });
            }

            this.props.setParams(param);
            this.props.toggleLoader(false);
        }
    }, {
        key: '_buildHeader',
        value: function _buildHeader(selectedProductList, sortBy, sortOrder) {
            var _this3 = this;

            var productsCount = selectedProductList.length;
            var _state = this.state,
                expandLicensee = _state.expandLicensee,
                expandTurnover = _state.expandTurnover;

            var rowSpan = productsCount > 1 && (expandLicensee || expandTurnover) ? 2 : 1;

            var rows = [_react2.default.createElement(
                _fixedTableHeader.Row,
                { key: 'header1', style: { height: 32 } },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 50, header: true, rowSpan: rowSpan },
                    _service.HeaderText.index
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, rowSpan: rowSpan, sortable: true, sorting: sortBy === 'Date', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('Date');
                        } },
                    _service.HeaderText.date
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 100, header: true, rowSpan: rowSpan, sortable: true, sorting: sortBy === 'CurrencyName', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('CurrencyName');
                        } },
                    _service.HeaderText.currency
                ),
                productsCount > 1 ? _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200 * (productsCount * expandTurnover + 1), header: true, rowSpan: expandTurnover ? 1 : rowSpan, colSpan: productsCount * expandTurnover + 1,
                        sortable: !expandTurnover, sorting: sortBy === 'TotalTurnOver', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('TotalTurnOver');
                        } },
                    _service.HeaderText.turnover,
                    _react2.default.createElement(
                        'button',
                        { className: 'header-button btn ' + (expandTurnover ? 'btnCollapse' : ''), onClick: function onClick(e) {
                                return _this3._onExpandTurnOverColumn(e);
                            } },
                        _react2.default.createElement('i', { className: 'btn-icon icon-arrow' })
                    )
                ) : _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, rowSpan: rowSpan, sortable: true, sorting: sortBy === 'TotalTurnOver', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('TotalTurnOver');
                        } },
                    _service.HeaderText.turnover
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 130, header: true, rowSpan: rowSpan, sortable: true, sorting: sortBy === 'TotalTransaction', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('TotalTransaction');
                        } },
                    _service.HeaderText.totalTransaction
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, rowSpan: rowSpan, sortable: true, sorting: sortBy === 'CustomerWinLoss', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('CustomerWinLoss');
                        } },
                    _service.HeaderText.customer
                ),
                productsCount > 1 ? _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200 * (productsCount * expandLicensee + 1), header: true, rowSpan: expandLicensee ? 1 : rowSpan, colSpan: productsCount * expandLicensee + 1,
                        sortable: !expandLicensee, sorting: sortBy === 'TotalLicenseeWL', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('TotalLicenseeWL');
                        } },
                    _service.HeaderText.licenseeWL,
                    _react2.default.createElement(
                        'button',
                        { className: 'header-button btn ' + (expandLicensee ? 'btnCollapse' : ''), onClick: function onClick(e) {
                                return _this3._onExpandLicenseeColumn(e);
                            } },
                        _react2.default.createElement('i', { className: 'btn-icon icon-arrow' })
                    )
                ) : _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 200, header: true, rowSpan: rowSpan, sortable: true, sorting: sortBy === 'TotalLicenseeWL', asc: sortOrder === 'asc', onClick: function onClick() {
                            return _this3._sort('TotalLicenseeWL');
                        } },
                    _service.HeaderText.licenseeWL
                )
            )];

            if (rowSpan > 1) {
                var cols = [];

                if (this.state.expandTurnover) {
                    var _loop = function _loop(i) {
                        var code = selectedProductList[i];
                        var colName = 'TurnOver' + code;
                        cols.push(_react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { key: colName, colWidth: 200, header: true, sortable: true, sorting: sortBy === colName, asc: sortOrder === 'asc', onClick: function onClick() {
                                    return _this3._sort(colName);
                                } },
                            _service.HeaderText[code]
                        ));
                    };

                    for (var i = 0; i < productsCount; i++) {
                        _loop(i);
                    }
                    cols.push(_react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { key: 'turnover-total', colWidth: 200, header: true, sortable: true, sorting: sortBy === 'TotalTurnOver', asc: sortOrder === 'asc', onClick: function onClick() {
                                return _this3._sort('TotalTurnOver');
                            } },
                        _service.HeaderText.total
                    ));
                }

                if (this.state.expandLicensee) {
                    var _loop2 = function _loop2(i) {
                        var code = selectedProductList[i];
                        var colName = 'LicenseeWL' + code;
                        cols.push(_react2.default.createElement(
                            _fixedTableHeader.Cell,
                            { key: colName, colWidth: 200, header: true, sortable: true, sorting: sortBy === colName, asc: sortOrder === 'asc', onClick: function onClick() {
                                    return _this3._sort(colName);
                                } },
                            _service.HeaderText[code]
                        ));
                    };

                    for (var i = 0; i < productsCount; i++) {
                        _loop2(i);
                    }
                    cols.push(_react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { key: 'licenseeWL-total', colWidth: 200, header: true, sortable: true, sorting: sortBy === 'TotalLicenseeWL', asc: sortOrder === 'asc', onClick: function onClick() {
                                return _this3._sort('TotalLicenseeWL');
                            } },
                        _service.HeaderText.total
                    ));
                }

                rows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: 'header2', style: { height: 32 } },
                    cols
                ));
            }

            return rows;
        }
    }, {
        key: '_buildBody',
        value: function _buildBody(reportBy, selectedProductList) {
            var data = this.state.gridData.Data,
                formatDate = reportBy === _report3.ReportBy.DAILY.toString() ? 'MM/DD/YYYY' : 'MM/YYYY';

            var rows = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var _reportBy = (0, _moment2.default)(item.Date).format(formatDate);

                rows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: item.Ticks + '-' + item.CurrencyId },
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: i + 1, className: 'center-align' },
                        i + 1
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: _reportBy, className: 'center-align' },
                        _reportBy
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: item.CurrencyName, className: 'center-align' },
                        item.CurrencyName
                    ),
                    this._buildCell(item, selectedProductList)
                ));
            }

            return rows;
        }
    }, {
        key: '_buildFooter',
        value: function _buildFooter(selectedProductList) {
            var totalRows = [];

            var total = this.state.gridData.Total,
                colSpan = 2,
                rowSpan = total.length;

            for (var i = 0; i < rowSpan; i++) {
                var item = total[i];
                totalRows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: item.CurrencyName, className: 'fixed-table-total' },
                    i === 0 && _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: _service.TotalText.total, className: 'center-align', rowSpan: rowSpan, colSpan: colSpan },
                        _service.TotalText.total
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: item.CurrencyName, className: 'center-align' },
                        item.CurrencyName
                    ),
                    this._buildCell(item, selectedProductList)
                ));
            }

            var dataSummary = this.state.gridData.SummaryTotal,
                baseCurrencyList = Object.keys(dataSummary),
                colSummarySpan = colSpan + 1;

            for (var _i = baseCurrencyList.length - 1; _i >= 0; _i--) {
                var currency = baseCurrencyList[_i],
                    _item = dataSummary[currency];
                totalRows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: 'tr-grandtotal' + _i, className: 'fixed-table-grant-total' },
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: _service.TotalText.totalIn + ' ' + currency, className: 'center-align', colSpan: colSummarySpan },
                        _service.TotalText.totalIn + ' ' + currency
                    ),
                    this._buildCell(_item, selectedProductList)
                ));
            }

            return totalRows;
        }
    }, {
        key: '_buildCell',
        value: function _buildCell(item, selectedProductList) {
            var turnover = _numberFormat2.default.formatDecimal(item.TotalTurnOver),
                customerWl = _numberFormat2.default.formatDecimal(item.CustomerWinLoss),
                licenseeWL = _numberFormat2.default.formatDecimal(item.TotalLicenseeWL),
                totalTransaction = _numberFormat2.default.format(item.TotalTransaction, 0);

            var productsCount = selectedProductList.length;
            if (productsCount === 1) {
                var _rows = [_react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { key: 'turnover', title: turnover, className: 'right-align' },
                    turnover
                ), _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { key: 'totalTransaction', title: totalTransaction, className: 'right-align' },
                    totalTransaction
                ), _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { key: 'customerWl', title: customerWl, className: 'right-align' },
                    customerWl
                ), _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { key: 'licenseeWL', title: licenseeWL, className: 'right-align ' + (item.TotalLicenseeWL < 0 ? 'negative' : '') },
                    licenseeWL
                )];

                return _rows;
            }

            var rows = [];

            if (this.state.expandTurnover) {
                for (var i = 0; i < productsCount; i++) {
                    var column = 'TurnOver' + selectedProductList[i];
                    var turnoverByProduct = _numberFormat2.default.formatDecimal(item[column]);
                    rows.push(_react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { key: column, title: turnoverByProduct, className: 'right-align' },
                        turnoverByProduct
                    ));
                }
            }

            rows.push(_react2.default.createElement(
                _fixedTableHeader.Cell,
                { key: 'turnover', title: turnover, className: 'right-align' },
                turnover
            ), _react2.default.createElement(
                _fixedTableHeader.Cell,
                { key: 'totalTransaction', title: totalTransaction, className: 'right-align' },
                totalTransaction
            ), _react2.default.createElement(
                _fixedTableHeader.Cell,
                { key: 'customerWl', title: customerWl, className: 'right-align' },
                customerWl
            ));

            if (this.state.expandLicensee) {
                for (var _i2 = 0; _i2 < productsCount; _i2++) {
                    var _column = 'LicenseeWL' + selectedProductList[_i2];
                    var licenseeWLByProduct = _numberFormat2.default.formatDecimal(item[_column]);
                    rows.push(_react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { key: _column, title: licenseeWLByProduct, className: 'right-align ' + (item[_column] < 0 ? 'negative' : '') },
                        licenseeWLByProduct
                    ));
                }
            }

            rows.push(_react2.default.createElement(
                _fixedTableHeader.Cell,
                { key: 'licenseeWL', title: licenseeWL, className: 'right-align ' + (item.TotalLicenseeWL < 0 ? 'negative' : '') },
                licenseeWL
            ));

            return rows;
        }
    }, {
        key: 'render',
        value: function render() {
            var data = this.state.gridData.Data;
            if (!data) {
                return null;
            }

            var params = this.props.params;

            if (!data.length) {
                return _react2.default.createElement(_NoDataTitle2.default, null);
            }

            return _react2.default.createElement(_fixedTableHeader.Table, {
                minWidth: 800,
                autoWidth: true,
                bodyHeight: 300,
                header: this._buildHeader(params.ProductCodeList, params.SortBy, params.SortOrder),
                body: this._buildBody(params.ReportBy, params.ProductCodeList),
                footer: this._buildFooter(params.ProductCodeList)
            });
        }
    }]);

    return DataGrid;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_index.ActionCreators, dispatch);
}
function mapStateToProps(state) {
    var params = state.params,
        xsrfToken = state.xsrfToken;

    return Object.assign({}, { params: params, xsrfToken: xsrfToken });
}

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(DataGrid);

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/components/index.jsx":
/*!*******************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/components/index.jsx ***!
  \*******************************************************************************/
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

var _AppContainer = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/pages/speciallicenseewl/components/AppContainer.jsx");

var _AppContainer2 = _interopRequireDefault(_AppContainer);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../reducers/index.js */ "./www/js/source/reactpages/pages/speciallicenseewl/reducers/index.js");

var _index2 = _interopRequireDefault(_index);

var _store = __webpack_require__(/*! ../../../lib/store */ "./www/js/source/reactpages/lib/store.js");

var _store2 = _interopRequireDefault(_store);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var defaultState = Object.assign({}, window.initialState, { reportTitle: '' }, { params: { SortBy: 'Date', SortOrder: 'asc' } });
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

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/lib/reportExcel.js":
/*!*****************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/lib/reportExcel.js ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _moment = __webpack_require__(/*! moment */ "./node_modules/moment/moment.js");

var _moment2 = _interopRequireDefault(_moment);

var _excel = __webpack_require__(/*! ../../../lib/excel */ "./www/js/source/reactpages/lib/excel.js");

var _excel2 = _interopRequireDefault(_excel);

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _excelStyle = __webpack_require__(/*! ../../../constants/excelStyle */ "./www/js/source/reactpages/constants/excelStyle.js");

var _service = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/pages/speciallicenseewl/lib/service.js");

var _report = __webpack_require__(/*! ../../../constants/report */ "./www/js/source/reactpages/constants/report.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var TITLE_ROW_INDEX = 0;
var HEADER_ROW_INDEX = 1;
var MIN_COLUMN = 7;
var MAIN_SECTION = 'main';
var TOTAL_SECTION = 'total';
var SUMMARYTOTAL_SECTION = 'summary';
var HEADER_SECTION = 'header';

var ReportExcel = function () {
    function ReportExcel() {
        var gridData = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
        var title = arguments[1];
        var reportBy = arguments[2];
        var productCodes = arguments[3];

        _classCallCheck(this, ReportExcel);

        this.worksheet = {};
        this.title = title;
        this.fileName = "LicenseeWinLoss";
        this.gridData = gridData;
        this.reportBy = reportBy;
        this.productCodes = productCodes;
        this.productCount = productCodes.length;
        this.currentRowIndex = HEADER_ROW_INDEX + (this.productCount > 1 ? 2 : 1);
        this.totalColumn = MIN_COLUMN + this.productCount * 2;
        this.configColumn = this._getConfigColumn(reportBy);
        this.colSpanConfig = this._getColSpanConfig();
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
                _this._createHeader();
                _this._createSheetData();
                _this._createExcelConfig();

                _excel2.default.export(_this.worksheet, _this.fileName);
                onFinished && onFinished();
            });
        }
    }, {
        key: '_getColSpanConfig',
        value: function _getColSpanConfig() {
            var _ref;

            return _ref = {}, _defineProperty(_ref, TOTAL_SECTION, {
                colSpan: 2,
                text: _service.TotalText.total,
                style: _excelStyle.totalCellStyle.rowspan
            }), _defineProperty(_ref, SUMMARYTOTAL_SECTION, {
                colSpan: 3,
                text: _service.TotalText.totalIn,
                style: _excelStyle.grandTotalCellStyle.rowspan
            }), _ref;
        }
    }, {
        key: '_getConfigColumn',
        value: function _getConfigColumn(reportBy) {
            var _ref2, _ref3, _ref4, _rows$push, _rows$push2;

            var rowSpan = this.productCount > 1 ? 2 : 1;

            var rows = [(_ref2 = {
                field: 'Index',
                type: 's'
            }, _defineProperty(_ref2, HEADER_SECTION, { title: 'index', isShow: true, rowSpan: rowSpan, index_r: HEADER_ROW_INDEX }), _defineProperty(_ref2, MAIN_SECTION, { isShow: true, style: _excelStyle.bodyCellStyle.center, width: 50 }), _defineProperty(_ref2, TOTAL_SECTION, { isShow: false }), _defineProperty(_ref2, SUMMARYTOTAL_SECTION, { isShow: false }), _ref2), (_ref3 = {
                field: 'Date',
                type: 's'
            }, _defineProperty(_ref3, HEADER_SECTION, { title: 'date', isShow: true, rowSpan: rowSpan }), _defineProperty(_ref3, MAIN_SECTION, { isShow: true, style: _excelStyle.bodyCellStyle.center, width: 150 }), _defineProperty(_ref3, TOTAL_SECTION, { isShow: false }), _defineProperty(_ref3, SUMMARYTOTAL_SECTION, { isShow: false }), _ref3), (_ref4 = {
                field: 'CurrencyName',
                type: 's'
            }, _defineProperty(_ref4, HEADER_SECTION, { title: 'currency', isShow: true, rowSpan: rowSpan }), _defineProperty(_ref4, MAIN_SECTION, { isShow: true, style: _excelStyle.bodyCellStyle.center, width: 100 }), _defineProperty(_ref4, TOTAL_SECTION, { isShow: true, style: _excelStyle.totalCellStyle.center }), _defineProperty(_ref4, SUMMARYTOTAL_SECTION, { isShow: false }), _ref4)];

            this._createExpandColumns('TurnOver', 'turnover', rows, 3, false);

            rows.push((_rows$push = {
                field: 'TotalTransaction',
                type: 'n'
            }, _defineProperty(_rows$push, HEADER_SECTION, { title: 'totalTransaction', isShow: true, rowSpan: rowSpan, index_r: HEADER_ROW_INDEX }), _defineProperty(_rows$push, MAIN_SECTION, { isShow: true, style: _excelStyle.bodyCellStyle.number, width: 130 }), _defineProperty(_rows$push, TOTAL_SECTION, { isShow: true, style: _excelStyle.totalCellStyle.number }), _defineProperty(_rows$push, SUMMARYTOTAL_SECTION, { isShow: true, style: _excelStyle.grandTotalCellStyle.number }), _rows$push), (_rows$push2 = {
                field: 'CustomerWinLoss',
                type: 'n'
            }, _defineProperty(_rows$push2, HEADER_SECTION, { title: 'customer', isShow: true, rowSpan: rowSpan, index_r: HEADER_ROW_INDEX }), _defineProperty(_rows$push2, MAIN_SECTION, { isShow: true, style: _excelStyle.bodyCellStyle.decimal, width: 200 }), _defineProperty(_rows$push2, TOTAL_SECTION, { isShow: true, style: _excelStyle.totalCellStyle.decimal }), _defineProperty(_rows$push2, SUMMARYTOTAL_SECTION, { isShow: true, style: _excelStyle.grandTotalCellStyle.decimal }), _rows$push2));

            this._createExpandColumns('LicenseeWL', 'licenseeWL', rows, this.totalColumn - (this.productCount > 1 ? this.productCount + 1 : 1), true);

            return rows;
        }
    }, {
        key: '_createExpandColumns',
        value: function _createExpandColumns(name, title, rows, colIndex, hasNegativeStyle) {
            if (this.productCount > 1) {
                var _rows$push3, _rows$push5;

                rows.push((_rows$push3 = {}, _defineProperty(_rows$push3, HEADER_SECTION, { title: title, isShow: true, rowSpan: this.productCount > 1 ? 1 : rowSpan, colSpan: this.productCount + 1 }), _defineProperty(_rows$push3, MAIN_SECTION, { isShow: false }), _defineProperty(_rows$push3, TOTAL_SECTION, { isShow: false }), _defineProperty(_rows$push3, SUMMARYTOTAL_SECTION, { isShow: false }), _rows$push3));
                for (var i = 0; i < this.productCount; i++) {
                    var _rows$push4;

                    var code = this.productCodes[i];
                    rows.push((_rows$push4 = {
                        field: name + code,
                        type: 'n'
                    }, _defineProperty(_rows$push4, HEADER_SECTION, { title: code, isShow: true, index_c: colIndex + i, index_r: HEADER_ROW_INDEX + 1 }), _defineProperty(_rows$push4, MAIN_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.bodyCellStyle.decimal, negative: _excelStyle.bodyCellStyle.negativeDecimal } : _excelStyle.bodyCellStyle.decimal, isShow: true, width: 200 }), _defineProperty(_rows$push4, TOTAL_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.totalCellStyle.decimal, negative: _excelStyle.totalCellStyle.negativeDecimal } : _excelStyle.totalCellStyle.decimal, isShow: true }), _defineProperty(_rows$push4, SUMMARYTOTAL_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.grandTotalCellStyle.decimal, negative: _excelStyle.grandTotalCellStyle.negativeDecimal } : _excelStyle.grandTotalCellStyle.decimal, isShow: true }), _rows$push4));
                }
                rows.push((_rows$push5 = {
                    field: 'Total' + name,
                    type: 'n'
                }, _defineProperty(_rows$push5, HEADER_SECTION, { title: 'total', isShow: true }), _defineProperty(_rows$push5, MAIN_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.bodyCellStyle.decimal, negative: _excelStyle.bodyCellStyle.negativeDecimal } : _excelStyle.bodyCellStyle.decimal, isShow: true, width: 200 }), _defineProperty(_rows$push5, TOTAL_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.totalCellStyle.decimal, negative: _excelStyle.totalCellStyle.negativeDecimal } : _excelStyle.totalCellStyle.decimal, isShow: true }), _defineProperty(_rows$push5, SUMMARYTOTAL_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.grandTotalCellStyle.decimal, negative: _excelStyle.grandTotalCellStyle.negativeDecimal } : _excelStyle.grandTotalCellStyle.decimal, isShow: true }), _rows$push5));
            } else {
                var _rows$push6;

                rows.push((_rows$push6 = {
                    field: name + this.productCodes[0],
                    type: 'n'
                }, _defineProperty(_rows$push6, HEADER_SECTION, { title: title, isShow: true }), _defineProperty(_rows$push6, MAIN_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.bodyCellStyle.decimal, negative: _excelStyle.bodyCellStyle.negativeDecimal } : _excelStyle.bodyCellStyle.decimal, isShow: true, width: 200 }), _defineProperty(_rows$push6, TOTAL_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.totalCellStyle.decimal, negative: _excelStyle.totalCellStyle.negativeDecimal } : _excelStyle.totalCellStyle.decimal, isShow: true }), _defineProperty(_rows$push6, SUMMARYTOTAL_SECTION, { hasNegativeStyle: hasNegativeStyle, style: hasNegativeStyle ? { positive: _excelStyle.grandTotalCellStyle.decimal, negative: _excelStyle.grandTotalCellStyle.negativeDecimal } : _excelStyle.grandTotalCellStyle.decimal, isShow: true }), _rows$push6));
            }
        }
    }, {
        key: '_createTitle',
        value: function _createTitle(colSpan) {
            this.mergeConfig.push({ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: colSpan - 1 } });
            var address = _excel2.default.getCellAddress({ c: 0, r: TITLE_ROW_INDEX });
            this.worksheet[address] = { t: "s", v: this.title, s: _excelStyle.titleCellStyle };
        }
    }, {
        key: '_createHeader',
        value: function _createHeader() {
            var nextColumnIndex = 0;
            var nextRowIndex = 0;
            var totalCol = this.configColumn.length;
            for (var i = 0; i < totalCol; i++) {
                var column = this.configColumn[i].header;
                if (column.isShow) {
                    nextColumnIndex = column.index_c ? column.index_c : nextColumnIndex;
                    nextRowIndex = column.index_r ? column.index_r : nextRowIndex;
                    var address = _excel2.default.getCellAddress({ c: nextColumnIndex, r: nextRowIndex });
                    var header = _service.HeaderText[column.title];
                    this.worksheet[address] = { t: "s", v: header, s: _excelStyle.headerCellStyle };

                    if (column.colSpan > 1 || column.rowSpan > 1) {
                        this.mergeConfig.push({
                            s: { r: nextRowIndex, c: nextColumnIndex },
                            e: { r: nextRowIndex + (column.rowSpan || 1) - 1, c: nextColumnIndex + (column.colSpan || 1) - 1 } });
                    }

                    nextColumnIndex += column.colSpan || 1;
                }
            }

            this._createTitle(nextColumnIndex);
        }
    }, {
        key: '_createSheetData',
        value: function _createSheetData() {
            this._createMainData();
            this._createTotalData();
            this._createSummaryData();
        }
    }, {
        key: '_createMainData',
        value: function _createMainData() {
            var totalData = this.gridData.Data.length;
            for (var i = 0; i < totalData; i++) {
                var data = this.gridData.Data[i];
                data.Date = (0, _moment2.default)(data.Date).format(this.reportBy === _report.ReportBy.DAILY ? "MM/DD/YYYY" : "MM/YYYY");
                data.Index = i + 1;

                this._createRowBySection(MAIN_SECTION, 0, this.currentRowIndex, data);
                ++this.currentRowIndex;
            }
        }
    }, {
        key: '_createTotalData',
        value: function _createTotalData() {
            var totalData = this.gridData.Total.length;

            var totalColSpanConfig = this.colSpanConfig[TOTAL_SECTION];
            this._createMergeTotalDataColumn(this.currentRowIndex, totalData, 0, totalColSpanConfig.colSpan, totalColSpanConfig.text, totalColSpanConfig.style);

            for (var i = 0; i < totalData; i++) {
                var data = this.gridData.Total[i];
                this._createRowBySection(TOTAL_SECTION, totalColSpanConfig.colSpan, this.currentRowIndex, data);
                ++this.currentRowIndex;
            }
        }
    }, {
        key: '_createSummaryData',
        value: function _createSummaryData() {
            var summaryColSpanConfig = this.colSpanConfig[SUMMARYTOTAL_SECTION];

            var keys = Object.keys(this.gridData.SummaryTotal);
            var length = keys.length;
            for (var i = length - 1; i >= 0; i--) {
                var key = keys[i];
                var data = this.gridData.SummaryTotal[key];

                this._createMergeTotalDataColumn(this.currentRowIndex, 1, 0, summaryColSpanConfig.colSpan, summaryColSpanConfig.text + ' ' + key, summaryColSpanConfig.style);

                this._createRowBySection(SUMMARYTOTAL_SECTION, summaryColSpanConfig.colSpan, this.currentRowIndex, data);
                ++this.currentRowIndex;
            }
        }
    }, {
        key: '_createMergeTotalDataColumn',
        value: function _createMergeTotalDataColumn(startRowIndex, rowSpan, startColIndex, colSpan, text, style) {
            var address = _excel2.default.getCellAddress({ c: startColIndex, r: startRowIndex });
            this.worksheet[address] = { t: 's', v: text, s: style };
            this.mergeConfig.push({
                s: { r: startRowIndex, c: startColIndex },
                e: { r: startRowIndex + rowSpan - 1, c: startColIndex + colSpan - 1 } });
        }
    }, {
        key: '_createRowBySection',
        value: function _createRowBySection(section, startColIndex, startRowIndex, data) {
            var nextColumnIndex = startColIndex;
            var totalCol = this.configColumn.length;
            for (var i = 0; i < totalCol; i++) {
                var column = this.configColumn[i];
                var sectionColumn = column[section];
                if (sectionColumn.isShow) {
                    nextColumnIndex = sectionColumn.index_c ? sectionColumn.index_c : nextColumnIndex;
                    var address = _excel2.default.getCellAddress({ c: nextColumnIndex, r: startRowIndex });
                    var value = data[column.field];
                    this.worksheet[address] = { t: column.type, v: value, s: sectionColumn.hasNegativeStyle ? value >= 0 ? sectionColumn.style.positive : sectionColumn.style.negative : sectionColumn.style };

                    ++nextColumnIndex;
                }
            }
        }
    }, {
        key: '_createExcelConfig',
        value: function _createExcelConfig() {
            var columnModels = [];
            var totalCol = this.configColumn.length;
            for (var i = 0; i < totalCol; i++) {
                var column = this.configColumn[i][MAIN_SECTION];
                if (column.isShow) {
                    columnModels.push({ wpx: column.width });
                }
            }

            var cellStart = _excel2.default.getCellAddress({ r: 0, c: 0 });
            var cellEnd = _excel2.default.getCellAddress({ r: this.currentRowIndex, c: this.totalColumn });

            this.worksheet["!merges"] = this.mergeConfig;
            this.worksheet["!ref"] = cellStart + ':' + cellEnd;

            this.worksheet['!cols'] = columnModels;
        }
    }]);

    return ReportExcel;
}();

exports.default = ReportExcel;

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/lib/service.js":
/*!*************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/lib/service.js ***!
  \*************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.Service = exports.TotalText = exports.HeaderText = exports.Url = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _langHelper = __webpack_require__(/*! ../../../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var Url = exports.Url = 'api/SpecialLicenseeWinLossReport/Index';

var HeaderText = exports.HeaderText = {
    index: '#',
    date: _langHelper2.default.getSingleResource('Date'),
    currency: _langHelper2.default.getSingleResource('Currency'),
    turnover: _langHelper2.default.getSingleResource('Turnover'),
    totalTransaction: _langHelper2.default.getSingleResource('TotalTransaction'),
    customer: _langHelper2.default.getSingleResource('CustomerW_L'),
    licenseeWL: _langHelper2.default.getSingleResource('LicenseeW_L'),
    total: _langHelper2.default.getSingleResource('Total'),
    SB: _langHelper2.default.getSingleResource('ProductSB'),
    NG: _langHelper2.default.getSingleResource('ProductNG')
};

var TotalText = exports.TotalText = {
    total: _langHelper2.default.getSingleResource('Total'),
    totalIn: _langHelper2.default.getSingleResource('Totalin')
};

var sortNumber = function sortNumber(a, b) {
    return a - b;
};
var sortString = function sortString(a, b) {
    return a > b ? 1 : a < b ? -1 : 0;
};

var Service = exports.Service = function () {
    function Service() {
        _classCallCheck(this, Service);
    }

    _createClass(Service, null, [{
        key: 'getReportTitle',
        value: function getReportTitle(fromDate, toDate, currencyId, currencyName, baseCurrency) {
            var viewInByCurrency = currencyId == 0 ? _langHelper2.default.getSingleResource('AllCurrencies') : '' + _langHelper2.default.getSingleResource('Viewin').replace('{0}', baseCurrency.toUpperCase() || currencyName);

            return _langHelper2.default.getSingleResource('LicenseeWinLoss') + ', GMT+8, ' + fromDate + ' - ' + toDate + ' - ' + viewInByCurrency;
        }
    }, {
        key: 'showErrorMsg',
        value: function showErrorMsg() {
            alert(_langHelper2.default.getSingleResource('ErrorProcessingMsg'));
        }
    }, {
        key: 'sort',
        value: function sort(data, sortBy, sortOrder) {

            sortBy = sortBy === 'Date' ? 'Ticks' : sortBy;

            var compareFunc = null;

            if (sortBy === 'CurrencyName') {
                if (sortOrder === 'asc') {
                    compareFunc = function compareFunc(a, b) {
                        return sortString(a[sortBy], b[sortBy]);
                    };
                } else {
                    compareFunc = function compareFunc(a, b) {
                        return sortString(b[sortBy], a[sortBy]);
                    };
                }
            } else {
                if (sortOrder === 'asc') {
                    compareFunc = function compareFunc(a, b) {
                        return sortNumber(a[sortBy], b[sortBy]);
                    };
                } else {
                    compareFunc = function compareFunc(a, b) {
                        return sortNumber(b[sortBy], a[sortBy]);
                    };
                }
            }

            data.sort(compareFunc);
        }
    }]);

    return Service;
}();

;

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/reducers/filterHandlerReducer.js":
/*!*******************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/reducers/filterHandlerReducer.js ***!
  \*******************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.reportTitle = exports.params = exports.dateConstraint = exports.xsrfToken = exports.dateRange = exports.selectedReportBy = exports.reportByList = undefined;

var _createReducer8 = __webpack_require__(/*! ../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer9 = _interopRequireDefault(_createReducer8);

var _types = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/pages/speciallicenseewl/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var reportByList = exports.reportByList = (0, _createReducer9.default)({}, _defineProperty({}, types.GET_REPORTBY, function (state, action) {
    return action.data;
}));

var selectedReportBy = exports.selectedReportBy = (0, _createReducer9.default)({}, _defineProperty({}, types.SET_REPORTBY, function (state, action) {
    return action.data;
}));

var dateRange = exports.dateRange = (0, _createReducer9.default)({}, _defineProperty({}, types.SET_DATE_RANGE, function (state, action) {
    return action.data;
}));

var xsrfToken = exports.xsrfToken = (0, _createReducer9.default)({}, _defineProperty({}, types.GET_XSRF_TOKEN, function (state, action) {
    return action.data;
}));

var dateConstraint = exports.dateConstraint = (0, _createReducer9.default)({}, _defineProperty({}, types.GET_DATE_CONSTRAINT, function (state, action) {
    return action.data;
}));

var params = exports.params = (0, _createReducer9.default)({}, _defineProperty({}, types.SET_PARAMS, function (state, action) {
    return action.data;
}));

var reportTitle = exports.reportTitle = (0, _createReducer9.default)({}, _defineProperty({}, types.SET_REPORT_TITLE, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/pages/speciallicenseewl/reducers/index.js":
/*!****************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/speciallicenseewl/reducers/index.js ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _filterHandlerReducer = __webpack_require__(/*! ./filterHandlerReducer.js */ "./www/js/source/reactpages/pages/speciallicenseewl/reducers/filterHandlerReducer.js");

var filterHandlerReducer = _interopRequireWildcard(_filterHandlerReducer);

var _productReducer = __webpack_require__(/*! ../../../reducers/productReducer */ "./www/js/source/reactpages/reducers/productReducer.js");

var productReducer = _interopRequireWildcard(_productReducer);

var _currencyReducer = __webpack_require__(/*! ../../../reducers/currencyReducer */ "./www/js/source/reactpages/reducers/currencyReducer.js");

var currencyReducer = _interopRequireWildcard(_currencyReducer);

var _loaderReducer = __webpack_require__(/*! ../../../reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");

var loaderReducer = _interopRequireWildcard(_loaderReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = (0, _redux.combineReducers)(Object.assign(filterHandlerReducer, productReducer, currencyReducer, loaderReducer));

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
//# sourceMappingURL=speciallicenseewl.chunk.js.map