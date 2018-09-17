webpackJsonp([4],{

/***/ 100:
/***/ (function(module, exports) {

module.exports = ods;

/***/ }),

/***/ 11:
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/*!
  Copyright (c) 2016 Jed Watson.
  Licensed under the MIT License (MIT), see
  http://jedwatson.github.io/classnames
*/
/* global define */

(function () {
	'use strict';

	var hasOwn = {}.hasOwnProperty;

	function classNames () {
		var classes = [];

		for (var i = 0; i < arguments.length; i++) {
			var arg = arguments[i];
			if (!arg) continue;

			var argType = typeof arg;

			if (argType === 'string' || argType === 'number') {
				classes.push(arg);
			} else if (Array.isArray(arg)) {
				classes.push(classNames.apply(null, arg));
			} else if (argType === 'object') {
				for (var key in arg) {
					if (hasOwn.call(arg, key) && arg[key]) {
						classes.push(key);
					}
				}
			}
		}

		return classes.join(' ');
	}

	if (typeof module !== 'undefined' && module.exports) {
		module.exports = classNames;
	} else if (true) {
		// register as 'classnames', consistent with npm package name
		!(__WEBPACK_AMD_DEFINE_ARRAY__ = [], __WEBPACK_AMD_DEFINE_RESULT__ = (function () {
			return classNames;
		}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	} else {
		window.classNames = classNames;
	}
}());


/***/ }),

/***/ 15:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var _rootUrl = window.rootUrl || "/";

var Site = function () {
    function Site() {
        _classCallCheck(this, Site);
    }

    _createClass(Site, null, [{
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
    }]);

    return Site;
}();

exports.default = Site;

/***/ }),

/***/ 154:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _filterHandlerAction = __webpack_require__(454);

var filterHandlerAction = _interopRequireWildcard(_filterHandlerAction);

var _productAction = __webpack_require__(86);

var productAction = _interopRequireWildcard(_productAction);

var _currencyAction = __webpack_require__(95);

var currencyAction = _interopRequireWildcard(_currencyAction);

var _loaderAction = __webpack_require__(34);

var loaderAction = _interopRequireWildcard(_loaderAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, filterHandlerAction, productAction, currencyAction, loaderAction);

/***/ }),

/***/ 155:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.Service = exports.TotalText = exports.HeaderText = exports.Url = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _langHelper = __webpack_require__(4);

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

/***/ 21:
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

/***/ 216:
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

/***/ 22:
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

/***/ 28:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Cell = exports.Row = exports.Table = undefined;

var _Table = __webpack_require__(39);

var _Table2 = _interopRequireDefault(_Table);

var _Row = __webpack_require__(43);

var _Row2 = _interopRequireDefault(_Row);

var _Cell = __webpack_require__(44);

var _Cell2 = _interopRequireDefault(_Cell);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.Table = _Table2.default;
exports.Row = _Row2.default;
exports.Cell = _Cell2.default;

/***/ }),

/***/ 29:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _langHelper = __webpack_require__(4);

var _langHelper2 = _interopRequireDefault(_langHelper);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var NoDataTitle = function (_Component) {
    _inherits(NoDataTitle, _Component);

    function NoDataTitle(props) {
        _classCallCheck(this, NoDataTitle);

        return _possibleConstructorReturn(this, (NoDataTitle.__proto__ || Object.getPrototypeOf(NoDataTitle)).call(this, props));
    }

    _createClass(NoDataTitle, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'p',
                { className: 'no-info-row center-align' },
                _langHelper2.default.getSingleResource("NoInfo")
            );
        }
    }]);

    return NoDataTitle;
}(_react.Component);

exports.default = NoDataTitle;

/***/ }),

/***/ 34:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.toggleLoader = undefined;

var _types = __webpack_require__(5);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var toggleLoader = exports.toggleLoader = function toggleLoader(show) {
    return {
        type: types.TOGGLE_LOADER,
        data: show
    };
};

/***/ }),

/***/ 35:
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

/***/ 36:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _report = __webpack_require__(22);

var _site = __webpack_require__(15);

var _site2 = _interopRequireDefault(_site);

var _axios = __webpack_require__(57);

var _axios2 = _interopRequireDefault(_axios);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var fetchData = function fetchData(relativeUrl, params) {
    var url = _site2.default.resolveClientUrl(relativeUrl);
    return _axios2.default.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': params.xsrfToken
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

/***/ 37:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _classnames = __webpack_require__(11);

var _classnames2 = _interopRequireDefault(_classnames);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Classes = {
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
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            if (this.state.isOpening !== nextProps.isOpening) {
                var onOpen = this.props.onOpen;

                nextProps.isOpening && onOpen && onOpen();
                this.setState({ isOpening: nextProps.isOpening });
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

            onClose && onClose();

            this.setState({ isOpening: false });
        }
    }, {
        key: 'render',
        value: function render() {
            var _props = this.props,
                className = _props.className,
                children = _props.children;

            var mergedClassName = (0, _classnames2.default)(Classes.OVERLAY, _defineProperty({}, Classes.OVERLAY_OPEN, this.state.isOpening), className);
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

/***/ 39:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _RowLayout = __webpack_require__(40);

var _RowLayout2 = _interopRequireDefault(_RowLayout);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _tableSection = __webpack_require__(41);

var _tableSection2 = _interopRequireDefault(_tableSection);

var _constants = __webpack_require__(21);

var _Page = __webpack_require__(42);

var _Page2 = _interopRequireDefault(_Page);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

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
        _this.columnsWidth = _this._getColumnsWidth(props.header);
        _this.adjustedHeight = adjustedHeight;

        _this.state = {
            maxWidth: maxWidth,
            contentHeight: bodyHeight
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
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            this.columnsWidth = this._getColumnsWidth(nextProps.header);
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

            var newColumnsWidth = this.columnWidthSum && this.columnsWidth.map(function (cellWidth) {
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

            var newColumnLayout = this.columnWidthSum && this.columnWidthSum !== width ? this._getUpdatedColumnLayout() : this.columnsWidth;

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
                isPaging && pageOption && _react2.default.createElement(_Page2.default, { pageOption: pageOption, onPaging: onPaging })
            );
        }
    }, {
        key: 'columnWidthSum',
        get: function get() {
            return !this.columnsWidth || !this.columnsWidth.length || this.columnsWidth.find(function (x) {
                return typeof x !== "number";
            }) ? null : this.columnsWidth.reduce(function (prev, current) {
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

/***/ 4:
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

/***/ 40:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(1);

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

/***/ 41:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _constants = __webpack_require__(21);

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

/***/ 42:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Page = function (_Component) {
    _inherits(Page, _Component);

    function Page(props) {
        _classCallCheck(this, Page);

        var _this = _possibleConstructorReturn(this, (Page.__proto__ || Object.getPrototypeOf(Page)).call(this, props));

        _this.onKeyUpHandle = function (e, totalPage) {
            var code = e.keyCode ? e.keyCode : e.which;
            if (code === 13) {
                var value = parseInt(_this.state.inputValue);
                if (value > 0 && value <= totalPage) {
                    _this.goToPage(value, _this.props.pageOption.PageSize);
                }
            }
        };

        _this.state = {
            inputValue: _this.props.pageOption.PageIndex
        };
        return _this;
    }

    _createClass(Page, [{
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            if (this.props.pageOption.PageIndex !== nextProps.pageOption.PageIndex) {
                this._updateState(nextProps.pageOption.PageIndex);
            }
        }
    }, {
        key: 'goToPage',
        value: function goToPage(index, pageSize) {
            this._updateState(index);
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
            this.setState({
                inputValue: inputValue
            });
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
                    onChange: function onChange(e) {
                        return _this4._updateState(e.target.value);
                    },
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

            var _props$pageOption = this.props.pageOption,
                PageIndex = _props$pageOption.PageIndex,
                PageSize = _props$pageOption.PageSize,
                TotalItem = _props$pageOption.TotalItem,
                PageList = _props$pageOption.PageList;
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
                this._renderPageInput(inputValue, totalPage),
                this._renderPageIcon("page-next", isDisable || PageIndex >= totalPage || PageIndex < 1, function () {
                    return _this5.goToPage(PageIndex + 1, PageSize);
                }),
                this._renderPageIcon("page-last", PageIndex === totalPage, function () {
                    return _this5.goToPage(totalPage, PageSize);
                }),
                this._renderPageSizeSelection(isDisable, PageIndex, PageSize),
                this._renderPageInfo(PageIndex, PageSize, TotalItem)
            );
        }
    }]);

    return Page;
}(_react.Component);

Page.propTypes = {
    pageOption: _propTypes2.default.shape({
        PageIndex: _propTypes2.default.number,
        PageSize: _propTypes2.default.number,
        PageList: _propTypes2.default.arrayOf(_propTypes2.default.number),
        TotalItem: _propTypes2.default.number
    }),
    onPaging: _propTypes2.default.func
};
Page.defaultProps = {
    pageOption: {
        PageIndex: 1,
        PageSize: 50,
        PageList: [50, 100, 200],
        TotalItem: 0
    }
};
exports.default = Page;

/***/ }),

/***/ 43:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

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

/***/ 44:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(1);

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

/***/ 45:
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
            __webpack_require__.e/* import() */(0).then(__webpack_require__.bind(null, 97)).then(function (XLSX) {
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

/***/ 452:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _AppContainer = __webpack_require__(453);

var _AppContainer2 = _interopRequireDefault(_AppContainer);

var _reactRedux = __webpack_require__(13);

var _index = __webpack_require__(462);

var _index2 = _interopRequireDefault(_index);

var _store = __webpack_require__(66);

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

/***/ 453:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.AppContainer = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(13);

var _redux = __webpack_require__(10);

var _index = __webpack_require__(154);

var _FormFilter = __webpack_require__(455);

var _FormFilter2 = _interopRequireDefault(_FormFilter);

var _datagrid = __webpack_require__(461);

var _datagrid2 = _interopRequireDefault(_datagrid);

var _Loader = __webpack_require__(46);

var _Loader2 = _interopRequireDefault(_Loader);

var _AppLayout = __webpack_require__(47);

var _AppLayout2 = _interopRequireDefault(_AppLayout);

var _ReportTitle = __webpack_require__(48);

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

/***/ 454:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setReportTitle = exports.setParams = exports.setDateRange = exports.setSelectedReportBy = undefined;

var _types = __webpack_require__(216);

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

/***/ 455:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.FormFilter = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(13);

var _index = __webpack_require__(154);

var _redux = __webpack_require__(10);

var _reactSelect = __webpack_require__(58);

var _reactSelect2 = _interopRequireDefault(_reactSelect);

var _multipleSelect = __webpack_require__(88);

var _multipleSelect2 = _interopRequireDefault(_multipleSelect);

var _langHelper = __webpack_require__(4);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(15);

var _site2 = _interopRequireDefault(_site);

var _report = __webpack_require__(36);

var _report2 = _interopRequireDefault(_report);

var _service = __webpack_require__(155);

var _reportExcel = __webpack_require__(456);

var _reportExcel2 = _interopRequireDefault(_reportExcel);

var _index2 = __webpack_require__(457);

var _index3 = _interopRequireDefault(_index2);

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
            var productOptions = this.props.productOptions;

            return productOptions && productOptions.length && _react2.default.createElement(_multipleSelect2.default, { id: 'ProductList',
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

            var _props2 = this.props,
                currencyList = _props2.currencyList,
                selectedCurrency = _props2.selectedCurrency;


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
            var _props3 = this.props,
                dateRange = _props3.dateRange,
                dateConstraint = _props3.dateConstraint;


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

/***/ 456:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _moment = __webpack_require__(7);

var _moment2 = _interopRequireDefault(_moment);

var _excel = __webpack_require__(45);

var _excel2 = _interopRequireDefault(_excel);

var _langHelper = __webpack_require__(4);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _excelStyle = __webpack_require__(65);

var _service = __webpack_require__(155);

var _report = __webpack_require__(22);

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

/***/ 457:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _MonthPicker = __webpack_require__(458);

var _MonthPicker2 = _interopRequireDefault(_MonthPicker);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _MonthPicker2.default;

/***/ }),

/***/ 458:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _Month = __webpack_require__(459);

var _Month2 = _interopRequireDefault(_Month);

var _Day = __webpack_require__(460);

var _Day2 = _interopRequireDefault(_Day);

var _moment = __webpack_require__(7);

var _moment2 = _interopRequireDefault(_moment);

var _propTypes = __webpack_require__(1);

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

/***/ 459:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _Overlay = __webpack_require__(37);

var _Overlay2 = _interopRequireDefault(_Overlay);

var _langHelper = __webpack_require__(4);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _propTypes = __webpack_require__(1);

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
            var _this2 = this;

            var options = [];

            var _loop = function _loop(i) {
                if (i === _this2.state.expandYear) {

                    options.push(_react2.default.createElement(
                        'li',
                        { key: i, className: 'year' },
                        _react2.default.createElement(
                            'a',
                            { href: 'javascript:;' },
                            i
                        )
                    ));

                    var minMonth = i === min.year ? min.month : 0;
                    var maxMonth = i === max.year ? max.month : 11;

                    var _loop2 = function _loop2(j) {
                        var isSelected = i === value.year && j === value.month;
                        var valueItem = { year: i, month: j };

                        options.push(_react2.default.createElement(
                            'li',
                            { key: '' + j + i, className: 'monthWrap', onClick: function onClick() {
                                    return _this2._onChange(valueItem);
                                } },
                            _react2.default.createElement(
                                'a',
                                { href: 'javascript:;', className: '' + (isSelected ? 'selected' : '') },
                                _this2.fullMonths[j]
                            )
                        ));
                    };

                    for (var j = minMonth; j <= maxMonth; j++) {
                        _loop2(j);
                    }
                } else {
                    options.push(_react2.default.createElement(
                        'li',
                        { key: i, className: 'year', onClick: function onClick() {
                                return _this2.setState({ expandYear: i });
                            } },
                        _react2.default.createElement(
                            'a',
                            { href: 'javascript:;' },
                            i
                        )
                    ));
                }
            };

            for (var i = min.year; i <= max.year; i++) {
                _loop(i);
            }

            return options;
        }
    }, {
        key: '_addMonth',
        value: function _addMonth(number) {
            var month = this.props.value.month + number;
            var year = this.props.value.year;

            if (month > 11) {
                month = 0;
                year += 1;
            } else if (month < 0) {
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


            return year > min.year && year < max.year || year === min.year && month >= min.month || year === max && month <= max.month;
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
            var _this3 = this;

            var nexMonth = this._addMonth(1);
            var preMonth = this._addMonth(-1);
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
                                        return validPreMonth && _this3._goToMonth(e, preMonth);
                                    } },
                                _react2.default.createElement('b', null)
                            ),
                            _react2.default.createElement(
                                'span',
                                { className: 'selectedMonth', onClick: function onClick() {
                                        _this3.setState({ showMonthDrop: true });
                                    } },
                                this._formatString(value.month, value.year)
                            ),
                            _react2.default.createElement(
                                'a',
                                { href: 'javascript:;', className: 'next ' + (validNexMonth ? '' : 'disable'), onClick: function onClick(e) {
                                        return validNexMonth && _this3._goToMonth(e, nexMonth);
                                    } },
                                _react2.default.createElement('b', null)
                            )
                        ),
                        _react2.default.createElement(
                            _Overlay2.default,
                            { isOpening: this.state.showMonthDrop, onClose: function onClose() {
                                    _this3.setState({ showMonthDrop: false });
                                } },
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

/***/ 46:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Loader = function (_React$Component) {
    _inherits(Loader, _React$Component);

    function Loader(props) {
        _classCallCheck(this, Loader);

        return _possibleConstructorReturn(this, (Loader.__proto__ || Object.getPrototypeOf(Loader)).call(this, props));
    }

    _createClass(Loader, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'loading-process default', style: { display: '' + (this.props.isLoading ? 'block' : 'none') } },
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
        }
    }]);

    return Loader;
}(_react2.default.Component);

exports.default = Loader;
;

/***/ }),

/***/ 460:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _Overlay = __webpack_require__(37);

var _Overlay2 = _interopRequireDefault(_Overlay);

