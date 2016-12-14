define(['jquery', 'common/site', 'common/querystring', 'lib/jquery.cookie'], function ($, Site, QueryString) {
    var curCountDown = 0;
    var countDownTimer = null;
    var cookieName = null;
    var countDownId = null;
    var defaultCountDown = 0;

    var _me = {
        initialize: function () {
            // Open count down menu
            $('.button-select').on('click', function () {
                _me.initMenu();
            });

            // Select countdown
            $('.menu-item').on('click', function () {
                var selectedCountDown = parseInt($(this).attr('data-count-down'));
                _me.setCountDown(selectedCountDown, true);
            });

            // Refresh button click
            $('.button-refresh').on('click', function () {
                _me.refresh();
            });

            // Get count down id from filter
            countDownId = $('#count-down-of').attr('name');

            // Get default count down
            defaultCountDown = parseInt($('#count-down-of').attr('data-default-value'));
        },
        initEvent: function () {
            cookieName = "RefeshCountDown" + countDownId;
            //read countdown from cookie
            var countDownFromCookie = $.cookie(cookieName);
            if (countDownFromCookie && Number(countDownFromCookie) !== 0) {
                _me.setCountDown(countDownFromCookie, false);
            } else {
                _me.setCountDown(defaultCountDown, true);
            }
        },
        initMenu: function () {
            var isVisible = $('#countdown-menu').css('display') !== 'none';
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
                $.cookie(cookieName, countDownInt);
            }
            curCountDown = countDownInt;
            _me.setMenuStatus();
            $('#countdown-menu').hide();
            _me.tick();
        },
        setMenuStatus: function () {
            var index = curCountDown;
            $('#countdown-menu div').removeClass('menu-selected');
            $('#option-' + index).addClass('menu-selected');
        },
        tick: function () {
            document.getElementById('count').innerHTML = curCountDown;
            curCountDown--;
            if (countDownTimer != null) {
                clearTimeout(countDownTimer);
            }
            countDownTimer = setTimeout(_me.tick, 1000);
            if (curCountDown < -1) {
                _me.refresh();
            }
        },
        refresh: function () {
            var oldScrollTop;
            if ($(".data-content").length) {
                oldScrollTop = $(".data-content")[0].scrollTop;
            }

            if (countDownTimer != null) {
                clearTimeout(countDownTimer);
            }
            document.getElementById('count').innerHTML = "<span class='timer-loading'>loading</span>";

            // Get param before load data
            var url = _me.calculateUrl();

            // Set up ajax no cache
            $.ajaxSetup({ cache: false });

            $('#Refresh-Content').load(url, { noncache: new Date().getTime() }, function () {
                Site.unBlockUI();
                _me.initEvent();

                // Remove white space between html tag (fix bug wrong ui displaying on IE9).
                var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
                $('#Refresh-Content').html($('#Refresh-Content').html().replace(expr, '><'));

                // Currency constraint
                Site.currencyConstraint();

                // Re init countdown event with forecast AH
                if (typeof (Forecast) !== 'undefined') {
                    _me.initialize();
                }

                // Scrollbar still kept current position
                if ($(".data-content").length) {
                    $(".data-content").scrollTop(oldScrollTop);
                }
                $(window).resize();
            });

            return true;
        },
        calculateUrl: function () {
            var leagueId = $('#LeagueId').val();
            var matchId = $('#MatchId').val();
            var betType = $("#BetType").val();
            var currentScore = $("#CurrentScore").val();
            var isShowFt = $("#IsShowFt").val();
            var isShowFh = $("#IsShowFh").val();
            var homeName = $("#HomeTeamId").val();
            var awayName = $("#AwayTeamId").val();

            var url = location.href;
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "LeagueId", leagueId);
            url = QueryString.setParam(url, "MatchId", matchId);
            url = QueryString.setParam(url, "BetType", betType);
            url = QueryString.setParam(url, "CurrentScore", currentScore);
            url = QueryString.setParam(url, "IsShowFt", isShowFt);
            url = QueryString.setParam(url, "IsShowFh", isShowFh);

            url = QueryString.setParam(url, "HomeTeamId", homeName);
            url = QueryString.setParam(url, "AwayTeamId", awayName);

            return url;
        }
    };

    return _me;
});