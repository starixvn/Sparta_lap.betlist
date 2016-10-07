var projectRoot = require('../config/path').projectRoot

var alias = {
    // vendors
    materialize: projectRoot + "/www/js/source/vendors/materialize/materialize.js",
    jquerycookie: projectRoot + "/www/js/source/vendors/jquery/jquery.cookie.js",
    jclock: projectRoot + "/www/js/source/vendors/jquery/jquery.jclock.js",

    // plugins
    daterangepicker: "bootstrap-daterangepicker",

    // common
    querystring: projectRoot + "/www/js/source/common/querystring.js",
    site: projectRoot + "/www/js/source/common/site.js",
    flatpopup: projectRoot + "/www/js/source/components/flatpopup/jquery.flatpopup.js",
    monthpicker: projectRoot + "/www/js/source/components/monthpicker/monpickr.js",
}

module.exports = alias