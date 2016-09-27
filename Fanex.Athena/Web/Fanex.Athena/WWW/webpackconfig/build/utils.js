const webpack = require('webpack')


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