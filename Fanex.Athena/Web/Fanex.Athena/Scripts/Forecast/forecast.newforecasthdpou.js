/*global Site, RefreshCountDown, QueryString*/
/// <reference path="/@JSense.js" />
"use strict";
var NewForecastHdpOu = (function ($) {
    return {
        initialize: function () {
            Site.unBlockUI();
            // Init timer count down

            RefreshCountDown.initialize();
            NewForecastHdpOu.initEvent();
            Site.addUserGuide({
                resourceName: "ForecastNewHdpOu",
                title: Site.getSingleResource("Forecast") + " " + Site.getSingleResource("HandicapOverUnderLive")
            });
        },
        initEvent: function () {
            // League change
            $('.league-list').on('change', function () {
                var leagueId = $(this).val();
                NewForecastHdpOu.loadMatchList(leagueId);
            });

            // Submit button click
            $("#SubmitButton").on("click", function () {
                NewForecastHdpOu.submit();
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
            var url = Site.resolveClientUrl("Forecast/GetMatchListByLeague");
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
                },
                type: "POST",
                async: true,
                dataType: "html"
            });
        },
        refreshMatchInfo: function () {
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
            var url = Site.resolveClientUrl("BetList/RunningBetListOfMatchChoice");
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
})(jQuery);