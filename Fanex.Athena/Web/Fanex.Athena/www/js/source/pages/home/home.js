"use strict";

define(["materialize-autocomplete"], function () {
    var rootUrl;
    var $searchBox;

    $(initialize);

    function initialize() {
        initVariables();
        initEvent();
    }

    function initVariables() {
        $searchBox = $('#searchbox');
        rootUrl = $('#RootUrl').val();
    }

    function initEvent() {
        $searchBox.keyup(function (e) {
            if (!e) {
                e = window.event;
            }
            var key = (e.keyCode) ? e.keyCode : e.which;
            if (key === 13) {
                searchByUserName();
                return false;
            }
        });

        initAutoComplete();
    }

    function searchByUserName() {
        var userName = $searchBox.val();
        userName = null == userName ? "" : userName;

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
});