var _propTypes = __webpack_require__(1);

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
            var _this2 = this;

            var options = [];

            var _loop = function _loop(i) {
                options.push(_react2.default.createElement(
                    'li',
                    { key: i, onClick: function onClick() {
                            return _this2._onChange(i);
                        } },
                    _react2.default.createElement(
                        'a',
                        { href: 'javascript:;' },
                        i
                    )
                ));
            };

            for (var i = min; i <= max; i++) {
                _loop(i);
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
            var _this3 = this;

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
                                    return _this3.setState({ showDateDrop: true });
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
                            { isOpening: this.state.showDateDrop, onClose: function onClose() {
                                    return _this3.setState({ showDateDrop: false });
                                } },
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

/***/ 461:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.DataGrid = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _moment = __webpack_require__(7);

var _moment2 = _interopRequireDefault(_moment);

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(13);

var _index = __webpack_require__(154);

var _redux = __webpack_require__(10);

var _fixedTableHeader = __webpack_require__(28);

var _numberFormat = __webpack_require__(35);

var _numberFormat2 = _interopRequireDefault(_numberFormat);

var _NoDataTitle = __webpack_require__(29);

var _NoDataTitle2 = _interopRequireDefault(_NoDataTitle);

var _report = __webpack_require__(36);

var _report2 = _interopRequireDefault(_report);

var _report3 = __webpack_require__(22);

var _service = __webpack_require__(155);

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
                    { key: item.Ticks + item.CurrencyId },
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

/***/ 462:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _redux = __webpack_require__(10);

var _filterHandlerReducer = __webpack_require__(463);

var filterHandlerReducer = _interopRequireWildcard(_filterHandlerReducer);

var _productReducer = __webpack_require__(93);

var productReducer = _interopRequireWildcard(_productReducer);

var _currencyReducer = __webpack_require__(96);

var currencyReducer = _interopRequireWildcard(_currencyReducer);

var _loaderReducer = __webpack_require__(49);

var loaderReducer = _interopRequireWildcard(_loaderReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = (0, _redux.combineReducers)(Object.assign(filterHandlerReducer, productReducer, currencyReducer, loaderReducer));

/***/ }),

/***/ 463:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.reportTitle = exports.params = exports.dateConstraint = exports.xsrfToken = exports.dateRange = exports.selectedReportBy = exports.reportByList = undefined;

var _createReducer8 = __webpack_require__(8);

var _createReducer9 = _interopRequireDefault(_createReducer8);

var _types = __webpack_require__(216);

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

/***/ 47:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

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

/***/ 48:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var ReportTitle = function (_Component) {
    _inherits(ReportTitle, _Component);

    function ReportTitle(props) {
        _classCallCheck(this, ReportTitle);

        return _possibleConstructorReturn(this, (ReportTitle.__proto__ || Object.getPrototypeOf(ReportTitle)).call(this, props));
    }

    _createClass(ReportTitle, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'report-title-container' },
                _react2.default.createElement(
                    'p',
                    { className: 'report-title' },
                    this.props.content
                )
            );
        }
    }]);

    return ReportTitle;
}(_react.Component);

exports.default = ReportTitle;

/***/ }),

/***/ 49:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.isLoading = undefined;

var _types = __webpack_require__(5);

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(8);

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var isLoading = exports.isLoading = (0, _createReducer3.default)({}, _defineProperty({}, types.TOGGLE_LOADER, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 5:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
/* Product */
var SET_SELECTED_PRODUCT = exports.SET_SELECTED_PRODUCT = "SET_SELECTED_PRODUCT";
var SET_PRODUCT_OPTIONS = exports.SET_PRODUCT_OPTIONS = "SET_PRODUCT_OPTIONS";

/* Base Currency */
var SET_SELECTED_BASECURRENCY = exports.SET_SELECTED_BASECURRENCY = "SET_SELECTED_BASECURRENCY";
var GET_BASECURRENCY = exports.GET_BASECURRENCY = "GET_BASECURRENCY";

/* Currency */
var SET_SELECTED_CURRENCY = exports.SET_SELECTED_CURRENCY = "SET_SELECTED_CURRENCY";
var GET_CURRENCY = exports.GET_CURRENCY = "GET_CURRENCY";

/* Loader */
var TOGGLE_LOADER = exports.TOGGLE_LOADER = "TOGGLE_LOADER";

/* Exclude Test Data */
var CHANGE_IS_EXCLUDE_TEST = exports.CHANGE_IS_EXCLUDE_TEST = "CHANGE_IS_EXCLUDE_TEST";

/* User mini guide */
var USER_GUIDE_CONTENT = exports.USER_GUIDE_CONTENT = "USER_GUIDE_CONTENT;";

/* Xsrf Token */
var GET_XSRF_TOKEN = exports.GET_XSRF_TOKEN = "GET_XSRF_TOKEN";
/* Date range */
var SET_DATE_RANGE = exports.SET_DATE_RANGE = 'SET_DATE_RANGE';
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = 'GET_DATE_CONSTRAINT';

/***/ }),

/***/ 50:
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

/***/ 54:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(1);

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

