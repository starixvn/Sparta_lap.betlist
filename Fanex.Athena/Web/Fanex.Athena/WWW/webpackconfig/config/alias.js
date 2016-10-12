var projectRoot = require('../config/path').projectRoot

var alias = {
    // vendors
    'vendor-materialize': projectRoot + "/www/js/source/vendors/materialize/materialize.js",

    // common
    common: projectRoot + "/www/js/source/common",
    lib: projectRoot + "/www/js/source/lib"
}

module.exports = alias