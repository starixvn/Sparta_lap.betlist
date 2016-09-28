var path = require('path')

var projectRoot = path.resolve(__dirname, '../../../');

var cssCommonPath = "./www/css/source/";
var cssPaths = {
    build: path.join(projectRoot, '/www/css/test'),

    materialize: "./www/css/source/vendors/materialize/materialize.scss",

    layout: ["./www/css/source/layout/navBar.scss", "./www/css/source/layout/sideBar.scss"],
    main: "./www/css/source/main.scss",

    livereport: "./www/css/source/pages/liveReport.scss",
}

var scriptPaths = {
    build: path.join(projectRoot, '/www/js/test'),

    materialize: "./www/js/source/vendors/materialize/materialize.js",

    layout: ["./www/js/source/components/navbar/navbar.js", "./www/js/source/components/sidebar/sidebar.js"],

    livereport: "./www/js/source/pages/livereport/livereport.js"
}

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
}