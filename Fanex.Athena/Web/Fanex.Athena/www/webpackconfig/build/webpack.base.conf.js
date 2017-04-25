var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var loader = require('../config/loader');
var pluginGenerator = require('../config/plugin');
var exposeLoader = require('expose-loader');
var utils = require('./utils');

function generateConfig(environment) {
    var commonConfigs = {
        resolve: {
            alias: alias
        },
        entry: {
            common: ["jquery", "vendor-materialize", "common/site", "common/querystring"],

            [`${pathConfig.scriptPaths.build}/${environment}/language`]: pathConfig.scriptPaths.language,
            [`${pathConfig.scriptPaths.build}/${environment}/pages/home/home`]: pathConfig.scriptPaths.home,
            [`${pathConfig.scriptPaths.build}/${environment}/pages/signin/signin`]: pathConfig.scriptPaths.signin,
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/livereport`]: [pathConfig.scriptPaths.livereport],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/livereportmatchlist`]: [pathConfig.scriptPaths.livereportmatchlist],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/licenseewinloss`]: [pathConfig.scriptPaths.licenseewinloss],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlist`]: [pathConfig.scriptPaths.betlist],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlistsimple`]: [pathConfig.scriptPaths.betlistsimple],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlistrunning`]: [pathConfig.scriptPaths.betlistrunning],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlistrunningmultiplebettypes`]: [pathConfig.scriptPaths.betlistrunningmultiplebettypes],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlistcorrectscore`]: [pathConfig.scriptPaths.betlistcorrectscore],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/handicapoverunder`]: [pathConfig.scriptPaths.handicapoverunder],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/forecast/ahforecast`]: [pathConfig.scriptPaths.ahforecast],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/moneyline`]: [pathConfig.scriptPaths.moneyline],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/oddevendrawnodraw`]: [pathConfig.scriptPaths.oddevendrawnodraw],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/totalgoal`]: [pathConfig.scriptPaths.totalgoal],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/outright`]: [pathConfig.scriptPaths.outright],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/halftimefulltime`]: [pathConfig.scriptPaths.halftimefulltime],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/firstgoallastgoal`]: [pathConfig.scriptPaths.firstgoallastgoal],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/totalbet/homedrawawaynobet`]: [pathConfig.scriptPaths.homedrawawaynobet],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/forecast/forecastonextwo`]: [pathConfig.scriptPaths.forecastonextwo],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/forecast/forecasthdpou`]: [pathConfig.scriptPaths.forecasthdpou],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/forecast/newforecasthdpou`]: [pathConfig.scriptPaths.newforecasthdpou],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlistcancelledbets`]: [pathConfig.scriptPaths.betlistcancelledbets],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/betlist/betlistdaily`]: [pathConfig.scriptPaths.betlistdaily],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/settledmatchcount`]: [pathConfig.scriptPaths.settledmatchcount],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/balance`]: [pathConfig.scriptPaths.balance],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/weeklylivematch`]: [pathConfig.scriptPaths.weeklylivematch],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/racingcommission`]: [pathConfig.scriptPaths.racingcommission],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/turnover`]: [pathConfig.scriptPaths.turnover],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/result`]: [pathConfig.scriptPaths.result],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/cashoutwinloss`]: [pathConfig.scriptPaths.cashoutwinloss],
            [`${pathConfig.scriptPaths.build}/${environment}/pages/reports/winlossbyproduct`]: [pathConfig.scriptPaths.winlossbyproduct],
            

            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/reportjqgridlayout`]: pathConfig.cssPaths.reportjqgridlayout,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/livereport`]: pathConfig.cssPaths.livereport,
            [`${pathConfig.cssPaths.build}/${environment}/main`]: pathConfig.cssPaths.main,
            [`${pathConfig.cssPaths.build}/${environment}/pages/utility/error`]: pathConfig.cssPaths.error,
            [`${pathConfig.cssPaths.build}/${environment}/home`]: pathConfig.cssPaths.home,
            [`${pathConfig.cssPaths.build}/${environment}/pages/login/login`]: pathConfig.cssPaths.login,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/licenseewinloss`]: pathConfig.cssPaths.licenseewinloss,

            [`${pathConfig.cssPaths.build}/${environment}/pages/betlist/betlistLayout`]: pathConfig.cssPaths.betlistLayout,
            [`${pathConfig.cssPaths.build}/${environment}/pages/betlist/betlistSimpleLayout`]: pathConfig.cssPaths.betlistSimpleLayout,
            [`${pathConfig.cssPaths.build}/${environment}/pages/betlist/betlistResult`]: pathConfig.cssPaths.betlistResult,
            [`${pathConfig.cssPaths.build}/${environment}/pages/betlist/colossusbetdetail`]: pathConfig.cssPaths.colossusbetdetail,
            [`${pathConfig.cssPaths.build}/${environment}/pages/betlist/systemparlaydetail`]: pathConfig.cssPaths.systemparlaydetail,

            [`${pathConfig.cssPaths.build}/${environment}/pages/totalbet/totalbetlayout`]: pathConfig.cssPaths.totalbetlayout,
            [`${pathConfig.cssPaths.build}/${environment}/pages/forecast/ahforecast`]: pathConfig.cssPaths.ahforecast,
            [`${pathConfig.cssPaths.build}/${environment}/pages/forecast/forecastonextwo`]: pathConfig.cssPaths.forecastonextwo,
            [`${pathConfig.cssPaths.build}/${environment}/pages/totalbet/mixparlay`]: pathConfig.cssPaths.mixparlay,
            [`${pathConfig.cssPaths.build}/${environment}/pages/forecast/newforecast`]: pathConfig.cssPaths.newforecast,
            [`${pathConfig.cssPaths.build }/${environment}/pages/betlist/betlistdaily`]: pathConfig.cssPaths.betlistdaily,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/settledmatchcount`]: pathConfig.cssPaths.settledmatchcount,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/balance`]: pathConfig.cssPaths.balance,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/weeklylivematch`]: pathConfig.cssPaths.weeklylivematch,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/racingcommission`]: pathConfig.cssPaths.racingcommission,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/turnover`]: pathConfig.cssPaths.turnover,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/result`]: pathConfig.cssPaths.result,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/cashoutwinloss`]: pathConfig.cssPaths.cashoutwinloss,
            [`${pathConfig.cssPaths.build}/${environment}/pages/betlist/cashouthistory`]: pathConfig.cssPaths.cashouthistory,
            [`${pathConfig.cssPaths.build}/${environment}/pages/reports/winlossbyproduct`]: pathConfig.cssPaths.winlossbyproduct
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js"
        },
        module: {
            loaders: loader.commonLoaders
        },
        plugins: pluginGenerator(environment)
    };

    return commonConfigs;
}

module.exports = generateConfig;