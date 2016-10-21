"use strict";

define(["common/site", "lib/jquery.cookie", "lib/jquery.jclock", "materialize-autocomplete"], function (Site) {
    var $searchBox;
    var _autocomplete;

    
    function initialize() {
        Site.initialize();    
        //initClock();
        initVariables();
        initEvent();
        activeDefaultMenuItem();
    }
   
    function initVariables() {
        $searchBox = $('#searchbox');
    };

    function initEvent() {
        initAutoComplete();

        $searchBox.keyup(function (e) {
            e = e || window.event;
            var key = e.keyCode || e.which;

            if (key === 13) {
                searchByUserName();
                _autocomplete.$dropdown.html('').hide();
            }
        });


        $('#languagesite').on('click', '.languageItem', function () {
            changeLanguage($(this));
        });

        $('#ddlLicensee').on('click', '.licenseeItem', function () {
            var licenseeId = $(this).attr('licenseeId');
            top.location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
        });

        $('#label-search').click(function () {
            $(this).parent().addClass('focused');
            $("#searchbox").focus();
        });

        $("#searchbox").blur(function () {
            if (!$(this).val()) {
                $(this).parent().removeClass('focused');
            }
        });

        $('body').click(function () {
            _autocomplete.$dropdown.html('').hide();
        });

        $('.collapsible2').on("click.menuevent", ".collapsible-body  a", function (e) {
            document.title = 'LRF' + ' - ' + $(".active > .menu-title").text() + ' - ' + $(this).text();
            $("#page-title").text($(this).text());
        });
    }

    function searchByUserName() {
        var userName = $searchBox.val();
        if (!userName) {
            return;
        }

        var url = Site.resolveClientUrl("Customer/CustomerList?userName=" + userName);
        top.main.location.href = url;
        $('.collapsible2').collapsibleMenu('activeMenuItem', $('#MenuItem43'));
    }

    function initAutoComplete() {
        _autocomplete = $searchBox.materialize_autocomplete({
            ignoreCase: false,
            multiple: {
                enable: false
            },
            dropdown: {
                el: '#suggestionDropdown'
            },
            getData: function (value, callback) {
                var url = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0&query=" + value);

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
    }

    function initClock() {
        var options = {
            utc: true,
            utcOffset: -4,
            format: '%H:%M:%S %b %d, %Y GMT'
        };
        $('#current-time-clock').jclock(options);
    }

    function changeLanguage($target) {
        var lang = $target.attr('langkey');
        var mainFrame = top.frames["main"];
        if (mainFrame && mainFrame.Site && mainFrame.Site.blockUI) {
            mainFrame.Site.blockUI();
        }

        $.ajax({
            url: Site.resolveClientUrl("Header/ChangeLanguage?languagesite=") + lang,
            async: false,
            type: "GET",
            cache: false,
            success: function (result) {
                if (result === "1") {
                    if ($.cookie("isClosedNotification") !== "1") {
                        $.cookie('isFirstLoad', '1', { path: '/' });
                    }
                    top.location.reload();
                }
            }
        });
    }

    function activeDefaultMenuItem() {
        var defaultUrl = $("#DefaultUrl").val();
        var $menuItem = $("#left-menu .collapsible-body").find("a[href='" + defaultUrl + "']");
        $('.collapsible2').collapsibleMenu('activeMenuItem', $menuItem);

        var menuItemName = $menuItem.length > 0 ? $menuItem.text() : "";
        $("#page-title").text(menuItemName);

        document.title = 'LRF' + ' - ' + $(".active > .menu-title").text() + ' - ' + menuItemName;
    }

    return {
        initialize: initialize
    };
});