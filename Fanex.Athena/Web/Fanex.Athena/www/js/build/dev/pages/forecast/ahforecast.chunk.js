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
/******/ 		"www/js/build/dev/pages/forecast/ahforecast": 0
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
/******/ 	deferredModules.push([11,"www/js/build/dev/pages/lib/common"]);
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

/***/ "./www/js/source/components/refreshcountdown.js":
/*!******************************************************!*\
  !*** ./www/js/source/components/refreshcountdown.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! common/querystring */ "./www/js/source/common/querystring.js"), __webpack_require__(/*! lib/jquery.cookie */ "./www/js/source/lib/jquery.cookie.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, QueryString) {
    "use strict";

    var curCountDown = 0;
    var countDownTimer = null;
    var cookieName = null;
    var countDownId = null;
    var defaultCountDown = 0;

    var _me = {
        initialize: function () {
            $('.menu-item').on('click', function () {
                var selectedCountDown = parseInt($(this).attr('data-count-down'));
                _me.setCountDown(selectedCountDown, true);
            });

            $('.button-refresh').on('click', function () {
                _me.refresh();
            });

            var $hiddenCountdown = $('#count-down-of');
            countDownId = $hiddenCountdown.attr('name');

            // Get default count down
            defaultCountDown = parseInt($hiddenCountdown.attr('data-default-value'));
        },
        initEvent: function () {
            cookieName = "RefeshCountDown" + countDownId;
            //read countdown from cookie
            var countDownFromCookie = $.cookie(cookieName);
            if (countDownFromCookie && Number(countDownFromCookie) !== 0) {
                _me.setCountDown(countDownFromCookie, false);
            } else {
                _me.setCountDown(defaultCountDown, true);
            }
        },
        setCountDown: function (countDown, isSetToCookie) {
            var countDownInt = parseInt(countDown);
            if (isSetToCookie) {
                $.cookie(cookieName, countDownInt);
            }
            curCountDown = countDownInt;
            _me.setMenuStatus();
            _me.tick();
        },
        setMenuStatus: function () {
            var index = curCountDown;
            $('#countdown-menu .menu-item').removeClass('menu-selected');
            $('#option-' + index).addClass('menu-selected');
        },
        tick: function () {
            document.getElementById('count').innerHTML = curCountDown;
            curCountDown--;
            if (countDownTimer !== null) {
                clearTimeout(countDownTimer);
            }
            countDownTimer = setTimeout(_me.tick, 1000);
            if (curCountDown < -1) {
                _me.refresh();
            }
        },
        refresh: function () {
            var oldScrollTop;
            if ($(".body-content").length) {
                oldScrollTop = $(".body-content").scrollTop();
            }

            if (countDownTimer !== null) {
                clearTimeout(countDownTimer);
            }
            document.getElementById('count').innerHTML = "...";

            // Get param before load data
            var url = _me.calculateUrl();

            Site.blockUI();
            $('#reportContent').load(url, {
                noncache: new Date().getTime()
            }, function () {
                Site.unBlockUI();
                _me.initEvent();
                
                // Currency constraint
                Site.currencyConstraint();

                $(window).resize();

                // Scrollbar still kept current position
                if ($(".body-content").length) {
                    $(".body-content").scrollTop(oldScrollTop);
                }
            });

            return true;
        },
        calculateUrl: function () {
            var leagueId = $('#LeagueId').val();
            var matchId = $('#MatchId').val();
            var betType = $("#BetType").val();
            var currentScore = $("#CurrentScore").val();
            var isShowFt = $("#IsShowFt").val();
            var isShowFh = $("#IsShowFh").val();
            var homeName = $("#HomeTeamId").val();
            var awayName = $("#AwayTeamId").val();

            var url = Site.getCurrentUrl();
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "LeagueId", leagueId);
            url = QueryString.setParam(url, "MatchId", matchId);
            url = QueryString.setParam(url, "BetType", betType);
            url = QueryString.setParam(url, "CurrentScore", currentScore);
            url = QueryString.setParam(url, "IsShowFt", isShowFt);
            url = QueryString.setParam(url, "IsShowFh", isShowFh);

            url = QueryString.setParam(url, "HomeTeamId", homeName);
            url = QueryString.setParam(url, "AwayTeamId", awayName);
            
            return encodeURI(url);
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

/***/ "./www/js/source/lib/jquery.cookie.js":
/*!********************************************!*\
  !*** ./www/js/source/lib/jquery.cookie.js ***!
  \********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/*!
* jQuery Cookie Plugin v1.4.1
* https://github.com/carhartl/jquery-cookie
*
* Copyright 2006, 2014 Klaus Hartl
* Released under the MIT license
*/
(function (factory) {
    if (true) {
        // AMD (Register as an anonymous module)
        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")], __WEBPACK_AMD_DEFINE_FACTORY__ = (factory),
				__WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ?
				(__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
    } else {}
} (function ($) {

    var pluses = /\+/g;

    function encode(s) {
        return config.raw ? s : encodeURIComponent(s);
    }

    function decode(s) {
        return config.raw ? s : decodeURIComponent(s);
    }

    function stringifyCookieValue(value) {
        return encode(config.json ? JSON.stringify(value) : String(value));
    }

    function parseCookieValue(s) {
        if (s.indexOf('"') === 0) {
            // This is a quoted cookie as according to RFC2068, unescape...
            s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
        }

        try {
            // Replace server-side written pluses with spaces.
            // If we can't decode the cookie, ignore it, it's unusable.
            // If we can't parse the cookie, ignore it, it's unusable.
            s = decodeURIComponent(s.replace(pluses, ' '));
            return config.json ? JSON.parse(s) : s;
        } catch (e) { }
    }

    function read(s, converter) {
        var value = config.raw ? s : parseCookieValue(s);
        return $.isFunction(converter) ? converter(value) : value;
    }

    var config = $.cookie = function (key, value, options) {

        // Write

        if (arguments.length > 1 && !$.isFunction(value)) {
            options = $.extend({}, config.defaults, options);

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setMilliseconds(t.getMilliseconds() + days * 864e+5);
            }

            return (document.cookie = [
				encode(key), '=', stringifyCookieValue(value),
				options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
				options.path ? '; path=' + options.path : '',
				options.domain ? '; domain=' + options.domain : '',
				options.secure ? '; secure' : ''
			].join(''));
        }

        // Read

        var result = key ? undefined : {},
        // To prevent the for loop in the first place assign an empty array
        // in case there are no cookies at all. Also prevents odd result when
        // calling $.cookie().
			cookies = document.cookie ? document.cookie.split('; ') : [],
			i = 0,
			l = cookies.length;

        for (; i < l; i++) {
            var parts = cookies[i].split('='),
				name = decode(parts.shift()),
				cookie = parts.join('=');

            if (key === name) {
                // If second argument (value) is a function it's a converter...
                result = read(cookie, value);
                break;
            }

            // Prevent storing a cookie that we couldn't decode.
            if (!key && (cookie = read(cookie)) !== undefined) {
                result[name] = cookie;
            }
        }

        return result;
    };

    config.defaults = {};

    $.removeCookie = function (key, options) {
        // Must not alter options, thus extending a fresh object...
        $.cookie(key, '', $.extend({}, options, { expires: -1 }));
        return !$.cookie(key);
    };

}));


/***/ }),

