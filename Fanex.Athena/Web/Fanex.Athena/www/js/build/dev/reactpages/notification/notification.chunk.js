webpackJsonp([3],{

/***/ 10:
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

/***/ }),

/***/ 100:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

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

/***/ 101:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.isLoading = undefined;

var _types = __webpack_require__(10);

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(13);

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var isLoading = exports.isLoading = (0, _createReducer3.default)({}, _defineProperty({}, types.TOGGLE_LOADER, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 102:
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

/***/ 123:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var FETCH_DATA = exports.FETCH_DATA = 'RESULTAMENDMENT_FETCH_DATA';
var SET_GRID_DATA = exports.SET_GRID_DATA = "RESULTAMENDMENT_SET_GRID_DATA";

var SET_DATE_RANGE = exports.SET_DATE_RANGE = "RESULTAMENDMENT_SET_DATE_RANGE";
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = "RESULTAMENDMENT_GET_DATE_CONSTRAINT";
var CHANGE_CONVERT_ALL = exports.CHANGE_CONVERT_ALL = "RESULTAMENDMENT_CHANGE_CONVERT_ALL";

var GET_XSRF_TOKEN = exports.GET_XSRF_TOKEN = "RESULTAMENDMENT_GET_XSRF_TOKEN";

var SET_PARAM = exports.SET_PARAM = "RESULTAMENDMENT_SET_PARAM";

var GET_LICENSEE_NAME = exports.GET_LICENSEE_NAME = "RESULTAMENDMENT_GET_LICENSEE_NAME";

/***/ }),

/***/ 13:
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

/***/ 16:
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
		!(__WEBPACK_AMD_DEFINE_ARRAY__ = [], __WEBPACK_AMD_DEFINE_RESULT__ = function () {
			return classNames;
		}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	} else {
		window.classNames = classNames;
	}
}());


/***/ }),

/***/ 21:
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

/***/ 24:
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

/***/ 31:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = parseInput;

var _moment = __webpack_require__(0);

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

/***/ 32:
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

/***/ 406:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ActionCreators = undefined;

var _dataHandlerAction = __webpack_require__(721);

var dataHandlerAction = _interopRequireWildcard(_dataHandlerAction);

var _filterHandlerAction = __webpack_require__(722);

var filterHandlerAction = _interopRequireWildcard(_filterHandlerAction);

var _loaderAction = __webpack_require__(76);

var loaderAction = _interopRequireWildcard(_loaderAction);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var ActionCreators = exports.ActionCreators = Object.assign({}, dataHandlerAction, filterHandlerAction, loaderAction);

/***/ }),

/***/ 407:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
var FETCH_DATA = exports.FETCH_DATA = 'GENERAL_FETCH_DATA';
var SET_GRID_DATA = exports.SET_GRID_DATA = "GENERAL_SET_GRID_DATA";

var SET_DATE_RANGE = exports.SET_DATE_RANGE = "GENERAL_SET_DATE_RANGE";
var GET_DATE_CONSTRAINT = exports.GET_DATE_CONSTRAINT = "GENERAL_GET_DATE_CONSTRAINT";
var CHANGE_CONVERT_ALL = exports.CHANGE_CONVERT_ALL = "GENERAL_CHANGE_CONVERT_ALL";

var SET_NOTIFICATION_TYPE = exports.SET_NOTIFICATION_TYPE = "GENERAL_SET_NOTIFICATION_TYPE";
var GET_NOTIFICATION_TYPE = exports.GET_NOTIFICATION_TYPE = "GENERAL_GET_NOTIFICATION_TYPE";

var SET_PARAM = exports.SET_PARAM = "GENERAL_SET_PARAM";

/***/ }),

/***/ 45:
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

/***/ 46:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _parseInput = __webpack_require__(31);

var _parseInput2 = _interopRequireDefault(_parseInput);

var _DayCell = __webpack_require__(95);

var _DayCell2 = _interopRequireDefault(_DayCell);

var _styles = __webpack_require__(21);

var _styles2 = _interopRequireDefault(_styles);

var _constants = __webpack_require__(32);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var START_OF_MONTH = 0;
var END_OF_MONTH = 11;
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
    var showDate = (shownDate || range && range['endDate'] || date).clone().add(offset, 'months');
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
        var startMonth = 0;
        startDate = (0, _moment2.default)([m.year() - DEFAULT_YEAR_RANGE, startMonth]);
      }

      if (maxDate) {
        endDate = _moment2.default.isMoment(maxDate) ? maxDate : (0, _moment2.default)(maxDate, _constants.MomentFormat.default);
      } else {
        var endMonth = 11;
        endDate = (0, _moment2.default)([m.year(), endMonth]).endOf("month");
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

/***/ 47:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _langHelper = __webpack_require__(9);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _locale = __webpack_require__(97);

var _locale2 = _interopRequireDefault(_locale);

var _constants = __webpack_require__(32);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

exports.default = {
    ranges: function ranges(today) {
        var _ref;

        if (today && !_moment2.default.isMoment(today)) {
            today = (0, _moment2.default)(today, _constants.MomentFormat.default);
        }

        if (!today) {
            today = (0, _moment2.default)();
        }

        return _ref = {}, _defineProperty(_ref, _langHelper2.default.getSingleResource("Today"), {
            startDate: today,
            endDate: today
        }), _defineProperty(_ref, _langHelper2.default.getSingleResource("Yesterday"), {
            startDate: today.clone().add(-1, 'days'),
            endDate: today.clone().add(-1, 'days')
        }), _defineProperty(_ref, _langHelper2.default.getSingleResource("Currentweek"), {
            startDate: today.clone().startOf('week'),
            endDate: today
        }), _defineProperty(_ref, _langHelper2.default.getSingleResource("Lastweek"), {
            startDate: today.clone().subtract(1, 'week').startOf('week'),
            endDate: today.clone().subtract(1, 'week').startOf('week').add(6, 'days')
        }), _defineProperty(_ref, _langHelper2.default.getSingleResource("Currentmonth"), {
            startDate: today.clone().startOf('month'),
            endDate: today
        }), _defineProperty(_ref, _langHelper2.default.getSingleResource("Lastmonth"), {
            startDate: today.clone().subtract(1, 'month').startOf('month'),
            endDate: today.clone().subtract(1, 'month').endOf('month')
        }), _defineProperty(_ref, _langHelper2.default.getSingleResource("Sincelastmonth"), {
            startDate: today.clone().subtract(1, 'month').startOf('month'),
            endDate: today
        }), _ref;
    },

    locale: _extends({
        applyLabel: _langHelper2.default.getSingleResource("OK"),
        cancelLabel: _langHelper2.default.getSingleResource("Cancel")
    }, _locale2.default)
};

/***/ }),

