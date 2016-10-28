var webpack = require('webpack')
var pathConfig = require('../config/path')
var CleanWebpackPlugin = require('clean-webpack-plugin')

var commonPlugins = [
    new CleanWebpackPlugin([pathConfig.scriptPaths.build, pathConfig.cssPaths.build], {
        root: process.cwd(),
        exclude: ['fonts', 'images']
    }),
    new webpack.optimize.CommonsChunkPlugin({
        name: "vendors",
        filename: "www/js/build/vendors.chunk.js",
        minChunks: Infinity
    }),
	new webpack.ProvidePlugin({
	    $: "jquery",
	    jQuery: "jquery",
	    jquery: 'jquery',
	    'window.$': 'jquery',
	    "window.jQuery": 'jquery',
	    "root.jQuery": 'jquery',
	})
]

module.exports = {
    commonPlugins: commonPlugins
}