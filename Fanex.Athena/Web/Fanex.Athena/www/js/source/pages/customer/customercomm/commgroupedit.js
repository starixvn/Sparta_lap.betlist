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
                 var groupCommId = $('#GroupCommId').val();
                 if (isParentSameOrigin()) {
                     $("li>#MenuItem55", window.parent.document).parent().removeClass("active");
                     $("li>#MenuItem43", window.parent.document).parent().addClass("active");
                     CommGroupDetail.setPageTitle("LRF - Customer Information - Customer List");
                 }
                 CommGroupDetail.goToSite("Customer/CustomerList", "?groupId=" + groupCommId);
             });

             $("#Submit").click(function () {
                 CommGroupDetail.submit("#customer-comm-group-edit");
             });
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

         var _me = {
             initialize: initialize,
             initEvent: initEvent
         };

         return _me;
     });