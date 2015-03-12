/// <reference path="/@JSense.js" />
/**
* --------------------------------------------------------------------
* jQuery-Plugin "daterangepicker.jQuery.js"
* by Scott Jehl, scott@filamentgroup.com
* reference article: http:// www.filamentgroup.com/lab/update_date_range_picker_with_jquery_ui/
* demo page: http:// www.filamentgroup.com/examples/daterangepicker/
*
* Copyright (c) 2010 Filament Group, Inc
* Dual licensed under the MIT (filamentgroup.com/examples/mit-license.txt) and GPL (filamentgroup.com/examples/gpl-license.txt) licenses.
*
* Dependencies: jquery, jquery UI datepicker, date.js, jQuery UI CSS Framework

*  12.15.2010 Made some fixes to resolve breaking changes introduced by jQuery UI 1.8.7

var _defaultPresetRanges = [
{ text: 'Today', dateStart: 'today', dateEnd: 'today' },
{ text: 'Yesterday', dateStart: 'yesterday', dateEnd: 'yesterday' },
{ text: 'Current week', dateStart: 'sunday', dateEnd: 'sartuday' },
{ text: 'Last week', dateStart: function () { return Date.parse('sunday').add(-7).day(); }, dateEnd: function () { return Date.parse('saturday').add(-7).day(); } },
{ text: 'Current month', dateStart: function () { return Date.today().moveToFirstDayOfMonth(); }, dateEnd: 'today' },
{ text: 'Last month', dateStart: function () { return Date.parse('1 month ago').moveToFirstDayOfMonth(); }, dateEnd: function () { return Date.parse('1 month ago').moveToLastDayOfMonth(); } },
{ text: 'Since last month', dateStart: function () { return Date.parse('1 month ago').moveToFirstDayOfMonth(); }, dateEnd: 'today' }

{text: 'Last 7 days', dateStart: 'today-7days', dateEnd: 'today' },
{ text: 'Month to date', dateStart: function () { return Date.parse('today').moveToFirstDayOfMonth(); }, dateEnd: 'today' },
{ text: 'Year to date', dateStart: function () { var x = Date.parse('today'); x.setMonth(0); x.setDate(1); return x; }, dateEnd: 'today' },
{text: 'The previous Month', dateStart: function () { return Date.parse('1 month ago').moveToFirstDayOfMonth(); }, dateEnd: function () { return Date.parse('1 month ago').moveToLastDayOfMonth(); } }
{text: 'Tomorrow', dateStart: 'Tomorrow', dateEnd: 'Tomorrow' },
{text: 'Ad Campaign', dateStart: '03/07/08', dateEnd: 'Today' },
{text: 'Last 30 Days', dateStart: 'Today-30', dateEnd: 'Today' },
{text: 'Next 30 Days', dateStart: 'Today', dateEnd: 'Today+30' },
{text: 'Our Ad Campaign', dateStart: '03/07/08', dateEnd: '07/08/08' }
* --------------------------------------------------------------------
*/

