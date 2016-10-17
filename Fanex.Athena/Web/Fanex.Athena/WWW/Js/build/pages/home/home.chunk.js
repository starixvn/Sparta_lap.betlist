webpackJsonp([7],[
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(32);
	__webpack_require__(33);
	module.exports = __webpack_require__(34);


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
/* 32 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {(function ($) {
	
	    var methods = {
	        init: function (options) {
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
	
	                if (options.edge == 'left'){
	                    menu_id.css('transform', 'translateX(-100%)');
	                    dragTarget.css({ 'left': 0 }); // Add Touch Area
	                }
	                else {
	                    menu_id.addClass('right-aligned') // Change text-alignment to right
	                      .css('transform', 'translateX(100%)');
	                    dragTarget.css({ 'right': 0 }); // Add Touch Area
	                }
	
	                // If fixed sidenav, bring menu out
	                if (menu_id.hasClass('fixed')) {
	                    if (window.innerWidth > 992) {
	                        menu_id.css('transform', 'translateX(0)');
	                    }
	                    else {
	                        $('body').removeClass('site-menubar-fold')
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
	                            }
	                            else {
	                                // menu_id.removeAttr('style');
	                                menu_id.css('transform', 'translateX(0%)');
	                                // menu_id.css('width', options.menuWidth);
	                            }
	
	
	                        }
	                        else if (menuOut === false) {
	
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
	                        complete: function () {
	                            $(this).remove();
	                        }
	                    });
	                    if (options.edge === 'left') {
	                        // Reset phantom div
	                        dragTarget.css({ width: '', right: '', left: '0' });
	                        menu_id.velocity(
	                          { 'translateX': '-100%' },
	                          {
	                              duration: 200,
	                              queue: false,
	                              easing: 'easeOutCubic',
	                              complete: function () {
	                                  if (restoreNav === true) {
	                                      // Restore Fixed sidenav
	                                      menu_id.removeAttr('style');
	                                      menu_id.css('width', options.menuWidth);
	                                  }
	                              }
	
	                          });
	                    }
	                    else {
	                        // Reset phantom div
	                        dragTarget.css({ width: '', right: '0', left: '' });
	                        menu_id.velocity(
	                          { 'translateX': '100%' },
	                          {
	                              duration: 200,
	                              queue: false,
	                              easing: 'easeOutCubic',
	                              complete: function () {
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
	                            if (x > options.menuWidth) { x = options.menuWidth; }
	                            else if (x < 0) { x = 0; }
	                        }
	
	                        if (options.edge === 'left') {
	                            // Left Direction
	                            if (x < (options.menuWidth / 2)) { menuOut = false; }
	                                // Right Direction
	                            else if (x >= (options.menuWidth / 2)) { menuOut = true; }
	                            menu_id.css('transform', 'translateX(' + (x - options.menuWidth) + 'px)');
	                        }
	                        else {
	                            // Left Direction
	                            if (x < (window.innerWidth - options.menuWidth / 2)) {
	                                menuOut = true;
	                            }
	                                // Right Direction
	                            else if (x >= (window.innerWidth - options.menuWidth / 2)) {
	                                menuOut = false;
	                            }
	                            var rightPos = (x - options.menuWidth / 2);
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
	                        }
	                        else {
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
	                            if ((menuOut && velocityX <= 0.3) || velocityX < -0.5) {
	                                // Return menu to open
	                                if (leftPos !== 0) {
	                                    menu_id.velocity({ 'translateX': [0, leftPos] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                                }
	
	                                $('#sidenav-overlay').velocity({ opacity: 1 }, { duration: 50, queue: false, easing: 'easeOutQuad' });
	                                dragTarget.css({ width: '50%', right: 0, left: '' });
	                                menuOut = true;
	                            }
	                            else if (!menuOut || velocityX > 0.3) {
	                                // Enable Scrolling
	                                $('body').css({
	                                    overflow: '',
	                                    width: ''
	                                });
	                                // Slide menu closed
	                                menu_id.velocity({ 'translateX': [-1 * options.menuWidth - 10, leftPos] }, { duration: 200, queue: false, easing: 'easeOutQuad' });
	                                $('#sidenav-overlay').velocity({ opacity: 0 }, {
	                                    duration: 200, queue: false, easing: 'easeOutQuad',
	                                    complete: function () {
	                                        $(this).remove();
	                                    }
	                                });
	                                dragTarget.css({ width: '10px', right: '', left: 0 });
	                            }
	                        }
	                        else {
	                            if ((menuOut && velocityX >= -0.3) || velocityX > 0.5) {
	                                // Return menu to open
	                                if (rightPos !== 0) {
	                                    menu_id.velocity({ 'translateX': [0, rightPos] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                                }
	
	                                $('#sidenav-overlay').velocity({ opacity: 1 }, { duration: 50, queue: false, easing: 'easeOutQuad' });
	                                dragTarget.css({ width: '50%', right: '', left: 0 });
	                                menuOut = true;
	                            }
	                            else if (!menuOut || velocityX < -0.3) {
	                                // Enable Scrolling
	                                $('body').css({
	                                    overflow: '',
	                                    width: ''
	                                });
	
	                                // Slide menu closed
	                                menu_id.velocity({ 'translateX': [options.menuWidth + 10, rightPos] }, { duration: 200, queue: false, easing: 'easeOutQuad' });
	                                $('#sidenav-overlay').velocity({ opacity: 0 }, {
	                                    duration: 200, queue: false, easing: 'easeOutQuad',
	                                    complete: function () {
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
	                        menu_id.removeAttr('style')
	                        $('body').toggleClass('site-menubar-fold');
	
	                    } else {
	
	
	                        if (menuOut === true) {
	                            menuOut = false;
	                            panning = false;
	                            removeMenu();
	                        }
	                        else {
	
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
	                            }
	                            else {
	                                dragTarget.css({ width: '50%', right: '', left: 0 });
	                                menu_id.velocity({ 'translateX': [0, options.menuWidth] }, { duration: 300, queue: false, easing: 'easeOutQuad' });
	                            }
	
	                            var overlay = $('<div id="sidenav-overlay"></div>');
	                            overlay.css('opacity', 0)
	                            .click(function () {
	                                menuOut = false;
	                                panning = false;
	                                removeMenu();
	                                overlay.velocity({ opacity: 0 }, {
	                                    duration: 300, queue: false, easing: 'easeOutQuad',
	                                    complete: function () {
	                                        $(this).remove();
	                                    }
	                                });
	
	                            });
	                            $('body').append(overlay);
	                            overlay.velocity({ opacity: 1 }, {
	                                duration: 300, queue: false, easing: 'easeOutQuad',
	                                complete: function () {
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
	        show: function () {
	            this.trigger('click');
	        },
	        hide: function () {
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
	}(jQuery));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 33 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {(function ($) {
	    function activeMenuItem($target) {
	        $(this).find('.collapsible-body li.active').removeClass('active');
	        $target.closest('li').addClass('active');
	        var $groupHeader = $target.closest('.menu-item').find('.collapsible-header');
	        if ($groupHeader.hasClass('active') === false) {
	            $groupHeader.trigger("click");
	        }
	    }
	
	    function init(options) {
	        var defaults = {
	            accordion: undefined,
	        };
	
	        options = $.extend(defaults, options);
	        var touch = 'ontouchstart' in window || 'onmsgesturechange' in window;
	
	        return this.each(function () {
	
	            var $this = $(this);
	
	            if (!$this.parent().hasClass('site-menu'))
	                return;
	
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
	                }
	                else {
	                    object.parent().removeClass('active');
	                }
	                if (object.parent().hasClass('active')) {
	                    object.siblings('.collapsible-body').stop(true, false).slideDown({ duration: 350, easing: "easeOutQuart", queue: false, complete: function () { $(this).css('height', ''); } });
	                }
	                else {
	                    object.siblings('.collapsible-body').stop(true, false).slideUp({ duration: 350, easing: "easeOutQuart", queue: false, complete: function () { $(this).css('height', ''); } });
	                }
	
	                $panel_headers.not(object).removeClass('active').parent().removeClass('active');
	                $panel_headers.not(object).parent().children('.collapsible-body').stop(true, false).slideUp(
	                  {
	                      duration: 350,
	                      easing: "easeOutQuart",
	                      queue: false,
	                      complete:
	                        function () {
	                            $(this).css('height', '');
	                        }
	                  });
	            }
	
	            // Expandable Open
	            function expandableOpen(object) {
	                if (object.hasClass('active')) {
	                    object.parent().addClass('active');
	                }
	                else {
	                    object.parent().removeClass('active');
	                }
	                if (object.parent().hasClass('active')) {
	                    object.siblings('.collapsible-body').stop(true, false).slideDown({ duration: 350, easing: "easeOutQuart", queue: false, complete: function () { $(this).css('height', ''); } });
	                }
	                else {
	                    object.siblings('.collapsible-body').stop(true, false).slideUp({ duration: 350, easing: "easeOutQuart", queue: false, complete: function () { $(this).css('height', ''); } });
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
	                    overlay.css('opacity', 0)
	                    .click(function () {
	                        collapse($this, event);
	                        overlay.velocity({ opacity: 0 }, {
	                            duration: 300, queue: false, easing: 'easeOutQuad',
	                            complete: function () {
	                                $(this).remove();
	                            }
	                        });
	
	                    });
	                    $('body').append(overlay);
	                    overlay.velocity({ opacity: 1 }, {
	                        duration: 300, queue: false, easing: 'easeOutQuad',
	                        complete: function () {
	
	                        }
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
	
	                if (options.accordion || collapsible_type === "accordion" || collapsible_type === undefined) { // Handle Accordion
	                    accordionOpen(element);
	                } else { // Handle Expandables
	                    expandableOpen(element);
	
	                    if ($header.hasClass('active')) {
	                        expandableOpen($header);
	                    }
	                }
	            };
	
	            if (!touch) {
	                $this.on('mouseover.opensub', '> li > .collapsible-header', function (e) {
	                    if ($('body').hasClass('site-menubar-fold'))
	                        collapse(this, e);
	                });
	            }
	
	            // Add click handler to only direct collapsible header children
	            $this.on('click.collapse2', '> li > .collapsible-header', function (e) {
	                return touch && $('body').hasClass('site-menubar-fold') ? collapseWithOverlay(this, e)
	                    : collapse(this, e);
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
	
	                        if (options.accordion || collapsible_type === "accordion" || collapsible_type === undefined) { // Handle Accordion
	                            accordionOpen(element);
	                        } else { // Handle Expandables
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
	                var $element = $(e.target);
	                if (touch && $('body').hasClass('site-menubar-fold')) {
	                    var temp = $element.closest('li > .collapsible-body');
	
	                    temp.parent().find('.collapsible-header').trigger("click");
	                }
	
	                activeMenuItem.call($this, $element);
	            });
	
	            // Open first active
	            var $panel_headers = $this.find('> li > .collapsible-header');
	            if (options.accordion || collapsible_type === "accordion" || collapsible_type === undefined) { // Handle Accordion
	                accordionOpen($panel_headers.filter('.active').first());
	            }
	            else { // Handle Expandables
	                $panel_headers.filter('.active').each(function () {
	                    expandableOpen($(this));
	                });
	            }
	        });
	    }
	
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
	    };
	
	}(jQuery));
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 34 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(35)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Home) {
	    $(Home.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },
/* 35 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(36), __webpack_require__(39), __webpack_require__(40), __webpack_require__(41)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site) {
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
	        $('.collapsible2').collapsibleMenu();
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
/* 36 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/*global _language_en_US*/
	/// <reference path="/@JSense.js" />
	/* Preset */
	// root JSON object
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(37), __webpack_require__(2), __webpack_require__(38)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, QueryString) {
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
	        var headerFilter = $("div.content-filter").outerHeight();
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
/* 37 */
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
/* 38 */
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
/* 39 */
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
/* 40 */
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
/* 41 */
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