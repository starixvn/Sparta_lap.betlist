define(['jquery', 'common/site', 'common/querystring', 'components/refreshcountdown'], function ($, Site, QueryString, RefreshCountDown) {
    var _me = {
        initialize: function () {
            Site.initialize();
            Site.unBlockUI();
            _me.initEvent();
            Site.addUserGuide({
                resourceName: "ForecastNewHdpOu",
                title: Site.getSingleResource("Forecast") + " " + Site.getSingleResource("HandicapOverUnderLive")
            });

            RefreshCountDown.initialize();
            RefreshCountDown.initEvent();
        },
        initEvent: function () {
            $('#DdlLeagueId').on('change', function () {
                var leagueId = $(this).val();
                _me.loadMatchList(leagueId);
            });

            $('.bet-list-link').on("click", function () {
                var $this = $(this);
                var betChoice = $this.data("betchoice");
                var param = $this.data("param");
                var bettype = $this.data("bettype");
                var handicap = $this.data("handicap");

                _me.openBetlist(betChoice, param, bettype, handicap);
            });

            $("#SubmitButton").on("click", function () {
                _me.submit();
            });
        },
        collapseExpand: function (context, statusElement) {
            var dataBlockId = $(context).attr("data-block-id");

            // is expanded
            if ($(context).hasClass('expand-icon')) {
                $('#content-area-' + dataBlockId).slideUp();
                $(context).removeClass('expand-icon');
                $(context).addClass('collapse-icon');
                $('#' + statusElement).val(false);
            }
            else {
                $('#content-area-' + dataBlockId).slideDown();
                $(context).removeClass('collapse-icon');
                $(context).addClass('expand-icon');
                $('#' + statusElement).val(true);
            }
        },
        loadMatchList: function (leagueId) {
            var url = Site.resolveClientUrl("NewHandicapOverUnderForecast/GetMatchListByLeague");
            url = QueryString.setParam(url, "LeagueId", leagueId);

            $.ajax({
                url: url,
                beforeSend: function () {
                    Site.blockUI();
                    $('#DdlMatchId').html('Loading...');
                },
                success: function (data) {
                    Site.unBlockUI();
                    $('#DdlMatchId').html(data);
                    var matchId = $('#DdlMatchId').val();
                    if (matchId === undefined) {
                        $('#SubmitButton').prop('disabled', true);
                    } else {
                        $('#SubmitButton').removeAttr('disabled');
                    }

                    $("#DdlMatchId").material_select(function() { // jshint ignore:line
                        $('input.select-dropdown').trigger('close');
                    });
                },
                type: "POST",
                async: true,
                dataType: "html"
            });
        },
        submit: function () {
            Site.blockUI();
            $('#LeagueId').val($('#DdlLeagueId').val());
            $('#MatchId').val($('#DdlMatchId').val());
            $('#IsShowFt').val(true);
            $('#IsShowFh').val(true);
            $('#NewForecastHdpOuForm').submit();
        },
        openBetlist: function (betChoice, param, betType, handicap) {
            var url = Site.resolveClientUrl("RunningMatchChoiceBetList/Index");
            url = QueryString.setParam(url, "betchoice", betChoice);
            url = QueryString.setParam(url, "matchid", param.MatchId);
            url = QueryString.setParam(url, "leagueid", param.LeagueId);
            url = QueryString.setParam(url, "CurrencyId", param.CurrencyId);
            url = QueryString.setParam(url, "CurrencyName", param.CurrencyName);
            url = QueryString.setParam(url, "BaseCurrency", param.BaseCurrency);
            url = QueryString.setParam(url, "bettype", betType);
            url = QueryString.setParam(url, "handicap", handicap === undefined ? "" : handicap);

            var popH = $(window).height(), popW = $(window).width();
            var formatter = new Array(
                "resizable=no",
                "scrollbars=yes",
                "toolbar=no",
                "menubar=no",
                "location=no",
                "addressbar=no",
                "directories=no",
                "status=yes",
                "top=0",
                "left=0",
                "height=" + popH,
                "width=" + popW
            ).join(',');

            window.open(url, '_blank', '' + ',' + formatter);
        }
    };

    return _me;
});