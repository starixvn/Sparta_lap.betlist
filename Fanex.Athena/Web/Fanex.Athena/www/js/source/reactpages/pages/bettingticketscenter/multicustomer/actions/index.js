import * as gridDataAction from '@jsroot/actions/gridDataAction'
import * as productAction from '@jsroot/actions/productAction'
import * as currencyAction from '@jsroot/actions/currencyAction'
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as statusAction from '@jsroot/actions/statusAction'
import * as platformAction from '@jsroot/actions/platformAction'
import * as liveStatusAction from '@jsroot/actions/liveStatusAction'
import * as loaderAction from '@jsroot/actions/loaderAction'
import * as dateRangeAction from '@jsroot/actions/dateRangeAction'
import * as sportAction from './sportAction'
import * as bettypeAction from './bettypeAction'
import * as leagueAction from './leagueAction'
import * as matchAction from './matchAction'

export const ActionCreators = Object.assign({},
    gridDataAction,
    productAction,
    currencyAction,
    baseCurrencyAction,
    statusAction,
    platformAction,
    liveStatusAction,
    loaderAction,
    dateRangeAction,
    sportAction,
    bettypeAction,
    leagueAction,
    matchAction
);