/*global Site, GroupComm*/
/// <reference path="/@JSense.js" />
"use strict";
var CreateGroup = (function ($) {
    return {
        initialize: function () {
            CreateGroup.initEvent();
        },
        initEvent: function () {
            var currentGroupColor = $("#GroupColor").val();
            $("#showInput").spectrum({
                chooseText: Site.getSingleResource('OK'),
                cancelText: Site.getSingleResource('Cancel'),
                palette: GroupComm.getcolorPalette(),
                color: currentGroupColor.length > 0 ? currentGroupColor : "#ffffff",
                showPalette: true,
                showInitial: true,
                move: function () {
                },
                show: function () {
                },
                beforeShow: function () {
                },
                hide: function () {
                },
                change: function (color) {
                    $('#GroupColor').val(color.toHexString());
                }
            });
            $('#Add').click(function () {
                // Init param
                GroupComm.initCommParam();
                GroupComm.initDefaultParam("#create-group-popup");
            });
            $('#Cancel').click(function () {
                Site.redirect(Site.resolveClientUrl("CustomerComGroup/Index"));
            });

            $('#Validate').click(function () {
                var groupName = $('#GroupName').val();
                if (groupName.length < 1) {
                    CreateGroup.displayError(Site.getSingleResource('GroupNameNotEmptyMsg'));
                    return;
                }

                if (groupName.length >= 100) {
                    CreateGroup.displayError(Site.getSingleResource('GroupNameNotExceed100CharactersMsg'));
                    return;
                }

                if ($.trim(groupName).toLowerCase() === "none") {
                    CreateGroup.displayError(Site.getSingleResource('ChooseGroupNameMsg'));
                    return;
                }

                if (!groupName.match('^[a-zA-Z0-9 ]+$')) {
                    CreateGroup.displayError(Site.getSingleResource('GroupNameOnlyContainCharactersAndNumberMsg'));
                    return;
                }

                $('span.field-validation-error[data-valmsg-for="GroupName"]').hide();

                CreateGroup.validateName($.trim(groupName));
            });

            $('#GroupName').keyup(function () {
                $("#Add").prop("disabled", false);
            });

            // Commission dropdownlist change
            $('#DdlSBComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlSBComm');
            });

            $('#DdlParlayComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlParlayComm');
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

            $('#DdlABComm1').on('change', function () {
                GroupComm.commissionOnChange($(this).val(), '#DdlABComm');
            });
        },
        displayError: function (msg) {
            $('.response-area').removeClass("field-validation-success");
            $('.response-area').addClass("field-validation-error");
            $('.response-area').text(msg);
            $('.response-area').show();

            $('#ValidateFail').css('display', 'inline');
            $('#ValidateOK').css('display', 'none');
            $('#Add').prop("disabled", true);
            $('#GroupName').focus();
        },
        validateName: function (groupName) {
            var url = "CustomerComGroup/Validate?groupName=" + groupName;

            $.ajax({
                url: Site.resolveClientUrl(url),
                type: 'GET',
                dataType: 'html',
                cache: false,
                beforeSend: function () {
                    Site.blockUI();
                },
                success: function (result) {
                    if (result === 'OK') {
                        $('.response-area').hide();

                        $('#ValidateOK').css('display', 'inline');
                        $('#ValidateFail').css('display', 'none');
                        $('#Add').prop("disabled", false);
                    }
                    else {
                        CreateGroup.displayError(Site.getSingleResource('GroupNameExistedMsg'));
                    }

                    Site.unBlockUI();
                }
            });
        }
    };
})(jQuery);

if (!window._IsInJsTestingMode) {
    $(window).load(function () {
        CreateGroup.initialize();
    });
}