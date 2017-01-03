var webpack = require('webpack');
var merge = require('webpack-merge');
var utils = require('./utils');
var baseWebpackConfigGenerator = require('./webpack.base.conf');

var baseConfigs = baseWebpackConfigGenerator("pro");

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'production'),
    utils.extractCSS(true),
    utils.minify()
);