/***/ 717:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _AppContainer = __webpack_require__(718);

var _AppContainer2 = _interopRequireDefault(_AppContainer);

var _reactRedux = __webpack_require__(23);

var _reduxLogger = __webpack_require__(112);

var _redux = __webpack_require__(17);

var _reduxThunk = __webpack_require__(111);

var _reduxThunk2 = _interopRequireDefault(_reduxThunk);

var _reducers = __webpack_require__(726);

var _reducers2 = _interopRequireDefault(_reducers);

var _config = __webpack_require__(102);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var isDev = "development" === "development";
var configName = _config.debuggingConfig.name;
// middleware that logs actions
var loggerMiddleware = (0, _reduxLogger.createLogger)({
    predicate: function predicate(getState, action) {
        return isDev || window[configName] && window[configName].env === _config.debuggingConfig.env && (!window[configName].actionFilter || window[configName].actionFilter === action.type);
    }
});

var defaultState = Object.assign({}, window.initialState);

function configureStore(initialState) {
    var enhancer = (0, _redux.compose)((0, _redux.applyMiddleware)(_reduxThunk2.default, // lets us dispatch() functions
    loggerMiddleware));

    return (0, _redux.createStore)(_reducers2.default, initialState, enhancer);
}

var store = configureStore(defaultState);

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

/***/ 718:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.AppContainer = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(23);

var _redux = __webpack_require__(17);

var _resultAmendment = __webpack_require__(719);

var _resultAmendment2 = _interopRequireDefault(_resultAmendment);

var _Loader = __webpack_require__(99);

var _Loader2 = _interopRequireDefault(_Loader);

var _AppLayout = __webpack_require__(100);

var _AppLayout2 = _interopRequireDefault(_AppLayout);

var _langHelper = __webpack_require__(9);

var _langHelper2 = _interopRequireDefault(_langHelper);

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
                _react2.default.createElement(_resultAmendment2.default, null)
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

/***/ 719:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _DataGrid = __webpack_require__(720);

var _DataGrid2 = _interopRequireDefault(_DataGrid);

var _FormFilter = __webpack_require__(725);

var _FormFilter2 = _interopRequireDefault(_FormFilter);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var ResultAmendment = function (_Component) {
  _inherits(ResultAmendment, _Component);

  function ResultAmendment(props) {
    _classCallCheck(this, ResultAmendment);

    return _possibleConstructorReturn(this, (ResultAmendment.__proto__ || Object.getPrototypeOf(ResultAmendment)).call(this, props));
  }

  _createClass(ResultAmendment, [{
    key: 'render',
    value: function render() {
      return _react2.default.createElement(
        'div',
        null,
        _react2.default.createElement(_FormFilter2.default, null),
        _react2.default.createElement(_DataGrid2.default, null)
      );
    }
  }]);

  return ResultAmendment;
}(_react.Component);

exports.default = ResultAmendment;

/***/ }),

/***/ 720:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.DataGrid = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(23);

var _resultAmendment = __webpack_require__(406);

var _redux = __webpack_require__(17);

var _fixedTableHeader = __webpack_require__(77);

var _langHelper = __webpack_require__(9);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _ReportTitle = __webpack_require__(84);

var _ReportTitle2 = _interopRequireDefault(_ReportTitle);

var _NoDataTitle = __webpack_require__(85);

var _NoDataTitle2 = _interopRequireDefault(_NoDataTitle);

var _resultAmendmentService = __webpack_require__(723);

var _resultAmendmentService2 = _interopRequireDefault(_resultAmendmentService);

var _site = __webpack_require__(24);

var _site2 = _interopRequireDefault(_site);

var _CollapsibleRow = __webpack_require__(724);

var _CollapsibleRow2 = _interopRequireDefault(_CollapsibleRow);

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _excel = __webpack_require__(98);

