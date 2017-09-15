var path = require('path');
var pathConfig = require('../config/pathReact');
var pluginGenerator = require('../config/pluginReact');
var utils = require('./utils');
var alias = require('../config/aliasReact');

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}/reactpages`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}/reactpages`;
    var commonConfigs = {
        resolve: {
            alias: alias,
            extensions: ['', '.js', '.jsx']
        },
        entry: {
            /*js*/
            reactVendorJs: ["react", "react-redux", "redux", "react-dom", "redux-thunk", "redux-logger", "axios", "xlsx-style",
                "core-js/fn/object/assign", "core-js/fn/promise", "core-js/fn/array/find"
            ],
            [`${jsPath}/subsidiaryperformance/subsidiaryperformance`]: pathConfig.scriptPaths.subsidiaryperformance,
            [`${jsPath}/lib/langHelper`]: pathConfig.scriptPaths.languageResource,
            /*css*/
            [`${cssPath}/vendor`]: pathConfig.cssPaths.layout,
            [`${cssPath}/subsidiaryperformance/index`]: pathConfig.cssPaths.subsidiaryperformance
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js"
        },
        module: {
            loaders: [{
                test: /\.(js|jsx)$/,
                loader: "babel-loader",
                exclude: /node_modules/
            }]
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
        },
        node: {
            fs: 'empty'
        },
        externals: [
            { './jszip': 'jszip' },
            { './ods': 'ods' },
            { './cptable': 'var cptable' }
        ]
    };

    return commonConfigs;
}

module.exports = generateConfig;