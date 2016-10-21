var webpack = require('webpack')
var merge = require('webpack-merge')
var utils = require('./utils')
var baseWebpackConfig = require('./webpack.base.conf')

module.exports = merge(baseWebpackConfig, {
    devtool: 'source-map',
    watch: true
    },
    utils.setDefinePlugin('process.env.NODE_ENV', 'development')
)