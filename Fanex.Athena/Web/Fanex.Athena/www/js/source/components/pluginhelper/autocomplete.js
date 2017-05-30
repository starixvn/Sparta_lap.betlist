// require option properties: suggestionDropdown, url, target
define(["jquery", "materialize-autocomplete"], function ($) {
    "use strict";

    var mainOption = {
        suggestionDropdown: '#suggestionDropdown',
        url: null,
        target: null
    };

    var _autoComplete;

    var _me = {
        initialize: function (option) {
            _me.initAutoComplete(option);
            _me.initEvent();
        },

        initEvent: function () {
            $('body').click(function () {
                _me.hideDropdown.call(_autoComplete);
            });

            $(mainOption.target).keypress(function (e) {
                if (e.keyCode === 13) {
                    _me.hideDropdown.call(_autoComplete);
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
                multiple: {
                    enable: false
                },
                dropdown: {
                    el: mainOption.suggestionDropdown
                },
                getData: function (value, callback) {
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
                        }
                    });
                }
            });

            _me.updateWidthOfSuggestionDropdown();
        },

        updateWidthOfSuggestionDropdown: function () {
            var $autoCompleteTarget = $(mainOption.target);
            var $suggestionDropdown = $(mainOption.suggestionDropdown);
            if ($suggestionDropdown.width() !== $autoCompleteTarget.width()) {
                $suggestionDropdown.width($autoCompleteTarget.width());
            }
        },

        hideDropdown: function () {
            this.$dropdown.html('').hide();
        }
    };

    return _me;
});