/// <reference path="/@JSense.js" />
"use strict";
var EditAccount = (function ($) {
    return {
        init: function () {
            $('#btnSave').click(function () {
                $('#resultMessage').hide();

                    var form = $('#EditAccountForm');
                    var url = form.attr('action');
                    var params = form.serializeObject();
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
                            var $resultMessage = $('#resultMessage');

                            $resultMessage.toggleClass('info', response.IsSuccessful)
                                          .toggleClass('err', !response.IsSuccessful);

                            if (response.IsSuccessful) {
                                // update old info
                                $('#OldFirstName').val($('#FirstName').val());
                                $('#OldLastName').val($('#LastName').val());
                                $('#OldStatus').val(status);
                            }

                            $resultMessage.html(response.Message).show()
                                          .animate({ "opacity": "hidden" }, 10000, function () { $(this).fadeOut(1000); });
                        }
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