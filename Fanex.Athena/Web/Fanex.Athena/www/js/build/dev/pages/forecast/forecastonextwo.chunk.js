webpackJsonp([24],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(215);


/***/ },

/***/ 213:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(214)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString) {
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
	
	            Site.blockUI();
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

/***/ 214:
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

/***/ 215:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(216)], __WEBPACK_AMD_DEFINE_RESULT__ = function (ForecastOneXTwo) {
	    "use strict";
	
	    $(ForecastOneXTwo.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 216:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(8), __webpack_require__(213), __webpack_require__(9)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, QueryString, RefreshCountDown, Popup) {
		"use strict";
	
		var isPopup;
	
		var _me = {
			resizeGrid: function () {
				var $grid = $(".body-content");
				if ($grid.length === 0) {
					return;
				}
	
				var gridTop = $grid.offset().top;
				var gridHeight = window.innerHeight - gridTop - 20;
				gridHeight = Math.max(150, gridHeight);
				$grid.css("max-height", gridHeight);
			},
			initialize: function () {
			    Site.initialize();
	
			    isPopup = $('#IsPopUp').val() === "true";
			    if (isPopup) {
			        $("body").css("background", "#fff");
			        _me.resizePopup();
			    }
	
				_me.initEvent();
				_me.resizeGrid();
				Site.addUserGuide({
					resourceName: "Forecast1x2",
					title: Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("OneXTwo")
				});
	
				RefreshCountDown.initialize();
				RefreshCountDown.initEvent();
	
				Site.unBlockUI();
			},
			initEvent: function () {
				$('.league-list').on('change', function () {
					var leagueId = $(this).val();
					_me.loadSelectedLeague(leagueId);
				});
	
				$('#reportContent').on('click', '.button-add', function () {
					var matchId = $(this).attr('data-match-id');
					var betType = parseInt($(this).attr('data-bettype'));
					_me.openForecast(matchId, betType);
				});
	
				$('#SubmitButton').on('click', function () {
					_me.submit();
				});
			},
			openForecast: function (matchId, betType) {
				var url = Site.resolveClientUrl("RunningMatchBetList/Index");
				url = Site.setLicenseeOptionOriginalParam(url);
				url = QueryString.setParam(url, "matchid", matchId);
				url = QueryString.setParam(url, "bettype", betType);
	
				if (isPopup) {
					var popH = Math.min(parent.innerHeight - 100, 650), popW = 900;
					Popup.updateCurrentPopup(url, { width: popW, height: popH, title: Site.getSingleResource("BetList") });
				} else {
					Site.redirect(url);
				}
			},
			loadSelectedLeague: function (leagueId) {
				var url = Site.resolveClientUrl("OneXTwoForecast/Index");
				url = Site.setLicenseeOptionOriginalParam(url);
				url = QueryString.setParam(url, "LeagueId", leagueId);
				_me.submit();
			},
			submit: function () {
				Site.blockUI();
				$("#ForecastOneXTwoForm").submit();
			},
			resizePopup: function () {
			    var popH = Math.min(parent.innerHeight - 100, 600), popW = 700;
			    var title = Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("OneXTwo");
			    Popup.updateCurrentPopup(null, { width: popW, height: popH, title: title });
			}
		};
	
		return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }

});
//# sourceMappingURL=forecastonextwo.chunk.js.map