var _excel2 = _interopRequireDefault(_excel);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DataGrid = exports.DataGrid = function (_Component) {
    _inherits(DataGrid, _Component);

    function DataGrid(props) {
        _classCallCheck(this, DataGrid);

        var _this = _possibleConstructorReturn(this, (DataGrid.__proto__ || Object.getPrototypeOf(DataGrid)).call(this, props));

        _this.onPaging = function (index, size) {
            var paramData = _this.props.param;

            paramData.PageIndex = index;
            paramData.PageSize = size;

            var params = _this._buildParams({
                data: paramData
            });

            _this.props.toggleLoader(true);
            _this.props.getGridData(params);
            _this.props.setParam(paramData);
        };

        _this.reportTitle = "";
        return _this;
    }

    _createClass(DataGrid, [{
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            if (this.props.gridData !== nextProps.gridData) {
                var paramData = this.props.param;
                if (paramData) {
                    this.reportTitle = _resultAmendmentService2.default.getReportTitle({
                        fromDate: paramData.FromDate,
                        toDate: paramData.ToDate
                    });
                }
                this.props.toggleLoader(false);
            }
        }
    }, {
        key: 'onExpand',
        value: function onExpand(index, item) {
            var _props$gridData = this.props.gridData,
                Data = _props$gridData.Data,
                Total = _props$gridData.Total,
                DownloadFileApi = _props$gridData.DownloadFileApi;

            Data[index].IsExpand = true;

            if (!item.IsRead) {
                this.props.markItemAsRead(this._buildParams({
                    Index: index,
                    Id: item.Id,
                    Type: item.Type,
                    Data: Data,
                    Total: Total,
                    DownloadFileApi: DownloadFileApi
                }));
            } else {
                this.props.setGridData({ Data: Data, Total: Total, DownloadFileApi: DownloadFileApi });
            }
        }
    }, {
        key: 'exportExcel',
        value: function exportExcel(item) {
            var _this2 = this;

            var params = this._buildParams({
                api: this.props.gridData.DownloadFileApi,
                fileName: item.DataFilePath
            });
            var fileName = this.props.licenseeName;

            this.props.toggleLoader(true);
            this.props.exportAttachment(params).then(function (response) {
                if (response.status === 204) {
                    alert(_langHelper2.default.getSingleResource('FileIsNotReady'));
                } else {
                    var blob = response.data;
                    _excel2.default.downloadFromBolb(blob, fileName);
                }
                _this2.props.toggleLoader(false);
            });
        }
    }, {
        key: '_buildParams',
        value: function _buildParams(params) {
            params.xsrfToken = this.props.xsrfToken;
            return params;
        }
    }, {
        key: '_buildHeader',
        value: function _buildHeader() {
            var headerText = _resultAmendmentService2.default.getHeaderText();

            return _react2.default.createElement(
                _fixedTableHeader.Row,
                { style: { height: 32 } },
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 30, header: true, title: headerText.index },
                    headerText.index
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 120, header: true, title: headerText.dateTime },
                    headerText.dateTime
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 350, header: true, title: headerText.content },
                    headerText.content
                ),
                _react2.default.createElement(
                    _fixedTableHeader.Cell,
                    { colWidth: 30, header: true, title: headerText.ticketList },
                    headerText.ticketList
                )
            );
        }
    }, {
        key: '_buildBody',
        value: function _buildBody() {
            var _this3 = this;

            var data = this.props.gridData.Data;
            var exportToExcelTitle = _langHelper2.default.getSingleResource("ExportToExcel");

            var rows = [];
            var index = (this.props.param.PageIndex - 1) * this.props.param.PageSize + 1;

            var _loop = function _loop(i) {
                var item = data[i];
                var datetimeString = (0, _moment2.default)(item.DateTime).format("MM/DD/YYYY hh:mm:ss A");
                rows.push(_react2.default.createElement(
                    _fixedTableHeader.Row,
                    { key: 'tr' + i, className: item.IsRead ? "" : "unread" },
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: index, className: 'center-align' },
                        index
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: datetimeString, className: 'center-align' },
                        datetimeString
                    ),
                    _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { className: 'notification-content' },
                        _react2.default.createElement(_CollapsibleRow2.default, {
                            subject: item.Subject,
                            content: item.Content,
                            onExpand: function onExpand() {
                                return _this3.onExpand(i, item);
                            },
                            isExpand: item.IsExpand,
                            isHtml: true })
                    ),
                    _site2.default.asNull(item.DataFilePath) === null ? _react2.default.createElement(_fixedTableHeader.Cell, null) : _react2.default.createElement(
                        _fixedTableHeader.Cell,
                        { title: exportToExcelTitle, className: 'center-align' },
                        _react2.default.createElement('a', { onClick: function onClick() {
                                return _this3.exportExcel(item);
                            }, className: 'icon-excel' })
                    )
                ));
                index++;
            };

            for (var i = 0; i < data.length; i++) {
                _loop(i);
            }

            return rows;
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
            var gridData = this.props.gridData;
            if (!gridData.Data) {
                return null;
            }

            if (!gridData.Data || !gridData.Data.length) {
                return this._renderNodata();
            }

            var param = this.props.param;
            var pageOption = {
                PageIndex: param.PageIndex,
                PageSize: param.PageSize,
                TotalItem: gridData.Total,
                PageList: [10, 20, 50, 100, 500]
            };

            return _react2.default.createElement(
                'div',
                { className: 'result-content' },
                _react2.default.createElement(_ReportTitle2.default, { content: this.reportTitle }),
                _react2.default.createElement(_fixedTableHeader.Table, { minWidth: 650,
                    autoWidth: true,
                    adjustedHeight: 32,
                    header: this._buildHeader(),
                    body: this._buildBody(),
                    isPaging: true,
                    pageOption: pageOption,
                    onPaging: this.onPaging })
            );
        }
    }]);

    return DataGrid;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_resultAmendment.ActionCreators, dispatch);
}

function mapStateToProps(state) {
    var _state$resultAmendmen = state.resultAmendment,
        gridData = _state$resultAmendmen.gridData,
        dateRange = _state$resultAmendmen.dateRange,
        param = _state$resultAmendmen.param,
        licenseeName = _state$resultAmendmen.licenseeName;

    return Object.assign({}, { gridData: gridData, dateRange: dateRange, param: param, xsrfToken: state.xsrfToken, licenseeName: licenseeName });
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(DataGrid);

/***/ }),

/***/ 721:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.getGridData = getGridData;
exports.setGridData = setGridData;
exports.markItemAsRead = markItemAsRead;
exports.exportAttachment = exportAttachment;

var _types = __webpack_require__(123);

var types = _interopRequireWildcard(_types);

var _axios = __webpack_require__(62);

var _axios2 = _interopRequireDefault(_axios);

var _site = __webpack_require__(24);

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var header = function header(xsrfToken) {
    return {
        'X-Requested-With': 'XMLHttpRequest',
        '__RequestVerificationToken': xsrfToken
    };
};

var fetchData = function fetchData(params) {
    var url = _site2.default.resolveClientUrl('api/NotificationReport/ResultAmendment');
    return _axios2.default.post(url, params.data, {
        headers: header(params.xsrfToken)
    });
};

var markNotification = function markNotification(params) {
    var url = _site2.default.resolveClientUrl('api/NotificationReport/MarkNotificationAsRead');
    return _axios2.default.post(url, { MessageId: params.id, MessageType: params.type }, {
        headers: header(params.xsrfToken)
    });
};

var downloadData = function downloadData(params) {
    var url = _site2.default.resolveClientUrl('api/NotificationReport/DownLoadFileAttachment');
    return _axios2.default.post(url, { FileName: params.fileName, Api: params.api }, {
        headers: header(params.xsrfToken),
        responseType: 'blob'
    });
};

