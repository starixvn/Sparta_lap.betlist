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
/******/ 		"www/js/build/dev/reactpages/bettingticketscenter/running": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
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
/******/ 	var jsonpArray = window["webpackJsonp"] = window["webpackJsonp"] || [];
/******/ 	var oldJsonpFunction = jsonpArray.push.bind(jsonpArray);
/******/ 	jsonpArray.push = webpackJsonpCallback;
/******/ 	jsonpArray = jsonpArray.slice();
/******/ 	for(var i = 0; i < jsonpArray.length; i++) webpackJsonpCallback(jsonpArray[i]);
/******/ 	var parentJsonpFunction = oldJsonpFunction;
/******/
/******/
/******/ 	// add entry module to deferred list
/******/ 	deferredModules.push(["./www/js/source/reactpages/pages/bettingticketscenter/running/components/index.jsx","common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

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

/***/ "./www/js/source/reactpages/actions/dateRangeAction.js":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/actions/dateRangeAction.js ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setDateRange = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

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

/***/ }),

/***/ "./www/js/source/reactpages/actions/gridDataAction.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/actions/gridDataAction.js ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.getGridData = getGridData;
exports.setGridData = setGridData;

var _types = __webpack_require__(/*! @jsroot/actions/types.js */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _DataService = __webpack_require__(/*! @jsroot/lib/DataService.js */ "./www/js/source/reactpages/lib/DataService.js");

var _DataService2 = _interopRequireDefault(_DataService);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _loaderAction = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function getGridData(url, params) {
    return function (dispatch) {
        _DataService2.default.postData(url, params).then(function (response) {
            dispatch(setGridData(response.data));
        }).catch(function (error) {
            alert("Sorry, an error occurred while processing your request.");
            dispatch((0, _loaderAction.toggleLoader)(false));
        });
    };
}

function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}

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

/***/ "./www/js/source/reactpages/actions/reportTitleAction.js":
/*!***************************************************************!*\
  !*** ./www/js/source/reactpages/actions/reportTitleAction.js ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setReportTitle = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setReportTitle = exports.setReportTitle = function setReportTitle(title) {
    return {
        type: types.SET_REPORT_TITLE,
        data: title
    };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/requestParamAction.js":
/*!****************************************************************!*\
  !*** ./www/js/source/reactpages/actions/requestParamAction.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setRequestParam = undefined;

var _types = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setRequestParam = exports.setRequestParam = function setRequestParam(data) {
    return {
        type: types.SET_REQUEST_PARAM,
        data: data
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

/***/ "./www/js/source/reactpages/actions/usernameAction.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/actions/usernameAction.js ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setUsername = undefined;

var _types = __webpack_require__(/*! ./types.js */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setUsername = exports.setUsername = function setUsername(value) {
    return {
        type: types.SET_USERNAME,
        data: value
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

/***/ "./www/js/source/reactpages/components/auto-complete/AutoComplete.jsx":
/*!****************************************************************************!*\
  !*** ./www/js/source/reactpages/components/auto-complete/AutoComplete.jsx ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");

var _propTypes2 = _interopRequireDefault(_propTypes);

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

var _Overlay = __webpack_require__(/*! ../Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");

var _Overlay2 = _interopRequireDefault(_Overlay);

var _SuggestionList = __webpack_require__(/*! ./SuggestionList */ "./www/js/source/reactpages/components/auto-complete/SuggestionList.jsx");

var _SuggestionList2 = _interopRequireDefault(_SuggestionList);

var _timer = __webpack_require__(/*! ../utils/timer */ "./www/js/source/reactpages/components/utils/timer.js");

var _timer2 = _interopRequireDefault(_timer);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DELAY_TIME_MILLISECOND = 300;

var AutoComplete = function (_Component) {
    _inherits(AutoComplete, _Component);

    function AutoComplete(props) {
        _classCallCheck(this, AutoComplete);

        var _this = _possibleConstructorReturn(this, (AutoComplete.__proto__ || Object.getPrototypeOf(AutoComplete)).call(this, props));

        _this._onKeyDown = function (event) {
            if (!_this.isShowSuggestionList) {
                return;
            }

            var selectedIndex = _this.state.selectedIndex;
            switch (event.keyCode) {
                case 38:
                    //up
                    if (selectedIndex > 0) {
                        _this.setState({ selectedIndex: selectedIndex - 1 });
                    }
                    break;
                case 40:
                    //down
                    if (selectedIndex < _this.state.suggestions.length - 1) {
                        _this.setState({ selectedIndex: selectedIndex + 1 });
                    }
                    break;
                case 13:
                    //enter
                    if (selectedIndex > -1) {
                        var selectedItem = _this.state.suggestions[selectedIndex];
                        _this._onSelect(selectedItem);
                    }
                    break;
            }
        };

        _this._clearState = function () {
            _this.setState({ suggestions: null, selectedIndex: -1 });
        };

        _this._onSelect = function (selectedItem) {
            var onSelect = _this.props.onSelect;

            onSelect && onSelect(selectedItem);
            _this.input.value = (typeof selectedItem === 'undefined' ? 'undefined' : _typeof(selectedItem)) === "object" ? selectedItem.value : selectedItem;

            _this._clearState();
        };

        _this._onChange = function (event) {
            var value = (_this.input.value || "").trim();

            var onInput = _this.props.onInput;

            onInput && onInput(value);

            if (value.length < 3) {
                _this._clearState();
                return;
            }

            var cachedData = _this.cacheData[value];
            if (cachedData) {
                _this.setState({ suggestions: cachedData });
                return;
            }

            var url = _this.props.url;

            if (url) {
                _this._getDataByUrl(url, value);
            }
        };

        _this.state = {
            suggestions: null,
            selectedIndex: -1
        };

        _this.input = null;
        _this.cacheData = {};

        _this.onChange = _timer2.default.debounce(_this._onChange, DELAY_TIME_MILLISECOND).bind(_this);
        return _this;
    }

    _createClass(AutoComplete, [{
        key: '_setStateWithCaching',
        value: function _setStateWithCaching(value, suggestionList) {
            if (this.props.cache) {
                var cacheKey = value.toLowerCase();
                this.cacheData[cacheKey] = suggestionList;
            }

            this.setState({
                suggestions: suggestionList
            });
        }
    }, {
        key: '_getDataByUrl',
        value: function _getDataByUrl(url, value) {
            var _this2 = this;

            var _props = this.props,
                responseDataField = _props.responseDataField,
                queryParamName = _props.queryParamName;

            var apiUrl = url + '?' + queryParamName + '=' + value;

            _axios2.default.get(apiUrl, {
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            }).then(function (response) {
                var data = responseDataField ? response.data[responseDataField] : response.data;
                _this2._setStateWithCaching(value, data);
            });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this3 = this;

            return _react2.default.createElement(
                'div',
                { className: 'ac-container' },
                _react2.default.createElement('input', _extends({ type: 'text', className: 'input-text' }, this.props.htmlAttributes, {
                    onChange: this.onChange, onKeyDown: this._onKeyDown,
                    ref: function ref(element) {
                        return _this3.input = element;
                    } })),
                this.isShowSuggestionList && _react2.default.createElement(
                    _Overlay2.default,
                    { isOpening: true, onClose: this._clearState },
                    _react2.default.createElement(_SuggestionList2.default, { selectedIndex: this.state.selectedIndex,
                        width: this.input.clientWidth,
                        dataList: this.state.suggestions,
                        onSelect: this._onSelect })
                )
            );
        }
    }, {
        key: 'isShowSuggestionList',
        get: function get() {
            return this.state.suggestions && this.state.suggestions.length > 0;
        }
    }]);

    return AutoComplete;
}(_react.Component);

AutoComplete.propTypes = {
    url: _propTypes2.default.string,
    onSelect: _propTypes2.default.func,
    onInput: _propTypes2.default.func,
    responseDataField: _propTypes2.default.string,
    cache: _propTypes2.default.bool,
    queryParamName: _propTypes2.default.string,
    htmlAttributes: _propTypes2.default.object
};
AutoComplete.defaultProps = {
    url: null,
    responseDataField: "suggestions",
    cache: true,
    queryParamName: "query"
};
exports.default = AutoComplete;

/***/ }),

