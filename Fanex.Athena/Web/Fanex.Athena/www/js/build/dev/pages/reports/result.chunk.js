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
/******/ 		"www/js/build/dev/pages/reports/result": 0
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
/******/ 	deferredModules.push([29,"www/js/build/dev/pages/lib/common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./www/js/source/components/jquery.flatpopup.js":
/*!******************************************************!*\
  !*** ./www/js/source/components/jquery.flatpopup.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
    var $blockBody;
    var DEFAULT_MODAL = true;

    var _methods = {
        init: function (options) {
            var defaultOptions = {
                width: 640,
                height: 480,
                padding: 0,
                id: "popup-content",
                className: "",
                title: "&nbsp;",
                container: "body",
                modal: DEFAULT_MODAL,
                onOpen: null,
                onOpened: null,
                onClose: null
            };

            $blockBody = $(".popup-block-body");
            options = $.extend(defaultOptions, options || {});
            return this.each(function () {
                var $self = $(this);
                var $container = $(options.container);

                if (options.modal && $blockBody.length === 0) {
                    $blockBody = $('<div class="popup-block-body" style="display: none"></div>');
                    $container.append($blockBody);
                }
                
                var $popupElement = $("#" + options.id);
                if ($popupElement.length === 0 || $popupElement.hasClass('flat-popup') === false) {
                    $self.attr("id", options.id);
                    $self.addClass("flat-popup " + options.className);
                    $self.children().wrapAll("<div class='wrapContent' style='padding:" + options.padding + "px'></div>");
                    $self.hide()
                         .prepend('<div class="title-bar popup-title"><div class="txt-title">'
                             + options.title
                             + '</div><a class="popup-close" href="javascript:;"><i class="icon-close"></i></a></div>');

                    $self.find('a.popup-close').on('click', function () {
                        if (options.onClose) {
                            options.onClose.apply($self);
                        }

                        if ($self.find("iframe").length > 0) {
                            $self.find(".wrapContent").empty();
                        }
                        _methods.hide.apply($self);
                    });

                    $container.append($self);
                }
                else {
                    $self = $("#" + options.id);
                    $self.find(".txt-title").text(options.title);
                    var $wrapContent = $self.find(".wrapContent");
                    $wrapContent.append($self.children().not(".popup-title"));
                }

                $self.width(options.width);
                $self.height(options.height);

                if (options.onOpen) {
                    options.onOpen.apply($self);
                }

                _methods.show.call($self, options.modal);

                if (options.onOpened) {
                    setTimeout(options.onOpened.apply($self), 350);
                }
            });
        },

        show: function (modal) {
            var $self = this;
            //Set the center alignment padding + border see css style
            var left = ($(window).width() - $self.width()) / 2;
            var top = ($(window).height() - $self.height()) / 2;

            if (top > 300) {
                top -= 100;
            }

            $self.css({
                'top': (top < 0 ? 0 : top) + "px",
                'left': (left < 0 ? 0 : left) + "px"
            });

            if (modal === true || modal === undefined) {
                $blockBody.show();
            }

            $self.fadeIn(300);
        },

        hide: function () {
            $(this).fadeOut(300);
            $blockBody.hide();
            return false;
        }
    };

    $.fn.flatPopup = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            // Default to "init"
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.flatPopup');
        }
    };
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/components/pluginhelper/daterangepicker.js":
/*!******************************************************************!*\
  !*** ./www/js/source/components/pluginhelper/daterangepicker.js ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! moment */ "./node_modules/moment/moment.js"), __webpack_require__(/*! bootstrap-daterangepicker */ "./node_modules/bootstrap-daterangepicker/daterangepicker.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, moment) {
        "use strict";

        var _daysOfWeek, _monthNames;

        function initCommonTextResource() {
            if (!_daysOfWeek || !_monthNames) {
                _daysOfWeek = [
                    Site.getSingleResource('Su'), Site.getSingleResource('Mo'), Site.getSingleResource('Tu'), Site.getSingleResource('We'),
                    Site.getSingleResource('Th'), Site.getSingleResource('Fr'), Site.getSingleResource('Sa')
                ];

                _monthNames = [
                    Site.getSingleResource('Jan'), Site.getSingleResource('Feb'), Site.getSingleResource('Mar'), Site.getSingleResource('Apr'),
                    Site.getSingleResource('May'), Site.getSingleResource('Jun'), Site.getSingleResource('Jul'), Site.getSingleResource('Aug'),
                    Site.getSingleResource('Sep'), Site.getSingleResource('Oct'), Site.getSingleResource('Nov'), Site.getSingleResource('Dec')
                ];
            }
        }

        function initDateRangePicker(firstDayOfWeekIsMonday, rangeOption) {
            var $dateRange = $('#DateRange');

            var $from = $('#FromDate');
            var $to = $('#ToDate');
            var start = moment($from.attr('data-original-from'), "MM/DD/YYYY");
            var end = moment($to.attr('data-original-to'), "MM/DD/YYYY");

            function setRangeDate(start, end) {
                $from.val(start.format('MM/DD/YYYY'));
                $to.val(end.format('MM/DD/YYYY'));
                $dateRange.val(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
            }

            var now = moment($dateRange.attr('data-today'), "MM/DD/YYYY");
            var isoDayOfWeek = firstDayOfWeekIsMonday ? 1 : 7; // 1.Monday  7.Sunday
            var startDayOfWeek = now.clone().startOf('week').isoWeekday(isoDayOfWeek);
            var startDayOfLastWeek = now.clone().subtract(1, 'week').startOf('week').isoWeekday(isoDayOfWeek);

            var ranges = {};
            ranges[Site.getSingleResource('Today')] = [now, now];
            ranges[Site.getSingleResource('Yesterday')] = [now.clone().subtract(1, 'days'), now.clone().subtract(1, 'days')];
            ranges[Site.getSingleResource('Currentweek')] = [startDayOfWeek, now];
            ranges[Site.getSingleResource('Lastweek')] = [startDayOfLastWeek, startDayOfLastWeek.clone().add(6, 'days')];
            ranges[Site.getSingleResource('Currentmonth')] = [now.clone().startOf('month'), now];
            ranges[Site.getSingleResource('Lastmonth')] = [now.clone().subtract(1, 'month').startOf('month'), now.clone().subtract(1, 'month').endOf('month')];
            ranges[Site.getSingleResource('Sincelastmonth')] = [now.clone().subtract(1, 'month').startOf('month'), now];

            var customRanges = {};
            var hasRangeOption = rangeOption && rangeOption.length > 0;
            if (hasRangeOption) {
                for (var i = 0; i < rangeOption.length; i++) {
                    var rangeLabel = Site.getSingleResource(rangeOption[i]);
                    customRanges[rangeLabel] = ranges[rangeLabel];
                }
            }

            initCommonTextResource();
            var locale = {
                format: 'MM/DD/YYYY',
                applyLabel: Site.getSingleResource('OK'),
                cancelLabel: Site.getSingleResource('Cancel'),
                daysOfWeek: _daysOfWeek,
                monthNames: _monthNames,
                firstDay: firstDayOfWeekIsMonday ? 1 : 0
            };

            $dateRange.daterangepicker({
                startDate: start,
                endDate: end,
                minDate: moment($dateRange.attr('data-mindate'), "MM/DD/YYYY"),
                maxDate: moment($dateRange.attr('data-maxdate'), "MM/DD/YYYY"),
                ranges: hasRangeOption ? customRanges : ranges,
                locale: locale,
                showDropdowns: true,
                linkedCalendars: false,
                alwaysShowCalendars: true,
                showCustomRangeLabel: false
            }, setRangeDate);

            $dateRange.on('apply.daterangepicker', function (ev, picker) {
                setRangeDate(picker.startDate, picker.endDate);
            });

            $dateRange.on('show.daterangepicker', function (ev, picker) {
                var toDate = picker.endDate || new Date();
                var toMoment = moment(toDate);

                picker.rightCalendar.month = toMoment;
                picker.renderCalendar("right");
            });

            $('.date-range').on("click", ".icon-calendar", function () {
                $dateRange.data('daterangepicker').toggle();
            });

            setRangeDate(start, end);
        }

        function initSingleDatePicker(idElement, parentElement) {
            var $picker = $("#" + idElement);
            initCommonTextResource();
            var locale = {
                format: 'MM/DD/YYYY',
                daysOfWeek: _daysOfWeek,
                monthNames: _monthNames
            };

            $picker.daterangepicker({
                startDate: moment($picker.attr('data-original'), "MM/DD/YYYY"),
                endDate: moment($picker.attr('data-original'), "MM/DD/YYYY"),
                minDate: moment($picker.attr('data-mindate'), "MM/DD/YYYY"),
                maxDate: moment($picker.attr('data-maxdate'), "MM/DD/YYYY"),
                locale: locale,
                singleDatePicker: true,
                showDropdowns: true,
                parentEl: parentElement && parentElement !== "" ? parentElement : ".single-date"
            });

            $('.single-date').on("click", ".icon-calendar", function () {
                $picker.data('daterangepicker').toggle();
            });
        }

        function setValueForDateRange(startDate, endDate) {
            if (startDate) {
                $('#DateRange').data('daterangepicker').setStartDate(startDate);
                $('#FromDate').val(startDate);
            }

            if (endDate) {
                $('#DateRange').data('daterangepicker').setEndDate(endDate);
                $('#ToDate').val(startDate);
            }
        }

        function setValueForSingleDate($element, dateValue) {
            $element.val(dateValue);
        }

        return {
            initDateRangePicker: initDateRangePicker,
            initSingleDatePicker: initSingleDatePicker,
            setValueForDateRange: setValueForDateRange,
            setValueForSingleDate: setValueForSingleDate
        };
    }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/components/scrollablegrid.js":
/*!****************************************************!*\
  !*** ./www/js/source/components/scrollablegrid.js ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
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
                $grid.addClass("noScroll").find("table").css("width", "");
            }
            else {
                $grid.removeClass("noScroll").find("table").css("width", $grid.width() - 17);
            }
        }
    };

    return _me;
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/components/selectlist.js":
/*!************************************************!*\
  !*** ./www/js/source/components/selectlist.js ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
    "use strict";

    var CONTAINER_SELECTOR = ".select-list-container";
    var DISABLED_CLASS = "disabled";
    var HIGHLIGHT_CLASS = "highlighted";

    function getLabelHtml(option) {
        var targetDropdownId = option.selectionId + "-result-container";
        var className = option.isDisabled ? "selection disabled" : "selection";
        return '<div class="select-list-container">' +
                '<div class="' + className + '" id="' + option.selectionId + '" aria-targetDropdown="' + targetDropdownId + '">' +
                    '<div class="selection-label">' + option.labelText + '</div>' +
                    '<div class="selection-icon"><span class="selection-icon-arrow"></span></div>' +
                '</div>' +
            '</div>';
    }

    function getSearchBoxHtml(isShow) {
        return isShow ? '<div class="select-list-dropdown-search"><input class="select-list-search-results" autocomplete="off" /></div>' : '';
    }

    function getOptionItemHtml(dataItem) {
        return '<li class="option-item ' + dataItem.className + '" data-key="' + dataItem.key + '">' + dataItem.value + '</li>';
    }

    function getOptionListHtml(dataList) {
        var htmlArray = ['<div class="select-list-dropdown-results"><ul class="option-list">'];

        for (var i = 0; i < dataList.length; i++) {
            var dataItem = dataList[i];
            htmlArray.push(getOptionItemHtml(dataItem));
        }

        htmlArray.push('</ul></div>');

        return htmlArray.join("");
    }

    function scrollToHighLightItem($container, $item, index) {
        var itemTop = $item.offset().top;
        var itemHeight = $item.outerHeight();
        var containerTop = $container.offset().top;
        var scrollUp = null;
        if (itemTop <= containerTop) {
            scrollUp = true;
        }
        else if (itemTop >= containerTop + $container.height()) {
            scrollUp = false;
        }
        if (scrollUp === null) {
            return;
        }
        if (scrollUp) {
            $container.scrollTop(itemHeight * (index - 1) - $container.height());
        }
        else {
            $container.scrollTop(itemHeight * index);
        }
    }

    function keyUp($element) {
        var $selectList = $element.closest(".select-list-dropdown").find(".select-list-dropdown-results");
        var showOptionList = $selectList.find(".option-item:visible");
        var $highlightItem = $selectList.find(".option-item." + HIGHLIGHT_CLASS + ":visible").first();
        var index = showOptionList.index($highlightItem);

        $highlightItem.removeClass(HIGHLIGHT_CLASS);
        if ($highlightItem.length > 0 && index !== 0) {
            var $preItem = showOptionList.eq(index - 1);
            $preItem.addClass(HIGHLIGHT_CLASS);
            scrollToHighLightItem($selectList, $preItem, index - 1);
        }
        else {
            showOptionList.last().addClass(HIGHLIGHT_CLASS);
            $selectList.scrollTop(showOptionList.length * $highlightItem.outerHeight());
        }
    }

    function keyDown($element) {
        var $selectList = $element.closest(".select-list-dropdown").find(".select-list-dropdown-results");
        var showOptionList = $selectList.find(".option-item:visible");
        var $highlightItem = $selectList.find(".option-item." + HIGHLIGHT_CLASS + ":visible").first();
        var index = showOptionList.index($highlightItem);

        $highlightItem.removeClass(HIGHLIGHT_CLASS);
        if ($highlightItem.length > 0 && index !== (showOptionList.length - 1)) {
            var $nextItem = showOptionList.eq(index + 1);
            $nextItem.addClass(HIGHLIGHT_CLASS);
            scrollToHighLightItem($selectList, $nextItem, index + 1);
        }
        else {
            showOptionList.first().addClass(HIGHLIGHT_CLASS);
            $selectList.scrollTop(0);
        }
    }

    function keySearch($element, canEnter) {
        setTimeout(function () {
            var query = $element.val().toUpperCase();
            var $selectList = $element.closest(".select-list-dropdown").find(".select-list-dropdown-results");
            var optionList = $selectList.find(".option-item");
            var isFirstItem = canEnter;
            if (canEnter) {
                optionList.removeClass(HIGHLIGHT_CLASS);
            }
            optionList.each(function (index, item) {
                var $item = $(item);
                var currentText = $item.text().toUpperCase();
                if (currentText.indexOf(query) === -1) {
                    $item.hide();
                }
                else {
                    if (isFirstItem) {
                        $item.addClass(HIGHLIGHT_CLASS);
                        isFirstItem = false;
                        $selectList.scrollTop(0);
                    }
                    $item.show();
                }
            });
        }, 100);
    }

    function enter($element) {
        var $highlightItem = $element.closest(".select-list-dropdown").find(".option-item." + HIGHLIGHT_CLASS + ":visible");
        $highlightItem.click();
    }

    function getDropdownList(resultContainerId, canSearch, canEnter) {
        /*jshint validthis: true*/
        var $originalSelectElement = $(this);
        var dataList = $originalSelectElement.find("option").map(function (index, item) {
            var key = item.value;
            var text = item.text;

            return { key: key, value: text, className: item.className };
        });
        var selectedValue = $originalSelectElement.val();

        var dropdownListHtml = '<div id="' + resultContainerId + '" class="select-list-result-container"><div class="select-list-dropdown">' +
                                getSearchBoxHtml(canSearch) + getOptionListHtml(dataList) +
                            '</div></div>';
        var $dropdown = $(dropdownListHtml);
        $dropdown.find('.option-item[data-key="' + selectedValue + '"]').addClass(HIGHLIGHT_CLASS);

        $dropdown.on("click", ".option-item", function () {
            $(".option-item").removeClass(HIGHLIGHT_CLASS);
            $(this).addClass(HIGHLIGHT_CLASS);
            var selectedKey = $(this).data("key");
            $originalSelectElement.val(selectedKey);

            $originalSelectElement.trigger("change");
        });

        var $searchBox = $dropdown.find(".select-list-search-results");

        $searchBox.on("click", function (e) {
            e.stopPropagation();
        });

        if (canEnter) {
            $searchBox.on("keyup", function (e) {
                var $this = $(this);
                if (e.keyCode === 40) {
                    keyDown($this);
                }
                else if (e.keyCode === 38) {
                    keyUp($this);
                }
                else if (e.keyCode === 13) {
                    enter($this);
                }
                else {
                    keySearch($this, true);
                }
            });
        }
        else {
            $searchBox.on("keyup", function () {
                var $this = $(this);
                keySearch($this, false);
            });
        }

        return $dropdown;
    }

    function generateId($element) {
        var id = '';

        if ($element.attr('id') !== null) {
            id = $element.attr('id');
        } else if ($element.attr('name') !== null) {
            id = $element.attr('name') + '-' + generateChars(2);
        } else {
            id = generateChars(4);
        }

        id = id.replace(/(:|\.|\[|\]|,)/g, '');
        id = 'select-list-' + id;

        return id;
    }

    function generateChars(length) {
        var chars = '';

        for (var i = 0; i < length; i++) {
            var randomChar = Math.floor(Math.random() * 36);
            chars += randomChar.toString(36);
        }

        return chars;
    }

    function closeAll() {
        $(".select-list-result-container").remove();
    }

    var defaultOption = {
        canSearch: false,
        canEnter: false,
        isDisabled: false,
        beforeOpen: function () { },
        afterOpen: function () { }
    };

    var _methods = {
        init: function (options) {
            var selectListOption = $.extend({}, defaultOption, options || {});

            return this.each(function () {
                var self = this;
                var $element = $(self);
                if (!selectListOption.canSearch) {
                    selectListOption.canSearch = $element.attr("data-can-search") === "1";
                }
                if (!selectListOption.canEnter) {
                    selectListOption.canEnter = $element.attr("data-can-enter") === "1";
                }

                _methods.buildLabel.call(self);

                var $container = $element.parent();
                $container.off("click", ".selection");
                $container.on("click", ".selection", function () {
                    var $selection = $(this);
                    if (!$selection.hasClass(DISABLED_CLASS)) {
                        _methods.toggle.call(self, selectListOption);
                    }
                });

                $element.on("change", function () {
                    _methods.update.call(self);
                });

                $("html").on("click", function (e) {
                    var $target = $(e.target);

                    if (!$target.closest(CONTAINER_SELECTOR).length) {
                        closeAll();
                    }
                });
            });
        },

        buildLabel: function (option) {
            option = option || {};
            var $self = $(this);
            $self.siblings(CONTAINER_SELECTOR).remove();
            var optionSelected = $self.find("option:selected");
            var labelText = optionSelected.length ? optionSelected.text() : "";
            var selectionId = generateId($self);

            option.labelText = labelText;
            option.selectionId = selectionId;
            var $selectionLabel = $(getLabelHtml(option));

            $self.hide().parent().append($selectionLabel);
        },

        open: function (resultContainerId, canSearch, canEnter) {
            closeAll();

            var $self = $(this);
            var $dropdown = getDropdownList.call(this, resultContainerId, canSearch, canEnter);

            var $selection = $self.siblings(CONTAINER_SELECTOR).find(".selection");
            var offset = $selection.offset();
            $dropdown.css({ top: (offset.top + 26) + 'px', left: offset.left + 'px', width: $selection.width() + 'px' });

            $("body").append($dropdown);
            $dropdown.find(".select-list-search-results").focus();
        },

        close: function () {
            var $this = $(this);
            var $selection = $this.siblings(CONTAINER_SELECTOR).find(".selection");
            var resultContainerId = $selection.attr("aria-targetDropdown");
            $("#" + resultContainerId).remove();
        },

        toggle: function (option) {
            var $this = $(this);
            var $selection = $this.siblings(CONTAINER_SELECTOR).find(".selection");
            var resultContainerId = $selection.attr("aria-targetDropdown");

            if (resultContainerId && resultContainerId.length > 0) {
                var $currentDropdown = $("#" + resultContainerId);
                var isShowing = $currentDropdown.length > 0;

                if (isShowing) {
                    $currentDropdown.remove();
                }
                else {
                    if (typeof option.beforeOpen === "function") {
                        option.beforeOpen.call(this);
                    }

                    _methods.open.call(this, resultContainerId, option.canSearch, option.canEnter);

                    if (typeof option.afterOpen === "function") {
                        option.afterOpen.call(this);
                    }
                }
            }
        },

        update: function (option) {
            _methods.buildLabel.call(this, option);
        },

        disabled: function (isDisabled) {
            var $selection = $(this).siblings(CONTAINER_SELECTOR).find(".selection");
            $selection.toggleClass(DISABLED_CLASS, !!isDisabled);
        }
    };

    $.fn.selectList = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on selectList');
        }
    };
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/extensions/jquery.extension.js":
/*!******************************************************!*\
  !*** ./www/js/source/extensions/jquery.extension.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(jQuery) {﻿/// <reference path="/@JSense.js" />

(function ($) {
    $.fn.checkboxgroup = function () {
        var $parent = $(this).parent();
        var allChk = this.first();
        var elseChk = this.slice(1);

        allChk.prop('checked', elseChk.filter(":checked").length === elseChk.length);

        allChk.change(function () {
            var thisIsChecked = $(this).is(':checked');
            elseChk.prop('checked', thisIsChecked);
            $parent.toggleClass("invalid", !thisIsChecked);
        });

        elseChk.change(function () {
            if ($(this).is(':checked')) {
                allChk.prop('checked', elseChk.filter(":checked").length === elseChk.length);
            } else {
                allChk.prop('checked', false);
            }

            $parent.toggleClass("invalid", elseChk.filter(":checked").length === 0);
        });

        return this;
    };

    $.fn.showHide = function (isShow) {
        if (isShow) {
            this.show();
        } else {
            this.hide();
        }

        return this;
    };

    jQuery.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    jQuery.fn.tableRowSpan = function (colIndexs, effectToIndex) {
        /// <summary>
        /// Created by Alex@20130228
        /// set table rowspan
        /// </summary>
        /// <param name="colIndexs">column indexs</param>
        /// <param name="effectToIndex">set rowspan for this columns base on lasted column in [colIndexs]</param>
        if (effectToIndex !== undefined) {
            if ($.inArray(effectToIndex, colIndexs) > -1) {
                return;
            }
        }

        function process(table, colIdx) {
            var that;
            var ethat;
            var ethis;
            var rowspan = 1;
            var len = table.rows.length - 1;
            for(var index = 0; index <= len; index++)
            {
                var currentRow = table.rows[index];                
                var td = currentRow.cells[colIdx];
                if(td) {
                    ethis = currentRow.cells[effectToIndex];
                    var found = that != null && $(td).html() === $(that).html();
                    if (found && index < len) {
                        rowspan++;
                        $(td).hide();
                        if (effectToIndex !== undefined) {
                            $(ethis).hide();
                        }
                        //.remove// do your action for the old cell here
                    }
                    else if (found && index === len) {
                        //do your action for lasted row
                        $(td).hide();
                        rowspan++;
                        $(that).attr("rowSpan", rowspan);
                        that = td;
                        if (effectToIndex !== undefined) {
                            $(ethis).hide();
                            $(ethat).attr("rowSpan", rowspan);
                            ethat = ethis;
                        }
                        rowspan = 1;
                        // do your action for the colSpan cell here
                    }
                    else {
                        if (effectToIndex !== undefined) {
                            $(ethat).attr("rowSpan", rowspan);
                            ethat = ethis;
                        }
                        $(that).attr("rowSpan", rowspan);
                        // do your action for the colSpan cell here
                        that = td;
                        rowspan = 1;
                    }
                    that = (that == null) ? td : that;
                    if (effectToIndex !== undefined) {
                        ethat = (ethat == null) ? ethis : ethat;
                    }
                    // set the that if not already set
                }
            }
        }

        return this.each(function () {
            for (var i = 0; i < colIndexs.length; i++) {
                process(this, colIndexs[i]);
            }
        });
    };

    jQuery.fn.numbericTextBox = function (min, max) {
        var thousandFormater = function (x) {
            x = x.toString();
            var pattern = /(-?\d+)(\d{3})/;
            while (pattern.test(x)) {
                x = x.replace(pattern, "$1,$2");
            }
            return x;
        };

        var maxLength = max.toString().length;

        this.each(function (i, e) {
            $(e).keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode === 46 || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 27 || event.keyCode === 13 ||
                // Allow: Ctrl+A
                    (event.keyCode === 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            }).keyup(function (event) {
                var textBox = event.target;
                var value = textBox.value.replace(/,/g, '');

                if (value.length > maxLength) {
                    value = value.slice(0, maxLength);
                }

                if (value < min) {
                    value = min;
                }
                else if (value > max) {
                    value = max;
                }

                textBox.value = thousandFormater(value);
            });
        });

        return this;
    };

    $.fn.hasVerticalScrollBar = function () {
        return this.length > 0 ? this.get(0).scrollHeight > this.height() : false;
    };
    $.fn.hasHorizontalScrollBar = function () {
        return this.length > 0 ? this.get(0).scrollWidth > this.width() : false;
    };
})(jQuery);

jQuery.extend({
    format: function () {
        /// <summary>
        /// simply format method
        /// </summary>
        /// <returns type="string">formated string</returns>
        var formatted = arguments[0];
        var passValue = jQuery.grep(arguments, function (n, i) { return i > 0; });
        for (var arg in passValue) {
            formatted = formatted.replace("{" + arg + "}", passValue[arg]);
        }
        return formatted;
    }
});
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")))

/***/ }),

