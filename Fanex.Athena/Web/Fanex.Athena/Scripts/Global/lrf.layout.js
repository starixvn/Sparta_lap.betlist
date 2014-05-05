/// <reference path="/@JSense.js" />
/// /* File Created: April 9, 2012 */
var lrf = {};
/* lrf.layout */
lrf.layout =
{
    lrfLayout: null,
    init: function (options) {
        var setOpts = options ? $.extend(options, this.defaults) : this.defaults;
        this.lrfLayout = $('body').layout({
            closable: true
            // pane can open & close
            ,
            fxName: "none",
            resizeWhileDragging: false,
            resizerDblClickToggle: true,
            resizable: false
            // when open, pane can be resized
            ,
            slidable: false
            // when closed, pane can 'slide' open over other panes - closes on mouse-out
            //	top panel
            ,
            north__slidable: true
            // OVERRIDE the pane-default of 'slidable=true'
            ,
            north__togglerLength_closed: 0
            // toggle-button is full-width of resizer-bar
            ,
            north__spacing_closed: 0
            // big resizer-bar when open (zero height)
            ,
            north__resizable: true,
            north__spacing_open: 0,
            north__size: setOpts.topPanel,
            maskIframesOnResize: true
            // bottom panel
            ,
            south__resizable: true
            // OVERRIDE the pane-default of 'resizable=true'
            ,
            south__spacing_open: 0
            // no resizer-bar when open (zero height)
            ,
            south__spacing_closed: 0
            // big resizer-bar when open (zero height)
            ,
            south__size: setOpts.bottomPanel,
            south__minSize: 0,
            south__maxSize: 0,

            west__resizerClickToggle: false
            //	left panel
            ,
            west__size: setOpts.leftPanel,
            west__minSize: 0,
            west__maxSize: 0,
            west__spacing_closed: 8,
            west__spacing_open: 8,
            west__onresize_end: function (e) {
                lrf.layout.westResize();
            }

            // right panel
            ,
            center__minWidth: 600,
            center__maxWidth: 0
            // use layout cookie
            ,
            useStateCookie: true
        });
        this.defaults = setOpts;
    },

    getLayout: function () {
        return this.lrfLayout;
    },
    westResize: function () { },
    defaults:
    {
        leftPanel: 170,
        rightPanel: 0,
        topPanel: 52,
        bottomPanel: 25
    }
};
/* fps.admin */

function getCookie(cName) {
    var i,
    x,
    y,
    arRcookies = document.cookie.split(";");
    for (i = 0; i < arRcookies.length; i++) {
        x = arRcookies[i].substr(0, arRcookies[i].indexOf("="));
        y = arRcookies[i].substr(arRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == cName) {
            return unescape(y);
        }
    }
}

function setCookie(cName, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var cValue = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString()) + "; path=/";
    document.cookie = cName + "=" + cValue;
}

function IsHideBrowserNotification() {
    var browserRemind = getCookie("hidebrowsernotification");
    if (browserRemind != null && browserRemind != "") {
        return true;
    }
    return false;
}

$(document).ready(function () {
    var matched = Site.detectBrowser(navigator.userAgent);
    var browser = {};
    if (matched.browser) {
        browser[matched.browser] = true;
        browser.version = matched.version;
    }

    if ((browser.msie && browser.version < 8) || (browser.mozilla && browser.version < 3.6)) {
        $("#browser").html(browser.msie ? "IE" : "Firefox");

        if (!IsHideBrowserNotification()) {
            $("#notification_popup").show("slow");
        }

        $("#notificaiton_close").click(function () {
            $("#notification_popup").slideUp();
        });

        $("a#remove_notif_cmd").click(function () {
            setCookie("hidebrowsernotification", "true", 14);
            $("#notification_popup").slideUp("slow");
        });
    } else {
        $("#notification_popup").hide();
    }
    lrf.layout.init();
});