/***/ "./www/js/source/reactpages/components/auto-complete/SuggestionList.jsx":
/*!******************************************************************************!*\
  !*** ./www/js/source/reactpages/components/auto-complete/SuggestionList.jsx ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

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

var SuggestionList = function (_Component) {
    _inherits(SuggestionList, _Component);

    function SuggestionList(props) {
        _classCallCheck(this, SuggestionList);

        return _possibleConstructorReturn(this, (SuggestionList.__proto__ || Object.getPrototypeOf(SuggestionList)).call(this, props));
    }

    _createClass(SuggestionList, [{
        key: 'onSelect',
        value: function onSelect(selectedItem) {
            var onSelect = this.props.onSelect;


            onSelect && onSelect(selectedItem);
        }
    }, {
        key: '_renderDataList',
        value: function _renderDataList() {
            var _this2 = this;

            return this.props.dataList.map(function (item, index) {
                var className = index === _this2.props.selectedIndex ? "ac-item ac-item-active" : "ac-item";
                return _react2.default.createElement(
                    'li',
                    { key: 'ac-item-' + index, className: className, onClick: _this2.onSelect.bind(_this2, item) },
                    (typeof item === 'undefined' ? 'undefined' : _typeof(item)) === "object" ? item.value : item
                );
            });
        }
    }, {
        key: 'render',
        value: function render() {
            var style = { width: this.props.width };
            return _react2.default.createElement(
                'ul',
                { className: 'suggestion-list', style: style },
                this._renderDataList()
            );
        }
    }]);

    return SuggestionList;
}(_react.Component);

SuggestionList.propTypes = {
    dataList: _propTypes2.default.array.isRequired,
    onSelect: _propTypes2.default.func,
    width: _propTypes2.default.number,
    selectedIndex: _propTypes2.default.number
};
exports.default = SuggestionList;

/***/ }),

/***/ "./www/js/source/reactpages/components/auto-complete/index.jsx":
/*!*********************************************************************!*\
  !*** ./www/js/source/reactpages/components/auto-complete/index.jsx ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _AutoComplete = __webpack_require__(/*! ./AutoComplete */ "./www/js/source/reactpages/components/auto-complete/AutoComplete.jsx");

var _AutoComplete2 = _interopRequireDefault(_AutoComplete);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _AutoComplete2.default;

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
                isPaging && pageOption && _react2.default.createElement(_Pager2.default, { alwaysVisible: alwaysVisiblePager, pageOption: pageOption, onPaging: onPaging })
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

/***/ "./www/js/source/reactpages/components/popup/index.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/popup/index.jsx ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _Popup = __webpack_require__(/*! ./Popup */ "./www/js/source/reactpages/components/popup/Popup.jsx");

var _Popup2 = _interopRequireDefault(_Popup);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _Popup2.default;

/***/ }),

/***/ "./www/js/source/reactpages/components/utils/timer.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/components/utils/timer.js ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var DEFAULT_TIME_DELAY_MILLISECOND = 500;

var Timer = function () {
    function Timer() {
        _classCallCheck(this, Timer);
    }

    _createClass(Timer, null, [{
        key: "debounce",
        value: function debounce(func, wait) {
            var timeout = void 0;
            return function () {
                var context = this,
                    args = arguments;

                var later = function later() {
                    func.apply(context, args);
                };

                clearTimeout(timeout);
                timeout = setTimeout(later, wait || DEFAULT_TIME_DELAY_MILLISECOND);
            };
        }
    }]);

    return Timer;
}();

exports.default = Timer;

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

/***/ "./www/js/source/reactpages/lib/DataService.js":
/*!*****************************************************!*\
  !*** ./www/js/source/reactpages/lib/DataService.js ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var DataService = function () {
    function DataService() {
        _classCallCheck(this, DataService);
    }

    _createClass(DataService, null, [{
        key: 'getData',
        value: function getData(url, params) {
            return _axios2.default.get(url, { params: params });
        }
    }, {
        key: 'postData',
        value: function postData(url, params) {
            return _axios2.default.post(url, params, {
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    '__RequestVerificationToken': _site2.default.getXsrfToken()
                }
            });
        }
    }, {
        key: 'getFile',
        value: function getFile(url, params) {
            return _axios2.default.post(url, params, {
                responseType: 'blob',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    '__RequestVerificationToken': _site2.default.getXsrfToken()
                }
            });
        }
    }]);

    return DataService;
}();

exports.default = DataService;

/***/ }),

/***/ "./www/js/source/reactpages/lib/QueryString.js":
/*!*****************************************************!*\
  !*** ./www/js/source/reactpages/lib/QueryString.js ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var QueryString = function () {
    function QueryString() {
        _classCallCheck(this, QueryString);
    }

    _createClass(QueryString, null, [{
        key: '_parseQueryString',
        value: function _parseQueryString(queryString) {
            var args = queryString.split('&');
            var argsList = {};

            for (var i = 0; i < args.length; i++) {
                var arg = unescape(args[i]);
                if (arg.length > 0) {
                    if (arg.indexOf('=') === -1) {
                        argsList[arg.toLowerCase().trim()] = null;
                    } else {
                        var keyValuePair = arg.split('=');
                        argsList[keyValuePair[0].toLowerCase().trim()] = keyValuePair[1].trim();
                    }
                }
            }

            return argsList;
        }
    }, {
        key: '_getQueryString',
        value: function _getQueryString(url) {
            var index = url.indexOf("?");
            return index < 0 ? "" : url.substring(index + 1, url.length);
        }
    }, {
        key: '_getRawUrl',
        value: function _getRawUrl(url) {
            var index = url.indexOf("?");
            return index < 0 ? url : url.substring(0, index);
        }
    }, {
        key: 'setParam',
        value: function setParam(url, name, value) {
            var rawUrl = QueryString._getRawUrl(url);
            var queryString = QueryString._getQueryString(url);
            var args = QueryString._parseQueryString(queryString);

            if (!name) {
                return url;
            }

            // Update value
            var found = false;

            for (var key in args) {
                if (key && key.toLowerCase() === name.toLowerCase()) {

                    // Does not update data if value = undefined
                    if (value !== undefined) {
                        args[key] = value;
                    }
                    found = true;
                }
            }

            if (!found) {
                args[name.toLowerCase()] = value;
            }

            var queryArray = [];

            // Rebuild query string, ignore null value
            for (var key1 in args) {
                if (args[key1] != null && args[key1] !== undefined) {
                    queryArray.push(key1.toLowerCase() + "=" + args[key1]);
                }
            }

            return rawUrl + "?" + queryArray.join("&");
        }
    }, {
        key: 'setParams',
        value: function setParams(url, parameters) {
            for (var property in parameters) {
                if (parameters.hasOwnProperty(property)) {
                    url = QueryString.setParam(url, property, parameters[property]);
                }
            }

            return url;
        }
    }, {
        key: 'getParam',
        value: function getParam(url, name) {
            var queryString = QueryString._getQueryString(url);
            var args = QueryString._parseQueryString(queryString);

            for (var key in args) {
                if (key != null && key.toLowerCase() === name.toLowerCase()) {
                    return args[key];
                }
            }

            return null;
        }
    }]);

    return QueryString;
}();

exports.default = QueryString;

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

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/common/components/BaseDataGrid.jsx":
/*!************************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/common/components/BaseDataGrid.jsx ***!
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

var _fixedTableHeader = __webpack_require__(/*! @jsroot/components/fixed-table-header */ "./www/js/source/reactpages/components/fixed-table-header/index.jsx");

