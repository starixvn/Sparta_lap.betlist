var path = require('path')

var projectRoot = path.resolve(__dirname, '../../../');

var commonPathPlugin = "./www/css/source/vendors/";
var cssPaths = {
    build: path.join(projectRoot, '/www/css/test'),
    site: "./www/css/source/site.scss",
    plugins: [
        commonPathPlugin + "asscrollable/asScrollable.min.css",
        commonPathPlugin + "daterangepicker/daterangepicker.scss",
        //commonPathPlugin + "flag-icon-css/flag-icon.min.css",
        commonPathPlugin + "waves/waves.min.css",
    ],

    livereport: "./www/css/source/pages/Report/liveReport.scss"
}

var scriptPaths = {
    build: path.join(projectRoot, '/www/js/test'),

    home: "./www/js/source/pages/home/home.js",
    livereport: "./www/js/source/pages/livereport/livereport.js"
}



module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
}