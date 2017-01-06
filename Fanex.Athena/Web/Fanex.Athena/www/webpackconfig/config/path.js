var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "www/css/build",

    home: ["./www/css/source/pages/home/home.scss"],
    main: ["./www/css/source/extensions/main-ex.scss"],
    error: ["./www/css/source/pages/utility/error.scss"],

    reportjqgridlayout: ["./www/css/source/pages/common/reportjqgridlayout.main.scss"],
    betlistLayout: ["./www/css/source/pages/betlist/betlistcommon.main.scss"],
    betlistSimpleLayout: ["./www/css/source/pages/betlist/betlistcommon.main.scss"],

    totalbetlayout: ["./www/css/source/pages/totalbet/totalbet.scss"],

    colossusbetdetail: ["./www/css/source/pages/betlist/colossusbetdetail.scss"],
    systemparlaydetail: ["./www/css/source/pages/betlist/systemparlaydetail.scss"],

    betlistResult: ["./www/css/source/pages/betlist/result/betlist.result.scss"],

    login: ["./www/css/source/pages/login/login-ex.scss"],
    livereport: ["./www/css/source/pages/report/livereport.scss"],
    licenseewinloss: ["./www/css/source/pages/report/licenseewinloss.scss"],

    ahforecast: ["./www/css/source/pages/forecast/forecast.ah.scss"],
    mixparlay: ["./www/css/source/pages/totalbet/mixparlay.scss"],
    forecastonextwo: ["./www/css/source/pages/forecast/forecastonextwo.scss"],
    newforecast: ["./www/css/source/pages/forecast/newforecast.scss"]
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
    forecasthdpou: "./www/js/source/pages/forecast/forecast.handicapoverunder.main.js",
    newforecasthdpou: "./www/js/source/pages/forecast/forecast.newforecasthdpou.main.js"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};