
import * as dataHandlerAction from './dataHandlerAction.js'
import * as productAction from '../../../action/productService'
export const ActionCreators = Object.assign({},
    dataHandlerAction,
    productAction
);