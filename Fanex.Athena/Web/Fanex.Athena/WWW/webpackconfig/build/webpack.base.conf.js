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

        "js/test/pages/layout/layout": pathConfig.scriptPaths.layout,
        "js/test/pages/livereport/livereport": pathConfig.scriptPaths.livereport,

        "css/test/pages/reports/livereport": pathConfig.cssPaths.livereport,
        "css/test/main": pathConfig.cssPaths.main,
        "css/test/vendors/materialize": pathConfig.cssPaths.materialize,
        "css/test/pages/layout/layout": pathConfig.cssPaths.layout,
        "css/test/vendors/daterangepicker": pathConfig.cssPaths.daterangepicker,
        "css/test/pages/login/login": pathConfig.cssPaths.login,
    },
    output: {
        path: path.join(pathConfig.projectRoot, '/www/'),
        filename: "[name].chunk.js"
    },
    module: {
        loaders: loader.commonLoaders
    },
    plugins: plugin.commonPlugins
}

module.exports = merge(commonConfigs, utils.extractCSS())