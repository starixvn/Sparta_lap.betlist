/// <reference path="/@JSense.js" />
var ResetPassword = (function ($) {
    return {
        init: function () {
            $('#btnSave').click(function () {
                $('#resultMessage').hide();

                // check password
                var validPassword = false;
                if ($('#Password').val().length <= 0) {
                    $('#rfvPassword').show();
                } else {
                    validPassword = true;
                    $('#rfvPassword').hide();
                }

                // check confirm password
                var validConfirm = false;
                if ($('#ConfirmPassword').val().length <= 0) {
                    $('#rfvConfirm').show();
                } else {
                    $('#rfvConfirm').hide();
                    validConfirm = true;
                }

                // compare two fields
                var validClient = false;
                if (validPassword && validConfirm) {
                    if ($('#Password').val() !== $('#ConfirmPassword').val()) {
                        $('#cvConfirm').show();
                    }
                    else {
                        $('#cvConfirm').hide();
                        validClient = true;
                    }
                }

                if (validClient) {
                    var form = $('#ResetPasswordForm');
                    var url = form.attr('action');
                    var params = form.serialize();

                    $.ajax({
                        type: 'POST',
                        dataType: "json",
                        cache: false,
                        url: url,
                        data: params,
                        success: function (response) {
                            if (response.IsSuccessful === false) {
                                $('#resultMessage').removeClass('info');
                                $('#resultMessage').addClass('err');
                            }
                            else {
                                $('#resultMessage').removeClass('err');
                                $('#resultMessage').addClass('info');
                            }

                            $('#resultMessage').html(response.Message).show();
                            $('#resultMessage').animate({ "opacity": "hidden" }, 10000, function () { $(this).fadeOut(1000); });
                        },
                        error: function () {
                        },
                    });
                }
            });

            $('#btnCancel').click(function () {
                parent.$('#manage-dialog').dialog('close');
            });
        }
    };
} (jQuery));

$(document).ready(function () {
    ResetPassword.init();
});