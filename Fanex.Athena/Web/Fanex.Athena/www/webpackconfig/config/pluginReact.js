var webpack = require('webpack');
var CleanWebpackPlugin = require('clean-webpack-plugin');
var pathConfig =require('./pathReact');
var cssRootPath = pathConfig.cssPaths.build;

function generatePlugins(environment) {
    var commonPlugins = [
        new CleanWebpackPlugin([cssRootPath], {
            root: process.cwd(),
            exclude: ['fonts', 'images', 'dev', 'pro']
        }),

         new webpack.optimize.CommonsChunkPlugin({
             name: ["excel", "reactpages"],
            filename: "www/js/build/" + environment + "/reactpages/vendors/[name].chunk.js",
            minChunks: Infinity
         })
    ];

    return commonPlugins;
}

module.exports = generatePlugins;