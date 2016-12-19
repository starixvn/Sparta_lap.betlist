webpackJsonp([31],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(278);


/***/ },

/***/ 209:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(210)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString) {
	    var curCountDown = 0;
	    var countDownTimer = null;
	    var cookieName = null;
	    var countDownId = null;
	    var defaultCountDown = 0;
	
	    var _me = {
	        isForecastAH: false,
	        initialize: function () {
	            // Select countdown
	            $('.menu-item').on('click', function () {
	                var selectedCountDown = parseInt($(this).attr('data-count-down'));
	                _me.setCountDown(selectedCountDown, true);
	            });
	
	            // Refresh button click
	            $('.button-refresh').on('click', function () {
	                _me.refresh();
	            });
	
	            if (_me.isForecastAH) {
	                $('.dropdown-button').dropdown();
	            }
	
	            // Get count down id from filter
	            countDownId = $('#count-down-of').attr('name');
	
	            // Get default count down
	            defaultCountDown = parseInt($('#count-down-of').attr('data-default-value'));
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
	
	            // Set up ajax no cache
	            $.ajaxSetup({ cache: false });
	
	            $('#reportContent').load(url, {
	                noncache: new Date().getTime()
	            }, function () {
	                Site.unBlockUI();
	                _me.initEvent();
	
	                // Currency constraint
	                Site.currencyConstraint();
	
	                // Re init countdown event with forecast AH
	                if (_me.isForecastAH) {
	                    _me.initialize();
	                }
	
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
	
	            var url = location.href;
	            url = Site.setLicenseeOptionOriginalParam(url);
	            url = QueryString.setParam(url, "LeagueId", leagueId);
	            url = QueryString.setParam(url, "MatchId", matchId);
	            url = QueryString.setParam(url, "BetType", betType);
	            url = QueryString.setParam(url, "CurrentScore", currentScore);
	            url = QueryString.setParam(url, "IsShowFt", isShowFt);
	            url = QueryString.setParam(url, "IsShowFh", isShowFh);
	
	            url = QueryString.setParam(url, "HomeTeamId", homeName);
	            url = QueryString.setParam(url, "AwayTeamId", awayName);
	
	            return url;
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 210:
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

/***/ 271:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(209)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, RefreshCountDown) {
	    var _me = {
	        resizeGrid: function () {
	            var $grid = $(".body-content");
	            if ($grid.length === 0) {
	                return;
	            }
	
	            var gridTop = $grid.offset().top;
	
	            var gridHeight = window.innerHeight - gridTop - $(".footer-content").height() - 20;
	            gridHeight = Math.max(150, gridHeight);
	            $grid.css("max-height", gridHeight);
	        },
	        initEvent: function () {
	            // Init refresh countdown
	            RefreshCountDown.initialize();
	
	            // Forcast number click
	            $("#reportContent").on("click", '.forcast-number', function () {
	                var matchId = $(this).attr("data-match-id");
	                var betType = $(this).attr("data-bet-type");
	                _me.viewRunningBetListMatch(matchId, betType);
	            });
	
	            $(document).ajaxComplete(function () {
	                _me.resizeGrid();
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
	            Site.unBlockUI();
	            this.initEvent();
	            _me.resizeGrid();
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 278:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(279)], __WEBPACK_AMD_DEFINE_RESULT__ = function (MoneyLine) {
	    "use strict";
	
	    $(MoneyLine.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__))
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 279:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(271), __webpack_require__(209)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Totalbets, RefreshCountDown) {
	    "use strict";
	
	    var _me = {
	        initialize: function () {
	            Site.initialize();
	            Totalbets.initialize();
	            _me.initEvent();
	            Site.addUserGuide({
	                resourceName: "TotalBetsMoneyLine",
	                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("MoneyLine")
	            });
	
	            RefreshCountDown.initEvent();
	        },
	        initEvent: function () {
	            $("#SubmitButton").on("click", function () {
	                _me.submit();
	            });
	        },
	        submit: function () {
	            Site.blockUI();
	            $("#MoneyLineForm").submit();
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }

});
//# sourceMappingURL=moneyline.chunk.js.map