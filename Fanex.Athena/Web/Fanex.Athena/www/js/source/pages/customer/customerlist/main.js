define(["jquery", "common/querystring", "./customerlist", "./customerlist.commision"],
    function ($, QueryString, CustomerList, CustomerComm) {
        "use strict";

        $(document).ready(function () {
            
            try {
                parent.document.title = 'LRF - ' + $('.content-title').html();
            }
            catch (e) { }
            CustomerList.initialize();
            Site.keepBlockingAfterInit = true;

            if (QueryString.getParam(window.location.href, "userName") ||
                QueryString.getParam(window.location.href, "groupId")) {
                setTimeout(CustomerList.submit, 500);
            } else {
                Site.unBlockUI();
            }

            CustomerComm.initialize();
        });

        $(window).resize(function () {
            "use strict";
            jqGridAdditional.resizeGrid();
        });

        $(window).unload(function () {
            "use strict";
            $(window).unbind("resize");
        });
    });