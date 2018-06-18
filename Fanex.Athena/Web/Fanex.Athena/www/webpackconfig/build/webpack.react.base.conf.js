var path = require('path');
var pathConfig = require('../config/pathReact');
var pluginGenerator = require('../config/pluginReact');
var utils = require('./utils');

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}/reactpages`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}/reactpages`;
    var commonConfigs = {
        resolve: {
            extensions: ['.js', '.jsx']
        },
        entry: {
            /*js*/

            /*  */
            reactpages: ["react", "react-redux", "redux", "react-dom", "redux-thunk", "redux-logger", "axios",
                "core-js/fn/object/assign", "core-js/fn/promise", "core-js/fn/array/find", "core-js/fn/array/find-index", "moment"
            ],

            [`${jsPath}/lib/langHelper`]: pathConfig.scriptPaths.languageResource,

            [`${jsPath}/subsidiaryperformance/subsidiaryperformance`]: pathConfig.scriptPaths.subsidiaryperformance,
            [`${jsPath}/notification/notification`]: pathConfig.scriptPaths.notification,
            [`${jsPath}/report/licenseewlbytimezone`]: pathConfig.scriptPaths.licenseewlbytimezone,
            [`${jsPath}/exchangewl/exchangewl`]: pathConfig.scriptPaths.exchangewl,
            [`${jsPath}/report/speciallicenseewl`]: pathConfig.scriptPaths.speciallicenseewl,
            [`${jsPath}/bettingticketscenter/multicustomer`]: pathConfig.scriptPaths.bettingticketscenter_multicustomer,

            /*css*/
            [`${cssPath}/vendor`]: pathConfig.cssPaths.layout,
            [`${cssPath}/subsidiaryperformance/index`]: pathConfig.cssPaths.subsidiaryperformance,
            [`${cssPath}/licenseewlbytimezone/index`]: pathConfig.cssPaths.licenseewlbytimezone,
            [`${cssPath}/notification/index`]: pathConfig.cssPaths.notification,
            [`${cssPath}/exchangewl/index`]: pathConfig.cssPaths.exchangewl,
            [`${cssPath}/speciallicenseewl/index`]: pathConfig.cssPaths.speciallicenseewl
            [`${cssPath}/bettingticketscenter/index`]: pathConfig.cssPaths.bettingticketscenter
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js",
            chunkFilename: jsPath + '/dynamic-import/[name].chunk.js',
        },
        module: {
            rules: [{
                test: /\.(js|jsx)$/,
                loader: ["babel-loader"],
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