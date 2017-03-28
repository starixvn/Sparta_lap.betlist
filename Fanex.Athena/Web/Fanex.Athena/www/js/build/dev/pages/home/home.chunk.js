webpackJsonp([37],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(207);
	__webpack_require__(208);
	__webpack_require__(209);
	module.exports = __webpack_require__(211);


/***/ },

/***/ 199:
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

/***/ 207:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {'use strict';
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	
	    var methods = {
	        init: function init(options) {
	            var defaults = {
	                menuWidth: 260,
	                edge: 'left',
	                closeOnClick: false
	            };
	            options = $.extend(defaults, options);
	
	            $(this).each(function () {
	                var $this = $(this);
	                var menu_id = $("#" + $this.attr('data-activates'));
	
	                // Set to width
	                if (options.menuWidth != 260) {
	                    menu_id.css('width', options.menuWidth);
	                }
	
	                // Add Touch Area
	                var dragTarget = $('<div class="drag-target"></div>');
	                $('body').append(dragTarget);
	
	                if (options.edge == 'left') {
	                    menu_id.css('transform', 'translateX(-100%)');
	                    dragTarget.css({ 'left': 0 }); // Add Touch Area
	                } else {
	                    menu_id.addClass('right-aligned') // Change text-alignment to right
	                    .css('transform', 'translateX(100%)');
	                    dragTarget.css({ 'right': 0 }); // Add Touch Area
	                }
	
	                // If fixed sidenav, bring menu out
	                if (menu_id.hasClass('fixed')) {
	                    if (window.innerWidth > 992) {
	                        menu_id.css('transform', 'translateX(0)');
	                    } else {
	                        $('body').removeClass('site-menubar-fold');
	                    }
	                }
	
	                // Window resize to reset on large screens fixed
	                if (menu_id.hasClass('fixed')) {
	                    $(window).resize(function () {
	                        if ($('body').hasClass('site-menubar-fold')) {
	                            $(".site-menu li.has-sub").removeClass("active");
	                            $('.site-menu .menu-item .collapsible-header').removeClass('active');
	                            $(".site-menu .menu-item div.collapsible-body").css('display', 'none');
	                        }
	                        if (window.innerWidth > 992) {
	                            // Close menu if window is resized bigger than 992 and user has fixed sidenav
	                            if ($('#sidenav-overlay').length !== 0 && menuOut) {
	                                removeMenu(true);
	                            } else {
	                                // menu_id.removeAttr('style');
	                                menu_id.css('transform', 'translateX(0%)');
	                                // menu_id.css('width', options.menuWidth);
	                            }
	                        } else if (menuOut === false) {
	
	                            if (options.edge === 'left') {
	                                menu_id.css('transform', 'translateX(-100%)');
	                            } else {
	                                menu_id.css('transform', 'translateX(100%)');
	                            }
	                        }
	                    });
	                }
	
	                // if closeOnClick, then add close event for all a tags in side sideNav
	                if (options.closeOnClick === true) {
	                    menu_id.on("click.itemclick", "a:not(.collapsible-header)", function () {
	                        removeMenu();
	                    });
	                }
	
	                function removeMenu(restoreNav) {
	                    panning = false;
	                    menuOut = false;
	                    // Reenable scrolling
	                    $('body').css({
	                        overflow: '',
	                        width: ''
	                    });
	
	                    $('#sidenav-overlay').velocity({ opacity: 0 }, {
	                        duration: 200,
	                        queue: false, easing: 'easeOutQuad',
	                        complete: function complete() {
	                            $(this).remove();
	                        }
	                    });
	                    if (options.edge === 'left') {
	                        // Reset phantom div
	                        dragTarget.css({ width: '', right: '', left: '0' });
	                        menu_id.velocity({ 'translateX': '-100%' }, {
	                            duration: 200,
	                            queue: false,
	                            easing: 'easeOutCubic',
	                            complete: function complete() {
	                                if (restoreNav === true) {
	                                    // Restore Fixed sidenav
	                                    menu_id.removeAttr('style');
	                                    menu_id.css('width', options.menuWidth);
	                                }
	                            }
	
	                        });
	                    } else {
	                        // Reset phantom div
	                        dragTarget.css({ width: '', right: '0', left: '' });
	                        menu_id.velocity({ 'translateX': '100%' }, {
	                            duration: 200,
	                            queue: false,
	                            easing: 'easeOutCubic',
	                            complete: function complete() {
	                                if (restoreNav === true) {
	                                    // Restore Fixed sidenav
	                                    menu_id.removeAttr('style');
	                                    menu_id.css('width', options.menuWidth);
	                                }
	                            }
	                        });
	                    }
	                }
	
	                // Touch Event
	                var panning = false;
	                var menuOut = false;
	
	                dragTarget.on('click', function () {
	                    if (menuOut) {
	                        removeMenu();
	                    }
	                });
	
	                dragTarget.hammer({
	                    prevent_default: false
	                }).bind('pan', function (e) {
	
	                    if (e.gesture.pointerType == "touch") {
	
	                        var direction = e.gesture.direction;
	                        var x = e.gesture.center.x;
	                        var y = e.gesture.center.y;
	                        var velocityX = e.gesture.velocityX;
	
	                        // Disable Scrolling
	                        var $body = $('body');
	                        var oldWidth = $body.innerWidth();
	                        $body.css('overflow', 'hidden');
	                        $body.width(oldWidth);
	
	                        // If overlay does not exist, create one and if it is clicked, close menu
	                        if ($('#sidenav-overlay').length === 0) {
	                            var overlay = $('<div id="sidenav-overlay"></div>');
	                            overlay.css('opacity', 0).click(function () {
	                                removeMenu();
	                            });
	                            $('body').append(overlay);
	                        }
	
	                        // Keep within boundaries
	                        if (options.edge === 'left') {
	                            if (x > options.menuWidth) {
	                                x = options.menuWidth;
	                            } else if (x < 0) {
	                                x = 0;
	                            }
	                        }
	
	                        if (options.edge === 'left') {
	                            // Left Direction
	                            if (x < options.menuWidth / 2) {
	                                menuOut = false;
	                            }
	                                // Right Direction
	                            else if (x >= options.menuWidth / 2) {
	                                menuOut = true;
	                            }
	                            menu_id.css('transform', 'translateX(' + (x - options.menuWidth) + 'px)');
	                        } else {
	                            // Left Direction
	                            if (x < window.innerWidth - options.menuWidth / 2) {
	                                menuOut = true;
	                            }
	                                // Right Direction
	                            else if (x >= window.innerWidth - options.menuWidth / 2) {
	                                menuOut = false;
	                            }
	                            var rightPos = x - options.menuWidth / 2;
	                            if (rightPos < 0) {
	                                rightPos = 0;
	                            }
	
	                            menu_id.css('transform', 'translateX(' + rightPos + 'px)');
	                        }
	
	                        // Percentage overlay
	                        var overlayPerc;
	                        if (options.edge === 'left') {
	                            overlayPerc = x / options.menuWidth;
	                            $('#sidenav-overlay').velocity({ opacity: overlayPerc }, { duration: 10, queue: false, easing: 'easeOutQuad' });
	                        } else {
	                            overlayPerc = Math.abs((x - window.innerWidth) / options.menuWidth);
	                            $('#sidenav-overlay').velocity({ opacity: overlayPerc }, { duration: 10, queue: false, easing: 'easeOutQuad' });
	                        }
	                    }
	                }).bind('panend', function (e) {
	
	                    if (e.gesture.pointerType == "touch") {
	                        var velocityX = e.gesture.velocityX;
	                        var x = e.gesture.center.x;
	                        var leftPos = x - options.menuWidth;
	                        var rightPos = x - options.menuWidth / 2;
	                        if (leftPos > 0) {
	                            leftPos = 0;
	                        }
	                        if (rightPos < 0) {
	                            rightPos = 0;
	                        }
	                        panning = false;
	
	                        if (options.edge === 'left') {
	                            // If velocityX <= 0.3 then the user is flinging the menu closed so ignore menuOut
	                            if (menuOut && velocityX <= 0.3 || velocityX < -0.5) {
	                                // Return menu to open
	                                if (leftPos !== 0) {
	                                    menu_id.velocity({ 'translateX': [0, leftPos] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                                }
	
	                                $('#sidenav-overlay').velocity({ opacity: 1 }, { duration: 50, queue: false, easing: 'easeOutQuad' });
	                                dragTarget.css({ width: '50%', right: 0, left: '' });
	                                menuOut = true;
	                            } else if (!menuOut || velocityX > 0.3) {
	                                // Enable Scrolling
	                                $('body').css({
	                                    overflow: '',
	                                    width: ''
	                                });
	                                // Slide menu closed
	                                menu_id.velocity({ 'translateX': [-1 * options.menuWidth - 10, leftPos] }, { duration: 200, queue: false, easing: 'easeOutQuad' });
	                                $('#sidenav-overlay').velocity({ opacity: 0 }, {
	                                    duration: 200, queue: false, easing: 'easeOutQuad',
	                                    complete: function complete() {
	                                        $(this).remove();
	                                    }
	                                });
	                                dragTarget.css({ width: '10px', right: '', left: 0 });
	                            }
	                        } else {
	                            if (menuOut && velocityX >= -0.3 || velocityX > 0.5) {
	                                // Return menu to open
	                                if (rightPos !== 0) {
	                                    menu_id.velocity({ 'translateX': [0, rightPos] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                                }
	
	                                $('#sidenav-overlay').velocity({ opacity: 1 }, { duration: 50, queue: false, easing: 'easeOutQuad' });
	                                dragTarget.css({ width: '50%', right: '', left: 0 });
	                                menuOut = true;
	                            } else if (!menuOut || velocityX < -0.3) {
	                                // Enable Scrolling
	                                $('body').css({
	                                    overflow: '',
	                                    width: ''
	                                });
	
	                                // Slide menu closed
	                                menu_id.velocity({ 'translateX': [options.menuWidth + 10, rightPos] }, { duration: 200, queue: false, easing: 'easeOutQuad' });
	                                $('#sidenav-overlay').velocity({ opacity: 0 }, {
	                                    duration: 200, queue: false, easing: 'easeOutQuad',
	                                    complete: function complete() {
	                                        $(this).remove();
	                                    }
	                                });
	                                dragTarget.css({ width: '10px', right: 0, left: '' });
	                            }
	                        }
	                    }
	                });
	
	                $this.click(function () {
	                    var $body = $('body');
	                    var $submenu = $(".site-menu .menu-item div.collapsible-body");
	                    var menuCollapse = $body.hasClass('site-menubar-fold');
	
	                    if (window.innerWidth > 992) {
	                        $(".site-menu li.has-sub").removeClass("active");
	                        $('.site-menu .menu-item .collapsible-header').removeClass('active');
	                        $submenu.css('display', 'none');
	                        menu_id.removeAttr('style');
	                        $body.toggleClass('site-menubar-fold', !menuCollapse);
	                    } else {
	
	                        if (menuOut === true) {
	                            menuOut = false;
	                            panning = false;
	                            removeMenu();
	                        } else {
	                            // Disable Scrolling
	                            var oldWidth = $body.innerWidth();
	                            $body.css('overflow', 'hidden');
	                            $body.width(oldWidth);
	
	                            // Push current drag target on top of DOM tree
	                            $('body').append(dragTarget);
	
	                            if (options.edge === 'left') {
	                                dragTarget.css({ width: '50%', right: 0, left: '' });
	                                menu_id.velocity({ 'translateX': [0, -1 * options.menuWidth] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                            } else {
	                                dragTarget.css({ width: '50%', right: '', left: 0 });
	                                menu_id.velocity({ 'translateX': [0, options.menuWidth] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                            }
	
	                            var overlay = $('<div id="sidenav-overlay"></div>');
	                            overlay.css('opacity', 0).click(function () {
	                                menuOut = false;
	                                panning = false;
	                                removeMenu();
	                                overlay.velocity({ opacity: 0 }, {
	                                    duration: 300, queue: false, easing: 'easeOutQuad',
	                                    complete: function complete() {
	                                        $(this).remove();
	                                    }
	                                });
	                            });
	                            $body.append(overlay);
	                            overlay.velocity({ opacity: 1 }, {
	                                duration: 300, queue: false, easing: 'easeOutQuad',
	                                complete: function complete() {
	                                    menuOut = true;
	                                    panning = false;
	                                }
	                            });
	                        }
	                    }
	
	                    if (menuCollapse) {
	                        $submenu.mCustomScrollbar("destroy");
	                    } else {
	                        $submenu.mCustomScrollbar({ theme: "minimal-dark" });
	                    }
	
	                    return false;
	                });
	            });
	        },
	        show: function show() {
	            this.trigger('click');
	        },
	        hide: function hide() {
	            $('#sidenav-overlay').trigger('click');
	        }
	    };
	
	    $.fn.sideNav2 = function (methodOrOptions) {
	        if (methods[methodOrOptions]) {
	            return methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
	        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
	            // Default to "init"
	            return methods.init.apply(this, arguments);
	        } else {
	            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.sideNav');
	        }
	    }; // Plugin end
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	
	$(document).ready(function () {
	    $(".toggleMenubar").sideNav2();
	});
	
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 208:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {'use strict';
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	    function activeMenuItem($target) {
	        $(this).find('.collapsible-body li.active').removeClass('active');
	        $target.closest('li').addClass('active');
	        var $groupHeader = $target.closest('.menu-item').find('.collapsible-header');
	        if ($groupHeader.hasClass('active') === false) {
	            $groupHeader.trigger("click");
	        }
	    }
	
	    function init (options) {
	        var defaults = {
	            accordion: undefined
	        };
	
	        options = $.extend(defaults, options);
	        var touch = 'ontouchstart' in window || navigator.maxTouchPoints;
	
	        return this.each(function () {
	
	            var $this = $(this);
	
	            if (!$this.parent().hasClass('site-menu')) return;
	
	            var $panel_headers = $(this).find('> li > .collapsible-header');
	
	            var collapsible_type = $this.data("collapsible");
	            var menu = {};
	
	            // Turn off any existing event handlers
	            $this.off('click.collapse2', '> li > .collapsible-header');
	            $panel_headers.off('click.collapse2');
	
	            /****************
	            Helper Functions
	            ****************/
	
	            // Accordion Open
	            function accordionOpen(object) {
	                $panel_headers = $this.find('> li > .collapsible-header');
	                if (object.hasClass('active')) {
	                    object.parent().addClass('active');
	                } else {
	                    object.parent().removeClass('active');
	                }
	                if (object.parent().hasClass('active')) {
	                    object.siblings('.collapsible-body').stop(true, false).slideDown({ duration: 350, easing: "easeOutQuart", queue: false, complete: function complete() {
	                            $(this).css('height', '');
	                        } });
	                } else {
	                    object.siblings('.collapsible-body').stop(true, false).slideUp({ duration: 350, easing: "easeOutQuart", queue: false, complete: function complete() {
	                            $(this).css('height', '');
	                        } });
	                }
	
	                $panel_headers.not(object).removeClass('active').parent().removeClass('active');
	                $panel_headers.not(object).parent().children('.collapsible-body').stop(true, false).slideUp({
	                    duration: 350,
	                    easing: "easeOutQuart",
	                    queue: false,
	                    complete: function complete() {
	                        $(this).css('height', '');
	                    }
	                });
	            }
	
	            // Expandable Open
	            function expandableOpen(object) {
	                if (object.hasClass('active')) {
	                    object.parent().addClass('active');
	                } else {
	                    object.parent().removeClass('active');
	                }
	                if (object.parent().hasClass('active')) {
	                    object.siblings('.collapsible-body').stop(true, false).slideDown({ duration: 350, easing: "easeOutQuart", queue: false, complete: function complete() {
	                            $(this).css('height', '');
	                        } });
	                } else {
	                    object.siblings('.collapsible-body').stop(true, false).slideUp({ duration: 350, easing: "easeOutQuart", queue: false, complete: function complete() {
	                            $(this).css('height', '');
	                        } });
	                }
	            }
	
	            /**
	             * Check if object is children of panel header
	             * @param  {Object}  object Jquery object
	             * @return {Boolean} true if it is children
	             */
	            function isChildrenOfPanelHeader(object) {
	
	                var panelHeader = getPanelHeader(object);
	
	                return panelHeader.length > 0;
	            }
	
	            /**
	             * Get panel header from a children element
	             * @param  {Object} object Jquery object
	             * @return {Object} panel header object
	             */
	            function getPanelHeader(object) {
	
	                return object.closest('li > .collapsible-header');
	            }
	
	            /*****  End Helper Functions  *****/
	
	            function collapseWithOverlay($this, event) {
	
	                collapse($this, event);
	
	                if ($('#sidenav-overlay').length > 0) {
	
	                    if (!$($this).hasClass('active')) {
	                        $('#sidenav-overlay').unbind("click");
	                        $('#sidenav-overlay').remove();
	                    }
	                } else {
	                    var overlay = $('<div id="sidenav-overlay"></div>');
	                    overlay.css('opacity', 0).click(function () {
	                        collapse($this, event);
	                        overlay.velocity({ opacity: 0 }, {
	                            duration: 300, queue: false, easing: 'easeOutQuad',
	                            complete: function complete() {
	                                $(this).remove();
	                            }
	                        });
	                    });
	                    $('body').append(overlay);
	                    overlay.velocity({ opacity: 1 }, {
	                        duration: 300, queue: false, easing: 'easeOutQuad',
	                        complete: function complete() {}
	                    });
	                }
	            };
	
	            function collapse($this, event) {
	                var $header = $($this),
	                    element = $(event.target);
	
	                if (isChildrenOfPanelHeader(element)) {
	                    element = getPanelHeader(element);
	                }
	
	                element.toggleClass('active');
	
	                if (options.accordion || collapsible_type === "accordion" || collapsible_type === undefined) {
	                    // Handle Accordion
	                    accordionOpen(element);
	                } else {
	                    // Handle Expandables
	                    expandableOpen(element);
	
	                    if ($header.hasClass('active')) {
	                        expandableOpen($header);
	                    }
	                }
	            };
	
	            if (!touch) {
	                $this.on('mouseover.opensub', '> li > .collapsible-header', function (e) {
	                    if ($('body').hasClass('site-menubar-fold')) collapse(this, e);
	                });
	            }
	
	            // Add click handler to only direct collapsible header children
	            $this.on('click.collapse2', '> li > .collapsible-header', function (e) {
	                return touch && $('body').hasClass('site-menubar-fold') ? collapseWithOverlay(this, e) : collapse(this, e);
	            });
	
	            if (!touch) {
	                // Add focusout event
	                $this.on('mouseleave.closesub', '> li > .collapsible-body', function (e) {
	                    var $header = $(this),
	                        element = $(e.target);
	                    var temp = element.closest('li > .collapsible-body');
	
	                    var element = temp.parent().find('.collapsible-header');
	
	                    if ($('body').hasClass('site-menubar-fold') && element.hasClass('active')) {
	
	                        element.toggleClass('active');
	
	                        if (options.accordion || collapsible_type === "accordion" || collapsible_type === undefined) {
	                            // Handle Accordion
	                            accordionOpen(element);
	                        } else {
	                            // Handle Expandables
	                            expandableOpen(element);
	
	                            if ($header.hasClass('active')) {
	                                expandableOpen($header);
	                            }
	                        }
	                    }
	                });
	            }
	
	            //click menu event
	            $this.on('click.menuevent', '> li > .collapsible-body  a', function (e) {
	                var url = $(this).attr("data-url");
	                $('#mainFrame').prop('src', url);
	
	                var element = $(e.target);
	                if (touch && $('body').hasClass('site-menubar-fold')) {                   
	                    var temp = element.closest('li > .collapsible-body');
	
	                    temp.parent().find('.collapsible-header').trigger("click");
	                }
	                activeMenuItem.call($this, element);
	            });
	
	            // Open first active
	            var $panel_headers = $this.find('> li > .collapsible-header');
	            if (options.accordion || collapsible_type === "accordion" || collapsible_type === undefined) {
	                // Handle Accordion
	                accordionOpen($panel_headers.filter('.active').first());
	            } else {
	                // Handle Expandables
	                $panel_headers.filter('.active').each(function () {
	                    expandableOpen($(this));
	                });
	            }
	        });
	    };
	
	    var methods = {
	        init: init,
	        activeMenuItem: activeMenuItem
	    };
	
	    $.fn.collapsibleMenu = function (methodOrOptions) {
	        if (methods[methodOrOptions]) {
	            return methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
	        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
	            // Default to "init"
	            return methods.init.apply(this, arguments);
	        } else {
	            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.collapsibleMenu');
	        }
	    }
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	
	$(document).ready(function () {
	    $('.collapsible2').collapsibleMenu();
	});
	
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 209:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {/* == jquery mousewheel plugin == Version: 3.1.13, License: MIT License (MIT) */
	!function(a){ true?!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (a), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__)):"object"==typeof exports?module.exports=a:a(jQuery)}(function(a){function b(b){var g=b||window.event,h=i.call(arguments,1),j=0,l=0,m=0,n=0,o=0,p=0;if(b=a.event.fix(g),b.type="mousewheel","detail"in g&&(m=-1*g.detail),"wheelDelta"in g&&(m=g.wheelDelta),"wheelDeltaY"in g&&(m=g.wheelDeltaY),"wheelDeltaX"in g&&(l=-1*g.wheelDeltaX),"axis"in g&&g.axis===g.HORIZONTAL_AXIS&&(l=-1*m,m=0),j=0===m?l:m,"deltaY"in g&&(m=-1*g.deltaY,j=m),"deltaX"in g&&(l=g.deltaX,0===m&&(j=-1*l)),0!==m||0!==l){if(1===g.deltaMode){var q=a.data(this,"mousewheel-line-height");j*=q,m*=q,l*=q}else if(2===g.deltaMode){var r=a.data(this,"mousewheel-page-height");j*=r,m*=r,l*=r}if(n=Math.max(Math.abs(m),Math.abs(l)),(!f||f>n)&&(f=n,d(g,n)&&(f/=40)),d(g,n)&&(j/=40,l/=40,m/=40),j=Math[j>=1?"floor":"ceil"](j/f),l=Math[l>=1?"floor":"ceil"](l/f),m=Math[m>=1?"floor":"ceil"](m/f),k.settings.normalizeOffset&&this.getBoundingClientRect){var s=this.getBoundingClientRect();o=b.clientX-s.left,p=b.clientY-s.top}return b.deltaX=l,b.deltaY=m,b.deltaFactor=f,b.offsetX=o,b.offsetY=p,b.deltaMode=0,h.unshift(b,j,l,m),e&&clearTimeout(e),e=setTimeout(c,200),(a.event.dispatch||a.event.handle).apply(this,h)}}function c(){f=null}function d(a,b){return k.settings.adjustOldDeltas&&"mousewheel"===a.type&&b%120===0}var e,f,g=["wheel","mousewheel","DOMMouseScroll","MozMousePixelScroll"],h="onwheel"in document||document.documentMode>=9?["wheel"]:["mousewheel","DomMouseScroll","MozMousePixelScroll"],i=Array.prototype.slice;if(a.event.fixHooks)for(var j=g.length;j;)a.event.fixHooks[g[--j]]=a.event.mouseHooks;var k=a.event.special.mousewheel={version:"3.1.12",setup:function(){if(this.addEventListener)for(var c=h.length;c;)this.addEventListener(h[--c],b,!1);else this.onmousewheel=b;a.data(this,"mousewheel-line-height",k.getLineHeight(this)),a.data(this,"mousewheel-page-height",k.getPageHeight(this))},teardown:function(){if(this.removeEventListener)for(var c=h.length;c;)this.removeEventListener(h[--c],b,!1);else this.onmousewheel=null;a.removeData(this,"mousewheel-line-height"),a.removeData(this,"mousewheel-page-height")},getLineHeight:function(b){var c=a(b),d=c["offsetParent"in a.fn?"offsetParent":"parent"]();return d.length||(d=a("body")),parseInt(d.css("fontSize"),10)||parseInt(c.css("fontSize"),10)||16},getPageHeight:function(b){return a(b).height()},settings:{adjustOldDeltas:!0,normalizeOffset:!0}};a.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})});!function(a){ true?!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (a), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__)):"object"==typeof exports?module.exports=a:a(jQuery)}(function(a){function b(b){var g=b||window.event,h=i.call(arguments,1),j=0,l=0,m=0,n=0,o=0,p=0;if(b=a.event.fix(g),b.type="mousewheel","detail"in g&&(m=-1*g.detail),"wheelDelta"in g&&(m=g.wheelDelta),"wheelDeltaY"in g&&(m=g.wheelDeltaY),"wheelDeltaX"in g&&(l=-1*g.wheelDeltaX),"axis"in g&&g.axis===g.HORIZONTAL_AXIS&&(l=-1*m,m=0),j=0===m?l:m,"deltaY"in g&&(m=-1*g.deltaY,j=m),"deltaX"in g&&(l=g.deltaX,0===m&&(j=-1*l)),0!==m||0!==l){if(1===g.deltaMode){var q=a.data(this,"mousewheel-line-height");j*=q,m*=q,l*=q}else if(2===g.deltaMode){var r=a.data(this,"mousewheel-page-height");j*=r,m*=r,l*=r}if(n=Math.max(Math.abs(m),Math.abs(l)),(!f||f>n)&&(f=n,d(g,n)&&(f/=40)),d(g,n)&&(j/=40,l/=40,m/=40),j=Math[j>=1?"floor":"ceil"](j/f),l=Math[l>=1?"floor":"ceil"](l/f),m=Math[m>=1?"floor":"ceil"](m/f),k.settings.normalizeOffset&&this.getBoundingClientRect){var s=this.getBoundingClientRect();o=b.clientX-s.left,p=b.clientY-s.top}return b.deltaX=l,b.deltaY=m,b.deltaFactor=f,b.offsetX=o,b.offsetY=p,b.deltaMode=0,h.unshift(b,j,l,m),e&&clearTimeout(e),e=setTimeout(c,200),(a.event.dispatch||a.event.handle).apply(this,h)}}function c(){f=null}function d(a,b){return k.settings.adjustOldDeltas&&"mousewheel"===a.type&&b%120===0}var e,f,g=["wheel","mousewheel","DOMMouseScroll","MozMousePixelScroll"],h="onwheel"in document||document.documentMode>=9?["wheel"]:["mousewheel","DomMouseScroll","MozMousePixelScroll"],i=Array.prototype.slice;if(a.event.fixHooks)for(var j=g.length;j;)a.event.fixHooks[g[--j]]=a.event.mouseHooks;var k=a.event.special.mousewheel={version:"3.1.12",setup:function(){if(this.addEventListener)for(var c=h.length;c;)this.addEventListener(h[--c],b,!1);else this.onmousewheel=b;a.data(this,"mousewheel-line-height",k.getLineHeight(this)),a.data(this,"mousewheel-page-height",k.getPageHeight(this))},teardown:function(){if(this.removeEventListener)for(var c=h.length;c;)this.removeEventListener(h[--c],b,!1);else this.onmousewheel=null;a.removeData(this,"mousewheel-line-height"),a.removeData(this,"mousewheel-page-height")},getLineHeight:function(b){var c=a(b),d=c["offsetParent"in a.fn?"offsetParent":"parent"]();return d.length||(d=a("body")),parseInt(d.css("fontSize"),10)||parseInt(c.css("fontSize"),10)||16},getPageHeight:function(b){return a(b).height()},settings:{adjustOldDeltas:!0,normalizeOffset:!0}};a.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})});
	/* == malihu jquery custom scrollbar plugin == Version: 3.1.5, License: MIT License (MIT) */
	!function(e){ true?!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (e), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__)):"undefined"!=typeof module&&module.exports?module.exports=e:e(jQuery,window,document)}(function(e){!function(t){var o="function"=="function"&&__webpack_require__(5),a="undefined"!=typeof module&&module.exports,n="https:"==document.location.protocol?"https:":"http:",i="cdnjs.cloudflare.com/ajax/libs/jquery-mousewheel/3.1.13/jquery.mousewheel.min.js";o||(a?__webpack_require__(210)(e):e.event.special.mousewheel||e("head").append(decodeURI("%3Cscript src="+n+"//"+i+"%3E%3C/script%3E"))),t()}(function(){var t,o="mCustomScrollbar",a="mCS",n=".mCustomScrollbar",i={setTop:0,setLeft:0,axis:"y",scrollbarPosition:"inside",scrollInertia:950,autoDraggerLength:!0,alwaysShowScrollbar:0,snapOffset:0,mouseWheel:{enable:!0,scrollAmount:"auto",axis:"y",deltaFactor:"auto",disableOver:["select","option","keygen","datalist","textarea"]},scrollButtons:{scrollType:"stepless",scrollAmount:"auto"},keyboard:{enable:!0,scrollType:"stepless",scrollAmount:"auto"},contentTouchScroll:25,documentTouchScroll:!0,advanced:{autoScrollOnFocus:"input,textarea,select,button,datalist,keygen,a[tabindex],area,object,[contenteditable='true']",updateOnContentResize:!0,updateOnImageLoad:"auto",autoUpdateTimeout:60},theme:"light",callbacks:{onTotalScrollOffset:0,onTotalScrollBackOffset:0,alwaysTriggerOffsets:!0}},r=0,l={},s=window.attachEvent&&!window.addEventListener?1:0,c=!1,d=["mCSB_dragger_onDrag","mCSB_scrollTools_onDrag","mCS_img_loaded","mCS_disabled","mCS_destroyed","mCS_no_scrollbar","mCS-autoHide","mCS-dir-rtl","mCS_no_scrollbar_y","mCS_no_scrollbar_x","mCS_y_hidden","mCS_x_hidden","mCSB_draggerContainer","mCSB_buttonUp","mCSB_buttonDown","mCSB_buttonLeft","mCSB_buttonRight"],u={init:function(t){var t=e.extend(!0,{},i,t),o=f.call(this);if(t.live){var s=t.liveSelector||this.selector||n,c=e(s);if("off"===t.live)return void m(s);l[s]=setTimeout(function(){c.mCustomScrollbar(t),"once"===t.live&&c.length&&m(s)},500)}else m(s);return t.setWidth=t.set_width?t.set_width:t.setWidth,t.setHeight=t.set_height?t.set_height:t.setHeight,t.axis=t.horizontalScroll?"x":p(t.axis),t.scrollInertia=t.scrollInertia>0&&t.scrollInertia<17?17:t.scrollInertia,"object"!=typeof t.mouseWheel&&1==t.mouseWheel&&(t.mouseWheel={enable:!0,scrollAmount:"auto",axis:"y",preventDefault:!1,deltaFactor:"auto",normalizeDelta:!1,invert:!1}),t.mouseWheel.scrollAmount=t.mouseWheelPixels?t.mouseWheelPixels:t.mouseWheel.scrollAmount,t.mouseWheel.normalizeDelta=t.advanced.normalizeMouseWheelDelta?t.advanced.normalizeMouseWheelDelta:t.mouseWheel.normalizeDelta,t.scrollButtons.scrollType=g(t.scrollButtons.scrollType),h(t),e(o).each(function(){var o=e(this);if(!o.data(a)){o.data(a,{idx:++r,opt:t,scrollRatio:{y:null,x:null},overflowed:null,contentReset:{y:null,x:null},bindEvents:!1,tweenRunning:!1,sequential:{},langDir:o.css("direction"),cbOffsets:null,trigger:null,poll:{size:{o:0,n:0},img:{o:0,n:0},change:{o:0,n:0}}});var n=o.data(a),i=n.opt,l=o.data("mcs-axis"),s=o.data("mcs-scrollbar-position"),c=o.data("mcs-theme");l&&(i.axis=l),s&&(i.scrollbarPosition=s),c&&(i.theme=c,h(i)),v.call(this),n&&i.callbacks.onCreate&&"function"==typeof i.callbacks.onCreate&&i.callbacks.onCreate.call(this),e("#mCSB_"+n.idx+"_container img:not(."+d[2]+")").addClass(d[2]),u.update.call(null,o)}})},update:function(t,o){var n=t||f.call(this);return e(n).each(function(){var t=e(this);if(t.data(a)){var n=t.data(a),i=n.opt,r=e("#mCSB_"+n.idx+"_container"),l=e("#mCSB_"+n.idx),s=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")];if(!r.length)return;n.tweenRunning&&Q(t),o&&n&&i.callbacks.onBeforeUpdate&&"function"==typeof i.callbacks.onBeforeUpdate&&i.callbacks.onBeforeUpdate.call(this),t.hasClass(d[3])&&t.removeClass(d[3]),t.hasClass(d[4])&&t.removeClass(d[4]),l.css("max-height","none"),l.height()!==t.height()&&l.css("max-height",t.height()),_.call(this),"y"===i.axis||i.advanced.autoExpandHorizontalScroll||r.css("width",x(r)),n.overflowed=y.call(this),M.call(this),i.autoDraggerLength&&S.call(this),b.call(this),T.call(this);var c=[Math.abs(r[0].offsetTop),Math.abs(r[0].offsetLeft)];"x"!==i.axis&&(n.overflowed[0]?s[0].height()>s[0].parent().height()?B.call(this):(G(t,c[0].toString(),{dir:"y",dur:0,overwrite:"none"}),n.contentReset.y=null):(B.call(this),"y"===i.axis?k.call(this):"yx"===i.axis&&n.overflowed[1]&&G(t,c[1].toString(),{dir:"x",dur:0,overwrite:"none"}))),"y"!==i.axis&&(n.overflowed[1]?s[1].width()>s[1].parent().width()?B.call(this):(G(t,c[1].toString(),{dir:"x",dur:0,overwrite:"none"}),n.contentReset.x=null):(B.call(this),"x"===i.axis?k.call(this):"yx"===i.axis&&n.overflowed[0]&&G(t,c[0].toString(),{dir:"y",dur:0,overwrite:"none"}))),o&&n&&(2===o&&i.callbacks.onImageLoad&&"function"==typeof i.callbacks.onImageLoad?i.callbacks.onImageLoad.call(this):3===o&&i.callbacks.onSelectorChange&&"function"==typeof i.callbacks.onSelectorChange?i.callbacks.onSelectorChange.call(this):i.callbacks.onUpdate&&"function"==typeof i.callbacks.onUpdate&&i.callbacks.onUpdate.call(this)),N.call(this)}})},scrollTo:function(t,o){if("undefined"!=typeof t&&null!=t){var n=f.call(this);return e(n).each(function(){var n=e(this);if(n.data(a)){var i=n.data(a),r=i.opt,l={trigger:"external",scrollInertia:r.scrollInertia,scrollEasing:"mcsEaseInOut",moveDragger:!1,timeout:60,callbacks:!0,onStart:!0,onUpdate:!0,onComplete:!0},s=e.extend(!0,{},l,o),c=Y.call(this,t),d=s.scrollInertia>0&&s.scrollInertia<17?17:s.scrollInertia;c[0]=X.call(this,c[0],"y"),c[1]=X.call(this,c[1],"x"),s.moveDragger&&(c[0]*=i.scrollRatio.y,c[1]*=i.scrollRatio.x),s.dur=ne()?0:d,setTimeout(function(){null!==c[0]&&"undefined"!=typeof c[0]&&"x"!==r.axis&&i.overflowed[0]&&(s.dir="y",s.overwrite="all",G(n,c[0].toString(),s)),null!==c[1]&&"undefined"!=typeof c[1]&&"y"!==r.axis&&i.overflowed[1]&&(s.dir="x",s.overwrite="none",G(n,c[1].toString(),s))},s.timeout)}})}},stop:function(){var t=f.call(this);return e(t).each(function(){var t=e(this);t.data(a)&&Q(t)})},disable:function(t){var o=f.call(this);return e(o).each(function(){var o=e(this);if(o.data(a)){o.data(a);N.call(this,"remove"),k.call(this),t&&B.call(this),M.call(this,!0),o.addClass(d[3])}})},destroy:function(){var t=f.call(this);return e(t).each(function(){var n=e(this);if(n.data(a)){var i=n.data(a),r=i.opt,l=e("#mCSB_"+i.idx),s=e("#mCSB_"+i.idx+"_container"),c=e(".mCSB_"+i.idx+"_scrollbar");r.live&&m(r.liveSelector||e(t).selector),N.call(this,"remove"),k.call(this),B.call(this),n.removeData(a),$(this,"mcs"),c.remove(),s.find("img."+d[2]).removeClass(d[2]),l.replaceWith(s.contents()),n.removeClass(o+" _"+a+"_"+i.idx+" "+d[6]+" "+d[7]+" "+d[5]+" "+d[3]).addClass(d[4])}})}},f=function(){return"object"!=typeof e(this)||e(this).length<1?n:this},h=function(t){var o=["rounded","rounded-dark","rounded-dots","rounded-dots-dark"],a=["rounded-dots","rounded-dots-dark","3d","3d-dark","3d-thick","3d-thick-dark","inset","inset-dark","inset-2","inset-2-dark","inset-3","inset-3-dark"],n=["minimal","minimal-dark"],i=["minimal","minimal-dark"],r=["minimal","minimal-dark"];t.autoDraggerLength=e.inArray(t.theme,o)>-1?!1:t.autoDraggerLength,t.autoExpandScrollbar=e.inArray(t.theme,a)>-1?!1:t.autoExpandScrollbar,t.scrollButtons.enable=e.inArray(t.theme,n)>-1?!1:t.scrollButtons.enable,t.autoHideScrollbar=e.inArray(t.theme,i)>-1?!0:t.autoHideScrollbar,t.scrollbarPosition=e.inArray(t.theme,r)>-1?"outside":t.scrollbarPosition},m=function(e){l[e]&&(clearTimeout(l[e]),$(l,e))},p=function(e){return"yx"===e||"xy"===e||"auto"===e?"yx":"x"===e||"horizontal"===e?"x":"y"},g=function(e){return"stepped"===e||"pixels"===e||"step"===e||"click"===e?"stepped":"stepless"},v=function(){var t=e(this),n=t.data(a),i=n.opt,r=i.autoExpandScrollbar?" "+d[1]+"_expand":"",l=["<div id='mCSB_"+n.idx+"_scrollbar_vertical' class='mCSB_scrollTools mCSB_"+n.idx+"_scrollbar mCS-"+i.theme+" mCSB_scrollTools_vertical"+r+"'><div class='"+d[12]+"'><div id='mCSB_"+n.idx+"_dragger_vertical' class='mCSB_dragger' style='position:absolute;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>","<div id='mCSB_"+n.idx+"_scrollbar_horizontal' class='mCSB_scrollTools mCSB_"+n.idx+"_scrollbar mCS-"+i.theme+" mCSB_scrollTools_horizontal"+r+"'><div class='"+d[12]+"'><div id='mCSB_"+n.idx+"_dragger_horizontal' class='mCSB_dragger' style='position:absolute;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>"],s="yx"===i.axis?"mCSB_vertical_horizontal":"x"===i.axis?"mCSB_horizontal":"mCSB_vertical",c="yx"===i.axis?l[0]+l[1]:"x"===i.axis?l[1]:l[0],u="yx"===i.axis?"<div id='mCSB_"+n.idx+"_container_wrapper' class='mCSB_container_wrapper' />":"",f=i.autoHideScrollbar?" "+d[6]:"",h="x"!==i.axis&&"rtl"===n.langDir?" "+d[7]:"";i.setWidth&&t.css("width",i.setWidth),i.setHeight&&t.css("height",i.setHeight),i.setLeft="y"!==i.axis&&"rtl"===n.langDir?"989999px":i.setLeft,t.addClass(o+" _"+a+"_"+n.idx+f+h).wrapInner("<div id='mCSB_"+n.idx+"' class='mCustomScrollBox mCS-"+i.theme+" "+s+"'><div id='mCSB_"+n.idx+"_container' class='mCSB_container' style='position:relative; top:"+i.setTop+"; left:"+i.setLeft+";' dir='"+n.langDir+"' /></div>");var m=e("#mCSB_"+n.idx),p=e("#mCSB_"+n.idx+"_container");"y"===i.axis||i.advanced.autoExpandHorizontalScroll||p.css("width",x(p)),"outside"===i.scrollbarPosition?("static"===t.css("position")&&t.css("position","relative"),t.css("overflow","visible"),m.addClass("mCSB_outside").after(c)):(m.addClass("mCSB_inside").append(c),p.wrap(u)),w.call(this);var g=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")];g[0].css("min-height",g[0].height()),g[1].css("min-width",g[1].width())},x=function(t){var o=[t[0].scrollWidth,Math.max.apply(Math,t.children().map(function(){return e(this).outerWidth(!0)}).get())],a=t.parent().width();return o[0]>a?o[0]:o[1]>a?o[1]:"100%"},_=function(){var t=e(this),o=t.data(a),n=o.opt,i=e("#mCSB_"+o.idx+"_container");if(n.advanced.autoExpandHorizontalScroll&&"y"!==n.axis){i.css({width:"auto","min-width":0,"overflow-x":"scroll"});var r=Math.ceil(i[0].scrollWidth);3===n.advanced.autoExpandHorizontalScroll||2!==n.advanced.autoExpandHorizontalScroll&&r>i.parent().width()?i.css({width:r,"min-width":"100%","overflow-x":"inherit"}):i.css({"overflow-x":"inherit",position:"absolute"}).wrap("<div class='mCSB_h_wrapper' style='position:relative; left:0; width:999999px;' />").css({width:Math.ceil(i[0].getBoundingClientRect().right+.4)-Math.floor(i[0].getBoundingClientRect().left),"min-width":"100%",position:"relative"}).unwrap()}},w=function(){var t=e(this),o=t.data(a),n=o.opt,i=e(".mCSB_"+o.idx+"_scrollbar:first"),r=oe(n.scrollButtons.tabindex)?"tabindex='"+n.scrollButtons.tabindex+"'":"",l=["<a href='#' class='"+d[13]+"' "+r+" />","<a href='#' class='"+d[14]+"' "+r+" />","<a href='#' class='"+d[15]+"' "+r+" />","<a href='#' class='"+d[16]+"' "+r+" />"],s=["x"===n.axis?l[2]:l[0],"x"===n.axis?l[3]:l[1],l[2],l[3]];n.scrollButtons.enable&&i.prepend(s[0]).append(s[1]).next(".mCSB_scrollTools").prepend(s[2]).append(s[3])},S=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")],l=[n.height()/i.outerHeight(!1),n.width()/i.outerWidth(!1)],c=[parseInt(r[0].css("min-height")),Math.round(l[0]*r[0].parent().height()),parseInt(r[1].css("min-width")),Math.round(l[1]*r[1].parent().width())],d=s&&c[1]<c[0]?c[0]:c[1],u=s&&c[3]<c[2]?c[2]:c[3];r[0].css({height:d,"max-height":r[0].parent().height()-10}).find(".mCSB_dragger_bar").css({"line-height":c[0]+"px"}),r[1].css({width:u,"max-width":r[1].parent().width()-10})},b=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")],l=[i.outerHeight(!1)-n.height(),i.outerWidth(!1)-n.width()],s=[l[0]/(r[0].parent().height()-r[0].height()),l[1]/(r[1].parent().width()-r[1].width())];o.scrollRatio={y:s[0],x:s[1]}},C=function(e,t,o){var a=o?d[0]+"_expanded":"",n=e.closest(".mCSB_scrollTools");"active"===t?(e.toggleClass(d[0]+" "+a),n.toggleClass(d[1]),e[0]._draggable=e[0]._draggable?0:1):e[0]._draggable||("hide"===t?(e.removeClass(d[0]),n.removeClass(d[1])):(e.addClass(d[0]),n.addClass(d[1])))},y=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=null==o.overflowed?i.height():i.outerHeight(!1),l=null==o.overflowed?i.width():i.outerWidth(!1),s=i[0].scrollHeight,c=i[0].scrollWidth;return s>r&&(r=s),c>l&&(l=c),[r>n.height(),l>n.width()]},B=function(){var t=e(this),o=t.data(a),n=o.opt,i=e("#mCSB_"+o.idx),r=e("#mCSB_"+o.idx+"_container"),l=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")];if(Q(t),("x"!==n.axis&&!o.overflowed[0]||"y"===n.axis&&o.overflowed[0])&&(l[0].add(r).css("top",0),G(t,"_resetY")),"y"!==n.axis&&!o.overflowed[1]||"x"===n.axis&&o.overflowed[1]){var s=dx=0;"rtl"===o.langDir&&(s=i.width()-r.outerWidth(!1),dx=Math.abs(s/o.scrollRatio.x)),r.css("left",s),l[1].css("left",dx),G(t,"_resetX")}},T=function(){function t(){r=setTimeout(function(){e.event.special.mousewheel?(clearTimeout(r),W.call(o[0])):t()},100)}var o=e(this),n=o.data(a),i=n.opt;if(!n.bindEvents){if(I.call(this),i.contentTouchScroll&&D.call(this),E.call(this),i.mouseWheel.enable){var r;t()}P.call(this),U.call(this),i.advanced.autoScrollOnFocus&&H.call(this),i.scrollButtons.enable&&F.call(this),i.keyboard.enable&&q.call(this),n.bindEvents=!0}},k=function(){var t=e(this),o=t.data(a),n=o.opt,i=a+"_"+o.idx,r=".mCSB_"+o.idx+"_scrollbar",l=e("#mCSB_"+o.idx+",#mCSB_"+o.idx+"_container,#mCSB_"+o.idx+"_container_wrapper,"+r+" ."+d[12]+",#mCSB_"+o.idx+"_dragger_vertical,#mCSB_"+o.idx+"_dragger_horizontal,"+r+">a"),s=e("#mCSB_"+o.idx+"_container");n.advanced.releaseDraggableSelectors&&l.add(e(n.advanced.releaseDraggableSelectors)),n.advanced.extraDraggableSelectors&&l.add(e(n.advanced.extraDraggableSelectors)),o.bindEvents&&(e(document).add(e(!A()||top.document)).unbind("."+i),l.each(function(){e(this).unbind("."+i)}),clearTimeout(t[0]._focusTimeout),$(t[0],"_focusTimeout"),clearTimeout(o.sequential.step),$(o.sequential,"step"),clearTimeout(s[0].onCompleteTimeout),$(s[0],"onCompleteTimeout"),o.bindEvents=!1)},M=function(t){var o=e(this),n=o.data(a),i=n.opt,r=e("#mCSB_"+n.idx+"_container_wrapper"),l=r.length?r:e("#mCSB_"+n.idx+"_container"),s=[e("#mCSB_"+n.idx+"_scrollbar_vertical"),e("#mCSB_"+n.idx+"_scrollbar_horizontal")],c=[s[0].find(".mCSB_dragger"),s[1].find(".mCSB_dragger")];"x"!==i.axis&&(n.overflowed[0]&&!t?(s[0].add(c[0]).add(s[0].children("a")).css("display","block"),l.removeClass(d[8]+" "+d[10])):(i.alwaysShowScrollbar?(2!==i.alwaysShowScrollbar&&c[0].css("display","none"),l.removeClass(d[10])):(s[0].css("display","none"),l.addClass(d[10])),l.addClass(d[8]))),"y"!==i.axis&&(n.overflowed[1]&&!t?(s[1].add(c[1]).add(s[1].children("a")).css("display","block"),l.removeClass(d[9]+" "+d[11])):(i.alwaysShowScrollbar?(2!==i.alwaysShowScrollbar&&c[1].css("display","none"),l.removeClass(d[11])):(s[1].css("display","none"),l.addClass(d[11])),l.addClass(d[9]))),n.overflowed[0]||n.overflowed[1]?o.removeClass(d[5]):o.addClass(d[5])},O=function(t){var o=t.type,a=t.target.ownerDocument!==document&&null!==frameElement?[e(frameElement).offset().top,e(frameElement).offset().left]:null,n=A()&&t.target.ownerDocument!==top.document&&null!==frameElement?[e(t.view.frameElement).offset().top,e(t.view.frameElement).offset().left]:[0,0];switch(o){case"pointerdown":case"MSPointerDown":case"pointermove":case"MSPointerMove":case"pointerup":case"MSPointerUp":return a?[t.originalEvent.pageY-a[0]+n[0],t.originalEvent.pageX-a[1]+n[1],!1]:[t.originalEvent.pageY,t.originalEvent.pageX,!1];case"touchstart":case"touchmove":case"touchend":var i=t.originalEvent.touches[0]||t.originalEvent.changedTouches[0],r=t.originalEvent.touches.length||t.originalEvent.changedTouches.length;return t.target.ownerDocument!==document?[i.screenY,i.screenX,r>1]:[i.pageY,i.pageX,r>1];default:return a?[t.pageY-a[0]+n[0],t.pageX-a[1]+n[1],!1]:[t.pageY,t.pageX,!1]}},I=function(){function t(e,t,a,n){if(h[0].idleTimer=d.scrollInertia<233?250:0,o.attr("id")===f[1])var i="x",s=(o[0].offsetLeft-t+n)*l.scrollRatio.x;else var i="y",s=(o[0].offsetTop-e+a)*l.scrollRatio.y;G(r,s.toString(),{dir:i,drag:!0})}var o,n,i,r=e(this),l=r.data(a),d=l.opt,u=a+"_"+l.idx,f=["mCSB_"+l.idx+"_dragger_vertical","mCSB_"+l.idx+"_dragger_horizontal"],h=e("#mCSB_"+l.idx+"_container"),m=e("#"+f[0]+",#"+f[1]),p=d.advanced.releaseDraggableSelectors?m.add(e(d.advanced.releaseDraggableSelectors)):m,g=d.advanced.extraDraggableSelectors?e(!A()||top.document).add(e(d.advanced.extraDraggableSelectors)):e(!A()||top.document);m.bind("contextmenu."+u,function(e){e.preventDefault()}).bind("mousedown."+u+" touchstart."+u+" pointerdown."+u+" MSPointerDown."+u,function(t){if(t.stopImmediatePropagation(),t.preventDefault(),ee(t)){c=!0,s&&(document.onselectstart=function(){return!1}),L.call(h,!1),Q(r),o=e(this);var a=o.offset(),l=O(t)[0]-a.top,u=O(t)[1]-a.left,f=o.height()+a.top,m=o.width()+a.left;f>l&&l>0&&m>u&&u>0&&(n=l,i=u),C(o,"active",d.autoExpandScrollbar)}}).bind("touchmove."+u,function(e){e.stopImmediatePropagation(),e.preventDefault();var a=o.offset(),r=O(e)[0]-a.top,l=O(e)[1]-a.left;t(n,i,r,l)}),e(document).add(g).bind("mousemove."+u+" pointermove."+u+" MSPointerMove."+u,function(e){if(o){var a=o.offset(),r=O(e)[0]-a.top,l=O(e)[1]-a.left;if(n===r&&i===l)return;t(n,i,r,l)}}).add(p).bind("mouseup."+u+" touchend."+u+" pointerup."+u+" MSPointerUp."+u,function(){o&&(C(o,"active",d.autoExpandScrollbar),o=null),c=!1,s&&(document.onselectstart=null),L.call(h,!0)})},D=function(){function o(e){if(!te(e)||c||O(e)[2])return void(t=0);t=1,b=0,C=0,d=1,y.removeClass("mCS_touch_action");var o=I.offset();u=O(e)[0]-o.top,f=O(e)[1]-o.left,z=[O(e)[0],O(e)[1]]}function n(e){if(te(e)&&!c&&!O(e)[2]&&(T.documentTouchScroll||e.preventDefault(),e.stopImmediatePropagation(),(!C||b)&&d)){g=K();var t=M.offset(),o=O(e)[0]-t.top,a=O(e)[1]-t.left,n="mcsLinearOut";if(E.push(o),W.push(a),z[2]=Math.abs(O(e)[0]-z[0]),z[3]=Math.abs(O(e)[1]-z[1]),B.overflowed[0])var i=D[0].parent().height()-D[0].height(),r=u-o>0&&o-u>-(i*B.scrollRatio.y)&&(2*z[3]<z[2]||"yx"===T.axis);if(B.overflowed[1])var l=D[1].parent().width()-D[1].width(),h=f-a>0&&a-f>-(l*B.scrollRatio.x)&&(2*z[2]<z[3]||"yx"===T.axis);r||h?(U||e.preventDefault(),b=1):(C=1,y.addClass("mCS_touch_action")),U&&e.preventDefault(),w="yx"===T.axis?[u-o,f-a]:"x"===T.axis?[null,f-a]:[u-o,null],I[0].idleTimer=250,B.overflowed[0]&&s(w[0],R,n,"y","all",!0),B.overflowed[1]&&s(w[1],R,n,"x",L,!0)}}function i(e){if(!te(e)||c||O(e)[2])return void(t=0);t=1,e.stopImmediatePropagation(),Q(y),p=K();var o=M.offset();h=O(e)[0]-o.top,m=O(e)[1]-o.left,E=[],W=[]}function r(e){if(te(e)&&!c&&!O(e)[2]){d=0,e.stopImmediatePropagation(),b=0,C=0,v=K();var t=M.offset(),o=O(e)[0]-t.top,a=O(e)[1]-t.left;if(!(v-g>30)){_=1e3/(v-p);var n="mcsEaseOut",i=2.5>_,r=i?[E[E.length-2],W[W.length-2]]:[0,0];x=i?[o-r[0],a-r[1]]:[o-h,a-m];var u=[Math.abs(x[0]),Math.abs(x[1])];_=i?[Math.abs(x[0]/4),Math.abs(x[1]/4)]:[_,_];var f=[Math.abs(I[0].offsetTop)-x[0]*l(u[0]/_[0],_[0]),Math.abs(I[0].offsetLeft)-x[1]*l(u[1]/_[1],_[1])];w="yx"===T.axis?[f[0],f[1]]:"x"===T.axis?[null,f[1]]:[f[0],null],S=[4*u[0]+T.scrollInertia,4*u[1]+T.scrollInertia];var y=parseInt(T.contentTouchScroll)||0;w[0]=u[0]>y?w[0]:0,w[1]=u[1]>y?w[1]:0,B.overflowed[0]&&s(w[0],S[0],n,"y",L,!1),B.overflowed[1]&&s(w[1],S[1],n,"x",L,!1)}}}function l(e,t){var o=[1.5*t,2*t,t/1.5,t/2];return e>90?t>4?o[0]:o[3]:e>60?t>3?o[3]:o[2]:e>30?t>8?o[1]:t>6?o[0]:t>4?t:o[2]:t>8?t:o[3]}function s(e,t,o,a,n,i){e&&G(y,e.toString(),{dur:t,scrollEasing:o,dir:a,overwrite:n,drag:i})}var d,u,f,h,m,p,g,v,x,_,w,S,b,C,y=e(this),B=y.data(a),T=B.opt,k=a+"_"+B.idx,M=e("#mCSB_"+B.idx),I=e("#mCSB_"+B.idx+"_container"),D=[e("#mCSB_"+B.idx+"_dragger_vertical"),e("#mCSB_"+B.idx+"_dragger_horizontal")],E=[],W=[],R=0,L="yx"===T.axis?"none":"all",z=[],P=I.find("iframe"),H=["touchstart."+k+" pointerdown."+k+" MSPointerDown."+k,"touchmove."+k+" pointermove."+k+" MSPointerMove."+k,"touchend."+k+" pointerup."+k+" MSPointerUp."+k],U=void 0!==document.body.style.touchAction&&""!==document.body.style.touchAction;I.bind(H[0],function(e){o(e)}).bind(H[1],function(e){n(e)}),M.bind(H[0],function(e){i(e)}).bind(H[2],function(e){r(e)}),P.length&&P.each(function(){e(this).bind("load",function(){A(this)&&e(this.contentDocument||this.contentWindow.document).bind(H[0],function(e){o(e),i(e)}).bind(H[1],function(e){n(e)}).bind(H[2],function(e){r(e)})})})},E=function(){function o(){return window.getSelection?window.getSelection().toString():document.selection&&"Control"!=document.selection.type?document.selection.createRange().text:0}function n(e,t,o){d.type=o&&i?"stepped":"stepless",d.scrollAmount=10,j(r,e,t,"mcsLinearOut",o?60:null)}var i,r=e(this),l=r.data(a),s=l.opt,d=l.sequential,u=a+"_"+l.idx,f=e("#mCSB_"+l.idx+"_container"),h=f.parent();f.bind("mousedown."+u,function(){t||i||(i=1,c=!0)}).add(document).bind("mousemove."+u,function(e){if(!t&&i&&o()){var a=f.offset(),r=O(e)[0]-a.top+f[0].offsetTop,c=O(e)[1]-a.left+f[0].offsetLeft;r>0&&r<h.height()&&c>0&&c<h.width()?d.step&&n("off",null,"stepped"):("x"!==s.axis&&l.overflowed[0]&&(0>r?n("on",38):r>h.height()&&n("on",40)),"y"!==s.axis&&l.overflowed[1]&&(0>c?n("on",37):c>h.width()&&n("on",39)))}}).bind("mouseup."+u+" dragend."+u,function(){t||(i&&(i=0,n("off",null)),c=!1)})},W=function(){function t(t,a){if(Q(o),!z(o,t.target)){var r="auto"!==i.mouseWheel.deltaFactor?parseInt(i.mouseWheel.deltaFactor):s&&t.deltaFactor<100?100:t.deltaFactor||100,d=i.scrollInertia;if("x"===i.axis||"x"===i.mouseWheel.axis)var u="x",f=[Math.round(r*n.scrollRatio.x),parseInt(i.mouseWheel.scrollAmount)],h="auto"!==i.mouseWheel.scrollAmount?f[1]:f[0]>=l.width()?.9*l.width():f[0],m=Math.abs(e("#mCSB_"+n.idx+"_container")[0].offsetLeft),p=c[1][0].offsetLeft,g=c[1].parent().width()-c[1].width(),v="y"===i.mouseWheel.axis?t.deltaY||a:t.deltaX;else var u="y",f=[Math.round(r*n.scrollRatio.y),parseInt(i.mouseWheel.scrollAmount)],h="auto"!==i.mouseWheel.scrollAmount?f[1]:f[0]>=l.height()?.9*l.height():f[0],m=Math.abs(e("#mCSB_"+n.idx+"_container")[0].offsetTop),p=c[0][0].offsetTop,g=c[0].parent().height()-c[0].height(),v=t.deltaY||a;"y"===u&&!n.overflowed[0]||"x"===u&&!n.overflowed[1]||((i.mouseWheel.invert||t.webkitDirectionInvertedFromDevice)&&(v=-v),i.mouseWheel.normalizeDelta&&(v=0>v?-1:1),(v>0&&0!==p||0>v&&p!==g||i.mouseWheel.preventDefault)&&(t.stopImmediatePropagation(),t.preventDefault()),t.deltaFactor<5&&!i.mouseWheel.normalizeDelta&&(h=t.deltaFactor,d=17),G(o,(m-v*h).toString(),{dir:u,dur:d}))}}if(e(this).data(a)){var o=e(this),n=o.data(a),i=n.opt,r=a+"_"+n.idx,l=e("#mCSB_"+n.idx),c=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")],d=e("#mCSB_"+n.idx+"_container").find("iframe");d.length&&d.each(function(){e(this).bind("load",function(){A(this)&&e(this.contentDocument||this.contentWindow.document).bind("mousewheel."+r,function(e,o){t(e,o)})})}),l.bind("mousewheel."+r,function(e,o){t(e,o)})}},R=new Object,A=function(t){var o=!1,a=!1,n=null;if(void 0===t?a="#empty":void 0!==e(t).attr("id")&&(a=e(t).attr("id")),a!==!1&&void 0!==R[a])return R[a];if(t){try{var i=t.contentDocument||t.contentWindow.document;n=i.body.innerHTML}catch(r){}o=null!==n}else{try{var i=top.document;n=i.body.innerHTML}catch(r){}o=null!==n}return a!==!1&&(R[a]=o),o},L=function(e){var t=this.find("iframe");if(t.length){var o=e?"auto":"none";t.css("pointer-events",o)}},z=function(t,o){var n=o.nodeName.toLowerCase(),i=t.data(a).opt.mouseWheel.disableOver,r=["select","textarea"];return e.inArray(n,i)>-1&&!(e.inArray(n,r)>-1&&!e(o).is(":focus"))},P=function(){var t,o=e(this),n=o.data(a),i=a+"_"+n.idx,r=e("#mCSB_"+n.idx+"_container"),l=r.parent(),s=e(".mCSB_"+n.idx+"_scrollbar ."+d[12]);s.bind("mousedown."+i+" touchstart."+i+" pointerdown."+i+" MSPointerDown."+i,function(o){c=!0,e(o.target).hasClass("mCSB_dragger")||(t=1)}).bind("touchend."+i+" pointerup."+i+" MSPointerUp."+i,function(){c=!1}).bind("click."+i,function(a){if(t&&(t=0,e(a.target).hasClass(d[12])||e(a.target).hasClass("mCSB_draggerRail"))){Q(o);var i=e(this),s=i.find(".mCSB_dragger");if(i.parent(".mCSB_scrollTools_horizontal").length>0){if(!n.overflowed[1])return;var c="x",u=a.pageX>s.offset().left?-1:1,f=Math.abs(r[0].offsetLeft)-u*(.9*l.width())}else{if(!n.overflowed[0])return;var c="y",u=a.pageY>s.offset().top?-1:1,f=Math.abs(r[0].offsetTop)-u*(.9*l.height())}G(o,f.toString(),{dir:c,scrollEasing:"mcsEaseInOut"})}})},H=function(){var t=e(this),o=t.data(a),n=o.opt,i=a+"_"+o.idx,r=e("#mCSB_"+o.idx+"_container"),l=r.parent();r.bind("focusin."+i,function(){var o=e(document.activeElement),a=r.find(".mCustomScrollBox").length,i=0;o.is(n.advanced.autoScrollOnFocus)&&(Q(t),clearTimeout(t[0]._focusTimeout),t[0]._focusTimer=a?(i+17)*a:0,t[0]._focusTimeout=setTimeout(function(){var e=[ae(o)[0],ae(o)[1]],a=[r[0].offsetTop,r[0].offsetLeft],s=[a[0]+e[0]>=0&&a[0]+e[0]<l.height()-o.outerHeight(!1),a[1]+e[1]>=0&&a[0]+e[1]<l.width()-o.outerWidth(!1)],c="yx"!==n.axis||s[0]||s[1]?"all":"none";"x"===n.axis||s[0]||G(t,e[0].toString(),{dir:"y",scrollEasing:"mcsEaseInOut",overwrite:c,dur:i}),"y"===n.axis||s[1]||G(t,e[1].toString(),{dir:"x",scrollEasing:"mcsEaseInOut",overwrite:c,dur:i})},t[0]._focusTimer))})},U=function(){var t=e(this),o=t.data(a),n=a+"_"+o.idx,i=e("#mCSB_"+o.idx+"_container").parent();i.bind("scroll."+n,function(){0===i.scrollTop()&&0===i.scrollLeft()||e(".mCSB_"+o.idx+"_scrollbar").css("visibility","hidden")})},F=function(){var t=e(this),o=t.data(a),n=o.opt,i=o.sequential,r=a+"_"+o.idx,l=".mCSB_"+o.idx+"_scrollbar",s=e(l+">a");s.bind("contextmenu."+r,function(e){e.preventDefault()}).bind("mousedown."+r+" touchstart."+r+" pointerdown."+r+" MSPointerDown."+r+" mouseup."+r+" touchend."+r+" pointerup."+r+" MSPointerUp."+r+" mouseout."+r+" pointerout."+r+" MSPointerOut."+r+" click."+r,function(a){function r(e,o){i.scrollAmount=n.scrollButtons.scrollAmount,j(t,e,o)}if(a.preventDefault(),ee(a)){var l=e(this).attr("class");switch(i.type=n.scrollButtons.scrollType,a.type){case"mousedown":case"touchstart":case"pointerdown":case"MSPointerDown":if("stepped"===i.type)return;c=!0,o.tweenRunning=!1,r("on",l);break;case"mouseup":case"touchend":case"pointerup":case"MSPointerUp":case"mouseout":case"pointerout":case"MSPointerOut":if("stepped"===i.type)return;c=!1,i.dir&&r("off",l);break;case"click":if("stepped"!==i.type||o.tweenRunning)return;r("on",l)}}})},q=function(){function t(t){function a(e,t){r.type=i.keyboard.scrollType,r.scrollAmount=i.keyboard.scrollAmount,"stepped"===r.type&&n.tweenRunning||j(o,e,t)}switch(t.type){case"blur":n.tweenRunning&&r.dir&&a("off",null);break;case"keydown":case"keyup":var l=t.keyCode?t.keyCode:t.which,s="on";if("x"!==i.axis&&(38===l||40===l)||"y"!==i.axis&&(37===l||39===l)){if((38===l||40===l)&&!n.overflowed[0]||(37===l||39===l)&&!n.overflowed[1])return;"keyup"===t.type&&(s="off"),e(document.activeElement).is(u)||(t.preventDefault(),t.stopImmediatePropagation(),a(s,l))}else if(33===l||34===l){if((n.overflowed[0]||n.overflowed[1])&&(t.preventDefault(),t.stopImmediatePropagation()),"keyup"===t.type){Q(o);var f=34===l?-1:1;if("x"===i.axis||"yx"===i.axis&&n.overflowed[1]&&!n.overflowed[0])var h="x",m=Math.abs(c[0].offsetLeft)-f*(.9*d.width());else var h="y",m=Math.abs(c[0].offsetTop)-f*(.9*d.height());G(o,m.toString(),{dir:h,scrollEasing:"mcsEaseInOut"})}}else if((35===l||36===l)&&!e(document.activeElement).is(u)&&((n.overflowed[0]||n.overflowed[1])&&(t.preventDefault(),t.stopImmediatePropagation()),"keyup"===t.type)){if("x"===i.axis||"yx"===i.axis&&n.overflowed[1]&&!n.overflowed[0])var h="x",m=35===l?Math.abs(d.width()-c.outerWidth(!1)):0;else var h="y",m=35===l?Math.abs(d.height()-c.outerHeight(!1)):0;G(o,m.toString(),{dir:h,scrollEasing:"mcsEaseInOut"})}}}var o=e(this),n=o.data(a),i=n.opt,r=n.sequential,l=a+"_"+n.idx,s=e("#mCSB_"+n.idx),c=e("#mCSB_"+n.idx+"_container"),d=c.parent(),u="input,textarea,select,datalist,keygen,[contenteditable='true']",f=c.find("iframe"),h=["blur."+l+" keydown."+l+" keyup."+l];f.length&&f.each(function(){e(this).bind("load",function(){A(this)&&e(this.contentDocument||this.contentWindow.document).bind(h[0],function(e){t(e)})})}),s.attr("tabindex","0").bind(h[0],function(e){t(e)})},j=function(t,o,n,i,r){function l(e){u.snapAmount&&(f.scrollAmount=u.snapAmount instanceof Array?"x"===f.dir[0]?u.snapAmount[1]:u.snapAmount[0]:u.snapAmount);var o="stepped"!==f.type,a=r?r:e?o?p/1.5:g:1e3/60,n=e?o?7.5:40:2.5,s=[Math.abs(h[0].offsetTop),Math.abs(h[0].offsetLeft)],d=[c.scrollRatio.y>10?10:c.scrollRatio.y,c.scrollRatio.x>10?10:c.scrollRatio.x],m="x"===f.dir[0]?s[1]+f.dir[1]*(d[1]*n):s[0]+f.dir[1]*(d[0]*n),v="x"===f.dir[0]?s[1]+f.dir[1]*parseInt(f.scrollAmount):s[0]+f.dir[1]*parseInt(f.scrollAmount),x="auto"!==f.scrollAmount?v:m,_=i?i:e?o?"mcsLinearOut":"mcsEaseInOut":"mcsLinear",w=!!e;return e&&17>a&&(x="x"===f.dir[0]?s[1]:s[0]),G(t,x.toString(),{dir:f.dir[0],scrollEasing:_,dur:a,onComplete:w}),e?void(f.dir=!1):(clearTimeout(f.step),void(f.step=setTimeout(function(){l()},a)))}function s(){clearTimeout(f.step),$(f,"step"),Q(t)}var c=t.data(a),u=c.opt,f=c.sequential,h=e("#mCSB_"+c.idx+"_container"),m="stepped"===f.type,p=u.scrollInertia<26?26:u.scrollInertia,g=u.scrollInertia<1?17:u.scrollInertia;switch(o){case"on":if(f.dir=[n===d[16]||n===d[15]||39===n||37===n?"x":"y",n===d[13]||n===d[15]||38===n||37===n?-1:1],Q(t),oe(n)&&"stepped"===f.type)return;l(m);break;case"off":s(),(m||c.tweenRunning&&f.dir)&&l(!0)}},Y=function(t){var o=e(this).data(a).opt,n=[];return"function"==typeof t&&(t=t()),t instanceof Array?n=t.length>1?[t[0],t[1]]:"x"===o.axis?[null,t[0]]:[t[0],null]:(n[0]=t.y?t.y:t.x||"x"===o.axis?null:t,n[1]=t.x?t.x:t.y||"y"===o.axis?null:t),"function"==typeof n[0]&&(n[0]=n[0]()),"function"==typeof n[1]&&(n[1]=n[1]()),n},X=function(t,o){if(null!=t&&"undefined"!=typeof t){var n=e(this),i=n.data(a),r=i.opt,l=e("#mCSB_"+i.idx+"_container"),s=l.parent(),c=typeof t;o||(o="x"===r.axis?"x":"y");var d="x"===o?l.outerWidth(!1)-s.width():l.outerHeight(!1)-s.height(),f="x"===o?l[0].offsetLeft:l[0].offsetTop,h="x"===o?"left":"top";switch(c){case"function":return t();case"object":var m=t.jquery?t:e(t);if(!m.length)return;return"x"===o?ae(m)[1]:ae(m)[0];case"string":case"number":if(oe(t))return Math.abs(t);if(-1!==t.indexOf("%"))return Math.abs(d*parseInt(t)/100);if(-1!==t.indexOf("-="))return Math.abs(f-parseInt(t.split("-=")[1]));if(-1!==t.indexOf("+=")){var p=f+parseInt(t.split("+=")[1]);return p>=0?0:Math.abs(p)}if(-1!==t.indexOf("px")&&oe(t.split("px")[0]))return Math.abs(t.split("px")[0]);if("top"===t||"left"===t)return 0;if("bottom"===t)return Math.abs(s.height()-l.outerHeight(!1));if("right"===t)return Math.abs(s.width()-l.outerWidth(!1));if("first"===t||"last"===t){var m=l.find(":"+t);return"x"===o?ae(m)[1]:ae(m)[0]}return e(t).length?"x"===o?ae(e(t))[1]:ae(e(t))[0]:(l.css(h,t),void u.update.call(null,n[0]))}}},N=function(t){function o(){return clearTimeout(f[0].autoUpdate),0===l.parents("html").length?void(l=null):void(f[0].autoUpdate=setTimeout(function(){return c.advanced.updateOnSelectorChange&&(s.poll.change.n=i(),s.poll.change.n!==s.poll.change.o)?(s.poll.change.o=s.poll.change.n,void r(3)):c.advanced.updateOnContentResize&&(s.poll.size.n=l[0].scrollHeight+l[0].scrollWidth+f[0].offsetHeight+l[0].offsetHeight+l[0].offsetWidth,s.poll.size.n!==s.poll.size.o)?(s.poll.size.o=s.poll.size.n,void r(1)):!c.advanced.updateOnImageLoad||"auto"===c.advanced.updateOnImageLoad&&"y"===c.axis||(s.poll.img.n=f.find("img").length,s.poll.img.n===s.poll.img.o)?void((c.advanced.updateOnSelectorChange||c.advanced.updateOnContentResize||c.advanced.updateOnImageLoad)&&o()):(s.poll.img.o=s.poll.img.n,void f.find("img").each(function(){n(this)}))},c.advanced.autoUpdateTimeout))}function n(t){function o(e,t){return function(){
	return t.apply(e,arguments)}}function a(){this.onload=null,e(t).addClass(d[2]),r(2)}if(e(t).hasClass(d[2]))return void r();var n=new Image;n.onload=o(n,a),n.src=t.src}function i(){c.advanced.updateOnSelectorChange===!0&&(c.advanced.updateOnSelectorChange="*");var e=0,t=f.find(c.advanced.updateOnSelectorChange);return c.advanced.updateOnSelectorChange&&t.length>0&&t.each(function(){e+=this.offsetHeight+this.offsetWidth}),e}function r(e){clearTimeout(f[0].autoUpdate),u.update.call(null,l[0],e)}var l=e(this),s=l.data(a),c=s.opt,f=e("#mCSB_"+s.idx+"_container");return t?(clearTimeout(f[0].autoUpdate),void $(f[0],"autoUpdate")):void o()},V=function(e,t,o){return Math.round(e/t)*t-o},Q=function(t){var o=t.data(a),n=e("#mCSB_"+o.idx+"_container,#mCSB_"+o.idx+"_container_wrapper,#mCSB_"+o.idx+"_dragger_vertical,#mCSB_"+o.idx+"_dragger_horizontal");n.each(function(){Z.call(this)})},G=function(t,o,n){function i(e){return s&&c.callbacks[e]&&"function"==typeof c.callbacks[e]}function r(){return[c.callbacks.alwaysTriggerOffsets||w>=S[0]+y,c.callbacks.alwaysTriggerOffsets||-B>=w]}function l(){var e=[h[0].offsetTop,h[0].offsetLeft],o=[x[0].offsetTop,x[0].offsetLeft],a=[h.outerHeight(!1),h.outerWidth(!1)],i=[f.height(),f.width()];t[0].mcs={content:h,top:e[0],left:e[1],draggerTop:o[0],draggerLeft:o[1],topPct:Math.round(100*Math.abs(e[0])/(Math.abs(a[0])-i[0])),leftPct:Math.round(100*Math.abs(e[1])/(Math.abs(a[1])-i[1])),direction:n.dir}}var s=t.data(a),c=s.opt,d={trigger:"internal",dir:"y",scrollEasing:"mcsEaseOut",drag:!1,dur:c.scrollInertia,overwrite:"all",callbacks:!0,onStart:!0,onUpdate:!0,onComplete:!0},n=e.extend(d,n),u=[n.dur,n.drag?0:n.dur],f=e("#mCSB_"+s.idx),h=e("#mCSB_"+s.idx+"_container"),m=h.parent(),p=c.callbacks.onTotalScrollOffset?Y.call(t,c.callbacks.onTotalScrollOffset):[0,0],g=c.callbacks.onTotalScrollBackOffset?Y.call(t,c.callbacks.onTotalScrollBackOffset):[0,0];if(s.trigger=n.trigger,0===m.scrollTop()&&0===m.scrollLeft()||(e(".mCSB_"+s.idx+"_scrollbar").css("visibility","visible"),m.scrollTop(0).scrollLeft(0)),"_resetY"!==o||s.contentReset.y||(i("onOverflowYNone")&&c.callbacks.onOverflowYNone.call(t[0]),s.contentReset.y=1),"_resetX"!==o||s.contentReset.x||(i("onOverflowXNone")&&c.callbacks.onOverflowXNone.call(t[0]),s.contentReset.x=1),"_resetY"!==o&&"_resetX"!==o){if(!s.contentReset.y&&t[0].mcs||!s.overflowed[0]||(i("onOverflowY")&&c.callbacks.onOverflowY.call(t[0]),s.contentReset.x=null),!s.contentReset.x&&t[0].mcs||!s.overflowed[1]||(i("onOverflowX")&&c.callbacks.onOverflowX.call(t[0]),s.contentReset.x=null),c.snapAmount){var v=c.snapAmount instanceof Array?"x"===n.dir?c.snapAmount[1]:c.snapAmount[0]:c.snapAmount;o=V(o,v,c.snapOffset)}switch(n.dir){case"x":var x=e("#mCSB_"+s.idx+"_dragger_horizontal"),_="left",w=h[0].offsetLeft,S=[f.width()-h.outerWidth(!1),x.parent().width()-x.width()],b=[o,0===o?0:o/s.scrollRatio.x],y=p[1],B=g[1],T=y>0?y/s.scrollRatio.x:0,k=B>0?B/s.scrollRatio.x:0;break;case"y":var x=e("#mCSB_"+s.idx+"_dragger_vertical"),_="top",w=h[0].offsetTop,S=[f.height()-h.outerHeight(!1),x.parent().height()-x.height()],b=[o,0===o?0:o/s.scrollRatio.y],y=p[0],B=g[0],T=y>0?y/s.scrollRatio.y:0,k=B>0?B/s.scrollRatio.y:0}b[1]<0||0===b[0]&&0===b[1]?b=[0,0]:b[1]>=S[1]?b=[S[0],S[1]]:b[0]=-b[0],t[0].mcs||(l(),i("onInit")&&c.callbacks.onInit.call(t[0])),clearTimeout(h[0].onCompleteTimeout),J(x[0],_,Math.round(b[1]),u[1],n.scrollEasing),!s.tweenRunning&&(0===w&&b[0]>=0||w===S[0]&&b[0]<=S[0])||J(h[0],_,Math.round(b[0]),u[0],n.scrollEasing,n.overwrite,{onStart:function(){n.callbacks&&n.onStart&&!s.tweenRunning&&(i("onScrollStart")&&(l(),c.callbacks.onScrollStart.call(t[0])),s.tweenRunning=!0,C(x),s.cbOffsets=r())},onUpdate:function(){n.callbacks&&n.onUpdate&&i("whileScrolling")&&(l(),c.callbacks.whileScrolling.call(t[0]))},onComplete:function(){if(n.callbacks&&n.onComplete){"yx"===c.axis&&clearTimeout(h[0].onCompleteTimeout);var e=h[0].idleTimer||0;h[0].onCompleteTimeout=setTimeout(function(){i("onScroll")&&(l(),c.callbacks.onScroll.call(t[0])),i("onTotalScroll")&&b[1]>=S[1]-T&&s.cbOffsets[0]&&(l(),c.callbacks.onTotalScroll.call(t[0])),i("onTotalScrollBack")&&b[1]<=k&&s.cbOffsets[1]&&(l(),c.callbacks.onTotalScrollBack.call(t[0])),s.tweenRunning=!1,h[0].idleTimer=0,C(x,"hide")},e)}}})}},J=function(e,t,o,a,n,i,r){function l(){S.stop||(x||m.call(),x=K()-v,s(),x>=S.time&&(S.time=x>S.time?x+f-(x-S.time):x+f-1,S.time<x+1&&(S.time=x+1)),S.time<a?S.id=h(l):g.call())}function s(){a>0?(S.currVal=u(S.time,_,b,a,n),w[t]=Math.round(S.currVal)+"px"):w[t]=o+"px",p.call()}function c(){f=1e3/60,S.time=x+f,h=window.requestAnimationFrame?window.requestAnimationFrame:function(e){return s(),setTimeout(e,.01)},S.id=h(l)}function d(){null!=S.id&&(window.requestAnimationFrame?window.cancelAnimationFrame(S.id):clearTimeout(S.id),S.id=null)}function u(e,t,o,a,n){switch(n){case"linear":case"mcsLinear":return o*e/a+t;case"mcsLinearOut":return e/=a,e--,o*Math.sqrt(1-e*e)+t;case"easeInOutSmooth":return e/=a/2,1>e?o/2*e*e+t:(e--,-o/2*(e*(e-2)-1)+t);case"easeInOutStrong":return e/=a/2,1>e?o/2*Math.pow(2,10*(e-1))+t:(e--,o/2*(-Math.pow(2,-10*e)+2)+t);case"easeInOut":case"mcsEaseInOut":return e/=a/2,1>e?o/2*e*e*e+t:(e-=2,o/2*(e*e*e+2)+t);case"easeOutSmooth":return e/=a,e--,-o*(e*e*e*e-1)+t;case"easeOutStrong":return o*(-Math.pow(2,-10*e/a)+1)+t;case"easeOut":case"mcsEaseOut":default:var i=(e/=a)*e,r=i*e;return t+o*(.499999999999997*r*i+-2.5*i*i+5.5*r+-6.5*i+4*e)}}e._mTween||(e._mTween={top:{},left:{}});var f,h,r=r||{},m=r.onStart||function(){},p=r.onUpdate||function(){},g=r.onComplete||function(){},v=K(),x=0,_=e.offsetTop,w=e.style,S=e._mTween[t];"left"===t&&(_=e.offsetLeft);var b=o-_;S.stop=0,"none"!==i&&d(),c()},K=function(){return window.performance&&window.performance.now?window.performance.now():window.performance&&window.performance.webkitNow?window.performance.webkitNow():Date.now?Date.now():(new Date).getTime()},Z=function(){var e=this;e._mTween||(e._mTween={top:{},left:{}});for(var t=["top","left"],o=0;o<t.length;o++){var a=t[o];e._mTween[a].id&&(window.requestAnimationFrame?window.cancelAnimationFrame(e._mTween[a].id):clearTimeout(e._mTween[a].id),e._mTween[a].id=null,e._mTween[a].stop=1)}},$=function(e,t){try{delete e[t]}catch(o){e[t]=null}},ee=function(e){return!(e.which&&1!==e.which)},te=function(e){var t=e.originalEvent.pointerType;return!(t&&"touch"!==t&&2!==t)},oe=function(e){return!isNaN(parseFloat(e))&&isFinite(e)},ae=function(e){var t=e.parents(".mCSB_container");return[e.offset().top-t.offset().top,e.offset().left-t.offset().left]},ne=function(){function e(){var e=["webkit","moz","ms","o"];if("hidden"in document)return"hidden";for(var t=0;t<e.length;t++)if(e[t]+"Hidden"in document)return e[t]+"Hidden";return null}var t=e();return t?document[t]:!1};e.fn[o]=function(t){return u[t]?u[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist"):u.init.apply(this,arguments)},e[o]=function(t){return u[t]?u[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist"):u.init.apply(this,arguments)},e[o].defaults=i,window[o]=!0,e(window).bind("load",function(){e(n)[o](),e.extend(e.expr[":"],{mcsInView:e.expr[":"].mcsInView||function(t){var o,a,n=e(t),i=n.parents(".mCSB_container");if(i.length)return o=i.parent(),a=[i[0].offsetTop,i[0].offsetLeft],a[0]+ae(n)[0]>=0&&a[0]+ae(n)[0]<o.height()-n.outerHeight(!1)&&a[1]+ae(n)[1]>=0&&a[1]+ae(n)[1]<o.width()-n.outerWidth(!1)},mcsInSight:e.expr[":"].mcsInSight||function(t,o,a){var n,i,r,l,s=e(t),c=s.parents(".mCSB_container"),d="exact"===a[3]?[[1,0],[1,0]]:[[.9,.1],[.6,.4]];if(c.length)return n=[s.outerHeight(!1),s.outerWidth(!1)],r=[c[0].offsetTop+ae(s)[0],c[0].offsetLeft+ae(s)[1]],i=[c.parent()[0].offsetHeight,c.parent()[0].offsetWidth],l=[n[0]<i[0]?d[0]:d[1],n[1]<i[1]?d[0]:d[1]],r[0]-i[0]*l[0][0]<0&&r[0]+n[0]-i[0]*l[0][1]>=0&&r[1]-i[1]*l[1][0]<0&&r[1]+n[1]-i[1]*l[1][1]>=0},mcsOverflow:e.expr[":"].mcsOverflow||function(t){var o=e(t).data(a);if(o)return o.overflowed[0]||o.overflowed[1]}})})})});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 210:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/*!
	 * jQuery Mousewheel 3.1.13
	 *
	 * Copyright jQuery Foundation and other contributors
	 * Released under the MIT license
	 * http://jquery.org/license
	 */
	
	(function (factory) {
	    if ( true ) {
	        // AMD. Register as an anonymous module.
	        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (factory), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	    } else if (typeof exports === 'object') {
	        // Node/CommonJS style for Browserify
	        module.exports = factory;
	    } else {
	        // Browser globals
	        factory(jQuery);
	    }
	}(function ($) {
	
	    var toFix  = ['wheel', 'mousewheel', 'DOMMouseScroll', 'MozMousePixelScroll'],
	        toBind = ( 'onwheel' in document || document.documentMode >= 9 ) ?
	                    ['wheel'] : ['mousewheel', 'DomMouseScroll', 'MozMousePixelScroll'],
	        slice  = Array.prototype.slice,
	        nullLowestDeltaTimeout, lowestDelta;
	
	    if ( $.event.fixHooks ) {
	        for ( var i = toFix.length; i; ) {
	            $.event.fixHooks[ toFix[--i] ] = $.event.mouseHooks;
	        }
	    }
	
	    var special = $.event.special.mousewheel = {
	        version: '3.1.12',
	
	        setup: function() {
	            if ( this.addEventListener ) {
	                for ( var i = toBind.length; i; ) {
	                    this.addEventListener( toBind[--i], handler, false );
	                }
	            } else {
	                this.onmousewheel = handler;
	            }
	            // Store the line height and page height for this particular element
	            $.data(this, 'mousewheel-line-height', special.getLineHeight(this));
	            $.data(this, 'mousewheel-page-height', special.getPageHeight(this));
	        },
	
	        teardown: function() {
	            if ( this.removeEventListener ) {
	                for ( var i = toBind.length; i; ) {
	                    this.removeEventListener( toBind[--i], handler, false );
	                }
	            } else {
	                this.onmousewheel = null;
	            }
	            // Clean up the data we added to the element
	            $.removeData(this, 'mousewheel-line-height');
	            $.removeData(this, 'mousewheel-page-height');
	        },
	
	        getLineHeight: function(elem) {
	            var $elem = $(elem),
	                $parent = $elem['offsetParent' in $.fn ? 'offsetParent' : 'parent']();
	            if (!$parent.length) {
	                $parent = $('body');
	            }
	            return parseInt($parent.css('fontSize'), 10) || parseInt($elem.css('fontSize'), 10) || 16;
	        },
	
	        getPageHeight: function(elem) {
	            return $(elem).height();
	        },
	
	        settings: {
	            adjustOldDeltas: true, // see shouldAdjustOldDeltas() below
	            normalizeOffset: true  // calls getBoundingClientRect for each event
	        }
	    };
	
	    $.fn.extend({
	        mousewheel: function(fn) {
	            return fn ? this.bind('mousewheel', fn) : this.trigger('mousewheel');
	        },
	
	        unmousewheel: function(fn) {
	            return this.unbind('mousewheel', fn);
	        }
	    });
	
	
	    function handler(event) {
	        var orgEvent   = event || window.event,
	            args       = slice.call(arguments, 1),
	            delta      = 0,
	            deltaX     = 0,
	            deltaY     = 0,
	            absDelta   = 0,
	            offsetX    = 0,
	            offsetY    = 0;
	        event = $.event.fix(orgEvent);
	        event.type = 'mousewheel';
	
	        // Old school scrollwheel delta
	        if ( 'detail'      in orgEvent ) { deltaY = orgEvent.detail * -1;      }
	        if ( 'wheelDelta'  in orgEvent ) { deltaY = orgEvent.wheelDelta;       }
	        if ( 'wheelDeltaY' in orgEvent ) { deltaY = orgEvent.wheelDeltaY;      }
	        if ( 'wheelDeltaX' in orgEvent ) { deltaX = orgEvent.wheelDeltaX * -1; }
	
	        // Firefox < 17 horizontal scrolling related to DOMMouseScroll event
	        if ( 'axis' in orgEvent && orgEvent.axis === orgEvent.HORIZONTAL_AXIS ) {
	            deltaX = deltaY * -1;
	            deltaY = 0;
	        }
	
	        // Set delta to be deltaY or deltaX if deltaY is 0 for backwards compatabilitiy
	        delta = deltaY === 0 ? deltaX : deltaY;
	
	        // New school wheel delta (wheel event)
	        if ( 'deltaY' in orgEvent ) {
	            deltaY = orgEvent.deltaY * -1;
	            delta  = deltaY;
	        }
	        if ( 'deltaX' in orgEvent ) {
	            deltaX = orgEvent.deltaX;
	            if ( deltaY === 0 ) { delta  = deltaX * -1; }
	        }
	
	        // No change actually happened, no reason to go any further
	        if ( deltaY === 0 && deltaX === 0 ) { return; }
	
	        // Need to convert lines and pages to pixels if we aren't already in pixels
	        // There are three delta modes:
	        //   * deltaMode 0 is by pixels, nothing to do
	        //   * deltaMode 1 is by lines
	        //   * deltaMode 2 is by pages
	        if ( orgEvent.deltaMode === 1 ) {
	            var lineHeight = $.data(this, 'mousewheel-line-height');
	            delta  *= lineHeight;
	            deltaY *= lineHeight;
	            deltaX *= lineHeight;
	        } else if ( orgEvent.deltaMode === 2 ) {
	            var pageHeight = $.data(this, 'mousewheel-page-height');
	            delta  *= pageHeight;
	            deltaY *= pageHeight;
	            deltaX *= pageHeight;
	        }
	
	        // Store lowest absolute delta to normalize the delta values
	        absDelta = Math.max( Math.abs(deltaY), Math.abs(deltaX) );
	
	        if ( !lowestDelta || absDelta < lowestDelta ) {
	            lowestDelta = absDelta;
	
	            // Adjust older deltas if necessary
	            if ( shouldAdjustOldDeltas(orgEvent, absDelta) ) {
	                lowestDelta /= 40;
	            }
	        }
	
	        // Adjust older deltas if necessary
	        if ( shouldAdjustOldDeltas(orgEvent, absDelta) ) {
	            // Divide all the things by 40!
	            delta  /= 40;
	            deltaX /= 40;
	            deltaY /= 40;
	        }
	
	        // Get a whole, normalized value for the deltas
	        delta  = Math[ delta  >= 1 ? 'floor' : 'ceil' ](delta  / lowestDelta);
	        deltaX = Math[ deltaX >= 1 ? 'floor' : 'ceil' ](deltaX / lowestDelta);
	        deltaY = Math[ deltaY >= 1 ? 'floor' : 'ceil' ](deltaY / lowestDelta);
	
	        // Normalise offsetX and offsetY properties
	        if ( special.settings.normalizeOffset && this.getBoundingClientRect ) {
	            var boundingRect = this.getBoundingClientRect();
	            offsetX = event.clientX - boundingRect.left;
	            offsetY = event.clientY - boundingRect.top;
	        }
	
	        // Add information to the event object
	        event.deltaX = deltaX;
	        event.deltaY = deltaY;
	        event.deltaFactor = lowestDelta;
	        event.offsetX = offsetX;
	        event.offsetY = offsetY;
	        // Go ahead and set deltaMode to 0 since we converted to pixels
	        // Although this is a little odd since we overwrite the deltaX/Y
	        // properties with normalized deltas.
	        event.deltaMode = 0;
	
	        // Add event and delta to the front of the arguments
	        args.unshift(event, delta, deltaX, deltaY);
	
	        // Clearout lowestDelta after sometime to better
	        // handle multiple device types that give different
	        // a different lowestDelta
	        // Ex: trackpad = 3 and mouse wheel = 120
	        if (nullLowestDeltaTimeout) { clearTimeout(nullLowestDeltaTimeout); }
	        nullLowestDeltaTimeout = setTimeout(nullLowestDelta, 200);
	
	        return ($.event.dispatch || $.event.handle).apply(this, args);
	    }
	
	    function nullLowestDelta() {
	        lowestDelta = null;
	    }
	
	    function shouldAdjustOldDeltas(orgEvent, absDelta) {
	        // If this is an older event and the delta is divisable by 120,
	        // then we are assuming that the browser is treating this as an
	        // older mouse wheel event and that we should divide the deltas
	        // by 40 to try and get a more usable deltaFactor.
	        // Side note, this actually impacts the reported scroll distance
	        // in older browsers and can cause scrolling to be slower than native.
	        // Turn this off by setting $.event.special.mousewheel.settings.adjustOldDeltas to false.
	        return special.settings.adjustOldDeltas && orgEvent.type === 'mousewheel' && absDelta % 120 === 0;
	    }
	
	}));


/***/ },

/***/ 211:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(212)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Home) {
	    "use strict";
	    $(Home.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 212:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(7), __webpack_require__(213), __webpack_require__(199), __webpack_require__(214), __webpack_require__(215)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site, Recheckin) {
	    var $searchBox;
	    var _autocomplete;
	
	    function initialize() {
	        Site.initialize();  
	        initVariables();
	        initEvent();
	        activeDefaultMenuItem();
	        setTimeout(function () {
	            var $mainFrame = $("#mainFrame");
	            $mainFrame.attr('src', $mainFrame.data('src'));
	        }, 100);
	        
	        if ($('#HasRecheckIn').val() === "1") {
	            Recheckin.init();
	        }
	    }
	   
	    function initVariables() {
	        $searchBox = $('#searchbox');
	    }
	
	    function initEvent() {
	        initAutoComplete();
	
	        $searchBox.keyup(function (e) {
	            e = e || window.event;
	            var key = e.keyCode || e.which;
	
	            if (key === 13) {
	                searchByUserName();
	                _autocomplete.$dropdown.html('').hide();
	            }
	        });
	
	        $('#languagesite').on('click', '.languageItem', function () {
	            changeLanguage($(this));
	        });
	
	        $("#ddlLicensee").select2()
	        .on('change', function () {
	            var licenseeId = $(this).val();
	            top.location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
	        });
	
	        $('#label-search').click(function () {
	            $(this).parent().addClass('focused');
	            $searchBox.focus();
	            searchByUserName();
	        });
	
	        $searchBox.blur(function () {
	            if (!$(this).val()) {
	                $(this).parent().removeClass('focused');
	            }
	        });
	
	        $('body').click(function () {
	            _autocomplete.$dropdown.html('').hide();
	        });
	
	        $('.collapsible2').on("click.menuevent", ".collapsible-body  a", function () {
	            var $currentMenuItem = $(this);
	            document.title = 'LRF' + ' - ' + $(".active > .menu-title").text() + ' - ' + $currentMenuItem.text();
	
	            var isCancelledBetListMenu = $currentMenuItem.prop("id") === "MenuItem36";
	            if (isCancelledBetListMenu) {
	                $("#page-title").text(Site.getSingleResource("CancelledBetsTitle"));
	                return;
	            }
	            $("#page-title").text($currentMenuItem.text());
	        });
	
	        $("#mainFrame").on("load", function () {
	            $(window.frames["main"]).click(function () {
	                $('.dropdown-button').dropdown('close');
	                $('#ddlLicensee').select2("close");
	            });
	        });
	    }
	
	    function searchByUserName() {
	        var userName = $searchBox.val();
	        if (!userName || userName === "") {
	            return;
	        }
	
	        var url = Site.resolveClientUrl("Customer/CustomerList?userName=" + userName);
	        top.main.location.href = url;
	        $('.collapsible2').collapsibleMenu('activeMenuItem', $('#MenuItem43'));
	    }
	
	    function initAutoComplete() {
	        _autocomplete = $searchBox.materialize_autocomplete({ // jshint ignore:line
	            ignoreCase: false,
	            multiple: {
	                enable: false
	            },
	            dropdown: {
	                el: '#suggestionDropdown'
	            },
	            getData: function (value, callback) {
	                var url = Site.resolveClientUrl("CustomerAPI/SearchCustomer?query=" + value);
	
	                $.ajax({
	                    type: "GET",
	                    dataType: "json",
	                    url: url,
	                    success: function (response) {
	                        var suggestions = response.suggestions;
	
	                        if (suggestions && suggestions.length > 0) {
	                            var data = suggestions.map(function (item) {
	                                return { id: item, text: item };
	                            });
	
	                            callback(value, data);
	                        }
	                    }
	                });
	            }
	        });
	    }
	
	    function changeLanguage($target) {
	        var lang = $target.attr('langkey');
	        var mainFrame = top.frames["main"];
	        if (mainFrame && mainFrame.Site && mainFrame.Site.blockUI) {
	            mainFrame.Site.blockUI();
	        }
	
	        $.ajax({
	            url: Site.resolveClientUrl("Header/ChangeLanguage?languagesite=") + lang,
	            async: false,
	            type: "GET",
	            cache: false,
	            success: function (result) {
	                if (result === "1") {
	                    if ($.cookie("isClosedNotification") !== "1") {
	                        $.cookie('isFirstLoad', '1', { path: '/' });
	                    }
	                    top.location.reload();
	                }
	            }
	        });
	    }
	
	    function activeDefaultMenuItem() {
	        var defaultUrl = $("#DefaultUrl").val();
	        var $menuItem = $("#left-menu .collapsible-body").find("a[href='" + defaultUrl + "']");
	        $('.collapsible2').collapsibleMenu('activeMenuItem', $menuItem);
	
	        var menuItemName = $menuItem.length > 0 ? $menuItem.text() : "";
	        $("#page-title").text(menuItemName);
	
	        document.title = 'LRF' + ' - ' + $(".active > .menu-title").text() + ' - ' + menuItemName;
	    }
	
	    return {
	        initialize: initialize
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 213:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {/// <reference path="/@JSense.js" />
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(7)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site) {
	    "use strict";
	    var _handle;
	    var _onUserId;
	
	    function logout() {
	        var index = window;
	        while (index !== index.parent) {
	            index = index.parent;
	        }
	
	        index.location = Site.resolveClientUrl('Authorization/SignOut');
	    }
	
	    function closeBrowser() {
	        var index = window;
	        while (index !== index.parent) {
	            index = index.parent;
	        }
	
	        index.close();
	    }
	
	    function reCheckin() {
	        var Success = 0;
	        // var Unknown = -1;
	        // var AccessDenied = 1;
	        // var AccountClosed = 2;
	        // var KickedOut = 3;
	        // var UnderMaintenance = 4;
	        var SessionOverride = 5;
	
	        $.ajax({
	            url: Site.resolveClientUrl("Authorization/ReCheckIn?onid=") + _onUserId,
	            async: true,
	            type: "GET",
	            cache: false,
	            success: function (result) {
	                if (typeof result !== "object") {
	                    return;
	                }
	
	                var errCode = result.ErrCode;
	                if (errCode !== Success) {
	                    clearInterval(_handle);
	                    alert(result.ErrMsg, true);
	                    var isInternal = $("#IsInternal").val() === "1";
	                    if (isInternal || SessionOverride === errCode) {
	                        var signOutUrl = Site.resolveClientUrl('Authorization/SignOut');
	                        $.ajax({
	                            url: signOutUrl,
	                            complete: function () {
	                                closeBrowser();
	                            },
	                            type: "GET",
	                            async: true
	                        });
	                    }
	                    else {
	                        logout();
	                    }
	                }
	            }
	        });
	    }
	
	    return {
	        init: function () {
	            _onUserId = $("#OnUserId").val();
	            reCheckin();
	            _handle = setInterval(reCheckin, 30000);
	        }
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 214:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;(function (root, factory) {
	
	    if(typeof module === 'object' && module.exports) {
	        module.exports = factory(__webpack_require__(1));
	    } else if(true) {
	        !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (factory), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	    } else {
	        factory(root.jQuery);
	    }
	
	}(this, function ($) {
	
	    var template = function (text) {
	        var matcher = new RegExp('<%=([\\s\\S]+?)%>|<%([\\s\\S]+?)%>|$', 'g');
	
	        var escapes = {
	            "'": "'",
	            '\\': '\\',
	            '\r': 'r',
	            '\n': 'n',
	            '\u2028': 'u2028',
	            '\u2029': 'u2029'
	        };
	
	        var escapeRegExp = /\\|'|\r|\n|\u2028|\u2029/g;
	
	        var escapeChar = function(match) {
	            return '\\' + escapes[match];
	        };
	
	        var index = 0;
	        var source = "__p+='";
	
	        text.replace(matcher, function(match, interpolate, evaluate, offset) {
	            source += text.slice(index, offset).replace(escapeRegExp, escapeChar);
	            index = offset + match.length;
	
	            if (interpolate) {
	                source += "'+\n((__t=(" + interpolate + "))==null?'':__t)+\n'";
	            } else if (evaluate) {
	                source += "';\n" + evaluate + "\n__p+='";
	            }
	
	            return match;
	        });
	
	        source += "';\n";
	        source = 'with(obj||{}){\n' + source + '}\n';
	        source = "var __t,__p='',__j=Array.prototype.join," +
	            "print=function(){__p+=__j.call(arguments,'');};\n" +
	            source + 'return __p;\n';
	
	        var render;
	
	        try {
	            render = new Function('obj', source);
	        } catch (e) {
	            e.source = source;
	            throw e;
	        }
	
	        var _template = function(data) {
	            return render.call(this, data);
	        };
	
	        _template.source = 'function(obj){\n' + source + '}';
	
	        return _template;
	    };
	
	    var Autocomplete = function (el, options) {
	        this.options = $.extend(true, {}, Autocomplete.defaults, options);
	        this.$el = $(el);
	        this.$wrapper = this.$el.parent();
	        this.compiled = {};
	        this.$dropdown = null;
	        this.$appender = null;
	        this.$hidden = null;
	        this.resultCache = {};
	        this.value = '';
	        this.initialize();
	    };
	
	    Autocomplete.defaults = {
	        cacheable: true,
	        limit: 10,
	        multiple: {
	            enable: false,
	            maxSize: 4,
	            onExist: function (item) {
	                Materialize.toast('Tag: ' + item.text + '(' + item.id + ') is already added!', 2000);
	            },
	            onExceed: function (maxSize, item) {
	                Materialize.toast('Can\'t add over ' + maxSize + ' tags!', 2000);
	            },
	            onAppend: function (item) {
	                var self = this;
	                self.$el.removeClass('active');
	                self.$el.click();
	            },
	            onRemove: function (item) {
	                var self = this;
	                self.$el.removeClass('active');
	                self.$el.click();
	            }
	        },
	        hidden: {
	            enable: true,
	            el: '',
	            inputName: '',
	            required: false
	        },
	        appender: {
	            el: '',
	            tagName: 'ul',
	            className: 'ac-appender',
	            tagTemplate: '<div class="chip" data-id="<%= item.id %>" data-text="<% item.text %>"><%= item.text %>(<%= item.id %>)<i class="material-icons close">close</i></div>'
	        },
	        dropdown: {
	            el: '',
	            tagName: 'ul',
	            className: 'ac-dropdown',
	            itemTemplate: '<li class="ac-item" data-id="<%= item.id %>" data-text="<%= item.text %>"><a href="javascript:void(0)"><%= item.text %></a></li>',
	            noItem: ''
	        },
	        handlers: {
	            'setValue': '.ac-dropdown .ac-item',
	            '.ac-appender .ac-tag .close': 'remove'
	        },
	        getData: function (value, callback) {
	            callback(value, []);
	        },
	        ignoreCase: true,
	        throttling: true
	    };
	
	    Autocomplete.prototype = {
	        constructor: Autocomplete,
	        initialize: function () {
	            var self = this;
	            var timer;
	            var fetching = false;
	
	            function getItemsHtml (list) {
	                var itemsHtml = '';
	
	                if (!list.length) {
	                    return self.options.dropdown.noItem;
	                }
	
	                list.forEach(function (item, idx) {
	
	                    if (idx >= self.options.limit) {
	                        return false;
	                    }
	
	                    itemsHtml += self.compiled.item({ 'item': item});
	                });
	
	                return itemsHtml;
	            }
	
	            function handleList (value, list) {
	                var itemsHtml = getItemsHtml(list);
	                var currentValue = self.$el.val();
	
	                if (self.options.ignoreCase) {
	                    currentValue = currentValue.toUpperCase();
	                }
	
	                if (self.options.cacheable && !self.resultCache.hasOwnProperty(value)) {
	                    self.resultCache[value] = list;
	                }
	
	                if (value !== currentValue) {
	                    return false;
	                }
	
	                if(itemsHtml) {
	                    self.$dropdown.html(itemsHtml);
	                    self.$dropdown.show();
	                } else {
	                    self.$dropdown.hide();
	                }
	
	            }
	
	            self.value = self.options.multiple.enable ? [] : '';
	
	            self.compiled.tag = template(self.options.appender.tagTemplate);
	            self.compiled.item = template(self.options.dropdown.itemTemplate);
	
	            self.render();
	
	            self.$el.on('input', function (e) {
	                var $t = $(this);
	                var value = $t.val();
	
	                if (!value) {
	                    self.$dropdown.hide();
	                    return false;
	                }
	
	                if (self.options.ignoreCase) {
	                    value = value.toUpperCase();
	                }
	
	                if (self.resultCache.hasOwnProperty(value) && self.resultCache[value]) {
	                    handleList(value, self.resultCache[value]);
	                    return true;
	                }
	
	                if (self.options.throttling) {
	                    clearTimeout(timer);
	                    timer = setTimeout(function () {
	                        self.options.getData(value, handleList);
	                    }, 200);
	                    return true;
	                }
	
	                self.options.getData(value, handleList);
	            });
	
	            self.$el.on('keydown', function (e) {
	                var $t = $(this);
	                var keyCode = e.keyCode;
	                var $items, $hover;
	                // BACKSPACE KEY
	                if (keyCode == '8' && !$t.val()) {
	
	                    if (!self.options.multiple.enable) {
	                        return true;
	                    }
	
	                    if (!self.value.length) {
	                        return true;
	                    }
	
	                    var lastItem = self.value[self.value.length - 1];
	                    self.remove(lastItem);
	                    return false;
	                }
	                // UP DOWN ARROW KEY
	                if (keyCode == '38' || keyCode == '40') {
	
	                    $items = self.$dropdown.find('[data-id]');
	
	                    if (!$items.length) {
	                        return false;
	                    }
	
	                    $hover = $items.filter('.ac-hover');
	
	                    if (!$hover.length) {
	                        $items.removeClass('ac-hover');
	                        $items.eq(keyCode == '40' ? 0 : -1).addClass('ac-hover');
	                    } else {
	                        var index = $hover.index();
	                        $items.removeClass('ac-hover');
	                        $items.eq(keyCode == '40' ? (index + 1) % $items.length : index - 1).addClass('ac-hover');
	                    }
	
	                    return false;
	                }
	                // ENTER KEY CODE
	                if (keyCode == '13') {
	                    $items = self.$dropdown.find('[data-id]');
	
	                    if (!$items.length) {
	                        return false;
	                    }
	
	                    $hover = $items.filter('.ac-hover');
	
	                    if (!$hover.length) {
	                        return false;
	                    }
	
	                    self.setValue({
	                        id: $hover.data('id'),
	                        text: $hover.data('text')
	                    });
	
	                    return false;
	                }
	
	            });
	
	            self.$dropdown.on('click', '[data-id]', function (e) {
	                var $t = $(this);
	                var item = {
	                    id: $t.data('id'),
	                    text: $t.data('text')
	                };
	
	                self.setValue(item);
	            });
	
	            self.$appender.on('click', '[data-id] .close', function (e) {
	                var $t = $(this);
	                var $li = $t.closest('[data-id]');
	                var item = {
	                    id: $li.data('id'),
	                    text: $li.data('text')
	                };
	
	                self.remove(item);
	            });
	
	        },
	        render: function () {
	            var self = this;
	
	            if (self.options.dropdown.el) {
	                self.$dropdown = $(self.options.dropdown.el);
	            } else {
	                self.$dropdown = $(document.createElement(self.options.dropdown.tagName));
	                self.$dropdown.insertAfter(self.$el);
	            }
	
	            self.$dropdown.addClass(self.options.dropdown.className);
	
	            if (self.options.appender.el) {
	                self.$appender = $(self.options.appender.el);
	            } else {
	                self.$appender = $(document.createElement(self.options.appender.tagName));
	                self.$appender.insertBefore(self.$el);
	            }
	
	            if (self.options.hidden.enable) {
	
	                if (self.options.hidden.el) {
	                    self.$hidden = $(self.options.hidden.el);
	                } else {
	                    self.$hidden = $('<input type="hidden" class="validate" />');
	                    self.$wrapper.append(self.$hidden);
	                }
	
	                if (self.options.hidden.inputName) {
	                    self.$el.attr('name', self.options.hidden.inputName);
	                }
	
	                if (self.options.hidden.required) {
	                    self.$hidden.attr('required', 'required');
	                }
	
	            }
	
	            self.$appender.addClass(self.options.appender.className);
	
	        },
	        setValue: function (item) {
	            var self = this;
	
	            if (self.options.multiple.enable) {
	                self.append(item);
	            } else {
	                self.select(item);
	            }
	
	        },
	        append: function (item) {
	            var self = this;
	            var $tag = self.compiled.tag({ 'item': item });
	
	            if (self.value.some(function (selectedItem) {
	                    return selectedItem.id === item.id;
	                })) {
	
	                if ('function' === typeof self.options.multiple.onExist) {
	                    self.options.multiple.onExist.call(this, item);
	                }
	
	                return false;
	            }
	
	            if (self.value.length >= self.options.multiple.maxSize) {
	
	                if ('function' === typeof self.options.multiple.onExceed) {
	                    self.options.multiple.onExceed.call(this, self.options.multiple.maxSize);
	                }
	
	                return false;
	            }
	
	            self.value.push(item);
	            self.$appender.append($tag);
	
	            var valueStr = self.value.map(function (selectedItem) {
	                return selectedItem.id;
	            }).join(',');
	
	            if (self.options.hidden.enable) {
	                self.$hidden.val(valueStr);
	            }
	
	            self.$el.val('');
	            self.$el.data('value', valueStr);
	            self.$dropdown.html('').hide();
	
	            if ('function' === typeof self.options.multiple.onAppend) {
	                self.options.multiple.onAppend.call(self, item);
	            }
	
	        },
	        remove: function (item) {
	            var self = this;
	
	            self.$appender.find('[data-id="' + item.id + '"]').remove();
	            self.value = self.value.filter(function (selectedItem) {
	                return selectedItem.id !== item.id;
	            });
	
	            var valueStr = self.value.map(function (selectedItem) {
	                return selectedItem.id;
	            }).join(',');
	
	            if (self.options.hidden.enable) {
	                self.$hidden.val(valueStr);
	                self.$el.data('value', valueStr);
	            }
	
	            self.$dropdown.html('').hide();
	
	            if ('function' === typeof self.options.multiple.onRemove) {
	                self.options.multiple.onRemove.call(self, item);
	            }
	
	        },
	        select: function (item) {
	            var self = this;
	
	            self.value = item.text;
	            self.$el.val(item.text);
	            self.$el.data('value', item.id);
	            self.$dropdown.html('').hide();
	
	            if (self.options.hidden.enable) {
	                self.$hidden.val(item.id);
	            }
	        }
	    };
	
	    $.fn.materialize_autocomplete = function (options) {
	        var el = this;
	        var $el = $(el).eq(0);
	        var instance = $el.data('autocomplete');
	
	        if (instance && arguments.length) {
	            return instance;
	        }
	
	        var autocomplete = new Autocomplete(el, options);
	        $el.data('autocomplete', autocomplete);
	        $el.dropdown();
	        return autocomplete;
	    };
	
	}));


/***/ },

/***/ 215:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;var require;var require;/* WEBPACK VAR INJECTION */(function($) {/*!
	 * Select2 4.0.3
	 * https://select2.github.io
	 *
	 * Released under the MIT license
	 * https://github.com/select2/select2/blob/master/LICENSE.md
	 */
	(function (factory) {
	  if (true) {
	    // AMD. Register as an anonymous module.
	    !(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (factory), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	  } else if (typeof module === 'object' && module.exports) {
	    // Node/CommonJS
	    module.exports = function (root, jQuery) {
	      if (jQuery === undefined) {
	        // require('jQuery') returns a factory that requires window to
	        // build a jQuery instance, we normalize how we use modules
	        // that require this pattern but the window provided is a noop
	        // if it's defined (how jquery works)
	        if (typeof window !== 'undefined') {
	          jQuery = require('jquery');
	        }
	        else {
	          jQuery = require('jquery')(root);
	        }
	      }
	      factory(jQuery);
	      return jQuery;
	    };
	  } else {
	    // Browser globals
	    factory(jQuery);
	  }
	} (function (jQuery) {
	  // This is needed so we can catch the AMD loader configuration and use it
	  // The inner file should be wrapped (by `banner.start.js`) in a function that
	  // returns the AMD loader references.
	  var S2 =(function () {
	  // Restore the Select2 AMD loader so it can be used
	  // Needed mostly in the language files, where the loader is not inserted
	  if (jQuery && jQuery.fn && jQuery.fn.select2 && jQuery.fn.select2.amd) {
	    var S2 = jQuery.fn.select2.amd;
	  }
	var S2;(function () { if (!S2 || !S2.requirejs) {
	if (!S2) { S2 = {}; } else { require = S2; }
	/**
	 * @license almond 0.3.1 Copyright (c) 2011-2014, The Dojo Foundation All Rights Reserved.
	 * Available via the MIT or new BSD license.
	 * see: http://github.com/jrburke/almond for details
	 */
	//Going sloppy to avoid 'use strict' string cost, but strict practices should
	//be followed.
	/*jslint sloppy: true */
	/*global setTimeout: false */
	
	var requirejs, require, define;
	(function (undef) {
	    var main, req, makeMap, handlers,
	        defined = {},
	        waiting = {},
	        config = {},
	        defining = {},
	        hasOwn = Object.prototype.hasOwnProperty,
	        aps = [].slice,
	        jsSuffixRegExp = /\.js$/;
	
	    function hasProp(obj, prop) {
	        return hasOwn.call(obj, prop);
	    }
	
	    /**
	     * Given a relative module name, like ./something, normalize it to
	     * a real name that can be mapped to a path.
	     * @param {String} name the relative name
	     * @param {String} baseName a real name that the name arg is relative
	     * to.
	     * @returns {String} normalized name
	     */
	    function normalize(name, baseName) {
	        var nameParts, nameSegment, mapValue, foundMap, lastIndex,
	            foundI, foundStarMap, starI, i, j, part,
	            baseParts = baseName && baseName.split("/"),
	            map = config.map,
	            starMap = (map && map['*']) || {};
	
	        //Adjust any relative paths.
	        if (name && name.charAt(0) === ".") {
	            //If have a base name, try to normalize against it,
	            //otherwise, assume it is a top-level require that will
	            //be relative to baseUrl in the end.
	            if (baseName) {
	                name = name.split('/');
	                lastIndex = name.length - 1;
	
	                // Node .js allowance:
	                if (config.nodeIdCompat && jsSuffixRegExp.test(name[lastIndex])) {
	                    name[lastIndex] = name[lastIndex].replace(jsSuffixRegExp, '');
	                }
	
	                //Lop off the last part of baseParts, so that . matches the
	                //"directory" and not name of the baseName's module. For instance,
	                //baseName of "one/two/three", maps to "one/two/three.js", but we
	                //want the directory, "one/two" for this normalization.
	                name = baseParts.slice(0, baseParts.length - 1).concat(name);
	
	                //start trimDots
	                for (i = 0; i < name.length; i += 1) {
	                    part = name[i];
	                    if (part === ".") {
	                        name.splice(i, 1);
	                        i -= 1;
	                    } else if (part === "..") {
	                        if (i === 1 && (name[2] === '..' || name[0] === '..')) {
	                            //End of the line. Keep at least one non-dot
	                            //path segment at the front so it can be mapped
	                            //correctly to disk. Otherwise, there is likely
	                            //no path mapping for a path starting with '..'.
	                            //This can still fail, but catches the most reasonable
	                            //uses of ..
	                            break;
	                        } else if (i > 0) {
	                            name.splice(i - 1, 2);
	                            i -= 2;
	                        }
	                    }
	                }
	                //end trimDots
	
	                name = name.join("/");
	            } else if (name.indexOf('./') === 0) {
	                // No baseName, so this is ID is resolved relative
	                // to baseUrl, pull off the leading dot.
	                name = name.substring(2);
	            }
	        }
	
	        //Apply map config if available.
	        if ((baseParts || starMap) && map) {
	            nameParts = name.split('/');
	
	            for (i = nameParts.length; i > 0; i -= 1) {
	                nameSegment = nameParts.slice(0, i).join("/");
	
	                if (baseParts) {
	                    //Find the longest baseName segment match in the config.
	                    //So, do joins on the biggest to smallest lengths of baseParts.
	                    for (j = baseParts.length; j > 0; j -= 1) {
	                        mapValue = map[baseParts.slice(0, j).join('/')];
	
	                        //baseName segment has  config, find if it has one for
	                        //this name.
	                        if (mapValue) {
	                            mapValue = mapValue[nameSegment];
	                            if (mapValue) {
	                                //Match, update name to the new value.
	                                foundMap = mapValue;
	                                foundI = i;
	                                break;
	                            }
	                        }
	                    }
	                }
	
	                if (foundMap) {
	                    break;
	                }
	
	                //Check for a star map match, but just hold on to it,
	                //if there is a shorter segment match later in a matching
	                //config, then favor over this star map.
	                if (!foundStarMap && starMap && starMap[nameSegment]) {
	                    foundStarMap = starMap[nameSegment];
	                    starI = i;
	                }
	            }
	
	            if (!foundMap && foundStarMap) {
	                foundMap = foundStarMap;
	                foundI = starI;
	            }
	
	            if (foundMap) {
	                nameParts.splice(0, foundI, foundMap);
	                name = nameParts.join('/');
	            }
	        }
	
	        return name;
	    }
	
	    function makeRequire(relName, forceSync) {
	        return function () {
	            //A version of a require function that passes a moduleName
	            //value for items that may need to
	            //look up paths relative to the moduleName
	            var args = aps.call(arguments, 0);
	
	            //If first arg is not require('string'), and there is only
	            //one arg, it is the array form without a callback. Insert
	            //a null so that the following concat is correct.
	            if (typeof args[0] !== 'string' && args.length === 1) {
	                args.push(null);
	            }
	            return req.apply(undef, args.concat([relName, forceSync]));
	        };
	    }
	
	    function makeNormalize(relName) {
	        return function (name) {
	            return normalize(name, relName);
	        };
	    }
	
	    function makeLoad(depName) {
	        return function (value) {
	            defined[depName] = value;
	        };
	    }
	
	    function callDep(name) {
	        if (hasProp(waiting, name)) {
	            var args = waiting[name];
	            delete waiting[name];
	            defining[name] = true;
	            main.apply(undef, args);
	        }
	
	        if (!hasProp(defined, name) && !hasProp(defining, name)) {
	            throw new Error('No ' + name);
	        }
	        return defined[name];
	    }
	
	    //Turns a plugin!resource to [plugin, resource]
	    //with the plugin being undefined if the name
	    //did not have a plugin prefix.
	    function splitPrefix(name) {
	        var prefix,
	            index = name ? name.indexOf('!') : -1;
	        if (index > -1) {
	            prefix = name.substring(0, index);
	            name = name.substring(index + 1, name.length);
	        }
	        return [prefix, name];
	    }
	
	    /**
	     * Makes a name map, normalizing the name, and using a plugin
	     * for normalization if necessary. Grabs a ref to plugin
	     * too, as an optimization.
	     */
	    makeMap = function (name, relName) {
	        var plugin,
	            parts = splitPrefix(name),
	            prefix = parts[0];
	
	        name = parts[1];
	
	        if (prefix) {
	            prefix = normalize(prefix, relName);
	            plugin = callDep(prefix);
	        }
	
	        //Normalize according
	        if (prefix) {
	            if (plugin && plugin.normalize) {
	                name = plugin.normalize(name, makeNormalize(relName));
	            } else {
	                name = normalize(name, relName);
	            }
	        } else {
	            name = normalize(name, relName);
	            parts = splitPrefix(name);
	            prefix = parts[0];
	            name = parts[1];
	            if (prefix) {
	                plugin = callDep(prefix);
	            }
	        }
	
	        //Using ridiculous property names for space reasons
	        return {
	            f: prefix ? prefix + '!' + name : name, //fullName
	            n: name,
	            pr: prefix,
	            p: plugin
	        };
	    };
	
	    function makeConfig(name) {
	        return function () {
	            return (config && config.config && config.config[name]) || {};
	        };
	    }
	
	    handlers = {
	        require: function (name) {
	            return makeRequire(name);
	        },
	        exports: function (name) {
	            var e = defined[name];
	            if (typeof e !== 'undefined') {
	                return e;
	            } else {
	                return (defined[name] = {});
	            }
	        },
	        module: function (name) {
	            return {
	                id: name,
	                uri: '',
	                exports: defined[name],
	                config: makeConfig(name)
	            };
	        }
	    };
	
	    main = function (name, deps, callback, relName) {
	        var cjsModule, depName, ret, map, i,
	            args = [],
	            callbackType = typeof callback,
	            usingExports;
	
	        //Use name if no relName
	        relName = relName || name;
	
	        //Call the callback to define the module, if necessary.
	        if (callbackType === 'undefined' || callbackType === 'function') {
	            //Pull out the defined dependencies and pass the ordered
	            //values to the callback.
	            //Default to [require, exports, module] if no deps
	            deps = !deps.length && callback.length ? ['require', 'exports', 'module'] : deps;
	            for (i = 0; i < deps.length; i += 1) {
	                map = makeMap(deps[i], relName);
	                depName = map.f;
	
	                //Fast path CommonJS standard dependencies.
	                if (depName === "require") {
	                    args[i] = handlers.require(name);
	                } else if (depName === "exports") {
	                    //CommonJS module spec 1.1
	                    args[i] = handlers.exports(name);
	                    usingExports = true;
	                } else if (depName === "module") {
	                    //CommonJS module spec 1.1
	                    cjsModule = args[i] = handlers.module(name);
	                } else if (hasProp(defined, depName) ||
	                           hasProp(waiting, depName) ||
	                           hasProp(defining, depName)) {
	                    args[i] = callDep(depName);
	                } else if (map.p) {
	                    map.p.load(map.n, makeRequire(relName, true), makeLoad(depName), {});
	                    args[i] = defined[depName];
	                } else {
	                    throw new Error(name + ' missing ' + depName);
	                }
	            }
	
	            ret = callback ? callback.apply(defined[name], args) : undefined;
	
	            if (name) {
	                //If setting exports via "module" is in play,
	                //favor that over return value and exports. After that,
	                //favor a non-undefined return value over exports use.
	                if (cjsModule && cjsModule.exports !== undef &&
	                        cjsModule.exports !== defined[name]) {
	                    defined[name] = cjsModule.exports;
	                } else if (ret !== undef || !usingExports) {
	                    //Use the return value from the function.
	                    defined[name] = ret;
	                }
	            }
	        } else if (name) {
	            //May just be an object definition for the module. Only
	            //worry about defining if have a module name.
	            defined[name] = callback;
	        }
	    };
	
	    requirejs = require = req = function (deps, callback, relName, forceSync, alt) {
	        if (typeof deps === "string") {
	            if (handlers[deps]) {
	                //callback in this case is really relName
	                return handlers[deps](callback);
	            }
	            //Just return the module wanted. In this scenario, the
	            //deps arg is the module name, and second arg (if passed)
	            //is just the relName.
	            //Normalize module name, if it contains . or ..
	            return callDep(makeMap(deps, callback).f);
	        } else if (!deps.splice) {
	            //deps is a config object, not an array.
	            config = deps;
	            if (config.deps) {
	                req(config.deps, config.callback);
	            }
	            if (!callback) {
	                return;
	            }
	
	            if (callback.splice) {
	                //callback is an array, which means it is a dependency list.
	                //Adjust args if there are dependencies
	                deps = callback;
	                callback = relName;
	                relName = null;
	            } else {
	                deps = undef;
	            }
	        }
	
	        //Support require(['a'])
	        callback = callback || function () {};
	
	        //If relName is a function, it is an errback handler,
	        //so remove it.
	        if (typeof relName === 'function') {
	            relName = forceSync;
	            forceSync = alt;
	        }
	
	        //Simulate async callback;
	        if (forceSync) {
	            main(undef, deps, callback, relName);
	        } else {
	            //Using a non-zero value because of concern for what old browsers
	            //do, and latest browsers "upgrade" to 4 if lower value is used:
	            //http://www.whatwg.org/specs/web-apps/current-work/multipage/timers.html#dom-windowtimers-settimeout:
	            //If want a value immediately, use require('id') instead -- something
	            //that works in almond on the global level, but not guaranteed and
	            //unlikely to work in other AMD implementations.
	            setTimeout(function () {
	                main(undef, deps, callback, relName);
	            }, 4);
	        }
	
	        return req;
	    };
	
	    /**
	     * Just drops the config on the floor, but returns req in case
	     * the config return value is used.
	     */
	    req.config = function (cfg) {
	        return req(cfg);
	    };
	
	    /**
	     * Expose module registry for debugging and tooling
	     */
	    requirejs._defined = defined;
	
	    define = function (name, deps, callback) {
	        if (typeof name !== 'string') {
	            throw new Error('See almond README: incorrect module build, no module name');
	        }
	
	        //This module may not have dependencies
	        if (!deps.splice) {
	            //deps is not an array, so probably means
	            //an object literal or factory function for
	            //the value. Adjust args.
	            callback = deps;
	            deps = [];
	        }
	
	        if (!hasProp(defined, name) && !hasProp(waiting, name)) {
	            waiting[name] = [name, deps, callback];
	        }
	    };
	
	    define.amd = {
	        jQuery: true
	    };
	}());
	
	S2.requirejs = requirejs;S2.require = require;S2.define = define;
	}
	}());
	S2.define("almond", function(){});
	
	/* global jQuery:false, $:false */
	S2.define('jquery',[],function () {
	  var _$ = jQuery || $;
	
	  if (_$ == null && console && console.error) {
	    console.error(
	      'Select2: An instance of jQuery or a jQuery-compatible library was not ' +
	      'found. Make sure that you are including jQuery before Select2 on your ' +
	      'web page.'
	    );
	  }
	
	  return _$;
	});
	
	S2.define('select2/utils',[
	  'jquery'
	], function ($) {
	  var Utils = {};
	
	  Utils.Extend = function (ChildClass, SuperClass) {
	    var __hasProp = {}.hasOwnProperty;
	
	    function BaseConstructor () {
	      this.constructor = ChildClass;
	    }
	
	    for (var key in SuperClass) {
	      if (__hasProp.call(SuperClass, key)) {
	        ChildClass[key] = SuperClass[key];
	      }
	    }
	
	    BaseConstructor.prototype = SuperClass.prototype;
	    ChildClass.prototype = new BaseConstructor();
	    ChildClass.__super__ = SuperClass.prototype;
	
	    return ChildClass;
	  };
	
	  function getMethods (theClass) {
	    var proto = theClass.prototype;
	
	    var methods = [];
	
	    for (var methodName in proto) {
	      var m = proto[methodName];
	
	      if (typeof m !== 'function') {
	        continue;
	      }
	
	      if (methodName === 'constructor') {
	        continue;
	      }
	
	      methods.push(methodName);
	    }
	
	    return methods;
	  }
	
	  Utils.Decorate = function (SuperClass, DecoratorClass) {
	    var decoratedMethods = getMethods(DecoratorClass);
	    var superMethods = getMethods(SuperClass);
	
	    function DecoratedClass () {
	      var unshift = Array.prototype.unshift;
	
	      var argCount = DecoratorClass.prototype.constructor.length;
	
	      var calledConstructor = SuperClass.prototype.constructor;
	
	      if (argCount > 0) {
	        unshift.call(arguments, SuperClass.prototype.constructor);
	
	        calledConstructor = DecoratorClass.prototype.constructor;
	      }
	
	      calledConstructor.apply(this, arguments);
	    }
	
	    DecoratorClass.displayName = SuperClass.displayName;
	
	    function ctr () {
	      this.constructor = DecoratedClass;
	    }
	
	    DecoratedClass.prototype = new ctr();
	
	    for (var m = 0; m < superMethods.length; m++) {
	        var superMethod = superMethods[m];
	
	        DecoratedClass.prototype[superMethod] =
	          SuperClass.prototype[superMethod];
	    }
	
	    var calledMethod = function (methodName) {
	      // Stub out the original method if it's not decorating an actual method
	      var originalMethod = function () {};
	
	      if (methodName in DecoratedClass.prototype) {
	        originalMethod = DecoratedClass.prototype[methodName];
	      }
	
	      var decoratedMethod = DecoratorClass.prototype[methodName];
	
	      return function () {
	        var unshift = Array.prototype.unshift;
	
	        unshift.call(arguments, originalMethod);
	
	        return decoratedMethod.apply(this, arguments);
	      };
	    };
	
	    for (var d = 0; d < decoratedMethods.length; d++) {
	      var decoratedMethod = decoratedMethods[d];
	
	      DecoratedClass.prototype[decoratedMethod] = calledMethod(decoratedMethod);
	    }
	
	    return DecoratedClass;
	  };
	
	  var Observable = function () {
	    this.listeners = {};
	  };
	
	  Observable.prototype.on = function (event, callback) {
	    this.listeners = this.listeners || {};
	
	    if (event in this.listeners) {
	      this.listeners[event].push(callback);
	    } else {
	      this.listeners[event] = [callback];
	    }
	  };
	
	  Observable.prototype.trigger = function (event) {
	    var slice = Array.prototype.slice;
	    var params = slice.call(arguments, 1);
	
	    this.listeners = this.listeners || {};
	
	    // Params should always come in as an array
	    if (params == null) {
	      params = [];
	    }
	
	    // If there are no arguments to the event, use a temporary object
	    if (params.length === 0) {
	      params.push({});
	    }
	
	    // Set the `_type` of the first object to the event
	    params[0]._type = event;
	
	    if (event in this.listeners) {
	      this.invoke(this.listeners[event], slice.call(arguments, 1));
	    }
	
	    if ('*' in this.listeners) {
	      this.invoke(this.listeners['*'], arguments);
	    }
	  };
	
	  Observable.prototype.invoke = function (listeners, params) {
	    for (var i = 0, len = listeners.length; i < len; i++) {
	      listeners[i].apply(this, params);
	    }
	  };
	
	  Utils.Observable = Observable;
	
	  Utils.generateChars = function (length) {
	    var chars = '';
	
	    for (var i = 0; i < length; i++) {
	      var randomChar = Math.floor(Math.random() * 36);
	      chars += randomChar.toString(36);
	    }
	
	    return chars;
	  };
	
	  Utils.bind = function (func, context) {
	    return function () {
	      func.apply(context, arguments);
	    };
	  };
	
	  Utils._convertData = function (data) {
	    for (var originalKey in data) {
	      var keys = originalKey.split('-');
	
	      var dataLevel = data;
	
	      if (keys.length === 1) {
	        continue;
	      }
	
	      for (var k = 0; k < keys.length; k++) {
	        var key = keys[k];
	
	        // Lowercase the first letter
	        // By default, dash-separated becomes camelCase
	        key = key.substring(0, 1).toLowerCase() + key.substring(1);
	
	        if (!(key in dataLevel)) {
	          dataLevel[key] = {};
	        }
	
	        if (k == keys.length - 1) {
	          dataLevel[key] = data[originalKey];
	        }
	
	        dataLevel = dataLevel[key];
	      }
	
	      delete data[originalKey];
	    }
	
	    return data;
	  };
	
	  Utils.hasScroll = function (index, el) {
	    // Adapted from the function created by @ShadowScripter
	    // and adapted by @BillBarry on the Stack Exchange Code Review website.
	    // The original code can be found at
	    // http://codereview.stackexchange.com/q/13338
	    // and was designed to be used with the Sizzle selector engine.
	
	    var $el = $(el);
	    var overflowX = el.style.overflowX;
	    var overflowY = el.style.overflowY;
	
	    //Check both x and y declarations
	    if (overflowX === overflowY &&
	        (overflowY === 'hidden' || overflowY === 'visible')) {
	      return false;
	    }
	
	    if (overflowX === 'scroll' || overflowY === 'scroll') {
	      return true;
	    }
	
	    return ($el.innerHeight() < el.scrollHeight ||
	      $el.innerWidth() < el.scrollWidth);
	  };
	
	  Utils.escapeMarkup = function (markup) {
	    var replaceMap = {
	      '\\': '&#92;',
	      '&': '&amp;',
	      '<': '&lt;',
	      '>': '&gt;',
	      '"': '&quot;',
	      '\'': '&#39;',
	      '/': '&#47;'
	    };
	
	    // Do not try to escape the markup if it's not a string
	    if (typeof markup !== 'string') {
	      return markup;
	    }
	
	    return String(markup).replace(/[&<>"'\/\\]/g, function (match) {
	      return replaceMap[match];
	    });
	  };
	
	  // Append an array of jQuery nodes to a given element.
	  Utils.appendMany = function ($element, $nodes) {
	    // jQuery 1.7.x does not support $.fn.append() with an array
	    // Fall back to a jQuery object collection using $.fn.add()
	    if ($.fn.jquery.substr(0, 3) === '1.7') {
	      var $jqNodes = $();
	
	      $.map($nodes, function (node) {
	        $jqNodes = $jqNodes.add(node);
	      });
	
	      $nodes = $jqNodes;
	    }
	
	    $element.append($nodes);
	  };
	
	  return Utils;
	});
	
	S2.define('select2/results',[
	  'jquery',
	  './utils'
	], function ($, Utils) {
	  function Results ($element, options, dataAdapter) {
	    this.$element = $element;
	    this.data = dataAdapter;
	    this.options = options;
	
	    Results.__super__.constructor.call(this);
	  }
	
	  Utils.Extend(Results, Utils.Observable);
	
	  Results.prototype.render = function () {
	    var $results = $(
	      '<ul class="select2-results__options" role="tree"></ul>'
	    );
	
	    if (this.options.get('multiple')) {
	      $results.attr('aria-multiselectable', 'true');
	    }
	
	    this.$results = $results;
	
	    return $results;
	  };
	
	  Results.prototype.clear = function () {
	    this.$results.empty();
	  };
	
	  Results.prototype.displayMessage = function (params) {
	    var escapeMarkup = this.options.get('escapeMarkup');
	
	    this.clear();
	    this.hideLoading();
	
	    var $message = $(
	      '<li role="treeitem" aria-live="assertive"' +
	      ' class="select2-results__option"></li>'
	    );
	
	    var message = this.options.get('translations').get(params.message);
	
	    $message.append(
	      escapeMarkup(
	        message(params.args)
	      )
	    );
	
	    $message[0].className += ' select2-results__message';
	
	    this.$results.append($message);
	  };
	
	  Results.prototype.hideMessages = function () {
	    this.$results.find('.select2-results__message').remove();
	  };
	
	  Results.prototype.append = function (data) {
	    this.hideLoading();
	
	    var $options = [];
	
	    if (data.results == null || data.results.length === 0) {
	      if (this.$results.children().length === 0) {
	        this.trigger('results:message', {
	          message: 'noResults'
	        });
	      }
	
	      return;
	    }
	
	    data.results = this.sort(data.results);
	
	    for (var d = 0; d < data.results.length; d++) {
	      var item = data.results[d];
	
	      var $option = this.option(item);
	
	      $options.push($option);
	    }
	
	    this.$results.append($options);
	  };
	
	  Results.prototype.position = function ($results, $dropdown) {
	    var $resultsContainer = $dropdown.find('.select2-results');
	    $resultsContainer.append($results);
	  };
	
	  Results.prototype.sort = function (data) {
	    var sorter = this.options.get('sorter');
	
	    return sorter(data);
	  };
	
	  Results.prototype.highlightFirstItem = function () {
	    var $options = this.$results
	      .find('.select2-results__option[aria-selected]');
	
	    var $selected = $options.filter('[aria-selected=true]');
	
	    // Check if there are any selected options
	    if ($selected.length > 0) {
	      // If there are selected options, highlight the first
	      $selected.first().trigger('mouseenter');
	    } else {
	      // If there are no selected options, highlight the first option
	      // in the dropdown
	      $options.first().trigger('mouseenter');
	    }
	
	    this.ensureHighlightVisible();
	  };
	
	  Results.prototype.setClasses = function () {
	    var self = this;
	
	    this.data.current(function (selected) {
	      var selectedIds = $.map(selected, function (s) {
	        return s.id.toString();
	      });
	
	      var $options = self.$results
	        .find('.select2-results__option[aria-selected]');
	
	      $options.each(function () {
	        var $option = $(this);
	
	        var item = $.data(this, 'data');
	
	        // id needs to be converted to a string when comparing
	        var id = '' + item.id;
	
	        if ((item.element != null && item.element.selected) ||
	            (item.element == null && $.inArray(id, selectedIds) > -1)) {
	          $option.attr('aria-selected', 'true');
	        } else {
	          $option.attr('aria-selected', 'false');
	        }
	      });
	
	    });
	  };
	
	  Results.prototype.showLoading = function (params) {
	    this.hideLoading();
	
	    var loadingMore = this.options.get('translations').get('searching');
	
	    var loading = {
	      disabled: true,
	      loading: true,
	      text: loadingMore(params)
	    };
	    var $loading = this.option(loading);
	    $loading.className += ' loading-results';
	
	    this.$results.prepend($loading);
	  };
	
	  Results.prototype.hideLoading = function () {
	    this.$results.find('.loading-results').remove();
	  };
	
	  Results.prototype.option = function (data) {
	    var option = document.createElement('li');
	    option.className = 'select2-results__option';
	
	    var attrs = {
	      'role': 'treeitem',
	      'aria-selected': 'false'
	    };
	
	    if (data.disabled) {
	      delete attrs['aria-selected'];
	      attrs['aria-disabled'] = 'true';
	    }
	
	    if (data.id == null) {
	      delete attrs['aria-selected'];
	    }
	
	    if (data._resultId != null) {
	      option.id = data._resultId;
	    }
	
	    if (data.title) {
	      option.title = data.title;
	    }
	
	    if (data.children) {
	      attrs.role = 'group';
	      attrs['aria-label'] = data.text;
	      delete attrs['aria-selected'];
	    }
	
	    for (var attr in attrs) {
	      var val = attrs[attr];
	
	      option.setAttribute(attr, val);
	    }
	
	    if (data.children) {
	      var $option = $(option);
	
	      var label = document.createElement('strong');
	      label.className = 'select2-results__group';
	
	      var $label = $(label);
	      this.template(data, label);
	
	      var $children = [];
	
	      for (var c = 0; c < data.children.length; c++) {
	        var child = data.children[c];
	
	        var $child = this.option(child);
	
	        $children.push($child);
	      }
	
	      var $childrenContainer = $('<ul></ul>', {
	        'class': 'select2-results__options select2-results__options--nested'
	      });
	
	      $childrenContainer.append($children);
	
	      $option.append(label);
	      $option.append($childrenContainer);
	    } else {
	      this.template(data, option);
	    }
	
	    $.data(option, 'data', data);
	
	    return option;
	  };
	
	  Results.prototype.bind = function (container, $container) {
	    var self = this;
	
	    var id = container.id + '-results';
	
	    this.$results.attr('id', id);
	
	    container.on('results:all', function (params) {
	      self.clear();
	      self.append(params.data);
	
	      if (container.isOpen()) {
	        self.setClasses();
	        self.highlightFirstItem();
	      }
	    });
	
	    container.on('results:append', function (params) {
	      self.append(params.data);
	
	      if (container.isOpen()) {
	        self.setClasses();
	      }
	    });
	
	    container.on('query', function (params) {
	      self.hideMessages();
	      self.showLoading(params);
	    });
	
	    container.on('select', function () {
	      if (!container.isOpen()) {
	        return;
	      }
	
	      self.setClasses();
	      self.highlightFirstItem();
	    });
	
	    container.on('unselect', function () {
	      if (!container.isOpen()) {
	        return;
	      }
	
	      self.setClasses();
	      self.highlightFirstItem();
	    });
	
	    container.on('open', function () {
	      // When the dropdown is open, aria-expended="true"
	      self.$results.attr('aria-expanded', 'true');
	      self.$results.attr('aria-hidden', 'false');
	
	      self.setClasses();
	      self.ensureHighlightVisible();
	    });
	
	    container.on('close', function () {
	      // When the dropdown is closed, aria-expended="false"
	      self.$results.attr('aria-expanded', 'false');
	      self.$results.attr('aria-hidden', 'true');
	      self.$results.removeAttr('aria-activedescendant');
	    });
	
	    container.on('results:toggle', function () {
	      var $highlighted = self.getHighlightedResults();
	
	      if ($highlighted.length === 0) {
	        return;
	      }
	
	      $highlighted.trigger('mouseup');
	    });
	
	    container.on('results:select', function () {
	      var $highlighted = self.getHighlightedResults();
	
	      if ($highlighted.length === 0) {
	        return;
	      }
	
	      var data = $highlighted.data('data');
	
	      if ($highlighted.attr('aria-selected') == 'true') {
	        self.trigger('close', {});
	      } else {
	        self.trigger('select', {
	          data: data
	        });
	      }
	    });
	
	    container.on('results:previous', function () {
	      var $highlighted = self.getHighlightedResults();
	
	      var $options = self.$results.find('[aria-selected]');
	
	      var currentIndex = $options.index($highlighted);
	
	      // If we are already at te top, don't move further
	      if (currentIndex === 0) {
	        return;
	      }
	
	      var nextIndex = currentIndex - 1;
	
	      // If none are highlighted, highlight the first
	      if ($highlighted.length === 0) {
	        nextIndex = 0;
	      }
	
	      var $next = $options.eq(nextIndex);
	
	      $next.trigger('mouseenter');
	
	      var currentOffset = self.$results.offset().top;
	      var nextTop = $next.offset().top;
	      var nextOffset = self.$results.scrollTop() + (nextTop - currentOffset);
	
	      if (nextIndex === 0) {
	        self.$results.scrollTop(0);
	      } else if (nextTop - currentOffset < 0) {
	        self.$results.scrollTop(nextOffset);
	      }
	    });
	
	    container.on('results:next', function () {
	      var $highlighted = self.getHighlightedResults();
	
	      var $options = self.$results.find('[aria-selected]');
	
	      var currentIndex = $options.index($highlighted);
	
	      var nextIndex = currentIndex + 1;
	
	      // If we are at the last option, stay there
	      if (nextIndex >= $options.length) {
	        return;
	      }
	
	      var $next = $options.eq(nextIndex);
	
	      $next.trigger('mouseenter');
	
	      var currentOffset = self.$results.offset().top +
	        self.$results.outerHeight(false);
	      var nextBottom = $next.offset().top + $next.outerHeight(false);
	      var nextOffset = self.$results.scrollTop() + nextBottom - currentOffset;
	
	      if (nextIndex === 0) {
	        self.$results.scrollTop(0);
	      } else if (nextBottom > currentOffset) {
	        self.$results.scrollTop(nextOffset);
	      }
	    });
	
	    container.on('results:focus', function (params) {
	      params.element.addClass('select2-results__option--highlighted');
	    });
	
	    container.on('results:message', function (params) {
	      self.displayMessage(params);
	    });
	
	    if ($.fn.mousewheel) {
	      this.$results.on('mousewheel', function (e) {
	        var top = self.$results.scrollTop();
	
	        var bottom = self.$results.get(0).scrollHeight - top + e.deltaY;
	
	        var isAtTop = e.deltaY > 0 && top - e.deltaY <= 0;
	        var isAtBottom = e.deltaY < 0 && bottom <= self.$results.height();
	
	        if (isAtTop) {
	          self.$results.scrollTop(0);
	
	          e.preventDefault();
	          e.stopPropagation();
	        } else if (isAtBottom) {
	          self.$results.scrollTop(
	            self.$results.get(0).scrollHeight - self.$results.height()
	          );
	
	          e.preventDefault();
	          e.stopPropagation();
	        }
	      });
	    }
	
	    this.$results.on('mouseup', '.select2-results__option[aria-selected]',
	      function (evt) {
	      var $this = $(this);
	
	      var data = $this.data('data');
	
	      if ($this.attr('aria-selected') === 'true') {
	        if (self.options.get('multiple')) {
	          self.trigger('unselect', {
	            originalEvent: evt,
	            data: data
	          });
	        } else {
	          self.trigger('close', {});
	        }
	
	        return;
	      }
	
	      self.trigger('select', {
	        originalEvent: evt,
	        data: data
	      });
	    });
	
	    this.$results.on('mouseenter', '.select2-results__option[aria-selected]',
	      function (evt) {
	      var data = $(this).data('data');
	
	      self.getHighlightedResults()
	          .removeClass('select2-results__option--highlighted');
	
	      self.trigger('results:focus', {
	        data: data,
	        element: $(this)
	      });
	    });
	  };
	
	  Results.prototype.getHighlightedResults = function () {
	    var $highlighted = this.$results
	    .find('.select2-results__option--highlighted');
	
	    return $highlighted;
	  };
	
	  Results.prototype.destroy = function () {
	    this.$results.remove();
	  };
	
	  Results.prototype.ensureHighlightVisible = function () {
	    var $highlighted = this.getHighlightedResults();
	
	    if ($highlighted.length === 0) {
	      return;
	    }
	
	    var $options = this.$results.find('[aria-selected]');
	
	    var currentIndex = $options.index($highlighted);
	
	    var currentOffset = this.$results.offset().top;
	    var nextTop = $highlighted.offset().top;
	    var nextOffset = this.$results.scrollTop() + (nextTop - currentOffset);
	
	    var offsetDelta = nextTop - currentOffset;
	    nextOffset -= $highlighted.outerHeight(false) * 2;
	
	    if (currentIndex <= 2) {
	      this.$results.scrollTop(0);
	    } else if (offsetDelta > this.$results.outerHeight() || offsetDelta < 0) {
	      this.$results.scrollTop(nextOffset);
	    }
	  };
	
	  Results.prototype.template = function (result, container) {
	    var template = this.options.get('templateResult');
	    var escapeMarkup = this.options.get('escapeMarkup');
	
	    var content = template(result, container);
	
	    if (content == null) {
	      container.style.display = 'none';
	    } else if (typeof content === 'string') {
	      container.innerHTML = escapeMarkup(content);
	    } else {
	      $(container).append(content);
	    }
	  };
	
	  return Results;
	});
	
	S2.define('select2/keys',[
	
	], function () {
	  var KEYS = {
	    BACKSPACE: 8,
	    TAB: 9,
	    ENTER: 13,
	    SHIFT: 16,
	    CTRL: 17,
	    ALT: 18,
	    ESC: 27,
	    SPACE: 32,
	    PAGE_UP: 33,
	    PAGE_DOWN: 34,
	    END: 35,
	    HOME: 36,
	    LEFT: 37,
	    UP: 38,
	    RIGHT: 39,
	    DOWN: 40,
	    DELETE: 46
	  };
	
	  return KEYS;
	});
	
	S2.define('select2/selection/base',[
	  'jquery',
	  '../utils',
	  '../keys'
	], function ($, Utils, KEYS) {
	  function BaseSelection ($element, options) {
	    this.$element = $element;
	    this.options = options;
	
	    BaseSelection.__super__.constructor.call(this);
	  }
	
	  Utils.Extend(BaseSelection, Utils.Observable);
	
	  BaseSelection.prototype.render = function () {
	    var $selection = $(
	      '<span class="select2-selection" role="combobox" ' +
	      ' aria-haspopup="true" aria-expanded="false">' +
	      '</span>'
	    );
	
	    this._tabindex = 0;
	
	    if (this.$element.data('old-tabindex') != null) {
	      this._tabindex = this.$element.data('old-tabindex');
	    } else if (this.$element.attr('tabindex') != null) {
	      this._tabindex = this.$element.attr('tabindex');
	    }
	
	    $selection.attr('title', this.$element.attr('title'));
	    $selection.attr('tabindex', this._tabindex);
	
	    this.$selection = $selection;
	
	    return $selection;
	  };
	
	  BaseSelection.prototype.bind = function (container, $container) {
	    var self = this;
	
	    var id = container.id + '-container';
	    var resultsId = container.id + '-results';
	
	    this.container = container;
	
	    this.$selection.on('focus', function (evt) {
	      self.trigger('focus', evt);
	    });
	
	    this.$selection.on('blur', function (evt) {
	      self._handleBlur(evt);
	    });
	
	    this.$selection.on('keydown', function (evt) {
	      self.trigger('keypress', evt);
	
	      if (evt.which === KEYS.SPACE) {
	        evt.preventDefault();
	      }
	    });
	
	    container.on('results:focus', function (params) {
	      self.$selection.attr('aria-activedescendant', params.data._resultId);
	    });
	
	    container.on('selection:update', function (params) {
	      self.update(params.data);
	    });
	
	    container.on('open', function () {
	      // When the dropdown is open, aria-expanded="true"
	      self.$selection.attr('aria-expanded', 'true');
	      self.$selection.attr('aria-owns', resultsId);
	
	      self._attachCloseHandler(container);
	    });
	
	    container.on('close', function () {
	      // When the dropdown is closed, aria-expanded="false"
	      self.$selection.attr('aria-expanded', 'false');
	      self.$selection.removeAttr('aria-activedescendant');
	      self.$selection.removeAttr('aria-owns');
	
	      self.$selection.focus();
	
	      self._detachCloseHandler(container);
	    });
	
	    container.on('enable', function () {
	      self.$selection.attr('tabindex', self._tabindex);
	    });
	
	    container.on('disable', function () {
	      self.$selection.attr('tabindex', '-1');
	    });
	  };
	
	  BaseSelection.prototype._handleBlur = function (evt) {
	    var self = this;
	
	    // This needs to be delayed as the active element is the body when the tab
	    // key is pressed, possibly along with others.
	    window.setTimeout(function () {
	      // Don't trigger `blur` if the focus is still in the selection
	      if (
	        (document.activeElement == self.$selection[0]) ||
	        ($.contains(self.$selection[0], document.activeElement))
	      ) {
	        return;
	      }
	
	      self.trigger('blur', evt);
	    }, 1);
	  };
	
	  BaseSelection.prototype._attachCloseHandler = function (container) {
	    var self = this;
	
	    $(document.body).on('mousedown.select2.' + container.id, function (e) {
	      var $target = $(e.target);
	
	      var $select = $target.closest('.select2');
	
	      var $all = $('.select2.select2-container--open');
	
	      $all.each(function () {
	        var $this = $(this);
	
	        if (this == $select[0]) {
	          return;
	        }
	
	        var $element = $this.data('element');
	
	        $element.select2('close');
	      });
	    });
	  };
	
	  BaseSelection.prototype._detachCloseHandler = function (container) {
	    $(document.body).off('mousedown.select2.' + container.id);
	  };
	
	  BaseSelection.prototype.position = function ($selection, $container) {
	    var $selectionContainer = $container.find('.selection');
	    $selectionContainer.append($selection);
	  };
	
	  BaseSelection.prototype.destroy = function () {
	    this._detachCloseHandler(this.container);
	  };
	
	  BaseSelection.prototype.update = function (data) {
	    throw new Error('The `update` method must be defined in child classes.');
	  };
	
	  return BaseSelection;
	});
	
	S2.define('select2/selection/single',[
	  'jquery',
	  './base',
	  '../utils',
	  '../keys'
	], function ($, BaseSelection, Utils, KEYS) {
	  function SingleSelection () {
	    SingleSelection.__super__.constructor.apply(this, arguments);
	  }
	
	  Utils.Extend(SingleSelection, BaseSelection);
	
	  SingleSelection.prototype.render = function () {
	    var $selection = SingleSelection.__super__.render.call(this);
	
	    $selection.addClass('select2-selection--single');
	
	    $selection.html(
	      '<span class="select2-selection__rendered"></span>' +
	      '<span class="select2-selection__arrow" role="presentation">' +
	        '<b role="presentation"></b>' +
	      '</span>'
	    );
	
	    return $selection;
	  };
	
	  SingleSelection.prototype.bind = function (container, $container) {
	    var self = this;
	
	    SingleSelection.__super__.bind.apply(this, arguments);
	
	    var id = container.id + '-container';
	
	    this.$selection.find('.select2-selection__rendered').attr('id', id);
	    this.$selection.attr('aria-labelledby', id);
	
	    this.$selection.on('mousedown', function (evt) {
	      // Only respond to left clicks
	      if (evt.which !== 1) {
	        return;
	      }
	
	      self.trigger('toggle', {
	        originalEvent: evt
	      });
	    });
	
	    this.$selection.on('focus', function (evt) {
	      // User focuses on the container
	    });
	
	    this.$selection.on('blur', function (evt) {
	      // User exits the container
	    });
	
	    container.on('focus', function (evt) {
	      if (!container.isOpen()) {
	        self.$selection.focus();
	      }
	    });
	
	    container.on('selection:update', function (params) {
	      self.update(params.data);
	    });
	  };
	
	  SingleSelection.prototype.clear = function () {
	    this.$selection.find('.select2-selection__rendered').empty();
	  };
	
	  SingleSelection.prototype.display = function (data, container) {
	    var template = this.options.get('templateSelection');
	    var escapeMarkup = this.options.get('escapeMarkup');
	
	    return escapeMarkup(template(data, container));
	  };
	
	  SingleSelection.prototype.selectionContainer = function () {
	    return $('<span></span>');
	  };
	
	  SingleSelection.prototype.update = function (data) {
	    if (data.length === 0) {
	      this.clear();
	      return;
	    }
	
	    var selection = data[0];
	
	    var $rendered = this.$selection.find('.select2-selection__rendered');
	    var formatted = this.display(selection, $rendered);
	
	    $rendered.empty().append(formatted);
	    $rendered.prop('title', selection.title || selection.text);
	  };
	
	  return SingleSelection;
	});
	
	S2.define('select2/selection/multiple',[
	  'jquery',
	  './base',
	  '../utils'
	], function ($, BaseSelection, Utils) {
	  function MultipleSelection ($element, options) {
	    MultipleSelection.__super__.constructor.apply(this, arguments);
	  }
	
	  Utils.Extend(MultipleSelection, BaseSelection);
	
	  MultipleSelection.prototype.render = function () {
	    var $selection = MultipleSelection.__super__.render.call(this);
	
	    $selection.addClass('select2-selection--multiple');
	
	    $selection.html(
	      '<ul class="select2-selection__rendered"></ul>'
	    );
	
	    return $selection;
	  };
	
	  MultipleSelection.prototype.bind = function (container, $container) {
	    var self = this;
	
	    MultipleSelection.__super__.bind.apply(this, arguments);
	
	    this.$selection.on('click', function (evt) {
	      self.trigger('toggle', {
	        originalEvent: evt
	      });
	    });
	
	    this.$selection.on(
	      'click',
	      '.select2-selection__choice__remove',
	      function (evt) {
	        // Ignore the event if it is disabled
	        if (self.options.get('disabled')) {
	          return;
	        }
	
	        var $remove = $(this);
	        var $selection = $remove.parent();
	
	        var data = $selection.data('data');
	
	        self.trigger('unselect', {
	          originalEvent: evt,
	          data: data
	        });
	      }
	    );
	  };
	
	  MultipleSelection.prototype.clear = function () {
	    this.$selection.find('.select2-selection__rendered').empty();
	  };
	
	  MultipleSelection.prototype.display = function (data, container) {
	    var template = this.options.get('templateSelection');
	    var escapeMarkup = this.options.get('escapeMarkup');
	
	    return escapeMarkup(template(data, container));
	  };
	
	  MultipleSelection.prototype.selectionContainer = function () {
	    var $container = $(
	      '<li class="select2-selection__choice">' +
	        '<span class="select2-selection__choice__remove" role="presentation">' +
	          '&times;' +
	        '</span>' +
	      '</li>'
	    );
	
	    return $container;
	  };
	
	  MultipleSelection.prototype.update = function (data) {
	    this.clear();
	
	    if (data.length === 0) {
	      return;
	    }
	
	    var $selections = [];
	
	    for (var d = 0; d < data.length; d++) {
	      var selection = data[d];
	
	      var $selection = this.selectionContainer();
	      var formatted = this.display(selection, $selection);
	
	      $selection.append(formatted);
	      $selection.prop('title', selection.title || selection.text);
	
	      $selection.data('data', selection);
	
	      $selections.push($selection);
	    }
	
	    var $rendered = this.$selection.find('.select2-selection__rendered');
	
	    Utils.appendMany($rendered, $selections);
	  };
	
	  return MultipleSelection;
	});
	
	S2.define('select2/selection/placeholder',[
	  '../utils'
	], function (Utils) {
	  function Placeholder (decorated, $element, options) {
	    this.placeholder = this.normalizePlaceholder(options.get('placeholder'));
	
	    decorated.call(this, $element, options);
	  }
	
	  Placeholder.prototype.normalizePlaceholder = function (_, placeholder) {
	    if (typeof placeholder === 'string') {
	      placeholder = {
	        id: '',
	        text: placeholder
	      };
	    }
	
	    return placeholder;
	  };
	
	  Placeholder.prototype.createPlaceholder = function (decorated, placeholder) {
	    var $placeholder = this.selectionContainer();
	
	    $placeholder.html(this.display(placeholder));
	    $placeholder.addClass('select2-selection__placeholder')
	                .removeClass('select2-selection__choice');
	
	    return $placeholder;
	  };
	
	  Placeholder.prototype.update = function (decorated, data) {
	    var singlePlaceholder = (
	      data.length == 1 && data[0].id != this.placeholder.id
	    );
	    var multipleSelections = data.length > 1;
	
	    if (multipleSelections || singlePlaceholder) {
	      return decorated.call(this, data);
	    }
	
	    this.clear();
	
	    var $placeholder = this.createPlaceholder(this.placeholder);
	
	    this.$selection.find('.select2-selection__rendered').append($placeholder);
	  };
	
	  return Placeholder;
	});
	
	S2.define('select2/selection/allowClear',[
	  'jquery',
	  '../keys'
	], function ($, KEYS) {
	  function AllowClear () { }
	
	  AllowClear.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    decorated.call(this, container, $container);
	
	    if (this.placeholder == null) {
	      if (this.options.get('debug') && window.console && console.error) {
	        console.error(
	          'Select2: The `allowClear` option should be used in combination ' +
	          'with the `placeholder` option.'
	        );
	      }
	    }
	
	    this.$selection.on('mousedown', '.select2-selection__clear',
	      function (evt) {
	        self._handleClear(evt);
	    });
	
	    container.on('keypress', function (evt) {
	      self._handleKeyboardClear(evt, container);
	    });
	  };
	
	  AllowClear.prototype._handleClear = function (_, evt) {
	    // Ignore the event if it is disabled
	    if (this.options.get('disabled')) {
	      return;
	    }
	
	    var $clear = this.$selection.find('.select2-selection__clear');
	
	    // Ignore the event if nothing has been selected
	    if ($clear.length === 0) {
	      return;
	    }
	
	    evt.stopPropagation();
	
	    var data = $clear.data('data');
	
	    for (var d = 0; d < data.length; d++) {
	      var unselectData = {
	        data: data[d]
	      };
	
	      // Trigger the `unselect` event, so people can prevent it from being
	      // cleared.
	      this.trigger('unselect', unselectData);
	
	      // If the event was prevented, don't clear it out.
	      if (unselectData.prevented) {
	        return;
	      }
	    }
	
	    this.$element.val(this.placeholder.id).trigger('change');
	
	    this.trigger('toggle', {});
	  };
	
	  AllowClear.prototype._handleKeyboardClear = function (_, evt, container) {
	    if (container.isOpen()) {
	      return;
	    }
	
	    if (evt.which == KEYS.DELETE || evt.which == KEYS.BACKSPACE) {
	      this._handleClear(evt);
	    }
	  };
	
	  AllowClear.prototype.update = function (decorated, data) {
	    decorated.call(this, data);
	
	    if (this.$selection.find('.select2-selection__placeholder').length > 0 ||
	        data.length === 0) {
	      return;
	    }
	
	    var $remove = $(
	      '<span class="select2-selection__clear">' +
	        '&times;' +
	      '</span>'
	    );
	    $remove.data('data', data);
	
	    this.$selection.find('.select2-selection__rendered').prepend($remove);
	  };
	
	  return AllowClear;
	});
	
	S2.define('select2/selection/search',[
	  'jquery',
	  '../utils',
	  '../keys'
	], function ($, Utils, KEYS) {
	  function Search (decorated, $element, options) {
	    decorated.call(this, $element, options);
	  }
	
	  Search.prototype.render = function (decorated) {
	    var $search = $(
	      '<li class="select2-search select2-search--inline">' +
	        '<input class="select2-search__field" type="search" tabindex="-1"' +
	        ' autocomplete="off" autocorrect="off" autocapitalize="off"' +
	        ' spellcheck="false" role="textbox" aria-autocomplete="list" />' +
	      '</li>'
	    );
	
	    this.$searchContainer = $search;
	    this.$search = $search.find('input');
	
	    var $rendered = decorated.call(this);
	
	    this._transferTabIndex();
	
	    return $rendered;
	  };
	
	  Search.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    decorated.call(this, container, $container);
	
	    container.on('open', function () {
	      self.$search.trigger('focus');
	    });
	
	    container.on('close', function () {
	      self.$search.val('');
	      self.$search.removeAttr('aria-activedescendant');
	      self.$search.trigger('focus');
	    });
	
	    container.on('enable', function () {
	      self.$search.prop('disabled', false);
	
	      self._transferTabIndex();
	    });
	
	    container.on('disable', function () {
	      self.$search.prop('disabled', true);
	    });
	
	    container.on('focus', function (evt) {
	      self.$search.trigger('focus');
	    });
	
	    container.on('results:focus', function (params) {
	      self.$search.attr('aria-activedescendant', params.id);
	    });
	
	    this.$selection.on('focusin', '.select2-search--inline', function (evt) {
	      self.trigger('focus', evt);
	    });
	
	    this.$selection.on('focusout', '.select2-search--inline', function (evt) {
	      self._handleBlur(evt);
	    });
	
	    this.$selection.on('keydown', '.select2-search--inline', function (evt) {
	      evt.stopPropagation();
	
	      self.trigger('keypress', evt);
	
	      self._keyUpPrevented = evt.isDefaultPrevented();
	
	      var key = evt.which;
	
	      if (key === KEYS.BACKSPACE && self.$search.val() === '') {
	        var $previousChoice = self.$searchContainer
	          .prev('.select2-selection__choice');
	
	        if ($previousChoice.length > 0) {
	          var item = $previousChoice.data('data');
	
	          self.searchRemoveChoice(item);
	
	          evt.preventDefault();
	        }
	      }
	    });
	
	    // Try to detect the IE version should the `documentMode` property that
	    // is stored on the document. This is only implemented in IE and is
	    // slightly cleaner than doing a user agent check.
	    // This property is not available in Edge, but Edge also doesn't have
	    // this bug.
	    var msie = document.documentMode;
	    var disableInputEvents = msie && msie <= 11;
	
	    // Workaround for browsers which do not support the `input` event
	    // This will prevent double-triggering of events for browsers which support
	    // both the `keyup` and `input` events.
	    this.$selection.on(
	      'input.searchcheck',
	      '.select2-search--inline',
	      function (evt) {
	        // IE will trigger the `input` event when a placeholder is used on a
	        // search box. To get around this issue, we are forced to ignore all
	        // `input` events in IE and keep using `keyup`.
	        if (disableInputEvents) {
	          self.$selection.off('input.search input.searchcheck');
	          return;
	        }
	
	        // Unbind the duplicated `keyup` event
	        self.$selection.off('keyup.search');
	      }
	    );
	
	    this.$selection.on(
	      'keyup.search input.search',
	      '.select2-search--inline',
	      function (evt) {
	        // IE will trigger the `input` event when a placeholder is used on a
	        // search box. To get around this issue, we are forced to ignore all
	        // `input` events in IE and keep using `keyup`.
	        if (disableInputEvents && evt.type === 'input') {
	          self.$selection.off('input.search input.searchcheck');
	          return;
	        }
	
	        var key = evt.which;
	
	        // We can freely ignore events from modifier keys
	        if (key == KEYS.SHIFT || key == KEYS.CTRL || key == KEYS.ALT) {
	          return;
	        }
	
	        // Tabbing will be handled during the `keydown` phase
	        if (key == KEYS.TAB) {
	          return;
	        }
	
	        self.handleSearch(evt);
	      }
	    );
	  };
	
	  /**
	   * This method will transfer the tabindex attribute from the rendered
	   * selection to the search box. This allows for the search box to be used as
	   * the primary focus instead of the selection container.
	   *
	   * @private
	   */
	  Search.prototype._transferTabIndex = function (decorated) {
	    this.$search.attr('tabindex', this.$selection.attr('tabindex'));
	    this.$selection.attr('tabindex', '-1');
	  };
	
	  Search.prototype.createPlaceholder = function (decorated, placeholder) {
	    this.$search.attr('placeholder', placeholder.text);
	  };
	
	  Search.prototype.update = function (decorated, data) {
	    var searchHadFocus = this.$search[0] == document.activeElement;
	
	    this.$search.attr('placeholder', '');
	
	    decorated.call(this, data);
	
	    this.$selection.find('.select2-selection__rendered')
	                   .append(this.$searchContainer);
	
	    this.resizeSearch();
	    if (searchHadFocus) {
	      this.$search.focus();
	    }
	  };
	
	  Search.prototype.handleSearch = function () {
	    this.resizeSearch();
	
	    if (!this._keyUpPrevented) {
	      var input = this.$search.val();
	
	      this.trigger('query', {
	        term: input
	      });
	    }
	
	    this._keyUpPrevented = false;
	  };
	
	  Search.prototype.searchRemoveChoice = function (decorated, item) {
	    this.trigger('unselect', {
	      data: item
	    });
	
	    this.$search.val(item.text);
	    this.handleSearch();
	  };
	
	  Search.prototype.resizeSearch = function () {
	    this.$search.css('width', '25px');
	
	    var width = '';
	
	    if (this.$search.attr('placeholder') !== '') {
	      width = this.$selection.find('.select2-selection__rendered').innerWidth();
	    } else {
	      var minimumWidth = this.$search.val().length + 1;
	
	      width = (minimumWidth * 0.75) + 'em';
	    }
	
	    this.$search.css('width', width);
	  };
	
	  return Search;
	});
	
	S2.define('select2/selection/eventRelay',[
	  'jquery'
	], function ($) {
	  function EventRelay () { }
	
	  EventRelay.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	    var relayEvents = [
	      'open', 'opening',
	      'close', 'closing',
	      'select', 'selecting',
	      'unselect', 'unselecting'
	    ];
	
	    var preventableEvents = ['opening', 'closing', 'selecting', 'unselecting'];
	
	    decorated.call(this, container, $container);
	
	    container.on('*', function (name, params) {
	      // Ignore events that should not be relayed
	      if ($.inArray(name, relayEvents) === -1) {
	        return;
	      }
	
	      // The parameters should always be an object
	      params = params || {};
	
	      // Generate the jQuery event for the Select2 event
	      var evt = $.Event('select2:' + name, {
	        params: params
	      });
	
	      self.$element.trigger(evt);
	
	      // Only handle preventable events if it was one
	      if ($.inArray(name, preventableEvents) === -1) {
	        return;
	      }
	
	      params.prevented = evt.isDefaultPrevented();
	    });
	  };
	
	  return EventRelay;
	});
	
	S2.define('select2/translation',[
	  'jquery',
	  'require'
	], function ($, require) {
	  function Translation (dict) {
	    this.dict = dict || {};
	  }
	
	  Translation.prototype.all = function () {
	    return this.dict;
	  };
	
	  Translation.prototype.get = function (key) {
	    return this.dict[key];
	  };
	
	  Translation.prototype.extend = function (translation) {
	    this.dict = $.extend({}, translation.all(), this.dict);
	  };
	
	  // Static functions
	
	  Translation._cache = {};
	
	  Translation.loadPath = function (path) {
	    if (!(path in Translation._cache)) {
	      var translations = require(path);
	
	      Translation._cache[path] = translations;
	    }
	
	    return new Translation(Translation._cache[path]);
	  };
	
	  return Translation;
	});
	
	S2.define('select2/diacritics',[
	
	], function () {
	  var diacritics = {
	    '\u24B6': 'A',
	    '\uFF21': 'A',
	    '\u00C0': 'A',
	    '\u00C1': 'A',
	    '\u00C2': 'A',
	    '\u1EA6': 'A',
	    '\u1EA4': 'A',
	    '\u1EAA': 'A',
	    '\u1EA8': 'A',
	    '\u00C3': 'A',
	    '\u0100': 'A',
	    '\u0102': 'A',
	    '\u1EB0': 'A',
	    '\u1EAE': 'A',
	    '\u1EB4': 'A',
	    '\u1EB2': 'A',
	    '\u0226': 'A',
	    '\u01E0': 'A',
	    '\u00C4': 'A',
	    '\u01DE': 'A',
	    '\u1EA2': 'A',
	    '\u00C5': 'A',
	    '\u01FA': 'A',
	    '\u01CD': 'A',
	    '\u0200': 'A',
	    '\u0202': 'A',
	    '\u1EA0': 'A',
	    '\u1EAC': 'A',
	    '\u1EB6': 'A',
	    '\u1E00': 'A',
	    '\u0104': 'A',
	    '\u023A': 'A',
	    '\u2C6F': 'A',
	    '\uA732': 'AA',
	    '\u00C6': 'AE',
	    '\u01FC': 'AE',
	    '\u01E2': 'AE',
	    '\uA734': 'AO',
	    '\uA736': 'AU',
	    '\uA738': 'AV',
	    '\uA73A': 'AV',
	    '\uA73C': 'AY',
	    '\u24B7': 'B',
	    '\uFF22': 'B',
	    '\u1E02': 'B',
	    '\u1E04': 'B',
	    '\u1E06': 'B',
	    '\u0243': 'B',
	    '\u0182': 'B',
	    '\u0181': 'B',
	    '\u24B8': 'C',
	    '\uFF23': 'C',
	    '\u0106': 'C',
	    '\u0108': 'C',
	    '\u010A': 'C',
	    '\u010C': 'C',
	    '\u00C7': 'C',
	    '\u1E08': 'C',
	    '\u0187': 'C',
	    '\u023B': 'C',
	    '\uA73E': 'C',
	    '\u24B9': 'D',
	    '\uFF24': 'D',
	    '\u1E0A': 'D',
	    '\u010E': 'D',
	    '\u1E0C': 'D',
	    '\u1E10': 'D',
	    '\u1E12': 'D',
	    '\u1E0E': 'D',
	    '\u0110': 'D',
	    '\u018B': 'D',
	    '\u018A': 'D',
	    '\u0189': 'D',
	    '\uA779': 'D',
	    '\u01F1': 'DZ',
	    '\u01C4': 'DZ',
	    '\u01F2': 'Dz',
	    '\u01C5': 'Dz',
	    '\u24BA': 'E',
	    '\uFF25': 'E',
	    '\u00C8': 'E',
	    '\u00C9': 'E',
	    '\u00CA': 'E',
	    '\u1EC0': 'E',
	    '\u1EBE': 'E',
	    '\u1EC4': 'E',
	    '\u1EC2': 'E',
	    '\u1EBC': 'E',
	    '\u0112': 'E',
	    '\u1E14': 'E',
	    '\u1E16': 'E',
	    '\u0114': 'E',
	    '\u0116': 'E',
	    '\u00CB': 'E',
	    '\u1EBA': 'E',
	    '\u011A': 'E',
	    '\u0204': 'E',
	    '\u0206': 'E',
	    '\u1EB8': 'E',
	    '\u1EC6': 'E',
	    '\u0228': 'E',
	    '\u1E1C': 'E',
	    '\u0118': 'E',
	    '\u1E18': 'E',
	    '\u1E1A': 'E',
	    '\u0190': 'E',
	    '\u018E': 'E',
	    '\u24BB': 'F',
	    '\uFF26': 'F',
	    '\u1E1E': 'F',
	    '\u0191': 'F',
	    '\uA77B': 'F',
	    '\u24BC': 'G',
	    '\uFF27': 'G',
	    '\u01F4': 'G',
	    '\u011C': 'G',
	    '\u1E20': 'G',
	    '\u011E': 'G',
	    '\u0120': 'G',
	    '\u01E6': 'G',
	    '\u0122': 'G',
	    '\u01E4': 'G',
	    '\u0193': 'G',
	    '\uA7A0': 'G',
	    '\uA77D': 'G',
	    '\uA77E': 'G',
	    '\u24BD': 'H',
	    '\uFF28': 'H',
	    '\u0124': 'H',
	    '\u1E22': 'H',
	    '\u1E26': 'H',
	    '\u021E': 'H',
	    '\u1E24': 'H',
	    '\u1E28': 'H',
	    '\u1E2A': 'H',
	    '\u0126': 'H',
	    '\u2C67': 'H',
	    '\u2C75': 'H',
	    '\uA78D': 'H',
	    '\u24BE': 'I',
	    '\uFF29': 'I',
	    '\u00CC': 'I',
	    '\u00CD': 'I',
	    '\u00CE': 'I',
	    '\u0128': 'I',
	    '\u012A': 'I',
	    '\u012C': 'I',
	    '\u0130': 'I',
	    '\u00CF': 'I',
	    '\u1E2E': 'I',
	    '\u1EC8': 'I',
	    '\u01CF': 'I',
	    '\u0208': 'I',
	    '\u020A': 'I',
	    '\u1ECA': 'I',
	    '\u012E': 'I',
	    '\u1E2C': 'I',
	    '\u0197': 'I',
	    '\u24BF': 'J',
	    '\uFF2A': 'J',
	    '\u0134': 'J',
	    '\u0248': 'J',
	    '\u24C0': 'K',
	    '\uFF2B': 'K',
	    '\u1E30': 'K',
	    '\u01E8': 'K',
	    '\u1E32': 'K',
	    '\u0136': 'K',
	    '\u1E34': 'K',
	    '\u0198': 'K',
	    '\u2C69': 'K',
	    '\uA740': 'K',
	    '\uA742': 'K',
	    '\uA744': 'K',
	    '\uA7A2': 'K',
	    '\u24C1': 'L',
	    '\uFF2C': 'L',
	    '\u013F': 'L',
	    '\u0139': 'L',
	    '\u013D': 'L',
	    '\u1E36': 'L',
	    '\u1E38': 'L',
	    '\u013B': 'L',
	    '\u1E3C': 'L',
	    '\u1E3A': 'L',
	    '\u0141': 'L',
	    '\u023D': 'L',
	    '\u2C62': 'L',
	    '\u2C60': 'L',
	    '\uA748': 'L',
	    '\uA746': 'L',
	    '\uA780': 'L',
	    '\u01C7': 'LJ',
	    '\u01C8': 'Lj',
	    '\u24C2': 'M',
	    '\uFF2D': 'M',
	    '\u1E3E': 'M',
	    '\u1E40': 'M',
	    '\u1E42': 'M',
	    '\u2C6E': 'M',
	    '\u019C': 'M',
	    '\u24C3': 'N',
	    '\uFF2E': 'N',
	    '\u01F8': 'N',
	    '\u0143': 'N',
	    '\u00D1': 'N',
	    '\u1E44': 'N',
	    '\u0147': 'N',
	    '\u1E46': 'N',
	    '\u0145': 'N',
	    '\u1E4A': 'N',
	    '\u1E48': 'N',
	    '\u0220': 'N',
	    '\u019D': 'N',
	    '\uA790': 'N',
	    '\uA7A4': 'N',
	    '\u01CA': 'NJ',
	    '\u01CB': 'Nj',
	    '\u24C4': 'O',
	    '\uFF2F': 'O',
	    '\u00D2': 'O',
	    '\u00D3': 'O',
	    '\u00D4': 'O',
	    '\u1ED2': 'O',
	    '\u1ED0': 'O',
	    '\u1ED6': 'O',
	    '\u1ED4': 'O',
	    '\u00D5': 'O',
	    '\u1E4C': 'O',
	    '\u022C': 'O',
	    '\u1E4E': 'O',
	    '\u014C': 'O',
	    '\u1E50': 'O',
	    '\u1E52': 'O',
	    '\u014E': 'O',
	    '\u022E': 'O',
	    '\u0230': 'O',
	    '\u00D6': 'O',
	    '\u022A': 'O',
	    '\u1ECE': 'O',
	    '\u0150': 'O',
	    '\u01D1': 'O',
	    '\u020C': 'O',
	    '\u020E': 'O',
	    '\u01A0': 'O',
	    '\u1EDC': 'O',
	    '\u1EDA': 'O',
	    '\u1EE0': 'O',
	    '\u1EDE': 'O',
	    '\u1EE2': 'O',
	    '\u1ECC': 'O',
	    '\u1ED8': 'O',
	    '\u01EA': 'O',
	    '\u01EC': 'O',
	    '\u00D8': 'O',
	    '\u01FE': 'O',
	    '\u0186': 'O',
	    '\u019F': 'O',
	    '\uA74A': 'O',
	    '\uA74C': 'O',
	    '\u01A2': 'OI',
	    '\uA74E': 'OO',
	    '\u0222': 'OU',
	    '\u24C5': 'P',
	    '\uFF30': 'P',
	    '\u1E54': 'P',
	    '\u1E56': 'P',
	    '\u01A4': 'P',
	    '\u2C63': 'P',
	    '\uA750': 'P',
	    '\uA752': 'P',
	    '\uA754': 'P',
	    '\u24C6': 'Q',
	    '\uFF31': 'Q',
	    '\uA756': 'Q',
	    '\uA758': 'Q',
	    '\u024A': 'Q',
	    '\u24C7': 'R',
	    '\uFF32': 'R',
	    '\u0154': 'R',
	    '\u1E58': 'R',
	    '\u0158': 'R',
	    '\u0210': 'R',
	    '\u0212': 'R',
	    '\u1E5A': 'R',
	    '\u1E5C': 'R',
	    '\u0156': 'R',
	    '\u1E5E': 'R',
	    '\u024C': 'R',
	    '\u2C64': 'R',
	    '\uA75A': 'R',
	    '\uA7A6': 'R',
	    '\uA782': 'R',
	    '\u24C8': 'S',
	    '\uFF33': 'S',
	    '\u1E9E': 'S',
	    '\u015A': 'S',
	    '\u1E64': 'S',
	    '\u015C': 'S',
	    '\u1E60': 'S',
	    '\u0160': 'S',
	    '\u1E66': 'S',
	    '\u1E62': 'S',
	    '\u1E68': 'S',
	    '\u0218': 'S',
	    '\u015E': 'S',
	    '\u2C7E': 'S',
	    '\uA7A8': 'S',
	    '\uA784': 'S',
	    '\u24C9': 'T',
	    '\uFF34': 'T',
	    '\u1E6A': 'T',
	    '\u0164': 'T',
	    '\u1E6C': 'T',
	    '\u021A': 'T',
	    '\u0162': 'T',
	    '\u1E70': 'T',
	    '\u1E6E': 'T',
	    '\u0166': 'T',
	    '\u01AC': 'T',
	    '\u01AE': 'T',
	    '\u023E': 'T',
	    '\uA786': 'T',
	    '\uA728': 'TZ',
	    '\u24CA': 'U',
	    '\uFF35': 'U',
	    '\u00D9': 'U',
	    '\u00DA': 'U',
	    '\u00DB': 'U',
	    '\u0168': 'U',
	    '\u1E78': 'U',
	    '\u016A': 'U',
	    '\u1E7A': 'U',
	    '\u016C': 'U',
	    '\u00DC': 'U',
	    '\u01DB': 'U',
	    '\u01D7': 'U',
	    '\u01D5': 'U',
	    '\u01D9': 'U',
	    '\u1EE6': 'U',
	    '\u016E': 'U',
	    '\u0170': 'U',
	    '\u01D3': 'U',
	    '\u0214': 'U',
	    '\u0216': 'U',
	    '\u01AF': 'U',
	    '\u1EEA': 'U',
	    '\u1EE8': 'U',
	    '\u1EEE': 'U',
	    '\u1EEC': 'U',
	    '\u1EF0': 'U',
	    '\u1EE4': 'U',
	    '\u1E72': 'U',
	    '\u0172': 'U',
	    '\u1E76': 'U',
	    '\u1E74': 'U',
	    '\u0244': 'U',
	    '\u24CB': 'V',
	    '\uFF36': 'V',
	    '\u1E7C': 'V',
	    '\u1E7E': 'V',
	    '\u01B2': 'V',
	    '\uA75E': 'V',
	    '\u0245': 'V',
	    '\uA760': 'VY',
	    '\u24CC': 'W',
	    '\uFF37': 'W',
	    '\u1E80': 'W',
	    '\u1E82': 'W',
	    '\u0174': 'W',
	    '\u1E86': 'W',
	    '\u1E84': 'W',
	    '\u1E88': 'W',
	    '\u2C72': 'W',
	    '\u24CD': 'X',
	    '\uFF38': 'X',
	    '\u1E8A': 'X',
	    '\u1E8C': 'X',
	    '\u24CE': 'Y',
	    '\uFF39': 'Y',
	    '\u1EF2': 'Y',
	    '\u00DD': 'Y',
	    '\u0176': 'Y',
	    '\u1EF8': 'Y',
	    '\u0232': 'Y',
	    '\u1E8E': 'Y',
	    '\u0178': 'Y',
	    '\u1EF6': 'Y',
	    '\u1EF4': 'Y',
	    '\u01B3': 'Y',
	    '\u024E': 'Y',
	    '\u1EFE': 'Y',
	    '\u24CF': 'Z',
	    '\uFF3A': 'Z',
	    '\u0179': 'Z',
	    '\u1E90': 'Z',
	    '\u017B': 'Z',
	    '\u017D': 'Z',
	    '\u1E92': 'Z',
	    '\u1E94': 'Z',
	    '\u01B5': 'Z',
	    '\u0224': 'Z',
	    '\u2C7F': 'Z',
	    '\u2C6B': 'Z',
	    '\uA762': 'Z',
	    '\u24D0': 'a',
	    '\uFF41': 'a',
	    '\u1E9A': 'a',
	    '\u00E0': 'a',
	    '\u00E1': 'a',
	    '\u00E2': 'a',
	    '\u1EA7': 'a',
	    '\u1EA5': 'a',
	    '\u1EAB': 'a',
	    '\u1EA9': 'a',
	    '\u00E3': 'a',
	    '\u0101': 'a',
	    '\u0103': 'a',
	    '\u1EB1': 'a',
	    '\u1EAF': 'a',
	    '\u1EB5': 'a',
	    '\u1EB3': 'a',
	    '\u0227': 'a',
	    '\u01E1': 'a',
	    '\u00E4': 'a',
	    '\u01DF': 'a',
	    '\u1EA3': 'a',
	    '\u00E5': 'a',
	    '\u01FB': 'a',
	    '\u01CE': 'a',
	    '\u0201': 'a',
	    '\u0203': 'a',
	    '\u1EA1': 'a',
	    '\u1EAD': 'a',
	    '\u1EB7': 'a',
	    '\u1E01': 'a',
	    '\u0105': 'a',
	    '\u2C65': 'a',
	    '\u0250': 'a',
	    '\uA733': 'aa',
	    '\u00E6': 'ae',
	    '\u01FD': 'ae',
	    '\u01E3': 'ae',
	    '\uA735': 'ao',
	    '\uA737': 'au',
	    '\uA739': 'av',
	    '\uA73B': 'av',
	    '\uA73D': 'ay',
	    '\u24D1': 'b',
	    '\uFF42': 'b',
	    '\u1E03': 'b',
	    '\u1E05': 'b',
	    '\u1E07': 'b',
	    '\u0180': 'b',
	    '\u0183': 'b',
	    '\u0253': 'b',
	    '\u24D2': 'c',
	    '\uFF43': 'c',
	    '\u0107': 'c',
	    '\u0109': 'c',
	    '\u010B': 'c',
	    '\u010D': 'c',
	    '\u00E7': 'c',
	    '\u1E09': 'c',
	    '\u0188': 'c',
	    '\u023C': 'c',
	    '\uA73F': 'c',
	    '\u2184': 'c',
	    '\u24D3': 'd',
	    '\uFF44': 'd',
	    '\u1E0B': 'd',
	    '\u010F': 'd',
	    '\u1E0D': 'd',
	    '\u1E11': 'd',
	    '\u1E13': 'd',
	    '\u1E0F': 'd',
	    '\u0111': 'd',
	    '\u018C': 'd',
	    '\u0256': 'd',
	    '\u0257': 'd',
	    '\uA77A': 'd',
	    '\u01F3': 'dz',
	    '\u01C6': 'dz',
	    '\u24D4': 'e',
	    '\uFF45': 'e',
	    '\u00E8': 'e',
	    '\u00E9': 'e',
	    '\u00EA': 'e',
	    '\u1EC1': 'e',
	    '\u1EBF': 'e',
	    '\u1EC5': 'e',
	    '\u1EC3': 'e',
	    '\u1EBD': 'e',
	    '\u0113': 'e',
	    '\u1E15': 'e',
	    '\u1E17': 'e',
	    '\u0115': 'e',
	    '\u0117': 'e',
	    '\u00EB': 'e',
	    '\u1EBB': 'e',
	    '\u011B': 'e',
	    '\u0205': 'e',
	    '\u0207': 'e',
	    '\u1EB9': 'e',
	    '\u1EC7': 'e',
	    '\u0229': 'e',
	    '\u1E1D': 'e',
	    '\u0119': 'e',
	    '\u1E19': 'e',
	    '\u1E1B': 'e',
	    '\u0247': 'e',
	    '\u025B': 'e',
	    '\u01DD': 'e',
	    '\u24D5': 'f',
	    '\uFF46': 'f',
	    '\u1E1F': 'f',
	    '\u0192': 'f',
	    '\uA77C': 'f',
	    '\u24D6': 'g',
	    '\uFF47': 'g',
	    '\u01F5': 'g',
	    '\u011D': 'g',
	    '\u1E21': 'g',
	    '\u011F': 'g',
	    '\u0121': 'g',
	    '\u01E7': 'g',
	    '\u0123': 'g',
	    '\u01E5': 'g',
	    '\u0260': 'g',
	    '\uA7A1': 'g',
	    '\u1D79': 'g',
	    '\uA77F': 'g',
	    '\u24D7': 'h',
	    '\uFF48': 'h',
	    '\u0125': 'h',
	    '\u1E23': 'h',
	    '\u1E27': 'h',
	    '\u021F': 'h',
	    '\u1E25': 'h',
	    '\u1E29': 'h',
	    '\u1E2B': 'h',
	    '\u1E96': 'h',
	    '\u0127': 'h',
	    '\u2C68': 'h',
	    '\u2C76': 'h',
	    '\u0265': 'h',
	    '\u0195': 'hv',
	    '\u24D8': 'i',
	    '\uFF49': 'i',
	    '\u00EC': 'i',
	    '\u00ED': 'i',
	    '\u00EE': 'i',
	    '\u0129': 'i',
	    '\u012B': 'i',
	    '\u012D': 'i',
	    '\u00EF': 'i',
	    '\u1E2F': 'i',
	    '\u1EC9': 'i',
	    '\u01D0': 'i',
	    '\u0209': 'i',
	    '\u020B': 'i',
	    '\u1ECB': 'i',
	    '\u012F': 'i',
	    '\u1E2D': 'i',
	    '\u0268': 'i',
	    '\u0131': 'i',
	    '\u24D9': 'j',
	    '\uFF4A': 'j',
	    '\u0135': 'j',
	    '\u01F0': 'j',
	    '\u0249': 'j',
	    '\u24DA': 'k',
	    '\uFF4B': 'k',
	    '\u1E31': 'k',
	    '\u01E9': 'k',
	    '\u1E33': 'k',
	    '\u0137': 'k',
	    '\u1E35': 'k',
	    '\u0199': 'k',
	    '\u2C6A': 'k',
	    '\uA741': 'k',
	    '\uA743': 'k',
	    '\uA745': 'k',
	    '\uA7A3': 'k',
	    '\u24DB': 'l',
	    '\uFF4C': 'l',
	    '\u0140': 'l',
	    '\u013A': 'l',
	    '\u013E': 'l',
	    '\u1E37': 'l',
	    '\u1E39': 'l',
	    '\u013C': 'l',
	    '\u1E3D': 'l',
	    '\u1E3B': 'l',
	    '\u017F': 'l',
	    '\u0142': 'l',
	    '\u019A': 'l',
	    '\u026B': 'l',
	    '\u2C61': 'l',
	    '\uA749': 'l',
	    '\uA781': 'l',
	    '\uA747': 'l',
	    '\u01C9': 'lj',
	    '\u24DC': 'm',
	    '\uFF4D': 'm',
	    '\u1E3F': 'm',
	    '\u1E41': 'm',
	    '\u1E43': 'm',
	    '\u0271': 'm',
	    '\u026F': 'm',
	    '\u24DD': 'n',
	    '\uFF4E': 'n',
	    '\u01F9': 'n',
	    '\u0144': 'n',
	    '\u00F1': 'n',
	    '\u1E45': 'n',
	    '\u0148': 'n',
	    '\u1E47': 'n',
	    '\u0146': 'n',
	    '\u1E4B': 'n',
	    '\u1E49': 'n',
	    '\u019E': 'n',
	    '\u0272': 'n',
	    '\u0149': 'n',
	    '\uA791': 'n',
	    '\uA7A5': 'n',
	    '\u01CC': 'nj',
	    '\u24DE': 'o',
	    '\uFF4F': 'o',
	    '\u00F2': 'o',
	    '\u00F3': 'o',
	    '\u00F4': 'o',
	    '\u1ED3': 'o',
	    '\u1ED1': 'o',
	    '\u1ED7': 'o',
	    '\u1ED5': 'o',
	    '\u00F5': 'o',
	    '\u1E4D': 'o',
	    '\u022D': 'o',
	    '\u1E4F': 'o',
	    '\u014D': 'o',
	    '\u1E51': 'o',
	    '\u1E53': 'o',
	    '\u014F': 'o',
	    '\u022F': 'o',
	    '\u0231': 'o',
	    '\u00F6': 'o',
	    '\u022B': 'o',
	    '\u1ECF': 'o',
	    '\u0151': 'o',
	    '\u01D2': 'o',
	    '\u020D': 'o',
	    '\u020F': 'o',
	    '\u01A1': 'o',
	    '\u1EDD': 'o',
	    '\u1EDB': 'o',
	    '\u1EE1': 'o',
	    '\u1EDF': 'o',
	    '\u1EE3': 'o',
	    '\u1ECD': 'o',
	    '\u1ED9': 'o',
	    '\u01EB': 'o',
	    '\u01ED': 'o',
	    '\u00F8': 'o',
	    '\u01FF': 'o',
	    '\u0254': 'o',
	    '\uA74B': 'o',
	    '\uA74D': 'o',
	    '\u0275': 'o',
	    '\u01A3': 'oi',
	    '\u0223': 'ou',
	    '\uA74F': 'oo',
	    '\u24DF': 'p',
	    '\uFF50': 'p',
	    '\u1E55': 'p',
	    '\u1E57': 'p',
	    '\u01A5': 'p',
	    '\u1D7D': 'p',
	    '\uA751': 'p',
	    '\uA753': 'p',
	    '\uA755': 'p',
	    '\u24E0': 'q',
	    '\uFF51': 'q',
	    '\u024B': 'q',
	    '\uA757': 'q',
	    '\uA759': 'q',
	    '\u24E1': 'r',
	    '\uFF52': 'r',
	    '\u0155': 'r',
	    '\u1E59': 'r',
	    '\u0159': 'r',
	    '\u0211': 'r',
	    '\u0213': 'r',
	    '\u1E5B': 'r',
	    '\u1E5D': 'r',
	    '\u0157': 'r',
	    '\u1E5F': 'r',
	    '\u024D': 'r',
	    '\u027D': 'r',
	    '\uA75B': 'r',
	    '\uA7A7': 'r',
	    '\uA783': 'r',
	    '\u24E2': 's',
	    '\uFF53': 's',
	    '\u00DF': 's',
	    '\u015B': 's',
	    '\u1E65': 's',
	    '\u015D': 's',
	    '\u1E61': 's',
	    '\u0161': 's',
	    '\u1E67': 's',
	    '\u1E63': 's',
	    '\u1E69': 's',
	    '\u0219': 's',
	    '\u015F': 's',
	    '\u023F': 's',
	    '\uA7A9': 's',
	    '\uA785': 's',
	    '\u1E9B': 's',
	    '\u24E3': 't',
	    '\uFF54': 't',
	    '\u1E6B': 't',
	    '\u1E97': 't',
	    '\u0165': 't',
	    '\u1E6D': 't',
	    '\u021B': 't',
	    '\u0163': 't',
	    '\u1E71': 't',
	    '\u1E6F': 't',
	    '\u0167': 't',
	    '\u01AD': 't',
	    '\u0288': 't',
	    '\u2C66': 't',
	    '\uA787': 't',
	    '\uA729': 'tz',
	    '\u24E4': 'u',
	    '\uFF55': 'u',
	    '\u00F9': 'u',
	    '\u00FA': 'u',
	    '\u00FB': 'u',
	    '\u0169': 'u',
	    '\u1E79': 'u',
	    '\u016B': 'u',
	    '\u1E7B': 'u',
	    '\u016D': 'u',
	    '\u00FC': 'u',
	    '\u01DC': 'u',
	    '\u01D8': 'u',
	    '\u01D6': 'u',
	    '\u01DA': 'u',
	    '\u1EE7': 'u',
	    '\u016F': 'u',
	    '\u0171': 'u',
	    '\u01D4': 'u',
	    '\u0215': 'u',
	    '\u0217': 'u',
	    '\u01B0': 'u',
	    '\u1EEB': 'u',
	    '\u1EE9': 'u',
	    '\u1EEF': 'u',
	    '\u1EED': 'u',
	    '\u1EF1': 'u',
	    '\u1EE5': 'u',
	    '\u1E73': 'u',
	    '\u0173': 'u',
	    '\u1E77': 'u',
	    '\u1E75': 'u',
	    '\u0289': 'u',
	    '\u24E5': 'v',
	    '\uFF56': 'v',
	    '\u1E7D': 'v',
	    '\u1E7F': 'v',
	    '\u028B': 'v',
	    '\uA75F': 'v',
	    '\u028C': 'v',
	    '\uA761': 'vy',
	    '\u24E6': 'w',
	    '\uFF57': 'w',
	    '\u1E81': 'w',
	    '\u1E83': 'w',
	    '\u0175': 'w',
	    '\u1E87': 'w',
	    '\u1E85': 'w',
	    '\u1E98': 'w',
	    '\u1E89': 'w',
	    '\u2C73': 'w',
	    '\u24E7': 'x',
	    '\uFF58': 'x',
	    '\u1E8B': 'x',
	    '\u1E8D': 'x',
	    '\u24E8': 'y',
	    '\uFF59': 'y',
	    '\u1EF3': 'y',
	    '\u00FD': 'y',
	    '\u0177': 'y',
	    '\u1EF9': 'y',
	    '\u0233': 'y',
	    '\u1E8F': 'y',
	    '\u00FF': 'y',
	    '\u1EF7': 'y',
	    '\u1E99': 'y',
	    '\u1EF5': 'y',
	    '\u01B4': 'y',
	    '\u024F': 'y',
	    '\u1EFF': 'y',
	    '\u24E9': 'z',
	    '\uFF5A': 'z',
	    '\u017A': 'z',
	    '\u1E91': 'z',
	    '\u017C': 'z',
	    '\u017E': 'z',
	    '\u1E93': 'z',
	    '\u1E95': 'z',
	    '\u01B6': 'z',
	    '\u0225': 'z',
	    '\u0240': 'z',
	    '\u2C6C': 'z',
	    '\uA763': 'z',
	    '\u0386': '\u0391',
	    '\u0388': '\u0395',
	    '\u0389': '\u0397',
	    '\u038A': '\u0399',
	    '\u03AA': '\u0399',
	    '\u038C': '\u039F',
	    '\u038E': '\u03A5',
	    '\u03AB': '\u03A5',
	    '\u038F': '\u03A9',
	    '\u03AC': '\u03B1',
	    '\u03AD': '\u03B5',
	    '\u03AE': '\u03B7',
	    '\u03AF': '\u03B9',
	    '\u03CA': '\u03B9',
	    '\u0390': '\u03B9',
	    '\u03CC': '\u03BF',
	    '\u03CD': '\u03C5',
	    '\u03CB': '\u03C5',
	    '\u03B0': '\u03C5',
	    '\u03C9': '\u03C9',
	    '\u03C2': '\u03C3'
	  };
	
	  return diacritics;
	});
	
	S2.define('select2/data/base',[
	  '../utils'
	], function (Utils) {
	  function BaseAdapter ($element, options) {
	    BaseAdapter.__super__.constructor.call(this);
	  }
	
	  Utils.Extend(BaseAdapter, Utils.Observable);
	
	  BaseAdapter.prototype.current = function (callback) {
	    throw new Error('The `current` method must be defined in child classes.');
	  };
	
	  BaseAdapter.prototype.query = function (params, callback) {
	    throw new Error('The `query` method must be defined in child classes.');
	  };
	
	  BaseAdapter.prototype.bind = function (container, $container) {
	    // Can be implemented in subclasses
	  };
	
	  BaseAdapter.prototype.destroy = function () {
	    // Can be implemented in subclasses
	  };
	
	  BaseAdapter.prototype.generateResultId = function (container, data) {
	    var id = container.id + '-result-';
	
	    id += Utils.generateChars(4);
	
	    if (data.id != null) {
	      id += '-' + data.id.toString();
	    } else {
	      id += '-' + Utils.generateChars(4);
	    }
	    return id;
	  };
	
	  return BaseAdapter;
	});
	
	S2.define('select2/data/select',[
	  './base',
	  '../utils',
	  'jquery'
	], function (BaseAdapter, Utils, $) {
	  function SelectAdapter ($element, options) {
	    this.$element = $element;
	    this.options = options;
	
	    SelectAdapter.__super__.constructor.call(this);
	  }
	
	  Utils.Extend(SelectAdapter, BaseAdapter);
	
	  SelectAdapter.prototype.current = function (callback) {
	    var data = [];
	    var self = this;
	
	    this.$element.find(':selected').each(function () {
	      var $option = $(this);
	
	      var option = self.item($option);
	
	      data.push(option);
	    });
	
	    callback(data);
	  };
	
	  SelectAdapter.prototype.select = function (data) {
	    var self = this;
	
	    data.selected = true;
	
	    // If data.element is a DOM node, use it instead
	    if ($(data.element).is('option')) {
	      data.element.selected = true;
	
	      this.$element.trigger('change');
	
	      return;
	    }
	
	    if (this.$element.prop('multiple')) {
	      this.current(function (currentData) {
	        var val = [];
	
	        data = [data];
	        data.push.apply(data, currentData);
	
	        for (var d = 0; d < data.length; d++) {
	          var id = data[d].id;
	
	          if ($.inArray(id, val) === -1) {
	            val.push(id);
	          }
	        }
	
	        self.$element.val(val);
	        self.$element.trigger('change');
	      });
	    } else {
	      var val = data.id;
	
	      this.$element.val(val);
	      this.$element.trigger('change');
	    }
	  };
	
	  SelectAdapter.prototype.unselect = function (data) {
	    var self = this;
	
	    if (!this.$element.prop('multiple')) {
	      return;
	    }
	
	    data.selected = false;
	
	    if ($(data.element).is('option')) {
	      data.element.selected = false;
	
	      this.$element.trigger('change');
	
	      return;
	    }
	
	    this.current(function (currentData) {
	      var val = [];
	
	      for (var d = 0; d < currentData.length; d++) {
	        var id = currentData[d].id;
	
	        if (id !== data.id && $.inArray(id, val) === -1) {
	          val.push(id);
	        }
	      }
	
	      self.$element.val(val);
	
	      self.$element.trigger('change');
	    });
	  };
	
	  SelectAdapter.prototype.bind = function (container, $container) {
	    var self = this;
	
	    this.container = container;
	
	    container.on('select', function (params) {
	      self.select(params.data);
	    });
	
	    container.on('unselect', function (params) {
	      self.unselect(params.data);
	    });
	  };
	
	  SelectAdapter.prototype.destroy = function () {
	    // Remove anything added to child elements
	    this.$element.find('*').each(function () {
	      // Remove any custom data set by Select2
	      $.removeData(this, 'data');
	    });
	  };
	
	  SelectAdapter.prototype.query = function (params, callback) {
	    var data = [];
	    var self = this;
	
	    var $options = this.$element.children();
	
	    $options.each(function () {
	      var $option = $(this);
	
	      if (!$option.is('option') && !$option.is('optgroup')) {
	        return;
	      }
	
	      var option = self.item($option);
	
	      var matches = self.matches(params, option);
	
	      if (matches !== null) {
	        data.push(matches);
	      }
	    });
	
	    callback({
	      results: data
	    });
	  };
	
	  SelectAdapter.prototype.addOptions = function ($options) {
	    Utils.appendMany(this.$element, $options);
	  };
	
	  SelectAdapter.prototype.option = function (data) {
	    var option;
	
	    if (data.children) {
	      option = document.createElement('optgroup');
	      option.label = data.text;
	    } else {
	      option = document.createElement('option');
	
	      if (option.textContent !== undefined) {
	        option.textContent = data.text;
	      } else {
	        option.innerText = data.text;
	      }
	    }
	
	    if (data.id !== undefined) {
	      option.value = data.id;
	    }
	
	    if (data.disabled) {
	      option.disabled = true;
	    }
	
	    if (data.selected) {
	      option.selected = true;
	    }
	
	    if (data.title) {
	      option.title = data.title;
	    }
	
	    var $option = $(option);
	
	    var normalizedData = this._normalizeItem(data);
	    normalizedData.element = option;
	
	    // Override the option's data with the combined data
	    $.data(option, 'data', normalizedData);
	
	    return $option;
	  };
	
	  SelectAdapter.prototype.item = function ($option) {
	    var data = {};
	
	    data = $.data($option[0], 'data');
	
	    if (data != null) {
	      return data;
	    }
	
	    if ($option.is('option')) {
	      data = {
	        id: $option.val(),
	        text: $option.text(),
	        disabled: $option.prop('disabled'),
	        selected: $option.prop('selected'),
	        title: $option.prop('title')
	      };
	    } else if ($option.is('optgroup')) {
	      data = {
	        text: $option.prop('label'),
	        children: [],
	        title: $option.prop('title')
	      };
	
	      var $children = $option.children('option');
	      var children = [];
	
	      for (var c = 0; c < $children.length; c++) {
	        var $child = $($children[c]);
	
	        var child = this.item($child);
	
	        children.push(child);
	      }
	
	      data.children = children;
	    }
	
	    data = this._normalizeItem(data);
	    data.element = $option[0];
	
	    $.data($option[0], 'data', data);
	
	    return data;
	  };
	
	  SelectAdapter.prototype._normalizeItem = function (item) {
	    if (!$.isPlainObject(item)) {
	      item = {
	        id: item,
	        text: item
	      };
	    }
	
	    item = $.extend({}, {
	      text: ''
	    }, item);
	
	    var defaults = {
	      selected: false,
	      disabled: false
	    };
	
	    if (item.id != null) {
	      item.id = item.id.toString();
	    }
	
	    if (item.text != null) {
	      item.text = item.text.toString();
	    }
	
	    if (item._resultId == null && item.id && this.container != null) {
	      item._resultId = this.generateResultId(this.container, item);
	    }
	
	    return $.extend({}, defaults, item);
	  };
	
	  SelectAdapter.prototype.matches = function (params, data) {
	    var matcher = this.options.get('matcher');
	
	    return matcher(params, data);
	  };
	
	  return SelectAdapter;
	});
	
	S2.define('select2/data/array',[
	  './select',
	  '../utils',
	  'jquery'
	], function (SelectAdapter, Utils, $) {
	  function ArrayAdapter ($element, options) {
	    var data = options.get('data') || [];
	
	    ArrayAdapter.__super__.constructor.call(this, $element, options);
	
	    this.addOptions(this.convertToOptions(data));
	  }
	
	  Utils.Extend(ArrayAdapter, SelectAdapter);
	
	  ArrayAdapter.prototype.select = function (data) {
	    var $option = this.$element.find('option').filter(function (i, elm) {
	      return elm.value == data.id.toString();
	    });
	
	    if ($option.length === 0) {
	      $option = this.option(data);
	
	      this.addOptions($option);
	    }
	
	    ArrayAdapter.__super__.select.call(this, data);
	  };
	
	  ArrayAdapter.prototype.convertToOptions = function (data) {
	    var self = this;
	
	    var $existing = this.$element.find('option');
	    var existingIds = $existing.map(function () {
	      return self.item($(this)).id;
	    }).get();
	
	    var $options = [];
	
	    // Filter out all items except for the one passed in the argument
	    function onlyItem (item) {
	      return function () {
	        return $(this).val() == item.id;
	      };
	    }
	
	    for (var d = 0; d < data.length; d++) {
	      var item = this._normalizeItem(data[d]);
	
	      // Skip items which were pre-loaded, only merge the data
	      if ($.inArray(item.id, existingIds) >= 0) {
	        var $existingOption = $existing.filter(onlyItem(item));
	
	        var existingData = this.item($existingOption);
	        var newData = $.extend(true, {}, item, existingData);
	
	        var $newOption = this.option(newData);
	
	        $existingOption.replaceWith($newOption);
	
	        continue;
	      }
	
	      var $option = this.option(item);
	
	      if (item.children) {
	        var $children = this.convertToOptions(item.children);
	
	        Utils.appendMany($option, $children);
	      }
	
	      $options.push($option);
	    }
	
	    return $options;
	  };
	
	  return ArrayAdapter;
	});
	
	S2.define('select2/data/ajax',[
	  './array',
	  '../utils',
	  'jquery'
	], function (ArrayAdapter, Utils, $) {
	  function AjaxAdapter ($element, options) {
	    this.ajaxOptions = this._applyDefaults(options.get('ajax'));
	
	    if (this.ajaxOptions.processResults != null) {
	      this.processResults = this.ajaxOptions.processResults;
	    }
	
	    AjaxAdapter.__super__.constructor.call(this, $element, options);
	  }
	
	  Utils.Extend(AjaxAdapter, ArrayAdapter);
	
	  AjaxAdapter.prototype._applyDefaults = function (options) {
	    var defaults = {
	      data: function (params) {
	        return $.extend({}, params, {
	          q: params.term
	        });
	      },
	      transport: function (params, success, failure) {
	        var $request = $.ajax(params);
	
	        $request.then(success);
	        $request.fail(failure);
	
	        return $request;
	      }
	    };
	
	    return $.extend({}, defaults, options, true);
	  };
	
	  AjaxAdapter.prototype.processResults = function (results) {
	    return results;
	  };
	
	  AjaxAdapter.prototype.query = function (params, callback) {
	    var matches = [];
	    var self = this;
	
	    if (this._request != null) {
	      // JSONP requests cannot always be aborted
	      if ($.isFunction(this._request.abort)) {
	        this._request.abort();
	      }
	
	      this._request = null;
	    }
	
	    var options = $.extend({
	      type: 'GET'
	    }, this.ajaxOptions);
	
	    if (typeof options.url === 'function') {
	      options.url = options.url.call(this.$element, params);
	    }
	
	    if (typeof options.data === 'function') {
	      options.data = options.data.call(this.$element, params);
	    }
	
	    function request () {
	      var $request = options.transport(options, function (data) {
	        var results = self.processResults(data, params);
	
	        if (self.options.get('debug') && window.console && console.error) {
	          // Check to make sure that the response included a `results` key.
	          if (!results || !results.results || !$.isArray(results.results)) {
	            console.error(
	              'Select2: The AJAX results did not return an array in the ' +
	              '`results` key of the response.'
	            );
	          }
	        }
	
	        callback(results);
	      }, function () {
	        // Attempt to detect if a request was aborted
	        // Only works if the transport exposes a status property
	        if ($request.status && $request.status === '0') {
	          return;
	        }
	
	        self.trigger('results:message', {
	          message: 'errorLoading'
	        });
	      });
	
	      self._request = $request;
	    }
	
	    if (this.ajaxOptions.delay && params.term != null) {
	      if (this._queryTimeout) {
	        window.clearTimeout(this._queryTimeout);
	      }
	
	      this._queryTimeout = window.setTimeout(request, this.ajaxOptions.delay);
	    } else {
	      request();
	    }
	  };
	
	  return AjaxAdapter;
	});
	
	S2.define('select2/data/tags',[
	  'jquery'
	], function ($) {
	  function Tags (decorated, $element, options) {
	    var tags = options.get('tags');
	
	    var createTag = options.get('createTag');
	
	    if (createTag !== undefined) {
	      this.createTag = createTag;
	    }
	
	    var insertTag = options.get('insertTag');
	
	    if (insertTag !== undefined) {
	        this.insertTag = insertTag;
	    }
	
	    decorated.call(this, $element, options);
	
	    if ($.isArray(tags)) {
	      for (var t = 0; t < tags.length; t++) {
	        var tag = tags[t];
	        var item = this._normalizeItem(tag);
	
	        var $option = this.option(item);
	
	        this.$element.append($option);
	      }
	    }
	  }
	
	  Tags.prototype.query = function (decorated, params, callback) {
	    var self = this;
	
	    this._removeOldTags();
	
	    if (params.term == null || params.page != null) {
	      decorated.call(this, params, callback);
	      return;
	    }
	
	    function wrapper (obj, child) {
	      var data = obj.results;
	
	      for (var i = 0; i < data.length; i++) {
	        var option = data[i];
	
	        var checkChildren = (
	          option.children != null &&
	          !wrapper({
	            results: option.children
	          }, true)
	        );
	
	        var optionText = (option.text || '').toUpperCase();
	        var paramsTerm = (params.term || '').toUpperCase();
	
	        var checkText = optionText === paramsTerm;
	
	        if (checkText || checkChildren) {
	          if (child) {
	            return false;
	          }
	
	          obj.data = data;
	          callback(obj);
	
	          return;
	        }
	      }
	
	      if (child) {
	        return true;
	      }
	
	      var tag = self.createTag(params);
	
	      if (tag != null) {
	        var $option = self.option(tag);
	        $option.attr('data-select2-tag', true);
	
	        self.addOptions([$option]);
	
	        self.insertTag(data, tag);
	      }
	
	      obj.results = data;
	
	      callback(obj);
	    }
	
	    decorated.call(this, params, wrapper);
	  };
	
	  Tags.prototype.createTag = function (decorated, params) {
	    var term = $.trim(params.term);
	
	    if (term === '') {
	      return null;
	    }
	
	    return {
	      id: term,
	      text: term
	    };
	  };
	
	  Tags.prototype.insertTag = function (_, data, tag) {
	    data.unshift(tag);
	  };
	
	  Tags.prototype._removeOldTags = function (_) {
	    var tag = this._lastTag;
	
	    var $options = this.$element.find('option[data-select2-tag]');
	
	    $options.each(function () {
	      if (this.selected) {
	        return;
	      }
	
	      $(this).remove();
	    });
	  };
	
	  return Tags;
	});
	
	S2.define('select2/data/tokenizer',[
	  'jquery'
	], function ($) {
	  function Tokenizer (decorated, $element, options) {
	    var tokenizer = options.get('tokenizer');
	
	    if (tokenizer !== undefined) {
	      this.tokenizer = tokenizer;
	    }
	
	    decorated.call(this, $element, options);
	  }
	
	  Tokenizer.prototype.bind = function (decorated, container, $container) {
	    decorated.call(this, container, $container);
	
	    this.$search =  container.dropdown.$search || container.selection.$search ||
	      $container.find('.select2-search__field');
	  };
	
	  Tokenizer.prototype.query = function (decorated, params, callback) {
	    var self = this;
	
	    function createAndSelect (data) {
	      // Normalize the data object so we can use it for checks
	      var item = self._normalizeItem(data);
	
	      // Check if the data object already exists as a tag
	      // Select it if it doesn't
	      var $existingOptions = self.$element.find('option').filter(function () {
	        return $(this).val() === item.id;
	      });
	
	      // If an existing option wasn't found for it, create the option
	      if (!$existingOptions.length) {
	        var $option = self.option(item);
	        $option.attr('data-select2-tag', true);
	
	        self._removeOldTags();
	        self.addOptions([$option]);
	      }
	
	      // Select the item, now that we know there is an option for it
	      select(item);
	    }
	
	    function select (data) {
	      self.trigger('select', {
	        data: data
	      });
	    }
	
	    params.term = params.term || '';
	
	    var tokenData = this.tokenizer(params, this.options, createAndSelect);
	
	    if (tokenData.term !== params.term) {
	      // Replace the search term if we have the search box
	      if (this.$search.length) {
	        this.$search.val(tokenData.term);
	        this.$search.focus();
	      }
	
	      params.term = tokenData.term;
	    }
	
	    decorated.call(this, params, callback);
	  };
	
	  Tokenizer.prototype.tokenizer = function (_, params, options, callback) {
	    var separators = options.get('tokenSeparators') || [];
	    var term = params.term;
	    var i = 0;
	
	    var createTag = this.createTag || function (params) {
	      return {
	        id: params.term,
	        text: params.term
	      };
	    };
	
	    while (i < term.length) {
	      var termChar = term[i];
	
	      if ($.inArray(termChar, separators) === -1) {
	        i++;
	
	        continue;
	      }
	
	      var part = term.substr(0, i);
	      var partParams = $.extend({}, params, {
	        term: part
	      });
	
	      var data = createTag(partParams);
	
	      if (data == null) {
	        i++;
	        continue;
	      }
	
	      callback(data);
	
	      // Reset the term to not include the tokenized portion
	      term = term.substr(i + 1) || '';
	      i = 0;
	    }
	
	    return {
	      term: term
	    };
	  };
	
	  return Tokenizer;
	});
	
	S2.define('select2/data/minimumInputLength',[
	
	], function () {
	  function MinimumInputLength (decorated, $e, options) {
	    this.minimumInputLength = options.get('minimumInputLength');
	
	    decorated.call(this, $e, options);
	  }
	
	  MinimumInputLength.prototype.query = function (decorated, params, callback) {
	    params.term = params.term || '';
	
	    if (params.term.length < this.minimumInputLength) {
	      this.trigger('results:message', {
	        message: 'inputTooShort',
	        args: {
	          minimum: this.minimumInputLength,
	          input: params.term,
	          params: params
	        }
	      });
	
	      return;
	    }
	
	    decorated.call(this, params, callback);
	  };
	
	  return MinimumInputLength;
	});
	
	S2.define('select2/data/maximumInputLength',[
	
	], function () {
	  function MaximumInputLength (decorated, $e, options) {
	    this.maximumInputLength = options.get('maximumInputLength');
	
	    decorated.call(this, $e, options);
	  }
	
	  MaximumInputLength.prototype.query = function (decorated, params, callback) {
	    params.term = params.term || '';
	
	    if (this.maximumInputLength > 0 &&
	        params.term.length > this.maximumInputLength) {
	      this.trigger('results:message', {
	        message: 'inputTooLong',
	        args: {
	          maximum: this.maximumInputLength,
	          input: params.term,
	          params: params
	        }
	      });
	
	      return;
	    }
	
	    decorated.call(this, params, callback);
	  };
	
	  return MaximumInputLength;
	});
	
	S2.define('select2/data/maximumSelectionLength',[
	
	], function (){
	  function MaximumSelectionLength (decorated, $e, options) {
	    this.maximumSelectionLength = options.get('maximumSelectionLength');
	
	    decorated.call(this, $e, options);
	  }
	
	  MaximumSelectionLength.prototype.query =
	    function (decorated, params, callback) {
	      var self = this;
	
	      this.current(function (currentData) {
	        var count = currentData != null ? currentData.length : 0;
	        if (self.maximumSelectionLength > 0 &&
	          count >= self.maximumSelectionLength) {
	          self.trigger('results:message', {
	            message: 'maximumSelected',
	            args: {
	              maximum: self.maximumSelectionLength
	            }
	          });
	          return;
	        }
	        decorated.call(self, params, callback);
	      });
	  };
	
	  return MaximumSelectionLength;
	});
	
	S2.define('select2/dropdown',[
	  'jquery',
	  './utils'
	], function ($, Utils) {
	  function Dropdown ($element, options) {
	    this.$element = $element;
	    this.options = options;
	
	    Dropdown.__super__.constructor.call(this);
	  }
	
	  Utils.Extend(Dropdown, Utils.Observable);
	
	  Dropdown.prototype.render = function () {
	    var $dropdown = $(
	      '<span class="select2-dropdown">' +
	        '<span class="select2-results"></span>' +
	      '</span>'
	    );
	
	    $dropdown.attr('dir', this.options.get('dir'));
	
	    this.$dropdown = $dropdown;
	
	    return $dropdown;
	  };
	
	  Dropdown.prototype.bind = function () {
	    // Should be implemented in subclasses
	  };
	
	  Dropdown.prototype.position = function ($dropdown, $container) {
	    // Should be implmented in subclasses
	  };
	
	  Dropdown.prototype.destroy = function () {
	    // Remove the dropdown from the DOM
	    this.$dropdown.remove();
	  };
	
	  return Dropdown;
	});
	
	S2.define('select2/dropdown/search',[
	  'jquery',
	  '../utils'
	], function ($, Utils) {
	  function Search () { }
	
	  Search.prototype.render = function (decorated) {
	    var $rendered = decorated.call(this);
	
	    var $search = $(
	      '<span class="select2-search select2-search--dropdown">' +
	        '<input class="select2-search__field" type="search" tabindex="-1"' +
	        ' autocomplete="off" autocorrect="off" autocapitalize="off"' +
	        ' spellcheck="false" role="textbox" />' +
	      '</span>'
	    );
	
	    this.$searchContainer = $search;
	    this.$search = $search.find('input');
	
	    $rendered.prepend($search);
	
	    return $rendered;
	  };
	
	  Search.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    decorated.call(this, container, $container);
	
	    this.$search.on('keydown', function (evt) {
	      self.trigger('keypress', evt);
	
	      self._keyUpPrevented = evt.isDefaultPrevented();
	    });
	
	    // Workaround for browsers which do not support the `input` event
	    // This will prevent double-triggering of events for browsers which support
	    // both the `keyup` and `input` events.
	    this.$search.on('input', function (evt) {
	      // Unbind the duplicated `keyup` event
	      $(this).off('keyup');
	    });
	
	    this.$search.on('keyup input', function (evt) {
	      self.handleSearch(evt);
	    });
	
	    container.on('open', function () {
	      self.$search.attr('tabindex', 0);
	
	      self.$search.focus();
	
	      window.setTimeout(function () {
	        self.$search.focus();
	      }, 0);
	    });
	
	    container.on('close', function () {
	      self.$search.attr('tabindex', -1);
	
	      self.$search.val('');
	    });
	
	    container.on('focus', function () {
	      if (container.isOpen()) {
	        self.$search.focus();
	      }
	    });
	
	    container.on('results:all', function (params) {
	      if (params.query.term == null || params.query.term === '') {
	        var showSearch = self.showSearch(params);
	
	        if (showSearch) {
	          self.$searchContainer.removeClass('select2-search--hide');
	        } else {
	          self.$searchContainer.addClass('select2-search--hide');
	        }
	      }
	    });
	  };
	
	  Search.prototype.handleSearch = function (evt) {
	    if (!this._keyUpPrevented) {
	      var input = this.$search.val();
	
	      this.trigger('query', {
	        term: input
	      });
	    }
	
	    this._keyUpPrevented = false;
	  };
	
	  Search.prototype.showSearch = function (_, params) {
	    return true;
	  };
	
	  return Search;
	});
	
	S2.define('select2/dropdown/hidePlaceholder',[
	
	], function () {
	  function HidePlaceholder (decorated, $element, options, dataAdapter) {
	    this.placeholder = this.normalizePlaceholder(options.get('placeholder'));
	
	    decorated.call(this, $element, options, dataAdapter);
	  }
	
	  HidePlaceholder.prototype.append = function (decorated, data) {
	    data.results = this.removePlaceholder(data.results);
	
	    decorated.call(this, data);
	  };
	
	  HidePlaceholder.prototype.normalizePlaceholder = function (_, placeholder) {
	    if (typeof placeholder === 'string') {
	      placeholder = {
	        id: '',
	        text: placeholder
	      };
	    }
	
	    return placeholder;
	  };
	
	  HidePlaceholder.prototype.removePlaceholder = function (_, data) {
	    var modifiedData = data.slice(0);
	
	    for (var d = data.length - 1; d >= 0; d--) {
	      var item = data[d];
	
	      if (this.placeholder.id === item.id) {
	        modifiedData.splice(d, 1);
	      }
	    }
	
	    return modifiedData;
	  };
	
	  return HidePlaceholder;
	});
	
	S2.define('select2/dropdown/infiniteScroll',[
	  'jquery'
	], function ($) {
	  function InfiniteScroll (decorated, $element, options, dataAdapter) {
	    this.lastParams = {};
	
	    decorated.call(this, $element, options, dataAdapter);
	
	    this.$loadingMore = this.createLoadingMore();
	    this.loading = false;
	  }
	
	  InfiniteScroll.prototype.append = function (decorated, data) {
	    this.$loadingMore.remove();
	    this.loading = false;
	
	    decorated.call(this, data);
	
	    if (this.showLoadingMore(data)) {
	      this.$results.append(this.$loadingMore);
	    }
	  };
	
	  InfiniteScroll.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    decorated.call(this, container, $container);
	
	    container.on('query', function (params) {
	      self.lastParams = params;
	      self.loading = true;
	    });
	
	    container.on('query:append', function (params) {
	      self.lastParams = params;
	      self.loading = true;
	    });
	
	    this.$results.on('scroll', function () {
	      var isLoadMoreVisible = $.contains(
	        document.documentElement,
	        self.$loadingMore[0]
	      );
	
	      if (self.loading || !isLoadMoreVisible) {
	        return;
	      }
	
	      var currentOffset = self.$results.offset().top +
	        self.$results.outerHeight(false);
	      var loadingMoreOffset = self.$loadingMore.offset().top +
	        self.$loadingMore.outerHeight(false);
	
	      if (currentOffset + 50 >= loadingMoreOffset) {
	        self.loadMore();
	      }
	    });
	  };
	
	  InfiniteScroll.prototype.loadMore = function () {
	    this.loading = true;
	
	    var params = $.extend({}, {page: 1}, this.lastParams);
	
	    params.page++;
	
	    this.trigger('query:append', params);
	  };
	
	  InfiniteScroll.prototype.showLoadingMore = function (_, data) {
	    return data.pagination && data.pagination.more;
	  };
	
	  InfiniteScroll.prototype.createLoadingMore = function () {
	    var $option = $(
	      '<li ' +
	      'class="select2-results__option select2-results__option--load-more"' +
	      'role="treeitem" aria-disabled="true"></li>'
	    );
	
	    var message = this.options.get('translations').get('loadingMore');
	
	    $option.html(message(this.lastParams));
	
	    return $option;
	  };
	
	  return InfiniteScroll;
	});
	
	S2.define('select2/dropdown/attachBody',[
	  'jquery',
	  '../utils'
	], function ($, Utils) {
	  function AttachBody (decorated, $element, options) {
	    this.$dropdownParent = options.get('dropdownParent') || $(document.body);
	
	    decorated.call(this, $element, options);
	  }
	
	  AttachBody.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    var setupResultsEvents = false;
	
	    decorated.call(this, container, $container);
	
	    container.on('open', function () {
	      self._showDropdown();
	      self._attachPositioningHandler(container);
	
	      if (!setupResultsEvents) {
	        setupResultsEvents = true;
	
	        container.on('results:all', function () {
	          self._positionDropdown();
	          self._resizeDropdown();
	        });
	
	        container.on('results:append', function () {
	          self._positionDropdown();
	          self._resizeDropdown();
	        });
	      }
	    });
	
	    container.on('close', function () {
	      self._hideDropdown();
	      self._detachPositioningHandler(container);
	    });
	
	    this.$dropdownContainer.on('mousedown', function (evt) {
	      evt.stopPropagation();
	    });
	  };
	
	  AttachBody.prototype.destroy = function (decorated) {
	    decorated.call(this);
	
	    this.$dropdownContainer.remove();
	  };
	
	  AttachBody.prototype.position = function (decorated, $dropdown, $container) {
	    // Clone all of the container classes
	    $dropdown.attr('class', $container.attr('class'));
	
	    $dropdown.removeClass('select2');
	    $dropdown.addClass('select2-container--open');
	
	    $dropdown.css({
	      position: 'absolute',
	      top: -999999
	    });
	
	    this.$container = $container;
	  };
	
	  AttachBody.prototype.render = function (decorated) {
	    var $container = $('<span></span>');
	
	    var $dropdown = decorated.call(this);
	    $container.append($dropdown);
	
	    this.$dropdownContainer = $container;
	
	    return $container;
	  };
	
	  AttachBody.prototype._hideDropdown = function (decorated) {
	    this.$dropdownContainer.detach();
	  };
	
	  AttachBody.prototype._attachPositioningHandler =
	      function (decorated, container) {
	    var self = this;
	
	    var scrollEvent = 'scroll.select2.' + container.id;
	    var resizeEvent = 'resize.select2.' + container.id;
	    var orientationEvent = 'orientationchange.select2.' + container.id;
	
	    var $watchers = this.$container.parents().filter(Utils.hasScroll);
	    $watchers.each(function () {
	      $(this).data('select2-scroll-position', {
	        x: $(this).scrollLeft(),
	        y: $(this).scrollTop()
	      });
	    });
	
	    $watchers.on(scrollEvent, function (ev) {
	      var position = $(this).data('select2-scroll-position');
	      $(this).scrollTop(position.y);
	    });
	
	    $(window).on(scrollEvent + ' ' + resizeEvent + ' ' + orientationEvent,
	      function (e) {
	      self._positionDropdown();
	      self._resizeDropdown();
	    });
	  };
	
	  AttachBody.prototype._detachPositioningHandler =
	      function (decorated, container) {
	    var scrollEvent = 'scroll.select2.' + container.id;
	    var resizeEvent = 'resize.select2.' + container.id;
	    var orientationEvent = 'orientationchange.select2.' + container.id;
	
	    var $watchers = this.$container.parents().filter(Utils.hasScroll);
	    $watchers.off(scrollEvent);
	
	    $(window).off(scrollEvent + ' ' + resizeEvent + ' ' + orientationEvent);
	  };
	
	  AttachBody.prototype._positionDropdown = function () {
	    var $window = $(window);
	
	    var isCurrentlyAbove = this.$dropdown.hasClass('select2-dropdown--above');
	    var isCurrentlyBelow = this.$dropdown.hasClass('select2-dropdown--below');
	
	    var newDirection = null;
	
	    var offset = this.$container.offset();
	
	    offset.bottom = offset.top + this.$container.outerHeight(false);
	
	    var container = {
	      height: this.$container.outerHeight(false)
	    };
	
	    container.top = offset.top;
	    container.bottom = offset.top + container.height;
	
	    var dropdown = {
	      height: this.$dropdown.outerHeight(false)
	    };
	
	    var viewport = {
	      top: $window.scrollTop(),
	      bottom: $window.scrollTop() + $window.height()
	    };
	
	    var enoughRoomAbove = viewport.top < (offset.top - dropdown.height);
	    var enoughRoomBelow = viewport.bottom > (offset.bottom + dropdown.height);
	
	    var css = {
	      left: offset.left,
	      top: container.bottom
	    };
	
	    // Determine what the parent element is to use for calciulating the offset
	    var $offsetParent = this.$dropdownParent;
	
	    // For statically positoned elements, we need to get the element
	    // that is determining the offset
	    if ($offsetParent.css('position') === 'static') {
	      $offsetParent = $offsetParent.offsetParent();
	    }
	
	    var parentOffset = $offsetParent.offset();
	
	    css.top -= parentOffset.top;
	    css.left -= parentOffset.left;
	
	    if (!isCurrentlyAbove && !isCurrentlyBelow) {
	      newDirection = 'below';
	    }
	
	    if (!enoughRoomBelow && enoughRoomAbove && !isCurrentlyAbove) {
	      newDirection = 'above';
	    } else if (!enoughRoomAbove && enoughRoomBelow && isCurrentlyAbove) {
	      newDirection = 'below';
	    }
	
	    if (newDirection == 'above' ||
	      (isCurrentlyAbove && newDirection !== 'below')) {
	      css.top = container.top - parentOffset.top - dropdown.height;
	    }
	
	    if (newDirection != null) {
	      this.$dropdown
	        .removeClass('select2-dropdown--below select2-dropdown--above')
	        .addClass('select2-dropdown--' + newDirection);
	      this.$container
	        .removeClass('select2-container--below select2-container--above')
	        .addClass('select2-container--' + newDirection);
	    }
	
	    this.$dropdownContainer.css(css);
	  };
	
	  AttachBody.prototype._resizeDropdown = function () {
	    var css = {
	      width: this.$container.outerWidth(false) + 'px'
	    };
	
	    if (this.options.get('dropdownAutoWidth')) {
	      css.minWidth = css.width;
	      css.position = 'relative';
	      css.width = 'auto';
	    }
	
	    this.$dropdown.css(css);
	  };
	
	  AttachBody.prototype._showDropdown = function (decorated) {
	    this.$dropdownContainer.appendTo(this.$dropdownParent);
	
	    this._positionDropdown();
	    this._resizeDropdown();
	  };
	
	  return AttachBody;
	});
	
	S2.define('select2/dropdown/minimumResultsForSearch',[
	
	], function () {
	  function countResults (data) {
	    var count = 0;
	
	    for (var d = 0; d < data.length; d++) {
	      var item = data[d];
	
	      if (item.children) {
	        count += countResults(item.children);
	      } else {
	        count++;
	      }
	    }
	
	    return count;
	  }
	
	  function MinimumResultsForSearch (decorated, $element, options, dataAdapter) {
	    this.minimumResultsForSearch = options.get('minimumResultsForSearch');
	
	    if (this.minimumResultsForSearch < 0) {
	      this.minimumResultsForSearch = Infinity;
	    }
	
	    decorated.call(this, $element, options, dataAdapter);
	  }
	
	  MinimumResultsForSearch.prototype.showSearch = function (decorated, params) {
	    if (countResults(params.data.results) < this.minimumResultsForSearch) {
	      return false;
	    }
	
	    return decorated.call(this, params);
	  };
	
	  return MinimumResultsForSearch;
	});
	
	S2.define('select2/dropdown/selectOnClose',[
	
	], function () {
	  function SelectOnClose () { }
	
	  SelectOnClose.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    decorated.call(this, container, $container);
	
	    container.on('close', function (params) {
	      self._handleSelectOnClose(params);
	    });
	  };
	
	  SelectOnClose.prototype._handleSelectOnClose = function (_, params) {
	    if (params && params.originalSelect2Event != null) {
	      var event = params.originalSelect2Event;
	
	      // Don't select an item if the close event was triggered from a select or
	      // unselect event
	      if (event._type === 'select' || event._type === 'unselect') {
	        return;
	      }
	    }
	
	    var $highlightedResults = this.getHighlightedResults();
	
	    // Only select highlighted results
	    if ($highlightedResults.length < 1) {
	      return;
	    }
	
	    var data = $highlightedResults.data('data');
	
	    // Don't re-select already selected resulte
	    if (
	      (data.element != null && data.element.selected) ||
	      (data.element == null && data.selected)
	    ) {
	      return;
	    }
	
	    this.trigger('select', {
	        data: data
	    });
	  };
	
	  return SelectOnClose;
	});
	
	S2.define('select2/dropdown/closeOnSelect',[
	
	], function () {
	  function CloseOnSelect () { }
	
	  CloseOnSelect.prototype.bind = function (decorated, container, $container) {
	    var self = this;
	
	    decorated.call(this, container, $container);
	
	    container.on('select', function (evt) {
	      self._selectTriggered(evt);
	    });
	
	    container.on('unselect', function (evt) {
	      self._selectTriggered(evt);
	    });
	  };
	
	  CloseOnSelect.prototype._selectTriggered = function (_, evt) {
	    var originalEvent = evt.originalEvent;
	
	    // Don't close if the control key is being held
	    if (originalEvent && originalEvent.ctrlKey) {
	      return;
	    }
	
	    this.trigger('close', {
	      originalEvent: originalEvent,
	      originalSelect2Event: evt
	    });
	  };
	
	  return CloseOnSelect;
	});
	
	S2.define('select2/i18n/en',[],function () {
	  // English
	  return {
	    errorLoading: function () {
	      return 'The results could not be loaded.';
	    },
	    inputTooLong: function (args) {
	      var overChars = args.input.length - args.maximum;
	
	      var message = 'Please delete ' + overChars + ' character';
	
	      if (overChars != 1) {
	        message += 's';
	      }
	
	      return message;
	    },
	    inputTooShort: function (args) {
	      var remainingChars = args.minimum - args.input.length;
	
	      var message = 'Please enter ' + remainingChars + ' or more characters';
	
	      return message;
	    },
	    loadingMore: function () {
	      return 'Loading more results…';
	    },
	    maximumSelected: function (args) {
	      var message = 'You can only select ' + args.maximum + ' item';
	
	      if (args.maximum != 1) {
	        message += 's';
	      }
	
	      return message;
	    },
	    noResults: function () {
	      return 'No results found';
	    },
	    searching: function () {
	      return 'Searching…';
	    }
	  };
	});
	
	S2.define('select2/defaults',[
	  'jquery',
	  'require',
	
	  './results',
	
	  './selection/single',
	  './selection/multiple',
	  './selection/placeholder',
	  './selection/allowClear',
	  './selection/search',
	  './selection/eventRelay',
	
	  './utils',
	  './translation',
	  './diacritics',
	
	  './data/select',
	  './data/array',
	  './data/ajax',
	  './data/tags',
	  './data/tokenizer',
	  './data/minimumInputLength',
	  './data/maximumInputLength',
	  './data/maximumSelectionLength',
	
	  './dropdown',
	  './dropdown/search',
	  './dropdown/hidePlaceholder',
	  './dropdown/infiniteScroll',
	  './dropdown/attachBody',
	  './dropdown/minimumResultsForSearch',
	  './dropdown/selectOnClose',
	  './dropdown/closeOnSelect',
	
	  './i18n/en'
	], function ($, require,
	
	             ResultsList,
	
	             SingleSelection, MultipleSelection, Placeholder, AllowClear,
	             SelectionSearch, EventRelay,
	
	             Utils, Translation, DIACRITICS,
	
	             SelectData, ArrayData, AjaxData, Tags, Tokenizer,
	             MinimumInputLength, MaximumInputLength, MaximumSelectionLength,
	
	             Dropdown, DropdownSearch, HidePlaceholder, InfiniteScroll,
	             AttachBody, MinimumResultsForSearch, SelectOnClose, CloseOnSelect,
	
	             EnglishTranslation) {
	  function Defaults () {
	    this.reset();
	  }
	
	  Defaults.prototype.apply = function (options) {
	    options = $.extend(true, {}, this.defaults, options);
	
	    if (options.dataAdapter == null) {
	      if (options.ajax != null) {
	        options.dataAdapter = AjaxData;
	      } else if (options.data != null) {
	        options.dataAdapter = ArrayData;
	      } else {
	        options.dataAdapter = SelectData;
	      }
	
	      if (options.minimumInputLength > 0) {
	        options.dataAdapter = Utils.Decorate(
	          options.dataAdapter,
	          MinimumInputLength
	        );
	      }
	
	      if (options.maximumInputLength > 0) {
	        options.dataAdapter = Utils.Decorate(
	          options.dataAdapter,
	          MaximumInputLength
	        );
	      }
	
	      if (options.maximumSelectionLength > 0) {
	        options.dataAdapter = Utils.Decorate(
	          options.dataAdapter,
	          MaximumSelectionLength
	        );
	      }
	
	      if (options.tags) {
	        options.dataAdapter = Utils.Decorate(options.dataAdapter, Tags);
	      }
	
	      if (options.tokenSeparators != null || options.tokenizer != null) {
	        options.dataAdapter = Utils.Decorate(
	          options.dataAdapter,
	          Tokenizer
	        );
	      }
	
	      if (options.query != null) {
	        var Query = require(options.amdBase + 'compat/query');
	
	        options.dataAdapter = Utils.Decorate(
	          options.dataAdapter,
	          Query
	        );
	      }
	
	      if (options.initSelection != null) {
	        var InitSelection = require(options.amdBase + 'compat/initSelection');
	
	        options.dataAdapter = Utils.Decorate(
	          options.dataAdapter,
	          InitSelection
	        );
	      }
	    }
	
	    if (options.resultsAdapter == null) {
	      options.resultsAdapter = ResultsList;
	
	      if (options.ajax != null) {
	        options.resultsAdapter = Utils.Decorate(
	          options.resultsAdapter,
	          InfiniteScroll
	        );
	      }
	
	      if (options.placeholder != null) {
	        options.resultsAdapter = Utils.Decorate(
	          options.resultsAdapter,
	          HidePlaceholder
	        );
	      }
	
	      if (options.selectOnClose) {
	        options.resultsAdapter = Utils.Decorate(
	          options.resultsAdapter,
	          SelectOnClose
	        );
	      }
	    }
	
	    if (options.dropdownAdapter == null) {
	      if (options.multiple) {
	        options.dropdownAdapter = Dropdown;
	      } else {
	        var SearchableDropdown = Utils.Decorate(Dropdown, DropdownSearch);
	
	        options.dropdownAdapter = SearchableDropdown;
	      }
	
	      if (options.minimumResultsForSearch !== 0) {
	        options.dropdownAdapter = Utils.Decorate(
	          options.dropdownAdapter,
	          MinimumResultsForSearch
	        );
	      }
	
	      if (options.closeOnSelect) {
	        options.dropdownAdapter = Utils.Decorate(
	          options.dropdownAdapter,
	          CloseOnSelect
	        );
	      }
	
	      if (
	        options.dropdownCssClass != null ||
	        options.dropdownCss != null ||
	        options.adaptDropdownCssClass != null
	      ) {
	        var DropdownCSS = require(options.amdBase + 'compat/dropdownCss');
	
	        options.dropdownAdapter = Utils.Decorate(
	          options.dropdownAdapter,
	          DropdownCSS
	        );
	      }
	
	      options.dropdownAdapter = Utils.Decorate(
	        options.dropdownAdapter,
	        AttachBody
	      );
	    }
	
	    if (options.selectionAdapter == null) {
	      if (options.multiple) {
	        options.selectionAdapter = MultipleSelection;
	      } else {
	        options.selectionAdapter = SingleSelection;
	      }
	
	      // Add the placeholder mixin if a placeholder was specified
	      if (options.placeholder != null) {
	        options.selectionAdapter = Utils.Decorate(
	          options.selectionAdapter,
	          Placeholder
	        );
	      }
	
	      if (options.allowClear) {
	        options.selectionAdapter = Utils.Decorate(
	          options.selectionAdapter,
	          AllowClear
	        );
	      }
	
	      if (options.multiple) {
	        options.selectionAdapter = Utils.Decorate(
	          options.selectionAdapter,
	          SelectionSearch
	        );
	      }
	
	      if (
	        options.containerCssClass != null ||
	        options.containerCss != null ||
	        options.adaptContainerCssClass != null
	      ) {
	        var ContainerCSS = require(options.amdBase + 'compat/containerCss');
	
	        options.selectionAdapter = Utils.Decorate(
	          options.selectionAdapter,
	          ContainerCSS
	        );
	      }
	
	      options.selectionAdapter = Utils.Decorate(
	        options.selectionAdapter,
	        EventRelay
	      );
	    }
	
	    if (typeof options.language === 'string') {
	      // Check if the language is specified with a region
	      if (options.language.indexOf('-') > 0) {
	        // Extract the region information if it is included
	        var languageParts = options.language.split('-');
	        var baseLanguage = languageParts[0];
	
	        options.language = [options.language, baseLanguage];
	      } else {
	        options.language = [options.language];
	      }
	    }
	
	    if ($.isArray(options.language)) {
	      var languages = new Translation();
	      options.language.push('en');
	
	      var languageNames = options.language;
	
	      for (var l = 0; l < languageNames.length; l++) {
	        var name = languageNames[l];
	        var language = {};
	
	        try {
	          // Try to load it with the original name
	          language = Translation.loadPath(name);
	        } catch (e) {
	          try {
	            // If we couldn't load it, check if it wasn't the full path
	            name = this.defaults.amdLanguageBase + name;
	            language = Translation.loadPath(name);
	          } catch (ex) {
	            // The translation could not be loaded at all. Sometimes this is
	            // because of a configuration problem, other times this can be
	            // because of how Select2 helps load all possible translation files.
	            if (options.debug && window.console && console.warn) {
	              console.warn(
	                'Select2: The language file for "' + name + '" could not be ' +
	                'automatically loaded. A fallback will be used instead.'
	              );
	            }
	
	            continue;
	          }
	        }
	
	        languages.extend(language);
	      }
	
	      options.translations = languages;
	    } else {
	      var baseTranslation = Translation.loadPath(
	        this.defaults.amdLanguageBase + 'en'
	      );
	      var customTranslation = new Translation(options.language);
	
	      customTranslation.extend(baseTranslation);
	
	      options.translations = customTranslation;
	    }
	
	    return options;
	  };
	
	  Defaults.prototype.reset = function () {
	    function stripDiacritics (text) {
	      // Used 'uni range + named function' from http://jsperf.com/diacritics/18
	      function match(a) {
	        return DIACRITICS[a] || a;
	      }
	
	      return text.replace(/[^\u0000-\u007E]/g, match);
	    }
	
	    function matcher (params, data) {
	      // Always return the object if there is nothing to compare
	      if ($.trim(params.term) === '') {
	        return data;
	      }
	
	      // Do a recursive check for options with children
	      if (data.children && data.children.length > 0) {
	        // Clone the data object if there are children
	        // This is required as we modify the object to remove any non-matches
	        var match = $.extend(true, {}, data);
	
	        // Check each child of the option
	        for (var c = data.children.length - 1; c >= 0; c--) {
	          var child = data.children[c];
	
	          var matches = matcher(params, child);
	
	          // If there wasn't a match, remove the object in the array
	          if (matches == null) {
	            match.children.splice(c, 1);
	          }
	        }
	
	        // If any children matched, return the new object
	        if (match.children.length > 0) {
	          return match;
	        }
	
	        // If there were no matching children, check just the plain object
	        return matcher(params, match);
	      }
	
	      var original = stripDiacritics(data.text).toUpperCase();
	      var term = stripDiacritics(params.term).toUpperCase();
	
	      // Check if the text contains the term
	      if (original.indexOf(term) > -1) {
	        return data;
	      }
	
	      // If it doesn't contain the term, don't return anything
	      return null;
	    }
	
	    this.defaults = {
	      amdBase: './',
	      amdLanguageBase: './i18n/',
	      closeOnSelect: true,
	      debug: false,
	      dropdownAutoWidth: false,
	      escapeMarkup: Utils.escapeMarkup,
	      language: EnglishTranslation,
	      matcher: matcher,
	      minimumInputLength: 0,
	      maximumInputLength: 0,
	      maximumSelectionLength: 0,
	      minimumResultsForSearch: 0,
	      selectOnClose: false,
	      sorter: function (data) {
	        return data;
	      },
	      templateResult: function (result) {
	        return result.text;
	      },
	      templateSelection: function (selection) {
	        return selection.text;
	      },
	      theme: 'default',
	      width: 'resolve'
	    };
	  };
	
	  Defaults.prototype.set = function (key, value) {
	    var camelKey = $.camelCase(key);
	
	    var data = {};
	    data[camelKey] = value;
	
	    var convertedData = Utils._convertData(data);
	
	    $.extend(this.defaults, convertedData);
	  };
	
	  var defaults = new Defaults();
	
	  return defaults;
	});
	
	S2.define('select2/options',[
	  'require',
	  'jquery',
	  './defaults',
	  './utils'
	], function (require, $, Defaults, Utils) {
	  function Options (options, $element) {
	    this.options = options;
	
	    if ($element != null) {
	      this.fromElement($element);
	    }
	
	    this.options = Defaults.apply(this.options);
	
	    if ($element && $element.is('input')) {
	      var InputCompat = require(this.get('amdBase') + 'compat/inputData');
	
	      this.options.dataAdapter = Utils.Decorate(
	        this.options.dataAdapter,
	        InputCompat
	      );
	    }
	  }
	
	  Options.prototype.fromElement = function ($e) {
	    var excludedData = ['select2'];
	
	    if (this.options.multiple == null) {
	      this.options.multiple = $e.prop('multiple');
	    }
	
	    if (this.options.disabled == null) {
	      this.options.disabled = $e.prop('disabled');
	    }
	
	    if (this.options.language == null) {
	      if ($e.prop('lang')) {
	        this.options.language = $e.prop('lang').toLowerCase();
	      } else if ($e.closest('[lang]').prop('lang')) {
	        this.options.language = $e.closest('[lang]').prop('lang');
	      }
	    }
	
	    if (this.options.dir == null) {
	      if ($e.prop('dir')) {
	        this.options.dir = $e.prop('dir');
	      } else if ($e.closest('[dir]').prop('dir')) {
	        this.options.dir = $e.closest('[dir]').prop('dir');
	      } else {
	        this.options.dir = 'ltr';
	      }
	    }
	
	    $e.prop('disabled', this.options.disabled);
	    $e.prop('multiple', this.options.multiple);
	
	    if ($e.data('select2Tags')) {
	      if (this.options.debug && window.console && console.warn) {
	        console.warn(
	          'Select2: The `data-select2-tags` attribute has been changed to ' +
	          'use the `data-data` and `data-tags="true"` attributes and will be ' +
	          'removed in future versions of Select2.'
	        );
	      }
	
	      $e.data('data', $e.data('select2Tags'));
	      $e.data('tags', true);
	    }
	
	    if ($e.data('ajaxUrl')) {
	      if (this.options.debug && window.console && console.warn) {
	        console.warn(
	          'Select2: The `data-ajax-url` attribute has been changed to ' +
	          '`data-ajax--url` and support for the old attribute will be removed' +
	          ' in future versions of Select2.'
	        );
	      }
	
	      $e.attr('ajax--url', $e.data('ajaxUrl'));
	      $e.data('ajax--url', $e.data('ajaxUrl'));
	    }
	
	    var dataset = {};
	
	    // Prefer the element's `dataset` attribute if it exists
	    // jQuery 1.x does not correctly handle data attributes with multiple dashes
	    if ($.fn.jquery && $.fn.jquery.substr(0, 2) == '1.' && $e[0].dataset) {
	      dataset = $.extend(true, {}, $e[0].dataset, $e.data());
	    } else {
	      dataset = $e.data();
	    }
	
	    var data = $.extend(true, {}, dataset);
	
	    data = Utils._convertData(data);
	
	    for (var key in data) {
	      if ($.inArray(key, excludedData) > -1) {
	        continue;
	      }
	
	      if ($.isPlainObject(this.options[key])) {
	        $.extend(this.options[key], data[key]);
	      } else {
	        this.options[key] = data[key];
	      }
	    }
	
	    return this;
	  };
	
	  Options.prototype.get = function (key) {
	    return this.options[key];
	  };
	
	  Options.prototype.set = function (key, val) {
	    this.options[key] = val;
	  };
	
	  return Options;
	});
	
	S2.define('select2/core',[
	  'jquery',
	  './options',
	  './utils',
	  './keys'
	], function ($, Options, Utils, KEYS) {
	  var Select2 = function ($element, options) {
	    if ($element.data('select2') != null) {
	      $element.data('select2').destroy();
	    }
	
	    this.$element = $element;
	
	    this.id = this._generateId($element);
	
	    options = options || {};
	
	    this.options = new Options(options, $element);
	
	    Select2.__super__.constructor.call(this);
	
	    // Set up the tabindex
	
	    var tabindex = $element.attr('tabindex') || 0;
	    $element.data('old-tabindex', tabindex);
	    $element.attr('tabindex', '-1');
	
	    // Set up containers and adapters
	
	    var DataAdapter = this.options.get('dataAdapter');
	    this.dataAdapter = new DataAdapter($element, this.options);
	
	    var $container = this.render();
	
	    this._placeContainer($container);
	
	    var SelectionAdapter = this.options.get('selectionAdapter');
	    this.selection = new SelectionAdapter($element, this.options);
	    this.$selection = this.selection.render();
	
	    this.selection.position(this.$selection, $container);
	
	    var DropdownAdapter = this.options.get('dropdownAdapter');
	    this.dropdown = new DropdownAdapter($element, this.options);
	    this.$dropdown = this.dropdown.render();
	
	    this.dropdown.position(this.$dropdown, $container);
	
	    var ResultsAdapter = this.options.get('resultsAdapter');
	    this.results = new ResultsAdapter($element, this.options, this.dataAdapter);
	    this.$results = this.results.render();
	
	    this.results.position(this.$results, this.$dropdown);
	
	    // Bind events
	
	    var self = this;
	
	    // Bind the container to all of the adapters
	    this._bindAdapters();
	
	    // Register any DOM event handlers
	    this._registerDomEvents();
	
	    // Register any internal event handlers
	    this._registerDataEvents();
	    this._registerSelectionEvents();
	    this._registerDropdownEvents();
	    this._registerResultsEvents();
	    this._registerEvents();
	
	    // Set the initial state
	    this.dataAdapter.current(function (initialData) {
	      self.trigger('selection:update', {
	        data: initialData
	      });
	    });
	
	    // Hide the original select
	    $element.addClass('select2-hidden-accessible');
	    $element.attr('aria-hidden', 'true');
	
	    // Synchronize any monitored attributes
	    this._syncAttributes();
	
	    $element.data('select2', this);
	  };
	
	  Utils.Extend(Select2, Utils.Observable);
	
	  Select2.prototype._generateId = function ($element) {
	    var id = '';
	
	    if ($element.attr('id') != null) {
	      id = $element.attr('id');
	    } else if ($element.attr('name') != null) {
	      id = $element.attr('name') + '-' + Utils.generateChars(2);
	    } else {
	      id = Utils.generateChars(4);
	    }
	
	    id = id.replace(/(:|\.|\[|\]|,)/g, '');
	    id = 'select2-' + id;
	
	    return id;
	  };
	
	  Select2.prototype._placeContainer = function ($container) {
	    $container.insertAfter(this.$element);
	
	    var width = this._resolveWidth(this.$element, this.options.get('width'));
	
	    if (width != null) {
	      $container.css('width', width);
	    }
	  };
	
	  Select2.prototype._resolveWidth = function ($element, method) {
	    var WIDTH = /^width:(([-+]?([0-9]*\.)?[0-9]+)(px|em|ex|%|in|cm|mm|pt|pc))/i;
	
	    if (method == 'resolve') {
	      var styleWidth = this._resolveWidth($element, 'style');
	
	      if (styleWidth != null) {
	        return styleWidth;
	      }
	
	      return this._resolveWidth($element, 'element');
	    }
	
	    if (method == 'element') {
	      var elementWidth = $element.outerWidth(false);
	
	      if (elementWidth <= 0) {
	        return 'auto';
	      }
	
	      return elementWidth + 'px';
	    }
	
	    if (method == 'style') {
	      var style = $element.attr('style');
	
	      if (typeof(style) !== 'string') {
	        return null;
	      }
	
	      var attrs = style.split(';');
	
	      for (var i = 0, l = attrs.length; i < l; i = i + 1) {
	        var attr = attrs[i].replace(/\s/g, '');
	        var matches = attr.match(WIDTH);
	
	        if (matches !== null && matches.length >= 1) {
	          return matches[1];
	        }
	      }
	
	      return null;
	    }
	
	    return method;
	  };
	
	  Select2.prototype._bindAdapters = function () {
	    this.dataAdapter.bind(this, this.$container);
	    this.selection.bind(this, this.$container);
	
	    this.dropdown.bind(this, this.$container);
	    this.results.bind(this, this.$container);
	  };
	
	  Select2.prototype._registerDomEvents = function () {
	    var self = this;
	
	    this.$element.on('change.select2', function () {
	      self.dataAdapter.current(function (data) {
	        self.trigger('selection:update', {
	          data: data
	        });
	      });
	    });
	
	    this.$element.on('focus.select2', function (evt) {
	      self.trigger('focus', evt);
	    });
	
	    this._syncA = Utils.bind(this._syncAttributes, this);
	    this._syncS = Utils.bind(this._syncSubtree, this);
	
	    if (this.$element[0].attachEvent) {
	      this.$element[0].attachEvent('onpropertychange', this._syncA);
	    }
	
	    var observer = window.MutationObserver ||
	      window.WebKitMutationObserver ||
	      window.MozMutationObserver
	    ;
	
	    if (observer != null) {
	      this._observer = new observer(function (mutations) {
	        $.each(mutations, self._syncA);
	        $.each(mutations, self._syncS);
	      });
	      this._observer.observe(this.$element[0], {
	        attributes: true,
	        childList: true,
	        subtree: false
	      });
	    } else if (this.$element[0].addEventListener) {
	      this.$element[0].addEventListener(
	        'DOMAttrModified',
	        self._syncA,
	        false
	      );
	      this.$element[0].addEventListener(
	        'DOMNodeInserted',
	        self._syncS,
	        false
	      );
	      this.$element[0].addEventListener(
	        'DOMNodeRemoved',
	        self._syncS,
	        false
	      );
	    }
	  };
	
	  Select2.prototype._registerDataEvents = function () {
	    var self = this;
	
	    this.dataAdapter.on('*', function (name, params) {
	      self.trigger(name, params);
	    });
	  };
	
	  Select2.prototype._registerSelectionEvents = function () {
	    var self = this;
	    var nonRelayEvents = ['toggle', 'focus'];
	
	    this.selection.on('toggle', function () {
	      self.toggleDropdown();
	    });
	
	    this.selection.on('focus', function (params) {
	      self.focus(params);
	    });
	
	    this.selection.on('*', function (name, params) {
	      if ($.inArray(name, nonRelayEvents) !== -1) {
	        return;
	      }
	
	      self.trigger(name, params);
	    });
	  };
	
	  Select2.prototype._registerDropdownEvents = function () {
	    var self = this;
	
	    this.dropdown.on('*', function (name, params) {
	      self.trigger(name, params);
	    });
	  };
	
	  Select2.prototype._registerResultsEvents = function () {
	    var self = this;
	
	    this.results.on('*', function (name, params) {
	      self.trigger(name, params);
	    });
	  };
	
	  Select2.prototype._registerEvents = function () {
	    var self = this;
	
	    this.on('open', function () {
	      self.$container.addClass('select2-container--open');
	    });
	
	    this.on('close', function () {
	      self.$container.removeClass('select2-container--open');
	    });
	
	    this.on('enable', function () {
	      self.$container.removeClass('select2-container--disabled');
	    });
	
	    this.on('disable', function () {
	      self.$container.addClass('select2-container--disabled');
	    });
	
	    this.on('blur', function () {
	      self.$container.removeClass('select2-container--focus');
	    });
	
	    this.on('query', function (params) {
	      if (!self.isOpen()) {
	        self.trigger('open', {});
	      }
	
	      this.dataAdapter.query(params, function (data) {
	        self.trigger('results:all', {
	          data: data,
	          query: params
	        });
	      });
	    });
	
	    this.on('query:append', function (params) {
	      this.dataAdapter.query(params, function (data) {
	        self.trigger('results:append', {
	          data: data,
	          query: params
	        });
	      });
	    });
	
	    this.on('keypress', function (evt) {
	      var key = evt.which;
	
	      if (self.isOpen()) {
	        if (key === KEYS.ESC || key === KEYS.TAB ||
	            (key === KEYS.UP && evt.altKey)) {
	          self.close();
	
	          evt.preventDefault();
	        } else if (key === KEYS.ENTER) {
	          self.trigger('results:select', {});
	
	          evt.preventDefault();
	        } else if ((key === KEYS.SPACE && evt.ctrlKey)) {
	          self.trigger('results:toggle', {});
	
	          evt.preventDefault();
	        } else if (key === KEYS.UP) {
	          self.trigger('results:previous', {});
	
	          evt.preventDefault();
	        } else if (key === KEYS.DOWN) {
	          self.trigger('results:next', {});
	
	          evt.preventDefault();
	        }
	      } else {
	        if (key === KEYS.ENTER || key === KEYS.SPACE ||
	            (key === KEYS.DOWN && evt.altKey)) {
	          self.open();
	
	          evt.preventDefault();
	        }
	      }
	    });
	  };
	
	  Select2.prototype._syncAttributes = function () {
	    this.options.set('disabled', this.$element.prop('disabled'));
	
	    if (this.options.get('disabled')) {
	      if (this.isOpen()) {
	        this.close();
	      }
	
	      this.trigger('disable', {});
	    } else {
	      this.trigger('enable', {});
	    }
	  };
	
	  Select2.prototype._syncSubtree = function (evt, mutations) {
	    var changed = false;
	    var self = this;
	
	    // Ignore any mutation events raised for elements that aren't options or
	    // optgroups. This handles the case when the select element is destroyed
	    if (
	      evt && evt.target && (
	        evt.target.nodeName !== 'OPTION' && evt.target.nodeName !== 'OPTGROUP'
	      )
	    ) {
	      return;
	    }
	
	    if (!mutations) {
	      // If mutation events aren't supported, then we can only assume that the
	      // change affected the selections
	      changed = true;
	    } else if (mutations.addedNodes && mutations.addedNodes.length > 0) {
	      for (var n = 0; n < mutations.addedNodes.length; n++) {
	        var node = mutations.addedNodes[n];
	
	        if (node.selected) {
	          changed = true;
	        }
	      }
	    } else if (mutations.removedNodes && mutations.removedNodes.length > 0) {
	      changed = true;
	    }
	
	    // Only re-pull the data if we think there is a change
	    if (changed) {
	      this.dataAdapter.current(function (currentData) {
	        self.trigger('selection:update', {
	          data: currentData
	        });
	      });
	    }
	  };
	
	  /**
	   * Override the trigger method to automatically trigger pre-events when
	   * there are events that can be prevented.
	   */
	  Select2.prototype.trigger = function (name, args) {
	    var actualTrigger = Select2.__super__.trigger;
	    var preTriggerMap = {
	      'open': 'opening',
	      'close': 'closing',
	      'select': 'selecting',
	      'unselect': 'unselecting'
	    };
	
	    if (args === undefined) {
	      args = {};
	    }
	
	    if (name in preTriggerMap) {
	      var preTriggerName = preTriggerMap[name];
	      var preTriggerArgs = {
	        prevented: false,
	        name: name,
	        args: args
	      };
	
	      actualTrigger.call(this, preTriggerName, preTriggerArgs);
	
	      if (preTriggerArgs.prevented) {
	        args.prevented = true;
	
	        return;
	      }
	    }
	
	    actualTrigger.call(this, name, args);
	  };
	
	  Select2.prototype.toggleDropdown = function () {
	    if (this.options.get('disabled')) {
	      return;
	    }
	
	    if (this.isOpen()) {
	      this.close();
	    } else {
	      this.open();
	    }
	  };
	
	  Select2.prototype.open = function () {
	    if (this.isOpen()) {
	      return;
	    }
	
	    this.trigger('query', {});
	  };
	
	  Select2.prototype.close = function () {
	    if (!this.isOpen()) {
	      return;
	    }
	
	    this.trigger('close', {});
	  };
	
	  Select2.prototype.isOpen = function () {
	    return this.$container.hasClass('select2-container--open');
	  };
	
	  Select2.prototype.hasFocus = function () {
	    return this.$container.hasClass('select2-container--focus');
	  };
	
	  Select2.prototype.focus = function (data) {
	    // No need to re-trigger focus events if we are already focused
	    if (this.hasFocus()) {
	      return;
	    }
	
	    this.$container.addClass('select2-container--focus');
	    this.trigger('focus', {});
	  };
	
	  Select2.prototype.enable = function (args) {
	    if (this.options.get('debug') && window.console && console.warn) {
	      console.warn(
	        'Select2: The `select2("enable")` method has been deprecated and will' +
	        ' be removed in later Select2 versions. Use $element.prop("disabled")' +
	        ' instead.'
	      );
	    }
	
	    if (args == null || args.length === 0) {
	      args = [true];
	    }
	
	    var disabled = !args[0];
	
	    this.$element.prop('disabled', disabled);
	  };
	
	  Select2.prototype.data = function () {
	    if (this.options.get('debug') &&
	        arguments.length > 0 && window.console && console.warn) {
	      console.warn(
	        'Select2: Data can no longer be set using `select2("data")`. You ' +
	        'should consider setting the value instead using `$element.val()`.'
	      );
	    }
	
	    var data = [];
	
	    this.dataAdapter.current(function (currentData) {
	      data = currentData;
	    });
	
	    return data;
	  };
	
	  Select2.prototype.val = function (args) {
	    if (this.options.get('debug') && window.console && console.warn) {
	      console.warn(
	        'Select2: The `select2("val")` method has been deprecated and will be' +
	        ' removed in later Select2 versions. Use $element.val() instead.'
	      );
	    }
	
	    if (args == null || args.length === 0) {
	      return this.$element.val();
	    }
	
	    var newVal = args[0];
	
	    if ($.isArray(newVal)) {
	      newVal = $.map(newVal, function (obj) {
	        return obj.toString();
	      });
	    }
	
	    this.$element.val(newVal).trigger('change');
	  };
	
	  Select2.prototype.destroy = function () {
	    this.$container.remove();
	
	    if (this.$element[0].detachEvent) {
	      this.$element[0].detachEvent('onpropertychange', this._syncA);
	    }
	
	    if (this._observer != null) {
	      this._observer.disconnect();
	      this._observer = null;
	    } else if (this.$element[0].removeEventListener) {
	      this.$element[0]
	        .removeEventListener('DOMAttrModified', this._syncA, false);
	      this.$element[0]
	        .removeEventListener('DOMNodeInserted', this._syncS, false);
	      this.$element[0]
	        .removeEventListener('DOMNodeRemoved', this._syncS, false);
	    }
	
	    this._syncA = null;
	    this._syncS = null;
	
	    this.$element.off('.select2');
	    this.$element.attr('tabindex', this.$element.data('old-tabindex'));
	
	    this.$element.removeClass('select2-hidden-accessible');
	    this.$element.attr('aria-hidden', 'false');
	    this.$element.removeData('select2');
	
	    this.dataAdapter.destroy();
	    this.selection.destroy();
	    this.dropdown.destroy();
	    this.results.destroy();
	
	    this.dataAdapter = null;
	    this.selection = null;
	    this.dropdown = null;
	    this.results = null;
	  };
	
	  Select2.prototype.render = function () {
	    var $container = $(
	      '<span class="select2 select2-container">' +
	        '<span class="selection"></span>' +
	        '<span class="dropdown-wrapper" aria-hidden="true"></span>' +
	      '</span>'
	    );
	
	    $container.attr('dir', this.options.get('dir'));
	
	    this.$container = $container;
	
	    this.$container.addClass('select2-container--' + this.options.get('theme'));
	
	    $container.data('element', this.$element);
	
	    return $container;
	  };
	
	  return Select2;
	});
	
	S2.define('jquery-mousewheel',[
	  'jquery'
	], function ($) {
	  // Used to shim jQuery.mousewheel for non-full builds.
	  return $;
	});
	
	S2.define('jquery.select2',[
	  'jquery',
	  'jquery-mousewheel',
	
	  './select2/core',
	  './select2/defaults'
	], function ($, _, Select2, Defaults) {
	  if ($.fn.select2 == null) {
	    // All methods that should return the element
	    var thisMethods = ['open', 'close', 'destroy'];
	
	    $.fn.select2 = function (options) {
	      options = options || {};
	
	      if (typeof options === 'object') {
	        this.each(function () {
	          var instanceOptions = $.extend(true, {}, options);
	
	          var instance = new Select2($(this), instanceOptions);
	        });
	
	        return this;
	      } else if (typeof options === 'string') {
	        var ret;
	        var args = Array.prototype.slice.call(arguments, 1);
	
	        this.each(function () {
	          var instance = $(this).data('select2');
	
	          if (instance == null && window.console && console.error) {
	            console.error(
	              'The select2(\'' + options + '\') method was called on an ' +
	              'element that is not using Select2.'
	            );
	          }
	
	          ret = instance[options].apply(instance, args);
	        });
	
	        // Check if we should be returning `this`
	        if ($.inArray(options, thisMethods) > -1) {
	          return this;
	        }
	
	        return ret;
	      } else {
	        throw new Error('Invalid arguments for Select2: ' + options);
	      }
	    };
	  }
	
	  if ($.fn.select2.defaults == null) {
	    $.fn.select2.defaults = Defaults;
	  }
	
	  return Select2;
	});
	
	  // Return the AMD loader configuration so it can be used outside of this file
	  return {
	    define: S2.define,
	    require: S2.require
	  };
	}());
	
	  // Autoload the jQuery bindings
	  // We know that all of the modules exist above this, so we're safe
	  var select2 = S2.require('jquery.select2');
	
	  // Hold the AMD module references on the jQuery function that was just loaded
	  // This allows Select2 to use the internal loader outside of this file, such
	  // as in the language files.
	  jQuery.fn.select2.amd = S2;
	
	  // Return the Select2 instance for anyone who is importing it.
	  return select2;
	}));
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }

});
//# sourceMappingURL=home.chunk.js.map