import * as dataHandlerAction from './dataHandlerAction.js'
import * as productAction from '@jsroot/actions/productAction'
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as usernameAction from '@jsroot/actions/usernameAction'
import * as statusAction from '@jsroot/actions/statusAction'
import * as platformAction from '@jsroot/actions/platformAction'
import * as liveStatusAction from '@jsroot/actions/liveStatusAction'
import * as loaderAction from '@jsroot/actions/loaderAction'
import * as dateRangeAction from '@jsroot/actions/dateRangeAction'
import * as convertAllAction from '@jsroot/actions/convertAllAction'

export const ActionCreators = Object.assign({},
    dataHandlerAction,
    productAction,
    baseCurrencyAction,
    usernameAction,
    statusAction,
    platformAction,
    liveStatusAction,
    loaderAction,
    dateRangeAction,
    convertAllAction
);