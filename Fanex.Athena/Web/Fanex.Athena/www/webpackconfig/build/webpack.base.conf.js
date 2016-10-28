var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var loader = require('../config/loader');
var plugin = require('../config/plugin');

var utils = require('./utils');

var commonConfigs = {
    resolve: {
        alias: alias
    },
    entry: {
        "vendors": ["jquery", "vendor-materialize"],

        [pathConfig.scriptPaths.build + "/language"]: pathConfig.scriptPaths.language,
        [pathConfig.scriptPaths.build + "/pages/home/home"]: pathConfig.scriptPaths.home,
        [pathConfig.scriptPaths.build + "/pages/livereport/livereport"]: [pathConfig.scriptPaths.livereport],
        [pathConfig.scriptPaths.build + "/pages/livereport/livereportmatchlist"]: [pathConfig.scriptPaths.livereportmatchlist],
        [pathConfig.scriptPaths.build + "/pages/licenseewinloss/licenseewinloss"]: [pathConfig.scriptPaths.licenseewinloss],

        [pathConfig.cssPaths.build + "/pages/reports/reportjqgridlayout"]: pathConfig.cssPaths.reportjqgridlayout,
        [pathConfig.cssPaths.build + "/pages/reports/livereport"]: pathConfig.cssPaths.livereport,
        [pathConfig.cssPaths.build + "/main"]: pathConfig.cssPaths.main,
        [pathConfig.cssPaths.build + "/pages/home/home"]: pathConfig.cssPaths.home,
        [pathConfig.cssPaths.build + "/pages/login/login"]: pathConfig.cssPaths.login,
        [pathConfig.cssPaths.build + "/pages/reports/licenseewinloss"]: pathConfig.cssPaths.licenseewinloss
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