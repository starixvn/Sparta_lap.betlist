var path = require('path');
var pathConfig = require('../config/pathReact');
var pluginGenerator = require('../config/pluginReact');
function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}/reactpages`;
    
    var commonConfigs = {
        entry: {
            reactVendorJs: ["react", "react-redux","redux", "react-dom", "redux-thunk","redux-logger"],
            [`${jsPath}/operatorperformance/operatorperformance`]: pathConfig.scriptPaths.operatorperformance
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
        plugins: pluginGenerator(environment),
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