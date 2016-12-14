var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var loader = require('../config/loader');
var plugin = require('../config/plugin');
var exposeLoader = require('expose-loader');
var utils = require('./utils');

var commonConfigs = {
    resolve: {
        alias: alias
    },
    entry: {
        "common": ["jquery", "vendor-materialize", "common/site", "common/querystring"],

        [pathConfig.scriptPaths.build + "/language"]: pathConfig.scriptPaths.language,
        [pathConfig.scriptPaths.build + "/pages/home/home"]: pathConfig.scriptPaths.home,
        [pathConfig.scriptPaths.build + "/pages/signin/signin"]: pathConfig.scriptPaths.signin,
        [pathConfig.scriptPaths.build + "/pages/livereport/livereport"]: [pathConfig.scriptPaths.livereport],
        [pathConfig.scriptPaths.build + "/pages/livereport/livereportmatchlist"]: [pathConfig.scriptPaths.livereportmatchlist],
        [pathConfig.scriptPaths.build + "/pages/licenseewinloss/licenseewinloss"]: [pathConfig.scriptPaths.licenseewinloss],
        [pathConfig.scriptPaths.build + "/pages/betlist/betlist"]: [pathConfig.scriptPaths.betlist],
        [pathConfig.scriptPaths.build + "/pages/betlist/betlistsimple"]: [pathConfig.scriptPaths.betlistsimple],
        [pathConfig.scriptPaths.build + "/pages/betlist/betlistrunning"]: [pathConfig.scriptPaths.betlistrunning],
        [pathConfig.scriptPaths.build + "/pages/handicapoverunder/handicapoverunder"]: [pathConfig.scriptPaths.handicapoverunder],
        [pathConfig.scriptPaths.build + "/pages/forecast/ahforecast"]: [pathConfig.scriptPaths.ahforecast],

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
        [pathConfig.cssPaths.build + "/pages/forecast/ahforecast"]: pathConfig.cssPaths.ahforecast
    },
    output: {
        path: pathConfig.projectRoot,
        filename: "[name].chunk.js"
    },
    module: {
        loaders: loader.commonLoaders
    },
    plugins: plugin.commonPlugins
};

module.exports = commonConfigs;