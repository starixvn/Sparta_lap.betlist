define(["jquery", "common/site", "common/popup", "lib/spectrum"],
     function ($, Site, Popup) {
         "use strict";

         function initialize() {
             Site.initialize();
          
             initColorPicker();
             setPageTitle();
             _me.initEvent();
         }

         function initEvent() {
             $("#Validate").click(function () {
                 checkAvailability();
             });

             $("#Cancel").click(function () {
                 window.location.href = Site.resolveClientUrl("CustomerComGroup/Index");
             });

             $("#GroupName").keyup(function () {
                 $("#Add").prop("disabled", false);
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

             $('#Add').click(function () {
                 // Init param
                 initCommParam("#SBComm");
                 initCommParam("#ParlayComm");
                 initCommParam("#RCComm");
                 initCommParam("#NGComm");
                 initCommParam("#LCComm");
                 initCommParam("#CSComm");

                 initAddParam();
             });
         }

         function initColorPicker() {
             var currentGroupColor = $("#GroupColor").val();
             $("#showInput").spectrum({
                 chooseText: Site.getSingleResource("OK"),
                 cancelText: Site.getSingleResource("Cancel"),
                 palette: getcolorPalette(),
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
                     $("#GroupColor").val(color.toHexString());
                 }
             });
         }

         function setPageTitle() {
             $("#page-title", window.parent.document).text(Site.getSingleResource("CreateNewGroup"));
         }

         function checkAvailability() {
             var groupName = $("#GroupName").val();

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

         function initCommParam(id) {
             var comm1 = parseInt($(id + "1").val());
             var comm2 = parseInt($(id + "2").val());
             var comm3 = parseInt($(id + "3").val());

             var comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
             comm = isNaN(comm) ? 0 : comm;

             $(id).val(comm);
         }         

         function initAddParam() {
             var isDefault = $("#IsDefaultGroup").is(":checked");
             var defaultGroupId = parseInt($('#DefaultGroupId').val());
             var currentGroupId = parseFloat($('#GroupCommId').val());
             $("#IsDefaultGroup").val(isDefault);
             if (isDefault && defaultGroupId !== currentGroupId && defaultGroupId !== 0) {
                 showConfirmAction(Site.getSingleResource('GroupSetAsDefaultMsg'), function () {
                     $('#IsResetDefault').val(true);
                     $("form").submit();
                 });
             } else {
                 $("form").submit();
             }
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

         function getcolorPalette() {
             return ["000000", "000033", "000066", "000099 ", "0000CC", "0000FF",
                 "003300", "003333", "003366", "003399 ", "0033CC", "0033FF",
                 "006600", "006633", "006666 ", "006699 ", "0066CC", "0066FF",
                 "009900", "009933", "009966 ", "009999 ", "0099CC", "0099FF",
                 "00CC00", "00CC33", "00CC66 ", "00CC99 ", "00CCCC", "00CCFF",
                 "00FF00", "00FF33", "00FF66 ", "00FF99 ", "00FFCC", "00FFFF",
                 "330000", "330033", "330066 ", "330099 ", "3300CC", "3300FF",
                 "333300", "333333", "333366 ", "333399 ", "3333CC", "3333FF",
                 "336600", "336633", "336666 ", "336699 ", "3366CC", "3366FF",
                 "339900", "339933", "339966 ", "339999 ", "3399CC", "3399FF",
                 "33CC00", "33CC33", "33CC66 ", "33CC99 ", "33CCCC", "33CCFF",
                 "33FF00", "33FF33", "33FF66 ", "33FF99 ", "33FFCC", "33FFFF",
                 "660000", "660033", "660066 ", "660099 ", "6600CC", "6600FF",
                 "663300", "663333", "663366 ", "663399 ", "6633CC", "6633FF",
                 "666600", "666633", "666666 ", "666699 ", "6666CC", "6666FF",
                 "669900", "669933", "669966 ", "669999 ", "6699CC", "6699FF",
                 "66CC00", "66CC33", "66CC66 ", "66CC99 ", "66CCCC", "66CCFF",
                 "66FF00", "66FF33", "66FF66 ", "66FF99 ", "66FFCC", "66FFFF",
                 "990000", "990033", "990066 ", "990099 ", "9900CC", "9900FF",
                 "993300", "993333", "993366 ", "993399 ", "9933CC", "9933FF",
                 "996600", "996633", "996666 ", "996699 ", "9966CC", "9966FF",
                 "999900", "999933", "999966 ", "999999 ", "9999CC", "9999FF",
                 "99CC00", "99CC33", "99CC66 ", "99CC99 ", "99CCCC", "99CCFF",
                 "99FF00", "99FF33", "99FF66 ", "99FF99 ", "99FFCC", "99FFFF",
                 "CC0000", "CC0033", "CC0066 ", "CC0099 ", "CC00CC", "CC00FF",
                 "CC3300", "CC3333", "CC3366 ", "CC3399 ", "CC33CC", "CC33FF",
                 "CC6600", "CC6633", "CC6666 ", "CC6699 ", "CC66CC", "CC66FF",
                 "CC9900", "CC9933", "CC9966 ", "CC9999 ", "CC99CC", "CC99FF",
                 "CCCC00", "CCCC33", "CCCC66 ", "CCCC99 ", "CCCCCC", "CCCCFF",
                 "CCFF00", "CCFF33", "CCFF66 ", "CCFF99 ", "CCFFCC", "CCFFFF",
                 "FF0000", "FF0033", "FF0066 ", "FF0099 ", "FF00CC", "FF00FF",
                 "FF3300", "FF3333", "FF3366 ", "FF3399 ", "FF33CC", "FF33FF",
                 "FF6600", "FF6633", "FF6666 ", "FF6699 ", "FF66CC", "FF66FF",
                 "FF9900", "FF9933", "FF9966 ", "FF9999 ", "FF99CC", "FF99FF",
                 "FFCC00", "FFCC33", "FFCC66 ", "FFCC99 ", "FFCCCC", "FFCCFF",
                 "FFFF00", "FFFF33", "FFFF66", "FFFF99", "FFFFCC", "FFFFFF"];
         }

         var _me = {
             initialize: initialize,
             initEvent: initEvent
         };

         return _me;
     }
);