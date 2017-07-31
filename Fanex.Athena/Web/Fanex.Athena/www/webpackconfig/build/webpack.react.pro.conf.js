var merge = require('webpack-merge');
var utils = require('./utils');
var baseWebpackConfigGenerator = require('./webpack.react.base.conf');

var baseConfigs = baseWebpackConfigGenerator("pro");

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'production'),
    utils.minify()
);