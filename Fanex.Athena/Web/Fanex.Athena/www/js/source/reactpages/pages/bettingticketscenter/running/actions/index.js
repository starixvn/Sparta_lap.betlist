import * as dataHandlerAction from './dataHandlerAction.js'
import * as filterHandlerAction from './filterHandlerAction.js'
import * as productAction from '@jsroot/actions/productAction'
import * as currencyAction from '@jsroot/actions/currencyAction'
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as loaderAction from '@jsroot/actions/loaderAction'
import * as dateRangeAction from '@jsroot/actions/dateRangeAction'

export const ActionCreators = Object.assign({},
    dataHandlerAction,
    filterHandlerAction,
    productAction,
    currencyAction,
    baseCurrencyAction,
    loaderAction,
    dateRangeAction
);