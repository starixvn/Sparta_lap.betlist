webpackJsonp([78],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(365);


/***/ },

/***/ 228:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/// <reference path="/@JSense.js" />
	"use strict";
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
	
	    $.fn.getStyleObject = function () {
	        /// <summary>
	        /// don't work on ie8
	        /// </summary>
	        /// <returns type=""></returns>
	        var dom = this.get(0);
	        if(!dom)
	        {
	            return;
	        }
	
	        var style;
	        var returns = {};
	
	        if (window.getComputedStyle) {
	            var camelize = function (a, b) {
	                return b.toUpperCase();
	            };
	            style = window.getComputedStyle(dom, null);
	            for (var i = 0, l = style.length; i < l; i++) {
	                var prop = style[i];
	                var camel = prop.replace(/\-([a-z])/, camelize);
	                var val = style.getPropertyValue(prop);
	                returns[camel] = val;
	            }
	            return returns;
	        }
	        try {
	            if (dom.currentStyle) {
	                style = dom.currentStyle;
	                for (var p1 in style) {
	                    returns[p1] = style[p1];
	                }
	                return returns;
	            }
	            if (style === dom.style) {
	                for (var p2 in style) {
	                    if (typeof style[p2] !== 'function') {
	                        returns[p2] = style[p2];
	                    }
	                }
	                return returns;
	            }
	        } catch (err) {
	        }
	        return returns;
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
	                var value = textBox.value;
	                value = value.replace(/,/g, '');
	
	                var isNotInRange = (value < min) || (value > max);
	                if (isNotInRange) {
	                    textBox.value = textBox.value.slice(0, -1);
	                    value = textBox.value.replace(/,/g, '');
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
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 266:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(267)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString) {
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
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 267:
/***/ function(module, exports, __webpack_require__) {

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
	        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (factory), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	    } else if (typeof exports === 'object') {
	        // Node/CommonJS
	        module.exports = factory(require('jquery'));
	    } else {
	        // Browser globals
	        factory(jQuery);
	    }
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


/***/ },

/***/ 364:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(266), __webpack_require__(228)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, RefreshCountDown) {
	        "use strict";
	
	        var _me = {
	            resizeGrid: function () {
	                var $grid = $(".body-content");
	                if ($grid.length === 0) {
	                    return;
	                }
	
	                var gridTop = $grid.offset().top;
	                var footerSite = 18;
	
	                var gridHeight = window.innerHeight - gridTop - $(".footer-content").height() - footerSite - 10;
	                gridHeight = Math.max(150, gridHeight);
	                $grid.css("max-height", gridHeight);
	
	                if (!$grid.hasVerticalScrollBar()) {
	                    $grid.addClass("hasScroll");
	                }
	                else {
	                    $grid.find("table").width($grid.width() - 17);
	                }
	            },
	            initEvent: function () {
	                RefreshCountDown.initialize();
	
	                $("#reportContent").on("click", '.forcast-number', function () {
	                    var matchId = $(this).attr("data-match-id");
	                    var betType = $(this).attr("data-bet-type");
	                    _me.viewRunningBetListMatch(matchId, betType);
	                });
	
	                $(document).ajaxComplete(function () {
	                    _me.resizeGrid();
	                });
	
	                $(window).on("resize", function () {
	                    _me.resizeGrid();
	                });
	
	                $(window).on("unload", function () {
	                    $(window).off("resize");
	                });
	            },
	            viewRunningBetListMatch: function (matchId, betType) {
	                var url = Site.resolveClientUrl("RunningMatchBetList/Index");
	                url = Site.setLicenseeOptionOriginalParam(url);
	                url = QueryString.setParam(url, "matchid", matchId);
	                url = QueryString.setParam(url, "bettype", betType);
	                url = QueryString.setParam(url, "isShowSumStake", true);
	
	                Site.redirect(url);
	            },
	            viewRunningBetListMultiBettype: function (matchId, betType) {
	                var url = Site.resolveClientUrl("RunningBettypesOfMatchBetList/Index");
	                url = Site.setLicenseeOptionOriginalParam(url);
	                url = QueryString.setParam(url, "matchid", matchId);
	                url = QueryString.setParam(url, "multiplebettype", betType);
	                url = QueryString.setParam(url, "isShowSumStake", true);
	
	                Site.redirect(url);
	            },
	
	            initialize: function () {
	                _me.initEvent();
	                _me.resizeGrid();
	                Site.unBlockUI();
	            }
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 365:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(366)], __WEBPACK_AMD_DEFINE_RESULT__ = function (HalfTimeFullTime) {
	    "use strict";
	
	    $(HalfTimeFullTime.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 366:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(364), __webpack_require__(266)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Totalbets, RefreshCountDown) {
	    "use strict";
	
	    var _me = {
	        initialize: function () {
	            Site.initialize();
	            Totalbets.initialize();
	            _me.initEvent();
	            Site.addUserGuide({
	                resourceName: "TotalBetsHTFT",
	                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("HTFT")
	            });
	
	            RefreshCountDown.initEvent();
	        },
	        initEvent: function () {
	            $("#SubmitButton").on("click", function () {
	                _me.submit();
	            });
	
	            $('#reportContent').on('click', '.team', function () {
	                var matchId = $(this).attr("data-match-id");
	                Totalbets.viewRunningBetListMatch(matchId, 16);
	            });
	        },
	        submit: function () {
	            Site.blockUI();
	            $("#HalfTimeFullTimeForm").submit();
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }

});
//# sourceMappingURL=halftimefulltime.chunk.js.map