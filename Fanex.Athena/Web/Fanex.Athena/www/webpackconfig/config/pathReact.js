var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "www/css/build",
    layout: "./www/css/source/reactpages/main.scss",
    operatorperformance:"./www/css/source/reactpages/pages/operatorperformance/index.scss"
};

var scriptPaths = {
    build: "www/js/build/",

    languageResource: ["./www/js/source/reactpages/lib/langHelper.js",
        "./www/js/source/common/languages/ViewResources.js",
        "./www/js/source/common/languages/ViewResources.ko-KR.js",
        "./www/js/source/common/languages/ViewResources.zh-CN.js",
        "./www/js/source/common/languages/ViewResources.zh-Hans.js",
        "./www/js/source/common/languages/ViewResources.zh-TW.js"
    ],
    /*React page*/
    operatorperformance: "./www/js/source/reactpages/pages/operatorperformance/components/index.jsx"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};