/***/ 58:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Async", function() { return Async; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AsyncCreatable", function() { return AsyncCreatableSelect; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Creatable", function() { return CreatableSelect; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Value", function() { return Value; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Option", function() { return Option; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultMenuRenderer", function() { return menuRenderer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultArrowRenderer", function() { return arrowRenderer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultClearRenderer", function() { return clearRenderer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultFilterOptions", function() { return filterOptions; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_react_input_autosize__ = __webpack_require__(59);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_react_input_autosize___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_react_input_autosize__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_classnames__ = __webpack_require__(11);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_classnames___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_classnames__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_prop_types__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_prop_types___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_prop_types__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_react__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_react___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_react__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_react_dom__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_react_dom___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_react_dom__);






var arrowRenderer = function arrowRenderer(_ref) {
	var onMouseDown = _ref.onMouseDown;

	return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('span', {
		className: 'Select-arrow',
		onMouseDown: onMouseDown
	});
};

arrowRenderer.propTypes = {
	onMouseDown: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func
};

var clearRenderer = function clearRenderer() {
	return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('span', {
		className: 'Select-clear',
		dangerouslySetInnerHTML: { __html: '&times;' }
	});
};

var map = [{ 'base': 'A', 'letters': /[\u0041\u24B6\uFF21\u00C0\u00C1\u00C2\u1EA6\u1EA4\u1EAA\u1EA8\u00C3\u0100\u0102\u1EB0\u1EAE\u1EB4\u1EB2\u0226\u01E0\u00C4\u01DE\u1EA2\u00C5\u01FA\u01CD\u0200\u0202\u1EA0\u1EAC\u1EB6\u1E00\u0104\u023A\u2C6F]/g }, { 'base': 'AA', 'letters': /[\uA732]/g }, { 'base': 'AE', 'letters': /[\u00C6\u01FC\u01E2]/g }, { 'base': 'AO', 'letters': /[\uA734]/g }, { 'base': 'AU', 'letters': /[\uA736]/g }, { 'base': 'AV', 'letters': /[\uA738\uA73A]/g }, { 'base': 'AY', 'letters': /[\uA73C]/g }, { 'base': 'B', 'letters': /[\u0042\u24B7\uFF22\u1E02\u1E04\u1E06\u0243\u0182\u0181]/g }, { 'base': 'C', 'letters': /[\u0043\u24B8\uFF23\u0106\u0108\u010A\u010C\u00C7\u1E08\u0187\u023B\uA73E]/g }, { 'base': 'D', 'letters': /[\u0044\u24B9\uFF24\u1E0A\u010E\u1E0C\u1E10\u1E12\u1E0E\u0110\u018B\u018A\u0189\uA779]/g }, { 'base': 'DZ', 'letters': /[\u01F1\u01C4]/g }, { 'base': 'Dz', 'letters': /[\u01F2\u01C5]/g }, { 'base': 'E', 'letters': /[\u0045\u24BA\uFF25\u00C8\u00C9\u00CA\u1EC0\u1EBE\u1EC4\u1EC2\u1EBC\u0112\u1E14\u1E16\u0114\u0116\u00CB\u1EBA\u011A\u0204\u0206\u1EB8\u1EC6\u0228\u1E1C\u0118\u1E18\u1E1A\u0190\u018E]/g }, { 'base': 'F', 'letters': /[\u0046\u24BB\uFF26\u1E1E\u0191\uA77B]/g }, { 'base': 'G', 'letters': /[\u0047\u24BC\uFF27\u01F4\u011C\u1E20\u011E\u0120\u01E6\u0122\u01E4\u0193\uA7A0\uA77D\uA77E]/g }, { 'base': 'H', 'letters': /[\u0048\u24BD\uFF28\u0124\u1E22\u1E26\u021E\u1E24\u1E28\u1E2A\u0126\u2C67\u2C75\uA78D]/g }, { 'base': 'I', 'letters': /[\u0049\u24BE\uFF29\u00CC\u00CD\u00CE\u0128\u012A\u012C\u0130\u00CF\u1E2E\u1EC8\u01CF\u0208\u020A\u1ECA\u012E\u1E2C\u0197]/g }, { 'base': 'J', 'letters': /[\u004A\u24BF\uFF2A\u0134\u0248]/g }, { 'base': 'K', 'letters': /[\u004B\u24C0\uFF2B\u1E30\u01E8\u1E32\u0136\u1E34\u0198\u2C69\uA740\uA742\uA744\uA7A2]/g }, { 'base': 'L', 'letters': /[\u004C\u24C1\uFF2C\u013F\u0139\u013D\u1E36\u1E38\u013B\u1E3C\u1E3A\u0141\u023D\u2C62\u2C60\uA748\uA746\uA780]/g }, { 'base': 'LJ', 'letters': /[\u01C7]/g }, { 'base': 'Lj', 'letters': /[\u01C8]/g }, { 'base': 'M', 'letters': /[\u004D\u24C2\uFF2D\u1E3E\u1E40\u1E42\u2C6E\u019C]/g }, { 'base': 'N', 'letters': /[\u004E\u24C3\uFF2E\u01F8\u0143\u00D1\u1E44\u0147\u1E46\u0145\u1E4A\u1E48\u0220\u019D\uA790\uA7A4]/g }, { 'base': 'NJ', 'letters': /[\u01CA]/g }, { 'base': 'Nj', 'letters': /[\u01CB]/g }, { 'base': 'O', 'letters': /[\u004F\u24C4\uFF2F\u00D2\u00D3\u00D4\u1ED2\u1ED0\u1ED6\u1ED4\u00D5\u1E4C\u022C\u1E4E\u014C\u1E50\u1E52\u014E\u022E\u0230\u00D6\u022A\u1ECE\u0150\u01D1\u020C\u020E\u01A0\u1EDC\u1EDA\u1EE0\u1EDE\u1EE2\u1ECC\u1ED8\u01EA\u01EC\u00D8\u01FE\u0186\u019F\uA74A\uA74C]/g }, { 'base': 'OI', 'letters': /[\u01A2]/g }, { 'base': 'OO', 'letters': /[\uA74E]/g }, { 'base': 'OU', 'letters': /[\u0222]/g }, { 'base': 'P', 'letters': /[\u0050\u24C5\uFF30\u1E54\u1E56\u01A4\u2C63\uA750\uA752\uA754]/g }, { 'base': 'Q', 'letters': /[\u0051\u24C6\uFF31\uA756\uA758\u024A]/g }, { 'base': 'R', 'letters': /[\u0052\u24C7\uFF32\u0154\u1E58\u0158\u0210\u0212\u1E5A\u1E5C\u0156\u1E5E\u024C\u2C64\uA75A\uA7A6\uA782]/g }, { 'base': 'S', 'letters': /[\u0053\u24C8\uFF33\u1E9E\u015A\u1E64\u015C\u1E60\u0160\u1E66\u1E62\u1E68\u0218\u015E\u2C7E\uA7A8\uA784]/g }, { 'base': 'T', 'letters': /[\u0054\u24C9\uFF34\u1E6A\u0164\u1E6C\u021A\u0162\u1E70\u1E6E\u0166\u01AC\u01AE\u023E\uA786]/g }, { 'base': 'TZ', 'letters': /[\uA728]/g }, { 'base': 'U', 'letters': /[\u0055\u24CA\uFF35\u00D9\u00DA\u00DB\u0168\u1E78\u016A\u1E7A\u016C\u00DC\u01DB\u01D7\u01D5\u01D9\u1EE6\u016E\u0170\u01D3\u0214\u0216\u01AF\u1EEA\u1EE8\u1EEE\u1EEC\u1EF0\u1EE4\u1E72\u0172\u1E76\u1E74\u0244]/g }, { 'base': 'V', 'letters': /[\u0056\u24CB\uFF36\u1E7C\u1E7E\u01B2\uA75E\u0245]/g }, { 'base': 'VY', 'letters': /[\uA760]/g }, { 'base': 'W', 'letters': /[\u0057\u24CC\uFF37\u1E80\u1E82\u0174\u1E86\u1E84\u1E88\u2C72]/g }, { 'base': 'X', 'letters': /[\u0058\u24CD\uFF38\u1E8A\u1E8C]/g }, { 'base': 'Y', 'letters': /[\u0059\u24CE\uFF39\u1EF2\u00DD\u0176\u1EF8\u0232\u1E8E\u0178\u1EF6\u1EF4\u01B3\u024E\u1EFE]/g }, { 'base': 'Z', 'letters': /[\u005A\u24CF\uFF3A\u0179\u1E90\u017B\u017D\u1E92\u1E94\u01B5\u0224\u2C7F\u2C6B\uA762]/g }, { 'base': 'a', 'letters': /[\u0061\u24D0\uFF41\u1E9A\u00E0\u00E1\u00E2\u1EA7\u1EA5\u1EAB\u1EA9\u00E3\u0101\u0103\u1EB1\u1EAF\u1EB5\u1EB3\u0227\u01E1\u00E4\u01DF\u1EA3\u00E5\u01FB\u01CE\u0201\u0203\u1EA1\u1EAD\u1EB7\u1E01\u0105\u2C65\u0250]/g }, { 'base': 'aa', 'letters': /[\uA733]/g }, { 'base': 'ae', 'letters': /[\u00E6\u01FD\u01E3]/g }, { 'base': 'ao', 'letters': /[\uA735]/g }, { 'base': 'au', 'letters': /[\uA737]/g }, { 'base': 'av', 'letters': /[\uA739\uA73B]/g }, { 'base': 'ay', 'letters': /[\uA73D]/g }, { 'base': 'b', 'letters': /[\u0062\u24D1\uFF42\u1E03\u1E05\u1E07\u0180\u0183\u0253]/g }, { 'base': 'c', 'letters': /[\u0063\u24D2\uFF43\u0107\u0109\u010B\u010D\u00E7\u1E09\u0188\u023C\uA73F\u2184]/g }, { 'base': 'd', 'letters': /[\u0064\u24D3\uFF44\u1E0B\u010F\u1E0D\u1E11\u1E13\u1E0F\u0111\u018C\u0256\u0257\uA77A]/g }, { 'base': 'dz', 'letters': /[\u01F3\u01C6]/g }, { 'base': 'e', 'letters': /[\u0065\u24D4\uFF45\u00E8\u00E9\u00EA\u1EC1\u1EBF\u1EC5\u1EC3\u1EBD\u0113\u1E15\u1E17\u0115\u0117\u00EB\u1EBB\u011B\u0205\u0207\u1EB9\u1EC7\u0229\u1E1D\u0119\u1E19\u1E1B\u0247\u025B\u01DD]/g }, { 'base': 'f', 'letters': /[\u0066\u24D5\uFF46\u1E1F\u0192\uA77C]/g }, { 'base': 'g', 'letters': /[\u0067\u24D6\uFF47\u01F5\u011D\u1E21\u011F\u0121\u01E7\u0123\u01E5\u0260\uA7A1\u1D79\uA77F]/g }, { 'base': 'h', 'letters': /[\u0068\u24D7\uFF48\u0125\u1E23\u1E27\u021F\u1E25\u1E29\u1E2B\u1E96\u0127\u2C68\u2C76\u0265]/g }, { 'base': 'hv', 'letters': /[\u0195]/g }, { 'base': 'i', 'letters': /[\u0069\u24D8\uFF49\u00EC\u00ED\u00EE\u0129\u012B\u012D\u00EF\u1E2F\u1EC9\u01D0\u0209\u020B\u1ECB\u012F\u1E2D\u0268\u0131]/g }, { 'base': 'j', 'letters': /[\u006A\u24D9\uFF4A\u0135\u01F0\u0249]/g }, { 'base': 'k', 'letters': /[\u006B\u24DA\uFF4B\u1E31\u01E9\u1E33\u0137\u1E35\u0199\u2C6A\uA741\uA743\uA745\uA7A3]/g }, { 'base': 'l', 'letters': /[\u006C\u24DB\uFF4C\u0140\u013A\u013E\u1E37\u1E39\u013C\u1E3D\u1E3B\u017F\u0142\u019A\u026B\u2C61\uA749\uA781\uA747]/g }, { 'base': 'lj', 'letters': /[\u01C9]/g }, { 'base': 'm', 'letters': /[\u006D\u24DC\uFF4D\u1E3F\u1E41\u1E43\u0271\u026F]/g }, { 'base': 'n', 'letters': /[\u006E\u24DD\uFF4E\u01F9\u0144\u00F1\u1E45\u0148\u1E47\u0146\u1E4B\u1E49\u019E\u0272\u0149\uA791\uA7A5]/g }, { 'base': 'nj', 'letters': /[\u01CC]/g }, { 'base': 'o', 'letters': /[\u006F\u24DE\uFF4F\u00F2\u00F3\u00F4\u1ED3\u1ED1\u1ED7\u1ED5\u00F5\u1E4D\u022D\u1E4F\u014D\u1E51\u1E53\u014F\u022F\u0231\u00F6\u022B\u1ECF\u0151\u01D2\u020D\u020F\u01A1\u1EDD\u1EDB\u1EE1\u1EDF\u1EE3\u1ECD\u1ED9\u01EB\u01ED\u00F8\u01FF\u0254\uA74B\uA74D\u0275]/g }, { 'base': 'oi', 'letters': /[\u01A3]/g }, { 'base': 'ou', 'letters': /[\u0223]/g }, { 'base': 'oo', 'letters': /[\uA74F]/g }, { 'base': 'p', 'letters': /[\u0070\u24DF\uFF50\u1E55\u1E57\u01A5\u1D7D\uA751\uA753\uA755]/g }, { 'base': 'q', 'letters': /[\u0071\u24E0\uFF51\u024B\uA757\uA759]/g }, { 'base': 'r', 'letters': /[\u0072\u24E1\uFF52\u0155\u1E59\u0159\u0211\u0213\u1E5B\u1E5D\u0157\u1E5F\u024D\u027D\uA75B\uA7A7\uA783]/g }, { 'base': 's', 'letters': /[\u0073\u24E2\uFF53\u00DF\u015B\u1E65\u015D\u1E61\u0161\u1E67\u1E63\u1E69\u0219\u015F\u023F\uA7A9\uA785\u1E9B]/g }, { 'base': 't', 'letters': /[\u0074\u24E3\uFF54\u1E6B\u1E97\u0165\u1E6D\u021B\u0163\u1E71\u1E6F\u0167\u01AD\u0288\u2C66\uA787]/g }, { 'base': 'tz', 'letters': /[\uA729]/g }, { 'base': 'u', 'letters': /[\u0075\u24E4\uFF55\u00F9\u00FA\u00FB\u0169\u1E79\u016B\u1E7B\u016D\u00FC\u01DC\u01D8\u01D6\u01DA\u1EE7\u016F\u0171\u01D4\u0215\u0217\u01B0\u1EEB\u1EE9\u1EEF\u1EED\u1EF1\u1EE5\u1E73\u0173\u1E77\u1E75\u0289]/g }, { 'base': 'v', 'letters': /[\u0076\u24E5\uFF56\u1E7D\u1E7F\u028B\uA75F\u028C]/g }, { 'base': 'vy', 'letters': /[\uA761]/g }, { 'base': 'w', 'letters': /[\u0077\u24E6\uFF57\u1E81\u1E83\u0175\u1E87\u1E85\u1E98\u1E89\u2C73]/g }, { 'base': 'x', 'letters': /[\u0078\u24E7\uFF58\u1E8B\u1E8D]/g }, { 'base': 'y', 'letters': /[\u0079\u24E8\uFF59\u1EF3\u00FD\u0177\u1EF9\u0233\u1E8F\u00FF\u1EF7\u1E99\u1EF5\u01B4\u024F\u1EFF]/g }, { 'base': 'z', 'letters': /[\u007A\u24E9\uFF5A\u017A\u1E91\u017C\u017E\u1E93\u1E95\u01B6\u0225\u0240\u2C6C\uA763]/g }];

var stripDiacritics = function stripDiacritics(str) {
	for (var i = 0; i < map.length; i++) {
		str = str.replace(map[i].letters, map[i].base);
	}
	return str;
};

var trim = function trim(str) {
  return str.replace(/^\s+|\s+$/g, '');
};

var isValid = function isValid(value) {
	return typeof value !== 'undefined' && value !== null && value !== '';
};

var filterOptions = function filterOptions(options, filterValue, excludeOptions, props) {
	if (props.ignoreAccents) {
		filterValue = stripDiacritics(filterValue);
	}

	if (props.ignoreCase) {
		filterValue = filterValue.toLowerCase();
	}

	if (props.trimFilter) {
		filterValue = trim(filterValue);
	}

	if (excludeOptions) excludeOptions = excludeOptions.map(function (i) {
		return i[props.valueKey];
	});

	return options.filter(function (option) {
		if (excludeOptions && excludeOptions.indexOf(option[props.valueKey]) > -1) return false;
		if (props.filterOption) return props.filterOption.call(undefined, option, filterValue);
		if (!filterValue) return true;

		var value = option[props.valueKey];
		var label = option[props.labelKey];
		var hasValue = isValid(value);
		var hasLabel = isValid(label);

		if (!hasValue && !hasLabel) {
			return false;
		}

		var valueTest = hasValue ? String(value) : null;
		var labelTest = hasLabel ? String(label) : null;

		if (props.ignoreAccents) {
			if (valueTest && props.matchProp !== 'label') valueTest = stripDiacritics(valueTest);
			if (labelTest && props.matchProp !== 'value') labelTest = stripDiacritics(labelTest);
		}

		if (props.ignoreCase) {
			if (valueTest && props.matchProp !== 'label') valueTest = valueTest.toLowerCase();
			if (labelTest && props.matchProp !== 'value') labelTest = labelTest.toLowerCase();
		}

		return props.matchPos === 'start' ? valueTest && props.matchProp !== 'label' && valueTest.substr(0, filterValue.length) === filterValue || labelTest && props.matchProp !== 'value' && labelTest.substr(0, filterValue.length) === filterValue : valueTest && props.matchProp !== 'label' && valueTest.indexOf(filterValue) >= 0 || labelTest && props.matchProp !== 'value' && labelTest.indexOf(filterValue) >= 0;
	});
};

var menuRenderer = function menuRenderer(_ref) {
	var focusedOption = _ref.focusedOption,
	    focusOption = _ref.focusOption,
	    inputValue = _ref.inputValue,
	    instancePrefix = _ref.instancePrefix,
	    onFocus = _ref.onFocus,
	    onOptionRef = _ref.onOptionRef,
	    onSelect = _ref.onSelect,
	    optionClassName = _ref.optionClassName,
	    optionComponent = _ref.optionComponent,
	    optionRenderer = _ref.optionRenderer,
	    options = _ref.options,
	    removeValue = _ref.removeValue,
	    selectValue = _ref.selectValue,
	    valueArray = _ref.valueArray,
	    valueKey = _ref.valueKey;

	var Option = optionComponent;

	return options.map(function (option, i) {
		var isSelected = valueArray && valueArray.some(function (x) {
			return x[valueKey] === option[valueKey];
		});
		var isFocused = option === focusedOption;
		var optionClass = __WEBPACK_IMPORTED_MODULE_1_classnames___default()(optionClassName, {
			'Select-option': true,
			'is-selected': isSelected,
			'is-focused': isFocused,
			'is-disabled': option.disabled
		});

		return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
			Option,
			{
				className: optionClass,
				focusOption: focusOption,
				inputValue: inputValue,
				instancePrefix: instancePrefix,
				isDisabled: option.disabled,
				isFocused: isFocused,
				isSelected: isSelected,
				key: 'option-' + i + '-' + option[valueKey],
				onFocus: onFocus,
				onSelect: onSelect,
				option: option,
				optionIndex: i,
				ref: function ref(_ref2) {
					onOptionRef(_ref2, isFocused);
				},
				removeValue: removeValue,
				selectValue: selectValue
			},
			optionRenderer(option, i, inputValue)
		);
	});
};

menuRenderer.propTypes = {
	focusOption: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	focusedOption: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object,
	inputValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string,
	instancePrefix: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string,
	onFocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	onOptionRef: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	onSelect: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	optionClassName: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string,
	optionComponent: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	optionRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	options: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.array,
	removeValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	selectValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,
	valueArray: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.array,
	valueKey: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string
};

var blockEvent = (function (event) {
	event.preventDefault();
	event.stopPropagation();
	if (event.target.tagName !== 'A' || !('href' in event.target)) {
		return;
	}
	if (event.target.target) {
		window.open(event.target.href, event.target.target);
	} else {
		window.location.href = event.target.href;
	}
});

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) {
  return typeof obj;
} : function (obj) {
  return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj;
};





var asyncGenerator = function () {
  function AwaitValue(value) {
    this.value = value;
  }

  function AsyncGenerator(gen) {
    var front, back;

    function send(key, arg) {
      return new Promise(function (resolve, reject) {
        var request = {
          key: key,
          arg: arg,
          resolve: resolve,
          reject: reject,
          next: null
        };

        if (back) {
          back = back.next = request;
        } else {
          front = back = request;
          resume(key, arg);
        }
      });
    }

    function resume(key, arg) {
      try {
        var result = gen[key](arg);
        var value = result.value;

        if (value instanceof AwaitValue) {
          Promise.resolve(value.value).then(function (arg) {
            resume("next", arg);
          }, function (arg) {
            resume("throw", arg);
          });
        } else {
          settle(result.done ? "return" : "normal", result.value);
        }
      } catch (err) {
        settle("throw", err);
      }
    }

    function settle(type, value) {
      switch (type) {
        case "return":
          front.resolve({
            value: value,
            done: true
          });
          break;

        case "throw":
          front.reject(value);
          break;

        default:
          front.resolve({
            value: value,
            done: false
          });
          break;
      }

      front = front.next;

      if (front) {
        resume(front.key, front.arg);
      } else {
        back = null;
      }
    }

    this._invoke = send;

    if (typeof gen.return !== "function") {
      this.return = undefined;
    }
  }

  if (typeof Symbol === "function" && Symbol.asyncIterator) {
    AsyncGenerator.prototype[Symbol.asyncIterator] = function () {
      return this;
    };
  }

  AsyncGenerator.prototype.next = function (arg) {
    return this._invoke("next", arg);
  };

  AsyncGenerator.prototype.throw = function (arg) {
    return this._invoke("throw", arg);
  };

  AsyncGenerator.prototype.return = function (arg) {
    return this._invoke("return", arg);
  };

  return {
    wrap: function (fn) {
      return function () {
        return new AsyncGenerator(fn.apply(this, arguments));
      };
    },
    await: function (value) {
      return new AwaitValue(value);
    }
  };
}();





var classCallCheck = function (instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
};

var createClass = function () {
  function defineProperties(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  return function (Constructor, protoProps, staticProps) {
    if (protoProps) defineProperties(Constructor.prototype, protoProps);
    if (staticProps) defineProperties(Constructor, staticProps);
    return Constructor;
  };
}();





var defineProperty = function (obj, key, value) {
  if (key in obj) {
    Object.defineProperty(obj, key, {
      value: value,
      enumerable: true,
      configurable: true,
      writable: true
    });
  } else {
    obj[key] = value;
  }

  return obj;
};

var _extends = Object.assign || function (target) {
  for (var i = 1; i < arguments.length; i++) {
    var source = arguments[i];

    for (var key in source) {
      if (Object.prototype.hasOwnProperty.call(source, key)) {
        target[key] = source[key];
      }
    }
  }

  return target;
};



var inherits = function (subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function, not " + typeof superClass);
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      enumerable: false,
      writable: true,
      configurable: true
    }
  });
  if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass;
};









var objectWithoutProperties = function (obj, keys) {
  var target = {};

  for (var i in obj) {
    if (keys.indexOf(i) >= 0) continue;
    if (!Object.prototype.hasOwnProperty.call(obj, i)) continue;
    target[i] = obj[i];
  }

  return target;
};

var possibleConstructorReturn = function (self, call) {
  if (!self) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return call && (typeof call === "object" || typeof call === "function") ? call : self;
};

var Option = function (_React$Component) {
	inherits(Option, _React$Component);

	function Option(props) {
		classCallCheck(this, Option);

		var _this = possibleConstructorReturn(this, (Option.__proto__ || Object.getPrototypeOf(Option)).call(this, props));

		_this.handleMouseDown = _this.handleMouseDown.bind(_this);
		_this.handleMouseEnter = _this.handleMouseEnter.bind(_this);
		_this.handleMouseMove = _this.handleMouseMove.bind(_this);
		_this.handleTouchStart = _this.handleTouchStart.bind(_this);
		_this.handleTouchEnd = _this.handleTouchEnd.bind(_this);
		_this.handleTouchMove = _this.handleTouchMove.bind(_this);
		_this.onFocus = _this.onFocus.bind(_this);
		return _this;
	}

	createClass(Option, [{
		key: 'handleMouseDown',
		value: function handleMouseDown(event) {
			event.preventDefault();
			event.stopPropagation();
			this.props.onSelect(this.props.option, event);
		}
	}, {
		key: 'handleMouseEnter',
		value: function handleMouseEnter(event) {
			this.onFocus(event);
		}
	}, {
		key: 'handleMouseMove',
		value: function handleMouseMove(event) {
			this.onFocus(event);
		}
	}, {
		key: 'handleTouchEnd',
		value: function handleTouchEnd(event) {
			// Check if the view is being dragged, In this case
			// we don't want to fire the click event (because the user only wants to scroll)
			if (this.dragging) return;

			this.handleMouseDown(event);
		}
	}, {
		key: 'handleTouchMove',
		value: function handleTouchMove() {
			// Set a flag that the view is being dragged
			this.dragging = true;
		}
	}, {
		key: 'handleTouchStart',
		value: function handleTouchStart() {
			// Set a flag that the view is not being dragged
			this.dragging = false;
		}
	}, {
		key: 'onFocus',
		value: function onFocus(event) {
			if (!this.props.isFocused) {
				this.props.onFocus(this.props.option, event);
			}
		}
	}, {
		key: 'render',
		value: function render() {
			var _props = this.props,
			    option = _props.option,
			    instancePrefix = _props.instancePrefix,
			    optionIndex = _props.optionIndex;

			var className = __WEBPACK_IMPORTED_MODULE_1_classnames___default()(this.props.className, option.className);

			return option.disabled ? __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'div',
				{ className: className,
					onMouseDown: blockEvent,
					onClick: blockEvent },
				this.props.children
			) : __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'div',
				{ className: className,
					style: option.style,
					role: 'option',
					'aria-label': option.label,
					onMouseDown: this.handleMouseDown,
					onMouseEnter: this.handleMouseEnter,
					onMouseMove: this.handleMouseMove,
					onTouchStart: this.handleTouchStart,
					onTouchMove: this.handleTouchMove,
					onTouchEnd: this.handleTouchEnd,
					id: instancePrefix + '-option-' + optionIndex,
					title: option.title },
				this.props.children
			);
		}
	}]);
	return Option;
}(__WEBPACK_IMPORTED_MODULE_3_react___default.a.Component);

Option.propTypes = {
	children: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node,
	className: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // className (based on mouse position)
	instancePrefix: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string.isRequired, // unique prefix for the ids (used for aria)
	isDisabled: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // the option is disabled
	isFocused: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // the option is focused
	isSelected: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // the option is selected
	onFocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // method to handle mouseEnter on option element
	onSelect: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // method to handle click on option element
	onUnfocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // method to handle mouseLeave on option element
	option: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object.isRequired, // object that is base for that option
	optionIndex: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.number // index of the option, used to generate unique ids for aria
};

