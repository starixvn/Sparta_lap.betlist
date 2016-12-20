var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var loader = require('../config/loader');
var pluginGenerator = require('../config/plugin');
var exposeLoader = require('expose-loader');
var utils = require('./utils');

function generateConfig(environment) {
    var commonConfigs = {
        resolve: {
            alias: alias
        },
        entry: {
            common: ["jquery", "vendor-materialize", "common/site", "common/querystring"],

            [pathConfig.scriptPaths.build + "/" +environment + "/language"]: pathConfig.scriptPaths.language,
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/home/home"]: pathConfig.scriptPaths.home,
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/signin/signin"]: pathConfig.scriptPaths.signin,
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/livereport/livereport"]: [pathConfig.scriptPaths.livereport],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/livereport/livereportmatchlist"]: [pathConfig.scriptPaths.livereportmatchlist],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/licenseewinloss/licenseewinloss"]: [pathConfig.scriptPaths.licenseewinloss],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/betlist/betlist"]: [pathConfig.scriptPaths.betlist],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/betlist/betlistsimple"]: [pathConfig.scriptPaths.betlistsimple],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/betlist/betlistrunning"]: [pathConfig.scriptPaths.betlistrunning],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/betlist/betlistrunningmultiplebettypes"]: [pathConfig.scriptPaths.betlistrunningmultiplebettypes],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/betlist/betlistcorrectscore"]: [pathConfig.scriptPaths.betlistcorrectscore],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/handicapoverunder"]: [pathConfig.scriptPaths.handicapoverunder],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/forecast/ahforecast"]: [pathConfig.scriptPaths.ahforecast],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/moneyline"]: [pathConfig.scriptPaths.moneyline],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/oddevendrawnodraw"]: [pathConfig.scriptPaths.oddevendrawnodraw],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/totalgoal"]: [pathConfig.scriptPaths.totalgoal],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/outright"]: [pathConfig.scriptPaths.outright],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/halftimefulltime"]: [pathConfig.scriptPaths.halftimefulltime],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/firstgoallastgoal"]: [pathConfig.scriptPaths.firstgoallastgoal],
            [pathConfig.scriptPaths.build + "/" +environment + "/pages/totalbet/homedrawawaynobet"]: [pathConfig.scriptPaths.homedrawawaynobet],

            [pathConfig.cssPaths.build + "/pages/reports/reportjqgridlayout"]: pathConfig.cssPaths.reportjqgridlayout,
            [pathConfig.cssPaths.build + "/pages/reports/livereport"]: pathConfig.cssPaths.livereport,
            [pathConfig.cssPaths.build + "/main"]: pathConfig.cssPaths.main,
            [pathConfig.cssPaths.build + "/pages/utility/error"]: pathConfig.cssPaths.error,
            [pathConfig.cssPaths.build + "/pages/home/home"]: pathConfig.cssPaths.home,
            [pathConfig.cssPaths.build + "/pages/login/login"]: pathConfig.cssPaths.login,
            [pathConfig.cssPaths.build + "/pages/reports/licenseewinloss"]: pathConfig.cssPaths.licenseewinloss,

            [pathConfig.cssPaths.build + "/pages/betlist/betlistLayout"]: pathConfig.cssPaths.betlistLayout,
            [pathConfig.cssPaths.build + "/pages/betlist/betlistSimpleLayout"]: pathConfig.cssPaths.betlistSimpleLayout,
            [pathConfig.cssPaths.build + "/pages/betlist/betlistResult"]: pathConfig.cssPaths.betlistResult,
            [pathConfig.cssPaths.build + "/pages/betlist/colossusbetdetail"]: pathConfig.cssPaths.colossusbetdetail,
            [pathConfig.cssPaths.build + "/pages/betlist/systemparlaydetail"]: pathConfig.cssPaths.systemparlaydetail,

            [pathConfig.cssPaths.build + "/pages/totalbet/totalbetlayout"]: pathConfig.cssPaths.totalbetlayout,
            [pathConfig.cssPaths.build + "/pages/forecast/ahforecast"]: pathConfig.cssPaths.ahforecast,
            [pathConfig.cssPaths.build + "/pages/totalbet/mixparlay"]: pathConfig.cssPaths.mixparlay
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js"
        },
        module: {
            loaders: loader.commonLoaders
        },
        plugins: pluginGenerator(environment)
    };

    return commonConfigs;
}

module.exports = generateConfig;