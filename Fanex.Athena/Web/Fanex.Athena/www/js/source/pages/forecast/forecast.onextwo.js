define(['jquery', 'common/site', 'common/querystring', 'components/refreshcountdown', 'common/popup'], function ($, Site, QueryString, RefreshCountDown, Popup) {
	"use strict";

	var isPopup;

	var _me = {
		resizeGrid: function () {
			var $grid = $(".body-content");
			if ($grid.length === 0) {
				return;
			}

			var gridTop = $grid.offset().top;
			var gridHeight = window.innerHeight - gridTop - 20;
			gridHeight = Math.max(150, gridHeight);
			$grid.css("max-height", gridHeight);
		},
		initialize: function () {
		    Site.initialize();

		    isPopup = $('#IsPopUp').val() === "true";
		    if (isPopup) {
		        $("body").css("background", "#fff");
		        _me.resizePopup();
		    }

			_me.initEvent();
			_me.resizeGrid();
			Site.addUserGuide({
				resourceName: "Forecast1x2",
				title: Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("OneXTwo")
			});

			RefreshCountDown.initialize();
			RefreshCountDown.initEvent();

			Site.unBlockUI();
		},
		initEvent: function () {
			$('.league-list').on('change', function () {
				var leagueId = $(this).val();
				_me.loadSelectedLeague(leagueId);
			});

			$('#reportContent').on('click', '.button-add', function () {
				var matchId = $(this).attr('data-match-id');
				var betType = parseInt($(this).attr('data-bettype'));
				_me.openForecast(matchId, betType);
			});

			$('#SubmitButton').on('click', function () {
				_me.submit();
			});
		},
		openForecast: function (matchId, betType) {
			var url = Site.resolveClientUrl("RunningMatchBetList/Index");
			url = Site.setLicenseeOptionOriginalParam(url);
			url = QueryString.setParam(url, "matchid", matchId);
			url = QueryString.setParam(url, "bettype", betType);

			if (isPopup) {
				var popH = Math.min(parent.innerHeight - 100, 650), popW = 900;
				Popup.updateCurrentPopup(url, { width: popW, height: popH, title: Site.getSingleResource("BetList") });
			} else {
				Site.redirect(url);
			}
		},
		loadSelectedLeague: function (leagueId) {
			var url = Site.resolveClientUrl("OneXTwoForecast/Index");
			url = Site.setLicenseeOptionOriginalParam(url);
			url = QueryString.setParam(url, "LeagueId", leagueId);
			_me.submit();
		},
		submit: function () {
			Site.blockUI();
			$("#ForecastOneXTwoForm").submit();
		},
		resizePopup: function () {
		    var popH = Math.min(parent.innerHeight - 100, 600), popW = 700;
		    var title = Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("OneXTwo");
		    Popup.updateCurrentPopup(null, { width: popW, height: popH, title: title });
		}
	};

	return _me;
});