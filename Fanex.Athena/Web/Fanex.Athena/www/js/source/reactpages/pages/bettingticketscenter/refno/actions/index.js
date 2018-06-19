import * as dataHandlerAction from './dataHandlerAction.js'
import * as filterHandlerAction from './filterHandlerAction.js'
import * as baseCurrencyAction from '../../../../actions/baseCurrencyAction'
import * as loaderAction from '../../../../actions/loaderAction'

export const ActionCreators = Object.assign({},
    dataHandlerAction,
    filterHandlerAction,
    baseCurrencyAction,
    loaderAction
);