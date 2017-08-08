var merge = require('webpack-merge');
var utils = require('./utils');
var baseWebpackConfigGenerator = require('./webpack.react.base.conf');

var baseConfigs = baseWebpackConfigGenerator("dev");

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'development'),
    {
        devtool: 'source-map'
    },

    utils.extractCSS(false)
);