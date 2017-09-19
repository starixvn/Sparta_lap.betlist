const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');


exports.setDefinePlugin = function (key, value) {
    const env = {};
    env[key] = JSON.stringify(value);

    return {
        plugins: [
          new webpack.DefinePlugin(env)
        ]
    };
};

exports.minify = function () {
    return {
        plugins: [
            new webpack.optimize.UglifyJsPlugin({
                parallel: true,
                beautify: false,
                comments: false,
                compress: {
                    warnings: false,
                    drop_console: true
                }
            })
        ]
    };
};

exports.extractCSS = function (isProduction) {
    return {
        module: {
            rules: [
                {
                    test: /\.css$/,
                    use: ExtractTextPlugin.extract({
                        fallback: 'style-loader',
                        use: 'css-loader'
                    })
                },
                {
                    test: /\.scss$/,
                    use: ExtractTextPlugin.extract({
                        fallback: 'style-loader',
                        use: [
                            {
                                loader: 'css-loader',
                                options: {
                                    minimize: isProduction,
                                    url: false,
                                }
                            },
                          {
                              loader: 'sass-loader',
                              options: {
                                  minimize: isProduction,
                                  url: false,
                              }
                          }]
                    })
                }
            ]
        },
        plugins: [
            new ExtractTextPlugin('[name].css')
        ]
    };
};