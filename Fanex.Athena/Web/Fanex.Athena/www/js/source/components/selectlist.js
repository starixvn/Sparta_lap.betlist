"use strict";
define(['jquery'], function ($) {
    function getLabelHtml(labelText, selectionId) {
        var targetDropdownId = selectionId + "-result-container";
        return '<div class="select-list-container">' +
                '<div class="selection" id="' + selectionId + '" aria-targetDropdown="' + targetDropdownId + '">' +
                    '<div class="selection-label">' + labelText + '</div>' +
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

        $dropdown.find(".option-item").on("click", function () {
            $(".option-item").removeClass("highlighted");
            $(this).addClass("highlighted");
            var selectedKey = $(this).data("key");
            $originalSelectElement.val(selectedKey);

            var $resultContainer = $(this).closest(".select-list-result-container");
            var $selectionLabel = $originalSelectElement.siblings(".select-list-container").find(".selection-label");
            $selectionLabel.text($(this).text());

            $resultContainer.remove();

            console.log($originalSelectElement.val());
        });

        var $searchBox = $dropdown.find(".select-list-search-results");

        $searchBox.on("click", function (e) {
            e.stopPropagation();
        });

        $searchBox.on("keyup", function (e) {
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

    var _methods = {
        init: function (options) {
            var selectListOption = {
                canSearch: false
            };
            $.extend(selectListOption, options || {});

            return this.each(function () {
                var $self = $(this);
                var optionSelected = $self.find("option:selected");
                var selectedValue = optionSelected.length ? optionSelected.text() : "";

                var selectionId = generateId($self);
                var $selectionLabel = $(getLabelHtml(selectedValue, selectionId));

                $selectionLabel.find(".selection").on("click", function (e) {
                    e.stopPropagation();
                    _methods.toggle.call($self, $(this), selectListOption.canSearch);
                });

                $(document).click(function () {
                    closeAll();
                });

                $self.hide().parent().append($selectionLabel);
            });
        },

        open: function (resultContainerId, canSearch) {
            closeAll();

            var $self = $(this);
            var $dropdown = getDropdownList.call(this, resultContainerId, canSearch);

            var $selection = $self.siblings(".select-list-container").find(".selection");
            var offset = $selection.offset();
            $dropdown.css({ top: (offset.top + 26) + 'px', left: offset.left + 'px', width: $selection.width() + 'px' });

            $("body").append($dropdown);
            $dropdown.find(".select-list-search-results").focus();
        },

        close: function(){
            var $this = $(this);
            var $selection = $this.siblings(".select-list-container").find(".selection");
            var resultContainerId = $selection.attr("aria-targetDropdown");
            $("#" + resultContainerId).remove();
        },

        toggle: function ($sourceElement, canSearch) {
            var resultContainerId = $sourceElement.attr("aria-targetDropdown");

            if (resultContainerId && resultContainerId.length > 0) {
                var $currentDropdown = $("#" + resultContainerId);
                var isShowing = $currentDropdown.length > 0;

                if (isShowing) {
                    $currentDropdown.remove();
                }
                else {
                    _methods.open.call(this, resultContainerId, canSearch);
                }
            }
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