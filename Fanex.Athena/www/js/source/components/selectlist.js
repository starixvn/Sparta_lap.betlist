define(['jquery'], function ($) {
    "use strict";

    var CONTAINER_SELECTOR = ".select-list-container";
    var DISABLED_CLASS = "disabled";
    var HIGHLIGHT_CLASS = "highlighted";

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

    function getOptionItemHtml(dataItem) {
        return '<li class="option-item ' + dataItem.className + '" data-key="' + dataItem.key + '">' + dataItem.value + '</li>';
    }

    function getOptionListHtml(dataList) {
        var htmlArray = ['<div class="select-list-dropdown-results"><ul class="option-list">'];

        for (var i = 0; i < dataList.length; i++) {
            var dataItem = dataList[i];
            htmlArray.push(getOptionItemHtml(dataItem));
        }

        htmlArray.push('</ul></div>');

        return htmlArray.join("");
    }

    function scrollToHighLightItem($container, $item, index) {
        var itemTop = $item.offset().top;
        var itemHeight = $item.outerHeight();
        var containerTop = $container.offset().top;
        var scrollUp = null;
        if (itemTop <= containerTop) {
            scrollUp = true;
        }
        else if (itemTop >= containerTop + $container.height()) {
            scrollUp = false;
        }
        if (scrollUp === null) {
            return;
        }
        if (scrollUp) {
            $container.scrollTop(itemHeight * (index - 1) - $container.height());
        }
        else {
            $container.scrollTop(itemHeight * index);
        }
    }

    function keyUp($element) {
        var $selectList = $element.closest(".select-list-dropdown").find(".select-list-dropdown-results");
        var showOptionList = $selectList.find(".option-item:visible");
        var $highlightItem = $selectList.find(".option-item." + HIGHLIGHT_CLASS + ":visible").first();
        var index = showOptionList.index($highlightItem);

        $highlightItem.removeClass(HIGHLIGHT_CLASS);
        if ($highlightItem.length > 0 && index !== 0) {
            var $preItem = showOptionList.eq(index - 1);
            $preItem.addClass(HIGHLIGHT_CLASS);
            scrollToHighLightItem($selectList, $preItem, index - 1);
        }
        else {
            showOptionList.last().addClass(HIGHLIGHT_CLASS);
            $selectList.scrollTop(showOptionList.length * $highlightItem.outerHeight());
        }
    }

    function keyDown($element) {
        var $selectList = $element.closest(".select-list-dropdown").find(".select-list-dropdown-results");
        var showOptionList = $selectList.find(".option-item:visible");
        var $highlightItem = $selectList.find(".option-item." + HIGHLIGHT_CLASS + ":visible").first();
        var index = showOptionList.index($highlightItem);

        $highlightItem.removeClass(HIGHLIGHT_CLASS);
        if ($highlightItem.length > 0 && index !== (showOptionList.length - 1)) {
            var $nextItem = showOptionList.eq(index + 1);
            $nextItem.addClass(HIGHLIGHT_CLASS);
            scrollToHighLightItem($selectList, $nextItem, index + 1);
        }
        else {
            showOptionList.first().addClass(HIGHLIGHT_CLASS);
            $selectList.scrollTop(0);
        }
    }

    function keySearch($element, canEnter) {
        setTimeout(function () {
            var query = $element.val().toUpperCase();
            var $selectList = $element.closest(".select-list-dropdown").find(".select-list-dropdown-results");
            var optionList = $selectList.find(".option-item");
            var isFirstItem = canEnter;
            if (canEnter) {
                optionList.removeClass(HIGHLIGHT_CLASS);
            }
            optionList.each(function (index, item) {
                var $item = $(item);
                var currentText = $item.text().toUpperCase();
                if (currentText.indexOf(query) === -1) {
                    $item.hide();
                }
                else {
                    if (isFirstItem) {
                        $item.addClass(HIGHLIGHT_CLASS);
                        isFirstItem = false;
                        $selectList.scrollTop(0);
                    }
                    $item.show();
                }
            });
        }, 100);
    }

    function enter($element) {
        var $highlightItem = $element.closest(".select-list-dropdown").find(".option-item." + HIGHLIGHT_CLASS + ":visible");
        $highlightItem.click();
    }

    function getDropdownList(resultContainerId, canSearch, canEnter) {
        /*jshint validthis: true*/
        var $originalSelectElement = $(this);
        var dataList = $originalSelectElement.find("option").map(function (index, item) {
            var key = item.value;
            var text = item.text;

            return { key: key, value: text, className: item.className };
        });
        var selectedValue = $originalSelectElement.val();

        var dropdownListHtml = '<div id="' + resultContainerId + '" class="select-list-result-container"><div class="select-list-dropdown">' +
                                getSearchBoxHtml(canSearch) + getOptionListHtml(dataList) +
                            '</div></div>';
        var $dropdown = $(dropdownListHtml);
        $dropdown.find('.option-item[data-key="' + selectedValue + '"]').addClass(HIGHLIGHT_CLASS);

        $dropdown.on("click", ".option-item", function () {
            $(".option-item").removeClass(HIGHLIGHT_CLASS);
            $(this).addClass(HIGHLIGHT_CLASS);
            var selectedKey = $(this).data("key");
            $originalSelectElement.val(selectedKey);

            $originalSelectElement.trigger("change");
        });

        var $searchBox = $dropdown.find(".select-list-search-results");

        $searchBox.on("click", function (e) {
            e.stopPropagation();
        });

        if (canEnter) {
            $searchBox.on("keyup", function (e) {
                var $this = $(this);
                if (e.keyCode === 40) {
                    keyDown($this);
                }
                else if (e.keyCode === 38) {
                    keyUp($this);
                }
                else if (e.keyCode === 13) {
                    enter($this);
                }
                else {
                    keySearch($this, true);
                }
            });
        }
        else {
            $searchBox.on("keyup", function () {
                var $this = $(this);
                keySearch($this, false);
            });
        }

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
        canEnter: false,
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
                if (!selectListOption.canSearch) {
                    selectListOption.canSearch = $element.attr("data-can-search") === "1";
                }
                if (!selectListOption.canEnter) {
                    selectListOption.canEnter = $element.attr("data-can-enter") === "1";
                }

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

        open: function (resultContainerId, canSearch, canEnter) {
            closeAll();

            var $self = $(this);
            var $dropdown = getDropdownList.call(this, resultContainerId, canSearch, canEnter);

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

                    _methods.open.call(this, resultContainerId, option.canSearch, option.canEnter);

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