var Value = function (_React$Component) {
	inherits(Value, _React$Component);

	function Value(props) {
		classCallCheck(this, Value);

		var _this = possibleConstructorReturn(this, (Value.__proto__ || Object.getPrototypeOf(Value)).call(this, props));

		_this.handleMouseDown = _this.handleMouseDown.bind(_this);
		_this.onRemove = _this.onRemove.bind(_this);
		_this.handleTouchEndRemove = _this.handleTouchEndRemove.bind(_this);
		_this.handleTouchMove = _this.handleTouchMove.bind(_this);
		_this.handleTouchStart = _this.handleTouchStart.bind(_this);
		return _this;
	}

	createClass(Value, [{
		key: 'handleMouseDown',
		value: function handleMouseDown(event) {
			if (event.type === 'mousedown' && event.button !== 0) {
				return;
			}
			if (this.props.onClick) {
				event.stopPropagation();
				this.props.onClick(this.props.value, event);
				return;
			}
			if (this.props.value.href) {
				event.stopPropagation();
			}
		}
	}, {
		key: 'onRemove',
		value: function onRemove(event) {
			event.preventDefault();
			event.stopPropagation();
			this.props.onRemove(this.props.value);
		}
	}, {
		key: 'handleTouchEndRemove',
		value: function handleTouchEndRemove(event) {
			// Check if the view is being dragged, In this case
			// we don't want to fire the click event (because the user only wants to scroll)
			if (this.dragging) return;

			// Fire the mouse events
			this.onRemove(event);
		}
	}, {
		key: 'handleTouchMove',
		value: function handleTouchMove() {
			// Set a flag that the view is being dragged
			this.dragging = true;
		}
	}, {
		key: 'handleTouchStart',
		value: function handleTouchStart() {
			// Set a flag that the view is not being dragged
			this.dragging = false;
		}
	}, {
		key: 'renderRemoveIcon',
		value: function renderRemoveIcon() {
			if (this.props.disabled || !this.props.onRemove) return;
			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'span',
				{ className: 'Select-value-icon',
					'aria-hidden': 'true',
					onMouseDown: this.onRemove,
					onTouchEnd: this.handleTouchEndRemove,
					onTouchStart: this.handleTouchStart,
					onTouchMove: this.handleTouchMove },
				'\xD7'
			);
		}
	}, {
		key: 'renderLabel',
		value: function renderLabel() {
			var className = 'Select-value-label';
			return this.props.onClick || this.props.value.href ? __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'a',
				{ className: className, href: this.props.value.href, target: this.props.value.target, onMouseDown: this.handleMouseDown, onTouchEnd: this.handleMouseDown },
				this.props.children
			) : __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'span',
				{ className: className, role: 'option', 'aria-selected': 'true', id: this.props.id },
				this.props.children
			);
		}
	}, {
		key: 'render',
		value: function render() {
			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'div',
				{ className: __WEBPACK_IMPORTED_MODULE_1_classnames___default()('Select-value', this.props.value.className),
					style: this.props.value.style,
					title: this.props.value.title
				},
				this.renderRemoveIcon(),
				this.renderLabel()
			);
		}
	}]);
	return Value;
}(__WEBPACK_IMPORTED_MODULE_3_react___default.a.Component);

Value.propTypes = {
	children: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node,
	disabled: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // disabled prop passed to ReactSelect
	id: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // Unique id for the value - used for aria
	onClick: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // method to handle click on value label
	onRemove: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // method to handle removal of the value
	value: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object.isRequired // the option object for this value
};

/*!
  Copyright (c) 2018 Jed Watson.
  Licensed under the MIT License (MIT), see
  http://jedwatson.github.io/react-select
*/
var stringifyValue = function stringifyValue(value) {
	return typeof value === 'string' ? value : value !== null && JSON.stringify(value) || '';
};

var stringOrNode = __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.oneOfType([__WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node]);
var stringOrNumber = __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.oneOfType([__WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.number]);

var instanceId = 1;

var shouldShowValue = function shouldShowValue(state, props) {
	var inputValue = state.inputValue,
	    isPseudoFocused = state.isPseudoFocused,
	    isFocused = state.isFocused;
	var onSelectResetsInput = props.onSelectResetsInput;


	if (!inputValue) return true;

	if (!onSelectResetsInput) {
		return !(!isFocused && isPseudoFocused || isFocused && !isPseudoFocused);
	}

	return false;
};

var shouldShowPlaceholder = function shouldShowPlaceholder(state, props, isOpen) {
	var inputValue = state.inputValue,
	    isPseudoFocused = state.isPseudoFocused,
	    isFocused = state.isFocused;
	var onSelectResetsInput = props.onSelectResetsInput;


	return !inputValue || !onSelectResetsInput && !isOpen && !isPseudoFocused && !isFocused;
};

/**
 * Retrieve a value from the given options and valueKey
 * @param {String|Number|Array} value	- the selected value(s)
 * @param {Object}		 props	- the Select component's props (or nextProps)
 */
var expandValue = function expandValue(value, props) {
	var valueType = typeof value === 'undefined' ? 'undefined' : _typeof(value);
	if (valueType !== 'string' && valueType !== 'number' && valueType !== 'boolean') return value;
	var options = props.options,
	    valueKey = props.valueKey;

	if (!options) return;
	for (var i = 0; i < options.length; i++) {
		if (String(options[i][valueKey]) === String(value)) return options[i];
	}
};

var handleRequired = function handleRequired(value, multi) {
	if (!value) return true;
	return multi ? value.length === 0 : Object.keys(value).length === 0;
};

