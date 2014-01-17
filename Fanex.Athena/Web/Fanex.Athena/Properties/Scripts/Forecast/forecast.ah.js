/// <reference path="/@JSense.js" />
var Forecast = (function ($) {
    var matchid, baseCurrency, licenseetype, currencyid;
    return {
        initialize: function () {
            // Init private variables            
            matchid = $('#forecast-table').attr('data-match-id');
            baseCurrency = $('#forecast-table').attr('data-basecurrency');
            licenseetype = $('#forecast-table').attr('data-licenseetype');
            currencyid = $('#forecast-table').attr('data-currencyid');

            // Init countdown
            RefreshCountDown.initialize();
            Forecast.initEvent();
        },
        initEvent: function () {
            // Forecast link click
            $('#Refresh-Content').on('click', '.forecast-link', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                var selectedScore = parseInt($(this).attr('data-current-score'));
                Forecast.loadForecast(selectedScore, bettype);
            });

            // View forecast click
            $('#Refresh-Content').on('click', '.view-forecast-link', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                Forecast.loadForecast(0, bettype);
            });

            // View betlist click
            $('#Refresh-Content').on('click', '.view-betlist', function () {
                var bettype = parseInt($(this).attr('data-bettype'));
                Forecast.viewBetlist(bettype);
            });
        },
        loadForecast: function (selectedScore, bettype) {
            var url = Site.resolveClientUrl("Forecast/ForeCastAh?matchid=" + matchid + '&bettype=' + bettype);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);
            url = QueryString.setParam(url, "LicenseeType", licenseetype);
            url = QueryString.setParam(url, "CurrencyId", currencyid);
            url = QueryString.setParam(url, "CurrentScore", selectedScore);

            $("#BetType").val(bettype);
            $("#CurrentScore").val(selectedScore);

            $.ajax({
                url: url,
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (data) {
                    Site.unBlockUI();
                    $('#Refresh-Content').html(data);
                    RefreshCountDown.initialize();
                    RefreshCountDown.initEvent();
                },
                type: "POST",
                async: true,
                dataType: "html"
            });
        },
        viewBetlist: function (bettype) {
            var currencyName = $("#CurrencyId option:selected").text();
            var url = Site.resolveClientUrl("BetList/RunningBetListOfMatch");
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);
            url = QueryString.setParam(url, "currencyid", currencyid);
            url = QueryString.setParam(url, "currencyname", currencyName);
            url = QueryString.setParam(url, "licenseetype", licenseetype);

            var popH = 650, popW = 900;

            ForecastPopup.resizePopup(popW, popH, url);
        }
    };
})(jQuery);

$(window).load(function () {
    parent.$('.ui-dialog').css({ 'width': 680 + 'px' });
    parent.$('.ui-dialog').css({ 'height': 250 + 'px' });
    parent.$('.ui-dialog').css({ 'position': 'absolute' });
    parent.$('.ui-dialog').css({ 'top': '25%' });
    parent.$('.ui-dialog').css({ 'left': '20%' });
    parent.$('.ui-dialog').css({ 'margin': 'auto' });
    parent.$('.ui-dialog-title').html(Site.getSingleResource("Forecast"));



    parent.$('#myframe').attr('width', 670);
    parent.$('#myframe').attr('height', 210);
    parent.$('#forecast-dialog').attr('style', 'width:660px;height:210px');
});
