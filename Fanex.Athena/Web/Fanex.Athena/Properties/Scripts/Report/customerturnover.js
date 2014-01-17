/// <reference path="/@JSense.js" />
var CustTurnover = (function ($) {
    return {
        defaultUserName: Site.getSingleResource('ExternalIdOrSystemId'),
        initialize: function () {
            CustTurnover.initEvent();
            CustTurnover.setDefaultUsername();
            CustTurnover.initAutoComplete();
            CustTurnover.setDefaultUsername();
        },

        initEvent: function () {

            $("#SubmitButton").click(function () {
                $('#IsExportExcel').val(false);
                CustTurnover.submitForm();
                return false;
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                CustTurnover.submitForm();
                Site.unBlockUI();
                return false;
            });

            $("#TickerFilter input[type=checkbox]").checkboxgroup();

            $("#Username").blur(function () {
                var username = $('#Username').val();
                if (username == '') {
                    $('#NullUsername').css('display', 'inline');
                };

                CustTurnover.setDefaultUsername();
            });

            $('#Username').focus(function () {
                CustTurnover.setUsernameOnFocus();
            });

            //Odds event
            $("#Odds").blur(function () {
                var odds = $('#Odds').val();
                if (odds == '') {
                    $('#NullOdds').css('display', 'inline');
                };
            });

            $("#Odds").keypress(function (event) {
                /*--------------------------------------------------------------
                Backspace, tab, enter, end, home, left, right
                Don't support the del key in Opera because del == . == 46.
                --------------------------------------------------------------*/
                var controlKeys = [8, 9, 13, 35, 36, 37, 39, 46];

                /*--------------------------------------------------------------
                IE doesn't support indexOf
                --------------------------------------------------------------*/
                var isControlKey = controlKeys.join(",").match(new RegExp(event.which));

                /*--------------------------------------------------------------
                Some browsers just don't raise events for control keys. Easy.
                e.g. Safari backspace.
                --------------------------------------------------------------*/
                if (!event.which ||
                // Control keys in most browsers. e.g. Firefox tab is 0
                 (48 <= event.which && event.which <= 57) ||
                // Always 0 through 9
                //(48 == event.which && $(this).attr("value")) ||   // No 0 first digit
                    isControlKey) {
                    // Opera assigns values for control keys.
                    return;
                }
                else {
                    event.preventDefault();
                }
            });
        },
        initAutoComplete: function () {
            var txtUserName = $('#Username');
            var serUrl = resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({
                    serviceUrl: serUrl,
                    minChars: 2
                });
            }
        },
        submitForm: function () {
            var username = $('#Username').val();
            var odds = $('#Odds').val();
            var isValid = true;
            if (username == '' || username == CustTurnover.defaultUserName) {
                $('#NullUsername').css('display', 'inline');
                $('#Username').val('');
                isValid = false;
            };

            if (odds == '') {
                $('#NullOdds').css('display', 'inline');
                isValid = false;
            };

            if (isValid) {
                Site.blockUI();
                $("#CustomerTurnoverForm").submit();
            };
        },
        setDefaultUsername: function () {
            var username = $('#Username').val();
            if (username == '' || username == CustTurnover.defaultUserName) {
                $('#Username').val(CustTurnover.defaultUserName);
                $('#Username').attr('class', 'default-username');
            }
            else {
                $('#Username').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#Username').val();
            if (username == CustTurnover.defaultUserName) {
                $('#Username').val('');
                $('#Username').removeAttr('class');
            }
        }
    };
})(jQuery);

$(window).load(function () {
    CustTurnover.initialize();
});