var Select$1 = function (_React$Component) {
	inherits(Select, _React$Component);

	function Select(props) {
		classCallCheck(this, Select);

		var _this = possibleConstructorReturn(this, (Select.__proto__ || Object.getPrototypeOf(Select)).call(this, props));

		['clearValue', 'focusOption', 'getOptionLabel', 'handleInputBlur', 'handleInputChange', 'handleInputFocus', 'handleInputValueChange', 'handleKeyDown', 'handleMenuScroll', 'handleMouseDown', 'handleMouseDownOnArrow', 'handleMouseDownOnMenu', 'handleTouchEnd', 'handleTouchEndClearValue', 'handleTouchMove', 'handleTouchOutside', 'handleTouchStart', 'handleValueClick', 'onOptionRef', 'removeValue', 'selectValue'].forEach(function (fn) {
			return _this[fn] = _this[fn].bind(_this);
		});

		_this.state = {
			inputValue: '',
			isFocused: false,
			isOpen: false,
			isPseudoFocused: false,
			required: false
		};
		return _this;
	}

	createClass(Select, [{
		key: 'componentWillMount',
		value: function componentWillMount() {
			this._instancePrefix = 'react-select-' + (this.props.instanceId || ++instanceId) + '-';
			var valueArray = this.getValueArray(this.props.value);

			if (this.props.required) {
				this.setState({
					required: handleRequired(valueArray[0], this.props.multi)
				});
			}
		}
	}, {
		key: 'componentDidMount',
		value: function componentDidMount() {
			if (typeof this.props.autofocus !== 'undefined' && typeof console !== 'undefined') {
				console.warn('Warning: The autofocus prop has changed to autoFocus, support will be removed after react-select@1.0');
			}
			if (this.props.autoFocus || this.props.autofocus) {
				this.focus();
			}
		}
	}, {
		key: 'componentWillReceiveProps',
		value: function componentWillReceiveProps(nextProps) {
			var valueArray = this.getValueArray(nextProps.value, nextProps);

			if (nextProps.required) {
				this.setState({
					required: handleRequired(valueArray[0], nextProps.multi)
				});
			} else if (this.props.required) {
				// Used to be required but it's not any more
				this.setState({ required: false });
			}

			if (this.state.inputValue && this.props.value !== nextProps.value && nextProps.onSelectResetsInput) {
				this.setState({ inputValue: this.handleInputValueChange('') });
			}
		}
	}, {
		key: 'componentDidUpdate',
		value: function componentDidUpdate(prevProps, prevState) {
			// focus to the selected option
			if (this.menu && this.focused && this.state.isOpen && !this.hasScrolledToOption) {
				var focusedOptionNode = Object(__WEBPACK_IMPORTED_MODULE_4_react_dom__["findDOMNode"])(this.focused);
				var menuNode = Object(__WEBPACK_IMPORTED_MODULE_4_react_dom__["findDOMNode"])(this.menu);

				var scrollTop = menuNode.scrollTop;
				var scrollBottom = scrollTop + menuNode.offsetHeight;
				var optionTop = focusedOptionNode.offsetTop;
				var optionBottom = optionTop + focusedOptionNode.offsetHeight;

				if (scrollTop > optionTop || scrollBottom < optionBottom) {
					menuNode.scrollTop = focusedOptionNode.offsetTop;
				}

				// We still set hasScrolledToOption to true even if we didn't
				// actually need to scroll, as we've still confirmed that the
				// option is in view.
				this.hasScrolledToOption = true;
			} else if (!this.state.isOpen) {
				this.hasScrolledToOption = false;
			}

			if (this._scrollToFocusedOptionOnUpdate && this.focused && this.menu) {
				this._scrollToFocusedOptionOnUpdate = false;
				var focusedDOM = Object(__WEBPACK_IMPORTED_MODULE_4_react_dom__["findDOMNode"])(this.focused);
				var menuDOM = Object(__WEBPACK_IMPORTED_MODULE_4_react_dom__["findDOMNode"])(this.menu);
				var focusedRect = focusedDOM.getBoundingClientRect();
				var menuRect = menuDOM.getBoundingClientRect();
				if (focusedRect.bottom > menuRect.bottom) {
					menuDOM.scrollTop = focusedDOM.offsetTop + focusedDOM.clientHeight - menuDOM.offsetHeight;
				} else if (focusedRect.top < menuRect.top) {
					menuDOM.scrollTop = focusedDOM.offsetTop;
				}
			}
			if (this.props.scrollMenuIntoView && this.menuContainer) {
				var menuContainerRect = this.menuContainer.getBoundingClientRect();
				if (window.innerHeight < menuContainerRect.bottom + this.props.menuBuffer) {
					window.scrollBy(0, menuContainerRect.bottom + this.props.menuBuffer - window.innerHeight);
				}
			}
			if (prevProps.disabled !== this.props.disabled) {
				this.setState({ isFocused: false }); // eslint-disable-line react/no-did-update-set-state
				this.closeMenu();
			}
			if (prevState.isOpen !== this.state.isOpen) {
				this.toggleTouchOutsideEvent(this.state.isOpen);
				var handler = this.state.isOpen ? this.props.onOpen : this.props.onClose;
				handler && handler();
			}
		}
	}, {
		key: 'componentWillUnmount',
		value: function componentWillUnmount() {
			this.toggleTouchOutsideEvent(false);
		}
	}, {
		key: 'toggleTouchOutsideEvent',
		value: function toggleTouchOutsideEvent(enabled) {
			if (enabled) {
				if (!document.addEventListener && document.attachEvent) {
					document.attachEvent('ontouchstart', this.handleTouchOutside);
				} else {
					document.addEventListener('touchstart', this.handleTouchOutside);
				}
			} else {
				if (!document.removeEventListener && document.detachEvent) {
					document.detachEvent('ontouchstart', this.handleTouchOutside);
				} else {
					document.removeEventListener('touchstart', this.handleTouchOutside);
				}
			}
		}
	}, {
		key: 'handleTouchOutside',
		value: function handleTouchOutside(event) {
			// handle touch outside on ios to dismiss menu
			if (this.wrapper && !this.wrapper.contains(event.target)) {
				this.closeMenu();
			}
		}
	}, {
		key: 'focus',
		value: function focus() {
			if (!this.input) return;
			this.input.focus();
		}
	}, {
		key: 'blurInput',
		value: function blurInput() {
			if (!this.input) return;
			this.input.blur();
		}
	}, {
		key: 'handleTouchMove',
		value: function handleTouchMove() {
			// Set a flag that the view is being dragged
			this.dragging = true;
		}
	}, {
		key: 'handleTouchStart',
		value: function handleTouchStart() {
			// Set a flag that the view is not being dragged
			this.dragging = false;
		}
	}, {
		key: 'handleTouchEnd',
		value: function handleTouchEnd(event) {
			// Check if the view is being dragged, In this case
			// we don't want to fire the click event (because the user only wants to scroll)
			if (this.dragging) return;

			// Fire the mouse events
			this.handleMouseDown(event);
		}
	}, {
		key: 'handleTouchEndClearValue',
		value: function handleTouchEndClearValue(event) {
			// Check if the view is being dragged, In this case
			// we don't want to fire the click event (because the user only wants to scroll)
			if (this.dragging) return;

			// Clear the value
			this.clearValue(event);
		}
	}, {
		key: 'handleMouseDown',
		value: function handleMouseDown(event) {
			// if the event was triggered by a mousedown and not the primary
			// button, or if the component is disabled, ignore it.
			if (this.props.disabled || event.type === 'mousedown' && event.button !== 0) {
				return;
			}

			if (event.target.tagName === 'INPUT') {
				if (!this.state.isFocused) {
					this._openAfterFocus = this.props.openOnClick;
					this.focus();
				} else if (!this.state.isOpen) {
					this.setState({
						isOpen: true,
						isPseudoFocused: false
					});
				}

				return;
			}

			// prevent default event handlers
			event.preventDefault();

			// for the non-searchable select, toggle the menu
			if (!this.props.searchable) {
				// This code means that if a select is searchable, onClick the options menu will not appear, only on subsequent click will it open.
				this.focus();
				return this.setState({
					isOpen: !this.state.isOpen
				});
			}

			if (this.state.isFocused) {
				// On iOS, we can get into a state where we think the input is focused but it isn't really,
				// since iOS ignores programmatic calls to input.focus() that weren't triggered by a click event.
				// Call focus() again here to be safe.
				this.focus();

				var input = this.input;
				var toOpen = true;

				if (typeof input.getInput === 'function') {
					// Get the actual DOM input if the ref is an <AutosizeInput /> component
					input = input.getInput();
				}

				// clears the value so that the cursor will be at the end of input when the component re-renders
				input.value = '';

				if (this._focusAfterClear) {
					toOpen = false;
					this._focusAfterClear = false;
				}

				// if the input is focused, ensure the menu is open
				this.setState({
					isOpen: toOpen,
					isPseudoFocused: false,
					focusedOption: null
				});
			} else {
				// otherwise, focus the input and open the menu
				this._openAfterFocus = this.props.openOnClick;
				this.focus();
				this.setState({ focusedOption: null });
			}
		}
	}, {
		key: 'handleMouseDownOnArrow',
		value: function handleMouseDownOnArrow(event) {
			// if the event was triggered by a mousedown and not the primary
			// button, or if the component is disabled, ignore it.
			if (this.props.disabled || event.type === 'mousedown' && event.button !== 0) {
				return;
			}

			if (this.state.isOpen) {
				// prevent default event handlers
				event.stopPropagation();
				event.preventDefault();
				// close the menu
				this.closeMenu();
			} else {
				// If the menu isn't open, let the event bubble to the main handleMouseDown
				this.setState({
					isOpen: true
				});
			}
		}
	}, {
		key: 'handleMouseDownOnMenu',
		value: function handleMouseDownOnMenu(event) {
			// if the event was triggered by a mousedown and not the primary
			// button, or if the component is disabled, ignore it.
			if (this.props.disabled || event.type === 'mousedown' && event.button !== 0) {
				return;
			}

			event.stopPropagation();
			event.preventDefault();

			this._openAfterFocus = true;
			this.focus();
		}
	}, {
		key: 'closeMenu',
		value: function closeMenu() {
			if (this.props.onCloseResetsInput) {
				this.setState({
					inputValue: this.handleInputValueChange(''),
					isOpen: false,
					isPseudoFocused: this.state.isFocused && !this.props.multi
				});
			} else {
				this.setState({
					isOpen: false,
					isPseudoFocused: this.state.isFocused && !this.props.multi
				});
			}
			this.hasScrolledToOption = false;
		}
	}, {
		key: 'handleInputFocus',
		value: function handleInputFocus(event) {
			if (this.props.disabled) return;

			var toOpen = this.state.isOpen || this._openAfterFocus || this.props.openOnFocus;
			toOpen = this._focusAfterClear ? false : toOpen; //if focus happens after clear values, don't open dropdown yet.

			if (this.props.onFocus) {
				this.props.onFocus(event);
			}

			this.setState({
				isFocused: true,
				isOpen: !!toOpen
			});

			this._focusAfterClear = false;
			this._openAfterFocus = false;
		}
	}, {
		key: 'handleInputBlur',
		value: function handleInputBlur(event) {
			// The check for menu.contains(activeElement) is necessary to prevent IE11's scrollbar from closing the menu in certain contexts.
			if (this.menu && (this.menu === document.activeElement || this.menu.contains(document.activeElement))) {
				this.focus();
				return;
			}

			if (this.props.onBlur) {
				this.props.onBlur(event);
			}
			var onBlurredState = {
				isFocused: false,
				isOpen: false,
				isPseudoFocused: false
			};
			if (this.props.onBlurResetsInput) {
				onBlurredState.inputValue = this.handleInputValueChange('');
			}
			this.setState(onBlurredState);
		}
	}, {
		key: 'handleInputChange',
		value: function handleInputChange(event) {
			var newInputValue = event.target.value;

			if (this.state.inputValue !== event.target.value) {
				newInputValue = this.handleInputValueChange(newInputValue);
			}

			this.setState({
				inputValue: newInputValue,
				isOpen: true,
				isPseudoFocused: false
			});
		}
	}, {
		key: 'setInputValue',
		value: function setInputValue(newValue) {
			if (this.props.onInputChange) {
				var nextState = this.props.onInputChange(newValue);
				if (nextState != null && (typeof nextState === 'undefined' ? 'undefined' : _typeof(nextState)) !== 'object') {
					newValue = '' + nextState;
				}
			}
			this.setState({
				inputValue: newValue
			});
		}
	}, {
		key: 'handleInputValueChange',
		value: function handleInputValueChange(newValue) {
			if (this.props.onInputChange) {
				var nextState = this.props.onInputChange(newValue);
				// Note: != used deliberately here to catch undefined and null
				if (nextState != null && (typeof nextState === 'undefined' ? 'undefined' : _typeof(nextState)) !== 'object') {
					newValue = '' + nextState;
				}
			}
			return newValue;
		}
	}, {
		key: 'handleKeyDown',
		value: function handleKeyDown(event) {
			if (this.props.disabled) return;

			if (typeof this.props.onInputKeyDown === 'function') {
				this.props.onInputKeyDown(event);
				if (event.defaultPrevented) {
					return;
				}
			}

			switch (event.keyCode) {
				case 8:
					// backspace
					if (!this.state.inputValue && this.props.backspaceRemoves) {
						event.preventDefault();
						this.popValue();
					}
					break;
				case 9:
					// tab
					if (event.shiftKey || !this.state.isOpen || !this.props.tabSelectsValue) {
						break;
					}
					event.preventDefault();
					this.selectFocusedOption();
					break;
				case 13:
					// enter
					event.preventDefault();
					event.stopPropagation();
					if (this.state.isOpen) {
						this.selectFocusedOption();
					} else {
						this.focusNextOption();
					}
					break;
				case 27:
					// escape
					event.preventDefault();
					if (this.state.isOpen) {
						this.closeMenu();
						event.stopPropagation();
					} else if (this.props.clearable && this.props.escapeClearsValue) {
						this.clearValue(event);
						event.stopPropagation();
					}
					break;
				case 32:
					// space
					if (this.props.searchable) {
						break;
					}
					event.preventDefault();
					if (!this.state.isOpen) {
						this.focusNextOption();
						break;
					}
					event.stopPropagation();
					this.selectFocusedOption();
					break;
				case 38:
					// up
					event.preventDefault();
					this.focusPreviousOption();
					break;
				case 40:
					// down
					event.preventDefault();
					this.focusNextOption();
					break;
				case 33:
					// page up
					event.preventDefault();
					this.focusPageUpOption();
					break;
				case 34:
					// page down
					event.preventDefault();
					this.focusPageDownOption();
					break;
				case 35:
					// end key
					if (event.shiftKey) {
						break;
					}
					event.preventDefault();
					this.focusEndOption();
					break;
				case 36:
					// home key
					if (event.shiftKey) {
						break;
					}
					event.preventDefault();
					this.focusStartOption();
					break;
				case 46:
					// delete
					if (!this.state.inputValue && this.props.deleteRemoves) {
						event.preventDefault();
						this.popValue();
					}
					break;
			}
		}
	}, {
		key: 'handleValueClick',
		value: function handleValueClick(option, event) {
			if (!this.props.onValueClick) return;
			this.props.onValueClick(option, event);
		}
	}, {
		key: 'handleMenuScroll',
		value: function handleMenuScroll(event) {
			if (!this.props.onMenuScrollToBottom) return;
			var target = event.target;

			if (target.scrollHeight > target.offsetHeight && target.scrollHeight - target.offsetHeight - target.scrollTop <= 0) {
				this.props.onMenuScrollToBottom();
			}
		}
	}, {
		key: 'getOptionLabel',
		value: function getOptionLabel(op) {
			return op[this.props.labelKey];
		}

		/**
   * Turns a value into an array from the given options
   * @param {String|Number|Array} value		- the value of the select input
   * @param {Object}		nextProps	- optionally specify the nextProps so the returned array uses the latest configuration
   * @returns	{Array}	the value of the select represented in an array
   */

	}, {
		key: 'getValueArray',
		value: function getValueArray(value) {
			var nextProps = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : undefined;

			/** support optionally passing in the `nextProps` so `componentWillReceiveProps` updates will function as expected */
			var props = (typeof nextProps === 'undefined' ? 'undefined' : _typeof(nextProps)) === 'object' ? nextProps : this.props;
			if (props.multi) {
				if (typeof value === 'string') {
					value = value.split(props.delimiter);
				}
				if (!Array.isArray(value)) {
					if (value === null || value === undefined) return [];
					value = [value];
				}
				return value.map(function (value) {
					return expandValue(value, props);
				}).filter(function (i) {
					return i;
				});
			}
			var expandedValue = expandValue(value, props);
			return expandedValue ? [expandedValue] : [];
		}
	}, {
		key: 'setValue',
		value: function setValue(value) {
			var _this2 = this;

			if (this.props.autoBlur) {
				this.blurInput();
			}
			if (this.props.required) {
				var required = handleRequired(value, this.props.multi);
				this.setState({ required: required });
			}
			if (this.props.simpleValue && value) {
				value = this.props.multi ? value.map(function (i) {
					return i[_this2.props.valueKey];
				}).join(this.props.delimiter) : value[this.props.valueKey];
			}
			if (this.props.onChange) {
				this.props.onChange(value);
			}
		}
	}, {
		key: 'selectValue',
		value: function selectValue(value) {
			var _this3 = this;

			// NOTE: we actually add/set the value in a callback to make sure the
			// input value is empty to avoid styling issues in Chrome
			if (this.props.closeOnSelect) {
				this.hasScrolledToOption = false;
			}
			var updatedValue = this.props.onSelectResetsInput ? '' : this.state.inputValue;
			if (this.props.multi) {
				this.setState({
					focusedIndex: null,
					inputValue: this.handleInputValueChange(updatedValue),
					isOpen: !this.props.closeOnSelect
				}, function () {
					var valueArray = _this3.getValueArray(_this3.props.value);
					if (valueArray.some(function (i) {
						return i[_this3.props.valueKey] === value[_this3.props.valueKey];
					})) {
						_this3.removeValue(value);
					} else {
						_this3.addValue(value);
					}
				});
			} else {
				this.setState({
					inputValue: this.handleInputValueChange(updatedValue),
					isOpen: !this.props.closeOnSelect,
					isPseudoFocused: this.state.isFocused
				}, function () {
					_this3.setValue(value);
				});
			}
		}
	}, {
		key: 'addValue',
		value: function addValue(value) {
			var valueArray = this.getValueArray(this.props.value);
			var visibleOptions = this._visibleOptions.filter(function (val) {
				return !val.disabled;
			});
			var lastValueIndex = visibleOptions.indexOf(value);
			this.setValue(valueArray.concat(value));
			if (visibleOptions.length - 1 === lastValueIndex) {
				// the last option was selected; focus the second-last one
				this.focusOption(visibleOptions[lastValueIndex - 1]);
			} else if (visibleOptions.length > lastValueIndex) {
				// focus the option below the selected one
				this.focusOption(visibleOptions[lastValueIndex + 1]);
			}
		}
	}, {
		key: 'popValue',
		value: function popValue() {
			var valueArray = this.getValueArray(this.props.value);
			if (!valueArray.length) return;
			if (valueArray[valueArray.length - 1].clearableValue === false) return;
			this.setValue(this.props.multi ? valueArray.slice(0, valueArray.length - 1) : null);
		}
	}, {
		key: 'removeValue',
		value: function removeValue(value) {
			var _this4 = this;

			var valueArray = this.getValueArray(this.props.value);
			this.setValue(valueArray.filter(function (i) {
				return i[_this4.props.valueKey] !== value[_this4.props.valueKey];
			}));
			this.focus();
		}
	}, {
		key: 'clearValue',
		value: function clearValue(event) {
			// if the event was triggered by a mousedown and not the primary
			// button, ignore it.
			if (event && event.type === 'mousedown' && event.button !== 0) {
				return;
			}

			event.preventDefault();

			this.setValue(this.getResetValue());
			this.setState({
				inputValue: this.handleInputValueChange(''),
				isOpen: false
			}, this.focus);

			this._focusAfterClear = true;
		}
	}, {
		key: 'getResetValue',
		value: function getResetValue() {
			if (this.props.resetValue !== undefined) {
				return this.props.resetValue;
			} else if (this.props.multi) {
				return [];
			} else {
				return null;
			}
		}
	}, {
		key: 'focusOption',
		value: function focusOption(option) {
			this.setState({
				focusedOption: option
			});
		}
	}, {
		key: 'focusNextOption',
		value: function focusNextOption() {
			this.focusAdjacentOption('next');
		}
	}, {
		key: 'focusPreviousOption',
		value: function focusPreviousOption() {
			this.focusAdjacentOption('previous');
		}
	}, {
		key: 'focusPageUpOption',
		value: function focusPageUpOption() {
			this.focusAdjacentOption('page_up');
		}
	}, {
		key: 'focusPageDownOption',
		value: function focusPageDownOption() {
			this.focusAdjacentOption('page_down');
		}
	}, {
		key: 'focusStartOption',
		value: function focusStartOption() {
			this.focusAdjacentOption('start');
		}
	}, {
		key: 'focusEndOption',
		value: function focusEndOption() {
			this.focusAdjacentOption('end');
		}
	}, {
		key: 'focusAdjacentOption',
		value: function focusAdjacentOption(dir) {
			var options = this._visibleOptions.map(function (option, index) {
				return { option: option, index: index };
			}).filter(function (option) {
				return !option.option.disabled;
			});
			this._scrollToFocusedOptionOnUpdate = true;
			if (!this.state.isOpen) {
				var newState = {
					focusedOption: this._focusedOption || (options.length ? options[dir === 'next' ? 0 : options.length - 1].option : null),
					isOpen: true
				};
				if (this.props.onSelectResetsInput) {
					newState.inputValue = '';
				}
				this.setState(newState);
				return;
			}
			if (!options.length) return;
			var focusedIndex = -1;
			for (var i = 0; i < options.length; i++) {
				if (this._focusedOption === options[i].option) {
					focusedIndex = i;
					break;
				}
			}
			if (dir === 'next' && focusedIndex !== -1) {
				focusedIndex = (focusedIndex + 1) % options.length;
			} else if (dir === 'previous') {
				if (focusedIndex > 0) {
					focusedIndex = focusedIndex - 1;
				} else {
					focusedIndex = options.length - 1;
				}
			} else if (dir === 'start') {
				focusedIndex = 0;
			} else if (dir === 'end') {
				focusedIndex = options.length - 1;
			} else if (dir === 'page_up') {
				var potentialIndex = focusedIndex - this.props.pageSize;
				if (potentialIndex < 0) {
					focusedIndex = 0;
				} else {
					focusedIndex = potentialIndex;
				}
			} else if (dir === 'page_down') {
				var _potentialIndex = focusedIndex + this.props.pageSize;
				if (_potentialIndex > options.length - 1) {
					focusedIndex = options.length - 1;
				} else {
					focusedIndex = _potentialIndex;
				}
			}

			if (focusedIndex === -1) {
				focusedIndex = 0;
			}

			this.setState({
				focusedIndex: options[focusedIndex].index,
				focusedOption: options[focusedIndex].option
			});
		}
	}, {
		key: 'getFocusedOption',
		value: function getFocusedOption() {
			return this._focusedOption;
		}
	}, {
		key: 'selectFocusedOption',
		value: function selectFocusedOption() {
			if (this._focusedOption) {
				return this.selectValue(this._focusedOption);
			}
		}
	}, {
		key: 'renderLoading',
		value: function renderLoading() {
			if (!this.props.isLoading) return;
			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'span',
				{ className: 'Select-loading-zone', 'aria-hidden': 'true' },
				__WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('span', { className: 'Select-loading' })
			);
		}
	}, {
		key: 'renderValue',
		value: function renderValue(valueArray, isOpen) {
			var _this5 = this;

			var renderLabel = this.props.valueRenderer || this.getOptionLabel;
			var ValueComponent = this.props.valueComponent;
			if (!valueArray.length) {
				var showPlaceholder = shouldShowPlaceholder(this.state, this.props, isOpen);
				return showPlaceholder ? __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
					'div',
					{ className: 'Select-placeholder' },
					this.props.placeholder
				) : null;
			}
			var onClick = this.props.onValueClick ? this.handleValueClick : null;
			if (this.props.multi) {
				return valueArray.map(function (value, i) {
					return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
						ValueComponent,
						{
							disabled: _this5.props.disabled || value.clearableValue === false,
							id: _this5._instancePrefix + '-value-' + i,
							instancePrefix: _this5._instancePrefix,
							key: 'value-' + i + '-' + value[_this5.props.valueKey],
							onClick: onClick,
							onRemove: _this5.removeValue,
							placeholder: _this5.props.placeholder,
							value: value
						},
						renderLabel(value, i),
						__WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
							'span',
							{ className: 'Select-aria-only' },
							'\xA0'
						)
					);
				});
			} else if (shouldShowValue(this.state, this.props)) {
				if (isOpen) onClick = null;
				return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
					ValueComponent,
					{
						disabled: this.props.disabled,
						id: this._instancePrefix + '-value-item',
						instancePrefix: this._instancePrefix,
						onClick: onClick,
						placeholder: this.props.placeholder,
						value: valueArray[0]
					},
					renderLabel(valueArray[0])
				);
			}
		}
	}, {
		key: 'renderInput',
		value: function renderInput(valueArray, focusedOptionIndex) {
			var _classNames,
			    _this6 = this;

			var className = __WEBPACK_IMPORTED_MODULE_1_classnames___default()('Select-input', this.props.inputProps.className);
			var isOpen = this.state.isOpen;

			var ariaOwns = __WEBPACK_IMPORTED_MODULE_1_classnames___default()((_classNames = {}, defineProperty(_classNames, this._instancePrefix + '-list', isOpen), defineProperty(_classNames, this._instancePrefix + '-backspace-remove-message', this.props.multi && !this.props.disabled && this.state.isFocused && !this.state.inputValue), _classNames));

			var value = this.state.inputValue;
			if (value && !this.props.onSelectResetsInput && !this.state.isFocused) {
				// it hides input value when it is not focused and was not reset on select
				value = '';
			}

			var inputProps = _extends({}, this.props.inputProps, {
				'aria-activedescendant': isOpen ? this._instancePrefix + '-option-' + focusedOptionIndex : this._instancePrefix + '-value',
				'aria-describedby': this.props['aria-describedby'],
				'aria-expanded': '' + isOpen,
				'aria-haspopup': '' + isOpen,
				'aria-label': this.props['aria-label'],
				'aria-labelledby': this.props['aria-labelledby'],
				'aria-owns': ariaOwns,
				className: className,
				onBlur: this.handleInputBlur,
				onChange: this.handleInputChange,
				onFocus: this.handleInputFocus,
				ref: function ref(_ref) {
					return _this6.input = _ref;
				},
				role: 'combobox',
				required: this.state.required,
				tabIndex: this.props.tabIndex,
				value: value
			});

			if (this.props.inputRenderer) {
				return this.props.inputRenderer(inputProps);
			}

			if (this.props.disabled || !this.props.searchable) {
				var divProps = objectWithoutProperties(this.props.inputProps, []);


				var _ariaOwns = __WEBPACK_IMPORTED_MODULE_1_classnames___default()(defineProperty({}, this._instancePrefix + '-list', isOpen));
				return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('div', _extends({}, divProps, {
					'aria-expanded': isOpen,
					'aria-owns': _ariaOwns,
					'aria-activedescendant': isOpen ? this._instancePrefix + '-option-' + focusedOptionIndex : this._instancePrefix + '-value',
					'aria-disabled': '' + this.props.disabled,
					'aria-label': this.props['aria-label'],
					'aria-labelledby': this.props['aria-labelledby'],
					className: className,
					onBlur: this.handleInputBlur,
					onFocus: this.handleInputFocus,
					ref: function ref(_ref2) {
						return _this6.input = _ref2;
					},
					role: 'combobox',
					style: { border: 0, width: 1, display: 'inline-block' },
					tabIndex: this.props.tabIndex || 0
				}));
			}

			if (this.props.autosize) {
				return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(__WEBPACK_IMPORTED_MODULE_0_react_input_autosize___default.a, _extends({ id: this.props.id }, inputProps, { minWidth: '5' }));
			}
			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'div',
				{ className: className, key: 'input-wrap', style: { display: 'inline-block' } },
				__WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('input', _extends({ id: this.props.id }, inputProps))
			);
		}
	}, {
		key: 'renderClear',
		value: function renderClear() {
			var valueArray = this.getValueArray(this.props.value);
			if (!this.props.clearable || !valueArray.length || this.props.disabled || this.props.isLoading) return;
			var ariaLabel = this.props.multi ? this.props.clearAllText : this.props.clearValueText;
			var clear = this.props.clearRenderer();

			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'span',
				{
					'aria-label': ariaLabel,
					className: 'Select-clear-zone',
					onMouseDown: this.clearValue,
					onTouchEnd: this.handleTouchEndClearValue,
					onTouchMove: this.handleTouchMove,
					onTouchStart: this.handleTouchStart,
					title: ariaLabel
				},
				clear
			);
		}
	}, {
		key: 'renderArrow',
		value: function renderArrow() {
			if (!this.props.arrowRenderer) return;

			var onMouseDown = this.handleMouseDownOnArrow;
			var isOpen = this.state.isOpen;
			var arrow = this.props.arrowRenderer({ onMouseDown: onMouseDown, isOpen: isOpen });

			if (!arrow) {
				return null;
			}

			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'span',
				{
					className: 'Select-arrow-zone',
					onMouseDown: onMouseDown
				},
				arrow
			);
		}
	}, {
		key: 'filterOptions',
		value: function filterOptions$$1(excludeOptions) {
			var filterValue = this.state.inputValue;
			var options = this.props.options || [];
			if (this.props.filterOptions) {
				// Maintain backwards compatibility with boolean attribute
				var filterOptions$$1 = typeof this.props.filterOptions === 'function' ? this.props.filterOptions : filterOptions;

				return filterOptions$$1(options, filterValue, excludeOptions, {
					filterOption: this.props.filterOption,
					ignoreAccents: this.props.ignoreAccents,
					ignoreCase: this.props.ignoreCase,
					labelKey: this.props.labelKey,
					matchPos: this.props.matchPos,
					matchProp: this.props.matchProp,
					trimFilter: this.props.trimFilter,
					valueKey: this.props.valueKey
				});
			} else {
				return options;
			}
		}
	}, {
		key: 'onOptionRef',
		value: function onOptionRef(ref, isFocused) {
			if (isFocused) {
				this.focused = ref;
			}
		}
	}, {
		key: 'renderMenu',
		value: function renderMenu(options, valueArray, focusedOption) {
			if (options && options.length) {
				return this.props.menuRenderer({
					focusedOption: focusedOption,
					focusOption: this.focusOption,
					inputValue: this.state.inputValue,
					instancePrefix: this._instancePrefix,
					labelKey: this.props.labelKey,
					onFocus: this.focusOption,
					onOptionRef: this.onOptionRef,
					onSelect: this.selectValue,
					optionClassName: this.props.optionClassName,
					optionComponent: this.props.optionComponent,
					optionRenderer: this.props.optionRenderer || this.getOptionLabel,
					options: options,
					removeValue: this.removeValue,
					selectValue: this.selectValue,
					valueArray: valueArray,
					valueKey: this.props.valueKey
				});
			} else if (this.props.noResultsText) {
				return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
					'div',
					{ className: 'Select-noresults' },
					this.props.noResultsText
				);
			} else {
				return null;
			}
		}
	}, {
		key: 'renderHiddenField',
		value: function renderHiddenField(valueArray) {
			var _this7 = this;

			if (!this.props.name) return;
			if (this.props.joinValues) {
				var value = valueArray.map(function (i) {
					return stringifyValue(i[_this7.props.valueKey]);
				}).join(this.props.delimiter);
				return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('input', {
					disabled: this.props.disabled,
					name: this.props.name,
					ref: function ref(_ref3) {
						return _this7.value = _ref3;
					},
					type: 'hidden',
					value: value
				});
			}
			return valueArray.map(function (item, index) {
				return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement('input', {
					disabled: _this7.props.disabled,
					key: 'hidden.' + index,
					name: _this7.props.name,
					ref: 'value' + index,
					type: 'hidden',
					value: stringifyValue(item[_this7.props.valueKey])
				});
			});
		}
	}, {
		key: 'getFocusableOptionIndex',
		value: function getFocusableOptionIndex(selectedOption) {
			var options = this._visibleOptions;
			if (!options.length) return null;

			var valueKey = this.props.valueKey;
			var focusedOption = this.state.focusedOption || selectedOption;
			if (focusedOption && !focusedOption.disabled) {
				var focusedOptionIndex = -1;
				options.some(function (option, index) {
					var isOptionEqual = option[valueKey] === focusedOption[valueKey];
					if (isOptionEqual) {
						focusedOptionIndex = index;
					}
					return isOptionEqual;
				});
				if (focusedOptionIndex !== -1) {
					return focusedOptionIndex;
				}
			}

			for (var i = 0; i < options.length; i++) {
				if (!options[i].disabled) return i;
			}
			return null;
		}
	}, {
		key: 'renderOuter',
		value: function renderOuter(options, valueArray, focusedOption) {
			var _this8 = this;

			var menu = this.renderMenu(options, valueArray, focusedOption);
			if (!menu) {
				return null;
			}

			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'div',
				{ ref: function ref(_ref5) {
						return _this8.menuContainer = _ref5;
					}, className: 'Select-menu-outer', style: this.props.menuContainerStyle },
				__WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
					'div',
					{
						className: 'Select-menu',
						id: this._instancePrefix + '-list',
						onMouseDown: this.handleMouseDownOnMenu,
						onScroll: this.handleMenuScroll,
						ref: function ref(_ref4) {
							return _this8.menu = _ref4;
						},
						role: 'listbox',
						style: this.props.menuStyle,
						tabIndex: -1
					},
					menu
				)
			);
		}
	}, {
		key: 'render',
		value: function render() {
			var _this9 = this;

			var valueArray = this.getValueArray(this.props.value);
			var options = this._visibleOptions = this.filterOptions(this.props.multi && this.props.removeSelected ? valueArray : null);
			var isOpen = this.state.isOpen;
			if (this.props.multi && !options.length && valueArray.length && !this.state.inputValue) isOpen = false;
			var focusedOptionIndex = this.getFocusableOptionIndex(valueArray[0]);

			var focusedOption = null;
			if (focusedOptionIndex !== null) {
				focusedOption = this._focusedOption = options[focusedOptionIndex];
			} else {
				focusedOption = this._focusedOption = null;
			}
			var className = __WEBPACK_IMPORTED_MODULE_1_classnames___default()('Select', this.props.className, {
				'has-value': valueArray.length,
				'is-clearable': this.props.clearable,
				'is-disabled': this.props.disabled,
				'is-focused': this.state.isFocused,
				'is-loading': this.props.isLoading,
				'is-open': isOpen,
				'is-pseudo-focused': this.state.isPseudoFocused,
				'is-searchable': this.props.searchable,
				'Select--multi': this.props.multi,
				'Select--rtl': this.props.rtl,
				'Select--single': !this.props.multi
			});

			var removeMessage = null;
			if (this.props.multi && !this.props.disabled && valueArray.length && !this.state.inputValue && this.state.isFocused && this.props.backspaceRemoves) {
				removeMessage = __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
					'span',
					{ id: this._instancePrefix + '-backspace-remove-message', className: 'Select-aria-only', 'aria-live': 'assertive' },
					this.props.backspaceToRemoveMessage.replace('{label}', valueArray[valueArray.length - 1][this.props.labelKey])
				);
			}

			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				'div',
				{ ref: function ref(_ref7) {
						return _this9.wrapper = _ref7;
					},
					className: className,
					style: this.props.wrapperStyle },
				this.renderHiddenField(valueArray),
				__WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
					'div',
					{ ref: function ref(_ref6) {
							return _this9.control = _ref6;
						},
						className: 'Select-control',
						onKeyDown: this.handleKeyDown,
						onMouseDown: this.handleMouseDown,
						onTouchEnd: this.handleTouchEnd,
						onTouchMove: this.handleTouchMove,
						onTouchStart: this.handleTouchStart,
						style: this.props.style
					},
					__WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
						'span',
						{ className: 'Select-multi-value-wrapper', id: this._instancePrefix + '-value' },
						this.renderValue(valueArray, isOpen),
						this.renderInput(valueArray, focusedOptionIndex)
					),
					removeMessage,
					this.renderLoading(),
					this.renderClear(),
					this.renderArrow()
				),
				isOpen ? this.renderOuter(options, valueArray, focusedOption) : null
			);
		}
	}]);
	return Select;
}(__WEBPACK_IMPORTED_MODULE_3_react___default.a.Component);