(function ($) {
    var _defaultPresetRanges = [{
        text: 'Today',
        dateStart: function () {
            return Date.parse(ServerDate).day();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).day();
        }
    },
    {
        text: 'Yesterday',
        dateStart: function () {
            return Date.parse(ServerDate).add(-1).day();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).add(-1).day();
        }
    },
    {
        text: 'Currentweek',
        dateStart: function () {
            return Date.parse(ServerDate).previous().sunday();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).day();
        }
    },
    {
        text: 'Lastweek',
        dateStart: function () {
            return Date.parse(ServerDate).previous().sunday().add(-7).day();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).previous().sunday().add(-1).day();
        }
    },
    {
        text: 'Currentmonth',
        dateStart: function () {
            return Date.parse(ServerDate).moveToFirstDayOfMonth();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).day();
        }
    },
    {
        text: 'Lastmonth',
        dateStart: function () {
            return Date.parse(ServerDate).addMonths(-1).moveToFirstDayOfMonth();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).addMonths(-1).moveToLastDayOfMonth();
        }
    },
    {
        text: 'Sincelastmonth',
        dateStart: function () {
            return Date.parse(ServerDate).addMonths(-1).moveToFirstDayOfMonth();
        },
        dateEnd: function () {
            return Date.parse(ServerDate).day();
        }
    }];

    var _instances = [];
    var _options = {};
    var _rangeInput;
    var _rangeInputParent;
    var _rangePicker;
    var _rangePickerWrapper;
    var _initialized = false;
    var _inputDateA;
    var _inputDateB;
    var _doneButton;
    /*
    * Private methods
    */

    // function to format a date string
    var _fDate = function (date) {
        if (!date.getDate()) {
            return '';
        }
        var day = date.getDate();
        var month = date.getMonth();
        var year = date.getFullYear();
        month++;
        // adjust javascript month
        var dateFormat = _options.dateFormat;
        return jQuery.datepicker.formatDate(dateFormat, date);
    };
    // show, hide, or toggle rangepicker
    var _showRP = function () {
        if (_rangePicker.data('state') == 'closed') {
            _positionRP();
            _rangePicker.fadeIn(300, function () {
                if (typeof (_rangePicker.data("activePreset")) == undefined || _rangePicker.data("activePreset") == null) {
                    _rangePicker.find('li[presettype="' + _options.defaultPreset.replace(/ /g, '') + '"]').trigger('click');
                }
            }).data('state', 'open');
            _options.onOpen();
        }
    };

    var _hideRP = function () {
        if (_rangePicker.data('state') == 'open') {
            _rangePicker.fadeOut(300).data('state', 'closed');
            _options.onClose();
        }
    };

    var _toggleRP = function () {
        if (_rangePicker.data('state') == 'open') {
            _hideRP();
        }
        else {
            _showRP();
        }
    };

    var _positionRP = function () {
        var relEl = _rangeInputParent || _rangeInput;
        // if arrows, use parent for offsets
        var riOffset = relEl.offset(), side = 'left', val = riOffset.left, offRight = jQuery(window).width() - val - relEl.outerWidth();

        if (val > offRight) {
            side = 'right', val = offRight;
        }

        _rangePicker.parent().css(side, val).css('top', riOffset.top + relEl.outerHeight());
    };

    var ServerDate;

    var _initOption = function (settings) {
        ServerDate = settings.today;
        var presetRanges = typeof (settings.presetRanges) == undefined || settings.presetRanges == null ? _defaultPresetRanges : settings.presetRanges;
        _options = jQuery.extend({
            // presetRanges: array of objects for each menu preset.
            // Each obj must have text, dateStart, dateEnd. dateStart, dateEnd accept date.js string or a function which returns a date object
            presets:
            {
                specificDate: Site.getSingleResource('SpecificDate'),
                dateRange: Site.getSingleResource('DateRange')
                // allDatesBefore: 'All Dates Before',
                // allDatesAfter: 'All Dates After',
            },
            disabledPreset: [],
            defaultPreset: Site.getSingleResource('DateRange'),
            rangeStartTitle: Site.getSingleResource('Fromdate'),
            rangeEndTitle: Site.getSingleResource('Todate'),
            nextLinkText: Site.getSingleResource('Next'),
            prevLinkText: Site.getSingleResource('Prev'),
            doneButtonText: Site.getSingleResource('OK'),
            earliestDate: Date.parse('-15years'),
            // earliest date allowed
            latestDate: Date.parse('+15years'),
            // latest date allowed
            constrainDates: false,
            // to date must larger than from date
            rangeSplitter: '-',
            // string to use between dates in single input
            dateFormat: 'd M yy',
            // date formatting. Available formats: http:// docs.jquery.com/UI/Datepicker/%24.datepicker.formatDate
            closeOnSelect: true,
            // if a complete selection is made, close the menu
            arrows: false,
            appendTo: 'body',
            onClose: function () { },
            onOpen: function () { },
            onChange: function () { },
            datepickerOptions: null
            // object containing native UI datepicker API _options
        }, settings);

        _options.presetRanges = presetRanges;
    };

    var _initDatePickerOption = function (settings) {
        // custom datepicker _options, extended by _options
        var datepickerOptions =
        {
            onSelect: function (dateText, inst) {
                var isSpecificDate = _rangePicker.find('.ui-daterangepicker-specificDate').is('.ui-state-active');
                if (isSpecificDate) {
                    _rangePicker.find('.range-end').datepicker('setDate', _rangePicker.find('.range-start').datepicker('getDate'));
                }

                jQuery(this).trigger('constrainOtherPicker');

                var rangeA = _fDate(_rangePicker.find('.range-start').datepicker('getDate'));
                var rangeB = isSpecificDate ? rangeA : _fDate(_rangePicker.find('.range-end').datepicker('getDate'));

                // send back to input or inputs
                if (_rangeInput.length == 2) {
                    _rangeInput.eq(0).val(rangeA);
                    _rangeInput.eq(1).val(rangeB);
                }
                else {
                    _rangeInput.val((rangeA != rangeB) ? rangeA + ' ' + _options.rangeSplitter + ' ' + rangeB : rangeA);
                }
                // if closeOnSelect is true
                if (_options.closeOnSelect) {
                    if (!_rangePicker.find('li.ui-state-active').is('.ui-daterangepicker-dateRange') && !_rangePicker.is(':animated')) {
                        _hideRP();
                    }
                }

                // Fire onChange only one time
                var isPresetActive = false;
                $(_defaultPresetRanges).each(function (index, item) {
                    var presetType = item.text.replace(/ /g, '');
                    isPresetActive = isPresetActive || _rangePicker.find('.ui-daterangepicker-' + presetType).is('.ui-state-active');
                });

                if (isPresetActive && $(this).is(".range-end")) {
                    _options.onChange()
                };
            },
            minDate: _options.earliestDate,
            maxDate: _options.latestDate,
            showOtherMonths: true,
            changeMonth: true,
            changeYear: true,
            selectOtherMonths: true,
            closeText: 'OK',
            defaultDate: +0
        };

        // datepicker _options from _options
        _options.datepickerOptions = (settings) ? jQuery.extend(datepickerOptions, settings.datepickerOptions) : datepickerOptions;
    };

    var _makeup = function (settings) {
        // Insert calendar image
        if (typeof (settings.buttonImage) != undefined) {
            jQuery("<img alt='' title='Show date range picker' class='ui-daterangepicker-trigger' src='" + settings.buttonImage + "' />").insertAfter(_rangeInput).click(function () {
                _toggleRP();
                return false;
            });
        }

        // Capture Dates from input(s)
        _inputDateB = Date.parse('today');
        var inputDateAtemp,
        inputDateBtemp;
        if (_rangeInput.size() == 2) {
            inputDateAtemp = Date.parse(_rangeInput.eq(0).prop('data-original'));
            inputDateBtemp = Date.parse(_rangeInput.eq(1).prop('data-original'));
            if (inputDateAtemp == null) {
                inputDateAtemp = inputDateBtemp;
            }
            if (inputDateBtemp == null) {
                inputDateBtemp = inputDateAtemp;
            }
        }
        else {
            inputDateAtemp = Date.parse(_rangeInput.val().split(_options.rangeSplitter)[0]);
            inputDateBtemp = Date.parse(_rangeInput.val().split(_options.rangeSplitter)[1]);
            if (inputDateBtemp == null) {
                inputDateBtemp = inputDateAtemp;
            }
            // if one date, set both
        }
        if (inputDateAtemp != null) {
            _inputDateA = inputDateAtemp;
        }
        if (inputDateBtemp != null) {
            _inputDateB = inputDateBtemp;
        }
    };

    var _build = function (settings) {
        // build picker
        _rangePicker = jQuery('<div class="ui-daterangepicker ui-widget ui-helper-clearfix ui-widget-content ui-corner-all"></div>');
        var rpPresets = (function () {
            var ul = jQuery('<ul class="ui-widget-content"></ul>').appendTo(_rangePicker);
            jQuery.each(_options.presetRanges, function () {
                var presetName = this.text.replace(/ /g, '');
                var enabled = $.inArray(presetName, _options.disabledPreset) < 0;

                var presetRangeItem =
                jQuery('<li class="ui-daterangepicker-' + presetName + ' ui-corner-all" presettype="' + this.text.replace(/ /g, '') + '"><a href="javascript:void();">' + 
                    Site.getSingleResource(presetName) + '</a></li>').data('dateStart', this.dateStart).data('dateEnd', this.dateEnd).addClass(enabled ? 'enabled' : 'ui-helper-hidden').appendTo(ul);
            });
            var x = 0;
            jQuery.each(_options.presets, function (key, value) {
                jQuery('<li class="ui-daterangepicker-' + key + ' preset_' + x + ' ui-helper-clearfix ui-corner-all" presettype="' + value.replace(/ /g, '') + '"><span class="ui-icon ui-icon-triangle-1-e"></span><a href="#">' + value + '</a></li>').addClass('enabled').appendTo(ul);
                x++;
            });

            ul.find('li.enabled').hover(function () {
                jQuery(this).addClass('ui-state-hover');
            }, function () {
                jQuery(this).removeClass('ui-state-hover');
            }).click(function () {
                _rangePicker.find('.ui-state-active').removeClass('ui-state-active');
                jQuery(this).addClass('ui-state-active');
                _clickActions(jQuery(this), _rangePicker, _rangePickerWrapper, _doneButton);
                return false;
            });
            return ul;
        })();
    };

    var _buildPickerWrapper = function (settings) {
        // picker divs
        _rangePickerWrapper = jQuery('<div class="ranges ui-widget-header ui-corner-all ui-helper-clearfix"><div class="range-start"><span class="title-start">Start Date</span></div><div class="range-end"><span class="title-end">' + Site.getSingleResource("EndDate") + '</span></div></div>').appendTo(_rangePicker);
        _rangePickerWrapper.find('.range-start, .range-end').datepicker(_options.datepickerOptions);

        _rangePickerWrapper.find('.range-start').datepicker('setDate', _inputDateA);
        _rangePickerWrapper.find('.range-end').datepicker('setDate', _inputDateB);

        _rangePickerWrapper.find('.range-start, .range-end').on('constrainOtherPicker', function () {
            if (_options.constrainDates) {
                // constrain dates
                if (jQuery(this).is('.range-start')) {
                    _rangePicker.find('.range-end').datepicker("option", "minDate", jQuery(this).datepicker('getDate'));
                }
                else {
                    _rangePicker.find('.range-start').datepicker("option", "maxDate", jQuery(this).datepicker('getDate'));
                }
            }
        }).trigger('constrainOtherPicker');

        _doneButton = jQuery('<button class="btnDone ui-state-default ui-corner-all">' + _options.doneButtonText + '</button>').click(function () {
            _rangePicker.find('.ui-datepicker-current-day').trigger('click');
            _hideRP();
        }).hover(function () {
            jQuery(this).addClass('ui-state-hover');
        }, function () {
            jQuery(this).removeClass('ui-state-hover');
        }).appendTo(_rangePickerWrapper);

        // hide em all
        _rangePickerWrapper.hide().find('.range-start, .range-end, .btnDone').hide();

        // Fixed for jQuery UI 1.8.7 - Calendars are hidden otherwise!
        _rangePickerWrapper.find('.ui-datepicker').css("display", "block");

        // inject _rangePicker
        jQuery(_options.appendTo).append(_rangePicker);

        // wrap and position
        _rangePicker.wrap('<div class="ui-daterangepickercontain"></div>');
    };

    var _buildEvent = function (settings) {
        // change event fires both when a calendar is updated or a change event on the input is triggered
        _rangeInput.on('change', _options.onChange);

        // inputs toggle rangepicker visibility
        _rangeInput.click(function () {
            _toggleRP();
            return false;
        });

        jQuery(document).click(function () {
            if (_rangePicker.is(':visible')) {
                _hideRP();
            }
        });

        _rangePicker.click(function () {
            return false;
        }).hide();
    };

    var _presetData = function (settings) {
        _rangePicker.data('state', 'closed');
    }
    // preset menu click events
    var _clickActions = function (el, _rangePicker, _rangePickerWrapper, _doneButton) {
        _rangePicker.data("activePreset", el.attr("presettype"));

        if (el.is('.ui-daterangepicker-specificDate')) {
            // Specific Date (show the "start" calendar)
            _doneButton.hide();
            _rangePickerWrapper.show();
            _rangePicker.find('.title-start').text(_options.presets.specificDate);
            _rangePicker.find('.range-start').restoreDateFromData().css('opacity', 1).show(400);
            _rangePicker.find('.range-end').restoreDateFromData().css('opacity', 0).hide(400);
            setTimeout(function () {
                _doneButton.fadeIn();
            }, 400);
        }
        else if (el.is('.ui-daterangepicker-allDatesBefore')) {
            // All dates before specific date (show the "end" calendar and set the "start" calendar to the earliest date)
            _doneButton.hide();
            _rangePickerWrapper.show();
            _rangePicker.find('.title-end').text(_options.presets.allDatesBefore);
            _rangePicker.find('.range-start').saveDateToData().datepicker('setDate', _options.earliestDate).css('opacity', 0).hide(400);
            _rangePicker.find('.range-end').restoreDateFromData().css('opacity', 1).show(400);
            setTimeout(function () {
                _doneButton.fadeIn();
            }, 400);
        }
        else if (el.is('.ui-daterangepicker-allDatesAfter')) {
            // All dates after specific date (show the "start" calendar and set the "end" calendar to the latest date)
            _doneButton.hide();
            _rangePickerWrapper.show();
            _rangePicker.find('.title-start').text(_options.presets.allDatesAfter);
            _rangePicker.find('.range-start').restoreDateFromData().css('opacity', 1).show(400);
            _rangePicker.find('.range-end').saveDateToData().datepicker('setDate', _options.latestDate).css('opacity', 0).hide(400);
            setTimeout(function () {
                _doneButton.fadeIn();
            }, 400);
        }
        else if (el.is('.ui-daterangepicker-dateRange')) {
            // Specific Date range (show both calendars)
            _doneButton.hide();
            _rangePickerWrapper.show();
            _rangePicker.find('.title-start').text(_options.rangeStartTitle);
            _rangePicker.find('.title-end').text(_options.rangeEndTitle);
            _rangePicker.find('.range-start').restoreDateFromData().css('opacity', 1).show(400);
            _rangePicker.find('.range-end').restoreDateFromData().css('opacity', 1).show(400);

            var getStartMonthSelected = $('.range-start .ui-datepicker-month').find(":selected").val();

            var getEndMonthSelected = $('.range-end .ui-datepicker-month').find(":selected").val();

            if (getStartMonthSelected > getEndMonthSelected) {
                _rangePicker.find('.range-start').datepicker('setDate', _rangePicker.find('.range-end').datepicker('getDate'));
            }

            setTimeout(function () {
                _doneButton.fadeIn();
            }, 400);
        }
        else {
            // custom date range specified in the _options (no calendars shown)
            _doneButton.hide();
            _rangePicker.find('.range-start, .range-end').css('opacity', 0).hide(400, function () {
                _rangePickerWrapper.hide();
            });
            var dateStart = (typeof el.data('dateStart') == 'string') ? Date.parse(el.data('dateStart')) : el.data('dateStart')();
            var dateEnd = (typeof el.data('dateEnd') == 'string') ? Date.parse(el.data('dateEnd')) : el.data('dateEnd')();

            _rangePicker.find('.range-start').datepicker('setDate', dateStart).find('.ui-datepicker-current-day').trigger('click');
            _rangePicker.find('.range-end').datepicker('setDate', dateEnd).find('.ui-datepicker-current-day').trigger('click');
        }

        return false;
    };

    /*
    * Public methods
    */
    var _methods =
    {
        initialize: function (settings) {
            _rangeInput = $(this);

            _initOption(settings);
            _initDatePickerOption(settings);
            _makeup(settings);
            _build(settings);
            _buildPickerWrapper(settings);
            _buildEvent(settings);
            _presetData(settings);

            _initialized = true;
            _instances.push(_rangeInput);

            return $(this);
        },
        option: function (settings) {
            _options = jQuery.extend(_options, settings);
            return $(this);
        },
        instance: function (index) {
            return index < 0 || index >= _instances.length ? $() : _instances[index];
        }
    };
    $.fn.daterangepicker = function (method) {
        if (_methods[method]) {
            // call init method
            if (!_initialized) {
                throw "Error! The function initialize has not be called yet.";
            }
            // call a method but init
            return _methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof method === 'object' || !method) {
            return _methods.initialize.apply(this, arguments);
        }
        else {
            //
            logger.error('Oops! Method "' + method + '" does not exist on jQuery.monPickr');
        }
    };
    $.fn.restoreDateFromData = function () {
        if (jQuery(this).data('saveDate')) {
            jQuery(this).datepicker('setDate', jQuery(this).data('saveDate')).removeData('saveDate');
        }
        return this;
    }

    $.fn.saveDateToData = function () {
        if (!jQuery(this).data('saveDate')) {
            jQuery(this).data('saveDate', jQuery(this).datepicker('getDate'));
        }
        return this;
    }
})(jQuery);