webpackJsonp([7],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(25);
	module.exports = __webpack_require__(26);


/***/ },

/***/ 25:
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
	    });
	});
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1), __webpack_require__(1)))

/***/ },

/***/ 26:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {(function ($) {
	    $.fn.collapsibleMenu = function (options) {
	        var defaults = {
	            accordion: undefined,
	        };
	
	        options = $.extend(defaults, options);
	        var touch =
	                   'ontouchstart' in window
	                  || 'onmsgesturechange' in window;
	
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
	
	                    if (!$($this).hasClass('active'))
	                    {
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
	                var url = $(this).attr("data-url");
	                $('#mainFrame').prop('src',url);
	
	                if (touch && $('body').hasClass('site-menubar-fold')) {
	                    var element = $(e.target);
	                    var temp = element.closest('li > .collapsible-body');
	
	                    var element = temp.parent().find('.collapsible-header');
	
	                    element.trigger("click");
	                }
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
	
	    };
	
	    $(document).ready(function () {
	        $('.collapsible2').collapsibleMenu();
	
	    });
	}(jQuery));
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }

});
//# sourceMappingURL=layout.chunk.js.map