var _ReportTitle = __webpack_require__(/*! @jsroot/components/ReportTitle */ "./www/js/source/reactpages/components/ReportTitle.jsx");

var _ReportTitle2 = _interopRequireDefault(_ReportTitle);

var _popup = __webpack_require__(/*! @jsroot/components/popup */ "./www/js/source/reactpages/components/popup/index.jsx");

var _popup2 = _interopRequireDefault(_popup);

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _NoDataTitle = __webpack_require__(/*! @jsroot/components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");

var _NoDataTitle2 = _interopRequireDefault(_NoDataTitle);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DEFAULT_PAGE_SIZE = 50;

var BaseDataGrid = function (_Component) {
    _inherits(BaseDataGrid, _Component);

    function BaseDataGrid(props) {
        _classCallCheck(this, BaseDataGrid);

        var _this = _possibleConstructorReturn(this, (BaseDataGrid.__proto__ || Object.getPrototypeOf(BaseDataGrid)).call(this, props));

        _this.onPaging = function (index, size) {
            var param = _this.props.requestParam;

            param.PageIndex = index;
            param.PageSize = size;

            _this._getData(param);
        };

        return _this;
    }

    _createClass(BaseDataGrid, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            var param = this.props.requestParam;
            param.PageSize = DEFAULT_PAGE_SIZE;
            this.props.setRequestParam(param);
        }
    }, {
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            if (this.props.gridData !== prevProps.gridData) {
                this.props.toggleLoader(false);
            }
        }
    }, {
        key: '_getData',
        value: function _getData(param) {
            this.props.toggleLoader(true);
            this.props.getGridData(this.dataUrl, param);
            this.props.setRequestParam(param);
        }
    }, {
        key: '_sort',
        value: function _sort(sortBy) {
            var param = this.props.requestParam;

            if (!param.SortBy || param.SortBy != sortBy) {
                param.SortBy = sortBy;
                param.SortOrder = 'asc';
            } else {
                param.SortOrder = param.SortOrder == 'asc' ? 'desc' : 'asc';
            }

            this._getData(param);
        }
    }, {
        key: '_buildHeader',
        value: function _buildHeader() {
            // This method should be override in child class.
            return [];
        }
    }, {
        key: '_buildBody',
        value: function _buildBody() {
            var data = this.props.gridData.Data;
            var rows = [];

            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                rows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: 'product-' + i },
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { className: 'center-align' },
                        item.Index
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        null,
                        item.ExternalId
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        null,
                        item.SystemId
                    ),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'center-align', dangerouslySetInnerHTML: { __html: item.TransTime } }),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'right-align', dangerouslySetInnerHTML: { __html: item.Choice } }),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'right-align', dangerouslySetInnerHTML: { __html: item.Odds } }),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'right-align', dangerouslySetInnerHTML: { __html: item.Stake } }),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'right-align', dangerouslySetInnerHTML: { __html: item.WinLoss } }),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'right-align', dangerouslySetInnerHTML: { __html: item.LicenseeWL } }),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'center-align', dangerouslySetInnerHTML: { __html: item.Status } }),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { className: 'center-align' },
                        item.Platform
                    ),
                    _react2.default.createElement(_fixedTableHeader.Cell, { className: 'center-align', dangerouslySetInnerHTML: { __html: item.Live } })
                ));
            }

            return rows;
        }
    }, {
        key: '_renderNodata',
        value: function _renderNodata() {
            return _react2.default.createElement(
                'div',
                { className: 'result-content' },
                _react2.default.createElement(_ReportTitle2.default, { content: this.props.reportTitle }),
                _react2.default.createElement(_NoDataTitle2.default, null)
            );
        }
    }, {
        key: 'render',
        value: function render() {
            if (!this.props.gridData || !this.props.gridData.Data) {
                return null;
            }

            var Data = this.props.gridData.Data;


            if (!Data.length) {
                return this._renderNodata();
            }

            var param = this.props.requestParam;
            var pageOption = {
                PageIndex: param.PageIndex,
                PageSize: param.PageSize,
                TotalItem: this.props.gridData.TotalRows,
                PageList: [DEFAULT_PAGE_SIZE, 100, 200, 300, 400, 500, 1000, 2000]
            };

            return _react2.default.createElement(
                'div',
                { className: 'betlist-container result-content' },
                _react2.default.createElement(_ReportTitle2.default, { content: this.props.reportTitle }),
                _react2.default.createElement(_fixedTableHeader.Table, {
                    minWidth: 960,
                    autoWidth: true,
                    bodyHeight: 300,
                    header: this._buildHeader(),
                    body: this._buildBody(),
                    footer: this._buildFooter && this._buildFooter(),
                    isPaging: true,
                    pageOption: pageOption,
                    onPaging: this.onPaging,
                    alwaysVisiblePager: true
                })
            );
        }
    }, {
        key: 'dataUrl',
        get: function get() {
            // This property should be override in child class.
        }
    }]);

    return BaseDataGrid;
}(_react.Component);

exports.default = BaseDataGrid;

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/common/components/PageLayout.jsx":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/common/components/PageLayout.jsx ***!
  \**********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.PageLayout = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(/*! react */ "./node_modules/react/index.js");

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _axios = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");

var _axios2 = _interopRequireDefault(_axios);

var _action = __webpack_require__(/*! ./action.js */ "./www/js/source/reactpages/pages/bettingticketscenter/common/components/action.js");

var _AppLayout = __webpack_require__(/*! @jsroot/layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");

var _AppLayout2 = _interopRequireDefault(_AppLayout);

var _Loader = __webpack_require__(/*! @jsroot/components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");

var _Loader2 = _interopRequireDefault(_Loader);

var _reactSimpleTab = __webpack_require__(/*! @9softstudio/react-simple-tab */ "./node_modules/@9softstudio/react-simple-tab/dist/index.js");

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _BetList = __webpack_require__(/*! ../lib/BetList */ "./www/js/source/reactpages/pages/bettingticketscenter/common/lib/BetList.js");

var _BetList2 = _interopRequireDefault(_BetList);

var _popup = __webpack_require__(/*! @jsroot/components/popup */ "./www/js/source/reactpages/components/popup/index.jsx");

