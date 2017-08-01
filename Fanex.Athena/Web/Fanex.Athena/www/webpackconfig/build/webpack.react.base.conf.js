var path = require('path');
var pathConfig = require('../config/path');

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}/react`;

    var commonConfigs = {
        entry: {
            [`${jsPath}/reactsample`]: pathConfig.scriptPaths.reactsample
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
        },
        stats: {
            assets: false,
            colors: true,
            version: false,
            hash: false,
            timings: true,
            chunks: false,
            chunkModules: false,
            children: true
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;