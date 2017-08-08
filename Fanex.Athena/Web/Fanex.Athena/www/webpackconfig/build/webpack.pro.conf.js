var webpack = require('webpack');
var merge = require('webpack-merge');
var CleanWebpackPlugin = require('clean-webpack-plugin');
var utils = require('./utils');
var pathConfig = require('../config/path');
var baseWebpackConfigGenerator = require('./webpack.base.conf');

var environment = "pro";
var baseConfigs = baseWebpackConfigGenerator(environment);
var cssRootPath = `${pathConfig.cssPaths.build}/${environment}`;

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'production'),
    utils.extractCSS(true),
    utils.minify(),
    {
        plugins: [
            new CleanWebpackPlugin([`${pathConfig.scriptPaths.build}/${environment}`], {
                root: process.cwd(),
                exclude: ['reactpages']
            }),
            new CleanWebpackPlugin([`${cssRootPath}/`], {
                root: process.cwd(),
                exclude: ['reactpages']
            }),
            new CleanWebpackPlugin([`${cssRootPath}/pages`], {
                root: process.cwd(),
                exclude: ['betlist']
            }),
            new CleanWebpackPlugin([`${cssRootPath}/pages/betlist`], {
                root: process.cwd(),
                exclude: ['images']
            })
        ]
    }
);