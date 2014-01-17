/// <reference path="/@JSense.js" />
var UpdateGroup = (function ($) {
    return {
        initialize: function () {
            UpdateGroup.initEvent();

            // Check commission first load
            UpdateGroup.checkCommFirstLoad();
        },
        checkCommFirstLoad: function () {
            GroupComm.commissionOnChange($('#DdlSBComm1').val(), '#DdlSBComm');
            GroupComm.commissionOnChange($('#DdlRCComm1').val(), '#DdlRCComm');
            GroupComm.commissionOnChange($('#DdlNGComm1').val(), '#DdlNGComm');
            GroupComm.commissionOnChange($('#DdlLCComm1').val(), '#DdlLCComm');
            GroupComm.commissionOnChange($('#DdlCSComm1').val(), '#DdlCSComm');
        },
        initEvent: function () {
            $("#GroupName").focus();

            $("#showInput").spectrum({
                chooseText: Site.getSingleResource('OK'),
                cancelText: Site.getSingleResource('Cancel'),
                color: $("#GroupColor").val(),
                palette: GroupComm.getcolorPalette(),
                showPalette: true,
                showInitial: true,

                change: function (color) {
                    colorText = color.toHexString(); // #ff0000
                    $('#GroupColor').val(colorText);
                }
            });

            $('#Cancel').click(function () {
                window.location.href = resolveClientUrl("CustomerComGroup/Index");
            });

            $('#CustList').click(function () {
                var groupCommId = $('#GroupCommId').val();

                parent.$("#menu").contents().find("td.active-menu").removeClass('active-menu');
                parent.$("#menu").contents().find("#child-menu-43").addClass('active-menu');
                parent.document.title = 'LRF - Customer Information - Customer List';
                window.location.href = resolveClientUrl("Customer/CustomerList") + "?groupId=" + groupCommId;
            });

            $('#Validate').click(function () {
                var groupName = $('#GroupName').val();

                //Check empty group name
                if (groupName.length < 1) {
                    UpdateGroup.displayError(Site.getSingleResource('GroupNameNotEmptyMsg'));
                    return;
                }

                //Check if group name lengh greater than 100.
                if (groupName.length >= 100) {
                    UpdateGroup.displayError(Site.getSingleResource('GroupNameNotExceed100CharactersMsg'));
                    return;
                }

                //Check if group name is 'none'.
                if ($.trim(groupName).toLowerCase() === "none") {
                    UpdateGroup.displayError(Site.getSingleResource('ChooseGroupNameMsg'));
                    return;
                }

                // Check valid group name (allow characters and number only).
                if (!groupName.match('^[a-zA-Z0-9 ]+$')) {
                    UpdateGroup.displayError(Site.getSingleResource('GroupNameOnlyContainCharactersAndNumberMsg'));
                    return;
                }

                $('span.field-validation-error[data-valmsg-for="GroupName"]').hide();

                UpdateGroup.validateName($.trim(groupName));
            });
            $("#Save").click(function () {
                //Site.blockUI();
                GroupComm.initCommParam();
                GroupComm.initDefaultParam("#update-group-popup");
            });

            // Commission dropdownlist change
            $('#DdlSBComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlSBComm');
            });

            $('#DdlRCComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlRCComm');
            });

            $('#DdlNGComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlNGComm');
            });

            $('#DdlLCComm1').on('change', function () {                
                GroupComm.commissionOnChange($(this).val(), '#DdlLCComm');
            });

            $('#DdlCSComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlCSComm');
            });
        },
        showConfirmAction: function (message, callback) {
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                buttons: {
                    OK: function () {
                        $("#update-group-confirm").dialog("close");
                        callback();
                    },
                    Cancel: function () {
                        $("#update-group-confirm").dialog("close");
                    }
                },
                close: function () {
                    $("#update-group-confirm").remove();
                }
            };
            $('<div id="update-group-confirm" title="">' + message + '</div>').dialog(options);
        },
        displayError: function (msg) {
            $('.response-area').removeClass("field-validation-success");
            $('.response-area').addClass("field-validation-error");
            $('.response-area').text(msg);
            $('.response-area').show();

            $('#ValidateFail').css('display', 'inline');
            $('#ValidateOK').css('display', 'none');
            $('#Save').attr("disabled", true);
            $('#GroupName').focus();
        },
        validateName: function (groupName) {
            var url = "CustomerComGroup/Validate?groupName=" + groupName;

            $.ajax({
                url: resolveClientUrl(url),
                type: 'GET',
                dataType: 'html',
                cache: false,
                beforeSend: function (xhr) {
                    Site.blockUI();
                },
                success: function (result) {
                    if (result === 'OK') {
                        $('.response-area').hide();

                        $('#ValidateOK').css('display', 'inline');
                        $('#ValidateFail').css('display', 'none');
                        $('#Save').attr("disabled", false);
                    }
                    else {
                        UpdateGroup.displayError(Site.getSingleResource('GroupNameExistedMsg'))
                    }

                    Site.unBlockUI();
                },
                error: function (xhr, status, err) {
                    handleAjaxError(xhr, status, err);
                }
            });
        }
    };
})(jQuery);

$(window).load(function () {
    UpdateGroup.initialize();
});