define(['jquery', 'common/site', 'common/querystring', 'common/popup', 'components/refreshcountdown'],
    function ($, Site, QueryString, Popup, RefreshCountDown) {
        "use strict";

        var _matchid, _baseCurrency, _currencyid, _currencyName;

        var _me = {
            initialize: function () {
                Site.initialize();
                _me.resizePopup();

                _matchid = $('#hidden-data').attr('data-match-id');
                _baseCurrency = $('#hidden-data').attr('data-basecurrency');
                _currencyid = $('#hidden-data').attr('data-currencyid');
                _currencyName = $('#hidden-data').attr('data-currencyname');

                _me.initEvent();

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
                var url = Site.resolveClientUrl("AhForecast/Index?matchid=" + _matchid + '&bettype=' + bettype);
                url = QueryString.setParam(url, "basecurrency", _baseCurrency);
                url = QueryString.setParam(url, "CurrencyId", _currencyid);
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
                url = QueryString.setParam(url, "matchid", _matchid);
                url = QueryString.setParam(url, "bettype", bettype);
                url = QueryString.setParam(url, "basecurrency", _baseCurrency);
                url = QueryString.setParam(url, "currencyid", _currencyid);
                url = QueryString.setParam(url, "currencyname", _currencyName);

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