define(["jquery", "./commgroupdetail"],
     function ($, CommGroupDetail) {
         "use strict";

         var FORMID = "#customer-comm-group-add";
         var TITLE = "CreateNewGroup";

         function initialize() {
             CommGroupDetail.initialize(TITLE);

             _me.initEvent();
         }

         function initEvent() {
             $("#Validate").click(function () {
                 var groupName = $("#GroupName").val();
                 CommGroupDetail.checkAvailableGroup(groupName);
             });

             $("#Submit").click(function () {
                 CommGroupDetail.submit(FORMID);
             });
         }

         var _me = {
             initialize: initialize,
             initEvent: initEvent
         };

         return _me;
     });