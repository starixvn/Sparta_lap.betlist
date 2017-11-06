import { combineReducers } from 'redux'

import * as loaderReducer from '../../../reducers/loaderReducer'
import * as xsrfTokenReducer from '../../../reducers/xsrfTokenReducer'

import productReducer from './products'

const products = combineReducers(productReducer);

export default combineReducers(Object.assign({}, {
    products
}, loaderReducer, xsrfTokenReducer));