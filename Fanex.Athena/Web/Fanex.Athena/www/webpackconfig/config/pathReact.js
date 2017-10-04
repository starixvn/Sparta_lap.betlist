var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "www/css/build",
    layout: "./www/css/source/reactpages/main.scss",
    subsidiaryperformance: "./www/css/source/reactpages/pages/subsidiaryperformance/index.scss"
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
    subsidiaryperformance: "./www/js/source/reactpages/pages/subsidiaryperformance/components/index.jsx"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};