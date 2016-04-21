"use strict";
var openVoidbridgeResult = function (resultApi) {
    if (resultApi != '') {
        window.open(resultApi, '_blank', 'left=20,top=20,width=950,height=650,toolbar=1,resizable=1,scrollbars,status=1');
    } else {
        alert("Could not open Voidbridge Result Page!");
    }
}