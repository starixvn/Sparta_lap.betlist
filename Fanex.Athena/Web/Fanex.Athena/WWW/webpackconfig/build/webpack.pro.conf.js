var webpack = require('webpack')
var merge = require('webpack-merge')
var utils = require('./utils')
var baseWebpackConfig = require('./webpack.base.conf')

module.exports = merge(baseWebpackConfig,
    utils.setDefinePlugin('process.env.NODE_ENV', 'production'),
    utils.minify()
)