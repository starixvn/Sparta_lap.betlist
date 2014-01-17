/// <reference path="/@JSense.js" />
var ForecastPopup = (function ($) {
    return {
        showPopup: function (width, height, url) {
            $('#forecast-dialog').remove();
            var title = Site.getSingleResource("Forecast");
            var dialogForm = '<div title="' + title + '" id="forecast-dialog"><iframe id="myframe" frameborder="0" width="100%" height="100%" src="' + url + '" ></iframe></div>';
            $(dialogForm).dialog({
                modal: true,
                width: width,
                height: height,
                resizable: true
            });
            $('#forecast-dialog').css({ 'overflow': 'hidden' });
        },
        resizePopup: function (width, height, url) {
            parent.$('.ui-dialog').css({ 'width': width + 25 + 'px' });
            parent.$('.ui-dialog').css({ 'height': height + 50 + 'px' });
            parent.$('.ui-dialog').css({ 'top': '50px' });
            parent.$('.ui-dialog').css({ 'left': '50px' });

            parent.$('.ui-dialog-title').html(Site.getSingleResource('BetList'))

            parent.$('#myframe').attr('width', '100%');
            parent.$('#myframe').attr('height', '100%');
            parent.$('#forecast-dialog').attr('style', 'width:' + width + 'px;height:' + height + 'px');
            parent.$('#forecast-dialog').css({ 'overflow': 'hidden' });
            window.location.href = url;
        }
    };
} (jQuery));