var _popup2 = _interopRequireDefault(_popup);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var PageLayout = exports.PageLayout = function (_Component) {
    _inherits(PageLayout, _Component);

    function PageLayout(props) {
        _classCallCheck(this, PageLayout);

        var _this = _possibleConstructorReturn(this, (PageLayout.__proto__ || Object.getPrototypeOf(PageLayout)).call(this, props));

        _this.state = {
            isShowPopup: false,
            popupUrl: null,
            popupTitle: null
        };
        return _this;
    }

    _createClass(PageLayout, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            var _this2 = this;

            window.ViewResult = function () {
                for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
                    args[_key] = arguments[_key];
                }

                var url = _BetList2.default.getResultUrl.apply(_BetList2.default, args);
                _this2._openPopup(url, _langHelper2.default.getSingleResource("Result"));
            };

            window.showMP = function (transId) {
                var divEvent = document.querySelector('#divEvent_' + transId);
                divEvent.classList.add("mx-parlay"); //to make line-through for void-parlay-ticket
                _this2._showMixParlayDetail(transId, divEvent);
            };

            window.showCombMP = function (transId, index) {
                var divEvent = document.querySelector('#divEvent_' + transId + "" + index);
                divEvent.classList.add("mx-parlay"); //to make line-through for void-parlay-ticket
                _this2._showMixParlayDetail(transId, divEvent);
            };

            // expand choice content
            window.showNewCombMP = function () {
                for (var _len2 = arguments.length, args = Array(_len2), _key2 = 0; _key2 < _len2; _key2++) {
                    args[_key2] = arguments[_key2];
                }

                _this2._showNewParlayDetail.apply(_this2, args);
            };

            // show detail popup (in choice column)
            window.showCombMPDetail = function (transId, refNo, betId, custId, betTeam, baseCurrency) {
                var url = _BetList2.default.getParlayDetailUrl(transId, refNo, betId, custId, betTeam, baseCurrency);
                _this2._openPopup(url, betTeam);
            };

            window.ViewCashOutHistory = function (transId, refNo, betType, betdaqId) {
                var _BetList$getCashOutHi = _BetList2.default.getCashOutHistoryPopupSetting(_this2.props.requestParam.BaseCurrency, transId, refNo, betType, betdaqId),
                    url = _BetList$getCashOutHi.url,
                    isCashOut = _BetList$getCashOutHi.isCashOut;

                var title = _langHelper2.default.getSingleResource(isCashOut ? "CashOutDetails" : "BoughtDetails");
                _this2._openPopup(url, title);
            };

            window.ShowBonusDetails = function (refCode, transId) {
                var url = _BetList2.default.getBonusDetailUrl(refCode, transId, _this2.props.requestParam.BaseCurrency);

                var title = _langHelper2.default.getSingleResource("TransactionDetail");
                _this2._openPopup(url, title);
            };
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            window.showMP = undefined;
            window.showCombMP = undefined;
            window.ViewResult = undefined;
            window.showNewCombMP = undefined;
            window.showCombMPDetail = undefined;
            window.ViewCashOutHistory = undefined;
            window.ShowBonusDetails = undefined;
        }
    }, {
        key: '_showMixParlayDetail',
        value: function _showMixParlayDetail(transId, divEvent) {
            var _this3 = this;

            if (divEvent.style.display === 'none') {
                this.props.toggleLoader(true);
                var url = _site2.default.resolveClientUrl('api/MixParlayBetList/Detail?transId=' + transId);
                _axios2.default.get(url).then(function (result) {
                    var choice = "";
                    var length = result.data.length;
                    for (var i = 0; i < length; i++) {
                        choice += result.data[i].Choice;
                        if (i < length - 1) {
                            choice += "<div class='line'></div>";
                        }
                    }

                    divEvent.innerHTML = choice;
                    divEvent.style.display = 'block';
                    _this3.props.toggleLoader(false);
                }).catch(function (error) {
                    console.log(error);
                });
            } else {
                divEvent.style.display = 'none';
                divEvent.innerHTML = "";
            }
        }
    }, {
        key: '_showNewParlayDetail',
        value: function _showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
            var _this4 = this;

            var divEvent = document.querySelector('#divEvent_' + transId + "" + transIndex);
            var nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);
            divEvent.classList.add("mx-parlay"); //to make line-through for void-parlay-ticket

            if (divEvent.style.display == "none") {
                var url = _BetList2.default.getChoiceParlayDetailUrl(transId, refNo, handicap1, handicap2, betcheck, betteam, custId);
                this.props.toggleLoader(true);
                _axios2.default.get(url).then(function (result) {
                    var choice = _BetList2.default.buildChoiceParlayDetail(result.data);
                    divEvent.innerHTML = choice;
                    divEvent.style.display = 'block';
                    nameDiv.style.display = 'none';
                    _this4.props.toggleLoader(false);
                }).catch(function (error) {
                    console.log(error);
                });
            } else {
                divEvent.style.display = 'none';
                divEvent.innerHTML = "";
                nameDiv.style.display = 'block';
            }
        }
    }, {
        key: '_openPopup',
        value: function _openPopup(url, title) {
            this.setState({ isShowPopup: true, popupUrl: url, popupTitle: title });
        }
    }, {
        key: '_closePopup',
        value: function _closePopup() {
            this.setState({ isShowPopup: false });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this5 = this;

            return _react2.default.createElement(
                _AppLayout2.default,
                null,
                _react2.default.createElement(_Loader2.default, { isLoading: this.props.isLoading }),
                _react2.default.createElement(
                    _reactSimpleTab.Tabs,
                    null,
                    _react2.default.createElement(_reactSimpleTab.Tab, { isActive: this.props.selectedTabIndex == 0, label: _langHelper2.default.getSingleResource("MultiCustomers"),
                        url: _site2.default.resolveClientUrl("BettingTicketCenter/MultipleCustomers") }),
                    _react2.default.createElement(_reactSimpleTab.Tab, { isActive: this.props.selectedTabIndex == 1, label: _langHelper2.default.getSingleResource("SingleCustomer"),
                        url: _site2.default.resolveClientUrl("BettingTicketCenter/SingleCustomer") }),
                    _react2.default.createElement(_reactSimpleTab.Tab, { isActive: this.props.selectedTabIndex == 2, label: _langHelper2.default.getSingleResource("RefNo"),
                        url: _site2.default.resolveClientUrl("BettingTicketCenter/RefNo") }),
                    _react2.default.createElement(_reactSimpleTab.Tab, { isActive: this.props.selectedTabIndex == 3, label: _langHelper2.default.getSingleResource("Running"),
                        url: _site2.default.resolveClientUrl("BettingTicketCenter/Running") })
                ),
                _react2.default.createElement(
                    'div',
                    null,
                    this.props.children
                ),
                this.state.isShowPopup && _react2.default.createElement(_popup2.default, { isShow: true,
                    iframeUrl: this.state.popupUrl,
                    title: this.state.popupTitle,
                    width: 900, height: 600,
                    onClose: function onClose() {
                        return _this5._closePopup();
                    }
                })
            );
        }
    }]);

    return PageLayout;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_action.ActionCreators, dispatch);
};

