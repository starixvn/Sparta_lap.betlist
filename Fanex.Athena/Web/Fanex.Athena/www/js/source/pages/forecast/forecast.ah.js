define(['jquery', 'common/site', 'common/querystring', 'common/popup', 'components/refreshcountdown'], function ($, Site, QueryString, Popup, RefreshCountDown) {
    var matchid, baseCurrency, licenseetype, currencyid, currencyName;

    var _me = {
        initialize: function () {
            Site.initialize();
            _me.resizePopup();

            matchid = $('#forecast-table').attr('data-match-id');
            baseCurrency = $('#forecast-table').attr('data-basecurrency');
            licenseetype = $('#forecast-table').attr('data-licenseetype');
            currencyid = $('#forecast-table').attr('data-currencyid');
            currencyName = $('#forecast-table').attr('data-currencyname');

            _me.initEvent();
            RefreshCountDown.isForecastAH = true;
            RefreshCountDown.initialize();
            RefreshCountDown.initEvent();
        },
        initEvent: function () {
            $('#reportContent').on('click', '.forecast-link', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                var selectedScore = parseInt($(this).attr('data-current-score'));
                _me.loadForecast(selectedScore, bettype);
            });

            $('#reportContent').on('click', '.view-forecast-link', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                _me.loadForecast(null, bettype);
            });

            $('#reportContent').on('click', '.view-betlist', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                _me.viewBetlist(bettype);
            });

            $('#reportContent').on('keyup', '#txtCurrentscore', function (evt) {
                if (!evt) {
                    evt = window.event;
                }

                if (evt.keyCode === 13) {
                    $("#btnCurrentscore").click();
                }
            });

            $('#reportContent').on("click", '#btnCurrentscore', function (event) {
                event.preventDefault();
                _me.setCurrentScore();
            });
        },

        loadForecast: function (selectedScore, bettype) {
            var url = Site.resolveClientUrl("AhForecast/Index?matchid=" + matchid + '&bettype=' + bettype);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);
            url = QueryString.setParam(url, "LicenseeType", licenseetype);
            url = QueryString.setParam(url, "CurrencyId", currencyid);
            if (selectedScore !== null) {
                url = QueryString.setParam(url, "CurrentScore", selectedScore);
            }
            $("#BetType").val(bettype);
            $("#CurrentScore").val(selectedScore);

            $.ajax({
                url: url,
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (data) {
                    Site.unBlockUI();
                    $('#reportContent').html(data);
                    RefreshCountDown.initialize();
                    RefreshCountDown.initEvent();
                    $("#txtCurrentscore").focus();
                },
                type: "POST",
                async: true,
                dataType: "html"
            });
        },

        viewBetlist: function (bettype) {
            var url = Site.resolveClientUrl("RunningMatchBetList/Index");
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);
            url = QueryString.setParam(url, "currencyid", currencyid);
            url = QueryString.setParam(url, "currencyname", currencyName);
            url = QueryString.setParam(url, "licenseetype", licenseetype);

            var popH = Math.min(parent.innerHeight - 100, 700), popW = 960;

            Popup.updateCurrentPopup(url, { width: popW, height: popH, title: Site.getSingleResource("BetList") });
        },

        setCurrentScore: function () {
            var data = $("#txtCurrentscore").val();
            if (data === null || data === undefined || !Site.isInteger(data)) {
                return;
            }
            var bettype = $("#reportContent a.view-betlist").attr("data-bettype");
            _me.loadForecast(data, bettype);
        },

        resizePopup: function () {
            var popH = 250, popW = 700;
            Popup.updateCurrentPopup(null, { width: popW, height: popH, title: Site.getSingleResource("Forecast") });
        }
    };

    return _me;
});