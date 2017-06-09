define(["jquery", "./commgroupdetail"],
     function ($, CommGroupDetail) {
         "use strict";

         var _originalGroupName = "";

         function initialize() {
             CommGroupDetail.initialize("EditGroup");

             _originalGroupName = $("#OriginalGroupName").val();
             _me.initEvent();
         }

         function initEvent() {
             $("#Validate").click(function () {
                 var groupName = $("#GroupName").val();

                 if (groupName === _originalGroupName) {
                     $("#GroupName").removeClass("invalid");
                     $("#GroupName").addClass("valid");
                     $("#Add").prop("disabled", false);
                     return;
                 }

                 CommGroupDetail.checkAvailableGroup(groupName);
             });

             $("#CustList").click(function () {
                 CommGroupDetail.goToCustomerList($('#GroupCommId').val());
             });

             $("#Submit").click(function () {
                 CommGroupDetail.submit("#customer-comm-group-edit");
             });
         }         

         var _me = {
             initialize: initialize,
             initEvent: initEvent
         };

         return _me;
     });