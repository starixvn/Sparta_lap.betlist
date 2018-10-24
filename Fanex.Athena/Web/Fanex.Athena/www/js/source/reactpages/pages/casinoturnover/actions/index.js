import * as dateRangeAction from '@jsroot/actions/dateRangeAction'
import * as currencyAction from '@jsroot/actions/currencyAction'
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as productAction from '@jsroot/actions/productAction'
import * as statusAction from '@jsroot/actions/statusAction'
import * as convertAllAction from '@jsroot/actions/convertAllAction'
import * as usernameAction from '@jsroot/actions/usernameAction'
import * as loaderAction from '@jsroot/actions/loaderAction'
import * as reportTitleAction from '@jsroot/actions/reportTitleAction'
import * as filterHandlerAction from './filterHandlerAction'

export const ActionCreators = Object.assign({},
    dateRangeAction,
    currencyAction,
    baseCurrencyAction,
    productAction,
    statusAction,
    convertAllAction,
    usernameAction,
    loaderAction,
    reportTitleAction,
    filterHandlerAction
);