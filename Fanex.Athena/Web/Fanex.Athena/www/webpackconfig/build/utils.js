const webpack = require('webpack')
const ExtractTextPlugin = require('extract-text-webpack-plugin')


exports.setDefinePlugin = function (key, value) {
    const env = {};
    env[key] = JSON.stringify(value);

    return {
        plugins: [
          new webpack.DefinePlugin(env)
        ]
    };
}

exports.minify = function () {
    return {
        plugins: [
            new webpack.optimize.DedupePlugin(),
            new webpack.optimize.UglifyJsPlugin({
                beautify: false,
                comments: false,
                compress: {
                    warnings: false,
                    drop_console: true
                }
            })
        ]
    };
}

exports.extractCSS = function (isProduction) {
    var queryString = isProduction ? "" : "&sourceMap";
    return {
        module: {
            loaders: [
                {
                    test: /\.css$/,
                    loader: ExtractTextPlugin.extract("style", "css?-url" + queryString)
                },
                {
                    test: /\.scss$/,
                    loader: ExtractTextPlugin.extract("style", "css?-url" + queryString + "!sass")
                },
            ]
        },
        plugins: [
            new ExtractTextPlugin('[name].css')
        ]
    };
}