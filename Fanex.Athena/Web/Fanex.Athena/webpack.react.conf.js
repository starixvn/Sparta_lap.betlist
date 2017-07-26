var path = require('path');
var pathConfig = require('./www/webpackconfig/config/path');

var jsPath = `${pathConfig.scriptPaths.build}/dev`;

module.exports = {
    entry: {
        [`${jsPath}/reactsample`]: pathConfig.scriptPaths.reactsample,
    },
    output: {
        path: pathConfig.projectRoot,
        filename: "[name].chunk.js"
    },
    module: {
        loaders: [
            {
                test: /\.(js|jsx)$/,
                loader: "babel-loader",
                exclude: /node_modules/
            }
        ]
    }
}