/***/ "./www/js/source/extensions/prototype.js":
/*!***********************************************!*\
  !*** ./www/js/source/extensions/prototype.js ***!
  \***********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
﻿/// <reference path="/@JSense.js" />
///
/*
* @Alex-1/11/2013
* The below function will work adequatly for the six simple types I mentioned,
* as long as data in the objects and arrays form a tree structure.That is, there isn't
* more than one reference to the same data in the object.
*
*/

//#region String Prototype

/* jshint ignore:start */
String.prototype.format = function () {
    /// <summary>
    /// simply format method
    /// </summary>
    /// <returns type="string">formated string</returns>
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        formatted = formatted.replace("{" + i + "}", arguments[i]);
    }
    return formatted;
};
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, '');
};

String.prototype.ltrim = function () {
    return this.replace(/^\s*/g, '');
};

String.prototype.rtrim = function () {
    return this.replace(/\s*$/g, '');
};
String.prototype.replaceAll = function (token, newToken, ignoreCase) {
    var _token;
    var str = this + "";
    var i = -1;

    if (typeof token === "string") {
        if (ignoreCase) {
            _token = token.toLowerCase();

            while ((
                i = str.toLowerCase().indexOf(
                    token, i >= 0 ? i + newToken.length : 0
                )) !== -1
            ) {
                str = str.substring(0, i) +
                    newToken +
                    str.substring(i + token.length);
            }
        } else {
            return this.split(token).join(newToken);
        }
    }
    return str;
};

