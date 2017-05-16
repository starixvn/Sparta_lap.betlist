var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var loader = require('../config/loader');
var pluginGenerator = require('../config/plugin');
var exposeLoader = require('expose-loader');
var utils = require('./utils');

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}`;

    var commonConfigs = {
        resolve: {
            alias: alias
        },
        entry: {
            common: ["jquery", "vendor-materialize", "common/site", "common/querystring"],

            [`${jsPath}/language`]: pathConfig.scriptPaths.language,
            [`${jsPath}/pages/home/home`]: pathConfig.scriptPaths.home,
            [`${jsPath}/pages/signin/signin`]: pathConfig.scriptPaths.signin,
            [`${jsPath}/pages/reports/livereport`]: [pathConfig.scriptPaths.livereport],
            [`${jsPath}/pages/reports/livereportmatchlist`]: [pathConfig.scriptPaths.livereportmatchlist],
            [`${jsPath}/pages/reports/licenseewinloss`]: [pathConfig.scriptPaths.licenseewinloss],
            [`${jsPath}/pages/betlist/betlist`]: [pathConfig.scriptPaths.betlist],
            [`${jsPath}/pages/betlist/betlistsimple`]: [pathConfig.scriptPaths.betlistsimple],
            [`${jsPath}/pages/betlist/betlistrunning`]: [pathConfig.scriptPaths.betlistrunning],
            [`${jsPath}/pages/betlist/betlistrunningmultiplebettypes`]: [pathConfig.scriptPaths.betlistrunningmultiplebettypes],
            [`${jsPath}/pages/betlist/betlistcorrectscore`]: [pathConfig.scriptPaths.betlistcorrectscore],
            [`${jsPath}/pages/totalbet/handicapoverunder`]: [pathConfig.scriptPaths.handicapoverunder],
            [`${jsPath}/pages/forecast/ahforecast`]: [pathConfig.scriptPaths.ahforecast],
            [`${jsPath}/pages/totalbet/moneyline`]: [pathConfig.scriptPaths.moneyline],
            [`${jsPath}/pages/totalbet/oddevendrawnodraw`]: [pathConfig.scriptPaths.oddevendrawnodraw],
            [`${jsPath}/pages/totalbet/totalgoal`]: [pathConfig.scriptPaths.totalgoal],
            [`${jsPath}/pages/totalbet/outright`]: [pathConfig.scriptPaths.outright],
            [`${jsPath}/pages/totalbet/halftimefulltime`]: [pathConfig.scriptPaths.halftimefulltime],
            [`${jsPath}/pages/totalbet/firstgoallastgoal`]: [pathConfig.scriptPaths.firstgoallastgoal],
            [`${jsPath}/pages/totalbet/homedrawawaynobet`]: [pathConfig.scriptPaths.homedrawawaynobet],
            [`${jsPath}/pages/forecast/forecastonextwo`]: [pathConfig.scriptPaths.forecastonextwo],
            [`${jsPath}/pages/forecast/forecasthdpou`]: [pathConfig.scriptPaths.forecasthdpou],
            [`${jsPath}/pages/forecast/newforecasthdpou`]: [pathConfig.scriptPaths.newforecasthdpou],
            [`${jsPath}/pages/betlist/betlistcancelledbets`]: [pathConfig.scriptPaths.betlistcancelledbets],
            [`${jsPath}/pages/betlist/betlistdaily`]: [pathConfig.scriptPaths.betlistdaily],
            [`${jsPath}/pages/reports/settledmatchcount`]: [pathConfig.scriptPaths.settledmatchcount],
            [`${jsPath}/pages/reports/balance`]: [pathConfig.scriptPaths.balance],
            [`${jsPath}/pages/reports/weeklylivematch`]: [pathConfig.scriptPaths.weeklylivematch],
            [`${jsPath}/pages/reports/racingcommission`]: [pathConfig.scriptPaths.racingcommission],
            [`${jsPath}/pages/reports/turnover`]: [pathConfig.scriptPaths.turnover],
            [`${jsPath}/pages/reports/result`]: [pathConfig.scriptPaths.result],
            [`${jsPath}/pages/reports/cashoutwinloss`]: [pathConfig.scriptPaths.cashoutwinloss],
            [`${jsPath}/pages/reports/winlossbyproduct`]: [pathConfig.scriptPaths.winlossbyproduct],
            [`${jsPath}/pages/reports/dailyracingwinloss`]: [pathConfig.scriptPaths.dailyracingwinloss],
            [`${jsPath}/pages/reports/winlossbyleague`]: [pathConfig.scriptPaths.winlossbyleague],
            [`${jsPath}/pages/reports/matchwinloss`]: [pathConfig.scriptPaths.matchwinloss],
            [`${jsPath}/pages/reports/platformwinloss`]: [pathConfig.scriptPaths.platformwinloss],
            [`${jsPath}/pages/reports/winlossbybettype`]: [pathConfig.scriptPaths.winlossbybettype],
            [`${jsPath}/pages/betlist/lastbetmonitoring`]: [pathConfig.scriptPaths.lastbetmonitoring],

            [`${cssPath}/pages/reports/reportjqgridlayout`]: pathConfig.cssPaths.reportjqgridlayout,
            [`${cssPath}/pages/reports/livereport`]: pathConfig.cssPaths.livereport,
            [`${cssPath}/main`]: pathConfig.cssPaths.main,
            [`${cssPath}/pages/utility/error`]: pathConfig.cssPaths.error,
            [`${cssPath}/home`]: pathConfig.cssPaths.home,
            [`${cssPath}/pages/login/login`]: pathConfig.cssPaths.login,
            [`${cssPath}/pages/reports/licenseewinloss`]: pathConfig.cssPaths.licenseewinloss,

            [`${cssPath}/pages/betlist/betlistLayout`]: pathConfig.cssPaths.betlistLayout,
            [`${cssPath}/pages/betlist/betlistSimpleLayout`]: pathConfig.cssPaths.betlistSimpleLayout,
            [`${cssPath}/pages/betlist/betlistResult`]: pathConfig.cssPaths.betlistResult,
            [`${cssPath}/pages/betlist/colossusbetdetail`]: pathConfig.cssPaths.colossusbetdetail,
            [`${cssPath}/pages/betlist/systemparlaydetail`]: pathConfig.cssPaths.systemparlaydetail,

            [`${cssPath}/pages/totalbet/totalbetlayout`]: pathConfig.cssPaths.totalbetlayout,
            [`${cssPath}/pages/forecast/ahforecast`]: pathConfig.cssPaths.ahforecast,
            [`${cssPath}/pages/forecast/forecastonextwo`]: pathConfig.cssPaths.forecastonextwo,
            [`${cssPath}/pages/totalbet/mixparlay`]: pathConfig.cssPaths.mixparlay,
            [`${cssPath}/pages/forecast/newforecast`]: pathConfig.cssPaths.newforecast,
            [`${cssPath}/pages/betlist/betlistdaily`]: pathConfig.cssPaths.betlistdaily,
            [`${cssPath}/pages/reports/settledmatchcount`]: pathConfig.cssPaths.settledmatchcount,
            [`${cssPath}/pages/reports/balance`]: pathConfig.cssPaths.balance,
            [`${cssPath}/pages/reports/weeklylivematch`]: pathConfig.cssPaths.weeklylivematch,
            [`${cssPath}/pages/reports/racingcommission`]: pathConfig.cssPaths.racingcommission,
            [`${cssPath}/pages/reports/turnover`]: pathConfig.cssPaths.turnover,
            [`${cssPath}/pages/reports/result`]: pathConfig.cssPaths.result,
            [`${cssPath}/pages/reports/cashoutwinloss`]: pathConfig.cssPaths.cashoutwinloss,
            [`${cssPath}/pages/betlist/cashouthistory`]: pathConfig.cssPaths.cashouthistory,
            [`${cssPath}/pages/reports/winlossbyproduct`]: pathConfig.cssPaths.winlossbyproduct,
            [`${cssPath}/pages/reports/dailyracingwinloss`]: pathConfig.cssPaths.dailyracingwinloss,
            [`${cssPath}/pages/reports/winlossbyleague`]: pathConfig.cssPaths.winlossbyleague,
            [`${cssPath}/pages/reports/matchwinloss`]: pathConfig.cssPaths.matchwinloss,
            [`${cssPath}/pages/reports/platformwinloss`]: pathConfig.cssPaths.platformwinloss,
            [`${cssPath}/pages/reports/winlossbybettype`]: pathConfig.cssPaths.winlossbybettype,
            [`${cssPath}/pages/betlist/lastbetmonitoring`]: pathConfig.cssPaths.lastbetmonitoring
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