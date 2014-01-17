/// <reference path="/@JSense.js" />
var RefreshCountDown = (function ($) {
    var curCountDown = 0;
    var countDownTimer = null;
    var cookieName = null;
    var countDownId = null;
    var defaultCountDown = 0;
    return {
        initialize: function () {
            // Open count down menu
            $('.button-select').on('click', function () {
                RefreshCountDown.initMenu();
            });

            // Select countdown
            $('.menu-item').on('click', function () {
                var selectedCountDown = parseInt($(this).attr('data-count-down'));
                RefreshCountDown.setCountDown(selectedCountDown, true);
            });

            // Refresh button click
            $('.button-refresh').on('click', function () {
                RefreshCountDown.refresh();
            });

            // Get count down id from filter
            countDownId = $('#count-down-of').attr('name');

            // Get default count down
            defaultCountDown = parseInt($('#count-down-of').attr('data-default-value'));
        },
        initEvent: function () {
            cookieName = "RefeshCountDown" + countDownId;
            //read countdown from cookie
            var countDownFromCookie = jQuery.cookie(cookieName);
            if (countDownFromCookie && countDownFromCookie != 0) {
                RefreshCountDown.setCountDown(countDownFromCookie, false);
            } else {
                RefreshCountDown.setCountDown(defaultCountDown, true);
            }
        },
        initMenu: function () {
            var isVisible = $('#countdown-menu').css('display') != 'none';
            if (isVisible) {
                $('#countdown-menu').hide();
            }
            else {
                $('#countdown-menu').show();
            }
        },

        setCountDown: function (countDown, isSetToCookie) {
            var countDownInt = parseInt(countDown);
            if (isSetToCookie) {
                jQuery.cookie(cookieName, countDownInt);
            }
            curCountDown = countDownInt;
            RefreshCountDown.setMenuStatus();
            $('#countdown-menu').hide();
            RefreshCountDown.tick();
        },
        setMenuStatus: function () {
            var index = curCountDown;
            $('#countdown-menu div').removeClass('menu-selected');
            $('#option-' + index).addClass('menu-selected');
        },
        tick: function () {
            document.getElementById('count').innerHTML = curCountDown;
            curCountDown--;
            if (countDownTimer != null) clearTimeout(countDownTimer);
            countDownTimer = setTimeout(RefreshCountDown.tick, 1000);
            if (curCountDown < -1) {
                RefreshCountDown.refresh();
            }
        },
        refresh: function () {
            var oldScrollTop;
            if ($(".data-content").length) {
                oldScrollTop = $(".data-content")[0].scrollTop;
            }

            if (countDownTimer != null) clearTimeout(countDownTimer);
            document.getElementById('count').innerHTML = "<span class='timer-loading'>loading</span>";

            // Check popup
            var isPopUp = $('#IsPopUp').val();

            // Get param before load data
            var url = RefreshCountDown.calculateUrl();

            // Set up ajax no cache
            $.ajaxSetup({ cache: false });

            $('#Refresh-Content').load(url, { noncache: new Date().getTime() }, function () {
                Site.unBlockUI();
                RefreshCountDown.initEvent();

                // Remove white space between html tag (fix bug wrong ui displaying on IE9).
                var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
                $('#Refresh-Content').html($('#Refresh-Content').html().replace(expr, '><'));

                // Currency constraint
                Site.currencyConstraint();

                // Re init countdown event with forecast AH
                if (typeof (Forecast) !== 'undefined') {
                    RefreshCountDown.initialize();
                }

                // Scrollbar still kept current position
                if ($(".data-content").length) {
                    $(".data-content").scrollTop(oldScrollTop);
                }
            });

            return true;
        },
        calculateUrl: function () {

            var leagueId = $('#LeagueId').val();
            var betType = $("#BetType").val();
            var currentScore = $("#CurrentScore").val();

            var url = location.href;
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "LeagueId", leagueId);
            url = QueryString.setParam(url, "BetType", betType);
            url = QueryString.setParam(url, "CurrentScore", currentScore);
            
            return url;
        }
    };
})(jQuery);