//#endregion

//#region Array prototype

Array.prototype.remove = function (value, fieldName) {
    /// <summary>
    ///
    /// Remove specific element in array
    /// </summary>
    /// <param name="value"> element or value of field in object</param>
    /// <param name="fieldName">Name of a field in object</param>
    for (var i = 0; i < this.length; ++i) {
        if (typeof this[i] == 'object' && fieldName !== undefined) {
            if (this[i][fieldName] !== undefined && this[i][fieldName] == value) {
                this.splice(i, 1);
                return;
            }
        } else {
            if (this[i] == value) {
                this.splice(i, 1);
                return;
            }
        }
    }
};
Array.prototype.insertAfter = function (value, after) {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">element</param>
    /// <param name="after">position</param>
    this.splice(after + 1, 0, value);
};
Array.prototype.insertBefore = function (value, before) {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">element</param>
    /// <param name="before">position</param>
    this.splice(before, 0, value);
};

//#endregion

/* jshint ignore:end */

/***/ }),

/***/ "./www/js/source/pages/report/result/main-result.js":
/*!**********************************************************!*\
  !*** ./www/js/source/pages/report/result/main-result.js ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! ./result */ "./www/js/source/pages/report/result/result.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function (Result) {
    $(Result.initialize);
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")))

/***/ }),

