import * as dataHandlerAction from './dataHandlerAction.js'
import * as filterHandlerAction from './filterHandlerAction.js'
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as loaderAction from '@jsroot/actions/loaderAction'
import * as convertAllAction from '@jsroot/actions/convertAllAction'

export const ActionCreators = Object.assign({},
    dataHandlerAction,
    filterHandlerAction,
    baseCurrencyAction,
    loaderAction,
    convertAllAction
);