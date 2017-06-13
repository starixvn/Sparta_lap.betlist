/*exported QueryString*/
/// <reference path="/@JSense.js" />

"use strict";
var QueryString = (function () {
    function getQueryString(url) {
        var index = url.indexOf("?");
        return index < 0 ? "" : url.substring(index + 1, url.length);
    }

    function getRawUrl(url) {
        var index = url.indexOf("?");
        return index < 0 ? url : url.substring(0, index);
    }

    function setParam(url, key, value) {
        // Does not update data if value = undefined
        if (value === undefined) {
            return url;
        }

        var isRemoving = value === null;

        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = url.indexOf('?') !== -1 ? "&" : "?";
        
        if (url.match(re)) {
            return url.replace(re, '$1' + (isRemoving ? '' : key + "=" + value + '$2')).replace("&&", "&");
        }
        else {
            return url + (isRemoving ? '' : separator + key + "=" + value).replace("&&", "&");
        }
    }

    function setParams(url, parameters) {
        for (var property in parameters) {
            if (parameters.hasOwnProperty(property)) {
                url = setParam(url, property, parameters[property]);
            }
        }

        return url;
    }

    function getParam(url, name) {
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) {
            return null;
        }
        if (!results[2]) {
            return '';
        }
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    return {
        getQueryString: getQueryString,
        getRawUrl: getRawUrl,
        setParam: setParam,
        setParams: setParams,
        getParam: getParam
    };
})();