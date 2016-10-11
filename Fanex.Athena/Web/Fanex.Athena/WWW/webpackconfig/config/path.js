var path = require('path')

var projectRoot = path.resolve(__dirname, '../../../');

var cssCommonPath = "./www/css/source/";
var cssPaths = {
    build: "www/css/build",

    materialize: "./www/css/source/vendors/materialize/materialize.css",
    daterangepicker: ["bootstrap-daterangepicker/daterangepicker.css", "./www/css/source/components/daterangepicker/daterangepicker-ex.scss"],

    layout: ["./www/css/source/components/navbar/navBar.css", "./www/css/source/components/sidebar/sideBar.css", "./www/css/source/extensions/home.scss"],
    main: ["./www/css/source/main.css", "./www/css/source/extensions/main-ex.scss"],

    login: ["./www/css/source/pages/login.css", "./www/css/source/extensions/login-ex.scss"],
    livereport: ["./www/css/source/pages/liveReport.css", "./www/css/source/components/flatpopup/flatpopup.scss", "./www/css/source/components/monthpicker/monpickr.scss"]
}

var scriptPaths = {
    build: "www/js/build",

    layout: ["./www/js/source/lib/navbar.js", "./www/js/source/lib/sidebar.js", "./www/js/source/pages/home/home.js"],

    livereport: "./www/js/source/pages/livereport/livereport.js",
    livereportmatchlist: "./www/js/source/pages/livereport/livereportmatchlist.js",
}

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
}