function getGridData(params) {
    return function (dispatch) {
        fetchData(params).then(function (response) {
            dispatch(setGridData({
                Data: response.data.DataList,
                Total: response.data.TotalRows,
                DownloadFileApi: response.data.DownloadFileApi
            }));
        });
    };
}

function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}

function markItemAsRead(params) {
    return function (dispatch) {
        markNotification({ id: params.Id, type: params.Type, xsrfToken: params.xsrfToken }).then(function (response) {
            if (response.data.IsSuccessful) {
                params.Data[params.Index].IsRead = true;

                dispatch(setGridData({
                    Data: params.Data,
                    Total: params.Total,
                    DownloadFileApi: params.DownloadFileApi
                }));
            }
        });
    };
}

function exportAttachment(params) {
    return function () {
        return downloadData(params);
    };
}

/***/ }),

/***/ 722:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setParam = exports.setDateRange = undefined;

var _types = __webpack_require__(123);

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

var setParam = exports.setParam = function setParam(param) {
    return {
        type: types.SET_PARAM,
        data: param
    };
};

/***/ }),

/***/ 723:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _langHelper = __webpack_require__(9);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(24);

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var headerText = {
    index: "#",
    ticketList: _langHelper2.default.getSingleResource("TicketList"),
    dateTime: _langHelper2.default.getSingleResource("Date") + '/' + _langHelper2.default.getSingleResource("Time"),
    content: _langHelper2.default.getSingleResource("Content")
};

var Service = function () {
    function Service() {
        _classCallCheck(this, Service);
    }

    _createClass(Service, null, [{
        key: 'getReportTitle',
        value: function getReportTitle(dateRange) {
            var fromDate = _site2.default.convertDateToString(dateRange.fromDate);
            var toDate = _site2.default.convertDateToString(dateRange.toDate);

            return _langHelper2.default.getSingleResource("Amendment") + ', ' + fromDate + ' - ' + toDate;
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

/***/ 724:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var CollapsibleRow = function (_React$Component) {
    _inherits(CollapsibleRow, _React$Component);

    function CollapsibleRow(props) {
        _classCallCheck(this, CollapsibleRow);

        var _this = _possibleConstructorReturn(this, (CollapsibleRow.__proto__ || Object.getPrototypeOf(CollapsibleRow)).call(this, props));

        _this.onClick = function () {
            var newState = !_this.state.isCollapse;

            _this.setState({ isCollapse: newState });

            if (!newState) {
                _this.props.onExpand && _this.props.onExpand();
            }
        };

        _this.state = {
            isCollapse: !_this.props.isExpand
        };
        return _this;
    }

    _createClass(CollapsibleRow, [{
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            if (this.props.isExpand !== nextProps.isExpand) {
                this.setState({ isCollapse: !nextProps.isExpand });
            }
        }
    }, {
        key: 'render',
        value: function render() {
            var _props = this.props,
                subject = _props.subject,
                content = _props.content,
                isHtml = _props.isHtml;


            return _react2.default.createElement(
                'div',
                { className: this.state.isCollapse ? "collapse-item collapse" : "collapse-item" },
                _react2.default.createElement(
                    'div',
                    { title: subject, className: 'subject-item' },
                    _react2.default.createElement(
                        'span',
                        null,
                        subject
                    ),
                    _react2.default.createElement('i', { className: 'icon-arrow', onClick: this.onClick })
                ),
                isHtml ? _react2.default.createElement('div', { className: 'content-item', dangerouslySetInnerHTML: { __html: content } }) : _react2.default.createElement(
                    'div',
                    { className: 'content-item' },
                    content
                )
            );
        }
    }]);

    return CollapsibleRow;
}(_react2.default.Component);

CollapsibleRow.propTypes = {
    isExpand: _propTypes2.default.bool,
    isHtml: _propTypes2.default.bool,
    subject: _propTypes2.default.string,
    content: _propTypes2.default.string,
    onExpand: _propTypes2.default.func
};
exports.default = CollapsibleRow;
;

/***/ }),

/***/ 725:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.FormFilter = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactRedux = __webpack_require__(23);

var _resultAmendment = __webpack_require__(406);

var _redux = __webpack_require__(17);

var _dateRange = __webpack_require__(93);

var _langHelper = __webpack_require__(9);

var _langHelper2 = _interopRequireDefault(_langHelper);

var _site = __webpack_require__(24);

var _site2 = _interopRequireDefault(_site);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var RANGE_MONTH_CONSTRAINT = 6;

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

            _this.props.setParam(params.data);
            _this.props.toggleLoader(true);
            _this.props.getGridData(params);
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
            if (!this.props.xsrfToken) {
                return null;
            }

            var pageSize = 10;
            var param = this.props.param;
            if (param && param.hasOwnProperty("PageSize")) {
                pageSize = param.PageSize;
            }

            var _props$dateRange = this.props.dateRange,
                fromDate = _props$dateRange.fromDate,
                toDate = _props$dateRange.toDate;


            var paramData = {
                FromDate: _site2.default.convertDateToString(fromDate),
                ToDate: _site2.default.convertDateToString(toDate),
                PageSize: pageSize,
                PageIndex: 1
            };

            return {
                xsrfToken: this.props.xsrfToken,
                data: paramData
            };
        }
    }, {
        key: '_renderDateRange',
        value: function _renderDateRange() {
            var _props = this.props,
                dateRange = _props.dateRange,
                dateConstraint = _props.dateConstraint;


            return _react2.default.createElement(_dateRange.DateRange, { startDate: dateRange.fromDate, endDate: dateRange.toDate,
                minDate: dateConstraint.minDate, maxDate: dateConstraint.maxDate,
                today: dateRange.toDate, limitedMonthRange: RANGE_MONTH_CONSTRAINT,
                onChange: this.applyDateRangeHandler });
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
                        { className: 'col-12 col-md-4 col-sm-4 col-lg-3 col-xl-3' },
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
                                'div',
                                { className: 'button-group' },
                                _react2.default.createElement(
                                    'button',
                                    { type: 'button', className: 'btn btn-submit', onClick: this.submit },
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
                        { className: 'col-12' },
                        _react2.default.createElement('span', { className: 'circle' }),
                        _langHelper2.default.getSingleResource("NotificationDateRangeLimitNote")
                    )
                )
            );
        }
    }]);

    return FormFilter;
}(_react.Component);

