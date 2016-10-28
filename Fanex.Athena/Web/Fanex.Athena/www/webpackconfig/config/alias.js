var projectRoot = require('../config/path').projectRoot;

var alias = {
    // vendors
    'vendor-materialize': projectRoot + "/www/js/source/vendors/materialize/materialize.js",
    'materialize-ext': projectRoot + "/www/js/source/vendors/materialize/materialize-ext.js",
    // common
    common: projectRoot + "/www/js/source/common",
    lib: projectRoot + "/www/js/source/lib",
    extension: projectRoot + "/www/js/source/extensions",
   
};

module.exports = alias;