function mapStateToProps(state) {
    var requestParam = state.requestParam;

    return Object.assign({}, { requestParam: requestParam });
}

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(PageLayout);

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/common/components/action.js":
/*!*****************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/common/components/action.js ***!
  \*****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _loaderAction = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");

var loaderAction = _interopRequireWildcard(_loaderAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, loaderAction);

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/common/constants/apiUrl.js":
/*!****************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/common/constants/apiUrl.js ***!
  \****************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var ApiUrl = {
    CUSTOMER_SEARCH_URL: _site2.default.resolveClientUrl("SearchCustomerAPI/SearchCustomer"),
    RUNNING_DATA_URL: _site2.default.resolveClientUrl('api/BettingTicketCenter/GetByRunning'),
    REFNO_DATA_URL: _site2.default.resolveClientUrl('api/BettingTicketCenter/GetByRefNo'),
    SINGLECUSTOMER_DATA_URL: _site2.default.resolveClientUrl('api/BettingTicketCenter/GetBySingleCustomer'),
    MULTICUSTOMER_DATA_URL: _site2.default.resolveClientUrl('api/BettingTicketCenterMultiCustomer/GetData')
};
exports.default = ApiUrl;

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/common/lib/BetList.js":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/common/lib/BetList.js ***!
  \***********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _QueryString = __webpack_require__(/*! @jsroot/lib/QueryString */ "./www/js/source/reactpages/lib/QueryString.js");

var _QueryString2 = _interopRequireDefault(_QueryString);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var BETTYPE_CASHOUT = 911;
var BETTYPE_MIXPARLAY = 9,
    BETTYPE_SYSTEMPARLAY = 29,
    BETTYPE_MIXPARLAY_BETRADAR = 2799;
var MIN_BETTYPE_FASTMARKET = 221;
var MAX_BETTYPE_FASTMARKET = 227;

var BetList = function () {
    function BetList() {
        _classCallCheck(this, BetList);
    }

    _createClass(BetList, [{
        key: '_isFastMarket',
        value: function _isFastMarket(bettype) {
            return bettype >= MIN_BETTYPE_FASTMARKET && bettype <= MAX_BETTYPE_FASTMARKET;
        }
    }, {
        key: '_getMixParlayResultUrl',
        value: function _getMixParlayResultUrl(refno, winlostdate, bettype) {
            var url = _site2.default.resolveClientUrl('ResultReport/ResultPopup');
            url = _QueryString2.default.setParam(url, "refno", refno);
            url = _QueryString2.default.setParam(url, "winlossdate", winlostdate);
            url = _QueryString2.default.setParam(url, "bettype", bettype);
            url = _QueryString2.default.setParam(url, "matchid", 1);

            return url;
        }
    }, {
        key: '_getSportBookResultUrl',
        value: function _getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
            var url = _site2.default.resolveClientUrl('ResultReport/ResultPopup');
            url = _QueryString2.default.setParam(url, "matchid", matchid);
            url = _QueryString2.default.setParam(url, "bettype", bettype);
            url = _QueryString2.default.setParam(url, "raceno", raceNo);
            url = _QueryString2.default.setParam(url, "sporttype", sporttype);
            url = _QueryString2.default.setParam(url, "isoutright", isOutright);

            if (this._isFastMarket(bettype)) {
                url = _QueryString2.default.setParam(url, "handicap1", extension1);
                url = _QueryString2.default.setParam(url, "handicap2", extension2);
            } else {
                url = _QueryString2.default.setParam(url, "betid", extension1);
            }

            return url;
        }
    }, {
        key: 'getResultUrl',
        value: function getResultUrl(matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid) {
            bettype = Number(bettype);

            var isParlay = bettype === BETTYPE_MIXPARLAY || bettype === BETTYPE_SYSTEMPARLAY || bettype === BETTYPE_MIXPARLAY_BETRADAR;
            var url = isParlay ? this._getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype) : this._getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

            return url;
        }
    }, {
        key: 'getChoiceParlayDetailUrl',
        value: function getChoiceParlayDetailUrl(transId, refNo, handicap1, handicap2, betcheck, betteam, custId) {
            var url = _site2.default.resolveClientUrl('api/MixParlayBetList/NewParlayDetail');
            url = _QueryString2.default.setParam(url, "transId", transId);
            url = _QueryString2.default.setParam(url, "refNo", refNo);
            url = _QueryString2.default.setParam(url, "handicap1", handicap1);
            url = _QueryString2.default.setParam(url, "handicap2", handicap2);
            url = _QueryString2.default.setParam(url, "betcheck", betcheck);
            url = _QueryString2.default.setParam(url, "betteam", betteam);
            url = _QueryString2.default.setParam(url, "custId", custId);

            return url;
        }
    }, {
        key: 'buildChoiceParlayDetail',
        value: function buildChoiceParlayDetail(response) {
            var choice = "";
            var length = response.Data === null ? 0 : response.Data.length;
            if (length > 0) {
                for (var i = 0; i < response.Texts.length; i++) {
                    choice += response.Texts[i];
                }

                choice += "<div class='line'></div>";

                for (var _i = 0; _i < response.Data.length; _i++) {
                    var item = response.Data[_i];
                    if (_i === length - 1) {
                        choice += item.Choice;
                    } else {
                        choice += item.Choice + "<div class='line'></div>";
                    }
                }
            }

            return choice;
        }
    }, {
        key: 'getParlayDetailUrl',
        value: function getParlayDetailUrl(transId, refNo, betId, custId, betTeam, baseCurrency) {
            if (Number(transId) === 0) {
                return;
            }

            var url = _site2.default.resolveClientUrl('SystemParlayBetList/Detail');
            url = _QueryString2.default.setParam(url, "refno", refNo);
            url = _QueryString2.default.setParam(url, "betteam", betTeam);
            url = _QueryString2.default.setParam(url, "basecurrency", baseCurrency);

            return url;
        }
    }, {
        key: 'getCashOutHistoryPopupSetting',
        value: function getCashOutHistoryPopupSetting(baseCurrency, transId, refNo, betType, betdaqId) {
            var isCashOut = betdaqId < 1 || betType === BETTYPE_CASHOUT;
            var url = _site2.default.resolveClientUrl('CashOutBetList/Detail');
            url = _QueryString2.default.setParam(url, "transId", transId);
            url = _QueryString2.default.setParam(url, "refNo", refNo);
            url = _QueryString2.default.setParam(url, "betType", betType);
            url = _QueryString2.default.setParam(url, "betdaqId", betdaqId);
            url = _QueryString2.default.setParam(url, "BaseCurrency", baseCurrency);
            url = _QueryString2.default.setParam(url, "isCashout", isCashOut);
            return {
                url: url,
                isCashOut: isCashOut
            };
        }
    }, {
        key: 'getBonusDetailUrl',
        value: function getBonusDetailUrl(refCode, transId, baseCurrency) {
            var url = _site2.default.resolveClientUrl('BonusBetList/Detail');
            url = _QueryString2.default.setParam(url, "RefCode", refCode);
            url = _QueryString2.default.setParam(url, "TransId", transId);
            url = _QueryString2.default.setParam(url, "BaseCurrency", baseCurrency);

            return url;
        }
    }]);

    return BetList;
}();

exports.default = new BetList();

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/common/lib/Service.js":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/common/lib/Service.js ***!
  \***********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var HeaderText = {
    Index: "#",
    Platform: _langHelper2.default.getSingleResource("Platform"),
    ExternalId: _langHelper2.default.getSingleResource("ExternalId"),
    SystemId: _langHelper2.default.getSingleResource("SystemId"),
    TransTime: _langHelper2.default.getSingleResource("TransTime"),
    Choice: _langHelper2.default.getSingleResource("Choice"),
    Odds: _langHelper2.default.getSingleResource("Odds"),
    Stake: _langHelper2.default.getSingleResource("Stake"),
    WinLoss: _langHelper2.default.getSingleResource("WinLoss"),
    LicenseeWL: _langHelper2.default.getSingleResource("LicenseeWL"),
    Comm: _langHelper2.default.getSingleResource("Comm"),
    Status: _langHelper2.default.getSingleResource("Status"),
    Live: _langHelper2.default.getSingleResource("Live")
};

