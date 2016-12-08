var webpack = require('webpack');
var pathConfig = require('../config/path');
var CleanWebpackPlugin = require('clean-webpack-plugin');

var cssRootPath = pathConfig.cssPaths.build;
var commonPlugins = [
	new CleanWebpackPlugin([cssRootPath], {
		root: process.cwd(),
		exclude: ['fonts', 'images', 'pages']
	}),
	new CleanWebpackPlugin([`${cssRootPath}/pages`], {
	    root: process.cwd(),
	    exclude: ['betlist']
	}),
	new CleanWebpackPlugin([`${cssRootPath}/pages/betlist`], {
	    root: process.cwd(),
	    exclude: ['images']
	}),

	new webpack.optimize.CommonsChunkPlugin({
		name: "common",
		filename: "www/js/build/common.chunk.js",
		minChunks: Infinity
	}),
	new webpack.ProvidePlugin({
		$: "jquery",
		jQuery: "jquery",
		jquery: 'jquery',
		'window.$': 'jquery',
		"window.jQuery": 'jquery',
		"root.jQuery": 'jquery'
	})
];

module.exports = {
	commonPlugins: commonPlugins
};