define(["jquery", "./commgroupdetail"],
     function ($, CommGroupDetail) {
         "use strict";

         var FORM_SELECTOR = "#customer-comm-group-edit";
         var TITLE = "EditGroup";
         var _originalGroupName = "";

         function initialize() {
             CommGroupDetail.initialize(TITLE);

             _originalGroupName = $("#OriginalGroupName").val();
             _me.initEvent();
         }

         function initEvent() {
             $("#Validate").click(function () {
                 var $groupName = $("#GroupName");

                 if ($groupName.val() === _originalGroupName) {
                     $groupName.removeClass("invalid");
                     $groupName.addClass("valid");
                     $("#Add").prop("disabled", false);
                     return;
                 }

                 CommGroupDetail.checkAvailableGroup($groupName.val());
             });

             $("#CustList").click(function () {
                 CommGroupDetail.goToCustomerList($('#GroupCommId').val());
             });

             $("#Submit").click(function () {
                 CommGroupDetail.submit(FORM_SELECTOR);
             });
         }         

         var _me = {
             initialize: initialize,
             initEvent: initEvent
         };

         return _me;
     });