Select$1.propTypes = {
	'aria-describedby': __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // html id(s) of element(s) that should be used to describe this input (for assistive tech)
	'aria-label': __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // aria label (for assistive tech)
	'aria-labelledby': __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // html id of an element that should be used as the label (for assistive tech)
	arrowRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // create the drop-down caret element
	autoBlur: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // automatically blur the component when an option is selected
	autoFocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // autofocus the component on mount
	autofocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // deprecated; use autoFocus instead
	autosize: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to enable autosizing or not
	backspaceRemoves: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether backspace removes an item if there is no text input
	backspaceToRemoveMessage: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // message to use for screenreaders to press backspace to remove the current item - {label} is replaced with the item label
	className: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // className for the outer element
	clearAllText: stringOrNode, // title for the "clear" control when multi: true
	clearRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // create clearable x element
	clearValueText: stringOrNode, // title for the "clear" control
	clearable: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // should it be possible to reset value
	closeOnSelect: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to close the menu when a value is selected
	deleteRemoves: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether delete removes an item if there is no text input
	delimiter: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // delimiter to use to join multiple values for the hidden field value
	disabled: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether the Select is disabled or not
	escapeClearsValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether escape clears the value when the menu is closed
	filterOption: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // method to filter a single option (option, filterString)
	filterOptions: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any, // boolean to enable default filtering or function to filter the options array ([options], filterString, [values])
	id: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // html id to set on the input element for accessibility or tests
	ignoreAccents: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to strip diacritics when filtering
	ignoreCase: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to perform case-insensitive filtering
	inputProps: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object, // custom attributes for the Input
	inputRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // returns a custom input component
	instanceId: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // set the components instanceId
	isLoading: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether the Select is loading externally or not (such as options being loaded)
	joinValues: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // joins multiple values into a single form field with the delimiter (legacy mode)
	labelKey: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // path of the label value in option objects
	matchPos: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // (any|start) match the start or entire string when filtering
	matchProp: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // (any|label|value) which option property to filter on
	menuBuffer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.number, // optional buffer (in px) between the bottom of the viewport and the bottom of the menu
	menuContainerStyle: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object, // optional style to apply to the menu container
	menuRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // renders a custom menu with options
	menuStyle: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object, // optional style to apply to the menu
	multi: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // multi-value input
	name: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // generates a hidden <input /> tag with this field name for html forms
	noResultsText: stringOrNode, // placeholder displayed when there are no matching search results
	onBlur: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // onBlur handler: function (event) {}
	onBlurResetsInput: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether input is cleared on blur
	onChange: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // onChange handler: function (newValue) {}
	onClose: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // fires when the menu is closed
	onCloseResetsInput: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether input is cleared when menu is closed through the arrow
	onFocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // onFocus handler: function (event) {}
	onInputChange: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // onInputChange handler: function (inputValue) {}
	onInputKeyDown: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // input keyDown handler: function (event) {}
	onMenuScrollToBottom: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // fires when the menu is scrolled to the bottom; can be used to paginate options
	onOpen: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // fires when the menu is opened
	onSelectResetsInput: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether input is cleared on select (works only for multiselect)
	onValueClick: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // onClick handler for value labels: function (value, event) {}
	openOnClick: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // boolean to control opening the menu when the control is clicked
	openOnFocus: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // always open options menu on focus
	optionClassName: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // additional class(es) to apply to the <Option /> elements
	optionComponent: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // option component to render in dropdown
	optionRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // optionRenderer: function (option) {}
	options: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.array, // array of options
	pageSize: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.number, // number of entries to page when using page up/down keys
	placeholder: stringOrNode, // field placeholder, displayed when there's no value
	removeSelected: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether the selected option is removed from the dropdown on multi selects
	required: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // applies HTML5 required attribute when needed
	resetValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any, // value to use when you clear the control
	rtl: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // set to true in order to use react-select in right-to-left direction
	scrollMenuIntoView: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // boolean to enable the viewport to shift so that the full menu fully visible when engaged
	searchable: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to enable searching feature or not
	simpleValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // pass the value to onChange as a simple value (legacy pre 1.0 mode), defaults to false
	style: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object, // optional style to apply to the control
	tabIndex: stringOrNumber, // optional tab index of the control
	tabSelectsValue: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to treat tabbing out while focused to be value selection
	trimFilter: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // whether to trim whitespace around filter value
	value: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any, // initial field value
	valueComponent: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // value component to render
	valueKey: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, // path of the label value in option objects
	valueRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // valueRenderer: function (option) {}
	wrapperStyle: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.object // optional style to apply to the component wrapper
};

Select$1.defaultProps = {
	arrowRenderer: arrowRenderer,
	autosize: true,
	backspaceRemoves: true,
	backspaceToRemoveMessage: 'Press backspace to remove {label}',
	clearable: true,
	clearAllText: 'Clear all',
	clearRenderer: clearRenderer,
	clearValueText: 'Clear value',
	closeOnSelect: true,
	deleteRemoves: true,
	delimiter: ',',
	disabled: false,
	escapeClearsValue: true,
	filterOptions: filterOptions,
	ignoreAccents: true,
	ignoreCase: true,
	inputProps: {},
	isLoading: false,
	joinValues: false,
	labelKey: 'label',
	matchPos: 'any',
	matchProp: 'any',
	menuBuffer: 0,
	menuRenderer: menuRenderer,
	multi: false,
	noResultsText: 'No results found',
	onBlurResetsInput: true,
	onCloseResetsInput: true,
	onSelectResetsInput: true,
	openOnClick: true,
	optionComponent: Option,
	pageSize: 5,
	placeholder: 'Select...',
	removeSelected: true,
	required: false,
	rtl: false,
	scrollMenuIntoView: true,
	searchable: true,
	simpleValue: false,
	tabSelectsValue: true,
	trimFilter: true,
	valueComponent: Value,
	valueKey: 'value'
};

var propTypes = {
	autoload: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool.isRequired, // automatically call the `loadOptions` prop on-mount; defaults to true
	cache: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any, // object to use to cache results; set to null/false to disable caching
	children: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func.isRequired, // Child function responsible for creating the inner Select component; (props: Object): PropTypes.element
	ignoreAccents: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // strip diacritics when filtering; defaults to true
	ignoreCase: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // perform case-insensitive filtering; defaults to true
	loadOptions: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func.isRequired, // callback to load options asynchronously; (inputValue: string, callback: Function): ?Promise
	loadingPlaceholder: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.oneOfType([// replaces the placeholder while options are loading
	__WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node]),
	multi: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.bool, // multi-value input
	noResultsText: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.oneOfType([// field noResultsText, displayed when no options come back from the server
	__WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node]),
	onChange: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // onChange handler: function (newValue) {}
	onInputChange: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func, // optional for keeping track of what is being typed
	options: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.array.isRequired, // array of options
	placeholder: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.oneOfType([// field placeholder, displayed when there's no value (shared with Select)
	__WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node]),
	searchPromptText: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.oneOfType([// label to prompt for search input
	__WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.string, __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.node]),
	value: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any // initial field value
};

