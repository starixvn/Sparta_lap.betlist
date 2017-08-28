import * as dataHandlerAction from './dataHandlerAction.js'
import * as productAction from '../../../actions/productAction'
import * as baseCurrencyAction from '../../../actions/baseCurrencyAction'

export const ActionCreators = Object.assign({},
    dataHandlerAction,
    productAction,
    baseCurrencyAction
);