/*global Site*/
/// <reference path="/@JSense.js" />
"use strict";
var EditPermission = (function ($) {
    var colUrl = null;
    var expUrl = null;

    return {
        // #region: edit permission

        collapseExpand: function (sender) {
            var target = $(sender);
            var id = target.attr('id');
            var curStatus = target.attr("data-status");

            if (id === 'img6') {
                return;
            }

            if (curStatus === 'expand') {
                target.attr('src', expUrl);
                $('#group_' + id).slideUp();
                target.attr("data-status", "collapse");
            }
            else {
                $('#group_' + id).slideDown();
                target.attr('src', colUrl);
                target.attr("data-status", "expand");
            }
        },

        checkedChange: function (sender) {
            var target = $(sender);
            var id = target.attr('id');
            var checked = target.is(':checked');

            if (checked === true) {
                EditPermission.checkAll(id);
            }
            else {
                EditPermission.uncheckAll(id);
            }

            if (id !== '6') {
                $('#group_img' + id).show();
                $('#img' + id).attr('src', colUrl);
                $('#img' + id).attr("status", "expand");
            }
        },

        checkAll: function (id) {
            var container = $("#group_img" + id);
            var list = $("input", container);

            for (var i = 0; i < list.length; i++) {
                list[i].checked = true;
            }
        },

        uncheckAll: function (id) {
            var container = $("#group_img" + id);
            var list = $("input", container);

            for (var i = 0; i < list.length; i++) {
                list[i].checked = false;
            }
        },

        childCheckedChange: function (parentId) {
            var container = $("#group_img" + parentId);
            var childCheckedCount = $("input:checked", container).length;

            if (childCheckedCount > 0) {
                $("#" + parentId).prop('checked', 'true');
            }
            else {
                $("#" + parentId).removeAttr('checked');
            }
        },

        getSelectedFunctions: function () {
            var functionIdList = '';

            if ($('.permissionBox').length > 0) {
                var checkedInputs = $("input:checked", '.permissionBox');
                $.each(checkedInputs, function () {
                    functionIdList += $(this).attr('id') + ",";
                });

                var len = functionIdList.length;
                functionIdList = functionIdList.substring(0, len - 1);
            }

            return functionIdList;
        },

        updatePermission: function () {
            var form = $('#EditPermissionForm');
            var url = form.attr('action');
            var functionIdList = EditPermission.getSelectedFunctions();
            $('.response-area').hide();
            var param =
            {
                UserId: $('#UserId').val(),
                FunctionIdList: functionIdList
            };
            $.ajax({
                type: 'POST',
                dataType: "json",
                contentType: "application/json",
                cache: false,
                data: JSON.stringify(param),
                url: url,
                success: function (response) {
                    if (response.IsSuccessful === false) {
                        $('#resultMessage').removeClass('field-validation-success');
                        $('#resultMessage').addClass('field-validation-error');
                    }
                    else {
                        $('#resultMessage').removeClass('field-validation-error');
                        $('#resultMessage').addClass('field-validation-success');
                    }

                    $('#resultMessage').text(response.Message);
                    $('.response-area').show();
                    $('.response-area').animate({ "opacity": "hidden" }, 10000, function () { $(this).fadeOut(1000); });
                },
                error: function () {
                }
            });
        },

        // #endregion: edit permission

        // #region: assign currency

        toggleCurrency: function (sender) {
            var target = $(sender);
            var curStatus = target.attr("data-status");

            if (curStatus === 'expand') {
                $('#currency-content').slideUp();
                target.attr('src', expUrl);
                target.attr("data-status", "collapse");
            }
            else {
                $('#currency-content').slideDown();
                target.attr('src', colUrl);
                target.attr("data-status", "expand");
            }
        },

        changeAllCurrency: function (sender) {
            var target = $(sender);
            var checked = target.is(':checked');
            var container = $("#currency-content");
            var list = $("input", container);

            for (var i = 0; i < list.length; i++) {
                list[i].checked = checked;
            }

            $("#currency-content").show();
            $('#currency-img').attr('src', colUrl);
            $('#currency-img').attr("data-status", "expand");
        },

        changeEachCurrency: function (/*sender*/) {
            var container = $("#currency-content");
            var totalCurrency = $("input", container).length;
            var childCheckedCurrency = $("input:checked", container).length;

            if (childCheckedCurrency === totalCurrency) {
                $("#currency-header").prop('checked', 'true');
            }
            else {
                $("#currency-header").removeAttr('checked');
            }
        },

        getSelectedCurrencies: function () {
            var currencyList = '';

            if ($('.currencyBox').length > 0) {
                var checkedInputs = $("input:checked", '#currency-content');

                $.each(checkedInputs, function () {
                    currencyList += $(this).val() + ",";
                });

                var len = currencyList.length;
                currencyList = currencyList.substring(0, len - 1);
            }

            return currencyList;
        },

        showResponseMessage: function (response) {
            if (response.IsSuccessful === false) {
                $('#resultMessage').removeClass('field-validation-success');
                $('#resultMessage').addClass('field-validation-error');
            }
            else {
                $('#resultMessage').removeClass('field-validation-error');
                $('#resultMessage').addClass('field-validation-success');
            }

            $('#resultMessage').text(response.Message);
            $('.response-area').show();
            $('.response-area').animate({ "opacity": "hidden" }, 10000, function () { $(this).fadeOut(1000); });
        },

        assignCurrency: function () {
            $('.response-area').hide();
            var form = $('#AssignCurrencyForm');
            var url = form.attr('action');
            var currencyList = EditPermission.getSelectedCurrencies();
            if (currencyList.length <= 0) {
                var response = {
                    IsSuccessful: false,
                    Message: Site.getSingleResource('SelectAtLeastOneCurrencyMsg'),
                    Code: -1
                };
                EditPermission.showResponseMessage(response);
                return;
            }
            var data = {
                UserId: $('#UserId').val(),
                LicenseeId: 0,
                CurrencyList: currencyList,
                ModifyBy: "",
                LangKey: "",
                IP: ""
            };
            $.ajax({
                type: 'POST',
                dataType: "json",
                contentType: "application/json",
                cache: false,
                data: JSON.stringify(data),
                url: url,
                success: function (response) {
                    EditPermission.showResponseMessage(response);
                },
                error: function () {
                }
            });
        },

        // #endregion: assign currency

        init: function (colurl, expurl/*, null_url*/) {
            colUrl = colurl;
            expUrl = expurl;

            $.ajaxSetup({
                cache: false
            });

            $("#permission-tabs").tabs({
                active: '0',
                beforeLoad: function (event, ui) {
                    ui.jqXHR.error(function () {
                        ui.panel.html("Couldn't load this tab. We'll try to fix this as soon as possible. " +
                            "If this wouldn't be a demo.");
                    });
                },
                load: function (/*event, ui*/) {
                    var active = $(this).tabs("option", "active");
                    var enable = true;

                    if (active === 0) {
                        var permissionBox = $('.permissionBox');

                        if (permissionBox.length === 0) {
                            enable = false;
                        }
                    } else {
                        var currencyBox = $('.currencyBox');

                        if (currencyBox.length === 0) {
                            enable = false;
                        }
                    }

                    if (enable === false) {
                        $('#btnSave').attr('disabled', 'disabled');
                        $('#btnSave').css('cursor', 'default');
                    } else {
                        $('#btnSave').removeAttr('disabled');
                        $('#btnSave').css('cursor', 'pointer');
                    }
                }
            });

            $('#btnSave').click(function () {
                var active = $("#permission-tabs").tabs("option", "active");

                if (active === 0) {
                    EditPermission.updatePermission();
                } else {
                    EditPermission.assignCurrency();
                }
            });

            $('#btnReset').click(function () {
                var active = $("#permission-tabs").tabs("option", "active");
                $("#permission-tabs").tabs("load", active);
            });

            $('#btnCancel').click(function () {
                var url = Site.resolveClientUrl("Account/Manage");
                window.location.href = url;
            });
        }
    };
} (jQuery));