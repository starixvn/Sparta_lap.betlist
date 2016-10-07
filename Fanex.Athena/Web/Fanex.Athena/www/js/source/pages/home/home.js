"use strict";

define([], function () {
    var rootUrl;
    var $searchBox;

    initialize();

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
            else {
                suggestCustomer(e.target.value);
            }
        });     
    }

    function searchByUserName() {
        var userName = $searchBox.val();
        userName = null == userName ? "" : userName;

        var url = rootUrl + "Customer/CustomerList?userName=" + userName;
        top.main.location.href = url;
    }

    function suggestCustomer(query) {
        var url = rootUrl + "CustomerAPI/SearchCustomer?licenseeType=&currencyId=0&query=" + query;

        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: function (response) {
                var suggestions = response.suggestions;

                if (suggestions && suggestions.length > 0) {
                    var $suggestionContent = $('.autocomplete-content');
                    $suggestionContent.empty();

                    suggestions.map(function (item) {
                        $suggestionContent.append('<li><span><span class="highlight">' + query + '</span>' + item.substring(item.indexOf(query)) + '</span></li>');
                    });

                    $searchBox.autocomplete({
                        data: {
                            "Apple": null,
                            "Microsoft": null,
                            "Google": 'http://placehold.it/250x250'
                        }
                    });
                }
            }
        });
    }
});