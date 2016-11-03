webpackJsonp([8],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(58);
	__webpack_require__(59);
	__webpack_require__(9);
	module.exports = __webpack_require__(60);


/***/ },

/***/ 58:
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

/***/ 59:
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

/***/ 60:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(61)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Home) {
	    $(Home.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 61:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(5), __webpack_require__(62), __webpack_require__(63), __webpack_require__(64)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site) {
	    var $searchBox;
	    var _autocomplete;
	
	    
	    function initialize() {
	        Site.initialize();    
	        //initClock();
	        initVariables();
	        initEvent();
	        activeDefaultMenuItem();
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
	
	        $('#label-search').click(function () {
	            $(this).parent().addClass('focused');
	            $("#searchbox").focus();
	        });
	
	        $("#searchbox").blur(function () {
	            if (!$(this).val()) {
	                $(this).parent().removeClass('focused');
	            }
	        });
	
	        $('body').click(function () {
	            _autocomplete.$dropdown.html('').hide();
	        });
	
	        $('.collapsible2').on("click.menuevent", ".collapsible-body  a", function (e) {
	            document.title = 'LRF' + ' - ' + $(".active > .menu-title").text() + ' - ' + $(this).text();
	            $("#page-title").text($(this).text());
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

/***/ 62:
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

/***/ 63:
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

/***/ 64:
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

});
//# sourceMappingURL=home.chunk.js.map