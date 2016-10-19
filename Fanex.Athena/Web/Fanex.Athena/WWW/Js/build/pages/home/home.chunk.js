webpackJsonp([7],[
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(34);
	__webpack_require__(35);
	__webpack_require__(36);
	module.exports = __webpack_require__(38);


/***/ },
/* 1 */,
/* 2 */,
/* 3 */,
/* 4 */,
/* 5 */,
/* 6 */,
/* 7 */,
/* 8 */,
/* 9 */,
/* 10 */,
/* 11 */,
/* 12 */,
/* 13 */,
/* 14 */,
/* 15 */,
/* 16 */,
/* 17 */,
/* 18 */,
/* 19 */,
/* 20 */,
/* 21 */,
/* 22 */,
/* 23 */,
/* 24 */,
/* 25 */,
/* 26 */,
/* 27 */,
/* 28 */,
/* 29 */,
/* 30 */,
/* 31 */,
/* 32 */,
/* 33 */,
/* 34 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery, $) {'use strict';
	
	(function ($) {
	
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
	
	                    if (window.innerWidth > 992) {
	                        $(".site-menu li.has-sub").removeClass("active");
	                        $('.site-menu .menu-item .collapsible-header').removeClass('active');
	                        $(".site-menu .menu-item div.collapsible-body").css('display', 'none');
	                        menu_id.removeAttr('style');
	                        $('body').toggleClass('site-menubar-fold');
	                    } else {
	
	                        if (menuOut === true) {
	                            menuOut = false;
	                            panning = false;
	                            removeMenu();
	                        } else {
	
	                            // Disable Scrolling
	                            var $body = $('body');
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
	                            $('body').append(overlay);
	                            overlay.velocity({ opacity: 1 }, {
	                                duration: 300, queue: false, easing: 'easeOutQuad',
	                                complete: function complete() {
	                                    menuOut = true;
	                                    panning = false;
	                                }
	                            });
	                        }
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
	})(jQuery);
	
	$(document).ready(function () {
	
	    $('#label-search').click(function () {
	        $(this).parent().addClass('focused');
	        $("#searchbox").focus();
	    });
	
	    $("#searchbox").blur(function () {
	        if (!$(this).val()) {
	            $(this).parent().removeClass('focused');
	        }
	    });
	
	    $(".toggleMenubar").sideNav2();
	});
	
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1), __webpack_require__(1)))

/***/ },
/* 35 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {'use strict';
	
	(function ($) {
	    $.fn.collapsibleMenu = function (options) {
	        var defaults = {
	            accordion: undefined
	        };
	
	        options = $.extend(defaults, options);
	        var touch = 'ontouchstart' in window || 'onmsgesturechange' in window;
	
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
	
	                if (touch && $('body').hasClass('site-menubar-fold')) {
	                    var element = $(e.target);
	                    var temp = element.closest('li > .collapsible-body');
	
	                    var element = temp.parent().find('.collapsible-header');
	
	                    element.trigger("click");
	                }
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
	
	    $(document).ready(function () {
	        $('.collapsible2').collapsibleMenu();
	    });
	})(jQuery);
	
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 36 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;var __WEBPACK_AMD_DEFINE_FACTORY__, __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {/* == jquery mousewheel plugin == Version: 3.1.13, License: MIT License (MIT) */
	!function(a){ true?!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (a), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__)):"object"==typeof exports?module.exports=a:a(jQuery)}(function(a){function b(b){var g=b||window.event,h=i.call(arguments,1),j=0,l=0,m=0,n=0,o=0,p=0;if(b=a.event.fix(g),b.type="mousewheel","detail"in g&&(m=-1*g.detail),"wheelDelta"in g&&(m=g.wheelDelta),"wheelDeltaY"in g&&(m=g.wheelDeltaY),"wheelDeltaX"in g&&(l=-1*g.wheelDeltaX),"axis"in g&&g.axis===g.HORIZONTAL_AXIS&&(l=-1*m,m=0),j=0===m?l:m,"deltaY"in g&&(m=-1*g.deltaY,j=m),"deltaX"in g&&(l=g.deltaX,0===m&&(j=-1*l)),0!==m||0!==l){if(1===g.deltaMode){var q=a.data(this,"mousewheel-line-height");j*=q,m*=q,l*=q}else if(2===g.deltaMode){var r=a.data(this,"mousewheel-page-height");j*=r,m*=r,l*=r}if(n=Math.max(Math.abs(m),Math.abs(l)),(!f||f>n)&&(f=n,d(g,n)&&(f/=40)),d(g,n)&&(j/=40,l/=40,m/=40),j=Math[j>=1?"floor":"ceil"](j/f),l=Math[l>=1?"floor":"ceil"](l/f),m=Math[m>=1?"floor":"ceil"](m/f),k.settings.normalizeOffset&&this.getBoundingClientRect){var s=this.getBoundingClientRect();o=b.clientX-s.left,p=b.clientY-s.top}return b.deltaX=l,b.deltaY=m,b.deltaFactor=f,b.offsetX=o,b.offsetY=p,b.deltaMode=0,h.unshift(b,j,l,m),e&&clearTimeout(e),e=setTimeout(c,200),(a.event.dispatch||a.event.handle).apply(this,h)}}function c(){f=null}function d(a,b){return k.settings.adjustOldDeltas&&"mousewheel"===a.type&&b%120===0}var e,f,g=["wheel","mousewheel","DOMMouseScroll","MozMousePixelScroll"],h="onwheel"in document||document.documentMode>=9?["wheel"]:["mousewheel","DomMouseScroll","MozMousePixelScroll"],i=Array.prototype.slice;if(a.event.fixHooks)for(var j=g.length;j;)a.event.fixHooks[g[--j]]=a.event.mouseHooks;var k=a.event.special.mousewheel={version:"3.1.12",setup:function(){if(this.addEventListener)for(var c=h.length;c;)this.addEventListener(h[--c],b,!1);else this.onmousewheel=b;a.data(this,"mousewheel-line-height",k.getLineHeight(this)),a.data(this,"mousewheel-page-height",k.getPageHeight(this))},teardown:function(){if(this.removeEventListener)for(var c=h.length;c;)this.removeEventListener(h[--c],b,!1);else this.onmousewheel=null;a.removeData(this,"mousewheel-line-height"),a.removeData(this,"mousewheel-page-height")},getLineHeight:function(b){var c=a(b),d=c["offsetParent"in a.fn?"offsetParent":"parent"]();return d.length||(d=a("body")),parseInt(d.css("fontSize"),10)||parseInt(c.css("fontSize"),10)||16},getPageHeight:function(b){return a(b).height()},settings:{adjustOldDeltas:!0,normalizeOffset:!0}};a.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})});!function(a){ true?!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (a), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__)):"object"==typeof exports?module.exports=a:a(jQuery)}(function(a){function b(b){var g=b||window.event,h=i.call(arguments,1),j=0,l=0,m=0,n=0,o=0,p=0;if(b=a.event.fix(g),b.type="mousewheel","detail"in g&&(m=-1*g.detail),"wheelDelta"in g&&(m=g.wheelDelta),"wheelDeltaY"in g&&(m=g.wheelDeltaY),"wheelDeltaX"in g&&(l=-1*g.wheelDeltaX),"axis"in g&&g.axis===g.HORIZONTAL_AXIS&&(l=-1*m,m=0),j=0===m?l:m,"deltaY"in g&&(m=-1*g.deltaY,j=m),"deltaX"in g&&(l=g.deltaX,0===m&&(j=-1*l)),0!==m||0!==l){if(1===g.deltaMode){var q=a.data(this,"mousewheel-line-height");j*=q,m*=q,l*=q}else if(2===g.deltaMode){var r=a.data(this,"mousewheel-page-height");j*=r,m*=r,l*=r}if(n=Math.max(Math.abs(m),Math.abs(l)),(!f||f>n)&&(f=n,d(g,n)&&(f/=40)),d(g,n)&&(j/=40,l/=40,m/=40),j=Math[j>=1?"floor":"ceil"](j/f),l=Math[l>=1?"floor":"ceil"](l/f),m=Math[m>=1?"floor":"ceil"](m/f),k.settings.normalizeOffset&&this.getBoundingClientRect){var s=this.getBoundingClientRect();o=b.clientX-s.left,p=b.clientY-s.top}return b.deltaX=l,b.deltaY=m,b.deltaFactor=f,b.offsetX=o,b.offsetY=p,b.deltaMode=0,h.unshift(b,j,l,m),e&&clearTimeout(e),e=setTimeout(c,200),(a.event.dispatch||a.event.handle).apply(this,h)}}function c(){f=null}function d(a,b){return k.settings.adjustOldDeltas&&"mousewheel"===a.type&&b%120===0}var e,f,g=["wheel","mousewheel","DOMMouseScroll","MozMousePixelScroll"],h="onwheel"in document||document.documentMode>=9?["wheel"]:["mousewheel","DomMouseScroll","MozMousePixelScroll"],i=Array.prototype.slice;if(a.event.fixHooks)for(var j=g.length;j;)a.event.fixHooks[g[--j]]=a.event.mouseHooks;var k=a.event.special.mousewheel={version:"3.1.12",setup:function(){if(this.addEventListener)for(var c=h.length;c;)this.addEventListener(h[--c],b,!1);else this.onmousewheel=b;a.data(this,"mousewheel-line-height",k.getLineHeight(this)),a.data(this,"mousewheel-page-height",k.getPageHeight(this))},teardown:function(){if(this.removeEventListener)for(var c=h.length;c;)this.removeEventListener(h[--c],b,!1);else this.onmousewheel=null;a.removeData(this,"mousewheel-line-height"),a.removeData(this,"mousewheel-page-height")},getLineHeight:function(b){var c=a(b),d=c["offsetParent"in a.fn?"offsetParent":"parent"]();return d.length||(d=a("body")),parseInt(d.css("fontSize"),10)||parseInt(c.css("fontSize"),10)||16},getPageHeight:function(b){return a(b).height()},settings:{adjustOldDeltas:!0,normalizeOffset:!0}};a.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})});
	/* == malihu jquery custom scrollbar plugin == Version: 3.1.5, License: MIT License (MIT) */
	!function(e){ true?!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_FACTORY__ = (e), __WEBPACK_AMD_DEFINE_RESULT__ = (typeof __WEBPACK_AMD_DEFINE_FACTORY__ === 'function' ? (__WEBPACK_AMD_DEFINE_FACTORY__.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__)) : __WEBPACK_AMD_DEFINE_FACTORY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__)):"undefined"!=typeof module&&module.exports?module.exports=e:e(jQuery,window,document)}(function(e){!function(t){var o="function"=="function"&&__webpack_require__(3),a="undefined"!=typeof module&&module.exports,n="https:"==document.location.protocol?"https:":"http:",i="cdnjs.cloudflare.com/ajax/libs/jquery-mousewheel/3.1.13/jquery.mousewheel.min.js";o||(a?__webpack_require__(37)(e):e.event.special.mousewheel||e("head").append(decodeURI("%3Cscript src="+n+"//"+i+"%3E%3C/script%3E"))),t()}(function(){var t,o="mCustomScrollbar",a="mCS",n=".mCustomScrollbar",i={setTop:0,setLeft:0,axis:"y",scrollbarPosition:"inside",scrollInertia:950,autoDraggerLength:!0,alwaysShowScrollbar:0,snapOffset:0,mouseWheel:{enable:!0,scrollAmount:"auto",axis:"y",deltaFactor:"auto",disableOver:["select","option","keygen","datalist","textarea"]},scrollButtons:{scrollType:"stepless",scrollAmount:"auto"},keyboard:{enable:!0,scrollType:"stepless",scrollAmount:"auto"},contentTouchScroll:25,documentTouchScroll:!0,advanced:{autoScrollOnFocus:"input,textarea,select,button,datalist,keygen,a[tabindex],area,object,[contenteditable='true']",updateOnContentResize:!0,updateOnImageLoad:"auto",autoUpdateTimeout:60},theme:"light",callbacks:{onTotalScrollOffset:0,onTotalScrollBackOffset:0,alwaysTriggerOffsets:!0}},r=0,l={},s=window.attachEvent&&!window.addEventListener?1:0,c=!1,d=["mCSB_dragger_onDrag","mCSB_scrollTools_onDrag","mCS_img_loaded","mCS_disabled","mCS_destroyed","mCS_no_scrollbar","mCS-autoHide","mCS-dir-rtl","mCS_no_scrollbar_y","mCS_no_scrollbar_x","mCS_y_hidden","mCS_x_hidden","mCSB_draggerContainer","mCSB_buttonUp","mCSB_buttonDown","mCSB_buttonLeft","mCSB_buttonRight"],u={init:function(t){var t=e.extend(!0,{},i,t),o=f.call(this);if(t.live){var s=t.liveSelector||this.selector||n,c=e(s);if("off"===t.live)return void m(s);l[s]=setTimeout(function(){c.mCustomScrollbar(t),"once"===t.live&&c.length&&m(s)},500)}else m(s);return t.setWidth=t.set_width?t.set_width:t.setWidth,t.setHeight=t.set_height?t.set_height:t.setHeight,t.axis=t.horizontalScroll?"x":p(t.axis),t.scrollInertia=t.scrollInertia>0&&t.scrollInertia<17?17:t.scrollInertia,"object"!=typeof t.mouseWheel&&1==t.mouseWheel&&(t.mouseWheel={enable:!0,scrollAmount:"auto",axis:"y",preventDefault:!1,deltaFactor:"auto",normalizeDelta:!1,invert:!1}),t.mouseWheel.scrollAmount=t.mouseWheelPixels?t.mouseWheelPixels:t.mouseWheel.scrollAmount,t.mouseWheel.normalizeDelta=t.advanced.normalizeMouseWheelDelta?t.advanced.normalizeMouseWheelDelta:t.mouseWheel.normalizeDelta,t.scrollButtons.scrollType=g(t.scrollButtons.scrollType),h(t),e(o).each(function(){var o=e(this);if(!o.data(a)){o.data(a,{idx:++r,opt:t,scrollRatio:{y:null,x:null},overflowed:null,contentReset:{y:null,x:null},bindEvents:!1,tweenRunning:!1,sequential:{},langDir:o.css("direction"),cbOffsets:null,trigger:null,poll:{size:{o:0,n:0},img:{o:0,n:0},change:{o:0,n:0}}});var n=o.data(a),i=n.opt,l=o.data("mcs-axis"),s=o.data("mcs-scrollbar-position"),c=o.data("mcs-theme");l&&(i.axis=l),s&&(i.scrollbarPosition=s),c&&(i.theme=c,h(i)),v.call(this),n&&i.callbacks.onCreate&&"function"==typeof i.callbacks.onCreate&&i.callbacks.onCreate.call(this),e("#mCSB_"+n.idx+"_container img:not(."+d[2]+")").addClass(d[2]),u.update.call(null,o)}})},update:function(t,o){var n=t||f.call(this);return e(n).each(function(){var t=e(this);if(t.data(a)){var n=t.data(a),i=n.opt,r=e("#mCSB_"+n.idx+"_container"),l=e("#mCSB_"+n.idx),s=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")];if(!r.length)return;n.tweenRunning&&Q(t),o&&n&&i.callbacks.onBeforeUpdate&&"function"==typeof i.callbacks.onBeforeUpdate&&i.callbacks.onBeforeUpdate.call(this),t.hasClass(d[3])&&t.removeClass(d[3]),t.hasClass(d[4])&&t.removeClass(d[4]),l.css("max-height","none"),l.height()!==t.height()&&l.css("max-height",t.height()),_.call(this),"y"===i.axis||i.advanced.autoExpandHorizontalScroll||r.css("width",x(r)),n.overflowed=y.call(this),M.call(this),i.autoDraggerLength&&S.call(this),b.call(this),T.call(this);var c=[Math.abs(r[0].offsetTop),Math.abs(r[0].offsetLeft)];"x"!==i.axis&&(n.overflowed[0]?s[0].height()>s[0].parent().height()?B.call(this):(G(t,c[0].toString(),{dir:"y",dur:0,overwrite:"none"}),n.contentReset.y=null):(B.call(this),"y"===i.axis?k.call(this):"yx"===i.axis&&n.overflowed[1]&&G(t,c[1].toString(),{dir:"x",dur:0,overwrite:"none"}))),"y"!==i.axis&&(n.overflowed[1]?s[1].width()>s[1].parent().width()?B.call(this):(G(t,c[1].toString(),{dir:"x",dur:0,overwrite:"none"}),n.contentReset.x=null):(B.call(this),"x"===i.axis?k.call(this):"yx"===i.axis&&n.overflowed[0]&&G(t,c[0].toString(),{dir:"y",dur:0,overwrite:"none"}))),o&&n&&(2===o&&i.callbacks.onImageLoad&&"function"==typeof i.callbacks.onImageLoad?i.callbacks.onImageLoad.call(this):3===o&&i.callbacks.onSelectorChange&&"function"==typeof i.callbacks.onSelectorChange?i.callbacks.onSelectorChange.call(this):i.callbacks.onUpdate&&"function"==typeof i.callbacks.onUpdate&&i.callbacks.onUpdate.call(this)),N.call(this)}})},scrollTo:function(t,o){if("undefined"!=typeof t&&null!=t){var n=f.call(this);return e(n).each(function(){var n=e(this);if(n.data(a)){var i=n.data(a),r=i.opt,l={trigger:"external",scrollInertia:r.scrollInertia,scrollEasing:"mcsEaseInOut",moveDragger:!1,timeout:60,callbacks:!0,onStart:!0,onUpdate:!0,onComplete:!0},s=e.extend(!0,{},l,o),c=Y.call(this,t),d=s.scrollInertia>0&&s.scrollInertia<17?17:s.scrollInertia;c[0]=X.call(this,c[0],"y"),c[1]=X.call(this,c[1],"x"),s.moveDragger&&(c[0]*=i.scrollRatio.y,c[1]*=i.scrollRatio.x),s.dur=ne()?0:d,setTimeout(function(){null!==c[0]&&"undefined"!=typeof c[0]&&"x"!==r.axis&&i.overflowed[0]&&(s.dir="y",s.overwrite="all",G(n,c[0].toString(),s)),null!==c[1]&&"undefined"!=typeof c[1]&&"y"!==r.axis&&i.overflowed[1]&&(s.dir="x",s.overwrite="none",G(n,c[1].toString(),s))},s.timeout)}})}},stop:function(){var t=f.call(this);return e(t).each(function(){var t=e(this);t.data(a)&&Q(t)})},disable:function(t){var o=f.call(this);return e(o).each(function(){var o=e(this);if(o.data(a)){o.data(a);N.call(this,"remove"),k.call(this),t&&B.call(this),M.call(this,!0),o.addClass(d[3])}})},destroy:function(){var t=f.call(this);return e(t).each(function(){var n=e(this);if(n.data(a)){var i=n.data(a),r=i.opt,l=e("#mCSB_"+i.idx),s=e("#mCSB_"+i.idx+"_container"),c=e(".mCSB_"+i.idx+"_scrollbar");r.live&&m(r.liveSelector||e(t).selector),N.call(this,"remove"),k.call(this),B.call(this),n.removeData(a),$(this,"mcs"),c.remove(),s.find("img."+d[2]).removeClass(d[2]),l.replaceWith(s.contents()),n.removeClass(o+" _"+a+"_"+i.idx+" "+d[6]+" "+d[7]+" "+d[5]+" "+d[3]).addClass(d[4])}})}},f=function(){return"object"!=typeof e(this)||e(this).length<1?n:this},h=function(t){var o=["rounded","rounded-dark","rounded-dots","rounded-dots-dark"],a=["rounded-dots","rounded-dots-dark","3d","3d-dark","3d-thick","3d-thick-dark","inset","inset-dark","inset-2","inset-2-dark","inset-3","inset-3-dark"],n=["minimal","minimal-dark"],i=["minimal","minimal-dark"],r=["minimal","minimal-dark"];t.autoDraggerLength=e.inArray(t.theme,o)>-1?!1:t.autoDraggerLength,t.autoExpandScrollbar=e.inArray(t.theme,a)>-1?!1:t.autoExpandScrollbar,t.scrollButtons.enable=e.inArray(t.theme,n)>-1?!1:t.scrollButtons.enable,t.autoHideScrollbar=e.inArray(t.theme,i)>-1?!0:t.autoHideScrollbar,t.scrollbarPosition=e.inArray(t.theme,r)>-1?"outside":t.scrollbarPosition},m=function(e){l[e]&&(clearTimeout(l[e]),$(l,e))},p=function(e){return"yx"===e||"xy"===e||"auto"===e?"yx":"x"===e||"horizontal"===e?"x":"y"},g=function(e){return"stepped"===e||"pixels"===e||"step"===e||"click"===e?"stepped":"stepless"},v=function(){var t=e(this),n=t.data(a),i=n.opt,r=i.autoExpandScrollbar?" "+d[1]+"_expand":"",l=["<div id='mCSB_"+n.idx+"_scrollbar_vertical' class='mCSB_scrollTools mCSB_"+n.idx+"_scrollbar mCS-"+i.theme+" mCSB_scrollTools_vertical"+r+"'><div class='"+d[12]+"'><div id='mCSB_"+n.idx+"_dragger_vertical' class='mCSB_dragger' style='position:absolute;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>","<div id='mCSB_"+n.idx+"_scrollbar_horizontal' class='mCSB_scrollTools mCSB_"+n.idx+"_scrollbar mCS-"+i.theme+" mCSB_scrollTools_horizontal"+r+"'><div class='"+d[12]+"'><div id='mCSB_"+n.idx+"_dragger_horizontal' class='mCSB_dragger' style='position:absolute;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>"],s="yx"===i.axis?"mCSB_vertical_horizontal":"x"===i.axis?"mCSB_horizontal":"mCSB_vertical",c="yx"===i.axis?l[0]+l[1]:"x"===i.axis?l[1]:l[0],u="yx"===i.axis?"<div id='mCSB_"+n.idx+"_container_wrapper' class='mCSB_container_wrapper' />":"",f=i.autoHideScrollbar?" "+d[6]:"",h="x"!==i.axis&&"rtl"===n.langDir?" "+d[7]:"";i.setWidth&&t.css("width",i.setWidth),i.setHeight&&t.css("height",i.setHeight),i.setLeft="y"!==i.axis&&"rtl"===n.langDir?"989999px":i.setLeft,t.addClass(o+" _"+a+"_"+n.idx+f+h).wrapInner("<div id='mCSB_"+n.idx+"' class='mCustomScrollBox mCS-"+i.theme+" "+s+"'><div id='mCSB_"+n.idx+"_container' class='mCSB_container' style='position:relative; top:"+i.setTop+"; left:"+i.setLeft+";' dir='"+n.langDir+"' /></div>");var m=e("#mCSB_"+n.idx),p=e("#mCSB_"+n.idx+"_container");"y"===i.axis||i.advanced.autoExpandHorizontalScroll||p.css("width",x(p)),"outside"===i.scrollbarPosition?("static"===t.css("position")&&t.css("position","relative"),t.css("overflow","visible"),m.addClass("mCSB_outside").after(c)):(m.addClass("mCSB_inside").append(c),p.wrap(u)),w.call(this);var g=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")];g[0].css("min-height",g[0].height()),g[1].css("min-width",g[1].width())},x=function(t){var o=[t[0].scrollWidth,Math.max.apply(Math,t.children().map(function(){return e(this).outerWidth(!0)}).get())],a=t.parent().width();return o[0]>a?o[0]:o[1]>a?o[1]:"100%"},_=function(){var t=e(this),o=t.data(a),n=o.opt,i=e("#mCSB_"+o.idx+"_container");if(n.advanced.autoExpandHorizontalScroll&&"y"!==n.axis){i.css({width:"auto","min-width":0,"overflow-x":"scroll"});var r=Math.ceil(i[0].scrollWidth);3===n.advanced.autoExpandHorizontalScroll||2!==n.advanced.autoExpandHorizontalScroll&&r>i.parent().width()?i.css({width:r,"min-width":"100%","overflow-x":"inherit"}):i.css({"overflow-x":"inherit",position:"absolute"}).wrap("<div class='mCSB_h_wrapper' style='position:relative; left:0; width:999999px;' />").css({width:Math.ceil(i[0].getBoundingClientRect().right+.4)-Math.floor(i[0].getBoundingClientRect().left),"min-width":"100%",position:"relative"}).unwrap()}},w=function(){var t=e(this),o=t.data(a),n=o.opt,i=e(".mCSB_"+o.idx+"_scrollbar:first"),r=oe(n.scrollButtons.tabindex)?"tabindex='"+n.scrollButtons.tabindex+"'":"",l=["<a href='#' class='"+d[13]+"' "+r+" />","<a href='#' class='"+d[14]+"' "+r+" />","<a href='#' class='"+d[15]+"' "+r+" />","<a href='#' class='"+d[16]+"' "+r+" />"],s=["x"===n.axis?l[2]:l[0],"x"===n.axis?l[3]:l[1],l[2],l[3]];n.scrollButtons.enable&&i.prepend(s[0]).append(s[1]).next(".mCSB_scrollTools").prepend(s[2]).append(s[3])},S=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")],l=[n.height()/i.outerHeight(!1),n.width()/i.outerWidth(!1)],c=[parseInt(r[0].css("min-height")),Math.round(l[0]*r[0].parent().height()),parseInt(r[1].css("min-width")),Math.round(l[1]*r[1].parent().width())],d=s&&c[1]<c[0]?c[0]:c[1],u=s&&c[3]<c[2]?c[2]:c[3];r[0].css({height:d,"max-height":r[0].parent().height()-10}).find(".mCSB_dragger_bar").css({"line-height":c[0]+"px"}),r[1].css({width:u,"max-width":r[1].parent().width()-10})},b=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")],l=[i.outerHeight(!1)-n.height(),i.outerWidth(!1)-n.width()],s=[l[0]/(r[0].parent().height()-r[0].height()),l[1]/(r[1].parent().width()-r[1].width())];o.scrollRatio={y:s[0],x:s[1]}},C=function(e,t,o){var a=o?d[0]+"_expanded":"",n=e.closest(".mCSB_scrollTools");"active"===t?(e.toggleClass(d[0]+" "+a),n.toggleClass(d[1]),e[0]._draggable=e[0]._draggable?0:1):e[0]._draggable||("hide"===t?(e.removeClass(d[0]),n.removeClass(d[1])):(e.addClass(d[0]),n.addClass(d[1])))},y=function(){var t=e(this),o=t.data(a),n=e("#mCSB_"+o.idx),i=e("#mCSB_"+o.idx+"_container"),r=null==o.overflowed?i.height():i.outerHeight(!1),l=null==o.overflowed?i.width():i.outerWidth(!1),s=i[0].scrollHeight,c=i[0].scrollWidth;return s>r&&(r=s),c>l&&(l=c),[r>n.height(),l>n.width()]},B=function(){var t=e(this),o=t.data(a),n=o.opt,i=e("#mCSB_"+o.idx),r=e("#mCSB_"+o.idx+"_container"),l=[e("#mCSB_"+o.idx+"_dragger_vertical"),e("#mCSB_"+o.idx+"_dragger_horizontal")];if(Q(t),("x"!==n.axis&&!o.overflowed[0]||"y"===n.axis&&o.overflowed[0])&&(l[0].add(r).css("top",0),G(t,"_resetY")),"y"!==n.axis&&!o.overflowed[1]||"x"===n.axis&&o.overflowed[1]){var s=dx=0;"rtl"===o.langDir&&(s=i.width()-r.outerWidth(!1),dx=Math.abs(s/o.scrollRatio.x)),r.css("left",s),l[1].css("left",dx),G(t,"_resetX")}},T=function(){function t(){r=setTimeout(function(){e.event.special.mousewheel?(clearTimeout(r),W.call(o[0])):t()},100)}var o=e(this),n=o.data(a),i=n.opt;if(!n.bindEvents){if(I.call(this),i.contentTouchScroll&&D.call(this),E.call(this),i.mouseWheel.enable){var r;t()}P.call(this),U.call(this),i.advanced.autoScrollOnFocus&&H.call(this),i.scrollButtons.enable&&F.call(this),i.keyboard.enable&&q.call(this),n.bindEvents=!0}},k=function(){var t=e(this),o=t.data(a),n=o.opt,i=a+"_"+o.idx,r=".mCSB_"+o.idx+"_scrollbar",l=e("#mCSB_"+o.idx+",#mCSB_"+o.idx+"_container,#mCSB_"+o.idx+"_container_wrapper,"+r+" ."+d[12]+",#mCSB_"+o.idx+"_dragger_vertical,#mCSB_"+o.idx+"_dragger_horizontal,"+r+">a"),s=e("#mCSB_"+o.idx+"_container");n.advanced.releaseDraggableSelectors&&l.add(e(n.advanced.releaseDraggableSelectors)),n.advanced.extraDraggableSelectors&&l.add(e(n.advanced.extraDraggableSelectors)),o.bindEvents&&(e(document).add(e(!A()||top.document)).unbind("."+i),l.each(function(){e(this).unbind("."+i)}),clearTimeout(t[0]._focusTimeout),$(t[0],"_focusTimeout"),clearTimeout(o.sequential.step),$(o.sequential,"step"),clearTimeout(s[0].onCompleteTimeout),$(s[0],"onCompleteTimeout"),o.bindEvents=!1)},M=function(t){var o=e(this),n=o.data(a),i=n.opt,r=e("#mCSB_"+n.idx+"_container_wrapper"),l=r.length?r:e("#mCSB_"+n.idx+"_container"),s=[e("#mCSB_"+n.idx+"_scrollbar_vertical"),e("#mCSB_"+n.idx+"_scrollbar_horizontal")],c=[s[0].find(".mCSB_dragger"),s[1].find(".mCSB_dragger")];"x"!==i.axis&&(n.overflowed[0]&&!t?(s[0].add(c[0]).add(s[0].children("a")).css("display","block"),l.removeClass(d[8]+" "+d[10])):(i.alwaysShowScrollbar?(2!==i.alwaysShowScrollbar&&c[0].css("display","none"),l.removeClass(d[10])):(s[0].css("display","none"),l.addClass(d[10])),l.addClass(d[8]))),"y"!==i.axis&&(n.overflowed[1]&&!t?(s[1].add(c[1]).add(s[1].children("a")).css("display","block"),l.removeClass(d[9]+" "+d[11])):(i.alwaysShowScrollbar?(2!==i.alwaysShowScrollbar&&c[1].css("display","none"),l.removeClass(d[11])):(s[1].css("display","none"),l.addClass(d[11])),l.addClass(d[9]))),n.overflowed[0]||n.overflowed[1]?o.removeClass(d[5]):o.addClass(d[5])},O=function(t){var o=t.type,a=t.target.ownerDocument!==document&&null!==frameElement?[e(frameElement).offset().top,e(frameElement).offset().left]:null,n=A()&&t.target.ownerDocument!==top.document&&null!==frameElement?[e(t.view.frameElement).offset().top,e(t.view.frameElement).offset().left]:[0,0];switch(o){case"pointerdown":case"MSPointerDown":case"pointermove":case"MSPointerMove":case"pointerup":case"MSPointerUp":return a?[t.originalEvent.pageY-a[0]+n[0],t.originalEvent.pageX-a[1]+n[1],!1]:[t.originalEvent.pageY,t.originalEvent.pageX,!1];case"touchstart":case"touchmove":case"touchend":var i=t.originalEvent.touches[0]||t.originalEvent.changedTouches[0],r=t.originalEvent.touches.length||t.originalEvent.changedTouches.length;return t.target.ownerDocument!==document?[i.screenY,i.screenX,r>1]:[i.pageY,i.pageX,r>1];default:return a?[t.pageY-a[0]+n[0],t.pageX-a[1]+n[1],!1]:[t.pageY,t.pageX,!1]}},I=function(){function t(e,t,a,n){if(h[0].idleTimer=d.scrollInertia<233?250:0,o.attr("id")===f[1])var i="x",s=(o[0].offsetLeft-t+n)*l.scrollRatio.x;else var i="y",s=(o[0].offsetTop-e+a)*l.scrollRatio.y;G(r,s.toString(),{dir:i,drag:!0})}var o,n,i,r=e(this),l=r.data(a),d=l.opt,u=a+"_"+l.idx,f=["mCSB_"+l.idx+"_dragger_vertical","mCSB_"+l.idx+"_dragger_horizontal"],h=e("#mCSB_"+l.idx+"_container"),m=e("#"+f[0]+",#"+f[1]),p=d.advanced.releaseDraggableSelectors?m.add(e(d.advanced.releaseDraggableSelectors)):m,g=d.advanced.extraDraggableSelectors?e(!A()||top.document).add(e(d.advanced.extraDraggableSelectors)):e(!A()||top.document);m.bind("contextmenu."+u,function(e){e.preventDefault()}).bind("mousedown."+u+" touchstart."+u+" pointerdown."+u+" MSPointerDown."+u,function(t){if(t.stopImmediatePropagation(),t.preventDefault(),ee(t)){c=!0,s&&(document.onselectstart=function(){return!1}),L.call(h,!1),Q(r),o=e(this);var a=o.offset(),l=O(t)[0]-a.top,u=O(t)[1]-a.left,f=o.height()+a.top,m=o.width()+a.left;f>l&&l>0&&m>u&&u>0&&(n=l,i=u),C(o,"active",d.autoExpandScrollbar)}}).bind("touchmove."+u,function(e){e.stopImmediatePropagation(),e.preventDefault();var a=o.offset(),r=O(e)[0]-a.top,l=O(e)[1]-a.left;t(n,i,r,l)}),e(document).add(g).bind("mousemove."+u+" pointermove."+u+" MSPointerMove."+u,function(e){if(o){var a=o.offset(),r=O(e)[0]-a.top,l=O(e)[1]-a.left;if(n===r&&i===l)return;t(n,i,r,l)}}).add(p).bind("mouseup."+u+" touchend."+u+" pointerup."+u+" MSPointerUp."+u,function(){o&&(C(o,"active",d.autoExpandScrollbar),o=null),c=!1,s&&(document.onselectstart=null),L.call(h,!0)})},D=function(){function o(e){if(!te(e)||c||O(e)[2])return void(t=0);t=1,b=0,C=0,d=1,y.removeClass("mCS_touch_action");var o=I.offset();u=O(e)[0]-o.top,f=O(e)[1]-o.left,z=[O(e)[0],O(e)[1]]}function n(e){if(te(e)&&!c&&!O(e)[2]&&(T.documentTouchScroll||e.preventDefault(),e.stopImmediatePropagation(),(!C||b)&&d)){g=K();var t=M.offset(),o=O(e)[0]-t.top,a=O(e)[1]-t.left,n="mcsLinearOut";if(E.push(o),W.push(a),z[2]=Math.abs(O(e)[0]-z[0]),z[3]=Math.abs(O(e)[1]-z[1]),B.overflowed[0])var i=D[0].parent().height()-D[0].height(),r=u-o>0&&o-u>-(i*B.scrollRatio.y)&&(2*z[3]<z[2]||"yx"===T.axis);if(B.overflowed[1])var l=D[1].parent().width()-D[1].width(),h=f-a>0&&a-f>-(l*B.scrollRatio.x)&&(2*z[2]<z[3]||"yx"===T.axis);r||h?(U||e.preventDefault(),b=1):(C=1,y.addClass("mCS_touch_action")),U&&e.preventDefault(),w="yx"===T.axis?[u-o,f-a]:"x"===T.axis?[null,f-a]:[u-o,null],I[0].idleTimer=250,B.overflowed[0]&&s(w[0],R,n,"y","all",!0),B.overflowed[1]&&s(w[1],R,n,"x",L,!0)}}function i(e){if(!te(e)||c||O(e)[2])return void(t=0);t=1,e.stopImmediatePropagation(),Q(y),p=K();var o=M.offset();h=O(e)[0]-o.top,m=O(e)[1]-o.left,E=[],W=[]}function r(e){if(te(e)&&!c&&!O(e)[2]){d=0,e.stopImmediatePropagation(),b=0,C=0,v=K();var t=M.offset(),o=O(e)[0]-t.top,a=O(e)[1]-t.left;if(!(v-g>30)){_=1e3/(v-p);var n="mcsEaseOut",i=2.5>_,r=i?[E[E.length-2],W[W.length-2]]:[0,0];x=i?[o-r[0],a-r[1]]:[o-h,a-m];var u=[Math.abs(x[0]),Math.abs(x[1])];_=i?[Math.abs(x[0]/4),Math.abs(x[1]/4)]:[_,_];var f=[Math.abs(I[0].offsetTop)-x[0]*l(u[0]/_[0],_[0]),Math.abs(I[0].offsetLeft)-x[1]*l(u[1]/_[1],_[1])];w="yx"===T.axis?[f[0],f[1]]:"x"===T.axis?[null,f[1]]:[f[0],null],S=[4*u[0]+T.scrollInertia,4*u[1]+T.scrollInertia];var y=parseInt(T.contentTouchScroll)||0;w[0]=u[0]>y?w[0]:0,w[1]=u[1]>y?w[1]:0,B.overflowed[0]&&s(w[0],S[0],n,"y",L,!1),B.overflowed[1]&&s(w[1],S[1],n,"x",L,!1)}}}function l(e,t){var o=[1.5*t,2*t,t/1.5,t/2];return e>90?t>4?o[0]:o[3]:e>60?t>3?o[3]:o[2]:e>30?t>8?o[1]:t>6?o[0]:t>4?t:o[2]:t>8?t:o[3]}function s(e,t,o,a,n,i){e&&G(y,e.toString(),{dur:t,scrollEasing:o,dir:a,overwrite:n,drag:i})}var d,u,f,h,m,p,g,v,x,_,w,S,b,C,y=e(this),B=y.data(a),T=B.opt,k=a+"_"+B.idx,M=e("#mCSB_"+B.idx),I=e("#mCSB_"+B.idx+"_container"),D=[e("#mCSB_"+B.idx+"_dragger_vertical"),e("#mCSB_"+B.idx+"_dragger_horizontal")],E=[],W=[],R=0,L="yx"===T.axis?"none":"all",z=[],P=I.find("iframe"),H=["touchstart."+k+" pointerdown."+k+" MSPointerDown."+k,"touchmove."+k+" pointermove."+k+" MSPointerMove."+k,"touchend."+k+" pointerup."+k+" MSPointerUp."+k],U=void 0!==document.body.style.touchAction&&""!==document.body.style.touchAction;I.bind(H[0],function(e){o(e)}).bind(H[1],function(e){n(e)}),M.bind(H[0],function(e){i(e)}).bind(H[2],function(e){r(e)}),P.length&&P.each(function(){e(this).bind("load",function(){A(this)&&e(this.contentDocument||this.contentWindow.document).bind(H[0],function(e){o(e),i(e)}).bind(H[1],function(e){n(e)}).bind(H[2],function(e){r(e)})})})},E=function(){function o(){return window.getSelection?window.getSelection().toString():document.selection&&"Control"!=document.selection.type?document.selection.createRange().text:0}function n(e,t,o){d.type=o&&i?"stepped":"stepless",d.scrollAmount=10,j(r,e,t,"mcsLinearOut",o?60:null)}var i,r=e(this),l=r.data(a),s=l.opt,d=l.sequential,u=a+"_"+l.idx,f=e("#mCSB_"+l.idx+"_container"),h=f.parent();f.bind("mousedown."+u,function(){t||i||(i=1,c=!0)}).add(document).bind("mousemove."+u,function(e){if(!t&&i&&o()){var a=f.offset(),r=O(e)[0]-a.top+f[0].offsetTop,c=O(e)[1]-a.left+f[0].offsetLeft;r>0&&r<h.height()&&c>0&&c<h.width()?d.step&&n("off",null,"stepped"):("x"!==s.axis&&l.overflowed[0]&&(0>r?n("on",38):r>h.height()&&n("on",40)),"y"!==s.axis&&l.overflowed[1]&&(0>c?n("on",37):c>h.width()&&n("on",39)))}}).bind("mouseup."+u+" dragend."+u,function(){t||(i&&(i=0,n("off",null)),c=!1)})},W=function(){function t(t,a){if(Q(o),!z(o,t.target)){var r="auto"!==i.mouseWheel.deltaFactor?parseInt(i.mouseWheel.deltaFactor):s&&t.deltaFactor<100?100:t.deltaFactor||100,d=i.scrollInertia;if("x"===i.axis||"x"===i.mouseWheel.axis)var u="x",f=[Math.round(r*n.scrollRatio.x),parseInt(i.mouseWheel.scrollAmount)],h="auto"!==i.mouseWheel.scrollAmount?f[1]:f[0]>=l.width()?.9*l.width():f[0],m=Math.abs(e("#mCSB_"+n.idx+"_container")[0].offsetLeft),p=c[1][0].offsetLeft,g=c[1].parent().width()-c[1].width(),v="y"===i.mouseWheel.axis?t.deltaY||a:t.deltaX;else var u="y",f=[Math.round(r*n.scrollRatio.y),parseInt(i.mouseWheel.scrollAmount)],h="auto"!==i.mouseWheel.scrollAmount?f[1]:f[0]>=l.height()?.9*l.height():f[0],m=Math.abs(e("#mCSB_"+n.idx+"_container")[0].offsetTop),p=c[0][0].offsetTop,g=c[0].parent().height()-c[0].height(),v=t.deltaY||a;"y"===u&&!n.overflowed[0]||"x"===u&&!n.overflowed[1]||((i.mouseWheel.invert||t.webkitDirectionInvertedFromDevice)&&(v=-v),i.mouseWheel.normalizeDelta&&(v=0>v?-1:1),(v>0&&0!==p||0>v&&p!==g||i.mouseWheel.preventDefault)&&(t.stopImmediatePropagation(),t.preventDefault()),t.deltaFactor<5&&!i.mouseWheel.normalizeDelta&&(h=t.deltaFactor,d=17),G(o,(m-v*h).toString(),{dir:u,dur:d}))}}if(e(this).data(a)){var o=e(this),n=o.data(a),i=n.opt,r=a+"_"+n.idx,l=e("#mCSB_"+n.idx),c=[e("#mCSB_"+n.idx+"_dragger_vertical"),e("#mCSB_"+n.idx+"_dragger_horizontal")],d=e("#mCSB_"+n.idx+"_container").find("iframe");d.length&&d.each(function(){e(this).bind("load",function(){A(this)&&e(this.contentDocument||this.contentWindow.document).bind("mousewheel."+r,function(e,o){t(e,o)})})}),l.bind("mousewheel."+r,function(e,o){t(e,o)})}},R=new Object,A=function(t){var o=!1,a=!1,n=null;if(void 0===t?a="#empty":void 0!==e(t).attr("id")&&(a=e(t).attr("id")),a!==!1&&void 0!==R[a])return R[a];if(t){try{var i=t.contentDocument||t.contentWindow.document;n=i.body.innerHTML}catch(r){}o=null!==n}else{try{var i=top.document;n=i.body.innerHTML}catch(r){}o=null!==n}return a!==!1&&(R[a]=o),o},L=function(e){var t=this.find("iframe");if(t.length){var o=e?"auto":"none";t.css("pointer-events",o)}},z=function(t,o){var n=o.nodeName.toLowerCase(),i=t.data(a).opt.mouseWheel.disableOver,r=["select","textarea"];return e.inArray(n,i)>-1&&!(e.inArray(n,r)>-1&&!e(o).is(":focus"))},P=function(){var t,o=e(this),n=o.data(a),i=a+"_"+n.idx,r=e("#mCSB_"+n.idx+"_container"),l=r.parent(),s=e(".mCSB_"+n.idx+"_scrollbar ."+d[12]);s.bind("mousedown."+i+" touchstart."+i+" pointerdown."+i+" MSPointerDown."+i,function(o){c=!0,e(o.target).hasClass("mCSB_dragger")||(t=1)}).bind("touchend."+i+" pointerup."+i+" MSPointerUp."+i,function(){c=!1}).bind("click."+i,function(a){if(t&&(t=0,e(a.target).hasClass(d[12])||e(a.target).hasClass("mCSB_draggerRail"))){Q(o);var i=e(this),s=i.find(".mCSB_dragger");if(i.parent(".mCSB_scrollTools_horizontal").length>0){if(!n.overflowed[1])return;var c="x",u=a.pageX>s.offset().left?-1:1,f=Math.abs(r[0].offsetLeft)-u*(.9*l.width())}else{if(!n.overflowed[0])return;var c="y",u=a.pageY>s.offset().top?-1:1,f=Math.abs(r[0].offsetTop)-u*(.9*l.height())}G(o,f.toString(),{dir:c,scrollEasing:"mcsEaseInOut"})}})},H=function(){var t=e(this),o=t.data(a),n=o.opt,i=a+"_"+o.idx,r=e("#mCSB_"+o.idx+"_container"),l=r.parent();r.bind("focusin."+i,function(){var o=e(document.activeElement),a=r.find(".mCustomScrollBox").length,i=0;o.is(n.advanced.autoScrollOnFocus)&&(Q(t),clearTimeout(t[0]._focusTimeout),t[0]._focusTimer=a?(i+17)*a:0,t[0]._focusTimeout=setTimeout(function(){var e=[ae(o)[0],ae(o)[1]],a=[r[0].offsetTop,r[0].offsetLeft],s=[a[0]+e[0]>=0&&a[0]+e[0]<l.height()-o.outerHeight(!1),a[1]+e[1]>=0&&a[0]+e[1]<l.width()-o.outerWidth(!1)],c="yx"!==n.axis||s[0]||s[1]?"all":"none";"x"===n.axis||s[0]||G(t,e[0].toString(),{dir:"y",scrollEasing:"mcsEaseInOut",overwrite:c,dur:i}),"y"===n.axis||s[1]||G(t,e[1].toString(),{dir:"x",scrollEasing:"mcsEaseInOut",overwrite:c,dur:i})},t[0]._focusTimer))})},U=function(){var t=e(this),o=t.data(a),n=a+"_"+o.idx,i=e("#mCSB_"+o.idx+"_container").parent();i.bind("scroll."+n,function(){0===i.scrollTop()&&0===i.scrollLeft()||e(".mCSB_"+o.idx+"_scrollbar").css("visibility","hidden")})},F=function(){var t=e(this),o=t.data(a),n=o.opt,i=o.sequential,r=a+"_"+o.idx,l=".mCSB_"+o.idx+"_scrollbar",s=e(l+">a");s.bind("contextmenu."+r,function(e){e.preventDefault()}).bind("mousedown."+r+" touchstart."+r+" pointerdown."+r+" MSPointerDown."+r+" mouseup."+r+" touchend."+r+" pointerup."+r+" MSPointerUp."+r+" mouseout."+r+" pointerout."+r+" MSPointerOut."+r+" click."+r,function(a){function r(e,o){i.scrollAmount=n.scrollButtons.scrollAmount,j(t,e,o)}if(a.preventDefault(),ee(a)){var l=e(this).attr("class");switch(i.type=n.scrollButtons.scrollType,a.type){case"mousedown":case"touchstart":case"pointerdown":case"MSPointerDown":if("stepped"===i.type)return;c=!0,o.tweenRunning=!1,r("on",l);break;case"mouseup":case"touchend":case"pointerup":case"MSPointerUp":case"mouseout":case"pointerout":case"MSPointerOut":if("stepped"===i.type)return;c=!1,i.dir&&r("off",l);break;case"click":if("stepped"!==i.type||o.tweenRunning)return;r("on",l)}}})},q=function(){function t(t){function a(e,t){r.type=i.keyboard.scrollType,r.scrollAmount=i.keyboard.scrollAmount,"stepped"===r.type&&n.tweenRunning||j(o,e,t)}switch(t.type){case"blur":n.tweenRunning&&r.dir&&a("off",null);break;case"keydown":case"keyup":var l=t.keyCode?t.keyCode:t.which,s="on";if("x"!==i.axis&&(38===l||40===l)||"y"!==i.axis&&(37===l||39===l)){if((38===l||40===l)&&!n.overflowed[0]||(37===l||39===l)&&!n.overflowed[1])return;"keyup"===t.type&&(s="off"),e(document.activeElement).is(u)||(t.preventDefault(),t.stopImmediatePropagation(),a(s,l))}else if(33===l||34===l){if((n.overflowed[0]||n.overflowed[1])&&(t.preventDefault(),t.stopImmediatePropagation()),"keyup"===t.type){Q(o);var f=34===l?-1:1;if("x"===i.axis||"yx"===i.axis&&n.overflowed[1]&&!n.overflowed[0])var h="x",m=Math.abs(c[0].offsetLeft)-f*(.9*d.width());else var h="y",m=Math.abs(c[0].offsetTop)-f*(.9*d.height());G(o,m.toString(),{dir:h,scrollEasing:"mcsEaseInOut"})}}else if((35===l||36===l)&&!e(document.activeElement).is(u)&&((n.overflowed[0]||n.overflowed[1])&&(t.preventDefault(),t.stopImmediatePropagation()),"keyup"===t.type)){if("x"===i.axis||"yx"===i.axis&&n.overflowed[1]&&!n.overflowed[0])var h="x",m=35===l?Math.abs(d.width()-c.outerWidth(!1)):0;else var h="y",m=35===l?Math.abs(d.height()-c.outerHeight(!1)):0;G(o,m.toString(),{dir:h,scrollEasing:"mcsEaseInOut"})}}}var o=e(this),n=o.data(a),i=n.opt,r=n.sequential,l=a+"_"+n.idx,s=e("#mCSB_"+n.idx),c=e("#mCSB_"+n.idx+"_container"),d=c.parent(),u="input,textarea,select,datalist,keygen,[contenteditable='true']",f=c.find("iframe"),h=["blur."+l+" keydown."+l+" keyup."+l];f.length&&f.each(function(){e(this).bind("load",function(){A(this)&&e(this.contentDocument||this.contentWindow.document).bind(h[0],function(e){t(e)})})}),s.attr("tabindex","0").bind(h[0],function(e){t(e)})},j=function(t,o,n,i,r){function l(e){u.snapAmount&&(f.scrollAmount=u.snapAmount instanceof Array?"x"===f.dir[0]?u.snapAmount[1]:u.snapAmount[0]:u.snapAmount);var o="stepped"!==f.type,a=r?r:e?o?p/1.5:g:1e3/60,n=e?o?7.5:40:2.5,s=[Math.abs(h[0].offsetTop),Math.abs(h[0].offsetLeft)],d=[c.scrollRatio.y>10?10:c.scrollRatio.y,c.scrollRatio.x>10?10:c.scrollRatio.x],m="x"===f.dir[0]?s[1]+f.dir[1]*(d[1]*n):s[0]+f.dir[1]*(d[0]*n),v="x"===f.dir[0]?s[1]+f.dir[1]*parseInt(f.scrollAmount):s[0]+f.dir[1]*parseInt(f.scrollAmount),x="auto"!==f.scrollAmount?v:m,_=i?i:e?o?"mcsLinearOut":"mcsEaseInOut":"mcsLinear",w=!!e;return e&&17>a&&(x="x"===f.dir[0]?s[1]:s[0]),G(t,x.toString(),{dir:f.dir[0],scrollEasing:_,dur:a,onComplete:w}),e?void(f.dir=!1):(clearTimeout(f.step),void(f.step=setTimeout(function(){l()},a)))}function s(){clearTimeout(f.step),$(f,"step"),Q(t)}var c=t.data(a),u=c.opt,f=c.sequential,h=e("#mCSB_"+c.idx+"_container"),m="stepped"===f.type,p=u.scrollInertia<26?26:u.scrollInertia,g=u.scrollInertia<1?17:u.scrollInertia;switch(o){case"on":if(f.dir=[n===d[16]||n===d[15]||39===n||37===n?"x":"y",n===d[13]||n===d[15]||38===n||37===n?-1:1],Q(t),oe(n)&&"stepped"===f.type)return;l(m);break;case"off":s(),(m||c.tweenRunning&&f.dir)&&l(!0)}},Y=function(t){var o=e(this).data(a).opt,n=[];return"function"==typeof t&&(t=t()),t instanceof Array?n=t.length>1?[t[0],t[1]]:"x"===o.axis?[null,t[0]]:[t[0],null]:(n[0]=t.y?t.y:t.x||"x"===o.axis?null:t,n[1]=t.x?t.x:t.y||"y"===o.axis?null:t),"function"==typeof n[0]&&(n[0]=n[0]()),"function"==typeof n[1]&&(n[1]=n[1]()),n},X=function(t,o){if(null!=t&&"undefined"!=typeof t){var n=e(this),i=n.data(a),r=i.opt,l=e("#mCSB_"+i.idx+"_container"),s=l.parent(),c=typeof t;o||(o="x"===r.axis?"x":"y");var d="x"===o?l.outerWidth(!1)-s.width():l.outerHeight(!1)-s.height(),f="x"===o?l[0].offsetLeft:l[0].offsetTop,h="x"===o?"left":"top";switch(c){case"function":return t();case"object":var m=t.jquery?t:e(t);if(!m.length)return;return"x"===o?ae(m)[1]:ae(m)[0];case"string":case"number":if(oe(t))return Math.abs(t);if(-1!==t.indexOf("%"))return Math.abs(d*parseInt(t)/100);if(-1!==t.indexOf("-="))return Math.abs(f-parseInt(t.split("-=")[1]));if(-1!==t.indexOf("+=")){var p=f+parseInt(t.split("+=")[1]);return p>=0?0:Math.abs(p)}if(-1!==t.indexOf("px")&&oe(t.split("px")[0]))return Math.abs(t.split("px")[0]);if("top"===t||"left"===t)return 0;if("bottom"===t)return Math.abs(s.height()-l.outerHeight(!1));if("right"===t)return Math.abs(s.width()-l.outerWidth(!1));if("first"===t||"last"===t){var m=l.find(":"+t);return"x"===o?ae(m)[1]:ae(m)[0]}return e(t).length?"x"===o?ae(e(t))[1]:ae(e(t))[0]:(l.css(h,t),void u.update.call(null,n[0]))}}},N=function(t){function o(){return clearTimeout(f[0].autoUpdate),0===l.parents("html").length?void(l=null):void(f[0].autoUpdate=setTimeout(function(){return c.advanced.updateOnSelectorChange&&(s.poll.change.n=i(),s.poll.change.n!==s.poll.change.o)?(s.poll.change.o=s.poll.change.n,void r(3)):c.advanced.updateOnContentResize&&(s.poll.size.n=l[0].scrollHeight+l[0].scrollWidth+f[0].offsetHeight+l[0].offsetHeight+l[0].offsetWidth,s.poll.size.n!==s.poll.size.o)?(s.poll.size.o=s.poll.size.n,void r(1)):!c.advanced.updateOnImageLoad||"auto"===c.advanced.updateOnImageLoad&&"y"===c.axis||(s.poll.img.n=f.find("img").length,s.poll.img.n===s.poll.img.o)?void((c.advanced.updateOnSelectorChange||c.advanced.updateOnContentResize||c.advanced.updateOnImageLoad)&&o()):(s.poll.img.o=s.poll.img.n,void f.find("img").each(function(){n(this)}))},c.advanced.autoUpdateTimeout))}function n(t){function o(e,t){return function(){
	return t.apply(e,arguments)}}function a(){this.onload=null,e(t).addClass(d[2]),r(2)}if(e(t).hasClass(d[2]))return void r();var n=new Image;n.onload=o(n,a),n.src=t.src}function i(){c.advanced.updateOnSelectorChange===!0&&(c.advanced.updateOnSelectorChange="*");var e=0,t=f.find(c.advanced.updateOnSelectorChange);return c.advanced.updateOnSelectorChange&&t.length>0&&t.each(function(){e+=this.offsetHeight+this.offsetWidth}),e}function r(e){clearTimeout(f[0].autoUpdate),u.update.call(null,l[0],e)}var l=e(this),s=l.data(a),c=s.opt,f=e("#mCSB_"+s.idx+"_container");return t?(clearTimeout(f[0].autoUpdate),void $(f[0],"autoUpdate")):void o()},V=function(e,t,o){return Math.round(e/t)*t-o},Q=function(t){var o=t.data(a),n=e("#mCSB_"+o.idx+"_container,#mCSB_"+o.idx+"_container_wrapper,#mCSB_"+o.idx+"_dragger_vertical,#mCSB_"+o.idx+"_dragger_horizontal");n.each(function(){Z.call(this)})},G=function(t,o,n){function i(e){return s&&c.callbacks[e]&&"function"==typeof c.callbacks[e]}function r(){return[c.callbacks.alwaysTriggerOffsets||w>=S[0]+y,c.callbacks.alwaysTriggerOffsets||-B>=w]}function l(){var e=[h[0].offsetTop,h[0].offsetLeft],o=[x[0].offsetTop,x[0].offsetLeft],a=[h.outerHeight(!1),h.outerWidth(!1)],i=[f.height(),f.width()];t[0].mcs={content:h,top:e[0],left:e[1],draggerTop:o[0],draggerLeft:o[1],topPct:Math.round(100*Math.abs(e[0])/(Math.abs(a[0])-i[0])),leftPct:Math.round(100*Math.abs(e[1])/(Math.abs(a[1])-i[1])),direction:n.dir}}var s=t.data(a),c=s.opt,d={trigger:"internal",dir:"y",scrollEasing:"mcsEaseOut",drag:!1,dur:c.scrollInertia,overwrite:"all",callbacks:!0,onStart:!0,onUpdate:!0,onComplete:!0},n=e.extend(d,n),u=[n.dur,n.drag?0:n.dur],f=e("#mCSB_"+s.idx),h=e("#mCSB_"+s.idx+"_container"),m=h.parent(),p=c.callbacks.onTotalScrollOffset?Y.call(t,c.callbacks.onTotalScrollOffset):[0,0],g=c.callbacks.onTotalScrollBackOffset?Y.call(t,c.callbacks.onTotalScrollBackOffset):[0,0];if(s.trigger=n.trigger,0===m.scrollTop()&&0===m.scrollLeft()||(e(".mCSB_"+s.idx+"_scrollbar").css("visibility","visible"),m.scrollTop(0).scrollLeft(0)),"_resetY"!==o||s.contentReset.y||(i("onOverflowYNone")&&c.callbacks.onOverflowYNone.call(t[0]),s.contentReset.y=1),"_resetX"!==o||s.contentReset.x||(i("onOverflowXNone")&&c.callbacks.onOverflowXNone.call(t[0]),s.contentReset.x=1),"_resetY"!==o&&"_resetX"!==o){if(!s.contentReset.y&&t[0].mcs||!s.overflowed[0]||(i("onOverflowY")&&c.callbacks.onOverflowY.call(t[0]),s.contentReset.x=null),!s.contentReset.x&&t[0].mcs||!s.overflowed[1]||(i("onOverflowX")&&c.callbacks.onOverflowX.call(t[0]),s.contentReset.x=null),c.snapAmount){var v=c.snapAmount instanceof Array?"x"===n.dir?c.snapAmount[1]:c.snapAmount[0]:c.snapAmount;o=V(o,v,c.snapOffset)}switch(n.dir){case"x":var x=e("#mCSB_"+s.idx+"_dragger_horizontal"),_="left",w=h[0].offsetLeft,S=[f.width()-h.outerWidth(!1),x.parent().width()-x.width()],b=[o,0===o?0:o/s.scrollRatio.x],y=p[1],B=g[1],T=y>0?y/s.scrollRatio.x:0,k=B>0?B/s.scrollRatio.x:0;break;case"y":var x=e("#mCSB_"+s.idx+"_dragger_vertical"),_="top",w=h[0].offsetTop,S=[f.height()-h.outerHeight(!1),x.parent().height()-x.height()],b=[o,0===o?0:o/s.scrollRatio.y],y=p[0],B=g[0],T=y>0?y/s.scrollRatio.y:0,k=B>0?B/s.scrollRatio.y:0}b[1]<0||0===b[0]&&0===b[1]?b=[0,0]:b[1]>=S[1]?b=[S[0],S[1]]:b[0]=-b[0],t[0].mcs||(l(),i("onInit")&&c.callbacks.onInit.call(t[0])),clearTimeout(h[0].onCompleteTimeout),J(x[0],_,Math.round(b[1]),u[1],n.scrollEasing),!s.tweenRunning&&(0===w&&b[0]>=0||w===S[0]&&b[0]<=S[0])||J(h[0],_,Math.round(b[0]),u[0],n.scrollEasing,n.overwrite,{onStart:function(){n.callbacks&&n.onStart&&!s.tweenRunning&&(i("onScrollStart")&&(l(),c.callbacks.onScrollStart.call(t[0])),s.tweenRunning=!0,C(x),s.cbOffsets=r())},onUpdate:function(){n.callbacks&&n.onUpdate&&i("whileScrolling")&&(l(),c.callbacks.whileScrolling.call(t[0]))},onComplete:function(){if(n.callbacks&&n.onComplete){"yx"===c.axis&&clearTimeout(h[0].onCompleteTimeout);var e=h[0].idleTimer||0;h[0].onCompleteTimeout=setTimeout(function(){i("onScroll")&&(l(),c.callbacks.onScroll.call(t[0])),i("onTotalScroll")&&b[1]>=S[1]-T&&s.cbOffsets[0]&&(l(),c.callbacks.onTotalScroll.call(t[0])),i("onTotalScrollBack")&&b[1]<=k&&s.cbOffsets[1]&&(l(),c.callbacks.onTotalScrollBack.call(t[0])),s.tweenRunning=!1,h[0].idleTimer=0,C(x,"hide")},e)}}})}},J=function(e,t,o,a,n,i,r){function l(){S.stop||(x||m.call(),x=K()-v,s(),x>=S.time&&(S.time=x>S.time?x+f-(x-S.time):x+f-1,S.time<x+1&&(S.time=x+1)),S.time<a?S.id=h(l):g.call())}function s(){a>0?(S.currVal=u(S.time,_,b,a,n),w[t]=Math.round(S.currVal)+"px"):w[t]=o+"px",p.call()}function c(){f=1e3/60,S.time=x+f,h=window.requestAnimationFrame?window.requestAnimationFrame:function(e){return s(),setTimeout(e,.01)},S.id=h(l)}function d(){null!=S.id&&(window.requestAnimationFrame?window.cancelAnimationFrame(S.id):clearTimeout(S.id),S.id=null)}function u(e,t,o,a,n){switch(n){case"linear":case"mcsLinear":return o*e/a+t;case"mcsLinearOut":return e/=a,e--,o*Math.sqrt(1-e*e)+t;case"easeInOutSmooth":return e/=a/2,1>e?o/2*e*e+t:(e--,-o/2*(e*(e-2)-1)+t);case"easeInOutStrong":return e/=a/2,1>e?o/2*Math.pow(2,10*(e-1))+t:(e--,o/2*(-Math.pow(2,-10*e)+2)+t);case"easeInOut":case"mcsEaseInOut":return e/=a/2,1>e?o/2*e*e*e+t:(e-=2,o/2*(e*e*e+2)+t);case"easeOutSmooth":return e/=a,e--,-o*(e*e*e*e-1)+t;case"easeOutStrong":return o*(-Math.pow(2,-10*e/a)+1)+t;case"easeOut":case"mcsEaseOut":default:var i=(e/=a)*e,r=i*e;return t+o*(.499999999999997*r*i+-2.5*i*i+5.5*r+-6.5*i+4*e)}}e._mTween||(e._mTween={top:{},left:{}});var f,h,r=r||{},m=r.onStart||function(){},p=r.onUpdate||function(){},g=r.onComplete||function(){},v=K(),x=0,_=e.offsetTop,w=e.style,S=e._mTween[t];"left"===t&&(_=e.offsetLeft);var b=o-_;S.stop=0,"none"!==i&&d(),c()},K=function(){return window.performance&&window.performance.now?window.performance.now():window.performance&&window.performance.webkitNow?window.performance.webkitNow():Date.now?Date.now():(new Date).getTime()},Z=function(){var e=this;e._mTween||(e._mTween={top:{},left:{}});for(var t=["top","left"],o=0;o<t.length;o++){var a=t[o];e._mTween[a].id&&(window.requestAnimationFrame?window.cancelAnimationFrame(e._mTween[a].id):clearTimeout(e._mTween[a].id),e._mTween[a].id=null,e._mTween[a].stop=1)}},$=function(e,t){try{delete e[t]}catch(o){e[t]=null}},ee=function(e){return!(e.which&&1!==e.which)},te=function(e){var t=e.originalEvent.pointerType;return!(t&&"touch"!==t&&2!==t)},oe=function(e){return!isNaN(parseFloat(e))&&isFinite(e)},ae=function(e){var t=e.parents(".mCSB_container");return[e.offset().top-t.offset().top,e.offset().left-t.offset().left]},ne=function(){function e(){var e=["webkit","moz","ms","o"];if("hidden"in document)return"hidden";for(var t=0;t<e.length;t++)if(e[t]+"Hidden"in document)return e[t]+"Hidden";return null}var t=e();return t?document[t]:!1};e.fn[o]=function(t){return u[t]?u[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist"):u.init.apply(this,arguments)},e[o]=function(t){return u[t]?u[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist"):u.init.apply(this,arguments)},e[o].defaults=i,window[o]=!0,e(window).bind("load",function(){e(n)[o](),e.extend(e.expr[":"],{mcsInView:e.expr[":"].mcsInView||function(t){var o,a,n=e(t),i=n.parents(".mCSB_container");if(i.length)return o=i.parent(),a=[i[0].offsetTop,i[0].offsetLeft],a[0]+ae(n)[0]>=0&&a[0]+ae(n)[0]<o.height()-n.outerHeight(!1)&&a[1]+ae(n)[1]>=0&&a[1]+ae(n)[1]<o.width()-n.outerWidth(!1)},mcsInSight:e.expr[":"].mcsInSight||function(t,o,a){var n,i,r,l,s=e(t),c=s.parents(".mCSB_container"),d="exact"===a[3]?[[1,0],[1,0]]:[[.9,.1],[.6,.4]];if(c.length)return n=[s.outerHeight(!1),s.outerWidth(!1)],r=[c[0].offsetTop+ae(s)[0],c[0].offsetLeft+ae(s)[1]],i=[c.parent()[0].offsetHeight,c.parent()[0].offsetWidth],l=[n[0]<i[0]?d[0]:d[1],n[1]<i[1]?d[0]:d[1]],r[0]-i[0]*l[0][0]<0&&r[0]+n[0]-i[0]*l[0][1]>=0&&r[1]-i[1]*l[1][0]<0&&r[1]+n[1]-i[1]*l[1][1]>=0},mcsOverflow:e.expr[":"].mcsOverflow||function(t){var o=e(t).data(a);if(o)return o.overflowed[0]||o.overflowed[1]}})})})});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 37 */
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
/* 38 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(39)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Home) {
	    $(Home.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 39 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(40), __webpack_require__(43), __webpack_require__(44), __webpack_require__(45)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site) {
	    var $searchBox;
	    var _autocomplete;
	
	    
	    function initialize() {
	        Site.initialize();    
	        //initClock();
	        initVariables();
	        initEvent();
	    }
	   
	    function initVariables() {
	        $searchBox = $('#searchbox');
	    };
	
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
	
	        $('#ddlLicensee').on('click', '.licenseeItem', function () {
	            var licenseeId = $(this).attr('licenseeId');
	            top.location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
	        });
	
	        $('body').click(function () {
	            _autocomplete.$dropdown.html('').hide();
	        });
	    }
	
	    function searchByUserName() {
	        var userName = $searchBox.val();
	        if (!userName) {
	            return;
	        }
	
	        var url = Site.resolveClientUrl("Customer/CustomerList?userName=" + userName);
	        top.main.location.href = url;
	        $('.collapsible2').collapsibleMenu('activeMenuItem', $('#MenuItem43'));
	    }
	
	    function initAutoComplete() {
	        _autocomplete = $searchBox.materialize_autocomplete({
	            ignoreCase: false,
	            multiple: {
	                enable: false
	            },
	            dropdown: {
	                el: '#suggestionDropdown'
	            },
	            getData: function (value, callback) {
	                var url = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0&query=" + value);
	
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
	
	    function initClock() {
	        var options = {
	            utc: true,
	            utcOffset: -4,
	            format: '%H:%M:%S %b %d, %Y GMT'
	        };
	        $('#current-time-clock').jclock(options);
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
	
	    return {
	        initialize: initialize
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 40 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/*global _language_en_US*/
	/// <reference path="/@JSense.js" />
	/* Preset */
	// root JSON object
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(41), __webpack_require__(2), __webpack_require__(42)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, QueryString) {
	    "use strict";
	    var UNBLOCKUI_TIMEOUT = 30000;
	    var BASE_CURRENT_LIST = ["EUR", "USD", "GBP"];
	    var REQUEST_VERIFICATION_TOKEN;
	    var _historyDate = null;
	    var _licenseeId = null;
	    var _isLoadedNotificationPopup = false;
	    var _rootUrl = null;
	    var _pageLanguage = null;
	
	    function addNoticationPopup() {
	        var $marquee = $("#notification-marquee");
	        if ("marquee" in $marquee) {
	            $marquee.marquee({ speed: 30 });
	        }
	
	        if (_isLoadedNotificationPopup === true) {
	            return;
	        }
	
	        var $popup = $("#NotificationPopup");
	        if ($popup.length > 0) {
	            var options = {
	                height: 160,
	                title: getSingleResource("Notification")
	            };
	            var popup = $popup.flatPopup(options);
	            popup.show();
	
	            $popup.find(".popup-close").click(function () {
	                $.cookie("isClosedNotification", "1", { path: '/' });
	            });
	            _isLoadedNotificationPopup = true;
	        }
	    }
	
	    function resolveClientUrl(relativeUrl) {
	        return _rootUrl + relativeUrl;
	    }
	
	    function setLicenseeOption(urlOrParams, licenseeType, currencyId, currencyName, baseCurrency) {
	        licenseeType = licenseeType === undefined ? "" : licenseeType;
	        currencyName = $.trim(currencyName);
	        // Set Url
	        if (typeof (urlOrParams) === "string") {
	            urlOrParams = QueryString.setParam(urlOrParams, "LicenseeType", licenseeType);
	            urlOrParams = QueryString.setParam(urlOrParams, "CurrencyId", currencyId);
	            urlOrParams = QueryString.setParam(urlOrParams, "CurrencyName", currencyName);
	            urlOrParams = QueryString.setParam(urlOrParams, "BaseCurrency", baseCurrency);
	        }
	        else {
	            urlOrParams.LicenseeType = licenseeType;
	            urlOrParams.CurrencyId = currencyId;
	            urlOrParams.CurrencyName = currencyName;
	            urlOrParams.BaseCurrency = baseCurrency;
	        }
	
	        return urlOrParams;
	    }
	
	    function getDifferenceMonth(fromDate, toDate) {
	        return toDate.getMonth() - fromDate.getMonth() + (12 * (toDate.getFullYear() - fromDate.getFullYear())) + 1;
	    }
	
	    function initialize() {
	        REQUEST_VERIFICATION_TOKEN = $("input[name=__RequestVerificationToken]").val();
	        currencyConstraint();
	        initEvent();
	        _historyDate = new Date($("#HistoryDate").val());
	        _rootUrl = $('#RootUrl').val();
	        _pageLanguage = $('#PageLanguage').val();
	        initBlockUI();
	        addNoticationPopup();
	        overrideInnerSize();
	    }
	    function initEvent() {
	        $(window).resize(overrideInnerSize);
	
	        $("#CurrencyId").change(function () {
	            currencyConstraint(true);
	        });
	
	        $.ajaxSetup({
	            cache: false,
	            headers: { '__RequestVerificationToken': REQUEST_VERIFICATION_TOKEN },
	            statusCode: {
	                0: unBlockUI(),
	                401: function (/*jqXHR, textStatus, errorThrown*/) {
	                    window.location.href = resolveClientUrl("Utility/LocationRestriction");
	                },
	                403: function () {
	                    unBlockUI();
	                    alert(getSingleResource("HttpError_403"));
	                },
	                500: function () {
	                    unBlockUI();
	                    alert(getSingleResource("ErrorProcessingMsg"));
	                }
	            }
	        });
	    }
	
	    function exportExcel(data, url, method) {
	        var $form = $('<form id="ExportExcelForm">').attr({ action: url, method: method || 'post' })
	            .appendTo('body');
	        for (var property in data) {
	            if (data.hasOwnProperty(property)) {
	                var value = data[property];
	                if (property === "ProductOptions") {
	                    var products = typeof (value) === "object" ? value : JSON.parse(value);
	                    for (var i = 0; i < products.length; i++) {
	                        var product = products[i];
	                        $('<input type="hidden">').attr({ name: 'ProductOptions[' + i + '].Code', value: product.Code }).appendTo($form);
	                        $('<input type="hidden">').attr({ name: 'ProductOptions[' + i + '].Status', value: product.Status }).appendTo($form);
	                    }
	                }
	                else {
	                    $('<input type="hidden">').attr({ name: property, value: value }).appendTo($form);
	                }
	            }
	        }
	
	        $('<input type="hidden">').attr({ name: '__RequestVerificationToken', value: REQUEST_VERIFICATION_TOKEN }).appendTo($form);
	        $form.submit();
	        $form.remove();
	    }
	
	    function setConstraintMonthRange(monthRange, url) {
	        var errorMessage = getSingleResource("DateRangeExceedMsg").replace("{0}", monthRange);
	
	        function isValidRange() {
	            var fromDate = new Date($("#FromDate").val());
	            var toDate = new Date($("#ToDate").val());
	
	            return getDifferenceMonth(fromDate, toDate) <= monthRange;
	        }
	
	        if (url && typeof url === "string") {
	            $(document).ajaxSend(function (event, jqxhr, settings) {
	                if (settings.url.search(new RegExp(url, "i")) > -1 && isValidRange() === false) {
	                    showMessaegBox(errorMessage);
	                    jqxhr.abort();
	                }
	            });
	        }
	
	        $("form").submit(function () {
	            if (isValidRange() === false) {
	                showMessaegBox(errorMessage);
	                return false;
	            }
	
	            return true;
	        });
	    }
	    function showMessaegBox(message) {
	        $('<div title="' + getSingleResource("LrfNotification") + '">' + message + '!</div>').dialog();
	    }
	    function getLicenseeId() {
	        if (!_licenseeId) {
	            _licenseeId = parseInt($("#LicenseeId").val());
	        }
	
	        return _licenseeId;
	    }
	    function getCurrencyName(currencyId) {
	        return userCurrencies[currencyId];
	    }
	    function getHistoryDate() {
	        return _historyDate;
	    }
	    function isBaseCurrency(currencyName) {
	        return $.inArray(currencyName.toUpperCase(), BASE_CURRENT_LIST) > -1;
	    }
	    function addUserGuide(options) {
	        var defaultOptions = {
	            buttonSelector: "#UserGuide"
	        };
	        options = $.extend(defaultOptions, options);
	        options.title = options.title || "";
	        $("body").prepend('<div id="UserGuidePopup" style="display: none" ></div>');
	        var url = resolveClientUrl('UserGuide/Index?resourcename=' + options.resourceName) + "&_" + (new Date().getTime());
	        var popup;
	        $(options.buttonSelector).click(function () {
	            var $userGuidePopup = $("#UserGuidePopup");
	            if ($userGuidePopup.is(":visible")) {
	                return;
	            }
	
	            if (!popup) {
	                $userGuidePopup.empty().attr('style', '').hide()
	                    .load(url, function (response, status) {
	                        if (status === "success") {
	                            setTimeout(function () {
	                                popup = $userGuidePopup.flatPopup(options);
	                                popup.show();
	                            }, 50);
	                        } else {
	                            $userGuidePopup.hide();
	                        }
	                    });
	            } else {
	                popup.show();
	            }
	        });
	    }
	    function overrideInnerSize() {
	        if (window.innerHeight === undefined) {
	            parent.innerHeight = parent.document.documentElement.clientHeight;
	            window.innerHeight = window.document.documentElement.clientHeight;
	            parent.innerWidth = parent.document.documentElement.clientWidth;
	            window.innerWidth = window.document.documentElement.clientWidth;
	        }
	    }
	    function addFilterCollapseExpand(containerSelector) {
	        if (!containerSelector) {
	            containerSelector = ".content-filter:has(form)";
	        }
	
	        var $handler = $("<div class='filter-handle filter-handle-collapse'></div>");
	        var $container = $(containerSelector);
	
	        $container.append($handler);
	
	        var originalHeight = $container.height();
	        var originalTop = originalHeight - 15;
	        var isCollapsed = false;
	        $handler.css("top", originalTop);
	        $handler.click(function () {
	            isCollapsed = !isCollapsed;
	            $container.find("*").toggle();
	            $container.height(isCollapsed ? 5 : originalHeight + 5);
	            $handler.css("top", isCollapsed ? -15 : originalTop);
	            $(this).show().toggleClass("filter-handle-collapse filter-handle-expand");
	            // $container.css("background", isCollapsed ? "#E7E7E7" : "").addClass("content-filter");
	            $(window).resize();
	        });
	    }
	    function asNull(text) {
	        if (text != null) {
	            text = text.trim();
	            if (text === '') {
	                return null;
	            }
	        }
	
	        return text;
	    }
	
	    function gotoErrorPage(status) {
	        if (status === undefined || status !== 0) {
	            window.location.href = resolveClientUrl("Utility/Error");
	        } else {
	            unBlockUI();
	        }
	    }
	
	    function unBlockUI() {
	        $('div.block-body').hide();
	        $('div.loading').hide();
	    }
	    function blockUI() {
	        $('div.block-body').show();
	        $('div.loading').show();
	    }
	
	    function setLicenseeOptionParam(urlOrParams) {
	        var licenseeType = $("#LicenseeType").val();
	        var currencyId = $("#CurrencyId").val();
	        var baseCurrency = $("#BaseCurrency").val();
	        var currencyName = $("#CurrencyId option[value=" + currencyId + "]").text();
	        if (currencyName === undefined) {
	            currencyName = $("#CurrencyName").val();
	        }
	
	        return setLicenseeOption(urlOrParams || {}, licenseeType, currencyId, currencyName, baseCurrency);
	    }
	
	    function setLicenseeOptionOriginalParam(urlOrParams) {
	        var licenseeType = $("#LicenseeType").attr('data-original');
	        var currencyId = $("#CurrencyId").attr('data-original');
	        var baseCurrency = $("#BaseCurrency").attr('data-original');
	        var currencyName = $("#CurrencyId option[value=" + currencyId + "]").text();
	        if (currencyName === undefined) {
	            currencyName = $("#CurrencyName").val();
	        }
	        return setLicenseeOption(urlOrParams, licenseeType, currencyId, currencyName, baseCurrency);
	    }
	
	    function updateOriginalLicenseeOption() {
	        var objectIdList = ["#LicenseeType", "#CurrencyId", "#BaseCurrency", "#CurrencyName"];
	        $(objectIdList).each(function (i, e) {
	            $(e).attr("data-original", $(e).val());
	        });
	    }
	
	    function setProductOptionParamList(urlOrParams) {
	        // Set Url
	        if (typeof (urlOrParams) === "string") {
	            var counter = 0;
	            $(".childCheckbox", "#ProductList").each(function (index, element) {
	                var productCode = $(element).attr('data-product-code');
	                if (productCode !== undefined && productCode.length > 0) {
	                    urlOrParams = setProductOptionParam(urlOrParams, counter, productCode, $(element).prop("checked"));
	                    counter++;
	                }
	            });
	
	            return urlOrParams;
	        }
	        else {
	            urlOrParams.ProductOptions = [];
	            $(".childCheckbox", "#ProductList").each(function (index, element) {
	                var productCode = $(element).attr('data-product-code');
	                if (productCode !== undefined && productCode.length > 0) {
	                    urlOrParams.ProductOptions.push({ Code: productCode, Name: productCode, Status: $(element).prop("checked") });
	                }
	            });
	
	            return urlOrParams;
	        }
	    }
	
	    function setProductOptionOriginalParamList(urlOrParams) {
	        if (typeof (urlOrParams) === "string") {
	            var counter = 0;
	            $(".childCheckbox", "#ProductList").each(function (index, element) {
	                var productCode = $(element).attr('data-product-code');
	                if (productCode !== undefined && productCode.length > 0) {
	                    urlOrParams = setProductOptionParam(urlOrParams, counter, productCode, $(element).attr("data-original"));
	                    counter++;
	                }
	            });
	
	            return urlOrParams;
	        }
	        else {
	            urlOrParams.ProductOptions = [];
	            $(".childCheckbox']", "#ProductList").each(function (index, element) {
	                var productCode = $(element).attr('data-product-code');
	                if (productCode !== undefined && productCode.length > 0) {
	                    urlOrParams.ProductOptions.push({ Code: productCode, Name: productCode, Status: $(element).attr("data-original") });
	                }
	            });
	
	            return urlOrParams;
	        }
	    }
	
	    function setProductOptionParam(url, counter, productCode, status) {
	        productCode = productCode == null || productCode === undefined ? undefined : productCode.toUpperCase();
	        url = QueryString.setParam(url, "ProductOptions[" + counter + "].Code", productCode);
	        url = QueryString.setParam(url, "ProductOptions[" + counter + "].Name", productCode);
	        url = QueryString.setParam(url, "ProductOptions[" + counter + "].Status", status);
	
	        return url;
	    }
	
	    function setDateRangeParam(url) {
	        url = QueryString.setParam(url, "FromDate", $("#FromDate").val());
	        url = QueryString.setParam(url, "ToDate", $("#ToDate").val());
	
	        return url;
	    }
	
	    function setDateRangeOriginalParam(url) {
	        url = QueryString.setParam(url, "FromDate", $("#FromDate").attr('data-original'));
	        url = QueryString.setParam(url, "ToDate", $("#ToDate").attr('data-original'));
	
	        return url;
	    }
	
	    function initBlockUI() {
	        try {
	            $(".blockui").click(function () {
	                blockUI();
	            });
	
	            unBlockUI();
	
	        } catch (err) {
	        }
	    }
	
	    function currencyConstraint(userChange) {
	        var currency = Number($("#CurrencyId").val());
	        var currencyName = $("#CurrencyId option:selected").text();
	        currencyName = $.trim(currencyName);
	        $("#CurrencyName").val(currencyName);
	
	        // Handle outside based currency list in some reports (customer wl, balance)
	        var $baseCurrency = $("#BaseCurrency");
	        var noConvertOption = $("option[value='']", $baseCurrency);
	        if (currency === 0 || isBaseCurrency(currencyName)) {
	            noConvertOption.remove();
	            if (userChange && currency !== 0) {
	                $("option[value='" + currencyName.toLowerCase() + "']", $baseCurrency).prop("selected", true);
	                $baseCurrency.val(currencyName.toLowerCase());
	            }
	        }
	        else {
	            if (noConvertOption.length === 0
	                && $baseCurrency.find('option').length > 0) { // make sure $baseCurrency is select object
	                $("<option value='' selected>" + currencyName + "</option>").appendTo($baseCurrency);
	            } else {
	                $("option[value='']", $baseCurrency).text(currencyName);
	            }
	            if (userChange) {
	                $baseCurrency.val("");
	                $("option[value='']", $baseCurrency).text(currencyName);
	            }
	        }
	
	        // Handle inside based currency list in some reports(licensee wl, wl by product)
	        if ($("option[value=-1]", "#CurrencyId").length > 0) {
	            var basedCurrencyMap = { "-1": "eur", "-2": "usd", "-3": "gbp" };
	            var basedCurrency = basedCurrencyMap[currency.toString()];
	            basedCurrency = basedCurrency === undefined ? "" : basedCurrency;
	            $baseCurrency.val(basedCurrency);
	        }
	        $("select#BaseCurrency").material_select();
	    }
	
	    function sortReport(sortColumn) {
	        var sortDirection = $("#SortColumn").val() === sortColumn ? $("#SortDirection").val() === "DESC" ? "ASC" : "DESC" : "ASC";
	
	        $("#SortColumn").val(sortColumn);
	        $("#SortDirection").val(sortDirection);
	
	        var currencyName = $("#CurrencyId option:selected").text();
	        currencyName = $.trim(currencyName);
	        $("#CurrencyName").val(currencyName);
	    }
	
	    function createSortingIcon(sortHandler) {
	        if ($("#RowCount").val() <= 1) {
	            $("#header-content .sort-heading").removeClass("sort-heading");
	        }
	        else {
	            $("#header-content .sort-heading").each(function (index, element) {
	                var headerDiv = $(element);
	                var sortColumn = headerDiv.attr("data-column-name");
	                if (sortColumn === $("#SortColumn").val()) {
	                    headerDiv.addClass("sorted-heading");
	
	                    var sortClass = $("#SortDirection").val() === "DESC" ? "sort-desc" : "sort-asc";
	                    var sortSpan = $("<span></span");
	                    sortSpan.addClass(sortClass);
	                    var left = 0;
	                    var top = 0;
	                    if (headerDiv.hasClass("row2")) {
	                        top = (headerDiv.height() - sortSpan.height()) / 2;
	                        sortSpan.css({
	                            top: top,
	                            right: 0
	                        });
	                    }
	                    else if ($(".grid-header-two-line", headerDiv).length > 0) {
	                        left = (headerDiv.width() - sortSpan.width()) / 2;
	                        sortSpan.css({
	                            top: 19,
	                            right: 0
	                        });
	                    }
	                    else {
	                        left = (headerDiv.width() - sortSpan.width()) / 2 + 35;
	                        sortSpan.css({
	                            top: 19,
	                            right: 0
	                        });
	                    }
	
	                    sortSpan.appendTo(headerDiv);
	                }
	
	                $(element).click(function () {
	                    sortReport($(element).attr("data-column-name"));
	                    sortHandler($(element).attr("data-column-name"));
	                });
	            });
	        }
	    }
	
	    function detectBrowser(userAgent) {
	        if (userAgent === undefined) { userAgent = navigator.userAgent; }
	        userAgent = userAgent.toLowerCase();
	        var match = /(chrome)[ \/]([\w.]+)/.exec(userAgent) || /(webkit)[ \/]([\w.]+)/.exec(userAgent) ||
	                /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(userAgent) || /(msie) ([\w.]+)/.exec(userAgent) ||
	                 userAgent.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(userAgent) || [];
	
	        return {
	            browser: match[1] || "",
	            version: match[2] || "0"
	        };
	    }
	
	    function getMaxHeight(element) {
	        var height = 0;
	
	        var title = 20;
	        var footer = 20;
	        var headerTitle = 26;
	        var headerFilter = $("div.page-form").outerHeight();
	        var exHeight = footer + headerTitle + headerFilter + title;
	
	        var body = window.document.body;
	        if (window.innerHeight) {
	            height = window.innerHeight;
	        } else if (body.parentElement.clientHeight) {
	            height = body.parentElement.clientHeight;
	        } else if (body && body.clientHeight) {
	            height = body.clientHeight;
	        }
	        var offset = element.offset();
	        return height - offset.top - exHeight;
	    }
	
	    function getMaxWidth() {
	        var width = 0;
	        var marginRight = 5;
	
	        var body = window.document.body;
	        if (window.innerWidth) {
	            width = window.innerWidth;
	        } else if (body.parentElement.clientWidth) {
	            width = body.parentElement.clientWidth;
	        } else if (body && body.clientWidth) {
	            width = body.clientWidth;
	        }
	
	        return width - marginRight;
	    }
	
	    function clone(obj) {
	        /// <summary>
	        ///
	        /// The below function will work adequatly for the six simple types I mentioned,
	        /// as long as data in the objects and arrays form a tree structure.That is, there isn't
	        /// more than one reference to the same data in the object.
	        /// Handle the 3 simple types, and null or undefined
	        /// </summary>
	        /// <param name="obj"></param>
	        /// <returns type=""></returns>
	
	        if (null == obj || "object" !== typeof obj) {
	            return obj;
	        }
	        // Handle Date
	        if (obj instanceof Date) {
	            var date = new Date();
	            date.setTime(obj.getTime());
	            return date;
	        }
	
	        // Handle Array
	        if (obj instanceof Array) {
	            var array = [];
	            for (var i = 0, len = obj.length; i < len; i++) {
	                array[i] = clone(obj[i]);
	            }
	            return array;
	        }
	
	        // Handle Object
	        if (obj instanceof Object) {
	            var copy = {};
	            for (var attr in obj) {
	                if (obj.hasOwnProperty(attr)) {
	                    copy[attr] = clone(obj[attr]);
	                }
	            }
	            return copy;
	        }
	
	        throw new Error("Unable to copy obj! Its type isn't supported.");
	    }
	
	    function asBool(input) {
	        if (typeof input === 'string') {
	            return input.toLowerCase() === 'true';
	        }
	
	        return !!input;
	    }
	    function getSingleResource(enghlishName) {
	        if (enghlishName == null || enghlishName === undefined) {
	            return enghlishName;
	        }
	
	        var keyName = "_language_" + _pageLanguage.replace("-", "_");
	
	        var resources = window[keyName];
	        if (!resources) {
	            resources = _language_en_US; // jshint ignore:line
	        }
	
	        var mapResource = function (key) {
	            var item = resources[key];
	            if (item === undefined) {
	                item = _language_en_US[key]; // jshint ignore:line
	                return item !== undefined ? item : key;
	            } else {
	                return item;
	            }
	        };
	        if (enghlishName.indexOf("@") > -1) {
	            var keys = enghlishName.match(/\@{1}\[{1}[a-zA-Z\d\_]+\]{1}/g);
	            if (keys != null) {
	                var temp = enghlishName;
	                for (var i = 0; i < keys.length; i++) {
	                    var realKey = keys[i].replace("@[", "").replace("]", "");
	                    temp = temp.replace(keys[i], mapResource(realKey));
	                }
	                return temp;
	            }
	            return mapResource(enghlishName);
	        }
	        else {
	            return mapResource(enghlishName);
	        }
	    }
	
	    function getResources(arrayText) {
	        if (arrayText == null) {
	            return null;
	        }
	        if (arrayText.constructor !== Array) {
	            return null;
	        }
	
	        var newArray = [];
	        for (var i = 0; i < arrayText.length; i++) {
	            newArray.push(getSingleResource(arrayText[i]));
	        }
	        return newArray;
	    }
	    function isNumber(text) {
	        return !isNaN(parseFloat(text)) && isFinite(text);
	    }
	    function isInteger(text) {
	        return typeof isNumber(text) && parseFloat(text) === parseInt(text, 10) && !isNaN(text);
	    }
	
	    function redirect(url) {
	        window.location.href = url;
	    }
	
	    function signOut() {
	        var index = window;
	        while (index !== index.parent) {
	            index = index.parent;
	        }
	        index.location = resolveClientUrl('Authorization/SignOut');
	    }
	
	    function setMultiSelectForDropDownList(dropDownListId, dataSource, valueField, textField, checkField) {
	        var data = $.parseJSON(dataSource);
	        var length = data.length;
	        var dataUI = [];
	        var checkedCount = 0;
	        for (var i = 0; i < length; i++) {
	            var item = data[i];
	            var isChecked = asBool(item[checkField]);
	            if (isChecked) {
	                checkedCount++;
	            }
	
	            dataUI.push({
	                Id: item[valueField],
	                Value: item[valueField],
	                Text: getSingleResource(item[textField]),
	                Checked: isChecked,
	                Enable: true
	            });
	        }
	
	        var isCheckAll = checkedCount === length;
	        $("#" + dropDownListId).multiSelect(
	            {
	                minWidth: 200,
	                maxHeight: 400,
	                shadow: true,
	                showHeader: true,
	                isCheckAll: isCheckAll,
	                checkAllText: getSingleResource('All'),
	                showFooter: false,
	                listData: dataUI,
	                onCheck: function () {
	                    var checkedItems = $("#" + dropDownListId).multiSelect("getCheckedItems");
	                    $("#product-container input[type=checkbox]:checked").prop("checked", false);
	                    var hasItemsChecked = checkedItems.length > 0;
	                    if (hasItemsChecked) {
	                        for (var i = 0; i < checkedItems.length; i++) {
	                            $('#product-container #' + checkedItems[i]).prop("checked", true);
	                        }
	                    }
	
	                    $("#SubmitButton").prop("disabled", !hasItemsChecked);
	                    $(".error-img").toggleClass("hide", hasItemsChecked);
	                }
	            });
	    }
	
	    ////////
	
	    return {
	        initialize: initialize,
	        resolveClientUrl: resolveClientUrl,
	        exportExcel: exportExcel,
	        setConstraintMonthRange: setConstraintMonthRange,
	        showMessaegBox: showMessaegBox,
	        getLicenseeId: getLicenseeId,
	        getCurrencyName: getCurrencyName,
	        getHistoryDate: getHistoryDate,
	        isBaseCurrency: isBaseCurrency,
	        addUserGuide: addUserGuide,
	        overrideInnerSize: overrideInnerSize,
	        addFilterCollapseExpand: addFilterCollapseExpand,
	        asNull: asNull,
	        gotoErrorPage: gotoErrorPage,
	        unBlockUI: unBlockUI,
	        blockUI: blockUI,
	        setLicenseeOptionParam: setLicenseeOptionParam,
	        setLicenseeOptionOriginalParam: setLicenseeOptionOriginalParam,
	        updateOriginalLicenseeOption: updateOriginalLicenseeOption,
	        setProductOptionParamList: setProductOptionParamList,
	        setProductOptionOriginalParamList: setProductOptionOriginalParamList,
	        setProductOptionParam: setProductOptionParam,
	        setDateRangeParam: setDateRangeParam,
	        setDateRangeOriginalParam: setDateRangeOriginalParam,
	        currencyConstraint: currencyConstraint,
	        sortReport: sortReport,
	        createSortingIcon: createSortingIcon,
	        detectBrowser: detectBrowser,
	        getMaxHeight: getMaxHeight,
	        getMaxWidth: getMaxWidth,
	        clone: clone,
	        asBool: asBool,
	        getSingleResource: getSingleResource,
	        getResources: getResources,
	        isNumber: isNumber,
	        isInteger: isInteger,
	        redirect: redirect,
	        signOut: signOut,
	        setMultiSelectForDropDownList: setMultiSelectForDropDownList
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },
/* 41 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/// <reference path="/@JSense.js" />
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($) {
	    "use strict";
	    function parseQueryString(queryString) {
	        var args = queryString.split('&');
	        var argsList = {};
	
	        for (var i = 0; i < args.length; i++) {
	            var arg = unescape(args[i]);
	            if (arg.length > 0) {
	                if (arg.indexOf('=') === -1) {
	                    argsList[$.trim(arg.toLowerCase())] = null;
	                }
	                else {
	                    var keyValuePair = arg.split('=');
	                    argsList[$.trim(keyValuePair[0].toLowerCase())] = $.trim(keyValuePair[1]);
	                }
	            }
	        }
	
	        return argsList;
	    }
	
	    function getQueryString(url) {
	        var index = url.indexOf("?");
	        return index < 0 ? "" : url.substring(index + 1, url.length);
	    }
	
	    function getRawUrl(url) {
	        var index = url.indexOf("?");
	        return index < 0 ? url : url.substring(0, index);
	    }
	
	    function setParam(url, name, value) {
	        var rawUrl = getRawUrl(url);
	        var queryString = getQueryString(url);
	        var args = parseQueryString(queryString);
	
	        if (name == null || name === undefined) {
	            return url;
	        }
	
	        // Update value
	        var found = false;
	        for (var key in args) {
	            if (key != null && key !== undefined && key.toLowerCase() === name.toLowerCase()) {
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
	
	    function getParam(url, name) {
	        var queryString = getQueryString(url);
	        var args = parseQueryString(queryString);
	
	        for (var key in args) {
	            if (key != null && key.toLowerCase() === name.toLowerCase()) {
	                return args[key];
	            }
	        }
	
	        return null;
	    }
	
	    return {
	        getQueryString: getQueryString,
	        getRawUrl: getRawUrl,
	        setParam: setParam,
	        getParam: getParam
	    };
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },
/* 42 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/// <reference path="/@JSense.js" />
	(function ($) {
	    var currentTriggerEvents = [];
	    var methods = {
	        createMultiselect: function (opts) {
	            opts = $.extend({}, $.fn.multiSelect.defaults, opts);
	            return this.each(function () {
	                return new MultiSelect(this, opts);
	            });
	        },
	        appendItem: function (item) {
	        },
	        removeItem: function (item) {
	        },
	        updateItems: function (opts) {
	            opts = $.extend({}, $.fn.multiSelect.defaults, opts);
	            return this.each(function () {
	                return new MultiSelect(this, opts);
	            });
	        },
	        checkAllItems: function () {
	            var $inputs = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').find('input');
	            $inputs.not(':disabled').attr('checked', 'checked');
	            var numChecked = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark').find('input').not("#" + $(this).attr('id') + "HeaderCheckBox").filter('[checked]').length;
	            if (o.isUsedCustomCheckAllText) {
	                $(this).find('input').val(o.customCheckAllText);
	            }
	            else {
	                $(this).find('input').val(numChecked + " selected");
	            }
	        },
	        unCheckAllItems: function () {
	            var $inputs = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').find('input');
	            $inputs.not(':disabled').removeAttr('checked');
	            var numChecked = $(this).next('div.ui-multiselect-options').find('label').not('.ui-multiselect-state-disabled').find('input').filter('[checked]').length;
	
	            $(this).find('input').val($.fn.multiSelect.defaults.noneSelectedText);
	        },
	        getUncheckItems: function () {
	            var listChecked = $(this).next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(:checked)').not("#" + $(this).attr('id') + "HeaderCheckBox");
	            var tempArray = [];
	            for (var i = 0; i < listChecked.length; i++) {
	                tempArray.push(listChecked[i].value);
	            }
	            return tempArray;
	        },
	        getCheckedItems: function () {
	            var listChecked = $(this).next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $(this).attr('id') + "HeaderCheckBox)").filter('[checked]');
	            var tempArray = [];
	            for (var i = 0; i < listChecked.length; i++) {
	                tempArray.push(listChecked[i].value);
	            }
	            return tempArray;
	        },
	        getCheckedItemsByText: function () {
	            var listChecked = $(this).next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $(this).attr('id') + "HeaderCheckBox)").filter('[checked]');
	            var tempArray = [];
	            for (var i = 0; i < listChecked.length; i++) {
	                tempArray.push(listChecked[i].title);
	            }
	            return tempArray;
	        },
	        hideMe: function () {
	            $(this).next('div.ui-multiselect-options').trigger('close', true, false);
	        },
	        openMe: function (waiting) {
	            //$(this).find('#ui-multiselect-options-' + $(this).find(":first-child").attr("id")).trigger('open', false);
	            $(this).next('div.ui-multiselect-options').trigger('open', false);
	        }
	    };
	
	    $.fn.multiSelect = function (opts) {
	        if (methods[opts]) {
	            return methods[opts].apply(this, Array.prototype.slice.call(arguments, 1));
	        }
	        else if (typeof opts === 'object' || !opts) {
	            // Default to "createMultiselect"
	            return methods.createMultiselect.apply(this, arguments);
	        }
	        else {
	            //throw error
	        }
	    };
	    //// Detect Browser
	    //// Grid Scroller
	    var matchedBrowser, browserDectect;
	
	    // Use of jQuery.browser is frowned upon.
	    // More details: http://api.jquery.com/jQuery.browser
	    // jQuery.uaMatch maintained for back-compat
	    function ubMatch(ua) {
	        ua = ua.toLowerCase();
	
	        var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
	        /(webkit)[ \/]([\w.]+)/.exec(ua) ||
	        /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
	        /(msie) ([\w.]+)/.exec(ua) ||
	        ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
	        [];
	
	        return {
	            browserDectect: match[1] || "",
	            version: match[2] || "0"
	        };
	    };
	
	    matchedBrowser = ubMatch(navigator.userAgent);
	    browserDectect = {};
	
	    if (matchedBrowser.browserDectect) {
	        browserDectect[matchedBrowser.browserDectect] = true;
	        browserDectect.version = matchedBrowser.version;
	    }
	
	    // Chrome is Webkit, but Webkit is also Safari.
	    if (browserDectect.chrome) {
	        browserDectect.webkit = true;
	    } else if (browserDectect.webkit) {
	        browserDectect.safari = true;
	    }
	
	    // counter to dynamically generate label/option IDs if they don't exist
	    var multiselectID = 0;
	    // built up a multiselect
	    var MultiSelect = function (select, o) {
	        //global variables
	        var $original = $(select);
	        var $select = $original,
	            $options, $header, $labels, $footer, $listGroupCheckBox, $body, $listItem, $listCollapse, $divHover,
	            $allLabel, $inputHeaderCheckBox, $selectinput, $headerCheckbox, $checkedInput, $dropdownlist,
	            html = [],
	            optgroups = [],
	            isDisabled = $select.is(':disabled'),
	            id = select.id || 'ui-multiselect-' + multiselectID++, // unique ID for the label & option tags
	            listData = o.listData,
	            isShowGroup = false,
	            isOpenning = false,
	            numGroup = 0;
	        //render MultiSleclect Interface
	        var RenderInterface = function (id, idDisable, noneSelectedText, title) {
	            //remove old
	            if ($select.prev().is('div.ui-multiselect-options')) {
	                $select.prev().remove();
	            }
	            if ($select.next().is('div.ui-multiselect-options')) {
	                $select.next().remove();
	            }
	
	            //render new
	            var tempHtml = [];
	            tempHtml.push('<a id="' + id + '" class="ui-multiselect ui-multiselect-widget ui-multiselect-state-default ' + (idDisable ? ' ui-multiselect-state-disabled' : '') + '">');
	            tempHtml.push('<input readonly="readonly" type="text" class="ui-multiselect-state-default" value="' + noneSelectedText + '" title="' + select.title + '" /><span class="ui-multiselect-icon ui-multiselect-icon-triangle-1-s"></span></a>');
	            return tempHtml.join('');
	        };
	        //render Header
	        var checkShouldShowGroupOrNo = function () {
	            for (var i = 0; i < o.listData.length; i++) {
	                if (typeof o.listData[i].Items !== 'undefined' && o.listData[i].Items.length > 0) {
	                    isShowGroup = true;
	                    break;
	                }
	            }
	        }
	        var RenderHeader = function (id, checkAllText) {
	            var tempHtml = [];
	            //html.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-corner-all ui-multiselect-header">');
	            //html.push('<ul class="ui-multiselect-helper-reset ui-multiselect-checkboxes" style="overflow-y:hidden;">');
	            tempHtml.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-header custom-ui-widget-header">');
	            tempHtml.push('<ul class="ui-multiselect-helper-reset ui-multiselect-checkboxes-head" style="overflow-y:hidden;">');
	            tempHtml.push('<li style="display:inline;">');
	            tempHtml.push('<label class="ui-multiselect-corner-all" style="color: #000;" for="' + id + "HeaderCheckBox" + '">');
	            tempHtml.push('<input type="checkbox" id="' + id + "HeaderCheckBox" + '">' + checkAllText + '</label></li>');
	            if (isShowGroup) {
	                tempHtml.push('<li class="ui-multiselect-icon-dropdown ui-multiselect-icon-collapse" title="Collapse/Expand" style="display:inline; float:right; cursor:pointer;" id="' + id + "CollapseButton" + '" >&nbsp;</li>');
	            }
	            //tempHtml.push('</li>');
	            tempHtml.push('</ul>');
	            tempHtml.push('</div>');
	            return tempHtml.join('');
	        };
	        var RenderNoItemHeader = function (id, noItemText) {
	            var tempHtml = [];
	            tempHtml.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-corner-all ui-multiselect-header custom-ui-widget-header">');
	            tempHtml.push('<ul class="ui-multiselect-helper-reset ui-multiselect-checkboxes-head" style="overflow-y:hidden;">');
	            tempHtml.push('<li style="display:inline;">');
	            tempHtml.push('<label class="ui-multiselect-corner-all" style="color: #000000" >' + noItemText + '</label>');
	            tempHtml.push('</ul>');
	            tempHtml.push('</div>');
	            return tempHtml.join('');
	        };
	        //render Body
	        var InsertRow = function (tempData, index, isChild, isLastChildItem, isNoGroup, siblingGroup, currentParentGroupId, currentGroupId) {
	            var tempHtml = [];
	            var title = tempData.Text || "",
	            value = tempData.Value || "",
	            inputID = tempData.Id || "ui-multiselect-" + id + "-option-" + index,
	            isDisabled = !tempData.Enable,
	            labelClasses = tempData.CssClass || "",
	            currentGroupStatus = (tempData.Show == false ? false : true),
	            liClasses = "",
	            divClasses = "ui-multiselect-div ui-multiselect-corner-all",
	            divHtml = "";
	            var groupIdAttr = "ui-multiselect-option-group-" + currentGroupId;
	            if (typeof value != 'undefined') {
	                if (isDisabled) {
	                    labelClasses += ' ui-multiselect-state-disabled';
	                }
	                if (typeof tempData.Items !== 'undefined' && (typeof isChild === 'undefined' || isChild === false) && siblingGroup) {
	                    labelClasses += ' ui-multiselect-group-mark';
	                }
	
	                liClasses += (isNoGroup == false ? 'rtLI ' : ' rtLI');
	                liClasses += (isDisabled ? ' ui-multiselect-disabled ' : '');
	                liClasses += (isLastChildItem == true ? " rtLast" : "");
	                liClasses += (index === 0 && isNoGroup == false) ? " rtFirst" : "";
	
	                if (!isShowGroup && o.columnNumber > 0) {
	                    var style = "float: left; width:" + (100 / o.columnNumber).toFixed(0) + "%;";
	                    tempHtml.push('<li class="' + liClasses + '" style="' + style + '">');
	                }
	                else {
	                    tempHtml.push('<li class="' + liClasses + '">');
	                }
	
	                if (siblingGroup == false) {
	                    if (isNoGroup == true && isShowGroup) {
	                        divClasses += (isShowGroup == false ? "" : ((isLastChildItem == true) ? ' rtBot' : ' rtMid'));
	                        tempHtml.push('<div class="' + divClasses + '">');
	                    }
	                    else {
	                        tempHtml.push('<div class="' + divClasses + '" style="margin-left:10px;">');
	                    }
	                }
	                else {
	                    divClasses += (index === 0) ? " rtMid" : " rtMid rtSelected";
	                    divClasses += (isLastChildItem === true ? " rtBot" : "");
	                    tempHtml.push('<div class="' + divClasses + '">');
	                    if (tempData.Items != null && tempData.Items.length !== 0) {
	                        tempHtml.push('<span class="collapseClass rtMinus" showstatus="' + currentGroupStatus + '"></span>');
	                    }
	                }
	
	                labelClasses += " ui-multiselect-corner-all";
	                tempHtml.push('<label for="' + inputID + '" class="' + labelClasses + '" style="' + tempData.CssClass + '">');
	                tempHtml.push('<input id="' + inputID + '" type="' + (o.multiple ? 'checkbox' : 'radio') + '" name="' + value + '" value="' + value + '" title="' + title + '" childgroupid="' + currentParentGroupId + '"');
	
	                if (o.isCheckAll) {
	                    tempHtml.push(' checked="checked"');
	                }
	                else if (tempData.Checked) {
	                    tempHtml.push(' checked="checked"');
	                }
	                if (isDisabled) {
	                    tempHtml.push(' disabled="disabled"');
	                }
	                tempHtml.push(' />' + title + '</label>');
	                tempHtml.push('</div>');
	
	                if (typeof tempData.Items !== 'undefined' && tempData.Items.length > 0) {
	                    //var nextGroupIdAtt = "ui-multiselect-option-group-" + currentGroupId + "-" + index.toString();
	                    tempHtml.push('<ul class="ui-multiselect-ul-group custom-multi-select-rtUL" childgroupid="' + groupIdAttr + '">');
	                    for (var j = 0; j < tempData.Items.length; j++) {
	                        if (typeof tempData.Items[j].Items === 'undefined'
	                        || tempData.Items[j].Items == null
	                        || tempData.Items[j].Items.length === 0) {
	                            tempHtml.push(InsertRow(tempData.Items[j], j,
	                                true, (j === tempData.Items.length - 1 ? true : false), true, siblingGroup, groupIdAttr, currentGroupId));
	                        }
	                        else {
	                            currentGroupId += "-" + j.toString();
	                            tempHtml.push(InsertRow(tempData.Items[j], j,
	                                false, (j === tempData.Items.length - 1 ? true : false), false, siblingGroup, groupIdAttr, currentGroupId));
	                        }
	
	                        //tempHtml.push(InsertRow(tempData.Items[j], j, true, (j === tempData.Items.length - 1 ? true : false), false, false));
	                    }
	                    tempHtml.push("</ul>");
	                }
	
	                tempHtml.push('</li>');
	            }
	            return tempHtml.join('');
	        };
	        var RenderBody = function () {
	            var tempHtml = [];
	            var groupIdAttr = "ui-multiselect-option-group-0";
	            tempHtml.push('<ul class="ui-multiselect-checkboxes ui-multiselect-helper-reset rtUL ' + (isShowGroup ? "rtLines" : "") + ' ui-multiselect-ul-group" childgroupid="' + groupIdAttr + '">');
	            for (var i = 0; i < listData.length; i++) {
	                var tempGroup = listData[i];
	                var isChild = tempGroup.Items == null || tempGroup.Items.length === 0;
	                var isLastChildItem = (i === listData.length - 1 ? true : false);
	                var isNoGroup = (typeof tempGroup.Items === 'undefined' || (typeof tempGroup.Items !== 'undefined' && tempGroup.Items.length === 0)) ? true : false;
	                var siblingIsGroup = isShowGroup ? true : false;
	                var currentGroupId = "0-" + i.toString();
	                numGroup++;
	                tempHtml.push(InsertRow(tempGroup, i, isChild, isLastChildItem, isNoGroup, siblingIsGroup, groupIdAttr, currentGroupId));
	            }
	            tempHtml.push('</ul>');
	            return tempHtml.join('');
	        };
	        //render Footer
	        var RenderFooter = function () {
	            var tempHtml = [];
	            tempHtml.push('<div class="ui-multiselect-widget-header ui-multiselect-helper-clearfix ui-multiselect-corner-all ui-multiselect-footer custom-ui-widget-header">');
	            tempHtml.push('<a class="ui-multiselect-update-result" href="">' + o.updateResult + '</a>');
	            tempHtml.push('</div>');
	            return tempHtml.join('');
	        };
	        //add property value
	        var AddPropertyValue = function () {
	            // calculate widths
	            var iconWidth = $select.find('span.ui-multiselect-icon').width();
	            var inputWidth = o.minWidth - iconWidth + 3;
	
	            // set widths
	            $select.width(o.minWidth).find('input').width(inputWidth);
	        };
	        // binding event
	        var checkCheckAll = function () {
	            if (numGroup == $options.find('input[childgroupid="ui-multiselect-option-group-0"]').filter('[checked]').length) {
	                $headerCheckbox.prop('checked', true);
	                $headerCheckbox.attr('checked', 'checked');
	                $listGroupCheckBox.prop('checked', true);
	                $listGroupCheckBox.attr('checked', 'checked');
	            }
	        };
	        var UpdateSelected = function () {
	            var $inputs = $inputHeaderCheckBox,
	                    $checked = $inputs.filter('[checked]'),
	                    value = '',
	                    numChecked = $checked.length;
	
	            if (numChecked === 0) {
	                value = o.noneSelectedText;
	            } else {
	                if (o.isUsedCustomCheckAllText && $inputs.length === numChecked) {
	                    value = o.customCheckAllText;
	                }
	                else {
	                    if (isShowGroup == false) {
	                        var arrayItemChecked = [];
	                        for (var i = 0; i < numChecked; i++) {
	                            arrayItemChecked.push($checked[i].title);
	                        }
	                        value = arrayItemChecked.join(', ');
	                        if (value.length * 8 > o.minWidth) {
	                            value = o.selectedText.replace('#', numChecked).replace('#', $inputs.length);
	                        }
	                    }
	                    else {
	                        value = o.selectedText.replace('#', numChecked).replace('#', $inputs.length);
	                    }
	                }
	            }
	
	            // Update tooltip
	            if (o.showTooltip) {
	                var tooltipValue = $checked.map(function () { return this.title; }).get().join(', ');
	                if (tooltipValue == '') {
	                    tooltipValue = o.tooltipNoItem;
	                }
	                $select.attr("original-title", tooltipValue);
	            }
	
	            $select.find('input').val(value);
	            return value;
	        };
	
	        var UpdateSelectedFromChild = function (currentGroup) {
	            var parentGroup = currentGroup.parent().parent();
	            if (parentGroup.prop("tagName").toLowerCase() == 'ul' && parentGroup.length > 0) {
	                var currentChildren = parentGroup.find("li > div > label > input");
	                var currentChildSelected = currentChildren.filter('[checked]').length;
	                if (currentChildren.length == currentChildSelected) {
	                    parentGroup.prev().find('input').prop('checked', true);
	                    parentGroup.prev().find('input').attr('checked', 'checked');
	                }
	                UpdateSelectedFromChild(parentGroup);
	            }
	        };
	
	        var UpdateRemoveParentSelected = function (currentGroup) {
	            if (currentGroup.hasClass('ui-multiselect-ul-group') && currentGroup.length > 0) {
	                if (currentGroup.prev().find('input').prop('checked') == true) {//true) {
	                    currentGroup.prev().find('input').prop('checked', false);
	                    currentGroup.prev().find('input').removeAttr('checked');
	                    $headerCheckbox.prop('checked', false);
	                    $headerCheckbox.removeAttr('checked');
	                }
	                UpdateRemoveParentSelected(currentGroup.parent().parent());
	            }
	        };
	
	        var BindingEvent = function () {
	            // build header links
	            if (o.showHeader) {
	                $header.find('input').click(function (e) {
	                    var $this = $(this);
	                    if ($this.prop('checked') == true) {
	                        $this.attr('checked', 'checked');
	                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').prop('checked', true);
	                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').attr('checked', 'checked');
	                    }
	                    else {
	                        $this.removeAttr('checked');
	                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').prop('checked', false);
	                        $options.find('label').not('.ui-multiselect-state-disabled').find('input').removeAttr('checked');
	                    }
	                    UpdateSelected();
	                });
	            }
	            //build footer link
	            if (o.showFooter) {
	                $footer.find('a').click(function (e) {
	                    o.onUpdateResult.call(this);
	                    e.preventDefault();
	                });
	            }
	
	            if (o.showTooltip) {
	                try {
	                    $select.tipsy(
	                        {
	                            html: true,
	                            opacity: 0.9,
	                            gravity: 'sw'
	                        }
	                    );
	                }
	                catch (e) { };
	            }
	            //add handler event collapse
	            $listCollapse.each(function () {
	                var $this = $(this);
	                $this.click(function () {
	                    if ($(this).hasClass('rtMinus')) {
	                        //alert("aaa");
	                        $(this).removeClass('rtMinus').addClass('rtPlus');
	                    }
	                    else {
	                        $(this).removeClass('rtPlus').addClass('rtMinus');
	                    }
	                    $(this).parent().next().toggle();
	                });
	
	                var currentStatus = $this.attr('showstatus');
	                if (currentStatus == 'false') {
	                    $this.removeClass('rtMinus').addClass('rtPlus');
	                    $this.parent().next().hide();
	                    //$this.trigger('click');
	                }
	            });
	            // the select box events
	            $select.bind({
	                click: function () {
	                    $options.trigger('toggle');
	                },
	                keypress: function (e) {
	                    switch (e.keyCode) {
	                        case 27: // esc
	                        case 38: // up
	                            $options.trigger('close');
	                            break;
	                        case 40: // down
	                        case 0: // space
	                            $options.trigger('toggle');
	                            break;
	                    }
	                },
	                mouseenter: function () {
	                    if (!$select.hasClass('ui-multiselect-state-disabled')) {
	                        $(this).addClass('ui-multiselect-state-hover');
	                    };
	                },
	                mouseleave: function () {
	                    $(this).removeClass('ui-multiselect-state-hover');
	                },
	                focus: function () {
	                    if (!$select.hasClass('ui-multiselect-state-disabled')) {
	                        $(this).addClass('ui-multiselect-state-focus');
	                    }
	                },
	                blur: function () {
	                    $(this).removeClass('ui-multiselect-state-focus');
	                }
	            });
	            // bind custom events to the options div
	            $options.bind({
	                'close': function (e, others, isRetrieveData, targetId) {
	                    if ($options.css('display') != 'none') {
	                        others = others || false;
	
	                        // hides all other options but the one clicked
	                        if (others === true) {
	                            $('div.ui-multiselect-options')
	                                .filter(':visible')
	                                .fadeOut(o.fadeSpeed)
	                                .prev('a.ui-multiselect')
	                                .removeClass('ui-multiselect-state-active')
	                                .trigger('mouseout');
	
	                            // hides the clicked options
	                        } else {
	                            $select.removeClass('ui-multiselect-state-active').trigger('mouseout');
	                            $options.fadeOut(o.fadeSpeed);
	                        }
	                        if (o.isRetrieveDataAfterClose) {
	                            isRetrieveData = isRetrieveData || false;
	                            if (isRetrieveData) {
	                                $allLabel.find('input[ischeck="true"]').attr('checked', 'checked');
	                                if (isOpenning == true) {
	                                    $allLabel.find('input[ischeck!="true"]').removeAttr('checked');
	                                }
	                            }
	                            $allLabel.find('input').removeAttr('isCheck');
	                            isOpenning = false;
	                        }
	                        UpdateSelected();
	                        //$.fn.multiSelect.defaults.onLeave();
	                        if (typeof (targetId) != "undefined" && (targetId != $select.attr("id") || targetId == 'html')) {
	                            o.onLeave.call($select);
	                        }
	                    }
	                },
	                'open': function (e, closeOthers) {
	                    // bail if this widget is disabled
	                    if ($select.hasClass('ui-multiselect-state-disabled')) {
	                        return;
	                    }
	
	                    //currentTriggerEvents.push(e.)
	
	                    // use position() if inside ui-widget-content, because offset() won't cut it.
	                    var offset = $select.position(),
	                            $container = $options.find('ul:last'),
	                            top, width;
	
	                    // calling select is active
	                    $select.addClass('ui-multiselect-state-active');
	
	                    // hide all other options
	                    if (closeOthers || typeof closeOthers === 'undefined') {
	                        //$options.trigger('close', [true, true]);
	                        $('div.ui-multiselect-options').trigger('close', [true, true]);
	                    }
	
	                    // calculate positioning
	                    if (o.position === 'middle') {
	                        top = (offset.top + ($select.height() / 2) - ($options.outerHeight() / 2));
	                    } else if (o.position === 'top') {
	                        top = (offset.top - $options.outerHeight());
	                    } else {
	                        top = (offset.top + $select.outerHeight());
	                    }
	                    top += 3; // padding extra 3px to seperate options with multi select div
	
	                    // calculate the width of the options menu
	                    if (browserDectect.msie) {
	                        width = $select.width() - 2 - parseInt($options.css('padding-left'), 10) - parseInt($options.css('padding-right'), 10);
	                    } else {
	                        width = $select.width() + 2 - parseInt($options.css('padding-left'), 10) - parseInt($options.css('padding-right'), 10);
	                    }
	
	                    // select the first option
	                    //$labels.filter('label:first').trigger('mouseenter').trigger('focus');
	
	                    // show the options div + position it
	                    $options.css({
	                        position: 'absolute',
	                        top: top + 'px',
	                        left: offset.left + 'px',
	                        width: width + 'px'
	                    }).show();
	
	                    // set the scroll of the checkbox container
	                    $container.scrollTop(0);
	
	                    // set the height of the checkbox container
	
	                    if (o.maxHeight) {
	                        if (o.maxHeight <= $body.height()) {
	                            $body.css('max-height', o.maxHeight);
	                        }
	                    }
	                    if (o.isRetrieveDataAfterClose) {
	                        $allLabel.find('input').filter('[checked]').attr("ischeck", "true");
	                        isOpenning = true;
	                    }
	                    //o.onOpen.call($options[0]);
	                },
	                'toggle': function () {
	                    var isHide = $(this).is(':hidden');
	                    if (isHide) {
	                        $options.trigger('open');
	                    }
	                    else {
	                        $options.trigger('close', [true, true]);
	                    }
	                }
	            });
	            // checkbox click event handle
	            $listItem.each(function () {
	                var $this = $(this);
	                //handle group checkbox click event
	                $this.prev().children().not('span.collapseClass').find('input').click(function () {
	                    if ($(this).prop('checked') == true) {
	                        $(this).attr('checked', 'checked');
	                        $(this).parent().parent().next().find('input:not(:disabled)').prop('checked', true);
	                        UpdateSelectedFromChild($this);
	                        checkCheckAll();
	                    }
	                    else {
	                        $(this).parent().parent().next().find('input:not(:disabled)').removeAttr('checked');
	                        $(this).parent().parent().next().find('input:not(:disabled)').prop('checked', false);
	                        $headerCheckbox.removeAttr('checked');
	                        $headerCheckbox.prop('checked', false);
	                    }
	                    o.onCheck.call(this);
	                    UpdateSelected();
	                });
	
	                //handler chilren checkbox event
	                var childrenNodes = $this.find('input[childgroupid="' + $this.attr("childgroupid") + '"]');
	                childrenNodes.click(function () {
	                    var parent = $this;
	                    var current = $(this);
	                    if (current.prop('checked') == false) {
	                        current.removeAttr('checked');
	                        UpdateRemoveParentSelected(parent);
	                    }
	                    else if (current.prop('checked') == true) {
	                        current.attr('checked', 'checked');
	                        var currentGroup = parent;
	                        var currentChildSelected = childrenNodes.filter('[checked]').length;
	                        var currentChildLength = childrenNodes.length;
	                        if (currentChildLength == currentChildSelected) {
	                            currentGroup.prev().find('input').prop('checked', true);
	                            currentGroup.prev().find('input').attr('checked', 'checked');
	                            UpdateSelectedFromChild(current.parent().parent().parent().parent());
	                        }
	
	                        //                        if (currentGroup.find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark').find('input').not("#" + currentGroup.attr('id') + "HeaderCheckBox").filter('[checked]').length
	                        //                                === currentGroup.find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark').find('input').not("#" + currentGroup.attr('id') + "HeaderCheckBox").length) {
	                        //                            currentGroup.prev().find('input').attr('checked', 'checked');
	                        //                        }
	                        //                        if (parent.find('input:not(:disabled)').length === (parent.find('input[checked]').length)) {
	                        //                            parent.prev().find('input').attr('checked', 'checked');
	                        //                        }
	
	                        checkCheckAll();
	                    }
	                    o.onCheck.call(this);
	                    UpdateSelected();
	                });
	            });
	            //handle collapse button
	            $('#' + id + "CollapseButton").click(function () {
	                if ($(this).hasClass("ui-multiselect-icon-collapse")) {
	                    $(this).removeClass("ui-multiselect-icon-collapse").addClass("ui-multiselect-icon-expand");
	                    $options.find('ul.ui-multiselect-ul-group:not(.ui-multiselect-checkboxes):visible').hide();
	                    $options.find('span.collapseClass').removeClass('rtMinus').addClass('rtPlus');;
	                    //$(this).text('Expande All');
	                }
	                else {
	                    $options.find('ul.ui-multiselect-ul-group:not(.ui-multiselect-checkboxes):hidden').show();
	                    $options.find('span.collapseClass').removeClass('rtPlus').addClass('rtMinus');
	                    $(this).removeClass("ui-multiselect-icon-expand").addClass("ui-multiselect-icon-collapse");
	                }
	            });
	        };
	        //initial setup
	        var InitialSetup = function () {
	            if (o.isCheckAll === true) {
	                $headerCheckbox.attr('checked', 'checked');
	            }
	            // open by default?
	            if (o.state === 'open') {
	                $options.trigger('open', [false]);
	            }
	
	            // update the number of selected elements when the page initially loads, and use that as the defaultValue.  necessary for form resets when options are pre-selected.
	            $select.find('input')[0].defaultValue = UpdateSelected();
	            //$options.trigger('closed', [true]);
	            //            $('div.ui-multiselect-options').trigger('close', [true]);
	            $options.hide();
	        };
	
	        //Render All
	        (function () {
	            checkShouldShowGroupOrNo();
	            html.push(RenderInterface(id, isDisabled || o.disabled, o.noneSelectedText));
	            html.push('<div class="ui-multiselect-options RadTreeView RadTreeView_Office2010Silver' + (o.shadow ? ' ui-multiselect-shadow' : '') + ' ui-multiselect-widget ui-multiselect-widget-content ui-multiselect-corner-all" >');
	            if (o.listData.length != 0) {
	                html.push(RenderHeader(id, o.checkAllText));
	                html.push(RenderBody());
	            }
	            else {
	                html.push(RenderNoItemHeader(id, o.noItemText));
	            }
	            if (o.showFooter == true) {
	                html.push(RenderFooter());
	            }
	            html.push('</div>');
	
	            //add global variable
	
	            $select = $select.after(html.join('')).next('a.ui-multiselect');
	            $options = $select.next('div.ui-multiselect-options');
	            $options.attr('id', 'ui-multiselect-options-' + id);
	            $options = $('#ui-multiselect-options-' + id);
	            $header = $options.find('div.ui-multiselect-header');
	            $body = $options.find('ul.ui-multiselect-checkboxes');
	            $footer = $options.find('div.ui-multiselect-footer');
	            $labels = $options.find('label').not('.ui-multiselect-state-disabled').not('.ui-multiselect-group-mark');
	            $allLabel = $options.find('label');
	            $listItem = $options.find('ul.ui-multiselect-ul-group');
	            $listCollapse = $options.find('span.collapseClass');
	            $listGroupCheckBox = $options.find('label.ui-multiselect-group-mark').find('input');
	            $divHover = $options.find('div.ui-multiselect-div');
	
	            AddPropertyValue();
	            BindingEvent();
	
	            $inputHeaderCheckBox = $labels.find('input').not("#" + id + "HeaderCheckBox");
	            $selectinput = $select.find('input');
	            $headerCheckbox = $options.find("#" + id + "HeaderCheckBox");
	            $checkedInput = $labels.find('input[checked]');
	
	            InitialSetup();
	            // remove the original input element
	
	            $original.remove();
	        })();
	        return $select;
	    };
	    // close each select when clicking on any other element/anywhere else on the page
	    $(document).delegate('html', 'click', function (e, isInsideCall) {
	        var $target = $(e.target);
	        if (!$target.closest('div.ui-multiselect-options').length && !$target.parent().hasClass('ui-multiselect')) {
	            if (!$target.hasClass('exeption-multiselect-click')) {
	                $('div.ui-multiselect-options').trigger('close', [true, true, ($target.attr("id") || 'html')]);
	            }
	        }
	    });
	    // default options
	    $.fn.multiSelect.defaults = {
	        showHeader: true,
	        showFooter: true,
	        showTooltip: false,
	        maxHeight: 175, /* max height of the checkbox container (scroll) in pixels */
	        minWidth: 215, /* min width of the entire widget in pixels. setting to 'auto' will disable */
	        checkAllText: 'Check All',
	        noItemText: 'No league found.',
	        unCheckAllText: 'Uncheck All',
	        updateResult: 'Update Result Table',
	        noneSelectedText: 'Select options',
	        selectedText: '# selected',
	        tooltipNoItem: 'There is no selected item.',
	        selectedList: 0,
	        position: 'bottom', /* top|middle|bottom */
	        shadow: false,
	        fadeSpeed: 100,
	        disabled: false,
	        state: 'closed',
	        multiple: true,
	        isCheckAll: false,
	        hasOptGroup: false,
	        isRetrieveDataAfterClose: false,
	        listData: [],
	        buttons: [],
	        isUsedCustomCheckAllText: false,
	        customCheckAllText: '',
	        columnNumber: 1,
	        onLeave: function () { },
	        onCheck: function () { }, /* when an individual checkbox is clicked */
	        onOpen: function () { }, /* when the select menu is opened */
	        onCheckAll: function () { }, /* when the check all link is clicked */
	        onUncheckAll: function () { }, /* when the uncheck all link is clicked */
	        onOptgroupToggle: function () { }, /* when the optgroup heading is clicked */
	        onUpdateResult: function () { }
	    };
	})(jQuery);
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 43 */
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
/* 44 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/*
	* jQuery jclock - Clock plugin - v 2.4.3
	* http://plugins.jquery.com/project/jclock
	*
	* Copyright (c) 2007-2014 Doug Sparling <http://www.dougsparling.com>
	* Licensed under the MIT License:
	* http://www.opensource.org/licenses/mit-license.php
	*/
	(function ($) {
	
	    $.fn.jclock = function (options) {
	        var version = '2.4.3';
	
	        // options
	        var opts = $.extend({}, $.fn.jclock.defaults, options);
	
	        return this.each(function () {
	            var $this = $(this);
	            $this.timerID = null;
	            $this.running = false;
	
	            // Record keeping for seeded clock
	            $this.increment = 0;
	            $this.lastCalled = new Date().getTime();
	
	            var o = $.meta ? $.extend({}, opts, $this.data()) : opts;
	
	            $this.format = o.format;
	            $this.utc = o.utc;
	            // deprecate utc_offset (v 2.2.0)
	            $this.utcOffset = (o.utc_offset != null) ? o.utc_offset : o.utcOffset;
	            $this.seedTime = o.seedTime;
	            $this.timeout = o.timeout;
	
	            $this.css({
	                fontFamily: o.fontFamily,
	                fontSize: o.fontSize,
	                backgroundColor: o.background,
	                color: o.foreground
	            });
	
	            // %a
	            $this.daysAbbrvNames = new Array(7);
	            $this.daysAbbrvNames[0] = "Sun";
	            $this.daysAbbrvNames[1] = "Mon";
	            $this.daysAbbrvNames[2] = "Tue";
	            $this.daysAbbrvNames[3] = "Wed";
	            $this.daysAbbrvNames[4] = "Thu";
	            $this.daysAbbrvNames[5] = "Fri";
	            $this.daysAbbrvNames[6] = "Sat";
	
	            // %A
	            $this.daysFullNames = new Array(7);
	            $this.daysFullNames[0] = "Sunday";
	            $this.daysFullNames[1] = "Monday";
	            $this.daysFullNames[2] = "Tuesday";
	            $this.daysFullNames[3] = "Wednesday";
	            $this.daysFullNames[4] = "Thursday";
	            $this.daysFullNames[5] = "Friday";
	            $this.daysFullNames[6] = "Saturday";
	
	            // %b
	            $this.monthsAbbrvNames = new Array(12);
	            $this.monthsAbbrvNames[0] = "Jan";
	            $this.monthsAbbrvNames[1] = "Feb";
	            $this.monthsAbbrvNames[2] = "Mar";
	            $this.monthsAbbrvNames[3] = "Apr";
	            $this.monthsAbbrvNames[4] = "May";
	            $this.monthsAbbrvNames[5] = "Jun";
	            $this.monthsAbbrvNames[6] = "Jul";
	            $this.monthsAbbrvNames[7] = "Aug";
	            $this.monthsAbbrvNames[8] = "Sep";
	            $this.monthsAbbrvNames[9] = "Oct";
	            $this.monthsAbbrvNames[10] = "Nov";
	            $this.monthsAbbrvNames[11] = "Dec";
	
	            // %B
	            $this.monthsFullNames = new Array(12);
	            $this.monthsFullNames[0] = "January";
	            $this.monthsFullNames[1] = "February";
	            $this.monthsFullNames[2] = "March";
	            $this.monthsFullNames[3] = "April";
	            $this.monthsFullNames[4] = "May";
	            $this.monthsFullNames[5] = "June";
	            $this.monthsFullNames[6] = "July";
	            $this.monthsFullNames[7] = "August";
	            $this.monthsFullNames[8] = "September";
	            $this.monthsFullNames[9] = "October";
	            $this.monthsFullNames[10] = "November";
	            $this.monthsFullNames[11] = "December";
	
	            $.fn.jclock.startClock($this);
	
	        });
	    };
	
	    $.fn.jclock.startClock = function (el) {
	        $.fn.jclock.stopClock(el);
	        $.fn.jclock.displayTime(el);
	    };
	
	    $.fn.jclock.stopClock = function (el) {
	        if (el.running)
	            clearTimeout(el.timerID);
	        el.running = false;
	    };
	
	    /* if the frequency is "once every minute" then we have to make sure this happens
	     * when the minute changes. */
	    // got this idea from digiclock http://www.radoslavdimov.com/jquery-plugins/jquery-plugin-digiclock/
	    function getDelay(timeout) {
	        if (timeout == 60000) {
	            var now = new Date();
	            timeout = 60000 - now.getSeconds() * 1000; // number of seconds before the next minute
	        }
	        return timeout;
	    }
	
	    $.fn.jclock.displayTime = function (el) {
	        var time = $.fn.jclock.currentTime(el);
	        var formatted_time = $.fn.jclock.formatTime(time, el);
	        el.attr('currentTime', time.getTime());
	        el.html(formatted_time);
	        el.timerID = setTimeout(function () { $.fn.jclock.displayTime(el) }, getDelay(el.timeout));
	    };
	
	    $.fn.jclock.currentTime = function (el) {
	        if (typeof (el.seedTime) == 'undefined') {
	            // Seed time not being used, use current time
	            var now = new Date();
	        } else {
	            // Otherwise, use seed time with increment
	            el.increment += new Date().getTime() - el.lastCalled;
	            var now = new Date(el.seedTime + el.increment);
	            el.lastCalled = new Date().getTime();
	        }
	
	        if (el.utc == true) {
	            var localTime = now.getTime();
	            var localOffset = now.getTimezoneOffset() * 60000;
	            var utc = localTime + localOffset;
	            var utcTime = utc + (3600000 * el.utcOffset);
	            var now = new Date(utcTime);
	        }
	
	        return now;
	    };
	
	    $.fn.jclock.formatTime = function (time, el) {
	
	        var timeNow = "";
	        var i = 0;
	        var index = 0;
	        while ((index = el.format.indexOf("%", i)) != -1) {
	            timeNow += el.format.substring(i, index);
	            index++;
	
	            // modifier flag
	            //switch (el.format.charAt(index++)) {
	            //}
	
	            var property = $.fn.jclock.getProperty(time, el, el.format.charAt(index));
	            index++;
	
	            //switch (switchCase) {
	            //}
	
	            timeNow += property;
	            i = index;
	        }
	
	        timeNow += el.format.substring(i);
	        return timeNow;
	    };
	
	    $.fn.jclock.getProperty = function (dateObject, el, property) {
	
	        switch (property) {
	            case "a": // abbrv day names
	                return (el.daysAbbrvNames[dateObject.getDay()]);
	            case "A": // full day names
	                return (el.daysFullNames[dateObject.getDay()]);
	            case "b": // abbrv month names
	                return (el.monthsAbbrvNames[dateObject.getMonth()]);
	            case "B": // full month names
	                return (el.monthsFullNames[dateObject.getMonth()]);
	            case "d": // day 01-31
	                return ((dateObject.getDate() < 10) ? "0" : "") + dateObject.getDate();
	            case "H": // hour as a decimal number using a 24-hour clock (range 00 to 23)
	                return ((dateObject.getHours() < 10) ? "0" : "") + dateObject.getHours();
	            case "I": // hour as a decimal number using a 12-hour clock (range 01 to 12)
	                var hours = (dateObject.getHours() % 12 || 12);
	                return ((hours < 10) ? "0" : "") + hours;
	            case "l": // hour as a decimal number using a 12-hour clock (range 1 to 12)
	                var hours = (dateObject.getHours() % 12 || 12);
	                //return ((hours < 10) ? "0" : "") + hours;
	                return hours;
	            case "m": // month number
	                return (((dateObject.getMonth() + 1) < 10) ? "0" : "") + (dateObject.getMonth() + 1);
	            case "M": // minute as a decimal number
	                return ((dateObject.getMinutes() < 10) ? "0" : "") + dateObject.getMinutes();
	            case "p": // either `am' or `pm' according to the given time value,
	                // or the corresponding strings for the current locale
	                return (dateObject.getHours() < 12 ? "am" : "pm");
	            case "P": // either `AM' or `PM' according to the given time value,
	                return (dateObject.getHours() < 12 ? "AM" : "PM");
	            case "S": // second as a decimal number
	                return ((dateObject.getSeconds() < 10) ? "0" : "") + dateObject.getSeconds();
	            case "y": // two-digit year
	                return dateObject.getFullYear().toString().substring(2);
	            case "Y": // full year
	                return (dateObject.getFullYear());
	            case "%":
	                return "%";
	        }
	
	    };
	
	    // plugin defaults (24-hour)
	    $.fn.jclock.defaults = {
	        format: '%H:%M:%S',
	        utcOffset: 0,
	        utc: false,
	        fontFamily: '',
	        fontSize: '',
	        foreground: '',
	        background: '',
	        seedTime: undefined,
	        timeout: 1000 // 1000 = one second, 60000 = one minute
	    };
	
	})(jQuery);
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 45 */
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


/***/ }
]);
//# sourceMappingURL=home.chunk.js.map