var Service = function () {
    function Service() {
        _classCallCheck(this, Service);
    }

    _createClass(Service, null, [{
        key: 'getHeaderText',
        value: function getHeaderText() {
            return HeaderText;
        }
    }, {
        key: 'getTitle',
        value: function getTitle(fromDate, toDate, currencyName, baseCurrency) {
            var dateRangeText = fromDate && toDate ? ', ' + _site2.default.convertDateToString(fromDate) + ' \u2192 ' + _site2.default.convertDateToString(toDate) : '';
            var currencyNameText = currencyName ? ', ' + _langHelper2.default.getSingleResource("Currency") + ': ' + currencyName : '';
            var baseCurrencyText = baseCurrency ? ', ' + _langHelper2.default.getSingleResource("BaseCurrency") + ': ' + baseCurrency : '';

            return _langHelper2.default.getSingleResource("BettingTicketCenter") + dateRangeText + currencyNameText + baseCurrencyText;
        }
    }]);

    return Service;
}();

exports.default = Service;

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/running/actions/index.js":
/*!**************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/running/actions/index.js ***!
  \**************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _gridDataAction = __webpack_require__(/*! @jsroot/actions/gridDataAction */ "./www/js/source/reactpages/actions/gridDataAction.js");

var gridDataAction = _interopRequireWildcard(_gridDataAction);

var _usernameAction = __webpack_require__(/*! @jsroot/actions/usernameAction */ "./www/js/source/reactpages/actions/usernameAction.js");

var usernameAction = _interopRequireWildcard(_usernameAction);

var _productAction = __webpack_require__(/*! @jsroot/actions/productAction */ "./www/js/source/reactpages/actions/productAction.js");

var productAction = _interopRequireWildcard(_productAction);

var _currencyAction = __webpack_require__(/*! @jsroot/actions/currencyAction */ "./www/js/source/reactpages/actions/currencyAction.js");

var currencyAction = _interopRequireWildcard(_currencyAction);

var _baseCurrencyAction = __webpack_require__(/*! @jsroot/actions/baseCurrencyAction */ "./www/js/source/reactpages/actions/baseCurrencyAction.js");

var baseCurrencyAction = _interopRequireWildcard(_baseCurrencyAction);

var _loaderAction = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");

var loaderAction = _interopRequireWildcard(_loaderAction);

var _dateRangeAction = __webpack_require__(/*! @jsroot/actions/dateRangeAction */ "./www/js/source/reactpages/actions/dateRangeAction.js");

var dateRangeAction = _interopRequireWildcard(_dateRangeAction);

var _requestParamAction = __webpack_require__(/*! @jsroot/actions/requestParamAction */ "./www/js/source/reactpages/actions/requestParamAction.js");

var requestParamAction = _interopRequireWildcard(_requestParamAction);

var _reportTitleAction = __webpack_require__(/*! @jsroot/actions/reportTitleAction */ "./www/js/source/reactpages/actions/reportTitleAction.js");

var reportTitleAction = _interopRequireWildcard(_reportTitleAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, gridDataAction, usernameAction, productAction, currencyAction, baseCurrencyAction, loaderAction, dateRangeAction, requestParamAction, reportTitleAction);

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/AppContainer.jsx":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/running/components/AppContainer.jsx ***!
  \*************************************************************************************************/
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

