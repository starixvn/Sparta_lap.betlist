var path = require('path')

var projectRoot = path.resolve(__dirname, '../../../');

var cssCommonPath = "./www/css/source/";
var cssPaths = {
    build: path.join(projectRoot, '/www/css/test'),

    materialize: "./www/css/source/vendors/materialize/materialize.css",
    daterangepicker: ["bootstrap-daterangepicker/daterangepicker.css", "./www/css/source/components/daterangepicker/daterangepicker-ex.scss"],

    layout: ["./www/css/source/components/navbar/navBar.css", "./www/css/source/components/sidebar/sideBar.css"],
    main: "./www/css/source/main.css",

    livereport: "./www/css/source/pages/liveReport.css",
}

var scriptPaths = {
    build: path.join(projectRoot, '/www/js/test'),

    layout: ["./www/js/source/components/navbar/navbar.js", "./www/js/source/components/sidebar/sidebar.js"],

    livereport: "./www/js/source/pages/livereport/livereport.js",
}

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
}