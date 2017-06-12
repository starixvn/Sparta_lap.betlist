define(["jquery", "common/site", "common/popup", "lib/jquery.colorpicker"],
     function ($, Site, Popup) {
         "use strict";

         function initialize(titleResource) {
             Site.initialize();
          
             initColorPicker();
             _me.setPageTitle(Site.getSingleResource(titleResource));
             _me.initEvent();
         }

         function initEvent() {
             $("#GroupName").keyup(function () {
                 $("#GroupName").removeClass("invalid");
                 $("#GroupName").removeClass("valid");
                 $("#Add").prop("disabled", false);
             });

             $("#Cancel").click(function () {
                 _me.setPageTitle("CommissionGroupList");
                 _me.goToSite("CustomerComGroup/Index");
             });

             // Commission dropdownlist change
             $("#SBComm1").on("change", function () {
                 commissionOnChange($(this).val(), "SBComm");
             });

             $("#ParlayComm1").on("change", function () {
                 commissionOnChange($(this).val(), "ParlayComm");
             });

             $("#RCComm1").on("change", function () {
                 commissionOnChange($(this).val(), "RCComm");
             });

             $("#NGComm1").on("change", function () {
                 commissionOnChange($(this).val(), "NGComm");
             });

             $("#LCComm1").on("change", function () {
                 commissionOnChange($(this).val(), "LCComm");
             });

             $("#CSComm1").on("change", function () {
                 commissionOnChange($(this).val(), "CSComm");
             });
         }

         function setPageTitle(title) {
             $("#page-title", window.parent.document).text(title);
         }

         function checkAvailableGroup(groupName) {
             //Check empty group name
             if (groupName.length < 1) {
                 displayGroupNameError(Site.getSingleResource("GroupNameNotEmptyMsg"));
                 return;
             }

             //Check if group name lengh greater than 100.
             if (groupName.length >= 100) {
                 displayGroupNameError(Site.getSingleResource("GroupNameNotExceed100CharactersMsg"));
                 return;
             }

             //Check if group name is "none".
             if ($.trim(groupName).toLowerCase() === "none") {
                 displayGroupNameError(Site.getSingleResource("ChooseGroupNameMsg"));
                 return;
             }

             // Check valid group name (allow characters and number only).
             if (!groupName.match("^[a-zA-Z0-9 ]+$")) {
                 displayGroupNameError(Site.getSingleResource("GroupNameOnlyContainCharactersAndNumberMsg"));
                 return;
             }

             validateGroupName($.trim(groupName));
         }

         function validateGroupName(groupName) {
             var url = "CustomerComGroup/Validate?groupName=" + groupName;

             $.ajax({
                 url: Site.resolveClientUrl(url),
                 type: "GET",
                 dataType: "html",
                 cache: false,
                 beforeSend: function () {
                     Site.blockUI();
                 },
                 success: function (result) {
                     if (result === "OK") {
                         $("#GroupName").removeClass("invalid");
                         $("#GroupName").addClass("valid");
                         $("#Add").prop("disabled", false);
                     }
                     else {
                         displayGroupNameError(Site.getSingleResource("GroupNameExistedMsg"));
                     }

                     Site.unBlockUI();
                 }
             });
         }

         function displayGroupNameError(errorMsg) {
             $('label[for="GroupName"]').attr("data-error", errorMsg);
             $("#GroupName").addClass("invalid").focus();
             $("#Add").prop("disabled", true);
         }

         function commissionOnChange(value, idName) {
             var comm2 = "#" + idName + "2", comm3 = "#" + idName + "3";

             if (Number(value) === 15) {
                 $(comm2).val(0);
                 $(comm2).selectList("update", {
                     isDisabled: true,
                     selectionId: "select-list-" + idName + "2",
                     labelText: "0"
                 });
                 $(comm3).val(0);
                 $(comm3).selectList("update", {
                     isDisabled: true,
                     selectionId: "select-list-" + idName + "3",
                     labelText: "0"
                 });
             } else {
                 $(comm2).selectList("disabled", false);
                 $(comm3).selectList("disabled", false);
             }
         }      

         function submit(formId) {
             initCommParam("#SBComm");
             initCommParam("#ParlayComm");
             initCommParam("#RCComm");
             initCommParam("#NGComm");
             initCommParam("#LCComm");
             initCommParam("#CSComm");

             initDefaultParam(formId);
         }

         function initCommParam(id) {
             var comm1 = parseInt($(id + "1").val());
             var comm2 = parseInt($(id + "2").val());
             var comm3 = parseInt($(id + "3").val());

             var comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
             comm = isNaN(comm) ? 0 : comm;

             $(id).val(comm);
         }         

         function initDefaultParam(formId) {
             var isDefault = $("#IsDefaultGroup").is(":checked");
             var defaultGroupId = parseInt($('#DefaultGroupId').val());
             var currentGroupId = parseFloat($('#GroupCommId').val());
             $("#IsDefaultGroup", formId).val(isDefault);
             $("#IsResetDefault", formId).val(false);
             if (isDefault && defaultGroupId !== currentGroupId && defaultGroupId !== 0) {
                 showConfirmAction(Site.getSingleResource('GroupSetAsDefaultMsg'), function () {
                     $("#IsResetDefault", formId).val(true);
                     $(formId).submit();
                 });
             } else {
                 $(formId).submit();
             }
         }

         function goToSite(relativeUrl, paramString) {
             var href = Site.resolveClientUrl(relativeUrl);
             if (paramString) {
                 href += paramString;
             }
             Site.redirect(href);
         }

         function goToCustomerList(groupCommId) {
             if (isParentSameOrigin()) {
                 $("li>#MenuItem55", window.parent.document).parent().removeClass("active");
                 $("li>#MenuItem43", window.parent.document).parent().addClass("active");
                 _me.setPageTitle("LRF - " + Site.getSingleResource("CustomerList"));
             }
             _me.goToSite("Customer/CustomerList", "?groupId=" + groupCommId);
         }

         function showConfirmAction(message, callback, groupId, isResetDefault) {
             var options = {
                 model: true,
                 title: Site.getSingleResource("Confirm"),
                 height: 150,
                 id: "confirm-action"
             };

             var buttons = [
                 {
                     text: "OK",
                     isClosedOnClick: true,
                     click: function () {
                         callback(groupId, isResetDefault);
                     },
                     classes: "btn btn-primary"
                 }, {
                     text: "Cancel",
                     isClosedOnClick: true,
                     classes: "btn btn-back"
                 }];

             Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
         }

         function isParentSameOrigin() {
             var sameOrigin;
             try {
                 sameOrigin = window.parent.location.host === window.location.host;
             }
             catch (e) {
                 sameOrigin = false;
             }
             return sameOrigin;
         }

         function initColorPicker() {
             var currentGroupColor = $("#GroupColor").val();
             $("#showInput")
                 .val(currentGroupColor)
                 .colorPicker({ 
                     transparency: true,
                     onColorChange : function(id, newValue) { 
                         $("#GroupColor").val(newValue);
                     }
                 });
         }

         var _me = {
             initialize: initialize,
             initEvent: initEvent,
             submit: submit,
             checkAvailableGroup: checkAvailableGroup,
             setPageTitle: setPageTitle,
             goToSite: goToSite,
             goToCustomerList: goToCustomerList
         };

         return _me;
     }
);