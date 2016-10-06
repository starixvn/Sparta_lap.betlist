var projectRoot = require('../config/path').projectRoot

var alias = {
    // vendors
    materialize: projectRoot + "/www/js/source/vendors/materialize/materialize.js",

    // plugins
    daterangepicker: "bootstrap-daterangepicker",

    // common
    querystring: projectRoot + "/www/js/source/common/querystring.js",
    site: projectRoot + "/www/js/source/common/site.js",
    flatpopup: projectRoot + "/www/js/source/components/flatpopup/jquery.flatpopup.js",
    monthpicker: projectRoot + "/www/js/source/components/monthpicker/monpickr.js",
}

module.exports = alias