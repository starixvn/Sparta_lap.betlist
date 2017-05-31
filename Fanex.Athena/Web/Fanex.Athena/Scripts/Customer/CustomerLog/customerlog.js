/*global Site*/
/*exported CustomerLog*/
/// <reference path="/@JSense.js" />
"use strict";
var CustomerLog = (function ($) {
    return {
        initAutoComplete: function () {
            var txtUserName = $('#Username');
            var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({
                    serviceUrl: serUrl,
                    minChars: 2
                });
            }
        },
        dateConstraint: function () {
            var fromDay = parseInt($('#MonthPickerFromDay .selectedDay').html());
            var toDay = parseInt($('#MonthPickerToDay .selectedDay').html());
            return toDay >= fromDay;
        },
        setDefaultUsername: function () {
            var username = $('#Username').val();
            if (username === '' || username === Site.getSingleResource("ExternalIdOrSystemId")) {
                $('#Username').val(Site.getSingleResource("ExternalIdOrSystemId"));
                $('#Username').attr('class', 'default-username');
            }
            else {
                $('#Username').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#Username').val();
            if (username === Site.getSingleResource("ExternalIdOrSystemId")) {
                $('#Username').val('');
                $('#Username').removeAttr('class');
            }
        },
        checkShowAll: function () {
            $('#showall').is(':checked') ? $('.old-value').show() : $('.old-value').hide();
        }
    };
})(jQuery);