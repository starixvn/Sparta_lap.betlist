webpackJsonp([54],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(255);


/***/ },

/***/ 252:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(9), __webpack_require__(253), __webpack_require__(254)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup, CustomerCommGroup) {
	         "use strict";
	
	         function initialize(titleResource) {
	             Site.initialize();
	          
	             initColorPicker();
	             _me.setPageTitle(Site.getSingleResource(titleResource));
	             _me.initEvent();
	         }
	
	         function initEvent() {
	             $("#GroupName").keyup(function () {
	                 $("#GroupName").removeClass("invalid valid");
	                 $("#Add").prop("disabled", false);
	             });
	
	             $("#Cancel").click(function () {
	                 _me.setPageTitle("CommissionGroupList");
	                 Site.redirect(Site.resolveClientUrl("CustomerComGroup/Index"));
	             });
	
	             // Commission dropdownlist change
	             $("#SBComm1").on("change", function () {
	                 commissionOnChange($(this).val(), "SBComm");
	             });
	
	             $("#ParlayComm1").on("change", function () {
	                 commissionOnChange($(this).val(), "ParlayComm");
	             });
	
	             $("#RCComm1").on("change", function () {
	                 commissionOnChange($(this).val(), "RCComm");
	             });
	
	             $("#NGComm1").on("change", function () {
	                 commissionOnChange($(this).val(), "NGComm");
	             });
	
	             $("#LCComm1").on("change", function () {
	                 commissionOnChange($(this).val(), "LCComm");
	             });
	
	             $("#CSComm1").on("change", function () {
	                 commissionOnChange($(this).val(), "CSComm");
	             });
	         }
	
	         function setPageTitle(title) {
	             CustomerCommGroup.setPageTitle(title);
	         }
	
	         function checkAvailableGroup(groupName) {
	             if (groupName.length < 1) {
	                 displayGroupNameError(Site.getSingleResource("GroupNameNotEmptyMsg"));
	                 return;
	             }
	
	             if (groupName.length >= 100) {
	                 displayGroupNameError(Site.getSingleResource("GroupNameNotExceed100CharactersMsg"));
	                 return;
	             }
	
	             if ($.trim(groupName).toLowerCase() === "none") {
	                 displayGroupNameError(Site.getSingleResource("ChooseGroupNameMsg"));
	                 return;
	             }
	
	             if (!groupName.match("^[a-zA-Z0-9 ]+$")) {
	                 displayGroupNameError(Site.getSingleResource("GroupNameOnlyContainCharactersAndNumberMsg"));
	                 return;
	             }
	
	             validateGroupName($.trim(groupName));
	         }
	
	         function validateGroupName(groupName) {
	             var url = "CustomerComGroup/Validate?groupName=" + groupName;
	
	             $.ajax({
	                 url: Site.resolveClientUrl(url),
	                 type: "GET",
	                 dataType: "html",
	                 cache: false,
	                 beforeSend: function () {
	                     Site.blockUI();
	                 },
	                 success: function (result) {
	                     if (result === "OK") {
	                         $("#GroupName").removeClass("invalid").addClass("valid");
	                         $("#Add").prop("disabled", false);
	                     }
	                     else {
	                         displayGroupNameError(Site.getSingleResource("GroupNameExistedMsg"));
	                     }
	
	                     Site.unBlockUI();
	                 }
	             });
	         }
	
	         function displayGroupNameError(errorMsg) {
	             $('label[for="GroupName"]').attr("data-error", errorMsg);
	             $("#GroupName").addClass("invalid").focus();
	             $("#Add").prop("disabled", true);
	         }
	
	         function commissionOnChange(value, idName) {
	             var comm2 = "#" + idName + "2", comm3 = "#" + idName + "3";
	
	             if (Number(value) === 15) {
	                 $(comm2).val(0);
	                 $(comm2).selectList("update", {
	                     isDisabled: true,
	                     selectionId: "select-list-" + idName + "2",
	                     labelText: "0"
	                 });
	                 $(comm3).val(0);
	                 $(comm3).selectList("update", {
	                     isDisabled: true,
	                     selectionId: "select-list-" + idName + "3",
	                     labelText: "0"
	                 });
	             } else {
	                 $(comm2).selectList("disabled", false);
	                 $(comm3).selectList("disabled", false);
	             }
	         }      
	
	         function submit(formId) {
	             initCommParam("#SBComm");
	             initCommParam("#ParlayComm");
	             initCommParam("#RCComm");
	             initCommParam("#NGComm");
	             initCommParam("#LCComm");
	             initCommParam("#CSComm");
	
	             initDefaultParam(formId);
	         }
	
	         function initCommParam(id) {
	             var comm1 = parseInt($(id + "1").val());
	             var comm2 = parseInt($(id + "2").val());
	             var comm3 = parseInt($(id + "3").val());
	
	             var comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
	             comm = isNaN(comm) ? 0 : comm;
	
	             $(id).val(comm);
	         }         
	
	         function initDefaultParam(formId) {
	             var isDefault = $("#IsDefaultGroup").is(":checked");
	             var defaultGroupId = parseInt($('#DefaultGroupId').val());
	             var currentGroupId = parseFloat($('#GroupCommId').val());
	             $("#IsDefaultGroup").val(isDefault);
	             $("#IsResetDefault").val(false);
	             if (isDefault && defaultGroupId !== currentGroupId && defaultGroupId !== 0) {
	                 showConfirmAction(Site.getSingleResource('GroupSetAsDefaultMsg'), function () {
	                     $("#IsResetDefault").val(true);
	                     $(formId).submit();
	                 });
	             } else {
	                 $(formId).submit();
	             }
	         }
	
	         function goToCustomerList(groupCommId) {
	             CustomerCommGroup.goToCustomerList(groupCommId);
	         }
	
	         function showConfirmAction(message, callback) {
	             var options = {
	                 modal: true,
	                 title: Site.getSingleResource("Confirm"),
	                 height: "auto",
	                 id: "confirm-action",
	                 width: 450
	             };
	
	             var buttons = [
	                 {
	                     text: Site.getSingleResource("OK"),
	                     isClosedOnClick: true,
	                     click: callback,
	                     classes: "btn btn-submit"
	                 }, {
	                     text: Site.getSingleResource("Cancel"),
	                     isClosedOnClick: true,
	                     classes: "btn btn-cancel"
	                 }];
	
	             Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
	         }
	
	         function initColorPicker() {
	             $("#GroupColor").colorPicker({
	                 transparency: true
	             });
	         }
	
	         var _me = {
	             initialize: initialize,
	             initEvent: initEvent,
	             submit: submit,
	             checkAvailableGroup: checkAvailableGroup,
	             setPageTitle: setPageTitle,
	             goToCustomerList: goToCustomerList
	         };
	
	         return _me;
	     }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 253:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
	    "use strict";
	
	    function goToCustomerList(groupCommId) {
	        if (isParentSameOrigin()) {
	            // Deactive Commission Group menu (55) and active Customer list menu (43)
	            $("li>#MenuItem55", window.parent.document).parent().removeClass("active");
	            $("li>#MenuItem43", window.parent.document).parent().addClass("active");
	            _me.setPageTitle(Site.getSingleResource("CustomerList"));
	        }
	        Site.redirect(Site.resolveClientUrl("Customer/CustomerList?groupId=" + groupCommId));
	    }
	
	    function setPageTitle(title) {
	        if (isParentSameOrigin) {
	            $("#page-title", window.parent.document).text(title);
	        }
	    }
	
	    function isParentSameOrigin() {
	        var sameOrigin;
	        try {
	            sameOrigin = window.parent.location.host === window.location.host;
	        }
	        catch (e) {
	            sameOrigin = false;
	        }
	        return sameOrigin;
	    }
	
	    var _me = {
	        goToCustomerList: goToCustomerList,
	        setPageTitle: setPageTitle
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 254:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/**
	 * Really Simple Color Picker in jQuery
	 *
	 * Licensed under the MIT (MIT-LICENSE.txt) licenses.
	 *
	 * Copyright (c) 2008-2012
	 * Lakshan Perera (www.laktek.com) & Daniel Lacy (daniellacy.com)
	 *
	 * Permission is hereby granted, free of charge, to any person obtaining a copy
	 * of this software and associated documentation files (the "Software"), to
	 * deal in the Software without restriction, including without limitation the
	 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
	 * sell copies of the Software, and to permit persons to whom the Software is
	 * furnished to do so, subject to the following conditions:
	 *
	 * The above copyright notice and this permission notice shall be included in
	 * all copies or substantial portions of the Software.
	 *
	 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
	 * IN THE SOFTWARE.
	 */
	
	(function ($) {
	    /**
	     * Create a couple private variables.
	    **/
	    var PICKER_PREVIEW_SELECTOR = ".colorPicker-preview";
	    var selectorOwner,
	        activePalette,
	        cItterate       = 0,
	        templates       = {
	            control: $('<div class="colorPicker-container"><div class="colorPicker-preview"></div><div class="colorPicker-handler"><span class="colorPicker-icon-arrow"></span></div></div>'),
	            palette : $('<div id="colorPicker_palette" class="colorPicker-palette" />'),
	            swatch  : $('<div class="colorPicker-swatch">&nbsp;</div>'),
	            hexLabel: $('<label for="colorPicker_hex">Hex</label>'),
	            hexField: $('<input type="text" id="colorPicker_hex" />')
	        },
	        transparent     = "transparent",
	        lastColor;
	
	    /**
	     * Create our colorPicker function
	    **/
	    $.fn.colorPicker = function (options) {
	
	        return this.each(function () {
	            // Setup time. Clone new elements from our templates, set some IDs, make shortcuts, jazzercise.
	            var element      = $(this),
	                opts         = $.extend({}, $.fn.colorPicker.defaults, options),
	                defaultColor = $.fn.colorPicker.toHex(
	                        (element.val().length > 0) ? element.val() : opts.pickerDefault
	                    ),
	                newControl   = templates.control.clone(),
	                newPalette   = templates.palette.clone().attr('id', 'colorPicker_palette-' + cItterate),
	                newHexLabel  = templates.hexLabel.clone(),
	                newHexField  = templates.hexField.clone(),
	                paletteId    = newPalette[0].id,
	                swatch, controlText;
	
	
	            /**
	             * Build a color palette.
	            **/
	            $.each(opts.colors, function (i) {
	                swatch = templates.swatch.clone();
	
	                if (opts.colors[i] === transparent) {
	                    swatch.addClass(transparent).text('X');
	                    $.fn.colorPicker.bindPalette(newHexField, swatch, transparent);
	                } else {
	                    swatch.css("background-color", "#" + this);
	                    $.fn.colorPicker.bindPalette(newHexField, swatch);
	                }
	                swatch.appendTo(newPalette);
	            });
	
	
	            newHexLabel.attr('for', 'colorPicker_hex-' + cItterate);
	
	            newHexField.attr({
	                'id'    : 'colorPicker_hex-' + cItterate,
	                'value' : defaultColor
	            });
	
	            newHexField.bind("keydown", function (event) {
	                if (event.keyCode === 13) {
	                    var hexColor = $.fn.colorPicker.toHex($(this).val());
	                    $.fn.colorPicker.changeColor(hexColor ? hexColor : element.val());
	                }
	                if (event.keyCode === 27) {
	                    $.fn.colorPicker.hidePalette();
	                }
	            });
	
	            newHexField.bind("keyup", function (event) {
	              var hexColor = $.fn.colorPicker.toHex($(event.target).val());
	              $.fn.colorPicker.previewColor(hexColor ? hexColor : element.val());
	            });
	
	            $('<div class="colorPicker_hexWrap" />').append(newHexLabel).appendTo(newPalette);
	
	            newPalette.find('.colorPicker_hexWrap').append(newHexField);
	            if (opts.showHexField === false) {
	                newHexField.hide();
	                newHexLabel.hide();
	            }
	
	            $("body").append(newPalette);
	
	            newPalette.hide();
	
	
	            /**
	             * Build replacement interface for original color input.
	            **/
	            newControl.find(PICKER_PREVIEW_SELECTOR).css("background-color", defaultColor);
	
	            newControl.bind("click", function () {
	                if( element.is( ':not(:disabled)' ) ) {
	                                    $.fn.colorPicker.togglePalette($('#' + paletteId), $(this));
	                }
	            });
	
	            if( options && options.onColorChange ) {
	              newControl.data('onColorChange', options.onColorChange);
	            } else {
	              newControl.data('onColorChange', function() {} );
	            }
	
	            if (controlText = element.data('text'))
	                newControl.html(controlText)
	
	            element.after(newControl);
	
	            element.bind("change", function () {
	                element.find(PICKER_PREVIEW_SELECTOR).css(
	                    "background-color", $.fn.colorPicker.toHex($(this).val())
	                );
	            });
	
	            element.val(defaultColor);
	
	            // Hide the original input.
	            if (element[0].tagName.toLowerCase() === 'input') {
	                try {
	                    element.attr('type', 'hidden')
	                } catch(err) { // oldIE doesn't allow changing of input.type
	                    element.css('visibility', 'hidden').css('position', 'absolute')
	                }
	            } else {
	                element.hide();
	            }
	
	            cItterate++;
	        });
	    };
	
	    /**
	     * Extend colorPicker with... all our functionality.
	    **/
	    $.extend(true, $.fn.colorPicker, {
	        /**
	         * Return a Hex color, convert an RGB value and return Hex, or return false.
	         *
	         * Inspired by http://code.google.com/p/jquery-color-utils
	        **/
	        toHex : function (color) {
	            // If we have a standard or shorthand Hex color, return that value.
	            if (color.match(/[0-9A-F]{6}|[0-9A-F]{3}$/i)) {
	                return (color.charAt(0) === "#") ? color : ("#" + color);
	
	            // Alternatively, check for RGB color, then convert and return it as Hex.
	            } else if (color.match(/^rgb\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)$/)) {
	                var c = ([parseInt(RegExp.$1, 10), parseInt(RegExp.$2, 10), parseInt(RegExp.$3, 10)]),
	                    pad = function (str) {
	                        if (str.length < 2) {
	                            for (var i = 0, len = 2 - str.length; i < len; i++) {
	                                str = '0' + str;
	                            }
	                        }
	
	                        return str;
	                    };
	
	                if (c.length === 3) {
	                    var r = pad(c[0].toString(16)),
	                        g = pad(c[1].toString(16)),
	                        b = pad(c[2].toString(16));
	
	                    return '#' + r + g + b;
	                }
	
	            // Otherwise we wont do anything.
	            } else {
	                return false;
	
	            }
	        },
	
	        /**
	         * Check whether user clicked on the selector or owner.
	        **/
	        checkMouse : function (event, paletteId) {
	            var selector = activePalette,
	                selectorParent = $(event.target).parents("#" + selector.attr('id')).length;
	
	            if (event.target === $(selector)[0] || event.target === selectorOwner[0] || selectorParent > 0) {
	                return;
	            }
	
	            $.fn.colorPicker.hidePalette();
	        },
	
	        /**
	         * Hide the color palette modal.
	        **/
	        hidePalette : function () {
	            $(document).unbind("mousedown", $.fn.colorPicker.checkMouse);
	
	            $('.colorPicker-palette').hide();
	        },
	
	        /**
	         * Show the color palette modal.
	        **/
	        showPalette : function (palette) {
	            var hexColor = selectorOwner.prev("input").val();
	
	            palette.css({
	                top: selectorOwner.offset().top + (selectorOwner.outerHeight()),
	                left: selectorOwner.offset().left
	            });
	
	            $("#color_value").val(hexColor);
	
	            palette.show();
	
	            $(document).bind("mousedown", $.fn.colorPicker.checkMouse);
	        },
	
	        /**
	         * Toggle visibility of the colorPicker palette.
	        **/
	        togglePalette : function (palette, origin) {
	            // selectorOwner is the clicked .colorPicker-picker.
	            if (origin) {
	                selectorOwner = origin;
	            }
	
	            activePalette = palette;
	
	            if (activePalette.is(':visible')) {
	                $.fn.colorPicker.hidePalette();
	
	            } else {
	                $.fn.colorPicker.showPalette(palette);
	
	            }
	        },
	
	        /**
	         * Update the input with a newly selected color.
	        **/
	        changeColor : function (value) {
	            selectorOwner.find(PICKER_PREVIEW_SELECTOR).css("background-color", value);
	            selectorOwner.prev("input").val(value).change();
	
	            $.fn.colorPicker.hidePalette();
	
	            selectorOwner.data('onColorChange').call(selectorOwner, $(selectorOwner).prev("input").attr("id"), value);
	        },
	
	
	        /**
	         * Preview the input with a newly selected color.
	        **/
	        previewColor : function (value) {
	            selectorOwner.find(PICKER_PREVIEW_SELECTOR).css("background-color", value);
	        },
	
	        /**
	         * Bind events to the color palette swatches.
	        */
	        bindPalette : function (paletteInput, element, color) {
	            color = color ? color : $.fn.colorPicker.toHex(element.css("background-color"));
	
	            element.bind({
	                click : function (ev) {
	                    lastColor = color;
	
	                    $.fn.colorPicker.changeColor(color);
	                },
	                mouseover : function (ev) {
	                    lastColor = paletteInput.val();
	
	                    $(this).css("border-color", "#598FEF");
	
	                    paletteInput.val(color);
	
	                    $.fn.colorPicker.previewColor(color);
	                },
	                mouseout : function (ev) {
	                    $(this).css("border-color", "#fff");
	
	                    paletteInput.val(selectorOwner.css("background-color"));
	
	                    paletteInput.val(lastColor);
	
	                    $.fn.colorPicker.previewColor(lastColor);
	                }
	            });
	        }
	    });
	
	    /**
	     * Default colorPicker options.
	     *
	     * These are publibly available for global modification using a setting such as:
	     *
	     * $.fn.colorPicker.defaults.colors = ['151337', '111111']
	     *
	     * They can also be applied on a per-bound element basis like so:
	     *
	     * $('#element1').colorPicker({pickerDefault: 'efefef', transparency: true});
	     * $('#element2').colorPicker({pickerDefault: '333333', colors: ['333333', '111111']});
	     *
	    **/
	    $.fn.colorPicker.defaults = {
	        // colorPicker default selected color.
	        pickerDefault : "FFFFFF",
	
	        // Default color set.
	        colors : [
	            '000000', '993300', '333300', '000080', '333399', '333333', '800000', 'FF6600',
	            '808000', '008000', '008080', '0000FF', '666699', '808080', 'FF0000', 'FF9900',
	            '99CC00', '339966', '33CCCC', '3366FF', '800080', '999999', 'FF00FF', 'FFCC00',
	            'FFFF00', '00FF00', '00FFFF', '00CCFF', '993366', 'C0C0C0', 'FF99CC', 'FFCC99',
	            'FFFF99', 'CCFFFF', '99CCFF', 'FFFFFF'
	        ],
	
	        // If we want to simply add more colors to the default set, use addColors.
	        addColors : [],
	
	        // Show hex field
	        showHexField: false
	    };
	
	})(jQuery);
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 255:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(256)], __WEBPACK_AMD_DEFINE_RESULT__ = function (CustomerCommGroupEdit) {
	    "use strict";
	
	    $(CustomerCommGroupEdit.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 256:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(252)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, CommGroupDetail) {
	         "use strict";
	
	         var FORM_SELECTOR = "#customer-comm-group-edit";
	         var TITLE = "EditGroup";
	         var _originalGroupName = "";
	
	         function initialize() {
	             CommGroupDetail.initialize(TITLE);
	
	             _originalGroupName = $("#OriginalGroupName").val();
	             _me.initEvent();
	         }
	
	         function initEvent() {
	             $("#Validate").click(function () {
	                 var $groupName = $("#GroupName");
	
	                 if ($groupName.val() === _originalGroupName) {
	                     $groupName.removeClass("invalid");
	                     $groupName.addClass("valid");
	                     $("#Add").prop("disabled", false);
	                     return;
	                 }
	
	                 CommGroupDetail.checkAvailableGroup($groupName.val());
	             });
	
	             $("#CustList").click(function () {
	                 CommGroupDetail.goToCustomerList($('#GroupCommId').val());
	             });
	
	             $("#Submit").click(function () {
	                 CommGroupDetail.submit(FORM_SELECTOR);
	             });
	         }         
	
	         var _me = {
	             initialize: initialize,
	             initEvent: initEvent
	         };
	
	         return _me;
	     }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }

});
//# sourceMappingURL=commgroupedit.chunk.js.map