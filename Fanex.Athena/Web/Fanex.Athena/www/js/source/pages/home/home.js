"use strict";

define(["site", "jquerycookie", "jclock", "materialize-autocomplete"], function (Site) {
    var rootUrl;
    var $searchBox;
    var _autocomplete;

    $(initialize);

    function initialize() {
        //initClock();
        initVariables();
        initEvent();
    }

    function initVariables() {
        $searchBox = $('#searchbox');
        rootUrl = $('#RootUrl').val();
    }

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

        $('#languagesite').find('.languageItem').click(function () {
            changeLanguage($(this));
        });

        $('#ddlLicensee').find('.licenseeItem').click(function () {
            changeLicensee($(this));
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

        $(".toggleMenubar").sideNav2();
    }

    function searchByUserName() {
        var userName = $searchBox.val();
        if (!userName) {
            return;
        }

        var url = rootUrl + "Customer/CustomerList?userName=" + userName;
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

    function changeLicensee($target) {
        var licenseeId = $target.attr('licenseeId');
        top.frames["main"].location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
    }
});