var _DataGrid = __webpack_require__(/*! ./DataGrid */ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/DataGrid.jsx");

var _DataGrid2 = _interopRequireDefault(_DataGrid);

var _FormFilter = __webpack_require__(/*! ./FormFilter */ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/FormFilter.jsx");

var _FormFilter2 = _interopRequireDefault(_FormFilter);

var _PageLayout = __webpack_require__(/*! ../../common/components/PageLayout */ "./www/js/source/reactpages/pages/bettingticketscenter/common/components/PageLayout.jsx");

var _PageLayout2 = _interopRequireDefault(_PageLayout);

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
                _PageLayout2.default,
                { selectedTabIndex: '3', isLoading: this.props.isLoading },
                _react2.default.createElement(_FormFilter2.default, null),
                _react2.default.createElement(_DataGrid2.default, null)
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

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/DataGrid.jsx":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/running/components/DataGrid.jsx ***!
  \*********************************************************************************************/
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

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _actions = __webpack_require__(/*! ../actions */ "./www/js/source/reactpages/pages/bettingticketscenter/running/actions/index.js");

var _fixedTableHeader = __webpack_require__(/*! @jsroot/components/fixed-table-header */ "./www/js/source/reactpages/components/fixed-table-header/index.jsx");

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _BaseDataGrid2 = __webpack_require__(/*! ../../common/components/BaseDataGrid */ "./www/js/source/reactpages/pages/bettingticketscenter/common/components/BaseDataGrid.jsx");

var _BaseDataGrid3 = _interopRequireDefault(_BaseDataGrid2);

var _Service = __webpack_require__(/*! ../../common/lib/Service */ "./www/js/source/reactpages/pages/bettingticketscenter/common/lib/Service.js");

var _Service2 = _interopRequireDefault(_Service);

var _apiUrl = __webpack_require__(/*! ../../common/constants/apiUrl */ "./www/js/source/reactpages/pages/bettingticketscenter/common/constants/apiUrl.js");

var _apiUrl2 = _interopRequireDefault(_apiUrl);

var _AppData = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");

var _AppData2 = _interopRequireDefault(_AppData);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DataGrid = exports.DataGrid = function (_BaseDataGrid) {
    _inherits(DataGrid, _BaseDataGrid);

    function DataGrid(props) {
        _classCallCheck(this, DataGrid);

        return _possibleConstructorReturn(this, (DataGrid.__proto__ || Object.getPrototypeOf(DataGrid)).call(this, props));
    }

    _createClass(DataGrid, [{
        key: '_buildFooter',
        value: function _buildFooter() {
            var item = this.props.gridData.Total;
            var colSpan = 6;
            return [_react2.default.createElement(
                _fixedTableHeader.Row,
                { className: 'fixed-table-grant-total', key: 1 },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { className: 'right-align', colSpan: colSpan },
                    _langHelper2.default.getSingleResource("Total")
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { className: 'right-align' },
                    item.SumStake
                ),
                _react2.default.createElement(_fixedTableHeader.Cell, { className: 'right-align', colSpan: 5 })
            )];
        }
    }, {
        key: '_buildHeader',
        value: function _buildHeader() {
            var _this2 = this;

            var headerText = _Service2.default.getHeaderText();
            var licenseeWLText = headerText.LicenseeWL + '/' + headerText.Comm;
            var winlossText = _AppData2.default.isAllowedCustomerComm ? headerText.WinLoss + '/' + headerText.Comm : headerText.WinLoss;

            var _props$requestParam = this.props.requestParam,
                SortBy = _props$requestParam.SortBy,
                SortOrder = _props$requestParam.SortOrder;


            return _react2.default.createElement(
                _fixedTableHeader.Row,
                { style: { height: 32 } },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 40, header: true, title: headerText.Index },
                    headerText.Index
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 120, header: true, title: headerText.ExternalId },
                    headerText.ExternalId
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 120, header: true, title: headerText.SystemId },
                    headerText.SystemId
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 150, header: true, title: headerText.TransTime },
                    headerText.TransTime
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 400, header: true, title: headerText.Choice },
                    headerText.Choice
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 70, header: true, title: headerText.Odds },
                    headerText.Odds
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 100, header: true, title: headerText.Stake, sortable: true, sorting: SortBy == 'Stake', asc: SortOrder == 'asc', onClick: function onClick() {
                            return _this2._sort('Stake');
                        } },
                    headerText.Stake
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 100, header: true, title: winlossText },
                    winlossText
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 100, header: true, title: licenseeWLText },
                    licenseeWLText
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 80, header: true, title: headerText.Status },
                    headerText.Status
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 80, header: true, title: headerText.Platform },
                    headerText.Platform
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 80, header: true, title: headerText.Live },
                    headerText.Live
                )
            );
        }
    }, {
        key: 'dataUrl',
        get: function get() {
            return _apiUrl2.default.RUNNING_DATA_URL;
        }
    }]);

    return DataGrid;
}(_BaseDataGrid3.default);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_actions.ActionCreators, dispatch);
}
function mapStateToProps(state) {
    var requestParam = state.requestParam,
        gridData = state.gridData,
        reportTitle = state.reportTitle;

    return Object.assign({}, { requestParam: requestParam, gridData: gridData, reportTitle: reportTitle });
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(DataGrid);

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/FormFilter.jsx":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/running/components/FormFilter.jsx ***!
  \***********************************************************************************************/
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

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _reactSelect = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");

var _reactSelect2 = _interopRequireDefault(_reactSelect);

var _reactMultiselect = __webpack_require__(/*! @9softstudio/react-multiselect */ "./node_modules/@9softstudio/react-multiselect/dist/index.js");

var _reactMultiselect2 = _interopRequireDefault(_reactMultiselect);

var _index = __webpack_require__(/*! @jsroot/components/date-range/index */ "./www/js/source/reactpages/components/date-range/index.js");

var _autoComplete = __webpack_require__(/*! @jsroot/components/auto-complete */ "./www/js/source/reactpages/components/auto-complete/index.jsx");

var _autoComplete2 = _interopRequireDefault(_autoComplete);

var _index2 = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/pages/bettingticketscenter/running/actions/index.js");

var _langHelper = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");

var _site2 = _interopRequireDefault(_site);

var _AppData = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");

var _AppData2 = _interopRequireDefault(_AppData);

var _DataService = __webpack_require__(/*! @jsroot/lib/DataService.js */ "./www/js/source/reactpages/lib/DataService.js");

var _DataService2 = _interopRequireDefault(_DataService);

var _apiUrl = __webpack_require__(/*! ../../common/constants/apiUrl */ "./www/js/source/reactpages/pages/bettingticketscenter/common/constants/apiUrl.js");

var _apiUrl2 = _interopRequireDefault(_apiUrl);

var _Service = __webpack_require__(/*! ../../common/lib/Service */ "./www/js/source/reactpages/pages/bettingticketscenter/common/lib/Service.js");

var _Service2 = _interopRequireDefault(_Service);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

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
            var params = _this._getParams();
            if (!params) {
                return;
            }

            _this.props.setReportTitle(_Service2.default.getTitle(params.FromDate, params.ToDate, params.CurrencyName, params.BaseCurrency));
            _this.props.setRequestParam(params);
            _this.props.toggleLoader(true);
            _this.props.getGridData(_apiUrl2.default.RUNNING_DATA_URL, params);
        };

        _this.productChangeHandler = function (currentItem, selectedItemsKey) {
            _this.props.setSelectedProduct(selectedItemsKey);
        };

        _this.applyDateRangeHandler = function (date) {
            var isValidMonthRange = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : true;

            if (isValidMonthRange) {
                _this.props.setDateRange(date);
            } else {
                var message = _langHelper2.default.getSingleResource("DateRangeExceedMsg").replace("{0}", _AppData2.default.dateConstraint.limitedMonthRange);
                alert(message);
            }
        };

        _this.onChangeCurrency = function (currency) {
            var currencyName = currency.label;
            var baseCurrency = currencyName;

            if (currency.value == 0) {
                _this.baseCurrencyList = _AppData2.default.baseCurrencyList;
                baseCurrency = _this.baseCurrencyList.find(function (x) {
                    return x.label === _this.props.selectedBaseCurrency;
                }) ? _this.props.selectedBaseCurrency : _this.defaultBaseCurrency;
            } else if (!_this.baseCurrencyList.find(function (x) {
                return x.label === currencyName;
            })) {
                _this.baseCurrencyList = _AppData2.default.baseCurrencyList.slice(0);
                _this.baseCurrencyList.push({ value: currencyName, label: currencyName });
            }

            _this.props.setSelectedCurrency(currency);
            _this.props.setSelectedBaseCurrency(baseCurrency);
        };

        _this.exportExcel = function (event) {
            event.preventDefault();

            var params = _this._getParams();
            if (!params) {
                return;
            }

            params.IsExport = true;
            _this.props.toggleLoader(true);
            _DataService2.default.getFile(_apiUrl2.default.RUNNING_DATA_URL, params).then(function (response) {
                var NoContentStatusCode = 204;

                if (response.status === NoContentStatusCode) {
                    alert(_langHelper2.default.getSingleResource("NoInfo"));
                } else {
                    _site2.default.downloadFile(response.data, 'BettingTicketsCenter_Running.xls');
                }
                _this.props.toggleLoader(false);
            });
        };

        _this.baseCurrencyList = _AppData2.default.baseCurrencyList;
        _this.defaultBaseCurrency = props.selectedBaseCurrency;
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
            var _props$requestParam = this.props.requestParam,
                PageSize = _props$requestParam.PageSize,
                SortBy = _props$requestParam.SortBy,
                SortOrder = _props$requestParam.SortOrder;


            return {
                PageSize: PageSize,
                PageIndex: 1,
                FromDate: _site2.default.convertDateToString(fromDate),
                ToDate: _site2.default.convertDateToString(toDate),
                ProductCodeList: this.props.selectedProduct,
                Username: this.props.username,
                CurrencyId: this.props.selectedCurrency.value,
                CurrencyName: this.props.selectedCurrency.label,
                BaseCurrency: this.props.selectedBaseCurrency,
                SortBy: SortBy,
                SortOrder: SortOrder
            };
        }
    }, {
        key: '_renderDateRange',
        value: function _renderDateRange() {
            var dateRange = this.props.dateRange;
            var dateConstraint = _AppData2.default.dateConstraint;

            return _react2.default.createElement(_index.DateRange, { startDate: dateRange.fromDate, endDate: dateRange.toDate,
                minDate: dateConstraint.minDate, maxDate: dateConstraint.maxDate,
                today: dateRange.toDate, limitedMonthRange: dateConstraint.limitedMonthRange,
                onChange: this.applyDateRangeHandler });
        }
    }, {
        key: '_renderProductSelectList',
        value: function _renderProductSelectList() {
            var productOptions = _AppData2.default.productOptions;

            return productOptions && productOptions.length ? _react2.default.createElement(_reactMultiselect2.default, { id: 'ProductList',
                dataSource: productOptions,
                keyField: "Code",
                valueField: "Name",
                statusField: "Status",
                optionAllLabel: _langHelper2.default.getSingleResource("All"),
                noneSelectedLabel: _langHelper2.default.getSingleResource("SelectOptions"),
                onChange: this.productChangeHandler }) : null;
        }
    }, {
        key: '_renderCurrencySelectList',
        value: function _renderCurrencySelectList() {
            return _react2.default.createElement(_reactSelect2.default, { instanceId: 'ddlCurrency',
                value: this.props.selectedCurrency.value,
                options: _AppData2.default.currencyList,
                onChange: this.onChangeCurrency });
        }
    }, {
        key: '_renderBaseCurrencySelectList',
        value: function _renderBaseCurrencySelectList() {
            var _this2 = this;

            var baseCurrencyList = this.baseCurrencyList;

            return baseCurrencyList && baseCurrencyList.length && _react2.default.createElement(_reactSelect2.default, { instanceId: 'ddlBased',
                value: this.props.selectedBaseCurrency,
                options: baseCurrencyList,
                onChange: function onChange(item) {
                    return _this2.props.setSelectedBaseCurrency(item.value);
                } });
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
                        { className: 'col-12 col-md-6 col-sm-6 col-lg-4 col-xl-3' },
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
                        { className: 'col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2' },
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
                        { className: 'col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2' },
                        _react2.default.createElement(
                            'div',
                            { className: 'form-group' },
                            _react2.default.createElement(
                                'label',
                                { className: 'label-text' },
                                _langHelper2.default.getSingleResource("Username")
                            ),
                            _react2.default.createElement(_autoComplete2.default, { url: _apiUrl2.default.CUSTOMER_SEARCH_URL,
                                onSelect: function onSelect(value) {
                                    return _this3.props.setUsername(value);
                                },
                                onInput: function onInput(value) {
                                    return _this3.props.setUsername(value);
                                },
                                htmlAttributes: { placeholder: _langHelper2.default.getSingleResource("ExternalIdOrSystemId") } })
                        )
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2' },
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
                        { className: 'col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2' },
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
                    ),
                    _react2.default.createElement(
                        'div',
                        { className: 'col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1' },
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
            return !!this.props.selectedProduct;
        }
    }]);

    return FormFilter;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_index2.ActionCreators, dispatch);
}
function mapStateToProps(state) {
    var requestParam = state.requestParam,
        dateRange = state.dateRange,
        selectedProduct = state.selectedProduct,
        selectedCurrency = state.selectedCurrency,
        selectedBaseCurrency = state.selectedBaseCurrency,
        username = state.username;

    return Object.assign({}, { requestParam: requestParam, dateRange: dateRange, selectedProduct: selectedProduct, selectedCurrency: selectedCurrency, selectedBaseCurrency: selectedBaseCurrency, username: username });
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(FormFilter);

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/index.jsx":
/*!******************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/running/components/index.jsx ***!
  \******************************************************************************************/
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

var _AppContainer = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/pages/bettingticketscenter/running/components/AppContainer.jsx");

var _AppContainer2 = _interopRequireDefault(_AppContainer);

var _reactRedux = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");

var _index = __webpack_require__(/*! ../reducers/index.js */ "./www/js/source/reactpages/pages/bettingticketscenter/running/reducers/index.js");

var _index2 = _interopRequireDefault(_index);

var _store = __webpack_require__(/*! ../../../../lib/store */ "./www/js/source/reactpages/lib/store.js");

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


_reactDom2.default.render(_react2.default.createElement(App, null), document.getElementById('app-container'));

/***/ }),

