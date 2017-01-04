var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var jqgridPath = ["./www/css/source/extensions/jqgrid-ex.scss"];
var cssReportCommon = ["./www/css/source/extensions/report-ex.scss", "./www/css/source/components/flatpopup/flatpopup.scss"];
var cssBetlistCommon = [
    ...cssReportCommon, ...jqgridPath,
    "./www/css/source/components/betlist/betlist.scss", "./www/css/source/components/betlist/Microgaming.css",
    "./www/css/source/components/betlist/AGCasino.css", "./www/css/source/components/betlist/Allbet.css"
];

var cssPaths = {
    build: "www/css/build",

    home: ["./www/css/source/extensions/home.scss"],
    main: ["./www/css/source/extensions/main-ex.scss"],
    error: ["./www/css/source/pages/utility/error.scss"],

    reportjqgridlayout: [...cssReportCommon, ...jqgridPath, "./www/css/source/extensions/multiselect-ex.scss"],
    betlistLayout: [...cssBetlistCommon],
    betlistSimpleLayout: [...cssBetlistCommon, "./www/css/source/extensions/popup-ex.scss"],

    totalbetlayout: [...cssReportCommon, "./www/css/source/pages/totalbet/totalbet.scss"],

    colossusbetdetail: ["./www/css/source/components/betlist/colossusbetdetail.scss"],
    systemparlaydetail: [...cssReportCommon, "./www/css/source/components/betlist/systemparlaydetail.scss"],

    betlistResult: ["./www/css/source/components/betlist/result/betlist.result.scss"],

    login: ["./www/css/source/extensions/login-ex.scss"],
    livereport: [...cssReportCommon, "./www/css/source/pages/report/livereport.scss"],
    licenseewinloss: ["./www/css/source/pages/report/licenseewinloss.scss"],

    ahforecast: [...cssReportCommon, "./www/css/source/pages/forecast/forecast.ah.scss"],
    mixparlay: ["./www/css/source/pages/totalbet/mixparlay.scss"],
    forecastonextwo: [...cssReportCommon, "./www/css/source/pages/forecast/forecastonextwo.scss"],
    newforecast: [...cssReportCommon, "./www/css/source/pages/forecast/newforecast.scss"]
};

var scriptPaths = {
    build: "www/js/build",

    language: ["common/languages/ViewResources.js", "common/languages/ViewResources.zh-CN.js", "common/languages/ViewResources.zh-TW.js", "common/languages/ViewResources.zh-Hans.js", "common/languages/ViewResources.ko-KR.js"],
    home: ["./www/js/source/components/navbar.js", "./www/js/source/components/sidebar.js", "malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js", "./www/js/source/pages/home/main.js"],
    signin: ["./www/js/source/pages/signin/main.js"],

    livereport: "./www/js/source/pages/report/livereport/main-livereport.js",
    livereportmatchlist: "./www/js/source/pages/report/livereport/main-matchlist.js",
    licenseewinloss: "./www/js/source/pages/report/licenseewinloss/main.js",
    betlist: "./www/js/source/pages/betlist/betlist.js",
    betlistsimple: "./www/js/source/pages/betlist/main-simple.js",
    betlistrunning: "./www/js/source/pages/betlist/main-betlistrunning.js",
    betlistrunningmultiplebettypes: "./www/js/source/pages/betlist/main-betlistrunning.multiplebettypes.js",
    betlistcorrectscore: "./www/js/source/pages/betlist/main-betlistcorrectscore.js",

    handicapoverunder: "./www/js/source/pages/totalbet/handicapoverunder/main.js",
    ahforecast: "./www/js/source/pages/forecast/forecast.ah.main.js",
    moneyline: "./www/js/source/pages/totalbet/moneyline/main.js",
    oddevendrawnodraw: "./www/js/source/pages/totalbet/oddevendrawnodraw/main.js",
    totalgoal: "./www/js/source/pages/totalbet/totalgoal/main.js",
    outright: "./www/js/source/pages/totalbet/outright/main.js",
    halftimefulltime: "./www/js/source/pages/totalbet/halftimefulltime/main.js",
    firstgoallastgoal: "./www/js/source/pages/totalbet/firstgoallastgoal/main.js",
    homedrawawaynobet: "./www/js/source/pages/totalbet/homedrawawaynobet/main.js",
    forecastonextwo: "./www/js/source/pages/forecast/forecast.onextwo.main.js",
    newforecasthdpou: "./www/js/source/pages/forecast/forecast.newforecasthdpou.main.js"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};