var defaultCache = {};

var defaultChildren = function defaultChildren(props) {
	return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(Select$1, props);
};

var defaultProps = {
	autoload: true,
	cache: defaultCache,
	children: defaultChildren,
	ignoreAccents: true,
	ignoreCase: true,
	loadingPlaceholder: 'Loading...',
	options: [],
	searchPromptText: 'Type to search'
};

var Async = function (_Component) {
	inherits(Async, _Component);

	function Async(props, context) {
		classCallCheck(this, Async);

		var _this = possibleConstructorReturn(this, (Async.__proto__ || Object.getPrototypeOf(Async)).call(this, props, context));

		_this._cache = props.cache === defaultCache ? {} : props.cache;

		_this.state = {
			inputValue: '',
			isLoading: false,
			options: props.options
		};

		_this.onInputChange = _this.onInputChange.bind(_this);
		return _this;
	}

	createClass(Async, [{
		key: 'componentDidMount',
		value: function componentDidMount() {
			var autoload = this.props.autoload;


			if (autoload) {
				this.loadOptions('');
			}
		}
	}, {
		key: 'componentWillReceiveProps',
		value: function componentWillReceiveProps(nextProps) {
			if (nextProps.options !== this.props.options) {
				this.setState({
					options: nextProps.options
				});
			}
		}
	}, {
		key: 'componentWillUnmount',
		value: function componentWillUnmount() {
			this._callback = null;
		}
	}, {
		key: 'loadOptions',
		value: function loadOptions(inputValue) {
			var _this2 = this;

			var loadOptions = this.props.loadOptions;

			var cache = this._cache;

			if (cache && Object.prototype.hasOwnProperty.call(cache, inputValue)) {
				this._callback = null;

				this.setState({
					isLoading: false,
					options: cache[inputValue]
				});

				return;
			}

			var callback = function callback(error, data) {
				var options = data && data.options || [];

				if (cache) {
					cache[inputValue] = options;
				}

				if (callback === _this2._callback) {
					_this2._callback = null;

					_this2.setState({
						isLoading: false,
						options: options
					});
				}
			};

			// Ignore all but the most recent request
			this._callback = callback;

			var promise = loadOptions(inputValue, callback);
			if (promise) {
				promise.then(function (data) {
					return callback(null, data);
				}, function (error) {
					return callback(error);
				});
			}

			if (this._callback && !this.state.isLoading) {
				this.setState({
					isLoading: true
				});
			}
		}
	}, {
		key: 'onInputChange',
		value: function onInputChange(inputValue) {
			var _props = this.props,
			    ignoreAccents = _props.ignoreAccents,
			    ignoreCase = _props.ignoreCase,
			    onInputChange = _props.onInputChange;

			var newInputValue = inputValue;

			if (onInputChange) {
				var value = onInputChange(newInputValue);
				// Note: != used deliberately here to catch undefined and null
				if (value != null && (typeof value === 'undefined' ? 'undefined' : _typeof(value)) !== 'object') {
					newInputValue = '' + value;
				}
			}

			var transformedInputValue = newInputValue;

			if (ignoreAccents) {
				transformedInputValue = stripDiacritics(transformedInputValue);
			}

			if (ignoreCase) {
				transformedInputValue = transformedInputValue.toLowerCase();
			}

			this.setState({ inputValue: newInputValue });
			this.loadOptions(transformedInputValue);

			// Return new input value, but without applying toLowerCase() to avoid modifying the user's view case of the input while typing.
			return newInputValue;
		}
	}, {
		key: 'noResultsText',
		value: function noResultsText() {
			var _props2 = this.props,
			    loadingPlaceholder = _props2.loadingPlaceholder,
			    noResultsText = _props2.noResultsText,
			    searchPromptText = _props2.searchPromptText;
			var _state = this.state,
			    inputValue = _state.inputValue,
			    isLoading = _state.isLoading;


			if (isLoading) {
				return loadingPlaceholder;
			}
			if (inputValue && noResultsText) {
				return noResultsText;
			}
			return searchPromptText;
		}
	}, {
		key: 'focus',
		value: function focus() {
			this.select.focus();
		}
	}, {
		key: 'render',
		value: function render() {
			var _this3 = this;

			var _props3 = this.props,
			    children = _props3.children,
			    loadingPlaceholder = _props3.loadingPlaceholder,
			    placeholder = _props3.placeholder;
			var _state2 = this.state,
			    isLoading = _state2.isLoading,
			    options = _state2.options;


			var props = {
				noResultsText: this.noResultsText(),
				placeholder: isLoading ? loadingPlaceholder : placeholder,
				options: isLoading && loadingPlaceholder ? [] : options,
				ref: function ref(_ref) {
					return _this3.select = _ref;
				}
			};

			return children(_extends({}, this.props, props, {
				isLoading: isLoading,
				onInputChange: this.onInputChange
			}));
		}
	}]);
	return Async;
}(__WEBPACK_IMPORTED_MODULE_3_react__["Component"]);

Async.propTypes = propTypes;
Async.defaultProps = defaultProps;

var CreatableSelect = function (_React$Component) {
	inherits(CreatableSelect, _React$Component);

	function CreatableSelect(props, context) {
		classCallCheck(this, CreatableSelect);

		var _this = possibleConstructorReturn(this, (CreatableSelect.__proto__ || Object.getPrototypeOf(CreatableSelect)).call(this, props, context));

		_this.filterOptions = _this.filterOptions.bind(_this);
		_this.menuRenderer = _this.menuRenderer.bind(_this);
		_this.onInputKeyDown = _this.onInputKeyDown.bind(_this);
		_this.onInputChange = _this.onInputChange.bind(_this);
		_this.onOptionSelect = _this.onOptionSelect.bind(_this);
		return _this;
	}

	createClass(CreatableSelect, [{
		key: 'createNewOption',
		value: function createNewOption() {
			var _props = this.props,
			    isValidNewOption = _props.isValidNewOption,
			    newOptionCreator = _props.newOptionCreator,
			    onNewOptionClick = _props.onNewOptionClick,
			    _props$options = _props.options,
			    options = _props$options === undefined ? [] : _props$options;


			if (isValidNewOption({ label: this.inputValue })) {
				var option = newOptionCreator({ label: this.inputValue, labelKey: this.labelKey, valueKey: this.valueKey });
				var _isOptionUnique = this.isOptionUnique({ option: option, options: options });

				// Don't add the same option twice.
				if (_isOptionUnique) {
					if (onNewOptionClick) {
						onNewOptionClick(option);
					} else {
						options.unshift(option);

						this.select.selectValue(option);
					}
				}
			}
		}
	}, {
		key: 'filterOptions',
		value: function filterOptions$$1() {
			var _props2 = this.props,
			    filterOptions$$1 = _props2.filterOptions,
			    isValidNewOption = _props2.isValidNewOption,
			    promptTextCreator = _props2.promptTextCreator;

			// TRICKY Check currently selected options as well.
			// Don't display a create-prompt for a value that's selected.
			// This covers async edge-cases where a newly-created Option isn't yet in the async-loaded array.

			var excludeOptions = (arguments.length <= 2 ? undefined : arguments[2]) || [];

			var filteredOptions = filterOptions$$1.apply(undefined, arguments) || [];

			if (isValidNewOption({ label: this.inputValue })) {
				var _newOptionCreator = this.props.newOptionCreator;


				var option = _newOptionCreator({
					label: this.inputValue,
					labelKey: this.labelKey,
					valueKey: this.valueKey
				});

				// TRICKY Compare to all options (not just filtered options) in case option has already been selected).
				// For multi-selects, this would remove it from the filtered list.
				var _isOptionUnique2 = this.isOptionUnique({
					option: option,
					options: excludeOptions.concat(filteredOptions)
				});

				if (_isOptionUnique2) {
					var prompt = promptTextCreator(this.inputValue);

					this._createPlaceholderOption = _newOptionCreator({
						label: prompt,
						labelKey: this.labelKey,
						valueKey: this.valueKey
					});

					filteredOptions.unshift(this._createPlaceholderOption);
				}
			}

			return filteredOptions;
		}
	}, {
		key: 'isOptionUnique',
		value: function isOptionUnique(_ref) {
			var option = _ref.option,
			    options = _ref.options;
			var isOptionUnique = this.props.isOptionUnique;


			options = options || this.props.options;

			return isOptionUnique({
				labelKey: this.labelKey,
				option: option,
				options: options,
				valueKey: this.valueKey
			});
		}
	}, {
		key: 'menuRenderer',
		value: function menuRenderer$$1(params) {
			var menuRenderer$$1 = this.props.menuRenderer;


			return menuRenderer$$1(_extends({}, params, {
				onSelect: this.onOptionSelect,
				selectValue: this.onOptionSelect
			}));
		}
	}, {
		key: 'onInputChange',
		value: function onInputChange(input) {
			var onInputChange = this.props.onInputChange;

			// This value may be needed in between Select mounts (when this.select is null)

			this.inputValue = input;

			if (onInputChange) {
				this.inputValue = onInputChange(input);
			}

			return this.inputValue;
		}
	}, {
		key: 'onInputKeyDown',
		value: function onInputKeyDown(event) {
			var _props3 = this.props,
			    shouldKeyDownEventCreateNewOption = _props3.shouldKeyDownEventCreateNewOption,
			    onInputKeyDown = _props3.onInputKeyDown;

			var focusedOption = this.select.getFocusedOption();

			if (focusedOption && focusedOption === this._createPlaceholderOption && shouldKeyDownEventCreateNewOption({ keyCode: event.keyCode })) {
				this.createNewOption();

				// Prevent decorated Select from doing anything additional with this keyDown event
				event.preventDefault();
			} else if (onInputKeyDown) {
				onInputKeyDown(event);
			}
		}
	}, {
		key: 'onOptionSelect',
		value: function onOptionSelect(option) {
			if (option === this._createPlaceholderOption) {
				this.createNewOption();
			} else {
				this.select.selectValue(option);
			}
		}
	}, {
		key: 'focus',
		value: function focus() {
			this.select.focus();
		}
	}, {
		key: 'render',
		value: function render() {
			var _this2 = this;

			var _props4 = this.props,
			    refProp = _props4.ref,
			    restProps = objectWithoutProperties(_props4, ['ref']);
			var children = this.props.children;

			// We can't use destructuring default values to set the children,
			// because it won't apply work if `children` is null. A falsy check is
			// more reliable in real world use-cases.

			if (!children) {
				children = defaultChildren$2;
			}

			var props = _extends({}, restProps, {
				allowCreate: true,
				filterOptions: this.filterOptions,
				menuRenderer: this.menuRenderer,
				onInputChange: this.onInputChange,
				onInputKeyDown: this.onInputKeyDown,
				ref: function ref(_ref2) {
					_this2.select = _ref2;

					// These values may be needed in between Select mounts (when this.select is null)
					if (_ref2) {
						_this2.labelKey = _ref2.props.labelKey;
						_this2.valueKey = _ref2.props.valueKey;
					}
					if (refProp) {
						refProp(_ref2);
					}
				}
			});

			return children(props);
		}
	}]);
	return CreatableSelect;
}(__WEBPACK_IMPORTED_MODULE_3_react___default.a.Component);

var defaultChildren$2 = function defaultChildren(props) {
	return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(Select$1, props);
};

var isOptionUnique = function isOptionUnique(_ref3) {
	var option = _ref3.option,
	    options = _ref3.options,
	    labelKey = _ref3.labelKey,
	    valueKey = _ref3.valueKey;

	if (!options || !options.length) {
		return true;
	}

	return options.filter(function (existingOption) {
		return existingOption[labelKey] === option[labelKey] || existingOption[valueKey] === option[valueKey];
	}).length === 0;
};

var isValidNewOption = function isValidNewOption(_ref4) {
	var label = _ref4.label;
	return !!label;
};

var newOptionCreator = function newOptionCreator(_ref5) {
	var label = _ref5.label,
	    labelKey = _ref5.labelKey,
	    valueKey = _ref5.valueKey;

	var option = {};
	option[valueKey] = label;
	option[labelKey] = label;
	option.className = 'Select-create-option-placeholder';

	return option;
};

var promptTextCreator = function promptTextCreator(label) {
	return 'Create option "' + label + '"';
};

var shouldKeyDownEventCreateNewOption = function shouldKeyDownEventCreateNewOption(_ref6) {
	var keyCode = _ref6.keyCode;

	switch (keyCode) {
		case 9: // TAB
		case 13: // ENTER
		case 188:
			// COMMA
			return true;
		default:
			return false;
	}
};

// Default prop methods
CreatableSelect.isOptionUnique = isOptionUnique;
CreatableSelect.isValidNewOption = isValidNewOption;
CreatableSelect.newOptionCreator = newOptionCreator;
CreatableSelect.promptTextCreator = promptTextCreator;
CreatableSelect.shouldKeyDownEventCreateNewOption = shouldKeyDownEventCreateNewOption;

CreatableSelect.defaultProps = {
	filterOptions: filterOptions,
	isOptionUnique: isOptionUnique,
	isValidNewOption: isValidNewOption,
	menuRenderer: menuRenderer,
	newOptionCreator: newOptionCreator,
	promptTextCreator: promptTextCreator,
	shouldKeyDownEventCreateNewOption: shouldKeyDownEventCreateNewOption
};

CreatableSelect.propTypes = {
	// Child function responsible for creating the inner Select component
	// This component can be used to compose HOCs (eg Creatable and Async)
	// (props: Object): PropTypes.element
	children: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// See Select.propTypes.filterOptions
	filterOptions: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any,

	// Searches for any matching option within the set of options.
	// This function prevents duplicate options from being created.
	// ({ option: Object, options: Array, labelKey: string, valueKey: string }): boolean
	isOptionUnique: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// Determines if the current input text represents a valid option.
	// ({ label: string }): boolean
	isValidNewOption: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// See Select.propTypes.menuRenderer
	menuRenderer: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.any,

	// Factory to create new option.
	// ({ label: string, labelKey: string, valueKey: string }): Object
	newOptionCreator: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// input change handler: function (inputValue) {}
	onInputChange: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// input keyDown handler: function (event) {}
	onInputKeyDown: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// new option click handler: function (option) {}
	onNewOptionClick: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// See Select.propTypes.options
	options: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.array,

	// Creates prompt/placeholder option text.
	// (filterText: string): string
	promptTextCreator: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	ref: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func,

	// Decides if a keyDown event (eg its `keyCode`) should result in the creation of a new option.
	shouldKeyDownEventCreateNewOption: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func
};

var AsyncCreatableSelect = function (_React$Component) {
	inherits(AsyncCreatableSelect, _React$Component);

	function AsyncCreatableSelect() {
		classCallCheck(this, AsyncCreatableSelect);
		return possibleConstructorReturn(this, (AsyncCreatableSelect.__proto__ || Object.getPrototypeOf(AsyncCreatableSelect)).apply(this, arguments));
	}

	createClass(AsyncCreatableSelect, [{
		key: 'focus',
		value: function focus() {
			this.select.focus();
		}
	}, {
		key: 'render',
		value: function render() {
			var _this2 = this;

			return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
				Async,
				this.props,
				function (_ref) {
					var ref = _ref.ref,
					    asyncProps = objectWithoutProperties(_ref, ['ref']);

					var asyncRef = ref;
					return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(
						CreatableSelect,
						asyncProps,
						function (_ref2) {
							var ref = _ref2.ref,
							    creatableProps = objectWithoutProperties(_ref2, ['ref']);

							var creatableRef = ref;
							return _this2.props.children(_extends({}, creatableProps, {
								ref: function ref(select) {
									creatableRef(select);
									asyncRef(select);
									_this2.select = select;
								}
							}));
						}
					);
				}
			);
		}
	}]);
	return AsyncCreatableSelect;
}(__WEBPACK_IMPORTED_MODULE_3_react___default.a.Component);

var defaultChildren$1 = function defaultChildren(props) {
	return __WEBPACK_IMPORTED_MODULE_3_react___default.a.createElement(Select$1, props);
};

