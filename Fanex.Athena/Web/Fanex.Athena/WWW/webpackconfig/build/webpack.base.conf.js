var path = require('path')
var merge = require('webpack-merge')

var pathConfig = require('../config/path')
var alias = require('../config/alias')
var loader = require('../config/loader')
var plugin = require('../config/plugin')

var utils = require('./utils')

var commonConfigs = {
    resolve: {
        alias: alias
    },
    entry: {
        "vendors": ["jquery", "materialize"],

        [pathConfig.scriptPaths.build + "/pages/layout/layout"]: pathConfig.scriptPaths.layout,
        [pathConfig.scriptPaths.build + "/pages/livereport/livereport"]: pathConfig.scriptPaths.livereport,

        [pathConfig.cssPaths.build + "/pages/reports/livereport"]: pathConfig.cssPaths.livereport,
        [pathConfig.cssPaths.build + "/main"]: pathConfig.cssPaths.main,
        [pathConfig.cssPaths.build + "/vendors/materialize"]: pathConfig.cssPaths.materialize,
        [pathConfig.cssPaths.build + "/pages/layout/layout"]: pathConfig.cssPaths.layout,
        [pathConfig.cssPaths.build + "/vendors/daterangepicker"]: pathConfig.cssPaths.daterangepicker,
        [pathConfig.cssPaths.build + "/pages/login/login"]: pathConfig.cssPaths.login,
    },
    output: {
        path: pathConfig.projectRoot,
        filename: "[name].chunk.js"
    },
    module: {
        loaders: loader.commonLoaders
    },
    plugins: plugin.commonPlugins
}

module.exports = merge(commonConfigs, utils.extractCSS())