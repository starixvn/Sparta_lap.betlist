/// <reference path="/@JSense.js" />
define([], function () {
    "use strict";

    var parseQueryString = function (queryString) {
        var args = queryString.split('&');
        var argsList = {};

        for (var i = 0; i < args.length; i++) {
            var arg = unescape(args[i]);
            if (arg.length > 0) {
                if (arg.indexOf('=') === -1) {
                    argsList[$.trim(arg.toLowerCase())] = null;
                }
                else {
                    var keyValuePair = arg.split('=');
                    argsList[$.trim(keyValuePair[0].toLowerCase())] = $.trim(keyValuePair[1]);
                }
            }
        }

        return argsList;
    };

    var getQueryString = function (url) {
        var index = url.indexOf("?");
        return index < 0 ? "" : url.substring(index + 1, url.length);
    };

    var getRawUrl = function (url) {
        var index = url.indexOf("?");
        return index < 0 ? url : url.substring(0, index);
    };

    function setParam(url, name, value) {
        var rawUrl = getRawUrl(url);
        var queryString = getQueryString(url);
        var args = parseQueryString(queryString);

        if (name == null || name === undefined) {
            return url;
        }

        // Update value
        var found = false;
        for (var key in args) {
            if (key != null && key !== undefined && key.toLowerCase() === name.toLowerCase()) {
                // Does not update data if value = undefined
                if (value !== undefined) {
                    args[key] = value;
                }
                found = true;
            }
        }

        if (!found) {
            args[name.toLowerCase()] = value;
        }

        var queryArray = [];

        // Rebuild query string, ignore null value
        for (var key1 in args) {
            if (args[key1] != null && args[key1] !== undefined) {
                queryArray.push(key1.toLowerCase() + "=" + args[key1]);
            }
        }

        return rawUrl + "?" + queryArray.join("&");
    }

    function setParams(url, parameters) {
        for (var property in parameters) {
            if (parameters.hasOwnProperty(property)) {
                url = setParam(url, property, parameters[property]);
            }
        }

        return url;
    }

    return {
        getQueryString: getQueryString,
        getRawUrl: getRawUrl,
        setParam: setParam,
        setParams: setParams,
        getParam: function (url, name) {
            var queryString = getQueryString(url);
            var args = parseQueryString(queryString);

            for (var key in args) {
                if (key != null && key.toLowerCase() === name.toLowerCase()) {
                    return args[key];
                }
            }

            return null;
        }
    };
});