AsyncCreatableSelect.propTypes = {
	children: __WEBPACK_IMPORTED_MODULE_2_prop_types___default.a.func.isRequired // Child function responsible for creating the inner Select component; (props: Object): PropTypes.element
};

AsyncCreatableSelect.defaultProps = {
	children: defaultChildren$1
};

Select$1.Async = Async;
Select$1.AsyncCreatable = AsyncCreatableSelect;
Select$1.Creatable = CreatableSelect;
Select$1.Value = Value;
Select$1.Option = Option;


/* harmony default export */ __webpack_exports__["default"] = (Select$1);


/***/ }),

/***/ 59:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
	value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var sizerStyle = {
	position: 'absolute',
	top: 0,
	left: 0,
	visibility: 'hidden',
	height: 0,
	overflow: 'scroll',
	whiteSpace: 'pre'
};

var INPUT_PROPS_BLACKLIST = ['extraWidth', 'injectStyles', 'inputClassName', 'inputRef', 'inputStyle', 'minWidth', 'onAutosize', 'placeholderIsMinWidth'];

var cleanInputProps = function cleanInputProps(inputProps) {
	INPUT_PROPS_BLACKLIST.forEach(function (field) {
		return delete inputProps[field];
	});
	return inputProps;
};

var copyStyles = function copyStyles(styles, node) {
	node.style.fontSize = styles.fontSize;
	node.style.fontFamily = styles.fontFamily;
	node.style.fontWeight = styles.fontWeight;
	node.style.fontStyle = styles.fontStyle;
	node.style.letterSpacing = styles.letterSpacing;
	node.style.textTransform = styles.textTransform;
};

var isIE = typeof window !== 'undefined' && window.navigator ? /MSIE |Trident\/|Edge\//.test(window.navigator.userAgent) : false;

var generateId = function generateId() {
	// we only need an auto-generated ID for stylesheet injection, which is only
	// used for IE. so if the browser is not IE, this should return undefined.
	return isIE ? '_' + Math.random().toString(36).substr(2, 12) : undefined;
};

var AutosizeInput = function (_Component) {
	_inherits(AutosizeInput, _Component);

	function AutosizeInput(props) {
		_classCallCheck(this, AutosizeInput);

		var _this = _possibleConstructorReturn(this, (AutosizeInput.__proto__ || Object.getPrototypeOf(AutosizeInput)).call(this, props));

		_this.inputRef = function (el) {
			_this.input = el;
			if (typeof _this.props.inputRef === 'function') {
				_this.props.inputRef(el);
			}
		};

		_this.placeHolderSizerRef = function (el) {
			_this.placeHolderSizer = el;
		};

		_this.sizerRef = function (el) {
			_this.sizer = el;
		};

		_this.state = {
			inputWidth: props.minWidth,
			inputId: props.id || generateId()
		};
		return _this;
	}

	_createClass(AutosizeInput, [{
		key: 'componentDidMount',
		value: function componentDidMount() {
			this.mounted = true;
			this.copyInputStyles();
			this.updateInputWidth();
		}
	}, {
		key: 'componentWillReceiveProps',
		value: function componentWillReceiveProps(nextProps) {
			var id = nextProps.id;

			if (id !== this.props.id) {
				this.setState({ inputId: id || generateId() });
			}
		}
	}, {
		key: 'componentDidUpdate',
		value: function componentDidUpdate(prevProps, prevState) {
			if (prevState.inputWidth !== this.state.inputWidth) {
				if (typeof this.props.onAutosize === 'function') {
					this.props.onAutosize(this.state.inputWidth);
				}
			}
			this.updateInputWidth();
		}
	}, {
		key: 'componentWillUnmount',
		value: function componentWillUnmount() {
			this.mounted = false;
		}
	}, {
		key: 'copyInputStyles',
		value: function copyInputStyles() {
			if (!this.mounted || !window.getComputedStyle) {
				return;
			}
			var inputStyles = this.input && window.getComputedStyle(this.input);
			if (!inputStyles) {
				return;
			}
			copyStyles(inputStyles, this.sizer);
			if (this.placeHolderSizer) {
				copyStyles(inputStyles, this.placeHolderSizer);
			}
		}
	}, {
		key: 'updateInputWidth',
		value: function updateInputWidth() {
			if (!this.mounted || !this.sizer || typeof this.sizer.scrollWidth === 'undefined') {
				return;
			}
			var newInputWidth = void 0;
			if (this.props.placeholder && (!this.props.value || this.props.value && this.props.placeholderIsMinWidth)) {
				newInputWidth = Math.max(this.sizer.scrollWidth, this.placeHolderSizer.scrollWidth) + 2;
			} else {
				newInputWidth = this.sizer.scrollWidth + 2;
			}
			// add extraWidth to the detected width. for number types, this defaults to 16 to allow for the stepper UI
			var extraWidth = this.props.type === 'number' && this.props.extraWidth === undefined ? 16 : parseInt(this.props.extraWidth) || 0;
			newInputWidth += extraWidth;
			if (newInputWidth < this.props.minWidth) {
				newInputWidth = this.props.minWidth;
			}
			if (newInputWidth !== this.state.inputWidth) {
				this.setState({
					inputWidth: newInputWidth
				});
			}
		}
	}, {
		key: 'getInput',
		value: function getInput() {
			return this.input;
		}
	}, {
		key: 'focus',
		value: function focus() {
			this.input.focus();
		}
	}, {
		key: 'blur',
		value: function blur() {
			this.input.blur();
		}
	}, {
		key: 'select',
		value: function select() {
			this.input.select();
		}
	}, {
		key: 'renderStyles',
		value: function renderStyles() {
			// this method injects styles to hide IE's clear indicator, which messes
			// with input size detection. the stylesheet is only injected when the
			// browser is IE, and can also be disabled by the `injectStyles` prop.
			var injectStyles = this.props.injectStyles;

			return isIE && injectStyles ? _react2.default.createElement('style', { dangerouslySetInnerHTML: {
					__html: 'input#' + this.state.inputId + '::-ms-clear {display: none;}'
				} }) : null;
		}
	}, {
		key: 'render',
		value: function render() {
			var sizerValue = [this.props.defaultValue, this.props.value, ''].reduce(function (previousValue, currentValue) {
				if (previousValue !== null && previousValue !== undefined) {
					return previousValue;
				}
				return currentValue;
			});

			var wrapperStyle = _extends({}, this.props.style);
			if (!wrapperStyle.display) wrapperStyle.display = 'inline-block';

			var inputStyle = _extends({
				boxSizing: 'content-box',
				width: this.state.inputWidth + 'px'
			}, this.props.inputStyle);

			var inputProps = _objectWithoutProperties(this.props, []);

			cleanInputProps(inputProps);
			inputProps.className = this.props.inputClassName;
			inputProps.id = this.state.inputId;
			inputProps.style = inputStyle;

			return _react2.default.createElement(
				'div',
				{ className: this.props.className, style: wrapperStyle },
				this.renderStyles(),
				_react2.default.createElement('input', _extends({}, inputProps, { ref: this.inputRef })),
				_react2.default.createElement(
					'div',
					{ ref: this.sizerRef, style: sizerStyle },
					sizerValue
				),
				this.props.placeholder ? _react2.default.createElement(
					'div',
					{ ref: this.placeHolderSizerRef, style: sizerStyle },
					this.props.placeholder
				) : null
			);
		}
	}]);

	return AutosizeInput;
}(_react.Component);

AutosizeInput.propTypes = {
	className: _propTypes2.default.string, // className for the outer element
	defaultValue: _propTypes2.default.any, // default field value
	extraWidth: _propTypes2.default.oneOfType([// additional width for input element
	_propTypes2.default.number, _propTypes2.default.string]),
	id: _propTypes2.default.string, // id to use for the input, can be set for consistent snapshots
	injectStyles: _propTypes2.default.bool, // inject the custom stylesheet to hide clear UI, defaults to true
	inputClassName: _propTypes2.default.string, // className for the input element
	inputRef: _propTypes2.default.func, // ref callback for the input element
	inputStyle: _propTypes2.default.object, // css styles for the input element
	minWidth: _propTypes2.default.oneOfType([// minimum width for input element
	_propTypes2.default.number, _propTypes2.default.string]),
	onAutosize: _propTypes2.default.func, // onAutosize handler: function(newWidth) {}
	onChange: _propTypes2.default.func, // onChange handler: function(event) {}
	placeholder: _propTypes2.default.string, // placeholder text
	placeholderIsMinWidth: _propTypes2.default.bool, // don't collapse size to less than the placeholder
	style: _propTypes2.default.object, // css styles for the outer element
	value: _propTypes2.default.any // field value
};
AutosizeInput.defaultProps = {
	minWidth: 1,
	injectStyles: true
};

exports.default = AutosizeInput;

/***/ }),

/***/ 65:
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

/***/ 66:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _redux = __webpack_require__(10);

var _reduxThunk = __webpack_require__(72);

var _reduxThunk2 = _interopRequireDefault(_reduxThunk);

var _logger = __webpack_require__(67);

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

/***/ 67:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _reduxLogger = __webpack_require__(73);

var _config = __webpack_require__(50);

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

/***/ 8:
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

/***/ 86:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setProductOptions = exports.setSelectedProduct = undefined;

var _types = __webpack_require__(5);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedProduct = exports.setSelectedProduct = function setSelectedProduct(product) {
    return {
        type: types.SET_SELECTED_PRODUCT,
        data: product
    };
};
var setProductOptions = exports.setProductOptions = function setProductOptions(productList) {
    return {
        type: types.SET_PRODUCT_OPTIONS,
        data: productList
    };
};

/***/ }),

/***/ 88:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _MultipleSelect = __webpack_require__(89);

var _MultipleSelect2 = _interopRequireDefault(_MultipleSelect);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _MultipleSelect2.default;

/***/ }),

/***/ 89:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _MultipleSelectLabel = __webpack_require__(90);

var _MultipleSelectLabel2 = _interopRequireDefault(_MultipleSelectLabel);

var _MultipleSelectOptionList = __webpack_require__(91);

var _MultipleSelectOptionList2 = _interopRequireDefault(_MultipleSelectOptionList);

var _OptionAll = __webpack_require__(92);

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

            _this._callBackToParent(selectedItem);

            _this.setState({ dataSource: newDataSource });
        };

        _this.onToggle = function () {
            _this.setState(function (prevState) {
                return { showOptionList: !prevState.showOptionList };
            }, function () {
                if (_this.searchInputBox && _this.state.showOptionList) {
                    _this.searchInputBox.focus();
                }
            });
        };

        _this.checkAllHandler = function (checked) {
            _this.originalDataSource.forEach(function (item) {
                item.checked = checked;
            });

            var searchText = _this.state.searchText;
            var dataSource = _this.originalDataSource.filter(function (item) {
                return item[VALUE_NAME].startsWith(searchText);
            });
            _this.setState({ dataSource: dataSource });
            _this._callBackToParent(null);
        };

        _this.onChangeSearchText = function (value) {
            _this.setState(function (prevState) {
                var dataSource = _this.originalDataSource.filter(function (item) {
                    return item[VALUE_NAME].startsWith(value);
                });

                return {
                    searchText: value,
                    dataSource: dataSource
                };
            });
        };

        _this.onClearSearch = function () {
            _this.setState({ searchText: '' }, function () {
                return _this.onChangeSearchText('');
            });
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

        _this.searchInputBox = null;
        _this.id = 'multiple-select-' + (props.id || id++);
        _this.originalDataSource = _this._convertDataSourceToState(props);

        _this.state = {
            searchText: '',
            showOptionList: false,
            dataSource: _this.originalDataSource
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
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            if (this.props.dataSource != nextProps.dataSource) {
                this.originalDataSource = this._convertDataSourceToState(nextProps);
                this.setState({
                    dataSource: this.originalDataSource
                });
                this._callBackToParent(null);
            }
        }
    }, {
        key: '_callBackToParent',
        value: function _callBackToParent(selectedItem) {
            var selectedItemsKey = this._getSelectedItemKey(this.originalDataSource);
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
                var checkedItemCount = this.originalDataSource.filter(function (x) {
                    return x.checked;
                }).length;
                var checkedAll = checkedItemCount === this.originalDataSource.length;
                var labelText = '(' + this.props.optionAllLabel + ')';
                return _react2.default.createElement(_OptionAll2.default, { id: this.id, checked: checkedAll, label: labelText, onChange: this.checkAllHandler });
            }

            return null;
        }
    }, {
        key: '_renderSearchBox',
        value: function _renderSearchBox() {
            var _this2 = this;

            if (!this.props.hasSearchBox) {
                return null;
            }

            var clearButtonStyle = {
                position: "absolute",
                right: "5px",
                width: "20px",
                height: "20px",
                border: 0,
                margin: "5px",
                backgroundColor: "transparent",
                cursor: "pointer" };

            return _react2.default.createElement(
                'div',
                { style: { width: "100%" } },
                _react2.default.createElement('input', { autoFocus: true, onChange: function onChange(e) {
                        return _this2.onChangeSearchText(e.target.value);
                    },
                    ref: function ref(element) {
                        return _this2.searchInputBox = element;
                    },
                    value: this.state.searchText, type: 'text', placeholder: 'Search data',
                    style: { width: "100%", border: "1px solid #ccc", padding: "5px 10px" } }),
                _react2.default.createElement(
                    'button',
                    { onClick: this.onClearSearch, style: clearButtonStyle },
                    'X'
                )
            );
        }
    }, {
        key: 'render',
        value: function render() {
            var _this3 = this;

            var _props = this.props,
                noneSelectedLabel = _props.noneSelectedLabel,
                maxDisplayItemCount = _props.maxDisplayItemCount;


            return _react2.default.createElement(
                'div',
                { className: 'multiple-select-container', id: this.id, ref: function ref(element) {
                        return _this3.wrapper = element;
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
                    this._renderSearchBox(),
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
            return this.originalDataSource.filter(function (item) {
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
    hasAllOption: _propTypes2.default.bool,
    hasSearchBox: _propTypes2.default.bool
};
MultipleSelect.defaultProps = {
    keyField: KEY_NAME,
    valueField: VALUE_NAME,
    statusField: STATUS_NAME,
    hasAllOption: true,
    hasSearchBox: false
};
exports.default = MultipleSelect;

/***/ }),

/***/ 90:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(1);

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

/***/ 91:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _Option = __webpack_require__(54);

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

            if (!dataSource || !dataSource.length) {
                return _react2.default.createElement(
                    'span',
                    null,
                    'No information'
                );
            }

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

/***/ 92:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(2);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(1);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _Option = __webpack_require__(54);

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

/***/ 93:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.productOptions = exports.selectedProduct = undefined;

var _types = __webpack_require__(5);

var types = _interopRequireWildcard(_types);

var _createReducer3 = __webpack_require__(8);

var _createReducer4 = _interopRequireDefault(_createReducer3);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var selectedProduct = exports.selectedProduct = (0, _createReducer4.default)({}, _defineProperty({}, types.SET_SELECTED_PRODUCT, function (state, action) {
    return action.data;
}));

var productOptions = exports.productOptions = (0, _createReducer4.default)({}, _defineProperty({}, types.SET_PRODUCT_OPTIONS, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 95:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setSelectedCurrency = undefined;

var _types = __webpack_require__(5);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var setSelectedCurrency = exports.setSelectedCurrency = function setSelectedCurrency(item) {
    return {
        type: types.SET_SELECTED_CURRENCY,
        data: item
    };
};

/***/ }),

/***/ 96:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.currencyList = exports.selectedCurrency = undefined;

var _types = __webpack_require__(5);

var types = _interopRequireWildcard(_types);

var _createReducer3 = __webpack_require__(8);

var _createReducer4 = _interopRequireDefault(_createReducer3);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var selectedCurrency = exports.selectedCurrency = (0, _createReducer4.default)({}, _defineProperty({}, types.SET_SELECTED_CURRENCY, function (state, action) {
    return action.data;
}));

var currencyList = exports.currencyList = (0, _createReducer4.default)({}, _defineProperty({}, types.GET_CURRENCY, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 98:
/***/ (function(module, exports) {

module.exports = cptable;

/***/ }),

/***/ 99:
/***/ (function(module, exports) {

module.exports = jszip;

/***/ })

},[452]);
//# sourceMappingURL=speciallicenseewl.chunk.js.map