var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "www/css/build",
    layout:"./www/css/source/reactpages/main.scss",
    
};

var scriptPaths = {
    build: "www/js/build/",

   
    /*React page*/
    operatorperformance: "./www/js/source/reactpages/pages/operatorperformance/components/index.jsx"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};