/***/ "./www/js/source/pages/report/result/result.js":
/*!*****************************************************!*\
  !*** ./www/js/source/pages/report/result/result.js ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! components/pluginhelper/daterangepicker */ "./www/js/source/components/pluginhelper/daterangepicker.js"), __webpack_require__(/*! common/popup */ "./www/js/source/common/popup.js"), __webpack_require__(/*! components/scrollablegrid */ "./www/js/source/components/scrollablegrid.js"), __webpack_require__(/*! extensions/jquery.extension */ "./www/js/source/extensions/jquery.extension.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, DatePicker, Popup, GridHelper) {
        var DATA_API = "ResultReport/Index";
        var EXCEL_API = "ResultReport/ResultExcel?";
        var SPORT_API = "ResultReport/GetSports";
        var KENO_RESULT_API = "ResultKenoReport/Index";
        var KENO_RESULT_SESSION_API = "ResultKenoReport/KeepSession";
        var ajaxRequest = null;
        var ajaxFilterRequest = null;
        var kenoAjaxRequest = null;
        var kenoIntervalId = 0;
        var lotteryAjaxRequest = null;
        var lotteryIntervalId = 0;
        var param = {
            FromDate: "",
            ToDate: "",
            IsOutRight: false,
            IsExportExcel: false,
            SportId: 0,
            LeagueId: 0
        };

        var ACTIVE_CLASS = "active";

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker("Date");
                DatePicker.initDateRangePicker();
                _me.initEvent();
            },

            initEvent: function () {
                var $submitOutright = $("#submitOutright");

                $("#resultContent").on("click", ".detail-link", function () {
                    var url = Site.resolveClientUrl("ResultReport/ResultDetail")
                     + "?matchid=" + $(this).attr("data-match-id") + "&SportType=" + param.SportId;

                    var popupOption = { id: "result-popup", width: 775, height: 520, title: Site.getSingleResource("Result") };
                    Popup.openPopupWithIframe(url, popupOption);
                });

                $submitOutright.click(function () {
                    if (_me.canSubmit(this)) {
                        _me.showOutRight(true);
                        var startDate = $("#Date").val();
                        DatePicker.setValueForDateRange(startDate);
                        _me.submit();
                    }
                });

                $("#submitNormal").click(function () {
                    if (_me.canSubmit(this)) {
                        _me.showOutRight(false);
                        DatePicker.setValueForSingleDate($("#Date"), $('#FromDate').val());
                        _me.submit();
                    }
                });

                $("#SubmitButton").click(function () {
                    _me.submit();
                });

                $("#submitToday").click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    var date = $(this).attr("data-date");
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    param.FromDate = date;
                    param.ToDate = date;
                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;

                    _me.updateDateInput(isShowOutright);
                    _me.getData();
                });

                $("#submitYesterday").click(function () {
                    _me.clearExcelExport();
                    _me.clearResult();
                    var date = $(this).attr("data-date");
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    param.FromDate = date;
                    param.ToDate = date;
                    param.IsExportExcel = false;
                    param.IsOutRight = isShowOutright;
                    param.SportId = 0;
                    param.LeagueId = 0;

                    _me.updateDateInput(isShowOutright);
                    _me.getData();
                });

                $("#ExportToExcel").click(function (e) {
                    e.preventDefault();
                    var isShowOutright = $submitOutright.attr("data-isoutright") === "1";
                    _me.setDateParam(isShowOutright);

                    param.IsExportExcel = true;
                    param.IsOutRight = isShowOutright;
                    param.SportId = $("#ddlSport option").length > 0 ? $("#ddlSport").val() : 0;
                    param.LeagueId = $("#ddlLeague").val();
                    _me.getData();
                });

                $("#ddlSport").change(function () {
                    _me.clearExcelExport();
                    _me.clearLeagueFilter();

                    param.SportId = $(this).val();
                    param.LeagueId = 0;
                    _me.getData();
                });

                $("#ddlLeague").change(function () {
                    _me.clearExcelExport();
                    param.LeagueId = $(this).val();
                    _me.getReportData();
                });

                $(window).on("resize", function () {
                    GridHelper.resizeGrid($(".fn-rs-content-scroll"));
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });

                if ($("#submitKeno").length) {
                    $("#submitKeno").click(function () {
                        _me.showKeno($(this).data("vendorproductid"));
                    });
                }

                if ($("#submitLottery").length) {
                    $("#submitLottery").click(function () {
                        _me.showLottery($(this).data("vendorproductid"));
                    });
                }
            },

            submit: function () {
                _me.clearExcelExport();
                _me.clearResult();
                var isShowOutright = $("#submitOutright").attr("data-isoutright") === "1";
                _me.setDateParam(isShowOutright);

                param.IsExportExcel = false;
                param.IsOutRight = isShowOutright;
                param.SportId = 0;
                param.LeagueId = 0;
                _me.getData();
            },

            showKeno: function (vendorProductId) {
                var option = {
                    type: "GET",
                    cache: false,
                    data: { vendorProductId: vendorProductId },
                    url: Site.resolveClientUrl(KENO_RESULT_API),
                    success: function (response) {
                        window.open(response.Url, '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
                        clearInterval(kenoIntervalId);
                        _me.extendKenoSession(vendorProductId, response.UserId);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                _me.stopAjaxRequest(kenoAjaxRequest);
                kenoAjaxRequest = $.ajax(option);
            },

            showLottery: function (vendorProductId) {
                var option = {
                    type: "GET",
                    cache: false,
                    data: { vendorProductId: vendorProductId },
                    url: Site.resolveClientUrl(KENO_RESULT_API),
                    success: function (response) {
                        window.open(response.Url, '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
                        clearInterval(lotteryIntervalId);
                        _me.extendLotterySession(vendorProductId, response.UserId);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                _me.stopAjaxRequest(lotteryAjaxRequest);
                lotteryAjaxRequest = $.ajax(option);
            },

            extendKenoSession: function (vendorProductId, userId) {
                if (!userId)
                {
                    return;
                }

                kenoIntervalId = setInterval(function () {
                    var option = {
                        type: "POST",
                        cache: false,
                        data: {
                            userId: userId,
                            vendorProductId: vendorProductId
                        },
                        url: Site.resolveClientUrl(KENO_RESULT_SESSION_API)
                    };
                    _me.stopAjaxRequest(kenoAjaxRequest);
                    kenoAjaxRequest = $.ajax(option);
                }, 6000);
            },

            extendLotterySession: function (vendorProductId, userId) {
                if (!userId) {
                    return;
                }

                lotteryIntervalId = setInterval(function () {
                    var option = {
                        type: "POST",
                        cache: false,
                        data: {
                            userId: userId,
                            vendorProductId: vendorProductId
                        },
                        url: Site.resolveClientUrl(KENO_RESULT_SESSION_API)
                    };
                    _me.stopAjaxRequest(lotteryAjaxRequest);
                    lotteryAjaxRequest = $.ajax(option);
                }, 6000);
            },

            getData: function () {
                if (param.IsExportExcel) {
                    _me.exportExcel();
                } else {
                    _me.getFilter();
                }
            },

            exportExcel: function () {
                var url = Site.resolveClientUrl(EXCEL_API);
                url += "FromDate=" + param.FromDate;
                url += "&ToDate=" + param.ToDate;
                url += "&IsOutRight=" + param.IsOutRight;
                url += "&SportId=" + param.SportId;
                url += "&LeagueId=" + param.LeagueId;
                url += "&IsExportExcel=" + param.IsExportExcel;

                $('#iframeForExportExcel').attr('src', url);
            },

            getFilter: function () {
                var url = Site.resolveClientUrl(SPORT_API);
                var option = {
                    type: "POST",
                    dataType: "json",
                    data: param,
                    cache: false,
                    url: url,
                    success: function (response) {
                        _me.renderFilter(response);
                        if (response !== null && response.toString().length > 1) {
                            _me.getReportData();
                        } else {
                            $("#noInfoRow").show();
                            Site.unBlockUI();
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    }
                };
                _me.stopAjaxRequest(ajaxFilterRequest);
                _me.stopAjaxRequest(ajaxRequest);
                ajaxFilterRequest = $.ajax(option);
            },

            getReportData: function () {
                var url = Site.resolveClientUrl(DATA_API);
                var option = {
                    type: "POST",
                    cache: false,
                    data: param,
                    url: url,
                    success: function (response) {
                        if (response !== null && response.length > 0) {
                            $("#noInfoRow").hide();
                            $("#resultContent").html(response);

                            if ($(".fn-rs-content-scroll").length > 0) {
                                GridHelper.resizeGrid($(".fn-rs-content-scroll"));
                            }
                        } else {
                            $("#noInfoRow").show();
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                _me.stopAjaxRequest(ajaxRequest);
                ajaxRequest = $.ajax(option);
            },

            renderFilter: function (data) {
                if (data) {
                    _me.renderSportFilter(data.Sports);
                    _me.renderLeagueFilter(data.Leagues);
                    Site.initSelectElements();

                    $("#sportAndLeagueFilter").css("visibility", "visible");
                    $("#noInfoRow").hide();
                }
                else {
                    $("#sportAndLeagueFilter").css("visibility", "hidden");
                }
            },

            renderSportFilter: function (sportList) {
                if (param.SportId === 0 && sportList.length > 0) {
                    var firstSport = sportList[0].SportId;
                    param.SportId = firstSport;

                    var $sport = $("#ddlSport");
                    $.each(sportList, function (index, value) {
                        $sport.append('<option value="' + value.SportId + '">' + value.SportName + '</option>');
                    });
                }
            },

            renderLeagueFilter: function (leagueList) {
                if (leagueList !== null && leagueList.length > 0) {
                    var $league = $("#ddlLeague");
                    $.each(leagueList, function (index, value) {
                        $league.append('<option value="' + value.LeagueId + '">' + value.LeagueName + '</option>');
                    });
                    $league.val(param.LeagueId);
                }
            },

            clearResult: function () {
                _me.clearLeagueFilter();
                $("#ddlSport option").remove();
                $("#resultContent").html("");
                $("#sportAndLeagueFilter").css("visibility", "hidden");
            },

            clearLeagueFilter: function () {
                var defaultLeague = $('#ddlLeague option[value="0"]').clone();
                $('#ddlLeague option').remove();
                $('#ddlLeague').append(defaultLeague);
            },

            clearExcelExport: function () {
                param.IsExportExcel = false;
                $('#iframeForExportExcel').attr('src', "");
            },

            showOutRight: function (isShow) {
                if (isShow) {
                    $("#singleDate").hide();
                    $("#dateRange").show();
                    $("#submitOutright").addClass(ACTIVE_CLASS);
                    $("#submitNormal").removeClass(ACTIVE_CLASS);
                } else {
                    $("#dateRange").hide();
                    $("#singleDate").show();
                    $("#submitOutright").removeClass(ACTIVE_CLASS);
                    $("#submitNormal").addClass(ACTIVE_CLASS);
                }
                $("#submitOutright").attr("data-isOutright", isShow ? "1" : "0");
            },

            setDateParam: function (isShowOutright) {
                if (isShowOutright) {
                    param.FromDate = $("#FromDate").val();
                    param.ToDate = $("#ToDate").val();
                }
                else {
                    param.FromDate = $("#Date").val();
                    param.ToDate = $("#Date").val();
                }
            },

            updateDateInput: function (isShowOutright) {
                if (isShowOutright) {
                    DatePicker.setValueForDateRange(param.FromDate, param.ToDate);
                }
                else {
                    DatePicker.setValueForSingleDate($("#Date"), param.FromDate);
                }
            },

            canSubmit: function (element) {
                return !$(element).hasClass(ACTIVE_CLASS);
            },

            stopAjaxRequest: function (request) {
                if (request !== null) {
                    request.abort();
                }
            }
        };

        return _me;
    }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 29:
/*!****************************************************************!*\
  !*** multi ./www/js/source/pages/report/result/main-result.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! ./www/js/source/pages/report/result/main-result.js */"./www/js/source/pages/report/result/main-result.js");


/***/ })

/******/ });
//# sourceMappingURL=result.chunk.js.map