function mapDispatchToProps(dispatch) {
    return (0, _redux.bindActionCreators)(_resultAmendment.ActionCreators, dispatch);
}
function mapStateToProps(state) {
    var _state$resultAmendmen = state.resultAmendment,
        gridData = _state$resultAmendmen.gridData,
        rest = _objectWithoutProperties(_state$resultAmendmen, ['gridData']);

    return Object.assign({}, rest, { xsrfToken: state.xsrfToken });
};

exports.default = (0, _reactRedux.connect)(mapStateToProps, mapDispatchToProps)(FormFilter);

/***/ }),

/***/ 726:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _redux = __webpack_require__(17);

var _general = __webpack_require__(727);

var _general2 = _interopRequireDefault(_general);

var _resultAmendment = __webpack_require__(730);

var _resultAmendment2 = _interopRequireDefault(_resultAmendment);

var _loaderReducer = __webpack_require__(101);

var loaderReducer = _interopRequireWildcard(_loaderReducer);

var _xsrfTokenReducer = __webpack_require__(733);

var xsrfTokenReducer = _interopRequireWildcard(_xsrfTokenReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var general = (0, _redux.combineReducers)(_general2.default);
var resultAmendment = (0, _redux.combineReducers)(_resultAmendment2.default);

exports.default = (0, _redux.combineReducers)(Object.assign({}, {
    general: general,
    resultAmendment: resultAmendment
}, loaderReducer, xsrfTokenReducer));

/***/ }),

/***/ 727:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _dataHandlerReducer = __webpack_require__(728);

var dataHandlerReducer = _interopRequireWildcard(_dataHandlerReducer);

var _filterHandlerReducer = __webpack_require__(729);

var filterHandlerReducer = _interopRequireWildcard(_filterHandlerReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = Object.assign({}, _extends({}, dataHandlerReducer, filterHandlerReducer));

/***/ }),

/***/ 728:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.gridData = undefined;

var _createReducer2 = __webpack_require__(13);

var _createReducer3 = _interopRequireDefault(_createReducer2);

var _types = __webpack_require__(407);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var gridData = exports.gridData = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_GRID_DATA, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 729:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.param = exports.selectedType = exports.notificationTypes = exports.dateConstraint = exports.dateRange = undefined;

var _createReducer6 = __webpack_require__(13);

var _createReducer7 = _interopRequireDefault(_createReducer6);

var _types = __webpack_require__(407);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var dateRange = exports.dateRange = (0, _createReducer7.default)({}, _defineProperty({}, types.SET_DATE_RANGE, function (state, action) {
    return action.data;
}));

var dateConstraint = exports.dateConstraint = (0, _createReducer7.default)({}, _defineProperty({}, types.GET_DATE_CONSTRAINT, function (state, action) {
    return action.data;
}));

var notificationTypes = exports.notificationTypes = (0, _createReducer7.default)({}, _defineProperty({}, types.GET_NOTIFICATION_TYPE, function (state, action) {
    return action.data;
}));

var selectedType = exports.selectedType = (0, _createReducer7.default)({}, _defineProperty({}, types.SET_NOTIFICATION_TYPE, function (state, action) {
    return action.data;
}));

var param = exports.param = (0, _createReducer7.default)({}, _defineProperty({}, types.SET_PARAM, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 730:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _dataHandlerReducer = __webpack_require__(731);

var dataHandlerReducer = _interopRequireWildcard(_dataHandlerReducer);

var _filterHandlerReducer = __webpack_require__(732);

var filterHandlerReducer = _interopRequireWildcard(_filterHandlerReducer);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

exports.default = Object.assign({}, _extends({}, dataHandlerReducer, filterHandlerReducer));

/***/ }),

/***/ 731:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.gridData = undefined;

var _createReducer2 = __webpack_require__(13);

var _createReducer3 = _interopRequireDefault(_createReducer2);

var _types = __webpack_require__(123);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var gridData = exports.gridData = (0, _createReducer3.default)({}, _defineProperty({}, types.SET_GRID_DATA, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 732:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.licenseeName = exports.param = exports.dateConstraint = exports.dateRange = undefined;

var _createReducer5 = __webpack_require__(13);

var _createReducer6 = _interopRequireDefault(_createReducer5);

var _types = __webpack_require__(123);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var dateRange = exports.dateRange = (0, _createReducer6.default)({}, _defineProperty({}, types.SET_DATE_RANGE, function (state, action) {
    return action.data;
}));

var dateConstraint = exports.dateConstraint = (0, _createReducer6.default)({}, _defineProperty({}, types.GET_DATE_CONSTRAINT, function (state, action) {
    return action.data;
}));

var param = exports.param = (0, _createReducer6.default)({}, _defineProperty({}, types.SET_PARAM, function (state, action) {
    return action.data;
}));

var licenseeName = exports.licenseeName = (0, _createReducer6.default)({}, _defineProperty({}, types.GET_LICENSEE_NAME, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 733:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.xsrfToken = undefined;

var _types = __webpack_require__(10);

var types = _interopRequireWildcard(_types);

var _createReducer2 = __webpack_require__(13);

var _createReducer3 = _interopRequireDefault(_createReducer2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var xsrfToken = exports.xsrfToken = (0, _createReducer3.default)({}, _defineProperty({}, types.GET_XSRF_TOKEN, function (state, action) {
    return action.data;
}));

/***/ }),

/***/ 76:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.toggleLoader = undefined;

var _types = __webpack_require__(10);

var types = _interopRequireWildcard(_types);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

var toggleLoader = exports.toggleLoader = function toggleLoader(show) {
    return {
        type: types.TOGGLE_LOADER,
        data: show
    };
};

/***/ }),

/***/ 77:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Cell = exports.Row = exports.Table = undefined;

var _Table = __webpack_require__(78);

var _Table2 = _interopRequireDefault(_Table);

var _Row = __webpack_require__(82);

var _Row2 = _interopRequireDefault(_Row);

var _Cell = __webpack_require__(83);

var _Cell2 = _interopRequireDefault(_Cell);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.Table = _Table2.default;
exports.Row = _Row2.default;
exports.Cell = _Cell2.default;

/***/ }),

