import * as dataHandlerAction from './dataHandlerAction.js'
import * as filterHandlerAction from './filterHandlerAction.js'
import * as currencyAction from '../../../actions/currencyAction'
import * as loaderAction from '../../../actions/loaderAction'
import * as excludeTestAction from '../../../actions/excludeTestAction'

export const ActionCreators = Object.assign({},
    dataHandlerAction,
    filterHandlerAction,
    currencyAction,
    loaderAction,
    excludeTestAction
);