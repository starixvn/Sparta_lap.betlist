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
            new webpack.optimize.UglifyJsPlugin({
                beautify: false,
                comments: false,
                compress: {
                    warnings: false,
                    drop_console: true
                },
                // Mangling specific options
                mangle: {
                    // Don't mangle $
                    except: ['$', 'webpackJsonp'],
                    // Don't care about IE8
                    screw_ie8: true,
                    // Don't mangle function names
                    keep_fnames: true
                }
            })
        ]
    };
}

exports.extractCSS = function () {
    return {
        module: {
            loaders: [
                {
                    test: /\.css$/,
                    loader: ExtractTextPlugin.extract("style", "css?-url")
                },
                {
                    test: /\.scss$/,
                    loader: ExtractTextPlugin.extract("style", "css?-url!sass")
                }
            ]
        },
        plugins: [
            new ExtractTextPlugin('[name].css')
        ]
    };
}