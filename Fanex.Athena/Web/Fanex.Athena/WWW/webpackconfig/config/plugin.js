var webpack = require('webpack')
var pathConfig = require('../config/path')
var CleanWebpackPlugin = require('clean-webpack-plugin')

var commonPlugins = [
    new CleanWebpackPlugin([pathConfig.scriptPaths.build, pathConfig.cssPaths.build], {
        root: process.cwd(),
        exclude: ['fonts']
    }),
    new webpack.optimize.CommonsChunkPlugin({
        name: "vendors",
        filename: "js/test/vendors.chunk.js",
        minChunks: Infinity
    }),
	new webpack.ProvidePlugin({
	    $: "jquery",
	    jQuery: "jquery"
	})
]

module.exports = {
    commonPlugins: commonPlugins
}