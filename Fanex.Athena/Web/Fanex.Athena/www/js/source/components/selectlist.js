define(['jquery'], function ($) {
    "use strict";

    var CONTAINER_SELECTOR = ".select-list-container";
    var DISABLED_CLASS = "disabled";

    function getLabelHtml(option) {
        var targetDropdownId = option.selectionId + "-result-container";
        var className = option.isDisabled ? "selection disabled" : "selection";
        return '<div class="select-list-container">' +
                '<div class="' + className + '" id="' + option.selectionId + '" aria-targetDropdown="' + targetDropdownId + '">' +
                    '<div class="selection-label">' + option.labelText + '</div>' +
                    '<div class="selection-icon"><span class="selection-icon-arrow"></span></div>' +
                '</div>' +
            '</div>';
    }

    function getSearchBoxHtml(isShow) {
        return isShow ? '<div class="select-list-dropdown-search"><input class="select-list-search-results" autocomplete="off" /></div>' : '';
    }

    function getOptionItemHtml(key, value) {
        return '<li class="option-item" data-key="' + key + '">' + value + '</li>';
    }

    function getOptionListHtml(dataList) {
        var htmlArray = ['<div class="select-list-dropdown-results"><ul class="option-list">'];

        for (var i = 0; i < dataList.length; i++) {
            var item = dataList[i];
            htmlArray.push(getOptionItemHtml(item.key, item.value));
        }

        htmlArray.push('</ul></div>');

        return htmlArray.join("");
    }

    function getDropdownList(resultContainerId, canSearch) {
        /*jshint validthis: true*/
        var $originalSelectElement = $(this);
        var dataList = $originalSelectElement.find("option").map(function (index, item) {
            var key = item.value;
            var text = item.text;

            return { key: key, value: text };
        });
        var selectedValue = $originalSelectElement.val();

        var dropdownListHtml = '<div id="' + resultContainerId + '" class="select-list-result-container"><div class="select-list-dropdown">' +
                                getSearchBoxHtml(canSearch) + getOptionListHtml(dataList) +
                            '</div></div>';
        var $dropdown = $(dropdownListHtml);
        $dropdown.find('.option-item[data-key="' + selectedValue + '"]').addClass("highlighted");

        $dropdown.on("click", ".option-item", function () {
            $(".option-item").removeClass("highlighted");
            $(this).addClass("highlighted");
            var selectedKey = $(this).data("key");
            $originalSelectElement.val(selectedKey);

            $originalSelectElement.trigger("change");
        });

        var $searchBox = $dropdown.find(".select-list-search-results");

        $searchBox.on("click", function (e) {
            e.stopPropagation();
        });

        $searchBox.on("keyup", function () {
            var $this = $(this);

            setTimeout(function () {
                var query = $this.val().toUpperCase();
                var optionList = $this.closest(".select-list-dropdown").find(".option-item");

                optionList.each(function (index, element) {
                    var $element = $(element);
                    var currentText = $element.text().toUpperCase();
                    if (currentText.indexOf(query) === -1) {
                        $element.hide();
                    }
                    else {
                        $element.show();
                    }
                });
            }, 100);
        });

        return $dropdown;
    }

    function generateId($element) {
        var id = '';

        if ($element.attr('id') !== null) {
            id = $element.attr('id');
        } else if ($element.attr('name') !== null) {
            id = $element.attr('name') + '-' + generateChars(2);
        } else {
            id = generateChars(4);
        }

        id = id.replace(/(:|\.|\[|\]|,)/g, '');
        id = 'select-list-' + id;

        return id;
    }

    function generateChars(length) {
        var chars = '';

        for (var i = 0; i < length; i++) {
            var randomChar = Math.floor(Math.random() * 36);
            chars += randomChar.toString(36);
        }

        return chars;
    }

    function closeAll() {
        $(".select-list-result-container").remove();
    }

    var defaultOption = {
        canSearch: false,
        isDisabled: false,
        beforeOpen: function () { },
        afterOpen: function () { }
    };

    var _methods = {
        init: function (options) {
            var selectListOption = $.extend({}, defaultOption, options || {});

            return this.each(function () {
                var self = this;
                var $element = $(self);
                _methods.buildLabel.call(self);

                var $container = $element.parent();
                $container.off("click", ".selection");
                $container.on("click", ".selection", function () {
                    var $selection = $(this);
                    if (!$selection.hasClass(DISABLED_CLASS)) {
                        _methods.toggle.call(self, selectListOption);
                    }
                });

                $element.on("change", function () {
                    _methods.update.call(self);
                });

                $("html").on("click", function (e) {
                    var $target = $(e.target);

                    if (!$target.closest(CONTAINER_SELECTOR).length) {
                        closeAll();
                    }
                });
            });
        },

        buildLabel: function (option) {
            option = option || {};
            var $self = $(this);
            $self.siblings(CONTAINER_SELECTOR).remove();
            var optionSelected = $self.find("option:selected");
            var labelText = optionSelected.length ? optionSelected.text() : "";
            var selectionId = generateId($self);

            option.labelText = labelText;
            option.selectionId = selectionId;
            var $selectionLabel = $(getLabelHtml(option));

            $self.hide().parent().append($selectionLabel);
        },

        open: function (resultContainerId, canSearch) {
            closeAll();

            var $self = $(this);
            var $dropdown = getDropdownList.call(this, resultContainerId, canSearch);

            var $selection = $self.siblings(CONTAINER_SELECTOR).find(".selection");
            var offset = $selection.offset();
            $dropdown.css({ top: (offset.top + 26) + 'px', left: offset.left + 'px', width: $selection.width() + 'px' });

            $("body").append($dropdown);
            $dropdown.find(".select-list-search-results").focus();
        },

        close: function () {
            var $this = $(this);
            var $selection = $this.siblings(CONTAINER_SELECTOR).find(".selection");
            var resultContainerId = $selection.attr("aria-targetDropdown");
            $("#" + resultContainerId).remove();
        },

        toggle: function (option) {
            var $this = $(this);
            var $selection = $this.siblings(CONTAINER_SELECTOR).find(".selection");
            var resultContainerId = $selection.attr("aria-targetDropdown");

            if (resultContainerId && resultContainerId.length > 0) {
                var $currentDropdown = $("#" + resultContainerId);
                var isShowing = $currentDropdown.length > 0;

                if (isShowing) {
                    $currentDropdown.remove();
                }
                else {
                    if (typeof option.beforeOpen === "function") {
                        option.beforeOpen.call(this);
                    }

                    _methods.open.call(this, resultContainerId, option.canSearch);

                    if (typeof option.afterOpen === "function") {
                        option.afterOpen.call(this);
                    }
                }
            }
        },

        update: function (option) {
            _methods.buildLabel.call(this, option);
        },

        disabled: function (isDisabled) {
            var $selection = $(this).siblings(CONTAINER_SELECTOR).find(".selection");
            $selection.toggleClass(DISABLED_CLASS, !!isDisabled);
        }
    };

    $.fn.selectList = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on selectList');
        }
    };
});