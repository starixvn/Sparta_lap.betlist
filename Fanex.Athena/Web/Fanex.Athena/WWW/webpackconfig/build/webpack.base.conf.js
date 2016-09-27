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
        "vendors": ["jquery", "bootstrap"],

        "js/test/pages/livereport/livereport": pathConfig.scriptPaths.livereport,

        "css/test/pages/reports/livereport": pathConfig.cssPaths.livereport,
        "css/test/site": pathConfig.cssPaths.site,
        "css/test/plugin": pathConfig.cssPaths.plugins,
    },
    output: {
        path: path.join(pathConfig.projectRoot, '/www/'),
        filename: "[name].chunk.js",
        publicPath: "/www/js/build/"
    },
    module: {
        loaders: loader.commonLoaders
    },
    plugins: plugin.commonPlugins
}

module.exports = merge(commonConfigs, utils.extractCSS())