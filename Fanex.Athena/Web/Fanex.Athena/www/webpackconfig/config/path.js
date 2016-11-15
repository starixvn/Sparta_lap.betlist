var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssCommonPath = "./www/css/source/";

var cssDateRangePickerPath = ["bootstrap-daterangepicker/daterangepicker.css", "./www/css/source/components/daterangepicker/daterangepicker-ex.scss"];

var cssPaths = {
    build: "www/css/build",

    home: ["malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css", "./www/css/source/components/navbar/navBar.scss", "./www/css/source/components/sidebar/sideBar.css", "./www/css/source/extensions/home.scss"],
    main: ["./www/css/source/vendors/materialize/materialize.css", "./www/css/source/main.css", "./www/css/source/extensions/main-ex.scss"],

    reportjqgridlayout: [
        "./www/css/source/components/flatpopup/flatpopup.scss",
        "./www/css/source/components/multiselect/jquery.multiselect.css", "./www/css/source/extensions/multiselect-ex.scss",
        "./www/css/source/pages/common.scss", "./www/css/source/extensions/report-ex.scss",
        "./www/css/source/components/jqgrid/jqgrid.css", "./www/css/source/extensions/jqgrid-ex.scss"
    ],

    login: ["./www/css/source/pages/login.css", "./www/css/source/extensions/login-ex.scss"],
    livereport: ["malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css", "./www/css/source/pages/common.scss", "./www/css/source/components/flatpopup/flatpopup.scss",
        "./www/css/source/components/monthpicker/monpickr.scss", "./www/css/source/extensions/report-ex.scss", "./www/css/source/pages/report/livereport.scss", ...cssDateRangePickerPath],
    licenseewinloss: ["./www/css/source/pages/report/licenseewinloss.scss", ...cssDateRangePickerPath]
};

var scriptPaths = {
    build: "www/js/build",

    language: ["common/languages/ViewResources.js", "common/languages/ViewResources.zh-CN.js", "common/languages/ViewResources.zh-TW.js", "common/languages/ViewResources.zh-Hans.js", "common/languages/ViewResources.ko-KR.js"],
    home: ["./www/js/source/lib/navbar.js", "./www/js/source/lib/sidebar.js", "malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js", "./www/js/source/pages/home/main.js"],
    signin: ["./www/js/source/pages/signin/main.js"],

    livereport: "./www/js/source/pages/report/livereport/main-livereport.js",
    livereportmatchlist: "./www/js/source/pages/report/livereport/main-matchlist.js",
    licenseewinloss: "./www/js/source/pages/report/licenseewinloss/main.js"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};