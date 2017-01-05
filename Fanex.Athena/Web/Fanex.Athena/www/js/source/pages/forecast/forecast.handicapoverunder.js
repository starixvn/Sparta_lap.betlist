define(['jquery', 'common/site', 'common/querystring', 'components/refreshcountdown', 'common/popup', 'extensions/jquery.extension'],
    function ($, Site, QueryString, RefreshCountDown, Popup) {
        "use strict";
        var _me = {
            resizeGrid: function () {
                var $grid = $(".body-content");
                if ($grid.length === 0) {
                    return;
                }

                var gridTop = $grid.offset().top;

                var gridHeight = window.innerHeight - gridTop - 30;
                gridHeight = Math.max(150, gridHeight);
                gridHeight = Math.min($grid.height(), gridHeight);
                $grid.css("height", gridHeight);

                if (!$grid.hasVerticalScrollBar()) {
                    $grid.addClass("hasScroll");
                }
                else {
                    $grid.find("table").width($grid.width() - 17);
                }
            },
            initialize: function () {
                Site.initialize();
                Site.unBlockUI();
                RefreshCountDown.initialize();
                RefreshCountDown.initEvent();

                _me.initEvent();
                _me.resizeGrid();
                Site.addUserGuide({
                    resourceName: "ForecastHdpOu",
                    title: Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("HandicapOverUnderLive")
                });
            },
            initEvent: function () {
                $('.league-list').on('change', function () {
                    var leagueId = $(this).val();
                    _me.loadSelectedLeague(leagueId);
                    return false;
                });

                $('#reportContent').on('click', '.button-add', function () {
                    var matchId = $(this).attr('data-match-id');
                    var betType = parseInt($(this).attr('data-bettype'));
                    _me.openForecast(matchId, betType);
                });

                $("#SubmitButton").on("click", function () {
                    _me.submit();
                    return false;
                });

                $(window).on("resize", function () {
                    _me.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },
            openForecast: function (matchId, betType) {
                var url = Site.resolveClientUrl("AhForecast/Index?matchid=" + matchId + '&bettype=' + betType);
                url = Site.setLicenseeOptionOriginalParam(url);

                var popH = 250, popW = 700;

                Popup.openPopupWithIframe(url, { width: popW, height: popH, title: Site.getSingleResource("Forecast"), id: "forecast-dialog" });
            },
            loadData: function(url, data){
                $.ajax({
                    url: url || Site.resolveClientUrl("HandicapOverUnderForecast/Index"),
                    data: data,
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    success: function (data) {
                        $('#reportContent').html(data);
                        _me.resizeGrid();
                        Site.unBlockUI();
                    },
                    type: "POST",
                    async: true,
                    dataType: "html"
                });
            },
            loadSelectedLeague: function (leagueId) {
                var url = Site.resolveClientUrl("HandicapOverUnderForecast/Index");
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "LeagueId", leagueId);

                _me.loadData(url);
            },
            submit: function () {
                Site.blockUI();
                var data = $('#ForecastHandicapOverUnderForm').serializeObject();
                Site.updateOriginalLicenseeOption();

                _me.loadData(null, data);
            }
        };

        return _me;
    });