/***/ 78:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _RowLayout = __webpack_require__(79);

var _RowLayout2 = _interopRequireDefault(_RowLayout);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _tableSection = __webpack_require__(80);

var _tableSection2 = _interopRequireDefault(_tableSection);

var _constants = __webpack_require__(45);

var _Page = __webpack_require__(81);

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

        _this._handleResize = function (event) {
            event && event.preventDefault();

            var bodyHeight = _this._calculateBodyHeight();
            if (_this.state.contentHeight !== bodyHeight) {
                _this.setState({ contentHeight: bodyHeight });
            }

            var maxWidth = document.body.clientWidth - _this.diffWidth;
            if (_this.state.maxWidth !== maxWidth) {
                _this.setState({ maxWidth: maxWidth });
            }
        };

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

        _this.diffWidth = _constants.BODY_WIDTH - props.maxWidth;
        _this.columnsWidth = _this._getColumnsWidth();
        _this.state = {
            maxWidth: props.maxWidth,
            contentHeight: props.bodyHeight
        };
        _this.adjustedHeight = props.adjustedHeight;
        return _this;
    }

    _createClass(Table, [{
        key: '_getColumnsWidth',
        value: function _getColumnsWidth() {
            var _this2 = this;

            var headerRows = this.props.header;
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
            this.columnsWidth = this._getColumnsWidth();
        }
    }, {
        key: '_calculateBodyHeight',
        value: function _calculateBodyHeight() {
            var windowHeight = window.innerHeight;
            var footerHeight = this.footerWrapper ? this.footerWrapper.offsetHeight : 0;
            var tableContentHeight = windowHeight - (this.bodyWrapper.offsetTop + footerHeight + 30) - this.adjustedHeight;

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
    adjustedHeight: 0
};
exports.default = Table;

/***/ }),

/***/ 79:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

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

/***/ 80:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _constants = __webpack_require__(45);

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

/***/ 81:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

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
                TotalItem = _props$pageOption.TotalItem;
            var inputValue = this.state.inputValue;


            var isDisable = isNaN(parseInt(inputValue));
            var isShow = TotalItem > PageSize;
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

/***/ 82:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

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

/***/ 83:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

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
                rest = _objectWithoutProperties(_props, ['header', 'colWidth']);

            return header ? _react2.default.createElement(
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
    header: _propTypes2.default.bool
};
Cell.defaultProps = {
    header: false
};
exports.default = Cell;

/***/ }),

/***/ 84:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

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

/***/ 85:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

var _reactDom2 = _interopRequireDefault(_reactDom);

var _langHelper = __webpack_require__(9);

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

/***/ 9:
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
        key: "currentLanguage",
        get: function get() {
            return langCode;
        }
    }]);

    return LangHelper;
}();

exports.default = LangHelper;

/***/ }),

/***/ 93:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Calendar = exports.DateRange = undefined;

var _DateRange = __webpack_require__(94);

var _DateRange2 = _interopRequireDefault(_DateRange);

var _Calendar = __webpack_require__(46);

var _Calendar2 = _interopRequireDefault(_Calendar);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.DateRange = _DateRange2.default;
exports.Calendar = _Calendar2.default;

/***/ }),

/***/ 94:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _parseInput = __webpack_require__(31);

var _parseInput2 = _interopRequireDefault(_parseInput);

var _Calendar = __webpack_require__(46);

var _Calendar2 = _interopRequireDefault(_Calendar);

var _PredefinedRanges = __webpack_require__(96);

var _PredefinedRanges2 = _interopRequireDefault(_PredefinedRanges);

var _styles = __webpack_require__(21);

var _styles2 = _interopRequireDefault(_styles);

var _classnames = __webpack_require__(16);

var _classnames2 = _interopRequireDefault(_classnames);

var _constants = __webpack_require__(32);

var _defaultOptions = __webpack_require__(47);

