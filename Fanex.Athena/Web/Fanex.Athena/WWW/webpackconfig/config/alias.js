var projectRoot = require('../config/path').projectRoot

var alias = {
    // vendors
    materialize: "./www/js/source/vendors/materialize/materialize.js",

    // plugins
    moment: projectRoot + "/www/js/source/plugins/moment/moment.min.js",
    daterangepicker: projectRoot + "/www/js/source/plugins/daterangepicker/daterangepicker.js",

    // common
    querystring: projectRoot + "/www/js/source/common/querystring.js",
    site: projectRoot + "/www/js/source/common/site.js",
}

module.exports = alias