/// <reference path="/@JSense.js" />
var EditAccount = (function ($) {
    return {
        init: function () {
            $('#btnSave').click(function () {
                $('#resultMessage').hide();

                    var form = $('#EditAccountForm');
                    var url = form.attr('action');
                    var params = form.serialize();
                    var status = 'Open';
                    if($('#rdoClosed').is(':checked'))
                    {
                        status = $('#rdoClosed').val();
                    }

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

                                // update old info
                                $('#OldFirstName').val($('#FirstName').val());
                                $('#OldLastName').val($('#LastName').val());
                                $('#OldStatus').val(status);
                            }

                            $('#resultMessage').html(response.Message).show();
                            $('#resultMessage').animate({ "opacity": "hidden" }, 10000, function () { $(this).fadeOut(1000); });
                        },
                        error: function () {
                        },
                    });
            });

            $('#btnReset').click(function () {
                $('#FirstName').attr('value', $('#OldFirstName').val());
                $('#LastName').attr('value', $('#OldLastName').val());
                var oldStatus = $('#OldStatus').val();

                if(oldStatus === 'Open')
                {
                    $('#rdoClosed').removeAttr('checked');
                    $('#rdoOpen').attr('checked', 'checked');
                }
                else
                {
                    $('#rdoOpen').removeAttr('checked');
                    $('#rdoClosed').attr('checked', 'checked');
                }
            });
        }
    };
} (jQuery));

$(document).ready(function () {
    EditAccount.init();
});