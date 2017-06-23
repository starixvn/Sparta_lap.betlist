// require option properties: suggestionDropdown, url, target
define(["jquery", "common/site", "materialize-autocomplete"], function ($, Site) {
    "use strict";

    var mainOption = {
        suggestionDropdown: '#suggestionDropdown',
        url: null,
        target: null
    };

    var DELAY_MILLISECOND = 600;

    var _autoComplete;

    var _me = {
        initialize: function (option) {
            _me.initAutoComplete(option);
            _me.initEvent();
        },

        initEvent: function () {
            $('body').click(function () {
                _me.hideDropdown();
            });

            $(mainOption.target).keyup(function (e) {
                if (e.keyCode === 13) {
                    _me.hideDropdown();
                }
            });
        },

        initAutoComplete: function (option) {
            $.extend(mainOption, option);
            if (!mainOption.url || !mainOption.target) {
                throw Error("Missing data in option");
            }

            _autoComplete = $(mainOption.target).materialize_autocomplete({ // jshint ignore:line
                ignoreCase: false,
                dropdown: {
                    el: mainOption.suggestionDropdown
                },
                getData: Site.debounce(_me.search, DELAY_MILLISECOND),
            });

            _me.updateWidthOfSuggestionDropdown();
        },

        search: function (value, callback) {
            var url = mainOption.url + "?query=" + value;

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
                    else {
                        _me.hideDropdown();
                    }
                }
            });
        },

        updateWidthOfSuggestionDropdown: function () {
            var $autoCompleteTarget = $(mainOption.target);
            var $suggestionDropdown = $(mainOption.suggestionDropdown);
            if ($suggestionDropdown.width() !== $autoCompleteTarget.width()) {
                $suggestionDropdown.width($autoCompleteTarget.width());
            }
        },

        hideDropdown: function () {
            _autoComplete.$dropdown.html('').hide();
        }
    };

    return _me;
});