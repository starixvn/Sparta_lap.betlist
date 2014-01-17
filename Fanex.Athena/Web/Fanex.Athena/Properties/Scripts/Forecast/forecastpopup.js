/// <reference path="/@JSense.js" />
var ForecastPopup = (function ($) {
    return {
        showPopup: function (width, height, url) {
            $('#forecast-dialog').remove();
            var title = Site.getSingleResource("Forecast");
            var dialogForm = '<div title="' + title + '" id="forecast-dialog"><iframe id="myframe" frameborder="0" width="' + width + '" height="' + height + '" src="' + url + '" ></iframe></div>';
            $(dialogForm).dialog({
                modal: true,
                width: width + 15,
                height: height + 50
            });

        },
        resizePopup: function (width, height, url) {
            parent.$('.ui-dialog').css({ 'width': width + 15 + 'px' });
            parent.$('.ui-dialog').css({ 'height': height + 50 + 'px' });
            parent.$('.ui-dialog').css({ 'top': '50px' });
            parent.$('.ui-dialog').css({ 'left': '50px' });
            parent.$('.ui-dialog-title').html(Site.getSingleResource('BetList'))

            parent.$('#myframe').attr('width', width);
            parent.$('#myframe').attr('height', height);
            parent.$('#forecast-dialog').attr('style', 'width:' + width + 'px;height:' + height + 'px');
            window.location.href = url;
        }
    };
} (jQuery));
