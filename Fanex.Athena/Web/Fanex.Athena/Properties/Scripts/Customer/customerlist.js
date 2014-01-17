/// <reference path="/@JSense.js" />
var CustomerList = (function ($) {
    return {
        mousePosition: { x: 0, y: 0 },
        initialize: function () {
            CustomerList.initEvent();
            CustomerList.initPopup();
            CustomerList.initAutoComplete();
        },
        initEvent: function () {
            CustomerList.loadPostbackParam();

            $('#SubmitButton').click(function () {
                CustomerList.submitForm();
            });

            $('#SubmitButtonComm').click(function () {
                CustomerList.generateGroupCommSelect();
                CustomerList.submitForm();
            });

            $("#txtUsername").click(function () {
                CustomerList.setUsernameOnFocus();
            }).blur(function () {
                CustomerList.setDefaultUsername();
            });

            $("#data-content").on("click", ".ba-status", function (event) {
                //only allow send data when it has div element!!
                if ($(this).find("div").length > 0) {
                    event.preventDefault();
                    return CustomerList.updateBASetting(this);
                }
            });
            $("#data-content").on("change", ".special-level-list", function (event) {
                var $this = $(this);
                CustomerList.setSpecialLevel($this, $this.attr("data-cust-id"), $this.attr("data-level"));
            });

            $("#data-content").on("click", ".cust-status-option", function (event) {
                CustomerList.viewUpdatePopUp($(this));
                return false;
            });

            $("#data-content").on("click", ".cust-option-link", function (event) {
                CustomerList.viewCustOption($(this));
                return false;
            });

            $("#popup-update").on("click", function () {
                CustomerList.updateCustStatus();
            });
            $("#popup-close").on("click", function () {
                CustomerList.closeUpdatePopUp();
            })

            $("#popup-suspended").change(function () {
                CustomerList.custStatusCheckedChanged();
            });

            $("#popup-closed").change(function () {
                CustomerList.custStatusCheckedChanged()
            });

            $("#bet-setting-link").on("click", function () {
                CustomerList.viewBetSetting();
            });
            $("#bet-list-link").on("click", function () {
                CustomerList.viewBetList();
            });
            $("#statement-link").on("click", function () {
                CustomerList.viewStatement();
            });
            $("#login-info-link").on("click", function () {
                CustomerList.viewLogInfo();
            });
            $("#close-cust-option-link").on("click", function () {
                CustomerList.closeCustOption();
            });

            $(document).on('mousedown', function (e) {
                CustomerList.mousePosition = { x: e.pageX, y: e.pageY };
            });
        },
        initAutoComplete: function () {
            var txtUserName = $('#txtUsername');
            var serUrl = resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({ serviceUrl: serUrl, minChars: 2 });
                CustomerList.setDefaultUsername();
            }
        },
        initPopup: function () {
            var popupList = ["#view-cust-option", "#update-cust-status-popup"];
            $(document).mouseup(function (e) {
                $(popupList).each(function (index, element) {
                    var container = $(element);
                    if (container.is(":visible")) {
                        var inWrapper = container.get(0) === e.target || container.has(e.target).length > 0;
                        if (!inWrapper) {
                            container.hide();
                        }
                    }
                });
            });

            // Hide popup if scroll
            $("#data-content").scroll(function (e) {
                var context = $(this);
                $(popupList).each(function (index, element) {
                    var popup = $(element);
                    if (popup.is(":visible")) {
                        popup.hide();
                    }
                });
            });
        },
        submitForm: function () {
            Site.blockUI();
            CustomerList.generateParam();
            $('#CustomerForm').submit();
        },
        generateParam: function () {
            var username = $("#txtUsername").val();
            if (username == Site.getSingleResource('ExternalIdOrSystemId')) {
                username = '';
            };
            $("#Username").val(username);
            $("#PageIndex").val(1);
            $("#PageSize").val(500);
            $("#BaFilter").val($("#BAFilterOption").val());
            $("#CustStatus").val($("#CustStatusOption").val());
            $("#SpecialLevel").val($("#special-level-list-search").val());
        },
        generateGroupCommSelect: function () {
            $("#GroupCommId").val($("#DdlGroupCommId").val());
        },
        loadPostbackParam: function () {
            var username = $('#Username').val();
            if (username == '' || username == undefined) {
                $('#txtUsername').val(Site.getSingleResource('ExternalIdOrSystemId'));
            }
            else {
                $('#txtUsername').val(username);
                $('#txtUsername').attr('class', '');
            };
            Site.currencyConstraint();
            $('.content-filter').css('width', $('.data-content').css('width'))
        },

        setDefaultUsername: function () {
            var username = $('#txtUsername').val();
            if (username == '' || username == Site.getSingleResource('ExternalIdOrSystemId')) {
                $('#txtUsername').val(Site.getSingleResource('ExternalIdOrSystemId'));
                $('#txtUsername').attr('class', 'default-username');
            }
            else {
                $('#txtUsername').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#txtUsername').val();
            if (username == Site.getSingleResource('ExternalIdOrSystemId')) {
                $('#txtUsername').val('');
                $('#txtUsername').removeAttr('class');
            }
        },

        onSpecialLevelFilterChanged: function () {
            var list = $('#special-level-list-search');
            list.attr('class', 'level-' + list.val());
        },

        setSpecialLevel: function (list, custId, oldValue) {

            var level = list.val();
            var msg = level == 0 ? Site.getSingleResource('AlertRemoveSpecialLevelForCustomerMsg') : Site.getSingleResource('AlertSetSpecialLevelForCustomerMsg');
            var answer = confirm(msg);
            if (!answer) {
                list.val(oldValue);
                list.attr('class', 'level-' + oldValue);
                return;
            }

            var requestUrl = resolveClientUrl("CustomerAPI/UpdateSpecialLevel");
            list.attr('class', 'level-' + level);
            var param =
            {
                custId: custId,
                level: level
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(param),
                url: requestUrl,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
                    list.attr('class', 'level-' + level);
                },
                success: function (data) {
                    if (data.Success) {
                        list.unbind("change");
                        list.change(function () {
                            CustomerList.setSpecialLevel($(this), custId, level);
                        });
                    }
                    else {
                        list.val(oldValue);
                        list.attr('class', 'level-' + oldValue);
                        alert(data.ErrorMessage);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    list.val(oldValue);
                    list.attr('class', 'level-' + oldValue);
                    alert("Error");
                }
            });

        },

        viewCustOption: function (context) {
            var custId = context.attr("custid");
            var popup = $("#view-cust-option");
            var link = $("#cust-option-link-" + custId);

            if (popup.attr("custid") == custId) {
                popup.toggle();
            }
            else {
                popup.attr("custid", custId);
                popup.attr("externalid", link.attr("externalid"));
                popup.attr("systemid", link.attr("systemid"));

                popup.css({
                    top: CustomerList.mousePosition.y + 10,
                    left: CustomerList.mousePosition.x - 40
                });

                popup.show();
            }

            return false;
        },

        closeCustOption: function (custId) {
            var popup = $("#view-cust-option");
            if (popup.is(":visible")) {
                popup.hide();
            }
        },

        // -------------Update cust status----------------------------------------
        // view popup update
        viewUpdatePopUp: function (context) {
            var custId = context.attr("custid");
            var statusLink = $("#edit-cust-status-" + custId);
            var popup = $("#update-cust-status-popup");

            if (popup.attr("custid") == custId) {
                popup.toggle();
            }
            else {

                var isClosed = statusLink.attr("isclosed").toLowerCase() == "true";
                var isSuspended = statusLink.attr("issuspended").toLowerCase() == "true";
                var externalId = statusLink.attr("externalid");

                $("#popup-external-id").html(externalId);
                $("#popup-suspended").attr('checked', isSuspended);
                $("#popup-closed").attr('checked', isClosed);


                popup.attr("custid", custId);
                popup.attr("closed", isClosed);
                popup.attr("suspended", isSuspended);

                popup.css({
                    top: CustomerList.mousePosition.y + 10,
                    left: CustomerList.mousePosition.x - 90
                });

                popup.show();
            }

            return false;
        },

        closeUpdatePopUp: function () {
            var popup = $("#update-cust-status-popup");
            if (popup.is(":visible")) {
                $("#update-cust-status-popup").hide();
            }
        },

        // option popup
        custStatusCheckedChanged: function () {
            var popup = $("#update-cust-status-popup");
            var custId = popup.attr("custid");
            var isSuspended = $("#popup-suspended").is(':checked');
            var isClosed = $("#popup-closed").is(':checked');
            var chkSuspended = $("#CheckSuspended" + custId).is(":checked");
            var chkClose = $("#CheckClose" + custId).is(":checked");

            var changed = popup.attr("closed") != isClosed || popup.attr("suspended") != isSuspended;
            $("#popup-update").attr("disabled", !changed);
        },

        // ---------------update cust status------------------------------------------------
        updateCustStatus: function () {
            if (!window.confirm(Site.getSingleResource('ChangeStatusOfThisCustomerMsg'))) {
                CustomerList.closeUpdatePopUp();
                return;
            }

            var popup = $("#update-cust-status-popup");
            var custId = popup.attr("custid");
            var statusLink = $("#edit-cust-status-" + custId);

            var isSuspended = $("#popup-suspended").is(':checked');
            var isClosed = $("#popup-closed").is(':checked');
            var requestUrl = resolveClientUrl("CustomerAPI/UpdateCustStatus");
            $.ajax({
                type: "POST",
                data:
                {
                    custId: custId,
                    isSuspended: isSuspended,
                    isClosed: isClosed
                },
                url: requestUrl,

                dataType: "json",
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                    CustomerList.closeUpdatePopUp(custId);
                },
                success: function (result) {
                    if (result.Code == 0) {
                        var color = isClosed == true ? "#ccc7ba" : isSuspended == true ? "#f8ded7" : "#F6F8F9";

                        var statusText = isClosed == true ? "Closed" : isSuspended == true ? "Suspended" : "Open";

                        statusLink.html(statusText + '<span title="' + Site.getSingleResource('Status') + '" class="arrow1" >&nbsp;&nbsp;&nbsp;</span>');
                        statusLink.attr("isclosed", isClosed);
                        statusLink.attr("issuspended", isSuspended);

                        $("#row" + custId).css("background-color", color);
                    }
                    else {
                        alert(result.Message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                    //window.location.href = window.location.href;
                }

            });
        },
        // -----------------------------------------------------------------------

        // --------------update BA Status ----------------------------------------
        updateBASetting: function (context) {
            var canUpdateBA = $("#CanUpdateBA").val();
            var canUpdateBAMessage = $("#CanUpdateBAMessage").val();
            if (!canUpdateBA) {
                alert(canUpdateBAMessage);
                return;
            }

            var paramString = $(context).attr("data-val");
            var params = paramString.split("#");
            var baparam = (params[2] == "True" || params[2] == "true") ? true : false;
            var confirmMsg = params[2].toLowerCase() == "true" ? Site.getSingleResource("AlertTurnOnBAMsg") : Site.getSingleResource("AlertTurnOffBAMsg");
            if (params.length == 4) {
                var requestUrl = resolveClientUrl("CustomerAPI/UpdateBAStatus?custId=" + params[1] + "&isAllowBA=" + baparam + "&externalId=" + params[3]);
                var isUpdate = window.confirm(confirmMsg);
                if (isUpdate) {
                    var ctx = context;
                    var currentBAStatus = $(ctx).html();
                    var ajaxloading = $("#ajaxLoaDingUpdateBA").html();
                    var warningMes = $("#ErrorWhenUpdateBA").html();
                    var contextId = "aBA_" + params[1];
                    $.ajax({
                        type: "POST",
                        url: requestUrl,

                        beforeSend: function () {
                            $(ctx).html(ajaxloading);
                        },

                        complete: function () { },
                        success: function (data) {

                            //ConfirmMessage
                            if (data.IsSuccessful) {

                                $(ctx).html('<div class="ba-' + data.BAStatus.toLowerCase() + '"></div>');
                                if (data.IsBA) {
                                    $(ctx).css("color", "#000000");
                                }
                                else {
                                    $(ctx).css("color", "#cc0033");
                                }
                                var hrefString = "#" + params[1] + "#" + data.IsBA + "#" + params[3];
                                $(ctx).attr("data-val", hrefString);
                                $(ctx).attr("title", data.Tooltip);
                            }
                            else {

                                $(ctx).html(currentBAStatus);
                                alert(data.Message);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {

                            $(ctx).html(currentBAStatus);
                            alert(textStatus);

                        },
                        statusCode:
                        {
                            404: function () {

                                $(ctx).html(currentBAStatus);
                                alert('Error-Page not found');

                            }
                        }

                    });
                    return;
                };
                return;
            };
        },
        //------------------------------------------------------------------------

        // ------------Bet setting------------------------------------------------
        viewBetSetting: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption();

            var url = resolveClientUrl("BetSetting/Index?custid=" + custId + "&username=" + systemId + "&nickname=" + externalId);

            var popH = 450, popW = 905;

            var dialogFrame = $("#bet-setting-frame");
            dialogFrame.attr("height", popH);
            dialogFrame.attr("width", popW);
            dialogFrame.attr("src", url);

            var dialogForm = $('#bet-setting-dialog');
            dialogForm.dialog({
                modal: true,
                width: popW + 15,
                height: popH + 50,
                close: function (event, ui) {
                    dialogFrame.attr("src", "");
                }
            });
        },
        //------------------------------------------------------------------------

        // ---------------View Log Info ------------------------------------------
        viewLogInfo: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption();

            var url = "Customer/LogInfo?custid=" + custId;
            url += "&externalid=" + externalId;
            url += "&systemid=" + systemId;
            url = resolveClientUrl(url);

            var dialog = $('<div title="' + Site.getSingleResource('LogInfo') + '" style="display:none" class="loading"></div>').appendTo('body');

            dialog.dialog({
                // add a close listener to prevent adding multiple divs to the document
                close: function (event, ui) {
                    // remove div with all data and events
                    dialog.remove();
                },
                width: 360,
                height: 260,
                modal: true
            });
            // load remote content
            dialog.load(url,
                {}, // omit this param object to issue a GET request instead a POST request, otherwise you may provide post parameters within the object
                function (responseText, textStatus, XMLHttpRequest) {
                    // remove the loading class
                    dialog.removeClass('loading');
                }
            );
            //prevent the browser to follow the link
            return false;
        },

        // ------------------------------------------------------------------------
        // ---------------View Bet List -------------------------------------------
        viewBetList: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption(custId);
            var url = resolveClientUrl("BetList/CustomerOutstanding");
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "betlistname", "betlist");
            url = Site.setLicenseeOptionOriginalParam(url);
            var popH = 700, popW = 900;

            var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('BetList') + '"><iframe frameborder="0" width="' + popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 50
            });
        },

        // ------------------------------------------------------------------------
        // ---------------View Statement-------------------------------------------
        viewStatement: function () {
            var popup = $("#view-cust-option");
            var custId = popup.attr("custid");
            var systemId = popup.attr("systemid");
            var externalId = popup.attr("externalid");

            CustomerList.closeCustOption(custId);
            var url = resolveClientUrl("BetList/CustomerOutstanding");
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "betlistname", "dailystatement");
            url = Site.setLicenseeOptionOriginalParam(url);
            var popH = 700, popW = 900;

            var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('DailyStatement') + '"><iframe frameborder="0" width="' + popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 50
            });
        },
        // ------------------------------------------------------------------------
        // ---------------View Running Betlist-------------------------------------
        viewRunningBetlist: function (custId, systemId, externalId) {
            var url = resolveClientUrl("BetList/CustomerOutstanding");
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "betlistname", "betlistrunning");
            url = Site.setLicenseeOptionOriginalParam(url);

            var popH = 700, popW = 900;

            var dialogForm = '<div class="popup-title" title="' + Site.getSingleResource('BetListRunning') + '"><iframe frameborder="0" width="' + popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 50
            });
        }
    };

})(jQuery);

$(window).load(function () {
    parent.document.title = 'LRF - ' + $('.content-title').html();
    CustomerList.initialize();
});
