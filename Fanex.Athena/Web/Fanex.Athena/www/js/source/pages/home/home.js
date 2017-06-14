"use strict";

define([
    "common/site",
    "./recheckin",
    "lib/jquery.cookie",
    "lib/jquery.marquee",
    "materialize-autocomplete",
    "components/selectlist"],
    function (Site, Recheckin) {
        var $searchBox;
        var _autocomplete;

        function initialize() {
            Site.initialize();
            initVariables();
            initEvent();
            checkMoveBetNotification();
            activeDefaultMenuItem();
            setTimeout(function () {
                var $mainFrame = $("#mainFrame");
                $mainFrame.attr('src', $mainFrame.data('src'));
            }, 100);

            if ($('#HasRecheckIn').val() === "1") {
                Recheckin.init();
            }
        }

        function initVariables() {
            $searchBox = $('#searchbox');
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

            $('#languagesite').on('click', '.languageItem', function () {
                changeLanguage($(this));
            });

            $("#ddlLicensee")
                .selectList({ canSearch: true })
                .on('change', function () {
                    var licenseeId = $(this).val();
                    top.location.href = Site.resolveClientUrl("Header/ChangeLicensee?licenseeid=") + licenseeId + "&nocache=" + (new Date().getTime());
                });

            $('#label-search').click(function () {
                $(this).parent().addClass('focused');
                $searchBox.focus();
                searchByUserName();
            });

            $searchBox.blur(function () {
                if (!$(this).val()) {
                    $(this).parent().removeClass('focused');
                }
            });

            $('html').click(function () {
                _autocomplete.$dropdown.html('').hide();
            });

            $('.collapsible2').on("click.menuevent", ".collapsible-body  a", function () {
                var $currentMenuItem = $(this);
                document.title = 'LRF' + ' - ' + $(".active > .menu-title").text() + ' - ' + $currentMenuItem.text();

                var isCancelledBetListMenu = $currentMenuItem.prop("id") === "MenuItem36";
                if (isCancelledBetListMenu) {
                    $("#page-title").text(Site.getSingleResource("CancelledBetsTitle"));
                    return;
                }
                $("#page-title").text($currentMenuItem.text());
            });

            $(window).on('blur', function () {
                $('.dropdown-button').dropdown('close');
                $('#ddlLicensee').selectList("close");
                _autocomplete.$dropdown.html('').hide();
            });
        }

        function searchByUserName() {
            var userName = $searchBox.val();
            if (!userName || userName === "") {
                return;
            }

            var url = Site.resolveClientUrl("Customer/CustomerList?userName=" + userName);
            top.main.location.href = url;
            $('.collapsible2').collapsibleMenu('activeMenuItem', $('#MenuItem43'));
        }

        function initAutoComplete() {
            _autocomplete = $searchBox.materialize_autocomplete({ // jshint ignore:line
                ignoreCase: false,
                multiple: {
                    enable: false
                },
                dropdown: {
                    el: '#suggestionDropdown'
                },
                getData: function (value, callback) {
                    var url = Site.resolveClientUrl("CustomerAPI/SearchCustomer?query=" + value);

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

        function checkMoveBetNotification() {
            var $marquee = $("#notification-marquee");

            if ($marquee.length === 0) {
                return;
            }

            var previousText = "";
            var $parent = $marquee.parent();
            var apiUrl = Site.resolveClientUrl("api/CommonApi/GetMoveBetNotification");
            function processNotification() {
                $.get(apiUrl, function (message) {
                    if (message === previousText) {
                        return;
                    }

                    previousText = message;
                    if (message) {
                        if ($parent.is(":hidden")) {
                            $parent.slideDown('fast');
                            $('#mainFrame').css('height', 'calc(100% - 30px)');
                        }

                        if ("marquee" in $marquee) {
                            $marquee.text(message);
                            $marquee.marquee({ speed: 30 });
                        }
                    } else {
                        $marquee.marquee("stop");
                        $parent.slideUp('fast');
                        $('#mainFrame').css('height', '100%');
                    }
                });
            }

            processNotification();
            setInterval(processNotification, 60000);
        }

        return {
            initialize: initialize
        };
    });