/***/ "./www/js/source/reactpages/pages/bettingticketscenter/running/reducers/index.js":
/*!***************************************************************************************!*\
  !*** ./www/js/source/reactpages/pages/bettingticketscenter/running/reducers/index.js ***!
  \***************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _redux = __webpack_require__(/*! redux */ "./node_modules/redux/es/index.js");

var _gridDataReducer = __webpack_require__(/*! @jsroot/reducers/gridDataReducer.js */ "./www/js/source/reactpages/reducers/gridDataReducer.js");

var gridDataReducer = _interopRequireWildcard(_gridDataReducer);

var _usernameReducer = __webpack_require__(/*! @jsroot/reducers/usernameReducer */ "./www/js/source/reactpages/reducers/usernameReducer.js");

var usernameReducer = _interopRequireWildcard(_usernameReducer);

var _productReducer = __webpack_require__(/*! @jsroot/reducers/productReducer */ "./www/js/source/reactpages/reducers/productReducer.js");

var productReducer = _interopRequireWildcard(_productReducer);

var _currencyReducer = __webpack_require__(/*! @jsroot/reducers/currencyReducer */ "./www/js/source/reactpages/reducers/currencyReducer.js");

var currencyReducer = _interopRequireWildcard(_currencyReducer);

var _baseCurrencyReducer = __webpack_require__(/*! @jsroot/reducers/baseCurrencyReducer */ "./www/js/source/reactpages/reducers/baseCurrencyReducer.js");

var baseCurrencyReducer = _interopRequireWildcard(_baseCurrencyReducer);

var _loaderReducer = __webpack_require__(/*! @jsroot/reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");

var loaderReducer = _interopRequireWildcard(_loaderReducer);

var _dateRangeReducer = __webpack_require__(/*! @jsroot/reducers/dateRangeReducer */ "./www/js/source/reactpages/reducers/dateRangeReducer.js");

var dateRangeReducer = _interopRequireWildcard(_dateRangeReducer);

var _requestParamReducer = __webpack_require__(/*! @jsroot/reducers/requestParamReducer */ "./www/js/source/reactpages/reducers/requestParamReducer.js");

var requestParamReducer = _interopRequireWildcard(_requestParamReducer);

var _reportTitleReducer = __webpack_require__(/*! @jsroot/reducers/reportTitleReducer */ "./www/js/source/reactpages/reducers/reportTitleReducer.js");

var reportTitleReducer = _interopRequireWildcard(_reportTitleReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = (0, _redux.combineReducers)(Object.assign(gridDataReducer, usernameReducer, productReducer, currencyReducer, baseCurrencyReducer, loaderReducer, dateRangeReducer, requestParamReducer, reportTitleReducer));

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

/***/ "./www/js/source/reactpages/reducers/dateRangeReducer.js":
/*!***************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/dateRangeReducer.js ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.dateRange = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var dateRange = exports.dateRange = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_DATE_RANGE, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/gridDataReducer.js":
/*!**************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/gridDataReducer.js ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.gridData = undefined;

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

var _types = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var gridData = exports.gridData = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_GRID_DATA, function (state, action) {
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

/***/ "./www/js/source/reactpages/reducers/reportTitleReducer.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/reportTitleReducer.js ***!
  \*****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.reportTitle = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var reportTitle = exports.reportTitle = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_REPORT_TITLE, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/requestParamReducer.js":
/*!******************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/requestParamReducer.js ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.requestParam = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var requestParam = exports.requestParam = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_REQUEST_PARAM, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ "./www/js/source/reactpages/reducers/usernameReducer.js":
/*!**************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/usernameReducer.js ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.username = undefined;

var _types = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var username = exports.username = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_USERNAME, function (state, action) {
    return action.data;
}));

/***/ })

/******/ });
//# sourceMappingURL=running.chunk.js.map