webpackJsonp([7],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(32);
	__webpack_require__(33);
	module.exports = __webpack_require__(34);


/***/ },

/***/ 32:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery, $) {(function ($) {
	
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
	
	    $("#CustomerSearchForm").submit(function (e) {
	        e.preventDefault();
	
	        var userName = $('#searchbox').val();
	        userName = userName || "";
	
	        var url = $("#RootUrl").val() + "Customer/CustomerList?userName=" + userName;
	
	        top.main.location.href = url;
	
	        $('.collapsible2').collapsibleMenu('activeMenuItem', $('#MenuItem43'));
	    });
	});
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1), __webpack_require__(1)))

/***/ },

/***/ 33:
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
	
	    $(document).ready(function () {
	        $('.collapsible2').collapsibleMenu();
	    });
	}(jQuery));
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 34:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {"use strict";
	
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(35), __webpack_require__(37), __webpack_require__(38), __webpack_require__(39)], __WEBPACK_AMD_DEFINE_RESULT__ = function (Site) {
	    var rootUrl;
	    var $searchBox;
	
	    $(initialize);
	
	    function initialize() {
	        //initClock();
	        initVariables();
	        initEvent();
	    }
	
	    function initVariables() {
	        $searchBox = $('#searchbox');
	        rootUrl = $('#RootUrl').val();
	    }
	
	    function initEvent() {
	        initAutoComplete();
	
	        $searchBox.keyup(function (e) {
	            e = e || window.event;
	            var key = e.keyCode || e.which;
	
	            if (key === 13) {
	                searchByUserName();
	                return false;
	            }
	        });
	
	        $('#languagesite').find('.languageItem').click(function () {
	            changeLanguage($(this));
	        });
	
	        $('#ddlLicensee').find('.licenseeItem').click(function () {
	            changeLicensee($(this));
	        }); 
	    }
	
	    function searchByUserName() {
	        var userName = $searchBox.val();
	        if (!userName) {
	            return;
	        }
	
	        var url = rootUrl + "Customer/CustomerList?userName=" + userName;
	        top.main.location.href = url;
	    }
	
	    function initAutoComplete() {
	        var autocomplete = $searchBox.materialize_autocomplete({
	            ignoreCase: false,
	            multiple: {
	                enable: false
	            },
	            dropdown: {
	                el: '#suggestionDropdown'
	            },
	            getData: function (value, callback) {
	                var url = rootUrl + "CustomerAPI/SearchCustomer?licenseeType=&currencyId=0&query=" + value;
	
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
	
	        autocomplete.select = function (item) {
	            $searchBox.val(item.id);
	            $searchBox.focus();
	            this.$dropdown.hide();
	        }
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
	
	    function changeLicensee($target) {
	        var licenseeId = $target.attr('licenseeId');
	        top.frames["main"].location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
	    }
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 35:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {/*global _language_en_US*/
	/// <reference path="/@JSense.js" />
	/* Preset */
	// root JSON object
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(36)], __WEBPACK_AMD_DEFINE_RESULT__ = function (QueryString) {
	    "use strict";
	    var UNBLOCKUI_TIMEOUT = 30000;
	    var BASE_CURRENT_LIST = ["EUR", "USD", "GBP"];
	    var REQUEST_VERIFICATION_TOKEN;
	    var _historyDate = null;
	    var _licenseeId = null;
	    var _isLoadedNotificationPopup = false;
	    var _rootUrl = null;
	    var _pageLanguage = null;
	
	    $(initialize);
	
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
	        $baseCurrency.material_select();
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
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 36:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {/// <reference path="/@JSense.js" />
	!(__WEBPACK_AMD_DEFINE_RESULT__ = function () {
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
	}.call(exports, __webpack_require__, exports, module), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 37:
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

/***/ 38:
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

/***/ 39:
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
//# sourceMappingURL=layout.chunk.js.map