var _defaultOptions2 = _interopRequireDefault(_defaultOptions);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var DateRange = function (_Component) {
  _inherits(DateRange, _Component);

  function DateRange(props, context) {
    _classCallCheck(this, DateRange);

    var _this = _possibleConstructorReturn(this, (DateRange.__proto__ || Object.getPrototypeOf(DateRange)).call(this, props, context));

    _this._handleDocumentClick = function (event) {
      if (_this.dateRangeInput.contains(event.target)) {
        return;
      }

      var clickOutside = _this.dateRangeContainer && !_this.dateRangeContainer.contains(event.target);

      if (clickOutside && _this.state.show) {
        _this.currentRange = Object.assign({}, _this.state.range);
        _this.hide();
      }
    };

    var format = props.format,
        linkedCalendars = props.linkedCalendars,
        theme = props.theme,
        locale = props.locale;


    var startDate = (0, _parseInput2.default)(props.startDate, format, 'startOf');
    var endDate = (0, _parseInput2.default)(props.endDate, format, 'endOf');
    _this.styles = (0, _styles2.default)(theme);
    _this.step = 0;
    _this.locale = _extends({}, _defaultOptions2.default.locale, props.locale);
    _this.ranges = _extends({}, _defaultOptions2.default.ranges(props.today), props.ranges);

    _this.currentRange = { startDate: startDate, endDate: endDate };
    _this.isValid = true;

    _this.state = {
      range: { startDate: startDate, endDate: endDate },
      link: linkedCalendars && endDate,
      show: false
    };
    return _this;
  }

  _createClass(DateRange, [{
    key: 'show',
    value: function show() {
      this.step = 0;
      this.setState({ show: true });
    }
  }, {
    key: 'hide',
    value: function hide() {
      this.setState({ show: false });
    }
  }, {
    key: 'componentDidMount',
    value: function componentDidMount() {
      var onInit = this.props.onInit;

      onInit && onInit(this.state.range);

      document.addEventListener('click', this._handleDocumentClick);
    }
  }, {
    key: 'componentWillUnmount',
    value: function componentWillUnmount() {
      document.removeEventListener('click', this._handleDocumentClick);
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
    value: function setRange(range, triggerChange, show) {
      var _this2 = this;

      var onChange = this.props.onChange;

      range = this.orderRange(range);
      this.isValid = this.isValidMonthRange(range);

      var callback = function callback() {
        triggerChange && onChange && onChange(range, _this2.isValid);
      };

      if (this.isValid) {
        var newState = show === undefined ? { range: range } : { range: range, show: show };
        this.setState(newState, callback);
      } else {
        onChange && onChange(range, this.isValid);
      }
    }
  }, {
    key: 'handleSelect',
    value: function handleSelect(date) {
      if (date.startDate && date.endDate) {
        //Select in range
        this.step = 0;
        var showDaterange = false,
            _triggerChange = true;
        this.currentRange = Object.assign({}, date);
        return this.setRange(date, _triggerChange, showDaterange);
      }

      var _state$range = this.state.range,
          startDate = _state$range.startDate,
          endDate = _state$range.endDate;


      var range = {
        startDate: startDate,
        endDate: endDate
      };

      switch (this.step) {
        case 0:
          range['startDate'] = date;
          range['endDate'] = date;
          this.step = 1;
          break;

        case 1:
          range['endDate'] = date;
          this.step = 0;
          break;
      }

      var triggerChange = !this.props.twoStepChange || this.step === 0 && this.props.twoStepChange;
      this.setRange(range, triggerChange);
    }
  }, {
    key: 'isValidMonthRange',
    value: function isValidMonthRange(date) {
      var limitedMonthRange = this.props.limitedMonthRange;


      if (limitedMonthRange) {
        var range = date.endDate.clone().subtract(date.startDate.month(), "month");
        if (range.month() > limitedMonthRange - 1) {
          return false;
        }
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
    key: 'handleApply',
    value: function handleApply() {
      var onClickApplyButton = this.props.onClickApplyButton;

      var range = this.state.range;
      onClickApplyButton && onClickApplyButton(range, this.isValid);

      this.currentRange = Object.assign({}, range);
      this.hide();
    }
  }, {
    key: 'componentWillReceiveProps',
    value: function componentWillReceiveProps(newProps) {
      // Whenever date props changes, update state with parsed variant
      if (newProps.startDate || newProps.endDate) {
        var format = newProps.format || this.props.format;
        var startDate = newProps.startDate && (0, _parseInput2.default)(newProps.startDate, format, 'startOf');
        var endDate = newProps.endDate && (0, _parseInput2.default)(newProps.endDate, format, 'endOf');
        var oldStartDate = this.props.startDate && (0, _parseInput2.default)(this.props.startDate, format, 'startOf');
        var oldEndDate = this.props.endDate && (0, _parseInput2.default)(this.props.endDate, format, 'endOf');

        if (!startDate.isSame(oldStartDate) || !endDate.isSame(oldEndDate)) {
          var range = {
            startDate: startDate || oldStartDate,
            endDate: endDate || oldEndDate
          };

          this.setRange(range);
        }
      }
    }
  }, {
    key: 'render',
    value: function render() {
      var _joiningClassNames,
          _this3 = this;

      var _props = this.props,
          showRange = _props.showRange,
          format = _props.format,
          linkedCalendars = _props.linkedCalendars,
          style = _props.style,
          calendars = _props.calendars,
          firstDayOfWeek = _props.firstDayOfWeek,
          minDate = _props.minDate,
          maxDate = _props.maxDate,
          classNames = _props.classNames,
          onlyClasses = _props.onlyClasses,
          specialDays = _props.specialDays,
          lang = _props.lang,
          disableDaysBeforeToday = _props.disableDaysBeforeToday,
          offsetPositive = _props.offsetPositive,
          shownDate = _props.shownDate,
          showMonthArrow = _props.showMonthArrow,
          rangedCalendars = _props.rangedCalendars;
      var _state = this.state,
          range = _state.range,
          link = _state.link;
      var styles = this.styles;


      var classes = _extends({}, _styles.defaultClasses, classNames);
      var yearsDiff = range.endDate.year() - range.startDate.year();
      var monthsDiff = range.endDate.month() - range.startDate.month();
      var diff = yearsDiff * 12 + monthsDiff;
      var calendarsCount = Number(calendars) - 1;

      var dateRangeClass = (0, _classnames2.default)((_joiningClassNames = {}, _defineProperty(_joiningClassNames, classes.dateRange, true), _defineProperty(_joiningClassNames, classes.show, this.state.show), _defineProperty(_joiningClassNames, classes.hide, !this.state.show), _joiningClassNames));

      var dateRangeStyle = {
        display: this.state.show ? "block" : "none"
      };

      var buttonHandlers = {
        apply: this.handleApply.bind(this),
        cancel: this.hide.bind(this)
      };

      return _react2.default.createElement(
        'div',
        { className: 'daterange-container' },
        _react2.default.createElement(
          'div',
          { className: 'daterange-label', onClick: function onClick() {
              return _this3.show();
            }, ref: function ref(element) {
              return _this3.dateRangeInput = element;
            } },
          _react2.default.createElement('input', { readOnly: true, name: 'DateRange', id: 'DateRange', className: 'input-text datepicker', type: 'text', autoComplete: 'off', value: this.dateRangeText }),
          _react2.default.createElement('i', { className: 'icon icon-calendar' })
        ),
        _react2.default.createElement(
          'div',
          { style: onlyClasses ? undefined : _extends({}, styles['DateRange'], style, dateRangeStyle),
            className: dateRangeClass,
            ref: function ref(element) {
              return _this3.dateRangeContainer = element;
            } },
          showRange && _react2.default.createElement(_PredefinedRanges2.default, {
            format: format,
            ranges: this.ranges,
            range: range,
            theme: styles,
            onSelect: this.handleSelect.bind(this),
            onlyClasses: onlyClasses,
            classNames: classes,
            locale: this.locale,
            buttonHandlers: buttonHandlers }),
          function () {
            var _calendars = [];
            var _method = offsetPositive ? 'unshift' : 'push';
            for (var i = calendarsCount; i >= 0; i--) {
              var offset = offsetPositive ? i : -i;
              var realDiff = offsetPositive ? diff : -diff;
              var realOffset = rangedCalendars && i == calendarsCount && diff != 0 ? realDiff : offset;
              _calendars[_method](_react2.default.createElement(_Calendar2.default, {
                showMonthArrow: showMonthArrow,
                shownDate: shownDate,
                disableDaysBeforeToday: disableDaysBeforeToday,
                lang: lang,
                key: i,
                offset: realOffset,
                link: linkedCalendars && link,
                linkCB: _this3.handleLinkChange.bind(_this3),
                range: range,
                format: format,
                firstDayOfWeek: firstDayOfWeek,
                theme: styles,
                minDate: minDate,
                maxDate: maxDate,
                onlyClasses: onlyClasses,
                specialDays: specialDays,
                classNames: classes,
                onChange: _this3.handleSelect.bind(_this3),
                locale: _this3.locale }));
            }
            return _calendars;
          }()
        )
      );
    }
  }, {
    key: 'dateRangeText',
    get: function get() {
      var date = this.currentRange;
      if (date.startDate && date.endDate) {
        return date.startDate.format(_constants.MomentFormat.default) + ' - ' + date.endDate.format(_constants.MomentFormat.default);
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
  offsetPositive: false,
  classNames: {},
  specialDays: [],
  rangedCalendars: false,
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
  offsetPositive: _propTypes2.default.bool,
  classNames: _propTypes2.default.object,
  rangedCalendars: _propTypes2.default.bool,
  onClickApplyButton: _propTypes2.default.func,
  showRange: _propTypes2.default.bool,
  limitedMonthRange: _propTypes2.default.number,
  today: _propTypes2.default.oneOfType([_propTypes2.default.object, _propTypes2.default.func, _propTypes2.default.string])
};

exports.default = DateRange;

/***/ }),

/***/ 95:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _classnames2 = __webpack_require__(16);

var _classnames3 = _interopRequireDefault(_classnames2);

var _styles = __webpack_require__(21);

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

/***/ 96:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _propTypes = __webpack_require__(2);

var _propTypes2 = _interopRequireDefault(_propTypes);

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _classnames2 = __webpack_require__(16);

var _classnames3 = _interopRequireDefault(_classnames2);

var _parseInput = __webpack_require__(31);

var _parseInput2 = _interopRequireDefault(_parseInput);

var _styles = __webpack_require__(21);

var _defaultOptions = __webpack_require__(47);

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
    _this.selectedItem = null;
    return _this;
  }

  _createClass(PredefinedRanges, [{
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

      var _props = this.props,
          ranges = _props.ranges,
          range = _props.range,
          onlyClasses = _props.onlyClasses;
      var styles = this.styles;


      return Object.keys(ranges).map(function (name) {
        var _classnames;

        var active = _this2.selectedItem === name;

        var style = _extends({}, styles['PredefinedRangesItem'], active ? styles['PredefinedRangesItemActive'] : {});

        var predefinedRangeClass = (0, _classnames3.default)((_classnames = {}, _defineProperty(_classnames, classes.predefinedRangesItem, true), _defineProperty(_classnames, classes.predefinedRangesItemActive, active), _classnames));

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

      var _props2 = this.props,
          style = _props2.style,
          onlyClasses = _props2.onlyClasses,
          locale = _props2.locale;

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
      var _props3 = this.props,
          style = _props3.style,
          onlyClasses = _props3.onlyClasses,
          classNames = _props3.classNames;
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

/***/ 97:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _langHelper = __webpack_require__(9);

var _langHelper2 = _interopRequireDefault(_langHelper);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = {
    months: [_langHelper2.default.getSingleResource('Jan'), _langHelper2.default.getSingleResource('Feb'), _langHelper2.default.getSingleResource('Mar'), _langHelper2.default.getSingleResource('Apr'), _langHelper2.default.getSingleResource('May'), _langHelper2.default.getSingleResource('Jun'), _langHelper2.default.getSingleResource('Jul'), _langHelper2.default.getSingleResource('Aug'), _langHelper2.default.getSingleResource('Sep'), _langHelper2.default.getSingleResource('Oct'), _langHelper2.default.getSingleResource('Nov'), _langHelper2.default.getSingleResource('Dec')],

    weekdays: [_langHelper2.default.getSingleResource('Su'), _langHelper2.default.getSingleResource('Mo'), _langHelper2.default.getSingleResource('Tu'), _langHelper2.default.getSingleResource('We'), _langHelper2.default.getSingleResource('Th'), _langHelper2.default.getSingleResource('Fr'), _langHelper2.default.getSingleResource('Sa')]
};

/***/ }),

/***/ 98:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _xlsxStyleSparta = __webpack_require__(114);

var XLSX = _interopRequireWildcard(_xlsxStyleSparta);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj.default = obj; return newObj; } }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EXCEL_TYPE = "xlsx";
var MapExcelType = {
    "number": "n", "string": "s", "boolean": "b", "object": "d"
};

var Excel = function () {
    function Excel() {
        _classCallCheck(this, Excel);
    }

    _createClass(Excel, null, [{
        key: "export",
        value: function _export(worksheet, fileName) {
            var option = { bookType: EXCEL_TYPE, bookSST: false, type: 'binary' };
            var workbook = { SheetNames: [fileName], Sheets: {}, Props: {} };
            workbook.Sheets[fileName] = worksheet;

            var workbookOutput = XLSX.write(workbook, option);
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
            return XLSX.utils.encode_cell(cellObject);
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

/***/ 99:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = __webpack_require__(1);

var _react2 = _interopRequireDefault(_react);

var _reactDom = __webpack_require__(7);

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

/***/ })

},[717]);
//# sourceMappingURL=notification.chunk.js.map