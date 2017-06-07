define(["jquery", "./commgroupdetail"],
     function ($, CommGroupDetail) {
         "use strict";

         function initialize() {
             debugger;
             CommGroupDetail.initialize("CreateNewGroup");

             _me.initEvent();
         }

         function initEvent() {
             $("#Validate").click(function () {
                 var groupName = $("#GroupName").val();
                 CommGroupDetail.checkAvailableGroup(groupName);
             });

             $("#Submit").click(function () {
                 CommGroupDetail.submit("#customer-comm-group-add");
             });
         }

         var _me = {
             initialize: initialize,
             initEvent: initEvent
         };

         return _me;
     });