/***/ "./www/js/source/pages/forecast/forecast.ah.js":
/*!*****************************************************!*\
  !*** ./www/js/source/pages/forecast/forecast.ah.js ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! common/querystring */ "./www/js/source/common/querystring.js"), __webpack_require__(/*! common/popup */ "./www/js/source/common/popup.js"), __webpack_require__(/*! components/refreshcountdown */ "./www/js/source/components/refreshcountdown.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, QueryString, Popup, RefreshCountDown) {
        "use strict";

        var _matchid, _baseCurrency, _currencyid, _currencyName;

        var _me = {
            initialize: function () {
                Site.initialize();
                _me.resizePopup();

                _matchid = $('#hidden-data').attr('data-match-id');
                _baseCurrency = $('#hidden-data').attr('data-basecurrency');
                _currencyid = $('#hidden-data').attr('data-currencyid');
                _currencyName = $('#hidden-data').attr('data-currencyname');

                _me.initEvent();

                RefreshCountDown.initialize();
                RefreshCountDown.initEvent();
            },
            initEvent: function () {
                $('#reportContent').on('click', '.forecast-link', function () {
                    var bettype = parseInt($(this).attr('data-bettype'));
                    var selectedScore = parseInt($(this).attr('data-current-score'));
                    _me.loadForecast(selectedScore, bettype);
                });

                $('#reportContent').on('click', '.view-forecast-link', function () {
                    var bettype = parseInt($(this).attr('data-bettype'));
                    _me.loadForecast(null, bettype);
                });

                $('#reportContent').on('click', '.view-betlist', function () {
                    var bettype = parseInt($(this).attr('data-bettype'));
                    _me.viewBetlist(bettype);
                });

                $('#reportContent').on('keyup', '#txtCurrentscore', function (evt) {
                    if (!evt) {
                        evt = window.event;
                    }

                    if (evt.keyCode === 13) {
                        $("#btnCurrentscore").click();
                    }
                });

                $('#reportContent').on("click", '#btnCurrentscore', function (event) {
                    event.preventDefault();
                    _me.setCurrentScore();
                });
            },

            loadForecast: function (selectedScore, bettype) {
                var url = Site.resolveClientUrl("AhForecast/Index?matchid=" + _matchid + '&bettype=' + bettype);
                url = QueryString.setParam(url, "basecurrency", _baseCurrency);
                url = QueryString.setParam(url, "CurrencyId", _currencyid);
                if (selectedScore !== null) {
                    url = QueryString.setParam(url, "CurrentScore", selectedScore);
                }
                $("#BetType").val(bettype);
                $("#CurrentScore").val(selectedScore);
                $.ajax({
                    url: url,
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    success: function (data) {
                        Site.unBlockUI();
                        $('#reportContent').html(data);
                        RefreshCountDown.initialize();
                        RefreshCountDown.initEvent();
                        $("#txtCurrentscore").focus();
                    },
                    type: "POST",
                    async: true,
                    dataType: "html"
                });
            },

            viewBetlist: function (bettype) {
                var url = Site.resolveClientUrl("RunningMatchBetList/Index");
                url = QueryString.setParam(url, "matchid", _matchid);
                url = QueryString.setParam(url, "bettype", bettype);
                url = QueryString.setParam(url, "basecurrency", _baseCurrency);
                url = QueryString.setParam(url, "currencyid", _currencyid);
                url = QueryString.setParam(url, "currencyname", _currencyName);

                var popH = Math.min(parent.innerHeight - 100, 700), popW = 960;

                Popup.updateCurrentPopup(url, { width: popW, height: popH, title: Site.getSingleResource("BetList") });
                window.logOpenBetlistEvent && window.logOpenBetlistEvent();
            },

            setCurrentScore: function () {
                var data = $("#txtCurrentscore").val();
                if (data === null || data === undefined || !Site.isInteger(data)) {
                    return;
                }
                var bettype = $("#reportContent a.view-betlist").attr("data-bettype");
                _me.loadForecast(data, bettype);
            },

            resizePopup: function () {
                var popH = 250, popW = 700;
                Popup.updateCurrentPopup(null, { width: popW, height: popH, title: Site.getSingleResource("Forecast") });
            }
        };

        return _me;
    }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/pages/forecast/forecast.ah.main.js":
/*!**********************************************************!*\
  !*** ./www/js/source/pages/forecast/forecast.ah.main.js ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! ./forecast.ah */ "./www/js/source/pages/forecast/forecast.ah.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function (Forecast) {
    "use strict";

    $(Forecast.initialize);
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js?fe57")))

/***/ }),

/***/ 11:
/*!****************************************************************!*\
  !*** multi ./www/js/source/pages/forecast/forecast.ah.main.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! ./www/js/source/pages/forecast/forecast.ah.main.js */"./www/js/source/pages/forecast/forecast.ah.main.js");


/***/ })

/******/ });
//# sourceMappingURL=ahforecast.chunk.js.map